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
    public partial class SiRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        int PatientID = 0;
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
            _cancelUrl = "/SessionRpt/SiView.aspx";
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

            SnehBLL.ReportSiMst_Bll RDB = new SnehBLL.ReportSiMst_Bll();

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

                    string[] DiagnosisIDs = ds.Tables[0].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
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
                    txtDiagnosisOther.Text = ds.Tables[0].Rows[0]["DiagnosisOther"].ToString();
                }
                if (HasDiagnosisID) { PanelDiagnosis.Visible = true; } else { PanelDiagnosis.Visible = false; }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (HasDiagnosisID)
                    {
                        PanelDiagnosis.Visible = true;
                        //string[] DiagnosisIDs = ds.Tables[1].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        //for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                        //{
                        //    for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                        //    {
                        //        if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                        //        {
                        //            txtDiagnosis.Items[j].Selected = true; break;
                        //        }
                        //    }
                        //}
                        //txtDiagnosisOther.Text = ds.Tables[1].Rows[0]["DiagnosisOther"].ToString();
                    }
                    else
                    {
                        PanelDiagnosis.Visible = false;
                    }

                    SensoryName1.Text = ds.Tables[1].Rows[0]["SensoryName1"].ToString();
                    Result1.Text = ds.Tables[1].Rows[0]["Result1"].ToString();
                    SensoryName2.Text = ds.Tables[1].Rows[0]["SensoryName2"].ToString();
                    Result2.Text = ds.Tables[1].Rows[0]["Result2"].ToString();
                    SensoryName3.Text = ds.Tables[1].Rows[0]["SensoryName3"].ToString();
                    Result3.Text = ds.Tables[1].Rows[0]["Result3"].ToString();
                    SensoryName4.Text = ds.Tables[1].Rows[0]["SensoryName4"].ToString();
                    Result4.Text = ds.Tables[1].Rows[0]["Result4"].ToString();
                    SensoryName5.Text = ds.Tables[1].Rows[0]["SensoryName5"].ToString();
                    Result5.Text = ds.Tables[1].Rows[0]["Result5"].ToString();
                    SensoryName6.Text = ds.Tables[1].Rows[0]["SensoryName6"].ToString();
                    Result6.Text = ds.Tables[1].Rows[0]["Result6"].ToString();
                    SensoryName7.Text = ds.Tables[1].Rows[0]["SensoryName7"].ToString();
                    Result7.Text = ds.Tables[1].Rows[0]["Result7"].ToString();
                    SensoryName8.Text = ds.Tables[1].Rows[0]["SensoryName8"].ToString();
                    Result8.Text = ds.Tables[1].Rows[0]["Result8"].ToString();
                    SensoryName9.Text = ds.Tables[1].Rows[0]["SensoryName9"].ToString();
                    Result9.Text = ds.Tables[1].Rows[0]["Result9"].ToString();
                    SensoryName10.Text = ds.Tables[1].Rows[0]["SensoryName10"].ToString();
                    Result10.Text = ds.Tables[1].Rows[0]["Result10"].ToString();
                    SensoryName11.Text = ds.Tables[1].Rows[0]["SensoryName11"].ToString();
                    Result11.Text = ds.Tables[1].Rows[0]["Result11"].ToString();
                    SensoryName12.Text = ds.Tables[1].Rows[0]["SensoryName12"].ToString();
                    Result12.Text = ds.Tables[1].Rows[0]["Result12"].ToString();
                    SensoryName13.Text = ds.Tables[1].Rows[0]["SensoryName13"].ToString();
                    Result13.Text = ds.Tables[1].Rows[0]["Result13"].ToString();
                    SensoryName14.Text = ds.Tables[1].Rows[0]["SensoryName14"].ToString();
                    Result14.Text = ds.Tables[1].Rows[0]["Result14"].ToString();
                    SensoryName15.Text = ds.Tables[1].Rows[0]["SensoryName15"].ToString();
                    Result15.Text = ds.Tables[1].Rows[0]["Result15"].ToString();
                    SensoryName16.Text = ds.Tables[1].Rows[0]["SensoryName16"].ToString();
                    Result16.Text = ds.Tables[1].Rows[0]["Result16"].ToString();
                    SensoryName17.Text = ds.Tables[1].Rows[0]["SensoryName17"].ToString();
                    Result17.Text = ds.Tables[1].Rows[0]["Result17"].ToString();
                    SensoryName18.Text = ds.Tables[1].Rows[0]["SensoryName18"].ToString();
                    Result18.Text = ds.Tables[1].Rows[0]["Result18"].ToString();
                    SensoryName19.Text = ds.Tables[1].Rows[0]["SensoryName19"].ToString();
                    Result19.Text = ds.Tables[1].Rows[0]["Result19"].ToString();
                    SensoryName20.Text = ds.Tables[1].Rows[0]["SensoryName20"].ToString();
                    Result20.Text = ds.Tables[1].Rows[0]["Result20"].ToString();
                    SensoryName21.Text = ds.Tables[1].Rows[0]["SensoryName21"].ToString();
                    Result21.Text = ds.Tables[1].Rows[0]["Result21"].ToString();
                    SensoryName22.Text = ds.Tables[1].Rows[0]["SensoryName22"].ToString();
                    Result22.Text = ds.Tables[1].Rows[0]["Result22"].ToString();
                    SensoryName23.Text = ds.Tables[1].Rows[0]["SensoryName23"].ToString();
                    Result23.Text = ds.Tables[1].Rows[0]["Result23"].ToString();
                    SensoryName24.Text = ds.Tables[1].Rows[0]["SensoryName24"].ToString();
                    Result24.Text = ds.Tables[1].Rows[0]["Result24"].ToString();
                    SensoryName25.Text = ds.Tables[1].Rows[0]["SensoryName25"].ToString();
                    Result25.Text = ds.Tables[1].Rows[0]["Result25"].ToString();
                    SensoryName26.Text = ds.Tables[1].Rows[0]["SensoryName26"].ToString();
                    Result26.Text = ds.Tables[1].Rows[0]["Result26"].ToString();
                    SensoryName27.Text = ds.Tables[1].Rows[0]["SensoryName27"].ToString();
                    Result27.Text = ds.Tables[1].Rows[0]["Result27"].ToString();
                    SensoryName28.Text = ds.Tables[1].Rows[0]["SensoryName28"].ToString();
                    Result28.Text = ds.Tables[1].Rows[0]["Result28"].ToString();
                    SensoryName29.Text = ds.Tables[1].Rows[0]["SensoryName29"].ToString();
                    Result29.Text = ds.Tables[1].Rows[0]["Result29"].ToString();
                    SensoryName30.Text = ds.Tables[1].Rows[0]["SensoryName30"].ToString();
                    Result30.Text = ds.Tables[1].Rows[0]["Result30"].ToString();
                    SensoryName31.Text = ds.Tables[1].Rows[0]["SensoryName31"].ToString();
                    Result31.Text = ds.Tables[1].Rows[0]["Result31"].ToString();
                    SensoryName32.Text = ds.Tables[1].Rows[0]["SensoryName32"].ToString();
                    Result32.Text = ds.Tables[1].Rows[0]["Result32"].ToString();
                    SensoryName33.Text = ds.Tables[1].Rows[0]["SensoryName33"].ToString();
                    Result33.Text = ds.Tables[1].Rows[0]["Result33"].ToString();
                    SensoryName34.Text = ds.Tables[1].Rows[0]["SensoryName34"].ToString();
                    Result34.Text = ds.Tables[1].Rows[0]["Result34"].ToString();
                    SensoryName35.Text = ds.Tables[1].Rows[0]["SensoryName35"].ToString();
                    Result35.Text = ds.Tables[1].Rows[0]["Result35"].ToString();
                    SensoryName36.Text = ds.Tables[1].Rows[0]["SensoryName36"].ToString();
                    Result36.Text = ds.Tables[1].Rows[0]["Result36"].ToString();
                    SensoryName37.Text = ds.Tables[1].Rows[0]["SensoryName37"].ToString();
                    Result37.Text = ds.Tables[1].Rows[0]["Result37"].ToString();
                    SensoryName38.Text = ds.Tables[1].Rows[0]["SensoryName38"].ToString();
                    Result38.Text = ds.Tables[1].Rows[0]["Result38"].ToString();
                    SensoryName39.Text = ds.Tables[1].Rows[0]["SensoryName39"].ToString();
                    Result39.Text = ds.Tables[1].Rows[0]["Result39"].ToString();
                    SensoryName40.Text = ds.Tables[1].Rows[0]["SensoryName40"].ToString();
                    Result40.Text = ds.Tables[1].Rows[0]["Result40"].ToString();


                    Praxistest.Text = ds.Tables[1].Rows[0]["Praxistest"].ToString();
                    Designcopying.Text = ds.Tables[1].Rows[0]["Designcopying"].ToString();
                    ConstructionalPraxis.Text = ds.Tables[1].Rows[0]["ConstructionalPraxis"].ToString();
                    Oralpraxis.Text = ds.Tables[1].Rows[0]["Oralpraxis"].ToString();
                    Posturalpraxis.Text = ds.Tables[1].Rows[0]["Posturalpraxis"].ToString();
                    Praxisonverbalcommands.Text = ds.Tables[1].Rows[0]["Praxisonverbalcommands"].ToString();
                    Sequencingpraxis.Text = ds.Tables[1].Rows[0]["Sequencingpraxis"].ToString();
                    Sensoryintegrationtests.Text = ds.Tables[1].Rows[0]["Sensoryintegrationtests"].ToString();
                    Bilateralmotorcoordination.Text = ds.Tables[1].Rows[0]["Bilateralmotorcoordination"].ToString();
                    Motoraccuracy.Text = ds.Tables[1].Rows[0]["Motoraccuracy"].ToString();
                    Postrotatorynystagmus.Text = ds.Tables[1].Rows[0]["Postrotatorynystagmus"].ToString();
                    Standingwalkingbalance.Text = ds.Tables[1].Rows[0]["Standingwalkingbalance"].ToString();
                    Touchtests.Text = ds.Tables[1].Rows[0]["Touchtests"].ToString();
                    Graphesthesia.Text = ds.Tables[1].Rows[0]["Graphesthesia"].ToString();
                    Kinesthesia.Text = ds.Tables[1].Rows[0]["Kinesthesia"].ToString();
                    Localizationoftactilestimuli.Text = ds.Tables[1].Rows[0]["Localizationoftactilestimuli"].ToString();
                    Manualformperception.Text = ds.Tables[1].Rows[0]["Manualformperception"].ToString();
                    Visualperceptiontests.Text = ds.Tables[1].Rows[0]["Visualperceptiontests"].ToString();
                    Figuregroundperception.Text = ds.Tables[1].Rows[0]["Figuregroundperception"].ToString();
                    Spacevisualization.Text = ds.Tables[1].Rows[0]["Spacevisualization"].ToString();
                    Others.Text = ds.Tables[1].Rows[0]["Others"].ToString();
                    Clockface.Text = ds.Tables[1].Rows[0]["Clockface"].ToString();
                    Motorplanning.Text = ds.Tables[1].Rows[0]["Motorplanning"].ToString();
                    Bodyimage.Text = ds.Tables[1].Rows[0]["Bodyimage"].ToString();
                    Bodyschema.Text = ds.Tables[1].Rows[0]["Bodyschema"].ToString();
                    Laterality.Text = ds.Tables[1].Rows[0]["Laterality"].ToString();
                    DataCollection_Referral.Text = ds.Tables[1].Rows[0]["DataCollection_Referral"].ToString();
                    DataCollection_MedicalHistory.Text = ds.Tables[1].Rows[0]["DataCollection_MedicalHistory"].ToString();
                    DataCollection_DailyRoutine.Text = ds.Tables[1].Rows[0]["DataCollection_DailyRoutine"].ToString();
                    DataCollection_Expectaion.Text = ds.Tables[1].Rows[0]["DataCollection_Expectaion"].ToString();
                    DataCollection_TherapyHistory.Text = ds.Tables[1].Rows[0]["DataCollection_TherapyHistory"].ToString();
                    DataCollection_Sources.Text = ds.Tables[1].Rows[0]["DataCollection_Sources"].ToString();
                    DataCollection_NumberVisit.Text = ds.Tables[1].Rows[0]["DataCollection_NumberVisit"].ToString();
                    DataCollection_AdaptedEquipment.Text = ds.Tables[1].Rows[0]["DataCollection_AdaptedEquipment"].ToString();
                    Morphology_Height.Text = ds.Tables[1].Rows[0]["Morphology_Height"].ToString();
                    Morphology_Weight.Text = ds.Tables[1].Rows[0]["Morphology_Weight"].ToString();
                    Morphology_LimbLength.Text = ds.Tables[1].Rows[0]["Morphology_LimbLength"].ToString();
                    Morphology_LimbLeft.Text = ds.Tables[1].Rows[0]["Morphology_LimbLeft"].ToString();
                    Morphology_LimbRight.Text = ds.Tables[1].Rows[0]["Morphology_LimbRight"].ToString();
                    Morphology_ArmLength.Text = ds.Tables[1].Rows[0]["Morphology_ArmLength"].ToString();
                    Morphology_ArmLeft.Text = ds.Tables[1].Rows[0]["Morphology_ArmLeft"].ToString();
                    Morphology_ArmRight.Text = ds.Tables[1].Rows[0]["Morphology_ArmRight"].ToString();
                    Morphology_Head.Text = ds.Tables[1].Rows[0]["Morphology_Head"].ToString();
                    Morphology_Nipple.Text = ds.Tables[1].Rows[0]["Morphology_Nipple"].ToString();
                    Morphology_Waist.Text = ds.Tables[1].Rows[0]["Morphology_Waist"].ToString();
                    Morphology_UpperLimbLevelRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_ABV"].ToString();
                    Morphology_UpperLimbLevelLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_ABV"].ToString();
                    Morphology_UpperLimbGirthRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_ABV"].ToString();
                    Morphology_UpperLimbGirthLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_ABV"].ToString();
                    Morphology_UpperLimbLevelRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_AT"].ToString();
                    Morphology_UpperLimbLevelLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_AT"].ToString();
                    Morphology_UpperLimbGirthRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_AT"].ToString();
                    Morphology_UpperLimbGirthLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_AT"].ToString();
                    Morphology_UpperLimbLevelRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_BLW"].ToString();
                    Morphology_UpperLimbLevelLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_BLW"].ToString();
                    Morphology_UpperLimbGirthRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_BLW"].ToString();
                    Morphology_UpperLimbGirthLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_BLW"].ToString();
                    Morphology_LowerLimbLevelRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_ABV"].ToString();
                    Morphology_LowerLimbLevelLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_ABV"].ToString();
                    Morphology_LowerLimbGirthRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_ABV"].ToString();
                    Morphology_LowerLimbGirthLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_ABV"].ToString();
                    Morphology_LowerLimbLevelRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_AT"].ToString();
                    Morphology_LowerLimbLevelLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_AT"].ToString();
                    Morphology_LowerLimbGirthRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_AT"].ToString();
                    Morphology_LowerLimbGirthLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_AT"].ToString();
                    Morphology_LowerLimbLevelRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_BLW"].ToString();
                    Morphology_LowerLimbLevelLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_BLW"].ToString();
                    Morphology_LowerLimbGirthRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_BLW"].ToString();
                    Morphology_LowerLimbGirthLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_BLW"].ToString();
                    Morphology_OralMotorFactors.Text = ds.Tables[1].Rows[0]["Morphology_OralMotorFactors"].ToString();
                    FunctionalActivities_GrossMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_GrossMotor"].ToString();
                    FunctionalActivities_HandFunction.Text = ds.Tables[1].Rows[0]["FunctionalActivities_HandFunction"].ToString();
                    FunctionalActivities_FineMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_FineMotor"].ToString();
                    FunctionalActivities_ADL.Text = ds.Tables[1].Rows[0]["FunctionalActivities_ADL"].ToString();
                    FunctionalActivities_OralMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_OralMotor"].ToString();
                    FunctionalActivities_Communication.Text = ds.Tables[1].Rows[0]["FunctionalActivities_Communication"].ToString();
                    TestMeasures_GMFCS.Text = ds.Tables[1].Rows[0]["TestMeasures_GMFCS"].ToString();
                    TestMeasures_GMFM.Text = ds.Tables[1].Rows[0]["TestMeasures_GMFM"].ToString();
                    TestMeasures_GMPM.Text = ds.Tables[1].Rows[0]["TestMeasures_GMPM"].ToString();
                    TestMeasures_AshworthScale.Text = ds.Tables[1].Rows[0]["TestMeasures_AshworthScale"].ToString();
                    TestMeasures_TradieusScale.Text = ds.Tables[1].Rows[0]["TestMeasures_TradieusScale"].ToString();
                    TestMeasures_OGS.Text = ds.Tables[1].Rows[0]["TestMeasures_OGS"].ToString();
                    TestMeasures_Melbourne.Text = ds.Tables[1].Rows[0]["TestMeasures_Melbourne"].ToString();
                    TestMeasures_COPM.Text = ds.Tables[1].Rows[0]["TestMeasures_COPM"].ToString();
                    TestMeasures_ClinicalObservation.Text = ds.Tables[1].Rows[0]["TestMeasures_ClinicalObservation"].ToString();
                    TestMeasures_Others.Text = ds.Tables[1].Rows[0]["TestMeasures_Others"].ToString();
                    Posture_Alignment.Text = ds.Tables[1].Rows[0]["Posture_Alignment"].ToString();
                    Posture_Biomechanics.Text = ds.Tables[1].Rows[0]["Posture_Biomechanics"].ToString();
                    Posture_Stability.Text = ds.Tables[1].Rows[0]["Posture_Stability"].ToString();
                    Posture_Anticipatory.Text = ds.Tables[1].Rows[0]["Posture_Anticipatory"].ToString();
                    Posture_Postural.Text = ds.Tables[1].Rows[0]["Posture_Postural"].ToString();
                    Posture_SignsPostural.Text = ds.Tables[1].Rows[0]["Posture_SignsPostural"].ToString();
                    Movement_Inertia.Text = ds.Tables[1].Rows[0]["Movement_Inertia"].ToString();
                    Movement_Strategies.Text = ds.Tables[1].Rows[0]["Movement_Strategies"].ToString();
                    Movement_Extremities.Text = ds.Tables[1].Rows[0]["Movement_Extremities"].ToString();
                    Movement_Stability.Text = ds.Tables[1].Rows[0]["Movement_Stability"].ToString();
                    Movement_Overuse.Text = ds.Tables[1].Rows[0]["Movement_Overuse"].ToString();
                    Others_Integration.Text = ds.Tables[1].Rows[0]["Others_Integration"].ToString();
                    Others_Assessments.Text = ds.Tables[1].Rows[0]["Others_Assessments"].ToString();
                    Regulatory_Arousal.Text = ds.Tables[1].Rows[0]["Regulatory_Arousal"].ToString();
                    Regulatory_Regulation.Text = ds.Tables[1].Rows[0]["Regulatory_Regulation"].ToString();
                    Musculoskeletal_Rom1_HipFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionLeft"].ToString();
                    Musculoskeletal_Rom1_HipFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionRight"].ToString();
                    Musculoskeletal_Rom1_HipExtensionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionLeft"].ToString();
                    Musculoskeletal_Rom1_HipExtensionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionRight"].ToString();
                    Musculoskeletal_Rom1_HipAbductionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionLeft"].ToString();
                    Musculoskeletal_Rom1_HipAbductionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionRight"].ToString();
                    Musculoskeletal_Rom1_HipExternalLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalLeft"].ToString();
                    Musculoskeletal_Rom1_HipExternalRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalRight"].ToString();
                    Musculoskeletal_Rom1_HipInternalLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalLeft"].ToString();
                    Musculoskeletal_Rom1_HipInternalRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalRight"].ToString();
                    Musculoskeletal_Rom1_PoplitealLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealLeft"].ToString();
                    Musculoskeletal_Rom1_PoplitealRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealRight"].ToString();
                    Musculoskeletal_Rom1_KneeFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionLeft"].ToString();
                    Musculoskeletal_Rom1_KneeFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionRight"].ToString();
                    Musculoskeletal_Rom1_KneeExtensionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionLeft"].ToString();
                    Musculoskeletal_Rom1_KneeExtensionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionRight"].ToString();
                    Musculoskeletal_Rom1_DorsiflexionFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionLeft"].ToString();
                    Musculoskeletal_Rom1_DorsiflexionFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionRight"].ToString();
                    Musculoskeletal_Rom1_DorsiflexionExtensionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionLeft"].ToString();
                    Musculoskeletal_Rom1_DorsiflexionExtensionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionRight"].ToString();
                    Musculoskeletal_Rom1_PlantarFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionLeft"].ToString();
                    Musculoskeletal_Rom1_PlantarFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionRight"].ToString();
                    Musculoskeletal_Rom1_OthersLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersLeft"].ToString();
                    Musculoskeletal_Rom1_OthersRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersRight"].ToString();
                    Musculoskeletal_Rom2_ShoulderFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionLeft"].ToString();
                    Musculoskeletal_Rom2_ShoulderFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionRight"].ToString();
                    Musculoskeletal_Rom2_ShoulderExtensionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionLeft"].ToString();
                    Musculoskeletal_Rom2_ShoulderExtensionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionRight"].ToString();
                    Musculoskeletal_Rom2_HorizontalAbductionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionLeft"].ToString();
                    Musculoskeletal_Rom2_HorizontalAbductionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionRight"].ToString();
                    Musculoskeletal_Rom2_ExternalRotationLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationLeft"].ToString();
                    Musculoskeletal_Rom2_ExternalRotationRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationRight"].ToString();
                    Musculoskeletal_Rom2_InternalRotationLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationLeft"].ToString();
                    Musculoskeletal_Rom2_InternalRotationRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationRight"].ToString();
                    Musculoskeletal_Rom2_ElbowFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionLeft"].ToString();
                    Musculoskeletal_Rom2_ElbowFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionRight"].ToString();
                    Musculoskeletal_Rom2_ElbowExtensionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionLeft"].ToString();
                    Musculoskeletal_Rom2_ElbowExtensionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionRight"].ToString();
                    Musculoskeletal_Rom2_SupinationLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationLeft"].ToString();
                    Musculoskeletal_Rom2_SupinationRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationRight"].ToString();
                    Musculoskeletal_Rom2_PronationLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationLeft"].ToString();
                    Musculoskeletal_Rom2_PronationRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationRight"].ToString();
                    Musculoskeletal_Rom2_WristFlexionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionLeft"].ToString();
                    Musculoskeletal_Rom2_WristFlexionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionRight"].ToString();
                    Musculoskeletal_Rom2_WristExtesionLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionLeft"].ToString();
                    Musculoskeletal_Rom2_WristExtesionRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionRight"].ToString();
                    Musculoskeletal_Rom2_OthersLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersLeft"].ToString();
                    Musculoskeletal_Rom2_OthersRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersRight"].ToString();
                    Musculoskeletal_Strengthlp.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Strengthlp"].ToString();
                    Musculoskeletal_StrengthCC.Text = ds.Tables[1].Rows[0]["Musculoskeletal_StrengthCC"].ToString();
                    Musculoskeletal_StrengthMuscle.Text = ds.Tables[1].Rows[0]["Musculoskeletal_StrengthMuscle"].ToString();
                    Musculoskeletal_StrengthSkeletal.Text = ds.Tables[1].Rows[0]["Musculoskeletal_StrengthSkeletal"].ToString();
                    Musculoskeletal_Mmt_HipflexorsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsLeft"].ToString();
                    Musculoskeletal_Mmt_HipflexorsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsRight"].ToString();
                    Musculoskeletal_Mmt_AbductorsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsLeft"].ToString();
                    Musculoskeletal_Mmt_AbductorsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsRight"].ToString();
                    Musculoskeletal_Mmt_ExtensorsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsLeft"].ToString();
                    Musculoskeletal_Mmt_ExtensorsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsRight"].ToString();
                    Musculoskeletal_Mmt_HamsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsLeft"].ToString();
                    Musculoskeletal_Mmt_HamsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsRight"].ToString();
                    Musculoskeletal_Mmt_QuadsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsLeft"].ToString();
                    Musculoskeletal_Mmt_QuadsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsRight"].ToString();
                    Musculoskeletal_Mmt_TibialisAnteriorLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorLeft"].ToString();
                    Musculoskeletal_Mmt_TibialisAnteriorRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorRight"].ToString();
                    Musculoskeletal_Mmt_TibialisPosteriorLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorLeft"].ToString();
                    Musculoskeletal_Mmt_TibialisPosteriorRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorRight"].ToString();
                    Musculoskeletal_Mmt_ExtensorDigitorumLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumLeft"].ToString();
                    Musculoskeletal_Mmt_ExtensorDigitorumRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumRight"].ToString();
                    Musculoskeletal_Mmt_ExtensorHallucisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisLeft"].ToString();
                    Musculoskeletal_Mmt_ExtensorHallucisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisRight"].ToString();
                    Musculoskeletal_Mmt_PeroneiLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiLeft"].ToString();
                    Musculoskeletal_Mmt_PeroneiRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiRight"].ToString();
                    Musculoskeletal_Mmt_FlexorDigitorumLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumLeft"].ToString();
                    Musculoskeletal_Mmt_FlexorDigitorumRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumRight"].ToString();
                    Musculoskeletal_Mmt_FlexorHallucisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisLeft"].ToString();
                    Musculoskeletal_Mmt_FlexorHallucisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisRight"].ToString();
                    Musculoskeletal_Mmt_AnteriorDeltoidLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidLeft"].ToString();
                    Musculoskeletal_Mmt_AnteriorDeltoidRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidRight"].ToString();
                    Musculoskeletal_Mmt_PosteriorDeltoidLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidLeft"].ToString();
                    Musculoskeletal_Mmt_PosteriorDeltoidRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidRight"].ToString();
                    Musculoskeletal_Mmt_MiddleDeltoidLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidLeft"].ToString();
                    Musculoskeletal_Mmt_MiddleDeltoidRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidRight"].ToString();
                    Musculoskeletal_Mmt_SupraspinatusLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusLeft"].ToString();
                    Musculoskeletal_Mmt_SupraspinatusRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusRight"].ToString();
                    Musculoskeletal_Mmt_SerratusAnteriorLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorLeft"].ToString();
                    Musculoskeletal_Mmt_SerratusAnteriorRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorRight"].ToString();
                    Musculoskeletal_Mmt_RhomboidsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsLeft"].ToString();
                    Musculoskeletal_Mmt_RhomboidsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsRight"].ToString();
                    Musculoskeletal_Mmt_BicepsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsLeft"].ToString();
                    Musculoskeletal_Mmt_BicepsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsRight"].ToString();
                    Musculoskeletal_Mmt_TricepsLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsLeft"].ToString();
                    Musculoskeletal_Mmt_TricepsRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsRight"].ToString();
                    Musculoskeletal_Mmt_SupinatorLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorLeft"].ToString();
                    Musculoskeletal_Mmt_SupinatorRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorRight"].ToString();
                    Musculoskeletal_Mmt_PronatorLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorLeft"].ToString();
                    Musculoskeletal_Mmt_PronatorRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorRight"].ToString();
                    Musculoskeletal_Mmt_ECULeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECULeft"].ToString();
                    Musculoskeletal_Mmt_ECURight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECURight"].ToString();
                    Musculoskeletal_Mmt_ECRLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRLeft"].ToString();
                    Musculoskeletal_Mmt_ECRRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRRight"].ToString();
                    Musculoskeletal_Mmt_ECSLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSLeft"].ToString();
                    Musculoskeletal_Mmt_ECSRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSRight"].ToString();
                    Musculoskeletal_Mmt_FCULeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCULeft"].ToString();
                    Musculoskeletal_Mmt_FCURight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCURight"].ToString();
                    Musculoskeletal_Mmt_FCRLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRLeft"].ToString();
                    Musculoskeletal_Mmt_FCRRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRRight"].ToString();
                    Musculoskeletal_Mmt_FCSLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSLeft"].ToString();
                    Musculoskeletal_Mmt_FCSRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSRight"].ToString();
                    Musculoskeletal_Mmt_OpponensPollicisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisLeft"].ToString();
                    Musculoskeletal_Mmt_OpponensPollicisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisRight"].ToString();
                    Musculoskeletal_Mmt_FlexorPollicisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisLeft"].ToString();
                    Musculoskeletal_Mmt_FlexorPollicisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisRight"].ToString();
                    Musculoskeletal_Mmt_AbductorPollicisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisLeft"].ToString();
                    Musculoskeletal_Mmt_AbductorPollicisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisRight"].ToString();
                    Musculoskeletal_Mmt_ExtensorPollicisLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisLeft"].ToString();
                    Musculoskeletal_Mmt_ExtensorPollicisRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisRight"].ToString();
                    SignOfCNS_NeuromotorControl.Text = ds.Tables[1].Rows[0]["SignOfCNS_NeuromotorControl"].ToString();
                    RemarkVariable_SustainGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_SustainGeneral"].ToString();
                    RemarkVariable_PosturalGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_PosturalGeneral"].ToString();
                    RemarkVariable_ContractionsGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_ContractionsGeneral"].ToString();
                    RemarkVariable_AntagonistGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_AntagonistGeneral"].ToString();
                    RemarkVariable_SynergyGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_SynergyGeneral"].ToString();
                    RemarkVariable_StiffinessGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_StiffinessGeneral"].ToString();
                    RemarkVariable_ExtraneousGeneral.Text = ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousGeneral"].ToString();
                    RemarkVariable_SustainControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_SustainControl"].ToString();
                    RemarkVariable_PosturalControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_PosturalControl"].ToString();
                    RemarkVariable_ContractionsControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_ContractionsControl"].ToString();
                    RemarkVariable_AntagonistControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_AntagonistControl"].ToString();
                    RemarkVariable_SynergyControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_SynergyControl"].ToString();
                    RemarkVariable_StiffinessControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_StiffinessControl"].ToString();
                    RemarkVariable_ExtraneousControl.Text = ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousControl"].ToString();
                    RemarkVariable_SustainTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_SustainTiming"].ToString();
                    RemarkVariable_PosturalTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_PosturalTiming"].ToString();
                    RemarkVariable_ContractionsTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_ContractionsTiming"].ToString();
                    RemarkVariable_AntagonistTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_AntagonistTiming"].ToString();
                    RemarkVariable_SynergyTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_SynergyTiming"].ToString();
                    RemarkVariable_StiffinessTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_StiffinessTiming"].ToString();
                    RemarkVariable_ExtraneousTiming.Text = ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousTiming"].ToString();
                    SensorySystem_Vision.Text = ds.Tables[1].Rows[0]["SensorySystem_Vision"].ToString();
                    SensorySystem_Somatosensory.Text = ds.Tables[1].Rows[0]["SensorySystem_Somatosensory"].ToString();
                    SensorySystem_Vestibular.Text = ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString();
                    SensorySystem_Auditory.Text = ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString();
                    SensorySystem_Gustatory.Text = ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString();
                    SensoryProfile_Profile.Text = ds.Tables[1].Rows[0]["SensoryProfile_Profile"].ToString();
                    SIPTInfo_History.Text = ds.Tables[1].Rows[0]["SIPTInfo_History"].ToString();
                    SIPTInfo_HandFunction1_GraspRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspRight"].ToString();
                    SIPTInfo_HandFunction1_GraspLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspLeft"].ToString();
                    SIPTInfo_HandFunction1_SphericalRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalRight"].ToString();
                    SIPTInfo_HandFunction1_SphericalLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalLeft"].ToString();
                    SIPTInfo_HandFunction1_HookRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookRight"].ToString();
                    SIPTInfo_HandFunction1_HookLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookLeft"].ToString();
                    SIPTInfo_HandFunction1_JawChuckRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckRight"].ToString();
                    SIPTInfo_HandFunction1_JawChuckLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckLeft"].ToString();
                    SIPTInfo_HandFunction1_GripRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripRight"].ToString();
                    SIPTInfo_HandFunction1_GripLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripLeft"].ToString();
                    SIPTInfo_HandFunction1_ReleaseRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseRight"].ToString();
                    SIPTInfo_HandFunction1_ReleaseLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseLeft"].ToString();
                    SIPTInfo_HandFunction2_OppositionLfR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfR"].ToString();
                    SIPTInfo_HandFunction2_OppositionLfL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfL"].ToString();
                    SIPTInfo_HandFunction2_OppositionMFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFR"].ToString();
                    SIPTInfo_HandFunction2_OppositionMFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFL"].ToString();
                    SIPTInfo_HandFunction2_OppositionRFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFR"].ToString();
                    SIPTInfo_HandFunction2_OppositionRFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFL"].ToString();
                    SIPTInfo_HandFunction2_PinchLfR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfR"].ToString();
                    SIPTInfo_HandFunction2_PinchLfL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfL"].ToString();
                    SIPTInfo_HandFunction2_PinchMFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFR"].ToString();
                    SIPTInfo_HandFunction2_PinchMFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFL"].ToString();
                    SIPTInfo_HandFunction2_PinchRFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFR"].ToString();
                    SIPTInfo_HandFunction2_PinchRFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFL"].ToString();
                    SIPTInfo_SIPT3_Spontaneous.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Spontaneous"].ToString();
                    SIPTInfo_SIPT3_Command.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Command"].ToString();
                    SIPTInfo_SIPT4_Kinesthesia.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Kinesthesia"].ToString();
                    SIPTInfo_SIPT4_Finger.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Finger"].ToString();
                    SIPTInfo_SIPT4_Localisation.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Localisation"].ToString();
                    SIPTInfo_SIPT4_DoubleTactile.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_DoubleTactile"].ToString();
                    SIPTInfo_SIPT4_Tactile.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Tactile"].ToString();
                    SIPTInfo_SIPT4_Graphesthesia.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Graphesthesia"].ToString();
                    SIPTInfo_SIPT4_PostRotary.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_PostRotary"].ToString();
                    SIPTInfo_SIPT4_Standing.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Standing"].ToString();
                    SIPTInfo_SIPT5_Color.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Color"].ToString();
                    SIPTInfo_SIPT5_Form.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Form"].ToString();
                    SIPTInfo_SIPT5_Size.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Size"].ToString();
                    SIPTInfo_SIPT5_Depth.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Depth"].ToString();
                    SIPTInfo_SIPT5_Figure.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Figure"].ToString();
                    SIPTInfo_SIPT5_Motor.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Motor"].ToString();
                    SIPTInfo_SIPT6_Design.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Design"].ToString();
                    SIPTInfo_SIPT6_Constructional.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Constructional"].ToString();
                    SIPTInfo_SIPT7_Scanning.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Scanning"].ToString();
                    SIPTInfo_SIPT7_Memory.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Memory"].ToString();
                    SIPTInfo_SIPT8_Postural.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Postural"].ToString();
                    SIPTInfo_SIPT8_Oral.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Oral"].ToString();
                    SIPTInfo_SIPT8_Sequencing.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Sequencing"].ToString();
                    SIPTInfo_SIPT8_Commands.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Commands"].ToString();
                    SIPTInfo_SIPT9_Bilateral.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Bilateral"].ToString();
                    SIPTInfo_SIPT9_Contralat.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Contralat"].ToString();
                    SIPTInfo_SIPT9_PreferredHand.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_PreferredHand"].ToString();
                    SIPTInfo_SIPT9_CrossingMidline.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_CrossingMidline"].ToString();
                    SIPTInfo_SIPT10_Draw.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Draw"].ToString();
                    SIPTInfo_SIPT10_ClockFace.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_ClockFace"].ToString();
                    SIPTInfo_SIPT10_Filtering.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Filtering"].ToString();
                    SIPTInfo_SIPT10_MotorPlanning.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_MotorPlanning"].ToString();
                    SIPTInfo_SIPT10_BodyImage.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodyImage"].ToString();
                    SIPTInfo_SIPT10_BodySchema.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodySchema"].ToString();
                    SIPTInfo_SIPT10_Laterality.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Laterality"].ToString();
                    SIPTInfo_ActivityGiven_Remark.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Remark"].ToString();
                    SIPTInfo_ActivityGiven_InterestActivity.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestActivity"].ToString();
                    SIPTInfo_ActivityGiven_InterestCompletion.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestCompletion"].ToString();
                    SIPTInfo_ActivityGiven_Learning.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Learning"].ToString();
                    SIPTInfo_ActivityGiven_Complexity.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Complexity"].ToString();
                    SIPTInfo_ActivityGiven_ProblemSolving.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_ProblemSolving"].ToString();
                    SIPTInfo_ActivityGiven_Concentration.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Concentration"].ToString();
                    SIPTInfo_ActivityGiven_Retension.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Retension"].ToString();
                    SIPTInfo_ActivityGiven_SpeedPerfom.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SpeedPerfom"].ToString();
                    SIPTInfo_ActivityGiven_Neatness.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Neatness"].ToString();
                    SIPTInfo_ActivityGiven_Frustation.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Frustation"].ToString();
                    SIPTInfo_ActivityGiven_Work.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Work"].ToString();
                    SIPTInfo_ActivityGiven_Reaction.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Reaction"].ToString();
                    SIPTInfo_ActivityGiven_SociabilityTherapist.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityTherapist"].ToString();
                    SIPTInfo_ActivityGiven_SociabilityStudents.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityStudents"].ToString();
                    Cognition_Intelligence.Text = ds.Tables[1].Rows[0]["Cognition_Intelligence"].ToString();
                    Cognition_Attention.Text = ds.Tables[1].Rows[0]["Cognition_Attention"].ToString();
                    Cognition_Memory.Text = ds.Tables[1].Rows[0]["Cognition_Memory"].ToString();
                    Cognition_Adaptibility.Text = ds.Tables[1].Rows[0]["Cognition_Adaptibility"].ToString();
                    Cognition_MotorPlanning.Text = ds.Tables[1].Rows[0]["Cognition_MotorPlanning"].ToString();
                    Cognition_ExecutiveFunction.Text = ds.Tables[1].Rows[0]["Cognition_ExecutiveFunction"].ToString();
                    Cognition_CognitiveFunctions.Text = ds.Tables[1].Rows[0]["Cognition_CognitiveFunctions"].ToString();
                    Integumentary_SkinIntegrity.Text = ds.Tables[1].Rows[0]["Integumentary_SkinIntegrity"].ToString();
                    Integumentary_SkinColor.Text = ds.Tables[1].Rows[0]["Integumentary_SkinColor"].ToString();
                    Integumentary_SkinExtensibility.Text = ds.Tables[1].Rows[0]["Integumentary_SkinExtensibility"].ToString();
                    Respiratory_RateResting.Text = ds.Tables[1].Rows[0]["Respiratory_RateResting"].ToString();
                    Respiratory_PostExercise.Text = ds.Tables[1].Rows[0]["Respiratory_PostExercise"].ToString();
                    Respiratory_Patterns.Text = ds.Tables[1].Rows[0]["Respiratory_Patterns"].ToString();
                    Respiratory_BreathControl.Text = ds.Tables[1].Rows[0]["Respiratory_BreathControl"].ToString();
                    Cardiovascular_HeartRate.Text = ds.Tables[1].Rows[0]["Cardiovascular_HeartRate"].ToString();
                    Cardiovascular_PostExercise.Text = ds.Tables[1].Rows[0]["Cardiovascular_PostExercise"].ToString();
                    Cardiovascular_BP.Text = ds.Tables[1].Rows[0]["Cardiovascular_BP"].ToString();
                    Cardiovascular_Edema.Text = ds.Tables[1].Rows[0]["Cardiovascular_Edema"].ToString();
                    Cardiovascular_Circulation.Text = ds.Tables[1].Rows[0]["Cardiovascular_Circulation"].ToString();
                    Cardiovascular_EEi.Text = ds.Tables[1].Rows[0]["Cardiovascular_EEi"].ToString();
                    Gastrointestinal_Bowel.Text = ds.Tables[1].Rows[0]["Gastrointestinal_Bowel"].ToString();
                    Gastrointestinal_Intake.Text = ds.Tables[1].Rows[0]["Gastrointestinal_Intake"].ToString();
                    Evaluation_Strengths.Text = ds.Tables[1].Rows[0]["Evaluation_Strengths"].ToString();
                    Evaluation_Concern_Barriers.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Barriers"].ToString();
                    Evaluation_Concern_Limitations.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Limitations"].ToString();
                    Evaluation_Concern_Posture.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Posture"].ToString();
                    Evaluation_Concern_Impairment.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Impairment"].ToString();
                    Evaluation_Goal_Summary.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString();
                    Evaluation_Goal_Previous.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Previous"].ToString();
                    Evaluation_Goal_LongTerm.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_LongTerm"].ToString();
                    Evaluation_Goal_ShortTerm.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_ShortTerm"].ToString();
                    Evaluation_Goal_Impairment.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Impairment"].ToString();
                    Evaluation_Plan_Frequency.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Frequency"].ToString();
                    Evaluation_Plan_Service.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Service"].ToString();
                    Evaluation_Plan_Strategies.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Strategies"].ToString();
                    Evaluation_Plan_Equipment.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Equipment"].ToString();
                    Evaluation_Plan_Education.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Education"].ToString();

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
                    FunctionalAbilities_GrossMotor.Text = ds.Tables[1].Rows[0]["FunctionalAbilities_GrossMotor"].ToString();
                    FunctionalAbilities_FineMotor.Text = ds.Tables[1].Rows[0]["FunctionalAbilities_FineMotor"].ToString();
                    FunctionalAbilities_Communication.Text = ds.Tables[1].Rows[0]["FunctionalAbilities_Communication"].ToString();
                    FunctionalAbilities_Cognitive.Text = ds.Tables[1].Rows[0]["FunctionalAbilities_Cognitive"].ToString();
                    FunctionalAbilities_Behaviour.Text = ds.Tables[1].Rows[0]["FunctionalAbilities_Behaviour"].ToString();
                    FunctionalLimitations_GrossMotor.Text = ds.Tables[1].Rows[0]["FunctionalLimitations_GrossMotor"].ToString();
                    FunctionalLimitations_FineMotor.Text = ds.Tables[1].Rows[0]["FunctionalLimitations_FineMotor"].ToString();
                    FunctionalLimitations_Communication.Text = ds.Tables[1].Rows[0]["FunctionalLimitations_Communication"].ToString();
                    FunctionalLimitations_Cognitive.Text = ds.Tables[1].Rows[0]["FunctionalLimitations_Cognitive"].ToString();
                    FunctionalLimitations_Behaviour.Text = ds.Tables[1].Rows[0]["FunctionalLimitations_Behaviour"].ToString();
                    ParticipationAbilities_GrossMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbilities_GrossMotor"].ToString();
                    ParticipationAbilities_FineMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbilities_FineMotor"].ToString();
                    ParticipationAbilities_Communication.Text = ds.Tables[1].Rows[0]["ParticipationAbilities_Communication"].ToString();
                    ParticipationAbilities_Cognitive.Text = ds.Tables[1].Rows[0]["ParticipationAbilities_Cognitive"].ToString();
                    ParticipationAbilities_Behaviour.Text = ds.Tables[1].Rows[0]["ParticipationAbilities_Behaviour"].ToString();
                    ParticipationLimitations_GrossMotor.Text = ds.Tables[1].Rows[0]["ParticipationLimitations_GrossMotor"].ToString();
                    ParticipationLimitations_FineMotor.Text = ds.Tables[1].Rows[0]["ParticipationLimitations_FineMotor"].ToString();
                    ParticipationLimitations_Communication.Text = ds.Tables[1].Rows[0]["ParticipationLimitations_Communication"].ToString();
                    ParticipationLimitations_Cognitive.Text = ds.Tables[1].Rows[0]["ParticipationLimitations_Cognitive"].ToString();
                    ParticipationLimitations_Behaviour.Text = ds.Tables[1].Rows[0]["ParticipationLimitations_Behaviour"].ToString();
                    FamilyStru_NoOfCaregivers.Text = ds.Tables[1].Rows[0]["FamilyStru_NoOfCaregivers"].ToString();
                    FamilyStru_TimeWithChild.Text = ds.Tables[1].Rows[0]["FamilyStru_TimeWithChild"].ToString();
                    FamilyStru_Holiday.Text = ds.Tables[1].Rows[0]["FamilyStru_Holiday"].ToString();
                    FamilyStru_DivoteTime.Text = ds.Tables[1].Rows[0]["FamilyStru_DivoteTime"].ToString();
                    FamilyStru_ContextualFactor.Text = ds.Tables[1].Rows[0]["FamilyStru_ContextualFactor"].ToString();
                    FamilyStru_Social.Text = ds.Tables[1].Rows[0]["FamilyStru_Social"].ToString();
                    FamilyStru_Environment.Text = ds.Tables[1].Rows[0]["FamilyStru_Environment"].ToString();
                    FamilyStru_Acceptance.Text = ds.Tables[1].Rows[0]["FamilyStru_Acceptance"].ToString();
                    FamilyStru_Accessibility.Text = ds.Tables[1].Rows[0]["FamilyStru_Accessibility"].ToString();
                    FamilyStru_CompareSibling.Text = ds.Tables[1].Rows[0]["FamilyStru_CompareSibling"].ToString();
                    FamilyStru_Working.Text = ds.Tables[1].Rows[0]["FamilyStru_Working"].ToString();
                    FamilyStru_FamilyPressure.Text = ds.Tables[1].Rows[0]["FamilyStru_FamilyPressure"].ToString();
                    FamilyStru_SpentMostTime.Text = ds.Tables[1].Rows[0]["FamilyStru_SpentMostTime"].ToString();
                    FamilyStru_CloselyInvolved.Text = ds.Tables[1].Rows[0]["FamilyStru_CloselyInvolved"].ToString();
                    FamilyStru_ChooseFreeTime.Text = ds.Tables[1].Rows[0]["FamilyStru_ChooseFreeTime"].ToString();
                    FamilyStru_PlayWithToys_1.Checked = false; FamilyStru_PlayWithToys_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStru_PlayWithToys"].ToString().Equals(FamilyStru_PlayWithToys_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStru_PlayWithToys_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStru_PlayWithToys"].ToString().Equals(FamilyStru_PlayWithToys_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStru_PlayWithToys_2.Checked = true;
                    }
                    FamilyStru_ToysExplain.Text = ds.Tables[1].Rows[0]["FamilyStru_ToysExplain"].ToString();
                    FamilyStru_ThrowTantrum_1.Checked = false; FamilyStru_ThrowTantrum_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStru_ThrowTantrum"].ToString().Equals(FamilyStru_ThrowTantrum_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStru_ThrowTantrum_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStru_ThrowTantrum"].ToString().Equals(FamilyStru_ThrowTantrum_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStru_ThrowTantrum_2.Checked = true;
                    }
                    SchoolInfo_SchoolType_1.Checked = false; SchoolInfo_SchoolType_2.Checked = false; SchoolInfo_SchoolType_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["SchoolInfo_SchoolType"].ToString().Equals(SchoolInfo_SchoolType_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SchoolInfo_SchoolType_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SchoolInfo_SchoolType"].ToString().Equals(SchoolInfo_SchoolType_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SchoolInfo_SchoolType_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SchoolInfo_SchoolType"].ToString().Equals(SchoolInfo_SchoolType_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SchoolInfo_SchoolType_3.Checked = true;
                    }
                    SchoolInfo_Hours.Text = ds.Tables[1].Rows[0]["SchoolInfo_Hours"].ToString();
                    SchoolInfo_Traveling.Text = ds.Tables[1].Rows[0]["SchoolInfo_Traveling"].ToString();
                    SchoolInfo_Teachers.Text = ds.Tables[1].Rows[0]["SchoolInfo_Teachers"].ToString();
                    SchoolInfo_SeatingArr.Text = ds.Tables[1].Rows[0]["SchoolInfo_SeatingArr"].ToString();
                    SchoolInfo_SeatingTol.Text = ds.Tables[1].Rows[0]["SchoolInfo_SeatingTol"].ToString();
                    SchoolInfo_MeanTime.Text = ds.Tables[1].Rows[0]["SchoolInfo_MeanTime"].ToString();
                    SchoolInfo_FriendInteraction.Text = ds.Tables[1].Rows[0]["SchoolInfo_FriendInteraction"].ToString();
                    SchoolInfo_Sports.Text = ds.Tables[1].Rows[0]["SchoolInfo_Sports"].ToString();
                    SchoolInfo_Curricular.Text = ds.Tables[1].Rows[0]["SchoolInfo_Curricular"].ToString();
                    SchoolInfo_Cultural.Text = ds.Tables[1].Rows[0]["SchoolInfo_Cultural"].ToString();
                    SchoolInfo_ShadowTeacher.Text = ds.Tables[1].Rows[0]["SchoolInfo_ShadowTeacher"].ToString();
                    SchoolInfo_RemarkTeacher.Text = ds.Tables[1].Rows[0]["SchoolInfo_RemarkTeacher"].ToString();
                    SchoolInfo_CopyBoard.Text = ds.Tables[1].Rows[0]["SchoolInfo_CopyBoard"].ToString();
                    SchoolInfo_CW_HW.Text = ds.Tables[1].Rows[0]["SchoolInfo_CW_HW"].ToString();
                    SchoolInfo_FollowInstru.Text = ds.Tables[1].Rows[0]["SchoolInfo_FollowInstru"].ToString();
                    SchoolInfo_SpecialEducator.Text = ds.Tables[1].Rows[0]["SchoolInfo_SpecialEducator"].ToString();
                    SchoolInfo_DeliveryMode.Text = ds.Tables[1].Rows[0]["SchoolInfo_DeliveryMode"].ToString();
                    SchoolInfo_AcademicScope.Text = ds.Tables[1].Rows[0]["SchoolInfo_AcademicScope"].ToString();
                    Behaviour_AtHome.Text = ds.Tables[1].Rows[0]["Behaviour_AtHome"].ToString();
                    Behaviour_AtSchool.Text = ds.Tables[1].Rows[0]["Behaviour_AtSchool"].ToString();
                    Behaviour_WithElder.Text = ds.Tables[1].Rows[0]["Behaviour_WithElder"].ToString();
                    Behaviour_WithPeers.Text = ds.Tables[1].Rows[0]["Behaviour_WithPeers"].ToString();
                    Behaviour_WithTeacher.Text = ds.Tables[1].Rows[0]["Behaviour_WithTeacher"].ToString();
                    Behaviour_AtTheMall.Text = ds.Tables[1].Rows[0]["Behaviour_AtTheMall"].ToString();
                    Behaviour_AtPlayground.Text = ds.Tables[1].Rows[0]["Behaviour_AtPlayground"].ToString();
                    BehaviourPl_Constructive.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPl_Constructive"].ToString().Equals(BehaviourPl_Constructive.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPl_Constructive.Checked = true;
                    }
                    BehaviourPl_Destructive.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPl_Destructive"].ToString().Equals(BehaviourPl_Destructive.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPl_Destructive.Checked = true;
                    }
                    BehaviourPl_CD_Remark.Text = ds.Tables[1].Rows[0]["BehaviourPl_CD_Remark"].ToString();
                    BehaviourPL_Independant.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Independant"].ToString().Equals(BehaviourPL_Independant.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Independant.Checked = true;
                    }
                    BehaviourPL_Supervised.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Supervised"].ToString().Equals(BehaviourPL_Supervised.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Supervised.Checked = true;
                    }
                    BehaviourPL_IS_Remark.Text = ds.Tables[1].Rows[0]["BehaviourPL_IS_Remark"].ToString();
                    BehaviourPL_Sedentary.Text = ds.Tables[1].Rows[0]["BehaviourPL_Sedentary"].ToString();
                    BehaviourPL_OnTheGo.Text = ds.Tables[1].Rows[0]["BehaviourPL_OnTheGo"].ToString();
                    BehaviourPL_AgeAppropriate_1.Checked = false; BehaviourPL_AgeAppropriate_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_AgeAppropriate"].ToString().Equals(BehaviourPL_AgeAppropriate_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_AgeAppropriate_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BehaviourPL_AgeAppropriate"].ToString().Equals(BehaviourPL_AgeAppropriate_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_AgeAppropriate_2.Checked = true;
                    }
                    BehaviourPL_FollowRule_1.Checked = false; BehaviourPL_FollowRule_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_FollowRule"].ToString().Equals(BehaviourPL_FollowRule_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_FollowRule_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BehaviourPL_FollowRule"].ToString().Equals(BehaviourPL_FollowRule_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_FollowRule_2.Checked = true;
                    }
                    BehaviourPL_Bullied_1.Checked = false; BehaviourPL_Bullied_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Bullied"].ToString().Equals(BehaviourPL_Bullied_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Bullied_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Bullied"].ToString().Equals(BehaviourPL_Bullied_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Bullied_2.Checked = true;
                    }
                    BehaviourPL_PlayAchieved.Text = ds.Tables[1].Rows[0]["BehaviourPL_PlayAchieved"].ToString();
                    BehaviourPL_ToyChoice.Text = ds.Tables[1].Rows[0]["BehaviourPL_ToyChoice"].ToString();

                    BehaviourPL_Repetitive_1.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Repetitive"].ToString().Equals(BehaviourPL_Repetitive_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Repetitive_1.Checked = true;
                    }
                    BehaviourPL_Repetitive_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_Versatile"].ToString().Equals(BehaviourPL_Repetitive_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_Repetitive_2.Checked = true;
                    }
                    BehaviourPL_PartInGroup_1.Checked = false; BehaviourPL_PartInGroup_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_PartInGroup"].ToString().Equals(BehaviourPL_PartInGroup_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_PartInGroup_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["BehaviourPL_PartInGroup"].ToString().Equals(BehaviourPL_PartInGroup_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_PartInGroup_2.Checked = true;
                    }
                    BehaviourPL_IsLeader_1.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_IsLeader"].ToString().Equals(BehaviourPL_IsLeader_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_IsLeader_1.Checked = true;
                    }
                    BehaviourPL_IsLeader_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["BehaviourPL_IsFollower"].ToString().Equals(BehaviourPL_IsLeader_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        BehaviourPL_IsLeader_2.Checked = true;
                    }
                    BehaviourPL_PretendPlay.Text = ds.Tables[1].Rows[0]["BehaviourPL_PretendPlay"].ToString();
                    Behaviour_RegulatesSelf_1.Checked = false; Behaviour_RegulatesSelf_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Behaviour_RegulatesSelf"].ToString().Equals(Behaviour_RegulatesSelf_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Behaviour_RegulatesSelf_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Behaviour_RegulatesSelf"].ToString().Equals(Behaviour_RegulatesSelf_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Behaviour_RegulatesSelf_2.Checked = true;
                    }
                    Behaviour_BehaveNotReg.Text = ds.Tables[1].Rows[0]["Behaviour_BehaveNotReg"].ToString();
                    Behaviour_WhatCalmDown.Text = ds.Tables[1].Rows[0]["Behaviour_WhatCalmDown"].ToString();
                    Behaviour_HappyLike.Text = ds.Tables[1].Rows[0]["Behaviour_HappyLike"].ToString();
                    Behaviour_HappyDislike.Text = ds.Tables[1].Rows[0]["Behaviour_HappyDislike"].ToString();
                    Arousal_EvalAlert.Text = ds.Tables[1].Rows[0]["Arousal_EvalAlert"].ToString();
                    Arousal_GeneralAlert.Text = ds.Tables[1].Rows[0]["Arousal_GeneralAlert"].ToString();
                    Arousal_StimuliResponse.Text = ds.Tables[1].Rows[0]["Arousal_StimuliResponse"].ToString();
                    Arousal_Transition.Text = ds.Tables[1].Rows[0]["Arousal_Transition"].ToString();
                    Arousal_Optimum.Text = ds.Tables[1].Rows[0]["Arousal_Optimum"].ToString();
                    Arousal_AlertingFactor.Text = ds.Tables[1].Rows[0]["Arousal_AlertingFactor"].ToString();
                    Arousal_CalmingFactor.Text = ds.Tables[1].Rows[0]["Arousal_CalmingFactor"].ToString();
                    Attention_InSchool.Text = ds.Tables[1].Rows[0]["Attention_InSchool"].ToString();
                    Attention_InHome.Text = ds.Tables[1].Rows[0]["Attention_InHome"].ToString();
                    Attention_Dividing.Text = ds.Tables[1].Rows[0]["Attention_Dividing"].ToString();
                    Attention_ChangeActivities.Text = ds.Tables[1].Rows[0]["Attention_ChangeActivities"].ToString();
                    Attention_AgeAppropriate_1.Checked = false; Attention_AgeAppropriate_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Equals(Attention_AgeAppropriate_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_AgeAppropriate_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Equals(Attention_AgeAppropriate_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_AgeAppropriate_2.Checked = true;
                    }
                    Attention_AttentionSpan.Text = ds.Tables[1].Rows[0]["Attention_AttentionSpan"].ToString();
                    Attention_Distractibility_Home.Text = ds.Tables[1].Rows[0]["Attention_Distractibility_Home"].ToString();
                    Attention_Distractibility_School.Text = ds.Tables[1].Rows[0]["Attention_Distractibility_School"].ToString();
                    Affect_EmotionRange_1.Checked = false; Affect_EmotionRange_2.Checked = false; Affect_EmotionRangeSometime.Checked = false;
                    if (ds.Tables[1].Rows[0]["Affect_EmotionRange"].ToString().Equals(Affect_EmotionRange_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionRange_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_EmotionRange"].ToString().Equals(Affect_EmotionRange_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionRange_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_EmotionRange"].ToString().Equals(Affect_EmotionRangeSometime.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionRangeSometime.Checked = true;
                    }
                    Affect_EmotionExpress_1.Checked = false; Affect_EmotionExpress_2.Checked = false; Affect_EmotionExpressSometime.Checked = false;
                    if (ds.Tables[1].Rows[0]["Affect_EmotionExpress"].ToString().Equals(Affect_EmotionExpress_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionExpress_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_EmotionExpress"].ToString().Equals(Affect_EmotionExpress_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionExpress_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_EmotionExpress"].ToString().Equals(Affect_EmotionExpressSometime.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_EmotionExpressSometime.Checked = true;
                    }
                    Affect_Environment.Text = ds.Tables[1].Rows[0]["Affect_Environment"].ToString();
                    Affect_Task.Text = ds.Tables[1].Rows[0]["Affect_Task"].ToString();
                    Affect_Individual.Text = ds.Tables[1].Rows[0]["Affect_Individual"].ToString();
                    Affect_Consistent.Text = ds.Tables[1].Rows[0]["Affect_Consistent"].ToString();
                    Affect_Characterising.Text = ds.Tables[1].Rows[0]["Affect_Characterising"].ToString();
                    Action_Planning.Text = ds.Tables[1].Rows[0]["Action_Planning"].ToString();
                    Action_Purposeful_1.Checked = false; Action_Purposeful_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Equals(Action_Purposeful_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Purposeful_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Equals(Action_Purposeful_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Purposeful_2.Checked = true;
                    }
                    Action_GoalOriented_1.Checked = false; Action_GoalOriented_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Equals(Action_GoalOriented_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_GoalOriented_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Equals(Action_GoalOriented_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_GoalOriented_2.Checked = true;
                    }
                    Action_FeedbackDependent_1.Checked = false; Action_FeedbackDependent_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_FeedbackDependent"].ToString().Equals(Action_FeedbackDependent_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_FeedbackDependent_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_FeedbackDependent"].ToString().Equals(Action_FeedbackDependent_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_FeedbackDependent_2.Checked = true;
                    }
                    Social_KnownPeople.Text = ds.Tables[1].Rows[0]["Social_KnownPeople"].ToString();
                    Social_Strangers.Text = ds.Tables[1].Rows[0]["Social_Strangers"].ToString();
                    Social_Gathering.Text = ds.Tables[1].Rows[0]["Social_Gathering"].ToString();
                    Social_Emotional.Text = ds.Tables[1].Rows[0]["Social_Emotional"].ToString();
                    Social_Appreciates_1.Checked = false; Social_Appreciates_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Social_Appreciates"].ToString().Equals(Social_Appreciates_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Social_Appreciates_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Social_Appreciates"].ToString().Equals(Social_Appreciates_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Social_Appreciates_2.Checked = true;
                    }
                    Social_Reaction.Text = ds.Tables[1].Rows[0]["Social_Reaction"].ToString();
                    Social_Sadness.Text = ds.Tables[1].Rows[0]["Social_Sadness"].ToString();
                    Social_Surprise.Text = ds.Tables[1].Rows[0]["Social_Surprise"].ToString();
                    Social_Shock.Text = ds.Tables[1].Rows[0]["Social_Shock"].ToString();
                    Social_Friendships_1.Checked = false; Social_Friendships_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Social_Friendships"].ToString().Equals(Social_Friendships_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Social_Friendships_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Social_Friendships"].ToString().Equals(Social_Friendships_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Social_Friendships_2.Checked = true;
                    }
                    Social_Relates.Text = ds.Tables[1].Rows[0]["Social_Relates"].ToString();
                    Social_ActivitiestheyEnjoy.Text = ds.Tables[1].Rows[0]["Social_ActivitiestheyEnjoy"].ToString();
                    Communication_StartToSpeak.Text = ds.Tables[1].Rows[0]["Communication_StartToSpeak"].ToString();
                    Communication_Monosyllables.Text = ds.Tables[1].Rows[0]["Communication_Monosyllables"].ToString();
                    Communication_Bisyllables.Text = ds.Tables[1].Rows[0]["Communication_Bisyllables"].ToString();
                    Communication_ShortSentences.Text = ds.Tables[1].Rows[0]["Communication_ShortSentences"].ToString();
                    Communication_LongSentence.Text = ds.Tables[1].Rows[0]["Communication_LongSentence"].ToString();
                    Communication_UnusualSounds_1.Checked = false; Communication_UnusualSounds_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Communication_UnusualSounds"].ToString().Equals(Communication_UnusualSounds_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_UnusualSounds_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Communication_UnusualSounds"].ToString().Equals(Communication_UnusualSounds_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_UnusualSounds_2.Checked = true;
                    }
                    Communication_ImitationOfSpeech_1.Checked = false; Communication_ImitationOfSpeech_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Communication_ImitationOfSpeech"].ToString().Equals(Communication_ImitationOfSpeech_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_ImitationOfSpeech_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Communication_ImitationOfSpeech"].ToString().Equals(Communication_ImitationOfSpeech_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_ImitationOfSpeech_2.Checked = true;
                    }
                    Communication_FacialExpression.Text = ds.Tables[1].Rows[0]["Communication_FacialExpression"].ToString();
                    Communication_EyeContact.Text = ds.Tables[1].Rows[0]["Communication_EyeContact"].ToString();
                    Communication_Gestures.Text = ds.Tables[1].Rows[0]["Communication_Gestures"].ToString();
                    Communication_InterpretationOfLanguage.Text = ds.Tables[1].Rows[0]["Communication_InterpretationOfLanguage"].ToString();
                    Communication_UnderstandImplied.Text = ds.Tables[1].Rows[0]["Communication_UnderstandImplied"].ToString();
                    Communication_UnderstandJoke.Text = ds.Tables[1].Rows[0]["Communication_UnderstandJoke"].ToString();
                    Communication_RespondsToName.Text = ds.Tables[1].Rows[0]["Communication_RespondsToName"].ToString();
                    Communication_TwoWayInteraction_1.Checked = false; Communication_TwoWayInteraction_2.Checked = false; Communication_TwoWayInteraction_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Communication_TwoWayInteraction"].ToString().Equals(Communication_TwoWayInteraction_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_TwoWayInteraction_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Communication_TwoWayInteraction"].ToString().Equals(Communication_TwoWayInteraction_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_TwoWayInteraction_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Communication_TwoWayInteraction"].ToString().Equals(Communication_TwoWayInteraction_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Communication_TwoWayInteraction_3.Checked = true;
                    }
                    Communication_NarrateIncidentsHome.Text = ds.Tables[1].Rows[0]["Communication_NarrateIncidentsHome"].ToString();
                    Communication_NarrateIncidentsSchool.Text = ds.Tables[1].Rows[0]["Communication_NarrateIncidentsSchool"].ToString();
                    Communication_ExpressionsWants.Text = ds.Tables[1].Rows[0]["Communication_ExpressionsWants"].ToString();
                    Communication_ExpressionsNeeds.Text = ds.Tables[1].Rows[0]["Communication_ExpressionsNeeds"].ToString();
                    Communication_ExpressionsEmotion.Text = ds.Tables[1].Rows[0]["Communication_ExpressionsEmotion"].ToString();
                    Communication_ExpressionsAchive.Text = ds.Tables[1].Rows[0]["Communication_ExpressionsAchive"].ToString();
                    Communication_LanguagSpoken.Text = ds.Tables[1].Rows[0]["Communication_LanguagSpoken"].ToString();
                    Communication_Echolalia.Text = ds.Tables[1].Rows[0]["Communication_Echolalia"].ToString();
                    RepetitiveInterests_Dominates.Text = ds.Tables[1].Rows[0]["RepetitiveInterests_Dominates"].ToString();
                    RepetitiveInterests_Behavior.Text = ds.Tables[1].Rows[0]["RepetitiveInterests_Behavior"].ToString();
                    RepetitiveInterests_Changes.Text = ds.Tables[1].Rows[0]["RepetitiveInterests_Changes"].ToString();
                    SensorySystemsVisual_Focal.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Focal"].ToString();
                    SensorySystemsVisual_Ambient.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Ambient"].ToString();
                    SensorySystemsVisual_Focus.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Focus"].ToString();
                    SensorySystemsVisual_Depth.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Depth"].ToString();
                    SensorySystemsVisual_Refractive.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Refractive"].ToString();
                    SensorySystemsVisual_Physical.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Physical"].ToString();
                    SensorySystemsVestibula_Seeking.Text = ds.Tables[1].Rows[0]["SensorySystemsVestibula_Seeking"].ToString();
                    SensorySystemsVestibula_Avoiding.Text = ds.Tables[1].Rows[0]["SensorySystemsVestibula_Avoiding"].ToString();
                    SensorySystemsVestibula_Insecurities.Text = ds.Tables[1].Rows[0]["SensorySystemsVestibula_Insecurities"].ToString();
                    SensorySystemsOromotor_Defensive.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Defensive"].ToString();
                    SensorySystemsOromotor_Drooling.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Drooling"].ToString();
                    SensorySystemsOromotor_Mouth.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouth"].ToString();
                    SensorySystemsOromotor_Mouthing.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouthing"].ToString();
                    SensorySystemsOromotor_Chew.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Chew"].ToString();
                    SensorySystemsAuditory_Response.Text = ds.Tables[1].Rows[0]["SensorySystemsAuditory_Response"].ToString();
                    SensorySystemsAuditory_Seeking.Text = ds.Tables[1].Rows[0]["SensorySystemsAuditory_Seeking"].ToString();
                    SensorySystemsAuditory_Avoiding.Text = ds.Tables[1].Rows[0]["SensorySystemsAuditory_Avoiding"].ToString();
                    SensorySystemsOlfactory_seeking.Text = ds.Tables[1].Rows[0]["SensorySystemsOlfactory_seeking"].ToString();
                    SensorySystemsOlfactory_Avoiding.Text = ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Avoiding"].ToString();
                    SensorySystemsSomatosensory_Seeking.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Seeking"].ToString();
                    SensorySystemsSomatosensory_Avoiding.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Avoiding"].ToString();
                    SensorySystemsSomatosensoryProprioceptive_BodyImage.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyImage"].ToString();
                    SensorySystemsSomatosensoryProprioceptive_BodyParts.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyParts"].ToString();
                    SensorySystemsSomatosensoryProprioceptive_Clumsiness.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Clumsiness"].ToString();
                    SensorySystemsSomatosensoryProprioceptive_Coordination.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Coordination"].ToString();
                    Other_SensoryProfile.Text = ds.Tables[1].Rows[0]["Other_SensoryProfile"].ToString();
                    Other_SIPT.Text = ds.Tables[1].Rows[0]["Other_SIPT"].ToString();
                    Other_DCD.Text = ds.Tables[1].Rows[0]["Other_DCD"].ToString();
                    Other_DSM.Text = ds.Tables[1].Rows[0]["Other_DSM"].ToString();
                    GoalsAndExpectations.Text = ds.Tables[1].Rows[0]["GoalsAndExpectations"].ToString();
                    SensorySystemsVisual_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsVisual_Comment"].ToString();
                    SensorySystemsVestibula_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsVestibula_Comment"].ToString();
                    SensorySystemsOromotor_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsOromotor_Comment"].ToString();
                    SensorySystemsAuditory_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsAuditory_Comment"].ToString();
                    SensorySystemsOlfactory_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Comment"].ToString();
                    SensorySystemsSomatosensory_Comment.Text = ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Comment"].ToString();

                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=si&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportSiMst_Bll RDB = new SnehBLL.ReportSiMst_Bll();
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
            DataSet ds = RDB.Get(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            if (g < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            }

            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }
            int EnterReport = 0; if (Doctor_EnterReport.SelectedItem != null) { int.TryParse(Doctor_EnterReport.SelectedItem.Value, out EnterReport); }

            string FamilyStru_PlayWithToys = string.Empty;
            if (FamilyStru_PlayWithToys_1.Checked) { FamilyStru_PlayWithToys = FamilyStru_PlayWithToys_1.Text.Trim(); }
            if (FamilyStru_PlayWithToys_2.Checked) { FamilyStru_PlayWithToys = FamilyStru_PlayWithToys_2.Text.Trim(); }
            string FamilyStru_ThrowTantrum = string.Empty;
            if (FamilyStru_ThrowTantrum_1.Checked) { FamilyStru_ThrowTantrum = FamilyStru_ThrowTantrum_1.Text.Trim(); }
            if (FamilyStru_ThrowTantrum_2.Checked) { FamilyStru_ThrowTantrum = FamilyStru_ThrowTantrum_2.Text.Trim(); }
            string SchoolInfo_SchoolType = string.Empty;
            if (SchoolInfo_SchoolType_1.Checked) { SchoolInfo_SchoolType = SchoolInfo_SchoolType_1.Text.Trim(); }
            if (SchoolInfo_SchoolType_2.Checked) { SchoolInfo_SchoolType = SchoolInfo_SchoolType_2.Text.Trim(); }
            if (SchoolInfo_SchoolType_3.Checked) { SchoolInfo_SchoolType = SchoolInfo_SchoolType_3.Text.Trim(); }
            string _behaviourPl_Constructive = string.Empty;
            if (BehaviourPl_Constructive.Checked) { _behaviourPl_Constructive = BehaviourPl_Constructive.Text.Trim(); }
            string _behaviourPl_Destructive = string.Empty;
            if (BehaviourPl_Destructive.Checked) { _behaviourPl_Destructive = BehaviourPl_Destructive.Text.Trim(); }
            string _behaviourPL_Independant = string.Empty;
            if (BehaviourPL_Independant.Checked) { _behaviourPL_Independant = BehaviourPL_Independant.Text.Trim(); }
            string _behaviourPL_Supervised = string.Empty;
            if (BehaviourPL_Supervised.Checked) { _behaviourPL_Supervised = BehaviourPL_Supervised.Text.Trim(); }
            string BehaviourPL_AgeAppropriate = string.Empty;
            if (BehaviourPL_AgeAppropriate_1.Checked) { BehaviourPL_AgeAppropriate = BehaviourPL_AgeAppropriate_1.Text.Trim(); }
            if (BehaviourPL_AgeAppropriate_2.Checked) { BehaviourPL_AgeAppropriate = BehaviourPL_AgeAppropriate_2.Text.Trim(); }
            string BehaviourPL_FollowRule = string.Empty;
            if (BehaviourPL_FollowRule_1.Checked) { BehaviourPL_FollowRule = BehaviourPL_FollowRule_1.Text.Trim(); }
            if (BehaviourPL_FollowRule_2.Checked) { BehaviourPL_FollowRule = BehaviourPL_FollowRule_2.Text.Trim(); }
            string BehaviourPL_Bullied = string.Empty;
            if (BehaviourPL_Bullied_1.Checked) { BehaviourPL_Bullied = BehaviourPL_Bullied_1.Text.Trim(); }
            if (BehaviourPL_Bullied_2.Checked) { BehaviourPL_Bullied = BehaviourPL_Bullied_2.Text.Trim(); }
            string BehaviourPL_Repetitive = string.Empty;
            if (BehaviourPL_Repetitive_1.Checked) { BehaviourPL_Repetitive = BehaviourPL_Repetitive_1.Text.Trim(); }
            string BehaviourPL_Versatile = string.Empty;
            if (BehaviourPL_Repetitive_2.Checked) { BehaviourPL_Versatile = BehaviourPL_Repetitive_2.Text.Trim(); }
            string BehaviourPL_PartInGroup = string.Empty;
            if (BehaviourPL_PartInGroup_1.Checked) { BehaviourPL_PartInGroup = BehaviourPL_PartInGroup_1.Text.Trim(); }
            if (BehaviourPL_PartInGroup_2.Checked) { BehaviourPL_PartInGroup = BehaviourPL_PartInGroup_2.Text.Trim(); }
            string BehaviourPL_IsLeader = string.Empty;
            if (BehaviourPL_IsLeader_1.Checked) { BehaviourPL_IsLeader = BehaviourPL_IsLeader_1.Text.Trim(); }
            string BehaviourPL_IsFollower = string.Empty;
            if (BehaviourPL_IsLeader_2.Checked) { BehaviourPL_IsFollower = BehaviourPL_IsLeader_2.Text.Trim(); }
            string Behaviour_RegulatesSelf = string.Empty;
            if (Behaviour_RegulatesSelf_1.Checked) { Behaviour_RegulatesSelf = Behaviour_RegulatesSelf_1.Text.Trim(); }
            if (Behaviour_RegulatesSelf_2.Checked) { Behaviour_RegulatesSelf = Behaviour_RegulatesSelf_2.Text.Trim(); }
            string Attention_AgeAppropriate = string.Empty;
            if (Attention_AgeAppropriate_1.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_1.Text.Trim(); }
            if (Attention_AgeAppropriate_2.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_2.Text.Trim(); }
            string Affect_EmotionRange = string.Empty;
            if (Affect_EmotionRange_1.Checked) { Affect_EmotionRange = Affect_EmotionRange_1.Text.Trim(); }
            if (Affect_EmotionRange_2.Checked) { Affect_EmotionRange = Affect_EmotionRange_2.Text.Trim(); }
            if (Affect_EmotionRangeSometime.Checked) { Affect_EmotionRange = Affect_EmotionRangeSometime.Text.Trim(); }
            string Affect_EmotionExpress = string.Empty;
            if (Affect_EmotionExpress_1.Checked) { Affect_EmotionExpress = Affect_EmotionExpress_1.Text.Trim(); }
            if (Affect_EmotionExpress_2.Checked) { Affect_EmotionExpress = Affect_EmotionExpress_2.Text.Trim(); }
            if (Affect_EmotionExpressSometime.Checked)
            {
                Affect_EmotionExpress
                    = Affect_EmotionExpressSometime.Text.Trim();
            }
            string Action_Purposeful = string.Empty;
            if (Action_Purposeful_1.Checked) { Action_Purposeful = Action_Purposeful_1.Text.Trim(); }
            if (Action_Purposeful_2.Checked) { Action_Purposeful = Action_Purposeful_2.Text.Trim(); }
            string Action_GoalOriented = string.Empty;
            if (Action_GoalOriented_1.Checked) { Action_GoalOriented = Action_GoalOriented_1.Text.Trim(); }
            if (Action_GoalOriented_2.Checked) { Action_GoalOriented = Action_GoalOriented_2.Text.Trim(); }
            string Action_FeedbackDependent = string.Empty;
            if (Action_FeedbackDependent_1.Checked) { Action_FeedbackDependent = Action_FeedbackDependent_1.Text.Trim(); }
            if (Action_FeedbackDependent_2.Checked) { Action_FeedbackDependent = Action_FeedbackDependent_2.Text.Trim(); }
            string Social_Appreciates = string.Empty;
            if (Social_Appreciates_1.Checked) { Social_Appreciates = Social_Appreciates_1.Text.Trim(); }
            if (Social_Appreciates_2.Checked) { Social_Appreciates = Social_Appreciates_2.Text.Trim(); }
            string Social_Friendships = string.Empty;
            if (Social_Friendships_1.Checked) { Social_Friendships = Social_Friendships_1.Text.Trim(); }
            if (Social_Friendships_2.Checked) { Social_Friendships = Social_Friendships_2.Text.Trim(); }
            string Communication_UnusualSounds = string.Empty;
            if (Communication_UnusualSounds_1.Checked) { Communication_UnusualSounds = Communication_UnusualSounds_1.Text.Trim(); }
            if (Communication_UnusualSounds_2.Checked) { Communication_UnusualSounds = Communication_UnusualSounds_2.Text.Trim(); }
            string Communication_ImitationOfSpeech = string.Empty;
            if (Communication_ImitationOfSpeech_1.Checked) { Communication_ImitationOfSpeech = Communication_ImitationOfSpeech_1.Text.Trim(); }
            if (Communication_ImitationOfSpeech_2.Checked) { Communication_ImitationOfSpeech = Communication_ImitationOfSpeech_2.Text.Trim(); }
            string Communication_TwoWayInteraction = string.Empty;
            if (Communication_TwoWayInteraction_1.Checked) { Communication_TwoWayInteraction = Communication_TwoWayInteraction_1.Text.Trim(); }
            if (Communication_TwoWayInteraction_2.Checked) { Communication_TwoWayInteraction = Communication_TwoWayInteraction_2.Text.Trim(); }
            if (Communication_TwoWayInteraction_3.Checked) { Communication_TwoWayInteraction = Communication_TwoWayInteraction_3.Text.Trim(); }

            int i = RDB.Set(_appointmentID, DataCollection_Referral.Text.Trim(), DataCollection_MedicalHistory.Text.Trim(), DataCollection_DailyRoutine.Text.Trim(),
              DataCollection_Expectaion.Text.Trim(), DataCollection_TherapyHistory.Text.Trim(), DataCollection_Sources.Text.Trim(), DataCollection_NumberVisit.Text.Trim(),
              DataCollection_AdaptedEquipment.Text.Trim(), Morphology_Height.Text.Trim(), Morphology_Weight.Text.Trim(), Morphology_LimbLength.Text.Trim(),
              Morphology_LimbLeft.Text.Trim(), Morphology_LimbRight.Text.Trim(), Morphology_ArmLength.Text.Trim(), Morphology_ArmLeft.Text.Trim(), Morphology_ArmRight.Text.Trim(),
              Morphology_Head.Text.Trim(), Morphology_Nipple.Text.Trim(), Morphology_Waist.Text.Trim(), Morphology_UpperLimbLevelRight_ABV.Text.Trim(), Morphology_UpperLimbLevelLeft_ABV.Text.Trim(),
              Morphology_UpperLimbGirthRight_ABV.Text.Trim(), Morphology_UpperLimbGirthLeft_ABV.Text.Trim(), Morphology_UpperLimbLevelRight_AT.Text.Trim(),
              Morphology_UpperLimbLevelLeft_AT.Text.Trim(), Morphology_UpperLimbGirthRight_AT.Text.Trim(), Morphology_UpperLimbGirthLeft_AT.Text.Trim(),
              Morphology_UpperLimbLevelRight_BLW.Text.Trim(), Morphology_UpperLimbLevelLeft_BLW.Text.Trim(), Morphology_UpperLimbGirthRight_BLW.Text.Trim(),
              Morphology_UpperLimbGirthLeft_BLW.Text.Trim(), Morphology_LowerLimbLevelRight_ABV.Text.Trim(), Morphology_LowerLimbLevelLeft_ABV.Text.Trim(),
              Morphology_LowerLimbGirthRight_ABV.Text.Trim(), Morphology_LowerLimbGirthLeft_ABV.Text.Trim(), Morphology_LowerLimbLevelRight_AT.Text.Trim(),
              Morphology_LowerLimbLevelLeft_AT.Text.Trim(), Morphology_LowerLimbGirthRight_AT.Text.Trim(), Morphology_LowerLimbGirthLeft_AT.Text.Trim(),
              Morphology_LowerLimbLevelRight_BLW.Text.Trim(), Morphology_LowerLimbLevelLeft_BLW.Text.Trim(), Morphology_LowerLimbGirthRight_BLW.Text.Trim(),
              Morphology_LowerLimbGirthLeft_BLW.Text.Trim(), Morphology_OralMotorFactors.Text.Trim(), FunctionalActivities_GrossMotor.Text.Trim(),
              FunctionalActivities_HandFunction.Text.Trim(), FunctionalActivities_FineMotor.Text.Trim(), FunctionalActivities_ADL.Text.Trim(), FunctionalActivities_OralMotor.Text.Trim(),
              FunctionalActivities_Communication.Text.Trim(), TestMeasures_GMFCS.Text.Trim(), TestMeasures_GMFM.Text.Trim(), TestMeasures_GMPM.Text.Trim(), TestMeasures_AshworthScale.Text.Trim(),
              TestMeasures_TradieusScale.Text.Trim(), TestMeasures_OGS.Text.Trim(), TestMeasures_Melbourne.Text.Trim(), TestMeasures_COPM.Text.Trim(), TestMeasures_ClinicalObservation.Text.Trim(),
              TestMeasures_Others.Text.Trim(), Posture_Alignment.Text.Trim(), Posture_Biomechanics.Text.Trim(), Posture_Stability.Text.Trim(), Posture_Anticipatory.Text.Trim(), Posture_Postural.Text.Trim(),
              Posture_SignsPostural.Text.Trim(), Movement_Inertia.Text.Trim(), Movement_Strategies.Text.Trim(), Movement_Extremities.Text.Trim(), Movement_Stability.Text.Trim(), Movement_Overuse.Text.Trim(),
              Others_Integration.Text.Trim(), Others_Assessments.Text.Trim(), Regulatory_Arousal.Text.Trim(), Regulatory_Regulation.Text.Trim(), Musculoskeletal_Rom1_HipFlexionLeft.Text.Trim(),
              Musculoskeletal_Rom1_HipFlexionRight.Text.Trim(), Musculoskeletal_Rom1_HipExtensionLeft.Text.Trim(), Musculoskeletal_Rom1_HipAbductionLeft.Text.Trim(), Musculoskeletal_Rom1_HipAbductionRight.Text.Trim(), Musculoskeletal_Rom1_HipExtensionRight.Text.Trim(),
              Musculoskeletal_Rom1_HipExternalLeft.Text.Trim(), Musculoskeletal_Rom1_HipExternalRight.Text.Trim(), Musculoskeletal_Rom1_HipInternalLeft.Text.Trim(),
              Musculoskeletal_Rom1_HipInternalRight.Text.Trim(), Musculoskeletal_Rom1_PoplitealLeft.Text.Trim(), Musculoskeletal_Rom1_PoplitealRight.Text.Trim(),
              Musculoskeletal_Rom1_KneeFlexionLeft.Text.Trim(), Musculoskeletal_Rom1_KneeFlexionRight.Text.Trim(), Musculoskeletal_Rom1_KneeExtensionLeft.Text.Trim(),
              Musculoskeletal_Rom1_KneeExtensionRight.Text.Trim(), Musculoskeletal_Rom1_DorsiflexionFlexionLeft.Text.Trim(), Musculoskeletal_Rom1_DorsiflexionFlexionRight.Text.Trim(),
              Musculoskeletal_Rom1_DorsiflexionExtensionLeft.Text.Trim(), Musculoskeletal_Rom1_DorsiflexionExtensionRight.Text.Trim(), Musculoskeletal_Rom1_PlantarFlexionLeft.Text.Trim(),
              Musculoskeletal_Rom1_PlantarFlexionRight.Text.Trim(), Musculoskeletal_Rom1_OthersLeft.Text.Trim(), Musculoskeletal_Rom1_OthersRight.Text.Trim(), Musculoskeletal_Rom2_ShoulderFlexionLeft.Text.Trim(),
              Musculoskeletal_Rom2_ShoulderFlexionRight.Text.Trim(), Musculoskeletal_Rom2_ShoulderExtensionLeft.Text.Trim(), Musculoskeletal_Rom2_ShoulderExtensionRight.Text.Trim(),
              Musculoskeletal_Rom2_HorizontalAbductionLeft.Text.Trim(), Musculoskeletal_Rom2_HorizontalAbductionRight.Text.Trim(), Musculoskeletal_Rom2_ExternalRotationLeft.Text.Trim(),
              Musculoskeletal_Rom2_ExternalRotationRight.Text.Trim(), Musculoskeletal_Rom2_InternalRotationLeft.Text.Trim(), Musculoskeletal_Rom2_InternalRotationRight.Text.Trim(),
              Musculoskeletal_Rom2_ElbowFlexionLeft.Text.Trim(), Musculoskeletal_Rom2_ElbowFlexionRight.Text.Trim(), Musculoskeletal_Rom2_ElbowExtensionLeft.Text.Trim(),
              Musculoskeletal_Rom2_ElbowExtensionRight.Text.Trim(), Musculoskeletal_Rom2_SupinationLeft.Text.Trim(), Musculoskeletal_Rom2_SupinationRight.Text.Trim(),
              Musculoskeletal_Rom2_PronationLeft.Text.Trim(), Musculoskeletal_Rom2_PronationRight.Text.Trim(), Musculoskeletal_Rom2_WristFlexionLeft.Text.Trim(),
              Musculoskeletal_Rom2_WristFlexionRight.Text.Trim(), Musculoskeletal_Rom2_WristExtesionLeft.Text.Trim(), Musculoskeletal_Rom2_WristExtesionRight.Text.Trim(),
              Musculoskeletal_Rom2_OthersLeft.Text.Trim(), Musculoskeletal_Rom2_OthersRight.Text.Trim(), Musculoskeletal_Strengthlp.Text.Trim(), Musculoskeletal_StrengthCC.Text.Trim(),
              Musculoskeletal_StrengthMuscle.Text.Trim(), Musculoskeletal_StrengthSkeletal.Text.Trim(), Musculoskeletal_Mmt_HipflexorsLeft.Text.Trim(), Musculoskeletal_Mmt_HipflexorsRight.Text.Trim(),
              Musculoskeletal_Mmt_AbductorsLeft.Text.Trim(), Musculoskeletal_Mmt_AbductorsRight.Text.Trim(), Musculoskeletal_Mmt_ExtensorsLeft.Text.Trim(), Musculoskeletal_Mmt_ExtensorsRight.Text.Trim(),
              Musculoskeletal_Mmt_HamsLeft.Text.Trim(), Musculoskeletal_Mmt_HamsRight.Text.Trim(), Musculoskeletal_Mmt_QuadsLeft.Text.Trim(), Musculoskeletal_Mmt_QuadsRight.Text.Trim(),
              Musculoskeletal_Mmt_TibialisAnteriorLeft.Text.Trim(), Musculoskeletal_Mmt_TibialisAnteriorRight.Text.Trim(), Musculoskeletal_Mmt_TibialisPosteriorLeft.Text.Trim(),
              Musculoskeletal_Mmt_TibialisPosteriorRight.Text.Trim(), Musculoskeletal_Mmt_ExtensorDigitorumLeft.Text.Trim(), Musculoskeletal_Mmt_ExtensorDigitorumRight.Text.Trim(),
              Musculoskeletal_Mmt_ExtensorHallucisLeft.Text.Trim(), Musculoskeletal_Mmt_ExtensorHallucisRight.Text.Trim(), Musculoskeletal_Mmt_PeroneiLeft.Text.Trim(), Musculoskeletal_Mmt_PeroneiRight.Text.Trim(),
              Musculoskeletal_Mmt_FlexorDigitorumLeft.Text.Trim(), Musculoskeletal_Mmt_FlexorDigitorumRight.Text.Trim(), Musculoskeletal_Mmt_FlexorHallucisLeft.Text.Trim(),
              Musculoskeletal_Mmt_FlexorHallucisRight.Text.Trim(), Musculoskeletal_Mmt_AnteriorDeltoidLeft.Text.Trim(), Musculoskeletal_Mmt_AnteriorDeltoidRight.Text.Trim(),
              Musculoskeletal_Mmt_PosteriorDeltoidLeft.Text.Trim(), Musculoskeletal_Mmt_PosteriorDeltoidRight.Text.Trim(), Musculoskeletal_Mmt_MiddleDeltoidLeft.Text.Trim(),
              Musculoskeletal_Mmt_MiddleDeltoidRight.Text.Trim(), Musculoskeletal_Mmt_SupraspinatusLeft.Text.Trim(), Musculoskeletal_Mmt_SupraspinatusRight.Text.Trim(),
              Musculoskeletal_Mmt_SerratusAnteriorLeft.Text.Trim(), Musculoskeletal_Mmt_SerratusAnteriorRight.Text.Trim(), Musculoskeletal_Mmt_RhomboidsLeft.Text.Trim(),
              Musculoskeletal_Mmt_RhomboidsRight.Text.Trim(), Musculoskeletal_Mmt_BicepsLeft.Text.Trim(), Musculoskeletal_Mmt_BicepsRight.Text.Trim(), Musculoskeletal_Mmt_TricepsLeft.Text.Trim(),
              Musculoskeletal_Mmt_TricepsRight.Text.Trim(), Musculoskeletal_Mmt_SupinatorLeft.Text.Trim(), Musculoskeletal_Mmt_SupinatorRight.Text.Trim(), Musculoskeletal_Mmt_PronatorLeft.Text.Trim(),
              Musculoskeletal_Mmt_PronatorRight.Text.Trim(), Musculoskeletal_Mmt_ECULeft.Text.Trim(), Musculoskeletal_Mmt_ECURight.Text.Trim(), Musculoskeletal_Mmt_ECRLeft.Text.Trim(),
              Musculoskeletal_Mmt_ECRRight.Text.Trim(), Musculoskeletal_Mmt_ECSLeft.Text.Trim(), Musculoskeletal_Mmt_ECSRight.Text.Trim(), Musculoskeletal_Mmt_FCULeft.Text.Trim(), Musculoskeletal_Mmt_FCURight.Text.Trim(),
              Musculoskeletal_Mmt_FCRLeft.Text.Trim(), Musculoskeletal_Mmt_FCRRight.Text.Trim(), Musculoskeletal_Mmt_FCSLeft.Text.Trim(), Musculoskeletal_Mmt_FCSRight.Text.Trim(),
              Musculoskeletal_Mmt_OpponensPollicisLeft.Text.Trim(), Musculoskeletal_Mmt_OpponensPollicisRight.Text.Trim(), Musculoskeletal_Mmt_FlexorPollicisLeft.Text.Trim(),
              Musculoskeletal_Mmt_FlexorPollicisRight.Text.Trim(), Musculoskeletal_Mmt_AbductorPollicisLeft.Text.Trim(), Musculoskeletal_Mmt_AbductorPollicisRight.Text.Trim(),
              Musculoskeletal_Mmt_ExtensorPollicisLeft.Text.Trim(), Musculoskeletal_Mmt_ExtensorPollicisRight.Text.Trim(), SignOfCNS_NeuromotorControl.Text.Trim(), RemarkVariable_SustainGeneral.Text.Trim(),
              RemarkVariable_PosturalGeneral.Text.Trim(), RemarkVariable_ContractionsGeneral.Text.Trim(), RemarkVariable_AntagonistGeneral.Text.Trim(), RemarkVariable_SynergyGeneral.Text.Trim(),
              RemarkVariable_StiffinessGeneral.Text.Trim(), RemarkVariable_ExtraneousGeneral.Text.Trim(), RemarkVariable_SustainControl.Text.Trim(), RemarkVariable_PosturalControl.Text.Trim(),
              RemarkVariable_ContractionsControl.Text.Trim(), RemarkVariable_AntagonistControl.Text.Trim(), RemarkVariable_SynergyControl.Text.Trim(), RemarkVariable_StiffinessControl.Text.Trim(),
              RemarkVariable_ExtraneousControl.Text.Trim(), RemarkVariable_SustainTiming.Text.Trim(), RemarkVariable_PosturalTiming.Text.Trim(), RemarkVariable_ContractionsTiming.Text.Trim(),
              RemarkVariable_AntagonistTiming.Text.Trim(), RemarkVariable_SynergyTiming.Text.Trim(), RemarkVariable_StiffinessTiming.Text.Trim(), RemarkVariable_ExtraneousTiming.Text.Trim(), SensorySystem_Vision.Text.Trim(),
              SensorySystem_Somatosensory.Text.Trim(), SensorySystem_Vestibular.Text.Trim(), SensorySystem_Auditory.Text.Trim(), SensorySystem_Gustatory.Text.Trim(), SensoryProfile_Profile.Text.Trim(), SIPTInfo_History.Text.Trim(),
              SIPTInfo_HandFunction1_GraspRight.Text.Trim(), SIPTInfo_HandFunction1_GraspLeft.Text.Trim(), SIPTInfo_HandFunction1_SphericalRight.Text.Trim(), SIPTInfo_HandFunction1_SphericalLeft.Text.Trim(),
              SIPTInfo_HandFunction1_HookRight.Text.Trim(), SIPTInfo_HandFunction1_HookLeft.Text.Trim(), SIPTInfo_HandFunction1_JawChuckRight.Text.Trim(), SIPTInfo_HandFunction1_JawChuckLeft.Text.Trim(),
              SIPTInfo_HandFunction1_GripRight.Text.Trim(), SIPTInfo_HandFunction1_GripLeft.Text.Trim(), SIPTInfo_HandFunction1_ReleaseRight.Text.Trim(), SIPTInfo_HandFunction1_ReleaseLeft.Text.Trim(),
              SIPTInfo_HandFunction2_OppositionLfR.Text.Trim(), SIPTInfo_HandFunction2_OppositionLfL.Text.Trim(), SIPTInfo_HandFunction2_OppositionMFR.Text.Trim(),
              SIPTInfo_HandFunction2_OppositionMFL.Text.Trim(), SIPTInfo_HandFunction2_OppositionRFR.Text.Trim(), SIPTInfo_HandFunction2_OppositionRFL.Text.Trim(), SIPTInfo_HandFunction2_PinchLfR.Text.Trim(),
              SIPTInfo_HandFunction2_PinchLfL.Text.Trim(), SIPTInfo_HandFunction2_PinchMFR.Text.Trim(), SIPTInfo_HandFunction2_PinchMFL.Text.Trim(), SIPTInfo_HandFunction2_PinchRFR.Text.Trim(),
              SIPTInfo_HandFunction2_PinchRFL.Text.Trim(), SIPTInfo_SIPT3_Spontaneous.Text.Trim(), SIPTInfo_SIPT3_Command.Text.Trim(), SIPTInfo_SIPT4_Kinesthesia.Text.Trim(), SIPTInfo_SIPT4_Finger.Text.Trim(),
              SIPTInfo_SIPT4_Localisation.Text.Trim(), SIPTInfo_SIPT4_DoubleTactile.Text.Trim(), SIPTInfo_SIPT4_Tactile.Text.Trim(), SIPTInfo_SIPT4_Graphesthesia.Text.Trim(), SIPTInfo_SIPT4_PostRotary.Text.Trim(),
              SIPTInfo_SIPT4_Standing.Text.Trim(), SIPTInfo_SIPT5_Color.Text.Trim(), SIPTInfo_SIPT5_Form.Text.Trim(), SIPTInfo_SIPT5_Size.Text.Trim(), SIPTInfo_SIPT5_Depth.Text.Trim(), SIPTInfo_SIPT5_Figure.Text.Trim(),
              SIPTInfo_SIPT5_Motor.Text.Trim(), SIPTInfo_SIPT6_Design.Text.Trim(), SIPTInfo_SIPT6_Constructional.Text.Trim(), SIPTInfo_SIPT7_Scanning.Text.Trim(), SIPTInfo_SIPT7_Memory.Text.Trim(), SIPTInfo_SIPT8_Postural.Text.Trim(),
              SIPTInfo_SIPT8_Oral.Text.Trim(), SIPTInfo_SIPT8_Sequencing.Text.Trim(), SIPTInfo_SIPT8_Commands.Text.Trim(), SIPTInfo_SIPT9_Bilateral.Text.Trim(), SIPTInfo_SIPT9_Contralat.Text.Trim(),
              SIPTInfo_SIPT9_PreferredHand.Text.Trim(), SIPTInfo_SIPT9_CrossingMidline.Text.Trim(), SIPTInfo_SIPT10_Draw.Text.Trim(), SIPTInfo_SIPT10_ClockFace.Text.Trim(), SIPTInfo_SIPT10_Filtering.Text.Trim(),
              SIPTInfo_SIPT10_MotorPlanning.Text.Trim(), SIPTInfo_SIPT10_BodyImage.Text.Trim(), SIPTInfo_SIPT10_BodySchema.Text.Trim(), SIPTInfo_SIPT10_Laterality.Text.Trim(), SIPTInfo_ActivityGiven_Remark.Text.Trim(),
              SIPTInfo_ActivityGiven_InterestActivity.Text.Trim(), SIPTInfo_ActivityGiven_InterestCompletion.Text.Trim(), SIPTInfo_ActivityGiven_Learning.Text.Trim(), SIPTInfo_ActivityGiven_Complexity.Text.Trim(),
              SIPTInfo_ActivityGiven_ProblemSolving.Text.Trim(), SIPTInfo_ActivityGiven_Concentration.Text.Trim(), SIPTInfo_ActivityGiven_Retension.Text.Trim(), SIPTInfo_ActivityGiven_SpeedPerfom.Text.Trim(),
              SIPTInfo_ActivityGiven_Neatness.Text.Trim(), SIPTInfo_ActivityGiven_Frustation.Text.Trim(), SIPTInfo_ActivityGiven_Work.Text.Trim(), SIPTInfo_ActivityGiven_Reaction.Text.Trim(),
              SIPTInfo_ActivityGiven_SociabilityTherapist.Text.Trim(), SIPTInfo_ActivityGiven_SociabilityStudents.Text.Trim(), Cognition_Intelligence.Text.Trim(), Cognition_Attention.Text.Trim(), Cognition_Memory.Text.Trim(),
              Cognition_Adaptibility.Text.Trim(), Cognition_MotorPlanning.Text.Trim(), Cognition_ExecutiveFunction.Text.Trim(), Cognition_CognitiveFunctions.Text.Trim(), Integumentary_SkinIntegrity.Text.Trim(),
              Integumentary_SkinColor.Text.Trim(), Integumentary_SkinExtensibility.Text.Trim(), Respiratory_RateResting.Text.Trim(), Respiratory_PostExercise.Text.Trim(), Respiratory_Patterns.Text.Trim(), Respiratory_BreathControl.Text.Trim(),
              Cardiovascular_HeartRate.Text.Trim(), Cardiovascular_PostExercise.Text.Trim(), Cardiovascular_BP.Text.Trim(), Cardiovascular_Edema.Text.Trim(), Cardiovascular_Circulation.Text.Trim(), Cardiovascular_EEi.Text.Trim(),
              Gastrointestinal_Bowel.Text.Trim(), Gastrointestinal_Intake.Text.Trim(), Evaluation_Strengths.Text.Trim(), Evaluation_Concern_Barriers.Text.Trim(), Evaluation_Concern_Limitations.Text.Trim(),
              Evaluation_Concern_Posture.Text.Trim(), Evaluation_Concern_Impairment.Text.Trim(), Evaluation_Goal_Summary.Text.Trim(), Evaluation_Goal_Previous.Text.Trim(), Evaluation_Goal_LongTerm.Text.Trim(),
              Evaluation_Goal_ShortTerm.Text.Trim(), Evaluation_Goal_Impairment.Text.Trim(), Evaluation_Plan_Frequency.Text.Trim(), Evaluation_Plan_Service.Text.Trim(), Evaluation_Plan_Strategies.Text.Trim(),
             Evaluation_Plan_Equipment.Text.Trim(), Evaluation_Plan_Education.Text.Trim(), Physioptherapist, Occupational, EnterReport, IsFinal, IsGiven, GivenDate,
             DateTime.UtcNow.AddMinutes(330), _loginID, Praxistest.Text.Trim(), Designcopying.Text.Trim(), ConstructionalPraxis.Text.Trim(), Oralpraxis.Text.Trim(), Posturalpraxis.Text.Trim(), Praxisonverbalcommands.Text.Trim(), Sequencingpraxis.Text.Trim(), Sensoryintegrationtests.Text.Trim(),
             Bilateralmotorcoordination.Text.Trim(), Motoraccuracy.Text.Trim(), Postrotatorynystagmus.Text.Trim(), Standingwalkingbalance.Text.Trim(), Touchtests.Text.Trim(), Graphesthesia.Text.Trim(), Kinesthesia.Text.Trim(), Localizationoftactilestimuli.Text.Trim(),
             Manualformperception.Text.Trim(), Visualperceptiontests.Text.Trim(), Figuregroundperception.Text.Trim(), Spacevisualization.Text.Trim(), Others.Text.Trim(), Clockface.Text.Trim(), Motorplanning.Text.Trim(), Bodyimage.Text.Trim(), Bodyschema.Text.Trim(),
             Laterality.Text.Trim(), SensoryName1.Text.Trim(), Result1.Text.Trim(), SensoryName2.Text.Trim(), Result2.Text.Trim(), SensoryName3.Text.Trim(), Result3.Text.Trim(), SensoryName4.Text.Trim(), Result4.Text.Trim(), SensoryName5.Text.Trim(), Result5.Text.Trim(), SensoryName6.Text.Trim(), Result6.Text.Trim(),
             SensoryName7.Text.Trim(), Result7.Text.Trim(), SensoryName8.Text.Trim(), Result8.Text.Trim(), SensoryName9.Text.Trim(), Result9.Text.Trim(), SensoryName10.Text.Trim(), Result10.Text.Trim(), SensoryName11.Text.Trim(), Result11.Text.Trim(), SensoryName12.Text.Trim(), Result12.Text.Trim(),
            SensoryName13.Text.Trim(), Result13.Text.Trim(), SensoryName14.Text.Trim(), Result14.Text.Trim(), SensoryName15.Text.Trim(), Result15.Text.Trim(), SensoryName16.Text.Trim(), Result16.Text.Trim(), SensoryName17.Text.Trim(), Result17.Text.Trim(), SensoryName18.Text.Trim(),
             Result18.Text.Trim(), SensoryName19.Text.Trim(), Result19.Text.Trim(), SensoryName20.Text.Trim(), Result20.Text.Trim(), SensoryName21.Text.Trim(), Result21.Text.Trim(), SensoryName22.Text.Trim(), Result22.Text.Trim(), SensoryName23.Text.Trim(), Result23.Text.Trim(),
             SensoryName24.Text.Trim(), Result24.Text.Trim(), SensoryName25.Text.Trim(), Result25.Text.Trim(), SensoryName26.Text.Trim(), Result26.Text.Trim(), SensoryName27.Text.Trim(), Result27.Text.Trim(), SensoryName28.Text.Trim(), Result28.Text.Trim(), SensoryName29.Text.Trim(),
             Result29.Text.Trim(), SensoryName30.Text.Trim(), Result30.Text.Trim(), SensoryName31.Text.Trim(), Result31.Text.Trim(), SensoryName32.Text.Trim(), Result32.Text.Trim(), SensoryName33.Text.Trim(), Result33.Text.Trim(), SensoryName34.Text.Trim(), Result34.Text.Trim(),
             SensoryName35.Text.Trim(), Result35.Text.Trim(), SensoryName36.Text.Trim(), Result36.Text.Trim(), SensoryName37.Text.Trim(), Result37.Text.Trim(), SensoryName38.Text.Trim(), Result38.Text.Trim(), SensoryName39.Text.Trim(), Result39.Text.Trim(), SensoryName40.Text.Trim(),
              Result40.Text.Trim(),
              FunctionalAbilities_GrossMotor.Text.Trim(), FunctionalAbilities_FineMotor.Text.Trim(), FunctionalAbilities_Communication.Text.Trim(), FunctionalAbilities_Cognitive.Text.Trim(),
                FunctionalAbilities_Behaviour.Text.Trim(), FunctionalLimitations_GrossMotor.Text.Trim(), FunctionalLimitations_FineMotor.Text.Trim(), FunctionalLimitations_Communication.Text.Trim(),
                FunctionalLimitations_Cognitive.Text.Trim(), FunctionalLimitations_Behaviour.Text.Trim(), ParticipationAbilities_GrossMotor.Text.Trim(), ParticipationAbilities_FineMotor.Text.Trim(),
                ParticipationAbilities_Communication.Text.Trim(), ParticipationAbilities_Cognitive.Text.Trim(), ParticipationAbilities_Behaviour.Text.Trim(), ParticipationLimitations_GrossMotor.Text.Trim(),
                ParticipationLimitations_FineMotor.Text.Trim(), ParticipationLimitations_Communication.Text.Trim(), ParticipationLimitations_Cognitive.Text.Trim(), ParticipationLimitations_Behaviour.Text.Trim(),
                FamilyStru_NoOfCaregivers.Text.Trim(), FamilyStru_TimeWithChild.Text.Trim(), FamilyStru_Holiday.Text.Trim(), FamilyStru_DivoteTime.Text.Trim(), FamilyStru_ContextualFactor.Text.Trim(), FamilyStru_Social.Text.Trim(),
                FamilyStru_Environment.Text.Trim(), FamilyStru_Acceptance.Text.Trim(), FamilyStru_Accessibility.Text.Trim(), FamilyStru_CompareSibling.Text.Trim(), FamilyStru_Working.Text.Trim(), FamilyStru_FamilyPressure.Text.Trim(),
                FamilyStru_SpentMostTime.Text.Trim(), FamilyStru_CloselyInvolved.Text.Trim(), FamilyStru_ChooseFreeTime.Text.Trim(), FamilyStru_PlayWithToys, FamilyStru_ToysExplain.Text.Trim(),
                FamilyStru_ThrowTantrum, SchoolInfo_SchoolType, SchoolInfo_Hours.Text.Trim(), SchoolInfo_Traveling.Text.Trim(), SchoolInfo_Teachers.Text.Trim(), SchoolInfo_SeatingArr.Text.Trim(), SchoolInfo_SeatingTol.Text.Trim(),
                SchoolInfo_MeanTime.Text.Trim(), SchoolInfo_FriendInteraction.Text.Trim(), SchoolInfo_Sports.Text.Trim(), SchoolInfo_Curricular.Text.Trim(), SchoolInfo_Cultural.Text.Trim(), SchoolInfo_ShadowTeacher.Text.Trim(),
                SchoolInfo_RemarkTeacher.Text.Trim(), SchoolInfo_CopyBoard.Text.Trim(), SchoolInfo_CW_HW.Text.Trim(), SchoolInfo_FollowInstru.Text.Trim(), SchoolInfo_SpecialEducator.Text.Trim(), SchoolInfo_DeliveryMode.Text.Trim(),
                SchoolInfo_AcademicScope.Text.Trim(), Behaviour_AtHome.Text.Trim(), Behaviour_AtSchool.Text.Trim(), Behaviour_WithElder.Text.Trim(), Behaviour_WithPeers.Text.Trim(), Behaviour_WithTeacher.Text.Trim(), Behaviour_AtTheMall.Text.Trim(),
                Behaviour_AtPlayground.Text.Trim(), _behaviourPl_Constructive, _behaviourPl_Destructive, BehaviourPl_CD_Remark.Text.Trim(), _behaviourPL_Independant, _behaviourPL_Supervised,
                BehaviourPL_IS_Remark.Text.Trim(), BehaviourPL_Sedentary.Text.Trim(), BehaviourPL_OnTheGo.Text.Trim(), BehaviourPL_AgeAppropriate, BehaviourPL_FollowRule, BehaviourPL_Bullied,
                BehaviourPL_PlayAchieved.Text.Trim(), BehaviourPL_ToyChoice.Text.Trim(), BehaviourPL_Repetitive, BehaviourPL_Versatile, BehaviourPL_PartInGroup, BehaviourPL_IsLeader,
                BehaviourPL_IsFollower, BehaviourPL_PretendPlay.Text.Trim(), Behaviour_RegulatesSelf, Behaviour_BehaveNotReg.Text.Trim(), Behaviour_WhatCalmDown.Text.Trim(), Behaviour_HappyLike.Text.Trim(),
                Behaviour_HappyDislike.Text.Trim(), Arousal_EvalAlert.Text.Trim(), Arousal_GeneralAlert.Text.Trim(), Arousal_StimuliResponse.Text.Trim(), Arousal_Transition.Text.Trim(), Arousal_Optimum.Text.Trim(), Arousal_AlertingFactor.Text.Trim(),
                Arousal_CalmingFactor.Text.Trim(), Attention_InSchool.Text.Trim(), Attention_InHome.Text.Trim(), Attention_Dividing.Text.Trim(), Attention_ChangeActivities.Text.Trim(), Attention_AgeAppropriate, Attention_AttentionSpan.Text.Trim(),
                Attention_Distractibility_Home.Text.Trim(), Attention_Distractibility_School.Text.Trim(), Affect_EmotionRange, Affect_EmotionExpress, Affect_Environment.Text.Trim(), Affect_Task.Text.Trim(), Affect_Individual.Text.Trim(),
                Affect_Consistent.Text.Trim(), Affect_Characterising.Text.Trim(), Action_Planning.Text.Trim(), Action_Purposeful, Action_GoalOriented, Action_FeedbackDependent, Social_KnownPeople.Text.Trim(),
                Social_Strangers.Text.Trim(), Social_Gathering.Text.Trim(), Social_Emotional.Text.Trim(), Social_Appreciates, Social_Reaction.Text.Trim(), Social_Sadness.Text.Trim(), Social_Surprise.Text.Trim(), Social_Shock.Text.Trim(), Social_Friendships,
                Social_Relates.Text.Trim(), Social_ActivitiestheyEnjoy.Text.Trim(), Communication_StartToSpeak.Text.Trim(), Communication_Monosyllables.Text.Trim(), Communication_Bisyllables.Text.Trim(),
                Communication_ShortSentences.Text.Trim(), Communication_LongSentence.Text.Trim(), Communication_UnusualSounds, Communication_ImitationOfSpeech,
                Communication_FacialExpression.Text.Trim(), Communication_EyeContact.Text.Trim(), Communication_Gestures.Text.Trim(), Communication_InterpretationOfLanguage.Text.Trim(),
                Communication_UnderstandImplied.Text.Trim(), Communication_UnderstandJoke.Text.Trim(), Communication_RespondsToName.Text.Trim(), Communication_TwoWayInteraction,
                Communication_NarrateIncidentsHome.Text.Trim(), Communication_NarrateIncidentsSchool.Text.Trim(), Communication_ExpressionsWants.Text.Trim(), Communication_ExpressionsNeeds.Text.Trim(),
                Communication_ExpressionsEmotion.Text.Trim(), Communication_ExpressionsAchive.Text.Trim(), Communication_LanguagSpoken.Text.Trim(), Communication_Echolalia.Text.Trim(),
                RepetitiveInterests_Dominates.Text.Trim(), RepetitiveInterests_Behavior.Text.Trim(), RepetitiveInterests_Changes.Text.Trim(), SensorySystemsVisual_Focal.Text.Trim(), SensorySystemsVisual_Ambient.Text.Trim(),
                SensorySystemsVisual_Focus.Text.Trim(), SensorySystemsVisual_Depth.Text.Trim(), SensorySystemsVisual_Refractive.Text.Trim(), SensorySystemsVisual_Physical.Text.Trim(),
                SensorySystemsVestibula_Seeking.Text.Trim(), SensorySystemsVestibula_Avoiding.Text.Trim(), SensorySystemsVestibula_Insecurities.Text.Trim(), SensorySystemsOromotor_Defensive.Text.Trim(),
                SensorySystemsOromotor_Drooling.Text.Trim(), SensorySystemsOromotor_Mouth.Text.Trim(), SensorySystemsOromotor_Mouthing.Text.Trim(), SensorySystemsOromotor_Chew.Text.Trim(),
                SensorySystemsAuditory_Response.Text.Trim(), SensorySystemsAuditory_Seeking.Text.Trim(), SensorySystemsAuditory_Avoiding.Text.Trim(), SensorySystemsOlfactory_seeking.Text.Trim(),
                SensorySystemsOlfactory_Avoiding.Text.Trim(), SensorySystemsSomatosensory_Seeking.Text.Trim(), SensorySystemsSomatosensory_Avoiding.Text.Trim(),
                SensorySystemsSomatosensoryProprioceptive_BodyImage.Text.Trim(), SensorySystemsSomatosensoryProprioceptive_BodyParts.Text.Trim(),
                SensorySystemsSomatosensoryProprioceptive_Clumsiness.Text.Trim(), SensorySystemsSomatosensoryProprioceptive_Coordination.Text.Trim(), Other_SensoryProfile.Text.Trim(), Other_SIPT.Text.Trim(),
                Other_DCD.Text.Trim(), Other_DSM.Text.Trim(), GoalsAndExpectations.Text.Trim(), SensorySystemsVisual_Comment.Text.Trim(), SensorySystemsVestibula_Comment.Text.Trim(), SensorySystemsOromotor_Comment.Text.Trim(),
                SensorySystemsAuditory_Comment.Text.Trim(), SensorySystemsOlfactory_Comment.Text.Trim(), SensorySystemsSomatosensory_Comment.Text.Trim(),
                DiagnosisIDs, DiagnosisOther
               );
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "SI report saved successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                //  Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
                Response.Redirect(ResolveClientUrl("~/SessionRpt/SiRpt.aspx?record=" + Request.QueryString["record"]), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
    }
}