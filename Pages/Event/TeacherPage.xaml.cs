using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Event
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Event> EventList { get; set; }

        // obecnie zalogowany nauczyciel
        private readonly Models.Teacher Teacher = (Models.Teacher)((MainWindow)Application.Current.MainWindow).loggedUser;
        public TeacherPage()
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
            _context.Teacher.Load();
            EventList = _context.Event.OrderByDescending(e => e.Date).ToList();
            if (FilterCheckBox.IsChecked == true)
            {
                EventListBox.ItemsSource = EventList.Where(e => e.TeacherId == Teacher.Id).ToList();
            }
            else
            {
                EventListBox.ItemsSource = EventList;
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
        private void CreateEvent(object sender, RoutedEventArgs e)
        {
            EventWindow eventWindow = new EventWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == eventWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
        private void EditEvent(object sender, RoutedEventArgs e)
        {
            if (EventListBox.SelectedItem != null)
            {
                EventWindow eventWindow = new EventWindow((Models.Event)EventListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == eventWindow.ShowDialog())
                {
                    this.fetchData();
                }
            }
        }
        private void RemoveEvent(object sender, RoutedEventArgs e)
        {
            if(EventListBox.SelectedItem != null)
            {
                _context.Event.Remove((Models.Event)EventListBox.SelectedItem);
                this.SaveDB();
                this.fetchData();
            }
        }

        private void EventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(EventListBox.SelectedItem != null && VerifyTeacher())
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

        private bool VerifyTeacher()
        {
            Models.Event e = EventListBox.SelectedItem as Models.Event;
            // jezeli obecnie zalogowany nauczyciel to ten, ktory utworzyl wydarzenie
            if(e.Teacher.Id == Teacher.Id)
            {
                return true;
            }
            else
                return false;
        }

        private void SortClick(object sender, RoutedEventArgs e)
        {
            if(FilterCheckBox.IsChecked == true)
            {
                EventListBox.ItemsSource = EventList.Where(e => e.TeacherId == Teacher.Id).ToList();
            }
            else
            {
                EventListBox.ItemsSource = EventList;
            }
        }
    }
}
