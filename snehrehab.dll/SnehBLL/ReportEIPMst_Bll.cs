using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class ReportEIPMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportEIPMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public bool IsValid(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 5;


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
            SqlCommand cmd = new SqlCommand("ReportEIPMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
            SqlCommand cmd = new SqlCommand("ReportEIPMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }

        public int Set(int _appointmentID, string DataCollection_EDD, string DataCollection_CGA, string BirthHistory_NC_SecDelivery, string BirthHistory_PreNatal_MaterialHistory,
            string BirthHistory_Natal, string BirthHistory_PostNatal, string Observation_Autonomic_HR, string Observation_Autonomic_TypesOfRespiration,
            string Observation_Autonomic_SkinColour, string Observation_Autonomic_TemperatureCentral_Peripheral, string Observation_Motor, string Observation_UpperLimbLevel1,
            string Observation_UpperLimbLevel2, string Observation_UpperLimbLevel3, string Observation_UpperLimbLeft1, string Observation_UpperLimbLeft2, string Observation_UpperLimbLeft3,
            string Observation_UpperLimbRight1, string Observation_UpperLimbRight2, string Observation_UpperLimbRight3,

            string Observation_LowerLimbLevel1, string Observation_LowerLimbLevel2, string Observation_LowerLimbLevel3,
            string Observation_LowerLimbLeft1, string Observation_LowerLimbLeft2, string Observation_LowerLimbLeft3,
            string Observation_LowerLimbRight1, string Observation_LowerLimbRight2, string Observation_LowerLimbRight3,
            string Observation_Trunk, string Observation_GeneralPosture, string Observation_SocialInteraction_Responsivity,
            string Observation_Feeding, string Observation_Participation, string Observation_Participation_Restriction,
            string Examination_Ballards1, string Examination_Ballards2, string Examination_Ballards3,
            string Examination_Ballards4, string Examination_Ballards5, string Examination_Ballards6,
            string Examination_Ballards7, string Examination_Ballards8, string Examination_Ballards9,
            string Examination_Ballards10, string Examination_Ballards11, string Examination_Ballards12,
            string Examination_Timp1, string Examination_Timp2, string Examination_Timp3,
            string Examination_Timp4, string Examination_Timp5, string Examination_Timp6,
            string Examination_Timp7, string Examination_Timp8, string Examination_Voitas1,
            string Examination_Voitas2, string Examination_Voitas3, string Examination_Voitas4,
            string Examination_Voitas5, string Examination_Voitas6, string Examination_Voitas7,

            string Examination_Voitas8, string Examination_GoalsOf_Treatment,
            string Examination_TreatmentGiven,
            int Doctor_Physioptherapist, int Doctor_Occupational, int Doctor_EnterReport, 
            bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int _loginID)
        {
            SqlCommand cmd = new SqlCommand("ReportEIPMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@DataCollection_EDD", SqlDbType.VarChar, -1).Value = DataCollection_EDD;
            cmd.Parameters.Add("@DataCollection_CGA", SqlDbType.VarChar, -1).Value = DataCollection_CGA;
            cmd.Parameters.Add("@BirthHistory_NC_SecDelivery", SqlDbType.VarChar, -1).Value = BirthHistory_NC_SecDelivery;
            cmd.Parameters.Add("@BirthHistory_PreNatal_MaterialHistory", SqlDbType.VarChar, -1).Value = BirthHistory_PreNatal_MaterialHistory;
            cmd.Parameters.Add("@BirthHistory_Natal", SqlDbType.VarChar, -1).Value = BirthHistory_Natal;
            cmd.Parameters.Add("@BirthHistory_PostNatal", SqlDbType.VarChar, -1).Value = BirthHistory_PostNatal;
            cmd.Parameters.Add("@Observation_Autonomic_HR", SqlDbType.VarChar, -1).Value = Observation_Autonomic_HR;
            cmd.Parameters.Add("@Observation_Autonomic_TypesOfRespiration", SqlDbType.VarChar, -1).Value = Observation_Autonomic_TypesOfRespiration;
            cmd.Parameters.Add("@Observation_Autonomic_SkinColour", SqlDbType.VarChar, -1).Value = Observation_Autonomic_SkinColour;
            cmd.Parameters.Add("@Observation_Autonomic_TemperatureCentral_Peripheral", SqlDbType.VarChar, -1).Value = Observation_Autonomic_TemperatureCentral_Peripheral;
            cmd.Parameters.Add("@Observation_Motor", SqlDbType.VarChar, -1).Value = Observation_Motor;
            cmd.Parameters.Add("@Observation_UpperLimbLevel1", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLevel1;
            cmd.Parameters.Add("@Observation_UpperLimbLevel2", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLevel2;
            cmd.Parameters.Add("@Observation_UpperLimbLevel3", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLevel3;
            cmd.Parameters.Add("@Observation_UpperLimbLeft1", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLeft1;
            cmd.Parameters.Add("@Observation_UpperLimbLeft2", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLeft2;
            cmd.Parameters.Add("@Observation_UpperLimbLeft3", SqlDbType.VarChar, -1).Value = Observation_UpperLimbLeft3;
            cmd.Parameters.Add("@Observation_UpperLimbRight1", SqlDbType.VarChar, -1).Value = Observation_UpperLimbRight1;
            cmd.Parameters.Add("@Observation_UpperLimbRight2", SqlDbType.VarChar, -1).Value = Observation_UpperLimbRight2;
            cmd.Parameters.Add("@Observation_UpperLimbRight3", SqlDbType.VarChar, -1).Value = Observation_UpperLimbRight3;

            cmd.Parameters.Add("@Observation_LowerLimbLevel1", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLevel1;
            cmd.Parameters.Add("@Observation_LowerLimbLevel2", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLevel2;
            cmd.Parameters.Add("@Observation_LowerLimbLevel3", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLevel3;

            cmd.Parameters.Add("@Observation_LowerLimbLeft1", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLeft1;
            cmd.Parameters.Add("@Observation_LowerLimbLeft2", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLeft2;
            cmd.Parameters.Add("@Observation_LowerLimbLeft3", SqlDbType.VarChar, -1).Value = Observation_LowerLimbLeft3;

            cmd.Parameters.Add("@Observation_LowerLimbRight1", SqlDbType.VarChar, -1).Value = Observation_LowerLimbRight1;
            cmd.Parameters.Add("@Observation_LowerLimbRight2", SqlDbType.VarChar, -1).Value = Observation_LowerLimbRight2;
            cmd.Parameters.Add("@Observation_LowerLimbRight3", SqlDbType.VarChar, -1).Value = Observation_LowerLimbRight3;

            cmd.Parameters.Add("@Observation_Trunk", SqlDbType.VarChar, -1).Value = Observation_Trunk;
            cmd.Parameters.Add("@Observation_GeneralPosture", SqlDbType.VarChar, -1).Value = Observation_GeneralPosture;
            cmd.Parameters.Add("@Observation_SocialInteraction_Responsivity", SqlDbType.VarChar, -1).Value = Observation_SocialInteraction_Responsivity;

            cmd.Parameters.Add("@Observation_Feeding", SqlDbType.VarChar, -1).Value = Observation_Feeding;
            cmd.Parameters.Add("@Observation_Participation", SqlDbType.VarChar, -1).Value = Observation_Participation;
            cmd.Parameters.Add("@Observation_Participation_Restriction", SqlDbType.VarChar, -1).Value = Observation_Participation_Restriction;

            cmd.Parameters.Add("@Examination_Ballards1", SqlDbType.VarChar, -1).Value = Examination_Ballards1;
            cmd.Parameters.Add("@Examination_Ballards2", SqlDbType.VarChar, -1).Value = Examination_Ballards2;
            cmd.Parameters.Add("@Examination_Ballards3", SqlDbType.VarChar, -1).Value = Examination_Ballards3;

            cmd.Parameters.Add("@Examination_Ballards4", SqlDbType.VarChar, -1).Value = Examination_Ballards4;
            cmd.Parameters.Add("@Examination_Ballards5", SqlDbType.VarChar, -1).Value = Examination_Ballards5;
            cmd.Parameters.Add("@Examination_Ballards6", SqlDbType.VarChar, -1).Value = Examination_Ballards6;

            cmd.Parameters.Add("@Examination_Ballards7", SqlDbType.VarChar, -1).Value = Examination_Ballards7;
            cmd.Parameters.Add("@Examination_Ballards8", SqlDbType.VarChar, -1).Value = Examination_Ballards8;
            cmd.Parameters.Add("@Examination_Ballards9", SqlDbType.VarChar, -1).Value = Examination_Ballards9;

            cmd.Parameters.Add("@Examination_Ballards10", SqlDbType.VarChar, -1).Value = Examination_Ballards10;
            cmd.Parameters.Add("@Examination_Ballards11", SqlDbType.VarChar, -1).Value = Examination_Ballards11;
            cmd.Parameters.Add("@Examination_Ballards12", SqlDbType.VarChar, -1).Value = Examination_Ballards12;

            cmd.Parameters.Add("@Examination_Timp1", SqlDbType.VarChar, -1).Value = Examination_Timp1;
            cmd.Parameters.Add("@Examination_Timp2", SqlDbType.VarChar, -1).Value = Examination_Timp2;
            cmd.Parameters.Add("@Examination_Timp3", SqlDbType.VarChar, -1).Value = Examination_Timp3;

            cmd.Parameters.Add("@Examination_Timp4", SqlDbType.VarChar, -1).Value = Examination_Timp4;
            cmd.Parameters.Add("@Examination_Timp5", SqlDbType.VarChar, -1).Value = Examination_Timp5;
            cmd.Parameters.Add("@Examination_Timp6", SqlDbType.VarChar, -1).Value = Examination_Timp6;

            cmd.Parameters.Add("@Examination_Timp7", SqlDbType.VarChar, -1).Value = Examination_Timp7;
            cmd.Parameters.Add("@Examination_Timp8", SqlDbType.VarChar, -1).Value = Examination_Timp8;
            cmd.Parameters.Add("@Examination_Voitas1", SqlDbType.VarChar, -1).Value = Examination_Voitas1;

            cmd.Parameters.Add("@Examination_Voitas2", SqlDbType.VarChar, -1).Value = Examination_Voitas2;
            cmd.Parameters.Add("@Examination_Voitas3", SqlDbType.VarChar, -1).Value = Examination_Voitas3;
            cmd.Parameters.Add("@Examination_Voitas4", SqlDbType.VarChar, -1).Value = Examination_Voitas4;

            cmd.Parameters.Add("@Examination_Voitas5", SqlDbType.VarChar, -1).Value = Examination_Voitas5;
            cmd.Parameters.Add("@Examination_Voitas6", SqlDbType.VarChar, -1).Value = Examination_Voitas6;
            cmd.Parameters.Add("@Examination_Voitas7", SqlDbType.VarChar, -1).Value = Examination_Voitas7;

            cmd.Parameters.Add("@Examination_Voitas8", SqlDbType.VarChar, -1).Value = Examination_Voitas8;
            cmd.Parameters.Add("@Examination_GoalsOf_Treatment", SqlDbType.VarChar, -1).Value = Examination_GoalsOf_Treatment;
            cmd.Parameters.Add("@Examination_TreatmentGiven", SqlDbType.VarChar, -1).Value = Examination_TreatmentGiven;

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
