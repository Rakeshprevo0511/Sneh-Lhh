using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace snehrehab.Handler
{
    /// <summary>
    /// Summary description for PreconstRpt_2021
    /// </summary>
    public class PreconstRpt_2021 : IHttpHandler, IRequiresSessionState
    {
        DbHelper.SqlDb db; int _loginID = 0;
        SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
        public PreconstRpt_2021()
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
                //context.Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
                context.Response.StatusCode = 401; // 🔥 better
                context.Response.Write("ERROR|SESSION_EXPIRED");
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
                        string logAction = context.Request.Form["LogAction"]; string modalLog = context.Request.Form["ModalLog"];

                        LogModal(context, "MODAL " + logAction + " | Data: " + modalLog, null, appointmentID);

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
                catch (Exception logEx)
                {
                    System.Diagnostics.Debug.WriteLine("Logging failed: " + logEx.Message);
                }
                context.Response.StatusCode = 500;
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.Write("ERROR|Internal server error");
            }
        }
        private int SaveTab1(HttpContext context, int appointmentID)
        {
            int tabNo = 1;

            string ComfortableLanguage = GetStr(context, "ComfortableLanguage");
            string DatepreConsultStr = GetStr(context, "DatepreConsult");
            string DateBirthStr = GetStr(context, "DateBirth");
            string DateDeliveryStr = GetStr(context, "DateDelivery");

            string CorrectAge = GetStr(context, "CorrectAge");
            string Age = GetStr(context, "Age");
            string Gender = GetStr(context, "Gender");
            string ChildAttend = GetStr(context, "ChildAttend");

            string OnlineOffline = GetStr(context, "OnlineOffline");
            string WhichGrade = GetStr(context, "WhichGrade");

            string MotherName = GetStr(context, "MotherName");
            string MotherAge = GetStr(context, "MotherAge");
            string MotherQualification = GetStr(context, "MotherQualification");
            string MotherOccupation = GetStr(context, "MotherOccupation");
            string MotherWorkingHour = GetStr(context, "MotherWorkingHour");

            string FatherName = GetStr(context, "FatherName");
            string FatherAge = GetStr(context, "FatherAge");
            string FatherOccupation = GetStr(context, "FatherOccupation");
            string FatherQualification = GetStr(context, "FatherQualification");
            string FatherWorkingHour = GetStr(context, "FatherWorkingHour");

            string Address = GetStr(context, "Address");
            string ContactDetails = GetStr(context, "ContactDetails");
            string EmailID = GetStr(context, "EmailID");
            string ReferredBy = GetStr(context, "ReferredBy");
            string TherapistDuringPC = GetStr(context, "TherapistDuringPC");
            string Diagnosis = GetStr(context, "Diagnosis");
            string CommentsPI = GetStr(context, "CommentsPI");

            DateTime DatepreConsult = DateTime.MinValue;
            DateTime DateBirth = DateTime.MinValue;
            DateTime DateDelivery = DateTime.MinValue;

            if (!string.IsNullOrEmpty(DatepreConsultStr))
                DateTime.TryParseExact(DatepreConsultStr, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatepreConsult);

            if (!string.IsNullOrEmpty(DateBirthStr))
                DateTime.TryParseExact(DateBirthStr, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth);

            if (!string.IsNullOrEmpty(DateDeliveryStr))
                DateTime.TryParseExact(DateDeliveryStr, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateDelivery);

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@ModifyDate", SqlDbType.DateTime, DateTime.UtcNow.AddMinutes(330));
                AddParam(cmd, "@ModifyBy", SqlDbType.Int, _loginID);

                AddParam(cmd, "@ComfortableLanguage", SqlDbType.VarChar, DbNullIfEmpty(ComfortableLanguage));

                AddParam(cmd, "@DatepreConsult", SqlDbType.DateTime, (DatepreConsult > DateTime.MinValue) ? (object)DatepreConsult : DBNull.Value);
                AddParam(cmd, "@DateBirth", SqlDbType.DateTime, (DateBirth > DateTime.MinValue) ? (object)DateBirth : DBNull.Value);
                AddParam(cmd, "@DateDelivery", SqlDbType.DateTime, (DateDelivery > DateTime.MinValue) ? (object)DateDelivery : DBNull.Value);

                AddParam(cmd, "@CorrectAge", SqlDbType.VarChar, DbNullIfEmpty(CorrectAge));
                AddParam(cmd, "@Age", SqlDbType.VarChar, DbNullIfEmpty(Age));
                AddParam(cmd, "@Gender", SqlDbType.VarChar, DbNullIfEmpty(Gender));
                AddParam(cmd, "@ChildAttend", SqlDbType.VarChar, DbNullIfEmpty(ChildAttend));
                AddParam(cmd, "@OnlineOffline", SqlDbType.VarChar, DbNullIfEmpty(OnlineOffline));
                AddParam(cmd, "@WhichGrade", SqlDbType.VarChar, DbNullIfEmpty(WhichGrade));

                AddParam(cmd, "@MotherName", SqlDbType.VarChar, DbNullIfEmpty(MotherName));
                AddParam(cmd, "@MotherAge", SqlDbType.VarChar, DbNullIfEmpty(MotherAge));
                AddParam(cmd, "@MotherQualification", SqlDbType.VarChar, DbNullIfEmpty(MotherQualification));
                AddParam(cmd, "@MotherOccupation", SqlDbType.VarChar, DbNullIfEmpty(MotherOccupation));
                AddParam(cmd, "@MotherWorkingHour", SqlDbType.VarChar, DbNullIfEmpty(MotherWorkingHour));

                AddParam(cmd, "@FatherName", SqlDbType.VarChar, DbNullIfEmpty(FatherName));
                AddParam(cmd, "@FatherAge", SqlDbType.VarChar, DbNullIfEmpty(FatherAge));
                AddParam(cmd, "@FatherOccupation", SqlDbType.VarChar, DbNullIfEmpty(FatherOccupation));
                AddParam(cmd, "@FatherQualification", SqlDbType.VarChar, DbNullIfEmpty(FatherQualification));
                AddParam(cmd, "@FatherWorkingHour", SqlDbType.VarChar, DbNullIfEmpty(FatherWorkingHour));

                AddParam(cmd, "@Address", SqlDbType.VarChar, DbNullIfEmpty(Address));
                AddParam(cmd, "@ContactDetails", SqlDbType.VarChar, DbNullIfEmpty(ContactDetails));
                AddParam(cmd, "@EmailID", SqlDbType.VarChar, DbNullIfEmpty(EmailID));
                AddParam(cmd, "@ReferredBy", SqlDbType.VarChar, DbNullIfEmpty(ReferredBy));
                AddParam(cmd, "@TherapistDuringPC", SqlDbType.VarChar, DbNullIfEmpty(TherapistDuringPC));
                AddParam(cmd, "@Diagnosis", SqlDbType.VarChar, DbNullIfEmpty(Diagnosis));
                AddParam(cmd, "@CommentsPI", SqlDbType.VarChar, DbNullIfEmpty(CommentsPI));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab2(HttpContext context, int appointmentID)
        {
            int tabNo = 2;

            string ChiefConcernsHome = GetStr(context, "ChiefConcernsHome");
            string ChiefConcernsSchool = GetStr(context, "ChiefConcernsSchool");
            string ChiefConcernsSocialGath = GetStr(context, "ChiefConcernsSocialGath");
            string CommentsCC = GetStr(context, "CommentsCC");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

                AddParam(cmd, "@ChiefConcernsHome", SqlDbType.VarChar, DbNullIfEmpty(ChiefConcernsHome));
                AddParam(cmd, "@ChiefConcernsSchool", SqlDbType.VarChar, DbNullIfEmpty(ChiefConcernsSchool));
                AddParam(cmd, "@ChiefConcernsSocialGath", SqlDbType.VarChar, DbNullIfEmpty(ChiefConcernsSocialGath));
                AddParam(cmd, "@CommentsCC", SqlDbType.VarChar, DbNullIfEmpty(CommentsCC));

                SqlParameter ret = cmd.Parameters.Add("@RetVal", SqlDbType.Int);
                ret.Direction = ParameterDirection.Output;

                db.DbUpdate(cmd);

                return ToInt(ret.Value);
            }
        }
        private int SaveTab3(HttpContext context, int appointmentID)
        {
            int tabNo = 3;

            string TimelineData = GetStr(context, "TimelineData");
            string DeletedTimelineIds = GetStr(context, "DeletedTimelineIds");

            // 1️⃣ DELETE
            if (!string.IsNullOrEmpty(DeletedTimelineIds))
            {
                string[] delIds = DeletedTimelineIds.Split(',');
                foreach (string idStr in delIds)
                {
                    int id;
                    if (int.TryParse(idStr.Trim(), out id) && id > 0)
                    {
                        RDB.DeleteRow(id);
                    }
                }
            }

            // 2️⃣ SAVE / UPDATE
            if (!string.IsNullOrEmpty(TimelineData))
            {
                string[] rows = TimelineData.Split('~');

                foreach (string r in rows)
                {
                    if (string.IsNullOrWhiteSpace(r)) continue;

                    // 🔹 KEEP YOUR LOGIC
                    // PreConsultID#Option1$Option2$Option3$Option4$Option5|Order
                    string[] parts = r.Split('#');
                    if (parts.Length < 2) continue;

                    int preConsultID = 0;
                    int.TryParse(parts[0], out preConsultID);

                    string dataPart = parts[1];
                    int sortOrder = 0;

                    // 🔹 SAFE ORDER EXTRACTION
                    if (dataPart.Contains("|"))
                    {
                        string[] orderSplit = dataPart.Split('|');
                        dataPart = orderSplit[0];
                        int.TryParse(orderSplit[1], out sortOrder);
                    }

                    string[] cols = dataPart.Split('$');

                    string Option1 = (cols.Length > 0) ? cols[0].Trim() : "";
                    string Option2 = (cols.Length > 1) ? cols[1].Trim() : "";
                    string Option3 = (cols.Length > 2) ? cols[2].Trim() : "";
                    string Option4 = (cols.Length > 3) ? cols[3].Trim() : "";
                    string Option5 = (cols.Length > 4) ? cols[4].Trim() : "";

                    if (
                        string.IsNullOrEmpty(Option1) &&
                        string.IsNullOrEmpty(Option2) &&
                        string.IsNullOrEmpty(Option3) &&
                        string.IsNullOrEmpty(Option4) &&
                        string.IsNullOrEmpty(Option5)
                    )
                    {
                        continue;
                    }

                    RDB.SetTimeLine(
                        appointmentID,
                        preConsultID,
                        Option1,
                        Option2,
                        Option3,
                        Option4,
                        Option5,
                        sortOrder
                    );
                }
            }

            // 3️⃣ TABWISE SP
            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);
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

            string Consanguinity = GetStr(context, "Consanguinity");
            string Consanguinity_1 = GetStr(context, "Consanguinity_1");

            string ConsanguinityDegree = GetStr(context, "ConsanguinityDegree");
            string ConsanguinityDegree_1 = GetStr(context, "ConsanguinityDegree_1");
            string ConsanguinityDegree_2 = GetStr(context, "ConsanguinityDegree_2");

            string YearsMarriage = GetStr(context, "YearsMarriage");

            string FamilyStructure = GetStr(context, "FamilyStructure");
            string FamilyStructure_1 = GetStr(context, "FamilyStructure_1");

            string Conception = GetStr(context, "Conception");
            string Conception_1 = GetStr(context, "Conception_1");
            string Conception_2 = GetStr(context, "Conception_2");
            string Conception_3 = GetStr(context, "Conception_3");
            string Conception_4 = GetStr(context, "Conception_4");

            string PlanningConception = GetStr(context, "PlanningConception");
            string PlanningConception_1 = GetStr(context, "PlanningConception_1");

            string Siblings = GetStr(context, "Siblings");
            string NoOfSiblings = GetStr(context, "NoOfSiblings");
            string RHASiblings = GetStr(context, "RHASiblings");
            string CommentsFH = GetStr(context, "CommentsFH");

            string InterParentalRelation = GetStr(context, "InterParentalRelation");
            string InterParentalRelation_1 = GetStr(context, "InterParentalRelation_1");
            string InterParentalRelation_2 = GetStr(context, "InterParentalRelation_2");

            string ParentChildRelation = GetStr(context, "ParentChildRelation");
            string ParentChildRelation_1 = GetStr(context, "ParentChildRelation_1");
            string ParentChildRelation_2 = GetStr(context, "ParentChildRelation_2");

            string InterSiblingRelation = GetStr(context, "InterSiblingRelation");
            string InterSiblingRelation_1 = GetStr(context, "InterSiblingRelation_1");
            string InterSiblingRelation_2 = GetStr(context, "InterSiblingRelation_2");

            string DomesticViolence = GetStr(context, "DomesticViolence");
            string DomesticViolence_1 = GetStr(context, "DomesticViolence_1");
            string DomesticViolence_2 = GetStr(context, "DomesticViolence_2");

            string FamilyRelocation = GetStr(context, "FamilyRelocation");
            string FamilyRelocation_1 = GetStr(context, "FamilyRelocation_1");

            string frequency = GetStr(context, "frequency");

            string PrimaryCare = GetStr(context, "PrimaryCare");
            string PrimaryCare_1 = GetStr(context, "PrimaryCare_1");
            string PrimaryCare_2 = GetStr(context, "PrimaryCare_2");
            string PrimaryCare_3 = GetStr(context, "PrimaryCare_3");

            string MotherScreenTime = GetStr(context, "MotherScreenTime");
            string ScreenTimeChild = GetStr(context, "ScreenTimeChild");
            string CommentsFR = GetStr(context, "CommentsFR");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Consanguinity", SqlDbType.VarChar, DbNullIfEmpty(Consanguinity));
                AddParam(cmd, "@Consanguinity_1", SqlDbType.VarChar, DbNullIfEmpty(Consanguinity_1));

                AddParam(cmd, "@ConsanguinityDegree", SqlDbType.VarChar, DbNullIfEmpty(ConsanguinityDegree));
                AddParam(cmd, "@ConsanguinityDegree_1", SqlDbType.VarChar, DbNullIfEmpty(ConsanguinityDegree_1));
                AddParam(cmd, "@ConsanguinityDegree_2", SqlDbType.VarChar, DbNullIfEmpty(ConsanguinityDegree_2));

                AddParam(cmd, "@YearsMarriage", SqlDbType.VarChar, DbNullIfEmpty(YearsMarriage));

                AddParam(cmd, "@FamilyStructure", SqlDbType.VarChar, DbNullIfEmpty(FamilyStructure));
                AddParam(cmd, "@FamilyStructure_1", SqlDbType.VarChar, DbNullIfEmpty(FamilyStructure_1));

                AddParam(cmd, "@Conception", SqlDbType.VarChar, DbNullIfEmpty(Conception));
                AddParam(cmd, "@Conception_1", SqlDbType.VarChar, DbNullIfEmpty(Conception_1));
                AddParam(cmd, "@Conception_2", SqlDbType.VarChar, DbNullIfEmpty(Conception_2));
                AddParam(cmd, "@Conception_3", SqlDbType.VarChar, DbNullIfEmpty(Conception_3));
                AddParam(cmd, "@Conception_4", SqlDbType.VarChar, DbNullIfEmpty(Conception_4));

                AddParam(cmd, "@PlanningConception", SqlDbType.VarChar, DbNullIfEmpty(PlanningConception));
                AddParam(cmd, "@PlanningConception_1", SqlDbType.VarChar, DbNullIfEmpty(PlanningConception_1));

                AddParam(cmd, "@Siblings", SqlDbType.VarChar, DbNullIfEmpty(Siblings));
                AddParam(cmd, "@NoOfSiblings", SqlDbType.VarChar, DbNullIfEmpty(NoOfSiblings));
                AddParam(cmd, "@RHASiblings", SqlDbType.VarChar, DbNullIfEmpty(RHASiblings));
                AddParam(cmd, "@CommentsFH", SqlDbType.VarChar, DbNullIfEmpty(CommentsFH));

                AddParam(cmd, "@InterParentalRelation", SqlDbType.VarChar, DbNullIfEmpty(InterParentalRelation));
                AddParam(cmd, "@InterParentalRelation_1", SqlDbType.VarChar, DbNullIfEmpty(InterParentalRelation_1));
                AddParam(cmd, "@InterParentalRelation_2", SqlDbType.VarChar, DbNullIfEmpty(InterParentalRelation_2));

                AddParam(cmd, "@ParentChildRelation", SqlDbType.VarChar, DbNullIfEmpty(ParentChildRelation));
                AddParam(cmd, "@ParentChildRelation_1", SqlDbType.VarChar, DbNullIfEmpty(ParentChildRelation_1));
                AddParam(cmd, "@ParentChildRelation_2", SqlDbType.VarChar, DbNullIfEmpty(ParentChildRelation_2));

                AddParam(cmd, "@InterSiblingRelation", SqlDbType.VarChar, DbNullIfEmpty(InterSiblingRelation));
                AddParam(cmd, "@InterSiblingRelation_1", SqlDbType.VarChar, DbNullIfEmpty(InterSiblingRelation_1));
                AddParam(cmd, "@InterSiblingRelation_2", SqlDbType.VarChar, DbNullIfEmpty(InterSiblingRelation_2));

                AddParam(cmd, "@DomesticViolence", SqlDbType.VarChar, DbNullIfEmpty(DomesticViolence));
                AddParam(cmd, "@DomesticViolence_1", SqlDbType.VarChar, DbNullIfEmpty(DomesticViolence_1));
                AddParam(cmd, "@DomesticViolence_2", SqlDbType.VarChar, DbNullIfEmpty(DomesticViolence_2));

                AddParam(cmd, "@FamilyRelocation", SqlDbType.VarChar, DbNullIfEmpty(FamilyRelocation));
                AddParam(cmd, "@FamilyRelocation_1", SqlDbType.VarChar, DbNullIfEmpty(FamilyRelocation_1));

                AddParam(cmd, "@frequency", SqlDbType.VarChar, DbNullIfEmpty(frequency));

                AddParam(cmd, "@PrimaryCare", SqlDbType.VarChar, DbNullIfEmpty(PrimaryCare));
                AddParam(cmd, "@PrimaryCare_1", SqlDbType.VarChar, DbNullIfEmpty(PrimaryCare_1));
                AddParam(cmd, "@PrimaryCare_2", SqlDbType.VarChar, DbNullIfEmpty(PrimaryCare_2));
                AddParam(cmd, "@PrimaryCare_3", SqlDbType.VarChar, DbNullIfEmpty(PrimaryCare_3));

                AddParam(cmd, "@MotherScreenTime", SqlDbType.VarChar, DbNullIfEmpty(MotherScreenTime));
                AddParam(cmd, "@ScreenTimeChild", SqlDbType.VarChar, DbNullIfEmpty(ScreenTimeChild));
                AddParam(cmd, "@CommentsFR", SqlDbType.VarChar, DbNullIfEmpty(CommentsFR));

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

            string PrenatalCondition = GetStr(context, "PrenatalCondition");
            string MaternalStress = GetStr(context, "MaternalStress");
            string MaternalStress_1 = GetStr(context, "MaternalStress_1");
            string DescribeStressors = GetStr(context, "DescribeStressors");
            string WGDP = GetStr(context, "WGDP");
            string FoetalMovement = GetStr(context, "FoetalMovement");
            string Prenatalwellness = GetStr(context, "Prenatalwellness");
            string CommentsMH = GetStr(context, "CommentsMH");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@PrenatalCondition", SqlDbType.VarChar, DbNullIfEmpty(PrenatalCondition));
                AddParam(cmd, "@MaternalStress", SqlDbType.VarChar, DbNullIfEmpty(MaternalStress));
                AddParam(cmd, "@MaternalStress_1", SqlDbType.VarChar, DbNullIfEmpty(MaternalStress_1));
                AddParam(cmd, "@DescribeStressors", SqlDbType.VarChar, DbNullIfEmpty(DescribeStressors));
                AddParam(cmd, "@WGDP", SqlDbType.VarChar, DbNullIfEmpty(WGDP));
                AddParam(cmd, "@FoetalMovement", SqlDbType.VarChar, DbNullIfEmpty(FoetalMovement));
                AddParam(cmd, "@Prenatalwellness", SqlDbType.VarChar, DbNullIfEmpty(Prenatalwellness));
                AddParam(cmd, "@CommentsMH", SqlDbType.VarChar, DbNullIfEmpty(CommentsMH));

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

            string DurationLabour = GetStr(context, "DurationLabour");

            string delivery = GetStr(context, "delivery");
            string delivery_1 = GetStr(context, "delivery_1");
            string delivery_2 = GetStr(context, "delivery_2");
            string delivery_3 = GetStr(context, "delivery_3");

            string ciab = GetStr(context, "ciab");

            string ConditionPostBirth = GetStr(context, "ConditionPostBirth");
            string BirthWeight = GetStr(context, "BirthWeight");

            string GestationalBirthAge = GetStr(context, "GestationalBirthAge");
            string GestationalBirthAge_1 = GetStr(context, "GestationalBirthAge_1");
            string GestationalBirthAge_2 = GetStr(context, "GestationalBirthAge_2");

            string NICUstay = GetStr(context, "NICUstay");
            string DurationNICUstay = GetStr(context, "DurationNICUstay");
            string NICUHistory = GetStr(context, "NICUHistory");
            string ReasonNICUstay = GetStr(context, "ReasonNICUstay");
            string APGARscore = GetStr(context, "APGARscore");

            string Breastfed = GetStr(context, "Breastfed");
            string BabyFed = GetStr(context, "BabyFed");

            string Problemsduringbreastfeeding = GetStr(context, "Problemsduringbreastfeeding");
            string MentionProblem = GetStr(context, "MentionProblem");
            string waswtcbf = GetStr(context, "waswtcbf");

            string colicissue = GetStr(context, "colicissue");
            string OthrtMedicalIssues = GetStr(context, "OthrtMedicalIssues");
            string CommentsPPH = GetStr(context, "CommentsPPH");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@DurationLabour", SqlDbType.VarChar, DbNullIfEmpty(DurationLabour));

                AddParam(cmd, "@delivery", SqlDbType.VarChar, DbNullIfEmpty(delivery));
                AddParam(cmd, "@delivery_1", SqlDbType.VarChar, DbNullIfEmpty(delivery_1));
                AddParam(cmd, "@delivery_2", SqlDbType.VarChar, DbNullIfEmpty(delivery_2));
                AddParam(cmd, "@delivery_3", SqlDbType.VarChar, DbNullIfEmpty(delivery_3));

                AddParam(cmd, "@ciab", SqlDbType.VarChar, DbNullIfEmpty(ciab));

                AddParam(cmd, "@ConditionPostBirth", SqlDbType.VarChar, DbNullIfEmpty(ConditionPostBirth));
                AddParam(cmd, "@BirthWeight", SqlDbType.VarChar, DbNullIfEmpty(BirthWeight));

                AddParam(cmd, "@GestationalBirthAge", SqlDbType.VarChar, DbNullIfEmpty(GestationalBirthAge));
                AddParam(cmd, "@GestationalBirthAge_1", SqlDbType.VarChar, DbNullIfEmpty(GestationalBirthAge_1));
                AddParam(cmd, "@GestationalBirthAge_2", SqlDbType.VarChar, DbNullIfEmpty(GestationalBirthAge_2));

                AddParam(cmd, "@NICUstay", SqlDbType.VarChar, DbNullIfEmpty(NICUstay));
                AddParam(cmd, "@DurationNICUstay", SqlDbType.VarChar, DbNullIfEmpty(DurationNICUstay));
                AddParam(cmd, "@NICUHistory", SqlDbType.VarChar, DbNullIfEmpty(NICUHistory));
                AddParam(cmd, "@ReasonNICUstay", SqlDbType.VarChar, DbNullIfEmpty(ReasonNICUstay));
                AddParam(cmd, "@APGARscore", SqlDbType.VarChar, DbNullIfEmpty(APGARscore));

                AddParam(cmd, "@Breastfed", SqlDbType.VarChar, DbNullIfEmpty(Breastfed));
                AddParam(cmd, "@BabyFed", SqlDbType.VarChar, DbNullIfEmpty(BabyFed));

                AddParam(cmd, "@Problemsduringbreastfeeding", SqlDbType.VarChar, DbNullIfEmpty(Problemsduringbreastfeeding));
                AddParam(cmd, "@MentionProblem", SqlDbType.VarChar, DbNullIfEmpty(MentionProblem));
                AddParam(cmd, "@waswtcbf", SqlDbType.VarChar, DbNullIfEmpty(waswtcbf));

                AddParam(cmd, "@colicissue", SqlDbType.VarChar, DbNullIfEmpty(colicissue));
                AddParam(cmd, "@OthrtMedicalIssues", SqlDbType.VarChar, DbNullIfEmpty(OthrtMedicalIssues));
                AddParam(cmd, "@CommentsPPH", SqlDbType.VarChar, DbNullIfEmpty(CommentsPPH));

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

            string GrossMotor = GetStr(context, "GrossMotor");
            string FineMotor = GetStr(context, "FineMotor");
            string PersonalandSocial = GetStr(context, "PersonalandSocial");
            string Communication = GetStr(context, "Communication");
            string CommentsDM = GetStr(context, "CommentsDM");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@GrossMotor", SqlDbType.VarChar, DbNullIfEmpty(GrossMotor));
                AddParam(cmd, "@FineMotor", SqlDbType.VarChar, DbNullIfEmpty(FineMotor));
                AddParam(cmd, "@PersonalandSocial", SqlDbType.VarChar, DbNullIfEmpty(PersonalandSocial));
                AddParam(cmd, "@Communication", SqlDbType.VarChar, DbNullIfEmpty(Communication));
                AddParam(cmd, "@CommentsDM", SqlDbType.VarChar, DbNullIfEmpty(CommentsDM));

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

            string Sleepissues = GetStr(context, "Sleepissues");
            string Presentsleep = GetStr(context, "Presentsleep");
            string Sleepduration = GetStr(context, "Sleepduration");
            string SleepType = GetStr(context, "SleepType");
            string Cosleeping = GetStr(context, "Cosleeping");
            string Cosleepingwith = GetStr(context, "Cosleepingwith");
            string AnySleepAdjunctsused = GetStr(context, "AnySleepAdjunctsused");
            string Naptime = GetStr(context, "Naptime");
            string Napduration = GetStr(context, "Napduration");
            string CommentsS = GetStr(context, "CommentsS");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Sleepissues", SqlDbType.VarChar, DbNullIfEmpty(Sleepissues));
                AddParam(cmd, "@Presentsleep", SqlDbType.VarChar, DbNullIfEmpty(Presentsleep));
                AddParam(cmd, "@Sleepduration", SqlDbType.VarChar, DbNullIfEmpty(Sleepduration));
                AddParam(cmd, "@SleepType", SqlDbType.VarChar, DbNullIfEmpty(SleepType));
                AddParam(cmd, "@Cosleeping", SqlDbType.VarChar, DbNullIfEmpty(Cosleeping));
                AddParam(cmd, "@Cosleepingwith", SqlDbType.VarChar, DbNullIfEmpty(Cosleepingwith));
                AddParam(cmd, "@AnySleepAdjunctsused", SqlDbType.VarChar, DbNullIfEmpty(AnySleepAdjunctsused));
                AddParam(cmd, "@Naptime", SqlDbType.VarChar, DbNullIfEmpty(Naptime));
                AddParam(cmd, "@Napduration", SqlDbType.VarChar, DbNullIfEmpty(Napduration));
                AddParam(cmd, "@CommentsS", SqlDbType.VarChar, DbNullIfEmpty(CommentsS));

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

            string Feedinghabits = GetStr(context, "Feedinghabits");
            string Typeoffoodhad = GetStr(context, "Typeoffoodhad");
            string Foodconsistency = GetStr(context, "Foodconsistency");
            string Foodtemperature = GetStr(context, "Foodtemperature");
            string Foodtaste = GetStr(context, "Foodtaste");
            string CommentsFeHa = GetStr(context, "CommentsFeHa");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Feedinghabits", SqlDbType.VarChar, DbNullIfEmpty(Feedinghabits));
                AddParam(cmd, "@Typeoffoodhad", SqlDbType.VarChar, DbNullIfEmpty(Typeoffoodhad));
                AddParam(cmd, "@Foodconsistency", SqlDbType.VarChar, DbNullIfEmpty(Foodconsistency));
                AddParam(cmd, "@Foodtemperature", SqlDbType.VarChar, DbNullIfEmpty(Foodtemperature));
                AddParam(cmd, "@Foodtaste", SqlDbType.VarChar, DbNullIfEmpty(Foodtaste));
                AddParam(cmd, "@CommentsFeHa", SqlDbType.VarChar, DbNullIfEmpty(CommentsFeHa));

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

            string ChildLikes = GetStr(context, "ChildLikes");
            string CommentsITCH = GetStr(context, "CommentsITCH");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@ChildLikes", SqlDbType.VarChar, DbNullIfEmpty(ChildLikes));
                AddParam(cmd, "@CommentsITCH", SqlDbType.VarChar, DbNullIfEmpty(CommentsITCH));

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

            string Playbehaviour = GetStr(context, "Playbehaviour");
            string Interactionwithpeers = GetStr(context, "Interactionwithpeers");
            string Strangeranxiety = GetStr(context, "Strangeranxiety");
            string PlayToys = GetStr(context, "PlayToys");
            string Preferenceoftoys = GetStr(context, "Preferenceoftoys");
            string CommentsPB = GetStr(context, "CommentsPB");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Playbehaviour", SqlDbType.VarChar, DbNullIfEmpty(Playbehaviour));
                AddParam(cmd, "@Interactionwithpeers", SqlDbType.VarChar, DbNullIfEmpty(Interactionwithpeers));
                AddParam(cmd, "@Strangeranxiety", SqlDbType.VarChar, DbNullIfEmpty(Strangeranxiety));
                AddParam(cmd, "@PlayToys", SqlDbType.VarChar, DbNullIfEmpty(PlayToys));
                AddParam(cmd, "@Preferenceoftoys", SqlDbType.VarChar, DbNullIfEmpty(Preferenceoftoys));
                AddParam(cmd, "@CommentsPB", SqlDbType.VarChar, DbNullIfEmpty(CommentsPB));

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

            string Brushing = GetStr(context, "Brushing");
            string Brushing_1 = GetStr(context, "Brushing_1");
            string Brushing_2 = GetStr(context, "Brushing_2");
            string CommentsBrushing = GetStr(context, "CommentsBrushing");

            string Bathing = GetStr(context, "Bathing");
            string Bathing_1 = GetStr(context, "Bathing_1");
            string Bathing_2 = GetStr(context, "Bathing_2");
            string CommentsBathing = GetStr(context, "CommentsBathing");

            string Toileting = GetStr(context, "Toileting");
            string Toileting_1 = GetStr(context, "Toileting_1");
            string Toileting_2 = GetStr(context, "Toileting_2");
            string CommentsToileting = GetStr(context, "CommentsToileting");

            string Dressing = GetStr(context, "Dressing");
            string Dressing_1 = GetStr(context, "Dressing_1");
            string Dressing_2 = GetStr(context, "Dressing_2");
            string CommentsDressing = GetStr(context, "CommentsDressing");

            string Eating = GetStr(context, "Eating");
            string Eating_1 = GetStr(context, "Eating_1");
            string Eating_2 = GetStr(context, "Eating_2");
            string CommentsEating = GetStr(context, "CommentsEating");

            string Ambulation = GetStr(context, "Ambulation");
            string Ambulation_1 = GetStr(context, "Ambulation_1");
            string Ambulation_2 = GetStr(context, "Ambulation_2");
            string CommentsAmbulation = GetStr(context, "CommentsAmbulation");

            string Transfers = GetStr(context, "Transfers");
            string Transfers_1 = GetStr(context, "Transfers_1");
            string Transfers_2 = GetStr(context, "Transfers_2");
            string CommentsTransfers = GetStr(context, "CommentsTransfers");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@Brushing", SqlDbType.VarChar, DbNullIfEmpty(Brushing));
                AddParam(cmd, "@Brushing_1", SqlDbType.VarChar, DbNullIfEmpty(Brushing_1));
                AddParam(cmd, "@Brushing_2", SqlDbType.VarChar, DbNullIfEmpty(Brushing_2));
                AddParam(cmd, "@CommentsBrushing", SqlDbType.VarChar, DbNullIfEmpty(CommentsBrushing));

                AddParam(cmd, "@Bathing", SqlDbType.VarChar, DbNullIfEmpty(Bathing));
                AddParam(cmd, "@Bathing_1", SqlDbType.VarChar, DbNullIfEmpty(Bathing_1));
                AddParam(cmd, "@Bathing_2", SqlDbType.VarChar, DbNullIfEmpty(Bathing_2));
                AddParam(cmd, "@CommentsBathing", SqlDbType.VarChar, DbNullIfEmpty(CommentsBathing));

                AddParam(cmd, "@Toileting", SqlDbType.VarChar, DbNullIfEmpty(Toileting));
                AddParam(cmd, "@Toileting_1", SqlDbType.VarChar, DbNullIfEmpty(Toileting_1));
                AddParam(cmd, "@Toileting_2", SqlDbType.VarChar, DbNullIfEmpty(Toileting_2));
                AddParam(cmd, "@CommentsToileting", SqlDbType.VarChar, DbNullIfEmpty(CommentsToileting));

                AddParam(cmd, "@Dressing", SqlDbType.VarChar, DbNullIfEmpty(Dressing));
                AddParam(cmd, "@Dressing_1", SqlDbType.VarChar, DbNullIfEmpty(Dressing_1));
                AddParam(cmd, "@Dressing_2", SqlDbType.VarChar, DbNullIfEmpty(Dressing_2));
                AddParam(cmd, "@CommentsDressing", SqlDbType.VarChar, DbNullIfEmpty(CommentsDressing));

                AddParam(cmd, "@Eating", SqlDbType.VarChar, DbNullIfEmpty(Eating));
                AddParam(cmd, "@Eating_1", SqlDbType.VarChar, DbNullIfEmpty(Eating_1));
                AddParam(cmd, "@Eating_2", SqlDbType.VarChar, DbNullIfEmpty(Eating_2));
                AddParam(cmd, "@CommentsEating", SqlDbType.VarChar, DbNullIfEmpty(CommentsEating));

                AddParam(cmd, "@Ambulation", SqlDbType.VarChar, DbNullIfEmpty(Ambulation));
                AddParam(cmd, "@Ambulation_1", SqlDbType.VarChar, DbNullIfEmpty(Ambulation_1));
                AddParam(cmd, "@Ambulation_2", SqlDbType.VarChar, DbNullIfEmpty(Ambulation_2));
                AddParam(cmd, "@CommentsAmbulation", SqlDbType.VarChar, DbNullIfEmpty(CommentsAmbulation));

                AddParam(cmd, "@Transfers", SqlDbType.VarChar, DbNullIfEmpty(Transfers));
                AddParam(cmd, "@Transfers_1", SqlDbType.VarChar, DbNullIfEmpty(Transfers_1));
                AddParam(cmd, "@Transfers_2", SqlDbType.VarChar, DbNullIfEmpty(Transfers_2));
                AddParam(cmd, "@CommentsTransfers", SqlDbType.VarChar, DbNullIfEmpty(CommentsTransfers));

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

            string AddComments = GetStr(context, "AddComments");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@AddComments", SqlDbType.VarChar, DbNullIfEmpty(AddComments));

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

            string AddEvalRec = GetStr(context, "AddEvalRec");

            using (SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                AddParam(cmd, "@AppointmentID", SqlDbType.Int, appointmentID);

                AddParam(cmd, "@AddEvalRec", SqlDbType.VarChar, DbNullIfEmpty(AddEvalRec));

                AddParam(cmd, "@TabNo", SqlDbType.Int, tabNo);

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

        public bool IsReusable { get { return false; } }
        private void LogRequest(HttpContext context, int tabNo, int appointmentID)
        {
            try
            {
                // Directly under Precon2021
                string relativePath = Path.Combine("Logs", "Precon2021");

                string logDir = context.Server.MapPath("~/" + relativePath);
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // File name = AppointmentID
                string logFile = Path.Combine(logDir, $"{appointmentID}.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("===== PRECON SAVE LOG =====");
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
                string logDir = context.Server.MapPath("~/Logs/Modal/Precon2021");

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

    }
}

