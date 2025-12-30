using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Globalization;


namespace snehrehab.Handler
{
    /// <summary>
    /// Summary description for Ndt_Report_MailPDF_new
    /// </summary>
    public class Ndt_Report_MailPDF_new : IHttpHandler, IRequiresSessionState
    {

        int _loginID = 0; string str = "<style>html{background: url(\"/images/bg_login.jpg\");}.alert{margin: 0 auto;max-width: 475px;margin-top: 10%;background: #F5F5F5;padding: 20px;color: #6E6666;font-family: calibri;font-size: 1.2em;min-height: 50px;border: 2px solid #A8A8A8;border-radius: 2px;font-weight: lighter;line-height: 30px;}</style>";
        int _appointmentID = 0; string mailid = string.Empty; string type = string.Empty;
        int _receivertype = 0;
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
            NDTReport evnt = jsonSerializer.Deserialize<NDTReport>(requestBody);
            int.TryParse(evnt.SiAppointmentID.ToString(), out _appointmentID);
            mailid = evnt.MailID;
            int.TryParse(evnt.Receivertype.ToString(), out _receivertype);

            if (_appointmentID <= 0 || string.IsNullOrEmpty(mailid) || _receivertype <= 0)
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.ReportNdtMst_Bll RNB = new SnehBLL.ReportNdtMst_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataSet ds = RNB.Get_NDT(_appointmentID);
            Document document = new Document(PageSize.A4, 30f, 30f, 50f, 30f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            Font SubHeadingFont = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font NextHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("NDT Report 2025 - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

                string result_sheet = HttpContext.Current.Server.MapPath("~/Files/Receipt/") + _fileName;
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
                table.TotalWidth = 530f;
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
                table.AddCell(PhraseCell(new Phrase("Patient Code", NormalFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                table.AddCell(PhraseCell(new Phrase(ds.Tables[0].Rows[0]["PatientCode"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                if (ds.Tables[0].Rows[0]["DiagnosisNames"].ToString().Trim().Length > 0 || ds.Tables[0].Rows[0]["DiagnosisOther"].ToString().Trim().Length > 0)
                {
                    DiagnosisNames = true;
                }
                if (DiagnosisNames)
                {
                    string Diagnosis = ds.Tables[0].Rows[0]["DiagnosisNames"].ToString().Trim();
                    string DiagnosisOther = ds.Tables[0].Rows[0]["DiagnosisOther"].ToString();
                    if (!string.IsNullOrEmpty(DiagnosisOther))
                    {
                        Diagnosis += ", " + DiagnosisOther;
                    }
                    Diagnosis = Diagnosis.Trim();
                    if (Diagnosis.EndsWith(","))
                    {
                        Diagnosis = Diagnosis.Substring(0, Diagnosis.LastIndexOf(","));// +".";
                    }
                    #region
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
                    #endregion
                }


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
                document.Add(Chunk.NEXTPAGE);
                #endregion

                #region****Functional Abilities and Limitations***
                // Boolean variables for multiple parameters
                bool FA_GrossMotor_Ability = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_GrossMotor_Ability"].ToString().Trim());
                bool FA_GrossMotor_Limit = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_GrossMotor_Limit"].ToString().Trim());
                bool FA_FineMotor_Ability = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_FineMotor_Ability"].ToString().Trim());
                bool FA_FineMotor_Limit = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_FineMotor_Limit"].ToString().Trim());
                bool FA_Communication_Ability = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_Communication_Ability"].ToString().Trim());
                bool FA_Communication_Limit = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_Communication_Limit"].ToString().Trim());
                bool FA_Cognition_Ability = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_Cognition_Ability"].ToString().Trim());
                bool FA_Cognition_Limit = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["FA_Cognition_Limit"].ToString().Trim());
                if (FA_GrossMotor_Ability || FA_GrossMotor_Limit || FA_FineMotor_Ability || FA_FineMotor_Limit || FA_Communication_Ability || FA_Communication_Limit || FA_Cognition_Ability || FA_Cognition_Limit)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Functional Abilities and Limitation :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region**Inertia**
                    if (FA_GrossMotor_Ability || FA_GrossMotor_Limit || FA_FineMotor_Ability || FA_FineMotor_Limit || FA_Communication_Ability || FA_Communication_Limit || FA_Cognition_Ability || FA_Cognition_Limit)
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
                        if (FA_GrossMotor_Ability || FA_GrossMotor_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_GrossMotor_Ability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_GrossMotor_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Fine Motor
                        if (FA_FineMotor_Ability || FA_FineMotor_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_FineMotor_Ability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_FineMotor_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Communication
                        if (FA_Communication_Ability || FA_Communication_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_Communication_Ability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_Communication_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Cognition
                        if (FA_Cognition_Ability || FA_Cognition_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Cognition", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_Cognition_Ability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FA_Cognition_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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

                }

                #endregion

                #region ****************** Participation Ability and Limitations ******************
                bool ParticipationAbility_GrossMotor = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor"].ToString().Trim().Length > 0) { ParticipationAbility_GrossMotor = true; }
                bool ParticipationAbility_FineMotor = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor"].ToString().Trim().Length > 0) { ParticipationAbility_FineMotor = true; }
                bool ParticipationAbility_Communication = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_Communication"].ToString().Trim().Length > 0) { ParticipationAbility_Communication = true; }
                bool ParticipationAbility_Cognition = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_Cognition"].ToString().Trim().Length > 0) { ParticipationAbility_Cognition = true; }
                bool ParticipationAbility_GrossMotor_Limit = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor_Limit"].ToString().Trim().Length > 0) { ParticipationAbility_GrossMotor_Limit = true; }
                bool ParticipationAbility_FineMotor_Limit = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor_Limit"].ToString().Trim().Length > 0) { ParticipationAbility_FineMotor_Limit = true; }
                bool ParticipationAbility_Communication_Limit = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_Communication_Limit"].ToString().Trim().Length > 0) { ParticipationAbility_Communication_Limit = true; }
                bool ParticipationAbility_Cognition_Limit = false; if (ds.Tables[1].Rows[0]["ParticipationAbility_Cognition_Limit"].ToString().Trim().Length > 0) { ParticipationAbility_Cognition_Limit = true; }
                bool Contextual_Personal_Positive = false; if (ds.Tables[1].Rows[0]["Contextual_Personal_Positive"].ToString().Trim().Length > 0) { Contextual_Personal_Positive = true; }
                bool Contextual_Personal_Negative = false; if (ds.Tables[1].Rows[0]["Contextual_Personal_Negative"].ToString().Trim().Length > 0) { Contextual_Personal_Negative = true; }
                bool Contextual_Enviremental_Positive = false; if (ds.Tables[1].Rows[0]["Contextual_Environmental_Positive"].ToString().Trim().Length > 0) { Contextual_Enviremental_Positive = true; }
                bool Contextual_Enviremental_Negative = false; if (ds.Tables[1].Rows[0]["Contextual_Environmental_Negative"].ToString().Trim().Length > 0) { Contextual_Enviremental_Negative = true; }
                if (ParticipationAbility_GrossMotor || ParticipationAbility_FineMotor || ParticipationAbility_Communication || ParticipationAbility_Cognition ||
                    ParticipationAbility_GrossMotor_Limit || ParticipationAbility_FineMotor_Limit || ParticipationAbility_Communication_Limit || ParticipationAbility_Cognition_Limit ||
                    Contextual_Personal_Positive || Contextual_Personal_Negative || Contextual_Enviremental_Positive || Contextual_Enviremental_Negative)
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
                    if (ParticipationAbility_GrossMotor || ParticipationAbility_FineMotor || ParticipationAbility_Communication || ParticipationAbility_Cognition ||
                    ParticipationAbility_GrossMotor_Limit || ParticipationAbility_FineMotor_Limit || ParticipationAbility_Communication_Limit || ParticipationAbility_Cognition_Limit)
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
                        if (ParticipationAbility_GrossMotor || ParticipationAbility_GrossMotor_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_GrossMotor_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Fine Motor
                        if (ParticipationAbility_FineMotor || ParticipationAbility_FineMotor_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_FineMotor_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Communication
                        if (ParticipationAbility_Communication || ParticipationAbility_Communication_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_Communication"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_Communication_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region Cognition
                        if (ParticipationAbility_Cognition || ParticipationAbility_Cognition_Limit)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Cognition", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_Cognition"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ParticipationAbility_Cognition_Limit"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        #endregion
                        document.Add(table);
                    }
                    if (Contextual_Personal_Positive || Contextual_Personal_Negative || Contextual_Enviremental_Positive || Contextual_Enviremental_Negative)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Contextual factors :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        #region Header
                        cell = new PdfPCell(PhraseCell(new Phrase("Personal", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Environmental", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Colspan = 2;
                        table.AddCell(cell);
                        #endregion
                        #region Row 1
                        cell = new PdfPCell(PhraseCell(new Phrase("Positive :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Contextual_Personal_Positive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Positive :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Contextual_Environmental_Positive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                        #endregion
                        #region Row 1

                        cell = new PdfPCell(PhraseCell(new Phrase("Negative :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Contextual_Personal_Negative"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Negative :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Contextual_Environmental_Negative"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        #endregion
                        document.Add(table);
                    }
                }
                #endregion

                #region**********Multisyatemposture**********
                bool Posture_Head = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_head"].ToString().Trim())) { Posture_Head = true; }
                bool Posture_allign_right = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_Alligmnet_right"].ToString().Trim())) { Posture_allign_right = true; }
                bool Posture_allign_left = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_headAlligmnet_left"].ToString().Trim())) { Posture_allign_left = true; }
                bool Posture_shoulder = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_Shoulder"].ToString().Trim())) { Posture_shoulder = true; }
                bool Posture_neck = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_neck"].ToString().Trim())) { Posture_neck = true; }
                bool Posture_Scapulae = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_scapulae"].ToString().Trim())) { Posture_Scapulae = true; }
                bool Posture_elbow = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_elbow"].ToString().Trim())) { Posture_elbow = true; }
                bool Posture_forearm = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_forarm"].ToString().Trim())) { Posture_forearm = true; }
                bool Posture_wrist = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_wrist"].ToString().Trim())) { Posture_wrist = true; }
                bool Posture_hand = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_hand"].ToString().Trim())) { Posture_hand = true; }
                bool Posture_finger = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_finger"].ToString().Trim())) { Posture_finger = true; }
                bool Posture_Thumb = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_thumb"].ToString().Trim())) { Posture_Thumb = true; }
                bool Posture_ribcage = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_ribcage"].ToString().Trim())) { Posture_ribcage = true; }
                bool Posture_Thoracicspine = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_thoracicspine"].ToString().Trim())) { Posture_Thoracicspine = true; }
                bool Posture_LumbarSpine = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_lumbarspine"].ToString().Trim())) { Posture_LumbarSpine = true; }
                bool Posture_Pelvis = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_pelvis"].ToString().Trim())) { Posture_Pelvis = true; }
                bool Posture_Hips = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_hips"].ToString().Trim())) { Posture_Hips = true; }
                bool Posture_Knees = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_knees"].ToString().Trim())) { Posture_Knees = true; }
                bool Posture_Ankle = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_ankle"].ToString().Trim())) { Posture_Ankle = true; }
                bool Posture_Feet = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_feet"].ToString().Trim())) { Posture_Feet = true; }
                bool Posture_Toes = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_toes"].ToString().Trim())) { Posture_Toes = true; }
                bool Posture_BOS = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_bos"].ToString().Trim())) { Posture_BOS = true; }
                bool Posture_com = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_com_cog"].ToString().Trim())) { Posture_com = true; }
                bool Posture_stabily = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_stabiltymethod"].ToString().Trim())) { Posture_stabily = true; }
                bool Multi_posture_Stability_comments = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_Stability"].ToString().Trim())) { Multi_posture_Stability_comments = true; }
                bool Posture_Counter = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_postural"].ToString().Trim())) { Posture_Counter = true; }
                bool Posture_Contrl = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_anticipatory"].ToString().Trim())) { Posture_Contrl = true; }
                bool AlignmentType = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_headAlignment_AlignmentType"].ToString().Trim())) { AlignmentType = true; }
                if (Posture_Head || Posture_allign_right || Posture_allign_left || Posture_shoulder || Posture_neck || Posture_Scapulae || Posture_elbow ||
                    Posture_forearm || Posture_wrist || Posture_finger || Posture_Thumb || Posture_ribcage || Posture_Thoracicspine || Posture_LumbarSpine || Posture_Pelvis
                    || Posture_Hips || Posture_Knees || Posture_Ankle || Posture_Feet || Posture_Toes || Posture_BOS || Posture_com || Posture_stabily)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Multisystem -Posture :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region*** alignment***
                    if (AlignmentType)
                    {

                        // Get alignment values from dataset (assuming values are stored in ds.Tables[1])
                        string alignmentText = ds.Tables[1].Rows[0]["Multi_posture_headAlignment_AlignmentType"].ToString().Trim();

                        // Create table for Alignment section
                        if (!string.IsNullOrEmpty(alignmentText))
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            // Add heading "Alignment"
                            cell = new PdfPCell(PhraseCell(new Phrase("Alignment", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 4f;
                            cell.PaddingLeft = 50f;
                            cell.Border = Rectangle.NO_BORDER;
                            table.AddCell(cell);

                            // Add alignment text
                            cell = new PdfPCell(PhraseCell(new Phrase(alignmentText, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 4f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            document.Add(table);
                        }
                    }

                    #endregion
                    #region**head**
                    if (Posture_Head)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Head :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_head"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Shoulder**
                    if (Posture_shoulder)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Shoulder :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_Shoulder"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion

                    #region**Neck**
                    if (Posture_neck)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Neck :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_neck"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Scapulae**
                    if (Posture_Scapulae)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Scapulae:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_scapulae"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**elbow**
                    if (Posture_elbow)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Elbow:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_elbow"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**fprearm**
                    if (Posture_forearm)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Forearm:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_forarm"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**wrists**
                    if (Posture_wrist)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Wrist:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_wrist"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Hand**
                    if (Posture_hand)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Hand:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_hand"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Finger**
                    if (Posture_finger)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Finger:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_finger"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**thumb**
                    if (Posture_Thumb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Thumb: ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_thumb"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**ribcage**
                    if (Posture_ribcage)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Ribcage : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_ribcage"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Thoracic Spine**
                    if (Posture_Thoracicspine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Thoracic Spine : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_thoracicspine"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Lumbar Spine**
                    if (Posture_LumbarSpine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Lumbar Spine : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_lumbarspine"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Pelvis**
                    if (Posture_LumbarSpine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Pelvis : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_pelvis"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Hips**
                    if (Posture_Hips)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Hips : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_hips"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Knees **
                    if (Posture_Knees)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Knees  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_knees"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Ankle **
                    if (Posture_Ankle)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Ankle  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_ankle"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region** Feet **
                    if (Posture_Feet)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase(" Feet  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_feet"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region** Toes **
                    if (Posture_Toes)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase(" Toes  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_toes"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**  BOS **
                    if (Posture_BOS)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("  BOS  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_bos"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**  COM & COG **
                    if (Posture_com)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("  COM & COG  : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_com_cog"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**  Strategies for Stability **
                    if (Posture_stabily)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("  Strategies for Stability : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_posture_stabiltymethod"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            var uniquePostures = new HashSet<string>(
                                postureValues.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p))
                            );

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            foreach (string posture in uniquePostures)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture, NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }

                    }
                    if (Multi_posture_Stability_comments)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("  Comments : ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Multi_posture_Stability"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region****oral Alignmnet**
                    bool Posture_Mouth = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_mouth"].ToString().Trim())) { Posture_Mouth = true; }
                    bool Posture_Toungh = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_toungh"].ToString().Trim())) { Posture_Toungh = true; }
                    bool Posture_teeth = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_teeth"].ToString().Trim())) { Posture_teeth = true; }
                    bool Posture_Chin = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_chin"].ToString().Trim())) { Posture_Chin = true; }
                    bool Posture_Cheeks = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_cheeks"].ToString().Trim())) { Posture_Mouth = true; }
                    bool Posture_lips = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_posture_lips"].ToString().Trim())) { Posture_Mouth = true; }
                    bool sp1 = false;
                    if (Posture_Mouth || Posture_Toungh || Posture_teeth || Posture_Chin || Posture_Cheeks || Posture_lips)
                    {
                        sp1 = true;
                    }
                    if (sp1)
                    {
                        // Create a table with 2 columns (Structure & Alignment)
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Section Title
                        cell = new PdfPCell(PhraseCell(new Phrase("Oral Structure Alignment:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        cell.Colspan = 2; // Merge two columns for the title
                        cell.BackgroundColor = new BaseColor(230, 230, 230);
                        table.AddCell(cell);

                        // Add Header Row
                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        table.AddCell(new PdfPCell(new Phrase("Structure", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(200, 200, 200) });
                        table.AddCell(new PdfPCell(new Phrase("Alignment", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(200, 200, 200) });

                        // Add Data Rows
                        void AddRow(string label, string columnName)
                        {
                            if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0][columnName]?.ToString()))
                            {
                                cell = new PdfPCell(new Phrase(label, NormalFont));
                                cell.BorderColor = BaseColor.LIGHT_GRAY;
                                cell.Padding = 5;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(ds.Tables[1].Rows[0][columnName].ToString(), NormalFont));
                                cell.BorderColor = BaseColor.LIGHT_GRAY;
                                cell.Padding = 5;
                                table.AddCell(cell);
                            }
                        }

                        AddRow("Mouth", "Multi_posture_mouth");
                        AddRow("Tongue", "Multi_posture_toungh");
                        AddRow("Teeth", "Multi_posture_teeth");
                        AddRow("Chin", "Multi_posture_chin");
                        AddRow("Cheeks", "Multi_posture_cheeks");
                        AddRow("Lips", "Multi_posture_lips");

                        // Add the table to the document
                        document.Add(table);
                    }

                    #endregion
                    #region**
                    if (Posture_Contrl)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" Anticipatory Control ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Multi_posture_anticipatory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Posture_Counter)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" Postural Counter Balance  ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Multi_posture_postural"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    #endregion

                }
                #endregion

                #region ******Multisystem Movement*******
                bool Movement_Inertia = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Movement_Inertia"].ToString().Trim())) { Movement_Inertia = true; }
                bool Multi_Movement_TypeOf_Quality = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_TypeOf_Quality"].ToString().Trim())) { Multi_Movement_TypeOf_Quality = true; }
                bool Multi_Movement_Plane = false;
                bool Movement_plane = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_Plane"].ToString().Trim())) { Movement_plane = true; }
                bool Movement_interlimb = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_interlimb"].ToString().Trim())) { Movement_interlimb = true; }

                bool txtSoinePoor = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtSoinePoor"].ToString().Trim())) { txtSoinePoor = true; }
                bool txtSoineFair = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtSoineFair"].ToString().Trim())) { txtSoineFair = true; }
                bool txtSoineGood = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtSoineGood"].ToString().Trim())) { txtSoineGood = true; }

                bool txtScapuloPoor = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtScapuloPoor"].ToString().Trim())) { txtScapuloPoor = true; }
                bool txtScapuloFair = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtScapuloFair"].ToString().Trim())) { txtScapuloFair = true; }
                bool txtScapuloGood = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtScapuloGood"].ToString().Trim())) { txtScapuloGood = true; }

                bool txtPelviPoor = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtPelviPoor"].ToString().Trim())) { txtPelviPoor = true; }
                bool txtPelviFair = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtPelviFair"].ToString().Trim())) { txtPelviFair = true; }
                bool txtPelviGood = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtPelviGood"].ToString().Trim())) { txtPelviGood = true; }

                bool txtWithinUlPoor = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinUlPoor"].ToString().Trim())) { txtWithinUlPoor = true; }
                bool txtWithinUlFair = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinUlFair"].ToString().Trim())) { txtWithinUlFair = true; }
                bool txtWithinUlGood = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinUlGood"].ToString().Trim())) { txtWithinUlGood = true; }

                bool txtWithinLlPoor = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinLlPoor"].ToString().Trim())) { txtWithinLlPoor = true; }
                bool txtWithinLlFair = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinLlFair"].ToString().Trim())) { txtWithinLlFair = true; }
                bool txtWithinLlGood = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["txtWithinLlGood"].ToString().Trim())) { txtWithinLlGood = true; }


                bool Movement_intralimb = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_intralimb"].ToString().Trim())) { Movement_intralimb = true; }
                bool Movement_upperlimb = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["UpperLimb_Movement"].ToString().Trim())) { Movement_upperlimb = true; }
                bool Movement_lowerlimb = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["LowerLimb_Movement"].ToString().Trim())) { Movement_lowerlimb = true; }
                bool Movement_Cervical = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["CervicalSpine_Movement"].ToString().Trim())) { Movement_Cervical = true; }
                bool Movement_thoriac = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["ThoracicSpine_Movement"].ToString().Trim())) { Movement_thoriac = true; }
                bool Movement_Stabilty = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_statbilty"].ToString().Trim())) { Movement_Stabilty = true; }
                bool Movement_overuse = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_overuse"].ToString().Trim())) { Movement_overuse = true; }
                bool Movement_Maintain = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_Bal_maintain"].ToString().Trim())) { Movement_Maintain = true; }
                bool Movement_balance = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_BAl_during"].ToString().Trim())) { Movement_balance = true; }

                bool Genral_observer = false; if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Gene_obsr_comments"].ToString().Trim())) { Genral_observer = true; }


                if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_Plane"].ToString().Trim()))
                {
                    Multi_Movement_Plane = true;
                }
                bool Multi_Movement_WeightShift = false;
                if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Multi_Movement_WeightShift"].ToString().Trim()))
                {
                    Multi_Movement_WeightShift = true;
                }

                if (Movement_Inertia || Multi_Movement_TypeOf_Quality || Multi_Movement_Plane || Multi_Movement_WeightShift || Movement_plane || Movement_intralimb || Movement_interlimb
                   || Movement_upperlimb || Movement_lowerlimb || Movement_thoriac || Movement_Cervical || Movement_thoriac || Movement_Stabilty || Movement_overuse || Movement_Maintain || Movement_balance
                   || txtSoinePoor || txtSoineFair || txtSoineGood || txtScapuloPoor || txtScapuloFair || txtScapuloGood || txtPelviPoor || txtPelviFair || txtPelviGood
                   || txtWithinUlPoor || txtWithinUlFair || txtWithinUlGood || txtWithinLlPoor || txtWithinLlFair || txtWithinLlGood)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Multisystem -Movement :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region**Inertia**
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
                    if (Multi_Movement_TypeOf_Quality)
                    {
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 0.7f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Quality of movement?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Multi_Movement_TypeOf_Quality"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion
                    #region**plane movement**
                    if (Movement_plane)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Plane of Movements :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_Plane"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region
                    if (Multi_Movement_WeightShift)
                    {
                        string movementTypes = ds.Tables[1].Rows[0]["Multi_Movement_WeightShift"].ToString();
                        string formattedText = string.Join("\n", movementTypes.Split(','));
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 0.7f });
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" Weight Shifts :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(formattedText, NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion
                    #region**plane movement interlimb**
                    if (Movement_interlimb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Dissociation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Interlimb Dissociation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_interlimb"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }

                    #endregion
                    #region *******interlimb ****
                    if (txtSoinePoor || txtSoineFair || txtSoineGood || txtScapuloPoor || txtScapuloFair || txtScapuloGood || txtPelviPoor || txtPelviFair || txtPelviGood
                        || txtWithinUlPoor || txtWithinUlFair || txtWithinUlGood || txtWithinLlPoor || txtWithinLlFair || txtWithinLlGood)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Interlimb Dissociation", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Poor", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Fair", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Good", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                        if (txtSoinePoor || txtSoineFair || txtSoineGood)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Spine to shoulder gridle", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtSoinePoor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtSoineFair"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtSoineGood"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                        }
                        if (txtScapuloPoor || txtScapuloFair || txtScapuloGood)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Scapulohumeral", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtScapuloPoor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtScapuloFair"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtScapuloGood"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                        }
                        if (txtPelviPoor || txtPelviFair || txtPelviGood)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Pelvefemerol", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtPelviPoor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtPelviFair"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtPelviGood"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                        }
                        if (txtWithinUlPoor || txtWithinUlFair || txtWithinUlGood)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Within UL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinUlPoor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinUlFair"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinUlGood"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                        }
                        if (txtWithinLlPoor || txtWithinLlFair || txtWithinLlGood)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Within LL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinLlPoor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinLlFair"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtWithinLlGood"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                        }
                        document.Add(table);
                    }
                    #endregion
                    #region**plane movement intralimb**
                    if (Movement_intralimb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Intralimb Dissociation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_intralimb"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**plane movement interlimb**
                    if (Movement_upperlimb || Movement_lowerlimb || Movement_Cervical || Movement_thoriac)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Range of Movements::", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (Movement_upperlimb)
                    {


                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Upper Limb:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["UpperLimb_Movement"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**plane movement lowerlimb**
                    if (Movement_lowerlimb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Lower Limb :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["LowerLimb_Movement"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**plane movement Cervical Spine*
                    if (Movement_Cervical)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Cervical Spine :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["CervicalSpine_Movement"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**plane movement thoriac**
                    if (Movement_thoriac)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Thoracic Spine:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["ThoracicSpine_Movement"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Strategies For Stability**
                    if (Movement_Stabilty)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Strategies For Stability & Mobility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_statbilty"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Sign of Movement System Impairment or Overuse **
                    if (Movement_overuse)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("Sign of Movement System Impairment or Overuse  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_overuse"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(position_head, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**balance maintain **
                    if (Movement_Maintain)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("While maintaining a posture :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_Bal_maintain"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region**Balance during **
                    if (Movement_balance)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("During Transitions  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Multi_Movement_BAl_during"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            string[] postureValues = position_head.Split(',');
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            foreach (string posture in postureValues)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase(posture.Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                    }
                    #endregion
                    #region*** general observation ****
                    if (Genral_observer)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        // Add Heading
                        cell = new PdfPCell(PhraseCell(new Phrase("General observations  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        string position_head = ds.Tables[1].Rows[0]["Gene_obsr_comments"].ToString();

                        if (!string.IsNullOrEmpty(position_head))
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(position_head, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                    }
                    #endregion


                }
                #endregion

                #region ************** Neuromoter System *********************
                bool Neuromotor_Recruitment_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Initial"].ToString().Trim().Length > 0) { Neuromotor_Recruitment_Initial = true; }

                bool Neurometer_Initialigy_Control = false, Neurometer_Initialigy_Termination = false, Neurometer_Initialigy_Sustainance = false, Neurometer_Initialigy_initial = false;
                if (ds.Tables[1].Rows[0]["Neurometer_Initialigy_Control"].ToString().Trim().Length > 0) Neurometer_Initialigy_Control = true;
                if (ds.Tables[1].Rows[0]["Neurometer_Initialigy_Termination"].ToString().Trim().Length > 0) Neurometer_Initialigy_Termination = true;
                if (ds.Tables[1].Rows[0]["Neurometer_Initialigy_Sustainance"].ToString().Trim().Length > 0) Neurometer_Initialigy_Sustainance = true;
                if (ds.Tables[1].Rows[0]["Neurometer_Initialigy_initial"].ToString().Trim().Length > 0) Neurometer_Initialigy_initial = true;

                bool Neuromotor_Recruitment_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Recruitment_Sustainance = true; }
                bool Neuromotor_Recruitment_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Termination"].ToString().Trim().Length > 0) { Neuromotor_Recruitment_Termination = true; }
                bool Neuromotor_Recruitment_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Control"].ToString().Trim().Length > 0) { Neuromotor_Recruitment_Control = true; }
                bool Neuromotor_Contraction_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Contraction_Initial"].ToString().Trim().Length > 0) { Neuromotor_Contraction_Initial = true; }
                bool Neuromotor_Contraction_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Contraction_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Contraction_Sustainance = true; }
                bool Neuromotor_Contraction_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Contraction_Termination"].ToString().Trim().Length > 0) { Neuromotor_Contraction_Termination = true; }
                bool Neuromotor_Contraction_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Contraction_Control"].ToString().Trim().Length > 0) { Neuromotor_Contraction_Control = true; }
                bool Neuromotor_Coactivation_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Initial"].ToString().Trim().Length > 0) { Neuromotor_Coactivation_Initial = true; }
                bool Neuromotor_Coactivation_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Coactivation_Sustainance = true; }
                bool Neuromotor_Coactivation_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Termination"].ToString().Trim().Length > 0) { Neuromotor_Coactivation_Termination = true; }
                bool Neuromotor_Coactivation_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Control"].ToString().Trim().Length > 0) { Neuromotor_Coactivation_Control = true; }
                bool Neuromotor_Synergy_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Synergy_Initial"].ToString().Trim().Length > 0) { Neuromotor_Synergy_Initial = true; }
                bool Neuromotor_Synergy_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Synergy_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Synergy_Sustainance = true; }
                bool Neuromotor_Synergy_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Synergy_Termination"].ToString().Trim().Length > 0) { Neuromotor_Synergy_Termination = true; }
                bool Neuromotor_Synergy_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Synergy_Control"].ToString().Trim().Length > 0) { Neuromotor_Synergy_Control = true; }
                bool Neuromotor_Stiffness_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Initial"].ToString().Trim().Length > 0) { Neuromotor_Stiffness_Initial = true; }
                bool Neuromotor_Stiffness_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Stiffness_Sustainance = true; }
                bool Neuromotor_Stiffness_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Termination"].ToString().Trim().Length > 0) { Neuromotor_Stiffness_Termination = true; }
                bool Neuromotor_Stiffness_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Control"].ToString().Trim().Length > 0) { Neuromotor_Stiffness_Control = true; }
                bool Neuromotor_Extraneous_Initial = false; if (ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Initial"].ToString().Trim().Length > 0) { Neuromotor_Extraneous_Initial = true; }
                bool Neuromotor_Extraneous_Sustainance = false; if (ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Sustainance"].ToString().Trim().Length > 0) { Neuromotor_Extraneous_Sustainance = true; }
                bool Neuromotor_Extraneous_Termination = false; if (ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Termination"].ToString().Trim().Length > 0) { Neuromotor_Extraneous_Termination = true; }
                bool Neuromotor_Extraneous_Control = false; if (ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Control"].ToString().Trim().Length > 0) { Neuromotor_Extraneous_Control = true; }

                bool OtherTest_Tardieus_TA_Right = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Right"].ToString().Trim().Length > 0) { OtherTest_Tardieus_TA_Right = true; }
                bool OtherTest_Tardieus_TA_Left = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Left"].ToString().Trim().Length > 0) { OtherTest_Tardieus_TA_Left = true; }
                bool OtherTest_Tardieus_Hamstring_Right = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Right"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Hamstring_Right = true; }
                bool OtherTest_Tardieus_Hamstring_Left = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Left"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Hamstring_Left = true; }
                bool OtherTest_Tardieus_Adductor_Right = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Right"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Adductor_Right = true; }
                bool OtherTest_Tardieus_Adductor_Left = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Left"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Adductor_Left = true; }
                bool OtherTest_Tardieus_Hip_Right = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Right"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Hip_Right = true; }
                bool OtherTest_Tardieus_Hip_Left = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Left"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Hip_Left = true; }
                bool OtherTest_Tardieus_Biceps_Right = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Right"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Biceps_Right = true; }
                bool OtherTest_Tardieus_Biceps_Left = false; if (ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Left"].ToString().Trim().Length > 0) { OtherTest_Tardieus_Biceps_Left = true; }

                bool SelectionMotorControl_Muscle = false; if (ds.Tables[1].Rows[0]["SelectionMotorControl_Muscle"].ToString().Trim().Length > 0) { SelectionMotorControl_Muscle = true; }
                bool SelectionMotorControl_Denvers = false; if (ds.Tables[1].Rows[0]["SelectionMotorControl_Denvers"].ToString().Trim().Length > 0) { SelectionMotorControl_Denvers = true; }
                bool SelectionMotorControl_GMFM = false; if (ds.Tables[1].Rows[0]["SelectionMotorControl_GMFM"].ToString().Trim().Length > 0) { SelectionMotorControl_GMFM = true; }
                bool SelectionMotorControl_MAS = false; if (ds.Tables[1].Rows[0]["SelectionMotorControl_MAS"].ToString().Trim().Length > 0) { SelectionMotorControl_MAS = true; }
                bool SelectionMotorControl_Observation = false; if (ds.Tables[1].Rows[0]["SelectionMotorControl_Observation"].ToString().Trim().Length > 0) { SelectionMotorControl_Observation = true; }

                bool TheFourA_Arousal = false; if (ds.Tables[1].Rows[0]["TheFourA_Arousal"].ToString().Trim().Length > 0) { TheFourA_Arousal = true; }
                bool TheFourA_Attention = false; if (ds.Tables[1].Rows[0]["TheFourA_Attention"].ToString().Trim().Length > 0) { TheFourA_Attention = true; }
                bool TheFourA_Affect = false; if (ds.Tables[1].Rows[0]["TheFourA_Affect"].ToString().Trim().Length > 0) { TheFourA_Affect = true; }
                bool TheFourA_Action = false; if (ds.Tables[1].Rows[0]["TheFourA_Action"].ToString().Trim().Length > 0) { TheFourA_Action = true; }
                bool TheFourA_StateRegulation = false; if (ds.Tables[1].Rows[0]["TheFourA_StateRegulation"].ToString().Trim().Length > 0) { TheFourA_StateRegulation = true; }

                #region
                if (Neuromotor_Recruitment_Initial || Neuromotor_Recruitment_Sustainance ||
                    Neuromotor_Recruitment_Termination || Neuromotor_Recruitment_Control || Neuromotor_Contraction_Initial || Neuromotor_Contraction_Sustainance ||
                    Neuromotor_Contraction_Termination || Neuromotor_Contraction_Control || Neuromotor_Coactivation_Initial || Neuromotor_Coactivation_Sustainance ||
                    Neuromotor_Coactivation_Termination || Neuromotor_Coactivation_Control || Neuromotor_Synergy_Initial || Neuromotor_Synergy_Sustainance ||
                    Neuromotor_Synergy_Termination || Neuromotor_Synergy_Control || Neuromotor_Stiffness_Initial || Neuromotor_Stiffness_Sustainance ||
                    Neuromotor_Stiffness_Termination || Neuromotor_Stiffness_Control || Neuromotor_Extraneous_Initial || Neuromotor_Extraneous_Sustainance ||
                    Neuromotor_Extraneous_Termination || Neuromotor_Extraneous_Control || Neurometer_Initialigy_Control || Neurometer_Initialigy_Termination || Neurometer_Initialigy_Sustainance || Neurometer_Initialigy_initial)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Neuromoter System :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    table = new PdfPTable(5);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    #region Header
                    cell = new PdfPCell(PhraseCell(new Phrase("Components", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                    cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Neck Trunk", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                    cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Upper Exthen", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                    cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Lower Exthen", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                    cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Control & Gradation", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                    cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    #endregion

                    #region
                    if (Neuromotor_Recruitment_Initial || Neuromotor_Recruitment_Sustainance ||
                        Neuromotor_Recruitment_Termination || Neuromotor_Recruitment_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Recruitment Movement Postrual", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Recruitment_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region 
                    if (Neurometer_Initialigy_initial || Neurometer_Initialigy_Sustainance ||
Neurometer_Initialigy_Termination || Neurometer_Initialigy_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Initialigy Sustainance Termination", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neurometer_Initialigy_initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neurometer_Initialigy_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neurometer_Initialigy_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neurometer_Initialigy_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Contraction_Initial || Neuromotor_Contraction_Sustainance ||
                    Neuromotor_Contraction_Termination || Neuromotor_Contraction_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Type of contraction Concentric Isometric Eccentric", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Contraction_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Contraction_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Contraction_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Contraction_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Coactivation_Initial || Neuromotor_Coactivation_Sustainance ||
                    Neuromotor_Coactivation_Termination || Neuromotor_Coactivation_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Co-activation / Reciprocal inhibition", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Coactivation_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Synergy_Initial || Neuromotor_Synergy_Sustainance ||
                    Neuromotor_Synergy_Termination || Neuromotor_Synergy_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Type of Synergy", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Synergy_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Synergy_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Synergy_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Synergy_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Stiffness_Initial || Neuromotor_Stiffness_Sustainance ||
                    Neuromotor_Stiffness_Termination || Neuromotor_Stiffness_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Stiffness(Static/Dynamic)", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Stiffness_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Extraneous_Initial || Neuromotor_Extraneous_Sustainance ||
                    Neuromotor_Extraneous_Termination || Neuromotor_Extraneous_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Extraneous Movement", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingTop = 5f;
                        cell.PaddingLeft = 5f; cell.PaddingRight = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    PdfPTable tableT = new PdfPTable(1);
                    tableT.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableT.WidthPercentage = 100;
                    tableT.SpacingBefore = 0f;

                    cell = new PdfPCell(table);
                    cell.PaddingBottom = 0f;
                    cell.PaddingLeft = 20f;
                    cell.BorderWidth = 0f;
                    tableT.AddCell(cell);

                    document.Add(tableT);
                    #endregion
                }
                #endregion

                #region
                if (OtherTest_Tardieus_TA_Right || OtherTest_Tardieus_TA_Left || OtherTest_Tardieus_Hamstring_Right ||
                    OtherTest_Tardieus_Hamstring_Left || OtherTest_Tardieus_Adductor_Right || OtherTest_Tardieus_Adductor_Left ||
                    OtherTest_Tardieus_Hip_Right || OtherTest_Tardieus_Hip_Left || OtherTest_Tardieus_Biceps_Right || OtherTest_Tardieus_Biceps_Left)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Other Tests :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    table = new PdfPTable(3);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    #region Header
                    cell = new PdfPCell(PhraseCell(new Phrase("", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f;
                    cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Right", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f;
                    cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Left", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.PaddingLeft = 5f;
                    table.AddCell(cell);
                    #endregion

                    #region
                    if (OtherTest_Tardieus_TA_Right || OtherTest_Tardieus_TA_Left)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("TA", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_TA_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (OtherTest_Tardieus_Hamstring_Right || OtherTest_Tardieus_Hamstring_Left)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Hamstrings", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hamstring_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (OtherTest_Tardieus_Adductor_Right || OtherTest_Tardieus_Adductor_Left)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Adductors", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Adductor_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (OtherTest_Tardieus_Hip_Right || OtherTest_Tardieus_Hip_Left)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Hip Flexor Angle", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Hip_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (OtherTest_Tardieus_Biceps_Right || OtherTest_Tardieus_Biceps_Left)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Biceps", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OtherTest_Tardieus_Biceps_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region
                    if (Neuromotor_Extraneous_Initial || Neuromotor_Extraneous_Sustainance ||
                    Neuromotor_Extraneous_Termination || Neuromotor_Extraneous_Control)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Extraneous Movement", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Initial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Sustainance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Termination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Neuromotor_Extraneous_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion

                    document.Add(table);
                    #endregion
                }
                #endregion

                #region
                if (SelectionMotorControl_Muscle)
                {
                    List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_Muscle> DL = new List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_Muscle>();
                    try
                    {
                        DL = JsonConvert.DeserializeObject<List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_Muscle>>(ds.Tables[1].Rows[0]["SelectionMotorControl_Muscle"].ToString().Trim());
                    }
                    catch
                    {
                    }
                    if (DL.Count > 0)
                    {
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 1f });
                        table.SpacingBefore = 20f;

                        cell = PhraseCell(new Phrase("Selective Motor Control :", HeadingFont), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        table.AddCell(cell);
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Muscle", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        for (int i = 0; i < DL.Count; i++)
                        {
                            snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_Muscle _denver = DL[i];
                            cell = new PdfPCell(PhraseCell(new Phrase(_denver.MUSCLE, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(_denver.RIGHT, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(_denver.LEFT, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                        #endregion
                    }
                }
                #endregion

                //#region

                //List<dynamic> _selectionMotorControl_Denvers = null;
                //try
                //{
                //    SelectionMotorControl_Denvers = false;
                //    _selectionMotorControl_Denvers = JsonConvert.DeserializeObject<List<dynamic>>(ds.Tables[1].Rows[0]["SelectionMotorControl_Denvers"].ToString().Trim());
                //    if (_selectionMotorControl_Denvers != null && _selectionMotorControl_Denvers.Count > 0)
                //    {
                //        for (int i = 0; i < _selectionMotorControl_Denvers.Count; i++)
                //        {
                //            dynamic _denver = _selectionMotorControl_Denvers[i];
                //            if (((string)_denver.t).Length > 0)
                //            {
                //                SelectionMotorControl_Denvers = true; break;
                //            }
                //        }
                //    }
                //}
                //catch
                //{
                //}
                //if (SelectionMotorControl_Denvers || SelectionMotorControl_GMFM)
                //{
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.3f, 1f });
                //    table.SpacingBefore = 20f;

                //    cell = PhraseCell(new Phrase("Denvers :", HeadingFont), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    table.AddCell(cell);
                //    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    cell.PaddingBottom = 30f;
                //    table.AddCell(cell);
                //    document.Add(table);

                //    #region
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.29f, 0.71f });
                //    table.WidthPercentage = 100;
                //    table.SpacingBefore = 10f;

                //    if (SelectionMotorControl_Denvers)
                //    {
                //        if (_selectionMotorControl_Denvers != null && _selectionMotorControl_Denvers.Count > 0)
                //        {
                //            for (int i = 0; i < _selectionMotorControl_Denvers.Count; i++)
                //            {
                //                dynamic _denver = _selectionMotorControl_Denvers[i];
                //                if (((string)_denver.n).Equals("gross", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                //                {
                //                    cell = new PdfPCell(PhraseCell(new Phrase("Gross Motor :", NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 60f;
                //                    table.AddCell(cell);

                //                    cell = new PdfPCell(PhraseCell(new Phrase(((string)_denver.t), NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 5f;
                //                    table.AddCell(cell);
                //                }
                //                if (((string)_denver.n).Equals("fine", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                //                {
                //                    cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor :", NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 60f;
                //                    table.AddCell(cell);

                //                    cell = new PdfPCell(PhraseCell(new Phrase(((string)_denver.t), NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 5f;
                //                    table.AddCell(cell);
                //                }
                //                if (((string)_denver.n).Equals("communication", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                //                {
                //                    cell = new PdfPCell(PhraseCell(new Phrase("Communication :", NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 60f;
                //                    table.AddCell(cell);

                //                    cell = new PdfPCell(PhraseCell(new Phrase(((string)_denver.t), NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 5f;
                //                    table.AddCell(cell);
                //                }
                //                if (((string)_denver.n).Equals("cognition", StringComparison.InvariantCultureIgnoreCase) && ((string)_denver.t).Length > 0)
                //                {
                //                    cell = new PdfPCell(PhraseCell(new Phrase("Cognition :", NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 60f;
                //                    table.AddCell(cell);

                //                    cell = new PdfPCell(PhraseCell(new Phrase(((string)_denver.t), NormalFont), PdfPCell.ALIGN_LEFT));
                //                    cell.PaddingBottom = 6f;
                //                    cell.PaddingLeft = 5f;
                //                    table.AddCell(cell);
                //                }
                //            }
                //        }
                //    }
                //    if (SelectionMotorControl_GMFM)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("GMFM score :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelectionMotorControl_GMFM"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    document.Add(table);
                //    #endregion
                //}
                //#endregion

                #region
                if (SelectionMotorControl_MAS || SelectionMotorControl_Observation)
                {
                    List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_MAS> DL = new List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_MAS>();
                    try
                    {
                        DL = JsonConvert.DeserializeObject<List<snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_MAS>>(ds.Tables[1].Rows[0]["SelectionMotorControl_MAS"].ToString().Trim());
                    }
                    catch
                    {
                    }
                    if (DL.Count > 0 || SelectionMotorControl_Observation)
                    {
                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 1f });
                        table.SpacingBefore = 20f;

                        cell = PhraseCell(new Phrase("MAS :", HeadingFont), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        table.AddCell(cell);
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (DL.Count > 0)
                        {
                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.5f, 0.5f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            cell = new PdfPCell(PhraseCell(new Phrase("Muscle", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase("MAS", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            for (int i = 0; i < DL.Count; i++)
                            {
                                snehrehab.SessionRpt.NdtRpt.SelectionMotorControl_MAS _denver = DL[i];
                                cell = new PdfPCell(PhraseCell(new Phrase(_denver.MUSCLE, NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 60f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);

                                cell = new PdfPCell(PhraseCell(new Phrase(_denver.MAS, NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 3f;
                                cell.PaddingLeft = 5f; cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);
                            }
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (SelectionMotorControl_Observation)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("Observational Gait Analysis Scale :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelectionMotorControl_Observation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                }
                #endregion

                //#region
                //if (TheFourA_Arousal || TheFourA_Attention || TheFourA_Affect || TheFourA_Action || TheFourA_StateRegulation)
                //{
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.3f, 1f });
                //    table.SpacingBefore = 20f;

                //    cell = PhraseCell(new Phrase("The four A’s :", HeadingFont), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    table.AddCell(cell);
                //    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    cell.PaddingBottom = 30f;
                //    table.AddCell(cell);
                //    document.Add(table);

                //    #region
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.25f, 0.75f });
                //    table.WidthPercentage = 100;
                //    table.SpacingBefore = 10f;
                //    if (TheFourA_Arousal)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("Arousal :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TheFourA_Arousal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    if (TheFourA_Attention)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("Attention :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TheFourA_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    if (TheFourA_Affect)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("Affect :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TheFourA_Affect"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    if (TheFourA_Action)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("Action :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TheFourA_Action"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    if (TheFourA_StateRegulation)
                //    {
                //        cell = new PdfPCell(PhraseCell(new Phrase("State Regulation  :", NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);

                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TheFourA_StateRegulation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 5f;
                //        table.AddCell(cell);
                //    }
                //    document.Add(table);
                //    #endregion
                //}
                //#endregion
                #endregion

                #region ************** Morphology *********************
                bool Morphology = false;
                if (ds.Tables[1].Rows[0]["Morphology_Height"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Weight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbLength"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbLeft"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_LimbRight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Head"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Nipple"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_Waist"].ToString().Trim().Length > 0)
                {
                    Morphology = true;
                }
                bool UpperLimb = false;
                if (ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLevel"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLeft"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowRight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight3"].ToString().Trim().Length > 0)
                {
                    UpperLimb = true;
                }
                bool LowerLimb = false;
                if (ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLevel"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLeft"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeRight"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight3"].ToString().Trim().Length > 0)
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Girth Upper Limb :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Description", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Level", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        //cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        //cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Above Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 3;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Above_ElbowRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_ABV"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("At Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLevel"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_At_ElbowRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Below Elbow", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 3;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthUpperLimb_Below_ElbowRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_UpperLimbGirthLeft_BLW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Girth Lower Limb :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Description", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        // cell.Rowspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Level ", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        // cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        // cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Right", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase("Left", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Above Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 3;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Above_KneeRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);


                        /**/

                        cell = new PdfPCell(PhraseCell(new Phrase("At Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLevel"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeLeft"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_At_KneeRight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_LowerLimbGirthLeft_AT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        /**/
                        cell = new PdfPCell(PhraseCell(new Phrase("Below Knee", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        cell.Rowspan = 3;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Morphology_GirthLowerLimb_Below_KneeRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                bool Musculoskeletal_Mmt_Ta_Left = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Left"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_Ta_Right = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Right"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_Hamstring_left = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_left"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_Hamstring_Right = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_Right"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_adductors_left = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_left"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_adductors_right = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_right"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_hipFlexor_left = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_left"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_hipFlexor_Right = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_Right"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_biceps_left = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_left"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Mmt_biceps_right = ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_right"].ToString().Trim().Length > 0;
                bool Musculoskeletal_Tardius = false;
                if (
                    Musculoskeletal_Mmt_Ta_Left || Musculoskeletal_Mmt_Ta_Right ||
                    Musculoskeletal_Mmt_Hamstring_left || Musculoskeletal_Mmt_Hamstring_Right ||
                    Musculoskeletal_Mmt_adductors_left || Musculoskeletal_Mmt_adductors_right ||
                    Musculoskeletal_Mmt_hipFlexor_left || Musculoskeletal_Mmt_hipFlexor_Right ||
                    Musculoskeletal_Mmt_biceps_left || Musculoskeletal_Mmt_biceps_right
                )
                {
                    Musculoskeletal_Tardius = true;
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
                if (Musculoskeletal_Rom1 || Musculoskeletal_Rom2 || Musculoskeletal_Strength || Musculoskeletal_Mmt || Musculoskeletal_Tardius)
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
                            cell = new PdfPCell(PhraseCell(new Phrase("lp(In pattern) :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("CC(Consious Control) :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Muscle Endurance :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Skeletal Comments :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                    #region ***** Terdious***
                    if (Musculoskeletal_Tardius)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Tardeius :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        if (Musculoskeletal_Mmt_Ta_Left || Musculoskeletal_Mmt_Ta_Right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("TA", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Ta_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Musculoskeletal_Mmt_Hamstring_left || Musculoskeletal_Mmt_Hamstring_Right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hamstring", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_Hamstring_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }

                        if (Musculoskeletal_Mmt_adductors_left || Musculoskeletal_Mmt_adductors_right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Adductors", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_adductors_right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }

                        if (Musculoskeletal_Mmt_hipFlexor_left || Musculoskeletal_Mmt_hipFlexor_Right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Hip Flexor Angle", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_hipFlexor_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }

                        if (Musculoskeletal_Mmt_biceps_left || Musculoskeletal_Mmt_biceps_right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Biceps", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_left"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Musculoskeletal_Mmt_biceps_right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }



                }
                #endregion
                #endregion

                #region***single system***
                bool SelfRegulation = false; if (ds.Tables[1].Rows[0]["SelfRegulation"].ToString().Trim().Length > 0) { SelfRegulation = true; }
                bool Arousal = false; if (ds.Tables[1].Rows[0]["Arousal"].ToString().Trim().Length > 0) { Arousal = true; }
                bool Attention = false; if (ds.Tables[1].Rows[0]["Attention"].ToString().Trim().Length > 0) { Attention = true; }
                bool Affect = false; if (ds.Tables[1].Rows[0]["Affect"].ToString().Trim().Length > 0) { Affect = true; }
                bool Action = false; if (ds.Tables[1].Rows[0]["Action"].ToString().Trim().Length > 0) { Action = true; }
                bool Cognition = false; if (ds.Tables[1].Rows[0]["Cognition"].ToString().Trim().Length > 0) { Cognition = true; }
                bool GI = false; if (ds.Tables[1].Rows[0]["GI"].ToString().Trim().Length > 0) { GI = true; }
                bool Respiratory = false; if (ds.Tables[1].Rows[0]["Respiratory"].ToString().Trim().Length > 0) { Respiratory = true; }
                bool Cardiovascular = false; if (ds.Tables[1].Rows[0]["Cardiovascular"].ToString().Trim().Length > 0) { Cardiovascular = true; }
                bool SkinIntegumentary = false; if (ds.Tables[1].Rows[0]["SkinIntegumentary"].ToString().Trim().Length > 0) { SkinIntegumentary = true; }
                bool Nutrition = false; if (ds.Tables[1].Rows[0]["Nutrition"].ToString().Trim().Length > 0) { Nutrition = true; }

                if (SelfRegulation || Arousal || Attention || Affect || Action ||
    Cognition || GI || Respiratory || Cardiovascular ||
    SkinIntegumentary || Nutrition)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Single System :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    if (SelfRegulation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Self Regulation:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfRegulation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Arousal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Arousal:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Attention:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Affect)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Affect:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Action)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Action:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Cognition)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Cognition:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cognition"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (GI)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("GI:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GI"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (Respiratory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Respiratory:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Respiratory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Cardiovascular)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Cardiovascular:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiovascular"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (SkinIntegumentary)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Skin Integumentary:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SkinIntegumentary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Nutrition)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Nutrition:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Nutrition"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                }
                #endregion

                #region ****************** Sensory System **************
                bool SensorySystem_Vision = false; if (ds.Tables[1].Rows[0]["SensorySystem_Vision"].ToString().Trim().Length > 0) { SensorySystem_Vision = true; }
                // bool SensorySystem_Somatosensory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Somatosensory"].ToString().Trim().Length > 0) { SensorySystem_Somatosensory = true; }
                bool SensorySystem_Vestibular = false; if (ds.Tables[1].Rows[0]["SensorySystem_Vestibular"].ToString().Trim().Length > 0) { SensorySystem_Vestibular = true; }
                bool SensorySystem_Auditory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Auditory"].ToString().Trim().Length > 0) { SensorySystem_Auditory = true; }
                // bool SensorySystem_Gustatory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString().Trim().Length > 0) { SensorySystem_Gustatory = true; }
                //bool SensoryProfile_Profile = false; if (ds.Tables[1].Rows[0]["SensoryProfile_Profile"].ToString().Trim().Length > 0) { SensoryProfile_Profile = true; }
                bool SensorySystem_Propioceptive = false; if (ds.Tables[1].Rows[0]["SensorySystem_Propioceptive"].ToString().Trim().Length > 0) { SensorySystem_Propioceptive = true; }
                bool SensorySystem_Oromotpor = false; if (ds.Tables[1].Rows[0]["SensorySystem_Oromotpor"].ToString().Trim().Length > 0) { SensorySystem_Oromotpor = true; }
                bool SensorySystem_Tactile = false; if (ds.Tables[1].Rows[0]["SensorySystem_Tactile"].ToString().Trim().Length > 0) { SensorySystem_Tactile = true; }
                bool SensorySystem_Olfactory = false; if (ds.Tables[1].Rows[0]["SensorySystem_Olfactory"].ToString().Trim().Length > 0) { SensorySystem_Olfactory = true; }

                if (SensorySystem_Vision || SensorySystem_Vestibular || SensorySystem_Auditory || SensorySystem_Propioceptive || SensorySystem_Oromotpor || SensorySystem_Tactile || SensorySystem_Olfactory)
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
                    //if (SensorySystem_Somatosensory)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("Somatosensory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Somatosensory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //}
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
                    //if (SensorySystem_Gustatory)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("Gustatory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //}
                    #endregion

                    #region
                    //if (SensoryProfile_Profile)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("Gustatory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Gustatory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //}
                    #endregion

                    #region
                    if (SensorySystem_Propioceptive)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Propioceptive :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Propioceptive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Oromotpor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Oromotor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Oromotpor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Tactile)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Tactile :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Tactile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (SensorySystem_Olfactory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Olfactory :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SensorySystem_Olfactory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region***********AGES AND STAGES*******************
                bool Overall_Question_2 = false; if (ds.Tables[1].Rows[0]["QUESTIONS"].ToString().Trim().Length > 0) { Overall_Question_2 = true; }
                if (Overall_Question_2)
                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES ", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(1);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;
                    cell = new PdfPCell(PhraseCell(new Phrase("Overall_Question :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f;
                    cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    #region
                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Months", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["MONTHS"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);
                    #endregion



                    table = new PdfPTable(7);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;

                    #region headers
                    cell = new PdfPCell(PhraseCell(new Phrase("SR.NO", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("OVERALL RESPONSES", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                    cell.Colspan = 3;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    cell = new PdfPCell(PhraseCell(new Phrase("YES", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("NO", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    cell = new PdfPCell(PhraseCell(new Phrase("COMMENT", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    #endregion

                    #region
                    DataTable dt = ds.Tables[1] as DataTable;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string[] Ques = dt.Rows[i]["QUESTIONS"].ToString().Split('~');

                        for (int j = 0; j < Ques.Length; j++)
                        {
                            DataRow dr = (ds.Tables[3].AsEnumerable().Where(w => w.Field<int>("QuestionNo").ToString() == Ques[j].Split('$')[0].ToString())).FirstOrDefault();
                            if (dr != null)
                            {

                                if (Ques[j].Split('$')[1].ToString() == "1")
                                {
                                    dr["Yes"] = 1;
                                    dr["No"] = 0;
                                }
                                else if (Ques[j].Split('$')[2].ToString() == "1")

                                {
                                    dr["No"] = 1;
                                    dr["Yes"] = 0;
                                }

                                dr["Comments"] = Ques[j].Split('$')[3].ToString();

                                cell = new PdfPCell(PhraseCell(new Phrase(dr[1].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);

                                cell = new PdfPCell(PhraseCell(new Phrase(dr[0].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                cell.Colspan = 3;
                                cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);


                                string yestxt = dr[2].ToString();
                                if (yestxt == "1")
                                {
                                    yestxt = "Yes";
                                }
                                else
                                {
                                    yestxt = "--";
                                }

                                cell = new PdfPCell(PhraseCell(new Phrase(yestxt.ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);

                                string Notxt = dr[3].ToString();
                                if (Notxt == "1")
                                {
                                    Notxt = "No";
                                }
                                else
                                {
                                    Notxt = "--";
                                }

                                cell = new PdfPCell(PhraseCell(new Phrase(Notxt.ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);

                                cell = new PdfPCell(PhraseCell(new Phrase(dr[4].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);

                            }



                        }

                    }
                    #endregion
                    document.Add(table);
                }


                bool score_Communication_2 = false; if (ds.Tables[1].Rows[0]["score_Communication_2"].ToString().Trim().Length > 0) { score_Communication_2 = true; }
                bool Inter_Communication_2 = false; if (ds.Tables[1].Rows[0]["Inter_Communication_2"].ToString().Trim().Length > 0) { Inter_Communication_2 = true; }
                bool GROSS_2 = false; if (ds.Tables[1].Rows[0]["GROSS_2"].ToString().Trim().Length > 0) { GROSS_2 = true; }
                bool inter_Gross_2 = false; if (ds.Tables[1].Rows[0]["inter_Gross_2"].ToString().Trim().Length > 0) { inter_Gross_2 = true; }
                bool FINE_2 = false; if (ds.Tables[1].Rows[0]["FINE_2"].ToString().Trim().Length > 0) { FINE_2 = true; }
                bool inter_FINE_2 = false; if (ds.Tables[1].Rows[0]["inter_FINE_2"].ToString().Trim().Length > 0) { inter_FINE_2 = true; }
                bool PROBLEM_2 = false; if (ds.Tables[1].Rows[0]["PROBLEM_2"].ToString().Trim().Length > 0) { PROBLEM_2 = true; }
                bool inter_PROBLEM_2 = false; if (ds.Tables[1].Rows[0]["inter_PROBLEM_2"].ToString().Trim().Length > 0) { inter_PROBLEM_2 = true; }
                bool PERSONAL_2 = false; if (ds.Tables[1].Rows[0]["PERSONAL_2"].ToString().Trim().Length > 0) { PERSONAL_2 = true; }
                bool inter_PERSONAL_2 = false; if (ds.Tables[1].Rows[0]["PERSONAL_2"].ToString().Trim().Length > 0) { PERSONAL_2 = true; }




                if (score_Communication_2 || Inter_Communication_2 || GROSS_2 || inter_Gross_2 || FINE_2 || inter_FINE_2 || PROBLEM_2 || inter_PROBLEM_2 || PERSONAL_2 || inter_PERSONAL_2)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 2 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("1 month 0 days through 2 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);



                    if (score_Communication_2 || Inter_Communication_2)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("22.7", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["score_Communication_2"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Inter_Communication_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                    }

                    if (GROSS_2 || inter_Gross_2)
                    {


                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("41.84", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_2"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_Gross_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (FINE_2 || inter_FINE_2)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.16", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_2"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_FINE_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                    }

                    if (PROBLEM_2 || inter_PROBLEM_2)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("24.62", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_2"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_PROBLEM_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (PERSONAL_2 || inter_PERSONAL_2)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.71", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_2"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_PROBLEM_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                    }
                    document.Add(table);


                }


                bool Comm_3 = false; if (ds.Tables[1].Rows[0]["Comm_3"].ToString().Trim().Length > 0) { Comm_3 = true; }
                bool inter_3 = false; if (ds.Tables[1].Rows[0]["inter_3"].ToString().Trim().Length > 0) { inter_3 = true; }
                bool GROSS_3 = false; if (ds.Tables[1].Rows[0]["GROSS_3"].ToString().Trim().Length > 0) { GROSS_3 = true; }
                bool GROSS_inter_3 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_3"].ToString().Trim().Length > 0) { GROSS_inter_3 = true; }
                bool FINE_3 = false; if (ds.Tables[1].Rows[0]["FINE_3"].ToString().Trim().Length > 0) { FINE_3 = true; }
                bool FINE_inter_3 = false; if (ds.Tables[1].Rows[0]["FINE_inter_3"].ToString().Trim().Length > 0) { FINE_inter_3 = true; }
                bool PROBLEM_3 = false; if (ds.Tables[1].Rows[0]["PROBLEM_3"].ToString().Trim().Length > 0) { PROBLEM_3 = true; }
                bool PROBLEM_inter_3 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_3"].ToString().Trim().Length > 0) { PROBLEM_inter_3 = true; }
                bool PERSONAL_3 = false; if (ds.Tables[1].Rows[0]["PERSONAL_3"].ToString().Trim().Length > 0) { PERSONAL_3 = true; }
                bool PERSONAL_inter_3 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_3"].ToString().Trim().Length > 0) { PERSONAL_inter_3 = true; }

                if (Comm_3 || inter_3 || GROSS_3 || GROSS_inter_3 || FINE_3 || FINE_inter_3 || PROBLEM_3 || PROBLEM_inter_3 || PERSONAL_3 || PERSONAL_inter_3)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 4 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("3 months 0 days through 4 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (Comm_3 || inter_3)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comm_3"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_3 || GROSS_inter_3)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("38.41", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_3"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (FINE_3 || FINE_inter_3)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.62", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_3"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (PROBLEM_3 || PROBLEM_inter_3)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.98", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_3"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                    }

                    if (PERSONAL_3 || PERSONAL_inter_3)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.71", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_3"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    document.Add(table);
                }


                bool Communication_6 = false; if (ds.Tables[1].Rows[0]["Communication_6"].ToString().Trim().Length > 0) { Communication_6 = true; }
                bool comm_inter_6 = false; if (ds.Tables[1].Rows[0]["comm_inter_6"].ToString().Trim().Length > 0) { comm_inter_6 = true; }
                bool GROSS_6 = false; if (ds.Tables[1].Rows[0]["GROSS_6"].ToString().Trim().Length > 0) { GROSS_6 = true; }
                bool GROSS_inter_6 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_6"].ToString().Trim().Length > 0) { GROSS_inter_6 = true; }
                bool FINE_6 = false; if (ds.Tables[1].Rows[0]["FINE_6"].ToString().Trim().Length > 0) { FINE_6 = true; }
                bool FINE_inter_6 = false; if (ds.Tables[1].Rows[0]["FINE_inter_6"].ToString().Trim().Length > 0) { FINE_inter_6 = true; }
                bool PROBLEM_6 = false; if (ds.Tables[1].Rows[0]["PROBLEM_6"].ToString().Trim().Length > 0) { PROBLEM_6 = true; }
                bool PROBLEM_inter_6 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_6"].ToString().Trim().Length > 0) { PROBLEM_inter_6 = true; }
                bool PERSONAL_6 = false; if (ds.Tables[1].Rows[0]["PERSONAL_6"].ToString().Trim().Length > 0) { PERSONAL_6 = true; }
                bool PERSONAL_inter_6 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_6"].ToString().Trim().Length > 0) { PERSONAL_inter_6 = true; }

                if (Communication_6 || comm_inter_6 || GROSS_6 || GROSS_inter_6 || FINE_6 || FINE_inter_6 || PROBLEM_6 || PROBLEM_inter_6 || PERSONAL_6 || PERSONAL_inter_6)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 6 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);



                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);



                    if (Communication_6 || comm_inter_6)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Communication_6"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_inter_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (GROSS_6 || GROSS_inter_6)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("38.41", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_6"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (FINE_6 || FINE_inter_6)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.62", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_6"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (PROBLEM_6 || PROBLEM_inter_6)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.98", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_6"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (PERSONAL_6 || PERSONAL_inter_6)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.98", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_6"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    document.Add(table);

                }

                bool comm_7 = false; if (ds.Tables[1].Rows[0]["comm_7"].ToString().Trim().Length > 0) { comm_7 = true; }
                bool inter_7 = false; if (ds.Tables[1].Rows[0]["inter_7"].ToString().Trim().Length > 0) { inter_7 = true; }
                bool GROSS_7 = false; if (ds.Tables[1].Rows[0]["GROSS_7"].ToString().Trim().Length > 0) { GROSS_7 = true; }
                bool GROSS_inter_7 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_7"].ToString().Trim().Length > 0) { GROSS_inter_7 = true; }
                bool FINE_7 = false; if (ds.Tables[1].Rows[0]["FINE_7"].ToString().Trim().Length > 0) { FINE_7 = true; }
                bool FINE_inter_7 = false; if (ds.Tables[1].Rows[0]["FINE_inter_7"].ToString().Trim().Length > 0) { FINE_inter_7 = true; }
                bool PROBLEM_7 = false; if (ds.Tables[1].Rows[0]["PROBLEM_7"].ToString().Trim().Length > 0) { PROBLEM_7 = true; }
                bool PROBLEM_inter_7 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_7"].ToString().Trim().Length > 0) { PROBLEM_inter_7 = true; }
                bool PERSONAL_7 = false; if (ds.Tables[1].Rows[0]["PERSONAL_7"].ToString().Trim().Length > 0) { PERSONAL_7 = true; }
                bool PERSONAL_inter_7 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_7"].ToString().Trim().Length > 0) { PERSONAL_inter_7 = true; }

                if (comm_7 || inter_7 || GROSS_7 || GROSS_inter_7 || FINE_7 || FINE_inter_7 || PROBLEM_7 || PROBLEM_inter_7 || PERSONAL_7 || PERSONAL_inter_7)
                {


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 8 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("7 months 0 days through 8 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_7 || inter_7)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_7"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }

                    if (GROSS_7 || GROSS_inter_7)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.61", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_7"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (FINE_7 || FINE_inter_7)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("40.15", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_7"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (PROBLEM_7 || PROBLEM_inter_7)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.17", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_7"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (PERSONAL_7 || PERSONAL_inter_7)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("35.84", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_7"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    document.Add(table);

                }


                bool comm_9 = false; if (ds.Tables[1].Rows[0]["comm_9"].ToString().Trim().Length > 0) { comm_9 = true; }
                bool inter_9 = false; if (ds.Tables[1].Rows[0]["inter_9"].ToString().Trim().Length > 0) { inter_9 = true; }
                bool GROSS_9 = false; if (ds.Tables[1].Rows[0]["GROSS_9"].ToString().Trim().Length > 0) { GROSS_9 = true; }
                bool GROSS_inter_9 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_9"].ToString().Trim().Length > 0) { GROSS_inter_9 = true; }
                bool FINE_9 = false; if (ds.Tables[1].Rows[0]["FINE_9"].ToString().Trim().Length > 0) { FINE_9 = true; }
                bool FINE_inter_9 = false; if (ds.Tables[1].Rows[0]["FINE_inter_9"].ToString().Trim().Length > 0) { FINE_inter_9 = true; }
                bool PROBLEM_9 = false; if (ds.Tables[1].Rows[0]["PROBLEM_9"].ToString().Trim().Length > 0) { PROBLEM_9 = true; }
                bool PROBLEM_inter_9 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_9"].ToString().Trim().Length > 0) { PROBLEM_inter_9 = true; }
                bool PERSONAL_9 = false; if (ds.Tables[1].Rows[0]["PERSONAL_9"].ToString().Trim().Length > 0) { PERSONAL_9 = true; }
                bool PERSONAL_inter_9 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_9"].ToString().Trim().Length > 0) { PERSONAL_inter_9 = true; }

                if (comm_9 || inter_9 || GROSS_9 || GROSS_inter_9 || FINE_9 || FINE_inter_9 || PROBLEM_9 || PROBLEM_inter_9 || PERSONAL_9 || PERSONAL_inter_9)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 9 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("9 months 0 days through 9 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_9 || inter_9)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_9"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                    }

                    if (GROSS_9 || GROSS_inter_9)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.61", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_9"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_9 || FINE_inter_9)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("40.15", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_9"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_9 || PROBLEM_inter_9)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.17", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_9"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_9 || PERSONAL_inter_9)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("35.84", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_9"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);

                }

                bool comm_10 = false; if (ds.Tables[1].Rows[0]["comm_10"].ToString().Trim().Length > 0) { comm_10 = true; }
                bool inter_10 = false; if (ds.Tables[1].Rows[0]["inter_10"].ToString().Trim().Length > 0) { inter_10 = true; }
                bool GROSS_10 = false; if (ds.Tables[1].Rows[0]["GROSS_10"].ToString().Trim().Length > 0) { GROSS_10 = true; }
                bool GROSS_inter_10 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_10"].ToString().Trim().Length > 0) { GROSS_inter_10 = true; }
                bool FINE_10 = false; if (ds.Tables[1].Rows[0]["FINE_10"].ToString().Trim().Length > 0) { FINE_10 = true; }
                bool FINE_inter_10 = false; if (ds.Tables[1].Rows[0]["FINE_inter_10"].ToString().Trim().Length > 0) { FINE_inter_10 = true; }
                bool PROBLEM_10 = false; if (ds.Tables[1].Rows[0]["PROBLEM_10"].ToString().Trim().Length > 0) { PROBLEM_10 = true; }
                bool PROBLEM_inter_10 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_10"].ToString().Trim().Length > 0) { PROBLEM_inter_10 = true; }
                bool PERSONAL_10 = false; if (ds.Tables[1].Rows[0]["PERSONAL_10"].ToString().Trim().Length > 0) { PERSONAL_10 = true; }
                bool PERSONAL_inter_10 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_10"].ToString().Trim().Length > 0) { PERSONAL_inter_10 = true; }

                if (comm_10 || inter_10 || GROSS_10 || GROSS_inter_10 || FINE_10 || FINE_inter_10 || PROBLEM_10 || PROBLEM_inter_10 || PERSONAL_10 || PERSONAL_inter_10)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 10 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("9 months 0 days through 10 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_10 || inter_10)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("22.87", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_10"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_10 || GROSS_inter_10)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.07", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_10"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_10 || FINE_inter_10)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("37.97", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_10"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_10 || PROBLEM_inter_10)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("32.51", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_10"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_10 || PERSONAL_inter_10)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.25", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_10"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);

                }


                bool comm_11 = false; if (ds.Tables[1].Rows[0]["comm_11"].ToString().Trim().Length > 0) { comm_11 = true; }
                bool inter_11 = false; if (ds.Tables[1].Rows[0]["inter_11"].ToString().Trim().Length > 0) { inter_11 = true; }
                bool GROSS_11 = false; if (ds.Tables[1].Rows[0]["GROSS_11"].ToString().Trim().Length > 0) { GROSS_11 = true; }
                bool GROSS_inter_11 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_11"].ToString().Trim().Length > 0) { GROSS_inter_11 = true; }
                bool FINE_11 = false; if (ds.Tables[1].Rows[0]["FINE_11"].ToString().Trim().Length > 0) { FINE_11 = true; }
                bool FINE_inter_11 = false; if (ds.Tables[1].Rows[0]["FINE_inter_11"].ToString().Trim().Length > 0) { FINE_inter_11 = true; }
                bool PROBLEM_11 = false; if (ds.Tables[1].Rows[0]["PROBLEM_11"].ToString().Trim().Length > 0) { PROBLEM_11 = true; }
                bool PROBLEM_inter_11 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_11"].ToString().Trim().Length > 0) { PROBLEM_inter_11 = true; }
                bool PERSONAL_11 = false; if (ds.Tables[1].Rows[0]["PERSONAL_11"].ToString().Trim().Length > 0) { PERSONAL_11 = true; }
                bool PERSONAL_inter_11 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_11"].ToString().Trim().Length > 0) { PERSONAL_inter_11 = true; }

                if (comm_11 || inter_11 || GROSS_11 || GROSS_inter_11 || FINE_11 || FINE_inter_11 || PROBLEM_11 || PROBLEM_inter_11 || PERSONAL_11 || PERSONAL_inter_11)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 12 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("11 months 0 days through 12 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_11 || inter_11)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("15.64", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_11"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_11 || GROSS_inter_11)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("21.49", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_11"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_11 || FINE_inter_11)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.50", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_11"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_11 || PROBLEM_inter_11)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.32", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_11"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_11 || PERSONAL_inter_11)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("21.73", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_11"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_13 = false; if (ds.Tables[1].Rows[0]["comm_13"].ToString().Trim().Length > 0) { comm_13 = true; }
                bool inter_13 = false; if (ds.Tables[1].Rows[0]["inter_13"].ToString().Trim().Length > 0) { inter_13 = true; }
                bool GROSS_13 = false; if (ds.Tables[1].Rows[0]["GROSS_13"].ToString().Trim().Length > 0) { GROSS_13 = true; }
                bool GROSS_inter_13 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_13"].ToString().Trim().Length > 0) { GROSS_inter_13 = true; }
                bool FINE_13 = false; if (ds.Tables[1].Rows[0]["FINE_13"].ToString().Trim().Length > 0) { FINE_13 = true; }
                bool FINE_inter_13 = false; if (ds.Tables[1].Rows[0]["FINE_inter_13"].ToString().Trim().Length > 0) { FINE_inter_13 = true; }
                bool PROBLEM_13 = false; if (ds.Tables[1].Rows[0]["PROBLEM_13"].ToString().Trim().Length > 0) { PROBLEM_13 = true; }
                bool PROBLEM_inter_13 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_13"].ToString().Trim().Length > 0) { PROBLEM_inter_13 = true; }
                bool PERSONAL_13 = false; if (ds.Tables[1].Rows[0]["PERSONAL_13"].ToString().Trim().Length > 0) { PERSONAL_13 = true; }
                bool PERSONAL_inter_13 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_13"].ToString().Trim().Length > 0) { PERSONAL_inter_13 = true; }

                if (comm_13 || inter_13 || GROSS_13 || GROSS_inter_13 || FINE_13 || FINE_inter_13 || PROBLEM_13 || PROBLEM_inter_13 || PERSONAL_13 || PERSONAL_inter_13)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 14 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("13 months 0 days through 14 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA :", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_13 || inter_13)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("17.40", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_13"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_13 || GROSS_inter_13)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.80", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_13"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_13 || FINE_inter_13)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_13"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_13 || PROBLEM_inter_13)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("22.56", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_13"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_13 || PERSONAL_inter_13)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.18", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_13"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_13"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }

                bool comm_15 = false; if (ds.Tables[1].Rows[0]["comm_15"].ToString().Trim().Length > 0) { comm_15 = true; }
                bool inter_15 = false; if (ds.Tables[1].Rows[0]["inter_15"].ToString().Trim().Length > 0) { inter_15 = true; }
                bool GROSS_15 = false; if (ds.Tables[1].Rows[0]["GROSS_15"].ToString().Trim().Length > 0) { GROSS_15 = true; }
                bool GROSS_inter_15 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_15"].ToString().Trim().Length > 0) { GROSS_inter_15 = true; }
                bool FINE_15 = false; if (ds.Tables[1].Rows[0]["FINE_15"].ToString().Trim().Length > 0) { FINE_15 = true; }
                bool FINE_inter_15 = false; if (ds.Tables[1].Rows[0]["FINE_inter_15"].ToString().Trim().Length > 0) { FINE_inter_15 = true; }
                bool PROBLEM_15 = false; if (ds.Tables[1].Rows[0]["PROBLEM_15"].ToString().Trim().Length > 0) { PROBLEM_15 = true; }
                bool PROBLEM_inter_15 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_15"].ToString().Trim().Length > 0) { PROBLEM_inter_15 = true; }
                bool PERSONAL_15 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_15"].ToString().Trim().Length > 0) { PROBLEM_inter_15 = true; }
                bool PERSONAL_inter_15 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_15"].ToString().Trim().Length > 0) { PERSONAL_inter_15 = true; }

                if (comm_15 || inter_15 || GROSS_15 || GROSS_inter_15 || FINE_15 || FINE_inter_15 || PROBLEM_15 || PROBLEM_inter_15 || PERSONAL_15 || PERSONAL_inter_15)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 16 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("15 months 0 days through 16 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_15 || inter_15)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("17.40", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_15"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_15 || GROSS_inter_15)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.80", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_15"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_15 || FINE_inter_15)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_15"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_15 || PROBLEM_inter_15)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("22.56", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_15"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_15 || PERSONAL_inter_15)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.18", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_15"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_15"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_17 = false; if (ds.Tables[1].Rows[0]["comm_17"].ToString().Trim().Length > 0) { comm_17 = true; }
                bool inter_17 = false; if (ds.Tables[1].Rows[0]["inter_17"].ToString().Trim().Length > 0) { inter_17 = true; }
                bool GROSS_17 = false; if (ds.Tables[1].Rows[0]["GROSS_17"].ToString().Trim().Length > 0) { GROSS_17 = true; }
                bool GROSS_inter_17 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_17"].ToString().Trim().Length > 0) { GROSS_inter_17 = true; }
                bool FINE_17 = false; if (ds.Tables[1].Rows[0]["FINE_17"].ToString().Trim().Length > 0) { FINE_17 = true; }
                bool FINE_inter_17 = false; if (ds.Tables[1].Rows[0]["FINE_inter_17"].ToString().Trim().Length > 0) { FINE_inter_17 = true; }
                bool PROBLEM_17 = false; if (ds.Tables[1].Rows[0]["PROBLEM_17"].ToString().Trim().Length > 0) { PROBLEM_17 = true; }
                bool PROBLEM_inter_17 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_17"].ToString().Trim().Length > 0) { PROBLEM_inter_17 = true; }
                bool PERSONAL_17 = false; if (ds.Tables[1].Rows[0]["PERSONAL_17"].ToString().Trim().Length > 0) { PERSONAL_17 = true; }
                bool PERSONAL_inter_17 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_17"].ToString().Trim().Length > 0) { PERSONAL_inter_17 = true; }

                if (comm_17 || inter_17 || GROSS_17 || GROSS_inter_17 || FINE_17 || FINE_inter_17 || PROBLEM_17 || PROBLEM_inter_17 || PERSONAL_17 || PERSONAL_inter_17)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 18 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("17 months 0 days through 18 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_17 || inter_17)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("17.40", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_17"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_17 || GROSS_inter_17)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.80", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_17"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_17 || FINE_inter_17)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_17"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_17 || PROBLEM_inter_17)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("22.56", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_17"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_17 || PERSONAL_inter_17)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("23.18", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_17"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_17"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_19 = false; if (ds.Tables[1].Rows[0]["comm_19"].ToString().Trim().Length > 0) { comm_19 = true; }
                bool inter_19 = false; if (ds.Tables[1].Rows[0]["inter_19"].ToString().Trim().Length > 0) { inter_19 = true; }
                bool GROSS_19 = false; if (ds.Tables[1].Rows[0]["GROSS_19"].ToString().Trim().Length > 0) { GROSS_19 = true; }
                bool GROSS_inter_19 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_19"].ToString().Trim().Length > 0) { GROSS_inter_19 = true; }
                bool FINE_19 = false; if (ds.Tables[1].Rows[0]["FINE_19"].ToString().Trim().Length > 0) { FINE_19 = true; }
                bool FINE_inter_19 = false; if (ds.Tables[1].Rows[0]["FINE_inter_19"].ToString().Trim().Length > 0) { FINE_inter_19 = true; }
                bool PROBLEM_19 = false; if (ds.Tables[1].Rows[0]["PROBLEM_19"].ToString().Trim().Length > 0) { PROBLEM_19 = true; }
                bool PROBLEM_inter_19 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_19"].ToString().Trim().Length > 0) { PROBLEM_inter_19 = true; }
                bool PERSONAL_19 = false; if (ds.Tables[1].Rows[0]["PERSONAL_19"].ToString().Trim().Length > 0) { PERSONAL_19 = true; }
                bool PERSONAL_inter_19 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_19"].ToString().Trim().Length > 0) { PERSONAL_inter_19 = true; }

                if (comm_19 || inter_19 || GROSS_19 || GROSS_inter_19 || FINE_19 || FINE_inter_19 || PROBLEM_19 || PROBLEM_inter_19 || PERSONAL_19 || PERSONAL_inter_19)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 20 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);



                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("19 months 0 days through 20 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_19 || inter_19)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("20.50", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_19"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_19 || GROSS_inter_19)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("39.89", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_19"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_19 || FINE_inter_19)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.05", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_19"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_19 || PROBLEM_inter_19)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("28.84", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_19"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_19 || PERSONAL_inter_19)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.36", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_19"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_19"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }



                bool comm_21 = false; if (ds.Tables[1].Rows[0]["comm_21"].ToString().Trim().Length > 0) { comm_21 = true; }
                bool inter_21 = false; if (ds.Tables[1].Rows[0]["inter_21"].ToString().Trim().Length > 0) { inter_21 = true; }
                bool GROSS_21 = false; if (ds.Tables[1].Rows[0]["GROSS_21"].ToString().Trim().Length > 0) { GROSS_21 = true; }
                bool GROSS_inter_21 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_21"].ToString().Trim().Length > 0) { GROSS_inter_21 = true; }
                bool FINE_21 = false; if (ds.Tables[1].Rows[0]["FINE_21"].ToString().Trim().Length > 0) { FINE_21 = true; }
                bool FINE_inter_21 = false; if (ds.Tables[1].Rows[0]["FINE_inter_21"].ToString().Trim().Length > 0) { FINE_inter_21 = true; }
                bool PROBLEM_21 = false; if (ds.Tables[1].Rows[0]["PROBLEM_21"].ToString().Trim().Length > 0) { PROBLEM_21 = true; }
                bool PROBLEM_inter_21 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_21"].ToString().Trim().Length > 0) { PROBLEM_inter_21 = true; }
                bool PERSONAL_21 = false; if (ds.Tables[1].Rows[0]["PERSONAL_21"].ToString().Trim().Length > 0) { PERSONAL_21 = true; }
                bool PERSONAL_inter_21 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_21"].ToString().Trim().Length > 0) { PERSONAL_inter_21 = true; }

                if (comm_21 || inter_21 || GROSS_21 || GROSS_inter_21 || FINE_21 || FINE_inter_21 || PROBLEM_21 || PROBLEM_inter_21 || PERSONAL_21 || PERSONAL_inter_21)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 22 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("21 months 0 days through 22months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_21 || inter_21)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("13.04", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_21"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_21 || GROSS_inter_21)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.75", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_21"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_21 || FINE_inter_21)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.61", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_21"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_21 || PROBLEM_inter_21)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.30", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_21"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_21 || PERSONAL_inter_21)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.07", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_21"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_21"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_23 = false; if (ds.Tables[1].Rows[0]["comm_23"].ToString().Trim().Length > 0) { comm_23 = true; }
                bool inter_23 = false; if (ds.Tables[1].Rows[0]["inter_23"].ToString().Trim().Length > 0) { inter_23 = true; }
                bool GROSS_23 = false; if (ds.Tables[1].Rows[0]["GROSS_23"].ToString().Trim().Length > 0) { GROSS_23 = true; }
                bool GROSS_inter_23 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_23"].ToString().Trim().Length > 0) { GROSS_inter_23 = true; }
                bool FINE_23 = false; if (ds.Tables[1].Rows[0]["FINE_23"].ToString().Trim().Length > 0) { FINE_23 = true; }
                bool FINE_inter_23 = false; if (ds.Tables[1].Rows[0]["FINE_inter_23"].ToString().Trim().Length > 0) { FINE_inter_23 = true; }
                bool PROBLEM_23 = false; if (ds.Tables[1].Rows[0]["PROBLEM_23"].ToString().Trim().Length > 0) { PROBLEM_23 = true; }
                bool PROBLEM_inter_23 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_23"].ToString().Trim().Length > 0) { PROBLEM_inter_23 = true; }
                bool PERSONAL_23 = false; if (ds.Tables[1].Rows[0]["PERSONAL_23"].ToString().Trim().Length > 0) { PERSONAL_23 = true; }
                bool PERSONAL_inter_23 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_23"].ToString().Trim().Length > 0) { PERSONAL_inter_23 = true; }

                if (comm_23 || inter_23 || GROSS_23 || GROSS_inter_23 || FINE_23 || FINE_inter_23 || PROBLEM_23 || PROBLEM_inter_23 || PERSONAL_23 || PERSONAL_inter_23)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 24 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("23 months 0 days through 25 months 15 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_23 || inter_23)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.17", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_23"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_23 || GROSS_inter_23)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("38.07", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_23"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_23 || FINE_inter_23)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("35.16", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_23"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_23 || PROBLEM_inter_23)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.78", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_23"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_23 || PERSONAL_inter_23)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("31.54", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_23"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_23"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_25 = false; if (ds.Tables[1].Rows[0]["comm_25"].ToString().Trim().Length > 0) { comm_25 = true; }
                bool inter_25 = false; if (ds.Tables[1].Rows[0]["inter_25"].ToString().Trim().Length > 0) { inter_25 = true; }
                bool GROSS_25 = false; if (ds.Tables[1].Rows[0]["GROSS_25"].ToString().Trim().Length > 0) { GROSS_25 = true; }
                bool GROSS_inter_25 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_25"].ToString().Trim().Length > 0) { GROSS_inter_25 = true; }
                bool FINE_25 = false; if (ds.Tables[1].Rows[0]["FINE_25"].ToString().Trim().Length > 0) { FINE_25 = true; }
                bool FINE_inter_25 = false; if (ds.Tables[1].Rows[0]["FINE_inter_25"].ToString().Trim().Length > 0) { FINE_inter_25 = true; }
                bool PROBLEM_25 = false; if (ds.Tables[1].Rows[0]["PROBLEM_25"].ToString().Trim().Length > 0) { PROBLEM_25 = true; }
                bool PROBLEM_inter_25 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_25"].ToString().Trim().Length > 0) { PROBLEM_inter_25 = true; }
                bool PERSONAL_25 = false; if (ds.Tables[1].Rows[0]["PERSONAL_25"].ToString().Trim().Length > 0) { PERSONAL_25 = true; }
                bool PERSONAL_inter_25 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_25"].ToString().Trim().Length > 0) { PERSONAL_inter_25 = true; }

                if (comm_25 || inter_25 || GROSS_25 || GROSS_inter_25 || FINE_25 || FINE_inter_25 || PROBLEM_25 || PROBLEM_inter_25 || PERSONAL_25 || PERSONAL_inter_25)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 27 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("25 months 16 days through 28 months 15 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_25 || inter_25)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("24.02", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_25"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_25 || GROSS_inter_25)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("28.01", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_25"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_25 || FINE_inter_25)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("18.42", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_25"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_25 || PROBLEM_inter_25)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.62", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_25"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_25 || PERSONAL_inter_25)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.31", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_25"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_25"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_28 = false; if (ds.Tables[1].Rows[0]["comm_28"].ToString().Trim().Length > 0) { comm_28 = true; }
                bool inter_28 = false; if (ds.Tables[1].Rows[0]["inter_28"].ToString().Trim().Length > 0) { inter_28 = true; }
                bool GROSS_28 = false; if (ds.Tables[1].Rows[0]["GROSS_28"].ToString().Trim().Length > 0) { GROSS_28 = true; }
                bool GROSS_inter_28 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_28"].ToString().Trim().Length > 0) { GROSS_inter_28 = true; }
                bool FINE_28 = false; if (ds.Tables[1].Rows[0]["FINE_28"].ToString().Trim().Length > 0) { FINE_28 = true; }
                bool FINE_inter_28 = false; if (ds.Tables[1].Rows[0]["FINE_inter_28"].ToString().Trim().Length > 0) { FINE_inter_28 = true; }
                bool PROBLEM_28 = false; if (ds.Tables[1].Rows[0]["PROBLEM_28"].ToString().Trim().Length > 0) { PROBLEM_28 = true; }
                bool PROBLEM_inter_28 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_28"].ToString().Trim().Length > 0) { PROBLEM_inter_28 = true; }
                bool PERSONAL_28 = false; if (ds.Tables[1].Rows[0]["PERSONAL_28"].ToString().Trim().Length > 0) { PERSONAL_28 = true; }
                bool PERSONAL_inter_28 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_28"].ToString().Trim().Length > 0) { PERSONAL_inter_28 = true; }

                if (comm_28 || inter_28 || GROSS_28 || GROSS_inter_28 || FINE_28 || FINE_inter_28 || PROBLEM_28 || PROBLEM_inter_28 || PERSONAL_28 || PERSONAL_inter_28)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 30 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("28 months 16 days through 31 months 15 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_28 || inter_28)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.30", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_28"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_28 || GROSS_inter_28)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.14", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_28"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_28 || FINE_inter_28)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("19.25", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_28"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_28 || PROBLEM_inter_28)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.08", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_28"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_28 || PERSONAL_inter_28)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.01", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_28"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_28"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_31 = false; if (ds.Tables[1].Rows[0]["comm_31"].ToString().Trim().Length > 0) { comm_31 = true; }
                bool inter_31 = false; if (ds.Tables[1].Rows[0]["inter_31"].ToString().Trim().Length > 0) { inter_31 = true; }
                bool GROSS_31 = false; if (ds.Tables[1].Rows[0]["GROSS_31"].ToString().Trim().Length > 0) { GROSS_31 = true; }
                bool GROSS_inter_31 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_31"].ToString().Trim().Length > 0) { GROSS_inter_31 = true; }
                bool FINE_31 = false; if (ds.Tables[1].Rows[0]["FINE_31"].ToString().Trim().Length > 0) { FINE_31 = true; }
                bool FINE_inter_31 = false; if (ds.Tables[1].Rows[0]["FINE_inter_31"].ToString().Trim().Length > 0) { FINE_inter_31 = true; }
                bool PROBLEM_31 = false; if (ds.Tables[1].Rows[0]["PROBLEM_31"].ToString().Trim().Length > 0) { PROBLEM_31 = true; }
                bool PROBLEM_inter_31 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_31"].ToString().Trim().Length > 0) { PROBLEM_inter_31 = true; }
                bool PERSONAL_31 = false; if (ds.Tables[1].Rows[0]["PERSONAL_31"].ToString().Trim().Length > 0) { PERSONAL_31 = true; }
                bool PERSONAL_inter_31 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_31"].ToString().Trim().Length > 0) { PERSONAL_inter_31 = true; }

                if (comm_31 || inter_31 || GROSS_31 || GROSS_inter_31 || FINE_31 || FINE_inter_31 || PROBLEM_31 || PROBLEM_inter_31 || PERSONAL_31 || PERSONAL_inter_31)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 33 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("31 months 16 days through 34 months 15 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_31 || inter_31)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("25.36", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_31"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_31 || GROSS_inter_31)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("34.80", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_31"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_31 || FINE_inter_31)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("12.28", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_31"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_31 || PROBLEM_inter_31)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("26.92", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_31"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_31 || PERSONAL_inter_31)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("28.96", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_31"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_31"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_34 = false; if (ds.Tables[1].Rows[0]["comm_34"].ToString().Trim().Length > 0) { comm_34 = true; }
                bool inter_34 = false; if (ds.Tables[1].Rows[0]["inter_34"].ToString().Trim().Length > 0) { inter_34 = true; }
                bool GROSS_34 = false; if (ds.Tables[1].Rows[0]["GROSS_34"].ToString().Trim().Length > 0) { GROSS_34 = true; }
                bool GROSS_inter_34 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_34"].ToString().Trim().Length > 0) { GROSS_inter_34 = true; }
                bool FINE_34 = false; if (ds.Tables[1].Rows[0]["FINE_34"].ToString().Trim().Length > 0) { FINE_34 = true; }
                bool FINE_inter_34 = false; if (ds.Tables[1].Rows[0]["FINE_inter_34"].ToString().Trim().Length > 0) { FINE_inter_34 = true; }
                bool PROBLEM_34 = false; if (ds.Tables[1].Rows[0]["PROBLEM_34"].ToString().Trim().Length > 0) { PROBLEM_34 = true; }
                bool PROBLEM_inter_34 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_34"].ToString().Trim().Length > 0) { PROBLEM_inter_34 = true; }
                bool PERSONAL_34 = false; if (ds.Tables[1].Rows[0]["PERSONAL_34"].ToString().Trim().Length > 0) { PERSONAL_34 = true; }
                bool PERSONAL_inter_34 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_34"].ToString().Trim().Length > 0) { PERSONAL_inter_34 = true; }

                if (comm_34 || inter_34 || GROSS_34 || GROSS_inter_34 || FINE_34 || FINE_inter_34 || PROBLEM_34 || PROBLEM_inter_34 || PERSONAL_34 || PERSONAL_inter_34)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 36 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("34 months 16 days to 38 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_34 || inter_34)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.99", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_34"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_34 || GROSS_inter_34)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.99", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_34"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_34 || FINE_inter_34)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("18.07", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_34"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_34 || PROBLEM_inter_34)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.29", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_34"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_34 || PERSONAL_inter_34)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("35.33", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_34"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_34"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_42 = false; if (ds.Tables[1].Rows[0]["comm_42"].ToString().Trim().Length > 0) { comm_42 = true; }
                bool inter_42 = false; if (ds.Tables[1].Rows[0]["inter_42"].ToString().Trim().Length > 0) { inter_42 = true; }
                bool GROSS_42 = false; if (ds.Tables[1].Rows[0]["GROSS_42"].ToString().Trim().Length > 0) { GROSS_42 = true; }
                bool GROSS_inter_42 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_42"].ToString().Trim().Length > 0) { GROSS_inter_42 = true; }
                bool FINE_42 = false; if (ds.Tables[1].Rows[0]["FINE_42"].ToString().Trim().Length > 0) { FINE_42 = true; }
                bool FINE_inter_42 = false; if (ds.Tables[1].Rows[0]["FINE_inter_42"].ToString().Trim().Length > 0) { FINE_inter_42 = true; }
                bool PROBLEM_42 = false; if (ds.Tables[1].Rows[0]["PROBLEM_42"].ToString().Trim().Length > 0) { PROBLEM_42 = true; }
                bool PROBLEM_inter_42 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_42"].ToString().Trim().Length > 0) { PROBLEM_inter_42 = true; }
                bool PERSONAL_42 = false; if (ds.Tables[1].Rows[0]["PERSONAL_42"].ToString().Trim().Length > 0) { PERSONAL_42 = true; }
                bool PERSONAL_inter_42 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_42"].ToString().Trim().Length > 0) { PERSONAL_inter_42 = true; }

                if (comm_42 || inter_42 || GROSS_42 || GROSS_inter_42 || FINE_42 || FINE_inter_42 || PROBLEM_42 || PROBLEM_inter_42 || PERSONAL_42 || PERSONAL_inter_42)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 42 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("39 months 0 days to 44 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_42 || inter_42)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("27.06", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_42"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_42"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_42 || GROSS_inter_42)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("36.27", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_42"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_42"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_42 || FINE_inter_42)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("19.82", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_42"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_42"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_42 || PROBLEM_inter_42)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("28.11", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_42"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_42"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_42 || PERSONAL_inter_42)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("31.12", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_42"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_42"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_45 = false; if (ds.Tables[1].Rows[0]["comm_45"].ToString().Trim().Length > 0) { comm_45 = true; }
                bool inter_45 = false; if (ds.Tables[1].Rows[0]["inter_45"].ToString().Trim().Length > 0) { inter_45 = true; }
                bool GROSS_45 = false; if (ds.Tables[1].Rows[0]["GROSS_45"].ToString().Trim().Length > 0) { GROSS_45 = true; }
                bool GROSS_inter_45 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_45"].ToString().Trim().Length > 0) { GROSS_inter_45 = true; }
                bool FINE_45 = false; if (ds.Tables[1].Rows[0]["FINE_45"].ToString().Trim().Length > 0) { FINE_45 = true; }
                bool FINE_inter_45 = false; if (ds.Tables[1].Rows[0]["FINE_inter_45"].ToString().Trim().Length > 0) { FINE_inter_45 = true; }
                bool PROBLEM_45 = false; if (ds.Tables[1].Rows[0]["PROBLEM_45"].ToString().Trim().Length > 0) { PROBLEM_45 = true; }
                bool PROBLEM_inter_45 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_45"].ToString().Trim().Length > 0) { PROBLEM_inter_45 = true; }
                bool PERSONAL_45 = false; if (ds.Tables[1].Rows[0]["PERSONAL_45"].ToString().Trim().Length > 0) { PERSONAL_45 = true; }
                bool PERSONAL_inter_45 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_45"].ToString().Trim().Length > 0) { PERSONAL_inter_45 = true; }

                if (comm_45 || inter_45 || GROSS_45 || GROSS_inter_45 || FINE_45 || FINE_inter_45 || PROBLEM_45 || PROBLEM_inter_45 || PERSONAL_45 || PERSONAL_inter_45)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 48 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("45 months 0 days through 50 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_45 || inter_45)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("30.72", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_45"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_45"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_45 || GROSS_inter_45)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("32.78", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_45"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_45"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_45 || FINE_inter_45)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("15.81", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_45"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_45"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_45 || PROBLEM_inter_45)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("31.30", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_45"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_45"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_45 || PERSONAL_inter_45)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("26.60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_45"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_45"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_51 = false; if (ds.Tables[1].Rows[0]["comm_51"].ToString().Trim().Length > 0) { comm_51 = true; }
                bool inter_51 = false; if (ds.Tables[1].Rows[0]["inter_51"].ToString().Trim().Length > 0) { inter_51 = true; }
                bool GROSS_51 = false; if (ds.Tables[1].Rows[0]["GROSS_51"].ToString().Trim().Length > 0) { GROSS_51 = true; }
                bool GROSS_inter_51 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_51"].ToString().Trim().Length > 0) { GROSS_inter_51 = true; }
                bool FINE_51 = false; if (ds.Tables[1].Rows[0]["FINE_51"].ToString().Trim().Length > 0) { FINE_51 = true; }
                bool FINE_inter_51 = false; if (ds.Tables[1].Rows[0]["FINE_inter_51"].ToString().Trim().Length > 0) { FINE_inter_51 = true; }
                bool PROBLEM_51 = false; if (ds.Tables[1].Rows[0]["PROBLEM_51"].ToString().Trim().Length > 0) { PROBLEM_51 = true; }
                bool PROBLEM_inter_51 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_51"].ToString().Trim().Length > 0) { PROBLEM_inter_51 = true; }
                bool PERSONAL_51 = false; if (ds.Tables[1].Rows[0]["PERSONAL_51"].ToString().Trim().Length > 0) { PERSONAL_51 = true; }
                bool PERSONAL_inter_51 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_51"].ToString().Trim().Length > 0) { PERSONAL_inter_51 = true; }

                if (comm_51 || inter_51 || GROSS_51 || GROSS_inter_51 || FINE_51 || FINE_inter_51 || PROBLEM_51 || PROBLEM_inter_51 || PERSONAL_51 || PERSONAL_inter_51)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 54 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("51 months 0 days through 56 months 30 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_51 || inter_51)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("31.85", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_51"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_51"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_51 || GROSS_inter_51)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("35.18", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_51"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_51"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_51 || FINE_inter_51)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("17.32", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_51"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_51"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_51 || PROBLEM_inter_51)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("28.12", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_51"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_51"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_51 || PERSONAL_inter_51)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("32.33", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_51"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_51"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);
                }


                bool comm_60 = false; if (ds.Tables[1].Rows[0]["comm_60"].ToString().Trim().Length > 0) { comm_60 = true; }
                bool inter_60 = false; if (ds.Tables[1].Rows[0]["inter_60"].ToString().Trim().Length > 0) { inter_60 = true; }
                bool GROSS_60 = false; if (ds.Tables[1].Rows[0]["GROSS_60"].ToString().Trim().Length > 0) { GROSS_60 = true; }
                bool GROSS_inter_60 = false; if (ds.Tables[1].Rows[0]["GROSS_inter_60"].ToString().Trim().Length > 0) { GROSS_inter_60 = true; }
                bool FINE_60 = false; if (ds.Tables[1].Rows[0]["FINE_60"].ToString().Trim().Length > 0) { FINE_60 = true; }
                bool FINE_inter_60 = false; if (ds.Tables[1].Rows[0]["FINE_inter_60"].ToString().Trim().Length > 0) { FINE_inter_60 = true; }
                bool PROBLEM_60 = false; if (ds.Tables[1].Rows[0]["PROBLEM_60"].ToString().Trim().Length > 0) { PROBLEM_60 = true; }
                bool PROBLEM_inter_60 = false; if (ds.Tables[1].Rows[0]["PROBLEM_inter_60"].ToString().Trim().Length > 0) { PROBLEM_inter_60 = true; }
                bool PERSONAL_60 = false; if (ds.Tables[1].Rows[0]["PERSONAL_60"].ToString().Trim().Length > 0) { PERSONAL_60 = true; }
                bool PERSONAL_inter_60 = false; if (ds.Tables[1].Rows[0]["PERSONAL_inter_60"].ToString().Trim().Length > 0) { PERSONAL_inter_60 = true; }
                //bool QUESTIONS = false; if (ds.Tables[1].Rows[0]["QUESTIONS"].ToString().Trim().Length > 0) { QUESTIONS = true; }
                if (comm_60 || inter_60 || GROSS_60 || GROSS_inter_60 || FINE_60 || FINE_inter_60 || PROBLEM_60 || PROBLEM_inter_60 || PERSONAL_60 || PERSONAL_inter_60)

                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AGES AND STAGES QUESTIONNAIRE - 60 months", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("57 months 0 days through 66 months 0 days", SubHeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 4;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("AREA", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("CUT-OFF", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    if (comm_60 || inter_60)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Communication", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("33.19", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["comm_60"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["inter_60"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }

                    if (GROSS_60 || GROSS_inter_60)
                    {

                        cell = new PdfPCell(PhraseCell(new Phrase("GROSS MOTOR", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("31.28", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_60"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GROSS_inter_60"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (FINE_60 || FINE_inter_60)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("FINE MOTOR ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("26.54", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_60"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FINE_inter_60"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PROBLEM_60 || PROBLEM_inter_60)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PROBLEM SOLVING", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("29.99", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_60"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PROBLEM_inter_60"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    if (PERSONAL_60 || PERSONAL_inter_60)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("PERSONAL SOCIAL", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("39.07", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_60"].ToString() + "/60", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_inter_60"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    document.Add(table);

                }


                #endregion

                #region *******ABILITY CHECKLIST ****************
                bool ability_QUESTION = false; if (ds.Tables[1].Rows[0]["ABILITY_questions"].ToString().Trim().Length > 0) { ability_QUESTION = true; }
                bool ability_TOTAL = false; if (ds.Tables[1].Rows[0]["ability_TOTAL"].ToString().Trim().Length > 0) { ability_TOTAL = true; }
                bool ability_COMMENTS = false; if (ds.Tables[1].Rows[0]["ability_COMMENTS"].ToString().Trim().Length > 0) { ability_COMMENTS = true; }

                if (ability_QUESTION || ability_TOTAL || ability_COMMENTS)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("ABILITY CHECKLIST :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (ability_QUESTION)
                    {

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Ability_Questions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);


                        #region
                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                        table.SpacingBefore = 20f;

                        table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase("Months", NormalFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                        table.AddCell(PhraseCell(new Phrase(ds.Tables[4].Rows[0]["ABILITY_months"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 3f;
                        table.AddCell(cell);
                        document.Add(table);
                        #endregion

                        table = new PdfPTable(6);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;

                        #region headers

                        ds.Relations.Add(new DataRelation("CategoriesRelation", ds.Tables[5].Columns["CATEGORYID"], ds.Tables[6].Columns["CATEGORYID"]));
                        for (int k = 0; k < ds.Tables[5].Rows.Count; k++)
                        {
                            string categoryID = ds.Tables[5].Rows[k]["CATEGORYID"].ToString();



                            cell = new PdfPCell(PhraseCell(new Phrase("Sr.No", HeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;

                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[5].Rows[k]["CATEGORY_NAME"].ToString().Trim(), HeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                            cell.Colspan = 3;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.Colspan = 3;
                            table.AddCell(cell);


                            cell = new PdfPCell(PhraseCell(new Phrase("Yes", HeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase("No", HeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            #endregion

                            #region


                            DataTable dt = ds.Tables[4] as DataTable;
                            if (dt != null)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string[] Ques1 = dt.Rows[i]["ABILITY_questions"].ToString().Split('~');



                                    for (int j = 0; j < Ques1.Length; j++)
                                    {
                                        string categoryId = Ques1[j].Split('#')[0].ToString();
                                        string questionNo = Ques1[j].Split('#')[1].ToString().Split('$')[0].ToString();
                                        string yes = Ques1[j].Split('#')[1].ToString().Split('$')[1].ToString();
                                        string No = Ques1[j].Split('#')[1].ToString().Split('$')[2].ToString();

                                        DataRow dr = (ds.Tables[6].AsEnumerable().Where(w => w.Field<int>("CategoryID").ToString() == categoryId && w.Field<int>("questionNO").ToString() == questionNo)).FirstOrDefault();

                                        string _catID = dr["CategoryID"].ToString();

                                        if (_catID == categoryID)
                                        {
                                            if (yes == "1")
                                            {
                                                dr["Yes"] = 1;
                                                dr["No"] = 0;
                                            }
                                            else if (No == "1")
                                            {
                                                dr["No"] = 1;
                                                dr["Yes"] = 0;
                                            }

                                            cell = new PdfPCell(PhraseCell(new Phrase(dr[0].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);

                                            cell = new PdfPCell(PhraseCell(new Phrase(dr[1].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                            cell.Colspan = 3;
                                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);

                                            string yestxt = dr[4].ToString();
                                            if (yestxt == "1")
                                            {
                                                yestxt = "Yes";
                                            }
                                            else
                                            {
                                                yestxt = "--";
                                            }

                                            cell = new PdfPCell(PhraseCell(new Phrase(yestxt.ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);


                                            string Notxt = dr[5].ToString();
                                            if (Notxt == "1")
                                            {
                                                Notxt = "No";
                                            }
                                            else
                                            {
                                                Notxt = "--";
                                            }

                                            cell = new PdfPCell(PhraseCell(new Phrase(Notxt.ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                            cell.PaddingBottom = 2f; cell.PaddingLeft = 1f; cell.PaddingRight = 1f; cell.PaddingTop = 2f;
                                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                            table.AddCell(cell);
                                        }
                                    }

                                }

                            }


                            #endregion


                        }

                        document.Add(table);
                    }
                    if (ability_TOTAL)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("TOTAL :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ability_TOTAL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (ability_COMMENTS)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("COMMENTS :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ability_COMMENTS"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                }

                #endregion

                #region**test and measure ***

                // GMFCS Levels
                DataRow row = ds.Tables[1].Rows[0];

                bool GMFCS_I = SafeConvertToBool(row["GMFCS_I"]);
                bool GMFCS_II = SafeConvertToBool(row["GMFCS_II"]);
                bool GMFCS_III = SafeConvertToBool(row["GMFCS_III"]);
                bool GMFCS_IV = SafeConvertToBool(row["GMFCS_IV"]);
                bool GMFCS_V = SafeConvertToBool(row["GMFCS_V"]);
                bool GmfCS = GMFCS_I || GMFCS_II || GMFCS_III || GMFCS_IV || GMFCS_V;

                // MACs Levels
                bool MACs_I = SafeConvertToBool(row["MACs_I"]);
                bool MACs_II = SafeConvertToBool(row["MACs_II"]);
                bool MACs_III = SafeConvertToBool(row["MACs_III"]);
                bool MACs_IV = SafeConvertToBool(row["MACs_IV"]);
                bool MACs_V = SafeConvertToBool(row["MACs_V"]);
                bool MACs = MACs_I || MACs_II || MACs_III || MACs_IV || MACs_V;

                // FMS Levels
                bool FMS_I = SafeConvertToBool(row["FMS_I"]);
                bool FMS_II = SafeConvertToBool(row["FMS_II"]);
                bool FMS_III = SafeConvertToBool(row["FMS_III"]);
                bool FMS_IV = SafeConvertToBool(row["FMS_IV"]);
                bool FMS_V = SafeConvertToBool(row["FMS_V"]);
                bool FMS = FMS_I || FMS_II || FMS_III || FMS_IV || FMS_V;

                // Barry Levels
                bool Barry_I = SafeConvertToBool(row["Barry_I"]);
                bool Barry_II = SafeConvertToBool(row["Barry_II"]);
                bool Barry_III = SafeConvertToBool(row["Barry_III"]);
                bool Barry_IV = SafeConvertToBool(row["Barry_IV"]);
                bool Barry_V = SafeConvertToBool(row["Barry_V"]);
                bool Barry_VI = SafeConvertToBool(row["Barry_VI"]);
                bool Barry = Barry_I || Barry_II || Barry_III || Barry_IV || Barry_V || Barry_VI;



                bool Gmfm_LyingRolling = false;
                if (ds.Tables[1].Rows[0]["Gmfm_LyingRolling"].ToString().Trim().Length > 0) { Gmfm_LyingRolling = true; }

                bool MACs_LyingRolling = false;
                if (ds.Tables[1].Rows[0]["MACs_LyingRolling"].ToString().Trim().Length > 0) { MACs_LyingRolling = true; }

                bool FMS_LyingRolling = false;
                if (ds.Tables[1].Rows[0]["FMS_LyingRolling"].ToString().Trim().Length > 0) { FMS_LyingRolling = true; }

                bool Barry_LyingRolling = false;
                if (ds.Tables[1].Rows[0]["Barry_LyingRolling"].ToString().Trim().Length > 0) { Barry_LyingRolling = true; }

                bool Gmfm_Sitting = false;
                if (ds.Tables[1].Rows[0]["Gmfm_Sitting"].ToString().Trim().Length > 0) { Gmfm_Sitting = true; }

                bool MACs_Sitting = false;
                if (ds.Tables[1].Rows[0]["MACs_Sitting"].ToString().Trim().Length > 0) { MACs_Sitting = true; }

                bool FMS_Sitting = false;
                if (ds.Tables[1].Rows[0]["FMS_Sitting"].ToString().Trim().Length > 0) { FMS_Sitting = true; }

                bool Barry_Sitting = false;
                if (ds.Tables[1].Rows[0]["Barry_Sitting"].ToString().Trim().Length > 0) { Barry_Sitting = true; }

                bool Gmfm_KneelingCrawling = false;
                if (ds.Tables[1].Rows[0]["Gmfm_KneelingCrawling"].ToString().Trim().Length > 0) { Gmfm_KneelingCrawling = true; }

                bool MACs_KneelingCrawling = false;
                if (ds.Tables[1].Rows[0]["MACs_KneelingCrawling"].ToString().Trim().Length > 0) { MACs_KneelingCrawling = true; }

                bool FMS_KneelingCrawling = false;
                if (ds.Tables[1].Rows[0]["FMS_KneelingCrawling"].ToString().Trim().Length > 0) { FMS_KneelingCrawling = true; }

                bool Barry_KneelingCrawling = false;
                if (ds.Tables[1].Rows[0]["Barry_KneelingCrawling"].ToString().Trim().Length > 0) { Barry_KneelingCrawling = true; }

                bool Gmfm_Standing = false;
                if (ds.Tables[1].Rows[0]["Gmfm_Standing"].ToString().Trim().Length > 0) { Gmfm_Standing = true; }

                bool MACs_Standing = false;
                if (ds.Tables[1].Rows[0]["MACs_Standing"].ToString().Trim().Length > 0) { MACs_Standing = true; }

                bool FMS_Standing = false;
                if (ds.Tables[1].Rows[0]["FMS_Standing"].ToString().Trim().Length > 0) { FMS_Standing = true; }

                bool Barry_Standing = false;
                if (ds.Tables[1].Rows[0]["Barry_Standing"].ToString().Trim().Length > 0) { Barry_Standing = true; }

                bool Gmfm_RunningJumping = false;
                if (ds.Tables[1].Rows[0]["Gmfm_RunningJumping"].ToString().Trim().Length > 0) { Gmfm_RunningJumping = true; }

                bool MACs_RunningJumping = false;
                if (ds.Tables[1].Rows[0]["MACs_RunningJumping"].ToString().Trim().Length > 0) { MACs_RunningJumping = true; }

                bool FMS_RunningJumping = false;
                if (ds.Tables[1].Rows[0]["FMS_RunningJumping"].ToString().Trim().Length > 0) { FMS_RunningJumping = true; }

                bool Barry_RunningJumping = false;
                if (ds.Tables[1].Rows[0]["Barry_RunningJumping"].ToString().Trim().Length > 0) { Barry_RunningJumping = true; }

                bool Gmfm_TotalScore = false;
                if (ds.Tables[1].Rows[0]["txtGmfm_TotalScore"].ToString().Trim().Length > 0) { Gmfm_TotalScore = true; }

                bool MACs_TotalScore = false;
                if (ds.Tables[1].Rows[0]["MACs_TotalScore"].ToString().Trim().Length > 0) { MACs_TotalScore = true; }

                bool FMS_TotalScore = false;
                if (ds.Tables[1].Rows[0]["FMS_TotalScore"].ToString().Trim().Length > 0) { FMS_TotalScore = true; }

                bool Barry_TotalScore = false;
                if (ds.Tables[1].Rows[0]["Barry_TotalScore"].ToString().Trim().Length > 0) { Barry_TotalScore = true; }


                if (Gmfm_LyingRolling || MACs_LyingRolling || FMS_LyingRolling || Barry_LyingRolling ||
                         Gmfm_Sitting || MACs_Sitting || FMS_Sitting || Barry_Sitting ||
                Gmfm_KneelingCrawling || MACs_KneelingCrawling || FMS_KneelingCrawling || Barry_KneelingCrawling ||
                        Gmfm_Standing || MACs_Standing || FMS_Standing || Barry_Standing ||
                  Gmfm_RunningJumping || MACs_RunningJumping || FMS_RunningJumping || Barry_RunningJumping ||
                      Gmfm_TotalScore || MACs_TotalScore || FMS_TotalScore || Barry_TotalScore || GmfCS || MACs || FMS || Barry)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Test and  Measure", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (GmfCS)
                    {

                        List<string> gmfcLevels = new List<string>();

                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["GMFCS_I"]))
                            gmfcLevels.Add("I");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["GMFCS_II"]))
                            gmfcLevels.Add("II");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["GMFCS_III"]))
                            gmfcLevels.Add("III");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["GMFCS_IV"]))
                            gmfcLevels.Add("IV");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["GMFCS_V"]))
                            gmfcLevels.Add("V");
                        string gmfcLevelText = string.Join(", ", gmfcLevels);

                        if (gmfcLevels.Count > 0)
                        {
                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.3f, 0.7f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            cell = new PdfPCell(PhraseCell(new Phrase("GMFCS :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            string gmfcText = string.Join(", ", gmfcLevels);
                            cell = new PdfPCell(PhraseCell(new Phrase(gmfcText, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);


                        }
                        document.Add(table);

                    }

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 20f;

                    cell = new PdfPCell(PhraseCell(new Phrase("Category", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("GMFM", NormalFont), PdfPCell.ALIGN_CENTER));
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    cell.BorderWidthLeft = 0.3f;
                    cell.BorderWidthTop = 0.3f;
                    cell.Padding = 5;
                    table.AddCell(cell);



                    if (Gmfm_LyingRolling)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Lying and Rolling", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gmfm_LyingRolling"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }
                    if (Gmfm_Sitting)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Sitting", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gmfm_Sitting"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                    }
                    if (Gmfm_KneelingCrawling)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Kneeling & Crawling", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gmfm_KneelingCrawling"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);




                    }
                    if (Gmfm_Standing)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Standing", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gmfm_Standing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                    if (Gmfm_RunningJumping)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Running & Jumping", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Gmfm_RunningJumping"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }
                    if (Gmfm_TotalScore)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Total Score", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["txtGmfm_TotalScore"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                    }
                    document.Add(table);
                    if (MACs)
                    {
                        List<string> macsLevels = new List<string>();

                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["MACs_I"]))
                            macsLevels.Add("I");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["MACs_II"]))
                            macsLevels.Add("II");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["MACs_III"]))
                            macsLevels.Add("III");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["MACs_IV"]))
                            macsLevels.Add("IV");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["MACs_V"]))
                            macsLevels.Add("V");

                        if (macsLevels.Count > 0)
                        {
                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.3f, 0.7f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            cell = new PdfPCell(PhraseCell(new Phrase("MACs:", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            string macsText = string.Join(", ", macsLevels);
                            cell = new PdfPCell(PhraseCell(new Phrase(macsText, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);
                            ;
                        }
                        document.Add(table);
                    }
                    if (FMS)
                    {
                        List<string> fmsLevels = new List<string>();

                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["FMS_I"]))
                            fmsLevels.Add("I");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["FMS_II"]))
                            fmsLevels.Add("II");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["FMS_III"]))
                            fmsLevels.Add("III");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["FMS_IV"]))
                            fmsLevels.Add("IV");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["FMS_V"]))
                            fmsLevels.Add("V");

                        if (fmsLevels.Count > 0)
                        {
                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.3f, 0.7f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            cell = new PdfPCell(PhraseCell(new Phrase("FMS :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            string fmsText = string.Join(", ", fmsLevels);
                            cell = new PdfPCell(PhraseCell(new Phrase(fmsText, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);

                        }
                        document.Add(table);
                    }
                    if (Barry)
                    {
                        List<string> barryLevels = new List<string>();

                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_I"]))
                            barryLevels.Add("I");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_II"]))
                            barryLevels.Add("II");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_III"]))
                            barryLevels.Add("III");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_IV"]))
                            barryLevels.Add("IV");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_V"]))
                            barryLevels.Add("V");
                        if (Convert.ToBoolean(ds.Tables[1].Rows[0]["Barry_VI"]))
                            barryLevels.Add("VI");

                        if (barryLevels.Count > 0)
                        {
                            table = new PdfPTable(2);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.SetWidths(new float[] { 0.3f, 0.7f });
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;

                            cell = new PdfPCell(PhraseCell(new Phrase("Barry Albright Dystonia Scale :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);

                            string barryText = string.Join(", ", barryLevels);
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Barry_albright_txt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 5f;
                            table.AddCell(cell);

                        }
                        document.Add(table);
                    }


                }

                #endregion

                #region*************Denvers**************
                //bool TestMeassures_IQ = false; if (ds.Tables[1].Rows[0]["TestMeassures_IQ"].ToString().Trim().Length > 0) { TestMeassures_IQ = true; }
                //bool TestMeassures_DQ = false; if (ds.Tables[1].Rows[0]["TestMeassures_DQ"].ToString().Trim().Length > 0) { TestMeassures_DQ = true; }
                bool TestMeassures_GrossMotor = false; if (ds.Tables[1].Rows[0]["TestMeassures_GrossMotor"].ToString().Trim().Length > 0) { TestMeassures_GrossMotor = true; }
                bool TestMeassures_FineMotor = false; if (ds.Tables[1].Rows[0]["TestMeassures_FineMotor"].ToString().Trim().Length > 0) { TestMeassures_FineMotor = true; }
                bool TestMeassures_DenverLanguage = false; if (ds.Tables[1].Rows[0]["TestMeassures_DenverLanguage"].ToString().Trim().Length > 0) { TestMeassures_DenverLanguage = true; }
                bool TestMeassures_DenverPersonal = false; if (ds.Tables[1].Rows[0]["TestMeassures_DenverPersonal"].ToString().Trim().Length > 0) { TestMeassures_DenverPersonal = true; }

                bool Tests_cmt = false; if (ds.Tables[1].Rows[0]["Tests_cmt"].ToString().Trim().Length > 0) { Tests_cmt = true; }


                if (/*TestMeassures_IQ || TestMeassures_DQ ||*/ TestMeassures_GrossMotor || TestMeassures_FineMotor || TestMeassures_DenverLanguage || TestMeassures_DenverPersonal ||
                     Tests_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("DENVERS:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    if (TestMeassures_GrossMotor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Denver’s checklist Gross motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeassures_GrossMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (TestMeassures_FineMotor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Denver’s checklist Fine motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeassures_FineMotor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (TestMeassures_DenverLanguage)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Denver’s checklist Language :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeassures_DenverLanguage"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (TestMeassures_DenverPersonal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Denver’s checklist Personal & social :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TestMeassures_DenverPersonal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Tests_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("COMMENTS  :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Tests_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        //document.Add(Chunk.NEXTPAGE);
                    }
                }
                #endregion

                #region ***************** SENSORY PROFILE 2*********************
                bool SP1 = false;
                bool General_Processing = false; if (ds.Tables[1].Rows[0]["General_Processing"].ToString().Trim().Length > 0) { General_Processing = true; }
                bool AUDITORY_Processing = false; if (ds.Tables[1].Rows[0]["AUDITORY_Processing"].ToString().Trim().Length > 0) { AUDITORY_Processing = true; }
                bool VISUAL_Processing = false; if (ds.Tables[1].Rows[0]["VISUAL_Processing"].ToString().Trim().Length > 0) { VISUAL_Processing = true; }
                bool TOUCH_Processing = false; if (ds.Tables[1].Rows[0]["TOUCH_Processing"].ToString().Trim().Length > 0) { TOUCH_Processing = true; }
                bool MOVEMENT_Processing = false; if (ds.Tables[1].Rows[0]["MOVEMENT_Processing"].ToString().Trim().Length > 0) { MOVEMENT_Processing = true; }
                bool ORAL_Processing = false; if (ds.Tables[1].Rows[0]["ORAL_Processing"].ToString().Trim().Length > 0) { ORAL_Processing = true; }
                bool Raw_score = false; if (ds.Tables[1].Rows[0]["Raw_score"].ToString().Trim().Length > 0) { Raw_score = true; }
                bool Interpretation = false; if (ds.Tables[1].Rows[0]["Interpretation"].ToString().Trim().Length > 0) { Interpretation = true; }
                bool Comments_1 = false; if (ds.Tables[1].Rows[0]["ORAL_Processing"].ToString().Trim().Length > 0) { Comments_1 = true; }
                if (General_Processing || AUDITORY_Processing || VISUAL_Processing || TOUCH_Processing || MOVEMENT_Processing || ORAL_Processing || Raw_score || Interpretation || Comments_1)
                {
                    SP1 = true;
                }
                bool SP2 = false;
                bool Score_seeking = false; if (ds.Tables[1].Rows[0]["Score_seeking"].ToString().Trim().Length > 0) { Score_seeking = true; }
                bool SEEKING = false; if (ds.Tables[1].Rows[0]["SEEKING"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["SEEKING"].ToString().Trim() != "0") { SEEKING = true; }
                bool Score_Avoiding = false; if (ds.Tables[1].Rows[0]["Score_Avoiding"].ToString().Trim().Length > 0) { Score_Avoiding = true; }
                bool AVOIDING = false; if (ds.Tables[1].Rows[0]["AVOIDING"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["AVOIDING"].ToString().Trim() != "0") { AVOIDING = true; }
                bool Score_sensitivity = false; if (ds.Tables[1].Rows[0]["Score_sensitivity"].ToString().Trim().Length > 0) { Score_sensitivity = true; }
                bool SENSITIVITY_2 = false; if (ds.Tables[1].Rows[0]["SENSITIVITY_2"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["SENSITIVITY_2"].ToString().Trim() != "0") { SENSITIVITY_2 = true; }
                bool Score_Registration = false; if (ds.Tables[1].Rows[0]["Score_Registration"].ToString().Trim().Length > 0) { Score_Registration = true; }
                bool REGISTRATION = false; if (ds.Tables[1].Rows[0]["REGISTRATION"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["REGISTRATION"].ToString().Trim() != "0") { REGISTRATION = true; }
                bool Score_general = false; if (ds.Tables[1].Rows[0]["Score_general"].ToString().Trim().Length > 0) { Score_general = true; }
                bool GENERAL = false; if (ds.Tables[1].Rows[0]["GENERAL"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["GENERAL"].ToString().Trim() != "0") { GENERAL = true; }
                bool Score_Auditory = false; if (ds.Tables[1].Rows[0]["Score_Auditory"].ToString().Trim().Length > 0) { Score_Auditory = true; }
                bool AUDITORY = false; if (ds.Tables[1].Rows[0]["GENERAL"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["AUDITORY"].ToString().Trim() != "0") { AUDITORY = true; }
                bool Score_visual = false; if (ds.Tables[1].Rows[0]["Score_visual"].ToString().Trim().Length > 0) { Score_visual = true; }
                bool VISUAL = false; if (ds.Tables[1].Rows[0]["GENERAL"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["VISUAL"].ToString().Trim() != "0") { VISUAL = true; }
                bool Score_touch = false; if (ds.Tables[1].Rows[0]["Score_touch"].ToString().Trim().Length > 0) { Score_touch = true; }
                bool TOUCH = false; if (ds.Tables[1].Rows[0]["TOUCH"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["TOUCH"].ToString().Trim() != "0") { TOUCH = true; }
                bool Score_movement = false; if (ds.Tables[1].Rows[0]["Score_movement"].ToString().Trim().Length > 0) { Score_movement = true; }
                bool MOVEMENT = false; if (ds.Tables[1].Rows[0]["MOVEMENT"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["MOVEMENT"].ToString().Trim() != "0") { MOVEMENT = true; }
                bool Score_oral = false; if (ds.Tables[1].Rows[0]["Score_oral"].ToString().Trim().Length > 0) { Score_oral = true; }
                bool ORAL = false; if (ds.Tables[1].Rows[0]["ORAL"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["MOVEMENT"].ToString().Trim() != "0") { ORAL = true; }
                bool Score_behavioural = false; if (ds.Tables[1].Rows[0]["Score_behavioural"].ToString().Trim().Length > 0) { Score_behavioural = true; }
                bool BEHAVIORAL = false; if (ds.Tables[1].Rows[0]["BEHAVIORAL"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["BEHAVIORAL"].ToString().Trim() != "0") { BEHAVIORAL = true; }
                bool Comments_2 = false; if (ds.Tables[1].Rows[0]["Comments_2"].ToString().Trim().Length > 0) { Comments_2 = true; }
                if (Score_seeking || SEEKING || Score_Avoiding || AVOIDING || Score_sensitivity || SENSITIVITY_2 || Score_Registration || REGISTRATION || Score_general || GENERAL || Score_Auditory ||
                   AUDITORY || Score_visual || VISUAL || Score_touch || TOUCH || Score_movement || MOVEMENT || Score_oral || ORAL || Score_behavioural || BEHAVIORAL || Comments_2)
                {
                    SP2 = true;
                }


                bool SP3 = false;
                bool SPchild_Seeker = false; if (ds.Tables[1].Rows[0]["SPchild_Seeker"].ToString().Trim().Length > 0) { SPchild_Seeker = true; }
                bool Seeking_Seeker = false; if (ds.Tables[1].Rows[0]["Seeking_Seeker"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Seeking_Seeker"].ToString().Trim() != "0") { Seeking_Seeker = true; }
                bool SPchild_Avoider = false; if (ds.Tables[1].Rows[0]["SPchild_Avoider"].ToString().Trim().Length > 0) { SPchild_Avoider = true; }
                bool Avoiding_Avoider = false; if (ds.Tables[1].Rows[0]["Avoiding_Avoider"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Avoiding_Avoider"].ToString().Trim() != "0") { Avoiding_Avoider = true; }
                bool SPchild_Sensor = false; if (ds.Tables[1].Rows[0]["SPchild_Sensor"].ToString().Trim().Length > 0) { SPchild_Sensor = true; }
                bool Sensitivity_Sensor = false; if (ds.Tables[1].Rows[0]["Sensitivity_Sensor"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensitivity_Sensor"].ToString().Trim() != "0") { Sensitivity_Sensor = true; }
                bool SPchild_Bystander = false; if (ds.Tables[1].Rows[0]["SPchild_Bystander"].ToString().Trim().Length > 0) { SPchild_Bystander = true; }
                bool Registration_Bystander = false; if (ds.Tables[1].Rows[0]["Registration_Bystander"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Registration_Bystander"].ToString().Trim() != "0") { Registration_Bystander = true; }
                bool SPchild_Auditory_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Auditory_3"].ToString().Trim().Length > 0) { SPchild_Auditory_3 = true; }
                bool Auditory_3 = false; if (ds.Tables[1].Rows[0]["Auditory_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Auditory_3"].ToString().Trim() != "0") { Auditory_3 = true; }
                bool SPchild_Visual_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Visual_3"].ToString().Trim().Length > 0) { SPchild_Visual_3 = true; }
                bool Visual_3 = false; if (ds.Tables[1].Rows[0]["Visual_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Visual_3"].ToString().Trim() != "0") { Visual_3 = true; }
                bool SPchild_Touch_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Touch_3"].ToString().Trim().Length > 0) { SPchild_Touch_3 = true; }
                bool Touch_3 = false; if (ds.Tables[1].Rows[0]["Touch_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Touch_3"].ToString().Trim() != "0") { Touch_3 = true; }
                bool SPchild_Movement_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Movement_3"].ToString().Trim().Length > 0) { SPchild_Movement_3 = true; }
                bool Movement_3 = false; if (ds.Tables[1].Rows[0]["Movement_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Movement_3"].ToString().Trim() != "0") { Movement_3 = true; }
                bool SPchild_Body_position = false; if (ds.Tables[1].Rows[0]["SPchild_Body_position"].ToString().Trim().Length > 0) { SPchild_Body_position = true; }
                bool Body_position = false; if (ds.Tables[1].Rows[0]["Body_position"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Body_position"].ToString().Trim() != "0") { Body_position = true; }
                bool SPchild_Oral_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Oral_3"].ToString().Trim().Length > 0) { SPchild_Oral_3 = true; }
                bool Oral_3 = false; if (ds.Tables[1].Rows[0]["Oral_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Oral_3"].ToString().Trim() != "0") { Oral_3 = true; }
                bool SPchild_Conduct_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Conduct_3"].ToString().Trim().Length > 0) { SPchild_Conduct_3 = true; }
                bool Conduct_3 = false; if (ds.Tables[1].Rows[0]["Conduct_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Conduct_3"].ToString().Trim() != "0") { Conduct_3 = true; }
                bool SPchild_Social_emotional = false; if (ds.Tables[1].Rows[0]["SPchild_Social_emotional"].ToString().Trim().Length > 0) { SPchild_Social_emotional = true; }
                bool Social_emotional = false; if (ds.Tables[1].Rows[0]["Social_emotional"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Social_emotional"].ToString().Trim() != "0") { Social_emotional = true; }
                bool SPchild_Attentional_3 = false; if (ds.Tables[1].Rows[0]["SPchild_Attentional_3"].ToString().Trim().Length > 0) { SPchild_Attentional_3 = true; }
                bool Attentional_3 = false; if (ds.Tables[1].Rows[0]["Attentional_3"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Attentional_3"].ToString().Trim() != "0") { Attentional_3 = true; }
                bool Comments_3 = false; if (ds.Tables[1].Rows[0]["Comments_3"].ToString().Trim().Length > 0) { Comments_3 = true; }

                if (SPchild_Seeker || Seeking_Seeker || SPchild_Avoider || Avoiding_Avoider || SPchild_Sensor || Sensitivity_Sensor || SPchild_Bystander || Registration_Bystander || SPchild_Auditory_3 ||
                   Auditory_3 || SPchild_Visual_3 || Visual_3 || SPchild_Touch_3 || Touch_3 || SPchild_Movement_3 || Movement_3 || SPchild_Body_position || Body_position || SPchild_Oral_3 || Oral_3 ||
                   SPchild_Conduct_3 || Conduct_3 || SPchild_Social_emotional || Social_emotional || SPchild_Attentional_3 || Attentional_3 || Comments_3)
                {
                    SP3 = true;
                }


                bool SP4 = false;
                bool SPAdult_Low_Registration = false; if (ds.Tables[1].Rows[0]["SPAdult_Low_Registration"].ToString().Trim().Length > 0) { SPAdult_Low_Registration = true; }
                bool Low_Registration = false; if (ds.Tables[1].Rows[0]["Low_Registration"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Low_Registration"].ToString().Trim() != "0") { Low_Registration = true; }
                bool SPAdult_Sensory_seeking = false; if (ds.Tables[1].Rows[0]["SPAdult_Sensory_seeking"].ToString().Trim().Length > 0) { SPAdult_Sensory_seeking = true; }
                bool Sensory_seeking = false; if (ds.Tables[1].Rows[0]["Sensory_seeking"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_seeking"].ToString().Trim() != "0") { Sensory_seeking = true; }
                bool SPAdult_Sensory_Sensitivity = false; if (ds.Tables[1].Rows[0]["SPAdult_Sensory_Sensitivity"].ToString().Trim().Length > 0) { SPAdult_Sensory_Sensitivity = true; }
                bool Sensory_Sensitivity = false; if (ds.Tables[1].Rows[0]["Sensory_Sensitivity"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Sensitivity"].ToString().Trim() != "0") { Sensory_Sensitivity = true; }
                bool SPAdult_Sensory_Avoiding = false; if (ds.Tables[1].Rows[0]["SPAdult_Sensory_Avoiding"].ToString().Trim().Length > 0) { SPAdult_Sensory_Avoiding = true; }
                bool Sensory_Avoiding = false; if (ds.Tables[1].Rows[0]["Sensory_Avoiding"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Avoiding"].ToString().Trim() != "0") { Sensory_Avoiding = true; }
                bool Comments_4 = false; if (ds.Tables[1].Rows[0]["Comments_4"].ToString().Trim().Length > 0) { Comments_4 = true; }
                if (SPAdult_Low_Registration || Low_Registration || SPAdult_Sensory_seeking || Sensory_seeking || SPAdult_Sensory_Sensitivity || Sensory_Sensitivity || SPAdult_Sensory_Avoiding || Sensory_Avoiding || Comments_4)
                {
                    SP4 = true;
                }


                bool SP5 = false;
                bool SP_Low_Registration64 = false; if (ds.Tables[1].Rows[0]["SP_Low_Registration64"].ToString().Trim().Length > 0) { SP_Low_Registration64 = true; }
                bool Low_Registration_5 = false; if (ds.Tables[1].Rows[0]["Low_Registration_5"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Low_Registration_5"].ToString().Trim() != "0") { Low_Registration_5 = true; }
                bool SP_Sensory_seeking_64 = false; if (ds.Tables[1].Rows[0]["SP_Sensory_seeking_64"].ToString().Trim().Length > 0) { SP_Sensory_seeking_64 = true; }
                bool Sensory_seeking_5 = false; if (ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString().Trim() != "0") { Sensory_seeking_5 = true; }
                bool SP_Sensory_Sensitivity64 = false; if (ds.Tables[1].Rows[0]["SP_Sensory_Sensitivity64"].ToString().Trim().Length > 0) { SP_Sensory_Sensitivity64 = true; }
                bool Sensory_Sensitivity_5 = false; if (ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString().Trim() != "0") { Sensory_Sensitivity_5 = true; }
                bool SP_Sensory_Avoiding64 = false; if (ds.Tables[1].Rows[0]["SP_Sensory_Avoiding64"].ToString().Trim().Length > 0) { SP_Sensory_Avoiding64 = true; }
                bool Sensory_Avoiding_5 = false; if (ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString().Trim() != "0") { Sensory_Avoiding_5 = true; }
                bool Comments_5 = false; if (ds.Tables[1].Rows[0]["Comments_5"].ToString().Trim().Length > 0) { Comments_5 = true; }
                if (SP_Low_Registration64 || Low_Registration_5 || SP_Sensory_seeking_64 || Sensory_seeking_5 || SP_Sensory_Sensitivity64 || Sensory_Sensitivity_5 ||
                   SP_Sensory_Avoiding64 || Sensory_Avoiding_5 || Comments_5)
                {
                    SP5 = true;
                }
                bool SP6 = false;
                bool Older_Low_Registration = false; if (ds.Tables[1].Rows[0]["Older_Low_Registration"].ToString().Trim().Length > 0) { Older_Low_Registration = true; }
                bool Low_Registration_6 = false; if (ds.Tables[1].Rows[0]["Low_Registration_6"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Low_Registration_6"].ToString().Trim() != "0") { Low_Registration_6 = true; }
                bool Older_Sensory_seeking = false; if (ds.Tables[1].Rows[0]["Older_Sensory_seeking"].ToString().Trim().Length > 0) { Older_Sensory_seeking = true; }
                bool Sensory_seeking_6 = false; if (ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString().Trim() != "0") { Sensory_seeking_6 = true; }
                bool Older_Sensory_Sensitivity = false; if (ds.Tables[1].Rows[0]["Older_Sensory_Sensitivity"].ToString().Trim().Length > 0) { Older_Sensory_Sensitivity = true; }
                bool Sensory_Sensitivity_6 = false; if (ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString().Trim() != "0") { Sensory_Sensitivity_6 = true; }
                bool Older_Sensory_Avoiding = false; if (ds.Tables[1].Rows[0]["Older_Sensory_Avoiding"].ToString().Trim().Length > 0) { Older_Sensory_Avoiding = true; }
                bool Sensory_Avoiding_6 = false; if (ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString().Trim() != "0") { Sensory_Avoiding_6 = true; }
                bool Comments_6 = false; if (ds.Tables[1].Rows[0]["Comments_6"].ToString().Trim().Length > 0) { Comments_6 = true; }
                if (Older_Low_Registration || Low_Registration_6 || Older_Sensory_seeking || Sensory_seeking_6 || Older_Sensory_Sensitivity ||
                   Sensory_Sensitivity_6 || Older_Sensory_Avoiding || Sensory_Avoiding_6 || Comments_6)
                {
                    SP6 = true;
                }
                if (SP1 || SP2 || SP3 || SP4 || SP5 || SP6)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Sensory Profile-2 :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region********* SP1 *********
                    if (SP1)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Sensory Profile-2 0-6 Months:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("SECTION :", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("RAW SCORE", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (General_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("GENERAL Processing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["General_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AUDITORY_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("AUDITORY  Processing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AUDITORY_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VISUAL_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("VISUAL   Processing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VISUAL_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (TOUCH_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("TOUCH   Processing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TOUCH_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (MOVEMENT_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("MOVEMENT   Processing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["MOVEMENT_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (ORAL_Processing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("ORAL Processing ", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ORAL_Processing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Raw_score)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Infant Sensory Profile 2 Raw Score Total ", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Raw_score"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Comments_1)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }

                        if (Raw_score)
                        {
                            // Add the row with "Infant Sensory Profile 2 Raw Score Total" spanning both columns
                            cell = new PdfPCell(PhraseCell(new Phrase("Infant Total Score", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            cell.Colspan = 2; // Span this cell across both columns
                            table.AddCell(cell);

                            // Add the row with "Raw score Total" and "Interpretation"
                            cell = new PdfPCell(PhraseCell(new Phrase("Raw score Total", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase("Interpretation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            // Add the row with the actual raw score value
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Total_rawscore"].ToString() + " /125", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);


                            // Add the row with the interpretation
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interpretation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            cell.Colspan = 2; // Span this cell across both columns
                            table.AddCell(cell);
                        }


                        document.Add(table);
                    }
                    #endregion
                    #region********* SP2 *********
                    if (SP2)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.SENSORY PROFILE-2 TODDLER :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("QUADRANTS/ SENSORY AND BEHAVIORAL SECTIONS", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("SCORES", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (Score_seeking || SEEKING)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("SEEKING", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_seeking"].ToString() + "/35", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SEEKING"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_Avoiding || AVOIDING)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("AVOIDING", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_Avoiding"].ToString() + "/55", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AVOIDING"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_sensitivity || SENSITIVITY_2)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("SENSITIVITY", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_sensitivity"].ToString() + "/65", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SENSITIVITY_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_Registration || REGISTRATION)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("REGISTRATION", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_Registration"].ToString() + "/55", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["REGISTRATION"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_general || GENERAL)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("GENERAL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_general"].ToString() + "/50", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["GENERAL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_Auditory || AUDITORY)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("AUDITORY", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_Auditory"].ToString() + "/35", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AUDITORY"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_visual || VISUAL)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("VISUAL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_visual"].ToString() + "/30", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VISUAL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_touch || TOUCH)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("TOUCH", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_touch"].ToString() + "/30", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TOUCH"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_movement || MOVEMENT)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("MOVEMENT", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_movement"].ToString() + "/25", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["MOVEMENT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_oral || ORAL)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("ORAL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_oral"].ToString() + "/35", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ORAL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Score_behavioural || BEHAVIORAL)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("BEHAVIORAL", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Score_behavioural"].ToString() + "/30", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BEHAVIORAL"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Comments_2)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            // Add the "Comments_2" cell with its content
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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

                        }
                        document.Add(table);
                    }
                    #endregion
                    #region********SP3*********
                    if (SP3)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.SENSORY PROFILE-2 : CHILD :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("QUADRANTS/ SENSORY AND BEHAVIORAL SECTIONS", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("SCORES", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SPchild_Seeker || Seeking_Seeker)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Seeking/Seeker", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Seeker"].ToString() + "/95", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Seeking_Seeker"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Avoider || Avoiding_Avoider)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Avoiding/Avoider", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Avoider"].ToString() + "/100", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Avoiding_Avoider"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Sensor || Sensitivity_Sensor)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensitivity/Sensor", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Sensor"].ToString() + "/95", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensitivity_Sensor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Bystander || Registration_Bystander)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration/Bystander", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Bystander"].ToString() + "/110", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Registration_Bystander"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Auditory_3 || Auditory_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Auditory_3"].ToString() + "/40", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Auditory_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Visual_3 || Visual_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Visual_3"].ToString() + "/30", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Visual_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Touch_3 || Touch_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Touch", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Touch_3"].ToString() + "/55", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Touch_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Movement_3 || Movement_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Movement", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Movement_3"].ToString() + "/40", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Movement_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Body_position || Body_position)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Body position", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Body_position"].ToString() + "/40", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Body_position"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Oral_3 || Oral_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Oral", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Oral_3"].ToString() + "/50", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Oral_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Conduct_3 || Conduct_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Conduct", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Conduct_3"].ToString() + "/45", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Conduct_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Social_emotional || Social_emotional)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Social emotional", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Social_emotional"].ToString() + "/70", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Social_emotional"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPchild_Attentional_3 || Attentional_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Attentional", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Attentional_3"].ToString() + "/50", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attentional_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);


                        }
                        if (Comments_3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region********SP4*********
                    if (SP4)
                    {

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Sensory Profile 2 - Adolescent and Adult:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("QUADRANTS", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("RAW SCORES", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SPAdult_Low_Registration || Low_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(" Low Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPAdult_Low_Registration"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Low_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPAdult_Sensory_seeking || Sensory_seeking)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory seeking", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPAdult_Sensory_seeking"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_seeking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPAdult_Sensory_Sensitivity || Sensory_Sensitivity)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Sensitivity", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPAdult_Sensory_seeking"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Sensitivity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SPAdult_Sensory_Avoiding || Sensory_Avoiding)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Avoiding", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPAdult_Sensory_Avoiding"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Avoiding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Comments_4)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                        }
                        document.Add(table);

                    }
                    #endregion
                    #region********SP5*********
                    if (SP5)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Quadrant Summary chart for the ages 16-64", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("QUADRANTS", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("RAW SCORES", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (SP_Low_Registration64 || Low_Registration_5)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Low Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SP_Low_Registration64"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Low_Registration_5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SP_Sensory_seeking_64 || Sensory_seeking_5)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory seeking", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SP_Sensory_seeking_64"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SP_Sensory_Sensitivity64 || Sensory_Sensitivity_5)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Sensitivity", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SP_Sensory_Sensitivity64"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SP_Sensory_Avoiding64 || Sensory_Avoiding_5)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Avoiding", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SP_Sensory_Avoiding64"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Comments_5)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                        }
                        document.Add(table);

                    }
                    #endregion
                    #region*******SP6*********
                    if (SP6)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Quadrant Summary chart for the ages 65 and older", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("QUADRANTS", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("RAW SCORES", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (Older_Low_Registration || Low_Registration_6)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Low Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Older_Low_Registration"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Low_Registration_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Older_Sensory_seeking || Sensory_seeking_6)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory seeking", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Older_Sensory_seeking"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Older_Sensory_Sensitivity || Sensory_Sensitivity_6)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Sensitivity", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Older_Sensory_Sensitivity"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Older_Sensory_Avoiding || Sensory_Avoiding_6)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Sensory Avoiding", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Older_Sensory_Avoiding"].ToString() + "/75", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (Comments_6)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Comments", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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


                        }
                        document.Add(table);
                    }

                    #endregion

                    #region *** reference table***
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;

                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("**Referance Table**", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("MLTO", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Much Less Than Others", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("LTO", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Less Than Others", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("JLMO", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Just Like the Majority of Others", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("MTO", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("More than Others", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.1f, 0.3f, 0.1f, 1f });
                    table.SpacingBefore = 20f;

                    table.AddCell(PhraseCell(new Phrase("", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("MMTO", NormalFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase(":", ColonFont), PdfPCell.ALIGN_LEFT));
                    table.AddCell(PhraseCell(new Phrase("Much More Than Others", NormalFont), PdfPCell.ALIGN_LEFT));
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 3f;
                    table.AddCell(cell);
                    document.Add(table);

                    #endregion
                }

                #endregion

                #region ***evaluation**
                bool Evaluation_Goal_Summary_Filled = false; if (ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString().Trim().Length > 0) Evaluation_Goal_Summary_Filled = true;
                bool Evaluation_System_Impairment_Filled = false; if (ds.Tables[1].Rows[0]["Evaluation_System_Impairment"].ToString().Trim().Length > 0) Evaluation_System_Impairment_Filled = true;
                bool Evaluation_LTG_Filled = false; if (ds.Tables[1].Rows[0]["Evaluation_LTG"].ToString().Trim().Length > 0) Evaluation_LTG_Filled = true;

                bool Evaluation_STG_Filled = false; if (ds.Tables[1].Rows[0]["Evaluation_STG"].ToString().Trim().Length > 0) Evaluation_STG_Filled = true;
                bool Evalution_Plan_advice_Filled = false; if (ds.Tables[1].Rows[0]["Evalution_Plan_advice"].ToString().Trim().Length > 0) Evalution_Plan_advice_Filled = true;
                bool Evalution_Plan_Frequency_Filled = false; if (ds.Tables[1].Rows[0]["Evalution_Plan__Frequency"].ToString().Trim().Length > 0) Evalution_Plan_Frequency_Filled = true;
                bool Evalution_Plan_Adjuncts_Filled = false; if (ds.Tables[1].Rows[0]["Evalution_Plan_Adjuncts"].ToString().Trim().Length > 0) Evalution_Plan_Adjuncts_Filled = true;
                bool Evalution_Plan_Education_Filled = false; if (ds.Tables[1].Rows[0]["Evalution_Plan__Education"].ToString().Trim().Length > 0) Evalution_Plan_Education_Filled = true;

                if (Evaluation_Goal_Summary_Filled || Evaluation_System_Impairment_Filled || Evaluation_LTG_Filled ||
                       Evaluation_STG_Filled || Evalution_Plan_advice_Filled || Evalution_Plan_Frequency_Filled ||
                       Evalution_Plan_Adjuncts_Filled || Evalution_Plan_Education_Filled)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Evaluation:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    if (Evaluation_Goal_Summary_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1. Summary :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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

                    if (Evaluation_System_Impairment_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2. Systems of Impairments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_System_Impairment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evaluation_LTG_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3. LTG :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_LTG"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evaluation_STG_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4. STG :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evaluation_STG"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evalution_Plan_advice_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5. Plan of Care - Advice :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evalution_Plan_advice"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evalution_Plan_Frequency_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6. Frequency and Duration :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evalution_Plan__Frequency"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evalution_Plan_Adjuncts_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7. Therapy Adjuncts :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evalution_Plan_Adjuncts"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                    if (Evalution_Plan_Education_Filled)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8. Family Education :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Evalution_Plan__Education"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }

                }






                #endregion

                #region ****************** END OF PRINT CONTENT *********************
                int _Doctor_Physioptherapist = 0; string Doctor_Physioptherapist = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Physioptherapist"].ToString(), out _Doctor_Physioptherapist);
                DMD = DMB.Get(_Doctor_Physioptherapist); if (DMD != null) { Doctor_Physioptherapist = DMD.PreFix + " " + DMD.FullName; }

                int _Doctor_Occupational = 0; string Doctor_Occupational = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Occupational"].ToString(), out _Doctor_Occupational);
                DMD = DMB.Get(_Doctor_Occupational); if (DMD != null) { Doctor_Occupational = DMD.PreFix + " " + DMD.FullName; }

                //int _Doctor_EnterReport = 0; string Doctor_Director = ""; int.TryParse(ds.Tables[1].Rows[0]["Doctor_EnterReport"].ToString(), out _Doctor_EnterReport);
                //DMD = DMB.Get(_Doctor_EnterReport); if (DMD != null) { Doctor_Director = DMD.PreFix + " " + DMD.FullName; }

                table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 33.33f, 33.33f, 33.33f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3; cell.BorderColorTop = BaseColor.GRAY; cell.BorderWidthTop = 0.3f;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                //Doctor_Director = "Dr Snehal Deshpande";
                //table.AddCell(PhraseCell(new Phrase(Doctor_Director, NormalBold), PdfPCell.ALIGN_CENTER));
                //if (Doctor_Director.Length > 0)
                //    table.AddCell(PhraseCell(new Phrase(Doctor_Director, NextHeadingFont), PdfPCell.ALIGN_CENTER));
                //else
                //    table.AddCell(PhraseCell(new Phrase("", NextHeadingFont), PdfPCell.ALIGN_CENTER));
                if (Doctor_Physioptherapist.Length > 0)
                    table.AddCell(PhraseCell(new Phrase(Doctor_Physioptherapist, NormalBold), PdfPCell.ALIGN_CENTER));
                else
                    table.AddCell(PhraseCell(new Phrase("", NormalBold), PdfPCell.ALIGN_CENTER));
                if (Doctor_Occupational.Length > 0)
                    table.AddCell(PhraseCell(new Phrase(Doctor_Occupational, NormalBold), PdfPCell.ALIGN_CENTER));
                else
                    table.AddCell(PhraseCell(new Phrase("", NormalBold), PdfPCell.ALIGN_CENTER));

                //if (Doctor_Director.Length > 0)
                //{
                //    //cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                //    cell = ImageCell("~/images/rpt-logo.png", 20f, PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 3;
                //    //cell.FixedHeight = 5f;
                //    cell.PaddingBottom = 0f; cell.PaddingTop = 10f;

                //    table.AddCell(cell);
                //}
                if (Doctor_Physioptherapist.Length > 0)
                {
                    cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    //cell.FixedHeight=5f;
                    cell.PaddingBottom = 10f; cell.PaddingTop = 10f;
                    table.AddCell(cell);
                }
                if (Doctor_Occupational.Length > 0)
                {
                    cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    //cell.FixedHeight = 5f;
                    cell.PaddingBottom = 10f; cell.PaddingTop = 10f;
                    table.AddCell(cell);
                }

                //table.AddCell(PhraseCell(new Phrase("Founder & Director- Sneh...RERC" + '\n' + "PT; MIAP:C/NDT;PGDHHM" + '\n' + "SI (Certified by USC/WPS)" + '\n' + "Reg No 1884", NormalBold), PdfPCell.ALIGN_CENTER));
                table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));
                table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));

                //if (Doctor_Director.Length > 0)
                //    table.AddCell(PhraseCell(new Phrase("DIRECTOR SNEH RERC", NormalBold), PdfPCell.ALIGN_CENTER));
                //else
                //    table.AddCell(PhraseCell(new Phrase("DIRECTOR SNEH RERC", NextHeadingFont), PdfPCell.ALIGN_CENTER));
                //if (Doctor_Physioptherapist.Length > 0)
                //    table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));
                //else
                //    table.AddCell(PhraseCell(new Phrase("THERAPIST", NextHeadingFont), PdfPCell.ALIGN_CENTER));
                //if (Doctor_Occupational.Length > 0)
                //    table.AddCell(PhraseCell(new Phrase("THERAPIST", NormalBold), PdfPCell.ALIGN_CENTER));
                //else
                //    table.AddCell(PhraseCell(new Phrase("THERAPIST", NextHeadingFont), PdfPCell.ALIGN_CENTER));

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
                mail = AM.sendAttachment(mailid, "", "NDT Report 2025", result_sheet);
                if (mail)
                {
                    r.status = true; r.msg = "Send successfully.";
                    context.Response.Write(JsonConvert.SerializeObject(r));

                    DbHelper.SqlDb db = new DbHelper.SqlDb();
                    SqlCommand cmd = new SqlCommand("UPDATE RPT_NDT_new_reval SET MailSend=CAST('True'AS BIT),Send_MailDate=GETDATE() WHERE AppointmentID=@AppointmentID");
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
                    int i = db.DbUpdate(cmd);
                    //if (i > 0)
                    //{
                    //    string receivername = string.Empty;
                    //    if (_receivertype == 1)
                    //    {
                    //        receivername = "Patient";
                    //    }
                    //    else if (_receivertype == 2)
                    //    {
                    //        receivername = "Father";
                    //    }
                    //    else if (_receivertype == 3)
                    //    {
                    //        receivername = "Mother";
                    //    }
                    //    else if (_receivertype == 4)
                    //    {
                    //        receivername = "Reference";
                    //    }
                    //    string reportname = string.Empty; reportname = "NDT Report 2025";
                    //    SqlCommand cmd1 = new SqlCommand("Set_ReportMail_ToPatient"); cmd1.CommandType = CommandType.StoredProcedure;
                    //    cmd1.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
                    //    cmd1.Parameters.Add("@ReportName", SqlDbType.VarChar, 250).Value = reportname;
                    //    cmd1.Parameters.Add("@Receiver_Account", SqlDbType.VarChar, 250).Value = receivername;
                    //    cmd1.Parameters.Add("@Receiver_MailID", SqlDbType.VarChar, 50).Value = mailid;
                    //    cmd1.Parameters.Add("@SendBy", SqlDbType.Int).Value = _loginID;
                    //    cmd1.Parameters.Add("@ReportID", SqlDbType.Int).Value = 3;

                    //    SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
                    //    Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
                    //    Param.Value = 0; cmd1.Parameters.Add(Param);

                    //    db.DbUpdate(cmd1);
                    //}
                    return;
                }
                else
                {
                    r.msg = "Unable to process, Please try again.";
                    context.Response.Write(JsonConvert.SerializeObject(r));
                    return;
                }

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class NDTReport
        {
            public string MailID { get; set; }
            public int SiAppointmentID { get; set; }
            public int Receivertype { get; set; }
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
        private bool SafeConvertToBool(object obj)
        {
            return obj != DBNull.Value && Convert.ToBoolean(obj);
        }
    }
}