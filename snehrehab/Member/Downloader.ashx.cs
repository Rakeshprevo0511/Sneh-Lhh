using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using System.IO;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Downloader : IHttpHandler, IRequiresSessionState
    {
        int _loginID = 0; string str = "<style>html{background: url(\"/images/bg_login.jpg\");}.alert{margin: 0 auto;max-width: 475px;margin-top: 10%;background: #F5F5F5;padding: 20px;color: #6E6666;font-family: calibri;font-size: 1.2em;min-height: 50px;border: 2px solid #A8A8A8;border-radius: 2px;font-weight: lighter;line-height: 30px;}</style>";

        public void ProcessRequest(HttpContext context)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">You required login to download file.</div>");
                context.Response.End();
            }
            else
            {
                string _fileName = "";
                if (context.Request.QueryString["file"] != null)
                {
                    _fileName = context.Request.QueryString["file"].ToString();
                }
                if (_fileName.Length > 0)
                {
                    string _filePath = context.Server.MapPath("~/Files/") + _fileName;
                    FileInfo fi = new FileInfo(_filePath);
                    if (fi.Exists)
                    {
                        context.Response.ClearContent();
                        context.Response.ClearHeaders();
                        context.Response.ContentType = DbHelper.MIMEType.Get(fi.Extension);
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + _fileName + "");
                        context.Response.WriteFile(_filePath);
                        context.Response.Flush();
                    }
                    else
                    {
                        context.Response.AddHeader("refresh", "5;/Member/");
                        context.Response.ContentType = "text/html";
                        context.Response.Write(str + "<div class=\"alert\">Unable to find attachment file, Please try again.</div>");
                        context.Response.End();
                    }
                }
                else
                {
                    context.Response.AddHeader("refresh", "5;/Member/");
                    context.Response.ContentType = "text/html";
                    context.Response.Write(str + "<div class=\"alert\">Invalid file download.</div>");
                    context.Response.End();
                }
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
