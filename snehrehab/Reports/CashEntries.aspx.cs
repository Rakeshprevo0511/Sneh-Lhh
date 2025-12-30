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
    public partial class CashEntries : System.Web.UI.Page
    {
        int _loginID = 0; DataTable dt = new DataTable(); decimal _debit = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                txtAccountType.DataBind(); LoadAccounts();
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                 1// DateTime.UtcNow.AddMinutes(330).Day
                  ).ToString(DbHelper.Configuration.showDateFormat);
                //txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);

                LoadData();
            }
        }

        protected void txtAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            int _accountType = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _accountType); }
            txtAccountName.Items.Clear(); txtAccountName.Items.Add(new ListItem("All Accounts", "-1"));
            if (_accountType == 1)
            {
                SnehBLL.AccountHeads_Bll AB = new SnehBLL.AccountHeads_Bll();
                foreach (SnehDLL.AccountHeads_Dll AD in AB.GetList())
                {
                    txtAccountName.Items.Add(new ListItem(AD.HeadName, AD.HeadID.ToString()));
                }
            }
            else if (_accountType == 2)
            {
                SnehBLL.DoctorMast_Bll AB = new SnehBLL.DoctorMast_Bll();
                foreach (SnehDLL.DoctorMast_Dll AD in AB.GetForDropdown())
                {
                    txtAccountName.Items.Add(new ListItem(AD.PreFix + " " + AD.FullName, AD.DoctorID.ToString()));
                }
            }
            else if (_accountType == 3)
            {
                SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
                foreach (SnehDLL.PatientMast_Dll PD in PB.GetForDropdown())
                {
                    txtAccountName.Items.Add(new ListItem(PD.FullName, PD.PatientID.ToString()));
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CashEntryGV.PageIndex = 0; LoadData();
        }

        protected void CashEntryGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CashEntryGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int _accountType = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _accountType); }
            int _accountID = 0; if (txtAccountName.SelectedItem != null) { int.TryParse(txtAccountName.SelectedItem.Value, out _accountID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.CashEntries_Bll DB = new SnehBLL.CashEntries_Bll();
            dt = DB.Search(_accountType, _accountID, _fromDate, _uptoDate, txtSearch.Text.Trim());
            CashEntryGV.DataSource = dt;
            CashEntryGV.DataBind();
            if (CashEntryGV.HeaderRow != null) { CashEntryGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            LoadTotal();
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        private void LoadTotal()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal _tmpD = 0;
                decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _tmpD);

                _debit += _tmpD;
            }
            PlaceHolder1.Controls.Add(new LiteralControl("<div class=\"alert alert-info\"><strong>" +
              "TOTAL AMT : " + Math.Round(_debit, 0).ToString() + "/- INR " +
              "</strong></div>"));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int _accountType = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _accountType); }
            int _accountID = 0; if (txtAccountName.SelectedItem != null) { int.TryParse(txtAccountName.SelectedItem.Value, out _accountID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.CashEntries_Bll DB = new SnehBLL.CashEntries_Bll();
            dt = DB.Search(_accountType, _accountID, _fromDate, _uptoDate, txtSearch.Text.Trim());
            LoadTotal();

            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table>");
                html.Append("<tr><td><b>Report Name:</b></td><td>Cash Entry Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");

                html.Append("<br/>");

                html.Append("<table border=\"1\">");
                html.Append("<tr><th>SR NO</th><th>ACCOUNT TYPE</th><th>ACCOUNT NAME</th><th>PRODUCT</th><th>DESCRIPTION</th><th>AMOUNT</th><th>DATE</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr><td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AccountType"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ProductName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Narration"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["DebitAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["PayDate"].ToString()) + "</td></tr>");
                }
                html.Append("<tr><td></td><td></td><td></td><td></td><td><b>TOTAL</b></td>");
                html.Append("<td style=\"vertical-align:top;\"><b>" + Math.Round(_debit, 2).ToString() + "</b></td>");
                html.Append("<td></td></tr>");
                html.Append("</table>");

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=cash entry report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 2);
            }
        }
    }
}
