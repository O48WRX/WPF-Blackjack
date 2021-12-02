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
        //Random
        public Random rnd = new Random();
        //Delegatek
        public HideTransfer hideIt;
        public ScoreTransfer scoreThis;

        //A játék logikája által használt kártyapakli tömbje.
        public int[] deck = new int[] {2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10,
                                       2,3,4,5,6,7,8,9,10,10,10,10};

        //A játék logika által használt változók
        public bool Doubling = false;
        public int playerTokens = 1000;
        public int wonTokens = 0;
        public bool VDCanPull = true;
        public int activeBet = 0;

        //A játékos és Virtuális Dealer káryáinak listája.
        public List<int> playerCards = new List<int>();
        public List<int> dealerCards = new List<int>();
        public GameWindow(HideTransfer hide, ScoreTransfer playerScore)
        {
            InitializeComponent();
            hideIt = hide;
            scoreThis = playerScore;
            GW_PlayerHand.IsReadOnly = true;
            GW_DealerHand.IsReadOnly = true;
            GW_Tokens.IsReadOnly = true;
            GW_Score.IsReadOnly = true;
            StartGame();
        }

        private void GW_Back_Click(object sender, RoutedEventArgs e)
        {
            //A Back gomb megnyomásakor, a delegate átadja a játékos pontjait a főmenünek.
            hideIt.Invoke();
            if (wonTokens != 0)
                scoreThis.Invoke(wonTokens);
            this.Close();
        }

        //Ez a metódus egy random kártyát húz a 'deck' tömbből és visszaadja azt.
        public int DrawRandomCard(int[] deck)
        { 
            return deck[rnd.Next(1, deck.Length)];
        }

        //Elindítja a játékot, valamint alaphelyzetbe is állítja azt.
        //Gyakran használt egyéb metódusokban az alaphelyzetbe állításhoz.
        public void StartGame()
        {
            //Lehetséges adatok alaphelyzetbe állítása.

            activeBet = 0;
            GW_BetBox.Text = "(Írjon ide számot!)";
            GW_Bet.IsEnabled = true;
            GW_Double.IsEnabled = true;

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

            //A playerhand textboxnak átadjuk a játékos kártyáit megjelenítésre.
            foreach (int number in playerCards)
            {
                GW_PlayerHand.AppendText(number + ", ");
            }

            //a dealerhand textboxnak átadjuk a dealer egy kártyáját, a másikat pedig elrejtjük
            //Ezzel megfelelve a blackjack játék szabályainak.
            GW_DealerHand.AppendText(dealerCards[0].ToString());
            GW_DealerHand.AppendText(", ?");

            //A játékos zsetonjait és pontjait átadjuk a megfelelő textboxoknak.
            GW_Tokens.Text = playerTokens.ToString();
            GW_Score.Text = wonTokens.ToString();
        }

        //A tét hozzáadási metódus
        private void GW_Bet_Click(object sender, RoutedEventArgs e)
        {
            //Ha van aktív tét, akkor nem tudunk hozzáadni többet ezen a módon.
            if (activeBet != 0)
            {
                MessageBox.Show("Már tett tétet!");
                GW_Bet.IsEnabled = false;
                return;
            }

            int bet = 0;

            //Ha nem szám amit a textboxba írtunk, akkor visszatér
            if (!int.TryParse(GW_BetBox.Text, out bet))
            {
                MessageBox.Show("A megadott tét nem szám!");
                return;
            }

            //Ha nagyobb a tét mind a rendelkezésre álló zsetonok száma, akkor visszatér.
            if (bet > playerTokens)
            {
                MessageBox.Show("A megadott tét nagyobb, mint a rendelkezésre álló összeg!");
                return;
            }

            //Tét kivonása a rendelkezésre álló zsetonokból.
            //A zsetonok számát átadjuk a megfelelő textbboxnak
            //Az aktív tét a megtett tétünk lesz.
            playerTokens -= bet;
            GW_Tokens.Text = playerTokens.ToString();
            activeBet = bet;
        }

        //Kártya értékeinek megszámolására való metódus
        //Később a kiértékelésnél van nagy szerepe.
        public int CountCards(List<int> deck)
        {
            int sum = 0;
            for (int i = 0; i < deck.Count(); i++)
            {
                sum += deck[i];
            }
            return sum;
        }

        //Kártyák hivása a játékos számára
        private void GW_Call_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
                return;
            }

            int card = DrawRandomCard(deck);
            playerCards.Add(card);

            //Ha a játékos kártyáinak értéke meghaladja a 21-et, akkor elveszette a kört.
            if (CountCards(playerCards) > 21)
            {
                MessageBox.Show("Elvesztette a kört!");
                RoundLost();
                return;
            }

            GW_PlayerHand.AppendText(card + ", ");

        }

        //A kör elvesztési metódus
        //Lényegébben alaphelyzet és a tét végleges elvesztése a zsetonokból.
        private void RoundLost()
        {
            GW_DealerHand.Text = "";
            GW_PlayerHand.Text = "";
            wonTokens -= activeBet;
            GW_Score.Text = wonTokens.ToString();
            if (playerTokens == 0 || playerTokens < 0)
            {
                GameLost();
                return;
            }
            StartGame();
            wonTokens -= activeBet;
            GW_Score.Text = wonTokens.ToString();
        }

        //A kör megnyerési metódus
        //A játékos a zsetonjainak a dupláját kapja vissza.
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

        //Bedobási metódus, ha a játékos bedobja a kártyáit,
        //Elveszti a kört, és alaphelyzetbe áll a játék.
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

        //A kör végső kiértékelése, és a virtuális dealer Call-ja
        //Addig húz ameddig vagy nagyobbat húz mint a játékos, vagy elveszti a kört.
        private void GW_Check_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
                return;
            }

            GW_DealerHand.Text = "";

            foreach (int number in dealerCards)
            {
                GW_DealerHand.AppendText(number + ", ");
            }


            if (CountCards(dealerCards) > CountCards(playerCards))
            {
                MessageBox.Show("Elvesztette a kört, és a tétet!");
                RoundLost();
                return;
            }

            while (CountCards(dealerCards) <= 21 || CountCards(dealerCards) < CountCards(playerCards))
            {
                if (CountCards(dealerCards) > CountCards(playerCards))
                {
                    break;
                }
                int card = DrawRandomCard(deck);
                dealerCards.Add(card);
                GW_DealerHand.AppendText(card.ToString());
                GW_DealerHand.AppendText(", ");
            }

            if (CountCards(dealerCards) > CountCards(playerCards) && CountCards(dealerCards) <= 21)
            {
                MessageBox.Show("Elvesztette a kört, és a tétet!");
                RoundLost();
                return;
            }

            if (CountCards(dealerCards) == CountCards(playerCards) && CountCards(dealerCards) <= 21)
            {
                MessageBox.Show("Megnyerte a kört, és megnyerte a tét dupláját!");
                RoundWon();
                return;
            }

            if (CountCards(dealerCards) > 21)
            {
                MessageBox.Show("Megnyerte a kört, és megnyerte a tét dupláját!");
                RoundWon();
                return;
            }
        }

        //A tét duplázási metódus
        //A játékos megduplázhatja tétet, de ezt, csak egyszer tudja megtenni.
        private void GW_Double_Click(object sender, RoutedEventArgs e)
        {
            if (activeBet == 0)
            {
                MessageBox.Show("Nincs aktív tét!");
                return;
            }

            if (playerTokens - activeBet < 0)
            {
                MessageBox.Show("A duplázás lehetetlenné tenné a játék folytatását!");
                return;
            }

            playerTokens -= activeBet;
            GW_Tokens.Text = activeBet.ToString();
            activeBet += activeBet;
            GW_BetBox.Text = activeBet.ToString();

            GW_Double.IsEnabled = false;
        }

        //A játék elvesztésének metódusa.
        //Teljes restart, a pontok, rendelkezésre álló zsetonok is alaphelyzetbe állnak.
        private void GameLost()
        {
            MessageBox.Show("Elvesztette a játékot, ezzel a pontjait is!");
            scoreThis.Invoke(wonTokens);
            GW_DealerHand.Text = "";
            GW_PlayerHand.Text = "";
            StartGame();
            playerTokens = 1000;
            GW_Tokens.Text = playerTokens.ToString();
            wonTokens = 0;
            GW_Score.Text = wonTokens.ToString();
        }
    }
}
