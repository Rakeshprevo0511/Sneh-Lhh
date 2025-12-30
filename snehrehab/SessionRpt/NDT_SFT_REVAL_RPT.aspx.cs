using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.SessionRpt
{
    public partial class NDT_SFT_REVAL_RPT : System.Web.UI.Page
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
        SqlConnection conn = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            DbHelper.SqlDb objSqlDb = new DbHelper.SqlDb();
            conn = objSqlDb.DbConnection();
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }

            if (SnehBLL.UserAccount_Bll.getCategory() == 3)
            {
                sessview.Visible = true;

            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 1)
            {
                rptview.Visible = true;
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
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
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            Doctor_Physioptherapist.Items.Clear(); Doctor_Physioptherapist.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_Occupational.Items.Clear(); Doctor_Occupational.Items.Add(new ListItem("Select Doctor", "-1"));
            //Doctor_EnterReport.Items.Clear(); Doctor_EnterReport.Items.Add(new ListItem("Select Doctor", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                Doctor_Physioptherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_Occupational.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                //Doctor_EnterReport.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }


            string que = "SELECT 'Select Month' as Month, '0' as MONTHS UNION ALL SELECT distinct cast(MONTHS as varchar) + ' Month' as Month, MONTHS FROM QUESTIONNAIRE_SI order by MONTHS";
            SqlCommand cmd = new SqlCommand(que, conn);
            conn.Open();
            DataSet dss = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dss);
            conn.Close();

            SelectMonth.DataSource = dss.Tables[0];
            SelectMonth.DataTextField = "Month";
            SelectMonth.DataValueField = "MONTHS";
            SelectMonth.DataBind();

            string que1 = "SELECT 'Select Month' as Month_Caption, 0 as MonthsQ, 0 as Sort_Order UNION ALL SELECT DISTINCT cast(Month_Caption as varchar), MonthsQ, Sort_Order FROM ABILITY_CHECKLIST_New ORDER BY Sort_Order";
            SqlCommand cmd1 = new SqlCommand(que1, conn);
            conn.Open();
            DataSet dss1 = new DataSet();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dss1);
            conn.Close();

            //MonthSelect.Items.Clear();
            //MonthSelect.Items.Add("select");
            //MonthSelect.Items[0].Value = "0";
            //MonthSelect.SelectedIndex = 0;
            MonthSelect.DataSource = dss1.Tables[0];
            MonthSelect.DataTextField = "Month_Caption";
            MonthSelect.DataValueField = "MonthsQ";
            MonthSelect.DataBind();
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

            DataSet ds = RDB.Get_NDT(_appointmentID);
            if (ds.Tables.Count > 0)
            {
                List<ListItem> selected = new List<ListItem>();
                bool HasDiagnosisID = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPatient.Text = ds.Tables[0].Rows[0]["FullName"].ToString();
                    txtSession.Text = ds.Tables[0].Rows[0]["SessionName"].ToString();
                    int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
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

                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=ndt_new&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
                    #region*****Movemmetn***
                    Multi_Movement_TypeOf_1.Checked = false; Multi_Movement_TypeOf_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Multi_Movement_TypeOf_Quality"].ToString().Equals(Multi_Movement_TypeOf_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Multi_Movement_TypeOf_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Multi_Movement_TypeOf_Quality"].ToString().Equals(Multi_Movement_TypeOf_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Multi_Movement_TypeOf_2.Checked = true;
                    }
                    Multi_Movement_Sagittal.Checked = false; Multi_Movement_Coronal.Checked = false; Multi_Movement_Frontal.Checked = false;

                    string movementType = ds.Tables[1].Rows[0]["Multi_Movement_Plane"].ToString().Trim();
                    if (movementType.Equals(Multi_Movement_Sagittal.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Multi_Movement_Sagittal.Checked = true;
                    }
                    if (movementType.Equals(Multi_Movement_Coronal.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Multi_Movement_Coronal.Checked = true;
                    }
                    if (movementType.Equals(Multi_Movement_Frontal.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Multi_Movement_Frontal.Checked = true;
                    }

                    if (ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0]["Multi_Movement_WeightShift"] != DBNull.Value)
                    {
                        string selectedValues = ds.Tables[1].Rows[0]["Multi_Movement_WeightShift"].ToString();

                        if (!string.IsNullOrEmpty(selectedValues))
                        {
                            string[] values = selectedValues.Split(',');

                            // Loop through CheckBoxList items and match with stored values
                            foreach (ListItem item in Movement_WeightShift.Items)
                            {
                                if (values.Contains(item.Value.Trim()))
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    Movement_Balance_Maintain.Text = ds.Tables[1].Rows[0]["Multi_Movement_Bal_maintain"].ToString();
                    Movement_Balance_During.Text = ds.Tables[1].Rows[0]["Multi_Movement_BAl_during"].ToString();
                    Movement_Inertia.Text = ds.Tables[1].Rows[0]["Movement_Inertia"].ToString();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        void BindCheckboxes(string columnName, params CheckBox[] checkboxes)
                        {
                            if (ds.Tables[1].Rows[0][columnName] != DBNull.Value)
                            {
                                string positions = ds.Tables[1].Rows[0][columnName].ToString();
                                if (!string.IsNullOrEmpty(positions))
                                {
                                    string[] values = positions.Split(',');
                                    foreach (var checkbox in checkboxes)
                                    {
                                        checkbox.Checked = values.Contains(checkbox.Text.Trim());
                                    }
                                }
                            }
                        }

                        // Binding checkboxes for all posture parameters
                        BindCheckboxes("Multi_Movement_interlimb", Movement_Interlimb_SpineToShoulder, Movement_Interlimb_Scapulohumeral, Movement_Interlimb_Pelvifemoral, Movement_Interlimb_WithinUL, Movement_Interlimb_WithinLL);
                        BindCheckboxes("Multi_Movement_intralimb", Movement_Intralimb_LE, Movement_Intralimb_UE, Movement_Intralimb_Spine);
                        BindCheckboxes("Multi_Movement_overuse", chkLeanMuscle, chkLockingJoints, chkBroadBOS, chkGeneralPosture);
                        BindCheckboxes("UpperLimb_Movement", Movement_UpperLimb_Inner, Movement_UpperLimb_Mid, Movement_UpperLimb_Outer);
                        BindCheckboxes("LowerLimb_Movement", Movement_LowerLimb_Inner, Movement_LowerLimb_Mid, Movement_LowerLimb_Outer);
                        BindCheckboxes("CervicalSpine_Movement", Movement_CervicalSpine_Inner, Movement_CervicalSpine_Mid, Movement_CervicalSpine_Outer);
                        BindCheckboxes("ThoracicSpine_Movement", Movement_ThoracicSpine_Inner, Movement_ThoracicSpine_Mid, Movement_ThoracicSpine_Outer);
                        BindCheckboxes("Multi_Movement_statbilty", chkOveruseMomentum, chkIncreasedBOS, chkIncreasingPosturalTone);
                    }
                    Gene_obsr_comments_txt.Text = ds.Tables[1].Rows[0]["Gene_obsr_comments"].ToString();

                    #endregion

                    FA_GrossMotor_Ability.Text = ds.Tables[1].Rows[0]["FA_GrossMotor_Ability"].ToString();
                    FA_GrossMotor_Limit.Text = ds.Tables[1].Rows[0]["FA_GrossMotor_Limit"].ToString();
                    FA_FineMotor_Ability.Text = ds.Tables[1].Rows[0]["FA_FineMotor_Ability"].ToString();
                    FA_FineMotor_Limit.Text = ds.Tables[1].Rows[0]["FA_FineMotor_Limit"].ToString();
                    FA_Communication_Ability.Text = ds.Tables[1].Rows[0]["FA_Communication_Ability"].ToString();
                    FA_Communication_Limit.Text = ds.Tables[1].Rows[0]["FA_Communication_Limit"].ToString();
                    FA_Cognition_Ability.Text = ds.Tables[1].Rows[0]["FA_Cognition_Ability"].ToString();
                    FA_Cognition_Limit.Text = ds.Tables[1].Rows[0]["FA_Cognition_Limit"].ToString();
                    ParticipationAbility_GrossMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor"].ToString();
                    ParticipationAbility_GrossMotor_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor_Limit"].ToString();
                    ParticipationAbility_FineMotor.Text = ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor"].ToString();
                    ParticipationAbility_FineMotor_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor_Limit"].ToString();
                    ParticipationAbility_Communication.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Communication"].ToString();
                    ParticipationAbility_Communication_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Communication_Limit"].ToString();
                    ParticipationAbility_Cognition.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Cognition"].ToString();
                    ParticipationAbility_Cognition_Limit.Text = ds.Tables[1].Rows[0]["ParticipationAbility_Cognition_Limit"].ToString();
                    Contextual_Personal_Positive.Text = ds.Tables[1].Rows[0]["Contextual_Personal_Positive"].ToString();
                    Contextual_Personal_Negative.Text = ds.Tables[1].Rows[0]["Contextual_Personal_Negative"].ToString();
                    Contextual_Enviremental_Positive.Text = ds.Tables[1].Rows[0]["Contextual_Environmental_Positive"].ToString();
                    Contextual_Enviremental_Negative.Text = ds.Tables[1].Rows[0]["Contextual_Environmental_Negative"].ToString();

                    chkSoinePoor.Checked = ds.Tables[1].Rows[0]["txtSoinePoor"].ToString() == "Poor";
                    chkSoineFair.Checked = ds.Tables[1].Rows[0]["txtSoineFair"].ToString() == "Fair";
                    chkSoineGood.Checked = ds.Tables[1].Rows[0]["txtSoineGood"].ToString() == "Good";

                    chkScapuloPoor.Checked = ds.Tables[1].Rows[0]["txtScapuloPoor"].ToString() == "Poor";
                    chkScapuloFair.Checked = ds.Tables[1].Rows[0]["txtScapuloFair"].ToString() == "Fair";
                    chkScapuloGood.Checked = ds.Tables[1].Rows[0]["txtScapuloGood"].ToString() == "Good";

                    chkPelviPoor.Checked = ds.Tables[1].Rows[0]["txtPelviPoor"].ToString() == "Poor";
                    chkPelviFair.Checked = ds.Tables[1].Rows[0]["txtPelviFair"].ToString() == "Fair";
                    chkPelviGood.Checked = ds.Tables[1].Rows[0]["txtPelviGood"].ToString() == "Good";

                    chkWithinUlPoor.Checked = ds.Tables[1].Rows[0]["txtWithinUlPoor"].ToString() == "Poor";
                    chkWithinUlFair.Checked = ds.Tables[1].Rows[0]["txtWithinUlFair"].ToString() == "Fair";
                    chkWithinUlGood.Checked = ds.Tables[1].Rows[0]["txtWithinUlGood"].ToString() == "Good";

                    chkWithinLlPoor.Checked = ds.Tables[1].Rows[0]["txtWithinLlPoor"].ToString() == "Poor";
                    chkWithinLlFair.Checked = ds.Tables[1].Rows[0]["txtWithinLlFair"].ToString() == "Fair";
                    chkWithinLlGood.Checked = ds.Tables[1].Rows[0]["txtWithinLlGood"].ToString() == "Good";

                    #region ***Neurometer**
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

                    string SelectionMotorControl_Muscle = ds.Tables[1].Rows[0]["SelectionMotorControl_Muscle"].ToString();

                    Neurometer_Initialigy_initial.Text = ds.Tables[1].Rows[0]["Neurometer_Initialigy_initial"].ToString();
                    Neurometer_Initialigy_Sustainance.Text = ds.Tables[1].Rows[0]["Neurometer_Initialigy_Sustainance"].ToString();
                    Neurometer_Initialigy_Termination.Text = ds.Tables[1].Rows[0]["Neurometer_Initialigy_Termination"].ToString();
                    Neurometer_Initialigy_Control.Text = ds.Tables[1].Rows[0]["Neurometer_Initialigy_Control"].ToString();

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
                    #endregion
                    #region*****************Morphology**********************
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
                    #endregion
                    #region*****Denver****
                    TestMeassures_GrossMotor.Text = ds.Tables[1].Rows[0]["TestMeassures_GrossMotor"].ToString();
                    TestMeassures_FineMotor.Text = ds.Tables[1].Rows[0]["TestMeassures_FineMotor"].ToString();
                    TestMeassures_DenverLanguage.Text = ds.Tables[1].Rows[0]["TestMeassures_DenverLanguage"].ToString();
                    TestMeassures_DenverPersonal.Text = ds.Tables[1].Rows[0]["TestMeassures_DenverPersonal"].ToString();
                    Tests_cmt.Text = ds.Tables[1].Rows[0]["Tests_cmt"].ToString();
                    #endregion
                    #region**********sensory Profile******
                    General_Processing.Text = ds.Tables[1].Rows[0]["General_Processing"].ToString();
                    AUDITORY_Processing.Text = ds.Tables[1].Rows[0]["AUDITORY_Processing"].ToString();
                    VISUAL_Processing.Text = ds.Tables[1].Rows[0]["VISUAL_Processing"].ToString();
                    TOUCH_Processing.Text = ds.Tables[1].Rows[0]["TOUCH_Processing"].ToString();
                    MOVEMENT_Processing.Text = ds.Tables[1].Rows[0]["MOVEMENT_Processing"].ToString();
                    ORAL_Processing.Text = ds.Tables[1].Rows[0]["ORAL_Processing"].ToString();
                    Raw_score.Text = ds.Tables[1].Rows[0]["Raw_score"].ToString();


                    //Percentile_Range.Text = ds.Tables[1].Rows[0]["Percentile_Range"].ToString();
                    Total_rawscore.Text = ds.Tables[1].Rows[0]["Total_rawscore"].ToString();
                    Interpretation.Text = ds.Tables[1].Rows[0]["Interpretation"].ToString();
                    Comments_1.Text = ds.Tables[1].Rows[0]["Comments_1"].ToString();

                    //Score_seeking.Text = ds.Tables[1].Rows[0]["Score_seeking"].ToString();
                    //Score_Avoiding.Text = ds.Tables[1].Rows[0]["Score_Avoiding"].ToString();
                    //Score_sensitivity.Text = ds.Tables[1].Rows[0]["Score_sensitivity"].ToString();
                    //Score_Registration.Text = ds.Tables[1].Rows[0]["Score_Registration"].ToString();


                    SEEKING.SelectedValue = ds.Tables[1].Rows[0]["SEEKING"].ToString();
                    AVOIDING.SelectedValue = ds.Tables[1].Rows[0]["AVOIDING"].ToString();
                    SENSITIVITY_2.SelectedValue = ds.Tables[1].Rows[0]["SENSITIVITY_2"].ToString();
                    REGISTRATION.SelectedValue = ds.Tables[1].Rows[0]["REGISTRATION"].ToString();
                    GENERAL.SelectedValue = ds.Tables[1].Rows[0]["GENERAL"].ToString();
                    AUDITORY.SelectedValue = ds.Tables[1].Rows[0]["AUDITORY"].ToString();
                    VISUAL.SelectedValue = ds.Tables[1].Rows[0]["VISUAL"].ToString();
                    TOUCH.SelectedValue = ds.Tables[1].Rows[0]["TOUCH"].ToString();
                    MOVEMENT.SelectedValue = ds.Tables[1].Rows[0]["MOVEMENT"].ToString();
                    ORAL.SelectedValue = ds.Tables[1].Rows[0]["ORAL"].ToString();
                    BEHAVIORAL.SelectedValue = ds.Tables[1].Rows[0]["BEHAVIORAL"].ToString();
                    Comments_2.Text = ds.Tables[1].Rows[0]["Comments_2"].ToString();

                    //  ability_TOTAL.Text = ds.Tables[1].Rows[0]["ability_TOTAL"].ToString();
                    // ability_COMMENTS.Text = ds.Tables[1].Rows[0]["ability_COMMENTS"].ToString();

                    Score_seeking.Text = ds.Tables[1].Rows[0]["Score_seeking"].ToString();
                    Score_Avoiding.Text = ds.Tables[1].Rows[0]["Score_Avoiding"].ToString();
                    Score_sensitivity.Text = ds.Tables[1].Rows[0]["Score_sensitivity"].ToString();
                    Score_Registration.Text = ds.Tables[1].Rows[0]["Score_Registration"].ToString();
                    Score_general.Text = ds.Tables[1].Rows[0]["Score_general"].ToString();
                    Score_Auditory.Text = ds.Tables[1].Rows[0]["Score_Auditory"].ToString();
                    Score_visual.Text = ds.Tables[1].Rows[0]["Score_visual"].ToString();
                    Score_touch.Text = ds.Tables[1].Rows[0]["Score_touch"].ToString();
                    Score_movement.Text = ds.Tables[1].Rows[0]["Score_movement"].ToString();
                    Score_oral.Text = ds.Tables[1].Rows[0]["Score_oral"].ToString();
                    Score_behavioural.Text = ds.Tables[1].Rows[0]["Score_behavioural"].ToString();

                    Seeking_Seeker.SelectedValue = ds.Tables[1].Rows[0]["Seeking_Seeker"].ToString();
                    SPchild_Seeker.Text = ds.Tables[1].Rows[0]["SPchild_Seeker"].ToString();
                    Avoiding_Avoider.SelectedValue = ds.Tables[1].Rows[0]["Avoiding_Avoider"].ToString();
                    SPchild_Avoider.Text = ds.Tables[1].Rows[0]["SPchild_Avoider"].ToString();
                    Sensitivity_Sensor.SelectedValue = ds.Tables[1].Rows[0]["Sensitivity_Sensor"].ToString();
                    SPchild_Sensor.Text = ds.Tables[1].Rows[0]["SPchild_Sensor"].ToString();
                    Registration_Bystander.SelectedValue = ds.Tables[1].Rows[0]["Registration_Bystander"].ToString();
                    SPchild_Bystander.Text = ds.Tables[1].Rows[0]["SPchild_Bystander"].ToString();
                    Auditory_3.SelectedValue = ds.Tables[1].Rows[0]["Auditory_3"].ToString();
                    SPchild_Auditory_3.Text = ds.Tables[1].Rows[0]["SPchild_Auditory_3"].ToString();
                    Visual_3.SelectedValue = ds.Tables[1].Rows[0]["Visual_3"].ToString();
                    SPchild_Visual_3.Text = ds.Tables[1].Rows[0]["SPchild_Visual_3"].ToString();
                    Touch_3.SelectedValue = ds.Tables[1].Rows[0]["Touch_3"].ToString();
                    SPchild_Touch_3.Text = ds.Tables[1].Rows[0]["SPchild_Touch_3"].ToString();
                    Movement_3.SelectedValue = ds.Tables[1].Rows[0]["Movement_3"].ToString();
                    SPchild_Movement_3.Text = ds.Tables[1].Rows[0]["SPchild_Movement_3"].ToString();
                    Body_position.SelectedValue = ds.Tables[1].Rows[0]["Body_position"].ToString();
                    SPchild_Body_position.Text = ds.Tables[1].Rows[0]["SPchild_Body_position"].ToString();
                    Oral_3.SelectedValue = ds.Tables[1].Rows[0]["Oral_3"].ToString();
                    SPchild_Oral_3.Text = ds.Tables[1].Rows[0]["SPchild_Oral_3"].ToString();
                    Conduct_3.SelectedValue = ds.Tables[1].Rows[0]["Conduct_3"].ToString();
                    SPchild_Conduct_3.Text = ds.Tables[1].Rows[0]["SPchild_Conduct_3"].ToString();
                    Social_emotional.SelectedValue = ds.Tables[1].Rows[0]["Social_emotional"].ToString();
                    SPchild_Social_emotional.Text = ds.Tables[1].Rows[0]["SPchild_Social_emotional"].ToString();
                    Attentional_3.SelectedValue = ds.Tables[1].Rows[0]["Attentional_3"].ToString();
                    SPchild_Attentional_3.Text = ds.Tables[1].Rows[0]["SPchild_Attentional_3"].ToString();


                    Comments_3.Text = ds.Tables[1].Rows[0]["Comments_3"].ToString();

                    SPAdult_Low_Registration.Text = ds.Tables[1].Rows[0]["SPAdult_Low_Registration"].ToString();
                    SPAdult_Sensory_seeking.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_seeking"].ToString();
                    SPAdult_Sensory_Sensitivity.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_Sensitivity"].ToString();
                    SPAdult_Sensory_Avoiding.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_Avoiding"].ToString();



                    Low_Registration.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration"].ToString();
                    SP_Low_Registration64.Text = ds.Tables[1].Rows[0]["SP_Low_Registration64"].ToString();
                    Sensory_seeking.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking"].ToString();
                    SP_Sensory_seeking_64.Text = ds.Tables[1].Rows[0]["SP_Sensory_seeking_64"].ToString();
                    Sensory_Sensitivity.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity"].ToString();
                    SP_Sensory_Sensitivity64.Text = ds.Tables[1].Rows[0]["SP_Sensory_Sensitivity64"].ToString();
                    Sensory_Avoiding.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding"].ToString();
                    SP_Sensory_Avoiding64.Text = ds.Tables[1].Rows[0]["SP_Sensory_Avoiding64"].ToString();
                    Comments_4.Text = ds.Tables[1].Rows[0]["Comments_4"].ToString();



                    Low_Registration_5.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_5"].ToString();
                    Sensory_seeking_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString();
                    Sensory_Sensitivity_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString();
                    Sensory_Avoiding_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString();
                    Comments_5.Text = ds.Tables[1].Rows[0]["Comments_5"].ToString();

                    Older_Low_Registration.Text = ds.Tables[1].Rows[0]["Older_Low_Registration"].ToString();
                    Older_Sensory_seeking.Text = ds.Tables[1].Rows[0]["Older_Sensory_seeking"].ToString();
                    Older_Sensory_Sensitivity.Text = ds.Tables[1].Rows[0]["Older_Sensory_Sensitivity"].ToString();
                    Older_Sensory_Avoiding.Text = ds.Tables[1].Rows[0]["Older_Sensory_Avoiding"].ToString();


                    Low_Registration_6.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_6"].ToString();
                    Sensory_seeking_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString();
                    Sensory_Sensitivity_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString();
                    Sensory_Avoiding_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString();
                    Comments_6.Text = ds.Tables[1].Rows[0]["Comments_6"].ToString();
                    #endregion
                    #region*****MutisystemPosture***
                    if (ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0]["Multi_posture_head"] != DBNull.Value)
                    {
                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_head"].ToString();
                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] values = position_head.Split(',');
                            chkHead_Forward.Checked = values.Contains("ForwardHead");
                            chkHead_Neutral.Checked = values.Contains("Neutral");
                            chkHead_PlagiocephalyRight.Checked = values.Contains("PlagiocephalyRight");
                            chkHead_PlagiocephalyLeft.Checked = values.Contains("PlagiocephalyLeft");
                            chkHead_FrontalBossing.Checked = values.Contains("FrontalBossing");
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0]["Multi_posture_elbow"] != DBNull.Value)
                    {
                        string position_elbow = ds.Tables[1].Rows[0]["Multi_posture_elbow"].ToString();
                        if (!string.IsNullOrEmpty(position_elbow))
                        {
                            string[] values = position_elbow.Split(',');
                            chkElbow_BL.Checked = values.Contains("B/L");
                            chkElbow_Right.Checked = values.Contains("Right");
                            chkElbow_Left.Checked = values.Contains("Left");
                            chkElbow_Flexed.Checked = values.Contains("Flexed");
                            chkElbow_Extended.Checked = values.Contains("Extended");
                            chkElbow_Neutral.Checked = values.Contains("Neutral");
                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        void BindCheckboxes(string columnName, params CheckBox[] checkboxes)
                        {
                            if (ds.Tables[1].Rows[0][columnName] != DBNull.Value)
                            {
                                string positions = ds.Tables[1].Rows[0][columnName].ToString();
                                if (!string.IsNullOrEmpty(positions))
                                {
                                    string[] values = positions.Split(',');
                                    foreach (var checkbox in checkboxes)
                                    {
                                        checkbox.Checked = values.Contains(checkbox.Text.Trim());
                                    }
                                }
                            }
                        }

                        // Binding checkboxes for all posture parameters
                        BindCheckboxes("Multi_posture_forarm", chkForearm_BL, chkForearm_Right, chkForearm_Left, chkForearm_Supinated, chkForearm_Pronated, chkForearm_Neutral);
                        BindCheckboxes("Multi_posture_neck", chkNeck_BL, chkNeck_Right, chkNeck_Left, chkNeck_LateralTilt, chkNeck_Hyperextended, chkNeck_Flexed, chkNeck_ChinTuck, chkNeck_Neutral);
                        BindCheckboxes("Multi_posture_Shoulder", chkShoulder_BL, chkShoulder_Right, chkShoulder_Left, chkShoulder_InternallyRotated, chkShoulder_ExternallyRotated, chkShoulder_Elevated, chkShoulder_Depressed, chkShoulder_Protracted, chkShoulder_Retracted, chkShoulder_Abducted, chkShoulder_Adducted, chkShoulder_Neutral);
                        BindCheckboxes("Multi_posture_scapulae", chkScapulae_BL, chkScapulae_Right, chkScapulae_Left, chkScapulae_Protracted, chkScapulae_Retracted, chkScapulae_Abducted, chkScapulae_Adducted, chkScapulae_Elevated, chkScapulae_Depressed, chkScapulae_Winging, chkScapulae_Neutral);
                        BindCheckboxes("Multi_posture_wrist", chkWrist_BL, chkWrist_Right, chkWrist_Left, chkWrist_Flexed, chkWrist_Extended);
                        BindCheckboxes("Multi_posture_hand", chkHand_Fist, chkHand_BL, chkHand_Right, chkHand_Left, chkHand_Neutral);
                        BindCheckboxes("Multi_posture_finger", chkFingers_BL, chkFingers_Right, chkFingers_Left, chkFingers_Flexed, chkFingers_Extended, chkFingers_Neutral);
                        BindCheckboxes("Multi_posture_thumb", chkThumb_BL, chkThumb_Right, chkThumb_Left, chkThumb_Adducted, chkThumb_Abducted, chkThumb_Neutral);
                        BindCheckboxes("Multi_posture_thoracicspine", chkThoracicSpine_Rounded, chkThoracicSpine_Hyperextended, chkThoracicSpine_LaterallyFlexed, chkThoracicSpine_Neutral);
                        BindCheckboxes("Multi_posture_lumbarspine", chkLumbarSpine_Flattened, chkLumbarSpine_Hyperextended, chkLumbarSpine_Neutral);
                        BindCheckboxes("Multi_posture_pelvis", chkPelvis_Neutral, chkPelvis_AnteriorTilted, chkPelvis_PosteriorTilted);
                        BindCheckboxes("Multi_posture_hips", chkHips_BL, chkHips_Right, chkHips_Left, chkHips_LaterallyRotated, chkHips_Abducted, chkHips_InternallyRotated, chkHips_Adducted, chkHips_Flexed, chkHips_Neutral);
                        BindCheckboxes("Multi_posture_knees", chkKnees_BL, chkKnees_Hyperextended, chkKnees_Flexed, chkKnees_Neutral);
                        BindCheckboxes("Multi_posture_ankle", chkAnkle_BL, chkAnkle_Plantarflexed, chkAnkle_Dorsiflexed, chkAnkle_Inverted, chkAnkle_Everted, chkAnkle_Neutral);
                        BindCheckboxes("Multi_posture_feet", chkFeet_BL, chkFeet_Right, chkFeet_Left, chkFeet_Pronated, chkFeet_Supinated, chkFeet_Neutral);
                        BindCheckboxes("Multi_posture_toes", chkToes_BL, chkToes_Right, chkToes_Left, chkToes_Curled, chkToes_Extended, chkToes_Neutral);
                        BindCheckboxes("Multi_posture_bos", chkBOS_Narrow, chkBOS_Wide);
                        BindCheckboxes("Multi_posture_stabiltymethod", chkStability_PosturalTone, chkStability_LockingJoints, chkStability_BroadeningBOS);
                        BindCheckboxes("Multi_posture_hand", chkHand_Fist, chkHand_BL, chkHand_Right, chkHand_Left);

                    }
                    txtCOM_COG.Text = ds.Tables[1].Rows[0]["Multi_posture_com_cog"].ToString();
                    //txtRight.Text = ds.Tables[1].Rows[0]["Multi_posture_Alligmnet_right"].ToString();
                    //txtLeft.Text = ds.Tables[1].Rows[0]["Multi_posture_headAlligmnet_left"].ToString();
                    string alignmentType = ds.Tables[1].Rows[0]["Multi_posture_headAlignment_AlignmentType"].ToString();

                    chkSymmetric.Checked = alignmentType.Equals("Symmetric", StringComparison.OrdinalIgnoreCase);
                    chkAsymmetric.Checked = alignmentType.Equals("Asymmetric", StringComparison.OrdinalIgnoreCase);
                    txtCheeks.Text = ds.Tables[1].Rows[0]["Multi_posture_cheeks"].ToString();
                    txtChin.Text = ds.Tables[1].Rows[0]["Multi_posture_chin"].ToString();
                    txtTeeth.Text = ds.Tables[1].Rows[0]["Multi_posture_teeth"].ToString();
                    txtTongue.Text = ds.Tables[1].Rows[0]["Multi_posture_toungh"].ToString();
                    txtMouth.Text = ds.Tables[1].Rows[0]["Multi_posture_mouth"].ToString();
                    txtLips.Text = ds.Tables[1].Rows[0]["Multi_posture_lips"].ToString();
                    txtStability_Comments.Text = ds.Tables[1].Rows[0]["Multi_posture_Stability"].ToString();
                    txtAnticipatoryControl.Text = ds.Tables[1].Rows[0]["Multi_posture_anticipatory"].ToString();
                    txtPosturalCounterBalance.Text = ds.Tables[1].Rows[0]["Multi_posture_postural"].ToString();
                    Posture_Gen_Ribcage.Text = ds.Tables[1].Rows[0]["Multi_posture_ribcage"].ToString();

                    #endregion
                    #region****ability checklist***
                    Session["Ability"] = ds.Tables[1];
                    ability_TOTAL.Text = ds.Tables[1].Rows[0]["ability_TOTAL"].ToString();
                    ability_COMMENTS.Text = ds.Tables[1].Rows[0]["ability_COMMENTS"].ToString();
                    #endregion
                    #region**ages and stages***
                    Session["AgeState"] = ds.Tables[1];

                    score_Communication_2.Text = ds.Tables[1].Rows[0]["score_Communication_2"].ToString();
                    Inter_Communication_2.Text = ds.Tables[1].Rows[0]["Inter_Communication_2"].ToString();
                    GROSS_2.Text = ds.Tables[1].Rows[0]["GROSS_2"].ToString();
                    inter_Gross_2.Text = ds.Tables[1].Rows[0]["inter_Gross_2"].ToString();
                    FINE_2.Text = ds.Tables[1].Rows[0]["FINE_2"].ToString();
                    inter_FINE_2.Text = ds.Tables[1].Rows[0]["inter_FINE_2"].ToString();
                    PROBLEM_2.Text = ds.Tables[1].Rows[0]["PROBLEM_2"].ToString();
                    inter_PROBLEM_2.Text = ds.Tables[1].Rows[0]["inter_PROBLEM_2"].ToString();
                    PERSONAL_2.Text = ds.Tables[1].Rows[0]["PERSONAL_2"].ToString();
                    inter_PERSONAL_2.Text = ds.Tables[1].Rows[0]["inter_PERSONAL_2"].ToString();

                    Low_Registration_5.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_5"].ToString();
                    Sensory_seeking_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString();
                    Sensory_Sensitivity_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString();
                    Sensory_Avoiding_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString();
                    Comments_5.Text = ds.Tables[1].Rows[0]["Comments_5"].ToString();

                    Older_Low_Registration.Text = ds.Tables[1].Rows[0]["Older_Low_Registration"].ToString();
                    Older_Sensory_seeking.Text = ds.Tables[1].Rows[0]["Older_Sensory_seeking"].ToString();
                    Older_Sensory_Sensitivity.Text = ds.Tables[1].Rows[0]["Older_Sensory_Sensitivity"].ToString();
                    Older_Sensory_Avoiding.Text = ds.Tables[1].Rows[0]["Older_Sensory_Avoiding"].ToString();


                    Low_Registration_6.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_6"].ToString();
                    Sensory_seeking_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString();
                    Sensory_Sensitivity_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString();
                    Sensory_Avoiding_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString();
                    Comments_6.Text = ds.Tables[1].Rows[0]["Comments_6"].ToString();

                    Comm_3.Text = ds.Tables[1].Rows[0]["Comm_3"].ToString();
                    inter_3.Text = ds.Tables[1].Rows[0]["inter_3"].ToString();
                    GROSS_3.Text = ds.Tables[1].Rows[0]["GROSS_3"].ToString();
                    GROSS_inter_3.Text = ds.Tables[1].Rows[0]["GROSS_inter_3"].ToString();
                    FINE_3.Text = ds.Tables[1].Rows[0]["FINE_3"].ToString();
                    PROBLEM_3.Text = ds.Tables[1].Rows[0]["PROBLEM_3"].ToString();
                    FINE_inter_3.Text = ds.Tables[1].Rows[0]["FINE_inter_3"].ToString();
                    PROBLEM_inter_3.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_3"].ToString();
                    PERSONAL_3.Text = ds.Tables[1].Rows[0]["PERSONAL_3"].ToString();
                    PERSONAL_inter_3.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_3"].ToString();
                    Communication_6.Text = ds.Tables[1].Rows[0]["Communication_6"].ToString();
                    comm_inter_6.Text = ds.Tables[1].Rows[0]["comm_inter_6"].ToString();
                    GROSS_6.Text = ds.Tables[1].Rows[0]["GROSS_6"].ToString();
                    GROSS_inter_6.Text = ds.Tables[1].Rows[0]["GROSS_inter_6"].ToString();
                    FINE_6.Text = ds.Tables[1].Rows[0]["FINE_6"].ToString();
                    FINE_inter_6.Text = ds.Tables[1].Rows[0]["FINE_inter_6"].ToString();
                    PROBLEM_6.Text = ds.Tables[1].Rows[0]["PROBLEM_6"].ToString();
                    PROBLEM_inter_6.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_6"].ToString();
                    PERSONAL_6.Text = ds.Tables[1].Rows[0]["PERSONAL_6"].ToString();
                    PERSONAL_inter_6.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_6"].ToString();
                    comm_7.Text = ds.Tables[1].Rows[0]["comm_7"].ToString();
                    inter_7.Text = ds.Tables[1].Rows[0]["inter_7"].ToString();
                    GROSS_7.Text = ds.Tables[1].Rows[0]["GROSS_7"].ToString();
                    GROSS_inter_7.Text = ds.Tables[1].Rows[0]["GROSS_inter_7"].ToString();
                    FINE_7.Text = ds.Tables[1].Rows[0]["FINE_7"].ToString();
                    FINE_inter_7.Text = ds.Tables[1].Rows[0]["FINE_inter_7"].ToString();
                    PROBLEM_7.Text = ds.Tables[1].Rows[0]["PROBLEM_7"].ToString();
                    PROBLEM_inter_7.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_7"].ToString();
                    PERSONAL_7.Text = ds.Tables[1].Rows[0]["PERSONAL_7"].ToString();
                    PERSONAL_inter_7.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_7"].ToString();
                    comm_9.Text = ds.Tables[1].Rows[0]["comm_9"].ToString();
                    inter_9.Text = ds.Tables[1].Rows[0]["inter_9"].ToString();
                    GROSS_9.Text = ds.Tables[1].Rows[0]["GROSS_9"].ToString();
                    GROSS_inter_9.Text = ds.Tables[1].Rows[0]["GROSS_inter_9"].ToString();
                    FINE_9.Text = ds.Tables[1].Rows[0]["FINE_9"].ToString();
                    FINE_inter_9.Text = ds.Tables[1].Rows[0]["FINE_inter_9"].ToString();
                    PROBLEM_9.Text = ds.Tables[1].Rows[0]["PROBLEM_9"].ToString();
                    PROBLEM_inter_9.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_9"].ToString();
                    PERSONAL_9.Text = ds.Tables[1].Rows[0]["PERSONAL_9"].ToString();
                    PERSONAL_inter_9.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_9"].ToString();
                    comm_10.Text = ds.Tables[1].Rows[0]["comm_10"].ToString();
                    inter_10.Text = ds.Tables[1].Rows[0]["inter_10"].ToString();
                    GROSS_10.Text = ds.Tables[1].Rows[0]["GROSS_10"].ToString();
                    GROSS_inter_10.Text = ds.Tables[1].Rows[0]["GROSS_inter_10"].ToString();
                    FINE_10.Text = ds.Tables[1].Rows[0]["FINE_10"].ToString();
                    FINE_inter_10.Text = ds.Tables[1].Rows[0]["FINE_inter_10"].ToString();
                    PROBLEM_10.Text = ds.Tables[1].Rows[0]["PROBLEM_10"].ToString();
                    PROBLEM_inter_10.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_10"].ToString();
                    PERSONAL_10.Text = ds.Tables[1].Rows[0]["PERSONAL_10"].ToString();
                    PERSONAL_inter_10.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_10"].ToString();
                    comm_11.Text = ds.Tables[1].Rows[0]["comm_11"].ToString();
                    inter_11.Text = ds.Tables[1].Rows[0]["inter_11"].ToString();
                    GROSS_11.Text = ds.Tables[1].Rows[0]["GROSS_11"].ToString();
                    GROSS_inter_11.Text = ds.Tables[1].Rows[0]["GROSS_inter_11"].ToString();
                    FINE_11.Text = ds.Tables[1].Rows[0]["FINE_11"].ToString();
                    FINE_inter_11.Text = ds.Tables[1].Rows[0]["FINE_inter_11"].ToString();
                    PROBLEM_11.Text = ds.Tables[1].Rows[0]["PROBLEM_11"].ToString();
                    PROBLEM_inter_11.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_11"].ToString();
                    PERSONAL_11.Text = ds.Tables[1].Rows[0]["PERSONAL_11"].ToString();
                    PERSONAL_inter_11.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_11"].ToString();
                    comm_13.Text = ds.Tables[1].Rows[0]["comm_13"].ToString();
                    inter_13.Text = ds.Tables[1].Rows[0]["inter_13"].ToString();
                    GROSS_13.Text = ds.Tables[1].Rows[0]["GROSS_13"].ToString();
                    GROSS_inter_13.Text = ds.Tables[1].Rows[0]["GROSS_inter_13"].ToString();
                    FINE_13.Text = ds.Tables[1].Rows[0]["FINE_13"].ToString();
                    FINE_inter_13.Text = ds.Tables[1].Rows[0]["FINE_inter_13"].ToString();
                    PROBLEM_13.Text = ds.Tables[1].Rows[0]["PROBLEM_13"].ToString();
                    PROBLEM_inter_13.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_13"].ToString();
                    PERSONAL_13.Text = ds.Tables[1].Rows[0]["PERSONAL_13"].ToString();
                    PERSONAL_inter_13.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_13"].ToString();
                    comm_15.Text = ds.Tables[1].Rows[0]["comm_15"].ToString();
                    inter_15.Text = ds.Tables[1].Rows[0]["inter_15"].ToString();
                    GROSS_15.Text = ds.Tables[1].Rows[0]["GROSS_15"].ToString();
                    GROSS_inter_15.Text = ds.Tables[1].Rows[0]["GROSS_inter_15"].ToString();
                    FINE_15.Text = ds.Tables[1].Rows[0]["FINE_15"].ToString();
                    FINE_inter_15.Text = ds.Tables[1].Rows[0]["FINE_inter_15"].ToString();
                    PROBLEM_15.Text = ds.Tables[1].Rows[0]["PROBLEM_15"].ToString();
                    PROBLEM_inter_15.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_15"].ToString();
                    PERSONAL_15.Text = ds.Tables[1].Rows[0]["PERSONAL_15"].ToString();
                    PERSONAL_inter_15.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_15"].ToString();
                    comm_17.Text = ds.Tables[1].Rows[0]["comm_17"].ToString();
                    inter_17.Text = ds.Tables[1].Rows[0]["inter_17"].ToString();
                    GROSS_17.Text = ds.Tables[1].Rows[0]["GROSS_17"].ToString();
                    GROSS_inter_17.Text = ds.Tables[1].Rows[0]["GROSS_inter_17"].ToString();
                    FINE_17.Text = ds.Tables[1].Rows[0]["FINE_17"].ToString();
                    FINE_inter_17.Text = ds.Tables[1].Rows[0]["FINE_inter_17"].ToString();
                    PROBLEM_17.Text = ds.Tables[1].Rows[0]["PROBLEM_17"].ToString();
                    PROBLEM_inter_17.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_17"].ToString();
                    PERSONAL_17.Text = ds.Tables[1].Rows[0]["PERSONAL_17"].ToString();
                    PERSONAL_inter_17.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_17"].ToString();
                    comm_19.Text = ds.Tables[1].Rows[0]["comm_19"].ToString();
                    inter_19.Text = ds.Tables[1].Rows[0]["inter_19"].ToString();
                    GROSS_19.Text = ds.Tables[1].Rows[0]["GROSS_19"].ToString();
                    GROSS_inter_19.Text = ds.Tables[1].Rows[0]["GROSS_inter_19"].ToString();
                    FINE_19.Text = ds.Tables[1].Rows[0]["FINE_19"].ToString();
                    FINE_inter_19.Text = ds.Tables[1].Rows[0]["FINE_inter_19"].ToString();
                    PROBLEM_19.Text = ds.Tables[1].Rows[0]["PROBLEM_19"].ToString();
                    PROBLEM_inter_19.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_19"].ToString();
                    PERSONAL_19.Text = ds.Tables[1].Rows[0]["PERSONAL_19"].ToString();
                    PERSONAL_inter_19.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_19"].ToString();
                    comm_21.Text = ds.Tables[1].Rows[0]["comm_21"].ToString();
                    inter_21.Text = ds.Tables[1].Rows[0]["inter_21"].ToString();
                    GROSS_21.Text = ds.Tables[1].Rows[0]["GROSS_21"].ToString();
                    GROSS_inter_21.Text = ds.Tables[1].Rows[0]["GROSS_inter_21"].ToString();
                    FINE_21.Text = ds.Tables[1].Rows[0]["FINE_21"].ToString();
                    FINE_inter_21.Text = ds.Tables[1].Rows[0]["FINE_inter_21"].ToString();
                    PROBLEM_21.Text = ds.Tables[1].Rows[0]["PROBLEM_21"].ToString();
                    PROBLEM_inter_21.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_21"].ToString();
                    PERSONAL_21.Text = ds.Tables[1].Rows[0]["PERSONAL_21"].ToString();
                    PERSONAL_inter_21.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_21"].ToString();
                    comm_23.Text = ds.Tables[1].Rows[0]["comm_23"].ToString();
                    inter_23.Text = ds.Tables[1].Rows[0]["inter_23"].ToString();
                    GROSS_23.Text = ds.Tables[1].Rows[0]["GROSS_23"].ToString();
                    GROSS_inter_23.Text = ds.Tables[1].Rows[0]["GROSS_inter_23"].ToString();
                    FINE_23.Text = ds.Tables[1].Rows[0]["FINE_23"].ToString();
                    FINE_inter_23.Text = ds.Tables[1].Rows[0]["FINE_inter_23"].ToString();
                    PROBLEM_23.Text = ds.Tables[1].Rows[0]["PROBLEM_23"].ToString();
                    PROBLEM_inter_23.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_23"].ToString();
                    PERSONAL_23.Text = ds.Tables[1].Rows[0]["PERSONAL_23"].ToString();
                    PERSONAL_inter_23.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_23"].ToString();
                    comm_25.Text = ds.Tables[1].Rows[0]["comm_25"].ToString();
                    inter_25.Text = ds.Tables[1].Rows[0]["inter_25"].ToString();
                    GROSS_25.Text = ds.Tables[1].Rows[0]["GROSS_25"].ToString();
                    GROSS_inter_25.Text = ds.Tables[1].Rows[0]["GROSS_inter_25"].ToString();
                    FINE_25.Text = ds.Tables[1].Rows[0]["FINE_25"].ToString();
                    FINE_inter_25.Text = ds.Tables[1].Rows[0]["FINE_inter_25"].ToString();
                    PROBLEM_25.Text = ds.Tables[1].Rows[0]["PROBLEM_25"].ToString();
                    PROBLEM_inter_25.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_25"].ToString();
                    PERSONAL_25.Text = ds.Tables[1].Rows[0]["PERSONAL_25"].ToString();
                    PERSONAL_inter_25.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_25"].ToString();
                    comm_28.Text = ds.Tables[1].Rows[0]["comm_28"].ToString();
                    inter_28.Text = ds.Tables[1].Rows[0]["inter_28"].ToString();
                    GROSS_28.Text = ds.Tables[1].Rows[0]["GROSS_28"].ToString();
                    GROSS_inter_28.Text = ds.Tables[1].Rows[0]["GROSS_inter_28"].ToString();
                    FINE_28.Text = ds.Tables[1].Rows[0]["FINE_28"].ToString();
                    FINE_inter_28.Text = ds.Tables[1].Rows[0]["FINE_inter_28"].ToString();
                    PROBLEM_28.Text = ds.Tables[1].Rows[0]["PROBLEM_28"].ToString();
                    PROBLEM_inter_28.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_28"].ToString();
                    PERSONAL_28.Text = ds.Tables[1].Rows[0]["PERSONAL_28"].ToString();
                    PERSONAL_inter_28.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_28"].ToString();
                    comm_31.Text = ds.Tables[1].Rows[0]["comm_31"].ToString();
                    inter_31.Text = ds.Tables[1].Rows[0]["inter_31"].ToString();
                    GROSS_31.Text = ds.Tables[1].Rows[0]["GROSS_31"].ToString();
                    GROSS_inter_31.Text = ds.Tables[1].Rows[0]["GROSS_inter_31"].ToString();
                    FINE_31.Text = ds.Tables[1].Rows[0]["FINE_31"].ToString();
                    FINE_inter_31.Text = ds.Tables[1].Rows[0]["FINE_inter_31"].ToString();
                    PROBLEM_31.Text = ds.Tables[1].Rows[0]["PROBLEM_31"].ToString();
                    PROBLEM_inter_31.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_31"].ToString();
                    PERSONAL_31.Text = ds.Tables[1].Rows[0]["PERSONAL_31"].ToString();
                    PERSONAL_inter_31.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_31"].ToString();
                    comm_34.Text = ds.Tables[1].Rows[0]["comm_34"].ToString();
                    inter_34.Text = ds.Tables[1].Rows[0]["inter_34"].ToString();
                    GROSS_34.Text = ds.Tables[1].Rows[0]["GROSS_34"].ToString();
                    GROSS_inter_34.Text = ds.Tables[1].Rows[0]["GROSS_inter_34"].ToString();
                    FINE_34.Text = ds.Tables[1].Rows[0]["FINE_34"].ToString();
                    FINE_inter_34.Text = ds.Tables[1].Rows[0]["FINE_inter_34"].ToString();
                    PROBLEM_34.Text = ds.Tables[1].Rows[0]["PROBLEM_34"].ToString();
                    PROBLEM_inter_34.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_34"].ToString();
                    PERSONAL_34.Text = ds.Tables[1].Rows[0]["PERSONAL_34"].ToString();
                    PERSONAL_inter_34.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_34"].ToString();
                    comm_42.Text = ds.Tables[1].Rows[0]["comm_42"].ToString();
                    inter_42.Text = ds.Tables[1].Rows[0]["inter_42"].ToString();
                    GROSS_42.Text = ds.Tables[1].Rows[0]["GROSS_42"].ToString();
                    GROSS_inter_42.Text = ds.Tables[1].Rows[0]["GROSS_inter_42"].ToString();
                    FINE_42.Text = ds.Tables[1].Rows[0]["FINE_42"].ToString();
                    FINE_inter_42.Text = ds.Tables[1].Rows[0]["FINE_inter_42"].ToString();
                    PROBLEM_42.Text = ds.Tables[1].Rows[0]["PROBLEM_42"].ToString();
                    PROBLEM_inter_42.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_42"].ToString();
                    PERSONAL_42.Text = ds.Tables[1].Rows[0]["PERSONAL_42"].ToString();
                    PERSONAL_inter_42.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_42"].ToString();
                    comm_45.Text = ds.Tables[1].Rows[0]["comm_45"].ToString();
                    inter_45.Text = ds.Tables[1].Rows[0]["inter_45"].ToString();
                    GROSS_45.Text = ds.Tables[1].Rows[0]["GROSS_45"].ToString();
                    GROSS_inter_45.Text = ds.Tables[1].Rows[0]["GROSS_inter_45"].ToString();
                    FINE_45.Text = ds.Tables[1].Rows[0]["FINE_45"].ToString();
                    FINE_inter_45.Text = ds.Tables[1].Rows[0]["FINE_inter_45"].ToString();
                    PROBLEM_45.Text = ds.Tables[1].Rows[0]["PROBLEM_45"].ToString();
                    PROBLEM_inter_45.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_45"].ToString();
                    PERSONAL_45.Text = ds.Tables[1].Rows[0]["PERSONAL_45"].ToString();
                    PERSONAL_inter_45.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_45"].ToString();
                    comm_51.Text = ds.Tables[1].Rows[0]["comm_51"].ToString();
                    inter_51.Text = ds.Tables[1].Rows[0]["inter_51"].ToString();
                    GROSS_51.Text = ds.Tables[1].Rows[0]["GROSS_51"].ToString();
                    GROSS_inter_51.Text = ds.Tables[1].Rows[0]["GROSS_inter_51"].ToString();
                    FINE_51.Text = ds.Tables[1].Rows[0]["FINE_51"].ToString();
                    FINE_inter_51.Text = ds.Tables[1].Rows[0]["FINE_inter_51"].ToString();
                    PROBLEM_51.Text = ds.Tables[1].Rows[0]["PROBLEM_51"].ToString();
                    PROBLEM_inter_51.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_51"].ToString();
                    PERSONAL_51.Text = ds.Tables[1].Rows[0]["PERSONAL_51"].ToString();
                    PERSONAL_inter_51.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_51"].ToString();
                    comm_60.Text = ds.Tables[1].Rows[0]["comm_60"].ToString();
                    inter_60.Text = ds.Tables[1].Rows[0]["inter_60"].ToString();
                    GROSS_60.Text = ds.Tables[1].Rows[0]["GROSS_60"].ToString();
                    GROSS_inter_60.Text = ds.Tables[1].Rows[0]["GROSS_inter_60"].ToString();
                    FINE_60.Text = ds.Tables[1].Rows[0]["FINE_60"].ToString();
                    FINE_inter_60.Text = ds.Tables[1].Rows[0]["FINE_inter_60"].ToString();
                    PROBLEM_60.Text = ds.Tables[1].Rows[0]["PROBLEM_60"].ToString();
                    PROBLEM_inter_60.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_60"].ToString();
                    PERSONAL_60.Text = ds.Tables[1].Rows[0]["PERSONAL_60"].ToString();
                    PERSONAL_inter_60.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_60"].ToString();

                    #endregion
                    #region*** sesory system***
                    SensorySystem_Vision.Text = ds.Tables[1].Rows[0]["SensorySystem_Vision"].ToString();
                    SensorySystem_Auditory.Text = ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString();
                    SensorySystem_Propioceptive.Text = ds.Tables[1].Rows[0]["SensorySystem_Propioceptive"].ToString();
                    SensorySystem_Oromotpor.Text = ds.Tables[1].Rows[0]["SensorySystem_Oromotpor"].ToString();
                    SensorySystem_Vestibular.Text = ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString();
                    SensorySystem_Tactile.Text = ds.Tables[1].Rows[0]["SensorySystem_Tactile"].ToString();
                    SensorySystem_Olfactory.Text = ds.Tables[1].Rows[0]["SensorySystem_Olfactory"].ToString();
                    #endregion
                    #region****evaluation ***
                    Evaluation_Goal_Summary.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString();
                    Evaluation_System_Impairment.Text = ds.Tables[1].Rows[0]["Evaluation_System_Impairment"].ToString();
                    Evaluation_LTG.Text = ds.Tables[1].Rows[0]["Evaluation_LTG"].ToString();
                    Evaluation_STG.Text = ds.Tables[1].Rows[0]["Evaluation_STG"].ToString();
                    Evalution_Plan_advice.Text = ds.Tables[1].Rows[0]["Evalution_Plan_advice"].ToString();
                    Evalution_Plan__Frequency.Text = ds.Tables[1].Rows[0]["Evalution_Plan__Frequency"].ToString();
                    Evalution_Plan_Adjuncts.Text = ds.Tables[1].Rows[0]["Evalution_Plan_Adjuncts"].ToString();
                    Evalution_Plan__Education.Text = ds.Tables[1].Rows[0]["Evalution_Plan__Education"].ToString();
                    #endregion
                    #region***single system**
                    Low_Registration_6.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_6"].ToString();
                    txtSelfRegulation.Text = ds.Tables[1].Rows[0]["SelfRegulation"].ToString();
                    txtArousal.Text = ds.Tables[1].Rows[0]["Arousal"].ToString();
                    txtAttention.Text = ds.Tables[1].Rows[0]["Attention"].ToString();
                    txtAffect.Text = ds.Tables[1].Rows[0]["Affect"].ToString();
                    txtAction.Text = ds.Tables[1].Rows[0]["Action"].ToString();
                    txtCognition.Text = ds.Tables[1].Rows[0]["Cognition"].ToString();
                    txtGI.Text = ds.Tables[1].Rows[0]["GI"].ToString();
                    txtRespiratory.Text = ds.Tables[1].Rows[0]["Respiratory"].ToString();
                    txtCardiovascular.Text = ds.Tables[1].Rows[0]["Cardiovascular"].ToString();
                    txtSkinIntegumentary.Text = ds.Tables[1].Rows[0]["SkinIntegumentary"].ToString();
                    txtNutrition.Text = ds.Tables[1].Rows[0]["Nutrition"].ToString();
                    #endregion
                    #region***test and measure***
                    DataRow row = ds.Tables[1].Rows[0];

                    GMFCSCheckBoxI.Checked = GetBool(row["GMFCS_I"]);
                    GMFCSCheckBoxII.Checked = GetBool(row["GMFCS_II"]);
                    GMFCSCheckBoxIII.Checked = GetBool(row["GMFCS_III"]);
                    GMFCSCheckBoxIV.Checked = GetBool(row["GMFCS_IV"]);
                    GMFCSCheckBoxV.Checked = GetBool(row["GMFCS_V"]);

                    txtGmfm_LyingRolling.Text = ds.Tables[1].Rows[0]["Gmfm_LyingRolling"].ToString();
                    //chkI_LyingRolling.Checked = GetBool(row["chkI_LyingRolling"]);
                    //chkII_LyingRolling.Checked = GetBool(row["chkII_LyingRolling"]);
                    //chkIII_LyingRolling.Checked = GetBool(row["chkIII_LyingRolling"]);
                    //chkIV_LyingRolling.Checked = GetBool(row["chkIV_LyingRolling"]);
                    //chkV_LyingRolling.Checked = GetBool(row["chkV_LyingRolling"]);


                    txtGmfm_Sitting.Text = ds.Tables[1].Rows[0]["Gmfm_Sitting"].ToString();
                    //chkI_Sitting.Checked = GetBool(row["chkI_Sitting"]);
                    //chkII_Sitting.Checked = GetBool(row["chkII_Sitting"]);
                    //chkIII_Sitting.Checked = GetBool(row["chkIII_Sitting"]);
                    //chkIV_Sitting.Checked = GetBool(row["chkIV_Sitting"]);
                    //chkV_Sitting.Checked = GetBool(row["chkV_Sitting"]);

                    txtGmfm_KneelingCrawling.Text = ds.Tables[1].Rows[0]["Gmfm_KneelingCrawling"].ToString();
                    //chkI_KneelingCrawling.Checked = GetBool(row["chkI_KneelingCrawling"]);
                    //chkII_KneelingCrawling.Checked = GetBool(row["chkII_KneelingCrawling"]);
                    //chkIII_KneelingCrawling.Checked = GetBool(row["chkIII_KneelingCrawling"]);
                    //chkIV_KneelingCrawling.Checked = GetBool(row["chkIV_KneelingCrawling"]);
                    //chkV_KneelingCrawling.Checked = GetBool(row["chkV_KneelingCrawling"]);


                    txtGmfm_Standing.Text = ds.Tables[1].Rows[0]["Gmfm_Standing"].ToString();
                    //chkI_Standing.Checked = GetBool(row["chkI_Standing"]);
                    //chkII_Standing.Checked = GetBool(row["chkII_Standing"]);
                    //chkIII_Standing.Checked = GetBool(row["chkIII_Standing"]);
                    //chkIV_Standing.Checked = GetBool(row["chkIV_Standing"]);
                    //chkV_Standing.Checked = GetBool(row["chkV_Standing"]);

                    txtGmfm_RunningJumping.Text = ds.Tables[1].Rows[0]["Gmfm_RunningJumping"].ToString();
                    txtGmfm_TotalScore.Text = ds.Tables[1].Rows[0]["txtGmfm_TotalScore"].ToString();
                    //chkI_RunningJumping.Checked = GetBool(row["chkI_RunningJumping"]);
                    //chkII_RunningJumping.Checked = GetBool(row["chkII_RunningJumping"]);
                    //chkIII_RunningJumping.Checked = GetBool(row["chkIII_RunningJumping"]);
                    //chkIV_RunningJumping.Checked = GetBool(row["chkIV_RunningJumping"]);
                    //chkV_RunningJumping.Checked = GetBool(row["chkV_RunningJumping"]);

                    chkMACs_I.Checked = GetBool(row["MACs_I"]);
                    chkMACs_II.Checked = GetBool(row["MACs_II"]);
                    chkMACs_III.Checked = GetBool(row["MACs_III"]);
                    chkMACs_IV.Checked = GetBool(row["MACs_IV"]);
                    chkMACs_V.Checked = GetBool(row["MACs_V"]);

                    // FMS

                    chkFMS_I.Checked = GetBool(row["FMS_I"]);
                    chkFMS_II.Checked = GetBool(row["FMS_II"]);
                    chkFMS_III.Checked = GetBool(row["FMS_III"]);
                    chkFMS_IV.Checked = GetBool(row["FMS_IV"]);
                    chkFMS_V.Checked = GetBool(row["FMS_V"]);

                    // Barry

                    chkBarry_I.Checked = GetBool(row["Barry_I"]);
                    chkBarry_II.Checked = GetBool(row["Barry_II"]);
                    chkBarry_III.Checked = GetBool(row["Barry_III"]);
                    chkBarry_IV.Checked = GetBool(row["Barry_IV"]);
                    chkBarry_V.Checked = GetBool(row["Barry_V"]);
                    chkBarry_VI.Checked = GetBool(row["Barry_VI"]);

                    Barry_albright_txt.Text = ds.Tables[1].Rows[0]["Barry_albright_txt"].ToString();
                    #endregion
                    #region***muscoskeleton***
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

                    Musculoskeletal_Mmt_ExternalObliquesRight.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExternalObliquesRight"].ToString();
                    Musculoskeletal_Mmt_ExternalObliquesLeft.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExternalObliquesLeft"].ToString();
                    Musculoskeletal_Back_Extensors_cmt.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Back_Extensors_cmt"].ToString();
                    Musculoskeletal_Rectus_Abdominis_cmt.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Rectus_Abdominis_cmt"].ToString();


                    Musculoskeletal_Mmt_Ta_Left.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Left"].ToString();
                    Musculoskeletal_Mmt_Ta_Right.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Right"].ToString();
                    Musculoskeletal_Mmt_Hamstring_left.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_left"].ToString();
                    Musculoskeletal_Mmt_Hamstring_Right.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_Right"].ToString();
                    Musculoskeletal_Mmt_adductors_left.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_left"].ToString();
                    Musculoskeletal_Mmt_adductors_right.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_right"].ToString();
                    Musculoskeletal_Mmt_hipFlexor_left.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_left"].ToString();
                    Musculoskeletal_Mmt_hipFlexor_Right.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_Right"].ToString();
                    Musculoskeletal_Mmt_biceps_left.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_left"].ToString();
                    Musculoskeletal_Mmt_biceps_right.Text = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_right"].ToString();

                    #endregion
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
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        SelectMonth.SelectedValue = ds.Tables[1].Rows[i]["MONTHS"].ToString();
                        SelectMonth_SelectedIndexChanged(null, null);
                        MonthSelect.SelectedValue = ds.Tables[1].Rows[i]["ABILITY_months"].ToString();
                        MonthSelect_SelectedIndexChanged(null, null);
                    }
                    updAgeStage.Update();
                    updAbility.Update();
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
                if (string.IsNullOrWhiteSpace(txtGivenDate.Text))
                {
                    DbHelper.Configuration.setAlert(Page, "Given Date is Required...", 2);
                    return;
                }
                else
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
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataSet ds = RDB.Get_NDT(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            if (g < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            }
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            SqlCommand cmd1 = new SqlCommand("SET_NDt_FLags");
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@AppointmentID", _appointmentID);
            cmd1.Parameters.AddWithValue("@IsGiven", IsGiven);
            cmd1.Parameters.AddWithValue("@IsFinal", IsFinal);
            cmd1.Parameters.AddWithValue("@GivenDate", IsGiven ? (object)GivenDate : DBNull.Value);
            db.DbUpdate(cmd1);

            int tabValue = 1;
            SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE");
            cmd.CommandType = CommandType.StoredProcedure;

            string tabsValue = hfdTabs.Value;
            string callFromValue = hfdCallFrom.Value;
            string curTabValue = hfdCurTab.Value;
            string prevTabValue = hfdPrevTab.Value;

            switch (this.hfdTabs.Value)
            {

                case "":
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab":
                    #region ===== Tab 1 =====

                    tabValue = 1;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Ability", FA_GrossMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Limit", FA_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Ability", FA_FineMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Limit", FA_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Ability", FA_Communication_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Limit", FA_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Ability", FA_Cognition_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Limit", FA_Cognition_Limit.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report2_tab":
                    #region ===== Tab 2 =====

                    tabValue = 2;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@ParticipationAbility_GrossMotor", ParticipationAbility_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_GrossMotor_Limit", ParticipationAbility_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_FineMotor", ParticipationAbility_FineMotor.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_FineMotor_Limit", ParticipationAbility_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Communication", ParticipationAbility_Communication.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Communication_Limit", ParticipationAbility_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Cognition", ParticipationAbility_Cognition.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Cognition_Limit", ParticipationAbility_Cognition_Limit.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Personal_Positive", Contextual_Personal_Positive.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Personal_Negative", Contextual_Personal_Negative.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Environmental_Positive", Contextual_Enviremental_Positive.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Environmental_Negative", Contextual_Enviremental_Negative.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);


                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab":
                    #region ===== Tab 3 =====

                    tabValue = 3;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    string headPositions = string.Empty;
                    List<string> selectedPositions = new List<string>();
                    if (chkHead_Forward.Checked) { selectedPositions.Add(chkHead_Forward.Text.Trim().Replace(" ", "")); }
                    if (chkHead_Neutral.Checked) { selectedPositions.Add(chkHead_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkHead_PlagiocephalyRight.Checked) { selectedPositions.Add(chkHead_PlagiocephalyRight.Text.Trim().Replace(" ", "")); }
                    if (chkHead_PlagiocephalyLeft.Checked) { selectedPositions.Add(chkHead_PlagiocephalyLeft.Text.Trim().Replace(" ", "")); }
                    if (chkHead_FrontalBossing.Checked) { selectedPositions.Add(chkHead_FrontalBossing.Text.Trim().Replace(" ", "")); }
                    headPositions = string.Join(",", selectedPositions);
                    /***/
                    string shoulderPositions = string.Empty;
                    List<string> selectedShoulderPositions = new List<string>();
                    if (chkShoulder_BL.Checked) { selectedShoulderPositions.Add(chkShoulder_BL.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Right.Checked) { selectedShoulderPositions.Add(chkShoulder_Right.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Left.Checked) { selectedShoulderPositions.Add(chkShoulder_Left.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_InternallyRotated.Checked) { selectedShoulderPositions.Add(chkShoulder_InternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_ExternallyRotated.Checked) { selectedShoulderPositions.Add(chkShoulder_ExternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Elevated.Checked) { selectedShoulderPositions.Add(chkShoulder_Elevated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Depressed.Checked) { selectedShoulderPositions.Add(chkShoulder_Depressed.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Protracted.Checked) { selectedShoulderPositions.Add(chkShoulder_Protracted.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Retracted.Checked) { selectedShoulderPositions.Add(chkShoulder_Retracted.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Abducted.Checked) { selectedShoulderPositions.Add(chkShoulder_Abducted.Text.Trim().Replace(" ", "")); }

                    if (chkShoulder_Neutral.Checked) { selectedShoulderPositions.Add(chkShoulder_Neutral.Text.Trim().Replace(" ", "")); }
                    shoulderPositions = string.Join(",", selectedShoulderPositions);
                    /***/
                    string neckPositions = string.Empty;
                    List<string> selectedNeckPositions = new List<string>();
                    if (chkNeck_BL.Checked) { selectedNeckPositions.Add(chkNeck_BL.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Right.Checked) { selectedNeckPositions.Add(chkNeck_Right.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Left.Checked) { selectedNeckPositions.Add(chkNeck_Left.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_LateralTilt.Checked) { selectedNeckPositions.Add(chkNeck_LateralTilt.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Hyperextended.Checked) { selectedNeckPositions.Add(chkNeck_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Flexed.Checked) { selectedNeckPositions.Add(chkNeck_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_ChinTuck.Checked) { selectedNeckPositions.Add(chkNeck_ChinTuck.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Neutral.Checked) { selectedNeckPositions.Add(chkNeck_Neutral.Text.Trim().Replace(" ", "")); }
                    neckPositions = string.Join(",", selectedNeckPositions);
                    /**/
                    string scapulaePositions = string.Empty;
                    List<string> selectedScapulaePositions = new List<string>();
                    if (chkScapulae_BL.Checked) { selectedScapulaePositions.Add(chkScapulae_BL.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Right.Checked) { selectedScapulaePositions.Add(chkScapulae_Right.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Left.Checked) { selectedScapulaePositions.Add(chkScapulae_Left.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Protracted.Checked) { selectedScapulaePositions.Add(chkScapulae_Protracted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Retracted.Checked) { selectedScapulaePositions.Add(chkScapulae_Retracted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Abducted.Checked) { selectedScapulaePositions.Add(chkScapulae_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Adducted.Checked) { selectedScapulaePositions.Add(chkScapulae_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Elevated.Checked) { selectedScapulaePositions.Add(chkScapulae_Elevated.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Depressed.Checked) { selectedScapulaePositions.Add(chkScapulae_Depressed.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Winging.Checked) { selectedScapulaePositions.Add(chkScapulae_Winging.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Neutral.Checked) { selectedScapulaePositions.Add(chkScapulae_Neutral.Text.Trim().Replace(" ", "")); }
                    scapulaePositions = string.Join(",", selectedScapulaePositions);
                    /**/
                    string elbowPositions = string.Empty;
                    List<string> selectedElbowPositions = new List<string>();
                    if (chkElbow_BL.Checked) { selectedElbowPositions.Add(chkElbow_BL.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Right.Checked) { selectedElbowPositions.Add(chkElbow_Right.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Left.Checked) { selectedElbowPositions.Add(chkElbow_Left.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Flexed.Checked) { selectedElbowPositions.Add(chkElbow_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Extended.Checked) { selectedElbowPositions.Add(chkElbow_Extended.Text.Trim().Replace(" ", "")); }
                    elbowPositions = string.Join(",", selectedElbowPositions);
                    //**//
                    string forearmPositions = string.Empty;
                    List<string> selectedForearmPositions = new List<string>();
                    if (chkForearm_BL.Checked) { selectedForearmPositions.Add(chkForearm_BL.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Right.Checked) { selectedForearmPositions.Add(chkForearm_Right.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Left.Checked) { selectedForearmPositions.Add(chkForearm_Left.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Supinated.Checked) { selectedForearmPositions.Add(chkForearm_Supinated.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Pronated.Checked) { selectedForearmPositions.Add(chkForearm_Pronated.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Neutral.Checked) { selectedForearmPositions.Add(chkForearm_Neutral.Text.Trim().Replace(" ", "")); }

                    forearmPositions = string.Join(",", selectedForearmPositions);
                    //*//
                    string wristPositions = string.Empty;
                    List<string> selectedWristPositions = new List<string>();
                    if (chkWrist_BL.Checked) { selectedWristPositions.Add(chkWrist_BL.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Neutral.Checked) { selectedWristPositions.Add(chkWrist_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Right.Checked) { selectedWristPositions.Add(chkWrist_Right.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Left.Checked) { selectedWristPositions.Add(chkWrist_Left.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Flexed.Checked) { selectedWristPositions.Add(chkWrist_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Extended.Checked) { selectedWristPositions.Add(chkWrist_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_UD.Checked) { selectedWristPositions.Add("UlnarDeviation"); }  // Handling special case
                    if (chkWrist_RD.Checked) { selectedWristPositions.Add("RadialDeviation"); } // Handling special case
                    wristPositions = string.Join(",", selectedWristPositions);
                    //**//
                    string handPositions = string.Empty;
                    List<string> selectedHandPositions = new List<string>();
                    if (chkHand_Fist.Checked) { selectedHandPositions.Add(chkHand_Fist.Text.Trim().Replace(" ", "")); }
                    if (chkHand_BL.Checked) { selectedHandPositions.Add(chkHand_BL.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Right.Checked) { selectedHandPositions.Add(chkHand_Right.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Left.Checked) { selectedHandPositions.Add(chkHand_Left.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Neutral.Checked) { selectedHandPositions.Add(chkHand_Neutral.Text.Trim().Replace(" ", "")); }
                    handPositions = string.Join(",", selectedHandPositions);
                    //**//
                    string fingersPositions = string.Empty;
                    List<string> selectedFingersPositions = new List<string>();
                    if (chkFingers_BL.Checked) { selectedFingersPositions.Add(chkFingers_BL.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Right.Checked) { selectedFingersPositions.Add(chkFingers_Right.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Left.Checked) { selectedFingersPositions.Add(chkFingers_Left.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Flexed.Checked) { selectedFingersPositions.Add(chkFingers_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Extended.Checked) { selectedFingersPositions.Add(chkFingers_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Neutral.Checked) { selectedFingersPositions.Add(chkFingers_Neutral.Text.Trim().Replace(" ", "")); }
                    fingersPositions = string.Join(",", selectedFingersPositions);
                    //*//
                    string thumbPositions = string.Empty;
                    List<string> selectedThumbPositions = new List<string>();
                    if (chkThumb_BL.Checked) { selectedThumbPositions.Add(chkThumb_BL.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Right.Checked) { selectedThumbPositions.Add(chkThumb_Right.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Left.Checked) { selectedThumbPositions.Add(chkThumb_Left.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Adducted.Checked) { selectedThumbPositions.Add(chkThumb_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Abducted.Checked) { selectedThumbPositions.Add(chkThumb_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Neutral.Checked) { selectedThumbPositions.Add(chkThumb_Neutral.Text.Trim().Replace(" ", "")); }
                    thumbPositions = string.Join(",", selectedThumbPositions);
                    //*//
                    string thoracicSpinePositions = string.Empty;
                    List<string> selectedThoracicSpinePositions = new List<string>();
                    if (chkThoracicSpine_Rounded.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Rounded.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_Hyperextended.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_LaterallyFlexed.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_LaterallyFlexed.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_Neutral.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Neutral.Text.Trim().Replace(" ", "")); }
                    thoracicSpinePositions = string.Join(",", selectedThoracicSpinePositions);
                    //*//
                    string lumbarSpinePositions = string.Empty;
                    List<string> selectedLumbarSpinePositions = new List<string>();
                    if (chkLumbarSpine_Flattened.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Flattened.Text.Trim().Replace(" ", "")); }
                    if (chkLumbarSpine_Hyperextended.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkLumbarSpine_Neutral.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Neutral.Text.Trim().Replace(" ", "")); }
                    lumbarSpinePositions = string.Join(",", selectedLumbarSpinePositions);
                    //**//
                    string pelvisPositions = string.Empty;
                    List<string> selectedPelvisPositions = new List<string>();
                    if (chkPelvis_Neutral.Checked) { selectedPelvisPositions.Add(chkPelvis_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkPelvis_AnteriorTilted.Checked) { selectedPelvisPositions.Add(chkPelvis_AnteriorTilted.Text.Trim().Replace(" ", "")); }
                    if (chkPelvis_PosteriorTilted.Checked) { selectedPelvisPositions.Add(chkPelvis_PosteriorTilted.Text.Trim().Replace(" ", "")); }
                    pelvisPositions = string.Join(",", selectedPelvisPositions);
                    //**//
                    string hipsPositions = string.Empty;
                    List<string> selectedHipsPositions = new List<string>();
                    if (chkHips_BL.Checked) { selectedHipsPositions.Add(chkHips_BL.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Right.Checked) { selectedHipsPositions.Add(chkHips_Right.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Left.Checked) { selectedHipsPositions.Add(chkHips_Left.Text.Trim().Replace(" ", "")); }
                    if (chkHips_LaterallyRotated.Checked) { selectedHipsPositions.Add(chkHips_LaterallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Abducted.Checked) { selectedHipsPositions.Add(chkHips_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkHips_InternallyRotated.Checked) { selectedHipsPositions.Add(chkHips_InternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Adducted.Checked) { selectedHipsPositions.Add(chkHips_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Flexed.Checked) { selectedHipsPositions.Add(chkHips_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Neutral.Checked) { selectedHipsPositions.Add(chkHips_Neutral.Text.Trim().Replace(" ", "")); }
                    hipsPositions = string.Join(",", selectedHipsPositions);
                    //**//
                    string kneesPositions = string.Empty;
                    List<string> selectedKneesPositions = new List<string>();
                    if (chkKnees_BL.Checked) { selectedKneesPositions.Add(chkKnees_BL.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Hyperextended.Checked) { selectedKneesPositions.Add(chkKnees_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Flexed.Checked) { selectedKneesPositions.Add(chkKnees_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Neutral.Checked) { selectedKneesPositions.Add(chkKnees_Neutral.Text.Trim().Replace(" ", "")); }
                    kneesPositions = string.Join(",", selectedKneesPositions);
                    //**//
                    string anklePositions = string.Empty;
                    List<string> selectedAnklePositions = new List<string>();
                    if (chkAnkle_BL.Checked) { selectedAnklePositions.Add(chkAnkle_BL.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Plantarflexed.Checked) { selectedAnklePositions.Add(chkAnkle_Plantarflexed.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Dorsiflexed.Checked) { selectedAnklePositions.Add(chkAnkle_Dorsiflexed.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Inverted.Checked) { selectedAnklePositions.Add(chkAnkle_Inverted.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Everted.Checked) { selectedAnklePositions.Add(chkAnkle_Everted.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Neutral.Checked) { selectedAnklePositions.Add(chkAnkle_Neutral.Text.Trim().Replace(" ", "")); }
                    anklePositions = string.Join(",", selectedAnklePositions);
                    //**//
                    string feetPositions = string.Empty;
                    List<string> selectedFeetPositions = new List<string>();
                    if (chkFeet_BL.Checked) { selectedFeetPositions.Add(chkFeet_BL.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Right.Checked) { selectedFeetPositions.Add(chkFeet_Right.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Left.Checked) { selectedFeetPositions.Add(chkFeet_Left.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Pronated.Checked) { selectedFeetPositions.Add(chkFeet_Pronated.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Supinated.Checked) { selectedFeetPositions.Add(chkFeet_Supinated.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Neutral.Checked) { selectedFeetPositions.Add(chkFeet_Neutral.Text.Trim().Replace(" ", "")); }
                    feetPositions = string.Join(",", selectedFeetPositions);
                    //**//
                    string toesPositions = string.Empty;
                    List<string> selectedToesPositions = new List<string>();
                    if (chkToes_BL.Checked) { selectedToesPositions.Add(chkToes_BL.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Right.Checked) { selectedToesPositions.Add(chkToes_Right.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Left.Checked) { selectedToesPositions.Add(chkToes_Left.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Curled.Checked) { selectedToesPositions.Add(chkToes_Curled.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Extended.Checked) { selectedToesPositions.Add(chkToes_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Neutral.Checked) { selectedToesPositions.Add(chkToes_Neutral.Text.Trim().Replace(" ", "")); }
                    toesPositions = string.Join(",", selectedToesPositions);
                    //**//
                    string bosPositions = string.Empty;
                    List<string> selectedBOSPositions = new List<string>();
                    if (chkBOS_Narrow.Checked) { selectedBOSPositions.Add(chkBOS_Narrow.Text.Trim().Replace(" ", "")); }
                    if (chkBOS_Wide.Checked) { selectedBOSPositions.Add(chkBOS_Wide.Text.Trim().Replace(" ", "")); }
                    bosPositions = string.Join(",", selectedBOSPositions);
                    //*?/
                    string stabilityMethods = string.Empty;
                    List<string> selectedStabilityMethods = new List<string>();
                    if (chkStability_PosturalTone.Checked) { selectedStabilityMethods.Add(chkStability_PosturalTone.Text.Trim()); }
                    if (chkStability_LockingJoints.Checked) { selectedStabilityMethods.Add(chkStability_LockingJoints.Text.Trim()); }
                    if (chkStability_BroadeningBOS.Checked) { selectedStabilityMethods.Add(chkStability_BroadeningBOS.Text.Trim()); }
                    stabilityMethods = string.Join(",", selectedStabilityMethods);
                    string alignmentType = "";

                    if (chkSymmetric.Checked)
                        alignmentType = "Symmetric";
                    else if (chkAsymmetric.Checked)
                        alignmentType = "Asymmetric";

                    //cmd.Parameters.AddWithValue("@Multi_posture_Alligmnet_right", txtRight.Text);
                    //cmd.Parameters.AddWithValue("@Multi_posture_headAlligmnet_left", txtLeft.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_headAlignment_AlignmentType", alignmentType);
                    cmd.Parameters.AddWithValue("@Multi_posture_head", headPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_Shoulder", shoulderPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_neck", neckPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_scapulae", scapulaePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_elbow", elbowPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_forarm", forearmPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_wrist", wristPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_hand", handPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_finger", fingersPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_thumb", thumbPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_ribcage", Posture_Gen_Ribcage.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_thoracicspine", thoracicSpinePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_lumbarspine", lumbarSpinePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_pelvis", pelvisPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_hips", hipsPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_knees", kneesPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_ankle", anklePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_feet", feetPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_toes", toesPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_bos", bosPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_stabiltymethod", stabilityMethods);
                    cmd.Parameters.AddWithValue("@Multi_posture_com_cog", txtCOM_COG.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_cheeks", txtCheeks.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_chin", txtChin.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_teeth", txtTeeth.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_toungh", txtTongue.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_mouth", txtMouth.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_lips", txtLips.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_Stability", txtStability_Comments.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_anticipatory", txtAnticipatoryControl.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_postural", txtPosturalCounterBalance.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab":
                    #region ===== Tab 4 =====

                    tabValue = 4;
                    string Posture_Alignment_Type = string.Empty;
                    if (Multi_Movement_TypeOf_1.Checked) { Posture_Alignment_Type = Multi_Movement_TypeOf_1.Text.Trim(); }
                    if (Multi_Movement_TypeOf_2.Checked) { Posture_Alignment_Type = Multi_Movement_TypeOf_2.Text.Trim(); }

                    string Multi_Movement_Type = string.Empty;

                    if (Multi_Movement_Sagittal != null && Multi_Movement_Sagittal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Sagittal.Text.Trim();
                    }
                    else if (Multi_Movement_Coronal != null && Multi_Movement_Coronal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Coronal.Text.Trim();
                    }
                    else if (Multi_Movement_Frontal != null && Multi_Movement_Frontal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Frontal.Text.Trim();
                    }

                    string Multi_Movement_WeightShift = string.Join(",", Movement_WeightShift.Items.Cast<ListItem>()
                                       .Where(i => i.Selected)
                                       .Select(i => i.Value));
                    string movementInterlimb = string.Empty;
                    //**
                    List<string> selectedMovementInterlimb = new List<string>();
                    if (Movement_Interlimb_SpineToShoulder.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_SpineToShoulder.Text.Trim()); }
                    if (Movement_Interlimb_Scapulohumeral.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_Scapulohumeral.Text.Trim()); }
                    if (Movement_Interlimb_Pelvifemoral.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_Pelvifemoral.Text.Trim()); }
                    if (Movement_Interlimb_WithinUL.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_WithinUL.Text.Trim()); }
                    if (Movement_Interlimb_WithinLL.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_WithinLL.Text.Trim()); }
                    movementInterlimb = string.Join(",", selectedMovementInterlimb);
                    //**
                    string intralimbDissociation = string.Empty;
                    List<string> selectedIntralimbDissociation = new List<string>();
                    if (Movement_Intralimb_LE.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_LE.Text.Trim()); }
                    if (Movement_Intralimb_UE.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_UE.Text.Trim()); }
                    if (Movement_Intralimb_Spine.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_Spine.Text.Trim()); }
                    intralimbDissociation = string.Join(",", selectedIntralimbDissociation);
                    //**
                    string movementOveruse = string.Empty;
                    List<string> selectedOveruseOptions = new List<string>();
                    if (chkLeanMuscle.Checked) { selectedOveruseOptions.Add(chkLeanMuscle.Text.Trim()); }
                    if (chkLockingJoints.Checked) { selectedOveruseOptions.Add(chkLockingJoints.Text.Trim()); }
                    if (chkBroadBOS.Checked) { selectedOveruseOptions.Add(chkBroadBOS.Text.Trim()); }
                    if (chkGeneralPosture.Checked) { selectedOveruseOptions.Add(chkGeneralPosture.Text.Trim()); }
                    movementOveruse = string.Join(",", selectedOveruseOptions);
                    //
                    List<string> selectedUpperLimb = new List<string>();
                    if (Movement_UpperLimb_Inner.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Inner.Text.Trim()); }
                    if (Movement_UpperLimb_Mid.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Mid.Text.Trim()); }
                    if (Movement_UpperLimb_Outer.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Outer.Text.Trim()); }
                    string upperLimbSelection = string.Join(",", selectedUpperLimb);
                    //
                    List<string> selectedLowerLimb = new List<string>();
                    if (Movement_LowerLimb_Inner.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Inner.Text.Trim()); }
                    if (Movement_LowerLimb_Mid.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Mid.Text.Trim()); }
                    if (Movement_LowerLimb_Outer.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Outer.Text.Trim()); }
                    string lowerLimbSelection = string.Join(",", selectedLowerLimb);
                    //
                    List<string> selectedCervicalSpine = new List<string>();
                    if (Movement_CervicalSpine_Inner.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Inner.Text.Trim()); }
                    if (Movement_CervicalSpine_Mid.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Mid.Text.Trim()); }
                    if (Movement_CervicalSpine_Outer.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Outer.Text.Trim()); }
                    string cervicalSpineSelection = string.Join(",", selectedCervicalSpine);
                    //
                    List<string> selectedThoracicSpine = new List<string>();
                    if (Movement_ThoracicSpine_Inner.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Inner.Text.Trim()); }
                    if (Movement_ThoracicSpine_Mid.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Mid.Text.Trim()); }
                    if (Movement_ThoracicSpine_Outer.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Outer.Text.Trim()); }
                    string thoracicSpineSelection = string.Join(",", selectedThoracicSpine);
                    //
                    string movementStability = string.Empty;
                    List<string> selectedStabilityOptions = new List<string>();
                    if (chkOveruseMomentum.Checked) { selectedStabilityOptions.Add(chkOveruseMomentum.Text.Trim()); }
                    if (chkIncreasedBOS.Checked) { selectedStabilityOptions.Add(chkIncreasedBOS.Text.Trim()); }
                    if (chkIncreasingPosturalTone.Checked) { selectedStabilityOptions.Add(chkIncreasingPosturalTone.Text.Trim()); }
                    movementStability = string.Join(",", selectedStabilityOptions);
                    //
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Movement_Inertia", Movement_Inertia.Text);
                    cmd.Parameters.AddWithValue("@Posture_Alignment_Type", Posture_Alignment_Type);
                    cmd.Parameters.AddWithValue("@Multi_Movement_Type", Multi_Movement_Type);
                    cmd.Parameters.AddWithValue("@Multi_Movement_WeightShift", Multi_Movement_WeightShift);
                    cmd.Parameters.AddWithValue("@Multi_Movement_interlimb", movementInterlimb);
                    cmd.Parameters.AddWithValue("@Multi_Movement_intralimb", intralimbDissociation);
                    cmd.Parameters.AddWithValue("@Multi_Movement_overuse", movementOveruse);
                    cmd.Parameters.AddWithValue("@Multi_Movement_Bal_maintain", Movement_Balance_Maintain.Text);
                    cmd.Parameters.AddWithValue("@Multi_Movement_BAl_during", Movement_Balance_During.Text);
                    cmd.Parameters.AddWithValue("@UpperLimb_Movement", upperLimbSelection);
                    cmd.Parameters.AddWithValue("@LowerLimb_Movement", lowerLimbSelection);
                    cmd.Parameters.AddWithValue("@CervicalSpine_Movement", cervicalSpineSelection);
                    cmd.Parameters.AddWithValue("@ThoracicSpine_Movement", thoracicSpineSelection);
                    cmd.Parameters.AddWithValue("@Multi_Movement_statbilty", movementStability);
                    cmd.Parameters.AddWithValue("@Gene_obsr_comments", Gene_obsr_comments_txt.Text);

                    cmd.Parameters.AddWithValue("@txtSoinePoor", chkSoinePoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtSoineFair", chkSoineFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtSoineGood", chkSoineGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtScapuloPoor", chkScapuloPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtScapuloFair", chkScapuloFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtScapuloGood", chkScapuloGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtPelviPoor", chkPelviPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtPelviFair", chkPelviFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtPelviGood", chkPelviGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtWithinUlPoor", chkWithinUlPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtWithinUlFair", chkWithinUlFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtWithinUlGood", chkWithinUlGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtWithinLlPoor", chkWithinLlPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtWithinLlFair", chkWithinLlFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtWithinLlGood", chkWithinLlGood.Checked ? "Good" : "");


                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);


                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab":
                    #region ===== Tab 5 =====
                    tabValue = 5;
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

                    tabValue = 5;
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
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Control", Neurometer_Initialigy_Control.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Termination", Neurometer_Initialigy_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Sustainance", Neurometer_Initialigy_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_initial", Neurometer_Initialigy_initial.Text);



                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab":
                    #region ===== Tab 6 =====
                    tabValue = 6;

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
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab":
                    #region ===== Tab 7 =====

                    tabValue = 7;

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

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExternalObliquesRight", Musculoskeletal_Mmt_ExternalObliquesRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExternalObliquesLeft", Musculoskeletal_Mmt_ExternalObliquesLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Back_Extensors_cmt", Musculoskeletal_Back_Extensors_cmt.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rectus_Abdominis_cmt", Musculoskeletal_Rectus_Abdominis_cmt.Text);

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Ta_Left", Musculoskeletal_Mmt_Ta_Left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Ta_Right", Musculoskeletal_Mmt_Ta_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Hamstring_left", Musculoskeletal_Mmt_Hamstring_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Hamstring_Right", Musculoskeletal_Mmt_Hamstring_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_adductors_left", Musculoskeletal_Mmt_adductors_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_adductors_right", Musculoskeletal_Mmt_adductors_right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_hipFlexor_left", Musculoskeletal_Mmt_hipFlexor_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_hipFlexor_Right", Musculoskeletal_Mmt_hipFlexor_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_biceps_left", Musculoskeletal_Mmt_biceps_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_biceps_right", Musculoskeletal_Mmt_biceps_right.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab":
                    #region ===== Tab 8 =====

                    tabValue = 8;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SelfRegulation", txtSelfRegulation.Text);
                    cmd.Parameters.AddWithValue("@Arousal", txtArousal.Text);
                    cmd.Parameters.AddWithValue("@Attention", txtAttention.Text);
                    cmd.Parameters.AddWithValue("@Affect", txtAffect.Text);
                    cmd.Parameters.AddWithValue("@Action", txtAction.Text);
                    cmd.Parameters.AddWithValue("@Cognition", txtCognition.Text);
                    cmd.Parameters.AddWithValue("@GI", txtGI.Text);
                    cmd.Parameters.AddWithValue("@Respiratory", txtRespiratory.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular", txtCardiovascular.Text);
                    cmd.Parameters.AddWithValue("@SkinIntegumentary", txtSkinIntegumentary.Text);
                    cmd.Parameters.AddWithValue("@Nutrition", txtNutrition.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab":
                    #region ===== Tab 9 =====

                    tabValue = 9;

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
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab":
                    #region ===== Tab 10 =====
                    tabValue = 10;

                    string questions = string.Empty;
                    //string questions = string.Empty;
                    foreach (RepeaterItem item in rptQuestions.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            string QuestionNo = ((Label)item.FindControl("lblQuestionNo")).Text;
                            string Yes = ((CheckBox)item.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                            string No = ((CheckBox)item.FindControl("chkMonthNo")).Checked == true ? "1" : "0";
                            string Comment = ((TextBox)item.FindControl("txtMonthComment")).Text;
                            questions += QuestionNo + "$" + Yes + "$" + No + "$" + Comment + "~";
                        }
                    }
                    questions = questions.Remove(questions.Length - 0, 0);
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@score_Communication_2", score_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@Inter_Communication_2", Inter_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@GROSS_2", GROSS_2.Text);
                    cmd.Parameters.AddWithValue("@inter_Gross_2 ", inter_Gross_2.Text);
                    cmd.Parameters.AddWithValue("@FINE_2", FINE_2.Text);
                    cmd.Parameters.AddWithValue("@inter_FINE_2", inter_FINE_2.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_2", PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PROBLEM_2", inter_PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_2", PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PERSONAL_2", inter_PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@Comm_3", Comm_3.Text);
                    cmd.Parameters.AddWithValue("@inter_3", inter_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_3", GROSS_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_3", GROSS_inter_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_3", FINE_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_3", FINE_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_3", PROBLEM_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_3", PROBLEM_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_3", PERSONAL_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_3", PERSONAL_inter_3.Text);
                    cmd.Parameters.AddWithValue("@Communication_6", Communication_6.Text);
                    cmd.Parameters.AddWithValue("@comm_inter_6", comm_inter_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_6", GROSS_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_6", GROSS_inter_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_6", FINE_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_6", FINE_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_6", PROBLEM_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_6", PROBLEM_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_6", PERSONAL_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_6", PERSONAL_inter_6.Text);
                    cmd.Parameters.AddWithValue("@comm_7", comm_7.Text);
                    cmd.Parameters.AddWithValue("@inter_7", inter_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_7", GROSS_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_7", GROSS_inter_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_7", FINE_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_7", FINE_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_7", PROBLEM_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_7", PROBLEM_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_7", PERSONAL_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_7", PERSONAL_inter_7.Text);
                    cmd.Parameters.AddWithValue("@comm_9", comm_9.Text);
                    cmd.Parameters.AddWithValue("@inter_9", inter_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_9", GROSS_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_9", GROSS_inter_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_9", FINE_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_9", FINE_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_9", PROBLEM_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_9", PROBLEM_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_9", PERSONAL_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_9", PERSONAL_inter_9.Text);
                    cmd.Parameters.AddWithValue("@comm_10", comm_10.Text);
                    cmd.Parameters.AddWithValue("@inter_10", inter_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_10 ", GROSS_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_10", GROSS_inter_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_10", FINE_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_10", FINE_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_10", PROBLEM_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_10", PROBLEM_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_10", PERSONAL_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_10", PERSONAL_inter_10.Text);
                    cmd.Parameters.AddWithValue("@comm_11", comm_11.Text);
                    cmd.Parameters.AddWithValue("@inter_11", inter_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_11", GROSS_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_11", GROSS_inter_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_11", FINE_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_11", FINE_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_11", PROBLEM_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_11", PROBLEM_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_11", PERSONAL_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_11", PERSONAL_inter_11.Text);
                    cmd.Parameters.AddWithValue("@comm_13", comm_13.Text);
                    cmd.Parameters.AddWithValue("@inter_13", inter_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_13", GROSS_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_13", GROSS_inter_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_13", FINE_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_13", FINE_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_13", PROBLEM_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_13", PROBLEM_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_13", PERSONAL_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_13", PERSONAL_inter_13.Text);
                    cmd.Parameters.AddWithValue("@comm_15", comm_15.Text);
                    cmd.Parameters.AddWithValue("@inter_15", inter_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_15", GROSS_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_15", GROSS_inter_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_15", FINE_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_15", FINE_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_15", PROBLEM_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_15", PROBLEM_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_15", PERSONAL_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_15", PERSONAL_inter_15.Text);
                    cmd.Parameters.AddWithValue("@comm_17", comm_17.Text);
                    cmd.Parameters.AddWithValue("@inter_17", inter_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_17", GROSS_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_17", GROSS_inter_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_17", FINE_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_17", FINE_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_17", PROBLEM_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_17", PROBLEM_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_17", PERSONAL_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_17", PERSONAL_inter_17.Text);
                    cmd.Parameters.AddWithValue("@comm_19", comm_19.Text);
                    cmd.Parameters.AddWithValue("@inter_19", inter_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_19", GROSS_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_19", GROSS_inter_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_19", FINE_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_19", FINE_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_19", PROBLEM_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_19", PROBLEM_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_19", PERSONAL_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_19", PERSONAL_inter_19.Text);
                    cmd.Parameters.AddWithValue("@comm_21", comm_21.Text);
                    cmd.Parameters.AddWithValue("@inter_21", inter_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_21", GROSS_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_21", GROSS_inter_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_21", FINE_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_21", FINE_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_21", PROBLEM_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_21", PROBLEM_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_21", PERSONAL_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_21", PERSONAL_inter_21.Text);
                    cmd.Parameters.AddWithValue("@comm_23", comm_23.Text);
                    cmd.Parameters.AddWithValue("@inter_23", inter_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_23", GROSS_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_23", GROSS_inter_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_23", FINE_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_23", FINE_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_23", PROBLEM_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_23", PROBLEM_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_23", PERSONAL_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_23", PERSONAL_inter_23.Text);
                    cmd.Parameters.AddWithValue("@comm_25", comm_25.Text);
                    cmd.Parameters.AddWithValue("@inter_25", inter_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_25", GROSS_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_25", GROSS_inter_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_25", FINE_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_25", FINE_inter_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_25", PROBLEM_25.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_25", PERSONAL_25.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_25", PERSONAL_inter_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_25", PROBLEM_inter_25.Text);
                    cmd.Parameters.AddWithValue("@comm_28", comm_28.Text);
                    cmd.Parameters.AddWithValue("@inter_28", inter_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_28", GROSS_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_28", GROSS_inter_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_28", FINE_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_28", FINE_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_28", PROBLEM_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_28", PROBLEM_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_28", PERSONAL_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_28", PERSONAL_inter_28.Text);
                    cmd.Parameters.AddWithValue("@comm_31", comm_31.Text);
                    cmd.Parameters.AddWithValue("@inter_31", inter_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_31", GROSS_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_31", GROSS_inter_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_31", FINE_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_31", FINE_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_31", PROBLEM_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_31", PROBLEM_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_31", PERSONAL_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_31", PERSONAL_inter_31.Text);
                    cmd.Parameters.AddWithValue("@comm_34", comm_34.Text);
                    cmd.Parameters.AddWithValue("@inter_34", inter_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_34", GROSS_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_34", GROSS_inter_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_34", FINE_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_34", FINE_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_34", PROBLEM_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_34", PROBLEM_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_34", PERSONAL_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_34", PERSONAL_inter_34.Text);
                    cmd.Parameters.AddWithValue("@comm_42", comm_42.Text);
                    cmd.Parameters.AddWithValue("@inter_42", inter_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_42", GROSS_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_42", GROSS_inter_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_42", FINE_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_42", FINE_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_42", PROBLEM_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_42", PROBLEM_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_42", PERSONAL_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_42", PERSONAL_inter_42.Text);
                    cmd.Parameters.AddWithValue("@comm_45", comm_45.Text);
                    cmd.Parameters.AddWithValue("@inter_45", inter_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_45", GROSS_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_45", GROSS_inter_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_45   ", FINE_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_45   ", FINE_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_45", PROBLEM_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_45", PROBLEM_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_45", PERSONAL_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_45", PERSONAL_inter_45.Text);
                    cmd.Parameters.AddWithValue("@comm_51", comm_51.Text);
                    cmd.Parameters.AddWithValue("@inter_51", inter_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_51", GROSS_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_51", GROSS_inter_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_51", FINE_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_51", FINE_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_51", PROBLEM_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_51", PROBLEM_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_51", PERSONAL_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_51", PERSONAL_inter_51.Text);
                    cmd.Parameters.AddWithValue("@comm_60", comm_60.Text);
                    cmd.Parameters.AddWithValue("@inter_60", inter_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_60", GROSS_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_60", GROSS_inter_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_60", FINE_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_60", FINE_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_60", PROBLEM_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_60", PROBLEM_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_60", PERSONAL_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_60", PERSONAL_inter_60.Text);

                    cmd.Parameters.AddWithValue("@MONTHS", SelectMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@questions", questions);



                    //cmd.Parameters.AddWithValue("lblQuestionNo", lblQuestionNo.Text);
                    //cmd.Parameters.AddWithValue("chkMonthYes", chkMonthYes);
                    //cmd.Parameters.AddWithValue("chkMonthNo", chkMonthNo);
                    //cmd.Parameters.AddWithValue("txtMonthComment", txtMonthComment.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab":
                    #region ===== Tab 11 =====
                    tabValue = 11;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    string ABILITY_questions = string.Empty;
                    foreach (RepeaterItem item in abilityQuestionsParent.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Repeater rpt = ((Repeater)item.FindControl("abilityQuestionsChild"));

                            if (rpt != null)
                            {
                                foreach (RepeaterItem rptitem in rpt.Items)
                                {
                                    string categoryId = ((Label)rptitem.FindControl("lblCategoryId")).Text;
                                    string questionNO = ((Label)rptitem.FindControl("abilityquestionNO")).Text;
                                    string Yes = ((CheckBox)rptitem.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                                    string No = ((CheckBox)rptitem.FindControl("chkMonthNo")).Checked == true ? "1" : "0";

                                    ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
                                }
                            }
                        }
                    }
                    if (ABILITY_questions.Length > 0)
                    {
                        ABILITY_questions = ABILITY_questions.Remove(ABILITY_questions.Length - 1, 1);
                        string abilityQuestionsChild = string.Empty;
                    }
                    cmd.Parameters.AddWithValue("@ABILITY_months", MonthSelect.Text);
                    cmd.Parameters.AddWithValue("@ability_TOTAL", ability_TOTAL.Text);
                    cmd.Parameters.AddWithValue("@ability_COMMENTS", ability_COMMENTS.Text);
                    cmd.Parameters.AddWithValue("@ABILITY_questions", ABILITY_questions);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab":
                    #region ===== Tab 12 =====

                    tabValue = 12;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@GMFCS_I", GMFCSCheckBoxI.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_II", GMFCSCheckBoxII.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_III", GMFCSCheckBoxIII.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_IV", GMFCSCheckBoxIV.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_V", GMFCSCheckBoxV.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_LyingRolling", txtGmfm_LyingRolling.Text);
                    //cmd.Parameters.AddWithValue("@chkI_LyingRolling", chkI_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_LyingRolling", chkII_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_LyingRolling", chkIII_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_LyingRolling", chkIV_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_LyingRolling", chkV_LyingRolling.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_Sitting", txtGmfm_Sitting.Text);
                    //cmd.Parameters.AddWithValue("@chkI_Sitting", chkI_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_Sitting", chkII_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_Sitting", chkIII_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_Sitting", chkIV_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_Sitting", chkV_Sitting.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_KneelingCrawling", txtGmfm_KneelingCrawling.Text);
                    //cmd.Parameters.AddWithValue("@chkI_KneelingCrawling", chkI_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_KneelingCrawling", chkII_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_KneelingCrawling", chkIII_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_KneelingCrawling", chkIV_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_KneelingCrawling", chkV_KneelingCrawling.Checked);


                    cmd.Parameters.AddWithValue("@Gmfm_Standing", txtGmfm_Standing.Text);
                    //cmd.Parameters.AddWithValue("@chkI_Standing", chkI_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_Standing", chkII_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_Standing", chkIII_Standing.Checked);Neuromotor_Recruitment_Initial
                    //cmd.Parameters.AddWithValue("@chkIV_Standing", chkIV_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_Standing", chkV_Standing.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_RunningJumping", txtGmfm_RunningJumping.Text);
                    cmd.Parameters.AddWithValue("@txtGmfm_TotalScore", txtGmfm_TotalScore.Text);
                    //cmd.Parameters.AddWithValue("@chkI_RunningJumping", chkI_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_RunningJumping", chkII_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_RunningJumping", chkIII_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_RunningJumping", chkIV_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_RunningJumping", chkV_RunningJumping.Checked);



                    cmd.Parameters.AddWithValue("@MACs_I", chkMACs_I.Checked);
                    cmd.Parameters.AddWithValue("@MACs_II", chkMACs_II.Checked);
                    cmd.Parameters.AddWithValue("@MACs_III", chkMACs_III.Checked);
                    cmd.Parameters.AddWithValue("@MACs_IV", chkMACs_IV.Checked);
                    cmd.Parameters.AddWithValue("@MACs_V", chkMACs_V.Checked);


                    cmd.Parameters.AddWithValue("@FMS_I", chkFMS_I.Checked);
                    cmd.Parameters.AddWithValue("@FMS_II", chkFMS_II.Checked);
                    cmd.Parameters.AddWithValue("@FMS_III", chkFMS_III.Checked);
                    cmd.Parameters.AddWithValue("@FMS_IV", chkFMS_IV.Checked);
                    cmd.Parameters.AddWithValue("@FMS_V", chkFMS_V.Checked);


                    cmd.Parameters.AddWithValue("@Barry_I", chkBarry_I.Checked);
                    cmd.Parameters.AddWithValue("@Barry_II", chkBarry_II.Checked);
                    cmd.Parameters.AddWithValue("@Barry_III", chkBarry_III.Checked);
                    cmd.Parameters.AddWithValue("@Barry_IV", chkBarry_IV.Checked);
                    cmd.Parameters.AddWithValue("@Barry_V", chkBarry_V.Checked);
                    cmd.Parameters.AddWithValue("@Barry_VI", chkBarry_VI.Checked);
                    cmd.Parameters.AddWithValue("@Barry_albright_txt", Barry_albright_txt.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab":
                    #region ===== Tab 13 =====
                    tabValue = 13;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@TestMeassures_GrossMotor", TestMeassures_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_FineMotor", TestMeassures_FineMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverLanguage", TestMeassures_DenverLanguage.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverPersonal", TestMeassures_DenverPersonal.Text);
                    cmd.Parameters.AddWithValue("@Tests_cmt", Tests_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab":
                    #region ===== Tab 14 =====
                    tabValue = 14;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@General_Processing", General_Processing.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY_Processing", AUDITORY_Processing.Text);
                    cmd.Parameters.AddWithValue("@VISUAL_Processing", VISUAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@TOUCH_Processing", TOUCH_Processing.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT_Processing", MOVEMENT_Processing.Text);
                    cmd.Parameters.AddWithValue("@ORAL_Processing", ORAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@Raw_score", Raw_score.Text);
                    cmd.Parameters.AddWithValue("@Total_rawscore", Total_rawscore.Text);
                    cmd.Parameters.AddWithValue("@Interpretation", Interpretation.Text);
                    cmd.Parameters.AddWithValue("@Comments_1", Comments_1.Text);
                    cmd.Parameters.AddWithValue("@Score_seeking", Score_seeking.Text);
                    cmd.Parameters.AddWithValue("@SEEKING", SEEKING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Avoiding", Score_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@AVOIDING", AVOIDING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_sensitivity", Score_sensitivity.Text);
                    cmd.Parameters.AddWithValue("@SENSITIVITY_2", SENSITIVITY_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Registration", Score_Registration.Text);
                    cmd.Parameters.AddWithValue("@REGISTRATION", REGISTRATION.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_general", Score_general.Text);
                    cmd.Parameters.AddWithValue("@GENERAL", GENERAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Auditory", Score_Auditory.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY", AUDITORY.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_visual", Score_visual.Text);
                    cmd.Parameters.AddWithValue("@VISUAL", VISUAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_touch", Score_touch.Text);
                    cmd.Parameters.AddWithValue("@TOUCH", TOUCH.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_movement", Score_movement.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT", MOVEMENT.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_oral", Score_oral.Text);
                    cmd.Parameters.AddWithValue("@ORAL", ORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_behavioural", Score_behavioural.Text);
                    cmd.Parameters.AddWithValue("@BEHAVIORAL", BEHAVIORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_2", Comments_2.Text);
                    cmd.Parameters.AddWithValue("@SPchild_Seeker", SPchild_Seeker.Text);
                    cmd.Parameters.AddWithValue("@Seeking_Seeker", Seeking_Seeker.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Avoider", SPchild_Avoider.Text);
                    cmd.Parameters.AddWithValue("@Avoiding_Avoider", Avoiding_Avoider.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Sensor", SPchild_Sensor.Text);
                    cmd.Parameters.AddWithValue("@Sensitivity_Sensor", Sensitivity_Sensor.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Bystander", SPchild_Bystander.Text);
                    cmd.Parameters.AddWithValue("@Registration_Bystander", Registration_Bystander.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Auditory_3", SPchild_Auditory_3.Text);
                    cmd.Parameters.AddWithValue("@Auditory_3", Auditory_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Visual_3", SPchild_Visual_3.Text);
                    cmd.Parameters.AddWithValue("@Visual_3", Visual_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Touch_3", SPchild_Touch_3.Text);
                    cmd.Parameters.AddWithValue("@Touch_3", Touch_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Movement_3", SPchild_Movement_3.Text);
                    cmd.Parameters.AddWithValue("@Movement_3", Movement_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Body_position", SPchild_Body_position.Text);
                    cmd.Parameters.AddWithValue("@Body_position", Body_position.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Oral_3", SPchild_Oral_3.Text);
                    cmd.Parameters.AddWithValue("@Oral_3", Oral_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Conduct_3", SPchild_Conduct_3.Text);
                    cmd.Parameters.AddWithValue("@Conduct_3", Conduct_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Social_emotional", SPchild_Social_emotional.Text);
                    cmd.Parameters.AddWithValue("@Social_emotional", Social_emotional.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Attentional_3", SPchild_Attentional_3.Text);
                    cmd.Parameters.AddWithValue("@Attentional_3", Attentional_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_3", Comments_3.Text);
                    cmd.Parameters.AddWithValue("@SPAdult_Low_Registration", SPAdult_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration", Low_Registration.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_seeking", SPAdult_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking", Sensory_seeking.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Sensitivity", SPAdult_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity", Sensory_Sensitivity.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Avoiding", SPAdult_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding", Sensory_Avoiding.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_4", Comments_4.Text);
                    cmd.Parameters.AddWithValue("@SP_Low_Registration64", SP_Low_Registration64.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_5", Low_Registration_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_seeking_64", SP_Sensory_seeking_64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_5", Sensory_seeking_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Sensitivity64", SP_Sensory_Sensitivity64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_5", Sensory_Sensitivity_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Avoiding64", SP_Sensory_Avoiding64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_5", Sensory_Avoiding_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_5", Comments_5.Text);
                    cmd.Parameters.AddWithValue("@Older_Low_Registration", Older_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_6", Low_Registration_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_seeking", Older_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_6", Sensory_seeking_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Sensitivity", Older_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_6", Sensory_Sensitivity_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Avoiding", Older_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_6", Sensory_Avoiding_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_6", Comments_6.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab":
                    #region ===== Tab 15 =====

                    tabValue = 15;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Summary", Evaluation_Goal_Summary.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_System_Impairment", Evaluation_System_Impairment.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_LTG", Evaluation_LTG.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_STG", Evaluation_STG.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan_advice", Evalution_Plan_advice.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan__Frequency", Evalution_Plan__Frequency.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan_Adjuncts", Evalution_Plan_Adjuncts.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan__Education", Evalution_Plan__Education.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab":
                    #region ===== Tab 16 =====

                    tabValue = 16;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Doctor_Physioptherapist", @Doctor_Physioptherapist.Text);
                    cmd.Parameters.AddWithValue("@Doctor_Occupational", @Doctor_Occupational.Text);
                    //cmd.Parameters.AddWithValue("@Doctor_EnterReport", @Doctor_EnterReport.Text);
                    //   cmd.Parameters.AddWithValue("@Doctor_EnterReport", DBNull.Value);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);
                    Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/NDT_SFT_REVAL_RPT.aspx?record=" + Request.QueryString["record"]), true);
                    #endregion
                    break;
            }
            Session[DbHelper.Configuration.messageTextSession] = "NDT Report Save Succesfully";
            Session[DbHelper.Configuration.messageTypeSession] = "1";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
            bool IsFinal = txtFinal.Checked;
            bool IsGiven = txtGiven.Checked;
            DateTime GivenDate = new DateTime();
            if (IsGiven)
            {
                if (string.IsNullOrWhiteSpace(txtGivenDate.Text))
                {
                    DbHelper.Configuration.setAlert(Page, "Given Date is Required...", 2);
                    return;
                }
                else
                {
                    DateTime.TryParseExact(txtGivenDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                }
            }
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataSet ds = RDB.Get_NDT(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            SqlCommand cmd1 = new SqlCommand("SET_NDt_FLags");
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@AppointmentID", _appointmentID);
            cmd1.Parameters.AddWithValue("@IsGiven", IsGiven);
            cmd1.Parameters.AddWithValue("@IsFinal", IsFinal);
            cmd1.Parameters.AddWithValue("@GivenDate", IsGiven ? (object)GivenDate : DBNull.Value);
            db.DbUpdate(cmd1);
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

            int tabValue = 1;
            SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE");
            cmd.CommandType = CommandType.StoredProcedure;

            switch (this.hfdTabs.Value)
            {
                case "":
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab":
                    #region ===== Tab 1 =====

                    tabValue = 1;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Ability", FA_GrossMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_GrossMotor_Limit", FA_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Ability", FA_FineMotor_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_FineMotor_Limit", FA_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Ability", FA_Communication_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Communication_Limit", FA_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Ability", FA_Cognition_Ability.Text);
                    cmd.Parameters.AddWithValue("@FA_Cognition_Limit", FA_Cognition_Limit.Text);

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

                    cmd.Parameters.AddWithValue("@ParticipationAbility_GrossMotor", ParticipationAbility_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_GrossMotor_Limit", ParticipationAbility_GrossMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_FineMotor", ParticipationAbility_FineMotor.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_FineMotor_Limit", ParticipationAbility_FineMotor_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Communication", ParticipationAbility_Communication.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Communication_Limit", ParticipationAbility_Communication_Limit.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Cognition", ParticipationAbility_Cognition.Text);
                    cmd.Parameters.AddWithValue("@ParticipationAbility_Cognition_Limit", ParticipationAbility_Cognition_Limit.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Personal_Positive", Contextual_Personal_Positive.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Personal_Negative", Contextual_Personal_Negative.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Environmental_Positive", Contextual_Enviremental_Positive.Text);
                    cmd.Parameters.AddWithValue("@Contextual_Environmental_Negative", Contextual_Enviremental_Negative.Text);
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

                    string headPositions = string.Empty;
                    List<string> selectedPositions = new List<string>();
                    if (chkHead_Forward.Checked) { selectedPositions.Add(chkHead_Forward.Text.Trim().Replace(" ", "")); }
                    if (chkHead_Neutral.Checked) { selectedPositions.Add(chkHead_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkHead_PlagiocephalyRight.Checked) { selectedPositions.Add(chkHead_PlagiocephalyRight.Text.Trim().Replace(" ", "")); }
                    if (chkHead_PlagiocephalyLeft.Checked) { selectedPositions.Add(chkHead_PlagiocephalyLeft.Text.Trim().Replace(" ", "")); }
                    if (chkHead_FrontalBossing.Checked) { selectedPositions.Add(chkHead_FrontalBossing.Text.Trim().Replace(" ", "")); }
                    headPositions = string.Join(",", selectedPositions);
                    /***/
                    string shoulderPositions = string.Empty;
                    List<string> selectedShoulderPositions = new List<string>();
                    if (chkShoulder_BL.Checked) { selectedShoulderPositions.Add(chkShoulder_BL.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Right.Checked) { selectedShoulderPositions.Add(chkShoulder_Right.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Left.Checked) { selectedShoulderPositions.Add(chkShoulder_Left.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_InternallyRotated.Checked) { selectedShoulderPositions.Add(chkShoulder_InternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_ExternallyRotated.Checked) { selectedShoulderPositions.Add(chkShoulder_ExternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Elevated.Checked) { selectedShoulderPositions.Add(chkShoulder_Elevated.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Depressed.Checked) { selectedShoulderPositions.Add(chkShoulder_Depressed.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Protracted.Checked) { selectedShoulderPositions.Add(chkShoulder_Protracted.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Retracted.Checked) { selectedShoulderPositions.Add(chkShoulder_Retracted.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Abducted.Checked) { selectedShoulderPositions.Add(chkShoulder_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkShoulder_Neutral.Checked) { selectedShoulderPositions.Add(chkShoulder_Neutral.Text.Trim().Replace(" ", "")); }
                    shoulderPositions = string.Join(",", selectedShoulderPositions);
                    /***/
                    string neckPositions = string.Empty;
                    List<string> selectedNeckPositions = new List<string>();
                    if (chkNeck_BL.Checked) { selectedNeckPositions.Add(chkNeck_BL.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Right.Checked) { selectedNeckPositions.Add(chkNeck_Right.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Left.Checked) { selectedNeckPositions.Add(chkNeck_Left.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_LateralTilt.Checked) { selectedNeckPositions.Add(chkNeck_LateralTilt.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Hyperextended.Checked) { selectedNeckPositions.Add(chkNeck_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Flexed.Checked) { selectedNeckPositions.Add(chkNeck_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_ChinTuck.Checked) { selectedNeckPositions.Add(chkNeck_ChinTuck.Text.Trim().Replace(" ", "")); }
                    if (chkNeck_Neutral.Checked) { selectedNeckPositions.Add(chkNeck_Neutral.Text.Trim().Replace(" ", "")); }
                    neckPositions = string.Join(",", selectedNeckPositions);
                    /**/
                    string scapulaePositions = string.Empty;
                    List<string> selectedScapulaePositions = new List<string>();
                    if (chkScapulae_BL.Checked) { selectedScapulaePositions.Add(chkScapulae_BL.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Right.Checked) { selectedScapulaePositions.Add(chkScapulae_Right.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Left.Checked) { selectedScapulaePositions.Add(chkScapulae_Left.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Protracted.Checked) { selectedScapulaePositions.Add(chkScapulae_Protracted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Retracted.Checked) { selectedScapulaePositions.Add(chkScapulae_Retracted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Abducted.Checked) { selectedScapulaePositions.Add(chkScapulae_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Adducted.Checked) { selectedScapulaePositions.Add(chkScapulae_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Elevated.Checked) { selectedScapulaePositions.Add(chkScapulae_Elevated.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Depressed.Checked) { selectedScapulaePositions.Add(chkScapulae_Depressed.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Winging.Checked) { selectedScapulaePositions.Add(chkScapulae_Winging.Text.Trim().Replace(" ", "")); }
                    if (chkScapulae_Neutral.Checked) { selectedScapulaePositions.Add(chkScapulae_Neutral.Text.Trim().Replace(" ", "")); }
                    scapulaePositions = string.Join(",", selectedScapulaePositions);
                    /**/
                    string elbowPositions = string.Empty;
                    List<string> selectedElbowPositions = new List<string>();
                    if (chkElbow_BL.Checked) { selectedElbowPositions.Add(chkElbow_BL.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Right.Checked) { selectedElbowPositions.Add(chkElbow_Right.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Left.Checked) { selectedElbowPositions.Add(chkElbow_Left.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Flexed.Checked) { selectedElbowPositions.Add(chkElbow_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Extended.Checked) { selectedElbowPositions.Add(chkElbow_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkElbow_Neutral.Checked) { selectedElbowPositions.Add(chkElbow_Neutral.Text.Trim().Replace(" ", "")); }
                    elbowPositions = string.Join(",", selectedElbowPositions);
                    //**//
                    string forearmPositions = string.Empty;
                    List<string> selectedForearmPositions = new List<string>();
                    if (chkForearm_BL.Checked) { selectedForearmPositions.Add(chkForearm_BL.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Right.Checked) { selectedForearmPositions.Add(chkForearm_Right.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Left.Checked) { selectedForearmPositions.Add(chkForearm_Left.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Supinated.Checked) { selectedForearmPositions.Add(chkForearm_Supinated.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Pronated.Checked) { selectedForearmPositions.Add(chkForearm_Pronated.Text.Trim().Replace(" ", "")); }
                    if (chkForearm_Neutral.Checked) { selectedForearmPositions.Add(chkForearm_Neutral.Text.Trim().Replace(" ", "")); }
                    forearmPositions = string.Join(",", selectedForearmPositions);
                    //*//
                    string wristPositions = string.Empty;
                    List<string> selectedWristPositions = new List<string>();
                    if (chkWrist_BL.Checked) { selectedWristPositions.Add(chkWrist_BL.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Neutral.Checked) { selectedWristPositions.Add(chkWrist_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Right.Checked) { selectedWristPositions.Add(chkWrist_Right.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Left.Checked) { selectedWristPositions.Add(chkWrist_Left.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Flexed.Checked) { selectedWristPositions.Add(chkWrist_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_Extended.Checked) { selectedWristPositions.Add(chkWrist_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkWrist_UD.Checked) { selectedWristPositions.Add("UlnarDeviation"); }  // Handling special case
                    if (chkWrist_RD.Checked) { selectedWristPositions.Add("RadialDeviation"); } // Handling special case
                    wristPositions = string.Join(",", selectedWristPositions);
                    //**//
                    string handPositions = string.Empty;
                    List<string> selectedHandPositions = new List<string>();
                    if (chkHand_Fist.Checked) { selectedHandPositions.Add(chkHand_Fist.Text.Trim().Replace(" ", "")); }
                    if (chkHand_BL.Checked) { selectedHandPositions.Add(chkHand_BL.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Right.Checked) { selectedHandPositions.Add(chkHand_Right.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Left.Checked) { selectedHandPositions.Add(chkHand_Left.Text.Trim().Replace(" ", "")); }
                    if (chkHand_Neutral.Checked) { selectedHandPositions.Add(chkHand_Neutral.Text.Trim().Replace(" ", "")); }
                    handPositions = string.Join(",", selectedHandPositions);
                    //**//
                    string fingersPositions = string.Empty;
                    List<string> selectedFingersPositions = new List<string>();
                    if (chkFingers_BL.Checked) { selectedFingersPositions.Add(chkFingers_BL.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Right.Checked) { selectedFingersPositions.Add(chkFingers_Right.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Left.Checked) { selectedFingersPositions.Add(chkFingers_Left.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Flexed.Checked) { selectedFingersPositions.Add(chkFingers_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Extended.Checked) { selectedFingersPositions.Add(chkFingers_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkFingers_Neutral.Checked) { selectedFingersPositions.Add(chkFingers_Neutral.Text.Trim().Replace(" ", "")); }
                    fingersPositions = string.Join(",", selectedFingersPositions);
                    //*//
                    string thumbPositions = string.Empty;
                    List<string> selectedThumbPositions = new List<string>();
                    if (chkThumb_BL.Checked) { selectedThumbPositions.Add(chkThumb_BL.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Right.Checked) { selectedThumbPositions.Add(chkThumb_Right.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Left.Checked) { selectedThumbPositions.Add(chkThumb_Left.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Adducted.Checked) { selectedThumbPositions.Add(chkThumb_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Abducted.Checked) { selectedThumbPositions.Add(chkThumb_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkThumb_Neutral.Checked) { selectedThumbPositions.Add(chkThumb_Neutral.Text.Trim().Replace(" ", "")); }
                    thumbPositions = string.Join(",", selectedThumbPositions);
                    //*//
                    string thoracicSpinePositions = string.Empty;
                    List<string> selectedThoracicSpinePositions = new List<string>();
                    if (chkThoracicSpine_Rounded.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Rounded.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_Hyperextended.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_LaterallyFlexed.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_LaterallyFlexed.Text.Trim().Replace(" ", "")); }
                    if (chkThoracicSpine_Neutral.Checked) { selectedThoracicSpinePositions.Add(chkThoracicSpine_Neutral.Text.Trim().Replace(" ", "")); }
                    thoracicSpinePositions = string.Join(",", selectedThoracicSpinePositions);
                    //*//
                    string lumbarSpinePositions = string.Empty;
                    List<string> selectedLumbarSpinePositions = new List<string>();
                    if (chkLumbarSpine_Flattened.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Flattened.Text.Trim().Replace(" ", "")); }
                    if (chkLumbarSpine_Hyperextended.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkLumbarSpine_Neutral.Checked) { selectedLumbarSpinePositions.Add(chkLumbarSpine_Neutral.Text.Trim().Replace(" ", "")); }
                    lumbarSpinePositions = string.Join(",", selectedLumbarSpinePositions);
                    //**//
                    string pelvisPositions = string.Empty;
                    List<string> selectedPelvisPositions = new List<string>();
                    if (chkPelvis_Neutral.Checked) { selectedPelvisPositions.Add(chkPelvis_Neutral.Text.Trim().Replace(" ", "")); }
                    if (chkPelvis_AnteriorTilted.Checked) { selectedPelvisPositions.Add(chkPelvis_AnteriorTilted.Text.Trim().Replace(" ", "")); }
                    if (chkPelvis_PosteriorTilted.Checked) { selectedPelvisPositions.Add(chkPelvis_PosteriorTilted.Text.Trim().Replace(" ", "")); }
                    pelvisPositions = string.Join(",", selectedPelvisPositions);
                    //**//
                    string hipsPositions = string.Empty;
                    List<string> selectedHipsPositions = new List<string>();
                    if (chkHips_BL.Checked) { selectedHipsPositions.Add(chkHips_BL.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Right.Checked) { selectedHipsPositions.Add(chkHips_Right.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Left.Checked) { selectedHipsPositions.Add(chkHips_Left.Text.Trim().Replace(" ", "")); }
                    if (chkHips_LaterallyRotated.Checked) { selectedHipsPositions.Add(chkHips_LaterallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Abducted.Checked) { selectedHipsPositions.Add(chkHips_Abducted.Text.Trim().Replace(" ", "")); }
                    if (chkHips_InternallyRotated.Checked) { selectedHipsPositions.Add(chkHips_InternallyRotated.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Adducted.Checked) { selectedHipsPositions.Add(chkHips_Adducted.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Flexed.Checked) { selectedHipsPositions.Add(chkHips_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkHips_Neutral.Checked) { selectedHipsPositions.Add(chkHips_Neutral.Text.Trim().Replace(" ", "")); }
                    hipsPositions = string.Join(",", selectedHipsPositions);
                    //**//
                    string kneesPositions = string.Empty;
                    List<string> selectedKneesPositions = new List<string>();
                    if (chkKnees_BL.Checked) { selectedKneesPositions.Add(chkKnees_BL.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Hyperextended.Checked) { selectedKneesPositions.Add(chkKnees_Hyperextended.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Flexed.Checked) { selectedKneesPositions.Add(chkKnees_Flexed.Text.Trim().Replace(" ", "")); }
                    if (chkKnees_Neutral.Checked) { selectedKneesPositions.Add(chkKnees_Neutral.Text.Trim().Replace(" ", "")); }
                    kneesPositions = string.Join(",", selectedKneesPositions);
                    //**//
                    string anklePositions = string.Empty;
                    List<string> selectedAnklePositions = new List<string>();
                    if (chkAnkle_BL.Checked) { selectedAnklePositions.Add(chkAnkle_BL.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Plantarflexed.Checked) { selectedAnklePositions.Add(chkAnkle_Plantarflexed.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Dorsiflexed.Checked) { selectedAnklePositions.Add(chkAnkle_Dorsiflexed.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Inverted.Checked) { selectedAnklePositions.Add(chkAnkle_Inverted.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Everted.Checked) { selectedAnklePositions.Add(chkAnkle_Everted.Text.Trim().Replace(" ", "")); }
                    if (chkAnkle_Neutral.Checked) { selectedAnklePositions.Add(chkAnkle_Neutral.Text.Trim().Replace(" ", "")); }
                    anklePositions = string.Join(",", selectedAnklePositions);
                    //**//
                    string feetPositions = string.Empty;
                    List<string> selectedFeetPositions = new List<string>();
                    if (chkFeet_BL.Checked) { selectedFeetPositions.Add(chkFeet_BL.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Right.Checked) { selectedFeetPositions.Add(chkFeet_Right.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Left.Checked) { selectedFeetPositions.Add(chkFeet_Left.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Pronated.Checked) { selectedFeetPositions.Add(chkFeet_Pronated.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Supinated.Checked) { selectedFeetPositions.Add(chkFeet_Supinated.Text.Trim().Replace(" ", "")); }
                    if (chkFeet_Neutral.Checked) { selectedFeetPositions.Add(chkFeet_Neutral.Text.Trim().Replace(" ", "")); }
                    feetPositions = string.Join(",", selectedFeetPositions);
                    //**//
                    string toesPositions = string.Empty;
                    List<string> selectedToesPositions = new List<string>();
                    if (chkToes_BL.Checked) { selectedToesPositions.Add(chkToes_BL.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Right.Checked) { selectedToesPositions.Add(chkToes_Right.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Left.Checked) { selectedToesPositions.Add(chkToes_Left.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Curled.Checked) { selectedToesPositions.Add(chkToes_Curled.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Extended.Checked) { selectedToesPositions.Add(chkToes_Extended.Text.Trim().Replace(" ", "")); }
                    if (chkToes_Neutral.Checked) { selectedToesPositions.Add(chkToes_Neutral.Text.Trim().Replace(" ", "")); }
                    toesPositions = string.Join(",", selectedToesPositions);
                    //**//
                    string bosPositions = string.Empty;
                    List<string> selectedBOSPositions = new List<string>();
                    if (chkBOS_Narrow.Checked) { selectedBOSPositions.Add(chkBOS_Narrow.Text.Trim().Replace(" ", "")); }
                    if (chkBOS_Wide.Checked) { selectedBOSPositions.Add(chkBOS_Wide.Text.Trim().Replace(" ", "")); }
                    bosPositions = string.Join(",", selectedBOSPositions);
                    //*?/
                    string stabilityMethods = string.Empty;
                    List<string> selectedStabilityMethods = new List<string>();
                    if (chkStability_PosturalTone.Checked) { selectedStabilityMethods.Add(chkStability_PosturalTone.Text.Trim()); }
                    if (chkStability_LockingJoints.Checked) { selectedStabilityMethods.Add(chkStability_LockingJoints.Text.Trim()); }
                    if (chkStability_BroadeningBOS.Checked) { selectedStabilityMethods.Add(chkStability_BroadeningBOS.Text.Trim()); }
                    stabilityMethods = string.Join(",", selectedStabilityMethods);
                    string alignmentType = "";

                    if (chkSymmetric.Checked)
                        alignmentType = "Symmetric";
                    else if (chkAsymmetric.Checked)
                        alignmentType = "Asymmetric";
                    //cmd.Parameters.AddWithValue("@Multi_posture_Alligmnet_right", txtRight.Text);
                    //cmd.Parameters.AddWithValue("@Multi_posture_headAlligmnet_left", txtLeft.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_headAlignment_AlignmentType", alignmentType);
                    cmd.Parameters.AddWithValue("@Multi_posture_head", headPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_Shoulder", shoulderPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_neck", neckPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_scapulae", scapulaePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_elbow", elbowPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_forarm", forearmPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_wrist", wristPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_hand", handPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_finger", fingersPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_thumb", thumbPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_ribcage", Posture_Gen_Ribcage.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_thoracicspine", thoracicSpinePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_lumbarspine", lumbarSpinePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_pelvis", pelvisPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_hips", hipsPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_knees", kneesPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_ankle", anklePositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_feet", feetPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_toes", toesPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_bos", bosPositions);
                    cmd.Parameters.AddWithValue("@Multi_posture_stabiltymethod", stabilityMethods);
                    cmd.Parameters.AddWithValue("@Multi_posture_com_cog", txtCOM_COG.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_cheeks", txtCheeks.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_chin", txtChin.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_teeth", txtTeeth.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_toungh", txtTongue.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_mouth", txtMouth.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_lips", txtLips.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_Stability", txtStability_Comments.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_anticipatory", txtAnticipatoryControl.Text);
                    cmd.Parameters.AddWithValue("@Multi_posture_postural", txtPosturalCounterBalance.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab";
                    }

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab":
                    #region ===== Tab 4 =====

                    tabValue = 4;
                    string Posture_Alignment_Type = string.Empty;
                    if (Multi_Movement_TypeOf_1.Checked) { Posture_Alignment_Type = Multi_Movement_TypeOf_1.Text.Trim(); }
                    if (Multi_Movement_TypeOf_2.Checked) { Posture_Alignment_Type = Multi_Movement_TypeOf_2.Text.Trim(); }

                    string Multi_Movement_Type = string.Empty;

                    if (Multi_Movement_Sagittal != null && Multi_Movement_Sagittal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Sagittal.Text.Trim();
                    }
                    else if (Multi_Movement_Coronal != null && Multi_Movement_Coronal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Coronal.Text.Trim();
                    }
                    else if (Multi_Movement_Frontal != null && Multi_Movement_Frontal.Checked)
                    {
                        Multi_Movement_Type = Multi_Movement_Frontal.Text.Trim();
                    }

                    string Multi_Movement_WeightShift = string.Join(",", Movement_WeightShift.Items.Cast<ListItem>()
                                       .Where(i => i.Selected)
                                       .Select(i => i.Value));
                    string movementInterlimb = string.Empty;
                    //**
                    List<string> selectedMovementInterlimb = new List<string>();
                    if (Movement_Interlimb_SpineToShoulder.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_SpineToShoulder.Text.Trim()); }
                    if (Movement_Interlimb_Scapulohumeral.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_Scapulohumeral.Text.Trim()); }
                    if (Movement_Interlimb_Pelvifemoral.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_Pelvifemoral.Text.Trim()); }
                    if (Movement_Interlimb_WithinUL.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_WithinUL.Text.Trim()); }
                    if (Movement_Interlimb_WithinLL.Checked) { selectedMovementInterlimb.Add(Movement_Interlimb_WithinLL.Text.Trim()); }
                    movementInterlimb = string.Join(",", selectedMovementInterlimb);
                    //**
                    string intralimbDissociation = string.Empty;
                    List<string> selectedIntralimbDissociation = new List<string>();
                    if (Movement_Intralimb_LE.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_LE.Text.Trim()); }
                    if (Movement_Intralimb_UE.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_UE.Text.Trim()); }
                    if (Movement_Intralimb_Spine.Checked) { selectedIntralimbDissociation.Add(Movement_Intralimb_Spine.Text.Trim()); }
                    intralimbDissociation = string.Join(",", selectedIntralimbDissociation);
                    //**
                    string movementOveruse = string.Empty;
                    List<string> selectedOveruseOptions = new List<string>();
                    if (chkLeanMuscle.Checked) { selectedOveruseOptions.Add(chkLeanMuscle.Text.Trim()); }
                    if (chkLockingJoints.Checked) { selectedOveruseOptions.Add(chkLockingJoints.Text.Trim()); }
                    if (chkBroadBOS.Checked) { selectedOveruseOptions.Add(chkBroadBOS.Text.Trim()); }
                    if (chkGeneralPosture.Checked) { selectedOveruseOptions.Add(chkGeneralPosture.Text.Trim()); }
                    movementOveruse = string.Join(",", selectedOveruseOptions);
                    //
                    List<string> selectedUpperLimb = new List<string>();
                    if (Movement_UpperLimb_Inner.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Inner.Text.Trim()); }
                    if (Movement_UpperLimb_Mid.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Mid.Text.Trim()); }
                    if (Movement_UpperLimb_Outer.Checked) { selectedUpperLimb.Add(Movement_UpperLimb_Outer.Text.Trim()); }
                    string upperLimbSelection = string.Join(",", selectedUpperLimb);
                    //
                    List<string> selectedLowerLimb = new List<string>();
                    if (Movement_LowerLimb_Inner.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Inner.Text.Trim()); }
                    if (Movement_LowerLimb_Mid.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Mid.Text.Trim()); }
                    if (Movement_LowerLimb_Outer.Checked) { selectedLowerLimb.Add(Movement_LowerLimb_Outer.Text.Trim()); }
                    string lowerLimbSelection = string.Join(",", selectedLowerLimb);
                    //
                    List<string> selectedCervicalSpine = new List<string>();
                    if (Movement_CervicalSpine_Inner.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Inner.Text.Trim()); }
                    if (Movement_CervicalSpine_Mid.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Mid.Text.Trim()); }
                    if (Movement_CervicalSpine_Outer.Checked) { selectedCervicalSpine.Add(Movement_CervicalSpine_Outer.Text.Trim()); }
                    string cervicalSpineSelection = string.Join(",", selectedCervicalSpine);
                    //
                    List<string> selectedThoracicSpine = new List<string>();
                    if (Movement_ThoracicSpine_Inner.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Inner.Text.Trim()); }
                    if (Movement_ThoracicSpine_Mid.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Mid.Text.Trim()); }
                    if (Movement_ThoracicSpine_Outer.Checked) { selectedThoracicSpine.Add(Movement_ThoracicSpine_Outer.Text.Trim()); }
                    string thoracicSpineSelection = string.Join(",", selectedThoracicSpine);
                    //
                    string movementStability = string.Empty;
                    List<string> selectedStabilityOptions = new List<string>();
                    if (chkOveruseMomentum.Checked) { selectedStabilityOptions.Add(chkOveruseMomentum.Text.Trim()); }
                    if (chkIncreasedBOS.Checked) { selectedStabilityOptions.Add(chkIncreasedBOS.Text.Trim()); }
                    if (chkIncreasingPosturalTone.Checked) { selectedStabilityOptions.Add(chkIncreasingPosturalTone.Text.Trim()); }
                    movementStability = string.Join(",", selectedStabilityOptions);
                    //
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@Movement_Inertia", Movement_Inertia.Text);
                    cmd.Parameters.AddWithValue("@Posture_Alignment_Type", Posture_Alignment_Type);
                    cmd.Parameters.AddWithValue("@Multi_Movement_Type", Multi_Movement_Type);
                    cmd.Parameters.AddWithValue("@Multi_Movement_WeightShift", Multi_Movement_WeightShift);
                    cmd.Parameters.AddWithValue("@Multi_Movement_interlimb", movementInterlimb);
                    cmd.Parameters.AddWithValue("@Multi_Movement_intralimb", intralimbDissociation);
                    cmd.Parameters.AddWithValue("@Multi_Movement_overuse", movementOveruse);
                    cmd.Parameters.AddWithValue("@Multi_Movement_Bal_maintain", Movement_Balance_Maintain.Text);
                    cmd.Parameters.AddWithValue("@Multi_Movement_BAl_during", Movement_Balance_During.Text);
                    cmd.Parameters.AddWithValue("@UpperLimb_Movement", upperLimbSelection);
                    cmd.Parameters.AddWithValue("@LowerLimb_Movement", lowerLimbSelection);
                    cmd.Parameters.AddWithValue("@CervicalSpine_Movement", cervicalSpineSelection);
                    cmd.Parameters.AddWithValue("@ThoracicSpine_Movement", thoracicSpineSelection);
                    cmd.Parameters.AddWithValue("@Multi_Movement_statbilty", movementStability);
                    cmd.Parameters.AddWithValue("@Gene_obsr_comments", Gene_obsr_comments_txt.Text);

                    cmd.Parameters.AddWithValue("@txtSoinePoor", chkSoinePoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtSoineFair", chkSoineFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtSoineGood", chkSoineGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtScapuloPoor", chkScapuloPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtScapuloFair", chkScapuloFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtScapuloGood", chkScapuloGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtPelviPoor", chkPelviPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtPelviFair", chkPelviFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtPelviGood", chkPelviGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtWithinUlPoor", chkWithinUlPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtWithinUlFair", chkWithinUlFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtWithinUlGood", chkWithinUlGood.Checked ? "Good" : "");

                    cmd.Parameters.AddWithValue("@txtWithinLlPoor", chkWithinLlPoor.Checked ? "Poor" : "");
                    cmd.Parameters.AddWithValue("@txtWithinLlFair", chkWithinLlFair.Checked ? "Fair" : "");
                    cmd.Parameters.AddWithValue("@txtWithinLlGood", chkWithinLlGood.Checked ? "Good" : "");


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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab";
                    }

                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab":
                    #region ===== Tab 5 =====
                    tabValue = 5;
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
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Control", Neurometer_Initialigy_Control.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Termination", Neurometer_Initialigy_Termination.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_Sustainance", Neurometer_Initialigy_Sustainance.Text);
                    cmd.Parameters.AddWithValue("@Neurometer_Initialigy_initial", Neurometer_Initialigy_initial.Text);


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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab":
                    #region ===== Tab 6 =====
                    tabValue = 6;

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab":
                    #region ===== Tab 7 =====

                    tabValue = 7;
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

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Ta_Left", Musculoskeletal_Mmt_Ta_Left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Ta_Right", Musculoskeletal_Mmt_Ta_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Hamstring_left", Musculoskeletal_Mmt_Hamstring_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_Hamstring_Right", Musculoskeletal_Mmt_Hamstring_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_adductors_left", Musculoskeletal_Mmt_adductors_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_adductors_right", Musculoskeletal_Mmt_adductors_right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_hipFlexor_left", Musculoskeletal_Mmt_hipFlexor_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_hipFlexor_Right", Musculoskeletal_Mmt_hipFlexor_Right.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_biceps_left", Musculoskeletal_Mmt_biceps_left.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_biceps_right", Musculoskeletal_Mmt_biceps_right.Text);

                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExternalObliquesRight", Musculoskeletal_Mmt_ExternalObliquesRight.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Mmt_ExternalObliquesLeft", Musculoskeletal_Mmt_ExternalObliquesLeft.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Back_Extensors_cmt", Musculoskeletal_Back_Extensors_cmt.Text);
                    cmd.Parameters.AddWithValue("@Musculoskeletal_Rectus_Abdominis_cmt", Musculoskeletal_Rectus_Abdominis_cmt.Text);


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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab":
                    #region ===== Tab 8 =====

                    tabValue = 8;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SelfRegulation", txtSelfRegulation.Text);
                    cmd.Parameters.AddWithValue("@Arousal", txtArousal.Text);
                    cmd.Parameters.AddWithValue("@Attention", txtAttention.Text);
                    cmd.Parameters.AddWithValue("@Affect", txtAffect.Text);
                    cmd.Parameters.AddWithValue("@Action", txtAction.Text);
                    cmd.Parameters.AddWithValue("@Cognition", txtCognition.Text);
                    cmd.Parameters.AddWithValue("@GI", txtGI.Text);
                    cmd.Parameters.AddWithValue("@Respiratory", txtRespiratory.Text);
                    cmd.Parameters.AddWithValue("@Cardiovascular", txtCardiovascular.Text);
                    cmd.Parameters.AddWithValue("@SkinIntegumentary", txtSkinIntegumentary.Text);
                    cmd.Parameters.AddWithValue("@Nutrition", txtNutrition.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab":
                    #region ===== Tab 9 =====

                    tabValue = 9;

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
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab":
                    #region ===== Tab 10 =====
                    tabValue = 10;

                    string questions = string.Empty;
                    //string questions = string.Empty;
                    foreach (RepeaterItem item in rptQuestions.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            string QuestionNo = ((Label)item.FindControl("lblQuestionNo")).Text;
                            string Yes = ((CheckBox)item.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                            string No = ((CheckBox)item.FindControl("chkMonthNo")).Checked == true ? "1" : "0";
                            string Comment = ((TextBox)item.FindControl("txtMonthComment")).Text;
                            questions += QuestionNo + "$" + Yes + "$" + No + "$" + Comment + "~";
                        }
                    }
                    questions = questions.Remove(questions.Length - 0, 0);
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@score_Communication_2", score_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@Inter_Communication_2", Inter_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@GROSS_2", GROSS_2.Text);
                    cmd.Parameters.AddWithValue("@inter_Gross_2 ", inter_Gross_2.Text);
                    cmd.Parameters.AddWithValue("@FINE_2", FINE_2.Text);
                    cmd.Parameters.AddWithValue("@inter_FINE_2", inter_FINE_2.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_2", PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PROBLEM_2", inter_PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_2", PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PERSONAL_2", inter_PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@Comm_3", Comm_3.Text);
                    cmd.Parameters.AddWithValue("@inter_3", inter_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_3", GROSS_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_3", GROSS_inter_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_3", FINE_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_3", FINE_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_3", PROBLEM_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_3", PROBLEM_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_3", PERSONAL_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_3", PERSONAL_inter_3.Text);
                    cmd.Parameters.AddWithValue("@Communication_6", Communication_6.Text);
                    cmd.Parameters.AddWithValue("@comm_inter_6", comm_inter_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_6", GROSS_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_6", GROSS_inter_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_6", FINE_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_6", FINE_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_6", PROBLEM_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_6", PROBLEM_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_6", PERSONAL_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_6", PERSONAL_inter_6.Text);
                    cmd.Parameters.AddWithValue("@comm_7", comm_7.Text);
                    cmd.Parameters.AddWithValue("@inter_7", inter_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_7", GROSS_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_7", GROSS_inter_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_7", FINE_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_7", FINE_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_7", PROBLEM_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_7", PROBLEM_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_7", PERSONAL_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_7", PERSONAL_inter_7.Text);
                    cmd.Parameters.AddWithValue("@comm_9", comm_9.Text);
                    cmd.Parameters.AddWithValue("@inter_9", inter_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_9", GROSS_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_9", GROSS_inter_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_9", FINE_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_9", FINE_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_9", PROBLEM_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_9", PROBLEM_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_9", PERSONAL_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_9", PERSONAL_inter_9.Text);
                    cmd.Parameters.AddWithValue("@comm_10", comm_10.Text);
                    cmd.Parameters.AddWithValue("@inter_10", inter_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_10 ", GROSS_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_10", GROSS_inter_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_10", FINE_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_10", FINE_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_10", PROBLEM_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_10", PROBLEM_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_10", PERSONAL_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_10", PERSONAL_inter_10.Text);
                    cmd.Parameters.AddWithValue("@comm_11", comm_11.Text);
                    cmd.Parameters.AddWithValue("@inter_11", inter_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_11", GROSS_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_11", GROSS_inter_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_11", FINE_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_11", FINE_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_11", PROBLEM_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_11", PROBLEM_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_11", PERSONAL_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_11", PERSONAL_inter_11.Text);
                    cmd.Parameters.AddWithValue("@comm_13", comm_13.Text);
                    cmd.Parameters.AddWithValue("@inter_13", inter_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_13", GROSS_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_13", GROSS_inter_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_13", FINE_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_13", FINE_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_13", PROBLEM_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_13", PROBLEM_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_13", PERSONAL_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_13", PERSONAL_inter_13.Text);
                    cmd.Parameters.AddWithValue("@comm_15", comm_15.Text);
                    cmd.Parameters.AddWithValue("@inter_15", inter_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_15", GROSS_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_15", GROSS_inter_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_15", FINE_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_15", FINE_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_15", PROBLEM_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_15", PROBLEM_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_15", PERSONAL_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_15", PERSONAL_inter_15.Text);
                    cmd.Parameters.AddWithValue("@comm_17", comm_17.Text);
                    cmd.Parameters.AddWithValue("@inter_17", inter_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_17", GROSS_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_17", GROSS_inter_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_17", FINE_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_17", FINE_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_17", PROBLEM_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_17", PROBLEM_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_17", PERSONAL_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_17", PERSONAL_inter_17.Text);
                    cmd.Parameters.AddWithValue("@comm_19", comm_19.Text);
                    cmd.Parameters.AddWithValue("@inter_19", inter_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_19", GROSS_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_19", GROSS_inter_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_19", FINE_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_19", FINE_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_19", PROBLEM_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_19", PROBLEM_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_19", PERSONAL_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_19", PERSONAL_inter_19.Text);
                    cmd.Parameters.AddWithValue("@comm_21", comm_21.Text);
                    cmd.Parameters.AddWithValue("@inter_21", inter_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_21", GROSS_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_21", GROSS_inter_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_21", FINE_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_21", FINE_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_21", PROBLEM_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_21", PROBLEM_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_21", PERSONAL_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_21", PERSONAL_inter_21.Text);
                    cmd.Parameters.AddWithValue("@comm_23", comm_23.Text);
                    cmd.Parameters.AddWithValue("@inter_23", inter_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_23", GROSS_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_23", GROSS_inter_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_23", FINE_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_23", FINE_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_23", PROBLEM_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_23", PROBLEM_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_23", PERSONAL_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_23", PERSONAL_inter_23.Text);
                    cmd.Parameters.AddWithValue("@comm_25", comm_25.Text);
                    cmd.Parameters.AddWithValue("@inter_25", inter_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_25", GROSS_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_25", GROSS_inter_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_25", FINE_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_25", FINE_inter_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_25", PROBLEM_25.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_25", PERSONAL_25.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_25", PERSONAL_inter_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_25", PROBLEM_inter_25.Text);
                    cmd.Parameters.AddWithValue("@comm_28", comm_28.Text);
                    cmd.Parameters.AddWithValue("@inter_28", inter_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_28", GROSS_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_28", GROSS_inter_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_28", FINE_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_28", FINE_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_28", PROBLEM_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_28", PROBLEM_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_28", PERSONAL_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_28", PERSONAL_inter_28.Text);
                    cmd.Parameters.AddWithValue("@comm_31", comm_31.Text);
                    cmd.Parameters.AddWithValue("@inter_31", inter_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_31", GROSS_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_31", GROSS_inter_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_31", FINE_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_31", FINE_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_31", PROBLEM_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_31", PROBLEM_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_31", PERSONAL_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_31", PERSONAL_inter_31.Text);
                    cmd.Parameters.AddWithValue("@comm_34", comm_34.Text);
                    cmd.Parameters.AddWithValue("@inter_34", inter_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_34", GROSS_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_34", GROSS_inter_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_34", FINE_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_34", FINE_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_34", PROBLEM_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_34", PROBLEM_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_34", PERSONAL_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_34", PERSONAL_inter_34.Text);
                    cmd.Parameters.AddWithValue("@comm_42", comm_42.Text);
                    cmd.Parameters.AddWithValue("@inter_42", inter_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_42", GROSS_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_42", GROSS_inter_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_42", FINE_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_42", FINE_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_42", PROBLEM_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_42", PROBLEM_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_42", PERSONAL_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_42", PERSONAL_inter_42.Text);
                    cmd.Parameters.AddWithValue("@comm_45", comm_45.Text);
                    cmd.Parameters.AddWithValue("@inter_45", inter_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_45", GROSS_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_45", GROSS_inter_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_45   ", FINE_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_45   ", FINE_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_45", PROBLEM_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_45", PROBLEM_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_45", PERSONAL_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_45", PERSONAL_inter_45.Text);
                    cmd.Parameters.AddWithValue("@comm_51", comm_51.Text);
                    cmd.Parameters.AddWithValue("@inter_51", inter_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_51", GROSS_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_51", GROSS_inter_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_51", FINE_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_51", FINE_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_51", PROBLEM_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_51", PROBLEM_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_51", PERSONAL_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_51", PERSONAL_inter_51.Text);
                    cmd.Parameters.AddWithValue("@comm_60", comm_60.Text);
                    cmd.Parameters.AddWithValue("@inter_60", inter_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_60", GROSS_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_60", GROSS_inter_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_60", FINE_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_60", FINE_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_60", PROBLEM_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_60", PROBLEM_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_60", PERSONAL_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_60", PERSONAL_inter_60.Text);

                    cmd.Parameters.AddWithValue("@MONTHS", SelectMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@questions", questions);



                    //cmd.Parameters.AddWithValue("lblQuestionNo", lblQuestionNo.Text);
                    //cmd.Parameters.AddWithValue("chkMonthYes", chkMonthYes);
                    //cmd.Parameters.AddWithValue("chkMonthNo", chkMonthNo);
                    //cmd.Parameters.AddWithValue("txtMonthComment", txtMonthComment.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab":
                    #region ===== Tab 11 =====
                    tabValue = 11;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    string ABILITY_questions = string.Empty;
                    foreach (RepeaterItem item in abilityQuestionsParent.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Repeater rpt = ((Repeater)item.FindControl("abilityQuestionsChild"));

                            if (rpt != null)
                            {
                                foreach (RepeaterItem rptitem in rpt.Items)
                                {
                                    string categoryId = ((Label)rptitem.FindControl("lblCategoryId")).Text;
                                    string questionNO = ((Label)rptitem.FindControl("abilityquestionNO")).Text;
                                    string Yes = ((CheckBox)rptitem.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                                    string No = ((CheckBox)rptitem.FindControl("chkMonthNo")).Checked == true ? "1" : "0";

                                    ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
                                }
                            }
                        }
                    }
                    if (ABILITY_questions.Length > 0)
                    {
                        ABILITY_questions = ABILITY_questions.Remove(ABILITY_questions.Length - 1, 1);
                        string abilityQuestionsChild = string.Empty;
                    }
                    cmd.Parameters.AddWithValue("@ABILITY_months", MonthSelect.Text);
                    cmd.Parameters.AddWithValue("@ability_TOTAL", ability_TOTAL.Text);
                    cmd.Parameters.AddWithValue("@ability_COMMENTS", ability_COMMENTS.Text);
                    cmd.Parameters.AddWithValue("@ABILITY_questions", ABILITY_questions);
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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab":
                    #region ===== Tab 12 =====

                    tabValue = 12;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    cmd.Parameters.AddWithValue("@GMFCS_I", GMFCSCheckBoxI.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_II", GMFCSCheckBoxII.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_III", GMFCSCheckBoxIII.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_IV", GMFCSCheckBoxIV.Checked);
                    cmd.Parameters.AddWithValue("@GMFCS_V", GMFCSCheckBoxV.Checked);



                    cmd.Parameters.AddWithValue("@Gmfm_LyingRolling", txtGmfm_LyingRolling.Text);
                    //cmd.Parameters.AddWithValue("@chkI_LyingRolling", chkI_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_LyingRolling", chkII_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_LyingRolling", chkIII_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_LyingRolling", chkIV_LyingRolling.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_LyingRolling", chkV_LyingRolling.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_Sitting", txtGmfm_Sitting.Text);
                    //cmd.Parameters.AddWithValue("@chkI_Sitting", chkI_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_Sitting", chkII_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_Sitting", chkIII_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_Sitting", chkIV_Sitting.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_Sitting", chkV_Sitting.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_KneelingCrawling", txtGmfm_KneelingCrawling.Text);
                    //cmd.Parameters.AddWithValue("@chkI_KneelingCrawling", chkI_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_KneelingCrawling", chkII_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_KneelingCrawling", chkIII_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_KneelingCrawling", chkIV_KneelingCrawling.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_KneelingCrawling", chkV_KneelingCrawling.Checked);


                    cmd.Parameters.AddWithValue("@Gmfm_Standing", txtGmfm_Standing.Text);
                    //cmd.Parameters.AddWithValue("@chkI_Standing", chkI_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_Standing", chkII_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_Standing", chkIII_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_Standing", chkIV_Standing.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_Standing", chkV_Standing.Checked);

                    cmd.Parameters.AddWithValue("@Gmfm_RunningJumping", txtGmfm_RunningJumping.Text);
                    cmd.Parameters.AddWithValue("@txtGmfm_TotalScore", txtGmfm_TotalScore.Text);
                    //cmd.Parameters.AddWithValue("@chkI_RunningJumping", chkI_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkII_RunningJumping", chkII_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkIII_RunningJumping", chkIII_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkIV_RunningJumping", chkIV_RunningJumping.Checked);
                    //cmd.Parameters.AddWithValue("@chkV_RunningJumping", chkV_RunningJumping.Checked);



                    cmd.Parameters.AddWithValue("@MACs_I", chkMACs_I.Checked);
                    cmd.Parameters.AddWithValue("@MACs_II", chkMACs_II.Checked);
                    cmd.Parameters.AddWithValue("@MACs_III", chkMACs_III.Checked);
                    cmd.Parameters.AddWithValue("@MACs_IV", chkMACs_IV.Checked);
                    cmd.Parameters.AddWithValue("@MACs_V", chkMACs_V.Checked);


                    cmd.Parameters.AddWithValue("@FMS_I", chkFMS_I.Checked);
                    cmd.Parameters.AddWithValue("@FMS_II", chkFMS_II.Checked);
                    cmd.Parameters.AddWithValue("@FMS_III", chkFMS_III.Checked);
                    cmd.Parameters.AddWithValue("@FMS_IV", chkFMS_IV.Checked);
                    cmd.Parameters.AddWithValue("@FMS_V", chkFMS_V.Checked);


                    cmd.Parameters.AddWithValue("@Barry_I", chkBarry_I.Checked);
                    cmd.Parameters.AddWithValue("@Barry_II", chkBarry_II.Checked);
                    cmd.Parameters.AddWithValue("@Barry_III", chkBarry_III.Checked);
                    cmd.Parameters.AddWithValue("@Barry_IV", chkBarry_IV.Checked);
                    cmd.Parameters.AddWithValue("@Barry_V", chkBarry_V.Checked);
                    cmd.Parameters.AddWithValue("@Barry_VI", chkBarry_VI.Checked);
                    cmd.Parameters.AddWithValue("@Barry_albright_txt", Barry_albright_txt.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab":
                    #region ===== Tab 13 =====
                    tabValue = 13;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@TestMeassures_GrossMotor", TestMeassures_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_FineMotor", TestMeassures_FineMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverLanguage", TestMeassures_DenverLanguage.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverPersonal", TestMeassures_DenverPersonal.Text);
                    cmd.Parameters.AddWithValue("@Tests_cmt", Tests_cmt.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab":
                    #region ===== Tab 14 =====
                    tabValue = 14;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@General_Processing", General_Processing.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY_Processing", AUDITORY_Processing.Text);
                    cmd.Parameters.AddWithValue("@VISUAL_Processing", VISUAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@TOUCH_Processing", TOUCH_Processing.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT_Processing", MOVEMENT_Processing.Text);
                    cmd.Parameters.AddWithValue("@ORAL_Processing", ORAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@Raw_score", Raw_score.Text);
                    cmd.Parameters.AddWithValue("@Total_rawscore", Total_rawscore.Text);
                    cmd.Parameters.AddWithValue("@Interpretation", Interpretation.Text);
                    cmd.Parameters.AddWithValue("@Comments_1", Comments_1.Text);
                    cmd.Parameters.AddWithValue("@Score_seeking", Score_seeking.Text);
                    cmd.Parameters.AddWithValue("@SEEKING", SEEKING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Avoiding", Score_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@AVOIDING", AVOIDING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_sensitivity", Score_sensitivity.Text);
                    cmd.Parameters.AddWithValue("@SENSITIVITY_2", SENSITIVITY_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Registration", Score_Registration.Text);
                    cmd.Parameters.AddWithValue("@REGISTRATION", REGISTRATION.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_general", Score_general.Text);
                    cmd.Parameters.AddWithValue("@GENERAL", GENERAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Auditory", Score_Auditory.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY", AUDITORY.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_visual", Score_visual.Text);
                    cmd.Parameters.AddWithValue("@VISUAL", VISUAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_touch", Score_touch.Text);
                    cmd.Parameters.AddWithValue("@TOUCH", TOUCH.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_movement", Score_movement.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT", MOVEMENT.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_oral", Score_oral.Text);
                    cmd.Parameters.AddWithValue("@ORAL", ORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_behavioural", Score_behavioural.Text);
                    cmd.Parameters.AddWithValue("@BEHAVIORAL", BEHAVIORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_2", Comments_2.Text);
                    cmd.Parameters.AddWithValue("@SPchild_Seeker", SPchild_Seeker.Text);
                    cmd.Parameters.AddWithValue("@Seeking_Seeker", Seeking_Seeker.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Avoider", SPchild_Avoider.Text);
                    cmd.Parameters.AddWithValue("@Avoiding_Avoider", Avoiding_Avoider.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Sensor", SPchild_Sensor.Text);
                    cmd.Parameters.AddWithValue("@Sensitivity_Sensor", Sensitivity_Sensor.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Bystander", SPchild_Bystander.Text);
                    cmd.Parameters.AddWithValue("@Registration_Bystander", Registration_Bystander.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Auditory_3", SPchild_Auditory_3.Text);
                    cmd.Parameters.AddWithValue("@Auditory_3", Auditory_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Visual_3", SPchild_Visual_3.Text);
                    cmd.Parameters.AddWithValue("@Visual_3", Visual_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Touch_3", SPchild_Touch_3.Text);
                    cmd.Parameters.AddWithValue("@Touch_3", Touch_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Movement_3", SPchild_Movement_3.Text);
                    cmd.Parameters.AddWithValue("@Movement_3", Movement_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Body_position", SPchild_Body_position.Text);
                    cmd.Parameters.AddWithValue("@Body_position", Body_position.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Oral_3", SPchild_Oral_3.Text);
                    cmd.Parameters.AddWithValue("@Oral_3", Oral_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Conduct_3", SPchild_Conduct_3.Text);
                    cmd.Parameters.AddWithValue("@Conduct_3", Conduct_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Social_emotional", SPchild_Social_emotional.Text);
                    cmd.Parameters.AddWithValue("@Social_emotional", Social_emotional.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Attentional_3", SPchild_Attentional_3.Text);
                    cmd.Parameters.AddWithValue("@Attentional_3", Attentional_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_3", Comments_3.Text);
                    cmd.Parameters.AddWithValue("@SPAdult_Low_Registration", SPAdult_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration", Low_Registration.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_seeking", SPAdult_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking", Sensory_seeking.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Sensitivity", SPAdult_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity", Sensory_Sensitivity.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Avoiding", SPAdult_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding", Sensory_Avoiding.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_4", Comments_4.Text);
                    cmd.Parameters.AddWithValue("@SP_Low_Registration64", SP_Low_Registration64.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_5", Low_Registration_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_seeking_64", SP_Sensory_seeking_64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_5", Sensory_seeking_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Sensitivity64", SP_Sensory_Sensitivity64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_5", Sensory_Sensitivity_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Avoiding64", SP_Sensory_Avoiding64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_5", Sensory_Avoiding_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_5", Comments_5.Text);
                    cmd.Parameters.AddWithValue("@Older_Low_Registration", Older_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_6", Low_Registration_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_seeking", Older_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_6", Sensory_seeking_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Sensitivity", Older_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_6", Sensory_Sensitivity_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Avoiding", Older_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_6", Sensory_Avoiding_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_6", Comments_6.Text);

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
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab":
                    #region ===== Tab 15 =====

                    tabValue = 15;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Summary", Evaluation_Goal_Summary.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_System_Impairment", Evaluation_System_Impairment.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_LTG", Evaluation_LTG.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_STG", Evaluation_STG.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan_advice", Evalution_Plan_advice.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan__Frequency", Evalution_Plan__Frequency.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan_Adjuncts", Evalution_Plan_Adjuncts.Text);
                    cmd.Parameters.AddWithValue("@Evalution_Plan__Education", Evalution_Plan__Education.Text);

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
                        this.tb_Contents.ActiveTabIndex = tabValue - 8;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab";
                    }
                    #endregion
                    break;
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab":
                    #region ===== Tab 16 =====

                    tabValue = 16;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Doctor_Physioptherapist", @Doctor_Physioptherapist.Text);
                    cmd.Parameters.AddWithValue("@Doctor_Occupational", @Doctor_Occupational.Text);
                    //cmd.Parameters.AddWithValue("@Doctor_EnterReport", @Doctor_EnterReport.Text);
                    //   cmd.Parameters.AddWithValue("@Doctor_EnterReport", DBNull.Value);

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
                        this.tb_Contents.ActiveTabIndex = tabValue - 8;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab";
                    }
                    #endregion
                    break;

            }
        }
        protected void MonthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string categoryId = string.Empty;
                string questionNo = string.Empty;
                string yes = string.Empty;
                string No = string.Empty;
                //SqlConnection conn1 = new SqlConnection("Data Source=184.154.187.166;Initial Catalog=demo2;User ID=demo2;Password=Sneh#123db");
                string que1 = "SELECT DISTINCT CATEGORYID, CATEGORY_NAME FROM ABILITY_CHECKLIST_New WHERE MonthsQ = " + MonthSelect.SelectedValue + "; SELECT questionNO, Question, CategoryID, category_name,0 as Yes ,0 as No FROM ABILITY_CHECKLIST_New  WHERE MonthsQ = " + MonthSelect.SelectedValue + "";
                SqlCommand cmd1 = new SqlCommand(que1, conn);
                conn.Open();
                DataSet dss1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dss1);
                conn.Close();

                dss1.Relations.Add(new DataRelation("CategoriesRelation", dss1.Tables[0].Columns["CATEGORYID"], dss1.Tables[1].Columns["CATEGORYID"]));

                abilityQuestionsParent.DataSource = dss1.Tables[0];
                abilityQuestionsParent.DataBind();
                //Session["Ability"] = null;

                DataTable dt = Session["Ability"] as DataTable;
                //if (dt != null)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        string[] Ques1 = dt.Rows[i]["ABILITY_questions"].ToString().Split('~');

                //        for (int j = 0; j < Ques1.Length; j++)
                //        {
                //            categoryId = Ques1[j].Split('#')[0].ToString();
                //            questionNo = Ques1[j].Split('#')[1].ToString().Split('$')[0].ToString();
                //            yes = Ques1[j].Split('#')[1].ToString().Split('$')[1].ToString();
                //            No = Ques1[j].Split('#')[1].ToString().Split('$')[2].ToString();

                //            DataRow dr = (dss1.Tables[1].AsEnumerable().Where(w => w.Field<int>("CategoryID").ToString() == categoryId && w.Field<int>("questionNO").ToString() == questionNo)).FirstOrDefault();

                //            if (yes == "1")
                //            {
                //                if (dr != null)
                //                {
                //                    dr["Yes"] = 1;
                //                    dr["No"] = 0;
                //                }
                //            }
                //            else if (No == "1")
                //            {
                //                if (dr != null)
                //                {
                //                    dr["No"] = 1;
                //                    dr["Yes"] = 0;
                //                }
                //            }
                //        }
                //    }
                //    abilityQuestionsParent.DataSource = dss1.Tables[0];
                //    abilityQuestionsParent.DataBind();
                //    //Session["Ability"] = null;
                //}
                if (dt != null)
                {
                    string selectedMonth = MonthSelect.Text; // Get the selected month

                    // Loop through each row in the DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        // Check if the month from the Ability_months column matches the selected month
                        if (row["ABILITY_months"].ToString() == selectedMonth)
                        {
                            // Split the question answers
                            string[] Ques1 = row["ABILITY_questions"].ToString().Split('~');

                            for (int j = 0; j < Ques1.Length; j++)
                            {
                                categoryId = Ques1[j].Split('#')[0];
                                questionNo = Ques1[j].Split('#')[1].Split('$')[0];
                                yes = Ques1[j].Split('#')[1].Split('$')[1];
                                string no = Ques1[j].Split('#')[1].Split('$')[2];

                                // Find the corresponding DataRow in dss1.Tables[1]
                                DataRow dr = dss1.Tables[1]
                                    .AsEnumerable()
                                    .FirstOrDefault(w =>
                                        w.Field<int>("CategoryID").ToString() == categoryId &&
                                        w.Field<int>("questionNO").ToString() == questionNo);

                                if (yes == "1")
                                {
                                    if (dr != null)
                                    {
                                        dr["Yes"] = 1;
                                        dr["No"] = 0;
                                    }
                                }
                                else if (no == "1")
                                {
                                    if (dr != null)
                                    {
                                        dr["No"] = 1;
                                        dr["Yes"] = 0;
                                    }
                                }
                            }
                        }
                    }

                    // Bind the data to the Repeater (after processing the answers)
                    abilityQuestionsParent.DataSource = dss1.Tables[0];
                    abilityQuestionsParent.DataBind();
                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void abilityQuestionsParent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Repeater abilityQuestionsChild = e.Item.FindControl("abilityQuestionsChild") as Repeater;
                abilityQuestionsChild.DataSource = drv.CreateChildView("CategoriesRelation");
                abilityQuestionsChild.DataBind();
            }



            //DataTable dt = Session["Ability"] as DataTable;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ 

            //}
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    var chkMonthYes = (CheckBox)e.Item.FindControl("chkMonthYes");
            //    var chkMonthNo = (CheckBox)e.Item.FindControl("chkMonthNo");
            //    checkBox.Checked = true;
            //}
        }
        protected void SelectMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //SqlConnection conn = new SqlConnection("Data Source=184.154.187.166;Initial Catalog=demo2;User ID=demo2;Password=Sneh#123db");
                string que = "SELECT QUESTIONS, QuestionNo, 0 as Yes, 0 as No, '' as Comments FROM QUESTIONNAIRE_SI WHERE MONTHS = " + SelectMonth.SelectedValue + "";
                SqlCommand cmd = new SqlCommand(que, conn);
                conn.Open();
                DataSet dss = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dss);
                conn.Close();

                DataTable dt = Session["AgeState"] as DataTable;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string _month = SelectMonth.SelectedValue;
                        string[] Ques = dt.Rows[i]["QUESTIONS"].ToString().Split('~');
                        if (dt.Rows[i]["MONTHS"].ToString() == _month)
                        {
                            for (int j = 0; j < Ques.Length; j++)
                            {
                                DataRow dr = (dss.Tables[0].AsEnumerable().Where(w => w.Field<int>("QuestionNo").ToString() == Ques[j].Split('$')[0].ToString())).FirstOrDefault();
                                if (dr != null)
                                {

                                    if (Ques[j].Split('$')[1].ToString() == "1")
                                    {
                                        dr["Yes"] = 1;
                                        dr["No"] = 0;
                                    }
                                    else if (Ques[j].Split('$')[2].ToString() == "1")

                                    {
                                        dr["No"] = 1;
                                        dr["Yes"] = 0;
                                    }

                                    dr["Comments"] = Ques[j].Split('$')[3].ToString();

                                }
                            }
                        }
                    }
                }

                rptQuestions.DataSource = dss.Tables[0];
                rptQuestions.DataBind();
                //Session["AgeState"] = null;
            }
            catch (Exception ex)
            {
            }

        }
        private bool GetBool(object obj)
        {
            return obj != DBNull.Value && Convert.ToBoolean(obj);
        }
        private string GetString(object obj)
        {
            return obj != DBNull.Value ? obj.ToString() : string.Empty;
        }
        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //datatable dt = session["agestate"] as datatable;
            //for (int i = 0; i < dt.rows.count; i++)
            //{ 

            //}
            //if (e.item.itemtype == listitemtype.item || e.item.itemtype == listitemtype.alternatingitem)
            //{
            //    var chkmonthyes = (checkbox)e.item.findcontrol("chkmonthyes");
            //    var chkmonthno = (checkbox)e.item.findcontrol("chkmonthno");
            //    checkbox.checked = true;
            //}
        }
    }
}