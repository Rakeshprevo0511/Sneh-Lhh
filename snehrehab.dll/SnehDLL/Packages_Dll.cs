using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Packages_Dll
    {
        public int PackageID { get; set; }
        public string UniqueID { get; set; }
        public string PackageCode { get; set; }
        public string cDescription { get; set; }
        public int PatientTypeID { get; set; }
        public int CategoryID { get; set; }
        public float PackageAmt { get; set; }
        public float OneTimeAmt { get; set; }
        public int Appointments { get; set; }
        public int ValidityDays { get; set; }
        public int MaximumTime { get; set; }
    }
}
