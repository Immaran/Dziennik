using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Message
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        private List<Models.Message> MessageList { get; set; }
        public TeacherPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void fetchData()
        {
            //wczytanie wiadomości z serwera
            Models.Teacher teacher = ((MainWindow)Application.Current.MainWindow).loggedUser;
            MessageList = new List<Models.Message>();
            foreach (Models.Message message in _context.Message)
            {
                if (message.Teacher == teacher)
                {
                    MessageList.Add(message);
                }
            }
            MessageBox.ItemsSource = MessageList;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.fetchData();
        }
        private void CreateMessage(object sender, RoutedEventArgs e)
        {
            MessageWindow messageWindow = new MessageWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == messageWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
    }
}
