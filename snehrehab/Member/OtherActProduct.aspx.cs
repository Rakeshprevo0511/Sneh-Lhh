using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class OtherActProduct : System.Web.UI.Page
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
                    _productID = SnehBLL.OtherActProduct_BLL.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_productID > 0) { _headerText = "Edit Other Activity Product"; } else { _headerText = "Add New Other Activity Product"; }
            if (!IsPostBack)
            {
                LoadForm();
                if (_productID > 0)
                {
                    LoadProduct();
                }
            }

        }
        private void LoadProduct()
        {
            SnehBLL.OtherActProduct_BLL OPB = new SnehBLL.OtherActProduct_BLL();
            SnehDLL.OtherActProduct_DLL OPD = OPB.Get(_productID);
            if (OPD != null)
            {
                if (txtOtherActCategory.Items.FindByValue(OPD.CategoryID.ToString()) != null)
                {
                    txtOtherActCategory.SelectedValue = OPD.CategoryID.ToString();
                }
                txtOtherActProductName.Text = OPD.ProductName;
                txtOtherActUnitPrice.Text = OPD.UnitPrice.ToString();
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find product details, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "3";
                Response.Redirect(ResolveClientUrl("~/Member/OtherActProducts.aspx"), true);
            }
        }
        private void LoadForm()
        {
            txtOtherActCategory.Items.Clear(); txtOtherActCategory.Items.Add(new ListItem("Select Category", "-1"));
            SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
            foreach (SnehDLL.OtherActCategory_DLL OCD in OCB.GetList())
            {
                txtOtherActCategory.Items.Add(new ListItem(OCD.CategoryName.ToString(), OCD.CategoryID.ToString()));
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _categoryID = 0; if (txtOtherActCategory.SelectedItem != null) { int.TryParse(txtOtherActCategory.SelectedItem.Value, out _categoryID); }
            if (_categoryID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select product category...", 2); return;
            }
            float _unitPrice = 0; float.TryParse(txtOtherActUnitPrice.Text.Trim(), out _unitPrice);
            SnehDLL.OtherActProduct_DLL OPD = new SnehDLL.OtherActProduct_DLL();
            OPD.ProductID = _productID; OPD.UniqueID = "";
            OPD.ProductName = txtOtherActProductName.Text.Trim();
            OPD.UnitPrice = _unitPrice;
            OPD.CategoryID = _categoryID;


            SnehBLL.OtherActProduct_BLL OPB = new SnehBLL.OtherActProduct_BLL();
            int i = OPB.Set(OPD);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Product details saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/OtherActProducts.aspx"), true);
            }
            else if (i < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Product Already Added.", 2);
                return;
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
                return;
            }
        }
    }
}