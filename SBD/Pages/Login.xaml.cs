using System;
using System.Windows;
using System.Windows.Controls;
using SBD.Pages.Student;


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
            else 
            {
                MessageBox.Show("Błędne nazwa użytkownika lub hasło");
            }
        }
    }
}
