using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler, IRequiresSessionState
    {
        string UniqueID = string.Empty; int _loginID = 0;
        public void ProcessRequest(HttpContext context)
        {
            rModel r = new rModel();
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                r.msg = "You need to login again into system.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }

            if (context.Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(context.Request.QueryString["record"].ToString()))
                {
                    UniqueID = context.Request.QueryString["record"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(UniqueID))
            {
                HttpPostedFile postedFile = context.Request.Files[0];
                int i = 0;
                string _filePath = "d_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                string _filename = System.IO.Path.GetFileName(postedFile.FileName);
                SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll(); SnehBLL.Degree_Bll DB = new SnehBLL.Degree_Bll();
                if (RB.SaveFileNew(ref postedFile, _filePath))
                {
                    i = DB.Update(UniqueID, _filePath, _filename);
                }
                if (i > 0)
                {
                    //r.status = true;
                    //r.msg = "Certificate updated successfully.";
                    //context.Response.Write(JsonConvert.SerializeObject(r));
                    context.Response.Write("OK");
                    return;
                }
                else
                {
                    //r.msg = "Unable to process, please try again.";
                    //context.Response.Write(JsonConvert.SerializeObject(r));
                    context.Response.Write("Unable to process, please try again.");
                    return;
                }
            }
            else
            {
                //r.msg = "Unable to process, please try again.";
                //context.Response.Write(JsonConvert.SerializeObject(r));
                //return;
                context.Response.Write("Unable to process, please try again.");
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