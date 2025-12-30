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

namespace snehrehab.Member
{
    public partial class Products : System.Web.UI.Page
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
                txtCategory.Items.Clear(); txtCategory.Items.Add(new ListItem("All Category", "-1"));
                SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
                foreach (SnehDLL.ProductCategory_Dll PCD in PCB.GetList())
                {
                    txtCategory.Items.Add(new ListItem(PCD.Category, PCD.CategoryID.ToString()));
                }
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ProductGV.PageIndex = 0; LoadData();
        }

        protected void ProductGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProductGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
            SnehBLL.ProductMst_Bll DB = new SnehBLL.ProductMst_Bll();
            ProductGV.DataSource = DB.Search(_categoryID, txtSearch.Text.Trim());
            ProductGV.DataBind();
            if (ProductGV.HeaderRow != null) { ProductGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            ProductGV.Columns[3].Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId){ ProductGV.Columns[3].Visible = true; }
            btn_add_product.Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                btn_add_product.Visible = true;
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
    }
}
