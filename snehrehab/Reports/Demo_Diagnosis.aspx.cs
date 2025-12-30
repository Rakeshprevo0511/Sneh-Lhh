using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text;

namespace snehrehab.Reports
{
    public partial class Demo_Diagnosis : System.Web.UI.Page
    {
        int _loginID = 0; bool _issuperadmin = false; 

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 4)
            {
                _issuperadmin = true;
            }
            if (!IsPostBack)
            {
                txtDiagnosisPatientType.Items.Clear(); txtDiagnosisPatientType.Items.Add(new ListItem("Any Registration", "-1"));
                SnehBLL.PatientTypes_Bll PTB = new SnehBLL.PatientTypes_Bll();
                foreach (SnehDLL.PatientTypes_Dll PTD in PTB.GetList())
                {
                    txtDiagnosisPatientType.Items.Add(new ListItem(PTD.PatientType, PTD.PatientTypeID.ToString()));
                }

                if (Request.QueryString["search"] != null)
                {
                    if (Request.QueryString["search"].ToString().Length > 0)
                    {
                        txtSearch.Text = Request.QueryString["search"].ToString();
                    }
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            int txtpatienttypeID = 0; if (txtDiagnosisPatientType.SelectedItem != null) { int.TryParse(txtDiagnosisPatientType.SelectedItem.Value, out txtpatienttypeID); }
            DateTime _fromdate = new DateTime(); DateTime.TryParseExact(txtFrom.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromdate);
            DateTime _uptodate = new DateTime(); DateTime.TryParseExact(txtUpto.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptodate);
            SnehBLL.Diagnosis_Bll PB = new SnehBLL.Diagnosis_Bll();
            DiagnosisGV.DataSource = PB.Search(txtpatienttypeID, txtSearch.Text, txtDiagnosis.Text, _fromdate, _uptodate);
            DiagnosisGV.DataBind();
            if (DiagnosisGV.HeaderRow != null) { DiagnosisGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DiagnosisGV.PageIndex = 0; LoadData();
        }

        protected void DiagnosisGV_PageIndexChanging(object sender, EventArgs e)
        {
            DiagnosisGV.PageIndex = 0; LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int txtpatienttypeID = 0; if (txtDiagnosisPatientType.SelectedItem != null) { int.TryParse(txtDiagnosisPatientType.SelectedItem.Value, out txtpatienttypeID); }
            DateTime _fromdate = new DateTime(); DateTime.TryParseExact(txtFrom.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromdate);
            DateTime _uptodate = new DateTime(); DateTime.TryParseExact(txtFrom.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptodate);
            SnehBLL.Diagnosis_Bll PB = new SnehBLL.Diagnosis_Bll();
            DataTable dt = PB.Search(txtpatienttypeID, txtSearch.Text, txtDiagnosis.Text, _fromdate, _uptodate);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name :</b></td><td>Diagnosis Patient List</td></tr>");
                html.Append("<tr><td><b>Report Date :</b></td><td>" + _fromdate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptodate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("</br>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><th>SR NO</th><th>PATIENT CODE</th><th>FULL NAME</th><th>TELEPHONE</th><th>ADDRESS</th><th>BIRTH DATE</th><th>REG DATE</th><th>DIAGNOSIS</th><th>REFERRED BY</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PatientCode"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["TelephoneNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["rAddress"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["BirthDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["RegistrationDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Diagnosis"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ReferredBy"].ToString() + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=Diagnosis Patient List -" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
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