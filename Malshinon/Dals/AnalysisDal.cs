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

        public void CreateAlert(int reporterId, int targetId, string reason)
        {
            string query = @"INSERT INTO alerts (reporter_id, target_id, reason, timestamp) 
                           VALUES (@reporter_id, @target_id, @reason, @timestamp)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    cmd.Parameters.AddWithValue("@reason", reason);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"[AnalysisDal.CreateAlert] Alert created for target {targetId}, reason: {reason}. Rows affected: {rowsAffected}");
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
            string query = @"SELECT * FROM alerts WHERE timestamp >= DATE_SUB(NOW(), INTERVAL 24 HOUR)";
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
                                    ReporterId = reader.GetInt32("reporter_id"),
                                    TargetId = reader.GetInt32("target_id"),
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

        public void CheckForBurstAlerts(int reporterId, int targetId)
        {
            Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Checking for burst alerts for target ID: {targetId}, triggered by reporter ID: {reporterId}");
            
            try
            {
                dbConnection.OpenConnection();
                using (var conn = dbConnection.GetConn())
                {
                    // First, get the target name
                    string targetName;
                    using (var cmd = new MySqlCommand("SELECT first_name, last_name FROM people WHERE id = @target_id", conn))
                    {
                        cmd.Parameters.AddWithValue("@target_id", targetId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                targetName = $"{reader.GetString("first_name")} {reader.GetString("last_name")}";
                            }
                            else
                            {
                                Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Target ID {targetId} not found.");
                                return;
                            }
                        }
                    }

                    // Then check for burst using ExecuteScalar
                    string query = @"SELECT COUNT(*) as report_count
                                   FROM intelreports 
                                   WHERE target_id = @target_id 
                                   AND timestamp >= DATE_SUB(NOW(), INTERVAL 15 MINUTE)";
                    
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@target_id", targetId);
                        int reportCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Found {reportCount} reports for target {targetId} in last 15 minutes.");
                        
                        if (reportCount >= 3)
                        {
                            string reason = $"Target: {targetName}, Reports in last 15 min: {reportCount}";
                            Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Potential burst detected. Reason: {reason}");

                            // Check if alert already exists
                            bool alertExists = false;
                            using (var checkCmd = new MySqlCommand(
                                @"SELECT COUNT(*) FROM alerts 
                                  WHERE target_id = @target_id 
                                  AND reason = @reason 
                                  AND timestamp >= DATE_SUB(NOW(), INTERVAL 15 MINUTE)", conn))
                            {
                                checkCmd.Parameters.AddWithValue("@target_id", targetId);
                                checkCmd.Parameters.AddWithValue("@reason", reason);
                                alertExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                            }

                            if (!alertExists)
                            {
                                Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Alert does not exist. Creating new alert.");
                                using (var insertCmd = new MySqlCommand(
                                    @"INSERT INTO alerts (reporter_id, target_id, reason, timestamp) 
                                      VALUES (@reporter_id, @target_id, @reason, NOW())", conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@reporter_id", reporterId);
                                    insertCmd.Parameters.AddWithValue("@target_id", targetId);
                                    insertCmd.Parameters.AddWithValue("@reason", reason);
                                    insertCmd.ExecuteNonQuery();
                                    Console.WriteLine("[AnalysisDal.CheckForBurstAlerts] Alert created successfully.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Alert already exists for target {targetId} with reason: {reason}. Skipping creation.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[AnalysisDal.CheckForBurstAlerts] Not enough reports for burst alert (required 3, found {reportCount}).");
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

        private string GetTargetNameById(int targetId)
        {
            Console.WriteLine($"[AnalysisDal.GetTargetNameById] Fetching name for target ID: {targetId}");
            string name = "";
            string query = "SELECT first_name, last_name FROM people WHERE id = @id";
            using (var newDbConnection = new DbConnectionMalshinon())
            {
                try
                {
                    newDbConnection.OpenConnection();
                    using (var cmd = new MySqlCommand(query, newDbConnection.GetConn()))
                    {
                        cmd.Parameters.AddWithValue("@id", targetId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                name = $"{reader.GetString("first_name")} {reader.GetString("last_name")}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AnalysisDal.GetTargetNameById] Error: {ex.Message}");
                }
                finally
                {
                    newDbConnection.CloseConnection();
                }
            }
            return name;
        }

        private bool AlertExists(int targetId, string reason)
        {
            Console.WriteLine($"[AnalysisDal.AlertExists] Checking if alert exists for target ID: {targetId}, reason: {reason}");
            string query = @"SELECT COUNT(*) FROM alerts WHERE target_id = @target_id AND reason = @reason AND timestamp >= DATE_SUB(NOW(), INTERVAL 15 MINUTE)";
            using (var newDbConnection = new DbConnectionMalshinon())
            {
                try
                {
                    newDbConnection.OpenConnection();
                    using (var cmd = new MySqlCommand(query, newDbConnection.GetConn()))
                    {
                        cmd.Parameters.AddWithValue("@target_id", targetId);
                        cmd.Parameters.AddWithValue("@reason", reason);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine($"[AnalysisDal.AlertExists] Found {count} existing alerts for target {targetId} with reason {reason} in last 15 minutes.");
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AnalysisDal.AlertExists] Error: {ex.Message}");
                    return false;
                }
                finally
                {
                    newDbConnection.CloseConnection();
                }
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