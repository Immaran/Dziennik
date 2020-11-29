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
        private IList<Models.Student> StudentList { get; set; }
        private IList<Models.Teacher> TeacherList { get; set; }

        private readonly object User = ((MainWindow)Application.Current.MainWindow).loggedUser;

        public MessageWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(User.GetType() == typeof(Teacher))
            {
                StudentList = _context.Student.ToList();
                RecipientBox.ItemsSource = StudentList;
            }
            else if (User.GetType() == typeof(Student))
            {
                TeacherList = _context.Teacher.ToList();
                RecipientBox.ItemsSource = TeacherList;
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
                Message message = new Message
                {
                    Content = Message.Text,
                    Date = DateTime.Now
                };
                if (User.GetType() == typeof(Teacher))
                {
                    Teacher teacher = (Teacher)User;
                    message.Teacher = teacher;
                    message.TeacherId = teacher.Id;
                    Student student = (Student)RecipientBox.SelectedItem;
                    message.Student = student;
                    message.StudentId = student.Id;
                    _context.Message.Add(message);
                    this.SaveDB();
                }
                else if (User.GetType() == typeof(Student))
                {
                    Student student = (Student)User;
                    message.Student = student;
                    message.StudentId = student.Id;
                    Teacher teacher = (Teacher)RecipientBox.SelectedItem;
                    message.Teacher = teacher;
                    message.TeacherId = teacher.Id;
                    _context.Message.Add(message);
                    this.SaveDB();
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Wiadomość", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
