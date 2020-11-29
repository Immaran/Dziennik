using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBD.Pages.StudentHome
{
    /// <summary>
    /// Interaction logic for HomeforStudent.xaml
    /// </summary>
    public partial class HomeForStudent : Page
    {
        public HomeForStudent()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sendet, RoutedEventArgs e)
        {
            Models.Student student = ((MainWindow)Application.Current.MainWindow).loggedUser;
            mainLabel.Content = "Strona główna studenta " + student.FirstName + " " + student.Surname;
        }
    }
}
