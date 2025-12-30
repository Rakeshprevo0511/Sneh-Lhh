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
using System.Globalization;

public partial class Member_PatientChrges : System.Web.UI.Page
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
            txtPatientType.Items.Clear(); txtPatientType.Items.Add(new ListItem("Select Registration", "-1"));
            SnehBLL.PatientTypes_Bll PTB = new SnehBLL.PatientTypes_Bll();
            foreach (SnehDLL.PatientTypes_Dll PTD in PTB.GetList())
            {
                txtPatientType.Items.Add(new ListItem(PTD.PatientType, PTD.PatientTypeID.ToString()));
            }

            LoadData();
        }
    }

    protected void ChargesGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ChargesGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        SnehBLL.PatientChrges_Bll DB = new SnehBLL.PatientChrges_Bll();
        ChargesGV.DataSource = DB.Search();
        ChargesGV.DataBind();
        if (ChargesGV.HeaderRow != null) { ChargesGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        update_charge.Visible = false;
        if (_loginID == DbHelper.Configuration.managerLoginId){ update_charge.Visible = true; }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int PatientTypeID = 0; if (txtPatientType.SelectedItem != null) { int.TryParse(txtPatientType.SelectedItem.Value, out PatientTypeID); }
        if (PatientTypeID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select patient registration type...", 2); return;
        }
        float _amount = 0; float.TryParse(txtAmount.Text.Trim(), out _amount);
        if (_amount <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter registration amount...", 2); return;
        }

        SnehDLL.PatientChrges_Dll PCD = new SnehDLL.PatientChrges_Dll();
        PCD.ChargeID = 0; PCD.UniqueID = ""; PCD.PatientTypeID = PatientTypeID;
        PCD.ChargeAmt = _amount;
        PCD.ValidFrom = new DateTime(2000, 01, 01);
        PCD.ValidUpto = new DateTime(2150, 01, 01);
        PCD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        PCD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        PCD.AddedBy = _loginID; PCD.ModifyBy = _loginID;

        SnehBLL.PatientChrges_Bll PCB = new SnehBLL.PatientChrges_Bll();
        int i = PCB.SetOnly(PCD);
        if (i > 0)
        {
            if (txtPatientType.Items.Count > 0) { txtPatientType.SelectedIndex = 0; }
            txtAmount.Text = ""; LoadData();
            DbHelper.Configuration.setAlert(Page, "Patient registration amount saved successfully...", 1);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
        }
    }
}
