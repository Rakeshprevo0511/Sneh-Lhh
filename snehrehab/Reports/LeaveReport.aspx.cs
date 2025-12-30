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
using System.Drawing;

namespace snehrehab.Reports
{
    public partial class LeaveReport : System.Web.UI.Page
    {
        DataTable dtNew = new DataTable(); string seperator = "----------";
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
                txtOnMonthNew.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                     1// DateTime.UtcNow.AddMinutes(330).Day
                      ).ToString(DbHelper.Configuration.showMonthFormat);
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

        public DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        private void LoadData()
        {
            //DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            //DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtOnMonthNew.Text.Trim(), DbHelper.Configuration.showMonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _firstdate = FirstDayOfMonth(_fromDate); DateTime _lastdate = LastDayOfMonth(_fromDate);

            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
           // DataSet ds1 = DB.LeaveReport1(_fromDate, _uptoDate);
           // DataTable dt1 = ds1.Tables[0];
            DataTable dt1 = DB.LeaveReport1(_firstdate, _lastdate);
            if (dt1.Rows.Count > 0)
            {
                //dtnew.Columns.Add("Sr. No.");
                dtNew.Columns.Add("Full Name");
                for (int k = _firstdate.Day; k < _lastdate.Day + 1; k++)
                {
                    dtNew.Columns.Add(k.ToString());
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row = dtNew.NewRow();
                    int UserID = 0; int.TryParse(dt1.Rows[i]["UserID"].ToString(), out UserID);
                    string _doctorName = dt1.Rows[i]["FullName"].ToString();
                    DataTable dt = DB.LeaveReport(UserID, _firstdate, _lastdate);
                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    DateTime FromDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["FromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                    //    DateTime UptoDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["UptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out UptoDate);
                    //    if (FromDate > DateTime.MinValue && UptoDate > DateTime.MinValue)
                    //    {
                    //        int days = 0; int.TryParse(dt.Rows[j]["LeaveDays"].ToString(), out days);
                    //        if (days > 0)
                    //        {
                    //            //row["Sr. No."] = (i + 1);
                    //            row["Full Name"] = _doctorName;
                    //            for (int p = FromDate.Day; p < (FromDate.Day + days); p++)
                    //            {
                    //                row[p] = dt.Rows[j]["LeaveID"].ToString() + seperator + dt.Rows[j]["TypeName"].ToString();
                    //            }
                    //        }
                    //    }
                    //}
                    //dtNew.Rows.Add(row);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DateTime FromDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["FromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                        DateTime UptoDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["UptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out UptoDate);
                        if (FromDate > DateTime.MinValue && UptoDate > DateTime.MinValue)
                        {
                            int days = 0; int.TryParse(dt.Rows[j]["LeaveDays"].ToString(), out days);
                            if (days > 0)
                            {
                                row["Full Name"] = _doctorName;
                                DateTime TempDate = FromDate; int addedDay = 0;

                                while (TempDate <= UptoDate)
                                {

                                    if (TempDate.Month == _firstdate.Month)
                                    {
                                        if (TempDate.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            row[TempDate.Day] = dt.Rows[j]["LeaveID"].ToString() + seperator + dt.Rows[j]["TypeName"].ToString();
                                        }
                                        else
                                        {
                                            row[TempDate.Day] = "Sunday";
                                        }
                                    }

                                    TempDate = TempDate.AddDays(1); addedDay += 1;
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(row);
                }
            }

            //if (dt1.Rows.Count > 0)
            //{
            //    DataRow row;
            //    dtnew.Columns.Add("FullName");
            //    DateTime _firstdate = FirstDayOfMonth(_fromDate);
            //    DateTime _lastdate = LastDayOfMonth(_fromDate);
            //    int _firstday = _firstdate.Day;
            //    int _lastday = _lastdate.Day;

            //    for (int k = _firstday; k < _lastday + 1; k++)
            //    {
            //        dtnew.Columns.Add(k.ToString());
            //    }
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        row = dtnew.NewRow();
            //        int _doctorID = Convert.ToInt32(dt1.Rows[i]["DoctorID"].ToString());
            //        string _doctorName = dt1.Rows[i]["FullName"].ToString();
               //     DataSet ds = DB.LeaveReport(_doctorID, _fromDate, _uptoDate);
            //        dt = ds.Tables[0];






            //        for (int j = 0; j < dt.Rows.Count; j++)
            //        {
            //            DateTime FromDate = DateTime.MinValue;
            //            DateTime UptoDate = DateTime.MinValue;
            //            FromDate = Convert.ToDateTime(dt.Rows[j]["FromDate"].ToString());
            //            UptoDate = Convert.ToDateTime(dt.Rows[j]["UptoDate"].ToString());
            //            int _datestart = 0;
            //            _datestart = FromDate.Day;
            //            TimeSpan _timeinterval = UptoDate - FromDate;
            //            if (_timeinterval.Days > 0)
            //            {
            //                int _finaldate = 0;
            //                _finaldate = _datestart + _timeinterval.Days;
            //                for (int p = _datestart; p < _finaldate + 1; p++)
            //                {

            //                    row["FullName"] = _doctorName;
            //                    row[p] = dt.Rows[j]["TypeName"].ToString();
            //                }
            //            }
            //        }
            //        dtnew.Rows.Add(row);
            //    }
            //}
            ReportGV.DataSource = dtNew;
            ReportGV.AutoGenerateColumns = true;
            ReportGV.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtOnMonthNew.Text.Trim(), DbHelper.Configuration.showMonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _firstdate = FirstDayOfMonth(_fromDate); DateTime _lastdate = LastDayOfMonth(_fromDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataTable dt1 = DB.LeaveReport1(_firstdate, _lastdate);
            if (dt1.Rows.Count > 0)
            {
                DataTable dtnew = new DataTable();
                dtnew.Columns.Add("Full Name");
                for (int k = _firstdate.Day; k < _lastdate.Day + 1; k++)
                {
                    dtnew.Columns.Add(k.ToString());
                }
                //for (int i = 0; i < dt1.Rows.Count; i++)
                //{
                //    DataRow row = dtnew.NewRow();
                //    int UserID = 0; int.TryParse(dt1.Rows[i]["UserID"].ToString(), out UserID);
                //    string _doctorName = dt1.Rows[i]["FullName"].ToString();
                //    DataTable dt = DB.LeaveReport(UserID, _firstdate, _lastdate);
                //    for (int j = 0; j < dt.Rows.Count; j++)
                //    {
                //        DateTime FromDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["FromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                //        DateTime UptoDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["UptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out UptoDate);
                //        if (FromDate > DateTime.MinValue && UptoDate > DateTime.MinValue)
                //        {
                //            int days = 0; int.TryParse(dt.Rows[j]["LeaveDays"].ToString(), out days);
                //            if (days > 0)
                //            {
                //                row["Full Name"] = _doctorName;
                //                for (int p = FromDate.Day; p < (FromDate.Day + days); p++)
                //                {
                //                    row[p] = dt.Rows[j]["LeaveID"].ToString() + seperator + dt.Rows[j]["TypeName"].ToString();
                //                }
                //            }
                //        }
                //    }
                //    dtnew.Rows.Add(row);
                //}
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row = dtnew.NewRow();
                    int UserID = 0; int.TryParse(dt1.Rows[i]["UserID"].ToString(), out UserID);
                    string _doctorName = dt1.Rows[i]["FullName"].ToString();
                    DataTable dt = DB.LeaveReport(UserID, _firstdate, _lastdate);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DateTime FromDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["FromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                        DateTime UptoDate = new DateTime(); DateTime.TryParseExact(dt.Rows[j]["UptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out UptoDate);
                        if (FromDate > DateTime.MinValue && UptoDate > DateTime.MinValue)
                        {
                            int days = 0; int.TryParse(dt.Rows[j]["LeaveDays"].ToString(), out days);
                            if (days > 0)
                            {
                                row["Full Name"] = _doctorName;
                                DateTime TempDate = FromDate; int addedDay = 0;
                                while (TempDate <= UptoDate)
                                {
                                    if (TempDate.Month == _firstdate.Month)
                                    {
                                        if (TempDate.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            row[TempDate.Day] = dt.Rows[j]["LeaveID"].ToString() + seperator + dt.Rows[j]["TypeName"].ToString();
                                        }
                                        else
                                        {
                                        }
                                    }

                                    TempDate = TempDate.AddDays(1); addedDay += 1;
                                }
                            }
                        }
                    }
                    dtnew.Rows.Add(row);
                }
                if (dtnew.Rows.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    html.Append("<h6 style=\"text-decoration:underline;text-transform: uppercase;font-size:12pt;\">Leave Report : " + _fromDate.ToString("MMM-yyyy") + ":-</h6>");
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    html.Append("<thead><tr><th>Sr No</th>");
                    for (int dc = 0; dc < dtnew.Columns.Count; dc++)
                    {
                        html.Append("<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dtnew.Columns[dc].ColumnName + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>");
                    }
                    html.Append("</tr></thead>");
                    html.Append("<tbody>");
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        html.Append("<tr>");
                        html.Append("<td>" + (j + 1).ToString() + "</td>");
                        html.Append("<td>" + dtnew.Rows[j]["Full Name"].ToString() + "</td>");
                        for (int ec = 1; ec < dtnew.Columns.Count; ec++)
                        {
                            if (dtnew.Rows[j][ec].ToString().Length > 0)
                            {
                                string[] text = dtnew.Rows[j][ec].ToString().Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
                                if (text.Length > 1)
                                {
                                    string id = text[0]; string val = text[1];
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        int colspan = 1;
                                        #region
                                        for (int cn = (ec + 1); cn < dtnew.Columns.Count; cn++)
                                        {
                                            if (!string.IsNullOrEmpty(dtnew.Rows[j][cn].ToString()))
                                            {
                                                string[] textn = dtnew.Rows[j][cn].ToString().Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
                                                if (textn.Length > 1)
                                                {
                                                    string idn = textn[0];
                                                    if (idn.Equals(id))
                                                    {
                                                        colspan++;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        #endregion
                                        setColor(ref html, val, colspan);
                                        if (colspan > 1)
                                        {
                                            ec = ec + (colspan - 1);
                                        }
                                    }
                                    else
                                    {
                                        html.Append("<td></td>");
                                    }
                                }
                                else
                                {
                                    html.Append("<td></td>");
                                }
                            }
                            else
                            {
                                html.Append("<td></td>");
                            }
                        }
                        html.Append("</tr>");
                    }
                    html.Append("</tbody>");
                    html.Append("</table>");
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=Leave Report - " + _fromDate.ToString("MMM-yyyy") + ".xls");
                    Response.ContentType = "application/vnd.xls";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                    Response.Charset = "";
                    Response.Output.Write(html.ToString());
                    Response.End();
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "No records found to export.", 2);
                }
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
            ////if (e.Row.RowType == DataControlRowType.DataRow)
            ////{
            ////TableCell cell = e.Row.Cells[1];
            ////int quantity = 0;//int.Parse(cell.Text);
            ////int.TryParse(cell.Text, out quantity);
            ////if (quantity == 0)
            ////{
            ////    cell.BackColor = Color.Red;
            ////}
            ////if (quantity == 1)
            ////{
            ////    cell.BackColor = Color.Yellow;
            ////}
            ////if (quantity == 2)
            ////{
            ////    cell.BackColor = Color.Orange;
            ////}
            ////if (quantity == 3)
            ////{
            ////    cell.BackColor = Color.Brown;
            ////}

            //int _rowscount = dtNew.Rows.Count;
            //int _columnscount = dtNew.Columns.Count;

            //int rowscount = ReportGV.Rows.Count;
            //int columnscount = ReportGV.Columns.Count;
            //for (int i = 0; i < _rowscount; i++)
            //{
            //    for (int j =2; j < _columnscount; j++)
            //    {

            //        if (e.Row.RowType == DataControlRowType.DataRow)
            //        {
            //            //Colouring GridView CELL based on condition

            //            if (e.Row.Cells[j].Text == "Casual Leaves" || e.Row.Cells[j].Text == "Emergency Leaves" || e.Row.Cells[j].Text == "Half Day Leaves" || e.Row.Cells[j].Text == "Sick Leaves")
            //            {

            //                if (e.Row.Cells[j].Text == "Casual Leaves")
            //                {
            //                    e.Row.Cells[j].BackColor = System.Drawing.Color.YellowGreen;
            //                    e.Row.ForeColor = System.Drawing.Color.White;

            //                }
            //                else if (e.Row.Cells[j].Text == "Emergency Leaves")
            //                {
            //                    e.Row.Cells[j].BackColor = System.Drawing.Color.BlueViolet;

            //                    e.Row.ForeColor = System.Drawing.Color.White;
            //                }
            //                else if (e.Row.Cells[j].Text == "Half Day Leaves")
            //                {
            //                    e.Row.Cells[j].BackColor = System.Drawing.Color.Black;
            //                    e.Row.ForeColor = System.Drawing.Color.White;
            //                }

            //                //Colouring GridView Row based on condition
            //                else if (e.Row.Cells[j].Text == "Sick Leaves")
            //                {
            //                    e.Row.BackColor = System.Drawing.Color.Red;
            //                    e.Row.ForeColor = System.Drawing.Color.White;
            //                }

            //              //Colouring GridView Column based on condition
            //                else if (e.Row.Cells[j].Text == "")
            //                    ReportGV.Columns[j].ItemStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            //                // }
            //            }
            //            else
            //            {
            //                //ReportGV.Columns[j].ItemStyle.BackColor = System.Drawing.Color.AntiqueWhite;
            //            }
            //        }
            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int c = 2; c <= dtNew.Columns.Count; c++)
                {
                    if (e.Row.Cells.Count > c && !string.IsNullOrEmpty(e.Row.Cells[c].Text))
                    {
                        string[] text = e.Row.Cells[c].Text.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
                        if (text.Length > 1)
                        {
                            string id = text[0]; string val = text[1];
                            if (!string.IsNullOrEmpty(val))
                            {
                                int colspan = 1;
                                #region
                                for (int cn = (c + 1); cn < dtNew.Columns.Count; cn++)
                                {
                                    if (!string.IsNullOrEmpty(e.Row.Cells[cn].Text))
                                    {
                                        string[] textn = e.Row.Cells[cn].Text.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
                                        if (textn.Length > 1)
                                        {
                                            string idn = textn[0];
                                            if (idn.Equals(id))
                                            {
                                                colspan++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                #endregion
                                if (colspan > 1)
                                {
                                    e.Row.Cells[c].Text = val;
                                    e.Row.Cells[c].ColumnSpan = colspan;
                                    for (int r = (c + 1); r < (c + colspan); r++)
                                    {
                                        e.Row.Cells.RemoveAt(c + 1);
                                    }
                                    setColor(e.Row.Cells[c]);
                                }
                                else
                                {
                                    e.Row.Cells[c].Text = val;
                                    setColor(e.Row.Cells[c]);
                                }
                            }
                        }
                        else
                        {
                            string textsunday = e.Row.Cells[c].Text;
                            e.Row.Cells[c].Text = textsunday;
                            setColor(e.Row.Cells[c]);
                        }
                    }
                }
            }

        }

        private void setColor(TableCell tableCell)
        {
            if (tableCell.Text == "Casual Leaves")
            {
                tableCell.BackColor = System.Drawing.Color.YellowGreen;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
            else if (tableCell.Text == "Emergency Leaves")
            {
                tableCell.BackColor = System.Drawing.Color.Brown;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
            else if (tableCell.Text == "Half Day Leaves")
            {
                tableCell.BackColor = System.Drawing.Color.Black;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
            else if (tableCell.Text == "Sick Leaves")
            {
                tableCell.BackColor = System.Drawing.Color.Red;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
            else if (tableCell.Text == "Public Holiday")
            {
                tableCell.BackColor = System.Drawing.Color.Red;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
            else if (tableCell.Text == "Sunday")
            {
                tableCell.BackColor = System.Drawing.Color.Red;
                tableCell.ForeColor = System.Drawing.Color.White;
            }
        }

        private void setColor(ref StringBuilder html, string val, int colspan)
        {
            if (val == "Casual Leaves")
            {
                html.Append("<td " + (colspan > 1 ? "colspan=\"" + colspan.ToString() + "\"" : "") + " style=\"color:#FFF;background-color:#9acd32\">" + val + "</td>");
            }
            else if (val == "Emergency Leaves")
            {
                html.Append("<td " + (colspan > 1 ? "colspan=\"" + colspan.ToString() + "\"" : "") + " style=\"color:#FFF;background-color:#a52a2a\">" + val + "</td>");
            }
            else if (val == "Half Day Leaves")
            {
                html.Append("<td " + (colspan > 1 ? "colspan=\"" + colspan.ToString() + "\"" : "") + " style=\"color:#FFF;background-color:#000000\">" + val + "</td>");
            }
            else if (val == "Sick Leaves")
            {
                html.Append("<td " + (colspan > 1 ? "colspan=\"" + colspan.ToString() + "\"" : "") + " style=\"color:#FFF;background-color:#ff0000\">" + val + "</td>");
            }
            else if (val == "Public Holiday")
            {
                html.Append("<td " + (colspan > 1 ? "colspan=\"" + colspan.ToString() + "\"" : "") + " style=\"color:#FFF;background-color:#ff0000\">" + val + "</td>");
            }
            else
            {
                html.Append("<td>" + val + "</td>");
            }
        }
    }
}
