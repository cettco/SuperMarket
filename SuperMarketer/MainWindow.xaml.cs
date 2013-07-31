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

namespace SuperMarketer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            
        }

        private void btnStore_Click(object sender, RoutedEventArgs e)
        {
            ChainstoreWindow newWindow = new ChainstoreWindow();
            newWindow.ShowDialog();
        }

        private void btnVendor_Click(object sender, RoutedEventArgs e)
        {
            VendorWindow newWindow = new VendorWindow();
            newWindow.ShowDialog();
        }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow newWindow = new StaffWindow();
            newWindow.ShowDialog();
        }

        private void btnMerchandise_Click(object sender, RoutedEventArgs e)
        {
            MerchandiseWindow newWindow = new MerchandiseWindow();
            newWindow.ShowDialog();
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow newWindow = new InventoryWindow();
            newWindow.ShowDialog();
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            StockWindow newWindow = new StockWindow();
            newWindow.ShowDialog();
        }

        private void btnConsume_Click(object sender, RoutedEventArgs e)
        {
            ConsumeWindow newWindow = new ConsumeWindow();
            newWindow.ShowDialog();
        }

        private void btnMember_Click(object sender, RoutedEventArgs e)
        {
            MemberWindow newWindow = new MemberWindow();
            newWindow.ShowDialog();
        }

        private void btnLogio_Click(object sender, RoutedEventArgs e)
        {
            LogioWindow newWindow = new LogioWindow();
            newWindow.ShowDialog();
        }
    }
}
