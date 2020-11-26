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
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                if (Teacher == null) // gdy tworzymy nowego nauczyciela
                {
                    Teacher = new Teacher();
                    Teacher.FirstName = name.Text;
                    if (secondName.Text.Length != 0)
                        Teacher.SecondName = secondName.Text;
                    Teacher.Surname = surname.Text;
                    /// tu należy przypisać dane logowania dla nowo utworzonego nauczyciela
                    /// ja widzę 4 wyjścia 
                    /// 1 - albo nowe okno gdzie wprowadzamy jakieś dane logowania
                    /// 2 - albo wbijamy coś na sztywno, a potem to edytujemy ręcznie w zarządzaniu danymi logowania
                    /// 3 - generujemy login i hasło automatycznie i przekazujemy dla nauczyciela, a potem hasło będzie mógł zmienić
                    /// 4 - dodajemy nowe pola w tym oknie czyli login i hasło i je uzupełniamy 
                    /// Teacher.IdNavigation = ??? new LoginData();
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            if(Teacher != null)
            {
                name.Text = Teacher.FirstName;
                if(Teacher.SecondName != null)
                    secondName.Text = Teacher.SecondName;
                surname.Text = Teacher.Surname;
            }
        }
    }
}
