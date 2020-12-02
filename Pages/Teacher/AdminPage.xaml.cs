using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Collections.Immutable;

namespace SBD.Pages.Teacher
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Teacher> TeacherList { get; set; }
        public AdminPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            TeacherWindow teacherWindow = new TeacherWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == teacherWindow.ShowDialog())
            {
                //_context.SaveChanges();
            }
            Load();

        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if(TeacherListBox.SelectedItem != null)
            {
                TeacherWindow teacherWindow = new TeacherWindow((Models.Teacher)TeacherListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == teacherWindow.ShowDialog())
                {
                    //_context.SaveChanges();
                }
                Load();
            }
            
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if(TeacherListBox.SelectedItem != null)
            {
                dynamic teacher = TeacherListBox.SelectedItem;
                int teacher_id = teacher.Id;
                _context.LoginData.Remove(_context.LoginData.Single(t => t.Id == teacher_id));
                _context.SaveChanges();
                Load();
            }
        }

        private void ClickGoBack(object sender, RoutedEventArgs e)
        {
            //_context.SaveChanges();
            this.NavigationService.GoBack();
        }

        private void Load()
        {
            TeacherList = _context.Teacher.ToList();  // wczytanie nauczycieli z bazy danych
            TeacherListBox.ItemsSource = TeacherList;          // przypisanie listy nauczycieli do listboxa
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeacherListBox.SelectedItem != null)
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
