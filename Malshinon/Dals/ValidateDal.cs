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
                Console.WriteLine($"[ValidateDal.ExistsInDatabase] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ValidateDal.ExistsInDatabase] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public string GetPersonType(string Fname)
        {
            string query = "SELECT type FROM people WHERE first_name = @Fname";
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
                            try
                            {
                                return reader.GetString("type");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ValidateDal.GetPersonType] Error reading row: {ex.Message}");
                                throw;
                            }
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
                Console.WriteLine($"[ValidateDal.GetPersonType] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ValidateDal.GetPersonType] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

  





    }
}


