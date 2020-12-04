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
        
        private readonly Models.Student Student = (Models.Student)((MainWindow)Application.Current.MainWindow).loggedUser;
        public StudentPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            groupList = new List<Models.Group>();
            groupstudentList = new List<Models.GroupStudent>();
            
            studentList = new List<Models.Student>();
            
        InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //loading data
            sPage.DataContext = null;
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

                Label lbl = new Label();
                lbl.Content = groupName;
                lbl.VerticalAlignment = VerticalAlignment.Center;
                lbl.HorizontalAlignment = HorizontalAlignment.Center;
                lbl.FontSize = 20;
                Panel.Children.Add(lbl);
                Button btn = new Button();
                btn.Margin = new Thickness(5, 5, 5, 5);
                btn.Padding = new Thickness(5, 5, 5, 5);
                btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                btn.Content = "Przejdź";

                List<Models.Student> st = studentList.ToList();

                
                studentList.Clear();
                btn.Click += (object sender, RoutedEventArgs e) =>
                {
                    this.NavigationService.Navigate(new ConcreteGroupPage(groupName, st));
                    Panel.Children.Clear();
                };
                Panel.Children.Add(btn);

                
            }
            groupList.Clear();




        }
    }
}
