using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AptWaitingDoctor_DAL
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public bool IsMain { get; set; }
        public int ShareType { get; set; }
        public float ShareAmount { get; set; }
    }
}
