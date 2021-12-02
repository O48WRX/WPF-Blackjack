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
    /// Interaction logic for GetName.xaml
    /// </summary>
    public partial class GetName : Window
    {
        //Delegate létrehozása, hogy átadhassuk a nevet a főmenünek.
        DataTransfer transferDel;
        public GetName(DataTransfer del)
        {
            InitializeComponent();
            transferDel = del;
        }

        private void GN_Submit_Click(object sender, RoutedEventArgs e)
        {
            //Ha nemm üres a név mező, akkor átadja a főmenünek.
            if (GN_Name.Text == String.Empty)
            {
                MessageBox.Show("Nem lehet üres a doboz!");
                return;
            }
            string data = GN_Name.Text;
            transferDel.Invoke(data);
            this.Close();
        }
    }
}
