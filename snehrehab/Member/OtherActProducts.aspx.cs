using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class OtherActProducts : System.Web.UI.Page
    {
        int _loginID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID < 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                txtOtherActCategory.Items.Clear(); txtOtherActCategory.Items.Add(new ListItem("All Category", "-1"));
                SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
                foreach (SnehDLL.OtherActCategory_DLL OCD in OCB.GetList())
                {
                    txtOtherActCategory.Items.Add(new ListItem((OCD.CategoryName.ToString()), OCD.CategoryID.ToString()));
                }
                LoadData();

            }

        }
        private void LoadData()
        {
            int _categoryID = 0; if (txtOtherActCategory.SelectedItem != null) { int.TryParse(txtOtherActCategory.SelectedItem.Value, out _categoryID); }
            SnehBLL.OtherActProduct_BLL OCB = new SnehBLL.OtherActProduct_BLL();
            ProductOtherActGV.DataSource = OCB.Search(_categoryID, txtOtherActSearch.Text.Trim());
            ProductOtherActGV.DataBind();
            if (ProductOtherActGV.HeaderRow != null) { ProductOtherActGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            ProductOtherActGV.Columns[3].Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId) { ProductOtherActGV.Columns[3].Visible = true; }
            btn_add_product.Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                btn_add_product.Visible = true;
            }
        }

        protected void btnOtherActSearch_Click(object sender, EventArgs e)
        {
            ProductOtherActGV.PageIndex = 0; LoadData();
        }

        protected void ProductOtherActGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProductOtherActGV.PageIndex = e.NewPageIndex; LoadData();
        }
    }
}