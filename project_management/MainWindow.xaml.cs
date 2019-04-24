using project_management.Windows;
using System.Windows;

namespace project_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new createUser().Show();
            this.Close();
        }
    }
}
