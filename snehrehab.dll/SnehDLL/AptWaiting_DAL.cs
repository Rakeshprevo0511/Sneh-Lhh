using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AptWaiting_DAL
    {
        public int AppointmentID { get; set; }
        public string UniqueID { get; set; }
        public int PatientID { get; set; }
        public int SessionID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public DateTime AppointmentFrom { get; set; }
        public DateTime AppointmentUpto { get; set; }
        public int Duration { get; set; }
        public string Narration { get; set; }
        public int AppointmentStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
