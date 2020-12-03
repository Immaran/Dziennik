using System.Windows;
using Microsoft.EntityFrameworkCore;
using SBD.Models;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for GradeWindow.xaml
    /// </summary>
    public partial class GradeWindow : Window
    {
        private readonly ModelContext _context;
        private Grade Grade;
        private readonly Student Student;    // uczen dla ktorego ocena jest wystawiana
        private readonly Subject Subject;    // przedmiot ktorego ocena dotyczy
        public GradeWindow(Subject subject, Student student)    // konstruktor gdy dodajemy nowa ocene
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Subject = subject;
            Student = student;
            InitializeComponent();
        }
        public GradeWindow(Grade grade) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Grade = grade;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Event.Load();
            DescriptionCBox.ItemsSource = ((MainWindow)Application.Current.MainWindow).loggedUser.Event;
            DescriptionCBox.DisplayMemberPath = "Name";
            DescriptionCBox.SelectedValuePath = "Id";
            Binding binding = new Binding();

            if (Grade != null)  // jezeli edytujemy ocene
            {

                binding.FallbackValue = Grade.Description; //start value of textbox
                int index = -1;
                foreach (dynamic ev in DescriptionCBox.Items)
                {
                    if (ev.Name == Grade.Description)
                    { index = ev.Id; break; }
                }
                if (index > -1)
                    DescriptionCBox.SelectedValue = index;

                this.descryptGrade();
            }

            binding.Source = DescriptionCBox;
            binding.Path = new PropertyPath("SelectedItem.Name");
            binding.Mode = BindingMode.OneWay;
            Descritpion.SetBinding(TextBox.TextProperty, binding);
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
            if(GradesComboBox.SelectedItem != null && Descritpion.Text.Length > 0)
            {
                if(Grade == null) // jezeli dodajemy nowa ocena
                {
                    Grade = new Grade();
                    Grade.Value = ((ComboBoxItem)GradesComboBox.SelectedItem).Content.ToString();
                    Grade.Description = Descritpion.Text;
                    Grade.Date = DateTime.Now;
                    Grade.Student = Student;
                    Grade.StudentId = Student.Id;
                    Grade.Subject = Subject;
                    Grade.SubjectId = Subject.Id;
                    _context.Grade.Add(Grade);
                }
                else // jezeli edytujemy ocene
                {
                    Grade.Value = ((ComboBoxItem)GradesComboBox.SelectedItem).Content.ToString();
                    Grade.Description = Descritpion.Text;
                    Grade.Date = DateTime.Now;
                    _context.Attach(Grade).State = EntityState.Modified;
                }

                this.SaveDB();

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Ocena", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void descryptGrade()
        {
            switch(Grade.Value)
            {
                case "1":
                    GradesComboBox.SelectedIndex = 0;
                    break;
                case "1+":
                    GradesComboBox.SelectedIndex = 0;
                    break;
                case "2-":
                    GradesComboBox.SelectedIndex = 1;
                    break;
                case "2":
                    GradesComboBox.SelectedIndex = 2;
                    break;
                case "2+":
                    GradesComboBox.SelectedIndex = 3;
                    break;
                case "3-":
                    GradesComboBox.SelectedIndex = 4;
                    break;
                case "3":
                    GradesComboBox.SelectedIndex = 5;
                    break;
                case "3+":
                    GradesComboBox.SelectedIndex = 6;
                    break;
                case "4-":
                    GradesComboBox.SelectedIndex = 7;
                    break;
                case "4":
                    GradesComboBox.SelectedIndex = 8;
                    break;
                case "4+":
                    GradesComboBox.SelectedIndex = 9;
                    break;
                case "5-":
                    GradesComboBox.SelectedIndex = 10;
                    break;
                case "5":
                    GradesComboBox.SelectedIndex = 11;
                    break;
                case "5+":
                    GradesComboBox.SelectedIndex = 12;
                    break;
                case "6":
                    GradesComboBox.SelectedIndex = 14;
                    break;
                case "6-":
                    GradesComboBox.SelectedIndex = 14;
                    break;
            }
        }
    }
}
