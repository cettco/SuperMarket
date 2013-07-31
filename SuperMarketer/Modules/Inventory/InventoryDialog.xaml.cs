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
    /// InventoryDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InventoryDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of a ChainStore item.
        bool isFindMode = false;

        public InventoryDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Inventory item = Application.Current.Properties["InventoryModifyItem"] as Inventory;
            if (item != null)
            {
                txbMerchID.IsEnabled = false;
                txbStoreID.IsEnabled = false;
                txbMerchID.Text = item.MerchID.ToString();
                txbStoreID.Text = item.StoreID.ToString();
                txbInvenQuantity.Text = item.InvenQuantity.ToString();
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

        private void txbStoreID_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbInvenQuantity_LostFocus(object sender, RoutedEventArgs e)
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
            Application.Current.Properties["InventoryDialogQuery"] = null;
            Application.Current.Properties["InventoryDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseMerchID, parseStoreID, parseInvenQuantity;
                bool bMerchID, bStoreID, bInvenQuantity;
                bMerchID = int.TryParse(txbMerchID.Text, out parseMerchID);
                bStoreID = int.TryParse(txbStoreID.Text, out parseStoreID);
                bInvenQuantity = int.TryParse(txbStoreID.Text, out parseInvenQuantity);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from Inventories in db.Inventories
                            where (bMerchID ? Inventories.MerchID == parseMerchID : true)
                            && (bStoreID ? Inventories.StoreID == parseStoreID : true)
                            && (bInvenQuantity ? Inventories.InvenQuantity == parseInvenQuantity : true)
                            select Inventories;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["InventoryDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    int parseMerchID = int.Parse(txbMerchID.Text);
                    int parseStoreID = int.Parse(txbStoreID.Text);
                    int parseInvenQuantity = int.Parse(txbInvenQuantity.Text);
                    Inventory item = new Inventory()
                    {
                        MerchID = parseMerchID,
                        StoreID = parseStoreID,
                        InvenQuantity = parseInvenQuantity
                    };

                    //save into app properties.
                    Application.Current.Properties["InventoryDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["InventoryDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
