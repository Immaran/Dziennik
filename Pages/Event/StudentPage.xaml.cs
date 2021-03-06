﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SBD.Models;

namespace SBD.Pages.Event
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        private readonly ModelContext _context;
        private IList<Models.Event> EventList { get; set; }
        public StudentPage()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // wczytanie danych
            _context.Teacher.Load();
            EventList = _context.Event.OrderByDescending(e=>e.Date).ToList();
            EventListBox.ItemsSource = EventList;
        }
    }
}
