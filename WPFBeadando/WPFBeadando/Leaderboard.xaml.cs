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
using System.Windows.Shapes;

namespace WPFBeadando
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {
        public object records = new List<object>();
        public HideTransfer hideIt;
        public Leaderboard(HideTransfer hide)
        {
            InitializeComponent();
            hideIt += hide;
            if (File.Exists("Leaderboard.csv"))
            {

                var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };
                using (var reader = new StreamReader("Leaderboard.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    var anonymousTypeDefinition = new
                    {
                        Name = String.Empty,
                        Score = default(int)
                    };
                    records = csv.GetRecords(anonymousTypeDefinition).ToList();
                    LB_Scores.ItemsSource = (IEnumerable<object>)records;
                }
            }
        }

        private void LB_Back_Click(object sender, RoutedEventArgs e)
        {
            hideIt.Invoke();
            this.Close();
        }
    }
}
