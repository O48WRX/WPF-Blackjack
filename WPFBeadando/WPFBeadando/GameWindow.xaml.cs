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
        public HideTransfer hideIt;
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
    }
}
