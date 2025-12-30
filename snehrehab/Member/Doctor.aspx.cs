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
using System.Collections.Generic;

public partial class Member_Doctor : System.Web.UI.Page
{
    int _loginID = 0; int _doctorID = 0; public string _headerText = "";

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
                _doctorID = SnehBLL.DoctorMast_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (_doctorID > 0) { _headerText = "Edit Doctor Detail"; } else { _headerText = "Add New Doctor"; }

        if (!IsPostBack)
        {
            LoadForm();
            if (_doctorID > 0)
            {
                LoadDoctor();
            }
            else
            {
                upload.Visible = true;
                Image1.ImageUrl = "../images/dh-users.png";
            }
        }
    }

    private void LoadDoctor()
    {
        SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
        SnehDLL.DoctorMast_Dll DD = DB.Get(_doctorID);
        if (DD != null)
        {
            txtFullName.Text = DD.FullName;
            if (txtSpeciality.Items.FindByValue(DD.SpecialityID.ToString()) != null)
            {
                txtSpeciality.SelectedValue = DD.SpecialityID.ToString();
            }
            if (txtWorkplace.Items.FindByValue(DD.WorkplaceID.ToString()) != null)
            {
                txtWorkplace.SelectedValue = DD.WorkplaceID.ToString();
            }
            txtOtherWorkplace.Text = DD.WorkplaceOther;
            if (DD.WorkplaceID == 0)
            {
                txtOtherWorkplace.Enabled = true;
            }
            txtMailID.Text = DD.MailID;
            txtMobile.Text = DD.MobileNo;
            txtTelephone1.Text = DD.Telephone1;
            txtTelephone2.Text = DD.Telephone2;
            txtPanCard.Text = DD.PanCard;
            if (DD.BirthDate > DateTime.MinValue)
            {
                txtBirthDate.Text = DD.BirthDate.ToString(DbHelper.Configuration.showDateFormat);
            }
            if (DD.JoinDate > DateTime.MinValue)
            {
                txtJoinDate.Text = DD.JoinDate.ToString(DbHelper.Configuration.showDateFormat);
            }
            txtAddress.Text = DD.cAddress;
            if (txtCountry.Items.FindByValue(DD.CountryID.ToString()) != null)
            {
                txtCountry.SelectedValue = DD.CountryID.ToString();
            }
            LoadStates();
            if (txtState.Items.FindByValue(DD.StateID.ToString()) != null)
            {
                txtState.SelectedValue = DD.StateID.ToString();
            }
            LoadCities();
            if (txtCity.Items.FindByValue(DD.CityID.ToString()) != null)
            {
                txtCity.SelectedValue = DD.CityID.ToString();
            }
            txtZipCode.Text = DD.ZipCode;
            txtClinicAddress.Text = DD.ClinicAddress;
            txtClinicTel1.Text = DD.ClinicTel1;
            txtClinicTel2.Text = DD.ClinicTel2;
            if (DD.Available1FromChar.Length > 0)
            {
                txtAvailability1From.Text = new DateTime(DD.Available1From.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            if (DD.Available1UptoChar.Length > 0)
            {
                txtAvailability1Upto.Text = new DateTime(DD.Available1Upto.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            if (DD.Available2FromChar.Length > 0)
            {
                txtAvailability2From.Text = new DateTime(DD.Available2From.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            if (DD.Available2UptoChar.Length > 0)
            {
                txtAvailability2Upto.Text = new DateTime(DD.Available2Upto.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            if (DD.FacilitatorID > 0) { txtFacilitator.Checked = true; } else { txtFacilitator.Checked = false; }
            txtRemarks.Text = DD.Remarks;

            SnehBLL.DoctorWeekOff_Bll DWB = new SnehBLL.DoctorWeekOff_Bll();
            foreach (SnehDLL.DoctorWeekOff_Dll DWD in DWB.GetList(_doctorID))
            {
                for (int i = 0; i < txtWeekDayOff.Items.Count; i++)
                {
                    if (txtWeekDayOff.Items[i].Value == DWD.DayID.ToString())
                    {
                        txtWeekDayOff.Items[i].Selected = true;
                    }
                }
            }
            if (DD.Profile_Image_Path.Length > 0)
            {
                Image1.ImageUrl = "/Files/" + DD.Profile_Image_Path;
            }
            else
            {
                Image1.ImageUrl = "../images/dh-users.png";
            }

            SnehBLL.Degree_Bll RB = new SnehBLL.Degree_Bll();
            DataTable dt = RB.Get_CerData(3, 0, 0, _doctorID);
            if (dt.Rows.Count > 0)
            {
                upload.Visible = false;
            }
            else
            {
                upload.Visible = true;
            }

            txtfather.Text = DD.FatherName;
            txtmother.Text = DD.MotherName;
            txthusbandwife.Text = DD.Husband_WifeName;
            if (txtQualification.Items.FindByValue(DD.QualificationID.ToString()) != null)
            {
                txtQualification.SelectedValue = DD.QualificationID.ToString();
            }
            txtbloodgroup.Text = DD.BloodGroup;
            txtaadharcard.Text = DD.Aadharcard;
            if (DD.Anniversary_Date > DateTime.MinValue)
            {
                txtanniversary.Text = DD.Anniversary_Date.ToString(DbHelper.Configuration.showDateFormat);
            }
        }
        else
        {
            Session[DbHelper.Configuration.messageTextSession] = "Unable to find doctor detail, Please try again.";
            Session[DbHelper.Configuration.messageTypeSession] = "2";
            Response.Redirect(ResolveClientUrl("~/Member/Doctor.aspx"), true);
        }
    }

    private void LoadForm()
    {
        SnehBLL.Specialities_Bll SB = new SnehBLL.Specialities_Bll();
        txtSpeciality.Items.Clear(); txtSpeciality.Items.Add(new ListItem("Select Speciality", "-1"));
        foreach (SnehDLL.Specialities_Dll SD in SB.GetList())
        {
            txtSpeciality.Items.Add(new ListItem(SD.Speciality, SD.SpecialityID.ToString()));
        }
        SnehBLL.WorkPlaces_Bll WB = new SnehBLL.WorkPlaces_Bll();
        txtWorkplace.Items.Clear(); txtWorkplace.Items.Add(new ListItem("Select Work Place", "-1"));

        List<SnehDLL.WorkPlaces_Dll> WDL = WB.GetList();
        SnehDLL.WorkPlaces_Dll OWD = new SnehDLL.WorkPlaces_Dll();
        //OWD.WorkplaceID = 0; OWD.Workplace = "Other"; WDL.Add(OWD);
        foreach (SnehDLL.WorkPlaces_Dll WD in WDL.OrderBy(w => w.Workplace))
        {
            txtWorkplace.Items.Add(new ListItem(WD.Workplace, WD.WorkplaceID.ToString()));
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int i = SaveData();
        if (i > 0)
        {
            Response.Redirect(ResolveClientUrl("~/Member/Doctor.aspx?record=" + SnehBLL.DoctorMast_Bll.Check(i) + ""), true);
        }
    }

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        int i = SaveData();
        if (i > 0)
        {
            Response.Redirect(ResolveClientUrl("~/Member/Doctor.aspx"), true);
        }
    }

    private int SaveData()
    {
        int i = 0;

        TimeSpan _available1From = new TimeSpan();
        DateTime _available1FromD = new DateTime(); DateTime.TryParseExact(txtAvailability1From.Text, DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _available1FromD);
        if (_available1FromD > DateTime.MinValue && _available1FromD < DateTime.MaxValue)
        {
            _available1From = _available1FromD.TimeOfDay;
        }
        TimeSpan _available1Upto = new TimeSpan();
        DateTime _available1UptoD = new DateTime(); DateTime.TryParseExact(txtAvailability1Upto.Text, DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _available1UptoD);
        if (_available1UptoD > DateTime.MinValue && _available1UptoD < DateTime.MaxValue)
        {
            _available1Upto = _available1UptoD.TimeOfDay;
        }
        TimeSpan _available2From = new TimeSpan();
        DateTime _available2FromD = new DateTime(); DateTime.TryParseExact(txtAvailability2From.Text, DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _available2FromD);
        if (_available2FromD > DateTime.MinValue && _available2FromD < DateTime.MaxValue)
        {
            _available2From = _available2FromD.TimeOfDay;
        }
        TimeSpan _available2Upto = new TimeSpan();
        DateTime _available2UptoD = new DateTime(); DateTime.TryParseExact(txtAvailability2Upto.Text, DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _available2UptoD);
        if (_available2UptoD > DateTime.MinValue && _available2UptoD < DateTime.MaxValue)
        {
            _available2Upto = _available2UptoD.TimeOfDay;
        }
        int _specialityID = 0; if (txtSpeciality.SelectedItem != null) { int.TryParse(txtSpeciality.SelectedItem.Value, out _specialityID); }
        int _workplaceID = 0; if (txtWorkplace.SelectedItem != null) { int.TryParse(txtWorkplace.SelectedItem.Value, out _workplaceID); }
        DateTime _birthDate = new DateTime(); DateTime.TryParseExact(txtBirthDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _birthDate);
        DateTime _joinDate = new DateTime(); DateTime.TryParseExact(txtJoinDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _joinDate);
        int _countryID = 0; if (txtCountry.SelectedItem != null) { int.TryParse(txtCountry.SelectedItem.Value, out _countryID); }
        int _stateID = 0; if (txtState.SelectedItem != null) { int.TryParse(txtState.SelectedItem.Value, out _stateID); }
        int _cityID = 0; if (txtCity.SelectedItem != null) { int.TryParse(txtCity.SelectedItem.Value, out _cityID); }
        int _facilitatorID = 0; if (txtFacilitator.Checked) { _facilitatorID = 1; }
        string _workPlace = ""; if (_workplaceID == 0) { _workPlace = txtOtherWorkplace.Text.Trim(); }
        string qualification = string.Empty; int QualificationID = 0;
        DateTime anniversarydate = new DateTime(); DateTime.TryParseExact(txtanniversary.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out anniversarydate);
        if (txtFullName.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter doctor name.", 2); return 0;
        }
        if (txtQualification.SelectedItem != null)
        {
            int.TryParse(txtQualification.SelectedItem.Value, out QualificationID);
            qualification = txtQualification.SelectedItem.Text;
        }
        if (QualificationID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select qualification.", 2); return 0;
        }
        if (_specialityID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select speciality.", 2); return 0;
        }
        if (_workplaceID < 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select workplace.", 2); return 0;
        }
        if (txtMobile.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter mobile no.", 2); return 0;
        }
        if (txtMobile.Text.Trim().Length < 10)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter correct mobile no.", 2); return 0;
        }
        if (txtTelephone1.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter emergency contact no.", 2); return 0;
        }
        if (txtAddress.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter residance address.", 2); return 0;
        }
        if (txtPanCard.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter pancard no.", 2); return 0;
        }
        if (txtaadharcard.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter aadharcard no.", 2); return 0;
        }
        if (_countryID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select country name.", 2); return 0;
        }
        if (_stateID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select state name.", 2); return 0;
        }
        if (_cityID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select city name.", 2); return 0;
        }
        if (txtZipCode.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter zip code.", 2); return 0;
        }
        SnehDLL.DoctorMast_Dll DD = new SnehDLL.DoctorMast_Dll();
        DD.DoctorID = _doctorID;
        DD.UniqueID = "";
        DD.PreFix = txtSalute.Text.Trim();
        DD.FullName = DbHelper.StringHelper.ToTitleCase(txtFullName.Text.Trim().ToUpper(), DbHelper.TitleCase.All);
        DD.Qualification = qualification;
        DD.SpecialityID = _specialityID;
        DD.WorkplaceID = _workplaceID;
        DD.WorkplaceOther = _workPlace;
        DD.cAddress = txtAddress.Text.Trim();
        DD.CountryID = _countryID;
        DD.StateID = _stateID;
        DD.CityID = _cityID;
        DD.ZipCode = txtZipCode.Text.Trim();
        DD.Telephone1 = txtTelephone1.Text.Trim();
        DD.Telephone2 = txtTelephone2.Text.Trim();
        DD.MailID = txtMailID.Text.Trim();
        DD.MobileNo = txtMobile.Text.Trim();
        DD.PanCard = txtPanCard.Text.Trim();
        DD.Remarks = txtRemarks.Text.Trim();
        DD.BirthDate = _birthDate;
        DD.ClinicAddress = txtClinicAddress.Text.Trim();
        DD.ClinicTel1 = txtClinicTel1.Text.Trim();
        DD.ClinicTel2 = txtClinicTel2.Text.Trim();
        DD.FacilitatorID = _facilitatorID;
        DD.JoinDate = _joinDate;
        DD.IsEnabled = true;
        DD.Available1From = _available1From;
        DD.Available1Upto = _available1Upto;
        DD.Available2From = _available2From;
        DD.Available2Upto = _available2Upto;
        DD.Available1FromChar = txtAvailability1From.Text.Trim();
        DD.Available1UptoChar = txtAvailability1Upto.Text.Trim();
        DD.Available2FromChar = txtAvailability2From.Text.Trim();
        DD.Available2UptoChar = txtAvailability2Upto.Text.Trim();
        DD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        DD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        DD.AddedBy = _loginID;
        DD.ModifyBy = _loginID; SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
        DD.Profile_Image_Path = "";
        if (txtprofilephoto.FileName.Length > 0)
        {
            decimal size = Math.Round(((decimal)txtprofilephoto.PostedFile.ContentLength / (decimal)1024), 2);
            if (size > 150)
            {
                DbHelper.Configuration.setAlert(Page, "File size must not exceed 150 KB.", 2); return 0;
            }
            string filename = "p_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + txtprofilephoto.FileName.Substring(txtprofilephoto.FileName.LastIndexOf('.'));
            if (RB.SaveFile(ref txtprofilephoto, filename))
            {
                DD.Profile_Image_Path = filename;
            }
        }

        DD.FatherName = txtfather.Text.Trim();
        DD.MotherName = txtmother.Text.Trim();
        DD.Husband_WifeName = txthusbandwife.Text.Trim();
        DD.QualificationID = QualificationID;
        DD.BloodGroup = txtbloodgroup.Text.Trim();
        DD.Aadharcard = txtaadharcard.Text.Trim();
        DD.Anniversary_Date = anniversarydate;

        SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
        i = DB.Set(DD);
        if (i > 0)
        {
            SnehBLL.DoctorWeekOff_Bll DWB = new SnehBLL.DoctorWeekOff_Bll();
            SnehDLL.DoctorWeekOff_Dll DWD; DWB.Delete(i);
            foreach (ListItem li in txtWeekDayOff.Items)
            {
                if (li.Selected)
                {
                    int _dayID = 0; int.TryParse(li.Value, out _dayID);
                    DWD = new SnehDLL.DoctorWeekOff_Dll();
                    DWD.DoctorID = i;
                    DWD.DayID = _dayID;
                    DWB.Set(DWD);
                }
            }
            if (upload.Visible == true)
            {
                if (txtdegcer.HasFile == true)
                {
                    SnehBLL.Degree_Bll DBN = new SnehBLL.Degree_Bll();
                    SnehDLL.Degree_Dll DDN = new SnehDLL.Degree_Dll();
                    int z = 0; int j = 0;
                    foreach (string fileName in Request.Files)
                    {
                        string FilePathQuestion = string.Empty; string fileNameQue = string.Empty;
                        HttpPostedFile file = Request.Files[z];
                        if (fileName == "ctl00$ContentPlaceHolder1$txtdegcer")
                        {
                            string FilePathCer = string.Empty; string FileNameCer = string.Empty;
                            FileNameCer = System.IO.Path.GetFileName(file.FileName);
                            FilePathCer = "d_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + FileNameCer.Substring(FileNameCer.LastIndexOf('.'));
                            if (RB.SaveFileNew(ref file, FilePathCer))
                            {
                                DDN.ManagerID = 0;
                                DDN.ReceiptionID = 0;
                                DDN.DoctorID = i;
                                DDN.Image_Path = FilePathCer;
                                DDN.Filename = FileNameCer;
                                j = DBN.set(DDN);
                            }
                        }
                        z++;
                    }

                }
            }
            if (_doctorID > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Doctor detail updated successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Doctor detail saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process...", 2); return 0;
        }
        return i;
    }

    protected void txtWorkplace_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _workplaceID = 0; if (txtWorkplace.SelectedItem != null) { int.TryParse(txtWorkplace.SelectedItem.Value, out _workplaceID); }
        SnehBLL.WorkPlaces_Bll WB = new SnehBLL.WorkPlaces_Bll();
        if (_workplaceID > 0)
        {
            SnehDLL.WorkPlaces_Dll WD = WB.Get(_workplaceID);
            if (WD != null)
            {
                txtClinicAddress.Text = WD.Address;
            }
        }
    }
}
