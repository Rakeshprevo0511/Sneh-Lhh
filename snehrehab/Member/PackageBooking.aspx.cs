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

public partial class Member_PackageBooking : System.Web.UI.Page
{
    int _loginID = 0; int _bookingID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            txtBookingDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            LoadForm();
        }
    }

    private void LoadForm()
    {
        SnehBLL.PatientMast_Bll PMB = new SnehBLL.PatientMast_Bll();
        txtPatient.Items.Clear(); txtPatient.Items.Add(new ListItem("Select Patient", "-1"));
        foreach (SnehDLL.PatientMast_Dll PMD in PMB.GetForDropdown())
        {
            txtPatient.Items.Add(new ListItem(PMD.FullName, PMD.PatientID.ToString()));
        }

        SnehBLL.SessionMast_Bll PSB = new SnehBLL.SessionMast_Bll();
        txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
        foreach (SnehDLL.SessionMast_Dll PSD in PSB.GetPackageList())
        {
            txtSession.Items.Add(new ListItem(PSD.SessionName, PSD.SessionID.ToString()));
        }
        txtPackage.Items.Clear(); txtPackage.Items.Add(new ListItem("Select Package", "-1"));

        SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
        txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
        foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
        {
            txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
        }
    }

    protected void txtPatient_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _patientID = 0; if (txtPatient.SelectedItem != null) { int.TryParse(txtPatient.SelectedItem.Value, out _patientID); }
        SnehBLL.PatientPackage_Bll PKB = new SnehBLL.PatientPackage_Bll();
        DataSet ds = PKB.PatientDetail(_patientID);
        if (ds.Tables.Count > 0)
        {
            PatientGV.DataSource = ds.Tables[0];
        }
        PatientGV.DataBind();
        if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

        LoadPackages();
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void txtSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPackages();
    }

    private void LoadPackages()
    {
        int _patientID = 0; txtPackageAmount.Text = ""; txtAppointmentCharge.Text = ""; txtAppointmentCount.Text = "";
        if (txtPatient.SelectedItem != null)
        {
            int.TryParse(txtPatient.SelectedItem.Value, out _patientID);
        }
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        txtPackage.Items.Clear(); txtPackage.Items.Add(new ListItem("Select Package", "-1"));
        if (_patientID > 0)
        {
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackage(_sessionID, _patientID))
            {
                txtPackage.Items.Add(new ListItem(PKD.PackageCode, PKD.PackageID.ToString()));
            }
        }
        CalculatePackageDiscount();
    }

    protected void txtPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPackageAmount.Text = ""; txtAppointmentCharge.Text = ""; txtAppointmentCount.Text = "";
        int _packageID = 0; if (txtPackage.SelectedItem != null) { int.TryParse(txtPackage.SelectedItem.Value, out _packageID); }
        SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
        SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
        if (PKD != null)
        {
            txtPackageAmount.Text = PKD.PackageAmt.ToString();
            txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
            txtAppointmentCount.Text = PKD.Appointments.ToString();
        }
        CalculatePackageDiscount();
    }

    protected void txtPackageIsDiscounted_CheckedChanged(object sender, EventArgs e)
    {
        txtPackageDiscountedOn.Enabled = false; txtPackageDiscountType.Enabled = false;
        tbPackageDiscount.Visible = false; tbPackageDiscountSessionCh.Visible = false; txtPackageSessionChargeNew.Text = "";
        txtPackageDiscountValue.Text = ""; txtPackageNetAmt.Text = ""; txtPackageSessionChargeNew.Text = "";
        if (txtPackageDiscountedOn.Items.Count > 0) { txtPackageDiscountedOn.SelectedIndex = 0; }
        if (txtPackageDiscountType.Items.Count > 0) { txtPackageDiscountType.SelectedIndex = 0; }
        if (txtPackageIsDiscounted.Checked)
        {
            txtPackageDiscountType.Enabled = true; txtPackageDiscountedOn.Enabled = true;
        }
    }

    protected void txtPackageDiscountedOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbPackageDiscount.Visible = false; tbPackageDiscountSessionCh.Visible = false; txtPackageSessionChargeNew.Text = "";
        int _discountOn = 0; if (txtPackageDiscountedOn.SelectedItem != null) { int.TryParse(txtPackageDiscountedOn.SelectedItem.Value, out _discountOn); }
        int _discountType = 0; if (txtPackageDiscountType.SelectedItem != null) { int.TryParse(txtPackageDiscountType.SelectedItem.Value, out _discountType); }
        if (_discountOn > 0 && _discountType > 0)
        {
            tbPackageDiscount.Visible = true;
            CalculatePackageDiscount();
        }
        else
        {
            txtPackageDiscountValue.Text = ""; txtPackageNetAmt.Text = ""; txtPackageSessionChargeNew.Text = "";
        }
    }

    protected void txtPackageDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbPackageDiscount.Visible = false; tbPackageDiscountSessionCh.Visible = false; txtPackageSessionChargeNew.Text = "";
        int _discountOn = 0; if (txtPackageDiscountedOn.SelectedItem != null) { int.TryParse(txtPackageDiscountedOn.SelectedItem.Value, out _discountOn); }
        int _discountType = 0; if (txtPackageDiscountType.SelectedItem != null) { int.TryParse(txtPackageDiscountType.SelectedItem.Value, out _discountType); }
        if (_discountType > 0 && _discountOn > 0)
        {
            tbPackageDiscount.Visible = true;
            CalculatePackageDiscount();
        }
        else
        {
            txtPackageDiscountValue.Text = ""; txtPackageNetAmt.Text = ""; txtPackageSessionChargeNew.Text = "";
        }
    }

    protected void txtPackageDiscountValue_TextChanged(object sender, EventArgs e)
    {
        CalculatePackageDiscount();
    }

    private void CalculatePackageDiscount()
    {
        tbPackageDiscountSessionCh.Visible = false; txtPackageSessionChargeNew.Text = "";
        int _discountOn = 0; if (txtPackageDiscountedOn.SelectedItem != null) { int.TryParse(txtPackageDiscountedOn.SelectedItem.Value, out _discountOn); }
        int _discountType = 0; if (txtPackageDiscountType.SelectedItem != null) { int.TryParse(txtPackageDiscountType.SelectedItem.Value, out _discountType); }
        float _discountValue = 0; float.TryParse(txtPackageDiscountValue.Text, out _discountValue);
        float _packageAmt = 0; float.TryParse(txtPackageAmount.Text, out _packageAmt);
        txtPackageNetAmt.Text = "";
        if (_discountValue > 0 && _discountType > 0 && _packageAmt > 0)
        {
            float _disccountAmt = 0;
            if (_discountType == 1)    //  PERCENT
            {
                float.TryParse(Math.Round((decimal.Parse(_packageAmt.ToString()) * decimal.Parse(_discountValue.ToString()) / 100), 2).ToString(), out _disccountAmt);
                txtPackageNetAmt.Text = (_packageAmt - _disccountAmt).ToString();
            }
            if (_discountType == 2)    //  RUPEES
            {
                _disccountAmt = _discountValue;
                txtPackageNetAmt.Text = (_packageAmt - _disccountAmt).ToString();
            }
            if (_discountOn == 1)
            {
                tbPackageDiscountSessionCh.Visible = true;
                float _netAmt = 0; float.TryParse(txtPackageNetAmt.Text, out _netAmt);
                float _appointmentCount = 0; float.TryParse(txtAppointmentCount.Text.Trim(), out _appointmentCount);
                float _newSessionCh = float.Parse(Math.Round(decimal.Parse(_netAmt.ToString()) / decimal.Parse(_appointmentCount.ToString()), 2).ToString());
                txtPackageSessionChargeNew.Text = _newSessionCh.ToString();
            }
        }
    }

    protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        tab_Cheque.Visible = false;tab_online.Visible = false; if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; } txtChequeDate.Text = "";
        if (_paymentMode == 1)
        {
            tab_cash_Credit.Visible = true;
        }
        if (_paymentMode == 2)
        {
            tab_cash_Credit.Visible = true;
        }
        if (_paymentMode == 3)
        {
            tab_Cheque.Visible = true;
            tab_cash_Credit.Visible = false;
        }
        if (_paymentMode == 4)
        {
            tab_online.Visible = true;
            tab_cash_Credit.Visible = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int _patientID = 0; if (txtPatient.SelectedItem != null) { int.TryParse(txtPatient.SelectedItem.Value, out _patientID); }
        if (_patientID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select patient, and try again...", 2); return;
        }
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        if (_sessionID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select session...", 2); return;
        }
        int _packageID = 0; if (txtPackage.SelectedItem != null) { int.TryParse(txtPackage.SelectedItem.Value, out _packageID); }
        if (_packageID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select package...", 2); return;
        }
        float _appointmentCharge = 0; float.TryParse(txtAppointmentCharge.Text.Trim(), out _appointmentCharge);
        float _appointmentCount = 0; float.TryParse(txtAppointmentCount.Text.Trim(), out _appointmentCount);
        float _packageAmount = 0; float.TryParse(txtPackageAmount.Text.Trim(), out _packageAmount);
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        int _bankID = 0; DateTime _chequeDate = new DateTime(); string BankBranch = string.Empty; string ChequeTxnNo = string.Empty;
        string HospitalReceiptID = string.Empty; DateTime HospitalReceiptDate = DateTime.MinValue;
        
        if (_paymentMode == 1 || _paymentMode == 2)   
        {
            HospitalReceiptID = txtHospitalReceiptID.Text.Trim();

            DateTime.TryParseExact( txtHospitalReceiptDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture,  DateTimeStyles.None, out HospitalReceiptDate
            );

            if (string.IsNullOrWhiteSpace(HospitalReceiptID))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter Hospital Receipt ID...", 2); return;
            }

            if (HospitalReceiptDate == DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select Hospital Receipt Date...", 2); return;
            }
        }
       
        if (_paymentMode == 3)
        {
            if (txtBankName.SelectedItem != null) { int.TryParse(txtBankName.SelectedItem.Value, out _bankID); }
            BankBranch = txtBranchName.Text.Trim(); ChequeTxnNo = txtChequeNo.Text.Trim();
            DateTime.TryParseExact(txtChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (_bankID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select bank name...", 2); return;
            }
            if (string.IsNullOrEmpty(ChequeTxnNo))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter cheque number...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
            }
        }
        if (_paymentMode == 4)
        {
            ChequeTxnNo = txtTransactionID.Text.Trim();
            DateTime.TryParseExact(txtTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            if (string.IsNullOrEmpty(ChequeTxnNo))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter transaction id...", 2); return;
            }
            if (_chequeDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            }
            if (_chequeDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            }
        }
        string _narration = txtNarration.Text.Trim();
        bool _isDiscounted = txtPackageIsDiscounted.Checked;
        int _discountType = 0; float _discountValue = 0; float _discountAmt = 0; float _netAmt = 0;  int _discountedOn = 0; float _newSessionCharge = 0;
        _netAmt = _packageAmount;
        if (_isDiscounted)
        {
            if (txtPackageDiscountedOn.SelectedItem != null) { int.TryParse(txtPackageDiscountedOn.SelectedItem.Value, out _discountedOn); }
            if (_discountedOn <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select discount on session or package...", 2); return;
            }
            if (txtPackageDiscountType.SelectedItem != null) { int.TryParse(txtPackageDiscountType.SelectedItem.Value, out _discountType); }
            if (_discountType <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select discount type...", 2); return;
            }
            float.TryParse(txtPackageDiscountValue.Text, out _discountValue);
            if (_discountValue <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter discount value...", 2); return;
            }
            if (_discountType == 1)    //  PERCENT
            {
                float.TryParse(Math.Round((decimal.Parse(_packageAmount.ToString()) * decimal.Parse(_discountValue.ToString()) / 100), 2).ToString(), out _discountAmt);
                _netAmt = (_packageAmount - _discountAmt);
            }
            if (_discountType == 2)    //  RUPEES
            {
                _discountAmt = _discountValue;
                _netAmt = (_packageAmount - _discountAmt);
            }
            if (_discountedOn == 1)    //  CALC NEW SESSION CHARGE IF SESSION DISCOUNT
            {
                float _newSessionCh = float.Parse(Math.Round(decimal.Parse(_netAmt.ToString()) / decimal.Parse(_appointmentCount.ToString()), 2).ToString());
                _newSessionCharge = _newSessionCh;
            }
        }
        if (txtBookingDate.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select patient package entry date...", 2); return;
        }
        DateTime _entryDate = new DateTime(); DateTime.TryParseExact(txtBookingDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _entryDate);
        if (_entryDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select proper patient package entry date...", 2); return;
        }
        if (_entryDate >= DateTime.MaxValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select proper patient package entry date...", 2); return;
        }
        SnehDLL.PatientPackage_Dll PPD = new SnehDLL.PatientPackage_Dll();
        PPD.BookingID = _bookingID; PPD.UniqueID = "";
        PPD.PatientID = _patientID; PPD.SessionID = _sessionID;
        PPD.PackageID = _packageID; PPD.AppointmentCharge = _appointmentCharge;
        PPD.AppointmentCount = _appointmentCount; PPD.PackageAmount = _packageAmount;
        PPD.ModePayment = _paymentMode; PPD.BankID = _bankID;
        PPD.Narration = _narration; PPD.ChequeDate = _chequeDate;
        PPD.AddedDate = _entryDate; PPD.ModifyDate = _entryDate;
        PPD.AddedBy = _loginID; PPD.ModifyBy = _loginID;
        PPD.ExtraCharge = 0; PPD.ChequeTxnNo = ChequeTxnNo; PPD.BankBranch = BankBranch;
        PPD.IsDiscounted = _isDiscounted; PPD.DiscountType = _discountType;
        PPD.DiscountValue = _discountValue; PPD.DiscountAmt = _discountAmt;
        PPD.NetAmt = _netAmt;
        PPD.DiscountedOn = _discountedOn; PPD.NewSessionCharge = _newSessionCharge;
        PPD.HospitalReceiptID = HospitalReceiptID; PPD.HospitalReceiptDate = HospitalReceiptDate;

        SnehBLL.PatientPackage_Bll PPB = new SnehBLL.PatientPackage_Bll();
        int i = PPB.Set(PPD);
        if (i > 0)
        {
            if (_bookingID <= 0) //ADD PACKAGE PAYMENT
            {
                SnehBLL.PatientLedger_Bll PLB = new SnehBLL.PatientLedger_Bll();
                PLB.NewPackageBooking(i);
            }

            Session[DbHelper.Configuration.messageTextSession] = "Patient package booking saved successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/PackageBooking.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}























 