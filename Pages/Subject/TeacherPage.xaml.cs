using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Subject> SubjectList { get; set; }

        private readonly Models.Teacher Teacher = (Models.Teacher)((MainWindow)Application.Current.MainWindow).loggedUser;
        public TeacherPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //wczytanie danych
            SubjectList = _context.Subject.Where(x => x.Teacher == Teacher).ToList();
            SubjectListBox.ItemsSource = SubjectList;
        }
    }
}
