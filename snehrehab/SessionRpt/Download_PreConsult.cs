using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Globalization;

namespace snehrehab.SessionRpt
{
    public class Download_PreConsult
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
                        Image imgSoc = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/theme3.jpg"));
                        imgSoc.ScaleToFit(110, 110);
                        imgSoc.SetAbsolutePosition(510, 810);
                        imgSoc.ScaleAbsoluteWidth(70F);
                      //  cb.AddImage(imgSoc);
                    }

                }
                //Add paging to footer
                {
                    if (pageNo == 0)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 12);
                        cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                        Image img = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/footernew.jpg"));
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

        public void PreScreenRpt(int _appointmentID, HttpContext context)
        {
            SnehBLL.ReportPreConsultMst_Bll RNB = new SnehBLL.ReportPreConsultMst_Bll();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll(); SnehDLL.DoctorMast_Dll DMD = null;
            DataSet ds = RNB.Get(_appointmentID);
            Document document = new Document(PageSize.A4, 30f, 30f, 50f, 30f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font NormalBold = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font NormalItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            Font ColonFont = FontFactory.GetFont("Arial", 10, Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font HeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            Font SubHeadingFont = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string _fileName = DbHelper.Configuration.MakeValidFilename("Pre-Screen Report - " + ds.Tables[0].Rows[0]["FullName"].ToString()) + ".pdf";

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
                cell = ImageCell("~/images/headernew.jpg", 25f, PdfPCell.ALIGN_LEFT);
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
                document.Add(Chunk.NEXTPAGE);
                #endregion

                #region ************** History *********************
                bool His_FamilyHistory = false; if (ds.Tables[1].Rows[0]["His_FamilyHistory"].ToString().Trim().Length > 0) { His_FamilyHistory = true; }
                bool His_FamilyStru = false; if (ds.Tables[1].Rows[0]["His_FamilyStru"].ToString().Trim().Length > 0) { His_FamilyStru = true; }
                bool His_InterParental = false; if (ds.Tables[1].Rows[0]["His_InterParental"].ToString().Trim().Length > 0) { His_InterParental = true; }
                bool His_ParentalChild = false; if (ds.Tables[1].Rows[0]["His_ParentalChild"].ToString().Trim().Length > 0) { His_ParentalChild = true; }
                bool His_EmotionalAbus = false; if (ds.Tables[1].Rows[0]["His_EmotionalAbus"].ToString().Trim().Length > 0) { His_EmotionalAbus = true; }
                bool His_FamilyRelocation = false; if (ds.Tables[1].Rows[0]["His_FamilyRelocation"].ToString().Trim().Length > 0) { His_FamilyRelocation = true; }
                bool His_PrimaryCareGiver = false; if (ds.Tables[1].Rows[0]["His_PrimaryCareGiver"].ToString().Trim().Length > 0) { His_PrimaryCareGiver = true; }
                bool His_MaternalHistory = false; if (ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString().Trim().Length > 0) { His_MaternalHistory = true; }
                bool His_AnyHistoryOf = false; if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Trim().Length > 0) { His_AnyHistoryOf = true; }
                bool PreNatal_AnyComplication = false; if (ds.Tables[1].Rows[0]["PreNatal_AnyComplication"].ToString().Trim().Length > 0) { PreNatal_AnyComplication = true; }
                bool PreNatal_Complications = false; if (ds.Tables[1].Rows[0]["PreNatal_Complications"].ToString().Trim().Length > 0) { PreNatal_Complications = true; }
                if (His_FamilyHistory || His_FamilyStru || His_InterParental || His_ParentalChild || His_EmotionalAbus || His_FamilyRelocation ||
                    His_PrimaryCareGiver || His_MaternalHistory || His_AnyHistoryOf || PreNatal_AnyComplication || PreNatal_Complications)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("History :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);

                    #region
                    if (His_FamilyHistory || His_FamilyStru)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Please give us details about your family structure :", NormalBold), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        PdfPTable tTtable = new PdfPTable(2);
                        tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                        tTtable.SetWidths(new float[] { 0.3f, 0.7f });
                        tTtable.WidthPercentage = 100;
                        tTtable.SpacingBefore = 0f;

                        if (His_FamilyHistory)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Family History :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["His_FamilyHistory"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                        }
                        if (His_FamilyStru)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Family Structure :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["His_FamilyStru"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                        }

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        cell = new PdfPCell(tTtable);
                        cell.BorderWidth = 0f;
                        cell.PaddingLeft = 40f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (His_InterParental || His_ParentalChild || His_EmotionalAbus || His_FamilyRelocation || His_PrimaryCareGiver)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Please tell us about your relationships with the child and amongst yourself?", NormalBold), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                        #region
                        if (His_InterParental || His_ParentalChild)
                        {
                            PdfPTable tTtable = new PdfPTable(1);
                            tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                            tTtable.WidthPercentage = 100;
                            tTtable.SpacingBefore = 0f;

                            cell = new PdfPCell(PhraseCell(new Phrase("Family Relation :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);

                            if (His_InterParental)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase("Inter-parental relationship : " + ds.Tables[1].Rows[0]["His_InterParental"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 5f; cell.PaddingLeft = 10f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                                tTtable.AddCell(cell);
                            }
                            if (His_ParentalChild)
                            {
                                cell = new PdfPCell(PhraseCell(new Phrase("Parent child relationship   : " + ds.Tables[1].Rows[0]["His_ParentalChild"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                                cell.PaddingBottom = 5f; cell.PaddingLeft = 10f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                                tTtable.AddCell(cell);
                            }
                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            cell = new PdfPCell(tTtable);
                            cell.BorderWidth = 0f;
                            cell.PaddingLeft = 40f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                        #region
                        if (His_EmotionalAbus)
                        {
                            PdfPTable tTtable = new PdfPTable(1);
                            tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                            tTtable.WidthPercentage = 100;
                            tTtable.SpacingBefore = 0f;
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Domestic violence/physical or verbal abuse/emotional abuse :", SubHeadingFont));
                            phrase.Add(new Chunk("    " + ds.Tables[1].Rows[0]["His_EmotionalAbus"].ToString().Trim(), NormalFont));
                            cell = new PdfPCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 45f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                            document.Add(tTtable);
                        }
                        #endregion
                        #region
                        if (His_FamilyRelocation)
                        {
                            PdfPTable tTtable = new PdfPTable(1);
                            tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                            tTtable.WidthPercentage = 100;
                            tTtable.SpacingBefore = 0f;
                            phrase = new Phrase();
                            phrase.Add(new Chunk("Family Relocation :", SubHeadingFont));
                            phrase.Add(new Chunk("    " + ds.Tables[1].Rows[0]["His_FamilyRelocation"].ToString().Trim(), NormalFont));
                            cell = new PdfPCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 45f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                            document.Add(tTtable);
                        }
                        #endregion
                        #region
                        if (His_PrimaryCareGiver)
                        {
                            PdfPTable tTtable = new PdfPTable(1);
                            tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                            tTtable.WidthPercentage = 100;
                            tTtable.SpacingBefore = 0f;

                            cell = new PdfPCell(PhraseCell(new Phrase("Primary Care Givers :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["His_PrimaryCareGiver"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 10f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);

                            table = new PdfPTable(1);
                            table.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.WidthPercentage = 100;
                            cell = new PdfPCell(tTtable);
                            cell.BorderWidth = 0f;
                            cell.PaddingLeft = 40f;
                            table.AddCell(cell);
                            document.Add(table);
                        }
                        #endregion
                    }
                    #endregion
                    #region
                    if (His_MaternalHistory || His_AnyHistoryOf)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Maternal History :", NormalBold), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        PdfPTable tTtable = new PdfPTable(2);
                        tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                        tTtable.SetWidths(new float[] { 0.3f, 0.7f });
                        tTtable.WidthPercentage = 100;
                        tTtable.SpacingBefore = 0f;

                        if (His_MaternalHistory)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Maternal History :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                            string His_MaternalHistory_str = string.Empty;
                            if (ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString().Trim().Equals("1st Child"))
                            {
                                His_MaternalHistory_str = "1st Child";
                            }
                            else
                            {
                                His_MaternalHistory_str = "Siblings" + ds.Tables[1].Rows[0]["His_MaternalHistory"].ToString().Trim();
                            }
                            cell = new PdfPCell(PhraseCell(new Phrase(His_MaternalHistory_str, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                        }
                        if (His_AnyHistoryOf)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Any History Of :", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                            string His_MaternalHistory_str = string.Empty;
                            if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Trim().Equals("Diabetes"))
                            {
                                His_MaternalHistory_str = "Diabetes";
                            }
                            else if (ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Trim().Equals("Hypertension"))
                            {
                                His_MaternalHistory_str = "Hypertension";
                            }
                            else
                            {
                                His_MaternalHistory_str = ds.Tables[1].Rows[0]["His_AnyHistoryOf"].ToString().Trim();
                            }
                            cell = new PdfPCell(PhraseCell(new Phrase(His_MaternalHistory_str, NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f; cell.BorderWidth = 0f;
                            tTtable.AddCell(cell);
                        }

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        cell = new PdfPCell(tTtable);
                        cell.BorderWidth = 0f;
                        cell.PaddingLeft = 40f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (PreNatal_AnyComplication || PreNatal_Complications)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Pre-Natal History :", NormalBold), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Any complications during pregnancy : " +
                        ((PreNatal_AnyComplication ? ds.Tables[1].Rows[0]["PreNatal_AnyComplication"].ToString() : string.Empty) + " " +
                           (PreNatal_Complications ? ds.Tables[1].Rows[0]["PreNatal_Complications"].ToString() : string.Empty)).Trim()
                        , NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 50f;
                        table.AddCell(cell);
                        document.Add(table);

                    }
                    #endregion
                }
                #endregion

                #region ************** Birth History *********************
                bool BirthHis_Terms = false; if (ds.Tables[1].Rows[0]["BirthHis_Terms"].ToString().Trim().Length > 0) { BirthHis_Terms = true; }
                bool BirthHis_TermWeek = false; if (ds.Tables[1].Rows[0]["BirthHis_TermWeek"].ToString().Trim().Length > 0) { BirthHis_TermWeek = true; }
                bool BirthHis_Delivery = false; if (ds.Tables[1].Rows[0]["BirthHis_Delivery"].ToString().Trim().Length > 0) { BirthHis_Delivery = true; }
                bool BirthHis_LabourTotal = false; if (ds.Tables[1].Rows[0]["BirthHis_LabourTotal"].ToString().Trim().Length > 0) { BirthHis_LabourTotal = true; }
                bool BirthHis_LabourDiff = false; if (ds.Tables[1].Rows[0]["BirthHis_LabourDiff"].ToString().Trim().Length > 0) { BirthHis_LabourDiff = true; }
                bool BirthHis_LabourProb = false; if (ds.Tables[1].Rows[0]["BirthHis_LabourProb"].ToString().Trim().Length > 0) { BirthHis_LabourProb = true; }
                bool BirthHis_Aneshthesia = false; if (ds.Tables[1].Rows[0]["BirthHis_Aneshthesia"].ToString().Trim().Length > 0) { BirthHis_Aneshthesia = true; }
                bool Other_CIAB = false; if (ds.Tables[1].Rows[0]["Other_CIAB"].ToString().Trim().Length > 0) { Other_CIAB = true; }
                bool Other_BirthWeight = false; if (ds.Tables[1].Rows[0]["Other_BirthWeight"].ToString().Trim().Length > 0) { Other_BirthWeight = true; }
                bool Other_SGA_AGA = false; if (ds.Tables[1].Rows[0]["Other_SGA_AGA"].ToString().Trim().Length > 0) { Other_SGA_AGA = true; }
                bool Other_APGAR_Score = false; if (ds.Tables[1].Rows[0]["Other_APGAR_Score"].ToString().Trim().Length > 0) { Other_APGAR_Score = true; }
                if (BirthHis_Terms || BirthHis_TermWeek || BirthHis_Delivery || BirthHis_LabourTotal || BirthHis_LabourDiff || BirthHis_LabourProb ||
                    BirthHis_Aneshthesia || Other_CIAB || Other_BirthWeight || Other_SGA_AGA || Other_APGAR_Score)
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
                    if (BirthHis_Terms || BirthHis_TermWeek || BirthHis_Delivery || BirthHis_LabourTotal || BirthHis_LabourDiff)
                    {
                        PdfPTable tTtable = new PdfPTable(2);
                        tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                        tTtable.SetWidths(new float[] { 0.25f, 0.75f });
                        tTtable.WidthPercentage = 100;
                        tTtable.SpacingBefore = 10f;

                        if (BirthHis_Terms)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Birth History :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_Terms"].ToString().Trim() + " " + (BirthHis_TermWeek ? ds.Tables[1].Rows[0]["BirthHis_TermWeek"].ToString().Trim() : ""), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        if (BirthHis_Delivery)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Delivery :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_Delivery"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        if (BirthHis_LabourTotal)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Length of total labour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_LabourTotal"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        if (BirthHis_LabourDiff)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Difficult labour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_LabourDiff"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        cell = new PdfPCell(tTtable);
                        cell.BorderWidth = 0f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (BirthHis_LabourProb)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Problems encountered during labour :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_LabourProb"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (BirthHis_Aneshthesia)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("What kind of aneshthesia if LSCS :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["BirthHis_Aneshthesia"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (Other_CIAB || Other_BirthWeight || Other_SGA_AGA || Other_APGAR_Score)
                    {
                        PdfPTable tTtable = new PdfPTable(2);
                        tTtable.HorizontalAlignment = Element.ALIGN_LEFT;
                        tTtable.SetWidths(new float[] { 0.25f, 0.75f });
                        tTtable.WidthPercentage = 100;
                        tTtable.SpacingBefore = 10f;
                        if (Other_CIAB)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("CIAB :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_CIAB"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        if (Other_BirthWeight)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Birth Weight :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_BirthWeight"].ToString().Trim() + " " + (Other_SGA_AGA ? ds.Tables[1].Rows[0]["Other_SGA_AGA"].ToString().Trim() : ""), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        if (Other_APGAR_Score)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("APGAR score :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_APGAR_Score"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            tTtable.AddCell(cell);
                        }
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        cell = new PdfPCell(tTtable);
                        cell.BorderWidth = 0f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Surgical History *********************
                bool NICU = false; if (ds.Tables[1].Rows[0]["NICU"].ToString().Trim().Length > 0) { NICU = true; }
                bool NICU_Duration = false; if (ds.Tables[1].Rows[0]["NICU_Duration"].ToString().Trim().Length > 0) { NICU_Duration = true; }
                bool NICU_Reason = false; if (ds.Tables[1].Rows[0]["NICU_Reason"].ToString().Trim().Length > 0) { NICU_Reason = true; }
                bool DischargedOnWhichDay = false; if (ds.Tables[1].Rows[0]["DischargedOnWhichDay"].ToString().Trim().Length > 0) { DischargedOnWhichDay = true; }
                bool ChildTakingMotherFeeds = false; if (ds.Tables[1].Rows[0]["ChildTakingMotherFeeds"].ToString().Trim().Length > 0) { ChildTakingMotherFeeds = true; }
                bool AnyOtherRelevantMedicalHistory = false; if (ds.Tables[1].Rows[0]["AnyOtherRelevantMedicalHistory"].ToString().Trim().Length > 0) { AnyOtherRelevantMedicalHistory = true; }
                bool MedicalTimeLine = false; if (ds.Tables[1].Rows[0]["MedicalTimeLine"].ToString().Trim().Length > 0) { MedicalTimeLine = true; }
                if (NICU || NICU_Duration || NICU_Reason || DischargedOnWhichDay || ChildTakingMotherFeeds || AnyOtherRelevantMedicalHistory || MedicalTimeLine)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Surgical History :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region
                    if (NICU)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("NICU :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["NICU"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (NICU_Duration)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Duration :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["NICU_Duration"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (NICU_Reason)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Reason :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["NICU_Reason"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (DischargedOnWhichDay)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Discharged on which day :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DischargedOnWhichDay"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (ChildTakingMotherFeeds)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Child taking mother’s feeds :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ChildTakingMotherFeeds"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (AnyOtherRelevantMedicalHistory)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Any other relevant Medical History :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AnyOtherRelevantMedicalHistory"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (MedicalTimeLine)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Medical Time Line :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["MedicalTimeLine"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Post Discharge *********************
                bool HowWasBabyAtHome = false; if (ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString().Trim().Length > 0) { HowWasBabyAtHome = true; }
                bool WasHeFeedingWell = false; if (ds.Tables[1].Rows[0]["WasHeFeedingWell"].ToString().Trim().Length > 0) { WasHeFeedingWell = true; }
                bool WasHeSleepingWell = false; if (ds.Tables[1].Rows[0]["WasHeSleepingWell"].ToString().Trim().Length > 0) { WasHeSleepingWell = true; }
                bool AnyDelay_MedicalEvent_Symptoms = false; if (ds.Tables[1].Rows[0]["AnyDelay_MedicalEvent_Symptoms"].ToString().Trim().Length > 0) { AnyDelay_MedicalEvent_Symptoms = true; }
                bool WhoWasTheFirstNotice = false; if (ds.Tables[1].Rows[0]["WhoWasTheFirstNotice"].ToString().Trim().Length > 0) { WhoWasTheFirstNotice = true; }
                bool WhatWasDoneForTheSame = false; if (ds.Tables[1].Rows[0]["WhatWasDoneForTheSame"].ToString().Trim().Length > 0) { WhatWasDoneForTheSame = true; }
                bool ChildStartedToHeadHold = false; if (ds.Tables[1].Rows[0]["ChildStartedToHeadHold"].ToString().Trim().Length > 0) { ChildStartedToHeadHold = true; }
                bool WasItOnTimeOrDelayed = false; if (ds.Tables[1].Rows[0]["WasItOnTimeOrDelayed"].ToString().Trim().Length > 0) { WasItOnTimeOrDelayed = true; }
                bool CloselyInvolvedWithChild = false; if (ds.Tables[1].Rows[0]["CloselyInvolvedWithChild"].ToString().Trim().Length > 0) { CloselyInvolvedWithChild = true; }
                bool ChildChooseToUseFreeTime = false; if (ds.Tables[1].Rows[0]["ChildChooseToUseFreeTime"].ToString().Trim().Length > 0) { ChildChooseToUseFreeTime = true; }
                bool ObservationsDuringFreePlay = false; if (ds.Tables[1].Rows[0]["ObservationsDuringFreePlay"].ToString().Trim().Length > 0) { ObservationsDuringFreePlay = true; }
                bool Brushing_Dependant = false; if (ds.Tables[1].Rows[0]["Brushing_Dependant"].ToString().Trim().Length > 0) { Brushing_Dependant = true; }
                bool Brushing_Independant = false; if (ds.Tables[1].Rows[0]["Brushing_Independant"].ToString().Trim().Length > 0) { Brushing_Independant = true; }
                bool Brushing_Assisted = false; if (ds.Tables[1].Rows[0]["Brushing_Assisted"].ToString().Trim().Length > 0) { Brushing_Assisted = true; }
                bool Toileting_Dependant = false; if (ds.Tables[1].Rows[0]["Toileting_Dependant"].ToString().Trim().Length > 0) { Toileting_Dependant = true; }
                bool Toileting_Independant = false; if (ds.Tables[1].Rows[0]["Toileting_Independant"].ToString().Trim().Length > 0) { Toileting_Independant = true; }
                bool Toileting_Assisted = false; if (ds.Tables[1].Rows[0]["Toileting_Assisted"].ToString().Trim().Length > 0) { Toileting_Assisted = true; }
                bool Bathing_Dependant = false; if (ds.Tables[1].Rows[0]["Bathing_Dependant"].ToString().Trim().Length > 0) { Bathing_Dependant = true; }
                bool Bathing_Independant = false; if (ds.Tables[1].Rows[0]["Bathing_Independant"].ToString().Trim().Length > 0) { Bathing_Independant = true; }
                bool Bathing_Assisted = false; if (ds.Tables[1].Rows[0]["Bathing_Assisted"].ToString().Trim().Length > 0) { Bathing_Assisted = true; }
                bool Dressing_Dependant = false; if (ds.Tables[1].Rows[0]["Dressing_Dependant"].ToString().Trim().Length > 0) { Dressing_Dependant = true; }
                bool Dressing_Independant = false; if (ds.Tables[1].Rows[0]["Dressing_Independant"].ToString().Trim().Length > 0) { Dressing_Independant = true; }
                bool Dressing_Assisted = false; if (ds.Tables[1].Rows[0]["Dressing_Assisted"].ToString().Trim().Length > 0) { Dressing_Assisted = true; }
                bool Feeding_Dependant = false; if (ds.Tables[1].Rows[0]["Feeding_Dependant"].ToString().Trim().Length > 0) { Feeding_Dependant = true; }
                bool Feeding_Independant = false; if (ds.Tables[1].Rows[0]["Feeding_Independant"].ToString().Trim().Length > 0) { Feeding_Independant = true; }
                bool Feeding_Assisted = false; if (ds.Tables[1].Rows[0]["Feeding_Assisted"].ToString().Trim().Length > 0) { Feeding_Assisted = true; }
                bool Ambulation_Dependant = false; if (ds.Tables[1].Rows[0]["Ambulation_Dependant"].ToString().Trim().Length > 0) { Ambulation_Dependant = true; }
                bool Ambulation_Independant = false; if (ds.Tables[1].Rows[0]["Ambulation_Independant"].ToString().Trim().Length > 0) { Ambulation_Independant = true; }
                bool Ambulation_Assisted = false; if (ds.Tables[1].Rows[0]["Ambulation_Assisted"].ToString().Trim().Length > 0) { Ambulation_Assisted = true; }
                bool Transfer_Dependant = false; if (ds.Tables[1].Rows[0]["Transfer_Dependant"].ToString().Trim().Length > 0) { Transfer_Dependant = true; }
                bool Transfer_Independant = false; if (ds.Tables[1].Rows[0]["Transfer_Independant"].ToString().Trim().Length > 0) { Transfer_Independant = true; }
                bool Transfer_Assisted = false; if (ds.Tables[1].Rows[0]["Transfer_Assisted"].ToString().Trim().Length > 0) { Transfer_Assisted = true; }
                bool Summary = false; if (ds.Tables[1].Rows[0]["Summary"].ToString().Trim().Length > 0) { Summary = true; }
                bool EvaluationNeeded = false; if (ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString().Trim().Length > 0) { EvaluationNeeded = true; }
                bool ReleventMedicalTimeline = false; if (ds.Tables[1].Rows[0]["ReleventMedicalTimeline"].ToString().Trim().Length > 0) { ReleventMedicalTimeline = true; }
                bool DailyRoutine = false; if (ds.Tables[1].Rows[0]["DailyRoutine"].ToString().Trim().Length > 0) { DailyRoutine = true; }
                if (HowWasBabyAtHome || WasHeFeedingWell || WasHeSleepingWell || AnyDelay_MedicalEvent_Symptoms || WhoWasTheFirstNotice ||
                    WhatWasDoneForTheSame || ChildStartedToHeadHold || WasItOnTimeOrDelayed || CloselyInvolvedWithChild || ChildChooseToUseFreeTime ||
                    ObservationsDuringFreePlay || Brushing_Dependant || Brushing_Independant || Brushing_Assisted || Toileting_Dependant ||
                    Toileting_Independant || Toileting_Assisted || Bathing_Dependant || Bathing_Independant || Bathing_Assisted || Dressing_Dependant ||
                    Dressing_Independant || Dressing_Assisted || Feeding_Dependant || Feeding_Independant || Feeding_Assisted || Ambulation_Dependant ||
                    Ambulation_Independant || Ambulation_Assisted || Transfer_Dependant || Transfer_Independant || Transfer_Assisted || Summary ||
                    EvaluationNeeded || ReleventMedicalTimeline || DailyRoutine)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Post Discharge :", HeadingFont), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 30f;
                    table.AddCell(cell);
                    document.Add(table);
                    #region
                    if (HowWasBabyAtHome)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How was the baby at home?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["HowWasBabyAtHome"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (WasHeFeedingWell)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Was he feeding well?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["WasHeFeedingWell"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (WasHeSleepingWell)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Was he sleeping well?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["WasHeSleepingWell"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (AnyDelay_MedicalEvent_Symptoms)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Any delay? Medical Event? Symptoms?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["AnyDelay_MedicalEvent_Symptoms"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (WhoWasTheFirstNotice)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Who was the first to notice?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["WhoWasTheFirstNotice"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (WhatWasDoneForTheSame)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("What was done for the same (whose consultation, investigation, diagnosis and treatment plan given?)", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["WhatWasDoneForTheSame"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (ChildStartedToHeadHold)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Please tell us when your child started to head hold, roll to side, crawl, creep, sit by himself, stand with and without support and walk by him/herself:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ChildStartedToHeadHold"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (WasItOnTimeOrDelayed)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Was it on time or delayed?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["WasItOnTimeOrDelayed"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (CloselyInvolvedWithChild)
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
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["CloselyInvolvedWithChild"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (ReleventMedicalTimeline)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Relevent medical time line :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ReleventMedicalTimeline"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (ChildChooseToUseFreeTime)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("How does your child choose to use his/her free time?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ChildChooseToUseFreeTime"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (ObservationsDuringFreePlay)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Observations during free play:", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ObservationsDuringFreePlay"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (Brushing_Dependant || Brushing_Independant || Brushing_Assisted || Toileting_Dependant || Toileting_Independant ||
                        Toileting_Assisted || Bathing_Dependant || Bathing_Independant || Bathing_Assisted || Dressing_Dependant ||
                        Dressing_Independant || Dressing_Assisted || Feeding_Dependant || Feeding_Independant || Feeding_Assisted ||
                        Ambulation_Dependant || Ambulation_Independant || Ambulation_Assisted || Transfer_Dependant || Transfer_Independant || Transfer_Assisted)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Please tell us how do you look after your child’s daily activities Does he/ she require assistance or can do it independently.", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(4);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;

                        #region headers

                        cell = new PdfPCell(PhraseCell(new Phrase("", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Dependent", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Independent", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase("Assisted", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        #endregion
                        #region
                        if (Brushing_Dependant || Brushing_Independant || Brushing_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Brushing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Brushing_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Brushing_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Brushing_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Toileting_Dependant || Toileting_Independant || Toileting_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Toileting", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Toileting_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Toileting_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Toileting_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Bathing_Dependant || Bathing_Independant || Bathing_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Bathing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bathing_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bathing_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Bathing_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Dressing_Dependant || Dressing_Independant || Dressing_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Dressing", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Dressing_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Dressing_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Dressing_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Feeding_Dependant || Feeding_Independant || Feeding_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Feeding", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Feeding_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Feeding_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Feeding_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Ambulation_Dependant || Ambulation_Independant || Ambulation_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Ambulation", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Ambulation_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Ambulation_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Ambulation_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        #region
                        if (Transfer_Dependant || Transfer_Independant || Transfer_Assisted)
                        {
                            cell = new PdfPCell(PhraseCell(new Phrase("Transfer", NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Transfer_Dependant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Transfer_Independant"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                            cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Transfer_Assisted"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                            cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }
                        #endregion
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (DailyRoutine)
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
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["DailyRoutine"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (Summary)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Summary :", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Summary"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                    #region
                    if (EvaluationNeeded)
                    {
                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 20f;
                        cell = new PdfPCell(PhraseCell(new Phrase("Evaluation needed ?", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 30f;
                        table.AddCell(cell);
                        document.Add(table);

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["EvaluationNeeded"].ToString(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 3f;
                        cell.PaddingLeft = 60f;
                        table.AddCell(cell);
                        document.Add(table);
                    }
                    #endregion
                }
                #endregion

                #region ************** Speciality Contacts *********************
                bool Cardiologist_Name = false; if (ds.Tables[1].Rows[0]["Cardiologist_Name"].ToString().Trim().Length > 0) { Cardiologist_Name = true; }
                bool Cardiologist_Date = false; if (ds.Tables[1].Rows[0]["Cardiologist_Date"].ToString().Trim().Length > 0) { Cardiologist_Date = true; }
                bool Cardiologist_Addr = false; if (ds.Tables[1].Rows[0]["Cardiologist_Addr"].ToString().Trim().Length > 0) { Cardiologist_Addr = true; }
                bool Cardiologist_Phone = false; if (ds.Tables[1].Rows[0]["Cardiologist_Phone"].ToString().Trim().Length > 0) { Cardiologist_Phone = true; }
                bool Orthopedist_Name = false; if (ds.Tables[1].Rows[0]["Orthopedist_Name"].ToString().Trim().Length > 0) { Orthopedist_Name = true; }
                bool Orthopedist_Date = false; if (ds.Tables[1].Rows[0]["Orthopedist_Date"].ToString().Trim().Length > 0) { Orthopedist_Date = true; }
                bool Orthopedist_Addr = false; if (ds.Tables[1].Rows[0]["Orthopedist_Addr"].ToString().Trim().Length > 0) { Orthopedist_Addr = true; }
                bool Orthopedist_Phone = false; if (ds.Tables[1].Rows[0]["Orthopedist_Phone"].ToString().Trim().Length > 0) { Orthopedist_Phone = true; }
                bool Psychologist_Name = false; if (ds.Tables[1].Rows[0]["Psychologist_Name"].ToString().Trim().Length > 0) { Psychologist_Name = true; }
                bool Psychologist_Date = false; if (ds.Tables[1].Rows[0]["Psychologist_Date"].ToString().Trim().Length > 0) { Psychologist_Date = true; }
                bool Psychologist_Addr = false; if (ds.Tables[1].Rows[0]["Psychologist_Addr"].ToString().Trim().Length > 0) { Psychologist_Addr = true; }
                bool Psychologist_Phone = false; if (ds.Tables[1].Rows[0]["Psychologist_Phone"].ToString().Trim().Length > 0) { Psychologist_Phone = true; }
                bool Psychiatrist_Name = false; if (ds.Tables[1].Rows[0]["Psychiatrist_Name"].ToString().Trim().Length > 0) { Psychiatrist_Name = true; }
                bool Psychiatrist_Date = false; if (ds.Tables[1].Rows[0]["Psychiatrist_Date"].ToString().Trim().Length > 0) { Psychiatrist_Date = true; }
                bool Psychiatrist_Addr = false; if (ds.Tables[1].Rows[0]["Psychiatrist_Addr"].ToString().Trim().Length > 0) { Psychiatrist_Addr = true; }
                bool Psychiatrist_Phone = false; if (ds.Tables[1].Rows[0]["Psychiatrist_Phone"].ToString().Trim().Length > 0) { Psychiatrist_Phone = true; }
                bool Opthalmologist_Name = false; if (ds.Tables[1].Rows[0]["Opthalmologist_Name"].ToString().Trim().Length > 0) { Opthalmologist_Name = true; }
                bool Opthalmologist_Date = false; if (ds.Tables[1].Rows[0]["Opthalmologist_Date"].ToString().Trim().Length > 0) { Opthalmologist_Date = true; }
                bool Opthalmologist_Addr = false; if (ds.Tables[1].Rows[0]["Opthalmologist_Addr"].ToString().Trim().Length > 0) { Opthalmologist_Addr = true; }
                bool Opthalmologist_Phone = false; if (ds.Tables[1].Rows[0]["Opthalmologist_Phone"].ToString().Trim().Length > 0) { Opthalmologist_Phone = true; }
                bool Speech_Name = false; if (ds.Tables[1].Rows[0]["Speech_Name"].ToString().Trim().Length > 0) { Speech_Name = true; }
                bool Speech_Date = false; if (ds.Tables[1].Rows[0]["Speech_Date"].ToString().Trim().Length > 0) { Speech_Date = true; }
                bool Speech_Addr = false; if (ds.Tables[1].Rows[0]["Speech_Addr"].ToString().Trim().Length > 0) { Speech_Addr = true; }
                bool Speech_Phone = false; if (ds.Tables[1].Rows[0]["Speech_Phone"].ToString().Trim().Length > 0) { Speech_Phone = true; }
                bool Pathologist_Name = false; if (ds.Tables[1].Rows[0]["Pathologist_Name"].ToString().Trim().Length > 0) { Pathologist_Name = true; }
                bool Pathologist_Date = false; if (ds.Tables[1].Rows[0]["Pathologist_Date"].ToString().Trim().Length > 0) { Pathologist_Date = true; }
                bool Pathologist_Addr = false; if (ds.Tables[1].Rows[0]["Pathologist_Addr"].ToString().Trim().Length > 0) { Pathologist_Addr = true; }
                bool Pathologist_Phone = false; if (ds.Tables[1].Rows[0]["Pathologist_Phone"].ToString().Trim().Length > 0) { Pathologist_Phone = true; }
                bool Occupational_Name = false; if (ds.Tables[1].Rows[0]["Occupational_Name"].ToString().Trim().Length > 0) { Occupational_Name = true; }
                bool Occupational_Date = false; if (ds.Tables[1].Rows[0]["Occupational_Date"].ToString().Trim().Length > 0) { Occupational_Date = true; }
                bool Occupational_Addr = false; if (ds.Tables[1].Rows[0]["Occupational_Addr"].ToString().Trim().Length > 0) { Occupational_Addr = true; }
                bool Occupational_Phone = false; if (ds.Tables[1].Rows[0]["Occupational_Phone"].ToString().Trim().Length > 0) { Occupational_Phone = true; }
                bool Physical_Name = false; if (ds.Tables[1].Rows[0]["Physical_Name"].ToString().Trim().Length > 0) { Physical_Name = true; }
                bool Physical_Date = false; if (ds.Tables[1].Rows[0]["Physical_Date"].ToString().Trim().Length > 0) { Physical_Date = true; }
                bool Physical_Addr = false; if (ds.Tables[1].Rows[0]["Physical_Addr"].ToString().Trim().Length > 0) { Physical_Addr = true; }
                bool Physical_Phone = false; if (ds.Tables[1].Rows[0]["Physical_Phone"].ToString().Trim().Length > 0) { Physical_Phone = true; }
                bool Audiologist_Name = false; if (ds.Tables[1].Rows[0]["Audiologist_Name"].ToString().Trim().Length > 0) { Audiologist_Name = true; }
                bool Audiologist_Date = false; if (ds.Tables[1].Rows[0]["Audiologist_Date"].ToString().Trim().Length > 0) { Audiologist_Date = true; }
                bool Audiologist_Addr = false; if (ds.Tables[1].Rows[0]["Audiologist_Addr"].ToString().Trim().Length > 0) { Audiologist_Addr = true; }
                bool Audiologist_Phone = false; if (ds.Tables[1].Rows[0]["Audiologist_Phone"].ToString().Trim().Length > 0) { Audiologist_Phone = true; }
                bool ENT_Name = false; if (ds.Tables[1].Rows[0]["ENT_Name"].ToString().Trim().Length > 0) { ENT_Name = true; }
                bool ENT_Date = false; if (ds.Tables[1].Rows[0]["ENT_Date"].ToString().Trim().Length > 0) { ENT_Date = true; }
                bool ENT_Addr = false; if (ds.Tables[1].Rows[0]["ENT_Addr"].ToString().Trim().Length > 0) { ENT_Addr = true; }
                bool ENT_Phone = false; if (ds.Tables[1].Rows[0]["ENT_Phone"].ToString().Trim().Length > 0) { ENT_Phone = true; }
                bool Chiropractor_Name = false; if (ds.Tables[1].Rows[0]["Chiropractor_Name"].ToString().Trim().Length > 0) { Chiropractor_Name = true; }
                bool Chiropractor_Date = false; if (ds.Tables[1].Rows[0]["Chiropractor_Date"].ToString().Trim().Length > 0) { Chiropractor_Date = true; }
                bool Chiropractor_Addr = false; if (ds.Tables[1].Rows[0]["Chiropractor_Addr"].ToString().Trim().Length > 0) { Chiropractor_Addr = true; }
                bool Chiropractor_Phone = false; if (ds.Tables[1].Rows[0]["Chiropractor_Phone"].ToString().Trim().Length > 0) { Chiropractor_Phone = true; }
                bool Other_Name = false; if (ds.Tables[1].Rows[0]["Other_Name"].ToString().Trim().Length > 0) { Other_Name = true; }
                bool Other_Date = false; if (ds.Tables[1].Rows[0]["Other_Date"].ToString().Trim().Length > 0) { Other_Date = true; }
                bool Other_Addr = false; if (ds.Tables[1].Rows[0]["Other_Addr"].ToString().Trim().Length > 0) { Other_Addr = true; }
                bool Other_Phone = false; if (ds.Tables[1].Rows[0]["Other_Phone"].ToString().Trim().Length > 0) { Other_Phone = true; }
                if (Cardiologist_Name || Cardiologist_Date || Cardiologist_Addr || Cardiologist_Phone || Orthopedist_Name || Orthopedist_Date ||
                    Orthopedist_Addr || Orthopedist_Phone || Psychologist_Name || Psychologist_Date || Psychologist_Addr || Psychologist_Phone ||
                    Psychiatrist_Name || Psychiatrist_Date || Psychiatrist_Addr || Psychiatrist_Phone || Opthalmologist_Name || Opthalmologist_Date ||
                    Opthalmologist_Addr || Opthalmologist_Phone || Speech_Name || Speech_Date || Speech_Addr || Speech_Phone || Pathologist_Name ||
                    Pathologist_Date || Pathologist_Addr || Pathologist_Phone || Occupational_Name || Occupational_Date || Occupational_Addr ||
                    Occupational_Phone || Physical_Name || Physical_Date || Physical_Addr || Physical_Phone || Audiologist_Name || Audiologist_Date ||
                    Audiologist_Addr || Audiologist_Phone || ENT_Name || ENT_Date || ENT_Addr || ENT_Phone || Chiropractor_Name || Chiropractor_Date ||
                    Chiropractor_Addr || Chiropractor_Phone || Other_Name || Other_Date || Other_Addr || Other_Phone)
                {
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.SetWidths(new float[] { 0.3f, 1f });
                    table.SpacingBefore = 20f;

                    cell = PhraseCell(new Phrase("Speciality Contacts :", HeadingFont), PdfPCell.ALIGN_LEFT);
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
                    table.SpacingBefore = 10f;

                    #region headers

                    cell = new PdfPCell(PhraseCell(new Phrase("", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Name of Agency", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Specialist Date", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Address", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(PhraseCell(new Phrase("Phone", SubHeadingFont), PdfPCell.ALIGN_LEFT));
                    cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                    cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    #endregion
                    #region
                    if (Cardiologist_Name || Cardiologist_Date || Cardiologist_Addr || Cardiologist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Cardiologist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiologist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiologist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiologist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Cardiologist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Orthopedist_Name || Orthopedist_Date || Orthopedist_Addr || Orthopedist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Orthopedist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Orthopedist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Orthopedist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Orthopedist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Orthopedist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Psychologist_Name || Psychologist_Date || Psychologist_Addr || Psychologist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Psychologist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychologist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychologist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychologist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychologist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Psychiatrist_Name || Psychiatrist_Date || Psychiatrist_Addr || Psychiatrist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Psychiatrist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychiatrist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychiatrist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychiatrist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Psychiatrist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Opthalmologist_Name || Opthalmologist_Date || Opthalmologist_Addr || Opthalmologist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Opthalmologist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Opthalmologist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Opthalmologist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Opthalmologist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Opthalmologist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Speech_Name || Speech_Date || Speech_Addr || Speech_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Speech", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Speech_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Speech_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Speech_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Speech_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Pathologist_Name || Pathologist_Date || Pathologist_Addr || Pathologist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Pathologist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Pathologist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Pathologist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Pathologist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Pathologist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Occupational_Name || Occupational_Date || Occupational_Addr || Occupational_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Occupational", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Occupational_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Occupational_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Occupational_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Occupational_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Physical_Name || Physical_Date || Physical_Addr || Physical_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Physical Therapist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Physical_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Physical_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Physical_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Physical_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Audiologist_Name || Audiologist_Date || Audiologist_Addr || Audiologist_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Audiologist", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Audiologist_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Audiologist_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Audiologist_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Audiologist_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (ENT_Name || ENT_Date || ENT_Addr || ENT_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("ENT", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ENT_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ENT_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ENT_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["ENT_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Chiropractor_Name || Chiropractor_Date || Chiropractor_Addr || Chiropractor_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Chiropractor", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Chiropractor_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Chiropractor_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Chiropractor_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Chiropractor_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region
                    if (Other_Name || Other_Date || Other_Addr || Other_Phone)
                    {
                        cell = new PdfPCell(PhraseCell(new Phrase("Other", NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_Name"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_Date"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_Addr"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);

                        cell = new PdfPCell(PhraseCell(new Phrase(ds.Tables[1].Rows[0]["Other_Phone"].ToString().Trim(), NormalFont), PdfPCell.ALIGN_LEFT));
                        cell.PaddingBottom = 5f; cell.PaddingLeft = 5f; cell.PaddingRight = 5f; cell.PaddingTop = 5f;
                        cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }
                    #endregion
                    document.Add(table);
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
                //table.AddCell(newtable);
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

                //table.AddCell(PhraseCell(new Phrase("DIRECTOR SNEH RERC", NormalBold), PdfPCell.ALIGN_CENTER));
                table.AddCell(PhraseCell(new Phrase("Dr. Snehal Deshpande" + '\n' + " Consultant Neonatal & Development Therapist" + '\n' + "Pediatric Physiotherapy Department" + '\n' + "Dr. LH Hiranandani Hospital, Powai", NormalBold), PdfPCell.ALIGN_CENTER));
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