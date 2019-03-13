using MySql.Data.MySqlClient;
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
        private MySQLConnector connector { get; set; }

        private MySQLConnector()
        {
            connection = new MySqlConnection("SERVER=mysql25.unoeuro.com;DATABASE=michaelwestergaard_dk_db;UID=michaelwest_dk;PASSWORD=peyifimobi27;");
        }

        public MySQLConnector GetInstance()
        {
            if (connector == null || connector.connection == null)
                connector = new MySQLConnector();
            return connector;
        }
    }
}
