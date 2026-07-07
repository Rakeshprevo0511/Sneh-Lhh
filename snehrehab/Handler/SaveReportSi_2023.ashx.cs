using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace snehrehab.Handler
{
    /// <summary>
    /// Summary description for SaveReportSi_2023
    /// </summary>
    public class SaveReportSi_2023 : IHttpHandler, IRequiresSessionState
    {
        DbHelper.SqlDb db; int _loginID = 0;
        public SaveReportSi_2023()
        {
            db = new DbHelper.SqlDb();
        }
        public class SITimelineModel
        {
            public int SI_ID { get; set; }
            public string TIME { get; set; }
            public string ACTIVITIES { get; set; }
            public string COMMENTS { get; set; }
        }
        SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
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
                string record = context.Request.Form["Record"];
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
                    context.Response.Write("ERROR|Invalid AppointmentID or TabNo");
                    return;
                }
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
                    case 14:
                        retVal = SaveTab14(context, appointmentID);
                        break;
                    case 15:
                        retVal = SaveTab15(context, appointmentID);
                        break;
                    case 16:
                        retVal = SaveTab16(context, appointmentID);
                        break;
                    case 17:
                        retVal = SaveTab17(context, appointmentID);
                        break;
                    case 18:
                        retVal = SaveTab18(context, appointmentID);
                        break;
                    case 19:
                        retVal = SaveTab19(context, appointmentID);
                        break;

                    case 20:
                        retVal = SaveTab20(context, appointmentID);
                        break;

                    case 21:
                        retVal = SaveTab21(context, appointmentID);
                        break;



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
            string clinicalObservation = GetStr(context, "ClinicalObsevation");
            string timelineJson = GetStr(context, "TimelineJson");

            List<SITimelineModel> list = new List<SITimelineModel>();

            try
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SITimelineModel>>(timelineJson);
            }
            catch
            {
                list = new List<SITimelineModel>();
            }

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@ClinicalObsevation", SqlDbType.NVarChar, DbNullIfEmpty(clinicalObservation));
                AddParam(cmd, "@ModifyDate", SqlDbType.DateTime, DateTime.UtcNow.AddMinutes(330));
                AddParam(cmd, "@ModifyBy", SqlDbType.Int, _loginID);

                AddParam(cmd, "@RetVal", SqlDbType.Int, 1);
                AddParam(cmd, "@TabNo", SqlDbType.Int, 1);

                db.DbUpdate(cmd);
            }

            foreach (var row in list)
            {
                string time = (row.TIME ?? "").Trim();
                string act = (row.ACTIVITIES ?? "").Trim();
                string com = (row.COMMENTS ?? "").Trim();

                // if any filled -> save
                if (!string.IsNullOrWhiteSpace(time) || !string.IsNullOrWhiteSpace(act) || !string.IsNullOrWhiteSpace(com))
                {
                    SID.SetTimeLine( appointmentID, row.SI_ID, time, act, com, DateTime.UtcNow.AddMinutes(330), _loginID );
                }
                else
                {
                    if (row.SI_ID > 0)
                        SID.DeleteRow(row.SI_ID);
                }
            }

            return 1;
        }
        private int SaveTab2(HttpContext context, int appointmentID)
        {
            int tabNo = 2;

            string FamilyStructure_QualityTimeMother = GetStr(context, "FamilyStructure_QualityTimeMother");
            string FamilyStructure_QualityTimeFather = GetStr(context, "FamilyStructure_QualityTimeFather");
            string FamilyStructure_QualityTimeWeekend = GetStr(context, "FamilyStructure_QualityTimeWeekend");
            string Father_Weekends = GetStr(context, "Father_Weekends");
            string Mother_Weekends = GetStr(context, "Mother_Weekends");
            string FamilyStructure_TimeForThreapy = GetStr(context, "FamilyStructure_TimeForThreapy");
            string FamilyStructure_AcceptanceCondition = GetStr(context, "FamilyStructure_AcceptanceCondition");
            string FamilyStructure_ExtraCaricular = GetStr(context, "FamilyStructure_ExtraCaricular");

            string FamilyStructure_Diciplinary = GetStr(context, "FamilyStructure_Diciplinary");
            string FamilyStructure_SiblingBrother = GetStr(context, "FamilyStructure_SiblingBrother");
            string FamilyStructure_Expectations = GetStr(context, "FamilyStructure_Expectations");
            string FamilyStructure_CloselyInvolved = GetStr(context, "FamilyStructure_CloselyInvolved");
            string FAMILY_cmt = GetStr(context, "FAMILY_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo); // ✅ FIX (2)

                AddParam(cmd, "@FamilyStructure_QualityTimeMother", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_QualityTimeMother));
                AddParam(cmd, "@FamilyStructure_QualityTimeFather", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_QualityTimeFather));
                AddParam(cmd, "@FamilyStructure_QualityTimeWeekend", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_QualityTimeWeekend));
                AddParam(cmd, "@Father_Weekends", SqlDbType.NVarChar, DbNullIfEmpty(Father_Weekends));
                //AddParam(cmd, "@Mother_Weekends", SqlDbType.NVarChar, DbNullIfEmpty(Mother_Weekends));
                AddParam(cmd, "@FamilyStructure_TimeForThreapy", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_TimeForThreapy));
                AddParam(cmd, "@FamilyStructure_AcceptanceCondition", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_AcceptanceCondition));
                AddParam(cmd, "@FamilyStructure_ExtraCaricular", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_ExtraCaricular));

                AddParam(cmd, "@FamilyStructure_Diciplinary", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_Diciplinary));
                AddParam(cmd, "@FamilyStructure_SiblingBrother", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_SiblingBrother));
                AddParam(cmd, "@FamilyStructure_Expectations", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_Expectations));
                AddParam(cmd, "@FamilyStructure_CloselyInvolved", SqlDbType.NVarChar, DbNullIfEmpty(FamilyStructure_CloselyInvolved));
                AddParam(cmd, "@FAMILY_cmt", SqlDbType.NVarChar, DbNullIfEmpty(FAMILY_cmt));

                // ✅ OUTPUT PARAM (FIX)
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab3(HttpContext context, int appointmentID)
        {
            int tabNo = 3;

            string Schoolinfo_Attend = GetStr(context, "Schoolinfo_Attend");
            string Schoolinfo_Type = GetStr(context, "Schoolinfo_Type");
            string Schoolinfo_SchoolHours = GetStr(context, "Schoolinfo_SchoolHours");

            string School_Travel_Mode = GetStr(context, "School_Travel_Mode");
            var modes = (School_Travel_Mode ?? "")
                     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string School_Bus = modes.Contains("School_bus") ? "School_bus" : null;
            string Car = modes.Contains("Car") ? "Car" : null;
            string Two_Wheelers = modes.Contains("Two_wheelers") ? "Two_wheelers" : null;
            string walking = modes.Contains("Walking") ? "Walking" : null;
            string Public_Transport = modes.Contains("Public_transport") ? "Public_transport" : null;


            string Schoolinfo_NoOfTeacher = GetStr(context, "Schoolinfo_NoOfTeacher");
            string Seating_Type = GetStr(context, "Seating_Type");
            var seating = (Seating_Type ?? "")
                   .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string Floor = seating.Contains("Floor") ? "Floor" : null;
            string Single_bench = seating.Contains("Single_bench") ? "Single_bench" : null;
            string Bench2 = seating.Contains("Bench2") ? "Bench2" : null;
            string Round_table = seating.Contains("Round_table") ? "Round_table" : null;

            string Schoolinfo_Mealtime = GetStr(context, "Schoolinfo_Mealtime");
            string Schoolinfo_MealType = GetStr(context, "Schoolinfo_MealType");

            string Schoolinfo_Shareing = GetStr(context, "Schoolinfo_Shareing");
            string Schoolinfo_HelpEating = GetStr(context, "Schoolinfo_HelpEating");
            string Schoolinfo_Friendship = GetStr(context, "Schoolinfo_Friendship");
            string Schoolinfo_InteractionPeer = GetStr(context, "Schoolinfo_InteractionPeer");
            string Schoolinfo_InteractionTeacher = GetStr(context, "Schoolinfo_InteractionTeacher");

            string Schoolinfo_AnnualFunction = GetStr(context, "Schoolinfo_AnnualFunction");
            string Schoolinfo_Sports = GetStr(context, "Schoolinfo_Sports");
            string Schoolinfo_Picnic = GetStr(context, "Schoolinfo_Picnic");
            string Schoolinfo_ExtraCaricular = GetStr(context, "Schoolinfo_ExtraCaricular");

            string Schoolinfo_CopyBoard = GetStr(context, "Schoolinfo_CopyBoard");
            string Schoolinfo_Instructions = GetStr(context, "Schoolinfo_Instructions");
            string Schoolinfo_ShadowTeacher = GetStr(context, "Schoolinfo_ShadowTeacher");
            string Schoolinfo_CW_HW = GetStr(context, "Schoolinfo_CW_HW");

            string Schoolinfo_SpecialEducator = GetStr(context, "Schoolinfo_SpecialEducator");
            string Schoolinfo_DeliveryInformation = GetStr(context, "Schoolinfo_DeliveryInformation");

            string Schoolinfo_RemarkTeacher = GetStr(context, "Schoolinfo_RemarkTeacher");
            string SCHOOL_cmt = GetStr(context, "SCHOOL_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Schoolinfo_Attend", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Attend));
                AddParam(cmd, "@Schoolinfo_Type", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Type));
                AddParam(cmd, "@Schoolinfo_SchoolHours", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_SchoolHours));


                AddParam(cmd, "@School_Bus", SqlDbType.NVarChar, DbNullIfEmpty(School_Bus));
                AddParam(cmd, "@Car", SqlDbType.NVarChar, DbNullIfEmpty(Car));
                AddParam(cmd, "@Two_Wheelers", SqlDbType.NVarChar, DbNullIfEmpty(Two_Wheelers));
                AddParam(cmd, "@walking", SqlDbType.NVarChar, DbNullIfEmpty(walking));
                AddParam(cmd, "@Public_Transport", SqlDbType.NVarChar, DbNullIfEmpty(Public_Transport));

                AddParam(cmd, "@Schoolinfo_NoOfTeacher", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_NoOfTeacher));
                AddParam(cmd, "@Floor", SqlDbType.NVarChar, DbNullIfEmpty(Floor));
                AddParam(cmd, "@Single_bench", SqlDbType.NVarChar, DbNullIfEmpty(Single_bench));
                AddParam(cmd, "@Bench2", SqlDbType.NVarChar, DbNullIfEmpty(Bench2));
                AddParam(cmd, "@Round_table", SqlDbType.NVarChar, DbNullIfEmpty(Round_table));
                AddParam(cmd, "@Schoolinfo_Mealtime", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Mealtime));
                AddParam(cmd, "@Schoolinfo_MealType", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_MealType));

                AddParam(cmd, "@Schoolinfo_Shareing", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Shareing));
                AddParam(cmd, "@Schoolinfo_HelpEating", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_HelpEating));
                AddParam(cmd, "@Schoolinfo_Friendship", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Friendship));
                AddParam(cmd, "@Schoolinfo_InteractionPeer", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_InteractionPeer));
                AddParam(cmd, "@Schoolinfo_InteractionTeacher", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_InteractionTeacher));

                AddParam(cmd, "@Schoolinfo_AnnualFunction", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_AnnualFunction));
                AddParam(cmd, "@Schoolinfo_Sports", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Sports));
                AddParam(cmd, "@Schoolinfo_Picnic", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Picnic));
                AddParam(cmd, "@Schoolinfo_ExtraCaricular", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_ExtraCaricular));

                AddParam(cmd, "@Schoolinfo_CopyBoard", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_CopyBoard));
                AddParam(cmd, "@Schoolinfo_Instructions", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_Instructions));
                AddParam(cmd, "@Schoolinfo_ShadowTeacher", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_ShadowTeacher));
                AddParam(cmd, "@Schoolinfo_CW_HW", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_CW_HW));

                AddParam(cmd, "@Schoolinfo_SpecialEducator", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_SpecialEducator));
                AddParam(cmd, "@Schoolinfo_DeliveryInformation", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_DeliveryInformation));

                AddParam(cmd, "@Schoolinfo_RemarkTeacher", SqlDbType.NVarChar, DbNullIfEmpty(Schoolinfo_RemarkTeacher));
                AddParam(cmd, "@SCHOOL_cmt", SqlDbType.NVarChar, DbNullIfEmpty(SCHOOL_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab4(HttpContext context, int appointmentID)
        {
            int tabNo = 4;

            string PersonalSocial_CurrentPlace = GetStr(context, "PersonalSocial_CurrentPlace");
            string PersonalSocial_WhatHeDoes = GetStr(context, "PersonalSocial_WhatHeDoes");
            string PersonalSocial_BodyAwareness = GetStr(context, "PersonalSocial_BodyAwareness");
            string PersonalSocial_BodySchema = GetStr(context, "PersonalSocial_BodySchema");
            string PersonalSocial_ExploreEnvironment = GetStr(context, "PersonalSocial_ExploreEnvironment");
            string PersonalSocial_Motivated = GetStr(context, "PersonalSocial_Motivated");

            string PersonalSocial_EyeContact = GetStr(context, "PersonalSocial_EyeContact");
            string PersonalSocial_SocialSmile = GetStr(context, "PersonalSocial_SocialSmile");
            string PersonalSocial_FamilyRegards = GetStr(context, "PersonalSocial_FamilyRegards");
            string PersonalSocial_ChildSocially = GetStr(context, "PersonalSocial_ChildSocially");

            string PERSONAL_cmt = GetStr(context, "PERSONAL_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@PersonalSocial_CurrentPlace", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_CurrentPlace));
                AddParam(cmd, "@PersonalSocial_WhatHeDoes", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_WhatHeDoes));
                AddParam(cmd, "@PersonalSocial_BodyAwareness", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_BodyAwareness));
                AddParam(cmd, "@PersonalSocial_BodySchema", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_BodySchema));
                AddParam(cmd, "@PersonalSocial_ExploreEnvironment", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_ExploreEnvironment));
                AddParam(cmd, "@PersonalSocial_Motivated", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_Motivated));

                AddParam(cmd, "@PersonalSocial_EyeContact", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_EyeContact));
                AddParam(cmd, "@PersonalSocial_SocialSmile", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_SocialSmile));
                AddParam(cmd, "@PersonalSocial_FamilyRegards", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_FamilyRegards));
                AddParam(cmd, "@PersonalSocial_ChildSocially", SqlDbType.NVarChar, DbNullIfEmpty(PersonalSocial_ChildSocially));

                AddParam(cmd, "@PERSONAL_cmt", SqlDbType.NVarChar, DbNullIfEmpty(PERSONAL_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab5(HttpContext context, int appointmentID)
        {
            int tabNo = 5;

            string SpeechLanguage_StartSpeek = GetStr(context, "SpeechLanguage_StartSpeek");
            string SpeechLanguage_Monosyllables = GetStr(context, "SpeechLanguage_Monosyllables");
            string SpeechLanguage_Bisyllables = GetStr(context, "SpeechLanguage_Bisyllables");
            string SpeechLanguage_ShrotScentences = GetStr(context, "SpeechLanguage_ShrotScentences");
            string SpeechLanguage_LongScentences = GetStr(context, "SpeechLanguage_LongScentences");

            string SpeechLanguage_UnusualSoundsJargonSpeech = GetStr(context, "SpeechLanguage_UnusualSoundsJargonSpeech");
            string SpeechLanguage_speechgestures = GetStr(context, "SpeechLanguage_speechgestures");

            string SpeechLanguage_NonverbalfacialExpression = GetStr(context, "SpeechLanguage_NonverbalfacialExpression");
            string SpeechLanguage_NonverbalfacialEyeContact = GetStr(context, "SpeechLanguage_NonverbalfacialEyeContact");
            string SpeechLanguage_NonverbalfacialGestures = GetStr(context, "SpeechLanguage_NonverbalfacialGestures");

            string SpeechLanguage_SimpleComplex = GetStr(context, "SpeechLanguage_SimpleComplex");
            string SpeechLanguage_UnderstandImpliedMeaning = GetStr(context, "SpeechLanguage_UnderstandImpliedMeaning");
            string SpeechLanguage_UnderstandJokesarcasm = GetStr(context, "SpeechLanguage_UnderstandJokesarcasm");
            string SpeechLanguage_Respondstoname = GetStr(context, "SpeechLanguage_Respondstoname");

            string SpeechLanguage_TwowayInteraction = GetStr(context, "SpeechLanguage_TwowayInteraction");

            string SpeechLanguage_NarrateIncidentsAtSchool = GetStr(context, "SpeechLanguage_NarrateIncidentsAtSchool");
            string SpeechLanguage_NarrateIncidentsAtHome = GetStr(context, "SpeechLanguage_NarrateIncidentsAtHome");

            string SpeechLanguage_Needs = GetStr(context, "SpeechLanguage_Needs");
            string SpeechLanguage_Emotions = GetStr(context, "SpeechLanguage_Emotions");
            string SpeechLanguage_AchievementsFailure = GetStr(context, "SpeechLanguage_AchievementsFailure");

            string SpeechLanguage_Echolalia = GetStr(context, "SpeechLanguage_Echolalia");

            string Speech_cmt = GetStr(context, "Speech_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@SpeechLanguage_StartSpeek", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_StartSpeek));
                AddParam(cmd, "@SpeechLanguage_Monosyllables", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Monosyllables));
                AddParam(cmd, "@SpeechLanguage_Bisyllables", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Bisyllables));
                AddParam(cmd, "@SpeechLanguage_ShrotScentences", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_ShrotScentences));
                AddParam(cmd, "@SpeechLanguage_LongScentences", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_LongScentences));

                AddParam(cmd, "@SpeechLanguage_UnusualSoundsJargonSpeech", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_UnusualSoundsJargonSpeech));
                AddParam(cmd, "@SpeechLanguage_speechgestures", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_speechgestures));

                AddParam(cmd, "@SpeechLanguage_NonverbalfacialExpression", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_NonverbalfacialExpression));
                AddParam(cmd, "@SpeechLanguage_NonverbalfacialEyeContact", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_NonverbalfacialEyeContact));
                AddParam(cmd, "@SpeechLanguage_NonverbalfacialGestures", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_NonverbalfacialGestures));

                AddParam(cmd, "@SpeechLanguage_SimpleComplex", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_SimpleComplex));
                AddParam(cmd, "@SpeechLanguage_UnderstandImpliedMeaning", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_UnderstandImpliedMeaning));
                AddParam(cmd, "@SpeechLanguage_UnderstandJokesarcasm", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_UnderstandJokesarcasm));
                AddParam(cmd, "@SpeechLanguage_Respondstoname", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Respondstoname));

                AddParam(cmd, "@SpeechLanguage_TwowayInteraction", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_TwowayInteraction));

                AddParam(cmd, "@SpeechLanguage_NarrateIncidentsAtSchool", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_NarrateIncidentsAtSchool));
                AddParam(cmd, "@SpeechLanguage_NarrateIncidentsAtHome", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_NarrateIncidentsAtHome));

                AddParam(cmd, "@SpeechLanguage_Needs", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Needs));
                AddParam(cmd, "@SpeechLanguage_Emotions", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Emotions));
                AddParam(cmd, "@SpeechLanguage_AchievementsFailure", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_AchievementsFailure));

                AddParam(cmd, "@SpeechLanguage_Echolalia", SqlDbType.NVarChar, DbNullIfEmpty(SpeechLanguage_Echolalia));

                AddParam(cmd, "@Speech_cmt", SqlDbType.NVarChar, DbNullIfEmpty(Speech_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab6(HttpContext context, int appointmentID)
        {
            int tabNo = 6;

            string Behaviour_FreeTime = GetStr(context, "Behaviour_FreeTime");

            string unassociated = GetStr(context, "unassociated");
            string solitary = GetStr(context, "solitary");
            string onlooker = GetStr(context, "onlooker");
            string parallel = GetStr(context, "parallel");
            string associative = GetStr(context, "associative");
            string cooperative = GetStr(context, "cooperative");

            string Behaviour_situationalmeltdowns = GetStr(context, "Behaviour_situationalmeltdowns");
            string BEHAVIOUR_cmt = GetStr(context, "BEHAVIOUR_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Behaviour_FreeTime", SqlDbType.NVarChar, DbNullIfEmpty(Behaviour_FreeTime));

                AddParam(cmd, "@unassociated", SqlDbType.NVarChar, DbNullIfEmpty(unassociated));
                AddParam(cmd, "@solitary", SqlDbType.NVarChar, DbNullIfEmpty(solitary));
                AddParam(cmd, "@onlooker", SqlDbType.NVarChar, DbNullIfEmpty(onlooker));
                AddParam(cmd, "@parallel", SqlDbType.NVarChar, DbNullIfEmpty(parallel));
                AddParam(cmd, "@associative", SqlDbType.NVarChar, DbNullIfEmpty(associative));
                AddParam(cmd, "@cooperative", SqlDbType.NVarChar, DbNullIfEmpty(cooperative));

                AddParam(cmd, "@Behaviour_situationalmeltdowns", SqlDbType.NVarChar, DbNullIfEmpty(Behaviour_situationalmeltdowns));
                AddParam(cmd, "@BEHAVIOUR_cmt", SqlDbType.NVarChar, DbNullIfEmpty(BEHAVIOUR_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab7(HttpContext context, int appointmentID)
        {
            int tabNo = 7;

            string rangevalue = GetStr(context, "rangevalue");   // slider1 raw 0..100
            string rangevalue2 = GetStr(context, "rangevalue2"); // slider2 raw 0..100

            int range1 = 0;
            if (!string.IsNullOrEmpty(rangevalue))
            {
                int.TryParse(rangevalue, out range1);
                range1 = range1 / 10;
            }

            int range2 = 0;
            if (!string.IsNullOrEmpty(rangevalue2))
            {
                int.TryParse(rangevalue2, out range2);
                range2 = range2 / 10;
            }

            string Arousal_Stimuli = GetStr(context, "Arousal_Stimuli");
            string Arousal_Transition = GetStr(context, "Arousal_Transition");

            string Arousal_FactorOCD = GetStr(context, "Arousal_FactorOCD");
            string Arousal_ClaimingFactor = GetStr(context, "Arousal_ClaimingFactor");
            string Arousal_DipsDown = GetStr(context, "Arousal_DipsDown");

            string AROUSAL_cmt = GetStr(context, "AROUSAL_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@rangevalue", SqlDbType.Int, range1);
                AddParam(cmd, "@rangevalue2", SqlDbType.Int, range2);

                AddParam(cmd, "@Arousal_Stimuli", SqlDbType.NVarChar, DbNullIfEmpty(Arousal_Stimuli));
                AddParam(cmd, "@Arousal_Transition", SqlDbType.NVarChar, DbNullIfEmpty(Arousal_Transition));

                AddParam(cmd, "@Arousal_FactorOCD", SqlDbType.NVarChar, DbNullIfEmpty(Arousal_FactorOCD));
                AddParam(cmd, "@Arousal_ClaimingFactor", SqlDbType.NVarChar, DbNullIfEmpty(Arousal_ClaimingFactor));
                AddParam(cmd, "@Arousal_DipsDown", SqlDbType.NVarChar, DbNullIfEmpty(Arousal_DipsDown));

                AddParam(cmd, "@AROUSAL_cmt", SqlDbType.NVarChar, DbNullIfEmpty(AROUSAL_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab8(HttpContext context, int appointmentID)
        {
            int tabNo = 8;

            string Affect_RangeEmotion = GetStr(context, "Affect_RangeEmotion");
            string Affect_ExpressEmotion = GetStr(context, "Affect_ExpressEmotion");

            string Affect_Environment = GetStr(context, "Affect_Environment");
            string Affect_Task = GetStr(context, "Affect_Task");
            string Affect_Individual = GetStr(context, "Affect_Individual");
            string Affect_ThroughOut = GetStr(context, "Affect_ThroughOut");
            string Affect_Charaterising = GetStr(context, "Affect_Charaterising");

            string Affect_cmt = GetStr(context, "Affect_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Affect_RangeEmotion", SqlDbType.NVarChar, DbNullIfEmpty(Affect_RangeEmotion));
                AddParam(cmd, "@Affect_ExpressEmotion", SqlDbType.NVarChar, DbNullIfEmpty(Affect_ExpressEmotion));

                AddParam(cmd, "@Affect_Environment", SqlDbType.NVarChar, DbNullIfEmpty(Affect_Environment));
                AddParam(cmd, "@Affect_Task", SqlDbType.NVarChar, DbNullIfEmpty(Affect_Task));
                AddParam(cmd, "@Affect_Individual", SqlDbType.NVarChar, DbNullIfEmpty(Affect_Individual));
                AddParam(cmd, "@Affect_ThroughOut", SqlDbType.NVarChar, DbNullIfEmpty(Affect_ThroughOut));
                AddParam(cmd, "@Affect_Charaterising", SqlDbType.NVarChar, DbNullIfEmpty(Affect_Charaterising));

                AddParam(cmd, "@Affect_cmt", SqlDbType.NVarChar, DbNullIfEmpty(Affect_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab9(HttpContext context, int appointmentID)
        {
            int tabNo = 9;

            string Attention_AttentionSpan = GetStr(context, "Attention_AttentionSpan");
            string Attention_FocusHandhome = GetStr(context, "Attention_FocusHandhome");
            string Attention_FocusHandSchool = GetStr(context, "Attention_FocusHandSchool");
            string Attention_Dividing = GetStr(context, "Attention_Dividing");

            string Attention_ChangeActivities = GetStr(context, "Attention_ChangeActivities");
            string Attention_AgeAppropriate = GetStr(context, "Attention_AgeAppropriate");
            string Attention_Distractibility = GetStr(context, "Attention_Distractibility");

            string Focal_Attention = GetStr(context, "Focal_Attention");
            string Joint_Attention = GetStr(context, "Joint_Attention");
            string Divided_Attention = GetStr(context, "Divided_Attention");
            string Sustained_Attention = GetStr(context, "Sustained_Attention");
            string Alternating_Attention = GetStr(context, "Alternating_Attention");

            string Attention_move = GetStr(context, "Attention_move");
            string ATTENTION_cmt = GetStr(context, "ATTENTION_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Attention_FocusHandhome", SqlDbType.NVarChar, DbNullIfEmpty(Attention_FocusHandhome));
                AddParam(cmd, "@Attention_FocusHandSchool", SqlDbType.NVarChar, DbNullIfEmpty(Attention_FocusHandSchool));
                AddParam(cmd, "@Attention_Dividing", SqlDbType.NVarChar, DbNullIfEmpty(Attention_Dividing));

                AddParam(cmd, "@Attention_ChangeActivities", SqlDbType.NVarChar, DbNullIfEmpty(Attention_ChangeActivities));
                AddParam(cmd, "@Attention_AgeAppropriate", SqlDbType.NVarChar, DbNullIfEmpty(Attention_AgeAppropriate));
                AddParam(cmd, "@Attention_AttentionSpan", SqlDbType.NVarChar, DbNullIfEmpty(Attention_AttentionSpan));
                AddParam(cmd, "@Attention_Distractibility", SqlDbType.NVarChar, DbNullIfEmpty(Attention_Distractibility));

                AddParam(cmd, "@Focal_Attention", SqlDbType.NVarChar, DbNullIfEmpty(Focal_Attention));
                AddParam(cmd, "@Joint_Attention", SqlDbType.NVarChar, DbNullIfEmpty(Joint_Attention));
                AddParam(cmd, "@Divided_Attention", SqlDbType.NVarChar, DbNullIfEmpty(Divided_Attention));
                AddParam(cmd, "@Sustained_Attention", SqlDbType.NVarChar, DbNullIfEmpty(Sustained_Attention));
                AddParam(cmd, "@Alternating_Attention", SqlDbType.NVarChar, DbNullIfEmpty(Alternating_Attention));

                AddParam(cmd, "@Attention_move", SqlDbType.NVarChar, DbNullIfEmpty(Attention_move));
                AddParam(cmd, "@ATTENTION_cmt", SqlDbType.NVarChar, DbNullIfEmpty(ATTENTION_cmt));

                // ✅ OUTPUT PARAM REQUIRED
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab10(HttpContext context, int appointmentID)
        {
            int tabNo = 10;

            string Action_MotorPlanning = GetStr(context, "Action_MotorPlanning");

            string Action_Purposeful = GetStr(context, "Action_Purposeful");
            string Action_GoalOriented = GetStr(context, "Action_GoalOriented");
            string Action_FeedBackDependent = GetStr(context, "Action_FeedBackDependent");
            string Action_Constructive = GetStr(context, "Action_Constructive");

            string Action_cmt = GetStr(context, "Action_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Action_MotorPlanning", SqlDbType.NVarChar, DbNullIfEmpty(Action_MotorPlanning));

                AddParam(cmd, "@Action_Purposeful", SqlDbType.NVarChar, DbNullIfEmpty(Action_Purposeful));
                AddParam(cmd, "@Action_GoalOriented", SqlDbType.NVarChar, DbNullIfEmpty(Action_GoalOriented));
                AddParam(cmd, "@Action_FeedBackDependent", SqlDbType.NVarChar, DbNullIfEmpty(Action_FeedBackDependent));
                AddParam(cmd, "@Action_Constructive", SqlDbType.NVarChar, DbNullIfEmpty(Action_Constructive));

                AddParam(cmd, "@Action_cmt", SqlDbType.NVarChar, DbNullIfEmpty(Action_cmt));

                // ✅ OUTPUT PARAM REQUIRED
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab11(HttpContext context, int appointmentID)
        {
            int tabNo = 11;

            string Interacts = GetStr(context, "Interacts");
            string cmtgathering = GetStr(context, "cmtgathering");

            string Does_not_initiate = GetStr(context, "Does_not_initiate");
            string Sustain = GetStr(context, "Sustain");

            string Fight = GetStr(context, "Fight");
            string Freeze = GetStr(context, "Freeze");
            string Fright = GetStr(context, "Fright");

            string Anxious = GetStr(context, "Anxious");
            string Comfortable = GetStr(context, "Comfortable");
            string Nervous = GetStr(context, "Nervous");

            string ANS_response = GetStr(context, "ANS_response");
            string OTHERS = GetStr(context, "OTHERS");

            string Interaction_SocialQues = GetStr(context, "Interaction_SocialQues");

            string Interaction_Happiness = GetStr(context, "Interaction_Happiness");
            string Interaction_Sadness = GetStr(context, "Interaction_Sadness");
            string Interaction_Surprise = GetStr(context, "Interaction_Surprise");
            string Interaction_Shock = GetStr(context, "Interaction_Shock");

            string Interaction_Friends = GetStr(context, "Interaction_Friends");
            string Interaction_Enjoy = GetStr(context, "Interaction_Enjoy");

            string INTERACTION_cmt = GetStr(context, "INTERACTION_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Interacts", SqlDbType.NVarChar, DbNullIfEmpty(Interacts));
                AddParam(cmd, "@cmtgathering", SqlDbType.NVarChar, DbNullIfEmpty(cmtgathering));

                AddParam(cmd, "@Does_not_initiate", SqlDbType.NVarChar, DbNullIfEmpty(Does_not_initiate));
                AddParam(cmd, "@Sustain", SqlDbType.NVarChar, DbNullIfEmpty(Sustain));

                AddParam(cmd, "@Fight", SqlDbType.NVarChar, DbNullIfEmpty(Fight));
                AddParam(cmd, "@Freeze", SqlDbType.NVarChar, DbNullIfEmpty(Freeze));
                AddParam(cmd, "@Fright", SqlDbType.NVarChar, DbNullIfEmpty(Fright));

                AddParam(cmd, "@Anxious", SqlDbType.NVarChar, DbNullIfEmpty(Anxious));
                AddParam(cmd, "@Comfortable", SqlDbType.NVarChar, DbNullIfEmpty(Comfortable));
                AddParam(cmd, "@Nervous", SqlDbType.NVarChar, DbNullIfEmpty(Nervous));

                AddParam(cmd, "@ANS_response", SqlDbType.NVarChar, DbNullIfEmpty(ANS_response));
                AddParam(cmd, "@OTHERS", SqlDbType.NVarChar, DbNullIfEmpty(OTHERS));

                AddParam(cmd, "@Interaction_SocialQues", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_SocialQues));

                AddParam(cmd, "@Interaction_Happiness", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Happiness));
                AddParam(cmd, "@Interaction_Sadness", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Sadness));
                AddParam(cmd, "@Interaction_Surprise", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Surprise));
                AddParam(cmd, "@Interaction_Shock", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Shock));

                AddParam(cmd, "@Interaction_Friends", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Friends));
                AddParam(cmd, "@Interaction_Enjoy", SqlDbType.NVarChar, DbNullIfEmpty(Interaction_Enjoy));

                AddParam(cmd, "@INTERACTION_cmt", SqlDbType.NVarChar, DbNullIfEmpty(INTERACTION_cmt));

                // ✅ OUTPUT PARAM REQUIRED
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab12(HttpContext context, int appointmentID)
        {
            int tabNo = 12;

            string TS_Registration = GetStr(context, "TS_Registration");
            string TS_Orientation = GetStr(context, "TS_Orientation");
            string TS_Discrimination = GetStr(context, "TS_Discrimination");
            string TS_Responsiveness = GetStr(context, "TS_Responsiveness");

            string SS_Bodyawareness = GetStr(context, "SS_Bodyawareness");
            string SS_Bodyschema = GetStr(context, "SS_Bodyschema");
            string SS_Orientation = GetStr(context, "SS_Orientation");
            string SS_Posterior = GetStr(context, "SS_Posterior");
            string SS_Bilateral = GetStr(context, "SS_Bilateral");
            string SS_Balance = GetStr(context, "SS_Balance");
            string SS_Dominance = GetStr(context, "SS_Dominance");
            string SS_Right = GetStr(context, "SS_Right");
            string SS_identifies = GetStr(context, "SS_identifies");
            string SS_point = GetStr(context, "SS_point");
            string SS_Constantly = GetStr(context, "SS_Constantly");
            string SS_clumsy = GetStr(context, "SS_clumsy");
            string SS_maneuver = GetStr(context, "SS_maneuver");
            string SS_overly = GetStr(context, "SS_overly");
            string SS_stand = GetStr(context, "SS_stand");
            string SS_indulge = GetStr(context, "SS_indulge");
            string SS_textures = GetStr(context, "SS_textures");
            string SS_monkey = GetStr(context, "SS_monkey");
            string SS_swings = GetStr(context, "SS_swings");

            string VM_Registration = GetStr(context, "VM_Registration");
            string VM_Orientation = GetStr(context, "VM_Orientation");
            string VM_Discrimination = GetStr(context, "VM_Discrimination");
            string VM_Responsiveness = GetStr(context, "VM_Responsiveness");

            string PS_Registration = GetStr(context, "PS_Registration");
            string PS_Gradation = GetStr(context, "PS_Gradation");
            string PS_Discrimination = GetStr(context, "PS_Discrimination");
            string PS_Responsiveness = GetStr(context, "PS_Responsiveness");

            string OM_Registration = GetStr(context, "OM_Registration");
            string OM_Orientation = GetStr(context, "OM_Orientation");
            string OM_Discrimination = GetStr(context, "OM_Discrimination");
            string OM_Responsiveness = GetStr(context, "OM_Responsiveness");

            string AS_Auditory = GetStr(context, "AS_Auditory");
            string AS_Orientation = GetStr(context, "AS_Orientation");
            string AS_Responsiveness = GetStr(context, "AS_Responsiveness");
            string AS_discrimination = GetStr(context, "AS_discrimination");
            string AS_Background = GetStr(context, "AS_Background");
            string AS_localization = GetStr(context, "AS_localization");
            string AS_Analysis = GetStr(context, "AS_Analysis");
            string AS_sequencing = GetStr(context, "AS_sequencing");
            string AS_blending = GetStr(context, "AS_blending");

            string VS_Visual = GetStr(context, "VS_Visual");
            string VS_Responsiveness = GetStr(context, "VS_Responsiveness");
            string VS_scanning = GetStr(context, "VS_scanning");
            string VS_constancy = GetStr(context, "VS_constancy");
            string VS_memory = GetStr(context, "VS_memory");
            string VS_Perception = GetStr(context, "VS_Perception");
            string VS_hand = GetStr(context, "VS_hand");
            string VS_foot = GetStr(context, "VS_foot");
            string VS_discrimination = GetStr(context, "VS_discrimination");
            string VS_closure = GetStr(context, "VS_closure");
            string VS_Figureground = GetStr(context, "VS_Figureground");
            string VS_Visualmemory = GetStr(context, "VS_Visualmemory");
            string VS_sequential = GetStr(context, "VS_sequential");
            string VS_spatial = GetStr(context, "VS_spatial");

            string OS_Registration = GetStr(context, "OS_Registration");
            string OS_Orientation = GetStr(context, "OS_Orientation");
            string OS_Discrimination = GetStr(context, "OS_Discrimination");
            string OS_Responsiveness = GetStr(context, "OS_Responsiveness");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@TS_Registration", SqlDbType.NVarChar, DbNullIfEmpty(TS_Registration));
                AddParam(cmd, "@TS_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(TS_Orientation));
                AddParam(cmd, "@TS_Discrimination", SqlDbType.NVarChar, DbNullIfEmpty(TS_Discrimination));
                AddParam(cmd, "@TS_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(TS_Responsiveness));

                AddParam(cmd, "@SS_Bodyawareness", SqlDbType.NVarChar, DbNullIfEmpty(SS_Bodyawareness));
                AddParam(cmd, "@SS_Bodyschema", SqlDbType.NVarChar, DbNullIfEmpty(SS_Bodyschema));
                AddParam(cmd, "@SS_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(SS_Orientation));
                AddParam(cmd, "@SS_Posterior", SqlDbType.NVarChar, DbNullIfEmpty(SS_Posterior));
                AddParam(cmd, "@SS_Bilateral", SqlDbType.NVarChar, DbNullIfEmpty(SS_Bilateral));
                AddParam(cmd, "@SS_Balance", SqlDbType.NVarChar, DbNullIfEmpty(SS_Balance));
                AddParam(cmd, "@SS_Dominance", SqlDbType.NVarChar, DbNullIfEmpty(SS_Dominance));
                AddParam(cmd, "@SS_Right", SqlDbType.NVarChar, DbNullIfEmpty(SS_Right));
                AddParam(cmd, "@SS_identifies", SqlDbType.NVarChar, DbNullIfEmpty(SS_identifies));
                AddParam(cmd, "@SS_point", SqlDbType.NVarChar, DbNullIfEmpty(SS_point));
                AddParam(cmd, "@SS_Constantly", SqlDbType.NVarChar, DbNullIfEmpty(SS_Constantly));
                AddParam(cmd, "@SS_clumsy", SqlDbType.NVarChar, DbNullIfEmpty(SS_clumsy));
                AddParam(cmd, "@SS_maneuver", SqlDbType.NVarChar, DbNullIfEmpty(SS_maneuver));
                AddParam(cmd, "@SS_overly", SqlDbType.NVarChar, DbNullIfEmpty(SS_overly));
                AddParam(cmd, "@SS_stand", SqlDbType.NVarChar, DbNullIfEmpty(SS_stand));
                AddParam(cmd, "@SS_indulge", SqlDbType.NVarChar, DbNullIfEmpty(SS_indulge));
                AddParam(cmd, "@SS_textures", SqlDbType.NVarChar, DbNullIfEmpty(SS_textures));
                AddParam(cmd, "@SS_monkey", SqlDbType.NVarChar, DbNullIfEmpty(SS_monkey));
                AddParam(cmd, "@SS_swings", SqlDbType.NVarChar, DbNullIfEmpty(SS_swings));

                AddParam(cmd, "@VM_Registration", SqlDbType.NVarChar, DbNullIfEmpty(VM_Registration));
                AddParam(cmd, "@VM_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(VM_Orientation));
                AddParam(cmd, "@VM_Discrimination", SqlDbType.NVarChar, DbNullIfEmpty(VM_Discrimination));
                AddParam(cmd, "@VM_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(VM_Responsiveness));

                AddParam(cmd, "@PS_Registration", SqlDbType.NVarChar, DbNullIfEmpty(PS_Registration));
                AddParam(cmd, "@PS_Gradation", SqlDbType.NVarChar, DbNullIfEmpty(PS_Gradation));
                AddParam(cmd, "@PS_Discrimination", SqlDbType.NVarChar, DbNullIfEmpty(PS_Discrimination));
                AddParam(cmd, "@PS_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(PS_Responsiveness));

                AddParam(cmd, "@OM_Registration", SqlDbType.NVarChar, DbNullIfEmpty(OM_Registration));
                AddParam(cmd, "@OM_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(OM_Orientation));
                AddParam(cmd, "@OM_Discrimination", SqlDbType.NVarChar, DbNullIfEmpty(OM_Discrimination));
                AddParam(cmd, "@OM_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(OM_Responsiveness));

                AddParam(cmd, "@AS_Auditory", SqlDbType.NVarChar, DbNullIfEmpty(AS_Auditory));
                AddParam(cmd, "@AS_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(AS_Orientation));
                AddParam(cmd, "@AS_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(AS_Responsiveness));
                AddParam(cmd, "@AS_discrimination", SqlDbType.NVarChar, DbNullIfEmpty(AS_discrimination));
                AddParam(cmd, "@AS_Background", SqlDbType.NVarChar, DbNullIfEmpty(AS_Background));
                AddParam(cmd, "@AS_localization", SqlDbType.NVarChar, DbNullIfEmpty(AS_localization));
                AddParam(cmd, "@AS_Analysis", SqlDbType.NVarChar, DbNullIfEmpty(AS_Analysis));
                AddParam(cmd, "@AS_sequencing", SqlDbType.NVarChar, DbNullIfEmpty(AS_sequencing));
                AddParam(cmd, "@AS_blending", SqlDbType.NVarChar, DbNullIfEmpty(AS_blending));

                AddParam(cmd, "@VS_Visual", SqlDbType.NVarChar, DbNullIfEmpty(VS_Visual));
                AddParam(cmd, "@VS_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(VS_Responsiveness));
                AddParam(cmd, "@VS_scanning", SqlDbType.NVarChar, DbNullIfEmpty(VS_scanning));
                AddParam(cmd, "@VS_constancy", SqlDbType.NVarChar, DbNullIfEmpty(VS_constancy));
                AddParam(cmd, "@VS_memory", SqlDbType.NVarChar, DbNullIfEmpty(VS_memory));
                AddParam(cmd, "@VS_Perception", SqlDbType.NVarChar, DbNullIfEmpty(VS_Perception));
                AddParam(cmd, "@VS_hand", SqlDbType.NVarChar, DbNullIfEmpty(VS_hand));
                AddParam(cmd, "@VS_foot", SqlDbType.NVarChar, DbNullIfEmpty(VS_foot));
                AddParam(cmd, "@VS_discrimination", SqlDbType.NVarChar, DbNullIfEmpty(VS_discrimination));
                AddParam(cmd, "@VS_closure", SqlDbType.NVarChar, DbNullIfEmpty(VS_closure));
                AddParam(cmd, "@VS_Figureground", SqlDbType.NVarChar, DbNullIfEmpty(VS_Figureground));
                AddParam(cmd, "@VS_Visualmemory", SqlDbType.NVarChar, DbNullIfEmpty(VS_Visualmemory));
                AddParam(cmd, "@VS_sequential", SqlDbType.NVarChar, DbNullIfEmpty(VS_sequential));
                AddParam(cmd, "@VS_spatial", SqlDbType.NVarChar, DbNullIfEmpty(VS_spatial));

                AddParam(cmd, "@OS_Registration", SqlDbType.NVarChar, DbNullIfEmpty(OS_Registration));
                AddParam(cmd, "@OS_Orientation", SqlDbType.NVarChar, DbNullIfEmpty(OS_Orientation));
                AddParam(cmd, "@OS_Discrimination", SqlDbType.NVarChar, DbNullIfEmpty(OS_Discrimination));
                AddParam(cmd, "@OS_Responsiveness", SqlDbType.NVarChar, DbNullIfEmpty(OS_Responsiveness));

                // ✅ OUTPUT PARAM REQUIRED
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab13(HttpContext context, int appointmentID)
        {
            int tabNo = 13;

            string TestMeassures_GrossMotor = GetStr(context, "TestMeassures_GrossMotor");
            string TestMeassures_FineMotor = GetStr(context, "TestMeassures_FineMotor");
            string TestMeassures_DenverLanguage = GetStr(context, "TestMeassures_DenverLanguage");
            string TestMeassures_DenverPersonal = GetStr(context, "TestMeassures_DenverPersonal");
            string Tests_cmt = GetStr(context, "Tests_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@TestMeassures_GrossMotor", SqlDbType.NVarChar, DbNullIfEmpty(TestMeassures_GrossMotor));
                AddParam(cmd, "@TestMeassures_FineMotor", SqlDbType.NVarChar, DbNullIfEmpty(TestMeassures_FineMotor));
                AddParam(cmd, "@TestMeassures_DenverLanguage", SqlDbType.NVarChar, DbNullIfEmpty(TestMeassures_DenverLanguage));
                AddParam(cmd, "@TestMeassures_DenverPersonal", SqlDbType.NVarChar, DbNullIfEmpty(TestMeassures_DenverPersonal));

                AddParam(cmd, "@Tests_cmt", SqlDbType.NVarChar, DbNullIfEmpty(Tests_cmt));

                // ✅ Output param required
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab14(HttpContext context, int appointmentID)
        {
            int tabValue = 14;

            // month value from ajax
            string month = GetStr(context, "MONTHS");

            string questions = GetStr(context, "questions");


            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@score_Communication_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "score_Communication_2")));
                AddParam(cmd, "@Inter_Communication_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "Inter_Communication_2")));
                AddParam(cmd, "@GROSS_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_2")));
                AddParam(cmd, "@inter_Gross_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_Gross_2")));
                AddParam(cmd, "@FINE_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_2")));
                AddParam(cmd, "@inter_FINE_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_FINE_2")));
                AddParam(cmd, "@PROBLEM_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_2")));
                AddParam(cmd, "@inter_PROBLEM_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_PROBLEM_2")));
                AddParam(cmd, "@PERSONAL_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_2")));
                AddParam(cmd, "@inter_PERSONAL_2", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_PERSONAL_2")));

                // ======= 3 MONTHS =======
                AddParam(cmd, "@Comm_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "Comm_3")));
                AddParam(cmd, "@inter_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_3")));
                AddParam(cmd, "@GROSS_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_3")));
                AddParam(cmd, "@GROSS_inter_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_3")));
                AddParam(cmd, "@FINE_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_3")));
                AddParam(cmd, "@FINE_inter_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_3")));
                AddParam(cmd, "@PROBLEM_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_3")));
                AddParam(cmd, "@PROBLEM_inter_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_3")));
                AddParam(cmd, "@PERSONAL_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_3")));
                AddParam(cmd, "@PERSONAL_inter_3", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_3")));

                // ======= 6 MONTHS =======
                AddParam(cmd, "@Communication_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "Communication_6")));
                AddParam(cmd, "@comm_inter_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_inter_6")));
                AddParam(cmd, "@GROSS_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_6")));
                AddParam(cmd, "@GROSS_inter_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_6")));
                AddParam(cmd, "@FINE_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_6")));
                AddParam(cmd, "@FINE_inter_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_6")));
                AddParam(cmd, "@PROBLEM_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_6")));
                AddParam(cmd, "@PROBLEM_inter_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_6")));
                AddParam(cmd, "@PERSONAL_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_6")));
                AddParam(cmd, "@PERSONAL_inter_6", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_6")));

                // ======= 7 MONTHS =======
                AddParam(cmd, "@comm_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_7")));
                AddParam(cmd, "@inter_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_7")));
                AddParam(cmd, "@GROSS_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_7")));
                AddParam(cmd, "@GROSS_inter_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_7")));
                AddParam(cmd, "@FINE_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_7")));
                AddParam(cmd, "@FINE_inter_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_7")));
                AddParam(cmd, "@PROBLEM_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_7")));
                AddParam(cmd, "@PROBLEM_inter_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_7")));
                AddParam(cmd, "@PERSONAL_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_7")));
                AddParam(cmd, "@PERSONAL_inter_7", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_7")));

                // ======= 9 MONTHS =======
                AddParam(cmd, "@comm_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_9")));
                AddParam(cmd, "@inter_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_9")));
                AddParam(cmd, "@GROSS_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_9")));
                AddParam(cmd, "@GROSS_inter_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_9")));
                AddParam(cmd, "@FINE_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_9")));
                AddParam(cmd, "@FINE_inter_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_9")));
                AddParam(cmd, "@PROBLEM_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_9")));
                AddParam(cmd, "@PROBLEM_inter_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_9")));
                AddParam(cmd, "@PERSONAL_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_9")));
                AddParam(cmd, "@PERSONAL_inter_9", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_9")));

                // ======= 10 MONTHS =======
                AddParam(cmd, "@comm_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_10")));
                AddParam(cmd, "@inter_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_10")));
                AddParam(cmd, "@GROSS_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_10")));
                AddParam(cmd, "@GROSS_inter_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_10")));
                AddParam(cmd, "@FINE_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_10")));
                AddParam(cmd, "@FINE_inter_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_10")));
                AddParam(cmd, "@PROBLEM_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_10")));
                AddParam(cmd, "@PROBLEM_inter_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_10")));
                AddParam(cmd, "@PERSONAL_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_10")));
                AddParam(cmd, "@PERSONAL_inter_10", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_10")));

                // ======= 11 MONTHS =======
                AddParam(cmd, "@comm_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_11")));
                AddParam(cmd, "@inter_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_11")));
                AddParam(cmd, "@GROSS_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_11")));
                AddParam(cmd, "@GROSS_inter_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_11")));
                AddParam(cmd, "@FINE_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_11")));
                AddParam(cmd, "@FINE_inter_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_11")));
                AddParam(cmd, "@PROBLEM_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_11")));
                AddParam(cmd, "@PROBLEM_inter_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_11")));
                AddParam(cmd, "@PERSONAL_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_11")));
                AddParam(cmd, "@PERSONAL_inter_11", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_11")));

                // ======= 13 MONTHS =======
                AddParam(cmd, "@comm_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_13")));
                AddParam(cmd, "@inter_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_13")));
                AddParam(cmd, "@GROSS_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_13")));
                AddParam(cmd, "@GROSS_inter_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_13")));
                AddParam(cmd, "@FINE_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_13")));
                AddParam(cmd, "@FINE_inter_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_13")));
                AddParam(cmd, "@PROBLEM_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_13")));
                AddParam(cmd, "@PROBLEM_inter_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_13")));
                AddParam(cmd, "@PERSONAL_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_13")));
                AddParam(cmd, "@PERSONAL_inter_13", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_13")));

                // ======= 15 MONTHS =======
                AddParam(cmd, "@comm_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_15")));
                AddParam(cmd, "@inter_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_15")));
                AddParam(cmd, "@GROSS_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_15")));
                AddParam(cmd, "@GROSS_inter_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_15")));
                AddParam(cmd, "@FINE_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_15")));
                AddParam(cmd, "@FINE_inter_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_15")));
                AddParam(cmd, "@PROBLEM_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_15")));
                AddParam(cmd, "@PROBLEM_inter_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_15")));
                AddParam(cmd, "@PERSONAL_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_15")));
                AddParam(cmd, "@PERSONAL_inter_15", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_15")));

                // ======= 17 MONTHS =======
                AddParam(cmd, "@comm_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_17")));
                AddParam(cmd, "@inter_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_17")));
                AddParam(cmd, "@GROSS_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_17")));
                AddParam(cmd, "@GROSS_inter_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_17")));
                AddParam(cmd, "@FINE_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_17")));
                AddParam(cmd, "@FINE_inter_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_17")));
                AddParam(cmd, "@PROBLEM_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_17")));
                AddParam(cmd, "@PROBLEM_inter_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_17")));
                AddParam(cmd, "@PERSONAL_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_17")));
                AddParam(cmd, "@PERSONAL_inter_17", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_17")));

                // ======= 19 MONTHS =======
                AddParam(cmd, "@comm_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_19")));
                AddParam(cmd, "@inter_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_19")));
                AddParam(cmd, "@GROSS_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_19")));
                AddParam(cmd, "@GROSS_inter_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_19")));
                AddParam(cmd, "@FINE_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_19")));
                AddParam(cmd, "@FINE_inter_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_19")));
                AddParam(cmd, "@PROBLEM_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_19")));
                AddParam(cmd, "@PROBLEM_inter_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_19")));
                AddParam(cmd, "@PERSONAL_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_19")));
                AddParam(cmd, "@PERSONAL_inter_19", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_19")));

                // ======= 21 MONTHS =======
                AddParam(cmd, "@comm_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_21")));
                AddParam(cmd, "@inter_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_21")));
                AddParam(cmd, "@GROSS_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_21")));
                AddParam(cmd, "@GROSS_inter_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_21")));
                AddParam(cmd, "@FINE_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_21")));
                AddParam(cmd, "@FINE_inter_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_21")));
                AddParam(cmd, "@PROBLEM_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_21")));
                AddParam(cmd, "@PROBLEM_inter_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_21")));
                AddParam(cmd, "@PERSONAL_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_21")));
                AddParam(cmd, "@PERSONAL_inter_21", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_21")));

                // ======= 23 MONTHS =======
                AddParam(cmd, "@comm_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_23")));
                AddParam(cmd, "@inter_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_23")));
                AddParam(cmd, "@GROSS_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_23")));
                AddParam(cmd, "@GROSS_inter_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_23")));
                AddParam(cmd, "@FINE_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_23")));
                AddParam(cmd, "@FINE_inter_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_23")));
                AddParam(cmd, "@PROBLEM_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_23")));
                AddParam(cmd, "@PROBLEM_inter_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_23")));
                AddParam(cmd, "@PERSONAL_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_23")));
                AddParam(cmd, "@PERSONAL_inter_23", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_23")));

                // ======= 25 MONTHS =======
                AddParam(cmd, "@comm_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_25")));
                AddParam(cmd, "@inter_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_25")));
                AddParam(cmd, "@GROSS_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_25")));
                AddParam(cmd, "@GROSS_inter_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_25")));
                AddParam(cmd, "@FINE_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_25")));
                AddParam(cmd, "@FINE_inter_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_25")));
                AddParam(cmd, "@PROBLEM_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_25")));
                AddParam(cmd, "@PROBLEM_inter_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_25")));
                AddParam(cmd, "@PERSONAL_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_25")));
                AddParam(cmd, "@PERSONAL_inter_25", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_25")));

                // ======= 28 MONTHS =======
                AddParam(cmd, "@comm_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_28")));
                AddParam(cmd, "@inter_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_28")));
                AddParam(cmd, "@GROSS_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_28")));
                AddParam(cmd, "@GROSS_inter_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_28")));
                AddParam(cmd, "@FINE_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_28")));
                AddParam(cmd, "@FINE_inter_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_28")));
                AddParam(cmd, "@PROBLEM_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_28")));
                AddParam(cmd, "@PROBLEM_inter_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_28")));
                AddParam(cmd, "@PERSONAL_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_28")));
                AddParam(cmd, "@PERSONAL_inter_28", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_28")));

                // ======= 31 MONTHS =======
                AddParam(cmd, "@comm_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_31")));
                AddParam(cmd, "@inter_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_31")));
                AddParam(cmd, "@GROSS_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_31")));
                AddParam(cmd, "@GROSS_inter_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_31")));
                AddParam(cmd, "@FINE_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_31")));
                AddParam(cmd, "@FINE_inter_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_31")));
                AddParam(cmd, "@PROBLEM_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_31")));
                AddParam(cmd, "@PROBLEM_inter_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_31")));
                AddParam(cmd, "@PERSONAL_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_31")));
                AddParam(cmd, "@PERSONAL_inter_31", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_31")));

                // ======= 34 MONTHS =======
                AddParam(cmd, "@comm_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_34")));
                AddParam(cmd, "@inter_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_34")));
                AddParam(cmd, "@GROSS_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_34")));
                AddParam(cmd, "@GROSS_inter_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_34")));
                AddParam(cmd, "@FINE_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_34")));
                AddParam(cmd, "@FINE_inter_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_34")));
                AddParam(cmd, "@PROBLEM_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_34")));
                AddParam(cmd, "@PROBLEM_inter_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_34")));
                AddParam(cmd, "@PERSONAL_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_34")));
                AddParam(cmd, "@PERSONAL_inter_34", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_34")));

                // ======= 42 MONTHS =======
                AddParam(cmd, "@comm_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_42")));
                AddParam(cmd, "@inter_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_42")));
                AddParam(cmd, "@GROSS_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_42")));
                AddParam(cmd, "@GROSS_inter_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_42")));
                AddParam(cmd, "@FINE_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_42")));
                AddParam(cmd, "@FINE_inter_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_42")));
                AddParam(cmd, "@PROBLEM_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_42")));
                AddParam(cmd, "@PROBLEM_inter_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_42")));
                AddParam(cmd, "@PERSONAL_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_42")));
                AddParam(cmd, "@PERSONAL_inter_42", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_42")));

                // ======= 45 MONTHS =======
                AddParam(cmd, "@comm_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_45")));
                AddParam(cmd, "@inter_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_45")));
                AddParam(cmd, "@GROSS_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_45")));
                AddParam(cmd, "@GROSS_inter_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_45")));
                AddParam(cmd, "@FINE_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_45")));
                AddParam(cmd, "@FINE_inter_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_45")));
                AddParam(cmd, "@PROBLEM_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_45")));
                AddParam(cmd, "@PROBLEM_inter_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_45")));
                AddParam(cmd, "@PERSONAL_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_45")));
                AddParam(cmd, "@PERSONAL_inter_45", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_45")));

                // ======= 51 MONTHS =======
                AddParam(cmd, "@comm_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_51")));
                AddParam(cmd, "@inter_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_51")));
                AddParam(cmd, "@GROSS_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_51")));
                AddParam(cmd, "@GROSS_inter_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_51")));
                AddParam(cmd, "@FINE_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_51")));
                AddParam(cmd, "@FINE_inter_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_51")));
                AddParam(cmd, "@PROBLEM_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_51")));
                AddParam(cmd, "@PROBLEM_inter_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_51")));
                AddParam(cmd, "@PERSONAL_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_51")));
                AddParam(cmd, "@PERSONAL_inter_51", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_51")));

                // ======= 60 MONTHS =======
                AddParam(cmd, "@comm_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "comm_60")));
                AddParam(cmd, "@inter_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "inter_60")));
                AddParam(cmd, "@GROSS_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_60")));
                AddParam(cmd, "@GROSS_inter_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "GROSS_inter_60")));
                AddParam(cmd, "@FINE_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_60")));
                AddParam(cmd, "@FINE_inter_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "FINE_inter_60")));
                AddParam(cmd, "@PROBLEM_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_60")));
                AddParam(cmd, "@PROBLEM_inter_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PROBLEM_inter_60")));
                AddParam(cmd, "@PERSONAL_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_60")));
                AddParam(cmd, "@PERSONAL_inter_60", SqlDbType.NVarChar, DbNullIfEmpty(GetStr(context, "PERSONAL_inter_60")));

                AddParam(cmd, "@MONTHS", SqlDbType.NVarChar, DbNullIfEmpty(month));
                AddParam(cmd, "@questions", SqlDbType.NVarChar, DbNullIfEmpty(questions));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabValue);

                // ✅ OUTPUT param correct
                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return Convert.ToInt32(ret.Value);
            }
        }
        private int SaveTab15(HttpContext context, int appointmentID)
        {
            int tabNo = 15;

            string General_Processing = GetStr(context, "General_Processing");
            string AUDITORY_Processing = GetStr(context, "AUDITORY_Processing");
            string VISUAL_Processing = GetStr(context, "VISUAL_Processing");
            string TOUCH_Processing = GetStr(context, "TOUCH_Processing");
            string MOVEMENT_Processing = GetStr(context, "MOVEMENT_Processing");
            string ORAL_Processing = GetStr(context, "ORAL_Processing");
            string Raw_score = GetStr(context, "Raw_score");

            string Total_rawscore = GetStr(context, "Total_rawscore");
            string Interpretation = GetStr(context, "Interpretation");
            string Comments_1 = GetStr(context, "Comments_1");

            string Score_seeking = GetStr(context, "Score_seeking");
            string SEEKING = GetStr(context, "SEEKING");

            string Score_Avoiding = GetStr(context, "Score_Avoiding");
            string AVOIDING = GetStr(context, "AVOIDING");

            string Score_sensitivity = GetStr(context, "Score_sensitivity");
            string SENSITIVITY_2 = GetStr(context, "SENSITIVITY_2");

            string Score_Registration = GetStr(context, "Score_Registration");
            string REGISTRATION = GetStr(context, "REGISTRATION");

            string Score_general = GetStr(context, "Score_general");
            string GENERAL = GetStr(context, "GENERAL");

            string Score_Auditory = GetStr(context, "Score_Auditory");
            string AUDITORY = GetStr(context, "AUDITORY");

            string Score_visual = GetStr(context, "Score_visual");
            string VISUAL = GetStr(context, "VISUAL");

            string Score_touch = GetStr(context, "Score_touch");
            string TOUCH = GetStr(context, "TOUCH");

            string Score_movement = GetStr(context, "Score_movement");
            string MOVEMENT = GetStr(context, "MOVEMENT");

            string Score_oral = GetStr(context, "Score_oral");
            string ORAL = GetStr(context, "ORAL");

            string Score_behavioural = GetStr(context, "Score_behavioural");
            string BEHAVIORAL = GetStr(context, "BEHAVIORAL");

            string Comments_2 = GetStr(context, "Comments_2");

            string SPchild_Seeker = GetStr(context, "SPchild_Seeker");
            string Seeking_Seeker = GetStr(context, "Seeking_Seeker");

            string SPchild_Avoider = GetStr(context, "SPchild_Avoider");
            string Avoiding_Avoider = GetStr(context, "Avoiding_Avoider");

            string SPchild_Sensor = GetStr(context, "SPchild_Sensor");
            string Sensitivity_Sensor = GetStr(context, "Sensitivity_Sensor");

            string SPchild_Bystander = GetStr(context, "SPchild_Bystander");
            string Registration_Bystander = GetStr(context, "Registration_Bystander");

            string SPchild_Auditory_3 = GetStr(context, "SPchild_Auditory_3");
            string Auditory_3 = GetStr(context, "Auditory_3");

            string SPchild_Visual_3 = GetStr(context, "SPchild_Visual_3");
            string Visual_3 = GetStr(context, "Visual_3");

            string SPchild_Touch_3 = GetStr(context, "SPchild_Touch_3");
            string Touch_3 = GetStr(context, "Touch_3");

            string SPchild_Movement_3 = GetStr(context, "SPchild_Movement_3");
            string Movement_3 = GetStr(context, "Movement_3");

            string SPchild_Body_position = GetStr(context, "SPchild_Body_position");
            string Body_position = GetStr(context, "Body_position");

            string SPchild_Oral_3 = GetStr(context, "SPchild_Oral_3");
            string Oral_3 = GetStr(context, "Oral_3");

            string SPchild_Conduct_3 = GetStr(context, "SPchild_Conduct_3");
            string Conduct_3 = GetStr(context, "Conduct_3");

            string SPchild_Social_emotional = GetStr(context, "SPchild_Social_emotional");
            string Social_emotional = GetStr(context, "Social_emotional");

            string SPchild_Attentional_3 = GetStr(context, "SPchild_Attentional_3");
            string Attentional_3 = GetStr(context, "Attentional_3");

            string Comments_3 = GetStr(context, "Comments_3");

            string SPAdult_Low_Registration = GetStr(context, "SPAdult_Low_Registration");
            string Low_Registration = GetStr(context, "Low_Registration");

            string SPAdult_Sensory_seeking = GetStr(context, "SPAdult_Sensory_seeking");
            string Sensory_seeking = GetStr(context, "Sensory_seeking");

            string SPAdult_Sensory_Sensitivity = GetStr(context, "SPAdult_Sensory_Sensitivity");
            string Sensory_Sensitivity = GetStr(context, "Sensory_Sensitivity");

            string SPAdult_Sensory_Avoiding = GetStr(context, "SPAdult_Sensory_Avoiding");
            string Sensory_Avoiding = GetStr(context, "Sensory_Avoiding");

            string Comments_4 = GetStr(context, "Comments_4");

            string SP_Low_Registration64 = GetStr(context, "SP_Low_Registration64");
            string Low_Registration_5 = GetStr(context, "Low_Registration_5");

            string SP_Sensory_seeking_64 = GetStr(context, "SP_Sensory_seeking_64");
            string Sensory_seeking_5 = GetStr(context, "Sensory_seeking_5");

            string SP_Sensory_Sensitivity64 = GetStr(context, "SP_Sensory_Sensitivity64");
            string Sensory_Sensitivity_5 = GetStr(context, "Sensory_Sensitivity_5");

            string SP_Sensory_Avoiding64 = GetStr(context, "SP_Sensory_Avoiding64");
            string Sensory_Avoiding_5 = GetStr(context, "Sensory_Avoiding_5");

            string Comments_5 = GetStr(context, "Comments_5");

            string Older_Low_Registration = GetStr(context, "Older_Low_Registration");
            string Low_Registration_6 = GetStr(context, "Low_Registration_6");

            string Older_Sensory_seeking = GetStr(context, "Older_Sensory_seeking");
            string Sensory_seeking_6 = GetStr(context, "Sensory_seeking_6");

            string Older_Sensory_Sensitivity = GetStr(context, "Older_Sensory_Sensitivity");
            string Sensory_Sensitivity_6 = GetStr(context, "Sensory_Sensitivity_6");

            string Older_Sensory_Avoiding = GetStr(context, "Older_Sensory_Avoiding");
            string Sensory_Avoiding_6 = GetStr(context, "Sensory_Avoiding_6");

            string Comments_6 = GetStr(context, "Comments_6");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@General_Processing", SqlDbType.NVarChar, DbNullIfEmpty(General_Processing));
                AddParam(cmd, "@AUDITORY_Processing", SqlDbType.NVarChar, DbNullIfEmpty(AUDITORY_Processing));
                AddParam(cmd, "@VISUAL_Processing", SqlDbType.NVarChar, DbNullIfEmpty(VISUAL_Processing));
                AddParam(cmd, "@TOUCH_Processing", SqlDbType.NVarChar, DbNullIfEmpty(TOUCH_Processing));
                AddParam(cmd, "@MOVEMENT_Processing", SqlDbType.NVarChar, DbNullIfEmpty(MOVEMENT_Processing));
                AddParam(cmd, "@ORAL_Processing", SqlDbType.NVarChar, DbNullIfEmpty(ORAL_Processing));

                AddParam(cmd, "@Raw_score", SqlDbType.NVarChar, DbNullIfEmpty(Raw_score));
                AddParam(cmd, "@Total_rawscore", SqlDbType.NVarChar, DbNullIfEmpty(Total_rawscore));
                AddParam(cmd, "@Interpretation", SqlDbType.NVarChar, DbNullIfEmpty(Interpretation));
                AddParam(cmd, "@Comments_1", SqlDbType.NVarChar, DbNullIfEmpty(Comments_1));

                AddParam(cmd, "@Score_seeking", SqlDbType.NVarChar, DbNullIfEmpty(Score_seeking));
                AddParam(cmd, "@SEEKING", SqlDbType.NVarChar, DbNullIfEmpty(SEEKING));

                AddParam(cmd, "@Score_Avoiding", SqlDbType.NVarChar, DbNullIfEmpty(Score_Avoiding));
                AddParam(cmd, "@AVOIDING", SqlDbType.NVarChar, DbNullIfEmpty(AVOIDING));

                AddParam(cmd, "@Score_sensitivity", SqlDbType.NVarChar, DbNullIfEmpty(Score_sensitivity));
                AddParam(cmd, "@SENSITIVITY_2", SqlDbType.NVarChar, DbNullIfEmpty(SENSITIVITY_2));

                AddParam(cmd, "@Score_Registration", SqlDbType.NVarChar, DbNullIfEmpty(Score_Registration));
                AddParam(cmd, "@REGISTRATION", SqlDbType.NVarChar, DbNullIfEmpty(REGISTRATION));

                AddParam(cmd, "@Score_general", SqlDbType.NVarChar, DbNullIfEmpty(Score_general));
                AddParam(cmd, "@GENERAL", SqlDbType.NVarChar, DbNullIfEmpty(GENERAL));

                AddParam(cmd, "@Score_Auditory", SqlDbType.NVarChar, DbNullIfEmpty(Score_Auditory));
                AddParam(cmd, "@AUDITORY", SqlDbType.NVarChar, DbNullIfEmpty(AUDITORY));

                AddParam(cmd, "@Score_visual", SqlDbType.NVarChar, DbNullIfEmpty(Score_visual));
                AddParam(cmd, "@VISUAL", SqlDbType.NVarChar, DbNullIfEmpty(VISUAL));

                AddParam(cmd, "@Score_touch", SqlDbType.NVarChar, DbNullIfEmpty(Score_touch));
                AddParam(cmd, "@TOUCH", SqlDbType.NVarChar, DbNullIfEmpty(TOUCH));

                AddParam(cmd, "@Score_movement", SqlDbType.NVarChar, DbNullIfEmpty(Score_movement));
                AddParam(cmd, "@MOVEMENT", SqlDbType.NVarChar, DbNullIfEmpty(MOVEMENT));

                AddParam(cmd, "@Score_oral", SqlDbType.NVarChar, DbNullIfEmpty(Score_oral));
                AddParam(cmd, "@ORAL", SqlDbType.NVarChar, DbNullIfEmpty(ORAL));

                AddParam(cmd, "@Score_behavioural", SqlDbType.NVarChar, DbNullIfEmpty(Score_behavioural));
                AddParam(cmd, "@BEHAVIORAL", SqlDbType.NVarChar, DbNullIfEmpty(BEHAVIORAL));

                AddParam(cmd, "@Comments_2", SqlDbType.NVarChar, DbNullIfEmpty(Comments_2));

                AddParam(cmd, "@SPchild_Seeker", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Seeker));
                AddParam(cmd, "@Seeking_Seeker", SqlDbType.NVarChar, DbNullIfEmpty(Seeking_Seeker));

                AddParam(cmd, "@SPchild_Avoider", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Avoider));
                AddParam(cmd, "@Avoiding_Avoider", SqlDbType.NVarChar, DbNullIfEmpty(Avoiding_Avoider));

                AddParam(cmd, "@SPchild_Sensor", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Sensor));
                AddParam(cmd, "@Sensitivity_Sensor", SqlDbType.NVarChar, DbNullIfEmpty(Sensitivity_Sensor));

                AddParam(cmd, "@SPchild_Bystander", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Bystander));
                AddParam(cmd, "@Registration_Bystander", SqlDbType.NVarChar, DbNullIfEmpty(Registration_Bystander));

                AddParam(cmd, "@SPchild_Auditory_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Auditory_3));
                AddParam(cmd, "@Auditory_3", SqlDbType.NVarChar, DbNullIfEmpty(Auditory_3));

                AddParam(cmd, "@SPchild_Visual_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Visual_3));
                AddParam(cmd, "@Visual_3", SqlDbType.NVarChar, DbNullIfEmpty(Visual_3));

                AddParam(cmd, "@SPchild_Touch_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Touch_3));
                AddParam(cmd, "@Touch_3", SqlDbType.NVarChar, DbNullIfEmpty(Touch_3));

                AddParam(cmd, "@SPchild_Movement_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Movement_3));
                AddParam(cmd, "@Movement_3", SqlDbType.NVarChar, DbNullIfEmpty(Movement_3));

                AddParam(cmd, "@SPchild_Body_position", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Body_position));
                AddParam(cmd, "@Body_position", SqlDbType.NVarChar, DbNullIfEmpty(Body_position));

                AddParam(cmd, "@SPchild_Oral_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Oral_3));
                AddParam(cmd, "@Oral_3", SqlDbType.NVarChar, DbNullIfEmpty(Oral_3));

                AddParam(cmd, "@SPchild_Conduct_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Conduct_3));
                AddParam(cmd, "@Conduct_3", SqlDbType.NVarChar, DbNullIfEmpty(Conduct_3));

                AddParam(cmd, "@SPchild_Social_emotional", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Social_emotional));
                AddParam(cmd, "@Social_emotional", SqlDbType.NVarChar, DbNullIfEmpty(Social_emotional));

                AddParam(cmd, "@SPchild_Attentional_3", SqlDbType.NVarChar, DbNullIfEmpty(SPchild_Attentional_3));
                AddParam(cmd, "@Attentional_3", SqlDbType.NVarChar, DbNullIfEmpty(Attentional_3));

                AddParam(cmd, "@Comments_3", SqlDbType.NVarChar, DbNullIfEmpty(Comments_3));

                AddParam(cmd, "@SPAdult_Low_Registration", SqlDbType.NVarChar, DbNullIfEmpty(SPAdult_Low_Registration));
                AddParam(cmd, "@Low_Registration", SqlDbType.NVarChar, DbNullIfEmpty(Low_Registration));

                AddParam(cmd, "@SPAdult_Sensory_seeking", SqlDbType.NVarChar, DbNullIfEmpty(SPAdult_Sensory_seeking));
                AddParam(cmd, "@Sensory_seeking", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_seeking));

                AddParam(cmd, "@SPAdult_Sensory_Sensitivity", SqlDbType.NVarChar, DbNullIfEmpty(SPAdult_Sensory_Sensitivity));
                AddParam(cmd, "@Sensory_Sensitivity", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Sensitivity));

                AddParam(cmd, "@SPAdult_Sensory_Avoiding", SqlDbType.NVarChar, DbNullIfEmpty(SPAdult_Sensory_Avoiding));
                AddParam(cmd, "@Sensory_Avoiding", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Avoiding));

                AddParam(cmd, "@Comments_4", SqlDbType.NVarChar, DbNullIfEmpty(Comments_4));

                AddParam(cmd, "@SP_Low_Registration64", SqlDbType.NVarChar, DbNullIfEmpty(SP_Low_Registration64));
                AddParam(cmd, "@Low_Registration_5", SqlDbType.NVarChar, DbNullIfEmpty(Low_Registration_5));

                AddParam(cmd, "@SP_Sensory_seeking_64", SqlDbType.NVarChar, DbNullIfEmpty(SP_Sensory_seeking_64));
                AddParam(cmd, "@Sensory_seeking_5", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_seeking_5));

                AddParam(cmd, "@SP_Sensory_Sensitivity64", SqlDbType.NVarChar, DbNullIfEmpty(SP_Sensory_Sensitivity64));
                AddParam(cmd, "@Sensory_Sensitivity_5", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Sensitivity_5));

                AddParam(cmd, "@SP_Sensory_Avoiding64", SqlDbType.NVarChar, DbNullIfEmpty(SP_Sensory_Avoiding64));
                AddParam(cmd, "@Sensory_Avoiding_5", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Avoiding_5));

                AddParam(cmd, "@Comments_5", SqlDbType.NVarChar, DbNullIfEmpty(Comments_5));

                AddParam(cmd, "@Older_Low_Registration", SqlDbType.NVarChar, DbNullIfEmpty(Older_Low_Registration));
                AddParam(cmd, "@Low_Registration_6", SqlDbType.NVarChar, DbNullIfEmpty(Low_Registration_6));

                AddParam(cmd, "@Older_Sensory_seeking", SqlDbType.NVarChar, DbNullIfEmpty(Older_Sensory_seeking));
                AddParam(cmd, "@Sensory_seeking_6", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_seeking_6));

                AddParam(cmd, "@Older_Sensory_Sensitivity", SqlDbType.NVarChar, DbNullIfEmpty(Older_Sensory_Sensitivity));
                AddParam(cmd, "@Sensory_Sensitivity_6", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Sensitivity_6));

                AddParam(cmd, "@Older_Sensory_Avoiding", SqlDbType.NVarChar, DbNullIfEmpty(Older_Sensory_Avoiding));
                AddParam(cmd, "@Sensory_Avoiding_6", SqlDbType.NVarChar, DbNullIfEmpty(Sensory_Avoiding_6));

                AddParam(cmd, "@Comments_6", SqlDbType.NVarChar, DbNullIfEmpty(Comments_6));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab16(HttpContext context, int appointmentID)
        {
            int tabNo = 16;

            string ABILITY_months = GetStr(context, "ABILITY_months");
            string ability_TOTAL = GetStr(context, "ability_TOTAL");
            string ability_COMMENTS = GetStr(context, "ability_COMMENTS");
            string ABILITY_questions = GetStr(context, "ABILITY_questions");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@ABILITY_months", SqlDbType.NVarChar, DbNullIfEmpty(ABILITY_months));
                AddParam(cmd, "@ability_TOTAL", SqlDbType.NVarChar, DbNullIfEmpty(ability_TOTAL));
                AddParam(cmd, "@ability_COMMENTS", SqlDbType.NVarChar, DbNullIfEmpty(ability_COMMENTS));
                AddParam(cmd, "@ABILITY_questions", SqlDbType.NVarChar, DbNullIfEmpty(ABILITY_questions));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab17(HttpContext context, int appointmentID)
        {
            int tabNo = ToInt(GetStr(context, "TabNo"));

            string DCDQ_Throws1 = GetStr(context, "DCDQ_Throws1");
            string DCDQ_Throws2 = GetStr(context, "DCDQ_Throws2");
            string DCDQ_Throws3 = GetStr(context, "DCDQ_Throws3");

            string DCDQ_Catches1 = GetStr(context, "DCDQ_Catches1");
            string DCDQ_Catches2 = GetStr(context, "DCDQ_Catches2");
            string DCDQ_Catches3 = GetStr(context, "DCDQ_Catches3");

            string DCDQ_Hits1 = GetStr(context, "DCDQ_Hits1");
            string DCDQ_Hits2 = GetStr(context, "DCDQ_Hits2");
            string DCDQ_Hits3 = GetStr(context, "DCDQ_Hits3");

            string DCDQ_Jumps1 = GetStr(context, "DCDQ_Jumps1");
            string DCDQ_Jumps2 = GetStr(context, "DCDQ_Jumps2");
            string DCDQ_Jumps3 = GetStr(context, "DCDQ_Jumps3");

            string DCDQ_Runs1 = GetStr(context, "DCDQ_Runs1");
            string DCDQ_Runs2 = GetStr(context, "DCDQ_Runs2");
            string DCDQ_Runs3 = GetStr(context, "DCDQ_Runs3");

            string DCDQ_Plans1 = GetStr(context, "DCDQ_Plans1");
            string DCDQ_Plans2 = GetStr(context, "DCDQ_Plans2");
            string DCDQ_Plans3 = GetStr(context, "DCDQ_Plans3");

            string DCDQ_Writing1 = GetStr(context, "DCDQ_Writing1");
            string DCDQ_Writing2 = GetStr(context, "DCDQ_Writing2");
            string DCDQ_Writing3 = GetStr(context, "DCDQ_Writing3");

            string DCDQ_legibly1 = GetStr(context, "DCDQ_legibly1");
            string DCDQ_legibly2 = GetStr(context, "DCDQ_legibly2");
            string DCDQ_legibly3 = GetStr(context, "DCDQ_legibly3");

            string DCDQ_Effort1 = GetStr(context, "DCDQ_Effort1");
            string DCDQ_Effort2 = GetStr(context, "DCDQ_Effort2");
            string DCDQ_Effort3 = GetStr(context, "DCDQ_Effort3");

            string DCDQ_Cuts1 = GetStr(context, "DCDQ_Cuts1");
            string DCDQ_Cuts2 = GetStr(context, "DCDQ_Cuts2");
            string DCDQ_Cuts3 = GetStr(context, "DCDQ_Cuts3");

            string DCDQ_Likes1 = GetStr(context, "DCDQ_Likes1");
            string DCDQ_Likes2 = GetStr(context, "DCDQ_Likes2");
            string DCDQ_Likes3 = GetStr(context, "DCDQ_Likes3");

            string DCDQ_Learning1 = GetStr(context, "DCDQ_Learning1");
            string DCDQ_Learning2 = GetStr(context, "DCDQ_Learning2");
            string DCDQ_Learning3 = GetStr(context, "DCDQ_Learning3");

            string DCDQ_Quick1 = GetStr(context, "DCDQ_Quick1");
            string DCDQ_Quick2 = GetStr(context, "DCDQ_Quick2");
            string DCDQ_Quick3 = GetStr(context, "DCDQ_Quick3");

            string DCDQ_Bull1 = GetStr(context, "DCDQ_Bull1");
            string DCDQ_Bull2 = GetStr(context, "DCDQ_Bull2");
            string DCDQ_Bull3 = GetStr(context, "DCDQ_Bull3");

            string DCDQ_Does1 = GetStr(context, "DCDQ_Does1");
            string DCDQ_Does2 = GetStr(context, "DCDQ_Does2");
            string DCDQ_Does3 = GetStr(context, "DCDQ_Does3");

            string DCDQ_Control = GetStr(context, "DCDQ_Control");
            string DCDQ_Fine = GetStr(context, "DCDQ_Fine");
            string DCDQ_General = GetStr(context, "DCDQ_General");
            string DCDQ_Total = GetStr(context, "DCDQ_Total");

            string DCDQ_INTERPRETATION = GetStr(context, "DCDQ_INTERPRETATION");
            string DCDQ_COMMENT = GetStr(context, "DCDQ_COMMENT");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@DCDQ_Throws1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Throws1));
                AddParam(cmd, "@DCDQ_Throws2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Throws2));
                AddParam(cmd, "@DCDQ_Throws3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Throws3));

                AddParam(cmd, "@DCDQ_Catches1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Catches1));
                AddParam(cmd, "@DCDQ_Catches2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Catches2));
                AddParam(cmd, "@DCDQ_Catches3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Catches3));

                AddParam(cmd, "@DCDQ_Hits1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Hits1));
                AddParam(cmd, "@DCDQ_Hits2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Hits2));
                AddParam(cmd, "@DCDQ_Hits3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Hits3));

                AddParam(cmd, "@DCDQ_Jumps1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Jumps1));
                AddParam(cmd, "@DCDQ_Jumps2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Jumps2));
                AddParam(cmd, "@DCDQ_Jumps3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Jumps3));

                AddParam(cmd, "@DCDQ_Runs1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Runs1));
                AddParam(cmd, "@DCDQ_Runs2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Runs2));
                AddParam(cmd, "@DCDQ_Runs3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Runs3));

                AddParam(cmd, "@DCDQ_Plans1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Plans1));
                AddParam(cmd, "@DCDQ_Plans2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Plans2));
                AddParam(cmd, "@DCDQ_Plans3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Plans3));

                AddParam(cmd, "@DCDQ_Writing1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Writing1));
                AddParam(cmd, "@DCDQ_Writing2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Writing2));
                AddParam(cmd, "@DCDQ_Writing3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Writing3));

                AddParam(cmd, "@DCDQ_legibly1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_legibly1));
                AddParam(cmd, "@DCDQ_legibly2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_legibly2));
                AddParam(cmd, "@DCDQ_legibly3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_legibly3));

                AddParam(cmd, "@DCDQ_Effort1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Effort1));
                AddParam(cmd, "@DCDQ_Effort2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Effort2));
                AddParam(cmd, "@DCDQ_Effort3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Effort3));

                AddParam(cmd, "@DCDQ_Cuts1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Cuts1));
                AddParam(cmd, "@DCDQ_Cuts2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Cuts2));
                AddParam(cmd, "@DCDQ_Cuts3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Cuts3));

                AddParam(cmd, "@DCDQ_Likes1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Likes1));
                AddParam(cmd, "@DCDQ_Likes2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Likes2));
                AddParam(cmd, "@DCDQ_Likes3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Likes3));

                AddParam(cmd, "@DCDQ_Learning1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Learning1));
                AddParam(cmd, "@DCDQ_Learning2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Learning2));
                AddParam(cmd, "@DCDQ_Learning3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Learning3));

                AddParam(cmd, "@DCDQ_Quick1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Quick1));
                AddParam(cmd, "@DCDQ_Quick2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Quick2));
                AddParam(cmd, "@DCDQ_Quick3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Quick3));

                AddParam(cmd, "@DCDQ_Bull1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Bull1));
                AddParam(cmd, "@DCDQ_Bull2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Bull2));
                AddParam(cmd, "@DCDQ_Bull3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Bull3));

                AddParam(cmd, "@DCDQ_Does1", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Does1));
                AddParam(cmd, "@DCDQ_Does2", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Does2));
                AddParam(cmd, "@DCDQ_Does3", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Does3));

                AddParam(cmd, "@DCDQ_Control", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Control));
                AddParam(cmd, "@DCDQ_Fine", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Fine));
                AddParam(cmd, "@DCDQ_General", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_General));
                AddParam(cmd, "@DCDQ_Total", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_Total));

                AddParam(cmd, "@DCDQ_INTERPRETATION", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_INTERPRETATION));
                AddParam(cmd, "@DCDQ_COMMENT", SqlDbType.NVarChar, DbNullIfEmpty(DCDQ_COMMENT));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab18(HttpContext context, int appointmentID)
        {
            int tabNo = 18;

            string SIPTInfo_History = GetStr(context, "SIPTInfo_History");

            string SIPTInfo_HandFunction1_GraspRight = GetStr(context, "SIPTInfo_HandFunction1_GraspRight");
            string SIPTInfo_HandFunction1_GraspLeft = GetStr(context, "SIPTInfo_HandFunction1_GraspLeft");
            string SIPTInfo_HandFunction1_SphericalRight = GetStr(context, "SIPTInfo_HandFunction1_SphericalRight");
            string SIPTInfo_HandFunction1_SphericalLeft = GetStr(context, "SIPTInfo_HandFunction1_SphericalLeft");
            string SIPTInfo_HandFunction1_HookRight = GetStr(context, "SIPTInfo_HandFunction1_HookRight");
            string SIPTInfo_HandFunction1_HookLeft = GetStr(context, "SIPTInfo_HandFunction1_HookLeft");
            string SIPTInfo_HandFunction1_JawChuckRight = GetStr(context, "SIPTInfo_HandFunction1_JawChuckRight");
            string SIPTInfo_HandFunction1_JawChuckLeft = GetStr(context, "SIPTInfo_HandFunction1_JawChuckLeft");
            string SIPTInfo_HandFunction1_GripRight = GetStr(context, "SIPTInfo_HandFunction1_GripRight");
            string SIPTInfo_HandFunction1_GripLeft = GetStr(context, "SIPTInfo_HandFunction1_GripLeft");
            string SIPTInfo_HandFunction1_ReleaseRight = GetStr(context, "SIPTInfo_HandFunction1_ReleaseRight");
            string SIPTInfo_HandFunction1_ReleaseLeft = GetStr(context, "SIPTInfo_HandFunction1_ReleaseLeft");

            string SIPTInfo_HandFunction2_OppositionLfR = GetStr(context, "SIPTInfo_HandFunction2_OppositionLfR");
            string SIPTInfo_HandFunction2_OppositionLfL = GetStr(context, "SIPTInfo_HandFunction2_OppositionLfL");
            string SIPTInfo_HandFunction2_OppositionMFR = GetStr(context, "SIPTInfo_HandFunction2_OppositionMFR");
            string SIPTInfo_HandFunction2_OppositionMFL = GetStr(context, "SIPTInfo_HandFunction2_OppositionMFL");
            string SIPTInfo_HandFunction2_OppositionRFR = GetStr(context, "SIPTInfo_HandFunction2_OppositionRFR");
            string SIPTInfo_HandFunction2_OppositionRFL = GetStr(context, "SIPTInfo_HandFunction2_OppositionRFL");
            string SIPTInfo_HandFunction2_PinchLfR = GetStr(context, "SIPTInfo_HandFunction2_PinchLfR");
            string SIPTInfo_HandFunction2_PinchLfL = GetStr(context, "SIPTInfo_HandFunction2_PinchLfL");
            string SIPTInfo_HandFunction2_PinchMFR = GetStr(context, "SIPTInfo_HandFunction2_PinchMFR");
            string SIPTInfo_HandFunction2_PinchMFL = GetStr(context, "SIPTInfo_HandFunction2_PinchMFL");
            string SIPTInfo_HandFunction2_PinchRFR = GetStr(context, "SIPTInfo_HandFunction2_PinchRFR");
            string SIPTInfo_HandFunction2_PinchRFL = GetStr(context, "SIPTInfo_HandFunction2_PinchRFL");

            string SIPTInfo_SIPT3_Spontaneous = GetStr(context, "SIPTInfo_SIPT3_Spontaneous");
            string SIPTInfo_SIPT3_Command = GetStr(context, "SIPTInfo_SIPT3_Command");

            string SIPTInfo_SIPT4_Kinesthesia = GetStr(context, "SIPTInfo_SIPT4_Kinesthesia");
            string SIPTInfo_SIPT4_Finger = GetStr(context, "SIPTInfo_SIPT4_Finger");
            string SIPTInfo_SIPT4_Localisation = GetStr(context, "SIPTInfo_SIPT4_Localisation");
            string SIPTInfo_SIPT4_DoubleTactile = GetStr(context, "SIPTInfo_SIPT4_DoubleTactile");
            string SIPTInfo_SIPT4_Tactile = GetStr(context, "SIPTInfo_SIPT4_Tactile");
            string SIPTInfo_SIPT4_Graphesthesia = GetStr(context, "SIPTInfo_SIPT4_Graphesthesia");
            string SIPTInfo_SIPT4_PostRotary = GetStr(context, "SIPTInfo_SIPT4_PostRotary");
            string SIPTInfo_SIPT4_Standing = GetStr(context, "SIPTInfo_SIPT4_Standing");

            string SIPTInfo_SIPT5_Color = GetStr(context, "SIPTInfo_SIPT5_Color");
            string SIPTInfo_SIPT5_Form = GetStr(context, "SIPTInfo_SIPT5_Form");
            string SIPTInfo_SIPT5_Size = GetStr(context, "SIPTInfo_SIPT5_Size");
            string SIPTInfo_SIPT5_Depth = GetStr(context, "SIPTInfo_SIPT5_Depth");
            string SIPTInfo_SIPT5_Figure = GetStr(context, "SIPTInfo_SIPT5_Figure");
            string SIPTInfo_SIPT5_Motor = GetStr(context, "SIPTInfo_SIPT5_Motor");

            string SIPTInfo_SIPT6_Design = GetStr(context, "SIPTInfo_SIPT6_Design");
            string SIPTInfo_SIPT6_Constructional = GetStr(context, "SIPTInfo_SIPT6_Constructional");

            string SIPTInfo_SIPT7_Scanning = GetStr(context, "SIPTInfo_SIPT7_Scanning");
            string SIPTInfo_SIPT7_Memory = GetStr(context, "SIPTInfo_SIPT7_Memory");

            string SIPTInfo_SIPT8_Postural = GetStr(context, "SIPTInfo_SIPT8_Postural");
            string SIPTInfo_SIPT8_Oral = GetStr(context, "SIPTInfo_SIPT8_Oral");
            string SIPTInfo_SIPT8_Sequencing = GetStr(context, "SIPTInfo_SIPT8_Sequencing");
            string SIPTInfo_SIPT8_Commands = GetStr(context, "SIPTInfo_SIPT8_Commands");

            string SIPTInfo_SIPT9_Bilateral = GetStr(context, "SIPTInfo_SIPT9_Bilateral");
            string SIPTInfo_SIPT9_Contralat = GetStr(context, "SIPTInfo_SIPT9_Contralat");
            string SIPTInfo_SIPT9_PreferredHand = GetStr(context, "SIPTInfo_SIPT9_PreferredHand");
            string SIPTInfo_SIPT9_CrossingMidline = GetStr(context, "SIPTInfo_SIPT9_CrossingMidline");

            string SIPTInfo_SIPT10_Draw = GetStr(context, "SIPTInfo_SIPT10_Draw");
            string SIPTInfo_SIPT10_ClockFace = GetStr(context, "SIPTInfo_SIPT10_ClockFace");
            string SIPTInfo_SIPT10_Filtering = GetStr(context, "SIPTInfo_SIPT10_Filtering");
            string SIPTInfo_SIPT10_MotorPlanning = GetStr(context, "SIPTInfo_SIPT10_MotorPlanning");
            string SIPTInfo_SIPT10_BodyImage = GetStr(context, "SIPTInfo_SIPT10_BodyImage");
            string SIPTInfo_SIPT10_BodySchema = GetStr(context, "SIPTInfo_SIPT10_BodySchema");
            string SIPTInfo_SIPT10_Laterality = GetStr(context, "SIPTInfo_SIPT10_Laterality");

            string SIPTInfo_ActivityGiven_Remark = GetStr(context, "SIPTInfo_ActivityGiven_Remark");
            string SIPTInfo_ActivityGiven_InterestActivity = GetStr(context, "SIPTInfo_ActivityGiven_InterestActivity");
            string SIPTInfo_ActivityGiven_InterestCompletion = GetStr(context, "SIPTInfo_ActivityGiven_InterestCompletion");
            string SIPTInfo_ActivityGiven_Learning = GetStr(context, "SIPTInfo_ActivityGiven_Learning");
            string SIPTInfo_ActivityGiven_Complexity = GetStr(context, "SIPTInfo_ActivityGiven_Complexity");
            string SIPTInfo_ActivityGiven_ProblemSolving = GetStr(context, "SIPTInfo_ActivityGiven_ProblemSolving");
            string SIPTInfo_ActivityGiven_Concentration = GetStr(context, "SIPTInfo_ActivityGiven_Concentration");
            string SIPTInfo_ActivityGiven_Retension = GetStr(context, "SIPTInfo_ActivityGiven_Retension");
            string SIPTInfo_ActivityGiven_SpeedPerfom = GetStr(context, "SIPTInfo_ActivityGiven_SpeedPerfom");
            string SIPTInfo_ActivityGiven_Neatness = GetStr(context, "SIPTInfo_ActivityGiven_Neatness");
            string SIPTInfo_ActivityGiven_Frustation = GetStr(context, "SIPTInfo_ActivityGiven_Frustation");
            string SIPTInfo_ActivityGiven_Work = GetStr(context, "SIPTInfo_ActivityGiven_Work");
            string SIPTInfo_ActivityGiven_Reaction = GetStr(context, "SIPTInfo_ActivityGiven_Reaction");
            string SIPTInfo_ActivityGiven_SociabilityTherapist = GetStr(context, "SIPTInfo_ActivityGiven_SociabilityTherapist");
            string SIPTInfo_ActivityGiven_SociabilityStudents = GetStr(context, "SIPTInfo_ActivityGiven_SociabilityStudents");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@SIPTInfo_History", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_History));

                AddParam(cmd, "@SIPTInfo_HandFunction1_GraspRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_GraspRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_GraspLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_GraspLeft));
                AddParam(cmd, "@SIPTInfo_HandFunction1_SphericalRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_SphericalRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_SphericalLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_SphericalLeft));
                AddParam(cmd, "@SIPTInfo_HandFunction1_HookRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_HookRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_HookLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_HookLeft));
                AddParam(cmd, "@SIPTInfo_HandFunction1_JawChuckRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_JawChuckRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_JawChuckLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_JawChuckLeft));
                AddParam(cmd, "@SIPTInfo_HandFunction1_GripRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_GripRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_GripLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_GripLeft));
                AddParam(cmd, "@SIPTInfo_HandFunction1_ReleaseRight", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_ReleaseRight));
                AddParam(cmd, "@SIPTInfo_HandFunction1_ReleaseLeft", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction1_ReleaseLeft));

                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionLfR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionLfR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionLfL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionLfL));
                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionMFR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionMFR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionMFL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionMFL));
                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionRFR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionRFR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_OppositionRFL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_OppositionRFL));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchLfR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchLfR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchLfL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchLfL));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchMFR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchMFR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchMFL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchMFL));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchRFR", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchRFR));
                AddParam(cmd, "@SIPTInfo_HandFunction2_PinchRFL", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_HandFunction2_PinchRFL));

                AddParam(cmd, "@SIPTInfo_SIPT3_Spontaneous", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT3_Spontaneous));
                AddParam(cmd, "@SIPTInfo_SIPT3_Command", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT3_Command));

                AddParam(cmd, "@SIPTInfo_SIPT4_Kinesthesia", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Kinesthesia));
                AddParam(cmd, "@SIPTInfo_SIPT4_Finger", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Finger));
                AddParam(cmd, "@SIPTInfo_SIPT4_Localisation", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Localisation));
                AddParam(cmd, "@SIPTInfo_SIPT4_DoubleTactile", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_DoubleTactile));
                AddParam(cmd, "@SIPTInfo_SIPT4_Tactile", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Tactile));
                AddParam(cmd, "@SIPTInfo_SIPT4_Graphesthesia", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Graphesthesia));
                AddParam(cmd, "@SIPTInfo_SIPT4_PostRotary", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_PostRotary));
                AddParam(cmd, "@SIPTInfo_SIPT4_Standing", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT4_Standing));

                AddParam(cmd, "@SIPTInfo_SIPT5_Color", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Color));
                AddParam(cmd, "@SIPTInfo_SIPT5_Form", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Form));
                AddParam(cmd, "@SIPTInfo_SIPT5_Size", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Size));
                AddParam(cmd, "@SIPTInfo_SIPT5_Depth", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Depth));
                AddParam(cmd, "@SIPTInfo_SIPT5_Figure", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Figure));
                AddParam(cmd, "@SIPTInfo_SIPT5_Motor", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT5_Motor));

                AddParam(cmd, "@SIPTInfo_SIPT6_Design", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT6_Design));
                AddParam(cmd, "@SIPTInfo_SIPT6_Constructional", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT6_Constructional));

                AddParam(cmd, "@SIPTInfo_SIPT7_Scanning", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT7_Scanning));
                AddParam(cmd, "@SIPTInfo_SIPT7_Memory", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT7_Memory));

                AddParam(cmd, "@SIPTInfo_SIPT8_Postural", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT8_Postural));
                AddParam(cmd, "@SIPTInfo_SIPT8_Oral", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT8_Oral));
                AddParam(cmd, "@SIPTInfo_SIPT8_Sequencing", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT8_Sequencing));
                AddParam(cmd, "@SIPTInfo_SIPT8_Commands", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT8_Commands));

                AddParam(cmd, "@SIPTInfo_SIPT9_Bilateral", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT9_Bilateral));
                AddParam(cmd, "@SIPTInfo_SIPT9_Contralat", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT9_Contralat));
                AddParam(cmd, "@SIPTInfo_SIPT9_PreferredHand", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT9_PreferredHand));
                AddParam(cmd, "@SIPTInfo_SIPT9_CrossingMidline", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT9_CrossingMidline));

                AddParam(cmd, "@SIPTInfo_SIPT10_Draw", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_Draw));
                AddParam(cmd, "@SIPTInfo_SIPT10_ClockFace", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_ClockFace));
                AddParam(cmd, "@SIPTInfo_SIPT10_Filtering", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_Filtering));
                AddParam(cmd, "@SIPTInfo_SIPT10_MotorPlanning", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_MotorPlanning));
                AddParam(cmd, "@SIPTInfo_SIPT10_BodyImage", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_BodyImage));
                AddParam(cmd, "@SIPTInfo_SIPT10_BodySchema", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_BodySchema));
                AddParam(cmd, "@SIPTInfo_SIPT10_Laterality", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_SIPT10_Laterality));

                AddParam(cmd, "@SIPTInfo_ActivityGiven_Remark", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Remark));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_InterestActivity", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_InterestActivity));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_InterestCompletion", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_InterestCompletion));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Learning", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Learning));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Complexity", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Complexity));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_ProblemSolving", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_ProblemSolving));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Concentration", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Concentration));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Retension", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Retension));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_SpeedPerfom", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_SpeedPerfom));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Neatness", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Neatness));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Frustation", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Frustation));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Work", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Work));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_Reaction", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_Reaction));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_SociabilityTherapist", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_SociabilityTherapist));
                AddParam(cmd, "@SIPTInfo_ActivityGiven_SociabilityStudents", SqlDbType.NVarChar, DbNullIfEmpty(SIPTInfo_ActivityGiven_SociabilityStudents));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab19(HttpContext context, int appointmentID)
        {
            int tabNo = 19;

            string Evaluation_Strengths = GetStr(context, "Evaluation_Strengths");

            string Evaluation_Concern_Barriers = GetStr(context, "Evaluation_Concern_Barriers");
            string Evaluation_Concern_Limitations = GetStr(context, "Evaluation_Concern_Limitations");
            string Evaluation_Concern_Posture = GetStr(context, "Evaluation_Concern_Posture");
            string Evaluation_Concern_Impairment = GetStr(context, "Evaluation_Concern_Impairment");

            string Evaluation_Goal_Summary = GetStr(context, "Evaluation_Goal_Summary");
            string Evaluation_Goal_Previous = GetStr(context, "Evaluation_Goal_Previous");
            string Evaluation_Goal_LongTerm = GetStr(context, "Evaluation_Goal_LongTerm");
            string Evaluation_Goal_ShortTerm = GetStr(context, "Evaluation_Goal_ShortTerm");
            string Evaluation_Goal_Impairment = GetStr(context, "Evaluation_Goal_Impairment");

            string Evaluation_Plan_Frequency = GetStr(context, "Evaluation_Plan_Frequency");
            string Evaluation_Plan_Service = GetStr(context, "Evaluation_Plan_Service");
            string Evaluation_Plan_Strategies = GetStr(context, "Evaluation_Plan_Strategies");
            string Evaluation_Plan_Equipment = GetStr(context, "Evaluation_Plan_Equipment");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Evaluation_Strengths", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Strengths));

                AddParam(cmd, "@Evaluation_Concern_Barriers", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Concern_Barriers));
                AddParam(cmd, "@Evaluation_Concern_Limitations", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Concern_Limitations));
                AddParam(cmd, "@Evaluation_Concern_Posture", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Concern_Posture));
                AddParam(cmd, "@Evaluation_Concern_Impairment", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Concern_Impairment));

                AddParam(cmd, "@Evaluation_Goal_Summary", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Goal_Summary));
                AddParam(cmd, "@Evaluation_Goal_Previous", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Goal_Previous));
                AddParam(cmd, "@Evaluation_Goal_LongTerm", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Goal_LongTerm));
                AddParam(cmd, "@Evaluation_Goal_ShortTerm", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Goal_ShortTerm));
                AddParam(cmd, "@Evaluation_Goal_Impairment", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Goal_Impairment));

                AddParam(cmd, "@Evaluation_Plan_Frequency", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Plan_Frequency));
                AddParam(cmd, "@Evaluation_Plan_Service", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Plan_Service));
                AddParam(cmd, "@Evaluation_Plan_Strategies", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Plan_Strategies));
                AddParam(cmd, "@Evaluation_Plan_Equipment", SqlDbType.NVarChar, DbNullIfEmpty(Evaluation_Plan_Equipment));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab20(HttpContext context, int appointmentID)
        {
            int tabNo = 20;

            string Treatment_Home = GetStr(context, "Treatment_Home");
            string Treatment_School = GetStr(context, "Treatment_School");
            string Treatment_Threapy = GetStr(context, "Treatment_Threapy");
            string Treatment_cmt = GetStr(context, "Treatment_cmt");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Treatment_Home", SqlDbType.NVarChar, DbNullIfEmpty(Treatment_Home));
                AddParam(cmd, "@Treatment_School", SqlDbType.NVarChar, DbNullIfEmpty(Treatment_School));
                AddParam(cmd, "@Treatment_Threapy", SqlDbType.NVarChar, DbNullIfEmpty(Treatment_Threapy));
                AddParam(cmd, "@Treatment_cmt", SqlDbType.NVarChar, DbNullIfEmpty(Treatment_cmt));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab21(HttpContext context, int appointmentID)
        {
            int tabNo = 21;

            string Doctor_Physioptherapist = GetStr(context, "Doctor_Physioptherapist");
            string Doctor_Occupational = GetStr(context, "Doctor_Occupational");

            using (SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@Doctor_Physioptherapist", SqlDbType.NVarChar, DbNullIfEmpty(Doctor_Physioptherapist));
                AddParam(cmd, "@Doctor_Occupational", SqlDbType.NVarChar, DbNullIfEmpty(Doctor_Occupational));

                // Always NULL in DB
                AddParam(cmd, "@Doctor_EnterReport", SqlDbType.NVarChar, DBNull.Value);

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }


        private string ResolveClientUrl(string sessionOutURL)
        {
            throw new NotImplementedException();
        }
        private string GetStr(HttpContext context, string key)
        {
            return (context.Request[key] ?? "").Trim();
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
                // Same as earlier pattern
                string logDir = context.Server.MapPath(
                    "~/" + Path.Combine("Logs", "Si_2021")
                );

                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // File per AppointmentID
                string logFile = Path.Combine(logDir, $"{appointmentID}.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("===== SI Report 2021 SAVE LOG =====");
                sb.AppendLine("Time          : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("TabNo         : " + tabNo);
                sb.AppendLine("AppointmentID : " + appointmentID);
                sb.AppendLine("LoginId       : " + _loginID);

                sb.AppendLine("FORM DATA:");
                foreach (string key in context.Request.Form.AllKeys)
                {
                    sb.AppendLine(key + " = " + context.Request.Form[key]);
                }

                sb.AppendLine("==================================");

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
                string logDir = context.Server.MapPath("~/Logs/Modal/Si_2021");

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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}