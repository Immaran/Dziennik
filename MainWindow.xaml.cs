﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using SBD.Models;
using SBD.Pages;

namespace SBD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Models.ModelContext context = new Models.ModelContext();
        public dynamic loggedUser;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                context.Database.EnsureCreated();
            }
            catch (Exception e) //Oracle.ManagedDataAccess.Client.OracleException
            {
                MessageBox.Show("Wystąpił problem z połączeniem z bazą danych: " + e.Message, "Wyjątek", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
            frame.NavigationService.Navigate(new Home());
        }
    }
}
