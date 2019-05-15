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

namespace project_management.Elements
{
    /// <summary>
    /// Interaction logic for LoadingModal.xaml
    /// </summary>
    public partial class LoadingModal : Window
    {
        public LoadingModal(object owner)
        {
            InitializeComponent();
            if(owner is Window)
                Owner = (Window) owner;
        }
        
        public void StartLoading()
        {
            Show();
            if (Owner != null)
                Owner.Opacity = 0.5;
        }

        public void StopLoading()
        {
            Close();
            if (Owner != null)
                Owner.Opacity = 1;
        }
    }
}
