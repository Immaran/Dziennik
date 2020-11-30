﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SBD.Models;
using SBD.Windows;

namespace SBD.Pages.Event
{
    /// <summary>
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Event> EventList { get; set; }
        public TeacherPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // wczytanie danych 
            this.fetchData();
        }
        private void fetchData()
        {
            EventList = _context.Event.ToList();
            EventListBox.ItemsSource = EventList;
        }
        private void CreateEvent(object sender, RoutedEventArgs e)
        {
            EventWindow eventWindow = new EventWindow
            {
                Owner = ((MainWindow)Application.Current.MainWindow)
            };
            if (true == eventWindow.ShowDialog())
            {
                this.fetchData();
            }
        }
    }
}
