using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        private readonly ModelContext _context;
        private Group Group { get; set; }
        private IList<Models.Student> StudentList { get; set; }
        private IList<Models.Subject> SubjectList { get; set; }
        private List<Models.GroupSubject> GroupSubjectList { get; set; }
        private List<Models.GroupStudent> GroupStudentList { get; set; }
        public GroupWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public GroupWindow(Group group) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Group = group;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Teacher.Load(); // wczytanie nauczycieli, aby wyświetlać całe nazwy przedmiotów
            SubjectList = _context.Subject.ToList();
            StudentList = _context.Student.ToList();
            studentBox.ItemsSource = StudentList;
            subjectBox.ItemsSource = SubjectList;
            //if to edit 
            if (Group != null)
            {
                //wczytanie danych z GroupStudent tylko dla danej grupy
                GroupStudentList = new List<Models.GroupStudent>();
                foreach (GroupStudent gs in _context.GroupStudent)
                {
                    if (gs.GroupId == Group.Id)
                    {
                        GroupStudentList.Add(gs);
                    }
                }

                //wczytanie danych z GroupSubject tylko dla danej grupy
                GroupSubjectList = new List<Models.GroupSubject>();
                foreach (GroupSubject gs in _context.GroupSubject)
                {
                    if (gs.GroupId == Group.Id)
                    {
                        GroupSubjectList.Add(gs);
                    }
                }

                //przypisanie nazwy grupy
                name.Text = Group.Name;

                /// tu należy przypisać listę uczniów
                foreach(GroupStudent gs in GroupStudentList)
                {
                    Student student = StudentList.First(s => s.Id == gs.StudentId);
                    studentList.Items.Add(student);
                }

                /// tu należy przypisać listę przedmiotów 
                foreach (GroupSubject gs in GroupSubjectList)
                {
                    Subject subject = SubjectList.First(s => s.Id == gs.SubjectId);
                    subjectList.Items.Add(subject);
                }
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
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && studentList.Items != null && subjectList.Items != null)
            {
                if (Group == null) // gdy tworzymy nową grupę
                {
                    Group = new Group();
                    Group.Name = name.Text;
                    _context.Group.Add(Group);
                    this.SaveDB();

                    // pobranie dodanej grupy z serwera, aby określić jego ID
                    Group =  _context.Group.FirstOrDefault(g => g.Name == Group.Name); 

                    // tu należy przypisać listę uczniów
                    foreach(Student student in studentList.Items)
                    {
                        GroupStudent gs = new GroupStudent
                        {
                            GroupId = Group.Id,
                            StudentId = student.Id,
                            Group = Group,
                            Student = student
                        };
                        _context.GroupStudent.Add(gs);
                        this.SaveDB();
                    }

                    // tu należy przypisać listę przedmiotów 
                    foreach (Subject subject in subjectList.Items)
                    {
                        GroupSubject gs = new GroupSubject
                        {
                            GroupId = Group.Id,
                            SubjectId = subject.Id,
                            Group = Group,
                            Subject = subject
                        };
                        _context.GroupSubject.Add(gs);
                        this.SaveDB();
                    }
                }
                else // gdy edytujemy dane grupy
                {
                    Group.Name = name.Text;
                    _context.Attach(Group).State = EntityState.Modified;
                    this.SaveDB();

                    // pomocnicza zmienna do flagowania znajdowania obiektów
                    bool found = false;

                    /// tu należy edytować listę uczniów
                    // iteracja po liście studentów
                    foreach (Student student in studentList.Items)
                    {
                        found = false;
                        // iteracja po rekordach w bazie
                        foreach(GroupStudent gr in GroupStudentList)
                        {
                            if(gr.Student == student)
                            {
                                found = true;
                                break;
                            }
                        }
                        // jeśli jest na liście studentów, ale nie ma go w bazie
                        if(found == false)
                        {
                            GroupStudent gs = new GroupStudent
                            {
                                GroupId = Group.Id,
                                StudentId = student.Id,
                                Group = Group,
                                Student = student
                            };
                            _context.GroupStudent.Add(gs);
                            this.SaveDB();
                        }
                    }

                    // iteracja po rekordach w bazie
                    foreach(GroupStudent gr in GroupStudentList)
                    {
                        // jeśli jest w bazie, a nie ma go na liście studentów
                        if( !studentList.Items.Contains(gr.Student) )
                        {
                            _context.GroupStudent.Remove(gr);
                            this.SaveDB();
                        }
                    }

                    /// tu należy edytować listę przedmiotów 
                    // iteracja po liście przedmiotów
                    foreach (Subject subject in subjectList.Items)
                    {
                        found = false;
                        // iteracja po rekordach w bazie
                        foreach (GroupSubject gr in GroupSubjectList)
                        {
                            if (gr.Subject == subject)
                            {
                                found = true;
                                break;
                            }
                        }
                        // jeśli jest na liście przedmiotów, ale nie ma go w bazie
                        if (found == false)
                        {
                            GroupSubject gs = new GroupSubject
                            {
                                GroupId = Group.Id,
                                SubjectId = subject.Id,
                                Group = Group,
                                Subject = subject
                            };
                            _context.GroupSubject.Add(gs);
                            this.SaveDB();
                        }
                    }

                    // iteracja po rekordach w bazie
                    foreach (GroupSubject gr in GroupSubjectList)
                    {
                        // jeśli jest w bazie, a nie ma go na liście studentów
                        if ( !subjectList.Items.Contains(gr.Subject) )
                        {
                            _context.GroupSubject.Remove(gr);
                            this.SaveDB();
                        }
                    }
                }
                
                this.SaveDB();

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Grupa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddStudent(object sender, RoutedEventArgs e)
        {
            if(studentBox.SelectedItem != null)
            {
                if( !studentList.Items.Contains(studentBox.SelectedItem) )
                    studentList.Items.Add(studentBox.SelectedItem);
            }
        }
        private void RemoveStudent(object sender, RoutedEventArgs e)
        {
            if (studentList.SelectedItem != null)
            {
                studentList.Items.Remove(studentList.SelectedItem);
            }
        }
        private void AddSubject(object sender, RoutedEventArgs e)
        {
            if(subjectBox.SelectedItem != null)
            {
                if( !subjectList.Items.Contains(subjectBox.SelectedItem) )
                    subjectList.Items.Add(subjectBox.SelectedItem);
            }
        }
        private void RemoveSubject(object sender, RoutedEventArgs e)
        {
            if (subjectList.SelectedItem != null)
            {
                subjectList.Items.Remove(subjectList.SelectedItem);
            }
        }
    }
}
