using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;


namespace SBD.Pages.Teacher
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly Models.ModelContext _context = new Models.ModelContext();

        private CollectionViewSource teacherViewSource;

        public SBD.Models.Teacher Teacher { get; set; }
        public AdminPage()
        {
            InitializeComponent();
            teacherViewSource = (CollectionViewSource)FindResource(nameof(teacherViewSource));
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // this is for make it easier purposes only
            // to get up and running
            //_context.Database.EnsureCreated();

            // load the entities into EF Core
            _context.Teacher.Load();

            // bind to the source
            teacherViewSource.Source = _context.Teacher.Local.ToObservableCollection();
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            Windows.TeacherWindow teacherWindow = new Windows.TeacherWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == teacherWindow.ShowDialog())
            {
                _context.SaveChanges();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            Windows.TeacherWindow teacherWindow = new Windows.TeacherWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == teacherWindow.ShowDialog())
            {
                _context.SaveChanges();
            }
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            _context.Dispose();
            this.NavigationService.GoBack();
        }
    }
}
