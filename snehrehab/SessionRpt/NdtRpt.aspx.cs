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
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace snehrehab.SessionRpt
{
    public partial class NdtRpt : System.Web.UI.Page
    {
        public class SelectionMotorControl_Muscle
        {
            public int SR_NO { get; set; }
            public string MUSCLE { get; set; }
            public string RIGHT { get; set; }
            public string LEFT { get; set; }
        }

        public class SelectionMotorControl_MAS
        {
            public int SR_NO { get; set; }
            public string MUSCLE { get; set; }
            public string MAS { get; set; }
        }

        public class Sensory_Profile_NameResults_CL
        {
            public int SR_NO { get; set; }
            public string NAME { get; set; }
            public string RESULTS { get; set; }
        }

        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        int SelectionMotorControl_Muscle_size = 5; int SelectionMotorControl_MAS_Size = 8;
        int Sensory_Profile_NameResults_Size = 40; string Demo = string.Empty; int PatientID = 0;

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

            _cancelUrl = "/SessionRpt/NdtView.aspx";
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
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();

            if (!RDB.IsValid(_appointmentID))
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            }
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
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }            
            List<SelectionMotorControl_Muscle> _selectionMotorControl_Muscle = new List<SelectionMotorControl_Muscle>();
            for (int i = 0; i < SelectionMotorControl_Muscle_size; i++)
            {
                _selectionMotorControl_Muscle.Add(new SelectionMotorControl_Muscle()
                {
                    SR_NO = i + 1,
                    MUSCLE = string.Empty,
                    RIGHT = string.Empty,
                    LEFT = string.Empty,
                });
            }
            txtSelectionMotorControl_Muscle.DataSource = _selectionMotorControl_Muscle;
            txtSelectionMotorControl_Muscle.DataBind();

            List<SelectionMotorControl_MAS> _selectionMotorControl_MAS = new List<SelectionMotorControl_MAS>();
            for (int i = 0; i < SelectionMotorControl_MAS_Size; i++)
            {
                _selectionMotorControl_MAS.Add(new SelectionMotorControl_MAS()
                {
                    SR_NO = i + 1,
                    MUSCLE = string.Empty,
                    MAS = string.Empty,
                });
            }
            txtSelectionMotorControl_MAS.DataSource = _selectionMotorControl_MAS;
            txtSelectionMotorControl_MAS.DataBind();

            List<Sensory_Profile_NameResults_CL> _sensory_Profile_NameResults_CL = new List<Sensory_Profile_NameResults_CL>();
            for (int i = 0; i < Sensory_Profile_NameResults_Size; i++)
            {
                _sensory_Profile_NameResults_CL.Add(new Sensory_Profile_NameResults_CL()
                {
                    SR_NO = i + 1,
                    NAME = string.Empty,
                    RESULTS = string.Empty,
                });
            }
            txtSensory_Profile_NameResults.DataSource = _sensory_Profile_NameResults_CL;
            txtSensory_Profile_NameResults.DataBind();

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
                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=ndt&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
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
                    DataCollection_Referral.Text = ds.Tables[1].Rows[0]["DataCollection_Referral"].ToString();
                    DataCollection_Investigation.Text = ds.Tables[1].Rows[0]["DataCollection_Investigation"].ToString();
                    DataCollection_MedicalHistory.Text = ds.Tables[1].Rows[0]["DataCollection_MedicalHistory"].ToString();
                    DataCollection_DailyRoutine.Text = ds.Tables[1].Rows[0]["DataCollection_DailyRoutine"].ToString();
                    DataCollection_Expectaion.Text = ds.Tables[1].Rows[0]["DataCollection_Expectaion"].ToString();
                    DataCollection_TherapyHistory.Text = ds.Tables[1].Rows[0]["DataCollection_TherapyHistory"].ToString();
                    DataCollection_Sources.Text = ds.Tables[1].Rows[0]["DataCollection_Sources"].ToString();
                    // DataCollection_NumberVisit.Text = ds.Tables[1].Rows[0]["DataCollection_NumberVisit"].ToString();
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

                    Morphology_GirthUpperLimb_Above_ElbowLevel1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel1"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowLevel2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel2"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowLevel3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel3"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowLeft1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft1"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowLeft2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft2"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowLeft3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft3"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowRight1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight1"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowRight2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight2"].ToString();
                    Morphology_GirthUpperLimb_Above_ElbowRight3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight3"].ToString();
                    Morphology_GirthUpperLimb_At_ElbowLevel.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLevel"].ToString();
                    Morphology_GirthUpperLimb_At_ElbowLeft.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLeft"].ToString();
                    Morphology_GirthUpperLimb_At_ElbowRight.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowRight"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLevel1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel1"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLevel2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel2"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLevel3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel3"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLeft1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft1"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLeft2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft2"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowLeft3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft3"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowRight1.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight1"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowRight2.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight2"].ToString();
                    Morphology_GirthUpperLimb_Below_ElbowRight3.Text = ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight3"].ToString();

                    Morphology_GirthLowerLimb_Above_KneeLevel1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel1"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeLevel2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel2"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeLevel3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel3"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeLeft1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft1"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeLeft2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft2"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeLeft3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft3"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeRight1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight1"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeRight2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight2"].ToString();
                    Morphology_GirthLowerLimb_Above_KneeRight3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight3"].ToString();
                    Morphology_GirthLowerLimb_At_KneeLevel.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLevel"].ToString();
                    Morphology_GirthLowerLimb_At_KneeLeft.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLeft"].ToString();
                    Morphology_GirthLowerLimb_At_KneeRight.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeRight"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLevel1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel1"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLevel2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel2"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLevel3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel3"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLeft1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft1"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLeft2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft2"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeLeft3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft3"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeRight1.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight1"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeRight2.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight2"].ToString();
                    Morphology_GirthLowerLimb_Below_KneeRight3.Text = ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight3"].ToString();
                    //Morphology_UpperLimbLevelRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_ABV"].ToString();
                    //Morphology_UpperLimbLevelLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_ABV"].ToString();
                    //Morphology_UpperLimbGirthRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_ABV"].ToString();
                    //Morphology_UpperLimbGirthLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_ABV"].ToString();
                    //Morphology_UpperLimbLevelRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_AT"].ToString();
                    //Morphology_UpperLimbLevelLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_AT"].ToString();
                    //Morphology_UpperLimbGirthRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_AT"].ToString();
                    //Morphology_UpperLimbGirthLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_AT"].ToString();
                    //Morphology_UpperLimbLevelRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_BLW"].ToString();
                    //Morphology_UpperLimbLevelLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_BLW"].ToString();
                    //Morphology_UpperLimbGirthRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_BLW"].ToString();
                    //Morphology_UpperLimbGirthLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_BLW"].ToString();
                    //Morphology_LowerLimbLevelRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_ABV"].ToString();
                    //Morphology_LowerLimbLevelLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_ABV"].ToString();
                    //Morphology_LowerLimbGirthRight_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_ABV"].ToString();
                    //Morphology_LowerLimbGirthLeft_ABV.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_ABV"].ToString();
                    //Morphology_LowerLimbLevelRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_AT"].ToString();
                    //Morphology_LowerLimbLevelLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_AT"].ToString();
                    //Morphology_LowerLimbGirthRight_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_AT"].ToString();
                    //Morphology_LowerLimbGirthLeft_AT.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_AT"].ToString();
                    //Morphology_LowerLimbLevelRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_BLW"].ToString();
                    //Morphology_LowerLimbLevelLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_BLW"].ToString();
                    //Morphology_LowerLimbGirthRight_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_BLW"].ToString();
                    //Morphology_LowerLimbGirthLeft_BLW.Text = ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_BLW"].ToString();
                    Morphology_OralMotorFactors.Text = ds.Tables[1].Rows[0]["Morphology_OralMotorFactors"].ToString();
                    //FunctionalActivities_GrossMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_GrossMotor"].ToString();
                    //FunctionalActivities_HandFunction.Text = ds.Tables[1].Rows[0]["FunctionalActivities_HandFunction"].ToString();
                    //FunctionalActivities_FineMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_FineMotor"].ToString();
                    FunctionalActivities_ADL.Text = ds.Tables[1].Rows[0]["FunctionalActivities_ADL"].ToString();
                    FunctionalActivities_OralMotor.Text = ds.Tables[1].Rows[0]["FunctionalActivities_OralMotor"].ToString();
                    // FunctionalActivities_Communication.Text = ds.Tables[1].Rows[0]["FunctionalActivities_Communication"].ToString();
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
                    Movement_Inertia.Text = ds.Tables[1].Rows[0]["Movement_Inertia"].ToString();
                    Movement_Strategies.Text = ds.Tables[1].Rows[0]["Movement_Strategies"].ToString();
                    Movement_Extremities.Text = ds.Tables[1].Rows[0]["Movement_Extremities"].ToString();
                    if (ds.Tables[1].Rows[0]["Movement_Stability"].ToString().Length > 0)
                    {
                        string[] Movement_Stability_Strs = ds.Tables[1].Rows[0]["Movement_Stability"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int msi = 0; msi < Movement_Stability_Strs.Length; msi++)
                        {
                            for (int msc = 0; msc < Movement_Stability.Items.Count; msc++)
                            {
                                if (Movement_Stability.Items[msc].Value.Equals(Movement_Stability_Strs[msi].Trim(), StringComparison.InvariantCultureIgnoreCase) ||
                                    Movement_Stability.Items[msc].Value.Equals(Movement_Stability_Strs[msi], StringComparison.InvariantCultureIgnoreCase)
                                    )
                                {
                                    Movement_Stability.Items[msc].Selected = true; break;
                                }
                            }
                        }
                    }
                    //Movement_Stability.Text = ds.Tables[1].Rows[0]["Movement_Stability"].ToString();                   
                    if (ds.Tables[1].Rows[0]["Movement_Overuse"].ToString().Length > 0)
                    {
                        string[] Movement_Overuse_Strs = ds.Tables[1].Rows[0]["Movement_Overuse"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int msi = 0; msi < Movement_Overuse_Strs.Length; msi++)
                        {
                            for (int msc = 0; msc < Movement_Overuse.Items.Count; msc++)
                            {
                                if (Movement_Overuse.Items[msc].Value.Equals(Movement_Overuse_Strs[msi].Trim(), StringComparison.InvariantCultureIgnoreCase) ||
                                    Movement_Overuse.Items[msc].Value.Equals(Movement_Overuse_Strs[msi], StringComparison.InvariantCultureIgnoreCase)
                                    )
                                {
                                    Movement_Overuse.Items[msc].Selected = true; break;
                                }
                            }
                        }
                    }
                    //Movement_Overuse.Text = ds.Tables[1].Rows[0]["Movement_Overuse"].ToString();
                    Others_Integration.Text = ds.Tables[1].Rows[0]["Others_Integration"].ToString();
                    Others_Assessments.Text = ds.Tables[1].Rows[0]["Others_Assessments"].ToString();
                    //Regulatory_Arousal.Text = ds.Tables[1].Rows[0]["Regulatory_Arousal"].ToString();
                    //Regulatory_Regulation.Text = ds.Tables[1].Rows[0]["Regulatory_Regulation"].ToString();
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
                    SensorySystem_Auditory.Text = ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString();
                    SensorySystem_Propioceptive.Text = ds.Tables[1].Rows[0]["SensorySystem_Propioceptive"].ToString();
                    SensorySystem_Oromotpor.Text = ds.Tables[1].Rows[0]["SensorySystem_Oromotpor"].ToString();
                    SensorySystem_Vestibular.Text = ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString();
                    SensorySystem_Tactile.Text = ds.Tables[1].Rows[0]["SensorySystem_Tactile"].ToString();
                    SensorySystem_Olfactory.Text = ds.Tables[1].Rows[0]["SensorySystem_Olfactory"].ToString();
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
                    Respiratory_ChestExcursion.Text = ds.Tables[1].Rows[0]["Respiratory_ChestExcursion"].ToString();
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


                    string FunctionalActivities_Cognition = ds.Tables[1].Rows[0]["FunctionalActivities_Cognition"].ToString();
                    ParticipationAbility_GrossMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor"].ToString();
                    ParticipationAbility_FineMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor"].ToString();
                    ParticipationAbility_Communication.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Communication"].ToString();
                    ParticipationAbility_Cognition.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Cognition"].ToString();
                    Contextual_Personal_Positive.Text = ds.Tables[1].Rows[0]["Contextual_Personal_Positive"].ToString();
                    Contextual_Personal_Negative.Text = ds.Tables[1].Rows[0]["Contextual_Personal_Negative"].ToString();
                    Contextual_Enviremental_Positive.Text = ds.Tables[1].Rows[0]["Contextual_Enviremental_Positive"].ToString();
                    Contextual_Enviremental_Negative.Text = ds.Tables[1].Rows[0]["Contextual_Enviremental_Negative"].ToString();
                    Posture_Alignment_Type_1.Checked = false; Posture_Alignment_Type_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Posture_Alignment_Type"].ToString().Equals(Posture_Alignment_Type_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Alignment_Type_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Posture_Alignment_Type"].ToString().Equals(Posture_Alignment_Type_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Alignment_Type_2.Checked = true;
                    }
                    Posture_Gen_Head.Text = ds.Tables[1].Rows[0]["Posture_Gen_Head"].ToString();
                    Posture_Gen_Shoulder.Text = ds.Tables[1].Rows[0]["Posture_Gen_Shoulder"].ToString();
                    Posture_Gen_Ribcage.Text = ds.Tables[1].Rows[0]["Posture_Gen_Ribcage"].ToString();
                    Posture_Gen_Trunk.Text = ds.Tables[1].Rows[0]["Posture_Gen_Trunk"].ToString();
                    Posture_Gen_Pelvis.Text = ds.Tables[1].Rows[0]["Posture_Gen_Pelvis"].ToString();
                    Posture_Gen_Hips.Text = ds.Tables[1].Rows[0]["Posture_Gen_Hips"].ToString();
                    Posture_Gen_Knees.Text = ds.Tables[1].Rows[0]["Posture_Gen_Knees"].ToString();
                    Posture_Gen_Ankle_Feet.Text = ds.Tables[1].Rows[0]["Posture_Gen_Ankle_Feet"].ToString();
                    Posture_Stru_Neck.Text = ds.Tables[1].Rows[0]["Posture_Stru_Neck"].ToString();
                    Posture_Stru_Jaw.Text = ds.Tables[1].Rows[0]["Posture_Stru_Jaw"].ToString();
                    Posture_Stru_Lips.Text = ds.Tables[1].Rows[0]["Posture_Stru_Lips"].ToString();
                    Posture_Stru_Teeth.Text = ds.Tables[1].Rows[0]["Posture_Stru_Teeth"].ToString();
                    Posture_Stru_Tounge.Text = ds.Tables[1].Rows[0]["Posture_Stru_Tounge"].ToString();
                    Posture_Stru_Palate_1.Checked = false; Posture_Stru_Palate_2.Checked = false; Posture_Stru_Palate_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Posture_Stru_Palate"].ToString().Equals(Posture_Stru_Palate_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Stru_Palate_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Posture_Stru_Palate"].ToString().Equals(Posture_Stru_Palate_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Stru_Palate_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Posture_Stru_Palate"].ToString().Equals(Posture_Stru_Palate_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Stru_Palate_3.Checked = true;
                    }
                    Posture_Stru_MouthPosture_1.Checked = false; Posture_Stru_MouthPosture_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Posture_Stru_MouthPosture"].ToString().Equals(Posture_Stru_MouthPosture_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Stru_MouthPosture_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Posture_Stru_MouthPosture"].ToString().Equals(Posture_Stru_MouthPosture_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Posture_Stru_MouthPosture_2.Checked = true;
                    }
                    Posture_Stru_ToungueMove.Text = ds.Tables[1].Rows[0]["Posture_Stru_ToungueMove"].ToString();
                    Posture_Stru_Bite.Text = ds.Tables[1].Rows[0]["Posture_Stru_Bite"].ToString();
                    Posture_Stru_Swallow.Text = ds.Tables[1].Rows[0]["Posture_Stru_Swallow"].ToString();
                    Posture_Stru_Chew.Text = ds.Tables[1].Rows[0]["Posture_Stru_Chew"].ToString();
                    Posture_Stru_Suck.Text = ds.Tables[1].Rows[0]["Posture_Stru_Suck"].ToString();
                    Posture_Stru_BaseSupport.Text = ds.Tables[1].Rows[0]["Posture_Stru_BaseSupport"].ToString();
                    Posture_Stru_CenterOfMass.Text = ds.Tables[1].Rows[0]["Posture_Stru_CenterOfMass"].ToString();
                    string[] Posture_Stru_StrategyForStability_ = ds.Tables[1].Rows[0]["Posture_Stru_StrategyForStability"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int pss = 0; pss < Posture_Stru_StrategyForStability_.Length; pss++)
                    {
                        for (int k = 0; k < Posture_Stru_StrategyForStability.Items.Count; k++)
                        {
                            if (Posture_Stru_StrategyForStability_[pss].Equals(Posture_Stru_StrategyForStability.Items[k].Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                Posture_Stru_StrategyForStability.Items[k].Selected = true;
                                break;
                            }
                        }
                    }
                    Posture_Stru_Anticipatory.Text = ds.Tables[1].Rows[0]["Posture_Stru_Anticipatory"].ToString();
                    Posture_Stru_CounterBalance.Text = ds.Tables[1].Rows[0]["Posture_Stru_CounterBalance"].ToString();
                    Posture_Impairment_Muscle.Text = ds.Tables[1].Rows[0]["Posture_Impairment_Muscle"].ToString();
                    Posture_Impairment_Atrophy.Text = ds.Tables[1].Rows[0]["Posture_Impairment_Atrophy"].ToString();
                    Posture_Impairment_Hypertrophy.Text = ds.Tables[1].Rows[0]["Posture_Impairment_Hypertrophy"].ToString();
                    Posture_Impairment_Callosities.Text = ds.Tables[1].Rows[0]["Posture_Impairment_Callosities"].ToString();
                    Posture_GeneralPosture.Text = ds.Tables[1].Rows[0]["Posture_GeneralPosture"].ToString();

                    Movement_TypeOf_1.Checked = false; Movement_TypeOf_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Movement_TypeOf"].ToString().Equals(Movement_TypeOf_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Movement_TypeOf_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Movement_TypeOf"].ToString().Equals(Movement_TypeOf_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Movement_TypeOf_2.Checked = true;
                    }
                    //Movement_Plane_1.Checked = false; Movement_Plane_2.Checked = false; Movement_Plane_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["Movement_Plane"].ToString().Equals(Movement_Plane_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Movement_Plane_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Movement_Plane"].ToString().Equals(Movement_Plane_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Movement_Plane_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Movement_Plane"].ToString().Equals(Movement_Plane_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Movement_Plane_3.Checked = true;
                    //}
                    Movement_Sagittal.Text = ds.Tables[1].Rows[0]["Movement_Sagittal"].ToString();
                    Movement_Coronal.Text = ds.Tables[1].Rows[0]["Movement_Coronal"].ToString();
                    Movement_Transverse.Text = ds.Tables[1].Rows[0]["Movement_Transverse"].ToString();
                    Movement_WeightShift.Text = ds.Tables[1].Rows[0]["Movement_WeightShift"].ToString();
                    Movement_LimbDissociation.Text = ds.Tables[1].Rows[0]["Movement_LimbDissociation"].ToString();
                    Movement_RangeSpeedOfMovements.Text = ds.Tables[1].Rows[0]["Movement_RangeSpeedOfMovements"].ToString();
                    //if (ds.Tables[1].Rows[0]["Movement_RangeSpeed"].ToString().Length > 0)
                    //{
                    //    string[] Movement_RangeSpeed_Strs = ds.Tables[1].Rows[0]["Movement_RangeSpeed"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //    for (int msi = 0; msi < Movement_RangeSpeed_Strs.Length; msi++)
                    //    {
                    //        for (int msc = 0; msc < Movement_RangeSpeed.Items.Count; msc++)
                    //        {
                    //            if (Movement_RangeSpeed.Items[msc].Value.Equals(Movement_RangeSpeed_Strs[msi].Trim(), StringComparison.InvariantCultureIgnoreCase) ||
                    //                Movement_RangeSpeed.Items[msc].Value.Equals(Movement_RangeSpeed_Strs[msi], StringComparison.InvariantCultureIgnoreCase)
                    //                )
                    //            {
                    //                Movement_RangeSpeed.Items[msc].Selected = true; break;
                    //            }
                    //        }
                    //    }
                    //}
                    //Movement_RangeSpeed.Text = ds.Tables[1].Rows[0]["Movement_RangeSpeed"].ToString();
                    Movement_Balance_Maintain.Text = ds.Tables[1].Rows[0]["Movement_Balance_Maintain"].ToString();
                    Movement_Balance_During.Text = ds.Tables[1].Rows[0]["Movement_Balance_During"].ToString();
                    Movement_Accuracy_Upper.Text = ds.Tables[1].Rows[0]["Movement_Accuracy_Upper"].ToString();
                    Movement_Accuracy_Lower.Text = ds.Tables[1].Rows[0]["Movement_Accuracy_Lower"].ToString();
                    Neuromotor_Recruitment_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Initial"].ToString();
                    Neuromotor_Recruitment_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Sustainance"].ToString();
                    Neuromotor_Recruitment_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Termination"].ToString();
                    Neuromotor_Recruitment_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Control"].ToString();
                    Neuromotor_Contraction_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Contraction_Initial"].ToString();
                    Neuromotor_Contraction_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Contraction_Sustainance"].ToString();
                    Neuromotor_Contraction_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Contraction_Termination"].ToString();
                    Neuromotor_Contraction_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Contraction_Control"].ToString();
                    Neuromotor_Coactivation_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Initial"].ToString();
                    Neuromotor_Coactivation_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Sustainance"].ToString();
                    Neuromotor_Coactivation_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Termination"].ToString();
                    Neuromotor_Coactivation_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Control"].ToString();
                    Neuromotor_Synergy_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Synergy_Initial"].ToString();
                    Neuromotor_Synergy_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Synergy_Sustainance"].ToString();
                    Neuromotor_Synergy_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Synergy_Termination"].ToString();
                    Neuromotor_Synergy_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Synergy_Control"].ToString();
                    Neuromotor_Stiffness_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Initial"].ToString();
                    Neuromotor_Stiffness_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Sustainance"].ToString();
                    Neuromotor_Stiffness_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Termination"].ToString();
                    Neuromotor_Stiffness_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Control"].ToString();
                    Neuromotor_Extraneous_Initial.Text = ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Initial"].ToString();
                    Neuromotor_Extraneous_Sustainance.Text = ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Sustainance"].ToString();
                    Neuromotor_Extraneous_Termination.Text = ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Termination"].ToString();
                    Neuromotor_Extraneous_Control.Text = ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Control"].ToString();
                    OtherTest_Tardieus_TA_Right.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Right"].ToString();
                    OtherTest_Tardieus_TA_Left.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Left"].ToString();
                    OtherTest_Tardieus_Hamstring_Right.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Right"].ToString();
                    OtherTest_Tardieus_Hamstring_Left.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Left"].ToString();
                    OtherTest_Tardieus_Adductor_Right.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Right"].ToString();
                    OtherTest_Tardieus_Adductor_Left.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Left"].ToString();
                    OtherTest_Tardieus_Hip_Right.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Right"].ToString();
                    OtherTest_Tardieus_Hip_Left.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Left"].ToString();
                    OtherTest_Tardieus_Biceps_Right.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Right"].ToString();
                    OtherTest_Tardieus_Biceps_Left.Text = ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Left"].ToString();
                    string SelectionMotorControl_Muscle = ds.Tables[1].Rows[0]["SelectionMotorControl_Muscle"].ToString();
                    if (SelectionMotorControl_Muscle.Length > 0)
                    {
                        List<SelectionMotorControl_Muscle> DL = new List<SelectionMotorControl_Muscle>();
                        try
                        {
                            DL = JsonConvert.DeserializeObject<List<SelectionMotorControl_Muscle>>(SelectionMotorControl_Muscle);
                        }
                        catch
                        {
                        }
                        if (DL == null) { DL = new List<SelectionMotorControl_Muscle>(); }
                        int tmp = SelectionMotorControl_Muscle_size - DL.Count;
                        for (int i = 0; i < tmp; i++)
                        {
                            DL.Add(new SelectionMotorControl_Muscle()
                            {
                                SR_NO = i + 1,
                                MUSCLE = string.Empty,
                                RIGHT = string.Empty,
                                LEFT = string.Empty,
                            });
                        }

                        for (int i = 0; i < DL.Count; i++)
                        {
                            DL[i].SR_NO = (i + 1);
                        }
                        txtSelectionMotorControl_Muscle.DataSource = DL;
                        txtSelectionMotorControl_Muscle.DataBind();
                    }
                    string SelectionMotorControl_Denvers = ds.Tables[1].Rows[0]["SelectionMotorControl_Denvers"].ToString();
                    if (SelectionMotorControl_Denvers.Length > 0)
                    {
                        try
                        {
                            List<dynamic> _selectionMotorControl_Denvers = JsonConvert.DeserializeObject<List<dynamic>>(SelectionMotorControl_Denvers);
                            if (_selectionMotorControl_Denvers != null && _selectionMotorControl_Denvers.Count > 0)
                            {
                                for (int i = 0; i < _selectionMotorControl_Denvers.Count; i++)
                                {
                                    dynamic _denver = _selectionMotorControl_Denvers[i];
                                    if (((string)_denver.n).Equals("gross", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                                    {
                                        SelectionMotorControl_Denvers_Gross.Text = ((string)_denver.t);
                                        // break;
                                    }
                                    if (((string)_denver.n).Equals("fine", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                                    {
                                        SelectionMotorControl_Denvers_Fine.Text = ((string)_denver.t);
                                        // break;
                                    }
                                    if (((string)_denver.n).Equals("communication", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                                    {
                                        SelectionMotorControl_Denvers_Communication.Text = ((string)_denver.t);
                                        //  break;
                                    }
                                    if (((string)_denver.n).Equals("cognition", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                                    {
                                        SelectionMotorControl_Denvers_Cognition.Text = ((string)_denver.t);
                                        // break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    SelectionMotorControl_GMFM.Text = ds.Tables[1].Rows[0]["SelectionMotorControl_GMFM"].ToString();
                    string SelectionMotorControl_MAS = ds.Tables[1].Rows[0]["SelectionMotorControl_MAS"].ToString();
                    if (SelectionMotorControl_MAS.Length > 0)
                    {
                        List<SelectionMotorControl_MAS> DL = new List<SelectionMotorControl_MAS>();
                        try
                        {
                            DL = JsonConvert.DeserializeObject<List<SelectionMotorControl_MAS>>(SelectionMotorControl_MAS);
                        }
                        catch
                        {
                        }
                        if (DL == null) { DL = new List<SelectionMotorControl_MAS>(); }
                        int tmp = SelectionMotorControl_MAS_Size - DL.Count;
                        for (int i = 0; i < tmp; i++)
                        {
                            DL.Add(new SelectionMotorControl_MAS()
                            {
                                SR_NO = i + 1,
                                MUSCLE = string.Empty,
                                MAS = string.Empty,
                            });
                        }
                        for (int i = 0; i < DL.Count; i++)
                        {
                            DL[i].SR_NO = (i + 1);
                        }
                        txtSelectionMotorControl_MAS.DataSource = DL;
                        txtSelectionMotorControl_MAS.DataBind();
                    }
                    SelectionMotorControl_Observation.Text = ds.Tables[1].Rows[0]["SelectionMotorControl_Observation"].ToString();
                    TheFourA_Arousal.Text = ds.Tables[1].Rows[0]["TheFourA_Arousal"].ToString();
                    TheFourA_Attention.Text = ds.Tables[1].Rows[0]["TheFourA_Attention"].ToString();
                    TheFourA_Affect.Text = ds.Tables[1].Rows[0]["TheFourA_Affect"].ToString();
                    TheFourA_Action.Text = ds.Tables[1].Rows[0]["TheFourA_Action"].ToString();
                    TheFourA_StateRegulation.Text = ds.Tables[1].Rows[0]["TheFourA_StateRegulation"].ToString();
                    FA_GrossMotor_Ability.Text = ds.Tables[1].Rows[0]["FA_GrossMotor_Ability"].ToString();
                    FA_GrossMotor_Limit.Text = ds.Tables[1].Rows[0]["FA_GrossMotor_Limit"].ToString();
                    FA_FineMotor_Ability.Text = ds.Tables[1].Rows[0]["FA_FineMotor_Ability"].ToString();
                    FA_FineMotor_Limit.Text = ds.Tables[1].Rows[0]["FA_FineMotor_Limit"].ToString();
                    FA_Communication_Ability.Text = ds.Tables[1].Rows[0]["FA_Communication_Ability"].ToString();
                    FA_Communication_Limit.Text = ds.Tables[1].Rows[0]["FA_Communication_Limit"].ToString();
                    FA_Cognition_Ability.Text = ds.Tables[1].Rows[0]["FA_Cognition_Ability"].ToString();
                    FA_Cognition_Limit.Text = ds.Tables[1].Rows[0]["FA_Cognition_Limit"].ToString();
                    ParticipationAbility_GrossMotor_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor_Limit"].ToString();
                    ParticipationAbility_FineMotor_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor_Limit"].ToString();
                    ParticipationAbility_Communication_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Communication_Limit"].ToString();
                    ParticipationAbility_Cognition_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Cognition_Limit"].ToString();
                    SensoryProfile_Profile.Text = ds.Tables[1].Rows[0]["SensoryProfile_Profile"].ToString();
                    string Sensory_Profile_NameResults = ds.Tables[1].Rows[0]["Sensory_Profile_NameResults"].ToString();
                    if (Sensory_Profile_NameResults.Length > 0)
                    {
                        List<Sensory_Profile_NameResults_CL> DL = new List<Sensory_Profile_NameResults_CL>();
                        try
                        {
                            DL = JsonConvert.DeserializeObject<List<Sensory_Profile_NameResults_CL>>(Sensory_Profile_NameResults);
                        }
                        catch
                        {
                        }
                        if (DL == null) { DL = new List<Sensory_Profile_NameResults_CL>(); }
                        int tmp = Sensory_Profile_NameResults_Size - DL.Count;
                        for (int i = 0; i < tmp; i++)
                        {
                            DL.Add(new Sensory_Profile_NameResults_CL()
                            {
                                SR_NO = i + 1,
                                NAME = string.Empty,
                                RESULTS = string.Empty,
                            });
                        }
                        for (int i = 0; i < DL.Count; i++)
                        {
                            DL[i].SR_NO = (i + 1);
                        }
                        txtSensory_Profile_NameResults.DataSource = DL;
                        txtSensory_Profile_NameResults.DataBind();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
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
            DataSet ds = RDB.Get(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
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
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            if (g < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            }

            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }
            int EnterReport = 0; if (Doctor_EnterReport.SelectedItem != null) { int.TryParse(Doctor_EnterReport.SelectedItem.Value, out EnterReport); }
            string FunctionalActivities_Cognition = string.Empty;
            string Posture_Alignment_Type = string.Empty;
            if (Posture_Alignment_Type_1.Checked) { Posture_Alignment_Type = Posture_Alignment_Type_1.Text.Trim(); }
            if (Posture_Alignment_Type_2.Checked) { Posture_Alignment_Type = Posture_Alignment_Type_2.Text.Trim(); }
            string Posture_Stru_Palate = string.Empty;
            if (Posture_Stru_Palate_1.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_1.Text.Trim(); }
            if (Posture_Stru_Palate_2.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_2.Text.Trim(); }
            if (Posture_Stru_Palate_3.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_3.Text.Trim(); }
            string Posture_Stru_MouthPosture = string.Empty;
            if (Posture_Stru_MouthPosture_1.Checked) { Posture_Stru_MouthPosture = Posture_Stru_MouthPosture_1.Text.Trim(); }
            if (Posture_Stru_MouthPosture_2.Checked) { Posture_Stru_MouthPosture = Posture_Stru_MouthPosture_2.Text.Trim(); }
            string Posture_Stru_StrategyForStability_ = string.Empty;
            for (int k = 0; k < Posture_Stru_StrategyForStability.Items.Count; k++)
            {
                if (Posture_Stru_StrategyForStability.Items[k].Selected)
                {
                    Posture_Stru_StrategyForStability_ += Posture_Stru_StrategyForStability.Items[k].Value + ", ";
                }
            }
            Posture_Stru_StrategyForStability_ = Posture_Stru_StrategyForStability_.Trim();
            if (Posture_Stru_StrategyForStability_.EndsWith(",")) { Posture_Stru_StrategyForStability_ = Posture_Stru_StrategyForStability_.Substring(0, Posture_Stru_StrategyForStability_.Length - 1); }
            string Movement_TypeOf = string.Empty;
            if (Movement_TypeOf_1.Checked) { Movement_TypeOf = Movement_TypeOf_1.Text.Trim(); }
            if (Movement_TypeOf_2.Checked) { Movement_TypeOf = Movement_TypeOf_2.Text.Trim(); }
            string Movement_Plane = string.Empty;
            //if (Movement_Plane_1.Checked) { Movement_Plane = Movement_Plane_1.Text.Trim(); }
            //if (Movement_Plane_2.Checked) { Movement_Plane = Movement_Plane_2.Text.Trim(); }
            //if (Movement_Plane_3.Checked) { Movement_Plane = Movement_Plane_3.Text.Trim(); }


            string _selectionMotorControl_json = string.Empty; int _selectionMotorControl_index = 1; List<SelectionMotorControl_Muscle> _selectionMotorControl_Muscle = new List<SelectionMotorControl_Muscle>();
            foreach (RepeaterItem item in txtSelectionMotorControl_Muscle.Items)
            {
                TextBox SelectionMotorControl_Muscle = item.FindControl("SelectionMotorControl_Muscle") as TextBox;
                TextBox SelectionMotorControl_Right = item.FindControl("SelectionMotorControl_Right") as TextBox;
                TextBox SelectionMotorControl_Left = item.FindControl("SelectionMotorControl_Left") as TextBox;
                if (SelectionMotorControl_Muscle != null && SelectionMotorControl_Right != null && SelectionMotorControl_Left != null)
                {
                    if (SelectionMotorControl_Muscle.Text.Trim().Length > 0 && (SelectionMotorControl_Right.Text.Trim().Length > 0 || SelectionMotorControl_Left.Text.Trim().Length > 0))
                    {
                        _selectionMotorControl_Muscle.Add(new SelectionMotorControl_Muscle()
                        {
                            SR_NO = _selectionMotorControl_index,
                            MUSCLE = SelectionMotorControl_Muscle.Text.Trim(),
                            RIGHT = SelectionMotorControl_Right.Text.Trim(),
                            LEFT = SelectionMotorControl_Left.Text.Trim(),
                        });
                        _selectionMotorControl_index++;
                    }
                }
            }
            _selectionMotorControl_json = JsonConvert.SerializeObject(_selectionMotorControl_Muscle);

            string _selectionMotorControl_MAS_json = string.Empty; int _selectionMotorControl_MAS_index = 1; List<SelectionMotorControl_MAS> _selectionMotorControl_MAS = new List<SelectionMotorControl_MAS>();
            foreach (RepeaterItem item in txtSelectionMotorControl_MAS.Items)
            {
                TextBox SelectionMotorControl_Muscle = item.FindControl("SelectionMotorControl_Muscle") as TextBox;
                TextBox SelectionMotorControl_Right = item.FindControl("SelectionMotorControl_MAS") as TextBox;
                if (SelectionMotorControl_Muscle != null && SelectionMotorControl_Right != null)
                {
                    if (SelectionMotorControl_Muscle.Text.Trim().Length > 0 && SelectionMotorControl_Right.Text.Trim().Length > 0)
                    {
                        _selectionMotorControl_MAS.Add(new SelectionMotorControl_MAS()
                        {
                            SR_NO = _selectionMotorControl_MAS_index,
                            MUSCLE = SelectionMotorControl_Muscle.Text.Trim(),
                            MAS = SelectionMotorControl_Right.Text.Trim(),
                        });
                        _selectionMotorControl_MAS_index++;
                    }
                }
            }
            _selectionMotorControl_MAS_json = JsonConvert.SerializeObject(_selectionMotorControl_MAS);
            string _selectionMotorControl_Denvers = string.Empty;
            var _selectionMotorControl_Denvers_list = new List<dynamic>();
            _selectionMotorControl_Denvers_list.Add(new { n = "gross", t = SelectionMotorControl_Denvers_Gross.Text.Trim(), });
            _selectionMotorControl_Denvers_list.Add(new { n = "fine", t = SelectionMotorControl_Denvers_Fine.Text.Trim(), });
            _selectionMotorControl_Denvers_list.Add(new { n = "communication", t = SelectionMotorControl_Denvers_Communication.Text.Trim(), });
            _selectionMotorControl_Denvers_list.Add(new { n = "cognition", t = SelectionMotorControl_Denvers_Cognition.Text.Trim(), });
            _selectionMotorControl_Denvers = JsonConvert.SerializeObject(_selectionMotorControl_Denvers_list);

            string Sensory_Profile_NameResults = string.Empty; int Sensory_Profile_NameResults_index = 1; List<Sensory_Profile_NameResults_CL> _sensory_Profile_NameResults = new List<Sensory_Profile_NameResults_CL>();
            foreach (RepeaterItem item in txtSensory_Profile_NameResults.Items)
            {
                TextBox txtSensory_Profile_NameResults_Name = item.FindControl("txtSensory_Profile_NameResults_Name") as TextBox;
                TextBox txtSensory_Profile_NameResults_Result = item.FindControl("txtSensory_Profile_NameResults_Result") as TextBox;
                if (txtSensory_Profile_NameResults_Name != null || txtSensory_Profile_NameResults_Result != null)
                {
                    if (txtSensory_Profile_NameResults_Name.Text.Trim().Length > 0 || txtSensory_Profile_NameResults_Result.Text.Trim().Length > 0)
                    {
                        _sensory_Profile_NameResults.Add(new Sensory_Profile_NameResults_CL()
                        {
                            SR_NO = Sensory_Profile_NameResults_index,
                            NAME = txtSensory_Profile_NameResults_Name.Text.Trim(),
                            RESULTS = txtSensory_Profile_NameResults_Result.Text.Trim(),
                        });
                        Sensory_Profile_NameResults_index++;
                    }
                }
            }
            Sensory_Profile_NameResults = JsonConvert.SerializeObject(_sensory_Profile_NameResults);
            string Movement_Stability_Str = string.Empty;
            for (int msi = 0; msi < Movement_Stability.Items.Count; msi++)
            {
                if (Movement_Stability.Items[msi].Selected)
                {
                    Movement_Stability_Str += Movement_Stability.Items[msi].Value + ", ";
                }
            }
            if (!string.IsNullOrEmpty(Movement_Stability_Str))
            {
                Movement_Stability_Str = Movement_Stability_Str.Trim();
                Movement_Stability_Str = Movement_Stability_Str.Substring(0, Movement_Stability_Str.Length - 1);
            }
            string Movement_Overuse_Str = string.Empty;
            for (int msi = 0; msi < Movement_Overuse.Items.Count; msi++)
            {
                if (Movement_Overuse.Items[msi].Selected)
                {
                    Movement_Overuse_Str += Movement_Overuse.Items[msi].Value + ", ";
                }
            }
            if (!string.IsNullOrEmpty(Movement_Overuse_Str))
            {
                Movement_Overuse_Str = Movement_Overuse_Str.Trim();
                Movement_Overuse_Str = Movement_Overuse_Str.Substring(0, Movement_Overuse_Str.Length - 1);
            }
            // string Movement_RangeSpeed_Str = string.Empty;
            //for (int msi = 0; msi < Movement_RangeSpeed.Items.Count; msi++)
            //{
            //    if (Movement_RangeSpeed.Items[msi].Selected)
            //    {
            //        Movement_RangeSpeed_Str += Movement_RangeSpeed.Items[msi].Value + ", ";
            //    }
            //}
            //if (!string.IsNullOrEmpty(Movement_RangeSpeed_Str))
            //{
            //    Movement_RangeSpeed_Str = Movement_RangeSpeed_Str.Trim();
            //    Movement_RangeSpeed_Str = Movement_RangeSpeed_Str.Substring(0, Movement_RangeSpeed_Str.Length - 1);
            //}

            int i = RDB.Set(_appointmentID, DataCollection_Referral.Text.Trim(), DataCollection_Investigation.Text.Trim(), DataCollection_MedicalHistory.Text.Trim(), DataCollection_DailyRoutine.Text.Trim(),
              DataCollection_Expectaion.Text.Trim(), DataCollection_TherapyHistory.Text.Trim(), DataCollection_Sources.Text.Trim(),
              DataCollection_AdaptedEquipment.Text.Trim(), Morphology_Height.Text.Trim(), Morphology_Weight.Text.Trim(), Morphology_LimbLength.Text.Trim(),
              Morphology_LimbLeft.Text.Trim(), Morphology_LimbRight.Text.Trim(), Morphology_ArmLength.Text.Trim(), Morphology_ArmLeft.Text.Trim(), Morphology_ArmRight.Text.Trim(),
              Morphology_Head.Text.Trim(), Morphology_Nipple.Text.Trim(), Morphology_Waist.Text.Trim(),

             Morphology_GirthUpperLimb_Above_ElbowLevel1.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowLevel2.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowLevel3.Text.Trim(),
             Morphology_GirthUpperLimb_Above_ElbowLeft1.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowLeft2.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowLeft3.Text.Trim(),
             Morphology_GirthUpperLimb_Above_ElbowRight1.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowRight2.Text.Trim(), Morphology_GirthUpperLimb_Above_ElbowRight3.Text.Trim(),
             Morphology_GirthUpperLimb_At_ElbowLevel.Text.Trim(), Morphology_GirthUpperLimb_At_ElbowLeft.Text.Trim(), Morphology_GirthUpperLimb_At_ElbowRight.Text.Trim(),
             Morphology_GirthUpperLimb_Below_ElbowLevel1.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowLevel2.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowLevel3.Text.Trim(),
             Morphology_GirthUpperLimb_Below_ElbowLeft1.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowLeft2.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowLeft3.Text.Trim(),
             Morphology_GirthUpperLimb_Below_ElbowRight1.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowRight2.Text.Trim(), Morphology_GirthUpperLimb_Below_ElbowRight3.Text.Trim(),
             Morphology_GirthLowerLimb_Above_KneeLevel1.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeLevel2.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeLevel3.Text.Trim(),
             Morphology_GirthLowerLimb_Above_KneeLeft1.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeLeft2.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeLeft3.Text.Trim(),
             Morphology_GirthLowerLimb_Above_KneeRight1.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeRight2.Text.Trim(), Morphology_GirthLowerLimb_Above_KneeRight3.Text.Trim(),
             Morphology_GirthLowerLimb_At_KneeLevel.Text.Trim(), Morphology_GirthLowerLimb_At_KneeLeft.Text.Trim(), Morphology_GirthLowerLimb_At_KneeRight.Text.Trim(),
             Morphology_GirthLowerLimb_Below_KneeLevel1.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeLevel2.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeLevel3.Text.Trim(),
             Morphology_GirthLowerLimb_Below_KneeLeft1.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeLeft2.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeLeft3.Text.Trim(),
             Morphology_GirthLowerLimb_Below_KneeRight1.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeRight2.Text.Trim(), Morphology_GirthLowerLimb_Below_KneeRight3.Text.Trim(),
                //Morphology_UpperLimbLevelRight_ABV.Text.Trim(), Morphology_UpperLimbLevelLeft_ABV.Text.Trim(),Morphology_UpperLimbGirthRight_ABV.Text.Trim(), Morphology_UpperLimbGirthLeft_ABV.Text.Trim(), Morphology_UpperLimbLevelRight_AT.Text.Trim(),
                //Morphology_UpperLimbLevelLeft_AT.Text.Trim(), Morphology_UpperLimbGirthRight_AT.Text.Trim(), Morphology_UpperLimbGirthLeft_AT.Text.Trim(),
                //Morphology_UpperLimbLevelRight_BLW.Text.Trim(), Morphology_UpperLimbLevelLeft_BLW.Text.Trim(), Morphology_UpperLimbGirthRight_BLW.Text.Trim(),Morphology_UpperLimbGirthLeft_BLW.Text.Trim(), 
                //Morphology_LowerLimbLevelRight_ABV.Text.Trim(), Morphology_LowerLimbLevelLeft_ABV.Text.Trim(),
                //Morphology_LowerLimbGirthRight_ABV.Text.Trim(), Morphology_LowerLimbGirthLeft_ABV.Text.Trim(), Morphology_LowerLimbLevelRight_AT.Text.Trim(),
                //Morphology_LowerLimbLevelLeft_AT.Text.Trim(), Morphology_LowerLimbGirthRight_AT.Text.Trim(), Morphology_LowerLimbGirthLeft_AT.Text.Trim(),
                //Morphology_LowerLimbLevelRight_BLW.Text.Trim(), Morphology_LowerLimbLevelLeft_BLW.Text.Trim(), Morphology_LowerLimbGirthRight_BLW.Text.Trim(), Morphology_LowerLimbGirthLeft_BLW.Text.Trim(),
              Morphology_OralMotorFactors.Text.Trim(),
              FunctionalActivities_ADL.Text.Trim(), FunctionalActivities_OralMotor.Text.Trim(),
               TestMeasures_GMFCS.Text.Trim(), TestMeasures_GMFM.Text.Trim(), TestMeasures_GMPM.Text.Trim(), TestMeasures_AshworthScale.Text.Trim(),
              TestMeasures_TradieusScale.Text.Trim(), TestMeasures_OGS.Text.Trim(), TestMeasures_Melbourne.Text.Trim(), TestMeasures_COPM.Text.Trim(), TestMeasures_ClinicalObservation.Text.Trim(),
              TestMeasures_Others.Text.Trim(),
              Movement_Inertia.Text.Trim(), Movement_Strategies.Text.Trim(), Movement_Extremities.Text.Trim(), Movement_Stability_Str, Movement_Overuse_Str,
              Others_Integration.Text.Trim(), Others_Assessments.Text.Trim(), Musculoskeletal_Rom1_HipFlexionLeft.Text.Trim(),
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
              Musculoskeletal_Mmt_ExtensorPollicisLeft.Text.Trim(), Musculoskeletal_Mmt_ExtensorPollicisRight.Text.Trim(), RemarkVariable_SustainGeneral.Text.Trim(),
              RemarkVariable_PosturalGeneral.Text.Trim(), RemarkVariable_ContractionsGeneral.Text.Trim(), RemarkVariable_AntagonistGeneral.Text.Trim(), RemarkVariable_SynergyGeneral.Text.Trim(),
              RemarkVariable_StiffinessGeneral.Text.Trim(), RemarkVariable_ExtraneousGeneral.Text.Trim(), RemarkVariable_SustainControl.Text.Trim(), RemarkVariable_PosturalControl.Text.Trim(),
              RemarkVariable_ContractionsControl.Text.Trim(), RemarkVariable_AntagonistControl.Text.Trim(), RemarkVariable_SynergyControl.Text.Trim(), RemarkVariable_StiffinessControl.Text.Trim(),
              RemarkVariable_ExtraneousControl.Text.Trim(), RemarkVariable_SustainTiming.Text.Trim(), RemarkVariable_PosturalTiming.Text.Trim(), RemarkVariable_ContractionsTiming.Text.Trim(),
              RemarkVariable_AntagonistTiming.Text.Trim(), RemarkVariable_SynergyTiming.Text.Trim(), RemarkVariable_StiffinessTiming.Text.Trim(), RemarkVariable_ExtraneousTiming.Text.Trim(),
              SensorySystem_Vision.Text.Trim(),
              SensorySystem_Auditory.Text.Trim(), SensorySystem_Propioceptive.Text.Trim(), SensorySystem_Oromotpor.Text.Trim(), SensorySystem_Vestibular.Text.Trim(), SensorySystem_Tactile.Text.Trim(), SensorySystem_Olfactory.Text.Trim(),
              SIPTInfo_History.Text.Trim(),
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
              Integumentary_SkinColor.Text.Trim(), Integumentary_SkinExtensibility.Text.Trim(), Respiratory_RateResting.Text.Trim(), Respiratory_PostExercise.Text.Trim(), Respiratory_Patterns.Text.Trim(), Respiratory_BreathControl.Text.Trim(), Respiratory_ChestExcursion.Text.Trim(),
              Cardiovascular_HeartRate.Text.Trim(), Cardiovascular_PostExercise.Text.Trim(), Cardiovascular_BP.Text.Trim(), Cardiovascular_Edema.Text.Trim(), Cardiovascular_Circulation.Text.Trim(), Cardiovascular_EEi.Text.Trim(),
              Gastrointestinal_Bowel.Text.Trim(), Gastrointestinal_Intake.Text.Trim(), Evaluation_Strengths.Text.Trim(), Evaluation_Concern_Barriers.Text.Trim(), Evaluation_Concern_Limitations.Text.Trim(),
              Evaluation_Concern_Posture.Text.Trim(), Evaluation_Concern_Impairment.Text.Trim(), Evaluation_Goal_Summary.Text.Trim(), Evaluation_Goal_Previous.Text.Trim(), Evaluation_Goal_LongTerm.Text.Trim(),
              Evaluation_Goal_ShortTerm.Text.Trim(), Evaluation_Goal_Impairment.Text.Trim(), Evaluation_Plan_Frequency.Text.Trim(), Evaluation_Plan_Service.Text.Trim(), Evaluation_Plan_Strategies.Text.Trim(),
             Evaluation_Plan_Equipment.Text.Trim(), Evaluation_Plan_Education.Text.Trim(), Physioptherapist, Occupational, EnterReport, IsFinal, IsGiven, GivenDate,
                DateTime.UtcNow.AddMinutes(330), _loginID,
                FunctionalActivities_Cognition, ParticipationAbility_GrossMotor.Text.Trim(), ParticipationAbility_FineMotor.Text.Trim(), ParticipationAbility_Communication.Text.Trim(), ParticipationAbility_Cognition.Text.Trim(),
  Contextual_Personal_Positive.Text.Trim(), Contextual_Personal_Negative.Text.Trim(), Contextual_Enviremental_Positive.Text.Trim(), Contextual_Enviremental_Negative.Text.Trim(), Posture_Alignment_Type,
  Posture_Gen_Head.Text.Trim(), Posture_Gen_Shoulder.Text.Trim(), Posture_Gen_Ribcage.Text.Trim(), Posture_Gen_Trunk.Text.Trim(), Posture_Gen_Pelvis.Text.Trim(), Posture_Gen_Hips.Text.Trim(), Posture_Gen_Knees.Text.Trim(),
  Posture_Gen_Ankle_Feet.Text.Trim(), Posture_Stru_Neck.Text.Trim(), Posture_Stru_Jaw.Text.Trim(), Posture_Stru_Lips.Text.Trim(), Posture_Stru_Teeth.Text.Trim(), Posture_Stru_Tounge.Text.Trim(), Posture_Stru_Palate,
  Posture_Stru_MouthPosture, Posture_Stru_ToungueMove.Text.Trim(), Posture_Stru_Bite.Text.Trim(), Posture_Stru_Swallow.Text.Trim(), Posture_Stru_Chew.Text.Trim(), Posture_Stru_Suck.Text.Trim(),
  Posture_Stru_BaseSupport.Text.Trim(), Posture_Stru_CenterOfMass.Text.Trim(), Posture_Stru_StrategyForStability_, Posture_Stru_Anticipatory.Text.Trim(), Posture_Stru_CounterBalance.Text.Trim(),
  Posture_Impairment_Muscle.Text.Trim(), Posture_Impairment_Atrophy.Text.Trim(), Posture_Impairment_Hypertrophy.Text.Trim(), Posture_Impairment_Callosities.Text.Trim(), Posture_GeneralPosture.Text.Trim(),
  Movement_TypeOf, Movement_Plane, Movement_Sagittal.Text.Trim(), Movement_Coronal.Text.Trim(), Movement_Transverse.Text.Trim(), Movement_WeightShift.Text.Trim(), Movement_LimbDissociation.Text.Trim(), Movement_RangeSpeedOfMovements.Text.Trim(), Movement_Balance_Maintain.Text.Trim(),
  Movement_Balance_During.Text.Trim(), Movement_Accuracy_Upper.Text.Trim(), Movement_Accuracy_Lower.Text.Trim(), Neuromotor_Recruitment_Initial.Text.Trim(), Neuromotor_Recruitment_Sustainance.Text.Trim(),
  Neuromotor_Recruitment_Termination.Text.Trim(), Neuromotor_Recruitment_Control.Text.Trim(), Neuromotor_Contraction_Initial.Text.Trim(), Neuromotor_Contraction_Sustainance.Text.Trim(),
  Neuromotor_Contraction_Termination.Text.Trim(), Neuromotor_Contraction_Control.Text.Trim(), Neuromotor_Coactivation_Initial.Text.Trim(), Neuromotor_Coactivation_Sustainance.Text.Trim(),
  Neuromotor_Coactivation_Termination.Text.Trim(), Neuromotor_Coactivation_Control.Text.Trim(), Neuromotor_Synergy_Initial.Text.Trim(), Neuromotor_Synergy_Sustainance.Text.Trim(),
  Neuromotor_Synergy_Termination.Text.Trim(), Neuromotor_Synergy_Control.Text.Trim(), Neuromotor_Stiffness_Initial.Text.Trim(), Neuromotor_Stiffness_Sustainance.Text.Trim(),
  Neuromotor_Stiffness_Termination.Text.Trim(), Neuromotor_Stiffness_Control.Text.Trim(), Neuromotor_Extraneous_Initial.Text.Trim(), Neuromotor_Extraneous_Sustainance.Text.Trim(),
  Neuromotor_Extraneous_Termination.Text.Trim(), Neuromotor_Extraneous_Control.Text.Trim(), OtherTest_Tardieus_TA_Right.Text.Trim(), OtherTest_Tardieus_TA_Left.Text.Trim(),
  OtherTest_Tardieus_Hamstring_Right.Text.Trim(), OtherTest_Tardieus_Hamstring_Left.Text.Trim(), OtherTest_Tardieus_Adductor_Right.Text.Trim(), OtherTest_Tardieus_Adductor_Left.Text.Trim(),
  OtherTest_Tardieus_Hip_Right.Text.Trim(), OtherTest_Tardieus_Hip_Left.Text.Trim(), OtherTest_Tardieus_Biceps_Right.Text.Trim(), OtherTest_Tardieus_Biceps_Left.Text.Trim(), _selectionMotorControl_json,
  _selectionMotorControl_Denvers, SelectionMotorControl_GMFM.Text.Trim(), _selectionMotorControl_MAS_json, SelectionMotorControl_Observation.Text.Trim(), TheFourA_Arousal.Text.Trim(),
  TheFourA_Attention.Text.Trim(), TheFourA_Affect.Text.Trim(), TheFourA_Action.Text.Trim(), TheFourA_StateRegulation.Text.Trim(),
  FA_GrossMotor_Ability.Text.Trim(), FA_GrossMotor_Limit.Text.Trim(), FA_FineMotor_Ability.Text.Trim(), FA_FineMotor_Limit.Text.Trim(),
     FA_Communication_Ability.Text.Trim(), FA_Communication_Limit.Text.Trim(), FA_Cognition_Ability.Text.Trim(), FA_Cognition_Limit.Text.Trim(), ParticipationAbility_GrossMotor_Limit.Text.Trim(),
     ParticipationAbility_FineMotor_Limit.Text.Trim(), ParticipationAbility_Communication_Limit.Text.Trim(), ParticipationAbility_Cognition_Limit.Text.Trim(),
     Sensory_Profile_NameResults, DiagnosisIDs, DiagnosisOther, SensoryProfile_Profile.Text.Trim());

            if (i > 0)
            {

                Session[DbHelper.Configuration.messageTextSession] = "NDT report saved successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                //Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
                Response.Redirect(ResolveClientUrl("~/SessionRpt/NdtRpt.aspx?record=" + Request.QueryString["record"]), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
            DataSet ds = RDB.Get(_appointmentID);

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            int tabValue = 1;
            SqlCommand cmd = new SqlCommand("ReportNdtMst_Set_TABWISE");
            cmd.CommandType = CommandType.StoredProcedure;

            #region
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
            //DataSet ds = RDB.Get(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);

            //string DiagnosisIDs = "";
            //for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            //{
            //    if (txtDiagnosis.Items[k].Selected)
            //    {
            //        DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
            //    }
            //}
            //string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            //SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            //int g = 0;
            //g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            //if (g < 0)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            //}

            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }
            int EnterReport = 0;
            //if (Doctor_EnterReport.SelectedItem != null) { int.TryParse(Doctor_EnterReport.SelectedItem.Value, out EnterReport); }
            string FunctionalActivities_Cognition = string.Empty;
            string Posture_Alignment_Type = string.Empty;
            if (Posture_Alignment_Type_1.Checked) { Posture_Alignment_Type = Posture_Alignment_Type_1.Text.Trim(); }
            if (Posture_Alignment_Type_2.Checked) { Posture_Alignment_Type = Posture_Alignment_Type_2.Text.Trim(); }
            string Posture_Stru_Palate = string.Empty;
            if (Posture_Stru_Palate_1.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_1.Text.Trim(); }
            if (Posture_Stru_Palate_2.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_2.Text.Trim(); }
            if (Posture_Stru_Palate_3.Checked) { Posture_Stru_Palate = Posture_Stru_Palate_3.Text.Trim(); }
            string Posture_Stru_MouthPosture = string.Empty;
            if (Posture_Stru_MouthPosture_1.Checked) { Posture_Stru_MouthPosture = Posture_Stru_MouthPosture_1.Text.Trim(); }
            if (Posture_Stru_MouthPosture_2.Checked) { Posture_Stru_MouthPosture = Posture_Stru_MouthPosture_2.Text.Trim(); }
            string Movement_TypeOf = string.Empty;
            if (Movement_TypeOf_1.Checked) { Movement_TypeOf = Movement_TypeOf_1.Text.Trim(); }
            if (Movement_TypeOf_2.Checked) { Movement_TypeOf = Movement_TypeOf_2.Text.Trim(); }
            string Movement_Plane = string.Empty;
            //if (Movement_Plane_1.Checked) { Movement_Plane = Movement_Plane_1.Text.Trim(); }
            //if (Movement_Plane_2.Checked) { Movement_Plane = Movement_Plane_2.Text.Trim(); }
            //if (Movement_Plane_3.Checked) { Movement_Plane = Movement_Plane_3.Text.Trim(); }





            string Movement_Stability_Str = string.Empty;
            for (int msi = 0; msi < Movement_Stability.Items.Count; msi++)
            {
                if (Movement_Stability.Items[msi].Selected)
                {
                    Movement_Stability_Str += Movement_Stability.Items[msi].Value + ", ";
                }
            }
            if (!string.IsNullOrEmpty(Movement_Stability_Str))
            {
                Movement_Stability_Str = Movement_Stability_Str.Trim();
                Movement_Stability_Str = Movement_Stability_Str.Substring(0, Movement_Stability_Str.Length - 1);
            }
            string Movement_Overuse_Str = string.Empty;
            for (int msi = 0; msi < Movement_Overuse.Items.Count; msi++)
            {
                if (Movement_Overuse.Items[msi].Selected)
                {
                    Movement_Overuse_Str += Movement_Overuse.Items[msi].Value + ", ";
                }
            }
            if (!string.IsNullOrEmpty(Movement_Overuse_Str))
            {
                Movement_Overuse_Str = Movement_Overuse_Str.Trim();
                Movement_Overuse_Str = Movement_Overuse_Str.Substring(0, Movement_Overuse_Str.Length - 1);
            }
            #endregion
            switch (this.hfdTabs.Value)
            {
                case "":
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab":
                    #region ===== Tab 1 =====

                    tabValue = 1;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@ModifyDate", DateTime.UtcNow.AddMinutes(330));
                    cmd.Parameters.AddWithValue("@ModifyBy", _loginID);
                    cmd.Parameters.AddWithValue("@DataCollection_Referral", @DataCollection_Referral.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_Investigation", @DataCollection_Investigation.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_MedicalHistory", @DataCollection_MedicalHistory.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_DailyRoutine", @DataCollection_DailyRoutine.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_Expectaion", @DataCollection_Expectaion.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_TherapyHistory", @DataCollection_TherapyHistory.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_Sources", @DataCollection_Sources.Text);
                    cmd.Parameters.AddWithValue("@DataCollection_AdaptedEquipment", @DataCollection_AdaptedEquipment.Text);


                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report2_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report2_tab":
                    #region ===== Tab 2 =====
                    tabValue = 2;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Morphology_Height", @Morphology_Height.Text);
                    cmd.Parameters.AddWithValue("@Morphology_Weight", @Morphology_Weight.Text);
                    cmd.Parameters.AddWithValue("@Morphology_LimbLength", Morphology_LimbLength.Text);

                    cmd.Parameters.AddWithValue("@Morphology_LimbLeft", @Morphology_LimbLeft.Text);
                    cmd.Parameters.AddWithValue("@Morphology_LimbRight", @Morphology_LimbRight.Text);
                    cmd.Parameters.AddWithValue("@Morphology_ArmLength", @Morphology_ArmLength.Text);
                    cmd.Parameters.AddWithValue("@Morphology_ArmLeft", @Morphology_ArmLeft.Text);
                    cmd.Parameters.AddWithValue("@Morphology_ArmRight", @Morphology_ArmRight.Text);
                    cmd.Parameters.AddWithValue("@Morphology_Head", @Morphology_Head.Text);
                    cmd.Parameters.AddWithValue("@Morphology_Nipple", @Morphology_Nipple.Text);
                    cmd.Parameters.AddWithValue("@Morphology_Waist", @Morphology_Waist.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLevel1", @Morphology_GirthUpperLimb_Above_ElbowLevel1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLevel2", @Morphology_GirthUpperLimb_Above_ElbowLevel2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLevel3", @Morphology_GirthUpperLimb_Above_ElbowLevel3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLeft1", @Morphology_GirthUpperLimb_Above_ElbowLeft1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLeft2", @Morphology_GirthUpperLimb_Above_ElbowLeft2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowLeft3", @Morphology_GirthUpperLimb_Above_ElbowLeft3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowRight1", @Morphology_GirthUpperLimb_Above_ElbowRight1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowRight2", @Morphology_GirthUpperLimb_Above_ElbowRight2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Above_ElbowRight3", @Morphology_GirthUpperLimb_Above_ElbowRight3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_At_ElbowLevel", @Morphology_GirthUpperLimb_At_ElbowLevel.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_At_ElbowLeft", @Morphology_GirthUpperLimb_At_ElbowLeft.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_At_ElbowRight", @Morphology_GirthUpperLimb_At_ElbowRight.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLevel1", @Morphology_GirthUpperLimb_Below_ElbowLevel1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLevel2", @Morphology_GirthUpperLimb_Below_ElbowLevel2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLevel3", @Morphology_GirthUpperLimb_Below_ElbowLevel3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLeft1", @Morphology_GirthUpperLimb_Below_ElbowLeft1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLeft2", @Morphology_GirthUpperLimb_Below_ElbowLeft2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowLeft3", @Morphology_GirthUpperLimb_Below_ElbowLeft3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowRight1", @Morphology_GirthUpperLimb_Below_ElbowRight1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowRight2", @Morphology_GirthUpperLimb_Below_ElbowRight2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthUpperLimb_Below_ElbowRight3", @Morphology_GirthUpperLimb_Below_ElbowRight3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLevel1", @Morphology_GirthLowerLimb_Above_KneeLevel1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLevel2", @Morphology_GirthLowerLimb_Above_KneeLevel2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLevel3", @Morphology_GirthLowerLimb_Above_KneeLevel3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLeft1", @Morphology_GirthLowerLimb_Above_KneeLeft1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLeft2", @Morphology_GirthLowerLimb_Above_KneeLeft2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeLeft3", @Morphology_GirthLowerLimb_Above_KneeLeft3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeRight1", @Morphology_GirthLowerLimb_Above_KneeRight1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeRight2", @Morphology_GirthLowerLimb_Above_KneeRight2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Above_KneeRight3", @Morphology_GirthLowerLimb_Above_KneeRight3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_At_KneeLevel", @Morphology_GirthLowerLimb_At_KneeLevel.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_At_KneeLeft", @Morphology_GirthLowerLimb_At_KneeLeft.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_At_KneeRight", @Morphology_GirthLowerLimb_At_KneeRight.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLevel1", @Morphology_GirthLowerLimb_Below_KneeLevel1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLevel2", @Morphology_GirthLowerLimb_Below_KneeLevel2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLevel3", @Morphology_GirthLowerLimb_Below_KneeLevel3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLeft1", @Morphology_GirthLowerLimb_Below_KneeLeft1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLeft2", @Morphology_GirthLowerLimb_Below_KneeLeft2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeLeft3", @Morphology_GirthLowerLimb_Below_KneeLeft3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeRight1", @Morphology_GirthLowerLimb_Below_KneeRight1.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeRight2", @Morphology_GirthLowerLimb_Below_KneeRight2.Text);
                    cmd.Parameters.AddWithValue("@Morphology_GirthLowerLimb_Below_KneeRight3", @Morphology_GirthLowerLimb_Below_KneeRight3.Text);
                    cmd.Parameters.AddWithValue("@Morphology_OralMotorFactors", @Morphology_OralMotorFactors.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab":
                    #region ===== Tab 3 =====
                    tabValue = 3;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Ability", @FA_GrossMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Limit", @FA_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Ability", @FA_FineMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Limit", @FA_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Ability", @FA_Communication_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Limit", @FA_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Ability", @FA_Cognition_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Limit", @FA_Cognition_Limit.Text);
                    cmd.Parameters.AddWithValue("@FunctionalActivities_ADL", @FunctionalActivities_ADL.Text);
                    cmd.Parameters.AddWithValue("@FunctionalActivities_OralMotor", @FunctionalActivities_OralMotor.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel19_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel19_tab":
                    #region ===== Tab 4 =====
                    tabValue = 4;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("ParticipationAbility_GrossMotor", @ParticipationAbility_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_GrossMotor_Limit", @ParticipationAbility_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_FineMotor", @ParticipationAbility_FineMotor.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_FineMotor_Limit", @ParticipationAbility_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_Communication", @ParticipationAbility_Communication.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_Communication_Limit", @ParticipationAbility_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_Cognition", @ParticipationAbility_Cognition.Text);
                    cmd.Parameters.AddWithValue("ParticipationAbility_Cognition_Limit", @ParticipationAbility_Cognition_Limit.Text);
                    cmd.Parameters.AddWithValue("Contextual_Personal_Positive", @Contextual_Personal_Positive.Text);
                    cmd.Parameters.AddWithValue("Contextual_Personal_Negative", @Contextual_Personal_Negative.Text);
                    cmd.Parameters.AddWithValue("Contextual_Enviremental_Positive", @Contextual_Enviremental_Positive.Text);
                    cmd.Parameters.AddWithValue("Contextual_Enviremental_Negative", @Contextual_Enviremental_Negative.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab":

                    #region ===== Tab 5 =====
                    tabValue = 5;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@TestMeasures_GMFCS", @TestMeasures_GMFCS.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_GMFM", @TestMeasures_GMFM.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_GMPM", @TestMeasures_GMPM.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_AshworthScale", @TestMeasures_AshworthScale.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_TradieusScale", @TestMeasures_TradieusScale.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_OGS", @TestMeasures_OGS.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_Melbourne", @TestMeasures_Melbourne.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_COPM", @TestMeasures_COPM.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_ClinicalObservation", @TestMeasures_ClinicalObservation.Text);
                    cmd.Parameters.AddWithValue("@TestMeasures_Others", @TestMeasures_Others.Text);


                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab":
                    #region ===== Tab 6 =====
                    string Posture_Stru_StrategyForStability_ = string.Empty;
                    for (int k = 0; k < Posture_Stru_StrategyForStability.Items.Count; k++)
                    {
                        if (Posture_Stru_StrategyForStability.Items[k].Selected)
                        {
                            Posture_Stru_StrategyForStability_ += Posture_Stru_StrategyForStability.Items[k].Value + ", ";
                        }
                    }
                    Posture_Stru_StrategyForStability_ = Posture_Stru_StrategyForStability_.Trim();
                    if (Posture_Stru_StrategyForStability_.EndsWith(",")) { Posture_Stru_StrategyForStability_ = Posture_Stru_StrategyForStability_.Substring(0, Posture_Stru_StrategyForStability_.Length - 1); }

                    tabValue = 6;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Head", Posture_Gen_Head.Text);
                    cmd.Parameters.AddWithValue("@Posture_Alignment_Type", Posture_Alignment_Type);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Shoulder", Posture_Gen_Shoulder.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Ribcage", Posture_Gen_Ribcage.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Trunk", Posture_Gen_Trunk.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Pelvis", Posture_Gen_Pelvis.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Hips", Posture_Gen_Hips.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Knees", Posture_Gen_Knees.Text);
                    cmd.Parameters.AddWithValue("@Posture_Gen_Ankle_Feet", Posture_Gen_Ankle_Feet.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Neck", Posture_Stru_Neck.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Jaw", Posture_Stru_Jaw.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Lips", Posture_Stru_Lips.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Teeth", Posture_Stru_Teeth.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Tounge", Posture_Stru_Tounge.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Palate", Posture_Stru_Palate);
                    cmd.Parameters.AddWithValue("@Posture_Stru_MouthPosture", Posture_Stru_MouthPosture);
                    cmd.Parameters.AddWithValue("@Posture_Stru_ToungueMove", Posture_Stru_ToungueMove.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Bite", Posture_Stru_Bite.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Swallow", Posture_Stru_Swallow.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Chew", Posture_Stru_Chew.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Suck", Posture_Stru_Suck.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_BaseSupport", Posture_Stru_BaseSupport.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_CenterOfMass", Posture_Stru_CenterOfMass.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_StrategyForStability_", Posture_Stru_StrategyForStability_);
                    cmd.Parameters.AddWithValue("@Posture_Stru_Anticipatory", Posture_Stru_Anticipatory.Text);
                    cmd.Parameters.AddWithValue("@Posture_Stru_CounterBalance", Posture_Stru_CounterBalance.Text);
                    cmd.Parameters.AddWithValue("@Posture_Impairment_Muscle", Posture_Impairment_Muscle.Text);
                    cmd.Parameters.AddWithValue("@Posture_Impairment_Atrophy", Posture_Impairment_Atrophy.Text);
                    cmd.Parameters.AddWithValue("@Posture_Impairment_Hypertrophy", Posture_Impairment_Hypertrophy.Text);
                    cmd.Parameters.AddWithValue("@Posture_Impairment_Callosities", Posture_Impairment_Callosities.Text);
                    cmd.Parameters.AddWithValue("@Posture_GeneralPosture", Posture_GeneralPosture.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab":

                    #region ===== Tab 7 =====

                    tabValue = 7;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Movement_Inertia", Movement_Inertia.Text);
                    cmd.Parameters.AddWithValue("@Movement_TypeOf", Movement_TypeOf);
                    cmd.Parameters.AddWithValue("@Movement_Plane", Movement_Plane);
                    cmd.Parameters.AddWithValue("@Movement_Sagittal", Movement_Sagittal.Text);
                    cmd.Parameters.AddWithValue("@Movement_Coronal", Movement_Coronal.Text);
                    cmd.Parameters.AddWithValue("@Movement_Transverse", Movement_Transverse.Text);
                    cmd.Parameters.AddWithValue("@Movement_WeightShift", Movement_WeightShift.Text);
                    cmd.Parameters.AddWithValue("@Movement_LimbDissociation", Movement_LimbDissociation.Text);
                    cmd.Parameters.AddWithValue("@Movement_RangeSpeedOfMovements", Movement_RangeSpeedOfMovements.Text);
                    cmd.Parameters.AddWithValue("@Movement_Balance_Maintain", Movement_Balance_Maintain.Text);
                    cmd.Parameters.AddWithValue("@Movement_Balance_During", Movement_Balance_During.Text);
                    cmd.Parameters.AddWithValue("@Movement_Accuracy_Upper", Movement_Accuracy_Upper.Text);
                    cmd.Parameters.AddWithValue("@Movement_Accuracy_Lower", Movement_Accuracy_Lower.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel20_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel20_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel20_tab":
                    #region ===== Tab 8 =====

                    string _selectionMotorControl_json = string.Empty; int _selectionMotorControl_index = 1; List<SelectionMotorControl_Muscle> _selectionMotorControl_Muscle = new List<SelectionMotorControl_Muscle>();
                    foreach (RepeaterItem item in txtSelectionMotorControl_Muscle.Items)
                    {
                        TextBox SelectionMotorControl_Muscle = item.FindControl("SelectionMotorControl_Muscle") as TextBox;
                        TextBox SelectionMotorControl_Right = item.FindControl("SelectionMotorControl_Right") as TextBox;
                        TextBox SelectionMotorControl_Left = item.FindControl("SelectionMotorControl_Left") as TextBox;
                        if (SelectionMotorControl_Muscle != null && SelectionMotorControl_Right != null && SelectionMotorControl_Left != null)
                        {
                            if (SelectionMotorControl_Muscle.Text.Trim().Length > 0 && (SelectionMotorControl_Right.Text.Trim().Length > 0 || SelectionMotorControl_Left.Text.Trim().Length > 0))
                            {
                                _selectionMotorControl_Muscle.Add(new SelectionMotorControl_Muscle()
                                {
                                    SR_NO = _selectionMotorControl_index,
                                    MUSCLE = SelectionMotorControl_Muscle.Text.Trim(),
                                    RIGHT = SelectionMotorControl_Right.Text.Trim(),
                                    LEFT = SelectionMotorControl_Left.Text.Trim(),
                                });
                                _selectionMotorControl_index++;
                            }
                        }
                    }
                    _selectionMotorControl_json = JsonConvert.SerializeObject(_selectionMotorControl_Muscle);

                    string _selectionMotorControl_MAS_json = string.Empty; int _selectionMotorControl_MAS_index = 1; List<SelectionMotorControl_MAS> _selectionMotorControl_MAS = new List<SelectionMotorControl_MAS>();
                    foreach (RepeaterItem item in txtSelectionMotorControl_MAS.Items)
                    {
                        TextBox SelectionMotorControl_Muscle = item.FindControl("SelectionMotorControl_Muscle") as TextBox;
                        TextBox SelectionMotorControl_Right = item.FindControl("SelectionMotorControl_MAS") as TextBox;
                        if (SelectionMotorControl_Muscle != null && SelectionMotorControl_Right != null)
                        {
                            if (SelectionMotorControl_Muscle.Text.Trim().Length > 0 && SelectionMotorControl_Right.Text.Trim().Length > 0)
                            {
                                _selectionMotorControl_MAS.Add(new SelectionMotorControl_MAS()
                                {
                                    SR_NO = _selectionMotorControl_MAS_index,
                                    MUSCLE = SelectionMotorControl_Muscle.Text.Trim(),
                                    MAS = SelectionMotorControl_Right.Text.Trim(),
                                });
                                _selectionMotorControl_MAS_index++;
                            }
                        }
                    }
                    _selectionMotorControl_MAS_json = JsonConvert.SerializeObject(_selectionMotorControl_MAS);
                    string _selectionMotorControl_Denvers = string.Empty;
                    var _selectionMotorControl_Denvers_list = new List<dynamic>();
                    _selectionMotorControl_Denvers_list.Add(new { n = "gross", t = SelectionMotorControl_Denvers_Gross.Text.Trim(), });
                    _selectionMotorControl_Denvers_list.Add(new { n = "fine", t = SelectionMotorControl_Denvers_Fine.Text.Trim(), });
                    _selectionMotorControl_Denvers_list.Add(new { n = "communication", t = SelectionMotorControl_Denvers_Communication.Text.Trim(), });
                    _selectionMotorControl_Denvers_list.Add(new { n = "cognition", t = SelectionMotorControl_Denvers_Cognition.Text.Trim(), });
                    _selectionMotorControl_Denvers = JsonConvert.SerializeObject(_selectionMotorControl_Denvers_list);

                    tabValue = 8;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Neuromotor_Recruitment_Initial", Neuromotor_Recruitment_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Recruitment_Sustainance", Neuromotor_Recruitment_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Recruitment_Termination", Neuromotor_Recruitment_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Recruitment_Control", Neuromotor_Recruitment_Control.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Contraction_Initial", Neuromotor_Contraction_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Contraction_Sustainance", Neuromotor_Contraction_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Contraction_Termination", Neuromotor_Contraction_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Contraction_Control", Neuromotor_Contraction_Control.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Coactivation_Initial", Neuromotor_Coactivation_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Coactivation_Sustainance", Neuromotor_Coactivation_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Coactivation_Termination", Neuromotor_Coactivation_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Coactivation_Control", Neuromotor_Coactivation_Control.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Synergy_Initial", Neuromotor_Synergy_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Synergy_Sustainance", Neuromotor_Synergy_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Synergy_Termination", Neuromotor_Synergy_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Synergy_Control", Neuromotor_Synergy_Control.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Stiffness_Initial", Neuromotor_Stiffness_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Stiffness_Sustainance", Neuromotor_Stiffness_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Stiffness_Termination", Neuromotor_Stiffness_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Stiffness_Control", Neuromotor_Stiffness_Control.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Extraneous_Initial", Neuromotor_Extraneous_Initial.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Extraneous_Sustainance", Neuromotor_Extraneous_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Extraneous_Termination", Neuromotor_Extraneous_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neuromotor_Extraneous_Control", Neuromotor_Extraneous_Control.Text);

                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_TA_Right", OtherTest_Tardieus_TA_Right.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_TA_Left", OtherTest_Tardieus_TA_Left.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Hamstring_Right", OtherTest_Tardieus_Hamstring_Right.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Hamstring_Left", OtherTest_Tardieus_Hamstring_Left.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Adductor_Right", OtherTest_Tardieus_Adductor_Right.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Adductor_Left", OtherTest_Tardieus_Adductor_Left.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Hip_Right", OtherTest_Tardieus_Hip_Right.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Hip_Left", OtherTest_Tardieus_Hip_Left.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Biceps_Right", OtherTest_Tardieus_Biceps_Right.Text);
                    cmd.Parameters.AddWithValue("@OtherTest_Tardieus_Biceps_Left", OtherTest_Tardieus_Biceps_Left.Text);
                    cmd.Parameters.AddWithValue("@SelectionMotorControl_Observation", SelectionMotorControl_Observation.Text);
                    cmd.Parameters.AddWithValue("@SelectionMotorControl_Muscle", _selectionMotorControl_json);
                    cmd.Parameters.AddWithValue("@SelectionMotorControl_MAS", _selectionMotorControl_MAS_json);
                    cmd.Parameters.AddWithValue("@SelectionMotorControl_GMFM", SelectionMotorControl_GMFM.Text);
                    cmd.Parameters.AddWithValue("@SelectionMotorControl_Denvers", _selectionMotorControl_Denvers);

                    cmd.Parameters.AddWithValue("@TheFourA_Arousal", TheFourA_Arousal.Text);
                    cmd.Parameters.AddWithValue("@TheFourA_Attention", TheFourA_Attention.Text);
                    cmd.Parameters.AddWithValue("@TheFourA_Affect", TheFourA_Affect.Text);
                    cmd.Parameters.AddWithValue("@TheFourA_Action", TheFourA_Action.Text);
                    cmd.Parameters.AddWithValue("@TheFourA_StateRegulation", TheFourA_StateRegulation.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab":
                    #region ===== Tab 9 =====

                    tabValue = 9;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Others_Integration", Others_Integration.Text);
                    cmd.Parameters.AddWithValue("@Others_Assessments", Others_Assessments.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab":
                    #region ===== Tab 10 =====

                    tabValue = 10;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipFlexionLeft", @Musculoskeletal_Rom1_HipFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipFlexionRight", @Musculoskeletal_Rom1_HipFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipExtensionLeft", @Musculoskeletal_Rom1_HipExtensionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipAbductionLeft", @Musculoskeletal_Rom1_HipAbductionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipAbductionRight", @Musculoskeletal_Rom1_HipAbductionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipExtensionRight", @Musculoskeletal_Rom1_HipExtensionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipExternalLeft", @Musculoskeletal_Rom1_HipExternalLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipExternalRight", @Musculoskeletal_Rom1_HipExternalRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipInternalLeft", @Musculoskeletal_Rom1_HipInternalLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_HipInternalRight", @Musculoskeletal_Rom1_HipInternalRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_PoplitealLeft", @Musculoskeletal_Rom1_PoplitealLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_PoplitealRight", @Musculoskeletal_Rom1_PoplitealRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_KneeFlexionLeft", @Musculoskeletal_Rom1_KneeFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_KneeFlexionRight", @Musculoskeletal_Rom1_KneeFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_KneeExtensionLeft", @Musculoskeletal_Rom1_KneeExtensionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_KneeExtensionRight", @Musculoskeletal_Rom1_KneeExtensionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_DorsiflexionFlexionLeft", @Musculoskeletal_Rom1_DorsiflexionFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_DorsiflexionFlexionRight", @Musculoskeletal_Rom1_DorsiflexionFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_DorsiflexionExtensionLeft", @Musculoskeletal_Rom1_DorsiflexionExtensionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_DorsiflexionExtensionRight", @Musculoskeletal_Rom1_DorsiflexionExtensionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_PlantarFlexionLeft", @Musculoskeletal_Rom1_PlantarFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_PlantarFlexionRight", @Musculoskeletal_Rom1_PlantarFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_OthersLeft", @Musculoskeletal_Rom1_OthersLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom1_OthersRight", @Musculoskeletal_Rom1_OthersRight.Text);

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ShoulderFlexionLeft", @Musculoskeletal_Rom2_ShoulderFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ShoulderFlexionRight", @Musculoskeletal_Rom2_ShoulderFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ShoulderExtensionLeft", @Musculoskeletal_Rom2_ShoulderExtensionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ShoulderExtensionRight", @Musculoskeletal_Rom2_ShoulderExtensionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_HorizontalAbductionLeft", @Musculoskeletal_Rom2_HorizontalAbductionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_HorizontalAbductionRight", @Musculoskeletal_Rom2_HorizontalAbductionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ExternalRotationLeft", @Musculoskeletal_Rom2_ExternalRotationLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ExternalRotationRight", @Musculoskeletal_Rom2_ExternalRotationRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_InternalRotationLeft", @Musculoskeletal_Rom2_InternalRotationLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_InternalRotationRight", @Musculoskeletal_Rom2_InternalRotationRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ElbowFlexionLeft", @Musculoskeletal_Rom2_ElbowFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ElbowFlexionRight", @Musculoskeletal_Rom2_ElbowFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ElbowExtensionLeft", @Musculoskeletal_Rom2_ElbowExtensionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_ElbowExtensionRight", @Musculoskeletal_Rom2_ElbowExtensionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_SupinationLeft", @Musculoskeletal_Rom2_SupinationLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_SupinationRight", @Musculoskeletal_Rom2_SupinationRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_PronationLeft", @Musculoskeletal_Rom2_PronationLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_PronationRight", @Musculoskeletal_Rom2_PronationRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_WristFlexionLeft", @Musculoskeletal_Rom2_WristFlexionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_WristFlexionRight", @Musculoskeletal_Rom2_WristFlexionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_WristExtesionLeft", @Musculoskeletal_Rom2_WristExtesionLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_WristExtesionRight", @Musculoskeletal_Rom2_WristExtesionRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_OthersLeft", @Musculoskeletal_Rom2_OthersLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rom2_OthersRight", @Musculoskeletal_Rom2_OthersRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Strengthlp", @Musculoskeletal_Strengthlp.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_StrengthCC", @Musculoskeletal_StrengthCC.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_StrengthMuscle", @Musculoskeletal_StrengthMuscle.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_StrengthSkeletal", @Musculoskeletal_StrengthSkeletal.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_HipflexorsLeft", @Musculoskeletal_Mmt_HipflexorsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_HipflexorsRight", @Musculoskeletal_Mmt_HipflexorsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AbductorsLeft", @Musculoskeletal_Mmt_AbductorsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AbductorsRight", @Musculoskeletal_Mmt_AbductorsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorsLeft", @Musculoskeletal_Mmt_ExtensorsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorsRight", @Musculoskeletal_Mmt_ExtensorsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_HamsLeft", @Musculoskeletal_Mmt_HamsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_HamsRight", @Musculoskeletal_Mmt_HamsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_QuadsLeft", @Musculoskeletal_Mmt_QuadsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_QuadsRight", @Musculoskeletal_Mmt_QuadsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TibialisAnteriorLeft", @Musculoskeletal_Mmt_TibialisAnteriorLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TibialisAnteriorRight", @Musculoskeletal_Mmt_TibialisAnteriorRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TibialisPosteriorLeft", @Musculoskeletal_Mmt_TibialisPosteriorLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TibialisPosteriorRight", @Musculoskeletal_Mmt_TibialisPosteriorRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorDigitorumLeft", @Musculoskeletal_Mmt_ExtensorDigitorumLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorDigitorumRight", @Musculoskeletal_Mmt_ExtensorDigitorumRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorHallucisLeft", @Musculoskeletal_Mmt_ExtensorHallucisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorHallucisRight", @Musculoskeletal_Mmt_ExtensorHallucisRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PeroneiLeft", @Musculoskeletal_Mmt_PeroneiLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PeroneiRight", @Musculoskeletal_Mmt_PeroneiRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorDigitorumLeft", @Musculoskeletal_Mmt_FlexorDigitorumLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorDigitorumRight", @Musculoskeletal_Mmt_FlexorDigitorumRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorHallucisLeft", @Musculoskeletal_Mmt_FlexorHallucisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorHallucisRight", @Musculoskeletal_Mmt_FlexorHallucisRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AnteriorDeltoidLeft", @Musculoskeletal_Mmt_AnteriorDeltoidLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AnteriorDeltoidRight", @Musculoskeletal_Mmt_AnteriorDeltoidRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PosteriorDeltoidLeft", @Musculoskeletal_Mmt_PosteriorDeltoidLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PosteriorDeltoidRight", @Musculoskeletal_Mmt_PosteriorDeltoidRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_MiddleDeltoidLeft", @Musculoskeletal_Mmt_MiddleDeltoidLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_MiddleDeltoidRight", @Musculoskeletal_Mmt_MiddleDeltoidRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SupraspinatusLeft", @Musculoskeletal_Mmt_SupraspinatusLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SupraspinatusRight", @Musculoskeletal_Mmt_SupraspinatusRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SerratusAnteriorLeft", @Musculoskeletal_Mmt_SerratusAnteriorLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SerratusAnteriorRight", @Musculoskeletal_Mmt_SerratusAnteriorRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_RhomboidsLeft", @Musculoskeletal_Mmt_RhomboidsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_RhomboidsRight", @Musculoskeletal_Mmt_RhomboidsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_BicepsLeft", @Musculoskeletal_Mmt_BicepsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_BicepsRight", @Musculoskeletal_Mmt_BicepsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TricepsLeft", @Musculoskeletal_Mmt_TricepsLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_TricepsRight", @Musculoskeletal_Mmt_TricepsRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SupinatorLeft", @Musculoskeletal_Mmt_SupinatorLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_SupinatorRight", @Musculoskeletal_Mmt_SupinatorRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PronatorLeft", @Musculoskeletal_Mmt_PronatorLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_PronatorRight", @Musculoskeletal_Mmt_PronatorRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECULeft", @Musculoskeletal_Mmt_ECULeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECURight", @Musculoskeletal_Mmt_ECURight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECRLeft", @Musculoskeletal_Mmt_ECRLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECRRight", @Musculoskeletal_Mmt_ECRRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECSLeft", @Musculoskeletal_Mmt_ECSLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ECSRight", @Musculoskeletal_Mmt_ECSRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCULeft", @Musculoskeletal_Mmt_FCULeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCURight", @Musculoskeletal_Mmt_FCURight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCRLeft", @Musculoskeletal_Mmt_FCRLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCRRight", @Musculoskeletal_Mmt_FCRRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCSLeft", @Musculoskeletal_Mmt_FCSLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FCSRight", @Musculoskeletal_Mmt_FCSRight.Text);

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_OpponensPollicisLeft", @Musculoskeletal_Mmt_OpponensPollicisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_OpponensPollicisRight", @Musculoskeletal_Mmt_OpponensPollicisRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorPollicisLeft", @Musculoskeletal_Mmt_FlexorPollicisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_FlexorPollicisRight", @Musculoskeletal_Mmt_FlexorPollicisRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AbductorPollicisLeft", @Musculoskeletal_Mmt_AbductorPollicisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_AbductorPollicisRight", @Musculoskeletal_Mmt_AbductorPollicisRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorPollicisLeft", @Musculoskeletal_Mmt_ExtensorPollicisLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExtensorPollicisRight", @Musculoskeletal_Mmt_ExtensorPollicisRight.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab":
                    #region ===== Tab 11 =====

                    tabValue = 11;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SustainGeneral", @RemarkVariable_SustainGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_PosturalGeneral", @RemarkVariable_PosturalGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ContractionsGeneral", @RemarkVariable_ContractionsGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_AntagonistGeneral", @RemarkVariable_AntagonistGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SynergyGeneral", @RemarkVariable_SynergyGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_StiffinessGeneral", @RemarkVariable_StiffinessGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ExtraneousGeneral", @RemarkVariable_ExtraneousGeneral.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SustainControl", @RemarkVariable_SustainControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_PosturalControl", @RemarkVariable_PosturalControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ContractionsControl", @RemarkVariable_ContractionsControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_AntagonistControl", @RemarkVariable_AntagonistControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SynergyControl", @RemarkVariable_SynergyControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_StiffinessControl", @RemarkVariable_StiffinessControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ExtraneousControl", @RemarkVariable_ExtraneousControl.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SustainTiming", @RemarkVariable_SustainTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_PosturalTiming", @RemarkVariable_PosturalTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ContractionsTiming", @RemarkVariable_ContractionsTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_AntagonistTiming", @RemarkVariable_AntagonistTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_SynergyTiming", @RemarkVariable_SynergyTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_StiffinessTiming", @RemarkVariable_StiffinessTiming.Text);
                    cmd.Parameters.AddWithValue("@RemarkVariable_ExtraneousTiming", @RemarkVariable_ExtraneousTiming.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab":
                    #region ===== Tab 12 =====

                    tabValue = 12;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SensorySystem_Vision", @SensorySystem_Vision.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Auditory", @SensorySystem_Auditory.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Propioceptive", @SensorySystem_Propioceptive.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Oromotpor", @SensorySystem_Oromotpor.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Vestibular", @SensorySystem_Vestibular.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Tactile", @SensorySystem_Tactile.Text);
                    cmd.Parameters.AddWithValue("@SensorySystem_Olfactory", @SensorySystem_Olfactory.Text);


                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab":
                    #region ===== Tab 13 =====
                    string Sensory_Profile_NameResults = string.Empty; int Sensory_Profile_NameResults_index = 1; List<Sensory_Profile_NameResults_CL> _sensory_Profile_NameResults = new List<Sensory_Profile_NameResults_CL>();
                    foreach (RepeaterItem item in txtSensory_Profile_NameResults.Items)
                    {
                        TextBox txtSensory_Profile_NameResults_Name = item.FindControl("txtSensory_Profile_NameResults_Name") as TextBox;
                        TextBox txtSensory_Profile_NameResults_Result = item.FindControl("txtSensory_Profile_NameResults_Result") as TextBox;
                        if (txtSensory_Profile_NameResults_Name != null || txtSensory_Profile_NameResults_Result != null)
                        {
                            if (txtSensory_Profile_NameResults_Name.Text.Trim().Length > 0 || txtSensory_Profile_NameResults_Result.Text.Trim().Length > 0)
                            {
                                _sensory_Profile_NameResults.Add(new Sensory_Profile_NameResults_CL()
                                {
                                    SR_NO = Sensory_Profile_NameResults_index,
                                    NAME = txtSensory_Profile_NameResults_Name.Text.Trim(),
                                    RESULTS = txtSensory_Profile_NameResults_Result.Text.Trim(),
                                });
                                Sensory_Profile_NameResults_index++;
                            }
                        }
                    }
                    Sensory_Profile_NameResults = JsonConvert.SerializeObject(_sensory_Profile_NameResults);

                    tabValue = 13;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SensoryProfile_Profile", @SensoryProfile_Profile.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Profile_NameResults", Sensory_Profile_NameResults);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab":
                    #region ===== Tab 14 =====

                    tabValue = 14;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@SIPTInfo_History", @SIPTInfo_History.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GraspRight", @SIPTInfo_HandFunction1_GraspRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GraspLeft", @SIPTInfo_HandFunction1_GraspLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_SphericalRight", @SIPTInfo_HandFunction1_SphericalRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_SphericalLeft", @SIPTInfo_HandFunction1_SphericalLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_HookRight", @SIPTInfo_HandFunction1_HookRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_HookLeft", @SIPTInfo_HandFunction1_HookLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_JawChuckRight", @SIPTInfo_HandFunction1_JawChuckRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_JawChuckLeft", @SIPTInfo_HandFunction1_JawChuckLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GripRight", @SIPTInfo_HandFunction1_GripRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GripLeft", @SIPTInfo_HandFunction1_GripLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_ReleaseRight", @SIPTInfo_HandFunction1_ReleaseRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_ReleaseLeft", @SIPTInfo_HandFunction1_ReleaseLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionLfR", @SIPTInfo_HandFunction2_OppositionLfR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionLfL", @SIPTInfo_HandFunction2_OppositionLfL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionMFR", @SIPTInfo_HandFunction2_OppositionMFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionMFL", @SIPTInfo_HandFunction2_OppositionMFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionRFR", @SIPTInfo_HandFunction2_OppositionRFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionRFL", @SIPTInfo_HandFunction2_OppositionRFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchLfR", @SIPTInfo_HandFunction2_PinchLfR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchLfL", @SIPTInfo_HandFunction2_PinchLfL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchMFR", @SIPTInfo_HandFunction2_PinchMFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchMFL", @SIPTInfo_HandFunction2_PinchMFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchRFR", @SIPTInfo_HandFunction2_PinchRFR.Text);

                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchRFL", @SIPTInfo_HandFunction2_PinchRFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT3_Spontaneous", @SIPTInfo_SIPT3_Spontaneous.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT3_Command", @SIPTInfo_SIPT3_Command.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Kinesthesia", @SIPTInfo_SIPT4_Kinesthesia.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Finger", @SIPTInfo_SIPT4_Finger.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Localisation", @SIPTInfo_SIPT4_Localisation.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_DoubleTactile", @SIPTInfo_SIPT4_DoubleTactile.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Tactile", @SIPTInfo_SIPT4_Tactile.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Graphesthesia", @SIPTInfo_SIPT4_Graphesthesia.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_PostRotary", @SIPTInfo_SIPT4_PostRotary.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Standing", @SIPTInfo_SIPT4_Standing.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Color", @SIPTInfo_SIPT5_Color.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Form", @SIPTInfo_SIPT5_Form.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Size", @SIPTInfo_SIPT5_Size.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Depth", @SIPTInfo_SIPT5_Depth.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Figure", @SIPTInfo_SIPT5_Figure.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Motor", @SIPTInfo_SIPT5_Motor.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT6_Design", @SIPTInfo_SIPT6_Design.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT6_Constructional", @SIPTInfo_SIPT6_Constructional.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT7_Scanning", @SIPTInfo_SIPT7_Scanning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT7_Memory", @SIPTInfo_SIPT7_Memory.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Postural", @SIPTInfo_SIPT8_Postural.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Oral", @SIPTInfo_SIPT8_Oral.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Sequencing", @SIPTInfo_SIPT8_Sequencing.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Commands", @SIPTInfo_SIPT8_Commands.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_Bilateral", @SIPTInfo_SIPT9_Bilateral.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_Contralat", @SIPTInfo_SIPT9_Contralat.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_PreferredHand", @SIPTInfo_SIPT9_PreferredHand.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_CrossingMidline", @SIPTInfo_SIPT9_CrossingMidline.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Draw", @SIPTInfo_SIPT10_Draw.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_ClockFace", @SIPTInfo_SIPT10_ClockFace.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Filtering", @SIPTInfo_SIPT10_Filtering.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_MotorPlanning", @SIPTInfo_SIPT10_MotorPlanning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_BodyImage", @SIPTInfo_SIPT10_BodyImage.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_BodySchema", @SIPTInfo_SIPT10_BodySchema.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Laterality", @SIPTInfo_SIPT10_Laterality.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Remark", @SIPTInfo_ActivityGiven_Remark.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_InterestActivity", @SIPTInfo_ActivityGiven_InterestActivity.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_InterestCompletion", @SIPTInfo_ActivityGiven_InterestCompletion.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Learning", @SIPTInfo_ActivityGiven_Learning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Complexity", @SIPTInfo_ActivityGiven_Complexity.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_ProblemSolving", @SIPTInfo_ActivityGiven_ProblemSolving.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Concentration", @SIPTInfo_ActivityGiven_Concentration.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Retension", @SIPTInfo_ActivityGiven_Retension.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SpeedPerfom", @SIPTInfo_ActivityGiven_SpeedPerfom.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Neatness", @SIPTInfo_ActivityGiven_Neatness.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Frustation", @SIPTInfo_ActivityGiven_Frustation.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Work", @SIPTInfo_ActivityGiven_Work.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Reaction", @SIPTInfo_ActivityGiven_Reaction.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SociabilityTherapist", @SIPTInfo_ActivityGiven_SociabilityTherapist.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SociabilityStudents", @SIPTInfo_ActivityGiven_SociabilityStudents.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab":
                    #region ===== Tab 15 =====

                    tabValue = 15;


                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Cognition_Intelligence", @Cognition_Intelligence.Text);
                    cmd.Parameters.AddWithValue("@Cognition_Attention", @Cognition_Attention.Text);
                    cmd.Parameters.AddWithValue("@Cognition_Memory", @Cognition_Memory.Text);
                    cmd.Parameters.AddWithValue("@Cognition_Adaptibility", @Cognition_Adaptibility.Text);
                    cmd.Parameters.AddWithValue("@Cognition_MotorPlanning", @Cognition_MotorPlanning.Text);
                    cmd.Parameters.AddWithValue("@Cognition_ExecutiveFunction", @Cognition_ExecutiveFunction.Text);
                    cmd.Parameters.AddWithValue("@Cognition_CognitiveFunctions", @Cognition_CognitiveFunctions.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab":
                    #region ===== Tab 16 =====

                    tabValue = 16;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Integumentary_SkinIntegrity", @Integumentary_SkinIntegrity.Text);
                    cmd.Parameters.AddWithValue("@Integumentary_SkinColor", @Integumentary_SkinColor.Text);
                    cmd.Parameters.AddWithValue("@Integumentary_SkinExtensibility", @Integumentary_SkinExtensibility.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab":
                    #region ===== Tab 17 =====

                    tabValue = 17;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Respiratory_RateResting", @Respiratory_RateResting.Text);
                    cmd.Parameters.AddWithValue("@Respiratory_PostExercise", @Respiratory_PostExercise.Text);
                    cmd.Parameters.AddWithValue("@Respiratory_Patterns", @Respiratory_Patterns.Text);
                    cmd.Parameters.AddWithValue("@Respiratory_BreathControl", @Respiratory_BreathControl.Text);
                    cmd.Parameters.AddWithValue("@Respiratory_ChestExcursion", @Respiratory_ChestExcursion.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab";
                    }
                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab":
                    #region ===== Tab 18 =====

                    tabValue = 18;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Cardiovascular_HeartRate", @Cardiovascular_HeartRate.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular_PostExercise", @Cardiovascular_PostExercise.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular_BP", @Cardiovascular_BP.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular_Edema", @Cardiovascular_Edema.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular_Circulation", @Cardiovascular_Circulation.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular_EEi", @Cardiovascular_EEi.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab":
                    #region ===== Tab 19 =====

                    tabValue = 19;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Gastrointestinal_Bowel", @Gastrointestinal_Bowel.Text);
                    cmd.Parameters.AddWithValue("@Gastrointestinal_Intake", @Gastrointestinal_Intake.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab":
                    #region ===== Tab 20 =====

                    tabValue = 20;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Evaluation_Strengths", @Evaluation_Strengths.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Barriers", @Evaluation_Concern_Barriers.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Limitations", @Evaluation_Concern_Limitations.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Posture", @Evaluation_Concern_Posture.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Impairment", @Evaluation_Concern_Impairment.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Summary", @Evaluation_Goal_Summary.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Previous", @Evaluation_Goal_Previous.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_LongTerm", @Evaluation_Goal_LongTerm.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_ShortTerm", @Evaluation_Goal_ShortTerm.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Impairment", @Evaluation_Goal_Impairment.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Frequency", @Evaluation_Plan_Frequency.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Service", @Evaluation_Plan_Service.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Strategies", @Evaluation_Plan_Strategies.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Equipment", @Evaluation_Plan_Equipment.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Education", @Evaluation_Plan_Education.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab":
                    #region ===== Tab 21 =====

                    tabValue = 21;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Doctor_Physioptherapist", @Doctor_Physioptherapist.Text);
                    cmd.Parameters.AddWithValue("@Doctor_Occupational", @Doctor_Occupational.Text);
                    //cmd.Parameters.AddWithValue("@Doctor_EnterReport", @Doctor_EnterReport.Text);
                    cmd.Parameters.AddWithValue("@Doctor_EnterReport", DBNull.Value);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue - 21;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab";
                    }
                    #endregion
                    break;
            }
        }


    }
}
