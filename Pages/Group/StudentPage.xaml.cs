using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SBD.Pages.Group
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        private readonly ModelContext _context;
        
        private List<Models.Group> groupList;
        private List<Models.GroupStudent> groupstudentList;
        
        private List<Models.Student> studentList;

        private List<Models.GroupSubject> groupsubjectList;
        private List<Models.Subject> subjectList;

        private readonly Models.Student Student = (Models.Student)((MainWindow)Application.Current.MainWindow).loggedUser;
        public StudentPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            groupList = new List<Models.Group>();
            groupstudentList = new List<Models.GroupStudent>();
            studentList = new List<Models.Student>();
            groupsubjectList = new List<Models.GroupSubject>();
            subjectList = new List<Models.Subject>();


            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //sPage.DataContext = null;

            //loading data
            _context.GroupStudent.Load();
            groupstudentList = Student.GroupStudent.ToList();

            foreach (Models.Group group in _context.Group)
            {
                foreach (Models.GroupStudent groupstudent in groupstudentList)
                {
                    if (group.Id == groupstudent.GroupId)
                        groupList.Add(group);
                }
            }

            foreach (Models.Group group in groupList)
            {
                string groupName = group.Name;
                _context.GroupStudent.Load();//

                foreach (Models.Student student in _context.Student)
                {
                    foreach (Models.GroupStudent groupstudent in student.GroupStudent)
                    {
                        if (group.Id == groupstudent.GroupId)
                        {
                            studentList.Add(student);
                            break;
                        }
                    }
                }

                Label lbl = initializeLabel(groupName);
                Panel.Children.Add(lbl);
                Button btn = new Button();
                btn.Content = "Przejdź";

                List<Models.Student> st = studentList.ToList();

                
                studentList.Clear();
                btn.Click += (object sender, RoutedEventArgs e) =>
                {
                    this.NavigationService.Navigate(new ConcreteGroupPage(groupName, st));
                    Panel.Children.Clear();
                };
                Panel.Children.Add(btn);

                
            } // End of viewing single groups

            
           
            //Start of viewing multigroups

            foreach (Models.Group group in groupList)
            {
                foreach (Models.GroupSubject groupSubject in _context.GroupSubject)
                {
                    if (groupSubject.GroupId == group.Id)
                    {
                        groupsubjectList.Add(groupSubject);
                    }
                }
            }
            foreach (Models.GroupSubject groupSubject in groupsubjectList )
            {
                foreach (Models.Subject subject in _context.Subject)
                {
                    if (subject.Id == groupSubject.SubjectId)
                    {
                        subjectList.Add(subject);
                        break;//
                    }
                }
            }

            _context.GroupSubject.Load();//
            List<Models.Subject> subjectListUpdated = subjectList.ToList();
            foreach (Models.Subject subject in subjectList)
            {
                if (subject.GroupSubject.Count<2)
                {
                    subjectListUpdated.RemoveAll(s => s.Id == subject.Id); //lub od razu remove subject jak się da lub subjectList.Remove(subject);
                }

            }

            //Now we have list of subjects that are multigroups in which student is, so we should get the list of students learning each subject
            _context.GroupSubject.Load();///
            if (subjectListUpdated.Count > 0)
            {
                List<Models.GroupSubject> gsubjectForSubject = new List<Models.GroupSubject>(); 
                List<Models.Group> groupForSubject = new List<Models.Group>();

                foreach (Models.Subject subject in subjectListUpdated)
                {

                    foreach (Models.GroupSubject groupsubject in subject.GroupSubject)
                    {
                        gsubjectForSubject.Add(groupsubject);
                    }

                    foreach (Models.GroupSubject groupsubject in gsubjectForSubject)
                    {
                        foreach (Models.Group group in _context.Group)
                        {
                             if (groupsubject.GroupId == group.Id)
                            {
                                groupForSubject.Add(group);
                                break;///
                            }
                                
                        }
                    }
                    List<Models.GroupStudent> gStudentForSubject = new List<Models.GroupStudent>();

                    foreach (Models.Group group in groupForSubject)
                    {
                        foreach (Models.GroupStudent gStudent in _context.GroupStudent)
                        {
                            if (gStudent.GroupId == group.Id)
                            {
                                gStudentForSubject.Add(gStudent);
                                
                            }
                        }
                    }
                    List<Models.Student> studentsOfSubject = new List<Models.Student>();

                    foreach (Models.GroupStudent gStudent in gStudentForSubject)
                    {
                        foreach (Models.Student student in _context.Student)
                        {
                            if (student.Id == gStudent.StudentId)
                            {
                                studentsOfSubject.Add(student);
                                
                            }
                        }
                    }




                    string subjectName = subject.Name;
                    Label lbl = initializeLabel(subjectName);
                    Panel.Children.Add(lbl);
                    Button btn = new Button();
                    btn.Content = "Przejdź";


                    List<Models.Student> studentsofSubjectCopy = studentsOfSubject.ToList();
                     btn.Click += (object sender, RoutedEventArgs e) =>
                    {
                        this.NavigationService.Navigate(new ConcreteGroupPage(subjectName, studentsofSubjectCopy));
                        Panel.Children.Clear();
                    };
                    Panel.Children.Add(btn);

                    gsubjectForSubject.Clear();
                    groupForSubject.Clear();
                    gStudentForSubject.Clear();
                    studentsOfSubject.Clear();
                }

                

            }
            groupList.Clear();
            groupsubjectList.Clear();
            subjectList.Clear();



            
        }
        public Label initializeLabel(string content)
        {
            Label lbl = new Label();
            lbl.Content = content;
            lbl.VerticalAlignment = VerticalAlignment.Center; //można te linijki w funkcję złożyć
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.FontSize = 30;
            return lbl;
        }
    }
}
