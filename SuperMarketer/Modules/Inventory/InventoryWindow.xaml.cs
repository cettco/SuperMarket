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
    /// InventoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InventoryWindow : Window
    {
        StoreDBEntities mainDb = ((App)Application.Current).MainDb;
        StoreDBEntities db = new StoreDBEntities();

        //store current query in order to refer when refreshing.
        Object currentQuery;

        public InventoryWindow()
        {
            InitializeComponent();
            ShowAll();
        }

        private void ShowAll()
        {
            var result = from Inventories in db.Inventories
                         select Inventories;
            currentQuery = result;
            dataGrid.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = (currentQuery as IQueryable<Inventory>).ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            InventoryDialog dlg = new InventoryDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Inventory> query = Application.Current.Properties["InventoryDialogQuery"] as IQueryable<Inventory>;

            if (query != null)
            {
                currentQuery = query;
                dataGrid.ItemsSource = query.ToList();
            }

            Application.Current.Properties["InventoryDialogQuery"] = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            InventoryDialog dlg = new InventoryDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Inventory newItem = Application.Current.Properties["InventoryDialogItem"] as Inventory;
            if (newItem == null)
            {
                Application.Current.Properties["InventoryDialogItem"] = null;
                return;
            }

            try
            {
                db.Inventories.AddObject(newItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR");
                Application.Current.Properties["InventoryDialogItem"] = null;
                return;
            }
            Application.Current.Properties["InventoryDialogItem"] = null;
            try
            {
                db.SaveChanges(false);
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
                db = new StoreDBEntities();
                ShowAll();
                return;
            }
            db.AcceptAllChanges();
            Refresh();
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Inventory item = dataGrid.SelectedItem as Inventory;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["InventoryModifyItem"] = item;
                InventoryDialog dlg = new InventoryDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["InventoryModifyItem"] = null;

                Inventory queryItem = Application.Current.Properties["InventoryDialogItem"] as Inventory;

                if (queryItem == null)
                {
                    Application.Current.Properties["InventoryDialogItem"] = null;
                    return;
                }

                Inventory modItem = db.Inventories.First(value => value.StoreID == item.StoreID && value.MerchID == item.MerchID);
                if (modItem != null)
                {
                    modItem.InvenQuantity = queryItem.InvenQuantity;
                }

                Application.Current.Properties["InventoryDialogItem"] = null;
                try
                {
                    db.SaveChanges(false);
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
                    db = new StoreDBEntities();
                    ShowAll();
                    return;
                }
                db.AcceptAllChanges();
            }

            Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Inventory item = dataGrid.SelectedItem as Inventory;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Inventories.DeleteObject(item);
                try
                {
                    db.SaveChanges(false);
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
                    db = new StoreDBEntities();
                    ShowAll();
                    return;
                }
                db.AcceptAllChanges();
            }
            Refresh();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }
    }
}
