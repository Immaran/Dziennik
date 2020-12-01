using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;
using Microsoft.EntityFrameworkCore;

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
            listbox.ItemsSource = SubjectList;          // przypisanie listy przedmiotów do listboxa
        }
        private void fetchData()
        {
            _context.Teacher.Load();                    // wczytanie nauczycieli, aby wyświetlać całe nazwy przedmiotów
            SubjectList = _context.Subject.ToList();    // wczytanie przedmiotów z bazy danych
            listbox.ItemsSource = SubjectList;          // przypisanie listy przedmiotów do listboxa
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            SubjectWindow subjectWindow = new SubjectWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == subjectWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                SubjectWindow subjectWindow = new SubjectWindow((Models.Subject)listbox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == subjectWindow.ShowDialog())
                {
                    this.fetchData();
                }
            }
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedItem != null)
            {
                _context.Subject.Remove((Models.Subject)listbox.SelectedItem);
                _context.SaveChanges();
                this.fetchData();
            }
        }
        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            //_context.SaveChanges();
            this.NavigationService.GoBack();
        }
    }
}
