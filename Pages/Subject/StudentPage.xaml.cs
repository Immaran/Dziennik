using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;

namespace SBD.Pages.Subject
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Subject> SubjectList { get; set; } // lista przedmiotow, ktorych uczy sie dany uczen
        
        private List<GroupSubject> GroupSubjectList;            // lista groupsubject do ktorej naleza przedmioty ucznia
        private List<Models.Group> GroupList;                   // lista grup w ktorych sa nauczane przedmioty ucznia
        private List<GroupStudent> GroupStudentList;            // lista groupstudent do ktorej naleza przedmioty ucznia

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
            this.FindGroupStudent();

            this.FindGroup();

            this.FindGroupSubject();
        }
        private void FindGroupStudent()
        {
            GroupStudentList = new List<GroupStudent>();
            GroupStudentList = _context.GroupStudent.Where(gs => gs.StudentId == Student.Id).ToList();
            //foreach(GroupStudent gs in _context.GroupStudent)
            //{
            //    if(gs.StudentId == Student.Id)
            //    {
            //        GroupStudentList.Add(gs);
            //    }
            //}
        }
        private void FindGroup()
        {
            GroupList = new List<Models.Group>();
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
            GroupSubjectList = new List<GroupSubject>();
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
    }
}
