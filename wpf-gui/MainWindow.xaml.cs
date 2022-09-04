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
using System.IO;
using System.IO.Pipes;
using System.ComponentModel;

// DataContext: https://stackoverflow.com/questions/4590446/how-do-i-set-a-viewmodel-on-a-window-in-xaml-using-datacontext-property
// views suggestion: https://social.msdn.microsoft.com/Forums/en-US/16d4e51a-dbea-4710-b5c0-cefd9bf9e66c/displaying-different-views-in-mainwindow?forum=wpf
// example of views: https://social.technet.microsoft.com/wiki/contents/articles/30898.simple-navigation-technique-in-wpf-using-mvvm.aspx
// include xaml from different file: https://stackoverflow.com/questions/10978079/load-control-style-from-separate-file-in-wpf
// same as above: https://stackoverflow.com/questions/13246602/datatemplate-in-a-separate-resourcedictionary

namespace wpf_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ipc ipc = new ipc();
        public PortfolioView portfolioView = new PortfolioView();
        public MainWindow() 
        { 
            InitializeComponent(); 
            switchToView(portfolioView); // start in portfolio view
        }

        public void switchToViewTmp<T>() where T : ViewBase, new() /* create and switch to a temporary view */
        {
            var v = new T();
            ActiveView.Content = v;
            v.setMainWindowReference(this);
        }
        public void switchToView(ViewBase v) /* switch to an existing view */
        {
            ActiveView.Content = v;
            v.setMainWindowReference(this);
        }

        

        private async void TestBtn_ActionAsync(object sender, RoutedEventArgs e) 
        {
            await ipc.send("test button clicked"); // TEST
        }
    }

    public class ViewBase : UserControl
    {
        protected MainWindow mw = null!; // somewhat unsafe, but using switchToView<V>() ensures that this always gets set 
        public void setMainWindowReference(MainWindow mainWindow_)
        {
            mw = mainWindow_;
        }
    }
}
