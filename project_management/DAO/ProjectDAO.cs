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

        Project project = new Project();

        public bool create(Project project)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", project.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects WHERE id = @id", parameters);

            if (dataReader.HasRows != true)
            {
                var newProject = new Dictionary<string, string>();
                newProject.Add("@id", project.Id.ToString());
                //newProject.Add("@parent_project_id", project.ParentProject.ToString());
                //newProject.Add("@user_id", project.ProjectOwner.ToString());
                newProject.Add("@name", project.Name);
                newProject.Add("@description", project.Description);
                newProject.Add("@completed", project.Completed.ToString());
                newProject.Add("@created_at", project.CreatedAt.ToString());
                newProject.Add("@due_date", project.DueDate.ToString());


                bool response = mySQLConnector.Execute("INSERT INTO projects (id, parent_project_id, user_id , name, description, completed, created_at, due_date) VALUES (@id, @parent_project_id, @user_id , @name, @description, @completed, @created_at, @due_date ) ", newProject);
                if (response)
                {
                    mySQLConnector.CloseConnection();
                    return true;
                }

            }
            mySQLConnector.CloseConnection();
            return false;
        }

        public int CreateProject(Project project)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var newProject = new Dictionary<string, string>();
            newProject.Add("@user_id", project.ProjectOwnerID.ToString());
            newProject.Add("@name", project.Name);
            newProject.Add("@description", project.Description);
            newProject.Add("@due_date", project.DueDate.ToString());
            
            int projectID = mySQLConnector.Insert("INSERT INTO projects (user_id, name, description, due_date) VALUES (@user_id, @name, @description, @due_date)", newProject);

            mySQLConnector.CloseConnection();

            return projectID;
        }

        public Project delete(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", project.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("DELETE FROM projects WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            return null;
        }

        public List<Project> list()
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            List<Project> projects = new List<Project>();

            //Tjek om email allereade findes
            //parameters.Add("@email", "michaelwestergaard@hotmail.dk");


            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects", null);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int parent_project_id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("parent_project_id");
                    int user_id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("user_id");
                    string name = dataReader.IsDBNull(0) ? "" : dataReader.GetString("name");
                    string description = dataReader.IsDBNull(0) ? "" : dataReader.GetString("description");
                    bool completed = dataReader.IsDBNull(0) ? false : dataReader.GetBoolean("completed");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");

          
                    Project project = new Project(id, parent_project_id, user_id, name, description, completed, created_at, due_date);
                    projects.Add(project);
                }
            }
            for (int i = 0; i < projects.Count; i++)
                Console.WriteLine("" + projects[i].Name);

            return projects;
        }

        public Project read(int ID)
        {
                MySQLConnector mySQLConnector = MySQLConnector.Instance;

                var parameters = new Dictionary<string, string>();

                parameters.Add("@id", ID.ToString());

                MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects WHERE id = @id", parameters);

                if (dataReader.Read())
                {
                 project.Id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                project.ParentProjectID = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("parent_project_id");
                project.ProjectOwnerID = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("user_id");
                project.Name = dataReader.IsDBNull(3) ? "" : dataReader.GetString("name");
                project.Description = dataReader.IsDBNull(4) ? "" : dataReader.GetString("description");
                project.Completed = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean("completed");
                project.CreatedAt = (DateTime)dataReader.GetMySqlDateTime("created_at");
                project.DueDate = (DateTime)dataReader.GetMySqlDateTime("due_date");

                return project;
                }


                return null;
            }



    

    public bool update(Project obj)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToProject(int projectID, int userID)
        {
            UserDAO userDAO = new UserDAO();
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            if (read(projectID) != null && userDAO.read(userID) != null)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "@projectID", projectID.ToString() },
                    { "@userID", userID.ToString() }
                };
                
                bool response = mySQLConnector.Execute("INSERT INTO project_users (project_id, user_id) VALUES (@projectID, @userID)", parameters);

                if (response)
                {
                    mySQLConnector.CloseConnection();
                    return true;
                }
                return false;
            }

            return false;
        }
    }
}
