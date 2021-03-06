﻿using SBD.Windows;
using System;
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

namespace SBD.Pages.TeacherHome
{
    /// <summary>
    /// Interaction logic for HomeTeacher.xaml
    /// </summary>
    public partial class HomeTeacher : Page
    {
        public HomeTeacher()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sendet, RoutedEventArgs e)
        {
            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
            navigator.Navigate(new HomeForTeacher()); 
        }
        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Event.TeacherPage());
        }
        private void ClickMessageSent(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Message.TeacherPage("sent"));
        }
        private void ClickMessageRecived(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Message.TeacherPage("received"));
        }
        private void ClickNewMessage(object sender, RoutedEventArgs e)
        {
            MessageTeacherWindow messageWindow = new MessageTeacherWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == messageWindow.ShowDialog())
            {

            }
        }
        private void ClickSubject(object sender, RoutedEventArgs e)
        {
            navigator.Navigate(new Subject.TeacherPage());
        }
        private void ClickLoginData(object sender, RoutedEventArgs e)
        {
            LoginDataTeacherWindow logindataWindow = new LoginDataTeacherWindow
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
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void ClickGoForward(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoForward)
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
