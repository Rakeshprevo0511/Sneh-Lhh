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
using System.Drawing;
using System.Text;

namespace snehrehab.Reports
{
    public partial class DoctorSession : System.Web.UI.Page
    {
        DataTable dtnew = new DataTable();
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

        private static DateTime GetNextSunday(DateTime dt)
        {
            var tomorrow = dt.AddDays(1);
            if (tomorrow.DayOfWeek != DayOfWeek.Sunday)
            {
                GetNextSunday(tomorrow);
            }

            return tomorrow;
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex;
            LoadData();
        }
        public DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        public DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        DateTime _f = new DateTime();
        DateTime _l = new DateTime();

        private void LoadData()
        {
            DateTime _Date = new DateTime();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            //DateTime _f = FirstDayOfMonth(_fromDate);
            //DateTime _l = LastDayOfMonth(_fromDate);
            _f = FirstDayOfMonth(_fromDate);
            _l = LastDayOfMonth(_fromDate);
            DataTable dtDR = DB.MonthlyPatientList1(_f, _l);
            DataSet ds1 = DB.DoctorSession(_f, _l);
            DataTable dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                DataRow row;
                dtnew.Columns.Add("Date");
                for (int i = 0; i < dtDR.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dtDR.Rows[i]["FullName"].ToString(), typeof(decimal));
                }
                int _firstday = _f.Day;
                int _lastday = _l.Day;

                for (int k = _firstday; k < _lastday + 1; k++)
                {
                    DateTime _newdatetime = new DateTime(_f.Year, _f.Month, k);
                    row = dtnew.NewRow();

                    string p = "";
                    if (k.ToString().Length == 1)
                    {
                        p = ("0" + k).ToString();
                    }
                    else
                    {
                        p = k.ToString();
                    }
                    row["Date"] = _newdatetime.ToString(DbHelper.Configuration.showDateFormat);

                    for (int i = 0; i < dtDR.Rows.Count; i++)
                    {
                        string _DoctorID = dtDR.Rows[i]["DoctorID"].ToString();
                        DataTable dtsession = DB.MonthlyPatientList2(_newdatetime, Convert.ToInt32(_DoctorID), _f, _l);
                        if (dtsession.Rows.Count > 0)
                        {
                            int _session = 0;
                            for (int n = 0; n < dtsession.Rows.Count; n++)
                            {

                                string _Duration = dtsession.Rows[n]["Duration"].ToString();
                                if (_Duration == "30")
                                {
                                    _session = _session + 1;
                                }
                                else if (_Duration == "45")
                                {
                                    _session = _session + 1;
                                }
                                else
                                {
                                    _session = _session + 2;
                                }
                            }

                            row[i + 1] = _session;
                        }
                    }
                    dtnew.Rows.Add(row);
                }
            }
            if (dtnew.Rows.Count>0)
            {
                DataRow row1;
                row1 = dtnew.NewRow();
                row1[0] = "Total";
                for (int i = 1; i < dtnew.Columns.Count; i++)
                {
                    decimal _sesssion = 0;
                    decimal _Totalsesssion = 0;
                    for (int z = 0; z < dtnew.Rows.Count; z++)
                    {

                        decimal.TryParse(dtnew.Rows[z][i].ToString(), out _sesssion);
                        _Totalsesssion = _Totalsesssion + _sesssion;
                    }

                    row1[i] = _Totalsesssion;
                }
                dtnew.Rows.Add(row1);
            }
            ReportGV.DataSource = dtnew;
            ReportGV.AutoGenerateColumns = true;
            ReportGV.DataBind();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //DateTime _Date = new DateTime();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            _f = FirstDayOfMonth(_fromDate);
            _l = LastDayOfMonth(_fromDate);
            DataTable dtDR = DB.MonthlyPatientList1(_f, _l);
            DataSet ds1 = DB.DoctorSession(_f, _l);
            DataTable dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                DataRow row;
                dtnew.Columns.Add("Date");
                for (int i = 0; i < dtDR.Rows.Count; i++)
                {
                    dtnew.Columns.Add(dtDR.Rows[i]["FullName"].ToString(), typeof(decimal));
                }
                int _firstday = _f.Day;
                int _lastday = _l.Day;

                for (int k = _firstday; k < _lastday + 1; k++)
                {
                    DateTime _newdatetime = new DateTime(_f.Year, _f.Month, k);
                    row = dtnew.NewRow();

                    string p = "";
                    if (k.ToString().Length == 1)
                    {
                        p = ("0" + k).ToString();
                    }
                    else
                    {
                        p = k.ToString();
                    }
                    row["Date"] = _newdatetime.ToString(DbHelper.Configuration.showDateFormat);

                    for (int i = 0; i < dtDR.Rows.Count; i++)
                    {
                        string _DoctorID = dtDR.Rows[i]["DoctorID"].ToString();
                        DataTable dtsession = DB.MonthlyPatientList2(_newdatetime, Convert.ToInt32(_DoctorID), _f, _l);
                        if (dtsession.Rows.Count > 0)
                        {
                            int _session = 0;
                            for (int n = 0; n < dtsession.Rows.Count; n++)
                            {

                                string _Duration = dtsession.Rows[n]["Duration"].ToString();
                                if (_Duration == "30")
                                {
                                    _session = _session + 1;
                                }
                                else if (_Duration == "45")
                                {
                                    _session = _session + 1;
                                }
                                else
                                {
                                    _session = _session + 2;
                                }
                            }

                            row[i + 1] = _session;
                        }
                    }
                    dtnew.Rows.Add(row);
                }
            }
            if (dtnew.Rows.Count > 0)
            {
                DataRow row1;
                row1 = dtnew.NewRow();
                row1[0] = "Total";
                for (int i = 1; i < dtnew.Columns.Count; i++)
                {
                    decimal _sesssion = 0;
                    decimal _Totalsesssion = 0;
                    for (int z = 0; z < dtnew.Rows.Count; z++)
                    {

                        decimal.TryParse(dtnew.Rows[z][i].ToString(), out _sesssion);
                        _Totalsesssion = _Totalsesssion + _sesssion;
                    }

                    row1[i] = _Totalsesssion;
                }
                dtnew.Rows.Add(row1);
            }
            string csv = "";
            csv += "\r\n";
            for (int i = 0; i < dtnew.Columns.Count; i++)
            {
                csv += dtnew.Columns[i].ColumnName.ToString() + ',';
            }
            csv += "\r\n";
            for (int j = 0; j < dtnew.Rows.Count; j++)
            {
                for (int k = 0; k < dtnew.Columns.Count; k++)
                {
                    csv += dtnew.Rows[j][dtnew.Columns[k].ColumnName.ToString()].ToString().Replace(",", ";") + ',';
                }
                csv += "\r\n";
            }
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=Doctor Session-" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".csv");
            Response.ContentType = "application/octet-stream";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Charset = "";
            Response.Output.Write(csv);
            Response.End();
            if (dtnew.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export.", 2);
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
        protected void ReportGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime _Date = new DateTime();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _firstdate = FirstDayOfMonth(_fromDate);
            DateTime _lastdate = LastDayOfMonth(_fromDate);
            int _firstday = _firstdate.Day;
            int _lastday = _lastdate.Day;
            string[] _datemonth = _firstdate.ToString().Split(' ');
            string _onlydate = _datemonth[0];
            string[] _month = _onlydate.Split('/');
            string _monthyear = _month[1] + '/' + _month[2];
            for (int k = 0; k < _lastday; k++)
            // for (int k = 1; k < ReportGV.Rows.Count;k++ )
            {
               // DateTime d = new DateTime(Convert.ToInt32(_month[2]), Convert.ToInt32(_month[1]), k + 1);
                //Compare date with sunday
                DateTime d = new DateTime(_f.Year, _f.Month, k+1);
                if (d.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (ReportGV.Rows.Count > k)
                    {
                        ReportGV.Rows[k].Cells[1].ForeColor = System.Drawing.Color.Red;

                    }
                }
                else
                {

                }
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    e.Row.Attributes.Add("onMouseOver", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#f1f3f5';this.style.cursor='pointer';");
                //    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor=this.originalstyle;");
                //}
            }
        }
    }
}
