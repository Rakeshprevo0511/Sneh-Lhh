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
using System.Globalization;

public partial class Member_LeaveAll : System.Web.UI.Page
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LeavesGV.PageIndex = 0; LoadData();
    }

    protected void LeavesGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LeavesGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

        SnehBLL.LeaveApplications_Bll DB = new SnehBLL.LeaveApplications_Bll();
        LeavesGV.DataSource = DB.Pending(txtEmployee.Text.Trim(), _fromDate, _uptoDate);
        LeavesGV.DataBind();
        if (LeavesGV.HeaderRow != null) { LeavesGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        //if (SnehBLL.UserAccount_Bll.getCategory() == 4)
        //{
        //    LeavesGV.Columns[8].Visible = false;
        //}
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    public string LEAVEFROM(string _sType, string _sFrom, string _sFromT)
    {
        int _leaveType = 0; int.TryParse(_sType, out _leaveType);
        if (_leaveType == 4)
        {
            DateTime _fromTime = new DateTime(); DateTime.TryParseExact(_sFrom, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromTime);
            DateTime TimeHourD = new DateTime(); DateTime.TryParseExact(_sFromT, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue && _fromTime > DateTime.MinValue)
            {
                return _fromTime.Add(TimeHourD.TimeOfDay).ToString("dd/MM/yyyy hh:mm tt");
            }
        }
        else
        {
            DateTime _fromTime = new DateTime(); DateTime.TryParseExact(_sFrom, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromTime);
            if (_fromTime > DateTime.MinValue)
            {
                return _fromTime.ToString(DbHelper.Configuration.showDateFormat);
            }
        }
        return "- - -";
    }

    public string LEAVEUPTO(string _sType, string _sFrom, string _sFromT)
    {
        int _leaveType = 0; int.TryParse(_sType, out _leaveType);
        if (_leaveType == 4)
        {
            DateTime _fromTime = new DateTime(); DateTime.TryParseExact(_sFrom, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromTime);
            DateTime TimeHourD = new DateTime(); DateTime.TryParseExact(_sFromT, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue && _fromTime > DateTime.MinValue)
            {
                return _fromTime.Add(TimeHourD.TimeOfDay).ToString("dd/MM/yyyy hh:mm tt");
            }
        }
        else
        {
            DateTime _fromTime = new DateTime(); DateTime.TryParseExact(_sFrom, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromTime);
            if (_fromTime > DateTime.MinValue)
            {
                return _fromTime.ToString(DbHelper.Configuration.showDateFormat);
            }
        }
        return "- - -";
    }
}