using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using project_management.Windows;
using System;
using System.Collections.Generic;
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
        Overview overview;
        MainController mainController = MainController.Instance;
        Board board;
        List<string> colors = new List<string>();
        StackPanel projectList;
        
        public Home()
        {
            //Skal gøres når man logger ind
            mainController.User = new UserDAO().Read(1);

            if (mainController.IsLoggedIn())
            {
                InitializeComponent();
                projectList = (StackPanel)FindName("ProjectList");
                AppContent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

                GetProjectList();
                board = new Board();
                overview = new Overview();

            } else
            {
               //Go to login again
            }
        }

        private void AddColors()
        {
            colors = new List<string>
                {
                    "#4CAF50",
                    "#3F51B5",
                    "#F44336",
                    "#009688",
                    "#ff9800",
                    "#673ab7",
                    "#00bcd4",
                    "#607d8b"
                };
        }

        private void GetProjectList()
        {
            List<Project> projects = mainController.UserProjects();

            if (projects.Count > 0)
            {
                mainController.Project = projects[0];

                foreach (Project project in projects)
                {
                    NewProjectElement(project.Id, project.Name);
                }
            }
        }

        public void NewProjectElement(int projectID, string nameStr)
        {
            char[] name = nameStr.ToCharArray();
            string content = "";

            content += name[0];

            if (name.Length > 1)
                content += name[1];

            // Kunne måske gemme farve ved oprettede, men w/e
            Random random = new Random();

            if (colors.Count == 0)
                AddColors();

            int index = random.Next(colors.Count);

            Brush color = (Brush)new BrushConverter().ConvertFrom(colors[index]);
            colors.RemoveAt(index);

            Button projectButton = new Button
            {
                Uid = projectID.ToString(),
                Content = content.ToUpper(),
                Margin = new Thickness(5),
                Width = 50,
                Height = 50,
                FontSize = 18,
                Padding = new Thickness(4),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ToolTip = nameStr,
                Background = color,
                BorderBrush = color,
            };

            projectButton.Click += ChangeProject_Click;

            projectList.Children.Insert(projectList.Children.Count - 1, projectButton);
        }
        
        private void ChangeProject_Click(object sender, RoutedEventArgs e)
        {
            Button source = (Button)e.Source;
            int projectID = int.Parse(source.Uid);
            mainController.Project = new ProjectDAO().Read(projectID);

            Board boardNew = new Board();

            if(AppContent.Content != null)
                if (AppContent.Content.Equals(board))
                {
                    AppContent.Content = boardNew;
                }

            board = boardNew;
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
                    AppContent.Content = overview;
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
            new CreateProject(this).Show();
        }
    }
}
