using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Subject> SubjectList { get; set; }

        public AdminPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SubjectList = _context.Subject.ToList();    // wczytanie przedmiotów z bazy danych
            SubjectListBox.ItemsSource = SubjectList;          // przypisanie listy przedmiotów do listboxa
        }
        private void Refresh()
        {
            SubjectList = _context.Subject.ToList();    // wczytanie przedmiotów z bazy danych
            SubjectListBox.ItemsSource = SubjectList;          // przypisanie listy przedmiotów do listboxa
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            SubjectWindow subjectWindow = new SubjectWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == subjectWindow.ShowDialog())
            {
                this.Refresh();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if (SubjectListBox.SelectedItem != null)
            {
                SubjectWindow subjectWindow = new SubjectWindow((Models.Subject)SubjectListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == subjectWindow.ShowDialog())
                {
                    this.Refresh();
                }
            }
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if(SubjectListBox.SelectedItem != null)
            {
                _context.Subject.Remove((Models.Subject)SubjectListBox.SelectedItem);
                _context.SaveChanges();
                this.Refresh();
            }
        }
        private void ClickGoBack(object sender, RoutedEventArgs e)
        {
            //_context.SaveChanges();
            this.NavigationService.GoBack();
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectListBox.SelectedItem != null)
            {
                Edit.IsEnabled = true;
                Remove.IsEnabled = true;
            }
            else
            {
                Edit.IsEnabled = false;
                Remove.IsEnabled = false;
            }
        }
    }
}
