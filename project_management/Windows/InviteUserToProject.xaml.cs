﻿using System;
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

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for InviteUserToProject.xaml
    /// </summary>
    public partial class InviteUserToProject : Window
    {
        UserDAO userDAO;
        public InviteUserToProject()
        {
            InitializeComponent();
            userDAO = new UserDAO();
        }
        
        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Member_Click(object sender, RoutedEventArgs e)
        {
            string email = this.email.Text;

            if (Utilities.IsValidEmail(email))
            {
                if(userDAO)
            }
        }
    }
}
