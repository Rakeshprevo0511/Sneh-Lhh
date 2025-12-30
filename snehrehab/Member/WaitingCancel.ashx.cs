using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for WaitingCancel
    /// </summary>
    public class WaitingCancel : IHttpHandler, IRequiresSessionState
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

            if (appointmentID <= 0)
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.AptWaiting_BAL B = new SnehBLL.AptWaiting_BAL();
            int i = B.Status(appointmentID, 10);
            if (i > 0)
            {
                r.status = true; r.msg = "Appointment cancelled successfully.";
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