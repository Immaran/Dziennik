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
        private void fetchData()
        {
            StudentList = _context.Student.ToList();        // loading students from database
            StudentListBox.ItemsSource = StudentList;       // assigning a student list to a listbox
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.fetchData();
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == studentWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            if(StudentListBox.SelectedItem != null)
            {
                StudentWindow studentWindow = new StudentWindow((Models.Student)StudentListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == studentWindow.ShowDialog())
                {
                    this.fetchData();
                }
            }
            
        }
        private void ClickRemove(object sender, RoutedEventArgs e)
        {
            if (StudentListBox.SelectedItem != null)
            {
                dynamic student = StudentListBox.SelectedItem;
                int student_id = student.Id;
                _context.LoginData.Remove(_context.LoginData.Single(t => t.Id == student_id));
                _context.SaveChanges();
                this.fetchData();
            }
            
        }
        private void ClickGoBack(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentListBox.SelectedItem != null)
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
