using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

namespace snehrehab.Reports
{
    public partial class PreConsultation : System.Web.UI.Page
    {
        int _loginID = 0; bool isSuperAdmin = false;    
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 4 || SnehBLL.UserAccount_Bll.getCategory() == 5)
            {
                isSuperAdmin = true;
            }
            if (!IsPostBack)
            {
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
                          1//DateTime.UtcNow.AddMinutes(330).Day
                           ).ToString(DbHelper.Configuration.showDateFormat);

                SearchData();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportGV.PageIndex = 0; SearchData();
        }

        protected void ReportGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportGV.PageIndex = e.NewPageIndex; SearchData();
        }
        private void SearchData()
        {
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

            ReportGV.DataSource = RDB.Search_New(_loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
            ReportGV.DataBind();
            if (ReportGV.HeaderRow != null) { ReportGV.HeaderRow.TableSection = TableRowSection.TableHeader; }

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

        public string SessionRptLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/CreateRpt.ashx?type=8&record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a></br>";
            }
            return "Pending</br>";
        }
        public string GetAction(string UniqueID)
        {
            if (isSuperAdmin)
                return string.Empty;
            else if (SnehBLL.UserAccount_Bll.getCategory() == 2)
                return string.Empty;
            else if (SnehBLL.UserAccount_Bll.getCategory() == 6)
                return string.Empty;
            else
                return "<a href=\"/SessionRpt/PreConsultRpt.aspx?record=" + UniqueID + "\">View/Edit</a> &nbsp;";
        }

        public string GetMailink(string UniqueID)
        {
            if (!string.IsNullOrEmpty(UniqueID))
            {
                return "<a href=\"/Member/SendMail.aspx?pretype=presc&record=" + UniqueID + "\" target=\"_blank\">Send Mail</a>";
            }
            return "";
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            DataTable dt = RDB.Search_New(_loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty; DbHelper.SqlDb db = new DbHelper.SqlDb();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 BranchName FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtPP = db.DbRead(cmd); if (dtPP.Rows.Count > 0) { centrename = dtPP.Rows[0]["BranchName"].ToString(); }
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>" + GetCentreName(centrename) + "</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>SESSION</th><th>THERAPIST</th><th>DATE</th><th>DURATION</th><th>TIME</th><th>STATUS</th><th>MAIL SEND	</th></tr>");
                string ReceiptPrefix = string.Empty;
                cmd = new SqlCommand("SELECT TOP 1 ReceiptPrefix FROM SettingsMst"); cmd.CommandType = CommandType.Text;
                DataTable dtP = db.DbRead(cmd); if (dtP.Rows.Count > 0) { ReceiptPrefix = dtP.Rows[0]["ReceiptPrefix"].ToString(); }
                int totalr = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SessionName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Therapist"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["AppointmentDate"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Duration"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + TIMEDURATION(dt.Rows[i]["Duration"].ToString(), dt.Rows[i]["AppointmentTime"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["IsFilled"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MailSend"].ToString() + "</td>");
                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + GetCentreName(centrename) + " Pre Screening Report.xls");
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

        protected void btnResearch_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            DataTable dt = RDB.Search_New_Report(_loginID, txtSearch.Text.Trim(), _fromDate, _uptoDate, true);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder(); string centrename = string.Empty; DbHelper.SqlDb db = new DbHelper.SqlDb();

                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;text-align:left;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>Age</th><th>Gender</th><th>Expected Date of delivery</th><th>which school does your child study in?</th><th>Which grade?</th><th>Mothers age</th><th>Fathers age </th><th>Diagnosis </th><th>Chief concerns at home</th>" +
                    "<th>chief concers at school</th><th>Chief concerns at social gatherings </th><th>Consanguinity</th><th>Years of marriage</th><th>Family Structure</th><th>Conception </th><th>Planning of conception </th><th>Sibling history </th><th>interparental relationship</th><th>Parent child relationship</th>" +
                    "<th>Family relocation</th><th>screen time of the child </th>" +
                    "<th>Screen time of the mother</th><th>Prenatal conditions </th><th>maternal stress </th><th>Drescribe stressoors in short </th><th>weight gain during pregnancy </th>" +
                    "<th>Fetal movements</th><th>Prenatal wellness program attended?</th><th>Type of delivery</th><th>CIAB?</th><th>Birth Weight</th><th>Gestational Birth Age</th><th>NICU stay</th><th>NICU History </th>" +
                    "<th>Reason For NICU stay</th><th>APGAR score</th><th>Breast fed</th><th>If not, how was the baby fed</th><th>Problems during breast feeding</th><th>Mention problems</th><th>Any colic issues as a baby?</th><th>Other medical issues</th><th>Gross Motor</th>" +
                    "<th>Fine motor  </th><th>Personal Social  </th><th>Communication</th><th>Sleep issues during 0-6 months </th><th>Present sleep concerns</th><th>Sleep type </th><th>cosleeping with </th><th>Feeding habits </th><th>What are your childs likes and dislikes</th>" +
                    "<th>Play behaviour</th><th>Interaction with peers</th><th>Stranger anxiety </th><th>does your child play with toys ?</th><th>Brushing</th><th>bathing</th><th>toiletting</th><th>dressing </th><th>eating </th><th>ambulation</th><th>transfers </th></tr>");
                string ReceiptPrefix = string.Empty;

                int totalr = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Age"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Gender"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + FORMATDATE(dt.Rows[i]["DateDelivery"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["OnlineOffline"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["WhichGrade"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MotherAge"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FatherAge"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Diagnosis"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ChiefConcernsHome"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ChiefConcernsSchool"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ChiefConcernsSocialGath"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Consanguinity"].ToString() != "" ? dt.Rows[i]["Consanguinity"].ToString() : dt.Rows[i]["Consanguinity_1"].ToString()) + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["YearsMarriage"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["FamilyStructure"].ToString()!=""?dt.Rows[i]["FamilyStructure"].ToString():dt.Rows[i]["FamilyStructure_1"].ToString()) + "</td>");
                    string resultConception = dt.Rows[i]["Conception"].ToString() != "" ?
                dt.Rows[i]["Conception"].ToString() :
                (dt.Rows[i]["Conception_1"].ToString() != "" ?
                dt.Rows[i]["Conception_1"].ToString() :
                (dt.Rows[i]["Conception_2"].ToString() != "" ?
                dt.Rows[i]["Conception_2"].ToString() :
                (dt.Rows[i]["Conception_3"].ToString() !=""?
                dt.Rows[i]["Conception_3"].ToString():
                dt.Rows[i]["Conception_4"].ToString())));


                    html.Append("<td style=\"vertical-align:top;\">" + resultConception + "</td>");
                  
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["PlanningConception"].ToString() != "" ? dt.Rows[i]["PlanningConception"].ToString() : dt.Rows[i]["PlanningConception_1"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Siblings"].ToString()== "Yes"? dt.Rows[i]["NoOfSiblings"].ToString()+", "+dt.Rows[i]["RHASiblings"].ToString(): dt.Rows[i]["Siblings"].ToString()) + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" +(dt.Rows[i]["InterParentalRelation"].ToString() != "" ? dt.Rows[i]["InterParentalRelation"].ToString() : (dt.Rows[i]["InterParentalRelation_1"].ToString()!=""?dt.Rows[i]["InterParentalRelation_1"].ToString(): dt.Rows[i]["InterParentalRelation_2"].ToString())) + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["ParentChildRelation"].ToString() != "" ? dt.Rows[i]["ParentChildRelation"].ToString() : (dt.Rows[i]["ParentChildRelation_1"].ToString() != "" ? dt.Rows[i]["ParentChildRelation_1"].ToString() : dt.Rows[i]["ParentChildRelation_2"].ToString())) + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["FamilyRelocation"].ToString() != "" ? dt.Rows[i]["FamilyRelocation"].ToString() : dt.Rows[i]["FamilyRelocation_1"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ScreenTimeChild"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MotherScreenTime"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PrenatalCondition"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["CheckMental"].ToString()!=""?dt.Rows[i]["CheckMental"].ToString():dt.Rows[i]["MaternalStress_1"].ToString()) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["DescribeStressors"].ToString() + "</td>");
                   
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["WGDP"].ToString() + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FoetalMovement"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Prenatalwellness"].ToString() + "</td>");

                    string delivery = dt.Rows[i]["delivery"].ToString() != "" ?
                dt.Rows[i]["delivery"].ToString() :
                (dt.Rows[i]["delivery_1"].ToString() != "" ?
                dt.Rows[i]["delivery_1"].ToString() :
                (dt.Rows[i]["delivery_2"].ToString() != "" ?
                dt.Rows[i]["delivery_2"].ToString() :
                dt.Rows[i]["delivery_3"].ToString()));
               
                    html.Append("<td style=\"vertical-align:top;\">" + delivery + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ciab"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["BirthWeight"].ToString() + "</td>");

                    string GestationalBirthAge = dt.Rows[i]["GestationalBirthAge"].ToString() != "" ?
                dt.Rows[i]["GestationalBirthAge"].ToString() :
                (dt.Rows[i]["GestationalBirthAge_1"].ToString() != "" ?
                dt.Rows[i]["GestationalBirthAge_1"].ToString() :
                dt.Rows[i]["GestationalBirthAge_2"].ToString());
                    html.Append("<td style=\"vertical-align:top;\">" + GestationalBirthAge + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["NICUstay"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["NICUHistory"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["ReasonNICUstay"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["APGARscore"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Breastfed"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["BabyFed"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Problemsduringbreastfeeding"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MentionProblem"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["colicissue"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["OthrtMedicalIssues"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["GrossMotor"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FineMotor"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PersonalandSocial"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Communication"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Sleepissues"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Presentsleep"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["SleepType"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Cosleeping"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Feedinghabits"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Childlikediskike"].ToString() + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Playbehaviour"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Interactionwithpeers"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Strangeranxiety"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["PlayToys"].ToString() + "</td>");

                    html.Append("<td style=\"vertical-align:top;\">" +( dt.Rows[i]["Brushing"].ToString() != "" ? dt.Rows[i]["Brushing"].ToString() : (dt.Rows[i]["Brushing_1"].ToString() != "" ? dt.Rows[i]["Brushing_1"].ToString() : dt.Rows[i]["Brushing_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Bathing"].ToString() != "" ? dt.Rows[i]["Bathing"].ToString() : (dt.Rows[i]["Bathing_1"].ToString() != "" ? dt.Rows[i]["Bathing_1"].ToString() : dt.Rows[i]["Bathing_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" +( dt.Rows[i]["Toileting"].ToString() != "" ? dt.Rows[i]["Toileting"].ToString() : (dt.Rows[i]["Toileting"].ToString() != "" ? dt.Rows[i]["Toileting_1"].ToString() : dt.Rows[i]["Toileting_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Dressing"].ToString() != "" ? dt.Rows[i]["Dressing"].ToString() : (dt.Rows[i]["Dressing_1"].ToString() != "" ? dt.Rows[i]["Dressing_1"].ToString() : dt.Rows[i]["Dressing_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Eating"].ToString() != "" ? dt.Rows[i]["Eating"].ToString() : (dt.Rows[i]["Eating_1"].ToString() != "" ? dt.Rows[i]["Eating_1"].ToString() : dt.Rows[i]["Eating_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Ambulation"].ToString() != "" ? dt.Rows[i]["Ambulation"].ToString() : (dt.Rows[i]["Ambulation_1"].ToString() != "" ? dt.Rows[i]["Ambulation_1"].ToString() : dt.Rows[i]["Ambulation_2"].ToString())) + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + (dt.Rows[i]["Transfers"].ToString() != "" ? dt.Rows[i]["Transfers"].ToString() : (dt.Rows[i]["Transfers_1"].ToString() != "" ? dt.Rows[i]["Transfers_1"].ToString() : dt.Rows[i]["Transfers_2"].ToString())) + "</td>");


                    html.Append("</tr>");
                }
                html.Append("<tr>");
                html.Append("<td style=\"vertical-align:top;\"><b>Total Records: " + totalr + " </b></td>");
                html.Append("</tr>");
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + GetCentreName(centrename) + " Pre Screening Report.xls");
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
        public string GetCentreName(string centre)
        {
            if (!string.IsNullOrEmpty(centre))
            {
                return centre + " " + " Pre Consultant  Report";
            }
            else
            {
                return " Pre Consultant  Report";
            }
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }
    }
}