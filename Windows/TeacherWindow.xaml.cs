using System;
using System.Linq;
using System.Security.Cryptography;
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
        public TeacherWindow() // konstrukor gdy dodajemy nowego nauczyciela
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
            if (Teacher != null)        // jezeli dane sa do edycji
            {
                name.Text = Teacher.FirstName;
                if (Teacher.SecondName != null)
                    secondName.Text = Teacher.SecondName;
                surname.Text = Teacher.Surname;
                Teacher.IdNavigation = _context.LoginData.First(x => x.Id == Teacher.Id);
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
                if(name.Text.Length > 20 || surname.Text.Length > 20 || secondName.Text.Length > 20)
                {
                    MessageBox.Show("Maksymalnie można wprowadzić 20 znaków w jednym polu", "Nauczyciel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Teacher == null && LoginData == null) // gdy tworzymy nowego nauczyciela
                {
                    Teacher = new Teacher();
                    Teacher.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Teacher.SecondName = secondName.Text;
                    Teacher.Surname = surname.Text;
                    string login = this.generateLogin(Teacher.FirstName, Teacher.Surname);
                    string password = this.generatePassword();
                    MessageBox.Show("Generated password: " + password);
                    Clipboard.SetText(password);
                    string hashedPassword = this.hashPassword(password);
                    LoginData = new LoginData
                    {
                        Login = login,
                        Password = hashedPassword,
                        Role = "teacher"
                    };
                    _context.LoginData.Add(LoginData);
                    this.SaveDB();
                    LoginData = _context.LoginData.First(x => x.Login == LoginData.Login);
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

                this.SaveDB();

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

        public string hashPassword(string password)
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
    }
}
