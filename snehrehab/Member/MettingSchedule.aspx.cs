using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{    
    public partial class MettingSchedule : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0;
        int toReturn = 0; public string returnUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["return"] != null)
            {
                int.TryParse(Request.QueryString["return"].ToString(), out toReturn);
            }
            if (toReturn == 101)
                returnUrl = "/Member/AppointmentChart.aspx";
            else
                returnUrl = "/Member/MettingSchedules.aspx";
            if (!IsPostBack)
            {
                LoadForm();
                LoadTiming();

            }
        }
        private void LoadForm()
        {
            txtAppointmentDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);

            txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));

            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();

            txtAvailability1Upto.Items.Clear(); txtAvailability1From.Items.Clear();
            txtTimeFrom.Items.Clear(); txtTimeUpto.Items.Clear();
            txtAvailability1From.Items.Add(new ListItem("Select Time", "-1"));
            txtAvailability1Upto.Items.Add(new ListItem("Select Time", "-1"));
            txtTimeFrom.Items.Add(new ListItem("Select Time", "-1"));
            txtTimeUpto.Items.Add(new ListItem("Select Time", "-1"));

            foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.GetList())
            {
                txtAvailability1Upto.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
                txtAvailability1From.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
                txtTimeFrom.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
                txtTimeUpto.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
            }
            LoadTherapist();
        }

        protected void txtAppointmentDate_TextChanged(object sender, EventArgs e)
        {
            LoadTherapist();
            LoadTiming();
        }

        private void LoadTherapist()
        {
            int _sessionID = 0; 
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);

            SnehBLL.Appointments_Bll APB = new SnehBLL.Appointments_Bll();
            txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in APB.getTherapist(_sessionID, _appointmentDate))
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }
        }
        protected void txtTherapist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTiming();
        }

        private void LoadTiming()
        {
            if ((txtAvailability1From.Items.Count > 0) && (txtAvailability1Upto.Items.Count > 0)) { txtAvailability1From.SelectedIndex = 0; txtAvailability1Upto.SelectedIndex = 0; }

            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            if (_doctorID > 0)
            {
                foreach (SnehDLL.Appointments_Dll AD in AB.GetDoctorSchedule(_doctorID, _appointmentDate))
                {
                    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorSchedule(AD.AppointmentTime, AD.Duration))
                    {
                        if ((txtAvailability1From.Items.FindByValue(ATD.TimeID.ToString()) != null) && (txtAvailability1Upto.Items.FindByValue(ATD.TimeID.ToString()) != null))
                        {
                            txtAvailability1From.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                            txtAvailability1Upto.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                        }
                        if((txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null) && (txtTimeUpto.Items.FindByValue(ATD.TimeID.ToString()) != null))
                        {
                            txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                            txtTimeUpto.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                        }
                    }
                }


                foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorMettingSchedule(_doctorID, _appointmentDate))
                {
                    if (ATD != null)
                    {
                        for (int i = ATD.TimeID; i < ATD.TimeID2; i++)
                        {
                            txtAvailability1From.Items.FindByValue(i.ToString()).Attributes.Add("style", "background-color:navy");
                        }
                        for (int j = ATD.TimeID; j < ATD.TimeID2; j++)
                        {
                            txtAvailability1Upto.Items.FindByValue(j.ToString()).Attributes.Add("style", "background-color:navy");
                        }
                        for(int i = ATD.TimeID; i < ATD.TimeID2; i++)
                        {
                            txtTimeFrom.Items.FindByValue(i.ToString()).Attributes.Add("style", "background-color:navy");
                        }
                        for (int j = ATD.TimeID; j < ATD.TimeID2; j++)
                        {
                            txtTimeUpto.Items.FindByValue(j.ToString()).Attributes.Add("style", "background-color:navy");
                        }
                    }
                    else { }
                }

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int i = SaveData();
            if (i > 0)
            {
                if (toReturn == 101)
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx?return=101"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Member/MettingSchedule.aspx"), true);
            }
        }
        public int SaveData()
        {
            int i = 0;

            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);
            if (_appointmentDate >= DateTime.MaxValue)
            {
                LoadTiming();
                DbHelper.Configuration.setAlert(Page, "Please select date of appointment...", 2); return i;
               
            }
            if (_appointmentDate <= DateTime.MinValue)
            {
                LoadTiming();
                DbHelper.Configuration.setAlert(Page, "Please select date of appointment...", 2); return i;

            }
            int _therapist = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _therapist); }
            
            if (_therapist <= 0)
            {
                LoadTiming();
                DbHelper.Configuration.setAlert(Page, "Please select Therapist for appointment...", 2); return i;
            }
            //TimeSpan _available1From = new TimeSpan();
            //DateTime _available1FromD = new DateTime(); DateTime.TryParseExact(txtAvailability1From.Text, DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _available1FromD);
            //if (_available1FromD > DateTime.MinValue && _available1FromD < DateTime.MaxValue)
            //{
            //    _available1From = _available1FromD.TimeOfDay;
            //}
            int _timeID = 0; if (txtTimeFrom.SelectedItem != null) { int.TryParse(txtTimeFrom.SelectedItem.Value, out _timeID); }
            if (_timeID <= 0)
            {
                LoadTiming();
                DbHelper.Configuration.setAlert(Page, "Please select time From for appointment...", 2); return i;
            }
            int _timeIDS = 0; if (txtTimeUpto.SelectedItem != null) { int.TryParse(txtTimeUpto.SelectedItem.Value, out _timeIDS); }
            if(_timeIDS <=0)
            {
                LoadTiming();
                DbHelper.Configuration.setAlert(Page, "Please select time Upto for appointment...", 2); return i;
            }
            //int _timeID1 = 0; if (txtAvailability1From.SelectedItem != null) { int.TryParse(txtAvailability1From.SelectedItem.Value, out _timeID1); }
            //if (_timeID1 <= 0)
            //{
            //   // LoadTiming();
            //    DbHelper.Configuration.setAlert(Page, "Please select time from of appointment...", 2); return i;
            //}
            //int _timeID2 = 0; if (txtAvailability1Upto.SelectedItem != null) { int.TryParse(txtAvailability1Upto.SelectedItem.Value, out _timeID2); }
            //if (_timeID2 <= 0)
            //{
            //   // LoadTiming();
            //    DbHelper.Configuration.setAlert(Page, "Please select time upto of appointment...", 2); return i;
            //}
            TimeSpan _available1From = new TimeSpan();
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehDLL.AppointmentTime_Dll ATD = ATB.Get(_timeID);
            if (ATD != null)

            {
                _available1From = ATD.TimeHour;
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Please select time of appointment...", 2); return i;
            }
            TimeSpan _available1Upto = new TimeSpan();
            SnehDLL.AppointmentTime_Dll ATDD = ATB.Get(_timeIDS);
            if (ATDD != null)
            {
                _available1Upto = ATDD.TimeHour;
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Please select time of appointment...", 2); return i;
            }
            DateTime _appointmentFrom = _appointmentDate.Date.Add(_available1From);
            DateTime _appointmentUpto = _appointmentDate.Date.Add(_available1Upto);
            // SnehDLL.MeetingScheTime_Dll DD = new SnehDLL.MeetingScheTime_Dll();
            SnehDLL.Appointments_Dll DD = new SnehDLL.Appointments_Dll();
            DD.AppointmentID = _appointmentID; DD.UniqueID = "";
            DD.AppointmentDate = _appointmentDate;
            DD.ScheduleType = txtScheduleType.Text.Trim();
            DD.IsDeleted = false; DD.AddedDate = DateTime.UtcNow.AddMinutes(330);
            DD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            DD.AddedBy = _loginID; DD.ModifyBy = _loginID;
            DD.Available1From = _appointmentFrom;
            DD.Available1Upto = _appointmentUpto;
            DD.Available1FromChar = txtTimeFrom.SelectedItem.Text.Trim() ;
            DD.Available1UptoChar = txtTimeUpto.SelectedItem.Text.Trim();
            DD.Available1FromTime = _available1From;
            DD.Available1UptoTime = _available1Upto;


            foreach (SnehDLL.Appointments_Dll atd in ATB.GetListAppointDr(_appointmentDate, _therapist))
            {
                if (atd != null)
                {
                    //if ((DD.AppointmentFrom > atd.Available1From && DD.AppointmentFrom < atd.Available1Upto) || (DD.AppointmentUpto > atd.Available1From && DD.AppointmentUpto < atd.Available1Upto))
                    if ((DD.Available1From > atd.AppointmentFrom && DD.Available1From < atd.AppointmentUpto) || (DD.Available1Upto > atd.AppointmentFrom && DD.Available1Upto < atd.AppointmentUpto)|| (DD.Available1From >= atd.Available1From && DD.Available1From < atd.Available1Upto) || (DD.Available1Upto > atd.Available1From && DD.Available1Upto <= atd.Available1Upto))
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select another time for appointment...", 2); return i;
                    }
                }
                else { }
            }

            foreach (SnehDLL.AppointmentTime_Dll atd in ATB.GetListDr(_appointmentDate, _therapist))
            {
                if (atd != null)
                {
                    //if ((atd.AppointmentFrom > DD.Available1From && atd.AppointmentFrom < DD.Available1Upto) || (atd.AppointmentUpto > DD.Available1From && atd.AppointmentUpto > DD.Available1Upto))
                    if ((DD.Available1From >= atd.AppointmentFrom && DD.Available1From < atd.AppointmentUpto) || (DD.Available1Upto > atd.AppointmentFrom && DD.Available1Upto < atd.AppointmentUpto))
                    //if((DD.Available1From >= atd.Available1From && DD.Available1From < atd.Available1Upto) || (DD.Available1Upto > atd.Available1From && DD.Available1Upto <= atd.Available1Upto))
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select another time for appointment...", 2); return i;
                    }
                }
                else { }

            }

            // SnehBLL.MeetngScheTime_Bll AB = new SnehBLL.MeetngScheTime_Bll();
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            i = AB.setDrMeetSche(DD);

            string courses_new = string.Empty;
            foreach (ListItem li in txtTherapist.Items)
            {
                if (li.Selected)
                {
                    courses_new =li.Value;
                    if (i > 0)
                    {
                        SnehBLL.AppointDrMeetSche_Bll ADB = new SnehBLL.AppointDrMeetSche_Bll();
                        SnehDLL.AppointmentDrMeetSch_Dll ADD = new SnehDLL.AppointmentDrMeetSch_Dll();

                        if (_therapist > 0 && li.Selected == true)
                        {
                            ADD.AppointmentID = i; ADD.DoctorID = courses_new; ADD.IsMain = true; ADD.ShareType = 2; ADD.ShareAmount = 100;
                            ADB.setNew(ADD);
                        }
                        Session[DbHelper.Configuration.messageTextSession] = "Dr Meeting Schedule detail saved successfully.";
                        Session[DbHelper.Configuration.messageTypeSession] = "1";
                    }
                    else
                    {
                        DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again..", 2);
                    }
                }
                
            }
            //LoadForm();
            return i;
        }
    }
}