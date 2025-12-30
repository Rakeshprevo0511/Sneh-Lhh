using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for MailReceipt_PDF
    /// </summary>
    public class MailReceipt_PDF : IHttpHandler, IRequiresSessionState
    {

        int _loginID = 0; string str = "<style>html{background: url(\"/images/bg_login.jpg\");}.alert{margin: 0 auto;max-width: 475px;margin-top: 10%;background: #F5F5F5;padding: 20px;color: #6E6666;font-family: calibri;font-size: 1.2em;min-height: 50px;border: 2px solid #A8A8A8;border-radius: 2px;font-weight: lighter;line-height: 30px;}</style>";
        string UniqueID = string.Empty; int ReceiptType = 0; string FiscalDate = string.Empty; string rn = string.Empty;
        string mailid = string.Empty; int patientid = 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            //rModel r = new rModel(); 
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">You required login to print document.</div>");
                context.Response.End();
                return;
            }
            //var request = context.Request;
            //var requestBody = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            //var jsonSerializer = new JavaScriptSerializer();
            //MailPDF evnt = jsonSerializer.Deserialize<MailPDF>(requestBody);
            //if (evnt == null)
            //{
            //    r.msg = "Invalid request.";
            //    context.Response.Write(JsonConvert.SerializeObject(r));
            //    return;
            //}
            if (context.Request.QueryString["id"] != null)
            {
                snehrehab.rModel r = new snehrehab.rModel();
                try
                {
                    dynamic data = JsonConvert.DeserializeObject((r.Decode(HttpUtility.UrlDecode(context.Request.QueryString["id"].ToString()))));
                    if (DbHelper.Configuration.IsGuid((string)data.un))
                    {
                        UniqueID = (string)data.un;
                    }
                    int.TryParse((string)data.rt, out ReceiptType);
                    FiscalDate = (string)data.fy;
                    rn = (string)data.rn;
                }
                catch
                {
                }
            }
            if (context.Request.QueryString["mailid"] != null)
            {
                mailid = context.Request.QueryString["mailid"].ToString();
            }
            if (context.Request.QueryString["pid"] != null)
            {
                int.TryParse(context.Request.QueryString["pid"].ToString(), out patientid);
            }

            DateTime _test = new DateTime(); DateTime.TryParseExact(FiscalDate, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (string.IsNullOrEmpty(rn) || string.IsNullOrEmpty(UniqueID) || ReceiptType <= 0 || _test <= DateTime.MinValue)
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">Invalid document print request.</div>");
                context.Response.End();
                return;
            }
            SqlCommand cmd = new SqlCommand("Receipt_PrintData"); cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.NVarChar, 500).Value = UniqueID;
            cmd.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
            cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = _test;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                long ReceiptNo = 0; long.TryParse(rn, out ReceiptNo);
                long TableID = 0; long.TryParse(dt.Rows[0]["TableID"].ToString(), out TableID);
                string ReceiptPrefix = dt.Rows[0]["ReceiptPrefix"].ToString();
                DateTime PayDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["PayDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PayDate);
                string FullName = dt.Rows[0]["FullName"].ToString();
                float NetAmt = 0; float.TryParse(dt.Rows[0]["NetAmt"].ToString(), out NetAmt);
                string PrintRemark = dt.Rows[0]["PrintRemark"].ToString();

                string _receipt_no = "Receipt_" + DbHelper.Configuration.RandomNumber(2) + ReceiptPrefix + DbHelper.Configuration.ReceiptNo(ReceiptNo) + ".pdf";
                string result_sheet = HttpContext.Current.Server.MapPath("~/Files/Receipt/") + _receipt_no;
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(result_sheet))) { Directory.CreateDirectory(System.IO.Path.GetDirectoryName(result_sheet)); }
                if (File.Exists(result_sheet)) { try { File.Delete(result_sheet); } catch { } }

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
                var FontColour = new BaseColor(82, 78, 79); string fontName = "Calibri";
                BaseFont customfont = BaseFont.CreateFont(context.Server.MapPath("~/fonts/OpenSans-Regular_0.ttf"), BaseFont.CP1252, BaseFont.EMBEDDED);
                Font fontNormal = new Font(customfont, 12, Font.NORMAL, FontColour);
                Font fontItalic = new Font(customfont, 12, Font.ITALIC, FontColour);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    #region PAGE HEADER
                    iTextSharp.text.Image image = null;
                    string logoPath = HttpContext.Current.Server.MapPath("~/images/compressedsnehlogo.png");
                    if (File.Exists(logoPath))
                    {
                        image = iTextSharp.text.Image.GetInstance(logoPath);
                        image.ScaleToFitLineWhenOverflow = false;
                        image.ScaleAbsoluteWidth(190F); image.ScaleAbsoluteHeight(50F);
                        image.SetAbsolutePosition(document.LeftMargin + 9, document.Top - (document.TopMargin + 40));
                        document.Add(image);
                    }

                    PdfPTable headerTable = new PdfPTable(1);
                    headerTable.WidthPercentage = 100F; headerTable.SetWidths(new float[] { 100f });
                    headerTable.AddCell(new PdfPCell() { Phrase = new Phrase("Dr. Snehal G. Deshpande", FontFactory.GetFont(fontName, 16, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 5, PaddingTop = 10, PaddingLeft = 10 });
                    headerTable.AddCell(new PdfPCell() { Phrase = new Phrase("B.Sc.(P.T.), MIAP, PGDHHM, NDTA trained, SI (USA)", FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 5, PaddingTop = 5, PaddingLeft = 10 });
                    headerTable.AddCell(new PdfPCell() { Phrase = new Phrase("Redg. No. : 1884", FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 5, PaddingTop = 0, PaddingLeft = 10 });
                    headerTable.AddCell(new PdfPCell() { Phrase = new Phrase("CONSULTING PHYSIOTHERAPIST", FontFactory.GetFont(fontName, 11, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 5, PaddingTop = 0, PaddingLeft = 10 });

                    PdfPTable headerOuterTable = new PdfPTable(2); headerOuterTable.WidthPercentage = 100F; headerOuterTable.SetWidths(new float[] { 30F, 70f });
                    headerOuterTable.AddCell(new PdfPCell() { Phrase = new Phrase(""), Border = PdfPCell.NO_BORDER });

                    PdfPCell headerOuterCell = new PdfPCell(headerTable); headerOuterCell.Border = PdfPCell.NO_BORDER;
                    headerOuterTable.AddCell(headerOuterCell);

                    document.Add(headerOuterTable);
                    #endregion
                    #region RECEIPT HEADER
                    PdfPTable receiptTable = new PdfPTable(3); receiptTable.SpacingBefore = 20f;
                    receiptTable.WidthPercentage = 100; receiptTable.SetWidths(new float[] { 33.33f, 33.33f, 33.33f });
                    PdfPTable receiptNoTable = new PdfPTable(2);
                    receiptNoTable.WidthPercentage = 100; receiptNoTable.SetWidths(new float[] { 28f, 72f });
                    receiptNoTable.AddCell(new PdfPCell() { Phrase = new Phrase("No. :", FontFactory.GetFont(fontName, 11, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingLeft = 10 });
                    receiptNoTable.AddCell(new PdfPCell() { Phrase = new Phrase(ReceiptPrefix + DbHelper.Configuration.ReceiptNo(ReceiptNo), FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingLeft = 0 });
                    PdfPCell AddCellA = new PdfPCell(receiptNoTable); AddCellA.Border = PdfPCell.NO_BORDER;
                    receiptTable.AddCell(AddCellA);
                    receiptTable.AddCell(new PdfPCell() { Phrase = new Phrase("RECEIPT", FontFactory.GetFont(fontName, 11, Font.BOLD | Font.UNDERLINE, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingLeft = 10 });
                    receiptNoTable = new PdfPTable(2);
                    receiptNoTable.WidthPercentage = 100; receiptNoTable.SetWidths(new float[] { 50f, 50f });
                    receiptNoTable.AddCell(new PdfPCell() { Phrase = new Phrase("Date :", FontFactory.GetFont(fontName, 11, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_RIGHT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingLeft = 10 });
                    receiptNoTable.AddCell(new PdfPCell() { Phrase = new Phrase((PayDate > DateTime.MinValue ? PayDate.ToString("dd/MM/yyyy") : string.Empty), FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingLeft = 0 });
                    AddCellA = new PdfPCell(receiptNoTable); AddCellA.Border = PdfPCell.NO_BORDER;
                    receiptTable.AddCell(AddCellA);
                    document.Add(receiptTable);
                    #endregion
                    bool hasChqueDetail = false;

                    #region CONTENT

                    PdfPTable contentTable = new PdfPTable(3); contentTable.SpacingBefore = 20f;
                    contentTable.WidthPercentage = 100F; contentTable.SetWidths(new float[] { 40F, 57F, 3F });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase("Received with thanks from Mr./Mrs. ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(FullName.ToUpper(), fontItalic), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderWidthBottom = 1F, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(" ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    document.Add(contentTable);

                    string outwords = DbHelper.NumberToText.NumbersToWords((decimal)NetAmt);
                    contentTable = new PdfPTable(3); contentTable.SpacingBefore = 5f;
                    contentTable.WidthPercentage = 100F; contentTable.SetWidths(new float[] { 23F, 74F, 3F });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase("the sum of Rupees ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(outwords, fontItalic), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderWidthBottom = 1F, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(" ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });

                    document.Add(contentTable);

                    string chequeDetail = string.Empty;
                    cmd = new SqlCommand("Receipt_PrintPayment"); cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
                    cmd.Parameters.Add("@TableID", SqlDbType.BigInt).Value = TableID;
                    cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = _test;
                    DataTable dtCq = db.DbRead(cmd);
                    if (dtCq.Rows.Count > 0)
                    {
                        for (int pt = 0; pt < dtCq.Rows.Count; pt++)
                        {
                            string ChequeTxnNo = dtCq.Rows[pt]["ChequeTxnNo"].ToString();
                            int PayMode = 0; int.TryParse(dtCq.Rows[pt]["PayMode"].ToString(), out PayMode);
                            if (PayMode == 3)
                            {
                                string BankName = dtCq.Rows[pt]["BankName"].ToString();
                                string BankBranch = dtCq.Rows[pt]["BankBranch"].ToString();
                                DateTime ChequeDate = new DateTime(); DateTime.TryParseExact(dtCq.Rows[pt]["ChequeDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ChequeDate);
                                chequeDetail = string.Empty;
                                if (!string.IsNullOrEmpty(ChequeTxnNo)) { chequeDetail += "Cheque No. " + ChequeTxnNo.Replace(Environment.NewLine, " "); }
                                chequeDetail = chequeDetail.Trim();
                                if (!string.IsNullOrEmpty(BankName)) { chequeDetail += ((chequeDetail.Length > 0 ? ", " + Environment.NewLine : "") + BankName.Replace(Environment.NewLine, " ")); }
                                chequeDetail = chequeDetail.Trim();
                                if (!string.IsNullOrEmpty(BankBranch)) { chequeDetail += ((chequeDetail.Length > 0 ? ", " + Environment.NewLine : "") + BankBranch.Replace(Environment.NewLine, " ")); }
                                chequeDetail = chequeDetail.Trim();
                                if (chequeDetail.Length > 0)
                                {
                                    chequeDetail += ".";
                                    hasChqueDetail = true;
                                }
                                //chequeDetail += " " + (ChequeDate > DateTime.MinValue ? ChequeDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty);
                                //chequeDetail = chequeDetail.Replace(Environment.NewLine, " ");
                                break;
                            }
                            if (PayMode == 4)
                            {
                                chequeDetail = string.Empty;
                                if (!string.IsNullOrEmpty(ChequeTxnNo)) { chequeDetail += "Transaction ID :" + Environment.NewLine + ChequeTxnNo.Replace(Environment.NewLine, " "); }
                                chequeDetail = chequeDetail.Trim();
                                if (chequeDetail.Length > 0)
                                {
                                    chequeDetail += ".";
                                    hasChqueDetail = true;
                                }
                                break;
                            }
                        }
                    }
                    if (hasChqueDetail)
                    {
                        chequeDetail += (PrintRemark.Length > 0 ? Environment.NewLine + "(" + PrintRemark + ")" : "");
                    }
                    else
                    {
                        hasChqueDetail = true;
                        chequeDetail = (PrintRemark.Length > 0 ? Environment.NewLine + "(" + PrintRemark + ")" : "");
                    }
                    contentTable = new PdfPTable(4); contentTable.SpacingBefore = 5f;
                    contentTable.WidthPercentage = 100F; contentTable.SetWidths(new float[] { 2F, 65F, 30F, 3F });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(" ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(chequeDetail, fontItalic), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderWidthBottom = (hasChqueDetail ? 0F : 1F), BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 0 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase("towords professional fees.", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    contentTable.AddCell(new PdfPCell() { Phrase = new Phrase(" ", fontNormal), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, BorderColor = FontColour, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 10 });
                    document.Add(contentTable);

                    #endregion
                    #region FOOTER

                    PdfPTable footerTable = new PdfPTable(2); footerTable.SpacingBefore = (hasChqueDetail ? 0F : 20f);
                    footerTable.WidthPercentage = 100F; footerTable.SetWidths(new float[] { 50f, 50f });

                    PdfPTable footerAmtTable = new PdfPTable(2);
                    footerAmtTable.WidthPercentage = 100F; footerAmtTable.SetWidths(new float[] { 30f, 65f });
                    footerAmtTable.AddCell(new PdfPCell() { Phrase = new Phrase("Rs.", FontFactory.GetFont(fontName, 13, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_RIGHT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingTop = 8, PaddingLeft = 10, PaddingRight = 0, });
                    footerAmtTable.AddCell(new PdfPCell() { Phrase = new Phrase(NetAmt.ToString() + "/-", FontFactory.GetFont(fontName, 13, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingTop = 8, PaddingLeft = 0, PaddingRight = 0, });
                    PdfPCell footerAmtCellA = new PdfPCell(footerAmtTable); footerAmtCellA.BorderColor = FontColour; footerAmtCellA.BorderWidthBottom = 2f; footerAmtCellA.BorderWidthRight = 2f;
                    PdfPTable footerAmtOuterTable = new PdfPTable(2); footerAmtOuterTable.WidthPercentage = 100F; footerAmtOuterTable.SetWidths(new float[] { 50f, 50f });
                    footerAmtOuterTable.AddCell(footerAmtCellA);
                    footerAmtOuterTable.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER, });
                    PdfPCell footerAmtOuterCellA = new PdfPCell(footerAmtOuterTable); footerAmtOuterCellA.Border = PdfPCell.NO_BORDER;
                    footerAmtOuterCellA.PaddingLeft = 10f; footerAmtOuterCellA.PaddingTop = 50F; footerAmtOuterCellA.PaddingRight = 50F;
                    footerTable.AddCell(footerAmtOuterCellA);

                    PdfPTable footerSignOuterTable = new PdfPTable(2); footerSignOuterTable.WidthPercentage = 100F; footerSignOuterTable.SetWidths(new float[] { 70f, 30f });
                    footerSignOuterTable.AddCell(new PdfPCell() { Phrase = new Phrase(""), Border = PdfPCell.NO_BORDER });
                    PdfPTable footerSignInnerTable = new PdfPTable(1); footerSignInnerTable.WidthPercentage = 100F; footerSignInnerTable.SetWidths(new float[] { 100F });
                    PdfPTable footerSignInnerNameTable = new PdfPTable(2); footerSignInnerNameTable.WidthPercentage = 100F; footerSignInnerNameTable.SetWidths(new float[] { 17F, 83F });
                    footerSignInnerNameTable.AddCell(new PdfPCell() { Phrase = new Phrase("  For ", FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingTop = 8, PaddingLeft = 0 });
                    footerSignInnerNameTable.AddCell(new PdfPCell() { Phrase = new Phrase("Dr. Snehal G. Deshpande  ", FontFactory.GetFont(fontName, 11, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, Border = PdfPCell.NO_BORDER, PaddingBottom = 10, PaddingTop = 8, PaddingLeft = 0 });
                    PdfPCell footerSignNameCell = new PdfPCell(footerSignInnerNameTable); footerSignNameCell.Border = PdfPCell.NO_BORDER;
                    footerSignInnerTable.AddCell(footerSignNameCell);


                    iTextSharp.text.Image imagenew = null;
                    string logoPathnew = HttpContext.Current.Server.MapPath("~/images/compressedsnehalsign.jpg");
                    if (File.Exists(logoPathnew))
                    {
                        imagenew = iTextSharp.text.Image.GetInstance(logoPathnew);
                        imagenew.ScaleToFitLineWhenOverflow = false;
                        imagenew.ScaleAbsoluteWidth(120F); imagenew.ScaleAbsoluteHeight(47F);
                    }


                    PdfPTable footerSignInnerImgTable = new PdfPTable(1); footerSignInnerImgTable.WidthPercentage = 100F; footerSignInnerImgTable.SetWidths(new float[] { 100F });
                    PdfPCell footerSignImgInnerCell = new PdfPCell(imagenew); footerSignImgInnerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER; footerSignImgInnerCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    footerSignImgInnerCell.PaddingTop = 10F;
                    footerSignImgInnerCell.BorderWidthBottom = 0F; footerSignImgInnerCell.BorderColor = FontColour;
                    footerSignInnerImgTable.AddCell(footerSignImgInnerCell);
                    footerSignImgInnerCell = new PdfPCell() { Phrase = new Phrase("Signature", FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 0 };
                    footerSignImgInnerCell.BorderWidthTop = 0F; footerSignImgInnerCell.BorderColor = FontColour;
                    footerSignInnerImgTable.AddCell(footerSignImgInnerCell);
                    PdfPCell footerSignImgCell = new PdfPCell(footerSignInnerImgTable); footerSignImgCell.Border = PdfPCell.NO_BORDER; footerSignImgCell.PaddingRight = 25f;
                    footerSignInnerTable.AddCell(footerSignImgCell);

                    //PdfPTable footerSignInnerImgTable = new PdfPTable(1); footerSignInnerImgTable.WidthPercentage = 100F; footerSignInnerImgTable.SetWidths(new float[] { 100F });
                    //PdfPCell footerSignImgInnerCell = new PdfPCell() { Phrase = new Phrase(" ", FontFactory.GetFont(fontName, 11, Font.BOLD, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfPCell.ALIGN_TOP, PaddingBottom = 10, PaddingTop = 8, PaddingLeft = 0 };
                    //footerSignImgInnerCell.BorderWidthBottom = 0F; footerSignImgInnerCell.BorderColor = FontColour;
                    //footerSignInnerImgTable.AddCell(footerSignImgInnerCell);
                    //footerSignImgInnerCell = new PdfPCell() { Phrase = new Phrase("Signature", FontFactory.GetFont(fontName, 11, Font.NORMAL, FontColour)), HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfPCell.ALIGN_TOP, PaddingBottom = 5, PaddingTop = 8, PaddingLeft = 0 };
                    //footerSignImgInnerCell.BorderWidthTop = 0F; footerSignImgInnerCell.BorderColor = FontColour;
                    //footerSignInnerImgTable.AddCell(footerSignImgInnerCell);
                    //PdfPCell footerSignImgCell = new PdfPCell(footerSignInnerImgTable); footerSignImgCell.Border = PdfPCell.NO_BORDER; footerSignImgCell.PaddingRight = 25f;
                    //footerSignInnerTable.AddCell(footerSignImgCell);


                    PdfPCell footerSignOuterCell = new PdfPCell(footerSignInnerTable); footerSignOuterCell.PaddingLeft = 60F; footerSignOuterCell.Border = PdfPCell.NO_BORDER;
                    footerSignOuterTable.AddCell(footerSignOuterCell);

                    footerTable.AddCell(footerSignOuterCell);
                    document.Add(footerTable);

                    #endregion

                    float curY = writer.GetVerticalPosition(true);

                    #region OUTER BORDER

                    PdfContentByte content = writer.DirectContent;
                    Rectangle rectangle = new Rectangle(document.PageSize);
                    rectangle.Left += document.LeftMargin;
                    rectangle.Right -= document.RightMargin;
                    rectangle.Top -= document.TopMargin;
                    rectangle.Bottom += (curY - document.BottomMargin + 18F);
                    content.SetLineWidth(1f);
                    content.SetColorStroke(FontColour);
                    content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                    content.Stroke();

                    rectangle = new Rectangle(document.PageSize);
                    rectangle.Left += (document.LeftMargin + 3F);
                    rectangle.Right -= (document.RightMargin + 3F);
                    rectangle.Top -= (document.TopMargin + 3F);
                    rectangle.Bottom += (curY - document.BottomMargin + 18F + 3F);
                    content.SetLineWidth(1f);
                    content.SetColorStroke(FontColour);
                    content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                    content.Stroke();

                    #endregion
                    //document.Close();
                    //byte[] bytes = memoryStream.ToArray();
                    //memoryStream.Close();
                    //context.Response.Clear();
                    //context.Response.ContentType = "application/pdf";
                    //context.Response.AddHeader("Content-Disposition", "inline; filename=" + DbHelper.Configuration.MakeValidFilename(FullName + ".pdf"));
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
                    mail = AM.sendAttachment(mailid, "", "Your Receipt", result_sheet);
                    if (mail)
                    {
                        context.Session[DbHelper.Configuration.messageTextSession] = "Mail Send Successfully";
                        context.Session[DbHelper.Configuration.messageTypeSession] = "1";
                        HttpResponse response = context.Response;
                        response.Redirect("/Member/SendMail.aspx?pid=" + patientid + "&id=" + context.Request.QueryString["id"]);

                    }
                    else
                    {
                        context.Session[DbHelper.Configuration.messageTextSession] = "Unable to process";
                        context.Session[DbHelper.Configuration.messageTypeSession] = "2";
                        HttpResponse response = context.Response;
                        response.Redirect("/Member/SendMail.aspx?pid=" + patientid + "&id=" + context.Request.QueryString["id"]);
                    }
                    //context.Response.Redirect("~/Files/Receipt/" + _receipt_no);
                }
            }
            else
            {
                context.Response.AddHeader("refresh", "5;/Member/");
                context.Response.ContentType = "text/html";
                context.Response.Write(str + "<div class=\"alert\">Unable to find receipt detail, Please try again.</div>");
                context.Response.End();
                return;

                //r.msg = "Unable to find receipt detail, Please try again.";
                //context.Response.Write(JsonConvert.SerializeObject(r));
                //return;

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class MailPDF
        {
            public string MailID { get; set; }
            public string ID { get; set; }
        }
    }
}