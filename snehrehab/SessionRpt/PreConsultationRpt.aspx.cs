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
    public partial class PreConsultationRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";

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
            _cancelUrl = "/SessionRpt/PreConsultationView.aspx";
            if (_appointmentID > 0)
            {
                if (!IsPostBack)
                {
                    LoadForm();
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            }
            _printUrl = txtPrint.Value;
        }

        private void LoadForm()
        {
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            Doctor_Physioptherapist.Items.Clear(); Doctor_Physioptherapist.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_Occupational.Items.Clear(); Doctor_Occupational.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_EnterReport.Items.Clear(); Doctor_EnterReport.Items.Add(new ListItem("Select Doctor", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                Doctor_Physioptherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_Occupational.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_EnterReport.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }

            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();

            if (!RDB.IsValid(_appointmentID))
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            }
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }
            DataSet ds = RDB.Get(_appointmentID);
            if (ds.Tables.Count > 0)
            {
                bool HasDiagnosisID = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPatient.Text = ds.Tables[0].Rows[0]["FullName"].ToString();
                    txtSession.Text = ds.Tables[0].Rows[0]["SessionName"].ToString();
                    bool.TryParse(ds.Tables[0].Rows[0]["HasDiagnosisID"].ToString(), out HasDiagnosisID);
                }
                if (HasDiagnosisID) { PanelDiagnosis.Visible = true; } else { PanelDiagnosis.Visible = false; }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (HasDiagnosisID)
                    {
                        PanelDiagnosis.Visible = true;
                        string[] DiagnosisIDs = ds.Tables[1].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                        {
                            for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                            {
                                if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                                {
                                    txtDiagnosis.Items[j].Selected = true; break;
                                }
                            }
                        }
                        txtDiagnosisOther.Text = ds.Tables[1].Rows[0]["DiagnosisOther"].ToString();
                    }
                    else
                    {
                        PanelDiagnosis.Visible = false;
                    }
                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=7&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
                    His_FamilyHistory_1.Checked = false; His_FamilyHistory_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_FamilyHistory"].ToString().Equals(His_FamilyHistory_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyHistory_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["His_FamilyHistory"].ToString().Equals(His_FamilyHistory_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyHistory_2.Checked = true;
                    }
                    His_FamilyStru_1.Checked = false; His_FamilyStru_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_FamilyStru"].ToString().Equals(His_FamilyStru_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyStru_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["His_FamilyStru"].ToString().Equals(His_FamilyStru_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyStru_2.Checked = true;
                    }
                    His_InterParental.Text = ds.Tables[1].Rows[0]["His_InterParental"].ToString();
                    His_ParentalChild.Text = ds.Tables[1].Rows[0]["His_ParentalChild"].ToString();
                    His_EmotionalAbus_1.Checked = false; His_EmotionalAbus_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_EmotionalAbus"].ToString().Equals(His_EmotionalAbus_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_EmotionalAbus_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["His_EmotionalAbus"].ToString().Equals(His_EmotionalAbus_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_EmotionalAbus_2.Checked = true;
                    }
                    His_FamilyRelocation_1.Checked = false; His_FamilyRelocation_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_FamilyRelocation"].ToString().Equals(His_FamilyRelocation_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyRelocation_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["His_FamilyRelocation"].ToString().Equals(His_FamilyRelocation_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_FamilyRelocation_2.Checked = true;
                    }
                    His_PrimaryCareGiver.Text = ds.Tables[1].Rows[0]["His_PrimaryCareGiver"].ToString();
                    His_MaternalHistory_1.Checked = false; His_MaternalHistory_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString().Equals(His_MaternalHistory_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_MaternalHistory_1.Checked = true;
                    }
                    else
                    {
                        if (ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString().Trim().Length > 0)
                        {
                            His_MaternalHistory_2.Checked = true;
                            His_MaternalHistory_3.Text = ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString();
                        }
                    }
                    His_AnyHistoryOf_1.Checked = false; His_AnyHistoryOf_2.Checked = false; His_AnyHistoryOf_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Equals(His_AnyHistoryOf_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        His_AnyHistoryOf_1.Checked = true;
                    }
                    else
                    {
                        if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Equals(His_AnyHistoryOf_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            His_AnyHistoryOf_2.Checked = true;
                        }
                        else
                        {
                            if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Trim().Length > 0)
                            {
                                His_AnyHistoryOf_3.Checked = true;
                                His_AnyHistoryOf_4.Text = ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString();
                            }
                        }
                    }
                    PreNatal_AnyComplication_1.Checked = false; PreNatal_AnyComplication_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["PreNatal_AnyComplication"].ToString().Equals(PreNatal_AnyComplication_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PreNatal_AnyComplication_1.Checked = true;
                        PreNatal_Complications.Text = ds.Tables[1].Rows[0]["PreNatal_Complications"].ToString();
                    }
                    if (ds.Tables[1].Rows[0]["PreNatal_AnyComplication"].ToString().Equals(PreNatal_AnyComplication_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PreNatal_AnyComplication_2.Checked = true;
                    }
                    BirthHis_Terms_1.Checked = false; BirthHis_Terms_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BirthHis_Terms"].ToString().Equals(BirthHis_Terms_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_Terms_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BirthHis_Terms"].ToString().Equals(BirthHis_Terms_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_Terms_2.Checked = true;
                    }
                    BirthHis_TermWeek.Text = ds.Tables[1].Rows[0]["BirthHis_TermWeek"].ToString();
                    BirthHis_Delivery_1.Checked = false; BirthHis_Delivery_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BirthHis_Delivery"].ToString().Equals(BirthHis_Delivery_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_Delivery_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BirthHis_Delivery"].ToString().Equals(BirthHis_Delivery_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_Delivery_2.Checked = true;
                    }
                    BirthHis_LabourTotal.Text = ds.Tables[1].Rows[0]["BirthHis_LabourTotal"].ToString();
                    BirthHis_LabourDiff_1.Checked = false; BirthHis_LabourDiff_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BirthHis_LabourDiff"].ToString().Equals(BirthHis_LabourDiff_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_LabourDiff_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BirthHis_LabourDiff"].ToString().Equals(BirthHis_LabourDiff_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BirthHis_LabourDiff_2.Checked = true;
                    }
                    BirthHis_LabourProb.Text = ds.Tables[1].Rows[0]["BirthHis_LabourProb"].ToString();
                    BirthHis_Aneshthesia.Text = ds.Tables[1].Rows[0]["BirthHis_Aneshthesia"].ToString();
                    Other_CIAB_1.Checked = false; Other_CIAB_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Other_CIAB"].ToString().Equals(Other_CIAB_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Other_CIAB_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Other_CIAB"].ToString().Equals(Other_CIAB_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Other_CIAB_2.Checked = true;
                    }
                    Other_BirthWeight.Text = ds.Tables[1].Rows[0]["Other_BirthWeight"].ToString();
                    Other_SGA_AGA_1.Checked = false; Other_SGA_AGA_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Other_SGA_AGA"].ToString().Equals(Other_SGA_AGA_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Other_SGA_AGA_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Other_SGA_AGA"].ToString().Equals(Other_SGA_AGA_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Other_SGA_AGA_2.Checked = true;
                    }
                    Other_APGAR_Score.Text = ds.Tables[1].Rows[0]["Other_APGAR_Score"].ToString();
                    NICU_1.Checked = false; NICU_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["NICU"].ToString().Equals(NICU_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        NICU_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["NICU"].ToString().Equals(NICU_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        NICU_2.Checked = true;
                    }
                    NICU_Duration.Text = ds.Tables[1].Rows[0]["NICU_Duration"].ToString();
                    NICU_Reason.Text = ds.Tables[1].Rows[0]["NICU_Reason"].ToString();
                    DischargedOnWhichDay.Text = ds.Tables[1].Rows[0]["DischargedOnWhichDay"].ToString();
                    ChildTakingMotherFeeds.Text = ds.Tables[1].Rows[0]["ChildTakingMotherFeeds"].ToString();
                    AnyOtherRelevantMedicalHistory.Text = ds.Tables[1].Rows[0]["AnyOtherRelevantMedicalHistory"].ToString();
                    MedicalTimeLine.Text = ds.Tables[1].Rows[0]["MedicalTimeLine"].ToString();
                    HowWasBabyAtHome_1.Checked = false; HowWasBabyAtHome_2.Checked = false; HowWasBabyAtHome_3.Checked = false;
                    HowWasBabyAtHome_4.Checked = false; HowWasBabyAtHome_5.Checked = false; HowWasBabyAtHome_6.Checked = false;
                    HowWasBabyAtHome_7.Checked = false;
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_4.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_5.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_5.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_6.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_6.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Equals(HowWasBabyAtHome_7.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        HowWasBabyAtHome_7.Checked = true;
                    }
                    WasHeFeedingWell_1.Checked = false; WasHeFeedingWell_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["WasHeFeedingWell"].ToString().Equals(WasHeFeedingWell_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        WasHeFeedingWell_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["WasHeFeedingWell"].ToString().Equals(WasHeFeedingWell_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        WasHeFeedingWell_2.Checked = true;
                    }
                    WasHeSleepingWell_1.Checked = false; WasHeSleepingWell_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["WasHeSleepingWell"].ToString().Equals(WasHeSleepingWell_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        WasHeSleepingWell_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["WasHeSleepingWell"].ToString().Equals(WasHeSleepingWell_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        WasHeSleepingWell_2.Checked = true;
                    }
                    AnyDelay_MedicalEvent_Symptoms.Text = ds.Tables[1].Rows[0]["AnyDelay_MedicalEvent_Symptoms"].ToString();
                    WhoWasTheFirstNotice.Text = ds.Tables[1].Rows[0]["WhoWasTheFirstNotice"].ToString();
                    WhatWasDoneForTheSame.Text = ds.Tables[1].Rows[0]["WhatWasDoneForTheSame"].ToString();
                    ChildStartedToHeadHold.Text = ds.Tables[1].Rows[0]["ChildStartedToHeadHold"].ToString();
                    WasItOnTimeOrDelayed.Text = ds.Tables[1].Rows[0]["WasItOnTimeOrDelayed"].ToString();
                    CloselyInvolvedWithChild.Text = ds.Tables[1].Rows[0]["CloselyInvolvedWithChild"].ToString();
                    ChildChooseToUseFreeTime.Text = ds.Tables[1].Rows[0]["ChildChooseToUseFreeTime"].ToString();
                    ObservationsDuringFreePlay.Text = ds.Tables[1].Rows[0]["ObservationsDuringFreePlay"].ToString();
                    EvaluationNeeded_1.Checked = false; EvaluationNeeded_2.Checked = false; EvaluationNeeded_3.Checked = false;
                    EvaluationNeeded_4.Checked = false; EvaluationNeeded_5.Checked = false; EvaluationNeeded_6.Checked = false;
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_4.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_5.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_5.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Equals(EvaluationNeeded_6.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        EvaluationNeeded_6.Checked = true;
                    }
                    Cardiologist_Name.Text = ds.Tables[1].Rows[0]["Cardiologist_Name"].ToString();
                    Cardiologist_Date.Text = ds.Tables[1].Rows[0]["Cardiologist_Date"].ToString();
                    Cardiologist_Addr.Text = ds.Tables[1].Rows[0]["Cardiologist_Addr"].ToString();
                    Cardiologist_Phone.Text = ds.Tables[1].Rows[0]["Cardiologist_Phone"].ToString();
                    Orthopedist_Name.Text = ds.Tables[1].Rows[0]["Orthopedist_Name"].ToString();
                    Orthopedist_Date.Text = ds.Tables[1].Rows[0]["Orthopedist_Date"].ToString();
                    Orthopedist_Addr.Text = ds.Tables[1].Rows[0]["Orthopedist_Addr"].ToString();
                    Orthopedist_Phone.Text = ds.Tables[1].Rows[0]["Orthopedist_Phone"].ToString();
                    Psychologist_Name.Text = ds.Tables[1].Rows[0]["Psychologist_Name"].ToString();
                    Psychologist_Date.Text = ds.Tables[1].Rows[0]["Psychologist_Date"].ToString();
                    Psychologist_Addr.Text = ds.Tables[1].Rows[0]["Psychologist_Addr"].ToString();
                    Psychologist_Phone.Text = ds.Tables[1].Rows[0]["Psychologist_Phone"].ToString();
                    Psychiatrist_Name.Text = ds.Tables[1].Rows[0]["Psychiatrist_Name"].ToString();
                    Psychiatrist_Date.Text = ds.Tables[1].Rows[0]["Psychiatrist_Date"].ToString();
                    Psychiatrist_Addr.Text = ds.Tables[1].Rows[0]["Psychiatrist_Addr"].ToString();
                    Psychiatrist_Phone.Text = ds.Tables[1].Rows[0]["Psychiatrist_Phone"].ToString();
                    Opthalmologist_Name.Text = ds.Tables[1].Rows[0]["Opthalmologist_Name"].ToString();
                    Opthalmologist_Date.Text = ds.Tables[1].Rows[0]["Opthalmologist_Date"].ToString();
                    Opthalmologist_Addr.Text = ds.Tables[1].Rows[0]["Opthalmologist_Addr"].ToString();
                    Opthalmologist_Phone.Text = ds.Tables[1].Rows[0]["Opthalmologist_Phone"].ToString();
                    Speech_Name.Text = ds.Tables[1].Rows[0]["Speech_Name"].ToString();
                    Speech_Date.Text = ds.Tables[1].Rows[0]["Speech_Date"].ToString();
                    Speech_Addr.Text = ds.Tables[1].Rows[0]["Speech_Addr"].ToString();
                    Speech_Phone.Text = ds.Tables[1].Rows[0]["Speech_Phone"].ToString();
                    Pathologist_Name.Text = ds.Tables[1].Rows[0]["Pathologist_Name"].ToString();
                    Pathologist_Date.Text = ds.Tables[1].Rows[0]["Pathologist_Date"].ToString();
                    Pathologist_Addr.Text = ds.Tables[1].Rows[0]["Pathologist_Addr"].ToString();
                    Pathologist_Phone.Text = ds.Tables[1].Rows[0]["Pathologist_Phone"].ToString();
                    Occupational_Name.Text = ds.Tables[1].Rows[0]["Occupational_Name"].ToString();
                    Occupational_Date.Text = ds.Tables[1].Rows[0]["Occupational_Date"].ToString();
                    Occupational_Addr.Text = ds.Tables[1].Rows[0]["Occupational_Addr"].ToString();
                    Occupational_Phone.Text = ds.Tables[1].Rows[0]["Occupational_Phone"].ToString();
                    Physical_Name.Text = ds.Tables[1].Rows[0]["Physical_Name"].ToString();
                    Physical_Date.Text = ds.Tables[1].Rows[0]["Physical_Date"].ToString();
                    Physical_Addr.Text = ds.Tables[1].Rows[0]["Physical_Addr"].ToString();
                    Physical_Phone.Text = ds.Tables[1].Rows[0]["Physical_Phone"].ToString();
                    Audiologist_Name.Text = ds.Tables[1].Rows[0]["Audiologist_Name"].ToString();
                    Audiologist_Date.Text = ds.Tables[1].Rows[0]["Audiologist_Date"].ToString();
                    Audiologist_Addr.Text = ds.Tables[1].Rows[0]["Audiologist_Addr"].ToString();
                    Audiologist_Phone.Text = ds.Tables[1].Rows[0]["Audiologist_Phone"].ToString();
                    ENT_Name.Text = ds.Tables[1].Rows[0]["ENT_Name"].ToString();
                    ENT_Date.Text = ds.Tables[1].Rows[0]["ENT_Date"].ToString();
                    ENT_Addr.Text = ds.Tables[1].Rows[0]["ENT_Addr"].ToString();
                    ENT_Phone.Text = ds.Tables[1].Rows[0]["ENT_Phone"].ToString();
                    Chiropractor_Name.Text = ds.Tables[1].Rows[0]["Chiropractor_Name"].ToString();
                    Chiropractor_Date.Text = ds.Tables[1].Rows[0]["Chiropractor_Date"].ToString();
                    Chiropractor_Addr.Text = ds.Tables[1].Rows[0]["Chiropractor_Addr"].ToString();
                    Chiropractor_Phone.Text = ds.Tables[1].Rows[0]["Chiropractor_Phone"].ToString();
                    bool IsFinal = false; bool.TryParse(ds.Tables[1].Rows[0]["IsFinal"].ToString(), out IsFinal);
                    txtFinal.Checked = IsFinal;
                    bool IsGiven = false; bool.TryParse(ds.Tables[1].Rows[0]["IsGiven"].ToString(), out IsGiven);
                    txtGiven.Checked = IsGiven;
                    DateTime _givenDate = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["GivenDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _givenDate);
                    if (_givenDate > DateTime.MinValue)
                    {
                        txtGivenDate.Text = _givenDate.ToString(DbHelper.Configuration.showDateFormat);
                    }
                    int Physioptherapist = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Physioptherapist"].ToString(), out Physioptherapist);
                    if (Doctor_Physioptherapist.Items.FindByValue(Physioptherapist.ToString()) != null)
                    {
                        Doctor_Physioptherapist.SelectedValue = Physioptherapist.ToString();
                    }
                    int Occupational = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Occupational"].ToString(), out Occupational);
                    if (Doctor_Occupational.Items.FindByValue(Occupational.ToString()) != null)
                    {
                        Doctor_Occupational.SelectedValue = Occupational.ToString();
                    }
                    int EnterReport = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_EnterReport"].ToString(), out EnterReport);
                    if (Doctor_EnterReport.Items.FindByValue(EnterReport.ToString()) != null)
                    {
                        Doctor_EnterReport.SelectedValue = EnterReport.ToString();
                    }
                    Other_Name.Text = ds.Tables[1].Rows[0]["Other_Name"].ToString();
                    Other_Date.Text = ds.Tables[1].Rows[0]["Other_Date"].ToString();
                    Other_Addr.Text = ds.Tables[1].Rows[0]["Other_Addr"].ToString();
                    Other_Phone.Text = ds.Tables[1].Rows[0]["Other_Phone"].ToString();
                    ReleventMedicalTimeline.Text = ds.Tables[1].Rows[0]["ReleventMedicalTimeline"].ToString();
                    DailyRoutine.Text = ds.Tables[1].Rows[0]["DailyRoutine"].ToString();
                    /*********************************************************************/
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            bool IsFinal = txtFinal.Checked;
            bool IsGiven = txtGiven.Checked;
            DateTime GivenDate = new DateTime();
            if (IsGiven)
            {
                if (txtGivenDate.Text.Trim().Length > 0)
                {
                    DateTime.TryParseExact(txtGivenDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                }
            }
            string DiagnosisIDs = "";
            for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            {
                if (txtDiagnosis.Items[k].Selected)
                {
                    DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
                }
            }
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            DIB.setFromOther(DiagnosisOther);

            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }
            int EnterReport = 0; if (Doctor_EnterReport.SelectedItem != null) { int.TryParse(Doctor_EnterReport.SelectedItem.Value, out EnterReport); }
            string His_FamilyHistory = string.Empty;
            if (His_FamilyHistory_1.Checked) { His_FamilyHistory = His_FamilyHistory_1.Text.Trim(); }
            if (His_FamilyHistory_2.Checked) { His_FamilyHistory = His_FamilyHistory_2.Text.Trim(); }
            string His_FamilyStru = string.Empty;
            if (His_FamilyStru_1.Checked) { His_FamilyStru = His_FamilyStru_1.Text.Trim(); }
            if (His_FamilyStru_2.Checked) { His_FamilyStru = His_FamilyStru_2.Text.Trim(); }
            string His_EmotionalAbus = string.Empty;
            if (His_EmotionalAbus_1.Checked) { His_EmotionalAbus = His_EmotionalAbus_1.Text.Trim(); }
            if (His_EmotionalAbus_2.Checked) { His_EmotionalAbus = His_EmotionalAbus_2.Text.Trim(); }
            string His_FamilyRelocation = string.Empty;
            if (His_FamilyRelocation_1.Checked) { His_FamilyRelocation = His_FamilyRelocation_1.Text.Trim(); }
            if (His_FamilyRelocation_2.Checked) { His_FamilyRelocation = His_FamilyRelocation_2.Text.Trim(); }
            string His_MaternalHistory = string.Empty;
            if (His_MaternalHistory_1.Checked) { His_MaternalHistory = His_MaternalHistory_1.Text.Trim(); }
            if (His_MaternalHistory_2.Checked) { His_MaternalHistory = His_MaternalHistory_3.Text.Trim(); }
            string His_AnyHistoryOf = string.Empty;
            if (His_AnyHistoryOf_1.Checked) { His_AnyHistoryOf = His_AnyHistoryOf_1.Text.Trim(); }
            if (His_AnyHistoryOf_2.Checked) { His_AnyHistoryOf = His_AnyHistoryOf_2.Text.Trim(); }
            if (His_AnyHistoryOf_3.Checked) { His_AnyHistoryOf = His_AnyHistoryOf_4.Text.Trim(); }
            string PreNatal_AnyComplication = string.Empty;
            if (PreNatal_AnyComplication_1.Checked) { PreNatal_AnyComplication = PreNatal_AnyComplication_1.Text.Trim(); }
            if (PreNatal_AnyComplication_2.Checked) { PreNatal_AnyComplication = PreNatal_AnyComplication_2.Text.Trim(); }
            string BirthHis_Terms = string.Empty;
            if (BirthHis_Terms_1.Checked) { BirthHis_Terms = BirthHis_Terms_1.Text.Trim(); }
            if (BirthHis_Terms_2.Checked) { BirthHis_Terms = BirthHis_Terms_2.Text.Trim(); }
            string BirthHis_Delivery = string.Empty;
            if (BirthHis_Delivery_1.Checked) { BirthHis_Delivery = BirthHis_Delivery_1.Text.Trim(); }
            if (BirthHis_Delivery_2.Checked) { BirthHis_Delivery = BirthHis_Delivery_2.Text.Trim(); }
            string BirthHis_LabourDiff = string.Empty;
            if (BirthHis_LabourDiff_1.Checked) { BirthHis_LabourDiff = BirthHis_LabourDiff_1.Text.Trim(); }
            if (BirthHis_LabourDiff_2.Checked) { BirthHis_LabourDiff = BirthHis_LabourDiff_2.Text.Trim(); }
            string Other_CIAB = string.Empty;
            if (Other_CIAB_1.Checked) { Other_CIAB = Other_CIAB_1.Text.Trim(); }
            if (Other_CIAB_2.Checked) { Other_CIAB = Other_CIAB_2.Text.Trim(); }
            string Other_SGA_AGA = string.Empty;
            if (Other_SGA_AGA_1.Checked) { Other_SGA_AGA = Other_SGA_AGA_1.Text.Trim(); }
            if (Other_SGA_AGA_2.Checked) { Other_SGA_AGA = Other_SGA_AGA_2.Text.Trim(); }
            string Surgical_History = string.Empty;
            string NICU = string.Empty;
            if (NICU_1.Checked) { NICU = NICU_1.Text.Trim(); }
            if (NICU_2.Checked) { NICU = NICU_2.Text.Trim(); }
            string PostDischarge = string.Empty;
            string HowWasBabyAtHome = string.Empty;
            if (HowWasBabyAtHome_1.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_1.Text.Trim(); }
            if (HowWasBabyAtHome_2.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_2.Text.Trim(); }
            if (HowWasBabyAtHome_3.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_3.Text.Trim(); }
            if (HowWasBabyAtHome_4.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_4.Text.Trim(); }
            if (HowWasBabyAtHome_5.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_5.Text.Trim(); }
            if (HowWasBabyAtHome_6.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_6.Text.Trim(); }
            if (HowWasBabyAtHome_7.Checked) { HowWasBabyAtHome = HowWasBabyAtHome_7.Text.Trim(); }
            string WasHeFeedingWell = string.Empty;
            if (WasHeFeedingWell_1.Checked) { WasHeFeedingWell = WasHeFeedingWell_1.Text.Trim(); }
            if (WasHeFeedingWell_2.Checked) { WasHeFeedingWell = WasHeFeedingWell_2.Text.Trim(); }
            string WasHeSleepingWell = string.Empty;
            if (WasHeSleepingWell_1.Checked) { WasHeSleepingWell = WasHeSleepingWell_1.Text.Trim(); }
            if (WasHeSleepingWell_2.Checked) { WasHeSleepingWell = WasHeSleepingWell_2.Text.Trim(); }
            string EvaluationNeeded = string.Empty;
            if (EvaluationNeeded_1.Checked) { EvaluationNeeded = EvaluationNeeded_1.Text.Trim(); }
            if (EvaluationNeeded_2.Checked) { EvaluationNeeded = EvaluationNeeded_2.Text.Trim(); }
            if (EvaluationNeeded_3.Checked) { EvaluationNeeded = EvaluationNeeded_3.Text.Trim(); }
            if (EvaluationNeeded_4.Checked) { EvaluationNeeded = EvaluationNeeded_4.Text.Trim(); }
            if (EvaluationNeeded_5.Checked) { EvaluationNeeded = EvaluationNeeded_5.Text.Trim(); }
            if (EvaluationNeeded_6.Checked) { EvaluationNeeded = EvaluationNeeded_6.Text.Trim(); }

            int i = RDB.Set(_appointmentID, His_FamilyHistory, His_FamilyStru, His_InterParental.Text.Trim(), His_ParentalChild.Text.Trim(), His_EmotionalAbus, His_FamilyRelocation, His_PrimaryCareGiver.Text.Trim(),
                His_MaternalHistory, His_AnyHistoryOf, PreNatal_AnyComplication, PreNatal_Complications.Text.Trim(), BirthHis_Terms, BirthHis_TermWeek.Text.Trim(), BirthHis_Delivery,
                BirthHis_LabourTotal.Text.Trim(), BirthHis_LabourDiff, BirthHis_LabourProb.Text.Trim(), BirthHis_Aneshthesia.Text.Trim(), Other_CIAB, Other_BirthWeight.Text.Trim(), Other_SGA_AGA, Other_APGAR_Score.Text.Trim(),
                Surgical_History, NICU, NICU_Duration.Text.Trim(), NICU_Reason.Text.Trim(), DischargedOnWhichDay.Text.Trim(), ChildTakingMotherFeeds.Text.Trim(), AnyOtherRelevantMedicalHistory.Text.Trim(), MedicalTimeLine.Text.Trim(),
                PostDischarge, HowWasBabyAtHome, WasHeFeedingWell, WasHeSleepingWell, AnyDelay_MedicalEvent_Symptoms.Text.Trim(), WhoWasTheFirstNotice.Text.Trim(),
                WhatWasDoneForTheSame.Text.Trim(), ChildStartedToHeadHold.Text.Trim(), WasItOnTimeOrDelayed.Text.Trim(), CloselyInvolvedWithChild.Text.Trim(), ChildChooseToUseFreeTime.Text.Trim(), ObservationsDuringFreePlay.Text.Trim(),
                (Brushing_Dependant.Checked ? "Yes" : ""), (Brushing_Independant.Checked ? "Yes" : ""), (Brushing_Assisted.Checked ? "Yes" : ""), (Toileting_Dependant.Checked ? "Yes" : ""), (Toileting_Independant.Checked ? "Yes" : ""),
                (Toileting_Assisted.Checked ? "Yes" : ""), (Bathing_Dependant.Checked ? "Yes" : ""), (Bathing_Independant.Checked ? "Yes" : ""), (Bathing_Assisted.Checked ? "Yes" : ""), (Dressing_Dependant.Checked ? "Yes" : ""),
                (Dressing_Independant.Checked ? "Yes" : ""), (Dressing_Assisted.Checked ? "Yes" : ""), (Feeding_Dependant.Checked ? "Yes" : ""), (Feeding_Independant.Checked ? "Yes" : ""), (Feeding_Assisted.Checked ? "Yes" : ""),
                (Ambulation_Dependant.Checked ? "Yes" : ""), (Ambulation_Independant.Checked ? "Yes" : ""), (Ambulation_Assisted.Checked ? "Yes" : ""), (Transfer_Dependant.Checked ? "Yes" : ""), (Transfer_Independant.Checked ? "Yes" : ""),
                (Transfer_Assisted.Checked ? "Yes" : ""), Summary.Text.Trim(), EvaluationNeeded, Cardiologist_Name.Text.Trim(), Cardiologist_Date.Text.Trim(), Cardiologist_Addr.Text.Trim(), Cardiologist_Phone.Text.Trim(),
                Orthopedist_Name.Text.Trim(), Orthopedist_Date.Text.Trim(), Orthopedist_Addr.Text.Trim(), Orthopedist_Phone.Text.Trim(), Psychologist_Name.Text.Trim(), Psychologist_Date.Text.Trim(), Psychologist_Addr.Text.Trim(),
                Psychologist_Phone.Text.Trim(), Psychiatrist_Name.Text.Trim(), Psychiatrist_Date.Text.Trim(), Psychiatrist_Addr.Text.Trim(), Psychiatrist_Phone.Text.Trim(), Opthalmologist_Name.Text.Trim(), Opthalmologist_Date.Text.Trim(),
                Opthalmologist_Addr.Text.Trim(), Opthalmologist_Phone.Text.Trim(), Speech_Name.Text.Trim(), Speech_Date.Text.Trim(), Speech_Addr.Text.Trim(), Speech_Phone.Text.Trim(), Pathologist_Name.Text.Trim(), Pathologist_Date.Text.Trim(),
                Pathologist_Addr.Text.Trim(), Pathologist_Phone.Text.Trim(), Occupational_Name.Text.Trim(), Occupational_Date.Text.Trim(), Occupational_Addr.Text.Trim(), Occupational_Phone.Text.Trim(), Physical_Name.Text.Trim(),
                Physical_Date.Text.Trim(), Physical_Addr.Text.Trim(), Physical_Phone.Text.Trim(), Audiologist_Name.Text.Trim(), Audiologist_Date.Text.Trim(), Audiologist_Addr.Text.Trim(), Audiologist_Phone.Text.Trim(), ENT_Name.Text.Trim(),
                ENT_Date.Text.Trim(), ENT_Addr.Text.Trim(), ENT_Phone.Text.Trim(), Chiropractor_Name.Text.Trim(), Chiropractor_Date.Text.Trim(), Chiropractor_Addr.Text.Trim(), Chiropractor_Phone.Text.Trim(), Physioptherapist,
                Occupational, EnterReport, IsFinal, IsGiven, GivenDate, DateTime.UtcNow.AddMinutes(330), _loginID,
                Other_Name.Text.Trim(), Other_Date.Text.Trim(), Other_Addr.Text.Trim(), Other_Phone.Text.Trim(), ReleventMedicalTimeline.Text.Trim(), DailyRoutine.Text.Trim(),
                DiagnosisIDs, DiagnosisOther);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Pre screening report saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                //Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
                Response.Redirect(ResolveClientUrl("~/SessionRpt/PreConsultationRpt.aspx?record=" + Request.QueryString["record"]), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}