using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DataBases
{
    internal class DbConnectionMalshinon
    {
        private string connStr = "server=localhost;user=root;password=;database=malshinondb";
        private MySqlConnection _conn;
        public DbConnectionMalshinon()
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
                //Console.WriteLine("Connection successful.");
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
        public MySqlConnection GetConn() => this._conn;

    }
}
