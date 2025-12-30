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
using System.Text;

namespace snehrehab.Reports
{
    public partial class AdultAccountSheet : System.Web.UI.Page
    {
        int _loginID = 0; DataSet dsA = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                txtDoctors.Items.Clear(); txtDoctors.Items.Add(new ListItem("Select Doctor", "-1"));
                SnehBLL.DoctorMast_Bll PMB = new SnehBLL.DoctorMast_Bll();
                foreach (SnehDLL.DoctorMast_Dll PMD in PMB.GetForDropdown())
                {
                    txtDoctors.Items.Add(new ListItem(PMD.PreFix + " " + PMD.FullName, PMD.DoctorID.ToString()));
                }
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                     1// DateTime.UtcNow.AddMinutes(330).Day
                      ).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        private void LoadData()
        {
            int _doctorID = 0; if (txtDoctors.SelectedItem != null) { int.TryParse(txtDoctors.SelectedItem.Value, out _doctorID); }
            if (_doctorID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select doctor...", 2); return;
            }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date...", 2); return;
            }
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date...", 2); return;
            }
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            dsA = DB.AdultAccountSheet(_doctorID, _fromDate, _uptoDate);

            if (dsA.Tables.Count > 1)
            {
                StringBuilder html = new StringBuilder(); DataTable dt = dsA.Tables[0]; bool hasData = false;
                //html.Append("<table><tr><td>Name:</td><td>" + dt.Rows[0]["FullName"].ToString() + "</td><td></td><td>Branch:</td><td>" + dt.Rows[0]["BranchName"].ToString() + "</td></tr>");
                //html.Append("<tr><td>Report Date:</td><td colspan=\"4\">" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + "-" + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr></table>");

                int _recFrmA = 1;
                for (int i = _recFrmA; i < dsA.Tables.Count; i++)
                {
                    dt = dsA.Tables[i];
                    if (dt.Rows.Count > 0)
                    {
                        if (!hasData) hasData = true;

                        html.Append("<h6 style=\"font-style: italic;text-transform: uppercase;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                        html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>DR. AMT</th><th>HOS. AMT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (j + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                            html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                            //html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                            html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                            html.Append("</tr>");
                        }
                        html.Append("</tbody>");
                        html.Append("</table><hr/>");
                    }
                }
                if (hasData)
                {
                    txtContent.Text = html.ToString();
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "No records found...", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found...", 2);
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int _doctorID = 0; if (txtDoctors.SelectedItem != null) { int.TryParse(txtDoctors.SelectedItem.Value, out _doctorID); }
            if (_doctorID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select doctor...", 2); return;
            }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date...", 2); return;
            }
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date...", 2); return;
            }
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            dsA = DB.AdultAccountSheet(_doctorID, _fromDate, _uptoDate);

            if (dsA.Tables.Count > 1)
            {
                StringBuilder html = new StringBuilder(); DataTable dt = dsA.Tables[0]; bool hasData = false;
                html.Append("<table><tr><td><b>Name:</b></td><td>" + dt.Rows[0]["FullName"].ToString() + "</td><td></td><td><b>Branch:</b></td><td>" + dt.Rows[0]["BranchName"].ToString() + "</td></tr>");
                html.Append("<tr><td><b>Report Name:</b></td><td colspan=\"4\">Adult Account Sheet</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td colspan=\"4\">" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr></table>");

                int _recFrm = 1;
                for (int i = _recFrm; i < dsA.Tables.Count; i++)
                {
                    dt = dsA.Tables[i];
                    if (dt.Rows.Count > 0)
                    {
                        if (!hasData) hasData = true;

                        html.Append("<h6 style=\"text-decoration:underline;text-transform: uppercase;font-size:12pt;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                        html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>DR. AMT</th><th>HOS. AMT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (j + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                            //html.Append("<td>" + dt.Rows[j]["PatientType"].ToString() + "</td>");
                            html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                            html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                            //html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                            html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                            html.Append("</tr>");
                        }
                        html.Append("</tbody>");
                        html.Append("</table><br/><br/>");
                    }
                }
                if (hasData)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=adult account sheet - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                    Response.ContentType = "application/vnd.xls";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                    Response.Charset = "";
                    Response.Output.Write(html.ToString());
                    Response.End();
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "No records found...", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 2);
            }
        }
    }
}
