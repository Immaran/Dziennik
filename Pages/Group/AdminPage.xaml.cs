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
            this.fetchData();   // pobranie danych z serwera
        }
        private void fetchData()
        {
            _context.Teacher.Load();                // wczytanie nauczycieli, aby wyświetlać całe nazwy przedmiotów
            GroupList = _context.Group.ToList();    // wczytanie grup z bazy danych
            GroupListBox.ItemsSource = GroupList;   // przypisanie listy grup do listboxa
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
            if(GroupListBox.SelectedItem != null)
            {
                GroupWindow groupWindow = new GroupWindow((Models.Group)GroupListBox.SelectedItem)
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
            if (GroupListBox.SelectedItem != null)
            {
                _context.Group.Remove((Models.Group)GroupListBox.SelectedItem);
                _context.SaveChanges();
                this.fetchData();
            }
        }
        private void ClickGoBack(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupListBox.SelectedItem != null)
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
