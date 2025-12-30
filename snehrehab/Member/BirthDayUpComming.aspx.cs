using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Text;

namespace snehrehab.Member
{
    public partial class BirthDayUpComming : System.Web.UI.Page
    {
        int _loginID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"].ToString().Length > 0)
                    {
                        if (Request.QueryString["tab"].ToString() == "patient")
                        {
                            tb_Contents.ActiveTabIndex = 0;
                        }
                        if (Request.QueryString["tab"].ToString() == "doctor")
                        {
                            tb_Contents.ActiveTabIndex = 1;
                        }
                        if (Request.QueryString["tab"].ToString() == "manager")
                        {
                            tb_Contents.ActiveTabIndex = 2;
                        }
                        if (Request.QueryString["tab"].ToString() == "reception")
                        {
                            tb_Contents.ActiveTabIndex = 3;
                        }
                    }
                }

                txtFrom.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);

                txtUpto.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                  DateTime.DaysInMonth(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month)
                    ).ToString(DbHelper.Configuration.showDateFormat);

                LoadForm();
            }
        }

        private void LoadForm()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date.", 2); return;
            }
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date.", 2); return;
            }
            DateTime StartDate = new DateTime(); StartDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year - 1 : _fromDate.Year), 4, 1);
            DateTime EndDate = new DateTime(); EndDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year : _fromDate.Year + 1), 3, 31);
            //if (_uptoDate < StartDate)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            //}
            //if (_uptoDate > EndDate)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            //}
            SqlCommand cmd = new SqlCommand("UpComingBirthDaxy"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            DbHelper.SqlDb db = new DbHelper.SqlDb();

            DataSet ds = db.DbFetch(cmd);
            if (ds.Tables.Count > 0)
                PatientGV.DataSource = ds.Tables[0];
            PatientGV.DataBind();
            if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            if (ds.Tables.Count > 1)
                DoctorGV.DataSource = ds.Tables[1];
            DoctorGV.DataBind();
            if (DoctorGV.HeaderRow != null) { DoctorGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            if (ds.Tables.Count > 2)
                ManagerGV.DataSource = ds.Tables[2];
            ManagerGV.DataBind();
            if (ManagerGV.HeaderRow != null) { ManagerGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            if (ds.Tables.Count > 3)
                ReceptionGV.DataSource = ds.Tables[3];
            ReceptionGV.DataBind();
            if (ReceptionGV.HeaderRow != null) { ReceptionGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PatientGV.PageIndex = 0; LoadForm();
        }
        protected void PatientGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PatientGV.PageIndex = e.NewPageIndex; LoadForm();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date.", 2); return;
            }
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date.", 2); return;
            }
            //DateTime StartDate = new DateTime(); StartDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year - 1 : _fromDate.Year), 4, 1);
            //DateTime EndDate = new DateTime(); EndDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year : _fromDate.Year + 1), 3, 31);
            //if (_uptoDate <= StartDate)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            //}
            //if (_uptoDate > EndDate)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            //}
            SqlCommand cmd = new SqlCommand("UpComingBirthDaxy"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            DbHelper.SqlDb db = new DbHelper.SqlDb(); // DataTable dt = db.DbRead(cmd);
            DataSet ds = db.DbFetch(cmd);           
            PatientGV.DataSource = ds.Tables[0];
            DoctorGV.DataSource = ds.Tables[1];
            ManagerGV.DataSource = ds.Tables[2];
            ReceptionGV.DataSource = ds.Tables[3];
            DataTable dt = ds.Tables[0];
            DataTable dtt = ds.Tables[1];
            DataTable dttt = ds.Tables[2];
            DataTable dts = ds.Tables[3];
         
            if (dt.Rows.Count > 0 || dtt.Rows.Count > 0 || dttt.Rows.Count > 0 || dts.Rows.Count >0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 BranchName FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPP = db.DbRead(cmd); if (dtPP.Rows.Count > 0) { centrename = dtPP.Rows[0]["BranchName"].ToString(); }
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>" + GetCentreName(centrename) + "</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<td style=\"vertical-align:top;\"><b>Patient Upcoming Birthday</b></td>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>BIRTHDATE</th><th>MOBILE NO.</th><th>MAIL ID</th><th>ADDRESS</th><th>REG. DATE</th>");
                string ReceiptPrefix = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtP = db.DbRead(cmd); if (dtP.Rows.Count > 0) { ReceiptPrefix = dtP.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalr = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["BirthDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MobileNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["rAddress"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["RegistrationDate"].ToString()) + "</td>");


                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                // Patient Export End //

                // Doctor Export Start//
                html.Append("<br/>");
                html.Append("<td style=\"vertical-align:top;\"><b>Doctor Upcoming Birthday</b></td>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>BIRTHDATE</th><th>MOBILE NO.</th><th>MAIL ID</th><th>ADDRESS</th><th>REG. DATE</th>");
                string ReceiptPrefixD = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPD = db.DbRead(cmd); if (dtPD.Rows.Count > 0) { ReceiptPrefixD = dtPD.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalD = dtt.Rows.Count;
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dtt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dtt.Rows[i]["BirthDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dtt.Rows[i]["MobileNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dtt.Rows[i]["MailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dtt.Rows[i]["ClinicAddress"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dtt.Rows[i]["JoinDate"].ToString()) + "</td>");


                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalD + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                // Doctor Export End //

                //Manager Export Start//
                html.Append("<br/>");
                html.Append("<td style=\"vertical-align:top;\"><b>Manager Upcoming Birthday</b></td>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>BIRTHDATE</th><th>MOBILE NO.</th><th>MAIL ID</th><th>ADDRESS</th><th>REG. DATE</th>");
                string ReceiptPrefixM = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPM = db.DbRead(cmd); if (dtPM.Rows.Count > 0) { ReceiptPrefixM = dtPM.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalM = dttt.Rows.Count;
                for (int i = 0; i < dttt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dttt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dttt.Rows[i]["BirthDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dttt.Rows[i]["MobileNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dttt.Rows[i]["MailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dttt.Rows[i]["ClinicAddress"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dttt.Rows[i]["JoinDate"].ToString()) + "</td>");


                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalM + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                //Manager Export End //

                // Receiption Export Start//
                html.Append("<br/>");
                html.Append("<td style=\"vertical-align:top;\"><b>Receiption Upcoming Birthday</b></td>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>BIRTHDATE</th><th>MOBILE NO.</th><th>MAIL ID</th><th>ADDRESS</th><th>REG. DATE</th>");
                string ReceiptPrefixR = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPR = db.DbRead(cmd); if (dtPR.Rows.Count > 0) { ReceiptPrefixR = dtPR.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalR = dts.Rows.Count;
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dts.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dts.Rows[i]["BirthDate"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dts.Rows[i]["ContactNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dts.Rows[i]["MailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dts.Rows[i]["Address"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dts.Rows[i]["JoinDate"].ToString()) + "</td>");


                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalR + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                //Receitpion Export End //

                //Response.AddHeader("Content-Disposition", "attachment;filename=print receipt report.xls");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + centrename + " Upcoming Birthday.xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No record found to export.", 3);
                return;
            }
        }
        public string GetCentreName(string centre)
        {
            if (!string.IsNullOrEmpty(centre))
            {
                return centre + " " + "UpComming BirthDay Report";
            }
            else
            {
                return "UpComming BirthDay Report";
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