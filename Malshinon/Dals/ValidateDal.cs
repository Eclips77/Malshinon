using Bogus;
using Malshinon.entityes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Dals
{
    public  class ValidateDal
    {
        private readonly Dal dal = new Dal();

        public bool ExistsInDatabase(string firstName)
        {
            string query = "SELECT * FROM people WHERE first_name = @firstName";
            try
            {
                dal.OpenConnection();
                using (var cmd = new MySqlCommand(query, dal.GetConn()))
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
                dal.CloseConnection();
            }
        }

        public int GetIdByName(string Fname)
        {
            int idresult = -1;
            string query = "SELECT id FROM people WHERE first_name = @first_name";
            try
            {
                dal.OpenConnection();
                using (var cmd = new MySqlCommand(query, dal.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@first_name", Fname);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idresult = reader.GetInt32("id");
                            return idresult;
                        }
                        else
                        {
                            return idresult; 
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"SQL error method get id by name: {ex.Message}");
                return idresult;
            }
            finally
            {
                dal.CloseConnection();
            }
        }

        public string CheckStatus(string Fname)
        {
            string query = $"SELECT type FROM people WHERE first_name = @Fname";
            try
            {
                dal.OpenConnection();
                using (var cmd = new MySqlCommand(query, dal.GetConn()))
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
                dal.CloseConnection();
            }
        }

        public double GetAvrageLenReports(string Fname)
        {
            int sum = 0;
            int counter = 0;
            double avrage = 0;
            try
            {
                dal.OpenConnection();
                string query = "SELECT i.text " +
                    "FROM IntelReports AS i " +
                    "JOIN people AS p " +
                    "ON i.reporter_id = p.id " +
                    "WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dal.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string text = reader.GetString("text");
                            counter++;
                            sum += text.Length;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception in get avrege: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                dal.CloseConnection();
            }
            if (counter > 0) avrage = sum / counter;
            return avrage;
        }

        public int GetReportsNumById(string Fname)
        {
            int reportsNum = -1;
            string query = $"SELECT num_reports FROM people WHERE first_name = $Fname";
            try
            {
                dal.OpenConnection();
                using (var cmd = new MySqlCommand(query, dal.GetConn()))
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
                dal.CloseConnection();
            }
            return reportsNum;
        }

        public People GetPersonById(int id)
        {
            string query = "SELECT * FROM people WHERE id = @id";
            try
            {
                using (var cmd = new MySqlCommand(query,dal.OpenConnection()))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return new People(
                            reader.GetString("first_name"),
                            reader.GetString("last_name"),
                            reader.GetString("secret_code"),
                            reader.GetString("type"),
                            reader.GetInt32("id"),
                            reader.GetInt32("num_reports"),
                            reader.GetInt32("num_mentions"));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"sql error in get by id: {ex.Message}");
                return null;
            }
            finally
            {
                dal.CloseConnection();
            }
            
            }
        }



    }
  
