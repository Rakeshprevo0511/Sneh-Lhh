using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

namespace snehrehab.Member
{
    public partial class AppointmentPaidEdit : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; bool isAdmin = false;
        int toReturn = 0; public string returnUrl = string.Empty; int sessionid = 0; int _therapist = 0, _assistant = 0;
        SnehDLL.Appointments_Dll AD_Loaded = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            isAdmin = SnehBLL.UserAccount_Bll.IsAdmin();
            if (!isAdmin)
            {
                Response.Redirect("/Member/"); return;
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (Request.QueryString["sessionid"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["sessionid"].ToString()))
                {
                    sessionid = SnehBLL.SessionMast_Bll.Check(Request.QueryString["sessionid"].ToString());
                }
            }
            if (Request.QueryString["therapist"] != null)
            {
                int.TryParse(Request.QueryString["therapist"].ToString(), out _therapist);
            }
            if (Request.QueryString["assistant1"] != null)
            {
                int.TryParse(Request.QueryString["assistant1"].ToString(), out _assistant);
            }
            if (_appointmentID <= 0)
            {
                Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
            }

            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            AD_Loaded = AB.Get(_appointmentID);
            if (AD_Loaded != null)
            {
                if (AD_Loaded.IsDeleted)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
                }
                if (AD_Loaded.AppointmentStatus != 1)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Appointment status is not completed.";
                    Session[DbHelper.Configuration.messageTypeSession] = "2";
                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
                }
                if (!IsPostBack)
                {
                    LoadForm();
                }

            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find appointment detail, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "2";
                Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
            }
        }

        private void LoadForm()
        {
            txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
            // txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
            txthSingleSession.Value = "0"; txthBulkPackage.Value = "0"; Type_BulkPay.Visible = false;
            Type_Package.Visible = false; txtAmountToPay.Enabled = false;
            Type_Evaluation.Visible = false; txtAmountToPay.Enabled = false;
            Type_Single.Visible = false; txtAmountToPay.Enabled = false;
            tabPaymentModes.Visible = false;
            //tabPaymentBulkMode.Visible = false;
            txtPackageBalance.Text = string.Empty; txtSessionCharge.Text = string.Empty; txtBalanceAmount.Text = string.Empty;
            txtEvaluationAmount.Text = string.Empty; txtSingleSessionCharge.Text = string.Empty; txtBulkSessionCharge.Text = string.Empty;
            txtAmountToPay.Text = string.Empty; txtBulkBalance.Text = string.Empty;
            tb_SessionBank.Visible = false; tb_SessionOnline.Visible = false; tb_SessionOtherPackages.Visible = false;
            txtBulkForword.Text = string.Empty;

            tb_Contents.ActiveTabIndex = 0; tb_Session.Enabled = true; tb_Packages.Enabled = false;
            SnehBLL.Appointments_Bll PKB = new SnehBLL.Appointments_Bll();
            DataSet ds = PKB.SessionDetail(AD_Loaded.AppointmentID);
            if (ds.Tables.Count > 0)
            {
                PatientGV.DataSource = ds.Tables[0];
            }
            PatientGV.DataBind();
            if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

            SnehBLL.SessionMast_Bll PSB = new SnehBLL.SessionMast_Bll();


            txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
            foreach (SnehDLL.SessionMast_Dll PSD in PSB.GetList())
            {
                txtSession.Items.Add(new ListItem(PSD.SessionName, PSD.SessionID.ToString()));
            }
            if (sessionid > 0)
            {
                if (txtSession.Items.FindByValue(sessionid.ToString()) != null)
                {
                    txtSession.SelectedValue = sessionid.ToString();
                }
            }
            else
            {
                if (txtSession.Items.FindByValue(AD_Loaded.SessionID.ToString()) != null)
                {
                    txtSession.SelectedValue = AD_Loaded.SessionID.ToString();
                }
            }
            txtAppointmentDate.Text = AD_Loaded.AppointmentDate > DateTime.MinValue ? AD_Loaded.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat) : "";
            LoadTherapist();

            if (_therapist > 0)
            {
                if (txtTherapist.Items.FindByValue(_therapist.ToString()) != null)
                {
                    txtTherapist.SelectedValue = _therapist.ToString();
                }

                if (_assistant > -1)
                {
                    if (txtAssistant1.Items.FindByValue(_assistant.ToString()) != null)
                    {
                        txtAssistant1.SelectedValue = _assistant.ToString();
                    }
                }
            }
            else
            {
                SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
                foreach (SnehDLL.AppointmentDoctor_Dll ADD in ADB.GetList(AD_Loaded.AppointmentID))
                {
                    if (ADD.IsMain)
                    {
                        if (txtTherapist.Items.FindByValue(ADD.DoctorID.ToString()) != null)
                        {
                            txtTherapist.SelectedValue = ADD.DoctorID.ToString();
                        }

                    }
                    else
                    {
                        if (txtAssistant1.Items.FindByValue(ADD.DoctorID.ToString()) != null)
                        {
                            txtAssistant1.SelectedValue = ADD.DoctorID.ToString();
                        }
                    }
                }
            }

            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            txtTimeFrom.Items.Clear(); txtTimeFrom.Items.Add(new ListItem("Select Time", "-1"));
            foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.GetList())
            {
                txtTimeFrom.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
            }
            LoadTiming();
            SnehDLL.AppointmentTime_Dll ATLD = ATB.Get(AD_Loaded.AppointmentTime);
            if (ATLD != null)
            {
                if (txtTimeFrom.Items.FindByValue(ATLD.TimeID.ToString()) != null)
                {
                    txtTimeFrom.SelectedValue = ATLD.TimeID.ToString();
                }
            }
            if (txtDuration.Items.FindByValue(AD_Loaded.Duration.ToString()) != null)
            {
                txtDuration.SelectedValue = AD_Loaded.Duration.ToString();
            }
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(AD_Loaded.SessionID);
            if (SMD != null)
            {
                SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
                txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
                txtSessionBankName.Items.Clear(); txtSessionBankName.Items.Add(new ListItem("Select Bank", "-1"));
                foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
                {
                    txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
                    txtSessionBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
                }
                SnehBLL.AppointmentSession_Bll APSB = new SnehBLL.AppointmentSession_Bll();
                SnehDLL.AppointmentSession_Dll APSD = APSB.Get(AD_Loaded.AppointmentID);
                if (APSD != null)
                {
                    txtEntryDateTime.Text = APSD.ModifyDate > DateTime.MinValue ? APSD.ModifyDate.ToString(DbHelper.Configuration.showDateFormat) : "";
                    txtSessionNarration.Text = APSD.Narration;
                    if (SMD.IsPackage)
                    {
                        txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
                        txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                        if (APSD.BulkBookingID > 0)
                        {
                            #region

                            Type_Package.Visible = false;
                            txthBulkPackage.Value = "1";
                            //Type_BulkPay.Visible = true; 
                            tabPaymentBulkMode.Visible = true;
                            chkBulkPackage.Checked = true;
                            LoadBulkBooking();
                            txtBulkPackages.Text = Session["new_bulk"].ToString();
                            //if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
                            //{
                            //    txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
                            //}
                            //LoadBulkBalance();
                            txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
                            #endregion
                        }
                        else
                        {
                            tabPaymentModes.Visible = true; Type_Package.Visible = true; txtAmountToPay.Enabled = false;
                            MyPackages();
                            if (txtPatientPackages.Items.FindByValue(APSD.BookingID.ToString()) != null)
                            {
                                txtPatientPackages.SelectedValue = APSD.BookingID.ToString();
                            }
                            MyPackageDetail();
                            if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
                            {
                                txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
                            }
                            LoadPaymentType();
                            if (APSD.PaymentType == 5)
                            {
                                if (txtPaymentOtherPackage.Items.FindByValue(APSD.OtherBookingID.ToString()) != null)
                                {
                                    txtPaymentOtherPackage.SelectedValue = APSD.OtherBookingID.ToString();
                                }
                                MyOtherPackageDetail();
                            }
                        }
                    }
                    else if (SMD.IsEvaluation)
                    {
                        #region
                        txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                        txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                        txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                        txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                        LoadEvaluation();
                        if (APSD.BulkBookingID > 0)
                        {
                            #region
                            Type_Evaluation.Visible = false;
                            txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
                            chkBulkPackage.Checked = true;
                            LoadBulkBooking();
                            txtBulkPackages.Text = Session["new_bulk"].ToString();

                            //if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
                            //{
                            //    txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
                            //}
                            LoadBulkBalance();
                            txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
                            #endregion
                        }
                        else
                        {
                            #region
                            tabPaymentModes.Visible = true; Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
                            if (txtEvaluation.Items.FindByValue(APSD.PackageID.ToString()) != null)
                            {
                                txtEvaluation.SelectedValue = APSD.PackageID.ToString();
                            }
                            txtEvaluationAmount.Text = APSD.AppointmentCharge.ToString();
                            if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
                            {
                                txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
                            }
                            LoadPaymentType();
                            if (APSD.PaymentType == 3)
                            {
                                if (txtSessionBankName.Items.FindByValue(APSD.BankID.ToString()) != null)
                                {
                                    txtSessionBankName.SelectedValue = APSD.BankID.ToString();
                                }
                                txtSessionBankBranch.Text = APSD.BankBranch;
                                txtSessionChequeNo.Text = APSD.ChequeTxnNo;
                                txtSessionChequeDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
                            }
                            if (APSD.PaymentType == 4)
                            {
                                txtSessionTransactionID.Text = APSD.ChequeTxnNo;
                                txtSessionTransactionDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
                            }
                            txtAmountToPay.Text = APSD.AmountToPay.ToString();
                            hidStatus.Value = "1";
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        txthSingleSession.Value = "1";
                        txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                        txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                        txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                        txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                        if (APSD.BulkBookingID > 0)
                        {
                            #region
                            Type_Single.Visible = false; txtAmountToPay.Enabled = false;
                            txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
                            chkBulkPackage.Checked = true;
                            LoadBulkBooking();
                            txtBulkPackages.Text = Session["new_bulk"].ToString();

                            //if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
                            //{
                            //    txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
                            //}
                            LoadBulkBalance();
                            txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
                            #endregion
                        }
                        else
                        {
                            #region
                            tabPaymentModes.Visible = true;
                            Type_Single.Visible = true; txtAmountToPay.Enabled = false;
                            txtSingleSessionCharge.Text = APSD.AppointmentCharge.ToString();
                            if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
                            {
                                txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
                            }
                            LoadPaymentType();
                            if (APSD.PaymentType == 3)
                            {
                                if (txtSessionBankName.Items.FindByValue(APSD.BankID.ToString()) != null)
                                {
                                    txtSessionBankName.SelectedValue = APSD.BankID.ToString();
                                }
                                txtSessionBankBranch.Text = APSD.BankBranch;
                                txtSessionChequeNo.Text = APSD.ChequeTxnNo;
                                txtSessionChequeDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
                            }
                            if (APSD.PaymentType == 4)
                            {
                                txtSessionTransactionID.Text = APSD.ChequeTxnNo;
                                txtSessionTransactionDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
                            }

                            txtAmountToPay.Text = APSD.AmountToPay.ToString();
                            hidStatus.Value = "1";
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Unable to find appointment detail, Please try again.";
                    Session[DbHelper.Configuration.messageTypeSession] = "2";
                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
                }
            }
            else
            {
                Session[DbHelper.Configuration.messageTextSession] = "Unable to find appointment detail, Please try again.";
                Session[DbHelper.Configuration.messageTypeSession] = "2";
                Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
            }
        }

        //private void LoadForm()
        //{
        //    txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
        //    //txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
        //    txthSingleSession.Value = "0"; txthBulkPackage.Value = "0"; 
        //    //Type_BulkPay.Visible = false;
        //    Type_Package.Visible = false; txtAmountToPay.Enabled = false;
        //    Type_Evaluation.Visible = false; txtAmountToPay.Enabled = false;
        //    Type_Single.Visible = false; txtAmountToPay.Enabled = false;
        //    tabPaymentModes.Visible = false; 
        //    //tabPaymentBulkMode.Visible = false;
        //    txtPackageBalance.Text = string.Empty; txtSessionCharge.Text = string.Empty; txtBalanceAmount.Text = string.Empty;
        //    txtEvaluationAmount.Text = string.Empty; txtSingleSessionCharge.Text = string.Empty; 
        //    //txtBulkSessionCharge.Text = string.Empty;
        //    txtAmountToPay.Text = string.Empty; 
        //    //txtBulkBalance.Text = string.Empty;
        //    tb_SessionBank.Visible = false; //tb_SessionOnline.Visible = false; 
        //    tb_SessionOtherPackages.Visible = false;
        //    //txtBulkForword.Text = string.Empty;

        //    tb_Contents.ActiveTabIndex = 0; tb_Session.Enabled = true; tb_Packages.Enabled = false;
        //    SnehBLL.Appointments_Bll PKB = new SnehBLL.Appointments_Bll();
        //    DataSet ds = PKB.SessionDetail(AD_Loaded.AppointmentID);
        //    if (ds.Tables.Count > 0)
        //    {
        //        PatientGV.DataSource = ds.Tables[0];
        //    }
        //    PatientGV.DataBind();
        //    if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

        //    SnehBLL.SessionMast_Bll PSB = new SnehBLL.SessionMast_Bll();


        //    txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
        //    foreach (SnehDLL.SessionMast_Dll PSD in PSB.GetList())
        //    {
        //        txtSession.Items.Add(new ListItem(PSD.SessionName, PSD.SessionID.ToString()));
        //    }
        //    if (sessionid > 0)
        //    {
        //        if (txtSession.Items.FindByValue(sessionid.ToString()) != null)
        //        {
        //            txtSession.SelectedValue = sessionid.ToString();
        //        }
        //    }
        //    else
        //    {
        //        if (txtSession.Items.FindByValue(AD_Loaded.SessionID.ToString()) != null)
        //        {
        //            txtSession.SelectedValue = AD_Loaded.SessionID.ToString();
        //        }
        //    }
        //    txtAppointmentDate.Text = AD_Loaded.AppointmentDate > DateTime.MinValue ? AD_Loaded.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //    LoadTherapist();

        //    if (_therapist > 0)
        //    {
        //        if (txtTherapist.Items.FindByValue(_therapist.ToString()) != null)
        //        {
        //            txtTherapist.SelectedValue = _therapist.ToString();
        //        }

        //        if (_assistant > -1)
        //        {
        //            if (txtAssistant1.Items.FindByValue(_assistant.ToString()) != null)
        //            {
        //                txtAssistant1.SelectedValue = _assistant.ToString();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
        //        foreach (SnehDLL.AppointmentDoctor_Dll ADD in ADB.GetList(AD_Loaded.AppointmentID))
        //        {
        //            if (ADD.IsMain)
        //            {
        //                if (txtTherapist.Items.FindByValue(ADD.DoctorID.ToString()) != null)
        //                {
        //                    txtTherapist.SelectedValue = ADD.DoctorID.ToString();
        //                }

        //            }
        //            else
        //            {
        //                if (txtAssistant1.Items.FindByValue(ADD.DoctorID.ToString()) != null)
        //                {
        //                    txtAssistant1.SelectedValue = ADD.DoctorID.ToString();
        //                }
        //            }
        //        }
        //    }

        //    SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
        //    txtTimeFrom.Items.Clear(); txtTimeFrom.Items.Add(new ListItem("Select Time", "-1"));
        //    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.GetList())
        //    {
        //        txtTimeFrom.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
        //    }
        //    LoadTiming();
        //    SnehDLL.AppointmentTime_Dll ATLD = ATB.Get(AD_Loaded.AppointmentTime);
        //    if (ATLD != null)
        //    {
        //        if (txtTimeFrom.Items.FindByValue(ATLD.TimeID.ToString()) != null)
        //        {
        //            txtTimeFrom.SelectedValue = ATLD.TimeID.ToString();
        //        }
        //    }
        //    if (txtDuration.Items.FindByValue(AD_Loaded.Duration.ToString()) != null)
        //    {
        //        txtDuration.SelectedValue = AD_Loaded.Duration.ToString();
        //    }
        //    SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        //    SnehDLL.SessionMast_Dll SMD = SMB.Get(AD_Loaded.SessionID);
        //    if (SMD != null)
        //    {
        //        SnehBLL.Banks_Bll BNB = new SnehBLL.Banks_Bll();
        //        txtBankName.Items.Clear(); txtBankName.Items.Add(new ListItem("Select Bank", "-1"));
        //        txtSessionBankName.Items.Clear(); txtSessionBankName.Items.Add(new ListItem("Select Bank", "-1"));
        //        foreach (SnehDLL.Banks_Dll PSD in BNB.GetList())
        //        {
        //            txtBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
        //            txtSessionBankName.Items.Add(new ListItem(PSD.BankName, PSD.BankID.ToString()));
        //        }
        //        SnehBLL.AppointmentSession_Bll APSB = new SnehBLL.AppointmentSession_Bll();
        //        SnehDLL.AppointmentSession_Dll APSD = APSB.Get(AD_Loaded.AppointmentID);
        //        if (APSD != null)
        //        {
        //            txtEntryDateTime.Text = APSD.ModifyDate > DateTime.MinValue ? APSD.ModifyDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //            txtSessionNarration.Text = APSD.Narration;
        //            if (SMD.IsPackage)
        //            {
        //                txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
        //                txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
        //                //if (APSD.BulkBookingID > 0)
        //                //{
        //                //    #region
        //                //    Type_Package.Visible = false;
        //                //    txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
        //                //    chkBulkPackage.Checked = true;
        //                //    LoadBulkBooking();
        //                //    if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
        //                //    {
        //                //        txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
        //                //    }
        //                //    LoadBulkBalance();
        //                //    txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
        //                //    #endregion
        //                //}
        //                //else
        //                //{
        //                    tabPaymentModes.Visible = true; Type_Package.Visible = true; txtAmountToPay.Enabled = false;
        //                    MyPackages();
        //                    if (txtPatientPackages.Items.FindByValue(APSD.BookingID.ToString()) != null)
        //                    {
        //                        txtPatientPackages.SelectedValue = APSD.BookingID.ToString();
        //                    }
        //                    MyPackageDetail();
        //                    if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
        //                    {
        //                        txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
        //                    }
        //                    LoadPaymentType();
        //                    if (APSD.PaymentType == 5)
        //                    {
        //                        if (txtPaymentOtherPackage.Items.FindByValue(APSD.OtherBookingID.ToString()) != null)
        //                        {
        //                            txtPaymentOtherPackage.SelectedValue = APSD.OtherBookingID.ToString();
        //                        }
        //                        MyOtherPackageDetail();
        //                    }
        //               // }
        //            }
        //            else if (SMD.IsEvaluation)
        //            {
        //                #region
        //                txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
        //                txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
        //                txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
        //                //txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
        //                LoadEvaluation();
        //                //if (APSD.BulkBookingID > 0)
        //                //{
        //                //    #region
        //                //    Type_Evaluation.Visible = false;
        //                //    txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
        //                //    chkBulkPackage.Checked = true;
        //                //    LoadBulkBooking();
        //                //    if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
        //                //    {
        //                //        txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
        //                //    }
        //                //    LoadBulkBalance();
        //                //    txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
        //                //    #endregion
        //                //}
        //                //else
        //                //{
        //                    #region
        //                    tabPaymentModes.Visible = true; Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
        //                    if (txtEvaluation.Items.FindByValue(APSD.PackageID.ToString()) != null)
        //                    {
        //                        txtEvaluation.SelectedValue = APSD.PackageID.ToString();
        //                    }
        //                    txtEvaluationAmount.Text = APSD.AppointmentCharge.ToString();
        //                    if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
        //                    {
        //                        txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
        //                    }
        //                    LoadPaymentType();
        //                    if (APSD.PaymentType == 3)
        //                    {
        //                        if (txtSessionBankName.Items.FindByValue(APSD.BankID.ToString()) != null)
        //                        {
        //                            txtSessionBankName.SelectedValue = APSD.BankID.ToString();
        //                        }
        //                        //txtSessionBankBranch.Text = APSD.BankBranch;
        //                        //txtSessionChequeNo.Text = APSD.ChequeTxnNo;
        //                        txtSessionChequeDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //                    }
        //                    //if (APSD.PaymentType == 4)
        //                    //{
        //                    //    txtSessionTransactionID.Text = APSD.ChequeTxnNo;
        //                    //    txtSessionTransactionDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //                    //}
        //                    txtAmountToPay.Text = APSD.AmountToPay.ToString();
        //                    hidStatus.Value = "1";
        //                    #endregion
        //               // }
        //                #endregion
        //            }
        //            else
        //            {
        //                #region
        //                txthSingleSession.Value = "1";
        //                txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
        //                txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
        //                txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
        //                txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
        //                //if (APSD.BulkBookingID > 0)
        //                //{
        //                //    #region
        //                //    Type_Single.Visible = false; txtAmountToPay.Enabled = false;
        //                //    txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
        //                //    chkBulkPackage.Checked = true;
        //                //    LoadBulkBooking();
        //                //    if (txtBulkPackages.Items.FindByValue(APSD.BulkBookingID.ToString()) != null)
        //                //    {
        //                //        txtBulkPackages.SelectedValue = APSD.BulkBookingID.ToString();
        //                //    }
        //                //    LoadBulkBalance();
        //                //    txtBulkSessionCharge.Text = APSD.AppointmentCharge.ToString();
        //                //    #endregion
        //                //}
        //                //else
        //                //{
        //                    #region
        //                    tabPaymentModes.Visible = true;
        //                    Type_Single.Visible = true; txtAmountToPay.Enabled = false;
        //                    txtSingleSessionCharge.Text = APSD.AppointmentCharge.ToString();
        //                    if (txtPaymentType.Items.FindByValue(APSD.PaymentType.ToString()) != null)
        //                    {
        //                        txtPaymentType.SelectedValue = APSD.PaymentType.ToString();
        //                    }
        //                    LoadPaymentType();
        //                    if (APSD.PaymentType == 3)
        //                    {
        //                        if (txtSessionBankName.Items.FindByValue(APSD.BankID.ToString()) != null)
        //                        {
        //                            txtSessionBankName.SelectedValue = APSD.BankID.ToString();
        //                        }
        //                        //txtSessionBankBranch.Text = APSD.BankBranch;
        //                        //txtSessionChequeNo.Text = APSD.ChequeTxnNo;
        //                        txtSessionChequeDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //                    }
        //                    //if (APSD.PaymentType == 4)
        //                    //{
        //                    //    txtSessionTransactionID.Text = APSD.ChequeTxnNo;
        //                    //    txtSessionTransactionDate.Text = APSD.ChequeDate > DateTime.MinValue ? APSD.ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : "";
        //                    //}

        //                    txtAmountToPay.Text = APSD.AmountToPay.ToString();
        //                    hidStatus.Value = "1";
        //                    #endregion
        //               // }
        //                #endregion
        //            }
        //        }
        //        else
        //        {
        //            Session[DbHelper.Configuration.messageTextSession] = "Unable to find appointment detail, Please try again.";
        //            Session[DbHelper.Configuration.messageTypeSession] = "2";
        //            Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
        //        }
        //    }
        //    else
        //    {
        //        Session[DbHelper.Configuration.messageTextSession] = "Unable to find appointment detail, Please try again.";
        //        Session[DbHelper.Configuration.messageTypeSession] = "2";
        //        Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true); return;
        //    }
        //}

        protected void txtSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
            txtPaymentType.Items.Clear();
            if (SMD != null)
            {
                if (SMD.IsPackage)
                {
                    txtPaymentType.Items.Clear();
                    txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
                    txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
                    txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Package.Visible = true; txtAmountToPay.Enabled = false;
                    Type_Single.Visible = false; Type_Evaluation.Visible = false;
                    MyPackages(); MyPackageDetail(); LoadPaymentType();
                }
                else if (SMD.IsEvaluation)
                {
                    txtPaymentType.Items.Clear();
                    txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
                    txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                    txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                    txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                    //txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                    //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
                    Type_Single.Visible = false; Type_Package.Visible = false;
                    LoadEvaluation();
                }
                else
                {
                    txtPaymentType.Items.Clear();
                    txthSingleSession.Value = "1";
                    txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
                    txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
                    txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
                    txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
                    //txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
                    //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                    Type_Single.Visible = true; txtAmountToPay.Enabled = false;
                    Type_Evaluation.Visible = false; Type_Package.Visible = false;
                }
            }
            LoadTherapist();

        }

        protected void txtAppointmentDate_TextChanged(object sender, EventArgs e)
        {
            // LoadTherapist();
        }

        private void LoadTherapist()
        {
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);

            SnehBLL.Appointments_Bll APB = new SnehBLL.Appointments_Bll();
            txtTherapist.Items.Clear(); txtTherapist.Items.Add(new ListItem("Select Therapist", "-1"));
            txtAssistant1.Items.Clear(); txtAssistant1.Items.Add(new ListItem("Select Assistant", "-1"));
            txtAssistant1.Items.Add(new ListItem("OBSERVER", "0"));
            foreach (SnehDLL.DoctorMast_Dll DMD in APB.getTherapist(_sessionID, _appointmentDate))
            {
                txtTherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                txtAssistant1.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }

            if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
            bool _hasMultipleDoctor = false; if (txtAssistant1.Items.Count > 0) { txtAssistant1.SelectedIndex = 0; }

            if (SMD != null) { _hasMultipleDoctor = SMD.MultipleDoctor; }
            if (_hasMultipleDoctor)
            {
                Tab_Assistant1.Visible = true;
            }
            else
            {
                Tab_Assistant1.Visible = false;
            }
            LoadTiming();
        }

        protected void txtTherapist_SelectedIndexChanged(object sender, EventArgs e)
        {
            // LoadTiming();
        }

        private void LoadTiming()
        {
            if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
            DateTime _appointmentDate = new DateTime();
            DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentDate);
            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            if (AD_Loaded.PatientID > 0)
            {
                foreach (SnehDLL.Appointments_Dll AD in AB.GetPatientSchedule(AD_Loaded.PatientID, _appointmentDate))
                {
                    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.PatientSchedule(AD.AppointmentTime, AD.Duration))
                    {
                        if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                        {
                            txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:green");
                        }
                    }
                }
            }
            int _doctorID = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _doctorID); }
            if (_doctorID > 0)
            {
                foreach (SnehDLL.Appointments_Dll AD in AB.GetDoctorSchedule(_doctorID, _appointmentDate))
                {
                    foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorSchedule(AD.AppointmentTime, AD.Duration))
                    {
                        if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                        {
                            txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:red");
                        }
                    }
                }
                /********** DOCTOR LEAVE CHECK HERE **********/
                SnehBLL.LeaveApplications_Bll LB = new SnehBLL.LeaveApplications_Bll();
                DataTable dt = LB.DoctorSchedule(_doctorID, _appointmentDate);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int _leaveType = 0; int.TryParse(dt.Rows[i]["TypeID"].ToString(), out _leaveType);
                    if (_leaveType == 4)
                    {
                        int _leaveID = 0; int.TryParse(dt.Rows[i]["LeaveID"].ToString(), out _leaveID);
                        foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.DoctorSchedule(_leaveID))
                        {
                            if (txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()) != null)
                            {
                                txtTimeFrom.Items.FindByValue(ATD.TimeID.ToString()).Attributes.Add("style", "background-color:#EDBF2D");
                            }
                        }
                    }
                    else
                    {
                        foreach (ListItem li in txtTimeFrom.Items)
                        {
                            li.Attributes.Add("style", "background-color:#EDBF2D");
                        }
                        if (txtTimeFrom.Items.Count > 0) { txtTimeFrom.SelectedIndex = 0; }
                        DbHelper.Configuration.setAlert(Page, "Selected therapist is on leave.", 3);
                        break;
                    }
                }
                /********** DOCTOR LEAVE CHECK HERE **********/
            }
        }

        private void MyPackages()
        {
            int SessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out SessionID); }
            txtPatientPackages.Items.Clear(); txtPatientPackages.Items.Add(new ListItem("Select Package", "-1"));
            SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
            List<SnehDLL.PatientSessionPackage_Dll> PKDL = PKB.GetList_ForEdit(_appointmentID, SessionID, AD_Loaded.PatientID);
            foreach (SnehDLL.PatientSessionPackage_Dll PKD in PKDL)
            {
                txtPatientPackages.Items.Add(new ListItem(PKD.PackageCode, PKD.BookingID.ToString()));

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
            int _duration = 0; if (txtDuration.SelectedItem != null) { int.TryParse(txtDuration.SelectedItem.Value, out _duration); }
            SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
            SnehDLL.PatientSessionPackage_Dll PKD = PKB.Get_ForEdit(AD_Loaded.AppointmentID, _bookingID);
            if (PKD != null)
            {
                if (PKD.MaximumTime > 0)
                {
                    if (AD_Loaded.Duration == _duration)
                    {
                        if (AD_Loaded.Duration > PKD.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                            txtSessionCharge.Text = _allAmount.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                            txtBalanceAmount.Text = (PKD.PackageBalance - _allAmount).ToString();
                            txtAmountToPay.Text = _allAmount.ToString(); txtAmountToPay.ReadOnly = true;

                        }
                        else if (AD_Loaded.Duration < PKD.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

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
                        if (_duration > PKD.MaximumTime)
                        {
                            float _oneminuteamtedit = 0; _oneminuteamtedit = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());

                            txtSessionCharge.Text = _allamountedit.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                            txtBalanceAmount.Text = (PKD.PackageBalance - _allamountedit).ToString();
                            txtAmountToPay.Text = _allamountedit.ToString(); txtAmountToPay.ReadOnly = true;
                        }
                        else if (_duration < PKD.MaximumTime)
                        {
                            float _oneminuteamtedit = 0; _oneminuteamtedit = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());

                            txtSessionCharge.Text = _allamountedit.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                            txtBalanceAmount.Text = (PKD.PackageBalance - _allamountedit).ToString();
                            txtAmountToPay.Text = _allamountedit.ToString(); txtAmountToPay.ReadOnly = true;
                        }
                        else
                        {
                            txtSessionCharge.Text = PKD.AppointmentCharge.ToString(); txtPackageBalance.Text = PKD.PackageBalance.ToString();
                            txtBalanceAmount.Text = (PKD.PackageBalance - PKD.AppointmentCharge).ToString();
                            txtAmountToPay.Text = PKD.AppointmentCharge.ToString(); txtAmountToPay.ReadOnly = true;
                        }
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

        private void LoadEvaluation()
        {
            int SessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out SessionID); }
            txtEvaluationAmount.Text = "";
            txtEvaluation.Items.Clear(); txtEvaluation.Items.Add(new ListItem("Select Evaluation", "-1"));
            if (AD_Loaded.PatientID > 0)
            {
                SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
                foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackage(_appointmentID, SessionID, AD_Loaded.PatientID))
                {
                    txtEvaluation.Items.Add(new ListItem(PKD.PackageCode, PKD.PackageID.ToString()));
                }
            }
        }

        //protected void chkBulkPackage_CheckedChanged(object sender, EventArgs e)
        //{
        //    //txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
        //    //txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
        //    //txthSingleSession.Value = "0"; txthBulkPackage.Value = "0"; Type_BulkPay.Visible = false;
        //    //Type_Package.Visible = false; txtAmountToPay.Enabled = false;
        //    //Type_Evaluation.Visible = false; txtAmountToPay.Enabled = false;
        //    //Type_Single.Visible = false; txtAmountToPay.Enabled = false;
        //    //tabPaymentModes.Visible = false; tabPaymentBulkMode.Visible = false;
        //    //txtPackageBalance.Text = string.Empty; txtSessionCharge.Text = string.Empty; txtBalanceAmount.Text = string.Empty;
        //    //txtEvaluationAmount.Text = string.Empty; txtSingleSessionCharge.Text = string.Empty; txtBulkSessionCharge.Text = string.Empty;
        //    //txtAmountToPay.Text = string.Empty; txtBulkBalance.Text = string.Empty;
        //    //tb_SessionBank.Visible = false; tb_SessionOnline.Visible = false; tb_SessionOtherPackages.Visible = false;
        //    //txtBulkForword.Text = string.Empty;

        //    //if (chkBulkPackage.Checked)
        //    //{
        //    //    txthBulkPackage.Value = "1"; Type_BulkPay.Visible = true; tabPaymentBulkMode.Visible = true;
        //    //    LoadBulkBooking(); LoadBulkBalance();
        //    //}
        //    //else
        //    //{
        //    //    int SessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out SessionID); }
        //    //    tabPaymentModes.Visible = true; if (txtEvaluation.Items.Count > 0) { txtEvaluation.SelectedIndex = 0; }
        //    //    SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        //    //    SnehDLL.SessionMast_Dll SMD = SMB.Get(SessionID);
        //    //    if (SMD != null)
        //    //    {
        //    //        if (SMD.IsPackage)
        //    //        {
        //    //            txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
        //    //            txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
        //    //            Type_Package.Visible = true; txtAmountToPay.Enabled = false;
        //    //            MyPackages(); MyPackageDetail(); LoadPaymentType();

        //    //        }
        //    //        else if (SMD.IsEvaluation)
        //    //        {
        //    //            txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
        //    //            txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
        //    //            txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
        //    //            txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
        //    //            //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
        //    //            Type_Evaluation.Visible = true; txtAmountToPay.Enabled = false;
        //    //            LoadEvaluation();
        //    //        }
        //    //        else
        //    //        {
        //    //            txthSingleSession.Value = "1";
        //    //            txtPaymentType.Items.Add(new ListItem("Cash Payment", "1"));
        //    //            txtPaymentType.Items.Add(new ListItem("Credit Payment", "2"));
        //    //            txtPaymentType.Items.Add(new ListItem("Cheque Payment", "3"));
        //    //            txtPaymentType.Items.Add(new ListItem("Online Payment", "4"));
        //    //            //txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
        //    //            Type_Single.Visible = true; txtAmountToPay.Enabled = false;
        //    //        }
        //    //    }
        //    //}
        //}

        //protected void txtBulkPackages_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //   // LoadBulkBalance();
        //}

        //private void LoadBulkBooking()
        //{
        //    txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
        //    SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
        //    List<SnehDLL.PatientBulk_Dll> DL = PB.ListPackage_ForEdit(AD_Loaded.PatientID, AD_Loaded.AppointmentID);
        //    foreach (SnehDLL.PatientBulk_Dll item in DL)
        //    {
        //        txtBulkPackages.Items.Add(new ListItem(item.Amount + "[" + item.PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]", item.BulkID.ToString()));
        //    }
        //}

        private void LoadBulkBooking()
        {
            //txtBulkPackages.Items.Clear();
            //txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
            SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
            List<SnehDLL.PatientBulk_Dll> DL = PB.ListPackage_ForEdit(AD_Loaded.PatientID, AD_Loaded.AppointmentID);
            //foreach (SnehDLL.PatientBulk_Dll item in DL)
            //{
            //    txtBulkPackages.Items.Add(new ListItem(item.Amount + "[" + item.PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]", item.BulkID.ToString()));
            //}
            decimal Total = 0; Session["bulkid"] = DL[0].BulkID;
            var balAmt = Convert.ToDecimal(DL[0].BalanceAmount);
            decimal newtotal = Convert.ToDecimal(DL[0].Amount);
            int hh = 0;
            foreach (SnehDLL.PatientBulk_Dll item in DL)
            {
                if (DL.Count == 1) // if(DL.count ==1)
                {
                    txtBulkBalance.Text = DL[0].BalanceAmount.ToString();
                    txtBulkForword.Text = DL[0].BalanceAmount.ToString();
                    Total = Total + Convert.ToDecimal(item.Amount);
                    txtBulkPackages.Text = Total.ToString() + "[" + item.PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]";
                }
                else
                {
                    Total = Total + Convert.ToDecimal(item.Amount);

                    if (hh > 0)
                    {
                        //for (int i = 1; i < DL.Count(); i++)
                        balAmt = balAmt + Convert.ToDecimal(item.Amount);
                        txtBulkBalance.Text = balAmt.ToString();
                        txtBulkForword.Text = balAmt.ToString();
                        //Total = Total + Convert.ToDecimal(item.Amount);
                        txtBulkPackages.Text = Total.ToString() + "[" + item.PaidDate.ToString(DbHelper.Configuration.showDateFormat) + "]";
                    }
                }
                hh = 1;
                Session["new_bulk"] = txtBulkPackages.Text;
            }

            LoadBulkPackages();
        }

        private void LoadBulkPackages()
        {
            //int BulkID = 0; if (txtBulkPackages.SelectedItem != null) { int.TryParse(txtBulkPackages.SelectedItem.Value, out BulkID); }
            int BulkID = 0; if (txtBulkPackages.Text != null) { int.TryParse(txtBulkPackages.Text, out BulkID); }
            if (BulkID > 0)
            {
                SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
                List<SnehDLL.PatientBulkPackage_Dll> PBD = PB.GetPatientBulkPackageFull(BulkID);
                if (PBD.Count > 0)
                {
                    bulkpack.Visible = true; txtbulkpackagesnew.Items.Clear(); Type_BulkPay.Visible = false;
                    //txtbulkpackagesnew.Items.Add(new ListItem("Select Package", "-1"));
                    foreach (var item in PBD)
                    {
                        txtbulkpackagesnew.Items.Add(new ListItem(item.PackageCode, item.PackageID.ToString()));
                    }
                    if (AD_Loaded.BulkPackageID > 0)
                    {
                        if (txtbulkpackagesnew.Items.FindByValue(AD_Loaded.BulkPackageID.ToString()) != null)
                        {
                            txtbulkpackagesnew.SelectedValue = AD_Loaded.BulkPackageID.ToString();
                        }
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
        private void CalculateBulkPackage()
        {
            bulkusedamntchrg.Visible = false; bulkpackage.Visible = false; float balamount = 0;
            //int BulkID = 0; if (txtBulkPackages.SelectedItem != null) { int.TryParse(txtBulkPackages.SelectedItem.Value, out BulkID); }
            int BulkID = 0; if (txtBulkPackages.Text != null) { int.TryParse(txtBulkPackages.Text, out BulkID); }
            int packageid = 0; if (txtbulkpackagesnew.SelectedItem != null) { int.TryParse(txtbulkpackagesnew.SelectedItem.Value, out packageid); }
            SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll(); balamount = PB.GetBalAmount(BulkID);
            SnehDLL.PatientBulkPackage_Dll PKD = PB.GetPatientBulkPackageByPackageNew(BulkID, packageid);

            if (PKD != null)
            {
                bulkusedamntchrg.Visible = true; bulkpackage.Visible = true; float balamountnew = 0; float bulkforward = 0;
                txtusedappointmentcharge.Text = PKD.UsedAppointmentCharge.ToString();
                txtusedappointmentcount.Text = PKD.UsedAppointmentCount.ToString();

                if (PKD.MaximumTime > 0)
                {
                    if (AD_Loaded.Duration > PKD.MaximumTime)
                    {
                        float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                        float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                        balamountnew = PKD.PackageAmount - balamount + _allAmount;
                        txtbulkappointmentcharge.Text = _allAmount.ToString();
                        txtBulkBalance.Text = balamountnew.ToString();
                        bulkforward = balamountnew - _allAmount;
                        txtBulkForword.Text = bulkforward.ToString();
                    }
                    else if (AD_Loaded.Duration < PKD.MaximumTime)
                    {
                        float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                        float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                        balamountnew = PKD.PackageAmount - balamount + _allAmount;
                        txtbulkappointmentcharge.Text = _allAmount.ToString();
                        txtBulkBalance.Text = balamountnew.ToString();
                        bulkforward = balamountnew - _allAmount;
                        txtBulkForword.Text = bulkforward.ToString();
                    }
                    else
                    {
                        balamountnew = PKD.PackageAmount - balamount + PKD.AppointmentCharge;
                        txtbulkappointmentcharge.Text = PKD.AppointmentCharge.ToString();
                        txtBulkBalance.Text = balamountnew.ToString();
                        bulkforward = balamountnew - PKD.AppointmentCharge;
                        txtBulkForword.Text = bulkforward.ToString();
                    }
                }
                else
                {
                    balamountnew = PKD.PackageAmount - balamount + PKD.AppointmentCharge;
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
        protected void txtbulkpackagesnew_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateBulkPackage();
        }
        private void LoadBulkBalance()
        {
            //long BulkID = 0; if (txtBulkPackages.SelectedItem != null) { long.TryParse(txtBulkPackages.SelectedItem.Value, out BulkID); }
            long BulkID = 0; if (txtBulkPackages.Text != null) { long.TryParse(txtBulkPackages.Text, out BulkID); }
            SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
            SnehDLL.PatientBulk_Dll item = PB.ListPackage_ForEdit(AD_Loaded.PatientID, AD_Loaded.AppointmentID).Find(f => f.BulkID == BulkID);
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
        //private void LoadBulkBalance()
        //{
        //    long BulkID = 0; if (txtBulkPackages.SelectedItem != null) { long.TryParse(txtBulkPackages.SelectedItem.Value, out BulkID); }
        //    SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
        //    SnehDLL.PatientBulk_Dll item = PB.ListPackage_ForEdit(AD_Loaded.PatientID, AD_Loaded.AppointmentID).Find(f => f.BulkID == BulkID);
        //    if (item != null)
        //    {
        //        txtBulkBalance.Text = item.BalanceAmount.ToString();
        //    }
        //    else
        //    {
        //        txtBulkBalance.Text = "0";
        //    }
        //}

        protected void chkBulkPackage_CheckedChanged(object sender, EventArgs e)
        {
            txtPaymentType.Items.Clear(); txtPaymentType.Items.Add(new ListItem("Select Payment Type", "-1"));
            // txtBulkPackages.Items.Clear(); txtBulkPackages.Items.Add(new ListItem("Select Booking", "-1"));
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
                int SessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out SessionID); }
                tabPaymentModes.Visible = true; if (txtEvaluation.Items.Count > 0) { txtEvaluation.SelectedIndex = 0; }
                SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
                SnehDLL.SessionMast_Dll SMD = SMB.Get(SessionID);
                if (SMD != null)
                {
                    if (SMD.IsPackage)
                    {
                        txtPaymentType.Items.Add(new ListItem("Selected Package", "4"));
                        txtPaymentType.Items.Add(new ListItem("Other Package", "5"));
                        Type_Package.Visible = true; txtAmountToPay.Enabled = false;
                        MyPackages(); MyPackageDetail(); LoadPaymentType();

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

        protected void txtPaymentOtherPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyOtherPackageDetail();
        }

        protected void txtEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEvaluationAmount.Text = ""; txtAmountToPay.Text = "";
            int _packageID = 0; if (txtEvaluation.SelectedItem != null) { int.TryParse(txtEvaluation.SelectedItem.Value, out _packageID); }
            int _duration = 0; if (txtDuration.SelectedItem != null) { int.TryParse(txtDuration.SelectedItem.Value, out _duration); }
            SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
            SnehDLL.Packages_Dll PKD = PKB.Get(_packageID);
            if (PKD != null)
            {
                if (PKD.MaximumTime > 0)
                {
                    if (AD_Loaded.Duration == _duration)
                    {
                        if (AD_Loaded.Duration > PKD.MaximumTime)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                            txtEvaluationAmount.Text = _allAmount.ToString(); txtAmountToPay.Text = _allAmount.ToString(); txtAmountToPay.ReadOnly = true;
                        }
                        else
                        {
                            txtEvaluationAmount.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.ReadOnly = true;
                        }
                    }
                    else
                    {
                        if (_duration > PKD.MaximumTime)
                        {
                            float _oneminuteamtedit = 0; _oneminuteamtedit = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());
                            txtEvaluationAmount.Text = _allamountedit.ToString(); txtAmountToPay.Text = _allamountedit.ToString(); txtAmountToPay.ReadOnly = true;
                        }
                        else
                        {
                            txtEvaluationAmount.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.Text = PKD.OneTimeAmt.ToString(); txtAmountToPay.ReadOnly = true;
                        }
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
            LoadPaymentType();
        }

        protected void txtDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyPackageDetail();
        }

        private void LoadPaymentType()
        {
            tb_SessionBank.Visible = false; tb_SessionOtherPackages.Visible = false; //tb_SessionOnline.Visible = false;
            int _paymentType = 0; if (txtPaymentType.SelectedItem != null) { int.TryParse(txtPaymentType.SelectedItem.Value, out _paymentType); }

            if (_paymentType == 3)
            {
                tb_SessionBank.Visible = true;
            }
            //if (_paymentType == 4)
            //{
            //    int SessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out SessionID); }
            //    tb_SessionOnline.Visible = true;
            //    SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            //    SnehDLL.SessionMast_Dll SMD = SMB.Get(SessionID);
            //    if (SMD != null)
            //    {
            //        if (SMD.IsPackage)
            //        {
            //            //tb_SessionOnline.Visible = false;
            //        }
            //    }
            //    MyPackageDetail();
            //}
            else if (_paymentType == 5)
            {
                tb_SessionOtherPackages.Visible = true;
                MyOtherPackages();
            }
        }

        private void MyOtherPackages()
        {
            txtPaymentOtherPackage.Items.Clear(); txtPaymentOtherPackage.Items.Add(new ListItem("Select Package", "-1"));
            int _bookingID = 0; if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _bookingID); }
            SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
            List<SnehDLL.PatientSessionPackage_Dll> PKDL = PKB.GetList_ForEdit(AD_Loaded.AppointmentID, AD_Loaded.PatientID).Where(r => r.BookingID != _bookingID).ToList();
            foreach (SnehDLL.PatientSessionPackage_Dll PKD in PKDL)
            {
                txtPaymentOtherPackage.Items.Add(new ListItem(PKD.PackageCode, PKD.BookingID.ToString()));
            }
            MyOtherPackageDetail();
        }

        private void MyOtherPackageDetail()
        {
            txtPaymentOtherPackageBalance.Text = ""; txtPaymentOtherBalanceAmount.Text = "";
            int _bookingID = 0; if (txtPaymentOtherPackage.SelectedItem != null) { int.TryParse(txtPaymentOtherPackage.SelectedItem.Value, out _bookingID); }
            SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll();
            SnehDLL.PatientSessionPackage_Dll PKD = PKB.Get_ForEdit(AD_Loaded.AppointmentID, _bookingID);
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

            int _bookingID = 0; DateTime _chequeDate = new DateTime(); int _packageID = 0; float _sessionCharge = 0;
            float _PackageTotalBalance = 0; float _PackageNewBalance = 0;
            int _bankID = 0; string BankBranch = string.Empty; string ChequeTxnNo = string.Empty;
            int _otherBookingID = 0; float _OtherBookingTotalBalance = 0; float _OtherBookingNewBalance = 0;
            float _payableAmount = 0;
            DateTime _entryDate = new DateTime(); DateTime.TryParseExact(txtEntryDateTime.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _entryDate);

            int _sessionid = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionid); }
            DateTime _appointmentdate = new DateTime(); DateTime.TryParseExact(txtAppointmentDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentdate);
            int _therapist = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _therapist); }
            int _timefrom = 0; if (txtTimeFrom.SelectedItem != null) { int.TryParse(txtTimeFrom.SelectedItem.Value, out _timefrom); }
            int _duration = 0; if (txtDuration.SelectedItem != null) { int.TryParse(txtDuration.SelectedItem.Value, out _duration); }
            DateTime _entrydatetime = new DateTime(); DateTime.TryParseExact(txtEntryDateTime.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _entrydatetime);
            int _patientpackage = 0; if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _patientpackage); }
            int _therapistedit = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _therapistedit); }

            if (_therapistedit <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select therapist for appointment...", 2); return;
            }
            //float _packagebalance = float.Parse(txtPackageBalance.Text);
            // float _sessioncharge = float.Parse(txtSessionCharge.Text);
            //float _balanceamount = float.Parse(txtBalanceAmount.Text);


            if (_sessionid < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select session", 2); return;
            }
            int _assistantTherapistedit = -1; float _assistantShare = 0; int _assistantShareType = 0;
            bool _hasMultipleDoctor = false;
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll(); SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionid);
            if (SMD != null)
            {
                _hasMultipleDoctor = SMD.MultipleDoctor;
                if (_hasMultipleDoctor)
                {
                    if (txtAssistant1.SelectedItem != null) { int.TryParse(txtAssistant1.SelectedItem.Value, out _assistantTherapistedit); }
                    float.TryParse(txtAssistantShare.Text, out _assistantShare);
                    if (txtAssistantShareType.SelectedItem != null) { int.TryParse(txtAssistantShareType.SelectedItem.Value, out _assistantShareType); }
                }
                SnehDLL.PatientSessionPackage_Dll PKD = null; SnehDLL.PatientSessionPackage_Dll PKO = null; SnehDLL.Packages_Dll PKE = null;
                //if (!chkBulkPackage.Checked)
                //{
                    if (SMD.IsPackage)
                    {
                        if (txtPatientPackages.SelectedItem != null) { int.TryParse(txtPatientPackages.SelectedItem.Value, out _bookingID); }
                        if (_bookingID <= 0)
                        {
                            DbHelper.Configuration.setAlert(Page, "Please select patient package, and try again...", 2); return;
                        }
                        SnehBLL.PatientSessionPackage_Bll PKB = new SnehBLL.PatientSessionPackage_Bll(); PKD = PKB.Get_ForEdit(AD_Loaded.AppointmentID, _bookingID);
                        if (PKD == null)
                        {
                            DbHelper.Configuration.setAlert(Page, "Unable to find patient package, and try again...", 2); return;
                        }
                        /*******************************************************/
                        if (PKD.MaximumTime > 0)
                        {
                            if (AD_Loaded.Duration == _duration)
                            {

                                if (AD_Loaded.Duration > PKD.MaximumTime)
                                {
                                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                                    _sessionCharge = _allAmount;
                                }
                                else if (AD_Loaded.Duration < PKD.MaximumTime)
                                {
                                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

                                    _sessionCharge = _allAmount;
                                }
                                else
                                {
                                    _sessionCharge = PKD.AppointmentCharge;
                                }
                            }
                            else
                            {
                                if (_duration > PKD.MaximumTime)
                                {
                                    float _oneminuteamtedit = 0; _oneminuteamtedit = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                                    float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());
                                    _sessionCharge = _allamountedit;
                                }
                                else if (_duration < PKD.MaximumTime)
                                {
                                    float _oneminuteamtedit = 0; _oneminuteamtedit = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                                    float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());

                                    _sessionCharge = _allamountedit;
                                }
                                else
                                {
                                    _sessionCharge = PKD.AppointmentCharge;
                                }
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
                            if (AD_Loaded.Duration == _duration)
                            {
                                if (AD_Loaded.Duration > PKE.MaximumTime)
                                {
                                    float _oneMinuteAmt = 0; _oneMinuteAmt = (PKE.OneTimeAmt / float.Parse(PKE.MaximumTime.ToString()));
                                    float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());
                                    _sessionCharge = _allAmount;
                                }
                                else
                                {
                                    _sessionCharge = PKE.OneTimeAmt;
                                }
                            }
                            else
                            {
                                if (_duration > PKE.MaximumTime)
                                {
                                    float _oneminuteamtedit = 0; _oneminuteamtedit = (PKE.OneTimeAmt / float.Parse(PKE.MaximumTime.ToString()));
                                    float _allamountedit = float.Parse(Math.Round(decimal.Parse((_duration * _oneminuteamtedit).ToString()), 2).ToString());
                                    _sessionCharge = _allamountedit;
                                }
                                else
                                {
                                    _sessionCharge = PKE.OneTimeAmt;
                                }
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
                        //BankBranch = txtSessionBankBranch.Text.Trim(); ChequeTxnNo = txtSessionChequeNo.Text.Trim();
                        //if (string.IsNullOrEmpty(ChequeTxnNo))
                        //{
                        //    DbHelper.Configuration.setAlert(Page, "Please enter cheque number...", 2); return;
                        //}
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
                    //if (_paymentType == 4)
                    //{
                    //    if (SMD.IsPackage)
                    //    {
                    //        if (PKD.PackageBalance <= 0)
                    //        {
                    //            DbHelper.Configuration.setAlert(Page, "Selected patient package balance is insufficient...", 2); return;
                    //        }
                    //        if ((PKD.PackageBalance - _sessionCharge) < 0)
                    //        {
                    //            DbHelper.Configuration.setAlert(Page, "Selected patient package balance is insufficient...", 2); return;
                    //        }
                    //        _PackageTotalBalance = PKD.PackageBalance;
                    //        _PackageNewBalance = PKD.PackageBalance - _sessionCharge;
                    //    }
                    //    else
                    //    {
                    //        ChequeTxnNo = txtSessionTransactionID.Text.Trim();
                    //        DateTime.TryParseExact(txtSessionTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
                    //        if (string.IsNullOrEmpty(ChequeTxnNo))
                    //        {
                    //            DbHelper.Configuration.setAlert(Page, "Please enter transaction id...", 2); return;
                    //        }
                    //        if (_chequeDate <= DateTime.MinValue)
                    //        {
                    //            DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
                    //        }
                    //        if (_chequeDate >= DateTime.MaxValue)
                    //        {
                    //            DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
                    //        }
                    //    }
                    //}
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
                            _OtherBookingNewBalance = PKO.PackageBalance - _sessionCharge;
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
                    //ASN.BankBranch = BankBranch;
                    //ASN.ChequeTxnNo = ChequeTxnNo;
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
                    ASB.AppointmentPaidEdit_Delete(_appointmentID);
                    DataSet ds = ASB.Hospital_And_Patient(_appointmentID);
                    DataTable dt = new DataTable();
                    DataTable dtnew = new DataTable();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            int Hledgerid = 0; int.TryParse(dt.Rows[0]["LedgerHeadID"].ToString(), out Hledgerid);
                            ASB.Update_HospitalLeger(Hledgerid);
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dtnew = ds.Tables[1];
                        if (dtnew.Rows.Count > 0)
                        {
                            int PledgerID = 0; int.TryParse(dt.Rows[0]["LedgerHeadID"].ToString(), out PledgerID);
                            ASB.Update_PatientLeger(PledgerID);
                        }
                    }
                    int i = ASB.Set(ASN, 1);
                    if (i > 0)
                    {
                        SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
                        SnehDLL.AppointmentDoctor_Dll ADD = new SnehDLL.AppointmentDoctor_Dll();
                        if (_therapistedit > 0)
                        {
                            ADD.AppointmentID = _appointmentID; ADD.DoctorID = _therapistedit; ADD.IsMain = true; ADD.ShareType = 2; ADD.ShareAmount = 100;
                            ADB.Delete(i);
                            ADB.setNew(ADD);
                        }
                        if (_assistantTherapistedit > -1)
                        {
                            ADD.AppointmentID = _appointmentID; ADD.DoctorID = _assistantTherapistedit; ADD.IsMain = false; ADD.ShareType = 2; ADD.ShareAmount = 0;
                            ADB.setNew(ADD);
                        }
                        DateTime _appointmentdateedit = new DateTime();
                        DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentdateedit);
                        int _timeID = 0; if (txtTimeFrom.SelectedItem != null) { int.TryParse(txtTimeFrom.SelectedItem.Value, out _timeID); }
                        TimeSpan _appointmentTimeEdit = new TimeSpan();
                        SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
                        SnehDLL.AppointmentTime_Dll ATD = ATB.Get(_timeID);
                        if (ATD != null)
                        {
                            _appointmentTimeEdit = ATD.TimeHour;
                        }
                        SnehBLL.Appointments_Bll A = new SnehBLL.Appointments_Bll();
                        A.EditAppointment(_appointmentID, _sessionid, _duration, _appointmentdateedit, _appointmentTimeEdit);

                        if (SMD.IsPackage)
                        {
                            int _deductBookingID = _bookingID; if (_otherBookingID > 0) { _deductBookingID = _otherBookingID; }
                            if (ASB.SetPackagePayment(i, _deductBookingID) > 0)
                            {
                                ASB.DeleteDoctorPayment(i);
                                ASB.SetDoctorPayment(i);

                                Session[DbHelper.Configuration.messageTextSession] = "Patient session entry updated successfully.";
                                Session[DbHelper.Configuration.messageTypeSession] = "1";

                                if (toReturn == 101)
                                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                                else
                                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true);
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
                                ASB.DeleteDoctorPayment(i);
                                ASB.SetDoctorPayment(i);

                                Session[DbHelper.Configuration.messageTextSession] = "Patient session entry updated successfully.";
                                Session[DbHelper.Configuration.messageTypeSession] = "1";

                                if (toReturn == 101)
                                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                                else
                                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true);
                            }
                            else
                            {
                                ASB.Delete(i);
                                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                            }
                        }
                        else
                        {
                            if (ASB.UpdateOtherPayment(i) > 0)
                            {
                                ASB.DeleteDoctorPayment(i);
                                ASB.SetDoctorPayment(i);

                                Session[DbHelper.Configuration.messageTextSession] = "Patient session entry updated successfully.";
                                Session[DbHelper.Configuration.messageTypeSession] = "1";

                                if (toReturn == 101)
                                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                                else
                                    Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true);
                            }
                            else if (ASB.UpdateOtherPayment(i) <= 0)
                            {
                                if (ASB.SetOtherPayment(i) > 0)
                                {
                                    ASB.DeleteDoctorPayment(i);
                                    ASB.SetDoctorPayment(i);

                                    Session[DbHelper.Configuration.messageTextSession] = "Patient session entry updated successfully.";
                                    Session[DbHelper.Configuration.messageTypeSession] = "1";

                                    if (toReturn == 101)
                                        Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                                    else
                                        Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true);
                                }
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
                //}
                //else
                //{
                    //float bulkSessionCharges = 0; float.TryParse(txtBulkSessionCharge.Text.Trim(), out bulkSessionCharges);
                    //if (bulkSessionCharges <= 0)
                    //{
                    //    DbHelper.Configuration.setAlert(Page, "Please enter session charges...", 2); return;
                    //}
                    //long bulkID = 0; if (txtBulkPackages.SelectedItem != null) { long.TryParse(txtBulkPackages.SelectedItem.Value, out bulkID); }
                    //if (bulkID <= 0)
                    //{
                    //    DbHelper.Configuration.setAlert(Page, "Please select bulk package...", 2); return;
                    //}
                    //SnehBLL.AppointmentSession_Bll ASB = new SnehBLL.AppointmentSession_Bll();
                    //ASB.AppointmentPaidEdit_Delete(_appointmentID);
                    //int i = ASB.Set(AD_Loaded.AppointmentID, bulkSessionCharges, bulkID, _entryDate, _loginID, txtNarration.Text.Trim(), 1);
                    //if (i > 0)
                    //{

                    //    SnehBLL.AppointmentDoctor_Bll ADB = new SnehBLL.AppointmentDoctor_Bll();
                    //    SnehDLL.AppointmentDoctor_Dll ADD = new SnehDLL.AppointmentDoctor_Dll();
                    //    if (_therapistedit > 0)
                    //    {
                    //        ADD.AppointmentID = _appointmentID; ADD.DoctorID = _therapistedit; ADD.IsMain = true; ADD.ShareType = 2; ADD.ShareAmount = 100;
                    //        ADB.Delete(i);
                    //        ADB.setNew(ADD);
                    //    }
                    //    if (_assistantTherapistedit > -1)
                    //    {
                    //        ADD.AppointmentID = _appointmentID; ADD.DoctorID = _assistantTherapistedit; ADD.IsMain = false; ADD.ShareType = 2; ADD.ShareAmount = 0;
                    //        ADB.setNew(ADD);
                    //    }
                    //    DateTime _appointmentdateedit = new DateTime();
                    //    DateTime.TryParseExact(txtAppointmentDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _appointmentdateedit);
                    //    int _timeID = 0; if (txtTimeFrom.SelectedItem != null) { int.TryParse(txtTimeFrom.SelectedItem.Value, out _timeID); }
                    //    TimeSpan _appointmentTimeEdit = new TimeSpan();
                    //    SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
                    //    SnehDLL.AppointmentTime_Dll ATD = ATB.Get(_timeID);
                    //    if (ATD != null)
                    //    {
                    //        _appointmentTimeEdit = ATD.TimeHour;
                    //    }
                    //    SnehBLL.Appointments_Bll A = new SnehBLL.Appointments_Bll();
                    //    A.EditAppointment(_appointmentID, _sessionid, _duration, _appointmentdateedit, _appointmentTimeEdit);
                    //    if (ASB.SetBulkPayment(i) > 0)
                    //    {
                    //        ASB.DeleteDoctorPayment(i);
                    //        ASB.SetDoctorPayment(i);

                    //        Session[DbHelper.Configuration.messageTextSession] = "Patient session entry updated successfully.";
                    //        Session[DbHelper.Configuration.messageTypeSession] = "1";

                    //        if (toReturn == 101)
                    //            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                    //        else
                    //            Response.Redirect(ResolveClientUrl("~/Member/Appointmentc.aspx"), true);
                    //    }
                    //    else
                    //    {
                    //        ASB.Delete(i);
                    //        DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                    //    }
                    //}
                    //else
                    //{
                    //    DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
                    //}
                //}
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to find session detail, please try again..", 2);
            }
        }

        protected void btnNewPackage_Click(object sender, EventArgs e)
        {
            tb_Contents.ActiveTabIndex = 1; tb_Session.Enabled = false; tb_Packages.Enabled = true;
            LoadPackages();
        }

        protected void btnCancelPackage_Click(object sender, EventArgs e)
        {
            tb_Contents.ActiveTabIndex = 0; tb_Session.Enabled = true; tb_Packages.Enabled = false;
        }

        private void LoadPackages()
        {
            int _session = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _session); }
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
            if (AD_Loaded.PatientID > 0)
            {
                SnehBLL.Packages_Bll PKB = new SnehBLL.Packages_Bll();
                foreach (SnehDLL.Packages_Dll PKD in PKB.GetSessionPackageNewFilter(_appointmentID, _session, AD_Loaded.PatientID))
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
                    if (AD_Loaded.Duration > PKD.MaximumTime)
                    {
                        float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                        float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

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
                    else if (AD_Loaded.Duration < PKD.MaximumTime)
                    {
                        if (PKD.Appointments == 1)
                        {
                            float _oneMinuteAmt = 0; _oneMinuteAmt = (PKD.OneTimeAmt / float.Parse(PKD.MaximumTime.ToString()));
                            float _allAmount = float.Parse(Math.Round(decimal.Parse((AD_Loaded.Duration * _oneMinuteAmt).ToString()), 2).ToString());

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

        protected void txtPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _paymentMode = 0; if (txtPaymentMode.SelectedItem != null) { int.TryParse(txtPaymentMode.SelectedItem.Value, out _paymentMode); }
            tab_Cheque.Visible = false; 
            //tab_online.Visible = false;
            if (txtBankName.Items.Count > 0) { txtBankName.SelectedIndex = 0; }
            //txtBranchName.Text = ""; txtChequeNo.Text = ""; 
            txtChequeDate.Text = "";
            //txtTransactionID.Text = ""; txtTransactionDate.Text = "";
            if (_paymentMode == 3)
            {
                tab_Cheque.Visible = true;
            }
            //if (_paymentMode == 4)
            //{
            //    tab_online.Visible = true;
            //}
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
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            int _therapist = 0; if (txtTherapist.SelectedItem != null) { int.TryParse(txtTherapist.SelectedItem.Value, out _therapist); }
            int _assistant1 = 0; if (txtAssistant1.SelectedItem != null) { int.TryParse(txtAssistant1.SelectedItem.Value, out _assistant1); }
            if (_paymentMode == 3)
            {
                if (txtBankName.SelectedItem != null) { int.TryParse(txtBankName.SelectedItem.Value, out _bankID); }
                //BankBranch = txtBranchName.Text.Trim(); ChequeTxnNo = txtChequeNo.Text.Trim();
                DateTime.TryParseExact(txtChequeDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
                if (_bankID <= 0)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select bank name...", 2); return;
                }
                //if (string.IsNullOrEmpty(ChequeTxnNo))
                //{
                //    DbHelper.Configuration.setAlert(Page, "Please enter cheque number...", 2); return;
                //}
                if (_chequeDate <= DateTime.MinValue)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
                }
                if (_chequeDate >= DateTime.MaxValue)
                {
                    DbHelper.Configuration.setAlert(Page, "Please select correct cheque date...", 2); return;
                }
            }
            //if (_paymentMode == 4)
            //{
            //    ChequeTxnNo = txtTransactionID.Text.Trim();
            //    DateTime.TryParseExact(txtTransactionDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _chequeDate);
            //    if (string.IsNullOrEmpty(ChequeTxnNo))
            //    {
            //        DbHelper.Configuration.setAlert(Page, "Please enter transaction id...", 2); return;
            //    }
            //    if (_chequeDate <= DateTime.MinValue)
            //    {
            //        DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            //    }
            //    if (_chequeDate >= DateTime.MaxValue)
            //    {
            //        DbHelper.Configuration.setAlert(Page, "Please select correct transaction date...", 2); return;
            //    }
            //}
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
            PPD.PatientID = AD_Loaded.PatientID; PPD.SessionID = AD_Loaded.SessionID;
            PPD.PackageID = _packageID; PPD.AppointmentCharge = _appointmentCharge;
            PPD.AppointmentCount = _appointmentCount; PPD.PackageAmount = _packageAmount;
            PPD.ModePayment = _paymentMode; PPD.BankID = _bankID; 
            //PPD.BankBranch = BankBranch;
            PPD.Narration = _narration; 
            //PPD.ChequeTxnNo = ChequeTxnNo; PPD.ChequeDate = _chequeDate;
            PPD.AddedDate = _entryDate; PPD.ModifyDate = _entryDate;
            PPD.AddedBy = _loginID; PPD.ModifyBy = _loginID;
            PPD.ExtraCharge = _extraSessionCharge;
            PPD.IsDiscounted = _isDiscounted; PPD.DiscountType = _discountType;
            PPD.DiscountValue = _discountValue; PPD.DiscountAmt = _discountAmt;
            PPD.NetAmt = _netAmt;
            PPD.DiscountedOn = _discountedOn; PPD.NewSessionCharge = _newSessionCharge;

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
                Session[DbHelper.Configuration.messageTextSession] = "Patient package booking saved successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                if (Tab_Assistant1.Visible == true)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentPaidEdit.aspx?record=" + Request.QueryString["record"].ToString() + "&sessionid=" + SnehBLL.SessionMast_Bll.Check(_sessionID) + "&therapist=" + _therapist + "&assistant1=" + _assistant1), true);
                }
                else
                {
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentPaidEdit.aspx?record=" + Request.QueryString["record"].ToString() + "&sessionid=" + SnehBLL.SessionMast_Bll.Check(_sessionID) + "&therapist=" + _therapist), true);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
    }
}