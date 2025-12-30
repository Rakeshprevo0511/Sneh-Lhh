<%@ WebHandler Language="C#" Class="logout" %>

using System;
using System.Web;
using System.Web.SessionState;

public class logout : IHttpHandler, IRequiresSessionState {

    public void ProcessRequest(HttpContext context)
    {
        SnehBLL.UserAccount_Bll.LogOut();
        context.Response.Redirect(DbHelper.Configuration.LogoutURL, true);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}