using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class AptWaitings : System.Web.UI.Page
    {
        int _loginID = 0; bool isAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            isAdmin = SnehBLL.UserAccount_Bll.IsAdminOrReception();
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
            int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
            int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            SnehBLL.AptWaiting_BAL DB = new SnehBLL.AptWaiting_BAL();
            AppointmentGV.DataSource = DB.Search(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            AppointmentGV.DataBind();
            if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
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

        public string GETACTION(string _appointmentStatus)
        {
            int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
            if (_appointmentStatusID == 1)
            {
                return ("<span class=\"label label-success label-mini\">Confirmed</span>");
            }
            else if (_appointmentStatusID == 10)
            {
                return ("<span class=\"label label-warning label-mini\">Cancelled</span>");
            }
            return string.Empty;
        }

        protected void AppointmentGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnConfirm = e.Row.FindControl("btnConfirm") as LinkButton; btnConfirm.Visible = false;
                LinkButton btnCancel = e.Row.FindControl("btnCancel") as LinkButton; btnCancel.Visible = false;
                HiddenField txtS = e.Row.FindControl("txtAppointmentStatusID") as HiddenField;
                if (txtS != null)
                {
                    int _appointmentStatusID = 0; int.TryParse(txtS.Value, out _appointmentStatusID);
                    if (_appointmentStatusID == 0)
                    {
                        btnConfirm.Visible = true;
                        btnCancel.Visible = true;
                    }
                    if (_appointmentStatusID == 1)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-complete";
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
            int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
            int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            SnehBLL.AptWaiting_BAL DB = new SnehBLL.AptWaiting_BAL();
            DataTable dt = DB.Search(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>Waiting List Report</td></tr>");
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
                Response.AddHeader("Content-Disposition", "attachment;filename=waiting list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
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
                return "Confirmed";
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender; int AppointmentID = 0;
            if (lk != null) { int.TryParse(lk.CommandArgument, out AppointmentID); }
            if (AppointmentID > 0)
            {
                SnehBLL.AptWaiting_BAL B = new SnehBLL.AptWaiting_BAL();
                int i = B.Status(AppointmentID, 1);
                if (i > 0)
                {
                    LoadData();
                    DbHelper.Configuration.setAlert(Page, "Appointment confirmed successfully..", 1);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process, please try again..", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process, please try again..", 2);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender; int AppointmentID = 0;
            if (lk != null) { int.TryParse(lk.CommandArgument, out AppointmentID); }
            if (AppointmentID > 0)
            {
                SnehBLL.AptWaiting_BAL B = new SnehBLL.AptWaiting_BAL();
                int i = B.Status(AppointmentID, 10);
                if (i > 0)
                {
                    LoadData();
                    DbHelper.Configuration.setAlert(Page, "Appointment cancelled successfully..", 1);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process, please try again..", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process, please try again..", 2);
            }
        }
    }
}