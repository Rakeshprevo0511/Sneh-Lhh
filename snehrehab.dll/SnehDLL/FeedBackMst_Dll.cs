using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class FeedBackMst_Dll
    {
        public int FeedBackID { get; set; }
        public string UniqueID { get; set; }
        public int TypeID { get; set; }
        public string OtherTypeID { get; set; }
        public int ToID { get; set; }
        public string OtherToID { get; set; }
        public string cMessage { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int StatusID { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusRemark { get; set; }
        public int StatusBy { get; set; }
    }
}
