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
    public partial class PackageBookingd : System.Web.UI.Page
    {
        int _loginID = 0; int _bookingID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _bookingID = SnehBLL.PatientPackage_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_bookingID > 0)
            {
                SnehBLL.PatientPackage_Bll DB = new SnehBLL.PatientPackage_Bll();
                if (DB.IsUsed(_bookingID))
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Patient package is already in use.";
                    Session[DbHelper.Configuration.messageTypeSession] = "3";

                    Response.Redirect(ResolveClientUrl("~/Member/PackageBookings.aspx"), true);
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/PackageBookings.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.PatientPackage_Bll DB = new SnehBLL.PatientPackage_Bll();
            int i = DB.Delete(_bookingID);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Patient package entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/PackageBookings.aspx"), true);
            }
            else if (i == -10)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Patient package is already in use.";
                Session[DbHelper.Configuration.messageTypeSession] = "3";

                Response.Redirect(ResolveClientUrl("~/Member/PackageBookings.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}
