using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using SBD.Models;
using SBD.Pages;

namespace SBD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Models.ModelContext context = new Models.ModelContext();
        public dynamic loggedUser; //public int loggedId; 
        public MainWindow()
        {
            InitializeComponent();

            frame.NavigationService.Navigate(new Home());

        }
    }
}
