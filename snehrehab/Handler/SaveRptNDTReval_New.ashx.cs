using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace snehrehab.Handler
{
    /// <summary>
    /// Summary description for SaveRptNDTReval_New
    /// </summary>
    public class SaveRptNDTReval_New : IHttpHandler, IRequiresSessionState
    {
        DbHelper.SqlDb db; int _loginID = 0;
        SnehBLL.ReportNdtMst_Bll RDB = new SnehBLL.ReportNdtMst_Bll();
        public SaveRptNDTReval_New()
        {
            db = new DbHelper.SqlDb();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            rModel r = new rModel();
            if (_loginID <= 0)
            {
                LogIssue(context, "Session expired / invalid login");
                context.Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
                return;
            }
            try
            {
                string record = GetStr(context, "Record");
                string action = GetStr(context, "Action"); // optional (can remove if not needed)
                int tabNo = ToInt(context.Request.Form["TabNo"]);

                if (string.IsNullOrEmpty(record) || !DbHelper.Configuration.IsGuid(record))
                {
                    LogIssue(context, "Invalid record guid: " + record);
                    context.Response.StatusCode = 400;
                    context.Response.Write("ERROR|Invalid record guid");
                    return;
                }

                int appointmentID = SnehBLL.Appointments_Bll.Check(record);

                if (appointmentID <= 0)
                {
                    LogIssue(context, "Invalid AppointmentID for record: " + record);
                    context.Response.StatusCode = 400;
                    context.Response.Write("ERROR|Invalid AppointmentID");
                    return;
                }

                // ✅ Tab Save (tabNo required)
                if (tabNo <= 0)
                {
                    LogIssue(context, "Invalid TabNo: " + tabNo);
                    context.Response.StatusCode = 400;
                    context.Response.Write("ERROR|Invalid TabNo");
                    return;
                }
                string actionType = context.Request.Form["ActionType"];

                if (actionType == "MODAL_LOG")
                {
                    context.Response.ContentType = "application/json";

                    try
                    {
                        string logAction = context.Request.Form["LogAction"];
                        string modalLog = context.Request.Form["ModalLog"];

                        LogModal(
                            context,
                            "MODAL " + logAction + " | Data: " + modalLog,
                            null,
                            appointmentID
                        );

                        context.Response.Write("{\"status\":true,\"message\":\"Log saved\"}");
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.Write("{\"status\":false,\"message\":\"" + ex.Message + "\"}");
                    }

                    return;
                }
                int retVal = 0;

                LogRequest(context, tabNo, appointmentID);
                // TAB WISE CALL
                switch (tabNo)
                {
                    case 1:
                        retVal = SaveTab1(context, appointmentID);
                        break;

                    case 2:
                        retVal = SaveTab2(context, appointmentID);
                        break;

                    case 3:
                        retVal = SaveTab3(context, appointmentID);
                        break;
                    case 4:
                        retVal = SaveTab4(context, appointmentID);
                        break;
                    case 5:
                        retVal = SaveTab5(context, appointmentID);
                        break;
                    case 6:
                        retVal = SaveTab6(context, appointmentID);
                        break;
                    case 7:
                        retVal = SaveTab7(context, appointmentID);
                        break;
                    case 8:
                        retVal = SaveTab8(context, appointmentID);
                        break;
                    case 9:
                        retVal = SaveTab9(context, appointmentID);
                        break;
                    case 10:
                        retVal = SaveTab10(context, appointmentID);
                        break;
                    case 11:
                        retVal = SaveTab11(context, appointmentID);
                        break;
                    case 12:
                        retVal = SaveTab12(context, appointmentID);
                        break;
                    case 13:
                        retVal = SaveTab13(context, appointmentID);
                        break;

                    case 14: retVal = SaveTab14(context, appointmentID); break;
                    case 15: retVal = SaveTab15(context, appointmentID); break;
                    case 16: retVal = SaveTab16(context, appointmentID); break;
                    case 17: retVal = SaveTab17(context, appointmentID); break;
                    case 18: retVal = SaveTab18(context, appointmentID); break;
                    case 19: retVal = SaveTab19(context, appointmentID); break;
                    case 20: retVal = SaveTab20(context, appointmentID); break;
                    case 21: retVal = SaveTab21(context, appointmentID); break;
                    default:
                        LogIssue(context, "Unsupported TabNo: " + tabNo);
                        context.Response.StatusCode = 400;
                        context.Response.Write("ERROR|Tab not supported");
                        return;
                }

                context.Response.Write("OK|" + retVal);
            }
            catch (Exception ex)
            {
                try
                {
                    string logDir = context.Server.MapPath("~/Logs");

                    if (!Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }

                    string logFile = Path.Combine(
                        logDir,
                        "error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"
                    );

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("===== ERROR =====");
                    sb.AppendLine("Time   : " + DateTime.Now);
                    sb.AppendLine("URL    : " + context.Request.Url);
                    sb.AppendLine("Method : " + context.Request.HttpMethod);
                    sb.AppendLine("IP     : " + context.Request.UserHostAddress);
                    sb.AppendLine("Error  : " + ex.Message);
                    sb.AppendLine("Stack  : " + ex.StackTrace);

                    if (ex.InnerException != null)
                    {
                        sb.AppendLine("Inner  : " + ex.InnerException.Message);
                    }

                    sb.AppendLine("FORM DATA:");
                    foreach (string key in context.Request.Form.AllKeys)
                    {
                        sb.AppendLine(key + " = " + context.Request.Form[key]);
                    }

                    sb.AppendLine("=================\n");

                    File.AppendAllText(logFile, sb.ToString());
                }
                catch
                {
                    // Avoid crash if logging fails
                }
            }
        }

        private string ResolveClientUrl(string sessionOutURL)
        {
            throw new NotImplementedException();
        }

        private int SaveTab1(HttpContext context, int appointmentID)
        {
            int tabNo = 1;
            string diagnosisID = GetStr(context, "txtDiagnosis");
            string diagnosisOther = GetStr(context, "txtDiagnosisOther");

            string currentConcern = GetStr(context, "DataCollection_CurrentConcern");
            string improvement = GetStr(context, "DataCollection_ImprovementsSinceLastEval");
            string medicalHistory = GetStr(context, "DataCollection_MedicalHistory");
            string dailyRoutine = GetStr(context, "DataCollection_DailyRoutine");
            string expectaion = GetStr(context, "DataCollection_Expectaion");
            string therapyHistory = GetStr(context, "DataCollection_TherapyHistory");
            string sources = GetStr(context, "DataCollection_Sources");
            string numberVisit = GetStr(context, "DataCollection_NumberVisit");
            string adaptedEquipment = GetStr(context, "DataCollection_AdaptedEquipment");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure; ;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
            AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

            AddParam(cmd, "@DiagnosisID", SqlDbType.NVarChar, DbNullIfEmpty(diagnosisID));
            AddParam(cmd, "@DiagnosisOther", SqlDbType.NVarChar, DbNullIfEmpty(diagnosisOther));

            AddParam(cmd, "@DataCollection_CurrentConcern", SqlDbType.NVarChar, DbNullIfEmpty(currentConcern));
            AddParam(cmd, "@DataCollection_ImprovementSinceLastEval", SqlDbType.NVarChar, DbNullIfEmpty(improvement));
            AddParam(cmd, "@DataCollection_MedicalHistory", SqlDbType.NVarChar, DbNullIfEmpty(medicalHistory));
            AddParam(cmd, "@DataCollection_DailyRoutine", SqlDbType.NVarChar, DbNullIfEmpty(dailyRoutine));
            AddParam(cmd, "@DataCollection_Expectaion", SqlDbType.NVarChar, DbNullIfEmpty(expectaion));
            AddParam(cmd, "@DataCollection_TherapyHistory", SqlDbType.NVarChar, DbNullIfEmpty(therapyHistory));
            AddParam(cmd, "@DataCollection_Sources", SqlDbType.NVarChar, DbNullIfEmpty(sources));
            AddParam(cmd, "@DataCollection_NumberVisit", SqlDbType.NVarChar, DbNullIfEmpty(numberVisit));
            AddParam(cmd, "@DataCollection_AdaptedEquipment", SqlDbType.NVarChar, DbNullIfEmpty(adaptedEquipment));

            // Output param
            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);
            return ToInt(ret.Value);
        }

        private int SaveTab2(HttpContext context, int appointmentID)
        {
            int tabNo = 2;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            SqlParameter pRet = new SqlParameter("@RetVal", SqlDbType.Int);
            pRet.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pRet);

            // helper local function (empty => DBNull)
            object V(string key)
            {
                string val = GetStr(context, key);
                return string.IsNullOrWhiteSpace(val) ? (object)DBNull.Value : val.Trim();
            }

            // ---- TAB 2 PARAMS (match SP names exactly) ----
            AddParam(cmd, "@Morphology_Height", SqlDbType.NVarChar, V("Morphology_Height"));
            AddParam(cmd, "@Morphology_Weight", SqlDbType.NVarChar, V("Morphology_Weight"));
            AddParam(cmd, "@Morphology_Head", SqlDbType.NVarChar, V("Morphology_Head"));
            AddParam(cmd, "@Morphology_Nipple", SqlDbType.NVarChar, V("Morphology_Nipple"));
            AddParam(cmd, "@Morphology_Waist", SqlDbType.NVarChar, V("Morphology_Waist"));

            AddParam(cmd, "@Morphology_LimbLeft", SqlDbType.NVarChar, V("Morphology_LimbLeft"));
            AddParam(cmd, "@Morphology_LimbRight", SqlDbType.NVarChar, V("Morphology_LimbRight"));

            AddParam(cmd, "@Morphology_TrueLimbLengthLeft", SqlDbType.NVarChar, V("Morphology_TrueLimbLengthLeft"));
            AddParam(cmd, "@Morphology_TrueLimbLengthRight", SqlDbType.NVarChar, V("Morphology_TrueLimbLengthRight"));

            AddParam(cmd, "@Morphology_ArmLeft", SqlDbType.NVarChar, V("Morphology_ArmLeft"));
            AddParam(cmd, "@Morphology_ArmRight", SqlDbType.NVarChar, V("Morphology_ArmRight"));
            AddParam(cmd, "@Morphology_ArmLength", SqlDbType.NVarChar, V("Morphology_ArmLength"));

            // Upper Limb - Above Elbow
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLevel1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLevel2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLevel3"));

            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLeft1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLeft2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowLeft3"));

            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowRight1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowRight2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Above_ElbowRight3"));

            // Upper Limb - At Elbow
            AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowLevel", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_At_ElbowLevel"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowLeft", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_At_ElbowLeft"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowRight", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_At_ElbowRight"));

            // Upper Limb - Below Elbow
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLevel1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLevel2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLevel3"));

            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLeft1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLeft2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowLeft3"));

            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight1", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowRight1"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight2", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowRight2"));
            AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight3", SqlDbType.NVarChar, V("Morphology_GirthUpperLimb_Below_ElbowRight3"));

            // Lower Limb - Above Knee
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLevel1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLevel2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLevel3"));

            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLeft1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLeft2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeLeft3"));

            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeRight1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeRight2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Above_KneeRight3"));

            // Lower Limb - At Knee
            AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeLevel", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_At_KneeLevel"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeLeft", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_At_KneeLeft"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeRight", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_At_KneeRight"));

            // Lower Limb - Below Knee
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLevel1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLevel2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLevel3"));

            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLeft1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLeft2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeLeft3"));

            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight1", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeRight1"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight2", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeRight2"));
            AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight3", SqlDbType.NVarChar, V("Morphology_GirthLowerLimb_Below_KneeRight3"));

            AddParam(cmd, "@Morphology_OralMotorFactors", SqlDbType.NVarChar, V("Morphology_OralMotorFactors"));

            db.DbUpdate(cmd);



            return ToInt(cmd.Parameters["@RetVal"].Value);
        }
        private int SaveTab3(HttpContext context, int appointmentID)
        {
            int tabNo = 3;

            string grossMotor = GetStr(context, "FunctionalActivities_GrossMotor");
            string handFunction = GetStr(context, "FunctionalActivities_HandFunction");
            string fineMotor = GetStr(context, "FunctionalActivities_FineMotor");
            string adl = GetStr(context, "FunctionalActivities_ADL");
            string oralMotor = GetStr(context, "FunctionalActivities_OralMotor");
            string communication = GetStr(context, "FunctionalActivities_Communication");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;
            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
            AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

            AddParam(cmd, "@FunctionalActivities_GrossMotor", SqlDbType.NVarChar, DbNullIfEmpty(grossMotor));
            AddParam(cmd, "@FunctionalActivities_HandFunction", SqlDbType.NVarChar, DbNullIfEmpty(handFunction));
            AddParam(cmd, "@FunctionalActivities_FineMotor", SqlDbType.NVarChar, DbNullIfEmpty(fineMotor));
            AddParam(cmd, "@FunctionalActivities_ADL", SqlDbType.NVarChar, DbNullIfEmpty(adl));
            AddParam(cmd, "@FunctionalActivities_OralMotor", SqlDbType.NVarChar, DbNullIfEmpty(oralMotor));
            AddParam(cmd, "@FunctionalActivities_Communication", SqlDbType.NVarChar, DbNullIfEmpty(communication));

            // Output param
            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);


            return ToInt(ret.Value);
        }
        private int SaveTab4(HttpContext context, int appointmentID)
        {
            int tabNo = 4;

            string gmfcs = GetStr(context, "TestMeasures_GMFCS");
            string gmfm = GetStr(context, "TestMeasures_GMFM");
            string gmpm = GetStr(context, "TestMeasures_GMPM");
            string ashworth = GetStr(context, "TestMeasures_AshworthScale");
            string tradieus = GetStr(context, "TestMeasures_TradieusScale");
            string ogs = GetStr(context, "TestMeasures_OGS");
            string melbourne = GetStr(context, "TestMeasures_Melbourne");
            string copm = GetStr(context, "TestMeasures_COPM");
            string clinicalObs = GetStr(context, "TestMeasures_ClinicalObservation");
            string others = GetStr(context, "TestMeasures_Others");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
            AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

            AddParam(cmd, "@TestMeasures_GMFCS", SqlDbType.VarChar, DbNullIfEmpty(gmfcs));
            AddParam(cmd, "@TestMeasures_GMFM", SqlDbType.VarChar, DbNullIfEmpty(gmfm));
            AddParam(cmd, "@TestMeasures_GMPM", SqlDbType.VarChar, DbNullIfEmpty(gmpm));
            AddParam(cmd, "@TestMeasures_AshworthScale", SqlDbType.VarChar, DbNullIfEmpty(ashworth));
            AddParam(cmd, "@TestMeasures_TradieusScale", SqlDbType.VarChar, DbNullIfEmpty(tradieus));
            AddParam(cmd, "@TestMeasures_OGS", SqlDbType.VarChar, DbNullIfEmpty(ogs));
            AddParam(cmd, "@TestMeasures_Melbourne", SqlDbType.VarChar, DbNullIfEmpty(melbourne));
            AddParam(cmd, "@TestMeasures_COPM", SqlDbType.VarChar, DbNullIfEmpty(copm));
            AddParam(cmd, "@TestMeasures_ClinicalObservation", SqlDbType.VarChar, DbNullIfEmpty(clinicalObs));
            AddParam(cmd, "@TestMeasures_Others", SqlDbType.VarChar, DbNullIfEmpty(others));

            // Output param
            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);


            return ToInt(ret.Value);
        }
        private int SaveTab5(HttpContext context, int appointmentID)
        {
            int tabNo = 5;

            string alignment = GetStr(context, "Posture_Alignment");
            string biomechanics = GetStr(context, "Posture_Biomechanics");
            string stability = GetStr(context, "Posture_Stability");
            string anticipatory = GetStr(context, "Posture_Anticipatory");
            string postural = GetStr(context, "Posture_Postural");
            string signsPostural = GetStr(context, "Posture_SignsPostural");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            cmd.Parameters.Add("@Posture_Alignment", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(alignment);

            cmd.Parameters.Add("@Posture_Biomechanics", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(biomechanics);

            cmd.Parameters.Add("@Posture_Stability", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(stability);

            cmd.Parameters.Add("@Posture_Anticipatory", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(anticipatory);

            cmd.Parameters.Add("@Posture_Postural", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(postural);

            cmd.Parameters.Add("@Posture_SignsPostural", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(signsPostural);

            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab6(HttpContext context, int appointmentID)
        {
            int tabNo = 6;

            string inertia = GetStr(context, "Movement_Inertia");
            string strategies = GetStr(context, "Movement_Strategies");
            string extremities = GetStr(context, "Movement_Extremities");
            string stability = GetStr(context, "Movement_Stability");
            string overuse = GetStr(context, "Movement_Overuse");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            cmd.Parameters.Add("@Movement_Inertia", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(inertia);

            cmd.Parameters.Add("@Movement_Strategies", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(strategies);

            cmd.Parameters.Add("@Movement_Extremities", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(extremities);

            cmd.Parameters.Add("@Movement_Stability", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(stability);

            cmd.Parameters.Add("@Movement_Overuse", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(overuse);

            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab7(HttpContext context, int appointmentID)
        {
            int tabNo = 7;

            string integration = GetStr(context, "Others_Integration");
            string assessments = GetStr(context, "Others_Assessments");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            cmd.Parameters.Add("@Others_Integration", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(integration);

            cmd.Parameters.Add("@Others_Assessments", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(assessments);

            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab8(HttpContext context, int appointmentID)
        {
            int tabNo = 8;

            string attention = GetStr(context, "Attention");
            string affect = GetStr(context, "Affect");
            string action = GetStr(context, "Action");
            string regulatoryArousal = GetStr(context, "Regulatory_Arousal");
            string regulatoryRegulation = GetStr(context, "Regulatory_Regulation");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            cmd.Parameters.Add("@Attention", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(attention);

            cmd.Parameters.Add("@Affect", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(affect);

            cmd.Parameters.Add("@Action", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(action);

            cmd.Parameters.Add("@Regulatory_Arousal", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(regulatoryArousal);

            cmd.Parameters.Add("@Regulatory_Regulation", SqlDbType.VarChar, -1)
                .Value = DbNullIfEmpty(regulatoryRegulation);

            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab9(HttpContext context, int appointmentID)
        {
            int tabNo = 9;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            // ROM 1
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExtensionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipAbductionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipAbductionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipAbductionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExtensionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExtensionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExternalLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipExternalRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExternalRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipInternalLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_HipInternalRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipInternalRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PoplitealLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PoplitealRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PoplitealRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeExtensionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_KneeExtensionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeExtensionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionExtensionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_DorsiflexionExtensionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionExtensionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PlantarFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_PlantarFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PlantarFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_OthersLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom1_OthersRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_OthersRight"));

            // ROM 2
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderExtensionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ShoulderExtensionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderExtensionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_HorizontalAbductionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_HorizontalAbductionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_HorizontalAbductionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ExternalRotationLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ExternalRotationRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ExternalRotationRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_InternalRotationLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_InternalRotationRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_InternalRotationRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowExtensionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_ElbowExtensionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowExtensionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_SupinationLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_SupinationRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_SupinationRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_PronationLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_PronationRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_PronationRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristFlexionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristFlexionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristFlexionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristExtesionLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_WristExtesionRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristExtesionRight"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_OthersLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Rom2_OthersRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_OthersRight"));

            // Strength
            cmd.Parameters.Add("@Musculoskeletal_Strengthlp", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Strengthlp"));
            cmd.Parameters.Add("@Musculoskeletal_StrengthCC", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthCC"));
            cmd.Parameters.Add("@Musculoskeletal_StrengthMuscle", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthMuscle"));
            cmd.Parameters.Add("@Musculoskeletal_StrengthSkeletal", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthSkeletal"));

            // MMT (major)
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HipflexorsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HipflexorsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HipflexorsRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorsRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorsRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HamsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_HamsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HamsRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_QuadsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_QuadsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_QuadsRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisAnteriorLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisAnteriorRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisAnteriorRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisPosteriorLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TibialisPosteriorRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisPosteriorRight"));

            // Thumb (end)
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_OpponensPollicisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_OpponensPollicisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_OpponensPollicisRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorPollicisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorPollicisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorPollicisRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorPollicisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AbductorPollicisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorPollicisRight"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorPollicisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorPollicisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorPollicisRight"));
            // ExtensorDigitorum
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorDigitorumLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorDigitorumRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorDigitorumRight"));
            // ExtensorHallucis
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorHallucisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ExtensorHallucisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorHallucisRight"));
            // Peronei
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PeroneiLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PeroneiRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PeroneiRight"));
            // FlexorDigitorum
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorDigitorumLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorDigitorumRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorDigitorumRight"));
            // FlexorHallucis
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorHallucisLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FlexorHallucisRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorHallucisRight"));
            // AnteriorDeltoid
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AnteriorDeltoidLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_AnteriorDeltoidRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AnteriorDeltoidRight"));
            // PosteriorDeltoid
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PosteriorDeltoidLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PosteriorDeltoidRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PosteriorDeltoidRight"));
            // MiddleDeltoid
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_MiddleDeltoidLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_MiddleDeltoidRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_MiddleDeltoidRight"));
            // Supraspinatus
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupraspinatusLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupraspinatusRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupraspinatusRight"));
            // SerratusAnterior
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SerratusAnteriorLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SerratusAnteriorRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SerratusAnteriorRight"));
            // Rhomboids
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_RhomboidsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_RhomboidsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_RhomboidsRight"));
            // Biceps
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_BicepsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_BicepsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_BicepsRight"));
            // Triceps
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TricepsLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_TricepsRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TricepsRight"));
            // Supinator
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupinatorLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_SupinatorRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupinatorRight"));
            // Pronator
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PronatorLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_PronatorRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PronatorRight"));
            // ECU
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECULeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECULeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECURight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECURight"));
            // ECR
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECRLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECRRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECRRight"));
            // ECS
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECSLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_ECSRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECSRight"));
            // FCU
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCULeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCULeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCURight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCURight"));
            // FCR
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCRLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCRRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCRRight"));
            // FCS
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSLeft", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCSLeft"));
            cmd.Parameters.Add("@Musculoskeletal_Mmt_FCSRight", SqlDbType.VarChar, -1).Value = DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCSRight"));
            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab10(HttpContext context, int appointmentID)
        {
            int tabNo = 10;

            string neuromotorControl = GetStr(context, "SignOfCNS_NeuromotorControl");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
            cmd.Parameters.Add("@TabNo", SqlDbType.Int).Value = tabNo;

            cmd.Parameters.Add("@SignOfCNS_NeuromotorControl", SqlDbType.VarChar, -1)
                .Value = string.IsNullOrEmpty(neuromotorControl) ? DBNull.Value : (object)neuromotorControl;

            SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab11(HttpContext context, int appointmentID)
        {
            int tabValue = 11;

            string SustainGeneral = GetStr(context, "RemarkVariable_SustainGeneral");
            string ContractionsGeneral = GetStr(context, "RemarkVariable_ContractionsGeneral");
            string AntagonistGeneral = GetStr(context, "RemarkVariable_AntagonistGeneral");
            string SynergyGeneral = GetStr(context, "RemarkVariable_SynergyGeneral");
            string PosturalGeneral = GetStr(context, "RemarkVariable_PosturalGeneral");
            string StiffinessGeneral = GetStr(context, "RemarkVariable_StiffinessGeneral");
            string ExtraneousGeneral = GetStr(context, "RemarkVariable_ExtraneousGeneral");

            string SustainControl = GetStr(context, "RemarkVariable_SustainControl");
            string PosturalControl = GetStr(context, "RemarkVariable_PosturalControl");
            string ContractionsControl = GetStr(context, "RemarkVariable_ContractionsControl");
            string AntagonistControl = GetStr(context, "RemarkVariable_AntagonistControl");
            string SynergyControl = GetStr(context, "RemarkVariable_SynergyControl");
            string StiffinessControl = GetStr(context, "RemarkVariable_StiffinessControl");
            string ExtraneousControl = GetStr(context, "RemarkVariable_ExtraneousControl");

            string SustainTiming = GetStr(context, "RemarkVariable_SustainTiming");
            string PosturalTiming = GetStr(context, "RemarkVariable_PosturalTiming");
            string ContractionsTiming = GetStr(context, "RemarkVariable_ContractionsTiming");
            string AntagonistTiming = GetStr(context, "RemarkVariable_AntagonistTiming");
            string SynergyTiming = GetStr(context, "RemarkVariable_SynergyTiming");
            string StiffinessTiming = GetStr(context, "RemarkVariable_StiffinessTiming");
            string ExtraneousTiming = GetStr(context, "RemarkVariable_ExtraneousTiming");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            // GENERAL
            AddParam(cmd, "@RemarkVariable_SustainGeneral", SqlDbType.VarChar, DbNullIfEmpty(SustainGeneral));
            AddParam(cmd, "@RemarkVariable_ContractionsGeneral", SqlDbType.VarChar, DbNullIfEmpty(ContractionsGeneral));
            AddParam(cmd, "@RemarkVariable_AntagonistGeneral", SqlDbType.VarChar, DbNullIfEmpty(AntagonistGeneral));
            AddParam(cmd, "@RemarkVariable_SynergyGeneral", SqlDbType.VarChar, DbNullIfEmpty(SynergyGeneral));
            AddParam(cmd, "@RemarkVariable_PosturalGeneral", SqlDbType.VarChar, DbNullIfEmpty(PosturalGeneral));
            AddParam(cmd, "@RemarkVariable_StiffinessGeneral", SqlDbType.VarChar, DbNullIfEmpty(StiffinessGeneral));
            AddParam(cmd, "@RemarkVariable_ExtraneousGeneral", SqlDbType.VarChar, DbNullIfEmpty(ExtraneousGeneral));

            // CONTROL
            AddParam(cmd, "@RemarkVariable_SustainControl", SqlDbType.VarChar, DbNullIfEmpty(SustainControl));
            AddParam(cmd, "@RemarkVariable_PosturalControl", SqlDbType.VarChar, DbNullIfEmpty(PosturalControl));
            AddParam(cmd, "@RemarkVariable_ContractionsControl", SqlDbType.VarChar, DbNullIfEmpty(ContractionsControl));
            AddParam(cmd, "@RemarkVariable_AntagonistControl", SqlDbType.VarChar, DbNullIfEmpty(AntagonistControl));
            AddParam(cmd, "@RemarkVariable_SynergyControl", SqlDbType.VarChar, DbNullIfEmpty(SynergyControl));
            AddParam(cmd, "@RemarkVariable_StiffinessControl", SqlDbType.VarChar, DbNullIfEmpty(StiffinessControl));
            AddParam(cmd, "@RemarkVariable_ExtraneousControl", SqlDbType.VarChar, DbNullIfEmpty(ExtraneousControl));

            // TIMING
            AddParam(cmd, "@RemarkVariable_SustainTiming", SqlDbType.VarChar, DbNullIfEmpty(SustainTiming));
            AddParam(cmd, "@RemarkVariable_PosturalTiming", SqlDbType.VarChar, DbNullIfEmpty(PosturalTiming));
            AddParam(cmd, "@RemarkVariable_ContractionsTiming", SqlDbType.VarChar, DbNullIfEmpty(ContractionsTiming));
            AddParam(cmd, "@RemarkVariable_AntagonistTiming", SqlDbType.VarChar, DbNullIfEmpty(AntagonistTiming));
            AddParam(cmd, "@RemarkVariable_SynergyTiming", SqlDbType.VarChar, DbNullIfEmpty(SynergyTiming));
            AddParam(cmd, "@RemarkVariable_StiffinessTiming", SqlDbType.VarChar, DbNullIfEmpty(StiffinessTiming));
            AddParam(cmd, "@RemarkVariable_ExtraneousTiming", SqlDbType.VarChar, DbNullIfEmpty(ExtraneousTiming));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab12(HttpContext context, int appointmentID)
        {
            int tabValue = 12;

            string Vision = GetStr(context, "SensorySystem_Vision");
            string Somatosensory = GetStr(context, "SensorySystem_Somatosensory");
            string Vestibular = GetStr(context, "SensorySystem_Vestibular");
            string Auditory = GetStr(context, "SensorySystem_Auditory");
            string Gustatory = GetStr(context, "SensorySystem_Gustatory");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@SensorySystem_Vision", SqlDbType.VarChar, DbNullIfEmpty(Vision));
            AddParam(cmd, "@SensorySystem_Somatosensory", SqlDbType.VarChar, DbNullIfEmpty(Somatosensory));
            AddParam(cmd, "@SensorySystem_Vestibular", SqlDbType.VarChar, DbNullIfEmpty(Vestibular));
            AddParam(cmd, "@SensorySystem_Auditory", SqlDbType.VarChar, DbNullIfEmpty(Auditory));
            AddParam(cmd, "@SensorySystem_Gustatory", SqlDbType.VarChar, DbNullIfEmpty(Gustatory));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab13(HttpContext context, int appointmentID)
        {
            int tabValue = 13;

            string sensoryProfile = GetStr(context, "SensoryProfile_Profile");
            string sensoryNameResultsJson = GetStr(context, "Sensory_Profile_NameResults");

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
            AddParam(cmd, "@SensoryProfile_Profile", SqlDbType.VarChar, DbNullIfEmpty(sensoryProfile));
            AddParam(cmd, "@Sensory_Profile_NameResults", SqlDbType.VarChar, DbNullIfEmpty(sensoryNameResultsJson));
            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab14(HttpContext context, int appointmentID)
        {
            int tabValue = 14;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@SIPTInfo_History", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_History")));

            AddParam(cmd, "@SIPTInfo_HandFunction1_GraspRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_GraspRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_GraspLeft")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_SphericalRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_SphericalLeft")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_HookRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_HookRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_HookLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_HookLeft")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_JawChuckRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_JawChuckLeft")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_GripRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_GripRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_GripLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_GripLeft")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_ReleaseRight")));
            AddParam(cmd, "@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction1_ReleaseLeft")));

            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionLfR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionLfL")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionMFR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionMFL")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionRFR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_OppositionRFL")));

            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchLfR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchLfL")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchMFR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchMFL")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchRFR")));
            AddParam(cmd, "@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_HandFunction2_PinchRFL")));

            AddParam(cmd, "@SIPTInfo_SIPT3_Spontaneous", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT3_Spontaneous")));
            AddParam(cmd, "@SIPTInfo_SIPT3_Command", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT3_Command")));

            AddParam(cmd, "@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Kinesthesia")));
            AddParam(cmd, "@SIPTInfo_SIPT4_Finger", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Finger")));
            AddParam(cmd, "@SIPTInfo_SIPT4_Localisation", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Localisation")));
            AddParam(cmd, "@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_DoubleTactile")));
            AddParam(cmd, "@SIPTInfo_SIPT4_Tactile", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Tactile")));
            AddParam(cmd, "@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Graphesthesia")));
            AddParam(cmd, "@SIPTInfo_SIPT4_PostRotary", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_PostRotary")));
            AddParam(cmd, "@SIPTInfo_SIPT4_Standing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT4_Standing")));

            AddParam(cmd, "@SIPTInfo_SIPT5_Color", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Color")));
            AddParam(cmd, "@SIPTInfo_SIPT5_Form", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Form")));
            AddParam(cmd, "@SIPTInfo_SIPT5_Size", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Size")));
            AddParam(cmd, "@SIPTInfo_SIPT5_Depth", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Depth")));
            AddParam(cmd, "@SIPTInfo_SIPT5_Figure", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Figure")));
            AddParam(cmd, "@SIPTInfo_SIPT5_Motor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT5_Motor")));

            AddParam(cmd, "@SIPTInfo_SIPT6_Design", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT6_Design")));
            AddParam(cmd, "@SIPTInfo_SIPT6_Constructional", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT6_Constructional")));

            AddParam(cmd, "@SIPTInfo_SIPT7_Scanning", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT7_Scanning")));
            AddParam(cmd, "@SIPTInfo_SIPT7_Memory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT7_Memory")));

            AddParam(cmd, "@SIPTInfo_SIPT8_Postural", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT8_Postural")));
            AddParam(cmd, "@SIPTInfo_SIPT8_Oral", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT8_Oral")));
            AddParam(cmd, "@SIPTInfo_SIPT8_Sequencing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT8_Sequencing")));
            AddParam(cmd, "@SIPTInfo_SIPT8_Commands", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT8_Commands")));

            AddParam(cmd, "@SIPTInfo_SIPT9_Bilateral", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT9_Bilateral")));
            AddParam(cmd, "@SIPTInfo_SIPT9_Contralat", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT9_Contralat")));
            AddParam(cmd, "@SIPTInfo_SIPT9_PreferredHand", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT9_PreferredHand")));
            AddParam(cmd, "@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT9_CrossingMidline")));

            AddParam(cmd, "@SIPTInfo_SIPT10_Draw", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_Draw")));
            AddParam(cmd, "@SIPTInfo_SIPT10_ClockFace", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_ClockFace")));
            AddParam(cmd, "@SIPTInfo_SIPT10_Filtering", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_Filtering")));
            AddParam(cmd, "@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_MotorPlanning")));
            AddParam(cmd, "@SIPTInfo_SIPT10_BodyImage", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_BodyImage")));
            AddParam(cmd, "@SIPTInfo_SIPT10_BodySchema", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_BodySchema")));
            AddParam(cmd, "@SIPTInfo_SIPT10_Laterality", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_SIPT10_Laterality")));

            AddParam(cmd, "@SIPTInfo_ActivityGiven_Remark", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Remark")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_InterestActivity")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_InterestCompletion")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Learning", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Learning")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Complexity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Complexity")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_ProblemSolving")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Concentration", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Concentration")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Retension", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Retension")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_SpeedPerfom")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Neatness", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Neatness")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Frustation", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Frustation")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Work", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Work")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_Reaction", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_Reaction")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_SociabilityTherapist")));
            AddParam(cmd, "@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SIPTInfo_ActivityGiven_SociabilityStudents")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab15(HttpContext context, int appointmentID)
        {
            int tabValue = 15;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Cognition_Intelligence", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_Intelligence")));
            AddParam(cmd, "@Cognition_Attention", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_Attention")));
            AddParam(cmd, "@Cognition_Memory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_Memory")));
            AddParam(cmd, "@Cognition_Adaptibility", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_Adaptibility")));
            AddParam(cmd, "@Cognition_MotorPlanning", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_MotorPlanning")));
            AddParam(cmd, "@Cognition_ExecutiveFunction", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_ExecutiveFunction")));
            AddParam(cmd, "@Cognition_CognitiveFunctions", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition_CognitiveFunctions")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab16(HttpContext context, int appointmentID)
        {
            int tabValue = 16;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Integumentary_SkinIntegrity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Integumentary_SkinIntegrity")));
            AddParam(cmd, "@Integumentary_SkinColor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Integumentary_SkinColor")));
            AddParam(cmd, "@Integumentary_SkinExtensibility", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Integumentary_SkinExtensibility")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab17(HttpContext context, int appointmentID)
        {
            int tabValue = 17;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Respiratory_RateResting", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Respiratory_RateResting")));
            AddParam(cmd, "@Respiratory_PostExercise", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Respiratory_PostExercise")));
            AddParam(cmd, "@Respiratory_Patterns", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Respiratory_Patterns")));
            AddParam(cmd, "@Respiratory_BreathControl", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Respiratory_BreathControl")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab18(HttpContext context, int appointmentID)
        {
            int tabValue = 18;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Cardiovascular_HeartRate", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_HeartRate")));
            AddParam(cmd, "@Cardiovascular_PostExercise", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_PostExercise")));
            AddParam(cmd, "@Cardiovascular_BP", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_BP")));
            AddParam(cmd, "@Cardiovascular_Edema", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_Edema")));
            AddParam(cmd, "@Cardiovascular_Circulation", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_Circulation")));
            AddParam(cmd, "@Cardiovascular_EEi", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular_EEi")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab19(HttpContext context, int appointmentID)
        {
            int tabValue = 19;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Gastrointestinal_Bowel", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gastrointestinal_Bowel")));
            AddParam(cmd, "@Gastrointestinal_Intake", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gastrointestinal_Intake")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab20(HttpContext context, int appointmentID)
        {
            int tabValue = 20;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Evaluation_Strengths", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Strengths")));
            AddParam(cmd, "@Evalutionadding_Strengths", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evalutionadding_Strengths")));

            AddParam(cmd, "@Evaluation_Concern_Barriers", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Concern_Barriers")));
            AddParam(cmd, "@Evaluation_Concern_Limitations", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Concern_Limitations")));
            AddParam(cmd, "@Evaluation_Concern_Posture", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Concern_Posture")));
            AddParam(cmd, "@Evaluation_Concern_Impairment", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Concern_Impairment")));

            AddParam(cmd, "@Evaluation_Goal_Summary", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_Summary")));
            AddParam(cmd, "@Evaluation_Goal_ShortTearm_Previous", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_ShortTearm_Previous")));
            AddParam(cmd, "@Evaluation_Goal_Previous", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_Previous")));
            AddParam(cmd, "@Evaluation_Goal_LongTerm", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_LongTerm")));
            AddParam(cmd, "@Evaluation_Goal_ShortTerm", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_ShortTerm")));
            AddParam(cmd, "@Evaluation_Goal_Impairment", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Goal_Impairment")));

            AddParam(cmd, "@Evaluation_Plan_Frequency", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Plan_Frequency")));
            AddParam(cmd, "@Evaluation_Plan_Service", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Plan_Service")));
            AddParam(cmd, "@Evaluation_Plan_Strategies", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Plan_Strategies")));
            AddParam(cmd, "@Evaluation_Plan_Equipment", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Plan_Equipment")));
            AddParam(cmd, "@Evaluation_Plan_Education", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Evaluation_Plan_Education")));

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveTab21(HttpContext context, int appointmentID)
        {
            int tabValue = 21;

            SqlCommand cmd = new SqlCommand("ReportREEVALMst_Set_Tab_Wise");
            cmd.CommandType = CommandType.StoredProcedure;

            AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

            AddParam(cmd, "@Doctor_Physioptherapist", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Doctor_Physioptherapist")));
            AddParam(cmd, "@Doctor_Occupational", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Doctor_Occupational")));

            // In old code you were using DBNull.Value always
            AddParam(cmd, "@Doctor_EnterReport", SqlDbType.VarChar, DBNull.Value);

            AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

            SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
            ret.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ret);

            db.DbUpdate(cmd);

            return ToInt(ret.Value);
        }
        private int SaveFinalOnly(HttpContext context, int appointmentID)
        {
            bool isFinal = ToBool(context.Request["IsFinal"]);
            bool isGiven = ToBool(context.Request["IsGiven"]);
            string givenDateStr = GetStr(context, "GivenDate");

            DateTime givenDate = DateTime.MinValue;

            if (isGiven)
            {
                if (string.IsNullOrWhiteSpace(givenDateStr))
                    throw new Exception("Given Date is Required...");

                DateTime.TryParseExact(
                    givenDateStr.Trim(),
                    DbHelper.Configuration.showDateFormat,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out givenDate
                );
            }

            // 1) Save Flags
            using (SqlCommand cmd1 = new SqlCommand("SET_REVAL_FLAGS"))
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@AppointmentID", appointmentID);
                cmd1.Parameters.AddWithValue("@IsGiven", isGiven);
                cmd1.Parameters.AddWithValue("@IsFinal", isFinal);
                cmd1.Parameters.AddWithValue("@GivenDate", isGiven ? (object)givenDate : DBNull.Value);

                db.DbUpdate(cmd1);
            }

            // 2) Save Diagnosis
            string diagnosisIDs = GetStr(context, "DiagnosisIDs");
            string diagnosisOther = GetStr(context, "DiagnosisOther");

            DataSet ds = RDB.GetReval(appointmentID);

            int patientID = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out patientID);

            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = DIB.setFromOther(diagnosisIDs, diagnosisOther, patientID);

            if (g < 0)
                throw new Exception("Diagnosis already exist...");

            return 1;
        }
        private bool ToBool(object val)
        {
            bool b;
            bool.TryParse(Convert.ToString(val), out b);
            return b;
        }
        private static string GetStr(HttpContext context, string key)
        {
            // return empty string (best for COALESCE(NULLIF(@Param,''),Column))
            return (context.Request.Form[key] ?? "").Trim();
        }

        private static int ToInt(object val)
        {
            int x;
            int.TryParse(Convert.ToString(val), out x);
            return x;
        }
        private object DbNullIfEmpty(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? (object)DBNull.Value : text.Trim();
        }

        private void AddParam(SqlCommand cmd, string name, SqlDbType type, object value)
        {
            SqlParameter p = cmd.Parameters.Add(name, type);
            p.Value = value ?? DBNull.Value;
        }
        private void LogRequest(HttpContext context, int tabNo, int appointmentID)
        {
            try
            {
                // Directly under Reval_report
                string logDir = context.Server.MapPath(
                    "~/" + Path.Combine("Logs", "Reval_report")
                );

                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // File name based on AppointmentID
                string logFile = Path.Combine(logDir, $"{appointmentID}.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("====================== Reval report SAVE LOG =================================");
                sb.AppendLine("Time          : " + DateTime.Now);
                sb.AppendLine("TabNo         : " + tabNo);
                sb.AppendLine("AppointmentID : " + appointmentID);
                sb.AppendLine("LoginId       : " + _loginID);
                sb.AppendLine("FORM DATA:");

                foreach (string key in context.Request.Form.AllKeys)
                {
                    sb.AppendLine($"{key} = {context.Request.Form[key]}");
                }

                sb.AppendLine("==============================================================================");

                File.AppendAllText(logFile, sb.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        private void LogIssue(HttpContext context, string message, Exception ex = null)
        {
            try
            {
                string logDir = context.Server.MapPath("~/Logs");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string logFile = Path.Combine(
                    logDir,
                    "error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"
                );

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("===== LOG =====");
                sb.AppendLine("Time   : " + DateTime.Now);
                sb.AppendLine("URL    : " + context.Request.Url);
                sb.AppendLine("Method : " + context.Request.HttpMethod);
                sb.AppendLine("IP     : " + context.Request.UserHostAddress);
                sb.AppendLine("Msg    : " + message);

                if (ex != null)
                {
                    sb.AppendLine("Error  : " + ex.Message);
                    sb.AppendLine("Stack  : " + ex.StackTrace);

                    if (ex.InnerException != null)
                        sb.AppendLine("Inner  : " + ex.InnerException.Message);
                }

                sb.AppendLine("FORM DATA:");
                foreach (string key in context.Request.Form.AllKeys)
                {
                    sb.AppendLine(key + " = " + context.Request.Form[key]);
                }

                sb.AppendLine("=================\n");

                File.AppendAllText(logFile, sb.ToString());
            }
            catch
            {
                // silent fail
            }
        }
        private void LogModal(HttpContext context, string message, Exception ex = null, int appointmentID = 0)
        {
            try
            {
                string logDir = context.Server.MapPath("~/Logs/Modal/Reval_report");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                // ✅ File per appointment
                string fileName = appointmentID > 0
                    ? appointmentID + ".txt"
                    : "general.txt";

                string logFile = Path.Combine(logDir, fileName);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("===== MODAL LOG =====");
                sb.AppendLine("AppointmentID : " + appointmentID);
                sb.AppendLine("Time   : " + DateTime.Now);
                sb.AppendLine("URL    : " + context.Request.Url);
                sb.AppendLine("IP     : " + context.Request.UserHostAddress);
                sb.AppendLine("Msg    : " + message);
                sb.AppendLine("LoginId       : " + _loginID);

                // 🔥 Convert ModalLog JSON → readable
                string modalLog = context.Request.Form["ModalLog"];

                if (!string.IsNullOrEmpty(modalLog))
                {
                    sb.AppendLine("---- MODAL DATA ----");

                    try
                    {
                        var dict = Newtonsoft.Json.JsonConvert
                            .DeserializeObject<Dictionary<string, object>>(modalLog);

                        foreach (var item in dict)
                        {
                            if (!string.IsNullOrEmpty(item.Value?.ToString()))
                                sb.AppendLine(item.Key + " : " + item.Value);
                        }
                    }
                    catch
                    {
                        sb.AppendLine("Invalid JSON: " + modalLog);
                    }
                }

                // 🔥 FORM DATA
                sb.AppendLine("---- FORM DATA ----");
                foreach (string key in context.Request.Form.AllKeys)
                {
                    if (key != "ModalLog")
                        sb.AppendLine($"{key} : {context.Request.Form[key]}");
                }

                if (ex != null)
                {
                    sb.AppendLine("---- ERROR ----");
                    sb.AppendLine("Error  : " + ex.Message);
                    sb.AppendLine("Stack  : " + ex.StackTrace);
                }

                sb.AppendLine("=================\n");

                File.AppendAllText(logFile, sb.ToString());
            }
            catch
            {
                // silent fail
            }
        }

        public bool IsReusable { get { return false; } }
    }
}