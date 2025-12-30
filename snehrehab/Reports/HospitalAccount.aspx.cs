using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using System.Text;

public partial class Reports_HospitalAccount : System.Web.UI.Page
{
    int _loginID = 0; DataTable dt = new DataTable(); decimal _credit = 0; decimal _debit = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                 1// DateTime.UtcNow.AddMinutes(330).Day
                  ).ToString(DbHelper.Configuration.showDateFormat);
            txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ReportGV.PageIndex = 0; LoadData();
    }

    protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ReportGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
        dt = DB.HospitalAccount(_fromDate, _uptoDate);
        ReportGV.DataSource = dt;
        ReportGV.DataBind();
        if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void ReportGV_DataBound(object sender, EventArgs e)
    {
        LoadTotal();
    }

    private void LoadTotal()
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            decimal _tmpC = 0; decimal _tmpD = 0;
            decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _tmpC);
            decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _tmpD);

            _credit += _tmpC;
            _debit += _tmpD;
        }
        PlaceHolder1.Controls.Add(new LiteralControl("<div class=\"alert alert-info\"><strong>" +
          "CREDIT AMT : " + Math.Round(_credit, 0).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +
          "DEBIT AMT : " + Math.Round(_debit, 0).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +
          "BALANCE AMT : " + Math.Round(_credit - _debit, 0).ToString() + "/- INR " +
          "</strong></div>"));
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
        dt = DB.HospitalAccount(_fromDate, _uptoDate);
        LoadTotal();

        if (dt.Rows.Count > 0)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<table>");
            html.Append("<tr><td><b>Report Name:</b></td><td>Hospital Account Report</td></tr>");
            html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
            html.Append("</table>");

            html.Append("<br/>");

            html.Append("<table border=\"1\">");
            html.Append("<tr><th>SR NO</th><th>ACCOUNT</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>CREDIT</th><th>DEDIT</th><th>DATE</th></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html.Append("<tr><td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["CreditAmt"].ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["DebitAmt"].ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["PayDate"].ToString()) + "</td></tr>");
            }
            html.Append("<tr><td></td><td></td><td></td><td><b>TOTAL</b></td>");
            html.Append("<td style=\"vertical-align:top;\"><b>" + Math.Round(_credit, 2).ToString() + "</b></td>");
            html.Append("<td style=\"vertical-align:top;\"><b>" + Math.Round(_debit, 2).ToString() + "</b></td>");
            html.Append("<td><b>" + Math.Round(_credit - _debit, 2).ToString() + "</b></td></tr>");
            html.Append("</table>");

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=hospital account report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
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