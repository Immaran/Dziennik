using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;
using System;
using System.Windows.Documents;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private readonly ModelContext _context;
        private Student Student { get; set; }
        private LoginData LoginData { get; set; }

        private readonly Random _rand = new Random(); //static
        public StudentWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public StudentWindow(Student student) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Student = student;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            if (Student != null)
            {
                name.Text = Student.FirstName;
                if (Student.SecondName != null)
                    secondName.Text = Student.SecondName;
                surname.Text = Student.Surname;
                Student.IdNavigation = _context.LoginData.FirstOrDefault(x => x.Id == Student.Id);
                
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                if (Student == null && LoginData == null) // gdy tworzymy nowego ucznia
                {
                    Student = new Student();
                    Student.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Student.SecondName = secondName.Text;
                    Student.Surname = surname.Text;
                    string login = this.generateLogin(Student.FirstName, Student.Surname);
                    string password = this.generatePassword();
                    LoginData = new LoginData
                    {
                        Login = login,
                        Password = password,
                        Role = "student"
                    };
                    _context.LoginData.Add(LoginData);
                    _context.SaveChanges();
                    LoginData = _context.LoginData.FirstOrDefault(x => x.Login == LoginData.Login);
                    Student.Id = LoginData.Id;
                    Student.IdNavigation = LoginData;
                    _context.Student.Add(Student);
                }
                else // gdy edytujemy dane ucznia
                {
                    Student.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Student.SecondName = secondName.Text;
                    Student.Surname = surname.Text;
                    _context.Attach(Student).State = EntityState.Modified;
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
                MessageBox.Show("Brak wszystkich danych", "Uczeń", MessageBoxButton.OK, MessageBoxImage.Warning);
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
