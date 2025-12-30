using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace snehrehab.Handler
{
    /// <summary>
    /// Summary description for Botox_Report_MailPDF
    /// </summary>
    public class Botox_Report_MailPDF : IHttpHandler, IRequiresSessionState
    {
        int _loginID = 0; string str = "<style>html{background: url(\"/images/bg_login.jpg\");}.alert{margin: 0 auto;max-width: 475px;margin-top: 10%;background: #F5F5F5;padding: 20px;color: #6E6666;font-family: calibri;font-size: 1.2em;min-height: 50px;border: 2px solid #A8A8A8;border-radius: 2px;font-weight: lighter;line-height: 30px;}</style>";
        int _appointmentID = 0; string mailid = string.Empty; string type = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            rModel r = new rModel();
            if (_loginID <= 0)
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">You required login to print document.</div>");
                context.Response.End();
                return;
            }
            var request = context.Request;
            var requestBody = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            var jsonSerializer = new JavaScriptSerializer();
            BotoxReport evnt = jsonSerializer.Deserialize<BotoxReport>(requestBody);
            int.TryParse(evnt.SiAppointmentID.ToString(), out _appointmentID);
            mailid = evnt.MailID;

            if (_appointmentID <= 0 || string.IsNullOrEmpty(mailid))
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }

            SnehBLL.ReportBotoxMst_Bll RDB = new SnehBLL.ReportBotoxMst_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataTable dt = RDB.GetTop(_appointmentID);
            DataTable dt1 = RDB.Get1(_appointmentID);
            DataTable dt2 = RDB.Get2(_appointmentID);
            DataTable dt3 = RDB.Get3(_appointmentID);
            Document document = new Document(PageSize.A4, 20f, 20f, 50f, 20f);
            Font NormalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("Botox Report - " + dt.Rows[0]["FullName"].ToString()) + ".pdf";

                string result_sheet = HttpContext.Current.Server.MapPath("~/Files/Receipt/") + _fileName;
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(result_sheet))) { Directory.CreateDirectory(System.IO.Path.GetDirectoryName(result_sheet)); }
                if (File.Exists(result_sheet)) { try { File.Delete(result_sheet); } catch { } }

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.PageEvent = new ITextEvents();
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                PdfPTable ntable = null;
                iTextSharp.text.BaseColor color = null;
                DataTable dtA;

                document.Open();

                //Header Table
                table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 0.3f, 0.7f });
                cell = ImageCell("~/images/snehlogo_small.png", 25f, PdfPCell.ALIGN_LEFT);
                table.AddCell(cell);
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                //color = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
                color = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f, color);
                DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, color);
                document.Add(table);

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("General Information :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);


                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 36f, 3f, 11f, 2f, 35f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("Full Name", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["FullName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Botox No.", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt1.Rows[0]["General_BotoxNo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                table.AddCell(PhraseCell(new Phrase("MrNo", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["MrNo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                table.AddCell(PhraseCell(new Phrase("Birth Date", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(DbHelper.Configuration.FORMATDATE(dt.Rows[0]["BirthDate"].ToString()), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Age", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["AgeYear"].ToString() + " Year " + dt.Rows[0]["AgeMonth"].ToString() + " Month", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                table.AddCell(PhraseCell(new Phrase("Address", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(dt.Rows[0]["cAddress"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 5;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                table.AddCell(PhraseCell(new Phrase("Mobile No", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["MobileNo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Telephone No", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["TelephoneNo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                table.AddCell(PhraseCell(new Phrase("E-Mail", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt.Rows[0]["MailID"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Botox Date", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(DbHelper.Configuration.FORMATDATE(dt.Rows[0]["AppointmentDate"].ToString()), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                int _pediatricianID = 0; string _pediatrician = ""; string _pediatricianNo = ""; int.TryParse(dt1.Rows[0]["General_Pediatrician"].ToString(), out _pediatricianID);
                DMD = DMB.Get(_pediatricianID); if (DMD != null) { _pediatrician = DMD.PreFix + " " + DMD.FullName; _pediatricianNo = DMD.MobileNo; }
                table.AddCell(PhraseCell(new Phrase("Pediatrician", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_pediatrician, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Phone No", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_pediatricianNo, NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                int _therapistID = 0; string _therapist = ""; string _therapistNo = ""; int.TryParse(dt1.Rows[0]["General_Therapist"].ToString(), out _therapistID);
                DMD = DMB.Get(_therapistID); if (DMD != null) { _therapist = DMD.PreFix + " " + DMD.FullName; _therapistNo = DMD.MobileNo; }
                table.AddCell(PhraseCell(new Phrase("Therapist", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_therapist, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalBold), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Phone No", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_therapistNo, NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);



                bool DiagnosisNames = false;
                if (dt1.Rows[0]["DiagnosisNames"].ToString().Trim().Length > 0 || dt1.Rows[0]["DiagnosisOther"].ToString().Trim().Length > 0)
                {
                    DiagnosisNames = true;
                }
                if (DiagnosisNames)
                {
                    string Diagnosis = dt1.Rows[0]["DiagnosisNames"].ToString().Trim();
                    if (string.IsNullOrEmpty(Diagnosis))
                    {
                        Diagnosis = dt1.Rows[0]["DiagnosisOther"].ToString().Trim();
                    }
                    else
                    {
                        if (Diagnosis.Trim().EndsWith(","))
                        {
                            Diagnosis = Diagnosis.Substring(0, Diagnosis.LastIndexOf(","));// +".";
                        }
                    }
                    table.AddCell(PhraseCell(new Phrase("Diagnosis", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(Diagnosis, NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 5;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 7;
                    cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                    table.AddCell(cell);
                }
                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("History and Exam :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 36f, 3f, 11f, 2f, 35f });
                table.SpacingBefore = 20f;

                int _historyExam_DeliveryID = 0; string _historyExam_Delivery = ""; int.TryParse(dt1.Rows[0]["HistoryExam_Delivery"].ToString(), out _historyExam_DeliveryID);
                _historyExam_Delivery = RDB.Delivery_Get(_historyExam_DeliveryID);
                table.AddCell(PhraseCell(new Phrase("Delivery", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_historyExam_Delivery, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Birth Weight", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt1.Rows[0]["HistoryExam_BirthWeight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);


                table.AddCell(PhraseCell(new Phrase("Perinatal Complications", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(dt1.Rows[0]["HistoryExam_PerinatalComplications"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Milestones", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                ntable = new PdfPTable(1);
                ntable.WidthPercentage = 100;
                dtA = RDB.GetAttr(_appointmentID, RDB._milestonesTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.Milestones_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            ntable.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                        }
                    }
                }
                cell = new PdfPCell(ntable);
                cell.Border = 0;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                int _diagnosedByID = 0; string _diagnosedBy = ""; int.TryParse(dt1.Rows[0]["HistoryExam_DiagnosedBy"].ToString(), out _diagnosedByID);
                _diagnosedBy = RDB.Diagnosed_Get(_diagnosedByID);
                table.AddCell(PhraseCell(new Phrase("Diagnosed By", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_diagnosedBy, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                document.Add(table);

                #endregion
                document.Add(Chunk.NEXTPAGE);
                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Type Of CP :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 36f, 3f, 11f, 2f, 35f });
                table.SpacingBefore = 20f;

                int _TypeOfCP_CPID = 0; string _TypeOfCP_CP = ""; int.TryParse(dt1.Rows[0]["TypeOfCP_CPID"].ToString(), out _TypeOfCP_CPID);
                _TypeOfCP_CP = RDB.TypeOfCP_Get(_TypeOfCP_CPID);
                table.AddCell(PhraseCell(new Phrase("Type", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_TypeOfCP_CP, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                //document.Add(Chunk.NEXTPAGE);

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Assistive Devices and Mobility Aids/Orthotics :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 36f, 3f, 11f, 2f, 35f });
                table.SpacingBefore = 20f;

                int _AssistiveDevices_OrthoticsID = 0; string _AssistiveDevices_Orthotics = ""; int.TryParse(dt1.Rows[0]["AssistiveDevices_Orthotics"].ToString(), out _AssistiveDevices_OrthoticsID);
                _AssistiveDevices_Orthotics = RDB.Orthotics_Get(_AssistiveDevices_OrthoticsID);
                table.AddCell(PhraseCell(new Phrase("Orthotics", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_AssistiveDevices_Orthotics, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                dtA = RDB.GetAttr(_appointmentID, RDB._assistiveDevicesTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.AssistiveDevices_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 7;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                dtA = RDB.GetAttr(_appointmentID, RDB._orthoticsDevicesTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.OrthoticsDevices_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 7;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("ADL :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 36f, 3f, 11f, 2f, 35f });
                table.SpacingBefore = 20f;

                int _ADLID = 0; string _ADL = ""; int.TryParse(dt1.Rows[0]["ADL_adlID"].ToString(), out _ADLID);
                _ADL = RDB.ADL_Get(_ADLID);
                table.AddCell(PhraseCell(new Phrase("ADL", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(_ADL, NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 7;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                dtA = RDB.GetAttr(_appointmentID, RDB._ADLListTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.ADLList_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 7;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Ambulation :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(7);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 14.28f, 14.28f, 14.28f, 14.28f, 14.28f, 14.28f, 14.28f });
                table.SpacingBefore = 20f;

                string Ambulation_Date1 = ""; string Ambulation_Date2 = ""; string Ambulation_Date3 = ""; string Ambulation_Date4 = ""; string Ambulation_Date5 = ""; string Ambulation_Date6 = "";
                DateTime _Ambulation_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date1);
                if (_Ambulation_Date1 > DateTime.MinValue)
                    Ambulation_Date1 = _Ambulation_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Ambulation_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date2);
                if (_Ambulation_Date2 > DateTime.MinValue)
                    Ambulation_Date2 = _Ambulation_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Ambulation_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date3);
                if (_Ambulation_Date3 > DateTime.MinValue)
                    Ambulation_Date3 = _Ambulation_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Ambulation_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date4);
                if (_Ambulation_Date4 > DateTime.MinValue)
                    Ambulation_Date4 = _Ambulation_Date4.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Ambulation_Date5 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date5"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date5);
                if (_Ambulation_Date5 > DateTime.MinValue)
                    Ambulation_Date5 = _Ambulation_Date5.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Ambulation_Date6 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Ambulation_Date6"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Ambulation_Date6);
                if (_Ambulation_Date6 > DateTime.MinValue)
                    Ambulation_Date6 = _Ambulation_Date6.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("DATE", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date5, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Ambulation_Date6, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);


                cell = PhraseCell(new Phrase("AMBULATION", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Ambulation_Amb6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.1f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10f, 90f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("1. Independent Community Ambulatory, no devices or wheelchair.", NormalFont), PdfPCell.ALIGN_LEFT));

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("2. Independent household ambulatory community ambulatory with device or w/c<50% or time.", NormalFont), PdfPCell.ALIGN_LEFT));

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("3. Household and limited community ambulatory uses w/c>50% of time.", NormalFont), PdfPCell.ALIGN_LEFT));

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("4. Household or exercise ambulatory,all mobility with walker or w/c.", NormalFont), PdfPCell.ALIGN_LEFT));

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("5. Primary wheelchair user performs assisted weight bearing transfers.", NormalFont), PdfPCell.ALIGN_LEFT));

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("6. Primary wheelchair user requires dependent non-weight bearing transfers.", NormalFont), PdfPCell.ALIGN_LEFT));

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Pre-Existing Deformity :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f });
                table.SpacingBefore = 20f;

                string PreExisting_Date1 = ""; string PreExisting_Date2 = ""; string PreExisting_Date3 = ""; string PreExisting_Date4 = "";
                DateTime _PreExisting_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date1);
                if (_PreExisting_Date1 > DateTime.MinValue)
                    PreExisting_Date1 = _PreExisting_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PreExisting_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date2);
                if (_PreExisting_Date2 > DateTime.MinValue)
                    PreExisting_Date2 = _PreExisting_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PreExisting_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date3);
                if (_PreExisting_Date3 > DateTime.MinValue)
                    PreExisting_Date3 = _PreExisting_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PreExisting_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PreExisting_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PreExisting_Date4);
                if (_PreExisting_Date4 > DateTime.MinValue)
                    PreExisting_Date4 = _PreExisting_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(PreExisting_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PreExisting_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PreExisting_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PreExisting_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip FID", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipFID_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Adduction", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_HipAdduction_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Knee FFD", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_KneeFFD_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Equinus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Equinus_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Planovalgoid", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Planovalgoid_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Cavovarus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_Cavovarus_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Elbow FFD", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_ElbowFFD_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Wrist Flex-Pron", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PreExisting_WristFlexPron_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Passive ROM :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f });
                table.SpacingBefore = 20f;

                string PassiveROM_Date1 = ""; string PassiveROM_Date2 = ""; string PassiveROM_Date3 = ""; string PassiveROM_Date4 = "";
                DateTime _PassiveROM_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date1);
                if (_PassiveROM_Date1 > DateTime.MinValue)
                    PassiveROM_Date1 = _PassiveROM_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PassiveROM_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date2);
                if (_PassiveROM_Date2 > DateTime.MinValue)
                    PassiveROM_Date2 = _PassiveROM_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PassiveROM_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date3);
                if (_PassiveROM_Date3 > DateTime.MinValue)
                    PassiveROM_Date3 = _PassiveROM_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _PassiveROM_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["PassiveROM_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _PassiveROM_Date4);
                if (_PassiveROM_Date4 > DateTime.MinValue)
                    PassiveROM_Date4 = _PassiveROM_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(PassiveROM_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PassiveROM_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PassiveROM_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(PassiveROM_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Flexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipFlexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Abduction", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipAbduction_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip IR", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipIR_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip ER", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_HipER_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Knee Flexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Popliteal Angle", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_PoplitealAngle_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Dorsi(Knee ext)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeExt_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Dorsi(Knee flex)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_KneeFlex_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Plantarflexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_Plantarflexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Ankle Inv", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleInv_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Ankle Ever", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["PassiveROM_AnkleEver_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Ton :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f, 11.11f });
                table.SpacingBefore = 20f;

                string Tone_Date1 = ""; string Tone_Date2 = ""; string Tone_Date3 = ""; string Tone_Date4 = "";
                DateTime _Tone_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date1);
                if (_Tone_Date1 > DateTime.MinValue)
                    Tone_Date1 = _Tone_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Tone_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date2);
                if (_Tone_Date2 > DateTime.MinValue)
                    Tone_Date2 = _Tone_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Tone_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date3);
                if (_Tone_Date3 > DateTime.MinValue)
                    Tone_Date3 = _Tone_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Tone_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["Tone_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Tone_Date4);
                if (_Tone_Date4 > DateTime.MinValue)
                    Tone_Date4 = _Tone_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(Tone_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Tone_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Tone_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(Tone_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Iliopsoas", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Iliopsoas_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Adductors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Adductors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Rectus Femoris", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_RectusFemoris_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hamstrings", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Hamstrings_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gastrosoleus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_Gastrosoleus_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Elbow Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_ElbowFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Wrist Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_WristFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Finger Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_FingerFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Pronator Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["Tone_PronatorFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Tardieus Scale :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.99f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f });
                table.SpacingBefore = 20f;

                string TardieusScale_Date1 = ""; string TardieusScale_Date2 = ""; string TardieusScale_Date3 = ""; string TardieusScale_Date4 = "";
                DateTime _TardieusScale_Date1 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date1);
                if (_TardieusScale_Date1 > DateTime.MinValue)
                    TardieusScale_Date1 = _TardieusScale_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _TardieusScale_Date2 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date2);
                if (_TardieusScale_Date2 > DateTime.MinValue)
                    TardieusScale_Date2 = _TardieusScale_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _TardieusScale_Date3 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date3);
                if (_TardieusScale_Date3 > DateTime.MinValue)
                    TardieusScale_Date3 = _TardieusScale_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _TardieusScale_Date4 = new DateTime(); DateTime.TryParseExact(dt1.Rows[0]["TardieusScale_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _TardieusScale_Date4);
                if (_TardieusScale_Date4 > DateTime.MinValue)
                    TardieusScale_Date4 = _TardieusScale_Date4.ToString(DbHelper.Configuration.showDateFormat);
                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gastrosoleus R1", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR1_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gastrosoleus R2", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR2_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gastrosoleus R3", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_GastrosoleusR3_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hamstrings R1", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR1_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hamstrings R2", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt1.Rows[0]["TardieusScale_HamstringsR2_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Muscle Strength :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.99f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f });
                table.SpacingBefore = 20f;

                string MuscleStrength_Date1 = ""; string MuscleStrength_Date2 = ""; string MuscleStrength_Date3 = ""; string MuscleStrength_Date4 = "";
                DateTime _MuscleStrength_Date1 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date1);
                if (_MuscleStrength_Date1 > DateTime.MinValue)
                    MuscleStrength_Date1 = _MuscleStrength_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _MuscleStrength_Date2 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date2);
                if (_MuscleStrength_Date2 > DateTime.MinValue)
                    MuscleStrength_Date2 = _MuscleStrength_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _MuscleStrength_Date3 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date3);
                if (_MuscleStrength_Date3 > DateTime.MinValue)
                    MuscleStrength_Date3 = _MuscleStrength_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _MuscleStrength_Date4 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["MuscleStrength_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _MuscleStrength_Date4);
                if (_MuscleStrength_Date4 > DateTime.MinValue)
                    MuscleStrength_Date4 = _MuscleStrength_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Iliopsoas", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Iliopsoas_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gluteus Max", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_GluteusMax_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Abductors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Abductors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Rectus Femoris", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_RectusFemoris_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hamstrings", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Hamstrings_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Gastrosoleus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_Gastrosoleus_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Tibialis Ant", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_TibialisAnt_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Elbow Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_ElbowFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Pronator Teres", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_PronatorTeres_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Wrist Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Wrist Extensors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_WristExtensors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Finger Flexors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["MuscleStrength_FingerFlexors_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Voluntary Control :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(9);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11.99f, 11f, 11f, 11f, 11f, 11f, 11f, 11f, 11f });
                table.SpacingBefore = 20f;

                string Voluntary_Date1 = ""; string Voluntary_Date2 = ""; string Voluntary_Date3 = ""; string Voluntary_Date4 = "";
                DateTime _Voluntary_Date1 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date1);
                if (_Voluntary_Date1 > DateTime.MinValue)
                    Voluntary_Date1 = _Voluntary_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Voluntary_Date2 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date2);
                if (_Voluntary_Date2 > DateTime.MinValue)
                    Voluntary_Date2 = _Voluntary_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Voluntary_Date3 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date3);
                if (_Voluntary_Date3 > DateTime.MinValue)
                    Voluntary_Date3 = _Voluntary_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _Voluntary_Date4 = new DateTime(); DateTime.TryParseExact(dt2.Rows[0]["Voluntary_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _Voluntary_Date4);
                if (_Voluntary_Date4 > DateTime.MinValue)
                    Voluntary_Date4 = _Voluntary_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(TardieusScale_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Flexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipFlexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Extension", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipExtension_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Hip Abduction", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_HipAbduction_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Knee Flexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeFlexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Knee Extension", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_KneeExtension_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Dorsiflexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Dorsiflexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Plantarflexion", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Plantarflexion_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Wrist Dorsiflex", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_WristDorsiflex_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Grasp", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Grasp_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Release", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f; table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_1R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_1L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_2R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_2L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_3R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_3L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_4R"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt2.Rows[0]["Voluntary_Release_4L"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Functional Strength :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(5);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 30f, 17.5f, 17.5f, 17.5f, 17.5f });
                table.SpacingBefore = 20f;

                string FunctionalStrength_Date1 = ""; string FunctionalStrength_Date2 = ""; string FunctionalStrength_Date3 = ""; string FunctionalStrength_Date4 = "";
                DateTime _FunctionalStrength_Date1 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date1);
                if (_FunctionalStrength_Date1 > DateTime.MinValue)
                    FunctionalStrength_Date1 = _FunctionalStrength_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _FunctionalStrength_Date2 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date2);
                if (_FunctionalStrength_Date2 > DateTime.MinValue)
                    FunctionalStrength_Date2 = _FunctionalStrength_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _FunctionalStrength_Date3 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date3);
                if (_FunctionalStrength_Date3 > DateTime.MinValue)
                    FunctionalStrength_Date3 = _FunctionalStrength_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _FunctionalStrength_Date4 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["FunctionalStrength_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _FunctionalStrength_Date4);
                if (_FunctionalStrength_Date4 > DateTime.MinValue)
                    FunctionalStrength_Date4 = _FunctionalStrength_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(FunctionalStrength_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(FunctionalStrength_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(FunctionalStrength_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(FunctionalStrength_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Pull to Stand", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_PullStand_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_PullStand_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_PullStand_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_PullStand_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Independent Standing Arms Free(3 sec)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent3Sec_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent3Sec_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent3Sec_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent3Sec_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Independent Standing Arms Free(20 sec)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent20Sec_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent20Sec_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent20Sec_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Independent20Sec_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("One Leg Stance Hand Held : R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldR_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldR_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldR_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldR_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("One Leg Stance Hand Held : L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldL_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldL_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldL_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HandHeldL_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("One Leg Stance : R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegR_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegR_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegR_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegR_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("One Leg Stance : L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegL_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegL_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegL_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_OneLegL_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Short Sit to Stand", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_ShortSit_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_ShortSit_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_ShortSit_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_ShortSit_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("High Knee to Stand : R", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeR_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeR_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeR_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeR_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("High Knee to Stand : L", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeL_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeL_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeL_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_HighKneeL_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Lowers to Floor", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_LowersFloor_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_LowersFloor_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_LowersFloor_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_LowersFloor_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Squats", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Squats_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Squats_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Squats_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Squats_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Standing-Picks Pen from Floor", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_StandingPicks_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_StandingPicks_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_StandingPicks_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_StandingPicks_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Total(39)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Total_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Total_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Total_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["FunctionalStrength_Total_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);


                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10f, 10f, 40f, 40f });
                table.SpacingBefore = 10f;

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("** Key : ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("0 = Cannot Initiate", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("1 = Initiates Independently", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("2 = Partially Completes", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("3 = Completes Independently", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Indication For Botox :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 87f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("Indications", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                dtA = RDB.GetAttr(_appointmentID, RDB._indicationForBotoxTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.IndicationForBotox_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            if (i != 0)
                            {
                                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            }
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 3;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Botox Data :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(5);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 30f, 17.5f, 17.5f, 17.5f, 17.5f });
                table.SpacingBefore = 20f;

                string BotoxData_Date1 = ""; string BotoxData_Date2 = ""; string BotoxData_Date3 = ""; string BotoxData_Date4 = "";
                DateTime _BotoxData_Date1 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date1"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date1);
                if (_BotoxData_Date1 > DateTime.MinValue)
                    BotoxData_Date1 = _BotoxData_Date1.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _BotoxData_Date2 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date2"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date2);
                if (_BotoxData_Date2 > DateTime.MinValue)
                    BotoxData_Date2 = _BotoxData_Date2.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _BotoxData_Date3 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date3"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date3);
                if (_BotoxData_Date3 > DateTime.MinValue)
                    BotoxData_Date3 = _BotoxData_Date3.ToString(DbHelper.Configuration.showDateFormat);
                DateTime _BotoxData_Date4 = new DateTime(); DateTime.TryParseExact(dt3.Rows[0]["BotoxData_Date4"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _BotoxData_Date4);
                if (_BotoxData_Date4 > DateTime.MinValue)
                    BotoxData_Date4 = _BotoxData_Date4.ToString(DbHelper.Configuration.showDateFormat);

                cell = PhraseCell(new Phrase("Date", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(BotoxData_Date1, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(BotoxData_Date2, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(BotoxData_Date3, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(BotoxData_Date4, NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("1. Weight", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Weight_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Weight_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Weight_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Weight_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("2. Total Does of Botox Injected(Units)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_BotoxInjected_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_BotoxInjected_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_BotoxInjected_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_BotoxInjected_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("3. Dilution(ml) in 0.9% NS", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Dilution_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Dilution_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Dilution_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Dilution_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("4. Muscles Injected(vol)", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_MusclesInjected_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_MusclesInjected_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_MusclesInjected_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_MusclesInjected_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Gastocnemius-Soleus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Gastocnemius_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Gastocnemius_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Gastocnemius_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Gastocnemius_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Tibialis Posterior", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Tibialis_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Tibialis_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Tibialis_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Tibialis_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Hamstrings", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Hamstrings_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Hamstrings_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Hamstrings_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Hamstrings_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Adductors", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductors_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductors_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductors_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductors_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Rectus", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Rectus_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Rectus_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Rectus_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Rectus_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Iliopsoas", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Iliopsoas_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Iliopsoas_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Iliopsoas_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Iliopsoas_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Pronator Teres", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Pronator_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Pronator_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Pronator_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Pronator_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 FCR", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCR_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCR_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCR_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCR_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 FCU", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCU_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCU_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCU_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FCU_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 FDS", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDS_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDS_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDS_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDS_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 FDP", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDP_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDP_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDP_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FDP_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 FPL", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FPL_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FPL_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FPL_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_FPL_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Adductor", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductor_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductor_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductor_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Adductor_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("   0 Intrinsics", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Intrinsics_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Intrinsics_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Intrinsics_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Intrinsics_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("5. Casting", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Casting_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Casting_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Casting_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(dt3.Rows[0]["BotoxData_Casting_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT);
                cell.BorderWidth = 0.3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY; cell.Padding = 5f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Ancillary Treatment :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 87f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("Treatment", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                dtA = RDB.GetAttr(_appointmentID, RDB._ancillaryTreatmentTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.AncillaryTreatment_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            if (i != 0)
                            {
                                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            }
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 3;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                document.Add(table);

                #endregion

                #region
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("Side Effects :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 11f, 2f, 87f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("Effects", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                dtA = RDB.GetAttr(_appointmentID, RDB._SideEffectsTypeID);
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    int _attrID = 0; int.TryParse(dtA.Rows[i]["AttributeID"].ToString(), out _attrID);
                    if (_attrID > 0)
                    {
                        string _milestone = RDB.SideEffects_Get(_attrID);
                        if (_milestone.Length > 0)
                        {
                            if (i != 0)
                            {
                                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                                table.AddCell(PhraseCell(new Phrase("", ColonFont), PdfPCell.ALIGN_LEFT));
                            }
                            table.AddCell(PhraseCell(new Phrase(_milestone, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                            cell.Colspan = 3;
                            cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                            table.AddCell(cell);
                        }
                    }
                }
                document.Add(table);

                #endregion

                #region

                int _Doctor_Physioptherapist = 0; string Doctor_Physioptherapist = ""; int.TryParse(dt3.Rows[0]["Doctor_Physiotheraist"].ToString(), out _Doctor_Physioptherapist);
                DMD = DMB.Get(_Doctor_Physioptherapist); if (DMD != null) { Doctor_Physioptherapist = DMD.PreFix + " " + DMD.FullName; }

                int _Doctor_Occupational = 0; string Doctor_Occupational = ""; int.TryParse(dt3.Rows[0]["Doctor_Occupational"].ToString(), out _Doctor_Occupational);
                DMD = DMB.Get(_Doctor_Occupational); if (DMD != null) { Doctor_Occupational = DMD.PreFix + " " + DMD.FullName; }

                int _Doctor_Director = 0; string Doctor_Director = ""; int.TryParse(dt3.Rows[0]["Doctor_Director"].ToString(), out _Doctor_Director);
                DMD = DMB.Get(_Doctor_Director); if (DMD != null) { Doctor_Director = DMD.PreFix + " " + DMD.FullName; }



                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 33.33f, 33.33f, 33.33f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3; cell.BorderColorTop = BaseColor.GRAY; cell.BorderWidthTop = 0.3f;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                Doctor_Director = "DR SNEHAL DESHPANDE";
                table.AddCell(PhraseCell(new Phrase(Doctor_Director, NormalItalic), PdfPCell.ALIGN_CENTER));
                //if (Doctor_Director.Length > 0)
                //    table.AddCell(PhraseCell(new Phrase(Doctor_Director, NormalItalic), PdfPCell.ALIGN_CENTER));
                //else
                //    table.AddCell(PhraseCell(new Phrase("", NormalItalic), PdfPCell.ALIGN_CENTER));
                if (Doctor_Physioptherapist.Length > 0)
                    table.AddCell(PhraseCell(new Phrase(Doctor_Physioptherapist, NormalItalic), PdfPCell.ALIGN_CENTER));
                else
                    table.AddCell(PhraseCell(new Phrase("", NormalItalic), PdfPCell.ALIGN_CENTER));
                if (Doctor_Occupational.Length > 0)
                    table.AddCell(PhraseCell(new Phrase(Doctor_Occupational, NormalItalic), PdfPCell.ALIGN_CENTER));
                else
                    table.AddCell(PhraseCell(new Phrase("", NormalItalic), PdfPCell.ALIGN_CENTER));

                if (Doctor_Director.Length > 0)
                {
                    //cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell = ImageCell("~/images/snehalsign.jpg", 20f, PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    //cell.FixedHeight = 5f;
                    cell.PaddingBottom = 0f; cell.PaddingTop = 10f;

                    table.AddCell(cell);
                }
                if (Doctor_Physioptherapist.Length > 0)
                {
                    cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    //cell.FixedHeight=5f;
                    cell.PaddingBottom = 0f; cell.PaddingTop = 10f;
                    table.AddCell(cell);
                }
                if (Doctor_Occupational.Length > 0)
                {
                    cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    //cell.FixedHeight = 5f;
                    cell.PaddingBottom = 0f; cell.PaddingTop = 10f;
                    table.AddCell(cell);
                }

                table.AddCell(PhraseCell(new Phrase("DIRECTOR SNEH RERC", NormalBold), PdfPCell.ALIGN_CENTER));
                table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));
                table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));

                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                document.Add(table);

                #endregion

                //document.Close();
                //byte[] bytes = memoryStream.ToArray();
                //memoryStream.Close();
                //context.Response.Clear();
                //context.Response.ContentType = "application/pdf";
                //context.Response.AddHeader("Content-Disposition", "inline; filename=botox.pdf");
                //context.Response.ContentType = "application/pdf";
                //context.Response.Buffer = true;
                //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //context.Response.BinaryWrite(bytes);
                //context.Response.End();
                //context.Response.Close();
                document.Close();

                byte[] buffer = new byte[0]; buffer = memoryStream.GetBuffer();
                var contentLength = buffer.Length;
                memoryStream.Close();
                File.WriteAllBytes(result_sheet, buffer);

                //send mail code (use sendAttachment in apimail in file pass result_sheet)
                bool mail = false;
                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                mail = AM.sendAttachment(mailid, "", "Botox Report", result_sheet);
                if (mail)
                {
                    DbHelper.SqlDb db = new DbHelper.SqlDb();
                    SqlCommand cmd = new SqlCommand("UPDATE Report_BotoxMst1 SET MailSend=CAST('True'AS BIT) WHERE AppointmentID=@AppointmentID");
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
                    int i = db.DbUpdate(cmd);
                    if (i > 0)
                    {
                        r.status = true; r.msg = "Send successfully.";
                        context.Response.Write(JsonConvert.SerializeObject(r));
                        return;
                    }
                    else
                    {
                        r.msg = "Unable to process, Please try again.";
                        context.Response.Write(JsonConvert.SerializeObject(r));
                        return;
                    }
                }
                else
                {
                    r.msg = "Unable to process, Please try again.";
                    context.Response.Write(JsonConvert.SerializeObject(r));
                    return;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class BotoxReport
        {
            public string MailID { get; set; }
            public int SiAppointmentID { get; set; }
        }

        private string TIMEDURATION(string _durationStr, string _timeStr)
        {
            int _duration = 0; int.TryParse(_durationStr, out _duration);
            DateTime TimeHourD = new DateTime();
            DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
            {
                return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + "  To  " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "- - -";
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, iTextSharp.text.BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }

        public class PDFFooter : PdfPageEventHelper
        {
            int pageNo = 0;

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                if (pageNo != 0)
                {
                    PdfPTable table = new PdfPTable(1);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    Font NormalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    PdfPCell cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.PaddingTop = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                }
                pageNo++;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
            }
        }

        public class ITextEvents : PdfPageEventHelper
        {
            // This is the contentbyte object of the writer
            PdfContentByte cb;
            int pageNo = 0;
            // we will put the final number of pages in a template
            PdfTemplate headerTemplate, footerTemplate;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;

            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {
                }
                catch (System.IO.IOException ioe)
                {
                }
            }

            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);

                iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                //Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);
                Phrase p1Header = new Phrase();

                //Create PdfTable object
                PdfPTable pdfTab = new PdfPTable(3);

                //We will have to create separate cells to include image logo and 2 separate strings
                //Row 1
                PdfPCell pdfCell1 = new PdfPCell();
                PdfPCell pdfCell2 = new PdfPCell(p1Header);
                PdfPCell pdfCell3 = new PdfPCell();
                String text = "Page " + writer.PageNumber + " of ";

                //Add paging to header
                {
                    if (pageNo > 0)
                    {
                        Image imgSoc = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/theme3.jpg"));
                        imgSoc.ScaleToFit(110, 110);
                        imgSoc.SetAbsolutePosition(510, 810);
                        imgSoc.ScaleAbsoluteWidth(70F);
                        cb.AddImage(imgSoc);
                    }

                }
                //Add paging to footer
                {
                    if (pageNo == 0)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 12);
                        cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                        Image img = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/sneh_address2.png"));
                        img.ScalePercent(30f); int x = 10, y = 10;

                        img.SetAbsolutePosition(x, y);
                        img.ScaleAbsoluteWidth(570F);
                        cb.AddImage(img);
                        cb.ShowText("");
                        cb.EndText();
                        float len = bf.GetWidthPoint("", 12);
                        cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                    }
                    else
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 12);
                        cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                        cb.ShowText("");
                        cb.EndText();
                        float len = bf.GetWidthPoint("", 12);
                        cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));

                    }

                }

                //Row 2
                //PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));

                //Row 3 
                //PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
                PdfPCell pdfCell6 = new PdfPCell();
                //PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));

                //set the alignment of all three cells and set border to 0
                pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
                //pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                //pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
                //pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
                //pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

                pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                //pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
                //pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
                //pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
                //pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

                // pdfCell4.Colspan = 3;

                pdfCell1.Border = 0;
                pdfCell2.Border = 0;
                pdfCell3.Border = 0;
                //pdfCell4.Border = 0;
                //pdfCell5.Border = 0;
                //pdfCell6.Border = 0;
                //pdfCell7.Border = 0;

                //add all three cells into PdfTable
                pdfTab.AddCell(pdfCell1);
                pdfTab.AddCell(pdfCell2);
                pdfTab.AddCell(pdfCell3);
                //pdfTab.AddCell(pdfCell4);
                //pdfTab.AddCell(pdfCell5);
                //pdfTab.AddCell(pdfCell6);
                //pdfTab.AddCell(pdfCell7);

                pdfTab.TotalWidth = document.PageSize.Width - 80f;
                pdfTab.WidthPercentage = 70;
                //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;    

                //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
                //first param is start row. -1 indicates there is no end row and all the rows to be included to write
                //Third and fourth param is x and y position to start writing
                pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
                //set pdfContent value

                //Move the pointer and draw line to separate header section from rest of page
                //cb.MoveTo(40, document.PageSize.Height - 100);
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                //cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                if (pageNo == 0)
                {
                    cb.MoveTo(50, document.PageSize.GetBottom(120));
                    cb.LineTo(document.PageSize.Width - 50, document.PageSize.GetBottom(120));
                    //cb.Stroke();
                }
                else
                {
                    cb.MoveTo(40, document.PageSize.GetBottom(60));
                    cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(60));
                    //cb.Stroke();
                }

                pageNo++;
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 12);
                headerTemplate.SetTextMatrix(0, 0);
                //headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.ShowText("");
                headerTemplate.EndText();

                footerTemplate.BeginText();
                footerTemplate.SetFontAndSize(bf, 12);
                footerTemplate.SetTextMatrix(0, 0);
                //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                footerTemplate.EndText();
            }
        }
    }
}