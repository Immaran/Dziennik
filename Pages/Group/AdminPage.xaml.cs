using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;
using Microsoft.EntityFrameworkCore;

namespace SBD.Pages.Group
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Group> GroupList { get; set; }

        public AdminPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GroupList = _context.Group.ToList();    // wczytanie grup z bazy danych
            listbox.ItemsSource = GroupList;        // przypisanie listy grup do listboxa
        }
        private void fetchData()
        {
            _context.Teacher.Load(); // wczytanie nauczycieli, aby wyświetlać całe nazwy przedmiotów
            GroupList = _context.Group.ToList();    // wczytanie grup z bazy danych
            listbox.ItemsSource = GroupList;        // przypisanie listy grup do listboxa
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            GroupWindow groupWindow = new GroupWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == groupWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedItem != null)
            {
                GroupWindow groupWindow = new GroupWindow((Models.Group)listbox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == groupWindow.ShowDialog())
                {
                    this.fetchData();
                }
            }
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                _context.Group.Remove((Models.Group)listbox.SelectedItem);
                _context.SaveChanges();
                this.fetchData();
            }
        }
        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
