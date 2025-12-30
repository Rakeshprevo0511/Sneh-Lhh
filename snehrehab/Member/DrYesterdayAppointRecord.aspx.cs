using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class DrYesterdayAppointRecord : System.Web.UI.Page
    {
        int _loginID = 0; bool isAdmin = false; bool isSuperAdmin = false;
        public DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            isAdmin = SnehBLL.UserAccount_Bll.IsAdminOrReception();
            if (SnehBLL.UserAccount_Bll.getCategory() == 4)
            {
                isSuperAdmin = true;
            }
            if (!IsPostBack)
            {
                SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
                txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
                foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
                {
                    txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                }
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,1).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }
        }
        private void LoadData()
        {
            int _status = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out _status); }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            yestAppointRecord.DataSource = DB.SearchYestAppointRecord(_status, _doctorID, _fromDate, _uptoDate);
            ///dt = ds.Tables[0];
            //if (ds.Tables[0].Columns.Count > 1)
            //{
            //    int _col = dt.Columns.Count - 1;
            //    for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
            //    {
            //        _col++; string _column = ds.Tables[0].Columns[i].ColumnName;

            //        dt.Columns.Add(_column, typeof(string));
            //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //        {
            //            // dt.Rows[j][_col] = ds.Tables[0].Rows[j][i].ToString();
            //        }

            //        TemplateField ckhColumn = new TemplateField();
            //        ckhColumn.HeaderTemplate = new GridViewTemplate(ListItemType.Header, _column, null, false);
            //        ckhColumn.ItemTemplate = new GridViewTemplate(ListItemType.Item, _column, null, false);
            //        ckhColumn.FooterTemplate = new GridViewTemplate(ListItemType.Footer, _column, dt, false);
            //        yestAppointRecord.Columns.Add(ckhColumn);
            //    }
            //}
           
           // yestAppointRecord.DataSource = ds; 
            yestAppointRecord.DataBind();
            if (yestAppointRecord.HeaderRow != null) { yestAppointRecord.HeaderRow.TableSection = TableRowSection.TableHeader; }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            yestAppointRecord.PageIndex = 0; LoadData();
        }

        protected void yestAppointRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            yestAppointRecord.PageIndex = e.NewPageIndex; LoadData();
        }
        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
        public string FORMATRECORD(string str)
        {
            int yes_no = 0; if (txtStatus.SelectedItem != null) { int.TryParse(txtStatus.SelectedItem.Value, out yes_no); }
            if (yes_no == 1)
            {
                return "NO";
            }
            else if (yes_no == 0)
            {
                return "YES";
            }
            else if (yes_no == -1)
            {
                return "YES_NO ";
            }
            return "- - -";
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Dr. Yesterday's Appointment Report" + " " + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            yestAppointRecord.GridLines = GridLines.Both;
            yestAppointRecord.HeaderStyle.Font.Bold = true;
            yestAppointRecord.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
            
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
        bool isDoctor;
        //Constructor where we define the template type and column name.
        public GridViewTemplate(ListItemType type, string colname, DataTable dt, bool isDoctor)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;
            this.isDoctor = isDoctor;
            _dt = dt;
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label();            //Allocates the new label object.
                    string _row = _columnName; if (_columnName.IndexOf(@"__________") > -1) { _row = _columnName.Substring(0, _columnName.IndexOf(@"__________")); }
                    lbl.Text = _row;             //Assigns the name of the column in the lable.
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
            object dataDate = DataBinder.Eval(container.DataItem, "AddedDate");
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            int _doctorID = 0; if (_columnName.LastIndexOf(@"__________") > -1) { int.TryParse(_columnName.Substring(_columnName.LastIndexOf(@"__________")).Replace(@"__________", ""), out _doctorID); }
            if (dataValue != DBNull.Value)
            {
                float amt = 0; float.TryParse(dataValue.ToString(), out amt);
                DateTime _test = new DateTime(); DateTime.TryParseExact(dataDate.ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
                if (amt > 0)
                {
                    if (isDoctor)
                    {
                        txtdata.Text = "<a href=\"javascript:;\"  onclick=\"DoctorAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + _doctorID.ToString() + ")\">" + amt.ToString() + "</a>";
                    }
                    else
                    {
                        txtdata.Text = "<a href=\"javascript:;\"  onclick=\"OtherAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + _doctorID.ToString() + ")\">" + amt.ToString() + "</a>";
                    }
                }
                else
                {
                    txtdata.Text = "- - -";
                }
            }
        }
    }
}