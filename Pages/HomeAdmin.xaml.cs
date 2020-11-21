using System.Windows;
using System.Windows.Controls;

namespace SBD.Pages
{
    /// <summary>
    /// Interaction logic for HomeAdmin.xaml
    /// </summary>
    public partial class HomeAdmin : Page
    {
        public HomeAdmin()
        {
            InitializeComponent();
        }
        public void ClickLoginData(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginData.AdminPage());
        }
        public void CLickGroup(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Group.AdminPage());
        }
        public void ClickSubject(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Subject.AdminPage());
        }
        public void ClickStudent(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Student.AdminPage());
        }
        public void ClickTeacher(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Teacher.AdminPage());
        }
        private void ClickLogOut(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
