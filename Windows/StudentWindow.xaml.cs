using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;


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
                login.Text = Student.IdNavigation.Login;
                password.Password = Student.IdNavigation.Password;
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0 && login.Text != null && password.Password != null)
            {
                if (Student == null && LoginData == null) // gdy tworzymy nowego ucznia
                {
                    Student = new Student();
                    Student.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Student.SecondName = secondName.Text;
                    Student.Surname = surname.Text;
                    LoginData = new LoginData
                    {
                        Login = login.Text,
                        Password = password.Password,
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
                    Student.IdNavigation.Login = login.Text;
                    Student.IdNavigation.Password = password.Password;
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
    }
}
