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
        public SubjectWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public SubjectWindow(Subject subject) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Subject = subject;
            InitializeComponent();
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
                MessageBox.Show("Brak wszystkich danych", "Przedmiot", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TeacherList = _context.Teacher.ToList();    // wczytanie nauczycieli z bazy danych
            teacher.ItemsSource = TeacherList;          // przypisanie listy nauczycieli do comboboxa
            //if to edit 
            if (Subject != null)
            {
                name.Text = Subject.Name;

                // przypisanie nauczyciela
                teacher.SelectedItem = Subject.Teacher;
            }
        }
    }
}
