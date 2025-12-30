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
    public partial class SettingContact : System.Web.UI.Page
    {
        int _loginID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                SnehBLL.SettingsMst_Bll SB = new SnehBLL.SettingsMst_Bll();
                SnehDLL.SettingsMst_Dll SD = SB.Get();
                if (SD != null)
                {
                    txtMailID.Text = SD.RptMailID;
                    txtMobileNo.Text = SD.RptMobileNo;
                }
            }
        }

        protected void btnMobileNo_Click(object sender, EventArgs e)
        {
            string _numbers = txtMobileNo.Text.Trim();
            SnehBLL.SettingsMst_Bll SB = new SnehBLL.SettingsMst_Bll();
            int i = SB.SetRptMobileNo(_numbers);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Mobile number saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/SettingContact.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void btnMailID_Click(object sender, EventArgs e)
        {
            string _mails = txtMailID.Text.Trim();
            SnehBLL.SettingsMst_Bll SB = new SnehBLL.SettingsMst_Bll();
            int i = SB.SetRptMailID(_mails);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Mail address saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/SettingContact.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
    }
}