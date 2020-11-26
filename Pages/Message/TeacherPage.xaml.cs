using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Message
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        //private IList<Models.Message> MessageList { get; set; }
        public TeacherPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //wczytanie danych
        }
    }
}
