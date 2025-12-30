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
    public partial class Packaged : System.Web.UI.Page
    {
        int _loginID = 0; int _packageID = 0;

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
                    _packageID = SnehBLL.Packages_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_packageID > 0)
            {
                SnehBLL.Packages_Bll DB = new SnehBLL.Packages_Bll();
                if (DB.IsUsed(_packageID))
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Package entry is already used in booking.";
                    Session[DbHelper.Configuration.messageTypeSession] = "3";

                    Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.Packages_Bll DB = new SnehBLL.Packages_Bll();
            int i = DB.Delete(_packageID);
            if (i > 0)
            {
                SnehBLL.SessionToPackage_Bll DWB = new SnehBLL.SessionToPackage_Bll();
                DWB.Delete(_packageID);

                Session[DbHelper.Configuration.messageTextSession] = "Package entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
            }
            else if (i == -10)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Package entry is already used in booking.";
                Session[DbHelper.Configuration.messageTypeSession] = "3";

                Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}
