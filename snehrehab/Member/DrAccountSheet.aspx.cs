using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text;

namespace snehrehab.Member
{
    public partial class DrAccountSheet : System.Web.UI.Page
    {
        int _loginID = 0; DataSet ds = new DataSet(); DataSet dsA = new DataSet();
        int _doctorID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID < 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL),true);
            }
            if (!IsPostBack)
            {
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month, 1).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
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
            SnehBLL.Reports_Bll RB = new SnehBLL.Reports_Bll();
            _doctorID = RB.GetDoctorID(_loginID);
            ds = RB.AccountSheet(_doctorID, _fromDate, _uptoDate);
            dsA = RB.AdultAccountSheet(_doctorID, _fromDate, _uptoDate);
            DataTable dtnew = RB.Other_Act_AccountSheet(_doctorID, _fromDate, _uptoDate);
            if (ds.Tables.Count > 1 || dsA.Tables.Count > 1 || dtnew.Rows.Count > 0)
            {
                bool hasData1 = false; bool hasData2 = false; bool hasData3 = false;
                StringBuilder html = new StringBuilder(); DataTable dt = ds.Tables[0];
                //html.Append("<table><tr><td>Name:</td><td>" + dt.Rows[0]["FullName"].ToString() + "</td><td></td><td>Branch:</td><td>" + dt.Rows[0]["BranchName"].ToString() + "</td></tr>");
                //html.Append("<tr><td>Report Date:</td><td colspan=\"4\">" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + "-" + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr></table>");
                if (ds.Tables.Count > 1)
                {
                    int _recFrm = 1;
                    for (int i = _recFrm; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].Rows.Count > 0)
                        {
                            if (!hasData1) hasData1 = true; break;
                        }
                    }
                    if (hasData1)
                    {
                        html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\"><tbody><tr><td><h5 style=\"margin: 0;\">All Account Sheet</h5></td></tr>");
                        html.Append("<tr><td>");
                        for (int i = _recFrm; i < ds.Tables.Count; i++)
                        {
                            dt = ds.Tables[i];
                            if (dt.Rows.Count > 0)
                            {
                                html.Append("<h6 style=\"font-style: italic;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                                html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                                html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>PATIENT TYPE</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>DURATION</th><th>HOS. AMT</th></tr></thead>");//<th>DR. AMT</th>
                                html.Append("<tbody>");
                                decimal _amount = 0;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>" + (j + 1).ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["PatientType"].ToString() + "</td>");
                                    html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                                    html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                                    //html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                                    float _per80 = 0; float.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _per80);
                                    _per80 = (_per80 * 80 / 100);
                                    decimal _temp = 0; decimal.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _temp);
                                    _amount += _temp;
                                    //html.Append("<td>" + _per80.ToString() + "</td>");
                                    html.Append("</tr>");
                                }
                                html.Append("<tr><td colspan=\"6\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                for (int k = (i + 1); k < ds.Tables.Count; k++)
                                {
                                    if (ds.Tables[k].Rows.Count > 0)
                                    {
                                        html.Append("<hr/>"); break;
                                    }
                                }

                            }
                        }
                        html.Append("</td></tr></tbody></table>");
                    }
                }
                if (dsA.Tables.Count > 1)
                {
                    int _recFrmA = 1;
                    for (int i = _recFrmA; i < dsA.Tables.Count; i++)
                    {
                        if (dsA.Tables[i].Rows.Count > 0)
                        {
                            if (!hasData2) hasData2 = true; break;
                        }
                    }
                    if (hasData2)
                    {
                        html.Append("<br/><table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\"><tbody><tr><td><h5>Adult Account Sheet</h5></td></tr>");
                        html.Append("<tr><td>");
                        for (int i = _recFrmA; i < dsA.Tables.Count; i++)
                        {
                            dt = dsA.Tables[i];
                            if (dt.Rows.Count > 0)
                            {
                                html.Append("<h6 style=\"font-style: italic;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                                html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                                html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>HOS. AMT</th></tr></thead>");//<th>DR. AMT</th>
                                html.Append("<tbody>");
                                decimal _amount = 0;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>" + (j + 1).ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                                    html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                                    //html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                                    //html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                                    float _per80 = 0; float.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _per80);
                                    _per80 = (_per80 * 80 / 100);
                                    decimal _temp = 0; decimal.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _temp);
                                    _amount += _temp;
                                    //html.Append("<td>" + _per80.ToString() + "</td>");
                                    html.Append("</tr>");
                                }
                                html.Append("<tr><td colspan=\"5\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                for (int k = (i + 1); k < dsA.Tables.Count; k++)
                                {
                                    if (dsA.Tables[k].Rows.Count > 0)
                                    {
                                        html.Append("<hr/>"); break;
                                    }
                                }
                            }
                        }
                        html.Append("</td></tr></tbody></table>");
                    }
                }
                if (dtnew.Rows.Count > 0)
                {
                    hasData3 = true;
                    html.Append("<h6 style=\"font-style: italic;font-weight:500;\">Other Activity Cash Entries:-</h6>");
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT NAME</th><th>DATE</th><th>DOCTOR</th><th>ASSISTANT DOCTOR</th><th>DEBIT AMOUNT</th></tr></thead>");//<th>DR. AMT</th>
                    html.Append("<tbody>"); decimal _amount = 0;
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        if (dtnew.Rows[i]["ASS_DOCTOR"].ToString() != null && dtnew.Rows[i]["ASS_DOCTOR"].ToString() != "")
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["AccountName"] + "</td>");
                            html.Append("<td>" + FORMATDATE(dtnew.Rows[i]["PayDate"].ToString()) + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Doctor"] + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Ass_Doctor"] + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["DebitAmt"] + "</td>");
                            decimal _temp = 0;
                            decimal.TryParse(dtnew.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        else
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["AccountName"] + "</td>");
                            html.Append("<td>" + FORMATDATE(dtnew.Rows[i]["PayDate"].ToString()) + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Doctor"] + "</td>");
                            html.Append("<td></td>");
                            html.Append("<td>" + dtnew.Rows[i]["DebitAmt"] + "</td>");
                            decimal _temp = 0;
                            decimal.TryParse(dtnew.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                    }
                    html.Append("<tr><td colspan=\"5\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                    html.Append("</td></tr></tbody></table>");
                }
                if (hasData1 || hasData2 || hasData3)
                {
                    txtContent.Text = html.ToString();
                }
                else
                {
                    txtContent.Text = "<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\"><tbody><tr><td>No records found...</td></tr></tbody></table>";
                    DbHelper.Configuration.setAlert(Page, "No records found...", 3);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found...", 3);
            }

        }

        private string TIMEDURATION(string _duration, string _time)
        {
            int duration = 0; int.TryParse(_duration, out duration);
            DateTime time = new DateTime(); DateTime.TryParseExact(_time, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out time);
            if (time > DateTime.MinValue && time < DateTime.MaxValue)
            {
                return time.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + time.AddMinutes(duration).ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "---";
        }

        public string FORMATDATE(string str)
        {
            DateTime appointmentdate = new DateTime(); DateTime.TryParseExact(str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out appointmentdate);
            if (appointmentdate > DateTime.MinValue)
                return appointmentdate.ToString(DbHelper.Configuration.showDateFormat);
            return "---";
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
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
            SnehBLL.Reports_Bll RB = new SnehBLL.Reports_Bll();
            _doctorID = RB.GetDoctorID(_loginID);
            ds = RB.AccountSheet(_doctorID, _fromDate, _uptoDate);
            dsA = RB.AdultAccountSheet(_doctorID, _fromDate, _uptoDate);
            DataTable dtnew = RB.Other_Act_AccountSheet(_doctorID, _fromDate, _uptoDate);
            if (ds.Tables.Count > 1 || dsA.Tables.Count > 1 || dtnew.Rows.Count > 0)
            {
                bool hasData1 = false; bool hasData2 = false; bool hasData3 = false;
                StringBuilder html = new StringBuilder(); DataTable dt = ds.Tables[0];
                //html.Append("<table><tr><td>Name:</td><td>" + dt.Rows[0]["FullName"].ToString() + "</td><td></td><td>Branch:</td><td>" + dt.Rows[0]["BranchName"].ToString() + "</td></tr>");
                //html.Append("<tr><td>Report Date:</td><td colspan=\"4\">" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + "-" + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr></table>");
                if (ds.Tables.Count > 1)
                {
                    int _recFrm = 1;
                    for (int i = _recFrm; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].Rows.Count > 0)
                        {
                            if (!hasData1) hasData1 = true; break;
                        }
                    }
                    if (hasData1)
                    {
                        html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\"><tbody><tr><td><h5 style=\"margin: 0;\">All Account Sheet</h5></td></tr>");
                        html.Append("<tr><td>");
                        for (int i = _recFrm; i < ds.Tables.Count; i++)
                        {
                            dt = ds.Tables[i];
                            if (dt.Rows.Count > 0)
                            {
                                html.Append("<h6 style=\"font-style: italic;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                                html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                                html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>PATIENT TYPE</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>DURATION</th><th>HOS. AMT</th></tr></thead>");//<th>DR. AMT</th>
                                html.Append("<tbody>");
                                decimal _amount = 0;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>" + (j + 1).ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["PatientType"].ToString() + "</td>");
                                    html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                                    html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                                    //html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                                    float _per80 = 0; float.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _per80);
                                    _per80 = (_per80 * 80 / 100);
                                    decimal _temp = 0; decimal.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _temp);
                                    _amount += _temp;
                                    //html.Append("<td>" + _per80.ToString() + "</td>");
                                    html.Append("</tr>");
                                }
                                html.Append("<tr><td colspan=\"6\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                for (int k = (i + 1); k < ds.Tables.Count; k++)
                                {
                                    if (ds.Tables[k].Rows.Count > 0)
                                    {
                                        html.Append("<hr/>"); break;
                                    }
                                }

                            }
                        }
                        html.Append("</td></tr></tbody></table>");
                    }
                }
                if (dsA.Tables.Count > 1)
                {
                    int _recFrmA = 1;
                    for (int i = _recFrmA; i < dsA.Tables.Count; i++)
                    {
                        if (dsA.Tables[i].Rows.Count > 0)
                        {
                            if (!hasData2) hasData2 = true; break;
                        }
                    }
                    if (hasData2)
                    {
                        html.Append("<br/><table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\"><tbody><tr><td><h5>Adult Account Sheet</h5></td></tr>");
                        html.Append("<tr><td>");
                        for (int i = _recFrmA; i < dsA.Tables.Count; i++)
                        {
                            dt = dsA.Tables[i];
                            if (dt.Rows.Count > 0)
                            {
                                html.Append("<h6 style=\"font-style: italic;\">" + dt.Rows[0]["SessionName"].ToString().Trim() + ":-</h6>");

                                html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                                html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>SESSION DATE</th><th>TIME IN MINUTE</th><th>HOS. AMT</th></tr></thead>");//<th>DR. AMT</th>
                                html.Append("<tbody>");
                                decimal _amount = 0;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>" + (j + 1).ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["FullName"].ToString() + "</td>");
                                    html.Append("<td>" + FORMATDATE(dt.Rows[j]["AppointmentDate"].ToString()) + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["Duration"].ToString() + "</td>");
                                    //html.Append("<td>" + TIMEDURATION(dt.Rows[j]["Duration"].ToString(), dt.Rows[j]["AppointmentTime"].ToString()) + "</td>");
                                    //html.Append("<td>" + dt.Rows[j]["DebitAmt"].ToString() + "</td>");
                                    html.Append("<td>" + dt.Rows[j]["AppointmentAmt"].ToString() + "</td>");
                                    float _per80 = 0; float.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _per80);
                                    _per80 = (_per80 * 80 / 100);
                                    decimal _temp = 0; decimal.TryParse(dt.Rows[j]["AppointmentAmt"].ToString(), out _temp);
                                    _amount += _temp;
                                    //html.Append("<td>" + _per80.ToString() + "</td>");
                                    html.Append("</tr>");
                                }
                                html.Append("<tr><td colspan=\"5\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                for (int k = (i + 1); k < dsA.Tables.Count; k++)
                                {
                                    if (dsA.Tables[k].Rows.Count > 0)
                                    {
                                        html.Append("<hr/>"); break;
                                    }
                                }
                            }
                        }
                        html.Append("</td></tr></tbody></table>");
                    }
                }
                if (dtnew.Rows.Count > 0)
                {
                    hasData3 = true;
                    html.Append("<h6 style=\"font-style: italic;font-weight:500;\">Other Activity Cash Entries:-</h6>");
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT NAME</th><th>DATE</th><th>DOCTOR</th><th>ASSISTANT DOCTOR</th><th>DEBIT AMOUNT</th></tr></thead>");//<th>DR. AMT</th>
                    html.Append("<tbody>"); decimal _amount = 0;
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        if (dtnew.Rows[i]["ASS_DOCTOR"].ToString() != null && dtnew.Rows[i]["ASS_DOCTOR"].ToString() != "")
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["AccountName"] + "</td>");
                            html.Append("<td>" + FORMATDATE(dtnew.Rows[i]["PayDate"].ToString()) + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Doctor"] + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Ass_Doctor"] + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["DebitAmt"] + "</td>");
                            decimal _temp = 0;
                            decimal.TryParse(dtnew.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        else
                        {
                            html.Append("<tr>");
                            html.Append("<td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["AccountName"] + "</td>");
                            html.Append("<td>" + FORMATDATE(dtnew.Rows[i]["PayDate"].ToString()) + "</td>");
                            html.Append("<td>" + dtnew.Rows[i]["Doctor"] + "</td>");
                            html.Append("<td></td>");
                            html.Append("<td>" + dtnew.Rows[i]["DebitAmt"] + "</td>");
                            decimal _temp = 0;
                            decimal.TryParse(dtnew.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                    }
                    html.Append("<tr><td colspan=\"5\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                    html.Append("</td></tr></tbody></table>");
                }
                if (hasData1 || hasData2 || hasData3)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=monthly account report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                    Response.ContentType = "application/vnd.xls";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                    Response.Charset = "";
                    Response.Output.Write(html.ToString());
                    Response.End();
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "No records found...", 3);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found...", 3);
            }
        }
    }
}