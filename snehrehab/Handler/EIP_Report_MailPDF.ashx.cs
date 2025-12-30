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
    /// Summary description for EIP_Report_MailPDF
    /// </summary>
    public class EIP_Report_MailPDF : IHttpHandler, IRequiresSessionState
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
            EIPReport evnt = jsonSerializer.Deserialize<EIPReport>(requestBody);
            int.TryParse(evnt.SiAppointmentID.ToString(), out _appointmentID);
            mailid = evnt.MailID;

            if (_appointmentID <= 0 || string.IsNullOrEmpty(mailid))
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            SnehBLL.ReportEIPMst_Bll REP = new SnehBLL.ReportEIPMst_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataSet ds = REP.Get(_appointmentID);
            Document document = new Document(PageSize.A4, 30f, 30f, 50f, 10f);
            Font NormalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            Font SubHeadingFont = FontFactory.GetFont("Arial", 10, Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("EIP Report - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

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
                table.AddCell(PhraseCell(new Phrase("Sesion", NormalFont), PdfPCell.ALIGN_LEFT));
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
                DataTable dtT = REP.GetTherapist(_appointmentID);
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
                bool DataCollection_EDD = false; if (ds.Tables[1].Rows[0]["DataCollection_EDD"].ToString().Trim().Length > 0) { DataCollection_EDD = true; }
                bool DataCollection_CGA = false; if (ds.Tables[1].Rows[0]["DataCollection_CGA"].ToString().Trim().Length > 0) { DataCollection_CGA = true; }

                if (DataCollection_EDD || DataCollection_CGA)
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
                    if (DataCollection_EDD)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("EDD :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_EDD"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        document.Add(Chunk.NEXTPAGE);
                    }
                    #endregion

                    #region
                    if (DataCollection_CGA)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("CGA :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DataCollection_CGA"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    // document.Add(Chunk.NEXTPAGE);
                }
                #endregion

                #region **************Birth History********************

                bool BirthHistory_NC_SecDelivery = false; if (ds.Tables[1].Rows[0]["BirthHistory_NC_SecDelivery"].ToString().Trim().Length > 0) { BirthHistory_NC_SecDelivery = true; }
                bool BirthHistory_PreNatal_MaterialHistory = false; if (ds.Tables[1].Rows[0]["BirthHistory_PreNatal_MaterialHistory"].ToString().Trim().Length > 0) { BirthHistory_PreNatal_MaterialHistory = true; }
                bool BirthHistory_Natal = false; if (ds.Tables[1].Rows[0]["BirthHistory_Natal"].ToString().Trim().Length > 0) { BirthHistory_Natal = true; }
                bool BirthHistory_PostNatal = false; if (ds.Tables[1].Rows[0]["BirthHistory_PostNatal"].ToString().Trim().Length > 0) { BirthHistory_PostNatal = true; }

                if (BirthHistory_NC_SecDelivery || BirthHistory_PreNatal_MaterialHistory || BirthHistory_Natal || BirthHistory_PostNatal)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Birth History :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (BirthHistory_NC_SecDelivery)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" N/C SEC DELIVERY :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHistory_NC_SecDelivery"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (BirthHistory_PreNatal_MaterialHistory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" PRE – NATAL / MATERAIL HISTORY :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHistory_PreNatal_MaterialHistory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (BirthHistory_Natal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" NATAL :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHistory_Natal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (BirthHistory_PostNatal)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" POST NATAL H/O :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHistory_PostNatal"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }

                #endregion

                #region************Observation****************

                bool Observation_Autonomic_HR = false; if (ds.Tables[1].Rows[0]["Observation_Autonomic_HR"].ToString().Trim().Length > 0) { Observation_Autonomic_HR = true; }
                bool Observation_Autonomic_TypesOfRespiration = false; if (ds.Tables[1].Rows[0]["Observation_Autonomic_TypesOfRespiration"].ToString().Trim().Length > 0) { Observation_Autonomic_TypesOfRespiration = true; }
                bool Observation_Autonomic_SkinColour = false; if (ds.Tables[1].Rows[0]["Observation_Autonomic_SkinColour"].ToString().Trim().Length > 0) { Observation_Autonomic_SkinColour = true; }
                bool Observation_Autonomic_TemperatureCentral_Peripheral = false; if (ds.Tables[1].Rows[0]["Observation_Autonomic_TemperatureCentral_Peripheral"].ToString().Trim().Length > 0) { Observation_Autonomic_TemperatureCentral_Peripheral = true; }
                //  bool Observation_Motor = false; if (ds.Tables[1].Rows[0]["Observation_Motor"].ToString().Trim().Length > 0) { Observation_Motor = true; }

                bool Autonomic = false;
                if (ds.Tables[1].Rows[0]["Observation_Autonomic_HR"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_Autonomic_TypesOfRespiration"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_Autonomic_SkinColour"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_Autonomic_TemperatureCentral_Peripheral"].ToString().Trim().Length > 0)
                {
                    Autonomic = true;
                }
                bool Motor = false;
                if (ds.Tables[1].Rows[0]["Observation_Motor"].ToString().Trim().Length > 0)
                {
                    Motor = true;
                }
                bool UpperLimb = false;
                if (ds.Tables[1].Rows[0]["Observation_UpperLimbLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_UpperLimbRight3"].ToString().Trim().Length > 0)
                {
                    UpperLimb = true;
                }
                bool LowerLimb = false;
                if (ds.Tables[1].Rows[0]["Observation_LowerLimbLevel1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbLevel2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbLevel3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbLeft1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbLeft2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbLeft3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbRight1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbRight2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Observation_LowerLimbRight3"].ToString().Trim().Length > 0)
                {
                    LowerLimb = true;
                }
                bool Trunk = false;
                if (ds.Tables[1].Rows[0]["Observation_Trunk"].ToString().Trim().Length > 0)
                {
                    Trunk = true;
                }
                bool Observation_GeneralPosture = false;
                if (ds.Tables[1].Rows[0]["Observation_GeneralPosture"].ToString().Trim().Length > 0)
                {
                    Observation_GeneralPosture = true;
                }
                bool Observation_SocialInteraction_Responsivity = false;
                if (ds.Tables[1].Rows[0]["Observation_SocialInteraction_Responsivity"].ToString().Trim().Length > 0)
                {
                    Observation_SocialInteraction_Responsivity = true;
                }
                bool Observation_Feeding = false;
                if (ds.Tables[1].Rows[0]["Observation_Feeding"].ToString().Trim().Length > 0)
                {
                    Observation_Feeding = true;
                }
                bool Observation_Participation = false;
                if (ds.Tables[1].Rows[0]["Observation_Participation"].ToString().Trim().Length > 0)
                {
                    Observation_Participation = true;
                }
                bool Observation_Participation_Restriction = false;
                if (ds.Tables[1].Rows[0]["Observation_Participation_Restriction"].ToString().Trim().Length > 0)
                {
                    Observation_Participation_Restriction = true;
                }
                if (Autonomic || Motor || UpperLimb || LowerLimb || Trunk || Observation_GeneralPosture || Observation_SocialInteraction_Responsivity || Observation_Feeding || Observation_Participation || Observation_Participation_Restriction)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Observation :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (Autonomic)
                    {

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.SetWidths(new float[] { 0.3f, 1f });
                        table.SpacingBefore = 20f;

                        cell = PhraseCell(new Phrase("Autonomic :", HeadingFont), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        table.AddCell(cell);
                        cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        cell.PaddingBottom = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        #region
                        if (Observation_Autonomic_HR)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("HR :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Autonomic_HR"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Observation_Autonomic_TypesOfRespiration)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("TYPES OF RESPIRATION :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Autonomic_TypesOfRespiration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Observation_Autonomic_SkinColour)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("SKIN COLOUR :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Autonomic_SkinColour"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion

                        #region
                        if (Observation_Autonomic_TemperatureCentral_Peripheral)
                        {
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 20f;
                            cell = new PdfPCell(PhraseCell(new Phrase("TEMPERATURE –CENTRAL /PERIPHERAL :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 30f;
                            table.AddCell(cell);
                            document.Add(table);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            table.SpacingBefore = 10f;
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Autonomic_TemperatureCentral_Peripheral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 3f;
                            cell.PaddingLeft = 60f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion

                    #region
                    if (Motor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Motor :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Motor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
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

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Level ", NormalFont), PdfPCell.ALIGN_CENTER));
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

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_UpperLimbRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


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

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("Level ", NormalFont), PdfPCell.ALIGN_CENTER));
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

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLevel1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLeft1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbRight1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLevel2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLeft2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbRight2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLevel3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbLeft3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);



                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_LowerLimbRight3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Trunk)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Trunk :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Trunk"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_GeneralPosture)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("General Posture :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_GeneralPosture"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_SocialInteraction_Responsivity)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Social Interaction /Responsivity :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_SocialInteraction_Responsivity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_SocialInteraction_Responsivity)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Social Interaction /Responsivity :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_SocialInteraction_Responsivity"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_Feeding)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Feeding :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Feeding"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_Participation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Participation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Participation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Observation_Participation_Restriction)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Participation Restriction :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Observation_Participation_Restriction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                }

                #endregion

                #region ************** Examination *********************
                bool Ballards = false;
                if (ds.Tables[1].Rows[0]["Examination_Ballards1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards4"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards5"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards6"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards7"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards8"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards9"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards10"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards11"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Ballards12"].ToString().Trim().Length > 0)
                {
                    Ballards = true;
                }
                bool Timp = false;
                if (ds.Tables[1].Rows[0]["Examination_Timp1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp4"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp5"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp6"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp7"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Timp8"].ToString().Trim().Length > 0)
                {
                    Timp = true;
                }
                bool Voitas = false;
                if (ds.Tables[1].Rows[0]["Examination_Voitas1"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas2"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas3"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas4"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas5"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas6"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas7"].ToString().Trim().Length > 0 || ds.Tables[1].Rows[0]["Examination_Voitas8"].ToString().Trim().Length > 0)
                {
                    Voitas = true;
                }
                bool Examination_GoalsOf_Treatment = false; if (ds.Tables[1].Rows[0]["Examination_GoalsOf_Treatment"].ToString().Trim().Length > 0) { Examination_GoalsOf_Treatment = true; }

                bool Examination_TreatmentGiven = false; if (ds.Tables[1].Rows[0]["Examination_TreatmentGiven"].ToString().Trim().Length > 0) { Examination_TreatmentGiven = true; }
                if (Ballards || Timp || Voitas || Examination_GoalsOf_Treatment || Examination_TreatmentGiven)
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
                    if (Ballards)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Ballards :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards8"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards9"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards10"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards11"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Ballards12"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Timp)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Timp :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Timp8"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Voitas)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Voitas :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas4"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas5"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas6"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas7"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_Voitas8"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Examination_GoalsOf_Treatment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Goals Of Treatment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_GoalsOf_Treatment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion

                    #region
                    if (Examination_TreatmentGiven)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Treatment Given :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Examination_TreatmentGiven"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
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
                    mail = AM.sendAttachment(mailid, "", "EIP Report", result_sheet);
                    if (mail)
                    {
                        DbHelper.SqlDb db = new DbHelper.SqlDb();
                        SqlCommand cmd = new SqlCommand("UPDATE ReportEPI_Mst SET MailSend=CAST('True'AS BIT) WHERE AppointmentID=@AppointmentID");
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

        private class EIPReport
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