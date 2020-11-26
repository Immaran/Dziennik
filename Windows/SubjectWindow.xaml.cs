using System.Windows;
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
            if (name.Text.Length > 0 && teacher.SelectedItem != null && group.SelectedItem != null)
            {
                if (Subject == null) // gdy tworzymy nową grupę
                {
                    Subject = new Subject();
                    Subject.Name = name.Text;

                    /// tu należy przypisać nauczyciela
                    /// Subject.TeacherId = ??

                    /// tu należy przypisać grupę

                    _context.Subject.Add(Subject);
                }
                else // gdy edytujemy dane przedmiotu
                {
                    Subject.Name = name.Text;

                    /// tu należy przypisać nauczyciela
                    /// Subject.TeacherId = ??

                    /// tu należy przypisać grupę

                    _context.Subject.Add(Subject);

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

                //_context.SaveChanges();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Przedmiot", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            if (Subject != null)
            {
                name.Text = Subject.Name;

                /// tu należy przypisać nauczyciela
                /// teacher.SelectedItem = ??

                /// tu należy przypisać grupę
                /// group.SelectedItem = ??
            }
        }
    }
}
