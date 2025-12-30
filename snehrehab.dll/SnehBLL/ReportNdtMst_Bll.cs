using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class ReportNdtMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportNdtMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public bool IsValid(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 2;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return (i > 0);
        }
        public DataTable Search_new(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public DataSet Get_NDT(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportNdtMst_new_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            return db.DbFetch(cmd);
        }
        public bool IsValidNew(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 6;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return (i > 0);
        }
        public bool IsValidReval(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 6;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return (i > 0);
        }
        public DataTable ReportRevalPopUp_Search(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportRevalPopUp_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public DataTable DemoSearchReval(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Demo_ReportRevalMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public DataTable SearchDemo(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Demo_ReportNdtMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public DataTable SearchLastReval_New(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportRevallast_Search_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public int SaveNarrat_RevalPopUp(SnehDLL.ReportNdtMst_Dll D)
        {
            SqlCommand cmd = new SqlCommand("RevalPopupNarration"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RevalPopUpID", SqlDbType.Int).Value = D.RevalPopUpID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int, 250).Value = D.UserID;
            cmd.Parameters.Add("@yNarration", SqlDbType.VarChar, 250).Value = D.yNarration;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);
            db.DbUpdate(cmd);
            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
        public DataTable SearchLastReval(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportRevallast_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public DataTable Search(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportNdtMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }

        public DataSet Get(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportNdtMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }

        public int Set(int _appointmentID, string DataCollection_Referral, string DataCollection_Investigation, string DataCollection_MedicalHistory, string DataCollection_DailyRoutine,
             string DataCollection_Expectaion, string DataCollection_TherapyHistory, string DataCollection_Sources,
             string DataCollection_AdaptedEquipment, string Morphology_Height, string Morphology_Weight, string Morphology_LimbLength,
             string Morphology_LimbLeft, string Morphology_LimbRight, string Morphology_ArmLength, string Morphology_ArmLeft, string Morphology_ArmRight,
             string Morphology_Head, string Morphology_Nipple, string Morphology_Waist,
             string Morphology_GirthUpperLimb_Above_ElbowLevel1, string Morphology_GirthUpperLimb_Above_ElbowLevel2, string Morphology_GirthUpperLimb_Above_ElbowLevel3,
             string Morphology_GirthUpperLimb_Above_ElbowLeft1, string Morphology_GirthUpperLimb_Above_ElbowLeft2, string Morphology_GirthUpperLimb_Above_ElbowLeft3,
             string Morphology_GirthUpperLimb_Above_ElbowRight1, string Morphology_GirthUpperLimb_Above_ElbowRight2, string Morphology_GirthUpperLimb_Above_ElbowRight3,
             string Morphology_GirthUpperLimb_At_ElbowLevel, string Morphology_GirthUpperLimb_At_ElbowLeft, string Morphology_GirthUpperLimb_At_ElbowRight,
             string Morphology_GirthUpperLimb_Below_ElbowLevel1, string Morphology_GirthUpperLimb_Below_ElbowLevel2, string Morphology_GirthUpperLimb_Below_ElbowLevel3,
             string Morphology_GirthUpperLimb_Below_ElbowLeft1, string Morphology_GirthUpperLimb_Below_ElbowLeft2, string Morphology_GirthUpperLimb_Below_ElbowLeft3,
             string Morphology_GirthUpperLimb_Below_ElbowRight1, string Morphology_GirthUpperLimb_Below_ElbowRight2, string Morphology_GirthUpperLimb_Below_ElbowRight3,
             string Morphology_GirthLowerLimb_Above_KneeLevel1, string Morphology_GirthLowerLimb_Above_KneeLevel2, string Morphology_GirthLowerLimb_Above_KneeLevel3,
             string Morphology_GirthLowerLimb_Above_KneeLeft1, string Morphology_GirthLowerLimb_Above_KneeLeft2, string Morphology_GirthLowerLimb_Above_KneeLeft3,
             string Morphology_GirthLowerLimb_Above_KneeRight1, string Morphology_GirthLowerLimb_Above_KneeRight2, string Morphology_GirthLowerLimb_Above_KneeRight3,
             string Morphology_GirthLowerLimb_At_KneeLevel, string Morphology_GirthLowerLimb_At_KneeLeft, string Morphology_GirthLowerLimb_At_KneeRight,
             string Morphology_GirthLowerLimb_Below_KneeLevel1, string Morphology_GirthLowerLimb_Below_KneeLevel2, string Morphology_GirthLowerLimb_Below_KneeLevel3,
             string Morphology_GirthLowerLimb_Below_KneeLeft1, string Morphology_GirthLowerLimb_Below_KneeLeft2, string Morphology_GirthLowerLimb_Below_KneeLeft3,
             string Morphology_GirthLowerLimb_Below_KneeRight1, string Morphology_GirthLowerLimb_Below_KneeRight2, string Morphology_GirthLowerLimb_Below_KneeRight3,

             string Morphology_OralMotorFactors,
             string FunctionalActivities_ADL, string FunctionalActivities_OralMotor,
             string TestMeasures_GMFCS, string TestMeasures_GMFM, string TestMeasures_GMPM, string TestMeasures_AshworthScale,
             string TestMeasures_TradieusScale, string TestMeasures_OGS, string TestMeasures_Melbourne, string TestMeasures_COPM, string TestMeasures_ClinicalObservation,
             string TestMeasures_Others, string Movement_Inertia, string Movement_Strategies, string Movement_Extremities, string Movement_Stability, string Movement_Overuse,
             string Others_Integration, string Others_Assessments, string Musculoskeletal_Rom1_HipFlexionLeft,
             string Musculoskeletal_Rom1_HipFlexionRight, string Musculoskeletal_Rom1_HipExtensionLeft,
             string Musculoskeletal_Rom1_HipAbductionLeft, string Musculoskeletal_Rom1_HipAbductionRight,
             string Musculoskeletal_Rom1_HipExtensionRight,
             string Musculoskeletal_Rom1_HipExternalLeft, string Musculoskeletal_Rom1_HipExternalRight, string Musculoskeletal_Rom1_HipInternalLeft,
             string Musculoskeletal_Rom1_HipInternalRight, string Musculoskeletal_Rom1_PoplitealLeft, string Musculoskeletal_Rom1_PoplitealRight,
             string Musculoskeletal_Rom1_KneeFlexionLeft, string Musculoskeletal_Rom1_KneeFlexionRight, string Musculoskeletal_Rom1_KneeExtensionLeft,
             string Musculoskeletal_Rom1_KneeExtensionRight, string Musculoskeletal_Rom1_DorsiflexionFlexionLeft, string Musculoskeletal_Rom1_DorsiflexionFlexionRight,
             string Musculoskeletal_Rom1_DorsiflexionExtensionLeft, string Musculoskeletal_Rom1_DorsiflexionExtensionRight, string Musculoskeletal_Rom1_PlantarFlexionLeft,
             string Musculoskeletal_Rom1_PlantarFlexionRight, string Musculoskeletal_Rom1_OthersLeft, string Musculoskeletal_Rom1_OthersRight, string Musculoskeletal_Rom2_ShoulderFlexionLeft,
             string Musculoskeletal_Rom2_ShoulderFlexionRight, string Musculoskeletal_Rom2_ShoulderExtensionLeft, string Musculoskeletal_Rom2_ShoulderExtensionRight,
             string Musculoskeletal_Rom2_HorizontalAbductionLeft, string Musculoskeletal_Rom2_HorizontalAbductionRight, string Musculoskeletal_Rom2_ExternalRotationLeft,
             string Musculoskeletal_Rom2_ExternalRotationRight, string Musculoskeletal_Rom2_InternalRotationLeft, string Musculoskeletal_Rom2_InternalRotationRight,
             string Musculoskeletal_Rom2_ElbowFlexionLeft, string Musculoskeletal_Rom2_ElbowFlexionRight, string Musculoskeletal_Rom2_ElbowExtensionLeft,
             string Musculoskeletal_Rom2_ElbowExtensionRight, string Musculoskeletal_Rom2_SupinationLeft, string Musculoskeletal_Rom2_SupinationRight,
             string Musculoskeletal_Rom2_PronationLeft, string Musculoskeletal_Rom2_PronationRight, string Musculoskeletal_Rom2_WristFlexionLeft,
             string Musculoskeletal_Rom2_WristFlexionRight, string Musculoskeletal_Rom2_WristExtesionLeft, string Musculoskeletal_Rom2_WristExtesionRight,
             string Musculoskeletal_Rom2_OthersLeft, string Musculoskeletal_Rom2_OthersRight, string Musculoskeletal_Strengthlp, string Musculoskeletal_StrengthCC,
             string Musculoskeletal_StrengthMuscle, string Musculoskeletal_StrengthSkeletal, string Musculoskeletal_Mmt_HipflexorsLeft, string Musculoskeletal_Mmt_HipflexorsRight,
             string Musculoskeletal_Mmt_AbductorsLeft, string Musculoskeletal_Mmt_AbductorsRight, string Musculoskeletal_Mmt_ExtensorsLeft, string Musculoskeletal_Mmt_ExtensorsRight,
             string Musculoskeletal_Mmt_HamsLeft, string Musculoskeletal_Mmt_HamsRight, string Musculoskeletal_Mmt_QuadsLeft, string Musculoskeletal_Mmt_QuadsRight,
             string Musculoskeletal_Mmt_TibialisAnteriorLeft, string Musculoskeletal_Mmt_TibialisAnteriorRight, string Musculoskeletal_Mmt_TibialisPosteriorLeft,
             string Musculoskeletal_Mmt_TibialisPosteriorRight, string Musculoskeletal_Mmt_ExtensorDigitorumLeft, string Musculoskeletal_Mmt_ExtensorDigitorumRight,
             string Musculoskeletal_Mmt_ExtensorHallucisLeft, string Musculoskeletal_Mmt_ExtensorHallucisRight, string Musculoskeletal_Mmt_PeroneiLeft, string Musculoskeletal_Mmt_PeroneiRight,
             string Musculoskeletal_Mmt_FlexorDigitorumLeft, string Musculoskeletal_Mmt_FlexorDigitorumRight, string Musculoskeletal_Mmt_FlexorHallucisLeft,
             string Musculoskeletal_Mmt_FlexorHallucisRight, string Musculoskeletal_Mmt_AnteriorDeltoidLeft, string Musculoskeletal_Mmt_AnteriorDeltoidRight,
             string Musculoskeletal_Mmt_PosteriorDeltoidLeft, string Musculoskeletal_Mmt_PosteriorDeltoidRight, string Musculoskeletal_Mmt_MiddleDeltoidLeft,
             string Musculoskeletal_Mmt_MiddleDeltoidRight, string Musculoskeletal_Mmt_SupraspinatusLeft, string Musculoskeletal_Mmt_SupraspinatusRight,
             string Musculoskeletal_Mmt_SerratusAnteriorLeft, string Musculoskeletal_Mmt_SerratusAnteriorRight, string Musculoskeletal_Mmt_RhomboidsLeft,
             string Musculoskeletal_Mmt_RhomboidsRight, string Musculoskeletal_Mmt_BicepsLeft, string Musculoskeletal_Mmt_BicepsRight, string Musculoskeletal_Mmt_TricepsLeft,
             string Musculoskeletal_Mmt_TricepsRight, string Musculoskeletal_Mmt_SupinatorLeft, string Musculoskeletal_Mmt_SupinatorRight, string Musculoskeletal_Mmt_PronatorLeft,
             string Musculoskeletal_Mmt_PronatorRight, string Musculoskeletal_Mmt_ECULeft, string Musculoskeletal_Mmt_ECURight, string Musculoskeletal_Mmt_ECRLeft,
             string Musculoskeletal_Mmt_ECRRight, string Musculoskeletal_Mmt_ECSLeft, string Musculoskeletal_Mmt_ECSRight, string Musculoskeletal_Mmt_FCULeft, string Musculoskeletal_Mmt_FCURight,
             string Musculoskeletal_Mmt_FCRLeft, string Musculoskeletal_Mmt_FCRRight, string Musculoskeletal_Mmt_FCSLeft, string Musculoskeletal_Mmt_FCSRight,
             string Musculoskeletal_Mmt_OpponensPollicisLeft, string Musculoskeletal_Mmt_OpponensPollicisRight, string Musculoskeletal_Mmt_FlexorPollicisLeft,
             string Musculoskeletal_Mmt_FlexorPollicisRight, string Musculoskeletal_Mmt_AbductorPollicisLeft, string Musculoskeletal_Mmt_AbductorPollicisRight,
             string Musculoskeletal_Mmt_ExtensorPollicisLeft, string Musculoskeletal_Mmt_ExtensorPollicisRight, string RemarkVariable_SustainGeneral,
             string RemarkVariable_PosturalGeneral, string RemarkVariable_ContractionsGeneral, string RemarkVariable_AntagonistGeneral, string RemarkVariable_SynergyGeneral,
             string RemarkVariable_StiffinessGeneral, string RemarkVariable_ExtraneousGeneral, string RemarkVariable_SustainControl, string RemarkVariable_PosturalControl,
             string RemarkVariable_ContractionsControl, string RemarkVariable_AntagonistControl, string RemarkVariable_SynergyControl, string RemarkVariable_StiffinessControl,
             string RemarkVariable_ExtraneousControl, string RemarkVariable_SustainTiming, string RemarkVariable_PosturalTiming, string RemarkVariable_ContractionsTiming,
             string RemarkVariable_AntagonistTiming, string RemarkVariable_SynergyTiming, string RemarkVariable_StiffinessTiming, string RemarkVariable_ExtraneousTiming,
             string SensorySystem_Vision,
             string SensorySystem_Auditory, string SensorySystem_Propioceptive, string SensorySystem_Oromotpor, string SensorySystem_Vestibular, string SensorySystem_Tactile, string SensorySystem_Olfactory,
             string SIPTInfo_History,
             string SIPTInfo_HandFunction1_GraspRight, string SIPTInfo_HandFunction1_GraspLeft, string SIPTInfo_HandFunction1_SphericalRight, string SIPTInfo_HandFunction1_SphericalLeft,
             string SIPTInfo_HandFunction1_HookRight, string SIPTInfo_HandFunction1_HookLeft, string SIPTInfo_HandFunction1_JawChuckRight, string SIPTInfo_HandFunction1_JawChuckLeft,
             string SIPTInfo_HandFunction1_GripRight, string SIPTInfo_HandFunction1_GripLeft, string SIPTInfo_HandFunction1_ReleaseRight, string SIPTInfo_HandFunction1_ReleaseLeft,
             string SIPTInfo_HandFunction2_OppositionLfR, string SIPTInfo_HandFunction2_OppositionLfL, string SIPTInfo_HandFunction2_OppositionMFR,
             string SIPTInfo_HandFunction2_OppositionMFL, string SIPTInfo_HandFunction2_OppositionRFR, string SIPTInfo_HandFunction2_OppositionRFL, string SIPTInfo_HandFunction2_PinchLfR,
             string SIPTInfo_HandFunction2_PinchLfL, string SIPTInfo_HandFunction2_PinchMFR, string SIPTInfo_HandFunction2_PinchMFL, string SIPTInfo_HandFunction2_PinchRFR,
             string SIPTInfo_HandFunction2_PinchRFL, string SIPTInfo_SIPT3_Spontaneous, string SIPTInfo_SIPT3_Command, string SIPTInfo_SIPT4_Kinesthesia, string SIPTInfo_SIPT4_Finger,
             string SIPTInfo_SIPT4_Localisation, string SIPTInfo_SIPT4_DoubleTactile, string SIPTInfo_SIPT4_Tactile, string SIPTInfo_SIPT4_Graphesthesia, string SIPTInfo_SIPT4_PostRotary,
             string SIPTInfo_SIPT4_Standing, string SIPTInfo_SIPT5_Color, string SIPTInfo_SIPT5_Form, string SIPTInfo_SIPT5_Size, string SIPTInfo_SIPT5_Depth, string SIPTInfo_SIPT5_Figure,
             string SIPTInfo_SIPT5_Motor, string SIPTInfo_SIPT6_Design, string SIPTInfo_SIPT6_Constructional, string SIPTInfo_SIPT7_Scanning, string SIPTInfo_SIPT7_Memory, string SIPTInfo_SIPT8_Postural,
             string SIPTInfo_SIPT8_Oral, string SIPTInfo_SIPT8_Sequencing, string SIPTInfo_SIPT8_Commands, string SIPTInfo_SIPT9_Bilateral, string SIPTInfo_SIPT9_Contralat,
             string SIPTInfo_SIPT9_PreferredHand, string SIPTInfo_SIPT9_CrossingMidline, string SIPTInfo_SIPT10_Draw, string SIPTInfo_SIPT10_ClockFace, string SIPTInfo_SIPT10_Filtering,
             string SIPTInfo_SIPT10_MotorPlanning, string SIPTInfo_SIPT10_BodyImage, string SIPTInfo_SIPT10_BodySchema, string SIPTInfo_SIPT10_Laterality, string SIPTInfo_ActivityGiven_Remark,
             string SIPTInfo_ActivityGiven_InterestActivity, string SIPTInfo_ActivityGiven_InterestCompletion, string SIPTInfo_ActivityGiven_Learning, string SIPTInfo_ActivityGiven_Complexity,
             string SIPTInfo_ActivityGiven_ProblemSolving, string SIPTInfo_ActivityGiven_Concentration, string SIPTInfo_ActivityGiven_Retension, string SIPTInfo_ActivityGiven_SpeedPerfom,
             string SIPTInfo_ActivityGiven_Neatness, string SIPTInfo_ActivityGiven_Frustation, string SIPTInfo_ActivityGiven_Work, string SIPTInfo_ActivityGiven_Reaction,
             string SIPTInfo_ActivityGiven_SociabilityTherapist, string SIPTInfo_ActivityGiven_SociabilityStudents, string Cognition_Intelligence, string Cognition_Attention, string Cognition_Memory,
             string Cognition_Adaptibility, string Cognition_MotorPlanning, string Cognition_ExecutiveFunction, string Cognition_CognitiveFunctions, string Integumentary_SkinIntegrity,
             string Integumentary_SkinColor, string Integumentary_SkinExtensibility, string Respiratory_RateResting, string Respiratory_PostExercise, string Respiratory_Patterns, string Respiratory_BreathControl,
             string Respiratory_ChestExcursion,
             string Cardiovascular_HeartRate, string Cardiovascular_PostExercise, string Cardiovascular_BP, string Cardiovascular_Edema, string Cardiovascular_Circulation, string Cardiovascular_EEi,
             string Gastrointestinal_Bowel, string Gastrointestinal_Intake, string Evaluation_Strengths, string Evaluation_Concern_Barriers, string Evaluation_Concern_Limitations,
             string Evaluation_Concern_Posture, string Evaluation_Concern_Impairment, string Evaluation_Goal_Summary, string Evaluation_Goal_Previous, string Evaluation_Goal_LongTerm,
             string Evaluation_Goal_ShortTerm, string Evaluation_Goal_Impairment, string Evaluation_Plan_Frequency, string Evaluation_Plan_Service, string Evaluation_Plan_Strategies,
             string Evaluation_Plan_Equipment, string Evaluation_Plan_Education, int Doctor_Physioptherapist, int Doctor_Occupational, int Doctor_EnterReport, bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int _loginID,
             string FunctionalActivities_Cognition, string ParticipationAbility_GrossMotor, string ParticipationAbility_FineMotor, string ParticipationAbility_Communication, string ParticipationAbility_Cognition,
             string Contextual_Personal_Positive, string Contextual_Personal_Negative, string Contextual_Enviremental_Positive, string Contextual_Enviremental_Negative, string Posture_Alignment_Type,
             string Posture_Gen_Head, string Posture_Gen_Shoulder, string Posture_Gen_Ribcage, string Posture_Gen_Trunk, string Posture_Gen_Pelvis, string Posture_Gen_Hips, string Posture_Gen_Knees,
             string Posture_Gen_Ankle_Feet, string Posture_Stru_Neck, string Posture_Stru_Jaw, string Posture_Stru_Lips, string Posture_Stru_Teeth, string Posture_Stru_Tounge, string Posture_Stru_Palate,
             string Posture_Stru_MouthPosture, string Posture_Stru_ToungueMove, string Posture_Stru_Bite, string Posture_Stru_Swallow, string Posture_Stru_Chew, string Posture_Stru_Suck,
             string Posture_Stru_BaseSupport, string Posture_Stru_CenterOfMass, string Posture_Stru_StrategyForStability, string Posture_Stru_Anticipatory, string Posture_Stru_CounterBalance,
             string Posture_Impairment_Muscle, string Posture_Impairment_Atrophy, string Posture_Impairment_Hypertrophy, string Posture_Impairment_Callosities, string Posture_GeneralPosture,
             string Movement_TypeOf, string Movement_Plane, string Movement_Sagittal, string Movement_Coronal, string Movement_Transverse, string Movement_WeightShift, string Movement_LimbDissociation, string Movement_RangeSpeedOfMovements, string Movement_Balance_Maintain,
             string Movement_Balance_During, string Movement_Accuracy_Upper, string Movement_Accuracy_Lower, string Neuromotor_Recruitment_Initial, string Neuromotor_Recruitment_Sustainance,
             string Neuromotor_Recruitment_Termination, string Neuromotor_Recruitment_Control, string Neuromotor_Contraction_Initial, string Neuromotor_Contraction_Sustainance,
             string Neuromotor_Contraction_Termination, string Neuromotor_Contraction_Control, string Neuromotor_Coactivation_Initial, string Neuromotor_Coactivation_Sustainance,
             string Neuromotor_Coactivation_Termination, string Neuromotor_Coactivation_Control, string Neuromotor_Synergy_Initial, string Neuromotor_Synergy_Sustainance,
             string Neuromotor_Synergy_Termination, string Neuromotor_Synergy_Control, string Neuromotor_Stiffness_Initial, string Neuromotor_Stiffness_Sustainance,
             string Neuromotor_Stiffness_Termination, string Neuromotor_Stiffness_Control, string Neuromotor_Extraneous_Initial, string Neuromotor_Extraneous_Sustainance,
             string Neuromotor_Extraneous_Termination, string Neuromotor_Extraneous_Control, string OtherTest_Tardieus_TA_Right, string OtherTest_Tardieus_TA_Left,
             string OtherTest_Tardieus_Hamstring_Right, string OtherTest_Tardieus_Hamstring_Left, string OtherTest_Tardieus_Adductor_Right, string OtherTest_Tardieus_Adductor_Left,
             string OtherTest_Tardieus_Hip_Right, string OtherTest_Tardieus_Hip_Left, string OtherTest_Tardieus_Biceps_Right, string OtherTest_Tardieus_Biceps_Left, string SelectionMotorControl_Muscle,
             string SelectionMotorControl_Denvers, string SelectionMotorControl_GMFM, string SelectionMotorControl_MAS, string SelectionMotorControl_Observation, string TheFourA_Arousal,
             string TheFourA_Attention, string TheFourA_Affect, string TheFourA_Action, string TheFourA_StateRegulation, string FA_GrossMotor_Ability, string FA_GrossMotor_Limit, string FA_FineMotor_Ability, string FA_FineMotor_Limit,
             string FA_Communication_Ability, string FA_Communication_Limit, string FA_Cognition_Ability, string FA_Cognition_Limit, string ParticipationAbility_GrossMotor_Limit,
             string ParticipationAbility_FineMotor_Limit, string ParticipationAbility_Communication_Limit, string ParticipationAbility_Cognition_Limit,
             string Sensory_Profile_NameResults, string DiagnosisIDs, string DiagnosisOther, string SensoryProfile_Profile)
        {
            SqlCommand cmd = new SqlCommand("ReportNdtMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@DataCollection_Referral", SqlDbType.VarChar, -1).Value = DataCollection_Referral;
            cmd.Parameters.Add("@DataCollection_Investigation", SqlDbType.VarChar, -1).Value = DataCollection_Investigation;
            cmd.Parameters.Add("@DataCollection_MedicalHistory", SqlDbType.VarChar, -1).Value = DataCollection_MedicalHistory;
            cmd.Parameters.Add("@DataCollection_DailyRoutine", SqlDbType.VarChar, -1).Value = DataCollection_DailyRoutine;
            cmd.Parameters.Add("@DataCollection_Expectaion", SqlDbType.VarChar, -1).Value = DataCollection_Expectaion;
            cmd.Parameters.Add("@DataCollection_TherapyHistory", SqlDbType.VarChar, -1).Value = DataCollection_TherapyHistory;
            cmd.Parameters.Add("@DataCollection_Sources", SqlDbType.VarChar, -1).Value = DataCollection_Sources;
            cmd.Parameters.Add("@DataCollection_AdaptedEquipment", SqlDbType.VarChar, -1).Value = DataCollection_AdaptedEquipment;
            cmd.Parameters.Add("@Morphology_Height", SqlDbType.VarChar, -1).Value = Morphology_Height;
            cmd.Parameters.Add("@Morphology_Weight", SqlDbType.VarChar, -1).Value = Morphology_Weight;
            cmd.Parameters.Add("@Morphology_LimbLength", SqlDbType.VarChar, -1).Value = Morphology_LimbLength;
            cmd.Parameters.Add("@Morphology_LimbLeft", SqlDbType.VarChar, -1).Value = Morphology_LimbLeft;
            cmd.Parameters.Add("@Morphology_LimbRight", SqlDbType.VarChar, -1).Value = Morphology_LimbRight;
            cmd.Parameters.Add("@Morphology_ArmLength", SqlDbType.VarChar, -1).Value = Morphology_ArmLength;
            cmd.Parameters.Add("@Morphology_ArmLeft", SqlDbType.VarChar, -1).Value = Morphology_ArmLeft;
            cmd.Parameters.Add("@Morphology_ArmRight", SqlDbType.VarChar, -1).Value = Morphology_ArmRight;
            cmd.Parameters.Add("@Morphology_Head", SqlDbType.VarChar, -1).Value = Morphology_Head;
            cmd.Parameters.Add("@Morphology_Nipple", SqlDbType.VarChar, -1).Value = Morphology_Nipple;
            cmd.Parameters.Add("@Morphology_Waist", SqlDbType.VarChar, -1).Value = Morphology_Waist;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowLevel", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowLevel;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowLeft", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowLeft;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowRight", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowRight;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeLevel", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeLevel;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeLeft", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeLeft;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeRight", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeRight;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight3;

            cmd.Parameters.Add("@Morphology_OralMotorFactors", SqlDbType.VarChar, -1).Value = Morphology_OralMotorFactors;

            cmd.Parameters.Add("@FunctionalActivities_ADL", SqlDbType.VarChar, -1).Value = FunctionalActivities_ADL;
            cmd.Parameters.Add("@FunctionalActivities_OralMotor", SqlDbType.VarChar, -1).Value = FunctionalActivities_OralMotor;
            cmd.Parameters.Add("@TestMeasures_GMFCS", SqlDbType.VarChar, -1).Value = TestMeasures_GMFCS;
            cmd.Parameters.Add("@TestMeasures_GMFM", SqlDbType.VarChar, -1).Value = TestMeasures_GMFM;
            cmd.Parameters.Add("@TestMeasures_GMPM", SqlDbType.VarChar, -1).Value = TestMeasures_GMPM;
            cmd.Parameters.Add("@TestMeasures_AshworthScale", SqlDbType.VarChar, -1).Value = TestMeasures_AshworthScale;
            cmd.Parameters.Add("@TestMeasures_TradieusScale", SqlDbType.VarChar, -1).Value = TestMeasures_TradieusScale;
            cmd.Parameters.Add("@TestMeasures_OGS", SqlDbType.VarChar, -1).Value = TestMeasures_OGS;
            cmd.Parameters.Add("@TestMeasures_Melbourne", SqlDbType.VarChar, -1).Value = TestMeasures_Melbourne;
            cmd.Parameters.Add("@TestMeasures_COPM", SqlDbType.VarChar, -1).Value = TestMeasures_COPM;
            cmd.Parameters.Add("@TestMeasures_ClinicalObservation", SqlDbType.VarChar, -1).Value = TestMeasures_ClinicalObservation;
            cmd.Parameters.Add("@TestMeasures_Others", SqlDbType.VarChar, -1).Value = TestMeasures_Others;
            cmd.Parameters.Add("@Movement_Inertia", SqlDbType.VarChar, -1).Value = Movement_Inertia;
            cmd.Parameters.Add("@Movement_Strategies", SqlDbType.VarChar, -1).Value = Movement_Strategies;
            cmd.Parameters.Add("@Movement_Extremities", SqlDbType.VarChar, -1).Value = Movement_Extremities;
            cmd.Parameters.Add("@Movement_Stability", SqlDbType.VarChar, -1).Value = Movement_Stability;
            cmd.Parameters.Add("@Movement_Overuse", SqlDbType.VarChar, -1).Value = Movement_Overuse;
            cmd.Parameters.Add("@Others_Integration", SqlDbType.VarChar, -1).Value = Others_Integration;
            cmd.Parameters.Add("@Others_Assessments", SqlDbType.VarChar, -1).Value = Others_Assessments;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExtensionLeft;

            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipAbductionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipAbductionRight;

            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExternalLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExternalRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipInternalLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipInternalRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PoplitealLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PoplitealRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PlantarFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PlantarFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_OthersLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_OthersRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_HorizontalAbductionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_HorizontalAbductionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ExternalRotationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ExternalRotationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_InternalRotationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_InternalRotationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_SupinationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_SupinationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_PronationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_PronationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristExtesionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristExtesionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_OthersLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_OthersRight;
            cmd.Parameters.Add("@Musculoskeletal_Strengthlp", SqlDbType.VarChar, -1).Value = Musculoskeletal_Strengthlp;
            cmd.Parameters.Add("@Musculoskeletal_StrengthCC", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthCC;
            cmd.Parameters.Add("@Musculoskeletal_StrengthMuscle", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthMuscle;
            cmd.Parameters.Add("@Musculoskeletal_StrengthSkeletal", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthSkeletal;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HipflexorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HipflexorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HamsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HamsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_QuadsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_QuadsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisAnteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisAnteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisPosteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisPosteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorDigitorumLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorDigitorumRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorHallucisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorHallucisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PeroneiLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PeroneiRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorDigitorumLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorDigitorumRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorHallucisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorHallucisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AnteriorDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AnteriorDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PosteriorDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PosteriorDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_MiddleDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_MiddleDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupraspinatusLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupraspinatusRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SerratusAnteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SerratusAnteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_RhomboidsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_RhomboidsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_BicepsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_BicepsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TricepsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TricepsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupinatorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupinatorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PronatorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PronatorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECULeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECULeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECURight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECURight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECRLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECRRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECSLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECSRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCULeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCULeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCURight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCURight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCRLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCRRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCSLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCSRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_OpponensPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_OpponensPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorPollicisRight;

            cmd.Parameters.Add("@RemarkVariable_SustainGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainGeneral;
            cmd.Parameters.Add("@RemarkVariable_PosturalGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalGeneral;
            cmd.Parameters.Add("@RemarkVariable_ContractionsGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsGeneral;
            cmd.Parameters.Add("@RemarkVariable_AntagonistGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistGeneral;
            cmd.Parameters.Add("@RemarkVariable_SynergyGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyGeneral;
            cmd.Parameters.Add("@RemarkVariable_StiffinessGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessGeneral;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousGeneral;
            cmd.Parameters.Add("@RemarkVariable_SustainControl", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainControl;
            cmd.Parameters.Add("@RemarkVariable_PosturalControl", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalControl;
            cmd.Parameters.Add("@RemarkVariable_ContractionsControl", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsControl;
            cmd.Parameters.Add("@RemarkVariable_AntagonistControl", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistControl;
            cmd.Parameters.Add("@RemarkVariable_SynergyControl", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyControl;
            cmd.Parameters.Add("@RemarkVariable_StiffinessControl", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessControl;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousControl", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousControl;
            cmd.Parameters.Add("@RemarkVariable_SustainTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainTiming;
            cmd.Parameters.Add("@RemarkVariable_PosturalTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalTiming;
            cmd.Parameters.Add("@RemarkVariable_ContractionsTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsTiming;
            cmd.Parameters.Add("@RemarkVariable_AntagonistTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistTiming;
            cmd.Parameters.Add("@RemarkVariable_SynergyTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyTiming;
            cmd.Parameters.Add("@RemarkVariable_StiffinessTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessTiming;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousTiming;
            cmd.Parameters.Add("@SensorySystem_Vision", SqlDbType.VarChar, -1).Value = SensorySystem_Vision;
            cmd.Parameters.Add("@SensorySystem_Auditory", SqlDbType.VarChar, -1).Value = SensorySystem_Auditory;
            cmd.Parameters.Add("@SensorySystem_Propioceptive", SqlDbType.VarChar, -1).Value = SensorySystem_Propioceptive;
            cmd.Parameters.Add("@SensorySystem_Oromotpor", SqlDbType.VarChar, -1).Value = SensorySystem_Oromotpor;
            cmd.Parameters.Add("@SensorySystem_Vestibular", SqlDbType.VarChar, -1).Value = SensorySystem_Vestibular;
            cmd.Parameters.Add("@SensorySystem_Tactile", SqlDbType.VarChar, -1).Value = SensorySystem_Tactile;
            cmd.Parameters.Add("@SensorySystem_Olfactory", SqlDbType.VarChar, -1).Value = SensorySystem_Olfactory;
            cmd.Parameters.Add("@SIPTInfo_History", SqlDbType.VarChar, -1).Value = SIPTInfo_History;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GraspRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GraspLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_SphericalRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_SphericalLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_HookRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_HookLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GripRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GripLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFL;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Spontaneous", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT3_Spontaneous;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Command", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT3_Command;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Kinesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Finger", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Finger;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Localisation", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Localisation;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_DoubleTactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Tactile", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Tactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Graphesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_PostRotary", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_PostRotary;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Standing", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Standing;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Color", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Color;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Form", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Form;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Size", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Size;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Depth", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Depth;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Figure", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Figure;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Motor", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Motor;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Design", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT6_Design;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Constructional", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT6_Constructional;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Scanning", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT7_Scanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Memory", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT7_Memory;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Postural", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Postural;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Oral", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Oral;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Sequencing", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Sequencing;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Commands", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Commands;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Bilateral", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_Bilateral;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Contralat", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_Contralat;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_PreferredHand", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_PreferredHand;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_CrossingMidline;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Draw", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Draw;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_ClockFace", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_ClockFace;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Filtering", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Filtering;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_MotorPlanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodyImage", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_BodyImage;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodySchema", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_BodySchema;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Laterality", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Laterality;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Remark", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Remark;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_InterestActivity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_InterestCompletion;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Learning", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Learning;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Complexity", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Complexity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_ProblemSolving;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Concentration", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Concentration;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Retension", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Retension;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SpeedPerfom;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Neatness", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Neatness;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Frustation", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Frustation;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Work", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Work;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Reaction", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Reaction;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityTherapist;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityStudents;
            cmd.Parameters.Add("@Cognition_Intelligence", SqlDbType.VarChar, -1).Value = Cognition_Intelligence;
            cmd.Parameters.Add("@Cognition_Attention", SqlDbType.VarChar, -1).Value = Cognition_Attention;
            cmd.Parameters.Add("@Cognition_Memory", SqlDbType.VarChar, -1).Value = Cognition_Memory;
            cmd.Parameters.Add("@Cognition_Adaptibility", SqlDbType.VarChar, -1).Value = Cognition_Adaptibility;
            cmd.Parameters.Add("@Cognition_MotorPlanning", SqlDbType.VarChar, -1).Value = Cognition_MotorPlanning;
            cmd.Parameters.Add("@Cognition_ExecutiveFunction", SqlDbType.VarChar, -1).Value = Cognition_ExecutiveFunction;
            cmd.Parameters.Add("@Cognition_CognitiveFunctions", SqlDbType.VarChar, -1).Value = Cognition_CognitiveFunctions;
            cmd.Parameters.Add("@Integumentary_SkinIntegrity", SqlDbType.VarChar, -1).Value = Integumentary_SkinIntegrity;
            cmd.Parameters.Add("@Integumentary_SkinColor", SqlDbType.VarChar, -1).Value = Integumentary_SkinColor;
            cmd.Parameters.Add("@Integumentary_SkinExtensibility", SqlDbType.VarChar, -1).Value = Integumentary_SkinExtensibility;
            cmd.Parameters.Add("@Respiratory_RateResting", SqlDbType.VarChar, -1).Value = Respiratory_RateResting;
            cmd.Parameters.Add("@Respiratory_PostExercise", SqlDbType.VarChar, -1).Value = Respiratory_PostExercise;
            cmd.Parameters.Add("@Respiratory_Patterns", SqlDbType.VarChar, -1).Value = Respiratory_Patterns;
            cmd.Parameters.Add("@Respiratory_BreathControl", SqlDbType.VarChar, -1).Value = Respiratory_BreathControl;
            cmd.Parameters.Add("@Respiratory_ChestExcursion", SqlDbType.VarChar, -1).Value = Respiratory_ChestExcursion;
            cmd.Parameters.Add("@Cardiovascular_HeartRate", SqlDbType.VarChar, -1).Value = Cardiovascular_HeartRate;
            cmd.Parameters.Add("@Cardiovascular_PostExercise", SqlDbType.VarChar, -1).Value = Cardiovascular_PostExercise;
            cmd.Parameters.Add("@Cardiovascular_BP", SqlDbType.VarChar, -1).Value = Cardiovascular_BP;
            cmd.Parameters.Add("@Cardiovascular_Edema", SqlDbType.VarChar, -1).Value = Cardiovascular_Edema;
            cmd.Parameters.Add("@Cardiovascular_Circulation", SqlDbType.VarChar, -1).Value = Cardiovascular_Circulation;
            cmd.Parameters.Add("@Cardiovascular_EEi", SqlDbType.VarChar, -1).Value = Cardiovascular_EEi;
            cmd.Parameters.Add("@Gastrointestinal_Bowel", SqlDbType.VarChar, -1).Value = Gastrointestinal_Bowel;
            cmd.Parameters.Add("@Gastrointestinal_Intake", SqlDbType.VarChar, -1).Value = Gastrointestinal_Intake;
            cmd.Parameters.Add("@Evaluation_Strengths", SqlDbType.VarChar, -1).Value = Evaluation_Strengths;
            cmd.Parameters.Add("@Evaluation_Concern_Barriers", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Barriers;
            cmd.Parameters.Add("@Evaluation_Concern_Limitations", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Limitations;
            cmd.Parameters.Add("@Evaluation_Concern_Posture", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Posture;
            cmd.Parameters.Add("@Evaluation_Concern_Impairment", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Impairment;
            cmd.Parameters.Add("@Evaluation_Goal_Summary", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Summary;
            cmd.Parameters.Add("@Evaluation_Goal_Previous", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Previous;
            cmd.Parameters.Add("@Evaluation_Goal_LongTerm", SqlDbType.VarChar, -1).Value = Evaluation_Goal_LongTerm;
            cmd.Parameters.Add("@Evaluation_Goal_ShortTerm", SqlDbType.VarChar, -1).Value = Evaluation_Goal_ShortTerm;
            cmd.Parameters.Add("@Evaluation_Goal_Impairment", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Impairment;
            cmd.Parameters.Add("@Evaluation_Plan_Frequency", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Frequency;
            cmd.Parameters.Add("@Evaluation_Plan_Service", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Service;
            cmd.Parameters.Add("@Evaluation_Plan_Strategies", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Strategies;
            cmd.Parameters.Add("@Evaluation_Plan_Equipment", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Equipment;
            cmd.Parameters.Add("@Evaluation_Plan_Education", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Education;
            cmd.Parameters.Add("@Doctor_Physioptherapist", SqlDbType.Int).Value = Doctor_Physioptherapist;
            cmd.Parameters.Add("@Doctor_Occupational", SqlDbType.Int).Value = Doctor_Occupational;
            cmd.Parameters.Add("@Doctor_EnterReport", SqlDbType.Int).Value = Doctor_EnterReport;
            cmd.Parameters.Add("@IsFinal", SqlDbType.Bit).Value = IsFinal;
            cmd.Parameters.Add("@IsGiven", SqlDbType.Bit).Value = IsGiven;
            if (GivenDate > DateTime.MinValue)
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = GivenDate;
            else
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            cmd.Parameters.Add("@FunctionalActivities_Cognition", SqlDbType.VarChar, -1).Value = FunctionalActivities_Cognition;
            cmd.Parameters.Add("@ParticipationAbility_GrossMotor", SqlDbType.VarChar, -1).Value = ParticipationAbility_GrossMotor;
            cmd.Parameters.Add("@ParticipationAbility_FineMotor", SqlDbType.VarChar, -1).Value = ParticipationAbility_FineMotor;
            cmd.Parameters.Add("@ParticipationAbility_Communication", SqlDbType.VarChar, -1).Value = ParticipationAbility_Communication;
            cmd.Parameters.Add("@ParticipationAbility_Cognition", SqlDbType.VarChar, -1).Value = ParticipationAbility_Cognition;
            cmd.Parameters.Add("@Contextual_Personal_Positive", SqlDbType.VarChar, -1).Value = Contextual_Personal_Positive;
            cmd.Parameters.Add("@Contextual_Personal_Negative", SqlDbType.VarChar, -1).Value = Contextual_Personal_Negative;
            cmd.Parameters.Add("@Contextual_Enviremental_Positive", SqlDbType.VarChar, -1).Value = Contextual_Enviremental_Positive;
            cmd.Parameters.Add("@Contextual_Enviremental_Negative", SqlDbType.VarChar, -1).Value = Contextual_Enviremental_Negative;
            cmd.Parameters.Add("@Posture_Alignment_Type", SqlDbType.VarChar, -1).Value = Posture_Alignment_Type;
            cmd.Parameters.Add("@Posture_Gen_Head", SqlDbType.VarChar, -1).Value = Posture_Gen_Head;
            cmd.Parameters.Add("@Posture_Gen_Shoulder", SqlDbType.VarChar, -1).Value = Posture_Gen_Shoulder;
            cmd.Parameters.Add("@Posture_Gen_Ribcage", SqlDbType.VarChar, -1).Value = Posture_Gen_Ribcage;
            cmd.Parameters.Add("@Posture_Gen_Trunk", SqlDbType.VarChar, -1).Value = Posture_Gen_Trunk;
            cmd.Parameters.Add("@Posture_Gen_Pelvis", SqlDbType.VarChar, -1).Value = Posture_Gen_Pelvis;
            cmd.Parameters.Add("@Posture_Gen_Hips", SqlDbType.VarChar, -1).Value = Posture_Gen_Hips;
            cmd.Parameters.Add("@Posture_Gen_Knees", SqlDbType.VarChar, -1).Value = Posture_Gen_Knees;
            cmd.Parameters.Add("@Posture_Gen_Ankle_Feet", SqlDbType.VarChar, -1).Value = Posture_Gen_Ankle_Feet;
            cmd.Parameters.Add("@Posture_Stru_Neck", SqlDbType.VarChar, -1).Value = Posture_Stru_Neck;
            cmd.Parameters.Add("@Posture_Stru_Jaw", SqlDbType.VarChar, -1).Value = Posture_Stru_Jaw;
            cmd.Parameters.Add("@Posture_Stru_Lips", SqlDbType.VarChar, -1).Value = Posture_Stru_Lips;
            cmd.Parameters.Add("@Posture_Stru_Teeth", SqlDbType.VarChar, -1).Value = Posture_Stru_Teeth;
            cmd.Parameters.Add("@Posture_Stru_Tounge", SqlDbType.VarChar, -1).Value = Posture_Stru_Tounge;
            cmd.Parameters.Add("@Posture_Stru_Palate", SqlDbType.VarChar, -1).Value = Posture_Stru_Palate;
            cmd.Parameters.Add("@Posture_Stru_MouthPosture", SqlDbType.VarChar, -1).Value = Posture_Stru_MouthPosture;
            cmd.Parameters.Add("@Posture_Stru_ToungueMove", SqlDbType.VarChar, -1).Value = Posture_Stru_ToungueMove;
            cmd.Parameters.Add("@Posture_Stru_Bite", SqlDbType.VarChar, -1).Value = Posture_Stru_Bite;
            cmd.Parameters.Add("@Posture_Stru_Swallow", SqlDbType.VarChar, -1).Value = Posture_Stru_Swallow;
            cmd.Parameters.Add("@Posture_Stru_Chew", SqlDbType.VarChar, -1).Value = Posture_Stru_Chew;
            cmd.Parameters.Add("@Posture_Stru_Suck", SqlDbType.VarChar, -1).Value = Posture_Stru_Suck;
            cmd.Parameters.Add("@Posture_Stru_BaseSupport", SqlDbType.VarChar, -1).Value = Posture_Stru_BaseSupport;
            cmd.Parameters.Add("@Posture_Stru_CenterOfMass", SqlDbType.VarChar, -1).Value = Posture_Stru_CenterOfMass;
            cmd.Parameters.Add("@Posture_Stru_StrategyForStability", SqlDbType.VarChar, -1).Value = Posture_Stru_StrategyForStability;
            cmd.Parameters.Add("@Posture_Stru_Anticipatory", SqlDbType.VarChar, -1).Value = Posture_Stru_Anticipatory;
            cmd.Parameters.Add("@Posture_Stru_CounterBalance", SqlDbType.VarChar, -1).Value = Posture_Stru_CounterBalance;
            cmd.Parameters.Add("@Posture_Impairment_Muscle", SqlDbType.VarChar, -1).Value = Posture_Impairment_Muscle;
            cmd.Parameters.Add("@Posture_Impairment_Atrophy", SqlDbType.VarChar, -1).Value = Posture_Impairment_Atrophy;
            cmd.Parameters.Add("@Posture_Impairment_Hypertrophy", SqlDbType.VarChar, -1).Value = Posture_Impairment_Hypertrophy;
            cmd.Parameters.Add("@Posture_Impairment_Callosities", SqlDbType.VarChar, -1).Value = Posture_Impairment_Callosities;
            cmd.Parameters.Add("@Posture_GeneralPosture", SqlDbType.VarChar, -1).Value = Posture_GeneralPosture;
            cmd.Parameters.Add("@Movement_TypeOf", SqlDbType.VarChar, -1).Value = Movement_TypeOf;
            cmd.Parameters.Add("@Movement_Plane", SqlDbType.VarChar, -1).Value = Movement_Plane;
            cmd.Parameters.Add("@Movement_Sagittal", SqlDbType.VarChar, -1).Value = Movement_Sagittal;
            cmd.Parameters.Add("@Movement_Coronal", SqlDbType.VarChar, -1).Value = Movement_Coronal;
            cmd.Parameters.Add("@Movement_Transverse", SqlDbType.VarChar, -1).Value = Movement_Transverse;
            cmd.Parameters.Add("@Movement_WeightShift", SqlDbType.VarChar, -1).Value = Movement_WeightShift;
            cmd.Parameters.Add("@Movement_LimbDissociation", SqlDbType.VarChar, -1).Value = Movement_LimbDissociation;
            cmd.Parameters.Add("@Movement_RangeSpeedOfMovements", SqlDbType.VarChar, -1).Value = Movement_RangeSpeedOfMovements;
            cmd.Parameters.Add("@Movement_Balance_Maintain", SqlDbType.VarChar, -1).Value = Movement_Balance_Maintain;
            cmd.Parameters.Add("@Movement_Balance_During", SqlDbType.VarChar, -1).Value = Movement_Balance_During;
            cmd.Parameters.Add("@Movement_Accuracy_Upper", SqlDbType.VarChar, -1).Value = Movement_Accuracy_Upper;
            cmd.Parameters.Add("@Movement_Accuracy_Lower", SqlDbType.VarChar, -1).Value = Movement_Accuracy_Lower;
            cmd.Parameters.Add("@Neuromotor_Recruitment_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Recruitment_Initial;
            cmd.Parameters.Add("@Neuromotor_Recruitment_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Recruitment_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Recruitment_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Recruitment_Termination;
            cmd.Parameters.Add("@Neuromotor_Recruitment_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Recruitment_Control;
            cmd.Parameters.Add("@Neuromotor_Contraction_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Contraction_Initial;
            cmd.Parameters.Add("@Neuromotor_Contraction_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Contraction_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Contraction_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Contraction_Termination;
            cmd.Parameters.Add("@Neuromotor_Contraction_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Contraction_Control;
            cmd.Parameters.Add("@Neuromotor_Coactivation_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Coactivation_Initial;
            cmd.Parameters.Add("@Neuromotor_Coactivation_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Coactivation_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Coactivation_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Coactivation_Termination;
            cmd.Parameters.Add("@Neuromotor_Coactivation_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Coactivation_Control;
            cmd.Parameters.Add("@Neuromotor_Synergy_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Synergy_Initial;
            cmd.Parameters.Add("@Neuromotor_Synergy_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Synergy_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Synergy_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Synergy_Termination;
            cmd.Parameters.Add("@Neuromotor_Synergy_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Synergy_Control;
            cmd.Parameters.Add("@Neuromotor_Stiffness_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Stiffness_Initial;
            cmd.Parameters.Add("@Neuromotor_Stiffness_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Stiffness_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Stiffness_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Stiffness_Termination;
            cmd.Parameters.Add("@Neuromotor_Stiffness_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Stiffness_Control;
            cmd.Parameters.Add("@Neuromotor_Extraneous_Initial", SqlDbType.VarChar, -1).Value = Neuromotor_Extraneous_Initial;
            cmd.Parameters.Add("@Neuromotor_Extraneous_Sustainance", SqlDbType.VarChar, -1).Value = Neuromotor_Extraneous_Sustainance;
            cmd.Parameters.Add("@Neuromotor_Extraneous_Termination", SqlDbType.VarChar, -1).Value = Neuromotor_Extraneous_Termination;
            cmd.Parameters.Add("@Neuromotor_Extraneous_Control", SqlDbType.VarChar, -1).Value = Neuromotor_Extraneous_Control;
            cmd.Parameters.Add("@OtherTest_Tardieus_TA_Right", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_TA_Right;
            cmd.Parameters.Add("@OtherTest_Tardieus_TA_Left", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_TA_Left;
            cmd.Parameters.Add("@OtherTest_Tardieus_Hamstring_Right", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Hamstring_Right;
            cmd.Parameters.Add("@OtherTest_Tardieus_Hamstring_Left", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Hamstring_Left;
            cmd.Parameters.Add("@OtherTest_Tardieus_Adductor_Right", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Adductor_Right;
            cmd.Parameters.Add("@OtherTest_Tardieus_Adductor_Left", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Adductor_Left;
            cmd.Parameters.Add("@OtherTest_Tardieus_Hip_Right", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Hip_Right;
            cmd.Parameters.Add("@OtherTest_Tardieus_Hip_Left", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Hip_Left;
            cmd.Parameters.Add("@OtherTest_Tardieus_Biceps_Right", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Biceps_Right;
            cmd.Parameters.Add("@OtherTest_Tardieus_Biceps_Left", SqlDbType.VarChar, -1).Value = OtherTest_Tardieus_Biceps_Left;
            cmd.Parameters.Add("@SelectionMotorControl_Muscle", SqlDbType.VarChar, -1).Value = SelectionMotorControl_Muscle;
            cmd.Parameters.Add("@SelectionMotorControl_Denvers", SqlDbType.VarChar, -1).Value = SelectionMotorControl_Denvers;
            cmd.Parameters.Add("@SelectionMotorControl_GMFM", SqlDbType.VarChar, -1).Value = SelectionMotorControl_GMFM;
            cmd.Parameters.Add("@SelectionMotorControl_MAS", SqlDbType.VarChar, -1).Value = SelectionMotorControl_MAS;
            cmd.Parameters.Add("@SelectionMotorControl_Observation", SqlDbType.VarChar, -1).Value = SelectionMotorControl_Observation;
            cmd.Parameters.Add("@TheFourA_Arousal", SqlDbType.VarChar, -1).Value = TheFourA_Arousal;
            cmd.Parameters.Add("@TheFourA_Attention", SqlDbType.VarChar, -1).Value = TheFourA_Attention;
            cmd.Parameters.Add("@TheFourA_Affect", SqlDbType.VarChar, -1).Value = TheFourA_Affect;
            cmd.Parameters.Add("@TheFourA_Action", SqlDbType.VarChar, -1).Value = TheFourA_Action;
            cmd.Parameters.Add("@TheFourA_StateRegulation", SqlDbType.VarChar, -1).Value = TheFourA_StateRegulation;
            cmd.Parameters.Add("@FA_GrossMotor_Ability", SqlDbType.VarChar, -1).Value = FA_GrossMotor_Ability;
            cmd.Parameters.Add("@FA_GrossMotor_Limit", SqlDbType.VarChar, -1).Value = FA_GrossMotor_Limit;
            cmd.Parameters.Add("@FA_FineMotor_Ability", SqlDbType.VarChar, -1).Value = FA_FineMotor_Ability;
            cmd.Parameters.Add("@FA_FineMotor_Limit", SqlDbType.VarChar, -1).Value = FA_FineMotor_Limit;
            cmd.Parameters.Add("@FA_Communication_Ability", SqlDbType.VarChar, -1).Value = FA_Communication_Ability;
            cmd.Parameters.Add("@FA_Communication_Limit", SqlDbType.VarChar, -1).Value = FA_Communication_Limit;
            cmd.Parameters.Add("@FA_Cognition_Ability", SqlDbType.VarChar, -1).Value = FA_Cognition_Ability;
            cmd.Parameters.Add("@FA_Cognition_Limit", SqlDbType.VarChar, -1).Value = FA_Cognition_Limit;
            cmd.Parameters.Add("@ParticipationAbility_GrossMotor_Limit", SqlDbType.VarChar, -1).Value = ParticipationAbility_GrossMotor_Limit;
            cmd.Parameters.Add("@ParticipationAbility_FineMotor_Limit", SqlDbType.VarChar, -1).Value = ParticipationAbility_FineMotor_Limit;
            cmd.Parameters.Add("@ParticipationAbility_Communication_Limit", SqlDbType.VarChar, -1).Value = ParticipationAbility_Communication_Limit;
            cmd.Parameters.Add("@ParticipationAbility_Cognition_Limit", SqlDbType.VarChar, -1).Value = ParticipationAbility_Cognition_Limit;
            cmd.Parameters.Add("@Sensory_Profile_NameResults", SqlDbType.VarChar, -1).Value = Sensory_Profile_NameResults;
            cmd.Parameters.Add("@SensoryProfile_Profile", SqlDbType.VarChar, -1).Value = SensoryProfile_Profile;
            cmd.Parameters.Add("@DiagnosisIDs", SqlDbType.VarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public DataTable GetTherapist(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand(" SELECT DM.Prefix +' '+ DM.FullName AS FullName FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            
            return db.DbRead(cmd);
        }

        public DataTable SearchReval(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportRevalMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }

        public DataSet GetReval(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportREVALMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            return db.DbFetch(cmd);
        }

        public int Setreval(int _appointmentID, string DataCollection_CurrentConcern, string DataCollection_ImprovementSinceLastEval, string DataCollection_MedicalHistory, string DataCollection_DailyRoutine,
           string DataCollection_Expectaion, string DataCollection_TherapyHistory, string DataCollection_Sources, string DataCollection_NumberVisit,
           string DataCollection_AdaptedEquipment, string Morphology_Height, string Morphology_Weight, string Morphology_TrueLimbLengthLeft, string Morphology_TrueLimbLengthRight,
           string Morphology_LimbLeft, string Morphology_LimbRight, string Morphology_ArmLength, string Morphology_ArmLeft, string Morphology_ArmRight,
           string Morphology_Head, string Morphology_Nipple, string Morphology_Waist,

           string Morphology_GirthUpperLimb_Above_ElbowLevel1, string Morphology_GirthUpperLimb_Above_ElbowLevel2, string Morphology_GirthUpperLimb_Above_ElbowLevel3,
           string Morphology_GirthUpperLimb_Above_ElbowLeft1, string Morphology_GirthUpperLimb_Above_ElbowLeft2, string Morphology_GirthUpperLimb_Above_ElbowLeft3,
           string Morphology_GirthUpperLimb_Above_ElbowRight1, string Morphology_GirthUpperLimb_Above_ElbowRight2, string Morphology_GirthUpperLimb_Above_ElbowRight3,
           string Morphology_GirthUpperLimb_At_ElbowLevel, string Morphology_GirthUpperLimb_At_ElbowLeft, string Morphology_GirthUpperLimb_At_ElbowRight,
           string Morphology_GirthUpperLimb_Below_ElbowLevel1, string Morphology_GirthUpperLimb_Below_ElbowLevel2, string Morphology_GirthUpperLimb_Below_ElbowLevel3,
           string Morphology_GirthUpperLimb_Below_ElbowLeft1, string Morphology_GirthUpperLimb_Below_ElbowLeft2, string Morphology_GirthUpperLimb_Below_ElbowLeft3,
           string Morphology_GirthUpperLimb_Below_ElbowRight1, string Morphology_GirthUpperLimb_Below_ElbowRight2, string Morphology_GirthUpperLimb_Below_ElbowRight3,
           string Morphology_GirthLowerLimb_Above_KneeLevel1, string Morphology_GirthLowerLimb_Above_KneeLevel2, string Morphology_GirthLowerLimb_Above_KneeLevel3,
           string Morphology_GirthLowerLimb_Above_KneeLeft1, string Morphology_GirthLowerLimb_Above_KneeLeft2, string Morphology_GirthLowerLimb_Above_KneeLeft3,
           string Morphology_GirthLowerLimb_Above_KneeRight1, string Morphology_GirthLowerLimb_Above_KneeRight2, string Morphology_GirthLowerLimb_Above_KneeRight3,
           string Morphology_GirthLowerLimb_At_KneeLevel, string Morphology_GirthLowerLimb_At_KneeLeft, string Morphology_GirthLowerLimb_At_KneeRight,
           string Morphology_GirthLowerLimb_Below_KneeLevel1, string Morphology_GirthLowerLimb_Below_KneeLevel2, string Morphology_GirthLowerLimb_Below_KneeLevel3,
           string Morphology_GirthLowerLimb_Below_KneeLeft1, string Morphology_GirthLowerLimb_Below_KneeLeft2, string Morphology_GirthLowerLimb_Below_KneeLeft3,
           string Morphology_GirthLowerLimb_Below_KneeRight1, string Morphology_GirthLowerLimb_Below_KneeRight2, string Morphology_GirthLowerLimb_Below_KneeRight3,

           string Morphology_OralMotorFactors, string FunctionalActivities_GrossMotor,
           string FunctionalActivities_HandFunction, string FunctionalActivities_FineMotor, string FunctionalActivities_ADL, string FunctionalActivities_OralMotor,
           string FunctionalActivities_Communication, string TestMeasures_GMFCS, string TestMeasures_GMFM, string TestMeasures_GMPM, string TestMeasures_AshworthScale,
           string TestMeasures_TradieusScale, string TestMeasures_OGS, string TestMeasures_Melbourne, string TestMeasures_COPM, string TestMeasures_ClinicalObservation,
           string TestMeasures_Others, string Posture_Alignment, string Posture_Biomechanics, string Posture_Stability, string Posture_Anticipatory, string Posture_Postural,
           string Posture_SignsPostural, string Movement_Inertia, string Movement_Strategies, string Movement_Extremities, string Movement_Stability, string Movement_Overuse,
           string Others_Integration, string Others_Assessments, string Regulatory_Arousal, string Regulatory_Regulation, string Musculoskeletal_Rom1_HipFlexionLeft,
           string Musculoskeletal_Rom1_HipFlexionRight, string Musculoskeletal_Rom1_HipExtensionLeft,
           string Musculoskeletal_Rom1_HipAbductionLeft, string Musculoskeletal_Rom1_HipAbductionRight,
           string Musculoskeletal_Rom1_HipExtensionRight,
           string Musculoskeletal_Rom1_HipExternalLeft, string Musculoskeletal_Rom1_HipExternalRight, string Musculoskeletal_Rom1_HipInternalLeft,
           string Musculoskeletal_Rom1_HipInternalRight, string Musculoskeletal_Rom1_PoplitealLeft, string Musculoskeletal_Rom1_PoplitealRight,
           string Musculoskeletal_Rom1_KneeFlexionLeft, string Musculoskeletal_Rom1_KneeFlexionRight, string Musculoskeletal_Rom1_KneeExtensionLeft,
           string Musculoskeletal_Rom1_KneeExtensionRight, string Musculoskeletal_Rom1_DorsiflexionFlexionLeft, string Musculoskeletal_Rom1_DorsiflexionFlexionRight,
           string Musculoskeletal_Rom1_DorsiflexionExtensionLeft, string Musculoskeletal_Rom1_DorsiflexionExtensionRight, string Musculoskeletal_Rom1_PlantarFlexionLeft,
           string Musculoskeletal_Rom1_PlantarFlexionRight, string Musculoskeletal_Rom1_OthersLeft, string Musculoskeletal_Rom1_OthersRight, string Musculoskeletal_Rom2_ShoulderFlexionLeft,
           string Musculoskeletal_Rom2_ShoulderFlexionRight, string Musculoskeletal_Rom2_ShoulderExtensionLeft, string Musculoskeletal_Rom2_ShoulderExtensionRight,
           string Musculoskeletal_Rom2_HorizontalAbductionLeft, string Musculoskeletal_Rom2_HorizontalAbductionRight, string Musculoskeletal_Rom2_ExternalRotationLeft,
           string Musculoskeletal_Rom2_ExternalRotationRight, string Musculoskeletal_Rom2_InternalRotationLeft, string Musculoskeletal_Rom2_InternalRotationRight,
           string Musculoskeletal_Rom2_ElbowFlexionLeft, string Musculoskeletal_Rom2_ElbowFlexionRight, string Musculoskeletal_Rom2_ElbowExtensionLeft,
           string Musculoskeletal_Rom2_ElbowExtensionRight, string Musculoskeletal_Rom2_SupinationLeft, string Musculoskeletal_Rom2_SupinationRight,
           string Musculoskeletal_Rom2_PronationLeft, string Musculoskeletal_Rom2_PronationRight, string Musculoskeletal_Rom2_WristFlexionLeft,
           string Musculoskeletal_Rom2_WristFlexionRight, string Musculoskeletal_Rom2_WristExtesionLeft, string Musculoskeletal_Rom2_WristExtesionRight,
           string Musculoskeletal_Rom2_OthersLeft, string Musculoskeletal_Rom2_OthersRight, string Musculoskeletal_Strengthlp, string Musculoskeletal_StrengthCC,
           string Musculoskeletal_StrengthMuscle, string Musculoskeletal_StrengthSkeletal, string Musculoskeletal_Mmt_HipflexorsLeft, string Musculoskeletal_Mmt_HipflexorsRight,
           string Musculoskeletal_Mmt_AbductorsLeft, string Musculoskeletal_Mmt_AbductorsRight, string Musculoskeletal_Mmt_ExtensorsLeft, string Musculoskeletal_Mmt_ExtensorsRight,
           string Musculoskeletal_Mmt_HamsLeft, string Musculoskeletal_Mmt_HamsRight, string Musculoskeletal_Mmt_QuadsLeft, string Musculoskeletal_Mmt_QuadsRight,
           string Musculoskeletal_Mmt_TibialisAnteriorLeft, string Musculoskeletal_Mmt_TibialisAnteriorRight, string Musculoskeletal_Mmt_TibialisPosteriorLeft,
           string Musculoskeletal_Mmt_TibialisPosteriorRight, string Musculoskeletal_Mmt_ExtensorDigitorumLeft, string Musculoskeletal_Mmt_ExtensorDigitorumRight,
           string Musculoskeletal_Mmt_ExtensorHallucisLeft, string Musculoskeletal_Mmt_ExtensorHallucisRight, string Musculoskeletal_Mmt_PeroneiLeft, string Musculoskeletal_Mmt_PeroneiRight,
           string Musculoskeletal_Mmt_FlexorDigitorumLeft, string Musculoskeletal_Mmt_FlexorDigitorumRight, string Musculoskeletal_Mmt_FlexorHallucisLeft,
           string Musculoskeletal_Mmt_FlexorHallucisRight, string Musculoskeletal_Mmt_AnteriorDeltoidLeft, string Musculoskeletal_Mmt_AnteriorDeltoidRight,
           string Musculoskeletal_Mmt_PosteriorDeltoidLeft, string Musculoskeletal_Mmt_PosteriorDeltoidRight, string Musculoskeletal_Mmt_MiddleDeltoidLeft,
           string Musculoskeletal_Mmt_MiddleDeltoidRight, string Musculoskeletal_Mmt_SupraspinatusLeft, string Musculoskeletal_Mmt_SupraspinatusRight,
           string Musculoskeletal_Mmt_SerratusAnteriorLeft, string Musculoskeletal_Mmt_SerratusAnteriorRight, string Musculoskeletal_Mmt_RhomboidsLeft,
           string Musculoskeletal_Mmt_RhomboidsRight, string Musculoskeletal_Mmt_BicepsLeft, string Musculoskeletal_Mmt_BicepsRight, string Musculoskeletal_Mmt_TricepsLeft,
           string Musculoskeletal_Mmt_TricepsRight, string Musculoskeletal_Mmt_SupinatorLeft, string Musculoskeletal_Mmt_SupinatorRight, string Musculoskeletal_Mmt_PronatorLeft,
           string Musculoskeletal_Mmt_PronatorRight, string Musculoskeletal_Mmt_ECULeft, string Musculoskeletal_Mmt_ECURight, string Musculoskeletal_Mmt_ECRLeft,
           string Musculoskeletal_Mmt_ECRRight, string Musculoskeletal_Mmt_ECSLeft, string Musculoskeletal_Mmt_ECSRight, string Musculoskeletal_Mmt_FCULeft, string Musculoskeletal_Mmt_FCURight,
           string Musculoskeletal_Mmt_FCRLeft, string Musculoskeletal_Mmt_FCRRight, string Musculoskeletal_Mmt_FCSLeft, string Musculoskeletal_Mmt_FCSRight,
           string Musculoskeletal_Mmt_OpponensPollicisLeft, string Musculoskeletal_Mmt_OpponensPollicisRight, string Musculoskeletal_Mmt_FlexorPollicisLeft,
           string Musculoskeletal_Mmt_FlexorPollicisRight, string Musculoskeletal_Mmt_AbductorPollicisLeft, string Musculoskeletal_Mmt_AbductorPollicisRight,
           string Musculoskeletal_Mmt_ExtensorPollicisLeft, string Musculoskeletal_Mmt_ExtensorPollicisRight, string SignOfCNS_NeuromotorControl, string RemarkVariable_SustainGeneral,
           string RemarkVariable_PosturalGeneral, string RemarkVariable_ContractionsGeneral, string RemarkVariable_AntagonistGeneral, string RemarkVariable_SynergyGeneral,
           string RemarkVariable_StiffinessGeneral, string RemarkVariable_ExtraneousGeneral, string RemarkVariable_SustainControl, string RemarkVariable_PosturalControl,
           string RemarkVariable_ContractionsControl, string RemarkVariable_AntagonistControl, string RemarkVariable_SynergyControl, string RemarkVariable_StiffinessControl,
           string RemarkVariable_ExtraneousControl, string RemarkVariable_SustainTiming, string RemarkVariable_PosturalTiming, string RemarkVariable_ContractionsTiming,
           string RemarkVariable_AntagonistTiming, string RemarkVariable_SynergyTiming, string RemarkVariable_StiffinessTiming, string RemarkVariable_ExtraneousTiming, string SensorySystem_Vision,
           string SensorySystem_Somatosensory, string SensorySystem_Vestibular, string SensorySystem_Auditory, string SensorySystem_Gustatory, string SensoryProfile_Profile, string SIPTInfo_History,
           string SIPTInfo_HandFunction1_GraspRight, string SIPTInfo_HandFunction1_GraspLeft, string SIPTInfo_HandFunction1_SphericalRight, string SIPTInfo_HandFunction1_SphericalLeft,
           string SIPTInfo_HandFunction1_HookRight, string SIPTInfo_HandFunction1_HookLeft, string SIPTInfo_HandFunction1_JawChuckRight, string SIPTInfo_HandFunction1_JawChuckLeft,
           string SIPTInfo_HandFunction1_GripRight, string SIPTInfo_HandFunction1_GripLeft, string SIPTInfo_HandFunction1_ReleaseRight, string SIPTInfo_HandFunction1_ReleaseLeft,
           string SIPTInfo_HandFunction2_OppositionLfR, string SIPTInfo_HandFunction2_OppositionLfL, string SIPTInfo_HandFunction2_OppositionMFR,
           string SIPTInfo_HandFunction2_OppositionMFL, string SIPTInfo_HandFunction2_OppositionRFR, string SIPTInfo_HandFunction2_OppositionRFL, string SIPTInfo_HandFunction2_PinchLfR,
           string SIPTInfo_HandFunction2_PinchLfL, string SIPTInfo_HandFunction2_PinchMFR, string SIPTInfo_HandFunction2_PinchMFL, string SIPTInfo_HandFunction2_PinchRFR,
           string SIPTInfo_HandFunction2_PinchRFL, string SIPTInfo_SIPT3_Spontaneous, string SIPTInfo_SIPT3_Command, string SIPTInfo_SIPT4_Kinesthesia, string SIPTInfo_SIPT4_Finger,
           string SIPTInfo_SIPT4_Localisation, string SIPTInfo_SIPT4_DoubleTactile, string SIPTInfo_SIPT4_Tactile, string SIPTInfo_SIPT4_Graphesthesia, string SIPTInfo_SIPT4_PostRotary,
           string SIPTInfo_SIPT4_Standing, string SIPTInfo_SIPT5_Color, string SIPTInfo_SIPT5_Form, string SIPTInfo_SIPT5_Size, string SIPTInfo_SIPT5_Depth, string SIPTInfo_SIPT5_Figure,
           string SIPTInfo_SIPT5_Motor, string SIPTInfo_SIPT6_Design, string SIPTInfo_SIPT6_Constructional, string SIPTInfo_SIPT7_Scanning, string SIPTInfo_SIPT7_Memory, string SIPTInfo_SIPT8_Postural,
           string SIPTInfo_SIPT8_Oral, string SIPTInfo_SIPT8_Sequencing, string SIPTInfo_SIPT8_Commands, string SIPTInfo_SIPT9_Bilateral, string SIPTInfo_SIPT9_Contralat,
           string SIPTInfo_SIPT9_PreferredHand, string SIPTInfo_SIPT9_CrossingMidline, string SIPTInfo_SIPT10_Draw, string SIPTInfo_SIPT10_ClockFace, string SIPTInfo_SIPT10_Filtering,
           string SIPTInfo_SIPT10_MotorPlanning, string SIPTInfo_SIPT10_BodyImage, string SIPTInfo_SIPT10_BodySchema, string SIPTInfo_SIPT10_Laterality, string SIPTInfo_ActivityGiven_Remark,
           string SIPTInfo_ActivityGiven_InterestActivity, string SIPTInfo_ActivityGiven_InterestCompletion, string SIPTInfo_ActivityGiven_Learning, string SIPTInfo_ActivityGiven_Complexity,
           string SIPTInfo_ActivityGiven_ProblemSolving, string SIPTInfo_ActivityGiven_Concentration, string SIPTInfo_ActivityGiven_Retension, string SIPTInfo_ActivityGiven_SpeedPerfom,
           string SIPTInfo_ActivityGiven_Neatness, string SIPTInfo_ActivityGiven_Frustation, string SIPTInfo_ActivityGiven_Work, string SIPTInfo_ActivityGiven_Reaction,
           string SIPTInfo_ActivityGiven_SociabilityTherapist, string SIPTInfo_ActivityGiven_SociabilityStudents, string Cognition_Intelligence, string Cognition_Attention, string Cognition_Memory,
           string Cognition_Adaptibility, string Cognition_MotorPlanning, string Cognition_ExecutiveFunction, string Cognition_CognitiveFunctions, string Integumentary_SkinIntegrity,
           string Integumentary_SkinColor, string Integumentary_SkinExtensibility, string Respiratory_RateResting, string Respiratory_PostExercise, string Respiratory_Patterns, string Respiratory_BreathControl,
           string Cardiovascular_HeartRate, string Cardiovascular_PostExercise, string Cardiovascular_BP, string Cardiovascular_Edema, string Cardiovascular_Circulation, string Cardiovascular_EEi,
           string Gastrointestinal_Bowel, string Gastrointestinal_Intake, string Evaluation_Strengths, string Evaluation_Concern_Barriers, string Evaluation_Concern_Limitations,
           string Evaluation_Concern_Posture, string Evaluation_Concern_Impairment, string Evaluation_Goal_Summary, string Evaluation_Goal_Previous, string Evaluation_Goal_LongTerm,
           string Evaluation_Goal_ShortTerm, string Evaluation_Goal_Impairment, string Evaluation_Plan_Frequency, string Evaluation_Plan_Service, string Evaluation_Plan_Strategies,
           string Evaluation_Plan_Equipment, string Evaluation_Plan_Education, int Doctor_Physioptherapist, int Doctor_Occupational, int Doctor_EnterReport, bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int _loginID, string Evalutionadding_Strengths, string Evaluation_Goal_ShortTearm_Previous, string Attention, string Affect, string Action,
           string Sensory_Profile_NameResults, string DiagnosisIDs, string DiagnosisOther)
        {
            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@Attention", SqlDbType.VarChar, -1).Value = Attention;
            cmd.Parameters.Add("@Affect", SqlDbType.VarChar, -1).Value = Affect;
            cmd.Parameters.Add("@Action", SqlDbType.VarChar, -1).Value = Action;
            cmd.Parameters.Add("@Evalutionadding_Strengths", SqlDbType.VarChar, -1).Value = Evalutionadding_Strengths;
            cmd.Parameters.Add("@Evaluation_Goal_ShortTearm_Previous", SqlDbType.VarChar, -1).Value = Evaluation_Goal_ShortTearm_Previous;
            cmd.Parameters.Add("@DataCollection_CurrentConcern", SqlDbType.VarChar, -1).Value = DataCollection_CurrentConcern;
            cmd.Parameters.Add("@DataCollection_ImprovementSinceLastEval", SqlDbType.VarChar, -1).Value = DataCollection_ImprovementSinceLastEval;
            cmd.Parameters.Add("@DataCollection_MedicalHistory", SqlDbType.VarChar, -1).Value = DataCollection_MedicalHistory;
            cmd.Parameters.Add("@DataCollection_DailyRoutine", SqlDbType.VarChar, -1).Value = DataCollection_DailyRoutine;
            cmd.Parameters.Add("@DataCollection_Expectaion", SqlDbType.VarChar, -1).Value = DataCollection_Expectaion;
            cmd.Parameters.Add("@DataCollection_TherapyHistory", SqlDbType.VarChar, -1).Value = DataCollection_TherapyHistory;
            cmd.Parameters.Add("@DataCollection_Sources", SqlDbType.VarChar, -1).Value = DataCollection_Sources;
            cmd.Parameters.Add("@DataCollection_NumberVisit", SqlDbType.VarChar, -1).Value = DataCollection_NumberVisit;
            cmd.Parameters.Add("@DataCollection_AdaptedEquipment", SqlDbType.VarChar, -1).Value = DataCollection_AdaptedEquipment;
            cmd.Parameters.Add("@Morphology_Height", SqlDbType.VarChar, -1).Value = Morphology_Height;
            cmd.Parameters.Add("@Morphology_Weight", SqlDbType.VarChar, -1).Value = Morphology_Weight;
            cmd.Parameters.Add("@Morphology_TrueLimbLengthLeft", SqlDbType.VarChar, -1).Value = Morphology_TrueLimbLengthLeft;
            cmd.Parameters.Add("@Morphology_TrueLimbLengthRight", SqlDbType.VarChar, -1).Value = Morphology_TrueLimbLengthRight;
            cmd.Parameters.Add("@Morphology_LimbLeft", SqlDbType.VarChar, -1).Value = Morphology_LimbLeft;
            cmd.Parameters.Add("@Morphology_LimbRight", SqlDbType.VarChar, -1).Value = Morphology_LimbRight;
            cmd.Parameters.Add("@Morphology_ArmLength", SqlDbType.VarChar, -1).Value = Morphology_ArmLength;
            cmd.Parameters.Add("@Morphology_ArmLeft", SqlDbType.VarChar, -1).Value = Morphology_ArmLeft;
            cmd.Parameters.Add("@Morphology_ArmRight", SqlDbType.VarChar, -1).Value = Morphology_ArmRight;
            cmd.Parameters.Add("@Morphology_Head", SqlDbType.VarChar, -1).Value = Morphology_Head;
            cmd.Parameters.Add("@Morphology_Nipple", SqlDbType.VarChar, -1).Value = Morphology_Nipple;
            cmd.Parameters.Add("@Morphology_Waist", SqlDbType.VarChar, -1).Value = Morphology_Waist;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLevel3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowLeft3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Above_ElbowRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Above_ElbowRight3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowLevel", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowLevel;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowLeft", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowLeft;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_At_ElbowRight", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_At_ElbowRight;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLevel3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowLeft3;

            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight1;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight2;
            cmd.Parameters.Add("@Morphology_GirthUpperLimb_Below_ElbowRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthUpperLimb_Below_ElbowRight3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLevel3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeLeft3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Above_KneeRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Above_KneeRight3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeLevel", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeLevel;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeLeft", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeLeft;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_At_KneeRight", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_At_KneeRight;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLevel3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLevel3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeLeft3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeLeft3;

            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight1", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight1;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight2", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight2;
            cmd.Parameters.Add("@Morphology_GirthLowerLimb_Below_KneeRight3", SqlDbType.VarChar, -1).Value = Morphology_GirthLowerLimb_Below_KneeRight3;

            cmd.Parameters.Add("@Morphology_OralMotorFactors", SqlDbType.VarChar, -1).Value = Morphology_OralMotorFactors;
            cmd.Parameters.Add("@FunctionalActivities_GrossMotor", SqlDbType.VarChar, -1).Value = FunctionalActivities_GrossMotor;
            cmd.Parameters.Add("@FunctionalActivities_HandFunction", SqlDbType.VarChar, -1).Value = FunctionalActivities_HandFunction;
            cmd.Parameters.Add("@FunctionalActivities_FineMotor", SqlDbType.VarChar, -1).Value = FunctionalActivities_FineMotor;
            cmd.Parameters.Add("@FunctionalActivities_ADL", SqlDbType.VarChar, -1).Value = FunctionalActivities_ADL;
            cmd.Parameters.Add("@FunctionalActivities_OralMotor", SqlDbType.VarChar, -1).Value = FunctionalActivities_OralMotor;
            cmd.Parameters.Add("@FunctionalActivities_Communication", SqlDbType.VarChar, -1).Value = FunctionalActivities_Communication;
            cmd.Parameters.Add("@TestMeasures_GMFCS", SqlDbType.VarChar, -1).Value = TestMeasures_GMFCS;
            cmd.Parameters.Add("@TestMeasures_GMFM", SqlDbType.VarChar, -1).Value = TestMeasures_GMFM;
            cmd.Parameters.Add("@TestMeasures_GMPM", SqlDbType.VarChar, -1).Value = TestMeasures_GMPM;
            cmd.Parameters.Add("@TestMeasures_AshworthScale", SqlDbType.VarChar, -1).Value = TestMeasures_AshworthScale;
            cmd.Parameters.Add("@TestMeasures_TradieusScale", SqlDbType.VarChar, -1).Value = TestMeasures_TradieusScale;
            cmd.Parameters.Add("@TestMeasures_OGS", SqlDbType.VarChar, -1).Value = TestMeasures_OGS;
            cmd.Parameters.Add("@TestMeasures_Melbourne", SqlDbType.VarChar, -1).Value = TestMeasures_Melbourne;
            cmd.Parameters.Add("@TestMeasures_COPM", SqlDbType.VarChar, -1).Value = TestMeasures_COPM;
            cmd.Parameters.Add("@TestMeasures_ClinicalObservation", SqlDbType.VarChar, -1).Value = TestMeasures_ClinicalObservation;
            cmd.Parameters.Add("@TestMeasures_Others", SqlDbType.VarChar, -1).Value = TestMeasures_Others;
            cmd.Parameters.Add("@Posture_Alignment", SqlDbType.VarChar, -1).Value = Posture_Alignment;
            cmd.Parameters.Add("@Posture_Biomechanics", SqlDbType.VarChar, -1).Value = Posture_Biomechanics;
            cmd.Parameters.Add("@Posture_Stability", SqlDbType.VarChar, -1).Value = Posture_Stability;
            cmd.Parameters.Add("@Posture_Anticipatory", SqlDbType.VarChar, -1).Value = Posture_Anticipatory;
            cmd.Parameters.Add("@Posture_Postural", SqlDbType.VarChar, -1).Value = Posture_Postural;
            cmd.Parameters.Add("@Posture_SignsPostural", SqlDbType.VarChar, -1).Value = Posture_SignsPostural;
            cmd.Parameters.Add("@Movement_Inertia", SqlDbType.VarChar, -1).Value = Movement_Inertia;
            cmd.Parameters.Add("@Movement_Strategies", SqlDbType.VarChar, -1).Value = Movement_Strategies;
            cmd.Parameters.Add("@Movement_Extremities", SqlDbType.VarChar, -1).Value = Movement_Extremities;
            cmd.Parameters.Add("@Movement_Stability", SqlDbType.VarChar, -1).Value = Movement_Stability;
            cmd.Parameters.Add("@Movement_Overuse", SqlDbType.VarChar, -1).Value = Movement_Overuse;
            cmd.Parameters.Add("@Others_Integration", SqlDbType.VarChar, -1).Value = Others_Integration;
            cmd.Parameters.Add("@Others_Assessments", SqlDbType.VarChar, -1).Value = Others_Assessments;
            cmd.Parameters.Add("@Regulatory_Arousal", SqlDbType.VarChar, -1).Value = Regulatory_Arousal;
            cmd.Parameters.Add("@Regulatory_Regulation", SqlDbType.VarChar, -1).Value = Regulatory_Regulation;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipAbductionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipAbductionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExternalLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipExternalRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipInternalLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_HipInternalRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PoplitealLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PoplitealRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_KneeExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_DorsiflexionExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PlantarFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_PlantarFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_OthersLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom1_OthersRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ShoulderExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_HorizontalAbductionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_HorizontalAbductionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ExternalRotationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ExternalRotationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_InternalRotationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_InternalRotationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowExtensionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_ElbowExtensionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_SupinationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_SupinationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_PronationLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_PronationRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristFlexionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristFlexionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristExtesionLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_WristExtesionRight;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_OthersLeft;
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Rom2_OthersRight;
            cmd.Parameters.Add("@Musculoskeletal_Strengthlp", SqlDbType.VarChar, -1).Value = Musculoskeletal_Strengthlp;
            cmd.Parameters.Add("@Musculoskeletal_StrengthCC", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthCC;
            cmd.Parameters.Add("@Musculoskeletal_StrengthMuscle", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthMuscle;
            cmd.Parameters.Add("@Musculoskeletal_StrengthSkeletal", SqlDbType.VarChar, -1).Value = Musculoskeletal_StrengthSkeletal;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HipflexorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HipflexorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HamsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_HamsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_QuadsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_QuadsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisAnteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisAnteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisPosteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TibialisPosteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorDigitorumLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorDigitorumRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorHallucisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorHallucisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PeroneiLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PeroneiRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorDigitorumLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorDigitorumRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorHallucisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorHallucisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AnteriorDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AnteriorDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PosteriorDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PosteriorDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_MiddleDeltoidLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_MiddleDeltoidRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupraspinatusLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupraspinatusRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SerratusAnteriorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SerratusAnteriorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_RhomboidsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_RhomboidsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_BicepsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_BicepsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TricepsLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_TricepsRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupinatorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_SupinatorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PronatorLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_PronatorRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECULeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECULeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECURight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECURight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECRLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECRRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECSLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ECSRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCULeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCULeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCURight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCURight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCRLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCRRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCSLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FCSRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_OpponensPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_OpponensPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_FlexorPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_AbductorPollicisRight;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisLeft", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorPollicisLeft;
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisRight", SqlDbType.VarChar, -1).Value = Musculoskeletal_Mmt_ExtensorPollicisRight;
            cmd.Parameters.Add("@SignOfCNS_NeuromotorControl", SqlDbType.VarChar, -1).Value = SignOfCNS_NeuromotorControl;
            cmd.Parameters.Add("@RemarkVariable_SustainGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainGeneral;
            cmd.Parameters.Add("@RemarkVariable_PosturalGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalGeneral;
            cmd.Parameters.Add("@RemarkVariable_ContractionsGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsGeneral;
            cmd.Parameters.Add("@RemarkVariable_AntagonistGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistGeneral;
            cmd.Parameters.Add("@RemarkVariable_SynergyGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyGeneral;
            cmd.Parameters.Add("@RemarkVariable_StiffinessGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessGeneral;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousGeneral", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousGeneral;
            cmd.Parameters.Add("@RemarkVariable_SustainControl", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainControl;
            cmd.Parameters.Add("@RemarkVariable_PosturalControl", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalControl;
            cmd.Parameters.Add("@RemarkVariable_ContractionsControl", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsControl;
            cmd.Parameters.Add("@RemarkVariable_AntagonistControl", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistControl;
            cmd.Parameters.Add("@RemarkVariable_SynergyControl", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyControl;
            cmd.Parameters.Add("@RemarkVariable_StiffinessControl", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessControl;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousControl", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousControl;
            cmd.Parameters.Add("@RemarkVariable_SustainTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_SustainTiming;
            cmd.Parameters.Add("@RemarkVariable_PosturalTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_PosturalTiming;
            cmd.Parameters.Add("@RemarkVariable_ContractionsTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_ContractionsTiming;
            cmd.Parameters.Add("@RemarkVariable_AntagonistTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_AntagonistTiming;
            cmd.Parameters.Add("@RemarkVariable_SynergyTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_SynergyTiming;
            cmd.Parameters.Add("@RemarkVariable_StiffinessTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_StiffinessTiming;
            cmd.Parameters.Add("@RemarkVariable_ExtraneousTiming", SqlDbType.VarChar, -1).Value = RemarkVariable_ExtraneousTiming;
            cmd.Parameters.Add("@SensorySystem_Vision", SqlDbType.VarChar, -1).Value = SensorySystem_Vision;
            cmd.Parameters.Add("@SensorySystem_Somatosensory", SqlDbType.VarChar, -1).Value = SensorySystem_Somatosensory;
            cmd.Parameters.Add("@SensorySystem_Vestibular", SqlDbType.VarChar, -1).Value = SensorySystem_Vestibular;
            cmd.Parameters.Add("@SensorySystem_Auditory", SqlDbType.VarChar, -1).Value = SensorySystem_Auditory;
            cmd.Parameters.Add("@SensorySystem_Gustatory", SqlDbType.VarChar, -1).Value = SensorySystem_Gustatory;
            cmd.Parameters.Add("@SensoryProfile_Profile", SqlDbType.VarChar, -1).Value = SensoryProfile_Profile;
            cmd.Parameters.Add("@SIPTInfo_History", SqlDbType.VarChar, -1).Value = SIPTInfo_History;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GraspRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GraspLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_SphericalRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_SphericalLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_HookRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_HookLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GripRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_GripLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.VarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFL;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Spontaneous", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT3_Spontaneous;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Command", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT3_Command;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Kinesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Finger", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Finger;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Localisation", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Localisation;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_DoubleTactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Tactile", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Tactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Graphesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_PostRotary", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_PostRotary;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Standing", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT4_Standing;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Color", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Color;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Form", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Form;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Size", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Size;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Depth", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Depth;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Figure", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Figure;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Motor", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT5_Motor;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Design", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT6_Design;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Constructional", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT6_Constructional;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Scanning", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT7_Scanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Memory", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT7_Memory;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Postural", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Postural;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Oral", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Oral;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Sequencing", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Sequencing;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Commands", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT8_Commands;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Bilateral", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_Bilateral;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Contralat", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_Contralat;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_PreferredHand", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_PreferredHand;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT9_CrossingMidline;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Draw", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Draw;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_ClockFace", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_ClockFace;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Filtering", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Filtering;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_MotorPlanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodyImage", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_BodyImage;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodySchema", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_BodySchema;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Laterality", SqlDbType.VarChar, -1).Value = SIPTInfo_SIPT10_Laterality;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Remark", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Remark;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_InterestActivity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_InterestCompletion;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Learning", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Learning;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Complexity", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Complexity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_ProblemSolving;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Concentration", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Concentration;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Retension", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Retension;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SpeedPerfom;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Neatness", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Neatness;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Frustation", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Frustation;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Work", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Work;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Reaction", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_Reaction;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityTherapist;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.VarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityStudents;
            cmd.Parameters.Add("@Cognition_Intelligence", SqlDbType.VarChar, -1).Value = Cognition_Intelligence;
            cmd.Parameters.Add("@Cognition_Attention", SqlDbType.VarChar, -1).Value = Cognition_Attention;
            cmd.Parameters.Add("@Cognition_Memory", SqlDbType.VarChar, -1).Value = Cognition_Memory;
            cmd.Parameters.Add("@Cognition_Adaptibility", SqlDbType.VarChar, -1).Value = Cognition_Adaptibility;
            cmd.Parameters.Add("@Cognition_MotorPlanning", SqlDbType.VarChar, -1).Value = Cognition_MotorPlanning;
            cmd.Parameters.Add("@Cognition_ExecutiveFunction", SqlDbType.VarChar, -1).Value = Cognition_ExecutiveFunction;
            cmd.Parameters.Add("@Cognition_CognitiveFunctions", SqlDbType.VarChar, -1).Value = Cognition_CognitiveFunctions;
            cmd.Parameters.Add("@Integumentary_SkinIntegrity", SqlDbType.VarChar, -1).Value = Integumentary_SkinIntegrity;
            cmd.Parameters.Add("@Integumentary_SkinColor", SqlDbType.VarChar, -1).Value = Integumentary_SkinColor;
            cmd.Parameters.Add("@Integumentary_SkinExtensibility", SqlDbType.VarChar, -1).Value = Integumentary_SkinExtensibility;
            cmd.Parameters.Add("@Respiratory_RateResting", SqlDbType.VarChar, -1).Value = Respiratory_RateResting;
            cmd.Parameters.Add("@Respiratory_PostExercise", SqlDbType.VarChar, -1).Value = Respiratory_PostExercise;
            cmd.Parameters.Add("@Respiratory_Patterns", SqlDbType.VarChar, -1).Value = Respiratory_Patterns;
            cmd.Parameters.Add("@Respiratory_BreathControl", SqlDbType.VarChar, -1).Value = Respiratory_BreathControl;
            cmd.Parameters.Add("@Cardiovascular_HeartRate", SqlDbType.VarChar, -1).Value = Cardiovascular_HeartRate;
            cmd.Parameters.Add("@Cardiovascular_PostExercise", SqlDbType.VarChar, -1).Value = Cardiovascular_PostExercise;
            cmd.Parameters.Add("@Cardiovascular_BP", SqlDbType.VarChar, -1).Value = Cardiovascular_BP;
            cmd.Parameters.Add("@Cardiovascular_Edema", SqlDbType.VarChar, -1).Value = Cardiovascular_Edema;
            cmd.Parameters.Add("@Cardiovascular_Circulation", SqlDbType.VarChar, -1).Value = Cardiovascular_Circulation;
            cmd.Parameters.Add("@Cardiovascular_EEi", SqlDbType.VarChar, -1).Value = Cardiovascular_EEi;
            cmd.Parameters.Add("@Gastrointestinal_Bowel", SqlDbType.VarChar, -1).Value = Gastrointestinal_Bowel;
            cmd.Parameters.Add("@Gastrointestinal_Intake", SqlDbType.VarChar, -1).Value = Gastrointestinal_Intake;
            cmd.Parameters.Add("@Evaluation_Strengths", SqlDbType.VarChar, -1).Value = Evaluation_Strengths;
            cmd.Parameters.Add("@Evaluation_Concern_Barriers", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Barriers;
            cmd.Parameters.Add("@Evaluation_Concern_Limitations", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Limitations;
            cmd.Parameters.Add("@Evaluation_Concern_Posture", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Posture;
            cmd.Parameters.Add("@Evaluation_Concern_Impairment", SqlDbType.VarChar, -1).Value = Evaluation_Concern_Impairment;
            cmd.Parameters.Add("@Evaluation_Goal_Summary", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Summary;
            cmd.Parameters.Add("@Evaluation_Goal_Previous", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Previous;
            cmd.Parameters.Add("@Evaluation_Goal_LongTerm", SqlDbType.VarChar, -1).Value = Evaluation_Goal_LongTerm;
            cmd.Parameters.Add("@Evaluation_Goal_ShortTerm", SqlDbType.VarChar, -1).Value = Evaluation_Goal_ShortTerm;
            cmd.Parameters.Add("@Evaluation_Goal_Impairment", SqlDbType.VarChar, -1).Value = Evaluation_Goal_Impairment;
            cmd.Parameters.Add("@Evaluation_Plan_Frequency", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Frequency;
            cmd.Parameters.Add("@Evaluation_Plan_Service", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Service;
            cmd.Parameters.Add("@Evaluation_Plan_Strategies", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Strategies;
            cmd.Parameters.Add("@Evaluation_Plan_Equipment", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Equipment;
            cmd.Parameters.Add("@Evaluation_Plan_Education", SqlDbType.VarChar, -1).Value = Evaluation_Plan_Education;
            cmd.Parameters.Add("@Doctor_Physioptherapist", SqlDbType.Int).Value = Doctor_Physioptherapist;
            cmd.Parameters.Add("@Doctor_Occupational", SqlDbType.Int).Value = Doctor_Occupational;
            cmd.Parameters.Add("@Doctor_EnterReport", SqlDbType.Int).Value = Doctor_EnterReport;
            cmd.Parameters.Add("@IsFinal", SqlDbType.Bit).Value = IsFinal;
            cmd.Parameters.Add("@IsGiven", SqlDbType.Bit).Value = IsGiven;
            cmd.Parameters.Add("@Sensory_Profile_NameResults", SqlDbType.VarChar, -1).Value = Sensory_Profile_NameResults;
            if (GivenDate > DateTime.MinValue)
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = GivenDate;
            else
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
    }

}
