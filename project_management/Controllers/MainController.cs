using project_management.DAO;
using project_management.DTO;
using project_management.Pages;
using project_management.Windows;
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
        private Home home;

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
        public Home Home { get => home; set => home = value; }

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

        public void ChangeProject()
        {
            if(Project != null)
            {
                Board boardNew = new Board();
                Dashboard overviewNew = new Dashboard();

                if (home.AppContent.Content != null)
                    if (home.AppContent.Content.Equals(home.Board))
                    {
                        home.AppContent.Content = boardNew;
                    }
                    else if (home.AppContent.Content.Equals(home.Overview))
                    {
                        home.AppContent.Content = overviewNew;
                    }

                home.Board = boardNew;
                home.Overview = overviewNew;
            } else
            {
                home.AppContent.Content = new WelcomePage();
            }
        }
    }
}
