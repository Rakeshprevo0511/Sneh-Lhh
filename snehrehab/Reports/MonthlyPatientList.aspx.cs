using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Globalization;

namespace snehrehab.Reports
{
    public partial class MonthlyPatientList : System.Web.UI.Page
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
            DataTable dtDR = DB.MonthlyPatientList1(_fromDate, _uptoDate);
            if (dtDR.Rows.Count > 0)
            {
                for (int k = 0; k < dtDR.Rows.Count; k++)
                {
                    dt.Columns.Add(dtDR.Rows[k]["FullName"].ToString(), typeof(string));

                }
                string _column1 = "";
                for (int i = 0; i < dtDR.Rows.Count; i++)
                {
                    int drid = int.Parse(dtDR.Rows[i]["DoctorID"].ToString());
                    _column1 = dtDR.Rows[i]["FullName"].ToString();
                    DataTable dtsinglepatient = null;
                    dtsinglepatient = DB.MonthlyPatientFinalLIST(_fromDate, _uptoDate, drid);
                    DataRow row;
                   
                    for (int j = 0; j < dtsinglepatient.Rows.Count; j++)
                    {
                        //row = dt.NewRow();
                        //row[i] = dtsinglepatient.Rows[j]["FullName"].ToString();
                        //dt.Rows.Add(row);
                       // dt.Rows[j][_col] = ds.Tables[1].Rows[j][i].ToString();
                        if (dt.Rows.Count>dtsinglepatient.Rows.Count)
                        {
                            dt.Rows[j][i] = dtsinglepatient.Rows[j]["FullName"].ToString();
                        }
                        else
                        {
                            int _addnewrows = dtsinglepatient.Rows.Count - dt.Rows.Count;
                            for (int k = 0; k < _addnewrows; k++)
                            {
                                row = dt.NewRow();
                                dt.Rows.Add(row);
                            }
                            dt.Rows[j][i] = dtsinglepatient.Rows[j]["FullName"].ToString();
                           
                        }

                  

                    }
                   
                }
                ReportGV.DataSource = dt;
                ReportGV.DataBind();

                //TemplateField ckhColumn = new TemplateField();
                //ckhColumn.HeaderTemplate = new GridViewTemplate(ListItemType.Header, _column1, null);
                //ckhColumn.ItemTemplate = new GridViewTemplate(ListItemType.Item, _column1, null);
                //ckhColumn.FooterTemplate = new GridViewTemplate(ListItemType.Footer, _column1, dt);
                //ReportGV.Columns.Add(ckhColumn);
                //if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            }
            
        }
   
        protected void btnExport_Click(object sender, EventArgs e)
        {
             DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
            DataTable dtDR = DB.MonthlyPatientList1(_fromDate, _uptoDate);
            if (dtDR.Rows.Count > 0)
            {
                for (int k = 0; k < dtDR.Rows.Count; k++)
                {
                    dt.Columns.Add(dtDR.Rows[k]["FullName"].ToString(), typeof(string));

                }
                string _column1 = "";
                for (int i = 0; i < dtDR.Rows.Count; i++)
                {
                    int drid = int.Parse(dtDR.Rows[i]["DoctorID"].ToString());
                    _column1 = dtDR.Rows[i]["FullName"].ToString();
                    DataTable dtsinglepatient = null;
                    dtsinglepatient = DB.MonthlyPatientFinalLIST(_fromDate, _uptoDate, drid);
                    DataRow row;

                    for (int j = 0; j < dtsinglepatient.Rows.Count; j++)
                    {

                        if (dt.Rows.Count > dtsinglepatient.Rows.Count)
                        {
                            dt.Rows[j][i] = dtsinglepatient.Rows[j]["FullName"].ToString();
                        }
                        else
                        {
                            int _addnewrows = dtsinglepatient.Rows.Count - dt.Rows.Count;
                            for (int k = 0; k < _addnewrows; k++)
                            {
                                row = dt.NewRow();
                                dt.Rows.Add(row);
                            }
                            dt.Rows[j][i] = dtsinglepatient.Rows[j]["FullName"].ToString();

                        }



                    }

                }
                ReportGV.DataSource = dt;
                ReportGV.DataBind();
            }
            string csv = "";
            csv += "\r\n";
            for (int i = 0; i < dt.Columns.Count; i++)
            { 
                csv += dt.Columns[i].ColumnName.ToString() + ',';
            }
            csv += "\r\n";
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    csv += dt.Rows[j][dt.Columns[k].ColumnName.ToString()].ToString().Replace(",", ";") + ',';
                }
                csv += "\r\n";
            }
            

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=Monthly Patient List-" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".csv");
            Response.ContentType = "application/octet-stream";
            Response.Cache.SetCacheability(HttpCacheability.NoCache); 
            Response.Charset = "";
            Response.Output.Write(csv);
            Response.End();
           

            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
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
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    decimal _totalAmt = 0, _expAmt = 0, _otherCash = 0, _clinicalAmt = 0, _speechAmt = 0, _nopdAmt = 0, _dietAmt = 0, _matrixAmt = 0, _visionAmt = 0, _walkAidAmt = 0, _specialEduAmt = 0, _cheqAmt = 0, _cashAmt = 0;
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        decimal _totalAmtTmp = 0, _expAmtTmp = 0, _otherCashTmp = 0, _clinicalAmtTmp = 0, _speechAmtTmp = 0, _nopdAmtTmp = 0, _dietAmtTmp = 0, _matrixAmtTmp = 0, _visionAmtTmp = 0, _walkAidAmtTmp = 0, _specialEduAmtTmp = 0, _cheqAmtTmp = 0, _cashAmtTmp = 0;

            //        decimal.TryParse(dt.Rows[i]["TotalAmt"].ToString(), out _totalAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["ExpAmt"].ToString(), out _expAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["CashEntry"].ToString(), out _otherCashTmp);
            //        decimal.TryParse(dt.Rows[i]["ClinicalAmt"].ToString(), out _clinicalAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["SpeechAmt"].ToString(), out _speechAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["NopdAmt"].ToString(), out _nopdAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["DietAmt"].ToString(), out _dietAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["MatrixAmt"].ToString(), out _matrixAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["CheqAmt"].ToString(), out _cheqAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["CashAmt"].ToString(), out _cashAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["VisionTherapyAmt"].ToString(), out _visionAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["WalkAidAmt"].ToString(), out _walkAidAmtTmp);
            //        decimal.TryParse(dt.Rows[i]["SpecialEdu"].ToString(), out _specialEduAmtTmp);

            //        _totalAmt += _totalAmtTmp;
            //        _expAmt += _expAmtTmp;
            //        _otherCash += _otherCashTmp;
            //        _clinicalAmt += _clinicalAmtTmp;
            //        _speechAmt += _speechAmtTmp;
            //        _nopdAmt += _nopdAmtTmp;
            //        _dietAmt += _dietAmtTmp;
            //        _matrixAmt += _matrixAmtTmp;
            //        _cheqAmt += _cheqAmtTmp;
            //        _cashAmt += _cashAmtTmp;
            //        _visionAmt += _visionAmtTmp;
            //        _walkAidAmt += _walkAidAmtTmp;
            //        _specialEduAmt += _specialEduAmtTmp;
            //    }
            //    Label _totalAmtLbl = e.Row.FindControl("lblTotal") as Label;
            //    if (_totalAmtLbl != null) { _totalAmtLbl.Text = Math.Round(_totalAmt, 2).ToString(); }
            //    Label _expAmtLbl = e.Row.FindControl("lblExp") as Label;
            //    if (_expAmtLbl != null) { _expAmtLbl.Text = Math.Round(_expAmt, 2).ToString(); }
            //    Label _otherCashLbl = e.Row.FindControl("lblOtherCash") as Label;
            //    if (_otherCashLbl != null) { _otherCashLbl.Text = Math.Round(_otherCash, 2).ToString(); }
            //    Label _clinicalAmtLbl = e.Row.FindControl("lblCp") as Label;
            //    if (_clinicalAmtLbl != null) { _clinicalAmtLbl.Text = Math.Round(_clinicalAmt, 2).ToString(); }
            //    Label _speechAmtLbl = e.Row.FindControl("lblSt") as Label;
            //    if (_speechAmtLbl != null) { _speechAmtLbl.Text = Math.Round(_speechAmt, 2).ToString(); }
            //    Label _nopdAmtLbl = e.Row.FindControl("lblNopd") as Label;
            //    if (_nopdAmtLbl != null) { _nopdAmtLbl.Text = Math.Round(_nopdAmt, 2).ToString(); }
            //    Label _dietAmtLbl = e.Row.FindControl("lblDiet") as Label;
            //    if (_dietAmtLbl != null) { _dietAmtLbl.Text = Math.Round(_dietAmt, 2).ToString(); }
            //    Label _matrixAmtLbl = e.Row.FindControl("lblMatrix") as Label;
            //    if (_matrixAmtLbl != null) { _matrixAmtLbl.Text = Math.Round(_matrixAmt, 2).ToString(); }
            //    Label _visionAmtLbl = e.Row.FindControl("lblVision") as Label;
            //    if (_visionAmtLbl != null) { _visionAmtLbl.Text = Math.Round(_visionAmt, 2).ToString(); }
            //    Label _walkAidAmtLbl = e.Row.FindControl("lblWalkAid") as Label;
            //    if (_walkAidAmtLbl != null) { _walkAidAmtLbl.Text = Math.Round(_walkAidAmt, 2).ToString(); }
            //    Label _specialEduAmtLbl = e.Row.FindControl("lblSpecialEdu") as Label;
            //    if (_specialEduAmtLbl != null) { _specialEduAmtLbl.Text = Math.Round(_specialEduAmt, 2).ToString(); }
            //    Label _cheqAmtLbl = e.Row.FindControl("lblCheque") as Label;
            //    if (_cheqAmtLbl != null) { _cheqAmtLbl.Text = Math.Round(_cheqAmt, 2).ToString(); }
            //    Label _cashAmtLbl = e.Row.FindControl("lblCash") as Label;
            //    if (_cashAmtLbl != null) { _cashAmtLbl.Text = Math.Round(_cashAmt, 2).ToString(); }
            //}
        }
    }
   
}
