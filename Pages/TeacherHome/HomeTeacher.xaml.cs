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
        private Button clicked = null;
        public HomeTeacher()
        {
            InitializeComponent();
        }
        private void update_button(Button btn)
        {
            if (clicked != null)
                clicked.IsEnabled = true;
            clicked = btn;
            clicked.IsEnabled = false;
        }
        private void Page_Loaded(object sendet, RoutedEventArgs e)
        {
            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
            navigator.Navigate(new HomeForTeacher()); 
        }
        private void ClickGrade(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new Grade.TeacherPage());
        }
        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new Event.TeacherPage());
        }
        private void ClickGroup(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new Group.TeacherPage());
        }
        private void ClickMessage(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new Message.TeacherPage());
        }
        private void ClickSubject(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new Subject.TeacherPage());
        }
        private void ClickLoginData(object sender, RoutedEventArgs e)
        {
            update_button((Button)sender);
            navigator.Navigate(new LoginData.TeacherPage());
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
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
