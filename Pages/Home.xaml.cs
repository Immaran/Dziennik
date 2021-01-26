using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SBD.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            /*ColorAnimation animation = new ColorAnimation();
            animation.From = Colors.Black;
            animation.To = Colors.White;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            this.homeLabel.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);*/
        }
        private void onClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
