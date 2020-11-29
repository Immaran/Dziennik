using System;
using System.Linq;
using System.Text;
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

        private readonly Random _rand = new Random(); //static
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
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                if (Teacher == null && LoginData == null) // gdy tworzymy nowego nauczyciela
                {
                    Teacher = new Teacher();
                    Teacher.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Teacher.SecondName = secondName.Text;
                    Teacher.Surname = surname.Text;
                    string login = this.generateLogin(Teacher.FirstName, Teacher.Surname);
                    string password = this.generatePassword();
                    LoginData = new LoginData
                    {
                        Login = login,
                        Password = password,
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

        public string generatePassword()
        {
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "1234567890";
            const string specials = "#$%&!?-*%";

            string[] characters = { lowers, uppers, numbers, specials };

            StringBuilder stringBuilder = new StringBuilder();

            int index;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    index = _rand.Next(characters[j].Length);

                    stringBuilder.Append(characters[j][index]);
                }
            }
            return stringBuilder.ToString();
        }

        public string generateLogin(string name, string surname)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(name[0]).Append(".").Append(surname);
            return stringBuilder.ToString().ToLower();
        }
    }
}
