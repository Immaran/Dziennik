using System;
using System.Windows;
using System.Windows.Controls;
using SBD.Pages.Student;
using SBD.Pages.StudentHome;
using SBD.Pages.TeacherHome;


namespace SBD.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void onLogin(object sender, EventArgs e)
        {
            if(login.Text=="admin" && password.Password == "admin")
            {
                this.NavigationService.Navigate(new HomeAdmin());
            }
            else if(login.Text=="student" && password.Password == "student")
            {
                this.NavigationService.Navigate(new HomeStudent());
            }
            else if(login.Text == "teacher" && password.Password == "teacher")
            {
                this.NavigationService.Navigate(new HomeTeacher());
            }
            else 
            {
                MessageBox.Show("Błędne nazwa użytkownika lub hasło");
            }
        }
    }
}
