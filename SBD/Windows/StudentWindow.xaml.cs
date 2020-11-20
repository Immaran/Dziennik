using System;
using System.Windows;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
        }
        private Models.Student student;
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                //if (student == null)
                //{
                //    student = new Models.Student();
                //}
                if (secondName.Text.Length != 0)
                {
                    //student.FirstName = name.Text;
                    //student.SecondName = secondName.Text;
                    //student.Surname = surname.Text;
                    //DialogResult = true;
                }
                else
                {
                    //student.FirstName = name.Text;
                    //student.Surname = surname.Text;
                    //DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Uczeń", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if to edit 
            //name.Text = ?
            //secondName.Text = ?
            //surname.Text = ?
        }
    }
}
