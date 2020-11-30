using Microsoft.EntityFrameworkCore;
using SBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SBD.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy LoginDataWindow.xaml
    /// </summary>
    public partial class LoginDataWindow : Window
    {
        private readonly ModelContext _context;
        public LoginDataWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (actualPassword.Password.Length < 5 || newPassword.Password.Length < 5)
            {
                MessageBox.Show("Hasła o długości mniejszej niż 5 znaków nie są akceptowalne.");
                return;
            }

            if (newPassword.Password != repeatedPassword.Password)
            {
                MessageBox.Show("Nowe hasło nie zostało poprawnie powtórzone. Spróbuj ponownie. Jeśli stare hasło również zostanie poprawnie zweryfikowane, zostanie zmienione.");
                return;
            }

            //int userId = ((MainWindow)Application.Current.MainWindow).loggedUser.Id;
            //LoginData userLoginData = _context.LoginData.First(l => l.Id == userId);
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
                MessageBox.Show("Dotychczasowe hasło ma inną treść.");
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
