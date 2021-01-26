using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private bool adminLoginattempt;
        LinearGradientBrush gradient;
        public Login()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            ((MainWindow)Application.Current.MainWindow).loggedUser = null;
            InitializeComponent();

            login.Focus();

            gradient = new LinearGradientBrush();
            gradient.StartPoint = new System.Windows.Point(0.5, 0);
            gradient.EndPoint = new System.Windows.Point(0.5, 1);
            System.Windows.Media.Color color1 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9a9c8e");
            System.Windows.Media.Color color2 = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#adb09b");
            gradient.GradientStops.Add(new GradientStop(color1, 0));
            gradient.GradientStops.Add(new GradientStop(color2, 1));


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
                        if (this.decrypt(ldata.Password, password.Password))
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

        public bool decrypt(string hashedPassword, string typedPassword)
        {
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(combinedBytes, 0, salt, 0, 16);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(typedPassword, salt, 10000); //Password-Based Key Derivation Function 2; 10k iterations
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] originalHash = new byte[20];
            Array.Copy(combinedBytes, 16, originalHash, 0, 20);
            if (hash.SequenceEqual(originalHash))
                return true;
            return false;
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            string login = ((TextBox)sender).Text;

            if (login == "admin")
            {
                adminLoginattempt = true;
                password.Background = new SolidColorBrush(Colors.Black); ;
                password.Foreground = new SolidColorBrush(Colors.White); ;
            }
            else if (adminLoginattempt == true)
            {
                adminLoginattempt = false;
                if (password.Password == "")
                {
                    
                    password.Background = gradient;
                }
                else
                {
                    password.Background = new SolidColorBrush(Colors.AliceBlue);
                }

                password.Foreground = new SolidColorBrush(Colors.Black); ;

            }
                
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Brush aliceBrush = new SolidColorBrush(Colors.AliceBlue);
            if (login.Text != "admin")
            {
                if (password.Password == "")
                {

                    password.Background = gradient;
                }
                else
                {
                    password.Background = aliceBrush;
                }

                password.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
