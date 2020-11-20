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
using System.Windows.Shapes;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        public GroupWindow()
        {
            InitializeComponent();
        }
        private Models.Group group;
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0)
            {
                //if (group == null)
                //{
                //    group = new Models.Group();
                //}

                //group.Name = name.Text;
                //DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Grupa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            //name.Text = ?
            //studentList.Items = ?
            //subjectList.Items = ?
        }
    }
}
