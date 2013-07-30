//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperMarketer
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChainStore
    {
        public ChainStore()
        {
            this.Inventories = new HashSet<Inventory>();
            this.Merchandises = new HashSet<Merchandise>();
            this.Staffs = new HashSet<Staff>();
            this.Works = new HashSet<Work>();
        }
    
        public int StoreID { get; set; }
        public string StoreAddr { get; set; }
        public Nullable<int> StorePhoneNO { get; set; }
        public Nullable<int> StaffQuantity { get; set; }
    
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Merchandise> Merchandises { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual ICollection<Work> Works { get; set; }
    }
}
