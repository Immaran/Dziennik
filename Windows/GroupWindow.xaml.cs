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
        private IList<Student> StudentList { get; set; }            // lista uczniow dla danej grupy
        private IList<Subject> SubjectList { get; set; }            // lista przedmiotow dla danej grupy
        private IList<GroupSubject> GroupSubjectList { get; set; }  // lista GroupSubject dla danej grupy
        private IList<GroupStudent> GroupStudentList { get; set; }  // lista GroupStudent dla danej grupy
        private string OldGroupName = "";   // stara nazwa grupy (jesli edytujemy grupe mozemy jej dac taka nazwe jaka miala)
        public GroupWindow()    // konstruktor, gdy tworzymy nowa grupe
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public GroupWindow(Group group) // konstrukor, gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Group = group;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Teacher.Load(); // wczytanie nauczycieli, aby wyswietlac całe nazwy przedmiotow

            SubjectList = _context.Subject.ToList();    // wczytanie przedmiotow
            subjectBox.ItemsSource = SubjectList;       // przypisanie przedmiotow do listboxa
            StudentList = _context.Student.ToList();    // wczytanie uczniow
            studentBox.ItemsSource = StudentList;       // przypisanie uczniow do listboxa
            
            if (Group != null) // jezeli dane sa do edycji
            {
                // wczytanie danych z GroupStudent tylko dla danej grupy
                GroupStudentList = _context.GroupStudent.Where(gs => gs.GroupId == Group.Id).ToList();

                // wczytanie danych z GroupSubject tylko dla danej grupy
                GroupSubjectList = _context.GroupSubject.Where(gs => gs.GroupId == Group.Id).ToList();

                // przypisanie nazwy grupy
                name.Text = Group.Name;
                OldGroupName = Group.Name;

                // przypisanie listy uczniów
                foreach(GroupStudent gs in GroupStudentList)
                {
                    Student student = StudentList.First(s => s.Id == gs.StudentId);
                    studentList.Items.Add(student);
                }

                // przypisanie listy przedmiotów 
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
            if (name.Text.Length > 0 && studentList.Items.Count > 0 && subjectList.Items.Count > 0)
            {
                if (this.GroupNameExist() == true)   // jezeli w bazie istnieje juz grupa o takiej nazwie
                {
                    MessageBox.Show("Grupa o takiej nazwie już istnieje!!", "Grupa", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(name.Text.Length > 20)
                {
                    MessageBox.Show("Zbyt długa nazwa", "Grupa", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Group == null) // gdy tworzymy nową grupę
                {
                    Group = new Group();
                    Group.Name = name.Text;
                    _context.Group.Add(Group);
                    this.SaveDB();

                    // pobranie dodanej grupy z serwera, aby określić jego ID
                    Group =  _context.Group.First(g => g.Name == Group.Name);

                    // przypisanie listy uczniów
                    foreach (Student student in studentList.Items)
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

                    // przypisanie listy przedmiotów 
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

                    /// edycja listy uczniów
                    // iteracja po liście studentów
                    foreach (Student student in studentList.Items)
                    {
                        found = false;
                        // iteracja po rekordach w bazie
                        foreach(GroupStudent gr in GroupStudentList)
                        {
                            if(gr.Student.Id == student.Id)
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

                    /// edycja listy przedmiotów 
                    // iteracja po liście przedmiotów
                    foreach (Subject subject in subjectList.Items)
                    {
                        found = false;
                        // iteracja po rekordach w bazie
                        foreach (GroupSubject gr in GroupSubjectList)
                        {
                            if (gr.Subject.Id == subject.Id)
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
        private bool GroupNameExist()
        {
            bool found = false;
            foreach(Group group in _context.Group)
            {
                if(group.Name == name.Text)
                {
                    if (name.Text == OldGroupName) // jezeli edytujemy mozemy nadac taka sama nazwe jak miala wczesniej
                        continue;
                    found = true;
                    break;
                }
            }
            return found;
        }
        private void AddStudent(object sender, RoutedEventArgs e)
        {
            if(studentBox.SelectedItem != null)
            {
                // jesli juz jest na liscie to nie dodajemy drugi raz
                if ( !studentList.Items.Contains(studentBox.SelectedItem) ) 
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
                // jesli juz jest na liscie to nie dodajemy drugi raz
                if ( !subjectList.Items.Contains(subjectBox.SelectedItem) )
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
