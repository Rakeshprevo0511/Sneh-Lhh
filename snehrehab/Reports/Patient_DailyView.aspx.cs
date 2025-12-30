using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace snehrehab.Reports
{
    public partial class Patient_DailyView : System.Web.UI.Page
    {
        int _loginID = 0; bool isSuperAdmin = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 4)
            {
                isSuperAdmin = true;
            }
            if (!IsPostBack)
            {
                //txtDoctor.Items.Clear(); txtDoctor.Items.Add(new ListItem("ALL DOCTOR", "-1"));
                //SnehBLL.DoctorMast_Bll PMB = new SnehBLL.DoctorMast_Bll();
                //foreach (SnehDLL.DoctorMast_Dll PMD in PMB.GetForDropdown())
                //{
                //    txtDoctor.Items.Add(new ListItem(PMD.PreFix + " " + PMD.FullName, PMD.DoctorID.ToString()));
                //}
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
            //int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }
            int statusid = 0; if (txtstatus.SelectedItem != null) { int.TryParse(txtstatus.SelectedItem.Value, out statusid); }
            if (statusid > 0)
            {
                ReportGV.DataSource = RDB.Patient_SearchByStatus(txtSearch.Text.Trim(), _fromDate, _uptoDate, false, statusid);
                ReportGV.DataBind();
                if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            }
            else
            {
                ReportGV.DataSource = RDB.PatientSearch(txtSearch.Text.Trim(), _fromDate, _uptoDate, false);
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
            //if (isSuperAdmin)
            //    return string.Empty;
            //else if (SnehBLL.UserAccount_Bll.getCategory() == 2)
                return string.Empty;
            //else
            //    return "<a href=\"/SessionRpt/DailyRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
        }
    }
}