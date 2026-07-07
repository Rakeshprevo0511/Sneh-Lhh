using System;
using System.Collections.Generic;
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
    /// Summary description for ReportNdt_2025
    /// </summary>
    public class ReportNdt_2025 : IHttpHandler, IRequiresSessionState
    {
        DbHelper.SqlDb db; int _loginID = 0;
        public ReportNdt_2025()
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

                    case 1: retVal = SaveTab1(context, appointmentID); break;
                    case 2: retVal = SaveTab2(context, appointmentID); break;
                    case 3: retVal = SaveTab3(context, appointmentID); break;
                    case 4: retVal = SaveTab4(context, appointmentID); break;
                    case 5: retVal = SaveTab5(context, appointmentID); break;
                    case 6: retVal = SaveTab6(context, appointmentID); break;
                    case 7: retVal = SaveTab7(context, appointmentID); break;
                    case 8: retVal = SaveTab8(context, appointmentID); break;
                    case 9: retVal = SaveTab9(context, appointmentID); break;
                    case 10: retVal = SaveTab10(context, appointmentID); break;
                    case 11: retVal = SaveTab11(context, appointmentID); break;
                    case 12: retVal = SaveTab12(context, appointmentID); break;
                    case 13: retVal = SaveTab13(context, appointmentID); break;
                    case 14: retVal = SaveTab14(context, appointmentID); break;
                    case 15: retVal = SaveTab15(context, appointmentID); break;
                    case 16: retVal = SaveTab16(context, appointmentID); break;

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
        private int SaveTab1(HttpContext context, int appointmentID)
        {
            int tabNo = 1;

            string FA_GrossMotor_Ability = GetStr(context, "FA_GrossMotor_Ability");
            string FA_GrossMotor_Limit = GetStr(context, "FA_GrossMotor_Limit");

            string FA_FineMotor_Ability = GetStr(context, "FA_FineMotor_Ability");
            string FA_FineMotor_Limit = GetStr(context, "FA_FineMotor_Limit");

            string FA_Communication_Ability = GetStr(context, "FA_Communication_Ability");
            string FA_Communication_Limit = GetStr(context, "FA_Communication_Limit");

            string FA_Cognition_Ability = GetStr(context, "FA_Cognition_Ability");
            string FA_Cognition_Limit = GetStr(context, "FA_Cognition_Limit");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@FA_GrossMotor_Ability", SqlDbType.VarChar, DbNullIfEmpty(FA_GrossMotor_Ability));
                AddParam(cmd, "@FA_GrossMotor_Limit", SqlDbType.VarChar, DbNullIfEmpty(FA_GrossMotor_Limit));

                AddParam(cmd, "@FA_FineMotor_Ability", SqlDbType.VarChar, DbNullIfEmpty(FA_FineMotor_Ability));
                AddParam(cmd, "@FA_FineMotor_Limit", SqlDbType.VarChar, DbNullIfEmpty(FA_FineMotor_Limit));

                AddParam(cmd, "@FA_Communication_Ability", SqlDbType.VarChar, DbNullIfEmpty(FA_Communication_Ability));
                AddParam(cmd, "@FA_Communication_Limit", SqlDbType.VarChar, DbNullIfEmpty(FA_Communication_Limit));

                AddParam(cmd, "@FA_Cognition_Ability", SqlDbType.VarChar, DbNullIfEmpty(FA_Cognition_Ability));
                AddParam(cmd, "@FA_Cognition_Limit", SqlDbType.VarChar, DbNullIfEmpty(FA_Cognition_Limit));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab2(HttpContext context, int appointmentID)
        {
            int tabNo = 2;

            string ParticipationAbility_GrossMotor = GetStr(context, "ParticipationAbility_GrossMotor");
            string ParticipationAbility_GrossMotor_Limit = GetStr(context, "ParticipationAbility_GrossMotor_Limit");

            string ParticipationAbility_FineMotor = GetStr(context, "ParticipationAbility_FineMotor");
            string ParticipationAbility_FineMotor_Limit = GetStr(context, "ParticipationAbility_FineMotor_Limit");

            string ParticipationAbility_Communication = GetStr(context, "ParticipationAbility_Communication");
            string ParticipationAbility_Communication_Limit = GetStr(context, "ParticipationAbility_Communication_Limit");

            string ParticipationAbility_Cognition = GetStr(context, "ParticipationAbility_Cognition");
            string ParticipationAbility_Cognition_Limit = GetStr(context, "ParticipationAbility_Cognition_Limit");

            string Contextual_Personal_Positive = GetStr(context, "Contextual_Personal_Positive");
            string Contextual_Personal_Negative = GetStr(context, "Contextual_Personal_Negative");

            string Contextual_Environmental_Positive = GetStr(context, "Contextual_Environmental_Positive");
            string Contextual_Environmental_Negative = GetStr(context, "Contextual_Environmental_Negative");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@ParticipationAbility_GrossMotor", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_GrossMotor));
                AddParam(cmd, "@ParticipationAbility_GrossMotor_Limit", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_GrossMotor_Limit));

                AddParam(cmd, "@ParticipationAbility_FineMotor", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_FineMotor));
                AddParam(cmd, "@ParticipationAbility_FineMotor_Limit", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_FineMotor_Limit));

                AddParam(cmd, "@ParticipationAbility_Communication", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_Communication));
                AddParam(cmd, "@ParticipationAbility_Communication_Limit", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_Communication_Limit));

                AddParam(cmd, "@ParticipationAbility_Cognition", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_Cognition));
                AddParam(cmd, "@ParticipationAbility_Cognition_Limit", SqlDbType.VarChar, DbNullIfEmpty(ParticipationAbility_Cognition_Limit));

                AddParam(cmd, "@Contextual_Personal_Positive", SqlDbType.VarChar, DbNullIfEmpty(Contextual_Personal_Positive));
                AddParam(cmd, "@Contextual_Personal_Negative", SqlDbType.VarChar, DbNullIfEmpty(Contextual_Personal_Negative));

                AddParam(cmd, "@Contextual_Environmental_Positive", SqlDbType.VarChar, DbNullIfEmpty(Contextual_Environmental_Positive));
                AddParam(cmd, "@Contextual_Environmental_Negative", SqlDbType.VarChar, DbNullIfEmpty(Contextual_Environmental_Negative));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab3(HttpContext context, int appointmentID)
        {
            int tabNo = 3;

            string alignmentType = GetStr(context, "Multi_posture_headAlignment_AlignmentType");

            string head = GetStr(context, "Multi_posture_head");
            string shoulder = GetStr(context, "Multi_posture_Shoulder");
            string neck = GetStr(context, "Multi_posture_neck");
            string scapulae = GetStr(context, "Multi_posture_scapulae");
            string elbow = GetStr(context, "Multi_posture_elbow");
            string forearm = GetStr(context, "Multi_posture_forarm");
            string wrist = GetStr(context, "Multi_posture_wrist");
            string hand = GetStr(context, "Multi_posture_hand");
            string finger = GetStr(context, "Multi_posture_finger");
            string thumb = GetStr(context, "Multi_posture_thumb");
            string ribcage = GetStr(context, "Multi_posture_ribcage");
            string thoracicspine = GetStr(context, "Multi_posture_thoracicspine");
            string lumbarspine = GetStr(context, "Multi_posture_lumbarspine");
            string pelvis = GetStr(context, "Multi_posture_pelvis");
            string hips = GetStr(context, "Multi_posture_hips");
            string knees = GetStr(context, "Multi_posture_knees");
            string ankle = GetStr(context, "Multi_posture_ankle");
            string feet = GetStr(context, "Multi_posture_feet");
            string toes = GetStr(context, "Multi_posture_toes");
            string bos = GetStr(context, "Multi_posture_bos");
            string stabilitymethod = GetStr(context, "Multi_posture_stabiltymethod");

            string comCog = GetStr(context, "Multi_posture_com_cog");

            string mouth = GetStr(context, "Multi_posture_mouth");
            string tongue = GetStr(context, "Multi_posture_toungh");
            string teeth = GetStr(context, "Multi_posture_teeth");
            string chin = GetStr(context, "Multi_posture_chin");
            string cheeks = GetStr(context, "Multi_posture_cheeks");
            string lips = GetStr(context, "Multi_posture_lips");

            string stabilityComments = GetStr(context, "Multi_posture_Stability");
            string anticipatory = GetStr(context, "Multi_posture_anticipatory");
            string postural = GetStr(context, "Multi_posture_postural");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Multi_posture_headAlignment_AlignmentType", SqlDbType.VarChar, DbNullIfEmpty(alignmentType));
                AddParam(cmd, "@Multi_posture_head", SqlDbType.VarChar, DbNullIfEmpty(head));
                AddParam(cmd, "@Multi_posture_Shoulder", SqlDbType.VarChar, DbNullIfEmpty(shoulder));
                AddParam(cmd, "@Multi_posture_neck", SqlDbType.VarChar, DbNullIfEmpty(neck));
                AddParam(cmd, "@Multi_posture_scapulae", SqlDbType.VarChar, DbNullIfEmpty(scapulae));
                AddParam(cmd, "@Multi_posture_elbow", SqlDbType.VarChar, DbNullIfEmpty(elbow));
                AddParam(cmd, "@Multi_posture_forarm", SqlDbType.VarChar, DbNullIfEmpty(forearm));
                AddParam(cmd, "@Multi_posture_wrist", SqlDbType.VarChar, DbNullIfEmpty(wrist));
                AddParam(cmd, "@Multi_posture_hand", SqlDbType.VarChar, DbNullIfEmpty(hand));
                AddParam(cmd, "@Multi_posture_finger", SqlDbType.VarChar, DbNullIfEmpty(finger));
                AddParam(cmd, "@Multi_posture_thumb", SqlDbType.VarChar, DbNullIfEmpty(thumb));
                AddParam(cmd, "@Multi_posture_ribcage", SqlDbType.VarChar, DbNullIfEmpty(ribcage));
                AddParam(cmd, "@Multi_posture_thoracicspine", SqlDbType.VarChar, DbNullIfEmpty(thoracicspine));
                AddParam(cmd, "@Multi_posture_lumbarspine", SqlDbType.VarChar, DbNullIfEmpty(lumbarspine));
                AddParam(cmd, "@Multi_posture_pelvis", SqlDbType.VarChar, DbNullIfEmpty(pelvis));
                AddParam(cmd, "@Multi_posture_hips", SqlDbType.VarChar, DbNullIfEmpty(hips));
                AddParam(cmd, "@Multi_posture_knees", SqlDbType.VarChar, DbNullIfEmpty(knees));
                AddParam(cmd, "@Multi_posture_ankle", SqlDbType.VarChar, DbNullIfEmpty(ankle));
                AddParam(cmd, "@Multi_posture_feet", SqlDbType.VarChar, DbNullIfEmpty(feet));
                AddParam(cmd, "@Multi_posture_toes", SqlDbType.VarChar, DbNullIfEmpty(toes));
                AddParam(cmd, "@Multi_posture_bos", SqlDbType.VarChar, DbNullIfEmpty(bos));
                AddParam(cmd, "@Multi_posture_stabiltymethod", SqlDbType.VarChar, DbNullIfEmpty(stabilitymethod));

                AddParam(cmd, "@Multi_posture_com_cog", SqlDbType.VarChar, DbNullIfEmpty(comCog));

                AddParam(cmd, "@Multi_posture_mouth", SqlDbType.VarChar, DbNullIfEmpty(mouth));
                AddParam(cmd, "@Multi_posture_toungh", SqlDbType.VarChar, DbNullIfEmpty(tongue));
                AddParam(cmd, "@Multi_posture_teeth", SqlDbType.VarChar, DbNullIfEmpty(teeth));
                AddParam(cmd, "@Multi_posture_chin", SqlDbType.VarChar, DbNullIfEmpty(chin));
                AddParam(cmd, "@Multi_posture_cheeks", SqlDbType.VarChar, DbNullIfEmpty(cheeks));
                AddParam(cmd, "@Multi_posture_lips", SqlDbType.VarChar, DbNullIfEmpty(lips));

                AddParam(cmd, "@Multi_posture_Stability", SqlDbType.VarChar, DbNullIfEmpty(stabilityComments));
                AddParam(cmd, "@Multi_posture_anticipatory", SqlDbType.VarChar, DbNullIfEmpty(anticipatory));
                AddParam(cmd, "@Multi_posture_postural", SqlDbType.VarChar, DbNullIfEmpty(postural));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab4(HttpContext context, int appointmentID)
        {
            int tabNo = 4;

            string Movement_Inertia = GetStr(context, "Movement_Inertia");
            string Posture_Alignment_Type = GetStr(context, "Posture_Alignment_Type");
            string Multi_Movement_Type = GetStr(context, "Multi_Movement_Type");
            string Multi_Movement_WeightShift = GetStr(context, "Multi_Movement_WeightShift");

            string Multi_Movement_interlimb = GetStr(context, "Multi_Movement_interlimb");
            string Multi_Movement_intralimb = GetStr(context, "Multi_Movement_intralimb");

            string Multi_Movement_overuse = GetStr(context, "Multi_Movement_overuse");
            string Multi_Movement_statbilty = GetStr(context, "Multi_Movement_statbilty");

            string Multi_Movement_Bal_maintain = GetStr(context, "Multi_Movement_Bal_maintain");
            string Multi_Movement_BAl_during = GetStr(context, "Multi_Movement_BAl_during");

            string UpperLimb_Movement = GetStr(context, "UpperLimb_Movement");
            string LowerLimb_Movement = GetStr(context, "LowerLimb_Movement");
            string CervicalSpine_Movement = GetStr(context, "CervicalSpine_Movement");
            string ThoracicSpine_Movement = GetStr(context, "ThoracicSpine_Movement");

            string Gene_obsr_comments = GetStr(context, "Gene_obsr_comments");

            string txtSoinePoor = GetStr(context, "txtSoinePoor");
            string txtSoineFair = GetStr(context, "txtSoineFair");
            string txtSoineGood = GetStr(context, "txtSoineGood");

            string txtScapuloPoor = GetStr(context, "txtScapuloPoor");
            string txtScapuloFair = GetStr(context, "txtScapuloFair");
            string txtScapuloGood = GetStr(context, "txtScapuloGood");

            string txtPelviPoor = GetStr(context, "txtPelviPoor");
            string txtPelviFair = GetStr(context, "txtPelviFair");
            string txtPelviGood = GetStr(context, "txtPelviGood");

            string txtWithinUlPoor = GetStr(context, "txtWithinUlPoor");
            string txtWithinUlFair = GetStr(context, "txtWithinUlFair");
            string txtWithinUlGood = GetStr(context, "txtWithinUlGood");

            string txtWithinLlPoor = GetStr(context, "txtWithinLlPoor");
            string txtWithinLlFair = GetStr(context, "txtWithinLlFair");
            string txtWithinLlGood = GetStr(context, "txtWithinLlGood");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Movement_Inertia", SqlDbType.VarChar, DbNullIfEmpty(Movement_Inertia));
                AddParam(cmd, "@Posture_Alignment_Type", SqlDbType.VarChar, DbNullIfEmpty(Posture_Alignment_Type));
                AddParam(cmd, "@Multi_Movement_Type", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_Type));
                AddParam(cmd, "@Multi_Movement_WeightShift", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_WeightShift));

                AddParam(cmd, "@Multi_Movement_interlimb", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_interlimb));
                AddParam(cmd, "@Multi_Movement_intralimb", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_intralimb));

                AddParam(cmd, "@Multi_Movement_overuse", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_overuse));
                AddParam(cmd, "@Multi_Movement_Bal_maintain", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_Bal_maintain));
                AddParam(cmd, "@Multi_Movement_BAl_during", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_BAl_during));

                AddParam(cmd, "@UpperLimb_Movement", SqlDbType.VarChar, DbNullIfEmpty(UpperLimb_Movement));
                AddParam(cmd, "@LowerLimb_Movement", SqlDbType.VarChar, DbNullIfEmpty(LowerLimb_Movement));
                AddParam(cmd, "@CervicalSpine_Movement", SqlDbType.VarChar, DbNullIfEmpty(CervicalSpine_Movement));
                AddParam(cmd, "@ThoracicSpine_Movement", SqlDbType.VarChar, DbNullIfEmpty(ThoracicSpine_Movement));

                AddParam(cmd, "@Multi_Movement_statbilty", SqlDbType.VarChar, DbNullIfEmpty(Multi_Movement_statbilty));
                AddParam(cmd, "@Gene_obsr_comments", SqlDbType.VarChar, DbNullIfEmpty(Gene_obsr_comments));

                AddParam(cmd, "@txtSoinePoor", SqlDbType.VarChar, DbNullIfEmpty(txtSoinePoor));
                AddParam(cmd, "@txtSoineFair", SqlDbType.VarChar, DbNullIfEmpty(txtSoineFair));
                AddParam(cmd, "@txtSoineGood", SqlDbType.VarChar, DbNullIfEmpty(txtSoineGood));

                AddParam(cmd, "@txtScapuloPoor", SqlDbType.VarChar, DbNullIfEmpty(txtScapuloPoor));
                AddParam(cmd, "@txtScapuloFair", SqlDbType.VarChar, DbNullIfEmpty(txtScapuloFair));
                AddParam(cmd, "@txtScapuloGood", SqlDbType.VarChar, DbNullIfEmpty(txtScapuloGood));

                AddParam(cmd, "@txtPelviPoor", SqlDbType.VarChar, DbNullIfEmpty(txtPelviPoor));
                AddParam(cmd, "@txtPelviFair", SqlDbType.VarChar, DbNullIfEmpty(txtPelviFair));
                AddParam(cmd, "@txtPelviGood", SqlDbType.VarChar, DbNullIfEmpty(txtPelviGood));

                AddParam(cmd, "@txtWithinUlPoor", SqlDbType.VarChar, DbNullIfEmpty(txtWithinUlPoor));
                AddParam(cmd, "@txtWithinUlFair", SqlDbType.VarChar, DbNullIfEmpty(txtWithinUlFair));
                AddParam(cmd, "@txtWithinUlGood", SqlDbType.VarChar, DbNullIfEmpty(txtWithinUlGood));

                AddParam(cmd, "@txtWithinLlPoor", SqlDbType.VarChar, DbNullIfEmpty(txtWithinLlPoor));
                AddParam(cmd, "@txtWithinLlFair", SqlDbType.VarChar, DbNullIfEmpty(txtWithinLlFair));
                AddParam(cmd, "@txtWithinLlGood", SqlDbType.VarChar, DbNullIfEmpty(txtWithinLlGood));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab5(HttpContext context, int appointmentID)
        {
            int tabNo = 5;

            // Neuromotor System
            string Neuromotor_Recruitment_Initial = GetStr(context, "Neuromotor_Recruitment_Initial");
            string Neuromotor_Recruitment_Sustainance = GetStr(context, "Neuromotor_Recruitment_Sustainance");
            string Neuromotor_Recruitment_Termination = GetStr(context, "Neuromotor_Recruitment_Termination");
            string Neuromotor_Recruitment_Control = GetStr(context, "Neuromotor_Recruitment_Control");

            string Neurometer_Initialigy_initial = GetStr(context, "Neurometer_Initialigy_initial");
            string Neurometer_Initialigy_Sustainance = GetStr(context, "Neurometer_Initialigy_Sustainance");
            string Neurometer_Initialigy_Termination = GetStr(context, "Neurometer_Initialigy_Termination");
            string Neurometer_Initialigy_Control = GetStr(context, "Neurometer_Initialigy_Control");

            string Neuromotor_Contraction_Initial = GetStr(context, "Neuromotor_Contraction_Initial");
            string Neuromotor_Contraction_Sustainance = GetStr(context, "Neuromotor_Contraction_Sustainance");
            string Neuromotor_Contraction_Termination = GetStr(context, "Neuromotor_Contraction_Termination");
            string Neuromotor_Contraction_Control = GetStr(context, "Neuromotor_Contraction_Control");

            string Neuromotor_Coactivation_Initial = GetStr(context, "Neuromotor_Coactivation_Initial");
            string Neuromotor_Coactivation_Sustainance = GetStr(context, "Neuromotor_Coactivation_Sustainance");
            string Neuromotor_Coactivation_Termination = GetStr(context, "Neuromotor_Coactivation_Termination");
            string Neuromotor_Coactivation_Control = GetStr(context, "Neuromotor_Coactivation_Control");

            string Neuromotor_Synergy_Initial = GetStr(context, "Neuromotor_Synergy_Initial");
            string Neuromotor_Synergy_Sustainance = GetStr(context, "Neuromotor_Synergy_Sustainance");
            string Neuromotor_Synergy_Termination = GetStr(context, "Neuromotor_Synergy_Termination");
            string Neuromotor_Synergy_Control = GetStr(context, "Neuromotor_Synergy_Control");

            string Neuromotor_Stiffness_Initial = GetStr(context, "Neuromotor_Stiffness_Initial");
            string Neuromotor_Stiffness_Sustainance = GetStr(context, "Neuromotor_Stiffness_Sustainance");
            string Neuromotor_Stiffness_Termination = GetStr(context, "Neuromotor_Stiffness_Termination");
            string Neuromotor_Stiffness_Control = GetStr(context, "Neuromotor_Stiffness_Control");

            string Neuromotor_Extraneous_Initial = GetStr(context, "Neuromotor_Extraneous_Initial");
            string Neuromotor_Extraneous_Sustainance = GetStr(context, "Neuromotor_Extraneous_Sustainance");
            string Neuromotor_Extraneous_Termination = GetStr(context, "Neuromotor_Extraneous_Termination");
            string Neuromotor_Extraneous_Control = GetStr(context, "Neuromotor_Extraneous_Control");

            // JSON strings
            string SelectionMotorControl_Muscle = GetStr(context, "SelectionMotorControl_Muscle");
            string SelectionMotorControl_MAS = GetStr(context, "SelectionMotorControl_MAS");
            string SelectionMotorControl_Denvers = GetStr(context, "SelectionMotorControl_Denvers");

            string SelectionMotorControl_GMFM = GetStr(context, "SelectionMotorControl_GMFM");
            string SelectionMotorControl_Observation = GetStr(context, "SelectionMotorControl_Observation");

            // The Four A’s
            string TheFourA_Arousal = GetStr(context, "TheFourA_Arousal");
            string TheFourA_Attention = GetStr(context, "TheFourA_Attention");
            string TheFourA_Affect = GetStr(context, "TheFourA_Affect");
            string TheFourA_Action = GetStr(context, "TheFourA_Action");
            string TheFourA_StateRegulation = GetStr(context, "TheFourA_StateRegulation");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Neuromotor_Recruitment_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Recruitment_Initial));
                AddParam(cmd, "@Neuromotor_Recruitment_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Recruitment_Sustainance));
                AddParam(cmd, "@Neuromotor_Recruitment_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Recruitment_Termination));
                AddParam(cmd, "@Neuromotor_Recruitment_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Recruitment_Control));

                AddParam(cmd, "@Neurometer_Initialigy_initial", SqlDbType.VarChar, DbNullIfEmpty(Neurometer_Initialigy_initial));
                AddParam(cmd, "@Neurometer_Initialigy_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neurometer_Initialigy_Sustainance));
                AddParam(cmd, "@Neurometer_Initialigy_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neurometer_Initialigy_Termination));
                AddParam(cmd, "@Neurometer_Initialigy_Control", SqlDbType.VarChar, DbNullIfEmpty(Neurometer_Initialigy_Control));

                AddParam(cmd, "@Neuromotor_Contraction_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Contraction_Initial));
                AddParam(cmd, "@Neuromotor_Contraction_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Contraction_Sustainance));
                AddParam(cmd, "@Neuromotor_Contraction_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Contraction_Termination));
                AddParam(cmd, "@Neuromotor_Contraction_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Contraction_Control));

                AddParam(cmd, "@Neuromotor_Coactivation_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Coactivation_Initial));
                AddParam(cmd, "@Neuromotor_Coactivation_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Coactivation_Sustainance));
                AddParam(cmd, "@Neuromotor_Coactivation_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Coactivation_Termination));
                AddParam(cmd, "@Neuromotor_Coactivation_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Coactivation_Control));

                AddParam(cmd, "@Neuromotor_Synergy_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Synergy_Initial));
                AddParam(cmd, "@Neuromotor_Synergy_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Synergy_Sustainance));
                AddParam(cmd, "@Neuromotor_Synergy_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Synergy_Termination));
                AddParam(cmd, "@Neuromotor_Synergy_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Synergy_Control));

                AddParam(cmd, "@Neuromotor_Stiffness_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Stiffness_Initial));
                AddParam(cmd, "@Neuromotor_Stiffness_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Stiffness_Sustainance));
                AddParam(cmd, "@Neuromotor_Stiffness_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Stiffness_Termination));
                AddParam(cmd, "@Neuromotor_Stiffness_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Stiffness_Control));

                AddParam(cmd, "@Neuromotor_Extraneous_Initial", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Extraneous_Initial));
                AddParam(cmd, "@Neuromotor_Extraneous_Sustainance", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Extraneous_Sustainance));
                AddParam(cmd, "@Neuromotor_Extraneous_Termination", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Extraneous_Termination));
                AddParam(cmd, "@Neuromotor_Extraneous_Control", SqlDbType.VarChar, DbNullIfEmpty(Neuromotor_Extraneous_Control));

                AddParam(cmd, "@SelectionMotorControl_Observation", SqlDbType.VarChar, DbNullIfEmpty(SelectionMotorControl_Observation));
                AddParam(cmd, "@SelectionMotorControl_Muscle", SqlDbType.VarChar, DbNullIfEmpty(SelectionMotorControl_Muscle));
                AddParam(cmd, "@SelectionMotorControl_MAS", SqlDbType.VarChar, DbNullIfEmpty(SelectionMotorControl_MAS));
                AddParam(cmd, "@SelectionMotorControl_GMFM", SqlDbType.VarChar, DbNullIfEmpty(SelectionMotorControl_GMFM));
                AddParam(cmd, "@SelectionMotorControl_Denvers", SqlDbType.VarChar, DbNullIfEmpty(SelectionMotorControl_Denvers));

                AddParam(cmd, "@TheFourA_Arousal", SqlDbType.VarChar, DbNullIfEmpty(TheFourA_Arousal));
                AddParam(cmd, "@TheFourA_Attention", SqlDbType.VarChar, DbNullIfEmpty(TheFourA_Attention));
                AddParam(cmd, "@TheFourA_Affect", SqlDbType.VarChar, DbNullIfEmpty(TheFourA_Affect));
                AddParam(cmd, "@TheFourA_Action", SqlDbType.VarChar, DbNullIfEmpty(TheFourA_Action));
                AddParam(cmd, "@TheFourA_StateRegulation", SqlDbType.VarChar, DbNullIfEmpty(TheFourA_StateRegulation));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab6(HttpContext context, int appointmentID)
        {
            int tabNo = 6;

            // Read all values from request
            string Morphology_Height = GetStr(context, "Morphology_Height");
            string Morphology_Weight = GetStr(context, "Morphology_Weight");
            string Morphology_LimbLength = GetStr(context, "Morphology_LimbLength");

            string Morphology_LimbLeft = GetStr(context, "Morphology_LimbLeft");
            string Morphology_LimbRight = GetStr(context, "Morphology_LimbRight");

            string Morphology_ArmLength = GetStr(context, "Morphology_ArmLength");
            string Morphology_ArmLeft = GetStr(context, "Morphology_ArmLeft");
            string Morphology_ArmRight = GetStr(context, "Morphology_ArmRight");

            string Morphology_Head = GetStr(context, "Morphology_Head");
            string Morphology_Nipple = GetStr(context, "Morphology_Nipple");
            string Morphology_Waist = GetStr(context, "Morphology_Waist");

            string Morphology_GirthUpperLimb_Above_ElbowLevel1 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLevel1");
            string Morphology_GirthUpperLimb_Above_ElbowLevel2 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLevel2");
            string Morphology_GirthUpperLimb_Above_ElbowLevel3 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLevel3");

            string Morphology_GirthUpperLimb_Above_ElbowLeft1 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLeft1");
            string Morphology_GirthUpperLimb_Above_ElbowLeft2 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLeft2");
            string Morphology_GirthUpperLimb_Above_ElbowLeft3 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowLeft3");

            string Morphology_GirthUpperLimb_Above_ElbowRight1 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowRight1");
            string Morphology_GirthUpperLimb_Above_ElbowRight2 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowRight2");
            string Morphology_GirthUpperLimb_Above_ElbowRight3 = GetStr(context, "Morphology_GirthUpperLimb_Above_ElbowRight3");

            string Morphology_GirthUpperLimb_At_ElbowLevel = GetStr(context, "Morphology_GirthUpperLimb_At_ElbowLevel");
            string Morphology_GirthUpperLimb_At_ElbowLeft = GetStr(context, "Morphology_GirthUpperLimb_At_ElbowLeft");
            string Morphology_GirthUpperLimb_At_ElbowRight = GetStr(context, "Morphology_GirthUpperLimb_At_ElbowRight");

            string Morphology_GirthUpperLimb_Below_ElbowLevel1 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLevel1");
            string Morphology_GirthUpperLimb_Below_ElbowLevel2 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLevel2");
            string Morphology_GirthUpperLimb_Below_ElbowLevel3 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLevel3");

            string Morphology_GirthUpperLimb_Below_ElbowLeft1 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLeft1");
            string Morphology_GirthUpperLimb_Below_ElbowLeft2 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLeft2");
            string Morphology_GirthUpperLimb_Below_ElbowLeft3 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowLeft3");

            string Morphology_GirthUpperLimb_Below_ElbowRight1 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowRight1");
            string Morphology_GirthUpperLimb_Below_ElbowRight2 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowRight2");
            string Morphology_GirthUpperLimb_Below_ElbowRight3 = GetStr(context, "Morphology_GirthUpperLimb_Below_ElbowRight3");

            string Morphology_GirthLowerLimb_Above_KneeLevel1 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLevel1");
            string Morphology_GirthLowerLimb_Above_KneeLevel2 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLevel2");
            string Morphology_GirthLowerLimb_Above_KneeLevel3 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLevel3");

            string Morphology_GirthLowerLimb_Above_KneeLeft1 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLeft1");
            string Morphology_GirthLowerLimb_Above_KneeLeft2 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLeft2");
            string Morphology_GirthLowerLimb_Above_KneeLeft3 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeLeft3");

            string Morphology_GirthLowerLimb_Above_KneeRight1 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeRight1");
            string Morphology_GirthLowerLimb_Above_KneeRight2 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeRight2");
            string Morphology_GirthLowerLimb_Above_KneeRight3 = GetStr(context, "Morphology_GirthLowerLimb_Above_KneeRight3");

            string Morphology_GirthLowerLimb_At_KneeLevel = GetStr(context, "Morphology_GirthLowerLimb_At_KneeLevel");
            string Morphology_GirthLowerLimb_At_KneeLeft = GetStr(context, "Morphology_GirthLowerLimb_At_KneeLeft");
            string Morphology_GirthLowerLimb_At_KneeRight = GetStr(context, "Morphology_GirthLowerLimb_At_KneeRight");

            string Morphology_GirthLowerLimb_Below_KneeLevel1 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLevel1");
            string Morphology_GirthLowerLimb_Below_KneeLevel2 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLevel2");
            string Morphology_GirthLowerLimb_Below_KneeLevel3 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLevel3");

            string Morphology_GirthLowerLimb_Below_KneeLeft1 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLeft1");
            string Morphology_GirthLowerLimb_Below_KneeLeft2 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLeft2");
            string Morphology_GirthLowerLimb_Below_KneeLeft3 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeLeft3");

            string Morphology_GirthLowerLimb_Below_KneeRight1 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeRight1");
            string Morphology_GirthLowerLimb_Below_KneeRight2 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeRight2");
            string Morphology_GirthLowerLimb_Below_KneeRight3 = GetStr(context, "Morphology_GirthLowerLimb_Below_KneeRight3");

            string Morphology_OralMotorFactors = GetStr(context, "Morphology_OralMotorFactors");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Morphology_Height", SqlDbType.VarChar, DbNullIfEmpty(Morphology_Height));
                AddParam(cmd, "@Morphology_Weight", SqlDbType.VarChar, DbNullIfEmpty(Morphology_Weight));
                AddParam(cmd, "@Morphology_LimbLength", SqlDbType.VarChar, DbNullIfEmpty(Morphology_LimbLength));

                AddParam(cmd, "@Morphology_LimbLeft", SqlDbType.VarChar, DbNullIfEmpty(Morphology_LimbLeft));
                AddParam(cmd, "@Morphology_LimbRight", SqlDbType.VarChar, DbNullIfEmpty(Morphology_LimbRight));
                AddParam(cmd, "@Morphology_ArmLength", SqlDbType.VarChar, DbNullIfEmpty(Morphology_ArmLength));
                AddParam(cmd, "@Morphology_ArmLeft", SqlDbType.VarChar, DbNullIfEmpty(Morphology_ArmLeft));
                AddParam(cmd, "@Morphology_ArmRight", SqlDbType.VarChar, DbNullIfEmpty(Morphology_ArmRight));
                AddParam(cmd, "@Morphology_Head", SqlDbType.VarChar, DbNullIfEmpty(Morphology_Head));
                AddParam(cmd, "@Morphology_Nipple", SqlDbType.VarChar, DbNullIfEmpty(Morphology_Nipple));
                AddParam(cmd, "@Morphology_Waist", SqlDbType.VarChar, DbNullIfEmpty(Morphology_Waist));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLevel1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLevel2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLevel3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLevel3));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLeft1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLeft2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowLeft3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowLeft3));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowRight1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowRight2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Above_ElbowRight3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Above_ElbowRight3));

                AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowLevel", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_At_ElbowLevel));
                AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowLeft", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_At_ElbowLeft));
                AddParam(cmd, "@Morphology_GirthUpperLimb_At_ElbowRight", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_At_ElbowRight));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLevel1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLevel2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLevel3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLevel3));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLeft1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLeft2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowLeft3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowLeft3));

                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowRight1));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowRight2));
                AddParam(cmd, "@Morphology_GirthUpperLimb_Below_ElbowRight3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthUpperLimb_Below_ElbowRight3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLevel1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLevel2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLevel3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLevel3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLeft1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLeft2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeLeft3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeLeft3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeRight1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeRight2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Above_KneeRight3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Above_KneeRight3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeLevel", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_At_KneeLevel));
                AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeLeft", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_At_KneeLeft));
                AddParam(cmd, "@Morphology_GirthLowerLimb_At_KneeRight", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_At_KneeRight));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLevel1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLevel2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLevel3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLevel3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLeft1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLeft2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeLeft3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeLeft3));

                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight1", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeRight1));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight2", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeRight2));
                AddParam(cmd, "@Morphology_GirthLowerLimb_Below_KneeRight3", SqlDbType.VarChar, DbNullIfEmpty(Morphology_GirthLowerLimb_Below_KneeRight3));

                AddParam(cmd, "@Morphology_OralMotorFactors", SqlDbType.VarChar, DbNullIfEmpty(Morphology_OralMotorFactors));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab7(HttpContext context, int appointmentID)
        {
            int tabNo = 7;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                // ROM-1
                AddParam(cmd, "@Musculoskeletal_Rom1_HipFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipExtensionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExtensionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipExtensionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExtensionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipAbductionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipAbductionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipAbductionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipAbductionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipExternalLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExternalLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipExternalRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipExternalRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipInternalLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipInternalLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_HipInternalRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_HipInternalRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_PoplitealLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PoplitealLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_PoplitealRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PoplitealRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_KneeFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_KneeFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_KneeExtensionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeExtensionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_KneeExtensionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_KneeExtensionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_DorsiflexionFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_DorsiflexionFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_DorsiflexionExtensionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionExtensionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_DorsiflexionExtensionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_DorsiflexionExtensionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_PlantarFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PlantarFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_PlantarFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_PlantarFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom1_OthersLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_OthersLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom1_OthersRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom1_OthersRight")));

                // ROM-2
                AddParam(cmd, "@Musculoskeletal_Rom2_ShoulderFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ShoulderFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ShoulderExtensionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderExtensionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ShoulderExtensionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ShoulderExtensionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_HorizontalAbductionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_HorizontalAbductionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_HorizontalAbductionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_HorizontalAbductionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ExternalRotationLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ExternalRotationLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ExternalRotationRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ExternalRotationRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_InternalRotationLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_InternalRotationLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_InternalRotationRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_InternalRotationRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ElbowFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ElbowFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ElbowExtensionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowExtensionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_ElbowExtensionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_ElbowExtensionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_SupinationLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_SupinationLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_SupinationRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_SupinationRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_PronationLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_PronationLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_PronationRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_PronationRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_WristFlexionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristFlexionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_WristFlexionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristFlexionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_WristExtesionLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristExtesionLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_WristExtesionRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_WristExtesionRight")));
                AddParam(cmd, "@Musculoskeletal_Rom2_OthersLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_OthersLeft")));
                AddParam(cmd, "@Musculoskeletal_Rom2_OthersRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rom2_OthersRight")));

                // Strength
                AddParam(cmd, "@Musculoskeletal_Strengthlp", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Strengthlp")));
                AddParam(cmd, "@Musculoskeletal_StrengthCC", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthCC")));
                AddParam(cmd, "@Musculoskeletal_StrengthMuscle", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthMuscle")));
                AddParam(cmd, "@Musculoskeletal_StrengthSkeletal", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_StrengthSkeletal")));

                // MMT
                AddParam(cmd, "@Musculoskeletal_Mmt_HipflexorsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HipflexorsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_HipflexorsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HipflexorsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_AbductorsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_AbductorsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_HamsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HamsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_HamsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_HamsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_QuadsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_QuadsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_QuadsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_QuadsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TibialisAnteriorLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisAnteriorLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TibialisAnteriorRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisAnteriorRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TibialisPosteriorLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisPosteriorLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TibialisPosteriorRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TibialisPosteriorRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorDigitorumLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorDigitorumLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorDigitorumRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorDigitorumRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorHallucisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorHallucisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorHallucisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorHallucisRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PeroneiLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PeroneiLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PeroneiRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PeroneiRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorDigitorumLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorDigitorumLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorDigitorumRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorDigitorumRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorHallucisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorHallucisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorHallucisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorHallucisRight")));

                AddParam(cmd, "@Musculoskeletal_Mmt_AnteriorDeltoidLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AnteriorDeltoidLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_AnteriorDeltoidRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AnteriorDeltoidRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PosteriorDeltoidLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PosteriorDeltoidLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PosteriorDeltoidRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PosteriorDeltoidRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_MiddleDeltoidLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_MiddleDeltoidLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_MiddleDeltoidRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_MiddleDeltoidRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SupraspinatusLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupraspinatusLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SupraspinatusRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupraspinatusRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SerratusAnteriorLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SerratusAnteriorLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SerratusAnteriorRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SerratusAnteriorRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_RhomboidsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_RhomboidsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_RhomboidsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_RhomboidsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_BicepsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_BicepsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_BicepsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_BicepsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TricepsLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TricepsLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_TricepsRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_TricepsRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SupinatorLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupinatorLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_SupinatorRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_SupinatorRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PronatorLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PronatorLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_PronatorRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_PronatorRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECULeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECULeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECURight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECURight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECRLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECRLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECRRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECRRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECSLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECSLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ECSRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ECSRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCULeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCULeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCURight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCURight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCRLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCRLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCRRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCRRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCSLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCSLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FCSRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FCSRight")));

                AddParam(cmd, "@Musculoskeletal_Mmt_OpponensPollicisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_OpponensPollicisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_OpponensPollicisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_OpponensPollicisRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorPollicisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorPollicisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_FlexorPollicisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_FlexorPollicisRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_AbductorPollicisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorPollicisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_AbductorPollicisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_AbductorPollicisRight")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorPollicisLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorPollicisLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExtensorPollicisRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExtensorPollicisRight")));

                AddParam(cmd, "@Musculoskeletal_Mmt_ExternalObliquesLeft", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExternalObliquesLeft")));
                AddParam(cmd, "@Musculoskeletal_Mmt_ExternalObliquesRight", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_ExternalObliquesRight")));

                AddParam(cmd, "@Musculoskeletal_Back_Extensors_cmt", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Back_Extensors_cmt")));
                AddParam(cmd, "@Musculoskeletal_Rectus_Abdominis_cmt", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Rectus_Abdominis_cmt")));

                // Tardieus
                AddParam(cmd, "@Musculoskeletal_Mmt_Ta_Left", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_Ta_Left")));
                AddParam(cmd, "@Musculoskeletal_Mmt_Ta_Right", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_Ta_Right")));
                AddParam(cmd, "@Musculoskeletal_Mmt_Hamstring_left", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_Hamstring_left")));
                AddParam(cmd, "@Musculoskeletal_Mmt_Hamstring_Right", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_Hamstring_Right")));
                AddParam(cmd, "@Musculoskeletal_Mmt_adductors_left", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_adductors_left")));
                AddParam(cmd, "@Musculoskeletal_Mmt_adductors_right", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_adductors_right")));
                AddParam(cmd, "@Musculoskeletal_Mmt_hipFlexor_left", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_hipFlexor_left")));
                AddParam(cmd, "@Musculoskeletal_Mmt_hipFlexor_Right", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_hipFlexor_Right")));
                AddParam(cmd, "@Musculoskeletal_Mmt_biceps_left", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_biceps_left")));
                AddParam(cmd, "@Musculoskeletal_Mmt_biceps_right", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Musculoskeletal_Mmt_biceps_right")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab8(HttpContext context, int appointmentID)
        {
            int tabNo = 8;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@SelfRegulation", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SelfRegulation")));
                AddParam(cmd, "@Arousal", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Arousal")));
                AddParam(cmd, "@Attention", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Attention")));
                AddParam(cmd, "@Affect", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Affect")));
                AddParam(cmd, "@Action", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Action")));
                AddParam(cmd, "@Cognition", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cognition")));
                AddParam(cmd, "@GI", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GI")));
                AddParam(cmd, "@Respiratory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Respiratory")));
                AddParam(cmd, "@Cardiovascular", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Cardiovascular")));
                AddParam(cmd, "@SkinIntegumentary", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SkinIntegumentary")));
                AddParam(cmd, "@Nutrition", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Nutrition")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab9(HttpContext context, int appointmentID)
        {
            int tabNo = 9;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@SensorySystem_Vision", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Vision")));
                AddParam(cmd, "@SensorySystem_Auditory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Auditory")));
                AddParam(cmd, "@SensorySystem_Propioceptive", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Propioceptive")));
                AddParam(cmd, "@SensorySystem_Oromotpor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Oromotpor")));
                AddParam(cmd, "@SensorySystem_Vestibular", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Vestibular")));
                AddParam(cmd, "@SensorySystem_Tactile", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Tactile")));
                AddParam(cmd, "@SensorySystem_Olfactory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SensorySystem_Olfactory")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab10(HttpContext context, int appointmentID)
        {
            int tabNo = 10;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@score_Communication_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "score_Communication_2")));
                AddParam(cmd, "@Inter_Communication_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Inter_Communication_2")));
                AddParam(cmd, "@GROSS_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_2")));
                AddParam(cmd, "@inter_Gross_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_Gross_2")));
                AddParam(cmd, "@FINE_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_2")));
                AddParam(cmd, "@inter_FINE_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_FINE_2")));
                AddParam(cmd, "@PROBLEM_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_2")));
                AddParam(cmd, "@inter_PROBLEM_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_PROBLEM_2")));
                AddParam(cmd, "@PERSONAL_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_2")));
                AddParam(cmd, "@inter_PERSONAL_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_PERSONAL_2")));

                AddParam(cmd, "@Comm_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comm_3")));
                AddParam(cmd, "@inter_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_3")));
                AddParam(cmd, "@GROSS_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_3")));
                AddParam(cmd, "@GROSS_inter_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_3")));
                AddParam(cmd, "@FINE_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_3")));
                AddParam(cmd, "@FINE_inter_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_3")));
                AddParam(cmd, "@PROBLEM_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_3")));
                AddParam(cmd, "@PROBLEM_inter_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_3")));
                AddParam(cmd, "@PERSONAL_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_3")));
                AddParam(cmd, "@PERSONAL_inter_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_3")));

                AddParam(cmd, "@Communication_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Communication_6")));
                AddParam(cmd, "@comm_inter_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_inter_6")));
                AddParam(cmd, "@GROSS_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_6")));
                AddParam(cmd, "@GROSS_inter_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_6")));
                AddParam(cmd, "@FINE_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_6")));
                AddParam(cmd, "@FINE_inter_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_6")));
                AddParam(cmd, "@PROBLEM_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_6")));
                AddParam(cmd, "@PROBLEM_inter_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_6")));
                AddParam(cmd, "@PERSONAL_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_6")));
                AddParam(cmd, "@PERSONAL_inter_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_6")));

                AddParam(cmd, "@comm_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_7")));
                AddParam(cmd, "@inter_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_7")));
                AddParam(cmd, "@GROSS_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_7")));
                AddParam(cmd, "@GROSS_inter_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_7")));
                AddParam(cmd, "@FINE_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_7")));
                AddParam(cmd, "@FINE_inter_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_7")));
                AddParam(cmd, "@PROBLEM_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_7")));
                AddParam(cmd, "@PROBLEM_inter_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_7")));
                AddParam(cmd, "@PERSONAL_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_7")));
                AddParam(cmd, "@PERSONAL_inter_7", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_7")));

                AddParam(cmd, "@comm_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_9")));
                AddParam(cmd, "@inter_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_9")));
                AddParam(cmd, "@GROSS_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_9")));
                AddParam(cmd, "@GROSS_inter_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_9")));
                AddParam(cmd, "@FINE_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_9")));
                AddParam(cmd, "@FINE_inter_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_9")));
                AddParam(cmd, "@PROBLEM_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_9")));
                AddParam(cmd, "@PROBLEM_inter_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_9")));
                AddParam(cmd, "@PERSONAL_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_9")));
                AddParam(cmd, "@PERSONAL_inter_9", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_9")));

                AddParam(cmd, "@comm_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_10")));
                AddParam(cmd, "@inter_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_10")));
                AddParam(cmd, "@GROSS_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_10")));
                AddParam(cmd, "@GROSS_inter_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_10")));
                AddParam(cmd, "@FINE_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_10")));
                AddParam(cmd, "@FINE_inter_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_10")));
                AddParam(cmd, "@PROBLEM_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_10")));
                AddParam(cmd, "@PROBLEM_inter_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_10")));
                AddParam(cmd, "@PERSONAL_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_10")));
                AddParam(cmd, "@PERSONAL_inter_10", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_10")));

                AddParam(cmd, "@comm_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_11")));
                AddParam(cmd, "@inter_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_11")));
                AddParam(cmd, "@GROSS_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_11")));
                AddParam(cmd, "@GROSS_inter_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_11")));
                AddParam(cmd, "@FINE_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_11")));
                AddParam(cmd, "@FINE_inter_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_11")));
                AddParam(cmd, "@PROBLEM_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_11")));
                AddParam(cmd, "@PROBLEM_inter_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_11")));
                AddParam(cmd, "@PERSONAL_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_11")));
                AddParam(cmd, "@PERSONAL_inter_11", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_11")));

                AddParam(cmd, "@comm_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_13")));
                AddParam(cmd, "@inter_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_13")));
                AddParam(cmd, "@GROSS_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_13")));
                AddParam(cmd, "@GROSS_inter_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_13")));
                AddParam(cmd, "@FINE_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_13")));
                AddParam(cmd, "@FINE_inter_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_13")));
                AddParam(cmd, "@PROBLEM_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_13")));
                AddParam(cmd, "@PROBLEM_inter_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_13")));
                AddParam(cmd, "@PERSONAL_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_13")));
                AddParam(cmd, "@PERSONAL_inter_13", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_13")));

                AddParam(cmd, "@comm_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_15")));
                AddParam(cmd, "@inter_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_15")));
                AddParam(cmd, "@GROSS_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_15")));
                AddParam(cmd, "@GROSS_inter_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_15")));
                AddParam(cmd, "@FINE_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_15")));
                AddParam(cmd, "@FINE_inter_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_15")));
                AddParam(cmd, "@PROBLEM_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_15")));
                AddParam(cmd, "@PROBLEM_inter_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_15")));
                AddParam(cmd, "@PERSONAL_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_15")));
                AddParam(cmd, "@PERSONAL_inter_15", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_15")));

                AddParam(cmd, "@comm_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_17")));
                AddParam(cmd, "@inter_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_17")));
                AddParam(cmd, "@GROSS_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_17")));
                AddParam(cmd, "@GROSS_inter_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_17")));
                AddParam(cmd, "@FINE_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_17")));
                AddParam(cmd, "@FINE_inter_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_17")));
                AddParam(cmd, "@PROBLEM_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_17")));
                AddParam(cmd, "@PROBLEM_inter_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_17")));
                AddParam(cmd, "@PERSONAL_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_17")));
                AddParam(cmd, "@PERSONAL_inter_17", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_17")));

                AddParam(cmd, "@comm_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_19")));
                AddParam(cmd, "@inter_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_19")));
                AddParam(cmd, "@GROSS_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_19")));
                AddParam(cmd, "@GROSS_inter_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_19")));
                AddParam(cmd, "@FINE_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_19")));
                AddParam(cmd, "@FINE_inter_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_19")));
                AddParam(cmd, "@PROBLEM_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_19")));
                AddParam(cmd, "@PROBLEM_inter_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_19")));
                AddParam(cmd, "@PERSONAL_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_19")));
                AddParam(cmd, "@PERSONAL_inter_19", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_19")));

                AddParam(cmd, "@comm_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_21")));
                AddParam(cmd, "@inter_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_21")));
                AddParam(cmd, "@GROSS_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_21")));
                AddParam(cmd, "@GROSS_inter_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_21")));
                AddParam(cmd, "@FINE_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_21")));
                AddParam(cmd, "@FINE_inter_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_21")));
                AddParam(cmd, "@PROBLEM_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_21")));
                AddParam(cmd, "@PROBLEM_inter_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_21")));
                AddParam(cmd, "@PERSONAL_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_21")));
                AddParam(cmd, "@PERSONAL_inter_21", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_21")));

                AddParam(cmd, "@comm_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_23")));
                AddParam(cmd, "@inter_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_23")));
                AddParam(cmd, "@GROSS_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_23")));
                AddParam(cmd, "@GROSS_inter_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_23")));
                AddParam(cmd, "@FINE_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_23")));
                AddParam(cmd, "@FINE_inter_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_23")));
                AddParam(cmd, "@PROBLEM_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_23")));
                AddParam(cmd, "@PROBLEM_inter_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_23")));
                AddParam(cmd, "@PERSONAL_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_23")));
                AddParam(cmd, "@PERSONAL_inter_23", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_23")));

                AddParam(cmd, "@comm_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_25")));
                AddParam(cmd, "@inter_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_25")));
                AddParam(cmd, "@GROSS_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_25")));
                AddParam(cmd, "@GROSS_inter_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_25")));
                AddParam(cmd, "@FINE_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_25")));
                AddParam(cmd, "@FINE_inter_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_25")));
                AddParam(cmd, "@PROBLEM_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_25")));
                AddParam(cmd, "@PROBLEM_inter_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_25")));
                AddParam(cmd, "@PERSONAL_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_25")));
                AddParam(cmd, "@PERSONAL_inter_25", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_25")));

                AddParam(cmd, "@comm_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_28")));
                AddParam(cmd, "@inter_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_28")));
                AddParam(cmd, "@GROSS_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_28")));
                AddParam(cmd, "@GROSS_inter_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_28")));
                AddParam(cmd, "@FINE_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_28")));
                AddParam(cmd, "@FINE_inter_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_28")));
                AddParam(cmd, "@PROBLEM_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_28")));
                AddParam(cmd, "@PROBLEM_inter_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_28")));
                AddParam(cmd, "@PERSONAL_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_28")));
                AddParam(cmd, "@PERSONAL_inter_28", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_28")));

                AddParam(cmd, "@comm_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_31")));
                AddParam(cmd, "@inter_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_31")));
                AddParam(cmd, "@GROSS_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_31")));
                AddParam(cmd, "@GROSS_inter_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_31")));
                AddParam(cmd, "@FINE_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_31")));
                AddParam(cmd, "@FINE_inter_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_31")));
                AddParam(cmd, "@PROBLEM_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_31")));
                AddParam(cmd, "@PROBLEM_inter_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_31")));
                AddParam(cmd, "@PERSONAL_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_31")));
                AddParam(cmd, "@PERSONAL_inter_31", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_31")));

                AddParam(cmd, "@comm_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_34")));
                AddParam(cmd, "@inter_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_34")));
                AddParam(cmd, "@GROSS_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_34")));
                AddParam(cmd, "@GROSS_inter_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_34")));
                AddParam(cmd, "@FINE_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_34")));
                AddParam(cmd, "@FINE_inter_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_34")));
                AddParam(cmd, "@PROBLEM_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_34")));
                AddParam(cmd, "@PROBLEM_inter_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_34")));
                AddParam(cmd, "@PERSONAL_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_34")));
                AddParam(cmd, "@PERSONAL_inter_34", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_34")));

                AddParam(cmd, "@comm_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_42")));
                AddParam(cmd, "@inter_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_42")));
                AddParam(cmd, "@GROSS_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_42")));
                AddParam(cmd, "@GROSS_inter_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_42")));
                AddParam(cmd, "@FINE_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_42")));
                AddParam(cmd, "@FINE_inter_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_42")));
                AddParam(cmd, "@PROBLEM_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_42")));
                AddParam(cmd, "@PROBLEM_inter_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_42")));
                AddParam(cmd, "@PERSONAL_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_42")));
                AddParam(cmd, "@PERSONAL_inter_42", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_42")));

                AddParam(cmd, "@comm_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_45")));
                AddParam(cmd, "@inter_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_45")));
                AddParam(cmd, "@GROSS_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_45")));
                AddParam(cmd, "@GROSS_inter_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_45")));
                AddParam(cmd, "@FINE_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_45")));
                AddParam(cmd, "@FINE_inter_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_45")));
                AddParam(cmd, "@PROBLEM_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_45")));
                AddParam(cmd, "@PROBLEM_inter_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_45")));
                AddParam(cmd, "@PERSONAL_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_45")));
                AddParam(cmd, "@PERSONAL_inter_45", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_45")));

                AddParam(cmd, "@comm_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_51")));
                AddParam(cmd, "@inter_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_51")));
                AddParam(cmd, "@GROSS_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_51")));
                AddParam(cmd, "@GROSS_inter_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_51")));
                AddParam(cmd, "@FINE_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_51")));
                AddParam(cmd, "@FINE_inter_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_51")));
                AddParam(cmd, "@PROBLEM_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_51")));
                AddParam(cmd, "@PROBLEM_inter_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_51")));
                AddParam(cmd, "@PERSONAL_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_51")));
                AddParam(cmd, "@PERSONAL_inter_51", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_51")));

                AddParam(cmd, "@comm_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "comm_60")));
                AddParam(cmd, "@inter_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "inter_60")));
                AddParam(cmd, "@GROSS_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_60")));
                AddParam(cmd, "@GROSS_inter_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_60")));
                AddParam(cmd, "@FINE_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_60")));
                AddParam(cmd, "@FINE_inter_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_60")));
                AddParam(cmd, "@PROBLEM_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_60")));
                AddParam(cmd, "@PROBLEM_inter_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_60")));
                AddParam(cmd, "@PERSONAL_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_60")));
                AddParam(cmd, "@PERSONAL_inter_60", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_60")));

                AddParam(cmd, "@MONTHS", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "MONTHS")));
                AddParam(cmd, "@questions", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "questions")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab11(HttpContext context, int appointmentID)
        {
            int tabNo = 11;
            string abilityQuestions = GetStr(context, "ABILITY_questions");
            string abilityMonths = GetStr(context, "ABILITY_months");
            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@ABILITY_months", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ABILITY_months")));
                AddParam(cmd, "@ability_TOTAL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ability_TOTAL")));
                AddParam(cmd, "@ability_COMMENTS", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ability_COMMENTS")));
                AddParam(cmd, "@ABILITY_questions", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ABILITY_questions")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab12(HttpContext context, int appointmentID)
        {
            int tabNo = 12;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@GMFCS_I", SqlDbType.Bit, GetBool(context, "GMFCS_I"));
                AddParam(cmd, "@GMFCS_II", SqlDbType.Bit, GetBool(context, "GMFCS_II"));
                AddParam(cmd, "@GMFCS_III", SqlDbType.Bit, GetBool(context, "GMFCS_III"));
                AddParam(cmd, "@GMFCS_IV", SqlDbType.Bit, GetBool(context, "GMFCS_IV"));
                AddParam(cmd, "@GMFCS_V", SqlDbType.Bit, GetBool(context, "GMFCS_V"));

                AddParam(cmd, "@Gmfm_LyingRolling", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gmfm_LyingRolling")));
                AddParam(cmd, "@Gmfm_Sitting", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gmfm_Sitting")));
                AddParam(cmd, "@Gmfm_KneelingCrawling", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gmfm_KneelingCrawling")));
                AddParam(cmd, "@Gmfm_Standing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gmfm_Standing")));
                AddParam(cmd, "@Gmfm_RunningJumping", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Gmfm_RunningJumping")));
                AddParam(cmd, "@txtGmfm_TotalScore", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "txtGmfm_TotalScore")));

                AddParam(cmd, "@MACs_I", SqlDbType.Bit, GetBool(context, "MACs_I"));
                AddParam(cmd, "@MACs_II", SqlDbType.Bit, GetBool(context, "MACs_II"));
                AddParam(cmd, "@MACs_III", SqlDbType.Bit, GetBool(context, "MACs_III"));
                AddParam(cmd, "@MACs_IV", SqlDbType.Bit, GetBool(context, "MACs_IV"));
                AddParam(cmd, "@MACs_V", SqlDbType.Bit, GetBool(context, "MACs_V"));

                AddParam(cmd, "@FMS_I", SqlDbType.Bit, GetBool(context, "FMS_I"));
                AddParam(cmd, "@FMS_II", SqlDbType.Bit, GetBool(context, "FMS_II"));
                AddParam(cmd, "@FMS_III", SqlDbType.Bit, GetBool(context, "FMS_III"));
                AddParam(cmd, "@FMS_IV", SqlDbType.Bit, GetBool(context, "FMS_IV"));
                AddParam(cmd, "@FMS_V", SqlDbType.Bit, GetBool(context, "FMS_V"));

                AddParam(cmd, "@Barry_I", SqlDbType.Bit, GetBool(context, "Barry_I"));
                AddParam(cmd, "@Barry_II", SqlDbType.Bit, GetBool(context, "Barry_II"));
                AddParam(cmd, "@Barry_III", SqlDbType.Bit, GetBool(context, "Barry_III"));
                AddParam(cmd, "@Barry_IV", SqlDbType.Bit, GetBool(context, "Barry_IV"));
                AddParam(cmd, "@Barry_V", SqlDbType.Bit, GetBool(context, "Barry_V"));
                AddParam(cmd, "@Barry_VI", SqlDbType.Bit, GetBool(context, "Barry_VI"));

                AddParam(cmd, "@Barry_albright_txt", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Barry_albright_txt")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab13(HttpContext context, int appointmentID)
        {
            int tabNo = 13;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@TestMeassures_GrossMotor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TestMeassures_GrossMotor")));
                AddParam(cmd, "@TestMeassures_FineMotor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TestMeassures_FineMotor")));
                AddParam(cmd, "@TestMeassures_DenverLanguage", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TestMeassures_DenverLanguage")));
                AddParam(cmd, "@TestMeassures_DenverPersonal", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TestMeassures_DenverPersonal")));
                AddParam(cmd, "@Tests_cmt", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Tests_cmt")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab14(HttpContext context, int appointmentID)
        {
            int tabNo = 14;

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@General_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "General_Processing")));
                AddParam(cmd, "@AUDITORY_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "AUDITORY_Processing")));
                AddParam(cmd, "@VISUAL_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "VISUAL_Processing")));
                AddParam(cmd, "@TOUCH_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TOUCH_Processing")));
                AddParam(cmd, "@MOVEMENT_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "MOVEMENT_Processing")));
                AddParam(cmd, "@ORAL_Processing", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ORAL_Processing")));
                AddParam(cmd, "@Raw_score", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Raw_score")));
                AddParam(cmd, "@Total_rawscore", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Total_rawscore")));
                AddParam(cmd, "@Interpretation", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Interpretation")));
                AddParam(cmd, "@Comments_1", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_1")));

                AddParam(cmd, "@Score_seeking", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_seeking")));
                AddParam(cmd, "@SEEKING", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SEEKING")));

                AddParam(cmd, "@Score_Avoiding", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_Avoiding")));
                AddParam(cmd, "@AVOIDING", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "AVOIDING")));

                AddParam(cmd, "@Score_sensitivity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_sensitivity")));
                AddParam(cmd, "@SENSITIVITY_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SENSITIVITY_2")));

                AddParam(cmd, "@Score_Registration", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_Registration")));
                AddParam(cmd, "@REGISTRATION", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "REGISTRATION")));

                AddParam(cmd, "@Score_general", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_general")));
                AddParam(cmd, "@GENERAL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "GENERAL")));

                AddParam(cmd, "@Score_Auditory", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_Auditory")));
                AddParam(cmd, "@AUDITORY", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "AUDITORY")));

                AddParam(cmd, "@Score_visual", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_visual")));
                AddParam(cmd, "@VISUAL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "VISUAL")));

                AddParam(cmd, "@Score_touch", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_touch")));
                AddParam(cmd, "@TOUCH", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "TOUCH")));

                AddParam(cmd, "@Score_movement", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_movement")));
                AddParam(cmd, "@MOVEMENT", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "MOVEMENT")));

                AddParam(cmd, "@Score_oral", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_oral")));
                AddParam(cmd, "@ORAL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "ORAL")));

                AddParam(cmd, "@Score_behavioural", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Score_behavioural")));
                AddParam(cmd, "@BEHAVIORAL", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "BEHAVIORAL")));

                AddParam(cmd, "@Comments_2", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_2")));

                AddParam(cmd, "@SPchild_Seeker", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Seeker")));
                AddParam(cmd, "@Seeking_Seeker", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Seeking_Seeker")));

                AddParam(cmd, "@SPchild_Avoider", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Avoider")));
                AddParam(cmd, "@Avoiding_Avoider", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Avoiding_Avoider")));

                AddParam(cmd, "@SPchild_Sensor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Sensor")));
                AddParam(cmd, "@Sensitivity_Sensor", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensitivity_Sensor")));

                AddParam(cmd, "@SPchild_Bystander", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Bystander")));
                AddParam(cmd, "@Registration_Bystander", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Registration_Bystander")));

                AddParam(cmd, "@SPchild_Auditory_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Auditory_3")));
                AddParam(cmd, "@Auditory_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Auditory_3")));

                AddParam(cmd, "@SPchild_Visual_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Visual_3")));
                AddParam(cmd, "@Visual_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Visual_3")));

                AddParam(cmd, "@SPchild_Touch_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Touch_3")));
                AddParam(cmd, "@Touch_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Touch_3")));

                AddParam(cmd, "@SPchild_Movement_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Movement_3")));
                AddParam(cmd, "@Movement_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Movement_3")));

                AddParam(cmd, "@SPchild_Body_position", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Body_position")));
                AddParam(cmd, "@Body_position", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Body_position")));

                AddParam(cmd, "@SPchild_Oral_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Oral_3")));
                AddParam(cmd, "@Oral_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Oral_3")));

                AddParam(cmd, "@SPchild_Conduct_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Conduct_3")));
                AddParam(cmd, "@Conduct_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Conduct_3")));

                AddParam(cmd, "@SPchild_Social_emotional", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Social_emotional")));
                AddParam(cmd, "@Social_emotional", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Social_emotional")));

                AddParam(cmd, "@SPchild_Attentional_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPchild_Attentional_3")));
                AddParam(cmd, "@Attentional_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Attentional_3")));

                AddParam(cmd, "@Comments_3", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_3")));

                AddParam(cmd, "@SPAdult_Low_Registration", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPAdult_Low_Registration")));
                AddParam(cmd, "@Low_Registration", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Low_Registration")));

                AddParam(cmd, "@SPAdult_Sensory_seeking", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPAdult_Sensory_seeking")));
                AddParam(cmd, "@Sensory_seeking", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_seeking")));

                AddParam(cmd, "@SPAdult_Sensory_Sensitivity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPAdult_Sensory_Sensitivity")));
                AddParam(cmd, "@Sensory_Sensitivity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Sensitivity")));

                AddParam(cmd, "@SPAdult_Sensory_Avoiding", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SPAdult_Sensory_Avoiding")));
                AddParam(cmd, "@Sensory_Avoiding", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Avoiding")));

                AddParam(cmd, "@Comments_4", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_4")));

                AddParam(cmd, "@SP_Low_Registration64", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SP_Low_Registration64")));
                AddParam(cmd, "@Low_Registration_5", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Low_Registration_5")));

                AddParam(cmd, "@SP_Sensory_seeking_64", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SP_Sensory_seeking_64")));
                AddParam(cmd, "@Sensory_seeking_5", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_seeking_5")));

                AddParam(cmd, "@SP_Sensory_Sensitivity64", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SP_Sensory_Sensitivity64")));
                AddParam(cmd, "@Sensory_Sensitivity_5", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Sensitivity_5")));

                AddParam(cmd, "@SP_Sensory_Avoiding64", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "SP_Sensory_Avoiding64")));
                AddParam(cmd, "@Sensory_Avoiding_5", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Avoiding_5")));

                AddParam(cmd, "@Comments_5", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_5")));

                AddParam(cmd, "@Older_Low_Registration", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Older_Low_Registration")));
                AddParam(cmd, "@Low_Registration_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Low_Registration_6")));

                AddParam(cmd, "@Older_Sensory_seeking", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Older_Sensory_seeking")));
                AddParam(cmd, "@Sensory_seeking_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_seeking_6")));

                AddParam(cmd, "@Older_Sensory_Sensitivity", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Older_Sensory_Sensitivity")));
                AddParam(cmd, "@Sensory_Sensitivity_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Sensitivity_6")));

                AddParam(cmd, "@Older_Sensory_Avoiding", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Older_Sensory_Avoiding")));
                AddParam(cmd, "@Sensory_Avoiding_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Sensory_Avoiding_6")));

                AddParam(cmd, "@Comments_6", SqlDbType.VarChar, DbNullIfEmpty(GetStr(context, "Comments_6")));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab15(HttpContext context, int appointmentID)
        {
            int tabNo = 15;

            string Evaluation_Goal_Summary = GetStr(context, "Evaluation_Goal_Summary");
            string Evaluation_System_Impairment = GetStr(context, "Evaluation_System_Impairment");
            string Evaluation_LTG = GetStr(context, "Evaluation_LTG");
            string Evaluation_STG = GetStr(context, "Evaluation_STG");

            string Evalution_Plan_advice = GetStr(context, "Evalution_Plan_advice");
            string Evalution_Plan__Frequency = GetStr(context, "Evalution_Plan__Frequency");
            string Evalution_Plan_Adjuncts = GetStr(context, "Evalution_Plan_Adjuncts");
            string Evalution_Plan__Education = GetStr(context, "Evalution_Plan__Education");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Evaluation_Goal_Summary", SqlDbType.VarChar, DbNullIfEmpty(Evaluation_Goal_Summary));
                AddParam(cmd, "@Evaluation_System_Impairment", SqlDbType.VarChar, DbNullIfEmpty(Evaluation_System_Impairment));
                AddParam(cmd, "@Evaluation_LTG", SqlDbType.VarChar, DbNullIfEmpty(Evaluation_LTG));
                AddParam(cmd, "@Evaluation_STG", SqlDbType.VarChar, DbNullIfEmpty(Evaluation_STG));

                AddParam(cmd, "@Evalution_Plan_advice", SqlDbType.VarChar, DbNullIfEmpty(Evalution_Plan_advice));
                AddParam(cmd, "@Evalution_Plan__Frequency", SqlDbType.VarChar, DbNullIfEmpty(Evalution_Plan__Frequency));
                AddParam(cmd, "@Evalution_Plan_Adjuncts", SqlDbType.VarChar, DbNullIfEmpty(Evalution_Plan_Adjuncts));
                AddParam(cmd, "@Evalution_Plan__Education", SqlDbType.VarChar, DbNullIfEmpty(Evalution_Plan__Education));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ret);

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab16(HttpContext context, int appointmentID)
        {
            int tabNo = 16;

            string Doctor_Physioptherapist = GetStr(context, "Doctor_Physioptherapist");
            string Doctor_Occupational = GetStr(context, "Doctor_Occupational");

            using (SqlCommand cmd = new SqlCommand("ReportNdtMst_NEW_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Doctor_Physioptherapist", SqlDbType.VarChar, DbNullIfEmpty(Doctor_Physioptherapist));
                AddParam(cmd, "@Doctor_Occupational", SqlDbType.VarChar, DbNullIfEmpty(Doctor_Occupational));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                SqlParameter ret = new SqlParameter("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ret);

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }

        private string ResolveClientUrl(string sessionOutURL)
        {
            throw new NotImplementedException();
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
        private bool GetBool(HttpContext context, string key)
        {
            if (context == null) return false;

            string val = context.Request.Form[key] ?? context.Request[key];

            if (string.IsNullOrWhiteSpace(val)) return false;

            val = val.Trim().ToLower();

            if (val == "1" || val == "true"  || val == "on"  || val == "yes") return true;
            if (val == "0" || val == "false" || val == "off" || val == "no") return false;

            bool b;
            if (bool.TryParse(val, out b)) return b;

            int n;
            if (int.TryParse(val, out n)) return n != 0;

            return false;
        }


        private string GetString(object obj)
        {
            return obj != DBNull.Value ? obj.ToString() : string.Empty;
        }
        private void LogRequest(HttpContext context, int tabNo, int appointmentID)
        {
            try
            {
                // Directly under Ndt_2025
                string logDir = context.Server.MapPath(
                    "~/" + Path.Combine("Logs", "Ndt_2025")
                );

                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // File name = AppointmentID
                string logFile = Path.Combine(logDir, $"{appointmentID}.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("===== NDT_2025 SAVE LOG =====");
                sb.AppendLine("Time          : " + DateTime.Now);
                sb.AppendLine("TabNo         : " + tabNo);
                sb.AppendLine("AppointmentID : " + appointmentID);
                sb.AppendLine("LoginId       : " + _loginID);
                sb.AppendLine("FORM DATA:");

                foreach (string key in context.Request.Form.AllKeys)
                {
                    sb.AppendLine($"{key} = {context.Request.Form[key]}");
                }

                sb.AppendLine("==========================");

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
                string logDir = context.Server.MapPath("~/Logs/Modal/Ndt_2025");

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