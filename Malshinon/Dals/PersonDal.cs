using Bogus;
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
        public void InsertPersonToDb(People p)
        {
            string query = @"INSERT INTO people (first_name, last_name, secret_code, type) 
                           VALUES (@first_name, @last_name, @secret_code, @type)";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@first_name", p.FirstName);
                    cmd.Parameters.AddWithValue("@last_name", p.LastName);
                    cmd.Parameters.AddWithValue("@secret_code", p.SecretCode);
                    cmd.Parameters.AddWithValue("@type", p.ManType);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[PersonDal.InsertPersonToDb] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.InsertPersonToDb] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public void SetPersonType(string fname, string status)
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
                Console.WriteLine($"[PersonDal.SetPersonType] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.SetPersonType] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
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
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[PersonDal.GetIdByName] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.GetIdByName] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return idresult;
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
                        try
                        {
                            return new People
                            {
                                id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                SecretCode = reader.GetString("secret_code"),
                                ManType = reader.GetString("type"),
                                NumReports = reader.GetInt32("num_reports"),
                                NumMentions = reader.GetInt32("num_mentions")
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[PersonDal.GetPersonById] Error reading row: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonById] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonById] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        
        public People GetPersonByName(string name)
        {
            string query = @"SELECT * FROM people 
                           WHERE first_name = @name 
                           OR last_name = @name 
                           OR CONCAT(first_name, ' ', last_name) = @name";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        try
                        {
                            return new People
                            {
                                id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                SecretCode = reader.GetString("secret_code"),
                                ManType = reader.GetString("type"),
                                NumReports = reader.GetInt32("num_reports"),
                                NumMentions = reader.GetInt32("num_mentions")
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[PersonDal.GetPersonByName] Error reading row: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonByName] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonByName] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public People GetPersonBySecretCode(string secretCode)
        {
            string query = "SELECT * FROM people WHERE secret_code = @secret_code";
            try
            {
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(query, dbConnection.GetConn()))
                {
                    cmd.Parameters.AddWithValue("@secret_code", secretCode);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        try
                        {
                            return new People
                            {
                                id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                SecretCode = reader.GetString("secret_code"),
                                ManType = reader.GetString("type"),
                                NumReports = reader.GetInt32("num_reports"),
                                NumMentions = reader.GetInt32("num_mentions")
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[PersonDal.GetPersonBySecretCode] Error reading row: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonBySecretCode] SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PersonDal.GetPersonBySecretCode] General error: {ex.Message}");
                throw;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
    }
}

