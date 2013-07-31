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
    /// MemberDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MemberDialog : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;
        //if initialized with findmode true, it refers to a query and will create a query instead of an item.
        bool isFindMode = false;

        public MemberDialog(bool isFindMode)
        {
            InitializeComponent();
            this.isFindMode = isFindMode;

            //load values of the selected item(when modifying an item).
            Member item = Application.Current.Properties["MemberModifyItem"] as Member;
            if (item != null)
            {
                txbMemID.IsEnabled = false;
                txbMemID.Text = item.MemID.ToString();
                txbCardNO.Text = item.CardNO.ToString();
                txbMemName.Text = item.MemName.ToString();
                txbMemPhoneNO.Text = item.MemPhoneNO.ToString();
                txbMemAddr.Text = item.MemAddr.ToString();
                txbTotalAmount.Text = item.TotalAmount.ToString();
                dtpRegistrationDate.Value = item.RegistrationDATE;
                switch (item.MemGender.ToString())
                {
                    case "男":
                        cmbMemGender.SelectedIndex = 0;
                        break;
                    case "女":
                        cmbMemGender.SelectedIndex = 1;
                        break;
                }
            }

            if (isFindMode)
            {
                dtpRegistrationDateTo.IsEnabled = true;
                txbTotalAmountTo.IsEnabled = true;
            }
        }

        private void txbMemID_LostFocus(object sender, RoutedEventArgs e)
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

        private void txbCardNO_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                //card number is an int64
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

        private void txbMemName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dtpRegistrationDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbTotalAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                //total amount is a decimal.
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

        private void txbMemAddr_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbMemPhoneNO_LostFocus(object sender, RoutedEventArgs e)
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //clear dialog-result properties.
            Application.Current.Properties["MemberDialogQuery"] = null;
            Application.Current.Properties["MemberDialogItem"] = null;

            //create a query in order to execute in findmode.
            if (isFindMode)
            {
                int parseMemID;
                Int64 parseMemPhoneNO;
                decimal parseTotalAmount, parseTotalAmountTo;
                Int64 parseCardNO;
                bool bMemID, bMemPhoneNO, bTotalAmount, bTotalAmountTo, bCardNO;
                DateTime? dtFrom = dtpRegistrationDate.Value;
                DateTime? dtTo = dtpRegistrationDateTo.Value;

                //record whether parsing succeeds or not.
                bMemID = int.TryParse(txbMemID.Text, out parseMemID);
                bMemPhoneNO = Int64.TryParse(txbMemPhoneNO.Text, out parseMemPhoneNO);
                bTotalAmount = decimal.TryParse(txbTotalAmount.Text, out parseTotalAmount);
                bTotalAmountTo = decimal.TryParse(txbTotalAmountTo.Text, out parseTotalAmountTo);
                bCardNO = Int64.TryParse(txbCardNO.Text, out parseCardNO);

                //create query. note the use of (p?a:b) expressions. 
                //if parsing failed, ignore that predicate(in other words, (&& true)).
                var query = from member in db.Members
                            where (bMemID ? member.MemID == parseMemID : true)
                            && (bMemPhoneNO ? member.MemPhoneNO == parseMemPhoneNO : true)
                            && (bTotalAmount ? member.TotalAmount >= parseTotalAmount : true)
                            && (bTotalAmountTo ? member.TotalAmount < parseTotalAmountTo : true)
                            && (dtFrom != null ? member.RegistrationDATE >= dtFrom : true)
                            && (dtTo != null ? member.RegistrationDATE < dtTo : true)
                            && (bCardNO ? member.CardNO == parseCardNO : true)
                            && (cmbMemGender.SelectedIndex != -1 ? member.MemGender == (cmbMemGender.SelectedIndex == 0 ? "男" : "女") : true)
                            && (member.MemName.Contains(txbMemName.Text))
                            && (member.MemAddr.Contains(txbMemAddr.Text))
                            select member;

                //save the query into application properties in order to be fetched by the dialog-caller.
                Application.Current.Properties["MemberDialogQuery"] = query;
                this.Close();
            }
            else
            {
                //create a ChainStore prototype item if not in findmode.
                try
                {
                    if (cmbMemGender.SelectedIndex == -1)
                    {
                        MessageBox.Show("请选择性别。", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (dtpRegistrationDate.Value == null)
                    {
                        MessageBox.Show("请选择日期。", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //parse and create prototype item.
                    int parseMemID = int.Parse(txbMemID.Text);
                    Int64 parseMemPhoneNO = Int64.Parse(txbMemPhoneNO.Text);
                    decimal parseTotalAmount = decimal.Parse(txbTotalAmount.Text);
                    Int64 parseCardNO = Int64.Parse(txbCardNO.Text);
                    DateTime? dateTime = dtpRegistrationDate.Value;

                    Member item = new Member()
                    {
                        MemID = parseMemID,
                        MemPhoneNO = parseMemPhoneNO,
                        TotalAmount = parseTotalAmount,
                        CardNO = parseCardNO,
                        MemName = txbMemName.Text,
                        MemAddr = txbMemAddr.Text,
                        RegistrationDATE = dateTime,
                        MemGender = (cmbMemGender.SelectedIndex == 0 ? "男" : "女")
                    };

                    //save into app properties.
                    Application.Current.Properties["MemberDialogItem"] = item;
                    //close the dialog.
                    this.Close();
                }
                catch (Exception exc)
                {
                    //error message if parsing failed, and the dialog will not be closed(waits for correction).
                    Application.Current.Properties["MemberDialogItem"] = null;
                    MessageBox.Show(exc.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void dtpRegistrationDateTo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txbTotalAmountTo_LostFocus(object sender, RoutedEventArgs e)
        {
            //simple validation.
            if (isFindMode)
                return;
            TextBox txb = sender as TextBox;
            try
            {
                //total amount is a decimal.
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
    }
}
