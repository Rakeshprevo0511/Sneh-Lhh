using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace snehrehab.SessionRpt
{
    public partial class Demo_BotoxRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            //if (Request.QueryString["record"] != null)
            //{
            //    if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
            //    {
            //        _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
            //    }
            //}
            //_cancelUrl = "/SessionRpt/BotoxView.aspx";
            //if (_appointmentID > 0)
            //{
            //    if (!IsPostBack)
            //    {
            //        LoadForm();
            //    }
            //}
            //else
            //{
            //    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            //}
            if (!IsPostBack)
            {
                LoadForm();
            }
            //_printUrl = txtPrint.Value;
        }

        private void LoadForm()
        {
            SnehBLL.ReportBotoxMst_Bll RDB = new SnehBLL.ReportBotoxMst_Bll();

            //if (!RDB.IsValid(_appointmentID))
            //{
            //    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            //}
            //bool HasDiagnosisID = false;
            //DataTable dt = RDB.GetTop(_appointmentID);
            //if (dt.Rows.Count > 0)
            //{
            //    txtPatient.Text = dt.Rows[0]["FullName"].ToString();
            //    txtSession.Text = dt.Rows[0]["SessionName"].ToString();
            //    bool.TryParse(dt.Rows[0]["HasDiagnosisID"].ToString(), out HasDiagnosisID);
            //}
            //SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            //foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            //{
            //    txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            //}
            //if (HasDiagnosisID)
            //{
            //    PanelDiagnosis.Visible = true;
            //}
            //else
            //{
            //    PanelDiagnosis.Visible = false;
            //}
            //#region INITIAL DATA
            //HistoryExam_Delivery.DataSource = RDB.Delivery_GetList();
            //HistoryExam_Delivery.DataTextField = "Delivery";
            //HistoryExam_Delivery.DataValueField = "DeliveryID";
            //HistoryExam_Delivery.DataBind();

            MilestonesType.DataSource = RDB.Milestones_GetList();
            MilestonesType.DataTextField = "Milestones";
            MilestonesType.DataValueField = "MilestonesID";
            MilestonesType.DataBind();

            //HistoryExam_DiagnosedBy.DataSource = RDB.Diagnosed_GetList();
            //HistoryExam_DiagnosedBy.DataTextField = "Diagnosed";
            //HistoryExam_DiagnosedBy.DataValueField = "DiagnosedID";
            //HistoryExam_DiagnosedBy.DataBind();

            //TypeOfCP_CPID.DataSource = RDB.TypeOfCP_GetList();
            //TypeOfCP_CPID.DataTextField = "TypeOfCP";
            //TypeOfCP_CPID.DataValueField = "TypeOfCPID";
            //TypeOfCP_CPID.DataBind();

            AssistiveDevices.DataSource = RDB.AssistiveDevices_GetList();
            AssistiveDevices.DataTextField = "AssistiveDevices";
            AssistiveDevices.DataValueField = "AssistiveDevicesID";
            AssistiveDevices.DataBind();

            //AssistiveDevices_Orthotics.DataSource = RDB.Orthotics_GetList();
            //AssistiveDevices_Orthotics.DataTextField = "Orthotics";
            //AssistiveDevices_Orthotics.DataValueField = "OrthoticsID";
            //AssistiveDevices_Orthotics.DataBind();

            OrthoticsDevices.DataSource = RDB.OrthoticsDevices_GetList();
            OrthoticsDevices.DataTextField = "OrthoticsDevices";
            OrthoticsDevices.DataValueField = "OrthoticsDevicesID";
            OrthoticsDevices.DataBind();

            //ADL_adlID.DataSource = RDB.ADL_GetList();
            //ADL_adlID.DataTextField = "ADL";
            //ADL_adlID.DataValueField = "ADLID";
            //ADL_adlID.DataBind();

            ADLList.DataSource = RDB.ADLList_GetList();
            ADLList.DataTextField = "ADLList";
            ADLList.DataValueField = "ADLListID";
            ADLList.DataBind();

            IndicationForBotox.DataSource = RDB.IndicationForBotox_GetList();
            IndicationForBotox.DataTextField = "IndicationForBotox";
            IndicationForBotox.DataValueField = "IndicationForBotoxID";
            IndicationForBotox.DataBind();

            AncillaryTreatment.DataSource = RDB.AncillaryTreatment_GetList();
            AncillaryTreatment.DataTextField = "AncillaryTreatment";
            AncillaryTreatment.DataValueField = "AncillaryTreatmentID";
            AncillaryTreatment.DataBind();

            SideEffects.DataSource = RDB.SideEffects_GetList();
            SideEffects.DataTextField = "SideEffects";
            SideEffects.DataValueField = "SideEffectsID";
            SideEffects.DataBind();


            //SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            //General_Pediatrician.Items.Clear(); General_Pediatrician.Items.Add(new ListItem("Select Pediatrician", "-1"));
            //General_Therapist.Items.Clear(); General_Therapist.Items.Add(new ListItem("Select Therapist", "-1"));
            //Doctor_Director.Items.Clear(); Doctor_Director.Items.Add(new ListItem("Select Director", "-1"));
            //Doctor_Physiotheraist.Items.Clear(); Doctor_Physiotheraist.Items.Add(new ListItem("Select Therapist", "-1"));
            //Doctor_Occupational.Items.Clear(); Doctor_Occupational.Items.Add(new ListItem("Select Therapist", "-1"));

            //foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            //{
            //    General_Pediatrician.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            //    General_Therapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            //    Doctor_Director.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            //    Doctor_Physiotheraist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            //    Doctor_Occupational.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            //}
            //#endregion
            //#region BOTOX DATA 1
            //DataTable dt1 = RDB.Get1(_appointmentID);
            //if (dt1.Rows.Count > 0)
            //{
            //    if (HasDiagnosisID)
            //    {
            //        string[] DiagnosisIDs = dt1.Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //        for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
            //        {
            //            for (int j = 0; j < txtDiagnosis.Items.Count; j++)
            //            {
            //                if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
            //                {
            //                    txtDiagnosis.Items[j].Selected = true; break;
            //                }
            //            }
            //        }
            //        txtDiagnosisOther.Text = dt1.Rows[0]["DiagnosisOther"].ToString();
            //    }
            //    General_BotoxNo.Text = dt1.Rows[0]["General_BotoxNo"].ToString();
            //    int _General_Pediatrician = 0; int.TryParse(dt1.Rows[0]["General_Pediatrician"].ToString(), out _General_Pediatrician);
            //    if (General_Pediatrician.Items.FindByValue(_General_Pediatrician.ToString()) != null)
            //    {
            //        General_Pediatrician.SelectedValue = _General_Pediatrician.ToString();
            //    }
            //    int _General_Therapist = 0; int.TryParse(dt1.Rows[0]["General_Therapist"].ToString(), out _General_Therapist);
            //    if (General_Therapist.Items.FindByValue(_General_Therapist.ToString()) != null)
            //    {
            //        General_Therapist.SelectedValue = _General_Therapist.ToString();
            //    }
            //    int _HistoryExam_Delivery = 0; int.TryParse(dt1.Rows[0]["HistoryExam_Delivery"].ToString(), out _HistoryExam_Delivery);
            //    if (HistoryExam_Delivery.Items.FindByValue(_HistoryExam_Delivery.ToString()) != null)
            //    {
            //        HistoryExam_Delivery.SelectedValue = _HistoryExam_Delivery.ToString();
            //    }
            //    HistoryExam_PerinatalComplications.Text = dt1.Rows[0]["HistoryExam_PerinatalComplications"].ToString();
            //    HistoryExam_BirthWeight.Text = dt1.Rows[0]["HistoryExam_BirthWeight"].ToString();
            //    int _HistoryExam_DiagnosedBy = 0; int.TryParse(dt1.Rows[0]["HistoryExam_DiagnosedBy"].ToString(), out _HistoryExam_DiagnosedBy);
            //    if (HistoryExam_DiagnosedBy.Items.FindByValue(_HistoryExam_DiagnosedBy.ToString()) != null)
            //    {
            //        HistoryExam_DiagnosedBy.SelectedValue = _HistoryExam_DiagnosedBy.ToString();
            //    }
            //    int _TypeOfCP_CPID = 0; int.TryParse(dt1.Rows[0]["TypeOfCP_CPID"].ToString(), out _TypeOfCP_CPID);
            //    if (TypeOfCP_CPID.Items.FindByValue(_TypeOfCP_CPID.ToString()) != null)
            //    {
            //        TypeOfCP_CPID.SelectedValue = _TypeOfCP_CPID.ToString();
            //    }
            //    int _AssistiveDevices_Orthotics = 0; int.TryParse(dt1.Rows[0]["AssistiveDevices_Orthotics"].ToString(), out _AssistiveDevices_Orthotics);
            //    if (AssistiveDevices_Orthotics.Items.FindByValue(_AssistiveDevices_Orthotics.ToString()) != null)
            //    {
            //        AssistiveDevices_Orthotics.SelectedValue = _AssistiveDevices_Orthotics.ToString();
            //    }
            //    int _ADL_adlID = 0; int.TryParse(dt1.Rows[0]["ADL_adlID"].ToString(), out _ADL_adlID);
            //    if (ADL_adlID.Items.FindByValue(_ADL_adlID.ToString()) != null)
            //    {
            //        ADL_adlID.SelectedValue = _ADL_adlID.ToString();
            //    }
            //    DateTime _Ambulation_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date1);
            //    if (_Ambulation_Date1 > DateTime.MinValue)
            //        Ambulation_Date1.Text = _Ambulation_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Ambulation_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date2);
            //    if (_Ambulation_Date2 > DateTime.MinValue)
            //        Ambulation_Date2.Text = _Ambulation_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Ambulation_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date3);
            //    if (_Ambulation_Date3 > DateTime.MinValue)
            //        Ambulation_Date3.Text = _Ambulation_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Ambulation_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date4);
            //    if (_Ambulation_Date4 > DateTime.MinValue)
            //        Ambulation_Date4.Text = _Ambulation_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Ambulation_Date5 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date5"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date5);
            //    if (_Ambulation_Date5 > DateTime.MinValue)
            //        Ambulation_Date5.Text = _Ambulation_Date5.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Ambulation_Date6 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date6"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date6);
            //    if (_Ambulation_Date6 > DateTime.MinValue)
            //        Ambulation_Date6.Text = _Ambulation_Date6.ToString(DbHelper.Configuration.showDateFormat);
            //    Ambulation_Amb1.Text = dt1.Rows[0]["Ambulation_Amb1"].ToString();
            //    Ambulation_Amb2.Text = dt1.Rows[0]["Ambulation_Amb2"].ToString();
            //    Ambulation_Amb3.Text = dt1.Rows[0]["Ambulation_Amb3"].ToString();
            //    Ambulation_Amb4.Text = dt1.Rows[0]["Ambulation_Amb4"].ToString();
            //    Ambulation_Amb5.Text = dt1.Rows[0]["Ambulation_Amb5"].ToString();
            //    Ambulation_Amb6.Text = dt1.Rows[0]["Ambulation_Amb6"].ToString();
            //    DateTime _PreExisting_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date1);
            //    if (_PreExisting_Date1 > DateTime.MinValue)
            //        PreExisting_Date1.Text = _PreExisting_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PreExisting_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date2);
            //    if (_PreExisting_Date2 > DateTime.MinValue)
            //        PreExisting_Date2.Text = _PreExisting_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PreExisting_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date3);
            //    if (_PreExisting_Date3 > DateTime.MinValue)
            //        PreExisting_Date3.Text = _PreExisting_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PreExisting_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date4);
            //    if (_PreExisting_Date4 > DateTime.MinValue)
            //        PreExisting_Date4.Text = _PreExisting_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    PreExisting_HipFID_1R.Text = dt1.Rows[0]["PreExisting_HipFID_1R"].ToString();
            //    PreExisting_HipFID_1L.Text = dt1.Rows[0]["PreExisting_HipFID_1L"].ToString();
            //    PreExisting_HipFID_2R.Text = dt1.Rows[0]["PreExisting_HipFID_2R"].ToString();
            //    PreExisting_HipFID_2L.Text = dt1.Rows[0]["PreExisting_HipFID_2L"].ToString();
            //    PreExisting_HipFID_3R.Text = dt1.Rows[0]["PreExisting_HipFID_3R"].ToString();
            //    PreExisting_HipFID_3L.Text = dt1.Rows[0]["PreExisting_HipFID_3L"].ToString();
            //    PreExisting_HipFID_4R.Text = dt1.Rows[0]["PreExisting_HipFID_4R"].ToString();
            //    PreExisting_HipFID_4R.Text = dt1.Rows[0]["PreExisting_HipFID_4R"].ToString();
            //    PreExisting_HipAdduction_1R.Text = dt1.Rows[0]["PreExisting_HipAdduction_1R"].ToString();
            //    PreExisting_HipAdduction_1L.Text = dt1.Rows[0]["PreExisting_HipAdduction_1L"].ToString();
            //    PreExisting_HipAdduction_2R.Text = dt1.Rows[0]["PreExisting_HipAdduction_2R"].ToString();
            //    PreExisting_HipAdduction_2L.Text = dt1.Rows[0]["PreExisting_HipAdduction_2L"].ToString();
            //    PreExisting_HipAdduction_3R.Text = dt1.Rows[0]["PreExisting_HipAdduction_3R"].ToString();
            //    PreExisting_HipAdduction_3L.Text = dt1.Rows[0]["PreExisting_HipAdduction_3L"].ToString();
            //    PreExisting_HipAdduction_4R.Text = dt1.Rows[0]["PreExisting_HipAdduction_4R"].ToString();
            //    PreExisting_HipAdduction_4L.Text = dt1.Rows[0]["PreExisting_HipAdduction_4L"].ToString();
            //    PreExisting_KneeFFD_1R.Text = dt1.Rows[0]["PreExisting_KneeFFD_1R"].ToString();
            //    PreExisting_KneeFFD_1L.Text = dt1.Rows[0]["PreExisting_KneeFFD_1L"].ToString();
            //    PreExisting_KneeFFD_2R.Text = dt1.Rows[0]["PreExisting_KneeFFD_2R"].ToString();
            //    PreExisting_KneeFFD_2L.Text = dt1.Rows[0]["PreExisting_KneeFFD_2L"].ToString();
            //    PreExisting_KneeFFD_3R.Text = dt1.Rows[0]["PreExisting_KneeFFD_3R"].ToString();
            //    PreExisting_KneeFFD_3L.Text = dt1.Rows[0]["PreExisting_KneeFFD_3L"].ToString();
            //    PreExisting_KneeFFD_4R.Text = dt1.Rows[0]["PreExisting_KneeFFD_4R"].ToString();
            //    PreExisting_KneeFFD_4L.Text = dt1.Rows[0]["PreExisting_KneeFFD_4L"].ToString();
            //    PreExisting_Equinus_1R.Text = dt1.Rows[0]["PreExisting_Equinus_1R"].ToString();
            //    PreExisting_Equinus_1L.Text = dt1.Rows[0]["PreExisting_Equinus_1L"].ToString();
            //    PreExisting_Equinus_2R.Text = dt1.Rows[0]["PreExisting_Equinus_2R"].ToString();
            //    PreExisting_Equinus_2L.Text = dt1.Rows[0]["PreExisting_Equinus_2L"].ToString();
            //    PreExisting_Equinus_3R.Text = dt1.Rows[0]["PreExisting_Equinus_3R"].ToString();
            //    PreExisting_Equinus_3L.Text = dt1.Rows[0]["PreExisting_Equinus_3L"].ToString();
            //    PreExisting_Equinus_4R.Text = dt1.Rows[0]["PreExisting_Equinus_4R"].ToString();
            //    PreExisting_Equinus_4L.Text = dt1.Rows[0]["PreExisting_Equinus_4L"].ToString();
            //    PreExisting_Planovalgoid_1R.Text = dt1.Rows[0]["PreExisting_Planovalgoid_1R"].ToString();
            //    PreExisting_Planovalgoid_1L.Text = dt1.Rows[0]["PreExisting_Planovalgoid_1L"].ToString();
            //    PreExisting_Planovalgoid_2R.Text = dt1.Rows[0]["PreExisting_Planovalgoid_2R"].ToString();
            //    PreExisting_Planovalgoid_2L.Text = dt1.Rows[0]["PreExisting_Planovalgoid_2L"].ToString();
            //    PreExisting_Planovalgoid_3R.Text = dt1.Rows[0]["PreExisting_Planovalgoid_3R"].ToString();
            //    PreExisting_Planovalgoid_3L.Text = dt1.Rows[0]["PreExisting_Planovalgoid_3L"].ToString();
            //    PreExisting_Planovalgoid_4R.Text = dt1.Rows[0]["PreExisting_Planovalgoid_4R"].ToString();
            //    PreExisting_Planovalgoid_4L.Text = dt1.Rows[0]["PreExisting_Planovalgoid_4L"].ToString();
            //    PreExisting_Cavovarus_1R.Text = dt1.Rows[0]["PreExisting_Cavovarus_1R"].ToString();
            //    PreExisting_Cavovarus_1L.Text = dt1.Rows[0]["PreExisting_Cavovarus_1L"].ToString();
            //    PreExisting_Cavovarus_2R.Text = dt1.Rows[0]["PreExisting_Cavovarus_2R"].ToString();
            //    PreExisting_Cavovarus_2L.Text = dt1.Rows[0]["PreExisting_Cavovarus_2L"].ToString();
            //    PreExisting_Cavovarus_3R.Text = dt1.Rows[0]["PreExisting_Cavovarus_3R"].ToString();
            //    PreExisting_Cavovarus_3L.Text = dt1.Rows[0]["PreExisting_Cavovarus_3L"].ToString();
            //    PreExisting_Cavovarus_4R.Text = dt1.Rows[0]["PreExisting_Cavovarus_4R"].ToString();
            //    PreExisting_Cavovarus_4L.Text = dt1.Rows[0]["PreExisting_Cavovarus_4L"].ToString();
            //    PreExisting_ElbowFFD_1R.Text = dt1.Rows[0]["PreExisting_ElbowFFD_1R"].ToString();
            //    PreExisting_ElbowFFD_1L.Text = dt1.Rows[0]["PreExisting_ElbowFFD_1L"].ToString();
            //    PreExisting_ElbowFFD_2R.Text = dt1.Rows[0]["PreExisting_ElbowFFD_2R"].ToString();
            //    PreExisting_ElbowFFD_2L.Text = dt1.Rows[0]["PreExisting_ElbowFFD_2L"].ToString();
            //    PreExisting_ElbowFFD_3R.Text = dt1.Rows[0]["PreExisting_ElbowFFD_3R"].ToString();
            //    PreExisting_ElbowFFD_3L.Text = dt1.Rows[0]["PreExisting_ElbowFFD_3L"].ToString();
            //    PreExisting_ElbowFFD_4R.Text = dt1.Rows[0]["PreExisting_ElbowFFD_4R"].ToString();
            //    PreExisting_ElbowFFD_4L.Text = dt1.Rows[0]["PreExisting_ElbowFFD_4L"].ToString();
            //    PreExisting_WristFlexPron_1R.Text = dt1.Rows[0]["PreExisting_WristFlexPron_1R"].ToString();
            //    PreExisting_WristFlexPron_1L.Text = dt1.Rows[0]["PreExisting_WristFlexPron_1L"].ToString();
            //    PreExisting_WristFlexPron_2R.Text = dt1.Rows[0]["PreExisting_WristFlexPron_2R"].ToString();
            //    PreExisting_WristFlexPron_2L.Text = dt1.Rows[0]["PreExisting_WristFlexPron_2L"].ToString();
            //    PreExisting_WristFlexPron_3R.Text = dt1.Rows[0]["PreExisting_WristFlexPron_3R"].ToString();
            //    PreExisting_WristFlexPron_3L.Text = dt1.Rows[0]["PreExisting_WristFlexPron_3L"].ToString();
            //    PreExisting_WristFlexPron_4R.Text = dt1.Rows[0]["PreExisting_WristFlexPron_4R"].ToString();
            //    PreExisting_WristFlexPron_4L.Text = dt1.Rows[0]["PreExisting_WristFlexPron_4L"].ToString();
            //    DateTime _PassiveROM_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date1);
            //    if (_PassiveROM_Date1 > DateTime.MinValue)
            //        PassiveROM_Date1.Text = _PassiveROM_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PassiveROM_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date2);
            //    if (_PassiveROM_Date2 > DateTime.MinValue)
            //        PassiveROM_Date2.Text = _PassiveROM_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PassiveROM_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date3);
            //    if (_PassiveROM_Date3 > DateTime.MinValue)
            //        PassiveROM_Date3.Text = _PassiveROM_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _PassiveROM_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date4);
            //    if (_PassiveROM_Date4 > DateTime.MinValue)
            //        PassiveROM_Date4.Text = _PassiveROM_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    PassiveROM_HipFlexion_1R.Text = dt1.Rows[0]["PassiveROM_HipFlexion_1R"].ToString();
            //    PassiveROM_HipFlexion_1L.Text = dt1.Rows[0]["PassiveROM_HipFlexion_1L"].ToString();
            //    PassiveROM_HipFlexion_2R.Text = dt1.Rows[0]["PassiveROM_HipFlexion_2R"].ToString();
            //    PassiveROM_HipFlexion_2L.Text = dt1.Rows[0]["PassiveROM_HipFlexion_2L"].ToString();
            //    PassiveROM_HipFlexion_3R.Text = dt1.Rows[0]["PassiveROM_HipFlexion_3R"].ToString();
            //    PassiveROM_HipFlexion_3L.Text = dt1.Rows[0]["PassiveROM_HipFlexion_3L"].ToString();
            //    PassiveROM_HipFlexion_4R.Text = dt1.Rows[0]["PassiveROM_HipFlexion_4R"].ToString();
            //    PassiveROM_HipFlexion_4L.Text = dt1.Rows[0]["PassiveROM_HipFlexion_4L"].ToString();
            //    PassiveROM_HipAbduction_1R.Text = dt1.Rows[0]["PassiveROM_HipAbduction_1R"].ToString();
            //    PassiveROM_HipAbduction_1L.Text = dt1.Rows[0]["PassiveROM_HipAbduction_1L"].ToString();
            //    PassiveROM_HipAbduction_2R.Text = dt1.Rows[0]["PassiveROM_HipAbduction_2R"].ToString();
            //    PassiveROM_HipAbduction_2L.Text = dt1.Rows[0]["PassiveROM_HipAbduction_2L"].ToString();
            //    PassiveROM_HipAbduction_3R.Text = dt1.Rows[0]["PassiveROM_HipAbduction_3R"].ToString();
            //    PassiveROM_HipAbduction_3L.Text = dt1.Rows[0]["PassiveROM_HipAbduction_3L"].ToString();
            //    PassiveROM_HipAbduction_4R.Text = dt1.Rows[0]["PassiveROM_HipAbduction_4R"].ToString();
            //    PassiveROM_HipAbduction_4L.Text = dt1.Rows[0]["PassiveROM_HipAbduction_4L"].ToString();
            //    PassiveROM_HipIR_1R.Text = dt1.Rows[0]["PassiveROM_HipIR_1R"].ToString();
            //    PassiveROM_HipIR_1L.Text = dt1.Rows[0]["PassiveROM_HipIR_1L"].ToString();
            //    PassiveROM_HipIR_2R.Text = dt1.Rows[0]["PassiveROM_HipIR_2R"].ToString();
            //    PassiveROM_HipIR_2L.Text = dt1.Rows[0]["PassiveROM_HipIR_2L"].ToString();
            //    PassiveROM_HipIR_3R.Text = dt1.Rows[0]["PassiveROM_HipIR_3R"].ToString();
            //    PassiveROM_HipIR_3L.Text = dt1.Rows[0]["PassiveROM_HipIR_3L"].ToString();
            //    PassiveROM_HipIR_4R.Text = dt1.Rows[0]["PassiveROM_HipIR_4R"].ToString();
            //    PassiveROM_HipIR_4L.Text = dt1.Rows[0]["PassiveROM_HipIR_4L"].ToString();
            //    PassiveROM_HipER_1R.Text = dt1.Rows[0]["PassiveROM_HipER_1R"].ToString();
            //    PassiveROM_HipER_1L.Text = dt1.Rows[0]["PassiveROM_HipER_1L"].ToString();
            //    PassiveROM_HipER_2R.Text = dt1.Rows[0]["PassiveROM_HipER_2R"].ToString();
            //    PassiveROM_HipER_2L.Text = dt1.Rows[0]["PassiveROM_HipER_2L"].ToString();
            //    PassiveROM_HipER_3R.Text = dt1.Rows[0]["PassiveROM_HipER_3R"].ToString();
            //    PassiveROM_HipER_3L.Text = dt1.Rows[0]["PassiveROM_HipER_3L"].ToString();
            //    PassiveROM_HipER_4R.Text = dt1.Rows[0]["PassiveROM_HipER_4R"].ToString();
            //    PassiveROM_HipER_4L.Text = dt1.Rows[0]["PassiveROM_HipER_4L"].ToString();
            //    PassiveROM_KneeFlexion_1R.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_1R"].ToString();
            //    PassiveROM_KneeFlexion_1L.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_1L"].ToString();
            //    PassiveROM_KneeFlexion_2R.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_2R"].ToString();
            //    PassiveROM_KneeFlexion_2L.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_2L"].ToString();
            //    PassiveROM_KneeFlexion_3R.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_3R"].ToString();
            //    PassiveROM_KneeFlexion_3L.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_3L"].ToString();
            //    PassiveROM_KneeFlexion_4R.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_4R"].ToString();
            //    PassiveROM_KneeFlexion_4L.Text = dt1.Rows[0]["PassiveROM_KneeFlexion_4L"].ToString();
            //    PassiveROM_PoplitealAngle_1R.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_1R"].ToString();
            //    PassiveROM_PoplitealAngle_1L.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_1L"].ToString();
            //    PassiveROM_PoplitealAngle_2R.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_2R"].ToString();
            //    PassiveROM_PoplitealAngle_2L.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_2L"].ToString();
            //    PassiveROM_PoplitealAngle_3R.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_3R"].ToString();
            //    PassiveROM_PoplitealAngle_3L.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_3L"].ToString();
            //    PassiveROM_PoplitealAngle_4R.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_4R"].ToString();
            //    PassiveROM_PoplitealAngle_4L.Text = dt1.Rows[0]["PassiveROM_PoplitealAngle_4L"].ToString();
            //    PassiveROM_KneeExt_1R.Text = dt1.Rows[0]["PassiveROM_KneeExt_1R"].ToString();
            //    PassiveROM_KneeExt_1L.Text = dt1.Rows[0]["PassiveROM_KneeExt_1L"].ToString();
            //    PassiveROM_KneeExt_2R.Text = dt1.Rows[0]["PassiveROM_KneeExt_2R"].ToString();
            //    PassiveROM_KneeExt_2L.Text = dt1.Rows[0]["PassiveROM_KneeExt_2L"].ToString();
            //    PassiveROM_KneeExt_3R.Text = dt1.Rows[0]["PassiveROM_KneeExt_3R"].ToString();
            //    PassiveROM_KneeExt_3L.Text = dt1.Rows[0]["PassiveROM_KneeExt_3L"].ToString();
            //    PassiveROM_KneeExt_4R.Text = dt1.Rows[0]["PassiveROM_KneeExt_4R"].ToString();
            //    PassiveROM_KneeExt_4L.Text = dt1.Rows[0]["PassiveROM_KneeExt_4L"].ToString();
            //    PassiveROM_KneeFlex_1R.Text = dt1.Rows[0]["PassiveROM_KneeFlex_1R"].ToString();
            //    PassiveROM_KneeFlex_1L.Text = dt1.Rows[0]["PassiveROM_KneeFlex_1L"].ToString();
            //    PassiveROM_KneeFlex_2R.Text = dt1.Rows[0]["PassiveROM_KneeFlex_2R"].ToString();
            //    PassiveROM_KneeFlex_2L.Text = dt1.Rows[0]["PassiveROM_KneeFlex_2L"].ToString();
            //    PassiveROM_KneeFlex_3R.Text = dt1.Rows[0]["PassiveROM_KneeFlex_3R"].ToString();
            //    PassiveROM_KneeFlex_3L.Text = dt1.Rows[0]["PassiveROM_KneeFlex_3L"].ToString();
            //    PassiveROM_KneeFlex_4R.Text = dt1.Rows[0]["PassiveROM_KneeFlex_4R"].ToString();
            //    PassiveROM_KneeFlex_4L.Text = dt1.Rows[0]["PassiveROM_KneeFlex_4L"].ToString();
            //    PassiveROM_Plantarflexion_1R.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_1R"].ToString();
            //    PassiveROM_Plantarflexion_1L.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_1L"].ToString();
            //    PassiveROM_Plantarflexion_2R.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_2R"].ToString();
            //    PassiveROM_Plantarflexion_2L.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_2L"].ToString();
            //    PassiveROM_Plantarflexion_3R.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_3R"].ToString();
            //    PassiveROM_Plantarflexion_3L.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_3L"].ToString();
            //    PassiveROM_Plantarflexion_4R.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_4R"].ToString();
            //    PassiveROM_Plantarflexion_4L.Text = dt1.Rows[0]["PassiveROM_Plantarflexion_4L"].ToString();
            //    PassiveROM_AnkleInv_1R.Text = dt1.Rows[0]["PassiveROM_AnkleInv_1R"].ToString();
            //    PassiveROM_AnkleInv_1L.Text = dt1.Rows[0]["PassiveROM_AnkleInv_1L"].ToString();
            //    PassiveROM_AnkleInv_2R.Text = dt1.Rows[0]["PassiveROM_AnkleInv_2R"].ToString();
            //    PassiveROM_AnkleInv_2L.Text = dt1.Rows[0]["PassiveROM_AnkleInv_2L"].ToString();
            //    PassiveROM_AnkleInv_3R.Text = dt1.Rows[0]["PassiveROM_AnkleInv_3R"].ToString();
            //    PassiveROM_AnkleInv_3L.Text = dt1.Rows[0]["PassiveROM_AnkleInv_3L"].ToString();
            //    PassiveROM_AnkleInv_4R.Text = dt1.Rows[0]["PassiveROM_AnkleInv_4R"].ToString();
            //    PassiveROM_AnkleInv_4L.Text = dt1.Rows[0]["PassiveROM_AnkleInv_4L"].ToString();
            //    PassiveROM_AnkleEver_1R.Text = dt1.Rows[0]["PassiveROM_AnkleEver_1R"].ToString();
            //    PassiveROM_AnkleEver_1L.Text = dt1.Rows[0]["PassiveROM_AnkleEver_1L"].ToString();
            //    PassiveROM_AnkleEver_2R.Text = dt1.Rows[0]["PassiveROM_AnkleEver_2R"].ToString();
            //    PassiveROM_AnkleEver_2L.Text = dt1.Rows[0]["PassiveROM_AnkleEver_2L"].ToString();
            //    PassiveROM_AnkleEver_3R.Text = dt1.Rows[0]["PassiveROM_AnkleEver_3R"].ToString();
            //    PassiveROM_AnkleEver_3L.Text = dt1.Rows[0]["PassiveROM_AnkleEver_3L"].ToString();
            //    PassiveROM_AnkleEver_4R.Text = dt1.Rows[0]["PassiveROM_AnkleEver_4R"].ToString();
            //    PassiveROM_AnkleEver_4L.Text = dt1.Rows[0]["PassiveROM_AnkleEver_4L"].ToString();
            //    DateTime _Tone_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date1);
            //    if (_Tone_Date1 > DateTime.MinValue)
            //        Tone_Date1.Text = _Tone_Date1.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _Tone_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date2);
            //    if (_Tone_Date2 > DateTime.MinValue)
            //        Tone_Date2.Text = _Tone_Date2.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _Tone_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date3);
            //    if (_Tone_Date3 > DateTime.MinValue)
            //        Tone_Date3.Text = _Tone_Date3.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _Tone_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date4);
            //    if (_Tone_Date4 > DateTime.MinValue)
            //        Tone_Date4.Text = _Tone_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    Tone_Iliopsoas_1R.Text = dt1.Rows[0]["Tone_Iliopsoas_1R"].ToString();
            //    Tone_Iliopsoas_1L.Text = dt1.Rows[0]["Tone_Iliopsoas_1L"].ToString();
            //    Tone_Iliopsoas_2R.Text = dt1.Rows[0]["Tone_Iliopsoas_2R"].ToString();
            //    Tone_Iliopsoas_2L.Text = dt1.Rows[0]["Tone_Iliopsoas_3R"].ToString();
            //    Tone_Iliopsoas_3R.Text = dt1.Rows[0]["Tone_Iliopsoas_3R"].ToString();
            //    Tone_Iliopsoas_3L.Text = dt1.Rows[0]["Tone_Iliopsoas_3L"].ToString();
            //    Tone_Iliopsoas_4R.Text = dt1.Rows[0]["Tone_Iliopsoas_4R"].ToString();
            //    Tone_Iliopsoas_4L.Text = dt1.Rows[0]["Tone_Iliopsoas_4L"].ToString();
            //    Tone_Adductors_1R.Text = dt1.Rows[0]["Tone_Adductors_1R"].ToString();
            //    Tone_Adductors_1L.Text = dt1.Rows[0]["Tone_Adductors_1L"].ToString();
            //    Tone_Adductors_2R.Text = dt1.Rows[0]["Tone_Adductors_2R"].ToString();
            //    Tone_Adductors_2L.Text = dt1.Rows[0]["Tone_Adductors_2L"].ToString();
            //    Tone_Adductors_3R.Text = dt1.Rows[0]["Tone_Adductors_3R"].ToString();
            //    Tone_Adductors_3L.Text = dt1.Rows[0]["Tone_Adductors_3L"].ToString();
            //    Tone_Adductors_4R.Text = dt1.Rows[0]["Tone_Adductors_4R"].ToString();
            //    Tone_Adductors_4L.Text = dt1.Rows[0]["Tone_Adductors_4L"].ToString();
            //    Tone_RectusFemoris_1R.Text = dt1.Rows[0]["Tone_RectusFemoris_1R"].ToString();
            //    Tone_RectusFemoris_1L.Text = dt1.Rows[0]["Tone_RectusFemoris_1L"].ToString();
            //    Tone_RectusFemoris_2R.Text = dt1.Rows[0]["Tone_RectusFemoris_2R"].ToString();
            //    Tone_RectusFemoris_2L.Text = dt1.Rows[0]["Tone_RectusFemoris_2L"].ToString();
            //    Tone_RectusFemoris_3R.Text = dt1.Rows[0]["Tone_RectusFemoris_3R"].ToString();
            //    Tone_RectusFemoris_3L.Text = dt1.Rows[0]["Tone_RectusFemoris_3L"].ToString();
            //    Tone_RectusFemoris_4R.Text = dt1.Rows[0]["Tone_RectusFemoris_4R"].ToString();
            //    Tone_RectusFemoris_4L.Text = dt1.Rows[0]["Tone_RectusFemoris_4L"].ToString();
            //    Tone_Hamstrings_1R.Text = dt1.Rows[0]["Tone_Hamstrings_1R"].ToString();
            //    Tone_Hamstrings_1L.Text = dt1.Rows[0]["Tone_Hamstrings_1L"].ToString();
            //    Tone_Hamstrings_2R.Text = dt1.Rows[0]["Tone_Hamstrings_2R"].ToString();
            //    Tone_Hamstrings_2L.Text = dt1.Rows[0]["Tone_Hamstrings_2L"].ToString();
            //    Tone_Hamstrings_3R.Text = dt1.Rows[0]["Tone_Hamstrings_3R"].ToString();
            //    Tone_Hamstrings_3L.Text = dt1.Rows[0]["Tone_Hamstrings_3L"].ToString();
            //    Tone_Hamstrings_4R.Text = dt1.Rows[0]["Tone_Hamstrings_4R"].ToString();
            //    Tone_Hamstrings_4L.Text = dt1.Rows[0]["Tone_Hamstrings_4L"].ToString();
            //    Tone_Gastrosoleus_1R.Text = dt1.Rows[0]["Tone_Gastrosoleus_1R"].ToString();
            //    Tone_Gastrosoleus_1L.Text = dt1.Rows[0]["Tone_Gastrosoleus_1L"].ToString();
            //    Tone_Gastrosoleus_2R.Text = dt1.Rows[0]["Tone_Gastrosoleus_2R"].ToString();
            //    Tone_Gastrosoleus_2L.Text = dt1.Rows[0]["Tone_Gastrosoleus_2L"].ToString();
            //    Tone_Gastrosoleus_3R.Text = dt1.Rows[0]["Tone_Gastrosoleus_3R"].ToString();
            //    Tone_Gastrosoleus_3L.Text = dt1.Rows[0]["Tone_Gastrosoleus_3L"].ToString();
            //    Tone_Gastrosoleus_4R.Text = dt1.Rows[0]["Tone_Gastrosoleus_4R"].ToString();
            //    Tone_Gastrosoleus_4L.Text = dt1.Rows[0]["Tone_Gastrosoleus_4L"].ToString();
            //    Tone_ElbowFlexors_1R.Text = dt1.Rows[0]["Tone_ElbowFlexors_1R"].ToString();
            //    Tone_ElbowFlexors_1L.Text = dt1.Rows[0]["Tone_ElbowFlexors_1L"].ToString();
            //    Tone_ElbowFlexors_2R.Text = dt1.Rows[0]["Tone_ElbowFlexors_2R"].ToString();
            //    Tone_ElbowFlexors_2L.Text = dt1.Rows[0]["Tone_ElbowFlexors_2L"].ToString();
            //    Tone_ElbowFlexors_3R.Text = dt1.Rows[0]["Tone_ElbowFlexors_3R"].ToString();
            //    Tone_ElbowFlexors_3L.Text = dt1.Rows[0]["Tone_ElbowFlexors_3L"].ToString();
            //    Tone_ElbowFlexors_4R.Text = dt1.Rows[0]["Tone_ElbowFlexors_4R"].ToString();
            //    Tone_ElbowFlexors_4L.Text = dt1.Rows[0]["Tone_ElbowFlexors_4L"].ToString();
            //    Tone_WristFlexors_1R.Text = dt1.Rows[0]["Tone_WristFlexors_1R"].ToString();
            //    Tone_WristFlexors_1L.Text = dt1.Rows[0]["Tone_WristFlexors_1L"].ToString();
            //    Tone_WristFlexors_2R.Text = dt1.Rows[0]["Tone_WristFlexors_2R"].ToString();
            //    Tone_WristFlexors_2L.Text = dt1.Rows[0]["Tone_WristFlexors_2L"].ToString();
            //    Tone_WristFlexors_3R.Text = dt1.Rows[0]["Tone_WristFlexors_3R"].ToString();
            //    Tone_WristFlexors_3L.Text = dt1.Rows[0]["Tone_WristFlexors_3L"].ToString();
            //    Tone_WristFlexors_4R.Text = dt1.Rows[0]["Tone_WristFlexors_4R"].ToString();
            //    Tone_WristFlexors_4L.Text = dt1.Rows[0]["Tone_WristFlexors_4L"].ToString();
            //    Tone_FingerFlexors_1R.Text = dt1.Rows[0]["Tone_FingerFlexors_1R"].ToString();
            //    Tone_FingerFlexors_1L.Text = dt1.Rows[0]["Tone_FingerFlexors_1L"].ToString();
            //    Tone_FingerFlexors_2R.Text = dt1.Rows[0]["Tone_FingerFlexors_2R"].ToString();
            //    Tone_FingerFlexors_2L.Text = dt1.Rows[0]["Tone_FingerFlexors_2L"].ToString();
            //    Tone_FingerFlexors_3R.Text = dt1.Rows[0]["Tone_FingerFlexors_3R"].ToString();
            //    Tone_FingerFlexors_3L.Text = dt1.Rows[0]["Tone_FingerFlexors_3L"].ToString();
            //    Tone_FingerFlexors_4R.Text = dt1.Rows[0]["Tone_FingerFlexors_4R"].ToString();
            //    Tone_FingerFlexors_4L.Text = dt1.Rows[0]["Tone_FingerFlexors_4L"].ToString();
            //    Tone_PronatorFlexors_1R.Text = dt1.Rows[0]["Tone_PronatorFlexors_1R"].ToString();
            //    Tone_PronatorFlexors_1L.Text = dt1.Rows[0]["Tone_PronatorFlexors_1L"].ToString();
            //    Tone_PronatorFlexors_2R.Text = dt1.Rows[0]["Tone_PronatorFlexors_2R"].ToString();
            //    Tone_PronatorFlexors_2L.Text = dt1.Rows[0]["Tone_PronatorFlexors_2L"].ToString();
            //    Tone_PronatorFlexors_3R.Text = dt1.Rows[0]["Tone_PronatorFlexors_3R"].ToString();
            //    Tone_PronatorFlexors_3L.Text = dt1.Rows[0]["Tone_PronatorFlexors_3L"].ToString();
            //    Tone_PronatorFlexors_4R.Text = dt1.Rows[0]["Tone_PronatorFlexors_4R"].ToString();
            //    Tone_PronatorFlexors_4L.Text = dt1.Rows[0]["Tone_PronatorFlexors_4L"].ToString();
            //    DateTime _TardieusScale_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date1);
            //    if (_TardieusScale_Date1 > DateTime.MinValue)
            //        TardieusScale_Date1.Text = _TardieusScale_Date1.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _TardieusScale_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date2);
            //    if (_TardieusScale_Date2 > DateTime.MinValue)
            //        TardieusScale_Date2.Text = _TardieusScale_Date2.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _TardieusScale_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date3);
            //    if (_TardieusScale_Date3 > DateTime.MinValue)
            //        TardieusScale_Date3.Text = _TardieusScale_Date3.ToString(DbHelper.Configuration.showDateFormat);

            //    DateTime _TardieusScale_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date4);
            //    if (_TardieusScale_Date4 > DateTime.MinValue)
            //        TardieusScale_Date4.Text = _TardieusScale_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    TardieusScale_GastrosoleusR1_1R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_1R"].ToString();
            //    TardieusScale_GastrosoleusR1_1L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_1L"].ToString();
            //    TardieusScale_GastrosoleusR1_2R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_2R"].ToString();
            //    TardieusScale_GastrosoleusR1_2L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_2L"].ToString();
            //    TardieusScale_GastrosoleusR1_3R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_3R"].ToString();
            //    TardieusScale_GastrosoleusR1_3L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_3L"].ToString();
            //    TardieusScale_GastrosoleusR1_4R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_4R"].ToString();
            //    TardieusScale_GastrosoleusR1_4L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR1_4L"].ToString();
            //    TardieusScale_GastrosoleusR2_1R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_1R"].ToString();
            //    TardieusScale_GastrosoleusR2_1L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_1L"].ToString();
            //    TardieusScale_GastrosoleusR2_2R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_2R"].ToString();
            //    TardieusScale_GastrosoleusR2_2L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_2L"].ToString();
            //    TardieusScale_GastrosoleusR2_3R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_3R"].ToString();
            //    TardieusScale_GastrosoleusR2_3L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_3L"].ToString();
            //    TardieusScale_GastrosoleusR2_4R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_4R"].ToString();
            //    TardieusScale_GastrosoleusR2_4L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR2_4L"].ToString();
            //    TardieusScale_GastrosoleusR3_1R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_1R"].ToString();
            //    TardieusScale_GastrosoleusR3_1L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_1L"].ToString();
            //    TardieusScale_GastrosoleusR3_2R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_2R"].ToString();
            //    TardieusScale_GastrosoleusR3_2L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_2L"].ToString();
            //    TardieusScale_GastrosoleusR3_3R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_3R"].ToString();
            //    TardieusScale_GastrosoleusR3_3L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_3L"].ToString();
            //    TardieusScale_GastrosoleusR3_4R.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_4R"].ToString();
            //    TardieusScale_GastrosoleusR3_4L.Text = dt1.Rows[0]["TardieusScale_GastrosoleusR3_4L"].ToString();
            //    TardieusScale_HamstringsR1_1R.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_1R"].ToString();
            //    TardieusScale_HamstringsR1_1L.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_1L"].ToString();
            //    TardieusScale_HamstringsR1_2R.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_2R"].ToString();
            //    TardieusScale_HamstringsR1_2L.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_2L"].ToString();
            //    TardieusScale_HamstringsR1_3R.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_3R"].ToString();
            //    TardieusScale_HamstringsR1_3L.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_3L"].ToString();
            //    TardieusScale_HamstringsR1_4R.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_4R"].ToString();
            //    TardieusScale_HamstringsR1_4L.Text = dt1.Rows[0]["TardieusScale_HamstringsR1_4L"].ToString();
            //    TardieusScale_HamstringsR2_1R.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_1R"].ToString();
            //    TardieusScale_HamstringsR2_1L.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_1L"].ToString();
            //    TardieusScale_HamstringsR2_2R.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_2R"].ToString();
            //    TardieusScale_HamstringsR2_2L.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_2L"].ToString();
            //    TardieusScale_HamstringsR2_3R.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_3R"].ToString();
            //    TardieusScale_HamstringsR2_3L.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_3L"].ToString();
            //    TardieusScale_HamstringsR2_4R.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_4R"].ToString();
            //    TardieusScale_HamstringsR2_4L.Text = dt1.Rows[0]["TardieusScale_HamstringsR2_4L"].ToString();
            //}
            //#endregion
            //#region BOTOX DATA 2
            //DataTable dt2 = RDB.Get2(_appointmentID);
            //if (dt2.Rows.Count > 0)
            //{
            //    DateTime _MuscleStrength_Date1 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date1);
            //    if (_MuscleStrength_Date1 > DateTime.MinValue)
            //        MuscleStrength_Date1.Text = _MuscleStrength_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _MuscleStrength_Date2 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date2);
            //    if (_MuscleStrength_Date2 > DateTime.MinValue)
            //        MuscleStrength_Date2.Text = _MuscleStrength_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _MuscleStrength_Date3 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date3);
            //    if (_MuscleStrength_Date3 > DateTime.MinValue)
            //        MuscleStrength_Date3.Text = _MuscleStrength_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _MuscleStrength_Date4 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date4);
            //    if (_MuscleStrength_Date4 > DateTime.MinValue)
            //        MuscleStrength_Date4.Text = _MuscleStrength_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    MuscleStrength_Iliopsoas_1R.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_1R"].ToString();
            //    MuscleStrength_Iliopsoas_1L.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_1L"].ToString();
            //    MuscleStrength_Iliopsoas_2R.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_2R"].ToString();
            //    MuscleStrength_Iliopsoas_2L.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_2L"].ToString();
            //    MuscleStrength_Iliopsoas_3R.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_3R"].ToString();
            //    MuscleStrength_Iliopsoas_3L.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_3L"].ToString();
            //    MuscleStrength_Iliopsoas_4R.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_4R"].ToString();
            //    MuscleStrength_Iliopsoas_4L.Text = dt2.Rows[0]["MuscleStrength_Iliopsoas_4L"].ToString();
            //    MuscleStrength_GluteusMax_1R.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_1R"].ToString();
            //    MuscleStrength_GluteusMax_1L.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_1L"].ToString();
            //    MuscleStrength_GluteusMax_2R.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_2R"].ToString();
            //    MuscleStrength_GluteusMax_2L.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_2L"].ToString();
            //    MuscleStrength_GluteusMax_3R.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_3R"].ToString();
            //    MuscleStrength_GluteusMax_3L.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_3L"].ToString();
            //    MuscleStrength_GluteusMax_4R.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_4R"].ToString();
            //    MuscleStrength_GluteusMax_4L.Text = dt2.Rows[0]["MuscleStrength_GluteusMax_4L"].ToString();
            //    MuscleStrength_Abductors_1R.Text = dt2.Rows[0]["MuscleStrength_Abductors_1R"].ToString();
            //    MuscleStrength_Abductors_1L.Text = dt2.Rows[0]["MuscleStrength_Abductors_1L"].ToString();
            //    MuscleStrength_Abductors_2R.Text = dt2.Rows[0]["MuscleStrength_Abductors_2R"].ToString();
            //    MuscleStrength_Abductors_2L.Text = dt2.Rows[0]["MuscleStrength_Abductors_2L"].ToString();
            //    MuscleStrength_Abductors_3R.Text = dt2.Rows[0]["MuscleStrength_Abductors_3R"].ToString();
            //    MuscleStrength_Abductors_3L.Text = dt2.Rows[0]["MuscleStrength_Abductors_3L"].ToString();
            //    MuscleStrength_Abductors_4R.Text = dt2.Rows[0]["MuscleStrength_Abductors_4R"].ToString();
            //    MuscleStrength_Abductors_4L.Text = dt2.Rows[0]["MuscleStrength_Abductors_4L"].ToString();
            //    MuscleStrength_RectusFemoris_1R.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_1R"].ToString();
            //    MuscleStrength_RectusFemoris_1L.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_1L"].ToString();
            //    MuscleStrength_RectusFemoris_2R.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_2R"].ToString();
            //    MuscleStrength_RectusFemoris_2L.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_2L"].ToString();
            //    MuscleStrength_RectusFemoris_3R.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_3R"].ToString();
            //    MuscleStrength_RectusFemoris_3L.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_3L"].ToString();
            //    MuscleStrength_RectusFemoris_4R.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_4R"].ToString();
            //    MuscleStrength_RectusFemoris_4L.Text = dt2.Rows[0]["MuscleStrength_RectusFemoris_4L"].ToString();
            //    MuscleStrength_Hamstrings_1R.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_1R"].ToString();
            //    MuscleStrength_Hamstrings_1L.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_1L"].ToString();
            //    MuscleStrength_Hamstrings_2R.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_2R"].ToString();
            //    MuscleStrength_Hamstrings_2L.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_2L"].ToString();
            //    MuscleStrength_Hamstrings_3R.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_3R"].ToString();
            //    MuscleStrength_Hamstrings_3L.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_3L"].ToString();
            //    MuscleStrength_Hamstrings_4R.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_4R"].ToString();
            //    MuscleStrength_Hamstrings_4L.Text = dt2.Rows[0]["MuscleStrength_Hamstrings_4L"].ToString();
            //    MuscleStrength_Gastrosoleus_1R.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_1R"].ToString();
            //    MuscleStrength_Gastrosoleus_1L.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_1L"].ToString();
            //    MuscleStrength_Gastrosoleus_2R.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_2R"].ToString();
            //    MuscleStrength_Gastrosoleus_2L.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_2L"].ToString();
            //    MuscleStrength_Gastrosoleus_3R.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_3R"].ToString();
            //    MuscleStrength_Gastrosoleus_3L.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_3L"].ToString();
            //    MuscleStrength_Gastrosoleus_4R.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_4R"].ToString();
            //    MuscleStrength_Gastrosoleus_4L.Text = dt2.Rows[0]["MuscleStrength_Gastrosoleus_4L"].ToString();
            //    MuscleStrength_TibialisAnt_1R.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_1R"].ToString();
            //    MuscleStrength_TibialisAnt_1L.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_1L"].ToString();
            //    MuscleStrength_TibialisAnt_2R.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_2R"].ToString();
            //    MuscleStrength_TibialisAnt_2L.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_2L"].ToString();
            //    MuscleStrength_TibialisAnt_3R.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_3R"].ToString();
            //    MuscleStrength_TibialisAnt_3L.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_3L"].ToString();
            //    MuscleStrength_TibialisAnt_4R.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_4R"].ToString();
            //    MuscleStrength_TibialisAnt_4L.Text = dt2.Rows[0]["MuscleStrength_TibialisAnt_4L"].ToString();
            //    MuscleStrength_ElbowFlexors_1R.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_1R"].ToString();
            //    MuscleStrength_ElbowFlexors_1L.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_1L"].ToString();
            //    MuscleStrength_ElbowFlexors_2R.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_2R"].ToString();
            //    MuscleStrength_ElbowFlexors_2L.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_2L"].ToString();
            //    MuscleStrength_ElbowFlexors_3R.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_3R"].ToString();
            //    MuscleStrength_ElbowFlexors_3L.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_3L"].ToString();
            //    MuscleStrength_ElbowFlexors_4R.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_4R"].ToString();
            //    MuscleStrength_ElbowFlexors_4L.Text = dt2.Rows[0]["MuscleStrength_ElbowFlexors_4L"].ToString();
            //    MuscleStrength_PronatorTeres_1R.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_1R"].ToString();
            //    MuscleStrength_PronatorTeres_1L.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_1L"].ToString();
            //    MuscleStrength_PronatorTeres_2R.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_2R"].ToString();
            //    MuscleStrength_PronatorTeres_2L.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_2L"].ToString();
            //    MuscleStrength_PronatorTeres_3R.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_3R"].ToString();
            //    MuscleStrength_PronatorTeres_3L.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_3L"].ToString();
            //    MuscleStrength_PronatorTeres_4R.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_4R"].ToString();
            //    MuscleStrength_PronatorTeres_4L.Text = dt2.Rows[0]["MuscleStrength_PronatorTeres_4L"].ToString();
            //    MuscleStrength_WristFlexors_1R.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_1R"].ToString();
            //    MuscleStrength_WristFlexors_1L.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_1L"].ToString();
            //    MuscleStrength_WristFlexors_2R.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_2R"].ToString();
            //    MuscleStrength_WristFlexors_2L.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_2L"].ToString();
            //    MuscleStrength_WristFlexors_3R.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_3R"].ToString();
            //    MuscleStrength_WristFlexors_3L.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_3L"].ToString();
            //    MuscleStrength_WristFlexors_4R.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_4R"].ToString();
            //    MuscleStrength_WristFlexors_4L.Text = dt2.Rows[0]["MuscleStrength_WristFlexors_4L"].ToString(); 
            //    MuscleStrength_WristExtensors_1R.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_1R"].ToString();
            //    MuscleStrength_WristExtensors_1L.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_1L"].ToString();
            //    MuscleStrength_WristExtensors_2R.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_2R"].ToString();
            //    MuscleStrength_WristExtensors_2L.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_2L"].ToString();
            //    MuscleStrength_WristExtensors_3R.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_3R"].ToString();
            //    MuscleStrength_WristExtensors_3L.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_3L"].ToString();
            //    MuscleStrength_WristExtensors_4R.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_4R"].ToString();
            //    MuscleStrength_WristExtensors_4L.Text = dt2.Rows[0]["MuscleStrength_WristExtensors_4L"].ToString();
            //    MuscleStrength_FingerFlexors_1R.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_1R"].ToString();
            //    MuscleStrength_FingerFlexors_1L.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_1L"].ToString();
            //    MuscleStrength_FingerFlexors_2R.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_2R"].ToString();
            //    MuscleStrength_FingerFlexors_2L.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_2L"].ToString();
            //    MuscleStrength_FingerFlexors_3R.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_3R"].ToString();
            //    MuscleStrength_FingerFlexors_3L.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_3L"].ToString();
            //    MuscleStrength_FingerFlexors_4R.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_4R"].ToString();
            //    MuscleStrength_FingerFlexors_4L.Text = dt2.Rows[0]["MuscleStrength_FingerFlexors_4L"].ToString();
            //    DateTime _Voluntary_Date1 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date1);
            //    if (_Voluntary_Date1 > DateTime.MinValue)
            //        Voluntary_Date1.Text = _Voluntary_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Voluntary_Date2 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date2);
            //    if (_Voluntary_Date2 > DateTime.MinValue)
            //        Voluntary_Date2.Text = _Voluntary_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Voluntary_Date3 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date3);
            //    if (_Voluntary_Date3 > DateTime.MinValue)
            //        Voluntary_Date3.Text = _Voluntary_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _Voluntary_Date4 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date4);
            //    if (_Voluntary_Date4 > DateTime.MinValue)
            //        Voluntary_Date4.Text = _Voluntary_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    Voluntary_HipFlexion_1R.Text = dt2.Rows[0]["Voluntary_HipFlexion_1R"].ToString();
            //    Voluntary_HipFlexion_1L.Text = dt2.Rows[0]["Voluntary_HipFlexion_1L"].ToString();
            //    Voluntary_HipFlexion_2R.Text = dt2.Rows[0]["Voluntary_HipFlexion_2R"].ToString();
            //    Voluntary_HipFlexion_2L.Text = dt2.Rows[0]["Voluntary_HipFlexion_2L"].ToString();
            //    Voluntary_HipFlexion_3R.Text = dt2.Rows[0]["Voluntary_HipFlexion_3R"].ToString();
            //    Voluntary_HipFlexion_3L.Text = dt2.Rows[0]["Voluntary_HipFlexion_3L"].ToString();
            //    Voluntary_HipFlexion_4R.Text = dt2.Rows[0]["Voluntary_HipFlexion_4R"].ToString();
            //    Voluntary_HipFlexion_4L.Text = dt2.Rows[0]["Voluntary_HipFlexion_4L"].ToString();
            //    Voluntary_HipExtension_1R.Text = dt2.Rows[0]["Voluntary_HipExtension_1R"].ToString();
            //    Voluntary_HipExtension_1L.Text = dt2.Rows[0]["Voluntary_HipExtension_1L"].ToString();
            //    Voluntary_HipExtension_2R.Text = dt2.Rows[0]["Voluntary_HipExtension_2R"].ToString();
            //    Voluntary_HipExtension_2L.Text = dt2.Rows[0]["Voluntary_HipExtension_2L"].ToString();
            //    Voluntary_HipExtension_3R.Text = dt2.Rows[0]["Voluntary_HipExtension_3R"].ToString();
            //    Voluntary_HipExtension_3L.Text = dt2.Rows[0]["Voluntary_HipExtension_3L"].ToString();
            //    Voluntary_HipExtension_4R.Text = dt2.Rows[0]["Voluntary_HipExtension_4R"].ToString();
            //    Voluntary_HipExtension_4L.Text = dt2.Rows[0]["Voluntary_HipExtension_4L"].ToString();
            //    Voluntary_HipAbduction_1R.Text = dt2.Rows[0]["Voluntary_HipAbduction_1R"].ToString();
            //    Voluntary_HipAbduction_1L.Text = dt2.Rows[0]["Voluntary_HipAbduction_1L"].ToString();
            //    Voluntary_HipAbduction_2R.Text = dt2.Rows[0]["Voluntary_HipAbduction_2R"].ToString();
            //    Voluntary_HipAbduction_2L.Text = dt2.Rows[0]["Voluntary_HipAbduction_2L"].ToString();
            //    Voluntary_HipAbduction_3R.Text = dt2.Rows[0]["Voluntary_HipAbduction_3R"].ToString();
            //    Voluntary_HipAbduction_3L.Text = dt2.Rows[0]["Voluntary_HipAbduction_3L"].ToString();
            //    Voluntary_HipAbduction_4R.Text = dt2.Rows[0]["Voluntary_HipAbduction_4R"].ToString();
            //    Voluntary_HipAbduction_4L.Text = dt2.Rows[0]["Voluntary_HipAbduction_4L"].ToString();
            //    Voluntary_KneeFlexion_1R.Text = dt2.Rows[0]["Voluntary_KneeFlexion_1R"].ToString();
            //    Voluntary_KneeFlexion_1L.Text = dt2.Rows[0]["Voluntary_KneeFlexion_1L"].ToString();
            //    Voluntary_KneeFlexion_2R.Text = dt2.Rows[0]["Voluntary_KneeFlexion_2R"].ToString();
            //    Voluntary_KneeFlexion_2L.Text = dt2.Rows[0]["Voluntary_KneeFlexion_2L"].ToString();
            //    Voluntary_KneeFlexion_3R.Text = dt2.Rows[0]["Voluntary_KneeFlexion_3R"].ToString();
            //    Voluntary_KneeFlexion_3L.Text = dt2.Rows[0]["Voluntary_KneeFlexion_3L"].ToString();
            //    Voluntary_KneeFlexion_4R.Text = dt2.Rows[0]["Voluntary_KneeFlexion_4R"].ToString();
            //    Voluntary_KneeFlexion_4L.Text = dt2.Rows[0]["Voluntary_KneeFlexion_4L"].ToString();
            //    Voluntary_KneeExtension_1R.Text = dt2.Rows[0]["Voluntary_KneeExtension_1R"].ToString();
            //    Voluntary_KneeExtension_1L.Text = dt2.Rows[0]["Voluntary_KneeExtension_1L"].ToString();
            //    Voluntary_KneeExtension_2R.Text = dt2.Rows[0]["Voluntary_KneeExtension_2R"].ToString();
            //    Voluntary_KneeExtension_2L.Text = dt2.Rows[0]["Voluntary_KneeExtension_2L"].ToString();
            //    Voluntary_KneeExtension_3R.Text = dt2.Rows[0]["Voluntary_KneeExtension_3R"].ToString();
            //    Voluntary_KneeExtension_3L.Text = dt2.Rows[0]["Voluntary_KneeExtension_3L"].ToString();
            //    Voluntary_KneeExtension_4R.Text = dt2.Rows[0]["Voluntary_KneeExtension_4R"].ToString();
            //    Voluntary_KneeExtension_4L.Text = dt2.Rows[0]["Voluntary_KneeExtension_4L"].ToString();
            //    Voluntary_Dorsiflexion_1R.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_1R"].ToString();
            //    Voluntary_Dorsiflexion_1L.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_1L"].ToString();
            //    Voluntary_Dorsiflexion_2R.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_2R"].ToString();
            //    Voluntary_Dorsiflexion_2L.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_2L"].ToString();
            //    Voluntary_Dorsiflexion_3R.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_3R"].ToString();
            //    Voluntary_Dorsiflexion_3L.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_3L"].ToString();
            //    Voluntary_Dorsiflexion_4R.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_4R"].ToString();
            //    Voluntary_Dorsiflexion_4L.Text = dt2.Rows[0]["Voluntary_Dorsiflexion_4L"].ToString();
            //    Voluntary_Plantarflexion_1R.Text = dt2.Rows[0]["Voluntary_Plantarflexion_1R"].ToString();
            //    Voluntary_Plantarflexion_1L.Text = dt2.Rows[0]["Voluntary_Plantarflexion_1L"].ToString();
            //    Voluntary_Plantarflexion_2R.Text = dt2.Rows[0]["Voluntary_Plantarflexion_2R"].ToString();
            //    Voluntary_Plantarflexion_2L.Text = dt2.Rows[0]["Voluntary_Plantarflexion_2L"].ToString();
            //    Voluntary_Plantarflexion_3R.Text = dt2.Rows[0]["Voluntary_Plantarflexion_3R"].ToString();
            //    Voluntary_Plantarflexion_3L.Text = dt2.Rows[0]["Voluntary_Plantarflexion_3L"].ToString();
            //    Voluntary_Plantarflexion_4R.Text = dt2.Rows[0]["Voluntary_Plantarflexion_4R"].ToString();
            //    Voluntary_Plantarflexion_4L.Text = dt2.Rows[0]["Voluntary_Plantarflexion_4L"].ToString();
            //    Voluntary_WristDorsiflex_1R.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_1R"].ToString();
            //    Voluntary_WristDorsiflex_1L.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_1L"].ToString();
            //    Voluntary_WristDorsiflex_2R.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_2R"].ToString();
            //    Voluntary_WristDorsiflex_2L.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_2L"].ToString();
            //    Voluntary_WristDorsiflex_3R.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_3R"].ToString();
            //    Voluntary_WristDorsiflex_3L.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_3L"].ToString();
            //    Voluntary_WristDorsiflex_4R.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_4R"].ToString();
            //    Voluntary_WristDorsiflex_4L.Text = dt2.Rows[0]["Voluntary_WristDorsiflex_4L"].ToString();
            //    Voluntary_Grasp_1R.Text = dt2.Rows[0]["Voluntary_Grasp_1R"].ToString();
            //    Voluntary_Grasp_1L.Text = dt2.Rows[0]["Voluntary_Grasp_1L"].ToString();
            //    Voluntary_Grasp_2R.Text = dt2.Rows[0]["Voluntary_Grasp_2R"].ToString();
            //    Voluntary_Grasp_2L.Text = dt2.Rows[0]["Voluntary_Grasp_2L"].ToString();
            //    Voluntary_Grasp_3R.Text = dt2.Rows[0]["Voluntary_Grasp_3R"].ToString();
            //    Voluntary_Grasp_3L.Text = dt2.Rows[0]["Voluntary_Grasp_3L"].ToString();
            //    Voluntary_Grasp_4R.Text = dt2.Rows[0]["Voluntary_Grasp_4R"].ToString();
            //    Voluntary_Grasp_4L.Text = dt2.Rows[0]["Voluntary_Grasp_4L"].ToString();
            //    Voluntary_Release_1R.Text = dt2.Rows[0]["Voluntary_Release_1R"].ToString();
            //    Voluntary_Release_1L.Text = dt2.Rows[0]["Voluntary_Release_1L"].ToString();
            //    Voluntary_Release_2R.Text = dt2.Rows[0]["Voluntary_Release_2R"].ToString();
            //    Voluntary_Release_2L.Text = dt2.Rows[0]["Voluntary_Release_2L"].ToString();
            //    Voluntary_Release_3R.Text = dt2.Rows[0]["Voluntary_Release_3R"].ToString();
            //    Voluntary_Release_3L.Text = dt2.Rows[0]["Voluntary_Release_3L"].ToString();
            //    Voluntary_Release_4R.Text = dt2.Rows[0]["Voluntary_Release_4R"].ToString();
            //    Voluntary_Release_4L.Text = dt2.Rows[0]["Voluntary_Release_4L"].ToString();
            //}
            //#endregion
            //#region BOTOX DATA 3
            //DataTable dt3 = RDB.Get3(_appointmentID);
            //if (dt3.Rows.Count > 0)
            //{
            //    DateTime _FunctionalStrength_Date1 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date1);
            //    if (_FunctionalStrength_Date1 > DateTime.MinValue)
            //        FunctionalStrength_Date1.Text = _FunctionalStrength_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _FunctionalStrength_Date2 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date2);
            //    if (_FunctionalStrength_Date2 > DateTime.MinValue)
            //        FunctionalStrength_Date2.Text = _FunctionalStrength_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _FunctionalStrength_Date3 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date3);
            //    if (_FunctionalStrength_Date3 > DateTime.MinValue)
            //        FunctionalStrength_Date3.Text = _FunctionalStrength_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _FunctionalStrength_Date4 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date4);
            //    if (_FunctionalStrength_Date4 > DateTime.MinValue)
            //        FunctionalStrength_Date4.Text = _FunctionalStrength_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    FunctionalStrength_PullStand_1.Text = dt3.Rows[0]["FunctionalStrength_PullStand_1"].ToString();
            //    FunctionalStrength_PullStand_2.Text = dt3.Rows[0]["FunctionalStrength_PullStand_2"].ToString();
            //    FunctionalStrength_PullStand_3.Text = dt3.Rows[0]["FunctionalStrength_PullStand_3"].ToString();
            //    FunctionalStrength_PullStand_4.Text = dt3.Rows[0]["FunctionalStrength_PullStand_4"].ToString();
            //    FunctionalStrength_Independent3Sec_1.Text = dt3.Rows[0]["FunctionalStrength_Independent3Sec_1"].ToString();
            //    FunctionalStrength_Independent3Sec_2.Text = dt3.Rows[0]["FunctionalStrength_Independent3Sec_2"].ToString();
            //    FunctionalStrength_Independent3Sec_3.Text = dt3.Rows[0]["FunctionalStrength_Independent3Sec_3"].ToString();
            //    FunctionalStrength_Independent3Sec_4.Text = dt3.Rows[0]["FunctionalStrength_Independent3Sec_4"].ToString();
            //    FunctionalStrength_Independent20Sec_1.Text = dt3.Rows[0]["FunctionalStrength_Independent20Sec_1"].ToString();
            //    FunctionalStrength_Independent20Sec_2.Text = dt3.Rows[0]["FunctionalStrength_Independent20Sec_2"].ToString();
            //    FunctionalStrength_Independent20Sec_3.Text = dt3.Rows[0]["FunctionalStrength_Independent20Sec_3"].ToString();
            //    FunctionalStrength_Independent20Sec_4.Text = dt3.Rows[0]["FunctionalStrength_Independent20Sec_4"].ToString();
            //    FunctionalStrength_HandHeldR_1.Text = dt3.Rows[0]["FunctionalStrength_HandHeldR_1"].ToString();
            //    FunctionalStrength_HandHeldR_2.Text = dt3.Rows[0]["FunctionalStrength_HandHeldR_2"].ToString();
            //    FunctionalStrength_HandHeldR_3.Text = dt3.Rows[0]["FunctionalStrength_HandHeldR_3"].ToString();
            //    FunctionalStrength_HandHeldR_4.Text = dt3.Rows[0]["FunctionalStrength_HandHeldR_4"].ToString();
            //    FunctionalStrength_HandHeldL_1.Text = dt3.Rows[0]["FunctionalStrength_HandHeldL_1"].ToString();
            //    FunctionalStrength_HandHeldL_2.Text = dt3.Rows[0]["FunctionalStrength_HandHeldL_2"].ToString();
            //    FunctionalStrength_HandHeldL_3.Text = dt3.Rows[0]["FunctionalStrength_HandHeldL_3"].ToString();
            //    FunctionalStrength_HandHeldL_4.Text = dt3.Rows[0]["FunctionalStrength_HandHeldL_4"].ToString();
            //    FunctionalStrength_OneLegR_1.Text = dt3.Rows[0]["FunctionalStrength_OneLegR_1"].ToString();
            //    FunctionalStrength_OneLegR_2.Text = dt3.Rows[0]["FunctionalStrength_OneLegR_2"].ToString();
            //    FunctionalStrength_OneLegR_3.Text = dt3.Rows[0]["FunctionalStrength_OneLegR_3"].ToString();
            //    FunctionalStrength_OneLegR_4.Text = dt3.Rows[0]["FunctionalStrength_OneLegR_4"].ToString();
            //    FunctionalStrength_OneLegL_1.Text = dt3.Rows[0]["FunctionalStrength_OneLegL_1"].ToString();
            //    FunctionalStrength_OneLegL_2.Text = dt3.Rows[0]["FunctionalStrength_OneLegL_2"].ToString();
            //    FunctionalStrength_OneLegL_3.Text = dt3.Rows[0]["FunctionalStrength_OneLegL_3"].ToString();
            //    FunctionalStrength_OneLegL_4.Text = dt3.Rows[0]["FunctionalStrength_OneLegL_4"].ToString();
            //    FunctionalStrength_ShortSit_1.Text = dt3.Rows[0]["FunctionalStrength_ShortSit_1"].ToString();
            //    FunctionalStrength_ShortSit_2.Text = dt3.Rows[0]["FunctionalStrength_ShortSit_2"].ToString();
            //    FunctionalStrength_ShortSit_3.Text = dt3.Rows[0]["FunctionalStrength_ShortSit_3"].ToString();
            //    FunctionalStrength_ShortSit_4.Text = dt3.Rows[0]["FunctionalStrength_ShortSit_4"].ToString();
            //    FunctionalStrength_HighKneeR_1.Text = dt3.Rows[0]["FunctionalStrength_HighKneeR_1"].ToString();
            //    FunctionalStrength_HighKneeR_2.Text = dt3.Rows[0]["FunctionalStrength_HighKneeR_2"].ToString();
            //    FunctionalStrength_HighKneeR_3.Text = dt3.Rows[0]["FunctionalStrength_HighKneeR_3"].ToString();
            //    FunctionalStrength_HighKneeR_4.Text = dt3.Rows[0]["FunctionalStrength_HighKneeR_4"].ToString();
            //    FunctionalStrength_HighKneeL_1.Text = dt3.Rows[0]["FunctionalStrength_HighKneeL_1"].ToString();
            //    FunctionalStrength_HighKneeL_2.Text = dt3.Rows[0]["FunctionalStrength_HighKneeL_2"].ToString();
            //    FunctionalStrength_HighKneeL_3.Text = dt3.Rows[0]["FunctionalStrength_HighKneeL_3"].ToString();
            //    FunctionalStrength_HighKneeL_4.Text = dt3.Rows[0]["FunctionalStrength_HighKneeL_4"].ToString();
            //    FunctionalStrength_LowersFloor_1.Text = dt3.Rows[0]["FunctionalStrength_LowersFloor_1"].ToString();
            //    FunctionalStrength_LowersFloor_2.Text = dt3.Rows[0]["FunctionalStrength_LowersFloor_2"].ToString();
            //    FunctionalStrength_LowersFloor_3.Text = dt3.Rows[0]["FunctionalStrength_LowersFloor_3"].ToString();
            //    FunctionalStrength_LowersFloor_4.Text = dt3.Rows[0]["FunctionalStrength_LowersFloor_4"].ToString();
            //    FunctionalStrength_Squats_1.Text = dt3.Rows[0]["FunctionalStrength_Squats_1"].ToString();
            //    FunctionalStrength_Squats_2.Text = dt3.Rows[0]["FunctionalStrength_Squats_2"].ToString();
            //    FunctionalStrength_Squats_3.Text = dt3.Rows[0]["FunctionalStrength_Squats_3"].ToString();
            //    FunctionalStrength_Squats_4.Text = dt3.Rows[0]["FunctionalStrength_Squats_4"].ToString();
            //    FunctionalStrength_StandingPicks_1.Text = dt3.Rows[0]["FunctionalStrength_StandingPicks_1"].ToString();
            //    FunctionalStrength_StandingPicks_2.Text = dt3.Rows[0]["FunctionalStrength_StandingPicks_2"].ToString();
            //    FunctionalStrength_StandingPicks_3.Text = dt3.Rows[0]["FunctionalStrength_StandingPicks_3"].ToString();
            //    FunctionalStrength_StandingPicks_4.Text = dt3.Rows[0]["FunctionalStrength_StandingPicks_4"].ToString();
            //    FunctionalStrength_Total_1.Text = dt3.Rows[0]["FunctionalStrength_Total_1"].ToString();
            //    FunctionalStrength_Total_2.Text = dt3.Rows[0]["FunctionalStrength_Total_2"].ToString();
            //    FunctionalStrength_Total_3.Text = dt3.Rows[0]["FunctionalStrength_Total_2"].ToString();
            //    FunctionalStrength_Total_4.Text = dt3.Rows[0]["FunctionalStrength_Total_4"].ToString();
            //    DateTime _BotoxData_Date1 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date1);
            //    if (_BotoxData_Date1 > DateTime.MinValue)
            //        BotoxData_Date1.Text = _BotoxData_Date1.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _BotoxData_Date2 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date2);
            //    if (_BotoxData_Date2 > DateTime.MinValue)
            //        BotoxData_Date2.Text = _BotoxData_Date2.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _BotoxData_Date3 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date3);
            //    if (_BotoxData_Date3 > DateTime.MinValue)
            //        BotoxData_Date3.Text = _BotoxData_Date3.ToString(DbHelper.Configuration.showDateFormat);
            //    DateTime _BotoxData_Date4 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date4);
            //    if (_BotoxData_Date4 > DateTime.MinValue)
            //        BotoxData_Date4.Text = _BotoxData_Date4.ToString(DbHelper.Configuration.showDateFormat);
            //    BotoxData_Weight_1.Text = dt3.Rows[0]["BotoxData_Weight_1"].ToString();
            //    BotoxData_Weight_2.Text = dt3.Rows[0]["BotoxData_Weight_2"].ToString();
            //    BotoxData_Weight_3.Text = dt3.Rows[0]["BotoxData_Weight_3"].ToString();
            //    BotoxData_Weight_4.Text = dt3.Rows[0]["BotoxData_Weight_4"].ToString();
            //    BotoxData_BotoxInjected_1.Text = dt3.Rows[0]["BotoxData_BotoxInjected_1"].ToString();
            //    BotoxData_BotoxInjected_2.Text = dt3.Rows[0]["BotoxData_BotoxInjected_2"].ToString();
            //    BotoxData_BotoxInjected_3.Text = dt3.Rows[0]["BotoxData_BotoxInjected_3"].ToString();
            //    BotoxData_BotoxInjected_4.Text = dt3.Rows[0]["BotoxData_BotoxInjected_4"].ToString();
            //    BotoxData_Dilution_1.Text = dt3.Rows[0]["BotoxData_Dilution_1"].ToString();
            //    BotoxData_Dilution_2.Text = dt3.Rows[0]["BotoxData_Dilution_2"].ToString();
            //    BotoxData_Dilution_3.Text = dt3.Rows[0]["BotoxData_Dilution_3"].ToString();
            //    BotoxData_Dilution_4.Text = dt3.Rows[0]["BotoxData_Dilution_4"].ToString();
            //    BotoxData_MusclesInjected_1.Text = dt3.Rows[0]["BotoxData_MusclesInjected_1"].ToString();
            //    BotoxData_MusclesInjected_2.Text = dt3.Rows[0]["BotoxData_MusclesInjected_2"].ToString();
            //    BotoxData_MusclesInjected_3.Text = dt3.Rows[0]["BotoxData_MusclesInjected_3"].ToString();
            //    BotoxData_MusclesInjected_4.Text = dt3.Rows[0]["BotoxData_MusclesInjected_4"].ToString();
            //    BotoxData_Gastocnemius_1.Text = dt3.Rows[0]["BotoxData_Gastocnemius_1"].ToString();
            //    BotoxData_Gastocnemius_2.Text = dt3.Rows[0]["BotoxData_Gastocnemius_2"].ToString();
            //    BotoxData_Gastocnemius_3.Text = dt3.Rows[0]["BotoxData_Gastocnemius_3"].ToString();
            //    BotoxData_Gastocnemius_4.Text = dt3.Rows[0]["BotoxData_Gastocnemius_4"].ToString();
            //    BotoxData_Tibialis_1.Text = dt3.Rows[0]["BotoxData_Tibialis_1"].ToString();
            //    BotoxData_Tibialis_2.Text = dt3.Rows[0]["BotoxData_Tibialis_2"].ToString();
            //    BotoxData_Tibialis_3.Text = dt3.Rows[0]["BotoxData_Tibialis_3"].ToString();
            //    BotoxData_Tibialis_4.Text = dt3.Rows[0]["BotoxData_Tibialis_4"].ToString();
            //    BotoxData_Hamstrings_1.Text = dt3.Rows[0]["BotoxData_Hamstrings_1"].ToString();
            //    BotoxData_Hamstrings_2.Text = dt3.Rows[0]["BotoxData_Hamstrings_2"].ToString();
            //    BotoxData_Hamstrings_3.Text = dt3.Rows[0]["BotoxData_Hamstrings_3"].ToString();
            //    BotoxData_Hamstrings_4.Text = dt3.Rows[0]["BotoxData_Hamstrings_4"].ToString();
            //    BotoxData_Adductors_1.Text = dt3.Rows[0]["BotoxData_Adductors_1"].ToString();
            //    BotoxData_Adductors_2.Text = dt3.Rows[0]["BotoxData_Adductors_2"].ToString();
            //    BotoxData_Adductors_3.Text = dt3.Rows[0]["BotoxData_Adductors_3"].ToString();
            //    BotoxData_Adductors_4.Text = dt3.Rows[0]["BotoxData_Adductors_4"].ToString();
            //    BotoxData_Rectus_1.Text = dt3.Rows[0]["BotoxData_Rectus_1"].ToString();
            //    BotoxData_Rectus_2.Text = dt3.Rows[0]["BotoxData_Rectus_2"].ToString();
            //    BotoxData_Rectus_3.Text = dt3.Rows[0]["BotoxData_Rectus_3"].ToString();
            //    BotoxData_Rectus_4.Text = dt3.Rows[0]["BotoxData_Rectus_4"].ToString();
            //    BotoxData_Iliopsoas_1.Text = dt3.Rows[0]["BotoxData_Iliopsoas_1"].ToString();
            //    BotoxData_Iliopsoas_2.Text = dt3.Rows[0]["BotoxData_Iliopsoas_2"].ToString();
            //    BotoxData_Iliopsoas_3.Text = dt3.Rows[0]["BotoxData_Iliopsoas_3"].ToString();
            //    BotoxData_Iliopsoas_4.Text = dt3.Rows[0]["BotoxData_Iliopsoas_4"].ToString();
            //    BotoxData_Pronator_1.Text = dt3.Rows[0]["BotoxData_Pronator_1"].ToString();
            //    BotoxData_Pronator_2.Text = dt3.Rows[0]["BotoxData_Pronator_2"].ToString();
            //    BotoxData_Pronator_3.Text = dt3.Rows[0]["BotoxData_Pronator_3"].ToString();
            //    BotoxData_Pronator_4.Text = dt3.Rows[0]["BotoxData_Pronator_4"].ToString();
            //    BotoxData_FCR_1.Text = dt3.Rows[0]["BotoxData_FCR_1"].ToString();
            //    BotoxData_FCR_2.Text = dt3.Rows[0]["BotoxData_FCR_2"].ToString();
            //    BotoxData_FCR_3.Text = dt3.Rows[0]["BotoxData_FCR_3"].ToString();
            //    BotoxData_FCR_4.Text = dt3.Rows[0]["BotoxData_FCR_4"].ToString();
            //    BotoxData_FCU_1.Text = dt3.Rows[0]["BotoxData_FCU_1"].ToString();
            //    BotoxData_FCU_2.Text = dt3.Rows[0]["BotoxData_FCU_2"].ToString();
            //    BotoxData_FCU_3.Text = dt3.Rows[0]["BotoxData_FCU_3"].ToString();
            //    BotoxData_FCU_4.Text = dt3.Rows[0]["BotoxData_FCU_4"].ToString();
            //    BotoxData_FDS_1.Text = dt3.Rows[0]["BotoxData_FDS_1"].ToString();
            //    BotoxData_FDS_2.Text = dt3.Rows[0]["BotoxData_FDS_2"].ToString();
            //    BotoxData_FDS_3.Text = dt3.Rows[0]["BotoxData_FDS_3"].ToString();
            //    BotoxData_FDS_4.Text = dt3.Rows[0]["BotoxData_FDS_4"].ToString();
            //    BotoxData_FDP_1.Text = dt3.Rows[0]["BotoxData_FDP_1"].ToString();
            //    BotoxData_FDP_2.Text = dt3.Rows[0]["BotoxData_FDP_2"].ToString();
            //    BotoxData_FDP_3.Text = dt3.Rows[0]["BotoxData_FDP_3"].ToString();
            //    BotoxData_FDP_4.Text = dt3.Rows[0]["BotoxData_FDP_4"].ToString();
            //    BotoxData_FPL_1.Text = dt3.Rows[0]["BotoxData_FPL_1"].ToString();
            //    BotoxData_FPL_2.Text = dt3.Rows[0]["BotoxData_FPL_2"].ToString();
            //    BotoxData_FPL_3.Text = dt3.Rows[0]["BotoxData_FPL_3"].ToString();
            //    BotoxData_FPL_4.Text = dt3.Rows[0]["BotoxData_FPL_4"].ToString();
            //    BotoxData_Adductor_1.Text = dt3.Rows[0]["BotoxData_Adductor_1"].ToString();
            //    BotoxData_Adductor_2.Text = dt3.Rows[0]["BotoxData_Adductor_2"].ToString();
            //    BotoxData_Adductor_3.Text = dt3.Rows[0]["BotoxData_Adductor_3"].ToString();
            //    BotoxData_Adductor_4.Text = dt3.Rows[0]["BotoxData_Adductor_4"].ToString();
            //    BotoxData_Intrinsics_1.Text = dt3.Rows[0]["BotoxData_Intrinsics_1"].ToString();
            //    BotoxData_Intrinsics_2.Text = dt3.Rows[0]["BotoxData_Intrinsics_2"].ToString();
            //    BotoxData_Intrinsics_3.Text = dt3.Rows[0]["BotoxData_Intrinsics_3"].ToString();
            //    BotoxData_Intrinsics_4.Text = dt3.Rows[0]["BotoxData_Intrinsics_4"].ToString();
            //    BotoxData_Casting_1.Text = dt3.Rows[0]["BotoxData_Casting_1"].ToString();
            //    BotoxData_Casting_2.Text = dt3.Rows[0]["BotoxData_Casting_2"].ToString();
            //    BotoxData_Casting_3.Text = dt3.Rows[0]["BotoxData_Casting_3"].ToString();
            //    BotoxData_Casting_4.Text = dt3.Rows[0]["BotoxData_Casting_4"].ToString();
            //    int _Doctor_Director = 0; int.TryParse(dt3.Rows[0]["Doctor_Director"].ToString(), out _Doctor_Director);
            //    if (Doctor_Director.Items.FindByValue(_Doctor_Director.ToString()) != null)
            //    {
            //        Doctor_Director.SelectedValue = _Doctor_Director.ToString();
            //    }
            //    int _Doctor_Physiotheraist = 0; int.TryParse(dt3.Rows[0]["Doctor_Physiotheraist"].ToString(), out _Doctor_Physiotheraist);
            //    if (Doctor_Physiotheraist.Items.FindByValue(_Doctor_Physiotheraist.ToString()) != null)
            //    {
            //        Doctor_Physiotheraist.SelectedValue = _Doctor_Physiotheraist.ToString();
            //    }
            //    int _Doctor_Occupational = 0; int.TryParse(dt3.Rows[0]["Doctor_Occupational"].ToString(), out _Doctor_Occupational);
            //    if (Doctor_Occupational.Items.FindByValue(_Doctor_Occupational.ToString()) != null)
            //    {
            //        Doctor_Occupational.SelectedValue = _Doctor_Occupational.ToString();
            //    }
            //    bool IsFinal = false; bool.TryParse(dt3.Rows[0]["IsFinal"].ToString(), out IsFinal);
            //    txtFinal.Checked = IsFinal;
            //    bool IsGiven = false; bool.TryParse(dt3.Rows[0]["IsGiven"].ToString(), out IsGiven);
            //    txtGiven.Checked = IsGiven;
            //    if (txtGiven.Checked)
            //    {
            //        DateTime _GivenDate = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["GivenDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _GivenDate);
            //        if (_GivenDate > DateTime.MinValue)
            //            txtGivenDate.Text = _GivenDate.ToString(DbHelper.Configuration.showDateFormat);
            //    }
            //}
            //#endregion
            //#region BOTOX ATTRIBUTES
            //if (dt1.Rows.Count > 0 || dt2.Rows.Count > 0 || dt3.Rows.Count > 0)
            //{
            //    DataTable dtAttr1 = RDB.GetAttr(_appointmentID, RDB._milestonesTypeID);
            //    foreach (DataRow dr in dtAttr1.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in MilestonesType.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr2 = RDB.GetAttr(_appointmentID, RDB._assistiveDevicesTypeID);
            //    foreach (DataRow dr in dtAttr2.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in AssistiveDevices.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr3 = RDB.GetAttr(_appointmentID, RDB._orthoticsDevicesTypeID);
            //    foreach (DataRow dr in dtAttr3.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in OrthoticsDevices.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr4 = RDB.GetAttr(_appointmentID, RDB._ADLListTypeID);
            //    foreach (DataRow dr in dtAttr4.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in ADLList.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr5 = RDB.GetAttr(_appointmentID, RDB._indicationForBotoxTypeID);
            //    foreach (DataRow dr in dtAttr5.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in IndicationForBotox.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr6 = RDB.GetAttr(_appointmentID, RDB._ancillaryTreatmentTypeID);
            //    foreach (DataRow dr in dtAttr6.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in AncillaryTreatment.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //    DataTable dtAttr7 = RDB.GetAttr(_appointmentID, RDB._SideEffectsTypeID);
            //    foreach (DataRow dr in dtAttr7.Rows)
            //    {
            //        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
            //        foreach (ListItem li in SideEffects.Items)
            //        {
            //            if (li.Value == _attrID.ToString())
            //            {
            //                li.Selected = true; break;
            //            }
            //        }
            //    }
            //}
            //#endregion
            //if (dt1.Rows.Count > 0 || dt2.Rows.Count > 0 || dt3.Rows.Count > 0)
            //{
            //    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //int i = 0; int j = 0; int k = 0;

            //SnehBLL.ReportBotoxMst_Bll RDB = new SnehBLL.ReportBotoxMst_Bll();
            //string DiagnosisIDs = "";
            //for (int di = 0; di < txtDiagnosis.Items.Count; di++)
            //{
            //    if (txtDiagnosis.Items[di].Selected)
            //    {
            //        DiagnosisIDs += txtDiagnosis.Items[di].Value + "|";
            //    }
            //}
            //string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            //SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            //DIB.setFromOther(DiagnosisOther);

            //#region BOTOX DATA 1
            //int _General_Pediatrician = 0; if (General_Pediatrician.SelectedItem != null) { int.TryParse(General_Pediatrician.SelectedItem.Value, out _General_Pediatrician); }
            //int _General_Therapist = 0; if (General_Therapist.SelectedItem != null) { int.TryParse(General_Therapist.SelectedItem.Value, out _General_Therapist); }

            //int _HistoryExam_Delivery = 0; if (HistoryExam_Delivery.SelectedItem != null) { int.TryParse(HistoryExam_Delivery.SelectedItem.Value, out _HistoryExam_Delivery); }
            //int _HistoryExam_DiagnosedBy = 0; if (HistoryExam_DiagnosedBy.SelectedItem != null) { int.TryParse(HistoryExam_DiagnosedBy.SelectedItem.Value, out _HistoryExam_DiagnosedBy); }
            //int _TypeOfCP_CPID = 0; if (TypeOfCP_CPID.SelectedItem != null) { int.TryParse(TypeOfCP_CPID.SelectedItem.Value, out _TypeOfCP_CPID); }
            //int _AssistiveDevices_Orthotics = 0; if (AssistiveDevices_Orthotics.SelectedItem != null) { int.TryParse(AssistiveDevices_Orthotics.SelectedItem.Value, out _AssistiveDevices_Orthotics); }
            //int _ADL_adlID = 0; if (ADL_adlID.SelectedItem != null) { int.TryParse(ADL_adlID.SelectedItem.Value, out _ADL_adlID); }

            //DateTime _Ambulation_Date1 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date1);
            //DateTime _Ambulation_Date2 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date2);
            //DateTime _Ambulation_Date3 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date3);
            //DateTime _Ambulation_Date4 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date4);
            //DateTime _Ambulation_Date5 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date5.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date5);
            //DateTime _Ambulation_Date6 = new DateTime();
            //DateTime.TryParseExact(Ambulation_Date6.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date6);

            //DateTime _PreExisting_Date1 = new DateTime();
            //DateTime.TryParseExact(PreExisting_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date1);
            //DateTime _PreExisting_Date2 = new DateTime();
            //DateTime.TryParseExact(PreExisting_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date2);
            //DateTime _PreExisting_Date3 = new DateTime();
            //DateTime.TryParseExact(PreExisting_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date3);
            //DateTime _PreExisting_Date4 = new DateTime();
            //DateTime.TryParseExact(PreExisting_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date4);

            //DateTime _PassiveROM_Date1 = new DateTime();
            //DateTime.TryParseExact(PassiveROM_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date1);
            //DateTime _PassiveROM_Date2 = new DateTime();
            //DateTime.TryParseExact(PassiveROM_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date2);
            //DateTime _PassiveROM_Date3 = new DateTime();
            //DateTime.TryParseExact(PassiveROM_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date3);
            //DateTime _PassiveROM_Date4 = new DateTime();
            //DateTime.TryParseExact(PassiveROM_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date4);

            //DateTime _Tone_Date1 = new DateTime();
            //DateTime.TryParseExact(Tone_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date1);
            //DateTime _Tone_Date2 = new DateTime();
            //DateTime.TryParseExact(Tone_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date2);
            //DateTime _Tone_Date3 = new DateTime();
            //DateTime.TryParseExact(Tone_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date3);
            //DateTime _Tone_Date4 = new DateTime();
            //DateTime.TryParseExact(Tone_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date4);

            //DateTime _TardieusScale_Date1 = new DateTime();
            //DateTime.TryParseExact(TardieusScale_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date1);
            //DateTime _TardieusScale_Date2 = new DateTime();
            //DateTime.TryParseExact(TardieusScale_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date2);
            //DateTime _TardieusScale_Date3 = new DateTime();
            //DateTime.TryParseExact(TardieusScale_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date3);
            //DateTime _TardieusScale_Date4 = new DateTime();
            //DateTime.TryParseExact(TardieusScale_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date4);

            //i = RDB.Set1(_appointmentID, General_Pediatrician.Text.Trim(), _General_Pediatrician, _General_Therapist, _HistoryExam_Delivery, HistoryExam_PerinatalComplications.Text.Trim(), HistoryExam_BirthWeight.Text.Trim(), _HistoryExam_DiagnosedBy, _TypeOfCP_CPID, _AssistiveDevices_Orthotics, _ADL_adlID,
            //    _Ambulation_Date1, _Ambulation_Date2, _Ambulation_Date3, _Ambulation_Date4, _Ambulation_Date5, _Ambulation_Date6,
            //    Ambulation_Amb1.Text.Trim(), Ambulation_Amb2.Text.Trim(), Ambulation_Amb3.Text.Trim(), Ambulation_Amb4.Text.Trim(), Ambulation_Amb5.Text.Trim(), Ambulation_Amb6.Text.Trim(),
            //    _PreExisting_Date1, _PreExisting_Date2, _PreExisting_Date3, _PreExisting_Date4,
            //    PreExisting_HipFID_1R.Text.Trim(), PreExisting_HipFID_1L.Text.Trim(), PreExisting_HipFID_2R.Text.Trim(), PreExisting_HipFID_2L.Text.Trim(), PreExisting_HipFID_3R.Text.Trim(),
            //    PreExisting_HipFID_3L.Text.Trim(), PreExisting_HipFID_4R.Text.Trim(), PreExisting_HipFID_4L.Text.Trim(), PreExisting_HipAdduction_1R.Text.Trim(), PreExisting_HipAdduction_1L.Text.Trim(), PreExisting_HipAdduction_2R.Text.Trim(),
            //    PreExisting_HipAdduction_2L.Text.Trim(), PreExisting_HipAdduction_3R.Text.Trim(), PreExisting_HipAdduction_3L.Text.Trim(), PreExisting_HipAdduction_4R.Text.Trim(),
            //    PreExisting_HipAdduction_4L.Text.Trim(), PreExisting_KneeFFD_1R.Text.Trim(), PreExisting_KneeFFD_1L.Text.Trim(), PreExisting_KneeFFD_2R.Text.Trim(), PreExisting_KneeFFD_2L.Text.Trim(), PreExisting_KneeFFD_3R.Text.Trim(),
            //    PreExisting_KneeFFD_3L.Text.Trim(), PreExisting_KneeFFD_4R.Text.Trim(), PreExisting_KneeFFD_4L.Text.Trim(), PreExisting_Equinus_1R.Text.Trim(), PreExisting_Equinus_1L.Text.Trim(), PreExisting_Equinus_2R.Text.Trim(),
            //    PreExisting_Equinus_2L.Text.Trim(), PreExisting_Equinus_3R.Text.Trim(), PreExisting_Equinus_3L.Text.Trim(), PreExisting_Equinus_4R.Text.Trim(), PreExisting_Equinus_4L.Text.Trim(), PreExisting_Planovalgoid_1R.Text.Trim(),
            //    PreExisting_Planovalgoid_1L.Text.Trim(), PreExisting_Planovalgoid_2R.Text.Trim(), PreExisting_Planovalgoid_2L.Text.Trim(), PreExisting_Planovalgoid_3R.Text.Trim(), PreExisting_Planovalgoid_3L.Text.Trim(),
            //    PreExisting_Planovalgoid_4R.Text.Trim(), PreExisting_Planovalgoid_4L.Text.Trim(), PreExisting_Cavovarus_1R.Text.Trim(), PreExisting_Cavovarus_1L.Text.Trim(), PreExisting_Cavovarus_2R.Text.Trim(),
            //    PreExisting_Cavovarus_2L.Text.Trim(), PreExisting_Cavovarus_3R.Text.Trim(), PreExisting_Cavovarus_3L.Text.Trim(), PreExisting_Cavovarus_4R.Text.Trim(), PreExisting_Cavovarus_4L.Text.Trim(),
            //    PreExisting_ElbowFFD_1R.Text.Trim(), PreExisting_ElbowFFD_1L.Text.Trim(), PreExisting_ElbowFFD_2R.Text.Trim(), PreExisting_ElbowFFD_2L.Text.Trim(), PreExisting_ElbowFFD_3R.Text.Trim(),
            //    PreExisting_ElbowFFD_3L.Text.Trim(), PreExisting_ElbowFFD_4R.Text.Trim(), PreExisting_ElbowFFD_4L.Text.Trim(), PreExisting_WristFlexPron_1R.Text.Trim(), PreExisting_WristFlexPron_1L.Text.Trim(),
            //    PreExisting_WristFlexPron_2R.Text.Trim(), PreExisting_WristFlexPron_2L.Text.Trim(), PreExisting_WristFlexPron_3R.Text.Trim(), PreExisting_WristFlexPron_3L.Text.Trim(), PreExisting_WristFlexPron_4R.Text.Trim(),
            //    PreExisting_WristFlexPron_4L.Text.Trim(), _PassiveROM_Date1, _PassiveROM_Date2, _PassiveROM_Date3, _PassiveROM_Date4,
            //    PassiveROM_HipFlexion_1R.Text.Trim(), PassiveROM_HipFlexion_1L.Text.Trim(), PassiveROM_HipFlexion_2R.Text.Trim(), PassiveROM_HipFlexion_2L.Text.Trim(), PassiveROM_HipFlexion_3R.Text.Trim(), PassiveROM_HipFlexion_3L.Text.Trim(),
            //    PassiveROM_HipFlexion_4R.Text.Trim(), PassiveROM_HipFlexion_4L.Text.Trim(), PassiveROM_HipAbduction_1R.Text.Trim(), PassiveROM_HipAbduction_1L.Text.Trim(), PassiveROM_HipAbduction_2R.Text.Trim(),
            //    PassiveROM_HipAbduction_2L.Text.Trim(), PassiveROM_HipAbduction_3R.Text.Trim(), PassiveROM_HipAbduction_3L.Text.Trim(), PassiveROM_HipAbduction_4R.Text.Trim(), PassiveROM_HipAbduction_4L.Text.Trim(),
            //    PassiveROM_HipIR_1R.Text.Trim(), PassiveROM_HipIR_1L.Text.Trim(), PassiveROM_HipIR_2R.Text.Trim(), PassiveROM_HipIR_2L.Text.Trim(), PassiveROM_HipIR_3R.Text.Trim(), PassiveROM_HipIR_3L.Text.Trim(),
            //    PassiveROM_HipIR_4R.Text.Trim(), PassiveROM_HipIR_4L.Text.Trim(), PassiveROM_HipER_1R.Text.Trim(), PassiveROM_HipER_1L.Text.Trim(), PassiveROM_HipER_2R.Text.Trim(), PassiveROM_HipER_2L.Text.Trim(),
            //    PassiveROM_HipER_3R.Text.Trim(), PassiveROM_HipER_3L.Text.Trim(), PassiveROM_HipER_4R.Text.Trim(), PassiveROM_HipER_4L.Text.Trim(), PassiveROM_KneeFlexion_1R.Text.Trim(),
            //    PassiveROM_KneeFlexion_1L.Text.Trim(), PassiveROM_KneeFlexion_2R.Text.Trim(), PassiveROM_KneeFlexion_2L.Text.Trim(), PassiveROM_KneeFlexion_3R.Text.Trim(), PassiveROM_KneeFlexion_3L.Text.Trim(),
            //    PassiveROM_KneeFlexion_4R.Text.Trim(), PassiveROM_KneeFlexion_4L.Text.Trim(), PassiveROM_PoplitealAngle_1R.Text.Trim(), PassiveROM_PoplitealAngle_1L.Text.Trim(), PassiveROM_PoplitealAngle_2R.Text.Trim(),
            //    PassiveROM_PoplitealAngle_2L.Text.Trim(), PassiveROM_PoplitealAngle_3R.Text.Trim(), PassiveROM_PoplitealAngle_3L.Text.Trim(), PassiveROM_PoplitealAngle_4R.Text.Trim(),
            //    PassiveROM_PoplitealAngle_4L.Text.Trim(), PassiveROM_KneeExt_1R.Text.Trim(), PassiveROM_KneeExt_1L.Text.Trim(), PassiveROM_KneeExt_2R.Text.Trim(), PassiveROM_KneeExt_2L.Text.Trim(),
            //    PassiveROM_KneeExt_3R.Text.Trim(), PassiveROM_KneeExt_3L.Text.Trim(), PassiveROM_KneeExt_4R.Text.Trim(), PassiveROM_KneeExt_4L.Text.Trim(), PassiveROM_KneeFlex_1R.Text.Trim(),
            //    PassiveROM_KneeFlex_1L.Text.Trim(), PassiveROM_KneeFlex_2R.Text.Trim(), PassiveROM_KneeFlex_2L.Text.Trim(), PassiveROM_KneeFlex_3R.Text.Trim(), PassiveROM_KneeFlex_3L.Text.Trim(),
            //    PassiveROM_KneeFlex_4R.Text.Trim(), PassiveROM_KneeFlex_4L.Text.Trim(), PassiveROM_Plantarflexion_1R.Text.Trim(), PassiveROM_Plantarflexion_1L.Text.Trim(), PassiveROM_Plantarflexion_2R.Text.Trim(),
            //    PassiveROM_Plantarflexion_2L.Text.Trim(), PassiveROM_Plantarflexion_3R.Text.Trim(), PassiveROM_Plantarflexion_3L.Text.Trim(), PassiveROM_Plantarflexion_4R.Text.Trim(),
            //    PassiveROM_Plantarflexion_4L.Text.Trim(), PassiveROM_AnkleInv_1R.Text.Trim(), PassiveROM_AnkleInv_1L.Text.Trim(), PassiveROM_AnkleInv_2R.Text.Trim(), PassiveROM_AnkleInv_2L.Text.Trim(),
            //    PassiveROM_AnkleInv_3R.Text.Trim(), PassiveROM_AnkleInv_3L.Text.Trim(), PassiveROM_AnkleInv_4R.Text.Trim(), PassiveROM_AnkleInv_4L.Text.Trim(), PassiveROM_AnkleEver_1R.Text.Trim(),
            //    PassiveROM_AnkleEver_1L.Text.Trim(), PassiveROM_AnkleEver_2R.Text.Trim(), PassiveROM_AnkleEver_2L.Text.Trim(), PassiveROM_AnkleEver_3R.Text.Trim(), PassiveROM_AnkleEver_3L.Text.Trim(),
            //    PassiveROM_AnkleEver_4R.Text.Trim(), PassiveROM_AnkleEver_4L.Text.Trim(), _Tone_Date1, _Tone_Date2, _Tone_Date3, _Tone_Date4,
            //    Tone_Iliopsoas_1R.Text.Trim(), Tone_Iliopsoas_1L.Text.Trim(), Tone_Iliopsoas_2R.Text.Trim(), Tone_Iliopsoas_2L.Text.Trim(), Tone_Iliopsoas_3R.Text.Trim(), Tone_Iliopsoas_3L.Text.Trim(), Tone_Iliopsoas_4R.Text.Trim(), Tone_Iliopsoas_4L.Text.Trim(),
            //    Tone_Adductors_1R.Text.Trim(), Tone_Adductors_1L.Text.Trim(), Tone_Adductors_2R.Text.Trim(), Tone_Adductors_2L.Text.Trim(), Tone_Adductors_3R.Text.Trim(), Tone_Adductors_3L.Text.Trim(), Tone_Adductors_4R.Text.Trim(), Tone_Adductors_4L.Text.Trim(), Tone_RectusFemoris_1R.Text.Trim(),
            //    Tone_RectusFemoris_1L.Text.Trim(), Tone_RectusFemoris_2R.Text.Trim(), Tone_RectusFemoris_2L.Text.Trim(), Tone_RectusFemoris_3R.Text.Trim(), Tone_RectusFemoris_3L.Text.Trim(), Tone_RectusFemoris_4R.Text.Trim(),
            //    Tone_RectusFemoris_4L.Text.Trim(), Tone_Hamstrings_1R.Text.Trim(), Tone_Hamstrings_1L.Text.Trim(), Tone_Hamstrings_2R.Text.Trim(), Tone_Hamstrings_2L.Text.Trim(), Tone_Hamstrings_3R.Text.Trim(), Tone_Hamstrings_3L.Text.Trim(),
            //    Tone_Hamstrings_4R.Text.Trim(), Tone_Hamstrings_4L.Text.Trim(), Tone_Gastrosoleus_1R.Text.Trim(), Tone_Gastrosoleus_1L.Text.Trim(), Tone_Gastrosoleus_2R.Text.Trim(), Tone_Gastrosoleus_2L.Text.Trim(),
            //    Tone_Gastrosoleus_3R.Text.Trim(), Tone_Gastrosoleus_3L.Text.Trim(), Tone_Gastrosoleus_4R.Text.Trim(), Tone_Gastrosoleus_4L.Text.Trim(), Tone_ElbowFlexors_1R.Text.Trim(), Tone_ElbowFlexors_1L.Text.Trim(),
            //    Tone_ElbowFlexors_2R.Text.Trim(), Tone_ElbowFlexors_2L.Text.Trim(), Tone_ElbowFlexors_3R.Text.Trim(), Tone_ElbowFlexors_3L.Text.Trim(), Tone_ElbowFlexors_4R.Text.Trim(), Tone_ElbowFlexors_4L.Text.Trim(),
            //    Tone_WristFlexors_1R.Text.Trim(), Tone_WristFlexors_1L.Text.Trim(), Tone_WristFlexors_2R.Text.Trim(), Tone_WristFlexors_2L.Text.Trim(), Tone_WristFlexors_3R.Text.Trim(), Tone_WristFlexors_3L.Text.Trim(),
            //    Tone_WristFlexors_4R.Text.Trim(), Tone_WristFlexors_4L.Text.Trim(), Tone_FingerFlexors_1R.Text.Trim(), Tone_FingerFlexors_1L.Text.Trim(), Tone_FingerFlexors_2R.Text.Trim(), Tone_FingerFlexors_2L.Text.Trim(),
            //    Tone_FingerFlexors_3R.Text.Trim(), Tone_FingerFlexors_3L.Text.Trim(), Tone_FingerFlexors_4R.Text.Trim(), Tone_FingerFlexors_4L.Text.Trim(), Tone_PronatorFlexors_1R.Text.Trim(), Tone_PronatorFlexors_1L.Text.Trim(),
            //    Tone_PronatorFlexors_2R.Text.Trim(), Tone_PronatorFlexors_2L.Text.Trim(), Tone_PronatorFlexors_3R.Text.Trim(), Tone_PronatorFlexors_3L.Text.Trim(), Tone_PronatorFlexors_4R.Text.Trim(), Tone_PronatorFlexors_4L.Text.Trim(),
            //    _TardieusScale_Date1, _TardieusScale_Date2, _TardieusScale_Date3, _TardieusScale_Date4,
            //    TardieusScale_GastrosoleusR1_1R.Text.Trim(), TardieusScale_GastrosoleusR1_1L.Text.Trim(), TardieusScale_GastrosoleusR1_2R.Text.Trim(), TardieusScale_GastrosoleusR1_2L.Text.Trim(), TardieusScale_GastrosoleusR1_3R.Text.Trim(),
            //    TardieusScale_GastrosoleusR1_3L.Text.Trim(), TardieusScale_GastrosoleusR1_4R.Text.Trim(), TardieusScale_GastrosoleusR1_4L.Text.Trim(), TardieusScale_GastrosoleusR2_1R.Text.Trim(), TardieusScale_GastrosoleusR2_1L.Text.Trim(),
            //    TardieusScale_GastrosoleusR2_2R.Text.Trim(), TardieusScale_GastrosoleusR2_2L.Text.Trim(), TardieusScale_GastrosoleusR2_3R.Text.Trim(), TardieusScale_GastrosoleusR2_3L.Text.Trim(),
            //    TardieusScale_GastrosoleusR2_4R.Text.Trim(), TardieusScale_GastrosoleusR2_4L.Text.Trim(), TardieusScale_GastrosoleusR3_1R.Text.Trim(), TardieusScale_GastrosoleusR3_1L.Text.Trim(),
            //    TardieusScale_GastrosoleusR3_2R.Text.Trim(), TardieusScale_GastrosoleusR3_2L.Text.Trim(), TardieusScale_GastrosoleusR3_3R.Text.Trim(), TardieusScale_GastrosoleusR3_3L.Text.Trim(),
            //    TardieusScale_GastrosoleusR3_4R.Text.Trim(), TardieusScale_GastrosoleusR3_4L.Text.Trim(), TardieusScale_HamstringsR1_1R.Text.Trim(), TardieusScale_HamstringsR1_1L.Text.Trim(),
            //    TardieusScale_HamstringsR1_2R.Text.Trim(), TardieusScale_HamstringsR1_2L.Text.Trim(), TardieusScale_HamstringsR1_3R.Text.Trim(), TardieusScale_HamstringsR1_3L.Text.Trim(),
            //    TardieusScale_HamstringsR1_4R.Text.Trim(), TardieusScale_HamstringsR1_4L.Text.Trim(), TardieusScale_HamstringsR2_1R.Text.Trim(), TardieusScale_HamstringsR2_1L.Text.Trim(),
            //    TardieusScale_HamstringsR2_2R.Text.Trim(), TardieusScale_HamstringsR2_2L.Text.Trim(), TardieusScale_HamstringsR2_3R.Text.Trim(), TardieusScale_HamstringsR2_3L.Text.Trim(),
            //    TardieusScale_HamstringsR2_4R.Text.Trim(), TardieusScale_HamstringsR2_4L.Text.Trim(), DiagnosisIDs, DiagnosisOther);
            //#endregion
            //#region BOTOX DATA 2
            //DateTime _MuscleStrength_Date1 = new DateTime();
            //DateTime.TryParseExact(MuscleStrength_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date1);
            //DateTime _MuscleStrength_Date2 = new DateTime();
            //DateTime.TryParseExact(MuscleStrength_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date2);
            //DateTime _MuscleStrength_Date3 = new DateTime();
            //DateTime.TryParseExact(MuscleStrength_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date3);
            //DateTime _MuscleStrength_Date4 = new DateTime();
            //DateTime.TryParseExact(MuscleStrength_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date4);

            //DateTime _Voluntary_Date1 = new DateTime();
            //DateTime.TryParseExact(Voluntary_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date1);
            //DateTime _Voluntary_Date2 = new DateTime();
            //DateTime.TryParseExact(Voluntary_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date2);
            //DateTime _Voluntary_Date3 = new DateTime();
            //DateTime.TryParseExact(Voluntary_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date3);
            //DateTime _Voluntary_Date4 = new DateTime();
            //DateTime.TryParseExact(Voluntary_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date4);

            //j = RDB.Set2(_appointmentID, _MuscleStrength_Date1, _MuscleStrength_Date2, _MuscleStrength_Date3, _MuscleStrength_Date4,
            //    MuscleStrength_Iliopsoas_1R.Text.Trim(), MuscleStrength_Iliopsoas_1L.Text.Trim(), MuscleStrength_Iliopsoas_2R.Text.Trim(), MuscleStrength_Iliopsoas_2L.Text.Trim(), MuscleStrength_Iliopsoas_3R.Text.Trim(), MuscleStrength_Iliopsoas_3L.Text.Trim(),
            //    MuscleStrength_Iliopsoas_4R.Text.Trim(), MuscleStrength_Iliopsoas_4L.Text.Trim(), MuscleStrength_GluteusMax_1R.Text.Trim(), MuscleStrength_GluteusMax_1L.Text.Trim(), MuscleStrength_GluteusMax_2R.Text.Trim(),
            //    MuscleStrength_GluteusMax_2L.Text.Trim(), MuscleStrength_GluteusMax_3R.Text.Trim(), MuscleStrength_GluteusMax_3L.Text.Trim(), MuscleStrength_GluteusMax_4R.Text.Trim(),
            //    MuscleStrength_GluteusMax_4L.Text.Trim(), MuscleStrength_Abductors_1R.Text.Trim(), MuscleStrength_Abductors_1L.Text.Trim(), MuscleStrength_Abductors_2R.Text.Trim(), MuscleStrength_Abductors_2L.Text.Trim(),
            //    MuscleStrength_Abductors_3R.Text.Trim(), MuscleStrength_Abductors_3L.Text.Trim(), MuscleStrength_Abductors_4R.Text.Trim(), MuscleStrength_Abductors_4L.Text.Trim(), MuscleStrength_RectusFemoris_1R.Text.Trim(),
            //    MuscleStrength_RectusFemoris_1L.Text.Trim(), MuscleStrength_RectusFemoris_2R.Text.Trim(), MuscleStrength_RectusFemoris_2L.Text.Trim(), MuscleStrength_RectusFemoris_3R.Text.Trim(),
            //    MuscleStrength_RectusFemoris_3L.Text.Trim(), MuscleStrength_RectusFemoris_4R.Text.Trim(), MuscleStrength_RectusFemoris_4L.Text.Trim(), MuscleStrength_Hamstrings_1R.Text.Trim(),
            //    MuscleStrength_Hamstrings_1L.Text.Trim(), MuscleStrength_Hamstrings_2R.Text.Trim(), MuscleStrength_Hamstrings_2L.Text.Trim(), MuscleStrength_Hamstrings_3R.Text.Trim(),
            //    MuscleStrength_Hamstrings_3L.Text.Trim(), MuscleStrength_Hamstrings_4R.Text.Trim(), MuscleStrength_Hamstrings_4L.Text.Trim(), MuscleStrength_Gastrosoleus_1R.Text.Trim(),
            //    MuscleStrength_Gastrosoleus_1L.Text.Trim(), MuscleStrength_Gastrosoleus_2R.Text.Trim(), MuscleStrength_Gastrosoleus_2L.Text.Trim(), MuscleStrength_Gastrosoleus_3R.Text.Trim(),
            //    MuscleStrength_Gastrosoleus_3L.Text.Trim(), MuscleStrength_Gastrosoleus_4R.Text.Trim(), MuscleStrength_Gastrosoleus_4L.Text.Trim(), MuscleStrength_TibialisAnt_1R.Text.Trim(),
            //    MuscleStrength_TibialisAnt_1L.Text.Trim(), MuscleStrength_TibialisAnt_2R.Text.Trim(), MuscleStrength_TibialisAnt_2L.Text.Trim(), MuscleStrength_TibialisAnt_3R.Text.Trim(), MuscleStrength_TibialisAnt_3L.Text.Trim(),
            //    MuscleStrength_TibialisAnt_4R.Text.Trim(), MuscleStrength_TibialisAnt_4L.Text.Trim(), MuscleStrength_ElbowFlexors_1R.Text.Trim(), MuscleStrength_ElbowFlexors_1L.Text.Trim(),
            //    MuscleStrength_ElbowFlexors_2R.Text.Trim(), MuscleStrength_ElbowFlexors_2L.Text.Trim(), MuscleStrength_ElbowFlexors_3R.Text.Trim(), MuscleStrength_ElbowFlexors_3L.Text.Trim(),
            //    MuscleStrength_ElbowFlexors_4R.Text.Trim(), MuscleStrength_ElbowFlexors_4L.Text.Trim(), MuscleStrength_PronatorTeres_1R.Text.Trim(), MuscleStrength_PronatorTeres_1L.Text.Trim(),
            //    MuscleStrength_PronatorTeres_2R.Text.Trim(), MuscleStrength_PronatorTeres_2L.Text.Trim(), MuscleStrength_PronatorTeres_3R.Text.Trim(), MuscleStrength_PronatorTeres_3L.Text.Trim(),
            //    MuscleStrength_PronatorTeres_4R.Text.Trim(), MuscleStrength_PronatorTeres_4L.Text.Trim(), MuscleStrength_WristFlexors_1R.Text.Trim(), MuscleStrength_WristFlexors_1L.Text.Trim(),
            //    MuscleStrength_WristFlexors_2R.Text.Trim(), MuscleStrength_WristFlexors_2L.Text.Trim(), MuscleStrength_WristFlexors_3R.Text.Trim(), MuscleStrength_WristFlexors_3L.Text.Trim(),
            //    MuscleStrength_WristFlexors_4R.Text.Trim(), MuscleStrength_WristFlexors_4L.Text.Trim(), MuscleStrength_WristExtensors_1R.Text.Trim(), MuscleStrength_WristExtensors_1L.Text.Trim(),
            //    MuscleStrength_WristExtensors_2R.Text.Trim(), MuscleStrength_WristExtensors_2L.Text.Trim(), MuscleStrength_WristExtensors_3R.Text.Trim(), MuscleStrength_WristExtensors_3L.Text.Trim(),
            //    MuscleStrength_WristExtensors_4R.Text.Trim(), MuscleStrength_WristExtensors_4L.Text.Trim(), MuscleStrength_FingerFlexors_1R.Text.Trim(), MuscleStrength_FingerFlexors_1L.Text.Trim(),
            //    MuscleStrength_FingerFlexors_2R.Text.Trim(), MuscleStrength_FingerFlexors_2L.Text.Trim(), MuscleStrength_FingerFlexors_3R.Text.Trim(), MuscleStrength_FingerFlexors_3L.Text.Trim(),
            //    MuscleStrength_FingerFlexors_4R.Text.Trim(), MuscleStrength_FingerFlexors_4L.Text.Trim(), _Voluntary_Date1, _Voluntary_Date2, _Voluntary_Date3, _Voluntary_Date4, Voluntary_HipFlexion_1R.Text.Trim(),
            //    Voluntary_HipFlexion_1L.Text.Trim(), Voluntary_HipFlexion_2R.Text.Trim(), Voluntary_HipFlexion_2L.Text.Trim(), Voluntary_HipFlexion_3R.Text.Trim(), Voluntary_HipFlexion_3L.Text.Trim(),
            //    Voluntary_HipFlexion_4R.Text.Trim(), Voluntary_HipFlexion_4L.Text.Trim(), Voluntary_HipExtension_1R.Text.Trim(), Voluntary_HipExtension_1L.Text.Trim(), Voluntary_HipExtension_2R.Text.Trim(),
            //    Voluntary_HipExtension_2L.Text.Trim(), Voluntary_HipExtension_3R.Text.Trim(), Voluntary_HipExtension_3L.Text.Trim(), Voluntary_HipExtension_4R.Text.Trim(), Voluntary_HipExtension_4L.Text.Trim(),
            //    Voluntary_HipAbduction_1R.Text.Trim(), Voluntary_HipAbduction_1L.Text.Trim(), Voluntary_HipAbduction_2R.Text.Trim(), Voluntary_HipAbduction_2L.Text.Trim(), Voluntary_HipAbduction_3R.Text.Trim(),
            //    Voluntary_HipAbduction_3L.Text.Trim(), Voluntary_HipAbduction_4R.Text.Trim(), Voluntary_HipAbduction_4L.Text.Trim(), Voluntary_KneeFlexion_1R.Text.Trim(), Voluntary_KneeFlexion_1L.Text.Trim(),
            //    Voluntary_KneeFlexion_2R.Text.Trim(), Voluntary_KneeFlexion_2L.Text.Trim(), Voluntary_KneeFlexion_3R.Text.Trim(), Voluntary_KneeFlexion_3L.Text.Trim(), Voluntary_KneeFlexion_4R.Text.Trim(),
            //    Voluntary_KneeFlexion_4L.Text.Trim(), Voluntary_KneeExtension_1R.Text.Trim(), Voluntary_KneeExtension_1L.Text.Trim(), Voluntary_KneeExtension_2R.Text.Trim(), Voluntary_KneeExtension_2L.Text.Trim(),
            //    Voluntary_KneeExtension_3R.Text.Trim(), Voluntary_KneeExtension_3L.Text.Trim(), Voluntary_KneeExtension_4R.Text.Trim(), Voluntary_KneeExtension_4L.Text.Trim(), Voluntary_Dorsiflexion_1R.Text.Trim(),
            //    Voluntary_Dorsiflexion_1L.Text.Trim(), Voluntary_Dorsiflexion_2R.Text.Trim(), Voluntary_Dorsiflexion_2L.Text.Trim(), Voluntary_Dorsiflexion_3R.Text.Trim(), Voluntary_Dorsiflexion_3L.Text.Trim(),
            //    Voluntary_Dorsiflexion_4R.Text.Trim(), Voluntary_Dorsiflexion_4L.Text.Trim(), Voluntary_Plantarflexion_1R.Text.Trim(), Voluntary_Plantarflexion_1L.Text.Trim(), Voluntary_Plantarflexion_2R.Text.Trim(),
            //    Voluntary_Plantarflexion_2L.Text.Trim(), Voluntary_Plantarflexion_3R.Text.Trim(), Voluntary_Plantarflexion_3L.Text.Trim(), Voluntary_Plantarflexion_4R.Text.Trim(), Voluntary_Plantarflexion_4L.Text.Trim(),
            //    Voluntary_WristDorsiflex_1R.Text.Trim(), Voluntary_WristDorsiflex_1L.Text.Trim(), Voluntary_WristDorsiflex_2R.Text.Trim(), Voluntary_WristDorsiflex_2L.Text.Trim(), Voluntary_WristDorsiflex_3R.Text.Trim(),
            //    Voluntary_WristDorsiflex_3L.Text.Trim(), Voluntary_WristDorsiflex_4R.Text.Trim(), Voluntary_WristDorsiflex_4L.Text.Trim(), Voluntary_Grasp_1R.Text.Trim(), Voluntary_Grasp_1L.Text.Trim(), Voluntary_Grasp_2R.Text.Trim(),
            //    Voluntary_Grasp_2L.Text.Trim(), Voluntary_Grasp_3R.Text.Trim(), Voluntary_Grasp_3L.Text.Trim(), Voluntary_Grasp_4R.Text.Trim(), Voluntary_Grasp_4L.Text.Trim(), Voluntary_Release_1R.Text.Trim(), Voluntary_Release_1L.Text.Trim(),
            //    Voluntary_Release_2R.Text.Trim(), Voluntary_Release_2L.Text.Trim(), Voluntary_Release_3R.Text.Trim(), Voluntary_Release_3L.Text.Trim(), Voluntary_Release_4R.Text.Trim(), Voluntary_Release_4L.Text.Trim());
            //#endregion
            //#region BOTOX DATA 3
            //DateTime _FunctionalStrength_Date1 = new DateTime();
            //DateTime.TryParseExact(FunctionalStrength_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date1);
            //DateTime _FunctionalStrength_Date2 = new DateTime();
            //DateTime.TryParseExact(FunctionalStrength_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date2);
            //DateTime _FunctionalStrength_Date3 = new DateTime();
            //DateTime.TryParseExact(FunctionalStrength_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date3);
            //DateTime _FunctionalStrength_Date4 = new DateTime();
            //DateTime.TryParseExact(FunctionalStrength_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date4);

            //DateTime _BotoxData_Date1 = new DateTime();
            //DateTime.TryParseExact(BotoxData_Date1.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date1);
            //DateTime _BotoxData_Date2 = new DateTime();
            //DateTime.TryParseExact(BotoxData_Date2.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date2);
            //DateTime _BotoxData_Date3 = new DateTime();
            //DateTime.TryParseExact(BotoxData_Date3.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date3);
            //DateTime _BotoxData_Date4 = new DateTime();
            //DateTime.TryParseExact(BotoxData_Date4.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date4);

            //int _Doctor_Director = 0; if (Doctor_Director.SelectedItem != null) { int.TryParse(Doctor_Director.SelectedItem.Value, out _Doctor_Director); }
            //int _Doctor_Physiotheraist = 0; if (Doctor_Physiotheraist.SelectedItem != null) { int.TryParse(Doctor_Physiotheraist.SelectedItem.Value, out _Doctor_Physiotheraist); }
            //int _Doctor_Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out _Doctor_Occupational); }

            //bool IsFinal = txtFinal.Checked;
            //bool IsGiven = txtGiven.Checked;
            //DateTime GivenDate = new DateTime();
            //if (IsGiven)
            //{
            //    if (txtGivenDate.Text.Trim().Length > 0)
            //    {
            //        DateTime.TryParseExact(txtGivenDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
            //    }
            //}

            //k = RDB.Set3(_appointmentID, _FunctionalStrength_Date1, _FunctionalStrength_Date2, _FunctionalStrength_Date3, _FunctionalStrength_Date4,
            //    FunctionalStrength_PullStand_1.Text.Trim(), FunctionalStrength_PullStand_2.Text.Trim(), FunctionalStrength_PullStand_3.Text.Trim(), FunctionalStrength_PullStand_4.Text.Trim(), FunctionalStrength_Independent3Sec_1.Text.Trim(),
            //    FunctionalStrength_Independent3Sec_2.Text.Trim(), FunctionalStrength_Independent3Sec_3.Text.Trim(), FunctionalStrength_Independent3Sec_4.Text.Trim(), FunctionalStrength_Independent20Sec_1.Text.Trim(),
            //    FunctionalStrength_Independent20Sec_2.Text.Trim(), FunctionalStrength_Independent20Sec_3.Text.Trim(), FunctionalStrength_Independent20Sec_4.Text.Trim(), FunctionalStrength_HandHeldR_1.Text.Trim(),
            //    FunctionalStrength_HandHeldR_2.Text.Trim(), FunctionalStrength_HandHeldR_3.Text.Trim(), FunctionalStrength_HandHeldR_4.Text.Trim(), FunctionalStrength_HandHeldL_1.Text.Trim(),
            //    FunctionalStrength_HandHeldL_2.Text.Trim(), FunctionalStrength_HandHeldL_3.Text.Trim(), FunctionalStrength_HandHeldL_4.Text.Trim(), FunctionalStrength_OneLegR_1.Text.Trim(),
            //    FunctionalStrength_OneLegR_2.Text.Trim(), FunctionalStrength_OneLegR_3.Text.Trim(), FunctionalStrength_OneLegR_4.Text.Trim(), FunctionalStrength_OneLegL_1.Text.Trim(), FunctionalStrength_OneLegL_2.Text.Trim(),
            //    FunctionalStrength_OneLegL_3.Text.Trim(), FunctionalStrength_OneLegL_4.Text.Trim(), FunctionalStrength_ShortSit_1.Text.Trim(), FunctionalStrength_ShortSit_2.Text.Trim(), FunctionalStrength_ShortSit_3.Text.Trim(),
            //    FunctionalStrength_ShortSit_4.Text.Trim(), FunctionalStrength_HighKneeR_1.Text.Trim(), FunctionalStrength_HighKneeR_2.Text.Trim(), FunctionalStrength_HighKneeR_3.Text.Trim(),
            //    FunctionalStrength_HighKneeR_4.Text.Trim(), FunctionalStrength_HighKneeL_1.Text.Trim(), FunctionalStrength_HighKneeL_2.Text.Trim(), FunctionalStrength_HighKneeL_3.Text.Trim(),
            //    FunctionalStrength_HighKneeL_4.Text.Trim(), FunctionalStrength_LowersFloor_1.Text.Trim(), FunctionalStrength_LowersFloor_2.Text.Trim(), FunctionalStrength_LowersFloor_3.Text.Trim(),
            //    FunctionalStrength_LowersFloor_4.Text.Trim(), FunctionalStrength_Squats_1.Text.Trim(), FunctionalStrength_Squats_2.Text.Trim(), FunctionalStrength_Squats_3.Text.Trim(), FunctionalStrength_Squats_4.Text.Trim(),
            //    FunctionalStrength_StandingPicks_1.Text.Trim(), FunctionalStrength_StandingPicks_2.Text.Trim(), FunctionalStrength_StandingPicks_3.Text.Trim(), FunctionalStrength_StandingPicks_4.Text.Trim(),
            //    FunctionalStrength_Total_1.Text.Trim(), FunctionalStrength_Total_2.Text.Trim(), FunctionalStrength_Total_3.Text.Trim(), FunctionalStrength_Total_4.Text.Trim(), _BotoxData_Date1, _BotoxData_Date2, _BotoxData_Date3, _BotoxData_Date4,
            //    BotoxData_Weight_1.Text.Trim(), BotoxData_Weight_2.Text.Trim(), BotoxData_Weight_3.Text.Trim(), BotoxData_Weight_4.Text.Trim(), BotoxData_BotoxInjected_1.Text.Trim(),
            //    BotoxData_BotoxInjected_2.Text.Trim(), BotoxData_BotoxInjected_3.Text.Trim(), BotoxData_BotoxInjected_4.Text.Trim(), BotoxData_Dilution_1.Text.Trim(), BotoxData_Dilution_2.Text.Trim(), BotoxData_Dilution_3.Text.Trim(),
            //    BotoxData_Dilution_4.Text.Trim(), BotoxData_MusclesInjected_1.Text.Trim(), BotoxData_MusclesInjected_2.Text.Trim(), BotoxData_MusclesInjected_3.Text.Trim(), BotoxData_MusclesInjected_4.Text.Trim(),
            //    BotoxData_Gastocnemius_1.Text.Trim(), BotoxData_Gastocnemius_2.Text.Trim(), BotoxData_Gastocnemius_3.Text.Trim(), BotoxData_Gastocnemius_4.Text.Trim(), BotoxData_Tibialis_1.Text.Trim(), BotoxData_Tibialis_2.Text.Trim(),
            //    BotoxData_Tibialis_3.Text.Trim(), BotoxData_Tibialis_4.Text.Trim(), BotoxData_Hamstrings_1.Text.Trim(), BotoxData_Hamstrings_2.Text.Trim(), BotoxData_Hamstrings_3.Text.Trim(), BotoxData_Hamstrings_4.Text.Trim(),
            //    BotoxData_Adductors_1.Text.Trim(), BotoxData_Adductors_2.Text.Trim(), BotoxData_Adductors_3.Text.Trim(), BotoxData_Adductors_4.Text.Trim(), BotoxData_Rectus_1.Text.Trim(), BotoxData_Rectus_2.Text.Trim(),
            //    BotoxData_Rectus_3.Text.Trim(), BotoxData_Rectus_4.Text.Trim(), BotoxData_Iliopsoas_1.Text.Trim(), BotoxData_Iliopsoas_2.Text.Trim(), BotoxData_Iliopsoas_3.Text.Trim(), BotoxData_Iliopsoas_4.Text.Trim(),
            //    BotoxData_Pronator_1.Text.Trim(), BotoxData_Pronator_2.Text.Trim(), BotoxData_Pronator_3.Text.Trim(), BotoxData_Pronator_4.Text.Trim(), BotoxData_FCR_1.Text.Trim(), BotoxData_FCR_2.Text.Trim(), BotoxData_FCR_3.Text.Trim(),
            //    BotoxData_FCR_4.Text.Trim(), BotoxData_FCU_1.Text.Trim(), BotoxData_FCU_2.Text.Trim(), BotoxData_FCU_3.Text.Trim(), BotoxData_FCU_4.Text.Trim(), BotoxData_FDS_1.Text.Trim(), BotoxData_FDS_2.Text.Trim(), BotoxData_FDS_3.Text.Trim(),
            //    BotoxData_FDS_4.Text.Trim(), BotoxData_FDP_1.Text.Trim(), BotoxData_FDP_2.Text.Trim(), BotoxData_FDP_3.Text.Trim(), BotoxData_FDP_4.Text.Trim(), BotoxData_FPL_1.Text.Trim(), BotoxData_FPL_2.Text.Trim(), BotoxData_FPL_3.Text.Trim(),
            //    BotoxData_FPL_4.Text.Trim(), BotoxData_Adductor_1.Text.Trim(), BotoxData_Adductor_2.Text.Trim(), BotoxData_Adductor_3.Text.Trim(), BotoxData_Adductor_4.Text.Trim(), BotoxData_Intrinsics_1.Text.Trim(), BotoxData_Intrinsics_2.Text.Trim(),
            //    BotoxData_Intrinsics_3.Text.Trim(), BotoxData_Intrinsics_4.Text.Trim(), BotoxData_Casting_1.Text.Trim(), BotoxData_Casting_2.Text.Trim(), BotoxData_Casting_3.Text.Trim(), BotoxData_Casting_4.Text.Trim(),
            //    _Doctor_Director, _Doctor_Physiotheraist, _Doctor_Occupational, IsFinal, IsGiven, GivenDate, DateTime.UtcNow.AddMinutes(330), _loginID);
            //#endregion

            //if (i > 0 || j > 0 || k > 0)
            //{
            //    #region SET ALL ATTRIBUTES
            //    RDB.DeleteAttr(_appointmentID, RDB._milestonesTypeID);
            //    foreach (ListItem li in MilestonesType.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._milestonesTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._assistiveDevicesTypeID);
            //    foreach (ListItem li in AssistiveDevices.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._assistiveDevicesTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._orthoticsDevicesTypeID);
            //    foreach (ListItem li in OrthoticsDevices.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._orthoticsDevicesTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._ADLListTypeID);
            //    foreach (ListItem li in ADLList.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._ADLListTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._indicationForBotoxTypeID);
            //    foreach (ListItem li in IndicationForBotox.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._indicationForBotoxTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._ancillaryTreatmentTypeID);
            //    foreach (ListItem li in AncillaryTreatment.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._ancillaryTreatmentTypeID, _attrID);
            //            }
            //        }
            //    }
            //    RDB.DeleteAttr(_appointmentID, RDB._SideEffectsTypeID);
            //    foreach (ListItem li in SideEffects.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            int _attrID = 0; int.TryParse(li.Value, out _attrID);
            //            if (_attrID > 0)
            //            {
            //                RDB.SetAttr(_appointmentID, RDB._SideEffectsTypeID, _attrID);
            //            }
            //        }
            //    }
            //    #endregion

            //    Session[DbHelper.Configuration.messageTextSession] = "Botox report saved successfully...";
            //    Session[DbHelper.Configuration.messageTypeSession] = "1";

            //    //Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            //    Response.Redirect(ResolveClientUrl("~/SessionRpt/Demo_BotoxRpt.aspx?record=" + Request.QueryString["record"]), true);
            //}
            //else
            //{
            //    DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            //}
        }
    }
}