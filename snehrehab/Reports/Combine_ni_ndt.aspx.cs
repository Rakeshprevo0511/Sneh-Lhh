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


    public partial class Combine_ni_ndt : System.Web.UI.Page
    {
        int _loginID = 0; bool isSuperAdmin = false, _rtype = false; int _doctorID = 0;
        SnehBLL.UserAccount_Bll ua = new SnehBLL.UserAccount_Bll();
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 4 || SnehBLL.UserAccount_Bll.getCategory() == 5 || SnehBLL.UserAccount_Bll.getCategory() == 6)
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
                          1//DateTime.UtcNow.AddMinutes(330).Day
                           ).ToString(DbHelper.Configuration.showDateFormat);

                SearchData();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; SearchData();
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


        private void SearchData()
        {
            //  _loginID =
            int.TryParse(txtDoctor.SelectedValue.ToString(), out _doctorID);

            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            ReportGV.DataSource = SID.Combine_SI_NDT(_doctorID, txtSearch.Text.Trim(), ddlStatus.Text.Trim(), _fromDate, _uptoDate, true);
            ReportGV.DataBind();
            if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }
        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; SearchData();
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string GetActionTemplate(string UniqueID, string ReportSource, string isFilled)
        {
            bool isSuperAdmin = false; // Assuming isSuperAdmin is defined somewhere else
            SnehBLL.UserAccount_Bll ua = new SnehBLL.UserAccount_Bll();

            if (isSuperAdmin || SnehBLL.UserAccount_Bll.getCategory() == 2)
            {
                return string.Empty;
            }

            if (_loginID == ua.AuthManager())
            {
                string viewEditLink = "";
                string printLink = "";
                string downloadLink = "";
                string mailLink = "";

                if (ReportSource == "Rpt_SI_2022")
                {
                    viewEditLink = "<a href=\"/SessionRpt/SIRpt2022.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
                    if (isFilled == "Complete")
                    {
                        printLink = "<a href=\"/SessionRpt/CreateRpt.ashx?type=si_rpt2022&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a>";
                        downloadLink = "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?type=si_rpt2022&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
                    }
                    else
                    {
                        printLink = "Pending";
                        downloadLink = "Pending</br>";
                    }
                    mailLink = "<a href=\"/Member/SendMail.aspx?typesi2022=si2022&record=" + UniqueID + "\" target=\"_blank\">Send Mail</a>";
                }
                else if (ReportSource == "Report_Ndt")
                {
                    viewEditLink = "<a href=\"/SessionRpt/NdtRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
                    if (isFilled == "Complete")
                    {
                        printLink = "<a href=\"/SessionRpt/CreateRpt.ashx?type=ndt&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a>";
                        downloadLink = "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?type=ndt&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
                    }
                    else
                    {
                        printLink = "Pending";
                        downloadLink = "Pending</br>";
                    }
                    mailLink = "<a href=\"/Member/SendMail.aspx?ndttype=ndt&record=" + UniqueID + "\" target=\"_blank\">Send Mail</a>";
                }
                else if (ReportSource == "Report_Si")
                {
                    viewEditLink = "<a href=\"/SessionRpt/SiRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
                    if (isFilled == "Complete")
                    {
                        printLink = "<a href=\"/SessionRpt/CreateRpt.ashx?type=si&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a>";
                        downloadLink = "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?type=si&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
                    }
                    else
                    {
                        printLink = "Pending";
                        downloadLink = "Pending</br>";
                    }
                    mailLink = "<a href=\"/Member/SendMail.aspx?ndttype=si&record=" + UniqueID + "\" target=\"_blank\">Send Mail</a>";
                }
                else if (ReportSource == "Report_EPI")
                {
                    viewEditLink = "<a href=\"/SessionRpt/EIPRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
                    if (isFilled == "Complete")
                    {
                        printLink = "<a href=\"/SessionRpt/CreateRpt.ashx?type=5&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a>";
                        downloadLink = "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?type=5&record=" + UniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
                    }
                    else
                    {
                        printLink = "Pending";
                        downloadLink = "Pending</br>";
                    }
                    mailLink = "<a href=\"/Member/SendMail.aspx?ndttype=5&record=" + UniqueID + "\" target=\"_blank\">Send Mail</a>";
                }

                return viewEditLink + " | " + printLink + " | " + downloadLink + " | " + mailLink;
            }

            return string.Empty;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int.TryParse(txtDoctor.SelectedValue.ToString(), out _loginID);

            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            DataTable dt = SID.Combine_SI_NDT(_loginID, txtSearch.Text.Trim(), ddlStatus.Text.Trim(), _fromDate, _uptoDate, true);

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
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>SESSION</th><th>THERAPIST</th><th>DATE</th><th>DURATION</th><th>TIME</th><th>MAIL SEND	</th></tr>");
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
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + TIMEDURATION(dt.Rows[i]["Duration"].ToString(), dt.Rows[i]["AppointmentTime"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MailSend"].ToString() + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ReportSource"].ToString() + "</td>");
                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + GetCentreName(centrename) + "  SI Report.xls");
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
                return centre + " " + "  SI&Ntd Report";
            }
            else
            {
                return "  SI&Ntd  Report";
            }
        }

    }
}