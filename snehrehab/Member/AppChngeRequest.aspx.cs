using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace snehrehab.Member
{
    public partial class AppChngeRequest : System.Web.UI.Page
    {
        int _loginID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID == 3)
            {
                Response.Redirect(ResolveClientUrl("~/Member/"), true);
            }
            if (!IsPostBack)
            {
                txtFrom.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                if (Request.QueryString["fdate"] != null)
                {
                    if (Request.QueryString["fdate"].ToString().Length > 0)
                    {
                        txtFrom.Text = Request.QueryString["fdate"].ToString();
                    }
                }
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
            int _requestStatus = -1; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _requestStatus); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RequestStatus", SqlDbType.Int).Value = _requestStatus;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;

            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
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

        public string GETACTION(string _uniqueID, string _requestStatus)
        {
            StringBuilder html = new StringBuilder();
            int _appointmentStatusID = 0; int.TryParse(_requestStatus, out _appointmentStatusID);
            if (_appointmentStatusID == 0)
            {
                html.Append("<a href=\"/Member/AppChngeRequestP.aspx?request=" + _uniqueID + "\" class=\"btn-change btn-success\">Change</a>");
                html.Append("<a href=\"/Member/AppChngeRequestC.aspx?record=" + _uniqueID + "\" class=\"btn-reject btn-warning\">Reject</a>");
                html.Append("<a href=\"/Member/AppChngeRequestD.aspx?record=" + _uniqueID + "\" class=\"btn-delete btn-danger\">Delete</a>");
            }
            else if (_appointmentStatusID == 1)
            {
                html.Append("<span class=\"label label-success label-mini\">Completed</span>");
            }
            else if (_appointmentStatusID == 2)
            {
                html.Append("<span class=\"label label-important label-mini\">Rejected</span>");
            }
            else
            {
                html.Append("<span class=\"label label-info label-mini\">Unknown</span>");
            }
            return html.ToString();
        }

        protected void ReportGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HiddenField txtA = e.Row.FindControl("txtRequestStatus") as HiddenField;
            if (txtA != null)
            {
                int _requestStatus = 0; int.TryParse(txtA.Value, out _requestStatus);

                if (_requestStatus == 1)
                {
                    e.Row.CssClass = e.Row.CssClass + " request-complete";
                }
                else if (_requestStatus == 2)
                {
                    e.Row.CssClass = e.Row.CssClass + " request-reject";
                }
            }
        }
    }
}
