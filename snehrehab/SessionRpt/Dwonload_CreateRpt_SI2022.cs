using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace snehrehab.SessionRpt
{
    public class Dwonload_CreateRpt_SI2022
    {
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
                        //Image imgSoc = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/theme3.jpg"));
                        //imgSoc.ScaleToFit(110, 110);
                        //imgSoc.SetAbsolutePosition(510, 810);
                        //imgSoc.ScaleAbsoluteWidth(70F);
                        //cb.AddImage(imgSoc);
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

                PdfPCell pdfCell6 = new PdfPCell();

                //set the alignment of all three cells and set border to 0
                pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;

                pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;

                // pdfCell4.Colspan = 3;

                pdfCell1.Border = 0;
                pdfCell2.Border = 0;
                pdfCell3.Border = 0;

                //add all three cells into PdfTable
                pdfTab.AddCell(pdfCell1);
                pdfTab.AddCell(pdfCell2);
                pdfTab.AddCell(pdfCell3);

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

        public void SI_RPT2022(int _appointmentID, HttpContext context)
        {
            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataSet ds = SID.Getsi2022(_appointmentID);
            Document document = new Document(PageSize.A4, 30f, 30f, 50f, 50f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            Font SubHeadingFont = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font NextHeadingFont = FontFactory.GetFont("Arial", 11, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("SI Report - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

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
                //  cell = ImageCell("~/images/rpt-logo.png", 25f, PdfPCell.ALIGN_LEFT);
                table.AddCell(cell);
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                //color = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
                color = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFF"));
                DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f, color);
                DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, color);
                document.Add(table);

                document.Add(new Paragraph(" "));
                //document.Add(new Paragraph(" "));
                //document.Add(new Paragraph(" "));

                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_LEFT;

                table.SetWidths(new float[] { 0.3f, 1f });
                table.SpacingBefore = 20f;

                cell = PhraseCell(new Phrase("PATIENT INFORMATION :", HeadingFont), PdfPCell.ALIGN_LEFT);
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
                DataTable dtT = SID.GetTherapist(_appointmentID);
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
                // document.Add(new Paragraph(" "));
                // document.Add(new Paragraph(" "));
                // document.Add(new Paragraph(" "));
                #endregion

                #region **************DailySchedule And ClinicalObservation*********************

                bool Clinical_orbsevation = false; if (ds.Tables[1].Rows[0]["ClinicalObservation"].ToString().Trim().Length > 0) { Clinical_orbsevation = true; }
                //bool TIME = false; if (ds.Tables[1].Rows[0]["TIME"].ToString().Trim().Length > 0) { TIME = true; }
                //bool ACTIVITIES = false; if (ds.Tables[1].Rows[0]["ACTIVITIES"].ToString().Trim().Length > 0) { ACTIVITIES = true; }
                //bool COMMENTS = false; if (ds.Tables[1].Rows[0]["COMMENTS"].ToString().Trim().Length > 0) { COMMENTS = true; }

                bool TIME = false;
                bool ACTIVITIES = false;
                bool COMMENTS = false;
                if (ds.Tables[7].Rows.Count > 0)
                {
                    if (ds.Tables[7].Rows[0]["TIME"].ToString().Trim().Length > 0) { TIME = true; }
                    if (ds.Tables[7].Rows[0]["ACTIVITIES"].ToString().Trim().Length > 0) { ACTIVITIES = true; }
                    if (ds.Tables[7].Rows[0]["COMMENTS"].ToString().Trim().Length > 0) { COMMENTS = true; }
                }


                if (Clinical_orbsevation)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("CLINICAL OBSERVATION :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    table = new PdfPTable(1);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ClinicalObservation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f;
                    cell.PaddingLeft = 60f;
                    table.AddCell(cell);
                    document.Add(table);
                    // document.Add(Chunk.NEXTPAGE);
                }


                if (TIME || ACTIVITIES || COMMENTS)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("DAILY SCHEDULE :", HeadingFont), PdfPCell.ALIGN_LEFT);
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
                    table.SpacingBefore = 10f;

                    #region headers
                    cell = new PdfPCell(PhraseCell(new Phrase("TIME", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("ACTIVITIES", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("COMMENTS", HeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    #endregion

                    #region
                    //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //{
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[i]["TIME"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    //    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    //    table.AddCell(cell);

                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[i]["ACTIVITIES"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    //    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    //    table.AddCell(cell);

                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[i]["COMMENTS"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                    //    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    //    table.AddCell(cell);

                    //}

                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[7].Rows[i]["TIME"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[7].Rows[i]["ACTIVITIES"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[7].Rows[i]["COMMENTS"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f; cell.PaddingLeft = 3f; cell.PaddingRight = 3f; cell.PaddingTop = 3f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                    }
                    #endregion
                    document.Add(table);
                }
                //document.Add(Chunk.NEXTPAGE);
                #endregion

                #region
                //#region **************SELF CARE*********************
                ////bool SelfCare_CurrentlyEats = false; if (ds.Tables[1].Rows[0]["SelfCare_CurrentlyEats"].ToString().Trim().Length > 0) { SelfCare_CurrentlyEats = true; }SelfCare_CurrentlyEats
                //bool SelfCare_Brushing = false; if (ds.Tables[1].Rows[0]["SelfCare_Brushing"].ToString().Trim().Length > 0) { SelfCare_Brushing = true; }
                //bool SelfCare_Bathing = false; if (ds.Tables[1].Rows[0]["SelfCare_Bathing"].ToString().Trim().Length > 0) { SelfCare_Bathing = true; }
                //bool SelfCare_Toileting = false; if (ds.Tables[1].Rows[0]["SelfCare_Toileting"].ToString().Trim().Length > 0) { SelfCare_Toileting = true; }
                //bool SelfCare_Dressing = false; if (ds.Tables[1].Rows[0]["SelfCare_Dressing"].ToString().Trim().Length > 0) { SelfCare_Dressing = true; }
                //bool SelfCare_Breakfast = false; if (ds.Tables[1].Rows[0]["SelfCare_Breakfast"].ToString().Trim().Length > 0) { SelfCare_Breakfast = true; }
                //bool SelfCare_Lunch = false; if (ds.Tables[1].Rows[0]["SelfCare_Lunch"].ToString().Trim().Length > 0) { SelfCare_Lunch = true; }
                //bool SelfCare_Snacks = false; if (ds.Tables[1].Rows[0]["SelfCare_Snacks"].ToString().Trim().Length > 0) { SelfCare_Snacks = true; }
                //bool SelfCare_Dinner = false; if (ds.Tables[1].Rows[0]["SelfCare_Dinner"].ToString().Trim().Length > 0) { SelfCare_Dinner = true; }
                //bool SelfCare_GettingInBus = false; if (ds.Tables[1].Rows[0]["SelfCare_GettingInBus"].ToString().Trim().Length > 0) { SelfCare_GettingInBus = true; }
                //bool SelfCare_GoingToSchool = false; if (ds.Tables[1].Rows[0]["SelfCare_GoingToSchool"].ToString().Trim().Length > 0) { SelfCare_GoingToSchool = true; }
                //bool SelfCare_comeBackSchool = false; if (ds.Tables[1].Rows[0]["SelfCare_comeBackSchool"].ToString().Trim().Length > 0) { SelfCare_comeBackSchool = true; }
                //bool SelfCare_Ambulation = false; if (ds.Tables[1].Rows[0]["SelfCare_Ambulation"].ToString().Trim().Length > 0) { SelfCare_Ambulation = true; }
                //bool SelfCare_Homeostaticchanges = false; if (ds.Tables[1].Rows[0]["SelfCare_Homeostaticchanges"].ToString().Trim().Length > 0) { SelfCare_Homeostaticchanges = true; }
                //bool SelfCare_UrinationdetailsBedwetting = false; if (ds.Tables[1].Rows[0]["SelfCare_UrinationdetailsBedwetting"].ToString().Trim().Length > 0) { SelfCare_UrinationdetailsBedwetting = true; }
                //bool Self_cmt = false; if (ds.Tables[1].Rows[0]["Self_cmt"].ToString().Trim().Length > 0) { Self_cmt = true; }
                //if (SelfCare_Brushing || SelfCare_Bathing || SelfCare_Toileting || SelfCare_Dressing || SelfCare_Breakfast || SelfCare_Lunch || SelfCare_Snacks ||
                //   SelfCare_Dinner || SelfCare_GettingInBus || SelfCare_GoingToSchool || SelfCare_comeBackSchool || SelfCare_Ambulation || SelfCare_Homeostaticchanges || SelfCare_UrinationdetailsBedwetting || Self_cmt)
                //{
                //    table = new PdfPTable(2);
                //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    table.SetWidths(new float[] { 0.3f, 1f });
                //    table.SpacingBefore = 20f;

                //    cell = PhraseCell(new Phrase("SELF CARE:", HeadingFont), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    table.AddCell(cell);
                //    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                //    cell.Colspan = 2;
                //    cell.PaddingBottom = 30f;
                //    table.AddCell(cell);
                //    document.Add(table);

                //    //if (SelfCare_CurrentlyEats)
                //    //{
                //    //    table = new PdfPTable(1);
                //    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    //    table.WidthPercentage = 100;
                //    //    table.SpacingBefore = 20f;
                //    //    cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_CurrentlyEats :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //    //    cell.PaddingBottom = 3f;
                //    //    cell.PaddingLeft = 30f;
                //    //    table.AddCell(cell);
                //    //    document.Add(table);

                //    //    table = new PdfPTable(1);
                //    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                //    //    table.WidthPercentage = 100;
                //    //    table.SpacingBefore = 10f;
                //    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_CurrentlyEats"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //    //    cell.PaddingBottom = 3f;
                //    //    cell.PaddingLeft = 60f;
                //    //    table.AddCell(cell);
                //    //    document.Add(table);
                //    //    // document.Add(Chunk.NEXTPAGE);
                //    //}
                //    if (SelfCare_Brushing)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Brushing :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Brushing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Bathing)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Bathing :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Bathing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Toileting)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Toileting :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Toileting"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Dressing)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Dressing :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Dressing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Breakfast)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Breakfast :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Breakfast"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Lunch)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Lunch :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Lunch"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Snacks)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Snacks :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Snacks"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Dinner)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Dinner :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Dinner"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_GettingInBus)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_GettingInBus :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_GettingInBus"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_GoingToSchool)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_GoingToSchool :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_GoingToSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_comeBackSchool)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_comeBackSchool :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_comeBackSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Ambulation)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Ambulation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Ambulation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_Homeostaticchanges)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_Homeostaticchanges :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_Homeostaticchanges"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);
                //    }
                //    if (SelfCare_UrinationdetailsBedwetting)
                //    {
                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 20f;
                //        cell = new PdfPCell(PhraseCell(new Phrase("SelfCare_UrinationdetailsBedwetting :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 30f;
                //        table.AddCell(cell);
                //        document.Add(table);

                //        table = new PdfPTable(1);
                //        table.HorizontalAlignment = Element.ALIGN_LEFT;
                //        table.WidthPercentage = 100;
                //        table.SpacingBefore = 10f;
                //        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SelfCare_UrinationdetailsBedwetting"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //        cell.PaddingBottom = 3f;
                //        cell.PaddingLeft = 60f;
                //        table.AddCell(cell);
                //        document.Add(table);
                //        // document.Add(Chunk.NEXTPAGE);

                //        if (Self_cmt)
                //        {
                //            table = new PdfPTable(1);
                //            table.HorizontalAlignment = Element.ALIGN_LEFT;
                //            table.WidthPercentage = 100;
                //            table.SpacingBefore = 20f;
                //            cell = new PdfPCell(PhraseCell(new Phrase("Self_cmt :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                //            cell.PaddingBottom = 3f;
                //            cell.PaddingLeft = 30f;
                //            table.AddCell(cell);
                //            document.Add(table);

                //            table = new PdfPTable(1);
                //            table.HorizontalAlignment = Element.ALIGN_LEFT;
                //            table.WidthPercentage = 100;
                //            table.SpacingBefore = 10f;
                //            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Self_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                //            cell.PaddingBottom = 3f;
                //            cell.PaddingLeft = 60f;
                //            table.AddCell(cell);
                //            document.Add(table);
                //            // document.Add(Chunk.NEXTPAGE);
                //        }
                //    }
                //}
                //#endregion
                #endregion

                #region***********FamilyStructure****************
                bool FamilyStructure_QualityTimeMother = false; if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeMother"].ToString().Trim().Length > 0) { FamilyStructure_QualityTimeMother = true; }
                bool FamilyStructure_QualityTimeFather = false; if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeFather"].ToString().Trim().Length > 0) { FamilyStructure_QualityTimeFather = true; }
                bool FamilyStructure_QualityTimeWeekend = false; if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeWeekend"].ToString().Trim().Length > 0) { FamilyStructure_QualityTimeWeekend = true; }
                bool Father_Weekends = false; if (ds.Tables[1].Rows[0]["Father_Weekends"].ToString().Trim().Length > 0) { Father_Weekends = true; }
                bool FamilyStructure_TimeForThreapy = false; if (ds.Tables[1].Rows[0]["FamilyStructure_TimeForThreapy"].ToString().Trim().Length > 0) { FamilyStructure_TimeForThreapy = true; }
                bool FamilyStructure_AcceptanceCondition = false; if (ds.Tables[1].Rows[0]["FamilyStructure_AcceptanceCondition"].ToString().Trim().Length > 0) { FamilyStructure_AcceptanceCondition = true; }
                bool FamilyStructure_ExtraCaricular = false; if (ds.Tables[1].Rows[0]["FamilyStructure_ExtraCaricular"].ToString().Trim().Length > 0) { FamilyStructure_ExtraCaricular = true; }
                bool FamilyStructure_Diciplinary = false; if (ds.Tables[1].Rows[0]["FamilyStructure_Diciplinary"].ToString().Trim().Length > 0) { FamilyStructure_Diciplinary = true; }
                bool FamilyStructure_SiblingBrother = false; if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingBrother"].ToString().Trim().Length > 0) { FamilyStructure_SiblingBrother = true; }
                //bool FamilyStructure_SiblingNA = false; if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingNA"].ToString().Trim().Length > 0) { FamilyStructure_SiblingNA = true; }
                bool FamilyStructure_Expectations = false; if (ds.Tables[1].Rows[0]["FamilyStructure_Expectations"].ToString().Trim().Length > 0) { FamilyStructure_Expectations = true; }
                bool FamilyStructure_CloselyInvolved = false; if (ds.Tables[1].Rows[0]["FamilyStructure_CloselyInvolved"].ToString().Trim().Length > 0) { FamilyStructure_CloselyInvolved = true; }
                bool FAMILY_cmt = false; if (ds.Tables[1].Rows[0]["FAMILY_cmt"].ToString().Trim().Length > 0) { FAMILY_cmt = true; }

                if (FamilyStructure_QualityTimeMother || FamilyStructure_QualityTimeFather || FamilyStructure_QualityTimeWeekend || Father_Weekends || FamilyStructure_TimeForThreapy ||
                    FamilyStructure_AcceptanceCondition || FamilyStructure_ExtraCaricular || FamilyStructure_Diciplinary || FamilyStructure_SiblingBrother ||
                   FamilyStructure_Expectations || FamilyStructure_CloselyInvolved || FAMILY_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("FAMILY STRUCTURE:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);



                    if (FamilyStructure_QualityTimeMother)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Mother's Quality Time spent with the child daily. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeMother"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_QualityTimeFather)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Father's Quality Time spent with the child daily. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeFather"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_QualityTimeWeekend)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Mother's Quality time spent on Sunday/ weekends with Child. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeWeekend"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Father_Weekends)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Father's Quality time spent on Sunday/ weekends with Child. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Father_Weekends"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_TimeForThreapy)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Willingness to devote time for therapy :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_TimeForThreapy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_AcceptanceCondition)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Acceptance of the condition:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_AcceptanceCondition"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_ExtraCaricular)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Accessibility to play areas/extracurricular activities :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_ExtraCaricular"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_Diciplinary)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.Disciplinary measures taken :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_Diciplinary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_SiblingBrother)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("9.Relationship with siblings :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_SiblingBrother"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_Expectations)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("10.Expectations from the child's performance :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_Expectations"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FamilyStructure_CloselyInvolved)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("11.Others Closely Involved With: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FamilyStructure_CloselyInvolved"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (FAMILY_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["FAMILY_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                }


                #endregion

                #region*********School Info************
                bool Schoolinfo_Attend = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Attend"].ToString().Trim().Length > 0) { Schoolinfo_Attend = true; }
                bool Schoolinfo_Type = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Type"].ToString().Trim().Length > 0) { Schoolinfo_Type = true; }
                bool Schoolinfo_SchoolHours = false; if (ds.Tables[1].Rows[0]["Schoolinfo_SchoolHours"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Schoolinfo_SchoolHours"].ToString().Trim() != "0") { Schoolinfo_SchoolHours = true; }
                bool School_Bus = false; if (ds.Tables[1].Rows[0]["School_Bus"].ToString().Trim().Length > 0) { School_Bus = true; }
                bool Car = false; if (ds.Tables[1].Rows[0]["Car"].ToString().Trim().Length > 0) { Car = true; }
                bool Two_Wheelers = false; if (ds.Tables[1].Rows[0]["Two_Wheelers"].ToString().Trim().Length > 0) { Two_Wheelers = true; }
                bool walking = false; if (ds.Tables[1].Rows[0]["walking"].ToString().Trim().Length > 0) { walking = true; }
                bool Public_Transport = false; if (ds.Tables[1].Rows[0]["Public_Transport"].ToString().Trim().Length > 0) { Public_Transport = true; }
                bool Schoolinfo_NoOfTeacher = false; if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString().Trim().Length > 0) { Schoolinfo_NoOfTeacher = true; }

                bool Floor = false; if (ds.Tables[1].Rows[0]["Floor"].ToString().Trim().Length > 0) { Floor = true; }
                bool single_bench = false; if (ds.Tables[1].Rows[0]["single_bench"].ToString().Trim().Length > 0) { single_bench = true; }
                bool bench2 = false; if (ds.Tables[1].Rows[0]["bench2"].ToString().Trim().Length > 0) { bench2 = true; }
                bool round_table = false; if (ds.Tables[1].Rows[0]["round_table"].ToString().Trim().Length > 0) { round_table = true; }
                bool Schoolinfo_Mealtime = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Mealtime"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["Schoolinfo_SchoolHours"].ToString().Trim() != "0") { Schoolinfo_Mealtime = true; }
                bool Schoolinfo_MealType = false; if (ds.Tables[1].Rows[0]["Schoolinfo_MealType"].ToString().Trim().Length > 0) { Schoolinfo_MealType = true; }
                bool Schoolinfo_Shareing = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Shareing"].ToString().Trim().Length > 0) { Schoolinfo_Shareing = true; }
                bool Schoolinfo_HelpEating = false; if (ds.Tables[1].Rows[0]["Schoolinfo_HelpEating"].ToString().Trim().Length > 0) { Schoolinfo_HelpEating = true; }
                bool Schoolinfo_Friendship = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Friendship"].ToString().Trim().Length > 0) { Schoolinfo_Friendship = true; }
                bool Schoolinfo_InteractionPeer = false; if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionPeer"].ToString().Trim().Length > 0) { Schoolinfo_InteractionPeer = true; }
                bool Schoolinfo_InteractionTeacher = false; if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionTeacher"].ToString().Trim().Length > 0) { Schoolinfo_InteractionTeacher = true; }
                bool Schoolinfo_AnnualFunction = false; if (ds.Tables[1].Rows[0]["Schoolinfo_AnnualFunction"].ToString().Trim().Length > 0) { Schoolinfo_AnnualFunction = true; }
                bool Schoolinfo_Sports = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Sports"].ToString().Trim().Length > 0) { Schoolinfo_Sports = true; }
                bool Schoolinfo_Picnic = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Picnic"].ToString().Trim().Length > 0) { Schoolinfo_Picnic = true; }
                bool Schoolinfo_ExtraCaricular = false; if (ds.Tables[1].Rows[0]["Schoolinfo_ExtraCaricular"].ToString().Trim().Length > 0) { Schoolinfo_ExtraCaricular = true; }
                bool Schoolinfo_CopyBoard = false; if (ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString().Trim().Length > 0) { Schoolinfo_CopyBoard = true; }
                bool Schoolinfo_Instructions = false; if (ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString().Trim().Length > 0) { Schoolinfo_Instructions = true; }
                bool Schoolinfo_ShadowTeacher = false; if (ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString().Trim().Length > 0) { Schoolinfo_ShadowTeacher = true; }
                bool Schoolinfo_CW_HW = false; if (ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString().Trim().Length > 0) { Schoolinfo_CW_HW = true; }
                bool Schoolinfo_SpecialEducator = false; if (ds.Tables[1].Rows[0]["Schoolinfo_SpecialEducator"].ToString().Trim().Length > 0) { Schoolinfo_SpecialEducator = true; }
                bool Schoolinfo_DeliveryInformation = false; if (ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString().Trim().Length > 0) { Schoolinfo_DeliveryInformation = true; }
                bool Schoolinfo_RemarkTeacher = false; if (ds.Tables[1].Rows[0]["Schoolinfo_RemarkTeacher"].ToString().Trim().Length > 0) { Schoolinfo_RemarkTeacher = true; }
                bool SCHOOL_cmt = false; if (ds.Tables[1].Rows[0]["SCHOOL_cmt"].ToString().Trim().Length > 0) { SCHOOL_cmt = true; }


                if (Schoolinfo_Attend || Schoolinfo_Type || Schoolinfo_SchoolHours || School_Bus || Car || Two_Wheelers || walking || Public_Transport || Schoolinfo_NoOfTeacher || Schoolinfo_NoOfTeacher || /*Schoolinfo_NoOfStudent ||*/ Floor || single_bench || bench2 || round_table || Schoolinfo_Mealtime ||
                    Schoolinfo_MealType || Schoolinfo_Shareing || Schoolinfo_HelpEating || Schoolinfo_Friendship || Schoolinfo_InteractionPeer || Schoolinfo_InteractionTeacher ||
                   Schoolinfo_AnnualFunction || Schoolinfo_Sports || Schoolinfo_Picnic || Schoolinfo_ExtraCaricular || Schoolinfo_CopyBoard || Schoolinfo_Instructions ||
                   Schoolinfo_ShadowTeacher || Schoolinfo_CW_HW || Schoolinfo_SpecialEducator || Schoolinfo_DeliveryInformation || Schoolinfo_RemarkTeacher || SCHOOL_cmt
                   )
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("SCHOOL INFORMATION:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (Schoolinfo_Attend)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Does the child attend school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Attend"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_Type)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Type of school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Type"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_SchoolHours)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Number of hours: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_SchoolHours"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (School_Bus)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["School_Bus"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Car)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Car"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Two_Wheelers)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Two_Wheelers"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (walking)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["walking"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Public_Transport)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.How do they travel :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Public_Transport"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Schoolinfo_NoOfTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("9.Teacher to child ratio :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Floor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("10.Seating arrangement: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Floor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (single_bench)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("11.Seating arrangement: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["single_bench"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (bench2)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("12.Seating arrangement: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["bench2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (round_table)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("13.Seating arrangement: :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["round_table"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Schoolinfo_Mealtime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("14.Meal time at the school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Mealtime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_MealType)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("16.Meal type :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_MealType"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_Shareing)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("17.Sharing done by child :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Shareing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_HelpEating)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("18.Help required in eating :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_HelpEating"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_Friendship)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("19.Friendships initiated by child :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Friendship"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_InteractionPeer)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("20.Interaction with peers :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_InteractionPeer"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_InteractionTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("21.Interaction with the teacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_InteractionTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_AnnualFunction)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("22.Annuals/culturals function :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_AnnualFunction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Schoolinfo_Picnic)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("23.Picnics/field trips :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Picnic"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_ExtraCaricular)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("24.Extra curricular :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_ExtraCaricular"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_CopyBoard)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("25.Copying from board :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_Instructions)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("26.Follows instructions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_ShadowTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("27.ShadowTeacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_CW_HW)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("28.Completing CW/HW :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_SpecialEducator)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("29.Provision of special educator/shadow/ remedial teacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_SpecialEducator"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_DeliveryInformation)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("30.Mode of delivery of information :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Schoolinfo_RemarkTeacher)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("31.Remark of the teacher :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Schoolinfo_RemarkTeacher"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SCHOOL_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SCHOOL_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                }
                #endregion

                #region **************Personal Social*********************
                bool PersonalSocial_CurrentPlace = false; if (ds.Tables[1].Rows[0]["PersonalSocial_CurrentPlace"].ToString().Trim().Length > 0) { PersonalSocial_CurrentPlace = true; }
                bool PersonalSocial_WhatHeDoes = false; if (ds.Tables[1].Rows[0]["PersonalSocial_WhatHeDoes"].ToString().Trim().Length > 0) { PersonalSocial_WhatHeDoes = true; }
                bool PersonalSocial_BodyAwareness = false; if (ds.Tables[1].Rows[0]["PersonalSocial_BodyAwareness"].ToString().Trim().Length > 0) { PersonalSocial_BodyAwareness = true; }
                bool PersonalSocial_BodySchema = false; if (ds.Tables[1].Rows[0]["PersonalSocial_BodySchema"].ToString().Trim().Length > 0) { PersonalSocial_BodySchema = true; }
                bool PersonalSocial_ExploreEnvironment = false; if (ds.Tables[1].Rows[0]["PersonalSocial_ExploreEnvironment"].ToString().Trim().Length > 0) { PersonalSocial_ExploreEnvironment = true; }
                bool PersonalSocial_Motivated = false; if (ds.Tables[1].Rows[0]["PersonalSocial_Motivated"].ToString().Trim().Length > 0) { PersonalSocial_Motivated = true; }
                bool PersonalSocial_EyeContact = false; if (ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString().Trim().Length > 0) { PersonalSocial_EyeContact = true; }
                bool PersonalSocial_SocialSmile = false; if (ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString().Trim().Length > 0) { PersonalSocial_SocialSmile = true; }
                bool PersonalSocial_FamilyRegards = false; if (ds.Tables[1].Rows[0]["PersonalSocial_FamilyRegards"].ToString().Trim().Length > 0) { PersonalSocial_FamilyRegards = true; }
                //bool PersonalSocial_RateChild = false; if (ds.Tables[1].Rows[0]["PersonalSocial_RateChild"].ToString().Trim().Length > 0) { PersonalSocial_RateChild = true; }
                bool PersonalSocial_ChildSocially = false; if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Trim().Length > 0) { PersonalSocial_ChildSocially = true; }
                bool PERSONAL_cmt = false; if (ds.Tables[1].Rows[0]["PERSONAL_cmt"].ToString().Trim().Length > 0) { PERSONAL_cmt = true; }

                if (PersonalSocial_CurrentPlace || PersonalSocial_WhatHeDoes || PersonalSocial_BodyAwareness || PersonalSocial_BodySchema || PersonalSocial_ExploreEnvironment ||
                   PersonalSocial_Motivated || PersonalSocial_EyeContact || PersonalSocial_SocialSmile || PersonalSocial_FamilyRegards || /*PersonalSocial_RateChild || */PersonalSocial_ChildSocially || PERSONAL_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("PERSONAL SOCIAL:", HeadingFont), PdfPCell.ALIGN_LEFT);
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

                    cell = PhraseCell(new Phrase("A)RELATIONSHIP WITH SELF", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (PersonalSocial_CurrentPlace)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1. Does he know the current place:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_CurrentPlace"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_WhatHeDoes)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" 2. Is your child aware of what he does? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_WhatHeDoes"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_BodyAwareness)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3. Does the child have own body awareness?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_BodyAwareness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_BodySchema)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" 4.Is your child aware of body schema?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_BodySchema"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_ExploreEnvironment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" 5.  Does your child self explores the environment?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_ExploreEnvironment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_Motivated)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" 6. Is your child motivated?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_Motivated"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("B)RELATIONSHIP WITH OTHERS", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (PersonalSocial_EyeContact)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("   1. Eye contact", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_SocialSmile)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase(" 2. Social smile", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PersonalSocial_FamilyRegards)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3. Family Regards", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_FamilyRegards"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    //if (PersonalSocial_RateChild)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("PersonalSocial_RateChild :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_RateChild"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //    // document.Add(Chunk.NEXTPAGE);
                    //}
                    if (PersonalSocial_ChildSocially)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("HOW IS THE CHILD SOCIALLY? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (PERSONAL_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PERSONAL_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                }
                #endregion

                #region **************Speech Language*********************
                bool SpeechLanguage_StartSpeek = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_StartSpeek"].ToString().Trim().Length > 0) { SpeechLanguage_StartSpeek = true; }
                bool SpeechLanguage_Monosyllables = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Monosyllables"].ToString().Trim().Length > 0) { SpeechLanguage_Monosyllables = true; }
                bool SpeechLanguage_Bisyllables = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Bisyllables"].ToString().Trim().Length > 0) { SpeechLanguage_Bisyllables = true; }
                bool SpeechLanguage_ShrotScentences = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_ShrotScentences"].ToString().Trim().Length > 0) { SpeechLanguage_ShrotScentences = true; }
                bool SpeechLanguage_LongScentences = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_LongScentences"].ToString().Trim().Length > 0) { SpeechLanguage_LongScentences = true; }
                bool SpeechLanguage_UnusualSoundsJargonSpeech = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_UnusualSoundsJargonSpeech"].ToString().Trim().Length > 0) { SpeechLanguage_UnusualSoundsJargonSpeech = true; }
                bool SpeechLanguage_speechgestures = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_speechgestures"].ToString().Trim().Length > 0) { SpeechLanguage_speechgestures = true; }
                bool SpeechLanguage_NonverbalfacialEyeContact = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialEyeContact"].ToString().Trim().Length > 0) { SpeechLanguage_NonverbalfacialEyeContact = true; }
                bool SpeechLanguage_NonverbalfacialGestures = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialGestures"].ToString().Trim().Length > 0) { SpeechLanguage_NonverbalfacialGestures = true; }
                bool SpeechLanguage_SimpleComplex = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_SimpleComplex"].ToString().Trim().Length > 0) { SpeechLanguage_SimpleComplex = true; }
                bool SpeechLanguage_UnderstandImpliedMeaning = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandImpliedMeaning"].ToString().Trim().Length > 0) { SpeechLanguage_UnderstandImpliedMeaning = true; }
                bool SpeechLanguage_UnderstandJokesarcasm = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandJokesarcasm"].ToString().Trim().Length > 0) { SpeechLanguage_UnderstandJokesarcasm = true; }
                bool SpeechLanguage_Respondstoname = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Respondstoname"].ToString().Trim().Length > 0) { SpeechLanguage_Respondstoname = true; }
                bool SpeechLanguage_Facialexpression = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialExpression"].ToString().Trim().Length > 0) { SpeechLanguage_Facialexpression = true; }

                bool SpeechLanguage_TwowayInteraction = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_TwowayInteraction"].ToString().Trim().Length > 0) { SpeechLanguage_TwowayInteraction = true; }
                bool SpeechLanguage_NarrateIncidentsAtSchool = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtSchool"].ToString().Trim().Length > 0) { SpeechLanguage_NarrateIncidentsAtSchool = true; }
                bool SpeechLanguage_NarrateIncidentsAtHome = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtHome"].ToString().Trim().Length > 0) { SpeechLanguage_NarrateIncidentsAtHome = true; }
                //bool SpeechLanguage_Want = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Want"].ToString().Trim().Length > 0) { SpeechLanguage_Want = true; }
                bool SpeechLanguage_Needs = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Needs"].ToString().Trim().Length > 0) { SpeechLanguage_Needs = true; }
                bool SpeechLanguage_Emotions = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Emotions"].ToString().Trim().Length > 0) { SpeechLanguage_Emotions = true; }
                bool SpeechLanguage_AchievementsFailure = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_AchievementsFailure"].ToString().Trim().Length > 0) { SpeechLanguage_AchievementsFailure = true; }
                //bool SpeechLanguage_LanguageSpoken = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_LanguageSpoken"].ToString().Trim().Length > 0) { SpeechLanguage_LanguageSpoken = true; }
                bool SpeechLanguage_Echolalia = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Echolalia"].ToString().Trim().Length > 0) { SpeechLanguage_Echolalia = true; }
                bool Speech_cmt = false; if (ds.Tables[1].Rows[0]["Speech_cmt"].ToString().Trim().Length > 0) { Speech_cmt = true; }
                //bool SpeechLanguage_Emotionalmilestones = false; if (ds.Tables[1].Rows[0]["SpeechLanguage_Emotionalmilestones"].ToString().Trim().Length > 0) { SpeechLanguage_Emotionalmilestones = true; }

                if (SpeechLanguage_StartSpeek || SpeechLanguage_Monosyllables || SpeechLanguage_Bisyllables || SpeechLanguage_ShrotScentences || SpeechLanguage_LongScentences ||
                   SpeechLanguage_UnusualSoundsJargonSpeech || SpeechLanguage_speechgestures || SpeechLanguage_NonverbalfacialEyeContact || SpeechLanguage_NonverbalfacialGestures ||
                   SpeechLanguage_SimpleComplex || SpeechLanguage_UnderstandImpliedMeaning || SpeechLanguage_UnderstandJokesarcasm || SpeechLanguage_Respondstoname ||
                   SpeechLanguage_TwowayInteraction || SpeechLanguage_NarrateIncidentsAtSchool || SpeechLanguage_NarrateIncidentsAtHome || /*SpeechLanguage_Want ||*/ SpeechLanguage_Needs ||
                   SpeechLanguage_Emotions || SpeechLanguage_AchievementsFailure || /*SpeechLanguage_LanguageSpoken ||*/ SpeechLanguage_Echolalia /*|| SpeechLanguage_Emotionalmilestones */ || Speech_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("SPEECH LANGUAGE:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (SpeechLanguage_StartSpeek)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.When did your Child Start to Speak :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_StartSpeek"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_Monosyllables)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.When did your Child Start to Monosyllables :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Monosyllables"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_Bisyllables)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.When did your Child Start to Bisyllables :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Bisyllables"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_ShrotScentences)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.When did your Child Start to short sentences :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_ShrotScentences"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_LongScentences)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.When did your Child Start to long Sentences :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_LongScentences"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_UnusualSoundsJargonSpeech)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Unusual Sounds /Jargon Speech :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_UnusualSoundsJargonSpeech"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_speechgestures)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Imitation of speech / gestures :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_speechgestures"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_Facialexpression)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.Non verbal facial: Expression", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialExpression"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_NonverbalfacialEyeContact)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("9. Non verbal facial: Eye contact", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialEyeContact"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_NonverbalfacialGestures)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("10.Non verbal facial: Gestures :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialGestures"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_SimpleComplex)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("11.Interpretation of language: Simple / Complex:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_SimpleComplex"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_UnderstandImpliedMeaning)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("12.Interpretation of language:Understand Implied Meaning :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandImpliedMeaning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_UnderstandJokesarcasm)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("13.Interpretation of language:Understand Joke / sarcasm :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandJokesarcasm"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_Respondstoname)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("14.Interpretation of language:Responds to name :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Respondstoname"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_TwowayInteraction)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("15.Two - way Interaction :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_TwowayInteraction"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_NarrateIncidentsAtSchool)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("16.Narrate Incidents:At School :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_NarrateIncidentsAtHome)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("17.Narrate Incidents:At Home/Expression of :Want :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtHome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (SpeechLanguage_Needs)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("18.Expression of :Needs :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Needs"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_Emotions)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("19.Expression of :Emotions :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Emotions"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (SpeechLanguage_AchievementsFailure)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("20.Expression of :Achievements / Failure :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_AchievementsFailure"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    //if (SpeechLanguage_LanguageSpoken)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("SpeechLanguage_LanguageSpoken :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_LanguageSpoken"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //    // document.Add(Chunk.NEXTPAGE);
                    //}
                    if (SpeechLanguage_Echolalia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("21.Echolalia :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Echolalia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Speech_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Speech_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    //if (SpeechLanguage_Emotionalmilestones)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("SpeechLanguage_Emotionalmilestones :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SpeechLanguage_Emotionalmilestones"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //    // document.Add(Chunk.NEXTPAGE);
                    //}

                }




                #endregion

                #region***********Behaviour****************
                bool Behaviour_FreeTime = false; if (ds.Tables[1].Rows[0]["Behaviour_FreeTime"].ToString().Trim().Length > 0) { Behaviour_FreeTime = true; }
                bool unassociated = false; if (ds.Tables[1].Rows[0]["unassociated"].ToString().Trim().Length > 0) { unassociated = true; }
                bool solitary = false; if (ds.Tables[1].Rows[0]["solitary"].ToString().Trim().Length > 0) { solitary = true; }
                bool onlooker = false; if (ds.Tables[1].Rows[0]["onlooker"].ToString().Trim().Length > 0) { onlooker = true; }
                bool parallel = false; if (ds.Tables[1].Rows[0]["parallel"].ToString().Trim().Length > 0) { parallel = true; }
                bool associative = false; if (ds.Tables[1].Rows[0]["associative"].ToString().Trim().Length > 0) { associative = true; }
                bool cooperative = false; if (ds.Tables[1].Rows[0]["cooperative"].ToString().Trim().Length > 0) { cooperative = true; }
                bool Behaviour_situationalmeltdowns = false; if (ds.Tables[1].Rows[0]["Behaviour_situationalmeltdowns"].ToString().Trim().Length > 0) { Behaviour_situationalmeltdowns = true; }
                bool BEHAVIOUR_cmt = false; if (ds.Tables[1].Rows[0]["BEHAVIOUR_cmt"].ToString().Trim().Length > 0) { BEHAVIOUR_cmt = true; }

                if (Behaviour_FreeTime || unassociated || solitary || onlooker || parallel || associative || cooperative || Behaviour_situationalmeltdowns || BEHAVIOUR_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("BEHAVIOUR:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (Behaviour_FreeTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.BEHAVIOUR OF THE CHILD :- What does the child do in his free time :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_FreeTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (unassociated)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["unassociated"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (solitary)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["solitary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (onlooker)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["onlooker"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (parallel)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["parallel"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (associative)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["associative"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (cooperative)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Type of play behaviour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["cooperative"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Behaviour_situationalmeltdowns)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.Does the child have situational meltdowns :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Behaviour_situationalmeltdowns"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (BEHAVIOUR_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BEHAVIOUR_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }


                }

                #endregion

                #region************Arousal*************
                bool rangevalue = false; if (ds.Tables[1].Rows[0]["rangevalue"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["rangevalue"].ToString().Trim() != "0") { rangevalue = true; }
                bool rangevalue2 = false; if (ds.Tables[1].Rows[0]["rangevalue2"].ToString().Trim().Length > 0 && ds.Tables[1].Rows[0]["rangevalue2"].ToString().Trim() != "0") { rangevalue2 = true; }
                bool Arousal_GeneralState = false; if (ds.Tables[1].Rows[0]["Arousal_GeneralState"].ToString().Trim().Length > 0) { Arousal_GeneralState = true; }
                bool Arousal_Stimuli = false; if (ds.Tables[1].Rows[0]["Arousal_Stimuli"].ToString().Trim().Length > 0) { Arousal_Stimuli = true; }
                bool Arousal_Transition = false; if (ds.Tables[1].Rows[0]["Arousal_Transition"].ToString().Trim().Length > 0) { Arousal_Transition = true; }
                //bool Arousal_Optimal = false; if (ds.Tables[1].Rows[0]["Arousal_Optimal"].ToString().Trim().Length > 0) { Arousal_Optimal = true; }
                bool Arousal_FactorOCD = false; if (ds.Tables[1].Rows[0]["Arousal_FactorOCD"].ToString().Trim().Length > 0) { Arousal_FactorOCD = true; }
                bool Arousal_ClaimingFactor = false; if (ds.Tables[1].Rows[0]["Arousal_ClaimingFactor"].ToString().Trim().Length > 0) { Arousal_ClaimingFactor = true; }
                bool Arousal_DipsDown = false; if (ds.Tables[1].Rows[0]["Arousal_DipsDown"].ToString().Trim().Length > 0) { Arousal_DipsDown = true; }
                bool AROUSAL_cmt = false; if (ds.Tables[1].Rows[0]["AROUSAL_cmt"].ToString().Trim().Length > 0) { AROUSAL_cmt = true; }

                //StringBuilder sb = new StringBuilder();                
                //sb.Append("<h1>INVOICE</h1>");

                //StringReader srr = new StringReader(sb.ToString());
                //HTMLWorker htmlparser = new HTMLWorker(document);
                //using (MemoryStream memoryStreamq = new MemoryStream())
                //{
                //    PdfWriter writerq = PdfWriter.GetInstance(document, memoryStreamq);
                //    //document.Open();
                //    htmlparser.Parse(srr);
                //    //document.Close();
                //    byte[] bytess = memoryStreamq.ToArray();
                //    memoryStreamq.Close();
                //}

                if (rangevalue || rangevalue2 || Arousal_GeneralState || Arousal_Stimuli || Arousal_Transition ||
                   Arousal_FactorOCD || Arousal_ClaimingFactor || Arousal_DipsDown || AROUSAL_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AROUSAL:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (rangevalue)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("0=====1=====2=====3=====4=====5=====6=====7=====8=====9=====10", NextHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);



                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.State of alertness during evaluation.:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["rangevalue"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (rangevalue2)
                    {

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("0=====1=====2=====3=====4=====5=====6=====7=====8=====9=====10", NextHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.General state of alertness.", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["rangevalue2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Arousal_GeneralState)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.General state of alertness.", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_GeneralState"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Arousal_Stimuli)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Responds to stimuli? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_Stimuli"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Arousal_Transition)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Maintainance Of Arousal During Transition. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Arousal_FactorOCD)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Alerting factor. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_FactorOCD"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);


                    }
                    if (Arousal_ClaimingFactor)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Calming factor. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_ClaimingFactor"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);


                    }
                    if (Arousal_DipsDown)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.When does your childs arousal dip down? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Arousal_DipsDown"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);


                    }
                    if (AROUSAL_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AROUSAL_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);


                    }

                }
                #endregion

                #region***********Affect***********
                bool Affect_RangeEmotion = false; if (ds.Tables[1].Rows[0]["Affect_RangeEmotion"].ToString().Trim().Length > 0) { Affect_RangeEmotion = true; }
                bool Affect_ExpressEmotion = false; if (ds.Tables[1].Rows[0]["Affect_ExpressEmotion"].ToString().Trim().Length > 0) { Affect_ExpressEmotion = true; }
                bool Affect_Environment = false; if (ds.Tables[1].Rows[0]["Affect_Environment"].ToString().Trim().Length > 0) { Affect_Environment = true; }
                bool Affect_Task = false; if (ds.Tables[1].Rows[0]["Affect_Task"].ToString().Trim().Length > 0) { Affect_Task = true; }
                bool Affect_Individual = false; if (ds.Tables[1].Rows[0]["Affect_Individual"].ToString().Trim().Length > 0) { Affect_Individual = true; }
                bool Affect_ThroughOut = false; if (ds.Tables[1].Rows[0]["Affect_ThroughOut"].ToString().Trim().Length > 0) { Affect_ThroughOut = true; }
                bool Affect_Charaterising = false; if (ds.Tables[1].Rows[0]["Affect_Charaterising"].ToString().Trim().Length > 0) { Affect_Charaterising = true; }
                bool Affect_cmt = false; if (ds.Tables[1].Rows[0]["Affect_cmt"].ToString().Trim().Length > 0) { Affect_cmt = true; }

                if (Affect_RangeEmotion || Affect_ExpressEmotion || Affect_Environment || Affect_Task || Affect_Individual || Affect_ThroughOut || Affect_Charaterising || Affect_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("AFFECT:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);


                    if (Affect_RangeEmotion)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Wide range of emotion :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_RangeEmotion"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Affect_ExpressEmotion)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Is the Child able to express emotion :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_ExpressEmotion"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Affect_Environment)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Affect appropriate to: Environment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Environment"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Affect_Task)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Affect appropriate to: Task :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Task"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Affect_Individual)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Affect appropriate to: Individual :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Individual"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Affect_ThroughOut)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Consistent emotion throughout :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_ThroughOut"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);



                    }
                    if (Affect_Charaterising)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Factors Characterising affect :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_Charaterising"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);



                    }
                    if (Affect_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Affect_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);



                    }
                }
                #endregion

                #region********Attention***********
                //bool Attention_Span = false; if (ds.Tables[1].Rows[0]["Attention_Span"].ToString().Trim().Length > 0) { Attention_Span = true; }
                bool Attention_FocusHandSchool = false; if (ds.Tables[1].Rows[0]["Attention_FocusHandSchool"].ToString().Trim().Length > 0) { Attention_FocusHandSchool = true; }
                bool Attention_FocusHandhome = false; if (ds.Tables[1].Rows[0]["Attention_FocusHandhome"].ToString().Trim().Length > 0) { Attention_FocusHandhome = true; }
                bool Attention_Dividing = false; if (ds.Tables[1].Rows[0]["Attention_Dividing"].ToString().Trim().Length > 0) { Attention_Dividing = true; }
                bool Attention_ChangeActivities = false; if (ds.Tables[1].Rows[0]["Attention_ChangeActivities"].ToString().Trim().Length > 0) { Attention_ChangeActivities = true; }
                bool Attention_AgeAppropriate = false; if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Trim().Length > 0) { Attention_AgeAppropriate = true; }
                bool Attention_AttentionSpan = false; if (ds.Tables[1].Rows[0]["Attention_AttentionSpan"].ToString().Trim().Length > 0) { Attention_AttentionSpan = true; }
                bool Attention_Distractibility = false; if (ds.Tables[1].Rows[0]["Attention_Distractibility"].ToString().Trim().Length > 0) { Attention_Distractibility = true; }
                bool Focal_Attention = false; if (ds.Tables[1].Rows[0]["Focal_Attention"].ToString().Trim().Length > 0) { Focal_Attention = true; }
                bool Joint_Attention = false; if (ds.Tables[1].Rows[0]["Joint_Attention"].ToString().Trim().Length > 0) { Joint_Attention = true; }
                bool Divided_Attention = false; if (ds.Tables[1].Rows[0]["Divided_Attention"].ToString().Trim().Length > 0) { Divided_Attention = true; }
                bool Alternating_Attention = false; if (ds.Tables[1].Rows[0]["Alternating_Attention"].ToString().Trim().Length > 0) { Alternating_Attention = true; }
                bool Sustained_Attention = false; if (ds.Tables[1].Rows[0]["Sustained_Attention"].ToString().Trim().Length > 0) { Sustained_Attention = true; }
                bool Attention_move = false; if (ds.Tables[1].Rows[0]["Attention_move"].ToString().Trim().Length > 0) { Attention_move = true; }
                bool ATTENTION_cmt = false; if (ds.Tables[1].Rows[0]["ATTENTION_cmt"].ToString().Trim().Length > 0) { ATTENTION_cmt = true; }

                if (/*Attention_Span ||*/ Attention_FocusHandSchool || Attention_FocusHandhome || Attention_Dividing || Attention_ChangeActivities || Attention_AgeAppropriate ||
                    Attention_AttentionSpan || Attention_Distractibility || Focal_Attention || Joint_Attention || Divided_Attention || Alternating_Attention || Sustained_Attention || Attention_move || ATTENTION_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("ATTENTION:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (Attention_AttentionSpan)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Attention span :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Attention_FocusHandhome)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Focus task at hand-Home :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_FocusHandhome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);




                    }
                    if (Attention_FocusHandSchool)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Focus task at hand-School :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_FocusHandSchool"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);




                    }
                    if (Attention_Dividing)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Dividing attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);




                    }
                    if (Attention_ChangeActivities)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Change of activities every :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);




                    }
                    if (Attention_AgeAppropriate)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Age appropriate attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);




                    }
                    if (Attention_Distractibility)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Factors of distractibility :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_Distractibility"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Focal_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.Focal Attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Focal_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Joint_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("9.Joint Attention:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Joint_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Divided_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("10.Divided Attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Divided_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Alternating_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("11.Alternating Attention :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Alternating_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Sustained_Attention)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("12.Sustained Attention:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sustained_Attention"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Attention_move)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("13.Does the child move from one activity to another continuously? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Attention_move"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (ATTENTION_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ATTENTION_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                }
                #endregion

                #region********Action***********
                bool Action_MotorPlanning = false; if (ds.Tables[1].Rows[0]["Action_MotorPlanning"].ToString().Trim().Length > 0) { Action_MotorPlanning = true; }
                bool Action_Purposeful = false; if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Trim().Length > 0) { Action_Purposeful = true; }
                bool Action_GoalOriented = false; if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Trim().Length > 0) { Action_GoalOriented = true; }
                bool Action_FeedBackDependent = false; if (ds.Tables[1].Rows[0]["Affect_Task"].ToString().Trim().Length > 0) { Action_FeedBackDependent = true; }
                bool Action_Constructive = false; if (ds.Tables[1].Rows[0]["Action_Constructive"].ToString().Trim().Length > 0) { Action_Constructive = true; }
                bool Action_cmt = false; if (ds.Tables[1].Rows[0]["Action_cmt"].ToString().Trim().Length > 0) { Action_cmt = true; }

                if (Action_MotorPlanning || Action_Purposeful || Action_GoalOriented || Action_FeedBackDependent || Action_Constructive || Action_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("ACTION:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (Action_MotorPlanning)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("1.Age appropriate Motor planning :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_MotorPlanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Action_Purposeful)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Purposeful :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Action_GoalOriented)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Goal Oriented :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Action_FeedBackDependent)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Feedback Dependent :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_FeedBackDependent"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Action_Constructive)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Constructive? :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_Constructive"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Action_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comment :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Action_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                }

                #endregion

                #region**********Interaction**********
                //bool Interaction_KnowPeople = false; if (ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString().Trim().Length > 0) { Interaction_KnowPeople = true; }
                //bool Interaction_Strangers = false; if (ds.Tables[1].Rows[0]["Interaction_Strangers"].ToString().Trim().Length > 0) { Interaction_Strangers = true; }
                bool Interacts = false; if (ds.Tables[1].Rows[0]["Interacts"].ToString().Trim().Length > 0) { Interacts = true; }
                bool Does_not_initiate = false; if (ds.Tables[1].Rows[0]["Does_not_initiate"].ToString().Trim().Length > 0) { Does_not_initiate = true; }
                bool Sustain = false; if (ds.Tables[1].Rows[0]["Sustain"].ToString().Trim().Length > 0) { Sustain = true; }
                bool Fight = false; if (ds.Tables[1].Rows[0]["Fight"].ToString().Trim().Length > 0) { Fight = true; }
                bool Freeze = false; if (ds.Tables[1].Rows[0]["Freeze"].ToString().Trim().Length > 0) { Freeze = true; }
                bool Fright = false; if (ds.Tables[1].Rows[0]["Fright"].ToString().Trim().Length > 0) { Fright = true; }


                bool Anxious = false; if (ds.Tables[1].Rows[0]["Anxious"].ToString().Trim().Length > 0) { Anxious = true; }
                bool Comfortable = false; if (ds.Tables[1].Rows[0]["Comfortable"].ToString().Trim().Length > 0) { Comfortable = true; }
                bool Nervous = false; if (ds.Tables[1].Rows[0]["Nervous"].ToString().Trim().Length > 0) { Nervous = true; }
                bool ANS_response = false; if (ds.Tables[1].Rows[0]["ANS_response"].ToString().Trim().Length > 0) { ANS_response = true; }
                bool OTHERS = false; if (ds.Tables[1].Rows[0]["OTHERS"].ToString().Trim().Length > 0) { OTHERS = true; }
                bool Interactcomments = false; if (ds.Tables[1].Rows[0]["cmtgathering"].ToString().Trim().Length > 0) { Interactcomments = true; }


                bool Interaction_SocialQues = false; if (ds.Tables[1].Rows[0]["Interaction_SocialQues"].ToString().Trim().Length > 0) { Interaction_SocialQues = true; }
                bool Interaction_Happiness = false; if (ds.Tables[1].Rows[0]["Interaction_Happiness"].ToString().Trim().Length > 0) { Interaction_Happiness = true; }
                bool Interaction_Sadness = false; if (ds.Tables[1].Rows[0]["Interaction_Sadness"].ToString().Trim().Length > 0) { Interaction_Sadness = true; }
                bool Interaction_Surprise = false; if (ds.Tables[1].Rows[0]["Interaction_Surprise"].ToString().Trim().Length > 0) { Interaction_Surprise = true; }
                bool Interaction_Shock = false; if (ds.Tables[1].Rows[0]["Interaction_Shock"].ToString().Trim().Length > 0) { Interaction_Shock = true; }
                bool Interaction_Friends = false; if (ds.Tables[1].Rows[0]["Interaction_Friends"].ToString().Trim().Length > 0) { Interaction_Friends = true; }
                //bool Interaction_RelatesPeople = false; if (ds.Tables[1].Rows[0]["Interaction_RelatesPeople"].ToString().Trim().Length > 0) { Interaction_RelatesPeople = true; }
                bool Interaction_Enjoy = false; if (ds.Tables[1].Rows[0]["Interaction_Enjoy"].ToString().Trim().Length > 0) { Interaction_Enjoy = true; }
                bool Interaction_Comments = false; if (ds.Tables[1].Rows[0]["INTERACTION_cmt"].ToString().Trim().Length > 0) { Interaction_Comments = true; }

                if (/*Interaction_KnowPeople*/  /*Interaction_Strangers ||*/ Interacts || Does_not_initiate || Sustain || Fight || Freeze || Fright || Anxious || Comfortable || Nervous || ANS_response ||
                  OTHERS || Interaction_SocialQues ||
                    Interaction_Happiness || Interaction_Sadness || Interaction_Surprise || Interaction_Shock || Interaction_Friends || /*Interaction_RelatesPeople ||*/ Interaction_Enjoy)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("INTERACTION:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    //if (Interaction_KnowPeople)
                    //{
                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("Interaction during social gathering. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(1);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 10f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 60f;
                    //    table.AddCell(cell);
                    //    document.Add(table);
                    //    // document.Add(Chunk.NEXTPAGE);
                    //}
                    if (Interacts)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("a).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interacts"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Does_not_initiate)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("b).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Does_not_initiate"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Sustain)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("c).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Sustain"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Fight)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("d).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Fight"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Freeze)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("e).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Freeze"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Fright)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("f).Interaction during Social Gathering :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Fright"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Anxious)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("g).Interaction during Social Gathering ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Anxious"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Comfortable)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("h).Interaction during Social Gathering ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comfortable"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Nervous)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("i).Interaction during Social Gathering ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Nervous"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (ANS_response)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("j).Interaction during Social Gathering ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ANS_response"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (OTHERS)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("k).Interaction during Social Gathering ", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OTHERS"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interactcomments)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["cmtgathering"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_SocialQues)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("2.Understands/Appreciates social cues. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_SocialQues"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Happiness)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("3.Reaction to emotion of other Happiness :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Happiness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Sadness)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("4.Reaction to emotion Sadness :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Sadness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Surprise)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("5.Reaction to emotion Surprise :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Surprise"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Shock)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("6.Reaction to emotion Shock :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Shock"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Friends)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("7.Friendship : can make friends :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Friends"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Interaction_Enjoy)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("8.What Activities Does He/She Enjoys. :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Interaction_Enjoy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                    if (Interaction_Comments)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["INTERACTION_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }

                }


                #endregion

                #region*********SYSTEM EVALUATION**************************




                bool SE1 = false;
                bool TS_Registration = false; if (ds.Tables[1].Rows[0]["TS_Registration"].ToString().Trim().Length > 0) { TS_Registration = true; }
                bool TS_Orientation = false; if (ds.Tables[1].Rows[0]["TS_Orientation"].ToString().Trim().Length > 0) { TS_Orientation = true; }
                bool TS_Discrimination = false; if (ds.Tables[1].Rows[0]["TS_Discrimination"].ToString().Trim().Length > 0) { TS_Discrimination = true; }
                bool TS_Responsiveness = false; if (ds.Tables[1].Rows[0]["TS_Responsiveness"].ToString().Trim().Length > 0) { TS_Responsiveness = true; }
                if (TS_Registration || TS_Orientation || TS_Discrimination || TS_Responsiveness)
                {
                    SE1 = true;
                }

                bool SE2 = false;
                bool SS_Bodyawareness = false; if (ds.Tables[1].Rows[0]["SS_Bodyawareness"].ToString().Trim().Length > 0) { SS_Bodyawareness = true; }
                bool SS_Bodyschema = false; if (ds.Tables[1].Rows[0]["SS_Bodyawareness"].ToString().Trim().Length > 0) { SS_Bodyschema = true; }
                bool SS_Orientation = false; if (ds.Tables[1].Rows[0]["SS_Bodyawareness"].ToString().Trim().Length > 0) { SS_Orientation = true; }
                bool SS_Posterior = false; if (ds.Tables[1].Rows[0]["SS_Posterior"].ToString().Trim().Length > 0) { SS_Posterior = true; }
                bool SS_Bilateral = false; if (ds.Tables[1].Rows[0]["SS_Bilateral"].ToString().Trim().Length > 0) { SS_Bilateral = true; }
                bool SS_Balance = false; if (ds.Tables[1].Rows[0]["SS_Balance"].ToString().Trim().Length > 0) { SS_Balance = true; }
                bool SS_Dominance = false; if (ds.Tables[1].Rows[0]["SS_Dominance"].ToString().Trim().Length > 0) { SS_Dominance = true; }
                bool SS_Right = false; if (ds.Tables[1].Rows[0]["SS_Right"].ToString().Trim().Length > 0) { SS_Right = true; }
                bool SS_identifies = false; if (ds.Tables[1].Rows[0]["SS_identifies"].ToString().Trim().Length > 0) { SS_identifies = true; }
                bool SS_point = false; if (ds.Tables[1].Rows[0]["SS_point"].ToString().Trim().Length > 0) { SS_point = true; }
                bool SS_Constantly = false; if (ds.Tables[1].Rows[0]["SS_Constantly"].ToString().Trim().Length > 0) { SS_Constantly = true; }
                bool SS_clumsy = false; if (ds.Tables[1].Rows[0]["SS_clumsy"].ToString().Trim().Length > 0) { SS_clumsy = true; }
                bool SS_maneuver = false; if (ds.Tables[1].Rows[0]["SS_maneuver"].ToString().Trim().Length > 0) { SS_maneuver = true; }
                bool SS_overly = false; if (ds.Tables[1].Rows[0]["SS_overly"].ToString().Trim().Length > 0) { SS_overly = true; }
                bool SS_stand = false; if (ds.Tables[1].Rows[0]["SS_stand"].ToString().Trim().Length > 0) { SS_stand = true; }
                bool SS_indulge = false; if (ds.Tables[1].Rows[0]["SS_indulge"].ToString().Trim().Length > 0) { SS_indulge = true; }
                bool SS_textures = false; if (ds.Tables[1].Rows[0]["SS_textures"].ToString().Trim().Length > 0) { SS_textures = true; }
                bool SS_monkey = false; if (ds.Tables[1].Rows[0]["SS_monkey"].ToString().Trim().Length > 0) { SS_monkey = true; }
                bool SS_swings = false; if (ds.Tables[1].Rows[0]["SS_monkey"].ToString().Trim().Length > 0) { SS_swings = true; }
                if (SS_Bodyawareness || SS_Bodyschema || SS_Orientation || SS_Posterior || SS_Bilateral || SS_Balance || SS_Dominance || SS_Right || SS_identifies || SS_point || SS_Constantly
                    || SS_clumsy || SS_maneuver || SS_overly || SS_stand || SS_indulge || SS_textures || SS_monkey || SS_swings)
                {
                    SE2 = true;
                }
                bool SE3 = false;
                bool VM_Registration = false; if (ds.Tables[1].Rows[0]["VM_Registration"].ToString().Trim().Length > 0) { VM_Registration = true; }
                bool VM_Orientation = false; if (ds.Tables[1].Rows[0]["VM_Orientation"].ToString().Trim().Length > 0) { VM_Orientation = true; }
                bool VM_Discrimination = false; if (ds.Tables[1].Rows[0]["VM_Discrimination"].ToString().Trim().Length > 0) { VM_Discrimination = true; }
                bool VM_Responsiveness = false; if (ds.Tables[1].Rows[0]["VM_Responsiveness"].ToString().Trim().Length > 0) { VM_Responsiveness = true; }
                if (VM_Registration || VM_Orientation || VM_Discrimination || VM_Responsiveness)
                {
                    SE3 = true;
                }

                bool SE4 = false;
                bool PS_Registration = false; if (ds.Tables[1].Rows[0]["PS_Registration"].ToString().Trim().Length > 0) { PS_Registration = true; }
                bool PS_Gradation = false; if (ds.Tables[1].Rows[0]["PS_Gradation"].ToString().Trim().Length > 0) { PS_Gradation = true; }
                bool PS_Discrimination = false; if (ds.Tables[1].Rows[0]["PS_Discrimination"].ToString().Trim().Length > 0) { PS_Discrimination = true; }
                bool PS_Responsiveness = false; if (ds.Tables[1].Rows[0]["PS_Responsiveness"].ToString().Trim().Length > 0) { PS_Responsiveness = true; }
                if (PS_Registration || PS_Gradation || PS_Discrimination || PS_Responsiveness)
                {
                    SE4 = true;
                }

                bool SE5 = false;
                bool OM_Registration = false; if (ds.Tables[1].Rows[0]["OM_Registration"].ToString().Trim().Length > 0) { OM_Registration = true; }
                bool OM_Orientation = false; if (ds.Tables[1].Rows[0]["OM_Orientation"].ToString().Trim().Length > 0) { OM_Orientation = true; }
                bool OM_Discrimination = false; if (ds.Tables[1].Rows[0]["OM_Discrimination"].ToString().Trim().Length > 0) { OM_Discrimination = true; }
                bool OM_Responsiveness = false; if (ds.Tables[1].Rows[0]["OM_Responsiveness"].ToString().Trim().Length > 0) { OM_Responsiveness = true; }
                if (OM_Registration || OM_Orientation || OM_Discrimination || OM_Responsiveness)
                {
                    SE5 = true;
                }
                bool SE6 = false;
                bool AS_Auditory = false; if (ds.Tables[1].Rows[0]["AS_Auditory"].ToString().Trim().Length > 0) { AS_Auditory = true; }
                bool AS_Orientation = false; if (ds.Tables[1].Rows[0]["AS_Orientation"].ToString().Trim().Length > 0) { AS_Orientation = true; }
                bool AS_Responsiveness = false; if (ds.Tables[1].Rows[0]["AS_Responsiveness"].ToString().Trim().Length > 0) { AS_Responsiveness = true; }
                bool AS_discrimination = false; if (ds.Tables[1].Rows[0]["AS_discrimination"].ToString().Trim().Length > 0) { AS_discrimination = true; }
                bool AS_Background = false; if (ds.Tables[1].Rows[0]["AS_Background"].ToString().Trim().Length > 0) { AS_Background = true; }
                bool AS_localization = false; if (ds.Tables[1].Rows[0]["AS_localization"].ToString().Trim().Length > 0) { AS_localization = true; }
                bool AS_Analysis = false; if (ds.Tables[1].Rows[0]["AS_Analysis"].ToString().Trim().Length > 0) { AS_Analysis = true; }
                bool AS_sequencing = false; if (ds.Tables[1].Rows[0]["AS_sequencing"].ToString().Trim().Length > 0) { AS_sequencing = true; }
                bool AS_blending = false; if (ds.Tables[1].Rows[0]["AS_blending"].ToString().Trim().Length > 0) { AS_blending = true; }
                if (AS_Auditory || AS_Orientation || AS_Responsiveness || AS_discrimination || AS_Background || AS_localization || AS_Analysis || AS_sequencing || AS_blending)
                {
                    SE6 = true;
                }
                bool SE7 = false;
                bool VS_Visual = false; if (ds.Tables[1].Rows[0]["VS_Visual"].ToString().Trim().Length > 0) { VS_Visual = true; }
                bool VS_Responsiveness = false; if (ds.Tables[1].Rows[0]["VS_Responsiveness"].ToString().Trim().Length > 0) { VS_Responsiveness = true; }
                bool VS_scanning = false; if (ds.Tables[1].Rows[0]["VS_scanning"].ToString().Trim().Length > 0) { VS_scanning = true; }
                bool VS_constancy = false; if (ds.Tables[1].Rows[0]["VS_constancy"].ToString().Trim().Length > 0) { VS_constancy = true; }
                bool VS_memory = false; if (ds.Tables[1].Rows[0]["VS_memory"].ToString().Trim().Length > 0) { VS_memory = true; }
                bool VS_Perception = false; if (ds.Tables[1].Rows[0]["VS_Perception"].ToString().Trim().Length > 0) { VS_Perception = true; }
                bool VS_hand = false; if (ds.Tables[1].Rows[0]["VS_hand"].ToString().Trim().Length > 0) { VS_hand = true; }
                bool VS_foot = false; if (ds.Tables[1].Rows[0]["VS_foot"].ToString().Trim().Length > 0) { VS_foot = true; }
                bool VS_discrimination = false; if (ds.Tables[1].Rows[0]["VS_discrimination"].ToString().Trim().Length > 0) { VS_discrimination = true; }
                bool VS_closure = false; if (ds.Tables[1].Rows[0]["VS_closure"].ToString().Trim().Length > 0) { VS_closure = true; }
                bool VS_Figureground = false; if (ds.Tables[1].Rows[0]["VS_Figureground"].ToString().Trim().Length > 0) { VS_Figureground = true; }
                bool VS_Visualmemory = false; if (ds.Tables[1].Rows[0]["VS_Visualmemory"].ToString().Trim().Length > 0) { VS_Visualmemory = true; }
                bool VS_sequential = false; if (ds.Tables[1].Rows[0]["VS_sequential"].ToString().Trim().Length > 0) { VS_sequential = true; }
                bool VS_spatial = false; if (ds.Tables[1].Rows[0]["VS_sequential"].ToString().Trim().Length > 0) { VS_spatial = true; }
                if (VS_Visual || VS_Responsiveness || VS_scanning || VS_constancy || VS_memory || VS_Perception || VS_hand || VS_foot || VS_discrimination || VS_closure ||
                    VS_Figureground || VS_Visualmemory || VS_sequential || VS_spatial)
                {
                    SE7 = true;
                }
                bool SE8 = false;
                bool OS_Registration = false; if (ds.Tables[1].Rows[0]["OS_Registration"].ToString().Trim().Length > 0) { OS_Registration = true; }
                bool OS_Orientation = false; if (ds.Tables[1].Rows[0]["OS_Orientation"].ToString().Trim().Length > 0) { OS_Orientation = true; }
                bool OS_Discrimination = false; if (ds.Tables[1].Rows[0]["OS_Discrimination"].ToString().Trim().Length > 0) { OS_Discrimination = true; }
                bool OS_Responsiveness = false; if (ds.Tables[1].Rows[0]["OS_Responsiveness"].ToString().Trim().Length > 0) { OS_Responsiveness = true; }

                if (OS_Registration || OS_Orientation || OS_Discrimination || OS_Responsiveness)
                {
                    SE8 = true;
                }
                if (SE1 || SE2 || SE3 || SE4 || SE5 || SE6 || SE7 || SE8)
                {

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("SYSTEM EVALUATION ", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region ********************** SE1 ***************************
                    if (SE1)
                    {

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("TACTILE SYSTEM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;


                        if (TS_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TS_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);


                        }

                        if (TS_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Orientation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TS_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (TS_Discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TS_Discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (TS_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(" Responsiveness ( Hyper responsive/Hyporesponsive ) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["TS_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                        //document.Add(Chunk.NEXTPAGE);
                    }
                    #endregion
                    #region ************** SE2  ***************************
                    if (SE2)
                    //    table = new PdfPTable(2);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;
                    //    cell = new PdfPCell(PhraseCell(new Phrase("SOMATOSENSORY SYSTEM- ( Tactile-Vestibular - Prop Trio):", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    //    cell.PaddingBottom = 3f;
                    //    cell.PaddingLeft = 30f;
                    //    table.AddCell(cell);
                    //    document.Add(table);

                    //    table = new PdfPTable(2);
                    //    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    table.WidthPercentage = 100;
                    //    table.SpacingBefore = 20f;

                    //cell = new PdfPCell(PhraseCell(new Phrase("SOMATOSENSORY SYSTEM- ( Tactile-Vestibular - Prop Trio):", NormalFont), PdfPCell.ALIGN_LEFT));
                    //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    //cell.BorderWidthLeft = 0.3f;
                    //cell.BorderWidthTop = 0.3f;
                    //cell.Padding = 5;
                    //table.AddCell(cell);

                    //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                    //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    //cell.BorderWidthLeft = 0.3f;
                    //cell.BorderWidthTop = 0.3f;
                    //cell.Padding = 5;
                    //table.AddCell(cell);
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("SOMATOSENSORY SYSTEM- ( Tactile-Vestibular - Prop Trio):", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;


                        if (SS_Bodyawareness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Body awareness", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Bodyawareness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Bodyschema)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(" Body schema", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Bodyschema"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Orientation of body in space", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Posterior)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Posterior space awareness", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Posterior"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Bilateral)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Bilateral Coordination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Bilateral"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Balance)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Balance on static and dynamic surfaces", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Balance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Dominance)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Dominance", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Dominance"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Right)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Right and Left Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Right"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_identifies)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("How well he identifies body parts", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_identifies"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_point)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Can name and point objects/ people", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_point"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_Constantly)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Constantly bumps into objects in his path", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_Constantly"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_clumsy)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Is he clumsy with his things", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_clumsy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_maneuver)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Can he maneuver himself out ofa variety of equipment orsituations?", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_maneuver"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_overly)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("  Is he overly fidgety?", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_overly"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_stand)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Is he able to stand in line duringor waits for his turn", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_stand"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_indulge)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Does he indulge into rough/sportplay?", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_indulge"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_textures)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Does he dislike any type of textures?", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_textures"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_monkey)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Use of monkey ladders, obstacle course (climbing up and crossing) Commando crawl", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_monkey"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (SS_swings)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Use of swings Slide Can he perform heavy activities Cycle/tricycle Riding Can he maintain good posture while sitting?", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SS_swings"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region*********SE3***************
                    if (SE3)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("VESTIBULAR SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("VESTIBULAR SYSTEM:", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("VESTIBULAR SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        if (VM_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VM_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VM_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Orientation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VM_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VM_Discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VM_Discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VM_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Responsiveness ( Hyporesponsive /Hyperresponsive ) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VM_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region*********SE4***************
                    if (SE4)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("PROPRIOCEPTIVE SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("PROPRIOCEPTIVE SYSTEM:", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("PROPRIOCEPTIVE SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        //document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        if (PS_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PS_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (PS_Gradation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Gradation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PS_Gradation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (PS_Discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PS_Discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (PS_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(" Responsiveness Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["PS_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region*******SE5 *************
                    if (SE5)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("ORO- MOTOR SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("ORO- MOTOR SYSTEM:", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("ORO- MOTOR SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        if (OM_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OM_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OM_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Orientation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OM_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OM_Discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OM_Discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OM_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase(" Responsiveness(Hyporesponsive /Hyperresponsive ) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OM_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region ****************SE6 ***************
                    if (SE6)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("AUDITORY SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("AUDITORY SYSTEM:", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("AUDITORY SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        if (AS_Auditory)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_Auditory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory Orientation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Responsiveness(Hyporesponsive/ Hyperresponsive) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_Background)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Background-foreground discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_Background"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_localization)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory localization", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_localization"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_Analysis)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Analysis and synthesis", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_Analysis"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_sequencing)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory memory and sequencing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_sequencing"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (AS_blending)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Auditory blending (breaking of sounds)", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AS_blending"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }

                        document.Add(table);
                    }
                    #endregion
                    #region**********SE7******************
                    if (SE7)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("VISUAL SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("VISUAL SYSTEM:", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("VISUAL SYSTEM:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;


                        if (VS_Visual)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual Localization and Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_Visual"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Responsiveness(Hyporesponsive/Hyperresponsive ) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_scanning)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual scanning", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_scanning"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_constancy)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual constancy", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_constancy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_memory)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual memory", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_memory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_Perception)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual Perception", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_Perception"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_hand)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Eye hand Co- ordination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_hand"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_foot)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Eye foot Co- ordination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_foot"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_closure)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual closure", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_closure"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_Figureground)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Figure-ground discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_Figureground"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_sequential)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual sequential memory", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_sequential"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (VS_spatial)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Visual spatial relationships", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["VS_spatial"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }
                    #endregion
                    #region**********SE8******************
                    if (SE8)
                    {
                        //    table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;
                        //cell = new PdfPCell(PhraseCell(new Phrase("OLFACTORY SYSTEM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        //cell.PaddingBottom = 3f;
                        //cell.PaddingLeft = 30f;
                        //table.AddCell(cell);
                        //document.Add(table);

                        //table = new PdfPTable(2);
                        //table.HorizontalAlignment = Element.ALIGN_LEFT;
                        //table.WidthPercentage = 100;
                        //table.SpacingBefore = 20f;

                        //cell = new PdfPCell(PhraseCell(new Phrase("OLFACTORY SYSTEM", NormalFont), PdfPCell.ALIGN_LEFT));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_CENTER));
                        //cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        //cell.BorderWidthLeft = 0.3f;
                        //cell.BorderWidthTop = 0.3f;
                        //cell.Padding = 5;
                        //table.AddCell(cell);


                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("OLFACTORY SYSTEM :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        if (OS_Registration)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Registration", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OS_Registration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OS_Orientation)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Orientation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OS_Orientation"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OS_Discrimination)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Discrimination", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OS_Discrimination"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (OS_Responsiveness)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Responsiveness(Hyporesponsive/Hyperresponsive ) Mention the Behavioral responses shown by the child", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["OS_Responsiveness"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
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
                    DataTable dt = ds.Tables[2] as DataTable;
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Seeking", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Avoiding", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Sensitivity", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Registration", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" General", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Auditory", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Visual", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Touch", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Movement", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("ORAL Oral", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase(" Behavioral", NormalFont), PdfPCell.ALIGN_LEFT));
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
                            cell.Colspan = 3; // Span the cell across all three columns
                            table.AddCell(cell);

                            // Create a cell for the "Comments_2" content spanning all columns
                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Comments_2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            cell.Colspan = 3; // Span the cell across all three columns
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

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Auditory_3"].ToString() + "/50", NormalFont), PdfPCell.ALIGN_LEFT));
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

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["SPchild_Oral_3"].ToString() + "/35", NormalFont), PdfPCell.ALIGN_LEFT));
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

                #region************DCDQ*********************
                bool DCDQ = false;
                bool DCDQ_Throws1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Throws1"].ToString().Trim().Length > 0) { DCDQ_Throws1 = true; }
                bool DCDQ_Throws2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Throws2"].ToString().Trim().Length > 0) { DCDQ_Throws2 = true; }
                bool DCDQ_Throws3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Throws3"].ToString().Trim().Length > 0) { DCDQ_Throws3 = true; }
                bool DCDQ_Catches1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Catches1"].ToString().Trim().Length > 0) { DCDQ_Catches1 = true; }
                bool DCDQ_Catches2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Catches2"].ToString().Trim().Length > 0) { DCDQ_Catches2 = true; }
                bool DCDQ_Catches3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Catches3"].ToString().Trim().Length > 0) { DCDQ_Catches3 = true; }
                bool DCDQ_Hits1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Hits1"].ToString().Trim().Length > 0) { DCDQ_Hits1 = true; }
                bool DCDQ_Hits2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Hits2"].ToString().Trim().Length > 0) { DCDQ_Hits2 = true; }
                bool DCDQ_Hits3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Hits3"].ToString().Trim().Length > 0) { DCDQ_Hits3 = true; }
                bool DCDQ_Jumps1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Jumps1"].ToString().Trim().Length > 0) { DCDQ_Jumps1 = true; }
                bool DCDQ_Jumps2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Jumps2"].ToString().Trim().Length > 0) { DCDQ_Jumps2 = true; }
                bool DCDQ_Jumps3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Jumps3"].ToString().Trim().Length > 0) { DCDQ_Jumps3 = true; }
                bool DCDQ_Runs1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Runs1"].ToString().Trim().Length > 0) { DCDQ_Runs1 = true; }
                bool DCDQ_Runs2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Runs2"].ToString().Trim().Length > 0) { DCDQ_Runs2 = true; }
                bool DCDQ_Runs3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Runs3"].ToString().Trim().Length > 0) { DCDQ_Runs3 = true; }
                bool DCDQ_Plans1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Plans1"].ToString().Trim().Length > 0) { DCDQ_Plans1 = true; }
                bool DCDQ_Plans2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Plans2"].ToString().Trim().Length > 0) { DCDQ_Plans2 = true; }
                bool DCDQ_Plans3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Plans3"].ToString().Trim().Length > 0) { DCDQ_Plans3 = true; }
                bool DCDQ_Writing1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Writing1"].ToString().Trim().Length > 0) { DCDQ_Writing1 = true; }
                bool DCDQ_Writing2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Writing1"].ToString().Trim().Length > 0) { DCDQ_Writing2 = true; }
                bool DCDQ_Writing3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Writing3"].ToString().Trim().Length > 0) { DCDQ_Writing3 = true; }
                bool DCDQ_legibly1 = false; if (ds.Tables[1].Rows[0]["DCDQ_legibly1"].ToString().Trim().Length > 0) { DCDQ_legibly1 = true; }
                bool DCDQ_legibly2 = false; if (ds.Tables[1].Rows[0]["DCDQ_legibly2"].ToString().Trim().Length > 0) { DCDQ_legibly2 = true; }
                bool DCDQ_legibly3 = false; if (ds.Tables[1].Rows[0]["DCDQ_legibly3"].ToString().Trim().Length > 0) { DCDQ_legibly3 = true; }
                bool DCDQ_Effort1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Effort1"].ToString().Trim().Length > 0) { DCDQ_Effort1 = true; }
                bool DCDQ_Effort2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Effort2"].ToString().Trim().Length > 0) { DCDQ_Effort2 = true; }
                bool DCDQ_Effort3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Effort3"].ToString().Trim().Length > 0) { DCDQ_Effort3 = true; }
                bool DCDQ_Cuts1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Cuts1"].ToString().Trim().Length > 0) { DCDQ_Cuts1 = true; }
                bool DCDQ_Cuts2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Cuts2"].ToString().Trim().Length > 0) { DCDQ_Cuts2 = true; }
                bool DCDQ_Cuts3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Cuts3"].ToString().Trim().Length > 0) { DCDQ_Cuts3 = true; }
                bool DCDQ_Likes1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Likes1"].ToString().Trim().Length > 0) { DCDQ_Likes1 = true; }
                bool DCDQ_Likes2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Likes2"].ToString().Trim().Length > 0) { DCDQ_Likes2 = true; }
                bool DCDQ_Likes3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Likes3"].ToString().Trim().Length > 0) { DCDQ_Likes3 = true; }
                bool DCDQ_Learning1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Learning1"].ToString().Trim().Length > 0) { DCDQ_Learning1 = true; }
                bool DCDQ_Learning2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Learning2"].ToString().Trim().Length > 0) { DCDQ_Learning2 = true; }
                bool DCDQ_Learning3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Learning3"].ToString().Trim().Length > 0) { DCDQ_Learning3 = true; }
                bool DCDQ_Quick1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Quick1"].ToString().Trim().Length > 0) { DCDQ_Quick1 = true; }
                bool DCDQ_Quick2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Quick2"].ToString().Trim().Length > 0) { DCDQ_Quick2 = true; }
                bool DCDQ_Quick3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Quick3"].ToString().Trim().Length > 0) { DCDQ_Quick3 = true; }
                bool DCDQ_Bull1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Bull1"].ToString().Trim().Length > 0) { DCDQ_Bull1 = true; }
                bool DCDQ_Bull2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Bull2"].ToString().Trim().Length > 0) { DCDQ_Bull2 = true; }
                bool DCDQ_Bull3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Bull3"].ToString().Trim().Length > 0) { DCDQ_Bull3 = true; }
                bool DCDQ_Does1 = false; if (ds.Tables[1].Rows[0]["DCDQ_Does1"].ToString().Trim().Length > 0) { DCDQ_Does1 = true; }
                bool DCDQ_Does2 = false; if (ds.Tables[1].Rows[0]["DCDQ_Does2"].ToString().Trim().Length > 0) { DCDQ_Does2 = true; }
                bool DCDQ_Does3 = false; if (ds.Tables[1].Rows[0]["DCDQ_Does3"].ToString().Trim().Length > 0) { DCDQ_Does3 = true; }

                bool DCDQ_INTERPRETATION = false; if (ds.Tables[1].Rows[0]["DCDQ_INTERPRETATION"].ToString().Trim().Length > 0) { DCDQ_INTERPRETATION = true; }

                if (DCDQ_Throws1 || DCDQ_Throws2 || DCDQ_Throws3 || DCDQ_Catches1 || DCDQ_Catches2 || DCDQ_Catches3 || DCDQ_Hits1 || DCDQ_Hits2 || DCDQ_Hits3 ||
                    DCDQ_Jumps1 || DCDQ_Jumps2 || DCDQ_Jumps3 || DCDQ_Runs1 || DCDQ_Runs2 || DCDQ_Runs3 || DCDQ_Plans1 || DCDQ_Plans2 || DCDQ_Plans3 || DCDQ_Writing1 || DCDQ_Writing2 || DCDQ_Writing3 || DCDQ_legibly1 || DCDQ_legibly2 || DCDQ_legibly3 || DCDQ_Effort1 || DCDQ_Effort2 || DCDQ_Effort3 || DCDQ_Cuts1 || DCDQ_Cuts2 || DCDQ_Cuts3 || DCDQ_Likes1 || DCDQ_Likes2 || DCDQ_Likes3 || DCDQ_Learning1 || DCDQ_Learning2 || DCDQ_Learning3 || DCDQ_Quick1 || DCDQ_Quick2 || DCDQ_Quick3 || DCDQ_Bull1 || DCDQ_Bull2 || DCDQ_Bull3 || DCDQ_Does1 || DCDQ_Does2 || DCDQ_Does3 || DCDQ_INTERPRETATION)
                {
                    DCDQ = true;
                }
                bool DCDQ1 = false;
                bool DCDQ_Control = false; if (ds.Tables[1].Rows[0]["DCDQ_Control"].ToString().Trim().Length > 0) { DCDQ_Control = true; }
                bool DCDQ_Fine = false; if (ds.Tables[1].Rows[0]["DCDQ_Fine"].ToString().Trim().Length > 0) { DCDQ_Fine = true; }
                bool DCDQ_General = false; if (ds.Tables[1].Rows[0]["DCDQ_General"].ToString().Trim().Length > 0) { DCDQ_General = true; }
                bool DCDQ_Total = false; if (ds.Tables[1].Rows[0]["DCDQ_Total"].ToString().Trim().Length > 0) { DCDQ_Total = true; }
                bool DCDQ_COMMENT = false; if (ds.Tables[1].Rows[0]["DCDQ_COMMENT"].ToString().Trim().Length > 0) { DCDQ_COMMENT = true; }

                if (DCDQ_Control || DCDQ_Fine || DCDQ_General || DCDQ_Total || DCDQ_COMMENT)
                {
                    DCDQ1 = true;
                }



                if (DCDQ)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("DCDQ :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 3;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region ********************** DCDQ ***************************

                    if (DCDQ)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("DCDQ :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;

                        cell = new PdfPCell(PhraseCell(new Phrase("List", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);


                        cell = new PdfPCell(PhraseCell(new Phrase("Control During Movement", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Fine Motor/Handwriting", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("General Coordination", NormalFont), PdfPCell.ALIGN_CENTER));
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthTop = 0.3f;
                        cell.Padding = 5;
                        table.AddCell(cell);

                        if (DCDQ_Throws1 || DCDQ_Throws2 || DCDQ_Throws3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("1. Throws ball", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Throws1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Throws2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Throws3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Catches1 || DCDQ_Catches2 || DCDQ_Catches3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("2.Catches ball", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Catches1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Catches2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Catches3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Hits1 || DCDQ_Hits2 || DCDQ_Hits3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("3.Hits ball/birdie", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Hits1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Hits2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Hits3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Jumps1 || DCDQ_Jumps2 || DCDQ_Jumps3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("4.Jumps ove", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Jumps1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Jumps2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Jumps3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Runs1 || DCDQ_Runs2 || DCDQ_Runs3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("5. Runs", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Runs1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Runs2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Runs3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Plans1 || DCDQ_Plans2 || DCDQ_Plans3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("6. Plans activity", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Plans1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Plans2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Plans3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Writing1 || DCDQ_Writing2 || DCDQ_Writing3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("7. Writing fast", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Writing1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Writing2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Writing3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_legibly1 || DCDQ_legibly2 || DCDQ_legibly3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("8. Writing legibly", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_legibly1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_legibly2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_legibly3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Effort1 || DCDQ_Effort2 || DCDQ_Effort3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("9.Effort and pressure", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Effort1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Effort2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Effort3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Cuts1 || DCDQ_Cuts2 || DCDQ_Cuts3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("10. Cuts", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Cuts1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Cuts2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Cuts3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Likes1 || DCDQ_Likes2 || DCDQ_Likes3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("11. Likes sports", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Likes1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Likes2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Likes3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Learning1 || DCDQ_Learning2 || DCDQ_Learning3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("12. Learning new skills", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Learning1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Learning2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Learning3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Quick1 || DCDQ_Quick2 || DCDQ_Quick3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("13. Quick and competent", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Quick1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Quick2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Quick3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Bull1 || DCDQ_Bull2 || DCDQ_Bull3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("14.Bull in shop", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Bull1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Bull2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Bull3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        if (DCDQ_Does1 || DCDQ_Does2 || DCDQ_Does3)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("15. Does not fatigue", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Does1"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Does2"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Does3"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            cell.BorderWidthLeft = 0.3f;
                            cell.BorderWidthTop = 0.3f;
                            cell.Padding = 5;
                            table.AddCell(cell);
                        }
                        document.Add(table);
                    }

                    if (DCDQ_Control)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Control during movement :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Control"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (DCDQ_Fine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Fine motor and Handwriting :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Fine"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (DCDQ_General)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("General Coordination :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_General"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (DCDQ_Total)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Total:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_Total"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (DCDQ_INTERPRETATION)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("INTERPRETATION:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_INTERPRETATION"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    if (DCDQ_COMMENT)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("COMMENT:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DCDQ_COMMENT"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
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
                        //document.Add(Chunk.NEXTPAGE);
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

                    cell = PhraseCell(new Phrase("EVALUATION :", HeadingFont), PdfPCell.ALIGN_LEFT);
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Barriers :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Functional Limitations :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Posture and Movement Limitation(Prioritized) :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Impairment(Prioritized) :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Summary :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Previous Long Term Goals :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Long Term Goals(Functional Outcome Measured)1 - Year :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Short Term Goals(Functional Outcome Measures) 3 - Month :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                            cell = new PdfPCell(PhraseCell(new Phrase("Impairment related Objective goal-3 Months :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Frequency and Duration :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Service Delivery Models :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Strategies to Address Impairments and Posture Movement Issues Motor Learning :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Equipment / Adjuncts :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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
                        cell = new PdfPCell(PhraseCell(new Phrase("Client / Family Education :", NextHeadingFont), PdfPCell.ALIGN_LEFT));
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

                #region**********Treatment**************
                bool Treatment_Home = false; if (ds.Tables[1].Rows[0]["Treatment_Home"].ToString().Trim().Length > 0) { Treatment_Home = true; }
                bool Treatment_School = false; if (ds.Tables[1].Rows[0]["Treatment_School"].ToString().Trim().Length > 0) { Treatment_School = true; }
                bool Treatment_Threapy = false; if (ds.Tables[1].Rows[0]["Treatment_Threapy"].ToString().Trim().Length > 0) { Treatment_Threapy = true; }
                bool Treatment_cmt = false; if (ds.Tables[1].Rows[0]["Treatment_cmt"].ToString().Trim().Length > 0) { Treatment_cmt = true; }

                if (Treatment_Home || Treatment_School || Treatment_Threapy || Treatment_cmt)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("TREATMENT ADVICE:", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    if (Treatment_Home)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Advice for home :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Treatment_Home"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Treatment_School)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Advice for school :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Treatment_School"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Treatment_Threapy)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Advice for therapy :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Treatment_Threapy"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        // document.Add(Chunk.NEXTPAGE);
                    }
                    if (Treatment_cmt)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Comments :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Treatment_cmt"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                        document.Add(Chunk.NEXTPAGE);
                    }
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


                //PdfPTable newtable = new PdfPTable(1);
                //newtable.AddCell(Doctor_Director);
                //PdfPCell cell1 = new PdfPCell();
                //Image img = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/snehalsign.jpg"));
                ////img.WidthPercentage = 50;
                ////cell1 = ImageCell("~/images/snehalsign.jpg", 20f, PdfPCell.ALIGN_LEFT);
                ////cell1.Colspan = 3;
                ////cell1.PaddingLeft = 5f;
                //newtable.AddCell(img);

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
                //    cell = ImageCell("~/images/snehalsign.jpg", 20f, PdfPCell.ALIGN_LEFT);
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

                cell = PhraseCell(new Phrase(" ", NormalFont), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3;
                cell.PaddingBottom = 0f; cell.PaddingTop = 0f;
                table.AddCell(cell);

                document.Add(table);
                #endregion


                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                context.Response.Clear();
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + _fileName + "");
                context.Response.ContentType = "application/pdf";
                context.Response.Buffer = true;
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.BinaryWrite(bytes);
                context.Response.End();
                context.Response.Close();
            }
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

    }
}