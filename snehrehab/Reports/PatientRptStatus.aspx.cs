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
    public partial class PatientRptStatus : System.Web.UI.Page
    {
        int _loginID = 0; public DataTable dt = new DataTable();

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
            ReportGV.PageIndex = 0; 
            LoadData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex;
            LoadData();
        }

        private void LoadData()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataSet ds = DB.PatientRptStatus(_fromDate, _uptoDate);
            dt = ds.Tables[0];
           
            ReportGV.DataSource = dt;
            ReportGV.DataBind();
          
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataSet ds = DB.PatientRptStatus(_fromDate, _uptoDate);
             dt = ds.Tables[0];

          
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();

               

                html.Append("<table border=\"1\">");
                html.Append("<tr><th>SR NO</th><th>Name of Patient</th>");
                html.Append("<th>Therapist</th><th>DOE PT/OT</th><th>Session Name</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr><td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PatientMast"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["DoctorName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" +dt.Rows[i]["AppointmentDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
             
                }
               
              
                html.Append("<td></td></tr>");
                html.Append("</table>");

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=Patient Report Status- " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
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

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string GetText(string str, string _str, string t)
        {
            float amt = 0; float.TryParse(str, out amt);
            if (amt > 0)
            {
                DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
                if (_test > DateTime.MinValue)
                {
                    return "<a href=\"javascript:;\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"LoadAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + t + ")\">" + amt.ToString() + "</a>";
                }
                return amt.ToString();
            }
            return "- - -";
        }

       
    }

    
}
