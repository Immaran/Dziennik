using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private List<Models.GroupSubject> GroupSubjectList { get; set; } // lista groupstudent do ktorej naleza przedmioty, ktore uczy nauczyciel
        private List<Models.Group> GroupList { get; set; } // lista grup w ktorych sa nauczane przedmioty przez danego nauczyciela

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

            // przypisanie listy tylko tych przedmiotów, które uczy dany nauczyciel
            SubjectList = _context.Subject.Where(x => x.Teacher == Teacher).ToList();

            GroupSubjectList = new List<GroupSubject>();
            // przejscie po groupstudent i wyciaganie obiektow ktore maja powiazanie z przedmiotami
            foreach(GroupSubject gs in _context.GroupSubject)
            {
                if ( SubjectList.Any(s => s.Id == gs.SubjectId) )
                {
                    GroupSubjectList.Add(gs);
                }
            }

            GroupList = new List<Models.Group>();
            // przejscie po group i wyciaganie tych grup ktore maja powiazanie z przedmiotami
            foreach (Models.Group g in _context.Group)
            {
                if (GroupSubjectList.Any(gs => gs.GroupId == g.Id))
                {
                    GroupList.Add(g);
                }
            }

            // czyszczenie StackPanela, aby nie dublowały się przyciski, gdy klika się powrót
            SubjectPanel.Children.Clear();

            // przejscie po liscie przedmiotow
            foreach (Models.Subject subject in SubjectList)
            {
                GroupSubject groupSubject = null;
                // znajdujemy odpowiedni groupsubject dla przedmiotu
                foreach(GroupSubject gs in GroupSubjectList)
                {
                    if(gs.SubjectId == subject.Id)
                    {
                        groupSubject = gs;
                    }
                }

                Models.Group group = null;
                // jezeli znalezlismy groupsubject tzn przedmiot ma przypisana jakas grupe
                if(groupSubject != null)
                {
                    // znajdujemy okreslona grupe przypisana do przedmiotu
                    foreach (Models.Group g in GroupList)
                    {
                        if (g.Id == groupSubject.GroupId)
                        {
                            group = g;
                        }
                    }
                }
                
                Button btn = new Button(); // przycisk z nawami przedmiotow

                if(group != null) // jezeli przedmiot jest przypisany do jakiejs grupy
                {
                    btn.Content = subject.Id + " - " +  subject.Name + " (" + group.Name + ")";
                }
                else // jezeli przedmiot nie ma przypisanej grupy
                {
                    btn.Content = subject.Id + " - " +  subject.Name;
                }

                // podpiecie zdarzenia na klikniecie przycisku
                btn.Click += new RoutedEventHandler(this.ButtonClick);

                // dodanie przycisku do StackPanelu
                SubjectPanel.Children.Add(btn);
            }
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // wyciągamy napis na przycisku
            string ButtonContent = (e.Source as Button).Content.ToString();
            
            // wyciagamy id wybranego przedmiotu, ktore jest pierwsze w napisie
            string[] IdOfSubject = ButtonContent.Split(' ');

            // znajdowanie przedmiotu po jego Id
            Models.Subject subject = SubjectList.First(s => s.Id.ToString() == IdOfSubject[0]);

            this.NavigationService.Navigate(new ConcreteSubjectPage(subject));
        }
    }
}
