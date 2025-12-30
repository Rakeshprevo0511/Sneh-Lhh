using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class OtherActCategory : System.Web.UI.Page
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
        protected void OtherActCategoryGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            OtherActCategoryGV.PageIndex = e.NewPageIndex; LoadForm();
        }
        private void LoadForm()
        {
            SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
            OtherActCategoryGV.DataSource = OCB.GetList();
            OtherActCategoryGV.DataBind();
            if (OtherActCategoryGV.HeaderRow != null) { OtherActCategoryGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (txtOtherActCategory.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter category name...", 2); return;
            }
            int _categoryID = 0; if (txtHidden.Value != null) { int.TryParse(txtHidden.Value, out _categoryID); }
            SnehDLL.OtherActCategory_DLL OCD = new SnehDLL.OtherActCategory_DLL();
            OCD.CategoryID = _categoryID; OCD.UniqueID = "";
            OCD.CategoryName = txtOtherActCategory.Text.Trim();
            SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
            int i = OCB.Set(OCD);
            if (i > 0)
            {
                btnSave.Text = "Add New"; btnCancel.Visible = false; txtOtherActCategory.Text = ""; txtHidden.Value = "0";
                OtherActCategoryGV.PageIndex = 0; LoadForm();
                DbHelper.Configuration.setAlert(Page, "Category details saved successfully...", 1);
            }
            else if (i < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Category Already Added.", 2);
                return;
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
                return;
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender;
            int _categoryID = 0; if (lk != null) { int.TryParse(lk.CommandArgument, out _categoryID); }
            if (_categoryID > 0)
            {
                SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
                SnehDLL.OtherActCategory_DLL OCD = OCB.Get(_categoryID);
                if (OCD != null)
                {
                    txtOtherActCategory.Text = OCD.CategoryName;
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
                SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
                int i = OCB.Delete(_categoryID);
                if (i > 0)
                {
                    txtOtherActCategory.Text = ""; btnSave.Text = "Add New"; btnCancel.Visible = false; txtHidden.Value = "0";
                    OtherActCategoryGV.PageIndex = 0; LoadForm();
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
            txtOtherActCategory.Text = ""; txtHidden.Value = "0"; btnSave.Text = "Add New"; btnCancel.Visible = false;
            OtherActCategoryGV.PageIndex = 0; LoadForm();
        }
    }
}