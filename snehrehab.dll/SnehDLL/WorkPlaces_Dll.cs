using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class WorkPlaces_Dll
    {
        public int WorkplaceID { get; set; }
        public string UniqueID { get; set; }
        public string Workplace { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public bool IsEnabled { get; set; }
        public string Address { get; set; }
    }
}
