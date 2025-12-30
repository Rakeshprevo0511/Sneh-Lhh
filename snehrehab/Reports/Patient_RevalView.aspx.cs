using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace snehrehab.Reports
{
    public partial class Patient_RevalView : System.Web.UI.Page
    {
        int _loginID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
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
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            //int _doctorID = 0; if (txtDoctor.SelectedItem != null) { int.TryParse(txtDoctor.SelectedItem.Value, out _doctorID); }

            ReportGV.DataSource = RDB.DemoSearchReval(txtSearch.Text.Trim(), _fromDate, _uptoDate, false);
            ReportGV.DataBind();
            if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

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
        public static string SessionDownloadViewLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?type=ndt_new&record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
            }
            return "Pending</br>";
        }
    }
}