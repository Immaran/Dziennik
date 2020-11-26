using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SBD.Pages;

namespace SBD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Models.ModelContext context = new Models.ModelContext();
        public MainWindow()
        {
            InitializeComponent();

            // Set an icon using code
            //Uri iconUri = new Uri("pack://application:,,,/[name].ico", UriKind.RelativeOrAbsolute);
            //this.Icon = BitmapFrame.Create(iconUri);

            frame.NavigationService.Navigate(new Home());
        }
    }
}
