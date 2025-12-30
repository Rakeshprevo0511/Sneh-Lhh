<%@ WebHandler Language="C#" Class="SupportFile" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;

public class SupportFile : IHttpHandler, IRequiresSessionState {
    
    int _loginID = 0; string str = "<style>html{background: url(\"/images/bg_login.jpg\");}.alert{margin: 0 auto;max-width: 475px;margin-top: 10%;background: #F5F5F5;padding: 20px;color: #6E6666;font-family: calibri;font-size: 1.2em;min-height: 50px;border: 2px solid #A8A8A8;border-radius: 2px;font-weight: lighter;line-height: 30px;}</style>";

    public void ProcessRequest(HttpContext context)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            context.Response.AddHeader("refresh", "5;/Member/");
            context.Response.ContentType = "text/html";
            context.Response.Write(str + "<div class=\"alert\">You required login to download attachment.</div>");
            context.Response.End();
        }
        else
        {
            int _ticketID = 0;
            if (context.Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(context.Request.QueryString["record"].ToString()))
                {
                    _ticketID = SnehBLL.SupportTicket_Bll.Check(context.Request.QueryString["record"].ToString());
                }
            }
            if (_ticketID > 0)
            {
                SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
                SnehDLL.SupportTicket_Dll SD = SB.Get(_ticketID);
                if (SD != null)
                {
                    if (SD.aFile.Trim().Length > 0)
                    {
                        string _filePath = SB.FilePath(true) + SD.aFile;
                        FileInfo fi = new FileInfo(_filePath);

                        context.Response.ClearContent();
                        context.Response.ClearHeaders();
                        context.Response.ContentType = DbHelper.MIMEType.Get(fi.Extension);
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + SD.uFile + "");
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
                    context.Response.Write(str + "<div class=\"alert\">Unable to find attachment file, Please try again.</div>");
                    context.Response.End();
                }
            }
            else
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">Invalid attachment download.</div>");
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