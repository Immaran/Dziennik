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
    /// Interaction logic for SubjectWindow.xaml
    /// </summary>
    public partial class SubjectWindow : Window
    {
        public SubjectWindow()
        {
            InitializeComponent();
        }
        private Models.Subject subject;
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && teacher.SelectedItem != null && group.SelectedItem != null)
            {
                //if (subject == null)
                //{
                //    subject = new Models.Subject();
                //}

                //subject.Name = name.Text;
                //subject.TeacherId = teacher.SelectedItem.Id;
                //miejsce na przypisanie przedmiotu do danej grupy
                //DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Przedmiot", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            //name.Text = ?
            //teacher.SelectedItem = ?
        }
    }
}
