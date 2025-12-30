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

namespace snehrehab.Member
{
    public partial class ProductCat : System.Web.UI.Page
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
                LoadForm();
            }
        }

        protected void CategoryGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CategoryGV.PageIndex = e.NewPageIndex; LoadForm();
        }

        private void LoadForm()
        {
            SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
            CategoryGV.DataSource = PCB.GetList();
            CategoryGV.DataBind();
            if (CategoryGV.HeaderRow != null) { CategoryGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter category name...", 2); return;
            }
            int _categoryID = 0; if (txtHidden.Value != null) { int.TryParse(txtHidden.Value, out _categoryID); }
            SnehDLL.ProductCategory_Dll PCD = new SnehDLL.ProductCategory_Dll();
            PCD.CategoryID = _categoryID; PCD.UniqueID = "";
            PCD.Category = txtCategory.Text.Trim(); PCD.ParentID = 0;
            PCD.AddedDate = DateTime.UtcNow.AddMinutes(330);
            PCD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            PCD.AddedBy = _loginID; PCD.ModifyBy = _loginID;

            SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
            int i = PCB.Set(PCD);
            if (i > 0)
            {
                btnSave.Text = "Add New"; btnCancel.Visible = false; txtCategory.Text = ""; txtHidden.Value = "0"; 
                CategoryGV.PageIndex = 0; LoadForm();
                DbHelper.Configuration.setAlert(Page, "Category details saved successfully...", 1);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender;
            int _categoryID = 0; if (lk != null) { int.TryParse(lk.CommandArgument, out _categoryID); }
            if (_categoryID > 0)
            {
                SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll(); SnehDLL.ProductCategory_Dll PCD = PCB.Get(_categoryID);
                if (PCD != null)
                {
                    txtCategory.Text = PCD.Category;
                    btnSave.Text = "Update"; btnCancel.Visible = true; txtHidden.Value = _categoryID.ToString();
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender;
            int _categoryID = 0; if (lk != null) { int.TryParse(lk.CommandArgument, out _categoryID); }
            if (_categoryID > 0)
            {
                SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
                int i = PCB.Delete(_categoryID);
                if (i > 0)
                {
                    txtCategory.Text = ""; btnSave.Text = "Add New"; btnCancel.Visible = false; txtHidden.Value = "0";
                    CategoryGV.PageIndex = 0; LoadForm();
                    DbHelper.Configuration.setAlert(Page, "Category detail deleted successfully...", 1);
                }
                else if (i == -10)
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to delete, Category contains products...", 3);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtCategory.Text = ""; txtHidden.Value = "0"; btnSave.Text = "Add New"; btnCancel.Visible = false;
            CategoryGV.PageIndex = 0; LoadForm();
        }
    }
}
