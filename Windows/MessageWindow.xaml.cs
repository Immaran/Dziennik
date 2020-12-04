using Microsoft.EntityFrameworkCore;
using SBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        private readonly ModelContext _context;
        private IList<Student> StudentList { get; set; }    // lista odbiorcow dla nauczyciela (uczniowie)
        private IList<Teacher> TeacherList { get; set; }    // lista odbiorcow dla ucznia (nauczyciele)

        // obecnie zalogowany uzytkownik
        private readonly object User = ((MainWindow)Application.Current.MainWindow).loggedUser;

        public MessageWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(User.GetType() == typeof(Teacher))   // jezeli zalogowany jest nauczyciel
            {
                StudentList = _context.Student.ToList();
                RecipientBox.ItemsSource = StudentList; // przypisanie listy odbiorcow
            }
            else if (User.GetType() == typeof(Student)) // jezeli zalogowany jest uczen
            {
                TeacherList = _context.Teacher.ToList();
                RecipientBox.ItemsSource = TeacherList; // przypisanie listy odbiorcow
            }
        }
        private void SaveDB()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        private void SendClick(object sender, RoutedEventArgs e)
        {
            if(Message.Text.Length > 0 && RecipientBox.SelectedItem != null)
            {
                if(Message.Text.Length > 500 )
                {
                    MessageBox.Show("Zbyt długa wiadomość", "Wiadomość", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Message message = new Message
                {
                    Date = DateTime.Now
                };
                // jezeli wiadomosc wysyla nauczyciel
                if (User.GetType() == typeof(Teacher))
                {
                    Teacher teacher = (Teacher)User;
                    message.Content = Message.Text + "\n\nWysłano przez: " + teacher;
                    message.Teacher = teacher;
                    message.TeacherId = teacher.Id;
                    message.SenderId = teacher.Id;
                    Student student = (Student)RecipientBox.SelectedItem;
                    message.Student = student;
                    message.StudentId = student.Id;
                    _context.Message.Add(message);
                }
                // jeżeli wiadomość wysyła uczeń
                else if (User.GetType() == typeof(Student))
                {
                    Student student = (Student)User;
                    message.Content = Message.Text + "\n\nWysłano przez: " + student;
                    message.Student = student;
                    message.StudentId = student.Id;
                    message.SenderId = student.Id;
                    Teacher teacher = (Teacher)RecipientBox.SelectedItem;
                    message.Teacher = teacher;
                    message.TeacherId = teacher.Id;
                    _context.Message.Add(message);
                }

                this.SaveDB();

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Wiadomość", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
