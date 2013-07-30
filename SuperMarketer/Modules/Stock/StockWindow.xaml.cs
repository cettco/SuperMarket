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
    /// StockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StockWindow : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;

        //store current query in order to refer when refreshing.
        Object currentStockQuery;

        public StockWindow()
        {
            InitializeComponent();
            currentStockQuery = from Stock in db.Stocks
                                select Stock;
            ShowAll();
        }

        private void ShowAll()
        {
            //Tab Stock
            var result = from Stock in db.Stocks
                         select Stock;
            currentStockQuery = result;
            dataGridStock.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            //Tab Stock
            dataGridStock.ItemsSource = (currentStockQuery as IQueryable<Stock>).ToList();
        }

        private void btnStockShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void btnStockRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnStockQuery_Click(object sender, RoutedEventArgs e)
        {
            StockDialog dlg = new StockDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Stock> query = Application.Current.Properties["StockDialogQuery"] as IQueryable<Stock>;

            if (query != null)
            {
                currentStockQuery = query;
                dataGridStock.ItemsSource = query.ToList();
            }

            Application.Current.Properties["StockDialogQuery"] = null;
        }

        private void btnStockAdd_Click(object sender, RoutedEventArgs e)
        {
            StockDialog dlg = new StockDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Stock newItem = Application.Current.Properties["StockDialogItem"] as Stock;
            if (newItem == null)
            {
                Application.Current.Properties["StockDialogItem"] = null;
                return;
            }

            db.Stocks.Add(newItem);
            Application.Current.Properties["StockDialogItem"] = null;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                db.Stocks.Remove(newItem);
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

        private void btnStockModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Stock item = dataGridStock.SelectedItem as Stock;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["StockModifyItem"] = item;
                StockDialog dlg = new StockDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["StockModifyItem"] = null;

                Stock queryItem = Application.Current.Properties["StockDialogItem"] as Stock;

                if (queryItem == null)
                {
                    Application.Current.Properties["StockDialogItem"] = null;
                    return;
                }

                Stock modItem = db.Stocks.First(value => value.VendorID == item.VendorID);
                if (modItem != null)
                {
                    modItem.VendorID = queryItem.VendorID;
                    modItem.StockDate = queryItem.StockDate;
                    modItem.StockQuantity= queryItem.StockQuantity;
                }

                Application.Current.Properties["StockDialogItem"] = null;
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

        private void btnStockDelete_Click(object sender, RoutedEventArgs e)
        {
            Stock item = dataGridStock.SelectedItem as Stock;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Stocks.Remove(item);
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

        private void cmbStockDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridStock == null)
                return;
            ComboBox cmb = sender as ComboBox;
            switch (cmb.SelectedIndex)
            {
                case 0:
                    dataGridStock.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
                case 1:
                    dataGridStock.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;
                case 2:
                    dataGridStock.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
                    break;
            }
        }
    }
}
