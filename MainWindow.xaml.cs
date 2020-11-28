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
        public MainWindow()
        {
            InitializeComponent();

            // Set an icon using code
            //Uri iconUri = new Uri("pack://application:,,,/[name].ico", UriKind.RelativeOrAbsolute);
            //this.Icon = BitmapFrame.Create(iconUri);

            frame.NavigationService.Navigate(new Home());
            

            /* var logindata = new Models.LoginData { Login = "Hej", Password = "Czesc", Role = "Ml. szeregowy" };
            context.Add<Models.LoginData>(logindata);
            context.SaveChanges();*/
            

        }
    }
}
