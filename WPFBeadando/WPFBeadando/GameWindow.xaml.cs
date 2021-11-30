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
using System.Windows.Shapes;

namespace WPFBeadando
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public Random rnd = new Random();
        public HideTransfer hideIt;

        public int[] deck = new int[] {2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10};

        public bool Doubling = false;
        public int defaultTokens = 1000;
        public int wonTokens;
        public bool VDCanPull = true;
        public int activeBet = 0;

        public List<int> playerCards = new List<int>();
        public List<int> dealerCards = new List<int>();
        public GameWindow(HideTransfer hide)
        {
            InitializeComponent();
            hideIt = hide;
        }

        private void GW_Back_Click(object sender, RoutedEventArgs e)
        {
            hideIt.Invoke();
            this.Close();
        }

        public int DrawRandomCard(int[] deck)
        { 
            return deck[rnd.Next(1, deck.Length + 1)];
        }

        public void StartGame()
        {
            GW_Bet.IsEnabled = true;

            playerCards.Add(DrawRandomCard(deck));
            playerCards.Add(DrawRandomCard(deck));

            dealerCards.Add(DrawRandomCard(deck));
            dealerCards.Add(DrawRandomCard(deck));

            if (dealerCards[0] + dealerCards[1] == 17)
            {
                VDCanPull = false;
            }
            Doubling = false;
        }

        private void GW_Bet_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet != 0)
            {
                MessageBox.Show("Már tett tétet!");
                GW_Bet.IsEnabled = false;
            }

            int bet = 0;

            if (!int.TryParse(GW_BetBox.Text, out bet))
            {
                MessageBox.Show("A megadott tét nem szám!");
                return;
            }

            activeBet = bet;
        }
    }
}
