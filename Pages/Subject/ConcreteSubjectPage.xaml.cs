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
using System.Text;

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
                Document document = new Document(PageSize.A4, 10, 10, 20, 40);

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

                // stworzenie tabeli do ktorej bedziemy wpisywac dane uczniow
                PdfPTable table = new PdfPTable(3); // parametr konstruktora to liczba kolumn

                // dodanie pierwszego wiersza z nazwą przedmiotu
                PdfPCell cell = new PdfPCell(new Phrase(Subject.Name));
                cell.Colspan = 3;
                cell.HorizontalAlignment = 1; // 0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                // dodanie naglowkow tabeli
                table.AddCell("Nr");                        // kolumna 1
                table.AddCell(RemoveDiacritics("Imię"));    // kolumna 2
                table.AddCell("Nazwisko");                  // kolumna 3

                int counter = 1;    // zmienna pomocnicza do okreslania numeru danego ucznia w tabeli
                // dodajemy po kolei uczniow z grupy do pdfa
                foreach (Models.Student student in Students)
                {
                    table.AddCell(counter++.ToString());
                    // jezeli uczen ma drugie imie to do kolumny imie dodajemy tez drugie imie
                    table.AddCell(RemoveDiacritics(student.FirstName + " " + student.SecondName));
                    table.AddCell(RemoveDiacritics(student.Surname));
                }
                document.Add(table);    // dodanie skonstruowanej tabeli do dokumentu
                document.Close();       // wymagane zamkniecie dokumentu
                MessageBox.Show("Eksport udany", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
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
        private string RemoveDiacritics(string text)
        {
            StringBuilder textCopy = new StringBuilder(text);
            for(int i = 0; i < text.Length; i++)
            {
                switch(text[i])
                {
                    case 'ę':
                        textCopy[i] = 'e';
                        break;
                    case 'ó':
                        textCopy[i] = 'o';
                        break;
                    case 'ą':
                        textCopy[i] = 'a';
                        break;
                    case 'ś':
                        textCopy[i] = 's';
                        break;
                    case 'ł':
                        textCopy[i] = 'l';
                        break;
                    case 'ć':
                        textCopy[i] = 'c';
                        break;
                    case 'ń':
                        textCopy[i] = 'n';
                        break;
                    case 'ż':
                    case 'ź':
                        textCopy[i] = 'z';
                        break;
                    case 'Ę':
                        textCopy[i] = 'E';
                        break;
                    case 'Ó':
                        textCopy[i] = 'O';
                        break;
                    case 'Ą':
                        textCopy[i] = 'A';
                        break;
                    case 'Ś':
                        textCopy[i] = 'S';
                        break;
                    case 'Ł':
                        textCopy[i] = 'L';
                        break;
                    case 'C':
                        textCopy[i] = 'C';
                        break;
                    case 'N':
                        textCopy[i] = 'N';
                        break;
                    case 'Ż':
                    case 'Ź':
                        textCopy[i] = 'Z';
                        break;
                    default:
                        break;
                }
            }
            return textCopy.ToString();
        }
    }
}
