using Malshinon.entityes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Dals
{
    internal class ValidateDal
    {
        private readonly Dal dal = new Dal();

        public bool SearchExist(string firstName)
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
                    cmd.Parameters.AddWithValue("@first_name", Fname);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string status = reader.GetString("Fname");
                            return status;
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

    }
}   
