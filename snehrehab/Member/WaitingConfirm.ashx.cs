using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for WaitingConfirm
    /// </summary>
    public class WaitingConfirm : IHttpHandler, IRequiresSessionState
    {
        int _loginID = 0;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            rModel r = new rModel();
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                r.msg = "You need to login again into system.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            if (!SnehBLL.UserAccount_Bll.IsAdminOrReception())
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            int appointmentID = 0; 
            if (context.Request.QueryString["id"] != null)
            {
                if (DbHelper.Configuration.IsGuid(context.Request.QueryString["id"].ToString()))
                {
                    appointmentID = SnehBLL.AptWaiting_BAL.Check(context.Request.QueryString["id"].ToString());
                }
            }
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehDLL.Appointments_Dll AD = new SnehDLL.Appointments_Dll();
            foreach (SnehDLL.Appointments_Dll atd in ATB.GetWaitingListDr( appointmentID))
            {
                if (atd != null)
                {
                    foreach (SnehDLL.AppointmentTime_Dll AB in ATB.GetListDrByDate(atd.AppointmentDate,atd.doctorId))
                    {
                        if (atd != null)
                        {
                            if ((AB.AppointmentFrom >= atd.AppointmentFrom && AB.AppointmentFrom < atd.AppointmentUpto) || (AB.AppointmentUpto > atd.AppointmentFrom && AB.AppointmentUpto <= atd.AppointmentUpto))
                            {
                                r.msg = "This time is already selected for another appointment.";
                                context.Response.Write(JsonConvert.SerializeObject(r));
                                return;
                            }
                        }
                    }
                    foreach(SnehDLL.Appointments_Dll AB in ATB.GetListDrByDate_new(atd.AppointmentDate, atd.doctorId))
                    {
                        if(atd!=null)
                        {
                            if ((AB.Available1From >= atd.AppointmentFrom && AB.Available1From < atd.AppointmentUpto) || (AB.Available1Upto > atd.AppointmentFrom && AB.Available1Upto <= atd.AppointmentUpto))
                            {
                                r.msg = "This time is already selected for another appointment.";
                                context.Response.Write(JsonConvert.SerializeObject(r));
                                return;
                            }
                        }
                    }
                }
            }
            if (appointmentID <= 0)
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.AptWaiting_BAL B = new SnehBLL.AptWaiting_BAL();
            int i = B.Status(appointmentID, 1);
            if (i > 0)
            {
                r.status = true; r.msg = "Appointment confirmed successfully.";
                r.data = B.GetActucalID(appointmentID);
                context.Response.Write(JsonConvert.SerializeObject(r));
            }
            else
            {
                r.msg = "Unable to process, please try again.";
                context.Response.Write(JsonConvert.SerializeObject(r));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}