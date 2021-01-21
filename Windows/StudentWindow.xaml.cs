using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;
using System;
using System.Security.Cryptography;
using System.Windows.Input;

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
        public StudentWindow() // konstrukor gdy dodajemy nowego ucznia
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
            name.Focus();
        }
        public StudentWindow(Student student) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Student = student;
            InitializeComponent();
            name.Focus();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Student != null)    // jezeli dane sa do edycji
            {
                name.Text = Student.FirstName;
                if (Student.SecondName != null)
                    secondName.Text = Student.SecondName;
                surname.Text = Student.Surname;
                Student.IdNavigation = _context.LoginData.First(x => x.Id == Student.Id);
                
            }
        }
        private void SaveDB()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                if (name.Text.Length > 20 || surname.Text.Length > 20 || secondName.Text.Length > 20)
                {
                    MessageBox.Show("Maksymalnie można wprowadzić 20 znaków w jednym polu", "Uczeń", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Student == null && LoginData == null) // gdy tworzymy nowego ucznia
                {
                    Student = new Student();
                    Student.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Student.SecondName = secondName.Text;
                    Student.Surname = surname.Text;
                    string login = this.generateLogin(Student.FirstName, Student.Surname);
                    string password = this.generatePassword();
                    MessageBox.Show("Generated password: "+ password);
                    Clipboard.SetText(password);
                    string hashedPassword = this.hashPassword(password);
                    LoginData = new LoginData
                    {
                        Login = login,
                        Password = hashedPassword,
                        Role = "student"
                    };
                    _context.LoginData.Add(LoginData);
                    this.SaveDB();
                    LoginData = _context.LoginData.First(x => x.Login == LoginData.Login);
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

                this.SaveDB();

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

        public string hashPassword (string password)
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value
                rngCsp.GetBytes(salt);
            }
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); //Password-Based Key Derivation Function 2; 10k iterations
            byte[] hash = pbkdf2.GetBytes(20); //Get pseudorandomkey
            byte[] combinedBytes = new byte[36];
            Array.Copy(salt, 0, combinedBytes, 0, 16);
            Array.Copy(hash, 0, combinedBytes, 16, 20);
            string hashedPassword = Convert.ToBase64String(combinedBytes);
            return hashedPassword;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch(Exception)
            {
                // do not throw
            }
        }
    }
}
