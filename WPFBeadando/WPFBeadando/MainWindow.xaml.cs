using System;
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
    public delegate void DataTransfer(string data);
    public delegate void HideTransfer();
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTransfer transferDelegate;
        public HideTransfer showDelegate;
        public string playerName = "";
        public int score;

        public MainWindow()
        {
            InitializeComponent();
            transferDelegate += new DataTransfer(SetName);
            showDelegate += new HideTransfer(ShowThisWindow);
        }

        private void GameStart_Click(object sender, RoutedEventArgs e)
        {
            if (playerName == String.Empty)
            {
                GetName namewindow = new GetName(transferDelegate);
                namewindow.DataContext = this;
                namewindow.Show();
                return;
            }
            GameWindow gw = new GameWindow(showDelegate);
            gw.Show();
            this.Hide();
        }

        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {
            Leaderboard scores = new Leaderboard(showDelegate);
            scores.Show();
            this.Hide();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void SetName(string name)
        {
            playerName = name;
        }

        public void ShowThisWindow()
        {
            this.Show();
        }
    }
}
