using System;
using System.Windows;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        public TeacherWindow()
        {
            InitializeComponent();
        }
        private Models.Teacher teacher;
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && surname.Text.Length > 0)
            {
                //if (teacher == null)
                //{
                //    teacher = new Models.Teacher();
                //}
                if (secondName.Text.Length != 0)
                {
                    //teacher.FirstName = name.Text;
                    //teacher.SecondName = secondName.Text;
                    //teacher.Surname = surname.Text;
                    //DialogResult = true;
                }
                else 
                {
                    //teacher.FirstName = name.Text;
                    //teacher.Surname = surname.Text;
                    //DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Nauczyciel", MessageBoxButton.OK, MessageBoxImage.Warning);
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
