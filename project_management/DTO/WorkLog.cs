using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DTO
{
    class WorkLog
    {
        public WorkLog(int id, User assignedUser, int taskID, double work, DateTime createdAt)
        {
            Id = id;
            AssignedUser = assignedUser;
            TaskID = taskID;
            Work = work;
            CreatedAt = createdAt;
        }

        public int Id { get; set; }
        public User AssignedUser { get; set; }
        public int TaskID { get; set; }
        public double Work { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
