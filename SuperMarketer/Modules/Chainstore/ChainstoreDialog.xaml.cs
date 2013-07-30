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
    /// ChainstoreDialog.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class ChainstoreDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of a ChainStore item.
        bool isFindMode = false;

        public ChainstoreDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            ChainStore item = Application.Current.Properties["ChainstoreModifyItem"] as ChainStore;
            if (item != null)
            {
                txbStoreID.IsEnabled = false;
                txbStoreID.Text = item.StoreID.ToString();
                txbStoreAddr.Text = item.StoreAddr;
                txbStorePhoneNO.Text = item.StorePhoneNO.ToString();
                txbStaffQuantity.Text = item.StaffQuantity.ToString();
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

        private void txbStoreAddr_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbStorePhoneNO_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbStaffQuantity_LostFocus(object sender, RoutedEventArgs e)
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
            Application.Current.Properties["ChainstoreDialogQuery"] = null;
            Application.Current.Properties["ChainstoreDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseID, parseStaffQuantity, parsePhone;
                bool bID, bStaffQuantity, bPhone;
                //record whether parsing succeeds or not.
                bID = int.TryParse(txbStoreID.Text,out parseID);
                bStaffQuantity = int.TryParse(txbStaffQuantity.Text, out parseStaffQuantity);
                bPhone = int.TryParse(txbStorePhoneNO.Text, out parsePhone);
                
                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from Chainstore in db.ChainStores
                            where (bID ? Chainstore.StoreID == parseID : true)
                            && (bStaffQuantity ? Chainstore.StaffQuantity == parseStaffQuantity : true)
                            && (bPhone ? Chainstore.StorePhoneNO == parsePhone : true)
                            && (Chainstore.StoreAddr.Contains(txbStoreAddr.Text))
                            select Chainstore;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["ChainstoreDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    //parse and create prototype item.
                    int parseID = int.Parse(txbStoreID.Text);
                    int parseStaffQuantity = int.Parse(txbStaffQuantity.Text);
                    int parsePhone = int.Parse(txbStorePhoneNO.Text);
                    ChainStore item = new ChainStore()
                    {
                        StoreID = parseID,
                        StorePhoneNO = parsePhone,
                        StoreAddr = txbStoreAddr.Text,
                        StaffQuantity = parseStaffQuantity
                    };

                    //save into app properties.
                    Application.Current.Properties["ChainstoreDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["ChainstoreDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
