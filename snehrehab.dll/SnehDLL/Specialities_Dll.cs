using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Specialities_Dll
    {
        public int SpecialityID { get; set; }
        public string UniqueID { get; set; }
        public string Speciality { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
