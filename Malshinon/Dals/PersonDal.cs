using Malshinon.DataBases;
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
    public  class PersonDal : ValidateDal
    {
        private readonly DbConnectionMalshinon dbConnection = new DbConnectionMalshinon();
        public void setPersonToDb(string fname,string lname,string scode,string type)
        {
            string query = $"INSERT INTO people (first_name,last_name,secret_code,type)" +
                    $"VALUES (@first_name,@last_name,@secret_code,@type)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query,dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@first_name", fname);
                    cmd.Parameters.AddWithValue("@last_name", lname);
                    cmd.Parameters.AddWithValue("@secret_code", scode);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"SQL error method set person: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public void UpdateStatus(string fname,string status)
        {
            string query = @"UPDATE people SET type = @type WHERE first_name = @fname";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@fname", fname);
                    cmd.Parameters.AddWithValue("@type", status);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"sql error method update status: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"system error: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
                Console.WriteLine("update secssesfull");
            }
        }
        public int GetIdByName(string Fname)
        {
            int idresult = -1;
            string query = "SELECT id FROM people WHERE first_name = @first_name";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
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
                dbConnection.CloseConnection();
            }
        }
        public People GetPersonById(int id)
        {
            string query = "SELECT * FROM people WHERE id = @id";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
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
                dbConnection.CloseConnection();
            }

        }
        


    }
}
