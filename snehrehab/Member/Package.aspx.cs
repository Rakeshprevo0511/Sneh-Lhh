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
    public partial class Package : System.Web.UI.Page
    {
        int _loginID = 0; int _packageID = 0; public string _headerText = "";

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
                    _packageID = SnehBLL.Packages_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_packageID > 0) { _headerText = "Edit Package Detail"; } else { _headerText = "Add New Package"; }

            if (!IsPostBack)
            {
                LoadForm();
                if (_packageID > 0)
                {
                    LoadPackage(); btnRevise.Visible = true;
                }
            }
        }

        private void LoadForm()
        {
            SnehBLL.PatientTypes_Bll PTB = new SnehBLL.PatientTypes_Bll();
            txtPatientType.Items.Clear(); txtPatientType.Items.Add(new ListItem("Any Type", "-1"));
            foreach (SnehDLL.PatientTypes_Dll PTD in PTB.GetList())
            {
                txtPatientType.Items.Add(new ListItem(PTD.PatientType, PTD.PatientTypeID.ToString()));
            }

            SnehBLL.PatientCategory_Bll PCB = new SnehBLL.PatientCategory_Bll();
            txtCategory.Items.Clear(); txtCategory.Items.Add(new ListItem("Any Category", "-1"));
            foreach (SnehDLL.PatientCategory_Dll PCD in PCB.GetList())
            {
                txtCategory.Items.Add(new ListItem(PCD.CategoryName, PCD.CategoryID.ToString()));
            }

            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            txtToSession.Items.Clear(); //txtToSession.Items.Add(new ListItem("Any Category", "-1"));
            foreach (SnehDLL.SessionMast_Dll SMD in SMB.GetList().Where(s => s.IsEvaluation == true || s.IsPackage == true).ToList())
            {
                txtToSession.Items.Add(new ListItem(SMD.SessionCode, SMD.SessionID.ToString()));
            }
        }

        private void LoadPackage()
        {
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
            if (PKD != null)
            {
                txtPackageCode.Text = PKD.PackageCode;
                txtcDescription.Text = PKD.cDescription;
                if (txtPatientType.Items.FindByValue(PKD.PatientTypeID.ToString()) != null)
                {
                    txtPatientType.SelectedValue = PKD.PatientTypeID.ToString();
                }
                if (txtCategory.Items.FindByValue(PKD.CategoryID.ToString()) != null)
                {
                    txtCategory.SelectedValue = PKD.CategoryID.ToString();
                }
                txtPackageAmt.Text = PKD.PackageAmt.ToString();
                txtSessionAmt.Text = PKD.OneTimeAmt.ToString();
                txtAppointments.Text = PKD.Appointments.ToString();
                if (PKD.ValidityDays > 0)
                    txtValidity.Text = PKD.ValidityDays.ToString();
                else
                    txtValidity.Text = "";
                if (txtTimeDuration.Items.FindByValue(PKD.MaximumTime.ToString()) != null)
                {
                    txtTimeDuration.SelectedValue = PKD.MaximumTime.ToString();
                }
                SnehBLL.SessionToPackage_Bll SPB = new SnehBLL.SessionToPackage_Bll();
                foreach (SnehDLL.SessionToPackage_Dll SPD in SPB.GetList(PKD.PackageID))
                {
                    for (int i = 0; i < txtToSession.Items.Count; i++)
                    {
                        if (txtToSession.Items[i].Value == SPD.SessionID.ToString())
                        {
                            txtToSession.Items[i].Selected = true;
                        }
                    }
                }
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find package detail, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "2";
                Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int _patientTypeID = 0; if (txtPatientType.SelectedItem != null) { int.TryParse(txtPatientType.SelectedItem.Value, out _patientTypeID); }
            int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
            float _packageAmt = 0; float.TryParse(txtPackageAmt.Text, out _packageAmt);
            float _oneTimeAmt = 0; float.TryParse(txtSessionAmt.Text, out _oneTimeAmt);
            int _appointments = 0; int.TryParse(txtAppointments.Text, out _appointments);
            int _validityDays = 0; int.TryParse(txtValidity.Text, out _validityDays);
            int _maximumTime = 0; if (txtTimeDuration.SelectedItem != null) { int.TryParse(txtTimeDuration.SelectedItem.Value, out _maximumTime); }

            SnehDLL.Packages_Dll PKD = new SnehDLL.Packages_Dll();
            PKD.PackageID = _packageID; PKD.UniqueID = "";
            PKD.PackageCode = txtPackageCode.Text.Trim();
            PKD.cDescription = txtcDescription.Text.Trim();
            PKD.PatientTypeID = _patientTypeID; PKD.CategoryID = _categoryID;
            PKD.PackageAmt = _packageAmt; PKD.OneTimeAmt = _oneTimeAmt;
            PKD.Appointments = _appointments; PKD.ValidityDays = _validityDays;
            PKD.MaximumTime = _maximumTime;

            if (txtPackageCode.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter package code.", 2); return;
            }
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            if (PKB.IsCodeAvailable(_packageID, txtPackageCode.Text.Trim()))
            {
                DbHelper.Configuration.setAlert(Page, "Entered package code is not available.", 2); return;
            }
            if (_packageAmt <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter package amount.", 2); return;
            }
            if (_oneTimeAmt <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter package session charge.", 2); return;
            }
            if (_appointments <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter package appointments.", 2); return;
            }
            bool _isSessionSelected = false;
            foreach (ListItem li in txtToSession.Items)
            {
                if (li.Selected)
                {
                    _isSessionSelected = true;
                    break;
                }
            }
            if (!_isSessionSelected)
            {
                DbHelper.Configuration.setAlert(Page, "Please select session to assign package.", 2); return;
            }

            int i = PKB.Set(PKD);
            if (i > 0)
            {
                SnehBLL.SessionToPackage_Bll SPB = new SnehBLL.SessionToPackage_Bll();
                SnehDLL.SessionToPackage_Dll SPD; SPB.Delete(i);
                foreach (ListItem li in txtToSession.Items)
                {
                    if (li.Selected)
                    {
                        int _sessionID = 0; int.TryParse(li.Value, out _sessionID);
                        SPD = new SnehDLL.SessionToPackage_Dll();
                        SPD.PackageID = i;
                        SPD.SessionID = _sessionID;
                        SPB.Set(SPD);
                    }
                }

                Session[DbHelper.Configuration.messageTextSession] = "Package detail saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process, Please try again.", 2);
            }
        }

        protected void btnRevise_Click(object sender, EventArgs e)
        {
            if (_packageID > 0)
            {
                int _patientTypeID = 0; if (txtPatientType.SelectedItem != null) { int.TryParse(txtPatientType.SelectedItem.Value, out _patientTypeID); }
                int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
                float _packageAmt = 0; float.TryParse(txtPackageAmt.Text, out _packageAmt);
                float _oneTimeAmt = 0; float.TryParse(txtSessionAmt.Text, out _oneTimeAmt);
                int _appointments = 0; int.TryParse(txtAppointments.Text, out _appointments);
                int _validityDays = 0; int.TryParse(txtValidity.Text, out _validityDays);
                int _maximumTime = 0; if (txtTimeDuration.SelectedItem != null) { int.TryParse(txtTimeDuration.SelectedItem.Value, out _maximumTime); }

                SnehDLL.Packages_Dll PKD = new SnehDLL.Packages_Dll();
                PKD.PackageID = _packageID; PKD.UniqueID = "";
                PKD.PackageCode = txtPackageCode.Text.Trim();
                PKD.cDescription = txtcDescription.Text.Trim();
                PKD.PatientTypeID = _patientTypeID; PKD.CategoryID = _categoryID;
                PKD.PackageAmt = _packageAmt; PKD.OneTimeAmt = _oneTimeAmt;
                PKD.Appointments = _appointments; PKD.ValidityDays = _validityDays;
                PKD.MaximumTime = _maximumTime;

                //if (txtPackageCode.Text.Trim().Length <= 0)
                //{
                //    DbHelper.Configuration.setAlert(Page, "Please enter package code.", 2); return;
                //}
                SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
                //if (PKB.IsCodeAvailable(_packageID, txtPackageCode.Text.Trim()))
                //{
                //    DbHelper.Configuration.setAlert(Page, "Entered package code is not available.", 2); return;
                //}
                if (_packageAmt <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter package amount.", 2); return;
                }
                if (_oneTimeAmt <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter package session charge.", 2); return;
                }
                if (_appointments <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter package appointments.", 2); return;
                }
                bool _isSessionSelected = false;
                foreach (ListItem li in txtToSession.Items)
                {
                    if (li.Selected)
                    {
                        _isSessionSelected = true;
                        break;
                    }
                }
                if (!_isSessionSelected)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select session to assign package.", 2); return;
                }

                int i = PKB.Revise(PKD);
                if (i > 0)
                {
                    SnehBLL.SessionToPackage_Bll SPB = new SnehBLL.SessionToPackage_Bll();
                    SnehDLL.SessionToPackage_Dll SPD; SPB.Delete(i);
                    foreach (ListItem li in txtToSession.Items)
                    {
                        if (li.Selected)
                        {
                            int _sessionID = 0; int.TryParse(li.Value, out _sessionID);
                            SPD = new SnehDLL.SessionToPackage_Dll();
                            SPD.PackageID = i;
                            SPD.SessionID = _sessionID;
                            SPB.Set(SPD);
                        }
                    }

                    Session[DbHelper.Configuration.messageTextSession] = "Package detail revised successfully.";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";

                    Response.Redirect(ResolveClientUrl("~/Member/Packages.aspx"), true);
                }
                else if (i == -1)
                {
                    DbHelper.Configuration.setAlert(Page, "Please check revised charges, Revised charges must be greater.", 2);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process, Please try again.", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process, Please try again.", 2);
            }
        }
    }
}