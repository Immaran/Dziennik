﻿using System.Collections.Generic;
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
        private IList<Models.Student> StudentList { get; set; }
        private IList<Models.Subject> SubjectList { get; set; }
        public GroupWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public GroupWindow(Group group) // konstrukor gdy dane są do modyfikacji
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            this.Group = group;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SubjectList = _context.Subject.ToList();
            StudentList = _context.Student.ToList();
            studentBox.ItemsSource = StudentList;
            subjectBox.ItemsSource = SubjectList;
            //if to edit 
            if (Group != null)
            {
                name.Text = Group.Name;

                /// tu należy przypisać listę uczniów
                //studentList.Items = ?

                /// tu należy przypisać listę przedmiotów 
                //subjectList.Items = ?
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length > 0 && studentList.Items != null && subjectList.Items != null)
            {
                if (Group == null) // gdy tworzymy nową grupę
                {
                    Group = new Group();
                    Group.Name = name.Text;

                    /// tu należy przypisać listę uczniów
                    /// Group.GroupStudent = ???
                    
                    /// tu należy przypisać listę przedmiotów 
                    /// Group.GroupSubject = ???
                    
                    _context.Group.Add(Group);
                }
                else // gdy edytujemy dane grupy
                {
                    Group.Name = name.Text;

                    /// tu należy przypisać listę uczniów
                    /// Group.GroupStudent = ???

                    /// tu należy przypisać listę przedmiotów 
                    /// Group.GroupSubject = ???
                    
                    _context.Attach(Group).State = EntityState.Modified;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //_context.SaveChanges();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Grupa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddStudent(object sender, RoutedEventArgs e)
        {
            if(studentBox.SelectedItem != null)
            {
                if( !studentList.Items.Contains(studentBox.SelectedItem) )
                    studentList.Items.Add(studentBox.SelectedItem);
            }
        }
        private void AddSubject(object sender, RoutedEventArgs e)
        {
            if(subjectBox.SelectedItem != null)
            {
                if( !subjectList.Items.Contains(subjectBox.SelectedItem) )
                    subjectList.Items.Add(subjectBox.SelectedItem);
            }
        }
    }
}
