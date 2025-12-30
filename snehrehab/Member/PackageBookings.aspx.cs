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

public partial class Member_PackageBookings : System.Web.UI.Page
{
    int _loginID = 0; bool isSuperAdmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (SnehBLL.UserAccount_Bll.getCategory() == 4)
        {
            isSuperAdmin = true;
        }
        if (!IsPostBack)
        {
            if (!isSuperAdmin)
            {               
                lblAddNew.Text = "<a href=\"/Member/PackageBooking.aspx\" class=\"btn btn-primary\">Add New</a>";
            }
            txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
               1// DateTime.UtcNow.AddMinutes(330).Day
                ).ToString(DbHelper.Configuration.showDateFormat);
            txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BookingGV.PageIndex = 0; LoadData();
    }

    protected void BookingGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BookingGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        SnehBLL.PatientPackage_Bll DB = new SnehBLL.PatientPackage_Bll();
        DataTable dt = DB.Search(txtSearch.Text.Trim(), _fromDate, _uptoDate);
        BookingGV.DataSource = dt;
        BookingGV.DataBind();
        if (BookingGV.HeaderRow != null) { BookingGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

        decimal _totalAmt = 0;
        if (dt.Rows.Count > 0)
        {
            decimal.TryParse(dt.Compute("SUM(PackageAmount)", string.Empty).ToString(), out _totalAmt);
        }
        lblTotal.Text = Math.Round(_totalAmt, 2).ToString();
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }
    public string FORMATDATETIME(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.dateFormattt);
        return "- - -";
    }

    public string DELETELINK(string str, string _can)
    {
        int _canDel = 0; int.TryParse(_can, out _canDel);
        if (_canDel > 0)
        {
            return "<span class='label label-success label-mini' style='font-weight:normal;font-size: 11px;'>Used</span>";
        }
        if (!isSuperAdmin)
        {
            if(_loginID == DbHelper.Configuration.managerLoginId)
            {
                return "<a href='/Member/PackageBookingd.aspx?record=" + str + "'>Delete</a>";
            }
        }
        return string.Empty;
    }
    public string CHECKUSAGELINK(string bookingId, string canUse)
    {
        int _canUseInt = 0;
        int.TryParse(canUse, out _canUseInt);
        int cat = SnehBLL.UserAccount_Bll.getCategory();
       

        // Allow only manager or super admin
        if (!isSuperAdmin)
        {
            
            if (cat == 1 || cat == 6)
            {
                return "<a href='/Member/ViewPackageDetails.aspx?id=" + bookingId + "' style='font-weight:bold;'>CHECK USAGE</a>";
            }
        }

        return string.Empty;
    }
    public string TOTALBALANCE(string str)
    {
        decimal _amt = 0; decimal.TryParse(str, out _amt);
        if (_amt < 0) { _amt = 0; }
        return Math.Round(_amt, 2).ToString();
    }
}
