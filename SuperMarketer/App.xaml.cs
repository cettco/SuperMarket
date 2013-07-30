using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SuperMarketer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        StoreDBEntities mainDb = new StoreDBEntities();

        public StoreDBEntities MainDb
        {
            get { return mainDb; }
            set { mainDb = value; }
        }
    }
}
