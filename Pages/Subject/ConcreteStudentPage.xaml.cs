using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for ConcreteStudent.xaml
    /// </summary>
    public partial class ConcreteStudentPage : Page
    {
        private readonly ModelContext _context;
        private readonly Models.Student Student;
        private readonly Models.Subject Subject;
        private IList<Models.Grade> Grades;
        public ConcreteStudentPage(Models.Student student, Models.Subject subject)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Student = student;
            Subject = subject;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StudentData.Content = Student.ToString();   // naglowek strony
            
            this.fetchData(); // znalezienie ocen dla konkretnego przedmiotu i ucznia
        }
        private void fetchData()
        {
            Grades = _context.Grade.Where(g => g.SubjectId == Subject.Id && g.StudentId == Student.Id).ToList();

            GradesListBox.ItemsSource = Grades; // przypisanie ocen do listboxa
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            GradeWindow gradeWindow = new GradeWindow(Subject, Student)
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == gradeWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
        private void EditClick(object sender, RoutedEventArgs e)
        {
            if(GradesListBox.SelectedItem != null)
            {
                GradeWindow gradeWindow = new GradeWindow((Models.Grade)GradesListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == gradeWindow.ShowDialog())
                {
                    this.fetchData();
                }
            }
        }
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            if (GradesListBox.SelectedItem != null)
            {
                _context.Grade.Remove((Models.Grade)GradesListBox.SelectedItem);
                _context.SaveChanges();
                this.fetchData();
            }
        }
    }
}
