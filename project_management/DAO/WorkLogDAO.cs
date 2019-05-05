using MySql.Data.MySqlClient;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project_management.DAO
{
    class WorkLogDAO : BaseDAO<WorkLog>
    {

        MySQLConnector mySQLConnector = MySQLConnector.Instance;

        public bool Create(WorkLog workLog)
        {
            Dictionary<string, string> newWorkLog = new Dictionary<string, string>();

            newWorkLog.Add("@user_id", workLog.AssignedUser?.Id.ToString());
            newWorkLog.Add("@task_id", workLog.TaskID.ToString());
            newWorkLog.Add("@work", workLog.Work.ToString());
            newWorkLog.Add("@created_at", workLog.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"));


            bool response = mySQLConnector.Execute("INSERT INTO work_log (user_id, task_id, work, created_at) VALUES (@user_id, @task_id , @work, @created_at)", newWorkLog);

            mySQLConnector.CloseConnection();

            if (response)
            {
                return true;
            }
            return false;
        }

        public int CreateWork(WorkLog workLog)
        {
            Dictionary<string, string> newWorkLog = new Dictionary<string, string>();

            newWorkLog.Add("@user_id", workLog.AssignedUser?.Id.ToString());
            newWorkLog.Add("@task_id", workLog.TaskID.ToString());
            newWorkLog.Add("@work", workLog.Work.ToString());
            newWorkLog.Add("@created_at", workLog.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"));

            return mySQLConnector.Insert("INSERT INTO work_log (user_id, task_id, work, created_at) VALUES (@user_id, @task_id , @work, @created_at)", newWorkLog);
        }
        
        public bool Delete(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", id.ToString() }
            };

            bool response = mySQLConnector.Execute("DELETE FROM work_log WHERE id = @id", parameters);

            mySQLConnector.CloseConnection();

            if (response)
                return true;

            return false;
        }

        public List<WorkLog> List()
        {
            List<WorkLog> workLogs = new List<WorkLog>();

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM work_log", null);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int user_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("user_id");
                    int task_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("task_id");
                    double work = dataReader.IsDBNull(3) ? 0.0 : dataReader.GetDouble("work");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    
                    WorkLog workLog = new WorkLog(id, new UserDAO().Read(user_id), task_id, work, created_at);

                    workLogs.Add(workLog);
                }
            }
            return workLogs;
        }

        public List<WorkLog> GetList(int taskID)
        {
            List<WorkLog> workLogs = new List<WorkLog>();

            Dictionary<String, String> parameters = new Dictionary<String, String>
            {
                { "@task_id", taskID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM work_log where task_id = @task_id", parameters);

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16("id");
                    int user_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("user_id");
                    int task_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("task_id");
                    double work = dataReader.IsDBNull(3) ? 0.0 : dataReader.GetDouble("work");
                    DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");
                    
                    WorkLog workLog = new WorkLog(id, new UserDAO().Read(user_id), task_id, work, created_at);

                    workLogs.Add(workLog);
                }
            }
            return workLogs;
        }


        public WorkLog Read(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@id", id.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT * FROM work_log WHERE id = @id", parameters);

            if (dataReader.HasRows)
            {
                dataReader.Read();
                int user_id = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16("user_id");
                int task_id = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt16("task_id");
                double work = dataReader.IsDBNull(3) ? 0.0 : dataReader.GetDouble("work");
                DateTime created_at = (DateTime)dataReader.GetMySqlDateTime("created_at");

                WorkLog workLog = new WorkLog(id, new UserDAO().Read(user_id), task_id, work, created_at);


                return workLog;
            }
            return null;
        }

        public bool Update(WorkLog workLog)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
    
               { "@user_id", workLog.AssignedUser?.Id.ToString() },
                { "@task_id", workLog.TaskID.ToString() },
                { "@work", workLog.Work.ToString() },

            };

            bool response = mySQLConnector.Execute("UPDATE work_log SET user_id = @user_id, task_id = @task_id, work = @work", parameters);

            mySQLConnector.CloseConnection();

            if (response)
                return true;

            return false;
        }

        public MySqlDataReader GetWorkLogByProject(int projectID)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>
            {
                { "@projectID", projectID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("select sum(work) as Work, created_at as Date from work_log WHERE task_id in (SELECT id from tasks WHERE section_id in (select id from sections WHERE project_id = @projectID)) GROUP BY created_at ORDER BY created_at asc", parameters);

            if (dataReader.HasRows)
            {
                return dataReader;
            }

            return null;
        }

        public double GetWorkSum(int taskID)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>
            {
                { "@task_id", taskID.ToString() }
            };

            MySqlDataReader dataReader = mySQLConnector.GetData("SELECT sum(work) as WorkDone FROM work_log where task_id = @task_id", parameters);

            if (dataReader.HasRows)
            {
                if (dataReader.Read())
                {
                    return dataReader.IsDBNull(0) ? 0.0 : dataReader.GetDouble("WorkDone");
                }
            }
            return 0;
        }

    }
}
        
