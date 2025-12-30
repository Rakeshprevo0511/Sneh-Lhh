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
using System.Globalization;
using System.Text;

namespace snehrehab.Reports
{
    public partial class AppointmentDaily : System.Web.UI.Page
    {
        int _loginID = 0; DataTable dt = new DataTable();

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
                txtDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
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
            DateTime _onDate = new DateTime(); DateTime.TryParseExact(txtDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _onDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            dt = DB.AppointmentDaily(_onDate);
            for (int i = 2; i < dt.Columns.Count; i++)
            {
                string _column = dt.Columns[i].ColumnName;

                TemplateField ckhColumn = new TemplateField();
                ckhColumn.HeaderTemplate = new GridViewTemplate(ListItemType.Header, _column, null);
                ckhColumn.ItemTemplate = new GridViewTemplate(ListItemType.Item, _column, null);
                ckhColumn.FooterTemplate = new GridViewTemplate(ListItemType.Footer, _column, dt);
                ReportGV.Columns.Add(ckhColumn);
            }
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

        public string FORMATTIME(string _str)
        {
            DateTime AppointmentTimeD = new DateTime();
            DateTime.TryParseExact(_str, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentTimeD);
            if (AppointmentTimeD > DateTime.MinValue && AppointmentTimeD < DateTime.MaxValue)
            {
                return AppointmentTimeD.ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "- - -";
        }

        protected void ReportGV_DataBound(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _onDate = new DateTime(); DateTime.TryParseExact(txtDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _onDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            dt = DB.AppointmentDaily(_onDate);
            
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table>");
                html.Append("<tr><td><b>Report Name:</b></td><td>Daily Appointment Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _onDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");

                html.Append("<br/>");

                html.Append("<table border=\"1\">");
                html.Append("<tr><th>TIME</th>");
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    string _column = dt.Columns[i].ColumnName; if (_column.IndexOf(@"__________") > -1) { _column = _column.Substring(0, _column.IndexOf(@"__________")); }
                    html.Append("<th>" + _column + "</th>");
                }
                html.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr><td style=\"vertical-align:top;\">" + FORMATTIME(dt.Rows[i]["TimeHourNew"].ToString()) + "</td>");
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        string dataValue = dt.Rows[i][j].ToString(); string _names = "";
                        if (dataValue.Trim().Length > 0)
                        {
                            string[] values = dataValue.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int k = 0; k < values.Length; k++)
                            {
                                string _name = values[k].Substring(values[k].LastIndexOf(@"__________")).Replace(@"__________", "").Trim();
                                int _appointmentID = 0; int.TryParse(values[k].Substring(0, values[k].IndexOf(@"__________")), out _appointmentID);
                                _names += _name + ",";
                            }
                            if (_names.Length > 0)
                            {
                                _names = _names.Substring(0, _names.Length - 1); 
                            }
                        }
                        html.Append("<td style=\"vertical-align:top;\">" + _names + "</td>");
                    }
                    html.Append("</tr>");
                }
                html.Append("</table>");

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=daily appointment report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 2);
            }
        }

        class GridViewTemplate : ITemplate
        {
            //A variable to hold the type of ListItemType.
            ListItemType _templateType;
            //A variable to hold the data source.
            DataTable _dt;
            //A variable to hold the column name.
            string _columnName;

            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();

            //Constructor where we define the template type and column name.
            public GridViewTemplate(ListItemType type, string colname, DataTable dt)
            {
                //Stores the template type.
                _templateType = type;

                //Stores the column name.
                _columnName = colname;

                _dt = dt;
            }

            void ITemplate.InstantiateIn(System.Web.UI.Control container)
            {
                switch (_templateType)
                {
                    case ListItemType.Header:
                        //Creates a new label control and add it to the container.
                        Label lbl = new Label();            //Allocates the new label object.
                        string _column = _columnName; if (_columnName.IndexOf(@"__________") > -1) { _column = _columnName.Substring(0, _columnName.IndexOf(@"__________")); }
                        lbl.Text = _column;             //Assigns the name of the column in the lable.
                        container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                        break;

                    case ListItemType.Item:
                        LiteralControl tb1 = new LiteralControl();
                        tb1.DataBinding += new EventHandler(tb1_DataBinding);
                        container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                        break;

                    case ListItemType.EditItem:
                        //As, I am not using any EditItem, I didnot added any code here.
                        break;

                    case ListItemType.Footer:
                        Label lblFooter = new Label();
                        lblFooter.DataBinding += new EventHandler(lblFooter_DataBinding);
                        container.Controls.Add(lblFooter);
                        break;
                }
            }

            void lblFooter_DataBinding(object sender, EventArgs e)
            {
                Label lbl = (Label)sender; decimal _totalAmt = 0;

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    decimal _totalAmtTmp = 0;
                    decimal.TryParse(_dt.Rows[i][_columnName].ToString(), out _totalAmtTmp);
                    _totalAmt += _totalAmtTmp;
                }
                lbl.Text = Math.Round(_totalAmt, 2).ToString(); lbl.Font.Bold = true;
            }

            void tb1_DataBinding(object sender, EventArgs e)
            {
                LiteralControl txtdata = (LiteralControl)sender;
                GridViewRow container = (GridViewRow)txtdata.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, _columnName);
                int _doctorID = 0; if (_columnName.LastIndexOf(@"__________") > -1) { int.TryParse(_columnName.Substring(_columnName.LastIndexOf(@"__________")).Replace(@"__________", ""), out _doctorID); }
                if (dataValue != DBNull.Value && dataValue.ToString().Trim().Length > 0)
                {
                    string[] values = dataValue.ToString().Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
                    string _names = "";
                    for (int i = 0; i < values.Length; i++)
                    {
                        string _name = values[i].Substring(values[i].LastIndexOf(@"__________")).Replace(@"__________", "").Trim();
                        int _appointmentID = 0; int.TryParse(values[i].Substring(0, values[i].IndexOf(@"__________")), out _appointmentID);
                        if (_appointmentID > 0)
                        {
                            SnehDLL.Appointments_Dll AD = AB.Get(_appointmentID);
                            if (AD.AppointmentStatus == 0)
                            {
                                _names += "<a href=\"/Member/AppChngeRequestP.aspx?record=" + AD.UniqueID + "\" >" + _name + "(" + AD.Duration.ToString() + " Min)" + "</a><br/>";
                            }
                            else
                            {
                                _names += _name + "(" + AD.Duration.ToString() + " Min)" + "<br/>";
                            }
                        }
                        else
                        {
                            _names += _name + "<br/>";
                        }
                    }
                    txtdata.Text = _names;
                }
            }
        }
    }
}
