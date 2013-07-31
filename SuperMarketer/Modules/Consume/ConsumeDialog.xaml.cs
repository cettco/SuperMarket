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
    /// ConsumeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConsumeDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public ConsumeDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Consume item = Application.Current.Properties["ConsumeModifyItem"] as Consume;
            if (item != null)
            {
                txbMerchID.IsEnabled = false;
                txbMerchID.Text = item.MerchID.ToString();
                txbMemID.Text = item.MemID.ToString();
                txbConsumeDate.Value = item.ConDATE;
                txbConsumeQuantity.Text = item.ConAmount.ToString();
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

        private void txbConsumeDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbConsumeQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                //consume amount is a decimal.
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //clear dialog-result properties.
            Application.Current.Properties["ConsumeDialogQuery"] = null;
            Application.Current.Properties["ConsumeDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseMerchID, parseVenderID;
                decimal parseConsumeQuantity;
                DateTime? parseConsumeDate;
                bool bMerchID, bVenderID, bConsumeQuantity;
                //record whether parsing succeeds or not.
                bMerchID = int.TryParse(txbMerchID.Text, out parseMerchID);
                bVenderID = int.TryParse(txbMemID.Text, out parseVenderID);
                parseConsumeDate = txbConsumeDate.Value;
                //bConsumeDate = int.TryParse(txbConsumeDate.Text, out parseConsumeDate);
                bConsumeQuantity = decimal.TryParse(txbConsumeQuantity.Text, out parseConsumeQuantity);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from Consume in db.Consumes
                            where (bMerchID ? Consume.MerchID == parseMerchID : true)
                            && (bVenderID ? Consume.MemID == parseVenderID : true)
                            && (parseConsumeDate != null ? Consume.ConDATE == parseConsumeDate : true)
                            && (bConsumeQuantity ? Consume.ConAmount == parseConsumeQuantity : true)
                            select Consume;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["ConsumeDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {

                    //parse and create prototype item.
                    int parseMerchID = int.Parse(txbMerchID.Text);
                    int parseVenderID = int.Parse(txbMemID.Text);
                    decimal parseConsumeQuantity = decimal.Parse(txbConsumeQuantity.Text);
                    DateTime? parseConsumeDate = txbConsumeDate.Value;

                    Consume item = new Consume()
                    {
                        MerchID = parseMerchID,
                        MemID = parseVenderID,
                        ConAmount = parseConsumeQuantity,
                        ConDATE = parseConsumeDate,
                    };

                    //save into app properties.
                    Application.Current.Properties["ConsumeDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["ConsumeDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
