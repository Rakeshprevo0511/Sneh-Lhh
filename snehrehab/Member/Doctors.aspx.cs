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

public partial class Member_Doctors : System.Web.UI.Page
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
                lblAddNew.Text = "<a  href=\"/Member/Doctor.aspx\" class=\"btn btn-primary\">Add New</a>";
            }
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoctorGV.PageIndex = 0; LoadData();
    }

    protected void DoctorGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DoctorGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

        SnehBLL.DoctorMast_Bll DB = new SnehBLL.DoctorMast_Bll();
        DoctorGV.DataSource = DB.Search(txtSearch.Text.Trim(), _fromDate, _uptoDate);
        DoctorGV.DataBind();
        if (DoctorGV.HeaderRow != null) { DoctorGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    public string SPECIALITY(string _str)
    {
        int _specialityID = 0; int.TryParse(_str, out _specialityID);
        if (_specialityID > 0)
        {
            SnehBLL.Specialities_Bll SB = new SnehBLL.Specialities_Bll();
            SnehDLL.Specialities_Dll SD = SB.Get(_specialityID);
            if (SD != null)
                return SD.Speciality;
        }
        return "- - -";
    }

    public string WORKPLACE(string _str, string _other)
    {
        int _workPlaceID = 0; int.TryParse(_str, out _workPlaceID);
        if (_workPlaceID > 0)
        {
            SnehBLL.WorkPlaces_Bll SB = new SnehBLL.WorkPlaces_Bll();
            SnehDLL.WorkPlaces_Dll SD = SB.Get(_workPlaceID);
            if (SD != null)
                return SD.Workplace;
        }
        else if (_workPlaceID == 0)
        {
            return _other;
        }
        return "- - -";
    }

    public string GetAction(string UniqueID)
    {
        if (isSuperAdmin)
        {
            return "<a href=\"Doctorv.aspx?record=" + UniqueID + "\">View</a>";
        }
        else
        {
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                return "<a href=\"/Member/Doctor.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;" +
                         "<a href=\"/Member/Doctord.aspx?record=" + UniqueID + "\">Delete</a>";
            }
            return "<a href=\"/Member/Doctor.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;";                         
        }
    }
}
