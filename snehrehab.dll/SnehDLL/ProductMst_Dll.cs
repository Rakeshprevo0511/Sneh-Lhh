using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class ProductMst_Dll
    {
        public int ProductID { get; set; }
        public string UniqueID { get; set; }
        public string ProductName { get; set; }
        public float UnitPrice { get; set; }
        public int CategoryID { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
