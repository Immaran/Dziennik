using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;

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
            TeacherList = _context.Teacher.ToList();    // wczytanie nauczycieli z bazy danych
            listbox.ItemsSource = TeacherList;          // przypisanie listy nauczycieli do listboxa
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
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedItem != null)
            {
                TeacherWindow teacherWindow = new TeacherWindow((Models.Teacher)listbox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == teacherWindow.ShowDialog())
                {
                    //_context.SaveChanges();
                }
            }
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedItem != null)
            {
                _context.Teacher.Remove((Models.Teacher)listbox.SelectedItem);
                _context.SaveChanges();
            }
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            //_context.SaveChanges();
            this.NavigationService.GoBack();
        }
    }
}
