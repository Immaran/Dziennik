using System;
using System.Windows.Controls;

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
        }
        private void onClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
