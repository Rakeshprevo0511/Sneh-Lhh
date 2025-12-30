using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace snehrehab.Member
{
    public partial class LeaveSetting : System.Web.UI.Page
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
                LoadData();
            }
        }

        private void LoadData()
        {
            SqlCommand cmd = new SqlCommand("SELECT LeaveMsgOn, LeaveMailOn, LeaveMailFt, LeaveAMailTo, LeaveAMailBcc FROM SettingsMst;"); cmd.CommandType = CommandType.Text;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                txtMobileNos.Text = dt.Rows[0]["LeaveMsgOn"].ToString();
                txtEmailIds.Text = dt.Rows[0]["LeaveMailOn"].ToString();
                txtEmailFooter.Text = dt.Rows[0]["LeaveMailFt"].ToString();
                txtLeaveAMailTo.Text = dt.Rows[0]["LeaveAMailTo"].ToString();
                txtLeaveAMailBcc.Text = dt.Rows[0]["LeaveAMailBcc"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string query = "IF EXISTS(SELECT 1 FROM SettingsMst) " +
                            " BEGIN " +
                            " UPDATE SettingsMst SET LeaveMsgOn = @LeaveMsgOn, LeaveMailOn = @LeaveMailOn, LeaveMailFt = @LeaveMailFt, LeaveAMailTo = @LeaveAMailTo, LeaveAMailBcc = @LeaveAMailBcc " +
                            " END " +
                            " ELSE " +
                            " BEGIN " +
                            " INSERT INTO SettingsMst(LeaveMsgOn, LeaveMailOn, LeaveMailFt, LeaveAMailTo, LeaveAMailBcc) VALUES (@LeaveMsgOn, @LeaveMailOn, @LeaveMailFt, @LeaveAMailTo, @LeaveAMailBcc)" +
                            " END";
            SqlCommand cmd = new SqlCommand(query); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@LeaveMsgOn", SqlDbType.NVarChar, 500).Value = txtMobileNos.Text.Trim();
            cmd.Parameters.Add("@LeaveMailOn", SqlDbType.NVarChar, 4000).Value = txtEmailIds.Text.Trim();
            cmd.Parameters.Add("@LeaveMailFt", SqlDbType.NVarChar, 4000).Value = txtEmailFooter.Text.Trim();
            cmd.Parameters.Add("@LeaveAMailTo", SqlDbType.NVarChar, 4000).Value = txtLeaveAMailTo.Text.Trim();
            cmd.Parameters.Add("@LeaveAMailBcc", SqlDbType.NVarChar, 4000).Value = txtLeaveAMailBcc.Text.Trim();

            DbHelper.SqlDb db = new DbHelper.SqlDb(); int i = db.DbUpdate(cmd);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Leave alert setting saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/LeaveSetting.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}