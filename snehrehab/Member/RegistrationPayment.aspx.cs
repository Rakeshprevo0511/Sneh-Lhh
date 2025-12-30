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

public partial class Member_RegistrationPayment : System.Web.UI.Page
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
        PatientGV.DataSource = DB.RegistrationBalance(txtSearch.Text.Trim());
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
        int _patientID = 0; if (txtHiddenPayment.Value.Length > 0)
        {
            if (DbHelper.Configuration.IsGuid(txtHiddenPayment.Value))
            {
                _patientID = SnehBLL.PatientMast_Bll.Check(txtHiddenPayment.Value);
            }
        }
        if (_patientID <= 0)
        {
            DbHelper.Configuration.setAlert(txtMsg, "Unable to find patient detail, please try again...", 2); return;
        }
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        string ChequeTxnNo = string.Empty; DateTime _chequeDate = new DateTime();
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
        string _narration = txtPaymentNarration.Text.Trim();
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
        long bulkbookingid = 0;
        if (_paymentMode == 100)
        {
            float bulkamount = 0; float.TryParse(txtbulkamount.Text.ToString(), out bulkamount);
            float _regamount = 0; float.TryParse(txtPaymentAmount.Text.Trim(), out _regamount);
            if (bulkamount < _regamount)
            {
                DbHelper.Configuration.setAlert(txtMsg, "Your bulk package amount is less...", 2); return;
            }
            long.TryParse(txthidbulkid.Value, out bulkbookingid);
        }
        SnehBLL.PatientLedger_Bll PLB = new SnehBLL.PatientLedger_Bll();
        int i = PLB.PayRegistration(_patientID, _amount, _payDate, ChequeTxnNo, _paymentMode, bulkbookingid, _narration);
        if (i > 0)
        { 
            Session[DbHelper.Configuration.messageTextSession] = "Payment entry added successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";
            if (txtSearch.Text.Trim().Length > 0)
            {
                Response.Redirect(ResolveClientUrl("~/Member/RegistrationPayment.aspx?search=" + txtSearch.Text.Trim() + ""), true);
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/RegistrationPayment.aspx"), true);
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
        tab_online.Visible = false; tab_Bulk.Visible = false;

        txtTransactionID.Text = ""; txtTransactionDate.Text = ""; txtbulkamount.Text = "";
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
                    _patientID = SnehBLL.PatientMast_Bll.Check(txtHiddenPayment.Value);
                }
            }
            if (_patientID > 0)
            {
                SqlCommand cmd = new SqlCommand("SELECT P.BulkID,(P.Amount - COALESCE((SELECT SUM(COALESCE(APS.AppointmentCharge, 0)) FROM AppointmentSession APS WHERE APS.BulkBookingID = P.BulkID), 0))AS BalanceAmount FROM PatientBulk P WHERE P.PatientID=@PatientID AND COALESCE(P.IsPackage,CAST('False'AS BIT))=CAST('False'AS BIT)");
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
                DataTable dt = new DataTable(); DbHelper.SqlDb db = new DbHelper.SqlDb();
                dt = db.DbRead(cmd); float bulkamount = 0; float _bulkAmount = 0;
                //if (dt.Rows.Count > 0)
                //{
                //    float.TryParse(dt.Rows[0]["BalanceAmount"].ToString(), out bulkamount);
                //    long bulkid = 0; long.TryParse(dt.Rows[0]["BulkID"].ToString(), out bulkid);
                //    txthidbulkid.Value = bulkid.ToString();
                //}
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    float.TryParse(dt.Rows[i]["BalanceAmount"].ToString(), out bulkamount);
                    _bulkAmount += bulkamount;
                    long bulkid = 0; long.TryParse(dt.Rows[i]["BulkID"].ToString(), out bulkid);
                    txthidbulkid.Value = bulkid.ToString();
                }
                if (bulkamount > 0)
                {
                    txtbulkamount.Text = _bulkAmount.ToString();
                    // txtbulkamount.Text = bulkamount.ToString();
                }
                else
                {
                    txtbulkamount.Text = bulkamount.ToString();
                    DbHelper.Configuration.setAlert(txtMsg, "No Bulk Booking Available...", 2); return;
                }
            }
        }
    }
}
