using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class DrYesterdayAppointEdit : System.Web.UI.Page
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
                    _ticketID = SnehBLL.SupportTicket_Bll.CheckYesAppoint(Request.QueryString["record"].ToString());
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
                Response.Redirect(ResolveClientUrl("~/Member/DrYesterdayAppoint.aspx"), true);
            }
        }

        private void LoadData()
        {
            SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
            SnehDLL.SupportTicket_Dll SD = SB.GetYestAppoint(_ticketID);
            if (SD != null)
            {
                txtMessage.Text = SD.yNarration;
                txtRemark.Text = SD.yRemark;
                if (txtStatus.Items.FindByValue(SD.yStatus.ToString()) != null)
                {
                    txtStatus.SelectedValue = SD.yStatus.ToString();
                }

                //if (SD.aFile.Trim().Length > 0)
                //{
                //    Literal1.Text = "<a href=\"/Member/SupportFile.ashx?record=" + SD.UniqueID + "\"  target=\"_blank\" >" + SD.uFile + "</a>";
                //}
                //else
                //{
                //    Literal1.Text = "N/A";
                //}
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/DrYesterdayAppoint.aspx"), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _yStatus = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _yStatus); }
            SnehBLL.SupportTicket_Bll SB = new SnehBLL.SupportTicket_Bll();
            int i = SB.Update_DrYesterdayAppoint(_ticketID, txtRemark.Text.Trim(), _yStatus);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Dr Yesterday Appointment status updated successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/DrYesterdayAppoint.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
    }
}