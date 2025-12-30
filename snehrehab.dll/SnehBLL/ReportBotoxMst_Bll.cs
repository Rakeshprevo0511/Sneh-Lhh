using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class ReportBotoxMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportBotoxMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public bool IsValid(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 3;


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
            SqlCommand cmd = new SqlCommand("ReportBotoxMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
            SqlCommand cmd = new SqlCommand("Demo_ReportBotoxMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable Delivery_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DeliveryID", typeof(int));
            dt.Columns.Add("Delivery", typeof(string));

            dt.Rows.Add(1, "Normal");
            dt.Rows.Add(2, "Vertex");
            dt.Rows.Add(3, "Breech");
            dt.Rows.Add(4, "LSCS");
            dt.Rows.Add(5, "Preterm");
            dt.Rows.Add(6, "Twin");


            return dt;
        }

        public string Delivery_Get(int _deliveryID)
        {
            string _delivery = "";
            foreach (DataRow dr in Delivery_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["DeliveryID"].ToString(), out _tmp);
                if (_tmp == _deliveryID)
                {
                    _delivery = dr["Delivery"].ToString();
                    break;
                }
            }
            return _delivery;
        }

        public int _milestonesTypeID = 1;

        public DataTable Milestones_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MilestonesID", typeof(int));
            dt.Columns.Add("Milestones", typeof(string));

            dt.Rows.Add(1, "Head Holding");
            dt.Rows.Add(2, "Sitting");
            dt.Rows.Add(3, "Standing");
            dt.Rows.Add(4, "Walking");

            return dt;
        }

        public string Milestones_Get(int _milestonesID)
        {
            string _milestones = "";
            foreach (DataRow dr in Milestones_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["MilestonesID"].ToString(), out _tmp);
                if (_tmp == _milestonesID)
                {
                    _milestones = dr["Milestones"].ToString();
                    break;
                }
            }
            return _milestones;
        }

        public DataTable Diagnosed_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DiagnosedID", typeof(int));
            dt.Columns.Add("Diagnosed", typeof(string));

            dt.Rows.Add(1, "GP");
            dt.Rows.Add(2, "Paediatrician");
            dt.Rows.Add(3, "Ortho");
            dt.Rows.Add(4, "Paed Ortho");
            dt.Rows.Add(5, "PT");
            dt.Rows.Add(6, "Parents");


            return dt;
        }

        public string Diagnosed_Get(int _diagnosedID)
        {
            string _diagnosed = "";
            foreach (DataRow dr in Diagnosed_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["DiagnosedID"].ToString(), out _tmp);
                if (_tmp == _diagnosedID)
                {
                    _diagnosed = dr["Diagnosed"].ToString();
                    break;
                }
            }
            return _diagnosed;
        }

        public DataTable TypeOfCP_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeOfCPID", typeof(int));
            dt.Columns.Add("TypeOfCP", typeof(string));

            dt.Rows.Add(1, "Hemiplegia");
            dt.Rows.Add(2, "Diplegia");
            dt.Rows.Add(3, "Quadriplegia");
            dt.Rows.Add(4, "Monoplegia");
            dt.Rows.Add(5, "Athetoid");

            return dt;
        }

        public string TypeOfCP_Get(int _typeOfCPID)
        {
            string _typeOfCP = "";
            foreach (DataRow dr in TypeOfCP_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["TypeOfCPID"].ToString(), out _tmp);
                if (_tmp == _typeOfCPID)
                {
                    _typeOfCP = dr["TypeOfCP"].ToString();
                    break;
                }
            }
            return _typeOfCP;
        }

        public DataTable Orthotics_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrthoticsID", typeof(int));
            dt.Columns.Add("Orthotics", typeof(string));

            dt.Rows.Add(1, "Left");
            dt.Rows.Add(2, "Right");
            dt.Rows.Add(3, "Bilateral");

            return dt;
        }

        public string Orthotics_Get(int _orthoticsID)
        {
            string _orthotics = "";
            foreach (DataRow dr in Orthotics_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["OrthoticsID"].ToString(), out _tmp);
                if (_tmp == _orthoticsID)
                {
                    _orthotics = dr["Orthotics"].ToString();
                    break;
                }
            }
            return _orthotics;
        }

        public int _assistiveDevicesTypeID = 2;

        public DataTable AssistiveDevices_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AssistiveDevicesID", typeof(int));
            dt.Columns.Add("AssistiveDevices", typeof(string));

            dt.Rows.Add(1, "No Device");
            dt.Rows.Add(2, "Cane/Single Crutch");
            dt.Rows.Add(3, "Bilcrutches");
            dt.Rows.Add(4, "Walker");
            dt.Rows.Add(5, "Wheelchair");
            dt.Rows.Add(6, "Bedridden");

            return dt;
        }

        public string AssistiveDevices_Get(int _assistiveDevicesID)
        {
            string _assistiveDevices = "";
            foreach (DataRow dr in AssistiveDevices_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["AssistiveDevicesID"].ToString(), out _tmp);
                if (_tmp == _assistiveDevicesID)
                {
                    _assistiveDevices = dr["AssistiveDevices"].ToString();
                    break;
                }
            }
            return _assistiveDevices;
        }

        public int _orthoticsDevicesTypeID = 3;

        public DataTable OrthoticsDevices_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrthoticsDevicesID", typeof(int));
            dt.Columns.Add("OrthoticsDevices", typeof(string));

            dt.Rows.Add(1, "No Braces");
            dt.Rows.Add(2, "Arch Support");
            dt.Rows.Add(3, "SMO");
            dt.Rows.Add(4, "Rigid AFO");
            dt.Rows.Add(5, "Articulating AFO");
            dt.Rows.Add(6, "FRO");
            dt.Rows.Add(7, "KAFO");
            dt.Rows.Add(8, "TLSO");
            dt.Rows.Add(9, "UE Brace");

            return dt;
        }

        public string OrthoticsDevices_Get(int _orthoticsDevicesID)
        {
            string _orthoticsDevices = "";
            foreach (DataRow dr in OrthoticsDevices_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["OrthoticsDevicesID"].ToString(), out _tmp);
                if (_tmp == _orthoticsDevicesID)
                {
                    _orthoticsDevices = dr["OrthoticsDevices"].ToString();
                    break;
                }
            }
            return _orthoticsDevices;
        }

        public DataTable ADL_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ADLID", typeof(int));
            dt.Columns.Add("ADL", typeof(string));

            dt.Rows.Add(1, "Independent(I)");
            dt.Rows.Add(2, "Partially Dependent(P)");
            dt.Rows.Add(3, "Dependent(D)");

            return dt;
        }

        public string ADL_Get(int _ADLID)
        {
            string _ADL = "";
            foreach (DataRow dr in ADL_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["ADLID"].ToString(), out _tmp);
                if (_tmp == _ADLID)
                {
                    _ADL = dr["ADL"].ToString();
                    break;
                }
            }
            return _ADL;
        }

        public int _ADLListTypeID = 4;

        public DataTable ADLList_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ADLListID", typeof(int));
            dt.Columns.Add("ADLList", typeof(string));

            dt.Rows.Add(1, "Eating");
            dt.Rows.Add(2, "Dressing");
            dt.Rows.Add(3, "Toileting");

            return dt;
        }

        public string ADLList_Get(int _ADLListID)
        {
            string _ADLList = "";
            foreach (DataRow dr in ADLList_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["ADLListID"].ToString(), out _tmp);
                if (_tmp == _ADLListID)
                {
                    _ADLList = dr["ADLList"].ToString();
                    break;
                }
            }
            return _ADLList;
        }

        public int _indicationForBotoxTypeID = 5;

        public DataTable IndicationForBotox_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IndicationForBotoxID", typeof(int));
            dt.Columns.Add("IndicationForBotox", typeof(string));

            dt.Rows.Add(1, "Spasticity Interfering with Ambulation");
            dt.Rows.Add(2, "Spasticity Interfering Perineal Care / Hygiene");
            dt.Rows.Add(3, "Spasticity Not Responding to PT / Casts");
            dt.Rows.Add(4, "Post Surgical Relapse");
            dt.Rows.Add(5, "Post Surgical New Target Area");
            dt.Rows.Add(6, "Second Decade Botox");

            return dt;
        }

        public string IndicationForBotox_Get(int _indicationForBotoxID)
        {
            string _indicationForBotox = "";
            foreach (DataRow dr in IndicationForBotox_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["IndicationForBotoxID"].ToString(), out _tmp);
                if (_tmp == _indicationForBotoxID)
                {
                    _indicationForBotox = dr["IndicationForBotox"].ToString();
                    break;
                }
            }
            return _indicationForBotox;
        }

        public int _ancillaryTreatmentTypeID = 6;

        public DataTable AncillaryTreatment_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AncillaryTreatmentID", typeof(int));
            dt.Columns.Add("AncillaryTreatment", typeof(string));

            dt.Rows.Add(1, "PT");
            dt.Rows.Add(2, "Casting");
            dt.Rows.Add(3, "Orthotics");

            return dt;
        }

        public string AncillaryTreatment_Get(int _ancillaryTreatmentID)
        {
            string _ancillaryTreatment = "";
            foreach (DataRow dr in AncillaryTreatment_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["AncillaryTreatmentID"].ToString(), out _tmp);
                if (_tmp == _ancillaryTreatmentID)
                {
                    _ancillaryTreatment = dr["AncillaryTreatment"].ToString();
                    break;
                }
            }
            return _ancillaryTreatment;
        }

        public int _SideEffectsTypeID = 7;

        public DataTable SideEffects_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SideEffectsID", typeof(int));
            dt.Columns.Add("SideEffects", typeof(string));

            dt.Rows.Add(1, "Weakness");
            dt.Rows.Add(2, "Lethargy");
            dt.Rows.Add(3, "Fever");
            dt.Rows.Add(4, "Pain at Injection Site");
            dt.Rows.Add(5, "Constipation");
            dt.Rows.Add(6, "Incontinence");

            return dt;
        }

        public string SideEffects_Get(int _sideEffectsID)
        {
            string _sideEffects = "";
            foreach (DataRow dr in SideEffects_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["SideEffectsID"].ToString(), out _tmp);
                if (_tmp == _sideEffectsID)
                {
                    _sideEffects = dr["SideEffects"].ToString();
                    break;
                }
            }
            return _sideEffects;
        }

        public int DeleteAttr(int _appointmentID, int _attributeTypeID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxChld_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;

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

        public int SetAttr(int _appointmentID, int _attributeTypeID, int _attributeID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxChld_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;
            cmd.Parameters.Add("@AttributeID", SqlDbType.Int).Value = _attributeID;

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

        public DataTable GetAttr(int _appointmentID, int _attributeTypeID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxChld_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;

            return db.DbRead(cmd);
        }

        public DataTable GetTop(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public DataTable Get1(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst1_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public DataTable Get2(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst2_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public DataTable Get3(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst3_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public int Set1(
            int _appointmentID, string General_BotoxNo, int General_Pediatrician, int General_Therapist, int HistoryExam_Delivery, string HistoryExam_PerinatalComplications, string HistoryExam_BirthWeight, int HistoryExam_DiagnosedBy, int TypeOfCP_CPID,
            int AssistiveDevices_Orthotics, int ADL_adlID, DateTime Ambulation_Date1, DateTime Ambulation_Date2, DateTime Ambulation_Date3, DateTime Ambulation_Date4, DateTime Ambulation_Date5, DateTime Ambulation_Date6,
            string Ambulation_Amb1, string Ambulation_Amb2, string Ambulation_Amb3, string Ambulation_Amb4, string Ambulation_Amb5, string Ambulation_Amb6, DateTime PreExisting_Date1, DateTime PreExisting_Date2,
            DateTime PreExisting_Date3, DateTime PreExisting_Date4, string PreExisting_HipFID_1R, string PreExisting_HipFID_1L, string PreExisting_HipFID_2R, string PreExisting_HipFID_2L, string PreExisting_HipFID_3R,
            string PreExisting_HipFID_3L, string PreExisting_HipFID_4R, string PreExisting_HipFID_4L, string PreExisting_HipAdduction_1R, string PreExisting_HipAdduction_1L, string PreExisting_HipAdduction_2R,
            string PreExisting_HipAdduction_2L, string PreExisting_HipAdduction_3R, string PreExisting_HipAdduction_3L, string PreExisting_HipAdduction_4R,
            string PreExisting_HipAdduction_4L, string PreExisting_KneeFFD_1R, string PreExisting_KneeFFD_1L, string PreExisting_KneeFFD_2R, string PreExisting_KneeFFD_2L, string PreExisting_KneeFFD_3R,
            string PreExisting_KneeFFD_3L, string PreExisting_KneeFFD_4R, string PreExisting_KneeFFD_4L, string PreExisting_Equinus_1R, string PreExisting_Equinus_1L, string PreExisting_Equinus_2R,
            string PreExisting_Equinus_2L, string PreExisting_Equinus_3R, string PreExisting_Equinus_3L, string PreExisting_Equinus_4R, string PreExisting_Equinus_4L, string PreExisting_Planovalgoid_1R, string
            PreExisting_Planovalgoid_1L, string PreExisting_Planovalgoid_2R, string PreExisting_Planovalgoid_2L, string PreExisting_Planovalgoid_3R, string PreExisting_Planovalgoid_3L, string
            PreExisting_Planovalgoid_4R, string PreExisting_Planovalgoid_4L, string PreExisting_Cavovarus_1R, string PreExisting_Cavovarus_1L, string PreExisting_Cavovarus_2R,
            string PreExisting_Cavovarus_2L, string PreExisting_Cavovarus_3R, string PreExisting_Cavovarus_3L, string PreExisting_Cavovarus_4R, string PreExisting_Cavovarus_4L, string
            PreExisting_ElbowFFD_1R, string PreExisting_ElbowFFD_1L, string PreExisting_ElbowFFD_2R, string PreExisting_ElbowFFD_2L, string PreExisting_ElbowFFD_3R,
            string PreExisting_ElbowFFD_3L, string PreExisting_ElbowFFD_4R, string PreExisting_ElbowFFD_4L, string PreExisting_WristFlexPron_1R, string PreExisting_WristFlexPron_1L,
            string PreExisting_WristFlexPron_2R, string PreExisting_WristFlexPron_2L, string PreExisting_WristFlexPron_3R, string PreExisting_WristFlexPron_3L, string PreExisting_WristFlexPron_4R, string
            PreExisting_WristFlexPron_4L, DateTime PassiveROM_Date1, DateTime PassiveROM_Date2, DateTime PassiveROM_Date3, DateTime PassiveROM_Date4, string PassiveROM_HipFlexion_1R,
            string PassiveROM_HipFlexion_1L, string PassiveROM_HipFlexion_2R, string PassiveROM_HipFlexion_2L, string PassiveROM_HipFlexion_3R, string PassiveROM_HipFlexion_3L,
            string PassiveROM_HipFlexion_4R, string PassiveROM_HipFlexion_4L, string PassiveROM_HipAbduction_1R, string PassiveROM_HipAbduction_1L, string PassiveROM_HipAbduction_2R, string
            PassiveROM_HipAbduction_2L, string PassiveROM_HipAbduction_3R, string PassiveROM_HipAbduction_3L, string PassiveROM_HipAbduction_4R, string PassiveROM_HipAbduction_4L,
            string PassiveROM_HipIR_1R, string PassiveROM_HipIR_1L, string PassiveROM_HipIR_2R, string PassiveROM_HipIR_2L, string PassiveROM_HipIR_3R, string PassiveROM_HipIR_3L,
            string PassiveROM_HipIR_4R, string PassiveROM_HipIR_4L, string PassiveROM_HipER_1R, string PassiveROM_HipER_1L, string PassiveROM_HipER_2R, string PassiveROM_HipER_2L,
            string PassiveROM_HipER_3R, string PassiveROM_HipER_3L, string PassiveROM_HipER_4R, string PassiveROM_HipER_4L, string PassiveROM_KneeFlexion_1R, string
            PassiveROM_KneeFlexion_1L, string PassiveROM_KneeFlexion_2R, string PassiveROM_KneeFlexion_2L, string PassiveROM_KneeFlexion_3R, string PassiveROM_KneeFlexion_3L, string
            PassiveROM_KneeFlexion_4R, string PassiveROM_KneeFlexion_4L, string PassiveROM_PoplitealAngle_1R, string PassiveROM_PoplitealAngle_1L, string PassiveROM_PoplitealAngle_2R,
            string PassiveROM_PoplitealAngle_2L, string PassiveROM_PoplitealAngle_3R, string PassiveROM_PoplitealAngle_3L, string PassiveROM_PoplitealAngle_4R, string
            PassiveROM_PoplitealAngle_4L, string PassiveROM_KneeExt_1R, string PassiveROM_KneeExt_1L, string PassiveROM_KneeExt_2R, string PassiveROM_KneeExt_2L,
            string PassiveROM_KneeExt_3R, string PassiveROM_KneeExt_3L, string PassiveROM_KneeExt_4R, string PassiveROM_KneeExt_4L, string PassiveROM_KneeFlex_1R,
            string PassiveROM_KneeFlex_1L, string PassiveROM_KneeFlex_2R, string PassiveROM_KneeFlex_2L, string PassiveROM_KneeFlex_3R, string PassiveROM_KneeFlex_3L,
            string PassiveROM_KneeFlex_4R, string PassiveROM_KneeFlex_4L, string PassiveROM_Plantarflexion_1R, string PassiveROM_Plantarflexion_1L, string PassiveROM_Plantarflexion_2R,
            string PassiveROM_Plantarflexion_2L, string PassiveROM_Plantarflexion_3R, string PassiveROM_Plantarflexion_3L, string PassiveROM_Plantarflexion_4R,
            string PassiveROM_Plantarflexion_4L, string PassiveROM_AnkleInv_1R, string PassiveROM_AnkleInv_1L, string PassiveROM_AnkleInv_2R, string PassiveROM_AnkleInv_2L,
            string PassiveROM_AnkleInv_3R, string PassiveROM_AnkleInv_3L, string PassiveROM_AnkleInv_4R, string PassiveROM_AnkleInv_4L, string PassiveROM_AnkleEver_1R,
            string PassiveROM_AnkleEver_1L, string PassiveROM_AnkleEver_2R, string PassiveROM_AnkleEver_2L, string PassiveROM_AnkleEver_3R, string PassiveROM_AnkleEver_3L,
            string PassiveROM_AnkleEver_4R, string PassiveROM_AnkleEver_4L, DateTime Tone_Date1, DateTime Tone_Date2, DateTime Tone_Date3, DateTime Tone_Date4, string Tone_Iliopsoas_1R, string Tone_Iliopsoas_1L,
            string Tone_Iliopsoas_2R, string Tone_Iliopsoas_2L, string Tone_Iliopsoas_3R, string Tone_Iliopsoas_3L, string Tone_Iliopsoas_4R, string Tone_Iliopsoas_4L, string Tone_Adductors_1R, string Tone_Adductors_1L,
            string Tone_Adductors_2R, string Tone_Adductors_2L, string Tone_Adductors_3R, string Tone_Adductors_3L, string Tone_Adductors_4R, string Tone_Adductors_4L, string Tone_RectusFemoris_1R, string
            Tone_RectusFemoris_1L, string Tone_RectusFemoris_2R, string Tone_RectusFemoris_2L, string Tone_RectusFemoris_3R, string Tone_RectusFemoris_3L, string Tone_RectusFemoris_4R, string
            Tone_RectusFemoris_4L, string Tone_Hamstrings_1R, string Tone_Hamstrings_1L, string Tone_Hamstrings_2R, string Tone_Hamstrings_2L, string Tone_Hamstrings_3R, string Tone_Hamstrings_3L,
            string Tone_Hamstrings_4R, string Tone_Hamstrings_4L, string Tone_Gastrosoleus_1R, string Tone_Gastrosoleus_1L, string Tone_Gastrosoleus_2R, string Tone_Gastrosoleus_2L, string
            Tone_Gastrosoleus_3R, string Tone_Gastrosoleus_3L, string Tone_Gastrosoleus_4R, string Tone_Gastrosoleus_4L, string Tone_ElbowFlexors_1R, string Tone_ElbowFlexors_1L,
            string Tone_ElbowFlexors_2R, string Tone_ElbowFlexors_2L, string Tone_ElbowFlexors_3R, string Tone_ElbowFlexors_3L, string Tone_ElbowFlexors_4R, string Tone_ElbowFlexors_4L,
            string Tone_WristFlexors_1R, string Tone_WristFlexors_1L, string Tone_WristFlexors_2R, string Tone_WristFlexors_2L, string Tone_WristFlexors_3R, string Tone_WristFlexors_3L,
            string Tone_WristFlexors_4R, string Tone_WristFlexors_4L, string Tone_FingerFlexors_1R, string Tone_FingerFlexors_1L, string Tone_FingerFlexors_2R, string Tone_FingerFlexors_2L,
            string Tone_FingerFlexors_3R, string Tone_FingerFlexors_3L, string Tone_FingerFlexors_4R, string Tone_FingerFlexors_4L, string Tone_PronatorFlexors_1R, string Tone_PronatorFlexors_1L,
            string Tone_PronatorFlexors_2R, string Tone_PronatorFlexors_2L, string Tone_PronatorFlexors_3R, string Tone_PronatorFlexors_3L, string Tone_PronatorFlexors_4R, string Tone_PronatorFlexors_4L,
            DateTime TardieusScale_Date1, DateTime TardieusScale_Date2, DateTime TardieusScale_Date3, DateTime TardieusScale_Date4, string TardieusScale_GastrosoleusR1_1R, string TardieusScale_GastrosoleusR1_1L,
            string TardieusScale_GastrosoleusR1_2R, string TardieusScale_GastrosoleusR1_2L, string TardieusScale_GastrosoleusR1_3R, string TardieusScale_GastrosoleusR1_3L,
            string TardieusScale_GastrosoleusR1_4R, string TardieusScale_GastrosoleusR1_4L, string TardieusScale_GastrosoleusR2_1R, string TardieusScale_GastrosoleusR2_1L,
            string TardieusScale_GastrosoleusR2_2R, string TardieusScale_GastrosoleusR2_2L, string TardieusScale_GastrosoleusR2_3R, string TardieusScale_GastrosoleusR2_3L,
            string TardieusScale_GastrosoleusR2_4R, string TardieusScale_GastrosoleusR2_4L, string TardieusScale_GastrosoleusR3_1R, string TardieusScale_GastrosoleusR3_1L,
            string TardieusScale_GastrosoleusR3_2R, string TardieusScale_GastrosoleusR3_2L, string TardieusScale_GastrosoleusR3_3R, string TardieusScale_GastrosoleusR3_3L,
            string TardieusScale_GastrosoleusR3_4R, string TardieusScale_GastrosoleusR3_4L, string TardieusScale_HamstringsR1_1R, string TardieusScale_HamstringsR1_1L,
            string TardieusScale_HamstringsR1_2R, string TardieusScale_HamstringsR1_2L, string TardieusScale_HamstringsR1_3R, string TardieusScale_HamstringsR1_3L,
            string TardieusScale_HamstringsR1_4R, string TardieusScale_HamstringsR1_4L, string TardieusScale_HamstringsR2_1R, string TardieusScale_HamstringsR2_1L,
            string TardieusScale_HamstringsR2_2R, string TardieusScale_HamstringsR2_2L, string TardieusScale_HamstringsR2_3R, string TardieusScale_HamstringsR2_3L,
            string TardieusScale_HamstringsR2_4R, string TardieusScale_HamstringsR2_4L, string DiagnosisIDs, string DiagnosisOther)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst1_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@General_BotoxNo", SqlDbType.VarChar, 500).Value = General_BotoxNo;
            cmd.Parameters.Add("@General_Pediatrician", SqlDbType.Int).Value = General_Pediatrician;
            cmd.Parameters.Add("@General_Therapist", SqlDbType.Int).Value = General_Therapist;
            cmd.Parameters.Add("@HistoryExam_Delivery", SqlDbType.Int).Value = HistoryExam_Delivery;
            cmd.Parameters.Add("@HistoryExam_PerinatalComplications", SqlDbType.VarChar, -1).Value = HistoryExam_PerinatalComplications;
            cmd.Parameters.Add("@HistoryExam_BirthWeight", SqlDbType.VarChar, 1000).Value = HistoryExam_BirthWeight;
            cmd.Parameters.Add("@HistoryExam_DiagnosedBy", SqlDbType.Int).Value = HistoryExam_DiagnosedBy;
            cmd.Parameters.Add("@TypeOfCP_CPID", SqlDbType.Int).Value = TypeOfCP_CPID;
            cmd.Parameters.Add("@AssistiveDevices_Orthotics", SqlDbType.Int).Value = AssistiveDevices_Orthotics;
            cmd.Parameters.Add("@ADL_adlID", SqlDbType.Int).Value = ADL_adlID;
            if (Ambulation_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date1", SqlDbType.DateTime).Value = Ambulation_Date1;
            else
                cmd.Parameters.Add("@Ambulation_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (Ambulation_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date2", SqlDbType.DateTime).Value = Ambulation_Date2;
            else
                cmd.Parameters.Add("@Ambulation_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (Ambulation_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date3", SqlDbType.DateTime).Value = Ambulation_Date3;
            else
                cmd.Parameters.Add("@Ambulation_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (Ambulation_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date4", SqlDbType.DateTime).Value = Ambulation_Date4;
            else
                cmd.Parameters.Add("@Ambulation_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            if (Ambulation_Date5 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date5", SqlDbType.DateTime).Value = Ambulation_Date5;
            else
                cmd.Parameters.Add("@Ambulation_Date5", SqlDbType.DateTime).Value = DBNull.Value;
            if (Ambulation_Date6 > DateTime.MinValue)
                cmd.Parameters.Add("@Ambulation_Date6", SqlDbType.DateTime).Value = Ambulation_Date6;
            else
                cmd.Parameters.Add("@Ambulation_Date6", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Ambulation_Amb1", SqlDbType.VarChar, 1000).Value = Ambulation_Amb1;
            cmd.Parameters.Add("@Ambulation_Amb2", SqlDbType.VarChar, 1000).Value = Ambulation_Amb2;
            cmd.Parameters.Add("@Ambulation_Amb3", SqlDbType.VarChar, 1000).Value = Ambulation_Amb3;
            cmd.Parameters.Add("@Ambulation_Amb4", SqlDbType.VarChar, 1000).Value = Ambulation_Amb4;
            cmd.Parameters.Add("@Ambulation_Amb5", SqlDbType.VarChar, 1000).Value = Ambulation_Amb5;
            cmd.Parameters.Add("@Ambulation_Amb6", SqlDbType.VarChar, 1000).Value = Ambulation_Amb6;
            if (PreExisting_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@PreExisting_Date1", SqlDbType.DateTime).Value = PreExisting_Date1;
            else
                cmd.Parameters.Add("@PreExisting_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (PreExisting_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@PreExisting_Date2", SqlDbType.DateTime).Value = PreExisting_Date2;
            else
                cmd.Parameters.Add("@PreExisting_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (PreExisting_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@PreExisting_Date3", SqlDbType.DateTime).Value = PreExisting_Date3;
            else
                cmd.Parameters.Add("@PreExisting_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (PreExisting_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@PreExisting_Date4", SqlDbType.DateTime).Value = PreExisting_Date4;
            else
                cmd.Parameters.Add("@PreExisting_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PreExisting_HipFID_1R", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_1R;
            cmd.Parameters.Add("@PreExisting_HipFID_1L", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_1L;
            cmd.Parameters.Add("@PreExisting_HipFID_2R", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_2R;
            cmd.Parameters.Add("@PreExisting_HipFID_2L", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_2L;
            cmd.Parameters.Add("@PreExisting_HipFID_3R", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_3R;
            cmd.Parameters.Add("@PreExisting_HipFID_3L", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_3L;
            cmd.Parameters.Add("@PreExisting_HipFID_4R", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_4R;
            cmd.Parameters.Add("@PreExisting_HipFID_4L", SqlDbType.VarChar, 1000).Value = PreExisting_HipFID_4L;
            cmd.Parameters.Add("@PreExisting_HipAdduction_1R", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_1R;
            cmd.Parameters.Add("@PreExisting_HipAdduction_1L", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_1L;
            cmd.Parameters.Add("@PreExisting_HipAdduction_2R", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_2R;
            cmd.Parameters.Add("@PreExisting_HipAdduction_2L", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_2L;
            cmd.Parameters.Add("@PreExisting_HipAdduction_3R", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_3R;
            cmd.Parameters.Add("@PreExisting_HipAdduction_3L", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_3L;
            cmd.Parameters.Add("@PreExisting_HipAdduction_4R", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_4R;
            cmd.Parameters.Add("@PreExisting_HipAdduction_4L", SqlDbType.VarChar, 1000).Value = PreExisting_HipAdduction_4L;
            cmd.Parameters.Add("@PreExisting_KneeFFD_1R", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_1R;
            cmd.Parameters.Add("@PreExisting_KneeFFD_1L", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_1L;
            cmd.Parameters.Add("@PreExisting_KneeFFD_2R", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_2R;
            cmd.Parameters.Add("@PreExisting_KneeFFD_2L", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_2L;
            cmd.Parameters.Add("@PreExisting_KneeFFD_3R", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_3R;
            cmd.Parameters.Add("@PreExisting_KneeFFD_3L", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_3L;
            cmd.Parameters.Add("@PreExisting_KneeFFD_4R", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_4R;
            cmd.Parameters.Add("@PreExisting_KneeFFD_4L", SqlDbType.VarChar, 1000).Value = PreExisting_KneeFFD_4L;
            cmd.Parameters.Add("@PreExisting_Equinus_1R", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_1R;
            cmd.Parameters.Add("@PreExisting_Equinus_1L", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_1L;
            cmd.Parameters.Add("@PreExisting_Equinus_2R", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_2R;
            cmd.Parameters.Add("@PreExisting_Equinus_2L", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_2L;
            cmd.Parameters.Add("@PreExisting_Equinus_3R", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_3R;
            cmd.Parameters.Add("@PreExisting_Equinus_3L", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_3L;
            cmd.Parameters.Add("@PreExisting_Equinus_4R", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_4R;
            cmd.Parameters.Add("@PreExisting_Equinus_4L", SqlDbType.VarChar, 1000).Value = PreExisting_Equinus_4L;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_1R", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_1R;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_1L", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_1L;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_2R", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_2R;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_2L", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_2L;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_3R", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_3R;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_3L", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_3L;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_4R", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_4R;
            cmd.Parameters.Add("@PreExisting_Planovalgoid_4L", SqlDbType.VarChar, 1000).Value = PreExisting_Planovalgoid_4L;
            cmd.Parameters.Add("@PreExisting_Cavovarus_1R", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_1R;
            cmd.Parameters.Add("@PreExisting_Cavovarus_1L", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_1L;
            cmd.Parameters.Add("@PreExisting_Cavovarus_2R", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_2R;
            cmd.Parameters.Add("@PreExisting_Cavovarus_2L", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_2L;
            cmd.Parameters.Add("@PreExisting_Cavovarus_3R", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_3R;
            cmd.Parameters.Add("@PreExisting_Cavovarus_3L", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_3L;
            cmd.Parameters.Add("@PreExisting_Cavovarus_4R", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_4R;
            cmd.Parameters.Add("@PreExisting_Cavovarus_4L", SqlDbType.VarChar, 1000).Value = PreExisting_Cavovarus_4L;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_1R", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_1R;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_1L", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_1L;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_2R", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_2R;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_2L", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_2L;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_3R", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_3R;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_3L", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_3L;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_4R", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_4R;
            cmd.Parameters.Add("@PreExisting_ElbowFFD_4L", SqlDbType.VarChar, 1000).Value = PreExisting_ElbowFFD_4L;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_1R", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_1R;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_1L", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_1L;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_2R", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_2R;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_2L", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_2L;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_3R", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_3R;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_3L", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_3L;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_4R", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_4R;
            cmd.Parameters.Add("@PreExisting_WristFlexPron_4L", SqlDbType.VarChar, 1000).Value = PreExisting_WristFlexPron_4L;
            if (PassiveROM_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@PassiveROM_Date1", SqlDbType.DateTime).Value = PassiveROM_Date1;
            else
                cmd.Parameters.Add("@PassiveROM_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (PassiveROM_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@PassiveROM_Date2", SqlDbType.DateTime).Value = PassiveROM_Date2;
            else
                cmd.Parameters.Add("@PassiveROM_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (PassiveROM_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@PassiveROM_Date3", SqlDbType.DateTime).Value = PassiveROM_Date3;
            else
                cmd.Parameters.Add("@PassiveROM_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (PassiveROM_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@PassiveROM_Date4", SqlDbType.DateTime).Value = PassiveROM_Date4;
            else
                cmd.Parameters.Add("@PassiveROM_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_1R;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_1L;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_2R;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_2L;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_3R;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_3L;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_4R;
            cmd.Parameters.Add("@PassiveROM_HipFlexion_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipFlexion_4L;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_1R;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_1L;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_2R;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_2L;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_3R;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_3L;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_4R;
            cmd.Parameters.Add("@PassiveROM_HipAbduction_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipAbduction_4L;
            cmd.Parameters.Add("@PassiveROM_HipIR_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_1R;
            cmd.Parameters.Add("@PassiveROM_HipIR_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_1L;
            cmd.Parameters.Add("@PassiveROM_HipIR_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_2R;
            cmd.Parameters.Add("@PassiveROM_HipIR_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_2L;
            cmd.Parameters.Add("@PassiveROM_HipIR_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_3R;
            cmd.Parameters.Add("@PassiveROM_HipIR_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_3L;
            cmd.Parameters.Add("@PassiveROM_HipIR_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_4R;
            cmd.Parameters.Add("@PassiveROM_HipIR_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipIR_4L;
            cmd.Parameters.Add("@PassiveROM_HipER_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_1R;
            cmd.Parameters.Add("@PassiveROM_HipER_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_1L;
            cmd.Parameters.Add("@PassiveROM_HipER_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_2R;
            cmd.Parameters.Add("@PassiveROM_HipER_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_2L;
            cmd.Parameters.Add("@PassiveROM_HipER_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_3R;
            cmd.Parameters.Add("@PassiveROM_HipER_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_3L;
            cmd.Parameters.Add("@PassiveROM_HipER_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_4R;
            cmd.Parameters.Add("@PassiveROM_HipER_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_HipER_4L;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_1R;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_1L;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_2R;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_2L;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_3R;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_3L;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_4R;
            cmd.Parameters.Add("@PassiveROM_KneeFlexion_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlexion_4L;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_1R;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_1L;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_2R;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_2L;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_3R;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_3L;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_4R;
            cmd.Parameters.Add("@PassiveROM_PoplitealAngle_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_PoplitealAngle_4L;
            cmd.Parameters.Add("@PassiveROM_KneeExt_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_1R;
            cmd.Parameters.Add("@PassiveROM_KneeExt_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_1L;
            cmd.Parameters.Add("@PassiveROM_KneeExt_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_2R;
            cmd.Parameters.Add("@PassiveROM_KneeExt_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_2L;
            cmd.Parameters.Add("@PassiveROM_KneeExt_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_3R;
            cmd.Parameters.Add("@PassiveROM_KneeExt_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_3L;
            cmd.Parameters.Add("@PassiveROM_KneeExt_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_4R;
            cmd.Parameters.Add("@PassiveROM_KneeExt_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeExt_4L;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_1R;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_1L;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_2R;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_2L;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_3R;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_3L;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_4R;
            cmd.Parameters.Add("@PassiveROM_KneeFlex_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_KneeFlex_4L;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_1R;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_1L;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_2R;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_2L;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_3R;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_3L;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_4R;
            cmd.Parameters.Add("@PassiveROM_Plantarflexion_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_Plantarflexion_4L;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_1R;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_1L;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_2R;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_2L;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_3R;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_3L;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_4R;
            cmd.Parameters.Add("@PassiveROM_AnkleInv_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleInv_4L;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_1R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_1R;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_1L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_1L;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_2R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_2R;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_2L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_2L;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_3R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_3R;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_3L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_3L;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_4R", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_4R;
            cmd.Parameters.Add("@PassiveROM_AnkleEver_4L", SqlDbType.VarChar, 1000).Value = PassiveROM_AnkleEver_4L;
            if (Tone_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@Tone_Date1", SqlDbType.DateTime).Value = Tone_Date1;
            else
                cmd.Parameters.Add("@Tone_Date1", SqlDbType.DateTime).Value = DBNull.Value;

            if (Tone_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@Tone_Date2", SqlDbType.DateTime).Value = Tone_Date2;
            else
                cmd.Parameters.Add("@Tone_Date2", SqlDbType.DateTime).Value = DBNull.Value;

            if (Tone_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@Tone_Date3", SqlDbType.DateTime).Value = Tone_Date3;
            else
                cmd.Parameters.Add("@Tone_Date3", SqlDbType.DateTime).Value = DBNull.Value;

            if (Tone_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@Tone_Date4", SqlDbType.DateTime).Value = Tone_Date4;
            else
                cmd.Parameters.Add("@Tone_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Tone_Iliopsoas_1R", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_1R;
            cmd.Parameters.Add("@Tone_Iliopsoas_1L", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_1L;
            cmd.Parameters.Add("@Tone_Iliopsoas_2R", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_2R;
            cmd.Parameters.Add("@Tone_Iliopsoas_2L", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_2L;
            cmd.Parameters.Add("@Tone_Iliopsoas_3R", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_3R;
            cmd.Parameters.Add("@Tone_Iliopsoas_3L", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_3L;
            cmd.Parameters.Add("@Tone_Iliopsoas_4R", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_4R;
            cmd.Parameters.Add("@Tone_Iliopsoas_4L", SqlDbType.VarChar, 1000).Value = Tone_Iliopsoas_4L;
            cmd.Parameters.Add("@Tone_Adductors_1R", SqlDbType.VarChar, 1000).Value = Tone_Adductors_1R;
            cmd.Parameters.Add("@Tone_Adductors_1L", SqlDbType.VarChar, 1000).Value = Tone_Adductors_1L;
            cmd.Parameters.Add("@Tone_Adductors_2R", SqlDbType.VarChar, 1000).Value = Tone_Adductors_2R;
            cmd.Parameters.Add("@Tone_Adductors_2L", SqlDbType.VarChar, 1000).Value = Tone_Adductors_2L;
            cmd.Parameters.Add("@Tone_Adductors_3R", SqlDbType.VarChar, 1000).Value = Tone_Adductors_3R;
            cmd.Parameters.Add("@Tone_Adductors_3L", SqlDbType.VarChar, 1000).Value = Tone_Adductors_3L;
            cmd.Parameters.Add("@Tone_Adductors_4R", SqlDbType.VarChar, 1000).Value = Tone_Adductors_4R;
            cmd.Parameters.Add("@Tone_Adductors_4L", SqlDbType.VarChar, 1000).Value = Tone_Adductors_4L;
            cmd.Parameters.Add("@Tone_RectusFemoris_1R", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_1R;
            cmd.Parameters.Add("@Tone_RectusFemoris_1L", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_1L;
            cmd.Parameters.Add("@Tone_RectusFemoris_2R", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_2R;
            cmd.Parameters.Add("@Tone_RectusFemoris_2L", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_2L;
            cmd.Parameters.Add("@Tone_RectusFemoris_3R", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_3R;
            cmd.Parameters.Add("@Tone_RectusFemoris_3L", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_3L;
            cmd.Parameters.Add("@Tone_RectusFemoris_4R", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_4R;
            cmd.Parameters.Add("@Tone_RectusFemoris_4L", SqlDbType.VarChar, 1000).Value = Tone_RectusFemoris_4L;
            cmd.Parameters.Add("@Tone_Hamstrings_1R", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_1R;
            cmd.Parameters.Add("@Tone_Hamstrings_1L", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_1L;
            cmd.Parameters.Add("@Tone_Hamstrings_2R", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_2R;
            cmd.Parameters.Add("@Tone_Hamstrings_2L", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_2L;
            cmd.Parameters.Add("@Tone_Hamstrings_3R", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_3R;
            cmd.Parameters.Add("@Tone_Hamstrings_3L", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_3L;
            cmd.Parameters.Add("@Tone_Hamstrings_4R", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_4R;
            cmd.Parameters.Add("@Tone_Hamstrings_4L", SqlDbType.VarChar, 1000).Value = Tone_Hamstrings_4L;
            cmd.Parameters.Add("@Tone_Gastrosoleus_1R", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_1R;
            cmd.Parameters.Add("@Tone_Gastrosoleus_1L", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_1L;
            cmd.Parameters.Add("@Tone_Gastrosoleus_2R", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_2R;
            cmd.Parameters.Add("@Tone_Gastrosoleus_2L", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_2L;
            cmd.Parameters.Add("@Tone_Gastrosoleus_3R", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_3R;
            cmd.Parameters.Add("@Tone_Gastrosoleus_3L", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_3L;
            cmd.Parameters.Add("@Tone_Gastrosoleus_4R", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_4R;
            cmd.Parameters.Add("@Tone_Gastrosoleus_4L", SqlDbType.VarChar, 1000).Value = Tone_Gastrosoleus_4L;
            cmd.Parameters.Add("@Tone_ElbowFlexors_1R", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_1R;
            cmd.Parameters.Add("@Tone_ElbowFlexors_1L", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_1L;
            cmd.Parameters.Add("@Tone_ElbowFlexors_2R", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_2R;
            cmd.Parameters.Add("@Tone_ElbowFlexors_2L", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_2L;
            cmd.Parameters.Add("@Tone_ElbowFlexors_3R", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_3R;
            cmd.Parameters.Add("@Tone_ElbowFlexors_3L", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_3L;
            cmd.Parameters.Add("@Tone_ElbowFlexors_4R", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_4R;
            cmd.Parameters.Add("@Tone_ElbowFlexors_4L", SqlDbType.VarChar, 1000).Value = Tone_ElbowFlexors_4L;
            cmd.Parameters.Add("@Tone_WristFlexors_1R", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_1R;
            cmd.Parameters.Add("@Tone_WristFlexors_1L", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_1L;
            cmd.Parameters.Add("@Tone_WristFlexors_2R", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_2R;
            cmd.Parameters.Add("@Tone_WristFlexors_2L", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_2L;
            cmd.Parameters.Add("@Tone_WristFlexors_3R", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_3R;
            cmd.Parameters.Add("@Tone_WristFlexors_3L", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_3L;
            cmd.Parameters.Add("@Tone_WristFlexors_4R", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_4R;
            cmd.Parameters.Add("@Tone_WristFlexors_4L", SqlDbType.VarChar, 1000).Value = Tone_WristFlexors_4L;
            cmd.Parameters.Add("@Tone_FingerFlexors_1R", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_1R;
            cmd.Parameters.Add("@Tone_FingerFlexors_1L", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_1L;
            cmd.Parameters.Add("@Tone_FingerFlexors_2R", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_2R;
            cmd.Parameters.Add("@Tone_FingerFlexors_2L", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_2L;
            cmd.Parameters.Add("@Tone_FingerFlexors_3R", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_3R;
            cmd.Parameters.Add("@Tone_FingerFlexors_3L", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_3L;
            cmd.Parameters.Add("@Tone_FingerFlexors_4R", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_4R;
            cmd.Parameters.Add("@Tone_FingerFlexors_4L", SqlDbType.VarChar, 1000).Value = Tone_FingerFlexors_4L;
            cmd.Parameters.Add("@Tone_PronatorFlexors_1R", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_1R;
            cmd.Parameters.Add("@Tone_PronatorFlexors_1L", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_1L;
            cmd.Parameters.Add("@Tone_PronatorFlexors_2R", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_2R;
            cmd.Parameters.Add("@Tone_PronatorFlexors_2L", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_2L;
            cmd.Parameters.Add("@Tone_PronatorFlexors_3R", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_3R;
            cmd.Parameters.Add("@Tone_PronatorFlexors_3L", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_3L;
            cmd.Parameters.Add("@Tone_PronatorFlexors_4R", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_4R;
            cmd.Parameters.Add("@Tone_PronatorFlexors_4L", SqlDbType.VarChar, 1000).Value = Tone_PronatorFlexors_4L;
            if (TardieusScale_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@TardieusScale_Date1", SqlDbType.DateTime).Value = TardieusScale_Date1;
            else
                cmd.Parameters.Add("@TardieusScale_Date1", SqlDbType.DateTime).Value = DBNull.Value;

            if (TardieusScale_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@TardieusScale_Date2", SqlDbType.DateTime).Value = TardieusScale_Date2;
            else
                cmd.Parameters.Add("@TardieusScale_Date2", SqlDbType.DateTime).Value = DBNull.Value;

            if (TardieusScale_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@TardieusScale_Date3", SqlDbType.DateTime).Value = TardieusScale_Date3;
            else
                cmd.Parameters.Add("@TardieusScale_Date3", SqlDbType.DateTime).Value = DBNull.Value;

            if (TardieusScale_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@TardieusScale_Date4", SqlDbType.DateTime).Value = TardieusScale_Date4;
            else
                cmd.Parameters.Add("@TardieusScale_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_1R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_1R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_1L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_1L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_2R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_2R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_2L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_2L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_3R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_3R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_3L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_3L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_4R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_4R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR1_4L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR1_4L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_1R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_1R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_1L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_1L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_2R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_2R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_2L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_2L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_3R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_3R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_3L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_3L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_4R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_4R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR2_4L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR2_4L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_1R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_1R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_1L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_1L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_2R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_2R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_2L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_2L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_3R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_3R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_3L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_3L;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_4R", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_4R;
            cmd.Parameters.Add("@TardieusScale_GastrosoleusR3_4L", SqlDbType.VarChar, 1000).Value = TardieusScale_GastrosoleusR3_4L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_1R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_1R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_1L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_1L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_2R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_2R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_2L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_2L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_3R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_3R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_3L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_3L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_4R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_4R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR1_4L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR1_4L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_1R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_1R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_1L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_1L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_2R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_2R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_2L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_2L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_3R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_3R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_3L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_3L;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_4R", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_4R;
            cmd.Parameters.Add("@TardieusScale_HamstringsR2_4L", SqlDbType.VarChar, 1000).Value = TardieusScale_HamstringsR2_4L;
            cmd.Parameters.Add("@DiagnosisIDs", SqlDbType.VarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;

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



        public int Set2(
            int _appointmentID, DateTime MuscleStrength_Date1, DateTime MuscleStrength_Date2, DateTime MuscleStrength_Date3, DateTime MuscleStrength_Date4, string MuscleStrength_Iliopsoas_1R,
            string MuscleStrength_Iliopsoas_1L, string MuscleStrength_Iliopsoas_2R, string MuscleStrength_Iliopsoas_2L, string MuscleStrength_Iliopsoas_3R, string MuscleStrength_Iliopsoas_3L, string
            MuscleStrength_Iliopsoas_4R, string MuscleStrength_Iliopsoas_4L, string MuscleStrength_GluteusMax_1R, string MuscleStrength_GluteusMax_1L, string MuscleStrength_GluteusMax_2R, string
            MuscleStrength_GluteusMax_2L, string MuscleStrength_GluteusMax_3R, string MuscleStrength_GluteusMax_3L, string MuscleStrength_GluteusMax_4R, string
            MuscleStrength_GluteusMax_4L, string MuscleStrength_Abductors_1R, string MuscleStrength_Abductors_1L, string MuscleStrength_Abductors_2R, string MuscleStrength_Abductors_2L,
            string MuscleStrength_Abductors_3R, string MuscleStrength_Abductors_3L, string MuscleStrength_Abductors_4R, string MuscleStrength_Abductors_4L, string MuscleStrength_RectusFemoris_1R,
            string MuscleStrength_RectusFemoris_1L, string MuscleStrength_RectusFemoris_2R, string MuscleStrength_RectusFemoris_2L, string MuscleStrength_RectusFemoris_3R, string
            MuscleStrength_RectusFemoris_3L, string MuscleStrength_RectusFemoris_4R, string MuscleStrength_RectusFemoris_4L, string MuscleStrength_Hamstrings_1R,
            string MuscleStrength_Hamstrings_1L, string MuscleStrength_Hamstrings_2R, string MuscleStrength_Hamstrings_2L, string MuscleStrength_Hamstrings_3R,
            string MuscleStrength_Hamstrings_3L, string MuscleStrength_Hamstrings_4R, string MuscleStrength_Hamstrings_4L, string MuscleStrength_Gastrosoleus_1R,
            string MuscleStrength_Gastrosoleus_1L, string MuscleStrength_Gastrosoleus_2R, string MuscleStrength_Gastrosoleus_2L, string MuscleStrength_Gastrosoleus_3R,
            string MuscleStrength_Gastrosoleus_3L, string MuscleStrength_Gastrosoleus_4R, string MuscleStrength_Gastrosoleus_4L, string MuscleStrength_TibialisAnt_1R,
            string MuscleStrength_TibialisAnt_1L, string MuscleStrength_TibialisAnt_2R, string MuscleStrength_TibialisAnt_2L, string MuscleStrength_TibialisAnt_3R, string MuscleStrength_TibialisAnt_3L,
            string MuscleStrength_TibialisAnt_4R, string MuscleStrength_TibialisAnt_4L, string MuscleStrength_ElbowFlexors_1R, string MuscleStrength_ElbowFlexors_1L,
            string MuscleStrength_ElbowFlexors_2R, string MuscleStrength_ElbowFlexors_2L, string MuscleStrength_ElbowFlexors_3R, string MuscleStrength_ElbowFlexors_3L,
            string MuscleStrength_ElbowFlexors_4R, string MuscleStrength_ElbowFlexors_4L, string MuscleStrength_PronatorTeres_1R, string MuscleStrength_PronatorTeres_1L,
            string MuscleStrength_PronatorTeres_2R, string MuscleStrength_PronatorTeres_2L, string MuscleStrength_PronatorTeres_3R, string MuscleStrength_PronatorTeres_3L,
            string MuscleStrength_PronatorTeres_4R, string MuscleStrength_PronatorTeres_4L, string MuscleStrength_WristFlexors_1R, string MuscleStrength_WristFlexors_1L,
            string MuscleStrength_WristFlexors_2R, string MuscleStrength_WristFlexors_2L, string MuscleStrength_WristFlexors_3R, string MuscleStrength_WristFlexors_3L,
            string MuscleStrength_WristFlexors_4R, string MuscleStrength_WristFlexors_4L, string MuscleStrength_WristExtensors_1R, string MuscleStrength_WristExtensors_1L,
            string MuscleStrength_WristExtensors_2R, string MuscleStrength_WristExtensors_2L, string MuscleStrength_WristExtensors_3R, string MuscleStrength_WristExtensors_3L,
            string MuscleStrength_WristExtensors_4R, string MuscleStrength_WristExtensors_4L, string MuscleStrength_FingerFlexors_1R, string MuscleStrength_FingerFlexors_1L,
            string MuscleStrength_FingerFlexors_2R, string MuscleStrength_FingerFlexors_2L, string MuscleStrength_FingerFlexors_3R, string MuscleStrength_FingerFlexors_3L,
            string MuscleStrength_FingerFlexors_4R, string MuscleStrength_FingerFlexors_4L, DateTime Voluntary_Date1, DateTime Voluntary_Date2, DateTime Voluntary_Date3, DateTime Voluntary_Date4,
            string Voluntary_HipFlexion_1R, string Voluntary_HipFlexion_1L, string Voluntary_HipFlexion_2R, string Voluntary_HipFlexion_2L, string Voluntary_HipFlexion_3R, string Voluntary_HipFlexion_3L,
            string Voluntary_HipFlexion_4R, string Voluntary_HipFlexion_4L, string Voluntary_HipExtension_1R, string Voluntary_HipExtension_1L, string Voluntary_HipExtension_2R,
            string Voluntary_HipExtension_2L, string Voluntary_HipExtension_3R, string Voluntary_HipExtension_3L, string Voluntary_HipExtension_4R, string Voluntary_HipExtension_4L,
            string Voluntary_HipAbduction_1R, string Voluntary_HipAbduction_1L, string Voluntary_HipAbduction_2R, string Voluntary_HipAbduction_2L, string Voluntary_HipAbduction_3R,
            string Voluntary_HipAbduction_3L, string Voluntary_HipAbduction_4R, string Voluntary_HipAbduction_4L, string Voluntary_KneeFlexion_1R, string Voluntary_KneeFlexion_1L,
            string Voluntary_KneeFlexion_2R, string Voluntary_KneeFlexion_2L, string Voluntary_KneeFlexion_3R, string Voluntary_KneeFlexion_3L, string Voluntary_KneeFlexion_4R,
            string Voluntary_KneeFlexion_4L, string Voluntary_KneeExtension_1R, string Voluntary_KneeExtension_1L, string Voluntary_KneeExtension_2R, string Voluntary_KneeExtension_2L,
            string Voluntary_KneeExtension_3R, string Voluntary_KneeExtension_3L, string Voluntary_KneeExtension_4R, string Voluntary_KneeExtension_4L, string Voluntary_Dorsiflexion_1R,
            string Voluntary_Dorsiflexion_1L, string Voluntary_Dorsiflexion_2R, string Voluntary_Dorsiflexion_2L, string Voluntary_Dorsiflexion_3R, string Voluntary_Dorsiflexion_3L,
            string Voluntary_Dorsiflexion_4R, string Voluntary_Dorsiflexion_4L, string Voluntary_Plantarflexion_1R, string Voluntary_Plantarflexion_1L, string Voluntary_Plantarflexion_2R,
            string Voluntary_Plantarflexion_2L, string Voluntary_Plantarflexion_3R, string Voluntary_Plantarflexion_3L, string Voluntary_Plantarflexion_4R, string Voluntary_Plantarflexion_4L,
            string Voluntary_WristDorsiflex_1R, string Voluntary_WristDorsiflex_1L, string Voluntary_WristDorsiflex_2R, string Voluntary_WristDorsiflex_2L, string Voluntary_WristDorsiflex_3R,
            string Voluntary_WristDorsiflex_3L, string Voluntary_WristDorsiflex_4R, string Voluntary_WristDorsiflex_4L, string Voluntary_Grasp_1R, string Voluntary_Grasp_1L, string Voluntary_Grasp_2R,
            string Voluntary_Grasp_2L, string Voluntary_Grasp_3R, string Voluntary_Grasp_3L, string Voluntary_Grasp_4R, string Voluntary_Grasp_4L, string Voluntary_Release_1R, string Voluntary_Release_1L,
            string Voluntary_Release_2R, string Voluntary_Release_2L, string Voluntary_Release_3R, string Voluntary_Release_3L, string Voluntary_Release_4R, string Voluntary_Release_4L)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst2_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            if (MuscleStrength_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@MuscleStrength_Date1", SqlDbType.DateTime).Value = MuscleStrength_Date1;
            else
                cmd.Parameters.Add("@MuscleStrength_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (MuscleStrength_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@MuscleStrength_Date2", SqlDbType.DateTime).Value = MuscleStrength_Date2;
            else
                cmd.Parameters.Add("@MuscleStrength_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (MuscleStrength_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@MuscleStrength_Date3", SqlDbType.DateTime).Value = MuscleStrength_Date3;
            else
                cmd.Parameters.Add("@MuscleStrength_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (MuscleStrength_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@MuscleStrength_Date4", SqlDbType.DateTime).Value = MuscleStrength_Date4;
            else
                cmd.Parameters.Add("@MuscleStrength_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_1R;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_1L;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_2R;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_2L;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_3R;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_3L;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_4R;
            cmd.Parameters.Add("@MuscleStrength_Iliopsoas_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Iliopsoas_4L;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_1R;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_1L;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_2R;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_2L;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_3R;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_3L;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_4R;
            cmd.Parameters.Add("@MuscleStrength_GluteusMax_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_GluteusMax_4L;
            cmd.Parameters.Add("@MuscleStrength_Abductors_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_1R;
            cmd.Parameters.Add("@MuscleStrength_Abductors_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_1L;
            cmd.Parameters.Add("@MuscleStrength_Abductors_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_2R;
            cmd.Parameters.Add("@MuscleStrength_Abductors_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_2L;
            cmd.Parameters.Add("@MuscleStrength_Abductors_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_3R;
            cmd.Parameters.Add("@MuscleStrength_Abductors_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_3L;
            cmd.Parameters.Add("@MuscleStrength_Abductors_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_4R;
            cmd.Parameters.Add("@MuscleStrength_Abductors_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Abductors_4L;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_1R;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_1L;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_2R;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_2L;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_3R;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_3L;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_4R;
            cmd.Parameters.Add("@MuscleStrength_RectusFemoris_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_RectusFemoris_4L;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_1R;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_1L;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_2R;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_2L;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_3R;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_3L;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_4R;
            cmd.Parameters.Add("@MuscleStrength_Hamstrings_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Hamstrings_4L;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_1R;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_1L;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_2R;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_2L;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_3R;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_3L;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_4R;
            cmd.Parameters.Add("@MuscleStrength_Gastrosoleus_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_Gastrosoleus_4L;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_1R;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_1L;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_2R;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_2L;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_3R;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_3L;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_4R;
            cmd.Parameters.Add("@MuscleStrength_TibialisAnt_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_TibialisAnt_4L;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_1R;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_1L;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_2R;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_2L;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_3R;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_3L;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_4R;
            cmd.Parameters.Add("@MuscleStrength_ElbowFlexors_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_ElbowFlexors_4L;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_1R;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_1L;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_2R;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_2L;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_3R;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_3L;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_4R;
            cmd.Parameters.Add("@MuscleStrength_PronatorTeres_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_PronatorTeres_4L;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_1R;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_1L;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_2R;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_2L;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_3R;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_3L;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_4R;
            cmd.Parameters.Add("@MuscleStrength_WristFlexors_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristFlexors_4L;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_1R;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_1L;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_2R;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_2L;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_3R;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_3L;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_4R;
            cmd.Parameters.Add("@MuscleStrength_WristExtensors_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_WristExtensors_4L;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_1R", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_1R;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_1L", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_1L;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_2R", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_2R;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_2L", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_2L;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_3R", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_3R;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_3L", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_3L;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_4R", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_4R;
            cmd.Parameters.Add("@MuscleStrength_FingerFlexors_4L", SqlDbType.VarChar, 1000).Value = MuscleStrength_FingerFlexors_4L;
            if (Voluntary_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@Voluntary_Date1", SqlDbType.DateTime).Value = Voluntary_Date1;
            else
                cmd.Parameters.Add("@Voluntary_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (Voluntary_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@Voluntary_Date2", SqlDbType.DateTime).Value = Voluntary_Date2;
            else
                cmd.Parameters.Add("@Voluntary_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (Voluntary_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@Voluntary_Date3", SqlDbType.DateTime).Value = Voluntary_Date3;
            else
                cmd.Parameters.Add("@Voluntary_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (Voluntary_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@Voluntary_Date4", SqlDbType.DateTime).Value = Voluntary_Date4;
            else
                cmd.Parameters.Add("@Voluntary_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Voluntary_HipFlexion_1R", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_1R;
            cmd.Parameters.Add("@Voluntary_HipFlexion_1L", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_1L;
            cmd.Parameters.Add("@Voluntary_HipFlexion_2R", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_2R;
            cmd.Parameters.Add("@Voluntary_HipFlexion_2L", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_2L;
            cmd.Parameters.Add("@Voluntary_HipFlexion_3R", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_3R;
            cmd.Parameters.Add("@Voluntary_HipFlexion_3L", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_3L;
            cmd.Parameters.Add("@Voluntary_HipFlexion_4R", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_4R;
            cmd.Parameters.Add("@Voluntary_HipFlexion_4L", SqlDbType.VarChar, 1000).Value = Voluntary_HipFlexion_4L;
            cmd.Parameters.Add("@Voluntary_HipExtension_1R", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_1R;
            cmd.Parameters.Add("@Voluntary_HipExtension_1L", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_1L;
            cmd.Parameters.Add("@Voluntary_HipExtension_2R", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_2R;
            cmd.Parameters.Add("@Voluntary_HipExtension_2L", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_2L;
            cmd.Parameters.Add("@Voluntary_HipExtension_3R", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_3R;
            cmd.Parameters.Add("@Voluntary_HipExtension_3L", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_3L;
            cmd.Parameters.Add("@Voluntary_HipExtension_4R", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_4R;
            cmd.Parameters.Add("@Voluntary_HipExtension_4L", SqlDbType.VarChar, 1000).Value = Voluntary_HipExtension_4L;
            cmd.Parameters.Add("@Voluntary_HipAbduction_1R", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_1R;
            cmd.Parameters.Add("@Voluntary_HipAbduction_1L", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_1L;
            cmd.Parameters.Add("@Voluntary_HipAbduction_2R", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_2R;
            cmd.Parameters.Add("@Voluntary_HipAbduction_2L", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_2L;
            cmd.Parameters.Add("@Voluntary_HipAbduction_3R", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_3R;
            cmd.Parameters.Add("@Voluntary_HipAbduction_3L", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_3L;
            cmd.Parameters.Add("@Voluntary_HipAbduction_4R", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_4R;
            cmd.Parameters.Add("@Voluntary_HipAbduction_4L", SqlDbType.VarChar, 1000).Value = Voluntary_HipAbduction_4L;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_1R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_1R;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_1L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_1L;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_2R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_2R;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_2L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_2L;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_3R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_3R;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_3L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_3L;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_4R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_4R;
            cmd.Parameters.Add("@Voluntary_KneeFlexion_4L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeFlexion_4L;
            cmd.Parameters.Add("@Voluntary_KneeExtension_1R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_1R;
            cmd.Parameters.Add("@Voluntary_KneeExtension_1L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_1L;
            cmd.Parameters.Add("@Voluntary_KneeExtension_2R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_2R;
            cmd.Parameters.Add("@Voluntary_KneeExtension_2L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_2L;
            cmd.Parameters.Add("@Voluntary_KneeExtension_3R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_3R;
            cmd.Parameters.Add("@Voluntary_KneeExtension_3L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_3L;
            cmd.Parameters.Add("@Voluntary_KneeExtension_4R", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_4R;
            cmd.Parameters.Add("@Voluntary_KneeExtension_4L", SqlDbType.VarChar, 1000).Value = Voluntary_KneeExtension_4L;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_1R", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_1R;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_1L", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_1L;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_2R", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_2R;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_2L", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_2L;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_3R", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_3R;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_3L", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_3L;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_4R", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_4R;
            cmd.Parameters.Add("@Voluntary_Dorsiflexion_4L", SqlDbType.VarChar, 1000).Value = Voluntary_Dorsiflexion_4L;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_1R", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_1R;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_1L", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_1L;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_2R", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_2R;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_2L", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_2L;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_3R", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_3R;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_3L", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_3L;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_4R", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_4R;
            cmd.Parameters.Add("@Voluntary_Plantarflexion_4L", SqlDbType.VarChar, 1000).Value = Voluntary_Plantarflexion_4L;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_1R ", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_1R;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_1L ", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_1L;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_2R", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_2R;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_2L", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_2L;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_3R", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_3R;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_3L", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_3L;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_4R", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_4R;
            cmd.Parameters.Add("@Voluntary_WristDorsiflex_4L", SqlDbType.VarChar, 1000).Value = Voluntary_WristDorsiflex_4L;
            cmd.Parameters.Add("@Voluntary_Grasp_1R", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_1R;
            cmd.Parameters.Add("@Voluntary_Grasp_1L", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_1L;
            cmd.Parameters.Add("@Voluntary_Grasp_2R", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_2R;
            cmd.Parameters.Add("@Voluntary_Grasp_2L", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_2L;
            cmd.Parameters.Add("@Voluntary_Grasp_3R", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_3R;
            cmd.Parameters.Add("@Voluntary_Grasp_3L", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_3L;
            cmd.Parameters.Add("@Voluntary_Grasp_4R", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_4R;
            cmd.Parameters.Add("@Voluntary_Grasp_4L", SqlDbType.VarChar, 1000).Value = Voluntary_Grasp_4L;
            cmd.Parameters.Add("@Voluntary_Release_1R", SqlDbType.VarChar, 1000).Value = Voluntary_Release_1R;
            cmd.Parameters.Add("@Voluntary_Release_1L", SqlDbType.VarChar, 1000).Value = Voluntary_Release_1L;
            cmd.Parameters.Add("@Voluntary_Release_2R", SqlDbType.VarChar, 1000).Value = Voluntary_Release_2R;
            cmd.Parameters.Add("@Voluntary_Release_2L", SqlDbType.VarChar, 1000).Value = Voluntary_Release_2L;
            cmd.Parameters.Add("@Voluntary_Release_3R", SqlDbType.VarChar, 1000).Value = Voluntary_Release_3R;
            cmd.Parameters.Add("@Voluntary_Release_3L", SqlDbType.VarChar, 1000).Value = Voluntary_Release_3L;
            cmd.Parameters.Add("@Voluntary_Release_4R", SqlDbType.VarChar, 1000).Value = Voluntary_Release_4R;
            cmd.Parameters.Add("@Voluntary_Release_4L", SqlDbType.VarChar, 1000).Value = Voluntary_Release_4L;

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




        public int Set3(
            int _appointmentID, DateTime FunctionalStrength_Date1, DateTime FunctionalStrength_Date2, DateTime FunctionalStrength_Date3, DateTime FunctionalStrength_Date4, string FunctionalStrength_PullStand_1,
            string FunctionalStrength_PullStand_2, string FunctionalStrength_PullStand_3, string FunctionalStrength_PullStand_4, string FunctionalStrength_Independent3Sec_1,
            string FunctionalStrength_Independent3Sec_2, string FunctionalStrength_Independent3Sec_3, string FunctionalStrength_Independent3Sec_4, string FunctionalStrength_Independent20Sec_1,
            string FunctionalStrength_Independent20Sec_2, string FunctionalStrength_Independent20Sec_3, string FunctionalStrength_Independent20Sec_4, string FunctionalStrength_HandHeldR_1,
            string FunctionalStrength_HandHeldR_2, string FunctionalStrength_HandHeldR_3, string FunctionalStrength_HandHeldR_4, string FunctionalStrength_HandHeldL_1,
            string FunctionalStrength_HandHeldL_2, string FunctionalStrength_HandHeldL_3, string FunctionalStrength_HandHeldL_4, string FunctionalStrength_OneLegR_1,
            string FunctionalStrength_OneLegR_2, string FunctionalStrength_OneLegR_3, string FunctionalStrength_OneLegR_4, string FunctionalStrength_OneLegL_1, string FunctionalStrength_OneLegL_2,
            string FunctionalStrength_OneLegL_3, string FunctionalStrength_OneLegL_4, string FunctionalStrength_ShortSit_1, string FunctionalStrength_ShortSit_2, string FunctionalStrength_ShortSit_3,
            string FunctionalStrength_ShortSit_4, string FunctionalStrength_HighKneeR_1, string FunctionalStrength_HighKneeR_2, string FunctionalStrength_HighKneeR_3,
            string FunctionalStrength_HighKneeR_4, string FunctionalStrength_HighKneeL_1, string FunctionalStrength_HighKneeL_2, string FunctionalStrength_HighKneeL_3,
            string FunctionalStrength_HighKneeL_4, string FunctionalStrength_LowersFloor_1, string FunctionalStrength_LowersFloor_2, string FunctionalStrength_LowersFloor_3,
            string FunctionalStrength_LowersFloor_4, string FunctionalStrength_Squats_1, string FunctionalStrength_Squats_2, string FunctionalStrength_Squats_3, string FunctionalStrength_Squats_4,
            string FunctionalStrength_StandingPicks_1, string FunctionalStrength_StandingPicks_2, string FunctionalStrength_StandingPicks_3, string FunctionalStrength_StandingPicks_4,
            string FunctionalStrength_Total_1, string FunctionalStrength_Total_2, string FunctionalStrength_Total_3, string FunctionalStrength_Total_4, DateTime BotoxData_Date1, DateTime BotoxData_Date2,
            DateTime BotoxData_Date3, DateTime BotoxData_Date4, string BotoxData_Weight_1, string BotoxData_Weight_2, string BotoxData_Weight_3, string BotoxData_Weight_4, string BotoxData_BotoxInjected_1,
            string BotoxData_BotoxInjected_2, string BotoxData_BotoxInjected_3, string BotoxData_BotoxInjected_4, string BotoxData_Dilution_1, string BotoxData_Dilution_2, string BotoxData_Dilution_3,
            string BotoxData_Dilution_4, string BotoxData_MusclesInjected_1, string BotoxData_MusclesInjected_2, string BotoxData_MusclesInjected_3, string BotoxData_MusclesInjected_4,
            string BotoxData_Gastocnemius_1, string BotoxData_Gastocnemius_2, string BotoxData_Gastocnemius_3, string BotoxData_Gastocnemius_4, string BotoxData_Tibialis_1, string BotoxData_Tibialis_2,
            string BotoxData_Tibialis_3, string BotoxData_Tibialis_4, string BotoxData_Hamstrings_1, string BotoxData_Hamstrings_2, string BotoxData_Hamstrings_3, string BotoxData_Hamstrings_4,
            string BotoxData_Adductors_1, string BotoxData_Adductors_2, string BotoxData_Adductors_3, string BotoxData_Adductors_4, string BotoxData_Rectus_1, string BotoxData_Rectus_2,
            string BotoxData_Rectus_3, string BotoxData_Rectus_4, string BotoxData_Iliopsoas_1, string BotoxData_Iliopsoas_2, string BotoxData_Iliopsoas_3, string BotoxData_Iliopsoas_4,
            string BotoxData_Pronator_1, string BotoxData_Pronator_2, string BotoxData_Pronator_3, string BotoxData_Pronator_4, string BotoxData_FCR_1, string BotoxData_FCR_2, string BotoxData_FCR_3,
            string BotoxData_FCR_4, string BotoxData_FCU_1, string BotoxData_FCU_2, string BotoxData_FCU_3, string BotoxData_FCU_4, string BotoxData_FDS_1, string BotoxData_FDS_2, string BotoxData_FDS_3,
            string BotoxData_FDS_4, string BotoxData_FDP_1, string BotoxData_FDP_2, string BotoxData_FDP_3, string BotoxData_FDP_4, string BotoxData_FPL_1, string BotoxData_FPL_2, string BotoxData_FPL_3,
            string BotoxData_FPL_4, string BotoxData_Adductor_1, string BotoxData_Adductor_2, string BotoxData_Adductor_3, string BotoxData_Adductor_4, string BotoxData_Intrinsics_1, string BotoxData_Intrinsics_2,
            string BotoxData_Intrinsics_3, string BotoxData_Intrinsics_4, string BotoxData_Casting_1, string BotoxData_Casting_2, string BotoxData_Casting_3, string BotoxData_Casting_4,
            int Doctor_Director, int Doctor_Physiotheraist, int Doctor_Occupational,
            bool IsFinal, bool IsGiven, DateTime GivenDate, DateTime ModifyDate, int _loginID)
        {
            SqlCommand cmd = new SqlCommand("ReportBotoxMst3_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            if (FunctionalStrength_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@FunctionalStrength_Date1", SqlDbType.DateTime).Value = FunctionalStrength_Date1;
            else
                cmd.Parameters.Add("@FunctionalStrength_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (FunctionalStrength_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@FunctionalStrength_Date2", SqlDbType.DateTime).Value = FunctionalStrength_Date2;
            else
                cmd.Parameters.Add("@FunctionalStrength_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (FunctionalStrength_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@FunctionalStrength_Date3", SqlDbType.DateTime).Value = FunctionalStrength_Date3;
            else
                cmd.Parameters.Add("@FunctionalStrength_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (FunctionalStrength_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@FunctionalStrength_Date4", SqlDbType.DateTime).Value = FunctionalStrength_Date4;
            else
                cmd.Parameters.Add("@FunctionalStrength_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@FunctionalStrength_PullStand_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_PullStand_1;
            cmd.Parameters.Add("@FunctionalStrength_PullStand_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_PullStand_2;
            cmd.Parameters.Add("@FunctionalStrength_PullStand_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_PullStand_3;
            cmd.Parameters.Add("@FunctionalStrength_PullStand_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_PullStand_4;
            cmd.Parameters.Add("@FunctionalStrength_Independent3Sec_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent3Sec_1;
            cmd.Parameters.Add("@FunctionalStrength_Independent3Sec_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent3Sec_2;
            cmd.Parameters.Add("@FunctionalStrength_Independent3Sec_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent3Sec_3;
            cmd.Parameters.Add("@FunctionalStrength_Independent3Sec_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent3Sec_4;
            cmd.Parameters.Add("@FunctionalStrength_Independent20Sec_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent20Sec_1;
            cmd.Parameters.Add("@FunctionalStrength_Independent20Sec_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent20Sec_2;
            cmd.Parameters.Add("@FunctionalStrength_Independent20Sec_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent20Sec_3;
            cmd.Parameters.Add("@FunctionalStrength_Independent20Sec_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Independent20Sec_4;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldR_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldR_1;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldR_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldR_2;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldR_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldR_3;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldR_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldR_4;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldL_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldL_1;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldL_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldL_2;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldL_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldL_3;
            cmd.Parameters.Add("@FunctionalStrength_HandHeldL_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HandHeldL_4;
            cmd.Parameters.Add("@FunctionalStrength_OneLegR_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegR_1;
            cmd.Parameters.Add("@FunctionalStrength_OneLegR_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegR_2;
            cmd.Parameters.Add("@FunctionalStrength_OneLegR_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegR_3;
            cmd.Parameters.Add("@FunctionalStrength_OneLegR_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegR_4;
            cmd.Parameters.Add("@FunctionalStrength_OneLegL_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegL_1;
            cmd.Parameters.Add("@FunctionalStrength_OneLegL_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegL_2;
            cmd.Parameters.Add("@FunctionalStrength_OneLegL_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegL_3;
            cmd.Parameters.Add("@FunctionalStrength_OneLegL_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_OneLegL_4;
            cmd.Parameters.Add("@FunctionalStrength_ShortSit_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_ShortSit_1;
            cmd.Parameters.Add("@FunctionalStrength_ShortSit_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_ShortSit_2;
            cmd.Parameters.Add("@FunctionalStrength_ShortSit_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_ShortSit_3;
            cmd.Parameters.Add("@FunctionalStrength_ShortSit_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_ShortSit_4;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeR_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeR_1;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeR_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeR_2;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeR_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeR_3;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeR_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeR_4;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeL_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeL_1;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeL_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeL_2;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeL_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeL_3;
            cmd.Parameters.Add("@FunctionalStrength_HighKneeL_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_HighKneeL_4;
            cmd.Parameters.Add("@FunctionalStrength_LowersFloor_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_LowersFloor_1;
            cmd.Parameters.Add("@FunctionalStrength_LowersFloor_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_LowersFloor_2;
            cmd.Parameters.Add("@FunctionalStrength_LowersFloor_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_LowersFloor_3;
            cmd.Parameters.Add("@FunctionalStrength_LowersFloor_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_LowersFloor_4;
            cmd.Parameters.Add("@FunctionalStrength_Squats_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Squats_1;
            cmd.Parameters.Add("@FunctionalStrength_Squats_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Squats_2;
            cmd.Parameters.Add("@FunctionalStrength_Squats_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Squats_3;
            cmd.Parameters.Add("@FunctionalStrength_Squats_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Squats_4;
            cmd.Parameters.Add("@FunctionalStrength_StandingPicks_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_StandingPicks_1;
            cmd.Parameters.Add("@FunctionalStrength_StandingPicks_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_StandingPicks_2;
            cmd.Parameters.Add("@FunctionalStrength_StandingPicks_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_StandingPicks_3;
            cmd.Parameters.Add("@FunctionalStrength_StandingPicks_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_StandingPicks_4;
            cmd.Parameters.Add("@FunctionalStrength_Total_1", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Total_1;
            cmd.Parameters.Add("@FunctionalStrength_Total_2", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Total_2;
            cmd.Parameters.Add("@FunctionalStrength_Total_3", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Total_3;
            cmd.Parameters.Add("@FunctionalStrength_Total_4", SqlDbType.VarChar, 1000).Value = FunctionalStrength_Total_4;

            if (BotoxData_Date1 > DateTime.MinValue)
                cmd.Parameters.Add("@BotoxData_Date1", SqlDbType.DateTime).Value = BotoxData_Date1;
            else
                cmd.Parameters.Add("@BotoxData_Date1", SqlDbType.DateTime).Value = DBNull.Value;
            if (BotoxData_Date2 > DateTime.MinValue)
                cmd.Parameters.Add("@BotoxData_Date2", SqlDbType.DateTime).Value = BotoxData_Date2;
            else
                cmd.Parameters.Add("@BotoxData_Date2", SqlDbType.DateTime).Value = DBNull.Value;
            if (BotoxData_Date3 > DateTime.MinValue)
                cmd.Parameters.Add("@BotoxData_Date3", SqlDbType.DateTime).Value = BotoxData_Date3;
            else
                cmd.Parameters.Add("@BotoxData_Date3", SqlDbType.DateTime).Value = DBNull.Value;
            if (BotoxData_Date4 > DateTime.MinValue)
                cmd.Parameters.Add("@BotoxData_Date4", SqlDbType.DateTime).Value = BotoxData_Date4;
            else
                cmd.Parameters.Add("@BotoxData_Date4", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@BotoxData_Weight_1", SqlDbType.VarChar, 1000).Value = BotoxData_Weight_1;
            cmd.Parameters.Add("@BotoxData_Weight_2", SqlDbType.VarChar, 1000).Value = BotoxData_Weight_2;
            cmd.Parameters.Add("@BotoxData_Weight_3", SqlDbType.VarChar, 1000).Value = BotoxData_Weight_3;
            cmd.Parameters.Add("@BotoxData_Weight_4", SqlDbType.VarChar, 1000).Value = BotoxData_Weight_4;
            cmd.Parameters.Add("@BotoxData_BotoxInjected_1", SqlDbType.VarChar, 1000).Value = BotoxData_BotoxInjected_1;
            cmd.Parameters.Add("@BotoxData_BotoxInjected_2", SqlDbType.VarChar, 1000).Value = BotoxData_BotoxInjected_2;
            cmd.Parameters.Add("@BotoxData_BotoxInjected_3", SqlDbType.VarChar, 1000).Value = BotoxData_BotoxInjected_3;
            cmd.Parameters.Add("@BotoxData_BotoxInjected_4", SqlDbType.VarChar, 1000).Value = BotoxData_BotoxInjected_4;
            cmd.Parameters.Add("@BotoxData_Dilution_1", SqlDbType.VarChar, 1000).Value = BotoxData_Dilution_1;
            cmd.Parameters.Add("@BotoxData_Dilution_2", SqlDbType.VarChar, 1000).Value = BotoxData_Dilution_2;
            cmd.Parameters.Add("@BotoxData_Dilution_3", SqlDbType.VarChar, 1000).Value = BotoxData_Dilution_3;
            cmd.Parameters.Add("@BotoxData_Dilution_4", SqlDbType.VarChar, 1000).Value = BotoxData_Dilution_4;
            cmd.Parameters.Add("@BotoxData_MusclesInjected_1", SqlDbType.VarChar, 1000).Value = BotoxData_MusclesInjected_1;
            cmd.Parameters.Add("@BotoxData_MusclesInjected_2", SqlDbType.VarChar, 1000).Value = BotoxData_MusclesInjected_2;
            cmd.Parameters.Add("@BotoxData_MusclesInjected_3", SqlDbType.VarChar, 1000).Value = BotoxData_MusclesInjected_3;
            cmd.Parameters.Add("@BotoxData_MusclesInjected_4", SqlDbType.VarChar, 1000).Value = BotoxData_MusclesInjected_4;
            cmd.Parameters.Add("@BotoxData_Gastocnemius_1", SqlDbType.VarChar, 1000).Value = BotoxData_Gastocnemius_1;
            cmd.Parameters.Add("@BotoxData_Gastocnemius_2", SqlDbType.VarChar, 1000).Value = BotoxData_Gastocnemius_2;
            cmd.Parameters.Add("@BotoxData_Gastocnemius_3", SqlDbType.VarChar, 1000).Value = BotoxData_Gastocnemius_3;
            cmd.Parameters.Add("@BotoxData_Gastocnemius_4", SqlDbType.VarChar, 1000).Value = BotoxData_Gastocnemius_4;
            cmd.Parameters.Add("@BotoxData_Tibialis_1", SqlDbType.VarChar, 1000).Value = BotoxData_Tibialis_1;
            cmd.Parameters.Add("@BotoxData_Tibialis_2", SqlDbType.VarChar, 1000).Value = BotoxData_Tibialis_2;
            cmd.Parameters.Add("@BotoxData_Tibialis_3", SqlDbType.VarChar, 1000).Value = BotoxData_Tibialis_3;
            cmd.Parameters.Add("@BotoxData_Tibialis_4", SqlDbType.VarChar, 1000).Value = BotoxData_Tibialis_4;
            cmd.Parameters.Add("@BotoxData_Hamstrings_1", SqlDbType.VarChar, 1000).Value = BotoxData_Hamstrings_1;
            cmd.Parameters.Add("@BotoxData_Hamstrings_2", SqlDbType.VarChar, 1000).Value = BotoxData_Hamstrings_2;
            cmd.Parameters.Add("@BotoxData_Hamstrings_3", SqlDbType.VarChar, 1000).Value = BotoxData_Hamstrings_3;
            cmd.Parameters.Add("@BotoxData_Hamstrings_4", SqlDbType.VarChar, 1000).Value = BotoxData_Hamstrings_4;
            cmd.Parameters.Add("@BotoxData_Adductors_1", SqlDbType.VarChar, 1000).Value = BotoxData_Adductors_1;
            cmd.Parameters.Add("@BotoxData_Adductors_2", SqlDbType.VarChar, 1000).Value = BotoxData_Adductors_2;
            cmd.Parameters.Add("@BotoxData_Adductors_3", SqlDbType.VarChar, 1000).Value = BotoxData_Adductors_3;
            cmd.Parameters.Add("@BotoxData_Adductors_4", SqlDbType.VarChar, 1000).Value = BotoxData_Adductors_4;
            cmd.Parameters.Add("@BotoxData_Rectus_1", SqlDbType.VarChar, 1000).Value = BotoxData_Rectus_1;
            cmd.Parameters.Add("@BotoxData_Rectus_2", SqlDbType.VarChar, 1000).Value = BotoxData_Rectus_2;
            cmd.Parameters.Add("@BotoxData_Rectus_3", SqlDbType.VarChar, 1000).Value = BotoxData_Rectus_3;
            cmd.Parameters.Add("@BotoxData_Rectus_4", SqlDbType.VarChar, 1000).Value = BotoxData_Rectus_4;
            cmd.Parameters.Add("@BotoxData_Iliopsoas_1", SqlDbType.VarChar, 1000).Value = BotoxData_Iliopsoas_1;
            cmd.Parameters.Add("@BotoxData_Iliopsoas_2", SqlDbType.VarChar, 1000).Value = BotoxData_Iliopsoas_2;
            cmd.Parameters.Add("@BotoxData_Iliopsoas_3", SqlDbType.VarChar, 1000).Value = BotoxData_Iliopsoas_3;
            cmd.Parameters.Add("@BotoxData_Iliopsoas_4", SqlDbType.VarChar, 1000).Value = BotoxData_Iliopsoas_4;
            cmd.Parameters.Add("@BotoxData_Pronator_1", SqlDbType.VarChar, 1000).Value = BotoxData_Pronator_1;
            cmd.Parameters.Add("@BotoxData_Pronator_2", SqlDbType.VarChar, 1000).Value = BotoxData_Pronator_2;
            cmd.Parameters.Add("@BotoxData_Pronator_3", SqlDbType.VarChar, 1000).Value = BotoxData_Pronator_3;
            cmd.Parameters.Add("@BotoxData_Pronator_4", SqlDbType.VarChar, 1000).Value = BotoxData_Pronator_4;
            cmd.Parameters.Add("@BotoxData_FCR_1", SqlDbType.VarChar, 1000).Value = BotoxData_FCR_1;
            cmd.Parameters.Add("@BotoxData_FCR_2", SqlDbType.VarChar, 1000).Value = BotoxData_FCR_2;
            cmd.Parameters.Add("@BotoxData_FCR_3", SqlDbType.VarChar, 1000).Value = BotoxData_FCR_3;
            cmd.Parameters.Add("@BotoxData_FCR_4", SqlDbType.VarChar, 1000).Value = BotoxData_FCR_4;
            cmd.Parameters.Add("@BotoxData_FCU_1", SqlDbType.VarChar, 1000).Value = BotoxData_FCU_1;
            cmd.Parameters.Add("@BotoxData_FCU_2", SqlDbType.VarChar, 1000).Value = BotoxData_FCU_2;
            cmd.Parameters.Add("@BotoxData_FCU_3", SqlDbType.VarChar, 1000).Value = BotoxData_FCU_3;
            cmd.Parameters.Add("@BotoxData_FCU_4", SqlDbType.VarChar, 1000).Value = BotoxData_FCU_4;
            cmd.Parameters.Add("@BotoxData_FDS_1", SqlDbType.VarChar, 1000).Value = BotoxData_FDS_1;
            cmd.Parameters.Add("@BotoxData_FDS_2", SqlDbType.VarChar, 1000).Value = BotoxData_FDS_2;
            cmd.Parameters.Add("@BotoxData_FDS_3", SqlDbType.VarChar, 1000).Value = BotoxData_FDS_3;
            cmd.Parameters.Add("@BotoxData_FDS_4", SqlDbType.VarChar, 1000).Value = BotoxData_FDS_4;
            cmd.Parameters.Add("@BotoxData_FDP_1", SqlDbType.VarChar, 1000).Value = BotoxData_FDP_1;
            cmd.Parameters.Add("@BotoxData_FDP_2", SqlDbType.VarChar, 1000).Value = BotoxData_FDP_2;
            cmd.Parameters.Add("@BotoxData_FDP_3", SqlDbType.VarChar, 1000).Value = BotoxData_FDP_3;
            cmd.Parameters.Add("@BotoxData_FDP_4", SqlDbType.VarChar, 1000).Value = BotoxData_FDP_4;
            cmd.Parameters.Add("@BotoxData_FPL_1", SqlDbType.VarChar, 1000).Value = BotoxData_FPL_1;
            cmd.Parameters.Add("@BotoxData_FPL_2", SqlDbType.VarChar, 1000).Value = BotoxData_FPL_2;
            cmd.Parameters.Add("@BotoxData_FPL_3", SqlDbType.VarChar, 1000).Value = BotoxData_FPL_3;
            cmd.Parameters.Add("@BotoxData_FPL_4", SqlDbType.VarChar, 1000).Value = BotoxData_FPL_4;
            cmd.Parameters.Add("@BotoxData_Adductor_1", SqlDbType.VarChar, 1000).Value = BotoxData_Adductor_1;
            cmd.Parameters.Add("@BotoxData_Adductor_2", SqlDbType.VarChar, 1000).Value = BotoxData_Adductor_2;
            cmd.Parameters.Add("@BotoxData_Adductor_3", SqlDbType.VarChar, 1000).Value = BotoxData_Adductor_3;
            cmd.Parameters.Add("@BotoxData_Adductor_4", SqlDbType.VarChar, 1000).Value = BotoxData_Adductor_4;
            cmd.Parameters.Add("@BotoxData_Intrinsics_1", SqlDbType.VarChar, 1000).Value = BotoxData_Intrinsics_1;
            cmd.Parameters.Add("@BotoxData_Intrinsics_2", SqlDbType.VarChar, 1000).Value = BotoxData_Intrinsics_2;
            cmd.Parameters.Add("@BotoxData_Intrinsics_3", SqlDbType.VarChar, 1000).Value = BotoxData_Intrinsics_3;
            cmd.Parameters.Add("@BotoxData_Intrinsics_4", SqlDbType.VarChar, 1000).Value = BotoxData_Intrinsics_4;
            cmd.Parameters.Add("@BotoxData_Casting_1", SqlDbType.VarChar, 1000).Value = BotoxData_Casting_1;
            cmd.Parameters.Add("@BotoxData_Casting_2", SqlDbType.VarChar, 1000).Value = BotoxData_Casting_2;
            cmd.Parameters.Add("@BotoxData_Casting_3", SqlDbType.VarChar, 1000).Value = BotoxData_Casting_3;
            cmd.Parameters.Add("@BotoxData_Casting_4", SqlDbType.VarChar, 1000).Value = BotoxData_Casting_4;
            cmd.Parameters.Add("@Doctor_Director", SqlDbType.Int).Value = Doctor_Director;
            cmd.Parameters.Add("@Doctor_Physiotheraist", SqlDbType.Int).Value = Doctor_Physiotheraist;
            cmd.Parameters.Add("@Doctor_Occupational", SqlDbType.Int).Value = Doctor_Occupational;
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
    }
}
