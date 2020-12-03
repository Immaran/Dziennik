using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SBD.Pages.Group
{
    /// <summary>
    /// Logika interakcji dla klasy ConcreteGroupPage.xaml
    /// </summary>
    public partial class ConcreteGroupPage : Page
    {
        public ConcreteGroupPage()
        {
            InitializeComponent();
           
        }
        public ConcreteGroupPage(string groupName, List<Models.Student> studentList)
        {
            InitializeComponent();
            StartLabel.Content += " " + groupName;
            GroupLBox.ItemsSource = studentList.OrderBy(s=>s.Surname).ThenBy(s=>s.FirstName).ToList();
            
        }
    }
}
