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
    public partial class Product : System.Web.UI.Page
    {
        int _loginID = 0; int _productID = 0; public string _headerText = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _productID = SnehBLL.ProductMst_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_productID > 0) { _headerText = "Edit Product"; } else { _headerText = "Add New Product"; }

            if (!IsPostBack)
            {
                LoadForm();
                if (_productID > 0)
                {
                    LoadProduct();
                }
            }
        }

        private void LoadForm()
        {
            txtCategory.Items.Clear(); txtCategory.Items.Add(new ListItem("Select Category", "-1"));
            SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
            foreach (SnehDLL.ProductCategory_Dll PCD in PCB.GetList())
            {
                txtCategory.Items.Add(new ListItem(PCD.Category, PCD.CategoryID.ToString()));
            }
        }

        private void LoadProduct()
        {
            SnehBLL.ProductMst_Bll PBM = new SnehBLL.ProductMst_Bll();
            SnehDLL.ProductMst_Dll PBD = PBM.Get(_productID);
            if (PBD != null)
            {
                if (txtCategory.Items.FindByValue(PBD.CategoryID.ToString()) != null)
                {
                    txtCategory.SelectedValue = PBD.CategoryID.ToString();
                }
                txtProductName.Text = PBD.ProductName;
                txtUnitPrice.Text = PBD.UnitPrice.ToString();
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find product details, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "3";
                Response.Redirect(ResolveClientUrl("~/Member/Products.aspx"), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
            if (_categoryID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select product category...", 2); return;
            }
            float _unitPrice = 0; float.TryParse(txtUnitPrice.Text.Trim(), out _unitPrice);
            SnehDLL.ProductMst_Dll PD = new SnehDLL.ProductMst_Dll();
            PD.ProductID = _productID; PD.UniqueID = "";
            PD.ProductName = txtProductName.Text.Trim(); PD.UnitPrice = _unitPrice;
            PD.CategoryID = _categoryID;
            PD.AddedDate = DateTime.UtcNow.AddMinutes(330);
            PD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            PD.AddedBy = _loginID; PD.ModifyBy = _loginID;

            SnehBLL.ProductMst_Bll PB = new SnehBLL.ProductMst_Bll();
            int i = PB.Set(PD);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Product details saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/Products.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
    }
}
