using project_management.DAO;
using project_management.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToastNotifications.Messages;
using Section = project_management.DTO.Section;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for NewSection.xaml
    /// </summary>
    public partial class NewSection : Window
    {
        private int projectID = 7;
        private StackPanel sectionList;

        public NewSection(int projectID, StackPanel sectionList)
        {
            this.projectID = projectID;
            this.sectionList = sectionList;
            InitializeComponent();
        }

        private void ButtonCreateSection_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Section section = new Section
                {
                    Name = name.Text,
                    ProjectId = projectID,
                    DueDate = DateTime.Parse(deadline.Text)
                };

                SectionDAO sectionDAO = new SectionDAO();

                int sectionID = sectionDAO.CreateSection(section);

                if (sectionDAO.Read(sectionID) != null)
                {
                    SectionElement sectionElement = new SectionElement(sectionList);

                    sectionElement.SectionID.Name = "Section" + sectionID;
                    sectionElement.SectionName.Text = name.Text;
                    
                    sectionList.Children.Insert(sectionList.Children.Count - 1, sectionElement);
                    this.Close();
                }
            }
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (name.Text != "")
            {
                if (deadline.Text != "")
                {
                    if (DateTime.TryParse(deadline.Text, out DateTime result))
                    {
                        return true;
                    }
                    else
                    {
                        new Utilities().GetNotifier().ShowError("Vælg venligst en korrekt dato");
                    }
                }
                else
                {
                    new Utilities().GetNotifier().ShowError("Udfyld deadline dato");
                }
            }
            else
            {
                new Utilities().GetNotifier().ShowError("Udfyld navn");
            }

            return false;
        }
    }
}
