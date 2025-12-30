using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

public partial class Member_Appointments : System.Web.UI.Page
{
    int _loginID = 0; bool isAdmin = false; bool isSuperAdmin = false;
    int PagingSize = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        isAdmin = SnehBLL.UserAccount_Bll.IsAdminOrReception();
        if (SnehBLL.UserAccount_Bll.getCategory() == 4 || SnehBLL.UserAccount_Bll.getCategory() == 5)
        {
            isSuperAdmin = true;
        }
        if (!IsPostBack)
        {
            if (!isSuperAdmin)
            {
                lblAddNew.Text = "<a href=\"/Member/Appointment.aspx\" class=\"btn btn-primary\">Add New</a> " +
                    "<a href=\"/Member/AptWaiting.aspx\" class=\"btn btn-primary\">Add Waiting</a> ";
            }
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            ddl_Session.Items.Clear(); ddl_Session.Items.Add(new ListItem("Select Session", "-1"));
            foreach (DataRow item in DB.fill_Session().Rows)
            {
                ddl_Session.Items.Add(new ListItem(item["SessionName"].ToString(), item["SessionID"].ToString()));
            }
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }
            txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
               1// DateTime.UtcNow.AddMinutes(330).Day
                ).ToString(DbHelper.Configuration.showDateFormat);
            txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            LoadData(1);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //AppointmentGV.PageIndex = 0; 
        LoadData(1);
    }

    //protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    AppointmentGV.PageIndex = e.NewPageIndex; LoadData(1);
    //}

    private void LoadData(int PageIndex)
    {
        int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
        int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
        int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        int _duration = 0; if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);

        SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll(); long TotalRecord = 0;
        DataTable dt = DB.SearchDur(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, _duration, PageIndex, out PagingSize, out TotalRecord);
        this.PopulatePager(TotalRecord, PageIndex);
        AppointmentGV.DataSource = dt;
        AppointmentGV.DataBind();
        if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        txtPage.Value = PageIndex.ToString();

    }

    private void PopulatePager(long TotalRecord, int PageIndex)
    {
        double dblPageCount = (double)((decimal)TotalRecord / Convert.ToDecimal(PagingSize));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 1)
        {
            pages.Add(new ListItem("«", "1", PageIndex > 1));

            int AFTER = pageCount - PageIndex; if (AFTER > 3) { AFTER = 3; }
            int BEFORE = 0; if (PageIndex > 3) { BEFORE = 3; } else { BEFORE = PageIndex - 1; }

            if (BEFORE < 3 && pageCount - PageIndex > 0)
            {
                AFTER = pageCount - PageIndex; if (AFTER > 5) { AFTER = 5; }
            }
            if (PageIndex > 1)
            {
                pages.Add(new ListItem("‹", (PageIndex - 1).ToString(), (PageIndex - 1) != PageIndex));
            }
            for (int i = BEFORE; i > 0; i--)
            {

                pages.Add(new ListItem((PageIndex - i).ToString(), (PageIndex - i).ToString(), (PageIndex - i) != PageIndex));
            }
            pages.Add(new ListItem((PageIndex).ToString(), (PageIndex).ToString(), false));
            for (int i = 1; i <= AFTER; i++)
            {
                pages.Add(new ListItem((PageIndex + i).ToString(), (PageIndex + i).ToString(), (PageIndex + i) != PageIndex));
            }
            if (PageIndex < pageCount)
            {
                pages.Add(new ListItem("›", (PageIndex + 1).ToString(), (PageIndex + 1) != PageIndex));
            }
            pages.Add(new ListItem("»", pageCount.ToString(), PageIndex < pageCount));
        }

        int StartIndex = ((PageIndex - 1) * PagingSize) + 1;

        rptPager.DataSource = pages;
        rptPager.DataBind();

        if (pages.Count > 1)
        {
            rptPager.Visible = true;
        }
        else
        {
            rptPager.Visible = false;
        }
    }

    protected void Page_Changed(object sender, EventArgs e)
    {
        int PAGE_INDEX = int.Parse((sender as LinkButton).CommandArgument);
        this.LoadData(PAGE_INDEX);
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    public string TIMEDURATION(string _durationStr, string _timeStr)
    {
        int _duration = 0; int.TryParse(_durationStr, out _duration);
        DateTime TimeHourD = new DateTime();
        DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
        if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
        {
            return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
        }
        return "- - -";
    }

    public string GETACTION(string _uniqueID, string _appointmentStatus)
    {
        StringBuilder html = new StringBuilder();
        int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
        if (_appointmentStatusID == 0)
        {
            if (!isSuperAdmin)
            {
                html.Append("<a href=\"/Member/AppointmentPay.aspx?record=" + _uniqueID + "\" class=\"btn-pay btn-success\">Pay</a>");
                //if()
                //{

                //}
                //html.Append("&nbsp;");
                html.Append("<a href=\"/Member/AppointmentCncl.aspx?record=" + _uniqueID + "\" class=\"btn-cancel btn-warning\">Cancel</a>");
                //html.Append("&nbsp;");
                html.Append("<a href=\"/Member/AppointmentAbst.aspx?record=" + _uniqueID + "\" class=\"btn-absent btn-danger\">Absent</a>");
                if (isAdmin)
                {
                    html.Append("<a href=\"/Member/AppointmentEdit.aspx?record=" + _uniqueID + "\" class=\"btn-pay btn-primary\">Edit</a>");
                }
            }
            else
            {
                html.Append("<span class=\"label label-primary label-mini\">Pending</span>");
            }
        }
        else if (_appointmentStatusID == 1)
        {
            html.Append("<span class=\"label label-success label-mini\">Completed</span>");
        }
        else if (_appointmentStatusID == 2)
        {
            html.Append("<span class=\"label label-important label-mini\">Absent</span>");
        }
        else if (_appointmentStatusID == 10)
        {
            html.Append("<span class=\"label label-warning label-mini\">Cancelled</span>");
        }
        else
        {
            html.Append("<span class=\"label label-info label-mini\">Unknown</span>");
        }
        return html.ToString();
    }

    protected void AppointmentGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField txtA = e.Row.FindControl("txtAppointmentID") as HiddenField;
            HiddenField txtS = e.Row.FindControl("txtAppointmentStatusID") as HiddenField;
            if (txtA != null && txtS != null)
            {
                if (DbHelper.Configuration.IsGuid(txtA.Value))
                {
                    int _appointmentStatusID = 0; int.TryParse(txtS.Value, out _appointmentStatusID);

                    if (_appointmentStatusID == 1)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-complete";

                    }
                    else if (_appointmentStatusID == 2)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-absent";
                    }
                    else if (_appointmentStatusID == 10)
                    {
                        e.Row.CssClass = e.Row.CssClass + "appointment-cancel";

                    }
                }
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void btnExport_Click_new(object sender, EventArgs e)
    {
        //AppointmentGV.AllowPaging = false;
        //LoadData(1);
        //Response.Clear();
        //Response.Buffer = true;
        //Response.ClearContent();
        //Response.ClearHeaders();
        //Response.Charset = "";
        //string FileName = "Appointment List Report" + " " + DateTime.Now + ".xls";
        //StringWriter strwritter = new StringWriter();
        //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = "application/vnd.ms-excel";
        //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //AppointmentGV.GridLines = GridLines.Both;
        //AppointmentGV.HeaderStyle.Font.Bold = true;
        //AppointmentGV.RenderControl(htmltextwrtter);
        //Response.Write(strwritter.ToString());
        //AppointmentGV.AllowPaging = true;
        //LoadData(1);
        //Response.End();

        //    //int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
        //    //int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
        //    //int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
        //    //DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        //    //DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        //    //int _duration = 0; if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);
        //    //SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
        //    //DataTable dt = DB.SearchDur(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, _duration);
        //    //if (dt.Rows.Count > 0)
        //    //{
        //    //    StringBuilder html = new StringBuilder();
        //    //    html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
        //    //    html.Append("<tr><td><b>Report Name:</b></td><td>Appointment List Report</td></tr>");
        //    //    html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
        //    //    html.Append("</table>");
        //    //    html.Append("<br/>");
        //    //    html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
        //    //    html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>SESSION</th><th>THERAPIST</th><th>DATE</th><th>DURATION</th><th>TIME</th><th>STATUS</th></tr>");
        //    //    for (int i = 0; i < dt.Rows.Count; i++)
        //    //    {
        //    //        html.Append("<tr " + GETCOLOR(dt.Rows[i]["AppointmentStatus"].ToString()) + ">");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + TIMEDURATION(dt.Rows[i]["Duration"].ToString(), dt.Rows[i]["AppointmentTime"].ToString()) + "</td>");
        //    //        html.Append("<td style=\"vertical-align:top;\">" + GETSTATUS(dt.Rows[i]["AppointmentStatus"].ToString()) + "</td>");
        //    //        html.Append("</tr>");
        //    //    }
        //    //    html.Append("</table>");
        //    //    Response.Clear();
        //    //    Response.AddHeader("Content-Disposition", "attachment;filename=appointment list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
        //    //    Response.ContentType = "application/vnd.xls";
        //    //    Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
        //    //    Response.Charset = "";
        //    //    Response.Output.Write(html.ToString());
        //    //    Response.End();
        //    //}
        //    //else
        //    //{
        //    //    DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
        //    //}
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        int PageIndex = 1; long TotalRecord = 0;
        int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
        int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
        int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        int _duration = 0; if (txtduraton.SelectedItem != null) int.TryParse(txtduraton.SelectedItem.Value, out _duration);

        SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
        DataTable dt = DB.SearchDur_Export(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, _duration, PageIndex, out PagingSize, out TotalRecord);
        double dblPageCount = (double)((decimal)TotalRecord / Convert.ToDecimal(PagingSize));
        int LastPage = (int)Math.Ceiling(dblPageCount);
        while (PageIndex < LastPage)
        {
            PageIndex++;
            DataTable dtN = DB.SearchDur_Export(_status, _SessionID, _doctorID, txtSearch.Text.Trim(), _fromDate, _uptoDate, _duration, PageIndex, out PagingSize, out TotalRecord);
            foreach (DataRow item in dtN.Rows)
            {
                dt.Rows.Add(item.ItemArray);
            }
        }
        if (dt.Rows.Count > 0)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=AppointmentList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
            Response.Charset = "";
            Response.Output.Write("<table border=\"1\" cellpadding=\"0\" style=\"font-family:Trebuchet MS;font-size:10pt;\">");
            Response.Output.Write("<thead><tr>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">SR.NO.</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">FULL NAME</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">SESSION</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">THERAPIST</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">DATE</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">DURATION</th>");
            Response.Output.Write("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">TIME</th>");
            Response.Output.Write("</tr>");
            Response.Output.Write("</thead><tbody>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Response.Output.Write("<tr>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["AppointmentDate"].ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
                Response.Output.Write("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["AppointmentTime"].ToString() + "</td>");
                Response.Output.Write("</tr>");
            }
            Response.Output.Write("</tbody></table>");

            //StringBuilder html = new StringBuilder("<table border=\"1\" cellpadding=\"0\" style=\"font-family:Trebuchet MS;font-size:10pt;\">");
            //html.Append("<thead><tr>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">SR.NO.</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">FULL NAME</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">SESSION</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">THERAPIST</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">DATE</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">DURATION</th>");
            //html.Append("<th style=\"background-color:#17375d;color:#FFF;padding:10px;text-align:left;\">TIME</th>");
            //html.Append("</tr>");
            //html.Append("</thead><tbody>");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    html.Append("<tr>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + (i + 1).ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["AppointmentDate"].ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
            //    html.Append("<td style=\"padding:10px;vertical-align:top;\">" + dt.Rows[i]["AppointmentTime"].ToString() + "</td>");
            //    html.Append("</tr>");
            //}
            //html.Append("</tbody></table>");           
            //Response.Output.Write(html.ToString());
            Response.End();
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
        }
    }

    private string GETSTATUS(string _appointmentStatus)
    {
        int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
        if (_appointmentStatusID == 0)
            return "Pending";
        else if (_appointmentStatusID == 1)
            return "Completed";
        else if (_appointmentStatusID == 2)
            return "Absent";
        else if (_appointmentStatusID == 10)
            return "Cancelled";
        return "Unknown";
    }

    private string GETCOLOR(string _appointmentStatus)
    {
        int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
        if (_appointmentStatusID == 0)
            return "";
        else if (_appointmentStatusID == 1)
            return "style=\"color: #3C8600;\"";
        else if (_appointmentStatusID == 2)
            return "style=\"color: #ff0024;\"";
        else if (_appointmentStatusID == 10)
            return "style=\"color: #ff8400;\"";
        return "Unknown";
    }
}