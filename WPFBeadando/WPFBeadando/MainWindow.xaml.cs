using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    //Delegate a név átadására a GetName formról a főmenü számára.
    public delegate void DataTransfer(string data);

    //Delegate a főmenü eltüntetésének figyelésére.
    public delegate void HideTransfer();

    //Delegate a score átadására a GameWindow-ról
    public delegate void ScoreTransfer(int score);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTransfer transferDelegate;
        public HideTransfer showDelegate;
        public ScoreTransfer scoreDelegate;
        public string playerName = "";
        public int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            transferDelegate += new DataTransfer(SetName);
            showDelegate += new HideTransfer(ShowThisWindow);
            scoreDelegate += new ScoreTransfer(SetScore);
            MW_PlayerName.IsReadOnly = true;
            MW_PlayerScores.IsReadOnly = true;
        }

        private void GameStart_Click(object sender, RoutedEventArgs e)
        {
            //Ha nincs megadva játékos név akkor
            //Nyissa meg a GetName formot, hogy megkaphassa a delegate-el.
            if (playerName == String.Empty)
            {
                GetName namewindow = new GetName(transferDelegate);
                namewindow.DataContext = this;
                namewindow.Show();
                return;
            }
            //Ha már megvan a név, akkor elindítja a játékot 
            //Átadjuk a 2 delegatet paraméterként, hogy átadhassunk a 2 form között adatot.
            GameWindow gw = new GameWindow(showDelegate, scoreDelegate);
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

        //Metódus a delegate.Invoke() számára, hogy átadhassuk a nevet a főmenünek.
        public void SetName(string name)
        {
            playerName = name;
            MW_PlayerName.Text = playerName;
        }

        //Delegate metódus
        public void ShowThisWindow()
        {
            this.Show();
        }

        //Delegate metódus
        public void SetScore(int score)
        {
            this.score = score;
            MW_PlayerScores.Text = this.score.ToString();
        }


        //Metódus a CSVHelper segítségével létrehozunk egy '.csv' fájlt, amiben tárolhatjuk a játékos adatait.
        private void MW_SaveScores_Click(object sender, RoutedEventArgs e)
        {
            if (playerName == "" || score == 0)
            {
                MessageBox.Show("Nem lehet menteni, hiányzik a név vagy pontok!");
                return;
            }

            var records = new List<object>
            {
                new { Name = this.playerName, Score = this.score },
            };

            if (!File.Exists("Leaderboard.csv"))
            {
                var config1 = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };
                using (var writer = new StreamWriter("Leaderboard.csv"))
                using (var csv = new CsvWriter(writer, config1))
                {
                    csv.WriteRecords(records);
                }
                return;
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            using (var stream = File.Open("Leaderboard.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
