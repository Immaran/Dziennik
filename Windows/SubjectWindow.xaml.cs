using System.Collections.Generic;
using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SBD.Models;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for SubjectWindow.xaml
    /// </summary>
    public partial class SubjectWindow : Window
    {
        private readonly ModelContext _context;
        private Subject Subject { get; set; }
        private IList<Teacher> TeacherList { get; set; }
        public SubjectWindow()  // konstrukor gdy dodajemy nowy przedmiot
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
            name.Focus();
        }
        public SubjectWindow(Subject subject) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Subject = subject;
            InitializeComponent();
            name.Focus();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TeacherList = _context.Teacher.ToList();    // wczytanie nauczycieli z bazy danych
            teacher.ItemsSource = TeacherList;          // przypisanie listy nauczycieli do comboboxa
            
            if (Subject != null)        // jezeli dane sa do edycji
            {
                name.Text = Subject.Name;               // przypisanie nazwy przedmiotu
                teacher.SelectedItem = Subject.Teacher; // przypisanie nauczyciela
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
            if (name.Text.Length > 0 && teacher.SelectedItem != null)
            {
                if (Subject == null) // gdy tworzymy nową grupę
                {
                    Subject = new Subject();
                    Subject.Name = name.Text;

                    // przypisanie nauczyciela
                    Teacher Teacher = (Teacher)teacher.SelectedItem;
                    Subject.TeacherId = Teacher.Id;

                    _context.Subject.Add(Subject);
                }
                else // gdy edytujemy dane przedmiotu
                {
                    Subject.Name = name.Text;

                    // przypisanie nauczyciela
                    Teacher Teacher = (Teacher)teacher.SelectedItem;
                    Subject.TeacherId = Teacher.Id;

                    _context.Attach(Subject).State = EntityState.Modified;
                }

                this.SaveDB();

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Przedmiot", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
