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
    /// StaffDialog.xaml 的交互逻辑
    /// </summary>
    public partial class StaffDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public StaffDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Staff item = Application.Current.Properties["StaffModifyItem"] as Staff;
            if (item != null)
            {
                txbStaffID.IsEnabled = false;
                txbStaffID.Text = item.StaffID.ToString();
                txbStoreID.Text = item.StoreID.ToString();
                txbStaffName.Text = item.StaffName.ToString();
                txbStaffAge.Text = item.StaffAge.ToString();
                txbStaffAddr.Text = item.StaffAddr.ToString();
                txbStaffWage.Text = item.StaffWage.ToString();
                txbStaffPhoneNO.Text = item.StaffPhoneNO.ToString();
                switch(item.StaffGender.ToString())
                {
                    case "M":
                        cmbStaffGender.SelectedIndex = 0;
                        break;
                    case "F":
                        cmbStaffGender.SelectedIndex = 1;
                        break;
                }
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

        private void txbStaffName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbStaffAge_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbStaffWage_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbStaffAddr_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbStaffPhoneNO_LostFocus(object sender, RoutedEventArgs e)
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
            Application.Current.Properties["StaffDialogQuery"] = null;
            Application.Current.Properties["StaffDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseStaffID, parseStoreID, parseStaffAge, parseStaffPhoneNO;
                decimal parseStaffWage;
                bool bStaffID, bStoreID, bStaffAge, bStaffPhoneNO, bStaffWage;
                //record whether parsing succeeds or not.
                bStaffID = int.TryParse(txbStaffID.Text, out parseStaffID);
                bStoreID = int.TryParse(txbStoreID.Text, out parseStoreID);
                bStaffAge = int.TryParse(txbStaffAge.Text, out parseStaffAge);
                bStaffPhoneNO = int.TryParse(txbStaffPhoneNO.Text, out parseStaffPhoneNO);
                bStaffWage = decimal.TryParse(txbStaffWage.Text, out parseStaffWage);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from staff in db.Staffs
                            where (bStaffID ? staff.StaffID == parseStaffID : true)
                            && (bStoreID ? staff.StoreID == parseStoreID : true)
                            && (bStaffAge ? staff.StaffAge == parseStaffAge : true)
                            && (bStaffPhoneNO ? staff.StaffPhoneNO == parseStaffPhoneNO : true)
                            && (bStaffWage ? staff.StaffWage == parseStaffWage : true)
                            && (cmbStaffGender.SelectedIndex != -1 ? staff.StaffGender == (cmbStaffGender.SelectedIndex == 0 ? "M" : "F") : true)
                            && (staff.StaffName.Contains(txbStaffName.Text))
                            && (staff.StaffAddr.Contains(txbStaffAddr.Text))
                            select staff;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["StaffDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    if(cmbStaffGender.SelectedIndex==-1)
                    {
                        MessageBox.Show("请选择性别。","ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }

                    //parse and create prototype item.
                    int parseStaffID = int.Parse(txbStaffID.Text);
                    int parseStoreID = int.Parse(txbStoreID.Text);
                    int parseStaffAge = int.Parse(txbStaffAge.Text);
                    int parseStaffPhoneNO = int.Parse(txbStaffPhoneNO.Text);
                    decimal parseStaffWage = decimal.Parse(txbStaffWage.Text);

                    Staff item = new Staff()
                    {
                        StaffID = parseStaffID,
                        StoreID = parseStoreID,
                        StaffAge = parseStaffAge,
                        StaffPhoneNO = parseStaffPhoneNO,
                        StaffWage = parseStaffWage,
                        StaffName = txbStaffName.Text,
                        StaffAddr = txbStaffAddr.Text,
                        StaffGender = (cmbStaffGender.SelectedIndex == 0 ? "M" : "F")
                    };

                    //save into app properties.
                    Application.Current.Properties["StaffDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["StaffDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
