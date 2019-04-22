using project_management.Controllers;
using project_management.DAO;
using project_management.DTO;
using project_management.Elements;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Page
    {
        public Board()
        {

            InitializeComponent();

            MainController mainController = MainController.Instance;
            SectionDAO sectionDAO = new SectionDAO();

            if(mainController.Project != null)
            {
                List<Section> sections = sectionDAO.GetAll(mainController.Project.Id);


                StackPanel sectionList = (StackPanel) FindName("SectionList");

                foreach (Section section in sections)
                {
                    SectionElement sectionElement = new SectionElement(sectionList);
                
                    foreach (Task task in section.TaskList)
                    {
                        TaskElement taskElement = new TaskElement(task.Id);

                        taskElement.TaskID.Name = "Task" + task.Id;

                        taskElement.title.Text = task.Name;
                        taskElement.description.Text = task.Description;

                        if(task.AssignedUser != null)
                        {
                            taskElement.avatar.ImageSource = new BitmapImage(new Uri(task.AssignedUser.Picture));
                            taskElement.UserButton.ToolTip = task.AssignedUser.Firstname + " " + task.AssignedUser.Lastname;
                        }
                        
                        ((StackPanel) sectionElement.SectionID).Children.Add(taskElement);
                    }


                    sectionElement.SectionID.Name = "Section" + section.Id;
                    sectionElement.SectionName.Text = section.Name;

                    sectionList.Children.Add(sectionElement);

                }

                sectionList.Children.Add(new NewSectionElement(sectionList));


            }
        }
                
    }
}
