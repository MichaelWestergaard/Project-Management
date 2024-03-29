﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using project_management.DTO;
using project_management.Elements;
using project_management.DAO;
using ToastNotifications.Messages;
using project_management.Controllers;

namespace project_management.Windows
{
    /// <summary>
    /// Interaction logic for AddWorkLog.xaml
    /// </summary>
    public partial class AddWorkLog : Window
    {
        private Task task;
        private TaskDAO taskDAO = new TaskDAO();
        private WorkLogDAO workLogDAO = new WorkLogDAO();
        private TaskElement taskElement;
        private Board board = new Board();
        private User user;
        StackPanel taskList;
        StackPanel completedTaskList;
        Utilities utilities = new Utilities();

        public AddWorkLog(TaskElement taskElement)
        {
            InitializeComponent();

            try
            {
                user = MainController.Instance.User;
                this.taskElement = taskElement;
                task = taskDAO.Read(taskElement.taskID);
            
                date.Text = DateTime.Now.Date.ToString();
           
                if (!task.Completed)
                {
                    taskList = (StackPanel)taskElement.Parent;
                    StackPanel lastStackPanel = (StackPanel)((StackPanel)taskElement.Parent).Children[taskList.Children.Count - 1];
                    completedTaskList = ((StackPanel)lastStackPanel.Children[lastStackPanel.Children.Count - 1]);
                } else
                {
                    completedTaskList = (StackPanel)taskElement.Parent;
                    taskList = ((StackPanel)((StackPanel)completedTaskList.Parent).Parent);
                }
            
                if(task.Completed)
                {
                    CompleteTask.Content = "Start Opgave";
                    CompleteTask.Background = new Utilities().GetColor("#FF2196F3");
                }
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtnUpdateWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {

                    double workLoad = Double.Parse(estimation.Text);
                    DateTime workDato = DateTime.Parse(date.Text);


                    WorkLog workLog = new WorkLog(user, task.Id, workLoad, workDato);

                    int workLogID = workLogDAO.CreateWork(workLog);

                    taskElement.UpdateProgress((workLogDAO.GetWorkSum(task.Id) / task.EstimatedTime) * 100);
                    MainController.Instance.Dashboard.UpdatePage();
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }
        }

        private void ButtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (task.Completed == false) {
                task.Completed = true;
                taskDAO.Update(task);
                taskElement.Opacity = 0.5;
                taskList.Children.Remove(taskElement);
                completedTaskList.Children.Add(taskElement);
            }
            else
            {
                task.Completed = false;
                taskDAO.Update(task);
                taskElement.Opacity = 1;
                completedTaskList.Children.Remove(taskElement);
                taskList.Children.Insert(taskList.Children.Count - 1, taskElement);
            }

            try
            {
                MainController.Instance.Dashboard.UpdatePage();
                this.Close();
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }
        }
        
        private bool ValidateInput()
        {
            try
            {
                DateTime dateCheck = Convert.ToDateTime(date.Text);
                if (estimation.Text != "" && double.TryParse(estimation.Text, out double resultEstimation))
                {
                    if (date.Text != "")
                    {
                        if (dateCheck <= DateTime.Today)
                        {
                            if (DateTime.TryParse(date.Text, out DateTime result))
                            {

                                return true;
                            }
                            else
                            {
                                new Utilities().GetNotifier().ShowError("Vælg venligst en korrekt dato");
                            }
                        }
                        else
                        {
                            new Utilities().GetNotifier().ShowError("Forkert dato");
                        }
                    }
                    else
                    {
                        new Utilities().GetNotifier().ShowError("Udfyld deadline dato");
                    }
                }
                else
                {
                    new Utilities().GetNotifier().ShowError("Udfyld estimation");
                }
            }
            catch (Exception exception)
            {
                utilities.GetNotifier().ShowError(utilities.HandleException(exception));
            }

            return false;
        }
    }
}
           
