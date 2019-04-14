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
    /// Interaction logic for NewSection.xaml
    /// </summary>
    public partial class NewSectionElement : UserControl
    {
        StackPanel sectionList;

        public NewSectionElement(StackPanel sectionList)
        {
            InitializeComponent();
            this.sectionList = sectionList;
        }

        private void NewSection_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            
            new NewSection(7, sectionList).Show();
        }
    }
}
