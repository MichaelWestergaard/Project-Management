﻿using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace project_management.DAO
{
    class TaskDAO : BaseDAO<Task>
    {
        MySQLConnector mySQLConnector = MySQLConnector.Instance;

        public bool Create(Task task)
        {
            Dictionary<string, string> newTask = new Dictionary<string, string>();

            newTask.Add("@parent_task_id", task.ParentTask?.Id.ToString());
            newTask.Add("@requires_task_id", task.RequiresTask?.Id.ToString());
            newTask.Add("@section_id", task.SectionID.ToString());
            newTask.Add("@user_id", task.AssignedUser?.Id.ToString());
            newTask.Add("@name", task.Name);
            newTask.Add("@description", task.Description);
            newTask.Add("@due_date", task.DueDate.ToString("yyyy/MM/dd HH:mm:ss"));
            newTask.Add("@estimated_time", task.EstimatedTime.ToString());
            newTask.Add("@priority", task.Priority.ToString());
            
            bool response = mySQLConnector.Execute("INSERT INTO tasks (parent_task_id, requires_task_id, section_id, user_id, name, description, due_date, estimated_time, priority) VALUES (@parent_task_id, @requires_task_id , @section_id, @user_id, @name, @description, @due_date, @estimated_time, @priority)", newTask);

            mySQLConnector.CloseConnection();

            if (response)
            {
                return true;
            }

            return false;
        }

        public int CreateTask(Task task)
        {
            Dictionary<string, string> newTask = new Dictionary<string, string>();

            newTask.Add("@parent_task_id", task.ParentTask?.Id.ToString());
            newTask.Add("@requires_task_id", task.RequiresTask?.Id.ToString());
            newTask.Add("@section_id", task.SectionID.ToString());
            newTask.Add("@user_id", task.AssignedUser?.Id.ToString());
            newTask.Add("@name", task.Name);
            newTask.Add("@description", task.Description);
            newTask.Add("@due_date", task.DueDate.ToString("yyyy/MM/dd HH:mm:ss"));
            newTask.Add("@estimated_time", task.EstimatedTime.ToString());
            newTask.Add("@priority", task.Priority.ToString());

            return mySQLConnector.Insert("INSERT INTO tasks (parent_task_id, requires_task_id, section_id, user_id, name, description, due_date, estimated_time, priority) VALUES (@parent_task_id, @requires_task_id , @section_id, @user_id, @name, @description, @due_date, @estimated_time, @priority)", newTask);
            
        }

        public bool Delete(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", id.ToString() }
            };

            bool response = mySQLConnector.Execute("DELETE FROM tasks WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            if (response)
                return true;

            return false;
        }

        public List<Task> List()
        {
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
                    bool completed = dataReader.IsDBNull(9) ? false : dataReader.GetBoolean("completed");
                    Double estimated_time = dataReader.IsDBNull(9) ? 0.0 : dataReader.GetDouble("estimated_time");
                    int priority = dataReader.IsDBNull(10) ? 0 : dataReader.GetInt16("priority");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                    Task parentTask = parent_task_id == 0 ? null : this.Read(parent_task_id);
                    Task requiredTask = requires_task_id == 0 ? null : this.Read(requires_task_id);

                    Task task = new Task(id, parentTask, requiredTask, new UserDAO().Read(user_id), section_id, name, description, estimated_time, priority, completed, start_date, due_date, created_at);
                    
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        public Task Read(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", id.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM tasks WHERE id = @id", parameters);

            if (dataReader.HasRows)
            {
                dataReader.Read();
                int parent_task_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("parent_task_id");
                int requires_task_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("requires_task_id");
                int section_id = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt16("section_id");
                int user_id = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt16("user_id");
                string name = dataReader.IsDBNull(5) ? "" : dataReader.GetString("name");
                string description = dataReader.IsDBNull(6) ? "" : dataReader.GetString("description");
                DateTime start_date = (DateTime)dataReader.GetMySqlDateTime("start_date");
                DateTime due_date = (DateTime)dataReader.GetMySqlDateTime("due_date");
                bool completed = dataReader.IsDBNull(9) ? false : dataReader.GetBoolean("completed");
                Double estimated_time = dataReader.IsDBNull(9) ? 0.0 : dataReader.GetDouble("estimated_time");
                int priority = dataReader.IsDBNull(10) ? 0 : dataReader.GetInt16("priority");
                DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                Task parentTask = parent_task_id == 0 ? null : this.Read(parent_task_id);
                Task requiredTask = requires_task_id == 0 ? null : this.Read(requires_task_id);

                Task task = new Task(id, parentTask, requiredTask, new UserDAO().Read(user_id), section_id, name, description, estimated_time, priority, completed, start_date, due_date, created_at);

                return task;
            }
            return null;
        }

        public bool Update(Task obj)
        {
            throw new NotImplementedException();
        }
    }
}
