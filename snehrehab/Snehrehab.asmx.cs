using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace snehrehab
{
    /// <summary>
    /// Summary description for Snehrehab
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Snehrehab : System.Web.Services.WebService, IRequiresSessionState
    {
        [WebMethod(EnableSession = true)]
        public string MonthlyAccount(string d, string t)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(d, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            int _reportType = 0; int.TryParse(t, out _reportType);
            if (_reportType > 0 && _test > DateTime.MinValue)
            {
                StringBuilder html = new StringBuilder();
                SqlCommand cmd = new SqlCommand("Reports_MonthlyAccount_Single"); cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = _reportType;
                cmd.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = _test;
                DbHelper.SqlDb db = new DbHelper.SqlDb();
                DataTable dt = db.DbRead(cmd);
                if (dt.Rows.Count > 0)
                {
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    if (_reportType == 1)
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>CREDIT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 2)
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>DEBIT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["DebitAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 3)
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT NAME</th><th>PRODUCT</th><th>DESCRIPTION</th><th>DEBIT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["ProductName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["DebitAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 6)//CLINICAL
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 7) // SPEECH
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 8) // NOPD
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 9) // DIET
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 10) // MATRIX
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 11)
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 12)
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>CREDIT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 13)
                    { 
                        html.Append("<thead style=\"white-space: nowrap;\"><tr><th>TOTAL</th><th>EXP</th><th>OTHER CASH</th><th>C. P.</th><th>S. T.</th><th>NOPD</th><th>DIET</th><th>MATRIX</th><th>MVPT</th><th>V. T.</th><th>WALK AID</th><th>S. EDU</th><th>CHEQUE</th><th>CASH</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + dt.Rows[i]["TotalAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["ExpAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CashEntry"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["ClinicalAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["SpeechAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["NopdAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["DietAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["MatrixAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["MVPTAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["VisionTherapyAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["WalkAidAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["SpecialEdu"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CheqAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CashAmt"].ToString() + "</td></tr>");
                        }
                        html.Append("</tbody>");
                    }

                    if (_reportType == 19)
                    {
                        html.Append("<thead style=\"white-space: nowrap;\"><tr><th>TOTAL</th><th>Sp.H.V</th><th>HOME VISIT</th><th>OTHER CASH</th> <th>OTHER ACTIVITY</th><th>Sp.T.</th><th>ONLINE</th><th>CASH</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + dt.Rows[i]["TotalAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["SpeechHomeVisitEvaluation"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["HomeVisitEvaluation"].ToString() + "</td>");
                            // html.Append("<td>" + dt.Rows[i]["ExpAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CashEntry"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["OTHER_ACTIVITIES"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["SpeechAmt"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["OnlineAmt"].ToString() + "</td>");                            
                            html.Append("<td>" + dt.Rows[i]["CashAmt"].ToString() + "</td></tr>");
                        }
                        html.Append("</tbody>");
                    }

                    if (_reportType == 22) // Online
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>CREDIT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }


                    if (_reportType == 14) // VISION THERAPY
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 15) // WALK AID
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }
                    if (_reportType == 16) // SPECIAL EDUCATION
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }

                    if (_reportType == 17) // Other Activities 
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }


                    if (_reportType == 18) // Total 
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }


                    if (_reportType == 20) // Total 
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }

                    if (_reportType == 21) // HomeVisitEvaluation
                    {
                        decimal _amount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _amount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_amount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }

                    if (_reportType == 25) // HOMEVISITSPEECH
                    {
                        decimal _homevisitspeechamount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _homevisitspeechamount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_homevisitspeechamount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }

                    if (_reportType == 26) // AUDIOLOGY
                    {
                        decimal _audiologyamount = 0;
                        html.Append("<thead><tr><th>SR NO</th><th>PATIENT</th><th>DESCRIPTION</th><th>AMOUNT</th></tr></thead>");
                        html.Append("<tbody>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["FullName"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                            html.Append("<td>" + dt.Rows[i]["CreditAmt"].ToString() + "</td></tr>");
                            decimal _temp = 0; decimal.TryParse(dt.Rows[i]["CreditAmt"].ToString(), out _temp);
                            _audiologyamount += _temp;
                        }
                        html.Append("<tr><td colspan=\"3\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_audiologyamount, 2).ToString() + "</b></td></tr>");
                        html.Append("</tbody>");
                    }

                    html.Append("</table>");
                }

                

                else
                {
                    return DbHelper.Configuration.setAlert("Unable to find any record...", 3);
                }
                return html.ToString();
            }
            else
            {
                return DbHelper.Configuration.setAlert("Unable to process your request, please try again...", 2);
            }
        }

        [WebMethod(EnableSession = true)]
        public string MonthlyAccount_Doctor(string d, string id)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(d, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            int _doctorID = 0; int.TryParse(id, out _doctorID);
            if (_doctorID > 0 && _test > DateTime.MinValue)
            {
                StringBuilder html = new StringBuilder();
                SqlCommand cmd = new SqlCommand("Reports_MonthlyAccount_Doctors"); cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
                cmd.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = _test;
                DbHelper.SqlDb db = new DbHelper.SqlDb();
                DataTable dt = db.DbRead(cmd);
                if (dt.Rows.Count > 0)
                {
                    decimal _appAmt = 0; decimal _debAmt = 0; decimal _deb80 = 0; decimal _deb40 = 0;
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    html.Append("<thead><tr><th>SR NO</th><th>PATIENT NAME</th><th>LEDGER HEAD</th><th>DESCRIPTION</th><th>SESSION AMT</th><th>20 %</th><th>DEBIT</th></tr></thead>");
                    html.Append("<tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal _appAmtTmp = 0; decimal _debAmtTmp = 0; decimal _deb80Tmp = 0; decimal _deb40tmp = 0;
                        decimal.TryParse(dt.Rows[i]["AppointmentAmt"].ToString(), out _appAmtTmp);
                        decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _debAmtTmp);
                        _deb80Tmp = _appAmtTmp - (_appAmtTmp * 20 / 100);
                        _deb40tmp = _deb80Tmp * 40 / 100;
                        html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["PatientFullName"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["LedgerHead"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["cDescription"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["AppointmentAmt"].ToString() + "</td>");
                        html.Append("<td>" + Math.Round(decimal.Parse(_deb80Tmp.ToString()), 2).ToString() + "</td>");
                        //html.Append("<td>" + dt.Rows[i]["DebitAmt"].ToString() + "</td>");
                        html.Append("<td>" + Math.Round(decimal.Parse(_deb40tmp.ToString()), 2).ToString() + "</td>");
                        
                        html.Append("</tr>");
                        _deb40 += _deb40tmp;
                        _deb80 += _deb80Tmp;
                        _appAmt += _appAmtTmp;
                        //_debAmt += _debAmtTmp;
                    }
                   // html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_appAmt, 2).ToString() + "</b></td><td><b>" + Math.Round(_deb80, 2).ToString() + "</b></td><td><b>" + Math.Round(_debAmt, 2).ToString() + "</b></td></tr>");

                    html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_appAmt, 2).ToString() + "</b></td><td><b>" + Math.Round(_deb80, 2).ToString() + "</b></td><td><b>" + Math.Round(_deb40, 2).ToString() + "</b></td></tr>");

                    html.Append("</tbody>");

                    html.Append("</table>");
                }
                else
                {
                    return DbHelper.Configuration.setAlert("Unable to find any record...", 3);
                }
                return html.ToString();
            }
            else
            {
                return DbHelper.Configuration.setAlert("Unable to process your request, please try again...", 2);
            }
        }

        [WebMethod(EnableSession = true)]
        public string Monthly_OtherAccount(string d, string id)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(d, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            int _otheraccountid = 0; int.TryParse(id, out _otheraccountid);
            if (_otheraccountid > 0 && _test > DateTime.MinValue)
            {
                StringBuilder html = new StringBuilder();
                SqlCommand cmd = new SqlCommand("OtherAccount_MonthlyReport"); cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@otheraccountid", SqlDbType.Int).Value = _otheraccountid;
                cmd.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = _test;
                DbHelper.SqlDb db = new DbHelper.SqlDb();
                DataTable dt = db.DbRead(cmd);
                if (dt.Rows.Count > 0)
                {
                    decimal _appAmt = 0; decimal _debAmt = 0;
                    html.Append("<table class=\"table table-bordered\" cellspacing=\"0\" border=\"1\" style=\"border-collapse:collapse;\">");
                    html.Append("<thead><tr><th>SR NO</th><th>ACCOUNT NAME</th><th>DOCTOR</th><th>ASS.DOCTOR</th><th>DEBIT AMT</th></tr></thead>");
                    html.Append("<tbody>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal _debAmtTmp = 0;
                        html.Append("<tr><td>" + (i + 1).ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["AccountName"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["Doctor"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["Ass_Doctor"].ToString() + "</td>");
                        html.Append("<td>" + dt.Rows[i]["DebitAmt"].ToString() + "</td>");

                        decimal.TryParse(dt.Rows[i]["DebitAmt"].ToString(), out _debAmtTmp);

                        _debAmt += _debAmtTmp;
                    }
                    html.Append("<tr><td colspan=\"4\" style=\"text-align:right;\"><b>Total</b></td><td><b>" + Math.Round(_debAmt, 2).ToString() + "</b></td></tr>");

                    html.Append("</tbody>");

                    html.Append("</table>");
                }
                else
                {
                    return DbHelper.Configuration.setAlert("Unable to find any record...", 3);
                }
                return html.ToString();
            }
            else
            {
                return DbHelper.Configuration.setAlert("Unable to process your request, please try again...", 2);
            }
        }
        [WebMethod(EnableSession = true)]
        public List<object[]> ChangeAppointmentDoctor(string d)
        {
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID > 0)
            {
                List<object[]> l = new List<object[]>();
                int _appointmentID = 0; if (DbHelper.Configuration.IsGuid(d)) { _appointmentID = SnehBLL.Appointments_Bll.Check(d); }
                if (_appointmentID > 0)
                {
                    SqlCommand cmd = new SqlCommand("Reports_AppointmentChangeView"); cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
                    DbHelper.SqlDb db = new DbHelper.SqlDb();
                    DataSet ds = db.DbFetch(cmd);
                    if (ds.Tables.Count > 0)
                    {
                        object[] ob = new object[] { ds.Tables[0].Rows[0]["FullName"].ToString(), ds.Tables[0].Rows[0]["SessionName"].ToString() };
                        l.Add(ob);
                    }
                    if (ds.Tables.Count > 1)
                    {
                        object[] obj = new object[ds.Tables[1].Rows.Count];
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            object[] ob = new object[] { ds.Tables[1].Rows[i]["DoctorID"].ToString(), ds.Tables[1].Rows[i]["FullName"].ToString() };
                            obj[i] = ob;
                        }
                        l.Add(obj);
                    }
                    return l;
                }
            }
            return null;
        }

        [WebMethod(EnableSession = true)]
        public int ChangeAppointmentRequest(string d, int n, string r)
        {
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID > 0)
            {
                int _appointmentID = 0; if (DbHelper.Configuration.IsGuid(d)) { _appointmentID = SnehBLL.Appointments_Bll.Check(d); }
                if (_appointmentID > 0)
                {
                    SnehBLL.AppointmentChangeRequest_Bll ACB = new SnehBLL.AppointmentChangeRequest_Bll();
                    SnehDLL.AppointmentChangeRequest_Dll ACD = new SnehDLL.AppointmentChangeRequest_Dll();
                    ACD.AppointmentID = _appointmentID;
                    ACD.ReqDoctorID = ACB.getDoctorID(_loginID);
                    ACD.AssignToDoctorID = n;
                    ACD.Remarks = r;
                    ACD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
                    ACD.ModifyBy = _loginID;

                    return ACB.New(ACD);
                }
            }
            return -1;
        }

        [WebMethod(EnableSession = true)]
        public string load_speedbar_notifications()
        {
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID > 0)
            {
                int _catID = SnehBLL.UserAccount_Bll.getCategory();
                if (_catID != 3)
                {
                    string _date = "";
                    int _count = SnehBLL.AppointmentChangeRequest_Bll.pending_count(out _date);
                    if (_count > 0)
                    {
                        if (_date.Length > 0)
                            return "<a href='/Member/AppChngeRequest.aspx?fdate=" + _date + "'>" + _count.ToString() + " - doctor request pending</a>";
                        else
                            return "<a href='/Member/AppChngeRequest.aspx'>" + _count.ToString() + " - doctor request pending</a>";
                    }
                }
            }
            return null;
        }

        public string TIMEDURATION(string _durationStr, string _timeStr)
        {
            int _duration = 0; int.TryParse(_durationStr, out _duration);
            DateTime TimeHourD = new DateTime();
            DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
            {
                return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "- - -";
        }

        [WebMethod(EnableSession = true)]
        public string BulkPackageDetail(string id)
        {
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID > 0)
            {
                long BulkID = 0; if (DbHelper.Configuration.IsGuid(id)) { BulkID = SnehBLL.PatientBulk_Bll.Check(id); }
                if (BulkID > 0)
                {
                    SnehBLL.PatientBulk_Bll PB = new SnehBLL.PatientBulk_Bll();
                    DataSet ds = PB.Data(BulkID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int PatientID = 0; int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
                        float Amount = 0; float.TryParse(ds.Tables[0].Rows[0]["Amount"].ToString(), out Amount);
                        int ModePayment = 0; int.TryParse(ds.Tables[0].Rows[0]["ModePayment"].ToString(), out ModePayment); string strModePayment = string.Empty;
                        if (ModePayment == 1) { strModePayment = "Cash"; } else if (ModePayment == 2) { strModePayment = "Credit"; } else if (ModePayment == 3) { strModePayment = "Cheque"; } else if (ModePayment == 4) { strModePayment = "Online"; }

                        StringBuilder html = new StringBuilder();
                        html.Append("<table class=\"table table-nobordered\" cellspacing=\"0\">");
                        html.Append("<tr><td><b>Booking Amount :</b></td><td>" + Amount.ToString() + "</td><td></td><td></td></tr>");
                        html.Append("<tr><td><b>Booking Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["AddedDate"].ToString()) + "</td><td><b>Payment Mode :</b></td><td>" + (!string.IsNullOrEmpty(strModePayment) ? strModePayment : "Unknown") + "</td></tr>");
                        if (ModePayment == 2)
                        {
                            /*SqlCommand cmd = new SqlCommand("SELECT PL.PayMode, CONVERT(VARCHAR(50), PL.PayDate, 20) AS PayDate," +
                                " B.BankName, PL.BankBranch, PL.ChequeTxnNo, CONVERT(VARCHAR(50), PL.ChequeDate, 20) AS ChequeDate " +
                                " FROM PatientLedger PL LEFT OUTER JOIN Banks B ON PL.BankID = B.BankID WHERE PL.PatientID = @PatientID AND PL.LedgerHeadID = @LedgerHeadID AND COALESCE(PL.DebitAmt, 0) > 0");
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                            cmd.Parameters.Add("@LedgerHeadID", SqlDbType.Int).Value = BookingID;
                            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
                            if (dt.Rows.Count > 0)
                            {
                                html.Append("<tr><td colspan=\"4\"><h5>Payment Detail :</h5></td></tr>");
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int PayMode = 0; int.TryParse(dt.Rows[i]["PayMode"].ToString(), out PayMode);
                                string PayModeStr = string.Empty; if (PayMode == 1) { PayModeStr = "Cash"; } else if (PayMode == 2) { PayModeStr = "Credit"; } else if (PayMode == 3) { PayModeStr = "Cheque"; } else if (PayMode == 4) { PayModeStr = "Online"; }
                                html.Append("<tr><td><b>Payment Mode :</b></td><td>" + (!string.IsNullOrEmpty(PayModeStr) ? PayModeStr : "Unknown") + "</td><td><b>Payment Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(dt.Rows[i]["PayDate"].ToString()) + "</td></tr>");
                                if (PayMode == 3)
                                {
                                    html.Append("<tr><td><b>Bank Name :</b></td><td>" + dt.Rows[i]["BankName"].ToString() + "</td><td><b>Cheque Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(dt.Rows[i]["ChequeDate"].ToString()) + "</td></tr>");
                                }
                                if (PayMode == 4)
                                {
                                    html.Append("<tr><td><b>Transaction ID :</b></td><td>" + dt.Rows[i]["ChequeTxnNo"].ToString() + "</td><td><b>Transaction Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(dt.Rows[i]["ChequeDate"].ToString()) + "</td></tr>");
                                }
                            }*/
                        }
                        if (ModePayment == 3)
                        {
                            html.Append("<tr><td><b>Bank Name :</b></td><td>" + ds.Tables[0].Rows[0]["BankName"].ToString() + "</td><td><b>Cheque Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["ChequeDate"].ToString()) + "</td></tr>");
                        }
                        if (ModePayment == 4)
                        {
                            html.Append("<tr><td><b>Transaction ID :</b></td><td>" + ds.Tables[0].Rows[0]["ChequeTxnNo"].ToString() + "</td><td><b>Transaction Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["ChequeDate"].ToString()) + "</td></tr>");
                            html.Append("<tr><td><b>Receipt Date :</b></td><td>" + DbHelper.Configuration.FORMATDATE(ds.Tables[0].Rows[0]["PaidDate"].ToString()) + "</td></tr>");
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Narration"].ToString()))
                        {
                            html.Append("<tr><td><b>Narration :</b></td><td colspan=\"3\">" + ds.Tables[0].Rows[0]["Narration"].ToString() + "</td></tr>");
                        }
                        html.Append("</table>");
                        return html.ToString();
                    }
                }
            }
            return null;
        }
    }
}