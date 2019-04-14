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
    }
}
