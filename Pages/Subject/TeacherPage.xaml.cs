using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SBD.Models;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Subject> SubjectList { get; set; } // lista przedmiotów, które uczy dany nauczyciel
        
        private List<GroupSubject> GroupSubjectList;    // lista groupstudent do ktorej naleza przedmioty, ktore uczy nauczyciel
        private List<Models.Group> GroupList;           // lista grup w ktorych sa nauczane przedmioty przez danego nauczyciela

        private List<GroupSubject> gsList;      // lista groupsubject dla konkretnego przedmiotu
        private List<Models.Group> gList;       // lista grup dla konkretnego przedmiotu
        
        private List< List<Models.Group> > GroupsForSubjects;           // lista list grup dla przedmiotow
        private int counter;                    // licznik, aby wiedziec ktora lista grup jest przypisana do jakiego przycisku

        // obecnie zalogowany nauczyciel
        private readonly Models.Teacher Teacher = (Models.Teacher)((MainWindow)Application.Current.MainWindow).loggedUser;
        public TeacherPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // wczytanie danych

            _context.Subject.Load(); // wczytanie tabeli z przedmiotami

            // przypisanie listy tylko tych przedmiotów, które uczy dany nauczyciel
            SubjectList = Teacher.Subject.ToList();

            //SubjectList = _context.Subject.Where(x => x.Teacher == Teacher).ToList();

            this.FindGroupSubject();    // znajdowanie obiektow groupstudent ktore maja powiazanie z przedmiotami

            this.FindGroup();           // znajdowanie tych grup ktore maja powiazanie z przedmiotami

            // czyszczenie StackPanela, aby nie dublowały się przyciski, gdy klika się powrót
            SubjectPanel.Children.Clear();

            GroupsForSubjects = new List<List<Models.Group>>(); // inicjalizacja listy, ktora bedzie potem potrzebna

            counter = -1; // reset licznika

            // przejscie po liscie przedmiotow
            foreach (Models.Subject subject in SubjectList)
            {
                gsList = new List<GroupSubject>(); // czyszcenie listy groupsubject

                // znajdujemy odpowiednie obiekty groupsubject dla przedmiotu
                foreach(GroupSubject gs in GroupSubjectList)
                {
                    if(gs.SubjectId == subject.Id)
                    {
                        gsList.Add(gs);
                    }
                }

                gList = new List<Models.Group>(); // czyszczenie listy grup

                // jezeli znalezlismy chociaz jeden groupsubject tzn do przedmiotu jest przypisana co najmniej jedna grupa
                if (gsList.Count > 0)
                {
                    // znajdujemy okreslona grupe przypisana do przedmiotu
                    foreach (Models.Group g in GroupList)
                    {
                        foreach(GroupSubject gs in gsList)
                        {
                            if (g.Id == gs.GroupId)
                            {
                                gList.Add(g);
                            }
                        }
                    }
                }
                
                if(gList.Count > 0) // jezeli znalezlismy jakas grupe przypisana do przedmiotu
                {
                    GroupsForSubjects.Add(gList);   // dodanie listy grup dla danego przedmiotu
                    counter++;
                }

                Button btn = new Button(); // przycisk z nawami przedmiotow

                btn.Style = (Style)Application.Current.FindResource("TeacherButton");
                btn.Margin = new Thickness(5, 5, 5, 5);
                btn.Padding = new Thickness(5, 5, 5, 5);
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.ToolTip = "Przejdź do listy uczniów.";

                if (gList.Count == 0) // jezeli przedmiot nie jest przypisany do żadnej grupy
                {
                    continue; // wtedy nie bedzie sie wgl pokazywal na liscie
                    //btn.Content = subject.Name;
                    //btn.IsEnabled = false;
                }
                else if(gList.Count == 1) // jezeli przedmiot jest przypisany do jednej grupy
                {
                    btn.Tag = counter + " " + subject.Id;
                    btn.Content = subject.Name + " (" + gList[0].Name + ")";
                }
                else // jezeli przedmiot jest przypisany do wiecej niz jednej grupy
                {
                    btn.Tag = counter + " " + subject.Id;
                    string tmp = subject.Name + " ( ";
                    foreach(Models.Group g in gList)
                    {
                        tmp += g.Name + " , ";
                    }
                    tmp = tmp.Remove(tmp.Length - 2);
                    btn.Content = tmp + ")";
                }

                // podpiecie zdarzenia na klikniecie przycisku
                btn.Click += new RoutedEventHandler(this.ButtonClick);

                // dodanie przycisku do StackPanelu
                SubjectPanel.Children.Add(btn);
            }
        }
        private void FindGroupSubject()
        {
            GroupSubjectList = new List<GroupSubject>();
            // przejscie po groupstudent i wyciaganie obiektow ktore maja powiazanie z przedmiotami
            foreach (GroupSubject gs in _context.GroupSubject)
            {
                if (SubjectList.Any(s => s.Id == gs.SubjectId))
                {
                    GroupSubjectList.Add(gs);
                }
            }
        }
        private void FindGroup()
        {
            GroupList = new List<Models.Group>();
            // przejscie po group i wyciaganie tych grup ktore maja powiazanie z przedmiotami
            foreach (Models.Group g in _context.Group)
            {
                if (GroupSubjectList.Any(gs => gs.GroupId == g.Id))
                {
                    GroupList.Add(g);
                }
            }
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string Tag = (e.Source as Button).Tag.ToString();       // wyciągamy Tag z przycisku
            string[] data = Tag.Split(' ');

            // znajdowanie przedmiotu po jego Id
            Models.Subject subject = SubjectList.First(s => s.Id.ToString() == data[1]);
            var x = GroupsForSubjects;
            this.NavigationService.Navigate(new ConcreteSubjectPage(subject,GroupsForSubjects[int.Parse(data[0])]));
        }
    }
}
