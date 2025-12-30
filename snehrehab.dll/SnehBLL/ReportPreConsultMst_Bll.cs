using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class ReportPreConsultMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportPreConsultMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public bool IsValid(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 7;


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
            SqlCommand cmd = new SqlCommand("Report_PreConsultMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable Search_New(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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

        public int DeleteRow(int HidPreConsultID)
        {
            SqlCommand cmd = new SqlCommand("DeletePreConsultID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PreConsultID", SqlDbType.Int).Value = HidPreConsultID;
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
        public DataTable DemoSearch(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Demo_Report_PreScreenMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
        public DataSet Get_New(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Get");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            return db.DbFetch(cmd);
        }
        public int SetTimeLine(int AppointmentID, int Option0, string Option1, string Option2, string Option3, string Option4, string Option5, DateTime ModifyDate, int ModifyBy)
        {
            SqlCommand cmd = new SqlCommand("TimeLine_PreConsultMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@PreConsultID", SqlDbType.Int).Value = Option0;
            cmd.Parameters.Add("@DateMonth", SqlDbType.NVarChar - 1).Value = Option1;
            cmd.Parameters.Add("@RelevantHistory", SqlDbType.NVarChar - 1).Value = Option2;
            cmd.Parameters.Add("@HospitalDoctorsVisited", SqlDbType.NVarChar - 1).Value = Option3;
            cmd.Parameters.Add("@DoctorsRecommendations", SqlDbType.NVarChar - 1).Value = Option4;
            cmd.Parameters.Add("@InvestigationsRecordsResults", SqlDbType.NVarChar - 1).Value = Option5;
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

        public int Set_New(int AppointmentID, bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int ModifyBy,
        DateTime DatepreConsult, string txtComfortableLanguage, DateTime DateBirth, DateTime DateDelivery, string txtCorrectAge,
        string txtAge, string Gender, string txtMotherName, string txtMotherAge, string txtMotherQualification, string txtMotherOccupation,
        string txtMotherWorkingHour, string txtFatherName, string txtFatherAge, string txtFatherOccupation, string txtFatherQualification,
        string txtFatherWorkingHour, string txtAddress, string txtContactDetails, string txtEmailID, string txtReferredBy, string txtTherapistDuringPC,
        string txtDiagnosis, string txtCommentsPI, string txtChiefConcernsHome, string txtChiefConcernsSchool, string txtChiefConcernsSocialGath,
        string txtCommentsCC, string Consanguinity, string ConsanguinityDegree, string txtYearsMarriage, string FamilyStructure, string Conception,
        string PlanningConception, string txtCommentsFH, string ParentChildRelation, string InterParentalRelation, string InterSiblingRelation,
        string DomesticViolence, string FamilyRelocation, string txtfrequency, string PrimaryCare, string txtMotherScreenTime, string txtScreenTimeChild,
        string txtCommentsFR, string txtPrenatalCondition, string CheckMental, string txtDescribeStressors, string txtWGDP, string txtFoetalMovement,
        string txtCommentsMH, string txtDurationLabour, string delivery, string ciab, string txtConditionPostBirth, string txtBirthWeight,
        string GestationalBirthAge, string NICUstay, string txtDurationNICUstay, string txtNICUHistory, string txtReasonNICUstay, string txtAPGARscore,
        string Breastfed, string txtBabyFed, string Problemsduringbreastfeeding, string txtMentionProblem, string txtwaswtcbf, string colicissue,
        string txtOthrtMedicalIssues, string txtCommentsPPH, string txtGrossMotor, string txtFineMotor, string txtPersonalandSocial, string txtCommunication,
        string txtCommentsDM, string Sleepissues, string Presentsleep, string txtSleepduration, string SleepType, string Cosleeping, string txtCosleepingwith,
        string txtAnySleepAdjunctsused, string Naptime, string txtNapduration, string txtCommentsS, string Feedinghabits, string txtTypeoffoodhad, string txtFoodconsistency,
        string txtFoodtemperature, string txtFoodtaste, string txtCommentsFeHa, string txtChildLikes, string txtCommentsITCH, string Playbehaviour, string txtInteractionwithpeers,
        string Strangeranxiety, string PlayToys, string txtPreferenceoftoys, string txtCommentsPB, string Brushing, string txtCommentsBrushing, string Bathing,
        string txtCommentsBathing, string Toileting, string txtCommentsToileting, string Dressing, string txtCommentsDressing, string Eating, string txtCommentsEating,
        string Ambulation, string txtCommentsAmbulation, string Transfers, string txtCommentsTransfers, string txtAddComments, string Prenatalwellness,
        string Siblings, string NoOfSiblings, string RHASiblings, string Consanguinity_1, string ConsanguinityDegree_1,
        string ConsanguinityDegree_2, string FamilyStructure_1, string Conception_1, string Conception_2, string Conception_3, string Conception_4,
        string PlanningConception_1, string InterParentalRelation_1, string InterParentalRelation_2, string ParentChildRelation_1, string ParentChildRelation_2,
        string InterSiblingRelation_1, string InterSiblingRelation_2, string DomesticViolence_1, string DomesticViolence_2, string FamilyRelocation_1,
        string PrimaryCare_1, string PrimaryCare_2, string PrimaryCare_3, string MaternalStress_1, string delivery_1, string delivery_2, string delivery_3,
        string GestationalBirthAge_1, string GestationalBirthAge_2, string AddEvalRec, string ChildAttend, string txtOnlineOffline, string txtWhichGrade,
        string Brushing_1, string Brushing_2, string Bathing_1, string Bathing_2, string Toileting_1, string Toileting_2, string Dressing_1, string Dressing_2,
        string Eating_1, string Eating_2, string Ambulation_1, string Ambulation_2, string Transfers_1, string Transfers_2
        //, string Option1, string Option2, string Option3, string Option4, string Option5
        )
        {
            SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
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
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;
            if (DatepreConsult > DateTime.MinValue)
                cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DatepreConsult;
            else
                cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DBNull.Value; ;
            cmd.Parameters.Add("@ComfortableLanguage", SqlDbType.NVarChar - 1).Value = txtComfortableLanguage;
            if (DateBirth > DateTime.MinValue)
                cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DateBirth;
            else
                cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DBNull.Value;
            if (DateDelivery > DateTime.MinValue)
                cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DateDelivery;
            else
                cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@CorrectAge", SqlDbType.NVarChar - 1).Value = txtCorrectAge;
            cmd.Parameters.Add("@Age", SqlDbType.NVarChar - 1).Value = txtAge;
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar - 1).Value = Gender;
            cmd.Parameters.Add("@MotherName", SqlDbType.NVarChar - 1).Value = txtMotherName;
            cmd.Parameters.Add("@MotherAge", SqlDbType.NVarChar - 1).Value = txtMotherAge;
            cmd.Parameters.Add("@MotherQualification", SqlDbType.NVarChar - 1).Value = txtMotherQualification;
            cmd.Parameters.Add("@MotherOccupation", SqlDbType.NVarChar - 1).Value = txtMotherOccupation;
            cmd.Parameters.Add("@MotherWorkingHour", SqlDbType.NVarChar - 1).Value = txtMotherWorkingHour;
            cmd.Parameters.Add("@FatherName", SqlDbType.NVarChar - 1).Value = txtFatherName;
            cmd.Parameters.Add("@FatherAge", SqlDbType.NVarChar - 1).Value = txtFatherAge;
            cmd.Parameters.Add("@FatherOccupation", SqlDbType.NVarChar - 1).Value = txtFatherOccupation;
            cmd.Parameters.Add("@FatherQualification", SqlDbType.NVarChar - 1).Value = txtFatherQualification;
            cmd.Parameters.Add("@FatherWorkingHour", SqlDbType.NVarChar - 1).Value = txtFatherWorkingHour;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar - 1).Value = txtAddress;
            cmd.Parameters.Add("@ContactDetails", SqlDbType.NVarChar - 1).Value = txtContactDetails;
            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar - 1).Value = txtEmailID;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.NVarChar - 1).Value = txtReferredBy;
            cmd.Parameters.Add("@TherapistDuringPC", SqlDbType.NVarChar - 1).Value = txtTherapistDuringPC;
            cmd.Parameters.Add("@Diagnosis", SqlDbType.NVarChar - 1).Value = txtDiagnosis;
            cmd.Parameters.Add("@CommentsPI", SqlDbType.NVarChar - 1).Value = txtCommentsPI;
            cmd.Parameters.Add("@ChiefConcernsHome", SqlDbType.NVarChar - 1).Value = txtChiefConcernsHome;
            cmd.Parameters.Add("@ChiefConcernsSchool", SqlDbType.NVarChar - 1).Value = txtChiefConcernsSchool;
            cmd.Parameters.Add("@ChiefConcernsSocialGath", SqlDbType.NVarChar - 1).Value = txtChiefConcernsSocialGath;
            cmd.Parameters.Add("@CommentsCC", SqlDbType.NVarChar - 1).Value = txtCommentsCC;
            cmd.Parameters.Add("@Consanguinity", SqlDbType.NVarChar - 1).Value = Consanguinity;
            cmd.Parameters.Add("@ConsanguinityDegree", SqlDbType.NVarChar - 1).Value = ConsanguinityDegree;
            cmd.Parameters.Add("@YearsMarriage", SqlDbType.NVarChar - 1).Value = txtYearsMarriage;
            cmd.Parameters.Add("@FamilyStructure", SqlDbType.NVarChar - 1).Value = FamilyStructure;
            cmd.Parameters.Add("@Conception", SqlDbType.NVarChar - 1).Value = Conception;
            cmd.Parameters.Add("@PlanningConception", SqlDbType.NVarChar - 1).Value = PlanningConception;
            cmd.Parameters.Add("@CommentsFH", SqlDbType.NVarChar - 1).Value = txtCommentsFH;
            cmd.Parameters.Add("@InterParentalRelation", SqlDbType.NVarChar - 1).Value = InterParentalRelation;
            cmd.Parameters.Add("@ParentChildRelation", SqlDbType.NVarChar - 1).Value = ParentChildRelation;
            cmd.Parameters.Add("@InterSiblingRelation", SqlDbType.NVarChar - 1).Value = InterSiblingRelation;
            cmd.Parameters.Add("@DomesticViolence", SqlDbType.NVarChar - 1).Value = DomesticViolence;
            cmd.Parameters.Add("@FamilyRelocation", SqlDbType.NVarChar - 1).Value = FamilyRelocation;
            cmd.Parameters.Add("@frequency", SqlDbType.NVarChar - 1).Value = txtfrequency;
            cmd.Parameters.Add("@PrimaryCare", SqlDbType.NVarChar - 1).Value = PrimaryCare;
            cmd.Parameters.Add("@MotherScreenTime", SqlDbType.NVarChar - 1).Value = txtMotherScreenTime;
            cmd.Parameters.Add("@ScreenTimeChild", SqlDbType.NVarChar - 1).Value = txtScreenTimeChild;
            cmd.Parameters.Add("@CommentsFR", SqlDbType.NVarChar - 1).Value = txtCommentsFR;
            cmd.Parameters.Add("@PrenatalCondition", SqlDbType.NVarChar - 1).Value = txtPrenatalCondition;
            cmd.Parameters.Add("@CheckMental", SqlDbType.NVarChar - 1).Value = CheckMental;
            cmd.Parameters.Add("@DescribeStressors", SqlDbType.NVarChar - 1).Value = txtDescribeStressors;
            cmd.Parameters.Add("@WGDP", SqlDbType.NVarChar - 1).Value = txtWGDP;
            cmd.Parameters.Add("@FoetalMovement", SqlDbType.NVarChar - 1).Value = txtFoetalMovement;
            cmd.Parameters.Add("@CommentsMH", SqlDbType.NVarChar - 1).Value = txtCommentsMH;
            cmd.Parameters.Add("@DurationLabour", SqlDbType.NVarChar - 1).Value = txtDurationLabour;
            cmd.Parameters.Add("@delivery", SqlDbType.NVarChar - 1).Value = delivery;
            cmd.Parameters.Add("@ciab", SqlDbType.NVarChar - 1).Value = ciab;
            cmd.Parameters.Add("@ConditionPostBirth", SqlDbType.NVarChar - 1).Value = txtConditionPostBirth;
            cmd.Parameters.Add("@BirthWeight", SqlDbType.NVarChar - 1).Value = txtBirthWeight;
            cmd.Parameters.Add("@GestationalBirthAge", SqlDbType.NVarChar - 1).Value = GestationalBirthAge;
            cmd.Parameters.Add("@NICUstay", SqlDbType.NVarChar - 1).Value = NICUstay;
            cmd.Parameters.Add("@DurationNICUstay", SqlDbType.NVarChar - 1).Value = txtDurationNICUstay;
            cmd.Parameters.Add("@NICUHistory", SqlDbType.NVarChar - 1).Value = txtNICUHistory;
            cmd.Parameters.Add("@ReasonNICUstay", SqlDbType.NVarChar - 1).Value = txtReasonNICUstay;
            cmd.Parameters.Add("@APGARscore", SqlDbType.NVarChar - 1).Value = txtAPGARscore;
            cmd.Parameters.Add("@Breastfed", SqlDbType.NVarChar - 1).Value = Breastfed;
            cmd.Parameters.Add("@BabyFed", SqlDbType.NVarChar - 1).Value = txtBabyFed;
            cmd.Parameters.Add("@Problemsduringbreastfeeding", SqlDbType.NVarChar - 1).Value = Problemsduringbreastfeeding;
            cmd.Parameters.Add("@MentionProblem", SqlDbType.NVarChar - 1).Value = txtMentionProblem;
            cmd.Parameters.Add("@waswtcbf", SqlDbType.NVarChar - 1).Value = txtwaswtcbf;
            cmd.Parameters.Add("@colicissue", SqlDbType.NVarChar - 1).Value = colicissue;
            cmd.Parameters.Add("@OthrtMedicalIssues", SqlDbType.NVarChar - 1).Value = txtOthrtMedicalIssues;
            cmd.Parameters.Add("@CommentsPPH", SqlDbType.NVarChar - 1).Value = txtCommentsPPH;
            cmd.Parameters.Add("@GrossMotor", SqlDbType.NVarChar - 1).Value = txtGrossMotor;
            cmd.Parameters.Add("@FineMotor", SqlDbType.NVarChar - 1).Value = txtFineMotor;
            cmd.Parameters.Add("@PersonalandSocial", SqlDbType.NVarChar - 1).Value = txtPersonalandSocial;
            cmd.Parameters.Add("@Communication", SqlDbType.NVarChar - 1).Value = txtCommunication;
            cmd.Parameters.Add("@CommentsDM", SqlDbType.NVarChar - 1).Value = txtCommentsDM;
            cmd.Parameters.Add("@Sleepissues", SqlDbType.NVarChar - 1).Value = Sleepissues;
            cmd.Parameters.Add("@Presentsleep", SqlDbType.NVarChar - 1).Value = Presentsleep;
            cmd.Parameters.Add("@Sleepduration", SqlDbType.NVarChar - 1).Value = txtSleepduration;
            cmd.Parameters.Add("@SleepType", SqlDbType.NVarChar - 1).Value = SleepType;
            cmd.Parameters.Add("@Cosleeping", SqlDbType.NVarChar - 1).Value = Cosleeping;
            cmd.Parameters.Add("@Cosleepingwith", SqlDbType.NVarChar - 1).Value = txtCosleepingwith;
            cmd.Parameters.Add("@AnySleepAdjunctsused", SqlDbType.NVarChar - 1).Value = txtAnySleepAdjunctsused;
            cmd.Parameters.Add("@Naptime", SqlDbType.NVarChar - 1).Value = Naptime;
            cmd.Parameters.Add("@Napduration", SqlDbType.NVarChar - 1).Value = txtNapduration;
            cmd.Parameters.Add("@CommentsS", SqlDbType.NVarChar - 1).Value = txtCommentsS;
            cmd.Parameters.Add("@Feedinghabits", SqlDbType.NVarChar - 1).Value = Feedinghabits;
            cmd.Parameters.Add("@Typeoffoodhad", SqlDbType.NVarChar - 1).Value = txtTypeoffoodhad;
            cmd.Parameters.Add("@Foodconsistency", SqlDbType.NVarChar - 1).Value = txtFoodconsistency;
            cmd.Parameters.Add("@Foodtemperature", SqlDbType.NVarChar - 1).Value = txtFoodtemperature;
            cmd.Parameters.Add("@Foodtaste", SqlDbType.NVarChar - 1).Value = txtFoodtaste;
            cmd.Parameters.Add("@CommentsFeHa", SqlDbType.NVarChar - 1).Value = txtCommentsFeHa;
            cmd.Parameters.Add("@ChildLikes", SqlDbType.NVarChar - 1).Value = txtChildLikes;
            cmd.Parameters.Add("@ChildDislikes", SqlDbType.NVarChar - 1).Value = txtChildLikes;
            cmd.Parameters.Add("@MomentsOfHappiness", SqlDbType.NVarChar - 1).Value = txtChildLikes;
            cmd.Parameters.Add("@MomentsOfFear", SqlDbType.NVarChar - 1).Value = txtChildLikes;
            cmd.Parameters.Add("@FeelingsNemotions", SqlDbType.NVarChar - 1).Value = txtChildLikes;
            cmd.Parameters.Add("@signsofstress", SqlDbType.NVarChar - 1).Value = Feedinghabits;
            cmd.Parameters.Add("@CommentsITCH", SqlDbType.NVarChar - 1).Value = txtCommentsITCH;
            cmd.Parameters.Add("@Playbehaviour", SqlDbType.NVarChar - 1).Value = Playbehaviour;
            cmd.Parameters.Add("@Interactionwithpeers", SqlDbType.NVarChar - 1).Value = txtInteractionwithpeers;
            cmd.Parameters.Add("@Strangeranxiety", SqlDbType.NVarChar - 1).Value = Strangeranxiety;
            cmd.Parameters.Add("@PlayToys", SqlDbType.NVarChar - 1).Value = PlayToys;
            cmd.Parameters.Add("@Preferenceoftoys", SqlDbType.NVarChar - 1).Value = txtPreferenceoftoys;
            cmd.Parameters.Add("@CommentsPB", SqlDbType.NVarChar - 1).Value = txtCommentsPB;
            cmd.Parameters.Add("@Brushing", SqlDbType.NVarChar - 1).Value = Brushing;
            cmd.Parameters.Add("@CommentsBrushing", SqlDbType.NVarChar - 1).Value = txtCommentsBrushing;
            cmd.Parameters.Add("@Bathing", SqlDbType.NVarChar - 1).Value = Bathing;
            cmd.Parameters.Add("@CommentsBathing", SqlDbType.NVarChar - 1).Value = txtCommentsBathing;
            cmd.Parameters.Add("@Toileting", SqlDbType.NVarChar - 1).Value = Toileting;
            cmd.Parameters.Add("@CommentsToileting", SqlDbType.NVarChar - 1).Value = txtCommentsToileting;
            cmd.Parameters.Add("@Dressing", SqlDbType.NVarChar - 1).Value = Dressing;
            cmd.Parameters.Add("@CommentsDressing", SqlDbType.NVarChar - 1).Value = txtCommentsDressing;
            cmd.Parameters.Add("@Eating", SqlDbType.NVarChar - 1).Value = Eating;
            cmd.Parameters.Add("@CommentsEating", SqlDbType.NVarChar - 1).Value = txtCommentsEating;
            cmd.Parameters.Add("@Ambulation", SqlDbType.NVarChar - 1).Value = Ambulation;
            cmd.Parameters.Add("@CommentsAmbulation", SqlDbType.NVarChar - 1).Value = txtCommentsAmbulation;
            cmd.Parameters.Add("@Transfers", SqlDbType.NVarChar - 1).Value = Transfers;
            cmd.Parameters.Add("@CommentsTransfers", SqlDbType.NVarChar - 1).Value = txtCommentsTransfers;
            cmd.Parameters.Add("@AddComments", SqlDbType.NVarChar - 1).Value = txtAddComments;
            cmd.Parameters.Add("@Prenatalwellness", SqlDbType.NVarChar - 1).Value = Prenatalwellness;

            cmd.Parameters.Add("@MotherAgeDC", SqlDbType.NVarChar - 1).Value = Siblings;
            cmd.Parameters.Add("@FatherAgeDC", SqlDbType.NVarChar - 1).Value = Siblings;
            cmd.Parameters.Add("@Siblings", SqlDbType.NVarChar - 1).Value = Siblings;
            cmd.Parameters.Add("@NoOfSiblings", SqlDbType.NVarChar - 1).Value = NoOfSiblings;
            cmd.Parameters.Add("@RHASiblings", SqlDbType.NVarChar - 1).Value = RHASiblings;
            cmd.Parameters.Add("@Consanguinity_1", SqlDbType.NVarChar - 1).Value = Consanguinity_1;
            cmd.Parameters.Add("@ConsanguinityDegree_1", SqlDbType.NVarChar - 1).Value = ConsanguinityDegree_1;
            cmd.Parameters.Add("@ConsanguinityDegree_2", SqlDbType.NVarChar - 1).Value = ConsanguinityDegree_2;
            cmd.Parameters.Add("@FamilyStructure_1", SqlDbType.NVarChar - 1).Value = FamilyStructure_1;
            cmd.Parameters.Add("@Conception_1", SqlDbType.NVarChar - 1).Value = Conception_1;
            cmd.Parameters.Add("@Conception_2", SqlDbType.NVarChar - 1).Value = Conception_2;
            cmd.Parameters.Add("@Conception_3", SqlDbType.NVarChar - 1).Value = Conception_3;
            cmd.Parameters.Add("@Conception_4", SqlDbType.NVarChar - 1).Value = Conception_4;
            cmd.Parameters.Add("@PlanningConception_1", SqlDbType.NVarChar - 1).Value = PlanningConception_1;
            cmd.Parameters.Add("@InterParentalRelation_1", SqlDbType.NVarChar - 1).Value = InterParentalRelation_1;
            cmd.Parameters.Add("@InterParentalRelation_2", SqlDbType.NVarChar - 1).Value = InterParentalRelation_2;
            cmd.Parameters.Add("@ParentChildRelation_1", SqlDbType.NVarChar - 1).Value = ParentChildRelation_1;
            cmd.Parameters.Add("@ParentChildRelation_2", SqlDbType.NVarChar - 1).Value = ParentChildRelation_2;
            cmd.Parameters.Add("@InterSiblingRelation_1", SqlDbType.NVarChar - 1).Value = InterSiblingRelation_1;
            cmd.Parameters.Add("@InterSiblingRelation_2", SqlDbType.NVarChar - 1).Value = InterSiblingRelation_2;
            cmd.Parameters.Add("@DomesticViolence_1", SqlDbType.NVarChar - 1).Value = DomesticViolence_1;
            cmd.Parameters.Add("@DomesticViolence_2", SqlDbType.NVarChar - 1).Value = DomesticViolence_2;
            cmd.Parameters.Add("@FamilyRelocation_1", SqlDbType.NVarChar - 1).Value = FamilyRelocation_1;
            cmd.Parameters.Add("@PrimaryCare_1", SqlDbType.NVarChar - 1).Value = PrimaryCare_1;
            cmd.Parameters.Add("@PrimaryCare_2", SqlDbType.NVarChar - 1).Value = PrimaryCare_2;
            cmd.Parameters.Add("@PrimaryCare_3", SqlDbType.NVarChar - 1).Value = PrimaryCare_3;
            cmd.Parameters.Add("@MaternalStress_1", SqlDbType.NVarChar - 1).Value = MaternalStress_1;
            cmd.Parameters.Add("@delivery_1", SqlDbType.NVarChar - 1).Value = delivery_1;
            cmd.Parameters.Add("@delivery_2", SqlDbType.NVarChar - 1).Value = delivery_2;
            cmd.Parameters.Add("@delivery_3", SqlDbType.NVarChar - 1).Value = delivery_3;
            cmd.Parameters.Add("@GestationalBirthAge_1", SqlDbType.NVarChar - 1).Value = GestationalBirthAge_1;
            cmd.Parameters.Add("@GestationalBirthAge_2", SqlDbType.NVarChar - 1).Value = GestationalBirthAge_2;
            cmd.Parameters.Add("@AddEvalRec", SqlDbType.NVarChar - 1).Value = AddEvalRec;
            cmd.Parameters.Add("@ChildAttend", SqlDbType.NVarChar - 1).Value = ChildAttend;
            cmd.Parameters.Add("@AddCommentaAttend", SqlDbType.NVarChar - 1).Value = ChildAttend;
            cmd.Parameters.Add("@OnlineOffline", SqlDbType.NVarChar - 1).Value = txtOnlineOffline;
            cmd.Parameters.Add("@WhichGrade", SqlDbType.NVarChar - 1).Value = txtWhichGrade;

            cmd.Parameters.Add("@Brushing_1", SqlDbType.NVarChar - 1).Value = Brushing_1;
            cmd.Parameters.Add("@Brushing_2", SqlDbType.NVarChar - 1).Value = Brushing_2;
            cmd.Parameters.Add("@Bathing_1", SqlDbType.NVarChar - 1).Value = Bathing_1;
            cmd.Parameters.Add("@Bathing_2", SqlDbType.NVarChar - 1).Value = Bathing_2;
            cmd.Parameters.Add("@Toileting_1", SqlDbType.NVarChar - 1).Value = Toileting_1;
            cmd.Parameters.Add("@Toileting_2", SqlDbType.NVarChar - 1).Value = Toileting_2;
            cmd.Parameters.Add("@Dressing_1", SqlDbType.NVarChar - 1).Value = Dressing_1;
            cmd.Parameters.Add("@Dressing_2", SqlDbType.NVarChar - 1).Value = Dressing_2;
            cmd.Parameters.Add("@Eating_1", SqlDbType.NVarChar - 1).Value = Eating_1;
            cmd.Parameters.Add("@Eating_2", SqlDbType.NVarChar - 1).Value = Eating_2;
            cmd.Parameters.Add("@Ambulation_1", SqlDbType.NVarChar - 1).Value = Ambulation_1;
            cmd.Parameters.Add("@Ambulation_2", SqlDbType.NVarChar - 1).Value = Ambulation_2;
            cmd.Parameters.Add("@Transfers_1", SqlDbType.NVarChar - 1).Value = Transfers_1;
            cmd.Parameters.Add("@Transfers_2", SqlDbType.NVarChar - 1).Value = Transfers_2;


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

        public DataSet Get(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("Report_PreConsultMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            return db.DbFetch(cmd);
        }

        public int Set(int AppointmentID, string His_FamilyHistory, string His_FamilyStru, string His_InterParental, string His_ParentalChild, string His_EmotionalAbus,
            string His_FamilyRelocation, string His_PrimaryCareGiver, string His_MaternalHistory, string His_AnyHistoryOf, string PreNatal_AnyComplication,
            string PreNatal_Complications, string BirthHis_Terms, string BirthHis_TermWeek, string BirthHis_Delivery, string BirthHis_LabourTotal, string BirthHis_LabourDiff,
            string BirthHis_LabourProb, string BirthHis_Aneshthesia, string Other_CIAB, string Other_BirthWeight, string Other_SGA_AGA, string Other_APGAR_Score,
            string Surgical_History, string NICU, string NICU_Duration, string NICU_Reason, string DischargedOnWhichDay, string ChildTakingMotherFeeds, string AnyOtherRelevantMedicalHistory, string MedicalTimeLine,
            string PostDischarge, string HowWasBabyAtHome, string WasHeFeedingWell, string WasHeSleepingWell, string AnyDelay_MedicalEvent_Symptoms, string WhoWasTheFirstNotice,
            string WhatWasDoneForTheSame, string ChildStartedToHeadHold, string WasItOnTimeOrDelayed, string CloselyInvolvedWithChild, string ChildChooseToUseFreeTime, string ObservationsDuringFreePlay,
            string Brushing_Dependant, string Brushing_Independant, string Brushing_Assisted, string Toileting_Dependant, string Toileting_Independant, string Toileting_Assisted, string Bathing_Dependant,
            string Bathing_Independant, string Bathing_Assisted, string Dressing_Dependant, string Dressing_Independant, string Dressing_Assisted, string Feeding_Dependant, string Feeding_Independant,
            string Feeding_Assisted, string Ambulation_Dependant, string Ambulation_Independant, string Ambulation_Assisted, string Transfer_Dependant, string Transfer_Independant, string Transfer_Assisted, string Summary,
            string EvaluationNeeded, string Cardiologist_Name, string Cardiologist_Date, string Cardiologist_Addr, string Cardiologist_Phone, string Orthopedist_Name, string Orthopedist_Date, string Orthopedist_Addr,
            string Orthopedist_Phone, string Psychologist_Name, string Psychologist_Date, string Psychologist_Addr, string Psychologist_Phone, string Psychiatrist_Name, string Psychiatrist_Date, string Psychiatrist_Addr,
            string Psychiatrist_Phone, string Opthalmologist_Name, string Opthalmologist_Date, string Opthalmologist_Addr, string Opthalmologist_Phone, string Speech_Name, string Speech_Date, string Speech_Addr,
            string Speech_Phone, string Pathologist_Name, string Pathologist_Date, string Pathologist_Addr, string Pathologist_Phone, string Occupational_Name, string Occupational_Date, string Occupational_Addr,
            string Occupational_Phone, string Physical_Name, string Physical_Date, string Physical_Addr, string Physical_Phone, string Audiologist_Name, string Audiologist_Date, string Audiologist_Addr, string Audiologist_Phone,
            string ENT_Name, string ENT_Date, string ENT_Addr, string ENT_Phone, string Chiropractor_Name, string Chiropractor_Date, string Chiropractor_Addr, string Chiropractor_Phone, int Doctor_Physioptherapist,
            int Doctor_Occupational, int Doctor_EnterReport, bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int ModifyBy,
            string Other_Name, string Other_Date, string Other_Addr, string Other_Phone, string ReleventMedicalTimeline, string DailyRoutine,
            string DiagnosisIDs, string DiagnosisOther)
        {
            SqlCommand cmd = new SqlCommand("Report_PreConsultMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@His_FamilyHistory", SqlDbType.NVarChar, -1).Value = His_FamilyHistory;
            cmd.Parameters.Add("@His_FamilyStru", SqlDbType.NVarChar, -1).Value = His_FamilyStru;
            cmd.Parameters.Add("@His_InterParental", SqlDbType.NVarChar, -1).Value = His_InterParental;
            cmd.Parameters.Add("@His_ParentalChild", SqlDbType.NVarChar, -1).Value = His_ParentalChild;
            cmd.Parameters.Add("@His_EmotionalAbus", SqlDbType.NVarChar, -1).Value = His_EmotionalAbus;
            cmd.Parameters.Add("@His_FamilyRelocation", SqlDbType.NVarChar, -1).Value = His_FamilyRelocation;
            cmd.Parameters.Add("@His_PrimaryCareGiver", SqlDbType.NVarChar, -1).Value = His_PrimaryCareGiver;
            cmd.Parameters.Add("@His_MaternalHistory", SqlDbType.NVarChar, -1).Value = His_MaternalHistory;
            cmd.Parameters.Add("@His_AnyHistoryOf", SqlDbType.NVarChar, -1).Value = His_AnyHistoryOf;
            cmd.Parameters.Add("@PreNatal_AnyComplication", SqlDbType.NVarChar, -1).Value = PreNatal_AnyComplication;
            cmd.Parameters.Add("@PreNatal_Complications", SqlDbType.NVarChar, -1).Value = PreNatal_Complications;
            cmd.Parameters.Add("@BirthHis_Terms", SqlDbType.NVarChar, -1).Value = BirthHis_Terms;
            cmd.Parameters.Add("@BirthHis_TermWeek", SqlDbType.NVarChar, -1).Value = BirthHis_TermWeek;
            cmd.Parameters.Add("@BirthHis_Delivery", SqlDbType.NVarChar, -1).Value = BirthHis_Delivery;
            cmd.Parameters.Add("@BirthHis_LabourTotal", SqlDbType.NVarChar, -1).Value = BirthHis_LabourTotal;
            cmd.Parameters.Add("@BirthHis_LabourDiff", SqlDbType.NVarChar, -1).Value = BirthHis_LabourDiff;
            cmd.Parameters.Add("@BirthHis_LabourProb", SqlDbType.NVarChar, -1).Value = BirthHis_LabourProb;
            cmd.Parameters.Add("@BirthHis_Aneshthesia", SqlDbType.NVarChar, -1).Value = BirthHis_Aneshthesia;
            cmd.Parameters.Add("@Other_CIAB", SqlDbType.NVarChar, -1).Value = Other_CIAB;
            cmd.Parameters.Add("@Other_BirthWeight", SqlDbType.NVarChar, -1).Value = Other_BirthWeight;
            cmd.Parameters.Add("@Other_SGA_AGA", SqlDbType.NVarChar, -1).Value = Other_SGA_AGA;
            cmd.Parameters.Add("@Other_APGAR_Score", SqlDbType.NVarChar, -1).Value = Other_APGAR_Score;
            cmd.Parameters.Add("@Surgical_History", SqlDbType.NVarChar, -1).Value = Surgical_History;
            cmd.Parameters.Add("@NICU", SqlDbType.NVarChar, -1).Value = NICU;
            cmd.Parameters.Add("@NICU_Duration", SqlDbType.NVarChar, -1).Value = NICU_Duration;
            cmd.Parameters.Add("@NICU_Reason", SqlDbType.NVarChar, -1).Value = NICU_Reason;
            cmd.Parameters.Add("@DischargedOnWhichDay", SqlDbType.NVarChar, -1).Value = DischargedOnWhichDay;
            cmd.Parameters.Add("@ChildTakingMotherFeeds", SqlDbType.NVarChar, -1).Value = ChildTakingMotherFeeds;
            cmd.Parameters.Add("@AnyOtherRelevantMedicalHistory", SqlDbType.NVarChar, -1).Value = AnyOtherRelevantMedicalHistory;
            cmd.Parameters.Add("@MedicalTimeLine", SqlDbType.NVarChar, -1).Value = MedicalTimeLine;
            cmd.Parameters.Add("@PostDischarge", SqlDbType.NVarChar, -1).Value = PostDischarge;
            cmd.Parameters.Add("@HowWasBabyAtHome", SqlDbType.NVarChar, -1).Value = HowWasBabyAtHome;
            cmd.Parameters.Add("@WasHeFeedingWell", SqlDbType.NVarChar, -1).Value = WasHeFeedingWell;
            cmd.Parameters.Add("@WasHeSleepingWell", SqlDbType.NVarChar, -1).Value = WasHeSleepingWell;
            cmd.Parameters.Add("@AnyDelay_MedicalEvent_Symptoms", SqlDbType.NVarChar, -1).Value = AnyDelay_MedicalEvent_Symptoms;
            cmd.Parameters.Add("@WhoWasTheFirstNotice", SqlDbType.NVarChar, -1).Value = WhoWasTheFirstNotice;
            cmd.Parameters.Add("@WhatWasDoneForTheSame", SqlDbType.NVarChar, -1).Value = WhatWasDoneForTheSame;
            cmd.Parameters.Add("@ChildStartedToHeadHold", SqlDbType.NVarChar, -1).Value = ChildStartedToHeadHold;
            cmd.Parameters.Add("@WasItOnTimeOrDelayed", SqlDbType.NVarChar, -1).Value = WasItOnTimeOrDelayed;
            cmd.Parameters.Add("@CloselyInvolvedWithChild", SqlDbType.NVarChar, -1).Value = CloselyInvolvedWithChild;
            cmd.Parameters.Add("@ChildChooseToUseFreeTime", SqlDbType.NVarChar, -1).Value = ChildChooseToUseFreeTime;
            cmd.Parameters.Add("@ObservationsDuringFreePlay", SqlDbType.NVarChar, -1).Value = ObservationsDuringFreePlay;
            cmd.Parameters.Add("@Brushing_Dependant", SqlDbType.NVarChar, -1).Value = Brushing_Dependant;
            cmd.Parameters.Add("@Brushing_Independant", SqlDbType.NVarChar, -1).Value = Brushing_Independant;
            cmd.Parameters.Add("@Brushing_Assisted", SqlDbType.NVarChar, -1).Value = Brushing_Assisted;
            cmd.Parameters.Add("@Toileting_Dependant", SqlDbType.NVarChar, -1).Value = Toileting_Dependant;
            cmd.Parameters.Add("@Toileting_Independant", SqlDbType.NVarChar, -1).Value = Toileting_Independant;
            cmd.Parameters.Add("@Toileting_Assisted", SqlDbType.NVarChar, -1).Value = Toileting_Assisted;
            cmd.Parameters.Add("@Bathing_Dependant", SqlDbType.NVarChar, -1).Value = Bathing_Dependant;
            cmd.Parameters.Add("@Bathing_Independant", SqlDbType.NVarChar, -1).Value = Bathing_Independant;
            cmd.Parameters.Add("@Bathing_Assisted", SqlDbType.NVarChar, -1).Value = Bathing_Assisted;
            cmd.Parameters.Add("@Dressing_Dependant", SqlDbType.NVarChar, -1).Value = Dressing_Dependant;
            cmd.Parameters.Add("@Dressing_Independant", SqlDbType.NVarChar, -1).Value = Dressing_Independant;
            cmd.Parameters.Add("@Dressing_Assisted", SqlDbType.NVarChar, -1).Value = Dressing_Assisted;
            cmd.Parameters.Add("@Feeding_Dependant", SqlDbType.NVarChar, -1).Value = Feeding_Dependant;
            cmd.Parameters.Add("@Feeding_Independant", SqlDbType.NVarChar, -1).Value = Feeding_Independant;
            cmd.Parameters.Add("@Feeding_Assisted", SqlDbType.NVarChar, -1).Value = Feeding_Assisted;
            cmd.Parameters.Add("@Ambulation_Dependant", SqlDbType.NVarChar, -1).Value = Ambulation_Dependant;
            cmd.Parameters.Add("@Ambulation_Independant", SqlDbType.NVarChar, -1).Value = Ambulation_Independant;
            cmd.Parameters.Add("@Ambulation_Assisted", SqlDbType.NVarChar, -1).Value = Ambulation_Assisted;
            cmd.Parameters.Add("@Transfer_Dependant", SqlDbType.NVarChar, -1).Value = Transfer_Dependant;
            cmd.Parameters.Add("@Transfer_Independant", SqlDbType.NVarChar, -1).Value = Transfer_Independant;
            cmd.Parameters.Add("@Transfer_Assisted", SqlDbType.NVarChar, -1).Value = Transfer_Assisted;
            cmd.Parameters.Add("@Summary", SqlDbType.NVarChar, -1).Value = Summary;
            cmd.Parameters.Add("@EvaluationNeeded", SqlDbType.NVarChar, -1).Value = EvaluationNeeded;
            cmd.Parameters.Add("@Cardiologist_Name", SqlDbType.NVarChar, -1).Value = Cardiologist_Name;
            cmd.Parameters.Add("@Cardiologist_Date", SqlDbType.NVarChar, -1).Value = Cardiologist_Date;
            cmd.Parameters.Add("@Cardiologist_Addr", SqlDbType.NVarChar, -1).Value = Cardiologist_Addr;
            cmd.Parameters.Add("@Cardiologist_Phone", SqlDbType.NVarChar, -1).Value = Cardiologist_Phone;
            cmd.Parameters.Add("@Orthopedist_Name", SqlDbType.NVarChar, -1).Value = Orthopedist_Name;
            cmd.Parameters.Add("@Orthopedist_Date", SqlDbType.NVarChar, -1).Value = Orthopedist_Date;
            cmd.Parameters.Add("@Orthopedist_Addr", SqlDbType.NVarChar, -1).Value = Orthopedist_Addr;
            cmd.Parameters.Add("@Orthopedist_Phone", SqlDbType.NVarChar, -1).Value = Orthopedist_Phone;
            cmd.Parameters.Add("@Psychologist_Name", SqlDbType.NVarChar, -1).Value = Psychologist_Name;
            cmd.Parameters.Add("@Psychologist_Date", SqlDbType.NVarChar, -1).Value = Psychologist_Date;
            cmd.Parameters.Add("@Psychologist_Addr", SqlDbType.NVarChar, -1).Value = Psychologist_Addr;
            cmd.Parameters.Add("@Psychologist_Phone", SqlDbType.NVarChar, -1).Value = Psychologist_Phone;
            cmd.Parameters.Add("@Psychiatrist_Name", SqlDbType.NVarChar, -1).Value = Psychiatrist_Name;
            cmd.Parameters.Add("@Psychiatrist_Date", SqlDbType.NVarChar, -1).Value = Psychiatrist_Date;
            cmd.Parameters.Add("@Psychiatrist_Addr", SqlDbType.NVarChar, -1).Value = Psychiatrist_Addr;
            cmd.Parameters.Add("@Psychiatrist_Phone", SqlDbType.NVarChar, -1).Value = Psychiatrist_Phone;
            cmd.Parameters.Add("@Opthalmologist_Name", SqlDbType.NVarChar, -1).Value = Opthalmologist_Name;
            cmd.Parameters.Add("@Opthalmologist_Date", SqlDbType.NVarChar, -1).Value = Opthalmologist_Date;
            cmd.Parameters.Add("@Opthalmologist_Addr", SqlDbType.NVarChar, -1).Value = Opthalmologist_Addr;
            cmd.Parameters.Add("@Opthalmologist_Phone", SqlDbType.NVarChar, -1).Value = Opthalmologist_Phone;
            cmd.Parameters.Add("@Speech_Name", SqlDbType.NVarChar, -1).Value = Speech_Name;
            cmd.Parameters.Add("@Speech_Date", SqlDbType.NVarChar, -1).Value = Speech_Date;
            cmd.Parameters.Add("@Speech_Addr", SqlDbType.NVarChar, -1).Value = Speech_Addr;
            cmd.Parameters.Add("@Speech_Phone", SqlDbType.NVarChar, -1).Value = Speech_Phone;
            cmd.Parameters.Add("@Pathologist_Name", SqlDbType.NVarChar, -1).Value = Pathologist_Name;
            cmd.Parameters.Add("@Pathologist_Date", SqlDbType.NVarChar, -1).Value = Pathologist_Date;
            cmd.Parameters.Add("@Pathologist_Addr", SqlDbType.NVarChar, -1).Value = Pathologist_Addr;
            cmd.Parameters.Add("@Pathologist_Phone", SqlDbType.NVarChar, -1).Value = Pathologist_Phone;
            cmd.Parameters.Add("@Occupational_Name", SqlDbType.NVarChar, -1).Value = Occupational_Name;
            cmd.Parameters.Add("@Occupational_Date", SqlDbType.NVarChar, -1).Value = Occupational_Date;
            cmd.Parameters.Add("@Occupational_Addr", SqlDbType.NVarChar, -1).Value = Occupational_Addr;
            cmd.Parameters.Add("@Occupational_Phone", SqlDbType.NVarChar, -1).Value = Occupational_Phone;
            cmd.Parameters.Add("@Physical_Name", SqlDbType.NVarChar, -1).Value = Physical_Name;
            cmd.Parameters.Add("@Physical_Date", SqlDbType.NVarChar, -1).Value = Physical_Date;
            cmd.Parameters.Add("@Physical_Addr", SqlDbType.NVarChar, -1).Value = Physical_Addr;
            cmd.Parameters.Add("@Physical_Phone", SqlDbType.NVarChar, -1).Value = Physical_Phone;
            cmd.Parameters.Add("@Audiologist_Name", SqlDbType.NVarChar, -1).Value = Audiologist_Name;
            cmd.Parameters.Add("@Audiologist_Date", SqlDbType.NVarChar, -1).Value = Audiologist_Date;
            cmd.Parameters.Add("@Audiologist_Addr", SqlDbType.NVarChar, -1).Value = Audiologist_Addr;
            cmd.Parameters.Add("@Audiologist_Phone", SqlDbType.NVarChar, -1).Value = Audiologist_Phone;
            cmd.Parameters.Add("@ENT_Name", SqlDbType.NVarChar, -1).Value = ENT_Name;
            cmd.Parameters.Add("@ENT_Date", SqlDbType.NVarChar, -1).Value = ENT_Date;
            cmd.Parameters.Add("@ENT_Addr", SqlDbType.NVarChar, -1).Value = ENT_Addr;
            cmd.Parameters.Add("@ENT_Phone", SqlDbType.NVarChar, -1).Value = ENT_Phone;
            cmd.Parameters.Add("@Chiropractor_Name", SqlDbType.NVarChar, -1).Value = Chiropractor_Name;
            cmd.Parameters.Add("@Chiropractor_Date", SqlDbType.NVarChar, -1).Value = Chiropractor_Date;
            cmd.Parameters.Add("@Chiropractor_Addr", SqlDbType.NVarChar, -1).Value = Chiropractor_Addr;
            cmd.Parameters.Add("@Chiropractor_Phone", SqlDbType.NVarChar, -1).Value = Chiropractor_Phone;

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
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            cmd.Parameters.Add("@Other_Name", SqlDbType.NVarChar, -1).Value = Other_Name;
            cmd.Parameters.Add("@Other_Date", SqlDbType.NVarChar, -1).Value = Other_Date;
            cmd.Parameters.Add("@Other_Addr", SqlDbType.NVarChar, -1).Value = Other_Addr;
            cmd.Parameters.Add("@Other_Phone", SqlDbType.NVarChar, -1).Value = Other_Phone;
            cmd.Parameters.Add("@ReleventMedicalTimeline", SqlDbType.NVarChar, -1).Value = ReleventMedicalTimeline;
            cmd.Parameters.Add("@DailyRoutine", SqlDbType.NVarChar, -1).Value = DailyRoutine;

            cmd.Parameters.Add("@DiagnosisID", SqlDbType.NVarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.NVarChar, -1).Value = DiagnosisOther;

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

        public DataTable Search_New_Report(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Export_PreConsultMst_Get_doctorwise"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable Search_New_Report_Patient(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Export_PreConsultMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
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
    }
}
