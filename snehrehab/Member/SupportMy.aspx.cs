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

public partial class Member_SupportMy : System.Web.UI.Page
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
        TicketGV.PageIndex = 0; LoadData();
    }

    protected void TicketGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        TicketGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        SnehBLL.SupportTicket_Bll DB = new SnehBLL.SupportTicket_Bll();
        TicketGV.DataSource = DB.Search(_loginID, _fromDate, _uptoDate);
        TicketGV.DataBind();
        if (TicketGV.HeaderRow != null) { TicketGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    public string FILELINK(string _uniqueID, string _uFile)
    {
        if (_uFile.Trim().Length > 0)
        {
            return "<a href=\"/Member/SupportFile.ashx?record=" + _uniqueID + "\"  target=\"_blank\" >" + _uFile + "</a>";
        }
        return "- - -";
    }
}
