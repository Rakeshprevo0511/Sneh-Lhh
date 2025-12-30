using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class AdultView : System.Web.UI.Page
    {
        int _loginID = 0; int _patientID = 0; int _patientTypeID = 1; public string _headerText = "";

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
            if (_patientID > 0)
            {
                _headerText = "View Adult Registration";
                if (!IsPostBack)
                {
                    LoadForm();
                    LoadPatient();
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/Patients.aspx"), true);
            }
        }

        private void LoadPatient()
        {
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll(); SnehDLL.PatientMast_Dll PD = PB.Get(_patientID);
            if (PD != null)
            {
                if (txtSaluteName.Items.FindByValue(PD.PreFix) != null)
                {
                    txtSaluteName.SelectedValue = PD.PreFix;
                }
                txtFullName.Text = PD.FullName;
                if (PD.BirthDate > DateTime.MinValue)
                {
                    txtDob.Text = PD.BirthDate.ToString(DbHelper.Configuration.showDateFormat);
                    txtAge.Text = PD.AgeYear.ToString();
                    txtMonths.Text = PD.AgeMonth.ToString();
                }
                if (txtSaluteName.SelectedIndex == 1 || txtSaluteName.SelectedIndex == 3)
                {
                    txtSex.Text = "Male";
                }
                else
                {
                    txtSex.Text = "Female";
                }
                txtMobileNo.Text = PD.MobileNo;
                txtTelephoneNo.Text = PD.TelephoneNo;
                txtMail.Text = PD.MailID;
                txtResidental.Text = PD.rAddress;
                txtCorrespondence.Text = PD.cAddress;
                txtPatientCode.Text = PD.PatientCode;
                if (txtCategory.Items.FindByValue(PD.CategoryID.ToString()) != null)
                {
                    txtCategory.SelectedValue = PD.CategoryID.ToString();
                }
                if (txtCountry.Items.FindByValue(PD.CountryID.ToString()) != null)
                {
                    txtCountry.SelectedValue = PD.CountryID.ToString();
                }
                LoadStates();
                if (txtState.Items.FindByValue(PD.StateID.ToString()) != null)
                {
                    txtState.SelectedValue = PD.StateID.ToString();
                }
                LoadCities();
                if (txtCity.Items.FindByValue(PD.CityID.ToString()) != null)
                {
                    txtCity.SelectedValue = PD.CityID.ToString();
                }
                txtZipCode.Text = PD.ZipCode;
                if (PD.RegistrationDate > DateTime.MinValue)
                {
                    txtRegistrationDate.Text = PD.RegistrationDate.ToString(DbHelper.Configuration.showDateFormat);
                }
                txtRegistrationCode.Text = PD.RegistrationCode;
                if (txtReferred.Items.FindByValue(PD.ReferredBy.ToString()) != null)
                {
                    txtReferred.SelectedValue = PD.ReferredBy.ToString();
                }
                if (txtConsulted.Items.FindByValue(PD.ConsultedBy.ToString()) != null)
                {
                    txtConsulted.SelectedValue = PD.ConsultedBy.ToString();
                }
                txtVisitPurpose.Text = PD.VisitPurpose;
                txtMedicalHistory.Text = PD.MedicalHistory;
                if (txtAdultCase.Items.FindByValue(PD.AdultCaseID.ToString()) != null)
                {
                    txtAdultCase.SelectedValue = PD.AdultCaseID.ToString();
                }
                txtPreferredTime.Text = PD.PreferredTime;
                txtInvestigation.Text = PD.Investigation;
                txtMedicalAdvice.Text = PD.MedicalAdvice;
                txtChiefComplaints.Text = PD.ChiefComplaints;
                txtBriefHistory.Text = PD.BriefHistory;
                if (PD.ImagePath.Length > 0)
                {
                    imgpic.ImageUrl = "/Files/" + PD.ImagePath;
                }
                else
                {
                    imgpic.ImageUrl = "/Files/no_course_preview.png";
                }
                if (txtPaymentType.Items.FindByValue(PD.PaymentType.ToString()) != null)
                {
                    txtPaymentType.SelectedValue = PD.PaymentType.ToString();
                }
                txtPaymentType.Enabled = false;
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find Patient registration detail, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "2";
                Response.Redirect(ResolveClientUrl("~/Member/Adult.aspx"), true);
            }
        }

        private void LoadForm()
        {
            tb_Contents.ActiveTabIndex = 0; Cheqfields.Visible = false; OnlineField.Visible = false;
           
            SnehBLL.PatientCategory_Bll PCB = new SnehBLL.PatientCategory_Bll();
            txtCategory.Items.Clear();
            foreach (SnehDLL.PatientCategory_Dll PCD in PCB.GetList())
            {
                txtCategory.Items.Add(new ListItem(PCD.CategoryName, PCD.CategoryID.ToString()));
            }

            SnehBLL.Location_Bll LB = new SnehBLL.Location_Bll();
            txtCountry.Items.Clear(); txtCountry.Items.Add(new ListItem("Select Country", "-1"));
            foreach (SnehDLL.Location_Dll LD in LB.Get(0, SnehBLL.Location_Bll.LocationType.Country))
            {
                txtCountry.Items.Add(new ListItem(LD.name, LD.location_id.ToString()));
            }
            //SELECT COUNTRY INDIA 
            if (txtCountry.Items.FindByValue("100") != null) { txtCountry.SelectedValue = "100"; }

            LoadStates();

            LoadCities();

            txtReferred.Items.Clear(); txtReferred.Items.Add(new ListItem("Select Referred By", "-1"));
            txtConsulted.Items.Clear(); txtConsulted.Items.Add(new ListItem("Select Consulted By", "-1"));
            SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
            foreach (SnehDLL.DoctorMast_Dll DD in DB.GetForDropdown())
            {
                txtReferred.Items.Add(new ListItem(DD.PreFix + " " + DD.FullName, DD.DoctorID.ToString()));
                txtConsulted.Items.Add(new ListItem(DD.PreFix + " " + DD.FullName, DD.DoctorID.ToString()));
            }

            SnehBLL.AdultCases_Bll ACB = new SnehBLL.AdultCases_Bll();
            txtAdultCase.Items.Clear(); txtAdultCase.Items.Add(new ListItem("Select Adult Case", "-1"));
            foreach (SnehDLL.AdultCases_Dll ACD in ACB.GetList())
            {
                txtAdultCase.Items.Add(new ListItem(ACD.AdultCase, ACD.AdultCaseID.ToString()));
            }

            SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
            txtPaymentBankName.Items.Clear(); txtPaymentBankName.Items.Add(new ListItem("Select Bank", "-1"));
            foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
            {
                txtPaymentBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
            }
        }
          
        private void LoadStates()
        {
            int _countryID = 0; if (txtCountry.SelectedItem != null) { int.TryParse(txtCountry.SelectedItem.Value, out _countryID); }
            SnehBLL.Location_Bll LB = new SnehBLL.Location_Bll();
            txtState.Items.Clear(); txtState.Items.Add(new ListItem("Select State", "-1"));
            foreach (SnehDLL.Location_Dll LD in LB.Get(_countryID, SnehBLL.Location_Bll.LocationType.State))
            {
                txtState.Items.Add(new ListItem(LD.name, LD.location_id.ToString()));
            }
        }
         
        private void LoadCities()
        {
            int _stateID = 0; if (txtState.SelectedItem != null) { int.TryParse(txtState.SelectedItem.Value, out _stateID); }
            SnehBLL.Location_Bll LB = new SnehBLL.Location_Bll();
            txtCity.Items.Clear(); txtCity.Items.Add(new ListItem("Select City", "-1"));
            foreach (SnehDLL.Location_Dll LD in LB.Get(_stateID, SnehBLL.Location_Bll.LocationType.City))
            {
                txtCity.Items.Add(new ListItem(LD.name, LD.location_id.ToString()));
            }
        } 
    }
}