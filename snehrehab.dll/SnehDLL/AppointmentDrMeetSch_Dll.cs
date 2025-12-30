using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AppointmentDrMeetSch_Dll
    {
        public int ID { get; set; }
        public int AppointmentID { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public bool IsMain { get; set; }
        public int ShareType { get; set; }
        public float ShareAmount { get; set; }
    }
}
