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
        MySQLConnector mySQLConnector = MySQLConnector.Instance;

        public bool Create(Section section)
        {
            var newSection = new Dictionary<string, string>();
            newSection.Add("@project_id", section.ProjectId.ToString());
            newSection.Add("@name", section.Name);
            newSection.Add("@due_date", section.DueDate.ToString());

            bool response = mySQLConnector.Execute("INSERT INTO sections (project_id, name, due_date) VALUES (@project_id, @name, @due_date)", newSection);
            
            mySQLConnector.CloseConnections();

            if (response)
            {
                return true;
            }

            return false;
        }

        public int CreateSection(Section section)
        {
            Dictionary<string, string> newSection = new Dictionary<string, string>();

            newSection.Add("@project_id", section.ProjectId.ToString());
            newSection.Add("@name", section.Name);
            newSection.Add("@due_date", section.DueDate.ToString("yyyy/MM/dd HH:mm:ss"));

            return mySQLConnector.Insert("INSERT INTO sections (project_id, name, due_date) VALUES (@project_id, @name, @due_date)", newSection);

        }

        public bool Delete(int id)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", id.ToString());

            bool reponse = mySQLConnector.Execute("DELETE FROM sections WHERE id = @id", parameters);

            mySQLConnector.CloseConnections();

            if (reponse)
                return true;

            return false;
        }


        public List<Section> List()
        {
            List<Section> sections = new List<Section>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM sections", null);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int project_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("project_id");
                    string name = dataReader.IsDBNull(2) ? "" : dataReader.GetString("name");
                    bool completed = dataReader.IsDBNull(3) ? false : dataReader.GetBoolean("completed");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                    //TODO: Get task list
                    Section section = new Section(id, project_id, name, completed, created_at, due_date, null);
                    sections.Add(section);
                }
            }
            mySQLConnector.CloseConnections(dataReader);

            return sections;
        }

        public List<Section> GetAll(int ID)
        {
            List<Section> sections = new List<Section>();


            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", ID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM sections WHERE project_id = @id", parameters);
            
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int project_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("project_id");
                    string name = dataReader.IsDBNull(2) ? "" : dataReader.GetString("name");
                    bool completed = dataReader.IsDBNull(3) ? false : dataReader.GetBoolean("completed");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    
                    Section section = new Section(id, project_id, name, completed, created_at, due_date, new TaskDAO().GetAll(id));
                    sections.Add(section);
                }
            }
            mySQLConnector.CloseConnections(dataReader);

            return sections;
        }

        public Section Read(int id)
        {

            var parameters = new Dictionary<string, string>();


            parameters.Add("@id", id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM sections WHERE id = @id", parameters);

            if (dataReader.Read())
            {
                int project_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("project_id");
                string name = dataReader.IsDBNull(2) ? "" : dataReader.GetString("name");
                bool completed = dataReader.IsDBNull(3) ? false : dataReader.GetBoolean("completed");
                DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                Section section = new Section(id, project_id, name, completed, created_at, due_date, null);
                mySQLConnector.CloseConnections(dataReader);

                return section;
            }
            mySQLConnector.CloseConnections(dataReader);

            return null;
        }


        public bool Update(Section section)
        {
            Dictionary<string, string> newSection = new Dictionary<string, string>
            {
                { "@id", section.Id.ToString() },
                { "@project_id", section.ProjectId.ToString() },
                { "@name", section.Name },
                { "@completed", section.Completed.ToString() },
                { "@due_date", section.DueDate.ToString() }
            };

            bool edit = mySQLConnector.Execute("UPDATE sections SET project_id = @project_id, name = @name, completed = @completed, due_date = @due_date WHERE id = @id", newSection);

            mySQLConnector.CloseConnections();

            if (edit)
            {
                return true;
            }
            return false;
        }
    }
}
