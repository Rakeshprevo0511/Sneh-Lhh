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
    /// Summary description for Si_Report_MailPDF
    /// </summary>
    public class Si_Report_MailPDF : IHttpHandler, IRequiresSessionState
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
            MailSIReport evnt = jsonSerializer.Deserialize<MailSIReport>(requestBody);
            int.TryParse(evnt.SiAppointmentID.ToString(), out _appointmentID);
            mailid = evnt.MailID;

            if (_appointmentID <= 0 || string.IsNullOrEmpty(mailid))
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.ReportSiMst_Bll RNB = new SnehBLL.ReportSiMst_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataSet ds = RNB.Get(_appointmentID);
            Document document = new Document(PageSize.A4, 30f, 30f, 50f, 30f);
            Font NormalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            Font SubHeadingFont = FontFactory.GetFont("Arial", 10, Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font NextHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("SI Report - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

                string _fileNameNew = DbHelper.Configuration.MakeValidFilename("Eval Report - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

                string result_sheet = HttpContext.Current.Server.MapPath("~/Files/Receipt/") + _fileNameNew;
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(result_sheet))) { Directory.CreateDirectory(System.IO.Path.GetDirectoryName(result_sheet)); }
                if (File.Exists(result_sheet)) { try { File.Delete(result_sheet); } catch { } }

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.PageEvent = new ITextEvents();
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                iTextSharp.text.BaseColor color = null;

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

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("     Patient Information :", HeadingFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.PaddingBottom = 30f;
                table.AddCell(cell);
                document.Add(table);

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 20f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Full Name", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["FullName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("MrNo", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["MrNo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Date Of Birth", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["BirthDate"].ToString()), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                #region
                if (ds.Tables[0].Rows[0]["FatherName"].ToString().Length > 0)
                {
                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 10f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Parents Name", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["FatherName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);
                }
                #endregion

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Session", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["SessionName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Evaluation Date", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["AppointmentDate"].ToString()), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                bool DiagnosisNames = false;
                if (ds.Tables[1].Rows[0]["DiagnosisNames"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["DiagnosisOther"].ToString().Trim().Length > 0)
                {
                    DiagnosisNames = true;
                }
                if (DiagnosisNames)
                {
                    string Diagnosis = ds.Tables[1].Rows[0]["DiagnosisNames"].ToString().Trim();
                    string DiagnosisOther = ds.Tables[1].Rows[0]["DiagnosisOther"].ToString();
                    if (!string.IsNullOrEmpty(DiagnosisOther))
                    {
                        Diagnosis += ", " + DiagnosisOther;
                    }
                    Diagnosis = Diagnosis.Trim();
                    if (Diagnosis.EndsWith(","))
                    {
                        Diagnosis = Diagnosis.Substring(0, Diagnosis.LastIndexOf(","));// +".";
                    }

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 10f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Diagnosis", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(Diagnosis, NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                }


                #region
                /*
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Duration", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["Duration"].ToString() + " Min", NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                #endregion

                #region
                table = new PdfPTable(4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                table.SpacingBefore = 10f;

                table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase("Time", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(TIMEDURATION(ds.Tables[0].Rows[0]["Duration"].ToString(), ds.Tables[0].Rows[0]["AppointmentTime"].ToString()), NormalFont), PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);
                document.Add(table);
                */
                #endregion

                #region
                DataTable dtT = RNB.GetTherapist(_appointmentID);
                for (int i = 0; i < dtT.Rows.Count; i++)
                {
                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 10f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    if (i == 0)
                    {
                        table.AddCell(PhraseCell(new Phrase("Therapist", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    }
                    else
                    {
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    }
                    table.AddCell(PhraseCell(new Phrase(dtT.Rows[i]["FullName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);
                }
                #endregion

                #region ************** Data Collection *********************
                bool DataCollection_Referral = false; if (ds.Tables[1].Rows[0]["DataCollection_Referral"].ToString().Trim().Length > 0) { DataCollection_Referral = true; }
                bool DataCollection_MedicalHistory = false; if (ds.Tables[1].Rows[0]["DataCollection_MedicalHistory"].ToString().Trim().Length > 0) { DataCollection_MedicalHistory = true; }
                bool DataCollection_DailyRoutine = false; if (ds.Tables[1].Rows[0]["DataCollection_DailyRoutine"].ToString().Trim().Length > 0) { DataCollection_DailyRoutine = true; }
                bool DataCollection_Expectaion = false; if (ds.Tables[1].Rows[0]["DataCollection_Expectaion"].ToString().Trim().Length > 0) { DataCollection_Expectaion = true; }
                bool DataCollection_TherapyHistory = false; if (ds.Tables[1].Rows[0]["DataCollection_TherapyHistory"].ToString().Trim().Length > 0) { DataCollection_TherapyHistory = true; }
                bool DataCollection_Sources = false; if (ds.Tables[1].Rows[0]["DataCollection_Sources"].ToString().Trim().Length > 0) { DataCollection_Sources = true; }
                bool DataCollection_NumberVisit = false; if (ds.Tables[1].Rows[0]["DataCollection_NumberVisit"].ToString().Trim().Length > 0) { DataCollection_NumberVisit = true; }
                bool DataCollection_AdaptedEquipment = false; if (ds.Tables[1].Rows[0]["DataCollection_AdaptedEquipment"].ToString().Trim().Length > 0) { DataCollection_AdaptedEquipment = true; }

                if (DataCollection_Referral || DataCollection_MedicalHistory || DataCollection_DailyRoutine || DataCollection_Expectaion || DataCollection_TherapyHistory || DataCollection_Sources || DataCollection_NumberVisit || DataCollection_AdaptedEquipment)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("    Other Information :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (DataCollection_Referral)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Birth History :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_Referral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        document.Add(Chunk.NEXTPAGE);
                    }
                    #endregion

                    #region
                    if (DataCollection_MedicalHistory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Medical History :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_MedicalHistory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    //document.Add(Chunk.NEXTPAGE);

                    #region
                    if (DataCollection_DailyRoutine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Daily Routine :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_DailyRoutine"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (DataCollection_Expectaion)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Milestones Achieved till now :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_Expectaion"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (DataCollection_TherapyHistory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Investigations like MRI, BERA, TSH, Karyotype test, Opthalmology test were done :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_TherapyHistory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (DataCollection_Sources)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sources at this facility or other :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_Sources"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (DataCollection_NumberVisit)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Number of visits since last evaluation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_NumberVisit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (DataCollection_AdaptedEquipment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Adapted Equipment / Assistive Technology :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_AdaptedEquipment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Functional Activities ******************
                bool FunctionalAbilities_GrossMotor = false; if (ds.Tables[1].Rows[0]["FunctionalAbilities_GrossMotor"].ToString().Trim().Length > 0) { FunctionalAbilities_GrossMotor = true; }
                bool FunctionalAbilities_FineMotor = false; if (ds.Tables[1].Rows[0]["FunctionalAbilities_FineMotor"].ToString().Trim().Length > 0) { FunctionalAbilities_FineMotor = true; }
                bool FunctionalAbilities_Communication = false; if (ds.Tables[1].Rows[0]["FunctionalAbilities_Communication"].ToString().Trim().Length > 0) { FunctionalAbilities_Communication = true; }
                bool FunctionalAbilities_Cognitive = false; if (ds.Tables[1].Rows[0]["FunctionalAbilities_Cognitive"].ToString().Trim().Length > 0) { FunctionalAbilities_Cognitive = true; }
                bool FunctionalAbilities_Behaviour = false; if (ds.Tables[1].Rows[0]["FunctionalAbilities_Behaviour"].ToString().Trim().Length > 0) { FunctionalAbilities_Behaviour = true; }
                bool FunctionalLimitations_GrossMotor = false; if (ds.Tables[1].Rows[0]["FunctionalLimitations_GrossMotor"].ToString().Trim().Length > 0) { FunctionalLimitations_GrossMotor = true; }
                bool FunctionalLimitations_FineMotor = false; if (ds.Tables[1].Rows[0]["FunctionalLimitations_FineMotor"].ToString().Trim().Length > 0) { FunctionalLimitations_FineMotor = true; }
                bool FunctionalLimitations_Communication = false; if (ds.Tables[1].Rows[0]["FunctionalLimitations_Communication"].ToString().Trim().Length > 0) { FunctionalLimitations_Communication = true; }
                bool FunctionalLimitations_Cognitive = false; if (ds.Tables[1].Rows[0]["FunctionalLimitations_Cognitive"].ToString().Trim().Length > 0) { FunctionalLimitations_Cognitive = true; }
                bool FunctionalLimitations_Behaviour = false; if (ds.Tables[1].Rows[0]["FunctionalLimitations_Behaviour"].ToString().Trim().Length > 0) { FunctionalLimitations_Behaviour = true; }
                bool FunctionalActivities_ADL = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_ADL"].ToString().Trim().Length > 0) { FunctionalActivities_ADL = true; }
                if (FunctionalAbilities_GrossMotor || FunctionalAbilities_FineMotor || FunctionalAbilities_Communication || FunctionalAbilities_Cognitive ||
                    FunctionalAbilities_Behaviour || FunctionalLimitations_GrossMotor || FunctionalLimitations_FineMotor || FunctionalLimitations_Communication ||
                    FunctionalLimitations_Cognitive || FunctionalLimitations_Behaviour || FunctionalActivities_ADL)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Functional Activities :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (FunctionalAbilities_GrossMotor || FunctionalAbilities_FineMotor || FunctionalAbilities_Communication || FunctionalAbilities_Cognitive ||
                    FunctionalAbilities_Behaviour || FunctionalLimitations_GrossMotor || FunctionalLimitations_FineMotor || FunctionalLimitations_Communication ||
                    FunctionalLimitations_Cognitive || FunctionalLimitations_Behaviour)
                    {
                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        #region Header
                        cell = new PdfPCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Functional Abilities", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Functional Limitations", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        #endregion
                        #region Gross Motor
                        if (FunctionalAbilities_GrossMotor || FunctionalLimitations_GrossMotor)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalAbilities_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalLimitations_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Fine Motor
                        if (FunctionalAbilities_FineMotor || FunctionalLimitations_FineMotor)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalAbilities_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalLimitations_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Communication
                        if (FunctionalAbilities_Communication || FunctionalLimitations_Communication)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalAbilities_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalLimitations_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Cognition
                        if (FunctionalAbilities_Cognitive || FunctionalLimitations_Cognitive)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Cognition", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalAbilities_Cognitive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalLimitations_Cognitive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Behaviour
                        if (FunctionalAbilities_Behaviour || FunctionalLimitations_Behaviour)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Behaviour", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalAbilities_Behaviour"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalLimitations_Behaviour"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        document.Add(table);
                    }
                    #region
                    if (FunctionalActivities_ADL)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("ADL :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_ADL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Participation Activities ******************
                bool ParticipationAbilities_GrossMotor = false; if (ds.Tables[1].Rows[0]["ParticipationAbilities_GrossMotor"].ToString().Trim().Length > 0) { ParticipationAbilities_GrossMotor = true; }
                bool ParticipationAbilities_FineMotor = false; if (ds.Tables[1].Rows[0]["ParticipationAbilities_FineMotor"].ToString().Trim().Length > 0) { ParticipationAbilities_FineMotor = true; }
                bool ParticipationAbilities_Communication = false; if (ds.Tables[1].Rows[0]["ParticipationAbilities_Communication"].ToString().Trim().Length > 0) { ParticipationAbilities_Communication = true; }
                bool ParticipationAbilities_Cognitive = false; if (ds.Tables[1].Rows[0]["ParticipationAbilities_Cognitive"].ToString().Trim().Length > 0) { ParticipationAbilities_Cognitive = true; }
                bool ParticipationAbilities_Behaviour = false; if (ds.Tables[1].Rows[0]["ParticipationAbilities_Behaviour"].ToString().Trim().Length > 0) { ParticipationAbilities_Behaviour = true; }
                bool ParticipationLimitations_GrossMotor = false; if (ds.Tables[1].Rows[0]["ParticipationLimitations_GrossMotor"].ToString().Trim().Length > 0) { ParticipationLimitations_GrossMotor = true; }
                bool ParticipationLimitations_FineMotor = false; if (ds.Tables[1].Rows[0]["ParticipationLimitations_FineMotor"].ToString().Trim().Length > 0) { ParticipationLimitations_FineMotor = true; }
                bool ParticipationLimitations_Communication = false; if (ds.Tables[1].Rows[0]["ParticipationLimitations_Communication"].ToString().Trim().Length > 0) { ParticipationLimitations_Communication = true; }
                bool ParticipationLimitations_Cognitive = false; if (ds.Tables[1].Rows[0]["ParticipationLimitations_Cognitive"].ToString().Trim().Length > 0) { ParticipationLimitations_Cognitive = true; }
                bool ParticipationLimitations_Behaviour = false; if (ds.Tables[1].Rows[0]["ParticipationLimitations_Behaviour"].ToString().Trim().Length > 0) { ParticipationLimitations_Behaviour = true; }
                if (ParticipationAbilities_GrossMotor || ParticipationAbilities_FineMotor || ParticipationAbilities_Communication || ParticipationAbilities_Cognitive ||
                    ParticipationAbilities_Behaviour ||
                    ParticipationLimitations_GrossMotor || ParticipationLimitations_FineMotor || ParticipationLimitations_Communication || ParticipationLimitations_Cognitive ||
                    ParticipationLimitations_Behaviour)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Participation Activities :", HeadingFont), PdfPCell.ALIGN_LEFT);
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
                    table.SpacingBefore = 20f;
                    #region Header
                    cell = new PdfPCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);


                    cell = new PdfPCell(PhraseCell(new Phrase("Participation Abilities", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Participation Limitations", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    #endregion
                    #region Gross Motor
                    if (ParticipationAbilities_GrossMotor || ParticipationLimitations_GrossMotor)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbilities_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationLimitations_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region Fine Motor
                    if (ParticipationAbilities_FineMotor || ParticipationLimitations_FineMotor)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbilities_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationLimitations_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region Communication
                    if (ParticipationAbilities_Communication || ParticipationLimitations_Communication)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbilities_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationLimitations_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region Cognition
                    if (ParticipationAbilities_Cognitive || ParticipationLimitations_Cognitive)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Cognition", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbilities_Cognitive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationLimitations_Cognitive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region Behaviour
                    if (ParticipationAbilities_Behaviour || ParticipationLimitations_Behaviour)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Behaviour", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbilities_Behaviour"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationLimitations_Behaviour"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    #endregion
                    document.Add(table);
                }
                #endregion

                #region ************** Family Structure *********************
                bool FamilyStru_NoOfCaregivers = false; if (ds.Tables[1].Rows[0]["FamilyStru_NoOfCaregivers"].ToString().Trim().Length > 0) { FamilyStru_NoOfCaregivers = true; }
                bool FamilyStru_TimeWithChild = false; if (ds.Tables[1].Rows[0]["FamilyStru_TimeWithChild"].ToString().Trim().Length > 0) { FamilyStru_TimeWithChild = true; }
                bool FamilyStru_Holiday = false; if (ds.Tables[1].Rows[0]["FamilyStru_Holiday"].ToString().Trim().Length > 0) { FamilyStru_Holiday = true; }
                bool FamilyStru_DivoteTime = false; if (ds.Tables[1].Rows[0]["FamilyStru_DivoteTime"].ToString().Trim().Length > 0) { FamilyStru_DivoteTime = true; }
                bool FamilyStru_ContextualFactor = false; if (ds.Tables[1].Rows[0]["FamilyStru_ContextualFactor"].ToString().Trim().Length > 0) { FamilyStru_ContextualFactor = true; }
                bool FamilyStru_Social = false; if (ds.Tables[1].Rows[0]["FamilyStru_Social"].ToString().Trim().Length > 0) { FamilyStru_Social = true; }
                bool FamilyStru_Environment = false; if (ds.Tables[1].Rows[0]["FamilyStru_Environment"].ToString().Trim().Length > 0) { FamilyStru_Environment = true; }
                bool FamilyStru_Acceptance = false; if (ds.Tables[1].Rows[0]["FamilyStru_Acceptance"].ToString().Trim().Length > 0) { FamilyStru_Acceptance = true; }
                bool FamilyStru_Accessibility = false; if (ds.Tables[1].Rows[0]["FamilyStru_Accessibility"].ToString().Trim().Length > 0) { FamilyStru_Accessibility = true; }
                bool FamilyStru_CompareSibling = false; if (ds.Tables[1].Rows[0]["FamilyStru_CompareSibling"].ToString().Trim().Length > 0) { FamilyStru_CompareSibling = true; }
                bool FamilyStru_Working = false; if (ds.Tables[1].Rows[0]["FamilyStru_Working"].ToString().Trim().Length > 0) { FamilyStru_Working = true; }
                bool FamilyStru_FamilyPressure = false; if (ds.Tables[1].Rows[0]["FamilyStru_FamilyPressure"].ToString().Trim().Length > 0) { FamilyStru_FamilyPressure = true; }
                bool FamilyStru_SpentMostTime = false; if (ds.Tables[1].Rows[0]["FamilyStru_SpentMostTime"].ToString().Trim().Length > 0) { FamilyStru_SpentMostTime = true; }
                bool FamilyStru_CloselyInvolved = false; if (ds.Tables[1].Rows[0]["FamilyStru_CloselyInvolved"].ToString().Trim().Length > 0) { FamilyStru_CloselyInvolved = true; }
                bool FamilyStru_ChooseFreeTime = false; if (ds.Tables[1].Rows[0]["FamilyStru_ChooseFreeTime"].ToString().Trim().Length > 0) { FamilyStru_ChooseFreeTime = true; }
                bool FamilyStru_PlayWithToys = false; if (ds.Tables[1].Rows[0]["FamilyStru_PlayWithToys"].ToString().Trim().Length > 0) { FamilyStru_PlayWithToys = true; }
                bool FamilyStru_ToysExplain = false; if (ds.Tables[1].Rows[0]["FamilyStru_ToysExplain"].ToString().Trim().Length > 0) { FamilyStru_ToysExplain = true; }
                bool FamilyStru_ThrowTantrum = false; if (ds.Tables[1].Rows[0]["FamilyStru_ThrowTantrum"].ToString().Trim().Length > 0) { FamilyStru_ThrowTantrum = true; }

                if (FamilyStru_NoOfCaregivers || FamilyStru_TimeWithChild || FamilyStru_Holiday || FamilyStru_DivoteTime || FamilyStru_ContextualFactor || FamilyStru_Social ||
                    FamilyStru_Environment || FamilyStru_Acceptance || FamilyStru_Accessibility || FamilyStru_CompareSibling || FamilyStru_Working || FamilyStru_FamilyPressure ||
                    FamilyStru_SpentMostTime || FamilyStru_CloselyInvolved || FamilyStru_ChooseFreeTime || FamilyStru_PlayWithToys || FamilyStru_ToysExplain || FamilyStru_ThrowTantrum)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Family Structure :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (FamilyStru_NoOfCaregivers)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("No. of caregivers :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_NoOfCaregivers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_TimeWithChild)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Time spent with the child: Daily :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_TimeWithChild"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Holiday)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sunday/Holidays :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Holiday"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_DivoteTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Willingness to devote time for therapy :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_DivoteTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_ContextualFactor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Contextual factor : Economic :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_ContextualFactor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Social)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Social :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Social"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Environment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Environment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Environment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Acceptance)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Acceptance of condition :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Acceptance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Accessibility)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Accessibility to play areas/ extra circular activities :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Accessibility"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_CompareSibling)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comparison with sibling/cousin :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_CompareSibling"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_Working)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Working parents/ household help :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_Working"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_FamilyPressure)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Family pressure for performance :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_FamilyPressure"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_SpentMostTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("With whom/who does the child spend most of his day? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_SpentMostTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_CloselyInvolved)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Name of others closely involved with child :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_CloselyInvolved"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_ChooseFreeTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How does your child choose to use his/her free time :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_ChooseFreeTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_PlayWithToys)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Does your child play appropriately with toys? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_PlayWithToys"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_ToysExplain)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("If no explain :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_ToysExplain"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (FamilyStru_ThrowTantrum)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Does the child Throw tantrum? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStru_ThrowTantrum"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** School Information *********************
                bool SchoolInfo_SchoolType = false; if (ds.Tables[1].Rows[0]["SchoolInfo_SchoolType"].ToString().Trim().Length > 0) { SchoolInfo_SchoolType = true; }
                bool SchoolInfo_Hours = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Hours"].ToString().Trim().Length > 0) { SchoolInfo_Hours = true; }
                bool SchoolInfo_Traveling = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Traveling"].ToString().Trim().Length > 0) { SchoolInfo_Traveling = true; }
                bool SchoolInfo_Teachers = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Teachers"].ToString().Trim().Length > 0) { SchoolInfo_Teachers = true; }
                bool SchoolInfo_SeatingArr = false; if (ds.Tables[1].Rows[0]["SchoolInfo_SeatingArr"].ToString().Trim().Length > 0) { SchoolInfo_SeatingArr = true; }
                bool SchoolInfo_SeatingTol = false; if (ds.Tables[1].Rows[0]["SchoolInfo_SeatingTol"].ToString().Trim().Length > 0) { SchoolInfo_SeatingTol = true; }
                bool SchoolInfo_MeanTime = false; if (ds.Tables[1].Rows[0]["SchoolInfo_MeanTime"].ToString().Trim().Length > 0) { SchoolInfo_MeanTime = true; }
                bool SchoolInfo_FriendInteraction = false; if (ds.Tables[1].Rows[0]["SchoolInfo_FriendInteraction"].ToString().Trim().Length > 0) { SchoolInfo_FriendInteraction = true; }
                bool SchoolInfo_Sports = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Sports"].ToString().Trim().Length > 0) { SchoolInfo_Sports = true; }
                bool SchoolInfo_Curricular = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Curricular"].ToString().Trim().Length > 0) { SchoolInfo_Curricular = true; }
                bool SchoolInfo_Cultural = false; if (ds.Tables[1].Rows[0]["SchoolInfo_Cultural"].ToString().Trim().Length > 0) { SchoolInfo_Cultural = true; }
                bool SchoolInfo_ShadowTeacher = false; if (ds.Tables[1].Rows[0]["SchoolInfo_ShadowTeacher"].ToString().Trim().Length > 0) { SchoolInfo_ShadowTeacher = true; }
                bool SchoolInfo_RemarkTeacher = false; if (ds.Tables[1].Rows[0]["SchoolInfo_RemarkTeacher"].ToString().Trim().Length > 0) { SchoolInfo_RemarkTeacher = true; }
                bool SchoolInfo_CopyBoard = false; if (ds.Tables[1].Rows[0]["SchoolInfo_CopyBoard"].ToString().Trim().Length > 0) { SchoolInfo_CopyBoard = true; }
                bool SchoolInfo_CW_HW = false; if (ds.Tables[1].Rows[0]["SchoolInfo_CW_HW"].ToString().Trim().Length > 0) { SchoolInfo_CW_HW = true; }
                bool SchoolInfo_FollowInstru = false; if (ds.Tables[1].Rows[0]["SchoolInfo_FollowInstru"].ToString().Trim().Length > 0) { SchoolInfo_FollowInstru = true; }
                bool SchoolInfo_SpecialEducator = false; if (ds.Tables[1].Rows[0]["SchoolInfo_SpecialEducator"].ToString().Trim().Length > 0) { SchoolInfo_SpecialEducator = true; }
                bool SchoolInfo_DeliveryMode = false; if (ds.Tables[1].Rows[0]["SchoolInfo_DeliveryMode"].ToString().Trim().Length > 0) { SchoolInfo_DeliveryMode = true; }
                bool SchoolInfo_AcademicScope = false; if (ds.Tables[1].Rows[0]["SchoolInfo_AcademicScope"].ToString().Trim().Length > 0) { SchoolInfo_AcademicScope = true; }

                if (SchoolInfo_SchoolType || SchoolInfo_Hours || SchoolInfo_Traveling || SchoolInfo_Teachers || SchoolInfo_SeatingArr || SchoolInfo_SeatingTol ||
                    SchoolInfo_MeanTime || SchoolInfo_FriendInteraction || SchoolInfo_Sports || SchoolInfo_Curricular || SchoolInfo_Cultural || SchoolInfo_ShadowTeacher ||
                    SchoolInfo_RemarkTeacher || SchoolInfo_CopyBoard || SchoolInfo_CW_HW || SchoolInfo_FollowInstru || SchoolInfo_SpecialEducator ||
                    SchoolInfo_DeliveryMode || SchoolInfo_AcademicScope)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("School Information :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SchoolInfo_SchoolType)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Type of school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_SchoolType"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_Hours)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How many hours :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Hours"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_Traveling)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Traveling"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_Teachers)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Child teacher ratio :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Teachers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_SeatingArr)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Seating arrangement :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_SeatingArr"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_SeatingTol)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sitting tolerance of child :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_SeatingTol"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_MeanTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Meal time/ help required/ sharing food :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_MeanTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_FriendInteraction)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Friend/ social interaction :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_FriendInteraction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_Sports || SchoolInfo_Curricular || SchoolInfo_Cultural)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Participate in :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 0.7f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (SchoolInfo_Sports)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sports :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Sports"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            table.AddCell(cell);
                        }
                        if (SchoolInfo_Curricular)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Extra-curricular :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Curricular"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            table.AddCell(cell);
                        }
                        if (SchoolInfo_Cultural)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Cultural :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_Cultural"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_ShadowTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Shadow teacher required :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_ShadowTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_RemarkTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Remark from teacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_RemarkTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_CopyBoard)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Copying from board :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_CopyBoard"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_CW_HW)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Completing CW/HW :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_CW_HW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_FollowInstru)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Following instruction :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_FollowInstru"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_SpecialEducator)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Provision of special educator/ remedial/ shadow teacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_SpecialEducator"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_DeliveryMode)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Mode of delivery of info ppt / CD / board / video :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_DeliveryMode"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SchoolInfo_AcademicScope)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Academic scope and syllabus :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SchoolInfo_AcademicScope"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Behavior *********************
                bool Behaviour_AtHome = false; if (ds.Tables[1].Rows[0]["Behaviour_AtHome"].ToString().Trim().Length > 0) { Behaviour_AtHome = true; }
                bool Behaviour_AtSchool = false; if (ds.Tables[1].Rows[0]["Behaviour_AtSchool"].ToString().Trim().Length > 0) { Behaviour_AtSchool = true; }
                bool Behaviour_WithElder = false; if (ds.Tables[1].Rows[0]["Behaviour_WithElder"].ToString().Trim().Length > 0) { Behaviour_WithElder = true; }
                bool Behaviour_WithPeers = false; if (ds.Tables[1].Rows[0]["Behaviour_WithPeers"].ToString().Trim().Length > 0) { Behaviour_WithPeers = true; }
                bool Behaviour_WithTeacher = false; if (ds.Tables[1].Rows[0]["Behaviour_WithTeacher"].ToString().Trim().Length > 0) { Behaviour_WithTeacher = true; }
                bool Behaviour_AtTheMall = false; if (ds.Tables[1].Rows[0]["Behaviour_AtTheMall"].ToString().Trim().Length > 0) { Behaviour_AtTheMall = true; }
                bool Behaviour_AtPlayground = false; if (ds.Tables[1].Rows[0]["Behaviour_AtPlayground"].ToString().Trim().Length > 0) { Behaviour_AtPlayground = true; }
                bool BehaviourPl_Constructive = false; if (ds.Tables[1].Rows[0]["BehaviourPl_Constructive"].ToString().Trim().Length > 0) { BehaviourPl_Constructive = true; }
                bool BehaviourPl_Destructive = false; if (ds.Tables[1].Rows[0]["BehaviourPl_Destructive"].ToString().Trim().Length > 0) { BehaviourPl_Destructive = true; }
                bool BehaviourPl_CD_Remark = false; if (ds.Tables[1].Rows[0]["BehaviourPl_CD_Remark"].ToString().Trim().Length > 0) { BehaviourPl_CD_Remark = true; }
                bool BehaviourPL_Independant = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Independant"].ToString().Trim().Length > 0) { BehaviourPL_Independant = true; }
                bool BehaviourPL_Supervised = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Supervised"].ToString().Trim().Length > 0) { BehaviourPL_Supervised = true; }
                bool BehaviourPL_IS_Remark = false; if (ds.Tables[1].Rows[0]["BehaviourPL_IS_Remark"].ToString().Trim().Length > 0) { BehaviourPL_IS_Remark = true; }
                bool BehaviourPL_Sedentary = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Sedentary"].ToString().Trim().Length > 0) { BehaviourPL_Sedentary = true; }
                bool BehaviourPL_OnTheGo = false; if (ds.Tables[1].Rows[0]["BehaviourPL_OnTheGo"].ToString().Trim().Length > 0) { BehaviourPL_OnTheGo = true; }
                bool BehaviourPL_AgeAppropriate = false; if (ds.Tables[1].Rows[0]["BehaviourPL_AgeAppropriate"].ToString().Trim().Length > 0) { BehaviourPL_AgeAppropriate = true; }
                bool BehaviourPL_FollowRule = false; if (ds.Tables[1].Rows[0]["BehaviourPL_FollowRule"].ToString().Trim().Length > 0) { BehaviourPL_FollowRule = true; }
                bool BehaviourPL_Bullied = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Bullied"].ToString().Trim().Length > 0) { BehaviourPL_Bullied = true; }
                bool BehaviourPL_PlayAchieved = false; if (ds.Tables[1].Rows[0]["BehaviourPL_PlayAchieved"].ToString().Trim().Length > 0) { BehaviourPL_PlayAchieved = true; }
                bool BehaviourPL_ToyChoice = false; if (ds.Tables[1].Rows[0]["BehaviourPL_ToyChoice"].ToString().Trim().Length > 0) { BehaviourPL_ToyChoice = true; }
                bool BehaviourPL_Repetitive = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Repetitive"].ToString().Trim().Length > 0) { BehaviourPL_Repetitive = true; }
                bool BehaviourPL_Versatile = false; if (ds.Tables[1].Rows[0]["BehaviourPL_Versatile"].ToString().Trim().Length > 0) { BehaviourPL_Versatile = true; }
                bool BehaviourPL_PartInGroup = false; if (ds.Tables[1].Rows[0]["BehaviourPL_PartInGroup"].ToString().Trim().Length > 0) { BehaviourPL_PartInGroup = true; }
                bool BehaviourPL_IsLeader = false; if (ds.Tables[1].Rows[0]["BehaviourPL_IsLeader"].ToString().Trim().Length > 0) { BehaviourPL_IsLeader = true; }
                bool BehaviourPL_IsFollower = false; if (ds.Tables[1].Rows[0]["BehaviourPL_IsFollower"].ToString().Trim().Length > 0) { BehaviourPL_IsFollower = true; }
                bool BehaviourPL_PretendPlay = false; if (ds.Tables[1].Rows[0]["BehaviourPL_PretendPlay"].ToString().Trim().Length > 0) { BehaviourPL_PretendPlay = true; }
                bool Behaviour_RegulatesSelf = false; if (ds.Tables[1].Rows[0]["Behaviour_RegulatesSelf"].ToString().Trim().Length > 0) { Behaviour_RegulatesSelf = true; }
                bool Behaviour_BehaveNotReg = false; if (ds.Tables[1].Rows[0]["Behaviour_BehaveNotReg"].ToString().Trim().Length > 0) { Behaviour_BehaveNotReg = true; }
                bool Behaviour_WhatCalmDown = false; if (ds.Tables[1].Rows[0]["Behaviour_WhatCalmDown"].ToString().Trim().Length > 0) { Behaviour_WhatCalmDown = true; }
                bool Behaviour_HappyLike = false; if (ds.Tables[1].Rows[0]["Behaviour_HappyLike"].ToString().Trim().Length > 0) { Behaviour_HappyLike = true; }
                bool Behaviour_HappyDislike = false; if (ds.Tables[1].Rows[0]["Behaviour_HappyDislike"].ToString().Trim().Length > 0) { Behaviour_HappyDislike = true; }
                if (Behaviour_AtHome || Behaviour_AtSchool || Behaviour_WithElder || Behaviour_WithPeers || Behaviour_WithTeacher || Behaviour_AtTheMall ||
                    Behaviour_AtPlayground || BehaviourPl_Constructive || BehaviourPl_Destructive || BehaviourPl_CD_Remark || BehaviourPL_Independant ||
                    BehaviourPL_Supervised || BehaviourPL_IS_Remark || BehaviourPL_Sedentary || BehaviourPL_OnTheGo || BehaviourPL_AgeAppropriate ||
                    BehaviourPL_FollowRule || BehaviourPL_Bullied || BehaviourPL_PlayAchieved || BehaviourPL_ToyChoice || BehaviourPL_Repetitive ||
                    BehaviourPL_Versatile || BehaviourPL_PartInGroup || BehaviourPL_IsLeader || BehaviourPL_IsFollower || BehaviourPL_PretendPlay ||
                    Behaviour_RegulatesSelf || Behaviour_BehaveNotReg || Behaviour_WhatCalmDown || Behaviour_HappyLike || Behaviour_HappyDislike)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Behavior :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region Behavior of child
                    if (Behaviour_AtHome || Behaviour_AtSchool || Behaviour_WithElder || Behaviour_WithPeers || Behaviour_WithTeacher || Behaviour_AtTheMall || Behaviour_AtPlayground)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Behavior of child :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 10f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (Behaviour_AtHome)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("At home :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_AtHome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_AtSchool)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("At school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_AtSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_WithElder)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("With elders :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_WithElder"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_WithPeers)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("With peers :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_WithPeers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_WithTeacher)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("With teachers :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_WithTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_AtTheMall)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("At the Mall :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_AtTheMall"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_AtPlayground)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("At the playground :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_AtPlayground"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion
                    #region Play Behavior
                    if (BehaviourPl_Constructive || BehaviourPl_Destructive || BehaviourPl_CD_Remark || BehaviourPL_Independant ||
                    BehaviourPL_Supervised || BehaviourPL_IS_Remark || BehaviourPL_Sedentary || BehaviourPL_OnTheGo || BehaviourPL_AgeAppropriate ||
                    BehaviourPL_FollowRule || BehaviourPL_Bullied || BehaviourPL_PlayAchieved || BehaviourPL_ToyChoice || BehaviourPL_Repetitive ||
                    BehaviourPL_Versatile || BehaviourPL_PartInGroup || BehaviourPL_IsLeader || BehaviourPL_IsFollower || BehaviourPL_PretendPlay ||
                    Behaviour_RegulatesSelf || Behaviour_BehaveNotReg || Behaviour_WhatCalmDown || Behaviour_HappyLike || Behaviour_HappyDislike)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Play Behavior :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 10f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (BehaviourPl_Constructive || BehaviourPl_Destructive)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Constructive / Destructive :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);
                            if (BehaviourPl_Constructive)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPl_Constructive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (BehaviourPl_Destructive)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPl_Destructive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion

                        #region
                        if ((BehaviourPl_Constructive || BehaviourPl_Destructive) && BehaviourPl_CD_Remark)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Remark :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPl_CD_Remark"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_Independant || BehaviourPL_Supervised)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Independent / supervised :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);
                            if (BehaviourPL_Independant)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Independant"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (BehaviourPL_Supervised)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Supervised"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion

                        #region
                        if ((BehaviourPL_Independant || BehaviourPL_Supervised) && BehaviourPL_IS_Remark)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Remark :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_IS_Remark"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_Sedentary)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Sedentary :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Sedentary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_OnTheGo)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("on the go :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_OnTheGo"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_AgeAppropriate)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Age appropriate :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_AgeAppropriate"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_FollowRule)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Follows rules :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_FollowRule"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_Bullied)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Bullies / Bullied :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Bullied"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_PlayAchieved)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Types of play achieved :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_PlayAchieved"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_ToyChoice)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Toys at home / choice :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_ToyChoice"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_Repetitive || BehaviourPL_Versatile)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Repetitive / Versatile :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);
                            if (BehaviourPL_Repetitive)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Repetitive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (BehaviourPL_Versatile)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_Versatile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion

                        #region
                        if (BehaviourPL_PartInGroup)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Participate in group :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_PartInGroup"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (BehaviourPL_IsLeader || BehaviourPL_IsFollower)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Leaders / Follower :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);
                            if (BehaviourPL_IsLeader)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_IsLeader"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (BehaviourPL_IsFollower)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_IsFollower"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion

                        #region
                        if (BehaviourPL_PretendPlay)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Pretend play :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BehaviourPL_PretendPlay"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Behaviour_RegulatesSelf || Behaviour_BehaveNotReg || Behaviour_WhatCalmDown)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Regulation  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            if (Behaviour_RegulatesSelf)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Child regulates self :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_RegulatesSelf"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (Behaviour_BehaveNotReg)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Any seeking behavior when not regulated :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_BehaveNotReg"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (Behaviour_WhatCalmDown)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("What calm him/her down :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_WhatCalmDown"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion

                        #region
                        if (Behaviour_HappyLike || Behaviour_HappyDislike)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Happiness :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.2f, 0.8f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            if (Behaviour_HappyLike)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase("Like :", NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);

                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_HappyLike"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 5f;
                                table.AddCell(cell);
                            }
                            if (Behaviour_HappyDislike)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase("Dislikes :", NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);

                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_HappyDislike"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 5f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region ************** Arousal *********************
                bool Arousal_EvalAlert = false; if (ds.Tables[1].Rows[0]["Arousal_EvalAlert"].ToString().Trim().Length > 0) { Arousal_EvalAlert = true; }
                bool Arousal_GeneralAlert = false; if (ds.Tables[1].Rows[0]["Arousal_GeneralAlert"].ToString().Trim().Length > 0) { Arousal_GeneralAlert = true; }
                bool Arousal_StimuliResponse = false; if (ds.Tables[1].Rows[0]["Arousal_StimuliResponse"].ToString().Trim().Length > 0) { Arousal_StimuliResponse = true; }
                bool Arousal_Transition = false; if (ds.Tables[1].Rows[0]["Arousal_Transition"].ToString().Trim().Length > 0) { Arousal_Transition = true; }
                bool Arousal_Optimum = false; if (ds.Tables[1].Rows[0]["Arousal_Optimum"].ToString().Trim().Length > 0) { Arousal_Optimum = true; }
                bool Arousal_AlertingFactor = false; if (ds.Tables[1].Rows[0]["Arousal_AlertingFactor"].ToString().Trim().Length > 0) { Arousal_AlertingFactor = true; }
                bool Arousal_CalmingFactor = false; if (ds.Tables[1].Rows[0]["Arousal_CalmingFactor"].ToString().Trim().Length > 0) { Arousal_CalmingFactor = true; }

                if (Arousal_EvalAlert || Arousal_GeneralAlert || Arousal_StimuliResponse || Arousal_Transition || Arousal_Optimum || Arousal_AlertingFactor || Arousal_CalmingFactor)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Arousal :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Arousal_EvalAlert)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("State of alertness during eval :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_EvalAlert"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_GeneralAlert)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("General state of alertness :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_GeneralAlert"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_StimuliResponse)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Responses to stimuli :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_StimuliResponse"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_Transition)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Transition :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_Transition"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_Optimum)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Optimum arousal :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_Optimum"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_AlertingFactor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Alerting factor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_AlertingFactor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Arousal_CalmingFactor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Calming factor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_CalmingFactor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Attention *********************
                bool Attention_InSchool = false; if (ds.Tables[1].Rows[0]["Attention_InSchool"].ToString().Trim().Length > 0) { Attention_InSchool = true; }
                bool Attention_InHome = false; if (ds.Tables[1].Rows[0]["Attention_InHome"].ToString().Trim().Length > 0) { Attention_InHome = true; }
                bool Attention_Dividing = false; if (ds.Tables[1].Rows[0]["Attention_Dividing"].ToString().Trim().Length > 0) { Attention_Dividing = true; }
                bool Attention_ChangeActivities = false; if (ds.Tables[1].Rows[0]["Attention_ChangeActivities"].ToString().Trim().Length > 0) { Attention_ChangeActivities = true; }
                bool Attention_AgeAppropriate = false; if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Trim().Length > 0) { Attention_AgeAppropriate = true; }
                bool Attention_AttentionSpan = false; if (ds.Tables[1].Rows[0]["Attention_AttentionSpan"].ToString().Trim().Length > 0) { Attention_AttentionSpan = true; }
                bool Attention_Distractibility_Home = false; if (ds.Tables[1].Rows[0]["Attention_Distractibility_Home"].ToString().Trim().Length > 0) { Attention_Distractibility_Home = true; }
                bool Attention_Distractibility_School = false; if (ds.Tables[1].Rows[0]["Attention_Distractibility_School"].ToString().Trim().Length > 0) { Attention_Distractibility_School = true; }

                if (Attention_InSchool || Attention_InHome || Attention_Dividing || Attention_ChangeActivities || Attention_AgeAppropriate ||
                    Attention_AttentionSpan || Attention_Distractibility_Home || Attention_Distractibility_School)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Attention :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Attention_InSchool || Attention_InHome)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Focus attention to task at hand :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.2f, 0.8f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Attention_InSchool)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("In school :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_InSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Attention_InHome)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("At home :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_InHome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Attention_Dividing)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Dividing Attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_Dividing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Attention_ChangeActivities)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Change of activities :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_ChangeActivities"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Attention_AgeAppropriate)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Age appropriate attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Attention_AttentionSpan)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Attention span :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_AttentionSpan"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Attention_Distractibility_Home || Attention_Distractibility_School)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Factors of distractibility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.2f, 0.8f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Attention_Distractibility_Home)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("In home :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_Distractibility_Home"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Attention_Distractibility_School)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("At school :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_Distractibility_School"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Affect / Action *********************
                bool Affect_EmotionRange = false; if (ds.Tables[1].Rows[0]["Affect_EmotionRange"].ToString().Trim().Length > 0) { Affect_EmotionRange = true; }
                bool Affect_EmotionExpress = false; if (ds.Tables[1].Rows[0]["Affect_EmotionExpress"].ToString().Trim().Length > 0) { Affect_EmotionExpress = true; }
                bool Affect_Environment = false; if (ds.Tables[1].Rows[0]["Affect_Environment"].ToString().Trim().Length > 0) { Affect_Environment = true; }
                bool Affect_Task = false; if (ds.Tables[1].Rows[0]["Affect_Task"].ToString().Trim().Length > 0) { Affect_Task = true; }
                bool Affect_Individual = false; if (ds.Tables[1].Rows[0]["Affect_Individual"].ToString().Trim().Length > 0) { Affect_Individual = true; }
                bool Affect_Consistent = false; if (ds.Tables[1].Rows[0]["Affect_Consistent"].ToString().Trim().Length > 0) { Affect_Consistent = true; }
                bool Affect_Characterising = false; if (ds.Tables[1].Rows[0]["Affect_Characterising"].ToString().Trim().Length > 0) { Affect_Characterising = true; }
                bool Action_Planning = false; if (ds.Tables[1].Rows[0]["Action_Planning"].ToString().Trim().Length > 0) { Action_Planning = true; }
                bool Action_Purposeful = false; if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Trim().Length > 0) { Action_Purposeful = true; }
                bool Action_GoalOriented = false; if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Trim().Length > 0) { Action_GoalOriented = true; }
                bool Action_FeedbackDependent = false; if (ds.Tables[1].Rows[0]["Action_FeedbackDependent"].ToString().Trim().Length > 0) { Action_FeedbackDependent = true; }
                if (Affect_EmotionRange || Affect_EmotionExpress || Affect_Environment || Affect_Task || Affect_Individual || Affect_Consistent || Affect_Characterising)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Affect :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Affect_EmotionRange)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Wide range of emotions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_EmotionRange"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Affect_EmotionExpress)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Is the child able to express emotion :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_EmotionExpress"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Affect_Environment || Affect_Task || Affect_Individual)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Affect appropriate to :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.25f, 0.75f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Affect_Environment)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Environment :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Environment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Affect_Task)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Task :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Task"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Affect_Individual)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Individual :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Individual"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Affect_Consistent)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Consistent emotion throughout :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Consistent"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Affect_Characterising)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Factors characterising affects :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Characterising"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                if (Action_Planning || Action_Purposeful || Action_GoalOriented || Action_FeedbackDependent)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Action :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Action_Planning)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Age appropriate action/ motor planning :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_Planning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Action_Purposeful)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Purposeful :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_Purposeful"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Action_GoalOriented)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Goal Oriented :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Action_FeedbackDependent)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Feedback Dependent :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_FeedbackDependent"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Social Interaction *********************
                bool Social_KnownPeople = false; if (ds.Tables[1].Rows[0]["Social_KnownPeople"].ToString().Trim().Length > 0) { Social_KnownPeople = true; }
                bool Social_Strangers = false; if (ds.Tables[1].Rows[0]["Social_Strangers"].ToString().Trim().Length > 0) { Social_Strangers = true; }
                bool Social_Gathering = false; if (ds.Tables[1].Rows[0]["Social_Gathering"].ToString().Trim().Length > 0) { Social_Gathering = true; }
                bool Social_Emotional = false; if (ds.Tables[1].Rows[0]["Social_Emotional"].ToString().Trim().Length > 0) { Social_Emotional = true; }
                bool Social_Appreciates = false; if (ds.Tables[1].Rows[0]["Social_Appreciates"].ToString().Trim().Length > 0) { Social_Appreciates = true; }
                bool Social_Reaction = false; if (ds.Tables[1].Rows[0]["Social_Reaction"].ToString().Trim().Length > 0) { Social_Reaction = true; }
                bool Social_Sadness = false; if (ds.Tables[1].Rows[0]["Social_Sadness"].ToString().Trim().Length > 0) { Social_Sadness = true; }
                bool Social_Surprise = false; if (ds.Tables[1].Rows[0]["Social_Surprise"].ToString().Trim().Length > 0) { Social_Surprise = true; }
                bool Social_Shock = false; if (ds.Tables[1].Rows[0]["Social_Shock"].ToString().Trim().Length > 0) { Social_Shock = true; }
                bool Social_Friendships = false; if (ds.Tables[1].Rows[0]["Social_Friendships"].ToString().Trim().Length > 0) { Social_Friendships = true; }
                bool Social_Relates = false; if (ds.Tables[1].Rows[0]["Social_Relates"].ToString().Trim().Length > 0) { Social_Relates = true; }
                bool Social_ActivitiestheyEnjoy = false; if (ds.Tables[1].Rows[0]["Social_ActivitiestheyEnjoy"].ToString().Trim().Length > 0) { Social_ActivitiestheyEnjoy = true; }
                if (Social_KnownPeople || Social_Strangers || Social_Gathering || Social_Emotional || Social_Appreciates || Social_Reaction || Social_Sadness ||
                    Social_Surprise || Social_Shock || Social_Friendships || Social_Relates || Social_ActivitiestheyEnjoy)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Social Interaction :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Social_KnownPeople || Social_Strangers || Social_Gathering)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Interaction with / At :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 0.7f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Social_KnownPeople)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Known People :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_KnownPeople"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Social_Strangers)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Strangers :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Strangers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Social_Gathering)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Social gathering :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Gathering"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Social_Emotional)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Emotional Response :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Emotional"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Social_Appreciates)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Understands / appreciates social cues :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Appreciates"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Social_Reaction || Social_Sadness || Social_Surprise || Social_Shock)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Reaction to emotion / of other :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.25f, 0.85f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Social_Reaction)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Happiness :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Reaction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Social_Sadness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sadness :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Sadness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Social_Surprise)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Surprise :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Surprise"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Social_Shock)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Shock :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Shock"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Social_Friendships || Social_Relates || Social_ActivitiestheyEnjoy)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Friendships  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (Social_Friendships)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Can make friends :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Friendships"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Social_Relates)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Relates to known people :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_Relates"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Social_ActivitiestheyEnjoy)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("What activities they enjoy :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_ActivitiestheyEnjoy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion
                }
                #endregion

                #region ************** Communication *********************
                bool Communication_StartToSpeak = false; if (ds.Tables[1].Rows[0]["Communication_StartToSpeak"].ToString().Trim().Length > 0) { Communication_StartToSpeak = true; }
                bool Communication_Monosyllables = false; if (ds.Tables[1].Rows[0]["Communication_Monosyllables"].ToString().Trim().Length > 0) { Communication_Monosyllables = true; }
                bool Communication_Bisyllables = false; if (ds.Tables[1].Rows[0]["Communication_Bisyllables"].ToString().Trim().Length > 0) { Communication_Bisyllables = true; }
                bool Communication_ShortSentences = false; if (ds.Tables[1].Rows[0]["Communication_ShortSentences"].ToString().Trim().Length > 0) { Communication_ShortSentences = true; }
                bool Communication_LongSentence = false; if (ds.Tables[1].Rows[0]["Communication_LongSentence"].ToString().Trim().Length > 0) { Communication_LongSentence = true; }
                bool Communication_UnusualSounds = false; if (ds.Tables[1].Rows[0]["Communication_UnusualSounds"].ToString().Trim().Length > 0) { Communication_UnusualSounds = true; }
                bool Communication_ImitationOfSpeech = false; if (ds.Tables[1].Rows[0]["Communication_ImitationOfSpeech"].ToString().Trim().Length > 0) { Communication_ImitationOfSpeech = true; }
                bool Communication_FacialExpression = false; if (ds.Tables[1].Rows[0]["Communication_FacialExpression"].ToString().Trim().Length > 0) { Communication_FacialExpression = true; }
                bool Communication_EyeContact = false; if (ds.Tables[1].Rows[0]["Communication_EyeContact"].ToString().Trim().Length > 0) { Communication_EyeContact = true; }
                bool Communication_Gestures = false; if (ds.Tables[1].Rows[0]["Communication_Gestures"].ToString().Trim().Length > 0) { Communication_Gestures = true; }
                bool Communication_InterpretationOfLanguage = false; if (ds.Tables[1].Rows[0]["Communication_InterpretationOfLanguage"].ToString().Trim().Length > 0) { Communication_InterpretationOfLanguage = true; }
                bool Communication_UnderstandImplied = false; if (ds.Tables[1].Rows[0]["Communication_UnderstandImplied"].ToString().Trim().Length > 0) { Communication_UnderstandImplied = true; }
                bool Communication_UnderstandJoke = false; if (ds.Tables[1].Rows[0]["Communication_UnderstandJoke"].ToString().Trim().Length > 0) { Communication_UnderstandJoke = true; }
                bool Communication_RespondsToName = false; if (ds.Tables[1].Rows[0]["Communication_RespondsToName"].ToString().Trim().Length > 0) { Communication_RespondsToName = true; }
                bool Communication_TwoWayInteraction = false; if (ds.Tables[1].Rows[0]["Communication_TwoWayInteraction"].ToString().Trim().Length > 0) { Communication_TwoWayInteraction = true; }
                bool Communication_NarrateIncidentsHome = false; if (ds.Tables[1].Rows[0]["Communication_NarrateIncidentsHome"].ToString().Trim().Length > 0) { Communication_NarrateIncidentsHome = true; }
                bool Communication_NarrateIncidentsSchool = false; if (ds.Tables[1].Rows[0]["Communication_NarrateIncidentsSchool"].ToString().Trim().Length > 0) { Communication_NarrateIncidentsSchool = true; }
                bool Communication_ExpressionsWants = false; if (ds.Tables[1].Rows[0]["Communication_ExpressionsWants"].ToString().Trim().Length > 0) { Communication_ExpressionsWants = true; }
                bool Communication_ExpressionsNeeds = false; if (ds.Tables[1].Rows[0]["Communication_ExpressionsNeeds"].ToString().Trim().Length > 0) { Communication_ExpressionsNeeds = true; }
                bool Communication_ExpressionsEmotion = false; if (ds.Tables[1].Rows[0]["Communication_ExpressionsEmotion"].ToString().Trim().Length > 0) { Communication_ExpressionsEmotion = true; }
                bool Communication_ExpressionsAchive = false; if (ds.Tables[1].Rows[0]["Communication_ExpressionsAchive"].ToString().Trim().Length > 0) { Communication_ExpressionsAchive = true; }
                bool Communication_LanguagSpoken = false; if (ds.Tables[1].Rows[0]["Communication_LanguagSpoken"].ToString().Trim().Length > 0) { Communication_LanguagSpoken = true; }
                bool Communication_Echolalia = false; if (ds.Tables[1].Rows[0]["Communication_Echolalia"].ToString().Trim().Length > 0) { Communication_Echolalia = true; }
                if (Communication_StartToSpeak || Communication_Monosyllables || Communication_Bisyllables ||
                    Communication_ShortSentences || Communication_LongSentence || Communication_UnusualSounds || Communication_ImitationOfSpeech ||
                    Communication_FacialExpression || Communication_EyeContact || Communication_Gestures || Communication_InterpretationOfLanguage ||
                    Communication_UnderstandImplied || Communication_UnderstandJoke || Communication_RespondsToName || Communication_TwoWayInteraction ||
                    Communication_NarrateIncidentsHome || Communication_NarrateIncidentsSchool || Communication_ExpressionsWants || Communication_ExpressionsNeeds ||
                    Communication_ExpressionsEmotion || Communication_ExpressionsAchive || Communication_LanguagSpoken || Communication_Echolalia)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Communication :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Communication_StartToSpeak || Communication_Monosyllables || Communication_Bisyllables || Communication_ShortSentences || Communication_LongSentence)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("When did your child start to :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 0.7f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Communication_StartToSpeak)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Speak :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_StartToSpeak"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_Monosyllables)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Monosyllables :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_Monosyllables"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_Bisyllables)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Bisyllables :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_Bisyllables"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_ShortSentences)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Short Sentences :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ShortSentences"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_LongSentence)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Long Sentence :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_LongSentence"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_UnusualSounds)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Unusual sounds/ jargon speech :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_UnusualSounds"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_ImitationOfSpeech)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Imitation of speech / gestures :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ImitationOfSpeech"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_FacialExpression || Communication_EyeContact || Communication_Gestures)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Non verbal facial :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.25f, 0.85f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Communication_FacialExpression)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Expression :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_FacialExpression"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_EyeContact)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Eye contact :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_EyeContact"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_Gestures)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Gestures :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_Gestures"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_InterpretationOfLanguage || Communication_UnderstandImplied || Communication_UnderstandJoke || Communication_RespondsToName)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Interpretation of language  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (Communication_InterpretationOfLanguage)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Simple / Complex :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_InterpretationOfLanguage"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_UnderstandImplied)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Understand implied meaning :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_UnderstandImplied"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_UnderstandJoke)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Understand joke/ sarcasm :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_UnderstandJoke"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_RespondsToName)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Responds to name :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_RespondsToName"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (Communication_UnusualSounds)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Two – way Interaction :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_TwoWayInteraction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_NarrateIncidentsHome || Communication_NarrateIncidentsSchool)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Narrate Incidents :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.25f, 0.85f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        if (Communication_NarrateIncidentsHome)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("At school :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_NarrateIncidentsHome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        if (Communication_NarrateIncidentsSchool)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("At home :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_NarrateIncidentsSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_ExpressionsWants || Communication_ExpressionsNeeds || Communication_ExpressionsEmotion || Communication_ExpressionsAchive)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Expressions of  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (Communication_ExpressionsWants)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Wants :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ExpressionsWants"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_ExpressionsNeeds)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Needs :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ExpressionsNeeds"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_ExpressionsEmotion)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Emotions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ExpressionsEmotion"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Communication_ExpressionsAchive)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Achievement/Failure :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_ExpressionsAchive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (Communication_LanguagSpoken)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Language spoken to the child :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_LanguagSpoken"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Communication_Echolalia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Echolalia :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_Echolalia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Examination *********************
                bool Morphology = false;
                if (ds.Tables[1].Rows[0]["Morphology_Height"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Weight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbLength"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbLeft"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbRight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Head"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Nipple"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Waist"].ToString().Trim().Length > 0)
                {
                    Morphology = true;
                }
                bool UpperLimb = false;
                if (ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_BLW"].ToString().Trim().Length > 0)
                {
                    UpperLimb = true;
                }
                bool LowerLimb = false;
                if (ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_ABV"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_AT"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_BLW"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_BLW"].ToString().Trim().Length > 0)
                {
                    LowerLimb = true;
                }
                bool Morphology_OralMotorFactors = false; if (ds.Tables[1].Rows[0]["Morphology_OralMotorFactors"].ToString().Trim().Length > 0) { Morphology_OralMotorFactors = true; }
                if (Morphology || UpperLimb || LowerLimb || Morphology_OralMotorFactors)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Examination :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Morphology)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Morphology :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(6);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Height :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_Height"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Weight :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_Weight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Limb Length :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LimbLength"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LimbLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LimbRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /****/
                        cell = new PdfPCell(PhraseCell(new Phrase("Head :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_Head"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Nipple :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_Nipple"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Waist :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_Waist"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (UpperLimb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Upper Limb :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(5);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Description", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Level (In ches)", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Girth (In cm)", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Above Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("At Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Below Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelRight_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbLevelLeft_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthRight_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        /**/
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (LowerLimb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Lower Limb :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(5);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Description", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Level (In ches)", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Girth (In cm)", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Above Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/

                        cell = new PdfPCell(PhraseCell(new Phrase("At Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Below Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelRight_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbLevelLeft_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthRight_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        /**/
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Morphology_OralMotorFactors)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Key Oral Motor Factors :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_OralMotorFactors"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Functional Activities OLD *********************
                //bool FunctionalActivities_GrossMotor = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_GrossMotor"].ToString().Trim().Length > 0) { FunctionalActivities_GrossMotor = true; }
                //bool FunctionalActivities_HandFunction = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_HandFunction"].ToString().Trim().Length > 0) { FunctionalActivities_HandFunction = true; }
                //bool FunctionalActivities_FineMotor = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_FineMotor"].ToString().Trim().Length > 0) { FunctionalActivities_FineMotor = true; }
                //bool FunctionalActivities_OralMotor = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_OralMotor"].ToString().Trim().Length > 0) { FunctionalActivities_OralMotor = true; }
                //bool FunctionalActivities_Communication = false; if (ds.Tables[1].Rows[0]["FunctionalActivities_Communication"].ToString().Trim().Length > 0) { FunctionalActivities_Communication = true; }
                //if (FunctionalActivities_GrossMotor || FunctionalActivities_HandFunction || FunctionalActivities_FineMotor || FunctionalActivities_ADL || FunctionalActivities_OralMotor || FunctionalActivities_Communication)
                //{
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.3f, 1f });
                //    table.SpacingBefore = 20f;

                //    cell = PhraseCell(new Phrase("Functional Activities :", HeadingFont), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    table.AddCell(cell);
                //    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    cell.PaddingBottom = 30f;
                //    table.AddCell(cell);
                //    document.Add(table);

                //    #region
                //    if (FunctionalActivities_GrossMotor)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //    }
                //    #endregion

                //    #region
                //    if (FunctionalActivities_HandFunction)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("Hand Function :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_HandFunction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //    }
                //    #endregion

                //    #region
                //    if (FunctionalActivities_FineMotor)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //    }
                //    #endregion


                //    #region
                //    if (FunctionalActivities_OralMotor)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("Oral Motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_OralMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //    }
                //    #endregion

                //    #region
                //    if (FunctionalActivities_Communication)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("Communication :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FunctionalActivities_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //    }
                //    #endregion
                //}
                #endregion

                #region ************** Repetitive Interests *********************
                bool RepetitiveInterests_Dominates = false; if (ds.Tables[1].Rows[0]["RepetitiveInterests_Dominates"].ToString().Trim().Length > 0) { RepetitiveInterests_Dominates = true; }
                bool RepetitiveInterests_Behavior = false; if (ds.Tables[1].Rows[0]["RepetitiveInterests_Behavior"].ToString().Trim().Length > 0) { RepetitiveInterests_Behavior = true; }
                bool RepetitiveInterests_Changes = false; if (ds.Tables[1].Rows[0]["RepetitiveInterests_Changes"].ToString().Trim().Length > 0) { RepetitiveInterests_Changes = true; }

                if (RepetitiveInterests_Dominates || RepetitiveInterests_Behavior || RepetitiveInterests_Changes)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Repetitive Interests :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (RepetitiveInterests_Dominates)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Any interests that dominates his/her life :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RepetitiveInterests_Dominates"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (RepetitiveInterests_Behavior)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Repetitive/ Unusual Behavior (stereotypes) :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RepetitiveInterests_Behavior"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (RepetitiveInterests_Changes)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Copes up with unexpected changes :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RepetitiveInterests_Changes"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Test And Measures *********************
                bool TestMeasures_GMFCS = false; if (ds.Tables[1].Rows[0]["TestMeasures_GMFCS"].ToString().Trim().Length > 0) { TestMeasures_GMFCS = true; }
                bool TestMeasures_GMFM = false; if (ds.Tables[1].Rows[0]["TestMeasures_GMFM"].ToString().Trim().Length > 0) { TestMeasures_GMFM = true; }
                bool TestMeasures_GMPM = false; if (ds.Tables[1].Rows[0]["TestMeasures_GMPM"].ToString().Trim().Length > 0) { TestMeasures_GMPM = true; }
                bool TestMeasures_AshworthScale = false; if (ds.Tables[1].Rows[0]["TestMeasures_AshworthScale"].ToString().Trim().Length > 0) { TestMeasures_AshworthScale = true; }
                bool TestMeasures_TradieusScale = false; if (ds.Tables[1].Rows[0]["TestMeasures_TradieusScale"].ToString().Trim().Length > 0) { TestMeasures_TradieusScale = true; }
                bool TestMeasures_OGS = false; if (ds.Tables[1].Rows[0]["TestMeasures_OGS"].ToString().Trim().Length > 0) { TestMeasures_OGS = true; }
                bool TestMeasures_Melbourne = false; if (ds.Tables[1].Rows[0]["TestMeasures_Melbourne"].ToString().Trim().Length > 0) { TestMeasures_Melbourne = true; }
                bool TestMeasures_COPM = false; if (ds.Tables[1].Rows[0]["TestMeasures_COPM"].ToString().Trim().Length > 0) { TestMeasures_COPM = true; }
                bool TestMeasures_ClinicalObservation = false; if (ds.Tables[1].Rows[0]["TestMeasures_ClinicalObservation"].ToString().Trim().Length > 0) { TestMeasures_ClinicalObservation = true; }
                bool TestMeasures_Others = false; if (ds.Tables[1].Rows[0]["TestMeasures_Others"].ToString().Trim().Length > 0) { TestMeasures_Others = true; }

                bool Praxistest = false; if (ds.Tables[1].Rows[0]["Praxistest"].ToString().Trim().Length > 0) { Praxistest = true; }
                bool Designcopying = false; if (ds.Tables[1].Rows[0]["Designcopying"].ToString().Trim().Length > 0) { Designcopying = true; }
                bool ConstructionalPraxis = false; if (ds.Tables[1].Rows[0]["ConstructionalPraxis"].ToString().Trim().Length > 0) { ConstructionalPraxis = true; }
                bool Oralpraxis = false; if (ds.Tables[1].Rows[0]["Oralpraxis"].ToString().Trim().Length > 0) { Oralpraxis = true; }
                bool Posturalpraxis = false; if (ds.Tables[1].Rows[0]["Posturalpraxis"].ToString().Trim().Length > 0) { Posturalpraxis = true; }
                bool Praxisonverbalcommands = false; if (ds.Tables[1].Rows[0]["Praxisonverbalcommands"].ToString().Trim().Length > 0) { Praxisonverbalcommands = true; }
                bool Sequencingpraxis = false; if (ds.Tables[1].Rows[0]["Sequencingpraxis"].ToString().Trim().Length > 0) { Sequencingpraxis = true; }
                bool Sensoryintegrationtests = false; if (ds.Tables[1].Rows[0]["Sensoryintegrationtests"].ToString().Trim().Length > 0) { Sensoryintegrationtests = true; }
                bool Bilateralmotorcoordination = false; if (ds.Tables[1].Rows[0]["Bilateralmotorcoordination"].ToString().Trim().Length > 0) { Bilateralmotorcoordination = true; }
                bool Motoraccuracy = false; if (ds.Tables[1].Rows[0]["Motoraccuracy"].ToString().Trim().Length > 0) { Motoraccuracy = true; }
                bool Postrotatorynystagmus = false; if (ds.Tables[1].Rows[0]["Postrotatorynystagmus"].ToString().Trim().Length > 0) { Postrotatorynystagmus = true; }
                bool Standingwalkingbalance = false; if (ds.Tables[1].Rows[0]["Standingwalkingbalance"].ToString().Trim().Length > 0) { Standingwalkingbalance = true; }
                bool Touchtests = false; if (ds.Tables[1].Rows[0]["Touchtests"].ToString().Trim().Length > 0) { Touchtests = true; }
                bool Graphesthesia = false; if (ds.Tables[1].Rows[0]["Graphesthesia"].ToString().Trim().Length > 0) { Graphesthesia = true; }
                bool Kinesthesia = false; if (ds.Tables[1].Rows[0]["Kinesthesia"].ToString().Trim().Length > 0) { Kinesthesia = true; }
                bool Localizationoftactilestimuli = false; if (ds.Tables[1].Rows[0]["Localizationoftactilestimuli"].ToString().Trim().Length > 0) { Localizationoftactilestimuli = true; }
                bool Manualformperception = false; if (ds.Tables[1].Rows[0]["Manualformperception"].ToString().Trim().Length > 0) { Manualformperception = true; }
                bool Visualperceptiontests = false; if (ds.Tables[1].Rows[0]["Visualperceptiontests"].ToString().Trim().Length > 0) { Visualperceptiontests = true; }
                bool Figuregroundperception = false; if (ds.Tables[1].Rows[0]["Figuregroundperception"].ToString().Trim().Length > 0) { Figuregroundperception = true; }
                bool Spacevisualization = false; if (ds.Tables[1].Rows[0]["Spacevisualization"].ToString().Trim().Length > 0) { Spacevisualization = true; }
                bool Others = false; if (ds.Tables[1].Rows[0]["Others"].ToString().Trim().Length > 0) { Others = true; }
                bool Clockface = false; if (ds.Tables[1].Rows[0]["Clockface"].ToString().Trim().Length > 0) { Clockface = true; }
                bool Motorplanning = false; if (ds.Tables[1].Rows[0]["Motorplanning"].ToString().Trim().Length > 0) { Motorplanning = true; }
                bool Bodyimage = false; if (ds.Tables[1].Rows[0]["Bodyimage"].ToString().Trim().Length > 0) { Bodyimage = true; }
                bool Bodyschema = false; if (ds.Tables[1].Rows[0]["Bodyschema"].ToString().Trim().Length > 0) { Bodyschema = true; }
                bool Laterality = false; if (ds.Tables[1].Rows[0]["Laterality"].ToString().Trim().Length > 0) { Laterality = true; }

                if (Laterality || Bodyschema || Bodyimage || Motorplanning || Clockface || Others || Spacevisualization || Figuregroundperception || Visualperceptiontests || Manualformperception || Localizationoftactilestimuli || Kinesthesia || Graphesthesia || Touchtests || Standingwalkingbalance || Postrotatorynystagmus || Motoraccuracy || Bilateralmotorcoordination || Sensoryintegrationtests || Sequencingpraxis || Praxisonverbalcommands || Posturalpraxis || Oralpraxis || ConstructionalPraxis || Designcopying || Praxistest || TestMeasures_GMFCS || TestMeasures_GMFM || TestMeasures_GMPM || TestMeasures_AshworthScale || TestMeasures_TradieusScale || TestMeasures_OGS || TestMeasures_Melbourne || TestMeasures_COPM || TestMeasures_ClinicalObservation || TestMeasures_Others)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Test And Measures :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (TestMeasures_GMFCS)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("GMFCS :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_GMFCS"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_GMFM)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("GMFM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_GMFM"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_GMPM)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("GMPM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_GMPM"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_AshworthScale)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Ashworth's Scale :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_AshworthScale"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_TradieusScale)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Tradieus Scale :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_TradieusScale"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_OGS)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("OGS :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_OGS"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_Melbourne)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("MELBOURNE :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_Melbourne"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_COPM)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("COPM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_COPM"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_ClinicalObservation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Clinical Observation Of Patient During Free Play :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_ClinicalObservation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Praxistest)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Praxis test", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Praxistest"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Designcopying)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Design copying ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Designcopying"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (ConstructionalPraxis)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Constructional Praxis  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ConstructionalPraxis"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Oralpraxis)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Oral praxis ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Oralpraxis"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posturalpraxis)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Postural praxis ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posturalpraxis"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Praxisonverbalcommands)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Praxis on verbal commands  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Praxisonverbalcommands"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Sequencingpraxis)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Praxis on verbal commands  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sequencingpraxis"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Sensoryintegrationtests)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sensory integration tests ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensoryintegrationtests"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Bilateralmotorcoordination)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Bilateral motor co-ordination  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bilateralmotorcoordination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion


                    #region
                    if (Motoraccuracy)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Motor accuracy  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Motoraccuracy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Postrotatorynystagmus)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Post rotatory nystagmus   ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Postrotatorynystagmus"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion


                    #region
                    if (Standingwalkingbalance)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Standing & walking balance  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Standingwalkingbalance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Touchtests)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Touch tests   ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Touchtests"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Graphesthesia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Graphesthesia    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Graphesthesia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Kinesthesia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Kinesthesia    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Kinesthesia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Localizationoftactilestimuli)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Localization of tactile stimuli    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Localizationoftactilestimuli"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Manualformperception)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Manual form perception   ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Manualformperception"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Visualperceptiontests)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Visual perception tests    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Visualperceptiontests"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Figuregroundperception)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Figure ground perception     ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Figuregroundperception"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Spacevisualization)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Space visualization   ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Spacevisualization"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Others)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Others  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Others"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Clockface)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Clock face   ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Clockface"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Motorplanning)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Motor planning    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Motorplanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Bodyimage)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Body image     ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bodyimage"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Bodyschema)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Body schema    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bodyschema"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Laterality)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Laterality    ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Laterality"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (TestMeasures_Others)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Others :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeasures_Others"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Posture *********************
                bool Posture_Alignment = false; if (ds.Tables[1].Rows[0]["Posture_Alignment"].ToString().Trim().Length > 0) { Posture_Alignment = true; }
                bool Posture_Biomechanics = false; if (ds.Tables[1].Rows[0]["Posture_Biomechanics"].ToString().Trim().Length > 0) { Posture_Biomechanics = true; }
                bool Posture_Stability = false; if (ds.Tables[1].Rows[0]["Posture_Stability"].ToString().Trim().Length > 0) { Posture_Stability = true; }
                bool Posture_Anticipatory = false; if (ds.Tables[1].Rows[0]["Posture_Anticipatory"].ToString().Trim().Length > 0) { Posture_Anticipatory = true; }
                bool Posture_Postural = false; if (ds.Tables[1].Rows[0]["Posture_Postural"].ToString().Trim().Length > 0) { Posture_Postural = true; }
                bool Posture_SignsPostural = false; if (ds.Tables[1].Rows[0]["Posture_SignsPostural"].ToString().Trim().Length > 0) { Posture_SignsPostural = true; }
                if (Posture_Alignment || Posture_Biomechanics || Posture_Stability || Posture_Anticipatory || Posture_Postural || Posture_SignsPostural)
                {
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;
                    cell = PhraseCell(new Phrase("Posture :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Posture_Alignment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Alignment(Head/Neck,Spine,Shoulder,Girdle,UE's,LE's) :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_Alignment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posture_Biomechanics)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("BOS/COM(Biomechanics) :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_Biomechanics"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posture_Stability)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Strategies for Stability(Posture tone,muscule synergies or biomechanical strategies utilized) :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_Stability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posture_Anticipatory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Anticipatory Control Set For Movement :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_Anticipatory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posture_Postural)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Postural Counter Balance During Movement :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_Postural"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Posture_SignsPostural)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Signs of Postural System Impairments(Muscular Architecture,General Posture) :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Posture_SignsPostural"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Movement *********************
                bool Movement_Inertia = false; if (ds.Tables[1].Rows[0]["Movement_Inertia"].ToString().Trim().Length > 0) { Movement_Inertia = true; }
                bool Movement_Strategies = false; if (ds.Tables[1].Rows[0]["Movement_Strategies"].ToString().Trim().Length > 0) { Movement_Strategies = true; }
                bool Movement_Extremities = false; if (ds.Tables[1].Rows[0]["Movement_Extremities"].ToString().Trim().Length > 0) { Movement_Extremities = true; }
                bool Movement_Stability = false; if (ds.Tables[1].Rows[0]["Movement_Stability"].ToString().Trim().Length > 0) { Movement_Stability = true; }
                bool Movement_Overuse = false; if (ds.Tables[1].Rows[0]["Movement_Overuse"].ToString().Trim().Length > 0) { Movement_Overuse = true; }
                if (Movement_Inertia || Movement_Strategies || Movement_Extremities || Movement_Stability || Movement_Overuse)
                {
                    cell = PhraseCell(new Phrase("Movement :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Movement_Inertia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How does child overcome inertia? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_Inertia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Movement_Strategies)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Movement Strategies :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_Strategies"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Movement_Extremities)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Discuss range,speed of movement,Consider both trunk and extremities :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_Extremities"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Movement_Stability)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Strategies for stability, Mobility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_Stability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Movement_Overuse)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sign of movement system impairment or overuse :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_Overuse"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Regulatory *********************
                bool Regulatory_Arousal = false; if (ds.Tables[1].Rows[0]["Regulatory_Arousal"].ToString().Trim().Length > 0) { Regulatory_Arousal = true; }
                bool Regulatory_Regulation = false; if (ds.Tables[1].Rows[0]["Regulatory_Regulation"].ToString().Trim().Length > 0) { Regulatory_Regulation = true; }
                if (Regulatory_Arousal || Regulatory_Regulation)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Regulatory :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Regulatory_Arousal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Arousal :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Regulatory_Arousal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Regulatory_Regulation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("State Regulation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Regulatory_Regulation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ********************* Musculoskeletal **********************
                bool Musculoskeletal_Rom1 = false;
                bool Musculoskeletal_Rom1_HipFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipFlexionLeft = true; }
                bool Musculoskeletal_Rom1_HipFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipFlexionRight = true; }
                bool Musculoskeletal_Rom1_HipExtensionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipExtensionLeft = true; }
                bool Musculoskeletal_Rom1_HipExtensionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipExtensionRight = true; }
                bool Musculoskeletal_Rom1_HipAbductionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipAbductionLeft = true; }
                bool Musculoskeletal_Rom1_HipAbductionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipAbductionRight = true; }
                bool Musculoskeletal_Rom1_HipExternalLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipExternalLeft = true; }
                bool Musculoskeletal_Rom1_HipExternalRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipExternalRight = true; }
                bool Musculoskeletal_Rom1_HipInternalLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipInternalLeft = true; }
                bool Musculoskeletal_Rom1_HipInternalRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_HipInternalRight = true; }
                bool Musculoskeletal_Rom1_PoplitealLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_PoplitealLeft = true; }
                bool Musculoskeletal_Rom1_PoplitealRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_PoplitealRight = true; }
                bool Musculoskeletal_Rom1_KneeFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_KneeFlexionLeft = true; }
                bool Musculoskeletal_Rom1_KneeFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_KneeFlexionRight = true; }
                bool Musculoskeletal_Rom1_KneeExtensionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_KneeExtensionLeft = true; }
                bool Musculoskeletal_Rom1_KneeExtensionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_KneeExtensionRight = true; }
                bool Musculoskeletal_Rom1_DorsiflexionFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_DorsiflexionFlexionLeft = true; }
                bool Musculoskeletal_Rom1_DorsiflexionFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_DorsiflexionFlexionRight = true; }
                bool Musculoskeletal_Rom1_DorsiflexionExtensionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_DorsiflexionExtensionLeft = true; }
                bool Musculoskeletal_Rom1_DorsiflexionExtensionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_DorsiflexionExtensionRight = true; }
                bool Musculoskeletal_Rom1_PlantarFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_PlantarFlexionLeft = true; }
                bool Musculoskeletal_Rom1_PlantarFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_PlantarFlexionRight = true; }
                bool Musculoskeletal_Rom1_OthersLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_OthersLeft = true; }
                bool Musculoskeletal_Rom1_OthersRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom1_OthersRight = true; }
                if (Musculoskeletal_Rom1_HipFlexionLeft || Musculoskeletal_Rom1_HipFlexionRight || Musculoskeletal_Rom1_HipExtensionLeft || Musculoskeletal_Rom1_HipExtensionRight || Musculoskeletal_Rom1_HipAbductionLeft || Musculoskeletal_Rom1_HipAbductionRight || Musculoskeletal_Rom1_HipExternalLeft || Musculoskeletal_Rom1_HipExternalRight || Musculoskeletal_Rom1_HipInternalLeft || Musculoskeletal_Rom1_HipInternalRight || Musculoskeletal_Rom1_PoplitealLeft || Musculoskeletal_Rom1_PoplitealRight || Musculoskeletal_Rom1_KneeFlexionLeft || Musculoskeletal_Rom1_KneeFlexionRight || Musculoskeletal_Rom1_KneeExtensionLeft || Musculoskeletal_Rom1_KneeExtensionRight || Musculoskeletal_Rom1_DorsiflexionFlexionLeft || Musculoskeletal_Rom1_DorsiflexionFlexionRight || Musculoskeletal_Rom1_DorsiflexionExtensionLeft || Musculoskeletal_Rom1_DorsiflexionExtensionRight || Musculoskeletal_Rom1_PlantarFlexionLeft || Musculoskeletal_Rom1_PlantarFlexionRight || Musculoskeletal_Rom1_OthersLeft || Musculoskeletal_Rom1_OthersRight)
                {
                    Musculoskeletal_Rom1 = true;
                }

                bool Musculoskeletal_Rom2_ShoulderFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ShoulderFlexionLeft = true; }
                bool Musculoskeletal_Rom2_ShoulderFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ShoulderFlexionRight = true; }
                bool Musculoskeletal_Rom2_ShoulderExtensionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ShoulderExtensionLeft = true; }
                bool Musculoskeletal_Rom2_ShoulderExtensionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ShoulderExtensionRight = true; }
                bool Musculoskeletal_Rom2_HorizontalAbductionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_HorizontalAbductionLeft = true; }
                bool Musculoskeletal_Rom2_HorizontalAbductionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_HorizontalAbductionRight = true; }
                bool Musculoskeletal_Rom2_ExternalRotationLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ExternalRotationLeft = true; }
                bool Musculoskeletal_Rom2_ExternalRotationRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ExternalRotationRight = true; }
                bool Musculoskeletal_Rom2_InternalRotationLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_InternalRotationLeft = true; }
                bool Musculoskeletal_Rom2_InternalRotationRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_InternalRotationRight = true; }
                bool Musculoskeletal_Rom2_ElbowFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ElbowFlexionLeft = true; }
                bool Musculoskeletal_Rom2_ElbowFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ElbowFlexionRight = true; }
                bool Musculoskeletal_Rom2_ElbowExtensionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ElbowExtensionLeft = true; }
                bool Musculoskeletal_Rom2_ElbowExtensionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_ElbowExtensionRight = true; }
                bool Musculoskeletal_Rom2_SupinationLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_SupinationLeft = true; }
                bool Musculoskeletal_Rom2_SupinationRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_SupinationRight = true; }
                bool Musculoskeletal_Rom2_PronationLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_PronationLeft = true; }
                bool Musculoskeletal_Rom2_PronationRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_PronationRight = true; }
                bool Musculoskeletal_Rom2_WristFlexionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_WristFlexionLeft = true; }
                bool Musculoskeletal_Rom2_WristFlexionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_WristFlexionRight = true; }
                bool Musculoskeletal_Rom2_WristExtesionLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_WristExtesionLeft = true; }
                bool Musculoskeletal_Rom2_WristExtesionRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_WristExtesionRight = true; }
                bool Musculoskeletal_Rom2_OthersLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_OthersLeft = true; }
                bool Musculoskeletal_Rom2_OthersRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersRight"].ToString().Trim().Length > 0) { Musculoskeletal_Rom2_OthersRight = true; }

                bool Musculoskeletal_Rom2 = false;
                if (Musculoskeletal_Rom2_ShoulderFlexionLeft || Musculoskeletal_Rom2_ShoulderFlexionRight || Musculoskeletal_Rom2_ShoulderExtensionLeft || Musculoskeletal_Rom2_ShoulderExtensionRight || Musculoskeletal_Rom2_HorizontalAbductionLeft || Musculoskeletal_Rom2_HorizontalAbductionRight || Musculoskeletal_Rom2_ExternalRotationLeft || Musculoskeletal_Rom2_ExternalRotationRight || Musculoskeletal_Rom2_InternalRotationLeft || Musculoskeletal_Rom2_InternalRotationRight || Musculoskeletal_Rom2_ElbowFlexionLeft || Musculoskeletal_Rom2_ElbowFlexionRight || Musculoskeletal_Rom2_ElbowExtensionLeft || Musculoskeletal_Rom2_ElbowExtensionRight || Musculoskeletal_Rom2_SupinationLeft || Musculoskeletal_Rom2_SupinationRight || Musculoskeletal_Rom2_PronationLeft || Musculoskeletal_Rom2_PronationRight || Musculoskeletal_Rom2_WristFlexionLeft || Musculoskeletal_Rom2_WristFlexionRight || Musculoskeletal_Rom2_WristExtesionLeft || Musculoskeletal_Rom2_WristExtesionRight || Musculoskeletal_Rom2_OthersLeft || Musculoskeletal_Rom2_OthersRight)
                {
                    Musculoskeletal_Rom2 = true;
                }

                bool Musculoskeletal_Strengthlp = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Strengthlp"].ToString().Trim().Length > 0) { Musculoskeletal_Strengthlp = true; }
                bool Musculoskeletal_StrengthCC = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_StrengthCC"].ToString().Trim().Length > 0) { Musculoskeletal_StrengthCC = true; }
                bool Musculoskeletal_StrengthMuscle = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_StrengthMuscle"].ToString().Trim().Length > 0) { Musculoskeletal_StrengthMuscle = true; }
                bool Musculoskeletal_StrengthSkeletal = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_StrengthSkeletal"].ToString().Trim().Length > 0) { Musculoskeletal_StrengthSkeletal = true; }
                bool Musculoskeletal_Strength = false; if (Musculoskeletal_Strengthlp || Musculoskeletal_StrengthCC || Musculoskeletal_StrengthMuscle || Musculoskeletal_StrengthSkeletal) { Musculoskeletal_Strength = true; }

                bool Musculoskeletal_Mmt_HipflexorsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_HipflexorsLeft = true; }
                bool Musculoskeletal_Mmt_HipflexorsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_HipflexorsRight = true; }
                bool Musculoskeletal_Mmt_AbductorsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AbductorsLeft = true; }
                bool Musculoskeletal_Mmt_AbductorsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AbductorsRight = true; }
                bool Musculoskeletal_Mmt_ExtensorsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorsLeft = true; }
                bool Musculoskeletal_Mmt_ExtensorsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorsRight = true; }
                bool Musculoskeletal_Mmt_HamsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_HamsLeft = true; }
                bool Musculoskeletal_Mmt_HamsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_HamsRight = true; }
                bool Musculoskeletal_Mmt_QuadsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_QuadsLeft = true; }
                bool Musculoskeletal_Mmt_QuadsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_QuadsRight = true; }
                bool Musculoskeletal_Mmt_TibialisAnteriorLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TibialisAnteriorLeft = true; }
                bool Musculoskeletal_Mmt_TibialisAnteriorRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TibialisAnteriorRight = true; }
                bool Musculoskeletal_Mmt_TibialisPosteriorLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TibialisPosteriorLeft = true; }
                bool Musculoskeletal_Mmt_TibialisPosteriorRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TibialisPosteriorRight = true; }
                bool Musculoskeletal_Mmt_ExtensorDigitorumLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorDigitorumLeft = true; }
                bool Musculoskeletal_Mmt_ExtensorDigitorumRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorDigitorumRight = true; }
                bool Musculoskeletal_Mmt_ExtensorHallucisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorHallucisLeft = true; }
                bool Musculoskeletal_Mmt_ExtensorHallucisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorHallucisRight = true; }
                bool Musculoskeletal_Mmt_PeroneiLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PeroneiLeft = true; }
                bool Musculoskeletal_Mmt_PeroneiRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PeroneiRight = true; }
                bool Musculoskeletal_Mmt_FlexorDigitorumLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorDigitorumLeft = true; }
                bool Musculoskeletal_Mmt_FlexorDigitorumRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorDigitorumRight = true; }
                bool Musculoskeletal_Mmt_FlexorHallucisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorHallucisLeft = true; }
                bool Musculoskeletal_Mmt_FlexorHallucisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorHallucisRight = true; }
                bool Musculoskeletal_Mmt_AnteriorDeltoidLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AnteriorDeltoidLeft = true; }
                bool Musculoskeletal_Mmt_AnteriorDeltoidRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AnteriorDeltoidRight = true; }
                bool Musculoskeletal_Mmt_PosteriorDeltoidLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PosteriorDeltoidLeft = true; }
                bool Musculoskeletal_Mmt_PosteriorDeltoidRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PosteriorDeltoidRight = true; }
                bool Musculoskeletal_Mmt_MiddleDeltoidLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_MiddleDeltoidLeft = true; }
                bool Musculoskeletal_Mmt_MiddleDeltoidRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_MiddleDeltoidRight = true; }
                bool Musculoskeletal_Mmt_SupraspinatusLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SupraspinatusLeft = true; }
                bool Musculoskeletal_Mmt_SupraspinatusRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SupraspinatusRight = true; }
                bool Musculoskeletal_Mmt_SerratusAnteriorLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SerratusAnteriorLeft = true; }
                bool Musculoskeletal_Mmt_SerratusAnteriorRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SerratusAnteriorRight = true; }
                bool Musculoskeletal_Mmt_RhomboidsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_RhomboidsLeft = true; }
                bool Musculoskeletal_Mmt_RhomboidsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_RhomboidsRight = true; }
                bool Musculoskeletal_Mmt_BicepsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_BicepsLeft = true; }
                bool Musculoskeletal_Mmt_BicepsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_BicepsRight = true; }
                bool Musculoskeletal_Mmt_TricepsLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TricepsLeft = true; }
                bool Musculoskeletal_Mmt_TricepsRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_TricepsRight = true; }
                bool Musculoskeletal_Mmt_SupinatorLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SupinatorLeft = true; }
                bool Musculoskeletal_Mmt_SupinatorRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_SupinatorRight = true; }
                bool Musculoskeletal_Mmt_PronatorLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PronatorLeft = true; }
                bool Musculoskeletal_Mmt_PronatorRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_PronatorRight = true; }
                bool Musculoskeletal_Mmt_ECULeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECULeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECULeft = true; }
                bool Musculoskeletal_Mmt_ECURight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECURight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECURight = true; }
                bool Musculoskeletal_Mmt_ECRLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECRLeft = true; }
                bool Musculoskeletal_Mmt_ECRRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECRRight = true; }
                bool Musculoskeletal_Mmt_ECSLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECSLeft = true; }
                bool Musculoskeletal_Mmt_ECSRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ECSRight = true; }
                bool Musculoskeletal_Mmt_FCULeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCULeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCULeft = true; }
                bool Musculoskeletal_Mmt_FCURight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCURight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCURight = true; }
                bool Musculoskeletal_Mmt_FCRLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCRLeft = true; }
                bool Musculoskeletal_Mmt_FCRRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCRRight = true; }
                bool Musculoskeletal_Mmt_FCSLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCSLeft = true; }
                bool Musculoskeletal_Mmt_FCSRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FCSRight = true; }
                bool Musculoskeletal_Mmt_OpponensPollicisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_OpponensPollicisLeft = true; }
                bool Musculoskeletal_Mmt_OpponensPollicisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_OpponensPollicisRight = true; }
                bool Musculoskeletal_Mmt_FlexorPollicisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorPollicisLeft = true; }
                bool Musculoskeletal_Mmt_FlexorPollicisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_FlexorPollicisRight = true; }
                bool Musculoskeletal_Mmt_AbductorPollicisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AbductorPollicisLeft = true; }
                bool Musculoskeletal_Mmt_AbductorPollicisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_AbductorPollicisRight = true; }
                bool Musculoskeletal_Mmt_ExtensorPollicisLeft = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisLeft"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorPollicisLeft = true; }
                bool Musculoskeletal_Mmt_ExtensorPollicisRight = false; if (ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisRight"].ToString().Trim().Length > 0) { Musculoskeletal_Mmt_ExtensorPollicisRight = true; }
                bool Musculoskeletal_Mmt = false;
                if (Musculoskeletal_Mmt_HipflexorsLeft || Musculoskeletal_Mmt_HipflexorsRight || Musculoskeletal_Mmt_AbductorsLeft || Musculoskeletal_Mmt_AbductorsRight || Musculoskeletal_Mmt_ExtensorsLeft || Musculoskeletal_Mmt_ExtensorsRight || Musculoskeletal_Mmt_HamsLeft || Musculoskeletal_Mmt_HamsRight || Musculoskeletal_Mmt_QuadsLeft || Musculoskeletal_Mmt_QuadsRight || Musculoskeletal_Mmt_TibialisAnteriorLeft || Musculoskeletal_Mmt_TibialisAnteriorRight || Musculoskeletal_Mmt_TibialisPosteriorLeft || Musculoskeletal_Mmt_TibialisPosteriorRight || Musculoskeletal_Mmt_ExtensorDigitorumLeft || Musculoskeletal_Mmt_ExtensorDigitorumRight || Musculoskeletal_Mmt_ExtensorHallucisLeft || Musculoskeletal_Mmt_ExtensorHallucisRight || Musculoskeletal_Mmt_PeroneiLeft || Musculoskeletal_Mmt_PeroneiRight || Musculoskeletal_Mmt_FlexorDigitorumLeft || Musculoskeletal_Mmt_FlexorDigitorumRight || Musculoskeletal_Mmt_FlexorHallucisLeft || Musculoskeletal_Mmt_FlexorHallucisRight || Musculoskeletal_Mmt_AnteriorDeltoidLeft || Musculoskeletal_Mmt_AnteriorDeltoidRight || Musculoskeletal_Mmt_PosteriorDeltoidLeft || Musculoskeletal_Mmt_PosteriorDeltoidRight || Musculoskeletal_Mmt_MiddleDeltoidLeft || Musculoskeletal_Mmt_MiddleDeltoidRight ||
                    Musculoskeletal_Mmt_SupraspinatusLeft || Musculoskeletal_Mmt_SupraspinatusRight || Musculoskeletal_Mmt_SerratusAnteriorLeft || Musculoskeletal_Mmt_SerratusAnteriorRight || Musculoskeletal_Mmt_RhomboidsLeft || Musculoskeletal_Mmt_RhomboidsRight || Musculoskeletal_Mmt_BicepsLeft || Musculoskeletal_Mmt_BicepsRight || Musculoskeletal_Mmt_TricepsLeft || Musculoskeletal_Mmt_TricepsRight || Musculoskeletal_Mmt_SupinatorLeft || Musculoskeletal_Mmt_SupinatorRight || Musculoskeletal_Mmt_PronatorLeft || Musculoskeletal_Mmt_PronatorRight || Musculoskeletal_Mmt_ECULeft || Musculoskeletal_Mmt_ECURight || Musculoskeletal_Mmt_ECRLeft || Musculoskeletal_Mmt_ECRRight || Musculoskeletal_Mmt_ECSLeft || Musculoskeletal_Mmt_ECSRight || Musculoskeletal_Mmt_FCULeft || Musculoskeletal_Mmt_FCURight || Musculoskeletal_Mmt_FCRLeft || Musculoskeletal_Mmt_FCRRight || Musculoskeletal_Mmt_FCSLeft || Musculoskeletal_Mmt_FCSRight || Musculoskeletal_Mmt_OpponensPollicisLeft || Musculoskeletal_Mmt_OpponensPollicisRight || Musculoskeletal_Mmt_FlexorPollicisLeft || Musculoskeletal_Mmt_FlexorPollicisRight || Musculoskeletal_Mmt_AbductorPollicisLeft || Musculoskeletal_Mmt_AbductorPollicisRight || Musculoskeletal_Mmt_ExtensorPollicisLeft || Musculoskeletal_Mmt_ExtensorPollicisRight)
                {
                    Musculoskeletal_Mmt = true;
                }
                if (Musculoskeletal_Rom1 || Musculoskeletal_Rom2 || Musculoskeletal_Strength || Musculoskeletal_Mmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Musculoskeletal :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region ********************** ROM 1 ***************************
                    if (Musculoskeletal_Rom1)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("ROM - 1 :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("List", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (Musculoskeletal_Rom1_HipFlexionLeft || Musculoskeletal_Rom1_HipFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_HipExtensionLeft || Musculoskeletal_Rom1_HipExtensionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip Extension", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExtensionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_HipAbductionLeft || Musculoskeletal_Rom1_HipAbductionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip Abduction", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipAbductionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_HipExternalLeft || Musculoskeletal_Rom1_HipExternalRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip External Rotation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipExternalRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_HipInternalLeft || Musculoskeletal_Rom1_HipInternalRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip Internal Rotation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_HipInternalRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_PoplitealLeft || Musculoskeletal_Rom1_PoplitealRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Popliteal Angle", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PoplitealRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_KneeFlexionLeft || Musculoskeletal_Rom1_KneeFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Knee Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_KneeExtensionLeft || Musculoskeletal_Rom1_KneeExtensionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Knee Extension", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_KneeExtensionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_DorsiflexionFlexionLeft || Musculoskeletal_Rom1_DorsiflexionFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Dorsiflexion With Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_DorsiflexionExtensionLeft || Musculoskeletal_Rom1_DorsiflexionExtensionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Dorsiflexion With Extension", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_DorsiflexionExtensionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_PlantarFlexionLeft || Musculoskeletal_Rom1_PlantarFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Plantar Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_PlantarFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom1_OthersLeft || Musculoskeletal_Rom1_OthersRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Others", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom1_OthersRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region ********************* ROM 2 **************************
                    if (Musculoskeletal_Rom2)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("ROM - 2 :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("List", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (Musculoskeletal_Rom2_ShoulderFlexionLeft || Musculoskeletal_Rom2_ShoulderFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Shoulder Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_ShoulderExtensionLeft || Musculoskeletal_Rom2_ShoulderExtensionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Shoulder Extension", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ShoulderExtensionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_HorizontalAbductionLeft || Musculoskeletal_Rom2_HorizontalAbductionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Horizontal Abduction", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_HorizontalAbductionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_ExternalRotationLeft || Musculoskeletal_Rom2_ExternalRotationRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("External Rotation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ExternalRotationRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_InternalRotationLeft || Musculoskeletal_Rom2_InternalRotationRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Internal Rotation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_InternalRotationRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_ElbowFlexionLeft || Musculoskeletal_Rom2_ElbowFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Elbow Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_ElbowExtensionLeft || Musculoskeletal_Rom2_ElbowExtensionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Elbow Extension", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_ElbowExtensionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_SupinationLeft || Musculoskeletal_Rom2_SupinationRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Supination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_SupinationRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_PronationLeft || Musculoskeletal_Rom2_PronationRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Pronation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_PronationRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_WristFlexionLeft || Musculoskeletal_Rom2_WristFlexionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Wrist Flexion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristFlexionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_WristExtesionLeft || Musculoskeletal_Rom2_WristExtesionRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Wrist Extesion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_WristExtesionRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Rom2_OthersLeft || Musculoskeletal_Rom2_OthersRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Others", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Rom2_OthersRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region ******************** Strength ***************
                    if (Musculoskeletal_Strength)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Strength :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                        if (Musculoskeletal_Strengthlp)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("lp(In pattern) :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Strengthlp"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Musculoskeletal_StrengthCC)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("CC(Consious Control) :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_StrengthCC"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Musculoskeletal_StrengthMuscle)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Muscle Endurance :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_StrengthMuscle"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (Musculoskeletal_StrengthSkeletal)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Skeletal Comments :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_StrengthSkeletal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region ******************** MMT **************************
                    if (Musculoskeletal_Mmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("MMT :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("List", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (Musculoskeletal_Mmt_HipflexorsLeft || Musculoskeletal_Mmt_HipflexorsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip flexors", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HipflexorsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_AbductorsLeft || Musculoskeletal_Mmt_AbductorsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Abductors", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ExtensorsLeft || Musculoskeletal_Mmt_ExtensorsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Extensors", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_HamsLeft || Musculoskeletal_Mmt_HamsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hams", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_HamsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_QuadsLeft || Musculoskeletal_Mmt_QuadsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Quads", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_QuadsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_TibialisAnteriorLeft || Musculoskeletal_Mmt_TibialisAnteriorRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Tibialis Anterior", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisAnteriorRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_TibialisPosteriorLeft || Musculoskeletal_Mmt_TibialisPosteriorRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Tibialis Posterior", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TibialisPosteriorRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ExtensorDigitorumLeft || Musculoskeletal_Mmt_ExtensorDigitorumRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Extensor Digitorum", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorDigitorumRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ExtensorHallucisLeft || Musculoskeletal_Mmt_ExtensorHallucisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Extensor Hallucis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorHallucisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_PeroneiLeft || Musculoskeletal_Mmt_PeroneiRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Peronei", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PeroneiRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FlexorDigitorumLeft || Musculoskeletal_Mmt_FlexorDigitorumRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Flexor Digitorum", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorDigitorumRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FlexorHallucisLeft || Musculoskeletal_Mmt_FlexorHallucisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Flexor Hallucis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorHallucisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_AnteriorDeltoidLeft || Musculoskeletal_Mmt_AnteriorDeltoidRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Anterior Deltoid", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AnteriorDeltoidRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_PosteriorDeltoidLeft || Musculoskeletal_Mmt_PosteriorDeltoidRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Posterior Deltoid", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PosteriorDeltoidRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_MiddleDeltoidLeft || Musculoskeletal_Mmt_MiddleDeltoidRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Middle Deltoid", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_MiddleDeltoidRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_SupraspinatusLeft || Musculoskeletal_Mmt_SupraspinatusRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Supraspinatus", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupraspinatusRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_SerratusAnteriorLeft || Musculoskeletal_Mmt_SerratusAnteriorRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Serratus Anterior", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SerratusAnteriorRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_RhomboidsLeft || Musculoskeletal_Mmt_RhomboidsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Rhomboids", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_RhomboidsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_BicepsLeft || Musculoskeletal_Mmt_BicepsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Biceps", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_BicepsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_TricepsLeft || Musculoskeletal_Mmt_TricepsRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Triceps", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_TricepsRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_SupinatorLeft || Musculoskeletal_Mmt_SupinatorRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Supinator", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_SupinatorRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_PronatorLeft || Musculoskeletal_Mmt_PronatorRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Pronator", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_PronatorRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ECULeft || Musculoskeletal_Mmt_ECURight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("ECU", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECULeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECURight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ECRLeft || Musculoskeletal_Mmt_ECRRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("ECR", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECRRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ECSLeft || Musculoskeletal_Mmt_ECSRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("ECS", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ECSRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FCULeft || Musculoskeletal_Mmt_FCURight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("FCU", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCULeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCURight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FCRLeft || Musculoskeletal_Mmt_FCRRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("FCR", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCRRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FCSLeft || Musculoskeletal_Mmt_FCSRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("FCS", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FCSRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_OpponensPollicisLeft || Musculoskeletal_Mmt_OpponensPollicisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Opponens Pollicis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_OpponensPollicisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_FlexorPollicisLeft || Musculoskeletal_Mmt_FlexorPollicisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Flexor Pollicis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_FlexorPollicisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_AbductorPollicisLeft || Musculoskeletal_Mmt_AbductorPollicisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Abductor Pollicis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_AbductorPollicisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_ExtensorPollicisLeft || Musculoskeletal_Mmt_ExtensorPollicisRight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Extensor Pollicis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_ExtensorPollicisRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ******************** Sign of CNS ********************
                bool SignOfCNS_NeuromotorControl = false; if (ds.Tables[1].Rows[0]["SignOfCNS_NeuromotorControl"].ToString().Trim().Length > 0) { SignOfCNS_NeuromotorControl = true; }
                if (SignOfCNS_NeuromotorControl)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Sign of CNS :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SignOfCNS_NeuromotorControl)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Neuromotor Control and Coordination Sign of CNS integrity/impairment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SignOfCNS_NeuromotorControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Remarks Variable ******************
                bool RemarkVariable_SustainGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SustainGeneral"].ToString().Trim().Length > 0) { RemarkVariable_SustainGeneral = true; }
                bool RemarkVariable_SustainControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SustainControl"].ToString().Trim().Length > 0) { RemarkVariable_SustainControl = true; }
                bool RemarkVariable_SustainTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SustainTiming"].ToString().Trim().Length > 0) { RemarkVariable_SustainTiming = true; }
                bool RemarkVariable_PosturalGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_PosturalGeneral"].ToString().Trim().Length > 0) { RemarkVariable_PosturalGeneral = true; }
                bool RemarkVariable_PosturalControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_PosturalControl"].ToString().Trim().Length > 0) { RemarkVariable_PosturalControl = true; }
                bool RemarkVariable_PosturalTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_PosturalTiming"].ToString().Trim().Length > 0) { RemarkVariable_PosturalTiming = true; }
                bool RemarkVariable_ContractionsGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ContractionsGeneral"].ToString().Trim().Length > 0) { RemarkVariable_ContractionsGeneral = true; }
                bool RemarkVariable_ContractionsControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ContractionsControl"].ToString().Trim().Length > 0) { RemarkVariable_ContractionsControl = true; }
                bool RemarkVariable_ContractionsTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ContractionsTiming"].ToString().Trim().Length > 0) { RemarkVariable_ContractionsTiming = true; }
                bool RemarkVariable_AntagonistGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_AntagonistGeneral"].ToString().Trim().Length > 0) { RemarkVariable_AntagonistGeneral = true; }
                bool RemarkVariable_AntagonistControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_AntagonistControl"].ToString().Trim().Length > 0) { RemarkVariable_AntagonistControl = true; }
                bool RemarkVariable_AntagonistTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_AntagonistTiming"].ToString().Trim().Length > 0) { RemarkVariable_AntagonistTiming = true; }
                bool RemarkVariable_SynergyGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SynergyGeneral"].ToString().Trim().Length > 0) { RemarkVariable_SynergyGeneral = true; }
                bool RemarkVariable_SynergyControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SynergyControl"].ToString().Trim().Length > 0) { RemarkVariable_SynergyControl = true; }
                bool RemarkVariable_SynergyTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_SynergyTiming"].ToString().Trim().Length > 0) { RemarkVariable_SynergyTiming = true; }
                bool RemarkVariable_StiffinessGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_StiffinessGeneral"].ToString().Trim().Length > 0) { RemarkVariable_StiffinessGeneral = true; }
                bool RemarkVariable_StiffinessControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_StiffinessControl"].ToString().Trim().Length > 0) { RemarkVariable_StiffinessControl = true; }
                bool RemarkVariable_StiffinessTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_StiffinessTiming"].ToString().Trim().Length > 0) { RemarkVariable_StiffinessTiming = true; }
                bool RemarkVariable_ExtraneousGeneral = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousGeneral"].ToString().Trim().Length > 0) { RemarkVariable_ExtraneousGeneral = true; }
                bool RemarkVariable_ExtraneousControl = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousControl"].ToString().Trim().Length > 0) { RemarkVariable_ExtraneousControl = true; }
                bool RemarkVariable_ExtraneousTiming = false; if (ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousTiming"].ToString().Trim().Length > 0) { RemarkVariable_ExtraneousTiming = true; }
                if (RemarkVariable_SustainGeneral || RemarkVariable_SustainControl || RemarkVariable_SustainTiming || RemarkVariable_PosturalGeneral || RemarkVariable_PosturalControl || RemarkVariable_PosturalTiming || RemarkVariable_ContractionsGeneral || RemarkVariable_ContractionsControl || RemarkVariable_ContractionsTiming || RemarkVariable_AntagonistGeneral || RemarkVariable_AntagonistControl || RemarkVariable_AntagonistTiming || RemarkVariable_SynergyGeneral || RemarkVariable_SynergyControl || RemarkVariable_SynergyTiming || RemarkVariable_StiffinessGeneral || RemarkVariable_StiffinessControl || RemarkVariable_StiffinessTiming || RemarkVariable_ExtraneousGeneral || RemarkVariable_ExtraneousControl || RemarkVariable_ExtraneousTiming)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Remarks Variable :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 33.3f, 33.3f, 33.3f, 33.3f });
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("Variables", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    cell = new PdfPCell(PhraseCell(new Phrase("General Comments", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    cell = new PdfPCell(PhraseCell(new Phrase("Control and Gradation", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    cell = new PdfPCell(PhraseCell(new Phrase("Co-ordination and Timing", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Ability to initate sustain,to initate sustain,and terminate muscle activity.", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SustainGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SustainControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SustainTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Recruitment of postural(SO)and phasic or movement(FF) motor unit.", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_PosturalGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_PosturalControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_PosturalTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Ability to perform concentric, isometric, and eccentric muscle contractions.", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ContractionsGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ContractionsControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ContractionsTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Recruitment of cocontraction and/or reciprocal inhibition of agonist and antagonist.", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_AntagonistGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_AntagonistControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_AntagonistTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Synergy selectivity( mas vs.isolated,repertoire).", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SynergyGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SynergyControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_SynergyTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Stiffiness(delta F/delta L) .", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_StiffinessGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_StiffinessControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_StiffinessTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    cell = new PdfPCell(PhraseCell(new Phrase("Extraneous movements (tremors,clonus,nystagmus, etc .", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousGeneral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["RemarkVariable_ExtraneousTiming"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);
                    /**/
                    document.Add(table);

                    #endregion
                }
                #endregion

                #region ****************** Sensory System Old **************
                bool SensorySystem_Vision = false; if (ds.Tables[1].Rows[0]["SensorySystem_Vision"].ToString().Trim().Length > 0) { SensorySystem_Vision = true; }
                bool SensorySystem_Somatosensory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Somatosensory"].ToString().Trim().Length > 0) { SensorySystem_Somatosensory = true; }
                bool SensorySystem_Vestibular = false; if (ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString().Trim().Length > 0) { SensorySystem_Vestibular = true; }
                bool SensorySystem_Auditory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString().Trim().Length > 0) { SensorySystem_Auditory = true; }
                bool SensorySystem_Gustatory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString().Trim().Length > 0) { SensorySystem_Gustatory = true; }
                if (SensorySystem_Vision || SensorySystem_Somatosensory || SensorySystem_Vestibular || SensorySystem_Auditory || SensorySystem_Gustatory)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Sensory System :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SensorySystem_Vision)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Vision :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Vision"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Somatosensory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Somatosensory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Somatosensory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Vestibular)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Vestibular :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Auditory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Auditory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Gustatory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Gustatory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Sensory System **************
                bool SensorySystemsVisual_Focal = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Focal"].ToString().Trim().Length > 0) { SensorySystemsVisual_Focal = true; }
                bool SensorySystemsVisual_Ambient = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Ambient"].ToString().Trim().Length > 0) { SensorySystemsVisual_Ambient = true; }
                bool SensorySystemsVisual_Focus = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Focus"].ToString().Trim().Length > 0) { SensorySystemsVisual_Focus = true; }
                bool SensorySystemsVisual_Depth = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Depth"].ToString().Trim().Length > 0) { SensorySystemsVisual_Depth = true; }
                bool SensorySystemsVisual_Refractive = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Refractive"].ToString().Trim().Length > 0) { SensorySystemsVisual_Refractive = true; }
                bool SensorySystemsVisual_Physical = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Physical"].ToString().Trim().Length > 0) { SensorySystemsVisual_Physical = true; }
                bool SensorySystemsVestibula_Seeking = false; if (ds.Tables[1].Rows[0]["SensorySystemsVestibula_Seeking"].ToString().Trim().Length > 0) { SensorySystemsVestibula_Seeking = true; }
                bool SensorySystemsVestibula_Avoiding = false; if (ds.Tables[1].Rows[0]["SensorySystemsVestibula_Avoiding"].ToString().Trim().Length > 0) { SensorySystemsVestibula_Avoiding = true; }
                bool SensorySystemsVestibula_Insecurities = false; if (ds.Tables[1].Rows[0]["SensorySystemsVestibula_Insecurities"].ToString().Trim().Length > 0) { SensorySystemsVestibula_Insecurities = true; }
                bool SensorySystemsOromotor_Defensive = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Defensive"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Defensive = true; }
                bool SensorySystemsOromotor_Drooling = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Drooling"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Drooling = true; }
                bool SensorySystemsOromotor_Mouth = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouth"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Mouth = true; }
                bool SensorySystemsOromotor_Mouthing = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouthing"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Mouthing = true; }
                bool SensorySystemsOromotor_Chew = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Chew"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Chew = true; }
                bool SensorySystemsAuditory_Response = false; if (ds.Tables[1].Rows[0]["SensorySystemsAuditory_Response"].ToString().Trim().Length > 0) { SensorySystemsAuditory_Response = true; }
                bool SensorySystemsAuditory_Seeking = false; if (ds.Tables[1].Rows[0]["SensorySystemsAuditory_Seeking"].ToString().Trim().Length > 0) { SensorySystemsAuditory_Seeking = true; }
                bool SensorySystemsAuditory_Avoiding = false; if (ds.Tables[1].Rows[0]["SensorySystemsAuditory_Avoiding"].ToString().Trim().Length > 0) { SensorySystemsAuditory_Avoiding = true; }
                bool SensorySystemsOlfactory_seeking = false; if (ds.Tables[1].Rows[0]["SensorySystemsOlfactory_seeking"].ToString().Trim().Length > 0) { SensorySystemsOlfactory_seeking = true; }
                bool SensorySystemsOlfactory_Avoiding = false; if (ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Avoiding"].ToString().Trim().Length > 0) { SensorySystemsOlfactory_Avoiding = true; }
                bool SensorySystemsSomatosensory_Seeking = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Seeking"].ToString().Trim().Length > 0) { SensorySystemsSomatosensory_Seeking = true; }
                bool SensorySystemsSomatosensory_Avoiding = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Avoiding"].ToString().Trim().Length > 0) { SensorySystemsSomatosensory_Avoiding = true; }
                bool SensorySystemsSomatosensoryProprioceptive_BodyImage = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyImage"].ToString().Trim().Length > 0) { SensorySystemsSomatosensoryProprioceptive_BodyImage = true; }
                bool SensorySystemsSomatosensoryProprioceptive_BodyParts = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyParts"].ToString().Trim().Length > 0) { SensorySystemsSomatosensoryProprioceptive_BodyParts = true; }
                bool SensorySystemsSomatosensoryProprioceptive_Clumsiness = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Clumsiness"].ToString().Trim().Length > 0) { SensorySystemsSomatosensoryProprioceptive_Clumsiness = true; }
                bool SensorySystemsSomatosensoryProprioceptive_Coordination = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Coordination"].ToString().Trim().Length > 0) { SensorySystemsSomatosensoryProprioceptive_Coordination = true; }
                bool SensorySystemsVisual_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsVisual_Comment"].ToString().Trim().Length > 0) { SensorySystemsVisual_Comment = true; }
                bool SensorySystemsSomatosensory_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Comment"].ToString().Trim().Length > 0) { SensorySystemsSomatosensory_Comment = true; }
                bool SensorySystemsVestibula_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsVestibula_Comment"].ToString().Trim().Length > 0) { SensorySystemsVestibula_Comment = true; }
                bool SensorySystemsOromotor_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsOromotor_Comment"].ToString().Trim().Length > 0) { SensorySystemsOromotor_Comment = true; }
                bool SensorySystemsAuditory_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsAuditory_Comment"].ToString().Trim().Length > 0) { SensorySystemsAuditory_Comment = true; }
                bool SensorySystemsOlfactory_Comment = false; if (ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Comment"].ToString().Trim().Length > 0) { SensorySystemsOlfactory_Comment = true; }

                if (SensorySystemsVisual_Focal || SensorySystemsVisual_Ambient || SensorySystemsVisual_Focus || SensorySystemsVisual_Depth || SensorySystemsVisual_Refractive || SensorySystemsVisual_Physical ||
                    SensorySystemsVestibula_Seeking || SensorySystemsVestibula_Avoiding || SensorySystemsVestibula_Insecurities || SensorySystemsOromotor_Defensive ||
                    SensorySystemsOromotor_Drooling || SensorySystemsOromotor_Mouth || SensorySystemsOromotor_Mouthing || SensorySystemsOromotor_Chew ||
                    SensorySystemsAuditory_Response || SensorySystemsAuditory_Seeking || SensorySystemsAuditory_Avoiding || SensorySystemsOlfactory_seeking ||
                    SensorySystemsOlfactory_Avoiding || SensorySystemsSomatosensory_Seeking || SensorySystemsSomatosensory_Avoiding || SensorySystemsVestibula_Comment ||
                    SensorySystemsSomatosensoryProprioceptive_BodyImage || SensorySystemsSomatosensoryProprioceptive_BodyParts || SensorySystemsVisual_Comment ||
                    SensorySystemsSomatosensoryProprioceptive_Clumsiness || SensorySystemsSomatosensoryProprioceptive_Coordination || SensorySystemsSomatosensory_Comment ||
                    SensorySystemsOromotor_Comment || SensorySystemsAuditory_Comment || SensorySystemsOlfactory_Comment)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Sensory System :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SensorySystemsVisual_Focal || SensorySystemsVisual_Ambient || SensorySystemsVisual_Focus || SensorySystemsVisual_Depth ||
                        SensorySystemsVisual_Refractive || SensorySystemsVisual_Physical || SensorySystemsVisual_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Visual System  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (SensorySystemsVisual_Focal)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Focal Vision :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Focal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Ambient)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Ambient vision :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Ambient"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Focus)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Focus & Tracking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Focus"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Depth)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Depth Perception :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Depth"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Refractive)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Refractive Error :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Refractive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Physical)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Physical Impairment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Physical"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVisual_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVisual_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (SensorySystemsVestibula_Seeking || SensorySystemsVestibula_Avoiding || SensorySystemsVestibula_Insecurities || SensorySystemsVestibula_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Vestibular  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (SensorySystemsVestibula_Seeking)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Seeking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVestibula_Seeking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVestibula_Avoiding)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Avoiding :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVestibula_Avoiding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVestibula_Insecurities)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Gravitational Insecurities :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVestibula_Insecurities"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsVestibula_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsVestibula_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (SensorySystemsOromotor_Defensive || SensorySystemsOromotor_Drooling || SensorySystemsOromotor_Mouth || SensorySystemsOromotor_Mouthing ||
                        SensorySystemsOromotor_Chew || SensorySystemsOromotor_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Oromotor / Gustatory  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (SensorySystemsOromotor_Defensive)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Defensive / Seeking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Defensive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOromotor_Drooling)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Drooling :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Drooling"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOromotor_Mouth)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Posture of mouth :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouth"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOromotor_Mouthing)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Mouthing :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Mouthing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOromotor_Chew)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Bite/ Swallow/ Chew :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Chew"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOromotor_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOromotor_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (SensorySystemsAuditory_Response || SensorySystemsAuditory_Seeking || SensorySystemsAuditory_Avoiding || SensorySystemsAuditory_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Auditory  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (SensorySystemsAuditory_Response)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Response and registration :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsAuditory_Response"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsAuditory_Seeking)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Seeking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsAuditory_Seeking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsAuditory_Avoiding)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Avoiding :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsAuditory_Avoiding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsAuditory_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsAuditory_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (SensorySystemsOlfactory_seeking || SensorySystemsOlfactory_Avoiding || SensorySystemsOlfactory_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Olfactory  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        if (SensorySystemsOlfactory_seeking)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Seeking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOlfactory_seeking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOlfactory_Avoiding)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Avoiding :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Avoiding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        if (SensorySystemsOlfactory_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 5f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsOlfactory_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 70f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region
                    if (SensorySystemsSomatosensory_Seeking || SensorySystemsSomatosensory_Avoiding || SensorySystemsSomatosensoryProprioceptive_BodyImage ||
                        SensorySystemsSomatosensoryProprioceptive_BodyParts || SensorySystemsSomatosensoryProprioceptive_Clumsiness ||
                        SensorySystemsSomatosensoryProprioceptive_Coordination || SensorySystemsSomatosensory_Comment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Somatosensory  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                        #region
                        if (SensorySystemsSomatosensory_Seeking || SensorySystemsSomatosensory_Avoiding)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Tactile Response  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 40f;
                            table.AddCell(cell);
                            document.Add(table);

                            if (SensorySystemsSomatosensory_Seeking)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Seeking :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Seeking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (SensorySystemsSomatosensory_Avoiding)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Avoiding :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Avoiding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                        #endregion
                        if (SensorySystemsSomatosensoryProprioceptive_BodyImage || SensorySystemsSomatosensoryProprioceptive_BodyParts || SensorySystemsSomatosensoryProprioceptive_Clumsiness ||
                            SensorySystemsSomatosensoryProprioceptive_Coordination || SensorySystemsSomatosensory_Comment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Proprioceptive  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 40f;
                            table.AddCell(cell);
                            document.Add(table);
                            if (SensorySystemsSomatosensoryProprioceptive_BodyImage)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Body Image :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyImage"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (SensorySystemsSomatosensoryProprioceptive_BodyParts)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Body Parts :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_BodyParts"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (SensorySystemsSomatosensoryProprioceptive_Clumsiness)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Clumsiness :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Clumsiness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (SensorySystemsSomatosensoryProprioceptive_Coordination)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Coordination :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensoryProprioceptive_Coordination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                            if (SensorySystemsSomatosensory_Comment)
                            {
                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 10f;
                                cell = new PdfPCell(PhraseCell(new Phrase("Additional Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                                document.Add(table);

                                table = new PdfPTable(1);
                                table.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.WidthPercentage = 100;
                                table.SpacingBefore = 5f;
                                cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystemsSomatosensory_Comment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 70f;
                                table.AddCell(cell);
                                document.Add(table);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region ****************** Sensory Profile ***********************
                bool SensoryProfile_Profile = false; if (ds.Tables[1].Rows[0]["SensoryProfile_Profile"].ToString().Trim().Length > 0) { SensoryProfile_Profile = true; }
                bool SensoryName1 = false; if (ds.Tables[1].Rows[0]["SensoryName1"].ToString().Trim().Length > 0) { SensoryName1 = true; }
                bool Result1 = false; if (ds.Tables[1].Rows[0]["Result1"].ToString().Trim().Length > 0) { Result1 = true; }
                bool SensoryName2 = false; if (ds.Tables[1].Rows[0]["SensoryName2"].ToString().Trim().Length > 0) { SensoryName2 = true; }
                bool Result2 = false; if (ds.Tables[1].Rows[0]["Result2"].ToString().Trim().Length > 0) { Result2 = true; }
                bool SensoryName3 = false; if (ds.Tables[1].Rows[0]["SensoryName3"].ToString().Trim().Length > 0) { SensoryName3 = true; }
                bool Result3 = false; if (ds.Tables[1].Rows[0]["Result3"].ToString().Trim().Length > 0) { Result3 = true; }
                bool SensoryName4 = false; if (ds.Tables[1].Rows[0]["SensoryName4"].ToString().Trim().Length > 0) { SensoryName4 = true; }
                bool Result4 = false; if (ds.Tables[1].Rows[0]["Result4"].ToString().Trim().Length > 0) { Result4 = true; }
                bool SensoryName5 = false; if (ds.Tables[1].Rows[0]["SensoryName5"].ToString().Trim().Length > 0) { SensoryName5 = true; }
                bool Result5 = false; if (ds.Tables[1].Rows[0]["Result5"].ToString().Trim().Length > 0) { Result5 = true; }
                bool SensoryName6 = false; if (ds.Tables[1].Rows[0]["SensoryName6"].ToString().Trim().Length > 0) { SensoryName6 = true; }
                bool Result6 = false; if (ds.Tables[1].Rows[0]["Result6"].ToString().Trim().Length > 0) { Result6 = true; }
                bool SensoryName7 = false; if (ds.Tables[1].Rows[0]["SensoryName7"].ToString().Trim().Length > 0) { SensoryName7 = true; }
                bool Result7 = false; if (ds.Tables[1].Rows[0]["Result7"].ToString().Trim().Length > 0) { Result7 = true; }
                bool SensoryName8 = false; if (ds.Tables[1].Rows[0]["SensoryName8"].ToString().Trim().Length > 0) { SensoryName8 = true; }
                bool Result8 = false; if (ds.Tables[1].Rows[0]["Result8"].ToString().Trim().Length > 0) { Result8 = true; }
                bool SensoryName9 = false; if (ds.Tables[1].Rows[0]["SensoryName9"].ToString().Trim().Length > 0) { SensoryName9 = true; }
                bool Result9 = false; if (ds.Tables[1].Rows[0]["Result9"].ToString().Trim().Length > 0) { Result9 = true; }
                bool SensoryName10 = false; if (ds.Tables[1].Rows[0]["SensoryName10"].ToString().Trim().Length > 0) { SensoryName10 = true; }
                bool Result10 = false; if (ds.Tables[1].Rows[0]["Result10"].ToString().Trim().Length > 0) { Result10 = true; }
                bool SensoryName11 = false; if (ds.Tables[1].Rows[0]["SensoryName11"].ToString().Trim().Length > 0) { SensoryName11 = true; }
                bool Result11 = false; if (ds.Tables[1].Rows[0]["Result11"].ToString().Trim().Length > 0) { Result11 = true; }
                bool SensoryName12 = false; if (ds.Tables[1].Rows[0]["SensoryName12"].ToString().Trim().Length > 0) { SensoryName12 = true; }
                bool Result12 = false; if (ds.Tables[1].Rows[0]["Result12"].ToString().Trim().Length > 0) { Result12 = true; }
                bool SensoryName13 = false; if (ds.Tables[1].Rows[0]["SensoryName13"].ToString().Trim().Length > 0) { SensoryName13 = true; }
                bool Result13 = false; if (ds.Tables[1].Rows[0]["Result13"].ToString().Trim().Length > 0) { Result13 = true; }
                bool SensoryName14 = false; if (ds.Tables[1].Rows[0]["SensoryName14"].ToString().Trim().Length > 0) { SensoryName14 = true; }
                bool Result14 = false; if (ds.Tables[1].Rows[0]["Result14"].ToString().Trim().Length > 0) { Result14 = true; }
                bool SensoryName15 = false; if (ds.Tables[1].Rows[0]["SensoryName15"].ToString().Trim().Length > 0) { SensoryName15 = true; }
                bool Result15 = false; if (ds.Tables[1].Rows[0]["Result15"].ToString().Trim().Length > 0) { Result15 = true; }
                bool SensoryName16 = false; if (ds.Tables[1].Rows[0]["SensoryName16"].ToString().Trim().Length > 0) { SensoryName16 = true; }
                bool Result16 = false; if (ds.Tables[1].Rows[0]["Result16"].ToString().Trim().Length > 0) { Result16 = true; }
                bool SensoryName17 = false; if (ds.Tables[1].Rows[0]["SensoryName17"].ToString().Trim().Length > 0) { SensoryName17 = true; }
                bool Result17 = false; if (ds.Tables[1].Rows[0]["Result17"].ToString().Trim().Length > 0) { Result17 = true; }
                bool SensoryName18 = false; if (ds.Tables[1].Rows[0]["SensoryName18"].ToString().Trim().Length > 0) { SensoryName18 = true; }
                bool Result18 = false; if (ds.Tables[1].Rows[0]["Result18"].ToString().Trim().Length > 0) { Result18 = true; }
                bool SensoryName19 = false; if (ds.Tables[1].Rows[0]["SensoryName19"].ToString().Trim().Length > 0) { SensoryName19 = true; }
                bool Result19 = false; if (ds.Tables[1].Rows[0]["Result19"].ToString().Trim().Length > 0) { Result19 = true; }
                bool SensoryName20 = false; if (ds.Tables[1].Rows[0]["SensoryName20"].ToString().Trim().Length > 0) { SensoryName20 = true; }
                bool Result20 = false; if (ds.Tables[1].Rows[0]["Result20"].ToString().Trim().Length > 0) { Result20 = true; }
                bool SensoryName21 = false; if (ds.Tables[1].Rows[0]["SensoryName21"].ToString().Trim().Length > 0) { SensoryName21 = true; }
                bool Result21 = false; if (ds.Tables[1].Rows[0]["Result21"].ToString().Trim().Length > 0) { Result21 = true; }
                bool SensoryName22 = false; if (ds.Tables[1].Rows[0]["SensoryName22"].ToString().Trim().Length > 0) { SensoryName22 = true; }
                bool Result22 = false; if (ds.Tables[1].Rows[0]["Result22"].ToString().Trim().Length > 0) { Result22 = true; }
                bool SensoryName23 = false; if (ds.Tables[1].Rows[0]["SensoryName23"].ToString().Trim().Length > 0) { SensoryName23 = true; }
                bool Result23 = false; if (ds.Tables[1].Rows[0]["Result23"].ToString().Trim().Length > 0) { Result23 = true; }
                bool SensoryName24 = false; if (ds.Tables[1].Rows[0]["SensoryName24"].ToString().Trim().Length > 0) { SensoryName24 = true; }
                bool Result24 = false; if (ds.Tables[1].Rows[0]["Result24"].ToString().Trim().Length > 0) { Result24 = true; }
                bool SensoryName25 = false; if (ds.Tables[1].Rows[0]["SensoryName25"].ToString().Trim().Length > 0) { SensoryName25 = true; }
                bool Result25 = false; if (ds.Tables[1].Rows[0]["Result25"].ToString().Trim().Length > 0) { Result25 = true; }
                bool SensoryName26 = false; if (ds.Tables[1].Rows[0]["SensoryName26"].ToString().Trim().Length > 0) { SensoryName26 = true; }
                bool Result26 = false; if (ds.Tables[1].Rows[0]["Result26"].ToString().Trim().Length > 0) { Result26 = true; }
                bool SensoryName27 = false; if (ds.Tables[1].Rows[0]["SensoryName27"].ToString().Trim().Length > 0) { SensoryName27 = true; }
                bool Result27 = false; if (ds.Tables[1].Rows[0]["Result27"].ToString().Trim().Length > 0) { Result27 = true; }
                bool SensoryName28 = false; if (ds.Tables[1].Rows[0]["SensoryName28"].ToString().Trim().Length > 0) { SensoryName28 = true; }
                bool Result28 = false; if (ds.Tables[1].Rows[0]["Result28"].ToString().Trim().Length > 0) { Result28 = true; }
                bool SensoryName29 = false; if (ds.Tables[1].Rows[0]["SensoryName29"].ToString().Trim().Length > 0) { SensoryName29 = true; }
                bool Result29 = false; if (ds.Tables[1].Rows[0]["Result29"].ToString().Trim().Length > 0) { Result29 = true; }
                bool SensoryName30 = false; if (ds.Tables[1].Rows[0]["SensoryName30"].ToString().Trim().Length > 0) { SensoryName30 = true; }
                bool Result30 = false; if (ds.Tables[1].Rows[0]["Result30"].ToString().Trim().Length > 0) { Result30 = true; }
                bool SensoryName31 = false; if (ds.Tables[1].Rows[0]["SensoryName31"].ToString().Trim().Length > 0) { SensoryName31 = true; }
                bool Result31 = false; if (ds.Tables[1].Rows[0]["Result31"].ToString().Trim().Length > 0) { Result31 = true; }
                bool SensoryName32 = false; if (ds.Tables[1].Rows[0]["SensoryName32"].ToString().Trim().Length > 0) { SensoryName32 = true; }
                bool Result32 = false; if (ds.Tables[1].Rows[0]["Result32"].ToString().Trim().Length > 0) { Result32 = true; }
                bool SensoryName33 = false; if (ds.Tables[1].Rows[0]["SensoryName33"].ToString().Trim().Length > 0) { SensoryName33 = true; }
                bool Result33 = false; if (ds.Tables[1].Rows[0]["Result33"].ToString().Trim().Length > 0) { Result33 = true; }
                bool SensoryName34 = false; if (ds.Tables[1].Rows[0]["SensoryName34"].ToString().Trim().Length > 0) { SensoryName34 = true; }
                bool Result34 = false; if (ds.Tables[1].Rows[0]["Result34"].ToString().Trim().Length > 0) { Result34 = true; }
                bool SensoryName35 = false; if (ds.Tables[1].Rows[0]["SensoryName35"].ToString().Trim().Length > 0) { SensoryName35 = true; }
                bool Result35 = false; if (ds.Tables[1].Rows[0]["Result35"].ToString().Trim().Length > 0) { Result35 = true; }
                bool SensoryName36 = false; if (ds.Tables[1].Rows[0]["SensoryName36"].ToString().Trim().Length > 0) { SensoryName36 = true; }
                bool Result36 = false; if (ds.Tables[1].Rows[0]["Result36"].ToString().Trim().Length > 0) { Result36 = true; }
                bool SensoryName37 = false; if (ds.Tables[1].Rows[0]["SensoryName37"].ToString().Trim().Length > 0) { SensoryName37 = true; }
                bool Result37 = false; if (ds.Tables[1].Rows[0]["Result37"].ToString().Trim().Length > 0) { Result37 = true; }
                bool SensoryName38 = false; if (ds.Tables[1].Rows[0]["SensoryName38"].ToString().Trim().Length > 0) { SensoryName38 = true; }
                bool Result38 = false; if (ds.Tables[1].Rows[0]["Result38"].ToString().Trim().Length > 0) { Result38 = true; }
                bool SensoryName39 = false; if (ds.Tables[1].Rows[0]["SensoryName39"].ToString().Trim().Length > 0) { SensoryName39 = true; }
                bool Result39 = false; if (ds.Tables[1].Rows[0]["Result39"].ToString().Trim().Length > 0) { Result39 = true; }
                bool SensoryName40 = false; if (ds.Tables[1].Rows[0]["SensoryName40"].ToString().Trim().Length > 0) { SensoryName40 = true; }
                bool Result40 = false; if (ds.Tables[1].Rows[0]["Result40"].ToString().Trim().Length > 0) { Result40 = true; }

                if (SensoryName1 || Result1 || SensoryName2 || Result2 || SensoryName3 || Result3 || SensoryName3 || Result3 || SensoryName4 || Result4 || SensoryName4 || Result4 || SensoryName5 || Result5 || SensoryName6 || Result6 || SensoryName7 || Result7 || SensoryName8 || Result8 || SensoryName9 || Result9 || SensoryName10 || Result10 || SensoryName11 || Result11 || SensoryName12 || Result12 || SensoryName13 || Result13 || SensoryName14 || Result14 || SensoryName15 || Result15 || SensoryName16 || Result16 || SensoryName17 || Result17 || SensoryName18 || Result18 || SensoryName19 || Result19 || SensoryName20 || Result20 || SensoryName21 || Result21 || SensoryName22 || Result22 || SensoryName23 || Result24 || SensoryName25 || Result25 || SensoryName26 || Result26 || SensoryName27 || Result27 || SensoryName28 || Result28 || SensoryName29 || Result29 || SensoryName30 || Result30 || SensoryName31 || Result31 || SensoryName32 || Result32 || SensoryName33 || Result33 || SensoryName34 || Result34 || SensoryName35 || Result35 || SensoryName36 || Result36 || SensoryName37 || Result37 || SensoryName38 || Result38 || SensoryName39 || Result39 || SensoryName40 || Result40 || SensoryProfile_Profile)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Sensory Profile :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SensoryProfile_Profile)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Sensory Profile :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryProfile_Profile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName1 && Result1)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;

                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName2 && Result2)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;

                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName3 && Result3)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;

                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName4 && Result4)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;

                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName5 && Result5)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName6 && Result6)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName7 && Result7)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName8 && Result8)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName8"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result8"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName9 && Result9)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName10 && Result10)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName11 && Result11)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName12 && Result12)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName12"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result12"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName13 && Result13)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName14 && Result14)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName14"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result14"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName15 && Result15)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName16 && Result16)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName16"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result16"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName17 && Result17)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName18 && Result18)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName18"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result18"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName19 && Result19)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName20 && Result20)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName20"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result20"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName21 && Result21)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName22 && Result22)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName22"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result22"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName23 && Result23)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName24 && Result24)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName24"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result24"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName25 && Result25)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName26 && Result26)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName26"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result26"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName27 && Result27)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName27"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result27"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName28 && Result28)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName29 && Result29)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName29"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result29"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName30 && Result30)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName30"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result30"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName31 && Result31)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName32 && Result32)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName32"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result32"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName33 && Result33)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName33"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result33"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName34 && Result34)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName35 && Result35)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName35"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result35"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName36 && Result36)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName36"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result36"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName37 && Result37)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName37"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result37"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                    #region
                    if (SensoryName38 && Result38)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName38"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result38"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName39 && Result39)
                    {
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName39"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result39"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (SensoryName40 && Result40)
                    {

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 10f;
                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensoryName40"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Result40"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ******************** SIPT Information ****************
                bool SIPTInfo_History = false; if (ds.Tables[1].Rows[0]["SIPTInfo_History"].ToString().Trim().Length > 0) { SIPTInfo_History = true; }

                bool SIPTInfo_HandFunction1_GraspRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_GraspRight = true; }
                bool SIPTInfo_HandFunction1_GraspLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_GraspLeft = true; }
                bool SIPTInfo_HandFunction1_SphericalRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_SphericalRight = true; }
                bool SIPTInfo_HandFunction1_SphericalLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_SphericalLeft = true; }
                bool SIPTInfo_HandFunction1_HookRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_HookRight = true; }
                bool SIPTInfo_HandFunction1_HookLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_HookLeft = true; }
                bool SIPTInfo_HandFunction1_JawChuckRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_JawChuckRight = true; }
                bool SIPTInfo_HandFunction1_JawChuckLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_JawChuckLeft = true; }
                bool SIPTInfo_HandFunction1_GripRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_GripRight = true; }
                bool SIPTInfo_HandFunction1_GripLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_GripLeft = true; }
                bool SIPTInfo_HandFunction1_ReleaseRight = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseRight"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_ReleaseRight = true; }
                bool SIPTInfo_HandFunction1_ReleaseLeft = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseLeft"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction1_ReleaseLeft = true; }
                bool SIPTInfo_HandFunction1 = false; if (SIPTInfo_HandFunction1_GraspRight || SIPTInfo_HandFunction1_GraspLeft || SIPTInfo_HandFunction1_SphericalRight || SIPTInfo_HandFunction1_SphericalLeft || SIPTInfo_HandFunction1_HookRight || SIPTInfo_HandFunction1_HookLeft || SIPTInfo_HandFunction1_JawChuckRight || SIPTInfo_HandFunction1_JawChuckLeft || SIPTInfo_HandFunction1_GripRight || SIPTInfo_HandFunction1_GripLeft || SIPTInfo_HandFunction1_ReleaseRight || SIPTInfo_HandFunction1_ReleaseLeft) { SIPTInfo_HandFunction1 = true; }


                bool SIPTInfo_HandFunction2_OppositionLfR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionLfR = true; }
                bool SIPTInfo_HandFunction2_OppositionLfL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionLfL = true; }
                bool SIPTInfo_HandFunction2_OppositionMFR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionMFR = true; }
                bool SIPTInfo_HandFunction2_OppositionMFL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionMFL = true; }
                bool SIPTInfo_HandFunction2_OppositionRFR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionRFR = true; }
                bool SIPTInfo_HandFunction2_OppositionRFL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_OppositionRFL = true; }
                bool SIPTInfo_HandFunction2_PinchLfR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchLfR = true; }
                bool SIPTInfo_HandFunction2_PinchLfL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchLfL = true; }
                bool SIPTInfo_HandFunction2_PinchMFR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchMFR = true; }
                bool SIPTInfo_HandFunction2_PinchMFL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchMFL = true; }
                bool SIPTInfo_HandFunction2_PinchRFR = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFR"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchRFR = true; }
                bool SIPTInfo_HandFunction2_PinchRFL = false; if (ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFL"].ToString().Trim().Length > 0) { SIPTInfo_HandFunction2_PinchRFL = true; }
                bool SIPTInfo_HandFunction2 = false; if (SIPTInfo_HandFunction2_OppositionLfR || SIPTInfo_HandFunction2_OppositionLfL || SIPTInfo_HandFunction2_OppositionMFR || SIPTInfo_HandFunction2_OppositionMFL || SIPTInfo_HandFunction2_OppositionRFR || SIPTInfo_HandFunction2_OppositionRFL || SIPTInfo_HandFunction2_PinchLfR || SIPTInfo_HandFunction2_PinchLfL || SIPTInfo_HandFunction2_PinchMFR || SIPTInfo_HandFunction2_PinchMFL || SIPTInfo_HandFunction2_PinchRFR || SIPTInfo_HandFunction2_PinchRFL) { SIPTInfo_HandFunction2 = true; }

                bool SIPTInfo_SIPT3_Spontaneous = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Spontaneous"].ToString().Trim().Length > 0) { SIPTInfo_SIPT3_Spontaneous = true; }
                bool SIPTInfo_SIPT3_Command = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Command"].ToString().Trim().Length > 0) { SIPTInfo_SIPT3_Command = true; }
                bool SIPTInfo_SIPT3 = false; if (SIPTInfo_SIPT3_Spontaneous || SIPTInfo_SIPT3_Command) { SIPTInfo_SIPT3 = true; }

                bool SIPTInfo_SIPT4_Kinesthesia = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Kinesthesia"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Kinesthesia = true; }
                bool SIPTInfo_SIPT4_Finger = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Finger"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Finger = true; }
                bool SIPTInfo_SIPT4_Localisation = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Localisation"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Localisation = true; }
                bool SIPTInfo_SIPT4_DoubleTactile = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_DoubleTactile"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_DoubleTactile = true; }
                bool SIPTInfo_SIPT4_Tactile = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Tactile"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Tactile = true; }
                bool SIPTInfo_SIPT4_Graphesthesia = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Graphesthesia"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Graphesthesia = true; }
                bool SIPTInfo_SIPT4_PostRotary = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_PostRotary"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_PostRotary = true; }
                bool SIPTInfo_SIPT4_Standing = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Standing"].ToString().Trim().Length > 0) { SIPTInfo_SIPT4_Standing = true; }
                bool SIPTInfo_SIPT4 = false; if (SIPTInfo_SIPT4_Kinesthesia || SIPTInfo_SIPT4_Finger || SIPTInfo_SIPT4_Localisation || SIPTInfo_SIPT4_DoubleTactile || SIPTInfo_SIPT4_Tactile || SIPTInfo_SIPT4_Graphesthesia || SIPTInfo_SIPT4_PostRotary || SIPTInfo_SIPT4_Standing) { SIPTInfo_SIPT4 = true; }

                bool SIPTInfo_SIPT5_Color = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Color"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Color = true; }
                bool SIPTInfo_SIPT5_Form = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Form"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Form = true; }
                bool SIPTInfo_SIPT5_Size = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Size"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Size = true; }
                bool SIPTInfo_SIPT5_Depth = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Depth"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Depth = true; }
                bool SIPTInfo_SIPT5_Figure = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Figure"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Figure = true; }
                bool SIPTInfo_SIPT5_Motor = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Motor"].ToString().Trim().Length > 0) { SIPTInfo_SIPT5_Motor = true; }
                bool SIPTInfo_SIPT5 = false; if (SIPTInfo_SIPT5_Color || SIPTInfo_SIPT5_Form || SIPTInfo_SIPT5_Size || SIPTInfo_SIPT5_Depth || SIPTInfo_SIPT5_Figure || SIPTInfo_SIPT5_Motor) { SIPTInfo_SIPT5 = true; }

                bool SIPTInfo_SIPT6_Design = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Design"].ToString().Trim().Length > 0) { SIPTInfo_SIPT6_Design = true; }
                bool SIPTInfo_SIPT6_Constructional = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Constructional"].ToString().Trim().Length > 0) { SIPTInfo_SIPT6_Constructional = true; }
                bool SIPTInfo_SIPT6 = false; if (SIPTInfo_SIPT6_Design || SIPTInfo_SIPT6_Constructional) { SIPTInfo_SIPT6 = true; }

                bool SIPTInfo_SIPT7_Scanning = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Scanning"].ToString().Trim().Length > 0) { SIPTInfo_SIPT7_Scanning = true; }
                bool SIPTInfo_SIPT7_Memory = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Memory"].ToString().Trim().Length > 0) { SIPTInfo_SIPT7_Memory = true; }
                bool SIPTInfo_SIPT7 = false; if (SIPTInfo_SIPT7_Scanning || SIPTInfo_SIPT7_Memory) { SIPTInfo_SIPT7 = true; }

                bool SIPTInfo_SIPT8_Postural = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Postural"].ToString().Trim().Length > 0) { SIPTInfo_SIPT8_Postural = true; }
                bool SIPTInfo_SIPT8_Oral = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Oral"].ToString().Trim().Length > 0) { SIPTInfo_SIPT8_Oral = true; }
                bool SIPTInfo_SIPT8_Sequencing = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Sequencing"].ToString().Trim().Length > 0) { SIPTInfo_SIPT8_Sequencing = true; }
                bool SIPTInfo_SIPT8_Commands = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Commands"].ToString().Trim().Length > 0) { SIPTInfo_SIPT8_Commands = true; }
                bool SIPTInfo_SIPT8 = false; if (SIPTInfo_SIPT8_Postural || SIPTInfo_SIPT8_Oral || SIPTInfo_SIPT8_Sequencing || SIPTInfo_SIPT8_Commands) { SIPTInfo_SIPT8 = true; }

                bool SIPTInfo_SIPT9_Bilateral = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Bilateral"].ToString().Trim().Length > 0) { SIPTInfo_SIPT9_Bilateral = true; }
                bool SIPTInfo_SIPT9_Contralat = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Contralat"].ToString().Trim().Length > 0) { SIPTInfo_SIPT9_Contralat = true; }
                bool SIPTInfo_SIPT9_PreferredHand = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_PreferredHand"].ToString().Trim().Length > 0) { SIPTInfo_SIPT9_PreferredHand = true; }
                bool SIPTInfo_SIPT9_CrossingMidline = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_CrossingMidline"].ToString().Trim().Length > 0) { SIPTInfo_SIPT9_CrossingMidline = true; }
                bool SIPTInfo_SIPT9 = false; if (SIPTInfo_SIPT9_Bilateral || SIPTInfo_SIPT9_Contralat || SIPTInfo_SIPT9_PreferredHand || SIPTInfo_SIPT9_CrossingMidline) { SIPTInfo_SIPT9 = true; }

                bool SIPTInfo_SIPT10_Draw = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Draw"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_Draw = true; }
                bool SIPTInfo_SIPT10_ClockFace = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_ClockFace"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_ClockFace = true; }
                bool SIPTInfo_SIPT10_Filtering = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Filtering"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_Filtering = true; }
                bool SIPTInfo_SIPT10_MotorPlanning = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_MotorPlanning"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_MotorPlanning = true; }
                bool SIPTInfo_SIPT10_BodyImage = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodyImage"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_BodyImage = true; }
                bool SIPTInfo_SIPT10_BodySchema = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodySchema"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_BodySchema = true; }
                bool SIPTInfo_SIPT10_Laterality = false; if (ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Laterality"].ToString().Trim().Length > 0) { SIPTInfo_SIPT10_Laterality = true; }
                bool SIPTInfo_SIPT10 = false; if (SIPTInfo_SIPT10_Draw || SIPTInfo_SIPT10_ClockFace || SIPTInfo_SIPT10_Filtering || SIPTInfo_SIPT10_MotorPlanning || SIPTInfo_SIPT10_BodyImage || SIPTInfo_SIPT10_BodySchema || SIPTInfo_SIPT10_Laterality) { SIPTInfo_SIPT10 = true; }

                bool SIPTInfo_ActivityGiven_Remark = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Remark"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Remark = true; }
                bool SIPTInfo_ActivityGiven_InterestActivity = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestActivity"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_InterestActivity = true; }
                bool SIPTInfo_ActivityGiven_InterestCompletion = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestCompletion"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_InterestCompletion = true; }
                bool SIPTInfo_ActivityGiven_Learning = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Learning"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Learning = true; }
                bool SIPTInfo_ActivityGiven_Complexity = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Complexity"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Complexity = true; }
                bool SIPTInfo_ActivityGiven_ProblemSolving = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_ProblemSolving"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_ProblemSolving = true; }
                bool SIPTInfo_ActivityGiven_Concentration = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Concentration"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Concentration = true; }
                bool SIPTInfo_ActivityGiven_Retension = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Retension"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Retension = true; }
                bool SIPTInfo_ActivityGiven_SpeedPerfom = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SpeedPerfom"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_SpeedPerfom = true; }
                bool SIPTInfo_ActivityGiven_Neatness = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Neatness"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Neatness = true; }
                bool SIPTInfo_ActivityGiven_Frustation = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Frustation"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Frustation = true; }
                bool SIPTInfo_ActivityGiven_Work = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Work"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Work = true; }
                bool SIPTInfo_ActivityGiven_Reaction = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Reaction"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_Reaction = true; }
                bool SIPTInfo_ActivityGiven_SociabilityTherapist = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityTherapist"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_SociabilityTherapist = true; }
                bool SIPTInfo_ActivityGiven_SociabilityStudents = false; if (ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityStudents"].ToString().Trim().Length > 0) { SIPTInfo_ActivityGiven_SociabilityStudents = true; }
                bool SIPTInfo_ActivityGiven = false; if (SIPTInfo_ActivityGiven_Remark || SIPTInfo_ActivityGiven_InterestActivity || SIPTInfo_ActivityGiven_InterestCompletion || SIPTInfo_ActivityGiven_Learning || SIPTInfo_ActivityGiven_Complexity || SIPTInfo_ActivityGiven_ProblemSolving || SIPTInfo_ActivityGiven_Concentration || SIPTInfo_ActivityGiven_Retension || SIPTInfo_ActivityGiven_SpeedPerfom || SIPTInfo_ActivityGiven_Neatness || SIPTInfo_ActivityGiven_Frustation || SIPTInfo_ActivityGiven_Work || SIPTInfo_ActivityGiven_Reaction || SIPTInfo_ActivityGiven_SociabilityTherapist || SIPTInfo_ActivityGiven_SociabilityStudents) { SIPTInfo_ActivityGiven = true; }

                if (SIPTInfo_History || SIPTInfo_HandFunction1 || SIPTInfo_HandFunction2 || SIPTInfo_SIPT3 || SIPTInfo_SIPT4 || SIPTInfo_SIPT5 || SIPTInfo_SIPT6 || SIPTInfo_SIPT7 || SIPTInfo_SIPT8 || SIPTInfo_SIPT9 || SIPTInfo_SIPT10 || SIPTInfo_ActivityGiven)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("SIPT Information :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (SIPTInfo_History)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("History :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_History"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_HandFunction1)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Hand Function - I :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Hand Functions", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_HandFunction1_GraspRight || SIPTInfo_HandFunction1_GraspLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Grasp : Cylindrical", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_HandFunction1_SphericalRight || SIPTInfo_HandFunction1_SphericalLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Spherical", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_HandFunction1_HookRight || SIPTInfo_HandFunction1_HookLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hook", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_HandFunction1_JawChuckRight || SIPTInfo_HandFunction1_JawChuckLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Jaw Chuck", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_HandFunction1_GripRight || SIPTInfo_HandFunction1_GripLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Grip", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_HandFunction1_ReleaseRight || SIPTInfo_HandFunction1_ReleaseLeft)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Release", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_HandFunction2)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Hand Function - II :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(7);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Hand Functions", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Lf -> R", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Lf -> L", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("MF -> R", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("MF -> L", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("RF -> R", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("RF -> L", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Opposition", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Pinch", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT3)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - III :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Reaching > Spontaneous", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Spontaneous"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Reaching > On Command", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Command"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        /**/
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT4)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - IV :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT4_Kinesthesia)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Kinesthesia", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Kinesthesia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_Finger)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Finger Identification Test", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Finger"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_Localisation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Localisation Of Tactile Stimuli", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Localisation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_DoubleTactile)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Double Tactile Localisation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_DoubleTactile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_Tactile)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Tactile Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Tactile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_Graphesthesia)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Graphesthesia", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Graphesthesia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_PostRotary)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Post Rotary Nystagmus", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_PostRotary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT4_Standing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Standing And Walking Balance", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Standing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT5)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - V :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT5_Color)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Color Recognition", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Color"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT5_Form)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Form Constancy", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Form"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT5_Size)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Size Differentiation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Size"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT5_Depth)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Depth Perception", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Depth"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT5_Figure)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Figure Ground Perception", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Figure"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT5_Motor)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Motor Accuracy", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Motor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT6)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - VI :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT6_Design)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Design Copying", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Design"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT6_Constructional)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Constructional Praxis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Constructional"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT7)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - VII :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT7_Scanning)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual Scanning", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Scanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT7_Memory)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual Memory", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Memory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT8)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - VIII :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT8_Postural)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Postural Praxis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Postural"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT8_Oral)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Oral Praxis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Oral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT8_Sequencing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sequencing Praxis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Sequencing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT8_Commands)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Praxis On Verbal Commands", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Commands"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT9)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - IX :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT9_Bilateral)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Bilateral Motor Co-ordination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Bilateral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT9_Contralat)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Space Visualisation Contralat Use", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Contralat"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT9_PreferredHand)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Space Visualisation Preferred Hand", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_PreferredHand"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT9_CrossingMidline)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Crossing Midline", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_CrossingMidline"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_SIPT10)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SIPT - X :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_SIPT10_Draw)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Draw A Person Test", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Draw"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_ClockFace)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Clock Face", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_ClockFace"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_Filtering)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Filtering Information", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Filtering"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_MotorPlanning)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Motor Planning", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_MotorPlanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_BodyImage)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Body Image", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodyImage"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_BodySchema)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Body Schema", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodySchema"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_SIPT10_Laterality)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Laterality", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Laterality"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SIPTInfo_ActivityGiven)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Activity Given :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                        if (SIPTInfo_ActivityGiven_Remark)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Remark"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Parameter", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Value", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SIPTInfo_ActivityGiven_InterestActivity)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Interest In Activity", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestActivity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_InterestCompletion)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Interest In Completion", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestCompletion"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Learning)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Initial Learning", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Learning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Complexity)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Complexity And Organisation Of Task", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Complexity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_ProblemSolving)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Problem Solving", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_ProblemSolving"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Concentration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Concentration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Concentration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Retension)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Retension And Recall", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Retension"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_SpeedPerfom)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Speed Of Perfomance", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SpeedPerfom"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Neatness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Activity Neatness", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Neatness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Frustation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Frustation Tolerance", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Frustation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Work)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Work Tolerance", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Work"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_Reaction)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Reaction To Authority", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Reaction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_SociabilityTherapist)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sociability With Therapist", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityTherapist"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SIPTInfo_ActivityGiven_SociabilityStudents)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sociability With Others Students", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityStudents"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Others *********************
                bool Other_DCD = false; if (ds.Tables[1].Rows[0]["Other_DCD"].ToString().Trim().Length > 0) { Other_DCD = true; }
                bool Other_DSM = false; if (ds.Tables[1].Rows[0]["Other_DSM"].ToString().Trim().Length > 0) { Other_DSM = true; }
                if (Other_DCD || Other_DSM)
                {
                    cell = PhraseCell(new Phrase("Others :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Other_DCD)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("DCD questionnaire :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_DCD"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Other_DCD)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("DSM 5 :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_DCD"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Cognition ********************
                bool Cognition_Intelligence = false; if (ds.Tables[1].Rows[0]["Cognition_Intelligence"].ToString().Trim().Length > 0) { Cognition_Intelligence = true; }
                bool Cognition_Attention = false; if (ds.Tables[1].Rows[0]["Cognition_Attention"].ToString().Trim().Length > 0) { Cognition_Attention = true; }
                bool Cognition_Memory = false; if (ds.Tables[1].Rows[0]["Cognition_Memory"].ToString().Trim().Length > 0) { Cognition_Memory = true; }
                bool Cognition_Adaptibility = false; if (ds.Tables[1].Rows[0]["Cognition_Adaptibility"].ToString().Trim().Length > 0) { Cognition_Adaptibility = true; }
                bool Cognition_MotorPlanning = false; if (ds.Tables[1].Rows[0]["Cognition_MotorPlanning"].ToString().Trim().Length > 0) { Cognition_MotorPlanning = true; }
                bool Cognition_ExecutiveFunction = false; if (ds.Tables[1].Rows[0]["Cognition_ExecutiveFunction"].ToString().Trim().Length > 0) { Cognition_ExecutiveFunction = true; }
                bool Cognition_CognitiveFunctions = false; if (ds.Tables[1].Rows[0]["Cognition_CognitiveFunctions"].ToString().Trim().Length > 0) { Cognition_CognitiveFunctions = true; }
                if (Cognition_Intelligence || Cognition_Attention || Cognition_Memory || Cognition_Adaptibility || Cognition_MotorPlanning || Cognition_ExecutiveFunction || Cognition_CognitiveFunctions)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Cognition :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Cognition_Intelligence)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Intelligence :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_Intelligence"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_Memory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Memory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_Memory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_Adaptibility)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Adaptibility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_Adaptibility"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_MotorPlanning)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Motor Planning :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_MotorPlanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_ExecutiveFunction)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Executive Function :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_ExecutiveFunction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cognition_CognitiveFunctions)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Age Appropriate Cognitive Functions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition_CognitiveFunctions"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ******************* Integumentary ********************
                bool Integumentary_SkinIntegrity = false; if (ds.Tables[1].Rows[0]["Integumentary_SkinIntegrity"].ToString().Trim().Length > 0) { Integumentary_SkinIntegrity = true; }
                bool Integumentary_SkinColor = false; if (ds.Tables[1].Rows[0]["Integumentary_SkinColor"].ToString().Trim().Length > 0) { Integumentary_SkinColor = true; }
                bool Integumentary_SkinExtensibility = false; if (ds.Tables[1].Rows[0]["Integumentary_SkinExtensibility"].ToString().Trim().Length > 0) { Integumentary_SkinExtensibility = true; }
                if (Integumentary_SkinIntegrity || Integumentary_SkinColor || Integumentary_SkinExtensibility)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Integumentary :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    #region
                    if (Integumentary_SkinIntegrity)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Skin Integrity :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Integumentary_SkinIntegrity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Integumentary_SkinColor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Skin Color :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Integumentary_SkinColor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Integumentary_SkinExtensibility)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Skin Extensibility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Integumentary_SkinExtensibility"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ****************** Respiratory ****************
                bool Respiratory_RateResting = false; if (ds.Tables[1].Rows[0]["Respiratory_RateResting"].ToString().Trim().Length > 0) { Respiratory_RateResting = true; }
                bool Respiratory_PostExercise = false; if (ds.Tables[1].Rows[0]["Respiratory_PostExercise"].ToString().Trim().Length > 0) { Respiratory_PostExercise = true; }
                bool Respiratory_Patterns = false; if (ds.Tables[1].Rows[0]["Respiratory_Patterns"].ToString().Trim().Length > 0) { Respiratory_Patterns = true; }
                bool Respiratory_BreathControl = false; if (ds.Tables[1].Rows[0]["Respiratory_BreathControl"].ToString().Trim().Length > 0) { Respiratory_BreathControl = true; }
                if (Respiratory_RateResting || Respiratory_PostExercise || Respiratory_Patterns || Respiratory_BreathControl)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Respiratory :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Respiratory_RateResting)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Rate-resting :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Respiratory_RateResting"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Respiratory_PostExercise)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Post Exercise :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Respiratory_PostExercise"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Respiratory_Patterns)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Patterns :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Respiratory_Patterns"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Respiratory_BreathControl)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Breath Control Capacity :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Respiratory_BreathControl"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region **************** Cardiovascular ***********************
                bool Cardiovascular_HeartRate = false; if (ds.Tables[1].Rows[0]["Cardiovascular_HeartRate"].ToString().Trim().Length > 0) { Cardiovascular_HeartRate = true; }
                bool Cardiovascular_PostExercise = false; if (ds.Tables[1].Rows[0]["Cardiovascular_PostExercise"].ToString().Trim().Length > 0) { Cardiovascular_PostExercise = true; }
                bool Cardiovascular_BP = false; if (ds.Tables[1].Rows[0]["Cardiovascular_BP"].ToString().Trim().Length > 0) { Cardiovascular_BP = true; }
                bool Cardiovascular_Edema = false; if (ds.Tables[1].Rows[0]["Cardiovascular_Edema"].ToString().Trim().Length > 0) { Cardiovascular_Edema = true; }
                bool Cardiovascular_Circulation = false; if (ds.Tables[1].Rows[0]["Cardiovascular_Circulation"].ToString().Trim().Length > 0) { Cardiovascular_Circulation = true; }
                bool Cardiovascular_EEi = false; if (ds.Tables[1].Rows[0]["Cardiovascular_EEi"].ToString().Trim().Length > 0) { Cardiovascular_EEi = true; }
                if (Cardiovascular_HeartRate || Cardiovascular_PostExercise || Cardiovascular_BP || Cardiovascular_Edema || Cardiovascular_Circulation || Cardiovascular_EEi)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Cardiovascular :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    #region
                    if (Cardiovascular_HeartRate)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Heart Rate-Resting :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_HeartRate"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cardiovascular_PostExercise)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Post Exercise :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_PostExercise"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cardiovascular_BP)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("BP :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_BP"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cardiovascular_Edema)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Edema :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_Edema"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cardiovascular_Circulation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Peripheral Circulation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_Circulation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Cardiovascular_EEi)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("EEi :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular_EEi"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ******************* Gastrointestinal *****************
                bool Gastrointestinal_Bowel = false; if (ds.Tables[1].Rows[0]["Gastrointestinal_Bowel"].ToString().Trim().Length > 0) { Gastrointestinal_Bowel = true; }
                bool Gastrointestinal_Intake = false; if (ds.Tables[1].Rows[0]["Gastrointestinal_Intake"].ToString().Trim().Length > 0) { Gastrointestinal_Intake = true; }
                if (Gastrointestinal_Bowel || Gastrointestinal_Intake)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Gastrointestinal :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Gastrointestinal_Bowel)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Bowel/Blader :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gastrointestinal_Bowel"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Gastrointestinal_Intake)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Intake and Tolerance :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gastrointestinal_Intake"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ********************* Evaluation *****************
                bool Evaluation_Strengths = false; if (ds.Tables[1].Rows[0]["Evaluation_Strengths"].ToString().Trim().Length > 0) { Evaluation_Strengths = true; }

                bool Evaluation_Concern_Barriers = false; if (ds.Tables[1].Rows[0]["Evaluation_Concern_Barriers"].ToString().Trim().Length > 0) { Evaluation_Concern_Barriers = true; }
                bool Evaluation_Concern_Limitations = false; if (ds.Tables[1].Rows[0]["Evaluation_Concern_Limitations"].ToString().Trim().Length > 0) { Evaluation_Concern_Limitations = true; }
                bool Evaluation_Concern_Posture = false; if (ds.Tables[1].Rows[0]["Evaluation_Concern_Posture"].ToString().Trim().Length > 0) { Evaluation_Concern_Posture = true; }
                bool Evaluation_Concern_Impairment = false; if (ds.Tables[1].Rows[0]["Evaluation_Concern_Impairment"].ToString().Trim().Length > 0) { Evaluation_Concern_Impairment = true; }
                bool Evaluation_AreaofConcerns = false; if (Evaluation_Concern_Barriers || Evaluation_Concern_Limitations || Evaluation_Concern_Posture || Evaluation_Concern_Impairment) { Evaluation_AreaofConcerns = true; }

                bool Evaluation_Goal_Summary = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString().Trim().Length > 0) { Evaluation_Goal_Summary = true; }
                bool Evaluation_Goal_Previous = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_Previous"].ToString().Trim().Length > 0) { Evaluation_Goal_Previous = true; }
                bool Evaluation_Goal_LongTerm = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_LongTerm"].ToString().Trim().Length > 0) { Evaluation_Goal_LongTerm = true; }
                bool Evaluation_Goal_ShortTerm = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_ShortTerm"].ToString().Trim().Length > 0) { Evaluation_Goal_ShortTerm = true; }
                bool Evaluation_Goal_Impairment = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_Impairment"].ToString().Trim().Length > 0) { Evaluation_Goal_Impairment = true; }
                bool Evaluation_Goal = false; if (Evaluation_Goal_Summary || Evaluation_Goal_Previous || Evaluation_Goal_LongTerm || Evaluation_Goal_ShortTerm || Evaluation_Goal_Impairment) { Evaluation_Goal = true; }

                bool Evaluation_Plan_Frequency = false; if (ds.Tables[1].Rows[0]["Evaluation_Plan_Frequency"].ToString().Trim().Length > 0) { Evaluation_Plan_Frequency = true; }
                bool Evaluation_Plan_Service = false; if (ds.Tables[1].Rows[0]["Evaluation_Plan_Service"].ToString().Trim().Length > 0) { Evaluation_Plan_Service = true; }
                bool Evaluation_Plan_Strategies = false; if (ds.Tables[1].Rows[0]["Evaluation_Plan_Strategies"].ToString().Trim().Length > 0) { Evaluation_Plan_Strategies = true; }
                bool Evaluation_Plan_Equipment = false; if (ds.Tables[1].Rows[0]["Evaluation_Plan_Equipment"].ToString().Trim().Length > 0) { Evaluation_Plan_Equipment = true; }
                bool Evaluation_Plan_Education = false; if (ds.Tables[1].Rows[0]["Evaluation_Plan_Education"].ToString().Trim().Length > 0) { Evaluation_Plan_Education = true; }
                bool Evaluation_PlanOfCare = false; if (Evaluation_Plan_Frequency || Evaluation_Plan_Service || Evaluation_Plan_Strategies || Evaluation_Plan_Equipment || Evaluation_Plan_Education) { Evaluation_PlanOfCare = true; }

                if (Evaluation_Strengths || Evaluation_AreaofConcerns || Evaluation_Goal || Evaluation_PlanOfCare)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Evaluation :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region ************ Strengths ****************
                    if (Evaluation_Strengths)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Strengths :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Strengths"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region ********** Area of Concerns *************
                    if (Evaluation_AreaofConcerns)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Area of Concerns :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (Evaluation_Concern_Barriers)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Barriers :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Concern_Barriers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Concern_Limitations)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Functional Limitations :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Concern_Limitations"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Concern_Posture)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Posture and Movement Limitation(Prioritized) :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Concern_Posture"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Concern_Impairment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Impairment(Prioritized) :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Concern_Impairment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion

                    #region *************** Goals **************
                    if (Evaluation_Goal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Goals :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (Evaluation_Goal_Summary)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Summary :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Goal_Previous)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Previous Long Term Goals :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Goal_Previous"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Goal_LongTerm)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Long Term Goals(Functional Outcome Measured)1 - Year :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);


                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Goal_LongTerm"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Goal_ShortTerm)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Short Term Goals(Functional Outcome Measures) 3 - Month :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Goal_ShortTerm"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Evaluation_Goal_Impairment)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Impairment related Objective goal-3 Months :", NormalItalic), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Goal_Impairment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion

                    #region ******************* Plan Of Care ****************
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;
                    cell = new PdfPCell(PhraseCell(new Phrase("Plan Of Care :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f;
                    cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    #region
                    if (Evaluation_Plan_Frequency)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Frequency and Duration :", NormalItalic), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);


                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Plan_Frequency"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Evaluation_Plan_Service)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Service Delivery Models :", NormalItalic), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);


                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Plan_Service"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Evaluation_Plan_Strategies)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Strategies to Address Impairments and Posture Movement Issues Motor Learning :", NormalItalic), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Plan_Strategies"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Evaluation_Plan_Equipment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Equipment / Adjuncts :", NormalItalic), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);


                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Plan_Equipment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Evaluation_Plan_Education)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Client / Family Education :", NormalItalic), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_Plan_Education"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #endregion
                }
                #endregion

                #region ****************** END OF PRINT CONTENT *********************
                int _Doctor_Physioptherapist = 0; string Doctor_Physioptherapist = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Physioptherapist"].ToString(), out _Doctor_Physioptherapist);
                DMD = DMB.Get(_Doctor_Physioptherapist); if (DMD != null) { Doctor_Physioptherapist = DMD.PreFix + " " + DMD.FullName; }

                int _Doctor_Occupational = 0; string Doctor_Occupational = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Occupational"].ToString(), out _Doctor_Occupational);
                DMD = DMB.Get(_Doctor_Occupational); if (DMD != null) { Doctor_Occupational = DMD.PreFix + " " + DMD.FullName; }

                int _Doctor_EnterReport = 0; string Doctor_Director = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_EnterReport"].ToString(), out _Doctor_EnterReport);
                DMD = DMB.Get(_Doctor_EnterReport); if (DMD != null) { Doctor_Director = DMD.PreFix + " " + DMD.FullName; }

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
                //context.Response.AddHeader("Content-Disposition", "inline; filename=" + _fileName + "");
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
                mail = AM.sendAttachment(mailid, "", "Eval Report", result_sheet);
                if (mail)
                {
                    DbHelper.SqlDb db = new DbHelper.SqlDb();
                    SqlCommand cmd = new SqlCommand("UPDATE Report_SiMst SET MailSend=CAST('True'AS BIT) WHERE AppointmentID=@AppointmentID");
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

        private class MailSIReport
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