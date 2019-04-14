﻿using project_management.Windows;
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
        NewTask newTask;
        int userID;
        public UserAvatar(NewTask newTask, int userID)
        {
            this.newTask = newTask;
            this.userID = userID;
            InitializeComponent();
        }

        private void AssignMemberToTask_Click(object sender, RoutedEventArgs e)
        {
            
            newTask.AssignUser(this, userID);
        }
    }
}
