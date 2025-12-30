using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for UpdateMail
    /// </summary>
    public class UpdateMail : IHttpHandler, IRequiresSessionState
    {

        int _loginID = 0; int PatientID = 0; string mailid = string.Empty;
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
            var request = context.Request;
            var requestBody = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            var jsonSerializer = new JavaScriptSerializer();
            MailSent evnt = jsonSerializer.Deserialize<MailSent>(requestBody);
            if (evnt == null)
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
            int i = PB.UpdateMail(evnt.PatientID, evnt.Type, evnt.MailId);
            if (i > 0)
            {
                r.status = true; r.msg = "Updated successfully.";
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

        private class MailSent
        {
            public int PatientID { get; set; }
            public string MailId { get; set; }
            public int Type { get; set; }
        }
    }
}