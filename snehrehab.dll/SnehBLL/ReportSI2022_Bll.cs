using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SnehBLL
{
    public class ReportSI2022_Bll
    {
        DbHelper.SqlDb db;

        public ReportSI2022_Bll()
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
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 50).Value = _fullName;
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
        public DataTable SI_Search2022(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportSiMst2022_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 50).Value = _fullName;
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
        public DataTable Combine_SI_NDT(int _doctorID, string _fullName, string _reportStatus, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("CombinedReport_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 50).Value = _fullName;
            cmd.Parameters.Add("@ReportStatus", SqlDbType.NVarChar, 50).Value = _reportStatus;
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
        public DataSet Getsi2022(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportSiMst_Get2022"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }
        public int Setsi2022(int _appointmentID,

             //string DailySchedule_Dailyaroutine, string DailySchedule_WakeUpTime, string DailySchedule_RestRoomTime,
             //string DailySchedule_Breakfast, string DailySchedule_BreakFastTime, string DailySchedule_SchoolTime,
             //string DailySchedule_MidMorinig, string DailySchedule_SchoolHours, string DailySchedule_LunchTime, 
             //string DailySchedule_AfternoonRoutine, string DailySchedule_Afternoo_nap, string DailySchedule_DinnerTime, string DailySchedule_PistDinnerAct,

             // string SelfCare_Brushing, string SelfCare_Bathing, string SelfCare_Toileting, string SelfCare_Dressing, string SelfCare_Breakfast,
             //string SelfCare_Lunch, string SelfCare_Snacks, string SelfCare_Dinner, string SelfCare_GettingInBus, string SelfCare_GoingToSchool,
             //string SelfCare_comeBackSchool, string SelfCare_Ambulation, string SelfCare_Homeostaticchanges, string SelfCare_UrinationdetailsBedwetting,

             string FamilyStructure_QualityTimeMother,
            string FamilyStructure_QualityTimeFather, string FamilyStructure_QualityTimeWeekend, string Father_Weekends, string FamilyStructure_TimeForThreapy, string FamilyStructure_AcceptanceCondition,
            string FamilyStructure_ExtraCaricular, string FamilyStructure_Diciplinary, string FamilyStructure_SiblingBrother,
            //string FamilyStructure_SiblingSister, string FamilyStructure_SiblingNA, 
            string FamilyStructure_Expectations, string FamilyStructure_CloselyInvolved, string FAMILY_cmt,

            string Schoolinfo_Attend, string Schoolinfo_Type, string Schoolinfo_SchoolHours, string School_Bus, string Car, string Two_Wheelers, string walking, string Public_Transport, string Schoolinfo_NoOfTeacher,
            /*string Schoolinfo_NoOfStudent,*/ string Floor, string single_bench, string bench2, string round_table, string Schoolinfo_Mealtime, string Schoolinfo_MealType, string Schoolinfo_Shareing,
            string Schoolinfo_HelpEating, string Schoolinfo_Friendship, string Schoolinfo_InteractionPeer, string Schoolinfo_InteractionTeacher,
            string Schoolinfo_AnnualFunction, string Schoolinfo_Sports, string Schoolinfo_Picnic, string Schoolinfo_ExtraCaricular, string Schoolinfo_CopyBoard,
            string Schoolinfo_Instructions, string Schoolinfo_ShadowTeacher, string Schoolinfo_CW_HW, string Schoolinfo_SpecialEducator, string Schoolinfo_DeliveryInformation,
            string Schoolinfo_RemarkTeacher, string SCHOOL_cmt,


            string PersonalSocial_CurrentPlace, string PersonalSocial_WhatHeDoes, string PersonalSocial_BodyAwareness, string PersonalSocial_BodySchema,
            string PersonalSocial_ExploreEnvironment, string PersonalSocial_Motivated, string PersonalSocial_EyeContact, string PersonalSocial_SocialSmile,
            string PersonalSocial_FamilyRegards, /*string PersonalSocial_RateChild,*/ string PersonalSocial_ChildSocially, string PERSONAL_cmt,

            string SpeechLanguage_StartSpeek,
            string SpeechLanguage_Monosyllables, string SpeechLanguage_Bisyllables, string SpeechLanguage_ShrotScentences, string SpeechLanguage_LongScentences,
            string SpeechLanguage_UnusualSoundsJargonSpeech, string SpeechLanguage_speechgestures, string SpeechLanguage_NonverbalfacialExpression, string SpeechLanguage_NonverbalfacialEyeContact,
            string SpeechLanguage_NonverbalfacialGestures, string SpeechLanguage_SimpleComplex, string SpeechLanguage_UnderstandImpliedMeaning,
            string SpeechLanguage_UnderstandJokesarcasm, string SpeechLanguage_Respondstoname, string SpeechLanguage_TwowayInteraction, string SpeechLanguage_NarrateIncidentsAtSchool,
            string SpeechLanguage_NarrateIncidentsAtHome, string SpeechLanguage_Needs, string SpeechLanguage_Emotions,
            string SpeechLanguage_AchievementsFailure, string SpeechLanguage_Echolalia, string Speech_cmt,

            string Behaviour_FreeTime, string unassociated, string solitary, string onlooker, string parallel, string associative, string cooperative, string Behaviour_situationalmeltdowns, string BEHAVIOUR_cmt,

            /*string Arousal_Evaluation, string Arousal_GeneralState,*/
            string rangevalue, string rangevalue2,
            string Arousal_Stimuli, string Arousal_Transition, string Arousal_FactorOCD, string Arousal_ClaimingFactor, string Arousal_DipsDown, string AROUSAL_cmt,

             string Affect_RangeEmotion, string Affect_ExpressEmotion,
            string Affect_Environment, string Affect_Task, string Affect_Individual, string Affect_ThroughOut, string Affect_Charaterising, string Affect_cmt,

              string Attention_AttentionSpan, string Attention_FocusHandhome, string Attention_FocusHandSchool, string Attention_Dividing, string Attention_ChangeActivities, string Attention_AgeAppropriate,
             string Attention_Distractibility, string Focal_Attention, string Joint_Attention, string Divided_Attention, string Sustained_Attention,
              string Alternating_Attention, string Attention_move, string ATTENTION_cmt,

               string Action_MotorPlanning, string Action_Purposeful,
            string Action_GoalOriented, string Action_FeedBackDependent, string Action_Constructive, string Action_cmt,

              //string Interaction_KnowPeople, 

              string Interacts, string cmtgathering, string Does_not_initiate, string Sustain, string Fight, string Freeze, string Fright,
            string Anxious, string Comfortable, string Nervous, string ANS_response, string OTHERS, string Interaction_SocialQues, string Interaction_Happiness, string Interaction_Sadness, string Interaction_Surprise,
            string Interaction_Shock, string Interaction_Friends, string Interaction_Enjoy, string INTERACTION_cmt,

             string TS_Registration, string TS_Orientation, string TS_Discrimination, string TS_Responsiveness, string SS_Bodyawareness, string SS_Bodyschema, string SS_Orientation,
            string SS_Posterior, string SS_Bilateral, string SS_Balance, string SS_Dominance, string SS_Right, string SS_identifies, string SS_point, string SS_Constantly,
            string SS_clumsy, string SS_maneuver, string SS_overly, string SS_stand, string SS_indulge, string SS_textures, string SS_monkey, string SS_swings, string VM_Registration,

            string VM_Orientation, string VM_Discrimination, string VM_Responsiveness, string PS_Registration, string PS_Gradation, string PS_Discrimination, string PS_Responsiveness,
            string OM_Registration, string OM_Orientation, string OM_Discrimination, string OM_Responsiveness, string AS_Auditory, string AS_Orientation, string AS_Responsiveness,
            string AS_discrimination, string AS_Background, string AS_localization, string AS_Analysis, string AS_sequencing, string AS_blending, string VS_Visual,
            string VS_Responsiveness, string VS_scanning, string VS_constancy, string VS_memory, string VS_Perception, string VS_hand, string VS_foot, string VS_discrimination,
            string VS_closure, string VS_Figureground, string VS_Visualmemory, string VS_sequential, string VS_spatial, string OS_Registration, string OS_Orientation,
            string OS_Discrimination, string OS_Responsiveness,





            string TestMeassures_GrossMotor,
            string TestMeassures_FineMotor, string TestMeassures_DenverLanguage, string TestMeassures_DenverPersonal, string Tests_cmt,

                 string score_Communication_2, string Inter_Communication_2, string GROSS_2, string inter_Gross_2, string FINE_2, string inter_FINE_2,
            string PROBLEM_2, string inter_PROBLEM_2, string PERSONAL_2, string inter_PERSONAL_2,

            // string score_Communication_2months, string Inter_Communication_2months, string GROSS_2months, string inter_Gross_2months, string FINE_2months, string inter_FINE_2months,
            //string PROBLEM_2months, string inter_PROBLEM_2moths, string PERSONAL_2months, string inter_PERSONAL_2months, 

            string Comm_3, string inter_3, string GROSS_3, string GROSS_inter_3,
            string FINE_3, string FINE_inter_3, string PROBLEM_3, string PROBLEM_inter_3, string PERSONAL_3, string PERSONAL_inter_3, string Communication_6, string comm_inter_6, string GROSS_6, string GROSS_inter_6,
            string FINE_6, string FINE_inter_6, string PROBLEM_6, string PROBLEM_inter_6, string PERSONAL_6, string PERSONAL_inter_6, string comm_7, string inter_7, string GROSS_7,
            string GROSS_inter_7, string FINE_7, string FINE_inter_7, string PROBLEM_7, string PROBLEM_inter_7, string PERSONAL_7, string PERSONAL_inter_7, string comm_9,
            string inter_9, string GROSS_9, string GROSS_inter_9, string FINE_9, string FINE_inter_9, string PROBLEM_9, string PROBLEM_inter_9, string PERSONAL_9, string PERSONAL_inter_9,
            string comm_10, string inter_10, string GROSS_10, string GROSS_inter_10, string FINE_10, string FINE_inter_10, string PROBLEM_10, string PROBLEM_inter_10, string PERSONAL_10,
            string PERSONAL_inter_10, string comm_11, string inter_11, string GROSS_11, string GROSS_inter_11, string FINE_11, string FINE_inter_11, string PROBLEM_11, string PROBLEM_inter_11,
            string PERSONAL_11, string PERSONAL_inter_11, string comm_13, string inter_13, string GROSS_13, string GROSS_inter_13, string FINE_13, string FINE_inter_13, string PROBLEM_13,
            string PROBLEM_inter_13, string PERSONAL_13, string PERSONAL_inter_13, string comm_15, string inter_15, string GROSS_15, string GROSS_inter_15, string FINE_15, string FINE_inter_15,
            string PROBLEM_15, string PROBLEM_inter_15, string PERSONAL_15, string PERSONAL_inter_15, string comm_17, string inter_17, string GROSS_17, string GROSS_inter_17, string FINE_17,
           string FINE_inter_17, string PROBLEM_17, string PROBLEM_inter_17, string PERSONAL_17, string PERSONAL_inter_17, string comm_19, string inter_19, string GROSS_19, string GROSS_inter_19,
           string FINE_19, string FINE_inter_19, string PROBLEM_19, string PROBLEM_inter_19, string PERSONAL_19, string PERSONAL_inter_19, string comm_21, string inter_21, string GROSS_21,
           string GROSS_inter_21, string FINE_21, string FINE_inter_21, string PROBLEM_21, string PROBLEM_inter_21, string PERSONAL_21, string PERSONAL_inter_21, string comm_23, string inter_23,
           string GROSS_23, string GROSS_inter_23, string FINE_23, string FINE_inter_23, string PROBLEM_23, string PROBLEM_inter_23, string PERSONAL_23, string PERSONAL_inter_23, string comm_25,
           string inter_25, string GROSS_25, string GROSS_inter_25, string FINE_25, string FINE_inter_25, string PROBLEM_25, string PROBLEM_inter_25, string PERSONAL_25, string PERSONAL_inter_25,
           string comm_28, string inter_28, string GROSS_28, string GROSS_inter_28, string FINE_28, string FINE_inter_28, string PROBLEM_28, string PROBLEM_inter_28, string PERSONAL_28,
           string PERSONAL_inter_28, string comm_31, string inter_31, string GROSS_31, string GROSS_inter_31, string FINE_31, string FINE_inter_31, string PROBLEM_31, string PROBLEM_inter_31,
           string PERSONAL_31, string PERSONAL_inter_31, string comm_34, string inter_34, string GROSS_34, string GROSS_inter_34, string FINE_34, string FINE_inter_34, string PROBLEM_34,
           string PROBLEM_inter_34, string PERSONAL_34, string PERSONAL_inter_34, string comm_42, string inter_42, string GROSS_42, string GROSS_inter_42, string FINE_42,
           string FINE_inter_42, string PROBLEM_42, string PROBLEM_inter_42, string PERSONAL_42, string PERSONAL_inter_42, string comm_45, string inter_45, string GROSS_45, string GROSS_inter_45,
           string FINE_45, string FINE_inter_45, string PROBLEM_45, string PROBLEM_inter_45, string PERSONAL_45, string PERSONAL_inter_45, string comm_51, string inter_51, string GROSS_51,
           string GROSS_inter_51, string FINE_51, string FINE_inter_51, string PROBLEM_51, string PROBLEM_inter_51, string PERSONAL_51, string PERSONAL_inter_51, string comm_60,
           string inter_60, string GROSS_60, string GROSS_inter_60, string FINE_60, string FINE_inter_60, string PROBLEM_60, string PROBLEM_inter_60, string PERSONAL_60,
           string PERSONAL_inter_60,
          string MONTHS, string QUESTIONS,


               string General_Processing, string AUDITORY_Processing, string VISUAL_Processing, string TOUCH_Processing, string MOVEMENT_Processing, string ORAL_Processing, string Raw_score, string Total_rawscore,
            string Interpretation, string Comments_1,

             string Score_seeking, string Score_Avoiding, string Score_sensitivity, string Score_Registration, string Score_general, string Score_Auditory, string Score_visual,
            string Score_touch, string Score_movement, string Score_oral, string Score_behavioural,
            string SEEKING, string AVOIDING, string SENSITIVITY_2, string REGISTRATION, string GENERAL, string AUDITORY,
            string VISUAL, string TOUCH, string MOVEMENT, string ORAL, string BEHAVIORAL, string Comments_2,
             string SPchild_Seeker, string SPchild_Avoider, string SPchild_Sensor, string SPchild_Bystander, string SPchild_Auditory_3, string SPchild_Visual_3, string SPchild_Touch_3,
            string SPchild_Movement_3, string SPchild_Body_position, string SPchild_Oral_3, string SPchild_Conduct_3, string SPchild_Social_emotional, string SPchild_Attentional_3,
            string Seeking_Seeker, string Avoiding_Avoider, string Sensitivity_Sensor,
            string Registration_Bystander, string Auditory_3, string Visual_3, string Touch_3, string Movement_3, string Body_position, string Oral_3, string Conduct_3, string Social_emotional,
            string Attentional_3, string Comments_3,
             string SPAdult_Low_Registration, string SPAdult_Sensory_seeking, string SPAdult_Sensory_Sensitivity, string SPAdult_Sensory_Avoiding,
            string Low_Registration, string Sensory_seeking, string Sensory_Sensitivity, string Sensory_Avoiding, string Comments_4,
            string SP_Low_Registration64, string SP_Sensory_seeking_64, string SP_Sensory_Sensitivity64, string SP_Sensory_Avoiding64,
            string Low_Registration_5, string Sensory_seeking_5,
            string Sensory_Sensitivity_5, string Sensory_Avoiding_5, string Comments_5,
             string Older_Low_Registration, string Older_Sensory_seeking, string Older_Sensory_Sensitivity, string Older_Sensory_Avoiding,
            string Low_Registration_6, string Sensory_seeking_6, string Sensory_Sensitivity_6, string Sensory_Avoiding_6, string Comments_6,

             string ABILITY_months, string ABILITY_questions, string ability_TOTAL, string ability_COMMENTS,

             string DCDQ_Throws1, string DCDQ_Throws2, string DCDQ_Throws3, string DCDQ_Catches1, string DCDQ_Catches2, string DCDQ_Catches3,
          string DCDQ_Hits1, string DCDQ_Hits2, string DCDQ_Hits3, string DCDQ_Jumps1, string DCDQ_Jumps2, string DCDQ_Jumps3, string DCDQ_Runs1, string DCDQ_Runs2, string DCDQ_Runs3,
       string DCDQ_Plans1, string DCDQ_Plans2, string DCDQ_Plans3, string DCDQ_Writing1, string DCDQ_Writing2, string DCDQ_Writing3, string DCDQ_legibly1, string DCDQ_legibly2, string DCDQ_legibly3, string DCDQ_Effort1, string DCDQ_Effort2, string DCDQ_Effort3, string DCDQ_Cuts1, string DCDQ_Cuts2, string DCDQ_Cuts3, string DCDQ_Likes1, string DCDQ_Likes2, string DCDQ_Likes3, string DCDQ_Learning1, string DCDQ_Learning2, string DCDQ_Learning3, string DCDQ_Quick1, string DCDQ_Quick2, string DCDQ_Quick3, string DCDQ_Bull1, string DCDQ_Bull2, string DCDQ_Bull3, string DCDQ_Does1, string DCDQ_Does2, string DCDQ_Does3, string DCDQ_Control, string DCDQ_Fine,
            string DCDQ_General, string DCDQ_Total, string DCDQ_INTERPRETATION, string DCDQ_COMMENT,


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
           string SIPTInfo_ActivityGiven_SociabilityTherapist, string SIPTInfo_ActivityGiven_SociabilityStudents,

           string Evaluation_Strengths, string Evaluation_Concern_Barriers, string Evaluation_Concern_Limitations,
           string Evaluation_Concern_Posture, string Evaluation_Concern_Impairment, string Evaluation_Goal_Summary, string Evaluation_Goal_Previous, string Evaluation_Goal_LongTerm,
           string Evaluation_Goal_ShortTerm, string Evaluation_Goal_Impairment, string Evaluation_Plan_Frequency, string Evaluation_Plan_Service, string Evaluation_Plan_Strategies,
           string Evaluation_Plan_Equipment, string Evaluation_Plan_Education,


            /*string Daily_cmt,*/ /*string Self_cmt,*/
            string Treatment_Home, string Treatment_School, string Treatment_Threapy, string Treatment_cmt,


           /*string abilityQuestionsChild,*/


           int Doctor_Physioptherapist, int Doctor_Occupational,
            bool IsFinal,
            bool IsGiven, DateTime GivenDate, string DiagnosisID, string DiagnosisOther,string Doctor_EnterReport)
        //(string DailySchedule_BreakFastContent, string DailySchedule_Dinner_content, string DailySchedule_LunchContent, string DailySchedule_Snacks,string Interaction_RelatesPeople,
        // string SelfCare_CurrentlyEats, string SpeechLanguage_Emotionalmilestones, string SpeechLanguage_LanguageSpoken,string SpeechLanguage_Want, string Mother_Working, string Father_Working, string Househelp,
        // string Schoolinfo_PlatformInteraction, string Schoolinfo_HourOnlineSchool, string Schoolinfo_SitOnlineSchool, string Schoolinfo_TeacherInstruction, string Schoolinfo_SetUp, string Schoolinfo_BehaviourOnlineSchool,
        // string Arousal_Optimal,string Attention_Span,string Interaction_Strangers, string TestMeassures_IQ, string TestMeassures_DQ,string Percentile_Range, string TestMeassures_ASQ, string TestMeassures_HandWriting,
        // string TestMeassures_SIPT, string TestMeassures_SensoryProfile,)
        {
            SqlCommand cmd = new SqlCommand("RPT_SI_SET_TABWISE"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            // cmd.Parameters.Add("@DailySchedule_Dailyaroutine", SqlDbType.NVarChar, -1).Value = DailySchedule_Dailyaroutine;
            // cmd.Parameters.Add("@DailySchedule_WakeUpTime", SqlDbType.NVarChar, -1).Value = DailySchedule_WakeUpTime;
            // //cmd.Parameters.Add("@hfdTime", SqlDbType.NVarChar, -1).Value = hfdTime;
            // cmd.Parameters.Add("@DailySchedule_RestRoomTime", SqlDbType.NVarChar, -1).Value = DailySchedule_RestRoomTime;
            // cmd.Parameters.Add("@DailySchedule_Breakfast", SqlDbType.NVarChar, -1).Value = DailySchedule_Breakfast;
            // cmd.Parameters.Add("@DailySchedule_BreakFastTime", SqlDbType.NVarChar, -1).Value = DailySchedule_BreakFastTime;
            //// cmd.Parameters.Add("@DailySchedule_BreakFastContent", SqlDbType.NVarChar, -1).Value = DailySchedule_BreakFastContent;
            // cmd.Parameters.Add("@DailySchedule_SchoolTime", SqlDbType.NVarChar, -1).Value = DailySchedule_SchoolTime;
            // cmd.Parameters.Add("@DailySchedule_MidMorinig", SqlDbType.NVarChar, -1).Value = DailySchedule_MidMorinig;
            // cmd.Parameters.Add("@DailySchedule_SchoolHours", SqlDbType.NVarChar, -1).Value = DailySchedule_SchoolHours;
            // cmd.Parameters.Add("@DailySchedule_LunchTime", SqlDbType.NVarChar, -1).Value = DailySchedule_LunchTime;
            // //cmd.Parameters.Add("@DailySchedule_LunchContent", SqlDbType.NVarChar, -1).Value = DailySchedule_LunchContent;
            // cmd.Parameters.Add("@DailySchedule_AfternoonRoutine", SqlDbType.NVarChar, -1).Value = DailySchedule_AfternoonRoutine;
            // cmd.Parameters.Add("@DailySchedule_Afternoo_nap", SqlDbType.NVarChar, -1).Value = DailySchedule_Afternoo_nap;
            // //cmd.Parameters.Add("@DailySchedule_Snacks", SqlDbType.NVarChar, -1).Value = DailySchedule_Snacks;
            // //cmd.Parameters.Add("@DailySchedule_Dinner_content", SqlDbType.NVarChar, -1).Value = DailySchedule_Dinner_content;
            // cmd.Parameters.Add("@DailySchedule_DinnerTime", SqlDbType.NVarChar, -1).Value = DailySchedule_DinnerTime;
            // cmd.Parameters.Add("@DailySchedule_PistDinnerAct", SqlDbType.NVarChar, -1).Value = DailySchedule_PistDinnerAct;
            //cmd.Parameters.Add("@SelfCare_CurrentlyEats", SqlDbType.NVarChar, -1).Value = SelfCare_CurrentlyEats;
            //cmd.Parameters.Add("@SelfCare_Brushing", SqlDbType.NVarChar, -1).Value = SelfCare_Brushing;
            //cmd.Parameters.Add("@SelfCare_Bathing", SqlDbType.NVarChar, -1).Value = SelfCare_Bathing;
            //cmd.Parameters.Add("@SelfCare_Toileting", SqlDbType.NVarChar, -1).Value = SelfCare_Toileting;
            //cmd.Parameters.Add("@SelfCare_Dressing", SqlDbType.NVarChar, -1).Value = SelfCare_Dressing;
            //cmd.Parameters.Add("@SelfCare_Breakfast", SqlDbType.NVarChar, -1).Value = SelfCare_Breakfast;
            //cmd.Parameters.Add("@SelfCare_Lunch", SqlDbType.NVarChar, -1).Value = SelfCare_Lunch;
            //cmd.Parameters.Add("@SelfCare_Snacks", SqlDbType.NVarChar, -1).Value = SelfCare_Snacks;
            //cmd.Parameters.Add("@SelfCare_Dinner", SqlDbType.NVarChar, -1).Value = SelfCare_Dinner;
            //cmd.Parameters.Add("@SelfCare_GettingInBus", SqlDbType.NVarChar, -1).Value = SelfCare_GettingInBus;
            //cmd.Parameters.Add("@SelfCare_GoingToSchool", SqlDbType.NVarChar, -1).Value = SelfCare_GoingToSchool;
            //cmd.Parameters.Add("@SelfCare_comeBackSchool", SqlDbType.NVarChar, -1).Value = SelfCare_comeBackSchool;
            //cmd.Parameters.Add("@SelfCare_Ambulation", SqlDbType.NVarChar, -1).Value = SelfCare_Ambulation;
            //cmd.Parameters.Add("@SelfCare_Homeostaticchanges", SqlDbType.NVarChar, -1).Value = SelfCare_Homeostaticchanges;
            //cmd.Parameters.Add("@SelfCare_UrinationdetailsBedwetting", SqlDbType.NVarChar, -1).Value = SelfCare_UrinationdetailsBedwetting;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeMother", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeMother;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeFather", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeFather;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeWeekend", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeWeekend;
            cmd.Parameters.Add("@Father_Weekends", SqlDbType.NVarChar, -1).Value = Father_Weekends;
            cmd.Parameters.Add("@FamilyStructure_TimeForThreapy", SqlDbType.NVarChar, -1).Value = FamilyStructure_TimeForThreapy;
            cmd.Parameters.Add("@FamilyStructure_AcceptanceCondition", SqlDbType.NVarChar, -1).Value = FamilyStructure_AcceptanceCondition;
            cmd.Parameters.Add("@FamilyStructure_ExtraCaricular", SqlDbType.NVarChar, -1).Value = FamilyStructure_ExtraCaricular;
            //cmd.Parameters.Add("@FamilyStructure_ParentsWorking", SqlDbType.NVarChar, -1).Value = FamilyStructure_ParentsWorking;
            cmd.Parameters.Add("@FamilyStructure_Diciplinary", SqlDbType.NVarChar, -1).Value = FamilyStructure_Diciplinary;
            cmd.Parameters.Add("@FamilyStructure_SiblingBrother", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingBrother;
            //cmd.Parameters.Add("@FamilyStructure_SiblingSister", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingSister;
            //cmd.Parameters.Add("@FamilyStructure_SiblingNA", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingNA;
            cmd.Parameters.Add("@FamilyStructure_Expectations", SqlDbType.NVarChar, -1).Value = FamilyStructure_Expectations;
            cmd.Parameters.Add("@FamilyStructure_CloselyInvolved", SqlDbType.NVarChar, -1).Value = FamilyStructure_CloselyInvolved;
            cmd.Parameters.Add("@FAMILY_cmt", SqlDbType.NVarChar, -1).Value = FAMILY_cmt;

            cmd.Parameters.Add("@Schoolinfo_Attend", SqlDbType.NVarChar, -1).Value = Schoolinfo_Attend;
            cmd.Parameters.Add("@Schoolinfo_Type", SqlDbType.NVarChar, -1).Value = Schoolinfo_Type;
            cmd.Parameters.Add("@Schoolinfo_SchoolHours", SqlDbType.NVarChar, -1).Value = Schoolinfo_SchoolHours;
            cmd.Parameters.Add("@School_Bus", SqlDbType.NVarChar, -1).Value = School_Bus;
            cmd.Parameters.Add("@Car", SqlDbType.NVarChar, -1).Value = Car;
            cmd.Parameters.Add("@Two_Wheelers", SqlDbType.NVarChar, -1).Value = Two_Wheelers;
            cmd.Parameters.Add("@walking", SqlDbType.NVarChar, -1).Value = walking;
            cmd.Parameters.Add("@Public_Transport", SqlDbType.NVarChar, -1).Value = Public_Transport;
            cmd.Parameters.Add("@Schoolinfo_NoOfTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_NoOfTeacher;
            //cmd.Parameters.Add("@Schoolinfo_NoOfStudent", SqlDbType.NVarChar, -1).Value = Schoolinfo_NoOfStudent;
            cmd.Parameters.Add("@Floor", SqlDbType.NVarChar, -1).Value = Floor;
            cmd.Parameters.Add("@single_bench", SqlDbType.NVarChar, -1).Value = single_bench;
            cmd.Parameters.Add("@bench2", SqlDbType.NVarChar, -1).Value = bench2;
            cmd.Parameters.Add("@round_table", SqlDbType.NVarChar, -1).Value = round_table;
            cmd.Parameters.Add("@Schoolinfo_Mealtime", SqlDbType.NVarChar, -1).Value = Schoolinfo_Mealtime;
            //cmd.Parameters.Add("@Schoolinfo_Mealtime", SqlDbType.NVarChar, -1).Value = Schoolinfo_Mealtime;
            cmd.Parameters.Add("@Schoolinfo_MealType", SqlDbType.NVarChar, -1).Value = Schoolinfo_MealType;
            cmd.Parameters.Add("@Schoolinfo_Shareing", SqlDbType.NVarChar, -1).Value = Schoolinfo_Shareing;
            cmd.Parameters.Add("@Schoolinfo_HelpEating", SqlDbType.NVarChar, -1).Value = Schoolinfo_HelpEating;
            cmd.Parameters.Add("@Schoolinfo_Friendship", SqlDbType.NVarChar, -1).Value = Schoolinfo_Friendship;
            cmd.Parameters.Add("@Schoolinfo_InteractionPeer", SqlDbType.NVarChar, -1).Value = Schoolinfo_InteractionPeer;
            cmd.Parameters.Add("@Schoolinfo_InteractionTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_InteractionTeacher;
            cmd.Parameters.Add("@Schoolinfo_AnnualFunction", SqlDbType.NVarChar, -1).Value = Schoolinfo_AnnualFunction;
            cmd.Parameters.Add("@Schoolinfo_Sports", SqlDbType.NVarChar, -1).Value = Schoolinfo_Sports;
            cmd.Parameters.Add("@Schoolinfo_Picnic", SqlDbType.NVarChar, -1).Value = Schoolinfo_Picnic;
            cmd.Parameters.Add("@Schoolinfo_ExtraCaricular", SqlDbType.NVarChar, -1).Value = Schoolinfo_ExtraCaricular;
            cmd.Parameters.Add("@Schoolinfo_CopyBoard", SqlDbType.NVarChar, -1).Value = Schoolinfo_CopyBoard;
            cmd.Parameters.Add("@Schoolinfo_Instructions", SqlDbType.NVarChar, -1).Value = Schoolinfo_Instructions;
            cmd.Parameters.Add("@Schoolinfo_ShadowTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_ShadowTeacher;
            cmd.Parameters.Add("@Schoolinfo_CW_HW", SqlDbType.NVarChar, -1).Value = Schoolinfo_CW_HW;
            cmd.Parameters.Add("@Schoolinfo_SpecialEducator", SqlDbType.NVarChar, -1).Value = Schoolinfo_SpecialEducator;
            cmd.Parameters.Add("@Schoolinfo_DeliveryInformation", SqlDbType.NVarChar, -1).Value = Schoolinfo_DeliveryInformation;
            cmd.Parameters.Add("@Schoolinfo_RemarkTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_RemarkTeacher;
            cmd.Parameters.Add("@SCHOOL_cmt", SqlDbType.NVarChar, -1).Value = SCHOOL_cmt;

            cmd.Parameters.Add("@PersonalSocial_CurrentPlace", SqlDbType.NVarChar, -1).Value = PersonalSocial_CurrentPlace;
            cmd.Parameters.Add("@PersonalSocial_WhatHeDoes", SqlDbType.NVarChar, -1).Value = PersonalSocial_WhatHeDoes;
            cmd.Parameters.Add("@PersonalSocial_BodyAwareness", SqlDbType.NVarChar, -1).Value = PersonalSocial_BodyAwareness;
            cmd.Parameters.Add("@PersonalSocial_BodySchema", SqlDbType.NVarChar, -1).Value = PersonalSocial_BodySchema;
            cmd.Parameters.Add("@PersonalSocial_ExploreEnvironment", SqlDbType.NVarChar, -1).Value = PersonalSocial_ExploreEnvironment;
            cmd.Parameters.Add("@PersonalSocial_Motivated", SqlDbType.NVarChar, -1).Value = PersonalSocial_Motivated;
            cmd.Parameters.Add("@PersonalSocial_EyeContact", SqlDbType.NVarChar, -1).Value = PersonalSocial_EyeContact;
            cmd.Parameters.Add("@PersonalSocial_SocialSmile", SqlDbType.NVarChar, -1).Value = PersonalSocial_SocialSmile;
            cmd.Parameters.Add("@PersonalSocial_FamilyRegards", SqlDbType.NVarChar, -1).Value = PersonalSocial_FamilyRegards;
            //cmd.Parameters.Add("@PersonalSocial_RateChild", SqlDbType.NVarChar, -1).Value = PersonalSocial_RateChild;
            cmd.Parameters.Add("@PersonalSocial_ChildSocially", SqlDbType.NVarChar, -1).Value = PersonalSocial_ChildSocially;
            cmd.Parameters.Add("@PERSONAL_cmt", SqlDbType.NVarChar, -1).Value = PERSONAL_cmt;


            cmd.Parameters.Add("@SpeechLanguage_StartSpeek", SqlDbType.NVarChar, -1).Value = SpeechLanguage_StartSpeek;
            cmd.Parameters.Add("@SpeechLanguage_Monosyllables", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Monosyllables;
            cmd.Parameters.Add("@SpeechLanguage_Bisyllables", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Bisyllables;
            cmd.Parameters.Add("@SpeechLanguage_ShrotScentences", SqlDbType.NVarChar, -1).Value = SpeechLanguage_ShrotScentences;
            cmd.Parameters.Add("@SpeechLanguage_LongScentences", SqlDbType.NVarChar, -1).Value = SpeechLanguage_LongScentences;
            cmd.Parameters.Add("@SpeechLanguage_UnusualSoundsJargonSpeech", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnusualSoundsJargonSpeech;
            cmd.Parameters.Add("@SpeechLanguage_speechgestures", SqlDbType.NVarChar, -1).Value = SpeechLanguage_speechgestures;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialExpression", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialExpression;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialEyeContact", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialEyeContact;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialGestures", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialGestures;
            cmd.Parameters.Add("@SpeechLanguage_SimpleComplex", SqlDbType.NVarChar, -1).Value = SpeechLanguage_SimpleComplex;
            cmd.Parameters.Add("@SpeechLanguage_UnderstandImpliedMeaning", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnderstandImpliedMeaning;
            cmd.Parameters.Add("@SpeechLanguage_UnderstandJokesarcasm", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnderstandJokesarcasm;
            cmd.Parameters.Add("@SpeechLanguage_Respondstoname", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Respondstoname;
            cmd.Parameters.Add("@SpeechLanguage_TwowayInteraction", SqlDbType.NVarChar, -1).Value = SpeechLanguage_TwowayInteraction;
            cmd.Parameters.Add("@SpeechLanguage_NarrateIncidentsAtSchool", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NarrateIncidentsAtSchool;
            cmd.Parameters.Add("@SpeechLanguage_NarrateIncidentsAtHome", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NarrateIncidentsAtHome;
            //Behaviour_situationalmeltdownscmd.Parameters.Add("@SpeechLanguage_Want", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Want;
            cmd.Parameters.Add("@SpeechLanguage_Needs", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Needs;
            cmd.Parameters.Add("@SpeechLanguage_Emotions", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Emotions;
            cmd.Parameters.Add("@SpeechLanguage_AchievementsFailure", SqlDbType.NVarChar, -1).Value = SpeechLanguage_AchievementsFailure;
            //cmd.Parameters.Add("@SpeechLanguage_LanguageSpoken", SqlDbType.NVarChar, -1).Value = SpeechLanguage_LanguageSpoken;
            cmd.Parameters.Add("@SpeechLanguage_Echolalia", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Echolalia;
            //cmd.Parameters.Add("@SpeechLanguage_Emotionalmilestones", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Emotionalmilestones;
            cmd.Parameters.Add("@Speech_cmt", SqlDbType.NVarChar, -1).Value = Speech_cmt;

            cmd.Parameters.Add("@Behaviour_FreeTime", SqlDbType.NVarChar, -1).Value = Behaviour_FreeTime;
            cmd.Parameters.Add("@unassociated", SqlDbType.NVarChar, -1).Value = unassociated;
            cmd.Parameters.Add("@solitary", SqlDbType.NVarChar, -1).Value = solitary;
            cmd.Parameters.Add("@onlooker", SqlDbType.NVarChar, -1).Value = onlooker;
            cmd.Parameters.Add("@parallel", SqlDbType.NVarChar, -1).Value = parallel;
            cmd.Parameters.Add("@associative", SqlDbType.NVarChar, -1).Value = associative;
            cmd.Parameters.Add("@cooperative", SqlDbType.NVarChar, -1).Value = cooperative;
            cmd.Parameters.Add("@Behaviour_situationalmeltdowns", SqlDbType.NVarChar, -1).Value = Behaviour_situationalmeltdowns;
            cmd.Parameters.Add("@BEHAVIOUR_cmt", SqlDbType.NVarChar, -1).Value = BEHAVIOUR_cmt;


            //cmd.Parameters.Add("@Schoolinfo_PlatformInteraction", SqlDbType.NVarChar, -1).Value = Schoolinfo_PlatformInteraction;
            //cmd.Parameters.Add("@Schoolinfo_HourOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_HourOnlineSchool;
            //cmd.Parameters.Add("@Schoolinfo_SitOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_SitOnlineSchool;
            //cmd.Parameters.Add("@Schoolinfo_TeacherInstruction", SqlDbType.NVarChar, -1).Value = Schoolinfo_TeacherInstruction;
            //cmd.Parameters.Add("@Schoolinfo_SetUp", SqlDbType.NVarChar, -1).Value = Schoolinfo_SetUp;
            //cmd.Parameters.Add("@Schoolinfo_BehaviourOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_BehaviourOnlineSchool;
            //cmd.Parameters.Add("@Arousal_Evaluation", SqlDbType.NVarChar, -1).Value = Arousal_Evaluation;
            // cmd.Parameters.Add("@Arousal_GeneralState", SqlDbType.NVarChar, -1).Value = Arousal_GeneralState;
            cmd.Parameters.Add("@rangevalue", SqlDbType.NVarChar, -1).Value = rangevalue;
            cmd.Parameters.Add("@rangevalue2", SqlDbType.NVarChar, -1).Value = rangevalue2;

            cmd.Parameters.Add("@Arousal_Stimuli", SqlDbType.NVarChar, -1).Value = Arousal_Stimuli;
            cmd.Parameters.Add("@Arousal_Transition", SqlDbType.NVarChar, -1).Value = Arousal_Transition;
            //cmd.Parameters.Add("@Arousal_Optimal", SqlDbType.NVarChar, -1).Value = Arousal_Optimal;
            cmd.Parameters.Add("@Arousal_FactorOCD", SqlDbType.NVarChar, -1).Value = Arousal_FactorOCD;
            cmd.Parameters.Add("@Arousal_ClaimingFactor", SqlDbType.NVarChar, -1).Value = Arousal_ClaimingFactor;
            cmd.Parameters.Add("@Arousal_DipsDown", SqlDbType.NVarChar, -1).Value = Arousal_DipsDown;
            cmd.Parameters.Add("@AROUSAL_cmt", SqlDbType.NVarChar, -1).Value = AROUSAL_cmt;

            cmd.Parameters.Add("@Affect_RangeEmotion", SqlDbType.NVarChar, -1).Value = Affect_RangeEmotion;
            cmd.Parameters.Add("@Affect_ExpressEmotion", SqlDbType.NVarChar, -1).Value = Affect_ExpressEmotion;
            cmd.Parameters.Add("@Affect_Environment", SqlDbType.NVarChar, -1).Value = Affect_Environment;
            cmd.Parameters.Add("@Affect_Task", SqlDbType.NVarChar, -1).Value = Affect_Task;
            cmd.Parameters.Add("@Affect_Individual", SqlDbType.NVarChar, -1).Value = Affect_Individual;
            cmd.Parameters.Add("@Affect_ThroughOut", SqlDbType.NVarChar, -1).Value = Affect_ThroughOut;
            cmd.Parameters.Add("@Affect_Charaterising", SqlDbType.NVarChar, -1).Value = Affect_Charaterising;
            cmd.Parameters.Add("@Affect_cmt", SqlDbType.NVarChar, -1).Value = Affect_cmt;

            //cmd.Parameters.Add("@Attention_Span", SqlDbType.NVarChar, -1).Value = Attention_Span;
            //cmd.Parameters.Add("@Attention_FocusHand", SqlDbType.NVarChar, -1).Value = Attention_FocusHand;
            cmd.Parameters.Add("@Attention_AttentionSpan", SqlDbType.NVarChar, -1).Value = Attention_AttentionSpan;
            cmd.Parameters.AddWithValue("@Attention_FocusHandhome", Attention_FocusHandhome);
            cmd.Parameters.AddWithValue("@Attention_FocusHandSchool", Attention_FocusHandSchool);
            cmd.Parameters.Add("@Attention_Dividing", SqlDbType.NVarChar, -1).Value = Attention_Dividing;
            cmd.Parameters.Add("@Attention_ChangeActivities", SqlDbType.NVarChar, -1).Value = Attention_ChangeActivities;
            cmd.Parameters.Add("@Attention_AgeAppropriate", SqlDbType.NVarChar, -1).Value = Attention_AgeAppropriate;
            cmd.Parameters.Add("@Attention_Distractibility", SqlDbType.NVarChar, -1).Value = Attention_Distractibility;
            cmd.Parameters.Add("@Focal_Attention", SqlDbType.NVarChar, -1).Value = Focal_Attention;
            cmd.Parameters.Add("@Joint_Attention", SqlDbType.NVarChar, -1).Value = Joint_Attention;
            cmd.Parameters.Add("@Divided_Attention", SqlDbType.NVarChar, -1).Value = Divided_Attention;
            cmd.Parameters.Add("@Sustained_Attention", SqlDbType.NVarChar, -1).Value = Sustained_Attention;
            cmd.Parameters.Add("@Alternating_Attention", SqlDbType.NVarChar, -1).Value = Alternating_Attention;
            cmd.Parameters.Add("@Attention_move", SqlDbType.NVarChar, -1).Value = Attention_move;
            cmd.Parameters.Add("@ATTENTION_cmt", SqlDbType.NVarChar, -1).Value = ATTENTION_cmt;



            cmd.Parameters.Add("@Action_MotorPlanning", SqlDbType.NVarChar, -1).Value = Action_MotorPlanning;
            cmd.Parameters.Add("@Action_Purposeful", SqlDbType.NVarChar, -1).Value = Action_Purposeful;
            cmd.Parameters.Add("@Action_GoalOriented", SqlDbType.NVarChar, -1).Value = Action_GoalOriented;
            cmd.Parameters.Add("@Action_FeedBackDependent", SqlDbType.NVarChar, -1).Value = Action_FeedBackDependent;
            cmd.Parameters.Add("@Action_Constructive", SqlDbType.NVarChar, -1).Value = Action_Constructive;
            cmd.Parameters.Add("@Action_cmt", SqlDbType.NVarChar, -1).Value = Action_cmt;
            //cmd.Parameters.Add("@Interaction_KnowPeople", SqlDbType.NVarChar, -1).Value = Interaction_KnowPeople;
            //cmd.Parameters.Add("@Interaction_Strangers", SqlDbType.NVarChar, -1).Value = Interaction_Strangers;
            cmd.Parameters.Add("@Interacts", SqlDbType.NVarChar, -1).Value = Interacts;
            cmd.Parameters.Add("@cmtgathering", SqlDbType.NVarChar, -1).Value = cmtgathering;
            cmd.Parameters.Add("@Does_not_initiate", SqlDbType.NVarChar, -1).Value = Does_not_initiate;
            cmd.Parameters.Add("@Sustain", SqlDbType.NVarChar, -1).Value = Sustain;
            cmd.Parameters.Add("@Fight", SqlDbType.NVarChar, -1).Value = Fight;
            cmd.Parameters.Add("@Freeze", SqlDbType.NVarChar, -1).Value = Freeze;
            cmd.Parameters.Add("@Fright", SqlDbType.NVarChar, -1).Value = Fright;

            cmd.Parameters.Add("@Anxious", SqlDbType.NVarChar, -1).Value = Anxious;
            cmd.Parameters.Add("@Comfortable", SqlDbType.NVarChar, -1).Value = Comfortable;
            cmd.Parameters.Add("@Nervous", SqlDbType.NVarChar, -1).Value = Nervous;
            cmd.Parameters.Add("@ANS_response", SqlDbType.NVarChar, -1).Value = ANS_response;
            cmd.Parameters.Add("@OTHERS", SqlDbType.NVarChar, -1).Value = OTHERS;

            cmd.Parameters.Add("@Interaction_SocialQues", SqlDbType.NVarChar, -1).Value = Interaction_SocialQues;
            cmd.Parameters.Add("@Interaction_Happiness", SqlDbType.NVarChar, -1).Value = Interaction_Happiness;
            cmd.Parameters.Add("@Interaction_Sadness", SqlDbType.NVarChar, -1).Value = Interaction_Sadness;
            cmd.Parameters.Add("@Interaction_Surprise", SqlDbType.NVarChar, -1).Value = Interaction_Surprise;
            cmd.Parameters.Add("@Interaction_Shock", SqlDbType.NVarChar, -1).Value = Interaction_Shock;
            cmd.Parameters.Add("@Interaction_Friends", SqlDbType.NVarChar, -1).Value = Interaction_Friends;
            //cmd.Parameters.Add("@Interaction_RelatesPeople", SqlDbType.NVarChar, -1).Value = Interaction_RelatesPeople;
            cmd.Parameters.Add("@Interaction_Enjoy", SqlDbType.NVarChar, -1).Value = Interaction_Enjoy;
            cmd.Parameters.Add("@INTERACTION_cmt", SqlDbType.NVarChar, -1).Value = INTERACTION_cmt;

            cmd.Parameters.Add("@TS_Registration", SqlDbType.NVarChar, -1).Value = TS_Registration;
            cmd.Parameters.Add("@TS_Orientation", SqlDbType.NVarChar, -1).Value = TS_Orientation;
            cmd.Parameters.Add("@TS_Discrimination", SqlDbType.NVarChar, -1).Value = TS_Discrimination;
            cmd.Parameters.Add("@TS_Responsiveness", SqlDbType.NVarChar, -1).Value = TS_Responsiveness;
            cmd.Parameters.Add("@SS_Bodyawareness", SqlDbType.NVarChar, -1).Value = SS_Bodyawareness;
            cmd.Parameters.Add("@SS_Bodyschema", SqlDbType.NVarChar, -1).Value = SS_Bodyschema;
            cmd.Parameters.Add("@SS_Orientation", SqlDbType.NVarChar, -1).Value = SS_Orientation;
            cmd.Parameters.Add("@SS_Posterior", SqlDbType.NVarChar, -1).Value = SS_Posterior;
            cmd.Parameters.Add("@SS_Bilateral", SqlDbType.NVarChar, -1).Value = SS_Bilateral;
            cmd.Parameters.Add("@SS_Balance", SqlDbType.NVarChar, -1).Value = SS_Balance;
            cmd.Parameters.Add("@SS_Dominance", SqlDbType.NVarChar, -1).Value = SS_Dominance;
            cmd.Parameters.Add("@SS_Right", SqlDbType.NVarChar, -1).Value = SS_Right;
            cmd.Parameters.Add("@SS_identifies", SqlDbType.NVarChar, -1).Value = SS_identifies;
            cmd.Parameters.Add("@SS_point", SqlDbType.NVarChar, -1).Value = SS_point;
            cmd.Parameters.Add("@SS_Constantly", SqlDbType.NVarChar, -1).Value = SS_Constantly;
            cmd.Parameters.Add("@SS_clumsy", SqlDbType.NVarChar, -1).Value = SS_clumsy;
            cmd.Parameters.Add("@SS_maneuver", SqlDbType.NVarChar, -1).Value = SS_maneuver;
            cmd.Parameters.Add("@SS_overly", SqlDbType.NVarChar, -1).Value = SS_overly;
            cmd.Parameters.Add("@SS_stand", SqlDbType.NVarChar, -1).Value = SS_stand;
            cmd.Parameters.Add("@SS_indulge", SqlDbType.NVarChar, -1).Value = SS_indulge;
            cmd.Parameters.Add("@SS_textures", SqlDbType.NVarChar, -1).Value = SS_textures;
            cmd.Parameters.Add("@SS_monkey", SqlDbType.NVarChar, -1).Value = SS_monkey;
            cmd.Parameters.Add("@SS_swings", SqlDbType.NVarChar, -1).Value = SS_swings;
            cmd.Parameters.Add("@VM_Registration", SqlDbType.NVarChar, -1).Value = VM_Registration;
            cmd.Parameters.Add("@VM_Orientation", SqlDbType.NVarChar, -1).Value = VM_Orientation;
            cmd.Parameters.Add("@VM_Discrimination", SqlDbType.NVarChar, -1).Value = VM_Discrimination;
            cmd.Parameters.Add("@VM_Responsiveness", SqlDbType.NVarChar, -1).Value = VM_Responsiveness;
            cmd.Parameters.Add("@PS_Registration", SqlDbType.NVarChar, -1).Value = PS_Registration;
            cmd.Parameters.Add("@PS_Gradation", SqlDbType.NVarChar, -1).Value = PS_Gradation;
            cmd.Parameters.Add("@PS_Discrimination", SqlDbType.NVarChar, -1).Value = PS_Discrimination;
            cmd.Parameters.Add("@PS_Responsiveness", SqlDbType.NVarChar, -1).Value = PS_Responsiveness;
            cmd.Parameters.Add("@OM_Registration", SqlDbType.NVarChar, -1).Value = OM_Registration;
            cmd.Parameters.Add("@OM_Orientation", SqlDbType.NVarChar, -1).Value = OM_Orientation;
            cmd.Parameters.Add("@OM_Discrimination", SqlDbType.NVarChar, -1).Value = OM_Discrimination;
            cmd.Parameters.Add("@OM_Responsiveness", SqlDbType.NVarChar, -1).Value = OM_Responsiveness;
            cmd.Parameters.Add("@AS_Auditory", SqlDbType.NVarChar, -1).Value = AS_Auditory;
            cmd.Parameters.Add("@AS_Orientation", SqlDbType.NVarChar, -1).Value = AS_Orientation;
            cmd.Parameters.Add("@AS_Responsiveness", SqlDbType.NVarChar, -1).Value = AS_Responsiveness;
            cmd.Parameters.Add("@AS_discrimination", SqlDbType.NVarChar, -1).Value = AS_discrimination;
            cmd.Parameters.Add("@AS_Background", SqlDbType.NVarChar, -1).Value = AS_Background;
            cmd.Parameters.Add("@AS_localization", SqlDbType.NVarChar, -1).Value = AS_localization;
            cmd.Parameters.Add("@AS_Analysis", SqlDbType.NVarChar, -1).Value = AS_Analysis;
            cmd.Parameters.Add("@AS_sequencing", SqlDbType.NVarChar, -1).Value = AS_sequencing;
            cmd.Parameters.Add("@AS_blending", SqlDbType.NVarChar, -1).Value = AS_blending;
            cmd.Parameters.Add("@VS_Visual", SqlDbType.NVarChar, -1).Value = VS_Visual;
            cmd.Parameters.Add("@VS_Responsiveness", SqlDbType.NVarChar, -1).Value = VS_Responsiveness;
            cmd.Parameters.Add("@VS_scanning", SqlDbType.NVarChar, -1).Value = VS_scanning;
            cmd.Parameters.Add("@VS_constancy", SqlDbType.NVarChar, -1).Value = VS_constancy;
            cmd.Parameters.Add("@VS_memory", SqlDbType.NVarChar, -1).Value = VS_memory;
            cmd.Parameters.Add("@VS_Perception", SqlDbType.NVarChar, -1).Value = VS_Perception;
            cmd.Parameters.Add("@VS_hand", SqlDbType.NVarChar, -1).Value = VS_hand;
            cmd.Parameters.Add("@VS_foot", SqlDbType.NVarChar, -1).Value = VS_foot;
            cmd.Parameters.Add("@VS_discrimination", SqlDbType.NVarChar, -1).Value = VS_discrimination;
            cmd.Parameters.Add("@VS_closure", SqlDbType.NVarChar, -1).Value = VS_closure;
            cmd.Parameters.Add("@VS_Figureground", SqlDbType.NVarChar, -1).Value = VS_Figureground;
            cmd.Parameters.Add("@VS_Visualmemory", SqlDbType.NVarChar, -1).Value = VS_Visualmemory;
            cmd.Parameters.Add("@VS_sequential", SqlDbType.NVarChar, -1).Value = VS_sequential;
            cmd.Parameters.Add("@VS_spatial", SqlDbType.NVarChar, -1).Value = VS_spatial;
            cmd.Parameters.Add("@OS_Registration", SqlDbType.NVarChar, -1).Value = OS_Registration;
            cmd.Parameters.Add("@OS_Orientation", SqlDbType.NVarChar, -1).Value = OS_Orientation;
            cmd.Parameters.Add("@OS_Discrimination", SqlDbType.NVarChar, -1).Value = OS_Discrimination;
            cmd.Parameters.Add("@OS_Responsiveness", SqlDbType.NVarChar, -1).Value = OS_Responsiveness;

            cmd.Parameters.Add("@TestMeassures_GrossMotor", SqlDbType.NVarChar, -1).Value = TestMeassures_GrossMotor;
            cmd.Parameters.Add("@TestMeassures_FineMotor", SqlDbType.NVarChar, -1).Value = TestMeassures_FineMotor;
            cmd.Parameters.Add("@TestMeassures_DenverLanguage", SqlDbType.NVarChar, -1).Value = TestMeassures_DenverLanguage;
            cmd.Parameters.Add("@TestMeassures_DenverPersonal", SqlDbType.NVarChar, -1).Value = TestMeassures_DenverPersonal;
            cmd.Parameters.Add("@Tests_cmt", SqlDbType.NVarChar, -1).Value = Tests_cmt;


            cmd.Parameters.Add("@score_Communication_2", SqlDbType.NVarChar, -1).Value = score_Communication_2;
            cmd.Parameters.Add("@Inter_Communication_2", SqlDbType.NVarChar, -1).Value = Inter_Communication_2;
            cmd.Parameters.Add("@GROSS_2", SqlDbType.NVarChar, -1).Value = GROSS_2;
            cmd.Parameters.Add("@inter_Gross_2", SqlDbType.NVarChar, -1).Value = inter_Gross_2;
            cmd.Parameters.Add("@FINE_2", SqlDbType.NVarChar, -1).Value = FINE_2;
            cmd.Parameters.Add("@inter_FINE_2", SqlDbType.NVarChar, -1).Value = inter_FINE_2;
            cmd.Parameters.Add("@PROBLEM_2", SqlDbType.NVarChar, -1).Value = PROBLEM_2;
            cmd.Parameters.Add("@inter_PROBLEM_2", SqlDbType.NVarChar, -1).Value = inter_PROBLEM_2;
            cmd.Parameters.Add("@PERSONAL_2", SqlDbType.NVarChar, -1).Value = PERSONAL_2;
            cmd.Parameters.Add("@inter_PERSONAL_2", SqlDbType.NVarChar, -1).Value = inter_PERSONAL_2;

            //cmd.Parameters.Add("@score_Communication_2months", SqlDbType.NVarChar, -1).Value = score_Communication_2months;
            //cmd.Parameters.Add("@Inter_Communication_2months", SqlDbType.NVarChar, -1).Value = Inter_Communication_2months;
            //cmd.Parameters.Add("@GROSS_2months", SqlDbType.NVarChar, -1).Value = GROSS_2months;
            //cmd.Parameters.Add("@inter_Gross_2months", SqlDbType.NVarChar, -1).Value = inter_Gross_2months;
            //cmd.Parameters.Add("@FINE_2months", SqlDbType.NVarChar, -1).Value = FINE_2months;
            //cmd.Parameters.Add("@inter_FINE_2months", SqlDbType.NVarChar, -1).Value = inter_FINE_2months;
            //cmd.Parameters.Add("@PROBLEM_2months", SqlDbType.NVarChar, -1).Value = PROBLEM_2months;
            //cmd.Parameters.Add("@inter_PROBLEM_2moths", SqlDbType.NVarChar, -1).Value = inter_PROBLEM_2moths;
            //cmd.Parameters.Add("@PERSONAL_2months", SqlDbType.NVarChar, -1).Value = PERSONAL_2months;
            //cmd.Parameters.Add("@inter_PERSONAL_2months", SqlDbType.NVarChar, -1).Value = inter_PERSONAL_2months;

            cmd.Parameters.Add("@Comm_3", SqlDbType.NVarChar, -1).Value = Comm_3;
            cmd.Parameters.Add("@inter_3", SqlDbType.NVarChar, -1).Value = inter_3;
            cmd.Parameters.Add("@GROSS_3", SqlDbType.NVarChar, -1).Value = GROSS_3;
            cmd.Parameters.Add("@GROSS_inter_3", SqlDbType.NVarChar, -1).Value = GROSS_inter_3;
            cmd.Parameters.Add("@FINE_3", SqlDbType.NVarChar, -1).Value = FINE_3;
            cmd.Parameters.Add("@FINE_inter_3", SqlDbType.NVarChar, -1).Value = FINE_inter_3;
            cmd.Parameters.Add("@PROBLEM_3", SqlDbType.NVarChar, -1).Value = PROBLEM_3;
            cmd.Parameters.Add("@PROBLEM_inter_3", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_3;
            cmd.Parameters.Add("@PERSONAL_3", SqlDbType.NVarChar, -1).Value = PERSONAL_3;
            cmd.Parameters.Add("@PERSONAL_inter_3", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_3;
            cmd.Parameters.Add("@Communication_6", SqlDbType.NVarChar, -1).Value = Communication_6;
            cmd.Parameters.Add("@comm_inter_6", SqlDbType.NVarChar, -1).Value = comm_inter_6;
            cmd.Parameters.Add("@GROSS_6", SqlDbType.NVarChar, -1).Value = GROSS_6;
            cmd.Parameters.Add("@GROSS_inter_6", SqlDbType.NVarChar, -1).Value = GROSS_inter_6;
            cmd.Parameters.Add("@FINE_6", SqlDbType.NVarChar, -1).Value = FINE_6;
            cmd.Parameters.Add("@FINE_inter_6", SqlDbType.NVarChar, -1).Value = FINE_inter_6;
            cmd.Parameters.Add("@PROBLEM_6", SqlDbType.NVarChar, -1).Value = PROBLEM_6;
            cmd.Parameters.Add("@PROBLEM_inter_6", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_6;
            cmd.Parameters.Add("@PERSONAL_6", SqlDbType.NVarChar, -1).Value = PERSONAL_6;
            cmd.Parameters.Add("@PERSONAL_inter_6", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_6;
            cmd.Parameters.Add("@comm_7", SqlDbType.NVarChar, -1).Value = comm_7;
            cmd.Parameters.Add("@inter_7", SqlDbType.NVarChar, -1).Value = inter_7;
            cmd.Parameters.Add("@GROSS_7", SqlDbType.NVarChar, -1).Value = GROSS_7;
            cmd.Parameters.Add("@GROSS_inter_7", SqlDbType.NVarChar, -1).Value = GROSS_inter_7;
            cmd.Parameters.Add("@FINE_7", SqlDbType.NVarChar, -1).Value = FINE_7;
            cmd.Parameters.Add("@FINE_inter_7", SqlDbType.NVarChar, -1).Value = FINE_inter_7;
            cmd.Parameters.Add("@PROBLEM_7", SqlDbType.NVarChar, -1).Value = PROBLEM_7;
            cmd.Parameters.Add("@PROBLEM_inter_7", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_7;
            cmd.Parameters.Add("@PERSONAL_7", SqlDbType.NVarChar, -1).Value = PERSONAL_7;
            cmd.Parameters.Add("@PERSONAL_inter_7", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_7;
            cmd.Parameters.Add("@comm_9", SqlDbType.NVarChar, -1).Value = comm_9;
            cmd.Parameters.Add("@inter_9", SqlDbType.NVarChar, -1).Value = inter_9;
            cmd.Parameters.Add("@GROSS_9", SqlDbType.NVarChar, -1).Value = GROSS_9;
            cmd.Parameters.Add("@GROSS_inter_9", SqlDbType.NVarChar, -1).Value = GROSS_inter_9;
            cmd.Parameters.Add("@FINE_9", SqlDbType.NVarChar, -1).Value = FINE_9;
            cmd.Parameters.Add("@FINE_inter_9", SqlDbType.NVarChar, -1).Value = FINE_inter_9;
            cmd.Parameters.Add("@PROBLEM_9", SqlDbType.NVarChar, -1).Value = PROBLEM_9;
            cmd.Parameters.Add("@PROBLEM_inter_9", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_9;
            cmd.Parameters.Add("@PERSONAL_9", SqlDbType.NVarChar, -1).Value = PERSONAL_9;
            cmd.Parameters.Add("@PERSONAL_inter_9", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_9;
            cmd.Parameters.Add("@comm_10", SqlDbType.NVarChar, -1).Value = comm_10;
            cmd.Parameters.Add("@inter_10", SqlDbType.NVarChar, -1).Value = inter_10;
            cmd.Parameters.Add("@GROSS_10", SqlDbType.NVarChar, -1).Value = GROSS_10;
            cmd.Parameters.Add("@GROSS_inter_10", SqlDbType.NVarChar, -1).Value = GROSS_inter_10;
            cmd.Parameters.Add("@FINE_10", SqlDbType.NVarChar, -1).Value = FINE_10;
            cmd.Parameters.Add("@FINE_inter_10", SqlDbType.NVarChar, -1).Value = FINE_inter_10;
            cmd.Parameters.Add("@PROBLEM_10", SqlDbType.NVarChar, -1).Value = PROBLEM_10;
            cmd.Parameters.Add("@PROBLEM_inter_10", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_10;
            cmd.Parameters.Add("@PERSONAL_10", SqlDbType.NVarChar, -1).Value = PERSONAL_10;
            cmd.Parameters.Add("@PERSONAL_inter_10", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_10;
            cmd.Parameters.Add("@comm_11", SqlDbType.NVarChar, -1).Value = comm_11;
            cmd.Parameters.Add("@inter_11", SqlDbType.NVarChar, -1).Value = inter_11;
            cmd.Parameters.Add("@GROSS_11", SqlDbType.NVarChar, -1).Value = GROSS_11;
            cmd.Parameters.Add("@GROSS_inter_11", SqlDbType.NVarChar, -1).Value = GROSS_inter_11;
            cmd.Parameters.Add("@FINE_11", SqlDbType.NVarChar, -1).Value = FINE_11;
            cmd.Parameters.Add("@FINE_inter_11", SqlDbType.NVarChar, -1).Value = FINE_inter_11;
            cmd.Parameters.Add("@PROBLEM_11", SqlDbType.NVarChar, -1).Value = PROBLEM_11;
            cmd.Parameters.Add("@PROBLEM_inter_11", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_11;
            cmd.Parameters.Add("@PERSONAL_11", SqlDbType.NVarChar, -1).Value = PERSONAL_11;
            cmd.Parameters.Add("@PERSONAL_inter_11", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_11;
            cmd.Parameters.Add("@comm_13", SqlDbType.NVarChar, -1).Value = comm_13;
            cmd.Parameters.Add("@inter_13", SqlDbType.NVarChar, -1).Value = inter_13;
            cmd.Parameters.Add("@GROSS_13", SqlDbType.NVarChar, -1).Value = GROSS_13;
            cmd.Parameters.Add("@GROSS_inter_13", SqlDbType.NVarChar, -1).Value = GROSS_inter_13;
            cmd.Parameters.Add("@FINE_13", SqlDbType.NVarChar, -1).Value = FINE_13;
            cmd.Parameters.Add("@FINE_inter_13", SqlDbType.NVarChar, -1).Value = FINE_inter_13;
            cmd.Parameters.Add("@PROBLEM_13", SqlDbType.NVarChar, -1).Value = PROBLEM_13;
            cmd.Parameters.Add("@PROBLEM_inter_13", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_13;
            cmd.Parameters.Add("@PERSONAL_13", SqlDbType.NVarChar, -1).Value = PERSONAL_13;
            cmd.Parameters.Add("@PERSONAL_inter_13", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_13;
            cmd.Parameters.Add("@comm_15", SqlDbType.NVarChar, -1).Value = comm_15;
            cmd.Parameters.Add("@inter_15", SqlDbType.NVarChar, -1).Value = inter_15;
            cmd.Parameters.Add("@GROSS_15", SqlDbType.NVarChar, -1).Value = GROSS_15;
            cmd.Parameters.Add("@GROSS_inter_15", SqlDbType.NVarChar, -1).Value = GROSS_inter_15;
            cmd.Parameters.Add("@FINE_15", SqlDbType.NVarChar, -1).Value = FINE_15;
            cmd.Parameters.Add("@FINE_inter_15", SqlDbType.NVarChar, -1).Value = FINE_inter_15;
            cmd.Parameters.Add("@PROBLEM_15", SqlDbType.NVarChar, -1).Value = PROBLEM_15;
            cmd.Parameters.Add("@PROBLEM_inter_15", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_15;
            cmd.Parameters.Add("@PERSONAL_15", SqlDbType.NVarChar, -1).Value = PERSONAL_15;
            cmd.Parameters.Add("@PERSONAL_inter_15", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_15;
            cmd.Parameters.Add("@comm_17", SqlDbType.NVarChar, -1).Value = comm_17;
            cmd.Parameters.Add("@inter_17", SqlDbType.NVarChar, -1).Value = inter_17;
            cmd.Parameters.Add("@GROSS_17", SqlDbType.NVarChar, -1).Value = GROSS_17;
            cmd.Parameters.Add("@GROSS_inter_17", SqlDbType.NVarChar, -1).Value = GROSS_inter_17;
            cmd.Parameters.Add("@FINE_17", SqlDbType.NVarChar, -1).Value = FINE_17;
            cmd.Parameters.Add("@FINE_inter_17", SqlDbType.NVarChar, -1).Value = FINE_inter_17;
            cmd.Parameters.Add("@PROBLEM_17", SqlDbType.NVarChar, -1).Value = PROBLEM_17;
            cmd.Parameters.Add("@PROBLEM_inter_17", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_17;
            cmd.Parameters.Add("@PERSONAL_17", SqlDbType.NVarChar, -1).Value = PERSONAL_17;
            cmd.Parameters.Add("@PERSONAL_inter_17", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_17;
            cmd.Parameters.Add("@comm_19", SqlDbType.NVarChar, -1).Value = comm_19;
            cmd.Parameters.Add("@inter_19", SqlDbType.NVarChar, -1).Value = inter_19;
            cmd.Parameters.Add("@GROSS_19", SqlDbType.NVarChar, -1).Value = GROSS_19;
            cmd.Parameters.Add("@GROSS_inter_19", SqlDbType.NVarChar, -1).Value = GROSS_inter_19;
            cmd.Parameters.Add("@FINE_19", SqlDbType.NVarChar, -1).Value = FINE_19;
            cmd.Parameters.Add("@FINE_inter_19", SqlDbType.NVarChar, -1).Value = FINE_inter_19;
            cmd.Parameters.Add("@PROBLEM_19", SqlDbType.NVarChar, -1).Value = PROBLEM_19;
            cmd.Parameters.Add("@PROBLEM_inter_19", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_19;
            cmd.Parameters.Add("@PERSONAL_19", SqlDbType.NVarChar, -1).Value = PERSONAL_19;
            cmd.Parameters.Add("@PERSONAL_inter_19", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_19;
            cmd.Parameters.Add("@comm_21", SqlDbType.NVarChar, -1).Value = comm_21;
            cmd.Parameters.Add("@inter_21", SqlDbType.NVarChar, -1).Value = inter_21;
            cmd.Parameters.Add("@GROSS_21", SqlDbType.NVarChar, -1).Value = GROSS_21;
            cmd.Parameters.Add("@GROSS_inter_21", SqlDbType.NVarChar, -1).Value = GROSS_inter_21;
            cmd.Parameters.Add("@FINE_21", SqlDbType.NVarChar, -1).Value = FINE_21;
            cmd.Parameters.Add("@FINE_inter_21", SqlDbType.NVarChar, -1).Value = FINE_inter_21;
            cmd.Parameters.Add("@PROBLEM_21", SqlDbType.NVarChar, -1).Value = PROBLEM_21;
            cmd.Parameters.Add("@PROBLEM_inter_21", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_21;
            cmd.Parameters.Add("@PERSONAL_21", SqlDbType.NVarChar, -1).Value = PERSONAL_21;
            cmd.Parameters.Add("@PERSONAL_inter_21", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_21;
            cmd.Parameters.Add("@comm_23", SqlDbType.NVarChar, -1).Value = comm_23;
            cmd.Parameters.Add("@inter_23", SqlDbType.NVarChar, -1).Value = inter_23;
            cmd.Parameters.Add("@GROSS_23", SqlDbType.NVarChar, -1).Value = GROSS_23;
            cmd.Parameters.Add("@GROSS_inter_23", SqlDbType.NVarChar, -1).Value = GROSS_inter_23;
            cmd.Parameters.Add("@FINE_23", SqlDbType.NVarChar, -1).Value = FINE_23;
            cmd.Parameters.Add("@FINE_inter_23", SqlDbType.NVarChar, -1).Value = FINE_inter_23;
            cmd.Parameters.Add("@PROBLEM_23", SqlDbType.NVarChar, -1).Value = PROBLEM_23;
            cmd.Parameters.Add("@PROBLEM_inter_23", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_23;
            cmd.Parameters.Add("@PERSONAL_23", SqlDbType.NVarChar, -1).Value = PERSONAL_23;
            cmd.Parameters.Add("@PERSONAL_inter_23", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_23;
            cmd.Parameters.Add("@comm_25", SqlDbType.NVarChar, -1).Value = comm_25;
            cmd.Parameters.Add("@inter_25", SqlDbType.NVarChar, -1).Value = inter_25;
            cmd.Parameters.Add("@GROSS_25", SqlDbType.NVarChar, -1).Value = GROSS_25;
            cmd.Parameters.Add("@GROSS_inter_25", SqlDbType.NVarChar, -1).Value = GROSS_inter_25;
            cmd.Parameters.Add("@FINE_25", SqlDbType.NVarChar, -1).Value = FINE_25;
            cmd.Parameters.Add("@FINE_inter_25", SqlDbType.NVarChar, -1).Value = FINE_inter_25;
            cmd.Parameters.Add("@PROBLEM_25", SqlDbType.NVarChar, -1).Value = PROBLEM_25;
            cmd.Parameters.Add("@PROBLEM_inter_25", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_25;
            cmd.Parameters.Add("@PERSONAL_25", SqlDbType.NVarChar, -1).Value = PERSONAL_25;
            cmd.Parameters.Add("@PERSONAL_inter_25", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_25;
            cmd.Parameters.Add("@comm_28", SqlDbType.NVarChar, -1).Value = comm_28;
            cmd.Parameters.Add("@inter_28", SqlDbType.NVarChar, -1).Value = inter_28;
            cmd.Parameters.Add("@GROSS_28", SqlDbType.NVarChar, -1).Value = GROSS_28;
            cmd.Parameters.Add("@GROSS_inter_28", SqlDbType.NVarChar, -1).Value = GROSS_inter_28;
            cmd.Parameters.Add("@FINE_28", SqlDbType.NVarChar, -1).Value = FINE_28;
            cmd.Parameters.Add("@FINE_inter_28", SqlDbType.NVarChar, -1).Value = FINE_inter_28;
            cmd.Parameters.Add("@PROBLEM_28", SqlDbType.NVarChar, -1).Value = PROBLEM_28;
            cmd.Parameters.Add("@PROBLEM_inter_28", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_28;
            cmd.Parameters.Add("@PERSONAL_28", SqlDbType.NVarChar, -1).Value = PERSONAL_28;
            cmd.Parameters.Add("@PERSONAL_inter_28", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_28;
            cmd.Parameters.Add("@comm_31", SqlDbType.NVarChar, -1).Value = comm_31;
            cmd.Parameters.Add("@inter_31", SqlDbType.NVarChar, -1).Value = inter_31;
            cmd.Parameters.Add("@GROSS_31", SqlDbType.NVarChar, -1).Value = GROSS_31;
            cmd.Parameters.Add("@GROSS_inter_31", SqlDbType.NVarChar, -1).Value = GROSS_inter_31;
            cmd.Parameters.Add("@FINE_31", SqlDbType.NVarChar, -1).Value = FINE_31;
            cmd.Parameters.Add("@FINE_inter_31", SqlDbType.NVarChar, -1).Value = FINE_inter_31;
            cmd.Parameters.Add("@PROBLEM_31", SqlDbType.NVarChar, -1).Value = PROBLEM_31;
            cmd.Parameters.Add("@PROBLEM_inter_31", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_31;
            cmd.Parameters.Add("@PERSONAL_31", SqlDbType.NVarChar, -1).Value = PERSONAL_31;
            cmd.Parameters.Add("@PERSONAL_inter_31", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_31;
            cmd.Parameters.Add("@comm_34", SqlDbType.NVarChar, -1).Value = comm_34;
            cmd.Parameters.Add("@inter_34", SqlDbType.NVarChar, -1).Value = inter_34;
            cmd.Parameters.Add("@GROSS_34", SqlDbType.NVarChar, -1).Value = GROSS_34;
            cmd.Parameters.Add("@GROSS_inter_34", SqlDbType.NVarChar, -1).Value = GROSS_inter_34;
            cmd.Parameters.Add("@FINE_34", SqlDbType.NVarChar, -1).Value = FINE_34;
            cmd.Parameters.Add("@FINE_inter_34", SqlDbType.NVarChar, -1).Value = FINE_inter_34;
            cmd.Parameters.Add("@PROBLEM_34", SqlDbType.NVarChar, -1).Value = PROBLEM_34;
            cmd.Parameters.Add("@PROBLEM_inter_34", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_34;
            cmd.Parameters.Add("@PERSONAL_34", SqlDbType.NVarChar, -1).Value = PERSONAL_34;
            cmd.Parameters.Add("@PERSONAL_inter_34", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_34;
            cmd.Parameters.Add("@comm_42", SqlDbType.NVarChar, -1).Value = comm_42;
            cmd.Parameters.Add("@inter_42", SqlDbType.NVarChar, -1).Value = inter_42;
            cmd.Parameters.Add("@GROSS_42", SqlDbType.NVarChar, -1).Value = GROSS_42;
            cmd.Parameters.Add("@GROSS_inter_42", SqlDbType.NVarChar, -1).Value = GROSS_inter_42;
            cmd.Parameters.Add("@FINE_42", SqlDbType.NVarChar, -1).Value = FINE_42;
            cmd.Parameters.Add("@FINE_inter_42", SqlDbType.NVarChar, -1).Value = FINE_inter_42;
            cmd.Parameters.Add("@PROBLEM_42", SqlDbType.NVarChar, -1).Value = PROBLEM_42;
            cmd.Parameters.Add("@PROBLEM_inter_42", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_42;
            cmd.Parameters.Add("@PERSONAL_42", SqlDbType.NVarChar, -1).Value = PERSONAL_42;
            cmd.Parameters.Add("@PERSONAL_inter_42", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_42;
            cmd.Parameters.Add("@comm_45", SqlDbType.NVarChar, -1).Value = comm_45;
            cmd.Parameters.Add("@inter_45", SqlDbType.NVarChar, -1).Value = inter_45;
            cmd.Parameters.Add("@GROSS_45", SqlDbType.NVarChar, -1).Value = GROSS_45;
            cmd.Parameters.Add("@GROSS_inter_45", SqlDbType.NVarChar, -1).Value = GROSS_inter_45;
            cmd.Parameters.Add("@FINE_45", SqlDbType.NVarChar, -1).Value = FINE_45;
            cmd.Parameters.Add("@FINE_inter_45", SqlDbType.NVarChar, -1).Value = FINE_inter_45;
            cmd.Parameters.Add("@PROBLEM_45", SqlDbType.NVarChar, -1).Value = PROBLEM_45;
            cmd.Parameters.Add("@PROBLEM_inter_45", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_45;
            cmd.Parameters.Add("@PERSONAL_45", SqlDbType.NVarChar, -1).Value = PERSONAL_45;
            cmd.Parameters.Add("@PERSONAL_inter_45", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_45;
            cmd.Parameters.Add("@comm_51", SqlDbType.NVarChar, -1).Value = comm_51;
            cmd.Parameters.Add("@inter_51", SqlDbType.NVarChar, -1).Value = inter_51;
            cmd.Parameters.Add("@GROSS_51", SqlDbType.NVarChar, -1).Value = GROSS_51;
            cmd.Parameters.Add("@GROSS_inter_51", SqlDbType.NVarChar, -1).Value = GROSS_inter_51;
            cmd.Parameters.Add("@FINE_51", SqlDbType.NVarChar, -1).Value = FINE_51;
            cmd.Parameters.Add("@FINE_inter_51", SqlDbType.NVarChar, -1).Value = FINE_inter_51;
            cmd.Parameters.Add("@PROBLEM_51", SqlDbType.NVarChar, -1).Value = PROBLEM_51;
            cmd.Parameters.Add("@PROBLEM_inter_51", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_51;
            cmd.Parameters.Add("@PERSONAL_51", SqlDbType.NVarChar, -1).Value = PERSONAL_51;
            cmd.Parameters.Add("@PERSONAL_inter_51", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_51;
            cmd.Parameters.Add("@comm_60", SqlDbType.NVarChar, -1).Value = comm_60;
            cmd.Parameters.Add("@inter_60", SqlDbType.NVarChar, -1).Value = inter_60;
            cmd.Parameters.Add("@GROSS_60", SqlDbType.NVarChar, -1).Value = GROSS_60;
            cmd.Parameters.Add("@GROSS_inter_60", SqlDbType.NVarChar, -1).Value = GROSS_inter_60;
            cmd.Parameters.Add("@FINE_60", SqlDbType.NVarChar, -1).Value = FINE_60;
            cmd.Parameters.Add("@FINE_inter_60", SqlDbType.NVarChar, -1).Value = FINE_inter_60;
            cmd.Parameters.Add("@PROBLEM_60", SqlDbType.NVarChar, -1).Value = PROBLEM_60;
            cmd.Parameters.Add("@PROBLEM_inter_60", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_60;
            cmd.Parameters.Add("@PERSONAL_60", SqlDbType.NVarChar, -1).Value = PERSONAL_60;
            cmd.Parameters.Add("@PERSONAL_inter_60", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_60;

            cmd.Parameters.Add("@MONTHS", SqlDbType.NVarChar, -1).Value = MONTHS;
            cmd.Parameters.Add("@QUESTIONS", SqlDbType.NVarChar, -1).Value = QUESTIONS;

            cmd.Parameters.Add("@General_Processing", SqlDbType.NVarChar, -1).Value = General_Processing;
            cmd.Parameters.Add("@AUDITORY_Processing", SqlDbType.NVarChar, -1).Value = AUDITORY_Processing;
            cmd.Parameters.Add("@VISUAL_Processing", SqlDbType.NVarChar, -1).Value = VISUAL_Processing;
            cmd.Parameters.Add("@TOUCH_Processing", SqlDbType.NVarChar, -1).Value = TOUCH_Processing;
            cmd.Parameters.Add("@MOVEMENT_Processing", SqlDbType.NVarChar, -1).Value = MOVEMENT_Processing;
            cmd.Parameters.Add("@ORAL_Processing", SqlDbType.NVarChar, -1).Value = ORAL_Processing;
            cmd.Parameters.Add("@Raw_score", SqlDbType.NVarChar, -1).Value = Raw_score;
            //cmd.Parameters.Add("@Percentile_Range", SqlDbType.NVarChar, -1).Value = Percentile_Range;
            cmd.Parameters.Add("@Total_rawscore", SqlDbType.NVarChar, -1).Value = Total_rawscore;
            cmd.Parameters.Add("@Interpretation", SqlDbType.NVarChar, -1).Value = Interpretation;
            cmd.Parameters.Add("@Comments_1", SqlDbType.NVarChar, -1).Value = Comments_1;

            cmd.Parameters.Add("@Score_seeking", SqlDbType.NVarChar, -1).Value = Score_seeking;
            cmd.Parameters.Add("@Score_Avoiding", SqlDbType.NVarChar, -1).Value = Score_Avoiding;
            cmd.Parameters.Add("@Score_sensitivity", SqlDbType.NVarChar, -1).Value = Score_sensitivity;
            cmd.Parameters.Add("@Score_Registration", SqlDbType.NVarChar, -1).Value = Score_Registration;
            cmd.Parameters.Add("@Score_general", SqlDbType.NVarChar, -1).Value = Score_general;
            cmd.Parameters.Add("@Score_Auditory", SqlDbType.NVarChar, -1).Value = Score_Auditory;
            cmd.Parameters.Add("@Score_visual", SqlDbType.NVarChar, -1).Value = Score_visual;
            cmd.Parameters.Add("@Score_touch", SqlDbType.NVarChar, -1).Value = Score_touch;
            cmd.Parameters.Add("@Score_movement", SqlDbType.NVarChar, -1).Value = Score_movement;
            cmd.Parameters.Add("@Score_oral", SqlDbType.NVarChar, -1).Value = Score_oral;
            cmd.Parameters.Add("@Score_behavioural", SqlDbType.NVarChar, -1).Value = Score_behavioural;
            cmd.Parameters.Add("@SEEKING", SqlDbType.NVarChar, -1).Value = SEEKING;
            cmd.Parameters.Add("@AVOIDING", SqlDbType.NVarChar, -1).Value = AVOIDING;
            cmd.Parameters.Add("@SENSITIVITY_2", SqlDbType.NVarChar, -1).Value = SENSITIVITY_2;
            cmd.Parameters.Add("@REGISTRATION", SqlDbType.NVarChar, -1).Value = REGISTRATION;
            cmd.Parameters.Add("@GENERAL", SqlDbType.NVarChar, -1).Value = GENERAL;
            cmd.Parameters.Add("@AUDITORY", SqlDbType.NVarChar, -1).Value = AUDITORY;
            cmd.Parameters.Add("@VISUAL", SqlDbType.NVarChar, -1).Value = VISUAL;
            cmd.Parameters.Add("@TOUCH", SqlDbType.NVarChar, -1).Value = TOUCH;
            cmd.Parameters.Add("@MOVEMENT", SqlDbType.NVarChar, -1).Value = MOVEMENT;
            cmd.Parameters.Add("@ORAL", SqlDbType.NVarChar, -1).Value = ORAL;
            cmd.Parameters.Add("@BEHAVIORAL", SqlDbType.NVarChar, -1).Value = BEHAVIORAL;
            cmd.Parameters.Add("@Comments_2", SqlDbType.NVarChar, -1).Value = Comments_2;

            cmd.Parameters.Add("@SPchild_Seeker", SqlDbType.NVarChar, -1).Value = SPchild_Seeker;
            cmd.Parameters.Add("@SPchild_Avoider", SqlDbType.NVarChar, -1).Value = SPchild_Avoider;
            cmd.Parameters.Add("@SPchild_Sensor", SqlDbType.NVarChar, -1).Value = SPchild_Sensor;
            cmd.Parameters.Add("@SPchild_Bystander", SqlDbType.NVarChar, -1).Value = SPchild_Bystander;
            cmd.Parameters.Add("@SPchild_Auditory_3", SqlDbType.NVarChar, -1).Value = SPchild_Auditory_3;
            cmd.Parameters.Add("@SPchild_Visual_3", SqlDbType.NVarChar, -1).Value = SPchild_Visual_3;
            cmd.Parameters.Add("@SPchild_Touch_3", SqlDbType.NVarChar, -1).Value = SPchild_Touch_3;
            cmd.Parameters.Add("@SPchild_Movement_3", SqlDbType.NVarChar, -1).Value = SPchild_Movement_3;
            cmd.Parameters.Add("@SPchild_Body_position", SqlDbType.NVarChar, -1).Value = SPchild_Body_position;
            cmd.Parameters.Add("@SPchild_Oral_3", SqlDbType.NVarChar, -1).Value = SPchild_Oral_3;
            cmd.Parameters.Add("@SPchild_Conduct_3", SqlDbType.NVarChar, -1).Value = SPchild_Conduct_3;
            cmd.Parameters.Add("@SPchild_Social_emotional", SqlDbType.NVarChar, -1).Value = SPchild_Social_emotional;
            cmd.Parameters.Add("@SPchild_Attentional_3", SqlDbType.NVarChar, -1).Value = SPchild_Attentional_3;
            cmd.Parameters.Add("@Seeking_Seeker", SqlDbType.NVarChar, -1).Value = Seeking_Seeker;
            cmd.Parameters.Add("@Avoiding_Avoider", SqlDbType.NVarChar, -1).Value = Avoiding_Avoider;
            cmd.Parameters.Add("@Sensitivity_Sensor", SqlDbType.NVarChar, -1).Value = Sensitivity_Sensor;
            cmd.Parameters.Add("@Registration_Bystander", SqlDbType.NVarChar, -1).Value = Registration_Bystander;
            cmd.Parameters.Add("@Auditory_3", SqlDbType.NVarChar, -1).Value = Auditory_3;
            cmd.Parameters.Add("@Visual_3", SqlDbType.NVarChar, -1).Value = Visual_3;
            cmd.Parameters.Add("@Touch_3", SqlDbType.NVarChar, -1).Value = Touch_3;
            cmd.Parameters.Add("@Movement_3", SqlDbType.NVarChar, -1).Value = Movement_3;
            cmd.Parameters.Add("@Body_position", SqlDbType.NVarChar, -1).Value = Body_position;
            cmd.Parameters.Add("@Oral_3", SqlDbType.NVarChar, -1).Value = Oral_3;
            cmd.Parameters.Add("@Conduct_3", SqlDbType.NVarChar, -1).Value = Conduct_3;
            cmd.Parameters.Add("@Social_emotional", SqlDbType.NVarChar, -1).Value = Social_emotional;
            cmd.Parameters.Add("@Attentional_3", SqlDbType.NVarChar, -1).Value = Attentional_3;
            cmd.Parameters.Add("@Comments_3", SqlDbType.NVarChar, -1).Value = Comments_3;
            cmd.Parameters.Add("@SPAdult_Low_Registration", SqlDbType.NVarChar, -1).Value = SPAdult_Low_Registration;
            cmd.Parameters.Add("@SPAdult_Sensory_seeking", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_seeking;
            cmd.Parameters.Add("@SPAdult_Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_Sensitivity;
            cmd.Parameters.Add("@SPAdult_Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_Avoiding;
            cmd.Parameters.Add("@Low_Registration", SqlDbType.NVarChar, -1).Value = Low_Registration;
            cmd.Parameters.Add("@Sensory_seeking", SqlDbType.NVarChar, -1).Value = Sensory_seeking;
            cmd.Parameters.Add("@Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity;
            cmd.Parameters.Add("@Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding;
            cmd.Parameters.Add("@Comments_4", SqlDbType.NVarChar, -1).Value = Comments_4;
            cmd.Parameters.Add("@SP_Low_Registration64", SqlDbType.NVarChar, -1).Value = SP_Low_Registration64;
            cmd.Parameters.Add("@SP_Sensory_seeking_64", SqlDbType.NVarChar, -1).Value = SP_Sensory_seeking_64;
            cmd.Parameters.Add("@SP_Sensory_Sensitivity64", SqlDbType.NVarChar, -1).Value = SP_Sensory_Sensitivity64;
            cmd.Parameters.Add("@SP_Sensory_Avoiding64", SqlDbType.NVarChar, -1).Value = SP_Sensory_Avoiding64;
            cmd.Parameters.Add("@Low_Registration_5", SqlDbType.NVarChar, -1).Value = Low_Registration_5;
            cmd.Parameters.Add("@Sensory_seeking_5", SqlDbType.NVarChar, -1).Value = Sensory_seeking_5;
            cmd.Parameters.Add("@Sensory_Sensitivity_5", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity_5;
            cmd.Parameters.Add("@Sensory_Avoiding_5", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding_5;
            cmd.Parameters.Add("@Comments_5", SqlDbType.NVarChar, -1).Value = Comments_5;
            cmd.Parameters.Add("@Older_Low_Registration", SqlDbType.NVarChar, -1).Value = Older_Low_Registration;
            cmd.Parameters.Add("@Older_Sensory_seeking", SqlDbType.NVarChar, -1).Value = Older_Sensory_seeking;
            cmd.Parameters.Add("@Older_Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = Older_Sensory_Sensitivity;
            cmd.Parameters.Add("@Older_Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = Older_Sensory_Avoiding;
            cmd.Parameters.Add("@Low_Registration_6", SqlDbType.NVarChar, -1).Value = Low_Registration_6;
            cmd.Parameters.Add("@Sensory_seeking_6", SqlDbType.NVarChar, -1).Value = Sensory_seeking_6;
            cmd.Parameters.Add("@Sensory_Sensitivity_6", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity_6;
            cmd.Parameters.Add("@Sensory_Avoiding_6", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding_6;
            cmd.Parameters.Add("@Comments_6", SqlDbType.NVarChar, -1).Value = Comments_6;


            cmd.Parameters.Add("@ABILITY_months", SqlDbType.NVarChar, -1).Value = ABILITY_months;
            cmd.Parameters.Add("@ABILITY_questions", SqlDbType.NVarChar, -1).Value = ABILITY_questions;
            cmd.Parameters.Add("@ability_TOTAL", SqlDbType.NVarChar, -1).Value = ability_TOTAL;
            cmd.Parameters.Add("@ability_COMMENTS", SqlDbType.NVarChar, -1).Value = ability_COMMENTS;


            cmd.Parameters.Add("@DCDQ_Throws1", SqlDbType.NVarChar, -1).Value = DCDQ_Throws1;
            cmd.Parameters.Add("@DCDQ_Throws2", SqlDbType.NVarChar, -1).Value = DCDQ_Throws2;
            cmd.Parameters.Add("@DCDQ_Throws3", SqlDbType.NVarChar, -1).Value = DCDQ_Throws3;
            cmd.Parameters.Add("@DCDQ_Catches1", SqlDbType.NVarChar, -1).Value = DCDQ_Catches1;
            cmd.Parameters.Add("@DCDQ_Catches2", SqlDbType.NVarChar, -1).Value = DCDQ_Catches2;
            cmd.Parameters.Add("@DCDQ_Catches3", SqlDbType.NVarChar, -1).Value = DCDQ_Catches3;
            cmd.Parameters.Add("@DCDQ_Hits1", SqlDbType.NVarChar, -1).Value = DCDQ_Hits1;
            cmd.Parameters.Add("@DCDQ_Hits2", SqlDbType.NVarChar, -1).Value = DCDQ_Hits2;
            cmd.Parameters.Add("@DCDQ_Hits3", SqlDbType.NVarChar, -1).Value = DCDQ_Hits3;
            cmd.Parameters.Add("@DCDQ_Jumps1", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps1;
            cmd.Parameters.Add("@DCDQ_Jumps2", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps2;
            cmd.Parameters.Add("@DCDQ_Jumps3", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps3;
            cmd.Parameters.Add("@DCDQ_Runs1", SqlDbType.NVarChar, -1).Value = DCDQ_Runs1;
            cmd.Parameters.Add("@DCDQ_Runs2", SqlDbType.NVarChar, -1).Value = DCDQ_Runs2;
            cmd.Parameters.Add("@DCDQ_Runs3", SqlDbType.NVarChar, -1).Value = DCDQ_Runs3;
            cmd.Parameters.Add("@DCDQ_Plans1", SqlDbType.NVarChar, -1).Value = DCDQ_Plans1;
            cmd.Parameters.Add("@DCDQ_Plans2", SqlDbType.NVarChar, -1).Value = DCDQ_Plans2;
            cmd.Parameters.Add("@DCDQ_Plans3", SqlDbType.NVarChar, -1).Value = DCDQ_Plans3;
            cmd.Parameters.Add("@DCDQ_Writing1", SqlDbType.NVarChar, -1).Value = DCDQ_Writing1;
            cmd.Parameters.Add("@DCDQ_Writing2", SqlDbType.NVarChar, -1).Value = DCDQ_Writing2;
            cmd.Parameters.Add("@DCDQ_Writing3", SqlDbType.NVarChar, -1).Value = DCDQ_Writing3;
            cmd.Parameters.Add("@DCDQ_legibly1", SqlDbType.NVarChar, -1).Value = DCDQ_legibly1;
            cmd.Parameters.Add("@DCDQ_legibly2", SqlDbType.NVarChar, -1).Value = DCDQ_legibly2;
            cmd.Parameters.Add("@DCDQ_legibly3", SqlDbType.NVarChar, -1).Value = DCDQ_legibly3;
            cmd.Parameters.Add("@DCDQ_Effort1", SqlDbType.NVarChar, -1).Value = DCDQ_Effort1;
            cmd.Parameters.Add("@DCDQ_Effort2", SqlDbType.NVarChar, -1).Value = DCDQ_Effort2;
            cmd.Parameters.Add("@DCDQ_Effort3", SqlDbType.NVarChar, -1).Value = DCDQ_Effort3;
            cmd.Parameters.Add("@DCDQ_Cuts1", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts1;
            cmd.Parameters.Add("@DCDQ_Cuts2", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts2;
            cmd.Parameters.Add("@DCDQ_Cuts3", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts3;
            cmd.Parameters.Add("@DCDQ_Likes1", SqlDbType.NVarChar, -1).Value = DCDQ_Likes1;
            cmd.Parameters.Add("@DCDQ_Likes2", SqlDbType.NVarChar, -1).Value = DCDQ_Likes2;
            cmd.Parameters.Add("@DCDQ_Likes3", SqlDbType.NVarChar, -1).Value = DCDQ_Likes3;
            cmd.Parameters.Add("@DCDQ_Learning1", SqlDbType.NVarChar, -1).Value = DCDQ_Learning1;
            cmd.Parameters.Add("@DCDQ_Learning2", SqlDbType.NVarChar, -1).Value = DCDQ_Learning2;
            cmd.Parameters.Add("@DCDQ_Learning3", SqlDbType.NVarChar, -1).Value = DCDQ_Learning3;
            cmd.Parameters.Add("@DCDQ_Quick1", SqlDbType.NVarChar, -1).Value = DCDQ_Quick1;
            cmd.Parameters.Add("@DCDQ_Quick2", SqlDbType.NVarChar, -1).Value = DCDQ_Quick2;
            cmd.Parameters.Add("@DCDQ_Quick3", SqlDbType.NVarChar, -1).Value = DCDQ_Quick3;
            cmd.Parameters.Add("@DCDQ_Bull1", SqlDbType.NVarChar, -1).Value = DCDQ_Bull1;
            cmd.Parameters.Add("@DCDQ_Bull2", SqlDbType.NVarChar, -1).Value = DCDQ_Bull2;
            cmd.Parameters.Add("@DCDQ_Bull3", SqlDbType.NVarChar, -1).Value = DCDQ_Bull3;
            cmd.Parameters.Add("@DCDQ_Does1", SqlDbType.NVarChar, -1).Value = DCDQ_Does1;
            cmd.Parameters.Add("@DCDQ_Does2", SqlDbType.NVarChar, -1).Value = DCDQ_Does2;
            cmd.Parameters.Add("@DCDQ_Does3", SqlDbType.NVarChar, -1).Value = DCDQ_Does3;
            cmd.Parameters.Add("@DCDQ_Control", SqlDbType.NVarChar, -1).Value = DCDQ_Control;
            cmd.Parameters.Add("@DCDQ_Fine", SqlDbType.NVarChar, -1).Value = DCDQ_Fine;
            cmd.Parameters.Add("@DCDQ_General", SqlDbType.NVarChar, -1).Value = DCDQ_General;
            cmd.Parameters.Add("@DCDQ_Total", SqlDbType.NVarChar, -1).Value = DCDQ_Total;
            cmd.Parameters.Add("@DCDQ_INTERPRETATION", SqlDbType.NVarChar, -1).Value = DCDQ_INTERPRETATION;
            cmd.Parameters.Add("@DCDQ_COMMENT", SqlDbType.NVarChar, -1).Value = DCDQ_COMMENT;


            cmd.Parameters.Add("@SIPTInfo_History", SqlDbType.NVarChar, -1).Value = SIPTInfo_History;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GraspRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GraspLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_SphericalRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_SphericalLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_HookRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_HookLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GripRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GripLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFL;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Spontaneous", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT3_Spontaneous;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Command", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT3_Command;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Kinesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Finger", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Finger;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Localisation", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Localisation;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_DoubleTactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Tactile", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Tactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Graphesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_PostRotary", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_PostRotary;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Standing", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Standing;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Color", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Color;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Form", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Form;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Size", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Size;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Depth", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Depth;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Figure", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Figure;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Motor", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Motor;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Design", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT6_Design;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Constructional", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT6_Constructional;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Scanning", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT7_Scanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Memory", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT7_Memory;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Postural", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Postural;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Oral", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Oral;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Sequencing", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Sequencing;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Commands", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Commands;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Bilateral", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_Bilateral;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Contralat", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_Contralat;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_PreferredHand", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_PreferredHand;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_CrossingMidline;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Draw", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Draw;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_ClockFace", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_ClockFace;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Filtering", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Filtering;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_MotorPlanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodyImage", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_BodyImage;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodySchema", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_BodySchema;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Laterality", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Laterality;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Remark", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Remark;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_InterestActivity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_InterestCompletion;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Learning", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Learning;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Complexity", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Complexity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_ProblemSolving;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Concentration", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Concentration;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Retension", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Retension;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SpeedPerfom;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Neatness", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Neatness;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Frustation", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Frustation;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Work", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Work;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Reaction", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Reaction;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityTherapist;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityStudents;


            cmd.Parameters.Add("@Evaluation_Strengths", SqlDbType.NVarChar, -1).Value = Evaluation_Strengths;
            cmd.Parameters.Add("@Evaluation_Concern_Barriers", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Barriers;
            cmd.Parameters.Add("@Evaluation_Concern_Limitations", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Limitations;
            cmd.Parameters.Add("@Evaluation_Concern_Posture", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Posture;
            cmd.Parameters.Add("@Evaluation_Concern_Impairment", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Impairment;
            cmd.Parameters.Add("@Evaluation_Goal_Summary", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Summary;
            cmd.Parameters.Add("@Evaluation_Goal_Previous", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Previous;
            cmd.Parameters.Add("@Evaluation_Goal_LongTerm", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_LongTerm;
            cmd.Parameters.Add("@Evaluation_Goal_ShortTerm", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_ShortTerm;
            cmd.Parameters.Add("@Evaluation_Goal_Impairment", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Impairment;
            cmd.Parameters.Add("@Evaluation_Plan_Frequency", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Frequency;
            cmd.Parameters.Add("@Evaluation_Plan_Service", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Service;
            cmd.Parameters.Add("@Evaluation_Plan_Strategies", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Strategies;
            cmd.Parameters.Add("@Evaluation_Plan_Equipment", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Equipment;
            cmd.Parameters.Add("@Evaluation_Plan_Education", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Education;

            //cmd.Parameters.Add("@TestMeassures_IQ", SqlDbType.NVarChar, -1).Value = TestMeassures_IQ;
            //cmd.Parameters.Add("@TestMeassures_DQ", SqlDbType.NVarChar, -1).Value = TestMeassures_DQ;

            //cmd.Parameters.Add("@TestMeassures_ASQ", SqlDbType.NVarChar, -1).Value = TestMeassures_ASQ;
            //cmd.Parameters.Add("@TestMeassures_HandWriting", SqlDbType.NVarChar, -1).Value = TestMeassures_HandWriting;
            //cmd.Parameters.Add("@TestMeassures_SIPT", SqlDbType.NVarChar, -1).Value = TestMeassures_SIPT;
            //cmd.Parameters.Add("@TestMeassures_SensoryProfile", SqlDbType.NVarChar, -1).Value = TestMeassures_SensoryProfile;
            cmd.Parameters.Add("@Treatment_Home", SqlDbType.NVarChar, -1).Value = Treatment_Home;
            cmd.Parameters.Add("@Treatment_School", SqlDbType.NVarChar, -1).Value = Treatment_School;
            cmd.Parameters.Add("@Treatment_Threapy", SqlDbType.NVarChar, -1).Value = Treatment_Threapy;
            cmd.Parameters.Add("@Treatment_cmt", SqlDbType.NVarChar, -1).Value = Treatment_cmt;
            //cmd.Parameters.Add("@Daily_cmt", SqlDbType.NVarChar, -1).Value = Daily_cmt;
            //cmd.Parameters.Add("@Self_cmt", SqlDbType.NVarChar, -1).Value = Self_cmt;
            //cmd.Parameters.Add("@abilityQuestionsChild", SqlDbType.NVarChar, -1).Value = abilityQuestionsChild;

            cmd.Parameters.Add("@Doctor_Physioptherapist", SqlDbType.Int).Value = Doctor_Physioptherapist;
            cmd.Parameters.Add("@Doctor_Occupational", SqlDbType.Int).Value = Doctor_Occupational;

            cmd.Parameters.Add("@IsFinal", SqlDbType.Bit).Value = IsFinal;
            cmd.Parameters.Add("@IsGiven", SqlDbType.Bit).Value = IsGiven;
            if (GivenDate > DateTime.MinValue)
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = GivenDate;
            else
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = DBNull.Value;
            //if (ModifyDate > DateTime.MinValue)
            //    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            //else
            //    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            //cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            cmd.Parameters.Add("@DiagnosisID", SqlDbType.NVarChar, -1).Value = DiagnosisID;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.NVarChar, -1).Value = DiagnosisOther;

            db.DbUpdate(cmd);

            int j = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out j);
            }
            return j;
        }

        public DataSet GetsiSubmit(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportSiSubmit"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }
        public int Rpt_SI_SET_SUBMIT(int _appointmentID, string ClinicleObse_txt, DateTime ModifyDate, int ModifyBy,

             //string DailySchedule_Dailyaroutine, string DailySchedule_WakeUpTime, string DailySchedule_RestRoomTime,
             //string DailySchedule_Breakfast, string DailySchedule_BreakFastTime, string DailySchedule_SchoolTime,
             //string DailySchedule_MidMorinig, string DailySchedule_SchoolHours, string DailySchedule_LunchTime, 
             //string DailySchedule_AfternoonRoutine, string DailySchedule_Afternoo_nap, string DailySchedule_DinnerTime, string DailySchedule_PistDinnerAct,

             // string SelfCare_Brushing, string SelfCare_Bathing, string SelfCare_Toileting, string SelfCare_Dressing, string SelfCare_Breakfast,
             //string SelfCare_Lunch, string SelfCare_Snacks, string SelfCare_Dinner, string SelfCare_GettingInBus, string SelfCare_GoingToSchool,
             //string SelfCare_comeBackSchool, string SelfCare_Ambulation, string SelfCare_Homeostaticchanges, string SelfCare_UrinationdetailsBedwetting,

             string FamilyStructure_QualityTimeMother,
            string FamilyStructure_QualityTimeFather, string FamilyStructure_QualityTimeWeekend, string Father_Weekends, string FamilyStructure_TimeForThreapy, string FamilyStructure_AcceptanceCondition,
            string FamilyStructure_ExtraCaricular, string FamilyStructure_Diciplinary, string FamilyStructure_SiblingBrother,
            string FamilyStructure_Expectations, string FamilyStructure_CloselyInvolved, string FAMILY_cmt,

            string Schoolinfo_Attend, string Schoolinfo_Type, string Schoolinfo_SchoolHours, string School_Bus, string Car, string Two_Wheelers, string walking, string Public_Transport, string Schoolinfo_NoOfTeacher,
            /*string Schoolinfo_NoOfStudent,*/ string Floor, string single_bench, string bench2, string round_table, string Schoolinfo_Mealtime, string Schoolinfo_MealType, string Schoolinfo_Shareing,
            string Schoolinfo_HelpEating, string Schoolinfo_Friendship, string Schoolinfo_InteractionPeer, string Schoolinfo_InteractionTeacher,
            string Schoolinfo_AnnualFunction, string Schoolinfo_Sports, string Schoolinfo_Picnic, string Schoolinfo_ExtraCaricular, string Schoolinfo_CopyBoard,
            string Schoolinfo_Instructions, string Schoolinfo_ShadowTeacher, string Schoolinfo_CW_HW, string Schoolinfo_SpecialEducator, string Schoolinfo_DeliveryInformation,
            string Schoolinfo_RemarkTeacher, string SCHOOL_cmt,


            string PersonalSocial_CurrentPlace, string PersonalSocial_WhatHeDoes, string PersonalSocial_BodyAwareness, string PersonalSocial_BodySchema,
            string PersonalSocial_ExploreEnvironment, string PersonalSocial_Motivated, string PersonalSocial_EyeContact, string PersonalSocial_SocialSmile,
            string PersonalSocial_FamilyRegards, /*string PersonalSocial_RateChild,*/ string PersonalSocial_ChildSocially, string PERSONAL_cmt,

            string SpeechLanguage_StartSpeek,
            string SpeechLanguage_Monosyllables, string SpeechLanguage_Bisyllables, string SpeechLanguage_ShrotScentences, string SpeechLanguage_LongScentences,
            string SpeechLanguage_UnusualSoundsJargonSpeech, string SpeechLanguage_speechgestures, string SpeechLanguage_NonverbalfacialExpression, string SpeechLanguage_NonverbalfacialEyeContact,
            string SpeechLanguage_NonverbalfacialGestures, string SpeechLanguage_SimpleComplex, string SpeechLanguage_UnderstandImpliedMeaning,
            string SpeechLanguage_UnderstandJokesarcasm, string SpeechLanguage_Respondstoname, string SpeechLanguage_TwowayInteraction, string SpeechLanguage_NarrateIncidentsAtSchool,
            string SpeechLanguage_NarrateIncidentsAtHome, string SpeechLanguage_Needs, string SpeechLanguage_Emotions,
            string SpeechLanguage_AchievementsFailure, string SpeechLanguage_Echolalia, string Speech_cmt,

            string Behaviour_FreeTime, string unassociated, string solitary, string onlooker, string parallel, string associative, string cooperative, string Behaviour_situationalmeltdowns, string BEHAVIOUR_cmt,

            /*string Arousal_Evaluation, string Arousal_GeneralState,*/
            string rangevalue, string rangevalue2,
            string Arousal_Stimuli, string Arousal_Transition, string Arousal_FactorOCD, string Arousal_ClaimingFactor, string Arousal_DipsDown, string AROUSAL_cmt,

             string Affect_RangeEmotion, string Affect_ExpressEmotion,
            string Affect_Environment, string Affect_Task, string Affect_Individual, string Affect_ThroughOut, string Affect_Charaterising, string Affect_cmt,

              string Attention_AttentionSpan, string Attention_FocusHandhome, string Attention_FocusHandSchool, string Attention_Dividing, string Attention_ChangeActivities, string Attention_AgeAppropriate,
             string Attention_Distractibility, string Focal_Attention, string Joint_Attention, string Divided_Attention, string Sustained_Attention,
              string Alternating_Attention, string Attention_move, string ATTENTION_cmt,

               string Action_MotorPlanning, string Action_Purposeful,
            string Action_GoalOriented, string Action_FeedBackDependent, string Action_Constructive, string Action_cmt,

              //string Interaction_KnowPeople, 

              string Interacts, string cmtgathering, string Does_not_initiate, string Sustain, string Fight, string Freeze, string Fright,
            string Anxious, string Comfortable, string Nervous, string ANS_response, string OTHERS, string Interaction_SocialQues, string Interaction_Happiness, string Interaction_Sadness, string Interaction_Surprise,
            string Interaction_Shock, string Interaction_Friends, string Interaction_Enjoy, string INTERACTION_cmt,

             string TS_Registration, string TS_Orientation, string TS_Discrimination, string TS_Responsiveness, string SS_Bodyawareness, string SS_Bodyschema, string SS_Orientation,
            string SS_Posterior, string SS_Bilateral, string SS_Balance, string SS_Dominance, string SS_Right, string SS_identifies, string SS_point, string SS_Constantly,
            string SS_clumsy, string SS_maneuver, string SS_overly, string SS_stand, string SS_indulge, string SS_textures, string SS_monkey, string SS_swings, string VM_Registration,

            string VM_Orientation, string VM_Discrimination, string VM_Responsiveness, string PS_Registration, string PS_Gradation, string PS_Discrimination, string PS_Responsiveness,
            string OM_Registration, string OM_Orientation, string OM_Discrimination, string OM_Responsiveness, string AS_Auditory, string AS_Orientation, string AS_Responsiveness,
            string AS_discrimination, string AS_Background, string AS_localization, string AS_Analysis, string AS_sequencing, string AS_blending, string VS_Visual,
            string VS_Responsiveness, string VS_scanning, string VS_constancy, string VS_memory, string VS_Perception, string VS_hand, string VS_foot, string VS_discrimination,
            string VS_closure, string VS_Figureground, string VS_Visualmemory, string VS_sequential, string VS_spatial, string OS_Registration, string OS_Orientation,
            string OS_Discrimination, string OS_Responsiveness,





            string TestMeassures_GrossMotor,
            string TestMeassures_FineMotor, string TestMeassures_DenverLanguage, string TestMeassures_DenverPersonal, string Tests_cmt,

                 string score_Communication_2, string Inter_Communication_2, string GROSS_2, string inter_Gross_2, string FINE_2, string inter_FINE_2,
            string PROBLEM_2, string inter_PROBLEM_2, string PERSONAL_2, string inter_PERSONAL_2,

            // string score_Communication_2months, string Inter_Communication_2months, string GROSS_2months, string inter_Gross_2months, string FINE_2months, string inter_FINE_2months,
            //string PROBLEM_2months, string inter_PROBLEM_2moths, string PERSONAL_2months, string inter_PERSONAL_2months, 

            string Comm_3, string inter_3, string GROSS_3, string GROSS_inter_3,
            string FINE_3, string FINE_inter_3, string PROBLEM_3, string PROBLEM_inter_3, string PERSONAL_3, string PERSONAL_inter_3, string Communication_6, string comm_inter_6, string GROSS_6, string GROSS_inter_6,
            string FINE_6, string FINE_inter_6, string PROBLEM_6, string PROBLEM_inter_6, string PERSONAL_6, string PERSONAL_inter_6, string comm_7, string inter_7, string GROSS_7,
            string GROSS_inter_7, string FINE_7, string FINE_inter_7, string PROBLEM_7, string PROBLEM_inter_7, string PERSONAL_7, string PERSONAL_inter_7, string comm_9,
            string inter_9, string GROSS_9, string GROSS_inter_9, string FINE_9, string FINE_inter_9, string PROBLEM_9, string PROBLEM_inter_9, string PERSONAL_9, string PERSONAL_inter_9,
            string comm_10, string inter_10, string GROSS_10, string GROSS_inter_10, string FINE_10, string FINE_inter_10, string PROBLEM_10, string PROBLEM_inter_10, string PERSONAL_10,
            string PERSONAL_inter_10, string comm_11, string inter_11, string GROSS_11, string GROSS_inter_11, string FINE_11, string FINE_inter_11, string PROBLEM_11, string PROBLEM_inter_11,
            string PERSONAL_11, string PERSONAL_inter_11, string comm_13, string inter_13, string GROSS_13, string GROSS_inter_13, string FINE_13, string FINE_inter_13, string PROBLEM_13,
            string PROBLEM_inter_13, string PERSONAL_13, string PERSONAL_inter_13, string comm_15, string inter_15, string GROSS_15, string GROSS_inter_15, string FINE_15, string FINE_inter_15,
            string PROBLEM_15, string PROBLEM_inter_15, string PERSONAL_15, string PERSONAL_inter_15, string comm_17, string inter_17, string GROSS_17, string GROSS_inter_17, string FINE_17,
           string FINE_inter_17, string PROBLEM_17, string PROBLEM_inter_17, string PERSONAL_17, string PERSONAL_inter_17, string comm_19, string inter_19, string GROSS_19, string GROSS_inter_19,
           string FINE_19, string FINE_inter_19, string PROBLEM_19, string PROBLEM_inter_19, string PERSONAL_19, string PERSONAL_inter_19, string comm_21, string inter_21, string GROSS_21,
           string GROSS_inter_21, string FINE_21, string FINE_inter_21, string PROBLEM_21, string PROBLEM_inter_21, string PERSONAL_21, string PERSONAL_inter_21, string comm_23, string inter_23,
           string GROSS_23, string GROSS_inter_23, string FINE_23, string FINE_inter_23, string PROBLEM_23, string PROBLEM_inter_23, string PERSONAL_23, string PERSONAL_inter_23, string comm_25,
           string inter_25, string GROSS_25, string GROSS_inter_25, string FINE_25, string FINE_inter_25, string PROBLEM_25, string PROBLEM_inter_25, string PERSONAL_25, string PERSONAL_inter_25,
           string comm_28, string inter_28, string GROSS_28, string GROSS_inter_28, string FINE_28, string FINE_inter_28, string PROBLEM_28, string PROBLEM_inter_28, string PERSONAL_28,
           string PERSONAL_inter_28, string comm_31, string inter_31, string GROSS_31, string GROSS_inter_31, string FINE_31, string FINE_inter_31, string PROBLEM_31, string PROBLEM_inter_31,
           string PERSONAL_31, string PERSONAL_inter_31, string comm_34, string inter_34, string GROSS_34, string GROSS_inter_34, string FINE_34, string FINE_inter_34, string PROBLEM_34,
           string PROBLEM_inter_34, string PERSONAL_34, string PERSONAL_inter_34, string comm_42, string inter_42, string GROSS_42, string GROSS_inter_42, string FINE_42,
           string FINE_inter_42, string PROBLEM_42, string PROBLEM_inter_42, string PERSONAL_42, string PERSONAL_inter_42, string comm_45, string inter_45, string GROSS_45, string GROSS_inter_45,
           string FINE_45, string FINE_inter_45, string PROBLEM_45, string PROBLEM_inter_45, string PERSONAL_45, string PERSONAL_inter_45, string comm_51, string inter_51, string GROSS_51,
           string GROSS_inter_51, string FINE_51, string FINE_inter_51, string PROBLEM_51, string PROBLEM_inter_51, string PERSONAL_51, string PERSONAL_inter_51, string comm_60,
           string inter_60, string GROSS_60, string GROSS_inter_60, string FINE_60, string FINE_inter_60, string PROBLEM_60, string PROBLEM_inter_60, string PERSONAL_60,
           string PERSONAL_inter_60,

          string MONTHS, string QUESTIONS,


               string General_Processing, string AUDITORY_Processing, string VISUAL_Processing, string TOUCH_Processing, string MOVEMENT_Processing, string ORAL_Processing, string Raw_score, string Total_rawscore,
            string Interpretation, string Comments_1,

             string Score_seeking, string Score_Avoiding, string Score_sensitivity, string Score_Registration, string Score_general, string Score_Auditory, string Score_visual,
            string Score_touch, string Score_movement, string Score_oral, string Score_behavioural,
            string SEEKING, string AVOIDING, string SENSITIVITY_2, string REGISTRATION, string GENERAL, string AUDITORY,
            string VISUAL, string TOUCH, string MOVEMENT, string ORAL, string BEHAVIORAL, string Comments_2,
             string SPchild_Seeker, string SPchild_Avoider, string SPchild_Sensor, string SPchild_Bystander, string SPchild_Auditory_3, string SPchild_Visual_3, string SPchild_Touch_3,
            string SPchild_Movement_3, string SPchild_Body_position, string SPchild_Oral_3, string SPchild_Conduct_3, string SPchild_Social_emotional, string SPchild_Attentional_3,
            string Seeking_Seeker, string Avoiding_Avoider, string Sensitivity_Sensor,
            string Registration_Bystander, string Auditory_3, string Visual_3, string Touch_3, string Movement_3, string Body_position, string Oral_3, string Conduct_3, string Social_emotional,
            string Attentional_3, string Comments_3,
             string SPAdult_Low_Registration, string SPAdult_Sensory_seeking, string SPAdult_Sensory_Sensitivity, string SPAdult_Sensory_Avoiding,
            string Low_Registration, string Sensory_seeking, string Sensory_Sensitivity, string Sensory_Avoiding, string Comments_4,
            string SP_Low_Registration64, string SP_Sensory_seeking_64, string SP_Sensory_Sensitivity64, string SP_Sensory_Avoiding64,
            string Low_Registration_5, string Sensory_seeking_5,
            string Sensory_Sensitivity_5, string Sensory_Avoiding_5, string Comments_5,
             string Older_Low_Registration, string Older_Sensory_seeking, string Older_Sensory_Sensitivity, string Older_Sensory_Avoiding,
            string Low_Registration_6, string Sensory_seeking_6, string Sensory_Sensitivity_6, string Sensory_Avoiding_6, string Comments_6,

             string ABILITY_months, string ABILITY_questions, string ability_TOTAL, string ability_COMMENTS,

             string DCDQ_Throws1, string DCDQ_Throws2, string DCDQ_Throws3, string DCDQ_Catches1, string DCDQ_Catches2, string DCDQ_Catches3,
          string DCDQ_Hits1, string DCDQ_Hits2, string DCDQ_Hits3, string DCDQ_Jumps1, string DCDQ_Jumps2, string DCDQ_Jumps3, string DCDQ_Runs1, string DCDQ_Runs2, string DCDQ_Runs3,
       string DCDQ_Plans1, string DCDQ_Plans2, string DCDQ_Plans3, string DCDQ_Writing1, string DCDQ_Writing2, string DCDQ_Writing3, string DCDQ_legibly1, string DCDQ_legibly2, string DCDQ_legibly3, string DCDQ_Effort1, string DCDQ_Effort2, string DCDQ_Effort3, string DCDQ_Cuts1, string DCDQ_Cuts2, string DCDQ_Cuts3, string DCDQ_Likes1, string DCDQ_Likes2, string DCDQ_Likes3, string DCDQ_Learning1, string DCDQ_Learning2, string DCDQ_Learning3, string DCDQ_Quick1, string DCDQ_Quick2, string DCDQ_Quick3, string DCDQ_Bull1, string DCDQ_Bull2, string DCDQ_Bull3, string DCDQ_Does1, string DCDQ_Does2, string DCDQ_Does3, string DCDQ_Control, string DCDQ_Fine,
            string DCDQ_General, string DCDQ_Total, string DCDQ_INTERPRETATION, string DCDQ_COMMENT,


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
           string SIPTInfo_ActivityGiven_SociabilityTherapist, string SIPTInfo_ActivityGiven_SociabilityStudents,

           string Evaluation_Strengths, string Evaluation_Concern_Barriers, string Evaluation_Concern_Limitations,
           string Evaluation_Concern_Posture, string Evaluation_Concern_Impairment, string Evaluation_Goal_Summary, string Evaluation_Goal_Previous, string Evaluation_Goal_LongTerm,
           string Evaluation_Goal_ShortTerm, string Evaluation_Goal_Impairment, string Evaluation_Plan_Frequency, string Evaluation_Plan_Service, string Evaluation_Plan_Strategies,
           string Evaluation_Plan_Equipment, string Evaluation_Plan_Education,


            /*string Daily_cmt,*/ /*string Self_cmt,*/
            string Treatment_Home, string Treatment_School, string Treatment_Threapy, string Treatment_cmt,


           /*string abilityQuestionsChild,*/


           int Doctor_Physioptherapist, int Doctor_Occupational,
            bool IsFinal,
            bool IsGiven, DateTime GivenDate, string DiagnosisID, string DiagnosisOther)
        //(string DailySchedule_BreakFastContent, string DailySchedule_Dinner_content, string DailySchedule_LunchContent, string DailySchedule_Snacks,string Interaction_RelatesPeople,
        //string SelfCare_CurrentlyEats, string SpeechLanguage_LanguageSpoken, string SpeechLanguage_Emotionalmilestones,string SpeechLanguage_Want,string Mother_Working, string Father_Working, string Househelp,
        // string Schoolinfo_PlatformInteraction, string Schoolinfo_HourOnlineSchool, string Schoolinfo_SitOnlineSchool,string Schoolinfo_TeacherInstruction, string Schoolinfo_SetUp, string Schoolinfo_BehaviourOnlineSchool,
        // string Arousal_Optimal,string Attention_Span,string Interaction_Strangers,string TestMeassures_IQ, string TestMeassures_DQ, string Percentile_Range,
        // string TestMeassures_ASQ, string TestMeassures_HandWriting,string TestMeassures_SIPT, string TestMeassures_SensoryProfile,)
        {
            SqlCommand cmd = new SqlCommand("Rpt_SI_SET_SUBMIT"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ClinicalObservation", SqlDbType.NVarChar, -1).Value = ClinicleObse_txt;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;

            // cmd.Parameters.Add("@DailySchedule_Dailyaroutine", SqlDbType.NVarChar, -1).Value = DailySchedule_Dailyaroutine;
            // cmd.Parameters.Add("@DailySchedule_WakeUpTime", SqlDbType.NVarChar, -1).Value = DailySchedule_WakeUpTime;
            // //cmd.Parameters.Add("@hfdTime", SqlDbType.NVarChar, -1).Value = hfdTime;
            // cmd.Parameters.Add("@DailySchedule_RestRoomTime", SqlDbType.NVarChar, -1).Value = DailySchedule_RestRoomTime;
            // cmd.Parameters.Add("@DailySchedule_Breakfast", SqlDbType.NVarChar, -1).Value = DailySchedule_Breakfast;
            // cmd.Parameters.Add("@DailySchedule_BreakFastTime", SqlDbType.NVarChar, -1).Value = DailySchedule_BreakFastTime;
            //// cmd.Parameters.Add("@DailySchedule_BreakFastContent", SqlDbType.NVarChar, -1).Value = DailySchedule_BreakFastContent;
            // cmd.Parameters.Add("@DailySchedule_SchoolTime", SqlDbType.NVarChar, -1).Value = DailySchedule_SchoolTime;
            // cmd.Parameters.Add("@DailySchedule_MidMorinig", SqlDbType.NVarChar, -1).Value = DailySchedule_MidMorinig;
            // cmd.Parameters.Add("@DailySchedule_SchoolHours", SqlDbType.NVarChar, -1).Value = DailySchedule_SchoolHours;
            // cmd.Parameters.Add("@DailySchedule_LunchTime", SqlDbType.NVarChar, -1).Value = DailySchedule_LunchTime;
            // //cmd.Parameters.Add("@DailySchedule_LunchContent", SqlDbType.NVarChar, -1).Value = DailySchedule_LunchContent;
            // cmd.Parameters.Add("@DailySchedule_AfternoonRoutine", SqlDbType.NVarChar, -1).Value = DailySchedule_AfternoonRoutine;
            // cmd.Parameters.Add("@DailySchedule_Afternoo_nap", SqlDbType.NVarChar, -1).Value = DailySchedule_Afternoo_nap;
            // //cmd.Parameters.Add("@DailySchedule_Snacks", SqlDbType.NVarChar, -1).Value = DailySchedule_Snacks;
            // //cmd.Parameters.Add("@DailySchedule_Dinner_content", SqlDbType.NVarChar, -1).Value = DailySchedule_Dinner_content;
            // cmd.Parameters.Add("@DailySchedule_DinnerTime", SqlDbType.NVarChar, -1).Value = DailySchedule_DinnerTime;
            // cmd.Parameters.Add("@DailySchedule_PistDinnerAct", SqlDbType.NVarChar, -1).Value = DailySchedule_PistDinnerAct;
            //cmd.Parameters.Add("@SelfCare_CurrentlyEats", SqlDbType.NVarChar, -1).Value = SelfCare_CurrentlyEats;
            //cmd.Parameters.Add("@SelfCare_Brushing", SqlDbType.NVarChar, -1).Value = SelfCare_Brushing;
            //cmd.Parameters.Add("@SelfCare_Bathing", SqlDbType.NVarChar, -1).Value = SelfCare_Bathing;
            //cmd.Parameters.Add("@SelfCare_Toileting", SqlDbType.NVarChar, -1).Value = SelfCare_Toileting;
            //cmd.Parameters.Add("@SelfCare_Dressing", SqlDbType.NVarChar, -1).Value = SelfCare_Dressing;
            //cmd.Parameters.Add("@SelfCare_Breakfast", SqlDbType.NVarChar, -1).Value = SelfCare_Breakfast;
            //cmd.Parameters.Add("@SelfCare_Lunch", SqlDbType.NVarChar, -1).Value = SelfCare_Lunch;
            //cmd.Parameters.Add("@SelfCare_Snacks", SqlDbType.NVarChar, -1).Value = SelfCare_Snacks;
            //cmd.Parameters.Add("@SelfCare_Dinner", SqlDbType.NVarChar, -1).Value = SelfCare_Dinner;
            //cmd.Parameters.Add("@SelfCare_GettingInBus", SqlDbType.NVarChar, -1).Value = SelfCare_GettingInBus;
            //cmd.Parameters.Add("@SelfCare_GoingToSchool", SqlDbType.NVarChar, -1).Value = SelfCare_GoingToSchool;
            //cmd.Parameters.Add("@SelfCare_comeBackSchool", SqlDbType.NVarChar, -1).Value = SelfCare_comeBackSchool;
            //cmd.Parameters.Add("@SelfCare_Ambulation", SqlDbType.NVarChar, -1).Value = SelfCare_Ambulation;
            //cmd.Parameters.Add("@SelfCare_Homeostaticchanges", SqlDbType.NVarChar, -1).Value = SelfCare_Homeostaticchanges;
            //cmd.Parameters.Add("@SelfCare_UrinationdetailsBedwetting", SqlDbType.NVarChar, -1).Value = SelfCare_UrinationdetailsBedwetting;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeMother", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeMother;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeFather", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeFather;
            cmd.Parameters.Add("@FamilyStructure_QualityTimeWeekend", SqlDbType.NVarChar, -1).Value = FamilyStructure_QualityTimeWeekend;
            cmd.Parameters.Add("@Father_Weekends", SqlDbType.NVarChar, -1).Value = Father_Weekends;
            cmd.Parameters.Add("@FamilyStructure_TimeForThreapy", SqlDbType.NVarChar, -1).Value = FamilyStructure_TimeForThreapy;
            cmd.Parameters.Add("@FamilyStructure_AcceptanceCondition", SqlDbType.NVarChar, -1).Value = FamilyStructure_AcceptanceCondition;
            cmd.Parameters.Add("@FamilyStructure_ExtraCaricular", SqlDbType.NVarChar, -1).Value = FamilyStructure_ExtraCaricular;
            //cmd.Parameters.Add("@FamilyStructure_ParentsWorking", SqlDbType.NVarChar, -1).Value = FamilyStructure_ParentsWorking;
            cmd.Parameters.Add("@FamilyStructure_Diciplinary", SqlDbType.NVarChar, -1).Value = FamilyStructure_Diciplinary;
            cmd.Parameters.Add("@FamilyStructure_SiblingBrother", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingBrother;
            //cmd.Parameters.Add("@FamilyStructure_SiblingSister", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingSister;
            //cmd.Parameters.Add("@FamilyStructure_SiblingNA", SqlDbType.NVarChar, -1).Value = FamilyStructure_SiblingNA;
            cmd.Parameters.Add("@FamilyStructure_Expectations", SqlDbType.NVarChar, -1).Value = FamilyStructure_Expectations;
            cmd.Parameters.Add("@FamilyStructure_CloselyInvolved", SqlDbType.NVarChar, -1).Value = FamilyStructure_CloselyInvolved;
            cmd.Parameters.Add("@FAMILY_cmt", SqlDbType.NVarChar, -1).Value = FAMILY_cmt;

            cmd.Parameters.Add("@Schoolinfo_Attend", SqlDbType.NVarChar, -1).Value = Schoolinfo_Attend;
            cmd.Parameters.Add("@Schoolinfo_Type", SqlDbType.NVarChar, -1).Value = Schoolinfo_Type;
            cmd.Parameters.Add("@Schoolinfo_SchoolHours", SqlDbType.NVarChar, -1).Value = Schoolinfo_SchoolHours;
            cmd.Parameters.Add("@School_Bus", SqlDbType.NVarChar, -1).Value = School_Bus;
            cmd.Parameters.Add("@Car", SqlDbType.NVarChar, -1).Value = Car;
            cmd.Parameters.Add("@Two_Wheelers", SqlDbType.NVarChar, -1).Value = Two_Wheelers;
            cmd.Parameters.Add("@walking", SqlDbType.NVarChar, -1).Value = walking;
            cmd.Parameters.Add("@Public_Transport", SqlDbType.NVarChar, -1).Value = Public_Transport;
            cmd.Parameters.Add("@Schoolinfo_NoOfTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_NoOfTeacher;
            //cmd.Parameters.Add("@Schoolinfo_NoOfStudent", SqlDbType.NVarChar, -1).Value = Schoolinfo_NoOfStudent;
            cmd.Parameters.Add("@Floor", SqlDbType.NVarChar, -1).Value = Floor;
            cmd.Parameters.Add("@single_bench", SqlDbType.NVarChar, -1).Value = single_bench;
            cmd.Parameters.Add("@bench2", SqlDbType.NVarChar, -1).Value = bench2;
            cmd.Parameters.Add("@round_table", SqlDbType.NVarChar, -1).Value = round_table;
            cmd.Parameters.Add("@Schoolinfo_Mealtime", SqlDbType.NVarChar, -1).Value = Schoolinfo_Mealtime;
            //cmd.Parameters.Add("@Schoolinfo_Mealtime", SqlDbType.NVarChar, -1).Value = Schoolinfo_Mealtime;
            cmd.Parameters.Add("@Schoolinfo_MealType", SqlDbType.NVarChar, -1).Value = Schoolinfo_MealType;
            cmd.Parameters.Add("@Schoolinfo_Shareing", SqlDbType.NVarChar, -1).Value = Schoolinfo_Shareing;
            cmd.Parameters.Add("@Schoolinfo_HelpEating", SqlDbType.NVarChar, -1).Value = Schoolinfo_HelpEating;
            cmd.Parameters.Add("@Schoolinfo_Friendship", SqlDbType.NVarChar, -1).Value = Schoolinfo_Friendship;
            cmd.Parameters.Add("@Schoolinfo_InteractionPeer", SqlDbType.NVarChar, -1).Value = Schoolinfo_InteractionPeer;
            cmd.Parameters.Add("@Schoolinfo_InteractionTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_InteractionTeacher;
            cmd.Parameters.Add("@Schoolinfo_AnnualFunction", SqlDbType.NVarChar, -1).Value = Schoolinfo_AnnualFunction;
            cmd.Parameters.Add("@Schoolinfo_Sports", SqlDbType.NVarChar, -1).Value = Schoolinfo_Sports;
            cmd.Parameters.Add("@Schoolinfo_Picnic", SqlDbType.NVarChar, -1).Value = Schoolinfo_Picnic;
            cmd.Parameters.Add("@Schoolinfo_ExtraCaricular", SqlDbType.NVarChar, -1).Value = Schoolinfo_ExtraCaricular;
            cmd.Parameters.Add("@Schoolinfo_CopyBoard", SqlDbType.NVarChar, -1).Value = Schoolinfo_CopyBoard;
            cmd.Parameters.Add("@Schoolinfo_Instructions", SqlDbType.NVarChar, -1).Value = Schoolinfo_Instructions;
            cmd.Parameters.Add("@Schoolinfo_ShadowTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_ShadowTeacher;
            cmd.Parameters.Add("@Schoolinfo_CW_HW", SqlDbType.NVarChar, -1).Value = Schoolinfo_CW_HW;
            cmd.Parameters.Add("@Schoolinfo_SpecialEducator", SqlDbType.NVarChar, -1).Value = Schoolinfo_SpecialEducator;
            cmd.Parameters.Add("@Schoolinfo_DeliveryInformation", SqlDbType.NVarChar, -1).Value = Schoolinfo_DeliveryInformation;
            cmd.Parameters.Add("@Schoolinfo_RemarkTeacher", SqlDbType.NVarChar, -1).Value = Schoolinfo_RemarkTeacher;
            cmd.Parameters.Add("@SCHOOL_cmt", SqlDbType.NVarChar, -1).Value = SCHOOL_cmt;

            cmd.Parameters.Add("@PersonalSocial_CurrentPlace", SqlDbType.NVarChar, -1).Value = PersonalSocial_CurrentPlace;
            cmd.Parameters.Add("@PersonalSocial_WhatHeDoes", SqlDbType.NVarChar, -1).Value = PersonalSocial_WhatHeDoes;
            cmd.Parameters.Add("@PersonalSocial_BodyAwareness", SqlDbType.NVarChar, -1).Value = PersonalSocial_BodyAwareness;
            cmd.Parameters.Add("@PersonalSocial_BodySchema", SqlDbType.NVarChar, -1).Value = PersonalSocial_BodySchema;
            cmd.Parameters.Add("@PersonalSocial_ExploreEnvironment", SqlDbType.NVarChar, -1).Value = PersonalSocial_ExploreEnvironment;
            cmd.Parameters.Add("@PersonalSocial_Motivated", SqlDbType.NVarChar, -1).Value = PersonalSocial_Motivated;
            cmd.Parameters.Add("@PersonalSocial_EyeContact", SqlDbType.NVarChar, -1).Value = PersonalSocial_EyeContact;
            cmd.Parameters.Add("@PersonalSocial_SocialSmile", SqlDbType.NVarChar, -1).Value = PersonalSocial_SocialSmile;
            cmd.Parameters.Add("@PersonalSocial_FamilyRegards", SqlDbType.NVarChar, -1).Value = PersonalSocial_FamilyRegards;
            //cmd.Parameters.Add("@PersonalSocial_RateChild", SqlDbType.NVarChar, -1).Value = PersonalSocial_RateChild;
            cmd.Parameters.Add("@PersonalSocial_ChildSocially", SqlDbType.NVarChar, -1).Value = PersonalSocial_ChildSocially;
            cmd.Parameters.Add("@PERSONAL_cmt", SqlDbType.NVarChar, -1).Value = PERSONAL_cmt;


            cmd.Parameters.Add("@SpeechLanguage_StartSpeek", SqlDbType.NVarChar, -1).Value = SpeechLanguage_StartSpeek;
            cmd.Parameters.Add("@SpeechLanguage_Monosyllables", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Monosyllables;
            cmd.Parameters.Add("@SpeechLanguage_Bisyllables", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Bisyllables;
            cmd.Parameters.Add("@SpeechLanguage_ShrotScentences", SqlDbType.NVarChar, -1).Value = SpeechLanguage_ShrotScentences;
            cmd.Parameters.Add("@SpeechLanguage_LongScentences", SqlDbType.NVarChar, -1).Value = SpeechLanguage_LongScentences;
            cmd.Parameters.Add("@SpeechLanguage_UnusualSoundsJargonSpeech", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnusualSoundsJargonSpeech;
            cmd.Parameters.Add("@SpeechLanguage_speechgestures", SqlDbType.NVarChar, -1).Value = SpeechLanguage_speechgestures;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialExpression", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialExpression;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialEyeContact", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialEyeContact;
            cmd.Parameters.Add("@SpeechLanguage_NonverbalfacialGestures", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NonverbalfacialGestures;
            cmd.Parameters.Add("@SpeechLanguage_SimpleComplex", SqlDbType.NVarChar, -1).Value = SpeechLanguage_SimpleComplex;
            cmd.Parameters.Add("@SpeechLanguage_UnderstandImpliedMeaning", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnderstandImpliedMeaning;
            cmd.Parameters.Add("@SpeechLanguage_UnderstandJokesarcasm", SqlDbType.NVarChar, -1).Value = SpeechLanguage_UnderstandJokesarcasm;
            cmd.Parameters.Add("@SpeechLanguage_Respondstoname", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Respondstoname;
            cmd.Parameters.Add("@SpeechLanguage_TwowayInteraction", SqlDbType.NVarChar, -1).Value = SpeechLanguage_TwowayInteraction;
            cmd.Parameters.Add("@SpeechLanguage_NarrateIncidentsAtSchool", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NarrateIncidentsAtSchool;
            cmd.Parameters.Add("@SpeechLanguage_NarrateIncidentsAtHome", SqlDbType.NVarChar, -1).Value = SpeechLanguage_NarrateIncidentsAtHome;
            //Behaviour_situationalmeltdownscmd.Parameters.Add("@SpeechLanguage_Want", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Want;
            cmd.Parameters.Add("@SpeechLanguage_Needs", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Needs;
            cmd.Parameters.Add("@SpeechLanguage_Emotions", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Emotions;
            cmd.Parameters.Add("@SpeechLanguage_AchievementsFailure", SqlDbType.NVarChar, -1).Value = SpeechLanguage_AchievementsFailure;
            //cmd.Parameters.Add("@SpeechLanguage_LanguageSpoken", SqlDbType.NVarChar, -1).Value = SpeechLanguage_LanguageSpoken;
            cmd.Parameters.Add("@SpeechLanguage_Echolalia", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Echolalia;
            //cmd.Parameters.Add("@SpeechLanguage_Emotionalmilestones", SqlDbType.NVarChar, -1).Value = SpeechLanguage_Emotionalmilestones;
            cmd.Parameters.Add("@Speech_cmt", SqlDbType.NVarChar, -1).Value = Speech_cmt;

            cmd.Parameters.Add("@Behaviour_FreeTime", SqlDbType.NVarChar, -1).Value = Behaviour_FreeTime;
            cmd.Parameters.Add("@unassociated", SqlDbType.NVarChar, -1).Value = unassociated;
            cmd.Parameters.Add("@solitary", SqlDbType.NVarChar, -1).Value = solitary;
            cmd.Parameters.Add("@onlooker", SqlDbType.NVarChar, -1).Value = onlooker;
            cmd.Parameters.Add("@parallel", SqlDbType.NVarChar, -1).Value = parallel;
            cmd.Parameters.Add("@associative", SqlDbType.NVarChar, -1).Value = associative;
            cmd.Parameters.Add("@cooperative", SqlDbType.NVarChar, -1).Value = cooperative;
            cmd.Parameters.Add("@Behaviour_situationalmeltdowns", SqlDbType.NVarChar, -1).Value = Behaviour_situationalmeltdowns;
            cmd.Parameters.Add("@BEHAVIOUR_cmt", SqlDbType.NVarChar, -1).Value = BEHAVIOUR_cmt;


            //cmd.Parameters.Add("@Schoolinfo_PlatformInteraction", SqlDbType.NVarChar, -1).Value = Schoolinfo_PlatformInteraction;
            //cmd.Parameters.Add("@Schoolinfo_HourOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_HourOnlineSchool;
            //cmd.Parameters.Add("@Schoolinfo_SitOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_SitOnlineSchool;
            //cmd.Parameters.Add("@Schoolinfo_TeacherInstruction", SqlDbType.NVarChar, -1).Value = Schoolinfo_TeacherInstruction;
            //cmd.Parameters.Add("@Schoolinfo_SetUp", SqlDbType.NVarChar, -1).Value = Schoolinfo_SetUp;
            //cmd.Parameters.Add("@Schoolinfo_BehaviourOnlineSchool", SqlDbType.NVarChar, -1).Value = Schoolinfo_BehaviourOnlineSchool;
            //cmd.Parameters.Add("@Arousal_Evaluation", SqlDbType.NVarChar, -1).Value = Arousal_Evaluation;
            // cmd.Parameters.Add("@Arousal_GeneralState", SqlDbType.NVarChar, -1).Value = Arousal_GeneralState;
            cmd.Parameters.Add("@rangevalue", SqlDbType.NVarChar, -1).Value = rangevalue;
            cmd.Parameters.Add("@rangevalue2", SqlDbType.NVarChar, -1).Value = rangevalue2;

            cmd.Parameters.Add("@Arousal_Stimuli", SqlDbType.NVarChar, -1).Value = Arousal_Stimuli;
            cmd.Parameters.Add("@Arousal_Transition", SqlDbType.NVarChar, -1).Value = Arousal_Transition;
            //cmd.Parameters.Add("@Arousal_Optimal", SqlDbType.NVarChar, -1).Value = Arousal_Optimal;
            cmd.Parameters.Add("@Arousal_FactorOCD", SqlDbType.NVarChar, -1).Value = Arousal_FactorOCD;
            cmd.Parameters.Add("@Arousal_ClaimingFactor", SqlDbType.NVarChar, -1).Value = Arousal_ClaimingFactor;
            cmd.Parameters.Add("@Arousal_DipsDown", SqlDbType.NVarChar, -1).Value = Arousal_DipsDown;
            cmd.Parameters.Add("@AROUSAL_cmt", SqlDbType.NVarChar, -1).Value = AROUSAL_cmt;

            cmd.Parameters.Add("@Affect_RangeEmotion", SqlDbType.NVarChar, -1).Value = Affect_RangeEmotion;
            cmd.Parameters.Add("@Affect_ExpressEmotion", SqlDbType.NVarChar, -1).Value = Affect_ExpressEmotion;
            cmd.Parameters.Add("@Affect_Environment", SqlDbType.NVarChar, -1).Value = Affect_Environment;
            cmd.Parameters.Add("@Affect_Task", SqlDbType.NVarChar, -1).Value = Affect_Task;
            cmd.Parameters.Add("@Affect_Individual", SqlDbType.NVarChar, -1).Value = Affect_Individual;
            cmd.Parameters.Add("@Affect_ThroughOut", SqlDbType.NVarChar, -1).Value = Affect_ThroughOut;
            cmd.Parameters.Add("@Affect_Charaterising", SqlDbType.NVarChar, -1).Value = Affect_Charaterising;
            cmd.Parameters.Add("@Affect_cmt", SqlDbType.NVarChar, -1).Value = Affect_cmt;

            //cmd.Parameters.Add("@Attention_Span", SqlDbType.NVarChar, -1).Value = Attention_Span;
            //cmd.Parameters.Add("@Attention_FocusHand", SqlDbType.NVarChar, -1).Value = Attention_FocusHand;
            cmd.Parameters.Add("@Attention_AttentionSpan", SqlDbType.NVarChar, -1).Value = Attention_AttentionSpan;
            cmd.Parameters.AddWithValue("@Attention_FocusHandhome", Attention_FocusHandhome);
            cmd.Parameters.AddWithValue("@Attention_FocusHandSchool", Attention_FocusHandSchool);
            cmd.Parameters.Add("@Attention_Dividing", SqlDbType.NVarChar, -1).Value = Attention_Dividing;
            cmd.Parameters.Add("@Attention_ChangeActivities", SqlDbType.NVarChar, -1).Value = Attention_ChangeActivities;
            cmd.Parameters.Add("@Attention_AgeAppropriate", SqlDbType.NVarChar, -1).Value = Attention_AgeAppropriate;
            cmd.Parameters.Add("@Attention_Distractibility", SqlDbType.NVarChar, -1).Value = Attention_Distractibility;
            cmd.Parameters.Add("@Focal_Attention", SqlDbType.NVarChar, -1).Value = Focal_Attention;
            cmd.Parameters.Add("@Joint_Attention", SqlDbType.NVarChar, -1).Value = Joint_Attention;
            cmd.Parameters.Add("@Divided_Attention", SqlDbType.NVarChar, -1).Value = Divided_Attention;
            cmd.Parameters.Add("@Sustained_Attention", SqlDbType.NVarChar, -1).Value = Sustained_Attention;
            cmd.Parameters.Add("@Alternating_Attention", SqlDbType.NVarChar, -1).Value = Alternating_Attention;
            cmd.Parameters.Add("@Attention_move", SqlDbType.NVarChar, -1).Value = Attention_move;
            cmd.Parameters.Add("@ATTENTION_cmt", SqlDbType.NVarChar, -1).Value = ATTENTION_cmt;



            cmd.Parameters.Add("@Action_MotorPlanning", SqlDbType.NVarChar, -1).Value = Action_MotorPlanning;
            cmd.Parameters.Add("@Action_Purposeful", SqlDbType.NVarChar, -1).Value = Action_Purposeful;
            cmd.Parameters.Add("@Action_GoalOriented", SqlDbType.NVarChar, -1).Value = Action_GoalOriented;
            cmd.Parameters.Add("@Action_FeedBackDependent", SqlDbType.NVarChar, -1).Value = Action_FeedBackDependent;
            cmd.Parameters.Add("@Action_Constructive", SqlDbType.NVarChar, -1).Value = Action_Constructive;
            cmd.Parameters.Add("@Action_cmt", SqlDbType.NVarChar, -1).Value = Action_cmt;
            //cmd.Parameters.Add("@Interaction_KnowPeople", SqlDbType.NVarChar, -1).Value = Interaction_KnowPeople;
            //cmd.Parameters.Add("@Interaction_Strangers", SqlDbType.NVarChar, -1).Value = Interaction_Strangers;
            cmd.Parameters.Add("@Interacts", SqlDbType.NVarChar, -1).Value = Interacts;
            cmd.Parameters.Add("@cmtgathering", SqlDbType.NVarChar, -1).Value = cmtgathering;
            cmd.Parameters.Add("@Does_not_initiate", SqlDbType.NVarChar, -1).Value = Does_not_initiate;
            cmd.Parameters.Add("@Sustain", SqlDbType.NVarChar, -1).Value = Sustain;
            cmd.Parameters.Add("@Fight", SqlDbType.NVarChar, -1).Value = Fight;
            cmd.Parameters.Add("@Freeze", SqlDbType.NVarChar, -1).Value = Freeze;
            cmd.Parameters.Add("@Fright", SqlDbType.NVarChar, -1).Value = Fright;

            cmd.Parameters.Add("@Anxious", SqlDbType.NVarChar, -1).Value = Anxious;
            cmd.Parameters.Add("@Comfortable", SqlDbType.NVarChar, -1).Value = Comfortable;
            cmd.Parameters.Add("@Nervous", SqlDbType.NVarChar, -1).Value = Nervous;
            cmd.Parameters.Add("@ANS_response", SqlDbType.NVarChar, -1).Value = ANS_response;
            cmd.Parameters.Add("@OTHERS", SqlDbType.NVarChar, -1).Value = OTHERS;

            cmd.Parameters.Add("@Interaction_SocialQues", SqlDbType.NVarChar, -1).Value = Interaction_SocialQues;
            cmd.Parameters.Add("@Interaction_Happiness", SqlDbType.NVarChar, -1).Value = Interaction_Happiness;
            cmd.Parameters.Add("@Interaction_Sadness", SqlDbType.NVarChar, -1).Value = Interaction_Sadness;
            cmd.Parameters.Add("@Interaction_Surprise", SqlDbType.NVarChar, -1).Value = Interaction_Surprise;
            cmd.Parameters.Add("@Interaction_Shock", SqlDbType.NVarChar, -1).Value = Interaction_Shock;
            cmd.Parameters.Add("@Interaction_Friends", SqlDbType.NVarChar, -1).Value = Interaction_Friends;
            //cmd.Parameters.Add("@Interaction_RelatesPeople", SqlDbType.NVarChar, -1).Value = Interaction_RelatesPeople;
            cmd.Parameters.Add("@Interaction_Enjoy", SqlDbType.NVarChar, -1).Value = Interaction_Enjoy;
            cmd.Parameters.Add("@INTERACTION_cmt", SqlDbType.NVarChar, -1).Value = INTERACTION_cmt;

            cmd.Parameters.Add("@TS_Registration", SqlDbType.NVarChar, -1).Value = TS_Registration;
            cmd.Parameters.Add("@TS_Orientation", SqlDbType.NVarChar, -1).Value = TS_Orientation;
            cmd.Parameters.Add("@TS_Discrimination", SqlDbType.NVarChar, -1).Value = TS_Discrimination;
            cmd.Parameters.Add("@TS_Responsiveness", SqlDbType.NVarChar, -1).Value = TS_Responsiveness;
            cmd.Parameters.Add("@SS_Bodyawareness", SqlDbType.NVarChar, -1).Value = SS_Bodyawareness;
            cmd.Parameters.Add("@SS_Bodyschema", SqlDbType.NVarChar, -1).Value = SS_Bodyschema;
            cmd.Parameters.Add("@SS_Orientation", SqlDbType.NVarChar, -1).Value = SS_Orientation;
            cmd.Parameters.Add("@SS_Posterior", SqlDbType.NVarChar, -1).Value = SS_Posterior;
            cmd.Parameters.Add("@SS_Bilateral", SqlDbType.NVarChar, -1).Value = SS_Bilateral;
            cmd.Parameters.Add("@SS_Balance", SqlDbType.NVarChar, -1).Value = SS_Balance;
            cmd.Parameters.Add("@SS_Dominance", SqlDbType.NVarChar, -1).Value = SS_Dominance;
            cmd.Parameters.Add("@SS_Right", SqlDbType.NVarChar, -1).Value = SS_Right;
            cmd.Parameters.Add("@SS_identifies", SqlDbType.NVarChar, -1).Value = SS_identifies;
            cmd.Parameters.Add("@SS_point", SqlDbType.NVarChar, -1).Value = SS_point;
            cmd.Parameters.Add("@SS_Constantly", SqlDbType.NVarChar, -1).Value = SS_Constantly;
            cmd.Parameters.Add("@SS_clumsy", SqlDbType.NVarChar, -1).Value = SS_clumsy;
            cmd.Parameters.Add("@SS_maneuver", SqlDbType.NVarChar, -1).Value = SS_maneuver;
            cmd.Parameters.Add("@SS_overly", SqlDbType.NVarChar, -1).Value = SS_overly;
            cmd.Parameters.Add("@SS_stand", SqlDbType.NVarChar, -1).Value = SS_stand;
            cmd.Parameters.Add("@SS_indulge", SqlDbType.NVarChar, -1).Value = SS_indulge;
            cmd.Parameters.Add("@SS_textures", SqlDbType.NVarChar, -1).Value = SS_textures;
            cmd.Parameters.Add("@SS_monkey", SqlDbType.NVarChar, -1).Value = SS_monkey;
            cmd.Parameters.Add("@SS_swings", SqlDbType.NVarChar, -1).Value = SS_swings;
            cmd.Parameters.Add("@VM_Registration", SqlDbType.NVarChar, -1).Value = VM_Registration;
            cmd.Parameters.Add("@VM_Orientation", SqlDbType.NVarChar, -1).Value = VM_Orientation;
            cmd.Parameters.Add("@VM_Discrimination", SqlDbType.NVarChar, -1).Value = VM_Discrimination;
            cmd.Parameters.Add("@VM_Responsiveness", SqlDbType.NVarChar, -1).Value = VM_Responsiveness;
            cmd.Parameters.Add("@PS_Registration", SqlDbType.NVarChar, -1).Value = PS_Registration;
            cmd.Parameters.Add("@PS_Gradation", SqlDbType.NVarChar, -1).Value = PS_Gradation;
            cmd.Parameters.Add("@PS_Discrimination", SqlDbType.NVarChar, -1).Value = PS_Discrimination;
            cmd.Parameters.Add("@PS_Responsiveness", SqlDbType.NVarChar, -1).Value = PS_Responsiveness;
            cmd.Parameters.Add("@OM_Registration", SqlDbType.NVarChar, -1).Value = OM_Registration;
            cmd.Parameters.Add("@OM_Orientation", SqlDbType.NVarChar, -1).Value = OM_Orientation;
            cmd.Parameters.Add("@OM_Discrimination", SqlDbType.NVarChar, -1).Value = OM_Discrimination;
            cmd.Parameters.Add("@OM_Responsiveness", SqlDbType.NVarChar, -1).Value = OM_Responsiveness;
            cmd.Parameters.Add("@AS_Auditory", SqlDbType.NVarChar, -1).Value = AS_Auditory;
            cmd.Parameters.Add("@AS_Orientation", SqlDbType.NVarChar, -1).Value = AS_Orientation;
            cmd.Parameters.Add("@AS_Responsiveness", SqlDbType.NVarChar, -1).Value = AS_Responsiveness;
            cmd.Parameters.Add("@AS_discrimination", SqlDbType.NVarChar, -1).Value = AS_discrimination;
            cmd.Parameters.Add("@AS_Background", SqlDbType.NVarChar, -1).Value = AS_Background;
            cmd.Parameters.Add("@AS_localization", SqlDbType.NVarChar, -1).Value = AS_localization;
            cmd.Parameters.Add("@AS_Analysis", SqlDbType.NVarChar, -1).Value = AS_Analysis;
            cmd.Parameters.Add("@AS_sequencing", SqlDbType.NVarChar, -1).Value = AS_sequencing;
            cmd.Parameters.Add("@AS_blending", SqlDbType.NVarChar, -1).Value = AS_blending;
            cmd.Parameters.Add("@VS_Visual", SqlDbType.NVarChar, -1).Value = VS_Visual;
            cmd.Parameters.Add("@VS_Responsiveness", SqlDbType.NVarChar, -1).Value = VS_Responsiveness;
            cmd.Parameters.Add("@VS_scanning", SqlDbType.NVarChar, -1).Value = VS_scanning;
            cmd.Parameters.Add("@VS_constancy", SqlDbType.NVarChar, -1).Value = VS_constancy;
            cmd.Parameters.Add("@VS_memory", SqlDbType.NVarChar, -1).Value = VS_memory;
            cmd.Parameters.Add("@VS_Perception", SqlDbType.NVarChar, -1).Value = VS_Perception;
            cmd.Parameters.Add("@VS_hand", SqlDbType.NVarChar, -1).Value = VS_hand;
            cmd.Parameters.Add("@VS_foot", SqlDbType.NVarChar, -1).Value = VS_foot;
            cmd.Parameters.Add("@VS_discrimination", SqlDbType.NVarChar, -1).Value = VS_discrimination;
            cmd.Parameters.Add("@VS_closure", SqlDbType.NVarChar, -1).Value = VS_closure;
            cmd.Parameters.Add("@VS_Figureground", SqlDbType.NVarChar, -1).Value = VS_Figureground;
            cmd.Parameters.Add("@VS_Visualmemory", SqlDbType.NVarChar, -1).Value = VS_Visualmemory;
            cmd.Parameters.Add("@VS_sequential", SqlDbType.NVarChar, -1).Value = VS_sequential;
            cmd.Parameters.Add("@VS_spatial", SqlDbType.NVarChar, -1).Value = VS_spatial;
            cmd.Parameters.Add("@OS_Registration", SqlDbType.NVarChar, -1).Value = OS_Registration;
            cmd.Parameters.Add("@OS_Orientation", SqlDbType.NVarChar, -1).Value = OS_Orientation;
            cmd.Parameters.Add("@OS_Discrimination", SqlDbType.NVarChar, -1).Value = OS_Discrimination;
            cmd.Parameters.Add("@OS_Responsiveness", SqlDbType.NVarChar, -1).Value = OS_Responsiveness;

            cmd.Parameters.Add("@TestMeassures_GrossMotor", SqlDbType.NVarChar, -1).Value = TestMeassures_GrossMotor;
            cmd.Parameters.Add("@TestMeassures_FineMotor", SqlDbType.NVarChar, -1).Value = TestMeassures_FineMotor;
            cmd.Parameters.Add("@TestMeassures_DenverLanguage", SqlDbType.NVarChar, -1).Value = TestMeassures_DenverLanguage;
            cmd.Parameters.Add("@TestMeassures_DenverPersonal", SqlDbType.NVarChar, -1).Value = TestMeassures_DenverPersonal;
            cmd.Parameters.Add("@Tests_cmt", SqlDbType.NVarChar, -1).Value = Tests_cmt;


            cmd.Parameters.Add("@score_Communication_2", SqlDbType.NVarChar, -1).Value = score_Communication_2;
            cmd.Parameters.Add("@Inter_Communication_2", SqlDbType.NVarChar, -1).Value = Inter_Communication_2;
            cmd.Parameters.Add("@GROSS_2", SqlDbType.NVarChar, -1).Value = GROSS_2;
            cmd.Parameters.Add("@inter_Gross_2", SqlDbType.NVarChar, -1).Value = inter_Gross_2;
            cmd.Parameters.Add("@FINE_2", SqlDbType.NVarChar, -1).Value = FINE_2;
            cmd.Parameters.Add("@inter_FINE_2", SqlDbType.NVarChar, -1).Value = inter_FINE_2;
            cmd.Parameters.Add("@PROBLEM_2", SqlDbType.NVarChar, -1).Value = PROBLEM_2;
            cmd.Parameters.Add("@inter_PROBLEM_2", SqlDbType.NVarChar, -1).Value = inter_PROBLEM_2;
            cmd.Parameters.Add("@PERSONAL_2", SqlDbType.NVarChar, -1).Value = PERSONAL_2;
            cmd.Parameters.Add("@inter_PERSONAL_2", SqlDbType.NVarChar, -1).Value = inter_PERSONAL_2;

            //cmd.Parameters.Add("@score_Communication_2months", SqlDbType.NVarChar, -1).Value = score_Communication_2months;
            //cmd.Parameters.Add("@Inter_Communication_2months", SqlDbType.NVarChar, -1).Value = Inter_Communication_2months;
            //cmd.Parameters.Add("@GROSS_2months", SqlDbType.NVarChar, -1).Value = GROSS_2months;
            //cmd.Parameters.Add("@inter_Gross_2months", SqlDbType.NVarChar, -1).Value = inter_Gross_2months;
            //cmd.Parameters.Add("@FINE_2months", SqlDbType.NVarChar, -1).Value = FINE_2months;
            //cmd.Parameters.Add("@inter_FINE_2months", SqlDbType.NVarChar, -1).Value = inter_FINE_2months;
            //cmd.Parameters.Add("@PROBLEM_2months", SqlDbType.NVarChar, -1).Value = PROBLEM_2months;
            //cmd.Parameters.Add("@inter_PROBLEM_2moths", SqlDbType.NVarChar, -1).Value = inter_PROBLEM_2moths;
            //cmd.Parameters.Add("@PERSONAL_2months", SqlDbType.NVarChar, -1).Value = PERSONAL_2months;
            //cmd.Parameters.Add("@inter_PERSONAL_2months", SqlDbType.NVarChar, -1).Value = inter_PERSONAL_2months;

            cmd.Parameters.Add("@Comm_3", SqlDbType.NVarChar, -1).Value = Comm_3;
            cmd.Parameters.Add("@inter_3", SqlDbType.NVarChar, -1).Value = inter_3;
            cmd.Parameters.Add("@GROSS_3", SqlDbType.NVarChar, -1).Value = GROSS_3;
            cmd.Parameters.Add("@GROSS_inter_3", SqlDbType.NVarChar, -1).Value = GROSS_inter_3;
            cmd.Parameters.Add("@FINE_3", SqlDbType.NVarChar, -1).Value = FINE_3;
            cmd.Parameters.Add("@FINE_inter_3", SqlDbType.NVarChar, -1).Value = FINE_inter_3;
            cmd.Parameters.Add("@PROBLEM_3", SqlDbType.NVarChar, -1).Value = PROBLEM_3;
            cmd.Parameters.Add("@PROBLEM_inter_3", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_3;
            cmd.Parameters.Add("@PERSONAL_3", SqlDbType.NVarChar, -1).Value = PERSONAL_3;
            cmd.Parameters.Add("@PERSONAL_inter_3", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_3;
            cmd.Parameters.Add("@Communication_6", SqlDbType.NVarChar, -1).Value = Communication_6;
            cmd.Parameters.Add("@comm_inter_6", SqlDbType.NVarChar, -1).Value = comm_inter_6;
            cmd.Parameters.Add("@GROSS_6", SqlDbType.NVarChar, -1).Value = GROSS_6;
            cmd.Parameters.Add("@GROSS_inter_6", SqlDbType.NVarChar, -1).Value = GROSS_inter_6;
            cmd.Parameters.Add("@FINE_6", SqlDbType.NVarChar, -1).Value = FINE_6;
            cmd.Parameters.Add("@FINE_inter_6", SqlDbType.NVarChar, -1).Value = FINE_inter_6;
            cmd.Parameters.Add("@PROBLEM_6", SqlDbType.NVarChar, -1).Value = PROBLEM_6;
            cmd.Parameters.Add("@PROBLEM_inter_6", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_6;
            cmd.Parameters.Add("@PERSONAL_6", SqlDbType.NVarChar, -1).Value = PERSONAL_6;
            cmd.Parameters.Add("@PERSONAL_inter_6", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_6;
            cmd.Parameters.Add("@comm_7", SqlDbType.NVarChar, -1).Value = comm_7;
            cmd.Parameters.Add("@inter_7", SqlDbType.NVarChar, -1).Value = inter_7;
            cmd.Parameters.Add("@GROSS_7", SqlDbType.NVarChar, -1).Value = GROSS_7;
            cmd.Parameters.Add("@GROSS_inter_7", SqlDbType.NVarChar, -1).Value = GROSS_inter_7;
            cmd.Parameters.Add("@FINE_7", SqlDbType.NVarChar, -1).Value = FINE_7;
            cmd.Parameters.Add("@FINE_inter_7", SqlDbType.NVarChar, -1).Value = FINE_inter_7;
            cmd.Parameters.Add("@PROBLEM_7", SqlDbType.NVarChar, -1).Value = PROBLEM_7;
            cmd.Parameters.Add("@PROBLEM_inter_7", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_7;
            cmd.Parameters.Add("@PERSONAL_7", SqlDbType.NVarChar, -1).Value = PERSONAL_7;
            cmd.Parameters.Add("@PERSONAL_inter_7", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_7;
            cmd.Parameters.Add("@comm_9", SqlDbType.NVarChar, -1).Value = comm_9;
            cmd.Parameters.Add("@inter_9", SqlDbType.NVarChar, -1).Value = inter_9;
            cmd.Parameters.Add("@GROSS_9", SqlDbType.NVarChar, -1).Value = GROSS_9;
            cmd.Parameters.Add("@GROSS_inter_9", SqlDbType.NVarChar, -1).Value = GROSS_inter_9;
            cmd.Parameters.Add("@FINE_9", SqlDbType.NVarChar, -1).Value = FINE_9;
            cmd.Parameters.Add("@FINE_inter_9", SqlDbType.NVarChar, -1).Value = FINE_inter_9;
            cmd.Parameters.Add("@PROBLEM_9", SqlDbType.NVarChar, -1).Value = PROBLEM_9;
            cmd.Parameters.Add("@PROBLEM_inter_9", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_9;
            cmd.Parameters.Add("@PERSONAL_9", SqlDbType.NVarChar, -1).Value = PERSONAL_9;
            cmd.Parameters.Add("@PERSONAL_inter_9", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_9;
            cmd.Parameters.Add("@comm_10", SqlDbType.NVarChar, -1).Value = comm_10;
            cmd.Parameters.Add("@inter_10", SqlDbType.NVarChar, -1).Value = inter_10;
            cmd.Parameters.Add("@GROSS_10", SqlDbType.NVarChar, -1).Value = GROSS_10;
            cmd.Parameters.Add("@GROSS_inter_10", SqlDbType.NVarChar, -1).Value = GROSS_inter_10;
            cmd.Parameters.Add("@FINE_10", SqlDbType.NVarChar, -1).Value = FINE_10;
            cmd.Parameters.Add("@FINE_inter_10", SqlDbType.NVarChar, -1).Value = FINE_inter_10;
            cmd.Parameters.Add("@PROBLEM_10", SqlDbType.NVarChar, -1).Value = PROBLEM_10;
            cmd.Parameters.Add("@PROBLEM_inter_10", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_10;
            cmd.Parameters.Add("@PERSONAL_10", SqlDbType.NVarChar, -1).Value = PERSONAL_10;
            cmd.Parameters.Add("@PERSONAL_inter_10", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_10;
            cmd.Parameters.Add("@comm_11", SqlDbType.NVarChar, -1).Value = comm_11;
            cmd.Parameters.Add("@inter_11", SqlDbType.NVarChar, -1).Value = inter_11;
            cmd.Parameters.Add("@GROSS_11", SqlDbType.NVarChar, -1).Value = GROSS_11;
            cmd.Parameters.Add("@GROSS_inter_11", SqlDbType.NVarChar, -1).Value = GROSS_inter_11;
            cmd.Parameters.Add("@FINE_11", SqlDbType.NVarChar, -1).Value = FINE_11;
            cmd.Parameters.Add("@FINE_inter_11", SqlDbType.NVarChar, -1).Value = FINE_inter_11;
            cmd.Parameters.Add("@PROBLEM_11", SqlDbType.NVarChar, -1).Value = PROBLEM_11;
            cmd.Parameters.Add("@PROBLEM_inter_11", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_11;
            cmd.Parameters.Add("@PERSONAL_11", SqlDbType.NVarChar, -1).Value = PERSONAL_11;
            cmd.Parameters.Add("@PERSONAL_inter_11", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_11;
            cmd.Parameters.Add("@comm_13", SqlDbType.NVarChar, -1).Value = comm_13;
            cmd.Parameters.Add("@inter_13", SqlDbType.NVarChar, -1).Value = inter_13;
            cmd.Parameters.Add("@GROSS_13", SqlDbType.NVarChar, -1).Value = GROSS_13;
            cmd.Parameters.Add("@GROSS_inter_13", SqlDbType.NVarChar, -1).Value = GROSS_inter_13;
            cmd.Parameters.Add("@FINE_13", SqlDbType.NVarChar, -1).Value = FINE_13;
            cmd.Parameters.Add("@FINE_inter_13", SqlDbType.NVarChar, -1).Value = FINE_inter_13;
            cmd.Parameters.Add("@PROBLEM_13", SqlDbType.NVarChar, -1).Value = PROBLEM_13;
            cmd.Parameters.Add("@PROBLEM_inter_13", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_13;
            cmd.Parameters.Add("@PERSONAL_13", SqlDbType.NVarChar, -1).Value = PERSONAL_13;
            cmd.Parameters.Add("@PERSONAL_inter_13", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_13;
            cmd.Parameters.Add("@comm_15", SqlDbType.NVarChar, -1).Value = comm_15;
            cmd.Parameters.Add("@inter_15", SqlDbType.NVarChar, -1).Value = inter_15;
            cmd.Parameters.Add("@GROSS_15", SqlDbType.NVarChar, -1).Value = GROSS_15;
            cmd.Parameters.Add("@GROSS_inter_15", SqlDbType.NVarChar, -1).Value = GROSS_inter_15;
            cmd.Parameters.Add("@FINE_15", SqlDbType.NVarChar, -1).Value = FINE_15;
            cmd.Parameters.Add("@FINE_inter_15", SqlDbType.NVarChar, -1).Value = FINE_inter_15;
            cmd.Parameters.Add("@PROBLEM_15", SqlDbType.NVarChar, -1).Value = PROBLEM_15;
            cmd.Parameters.Add("@PROBLEM_inter_15", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_15;
            cmd.Parameters.Add("@PERSONAL_15", SqlDbType.NVarChar, -1).Value = PERSONAL_15;
            cmd.Parameters.Add("@PERSONAL_inter_15", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_15;
            cmd.Parameters.Add("@comm_17", SqlDbType.NVarChar, -1).Value = comm_17;
            cmd.Parameters.Add("@inter_17", SqlDbType.NVarChar, -1).Value = inter_17;
            cmd.Parameters.Add("@GROSS_17", SqlDbType.NVarChar, -1).Value = GROSS_17;
            cmd.Parameters.Add("@GROSS_inter_17", SqlDbType.NVarChar, -1).Value = GROSS_inter_17;
            cmd.Parameters.Add("@FINE_17", SqlDbType.NVarChar, -1).Value = FINE_17;
            cmd.Parameters.Add("@FINE_inter_17", SqlDbType.NVarChar, -1).Value = FINE_inter_17;
            cmd.Parameters.Add("@PROBLEM_17", SqlDbType.NVarChar, -1).Value = PROBLEM_17;
            cmd.Parameters.Add("@PROBLEM_inter_17", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_17;
            cmd.Parameters.Add("@PERSONAL_17", SqlDbType.NVarChar, -1).Value = PERSONAL_17;
            cmd.Parameters.Add("@PERSONAL_inter_17", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_17;
            cmd.Parameters.Add("@comm_19", SqlDbType.NVarChar, -1).Value = comm_19;
            cmd.Parameters.Add("@inter_19", SqlDbType.NVarChar, -1).Value = inter_19;
            cmd.Parameters.Add("@GROSS_19", SqlDbType.NVarChar, -1).Value = GROSS_19;
            cmd.Parameters.Add("@GROSS_inter_19", SqlDbType.NVarChar, -1).Value = GROSS_inter_19;
            cmd.Parameters.Add("@FINE_19", SqlDbType.NVarChar, -1).Value = FINE_19;
            cmd.Parameters.Add("@FINE_inter_19", SqlDbType.NVarChar, -1).Value = FINE_inter_19;
            cmd.Parameters.Add("@PROBLEM_19", SqlDbType.NVarChar, -1).Value = PROBLEM_19;
            cmd.Parameters.Add("@PROBLEM_inter_19", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_19;
            cmd.Parameters.Add("@PERSONAL_19", SqlDbType.NVarChar, -1).Value = PERSONAL_19;
            cmd.Parameters.Add("@PERSONAL_inter_19", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_19;
            cmd.Parameters.Add("@comm_21", SqlDbType.NVarChar, -1).Value = comm_21;
            cmd.Parameters.Add("@inter_21", SqlDbType.NVarChar, -1).Value = inter_21;
            cmd.Parameters.Add("@GROSS_21", SqlDbType.NVarChar, -1).Value = GROSS_21;
            cmd.Parameters.Add("@GROSS_inter_21", SqlDbType.NVarChar, -1).Value = GROSS_inter_21;
            cmd.Parameters.Add("@FINE_21", SqlDbType.NVarChar, -1).Value = FINE_21;
            cmd.Parameters.Add("@FINE_inter_21", SqlDbType.NVarChar, -1).Value = FINE_inter_21;
            cmd.Parameters.Add("@PROBLEM_21", SqlDbType.NVarChar, -1).Value = PROBLEM_21;
            cmd.Parameters.Add("@PROBLEM_inter_21", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_21;
            cmd.Parameters.Add("@PERSONAL_21", SqlDbType.NVarChar, -1).Value = PERSONAL_21;
            cmd.Parameters.Add("@PERSONAL_inter_21", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_21;
            cmd.Parameters.Add("@comm_23", SqlDbType.NVarChar, -1).Value = comm_23;
            cmd.Parameters.Add("@inter_23", SqlDbType.NVarChar, -1).Value = inter_23;
            cmd.Parameters.Add("@GROSS_23", SqlDbType.NVarChar, -1).Value = GROSS_23;
            cmd.Parameters.Add("@GROSS_inter_23", SqlDbType.NVarChar, -1).Value = GROSS_inter_23;
            cmd.Parameters.Add("@FINE_23", SqlDbType.NVarChar, -1).Value = FINE_23;
            cmd.Parameters.Add("@FINE_inter_23", SqlDbType.NVarChar, -1).Value = FINE_inter_23;
            cmd.Parameters.Add("@PROBLEM_23", SqlDbType.NVarChar, -1).Value = PROBLEM_23;
            cmd.Parameters.Add("@PROBLEM_inter_23", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_23;
            cmd.Parameters.Add("@PERSONAL_23", SqlDbType.NVarChar, -1).Value = PERSONAL_23;
            cmd.Parameters.Add("@PERSONAL_inter_23", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_23;
            cmd.Parameters.Add("@comm_25", SqlDbType.NVarChar, -1).Value = comm_25;
            cmd.Parameters.Add("@inter_25", SqlDbType.NVarChar, -1).Value = inter_25;
            cmd.Parameters.Add("@GROSS_25", SqlDbType.NVarChar, -1).Value = GROSS_25;
            cmd.Parameters.Add("@GROSS_inter_25", SqlDbType.NVarChar, -1).Value = GROSS_inter_25;
            cmd.Parameters.Add("@FINE_25", SqlDbType.NVarChar, -1).Value = FINE_25;
            cmd.Parameters.Add("@FINE_inter_25", SqlDbType.NVarChar, -1).Value = FINE_inter_25;
            cmd.Parameters.Add("@PROBLEM_25", SqlDbType.NVarChar, -1).Value = PROBLEM_25;
            cmd.Parameters.Add("@PROBLEM_inter_25", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_25;
            cmd.Parameters.Add("@PERSONAL_25", SqlDbType.NVarChar, -1).Value = PERSONAL_25;
            cmd.Parameters.Add("@PERSONAL_inter_25", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_25;
            cmd.Parameters.Add("@comm_28", SqlDbType.NVarChar, -1).Value = comm_28;
            cmd.Parameters.Add("@inter_28", SqlDbType.NVarChar, -1).Value = inter_28;
            cmd.Parameters.Add("@GROSS_28", SqlDbType.NVarChar, -1).Value = GROSS_28;
            cmd.Parameters.Add("@GROSS_inter_28", SqlDbType.NVarChar, -1).Value = GROSS_inter_28;
            cmd.Parameters.Add("@FINE_28", SqlDbType.NVarChar, -1).Value = FINE_28;
            cmd.Parameters.Add("@FINE_inter_28", SqlDbType.NVarChar, -1).Value = FINE_inter_28;
            cmd.Parameters.Add("@PROBLEM_28", SqlDbType.NVarChar, -1).Value = PROBLEM_28;
            cmd.Parameters.Add("@PROBLEM_inter_28", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_28;
            cmd.Parameters.Add("@PERSONAL_28", SqlDbType.NVarChar, -1).Value = PERSONAL_28;
            cmd.Parameters.Add("@PERSONAL_inter_28", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_28;
            cmd.Parameters.Add("@comm_31", SqlDbType.NVarChar, -1).Value = comm_31;
            cmd.Parameters.Add("@inter_31", SqlDbType.NVarChar, -1).Value = inter_31;
            cmd.Parameters.Add("@GROSS_31", SqlDbType.NVarChar, -1).Value = GROSS_31;
            cmd.Parameters.Add("@GROSS_inter_31", SqlDbType.NVarChar, -1).Value = GROSS_inter_31;
            cmd.Parameters.Add("@FINE_31", SqlDbType.NVarChar, -1).Value = FINE_31;
            cmd.Parameters.Add("@FINE_inter_31", SqlDbType.NVarChar, -1).Value = FINE_inter_31;
            cmd.Parameters.Add("@PROBLEM_31", SqlDbType.NVarChar, -1).Value = PROBLEM_31;
            cmd.Parameters.Add("@PROBLEM_inter_31", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_31;
            cmd.Parameters.Add("@PERSONAL_31", SqlDbType.NVarChar, -1).Value = PERSONAL_31;
            cmd.Parameters.Add("@PERSONAL_inter_31", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_31;
            cmd.Parameters.Add("@comm_34", SqlDbType.NVarChar, -1).Value = comm_34;
            cmd.Parameters.Add("@inter_34", SqlDbType.NVarChar, -1).Value = inter_34;
            cmd.Parameters.Add("@GROSS_34", SqlDbType.NVarChar, -1).Value = GROSS_34;
            cmd.Parameters.Add("@GROSS_inter_34", SqlDbType.NVarChar, -1).Value = GROSS_inter_34;
            cmd.Parameters.Add("@FINE_34", SqlDbType.NVarChar, -1).Value = FINE_34;
            cmd.Parameters.Add("@FINE_inter_34", SqlDbType.NVarChar, -1).Value = FINE_inter_34;
            cmd.Parameters.Add("@PROBLEM_34", SqlDbType.NVarChar, -1).Value = PROBLEM_34;
            cmd.Parameters.Add("@PROBLEM_inter_34", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_34;
            cmd.Parameters.Add("@PERSONAL_34", SqlDbType.NVarChar, -1).Value = PERSONAL_34;
            cmd.Parameters.Add("@PERSONAL_inter_34", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_34;
            cmd.Parameters.Add("@comm_42", SqlDbType.NVarChar, -1).Value = comm_42;
            cmd.Parameters.Add("@inter_42", SqlDbType.NVarChar, -1).Value = inter_42;
            cmd.Parameters.Add("@GROSS_42", SqlDbType.NVarChar, -1).Value = GROSS_42;
            cmd.Parameters.Add("@GROSS_inter_42", SqlDbType.NVarChar, -1).Value = GROSS_inter_42;
            cmd.Parameters.Add("@FINE_42", SqlDbType.NVarChar, -1).Value = FINE_42;
            cmd.Parameters.Add("@FINE_inter_42", SqlDbType.NVarChar, -1).Value = FINE_inter_42;
            cmd.Parameters.Add("@PROBLEM_42", SqlDbType.NVarChar, -1).Value = PROBLEM_42;
            cmd.Parameters.Add("@PROBLEM_inter_42", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_42;
            cmd.Parameters.Add("@PERSONAL_42", SqlDbType.NVarChar, -1).Value = PERSONAL_42;
            cmd.Parameters.Add("@PERSONAL_inter_42", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_42;
            cmd.Parameters.Add("@comm_45", SqlDbType.NVarChar, -1).Value = comm_45;
            cmd.Parameters.Add("@inter_45", SqlDbType.NVarChar, -1).Value = inter_45;
            cmd.Parameters.Add("@GROSS_45", SqlDbType.NVarChar, -1).Value = GROSS_45;
            cmd.Parameters.Add("@GROSS_inter_45", SqlDbType.NVarChar, -1).Value = GROSS_inter_45;
            cmd.Parameters.Add("@FINE_45", SqlDbType.NVarChar, -1).Value = FINE_45;
            cmd.Parameters.Add("@FINE_inter_45", SqlDbType.NVarChar, -1).Value = FINE_inter_45;
            cmd.Parameters.Add("@PROBLEM_45", SqlDbType.NVarChar, -1).Value = PROBLEM_45;
            cmd.Parameters.Add("@PROBLEM_inter_45", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_45;
            cmd.Parameters.Add("@PERSONAL_45", SqlDbType.NVarChar, -1).Value = PERSONAL_45;
            cmd.Parameters.Add("@PERSONAL_inter_45", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_45;
            cmd.Parameters.Add("@comm_51", SqlDbType.NVarChar, -1).Value = comm_51;
            cmd.Parameters.Add("@inter_51", SqlDbType.NVarChar, -1).Value = inter_51;
            cmd.Parameters.Add("@GROSS_51", SqlDbType.NVarChar, -1).Value = GROSS_51;
            cmd.Parameters.Add("@GROSS_inter_51", SqlDbType.NVarChar, -1).Value = GROSS_inter_51;
            cmd.Parameters.Add("@FINE_51", SqlDbType.NVarChar, -1).Value = FINE_51;
            cmd.Parameters.Add("@FINE_inter_51", SqlDbType.NVarChar, -1).Value = FINE_inter_51;
            cmd.Parameters.Add("@PROBLEM_51", SqlDbType.NVarChar, -1).Value = PROBLEM_51;
            cmd.Parameters.Add("@PROBLEM_inter_51", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_51;
            cmd.Parameters.Add("@PERSONAL_51", SqlDbType.NVarChar, -1).Value = PERSONAL_51;
            cmd.Parameters.Add("@PERSONAL_inter_51", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_51;
            cmd.Parameters.Add("@comm_60", SqlDbType.NVarChar, -1).Value = comm_60;
            cmd.Parameters.Add("@inter_60", SqlDbType.NVarChar, -1).Value = inter_60;
            cmd.Parameters.Add("@GROSS_60", SqlDbType.NVarChar, -1).Value = GROSS_60;
            cmd.Parameters.Add("@GROSS_inter_60", SqlDbType.NVarChar, -1).Value = GROSS_inter_60;
            cmd.Parameters.Add("@FINE_60", SqlDbType.NVarChar, -1).Value = FINE_60;
            cmd.Parameters.Add("@FINE_inter_60", SqlDbType.NVarChar, -1).Value = FINE_inter_60;
            cmd.Parameters.Add("@PROBLEM_60", SqlDbType.NVarChar, -1).Value = PROBLEM_60;
            cmd.Parameters.Add("@PROBLEM_inter_60", SqlDbType.NVarChar, -1).Value = PROBLEM_inter_60;
            cmd.Parameters.Add("@PERSONAL_60", SqlDbType.NVarChar, -1).Value = PERSONAL_60;
            cmd.Parameters.Add("@PERSONAL_inter_60", SqlDbType.NVarChar, -1).Value = PERSONAL_inter_60;

            cmd.Parameters.Add("@MONTHS", SqlDbType.NVarChar, -1).Value = MONTHS;
            cmd.Parameters.Add("@QUESTIONS", SqlDbType.NVarChar, -1).Value = QUESTIONS;

            cmd.Parameters.Add("@General_Processing", SqlDbType.NVarChar, -1).Value = General_Processing;
            cmd.Parameters.Add("@AUDITORY_Processing", SqlDbType.NVarChar, -1).Value = AUDITORY_Processing;
            cmd.Parameters.Add("@VISUAL_Processing", SqlDbType.NVarChar, -1).Value = VISUAL_Processing;
            cmd.Parameters.Add("@TOUCH_Processing", SqlDbType.NVarChar, -1).Value = TOUCH_Processing;
            cmd.Parameters.Add("@MOVEMENT_Processing", SqlDbType.NVarChar, -1).Value = MOVEMENT_Processing;
            cmd.Parameters.Add("@ORAL_Processing", SqlDbType.NVarChar, -1).Value = ORAL_Processing;
            cmd.Parameters.Add("@Raw_score", SqlDbType.NVarChar, -1).Value = Raw_score;
            //cmd.Parameters.Add("@Percentile_Range", SqlDbType.NVarChar, -1).Value = Percentile_Range;
            cmd.Parameters.Add("@Total_rawscore", SqlDbType.NVarChar, -1).Value = Total_rawscore;
            cmd.Parameters.Add("@Interpretation", SqlDbType.NVarChar, -1).Value = Interpretation;
            cmd.Parameters.Add("@Comments_1", SqlDbType.NVarChar, -1).Value = Comments_1;

            cmd.Parameters.Add("@Score_seeking", SqlDbType.NVarChar, -1).Value = Score_seeking;
            cmd.Parameters.Add("@Score_Avoiding", SqlDbType.NVarChar, -1).Value = Score_Avoiding;
            cmd.Parameters.Add("@Score_sensitivity", SqlDbType.NVarChar, -1).Value = Score_sensitivity;
            cmd.Parameters.Add("@Score_Registration", SqlDbType.NVarChar, -1).Value = Score_Registration;
            cmd.Parameters.Add("@Score_general", SqlDbType.NVarChar, -1).Value = Score_general;
            cmd.Parameters.Add("@Score_Auditory", SqlDbType.NVarChar, -1).Value = Score_Auditory;
            cmd.Parameters.Add("@Score_visual", SqlDbType.NVarChar, -1).Value = Score_visual;
            cmd.Parameters.Add("@Score_touch", SqlDbType.NVarChar, -1).Value = Score_touch;
            cmd.Parameters.Add("@Score_movement", SqlDbType.NVarChar, -1).Value = Score_movement;
            cmd.Parameters.Add("@Score_oral", SqlDbType.NVarChar, -1).Value = Score_oral;
            cmd.Parameters.Add("@Score_behavioural", SqlDbType.NVarChar, -1).Value = Score_behavioural;
            cmd.Parameters.Add("@SEEKING", SqlDbType.NVarChar, -1).Value = SEEKING;
            cmd.Parameters.Add("@AVOIDING", SqlDbType.NVarChar, -1).Value = AVOIDING;
            cmd.Parameters.Add("@SENSITIVITY_2", SqlDbType.NVarChar, -1).Value = SENSITIVITY_2;
            cmd.Parameters.Add("@REGISTRATION", SqlDbType.NVarChar, -1).Value = REGISTRATION;
            cmd.Parameters.Add("@GENERAL", SqlDbType.NVarChar, -1).Value = GENERAL;
            cmd.Parameters.Add("@AUDITORY", SqlDbType.NVarChar, -1).Value = AUDITORY;
            cmd.Parameters.Add("@VISUAL", SqlDbType.NVarChar, -1).Value = VISUAL;
            cmd.Parameters.Add("@TOUCH", SqlDbType.NVarChar, -1).Value = TOUCH;
            cmd.Parameters.Add("@MOVEMENT", SqlDbType.NVarChar, -1).Value = MOVEMENT;
            cmd.Parameters.Add("@ORAL", SqlDbType.NVarChar, -1).Value = ORAL;
            cmd.Parameters.Add("@BEHAVIORAL", SqlDbType.NVarChar, -1).Value = BEHAVIORAL;
            cmd.Parameters.Add("@Comments_2", SqlDbType.NVarChar, -1).Value = Comments_2;

            cmd.Parameters.Add("@SPchild_Seeker", SqlDbType.NVarChar, -1).Value = SPchild_Seeker;
            cmd.Parameters.Add("@SPchild_Avoider", SqlDbType.NVarChar, -1).Value = SPchild_Avoider;
            cmd.Parameters.Add("@SPchild_Sensor", SqlDbType.NVarChar, -1).Value = SPchild_Sensor;
            cmd.Parameters.Add("@SPchild_Bystander", SqlDbType.NVarChar, -1).Value = SPchild_Bystander;
            cmd.Parameters.Add("@SPchild_Auditory_3", SqlDbType.NVarChar, -1).Value = SPchild_Auditory_3;
            cmd.Parameters.Add("@SPchild_Visual_3", SqlDbType.NVarChar, -1).Value = SPchild_Visual_3;
            cmd.Parameters.Add("@SPchild_Touch_3", SqlDbType.NVarChar, -1).Value = SPchild_Touch_3;
            cmd.Parameters.Add("@SPchild_Movement_3", SqlDbType.NVarChar, -1).Value = SPchild_Movement_3;
            cmd.Parameters.Add("@SPchild_Body_position", SqlDbType.NVarChar, -1).Value = SPchild_Body_position;
            cmd.Parameters.Add("@SPchild_Oral_3", SqlDbType.NVarChar, -1).Value = SPchild_Oral_3;
            cmd.Parameters.Add("@SPchild_Conduct_3", SqlDbType.NVarChar, -1).Value = SPchild_Conduct_3;
            cmd.Parameters.Add("@SPchild_Social_emotional", SqlDbType.NVarChar, -1).Value = SPchild_Social_emotional;
            cmd.Parameters.Add("@SPchild_Attentional_3", SqlDbType.NVarChar, -1).Value = SPchild_Attentional_3;
            cmd.Parameters.Add("@Seeking_Seeker", SqlDbType.NVarChar, -1).Value = Seeking_Seeker;
            cmd.Parameters.Add("@Avoiding_Avoider", SqlDbType.NVarChar, -1).Value = Avoiding_Avoider;
            cmd.Parameters.Add("@Sensitivity_Sensor", SqlDbType.NVarChar, -1).Value = Sensitivity_Sensor;
            cmd.Parameters.Add("@Registration_Bystander", SqlDbType.NVarChar, -1).Value = Registration_Bystander;
            cmd.Parameters.Add("@Auditory_3", SqlDbType.NVarChar, -1).Value = Auditory_3;
            cmd.Parameters.Add("@Visual_3", SqlDbType.NVarChar, -1).Value = Visual_3;
            cmd.Parameters.Add("@Touch_3", SqlDbType.NVarChar, -1).Value = Touch_3;
            cmd.Parameters.Add("@Movement_3", SqlDbType.NVarChar, -1).Value = Movement_3;
            cmd.Parameters.Add("@Body_position", SqlDbType.NVarChar, -1).Value = Body_position;
            cmd.Parameters.Add("@Oral_3", SqlDbType.NVarChar, -1).Value = Oral_3;
            cmd.Parameters.Add("@Conduct_3", SqlDbType.NVarChar, -1).Value = Conduct_3;
            cmd.Parameters.Add("@Social_emotional", SqlDbType.NVarChar, -1).Value = Social_emotional;
            cmd.Parameters.Add("@Attentional_3", SqlDbType.NVarChar, -1).Value = Attentional_3;
            cmd.Parameters.Add("@Comments_3", SqlDbType.NVarChar, -1).Value = Comments_3;
            cmd.Parameters.Add("@SPAdult_Low_Registration", SqlDbType.NVarChar, -1).Value = SPAdult_Low_Registration;
            cmd.Parameters.Add("@SPAdult_Sensory_seeking", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_seeking;
            cmd.Parameters.Add("@SPAdult_Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_Sensitivity;
            cmd.Parameters.Add("@SPAdult_Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = SPAdult_Sensory_Avoiding;
            cmd.Parameters.Add("@Low_Registration", SqlDbType.NVarChar, -1).Value = Low_Registration;
            cmd.Parameters.Add("@Sensory_seeking", SqlDbType.NVarChar, -1).Value = Sensory_seeking;
            cmd.Parameters.Add("@Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity;
            cmd.Parameters.Add("@Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding;
            cmd.Parameters.Add("@Comments_4", SqlDbType.NVarChar, -1).Value = Comments_4;
            cmd.Parameters.Add("@SP_Low_Registration64", SqlDbType.NVarChar, -1).Value = SP_Low_Registration64;
            cmd.Parameters.Add("@SP_Sensory_seeking_64", SqlDbType.NVarChar, -1).Value = SP_Sensory_seeking_64;
            cmd.Parameters.Add("@SP_Sensory_Sensitivity64", SqlDbType.NVarChar, -1).Value = SP_Sensory_Sensitivity64;
            cmd.Parameters.Add("@SP_Sensory_Avoiding64", SqlDbType.NVarChar, -1).Value = SP_Sensory_Avoiding64;
            cmd.Parameters.Add("@Low_Registration_5", SqlDbType.NVarChar, -1).Value = Low_Registration_5;
            cmd.Parameters.Add("@Sensory_seeking_5", SqlDbType.NVarChar, -1).Value = Sensory_seeking_5;
            cmd.Parameters.Add("@Sensory_Sensitivity_5", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity_5;
            cmd.Parameters.Add("@Sensory_Avoiding_5", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding_5;
            cmd.Parameters.Add("@Comments_5", SqlDbType.NVarChar, -1).Value = Comments_5;
            cmd.Parameters.Add("@Older_Low_Registration", SqlDbType.NVarChar, -1).Value = Older_Low_Registration;
            cmd.Parameters.Add("@Older_Sensory_seeking", SqlDbType.NVarChar, -1).Value = Older_Sensory_seeking;
            cmd.Parameters.Add("@Older_Sensory_Sensitivity", SqlDbType.NVarChar, -1).Value = Older_Sensory_Sensitivity;
            cmd.Parameters.Add("@Older_Sensory_Avoiding", SqlDbType.NVarChar, -1).Value = Older_Sensory_Avoiding;
            cmd.Parameters.Add("@Low_Registration_6", SqlDbType.NVarChar, -1).Value = Low_Registration_6;
            cmd.Parameters.Add("@Sensory_seeking_6", SqlDbType.NVarChar, -1).Value = Sensory_seeking_6;
            cmd.Parameters.Add("@Sensory_Sensitivity_6", SqlDbType.NVarChar, -1).Value = Sensory_Sensitivity_6;
            cmd.Parameters.Add("@Sensory_Avoiding_6", SqlDbType.NVarChar, -1).Value = Sensory_Avoiding_6;
            cmd.Parameters.Add("@Comments_6", SqlDbType.NVarChar, -1).Value = Comments_6;


            cmd.Parameters.Add("@ABILITY_months", SqlDbType.NVarChar, -1).Value = ABILITY_months;
            cmd.Parameters.Add("@ABILITY_questions", SqlDbType.NVarChar, -1).Value = ABILITY_questions;
            cmd.Parameters.Add("@ability_TOTAL", SqlDbType.NVarChar, -1).Value = ability_TOTAL;
            cmd.Parameters.Add("@ability_COMMENTS", SqlDbType.NVarChar, -1).Value = ability_COMMENTS;


            cmd.Parameters.Add("@DCDQ_Throws1", SqlDbType.NVarChar, -1).Value = DCDQ_Throws1;
            cmd.Parameters.Add("@DCDQ_Throws2", SqlDbType.NVarChar, -1).Value = DCDQ_Throws2;
            cmd.Parameters.Add("@DCDQ_Throws3", SqlDbType.NVarChar, -1).Value = DCDQ_Throws3;
            cmd.Parameters.Add("@DCDQ_Catches1", SqlDbType.NVarChar, -1).Value = DCDQ_Catches1;
            cmd.Parameters.Add("@DCDQ_Catches2", SqlDbType.NVarChar, -1).Value = DCDQ_Catches2;
            cmd.Parameters.Add("@DCDQ_Catches3", SqlDbType.NVarChar, -1).Value = DCDQ_Catches3;
            cmd.Parameters.Add("@DCDQ_Hits1", SqlDbType.NVarChar, -1).Value = DCDQ_Hits1;
            cmd.Parameters.Add("@DCDQ_Hits2", SqlDbType.NVarChar, -1).Value = DCDQ_Hits2;
            cmd.Parameters.Add("@DCDQ_Hits3", SqlDbType.NVarChar, -1).Value = DCDQ_Hits3;
            cmd.Parameters.Add("@DCDQ_Jumps1", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps1;
            cmd.Parameters.Add("@DCDQ_Jumps2", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps2;
            cmd.Parameters.Add("@DCDQ_Jumps3", SqlDbType.NVarChar, -1).Value = DCDQ_Jumps3;
            cmd.Parameters.Add("@DCDQ_Runs1", SqlDbType.NVarChar, -1).Value = DCDQ_Runs1;
            cmd.Parameters.Add("@DCDQ_Runs2", SqlDbType.NVarChar, -1).Value = DCDQ_Runs2;
            cmd.Parameters.Add("@DCDQ_Runs3", SqlDbType.NVarChar, -1).Value = DCDQ_Runs3;
            cmd.Parameters.Add("@DCDQ_Plans1", SqlDbType.NVarChar, -1).Value = DCDQ_Plans1;
            cmd.Parameters.Add("@DCDQ_Plans2", SqlDbType.NVarChar, -1).Value = DCDQ_Plans2;
            cmd.Parameters.Add("@DCDQ_Plans3", SqlDbType.NVarChar, -1).Value = DCDQ_Plans3;
            cmd.Parameters.Add("@DCDQ_Writing1", SqlDbType.NVarChar, -1).Value = DCDQ_Writing1;
            cmd.Parameters.Add("@DCDQ_Writing2", SqlDbType.NVarChar, -1).Value = DCDQ_Writing2;
            cmd.Parameters.Add("@DCDQ_Writing3", SqlDbType.NVarChar, -1).Value = DCDQ_Writing3;
            cmd.Parameters.Add("@DCDQ_legibly1", SqlDbType.NVarChar, -1).Value = DCDQ_legibly1;
            cmd.Parameters.Add("@DCDQ_legibly2", SqlDbType.NVarChar, -1).Value = DCDQ_legibly2;
            cmd.Parameters.Add("@DCDQ_legibly3", SqlDbType.NVarChar, -1).Value = DCDQ_legibly3;
            cmd.Parameters.Add("@DCDQ_Effort1", SqlDbType.NVarChar, -1).Value = DCDQ_Effort1;
            cmd.Parameters.Add("@DCDQ_Effort2", SqlDbType.NVarChar, -1).Value = DCDQ_Effort2;
            cmd.Parameters.Add("@DCDQ_Effort3", SqlDbType.NVarChar, -1).Value = DCDQ_Effort3;
            cmd.Parameters.Add("@DCDQ_Cuts1", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts1;
            cmd.Parameters.Add("@DCDQ_Cuts2", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts2;
            cmd.Parameters.Add("@DCDQ_Cuts3", SqlDbType.NVarChar, -1).Value = DCDQ_Cuts3;
            cmd.Parameters.Add("@DCDQ_Likes1", SqlDbType.NVarChar, -1).Value = DCDQ_Likes1;
            cmd.Parameters.Add("@DCDQ_Likes2", SqlDbType.NVarChar, -1).Value = DCDQ_Likes2;
            cmd.Parameters.Add("@DCDQ_Likes3", SqlDbType.NVarChar, -1).Value = DCDQ_Likes3;
            cmd.Parameters.Add("@DCDQ_Learning1", SqlDbType.NVarChar, -1).Value = DCDQ_Learning1;
            cmd.Parameters.Add("@DCDQ_Learning2", SqlDbType.NVarChar, -1).Value = DCDQ_Learning2;
            cmd.Parameters.Add("@DCDQ_Learning3", SqlDbType.NVarChar, -1).Value = DCDQ_Learning3;
            cmd.Parameters.Add("@DCDQ_Quick1", SqlDbType.NVarChar, -1).Value = DCDQ_Quick1;
            cmd.Parameters.Add("@DCDQ_Quick2", SqlDbType.NVarChar, -1).Value = DCDQ_Quick2;
            cmd.Parameters.Add("@DCDQ_Quick3", SqlDbType.NVarChar, -1).Value = DCDQ_Quick3;
            cmd.Parameters.Add("@DCDQ_Bull1", SqlDbType.NVarChar, -1).Value = DCDQ_Bull1;
            cmd.Parameters.Add("@DCDQ_Bull2", SqlDbType.NVarChar, -1).Value = DCDQ_Bull2;
            cmd.Parameters.Add("@DCDQ_Bull3", SqlDbType.NVarChar, -1).Value = DCDQ_Bull3;
            cmd.Parameters.Add("@DCDQ_Does1", SqlDbType.NVarChar, -1).Value = DCDQ_Does1;
            cmd.Parameters.Add("@DCDQ_Does2", SqlDbType.NVarChar, -1).Value = DCDQ_Does2;
            cmd.Parameters.Add("@DCDQ_Does3", SqlDbType.NVarChar, -1).Value = DCDQ_Does3;
            cmd.Parameters.Add("@DCDQ_Control", SqlDbType.NVarChar, -1).Value = DCDQ_Control;
            cmd.Parameters.Add("@DCDQ_Fine", SqlDbType.NVarChar, -1).Value = DCDQ_Fine;
            cmd.Parameters.Add("@DCDQ_General", SqlDbType.NVarChar, -1).Value = DCDQ_General;
            cmd.Parameters.Add("@DCDQ_Total", SqlDbType.NVarChar, -1).Value = DCDQ_Total;
            cmd.Parameters.Add("@DCDQ_INTERPRETATION", SqlDbType.NVarChar, -1).Value = DCDQ_INTERPRETATION;
            cmd.Parameters.Add("@DCDQ_COMMENT", SqlDbType.NVarChar, -1).Value = DCDQ_COMMENT;


            cmd.Parameters.Add("@SIPTInfo_History", SqlDbType.NVarChar, -1).Value = SIPTInfo_History;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GraspRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GraspLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_SphericalRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_SphericalLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_HookRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_HookLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_HookLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_JawChuckLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GripRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_GripLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_GripLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseRight;
            cmd.Parameters.Add("@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction1_ReleaseLeft;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_OppositionRFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchLfL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchMFL;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFR;
            cmd.Parameters.Add("@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.NVarChar, -1).Value = SIPTInfo_HandFunction2_PinchRFL;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Spontaneous", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT3_Spontaneous;
            cmd.Parameters.Add("@SIPTInfo_SIPT3_Command", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT3_Command;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Kinesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Finger", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Finger;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Localisation", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Localisation;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_DoubleTactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Tactile", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Tactile;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Graphesthesia;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_PostRotary", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_PostRotary;
            cmd.Parameters.Add("@SIPTInfo_SIPT4_Standing", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT4_Standing;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Color", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Color;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Form", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Form;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Size", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Size;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Depth", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Depth;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Figure", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Figure;
            cmd.Parameters.Add("@SIPTInfo_SIPT5_Motor", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT5_Motor;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Design", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT6_Design;
            cmd.Parameters.Add("@SIPTInfo_SIPT6_Constructional", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT6_Constructional;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Scanning", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT7_Scanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT7_Memory", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT7_Memory;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Postural", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Postural;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Oral", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Oral;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Sequencing", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Sequencing;
            cmd.Parameters.Add("@SIPTInfo_SIPT8_Commands", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT8_Commands;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Bilateral", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_Bilateral;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_Contralat", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_Contralat;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_PreferredHand", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_PreferredHand;
            cmd.Parameters.Add("@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT9_CrossingMidline;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Draw", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Draw;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_ClockFace", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_ClockFace;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Filtering", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Filtering;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_MotorPlanning;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodyImage", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_BodyImage;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_BodySchema", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_BodySchema;
            cmd.Parameters.Add("@SIPTInfo_SIPT10_Laterality", SqlDbType.NVarChar, -1).Value = SIPTInfo_SIPT10_Laterality;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Remark", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Remark;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_InterestActivity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_InterestCompletion;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Learning", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Learning;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Complexity", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Complexity;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_ProblemSolving;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Concentration", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Concentration;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Retension", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Retension;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SpeedPerfom;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Neatness", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Neatness;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Frustation", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Frustation;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Work", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Work;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_Reaction", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_Reaction;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityTherapist;
            cmd.Parameters.Add("@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.NVarChar, -1).Value = SIPTInfo_ActivityGiven_SociabilityStudents;


            cmd.Parameters.Add("@Evaluation_Strengths", SqlDbType.NVarChar, -1).Value = Evaluation_Strengths;
            cmd.Parameters.Add("@Evaluation_Concern_Barriers", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Barriers;
            cmd.Parameters.Add("@Evaluation_Concern_Limitations", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Limitations;
            cmd.Parameters.Add("@Evaluation_Concern_Posture", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Posture;
            cmd.Parameters.Add("@Evaluation_Concern_Impairment", SqlDbType.NVarChar, -1).Value = Evaluation_Concern_Impairment;
            cmd.Parameters.Add("@Evaluation_Goal_Summary", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Summary;
            cmd.Parameters.Add("@Evaluation_Goal_Previous", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Previous;
            cmd.Parameters.Add("@Evaluation_Goal_LongTerm", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_LongTerm;
            cmd.Parameters.Add("@Evaluation_Goal_ShortTerm", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_ShortTerm;
            cmd.Parameters.Add("@Evaluation_Goal_Impairment", SqlDbType.NVarChar, -1).Value = Evaluation_Goal_Impairment;
            cmd.Parameters.Add("@Evaluation_Plan_Frequency", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Frequency;
            cmd.Parameters.Add("@Evaluation_Plan_Service", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Service;
            cmd.Parameters.Add("@Evaluation_Plan_Strategies", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Strategies;
            cmd.Parameters.Add("@Evaluation_Plan_Equipment", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Equipment;
            cmd.Parameters.Add("@Evaluation_Plan_Education", SqlDbType.NVarChar, -1).Value = Evaluation_Plan_Education;

            //cmd.Parameters.Add("@TestMeassures_IQ", SqlDbType.NVarChar, -1).Value = TestMeassures_IQ;
            //cmd.Parameters.Add("@TestMeassures_DQ", SqlDbType.NVarChar, -1).Value = TestMeassures_DQ;

            //cmd.Parameters.Add("@TestMeassures_ASQ", SqlDbType.NVarChar, -1).Value = TestMeassures_ASQ;
            //cmd.Parameters.Add("@TestMeassures_HandWriting", SqlDbType.NVarChar, -1).Value = TestMeassures_HandWriting;
            //cmd.Parameters.Add("@TestMeassures_SIPT", SqlDbType.NVarChar, -1).Value = TestMeassures_SIPT;
            //cmd.Parameters.Add("@TestMeassures_SensoryProfile", SqlDbType.NVarChar, -1).Value = TestMeassures_SensoryProfile;
            cmd.Parameters.Add("@Treatment_Home", SqlDbType.NVarChar, -1).Value = Treatment_Home;
            cmd.Parameters.Add("@Treatment_School", SqlDbType.NVarChar, -1).Value = Treatment_School;
            cmd.Parameters.Add("@Treatment_Threapy", SqlDbType.NVarChar, -1).Value = Treatment_Threapy;
            cmd.Parameters.Add("@Treatment_cmt", SqlDbType.NVarChar, -1).Value = Treatment_cmt;
            //cmd.Parameters.Add("@Daily_cmt", SqlDbType.NVarChar, -1).Value = Daily_cmt;
            //cmd.Parameters.Add("@Self_cmt", SqlDbType.NVarChar, -1).Value = Self_cmt;
            //cmd.Parameters.Add("@DiagnosisIDs", SqlDbType.VarChar, -1).Value = DiagnosisID;
            //cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;
















            //cmd.Parameters.Add("@abilityQuestionsChild", SqlDbType.NVarChar, -1).Value = abilityQuestionsChild;










            cmd.Parameters.Add("@Doctor_Physioptherapist", SqlDbType.Int).Value = Doctor_Physioptherapist;
            cmd.Parameters.Add("@Doctor_Occupational", SqlDbType.Int).Value = Doctor_Occupational;

            cmd.Parameters.Add("@IsFinal", SqlDbType.Bit).Value = IsFinal;
            cmd.Parameters.Add("@IsGiven", SqlDbType.Bit).Value = IsGiven;
            if (GivenDate > DateTime.MinValue)
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = GivenDate;
            else
                cmd.Parameters.Add("@GivenDate", SqlDbType.DateTime).Value = DBNull.Value;
            //if (ModifyDate > DateTime.MinValue)
            //    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            //else
            //    cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            //cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            cmd.Parameters.Add("@DiagnosisID", SqlDbType.NVarChar, -1).Value = DiagnosisID;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.NVarChar, -1).Value = DiagnosisOther;

            db.DbUpdate(cmd);

            int j = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out j);
            }
            return j;
        }
        //public int SetTimeLine(int AppointmentID, int Option0, string Option1, string Option2, string Option3)
        //{
        //    SqlCommand cmd = new SqlCommand("TimeLine_SI_Set"); cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
        //    cmd.Parameters.Add("@SI_ID", SqlDbType.Int).Value = Option0;
        //    cmd.Parameters.Add("@TIME", SqlDbType.NVarChar - 1).Value = Option1;
        //    cmd.Parameters.Add("@ACTIVITIES", SqlDbType.NVarChar - 1).Value = Option2;
        //    cmd.Parameters.Add("@COMMENTS", SqlDbType.NVarChar - 1).Value = Option3;


        //    SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
        //    Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
        //    Param.Value = 0; cmd.Parameters.Add(Param);

        //    db.DbUpdate(cmd);
        //    int i = 0;
        //    if (cmd.Parameters["@RetVal"].Value != null)
        //    {
        //        int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
        //    }
        //    return i;
        //}

        public int SetTimeLine(int AppointmentID, int Option0, string Option1, string Option2, string Option3, DateTime ModifyDate, int ModifyBy)
        {
            SqlCommand cmd = new SqlCommand("TimeLine_SI_Set_new"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SITime_ID", SqlDbType.Int).Value = Option0;
            cmd.Parameters.Add("@TIME", SqlDbType.NVarChar - 1).Value = Option1;
            cmd.Parameters.Add("@ACTIVITIES", SqlDbType.NVarChar - 1).Value = Option2;
            cmd.Parameters.Add("@COMMENTS", SqlDbType.NVarChar - 1).Value = Option3;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;


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
        public int DeleteRow(int HidSI_ID)
        {
            SqlCommand cmd = new SqlCommand("DeleteSI_ID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SI_ID", SqlDbType.Int).Value = HidSI_ID;
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
        public DataTable GetTherapist(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand(" SELECT DM.Prefix +' '+ DM.FullName AS FullName FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }
    }
}
