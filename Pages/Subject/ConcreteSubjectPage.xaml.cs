using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for ConcreteSubjectPage.xaml
    /// </summary>
    public partial class ConcreteSubjectPage : Page
    {
        private readonly ModelContext _context;
        private readonly Models.Subject Subject;        // konkretny przedmiot
        private readonly List<Models.Group> Groups;     // lista grup, ktore ucza sie tego przedmiotu
        private List<GroupStudent> GroupStudentList;    // lista groupstudent laczaca uczniow i grupe
        private List<Models.Student> Students;          // lista uczniow, ktorzy ucza sie tego przedmiotu
        public ConcreteSubjectPage(Models.Subject subject, List<Models.Group> groups)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Subject = subject;
            Groups = groups;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SubjectName.Content = Subject.Name; // nazwa przedmiotu

            this.FindGroupStudent();    // znalezienie obiektow groupstudent

            this.FindStudents();        // znalezienie uczniow uczacych sie danego przedmiotu

            StudentsListBox.ItemsSource = Students; // przypisanie uczniów do listboxa
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
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if(StudentsListBox.SelectedItem != null)
            {
                GradeWindow gradeWindow = new GradeWindow(Subject, (Models.Student)StudentsListBox.SelectedItem)
                {
                    Owner = ((MainWindow)Application.Current.MainWindow)
                };
                if (true == gradeWindow.ShowDialog())
                {
                    //_context.SaveChanges();
                }
            }
        }
        private void ViewGradesClick(object sender, RoutedEventArgs e)
        {
            if (StudentsListBox.SelectedItem != null)
            {
                this.NavigationService.Navigate(new ConcreteStudentPage((Models.Student)StudentsListBox.SelectedItem,Subject));
            }
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsListBox.SelectedItem != null)
            {
                Add.IsEnabled = true;
                More.IsEnabled = true;
            }
            else
            {
                Add.IsEnabled = false;
                More.IsEnabled = false;
            }
        }
        private void MyExport(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // tworzenie nowego dokumentu do pisania w nim
                Document document = new Document(PageSize.LETTER, 10, 10, 42, 35);

                // nazwa pliku pdf
                string PathName = "/Lista Uczniów " + Subject.Name + ".pdf";

                // zmienna pomocnicza do przechodzenia wyzej po katalogach
                string currentPath = Directory.GetCurrentDirectory(); 

                // dopóki nie dojdziemy do folderu Dziennik
                while (!currentPath.EndsWith("Dziennik"))
                {
                    currentPath = Directory.GetParent(currentPath).FullName;
                }
                // przechodzimy jeszcze raz wyzej, wiec pdf znajdzie sie w tym samym pliku co katalog Dziennik
                currentPath = Directory.GetParent(currentPath).FullName;

                // miejsce gdzie zapisujemy pdfa
                PathName = currentPath + PathName;

                // tworzymy go
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(PathName, FileMode.Create));
                document.Open();

                // dodajemy po kolei uczniow z grupy do pdfa
                foreach (Models.Student student in Students)
                {
                    document.Add(new Paragraph(student.ToString()));
                }

                document.Close();
                MessageBox.Show("Eksport udany", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception)
            {
                MessageBox.Show("Błąd przy eksportowaniu", "Wyjątek", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void MyExportCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(Students != null)
            {
                if (Students.Count > 0)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }
    }
}
