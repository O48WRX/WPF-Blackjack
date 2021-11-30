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
        public int playerTokens = 1000;
        public int wonTokens = 0;
        public bool VDCanPull = true;
        public int activeBet = 0;

        public List<int> playerCards = new List<int>();
        public List<int> dealerCards = new List<int>();
        public GameWindow(HideTransfer hide)
        {
            InitializeComponent();
            hideIt = hide;
            GW_PlayerHand.IsReadOnly = true;
            GW_DealerHand.IsReadOnly = true;
            GW_Tokens.IsReadOnly = true;
            GW_Score.IsReadOnly = true;
            StartGame();
        }

        private void GW_Back_Click(object sender, RoutedEventArgs e)
        {
            hideIt.Invoke();
            this.Close();
        }

        public int DrawRandomCard(int[] deck)
        { 
            return deck[rnd.Next(1, deck.Length)];
        }

        public void StartGame()
        {
            //Lehetséges adatok alaphelyzetbe állítása.

            activeBet = 0;
            GW_BetBox.Text = "(Írjon ide számot!)";
            GW_Bet.IsEnabled = true;

            playerCards.Clear();
            dealerCards.Clear();

            //Inicializálás
            playerCards.Add(DrawRandomCard(deck));
            playerCards.Add(DrawRandomCard(deck));

            dealerCards.Add(DrawRandomCard(deck));
            dealerCards.Add(DrawRandomCard(deck));

            if (dealerCards[0] + dealerCards[1] == 17)
            {
                VDCanPull = false;
            }
            Doubling = false;

            foreach (int number in playerCards)
            {
                GW_PlayerHand.AppendText(number + ", ");
            }


            GW_DealerHand.AppendText(dealerCards[0].ToString());
            GW_DealerHand.AppendText(", ?");

            GW_Tokens.Text = playerTokens.ToString();
            GW_Score.Text = wonTokens.ToString();
        }

        private void GW_Bet_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet != 0)
            {
                MessageBox.Show("Már tett tétet!");
                GW_Bet.IsEnabled = false;
                return;
            }

            int bet = 0;

            if (!int.TryParse(GW_BetBox.Text, out bet))
            {
                MessageBox.Show("A megadott tét nem szám!");
                return;
            }

            if (bet > playerTokens)
            {
                MessageBox.Show("A megadott tét nagyobb, mint a rendelkezésre álló összeg!");
                return;
            }

            playerTokens -= bet;
            GW_Tokens.Text = playerTokens.ToString();
            activeBet = bet;
        }

        public int CountCards(List<int> deck)
        {
            int sum = 0;
            for (int i = 0; i < deck.Count(); i++)
            {
                sum += deck[i];
            }
            return sum;
        }

        private void GW_Call_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
                return;
            }

            int card = DrawRandomCard(deck);
            playerCards.Add(card);

            if (CountCards(playerCards) > 21)
            {
                MessageBox.Show("Elvesztette a kört!");
                RoundLost();
                return;
            }

            GW_PlayerHand.AppendText(card + ", ");

        }

        private void RoundLost()
        {
            GW_DealerHand.Text = "";
            GW_PlayerHand.Text = "";
            StartGame();
            wonTokens -= activeBet;
            GW_Score.Text = wonTokens.ToString();
        }

        private void RoundWon()
        {
            GW_DealerHand.Text = "";
            GW_PlayerHand.Text = "";
            playerTokens += activeBet * 2;
            wonTokens += activeBet * 2;
            StartGame();
            GW_Score.Text = wonTokens.ToString();
            GW_Tokens.Text = playerTokens.ToString();
        }

        private void GW_Throw_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
                return;
            }

            MessageBox.Show("Bedobta a kártyáit, ezzel elvesztette a kört!");
            GW_DealerHand.Text = "";
            GW_PlayerHand.Text = "";
            StartGame();
            wonTokens -= activeBet;
            GW_Score.Text = wonTokens.ToString();
        }

        private void GW_Check_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
            }
        }
    }
}
