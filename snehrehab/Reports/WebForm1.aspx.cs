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
using System.Text;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace snehrehab.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
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
           // ReportGV.PageIndex = e.NewPageIndex;
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
            _f = FirstDayOfMonth(_fromDate);
            _l = LastDayOfMonth(_fromDate);
            #region  New Registration List
            DataTable dtRegistrationList = DB.RegistrationList(_f, _l);
            if (dtRegistrationList.Rows.Count > 0)
            {
                string[] x = new string[1];
                int[] y = new int[1];
                x[0] = "Status";
                y[0] = dtRegistrationList.Rows.Count;
                Chart1.Series[0].Points.DataBindXY(x, y);
                Chart1.Series[0].ChartType = SeriesChartType.Column;
                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;

                Chart1.ChartAreas[0].AxisY.Minimum = 0;
                Chart1.ChartAreas[0].AxisY.Interval = 10;

              
                Series S0 = Chart1.Series[0];
                S0.ChartType = SeriesChartType.Column;
                S0.IsValueShownAsLabel = true;
                Chart1.Series[0]["PixelPointWidth"] = "40";
                Chart1.Series[0]["DrawingStyle"] = "Cylinder";
            
                S0.LegendText = "";
                Chart1.BackColor = Color.White;
                Chart1.BackSecondaryColor = Color.LightSteelBlue;

                Chart1.BackGradientStyle = GradientStyle.DiagonalRight;
                
               
               // Chart1.Series[0].Points.AddXY(10, 10);

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Gainsboro;

                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Gainsboro;
               
            }
            #endregion

            #region  Total No. of Patient List
            DataTable dtPatientList = DB.PatientList(_f, _l);
            if (dtPatientList.Rows.Count > 0)
            {
                string[] x1 = new string[1];
                int[] y1 = new int[1];
                x1[0] = "Status";
                y1[0] = dtPatientList.Rows.Count;
                Chart2.Series[0].Points.DataBindXY(x1, y1);
                Chart2.Series[0].ChartType = SeriesChartType.Column;
                Chart2.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Chart2.ChartAreas[0].AxisY.Minimum = 0;
                Chart2.ChartAreas[0].AxisY.Interval = 100;
                Series S1 = Chart2.Series[0];
                S1.ChartType = SeriesChartType.Column;
                S1.IsValueShownAsLabel = true;
                Chart2.Series[0]["PixelPointWidth"] = "40";
                Chart2.Series[0]["DrawingStyle"] = "Cylinder";
                //S1.Color = Color.Transparent;
                //S1.LegendText = "";
                Chart2.BackColor = Color.White;
                Chart2.BackSecondaryColor = Color.Goldenrod;
                Chart2.BackGradientStyle = GradientStyle.DiagonalRight;
            }
            #endregion


            #region  AppointmentStatus_Pending
            DataTable dtAppointmentStatus_Pending = DB.AppointmentStatus_Pending(_f, _l);
            if (dtAppointmentStatus_Pending.Rows.Count > 0)
            {
                string[] x2 = new string[1];
                int[] y2= new int[1];
                x2[0] = "Status";
                y2[0] = dtAppointmentStatus_Pending.Rows.Count;
                Chart3.Series[0].Points.DataBindXY(x2, y2);
                Chart3.Series[0].ChartType = SeriesChartType.Column;
                Chart3.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Series S0 = Chart3.Series[0];
                S0.ChartType = SeriesChartType.Column;
                S0.IsValueShownAsLabel = true;
                Chart3.Series[0]["PixelPointWidth"] = "40";
                Chart3.Series[0]["DrawingStyle"] = "Cylinder";
                Chart3.ChartAreas[0].AxisY.Minimum = 0;
                Chart3.ChartAreas[0].AxisY.Interval = 50;

                Chart3.BackColor = Color.White;
                //#61b446
                Chart3.BackSecondaryColor = Color.Olive;
                Chart3.BackGradientStyle = GradientStyle.DiagonalRight;
            }
            #endregion


            #region  AppointmentStatus_Completed
            DataTable dtAppointmentStatus_Completed = DB.AppointmentStatus_Completed(_f, _l);
            if (dtAppointmentStatus_Completed.Rows.Count > 0)
            {
                string[] x3= new string[1];
                int[] y3 = new int[1];
                x3[0] = "Status";
                y3[0] = dtAppointmentStatus_Completed.Rows.Count;
                Chart3.Series[1].Points.DataBindXY(x3, y3);
               
                Chart3.Series[1].ChartType = SeriesChartType.Column;
                Chart3.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Series S0 = Chart3.Series[1];
                S0.ChartType = SeriesChartType.Column;
                S0.IsValueShownAsLabel = true;
                Chart3.Series[1]["PixelPointWidth"] = "40";
                Chart3.Series[1]["DrawingStyle"] = "Cylinder";
                Chart3.ChartAreas[0].AxisY.Minimum = 0;
                Chart3.ChartAreas[0].AxisY.Interval = 50;

                Chart3.BackColor = Color.White;
                Chart3.BackSecondaryColor = Color.Olive;
                Chart3.BackGradientStyle = GradientStyle.DiagonalRight;
            }
            #endregion


            #region  AppointmentStatus_Absent
            DataTable dtAppointmentStatus_Absent = DB.AppointmentStatus_Absent(_f, _l);
            if (dtAppointmentStatus_Absent.Rows.Count > 0)
            {
                string[] x4 = new string[1];
                int[] y4 = new int[1];
                x4[0] = "Status";
                y4[0] = dtAppointmentStatus_Absent.Rows.Count;
                Chart3.Series[2].Points.DataBindXY(x4, y4);
                Chart3.Series[2].ChartType = SeriesChartType.Column;
                Chart3.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Series S0 = Chart3.Series[2];
                S0.ChartType = SeriesChartType.Column;
                S0.IsValueShownAsLabel = true;
                Chart3.Series[2]["PixelPointWidth"] = "40";
                Chart3.Series[2]["DrawingStyle"] = "Cylinder";
                Chart3.ChartAreas[0].AxisY.Minimum = 0;
                Chart3.ChartAreas[0].AxisY.Interval = 50;

                Chart3.BackColor = Color.White;
                Chart3.BackSecondaryColor = Color.Olive;
                Chart3.BackGradientStyle = GradientStyle.DiagonalRight;
            }
            #endregion


            #region  AppointmentStatus_Cancelled
            DataTable dtAppointmentStatus_Cancelled = DB.AppointmentStatus_Cancelled(_f, _l);
            if (dtAppointmentStatus_Cancelled.Rows.Count > 0)
            {
                string[] x5 = new string[1];
                int[] y5 = new int[1];
                x5[0] = "Status";
                y5[0] = dtAppointmentStatus_Cancelled.Rows.Count;
                Chart3.Series[3].Points.DataBindXY(x5, y5);
                Chart3.Series[3].ChartType = SeriesChartType.Column;
                Chart3.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                Series S0 = Chart3.Series[3];
                S0.ChartType = SeriesChartType.Column;
                //StackedColumn
                S0.IsValueShownAsLabel = true;
                Chart3.Series[3]["PixelPointWidth"] = "40";
                Chart3.Series[3]["DrawingStyle"] = "Cylinder";
                Chart3.ChartAreas[0].AxisY.Minimum = 0;
                Chart3.ChartAreas[0].AxisY.Interval = 50;

                Chart3.BackColor = Color.White;
                Chart3.BackSecondaryColor = Color.Olive;
                Chart3.BackGradientStyle = GradientStyle.DiagonalRight;
            }
            #endregion
            #region pi chart
            string[] x10 = new string[4];
            decimal[] y10 = new decimal[4];
            decimal _Pending = 11;
            decimal _Completed = 70;
            decimal _Absent = 4;
            decimal _Cancelled = 15;
            x10[0] = "Pending";
            y10[0] = _Pending;
            x10[1] = "Completed";
            y10[1] = _Completed;
            x10[2] = "Absent";
            y10[2] = _Absent;
            x10[3] = "Cancelled";
            y10[3] = _Cancelled;

            Chart4.Series[0].Points.DataBindXY(x10, y10);
            Chart4.Series[0].ChartType = SeriesChartType.Pie;
            Chart4.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            //Chart4.Legends[0].Enabled = true;


            #endregion

        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            //DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            //SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            //DataSet ds = DB.PediatricFrequency(_fromDate, _uptoDate);
            //dt = ds.Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    StringBuilder html = new StringBuilder();
            //    html.Append("<table border=\"1\">");
            //    html.Append("<tr><th>SR NO</th><th>Name of Patient</th>");
            //    html.Append("<th>Year</th><th>Month</th><th>Frequency</th></tr>");
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        html.Append("<tr><td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");

            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AgeYear"].ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["AgeMonth"].ToString() + "</td>");
            //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PATIENTCOUNT"].ToString() + "</td>");
            //    }
            //    html.Append("<td></td></tr>");
            //    html.Append("</table>");
            //    Response.Clear();
            //    Response.AddHeader("Content-Disposition", "attachment;filename=patient account report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
            //    Response.ContentType = "application/vnd.xls";
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
            //    Response.Charset = "";
            //    Response.Output.Write(html.ToString());
            //    Response.End();
            //}
            //else
            //{
            //    DbHelper.Configuration.setAlert(Page, "No records found to export...", 2);
            //}
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
                DateTime d = new DateTime(_f.Year, _f.Month, k + 1);
                //if (d.DayOfWeek == DayOfWeek.Sunday)
                //{
                //    if (ReportGV.Rows.Count > k)
                //    {
                //        ReportGV.Rows[k].Cells[1].ForeColor = System.Drawing.Color.Red;

                //    }
                //}
                //else
                //{

                //}
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    e.Row.Attributes.Add("onMouseOver", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#f1f3f5';this.style.cursor='pointer';");
                //    e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor=this.originalstyle;");
                //}
            }
        }
    }
}
