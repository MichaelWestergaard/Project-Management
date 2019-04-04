using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DTO
{
    class Task
    {
        public Task(int id, Task parentTask, Task requiresTask, User assignedUser, int sectionID, string name, string description, double estimatedTime, int priority, bool completed, DateTime startDate, DateTime dueDate, DateTime createdAt)
        {
            Id = id;
            ParentTask = parentTask;
            RequiresTask = requiresTask;
            AssignedUser = assignedUser;
            Name = name;
            SectionID = sectionID;
            Description = description;
            EstimatedTime = estimatedTime;
            Priority = priority;
            Completed = completed;
            StartDate = startDate;
            DueDate = dueDate;
            CreatedAt = createdAt;
        }

        public Task(Task parentTask, Task requiresTask, User assignedUser, string name, string description, double estimatedTime, int priority, DateTime dueDate)
        {
            ParentTask = parentTask;
            RequiresTask = requiresTask;
            AssignedUser = assignedUser;
            Name = name;
            Description = description;
            EstimatedTime = estimatedTime;
            Priority = priority;
            DueDate = dueDate;
        }

        public int Id { get; set; }
        public Task ParentTask { get; set; }
        public Task RequiresTask { get; set; }
        public User AssignedUser { get; set; }
        public int SectionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double EstimatedTime { get; set; }
        public int Priority { get; set; }
        public bool Completed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
