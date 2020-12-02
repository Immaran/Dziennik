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
        
        private List<Models.GroupSubject> GroupSubjectList; // lista groupstudent do ktorej naleza przedmioty, ktore uczy nauczyciel
        private List<Models.Group> GroupList; // lista grup w ktorych sa nauczane przedmioty przez danego nauczyciela
        
        private List<GroupSubject> gsList = new List<GroupSubject>(); // lista groupsubject dla konkretnego przedmiotu
        private List<Models.Group> gList = new List<Models.Group>(); // lista grup dla konkretnego przedmiotu

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

            GroupSubjectList = new List<GroupSubject>();
            // przejscie po groupstudent i wyciaganie obiektow ktore maja powiazanie z przedmiotami
            foreach (GroupSubject gs in _context.GroupSubject)
            {
                if (SubjectList.Any(s => s.Id == gs.SubjectId))
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
                gsList.Clear();
                // znajdujemy odpowiednie obiekty groupsubject dla przedmiotu
                foreach(GroupSubject gs in GroupSubjectList)
                {
                    if(gs.SubjectId == subject.Id)
                    {
                        gsList.Add(gs);
                    }
                }

                // jezeli znalezlismy chociaz jeden groupsubject tzn do przedmiotu jest przypisana co najmniej jedna grupa
                if (gsList.Count > 0)
                {
                    gList.Clear();
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
                
                Button btn = new Button(); // przycisk z nawami przedmiotow

                if(gList.Count == 0) // jezeli przedmiot nie jest przypisany do żadnej grupy
                {
                    //btn.Tag = subject.Id;
                    btn.Content = subject.Name;
                    btn.IsEnabled = false;
                }
                else if(gList.Count == 1) // jezeli przedmiot jest przypisany do jednej grupy
                {
                    btn.Tag = subject.Id;
                    btn.Content = subject.Name + " (" + gList[0].Name + ")";
                }
                else // jezeli przedmiot jest przypisany do wiecej niz jednej grupy
                {
                    btn.Tag = subject.Id;
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
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // wyciągamy id przedmiotu
            string id = (e.Source as Button).Tag.ToString();

            // znajdowanie przedmiotu po jego Id
            Models.Subject subject = SubjectList.First(s => s.Id.ToString() == id);

            this.NavigationService.Navigate(new ConcreteSubjectPage(subject));
        }
    }
}
