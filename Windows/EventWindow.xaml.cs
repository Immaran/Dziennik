using System;
using SBD.Models;
using System.Collections.Generic;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

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

            NameOfEvent.Focus();
        }
        public EventWindow(Event ev)
        {
            _context = ((MainWindow)Application.Current.MainWindow).context;
            Event = ev;
            InitializeComponent();

            NameOfEvent.Focus();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CalendarDateRange rangeOfBlackoutDates;
            // if to edit
            if (Event != null)
            {
                if (Event.Date < DateTime.Today) //if event have date from past
                    rangeOfBlackoutDates = new CalendarDateRange(DateTime.MinValue, Event.Date.AddDays(-1));
                else
                    rangeOfBlackoutDates = new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1));
                NameOfEvent.Text = Event.Name;
                if (Event.Description.Length > 0)
                    DescriptionOfEvent.Text = Event.Description;
                DateOfEvent.SelectedDate = Event.Date;
                HourOfEvent.Text = Event.Date.Hour.ToString();
                MinuteOfEvent.Text = Event.Date.Minute.ToString();

            }
            else
            {
                //DateOfEvent.DisplayDateStart = DateTime.Today;
                rangeOfBlackoutDates = new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1));
            }
            DateOfEvent.BlackoutDates.Add(rangeOfBlackoutDates);
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            int hh = int.Parse(HourOfEvent.Text);
            int mm = int.Parse(MinuteOfEvent.Text);

            if(NameOfEvent.Text.Length > 0 && DateOfEvent.SelectedDate != null && hh >= 0 && hh <= 23 && mm >= 0 && mm <= 59)
            {
                DateTime dt = DateOfEvent.SelectedDate.Value; //object to hold date from datepicker and time from boxes
                TimeSpan ts = new TimeSpan(hh, mm, 0);
                dt = dt.Date + ts;
                // if add new event
                if (Event == null)
                {
                    Event = new Event();
                    Event.Name = NameOfEvent.Text;
                    if( DescriptionOfEvent.Text.Length > 0 )
                        Event.Description = DescriptionOfEvent.Text;

                    Event.Date = dt;
                    //Event.Date = (DateTime)DateOfEvent.SelectedDate;
                    Event.TeacherId = Teacher.Id;
                    Event.Teacher = Teacher;
                    _context.Event.Add(Event);
                }
                else // if to edit
                {
                    Event.Name = NameOfEvent.Text;
                    if (DescriptionOfEvent.Text.Length > 0)
                        Event.Description = DescriptionOfEvent.Text;
                    Event.Date = dt;
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
                MessageBox.Show("Brak poprawnych danych", "Wydarzenie", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PreviewInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void SelectAddress(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }
        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }
    }
}
