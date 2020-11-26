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
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                if (Student == null) // gdy tworzymy nowego ucznia
                {
                    Student = new Student();
                    Student.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Student.SecondName = secondName.Text;
                    Student.Surname = surname.Text;
                    /// tu należy przypisać dane logowania dla nowo utworzonego ucznia
                    /// ja widzę 4 wyjścia 
                    /// 1 - albo nowe okno gdzie wprowadzamy jakieś dane logowania
                    /// 2 - albo wbijamy coś na sztywno, a potem to edytujemy ręcznie w zarządzaniu danymi logowania
                    /// 3 - generujemy login i hasło automatycznie i przekazujemy dla ucznia, a potem hasło będzie mógł zmienić
                    /// 4 - dodajemy nowe pola w tym oknie czyli login i hasło i je uzupełniamy 
                    /// Student.IdNavigation = ??? new LoginData();
                    _context.Student.Add(Student);
                }
                else // gdy edytujemy dane nauczyciela
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            if (Student != null)
            {
                name.Text = Student.FirstName;
                if (Student.SecondName != null)
                    secondName.Text = Student.SecondName;
                surname.Text = Student.Surname;
            }
        }
    }
}
