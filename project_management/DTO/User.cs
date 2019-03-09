using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DTO
{
    class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }

        public User(int id, string firstname, string lastname, string email, string picture, int status, DateTime createdAt, DateTime lastLogin)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Picture = picture;
            Status = status;
            CreatedAt = createdAt;
            LastLogin = lastLogin;
        }

    }
}
