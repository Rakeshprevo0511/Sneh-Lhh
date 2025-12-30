using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Globalization;
using System.Text;


namespace snehrehab.Reports
{
    public partial class YearlyAccount : System.Web.UI.Page
    {
        int _loginID = 0; public DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; LoadData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataSet ds = DB.YearlyAccount(_fromDate, _uptoDate);
            dt = ds.Tables[0];

            ReportGV.DataSource = dt;
            ReportGV.DataBind();
            if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataSet ds = DB.YearlyAccount(_fromDate, _uptoDate);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table>");
                html.Append("<tr><td><b>Report Name:</b></td><td>Yearly Account Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\">");
                html.Append("<tr><th>DATE</th><th>TOTAL</th><th>EXP</th><th>OTHER CASH</th><th>C. P.</th><th>S. T.</th><th>NOPD</th><th>DIET</th><th>MATRIX</th><th>V. T.</th><th>P.O</th><th>WALK AID</th><th>S. EDU</th><th>H. V.</th><th>H. P.</th><th>A. B. T.</th><th>S.H.V</th><th>CHEQUE</th><th>ONLINE</th><th>CASH</th>");
                html.Append("</tr>");
                decimal _totalAmt = 0, _cashEntry = 0, _expAmt = 0, _clinicalAmt = 0, _speechAmt = 0, _nopdAmt = 0, _dietAmt = 0, _matrixAmt = 0, _visionAmt = 0, _walkAidAmt = 0, _specialEduAmt = 0, _HomeVisitEvaluationAmt = 0, _HomeProgrammAmt = 0, _ArtBasedTherapyAmt = 0, _cheqAmt = 0, _OnlineAmt = 0, _cashAmt = 0, __paediatricorthoamt = 0,
                _speechhomevisit = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr><td style=\"vertical-align:top;\">" + DateName(dt.Rows[i]["PayDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["TotalAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ExpAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["CashEntry"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ClinicalAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SpeechAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["NopdAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["DietAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MatrixAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["VisionTherapyAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PaediatricOrthopaedicAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["WalkAidAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SpecialEdu"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["HomeVisitEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["HomeProgramm"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ArtBasedTherapy"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SpeechHomeVisitEvaluation"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["CheqAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["OnlineAmt"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["CashAmt"].ToString() + "</td>");

                    decimal _totalAmtTmp = 0, _cashEntryTmp = 0, _expAmtTmp = 0, _clinicalAmtTmp = 0, _speechAmtTmp = 0, _nopdAmtTmp = 0, _dietAmtTmp = 0, _matrixAmtTmp = 0, _visionAmtTmp = 0, _walkAidAmtTmp = 0, _specialEduAmtTmp = 0, _HomeVisitEvaluationAmtTmp = 0, _HomeProgrammAmtTmp = 0, _ArtBasedTherapyEduAmtTmp = 0, _cheqAmtTmp = 0, _OnlineAmtTmp = 0, _cashAmtTmp = 0, _paediatricorthoamtTmp = 0, _speechhomevisitTmp = 0;

                    decimal.TryParse(dt.Rows[i]["TotalAmt"].ToString(), out _totalAmtTmp); _totalAmt += _totalAmtTmp;
                    decimal.TryParse(dt.Rows[i]["ExpAmt"].ToString(), out _expAmtTmp); _expAmt += _expAmtTmp;
                    decimal.TryParse(dt.Rows[i]["CashEntry"].ToString(), out _cashEntryTmp); _cashEntry += _cashEntryTmp;
                    decimal.TryParse(dt.Rows[i]["ClinicalAmt"].ToString(), out _clinicalAmtTmp); _clinicalAmt += _clinicalAmtTmp;
                    decimal.TryParse(dt.Rows[i]["SpeechAmt"].ToString(), out _speechAmtTmp); _speechAmt += _speechAmtTmp;
                    decimal.TryParse(dt.Rows[i]["NopdAmt"].ToString(), out _nopdAmtTmp); _nopdAmt += _nopdAmtTmp;
                    decimal.TryParse(dt.Rows[i]["DietAmt"].ToString(), out _dietAmtTmp); _dietAmt += _dietAmtTmp;
                    decimal.TryParse(dt.Rows[i]["MatrixAmt"].ToString(), out _matrixAmtTmp); _matrixAmt += _matrixAmtTmp;
                    decimal.TryParse(dt.Rows[i]["VisionTherapyAmt"].ToString(), out _visionAmtTmp); _visionAmt += _visionAmtTmp;
                    decimal.TryParse(dt.Rows[i]["PaediatricOrthopaedicAmt"].ToString(), out _paediatricorthoamtTmp); __paediatricorthoamt += _paediatricorthoamtTmp;
                    decimal.TryParse(dt.Rows[i]["WalkAidAmt"].ToString(), out _walkAidAmtTmp); _walkAidAmt += _walkAidAmtTmp;
                    decimal.TryParse(dt.Rows[i]["SpecialEdu"].ToString(), out _specialEduAmtTmp); _specialEduAmt += _specialEduAmtTmp;
                    decimal.TryParse(dt.Rows[i]["HomeVisitEvaluation"].ToString(), out _HomeVisitEvaluationAmtTmp); _HomeVisitEvaluationAmt += _HomeVisitEvaluationAmtTmp;
                    decimal.TryParse(dt.Rows[i]["HomeProgramm"].ToString(), out _HomeProgrammAmtTmp); _HomeProgrammAmt += _HomeProgrammAmtTmp;
                    decimal.TryParse(dt.Rows[i]["ArtBasedTherapy"].ToString(), out _ArtBasedTherapyEduAmtTmp); _ArtBasedTherapyAmt += _ArtBasedTherapyEduAmtTmp;
                    decimal.TryParse(dt.Rows[i]["CheqAmt"].ToString(), out _cheqAmtTmp); _cheqAmt += _cheqAmtTmp;
                    decimal.TryParse(dt.Rows[i]["OnlineAmt"].ToString(), out _OnlineAmtTmp); _OnlineAmt += _OnlineAmtTmp;
                    decimal.TryParse(dt.Rows[i]["CashAmt"].ToString(), out _cashAmtTmp); _cashAmt += _cashAmtTmp;
                    decimal.TryParse(dt.Rows[i]["SpeechHomeVisitEvaluation"].ToString(), out _speechhomevisit); _speechhomevisit += _speechhomevisitTmp;

                    //for (int m = 1; m < ds.Tables[2].Columns.Count; m++)
                    //{
                    //    html.Append("<td style=\"vertical-align:top;\">" + ds.Tables[2].Rows[i][m].ToString() + "</td>");
                    //}
                    //for (int j = 1; j < ds.Tables[1].Columns.Count; j++)
                    //{
                    //    html.Append("<td style=\"vertical-align:top;\">" + ds.Tables[1].Rows[i][j].ToString() + "</td>");
                    //}
                    html.Append("</tr>");
                }
                html.Append("<tr><td style=\"vertical-align:top;\"><b>Total:</b></td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_totalAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_expAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_cashEntry, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_clinicalAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_speechAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_nopdAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_dietAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_matrixAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_visionAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(__paediatricorthoamt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_walkAidAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_specialEduAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_HomeVisitEvaluationAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_HomeProgrammAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_ArtBasedTherapyAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_speechhomevisit, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_cheqAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_OnlineAmt, 2).ToString() + "</td>");
                html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_cashAmt, 2).ToString() + "</td>");


                //for (int k = 1; k < ds.Tables[2].Columns.Count; k++)
                //{
                //    decimal _otheractTotal = 0;
                //    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //    {
                //        decimal _otheractTotalTmp = 0;
                //        decimal.TryParse(ds.Tables[2].Rows[i][k].ToString(), out _otheractTotalTmp);
                //        _otheractTotal += _otheractTotalTmp;
                //    }
                //    html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_otheractTotal, 2).ToString() + "</td>");
                //}
                //for (int j = 1; j < ds.Tables[1].Columns.Count; j++)
                //{
                //    decimal _dTotal = 0;
                //    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //    {
                //        decimal _dTotalTmp = 0; decimal.TryParse(ds.Tables[1].Rows[i][j].ToString(), out _dTotalTmp);
                //        _dTotal += _dTotalTmp;
                //    }
                //    html.Append("<td style=\"vertical-align:top;\">" + Math.Round(_dTotal, 2).ToString() + "</td>");
                //}

                html.Append("</tr>");


                html.Append("</table>");

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=yearly account report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }

            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 2);
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
        public string DateName(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date)) return date;
                DateTime dat = Convert.ToDateTime(date);
                return dat.ToString("MMMM") + " - " + dat.Year.ToString();
            }
            catch { return date; }
        }
        public string GetText(string str, string _str, string t)
        {
            float amt = 0; float.TryParse(str, out amt);
            if (amt > 0)
            {
                DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
                if (_test > DateTime.MinValue)
                {
                    return "<a href=\"javascript:;\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"LoadAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + t + ")\">" + amt.ToString() + "</a>";
                }
                return amt.ToString();
            }
            return "- - -";
        }

        protected void ReportGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                decimal _totalAmt = 0, _expAmt = 0, _otherCash = 0, _clinicalAmt = 0, _speechAmt = 0, _nopdAmt = 0, _dietAmt = 0, _matrixAmt = 0, _visionAmt = 0, _walkAidAmt = 0, _specialEduAmt = 0, _HomeVisitEvaluationEduAmt = 0, _HomeProgramAmt = 0, _ArtBasedTherapyEduAmt = 0, _cheqAmt = 0, _onlineAmt = 0, _cashAmt = 0, _paediatricOrthopaedicAmt = 0, _musictherapy = 0, _speechhomevisit = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    decimal _totalAmtTmp = 0, _expAmtTmp = 0, _otherCashTmp = 0, _clinicalAmtTmp = 0, _speechAmtTmp = 0, _nopdAmtTmp = 0, _dietAmtTmp = 0, _matrixAmtTmp = 0, _visionAmtTmp = 0, _walkAidAmtTmp = 0, _specialEduAmtTmp = 0, _HomeVisitEvaluationAmtTmp = 0, _HomeProgramAmtTmp = 0, _ArtBasedTherapyAmtTmp = 0, _cheqAmtTmp = 0, _onlineAmtTmp = 0, _cashAmtTmp = 0, _paediatricOrthopaedicAmtTmp = 0, _musictherapytmp = 0, _speechhomevisittmp = 0;

                    decimal.TryParse(dt.Rows[i]["TotalAmt"].ToString(), out _totalAmtTmp);
                    decimal.TryParse(dt.Rows[i]["ExpAmt"].ToString(), out _expAmtTmp);
                    decimal.TryParse(dt.Rows[i]["CashEntry"].ToString(), out _otherCashTmp);
                    decimal.TryParse(dt.Rows[i]["ClinicalAmt"].ToString(), out _clinicalAmtTmp);
                    decimal.TryParse(dt.Rows[i]["SpeechAmt"].ToString(), out _speechAmtTmp);
                    decimal.TryParse(dt.Rows[i]["NopdAmt"].ToString(), out _nopdAmtTmp);
                    decimal.TryParse(dt.Rows[i]["DietAmt"].ToString(), out _dietAmtTmp);
                    decimal.TryParse(dt.Rows[i]["MatrixAmt"].ToString(), out _matrixAmtTmp);
                    decimal.TryParse(dt.Rows[i]["CheqAmt"].ToString(), out _cheqAmtTmp);
                    decimal.TryParse(dt.Rows[i]["OnlineAmt"].ToString(), out _onlineAmtTmp);
                    decimal.TryParse(dt.Rows[i]["CashAmt"].ToString(), out _cashAmtTmp);
                    decimal.TryParse(dt.Rows[i]["VisionTherapyAmt"].ToString(), out _visionAmtTmp);
                    decimal.TryParse(dt.Rows[i]["WalkAidAmt"].ToString(), out _walkAidAmtTmp);
                    decimal.TryParse(dt.Rows[i]["SpecialEdu"].ToString(), out _specialEduAmtTmp);
                    decimal.TryParse(dt.Rows[i]["HomeVisitEvaluation"].ToString(), out _HomeVisitEvaluationAmtTmp);
                    decimal.TryParse(dt.Rows[i]["HomeProgramm"].ToString(), out _HomeProgramAmtTmp);
                    decimal.TryParse(dt.Rows[i]["ArtBasedTherapy"].ToString(), out _ArtBasedTherapyAmtTmp);
                    decimal.TryParse(dt.Rows[i]["PaediatricOrthopaedicAmt"].ToString(), out _paediatricOrthopaedicAmtTmp);
                    decimal.TryParse(dt.Rows[i]["MusicTherapy"].ToString(), out _musictherapytmp);
                    decimal.TryParse(dt.Rows[i]["SpeechHomeVisitEvaluation"].ToString(), out _speechhomevisittmp);


                    _totalAmt += _totalAmtTmp;
                    _expAmt += _expAmtTmp;
                    _onlineAmt += _onlineAmtTmp;
                    _otherCash += _otherCashTmp;
                    _clinicalAmt += _clinicalAmtTmp;
                    _speechAmt += _speechAmtTmp;
                    _nopdAmt += _nopdAmtTmp;
                    _dietAmt += _dietAmtTmp;
                    _matrixAmt += _matrixAmtTmp;
                    _cheqAmt += _cheqAmtTmp;
                    _cashAmt += _cashAmtTmp;
                    _visionAmt += _visionAmtTmp;
                    _walkAidAmt += _walkAidAmtTmp;
                    _specialEduAmt += _specialEduAmtTmp;
                    _HomeVisitEvaluationEduAmt += _HomeVisitEvaluationAmtTmp;
                    _HomeProgramAmt += _HomeProgramAmtTmp;
                    _ArtBasedTherapyEduAmt += _ArtBasedTherapyAmtTmp;
                    _paediatricOrthopaedicAmt += _paediatricOrthopaedicAmtTmp;
                    _musictherapy += _musictherapytmp;
                    _speechhomevisit += _speechhomevisittmp;
                }
                Label _totalAmtLbl = e.Row.FindControl("lblTotal") as Label;
                if (_totalAmtLbl != null) { _totalAmtLbl.Text = Math.Round(_totalAmt, 2).ToString(); }
                Label _expAmtLbl = e.Row.FindControl("lblExp") as Label;
                if (_expAmtLbl != null) { _expAmtLbl.Text = Math.Round(_expAmt, 2).ToString(); }
                Label _otherCashLbl = e.Row.FindControl("lblOtherCash") as Label;
                if (_otherCashLbl != null) { _otherCashLbl.Text = Math.Round(_otherCash, 2).ToString(); }
                Label _clinicalAmtLbl = e.Row.FindControl("lblCp") as Label;
                if (_clinicalAmtLbl != null) { _clinicalAmtLbl.Text = Math.Round(_clinicalAmt, 2).ToString(); }
                Label _speechAmtLbl = e.Row.FindControl("lblSt") as Label;
                if (_speechAmtLbl != null) { _speechAmtLbl.Text = Math.Round(_speechAmt, 2).ToString(); }
                Label _nopdAmtLbl = e.Row.FindControl("lblNopd") as Label;
                if (_nopdAmtLbl != null) { _nopdAmtLbl.Text = Math.Round(_nopdAmt, 2).ToString(); }
                Label _dietAmtLbl = e.Row.FindControl("lblDiet") as Label;
                if (_dietAmtLbl != null) { _dietAmtLbl.Text = Math.Round(_dietAmt, 2).ToString(); }
                Label _matrixAmtLbl = e.Row.FindControl("lblMatrix") as Label;
                if (_matrixAmtLbl != null) { _matrixAmtLbl.Text = Math.Round(_matrixAmt, 2).ToString(); }
                Label _visionAmtLbl = e.Row.FindControl("lblVision") as Label;
                if (_visionAmtLbl != null) { _visionAmtLbl.Text = Math.Round(_visionAmt, 2).ToString(); }
                Label _walkAidAmtLbl = e.Row.FindControl("lblWalkAid") as Label;
                if (_walkAidAmtLbl != null) { _walkAidAmtLbl.Text = Math.Round(_walkAidAmt, 2).ToString(); }
                Label _specialEduAmtLbl = e.Row.FindControl("lblSpecialEdu") as Label;
                if (_specialEduAmtLbl != null) { _specialEduAmtLbl.Text = Math.Round(_specialEduAmt, 2).ToString(); }
                Label lblHomeVisitEvaluation = e.Row.FindControl("lblHomeVisitEvaluation") as Label;
                if (lblHomeVisitEvaluation != null) { lblHomeVisitEvaluation.Text = Math.Round(_HomeVisitEvaluationEduAmt, 2).ToString(); }
                Label lblHomeProgramm = e.Row.FindControl("lblHomeProgramm") as Label;
                if (lblHomeProgramm != null) { lblHomeProgramm.Text = Math.Round(_HomeProgramAmt, 2).ToString(); }
                Label lblArtBasedTherapy = e.Row.FindControl("lblArtBasedTherapy") as Label;
                if (lblArtBasedTherapy != null) { lblArtBasedTherapy.Text = Math.Round(_ArtBasedTherapyEduAmt, 2).ToString(); }
                Label _cheqAmtLbl = e.Row.FindControl("lblCheque") as Label;
                if (_cheqAmtLbl != null) { _cheqAmtLbl.Text = Math.Round(_cheqAmt, 2).ToString(); }
                Label lblOnline = e.Row.FindControl("lblOnline") as Label;
                if (lblOnline != null) { lblOnline.Text = Math.Round(_onlineAmt, 2).ToString(); }
                Label _cashAmtLbl = e.Row.FindControl("lblCash") as Label;
                if (_cashAmtLbl != null) { _cashAmtLbl.Text = Math.Round(_cashAmt, 2).ToString(); }
                Label _PaediatricOrtholbl = e.Row.FindControl("lblPaediatricOrthopaedic") as Label;
                if (_PaediatricOrtholbl != null) { _PaediatricOrtholbl.Text = Math.Round(_paediatricOrthopaedicAmt, 2).ToString(); }
                Label _musivtherapylbl = e.Row.FindControl("lblmusictherapy") as Label;
                if (_musivtherapylbl != null) { _musivtherapylbl.Text = Math.Round(_musictherapy, 2).ToString(); }
                Label _speechHomeVisitLbl = e.Row.FindControl("lblspeechhomevisiteval") as Label;
                if (_speechHomeVisitLbl != null) { _speechHomeVisitLbl.Text = Math.Round(_speechhomevisit, 2).ToString(); }
            }
        }
    }

    //class GridViewTemplate : ITemplate
    //{
    //    //A variable to hold the type of ListItemType.
    //    ListItemType _templateType;
    //    //A variable to hold the data source.
    //    DataTable _dt;
    //    //A variable to hold the column name.
    //    string _columnName;
    //    bool isDoctor;
    //    //Constructor where we define the template type and column name.
    //    public GridViewTemplate(ListItemType type, string colname, DataTable dt, bool isDoctor)
    //    {
    //        //Stores the template type.
    //        _templateType = type;

    //        //Stores the column name.
    //        _columnName = colname;
    //        this.isDoctor = isDoctor;
    //        _dt = dt;
    //    }

    //    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    //    {
    //        switch (_templateType)
    //        {
    //            case ListItemType.Header:
    //                //Creates a new label control and add it to the container.
    //                Label lbl = new Label();            //Allocates the new label object.
    //                string _column = _columnName; if (_columnName.IndexOf(@"__________") > -1) { _column = _columnName.Substring(0, _columnName.IndexOf(@"__________")); }
    //                lbl.Text = _column;             //Assigns the name of the column in the lable.
    //                container.Controls.Add(lbl);        //Adds the newly created label control to the container.
    //                break;

    //            case ListItemType.Item:
    //                LiteralControl tb1 = new LiteralControl();
    //                tb1.DataBinding += new EventHandler(tb1_DataBinding);
    //                container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
    //                break;

    //            case ListItemType.EditItem:
    //                //As, I am not using any EditItem, I didnot added any code here.
    //                break;

    //            case ListItemType.Footer:
    //                Label lblFooter = new Label();
    //                lblFooter.DataBinding += new EventHandler(lblFooter_DataBinding);
    //                container.Controls.Add(lblFooter);
    //                break;
    //        }
    //    }

    //    void lblFooter_DataBinding(object sender, EventArgs e)
    //    {
    //        Label lbl = (Label)sender; decimal _totalAmt = 0;

    //        for (int i = 0; i < _dt.Rows.Count; i++)
    //        {
    //            decimal _totalAmtTmp = 0;
    //            decimal.TryParse(_dt.Rows[i][_columnName].ToString(), out _totalAmtTmp);
    //            _totalAmt += _totalAmtTmp;
    //        }
    //        lbl.Text = Math.Round(_totalAmt, 2).ToString(); lbl.Font.Bold = true;
    //    }

    //    void tb1_DataBinding(object sender, EventArgs e)
    //    {
    //        LiteralControl txtdata = (LiteralControl)sender;
    //        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
    //        object dataDate = DataBinder.Eval(container.DataItem, "PayDate");
    //        object dataValue = DataBinder.Eval(container.DataItem, _columnName);
    //        int _doctorID = 0; if (_columnName.LastIndexOf(@"__________") > -1) { int.TryParse(_columnName.Substring(_columnName.LastIndexOf(@"__________")).Replace(@"__________", ""), out _doctorID); }
    //        if (dataValue != DBNull.Value)
    //        {
    //            float amt = 0; float.TryParse(dataValue.ToString(), out amt);
    //            DateTime _test = new DateTime(); DateTime.TryParseExact(dataDate.ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
    //            if (amt > 0)
    //            {
    //                if (isDoctor)
    //                {
    //                    txtdata.Text = "<a href=\"javascript:;\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"DoctorAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + _doctorID.ToString() + ")\">" + amt.ToString() + "</a>";
    //                }
    //                else
    //                {
    //                    txtdata.Text = "<a href=\"javascript:;\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"OtherAccount('" + _test.ToString(DbHelper.Configuration.dateFormat) + "', " + _doctorID.ToString() + ")\">" + amt.ToString() + "</a>";
    //                }
    //            }
    //            else
    //            {
    //                txtdata.Text = "- - -";
    //            }
    //        }
    //    }
    //}
}