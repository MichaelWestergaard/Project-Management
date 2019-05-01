﻿using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_management;
using MySql.Data.MySqlClient;
using System.Collections;

namespace project_management.DAO
{
    class UserDAO : BaseDAO<User>
    {

        public bool Create(User user)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@email", user.Email }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            if (dataReader.HasRows != true)
            {
                Dictionary<string, string> newUser = new Dictionary<string, string>
                {
                    { "@firstname", user.Firstname },
                    { "@lastname", user.Lastname },
                    { "@password", user.Password },
                    { "@email", user.Email },
                    { "@picture", user.Picture }
                };

                bool response = mySQLConnector.Execute("INSERT INTO users (firstname, lastname, password, email, picture) VALUES (@firstname, @lastname, @password, @email, @picture)", newUser);
                if (response)
                {
                    mySQLConnector.CloseConnection();
                    return true;
                }

            }
            mySQLConnector.CloseConnection();
            return false;
        }
        
        public int CreateUser(User user)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var newUser = new Dictionary<string, string>();
            newUser.Add("@firstName", user.Firstname);
            newUser.Add("@lastName", user.Lastname);
            newUser.Add("@email", user.Email);
            newUser.Add("@password", user.Password);
            newUser.Add("@picture", user.Picture);

            int userID = mySQLConnector.Insert("INSERT INTO users (firstName, lastName, email, password, picture) VALUES (@firstName, @lastName, @email, @password, @picture)", newUser);

            mySQLConnector.CloseConnection();

            return userID;
        }

        public bool Delete(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", ID.ToString() }
            };

            bool reponse = mySQLConnector.Execute("DELETE FROM users WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            if (reponse)
                return true;

            return false;
        }

        public List<User> List()
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            List<User> users = new List<User>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users", null);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    string firstname = dataReader.IsDBNull(1) ? "" : dataReader.GetString("firstname");
                    string lastname = dataReader.IsDBNull(2) ? "" : dataReader.GetString("lastname");
                    string password = dataReader.IsDBNull(3) ? "" : dataReader.GetString("password");
                    string email = dataReader.IsDBNull(4) ? "" : dataReader.GetString("email");
                    string picture = dataReader.IsDBNull(5) ? "" : dataReader.GetString("picture");
                    int status = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt16("status");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    DateTime last_login = (DateTime)dataReader.GetMySqlDateTime("last_login");

                    User user = new User(id, firstname, lastname, password, email, picture, status, created_at, last_login);
                    users.Add(user);
                }
            }

            return users;
        }

        public User Read(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", ID.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE id = @id", parameters);

            if (dataReader.Read())
            {
                int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                string firstname = dataReader.IsDBNull(1) ? "" : dataReader.GetString("firstname");
                string lastname = dataReader.IsDBNull(2) ? "" : dataReader.GetString("lastname");
                string password = dataReader.IsDBNull(3) ? "" : dataReader.GetString("password");
                string email = dataReader.IsDBNull(4) ? "" : dataReader.GetString("email");
                string picture = dataReader.IsDBNull(5) ? "" : dataReader.GetString("picture");
                int status = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt16("status");
                DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                DateTime last_login = (DateTime)dataReader.GetMySqlDateTime("last_login");

                User user = new User(id, firstname, lastname, password, email, picture, status, created_at, last_login);

                return user;
            }
            
            return null;
        }

        public bool Update(User user)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", user.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE id = @id", parameters);

            Dictionary<string, string> newUser = new Dictionary<string, string>
            {
                { "@id", user.Id.ToString() },
                { "@firstname", user.Firstname },
                { "@lastname", user.Lastname },
                { "@password", user.Password },
                { "@email", user.Email },
                { "@picture", user.Picture },
                { "@status", user.Status.ToString() },
                { "@last_login", user.LastLogin.ToString() }
            };

            bool edit = mySQLConnector.Execute("UPDATE users SET firstname = @firstname, lastname = @lastname, password = @password, email = @email, picture = @picture, status = @status, last_login = @last_login WHERE id = @id", newUser);

            if (edit)
            {
                mySQLConnector.CloseConnection();
                return true;
            }
            return false;
        }

        public bool IsEmailFree(string email)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>
            {
                { "@email", email }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            if (dataReader.HasRows)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@email", email }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            if (dataReader.Read())
            {
                user.Id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                user.Firstname = dataReader.IsDBNull(1) ? "" : dataReader.GetString("firstname");
                user.Lastname = dataReader.IsDBNull(2) ? "" : dataReader.GetString("lastname");
                user.Password = dataReader.IsDBNull(3) ? "" : dataReader.GetString("password");
                user.Email = dataReader.IsDBNull(4) ? "" : dataReader.GetString("email");
                user.Picture = dataReader.IsDBNull(5) ? "" : dataReader.GetString("picture");
                user.Status = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt16("status");
                user.CreatedAt = (DateTime)dataReader.GetMySqlDateTime("created_at");
                user.LastLogin = (DateTime)dataReader.GetMySqlDateTime("last_login");

                return user;
            }

            return null;
        }
    }
}
