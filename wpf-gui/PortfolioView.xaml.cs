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

namespace wpf_gui
{
    /// <summary>
    /// Interaction logic for PortfolioView.xaml
    /// </summary>
    public partial class PortfolioView : ViewBase
    {
        List<StockDisplayData> stocks = new List<StockDisplayData>();
        public PortfolioView()
        {
            InitializeComponent();
            portfolioListBox.DataContext = this;
            stocks.Add(new StockDisplayData() { ticker = "ABC", name = "ABC Corporation" });
            stocks.Add(new StockDisplayData() { ticker = "CBA", name = "C. B .A Enterprises International" });
            stocks.Add(new StockDisplayData() { ticker = "ACB", name = "ACB Holdings" });
            stocks.Add(new StockDisplayData() { ticker = "BCA", name = "BCA Unlimited" });
            stocks.Add(new StockDisplayData() { ticker = "XYZ", name = "X Group LTD." });
            portfolioListBox.ItemsSource = stocks;
        }
        private void ToNewTransactionView_Action(object sender, RoutedEventArgs e) { mw.switchToView<NewTransactionView>(); }
        
    }

    public class StockDisplayData 
    {
        public string ticker { get; set; } = "";
        public string name { get; set; } = "";
        public string price { get; set; } = "0";
        public string buyPrice { get; set; } = "0";
    }

}
