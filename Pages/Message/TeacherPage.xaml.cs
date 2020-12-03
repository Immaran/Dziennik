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
        private List<Models.Message> MessagesList = new List<Models.Message>();
        private readonly string type;       // okresla czy wyswietlic wiadomosci wyslane czy odebrane
        public TeacherPage(string type)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.type = type;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.fetchData();
        }
        private void fetchData() // wczytanie wiadomości z serwera
        {
            // obecnie zalogowany student
            Models.Teacher teacher = ((MainWindow)Application.Current.MainWindow).loggedUser;

            // jeśli odebrane
            if (type == "received")
            {
                MessagesList.Clear();
                foreach (Models.Message message in _context.Message)
                {
                    if (message.Teacher == teacher)
                    {
                        if (message.SenderId != teacher.Id)
                        {
                            MessagesList.Add(message);
                        }
                    }
                }
                MessageBox.ItemsSource = MessagesList;
                MainLabel.Content = "Odebrane";
            }
            // jeśli wysłane
            else if (type == "sent")
            {
                MessagesList.Clear();
                foreach (Models.Message message in _context.Message)
                {
                    if (message.Teacher == teacher)
                    {
                        if (message.SenderId == teacher.Id)
                        {
                            MessagesList.Add(message);
                        }
                    }
                }
                MessageBox.ItemsSource = MessagesList;
                MainLabel.Content = "Wysłane";
            }
        }
        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            this.fetchData();
        }
    }
}
