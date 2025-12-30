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
    public partial class RegPaymentDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        protected void RegPayDetailGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RegPayDetailGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int paymentmode = 0; if (txtpaymentmode.SelectedItem != null) { int.TryParse(txtpaymentmode.SelectedItem.Value, out paymentmode); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Diagnosis_Bll DB = new SnehBLL.Diagnosis_Bll();
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
            RegPayDetailGV.DataSource = PB.SearchRegPayDetail(paymentmode, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            RegPayDetailGV.DataBind();
            if (RegPayDetailGV.HeaderRow != null) { RegPayDetailGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RegPayDetailGV.PageIndex = 0; LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int paymentmode = 0; if (txtpaymentmode.SelectedItem != null) { int.TryParse(txtpaymentmode.SelectedItem.Value, out paymentmode); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Diagnosis_Bll DB = new SnehBLL.Diagnosis_Bll();
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
            DataTable dt = PB.SearchRegPayDetail(paymentmode, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>Registration List Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>REGISTRATION AMOUNT</th><th>PAY DATE</th><th>REG DATE</th><th>PAYMODE</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["CreditAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["PayDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["RegistrationDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PayMode"].ToString() + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=registration list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
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
    }
}