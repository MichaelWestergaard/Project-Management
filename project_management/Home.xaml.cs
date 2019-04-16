using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using project_management.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace project_management
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        MainController mainController = MainController.Instance;
        Board board;

        public Home()
        {
            //Skal gøres når man logger ind
            mainController.User = new UserDAO().Read(1);

            if (mainController.IsLoggedIn())
            {
                InitializeComponent();

                GetProjectList();
                board = new Board();

            } else
            {
               //Go to login again
            }
        }

        private void GetProjectList()
        {
            StackPanel projectList = (StackPanel) FindName("ProjectList");

            foreach (Project project in mainController.UserProjects())
            {
                ProjectSidebarElement projectSidebarElement = new ProjectSidebarElement
                {
                    Content = project.Name.ToCharArray()[0]
                };

                projectList.Children.Insert(projectList.Children.Count - 1, projectSidebarElement);
            }
                      
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button source = (Button)e.Source;
            int i = int.Parse(source.Uid);

            MenuTabIndicator.Margin = new Thickness(150*i, 0, 0, 0);

            //TODO: add different windows here
            switch (i)
            {
                case 0:
                    AppContent.Background = Brushes.AliceBlue;
                    break;

                case 1:
                    AppContent.Content = board;
                    break;

                case 2:

                    AppContent.Background = Brushes.Blue;
                    break;

                case 3:
                    AppContent.Background = Brushes.Brown;
                    break;
            }
        }

        private void ButtonCreateProject_Click(object sender, RoutedEventArgs e)
        {
            new CreateProject().Show();
        }
    }
}
