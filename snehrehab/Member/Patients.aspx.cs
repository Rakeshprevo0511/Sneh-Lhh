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
using System.Text;
using System.IO;

public partial class Member_Patients : System.Web.UI.Page
{
    int _loginID = 0; bool isSuperAdmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (SnehBLL.UserAccount_Bll.getCategory() == 4 || SnehBLL.UserAccount_Bll.getCategory() == 5)
        {
            isSuperAdmin = true;
        }
        if (!IsPostBack)
        {
            txtPatientType.Items.Clear(); txtPatientType.Items.Add(new ListItem("Any Registration", "-1"));
            SnehBLL.PatientTypes_Bll PTB = new SnehBLL.PatientTypes_Bll();
            foreach (SnehDLL.PatientTypes_Dll PTD in PTB.GetList())
            {
                txtPatientType.Items.Add(new ListItem(PTD.PatientType, PTD.PatientTypeID.ToString()));
            }

            if (Request.QueryString["search"] != null)
            {
                if (Request.QueryString["search"].ToString().Length > 0)
                {
                    txtSearch.Text = Request.QueryString["search"].ToString(); 
                }
            } 
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PatientGV.PageIndex = 0; LoadData();
    }

    protected void PatientGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PatientGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        int _patientType = 0; if (txtPatientType.SelectedItem != null) { int.TryParse(txtPatientType.SelectedItem.Value, out _patientType); }
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

        SnehBLL.PatientMast_Bll DB = new SnehBLL.PatientMast_Bll();
        PatientGV.DataSource = DB.Search(_patientType, txtSearch.Text.Trim(), _fromDate, _uptoDate);
        PatientGV.DataBind();
        if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        PatientGV.AllowPaging = false;
        LoadData();
        Response.Clear();
        Response.Buffer = true;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Charset = "";
        string FileName = "Patient List Report" + " " + DateTime.Now + ".xls";
        StringWriter strwritter = new StringWriter();
        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        PatientGV.GridLines = GridLines.Both;
        PatientGV.HeaderStyle.Font.Bold = true;
        //PatientGV.RenderControl(htmltextwrtter);
        Response.Write(strwritter.ToString());
        PatientGV.AllowPaging = true;
        LoadData();
        Response.End();
    }
    public string GetAction(string UniqueID)
    {
        if (isSuperAdmin)
        {
            return "<a href=\"Patientv.aspx?record=" + UniqueID + "\">View</a>";
        }
        else
        {
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                return "<a href=\"Pediatric.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;" +
                         "<a href=\"Patientd.aspx?record=" + UniqueID + "\">Delete</a>";
            }
            return "<a href=\"Pediatric.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;";                         
        }
    }
}
