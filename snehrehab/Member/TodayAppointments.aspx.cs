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

public partial class Member_TodayAppointments : System.Web.UI.Page
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
            LoadData();
        }
    }

    protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AppointmentGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = DateTime.UtcNow.AddMinutes(330).Date;
        DateTime _uptoDate = DateTime.UtcNow.AddMinutes(330).Date;

        SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
        AppointmentGV.DataSource = DB.MySearch(_loginID, "", _fromDate, _uptoDate);
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
}
