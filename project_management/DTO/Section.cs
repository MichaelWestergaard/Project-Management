using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DTO
{
    class Section
    {
        public Section(int id, string name, bool completed, DateTime createdAt, DateTime dueDate, List<Task> taskList)
        {
            Id = id;
            Name = name;
            Completed = completed;
            CreatedAt = createdAt;
            DueDate = dueDate;
            TaskList = taskList;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public List<Task> TaskList { get; set; }
    }
}
