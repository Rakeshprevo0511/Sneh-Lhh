using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class PackageBulk : System.Web.UI.Page
    {
        int _loginID = 0; int _bookingID = 0; long BulkID = 0; int patientid = 0; int PreviousPatientID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["record"] != null)
            {
                BulkID = SnehBLL.PatientBulk_Bll.Check(Request.QueryString["record"].ToString());
            }

            if (!IsPostBack)
            {
                txtBookingDate.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadForm();
                if (BulkID > 0)
                {
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
            SnehDLL.PatientBulkPackage_Dll PBD = PB.GetPatientBulkPackage(BulkID);
            if (PBD != null && PBD.IsPackage == true)
            {
                chkamount.Checked = false; chkpackage.Checked = true; session.Visible = true; packageamount.Visible = true; amount.Visible = false; package.Visible = true;
                txtAppointmentCharge.Text = PBD.AppointmentCharge.ToString();
                txtAppointmentCount.Text = PBD.AppointmentCount.ToString();
                txtPackageAmount.Text = PBD.PackageAmount.ToString();
                txtBranchName.Text = PBD.BankBranch;
                txtNarration.Text = PBD.Narration;
                patientid = PBD.PatientID;
                if (txtPatient.Items.FindByValue(patientid.ToString()) != null)
                {
                    txtPatient.SelectedValue = patientid.ToString();
                }
                if (txtBankName.Items.FindByValue(PBD.BankID.ToString()) != null)
                {
                    txtBankName.SelectedValue = PBD.BankID.ToString();
                }
                if (txtPaymentMode.Items.FindByValue(PBD.ModePayment.ToString()) != null)
                {
                    txtPaymentMode.SelectedValue = PBD.ModePayment.ToString();
                }

                List<SnehDLL.PatientBulkPackage_Dll> PD = PB.GetSessionList(BulkID);
                if (PD != null)
                {
                    foreach (var items in PD)
                    {
                        if (txtSession.Items.FindByValue(items.SessionID.ToString()) != null)
                        {
                            txtSession.Items.FindByValue(items.SessionID.ToString()).Selected = true;
                        }
                    }
                    LoadPackages();

                    foreach (var items in PD)
                    {
                        if (txtPackage.Items.FindByValue(items.PackageID.ToString()) != null)
                        {
                            txtPackage.Items.FindByValue(items.PackageID.ToString()).Selected = true;
                        }
                    }
                }
                int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
                if (_paymentMode == 3)
                {
                    tab_Cheque.Visible = true;
                    txtChequeDate.Text = PBD.ChequeDate > DateTime.MinValue ? PBD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                    txtChequeNo.Text = PBD.ChequeTxnNo;
                }
                if (_paymentMode == 4)
                {
                    tab_online.Visible = true;
                    txtTransactionID.Text = PBD.ChequeTxnNo;
                    txtTransactionDate.Text = PBD.ChequeDate > DateTime.MinValue ? PBD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                    txtBookingDate.Text = PBD.PaidDate > DateTime.MinValue ? PBD.PaidDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                }

            }
            else
            {
                SnehBLL.PatientBulk_Bll PBN = new SnehBLL.PatientBulk_Bll();
                SnehDLL.PatientBulk_Dll PBDN = PBN.GetPatientBulk(BulkID);
                if (PBDN != null)
                {
                    chkamount.Checked = true; chkpackage.Checked = false; session.Visible = false; packageamount.Visible = false; amount.Visible = true; package.Visible = false;
                    txtAmount.Text = PBDN.Amount.ToString();
                    txtBranchName.Text = PBDN.BankBranch;
                    txtNarration.Text = PBDN.Narration;
                    patientid = PBDN.PatientID;
                    Session["PreviousPatientID"] = patientid;
                    if (txtPatient.Items.FindByValue(patientid.ToString()) != null)
                    {
                        txtPatient.SelectedValue = patientid.ToString();
                    }
                    if (txtBankName.Items.FindByValue(PBDN.BankID.ToString()) != null)
                    {
                        txtBankName.SelectedValue = PBDN.BankID.ToString();
                    }
                    if (txtPaymentMode.Items.FindByValue(PBDN.ModePayment.ToString()) != null)
                    {
                        txtPaymentMode.SelectedValue = PBDN.ModePayment.ToString();
                    }
                    int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
                    if (_paymentMode == 3)
                    {
                        tab_Cheque.Visible = true;
                        txtChequeDate.Text = PBDN.ChequeDate > DateTime.MinValue ? PBDN.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                        txtChequeNo.Text = PBDN.ChequeTxnNo;
                    }
                    if (_paymentMode == 4)
                    {
                        tab_online.Visible = true;
                        txtTransactionID.Text = PBDN.ChequeTxnNo;
                        txtTransactionDate.Text = PBDN.ChequeDate > DateTime.MinValue ? PBDN.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                        txtBookingDate.Text = PBDN.PaidDate > DateTime.MinValue ? PBDN.PaidDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty;
                    }
                }
            }
        }

        private void LoadForm()
        {
            SnehBLL.PatientMast_Bll PMB = new SnehBLL.PatientMast_Bll();
            txtPatient.Items.Clear(); txtPatient.Items.Add(new ListItem("Select Patient", "-1"));
            foreach (SnehDLL.PatientMast_Dll PMD in PMB.GetForDropdown())
            {
                if (PMD.PatientTypeID != 3)
                {
                    txtPatient.Items.Add(new ListItem(PMD.FullName, PMD.PatientID.ToString()));
                }
            }

            SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
            txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
            foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
            {
                txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
            }

            SnehBLL.SessionMast_Bll PSB = new SnehBLL.SessionMast_Bll();
            txtSession.Items.Clear();
            //txtSession.Items.Add(new ListItem("Select Session", "-1"));
            foreach (SnehDLL.SessionMast_Dll PSD in PSB.GetPackageList())
            {
                txtSession.Items.Add(new ListItem(PSD.SessionName, PSD.SessionID.ToString()));
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
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
            tab_Cheque.Visible = false; tab_online.Visible = false;
            if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; }
            txtBranchName.Text = ""; txtChequeNo.Text = ""; txtChequeDate.Text = "";
            txtTransactionID.Text = ""; txtTransactionDate.Text = "";
            if (_paymentMode == 3)
            {
                tab_Cheque.Visible = true;
            }
            if (_paymentMode == 4)
            {
                tab_online.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int _patientID = 0; if (txtPatient.SelectedItem != null) { int.TryParse(txtPatient.SelectedItem.Value, out _patientID); }
            if (_patientID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select patient, and try again...", 2); return;
            }
            float Amount = 0; bool ispackage = false;
            if (amount.Visible == true)
            {
                float.TryParse(txtAmount.Text.Trim(), out Amount);
                if (Amount <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please enter booking amount...", 2); return;
                }
            }
            else
            {
                ispackage = true;
                float.TryParse(txtPackageAmount.Text.Trim(), out Amount);
            }
            int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
            int _bankID = 0; string BankBranch = string.Empty; string ChequeTxnNo = string.Empty; DateTime _chequeDate = new DateTime();
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
            if (txtBookingDate.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select booking date...", 2); return;
            }
            DateTime _entryDate = new DateTime(); DateTime.TryParseExact(txtBookingDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _entryDate);
            if (_entryDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select proper booking date...", 2); return;
            }
            if (_entryDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select proper booking date...", 2); return;
            }
            string _narration = txtNarration.Text.Trim();
            if(_narration == "")
            {
                DbHelper.Configuration.setAlert(Page, "Please fill narration...", 2); return;
            }
            PreviousPatientID = Convert.ToInt32(Session["PreviousPatientID"]);

            SnehBLL.PatientBulk_Bll PPB = new SnehBLL.PatientBulk_Bll();
            long i = PPB.Set(new SnehDLL.PatientBulk_Dll()
            {
                BulkID = BulkID,
                PatientID = _patientID,
                Amount = Amount,
                ModePayment = _paymentMode,
                BankID = _bankID,
                BankBranch = BankBranch,
                ChequeTxnNo = ChequeTxnNo,
                ChequeDate = _chequeDate,
                Narration = _narration,
                PaidDate = _entryDate,
                ModifyBy = _loginID,
                IsPackage = ispackage,
            });

            if (i > 0)
            {
                if (BulkID > 0)
                {
                    long d = PPB.DeletePatientHospitalLedger(BulkID, PreviousPatientID, _paymentMode);
                }

                if (_bookingID <= 0) //ADD BULK PAYMENT
                {
                    SnehBLL.PatientLedger_Bll PLB = new SnehBLL.PatientLedger_Bll();
                    PLB.NewBulkBooking(i);
                }

                bool IsPackageNew = false;
                IsPackageNew = PPB.GetIspackage(i);
                if (IsPackageNew)
                {
                    if (BulkID > 0)
                    {
                        int z = PPB.DeletePatientBulkPackage(BulkID);
                    }
                    long bookingid = 0;
                    foreach (ListItem li in txtPackage.Items)
                    {
                        if (li.Selected)
                        {
                            int packageid = 0; int.TryParse(li.Value, out packageid);
                            int packageamount = 0; int.TryParse(txtPackageAmount.Text.Trim(), out packageamount);
                            //int appointmentcharge = 0; int.TryParse(txtAppointmentCharge.Text.Trim(), out appointmentcharge);
                            //int appointmentcount = 0; int.TryParse(txtAppointmentCount.Text.Trim(), out appointmentcount);
                            bookingid = PPB.SetBulkPackage(new SnehDLL.PatientBulkPackage_Dll()
                            {
                                BookingID = 0,
                                BulkID = i,
                                SessionID = 0,
                                PackageID = packageid,
                                AppointmentCharge = 0,
                                AppointmentCount = 0,
                                PackageAmount = packageamount
                            });
                        }
                    }
                    if (bookingid > 0)
                    {
                        DataTable dt = PPB.GetBulkPackage(i);
                        for (int a = 0; a < dt.Rows.Count; a++)
                        {
                            int packageid = 0; int.TryParse(dt.Rows[a]["PackageID"].ToString(), out packageid);
                            int sessionid = 0; int.TryParse(dt.Rows[a]["SessionID"].ToString(), out sessionid);
                            long booking = 0; long.TryParse(dt.Rows[a]["BookingID"].ToString(), out booking);
                            int packageamount = 0; int.TryParse(dt.Rows[a]["PackageAmt"].ToString(), out packageamount);
                            float onetimeamount = 0; float.TryParse(dt.Rows[a]["OneTimeAmt"].ToString(), out onetimeamount);
                            int appointments = 0; int.TryParse(dt.Rows[a]["Appointments"].ToString(), out appointments);
                            foreach (ListItem item in txtSession.Items)
                            {
                                if (item.Selected)
                                {
                                    int sessionidnew = 0; int.TryParse(item.Value, out sessionidnew);
                                    if (sessionidnew == sessionid)
                                    {
                                        int x = PPB.UpdateBulkPackage(sessionidnew, packageid, booking, onetimeamount, appointments);
                                    }
                                }
                            }
                        }

                    }
                }
                if (BulkID > 0)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Patient bulk package booking updated successfully.";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/Member/PackageBulks.aspx"), true);
                }
                else
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Patient bulk package booking saved successfully.";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/Member/PackageBulk.aspx"), true);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again.", 2);
            }
        }

        protected void chkpackage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkpackage.Checked == true)
            {
                session.Visible = true; packageamount.Visible = true; amount.Visible = false;
            }
            else
            {
                session.Visible = false; packageamount.Visible = false; amount.Visible = true;
            }

        }

        protected void chkamount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkamount.Checked == true)
            {
                session.Visible = false; packageamount.Visible = false; amount.Visible = true;
            }
            else
            {
                session.Visible = true; packageamount.Visible = true; amount.Visible = false;
            }

        }

        protected void txtSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPackages();
        }

        private void LoadPackages()
        {
            int _patientID = 0;
            if (txtPatient.SelectedItem != null)
            {
                int.TryParse(txtPatient.SelectedItem.Value, out _patientID);
            }
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            if (_sessionID > 0 && _patientID > 0)
            {
                package.Visible = true;
            }
            else
            {
                package.Visible = false;
            }
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            txtPackage.Items.Clear();
            //txtPackage.Items.Add(new ListItem("Select Package", "-1"));
            foreach (ListItem li in txtSession.Items)
            {
                if (li.Selected)
                {
                    int SessionID = 0; int.TryParse(li.Value, out SessionID);
                    if (SessionID > 0 && _patientID > 0)
                    {
                        foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackageNewFilter(0, SessionID, _patientID))
                        {
                            txtPackage.Items.Add(new ListItem(PKD.PackageCode, PKD.PackageID.ToString()));
                        }
                    }
                }
            }
        }

        protected void txtPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPackageAmount.Text = ""; txtAppointmentCharge.Text = ""; txtAppointmentCount.Text = "";
            float packageamount = 0; float onetimeamt = 0; int appointcount = 0;
            foreach (ListItem li in txtPackage.Items)
            {
                SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
                if (li.Selected)
                {
                    int _packageID = 0; int.TryParse(li.Value, out _packageID);
                    SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
                    if (PKD != null)
                    {
                        packageamount += PKD.PackageAmt;
                        //onetimeamt += PKD.OneTimeAmt;
                        //appointcount += PKD.Appointments;
                    }
                }
            }
            txtPackageAmount.Text = packageamount.ToString();
            //txtAppointmentCharge.Text = onetimeamt.ToString();
            //txtAppointmentCount.Text = appointcount.ToString();
        }
    }
}