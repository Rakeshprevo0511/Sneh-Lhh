using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for MailIDGet
    /// </summary>
    public class MailIDGet : IHttpHandler, IRequiresSessionState
    {

        int _loginID = 0; int PatientID = 0; int user = 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            rModel r = new rModel(); _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                r.msg = "You need to login again into system.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            if (context.Request.QueryString["id"] != null)
            {
                int.TryParse(context.Request.QueryString["id"].ToString(), out PatientID);
            }
            if (context.Request.QueryString["user"] != null)
            {
                int.TryParse(context.Request.QueryString["user"].ToString(), out user);
            }
            if (PatientID > 0 && user > 0)
            {
                string maild = string.Empty;
                maild = SnehBLL.PatientMast_Bll.GetMail(PatientID, user);

                //string maild = string.Empty;
                r.status = true; r.msg = "Mail Detail.";
                r.data = new
                {
                    MailID = maild
                };

                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            else
            {
                r.msg = "Unable to process, Please try again.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
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