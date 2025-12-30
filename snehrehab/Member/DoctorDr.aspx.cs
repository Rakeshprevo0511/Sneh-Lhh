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

public partial class Member_DoctorDr : System.Web.UI.Page
{
    int _loginID = 0; DataTable dt = new DataTable(); decimal _paidAmt = 0; decimal _debit = 0; decimal _net = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            txtSession.Items.Clear(); 
            txtSession.Items.Add(new ListItem("All Session", "-1"));
            foreach (SnehDLL.SessionMast_Dll SMD in SMB.GetList())
            {
                if (SMD.SessionID == 1 || SMD.SessionID == 16|| SMD.SessionID == 17 || SMD.SessionID == 18)
                {
                    txtSession.Items.Add(new ListItem(SMD.SessionName, SMD.SessionID.ToString()));
                }
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
        ReportGV.PageIndex = 0; LoadData();
    }

    protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ReportGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
        SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
        
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        dt = DB.DoctorDr(_loginID, _sessionID, _fromDate, _uptoDate);
        ReportGV.DataSource = dt;
        ReportGV.DataBind();
        if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void ReportGV_DataBound(object sender, EventArgs e)
    {
        LoadTotal();
    }

    private void LoadTotal()
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            decimal _tmpP = 0; decimal _tmpD = 0; decimal _tempN = 0;
            decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _tmpD);
            decimal.TryParse(dt.Rows[i]["AppointmentAmt"].ToString(), out _tmpP);
            decimal.TryParse(dt.Rows[i]["NetAmt"].ToString(), out _tempN);
            _paidAmt += _tmpP; _debit += _tmpD; _net += _tempN;
            
        }
        PlaceHolder1.Controls.Add(new LiteralControl("<div class=\"alert alert-info\"><strong>" +
          "SESSION AMT : " + Math.Round(_paidAmt, 2).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +
          //"DR. AMT : " + Math.Round(_debit, 0).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +
          "DR. AMT : " + Math.Round(_net, 2).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +

          "</strong></div>"));

        //PlaceHolder1.Controls.Add(new LiteralControl("<div class=\"alert alert-info\"><strong>" +
        //  "SESSION AMT : " + _paidAmt.ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +
        //    //"DR. AMT : " + Math.Round(_debit, 0).ToString() + "/- INR &nbsp;&nbsp;&nbsp;" +

        //  "</strong></div>"));
    }
}
