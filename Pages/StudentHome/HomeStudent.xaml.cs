﻿using SBD.Windows;
using System.Windows;
using System.Windows.Controls;


namespace SBD.Pages.StudentHome
{
    /// <summary>
    /// Interaction logic for HomeStudent.xaml
    /// </summary>
    public partial class HomeStudent : Page
    {
        public HomeStudent()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sendet, RoutedEventArgs e)
        {
            while(this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
            navigator.Navigate(new HomeForStudent());
        }
        private void ClickGrade(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Grade.StudentPage());
        }
        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Event.StudentPage());
        }
        private void ClickGroup(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Group.StudentPage());
        }
        private void ClickMessageSent(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Message.StudentPage("sent"));
        }
        private void ClickMessageRecived(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Message.StudentPage("received"));
        }
        private void ClickNewMessage(object sender, RoutedEventArgs e)
        {
            MessageStudentWindow messageWindow = new MessageStudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == messageWindow.ShowDialog())
            {

            }
        }
        private void ClickLoginData(object sender, RoutedEventArgs e)
        {
            LoginDataStudentWindow logindataWindow = new LoginDataStudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == logindataWindow.ShowDialog())
            {
                //_context.SaveChanges();
            }
        }

        private void ClickGoBack(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void ClickGoForward(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }
        }
        private void ClickLogOut(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
