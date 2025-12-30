using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SnehBLL
{
    public class ReportSiMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportSiMst_Bll()
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

        public DataTable Search(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportSiMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable DemoSearch(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Demo_ReportSiMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
        public DataSet 
            Get(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportSiMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }

        public int Set(int _appointmentID, string DataCollection_Referral, string DataCollection_MedicalHistory, string DataCollection_DailyRoutine,
           string DataCollection_Expectaion, string DataCollection_TherapyHistory, string DataCollection_Sources, string DataCollection_NumberVisit,
           string DataCollection_AdaptedEquipment, string Morphology_Height, string Morphology_Weight, string Morphology_LimbLength,
           string Morphology_LimbLeft, string Morphology_LimbRight, string Morphology_ArmLength, string Morphology_ArmLeft, string Morphology_ArmRight,
           string Morphology_Head, string Morphology_Nipple, string Morphology_Waist, string Morphology_UpperLimbLevelRight_ABV, string Morphology_UpperLimbLevelLeft_ABV,
           string Morphology_UpperLimbGirthRight_ABV, string Morphology_UpperLimbGirthLeft_ABV, string Morphology_UpperLimbLevelRight_AT,
           string Morphology_UpperLimbLevelLeft_AT, string Morphology_UpperLimbGirthRight_AT, string Morphology_UpperLimbGirthLeft_AT,
           string Morphology_UpperLimbLevelRight_BLW, string Morphology_UpperLimbLevelLeft_BLW, string Morphology_UpperLimbGirthRight_BLW,
           string Morphology_UpperLimbGirthLeft_BLW, string Morphology_LowerLimbLevelRight_ABV, string Morphology_LowerLimbLevelLeft_ABV,
           string Morphology_LowerLimbGirthRight_ABV, string Morphology_LowerLimbGirthLeft_ABV, string Morphology_LowerLimbLevelRight_AT,
           string Morphology_LowerLimbLevelLeft_AT, string Morphology_LowerLimbGirthRight_AT, string Morphology_LowerLimbGirthLeft_AT,
           string Morphology_LowerLimbLevelRight_BLW, string Morphology_LowerLimbLevelLeft_BLW, string Morphology_LowerLimbGirthRight_BLW,
           string Morphology_LowerLimbGirthLeft_BLW, string Morphology_OralMotorFactors, string FunctionalActivities_GrossMotor,
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
           string Evaluation_Plan_Equipment, string Evaluation_Plan_Education, int Doctor_Physioptherapist, int Doctor_Occupational, int Doctor_EnterReport, bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate,
           int _loginID, string Praxistest, string Designcopying, string ConstructionalPraxis, string Oralpraxis, string Posturalpraxis, string Praxisonverbalcommands, string Sequencingpraxis, string Sensoryintegrationtests,
           string Bilateralmotorcoordination, string Motoraccuracy, string Postrotatorynystagmus, string Standingwalkingbalance, string Touchtests, string Graphesthesia, string Kinesthesia, string Localizationoftactilestimuli,
           string Manualformperception, string Visualperceptiontests, string Figuregroundperception, string Spacevisualization, string Others, string Clockface, string Motorplanning, string Bodyimage, string Bodyschema,
           string Laterality, string SensoryName1, string Result1, string SensoryName2, string Result2, string SensoryName3, string Result3, string SensoryName4, string Result4, string SensoryName5, string Result5, string SensoryName6, string Result6,
           string SensoryName7, string Result7, string SensoryName8, string Result8, string SensoryName9, string Result9, string SensoryName10, string Result10, string SensoryName11, string Result11, string SensoryName12, string Result12,
           string SensoryName13, string Result13, string SensoryName14, string Result14, string SensoryName15, string Result15, string SensoryName16, string Result16, string SensoryName17, string Result17, string SensoryName18,
           string Result18, string SensoryName19, string Result19, string SensoryName20, string Result20, string SensoryName21, string Result21, string SensoryName22, string Result22, string SensoryName23, string Result23,
           string SensoryName24, string Result24, string SensoryName25, string Result25, string SensoryName26, string Result26, string SensoryName27, string Result27, string SensoryName28, string Result28, string SensoryName29,
           string Result29, string SensoryName30, string Result30, string SensoryName31, string Result31, string SensoryName32, string Result32, string SensoryName33, string Result33, string SensoryName34, string Result34,
           string SensoryName35, string Result35, string SensoryName36, string Result36, string SensoryName37, string Result37, string SensoryName38, string Result38, string SensoryName39, string Result39, string SensoryName40,
           string Result40,
            string FunctionalAbilities_GrossMotor, string FunctionalAbilities_FineMotor, string FunctionalAbilities_Communication, string FunctionalAbilities_Cognitive,
            string FunctionalAbilities_Behaviour, string FunctionalLimitations_GrossMotor, string FunctionalLimitations_FineMotor, string FunctionalLimitations_Communication,
            string FunctionalLimitations_Cognitive, string FunctionalLimitations_Behaviour, string ParticipationAbilities_GrossMotor, string ParticipationAbilities_FineMotor,
            string ParticipationAbilities_Communication, string ParticipationAbilities_Cognitive, string ParticipationAbilities_Behaviour, string ParticipationLimitations_GrossMotor,
            string ParticipationLimitations_FineMotor, string ParticipationLimitations_Communication, string ParticipationLimitations_Cognitive, string ParticipationLimitations_Behaviour,
            string FamilyStru_NoOfCaregivers, string FamilyStru_TimeWithChild, string FamilyStru_Holiday, string FamilyStru_DivoteTime, string FamilyStru_ContextualFactor, string FamilyStru_Social,
            string FamilyStru_Environment, string FamilyStru_Acceptance, string FamilyStru_Accessibility, string FamilyStru_CompareSibling, string FamilyStru_Working, string FamilyStru_FamilyPressure,
            string FamilyStru_SpentMostTime, string FamilyStru_CloselyInvolved, string FamilyStru_ChooseFreeTime, string FamilyStru_PlayWithToys, string FamilyStru_ToysExplain,
            string FamilyStru_ThrowTantrum, string SchoolInfo_SchoolType, string SchoolInfo_Hours, string SchoolInfo_Traveling, string SchoolInfo_Teachers, string SchoolInfo_SeatingArr, string SchoolInfo_SeatingTol,
            string SchoolInfo_MeanTime, string SchoolInfo_FriendInteraction, string SchoolInfo_Sports, string SchoolInfo_Curricular, string SchoolInfo_Cultural, string SchoolInfo_ShadowTeacher,
            string SchoolInfo_RemarkTeacher, string SchoolInfo_CopyBoard, string SchoolInfo_CW_HW, string SchoolInfo_FollowInstru, string SchoolInfo_SpecialEducator, string SchoolInfo_DeliveryMode,
            string SchoolInfo_AcademicScope, string Behaviour_AtHome, string Behaviour_AtSchool, string Behaviour_WithElder, string Behaviour_WithPeers, string Behaviour_WithTeacher, string Behaviour_AtTheMall,
            string Behaviour_AtPlayground, string BehaviourPl_Constructive, string BehaviourPl_Destructive, string BehaviourPl_CD_Remark, string BehaviourPL_Independant, string BehaviourPL_Supervised,
            string BehaviourPL_IS_Remark, string BehaviourPL_Sedentary, string BehaviourPL_OnTheGo, string BehaviourPL_AgeAppropriate, string BehaviourPL_FollowRule, string BehaviourPL_Bullied,
            string BehaviourPL_PlayAchieved, string BehaviourPL_ToyChoice, string BehaviourPL_Repetitive, string BehaviourPL_Versatile, string BehaviourPL_PartInGroup, string BehaviourPL_IsLeader,
            string BehaviourPL_IsFollower, string BehaviourPL_PretendPlay, string Behaviour_RegulatesSelf, string Behaviour_BehaveNotReg, string Behaviour_WhatCalmDown, string Behaviour_HappyLike,
            string Behaviour_HappyDislike, string Arousal_EvalAlert, string Arousal_GeneralAlert, string Arousal_StimuliResponse, string Arousal_Transition, string Arousal_Optimum, string Arousal_AlertingFactor,
            string Arousal_CalmingFactor, string Attention_InSchool, string Attention_InHome, string Attention_Dividing, string Attention_ChangeActivities, string Attention_AgeAppropriate, string Attention_AttentionSpan,
            string Attention_Distractibility_Home, string Attention_Distractibility_School, string Affect_EmotionRange, string Affect_EmotionExpress, string Affect_Environment, string Affect_Task, string Affect_Individual,
            string Affect_Consistent, string Affect_Characterising, string Action_Planning, string Action_Purposeful, string Action_GoalOriented, string Action_FeedbackDependent, string Social_KnownPeople,
            string Social_Strangers, string Social_Gathering, string Social_Emotional, string Social_Appreciates, string Social_Reaction, string Social_Sadness, string Social_Surprise, string Social_Shock, string Social_Friendships,
            string Social_Relates, string Social_ActivitiestheyEnjoy, string Communication_StartToSpeak, string Communication_Monosyllables, string Communication_Bisyllables,
            string Communication_ShortSentences, string Communication_LongSentence, string Communication_UnusualSounds, string Communication_ImitationOfSpeech,
            string Communication_FacialExpression, string Communication_EyeContact, string Communication_Gestures, string Communication_InterpretationOfLanguage,
            string Communication_UnderstandImplied, string Communication_UnderstandJoke, string Communication_RespondsToName, string Communication_TwoWayInteraction,
            string Communication_NarrateIncidentsHome, string Communication_NarrateIncidentsSchool, string Communication_ExpressionsWants, string Communication_ExpressionsNeeds,
            string Communication_ExpressionsEmotion, string Communication_ExpressionsAchive, string Communication_LanguagSpoken, string Communication_Echolalia,
            string RepetitiveInterests_Dominates, string RepetitiveInterests_Behavior, string RepetitiveInterests_Changes, string SensorySystemsVisual_Focal, string SensorySystemsVisual_Ambient,
            string SensorySystemsVisual_Focus, string SensorySystemsVisual_Depth, string SensorySystemsVisual_Refractive, string SensorySystemsVisual_Physical,
            string SensorySystemsVestibula_Seeking, string SensorySystemsVestibula_Avoiding, string SensorySystemsVestibula_Insecurities, string SensorySystemsOromotor_Defensive,
            string SensorySystemsOromotor_Drooling, string SensorySystemsOromotor_Mouth, string SensorySystemsOromotor_Mouthing, string SensorySystemsOromotor_Chew,
            string SensorySystemsAuditory_Response, string SensorySystemsAuditory_Seeking, string SensorySystemsAuditory_Avoiding, string SensorySystemsOlfactory_seeking,
            string SensorySystemsOlfactory_Avoiding, string SensorySystemsSomatosensory_Seeking, string SensorySystemsSomatosensory_Avoiding,
            string SensorySystemsSomatosensoryProprioceptive_BodyImage, string SensorySystemsSomatosensoryProprioceptive_BodyParts,
            string SensorySystemsSomatosensoryProprioceptive_Clumsiness, string SensorySystemsSomatosensoryProprioceptive_Coordination, string Other_SensoryProfile, string Other_SIPT,
            string Other_DCD, string Other_DSM, string GoalsAndExpectations, string SensorySystemsVisual_Comment, string SensorySystemsVestibula_Comment, string SensorySystemsOromotor_Comment,
            string SensorySystemsAuditory_Comment, string SensorySystemsOlfactory_Comment, string SensorySystemsSomatosensory_Comment,
            string DiagnosisIDs, string DiagnosisOther
            )
        {
            SqlCommand cmd = new SqlCommand("ReportSiMst_Set"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SensoryName1", SqlDbType.VarChar, -1).Value = SensoryName1;
            cmd.Parameters.Add("@Result1", SqlDbType.VarChar, -1).Value = Result1;
            cmd.Parameters.Add("@SensoryName2", SqlDbType.VarChar, -1).Value = SensoryName2;
            cmd.Parameters.Add("@Result2", SqlDbType.VarChar, -1).Value = Result2;
            cmd.Parameters.Add("@SensoryName3", SqlDbType.VarChar, -1).Value = SensoryName3;
            cmd.Parameters.Add("@Result3", SqlDbType.VarChar, -1).Value = Result3;
            cmd.Parameters.Add("@SensoryName4", SqlDbType.VarChar, -1).Value = SensoryName4;
            cmd.Parameters.Add("@Result4", SqlDbType.VarChar, -1).Value = Result4;
            cmd.Parameters.Add("@SensoryName5", SqlDbType.VarChar, -1).Value = SensoryName5;
            cmd.Parameters.Add("@Result5", SqlDbType.VarChar, -1).Value = Result5;
            cmd.Parameters.Add("@SensoryName6", SqlDbType.VarChar, -1).Value = SensoryName6;
            cmd.Parameters.Add("@Result6", SqlDbType.VarChar, -1).Value = Result6;
            cmd.Parameters.Add("@SensoryName7", SqlDbType.VarChar, -1).Value = SensoryName7;
            cmd.Parameters.Add("@Result7", SqlDbType.VarChar, -1).Value = Result7;
            cmd.Parameters.Add("@SensoryName8", SqlDbType.VarChar, -1).Value = SensoryName8;
            cmd.Parameters.Add("@Result8", SqlDbType.VarChar, -1).Value = Result8;
            cmd.Parameters.Add("@SensoryName9", SqlDbType.VarChar, -1).Value = SensoryName9;
            cmd.Parameters.Add("@Result9", SqlDbType.VarChar, -1).Value = Result9;
            cmd.Parameters.Add("@SensoryName10", SqlDbType.VarChar, -1).Value = SensoryName10;
            cmd.Parameters.Add("@Result10", SqlDbType.VarChar, -1).Value = Result10;
            cmd.Parameters.Add("@SensoryName11", SqlDbType.VarChar, -1).Value = SensoryName11;
            cmd.Parameters.Add("@Result11", SqlDbType.VarChar, -1).Value = Result11;
            cmd.Parameters.Add("@SensoryName12", SqlDbType.VarChar, -1).Value = SensoryName12;
            cmd.Parameters.Add("@Result12", SqlDbType.VarChar, -1).Value = Result12;
            cmd.Parameters.Add("@SensoryName13", SqlDbType.VarChar, -1).Value = SensoryName13;
            cmd.Parameters.Add("@Result13", SqlDbType.VarChar, -1).Value = Result13;
            cmd.Parameters.Add("@SensoryName14", SqlDbType.VarChar, -1).Value = SensoryName14;
            cmd.Parameters.Add("@Result14", SqlDbType.VarChar, -1).Value = Result14;
            cmd.Parameters.Add("@SensoryName15", SqlDbType.VarChar, -1).Value = SensoryName15;
            cmd.Parameters.Add("@Result15", SqlDbType.VarChar, -1).Value = Result15;
            cmd.Parameters.Add("@SensoryName16", SqlDbType.VarChar, -1).Value = SensoryName16;
            cmd.Parameters.Add("@Result16", SqlDbType.VarChar, -1).Value = Result16;
            cmd.Parameters.Add("@SensoryName17", SqlDbType.VarChar, -1).Value = SensoryName17;
            cmd.Parameters.Add("@Result17", SqlDbType.VarChar, -1).Value = Result17;
            cmd.Parameters.Add("@SensoryName18", SqlDbType.VarChar, -1).Value = SensoryName18;
            cmd.Parameters.Add("@Result18", SqlDbType.VarChar, -1).Value = Result18;
            cmd.Parameters.Add("@SensoryName19", SqlDbType.VarChar, -1).Value = SensoryName19;
            cmd.Parameters.Add("@Result19", SqlDbType.VarChar, -1).Value = Result19;
            cmd.Parameters.Add("@SensoryName20", SqlDbType.VarChar, -1).Value = SensoryName20;
            cmd.Parameters.Add("@Result20", SqlDbType.VarChar, -1).Value = Result20;
            cmd.Parameters.Add("@SensoryName21", SqlDbType.VarChar, -1).Value = SensoryName21;
            cmd.Parameters.Add("@Result21", SqlDbType.VarChar, -1).Value = Result21;
            cmd.Parameters.Add("@SensoryName22", SqlDbType.VarChar, -1).Value = SensoryName22;
            cmd.Parameters.Add("@Result22", SqlDbType.VarChar, -1).Value = Result22;
            cmd.Parameters.Add("@SensoryName23", SqlDbType.VarChar, -1).Value = SensoryName23;
            cmd.Parameters.Add("@Result23", SqlDbType.VarChar, -1).Value = Result23;
            cmd.Parameters.Add("@SensoryName24", SqlDbType.VarChar, -1).Value = SensoryName24;
            cmd.Parameters.Add("@Result24", SqlDbType.VarChar, -1).Value = Result24;
            cmd.Parameters.Add("@SensoryName25", SqlDbType.VarChar, -1).Value = SensoryName25;
            cmd.Parameters.Add("@Result25", SqlDbType.VarChar, -1).Value = Result25;
            cmd.Parameters.Add("@SensoryName26", SqlDbType.VarChar, -1).Value = SensoryName26;
            cmd.Parameters.Add("@Result26", SqlDbType.VarChar, -1).Value = Result26;
            cmd.Parameters.Add("@SensoryName27", SqlDbType.VarChar, -1).Value = SensoryName27;
            cmd.Parameters.Add("@Result27", SqlDbType.VarChar, -1).Value = Result27;
            cmd.Parameters.Add("@SensoryName28", SqlDbType.VarChar, -1).Value = SensoryName28;
            cmd.Parameters.Add("@Result28", SqlDbType.VarChar, -1).Value = Result28;
            cmd.Parameters.Add("@SensoryName29", SqlDbType.VarChar, -1).Value = SensoryName29;
            cmd.Parameters.Add("@Result29", SqlDbType.VarChar, -1).Value = Result29;
            cmd.Parameters.Add("@SensoryName30", SqlDbType.VarChar, -1).Value = SensoryName30;
            cmd.Parameters.Add("@Result30", SqlDbType.VarChar, -1).Value = Result30;
            cmd.Parameters.Add("@SensoryName31", SqlDbType.VarChar, -1).Value = SensoryName31;
            cmd.Parameters.Add("@Result31", SqlDbType.VarChar, -1).Value = Result31;
            cmd.Parameters.Add("@SensoryName32", SqlDbType.VarChar, -1).Value = SensoryName32;
            cmd.Parameters.Add("@Result32", SqlDbType.VarChar, -1).Value = Result32;
            cmd.Parameters.Add("@SensoryName33", SqlDbType.VarChar, -1).Value = SensoryName33;
            cmd.Parameters.Add("@Result33", SqlDbType.VarChar, -1).Value = Result33;
            cmd.Parameters.Add("@SensoryName34", SqlDbType.VarChar, -1).Value = SensoryName34;
            cmd.Parameters.Add("@Result34", SqlDbType.VarChar, -1).Value = Result34;
            cmd.Parameters.Add("@SensoryName35", SqlDbType.VarChar, -1).Value = SensoryName35;
            cmd.Parameters.Add("@Result35", SqlDbType.VarChar, -1).Value = Result35;
            cmd.Parameters.Add("@SensoryName36", SqlDbType.VarChar, -1).Value = SensoryName36;
            cmd.Parameters.Add("@Result36", SqlDbType.VarChar, -1).Value = Result36;
            cmd.Parameters.Add("@SensoryName37", SqlDbType.VarChar, -1).Value = SensoryName37;
            cmd.Parameters.Add("@Result37", SqlDbType.VarChar, -1).Value = Result37;
            cmd.Parameters.Add("@SensoryName38", SqlDbType.VarChar, -1).Value = SensoryName38;
            cmd.Parameters.Add("@Result38", SqlDbType.VarChar, -1).Value = Result38;
            cmd.Parameters.Add("@SensoryName39", SqlDbType.VarChar, -1).Value = SensoryName39;
            cmd.Parameters.Add("@Result39", SqlDbType.VarChar, -1).Value = Result39;
            cmd.Parameters.Add("@SensoryName40", SqlDbType.VarChar, -1).Value = SensoryName40;
            cmd.Parameters.Add("@Result40", SqlDbType.VarChar, -1).Value = Result40;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@Praxistest", SqlDbType.VarChar, -1).Value = Praxistest;
            cmd.Parameters.Add("@Designcopying", SqlDbType.VarChar, -1).Value = Designcopying;
            cmd.Parameters.Add("@ConstructionalPraxis", SqlDbType.VarChar, -1).Value = ConstructionalPraxis;
            cmd.Parameters.Add("@Oralpraxis", SqlDbType.VarChar, -1).Value = Oralpraxis;
            cmd.Parameters.Add("@Posturalpraxis", SqlDbType.VarChar, -1).Value = Posturalpraxis;
            cmd.Parameters.Add("@Praxisonverbalcommands", SqlDbType.VarChar, -1).Value = Praxisonverbalcommands;
            cmd.Parameters.Add("@Sequencingpraxis", SqlDbType.VarChar, -1).Value = Sequencingpraxis;
            cmd.Parameters.Add("@Sensoryintegrationtests", SqlDbType.VarChar, -1).Value = Sensoryintegrationtests;
            cmd.Parameters.Add("@Bilateralmotorcoordination", SqlDbType.VarChar, -1).Value = Bilateralmotorcoordination;
            cmd.Parameters.Add("@Motoraccuracy", SqlDbType.VarChar, -1).Value = Motoraccuracy;
            cmd.Parameters.Add("@Postrotatorynystagmus", SqlDbType.VarChar, -1).Value = Postrotatorynystagmus;
            cmd.Parameters.Add("@Standingwalkingbalance", SqlDbType.VarChar, -1).Value = Standingwalkingbalance;
            cmd.Parameters.Add("@Touchtests", SqlDbType.VarChar, -1).Value = Touchtests;
            cmd.Parameters.Add("@Graphesthesia", SqlDbType.VarChar, -1).Value = Graphesthesia;
            cmd.Parameters.Add("@Kinesthesia", SqlDbType.VarChar, -1).Value = Kinesthesia;
            cmd.Parameters.Add("@Localizationoftactilestimuli", SqlDbType.VarChar, -1).Value = Localizationoftactilestimuli;
            cmd.Parameters.Add("@Manualformperception", SqlDbType.VarChar, -1).Value = Manualformperception;
            cmd.Parameters.Add("@Visualperceptiontests", SqlDbType.VarChar, -1).Value = Visualperceptiontests;
            cmd.Parameters.Add("@Figuregroundperception", SqlDbType.VarChar, -1).Value = Figuregroundperception;
            cmd.Parameters.Add("@Spacevisualization", SqlDbType.VarChar, -1).Value = Spacevisualization;
            cmd.Parameters.Add("@Others", SqlDbType.VarChar, -1).Value = Others;
            cmd.Parameters.Add("@Clockface", SqlDbType.VarChar, -1).Value = Clockface;
            cmd.Parameters.Add("@Motorplanning", SqlDbType.VarChar, -1).Value = Motorplanning;
            cmd.Parameters.Add("@Bodyimage", SqlDbType.VarChar, -1).Value = Bodyimage;
            cmd.Parameters.Add("@Bodyschema", SqlDbType.VarChar, -1).Value = Bodyschema;
            cmd.Parameters.Add("@Laterality", SqlDbType.VarChar, -1).Value = Laterality;
            cmd.Parameters.Add("@DataCollection_Referral", SqlDbType.VarChar, -1).Value = DataCollection_Referral;
            cmd.Parameters.Add("@DataCollection_MedicalHistory", SqlDbType.VarChar, -1).Value = DataCollection_MedicalHistory;
            cmd.Parameters.Add("@DataCollection_DailyRoutine", SqlDbType.VarChar, -1).Value = DataCollection_DailyRoutine;
            cmd.Parameters.Add("@DataCollection_Expectaion", SqlDbType.VarChar, -1).Value = DataCollection_Expectaion;
            cmd.Parameters.Add("@DataCollection_TherapyHistory", SqlDbType.VarChar, -1).Value = DataCollection_TherapyHistory;
            cmd.Parameters.Add("@DataCollection_Sources", SqlDbType.VarChar, -1).Value = DataCollection_Sources;
            cmd.Parameters.Add("@DataCollection_NumberVisit", SqlDbType.VarChar, -1).Value = DataCollection_NumberVisit;
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
            cmd.Parameters.Add("@Morphology_UpperLimbLevelRight_ABV", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelRight_ABV;
            cmd.Parameters.Add("@Morphology_UpperLimbLevelLeft_ABV", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelLeft_ABV;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthRight_ABV", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthRight_ABV;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthLeft_ABV", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthLeft_ABV;
            cmd.Parameters.Add("@Morphology_UpperLimbLevelRight_AT", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelRight_AT;
            cmd.Parameters.Add("@Morphology_UpperLimbLevelLeft_AT", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelLeft_AT;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthRight_AT", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthRight_AT;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthLeft_AT", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthLeft_AT;
            cmd.Parameters.Add("@Morphology_UpperLimbLevelRight_BLW", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelRight_BLW;
            cmd.Parameters.Add("@Morphology_UpperLimbLevelLeft_BLW", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbLevelLeft_BLW;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthRight_BLW", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthRight_BLW;
            cmd.Parameters.Add("@Morphology_UpperLimbGirthLeft_BLW", SqlDbType.VarChar, -1).Value = Morphology_UpperLimbGirthLeft_BLW;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelRight_ABV", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelRight_ABV;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelLeft_ABV", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelLeft_ABV;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthRight_ABV", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthRight_ABV;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthLeft_ABV", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthLeft_ABV;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelRight_AT", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelRight_AT;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelLeft_AT", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelLeft_AT;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthRight_AT", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthRight_AT;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthLeft_AT", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthLeft_AT;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelRight_BLW", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelRight_BLW;
            cmd.Parameters.Add("@Morphology_LowerLimbLevelLeft_BLW", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbLevelLeft_BLW;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthRight_BLW", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthRight_BLW;
            cmd.Parameters.Add("@Morphology_LowerLimbGirthLeft_BLW", SqlDbType.VarChar, -1).Value = Morphology_LowerLimbGirthLeft_BLW;
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

            cmd.Parameters.Add("@FunctionalAbilities_GrossMotor", SqlDbType.NVarChar, -1).Value = FunctionalAbilities_GrossMotor;
            cmd.Parameters.Add("@FunctionalAbilities_FineMotor", SqlDbType.NVarChar, -1).Value = FunctionalAbilities_FineMotor;
            cmd.Parameters.Add("@FunctionalAbilities_Communication", SqlDbType.NVarChar, -1).Value = FunctionalAbilities_Communication;
            cmd.Parameters.Add("@FunctionalAbilities_Cognitive", SqlDbType.NVarChar, -1).Value = FunctionalAbilities_Cognitive;
            cmd.Parameters.Add("@FunctionalAbilities_Behaviour", SqlDbType.NVarChar, -1).Value = FunctionalAbilities_Behaviour;
            cmd.Parameters.Add("@FunctionalLimitations_GrossMotor", SqlDbType.NVarChar, -1).Value = FunctionalLimitations_GrossMotor;
            cmd.Parameters.Add("@FunctionalLimitations_FineMotor", SqlDbType.NVarChar, -1).Value = FunctionalLimitations_FineMotor;
            cmd.Parameters.Add("@FunctionalLimitations_Communication", SqlDbType.NVarChar, -1).Value = FunctionalLimitations_Communication;
            cmd.Parameters.Add("@FunctionalLimitations_Cognitive", SqlDbType.NVarChar, -1).Value = FunctionalLimitations_Cognitive;
            cmd.Parameters.Add("@FunctionalLimitations_Behaviour", SqlDbType.NVarChar, -1).Value = FunctionalLimitations_Behaviour;
            cmd.Parameters.Add("@ParticipationAbilities_GrossMotor", SqlDbType.NVarChar, -1).Value = ParticipationAbilities_GrossMotor;
            cmd.Parameters.Add("@ParticipationAbilities_FineMotor", SqlDbType.NVarChar, -1).Value = ParticipationAbilities_FineMotor;
            cmd.Parameters.Add("@ParticipationAbilities_Communication", SqlDbType.NVarChar, -1).Value = ParticipationAbilities_Communication;
            cmd.Parameters.Add("@ParticipationAbilities_Cognitive", SqlDbType.NVarChar, -1).Value = ParticipationAbilities_Cognitive;
            cmd.Parameters.Add("@ParticipationAbilities_Behaviour", SqlDbType.NVarChar, -1).Value = ParticipationAbilities_Behaviour;
            cmd.Parameters.Add("@ParticipationLimitations_GrossMotor", SqlDbType.NVarChar, -1).Value = ParticipationLimitations_GrossMotor;
            cmd.Parameters.Add("@ParticipationLimitations_FineMotor", SqlDbType.NVarChar, -1).Value = ParticipationLimitations_FineMotor;
            cmd.Parameters.Add("@ParticipationLimitations_Communication", SqlDbType.NVarChar, -1).Value = ParticipationLimitations_Communication;
            cmd.Parameters.Add("@ParticipationLimitations_Cognitive", SqlDbType.NVarChar, -1).Value = ParticipationLimitations_Cognitive;
            cmd.Parameters.Add("@ParticipationLimitations_Behaviour", SqlDbType.NVarChar, -1).Value = ParticipationLimitations_Behaviour;
            cmd.Parameters.Add("@FamilyStru_NoOfCaregivers", SqlDbType.NVarChar, -1).Value = FamilyStru_NoOfCaregivers;
            cmd.Parameters.Add("@FamilyStru_TimeWithChild", SqlDbType.NVarChar, -1).Value = FamilyStru_TimeWithChild;
            cmd.Parameters.Add("@FamilyStru_Holiday", SqlDbType.NVarChar, -1).Value = FamilyStru_Holiday;
            cmd.Parameters.Add("@FamilyStru_DivoteTime", SqlDbType.NVarChar, -1).Value = FamilyStru_DivoteTime;
            cmd.Parameters.Add("@FamilyStru_ContextualFactor", SqlDbType.NVarChar, -1).Value = FamilyStru_ContextualFactor;
            cmd.Parameters.Add("@FamilyStru_Social", SqlDbType.NVarChar, -1).Value = FamilyStru_Social;
            cmd.Parameters.Add("@FamilyStru_Environment", SqlDbType.NVarChar, -1).Value = FamilyStru_Environment;
            cmd.Parameters.Add("@FamilyStru_Acceptance", SqlDbType.NVarChar, -1).Value = FamilyStru_Acceptance;
            cmd.Parameters.Add("@FamilyStru_Accessibility", SqlDbType.NVarChar, -1).Value = FamilyStru_Accessibility;
            cmd.Parameters.Add("@FamilyStru_CompareSibling", SqlDbType.NVarChar, -1).Value = FamilyStru_CompareSibling;
            cmd.Parameters.Add("@FamilyStru_Working", SqlDbType.NVarChar, -1).Value = FamilyStru_Working;
            cmd.Parameters.Add("@FamilyStru_FamilyPressure", SqlDbType.NVarChar, -1).Value = FamilyStru_FamilyPressure;
            cmd.Parameters.Add("@FamilyStru_SpentMostTime", SqlDbType.NVarChar, -1).Value = FamilyStru_SpentMostTime;
            cmd.Parameters.Add("@FamilyStru_CloselyInvolved", SqlDbType.NVarChar, -1).Value = FamilyStru_CloselyInvolved;
            cmd.Parameters.Add("@FamilyStru_ChooseFreeTime", SqlDbType.NVarChar, -1).Value = FamilyStru_ChooseFreeTime;
            cmd.Parameters.Add("@FamilyStru_PlayWithToys", SqlDbType.NVarChar, -1).Value = FamilyStru_PlayWithToys;
            cmd.Parameters.Add("@FamilyStru_ToysExplain", SqlDbType.NVarChar, -1).Value = FamilyStru_ToysExplain;
            cmd.Parameters.Add("@FamilyStru_ThrowTantrum", SqlDbType.NVarChar, -1).Value = FamilyStru_ThrowTantrum;
            cmd.Parameters.Add("@SchoolInfo_SchoolType", SqlDbType.NVarChar, -1).Value = SchoolInfo_SchoolType;
            cmd.Parameters.Add("@SchoolInfo_Hours", SqlDbType.NVarChar, -1).Value = SchoolInfo_Hours;
            cmd.Parameters.Add("@SchoolInfo_Traveling", SqlDbType.NVarChar, -1).Value = SchoolInfo_Traveling;
            cmd.Parameters.Add("@SchoolInfo_Teachers", SqlDbType.NVarChar, -1).Value = SchoolInfo_Teachers;
            cmd.Parameters.Add("@SchoolInfo_SeatingArr", SqlDbType.NVarChar, -1).Value = SchoolInfo_SeatingArr;
            cmd.Parameters.Add("@SchoolInfo_SeatingTol", SqlDbType.NVarChar, -1).Value = SchoolInfo_SeatingTol;
            cmd.Parameters.Add("@SchoolInfo_MeanTime", SqlDbType.NVarChar, -1).Value = SchoolInfo_MeanTime;
            cmd.Parameters.Add("@SchoolInfo_FriendInteraction", SqlDbType.NVarChar, -1).Value = SchoolInfo_FriendInteraction;
            cmd.Parameters.Add("@SchoolInfo_Sports", SqlDbType.NVarChar, -1).Value = SchoolInfo_Sports;
            cmd.Parameters.Add("@SchoolInfo_Curricular", SqlDbType.NVarChar, -1).Value = SchoolInfo_Curricular;
            cmd.Parameters.Add("@SchoolInfo_Cultural", SqlDbType.NVarChar, -1).Value = SchoolInfo_Cultural;
            cmd.Parameters.Add("@SchoolInfo_ShadowTeacher", SqlDbType.NVarChar, -1).Value = SchoolInfo_ShadowTeacher;
            cmd.Parameters.Add("@SchoolInfo_RemarkTeacher", SqlDbType.NVarChar, -1).Value = SchoolInfo_RemarkTeacher;
            cmd.Parameters.Add("@SchoolInfo_CopyBoard", SqlDbType.NVarChar, -1).Value = SchoolInfo_CopyBoard;
            cmd.Parameters.Add("@SchoolInfo_CW_HW", SqlDbType.NVarChar, -1).Value = SchoolInfo_CW_HW;
            cmd.Parameters.Add("@SchoolInfo_FollowInstru", SqlDbType.NVarChar, -1).Value = SchoolInfo_FollowInstru;
            cmd.Parameters.Add("@SchoolInfo_SpecialEducator", SqlDbType.NVarChar, -1).Value = SchoolInfo_SpecialEducator;
            cmd.Parameters.Add("@SchoolInfo_DeliveryMode", SqlDbType.NVarChar, -1).Value = SchoolInfo_DeliveryMode;
            cmd.Parameters.Add("@SchoolInfo_AcademicScope", SqlDbType.NVarChar, -1).Value = SchoolInfo_AcademicScope;
            cmd.Parameters.Add("@Behaviour_AtHome", SqlDbType.NVarChar, -1).Value = Behaviour_AtHome;
            cmd.Parameters.Add("@Behaviour_AtSchool", SqlDbType.NVarChar, -1).Value = Behaviour_AtSchool;
            cmd.Parameters.Add("@Behaviour_WithElder", SqlDbType.NVarChar, -1).Value = Behaviour_WithElder;
            cmd.Parameters.Add("@Behaviour_WithPeers", SqlDbType.NVarChar, -1).Value = Behaviour_WithPeers;
            cmd.Parameters.Add("@Behaviour_WithTeacher", SqlDbType.NVarChar, -1).Value = Behaviour_WithTeacher;
            cmd.Parameters.Add("@Behaviour_AtTheMall", SqlDbType.NVarChar, -1).Value = Behaviour_AtTheMall;
            cmd.Parameters.Add("@Behaviour_AtPlayground", SqlDbType.NVarChar, -1).Value = Behaviour_AtPlayground;
            cmd.Parameters.Add("@BehaviourPl_Constructive", SqlDbType.NVarChar, -1).Value = BehaviourPl_Constructive;
            cmd.Parameters.Add("@BehaviourPl_Destructive", SqlDbType.NVarChar, -1).Value = BehaviourPl_Destructive;
            cmd.Parameters.Add("@BehaviourPl_CD_Remark", SqlDbType.NVarChar, -1).Value = BehaviourPl_CD_Remark;
            cmd.Parameters.Add("@BehaviourPL_Independant", SqlDbType.NVarChar, -1).Value = BehaviourPL_Independant;
            cmd.Parameters.Add("@BehaviourPL_Supervised", SqlDbType.NVarChar, -1).Value = BehaviourPL_Supervised;
            cmd.Parameters.Add("@BehaviourPL_IS_Remark", SqlDbType.NVarChar, -1).Value = BehaviourPL_IS_Remark;
            cmd.Parameters.Add("@BehaviourPL_Sedentary", SqlDbType.NVarChar, -1).Value = BehaviourPL_Sedentary;
            cmd.Parameters.Add("@BehaviourPL_OnTheGo", SqlDbType.NVarChar, -1).Value = BehaviourPL_OnTheGo;
            cmd.Parameters.Add("@BehaviourPL_AgeAppropriate", SqlDbType.NVarChar, -1).Value = BehaviourPL_AgeAppropriate;
            cmd.Parameters.Add("@BehaviourPL_FollowRule", SqlDbType.NVarChar, -1).Value = BehaviourPL_FollowRule;
            cmd.Parameters.Add("@BehaviourPL_Bullied", SqlDbType.NVarChar, -1).Value = BehaviourPL_Bullied;
            cmd.Parameters.Add("@BehaviourPL_PlayAchieved", SqlDbType.NVarChar, -1).Value = BehaviourPL_PlayAchieved;
            cmd.Parameters.Add("@BehaviourPL_ToyChoice", SqlDbType.NVarChar, -1).Value = BehaviourPL_ToyChoice;
            cmd.Parameters.Add("@BehaviourPL_Repetitive", SqlDbType.NVarChar, -1).Value = BehaviourPL_Repetitive;
            cmd.Parameters.Add("@BehaviourPL_Versatile", SqlDbType.NVarChar, -1).Value = BehaviourPL_Versatile;
            cmd.Parameters.Add("@BehaviourPL_PartInGroup", SqlDbType.NVarChar, -1).Value = BehaviourPL_PartInGroup;
            cmd.Parameters.Add("@BehaviourPL_IsLeader", SqlDbType.NVarChar, -1).Value = BehaviourPL_IsLeader;
            cmd.Parameters.Add("@BehaviourPL_IsFollower", SqlDbType.NVarChar, -1).Value = BehaviourPL_IsFollower;
            cmd.Parameters.Add("@BehaviourPL_PretendPlay", SqlDbType.NVarChar, -1).Value = BehaviourPL_PretendPlay;
            cmd.Parameters.Add("@Behaviour_RegulatesSelf", SqlDbType.NVarChar, -1).Value = Behaviour_RegulatesSelf;
            cmd.Parameters.Add("@Behaviour_BehaveNotReg", SqlDbType.NVarChar, -1).Value = Behaviour_BehaveNotReg;
            cmd.Parameters.Add("@Behaviour_WhatCalmDown", SqlDbType.NVarChar, -1).Value = Behaviour_WhatCalmDown;
            cmd.Parameters.Add("@Behaviour_HappyLike", SqlDbType.NVarChar, -1).Value = Behaviour_HappyLike;
            cmd.Parameters.Add("@Behaviour_HappyDislike", SqlDbType.NVarChar, -1).Value = Behaviour_HappyDislike;
            cmd.Parameters.Add("@Arousal_EvalAlert", SqlDbType.NVarChar, -1).Value = Arousal_EvalAlert;
            cmd.Parameters.Add("@Arousal_GeneralAlert", SqlDbType.NVarChar, -1).Value = Arousal_GeneralAlert;
            cmd.Parameters.Add("@Arousal_StimuliResponse", SqlDbType.NVarChar, -1).Value = Arousal_StimuliResponse;
            cmd.Parameters.Add("@Arousal_Transition", SqlDbType.NVarChar, -1).Value = Arousal_Transition;
            cmd.Parameters.Add("@Arousal_Optimum", SqlDbType.NVarChar, -1).Value = Arousal_Optimum;
            cmd.Parameters.Add("@Arousal_AlertingFactor", SqlDbType.NVarChar, -1).Value = Arousal_AlertingFactor;
            cmd.Parameters.Add("@Arousal_CalmingFactor", SqlDbType.NVarChar, -1).Value = Arousal_CalmingFactor;
            cmd.Parameters.Add("@Attention_InSchool", SqlDbType.NVarChar, -1).Value = Attention_InSchool;
            cmd.Parameters.Add("@Attention_InHome", SqlDbType.NVarChar, -1).Value = Attention_InHome;
            cmd.Parameters.Add("@Attention_Dividing", SqlDbType.NVarChar, -1).Value = Attention_Dividing;
            cmd.Parameters.Add("@Attention_ChangeActivities", SqlDbType.NVarChar, -1).Value = Attention_ChangeActivities;
            cmd.Parameters.Add("@Attention_AgeAppropriate", SqlDbType.NVarChar, -1).Value = Attention_AgeAppropriate;
            cmd.Parameters.Add("@Attention_AttentionSpan", SqlDbType.NVarChar, -1).Value = Attention_AttentionSpan;
            cmd.Parameters.Add("@Attention_Distractibility_Home", SqlDbType.NVarChar, -1).Value = Attention_Distractibility_Home;
            cmd.Parameters.Add("@Attention_Distractibility_School", SqlDbType.NVarChar, -1).Value = Attention_Distractibility_School;
            cmd.Parameters.Add("@Affect_EmotionRange", SqlDbType.NVarChar, -1).Value = Affect_EmotionRange;
            cmd.Parameters.Add("@Affect_EmotionExpress", SqlDbType.NVarChar, -1).Value = Affect_EmotionExpress;
            cmd.Parameters.Add("@Affect_Environment", SqlDbType.NVarChar, -1).Value = Affect_Environment;
            cmd.Parameters.Add("@Affect_Task", SqlDbType.NVarChar, -1).Value = Affect_Task;
            cmd.Parameters.Add("@Affect_Individual", SqlDbType.NVarChar, -1).Value = Affect_Individual;
            cmd.Parameters.Add("@Affect_Consistent", SqlDbType.NVarChar, -1).Value = Affect_Consistent;
            cmd.Parameters.Add("@Affect_Characterising", SqlDbType.NVarChar, -1).Value = Affect_Characterising;
            cmd.Parameters.Add("@Action_Planning", SqlDbType.NVarChar, -1).Value = Action_Planning;
            cmd.Parameters.Add("@Action_Purposeful", SqlDbType.NVarChar, -1).Value = Action_Purposeful;
            cmd.Parameters.Add("@Action_GoalOriented", SqlDbType.NVarChar, -1).Value = Action_GoalOriented;
            cmd.Parameters.Add("@Action_FeedbackDependent", SqlDbType.NVarChar, -1).Value = Action_FeedbackDependent;
            cmd.Parameters.Add("@Social_KnownPeople", SqlDbType.NVarChar, -1).Value = Social_KnownPeople;
            cmd.Parameters.Add("@Social_Strangers", SqlDbType.NVarChar, -1).Value = Social_Strangers;
            cmd.Parameters.Add("@Social_Gathering", SqlDbType.NVarChar, -1).Value = Social_Gathering;
            cmd.Parameters.Add("@Social_Emotional", SqlDbType.NVarChar, -1).Value = Social_Emotional;
            cmd.Parameters.Add("@Social_Appreciates", SqlDbType.NVarChar, -1).Value = Social_Appreciates;
            cmd.Parameters.Add("@Social_Reaction", SqlDbType.NVarChar, -1).Value = Social_Reaction;
            cmd.Parameters.Add("@Social_Sadness", SqlDbType.NVarChar, -1).Value = Social_Sadness;
            cmd.Parameters.Add("@Social_Surprise", SqlDbType.NVarChar, -1).Value = Social_Surprise;
            cmd.Parameters.Add("@Social_Shock", SqlDbType.NVarChar, -1).Value = Social_Shock;
            cmd.Parameters.Add("@Social_Friendships", SqlDbType.NVarChar, -1).Value = Social_Friendships;
            cmd.Parameters.Add("@Social_Relates", SqlDbType.NVarChar, -1).Value = Social_Relates;
            cmd.Parameters.Add("@Social_ActivitiestheyEnjoy", SqlDbType.NVarChar, -1).Value = Social_ActivitiestheyEnjoy;
            cmd.Parameters.Add("@Communication_StartToSpeak", SqlDbType.NVarChar, -1).Value = Communication_StartToSpeak;
            cmd.Parameters.Add("@Communication_Monosyllables", SqlDbType.NVarChar, -1).Value = Communication_Monosyllables;
            cmd.Parameters.Add("@Communication_Bisyllables", SqlDbType.NVarChar, -1).Value = Communication_Bisyllables;
            cmd.Parameters.Add("@Communication_ShortSentences", SqlDbType.NVarChar, -1).Value = Communication_ShortSentences;
            cmd.Parameters.Add("@Communication_LongSentence", SqlDbType.NVarChar, -1).Value = Communication_LongSentence;
            cmd.Parameters.Add("@Communication_UnusualSounds", SqlDbType.NVarChar, -1).Value = Communication_UnusualSounds;
            cmd.Parameters.Add("@Communication_ImitationOfSpeech", SqlDbType.NVarChar, -1).Value = Communication_ImitationOfSpeech;
            cmd.Parameters.Add("@Communication_FacialExpression", SqlDbType.NVarChar, -1).Value = Communication_FacialExpression;
            cmd.Parameters.Add("@Communication_EyeContact", SqlDbType.NVarChar, -1).Value = Communication_EyeContact;
            cmd.Parameters.Add("@Communication_Gestures", SqlDbType.NVarChar, -1).Value = Communication_Gestures;
            cmd.Parameters.Add("@Communication_InterpretationOfLanguage", SqlDbType.NVarChar, -1).Value = Communication_InterpretationOfLanguage;
            cmd.Parameters.Add("@Communication_UnderstandImplied", SqlDbType.NVarChar, -1).Value = Communication_UnderstandImplied;
            cmd.Parameters.Add("@Communication_UnderstandJoke", SqlDbType.NVarChar, -1).Value = Communication_UnderstandJoke;
            cmd.Parameters.Add("@Communication_RespondsToName", SqlDbType.NVarChar, -1).Value = Communication_RespondsToName;
            cmd.Parameters.Add("@Communication_TwoWayInteraction", SqlDbType.NVarChar, -1).Value = Communication_TwoWayInteraction;
            cmd.Parameters.Add("@Communication_NarrateIncidentsHome", SqlDbType.NVarChar, -1).Value = Communication_NarrateIncidentsHome;
            cmd.Parameters.Add("@Communication_NarrateIncidentsSchool", SqlDbType.NVarChar, -1).Value = Communication_NarrateIncidentsSchool;
            cmd.Parameters.Add("@Communication_ExpressionsWants", SqlDbType.NVarChar, -1).Value = Communication_ExpressionsWants;
            cmd.Parameters.Add("@Communication_ExpressionsNeeds", SqlDbType.NVarChar, -1).Value = Communication_ExpressionsNeeds;
            cmd.Parameters.Add("@Communication_ExpressionsEmotion", SqlDbType.NVarChar, -1).Value = Communication_ExpressionsEmotion;
            cmd.Parameters.Add("@Communication_ExpressionsAchive", SqlDbType.NVarChar, -1).Value = Communication_ExpressionsAchive;
            cmd.Parameters.Add("@Communication_LanguagSpoken", SqlDbType.NVarChar, -1).Value = Communication_LanguagSpoken;
            cmd.Parameters.Add("@Communication_Echolalia", SqlDbType.NVarChar, -1).Value = Communication_Echolalia;
            cmd.Parameters.Add("@RepetitiveInterests_Dominates", SqlDbType.NVarChar, -1).Value = RepetitiveInterests_Dominates;
            cmd.Parameters.Add("@RepetitiveInterests_Behavior", SqlDbType.NVarChar, -1).Value = RepetitiveInterests_Behavior;
            cmd.Parameters.Add("@RepetitiveInterests_Changes", SqlDbType.NVarChar, -1).Value = RepetitiveInterests_Changes;
            cmd.Parameters.Add("@SensorySystemsVisual_Focal", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Focal;
            cmd.Parameters.Add("@SensorySystemsVisual_Ambient", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Ambient;
            cmd.Parameters.Add("@SensorySystemsVisual_Focus", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Focus;
            cmd.Parameters.Add("@SensorySystemsVisual_Depth", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Depth;
            cmd.Parameters.Add("@SensorySystemsVisual_Refractive", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Refractive;
            cmd.Parameters.Add("@SensorySystemsVisual_Physical", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Physical;
            cmd.Parameters.Add("@SensorySystemsVestibula_Seeking", SqlDbType.NVarChar, -1).Value = SensorySystemsVestibula_Seeking;
            cmd.Parameters.Add("@SensorySystemsVestibula_Avoiding", SqlDbType.NVarChar, -1).Value = SensorySystemsVestibula_Avoiding;
            cmd.Parameters.Add("@SensorySystemsVestibula_Insecurities", SqlDbType.NVarChar, -1).Value = SensorySystemsVestibula_Insecurities;
            cmd.Parameters.Add("@SensorySystemsOromotor_Defensive", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Defensive;
            cmd.Parameters.Add("@SensorySystemsOromotor_Drooling", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Drooling;
            cmd.Parameters.Add("@SensorySystemsOromotor_Mouth", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Mouth;
            cmd.Parameters.Add("@SensorySystemsOromotor_Mouthing", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Mouthing;
            cmd.Parameters.Add("@SensorySystemsOromotor_Chew", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Chew;
            cmd.Parameters.Add("@SensorySystemsAuditory_Response", SqlDbType.NVarChar, -1).Value = SensorySystemsAuditory_Response;
            cmd.Parameters.Add("@SensorySystemsAuditory_Seeking", SqlDbType.NVarChar, -1).Value = SensorySystemsAuditory_Seeking;
            cmd.Parameters.Add("@SensorySystemsAuditory_Avoiding", SqlDbType.NVarChar, -1).Value = SensorySystemsAuditory_Avoiding;
            cmd.Parameters.Add("@SensorySystemsOlfactory_seeking", SqlDbType.NVarChar, -1).Value = SensorySystemsOlfactory_seeking;
            cmd.Parameters.Add("@SensorySystemsOlfactory_Avoiding", SqlDbType.NVarChar, -1).Value = SensorySystemsOlfactory_Avoiding;
            cmd.Parameters.Add("@SensorySystemsSomatosensory_Seeking", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensory_Seeking;
            cmd.Parameters.Add("@SensorySystemsSomatosensory_Avoiding", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensory_Avoiding;
            cmd.Parameters.Add("@SensorySystemsSomatosensoryProprioceptive_BodyImage", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensoryProprioceptive_BodyImage;
            cmd.Parameters.Add("@SensorySystemsSomatosensoryProprioceptive_BodyParts", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensoryProprioceptive_BodyParts;
            cmd.Parameters.Add("@SensorySystemsSomatosensoryProprioceptive_Clumsiness", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensoryProprioceptive_Clumsiness;
            cmd.Parameters.Add("@SensorySystemsSomatosensoryProprioceptive_Coordination", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensoryProprioceptive_Coordination;
            cmd.Parameters.Add("@Other_SensoryProfile", SqlDbType.NVarChar, -1).Value = Other_SensoryProfile;
            cmd.Parameters.Add("@Other_SIPT", SqlDbType.NVarChar, -1).Value = Other_SIPT;
            cmd.Parameters.Add("@Other_DCD", SqlDbType.NVarChar, -1).Value = Other_DCD;
            cmd.Parameters.Add("@Other_DSM", SqlDbType.NVarChar, -1).Value = Other_DSM;
            cmd.Parameters.Add("@GoalsAndExpectations", SqlDbType.NVarChar, -1).Value = GoalsAndExpectations;
            cmd.Parameters.Add("@SensorySystemsVisual_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsVisual_Comment;
            cmd.Parameters.Add("@SensorySystemsVestibula_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsVestibula_Comment;
            cmd.Parameters.Add("@SensorySystemsOromotor_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsOromotor_Comment;
            cmd.Parameters.Add("@SensorySystemsAuditory_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsAuditory_Comment;
            cmd.Parameters.Add("@SensorySystemsOlfactory_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsOlfactory_Comment;
            cmd.Parameters.Add("@SensorySystemsSomatosensory_Comment", SqlDbType.NVarChar, -1).Value = SensorySystemsSomatosensory_Comment;

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

        public DataTable GetTherapist(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand(" SELECT DM.Prefix +' '+ DM.FullName AS FullName FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }
    }
}
