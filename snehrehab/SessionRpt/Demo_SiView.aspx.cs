using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace snehrehab.SessionRpt
{
    public partial class Demo_SiView : System.Web.UI.Page
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
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                          1//DateTime.UtcNow.AddMinutes(330).Day
                           ).ToString(DbHelper.Configuration.showDateFormat);

                DemoSearchData();
            }
        }

        private void DemoSearchData()
        {
            SnehBLL.ReportSiMst_Bll RDB = new SnehBLL.ReportSiMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            ReportGV.DataSource = RDB.DemoSearch(txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
            ReportGV.DataBind();
            if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; DemoSearchData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; DemoSearchData();
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

        public string SessionRptLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/CreateRpt.ashx?type=si&record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a>";
            }
            return "Pending";
        }

        public string GetAction(string UniqueID)
        {
            if (isSuperAdmin)
                return string.Empty;
            else
                return "<a href=\"/SessionRpt/Demo_SiRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
        }
    }
}