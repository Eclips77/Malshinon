using Malshinon.DataBases;
using Malshinon.entityes;
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

        public int GetReportsNumById(string Fname)
        {
            int reportsNum = -1;
            string query = $"SELECT num_reports FROM people WHERE first_name = @Fname";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
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
                Console.WriteLine($"sql error in get reports num: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return reportsNum;
        }
        public void SetReportToDb(IntelReport report)
        {
            string query = "INSERT INTO intelreports(reporter_id, target_id, report_text)VALUES(@reporter_id, @target_id, @report_text)"
            ;
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", report.ReporterId);
                    cmd.Parameters.AddWithValue("@target_id", report.TargetId);
                    cmd.Parameters.AddWithValue("@report_text", report.ReportTxt);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"sql error in set report {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            
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
