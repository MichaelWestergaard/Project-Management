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
    /// Interaction logic for UserAvatar.xaml
    /// </summary>
    public partial class UserAvatar : UserControl
    {
        Object senderObject;
        int userID;
        public UserAvatar(Object senderObject, int userID)
        {
            this.senderObject = senderObject;
            this.userID = userID;
            InitializeComponent();
        }

        private void AssignMemberToTask_Click(object sender, RoutedEventArgs e)
        {
            if(senderObject is NewTask)
                ((NewTask)senderObject).AssignUser(this, userID);
            if (senderObject is EditTask)
                ((EditTask)senderObject).AssignUser(this, userID);
        }
    }
}
