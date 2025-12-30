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

public partial class Member_UserAcc : System.Web.UI.Page
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
            SnehBLL.UserCategory_Bll BNB = new SnehBLL.UserCategory_Bll();
            txtAccountType.Items.Clear(); txtAccountType.Items.Add(new ListItem("Select Account Type", "-1"));
            foreach (SnehDLL.UserCategory_Dll PSD in BNB.GetList())
            {
                txtAccountType.Items.Add(new ListItem(PSD.CategoryName, PSD.UserCatID.ToString()));
            }

            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }


        }
    }


    protected void txtAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFullName.Text = ""; txtMail.Text = ""; txtMobileNo.Text = "";
        tb_Therapist.Visible = false; if (txtTherapist.Items.Count > 0) { txtTherapist.SelectedIndex = 0; }
        int _categoryID = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _categoryID); }
        if (_categoryID == 3)
        {
            tb_Therapist.Visible = true;
        }
    }

    protected void txtTherapist_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFullName.Text = ""; txtMail.Text = ""; txtMobileNo.Text = "";
        int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
        if (_doctorID > 0)
        {
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            SnehDLL.DoctorMast_Dll DMD = DMB.Get(_doctorID);
            if (DMD != null)
            {
                txtFullName.Text = DMD.PreFix + " " + DMD.FullName;
                txtMail.Text = DMD.MailID; 
                txtMobileNo.Text = DMD.MobileNo;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _categoryID = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _categoryID); }
        if (_categoryID <= 0) { DbHelper.Configuration.setAlert(Page, "Please select account type and try again...", 2); return; }
        int _doctorID = 0;
        if (_categoryID == 3)
        {
            if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            if (_doctorID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select Therapist and try again...", 2); return;
            }
        }
        if (txtFullName.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter full name of account holder...", 2); return;
        }
        if (txtLoginName.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter account login name...", 2); return;
        }
        SnehBLL.UserAccount_Bll UB = new SnehBLL.UserAccount_Bll();
        if (UB.LoginChk(0, txtLoginName.Text.Trim()))
        {
            DbHelper.Configuration.setAlert(Page, "Enter login name is not available, please try another...", 2); return;
        }
        if (txtPassword.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter account password...", 2); return;
        }
        if (txtPassword.Text.Trim().Length <= 3)
        {
            DbHelper.Configuration.setAlert(Page, "Account password should be minimum 4 character...", 2); return;
        }
        SnehDLL.UserAccount_Dll UD = new SnehDLL.UserAccount_Dll();
        UD.UserID = 0; UD.UniqueID = ""; UD.DoctorID = _doctorID;
        UD.FullName = txtFullName.Text.Trim();
        UD.MobileNo = txtMobileNo.Text.Trim();
        UD.MailID = txtMail.Text.Trim();
        UD.LoginName = txtLoginName.Text.Trim();
        UD.LoginPwd = txtPassword.Text;
        UD.LastLogin = ""; UD.UserCatID = _categoryID;
        UD.IsMain = false; UD.IsEnabled = true;
        UD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        UD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        UD.AddedBy = _loginID; UD.ModifyBy = _loginID;

        int i = UB.Set(UD);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "User account created successfully...";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
        else if (i == -1)
        {
            DbHelper.Configuration.setAlert(Page, "Therapist account is already available...", 2); return;
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
    
}
