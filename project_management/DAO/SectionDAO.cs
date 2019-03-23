using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DAO
{
    class SectionDAO : BaseDAO<Section>
    {

        Section section = new Section();

        public bool create(Section section)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", section.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM section WHERE id = @id", parameters);

            if (dataReader.HasRows != true)
            {
                var newSection = new Dictionary<string, string>();
                newSection.Add("@id", section.Id.ToString());
                newSection.Add("@project_id", section.ProjectId.ToString());
                newSection.Add("@name", section.Name);
                newSection.Add("@completed", section.Completed.ToString());
                newSection.Add("@due_date", section.DueDate.ToString());
                newSection.Add("@created_at", section.CreatedAt.ToString());

                bool response = mySQLConnector.Execute("INSERT INTO sections (id, project_id, name, completed, due_date, created_at) VALUES (@id, @project_id, @name, @completed, @due_date, @created_at ) ", newSection);
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


        public Section delete(int ID)
        {
                MySQLConnector mySQLConnector = MySQLConnector.Instance;

                var parameters = new Dictionary<string, string>();

                parameters.Add("@id", section.Id.ToString());

                MySqlDataReader dataReader = mySQLConnector.GetData("DELETE FROM section WHERE id = @id", parameters);

                mySQLConnector.CloseConnection();

                return null;
            }
        

        public List<Section> list()
        {
            throw new NotImplementedException();
        }

        public Section read(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", section.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM section WHERE id = @id", parameters);

            if (dataReader.Read())
            {
                section.Id = dataReader.GetInt16("id");
                section.ProjectId = dataReader.GetInt16("project_id");
                section.Name = dataReader.GetString("name");
                section.Completed = dataReader.GetBoolean("completed");

                //    user.DueDate = dataReader.GetDateTime("due_date");
                //   user.CreatedAt = dataReader.GetDateTime("created_at");

                Console.WriteLine("Id: " + dataReader.GetString("id"));
                Console.WriteLine("Firstname: " + dataReader.GetString("firstname"));
                Console.WriteLine("Lastname: " + dataReader.GetString("lastname"));
                Console.WriteLine("Email: " + dataReader.GetString("email"));
                Console.WriteLine("Picture: " + dataReader.GetString("picture"));
                Console.WriteLine("Status: " + dataReader.GetString("status"));

                return section;
            }


            return null;
        }
   

        public bool update(Section obj)
        {
            throw new NotImplementedException();
        }
    }
}
