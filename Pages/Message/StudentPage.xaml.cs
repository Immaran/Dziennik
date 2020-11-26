using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Message
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        private readonly ModelContext _context;
        //private IList<Models.Message> MessageList { get; set; }
        public StudentPage()
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
