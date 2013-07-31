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
    /// ConsumeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConsumeWindow : Window
    {
        StoreDBEntities mainDb = ((App)Application.Current).MainDb;
        StoreDBEntities db = new StoreDBEntities();

        //store current query in order to refer when refreshing.
        Object currentConsumeQuery;

        public ConsumeWindow()
        {
            InitializeComponent();
            currentConsumeQuery = from consume in db.Consumes
                                  select consume;
            ShowAll();
        }

        private void ShowAll()
        {
            //Tab Consume
            var result = from consume in db.Consumes
                         select consume;
            currentConsumeQuery = result;
            dataGridConsume.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            //Tab Consume
            dataGridConsume.ItemsSource = (currentConsumeQuery as IQueryable<Consume>).ToList();
        }

        private void btnConsumeShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void btnConsumeRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnConsumeQuery_Click(object sender, RoutedEventArgs e)
        {
            ConsumeDialog dlg = new ConsumeDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Consume> query = Application.Current.Properties["ConsumeDialogQuery"] as IQueryable<Consume>;

            if (query != null)
            {
                currentConsumeQuery = query;
                dataGridConsume.ItemsSource = query.ToList();
            }

            Application.Current.Properties["ConsumeDialogQuery"] = null;
        }

        private void btnConsumeAdd_Click(object sender, RoutedEventArgs e)
        {
            ConsumeDialog dlg = new ConsumeDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Consume newItem = Application.Current.Properties["ConsumeDialogItem"] as Consume;
            if (newItem == null)
            {
                Application.Current.Properties["ConsumeDialogItem"] = null;
                return;
            }

            try
            {
                db.Consumes.AddObject(newItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR");
                Application.Current.Properties["ConsumeDialogItem"] = null;
                return;
            }
            Application.Current.Properties["ConsumeDialogItem"] = null;
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

        private void btnConsumeModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Consume item = dataGridConsume.SelectedItem as Consume;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["ConsumeModifyItem"] = item;
                ConsumeDialog dlg = new ConsumeDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["ConsumeModifyItem"] = null;

                Consume queryItem = Application.Current.Properties["ConsumeDialogItem"] as Consume;

                if (queryItem == null)
                {
                    Application.Current.Properties["ConsumeDialogItem"] = null;
                    return;
                }

                Consume modItem = db.Consumes.First(value => value.MemID == item.MemID);
                if (modItem != null)
                {
                    modItem.MemID = queryItem.MemID;
                    modItem.ConDATE = queryItem.ConDATE;
                    modItem.ConAmount = queryItem.ConAmount;
                }

                Application.Current.Properties["ConsumeDialogItem"] = null;
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

        private void btnConsumeDelete_Click(object sender, RoutedEventArgs e)
        {
            Consume item = dataGridConsume.SelectedItem as Consume;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Consumes.DeleteObject(item);
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

        private void cmbConsumeDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridConsume == null)
                return;
            ComboBox cmb = sender as ComboBox;
            switch (cmb.SelectedIndex)
            {
                case 0:
                    dataGridConsume.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
                case 1:
                    dataGridConsume.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;
                case 2:
                    dataGridConsume.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
                    break;
            }
        }
    }
}
