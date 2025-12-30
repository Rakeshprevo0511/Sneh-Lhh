using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientChrges_Dll
    {
        public int ChargeID { get; set; }
        public string UniqueID { get; set; }
        public int PatientTypeID { get; set; }
        public float ChargeAmt { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUpto { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
