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

public partial class Member_DoctorChrges : System.Web.UI.Page
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
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            txtDoctor.Items.Clear(); txtDoctor.Items.Add(new ListItem("Select Doctor", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtDoctor.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }

            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
            foreach (SnehDLL.SessionMast_Dll SMD in SMB.GetList())
            {
                txtSession.Items.Add(new ListItem(SMD.SessionName, SMD.SessionID.ToString()));
            }

        }
    }

    protected void txtDoctor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtSession.Items.Count > 0) { txtSession.SelectedIndex = 0; }

        LoadData();
    }

    private void LoadData()
    {
        if (txtSession.Items.Count > 0) { txtSession.SelectedIndex = 0; }
        int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }
        SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
        ChargesGV.DataSource = DB.getCharges(_doctorID);
        ChargesGV.DataBind();
        if (ChargesGV.HeaderRow != null) { ChargesGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }
        if (_doctorID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select doctor name...", 2); return;
        }
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        if (_sessionID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select session name...", 2); return;
        }
        int _typeID = 0; if (txtType.SelectedItem != null) { int.TryParse(txtType.SelectedItem.Value, out _typeID); }
        float _amount = 0; float.TryParse(txtAmount.Text.Trim(), out _amount);
        if (_amount <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter session charge amount...", 2); return;
        }

        SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
        if (DMB.SetCharges(_doctorID, _sessionID, _typeID, _amount) > 0)
        {
            LoadData(); txtAmount.Text = "";
            DbHelper.Configuration.setAlert(Page, "Doctor session charges saved successfully...", 1); return;
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2); return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int _chargeID = 0; LinkButton lk = (LinkButton)sender;
        if (lk != null) { int.TryParse(lk.CommandArgument, out _chargeID); }
        if (_chargeID > 0)
        {
            SnehBLL.DoctorMast_Bll SMB = new SnehBLL.DoctorMast_Bll();
            int i = SMB.SetCharges(_chargeID);
            if (i > 0)
            {
                LoadData();
                DbHelper.Configuration.setAlert(Page, "session charges for doctor's removed successfully...", 1);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}
