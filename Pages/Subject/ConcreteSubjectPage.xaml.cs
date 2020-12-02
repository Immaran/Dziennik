using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using SBD.Models;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for ConcreteSubjectPage.xaml
    /// </summary>
    public partial class ConcreteSubjectPage : Page
    {
        private readonly ModelContext _context;
        private readonly Models.Subject Subject;
        private List<Models.Grade> Grades;              // lista ocen 
        private List<string> Categories;                // lista kategorii ocen, aby okreslic liczbe kolumn
        private readonly List<Models.Group> Groups;     // lista grup, ktore ucza sie tego przedmiotu
        private List<GroupStudent> GroupStudentList;    // lista groupstudent laczaca uczniow i grupe
        private List<Models.Student> Students;          // lista uczniow, ktorzy ucza sie tego przedmiotu
        private List<dynamic> Rows;                     // lista wierszy do wstawienia do tabeli
        public ConcreteSubjectPage(Models.Subject subject, List<Models.Group> groups)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Subject = subject;
            Groups = groups;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SubjectName.Content = Subject.Name + " " + Subject.Id; // nazwa przedmiotu

            this.FindGroupStudent();    // znalezienie obiektow groupstudent

            this.FindStudents();        // znalezienie uczniow uczacych sie danego przedmiotu

            this.FindGrades();          // znalezienie ocen przypisanych do danego przedmiotu

            this.FindCategories();      // znalezienie różnych kategorii ocen

        }
        private void FindGroupStudent()
        {
            GroupStudentList = new List<GroupStudent>();
            foreach (GroupStudent gs in _context.GroupStudent)
            {
                foreach (Models.Group group in Groups)
                {
                    if (gs.GroupId == group.Id)
                    {
                        GroupStudentList.Add(gs);
                    }
                }
            }
        }
        private void FindStudents()
        {
            Students = new List<Models.Student>();
            foreach (Models.Student student in _context.Student)
            {
                foreach (GroupStudent gs in GroupStudentList)
                {
                    if (student.Id == gs.StudentId)
                    {
                        Students.Add(student);
                    }
                }
            }
        }
        private void FindGrades()
        {
            Grades = new List<Models.Grade>();
            foreach (Models.Grade grade in _context.Grade)
            {
                if (grade.SubjectId == Subject.Id)
                {
                    Grades.Add(grade);
                }
            }
        }
        private void FindCategories()
        {
            Categories = new List<string>();
            foreach(Models.Grade grade in Grades)
            {
                if( !Categories.Contains(grade.Description) )
                {
                    Categories.Add(grade.Description);
                }
            }
        }
        private void fetchData()
        {

        }
    }
}
