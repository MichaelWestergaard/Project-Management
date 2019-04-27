using MaterialDesignThemes.Wpf;
using project_management.DAO;
using project_management.DTO;
using project_management.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for SectionElement.xaml
    /// </summary>
    public partial class SectionElement : UserControl
    {

        string name;
        StackPanel sectionList;

        public string Name { get => name; set => name = value; }

        public SectionElement(StackPanel sectionList)
        {
            this.sectionList = sectionList;

            InitializeComponent();
        }

        private void NewTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            
            StackPanel currentSection = (StackPanel)button.Parent;

            new NewTask(currentSection).Show();

        }

        private void DeleteSection_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            StackPanel currentSection = (StackPanel) ((MaterialDesignThemes.Wpf.Card) button.FindName("SectionCard")).Parent;

            for (int i = 0; i < sectionList.Children.Count-1; i++)
            {
                if (((StackPanel)((SectionElement)sectionList.Children[i]).Content).Name.Equals(currentSection.Name))
                {
                    if(new SectionDAO().Delete(int.Parse(currentSection.Name.Replace("Section", ""))))
                        sectionList.Children.RemoveAt(i);
                    
                    break;
                }
            }
        }

        private void SectionCard_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));

            Button button = (Button) FindName("DeleteSection");

            button.BeginAnimation(OpacityProperty, doubleAnimation);
        }

        private void SectionCard_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));

            Button button = (Button)FindName("DeleteSection");

            button.BeginAnimation(OpacityProperty, doubleAnimation);
        }

        private void ShowCompletedTasks_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            StackPanel completedTaskList = (StackPanel) ((StackPanel)button.Parent).FindName("CompletedTaskList");

            PackIcon icon = (PackIcon) button.FindName("CompletedIcon");

            if (icon.Kind.Equals(PackIconKind.ArrowDropDown))
            {
                icon.Kind = PackIconKind.ArrowDropUp;
                completedTaskList.Visibility = Visibility.Visible;
            } else
            {
                icon.Kind = PackIconKind.ArrowDropDown;
                completedTaskList.Visibility = Visibility.Collapsed;
            }


        }

        private void SectionID_Drop(object sender, DragEventArgs e)
        {
            if(e.Data != null)
            {
                InsertDroppedTask(e.Data);
            }
        }

        public void InsertDroppedTask(IDataObject dataObject)
        {
            TaskElement element = (TaskElement) dataObject.GetData("Task");
            StackPanel sectionFrom = (StackPanel) dataObject.GetData("SectionFrom");
            sectionFrom.Children.Remove(element);
            SectionElement currentSection = this;
            StackPanel taskList = ((StackPanel)currentSection.Content);
            taskList.Children.Insert(taskList.Children.Count - 1, element);

            TaskDAO taskDAO = new TaskDAO();

            Task task = taskDAO.Read(element.taskID);
            task.SectionID = int.Parse(currentSection.Name.Replace("Section", ""));

            taskDAO.Update(task);
        }
    }
}
