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
    /// VendorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VendorWindow : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;

        //store current query in order to refer when refreshing.
        Object currentQuery;

        public VendorWindow()
        {
            InitializeComponent();
            ShowAll();
        }

        private void ShowAll()
        {
            var result = from Vendors in db.Vendors
                         select Vendors;
            currentQuery = result;
            dataGrid.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = (currentQuery as IQueryable<Vendor>).ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            VendorDialog dlg = new VendorDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Vendor> query = Application.Current.Properties["VendorDialogQuery"] as IQueryable<Vendor>;
            
            if (query != null)
            {
                currentQuery = query;
                dataGrid.ItemsSource = query.ToList();
            }
            
            Application.Current.Properties["VendorDialogQuery"] = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            VendorDialog dlg = new VendorDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Vendor newItem = Application.Current.Properties["VendorDialogItem"] as Vendor;
            if (newItem == null)
            {
                Application.Current.Properties["VendorDialogItem"] = null;
                return;
            }
            
            db.Vendors.Add(newItem);
            Application.Current.Properties["VendorDialogItem"] = null;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                db.Vendors.Remove(newItem);
                Exception innerExc = exc;
                while (!(innerExc is System.Data.SqlClient.SqlException))
                {
                    if (innerExc.InnerException == null)
                    {
                        break;
                    }
                    innerExc = innerExc.InnerException;
                }
                MessageBox.Show(innerExc.Message, "ERROR");
            }
            Refresh();
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Vendor item = dataGrid.SelectedItem as Vendor;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["VendorModifyItem"] = item;
                VendorDialog dlg = new VendorDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["VendorModifyItem"] = null;

                Vendor queryItem = Application.Current.Properties["VendorDialogItem"] as Vendor;

                if (queryItem == null)
                {
                    Application.Current.Properties["VendorDialogItem"] = null;
                    return;
                }

                Vendor modItem = db.Vendors.First(value => value.VendorID == item.VendorID);
                if (modItem != null)
                {
                    modItem.VendorName = queryItem.VendorName;
                    modItem.VendorAddr = queryItem.VendorAddr;
                    modItem.VendorPhoneNO = queryItem.VendorPhoneNO;
                }
                
                Application.Current.Properties["VendorDialogItem"] = null;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception exc)
                {
                    //get the inner SqlException.
                    Exception innerExc = exc;
                    while (!(innerExc is System.Data.SqlClient.SqlException))
                    {
                        if (innerExc.InnerException == null)
                        {
                            break;
                        }
                        innerExc = innerExc.InnerException;
                    }
                    MessageBox.Show(innerExc.Message, "ERROR");
                }
            }

            Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Vendor item = dataGrid.SelectedItem as Vendor;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？","操作确认", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Vendors.Remove(item);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception exc)
                {
                    Exception innerExc = exc;
                    while (!(innerExc is System.Data.SqlClient.SqlException))
                    {
                        if (innerExc.InnerException == null)
                        {
                            break;
                        }
                        innerExc = innerExc.InnerException;
                    }
                    MessageBox.Show(innerExc.Message, "ERROR");
                }
            }
            Refresh();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }
    }
}
