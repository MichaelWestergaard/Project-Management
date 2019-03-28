using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DAO
{
    class ProjectDAO : BaseDAO<Project>
    {
        MySQLConnector mySQLConnector = MySQLConnector.Instance;

        public bool Create(Project project)
        {
            Dictionary<string, string> newProject = new Dictionary<string, string>
            {
                { "@parent_project_id", project.ParentProjectID.ToString() },
                { "@user_id", project.ProjectOwnerID.ToString() },
                { "@name", project.Name },
                { "@description", project.Description },
                { "@due_date", project.DueDate.ToString() }
            };


            bool response = mySQLConnector.Execute("INSERT INTO projects (parent_project_id, user_id , name, description, due_date) VALUES (@parent_project_id, @user_id , @name, @description, @due_date)", newProject);
            if (response)
            {
                mySQLConnector.CloseConnection();
                return true;
            }

            mySQLConnector.CloseConnection();
            return false;
        }

        public bool Delete(int id)
        {
            var parameters = new Dictionary<string, string>
            {
                { "@id", id.ToString() }
            };

            bool response = mySQLConnector.Execute("DELETE FROM projects WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            if (response)
                return true;

            return false;
        }

        public List<Project> List()
        {
            List<Project> projects = new List<Project>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects", null);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int parent_project_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("parent_project_id");
                    int user_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("user_id");
                    string name = dataReader.IsDBNull(3) ? "" : dataReader.GetString("name");
                    string description = dataReader.IsDBNull(4) ? "" : dataReader.GetString("description");
                    bool completed = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean("completed");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");


                    Project project = new Project(id, parent_project_id, user_id, name, description, completed, created_at, due_date);
                    projects.Add(project);
                }
            }

            return projects;
        }

        public Project Read(int ID)
        {
            var parameters = new Dictionary<string, string>
            {
                { "@id", ID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects WHERE id = @id", parameters);

            if (dataReader.Read())
            {
                int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                int parent_project_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("parent_project_id");
                int user_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("user_id");
                string name = dataReader.IsDBNull(3) ? "" : dataReader.GetString("name");
                string description = dataReader.IsDBNull(4) ? "" : dataReader.GetString("description");
                bool completed = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean("completed");
                DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                
                Project project = new Project(id, parent_project_id, user_id, name, description, completed, created_at, due_date);

                return project;
            }
            
            return null;
        }
        
        public bool Update(Project project)
        {

            var newProject = new Dictionary<string, string>();
            newProject.Add("@id", project.Id.ToString());
            newProject.Add("@parent_project_id", project.ParentProjectID.ToString());
            newProject.Add("@user_id", project.ProjectOwnerID.ToString());
            newProject.Add("@name", project.Name);
            newProject.Add("@description", project.Description);
            newProject.Add("@completed", project.Completed.ToString());
            newProject.Add("@created_at", project.CreatedAt.ToString());
            newProject.Add("@due_date", project.DueDate.ToString());
            
            bool edit = mySQLConnector.Execute("UPDATE projects SET parent_project_id = @parent_project_id, user_id = @user_id, name = @name, description = @description, completed = @completed, created_at = @created_at, due_date = @due_date WHERE id = @id", newProject);

            mySQLConnector.CloseConnection();

            if (edit)
            {
                return true;
            }
            return false;
        }
    }
}
