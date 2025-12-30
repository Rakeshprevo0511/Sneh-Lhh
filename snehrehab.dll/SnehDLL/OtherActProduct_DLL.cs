using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class OtherActProduct_DLL
    {
        public Int64 ProductID { get; set; }
        public string UniqueID { get; set; }
        public string ProductName { get; set; }
        public Int64 CategoryID { get; set; }
        public float UnitPrice { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
