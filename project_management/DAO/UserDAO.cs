﻿using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_management;
using MySql.Data.MySqlClient;

namespace project_management.DAO
{
    class UserDAO : BaseDAO<User>
    {
        User user = new User();

        public bool create(User user)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            //Tjek om email allereade findes
            //parameters.Add("@email", "michaelwestergaard@hotmail.dk");
               parameters.Add("@email",  user.Email);

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            if (dataReader.HasRows != true)
            {
                var newUser = new Dictionary<string, string>();
                newUser.Add("@id", user.Id.ToString());
                newUser.Add("@firstname", user.Firstname);
                newUser.Add("@lastname", user.Lastname);
                newUser.Add("@password", user.Password);
                newUser.Add("@email", user.Email);
                newUser.Add("@picture", user.Picture);
                newUser.Add("@status", user.Status.ToString());
                newUser.Add("@created_at", user.CreatedAt.ToString());
                newUser.Add("@last_login", user.LastLogin.ToString());

                bool response = mySQLConnector.Execute("INSERT INTO users (id, firstname, lastname, password, email, picture, status, created_at, last_login ) VALUES (@id, @firstname, @lastname, @password, @email, @picture, @status, @created_at, @last_login ) ", newUser);
                Console.WriteLine("Inden oprettelse:");
                if (response)
                {
                    mySQLConnector.CloseConnection();
                    Console.WriteLine("Oprettet");
                    return true;
                }

            }
            mySQLConnector.CloseConnection();
            Console.WriteLine("Done");

            return false;
        }

        /*    while (dataReader.Read())
            {
                Console.WriteLine("Id: " + dataReader.GetString("id"));
                Console.WriteLine("Firstname: " + dataReader.GetString("firstname"));
                Console.WriteLine("Lastname: " + dataReader.GetString("lastname"));
                Console.WriteLine("Email: " + dataReader.GetString("email"));
                Console.WriteLine("Picture: " + dataReader.GetString("picture"));
                Console.WriteLine("Status: " + dataReader.GetString("status"));
                Console.WriteLine("CreatedAt: " + dataReader.GetString("createdAt"));
                Console.WriteLine("LastLogin: " + dataReader.GetString("lastLogin"));
            }
            */
        

        public User delete(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@email", "alenhasana@yahoo.dk");

            MySqlDataReader dataReader = mySQLConnector.GetData("DELETE FROM users WHERE email = @email", parameters);
           
            mySQLConnector.CloseConnection();

            return null;
        }

        public List<User> list()
        {
            throw new NotImplementedException();
        }

        public User read(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

              parameters.Add("@email", "alenhasana@yahoo.dk");
           
            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            if (dataReader.Read())
            {
                user.Id = dataReader.GetInt16("id");
                user.Firstname = dataReader.GetString("firstname");
                user.Lastname = dataReader.GetString("lastname");
                user.Password = dataReader.GetString("password");
                user.Email = dataReader.GetString("email");
                user.Picture = dataReader.GetString("picture");
                user.Status = dataReader.GetInt16("status");
                //   user.CreatedAt = dataReader.GetDateTime("created_at");
                //    user.LastLogin = dataReader.GetDateTime("last_login");
                Console.WriteLine("Id: " + dataReader.GetString("id"));
                Console.WriteLine("Firstname: " + dataReader.GetString("firstname"));
                Console.WriteLine("Lastname: " + dataReader.GetString("lastname"));
                Console.WriteLine("Email: " + dataReader.GetString("email"));
                Console.WriteLine("Picture: " + dataReader.GetString("picture"));
                Console.WriteLine("Status: " + dataReader.GetString("status"));
            
                return user;
            }


                return null;
        }

        public bool update(User obj)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@email", "alenhasana@yahoo.dk");

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            var newUser = new Dictionary<string, string>();
            newUser.Add("@id", user.Id.ToString());
            newUser.Add("@firstname", user.Firstname);
            newUser.Add("@lastname", user.Lastname);
            newUser.Add("@password", user.Password);
            newUser.Add("@email", user.Email);
            newUser.Add("@picture", user.Picture);
            newUser.Add("@status", user.Status.ToString());
            newUser.Add("@created_at", user.CreatedAt.ToString());
            newUser.Add("@last_login", user.LastLogin.ToString());

            bool edit = mySQLConnector.Execute("UPDATE users SET id = @id, firstname = @firstname, lastname = @lastname, password = @password, email = @email, picture = @picture, status = @status, created_at = @created_at, last_login = @last_login", newUser);

            if (edit)
            {
                mySQLConnector.CloseConnection();
                return true;
            }
            return false;
        }
    }
}
