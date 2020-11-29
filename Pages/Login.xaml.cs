using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;
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
        private readonly ModelContext _context;
        private bool logged = false;
        public Login()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            ((MainWindow)Application.Current.MainWindow).loggedUser = null;
            InitializeComponent();
        }
        private void onLogin(object sender, EventArgs e)
        {
            if (login.Text.Length > 0 && password.Password.Length > 0)
            {
                if (login.Text == "admin" && password.Password == "admin")
                {
                    this.NavigationService.Navigate(new HomeAdmin());
                    logged = true;
                }
                else
                {
                    Models.LoginData ldata = _context.LoginData.SingleOrDefault(ld => ld.Login == login.Text);
                    if (ldata != null)
                    {
                        if (ldata.Password == password.Password)
                        {
                            if (ldata.Role.Equals("student"))
                            {
                                ((MainWindow)Application.Current.MainWindow).loggedUser = _context.Student.SingleOrDefault(s => s.Id == ldata.Id);
                                this.NavigationService.Navigate(new HomeStudent());
                            }
                            else if (ldata.Role.Equals("teacher"))
                            {
                                ((MainWindow)Application.Current.MainWindow).loggedUser = _context.Teacher.SingleOrDefault(t => t.Id == ldata.Id);
                                this.NavigationService.Navigate(new HomeTeacher());
                            }
                            logged = true;
                        }
                    }
                }
                if (logged == false)
                {
                    MessageBox.Show("Błędne nazwa użytkownika lub hasło");
                }
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych");
            }
        }
    }
}
