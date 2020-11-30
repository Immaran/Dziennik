using System;
using SBD.Models;
using System.Collections.Generic;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace SBD.Windows
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        private readonly ModelContext _context;
        private Event Event { get; set; }

        private readonly Teacher Teacher = (Teacher)((MainWindow)Application.Current.MainWindow).loggedUser;
        public EventWindow()
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            InitializeComponent();
        }
        public EventWindow(Event ev)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Event = ev;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // if to edit
            if(Event != null)
            {
                NameOfEvent.Text = Event.Name;
                if (Event.Description.Length > 0)
                    DescriptionOfEvent.Text = Event.Description;
                DateOfEvent.SelectedDate = Event.Date;
            }
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if(NameOfEvent.Text.Length > 0 && DateOfEvent.SelectedDate != null)
            {
                // if add new event
                if(Event == null)
                {
                    Event = new Event();
                    Event.Name = NameOfEvent.Text;
                    if( DescriptionOfEvent.Text.Length > 0 )
                        Event.Description = DescriptionOfEvent.Text;
                    Event.Date = (DateTime)DateOfEvent.SelectedDate;
                    Event.TeacherId = Teacher.Id;
                    Event.Teacher = Teacher;
                    _context.Event.Add(Event);
                }
                else // if to edit
                {
                    Event.Name = NameOfEvent.Text;
                    if (DescriptionOfEvent.Text.Length > 0)
                        Event.Description = DescriptionOfEvent.Text;
                    Event.Date = (DateTime)DateOfEvent.SelectedDate;
                    Event.TeacherId = Teacher.Id;
                    Event.Teacher = Teacher;
                    _context.Attach(Event).State = EntityState.Modified;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Brak wszystkich danych", "Wydarzenie", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
