using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class EnquiryReg : System.Web.UI.Page
    {
        int _loginID = 0; int _patientID = 0; int _patientTypeID = 3; public string _headerText = "";

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
                    _patientID = SnehBLL.PatientMast_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_patientID > 0) { _headerText = "Edit Enquiry Registration"; } else { _headerText = "New Enquiry Registration"; }
            if (!IsPostBack)
            {
                if (_patientID > 0)
                {
                    LoadPatient();
                }
            }
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }
        }

        private void LoadPatient()
        {
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll(); SnehDLL.PatientMast_Dll PD = PB.Get(_patientID);
            if (PD != null && PD.PatientTypeID == _patientTypeID)
            {
                txtPatientUnique.Value = PD.UniqueID;
                if (txtSaluteName.Items.FindByValue(PD.PreFix) != null)
                {
                    txtSaluteName.SelectedValue = PD.PreFix;
                }
                txtFullName.Text = PD.FullName;
                if (txtSaluteName.SelectedIndex == 1 || txtSaluteName.SelectedIndex == 3)
                {
                    txtSex.Text = "Male";
                }
                else
                {
                    txtSex.Text = "Female";
                }
                txtTelephoneNo.Text = PD.TelephoneNo;
                txtPatientCode.Text = PD.PatientCode;
                txtFatherMobileNo.Text = PD.FatherMobileNo;
                txtMotherMobileNo.Text = PD.MotherMobileNo;
                btnComplete.Visible = true;
                btnComplete.OnClientClick = GetCompleteClick();
                string[] DiagnosisIDs = PD.DiagnosisID.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                {
                    for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                    {
                        if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                        {
                            txtDiagnosis.Items[j].Selected = true; break;
                        }
                    }
                }
                txtDiagnosisOther.Text = PD.DiagnosisOther;
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find enquiry registration detail, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "2";
                Response.Redirect(ResolveClientUrl("~/Member/EnquiryReg.aspx"), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _gender = 0;
            if (txtSaluteName.SelectedIndex == 1 || txtSaluteName.SelectedIndex == 3)
            {
                _gender = 1;
            }

            SnehDLL.PatientMast_Dll PD = new SnehDLL.PatientMast_Dll();
            PD.PatientID = _patientID; PD.UniqueID = ""; PD.PatientTypeID = _patientTypeID;
            PD.PreFix = ""; PD.FullName = DbHelper.StringHelper.ToTitleCase(txtFullName.Text.Trim(), DbHelper.TitleCase.All);
            PD.BirthDate = new DateTime(); PD.AgeYear = 0; PD.AgeMonth = 0;
            PD.Gender = _gender; PD.MobileNo = ""; PD.MailID = "";

            PD.HasSchool = false; PD.SchoolingYear = ""; PD.SchoolName = ""; PD.SchoolRemark = "";

            PD.rAddress = ""; PD.cAddress = "";
            int _categoryID = 0;
            PD.CategoryID = _categoryID;
            PD.CountryID = 0; PD.StateID = 0; PD.CityID = 0;
            PD.ZipCode = "";

            PD.TelephoneNo = txtTelephoneNo.Text.Trim();

            PD.FatherName = ""; PD.FatherOccupation = ""; PD.FatherMailID = ""; PD.FatherMobileNo = txtFatherMobileNo.Text.Trim();
            PD.MotherName = ""; PD.MotherOccupation = ""; PD.MotherMailID = ""; PD.MotherMobileNo = txtMotherMobileNo.Text.Trim();

            PD.RegistrationCode = ""; PD.RegistrationDate = new DateTime();

            PD.ReferredBy = -1; PD.ConsultedBy = -1;
            PD.VisitPurpose = ""; PD.ChiefComplaints = "";
            PD.MedicalHistory = ""; PD.BriefHistory = "";
            PD.PreferredTime = ""; PD.AdultCaseID = -1;
            PD.Investigation = ""; PD.MedicalAdvice = "";

            PD.IsEnabled = true;
            PD.AddedDate = DateTime.UtcNow.AddMinutes(330); PD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            PD.AddedBy = _loginID; PD.ModifyBy = _loginID;

            string BankBranch = string.Empty; string ChequeTxnNo = string.Empty;
            int _paymentType = 0; PD.PaymentType = _paymentType;


            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();

            PD.ImagePath = "";
            string DiagnosisIDs = "";
            for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            {
                if (txtDiagnosis.Items[k].Selected)
                {
                    DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
                }
            }
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, _patientID);
            if (g < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            }
            PD.DiagnosisOther = DiagnosisOther;
            PD.DiagnosisID = DiagnosisIDs;
            PD.MrNo = "0";

            int i = PB.Set(PD);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Patient registration detail saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/EnquiryReg.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        public string GetCompleteClick()
        {
            if (_patientID > 0)
            {
                string apt = string.Empty;
                if (Request.QueryString["apt"] != null)
                {
                    apt = Request.QueryString["apt"].ToString();
                }
                return "fwdToRegistration('" + Request.QueryString["record"].ToString() + "', '" + apt + "');return false;";
            }
            return "return false;";
        }
    }
}