﻿using MySql.Data.MySqlClient;
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
            connection = new MySqlConnection("SERVER=mysql25.unoeuro.com;DATABASE=michaelwestergaard_dk_db;UID=michaelwest_dk;PASSWORD=peyifimobi27;");
        }

        public static MySQLConnector Instance
        {
           get
           {
                if (connector == null || connector.connection == null)
                    connector = new MySQLConnector();
                return connector;
           }
        }

        public MySqlDataReader GetData(string stmt, Dictionary<string, string> parameters)
        {
           return CreateCommand(stmt, parameters).ExecuteReader();
        }

        public bool Execute(string stmt, Dictionary<string, string> parameters)
        {
            int rowsAffected = CreateCommand(stmt, parameters).ExecuteNonQuery();
            connection.Close();

            if (rowsAffected > 0)
                return true;

            return false;
        }
        

        private MySqlCommand CreateCommand(string stmt, Dictionary<string, string> parameters)
        {
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = stmt;

            foreach (var element in parameters)
            {
                cmd.Parameters.AddWithValue(element.Key, element.Value);
            }

            return cmd;
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}