using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class SessionMast_Dll
    {
        public int SessionID { get; set; }
        public string UniqueID { get; set; }
        public string SessionCode { get; set; }
        public string SessionName { get; set; }
        public bool IsPackage { get; set; }
        public bool IsEvaluation { get; set; }
        public bool MultipleDoctor { get; set; }
        public int SessionGroupID { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public bool IsPrebooking { get; set; }
        public bool IsFirstPre { get; set; }
        public bool TimeWise { get; set; }

        public int ChargeType { get; set; }
    }
}
