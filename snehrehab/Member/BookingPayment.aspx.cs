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
using System.Data.SqlClient;

public partial class Member_BookingPayment : System.Web.UI.Page
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
            txtPaymentMode.DataBind();
            SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
            txtPaymentBankName.Items.Clear(); txtPaymentBankName.Items.Add(new ListItem("Select Bank", "-1"));
            foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
            {
                txtPaymentBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
            }

            txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                     1// DateTime.UtcNow.AddMinutes(330).Day
                      ).ToString(DbHelper.Configuration.showDateFormat);
            txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            if (Request.QueryString["search"] != null) { if (Request.QueryString["search"].ToString().Length > 0) { txtSearch.Text = Request.QueryString["search"].ToString(); } }
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PatientGV.PageIndex = 0; LoadData();
    }

    protected void PatientGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PatientGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        SnehBLL.PatientLedger_Bll DB = new SnehBLL.PatientLedger_Bll();
        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

        PatientGV.DataSource = DB.PackageBalance(txtSearch.Text.Trim(), _fromDate, _uptoDate);
        PatientGV.DataBind();
        if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        if (SnehBLL.UserAccount_Bll.getCategory() == 4)
        {
            PatientGV.Columns[8].Visible = false;
        }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void btnPayAmount_Click(object sender, EventArgs e)
    {
        int _ledgerID = 0; 
        if (txtHiddenPayment.Value.Length > 0)
        {
            if (DbHelper.Configuration.IsGuid(txtHiddenPayment.Value))
            {
                _ledgerID = SnehBLL.PatientLedger_Bll.Check(txtHiddenPayment.Value);
            }
        }
        if (_ledgerID <= 0)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Unable to find session / package detail, please try again...", 2); return;
        }
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        int _bankID = 0; DateTime _chequeDate = new DateTime(); string ChequeTxnNo = string.Empty; string BankBranch = string.Empty;
        if (_paymentMode == 3)
        {
            if (txtPaymentBankName.SelectedItem != null) { int.TryParse(txtPaymentBankName.SelectedItem.Value, out _bankID); }
            BankBranch = txtBankBranch.Text.Trim(); ChequeTxnNo = txtChequeNo.Text.Trim();
            DateTime.TryParseExact(txtPaymentChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (_bankID <= 0)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please select bank name...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please select correct bank cheque date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please select correct bank cheque date...", 2); return;
            }
        }
        if (_paymentMode == 4)
        {
            ChequeTxnNo = txtTransactionID.Text.Trim();
            DateTime.TryParseExact(txtTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (string.IsNullOrEmpty(ChequeTxnNo))
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please enter transaction id...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please select correct transaction date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Please select correct transaction date...", 2); return;
            }
        }
        float _amount = 0; float.TryParse(txtPaymentAmount.Text.Trim(), out _amount);
        if (_amount <= 0)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Please enter pay amount and try again...", 2); return;
        }
        DateTime _payDate = new DateTime(); DateTime.TryParseExact(txtPaymentDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _payDate);
        if (_payDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Please select payment date...", 2); return;
        }
        if (_payDate >= DateTime.MaxValue)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Please select payment date...", 2); return;
        }
        string _narration = txtPaymentNarration.Text.Trim();

        SnehBLL.PatientLedger_Bll PLB = new SnehBLL.PatientLedger_Bll();
        int i = PLB.PayPackage(_ledgerID, _amount, _payDate, _paymentMode, _bankID, BankBranch, ChequeTxnNo, _chequeDate, _narration);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "Payment entry added successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";
            if (txtSearch.Text.Trim().Length > 0)
            {
                Response.Redirect(ResolveClientUrl("~/Member/BookingPayment.aspx?search=" + txtSearch.Text.Trim() + ""), true);
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/BookingPayment.aspx"), true);
            }
        }
        else if (i == -1)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Please check balance amount of patient...", 2); return;
        }
        else
        {
            DbHelper.Configuration.setAlert(txtMsg, "Unable to process your request, please try again...", 2); return;
        }
    }

    protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        tab_Cheque.Visible = false; tab_online.Visible = false;
        if (txtPaymentBankName.Items.Count > 0) { txtPaymentBankName.SelectedIndex = 0; }
        txtBankBranch.Text = ""; txtChequeNo.Text = ""; txtPaymentChequeDate.Text = "";
        txtTransactionID.Text = ""; txtTransactionDate.Text = "";
        if (_paymentMode == 3)
        {
            tab_Cheque.Visible = true;
        }
        if (_paymentMode == 4)
        {
            tab_online.Visible = true;
        }
        if (_paymentMode == 100)
        {
            tab_Bulk.Visible = true;
            int _patientID = 0; if (txtHiddenPayment.Value.Length > 0)
            {
                if (DbHelper.Configuration.IsGuid(txtHiddenPayment.Value))
                {
                    _patientID = SnehBLL.PatientMast_Bll.BulkCheck(txtHiddenPayment.Value);
                }
            }
            if (_patientID > 0)
            {
                SqlCommand cmd = new SqlCommand("SELECT P.BulkID,(P.Amount - COALESCE((SELECT SUM(COALESCE(APS.AppointmentCharge, 0)) FROM AppointmentSession APS WHERE APS.BulkBookingID = P.BulkID), 0))AS BalanceAmount FROM PatientBulk P WHERE P.PatientID=@PatientID AND COALESCE(P.IsPackage,CAST('False'AS BIT))=CAST('False'AS BIT)");
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
                DataTable dt = new DataTable(); DbHelper.SqlDb db = new DbHelper.SqlDb();
                dt = db.DbRead(cmd); float bulkamount = 0;
                if (dt.Rows.Count > 0)
                {
                    float.TryParse(dt.Rows[0]["BalanceAmount"].ToString(), out bulkamount);
                    long bulkid = 0; long.TryParse(dt.Rows[0]["BulkID"].ToString(), out bulkid);
                    txthidbulkid.Value = bulkid.ToString();
                }
                if (bulkamount > 0)
                {
                    txtbulkamount.Text = bulkamount.ToString();
                }
                else
                {
                    txtbulkamount.Text = bulkamount.ToString();
                    DbHelper.Configuration.setAlert(txtMsg, "No Bulk Booking Available...", 2); return;
                }
            }
        }
    }

    //protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
    //    tab_Cheque.Visible = false; tab_online.Visible = false; if (txtPaymentBankName.Items.Count > 0) { txtPaymentBankName.SelectedIndex = 0; } txtPaymentChequeDate.Text = "";
    //    if (_paymentMode == 3)
    //    {
    //        tab_Cheque.Visible = true;
    //    }
    //    if (_paymentMode == 4)
    //    {
    //        tab_online.Visible = true;
    //    }
    //}
}
