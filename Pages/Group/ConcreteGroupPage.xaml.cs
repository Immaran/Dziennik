using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

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
