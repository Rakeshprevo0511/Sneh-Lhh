using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Banks_Dll
    {
        public int BankID { get; set; }
        public string UniqueID { get; set; }
        public string BankName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
