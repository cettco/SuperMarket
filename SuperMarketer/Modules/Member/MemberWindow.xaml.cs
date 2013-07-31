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
    /// MemberWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MemberWindow : Window
    {
        StoreDBEntities mainDb = ((App)Application.Current).MainDb;
        StoreDBEntities db = new StoreDBEntities();

        //store current query in order to refer when refreshing.
        Object currentQuery;

        public MemberWindow()
        {
            InitializeComponent();
            ShowAll();
        }

        private void ShowAll()
        {
            var result = from member in db.Members
                         select member;
            currentQuery = result;
            dataGrid.ItemsSource = result.ToList();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = (currentQuery as IQueryable<Member>).ToList();
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
            MemberDialog dlg = new MemberDialog(true);
            dlg.ShowDialog();
            //query returned by dialog.
            IQueryable<Member> query = Application.Current.Properties["MemberDialogQuery"] as IQueryable<Member>;

            if (query != null)
            {
                currentQuery = query;
                dataGrid.ItemsSource = query.ToList();
            }

            Application.Current.Properties["MemberDialogQuery"] = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MemberDialog dlg = new MemberDialog(false);
            dlg.ShowDialog();

            //new item prototype returned by dialog.
            Member newItem = Application.Current.Properties["MemberDialogItem"] as Member;
            if (newItem == null)
            {
                Application.Current.Properties["MemberDialogItem"] = null;
                return;
            }

            try
            {
                db.Members.AddObject(newItem);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "ERROR");
                Application.Current.Properties["MemberDialogItem"] = null;
                return;
            }
            Application.Current.Properties["MemberDialogItem"] = null;
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
            Member item = dataGrid.SelectedItem as Member;
            if (item != null)
            {
                //pass selected item to dialog in order to initialize with current values.
                Application.Current.Properties["MemberModifyItem"] = item;
                MemberDialog dlg = new MemberDialog(false);
                dlg.ShowDialog();
                Application.Current.Properties["MemberModifyItem"] = null;

                Member queryItem = Application.Current.Properties["MemberDialogItem"] as Member;

                if (queryItem == null)
                {
                    Application.Current.Properties["MemberDialogItem"] = null;
                    return;
                }

                Member modItem = db.Members.First(value => value.MemID == item.MemID);
                if (modItem != null)
                {
                    modItem.MemName = queryItem.MemName;
                    modItem.MemGender = queryItem.MemGender;
                    modItem.MemAddr = queryItem.MemAddr;
                    modItem.MemPhoneNO = queryItem.MemPhoneNO;
                    modItem.CardNO = queryItem.CardNO;
                    modItem.RegistrationDATE = queryItem.RegistrationDATE;
                    modItem.TotalAmount = queryItem.TotalAmount;
                }

                Application.Current.Properties["MemberDialogItem"] = null;
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
            Member item = dataGrid.SelectedItem as Member;
            if (item != null)
            {
                //confirm.
                if (MessageBox.Show("确认删除该条信息吗？", "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;
                db.Members.DeleteObject(item);
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
