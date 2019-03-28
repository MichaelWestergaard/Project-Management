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


        public bool Create(Task task)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

          
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", task.Id.ToString() }
            };


            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM tasks WHERE id = @id", parameters);

            if (dataReader.HasRows != true)
            {
                Dictionary<string, string> newTask = new Dictionary<string, string>
                {
               //     { "@id", task.Id },
              //      { "@parent_task_id", task.ParentTask },
              //      { "@requires_task_id", task.RequiresTask },
               //     { "@section_id", task.SectionId },
               //     { "@user_id", task.AssignedUser },
                      { "@name", task.Name },
                      { "@description", task.Description },
  //                  { "@start_date", task.StartDate },
         //           { "@due_date", task.DueDate },
          //          { "@completed", task.Completed },
            //        { "@estimated_time", task.EstimatedTime },
            //        { "@priority", task.Priority },  
            //        { "@created_at", task.CreatedAt }
                };
            


        bool response = mySQLConnector.Execute("INSERT INTO tasks (id, parent_task_id, requires_task_id, section_id, user_id, name, description, start_date, due_date, completed, estimated_time, priority, created_at) VALUES (@id, @parent_task_id, @requires_task_id , @section_id, @user_id, @name, @description, @start_date, @due_date, @completed, @estimated_time, @priority,  @created_at  ) ", newTask);
                if (response)
                {
                    mySQLConnector.CloseConnection();
                    return true;
                }

            }
            mySQLConnector.CloseConnection();
            return false;
        }

        public Task Delete(int ID)
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", ID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("DELETE FROM tasks WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            return null;






        }

        public List<Task> List()
        {
            MySQLConnector mySQLConnector = MySQLConnector.Instance;

            List<Task> tasks = new List<Task>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM tasks", null);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {

                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int parent_task_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("parent_task_id");
                    int requires_task_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("requires_task_id");
                    int section_id = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("section_id");
                    int user_id = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("user_id");
                    string name = dataReader.IsDBNull(5) ? "" : dataReader.GetString("name");
                    string description = dataReader.IsDBNull(6) ? "" : dataReader.GetString("description");
                    DateTime start_date = (DateTime)dataReader.GetMySqlDateTime("start_date");
                    DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                    bool completed = dataReader.IsDBNull(0) ? false : dataReader.GetBoolean("completed");
                    Double estimated_time = dataReader.IsDBNull(0) ? 0.0 : dataReader.GetDouble("estimated_time");
                    int priority = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("priority");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                    Task task = new Task(id, parent_task_id, requires_task_id, section_id, user_id, name, description, start_date, due_date, completed, estimated_time, priority, created_at);
                    tasks.Add(task);
                }
            }
            return tasks;
        }

            public Task Read(int ID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Task obj)
        {
            throw new NotImplementedException();
        }
    }
}
