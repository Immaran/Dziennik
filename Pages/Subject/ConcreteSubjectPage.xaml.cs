using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for ConcreteSubjectPage.xaml
    /// </summary>
    public partial class ConcreteSubjectPage : Page
    {
        private readonly ModelContext _context;
        private Models.Subject Subject;
        public ConcreteSubjectPage(Models.Subject subject)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Subject = subject;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // wczytanie danych
            SubjectName.Content = Subject.Id + " - " + Subject.Name;
            GradesListBox.ItemsSource = _context.Grade.Where(g => g.Subject == Subject).ToList();
        }
    }
}
