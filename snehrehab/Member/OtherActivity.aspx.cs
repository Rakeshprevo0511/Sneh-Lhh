using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
namespace snehrehab.Member
{
    public partial class OtherActivity : System.Web.UI.Page
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
                txtOtherActPayDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
                txtOtherActBankName.Items.Clear(); txtOtherActBankName.Items.Add(new ListItem("Select Bank", "-1"));
                foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
                {
                    txtOtherActBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
                }
                LoadAccountName();
                txtOtherActProductCategory.Items.Clear(); txtOtherActProductCategory.Items.Add(new ListItem("Select Category", "-1"));
                SnehBLL.OtherActCategory_BLL OCB = new SnehBLL.OtherActCategory_BLL();
                foreach (SnehDLL.OtherActCategory_DLL OCD in OCB.GetList())
                {
                    txtOtherActProductCategory.Items.Add(new ListItem(OCD.CategoryName, OCD.CategoryID.ToString()));
                }
                txtOtherAct_Doctor.Items.Clear(); txtOtherAct_Doctor.Items.Add(new ListItem("Select Doctor", "-1"));
                txtOtherAct_Ass_Doctor.Items.Clear(); txtOtherAct_Ass_Doctor.Items.Add(new ListItem("Select Assistant Doctor", "-1"));
                SnehBLL.DoctorMast_Bll AB = new SnehBLL.DoctorMast_Bll();
                foreach (SnehDLL.DoctorMast_Dll AD in AB.GetForDropdown())
                {
                    txtOtherAct_Doctor.Items.Add(new ListItem(AD.PreFix + " " + AD.FullName, AD.DoctorID.ToString()));
                    txtOtherAct_Ass_Doctor.Items.Add(new ListItem(AD.PreFix + " " + AD.FullName, AD.DoctorID.ToString()));
                }
                LoadProducts();
            }
        }

        protected void txtOtherActProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            float _unitPrice = 0;
            int productid = 0; if (txtOtherActProductName.SelectedItem != null) { int.TryParse(txtOtherActProductName.SelectedItem.Value, out productid); }
            SqlCommand cmd = new SqlCommand("Select UnitPrice from OtherAct_Product where ProductID = @ProductID");
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productid; cmd.CommandType = CommandType.Text;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);

            if (SnehBLL.UserAccount_Bll.getCategory() == 1)
            {
                txtOtherActAmount.ReadOnly = false;
                if (dt.Rows.Count > 0)
                {
                    float.TryParse(dt.Rows[0]["UnitPrice"].ToString(), out _unitPrice);
                    txtOtherActAmount.Text = _unitPrice.ToString();

                }
            }
            else
            {
                txtOtherActAmount.ReadOnly = true;
                if (dt.Rows.Count > 0)
                {
                    float.TryParse(dt.Rows[0]["UnitPrice"].ToString(), out _unitPrice);
                    txtOtherActAmount.Text = _unitPrice.ToString();
                }
            }
        }

        private void LoadProducts()
        {
            txtOtherActProductName.Items.Clear(); txtOtherActProductName.Items.Add(new ListItem("Select Product", "-1"));
            SnehBLL.OtherActProduct_BLL OAB = new SnehBLL.OtherActProduct_BLL();
            int _categoryid = 0; if (txtOtherActProductCategory.SelectedItem != null) { int.TryParse(txtOtherActProductCategory.SelectedItem.Value, out _categoryid); }
            foreach (SnehDLL.OtherActProduct_DLL OAD in OAB.GetList(_categoryid))
            {
                txtOtherActProductName.Items.Add(new ListItem(OAD.ProductName, OAD.ProductID.ToString()));
            }
        }

        protected void txtOtherActAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAccountName();
        }

        private void LoadAccountName()
        {
            int _accountType = 0; if (txtOtherActAccountType.SelectedItem != null) { int.TryParse(txtOtherActAccountType.SelectedItem.Value, out _accountType); }
            txtOtherActAccountName.Items.Clear(); txtOtherActAccountName.Items.Add(new ListItem("Select Account Name", "-1"));
            txtOtherActAccountName.Visible = true; textOtherAct_AccountName.Visible = false; textOtherAct_AccountName.Text = "";
            txtOtherActProductCategory.Visible = true; textOtherAct_ProductCategory.Visible = false; textOtherAct_ProductCategory.Text = "";
            txtOtherActProductName.Visible = true; textOtherAct_ProductName.Visible = false; textOtherAct_ProductName.Text = "";
            txtOtherAct_Doctor.Visible = true; textOtherAct_Doctor.Visible = false; textOtherAct_Doctor.Text = "";
            txtOtherAct_Ass_Doctor.Visible = true; textOtherAct_Ass_Doctor.Visible = false; textOtherAct_Ass_Doctor.Text = "";
            //if (_accountType == 1)
            //{
            //    SnehBLL.AccountHeads_Bll AB = new SnehBLL.AccountHeads_Bll();
            //    foreach (SnehDLL.AccountHeads_Dll AD in AB.GetList())
            //    {
            //        txtOtherActAccountName.Items.Add(new ListItem(AD.HeadName, AD.HeadID.ToString()));
            //    }
            //}
            //else if (_accountType == 2)
            //{
            //    SnehBLL.DoctorMast_Bll AB = new SnehBLL.DoctorMast_Bll();
            //    foreach (SnehDLL.DoctorMast_Dll AD in AB.GetForDropdown())
            //    {
            //        txtOtherActAccountName.Items.Add(new ListItem(AD.PreFix + " " + AD.FullName, AD.DoctorID.ToString()));
            //    }
            //}
            if (_accountType == 3)
            {
                SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
                foreach (SnehDLL.PatientMast_Dll PD in PB.GetForDropdown())
                {
                    if (PD.PatientTypeID != 3)
                    {
                        txtOtherActAccountName.Items.Add(new ListItem(PD.FullName, PD.PatientID.ToString()));
                    }
                }
            }
        }

        protected void txtOtherActProductCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void txtOtherActPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_SessionBank.Visible = false; tab_online.Visible = false;
            int _paymentType = 0; if (txtOtherActPaymentType.SelectedItem != null) { int.TryParse(txtOtherActPaymentType.SelectedItem.Value, out _paymentType); }
            if (_paymentType == 3)
            {
                tb_SessionBank.Visible = true;
            }
            if (_paymentType == 4)
            {
                tab_online.Visible = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            int _accountType = 0; if (txtOtherActAccountType.SelectedItem != null) { int.TryParse(txtOtherActAccountType.SelectedItem.Value, out _accountType); }
            if (_accountType <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select account type.", 2); return;
            }
            int _accountID = 0; if (txtOtherActAccountName.SelectedItem != null) { int.TryParse(txtOtherActAccountName.SelectedItem.Value, out _accountID); }
            string accountname = txtOtherActAccountName.SelectedItem.ToString();
            if (_accountID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select account name.", 2); return;
            }

            int _doctorid = 0; if (txtOtherAct_Doctor.SelectedItem.Value != null) { int.TryParse(txtOtherAct_Doctor.SelectedItem.Value, out _doctorid); }
            string doctor = txtOtherAct_Doctor.SelectedItem.ToString();
            if (_doctorid <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select Doctor.", 2); return;
            }
            int _assdoctorid = 0; if (txtOtherAct_Ass_Doctor.SelectedItem.Value != null) { int.TryParse(txtOtherAct_Ass_Doctor.SelectedItem.Value, out _assdoctorid); }
            string assdoctor = txtOtherAct_Ass_Doctor.SelectedItem.ToString();
            if (_assdoctorid == -1)
            {
                assdoctor = "";
            }
            int _categoryID = 0; if (txtOtherActProductCategory.SelectedItem != null) { int.TryParse(txtOtherActProductCategory.SelectedItem.Value, out _categoryID); }
            string productcategory = txtOtherActProductCategory.SelectedItem.ToString();
            if (_categoryID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select product category.", 2); return;
            }

            int _productID = 0; if (txtOtherActProductName.SelectedItem != null) { int.TryParse(txtOtherActProductName.SelectedItem.Value, out _productID); }
            string productname = txtOtherActProductName.SelectedItem.ToString();
            if (_productID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select product name.", 2); return;
            }

            float _amount = 0; float.TryParse(txtOtherActAmount.Text.Trim(), out _amount);
            if (_amount <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter transaction amount.", 2); return;
            }
            DateTime _payDate = new DateTime();
            DateTime.TryParseExact(txtOtherActPayDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _payDate);
            int _paymentType = 0; if (txtOtherActPaymentType.SelectedItem != null) { int.TryParse(txtOtherActPaymentType.SelectedItem.Value, out _paymentType); }
            if (_paymentType == 4)
            {
                if (txtOtherActTransactionID.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select transaction Id.", 2); return;
                }
                
                if (_payDate <= DateTime.MinValue)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter correct transaction date.", 2); return;
                }
                if (_payDate >= DateTime.MaxValue)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter correct transaction date.", 2); return;
                }
            }
            if (_paymentType == 3)
            {
                if (txtOtherActBankName.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select benk name.", 2); return;
                }
                DateTime.TryParseExact(txtOtherActChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _payDate);
                if (_payDate <= DateTime.MinValue)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter correct cheque date.", 2); return;
                }
                if (txtOtherActChequeNumber.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter cheque number.", 2); return;
                }

            }
            int _paymentID = 0; if (txtOtherActPaymentType.SelectedItem != null) { int.TryParse(txtOtherActPaymentType.SelectedItem.Value, out _paymentID); }
            if (_paymentID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select payment transaction mode.", 2); return;
            }
            int _bankID = 0; if (txtOtherActBankName.SelectedItem != null) { int.TryParse(txtOtherActBankName.SelectedItem.Value, out _bankID); }
            DateTime _chequeDate = new DateTime(); DateTime.TryParseExact(txtOtherActChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            string chequenumber = txtOtherActChequeNumber.Text;
            string bankbranch = txtOtherActBranchName.Text;
            string _onlinetransactionid = ""; if (txtOtherActTransactionID.Text != "" && txtOtherActTransactionID.Text != null) { _onlinetransactionid = txtOtherActTransactionID.Text.Trim(); }
            DateTime _onlinetransactiondate = new DateTime(); DateTime.TryParseExact(TextOthActTran_Date.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _onlinetransactiondate);
            SnehDLL.OtherAct_CashEntry_DLL OCD = new SnehDLL.OtherAct_CashEntry_DLL();
            OCD.LedgerID = 0; OCD.UniqueID = ""; OCD.AccountType = _accountType; OCD.AccountNameID = _accountID;
            OCD.CreditAmt = 0; OCD.DebitAmt = _amount;
            OCD.LinkLedgerID = -1; OCD.PayMode = _paymentID; OCD.Narration = txtOtherActNarration.Text.Trim();
            OCD.PayDate = _payDate; OCD.AddedDate = DateTime.UtcNow.AddMinutes(330);
            OCD.ModifyDate = DateTime.UtcNow.AddMinutes(330); OCD.AddedBy = _loginID; OCD.ModifyBy = _loginID;
            OCD.BankID = _bankID; OCD.ChequeDate = _chequeDate;
            OCD.ChequeTxnNo = chequenumber; OCD.BankBranch = bankbranch;
            if (_accountType == 1) { OCD.HeadID = _accountID; } else { OCD.HeadID = -1; }
            if (_accountType == 2) { OCD.Account_NameDoctorID = _accountID; } else { OCD.Account_NameDoctorID = -1; }
            if (_accountType == 3) { OCD.Account_NamePatientID = _accountID; } else { OCD.Account_NamePatientID = -1; }
            OCD.ProductCatID = _categoryID; OCD.ProductID = _productID;
            OCD.Doctor = doctor.Trim(); OCD.DoctorID = _doctorid;
            OCD.Ass_Doctor = assdoctor.Trim(); OCD.Ass_DoctorID = _assdoctorid;
            OCD.AccountName = accountname.Trim(); OCD.ProductCategory = productcategory.Trim();
            OCD.ProductName = productname.Trim();
            OCD.Online_TransactionID = _onlinetransactionid;
            OCD.Online_TransactionDate = _onlinetransactiondate;

            SnehBLL.OtherAct_CashEntry_BLL OCB = new SnehBLL.OtherAct_CashEntry_BLL();
            int i = OCB.set(OCD);
            if (SnehBLL.UserAccount_Bll.getCategory() == 1)
            {
                int productid = 0; if (txtOtherActProductName.SelectedItem != null) { int.TryParse(txtOtherActProductName.SelectedItem.Value, out productid); }
                SqlCommand CmnUpdate = new SqlCommand("UPDATE OtherAct_Product SET UnitPrice = @UnitPrice WHERE ProductID = @ProductID"); CmnUpdate.CommandType = CommandType.Text;
                CmnUpdate.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = _amount;
                CmnUpdate.Parameters.Add("@ProductID", SqlDbType.BigInt).Value = _productID;
                DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbUpdate(CmnUpdate);
            }
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Other Activity Entry Saved Successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/OtherActivity.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again.", 2);
            }


        }
    }
}