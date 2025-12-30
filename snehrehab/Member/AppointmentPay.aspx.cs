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
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class Member_AppointmentPay : System.Web.UI.Page
{
    int _loginID = 0; int _appointmentID = 0; SnehDLL.Appointments_Dll AD;
    int toReturn = 0; public string returnUrl = string.Empty;

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
                _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (Request.QueryString["return"] != null)
        {
            int.TryParse(Request.QueryString["return"].ToString(), out toReturn);
        }
        if (toReturn == 101)
            returnUrl = "/Member/AppointmentChart.aspx";
        else
            returnUrl = "/Member/Appointments.aspx";

        if (_appointmentID > 0)
        {
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            AD = AB.Get(_appointmentID);
            if (AD != null)
            {
                if (!AD.IsDeleted)
                {
                    if (AD.AppointmentStatus != 0)
                    {
                        if (toReturn == 101)
                            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                        else
                            Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                    }
                }
                else
                {
                    if (toReturn == 101)
                        Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                    else
                        Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                }
            }
            else
            {
                if (toReturn == 101)
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
            }
        }
        else
        {
            if (toReturn == 101)
                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
            else
                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
        }
        if (!IsPostBack)
        {
            lblEntryDateTime.Text = "(" + DbHelper.Configuration.showDateFormat + ")";
            LoadForm();

            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            if (!SMB.chargeIsUpdated(AD.SessionID))
            {
                DbHelper.Configuration.setAlert(Page, "Doctor session charges is not updated. <a href=\"/Member/SessionChrges.aspx\" class=\"alert-link\">Click here</a> to update.", 2);
            }
            if (AD.AppointmentDate > DateTime.MinValue)
                txtEntryDateTime.Text = AD.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat);
            else
                txtEntryDateTime.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
            txtBookingDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
        }
    }

    private void LoadForm()
    {
        tb_Contents.ActiveTabIndex = 0; tb_Session.Enabled = true; tb_Packages.Enabled = false;
        SnehBLL.Appointments_Bll PKB = new SnehBLL.Appointments_Bll();
        DataSet ds = PKB.SessionDetail(_appointmentID);
        if (ds.Tables.Count > 0)
        {
            PatientGV.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                int PatientTypeID = 0; int.TryParse(ds.Tables[0].Rows[0]["PatientTypeID"].ToString(), out PatientTypeID);
                //if (PatientTypeID == 3)
                //{
                //    Response.Redirect(ResolveClientUrl("~/Member/EnquiryReg.aspx?record=" + ds.Tables[0].Rows[0]["UniqueID"].ToString() + "&apt=" + AD.UniqueID), true);
                //    return;
                //}
            }
        }
        PatientGV.DataBind();
        if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        if (ds.Tables.Count > 1)
        {
            SessionGV.DataSource = ds.Tables[1];
        }
        SessionGV.DataBind();
        if (SessionGV.HeaderRow != null) { SessionGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

        txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
        tb_SessionBank.Visible = false; tb_SessionOtherPackages.Visible = false; tb_SessionOnline.Visible = false;
        tabPaymentModes.Visible = true;
        //tabPaymentBulkMode.Visible = false;
        bulk.Visible = false; bulkcarry.Visible = false;
        txthSingleSession.Value = "0"; txthBulkPackage.Value = "0"; Type_BulkPay.Visible = false;
        SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        SnehDLL.SessionMast_Dll SMD = SMB.Get(AD.SessionID);
        if (SMD != null)
        {
            if (SMD.IsPrebooking)
            {
                if (!SMD.IsFirstPre)
                {
                    string SessionName = string.Empty;
                    bool isFirstPreAdded = IsFirstPrePaid(AD.PatientID, out  SessionName);
                    if (!isFirstPreAdded)
                    {
                        Session[DbHelper.Configuration.messageTextSession] = "Please pay " + SessionName + " session firstly.";
                        Session[DbHelper.Configuration.messageTypeSession] = "2";
                        if (toReturn == 101)
                            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                        else
                            Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                        return;
                    }
                }
            }
            if (SMD.IsPackage)
            {
                txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
                txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                Type_Package.Visible = true; txtAmountToPay.Enabled = false;
                MyPackages(); LoadPackages();
            }
            else if (SMD.IsEvaluation)
            {
                txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
                LoadEvaluation();
            }
            else
            {
                txthSingleSession.Value = "1";
                txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                Type_Single.Visible = true; txtAmountToPay.Enabled = false;
            }
        }

        SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
        txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
        txtSessionBankName.Items.Clear(); txtSessionBankName.Items.Add(new ListItem("Select Bank", "-1"));
        foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
        {
            txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
            txtSessionBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
        }
    }

    private bool IsFirstPrePaid(int _patientID, out string SessionName)
    {
        SessionName = string.Empty;
        SqlCommand cmd = new SqlCommand("PatientPre_IsPaid"); cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;

        SqlParameter Param = new SqlParameter(); Param.ParameterName = "@SessionName";
        Param.DbType = DbType.String; Param.Direction = ParameterDirection.Output;
        Param.Value = ""; Param.Size = 500; cmd.Parameters.Add(Param);

        Param = new SqlParameter(); Param.ParameterName = "@RetVal";
        Param.DbType = DbType.Boolean; Param.Direction = ParameterDirection.Output;
        Param.Value = false; cmd.Parameters.Add(Param);

        DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbUpdate(cmd);

        bool i = false;
        if (cmd.Parameters["@RetVal"].Value != null)
        {
            bool.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
        }
        if (!i)
        {
            if (cmd.Parameters["@SessionName"].Value != null)
            {
                SessionName = cmd.Parameters["@SessionName"].Value.ToString();
            }
        }
        return i;
    }

    public string TIMEDURATION(string _durationStr, string _timeStr)
    {
        int _duration = 0; int.TryParse(_durationStr, out _duration);
        DateTime TimeHourD = new DateTime();
        DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
        if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
        {
            return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
        }
        return "- - -";
    }

    protected void btnNewPackage_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 1; tb_Session.Enabled = false; tb_Packages.Enabled = true;
        LoadPackages();
    }

    private void MyPackages()
    {
        txtPatientPackages.Items.Clear(); txtPatientPackages.Items.Add(new ListItem("Select Package", "-1"));
        SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
        List<SnehDLL.PatientSessionPackage_Dll> PKDL = PKB.GetList(_appointmentID, AD.SessionID, AD.PatientID);
        foreach (SnehDLL.PatientSessionPackage_Dll PKD in PKDL)
        {
            txtPatientPackages.Items.Add(new ListItem(PKD.PackageCode, PKD.BookingID.ToString()));
        }
        if (PKDL.Count == 1)
        {
            txtPatientPackages.SelectedIndex = txtPatientPackages.Items.Count - 1;

            MyPackageDetail();
        }
    }

    protected void txtPatientPackages_SelectedIndexChanged(object sender, EventArgs e)
    {
        MyPackageDetail();
    }

    private void MyPackageDetail()
    {
        txtSessionCharge.Text = ""; txtPackageBalance.Text = ""; txtBalanceAmount.Text = ""; txtAmountToPay.Text = "";
        int _bookingID = 0; if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _bookingID); }
        SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
        SnehDLL.PatientSessionPackage_Dll PKD = PKB.Get(_bookingID);
        if (PKD != null)
        {
            //txtSessionCharge.Text = PKD.AppointmentCharge.ToString();
            //txtPackageBalance.Text = PKD.PackageBalance.ToString();
            //txtBalanceAmount.Text = (PKD.PackageBalance - PKD.AppointmentCharge).ToString();
            //txtAmountToPay.Text = PKD.AppointmentCharge.ToString(); txtAmountToPay.ReadOnly = true;

            if (PKD.MaximumTime > 0)
            {
                if (AD.Duration > PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                    txtSessionCharge.Text = _allAmount.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                    txtBalanceAmount.Text = (PKD.PackageBalance - _allAmount).ToString();
                    txtAmountToPay.Text = _allAmount.ToString(); txtAmountToPay.ReadOnly = true;
                }
                else if (AD.Duration < PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                    txtSessionCharge.Text = _allAmount.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                    txtBalanceAmount.Text = (PKD.PackageBalance - _allAmount).ToString();
                    txtAmountToPay.Text = _allAmount.ToString(); txtAmountToPay.ReadOnly = true;
                }
                else
                {
                    txtSessionCharge.Text = PKD.AppointmentCharge.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                    txtBalanceAmount.Text = (PKD.PackageBalance - PKD.AppointmentCharge).ToString();
                    txtAmountToPay.Text = PKD.AppointmentCharge.ToString(); txtAmountToPay.ReadOnly = true;
                }
            }
            else
            {
                txtSessionCharge.Text = PKD.AppointmentCharge.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                txtBalanceAmount.Text = (PKD.PackageBalance - PKD.AppointmentCharge).ToString();
                txtAmountToPay.Text = PKD.AppointmentCharge.ToString(); txtAmountToPay.ReadOnly = true;
            }
        }
    }

    private void LoadPackages()
    {
        txtExtraSessionCharge.Text = ""; tbExtraSessionCharge.Visible = false;
        txtPackageDiscountType.Enabled = false; tbPackageDiscount.Visible = false;
        tbPackageDiscountSessionCh.Visible = false; txtPackageSessionChargeNew.Text = "";
        txtPackageDiscountValue.Text = ""; txtPackageNetAmt.Text = "";
        if (txtPackageDiscountedOn.Items.Count > 0) { txtPackageDiscountedOn.SelectedIndex = 0; }
        if (txtPackageDiscountType.Items.Count > 0) { txtPackageDiscountType.SelectedIndex = 0; }

        if (txtPackageIsDiscounted.Checked)
        {
            txtPackageDiscountType.Enabled = true; txtPackageDiscountedOn.Enabled = true;
        }
        txtPackageAmount.Text = ""; txtAppointmentCharge.Text = ""; txtAppointmentCount.Text = ""; txtNarration.Text = ""; txtChequeDate.Text = ""; if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; }
        txtPackage.Items.Clear(); txtPackage.Items.Add(new ListItem("Select Package", "-1"));
        if (AD.PatientID > 0)
        {
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackageNewFilter(_appointmentID, AD.SessionID, AD.PatientID))
            {
                txtPackage.Items.Add(new ListItem(PKD.PackageCode, PKD.PackageID.ToString()));
            }
        }
    }

    protected void txtPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtExtraSessionCharge.Text = ""; tbExtraSessionCharge.Visible = false;
        txtPackageAmount.Text = ""; txtAppointmentCharge.Text = ""; txtAppointmentCount.Text = "";
        int _packageID = 0; if (txtPackage.SelectedItem != null) { int.TryParse(txtPackage.SelectedItem.Value, out _packageID); }
        SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
        SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
        if (PKD != null)
        {
            //txtPackageAmount.Text = PKD.PackageAmt.ToString();
            //txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
            //txtAppointmentCount.Text = PKD.Appointments.ToString();

            if (PKD.MaximumTime > 0)
            {
                if (AD.Duration > PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                    if (PKD.PackageAmt >= _allAmount)
                    {
                        txtPackageAmount.Text = PKD.PackageAmt.ToString();
                        txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
                        txtAppointmentCount.Text = PKD.Appointments.ToString();
                    }
                    else
                    {
                        tbExtraSessionCharge.Visible = true;
                        txtExtraSessionCharge.Text = (_allAmount - PKD.OneTimeAmt).ToString();
                        txtPackageAmount.Text = _allAmount.ToString();
                        txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
                        txtAppointmentCount.Text = PKD.Appointments.ToString();
                    }
                }
                else if (AD.Duration < PKD.MaximumTime)
                {
                    if (PKD.Appointments == 1)
                    {
                        float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                        float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                        txtPackageAmount.Text = _allAmount.ToString();
                        txtAppointmentCharge.Text = _allAmount.ToString();
                        txtAppointmentCount.Text = PKD.Appointments.ToString();
                    }
                    else
                    {
                        txtPackageAmount.Text = PKD.PackageAmt.ToString();
                        txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
                        txtAppointmentCount.Text = PKD.Appointments.ToString();
                    }
                }
                else
                {
                    txtPackageAmount.Text = PKD.PackageAmt.ToString();
                    txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
                    txtAppointmentCount.Text = PKD.Appointments.ToString();
                }
            }
            else
            {
                txtPackageAmount.Text = PKD.PackageAmt.ToString();
                txtAppointmentCharge.Text = PKD.OneTimeAmt.ToString();
                txtAppointmentCount.Text = PKD.Appointments.ToString();
            }
        }
        CalculatePackageDiscount();
    }

    protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        tab_Cheque.Visible = false; tab_online.Visible = false;
        if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; }
        txtBranchName.Text = ""; txtChequeNo.Text = ""; txtChequeDate.Text = "";
        txtTransactionID.Text = ""; txtTransactionDate.Text = ""; tab_online.Visible = false;

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

    protected void btnCancelPackage_Click(object sender, EventArgs e)
    {
        tb_Contents.ActiveTabIndex = 0; tb_Session.Enabled = true; tb_Packages.Enabled = false;
    }

    protected void btnSavePackage_Click(object sender, EventArgs e)
    {
        int _bookingID = 0;

        int _packageID = 0; if (txtPackage.SelectedItem != null) { int.TryParse(txtPackage.SelectedItem.Value, out _packageID); }
        if (_packageID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select package...", 2); return;
        }
        float _extraSessionCharge = 0; float.TryParse(txtExtraSessionCharge.Text.Trim(), out _extraSessionCharge);
        float _appointmentCharge = 0; float.TryParse(txtAppointmentCharge.Text.Trim(), out _appointmentCharge);
        float _appointmentCount = 0; float.TryParse(txtAppointmentCount.Text.Trim(), out _appointmentCount);
        float _packageAmount = 0; float.TryParse(txtPackageAmount.Text.Trim(), out _packageAmount);
        int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
        int _bankID = 0; string BankBranch = string.Empty; string ChequeTxnNo = string.Empty; DateTime _chequeDate = new DateTime();
        string HospitalReceiptID = string.Empty; DateTime HospitalReceiptDate = DateTime.MinValue;

        if (_paymentMode == 1 || _paymentMode == 2)
        {
            HospitalReceiptID = txtHospitalReceiptID.Text.Trim();

            DateTime.TryParseExact(txtHospitalReceiptDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out HospitalReceiptDate
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
        int _discountType = 0; float _discountValue = 0; float _discountAmt = 0; float _netAmt = 0; int _discountedOn = 0; float _newSessionCharge = 0;
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
        PPD.PatientID = AD.PatientID; PPD.SessionID = AD.SessionID;
        PPD.PackageID = _packageID; PPD.AppointmentCharge = _appointmentCharge;
        PPD.AppointmentCount = _appointmentCount; PPD.PackageAmount = _packageAmount;
        PPD.ModePayment = _paymentMode; PPD.BankID = _bankID; PPD.BankBranch = BankBranch;
        PPD.Narration = _narration; PPD.ChequeTxnNo = ChequeTxnNo; PPD.ChequeDate = _chequeDate;
        PPD.AddedDate = _entryDate; PPD.ModifyDate = _entryDate;
        PPD.AddedBy = _loginID; PPD.ModifyBy = _loginID;
        PPD.ExtraCharge = _extraSessionCharge;
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

            LoadForm();
            DbHelper.Configuration.setAlert(Page, "Patient package booking saved successfully...", 1);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }

    private void LoadEvaluation()
    {
        txtEvaluationAmount.Text = "";
        txtNarration.Text = ""; txtChequeDate.Text = ""; if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; }
        txtEvaluation.Items.Clear(); txtEvaluation.Items.Add(new ListItem("Select Evaluation", "-1"));
        if (AD.PatientID > 0)
        {
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackage(_appointmentID, AD.SessionID, AD.PatientID))
            {
                txtEvaluation.Items.Add(new ListItem(PKD.PackageCode, PKD.PackageID.ToString()));
            }
        }
    }

    protected void txtEvaluation_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEvaluationAmount.Text = ""; txtAmountToPay.Text = "";
        int _packageID = 0; if (txtEvaluation.SelectedItem != null) { int.TryParse(txtEvaluation.SelectedItem.Value, out _packageID); }
        SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
        SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
        if (PKD != null)
        {
            if (PKD.MaximumTime > 0)
            {
                if (AD.Duration > PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                    txtEvaluationAmount.Text = _allAmount.ToString(); txtAmountToPay.Text = _allAmount.ToString(); txtAmountToPay.ReadOnly = true;
                }
                else
                {
                    txtEvaluationAmount.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.ReadOnly = true;
                }
            }
            else
            {
                txtEvaluationAmount.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.ReadOnly = true;
            }
        }
    }

    protected void txtPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_SessionBank.Visible = false; tb_SessionOtherPackages.Visible = false; tb_SessionOnline.Visible = false;
        int _paymentType = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentType); }

        if (_paymentType == 3)
        {
            tb_SessionBank.Visible = true;
        }
        if (_paymentType == 4)
        {
            tb_SessionOnline.Visible = true;
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(AD.SessionID);
            if (SMD != null)
            {
                if (SMD.IsPackage)
                {
                    tb_SessionOnline.Visible = false;
                }
            }
        }
        else if (_paymentType == 5)
        {
            tb_SessionOtherPackages.Visible = true;
            MyOtherPackages();
        }
    }

    public void MyOtherPackages()
    {
        txtPaymentOtherPackage.Items.Clear(); txtPaymentOtherPackage.Items.Add(new ListItem("Select Package", "-1"));
        int _bookingID = 0; if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _bookingID); }
        SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
        List<SnehDLL.PatientSessionPackage_Dll> PKDL = PKB.GetList(AD.PatientID).Where(r => r.BookingID != _bookingID).ToList();
        foreach (SnehDLL.PatientSessionPackage_Dll PKD in PKDL)
        {
            txtPaymentOtherPackage.Items.Add(new ListItem(PKD.PackageCode, PKD.BookingID.ToString()));
        }
        if (PKDL.Count == 1)
        {
            txtPaymentOtherPackage.SelectedIndex = txtPaymentOtherPackage.Items.Count - 1;

            MyOtherPackageDetail();
        }
    }

    protected void txtPaymentOtherPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        MyOtherPackageDetail();
    }

    private void MyOtherPackageDetail()
    {
        txtPaymentOtherPackageBalance.Text = ""; txtPaymentOtherBalanceAmount.Text = "";
        int _bookingID = 0; if (txtPaymentOtherPackage.SelectedItem != null) { int.TryParse(txtPaymentOtherPackage.SelectedItem.Value, out _bookingID); }
        SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
        SnehDLL.PatientSessionPackage_Dll PKD = PKB.Get(_bookingID);
        if (PKD != null)
        {
            txtBalanceAmount.Text = txtPackageBalance.Text;
            txtPaymentOtherPackageBalance.Text = PKD.PackageBalance.ToString();
            float _sessionChrge = 0; float.TryParse(txtSessionCharge.Text.Trim(), out _sessionChrge);
            txtPaymentOtherBalanceAmount.Text = (PKD.PackageBalance - _sessionChrge).ToString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtEntryDateTime.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select patient session entry date...", 2); return;
        }
        DateTime _entryDate = new DateTime(); DateTime.TryParseExact(txtEntryDateTime.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _entryDate);
        if (_entryDate <= DateTime.MinValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select proper patient session entry date...", 2); return;
        }
        if (_entryDate >= DateTime.MaxValue)
        {
            DbHelper.Configuration.setAlert(Page, "Please select proper patient session entry date...", 2); return;
        }
        SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
        AD = AB.Get(_appointmentID);
        int patientID = AD.PatientID;
        SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll(); SnehDLL.SessionMast_Dll SMD = SMB.Get(AD.SessionID);
        if (SMD != null)
        {
            if (!chkBulkPackage.Checked)
            {
                #region
                int _bookingID = 0; DateTime _chequeDate = new DateTime(); int _packageID = 0; float _sessionCharge = 0;
                float _PackageTotalBalance = 0; float _PackageNewBalance = 0;
                int _bankID = 0; string BankBranch = string.Empty; string ChequeTxnNo = string.Empty;
                int _otherBookingID = 0; float _OtherBookingTotalBalance = 0; float _OtherBookingNewBalance = 0;
                float _payableAmount = 0;

                SnehDLL.PatientSessionPackage_Dll PKD = null; SnehDLL.PatientSessionPackage_Dll PKO = null; SnehDLL.Packages_Dll PKE = null;
                if (SMD.IsPackage)
                {
                    if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _bookingID); }
                    if (_bookingID <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select patient package, and try again...", 2); return;
                    }
                    SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll(); PKD = PKB.Get(_bookingID);
                    if (PKD == null)
                    {
                        DbHelper.Configuration.setAlert(Page, "Unable to find patient package, and try again...", 2); return;
                    }
                    /*******************************************************/
                    if (PKD.MaximumTime > 0)
                    {
                        if (AD.Duration > PKD.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                            _sessionCharge = _allAmount;
                        }
                        else if (AD.Duration < PKD.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                            _sessionCharge = _allAmount;
                        }
                        else
                        {
                            _sessionCharge = PKD.AppointmentCharge;
                        }
                    }
                    else
                    {
                        _sessionCharge = PKD.AppointmentCharge;
                    }
                    /*******************************************************/
                }
                else if (SMD.IsEvaluation)
                {
                    if (txtEvaluation.SelectedItem != null) { int.TryParse(txtEvaluation.SelectedItem.Value, out _packageID); }
                    if (_packageID <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select patient evaluation, and try again...", 2); return;
                    }
                    SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll(); PKE = PKB.Get(_packageID);
                    if (PKE == null)
                    {
                        DbHelper.Configuration.setAlert(Page, "Unable to find evaluation package, and try again...", 2); return;
                    }
                    if (PKE.MaximumTime > 0)
                    {
                        if (AD.Duration > PKE.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKE.OneTimeAmt / float.Parse(PKE.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                            _sessionCharge = _allAmount;
                        }
                        else
                        {
                            _sessionCharge = PKE.OneTimeAmt;
                        }
                    }
                    else
                    {
                        _sessionCharge = PKE.OneTimeAmt;
                    }
                }
                else
                {
                    float.TryParse(txtSingleSessionCharge.Text.Trim(), out _sessionCharge);
                    if (_sessionCharge <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please enter session charges...", 2); return;
                    }
                }
                int _paymentType = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentType); }
                if (_paymentType <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select session entry payment type...", 2); return;
                }
                if (_paymentType == 3)
                {
                    if (txtSessionBankName.SelectedItem != null) { int.TryParse(txtSessionBankName.SelectedItem.Value, out _bankID); }
                    if (_bankID <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select bank name of cheque...", 2); return;
                    }
                    BankBranch = txtSessionBankBranch.Text.Trim(); ChequeTxnNo = txtSessionChequeNo.Text.Trim();
                    if (string.IsNullOrEmpty(ChequeTxnNo))
                    {
                        DbHelper.Configuration.setAlert(Page, "Please enter cheque number...", 2); return;
                    }
                    if (txtSessionChequeDate.Text.Trim().Length <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please enter issued date of cheque...", 2); return;
                    }
                    DateTime.TryParseExact(txtSessionChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
                    if (_chequeDate >= DateTime.MaxValue)
                    {
                        DbHelper.Configuration.setAlert(Page, "Enter correct issued date of cheque...", 2); return;
                    }
                    if (_chequeDate <= DateTime.MinValue)
                    {
                        DbHelper.Configuration.setAlert(Page, "Enter correct issued date of cheque...", 2); return;
                    }
                }
                if (_paymentType == 4)
                {
                    if (SMD.IsPackage)
                    {
                        if (PKD.PackageBalance <= 0)
                        {
                            DbHelper.Configuration.setAlert(Page, "Selected patient package balance is insufficient...", 2); return;
                        }
                        if ((PKD.PackageBalance - _sessionCharge) < 0)
                        {
                            DbHelper.Configuration.setAlert(Page, "Selected patient package balance is insufficient...", 2); return;
                        }
                        _PackageTotalBalance = PKD.PackageBalance;
                        _PackageNewBalance = PKD.PackageBalance - _sessionCharge;
                    }
                    else
                    {
                        ChequeTxnNo = txtSessionTransactionID.Text.Trim();
                        DateTime.TryParseExact(txtSessionTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
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
                }
                if (_paymentType == 5)
                {
                    if (txtPaymentOtherPackage.SelectedItem != null) { int.TryParse(txtPaymentOtherPackage.SelectedItem.Value, out _otherBookingID); }
                    if (_otherBookingID <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please select patient other package, and try again...", 2); return;
                    }
                    SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll(); PKO = PKB.Get(_otherBookingID);
                    if (PKO == null)
                    {
                        DbHelper.Configuration.setAlert(Page, "Unable to find patient other package, and try again...", 2); return;
                    }
                    else
                    {
                        if (PKO.PackageBalance <= 0)
                        {
                            DbHelper.Configuration.setAlert(Page, "Selected patient other package balance is insufficient...", 2); return;
                        }
                        if ((PKO.PackageBalance - _sessionCharge) < 0)
                        {
                            DbHelper.Configuration.setAlert(Page, "Selected patient other package balance is insufficient...", 2); return;
                        }
                        _OtherBookingTotalBalance = PKO.PackageBalance;
                        _OtherBookingNewBalance = PKD.PackageBalance - _sessionCharge;
                    }
                }
                float.TryParse(txtAmountToPay.Text.Trim(), out _payableAmount);

                SnehDLL.AppointmentSession_Dll ASN = new SnehDLL.AppointmentSession_Dll();
                ASN.AppointmentID = _appointmentID;
                ASN.BookingID = _bookingID;
                ASN.PackageID = _packageID;
                ASN.PackageTotalBalance = _PackageTotalBalance;
                ASN.PackageNewBalance = _PackageNewBalance;
                ASN.AppointmentCharge = _sessionCharge;
                ASN.PaymentType = _paymentType;
                ASN.AmountToPay = _payableAmount;
                ASN.BankID = _bankID;
                ASN.BankBranch = BankBranch;
                ASN.ChequeTxnNo = ChequeTxnNo;
                ASN.ChequeDate = _chequeDate;
                ASN.Narration = txtSessionNarration.Text.Trim();
                ASN.OtherBookingID = _otherBookingID;
                ASN.OtherBookingTotalBalance = _OtherBookingTotalBalance;
                ASN.OtherBookingNewBalance = _OtherBookingNewBalance;
                ASN.AddedDate = _entryDate;
                ASN.ModifyDate = _entryDate;
                ASN.AddedBy = _loginID;
                ASN.ModifyBy = _loginID;

                SnehBLL.AppointmentSession_Bll ASB = new SnehBLL.AppointmentSession_Bll();
                int i = ASB.Set(ASN, 1);
                if (i > 0)
                {
                    if (SMD.IsPackage)
                    {
                        int _deductBookingID = _bookingID; if (_otherBookingID > 0) { _deductBookingID = _otherBookingID; }
                        if (ASB.SetPackagePayment(i, _deductBookingID) > 0)
                        {
                            ASB.SetDoctorPayment(i);

                            Session[DbHelper.Configuration.messageTextSession] = "Patient session entry completed successfully.";
                            Session[DbHelper.Configuration.messageTypeSession] = "1";

                            if (toReturn == 101)
                                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                            else
                                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                        }
                        else
                        {
                            ASB.Delete(i);
                            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                        }
                    }
                    else if (SMD.IsEvaluation)
                    {
                        if (ASB.SetEvalutionPayment(i) > 0)
                        {
                            ASB.SetDoctorPayment(i);

                            Session[DbHelper.Configuration.messageTextSession] = "Patient session entry completed successfully.";
                            Session[DbHelper.Configuration.messageTypeSession] = "1";

                            if (toReturn == 101)
                                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                            else
                                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                        }
                        else
                        {
                            ASB.Delete(i);
                            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                        }
                    }
                    else
                    {
                        if (ASB.SetOtherPayment(i) > 0)
                        {
                            ASB.SetDoctorPayment(i);

                            Session[DbHelper.Configuration.messageTextSession] = "Patient session entry completed successfully.";
                            Session[DbHelper.Configuration.messageTypeSession] = "1";

                            if (toReturn == 101)
                                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                            else
                                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                        }
                        else
                        {
                            ASB.Delete(i);
                            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                        }
                    }
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                }
                #endregion
            }
            else
            {

                //long bulkID = 0; if (txtBulkPackages.SelectedItem != null) { long.TryParse(txtBulkPackages.SelectedItem.Value, out bulkID); }
                long bulkID = 0; if (txtBulkPackages.Text != null) { long.TryParse(txtBulkPackages.Text, out bulkID); }
                bulkID = long.Parse(Session["BulkID"].ToString());
                int BulkSelected = 1;
                if(txtSessionNarration.Text.Trim().Length <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please fill naration...", 2); return;
                }
                //if (bulkID <= 0)
                //{
                //    DbHelper.Configuration.setAlert(Page, "Please select bulk package...", 2); return;
                //}
                SnehBLL.AppointmentSession_Bll ASB = new SnehBLL.AppointmentSession_Bll(); int i = 0; int packageid = 0;
                SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
                if (txtbulkpackagesnew.SelectedItem != null) { int.TryParse(txtbulkpackagesnew.SelectedItem.Value, out packageid); }
                DataTable dt = PB.GetSingleBulkPackage(bulkID, packageid);
                if (dt.Rows.Count > 0)
                {
                    float appntmntcontdatabase = 0; float appointmntcount = 0; double exactusedappointmentcount = 0; int session = 0;
                    float.TryParse(dt.Rows[0]["UsedAppointmentCount"].ToString(), out appntmntcontdatabase);
                    float.TryParse(dt.Rows[0]["AppointmentCount"].ToString(), out appointmntcount);
                    int.TryParse(dt.Rows[0]["SessionID"].ToString(), out session);
                    DataTable dt1 = PB.GetSessionTime(_appointmentID); int duration = 0;
                    if (dt1.Rows.Count > 0)
                    {
                        int.TryParse(dt1.Rows[0]["Duration"].ToString(), out duration);

                        if (duration == 30 && session == 30)
                        {
                            exactusedappointmentcount = appntmntcontdatabase + 0.5;
                        }
                        else
                        {
                            exactusedappointmentcount = appntmntcontdatabase + 1;
                        }
                    }
                    if (exactusedappointmentcount <= appointmntcount)
                    {
                        float bulkappointmentcharge = 0; float.TryParse(txtbulkappointmentcharge.Text.Trim(), out bulkappointmentcharge);
                        i = ASB.Set(AD.AppointmentID, bulkappointmentcharge, bulkID, BulkSelected, _entryDate, _loginID, txtSessionNarration.Text.Trim(), 1, packageid, patientID);
                        if (i > 0)
                        {

                            long bookingid = 0; long.TryParse(dt.Rows[0]["BookingID"].ToString(), out bookingid);
                            float usedappointmentcharge = 0; float exactusedappointmntchg = 0; float appointchrgdatabase = 0;
                            float.TryParse(txtbulkappointmentcharge.Text.Trim(), out appointchrgdatabase);
                            float.TryParse(txtusedappointmentcharge.Text.Trim(), out usedappointmentcharge);
                            exactusedappointmntchg = usedappointmentcharge + appointchrgdatabase;

                            int j = PB.UpdatePatientBulkPackage(exactusedappointmntchg, exactusedappointmentcount, bookingid);
                        }
                    }
                    else
                    {
                        DbHelper.Configuration.setAlert(Page, "Appointments Count bal is less...", 2); return;
                    }
                }
                else
                {
                    float bulkSessionCharges = 0; float.TryParse(txtBulkSessionCharge.Text.Trim(), out bulkSessionCharges);
                    if (bulkSessionCharges <= 0)
                    {
                        DbHelper.Configuration.setAlert(Page, "Please enter session charges...", 2); return;
                    }
                    //txtSessionNarration
                    i = ASB.Set(AD.AppointmentID, bulkSessionCharges, bulkID, BulkSelected, _entryDate, _loginID, txtSessionNarration.Text.Trim(), 1, packageid, patientID);
                }


                if (i > 0)
                {
                    if (ASB.SetBulkPayment(i) > 0)
                    {
                        ASB.SetDoctorPayment(i);

                        Session[DbHelper.Configuration.messageTextSession] = "Patient session entry completed successfully.";
                        Session[DbHelper.Configuration.messageTypeSession] = "1";

                        if (toReturn == 101)
                            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                        else
                            Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                    }
                    else
                    {
                        ASB.Delete(i);
                        DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                    }
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                }
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to find session detail, please try again..", 2);
        }
    }

    protected void chkBulkPackage_CheckedChanged(object sender, EventArgs e)
    {
        txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
      //  txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
        txthSingleSession.Value = "0"; txthBulkPackage.Value = "0";
        //Type_BulkPay.Visible = false;
        Type_Package.Visible = false; txtAmountToPay.Enabled = false;
        Type_Evaluation.Visible = false; txtAmountToPay.Enabled = false;
        Type_Single.Visible = false; txtAmountToPay.Enabled = false;
        tabPaymentModes.Visible = false;
        //tabPaymentBulkMode.Visible = false;
        bulk.Visible = false; bulkcarry.Visible = false;
        txtPackageBalance.Text = string.Empty; txtSessionCharge.Text = string.Empty; txtBalanceAmount.Text = string.Empty;
        txtEvaluationAmount.Text = string.Empty; txtSingleSessionCharge.Text = string.Empty; txtBulkSessionCharge.Text = string.Empty;
        txtAmountToPay.Text = string.Empty; txtBulkBalance.Text = string.Empty;
        tb_SessionBank.Visible = false; tb_SessionOnline.Visible = false; tb_SessionOtherPackages.Visible = false;
        txtBulkForword.Text = string.Empty;

        if (chkBulkPackage.Checked)
        {
            txthBulkPackage.Value = "1";
            //Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
            bulk.Visible = true; bulkcarry.Visible = true; bulkpack.Visible = false;

            LoadBulkBooking();
            //LoadBulkBalance();

        }
        else
        {
            tabPaymentModes.Visible = true; Type_BulkPay.Visible = false;
            bulkpackage.Visible = false; bulkusedamntchrg.Visible = false;
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(AD.SessionID);
            if (SMD != null)
            {
                if (SMD.IsPackage)
                {
                    txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
                    txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Package.Visible = true; txtAmountToPay.Enabled = false;
                    MyPackages(); LoadPackages();
                }
                else if (SMD.IsEvaluation)
                {
                    txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                    txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                    txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                    txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                    //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
                    LoadEvaluation();
                }
                else
                {
                    txthSingleSession.Value = "1";
                    txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                    txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                    txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                    txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                    //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Single.Visible = true; txtAmountToPay.Enabled = false;
                }
            }
        }
    }

    private void LoadBulkBooking()
    {
        SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
        List<SnehDLL.PatientBulk_Dll> DL = PB.ListPackage(AD.PatientID);
        int LAST_COUNT = DL.Count > 1 ? DL.Count - 1 : 0;

        if (DL.Count == 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "There is no such Bulk Booking....!!", 2); return;
        }

        Session["BulkID"] = DL[0].BulkID;

        #region GAURAV CODE
        var balAmt = Convert.ToDecimal(DL[LAST_COUNT].BalanceAmount);
        float newtotal = 0;

        txtBulkBalance.Text = balAmt.ToString();
        txtBulkForword.Text = balAmt.ToString();
        foreach (SnehDLL.PatientBulk_Dll item in DL)
        {
            newtotal += item.Amount;
        }
        txtBulkPackages.Text = newtotal.ToString() + "[" + DL[LAST_COUNT].PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]";
        #endregion
        if (balAmt == 0)
        {
            DbHelper.Configuration.setAlert(msgmodal, "There is no such Bulk Booking. Please create new bulk booking...!!", 2); return;
        }
        LoadBulkPackages();


        //txtBulkPackages.Items.Clear();
        ////txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
        //SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
        //List<SnehDLL.PatientBulk_Dll> DL = PB.ListPackage(AD.PatientID);
        //foreach (SnehDLL.PatientBulk_Dll item in DL)
        //{
        //    txtBulkPackages.Items.Add(new ListItem(item.Amount + "[" + item.PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]", item.BulkID.ToString()));
        //}
        ////if (DL.Count == 1 && txtBulkPackages.Items.Count > 0)
        ////{
        ////    txtBulkPackages.SelectedIndex = txtBulkPackages.Items.Count - 1;
        ////}
        //LoadBulkPackages();
    }

    protected void txtBulkPackages_SelectedIndexChanged(object sender, EventArgs e)
    {
        // LoadBulkBalance();
        LoadBulkPackages();
    }

    private void LoadBulkPackages()
    {
        // int BulkID = 0; if (txtBulkPackages.SelectedItem != null) { int.TryParse(txtBulkPackages.SelectedItem.Value, out BulkID); }
        int BulkID = 0; if (txtBulkPackages.Text != null) { int.TryParse(txtBulkPackages.Text, out BulkID); }
        if (BulkID > 0)
        {
            SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
            List<SnehDLL.PatientBulkPackage_Dll> PBD = PB.GetPatientBulkPackageNew(BulkID);
            if (PBD.Count > 0)
            {
                bulkpack.Visible = true; txtbulkpackagesnew.Items.Clear(); Type_BulkPay.Visible = false;
                //txtbulkpackagesnew.Items.Add(new ListItem("Select Package", "-1"));
                foreach (var item in PBD)
                {
                    txtbulkpackagesnew.Items.Add(new ListItem(item.PackageCode, item.PackageID.ToString()));
                }
                CalculateBulkPackage();
            }
            else
            {
                bulkpack.Visible = false; Type_BulkPay.Visible = true; bulkusedamntchrg.Visible = false; bulkpackage.Visible = false;
                LoadBulkBalance();
            }
        }
        else
        {
            Type_BulkPay.Visible = true;
        }
    }

    protected void txtbulkpackagesnew_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateBulkPackage();
    }

    private void CalculateBulkPackage()
    {

        bulkusedamntchrg.Visible = false; bulkpackage.Visible = false; float balamount = 0;
        int BulkID = 0; if (txtBulkPackages.Text != null) { int.TryParse(txtBulkPackages.Text, out BulkID); }
        int packageid = 0; if (txtbulkpackagesnew.SelectedItem != null) { int.TryParse(txtbulkpackagesnew.SelectedItem.Value, out packageid); }
        SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll(); balamount = PB.GetBalAmount(BulkID);
        SnehDLL.PatientBulkPackage_Dll PKD = PB.GetPatientBulkPackageByPackageNew(BulkID, packageid);
        if (PKD != null)
        {
            bulkusedamntchrg.Visible = true; bulkpackage.Visible = true; float balamountnew = 0; float bulkforward = 0;
            txtusedappointmentcharge.Text = PKD.UsedAppointmentCharge.ToString();
            txtusedappointmentcount.Text = PKD.UsedAppointmentCount.ToString();
            balamountnew = PKD.PackageAmount - balamount;

            if (PKD.MaximumTime > 0)
            {
                if (AD.Duration > PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                    txtbulkappointmentcharge.Text = _allAmount.ToString();
                    txtBulkBalance.Text = balamountnew.ToString();
                    bulkforward = balamountnew - _allAmount;
                    txtBulkForword.Text = bulkforward.ToString();
                }
                else if (AD.Duration < PKD.MaximumTime)
                {
                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                    txtbulkappointmentcharge.Text = _allAmount.ToString();
                    txtBulkBalance.Text = balamountnew.ToString();
                    bulkforward = balamountnew - _allAmount;
                    txtBulkForword.Text = bulkforward.ToString();
                }
                else
                {
                    txtbulkappointmentcharge.Text = PKD.AppointmentCharge.ToString();
                    txtBulkBalance.Text = balamountnew.ToString();
                    bulkforward = balamountnew - PKD.AppointmentCharge;
                    txtBulkForword.Text = bulkforward.ToString();
                }
            }
            else
            {
                txtbulkappointmentcharge.Text = PKD.AppointmentCharge.ToString();
                txtBulkBalance.Text = balamountnew.ToString();
                txtBulkForword.Text = (balamountnew - PKD.AppointmentCharge).ToString();
            }
        }
        else
        {
            bulkusedamntchrg.Visible = false; bulkpackage.Visible = false;
        }
    }

    private void LoadBulkBalance()
    {
        long BulkID = 0; if (txtBulkPackages.Text != null) { long.TryParse(txtBulkPackages.Text, out BulkID); }
        SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
        SnehDLL.PatientBulk_Dll item = PB.ListPackage(AD.PatientID).Find(f => f.BulkID == BulkID);
        if (item != null)
        {
            txtBulkBalance.Text = item.BalanceAmount.ToString();
            txtBulkForword.Text = item.BalanceAmount.ToString();
        }
        else
        {
            txtBulkBalance.Text = "0";
        }
    }
}