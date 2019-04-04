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
using System.Windows.Shapes;

namespace project_management
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
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
                    AppContent.Background = Brushes.AliceBlue;
                    break;

                case 1:
                    AppContent.Background = Brushes.Azure;
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
            new CreateProject().Show();
        }
    }
}
