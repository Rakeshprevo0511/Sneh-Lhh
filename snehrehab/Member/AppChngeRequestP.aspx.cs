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
using System.Globalization;

namespace snehrehab.Member
{
    public partial class AppChngeRequestP : System.Web.UI.Page
    {
        int _loginID = 0; int _requestID = 0; int _appointmentID = 0;
        SnehDLL.AppointmentChangeRequest_Dll CRD = null;
        SnehDLL.Appointments_Dll ACD = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            } 
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID == 3)
            {
                Response.Redirect(ResolveClientUrl("~/Member/"), true);
            }
            if (Request.QueryString["request"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["request"].ToString())) { _requestID = SnehBLL.AppointmentChangeRequest_Bll.Check(Request.QueryString["request"].ToString()); }
            }
            if (_requestID > 0)
            {
                SnehBLL.AppointmentChangeRequest_Bll CRB = new SnehBLL.AppointmentChangeRequest_Bll(); CRD = CRB.Get(_requestID);
                if (CRD != null)
                {
                    if (CRD.RequestStatus != 0)
                    {
                        Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
                    }
                    _appointmentID = CRD.AppointmentID;
                }
            }
            if (_appointmentID <= 0)
            {
                if (Request.QueryString["record"] != null)
                {
                    if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString())) { _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString()); }
                }
            }
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            if (_appointmentID > 0)
            {
                ACD = AB.Get(_appointmentID);
            }
            if (ACD != null)
            {
                if (ACD.AppointmentStatus != 0 || ACD.IsDeleted)
                {
                    if (_requestID > 0)
                        Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
                    else
                        Response.Redirect(ResolveClientUrl("~/Reports/AppointmentDaily.aspx"), true);
                }
                if (!IsPostBack)
                {
                    if (_requestID > 0)
                        btnCancel.HRef = "/Member/AppChngeRequest.aspx";
                    else
                        btnCancel.HRef = "/Reports/AppointmentDaily.aspx";

                    DataSet ds = AB.PatientDetail(ACD.PatientID);
                    if (ds.Tables.Count > 0)
                    {
                        PatientGV.DataSource = ds.Tables[0];
                    }
                    PatientGV.DataBind();
                    if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    LoadFormData();
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Reports/AppointmentDaily.aspx"), true);
            }
        }

        private void LoadFormData()
        {
            SnehBLL.SessionMast_Bll PSB = new SnehBLL.SessionMast_Bll();
            txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
            foreach (SnehDLL.SessionMast_Dll PSD in PSB.GetList())
            {
                txtSession.Items.Add(new ListItem(PSD.SessionName, PSD.SessionID.ToString()));
            }
            if (txtSession.Items.FindByValue(ACD.SessionID.ToString()) != null)
            {
                txtSession.SelectedValue = ACD.SessionID.ToString();
            }
            txtAppointmentDate.Text = ACD.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat);

            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            txtAssistant1.Items.Clear(); txtAssistant1.Items.Add(new ListItem("Select Assistant", "-1"));
            txtAssistant1.Items.Add(new ListItem("OBSERVER", "0"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtAssistant1.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }

            LoadTherapist();
            SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
            foreach (SnehDLL.AppointmentDoctor_Dll ADD in ADB.GetList(_appointmentID))
            {
                if (ADD.IsMain)
                {
                    if (txtTherapist.Items.FindByValue(ADD.DoctorID.ToString()) != null)
                    {
                        txtTherapist.SelectedValue = ADD.DoctorID.ToString();
                    }
                }
                if (!ADD.IsMain)
                {
                    if (txtAssistant1.Items.FindByValue(ADD.DoctorID.ToString()) != null)
                    {
                        txtAssistant1.SelectedValue = ADD.DoctorID.ToString();
                    }
                }
            }
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            txtTimeFrom.Items.Clear(); txtTimeFrom.Items.Add(new ListItem("Select Time", "-1"));
            foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.GetList())
            {
                txtTimeFrom.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
            }
            LoadTiming();
            SnehDLL.AppointmentTime_Dll ATd = ATB.Get(ACD.AppointmentTime);
            if (ATd != null)
            {
                if (txtTimeFrom.Items.FindByValue(ATd.TimeID.ToString()) != null)
                {
                    txtTimeFrom.SelectedValue = ATd.TimeID.ToString();
                }
            }
            if (txtDuration.Items.FindByValue(ACD.Duration.ToString()) != null)
            {
                txtDuration.SelectedValue = ACD.Duration.ToString();
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        protected void txtSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTherapist();
        }

        private void LoadTherapist()
        {
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);

            SnehBLL.Appointments_Bll APB = new SnehBLL.Appointments_Bll();
            txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in APB.getTherapist(_sessionID, _appointmentDate))
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }
            if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
            bool _hasMultipleDoctor = false; if (txtAssistant1.Items.Count > 0) { txtAssistant1.SelectedIndex = 0; }
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
            if (SMD != null) { _hasMultipleDoctor = SMD.MultipleDoctor; }
            if (_hasMultipleDoctor)
            {
                Tab_Assistant1.Visible = true;
            }
            else
            {
                Tab_Assistant1.Visible = false;
            }
            LoadTiming();
        }

        private void LoadTiming()
        {
            if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
            int _patientID = ACD.PatientID;
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            if (_patientID > 0)
            {
                foreach (SnehDLL.Appointments_Dll AD in AB.GetPatientSchedule(_patientID, _appointmentDate))
                {
                    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.PatientSchedule(AD.AppointmentTime, AD.Duration))
                    {
                        if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                        {
                            txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:green");
                        }
                    }
                }
            }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            if (_doctorID > 0)
            {
                foreach (SnehDLL.Appointments_Dll AD in AB.GetDoctorSchedule(_doctorID, _appointmentDate))
                {
                    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorSchedule(AD.AppointmentTime, AD.Duration))
                    {
                        if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                        {
                            txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                        }
                    }
                }
                /********** DOCTOR LEAVE CHECK HERE **********/
                SnehBLL.LeaveApplications_Bll LB = new SnehBLL.LeaveApplications_Bll();
                DataTable dt = LB.DoctorSchedule(_doctorID, _appointmentDate);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int _leaveType = 0; int.TryParse(dt.Rows[i]["TypeID"].ToString(), out _leaveType);
                    if (_leaveType == 4)
                    {
                        int _leaveID = 0; int.TryParse(dt.Rows[i]["LeaveID"].ToString(), out _leaveID);
                        foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorSchedule(_leaveID))
                        {
                            if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                            {
                                txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:#EDBF2D");
                            }
                        }
                    }
                    else
                    {
                        foreach (ListItem li in txtTimeFrom.Items)
                        {
                            li.Attributes.Add("style", "background-color:#EDBF2D");
                        }
                        if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
                        DbHelper.Configuration.setAlert(Page, "Selected therapist is on leave.", 3);
                        break;
                    }
                }
                /********** DOCTOR LEAVE CHECK HERE **********/
            }
        }

        protected void txtTherapist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTiming();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            if (_sessionID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select session for appointment...", 2); return;
            }
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);
            if (_appointmentDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select date of appointment...", 2); return;
            }
            if (_appointmentDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select date of appointment...", 2); return;
            }
            int _therapist = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _therapist); }
            if (_therapist <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select therapist for appointment...", 2); return;
            }
            int _timeID = 0; if (txtTimeFrom.SelectedItem != null) { int.TryParse(txtTimeFrom.SelectedItem.Value, out _timeID); }
            if (_timeID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select time of appointment...", 2); return;
            }
            TimeSpan _appointmentTime = new TimeSpan();
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehDLL.AppointmentTime_Dll ATD = ATB.Get(_timeID);
            if (ATD != null)
            {
                _appointmentTime = ATD.TimeHour;
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Please select time of appointment...", 2); return;
            }
            int _duration = 0; if (txtDuration.SelectedItem != null) { int.TryParse(txtDuration.SelectedItem.Value, out _duration); }
            if (_duration <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select duration of appointment...", 2); return;
            }
            DateTime _appointmentFrom = _appointmentDate.Date.Add(_appointmentTime);
            DateTime _appointmentUpto = _appointmentFrom.AddMinutes(_duration);

            int _assistantTherapist = -1; float _assistantShare = 0; int _assistantShareType = 0;

            bool _hasMultipleDoctor = false;
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll(); SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
            if (SMD != null) { _hasMultipleDoctor = SMD.MultipleDoctor; }
            if (_hasMultipleDoctor)
            {
                if (txtAssistant1.SelectedItem != null) { int.TryParse(txtAssistant1.SelectedItem.Value, out _assistantTherapist); }
                float.TryParse(txtAssistantShare.Text, out _assistantShare);
                if (txtAssistantShareType.SelectedItem != null) { int.TryParse(txtAssistantShareType.SelectedItem.Value, out _assistantShareType); }
            }
            if (txtNarration.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "enter appointment change remark...", 2); return;
            }

            SnehDLL.Appointments_Dll AD = new SnehDLL.Appointments_Dll();
            AD.AppointmentID = _appointmentID; AD.UniqueID = "";
            AD.PatientID = ACD.PatientID; AD.SessionID = _sessionID;
            AD.AppointmentDate = _appointmentDate;
            AD.AppointmentTime = _appointmentTime;
            AD.AppointmentFrom = _appointmentFrom;
            AD.AppointmentUpto = _appointmentUpto;
            AD.Duration = _duration;
            AD.Narration = txtNarration.Text.Trim();
            AD.AppointmentStatus = 0;
            AD.IsDeleted = false; AD.AddedDate = DateTime.UtcNow.AddMinutes(330);
            AD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            AD.AddedBy = _loginID; AD.ModifyBy = _loginID;

            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            int i = AB.Update(AD, _requestID);
            if (i > 0)
            {
                SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
                SnehDLL.AppointmentDoctor_Dll ADD = new SnehDLL.AppointmentDoctor_Dll();
                if (_therapist > 0)
                {
                    ADD.AppointmentID = _appointmentID; ADD.DoctorID = _therapist; ADD.IsMain = true; ADD.ShareType = 2; ADD.ShareAmount = 100;
                    ADB.setNew(ADD);
                }
                if (_assistantTherapist > -1)
                {
                    ADD.AppointmentID = _appointmentID; ADD.DoctorID = _assistantTherapist; ADD.IsMain = false; ADD.ShareType = 2; ADD.ShareAmount = 0;
                    ADB.setNew(ADD);
                }

                Session[DbHelper.Configuration.messageTextSession] = "Appointment detail changed successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                if (_requestID > 0)
                    Response.Redirect(ResolveClientUrl("~/Member/AppChngeRequest.aspx"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Reports/AppointmentDaily.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again..", 2);
            }
        }
    }
}
