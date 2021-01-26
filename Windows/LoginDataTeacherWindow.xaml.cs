using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using SBD.Models;
using System.Media;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for LoginDataTeacherWindow.xaml
    /// </summary>
    public partial class LoginDataTeacherWindow : Window
    {
        private readonly ModelContext _context;
        public LoginDataTeacherWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
            actualPassword.Focus();
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (actualPassword.Password.Length < 5 || newPassword.Password.Length < 5)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Hasła o długości mniejszej niż 5 znaków nie są akceptowalne.");
                return;
            }

            if (newPassword.Password != repeatedPassword.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Nowe hasło nie zostało poprawnie powtórzone. Spróbuj ponownie. Jeśli stare hasło również zostanie poprawnie zweryfikowane, zostanie zmienione.");
                return;
            }

            if (newPassword.Password == actualPassword.Password)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Nowe hasło nie może być takie samo jak stare. Spróbuj ponownie.");
                return;
            }

            LoginData userLoginData = ((MainWindow)Application.Current.MainWindow).loggedUser.IdNavigation;
            string userPassword = userLoginData.Password;

            if (this.decrypt(userPassword, actualPassword.Password))
            {
                string passwordToChange = hashPassword(newPassword.Password);
                userLoginData.Password = passwordToChange;
                _context.Attach(userLoginData).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                DialogResult = true;
            }

            else
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Dotychczasowe hasło ma inną treść.");
            }
        }

        private bool decrypt(string hashedPassword, string typedPassword)
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
        private string hashPassword(string password)
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
