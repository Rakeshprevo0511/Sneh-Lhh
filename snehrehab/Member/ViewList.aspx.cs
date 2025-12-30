using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class ViewList : System.Web.UI.Page
    {
        int _loginID = 0; bool isreceiption = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 2)
            {
                isreceiption = true;
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            StaffGV.PageIndex = 0; LoadData();
        }

        private void LoadData()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            int id = 0; if (txtType.SelectedItem != null) { int.TryParse(txtType.SelectedItem.Value, out id); }
            if (id == 1)
            {
                SnehBLL.Management_Bll MB = new SnehBLL.Management_Bll();
                StaffGV.DataSource = MB.GetList(id, txtSearch.Text.Trim(), _fromDate, _uptoDate);
                StaffGV.DataBind();
            }
            else if (id == 3)
            {
                SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
                StaffGV.DataSource = RB.GetList(id, txtSearch.Text.Trim(), _fromDate, _uptoDate);
                StaffGV.DataBind();
            }

            if (StaffGV.HeaderRow != null) { StaffGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParse(_str, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string FORMATDATENEW(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string GetAction(string UniqueID, string id)
        {
            int idnew = 0; int.TryParse(id, out idnew);
            if (isreceiption)
            {
                //return "<a href=\"ViewList.aspx?record=" + UniqueID + "\">View</a>";
                if (idnew == 1)
                {
                    return "<a href=\"/Member/ManagerView.aspx?record=" + UniqueID + "\">View</a>";
                }
                else if (idnew == 3)
                {
                    return "<a href=\"/Member/ReceiptionView.aspx?record=" + UniqueID + "\">View</a>";
                }
                return string.Empty;
            }
            else
            {
                if (idnew == 1)
                {
                    if (_loginID == DbHelper.Configuration.managerLoginId)
                    {
                        return "<a href=\"/Member/ManagerView.aspx?record=" + UniqueID + "\">View</a>&nbsp;" +
                                "<a href=\"/Member/Manager.aspx?record=" + UniqueID + "\">Edit</a>&nbsp;" +
                               "<a href=\"/Member/Managerd.aspx?record=" + UniqueID + "\">Delete</a>";
                    }
                    return "<a href=\"/Member/ManagerView.aspx?record=" + UniqueID + "\">View</a>&nbsp;" +
                                "<a href=\"/Member/Manager.aspx?record=" + UniqueID + "\">Edit</a>&nbsp;";                               
                }
                else if (idnew == 3)
                {
                    if (_loginID == DbHelper.Configuration.managerLoginId)
                    {
                        return "<a href=\"/Member/ReceiptionView.aspx?record=" + UniqueID + "\">View</a>&nbsp;" +
                            "<a href=\"/Member/Receiption.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;" +
                           "<a href=\"/Member/Receiptiond.aspx?record=" + UniqueID + "\">Delete</a>";
                    }
                    return "<a href=\"/Member/ReceiptionView.aspx?record=" + UniqueID + "\">View</a>&nbsp;" +
                            "<a href=\"/Member/Receiption.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;";                           
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            int id = 0; if (txtType.SelectedItem != null) { int.TryParse(txtType.SelectedItem.Value, out id); }
            DataTable dt = new DataTable();
            SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
            dt = RB.GetListNew(id, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            StringBuilder html = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                if (id == 1)
                {
                    html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                    html.Append("<tr><td><b>Report Name:</b></td><td>Manager List Report</td></tr>");
                    html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                    html.Append("</table>");
                    html.Append("<br/>");
                    html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                    html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>TELEPHONE</th><th>EMAIL ID</th><th>DESIGNATION</th><th>QUALIFICATIONS</th><th>BIRTH DATE</th><th>JOIN DATE</th></tr>");
                }
                else if (id == 3)
                {
                    html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                    html.Append("<tr><td><b>Report Name:</b></td><td>Receiption List Report</td></tr>");
                    html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                    html.Append("</table>");
                    html.Append("<br/>");
                    html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                    html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>TELEPHONE</th><th>EMAIL ID</th><th>DESIGNATION</th><th>QUALIFICATIONS</th><th>BIRTH DATE</th><th>JOIN DATE</th></tr>");
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ContactNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Designation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Qualifications"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATENEW(dt.Rows[i]["BirthDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATENEW(dt.Rows[i]["JoinDate"].ToString()) + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=registration list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();

            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
            }

        }

        protected void StaffGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StaffGV.PageIndex = e.NewPageIndex; LoadData();
        }
    }
}