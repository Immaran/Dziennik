using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private readonly ModelContext _context;
        private Teacher Teacher { get; set; }
        private LoginData LoginData { get; set; }
        public TeacherWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public TeacherWindow(Teacher teacher) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Teacher = teacher;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            if (Teacher != null)
            {
                name.Text = Teacher.FirstName;
                if (Teacher.SecondName != null)
                    secondName.Text = Teacher.SecondName;
                surname.Text = Teacher.Surname;
                Teacher.IdNavigation = _context.LoginData.FirstOrDefault(x => x.Id == Teacher.Id);
                login.Text = Teacher.IdNavigation.Login;
                password.Password = Teacher.IdNavigation.Password;
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0 && login.Text != null && password.Password != null)
            {
                if (Teacher == null && LoginData == null) // gdy tworzymy nowego nauczyciela
                {
                    Teacher = new Teacher();
                    Teacher.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Teacher.SecondName = secondName.Text;
                    Teacher.Surname = surname.Text;
                    LoginData = new LoginData
                    {
                        Login = login.Text,
                        Password = password.Password,
                        Role = "teacher"
                    };
                    _context.LoginData.Add(LoginData);
                    _context.SaveChanges();
                    LoginData = _context.LoginData.FirstOrDefault(x => x.Login == LoginData.Login);
                    Teacher.Id = LoginData.Id;
                    Teacher.IdNavigation = LoginData;
                    _context.Teacher.Add(Teacher);
                }
                else // gdy edytujemy dane nauczyciela
                {
                    Teacher.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Teacher.SecondName = secondName.Text;
                    Teacher.Surname = surname.Text;
                    Teacher.IdNavigation.Login = login.Text;
                    Teacher.IdNavigation.Password = password.Password;
                    _context.Attach(Teacher).State = EntityState.Modified;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //_context.SaveChanges();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Nauczyciel", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
