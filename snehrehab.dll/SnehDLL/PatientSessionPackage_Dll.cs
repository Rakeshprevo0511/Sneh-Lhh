using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientSessionPackage_Dll
    {
        public int BookingID { get; set; }
        public string PackageCode { get; set; }
        public float AppointmentCharge { get; set; }
        public int AppointmentCount { get; set; }
        public float PackageAmount { get; set; }
        public float PackageBalance { get; set; }
        public int ValidityDays { get; set; }
        public float OneTimeAmt { get; set; }
        public int MaximumTime { get; set; }
    }
}
