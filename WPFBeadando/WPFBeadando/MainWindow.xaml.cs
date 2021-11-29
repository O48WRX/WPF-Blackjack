﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFBeadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string playerName;
        public int score;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GameStart_Click(object sender, RoutedEventArgs e)
        {
            if (playerName == String.Empty)
            {
                GetName namewindow = new GetName();
                namewindow.DataContext = this;
                namewindow.Show();
            }
            GameWindow gw = new GameWindow();
            gw.Show();
        }

        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {
            Leaderboard scores = new Leaderboard();
            scores.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void SetName(string name)
        {
            playerName = name;
        }
    }
}
