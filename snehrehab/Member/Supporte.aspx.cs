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

public partial class Member_Supporte : System.Web.UI.Page
{
    int _loginID = 0; int _ticketID = 0;

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
                _ticketID = SnehBLL.SupportTicket_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (_ticketID > 0)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        else
        {
            Response.Redirect(ResolveClientUrl("~/Member/Supports.aspx"), true);
        }
    }

    private void LoadData()
    {
        SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
        SnehDLL.SupportTicket_Dll SD = SB.Get(_ticketID);
        if (SD != null)
        {
            txtMessage.Text = SD.tMessage;
            txtRemark.Text = SD.Remark;
            if (txtStatus.Items.FindByValue(SD.cStatus.ToString()) != null)
            {
                txtStatus.SelectedValue = SD.cStatus.ToString();
            }

            if (SD.aFile.Trim().Length > 0)
            {
                Literal1.Text = "<a href=\"/Member/SupportFile.ashx?record=" + SD.UniqueID + "\"  target=\"_blank\" >" + SD.uFile + "</a>";
            }
            else
            {
                Literal1.Text = "N/A";
            }
        }
        else
        {
            Response.Redirect(ResolveClientUrl("~/Member/Supports.aspx"), true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _cStatus = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _cStatus); }
        SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
        int i = SB.Update(_ticketID, txtRemark.Text.Trim(), _cStatus);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "Support ticket status updated successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/Supports.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}