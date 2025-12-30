using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Member_Doctord : System.Web.UI.Page
{
    int _loginID = 0; int _doctorID = 0;

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
                _doctorID = SnehBLL.DoctorMast_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (_doctorID > 0)
        {
            if (!IsPostBack)
            {

            }
        }
        else
        {
            Response.Redirect(ResolveClientUrl("~/Member/Doctors.aspx"), true);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
        int i = DB.Delete(_doctorID);
        if (i > 0)
        {
            SnehBLL.DoctorWeekOff_Bll DWB = new SnehBLL.DoctorWeekOff_Bll();
            DWB.Delete(_doctorID);

            Session[DbHelper.Configuration.messageTextSession] = "Doctor entry deleted successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/Doctors.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
        }
    }
}
