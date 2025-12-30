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

public partial class Member_Support : System.Web.UI.Page
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

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtMessage.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }
        string _fileName = ""; string _oldFileName = "";
        SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
        if (txtFile.HasFile)
        {
            _fileName = DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + txtFile.FileName.Substring(txtFile.FileName.LastIndexOf('.'));
            if (SB.Upload(ref txtFile, _fileName))
            {
                _oldFileName = txtFile.FileName;
            }
            else
            {
                _fileName = "";
                DbHelper.Configuration.setAlert(Page, "Unable to upload your attachment please try again...", 2); return;
            }
        }

        SnehDLL.SupportTicket_Dll SD = new SnehDLL.SupportTicket_Dll();
        int i = SB.Set(_loginID, txtMessage.Text.Trim(), _fileName, _oldFileName, DateTime.UtcNow.AddMinutes(330), _loginID);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "Your message has been submitted successfully...";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/SupportMy.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2); return;
        }
    }
}