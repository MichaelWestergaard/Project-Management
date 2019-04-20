using project_management.DAO;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.Controllers
{
    class MainController
    {
        private User user;
        private Project project;

        private ProjectDAO projectDAO = new ProjectDAO();

        private static MainController instance = null;

        public static MainController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainController();
                }
                return instance;
            }
        }

        internal User User { get => user; set => user = value; }
        internal Project Project { get => project; set => project = value; }

        public bool IsLoggedIn()
        {
            if (User != null)
                return true;
            return false;
        }

        public List<Project> UserProjects()
        {
            return projectDAO.UserProjects(user.Id);
        }
    }
}
