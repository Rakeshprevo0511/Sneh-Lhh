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
using System.Data.SqlClient;
using System.Text;

namespace snehrehab.Reports
{
    public partial class DailyView : System.Web.UI.Page
    {
        int _loginID = 0; bool isSuperAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 4 || SnehBLL.UserAccount_Bll.getCategory() == 5)
            {
                isSuperAdmin = true;
            }
            if (!IsPostBack)
            {
                txtDoctor.Items.Clear(); txtDoctor.Items.Add(new ListItem("ALL DOCTOR", "-1"));
                SnehBLL.DoctorMast_Bll PMB = new SnehBLL.DoctorMast_Bll();
                foreach (SnehDLL.DoctorMast_Dll PMD in PMB.GetForDropdown())
                {
                    txtDoctor.Items.Add(new ListItem(PMD.PreFix + " " + PMD.FullName, PMD.DoctorID.ToString()));
                }
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                     DateTime.UtcNow.AddMinutes(330).Day
                      ).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                SearchData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; SearchData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; SearchData();
        }

        private void SearchData()
        {
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }
            int statusid = 0; if (txtstatus.SelectedItem != null) { int.TryParse(txtstatus.SelectedItem.Value, out statusid); }
            //ReportGV.DataSource = RDB.Search(_doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, false);
            //ReportGV.DataBind();
            //if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            if (statusid > 0)
            {
                ReportGV.DataSource = RDB.SearchByStatus(_doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, false, statusid);
                ReportGV.DataBind();
                if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            }
            else
            {
                ReportGV.DataSource = RDB.Search(_doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, false);
                ReportGV.DataBind();
                if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            }

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

        public string IMPAIREMENTS(string _strAppointmentID)
        {
            int _appointmentID = 0; int.TryParse(_strAppointmentID, out _appointmentID);
            string str = string.Empty;
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            DataTable dt = RDB.GetAttr(_appointmentID, RDB._impairementTypeID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int _attributeID = 0; int.TryParse(dt.Rows[i]["AttributeID"].ToString(), out _attributeID);
                if (string.IsNullOrEmpty(str))
                    str = RDB.impairement_Get(_attributeID) + ",";
                else
                    str += "<br/>" + RDB.impairement_Get(_attributeID) + ",";
            }
            if (str.Length > 0 || !string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public string PERFORMANCE(string _strPerformanceID)
        {
            string str = string.Empty; int _performanceID = 0; int.TryParse(_strPerformanceID, out _performanceID);
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            if (_performanceID > 0)
                return RDB.Performance_Get(_performanceID);
            return str;
        }

        public string GOALASSSCALE(string _strGoalAssScaleID)
        {
            string str = string.Empty; int _goalAssScaleID = 0; int.TryParse(_strGoalAssScaleID, out _goalAssScaleID);
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            if (_goalAssScaleID > 0)
                return RDB.GoalScale_Get(_goalAssScaleID);
            return str;
        }

        public string GetAction(string UniqueID)
        {
            if (isSuperAdmin)
                return string.Empty;
            else if (SnehBLL.UserAccount_Bll.getCategory() == 2)
                return string.Empty;
            else if (SnehBLL.UserAccount_Bll.getCategory() == 6)
                return string.Empty;
            else
                return "<a href=\"/SessionRpt/DailyRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
        }

        public string GetMailink(string _uniqueID)
        {
            if (!string.IsNullOrEmpty(_uniqueID))
            {
                return "<a href=\"/Member/SendMail.aspx?dtype=dai&record=" + _uniqueID + "\" target=\"_blank\">Send Mail</a>";
            }
            return "";
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }
            int statusid = 0; if (txtstatus.SelectedItem != null) { int.TryParse(txtstatus.SelectedItem.Value, out statusid); }
            DataTable dt;

            if (statusid > 0)
            {
                dt = RDB.SearchByStatus(_doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, false, statusid);

            }
            else
            {
                dt = RDB.Search(_doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, false);

            }

            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty; DbHelper.SqlDb db = new DbHelper.SqlDb();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 BranchName FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPP = db.DbRead(cmd); if (dtPP.Rows.Count > 0) { centrename = dtPP.Rows[0]["BranchName"].ToString(); }
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>" + GetCentreName(centrename) + "</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>PATIENT NAME</th><th>SESSION</th><th>DATE</th><th>DURATION</th><th>SESSION GOAL</th><th>IMPAIREMENTS</th><th>ACTIVITY</th><th>EQUIPMENTS</th><th>PERFORMANCE</th><th>GOAL ASS. SCALE</th><th>LONG TERM GOAL</th><th>SHORT TERM GOAL</th><th>SUGGESTIONS</th><th>MAIL SEND</th>");
                string ReceiptPrefix = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtP = db.DbRead(cmd); if (dtP.Rows.Count > 0) { ReceiptPrefix = dtP.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalr = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionGoal"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AppointmentID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Activity"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Equipments"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PerformanceID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["GoalAssScaleID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["LongTermGoals"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ShortTermGoals"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Suggestions"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MailSend"].ToString() + "</td>");



                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + GetCentreName(centrename) + " Daily Report.xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No record found to export.", 3); return;
            }
        }
        public string GetCentreName(string centre)
        {
            if (!string.IsNullOrEmpty(centre))
            {
                return centre + " " + " Daily Report";
            }
            else
            {
                return " Daily Report";
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
    }
}
