using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class PatientPhotoAlert : System.Web.UI.Page
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
            if (!SnehBLL.UserAccount_Bll.IsAdminOrReception())
            {
                if (!isSuperAdmin)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/"), true);
                }
            }
            if (!IsPostBack)
            {
                txtPatientType.Items.Clear(); txtPatientType.Items.Add(new ListItem("Any Registration", "-1"));
                SnehBLL.PatientTypes_Bll PTB = new SnehBLL.PatientTypes_Bll();
                foreach (SnehDLL.PatientTypes_Dll PTD in PTB.GetList())
                {
                    if (PTD.PatientTypeID != 3)
                    {
                        txtPatientType.Items.Add(new ListItem(PTD.PatientType, PTD.PatientTypeID.ToString()));
                    }
                }
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, 1, 1).ToString(DbHelper.Configuration.showDateFormat);
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

            SqlCommand cmd = new SqlCommand("PatientMast_PhotoAlertSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = _patientType;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            PatientGV.DataSource = dt;
            PatientGV.DataBind();
            if (isSuperAdmin)
            {
                PatientGV.Columns[9].Visible = false;
            }
            if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string GetAction(string UniqueID)
        {
            if (!isSuperAdmin)
                return "<a href=\"/Member/Patiente.aspx?record=" + UniqueID + "&tab=upload\">Upload</a>";
            return string.Empty;
        }
    }
}