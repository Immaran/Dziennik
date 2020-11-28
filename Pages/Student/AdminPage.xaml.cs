using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Student
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Student> StudentList { get; set; }

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
            StudentWindow studentWindow = new StudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == studentWindow.ShowDialog())
            {
                //_context.SaveChanges();
            }
            Load();
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedItem != null)
            {
                StudentWindow studentWindow = new StudentWindow((Models.Student)listbox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == studentWindow.ShowDialog())
                {
                    //_context.SaveChanges();
                }
                Load();
            }
            
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                dynamic student = listbox.SelectedItem;
                int student_id = student.Id;
                _context.LoginData.Remove(_context.LoginData.Single(t => t.Id == student_id));
                _context.SaveChanges();
                Load();
            }
            
        }
        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Load()
        {
            StudentList = _context.Student.ToList();  // wczytanie nauczycieli z bazy danych
            listbox.ItemsSource = StudentList;          // przypisanie listy nauczycieli do listboxa
        }
    }
}
