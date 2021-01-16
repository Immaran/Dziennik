using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Grade
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Grade> GradeList { get; set; } // ogolna lista wszystkich ocen ucznia
        private List<List<Models.Grade>> GradesForSubject = new List<List<Models.Grade>>(); // lista list ocen dla odpowiednichprzedmiotow
        private List<Models.Grade> gList;   // lista ocen dla odpowiedniego przedmiotu
        private List<Models.Subject> SubjectList = new List<Models.Subject>();  // lista przedmiotow jakich uczy sie dany uczen
        
        // listy pomocnicze do znalezienia przedmiotow ktorych uczy sie dany uczen
        private IList<GroupStudent> GroupStudentList { get; set; }
        private List<Models.Group> GroupList = new List<Models.Group>();
        private List<GroupSubject> GroupSubjectList = new List<GroupSubject>();

        // obecnie zalogowany uczen
        private readonly Models.Student Student = (Models.Student)((MainWindow)Application.Current.MainWindow).loggedUser;
        public StudentPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //wczytanie danych 

            this.FindGrades(); // znalezienie ocen przypisanych do danego ucznia

            // przejscie z ucznia do przedmiotu po tabelach laczacych
            this.FindGroupStudent();
            this.FindGroup();
            this.FindGroupSubject();

            this.FindSubjects(); // znalezienie przedmiotow ktorych uczy sie uczen

            StackPanel.Children.Clear(); // czyszczenie StackPanela, aby przycisk powrot dzialal

            bool found; // zmienna pomocnicza okreslajaca czy uczen ma jakies oceny z obecnie przetwarzanego przedmiotu w petli
            foreach (Models.Subject subject in SubjectList)
            {
                gList = new List<Models.Grade>();   // inicjacja listy ocen dla danego przedmiotu
                found = false;

                // znajdowanie tylko tych ocen ktore sa przypisane do danego przedmiotu
                foreach (Models.Grade grade in GradeList)
                {
                    if (grade.SubjectId == subject.Id)
                    {
                        gList.Add(grade);
                        found = true;
                    }
                }

                if (found == true) // jezeli uczen z danego przedmiotu ma jakies oceny
                {
                    GradesForSubject.Add(gList);
                    Label label = new Label();
                    label.FontSize = 20;
                    label.Content = subject.Name;
                    StackPanel.Children.Add(label);

                    ListBox listBox = new ListBox();
                    listBox.Style = (Style)Application.Current.FindResource("StudentListBox");

                    //listBox.Resources.Add(subject.Name, (Style)Application.Current.FindResource("StudentScrollBar"));
                    //Style style = new Style();
                    //style.TargetType = typeof(ListBoxItem);
                    //style.BasedOn = (Style)Application.Current.FindResource("StudentListBoxItem");
                    //listBox.Resources.Add(subject.Name, style);
                    //listBox.Resources
                    //listBox.Resources.Add(subject.Name, (Style)Application.Current.FindResource("StudentListBoxItem"));

                    listBox.ItemsSource = GradesForSubject.Last();
                    StackPanel.Children.Add(listBox);
                }
                else // jezeli uczen z danego przedmiotu nie ma ocen
                {
                    Label label = new Label();
                    label.FontSize = 20;
                    label.Content = subject.Name;
                    StackPanel.Children.Add(label);
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "Nie masz jeszcze żadnych ocen z tego przedmiotu";
                    StackPanel.Children.Add(textBlock);
                }
            }
        }
        private void FindGrades()
        {
            GradeList = _context.Grade.Where(g => g.StudentId == Student.Id).ToList();
        }
        private void FindGroupStudent()
        {
            GroupStudentList = _context.GroupStudent.Where(gs => gs.StudentId == Student.Id).ToList();
        }
        private void FindGroup()
        {
            GroupList.Clear();
            foreach(Models.Group group in _context.Group)
            {
                foreach(GroupStudent gs in GroupStudentList)
                {
                    if(group.Id == gs.GroupId)
                    {
                        GroupList.Add(group);
                    }
                }
            }
        }
        private void FindGroupSubject()
        {
            GroupSubjectList.Clear();
            foreach(GroupSubject gs in _context.GroupSubject)
            {
                foreach(Models.Group group in GroupList)
                {
                    if(gs.GroupId == group.Id)
                    {
                        GroupSubjectList.Add(gs);
                    }
                }
            }
        }
        private void FindSubjects()
        {
            SubjectList.Clear();
            foreach(Models.Subject subject in _context.Subject)
            {
                foreach(GroupSubject gs in GroupSubjectList)
                {
                    if(gs.SubjectId == subject.Id)
                    {
                        SubjectList.Add(subject);
                    }
                }
            }
        }
    }
}
