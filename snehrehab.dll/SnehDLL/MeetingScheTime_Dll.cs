using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class MeetingScheTime_Dll
    {
        public int TimeID { get; set; }
        public string TimeFrom { get; set; }
        public string TimeUpto { get; set; }
        public TimeSpan TimeHour { get; set; }
        public TimeSpan Available1Upto { get; set; }
        public TimeSpan Available1From { get; set; }
        public string Available1FromChar { get; set; }
        public string Available1UptoChar { get; set; }
        public int AppointmentID { get; set; }
        public string UniqueID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentFrom { get; set; }
        public DateTime AppointmentUpto { get; set; }
        public string ScheduleType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
