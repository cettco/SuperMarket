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
    /// ChainstoreWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChainstoreWindow : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;

        //store current query in order to refer when refreshing.
        Object currentQuery;

        public ChainstoreWindow()
        {
            InitializeComponent();
            ShowAll();
        }

        private void ShowAll()
        {
            var result = from Chainstore in db.ChainStores
                         select Chainstore;
            currentQuery = result;
            dataGrid.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = (currentQuery as IQueryable<ChainStore>).ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            ChainstoreDialog dlg = new ChainstoreDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<ChainStore> query = Application.Current.Properties["ChainstoreDialogQuery"] as IQueryable<ChainStore>;
            
            if (query != null)
            {
                currentQuery = query;
                dataGrid.ItemsSource = query.ToList();
            }
            
            Application.Current.Properties["ChainstoreDialogQuery"] = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ChainstoreDialog dlg = new ChainstoreDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            ChainStore newItem = Application.Current.Properties["ChainstoreDialogItem"] as ChainStore;
            if (newItem == null)
            {
                Application.Current.Properties["ChainstoreDialogItem"] = null;
                return;
            }
            
            db.ChainStores.Add(newItem);
            Application.Current.Properties["ChainstoreDialogItem"] = null;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                db.ChainStores.Remove(newItem);
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
            ChainStore item = dataGrid.SelectedItem as ChainStore;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["ChainstoreModifyItem"] = item;
                ChainstoreDialog dlg = new ChainstoreDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["ChainstoreModifyItem"] = null;

                ChainStore queryItem = Application.Current.Properties["ChainstoreDialogItem"] as ChainStore;

                if (queryItem == null)
                {
                    Application.Current.Properties["ChainstoreDialogItem"] = null;
                    return;
                }

                ChainStore modItem = db.ChainStores.First(value => value.StoreID == item.StoreID);
                if (modItem != null)
                {
                    modItem.StoreAddr = queryItem.StoreAddr;
                    modItem.StorePhoneNO = queryItem.StorePhoneNO;
                    modItem.StaffQuantity = queryItem.StaffQuantity;
                }
                
                Application.Current.Properties["ChainstoreDialogItem"] = null;
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
            ChainStore item = dataGrid.SelectedItem as ChainStore;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？","操作确认", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.ChainStores.Remove(item);
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
