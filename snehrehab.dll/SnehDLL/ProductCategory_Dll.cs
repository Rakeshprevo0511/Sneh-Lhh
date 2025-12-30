using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class ProductCategory_Dll
    {
        public int CategoryID { get; set; }
        public string UniqueID { get; set; }
        public string Category { get; set; }
        public int ParentID { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
