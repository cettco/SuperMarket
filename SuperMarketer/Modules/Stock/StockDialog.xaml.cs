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

namespace SuperMarketer
{
    /// <summary>
    /// StockDialog.xaml 的交互逻辑
    /// </summary>
    public partial class StockDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public StockDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Stock item = Application.Current.Properties["StockModifyItem"] as Stock;
            if (item != null)
            {
                txbMerchID.IsEnabled = false;
                txbMerchID.Text = item.MerchID.ToString();
                txbVenderID.Text = item.VendorID.ToString();
                txbStockDate.Value = item.StockDATE;
                txbStockQuantity.Text = item.StockQuantity.ToString();
            }
        }

        private void txbMerchID_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                int.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbVenderID_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                int.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbStockDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbStockQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                int.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //clear dialog-result properties.
            Application.Current.Properties["StockDialogQuery"] = null;
            Application.Current.Properties["StockDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseMerchID, parseVenderID, parseStockQuantity;
                DateTime? parseStockDate;
                bool bMerchID, bVenderID, bStockQuantity;
                //record whether parsing succeeds or not.
                bMerchID = int.TryParse(txbMerchID.Text, out parseMerchID);
                bVenderID = int.TryParse(txbVenderID.Text, out parseVenderID);
                parseStockDate = txbStockDate.Value;
                //bStockDate = int.TryParse(txbStockDate.Text, out parseStockDate);
                bStockQuantity = int.TryParse(txbStockQuantity.Text, out parseStockQuantity);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from Stock in db.Stocks
                            where (bMerchID ? Stock.MerchID == parseMerchID : true)
                            && (bVenderID ? Stock.VendorID == parseVenderID : true)
                            && (parseStockDate != null ? Stock.StockDATE == parseStockDate : true)
                            && (bStockQuantity ? Stock.StockQuantity == parseStockQuantity : true)
                            select Stock;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["StockDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {

                    //parse and create prototype item.
                    int parseMerchID = int.Parse(txbMerchID.Text);
                    int parseVenderID = int.Parse(txbVenderID.Text);
                    int parseStockQuantity = int.Parse(txbStockQuantity.Text);
                    DateTime? parseStockDate = txbStockDate.Value;

                    Stock item = new Stock()
                    {
                        MerchID = parseMerchID,
                        VendorID = parseVenderID,
                        StockQuantity = parseStockQuantity,
                        StockDATE = parseStockDate,
                    };

                    //save into app properties.
                    Application.Current.Properties["StockDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["StockDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}