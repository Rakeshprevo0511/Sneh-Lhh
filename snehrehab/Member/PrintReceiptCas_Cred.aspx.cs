using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class PrintReceiptCas_Cred : System.Web.UI.Page
    {
        int _loginID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID != 1 && _catID != 2 && _catID != 4 && _catID != 6)
            {
                Response.Redirect("/Member/"); return;
            }
            if (!IsPostBack)
            {
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                   1// DateTime.UtcNow.AddMinutes(330).Day
                    ).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }

        }
        public string GetReceipt(string prefix, string recno)
        {
            string receipt = string.Empty; long recpno = 0;

            if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(recno))
            {
                long.TryParse(recno.ToString(), out recpno);
                receipt = prefix + "" + DbHelper.Configuration.ReceiptNo(recpno);
            }
            return receipt;
        }

        private void LoadData()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date.", 2); return;
            }
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date.", 2); return;
            }
            DateTime StartDate = new DateTime(); StartDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year - 1 : _fromDate.Year), 4, 1);
            DateTime EndDate = new DateTime(); EndDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year : _fromDate.Year + 1), 3, 31);
            if (_uptoDate < StartDate)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            }
            if (_uptoDate > EndDate)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            }
            //SqlCommand cmd = new SqlCommand("Receipt_PrintSearch"); cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Receipt_PrintSearch_ALL_NEW"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);

            BookingGV.DataSource = dt;
            BookingGV.DataBind();
            if (BookingGV.HeaderRow != null) { BookingGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

            decimal _totalAmt = 0;
            if (dt.Rows.Count > 0)
            {
                decimal.TryParse(dt.Compute("SUM(NetAmt)", string.Empty).ToString(), out _totalAmt);
            }
            lblTotal.Text = Math.Round(_totalAmt, 2).ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BookingGV.PageIndex = 0; LoadData();
        }

        protected void BookingGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BookingGV.PageIndex = e.NewPageIndex; LoadData();
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string FORMATLDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat) + " " + _test.ToString(DbHelper.Configuration.showTimeFormat);
            return "- - -";
        }

        public string GetUrlQuery(string UniqueID, string ReceiptType, string FiscalDate, string ReceiptNo)
        {
            snehrehab.rModel r = new snehrehab.rModel();
            string lData = (new JavaScriptSerializer().Serialize(new
            {
                un = UniqueID,
                rt = ReceiptType,
                fy = FiscalDate,
                rn = ReceiptNo,
            }));
            return HttpUtility.UrlEncode(r.Encode(lData));
        }

        public string GetCentreName(string centre)
        {
            if (!string.IsNullOrEmpty(centre))
            {
                return centre + " " + "OPD";
            }
            else
            {
                return "OPD";
            }
        }

        public string GetTxnNo(string rectype, string tableid, string fiscaldate)
        {
            string ChequeTxnNo = string.Empty;
            DateTime FiscalDate = new DateTime(); DateTime.TryParseExact(fiscaldate, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FiscalDate);
            int ReceiptType = 0; int.TryParse(rectype, out ReceiptType);
            long TableID = 0; long.TryParse(tableid, out TableID);
            SqlCommand cmd = new SqlCommand("Receipt_PrintPayment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
            cmd.Parameters.Add("@TableID", SqlDbType.BigInt).Value = TableID;
            if (FiscalDate > DateTime.MinValue)
                cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = FiscalDate;
            else
                cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DBNull.Value;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dtCq = db.DbRead(cmd);
            if (dtCq.Rows.Count > 0)
            {
                for (int i = 0; i < dtCq.Rows.Count; i++)
                {
                    ChequeTxnNo = dtCq.Rows[i]["ChequeTxnNo"].ToString();
                }
            }
            return ChequeTxnNo;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select from date.", 2); return;
            }
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select upto date.", 2); return;
            }
            DateTime StartDate = new DateTime(); StartDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year - 1 : _fromDate.Year), 4, 1);
            DateTime EndDate = new DateTime(); EndDate = new DateTime((_fromDate.Month <= 3 ? _fromDate.Year : _fromDate.Year + 1), 3, 31);
            if (_uptoDate <= StartDate)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            }
            if (_uptoDate > EndDate)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct financial dates.", 2); return;
            }
            SqlCommand cmd = new SqlCommand("Receipt_PrintSearch_ALL_NEW"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = txtSearch.Text.Trim();
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 BranchName FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPP = db.DbRead(cmd); if (dtPP.Rows.Count > 0) { centrename = dtPP.Rows[0]["BranchName"].ToString(); }
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>" + GetCentreName(centrename) + "</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>RECEIPT NO.</th><th>AMOUNT</th><th>RECEIPT DATE</th><th>Hospital Receipt ID</th><th>BOOKING DATE</th><th>Hospital Receipt Date</th><th>CANCEL</th><th>PACKAGE CODE</th></tr>");
                string ReceiptPrefix = string.Empty; float TotalAmount = 0;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtP = db.DbRead(cmd); if (dtP.Rows.Count > 0) { ReceiptPrefix = dtP.Rows[0]["ReceiptPrefix"].ToString(); }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long ReceiptNo = 0; long.TryParse(dt.Rows[i]["ReceiptNo"].ToString(), out ReceiptNo);
                    float NetAmt = 0; float.TryParse(dt.Rows[i]["NetAmt"].ToString(), out NetAmt);
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + ReceiptPrefix + DbHelper.Configuration.ReceiptNo(ReceiptNo) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["NetAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["PayDate"].ToString()) + "</td>");

                    int ReceiptType = 0; int.TryParse(dt.Rows[i]["ReceiptType"].ToString(), out ReceiptType);
                    long TableID = 0; long.TryParse(dt.Rows[i]["TableID"].ToString(), out TableID);
                    DateTime FiscalDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["FiscalDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FiscalDate);
                    string chequeDetail = string.Empty; //bool hasChqueDetail = false;
                    cmd = new SqlCommand("Receipt_PrintPayment"); cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
                    cmd.Parameters.Add("@TableID", SqlDbType.BigInt).Value = TableID;
                    if (FiscalDate > DateTime.MinValue)
                        cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = FiscalDate;
                    else
                        cmd.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = DBNull.Value;
                    DataTable dtCq = db.DbRead(cmd);
                    if (dtCq.Rows.Count > 0)
                    {
                        for (int pt = 0; pt < dtCq.Rows.Count; pt++)
                        {
                            string ChequeTxnNo = dtCq.Rows[pt]["ChequeTxnNo"].ToString(); int PayMode = 0; int.TryParse(dtCq.Rows[pt]["PayMode"].ToString(), out PayMode);
                            if (PayMode == 1 || PayMode == 2)
                            {
                                 ChequeTxnNo = dtCq.Rows[pt]["ChequeTxnNo"]?.ToString(); chequeDetail = string.Empty;
                                if (!string.IsNullOrWhiteSpace(ChequeTxnNo))
                                {
                                    chequeDetail =  ChequeTxnNo.Replace(Environment.NewLine, " ");
                                }
                                break;
                            }
                           
                        }
                    }
                    html.Append("<td style=\"vertical-align:top;\">" + chequeDetail + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATLDATE(dt.Rows[i]["AddedDate"].ToString()).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["HospitalReceiptDate"].ToString()).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["IsDeleted"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Remarks"].ToString() + "</td>");
                    html.Append("</tr>"); TotalAmount += NetAmt;
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total : </b></td>");
                html.Append("<td style=\"vertical-align:top;\"><b>" + TotalAmount.ToString() + "</b></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("<td style=\"vertical-align:top;\"></td>");
                html.Append("</tr>");

                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=print receipt report.xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No record found to export.", 3); return;
            }
        }
    }
}