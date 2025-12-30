using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace snehrehab.Reports
{
    public partial class PatienSession : System.Web.UI.Page
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
                SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
                //ddl_Session.Items.Clear(); ddl_Session.Items.Add(new ListItem("Select Session", "-1"));
                //foreach (DataRow item in DB.fill_Session().Rows)
                //{
                //    ddl_Session.Items.Add(new ListItem(item["SessionName"].ToString(), item["SessionID"].ToString()));
                //}
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
            int _SessionID = 0; /*if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }*/
            //int _duration = 0; if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataSet ds = DB.PatientSession(_fromDate, _uptoDate, /*txtSearch.Text.Trim(),*/ _SessionID/*, _duration*/);
            if (ds.Tables[0] != null)
            {
                dt = ds.Tables[0];
                ReportGV.DataSource = dt;
                ReportGV.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    int OTPT = dt.AsEnumerable().Sum(row => row.Field<int?>("OTPT") == null ? 0 : row.Field<int>("OTPT"));
                    int MATRIX = dt.AsEnumerable().Sum(row => row.Field<int?>("MATRIX") == null ? 0 : row.Field<int>("MATRIX"));
                    int GroupSession = dt.AsEnumerable().Sum(row => row.Field<int?>("GroupSession") == null ? 0 : row.Field<int>("GroupSession"));
                    int SpeechEvaluation = dt.AsEnumerable().Sum(row => row.Field<int?>("SpeechEvaluation") == null ? 0 : row.Field<int>("SpeechEvaluation"));
                    int SpeechTherapy = dt.AsEnumerable().Sum(row => row.Field<int?>("SpeechTherapy") == null ? 0 : row.Field<int>("SpeechTherapy"));
                    int ReEvaluation = dt.AsEnumerable().Sum(row => row.Field<int?>("ReEvaluation") == null ? 0 : row.Field<int>("ReEvaluation"));
                    int GeneralEvaluation = dt.AsEnumerable().Sum(row => row.Field<int?>("GeneralEvaluation") == null ? 0 : row.Field<int>("GeneralEvaluation"));
                    int FirstEvaluation = dt.AsEnumerable().Sum(row => row.Field<int?>("FirstEvaluation") == null ? 0 : row.Field<int>("FirstEvaluation"));
                    int SP2 = dt.AsEnumerable().Sum(row => row.Field<int?>("SP2") == null ? 0 : row.Field<int>("SP2"));
                    int IPDA = dt.AsEnumerable().Sum(row => row.Field<int?>("IPDA") == null ? 0 : row.Field<int>("IPDA"));
                    int IPDB = dt.AsEnumerable().Sum(row => row.Field<int?>("IPDB") == null ? 0 : row.Field<int>("IPDB"));
                    int OtherSession = dt.AsEnumerable().Sum(row => row.Field<int?>("OtherSession") == null ? 0 : row.Field<int>("OtherSession"));

                    ReportGV.FooterRow.Cells[0].Text = "Total";
                    ReportGV.FooterRow.Cells[0].Font.Bold = true;
                    ReportGV.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[2].Text = OTPT.ToString();
                    ReportGV.FooterRow.Cells[2].Font.Bold = true;
                    ReportGV.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[3].Text = MATRIX.ToString();
                    ReportGV.FooterRow.Cells[3].Font.Bold = true;
                    ReportGV.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[4].Text = GroupSession.ToString();
                    ReportGV.FooterRow.Cells[4].Font.Bold = true;
                    ReportGV.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[5].Text = SpeechEvaluation.ToString();
                    ReportGV.FooterRow.Cells[5].Font.Bold = true;
                    ReportGV.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[6].Text = SpeechTherapy.ToString();
                    ReportGV.FooterRow.Cells[6].Font.Bold = true;
                    ReportGV.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[7].Text = ReEvaluation.ToString();
                    ReportGV.FooterRow.Cells[7].Font.Bold = true;
                    ReportGV.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[8].Text = GeneralEvaluation.ToString();
                    ReportGV.FooterRow.Cells[8].Font.Bold = true;
                    ReportGV.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[9].Text = FirstEvaluation.ToString();
                    ReportGV.FooterRow.Cells[9].Font.Bold = true;
                    ReportGV.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                    ReportGV.FooterRow.Cells[10].Text = SP2.ToString();
                    ReportGV.FooterRow.Cells[10].Font.Bold = true;
                    ReportGV.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;

                    ReportGV.FooterRow.Cells[11].Text = IPDA.ToString();
                    ReportGV.FooterRow.Cells[11].Font.Bold = true;
                    ReportGV.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Left;

                    ReportGV.FooterRow.Cells[12].Text = IPDB.ToString();
                    ReportGV.FooterRow.Cells[12].Font.Bold = true;
                    ReportGV.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;

                    ReportGV.FooterRow.Cells[13].Text = OtherSession.ToString();
                    ReportGV.FooterRow.Cells[13].Font.Bold = true;
                }
                

            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found ...", 2);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int _SessionID = 0; /*if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }*/
            //int _duration = 0; if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();

            int TotOTPT=0; int TotMATRIX=0; int TotGroupSession=0; int TotSpeechEvaluation=0;int TotSpeechTherapy=0;int TotReEvaluation=0;
            int TotGeneralEvaluation=0;int TotFirstEvaluation=0; int TotSP2=0; int TotIPDA = 0; int TotIPDB = 0; int TotOtherSession=0; 
            DataSet ds = DB.PatientSession(_fromDate, _uptoDate, /*txtSearch.Text.Trim(),*/ _SessionID/*, _duration*/);
            SqlCommand cmd = new SqlCommand("PatientSessionReport"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _SessionID;
            //cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);

            dt = ds.Tables[0];
            if(dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 BranchName FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPP = db.DbRead(cmd); if (dtPP.Rows.Count > 0) { centrename = dtPP.Rows[0]["BranchName"].ToString(); }
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>Patient Session List</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\">");
                html.Append("<tr><th>SR NO</th><th>Name of Patient</th>");
                html.Append("<th>Pediatric Physio Therapy</th><th>MATRIX</th><th>GroupSession</th><th>SpeechEvaluation</th><th>SpeechTherapy</th><th>ReEvaluation</th><th>GeneralEvaluation</th><th>FirstEvaluation</th><th>SP2</th> <th>IPD Class A</th> <th>IPD Class B</th><th>OtherSession</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr><td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["OTPT"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MATRIX"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["GroupSession"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SpeechEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SpeechTherapy"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ReEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["GeneralEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FirstEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SP2"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["IPDA"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["IPDB"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["OtherSession"].ToString() + "</td>");

                        TotOTPT += int.Parse(dt.Rows[i]["OTPT"].ToString());
                        TotMATRIX += int.Parse(dt.Rows[i]["MATRIX"].ToString());
                    TotGroupSession += int.Parse(dt.Rows[i]["GroupSession"].ToString());
                    TotSpeechEvaluation += int.Parse(dt.Rows[i]["SpeechEvaluation"].ToString());
                    TotSpeechTherapy += int.Parse(dt.Rows[i]["SpeechTherapy"].ToString());
                    TotReEvaluation += int.Parse(dt.Rows[i]["ReEvaluation"].ToString());
                    TotGeneralEvaluation += int.Parse(dt.Rows[i]["GeneralEvaluation"].ToString());
                    TotFirstEvaluation += int.Parse(dt.Rows[i]["FirstEvaluation"].ToString());
                    TotSP2 += int.Parse(dt.Rows[i]["SP2"].ToString());
                    TotIPDA += int.Parse(dt.Rows[i]["IPDA"].ToString());
                    TotIPDB += int.Parse(dt.Rows[i]["IPDB"].ToString());
                    TotOtherSession += int.Parse(dt.Rows[i]["OtherSession"].ToString());

                }

                #region Total Calculation
                html.Append("<tr>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">Total</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\"></td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotOTPT + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotMATRIX + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotGroupSession + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotSpeechEvaluation + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotSpeechTherapy + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotReEvaluation + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotGeneralEvaluation + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotFirstEvaluation + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotSP2 + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotIPDA + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotIPDB + "</td>");
                html.Append("<td style=\"padding:10px;vertical-align:top;\">" + TotOtherSession + "</td>");
                html.Append("</tr>");
                #endregion
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename= " + centrename + " Patient Session List " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
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