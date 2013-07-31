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
    /// MerchandiseDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MerchandiseDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public MerchandiseDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Merchandise item = Application.Current.Properties["MerchandiseModifyItem"] as Merchandise;
            if (item != null)
            {
                txbMerchID.IsEnabled = false;
                txbMerchID.Text = item.MerchID.ToString();
                txbBarCode.Text = item.BarCode.ToString();
                txbMerchName.Text = item.MerchName.ToString();
                txbMerchPrice.Text = item.MerchPrice.ToString();
                txbPromotionPrice.Text = item.PromotionPrice.ToString();
                txbStoreID.Text = item.StoreID.ToString();
                txbVendorID.Text = item.VendorID.ToString();
                dtpPromotionDateS.Value = item.PromotionDATES;
                dtpPromotionDateE.Value = item.PromotionDATEE;
            }

            if (isFindMode)
            {
                txbPromotionPriceTo.IsEnabled = true;
                txbMerchPriceTo.IsEnabled = true;
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

        private void txbMerchName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbMerchPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                //merchandise price is a decimal.
                decimal.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbMerchPriceTo_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                decimal.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbBarCode_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                Int64.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbPromotionPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                decimal.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void txbPromotionPriceTo_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                decimal.Parse(txb.Text);
                txb.Background = new SolidColorBrush(Colors.White);
                txb.ToolTip = null;
            }
            catch (Exception exc)
            {
                txb.Background = new SolidColorBrush(Colors.Red);
                txb.ToolTip = exc.Message;
            }
        }

        private void dtpPromotionDateS_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dtpPromotionDateE_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //clear dialog-result properties.
            Application.Current.Properties["MerchandiseDialogQuery"] = null;
            Application.Current.Properties["MerchandiseDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseMerchID, parseStoreID, parseVendorID;
                decimal parsePrice, parsePriceTo, parsePrPrice, parsePrPriceTo;
                Int64 parseBarCode;
                bool bMerchID, bStoreID, bVendorID, bPrice, bPriceTo, bPrPrice, bPrPriceTo, bBarCode;
                DateTime? dtFrom = dtpPromotionDateS.Value;
                DateTime? dtTo = dtpPromotionDateE.Value;

                //record whether parsing succeeds or not.
                bMerchID = int.TryParse(txbMerchID.Text, out parseMerchID);
                bStoreID = int.TryParse(txbStoreID.Text, out parseStoreID);
                bVendorID = int.TryParse(txbVendorID.Text, out parseVendorID);
                bPrice = decimal.TryParse(txbMerchPrice.Text, out parsePrice);
                bPriceTo = decimal.TryParse(txbMerchPriceTo.Text, out parsePriceTo);
                bPrPrice = decimal.TryParse(txbPromotionPrice.Text, out parsePrPrice);
                bPrPriceTo = decimal.TryParse(txbPromotionPriceTo.Text, out parsePrPriceTo);
                bBarCode = Int64.TryParse(txbBarCode.Text, out parseBarCode);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from merchandise in db.Merchandises
                            where (bMerchID ? merchandise.MerchID == parseMerchID : true)
                            && (bStoreID ? merchandise.StoreID == parseStoreID : true)
                            && (bVendorID ? merchandise.VendorID == parseVendorID : true)
                            && (bPrice ? merchandise.MerchPrice >= parsePrice : true)
                            && (bPriceTo ? merchandise.MerchPrice < parsePriceTo : true)
                            && (bPrPrice ? merchandise.PromotionPrice >= parsePrPrice : true)
                            && (bPrPriceTo ? merchandise.PromotionPrice < parsePrPriceTo : true)
                            && (bBarCode ? merchandise.BarCode == parseBarCode : true)
                            && (dtFrom != null ? merchandise.PromotionDATES >= dtFrom : true)
                            && (dtTo != null ? merchandise.PromotionDATEE < dtTo : true)
                            && (merchandise.MerchName.Contains(txbMerchName.Text))
                            select merchandise;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["MerchandiseDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a prototype item if not in findmode.
                try
                {
                    if (dtpPromotionDateE.Value == null || dtpPromotionDateS.Value == null)
                    {
                        MessageBox.Show("请选择日期。", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //parse and create prototype item.
                    int parseMerchID = int.Parse(txbMerchID.Text);
                    int parseStoreID = int.Parse(txbStoreID.Text);
                    int parseVendorID = int.Parse(txbVendorID.Text);
                    decimal parsePrice = decimal.Parse(txbMerchPrice.Text);
                    decimal parsePrPrice = decimal.Parse(txbPromotionPrice.Text);
                    Int64 parseBarCode = Int64.Parse(txbBarCode.Text);
                    DateTime? dtS = dtpPromotionDateS.Value;
                    DateTime? dtE = dtpPromotionDateE.Value;

                    Merchandise item = new Merchandise()
                    {
                        MerchID = parseMerchID,
                        StoreID = parseStoreID,
                        VendorID = parseVendorID,
                        MerchPrice = parsePrice,
                        PromotionPrice = parsePrPrice,
                        BarCode = parseBarCode,
                        PromotionDATES = dtS,
                        PromotionDATEE = dtE,
                        MerchName = txbMerchName.Text
                    };

                    //save into app properties.
                    Application.Current.Properties["MerchandiseDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["MerchandiseDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
