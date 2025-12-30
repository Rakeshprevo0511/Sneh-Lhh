using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class Appointmentc : System.Web.UI.Page
    {
        int _loginID = 0; bool isAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            isAdmin = SnehBLL.UserAccount_Bll.IsAdmin();
            if (!isAdmin)
            {
                Response.Redirect("/Member/"); return;
            }
            if (!IsPostBack)
            {
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
                   1// DateTime.UtcNow.AddMinutes(330).Day
                    ).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AppointmentGV.PageIndex = 0; LoadData();
        }

        protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AppointmentGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            AppointmentGV.DataSource = DB.Search(1, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            AppointmentGV.DataBind();
            if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            AppointmentGV.Columns[7].Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                AppointmentGV.Columns[7].Visible = true;
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string TIMEDURATION(string _durationStr, string _timeStr)
        {
            int _duration = 0; int.TryParse(_durationStr, out _duration);
            DateTime TimeHourD = new DateTime();
            DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
            {
                return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "- - -";
        }

        public string GETACTION(string _uniqueID, string _appointmentStatus)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<a href=\"/Member/AppointmentPaidEdit.aspx?record=" + _uniqueID + "\" class=\"btn btn-mini btn-danger\"> Edit </a>");
            return html.ToString();
        }

        protected void AppointmentGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HiddenField txtA = e.Row.FindControl("txtAppointmentID") as HiddenField;
            HiddenField txtS = e.Row.FindControl("txtAppointmentStatusID") as HiddenField;
            if (txtA != null && txtS != null)
            {
                if (DbHelper.Configuration.IsGuid(txtA.Value))
                {
                    int _appointmentStatusID = 0; int.TryParse(txtS.Value, out _appointmentStatusID);

                    if (_appointmentStatusID == 1)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-complete";

                    }
                    else if (_appointmentStatusID == 2)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-absent";
                    }
                    else if (_appointmentStatusID == 10)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-cancel";

                    }
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            DataTable dt = DB.Search(1, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>Appointment List Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>SESSION</th><th>THERAPIST</th><th>DATE</th><th>DURATION</th><th>TIME</th><th>STATUS</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr " + GETCOLOR(dt.Rows[i]["AppointmentStatus"].ToString()) + ">");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + TIMEDURATION(dt.Rows[i]["Duration"].ToString(), dt.Rows[i]["AppointmentTime"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + GETSTATUS(dt.Rows[i]["AppointmentStatus"].ToString()) + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=appointment list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
            }
        }

        private string GETSTATUS(string _appointmentStatus)
        {
            int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
            if (_appointmentStatusID == 0)
                return "Pending";
            else if (_appointmentStatusID == 1)
                return "Completed";
            else if (_appointmentStatusID == 2)
                return "Absent";
            else if (_appointmentStatusID == 10)
                return "Cancelled";
            return "Unknown";
        }

        private string GETCOLOR(string _appointmentStatus)
        {
            int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
            if (_appointmentStatusID == 0)
                return "";
            else if (_appointmentStatusID == 1)
                return "style=\"color: #3C8600;\"";
            else if (_appointmentStatusID == 2)
                return "style=\"color: #ff0024;\"";
            else if (_appointmentStatusID == 10)
                return "style=\"color: #ff8400;\"";
            return "Unknown";
        }
    }
}