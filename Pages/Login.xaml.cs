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
        public Login()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();

        }

        private void onLogin(object sender, EventArgs e)
        {
            if(login.Text=="admin" || password.Password == "admin")
            {
                this.NavigationService.Navigate(new HomeAdmin());
            }

            Models.LoginData ldata = _context.LoginData.SingleOrDefault(ld => ld.Login == login.Text);
            if (ldata!=null)
            {
               if (ldata.Password==password.Password)
               {
                    //((MainWindow)Application.Current.MainWindow).loggedId = ldata.Id;
                    if (ldata.Role.Equals("student"))
                    {
                        ((MainWindow)Application.Current.MainWindow).loggedUser = _context.Student.SingleOrDefault(s => s.Id == ldata.Id);
                        this.NavigationService.Navigate(new HomeStudent());
                        return;
                    }
                    ((MainWindow)Application.Current.MainWindow).loggedUser = _context.Teacher.SingleOrDefault(t => t.Id == ldata.Id);
                    this.NavigationService.Navigate(new HomeTeacher());
               }
            }

            else 
            {
                MessageBox.Show("Błędne nazwa użytkownika lub hasło");
            }
        }
    }
}
