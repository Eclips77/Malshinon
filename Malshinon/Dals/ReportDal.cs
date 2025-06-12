using Malshinon.DataBases;
using Malshinon.entityes;
using Malshinon.Tools;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Dals
{
    public class ReportDal
    {
        private readonly DbConnectionMalshinon dbConnection = new DbConnectionMalshinon();
        private readonly PersonDal personDal = new PersonDal();
        private readonly AnalysisDal analysisDal = new AnalysisDal();

        public int GetReportsNumById(string Fname)
        {
            int reportsNum = -1;
            string query = "SELECT num_reports FROM people WHERE first_name = @Fname";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reportsNum = reader.GetInt32("num_reports");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsNumById] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsNumById] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return reportsNum;
        }
        public void SetReportToDb(IntelReport report)
        {
            string query = @"INSERT INTO intelreports(reporter_id, target_id, report_text, timestamp) 
                           VALUES(@reporter_id, @target_id, @report_text, @timestamp)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", report.ReporterId);
                    cmd.Parameters.AddWithValue("@target_id", report.TargetId);
                    cmd.Parameters.AddWithValue("@report_text", CryptoHelper.Encrypt(report.ReportTxt));
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ReportDal.SetReportToDb] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportDal.SetReportToDb] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public List<IntelReport> GetReportsByReporterId(int reporterId)
        {
            var reports = new List<IntelReport>();
            string query = @"SELECT r.*, p1.first_name as reporter_first_name, p1.last_name as reporter_last_name,
                           p2.first_name as target_first_name, p2.last_name as target_last_name
                           FROM intelreports r
                           JOIN people p1 ON r.reporter_id = p1.id
                           JOIN people p2 ON r.target_id = p2.id
                           WHERE r.reporter_id = @reporter_id
                           ORDER BY r.timestamp DESC";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                reports.Add(new IntelReport
                                {
                                    Id = reader.GetInt32("id"),
                                    ReporterId = reader.GetInt32("reporter_id"),
                                    ReporterName = $"{reader.GetString("reporter_first_name")} {reader.GetString("reporter_last_name")}",
                                    TargetId = reader.GetInt32("target_id"),
                                    TargetName = $"{reader.GetString("target_first_name")} {reader.GetString("target_last_name")}",
                                    ReportTxt = CryptoHelper.Decrypt(reader.GetString("report_text")),
                                    Timestamp = reader.GetDateTime("timestamp")
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ReportDal.GetReportsByReporterId] Error reading row: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsByReporterId] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsByReporterId] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return reports;
        }

        public List<IntelReport> GetReportsByTargetId(int targetId)
        {
            var reports = new List<IntelReport>();
            string query = @"SELECT r.*, p1.first_name as reporter_first_name, p1.last_name as reporter_last_name,
                           p2.first_name as target_first_name, p2.last_name as target_last_name
                           FROM intelreports r
                           JOIN people p1 ON r.reporter_id = p1.id
                           JOIN people p2 ON r.target_id = p2.id
                           WHERE r.target_id = @target_id
                           ORDER BY r.timestamp DESC";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                reports.Add(new IntelReport
                                {
                                    Id = reader.GetInt32("id"),
                                    ReporterId = reader.GetInt32("reporter_id"),
                                    ReporterName = $"{reader.GetString("reporter_first_name")} {reader.GetString("reporter_last_name")}",
                                    TargetId = reader.GetInt32("target_id"),
                                    TargetName = $"{reader.GetString("target_first_name")} {reader.GetString("target_last_name")}",
                                    ReportTxt = CryptoHelper.Decrypt(reader.GetString("report_text")),
                                    Timestamp = reader.GetDateTime("timestamp")
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ReportDal.GetReportsByTargetId] Error reading row: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsByTargetId] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportDal.GetReportsByTargetId] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return reports;
        }

        //public void GetAllReports()
        //{
        //    string query = $"SELECT * FROM intelreports";
        //    using (var cmd = new MySqlCommand(query, this._conn))
        //    {

        //    }
        //}
    }
}
