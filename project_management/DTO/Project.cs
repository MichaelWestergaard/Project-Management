using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DTO
{
    class Project
    {
        public Project()
        {
        }

        public Project(int id, int parentProjectID, int projectOwnerID, string name, string description, bool completed, DateTime createdAt, DateTime dueDate /*List<Section> sectionList*/)
        {
            Id = id;
            ParentProjectID = parentProjectID;
            ProjectOwnerID = projectOwnerID;
            Name = name;
            Description = description;
            Completed = completed;
            CreatedAt = createdAt;
            DueDate = dueDate;
  //          SectionList = sectionList;
        }

        public int Id { get; set; }
        public int ParentProjectID { get; set; }
        public int ProjectOwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        //      public List<Section> SectionList { get; set; }
        bool edit = mySQLConnector.Execute("UPDATE users SET id = @id, firstname = @firstname, lastname = @lastname, password = @password, email = @email, picture = @picture, status = @status, created_at = @created_at, last_login = @last_login WHERE email = @email", newUser);

            if (edit)
            {
                mySQLConnector.CloseConnection();
                return true;
            }
            return false;
        }
}
