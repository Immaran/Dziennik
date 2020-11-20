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

namespace SBD.Pages.Student
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            Windows.StudentWindow studentWindow = new Windows.StudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == studentWindow.ShowDialog())
            {
                //_context.SaveChanges();
            }
        }
        private void ClickEdit(object sender, RoutedEventArgs e)
        {
            Windows.StudentWindow studentWindow = new Windows.StudentWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == studentWindow.ShowDialog())
            {
                //_context.SaveChanges();
            }
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
