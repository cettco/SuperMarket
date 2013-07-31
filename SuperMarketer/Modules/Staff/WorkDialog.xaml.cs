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
    /// WorkDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WorkDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public WorkDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Work item = Application.Current.Properties["WorkModifyItem"] as Work;
            if (item != null)
            {
                txbStaffID.IsEnabled = false;
                txbStaffID.Text = item.StaffID.ToString();
                txbStoreID.Text = item.StoreID.ToString();
                txbAwards.Text = item.AwardsRecords.ToString();
                txbPunishment.Text = item.PunishmentRecords.ToString();
                dtpHireDate.Value = item.HireDATE;
            }

            if (isFindMode)
            {
                dtpHireDateTo.IsEnabled = true;
            }
        }

        private void txbStaffID_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbHireDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbAwards_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txbPunishment_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dtpHireDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //clear dialog-result properties.
            Application.Current.Properties["WorkDialogQuery"] = null;
            Application.Current.Properties["WorkDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseStaffID, parseStoreID;
                bool bStaffID, bStoreID;
                //record whether parsing succeeds or not.
                bStaffID = int.TryParse(txbStaffID.Text, out parseStaffID);
                bStoreID = int.TryParse(txbStoreID.Text, out parseStoreID);
                DateTime? dtFrom = dtpHireDate.Value;
                DateTime? dtTo = dtpHireDateTo.Value;

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from work in db.Works
                            where (bStaffID ? work.StaffID == parseStaffID : true)
                            && (bStoreID ? work.StoreID == parseStoreID : true)
                            && (dtFrom != null ? work.HireDATE >= dtFrom : true)
                            && (dtTo != null ? work.HireDATE < dtTo : true)
                            && (work.AwardsRecords.Contains(txbAwards.Text))
                            && (work.PunishmentRecords.Contains(txbPunishment.Text))
                            select work;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["WorkDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    //parse and create prototype item.
                    int parseStaffID = int.Parse(txbStaffID.Text);
                    int parseStoreID = int.Parse(txbStoreID.Text);
                    if (dtpHireDate.Value == null)
                    {
                        MessageBox.Show("日期不可为空。", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    Work item = new Work()
                    {
                        StaffID = parseStaffID,
                        StoreID = parseStoreID,
                        HireDATE = dtpHireDate.Value,
                        AwardsRecords = txbAwards.Text,
                        PunishmentRecords = txbPunishment.Text
                    };

                    //save into app properties.
                    Application.Current.Properties["WorkDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["WorkDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dtpHireDateTo_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
