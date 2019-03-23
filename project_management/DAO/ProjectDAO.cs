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
                newProject.Add("@parent_project_id", project.ParentProject.ToString());
                newProject.Add("@user_id", project.ProjectOwner.ToString());
                newProject.Add("@name", project.Name);
                newProject.Add("@description", project.Description);
                newProject.Add("@completed", project.Completed.ToString());
                newProject.Add("@created_at", project.CreatedAt.ToString());
                newProject.Add("@due_date", project.DueDate.ToString());


                bool response = mySQLConnector.Execute("INSERT INTO projects (id, parent_project_id, user_id , name, description, completed, created_at, due_date) VALUES (@id, @parent_project_id, @user_id , @name, @description, @completed, @created_at, @due_date ) ", newProject);
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
            throw new NotImplementedException();
        }

        public Project read(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", project.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM projects WHERE id = @id", parameters);

            if (dataReader.Read())
            {
                //Alle reads skal lige skrives, gøres senere i dag                
                //    user.DueDate = dataReader.GetDateTime("due_date");
                //   user.CreatedAt = dataReader.GetDateTime("created_at");

             

                return project;
            }


            return null;
        }

    

    public bool update(Project obj)
        {
            throw new NotImplementedException();
        }
    }
}
