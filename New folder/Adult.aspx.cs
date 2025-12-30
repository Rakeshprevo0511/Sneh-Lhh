using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;

public partial class Member_Adult : System.Web.UI.Page
{
    int _loginID = 0; int _patientID = 0; int _patientTypeID = 1; public string _headerText = ""; int ReferredID = 0;

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
        if (Request.QueryString["referid"] != null)
        {
            int.TryParse(Request.QueryString["referid"].ToString(), out ReferredID);

        }
        if (ReferredID > 0)
        {
            reflist.Visible = true;
            pedlist.Visible = false;
        }
        else
        {
            reflist.Visible = false;
            pedlist.Visible = true;
        }
        if (_patientID > 0) { _headerText = "Edit Adult Registration"; } else { _headerText = "New Adult Registration"; }
        if (!IsPostBack)
        {
            LoadForm();
            if (_patientID > 0)
            {
                LoadPatient();
                string tab = string.Empty; if (Request.QueryString["tab"] != null) { tab = Request.QueryString["tab"].ToString(); }
                if (tab.Equals("upload", StringComparison.OrdinalIgnoreCase))
                {
                    tb_Contents.ActiveTabIndex = 4;
                    tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = true;
                }
            }
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
                txthAge.Value = PD.AgeYear.ToString();
                txthMonth.Value = PD.AgeMonth.ToString();
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
            txtmrno.Text = PD.MrNo;
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
            if ((PD.ReferredBy == 1 && PD.Ref_Selected > 0) || (PD.ReferredBy == 2 && PD.Ref_Selected > 0) || (PD.ReferredBy == 3 && PD.Ref_Selected > 0) || (PD.ReferredBy == 4 && PD.Ref_Selected > 0) || (PD.ReferredBy == 5 && PD.Ref_Selected > 0) || (PD.ReferredBy == 6 && PD.Ref_Selected > 0))
            {
                referselect.Visible = true;
                txtreferedbyid.Value = PD.ReferredBy.ToString();
                if (txtReferred.Items.FindByValue(PD.ReferredBy.ToString()) != null)
                {
                    txtReferred.SelectedValue = PD.ReferredBy.ToString();
                    txtrefername.Value = txtReferred.SelectedItem.Text;
                }
                LoadReferedBY(PD.ReferredBy);
                SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll(); DataTable dt = new DataTable();
                if (PD.ReferredBy == 1)
                {
                    dt = RB.Get_RefDR(PD.Ref_Selected);
                    int DoctorID = 0; int.TryParse(dt.Rows[0]["DoctorID"].ToString(), out DoctorID);
                    if (txtReferSelected.Items.FindByValue(DoctorID.ToString()) != null)
                    {
                        txtReferSelected.SelectedValue = DoctorID.ToString();
                    }
                }
                else if (PD.ReferredBy == 2)
                {
                    dt = RB.Get_RefSchool(PD.Ref_Selected);
                    if (dt.Rows.Count > 0)
                    {
                        int SchoolID = 0; int.TryParse(dt.Rows[0]["SchoolID"].ToString(), out SchoolID);
                        if (txtReferSelected.Items.FindByValue(SchoolID.ToString()) != null)
                        {
                            txtReferSelected.SelectedValue = SchoolID.ToString();
                        }
                    }
                }
                else if (PD.ReferredBy == 3)
                {
                    dt = RB.Get_RefHospital(PD.Ref_Selected);
                    if (dt.Rows.Count > 0)
                    {
                        int HospitalID = 0; int.TryParse(dt.Rows[0]["HospitalID"].ToString(), out HospitalID);
                        if (txtReferSelected.Items.FindByValue(HospitalID.ToString()) != null)
                        {
                            txtReferSelected.SelectedValue = HospitalID.ToString();
                        }
                    }
                }
                else if (PD.ReferredBy == 4)
                {
                    dt = RB.Get_RefTeacher(PD.Ref_Selected);
                    if (dt.Rows.Count > 0)
                    {
                        int TeacherID = 0; int.TryParse(dt.Rows[0]["TeacherID"].ToString(), out TeacherID);
                        if (txtReferSelected.Items.FindByValue(TeacherID.ToString()) != null)
                        {
                            txtReferSelected.SelectedValue = TeacherID.ToString();
                        }
                    }
                }
                else if (PD.ReferredBy == 5)
                {
                    dt = RB.Get_RefOther(PD.Ref_Selected);
                    if (dt.Rows.Count > 0)
                    {
                        int OtherID = 0; int.TryParse(dt.Rows[0]["OtherID"].ToString(), out OtherID);
                        if (txtReferSelected.Items.FindByValue(OtherID.ToString()) != null)
                        {
                            txtReferSelected.SelectedValue = OtherID.ToString();
                        }
                    }
                }
                else if (PD.ReferredBy == 6)
                {
                    dt = RB.Get_RefOnline(PD.Ref_Selected);
                    if (dt.Rows.Count > 0)
                    {
                        int OnlineID = 0; int.TryParse(dt.Rows[0]["OnlineID"].ToString(), out OnlineID);
                        if (txtReferSelected.Items.FindByValue(OnlineID.ToString()) != null)
                        {
                            txtReferSelected.SelectedValue = OnlineID.ToString();
                        }
                    }
                }


            }
            else
            {
                referselect.Visible = false;
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

    protected void txtreferred_SelectedIndexChanged(object sender, EventArgs e)
    {
        int referid = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out referid); }
        LoadReferedBY(referid);
    }

    private void LoadReferedBY(int ReferID)
    {
        txtReferSelected.Items.Clear(); txtReferSelected.Items.Add(new ListItem("Select Name", "-1"));
        SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll();
        if (ReferID > 0)
        {
            referselect.Visible = true;
            if (ReferID == 1)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_DrList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Prefix + " " + RD.Name, RD.DoctorID.ToString()));
                }

            }
            else if (ReferID == 2)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_SchoolList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Name, RD.SchoolID.ToString()));
                }
            }
            else if (ReferID == 3)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_HospitalList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Name, RD.HospitalID.ToString()));
                }
            }
            else if (ReferID == 4)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_TeacherList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Name, RD.TeacherID.ToString()));
                }
            }
            else if (ReferID == 5)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_OtherList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Name, RD.OtherID.ToString()));
                }
            }
            else if (ReferID == 6)
            {
                foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_OnlineList())
                {
                    txtReferSelected.Items.Add(new ListItem(RD.Name, RD.OnlineID.ToString()));
                }
            }
        }
        else
        {
            referselect.Visible = false;
        }
    }
    private void LoadForm()
    {
        tb_Contents.ActiveTabIndex = 0; Cheqfields.Visible = false; OnlineField.Visible = false;
        tb_Personal.Enabled = true; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;

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

        //txtReferred.Items.Clear(); txtReferred.Items.Add(new ListItem("Select Referred By", "-1"));
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

    protected void txtCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadStates(); LoadCities();
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

    protected void txtState_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCities();
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

    protected void btnNext1_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 0;
        tb_Personal.Enabled = true; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;
        tb_photo.Enabled = false;

        if (txtmrno.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter mrno.", 2); return;
        }

        string _prefix = ""; if (txtSaluteName.SelectedItem != null) { _prefix = txtSaluteName.SelectedItem.Value; }
        if (_prefix.Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select salute name of patient.", 2); return;
        }
        string _patientName = txtFullName.Text.Trim();
        if (_patientName.Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter full name of patient.", 2); return;
        }
        if (txtDob.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient birth date.", 2); return;
        }
        DateTime _birthDate = new DateTime(); DateTime.TryParseExact(txtDob.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _birthDate);
        if (_birthDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select correct birth date.", 2); return;
        }
        if (txtMobileNo.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient personal mobile no.", 2); return;
        }
        if (txtMobileNo.Text.Trim().Length != 10)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter correct mobile no.", 2); return;
        }
        //if (txtTelephoneNo.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter telephone number.", 2); return;
        //}
        //if (txtMail.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter email address.", 2); return;
        //}
        if (txtMail.Text.Trim().Length > 0 && !DbHelper.Configuration.isValidEmail(txtMail.Text.Trim()))
        {
            if (!txtMail.Text.Trim().Equals("-"))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter correct email address.", 2); return;
            }
        }
        tb_Contents.ActiveTabIndex = 1;
        tb_Personal.Enabled = false; tb_Address.Enabled = true; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;
    }

    protected void btnPrevious2_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 0;
        tb_Personal.Enabled = true; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;
    }

    protected void btnNext2_Click(object sender, EventArgs e)
    {
        if (txtResidental.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter residental address.", 2); return;
        }
        //if (txtCorrespondence.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter correspondence address.", 2); return;
        //}
        int _countryID = 0; if (txtCountry.SelectedItem != null) { int.TryParse(txtCountry.SelectedItem.Value, out _countryID); }
        if (_countryID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select country name.", 2); return;
        }
        int _stateID = 0; if (txtState.SelectedItem != null) { int.TryParse(txtState.SelectedItem.Value, out _stateID); }
        if (_stateID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select state name.", 2); return;
        }
        int _cityID = 0; if (txtCity.SelectedItem != null) { int.TryParse(txtCity.SelectedItem.Value, out _cityID); }
        if (_cityID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select city name.", 2); return;
        }
        if (txtZipCode.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter zip code.", 2); return;
        } 
        tb_Contents.ActiveTabIndex = 2; 
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = true; tb_Medical.Enabled = false; tb_photo.Enabled = false;
    }

    protected void btnPrevious3_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 1;
        tb_Personal.Enabled = false; tb_Address.Enabled = true; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;
    }

    protected void btnNext3_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 2;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = true; tb_Medical.Enabled = false; tb_photo.Enabled = false;

        if (txtRegistrationDate.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient registration date.", 2); return;
        }
        DateTime _registrationDate = new DateTime(); DateTime.TryParseExact(txtRegistrationDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _registrationDate);
        if (_registrationDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select correct registration date.", 2); return;
        }
        int _referredBy = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out _referredBy); }
        if (_referredBy <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select referred by.", 2); return;
        }
        int _consultedBy = 0; if (txtConsulted.SelectedItem != null) { int.TryParse(txtConsulted.SelectedItem.Value, out _consultedBy); }
        if (_consultedBy <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select consulted by.", 2); return;
        }
        //if (txtVisitPurpose.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter purpose of visit.", 2); return;
        //}
        //if (txtMedicalHistory.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter medical history.", 2); return;
        //}
        tb_Contents.ActiveTabIndex = 3;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = true; tb_photo.Enabled = false;
    }

    protected void btnPrevious4_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 3;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = true; tb_Medical.Enabled = false; tb_photo.Enabled = false;
    }

    protected void txtPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    { 
        int _paymentMode = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentMode); }
        Cheqfields.Visible = false; OnlineField.Visible = false;
        if (txtPaymentBankName.Items.Count > 0) { txtPaymentBankName.SelectedIndex = 0; }
        txtBankBranch.Text = ""; txtCheqNo.Text = ""; txtChequeDate.Text = "";
        txtTransactionID.Text = ""; txtTransactionDate.Text = "";
        if (_paymentMode == 3)
        {
            Cheqfields.Visible = true;
        }
        if (_paymentMode == 4)
        {
            OnlineField.Visible = true;
        }
    }

    protected void btnNext4_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 3;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = true; tb_photo.Enabled = false;

        int _adultCaseID = 0; if (txtAdultCase.SelectedItem != null) { int.TryParse(txtAdultCase.SelectedItem.Value, out _adultCaseID); }
        if (_adultCaseID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select adult case type.", 2); return;
        }
        //if (txtInvestigation.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter investigation done.", 2); return;
        //}
        //if (txtMedicalAdvice.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter medical advice or treatment.", 2); return;
        //}
        //if (txtChiefComplaints.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter chief complaints.", 2); return;
        //}
        //if (txtBriefHistory.Text.Trim().Length <= 0)
        //{
        //    DbHelper.Configuration.setAlert(Page, "Please enter brief history.", 2); return;
        //}
        tb_Contents.ActiveTabIndex = 4;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = true;
    }

    protected void btnPrevious5_OnClick(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 4;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = true; tb_photo.Enabled = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region ################# TAB 0 VALIDATION #################

        tb_Contents.ActiveTabIndex = 0;
        tb_Personal.Enabled = true; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;

        if (txtmrno.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter mrno.", 2); return;
        }
        string _prefix = ""; if (txtSaluteName.SelectedItem != null) { _prefix = txtSaluteName.SelectedItem.Value; }
        if (_prefix.Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select salute name of patient.", 2); return;
        }
        int _gender = 0;
        if (txtSaluteName.SelectedIndex == 1 || txtSaluteName.SelectedIndex == 3)
        {
            _gender = 1;
        }
        string _patientName = txtFullName.Text.Trim();
        if (_patientName.Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter full name of patient.", 2); return;
        }
        if (txtDob.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient birth date.", 2); return;
        }
        DateTime _birthDate = new DateTime(); DateTime.TryParseExact(txtDob.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _birthDate);
        if (_birthDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select correct birth date.", 2); return;
        }
        int _year = 0; int.TryParse(txthAge.Value.Trim(), out _year);
        int _month = 0; int.TryParse(txthMonth.Value.Trim(), out _month);

        if (txtMobileNo.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient personal mobile no.", 2); return;
        }
        if (txtMobileNo.Text.Trim().Length != 10)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter correct mobile no.", 2); return;
        }
        #endregion

        #region ################# TAB 1 VALIDATION #################

        tb_Contents.ActiveTabIndex = 1;
        tb_Personal.Enabled = false; tb_Address.Enabled = true; tb_Registration.Enabled = false; tb_Medical.Enabled = false; tb_photo.Enabled = false;

        int _countryID = 0; if (txtCountry.SelectedItem != null) { int.TryParse(txtCountry.SelectedItem.Value, out _countryID); }
        int _stateID = 0; if (txtState.SelectedItem != null) { int.TryParse(txtState.SelectedItem.Value, out _stateID); }
        int _cityID = 0; if (txtCity.SelectedItem != null) { int.TryParse(txtCity.SelectedItem.Value, out _cityID); }

        #endregion

        #region #################  TAB 2 VALIDATION #################

        tb_Contents.ActiveTabIndex = 2;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = true; tb_Medical.Enabled = false; tb_photo.Enabled = false;

        if (txtRegistrationDate.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter patient registration date.", 2); return;
        }
        DateTime _registrationDate = new DateTime(); DateTime.TryParseExact(txtRegistrationDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _registrationDate);
        if (_registrationDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select correct registration date.", 2); return;
        }

        #endregion

        #region ################# TAB 3 VALIDATION #################

        tb_Contents.ActiveTabIndex = 3;
        tb_Personal.Enabled = false; tb_Address.Enabled = false; tb_Registration.Enabled = false; tb_Medical.Enabled = true; tb_photo.Enabled = false;

        int _referredBy = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out _referredBy); }
        int referselectedid = 0;
        if (referselect.Visible == true)
        {
            if (txtReferSelected.SelectedItem != null) { int.TryParse(txtReferSelected.SelectedItem.Value, out referselectedid); }
        }
        int _consultedBy = 0; if (txtConsulted.SelectedItem != null) { int.TryParse(txtConsulted.SelectedItem.Value, out _consultedBy); }
        int _adultCaseID = 0; if (txtAdultCase.SelectedItem != null) { int.TryParse(txtAdultCase.SelectedItem.Value, out _adultCaseID); }

        #endregion

        SnehDLL.PatientMast_Dll PD = new SnehDLL.PatientMast_Dll();
        PD.MrNo = txtmrno.Text.Trim();
        PD.PatientID = _patientID; PD.UniqueID = ""; PD.PatientTypeID = _patientTypeID;
        PD.PreFix = _prefix; PD.FullName = DbHelper.StringHelper.ToTitleCase(txtFullName.Text.Trim(), DbHelper.TitleCase.All);
        PD.BirthDate = _birthDate; PD.AgeYear = _year; PD.AgeMonth = _month;
        PD.Gender = _gender; PD.MobileNo = txtMobileNo.Text.Trim();
        PD.MailID = txtMail.Text.Trim();

        PD.HasSchool = false; PD.SchoolingYear = ""; PD.SchoolName = ""; PD.SchoolRemark = "";

        PD.rAddress = txtResidental.Text.Trim(); PD.cAddress = txtCorrespondence.Text.Trim();
        int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
        PD.CategoryID = _categoryID;
        PD.CountryID = _countryID; PD.StateID = _stateID; PD.CityID = _cityID;
        PD.ZipCode = txtZipCode.Text.Trim();

        PD.TelephoneNo = txtTelephoneNo.Text.Trim();

        PD.FatherName = ""; PD.FatherOccupation = ""; PD.FatherMailID = ""; PD.FatherMobileNo = "";
        PD.MotherName = ""; PD.MotherOccupation = ""; PD.MotherMailID = ""; PD.MotherMobileNo = "";

        PD.RegistrationCode = txtRegistrationCode.Text.Trim(); PD.RegistrationDate = _registrationDate;

        PD.ReferredBy = _referredBy; PD.ConsultedBy = _consultedBy; PD.Ref_Selected = referselectedid;
        PD.VisitPurpose = txtVisitPurpose.Text.Trim(); PD.ChiefComplaints = txtChiefComplaints.Text.Trim();
        PD.MedicalHistory = txtMedicalHistory.Text.Trim(); PD.BriefHistory = txtBriefHistory.Text.Trim();
        PD.PreferredTime = txtPreferredTime.Text.Trim(); PD.AdultCaseID = _adultCaseID;
        PD.Investigation = txtInvestigation.Text.Trim(); PD.MedicalAdvice = txtMedicalAdvice.Text.Trim();

        PD.IsEnabled = true;
        PD.AddedDate = DateTime.UtcNow.AddMinutes(330); PD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        PD.AddedBy = _loginID; PD.ModifyBy = _loginID;
        
        int _bankID = 0; string BankBranch = string.Empty; string ChequeTxnNo = string.Empty; DateTime _chequeDate = new DateTime();
        int _paymentType = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentType); }
        PD.PaymentType = _paymentType;
        if (_paymentType == 3)
        {
            Cheqfields.Visible = true;
            if (txtPaymentBankName.SelectedItem != null) { int.TryParse(txtPaymentBankName.SelectedItem.Value, out _bankID); }
            BankBranch = txtBankBranch.Text.Trim(); ChequeTxnNo = txtCheqNo.Text.Trim();
            DateTime.TryParseExact(txtChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (_bankID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select bank name...", 2); return;
            }
            if (string.IsNullOrEmpty(ChequeTxnNo))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter cheque number...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
            }
        }
        if (_paymentType == 4)
        {
            OnlineField.Visible = true;
            ChequeTxnNo = txtTransactionID.Text.Trim();
            DateTime.TryParseExact(txtTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (string.IsNullOrEmpty(ChequeTxnNo))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter transaction id...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            }
        }
        SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();

        PD.ImagePath = "";

        if (FileUpload1.HasFile)
        {
            string _fileName = "f_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.'));

            if (PB.SaveFile(ref FileUpload1, _fileName))
            {
                imgpic.ImageUrl = _fileName;
                PD.ImagePath = _fileName;
            }
        }  
        
        int i = PB.Set(PD);

        if (i > 0)
        {
            if (_patientID <= 0) //ADD REGISTRATION PAYMENT
            {
                SnehBLL.PatientLedger_Bll PLB = new SnehBLL.PatientLedger_Bll();
                PLB.NewRegistration(i, _paymentType, _bankID, BankBranch, ChequeTxnNo, _chequeDate, txtNarration.Text.Trim(), _patientTypeID);
            }
             
            Session[DbHelper.Configuration.messageTextSession] = "Patient registration detail saved successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/Adult.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
        }
    }

    protected void btnSaveRefer_Click(object sender, EventArgs e)
    {
        string name = string.Empty; string contact = string.Empty; string email = string.Empty; string msg = string.Empty;
        string website = string.Empty; string address = string.Empty; string referedbymsg = string.Empty; int referid = 0;
        name = txtmodname.Text;
        contact = txtmodcontact.Text;
        email = txtmodemailid.Text;
        website = txtmodwebsite.Text;
        address = txtmodaddress.Text;
        DateTime addeddate = new DateTime(); DateTime.TryParseExact(txtmodAddedDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out addeddate);
        if (txtmodname.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please enter name.", 2); return;
        }
        if (txtmodcontact.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please enter contact no.", 2); return;
        }
        if (txtmodemailid.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please enter emailid.", 2); return;
        }
        if (txtmodwebsite.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please enter website name.", 2); return;
        }
        if (txtmodaddress.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please enter address.", 2); return;
        }
        if (addeddate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(msgmodal, "Please select date.", 2); return;
        }
        if (txtReferred.SelectedItem != null)
        {
            txtreferedbyid.Value = txtReferred.SelectedItem.Value; int.TryParse(txtreferedbyid.Value.ToString(), out referid);
            txtrefername.Value = txtReferred.SelectedItem.Text;
        }

        SnehDLL.Reference_Dll RD = new SnehDLL.Reference_Dll();
        SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll(); int i = 0; string refername = string.Empty;
        if (referid == 1)
        {
            RD.DoctorID = 0;
            RD.Prefix = "DR";
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceDr(RD);
        }
        else if (referid == 2)
        {
            RD.SchoolID = 0;
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceSchool(RD);
        }
        else if (referid == 3)
        {
            RD.HospitalID = 0;
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceHospital(RD);
        }
        else if (referid == 4)
        {
            RD.TeacherID = 0;
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceTeacher(RD);
        }
        else if (referid == 5)
        {
            RD.OtherID = 0;
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceOther(RD);
        }
        else if (referid == 6)
        {
            RD.OnlineID = 0;
            RD.Name = name;
            RD.MobileNo = contact;
            RD.EmailID = email;
            RD.Website = website;
            RD.Address = address;
            RD.AddedDate = addeddate;
            i = RB.Set_RefernceOnlie(RD);
        }
        if (i > 0)
        {
            refername = txtrefername.Value;
            LoadReferedBY(referid);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "close_modal();", true);
            DbHelper.Configuration.setAlert(newmsg, refername + " added successfully..", 1); return;
        }
    }
}
