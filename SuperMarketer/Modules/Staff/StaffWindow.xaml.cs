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
    /// StaffWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StaffWindow : Window
    {
        StoreDBEntities db = ((App)Application.Current).MainDb;

        //store current query in order to refer when refreshing.
        Object currentStaffQuery;
        Object currentWorkQuery;

        public StaffWindow()
        {
            InitializeComponent();
            tabControl.SelectedIndex = 0;
            currentStaffQuery = from staff in db.Staffs
                                select staff;
            currentWorkQuery = from work in db.Works
                               select work;
            ShowAll();
        }

        private void ShowAll()
        {
            if (tabControl.SelectedIndex == 0)
            {
                //Tab Staff
                var result = from staff in db.Staffs
                             select staff;
                currentStaffQuery = result;
                dataGridStaff.ItemsSource = result.ToList();
            }
            else if (tabControl.SelectedIndex == 1)
            {
                //Tab Work
                var result = from work in db.Works
                             select work;
                currentWorkQuery = result;
                dataGridWork.ItemsSource = result.ToList();
            }
        }

        private void Refresh()
        {
            if (tabControl.SelectedIndex == 0)
            {
                //Tab Staff
                dataGridStaff.ItemsSource = (currentStaffQuery as IQueryable<Staff>).ToList();
            }
            else if (tabControl.SelectedIndex == 1)
            {
                //Tab Work
                dataGridWork.ItemsSource = (currentWorkQuery as IQueryable<Work>).ToList();
            }
        }

        private void btnStaffShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void btnStaffRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnStaffQuery_Click(object sender, RoutedEventArgs e)
        {
            StaffDialog dlg = new StaffDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Staff> query = Application.Current.Properties["StaffDialogQuery"] as IQueryable<Staff>;

            if (query != null)
            {
                currentStaffQuery = query;
                dataGridStaff.ItemsSource = query.ToList();
            }

            Application.Current.Properties["StaffDialogQuery"] = null;
        }

        private void btnStaffAdd_Click(object sender, RoutedEventArgs e)
        {
            StaffDialog dlg = new StaffDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Staff newItem = Application.Current.Properties["StaffDialogItem"] as Staff;
            if (newItem == null)
            {
                Application.Current.Properties["StaffDialogItem"] = null;
                return;
            }

            db.Staffs.Add(newItem);
            Application.Current.Properties["StaffDialogItem"] = null;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                db.Staffs.Remove(newItem);
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

        private void btnStaffModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Staff item = dataGridStaff.SelectedItem as Staff;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["StaffModifyItem"] = item;
                StaffDialog dlg = new StaffDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["StaffModifyItem"] = null;

                Staff queryItem = Application.Current.Properties["StaffDialogItem"] as Staff;

                if (queryItem == null)
                {
                    Application.Current.Properties["StaffDialogItem"] = null;
                    return;
                }

                Staff modItem = db.Staffs.First(value => value.StaffID == item.StaffID);
                if (modItem != null)
                {
                    modItem.StoreID = queryItem.StoreID;
                    modItem.StaffName = queryItem.StaffName;
                    modItem.StaffAddr = queryItem.StaffAddr;
                    modItem.StaffGender = queryItem.StaffGender;
                    modItem.StaffAge = queryItem.StaffAge;
                    modItem.StaffWage = queryItem.StaffWage;
                    modItem.StaffPhoneNO = queryItem.StaffPhoneNO;
                }

                Application.Current.Properties["StaffDialogItem"] = null;
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

        private void btnStaffDelete_Click(object sender, RoutedEventArgs e)
        {
            Staff item = dataGridStaff.SelectedItem as Staff;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Staffs.Remove(item);
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

        private void btnWorkShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void btnWorkRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnWorkQuery_Click(object sender, RoutedEventArgs e)
        {
            WorkDialog dlg = new WorkDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Work> query = Application.Current.Properties["WorkDialogQuery"] as IQueryable<Work>;

            if (query != null)
            {
                currentWorkQuery = query;
                dataGridWork.ItemsSource = query.ToList();
            }

            Application.Current.Properties["WorkDialogQuery"] = null;
        }

        private void btnWorkAdd_Click(object sender, RoutedEventArgs e)
        {
            WorkDialog dlg = new WorkDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Work newItem = Application.Current.Properties["WorkDialogItem"] as Work;
            if (newItem == null)
            {
                Application.Current.Properties["WorkDialogItem"] = null;
                return;
            }

            db.Works.Add(newItem);
            Application.Current.Properties["WorkDialogItem"] = null;
            try
            {
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                db.Works.Remove(newItem);
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

        private void btnWorkModify_Click(object sender, RoutedEventArgs e)
        {
            //get selected item.
            Work item = dataGridWork.SelectedItem as Work;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["WorkModifyItem"] = item;
                WorkDialog dlg = new WorkDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["WorkModifyItem"] = null;

                Work queryItem = Application.Current.Properties["WorkDialogItem"] as Work;

                if (queryItem == null)
                {
                    Application.Current.Properties["WorkDialogItem"] = null;
                    return;
                }

                Work modItem = db.Works.First(value => value.StaffID == item.StaffID);
                if (modItem != null)
                {
                    modItem.StoreID = queryItem.StoreID;
                    modItem.HireDate = queryItem.HireDate;
                    modItem.AwardsRecords = queryItem.AwardsRecords;
                    modItem.PunishmentRecords = queryItem.PunishmentRecords;
                }

                Application.Current.Properties["WorkDialogItem"] = null;
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

        private void btnWorkDelete_Click(object sender, RoutedEventArgs e)
        {
            Work item = dataGridWork.SelectedItem as Work;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Works.Remove(item);
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

        private void cmbWorkDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridWork == null)
                return;
            ComboBox cmb = sender as ComboBox;
            switch (cmb.SelectedIndex)
            {
                case 0:
                    dataGridWork.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
                case 1:
                    dataGridWork.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;
                case 2:
                    dataGridWork.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
                    break;
            }
        }

        private void cmbStaffDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridStaff == null)
                return;
            ComboBox cmb = sender as ComboBox;
            switch (cmb.SelectedIndex)
            {
                case 0:
                    dataGridStaff.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    break;
                case 1:
                    dataGridStaff.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
                    break;
                case 2:
                    dataGridStaff.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
                    break;
            }
        }
    }
}
