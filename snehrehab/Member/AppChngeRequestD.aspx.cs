using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace snehrehab.Member
{
    public partial class AppChngeRequestD : System.Web.UI.Page
    {
        int _loginID = 0; int _requestID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID == 3)
            {
                Response.Redirect(ResolveClientUrl("~/Member/"), true);
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _requestID = SnehBLL.AppointmentChangeRequest_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_requestID > 0)
            {
                if (!IsPostBack)
                {
                    SnehBLL.AppointmentChangeRequest_Bll ACB = new SnehBLL.AppointmentChangeRequest_Bll();
                    SnehDLL.AppointmentChangeRequest_Dll ACD = ACB.Get(_requestID);
                    if (ACD.RequestStatus != 0)
                    {
                        Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
                    }
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.AppointmentChangeRequest_Bll DB = new SnehBLL.AppointmentChangeRequest_Bll();
            int i = DB.Delete(_requestID);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Doctor change request entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}
