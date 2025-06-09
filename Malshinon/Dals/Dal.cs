using Malshinon.entityes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Malshinon.Dals
{
    internal class Dal
    {
        private string connStr = "server=localhost;user=root;password=;database=malshinondb";
        private MySqlConnection _conn;

        public Dal()
        {
            try
            {
                OpenConnection();
                CloseConnection();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
        public MySqlConnection OpenConnection()
        {
            if (this._conn == null)
            {
                this._conn = new MySqlConnection(connStr);
            }
            if (this._conn.State != System.Data.ConnectionState.Open)
            {
                this._conn.Open();
                Console.WriteLine("Connection successful.");
                //Console.Clear();
            }
            return this._conn;
        }
        public void CloseConnection()
        {
            if (this._conn != null && this._conn.State == System.Data.ConnectionState.Open)
            {
                this._conn.Close();
                this._conn = null;
            }
        }
        public bool SearchExist(string firstName)
        {
            string query = "SELECT 1 FROM people WHERE first_name = '@firstName' LIMIT 1";
            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, this._conn))
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
                CloseConnection();
            }
        }
        public void setPersonToDb(People person)
        {
            string query = $"INSERT INTO people (first_name,last_name,secret_code,type)" +
                    $"VALUES (@first_name, @last_name, @secret_code, @type)";
            try
            {
                OpenConnection();
                using (var cmd = new MySqlCommand(query, this._conn))
                using (var reader = cmd.ExecuteReader())
                {
                    cmd.Parameters.AddWithValue("@first_name", person.GetFirstName());
                    cmd.Parameters.AddWithValue("@last_name", person.GetLastName());
                    cmd.Parameters.AddWithValue("@secret_code", person.GetSecretCode());
                    cmd.Parameters.AddWithValue("@type", person.GetType());
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"SQL error: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }


    }
}
