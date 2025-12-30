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
    public partial class FeedBack : System.Web.UI.Page
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
            tab_FeedbackTo.Visible = false; tab_IssueType.Visible = false;

            txtFeedbackTo.Items.Clear();
            SnehBLL.FeedBackTo_Bll FB = new SnehBLL.FeedBackTo_Bll();
            foreach (SnehDLL.FeedBackTo_Dll FD in FB.GetList())
            {
                txtFeedbackTo.Items.Add(new ListItem(FD.ToName, FD.ToID.ToString()));
            }
            txtFeedbackTo.Items.Add(new ListItem("Other", "0"));

            txtIssueType.Items.Clear();
            SnehBLL.FeedBackType_Bll SB = new SnehBLL.FeedBackType_Bll();
            foreach (SnehDLL.FeedBackType_Dll SD in SB.GetList())
            {
                if (SD.IsEnabled)
                    txtIssueType.Items.Add(new ListItem(SD.TypeName, SD.TypeID.ToString()));
            }
            txtIssueType.Items.Add(new ListItem("Other", "0"));
        }

        protected void txtFeedbackTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _id = 0; if (txtFeedbackTo.SelectedItem != null) { int.TryParse(txtFeedbackTo.SelectedItem.Value, out _id); }
            if (_id > 0) { tab_FeedbackTo.Visible = false; } else { tab_FeedbackTo.Visible = true; }
        }

        protected void txtIssueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _id = 0; if (txtIssueType.SelectedItem != null) { int.TryParse(txtIssueType.SelectedItem.Value, out _id); }
            if (_id > 0) { tab_IssueType.Visible = false; } else { tab_IssueType.Visible = true; }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _feedbackTo = 0; if (txtFeedbackTo.SelectedItem != null) { int.TryParse(txtFeedbackTo.SelectedItem.Value, out _feedbackTo); }
            string _feedbackToOther = "";
            if (_feedbackTo <= 0)
            {
                _feedbackToOther = txtFeedbackToOther.Text.Trim();
                if (txtFeedbackToOther.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter your feedback to...", 2);
                    return;
                }
            }
            int _feedbackType = 0; if (txtIssueType.SelectedItem != null) { int.TryParse(txtIssueType.SelectedItem.Value, out _feedbackType); }
            string _feedbackTypeOther = "";
            if (_feedbackType <= 0)
            {
                if (txtIssueTypeOther.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter your feedback issue type...", 2);
                    return;
                }
                _feedbackTypeOther = txtIssueTypeOther.Text.Trim();
            }
            if (txtMessage.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter your feedback message...", 2);
                return;
            }
            SnehDLL.FeedBackMst_Dll FD = new SnehDLL.FeedBackMst_Dll();
            FD.TypeID = _feedbackType;
            FD.OtherTypeID = _feedbackTypeOther;
            FD.ToID = _feedbackTo;
            FD.OtherToID = _feedbackToOther;
            FD.cMessage = txtMessage.Text.Trim();
            FD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            FD.ModifyBy = _loginID;

            SnehBLL.FeedBackMst_Bll FB = new SnehBLL.FeedBackMst_Bll();
            int i = FB.New(FD);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Your feedback details submitted successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/FeedBack.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
    }
}
