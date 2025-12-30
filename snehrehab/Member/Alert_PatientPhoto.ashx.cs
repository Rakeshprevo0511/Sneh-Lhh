using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for Alert_PatientPhoto
    /// </summary>
    public class Alert_PatientPhoto : IHttpHandler, IRequiresSessionState
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
            SqlCommand cmd = new SqlCommand("PatientMast_PhotoAlert"); cmd.CommandType = CommandType.StoredProcedure;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            var data = new List<dynamic>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data.Add(new
                {
                    id = dt.Rows[i]["UniqueID"].ToString(),
                    pcode = dt.Rows[i]["PatientCode"].ToString(),
                    name = dt.Rows[i]["FullName"].ToString(),
                    rcode = dt.Rows[i]["RegistrationCode"].ToString(),
                    mob = dt.Rows[i]["MobileNo"].ToString(),
                    mail = dt.Rows[i]["MailID"].ToString(),
                });
            }
            r.status = true; r.msg = "Patient list"; r.data = data;
            context.Response.Write(JsonConvert.SerializeObject(r));
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