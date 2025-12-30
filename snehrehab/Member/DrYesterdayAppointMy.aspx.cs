using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class DrYesterdayAppointMy : System.Web.UI.Page
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
            //int _catID = SnehBLL.UserAccount_Bll.getCategory();
            //if (_catID == 1)
            //{
            //    TicketGV.Columns[7].Visible = true;
            //}
        }
        protected void TicketGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TicketGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            TicketGV.DataSource = DB.DrYesterdayAppointMy(_loginID, _fromDate, _uptoDate);
            TicketGV.DataBind();
            if (TicketGV.HeaderRow != null) { TicketGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            TicketGV.PageIndex = 0; LoadData();
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
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Dr Yesterday Appointment Report" + " " + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            TicketGV.GridLines = GridLines.Both;
            TicketGV.HeaderStyle.Font.Bold = true;
            TicketGV.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}
