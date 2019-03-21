using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_management.Connector;

namespace project_management.DAO
{
    class UserDAO : BaseDAO<User>
    {

        public bool create(User obj)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@email", "michaelwestergaard@hotmail.dk");

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM users WHERE email = @email", parameters);

            while (dataReader.Read())
            {
                Console.WriteLine("Username: " + dataReader.GetString("firstname"));
            }

            mySQLConnector.CloseConnection();




            return true;
        }

        public User delete(int ID)
        {
            throw new NotImplementedException("Denne metode er ikke lavet");
        }

        public List<User> list()
        {
            throw new NotImplementedException();
        }

        public User read(int ID)
        {
            throw new NotImplementedException();
        }

        public bool update(User obj)
        {
            throw new NotImplementedException();
        }
    }
}
