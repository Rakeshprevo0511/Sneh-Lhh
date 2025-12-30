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
    public partial class ExpenseEntry : System.Web.UI.Page
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

                if (txtAccountType.Items.Count > 1)
                {
                    txtAccountType.SelectedIndex = 1;
                }


                LoadAccountName();
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
                DbHelper.Configuration.setAlert(Page, "Please select transaction payment mode...", 2); return;
            }
            int _bankID = 0; if (txtBankName.SelectedItem != null) { int.TryParse(txtBankName.SelectedItem.Value, out _bankID); }
            DateTime _chequeDate = new DateTime(); DateTime.TryParseExact(txtChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);

            if (_accountType == 1)
            {
                SnehBLL.AccountLedger_Bll AB = new SnehBLL.AccountLedger_Bll();
                int i = AB.NewExpense(_accountID, _amount, _payDate, _paymentID, txtNarration.Text.Trim(), _bankID, _chequeDate, _loginID);
                if (i > 0)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Payment detail saved successfully...";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";

                    Response.Redirect(ResolveClientUrl("~/Member/ExpenseEntry.aspx"), true);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                }
            }
            else if (_accountType == 2)
            {
                SnehBLL.DoctorLedger_Bll AB = new SnehBLL.DoctorLedger_Bll();
                int i = AB.NewExpense(_accountID, _amount, _payDate, _paymentID, txtNarration.Text.Trim(), _bankID, _chequeDate, _loginID);
                if (i > 0)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Payment detail saved successfully...";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";

                    Response.Redirect(ResolveClientUrl("~/Member/ExpenseEntry.aspx"), true);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
    }
}
