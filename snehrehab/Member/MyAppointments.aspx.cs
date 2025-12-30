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
using System.Globalization;
using System.Text;

public partial class Member_MyAppointments : System.Web.UI.Page
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
            txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
               1// DateTime.UtcNow.AddMinutes(330).Day
                ).ToString(DbHelper.Configuration.showDateFormat);
            txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            ddl_Session.Items.Clear(); ddl_Session.Items.Add(new ListItem("Select Session", "-1"));
            foreach (DataRow item in DB.fill_Session().Rows)
            {
                ddl_Session.Items.Add(new ListItem(item["SessionName"].ToString(), item["SessionID"].ToString()));
            }
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        AppointmentGV.PageIndex = 0; LoadData();
    }

    protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AppointmentGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
        int _SessionID = 0; if (ddl_Session.SelectedItem != null) { int.TryParse(ddl_Session.SelectedItem.Value, out _SessionID); }
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

        SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
        AppointmentGV.DataSource = DB.MySearchNew(_status, _SessionID, _loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate);
        AppointmentGV.DataBind();
        if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
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
        int appointmentstatusid = 0; int.TryParse(_appointmentStatus, out appointmentstatusid);
        if (appointmentstatusid == 0)
        {
            html.Append("<span class=\"label label-primary label-mini\">Pending</span>");
        }
        else if (appointmentstatusid == 1)
        {
            html.Append("<span class=\"label label-success label-mini\">Completed</span>");
        }
        else if (appointmentstatusid == 2)
        {
            html.Append("<span class=\"label label-important label-mini\">Absent</span>");
        }
        else if (appointmentstatusid == 10)
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
}
