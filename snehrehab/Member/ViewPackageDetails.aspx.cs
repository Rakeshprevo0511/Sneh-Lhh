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
    public partial class ViewPackageDetails : System.Web.UI.Page
    {
        int _loginID = 0; bool isSuperAdmin = false; int _bookingID = 0;
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
                // Read from Query String first time only
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    if (!int.TryParse(Request.QueryString["id"], out _bookingID))
                    {
                        DbHelper.Configuration.setAlert(Page, "Invalid Package ID", 3);
                        Response.Redirect("/Member/PackageBookings.aspx");
                        return;
                    }
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Package ID missing", 3);
                    Response.Redirect("/Member/PackageBookings.aspx");
                    return;
                }

                // Save to HiddenField
                hfBookingID.Value = _bookingID.ToString();

                LoadData();
            }
            else
            {
                // Read BookingID again on postback
                if (!string.IsNullOrEmpty(hfBookingID.Value))
                    _bookingID = Convert.ToInt32(hfBookingID.Value);
            }

        }

        private void LoadData()
        {
            SnehBLL.PatientPackage_Bll DB = new SnehBLL.PatientPackage_Bll();
            DataTable dt = DB.GetPacakgeUsage(_bookingID);
            BookingGV.DataSource = dt;
            BookingGV.DataBind();
        }
        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Member/PackageBookings.aspx");
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SnehBLL.PatientPackage_Bll DB = new SnehBLL.PatientPackage_Bll();
            DataTable dt = DB.GetPacakgeUsage(_bookingID);

            if (dt == null || dt.Rows.Count == 0)
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
                return;
            }

            StringBuilder html = new StringBuilder();

            html.Append("<table style=\"font-family: Verdana; font-size: 11px;\">");
            html.Append("<tr><td><b>Report Name:</b></td><td>Package Usage Details</td></tr>");

            html.Append("</table><br/>");

            html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px; border-collapse: collapse;\">");
            html.Append("<tr>");
            html.Append("<th>SR NO</th>");
            html.Append("<th>APPOINTMENT DATE</th>");
            html.Append("<th>TIME</th>");
            html.Append("<th>PACKAGE AMOUNT</th>");
            html.Append("<th>APPOINTMENT CHARGE</th>");
            html.Append("<th>REMAINING BALANCE</th>");
            html.Append("<th>ENTRY DATE</th>");
            html.Append("<th>ADDED DATE</th>");
            html.Append("<th>ADDED BY</th>");
            html.Append("<th>MODIFY BY</th>");
            html.Append("</tr>");

            int sr = 1;

            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");

                html.Append("<td style='vertical-align:top;'>" + sr + "</td>");

                string apptDate = row["AppointmentDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["AppointmentDate"]).ToString("dd-MMM-yyyy")
                    : "-";
                html.Append("<td style='vertical-align:top;'>" + apptDate + "</td>");

                string apptTime = row["AppointmentTime"] != DBNull.Value
                    ? Convert.ToDateTime(row["AppointmentTime"]).ToString("hh:mm tt")
                    : "-";

                html.Append("<td style='vertical-align:top;'>" + apptTime + "</td>");

                string packageAmount = SafeStr(row["PackageAmount"]);
                html.Append("<td style='vertical-align:top;'> " + packageAmount + "</td>");

                string appointCharge = SafeStr(row["AppointmentCharge"]);
                html.Append("<td style='vertical-align:top;'>" + appointCharge + "</td>");
                string remaining = row["RemainingBalance"] != DBNull.Value
                    ? Convert.ToDecimal(row["RemainingBalance"]).ToString("0.00")
                    : "-";
                html.Append("<td style='vertical-align:top;'> " + remaining + "</td>");

                string addedDate = row["AddedDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["AddedDate"]).ToString("dd-MMM-yyyy")
                    : "-";

                html.Append("<td style='vertical-align:top;'>" + addedDate + "</td>");
                string entryDate = "-";
                if (row["EntryDate"] != DBNull.Value)
                {
                    entryDate = Convert.ToDateTime(row["EntryDate"])
                                    .ToString("dd-MMM-yyyy hh:mm tt");
                }
                html.Append("<td style='vertical-align:top;'>" + entryDate + "</td>");

                html.Append("<td style='vertical-align:top;'>" + SafeStr(row["AddedByName"]) + "</td>");
                html.Append("<td style='vertical-align:top;'>" + SafeStr(row["ModifyByName"]) + "</td>");

                html.Append("</tr>");

                sr++;
            }

            html.Append("</table>");

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=Package_Usage_" +
                DateTime.UtcNow.AddMinutes(330).Ticks + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            Response.Output.Write(html.ToString());
            Response.End();
        }

        private string SafeStr(object value)
        {
            if (value == null || value == DBNull.Value)
                return "-";

            string v = value.ToString().Trim();
            return v == "" ? "-" : v;
        }
    }
}