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
            SectionDAO sectionDAO = new SectionDAO();

            List<Section> sections = sectionDAO.GetAll(7);

            InitializeComponent();

            StackPanel sectionList = (StackPanel) FindName("SectionList");

            foreach (Section section in sections)
            {
                SectionElement sectionElement = new SectionElement(sectionList);
                
                foreach (Task task in section.TaskList)
                {
                    TaskElement taskElement = new TaskElement();

                    taskElement.TaskID.Name = "Task" + task.Id;

                    taskElement.title.Text = task.Name;
                    taskElement.description.Text = task.Description;
                    taskElement.avatar.ImageSource = new BitmapImage(new Uri(task.AssignedUser.Picture));
                    taskElement.UserButton.ToolTip = task.AssignedUser.Firstname + " " + task.AssignedUser.Lastname;

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
