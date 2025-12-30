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
using System.Text;
using System.Globalization;
using System.IO;

namespace snehrehab.Member
{
    public partial class MettingSchedules : System.Web.UI.Page
    {
        int _loginID = 0; bool isAdmin = false; bool isSuperAdmin = false;
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
                    lblAddNew.Text = "<a href=\"/Member/MettingSchedule.aspx\" class=\"btn btn-primary\">Add New</a> " ;
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
        private void LoadData()
        {         
            int _status = 0; //if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }   
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }            
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
             int _duration = 0; //if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);
            SnehBLL.MeetngScheTime_Bll DB = new SnehBLL.MeetngScheTime_Bll();
            AppointmentGV.DataSource = DB.SearchDrMeetingSche(_status, _doctorID, _fromDate, _uptoDate, _duration);
            AppointmentGV.DataBind();
            if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AppointmentGV.PageIndex = 0; LoadData();
        }

        protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AppointmentGV.PageIndex = e.NewPageIndex; LoadData();
        }
        public string DOCTORLIST(string _appointmentID)
        {
            int id = Convert.ToInt32(_appointmentID);
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            SnehDLL.Appointments_Dll AD = AB.Get(id);
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }
            SnehBLL.AppointDrMeetSche_Bll ADB = new SnehBLL.AppointDrMeetSche_Bll();
            string courses = string.Empty;
            foreach (SnehDLL.AppointmentDrMeetSch_Dll ADD in ADB.getlist_dr(AD.AppointmentID))
            {
                courses += ADD.DoctorName + " ,"; 
            }
            courses = courses.TrimEnd(',');
            return courses;
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
        protected void AppointmentGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
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
            }
            catch(Exception ex)
            {
               
            }
            
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Dr Meeting Schedule Report" + " " + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            AppointmentGV.GridLines = GridLines.Both;
            AppointmentGV.HeaderStyle.Font.Bold = true;
            AppointmentGV.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();


            //int _status = 0;
            //int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            //DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            //DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            //int _duration = 0; //if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);
            //SnehBLL.MeetngScheTime_Bll DB = new SnehBLL.MeetngScheTime_Bll();
            //DataTable dt = DB.SearchDrMeetingSche(_status, _doctorID,  _fromDate, _uptoDate, _duration);
            //if (dt.Rows.Count > 0)
            //{
            //    StringBuilder html = new StringBuilder();
            //    html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
            //    html.Append("<tr><td><b>Report Name:</b></td><td>Appointment List Report</td></tr>");
            //    html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
            //    html.Append("</table>");
            //    html.Append("<br/>");
            //    html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
            //    html.Append("<tr><th>SR NO</th><th>THERAPIST</th><th>DATE</th><th>TIME</th></tr>");
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        //html.Append("<tr " + GETCOLOR(dt.Rows[i]["AppointmentStatus"].ToString()) + ">");
            //        html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ScheduleType"].ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + TIMEDURATION(dt.Rows[i]["Duration"].ToString(), dt.Rows[i]["AppointmentTime"].ToString()) + "</td>");
            //        //html.Append("<td style=\"vertical-align:top;\">" + GETSTATUS(dt.Rows[i]["AppointmentStatus"].ToString()) + "</td>");
            //        html.Append("</tr>");
            //    }
            //    html.Append("</table>");
            //    Response.Clear();
            //    Response.AddHeader("Content-Disposition", "attachment;filename=appointment list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
            //    Response.ContentType = "application/vnd.xls";
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
            //    Response.Charset = "";
            //    Response.Output.Write(html.ToString());
            //    Response.End();
        }
        //    else
        //    {
        //        DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
        //    }
        //}

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        public string GETACTION(string _uniqueID, string _appointmentStatus)
        {
            StringBuilder html = new StringBuilder();
            int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
            if (_appointmentStatusID == 0)
            {
                if (!isSuperAdmin)
                {
                    html.Append("<a href=\"/Member/MettingScheduleCncl.aspx?record=" + _uniqueID + "\" class=\"btn-cancel btn-warning\">Delete</a>");

                    if (isAdmin)
                    {
                        html.Append("<a href=\"/Member/MettingScheduleEdit.aspx?record=" + _uniqueID + "\" class=\"btn-pay btn-primary\">Edit</a>")  ;
                    }
                }
                else
                {
                    html.Append("<span class=\"label label-primary label-mini\">Pending</span>");
                }
            }
            
            else if (_appointmentStatusID == 1)
            {
                html.Append("<span class=\"label label-success label-mini\">Completed</span>");
            }
            else if (_appointmentStatusID == 2)
            {
                html.Append("<span class=\"label label-important label-mini\">Absent</span>");
            }
            else if (_appointmentStatusID == 10)
            {
                html.Append("<span class=\"label label-warning label-mini\">Deleted</span>");
            }
            else
            {
                html.Append("<span class=\"label label-info label-mini\">Unknown</span>");
            }
            return html.ToString();
        }
        //private string GETSTATUS(string _appointmentStatus)
        //{
        //    int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
        //    if (_appointmentStatusID == 0)
        //        return "Pending";
        //    else if (_appointmentStatusID == 1)
        //        return "Completed";
        //    else if (_appointmentStatusID == 2)
        //        return "Absent";
        //    else if (_appointmentStatusID == 10)
        //        return "Cancelled";
        //    return "Unknown";
        //}

        //private string GETCOLOR(string _appointmentStatus)
        //{
        //    int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
        //    if (_appointmentStatusID == 0)
        //        return "";
        //    else if (_appointmentStatusID == 1)
        //        return "style=\"color: #3C8600;\"";
        //    else if (_appointmentStatusID == 2)
        //        return "style=\"color: #ff0024;\"";
        //    else if (_appointmentStatusID == 10)
        //        return "style=\"color: #ff8400;\"";
        //    return "Unknown";
        //}
    }
}