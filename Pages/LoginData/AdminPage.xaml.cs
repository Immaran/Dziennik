using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;

namespace SBD.Pages.LoginData
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private readonly Models.ModelContext _context;

        public IList<Models.LoginData> LoginData { get; set; }
        public Models.LoginData NewLoginData { get; set; }
        public AdminPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            // load the entities into EF Core
            LoginData = _context.LoginData.ToList();

            listbox.ItemsSource = LoginData;

            //foreach (var item in LoginData)
            //{
            //    listbox.Items.Add(item.ToString());
            //}
        }

        private void ClickAdd(object sender,RoutedEventArgs e)
        {
            //_context.LoginData.Add(NewLoginData);
            //_context.SaveChanges();
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
