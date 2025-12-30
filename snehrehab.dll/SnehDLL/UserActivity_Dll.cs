using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class UserActivity_Dll
    {
        public int ActivityID { get; set; }
        public int UserID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remark { get; set; }
        public DateTime aDate { get; set; }
    }
}
