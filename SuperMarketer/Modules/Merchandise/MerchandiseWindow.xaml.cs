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
    /// MerchandiseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MerchandiseWindow : Window
    {
        StoreDBEntities mainDb = ((App)Application.Current).MainDb;
        StoreDBEntities db = new StoreDBEntities();

        //store current query in order to refer when refreshing.
        Object currentQuery;

        public MerchandiseWindow()
        {
            InitializeComponent();
            ShowAll();
        }

        private void ShowAll()
        {
            var result = from merchandise in db.Merchandises
                         select merchandise;
            currentQuery = result;
            dataGrid.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = (currentQuery as IQueryable<Merchandise>).ToList();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            MerchandiseDialog dlg = new MerchandiseDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Merchandise> query = Application.Current.Properties["MerchandiseDialogQuery"] as IQueryable<Merchandise>;

            if (query != null)
            {
                currentQuery = query;
                dataGrid.ItemsSource = query.ToList();
            }

            Application.Current.Properties["MerchandiseDialogQuery"] = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MerchandiseDialog dlg = new MerchandiseDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Merchandise newItem = Application.Current.Properties["MerchandiseDialogItem"] as Merchandise;
            if (newItem == null)
            {
                Application.Current.Properties["MerchandiseDialogItem"] = null;
                return;
            }

            try
            {
                db.Merchandises.AddObject(newItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR");
                Application.Current.Properties["MerchandiseDialogItem"] = null;
                return;
            }
            Application.Current.Properties["MerchandiseDialogItem"] = null;
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
            Merchandise item = dataGrid.SelectedItem as Merchandise;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["MerchandiseModifyItem"] = item;
                MerchandiseDialog dlg = new MerchandiseDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["MerchandiseModifyItem"] = null;

                Merchandise queryItem = Application.Current.Properties["MerchandiseDialogItem"] as Merchandise;

                if (queryItem == null)
                {
                    Application.Current.Properties["MerchandiseDialogItem"] = null;
                    return;
                }

                Merchandise modItem = db.Merchandises.First(value => value.MerchID == item.MerchID);

                if (modItem != null)
                {
                    modItem.BarCode = queryItem.BarCode;
                    modItem.MerchName = queryItem.MerchName;
                    modItem.MerchPrice = queryItem.MerchPrice;
                    modItem.PromotionDATEE = queryItem.PromotionDATEE;
                    modItem.PromotionDATES = queryItem.PromotionDATES;
                    modItem.PromotionPrice = queryItem.PromotionPrice;
                    modItem.StoreID = queryItem.StoreID;
                    modItem.VendorID = queryItem.VendorID;
                }

                Application.Current.Properties["MerchandiseDialogItem"] = null;
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
            Merchandise item = dataGrid.SelectedItem as Merchandise;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                //

                db.Merchandises.DeleteObject(item);
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
    }
}
