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

namespace project_management
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {
        public Overview()
        {
            InitializeComponent();
            List<Userss> items = new List<Userss>();
            items.Add(new Userss() { Name = "John Doe", Age = 42 });
            items.Add(new Userss() { Name = "Jane Doe", Age = 39 });
            items.Add(new Userss() { Name = "Sammy Doe", Age = 13 });
            lvDataBinding.ItemsSource = items;
        }
    }

    public class Userss
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
