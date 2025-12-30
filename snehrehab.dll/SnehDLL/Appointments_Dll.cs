using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Appointments_Dll
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
        public int BulkPackageID { get; set; }
        public long BulkBookingID { get; set; }

        public string ScheduleType { get; set; }
        public DateTime Available1Upto { get; set; }
        public DateTime Available1From { get; set; }
        public TimeSpan Available1FromTime { get; set; }
        public TimeSpan Available1UptoTime { get; set; }
        public string Available1FromChar { get; set; }
        public string Available1UptoChar { get; set; }
        public int doctorId { get; set; }
    }
}
