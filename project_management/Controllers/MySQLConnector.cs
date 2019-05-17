using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management
{
    class MySQLConnector
    {
        private MySqlConnection connection { get; set; }
        private static MySQLConnector connector { get; set; }

        private MySQLConnector()
        {
            connection = new MySqlConnection(
                "SERVER=mysql25.unoeuro.com;" +
                "DATABASE=michaelwestergaard_dk_db;" +
                "UID=michaelwest_dk;" +
                "PASSWORD=tim;");
        }

        public static MySQLConnector Instance
        {
           get
           {
                connector = new MySQLConnector();
                return connector;
           }
        }

        public MySqlDataReader GetData(string stmt, Dictionary<string, string> parameters)
        {
            try
            {
                MySqlCommand cmd = CreateCommand(stmt, parameters);
                return cmd.ExecuteReader();
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public bool Execute(string stmt, Dictionary<string, string> parameters)
        {
            try
            {
                int rowsAffected = CreateCommand(stmt, parameters).ExecuteNonQuery();

                if (rowsAffected > 0)
                    return true;

                return false;
            }
            catch (MySqlException e)
            {
                throw;
            }
            finally
            {
                CloseConnections();
            }
        }

        public int Insert(string stmt, Dictionary<string, string> parameters)
        {
            try
            {
                MySqlCommand cmd = CreateCommand(stmt, parameters);
                cmd.ExecuteNonQuery();
                int id = (int) cmd.LastInsertedId;
                CloseConnections();

                return id;
            }
            catch (MySqlException e)
            {
                throw;
            }
}

        private MySqlCommand CreateCommand(string stmt, Dictionary<string, string> parameters)
        {
            try
            {
                CloseConnections();
                connection.Open();

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = stmt;

                if (parameters != null)
                {
                    foreach (var element in parameters)
                    {
                        cmd.Parameters.AddWithValue(element.Key, element.Value);
                    }
                }
                return cmd;
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public void CloseConnections(MySqlDataReader mySqlDataReader = null)
        {
            if (mySqlDataReader != null)
            {
                mySqlDataReader.Dispose();
                mySqlDataReader.Close();
            }

            connection.Close();
            connection.ClearAllPoolsAsync();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
