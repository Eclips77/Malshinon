using MySql.Data.MySqlClient;
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
                Console.WriteLine($"SQL error: {ex.Message}");
                return false;
            }
            finally
            {
                dal.CloseConnection();
            }
        }
    }
}   
