using Malshinon.DataBases;
using Malshinon.entityes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Dals
{
    public class ValidateDal
    {
        private readonly DbConnectionMalshinon dbConnection = new DbConnectionMalshinon();

        public bool ExistsInDatabase(string firstName)
        {
            string query = "SELECT * FROM people WHERE first_name = @firstName";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"SQL error in meyhod search exsist: {ex.Message}");
                return false;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public string GetPersonType(string Fname)
        {
            string query = $"SELECT type FROM people WHERE first_name = @Fname";
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
                            string type = reader.GetString("type");
                            return type;
                        }
                        else
                        {
                            return "no person found";
                        }
                    }
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"sql error in check status: {ex.Message}");
                return "";
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

  





    }
}


