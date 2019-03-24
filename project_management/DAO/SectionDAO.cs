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
                    return true;
                }

            }
            mySQLConnector.CloseConnection();
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
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            List<Section> sections = new List<Section>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM section", null);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int project_id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("project_id");
                    string name = dataReader.IsDBNull(0) ? "" : dataReader.GetString("name");
                    bool completed = dataReader.IsDBNull(0) ? false : dataReader.GetBoolean("completed");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                    Section section = new Section(id, project_id, name, completed, created_at, due_date);
                    sections.Add(section);
                }
            }
            for (int i = 0; i < sections.Count; i++)
                Console.WriteLine("" + sections[i].Name);

            return sections;
        }
    

        public Section read(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();


            parameters.Add("@id", section.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM section WHERE id = @id", parameters);

            if (dataReader.Read())
            {              
            
                section.Id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                section.ProjectId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("ProjectId");
                section.Name = dataReader.IsDBNull(0) ? "" : dataReader.GetString("name");
                section.Completed = dataReader.IsDBNull(0) ? false : dataReader.GetBoolean("completed");
                section.CreatedAt = (DateTime)dataReader.GetMySqlDateTime("created_at");
                section.DueDate = (DateTime)dataReader.GetMySqlDateTime("due_date");

                return section;
            }


            return null;
        }
   

        public bool update(Section obj)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", section.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM section WHERE id = @id", parameters);

           
                var newSection = new Dictionary<string, string>();
                newSection.Add("@id", section.Id.ToString());
                newSection.Add("@project_id", section.ProjectId.ToString());
                newSection.Add("@name", section.Name);
                newSection.Add("@completed", section.Completed.ToString());
                newSection.Add("@due_date", section.DueDate.ToString());
                newSection.Add("@created_at", section.CreatedAt.ToString());

                bool edit = mySQLConnector.Execute("UPDATE section SET id = @id, project_id = @project_id, name = @name, completed = @completed, due_date = @due_date, created_at = @created_at WHERE id = @id", newSection);

                if (edit)
                {
                    mySQLConnector.CloseConnection();
                    return true;
                }
                return false;
            }
    }
}
