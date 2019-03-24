using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace project_management.DAO
{
    class TaskDAO : BaseDAO<Task>
    {

        Task task = new Task();


        public bool create(Task obj)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            var parameters = new Dictionary<string, string>();

            parameters.Add("@id", task.Id.ToString());

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM tasks WHERE id = @id", parameters);

            if (dataReader.HasRows != true)
            {
                var newTask = new Dictionary<string, string>();
                newTask.Add("@id", task.Id.ToString());
                newTask.Add("@parent_task_id", task.ParentTask.ToString());


                newTask.Add("@requires_task_id", task.AssignedUser.ToString());
                newTask.Add("@name", project.Name);
                newTask.Add("@description", project.Description);
                newTask.Add("@completed", project.Completed.ToString());
                newTask.Add("@created_at", project.CreatedAt.ToString());
                newTask.Add("@due_date", project.DueDate.ToString());

        public Task ParentTask { get; set; }
        public Task RequiresTask { get; set; }
        public User AssignedUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double EstimatedTime { get; set; }
        public int Priority { get; set; }
        public bool Completed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }

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

        public Task delete(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Task> list()
        {
            throw new NotImplementedException();
        }

        public Task read(int ID)
        {
            throw new NotImplementedException();
        }

        public bool update(Task obj)
        {
            throw new NotImplementedException();
        }
    }
}
