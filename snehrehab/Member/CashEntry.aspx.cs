using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;

public partial class Member_CashEntry : System.Web.UI.Page
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
            txtPayDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
            txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
            foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
            {
                txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
            }

            //if (txtAccountType.Items.Count > 1)
            //{
            //    txtAccountType.SelectedIndex = 1;
            //}

            LoadAccountName();

            SnehBLL.ProductCategory_Bll PCB = new SnehBLL.ProductCategory_Bll();
            txtProductCategory.Items.Clear(); txtProductCategory.Items.Add(new ListItem("Select Category", "-1"));
            foreach (SnehDLL.ProductCategory_Dll PCD in PCB.GetList())
            {
                txtProductCategory.Items.Add(new ListItem(PCD.Category, PCD.CategoryID.ToString()));
            }

            LoadProducts();

        }
    }

    protected void txtAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAccountName();
    }

    private void LoadAccountName()
    {
        int _accountType = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _accountType); }
        txtAccountName.Items.Clear(); txtAccountName.Items.Add(new ListItem("Select Account Name", "-1"));
        if (_accountType == 1)
        {
            SnehBLL.AccountHeads_Bll AB = new SnehBLL.AccountHeads_Bll();
            foreach (SnehDLL.AccountHeads_Dll AD in AB.GetList())
            {
                txtAccountName.Items.Add(new ListItem(AD.HeadName, AD.HeadID.ToString()));
            }
        }
        else if (_accountType == 2)
        {
            SnehBLL.DoctorMast_Bll AB = new SnehBLL.DoctorMast_Bll();
            foreach (SnehDLL.DoctorMast_Dll AD in AB.GetForDropdown())
            {
                txtAccountName.Items.Add(new ListItem(AD.PreFix + " " + AD.FullName, AD.DoctorID.ToString()));
            }
        }
        else if (_accountType == 3)
        {
            SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
            foreach (SnehDLL.PatientMast_Dll PD in PB.GetForDropdown())
            {
                txtAccountName.Items.Add(new ListItem(PD.FullName, PD.PatientID.ToString()));
            }
        }
    }

    protected void txtProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void LoadProducts()
    {
        int _productCategory = 0; if (txtProductCategory.SelectedItem != null) { int.TryParse(txtProductCategory.SelectedItem.Value, out _productCategory); }
        SnehBLL.ProductMst_Bll PCB = new SnehBLL.ProductMst_Bll();
        txtProductName.Items.Clear(); txtProductName.Items.Add(new ListItem("Select Product", "-1"));
        foreach (SnehDLL.ProductMst_Dll PCD in PCB.GetList(_productCategory))
        {
            txtProductName.Items.Add(new ListItem(PCD.ProductName, PCD.ProductID.ToString()));
        }
    }

    protected void txtPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_SessionBank.Visible = false;
        int _paymentType = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentType); }
        if (_paymentType == 3)
        {
            tb_SessionBank.Visible = true;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _accountType = 0; if (txtAccountType.SelectedItem != null) { int.TryParse(txtAccountType.SelectedItem.Value, out _accountType); }
        if (_accountType <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select account type...", 2); return;
        }
        int _accountID = 0; if (txtAccountName.SelectedItem != null) { int.TryParse(txtAccountName.SelectedItem.Value, out _accountID); }
        if (_accountID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select account name...", 2); return;
        }
        int _categoryID = 0; if (txtProductCategory.SelectedItem != null) { int.TryParse(txtProductCategory.SelectedItem.Value, out _categoryID); }
        if (_categoryID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select product category...", 2); return;
        }
        int _productID = 0; if (txtProductName.SelectedItem != null) { int.TryParse(txtProductName.SelectedItem.Value, out _productID); }
        if (_productID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select product...", 2); return;
        }
        float _amount = 0; float.TryParse(txtAmount.Text.Trim(), out _amount);
        if (_amount <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter transaction amount...", 2); return;
        }
        if (txtPayDate.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select transaction date...", 2); return;
        }
        DateTime _payDate = new DateTime(); DateTime.TryParseExact(txtPayDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _payDate);
        if (_payDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter correct transaction date...", 2); return;
        }
        if (_payDate >= DateTime.MaxValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter correct transaction date...", 2); return;
        }
        int _paymentID = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentID); }
        if (_paymentID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select payment transaction mode...", 2); return;
        }
        int _bankID = 0; if (txtBankName.SelectedItem != null) { int.TryParse(txtBankName.SelectedItem.Value, out _bankID); }
        DateTime _chequeDate = new DateTime(); DateTime.TryParseExact(txtChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);

        SnehDLL.CashEntries_Dll AD = new SnehDLL.CashEntries_Dll();
        AD.LedgerID = 0; AD.UniqueID = ""; AD.AccountType = _accountType; AD.AccountNameID = _accountID;
        AD.CreditAmt = 0; AD.DebitAmt = _amount;
        AD.LinkLedgerID = -1; AD.PayMode = _paymentID; AD.Narration = txtNarration.Text.Trim();
        AD.PayDate = _payDate; AD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        AD.ModifyDate = DateTime.UtcNow.AddMinutes(330); AD.AddedBy = _loginID; AD.ModifyBy = _loginID;
        AD.BankID = _bankID; AD.ChequeDate = _chequeDate;
        if (_accountType == 1) { AD.HeadID = _accountID; } else { AD.HeadID = -1; }
        if (_accountType == 2) { AD.DoctorID = _accountID; } else { AD.DoctorID = -1; }
        if (_accountType == 3) { AD.PatientID = _accountID; } else { AD.PatientID = -1; }
        AD.ProductCatID = _categoryID; AD.ProductID = _productID;

        SnehBLL.CashEntries_Bll AB = new SnehBLL.CashEntries_Bll();
        int i = AB.NewCash(AD);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "Cash payment entry saved successfully...";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/CashEntry.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }

}
