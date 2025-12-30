using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class Doctorv : System.Web.UI.Page
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
            if (_doctorID > 0)
            {
                _headerText = "View Doctor Detail";
                if (!IsPostBack)
                {
                    LoadForm();
                    LoadDoctor();
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/Doctors.aspx"), true);
            }
        }

        private void LoadDoctor()
        {
            //



            SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
            SnehDLL.DoctorMast_Dll DD = DB.Get(_doctorID);
            if (DD != null)
            {
                txtFullName.Text = DD.FullName;
                txtQualification.Text = DD.Qualification;
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
            OWD.WorkplaceID = 0; OWD.Workplace = "Other"; WDL.Add(OWD);
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