using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class AppointmentChart : System.Web.UI.Page
    {
        int _loginID = 0; bool isAdmin = false; public string courId = string.Empty;
        public List<dynamic> cal_resources = new List<dynamic>();
        public List<dynamic> cal_events = new List<dynamic>();
        public string start_time = string.Empty;
        public string end_time = string.Empty;
        bool isSuperAdmin = false; public int _appointmentId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            isAdmin = SnehBLL.UserAccount_Bll.IsAdminOrReception();
            if (SnehBLL.UserAccount_Bll.getCategory() == 4)
            {
                isSuperAdmin = true;
            }
            if (!IsPostBack)
            {
                if (!isSuperAdmin)
                {
                    lblAddNew.Text = "<a href=\"/Member/Appointment.aspx?return=101\" class=\"btn btn-primary\">Add New</a>" +
                        "&nbsp; <a href=\"/Member/AptWaiting.aspx?return=101\" class=\"btn btn-primary\">Add Waiting</a>";
                }
                SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
                ddl_Session.Items.Clear(); ddl_Session.Items.Add(new ListItem("Select Session", "-1"));
                foreach (DataRow item in DB.fill_Session().Rows)
                {
                    ddl_Session.Items.Add(new ListItem(item["SessionName"].ToString(), item["SessionID"].ToString()));
                }
                SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
                txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
                foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
                {
                    txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                }
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                   DateTime.UtcNow.AddMinutes(330).Day).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
            int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            if (_fromDate <= DateTime.MinValue)
            {
                _fromDate = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                   DateTime.UtcNow.AddMinutes(330).Day);
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                  DateTime.UtcNow.AddMinutes(330).Day).ToString(DbHelper.Configuration.showDateFormat);
            }
            DateTime _uptoDate = _fromDate;
            txtSearchDate.Value = _fromDate.ToString(DbHelper.Configuration.showDateFormat);
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll(); int o = 0;
            DataTable dt = DB.AptChart(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            List<int> doct = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int TherapistID = 0; int.TryParse(dt.Rows[i]["TherapistID"].ToString(), out TherapistID);
                if (TherapistID > 0)
                {
                    bool found = false;
                    for (int j = 0; j < doct.Count; j++)
                    {
                        if (TherapistID == doct[j])
                        {
                            found = true; break;
                        }
                    }
                    if (!found)
                    {
                        doct.Add(TherapistID);
                        cal_resources.Add(new
                        {
                            id = TherapistID.ToString(),
                            title = dt.Rows[i]["Therapist"].ToString(),
                        });
                    }
                }
            }
            SnehBLL.Appointments_Bll AABB = new SnehBLL.Appointments_Bll();
            SnehBLL.DoctorMast_Bll PPBB = new SnehBLL.DoctorMast_Bll();
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[o]["AppointmentID"].ToString(), out _appointmentId);
                foreach (SnehDLL.DoctorMast_Dll DM in PPBB.get_newapp(_appointmentId))
                {
                    bool found = false;
                    for (int j = 0; j < doct.Count; j++)
                    {
                        if (DM.DoctorID == doct[j])
                        {
                            found = true; break;
                        }
                    }
                    if (!found)
                    {

                        cal_resources.Add(new
                        {
                            id = DM.DoctorID.ToString(),
                            title = DM.FullName.ToString(),
                        });
                    }
                }
            }
            else
            {

            }

            List<int> docts = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int TherapistID = 0; int.TryParse(dt.Rows[i]["TherapistIDs"].ToString(), out TherapistID);
                if (TherapistID > 0)
                {
                    bool found = false;
                    for (int j = 0; j < docts.Count; j++)
                    {
                        if (TherapistID == docts[j])
                        {
                            found = true; break;
                        }
                    }
                    if (!found)
                    {
                        docts.Add(TherapistID); int ID = 0;
                        SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
                        //SnehDLL.Appointments_Dll AD = AB.Get(ID);
                        SnehBLL.DoctorMast_Bll PB = new SnehBLL.DoctorMast_Bll();
                        int.TryParse(dt.Rows[i]["AppointmentID"].ToString(), out _appointmentId);
                        foreach (SnehDLL.DoctorMast_Dll PD in PB.get_new(_appointmentId))
                        {
                            //courses += PD.FullName + " " ;
                            courId = PD.DoctorID.ToString();
                            cal_resources.Add(new
                            {
                                id = courId.ToString(),
                                // title = GetDR(Convert.ToInt32(dt.Rows[i]["AppointmentID"].ToString())),
                                title = PD.FullName.ToString(),
                            });
                        }
                    }
                }
            }
            DateTime AppointmentMaxTime = new DateTime();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime AppointmentFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                DateTime AppointmentUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);

                DateTime Available1From = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1From);
                DateTime Available1Upto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1Upto);

                if (AppointmentMaxTime > DateTime.MinValue)
                {
                    if (AppointmentMaxTime.TimeOfDay < AppointmentUpto.TimeOfDay || AppointmentMaxTime.TimeOfDay < Available1Upto.TimeOfDay)
                    {
                        AppointmentMaxTime = AppointmentUpto;
                        AppointmentMaxTime = Available1Upto;
                    }
                }
                else
                {
                    AppointmentMaxTime = AppointmentUpto;
                    AppointmentMaxTime = Available1Upto;
                }
                string className = string.Empty; int _appointmentStatusID = 0; int.TryParse(dt.Rows[i]["AppointmentStatus"].ToString(), out _appointmentStatusID);
                if (_appointmentStatusID == 0)
                {
                    int PatientTypeID = -1; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID);
                    if (PatientTypeID == 3)
                    {
                        className = "appointment-pre";
                    }
                    else if (PatientTypeID == 0)
                    {
                        className = "appointment-schedule";
                    }
                }
                else if (_appointmentStatusID == 1)
                {
                    className = "appointment-complete";
                }
                else if (_appointmentStatusID == 2)
                {
                    className = "appointment-absent";
                }
                else if (_appointmentStatusID == 10)
                {
                    className = "appointment-cancel";
                }
                else if (_appointmentStatusID == 50)
                {
                    className = "appointment-wait";
                }

                SnehBLL.Appointments_Bll ABB = new SnehBLL.Appointments_Bll();
                SnehBLL.DoctorMast_Bll PBB = new SnehBLL.DoctorMast_Bll();
                int.TryParse(dt.Rows[i]["AppointmentID"].ToString(), out _appointmentId);
                cal_events.Add(new
                {
                    id = dt.Rows[i]["UniqueID"].ToString(),
                    resourceId = dt.Rows[i]["TherapistID"].ToString(),
                    //resourceId =courId,
                    //start = dt.Rows[i]["AppointmentFrom"].ToString(),
                    //start = AppointmentFrom.ToString(DbHelper.Configuration.dateFormat),
                    start = AppointmentFrom.ToString("HH:mm:ss"),
                    //end = dt.Rows[i]["AppointmentUpto"].ToString(),
                    //end = AppointmentUpto.ToString(DbHelper.Configuration.dateFormat),
                    end = AppointmentUpto.ToString("HH:mm:ss"),
                    title = dt.Rows[i]["FullName"].ToString() + "<br/>" + dt.Rows[i]["SessionName"].ToString(),
                    className = className,
                    table = (_appointmentStatusID == 50 ? 1 : 0),
                });

                foreach (SnehDLL.DoctorMast_Dll DM in PBB.get_newapp(_appointmentId))
                {
                    bool found = false;
                    for (int j = 0; j < doct.Count; j++)
                    {
                        if (DM.DoctorID == doct[j])
                        {
                            found = true; break;
                        }
                    }
                    if (!found)
                    {

                        cal_events.Add(new
                        {
                            id = dt.Rows[i]["UniqueID"].ToString(),
                            resourceId = DM.DoctorID,
                            start = AppointmentFrom.ToString("HH:mm:ss"),
                            end = AppointmentUpto.ToString("HH:mm:ss"),
                            title = dt.Rows[i]["FullName"].ToString() + "<br/>" + dt.Rows[i]["SessionName"].ToString(),
                            className = className,
                            table = (_appointmentStatusID == 50 ? 1 : 0),
                        });
                    }
                }
                SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
                //SnehDLL.Appointments_Dll AD = AB.Get(ID);
                SnehBLL.DoctorMast_Bll PB = new SnehBLL.DoctorMast_Bll();
                int.TryParse(dt.Rows[i]["AppointmentID"].ToString(), out _appointmentId);
                foreach (SnehDLL.DoctorMast_Dll PD in PB.get_new(_appointmentId))
                {
                    courId = PD.DoctorID.ToString();
                    cal_events.Add(new
                    {
                        id = dt.Rows[i]["UniqueID"].ToString(),
                        resourceId = courId,
                        title = dt.Rows[i]["ScheduleType"].ToString(),
                        start = Available1From.ToString("HH:mm:ss"),
                        end = Available1Upto.ToString("HH:mm:ss"),
                        className = className,
                        table = (_appointmentStatusID == 50 ? 1 : 0),
                    });
                }
            }

            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            List<SnehDLL.AppointmentTime_Dll> ATL = ATB.GetList();

            SnehDLL.AppointmentTime_Dll ADF = ATL.OrderBy(r => r.TimeHour).FirstOrDefault();
            SnehDLL.AppointmentTime_Dll ADL = ATL.OrderByDescending(r => r.TimeHour).FirstOrDefault();
            start_time = new DateTime().Add(ADF.TimeHour).ToString("HH:mm:ss");
            if (AppointmentMaxTime > DateTime.MinValue)
            {
                if (AppointmentMaxTime.TimeOfDay > ADL.TimeHour)
                {
                    end_time = new DateTime().Add(AppointmentMaxTime.TimeOfDay).ToString("HH:mm:ss");
                }
                else
                {
                    end_time = new DateTime().Add(ADL.TimeHour).ToString("HH:mm:ss");
                }
            }
            else
            {
                end_time = new DateTime().Add(ADL.TimeHour).ToString("HH:mm:ss");
            }
        }
    }
}