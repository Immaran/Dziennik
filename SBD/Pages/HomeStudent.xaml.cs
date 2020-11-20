﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBD.Pages
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
            navigator.Navigate(new HomeforStudent());
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
        private void ClickMessage(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Message.StudentPage());
        }
        private void ClickSubject(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Subject.StudentPage());
        }
        private void ClickLoginData(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new LoginData.StudentPage());
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void ClickLogOut(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
