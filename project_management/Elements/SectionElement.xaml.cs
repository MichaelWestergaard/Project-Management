using MaterialDesignThemes.Wpf;
using project_management.DAO;
using project_management.Windows;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for SectionElement.xaml
    /// </summary>
    public partial class SectionElement : UserControl
    {
        StackPanel sectionList;
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
            StackPanel completedTaskList = (StackPanel)((Grid)button.Parent).FindName("CompletedTaskList");

            PackIcon icon = (PackIcon) button.FindName("CompletedIcon");

            if (icon.Kind.Equals(PackIconKind.ArrowDropDown))
            {
                icon.Kind = PackIconKind.ArrowDropUp;
                completedTaskList.Visibility = Visibility.Visible;
            } else
            {
                icon.Kind = PackIconKind.ArrowDropDown;
                completedTaskList.Visibility = Visibility.Hidden;
            }


        }
    }
}
