using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AppointmentChangeRequest_Dll
    {
        public int RequestID { get; set; }
        public string UniqueID { get; set; }
        public int AppointmentID { get; set; }
        public int ReqDoctorID { get; set; }
        public int AssignToDoctorID { get; set; }
        public string Remarks { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int RequestStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusRemark { get; set; }
    }
}
