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
    /// VendorDialog.xaml 的交互逻辑
    /// </summary>
    public partial class VendorDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of a ChainStore item.
        bool isFindMode = false;

        public VendorDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Vendor item = Application.Current.Properties["VendorModifyItem"] as Vendor;
            if (item != null)
            {
                txbVendorID.IsEnabled = false;
                txbVendorID.Text = item.VendorID.ToString();
                txbVendorName.Text = item.VendorName.ToString();
                txbVendorAddr.Text = item.VendorAddr.ToString();
                txbVendorPhoneNO.Text = item.VendorPhoneNO.ToString();
            }
        }

        private void txbVendorID_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbVendorName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbVendorAddr_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            //if (isFindMode)
            //    return;
            //TextBox txb = sender as TextBox;
            //try
            //{
            //    int.Parse(txb.Text);
            //    txb.Background = new SolidColorBrush(Colors.White);
            //    txb.ToolTip = null;
            //}
            //catch (Exception exc)
            //{
            //    txb.Background = new SolidColorBrush(Colors.Red);
            //    txb.ToolTip = exc.Message;
            //}
        }

        private void txbVendorPhoneNO_LostFocus(object sender, RoutedEventArgs e)
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
            Application.Current.Properties["VendorDialogQuery"] = null;
            Application.Current.Properties["VendorDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseID, parseVendorPhoneNo;
                //int parseVendorPhoneNo;
                bool bID, bVendorPhoneNo;
                //record whether parsing succeeds or not.
                bID = int.TryParse(txbVendorID.Text,out parseID);
                bVendorPhoneNo = int.TryParse(txbVendorPhoneNO.Text, out parseVendorPhoneNo);
                
                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from Vendors in db.Vendors
                            where (bID ? Vendors.VendorID == parseID : true)
                            && (bVendorPhoneNo ? Vendors.VendorPhoneNO == parseVendorPhoneNo : true)
                            //&& (bPhone ? Vendors.StorePhoneNO == parsePhone : true)
                            && (Vendors.VendorAddr.Contains(txbVendorAddr.Text))
                            && (Vendors.VendorName.Contains(txbVendorName.Text))
                            select Vendors;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["VendorDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    //parse and create prototype item.
                    int parseID = int.Parse(txbVendorID.Text);
                    int parseVendorPhoneNo = int.Parse(txbVendorPhoneNO.Text);
                    //int parsePhone = int.Parse(txbStorePhoneNO.Text);
                    Vendor item = new Vendor()
                    {
                        //StoreID = parseID,
                        ////StorePhoneNO = parsePhone,
                        //StorePhoneNO = txbStorePhoneNO.Text,
                        //StoreAddr = txbStoreAddr.Text,
                        //StaffQuantity = parseStaffQuantity
                        VendorID = parseID,
                        VendorName = txbVendorName.Text,
                        VendorAddr = txbVendorAddr.Text,
                        VendorPhoneNO = parseVendorPhoneNo
                    };

                    //save into app properties.
                    Application.Current.Properties["VendorDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["VendorDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
