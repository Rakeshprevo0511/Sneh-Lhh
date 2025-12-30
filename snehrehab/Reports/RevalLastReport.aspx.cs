using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Reports
{
    public partial class RevalLastReport : System.Web.UI.Page
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
              ///  txtDoctor.SelectedItem.Value = "3";

                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                          1//DateTime.UtcNow.AddMinutes(330).Day
                           ).ToString(DbHelper.Configuration.showDateFormat);

                SearchData();
            }
        }
        private void SearchData()
        {
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            int.TryParse(txtDoctor.SelectedValue.ToString(), out _loginID);

            ReportGV.DataSource = RDB.SearchLastReval_New(_loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; SearchData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; SearchData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            int.TryParse(txtDoctor.SelectedValue.ToString(), out _loginID);

            DataTable dt = RDB.SearchLastReval_New(_loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
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
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>SESSION</th><th>THERAPIST</th><th>LAST REVAL DATE</th><th>CURRENT REVAL DATE</th><th>POPUP REVAL DATE</th><th>REMARK</th>");
                string ReceiptPrefix = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtP = db.DbRead(cmd); if (dtP.Rows.Count > 0) { ReceiptPrefix = dtP.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalr = dt.Rows.Count;
                for (int i = 0; dt.Rows.Count > i; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["PopUpDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentFrom"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["yNarration"].ToString() + "</td>");

                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                //Response.AddHeader("Content-Disposition", "attachment;filename=print receipt report.xls");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + GetCentreName(centrename) + " OPD report.xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
        }
        public string GetCentreName(string centre)
        {
            if (!string.IsNullOrEmpty(centre))
            {
                return centre + " " + "Last RE-EVAL Report";
            }
            else
            {
                return "Last RE-EVAL Report";
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