using Malshinon.DataBases;
using Malshinon.entityes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Malshinon.Dals
{
    public class AnalysisDal
    {
        private readonly DbConnectionMalshinon dbConnection = new DbConnectionMalshinon();

        public void CreateAlert(int targetId, DateTime startTime, DateTime endTime, string reason)
        {
            string query = @"INSERT INTO alerts (target_id, start_time, end_time, reason, timestamp) 
                           VALUES (@target_id, @start_time, @end_time, @reason, @timestamp)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    cmd.Parameters.AddWithValue("@start_time", startTime);
                    cmd.Parameters.AddWithValue("@end_time", endTime);
                    cmd.Parameters.AddWithValue("@reason", reason);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[AnalysisDal.CreateAlert] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisDal.CreateAlert] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public List<Alert> GetActiveAlerts()
        {
            var alerts = new List<Alert>();
            string query = @"SELECT a.*, p.first_name, p.last_name 
                           FROM alerts a 
                           JOIN people p ON a.target_id = p.id 
                           WHERE a.timestamp >= DATE_SUB(NOW(), INTERVAL 24 HOUR)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                alerts.Add(new Alert
                                {
                                    Id = reader.GetInt32("id"),
                                    TargetId = reader.GetInt32("target_id"),
                                    TargetName = $"{reader.GetString("first_name")} {reader.GetString("last_name")}",
                                    StartTime = reader.GetDateTime("start_time"),
                                    EndTime = reader.GetDateTime("end_time"),
                                    Reason = reader.GetString("reason"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[AnalysisDal.GetActiveAlerts] Error reading row: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[AnalysisDal.GetActiveAlerts] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisDal.GetActiveAlerts] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return alerts;
        }

        public void CheckForBurstAlerts(int targetId)
        {
            string query = @"SELECT COUNT(*) as report_count, 
                           MIN(timestamp) as start_time, 
                           MAX(timestamp) as end_time
                           FROM intelreports 
                           WHERE target_id = @target_id 
                           AND timestamp >= DATE_SUB(NOW(), INTERVAL 15 MINUTE)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int reportCount = reader.GetInt32("report_count");
                            if (reportCount >= 3)
                            {
                                DateTime startTime = reader.GetDateTime("start_time");
                                DateTime endTime = reader.GetDateTime("end_time");
                                CreateAlert(targetId, startTime, endTime, "Rapid reports detected");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public List<ReporterStats> GetReporterStats()
        {
            var stats = new List<ReporterStats>();
            string query = @"SELECT p.id, p.first_name, p.last_name, p.num_reports,
                           AVG(LENGTH(r.report_text)) as avg_length
                           FROM people p
                           LEFT JOIN intelreports r ON p.id = r.reporter_id
                           WHERE p.type IN ('reporter', 'both', 'potential_agent')
                           GROUP BY p.id, p.first_name, p.last_name, p.num_reports";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                stats.Add(new ReporterStats
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = $"{reader.GetString("first_name")} {reader.GetString("last_name")}",
                                    ReportCount = reader.GetInt32("num_reports"),
                                    AverageLength = reader.IsDBNull(reader.GetOrdinal("avg_length")) ? 0 : reader.GetDouble("avg_length")
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"AnalysisDal.GetReporterStats Error reading row: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[AnalysisDal.GetReporterStats] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AnalysisDal.GetReporterStats] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return stats;
        }

        public void PromotePotentialAgents()
        {
            try
            {
                var stats = GetReporterStats();
                foreach (var stat in stats)
                {
                    if (stat.ReportCount >= 10 && stat.AverageLength >= 100)
                    {
                        try
                        {
                            dbConnection.OpenConnection();
                            string updateQuery = @"UPDATE people 
                                                SET type = 'potential_agent' 
                                                WHERE id = @id AND type != 'potential_agent'";
                            using (var cmd = new MySqlCommand(updateQuery, dbConnection.GetConn()))
                            {
                                cmd.Parameters.AddWithValue("@id", stat.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"AnalysisDal.PromotePotentialAgents Error promoting agent {stat.Name}: {ex.Message}");
                        }
                        finally
                        {
                            dbConnection.CloseConnection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AnalysisDal.PromotePotentialAgents General error: {ex.Message}");
                throw;
            }
        }

        public List<TargetStats> GetTargetStats()
        {
            var stats = new List<TargetStats>();
            string query = @"SELECT p.id, p.first_name, p.last_name, p.num_mentions,
                           CASE WHEN p.num_mentions >= 20 THEN 1 ELSE 0 END as is_dangerous
                           FROM people p
                           WHERE p.type IN ('target', 'both', 'dangurus')";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                stats.Add(new TargetStats
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = $"{reader.GetString("first_name")} {reader.GetString("last_name")}",
                                    MentionCount = reader.GetInt32("num_mentions"),
                                    IsDangerous = reader.GetInt32("is_dangerous") == 1
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"AnalysisDal.GetTargetStats Error reading row: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"AnalysisDal.GetTargetStats SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AnalysisDal.GetTargetStats General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return stats;
        }
    }
} 