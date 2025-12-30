using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;
using static snehrehab.rModel;
using System.Linq;

namespace snehrehab.SessionRpt
{
    public partial class SIRpt2022 : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        int PatientID = 0; int tabvalue = 0; public int OptionCount = 100;
        SqlConnection conn = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            DbHelper.SqlDb objSqlDb = new DbHelper.SqlDb();
            conn = objSqlDb.DbConnection();

            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            _cancelUrl = "/SessionRpt/SIview2022.aspx";
            if (_appointmentID > 0)
            {
                if (!IsPostBack)
                {
                    LoadForm();
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            }
            _printUrl = txtPrint.Value;
        }

        private void LoadForm()
        {

            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            List<optionMdel> ql = new List<optionMdel>();
            for (int i = 1; i <= OptionCount; i++) { ql.Add(new optionMdel() { }); }
            txtSignleChoice.DataSource = ql;
            txtSignleChoice.DataBind();

            if (!SID.IsValid(_appointmentID))
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            }
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }
            DataSet ds = SID.Getsi2022(_appointmentID);
            //SetData();
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();

            Doctor_Physioptherapist.Items.Clear(); Doctor_Physioptherapist.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_Occupational.Items.Clear(); Doctor_Occupational.Items.Add(new ListItem("Select Doctor", "-1"));
            //Doctor_EnterReport.Items.Clear(); Doctor_EnterReport.Items.Add(new ListItem("Select Doctor", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                Doctor_Physioptherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_Occupational.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                //Doctor_EnterReport.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }



            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //SqlConnection conn = new SqlConnection(conn);





            string que = "SELECT 'Select Month' as Month, '0' as MONTHS UNION ALL SELECT distinct cast(MONTHS as varchar) + ' Month' as Month, MONTHS FROM QUESTIONNAIRE_SI order by MONTHS";
            SqlCommand cmd = new SqlCommand(que, conn);
            conn.Open();
            DataSet dss = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dss);
            conn.Close();


            SelectMonth.DataSource = dss.Tables[0];
            SelectMonth.DataTextField = "Month";
            SelectMonth.DataValueField = "MONTHS";
            SelectMonth.DataBind();

            //SqlConnection conn1 = new SqlConnection("Data Source=184.154.187.166;Initial Catalog=demo2;User ID=demo2;Password=Sneh#123db");
            //string que1 = "SELECT  DISTINCT MonthsQ, Month_Caption, Sort_Order FROM ABILITY_CHECKLIST_New ORDER BY Sort_Order";
            string que1 = "SELECT 'Select Month' as Month_Caption, 0 as MonthsQ, 0 as Sort_Order UNION ALL SELECT DISTINCT cast(Month_Caption as varchar), MonthsQ, Sort_Order FROM ABILITY_CHECKLIST_New ORDER BY Sort_Order";
            SqlCommand cmd1 = new SqlCommand(que1, conn);
            conn.Open();
            DataSet dss1 = new DataSet();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dss1);
            conn.Close();

            //MonthSelect.Items.Clear();
            //MonthSelect.Items.Add("select");
            //MonthSelect.Items[0].Value = "0";
            //MonthSelect.SelectedIndex = 0;
            MonthSelect.DataSource = dss1.Tables[0];
            MonthSelect.DataTextField = "Month_Caption";
            MonthSelect.DataValueField = "MonthsQ";
            MonthSelect.DataBind();



            Session["rptData"] = ds;
            if (ds.Tables.Count > 0)
            {

                List<ListItem> selected = new List<ListItem>();
                bool HasDiagnosisID = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPatient.Text = ds.Tables[0].Rows[0]["FullName"].ToString();
                    txtSession.Text = ds.Tables[0].Rows[0]["SessionName"].ToString();
                    int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
                    bool.TryParse(ds.Tables[0].Rows[0]["HasDiagnosisID"].ToString(), out HasDiagnosisID);

                    string[] DiagnosisIDs = ds.Tables[0].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                    {
                        for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                        {
                            if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                            {
                                txtDiagnosis.Items[j].Selected = true; break;
                            }
                        }
                    }
                    txtDiagnosisOther.Text = ds.Tables[0].Rows[0]["DiagnosisOther"].ToString();
                }
                if (HasDiagnosisID) { PanelDiagnosis.Visible = true; } else { PanelDiagnosis.Visible = false; }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (HasDiagnosisID)
                    {
                        PanelDiagnosis.Visible = true;
                        string[] DiagnosisIDs = ds.Tables[1].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                        {
                            for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                            {
                                if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                                {
                                    txtDiagnosis.Items[j].Selected = true; break;
                                }
                            }
                        }
                        txtDiagnosisOther.Text = ds.Tables[1].Rows[0]["DiagnosisOther"].ToString();
                    }
                    else
                    {
                        PanelDiagnosis.Visible = false;
                    }
                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=si_rpt2022&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
                    //DailySchedule_Dailyaroutine_1.Checked = false; DailySchedule_Dailyaroutine_2.Checked = false; DailySchedule_Dailyaroutine_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["DailySchedule_Dailyaroutine"].ToString().Equals(DailySchedule_Dailyaroutine_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    DailySchedule_Dailyaroutine_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["DailySchedule_Dailyaroutine"].ToString().Equals(DailySchedule_Dailyaroutine_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    DailySchedule_Dailyaroutine_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["DailySchedule_Dailyaroutine"].ToString().Equals(DailySchedule_Dailyaroutine_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    DailySchedule_Dailyaroutine_3.Checked = true;
                    //}

                    //hfdTime.Value = ds.Tables[1].Rows[0]["DailySchedule_WakeUpTime"].ToString();
                    ////hfdTime.Value = "DailySchedule_WakeUpTime";
                    //hfdTimerest.Value = ds.Tables[1].Rows[0]["DailySchedule_RestRoomTime"].ToString();
                    //DailySchedule_Breakfast.Text = ds.Tables[1].Rows[0]["DailySchedule_Breakfast"].ToString();
                    //hfdbreaktime.Value = ds.Tables[1].Rows[0]["DailySchedule_BreakFastTime"].ToString();


                    ////foreach (ListItem item in DailySchedule_BreakFastContent.Items)
                    ////    if (item.Selected) selected.Add(item);

                    //hfdschooltime.Value = ds.Tables[1].Rows[0]["DailySchedule_SchoolTime"].ToString();
                    //DailySchedule_MidMorinig.Text = ds.Tables[1].Rows[0]["DailySchedule_MidMorinig"].ToString();
                    //DailySchedule_SchoolHours.Text = ds.Tables[1].Rows[0]["DailySchedule_SchoolHours"].ToString();
                    //hfdLunchTime.Value = ds.Tables[1].Rows[0]["DailySchedule_LunchTime"].ToString();

                    ////List<ListItem> selected = new List<ListItem>();
                    ////foreach (ListItem item in DailySchedule_LunchContent.Items)
                    ////    if (item.Selected) selected.Add(item);

                    //DailySchedule_AfternoonRoutine.Text = ds.Tables[1].Rows[0]["DailySchedule_AfternoonRoutine"].ToString();
                    //DailySchedule_Afternoo_nap.Text = ds.Tables[1].Rows[0]["DailySchedule_Afternoo_nap"].ToString();


                    ////foreach (ListItem item in DailySchedule_Snacks.Items)
                    ////    if (item.Selected) selected.Add(item);

                    //hfddinnertime.Value = ds.Tables[1].Rows[0]["DailySchedule_DinnerTime"].ToString();

                    ////foreach (ListItem item in DailySchedule_Dinner_content.Items)
                    ////    if (item.Selected) selected.Add(item);

                    //DailySchedule_PistDinnerAct.Text = ds.Tables[1].Rows[0]["DailySchedule_PistDinnerAct"].ToString();

                    //foreach (ListItem item in SelfCare_CurrentlyEats.Items)
                    //    if (item.Selected) selected.Add(item);
                    //#region
                    //SelfCare_Brushing_1.Checked = false; SelfCare_Brushing_2.Checked = false; SelfCare_Brushing_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Brushing"].ToString().Equals(SelfCare_Brushing_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Brushing_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Brushing"].ToString().Equals(SelfCare_Brushing_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Brushing_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Brushing"].ToString().Equals(SelfCare_Brushing_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Brushing_3.Checked = true;
                    //}


                    //SelfCare_Bathing_1.Checked = false; SelfCare_Bathing_2.Checked = false; SelfCare_Bathing_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Bathing"].ToString().Equals(SelfCare_Bathing_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Bathing_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Bathing"].ToString().Equals(SelfCare_Bathing_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Bathing_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Bathing"].ToString().Equals(SelfCare_Bathing_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Bathing_3.Checked = true;
                    //}

                    //SelfCare_Toileting_1.Checked = false; SelfCare_Toileting_2.Checked = false; SelfCare_Toileting_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Toileting"].ToString().Equals(SelfCare_Toileting_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Toileting_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Toileting"].ToString().Equals(SelfCare_Toileting_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Toileting_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Toileting"].ToString().Equals(SelfCare_Toileting_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Toileting_3.Checked = true;
                    //}

                    //SelfCare_Dressing_1.Checked = false; SelfCare_Dressing_2.Checked = false; SelfCare_Dressing_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dressing"].ToString().Equals(SelfCare_Dressing_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dressing_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dressing"].ToString().Equals(SelfCare_Dressing_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dressing_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dressing"].ToString().Equals(SelfCare_Dressing_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dressing_3.Checked = true;
                    //}


                    //SelfCare_Breakfast_1.Checked = false; SelfCare_Breakfast_2.Checked = false; SelfCare_Breakfast_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Breakfast"].ToString().Equals(SelfCare_Breakfast_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Breakfast_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Breakfast"].ToString().Equals(SelfCare_Breakfast_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Breakfast_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Breakfast"].ToString().Equals(SelfCare_Breakfast_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Breakfast_3.Checked = true;
                    //}

                    //SelfCare_Lunch_1.Checked = false; SelfCare_Lunch_2.Checked = false; SelfCare_Lunch_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Lunch"].ToString().Equals(SelfCare_Lunch_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Lunch_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Lunch"].ToString().Equals(SelfCare_Lunch_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Lunch_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Lunch"].ToString().Equals(SelfCare_Lunch_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Lunch_3.Checked = true;
                    //}

                    //SelfCare_Snacks_1.Checked = false; SelfCare_Snacks_2.Checked = false; SelfCare_Snacks_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Snacks"].ToString().Equals(SelfCare_Snacks_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Snacks_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Snacks"].ToString().Equals(SelfCare_Snacks_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Snacks_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Snacks"].ToString().Equals(SelfCare_Snacks_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Snacks_3.Checked = true;
                    //}


                    //SelfCare_Dinner_1.Checked = false; SelfCare_Dinner_2.Checked = false; SelfCare_Dinner_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dinner"].ToString().Equals(SelfCare_Dinner_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dinner_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dinner"].ToString().Equals(SelfCare_Dinner_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dinner_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Dinner"].ToString().Equals(SelfCare_Dinner_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Dinner_3.Checked = true;
                    //}

                    //SelfCare_GettingInBus_1.Checked = false; SelfCare_GettingInBus_2.Checked = false; SelfCare_GettingInBus_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_GettingInBus"].ToString().Equals(SelfCare_GettingInBus_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GettingInBus_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_GettingInBus"].ToString().Equals(SelfCare_GettingInBus_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GettingInBus_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_GettingInBus"].ToString().Equals(SelfCare_GettingInBus_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GettingInBus_3.Checked = true;
                    //}


                    //SelfCare_GoingToSchool_1.Checked = false; SelfCare_GoingToSchool_2.Checked = false; SelfCare_GoingToSchool_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_GoingToSchool"].ToString().Equals(SelfCare_GoingToSchool_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GoingToSchool_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_GoingToSchool"].ToString().Equals(SelfCare_GoingToSchool_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GoingToSchool_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_GoingToSchool"].ToString().Equals(SelfCare_GoingToSchool_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_GoingToSchool_3.Checked = true;
                    //}



                    //SelfCare_comeBackSchool_1.Checked = false; SelfCare_comeBackSchool_2.Checked = false; SelfCare_comeBackSchool_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_comeBackSchool"].ToString().Equals(SelfCare_comeBackSchool_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_comeBackSchool_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_comeBackSchool"].ToString().Equals(SelfCare_comeBackSchool_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_comeBackSchool_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_comeBackSchool"].ToString().Equals(SelfCare_comeBackSchool_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_comeBackSchool_3.Checked = true;
                    //}

                    //SelfCare_Ambulation_1.Checked = false; SelfCare_Ambulation_2.Checked = false; SelfCare_Ambulation_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["SelfCare_Ambulation"].ToString().Equals(SelfCare_Ambulation_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Ambulation_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Ambulation"].ToString().Equals(SelfCare_Ambulation_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Ambulation_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["SelfCare_Ambulation"].ToString().Equals(SelfCare_Ambulation_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    SelfCare_Ambulation_3.Checked = true;
                    //}


                    //SelfCare_Homeostaticchanges.Text = ds.Tables[1].Rows[0]["SelfCare_Homeostaticchanges"].ToString();
                    //SelfCare_UrinationdetailsBedwetting.Text = ds.Tables[1].Rows[0]["SelfCare_UrinationdetailsBedwetting"].ToString();
                    //#endregion

                    PersonalSocial_CurrentPlace_1.Checked = false; PersonalSocial_CurrentPlace_2.Checked = false; PersonalSocial_CurrentPlace_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_CurrentPlace"].ToString().Equals(PersonalSocial_CurrentPlace_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_CurrentPlace_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_CurrentPlace"].ToString().Equals(PersonalSocial_CurrentPlace_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_CurrentPlace_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_CurrentPlace"].ToString().Equals(PersonalSocial_CurrentPlace_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_CurrentPlace_3.Checked = true;
                    }

                    PersonalSocial_WhatHeDoes_1.Checked = false; PersonalSocial_WhatHeDoes_2.Checked = false; PersonalSocial_WhatHeDoes_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_WhatHeDoes"].ToString().Equals(PersonalSocial_WhatHeDoes_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_WhatHeDoes_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_WhatHeDoes"].ToString().Equals(PersonalSocial_WhatHeDoes_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_WhatHeDoes_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_WhatHeDoes"].ToString().Equals(PersonalSocial_WhatHeDoes_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_WhatHeDoes_3.Checked = true;
                    }

                    PersonalSocial_BodyAwareness_1.Checked = false; PersonalSocial_BodyAwareness_2.Checked = false; PersonalSocial_BodyAwareness_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodyAwareness"].ToString().Equals(PersonalSocial_BodyAwareness_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodyAwareness_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodyAwareness"].ToString().Equals(PersonalSocial_BodyAwareness_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodyAwareness_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodyAwareness"].ToString().Equals(PersonalSocial_BodyAwareness_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodyAwareness_3.Checked = true;
                    }

                    PersonalSocial_BodySchema_1.Checked = false; PersonalSocial_BodySchema_2.Checked = false; PersonalSocial_BodySchema_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodySchema"].ToString().Equals(PersonalSocial_BodySchema_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodySchema_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodySchema"].ToString().Equals(PersonalSocial_BodySchema_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodySchema_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_BodySchema"].ToString().Equals(PersonalSocial_BodySchema_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_BodySchema_3.Checked = true;
                    }

                    PersonalSocial_ExploreEnvironment_1.Checked = false; PersonalSocial_ExploreEnvironment_2.Checked = false; PersonalSocial_ExploreEnvironment_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ExploreEnvironment"].ToString().Equals(PersonalSocial_ExploreEnvironment_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ExploreEnvironment_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ExploreEnvironment"].ToString().Equals(PersonalSocial_ExploreEnvironment_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ExploreEnvironment_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ExploreEnvironment"].ToString().Equals(PersonalSocial_ExploreEnvironment_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ExploreEnvironment_3.Checked = true;
                    }

                    PersonalSocial_Motivated_1.Checked = false; PersonalSocial_Motivated_2.Checked = false; PersonalSocial_Motivated_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_Motivated"].ToString().Equals(PersonalSocial_Motivated_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_Motivated_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_Motivated"].ToString().Equals(PersonalSocial_Motivated_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_Motivated_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_Motivated"].ToString().Equals(PersonalSocial_Motivated_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_Motivated_3.Checked = true;
                    }

                    PersonalSocial_EyeContact_1.Checked = false; PersonalSocial_EyeContact_2.Checked = false; PersonalSocial_EyeContact_3.Checked = false; PersonalSocial_EyeContact_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString().Equals(PersonalSocial_EyeContact_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_EyeContact_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString().Equals(PersonalSocial_EyeContact_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_EyeContact_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString().Equals(PersonalSocial_EyeContact_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_EyeContact_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_EyeContact"].ToString().Equals(PersonalSocial_EyeContact_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_EyeContact_4.Checked = true;
                    }

                    PersonalSocial_SocialSmile_1.Checked = false; PersonalSocial_SocialSmile_2.Checked = false; PersonalSocial_SocialSmile_3.Checked = false; PersonalSocial_SocialSmile_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString().Equals(PersonalSocial_SocialSmile_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_SocialSmile_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString().Equals(PersonalSocial_SocialSmile_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_SocialSmile_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString().Equals(PersonalSocial_SocialSmile_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_SocialSmile_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_SocialSmile"].ToString().Equals(PersonalSocial_SocialSmile_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_SocialSmile_4.Checked = true;
                    }

                    PersonalSocial_FamilyRegards_1.Checked = false; PersonalSocial_FamilyRegards_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_FamilyRegards"].ToString().Equals(PersonalSocial_FamilyRegards_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_FamilyRegards_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_FamilyRegards"].ToString().Equals(PersonalSocial_FamilyRegards_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_FamilyRegards_2.Checked = true;
                    }

                    //PersonalSocial_RateChild.Text = ds.Tables[1].Rows[0]["PersonalSocial_RateChild"].ToString();

                    PersonalSocial_ChildSocially_1.Checked = false; PersonalSocial_ChildSocially_2.Checked = false; PersonalSocial_ChildSocially_3.Checked = false;
                    PersonalSocial_ChildSocially_4.Checked = false; PersonalSocial_ChildSocially_5.Checked = false;
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Equals(PersonalSocial_ChildSocially_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ChildSocially_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Equals(PersonalSocial_ChildSocially_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ChildSocially_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Equals(PersonalSocial_ChildSocially_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ChildSocially_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Equals(PersonalSocial_ChildSocially_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ChildSocially_4.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PersonalSocial_ChildSocially"].ToString().Equals(PersonalSocial_ChildSocially_5.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PersonalSocial_ChildSocially_5.Checked = true;
                    }


                    SpeechLanguage_StartSpeek.Text = ds.Tables[1].Rows[0]["SpeechLanguage_StartSpeek"].ToString();
                    SpeechLanguage_Monosyllables.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Monosyllables"].ToString();
                    SpeechLanguage_Bisyllables.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Bisyllables"].ToString();
                    SpeechLanguage_ShrotScentences.Text = ds.Tables[1].Rows[0]["SpeechLanguage_ShrotScentences"].ToString();
                    SpeechLanguage_LongScentences.Text = ds.Tables[1].Rows[0]["SpeechLanguage_LongScentences"].ToString();

                    SpeechLanguage_UnusualSoundsJargonSpeech_1.Checked = false; SpeechLanguage_UnusualSoundsJargonSpeech_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_UnusualSoundsJargonSpeech"].ToString().Equals(SpeechLanguage_UnusualSoundsJargonSpeech_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_UnusualSoundsJargonSpeech_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_UnusualSoundsJargonSpeech"].ToString().Equals(SpeechLanguage_UnusualSoundsJargonSpeech_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_UnusualSoundsJargonSpeech_2.Checked = true;
                    }

                    SpeechLanguage_speechgestures_1.Checked = false; SpeechLanguage_UnusualSoundsJargonSpeech_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_speechgestures"].ToString().Equals(SpeechLanguage_speechgestures_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_speechgestures_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_speechgestures"].ToString().Equals(SpeechLanguage_speechgestures_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_speechgestures_2.Checked = true;
                    }

                    SpeechLanguage_NonverbalfacialExpression.Text = ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialExpression"].ToString();
                    SpeechLanguage_NonverbalfacialEyeContact.Text = ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialEyeContact"].ToString();
                    SpeechLanguage_NonverbalfacialGestures.Text = ds.Tables[1].Rows[0]["SpeechLanguage_NonverbalfacialGestures"].ToString();
                    SpeechLanguage_SimpleComplex.Text = ds.Tables[1].Rows[0]["SpeechLanguage_SimpleComplex"].ToString();
                    SpeechLanguage_UnderstandImpliedMeaning.Text = ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandImpliedMeaning"].ToString();
                    SpeechLanguage_UnderstandJokesarcasm.Text = ds.Tables[1].Rows[0]["SpeechLanguage_UnderstandJokesarcasm"].ToString();
                    SpeechLanguage_Respondstoname.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Respondstoname"].ToString();

                    SpeechLanguage_TwowayInteraction_1.Checked = false; SpeechLanguage_TwowayInteraction_2.Checked = false; SpeechLanguage_TwowayInteraction_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_TwowayInteraction"].ToString().Equals(SpeechLanguage_TwowayInteraction_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_TwowayInteraction_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_TwowayInteraction"].ToString().Equals(SpeechLanguage_TwowayInteraction_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_TwowayInteraction_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SpeechLanguage_TwowayInteraction"].ToString().Equals(SpeechLanguage_TwowayInteraction_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        SpeechLanguage_TwowayInteraction_3.Checked = true;
                    }

                    SpeechLanguage_NarrateIncidentsAtSchool.Text = ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtSchool"].ToString();
                    SpeechLanguage_NarrateIncidentsAtHome.Text = ds.Tables[1].Rows[0]["SpeechLanguage_NarrateIncidentsAtHome"].ToString();
                    //SpeechLanguage_Want.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Want"].ToString();
                    SpeechLanguage_Needs.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Needs"].ToString();
                    SpeechLanguage_Emotions.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Emotions"].ToString();
                    SpeechLanguage_AchievementsFailure.Text = ds.Tables[1].Rows[0]["SpeechLanguage_AchievementsFailure"].ToString();
                    //SpeechLanguage_LanguageSpoken.Text = ds.Tables[1].Rows[0]["SpeechLanguage_LanguageSpoken"].ToString();
                    SpeechLanguage_Echolalia.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Echolalia"].ToString();
                    //SpeechLanguage_Emotionalmilestones.Text = ds.Tables[1].Rows[0]["SpeechLanguage_Emotionalmilestones"].ToString();


                    Behaviour_FreeTime.Text = ds.Tables[1].Rows[0]["Behaviour_FreeTime"].ToString();


                    chkunassociated.Checked = false; chksolitary.Checked = false; chkonlooker.Checked = false; chkparallel.Checked = false; chkassociative.Checked = false; chkcooperative.Checked = false;
                    if (ds.Tables[1].Rows[0]["unassociated"].ToString().Equals(chkunassociated.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkunassociated.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["solitary"].ToString().Equals(chksolitary.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chksolitary.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["onlooker"].ToString().Equals(chkonlooker.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkonlooker.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["parallel"].ToString().Equals(chkparallel.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkparallel.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["associative"].ToString().Equals(chkassociative.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkassociative.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["cooperative"].ToString().Equals(chkcooperative.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkcooperative.Checked = true;
                    }


                    Behaviour_situationalmeltdowns_1.Checked = false; Behaviour_situationalmeltdowns_2.Checked = false; Behaviour_situationalmeltdowns_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Behaviour_situationalmeltdowns"].ToString().Equals(Behaviour_situationalmeltdowns_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Behaviour_situationalmeltdowns_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Behaviour_situationalmeltdowns"].ToString().Equals(Behaviour_situationalmeltdowns_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Behaviour_situationalmeltdowns_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Behaviour_situationalmeltdowns"].ToString().Equals(Behaviour_situationalmeltdowns_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Behaviour_situationalmeltdowns_3.Checked = true;
                    }



                    FamilyStructure_QualityTimeMother_1.Checked = false; FamilyStructure_QualityTimeMother_2.Checked = false; FamilyStructure_QualityTimeMother_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeMother"].ToString().Equals(FamilyStructure_QualityTimeMother_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeMother_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeMother"].ToString().Equals(FamilyStructure_QualityTimeMother_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeMother_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeMother"].ToString().Equals(FamilyStructure_QualityTimeMother_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeMother_3.Checked = true;
                    }

                    FamilyStructure_QualityTimeFather_1.Checked = false; FamilyStructure_QualityTimeFather_2.Checked = false; FamilyStructure_QualityTimeFather_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeFather"].ToString().Equals(FamilyStructure_QualityTimeFather_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeFather_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeFather"].ToString().Equals(FamilyStructure_QualityTimeFather_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeFather_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeFather"].ToString().Equals(FamilyStructure_QualityTimeFather_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_QualityTimeFather_3.Checked = true;
                    }


                    Mother_Weekends_1.Checked = false; Mother_Weekends_2.Checked = false; Mother_Weekends_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeWeekend"].ToString().Equals(Mother_Weekends_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Mother_Weekends_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeWeekend"].ToString().Equals(Mother_Weekends_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Mother_Weekends_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_QualityTimeWeekend"].ToString().Equals(Mother_Weekends_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Mother_Weekends_3.Checked = true;
                    }

                    Father_Weekends_1.Checked = false; Father_Weekends_2.Checked = false; Father_Weekends_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Father_Weekends"].ToString().Equals(Father_Weekends_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Father_Weekends_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Father_Weekends"].ToString().Equals(Father_Weekends_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Father_Weekends_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Father_Weekends"].ToString().Equals(Father_Weekends_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Father_Weekends_3.Checked = true;
                    }

                    FamilyStructure_TimeForThreapy_1.Checked = false; FamilyStructure_TimeForThreapy_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_TimeForThreapy"].ToString().Equals(FamilyStructure_TimeForThreapy_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_TimeForThreapy_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_TimeForThreapy"].ToString().Equals(FamilyStructure_TimeForThreapy_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_TimeForThreapy_2.Checked = true;
                    }

                    FamilyStructure_AcceptanceCondition_1.Checked = false; FamilyStructure_AcceptanceCondition_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_AcceptanceCondition"].ToString().Equals(FamilyStructure_AcceptanceCondition_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_AcceptanceCondition_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_AcceptanceCondition"].ToString().Equals(FamilyStructure_AcceptanceCondition_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_AcceptanceCondition_2.Checked = true;
                    }

                    FamilyStructure_ExtraCaricular_1.Checked = false; FamilyStructure_ExtraCaricular_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure_ExtraCaricular"].ToString().Equals(FamilyStructure_ExtraCaricular_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_ExtraCaricular_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_ExtraCaricular"].ToString().Equals(FamilyStructure_ExtraCaricular_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        FamilyStructure_ExtraCaricular_2.Checked = true;
                    }

                    //string Mother_Working = string.Empty;
                    //if (Mother_Working_1.Checked) { Mother_Working = Mother_Working_1.Text.Trim(); }
                    //if (Mother_Working_2.Checked) { Mother_Working = Mother_Working_2.Text.Trim(); }

                    //string Father_Working = string.Empty;
                    //if (Father_Working_1.Checked) { Father_Working = Father_Working_1.Text.Trim(); }
                    //if (Father_Working_2.Checked) { Father_Working = Father_Working_2.Text.Trim(); }

                    //string Househelp = string.Empty;
                    //if (Househelp_1.Checked) { Househelp = Househelp_1.Text.Trim(); }
                    //if (Househelp_2.Checked) { Househelp = Househelp_2.Text.Trim(); }


                    FamilyStructure_Diciplinary.Text = ds.Tables[1].Rows[0]["FamilyStructure_Diciplinary"].ToString();
                    FamilyStructure_SiblingBrother.Text = ds.Tables[1].Rows[0]["FamilyStructure_SiblingBrother"].ToString();
                    FamilyStructure_Expectations.Text = ds.Tables[1].Rows[0]["FamilyStructure_Expectations"].ToString();

                    FamilyStructure_CloselyInvolved.Text = ds.Tables[1].Rows[0]["FamilyStructure_CloselyInvolved"].ToString();
                    FAMILY_cmt.Text = ds.Tables[1].Rows[0]["FAMILY_cmt"].ToString();
                    ClinicleObse_txt.Text = ds.Tables[1].Rows[0]["ClinicalObservation"].ToString();


                    //FamilyStructure_SiblingBrother_1.Checked = false; FamilyStructure_SiblingBrother_2.Checked = false;
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingBrother"].ToString().Equals(FamilyStructure_SiblingBrother_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingBrother_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingBrother"].ToString().Equals(FamilyStructure_SiblingBrother_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingBrother_2.Checked = true;
                    //}

                    //FamilyStructure_SiblingSister_1.Checked = false; FamilyStructure_SiblingSister_2.Checked = false;
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingSister"].ToString().Equals(FamilyStructure_SiblingSister_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingSister_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingSister"].ToString().Equals(FamilyStructure_SiblingSister_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingSister_2.Checked = true;
                    //}

                    //FamilyStructure_SiblingNA_1.Checked = false; FamilyStructure_SiblingNA_2.Checked = false;
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingNA"].ToString().Equals(FamilyStructure_SiblingNA_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingNA_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["FamilyStructure_SiblingNA"].ToString().Equals(FamilyStructure_SiblingNA_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    FamilyStructure_SiblingNA_2.Checked = true;
                    //}



                    //Schoolinfo_Attend.Text = ds.Tables[1].Rows[0]["Schoolinfo_Attend"].ToString();
                    Schoolinfo_Attend_1.Checked = false; Schoolinfo_Attend_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Attend"].ToString().Equals(Schoolinfo_Attend_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Attend_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Attend"].ToString().Equals(Schoolinfo_Attend_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Attend_2.Checked = true;
                    }

                    Schoolinfo_Type_1.Checked = false; Schoolinfo_Type_2.Checked = false; Schoolinfo_Type_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Type"].ToString().Equals(Schoolinfo_Type_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Type_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Type"].ToString().Equals(Schoolinfo_Type_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Type_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Type"].ToString().Equals(Schoolinfo_Type_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Type_3.Checked = true;
                    }
                    Schoolinfo_SchoolHours.SelectedValue = ds.Tables[1].Rows[0]["Schoolinfo_SchoolHours"].ToString();

                    chkSchool_Bus.Checked = false; chkCar.Checked = false; chkTwo_Wheelers.Checked = false; chkwalking.Checked = false; chkPublic_Transport.Checked = false;
                    if (ds.Tables[1].Rows[0]["School_Bus"].ToString().Equals(chkSchool_Bus.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkSchool_Bus.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Car"].ToString().Equals(chkCar.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkCar.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Two_Wheelers"].ToString().Equals(chkTwo_Wheelers.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkTwo_Wheelers.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["walking"].ToString().Equals(chkwalking.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkwalking.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Public_Transport"].ToString().Equals(chkPublic_Transport.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkPublic_Transport.Checked = true;
                    }


                    Schoolinfo_NoOfTeacher_1.Checked = false; Schoolinfo_NoOfTeacher_2.Checked = false; Schoolinfo_NoOfTeacher_3.Checked = false; Schoolinfo_NoOfTeacher_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString().Equals(Schoolinfo_NoOfTeacher_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_NoOfTeacher_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString().Equals(Schoolinfo_NoOfTeacher_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_NoOfTeacher_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString().Equals(Schoolinfo_NoOfTeacher_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_NoOfTeacher_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfTeacher"].ToString().Equals(Schoolinfo_NoOfTeacher_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_NoOfTeacher_4.Checked = true;
                    }

                    //Schoolinfo_NoOfStudent_1.Checked = false; Schoolinfo_NoOfStudent_2.Checked = false; Schoolinfo_NoOfStudent_3.Checked = false; Schoolinfo_NoOfStudent_4.Checked = false;
                    //if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfStudent"].ToString().Equals(Schoolinfo_NoOfStudent_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Schoolinfo_NoOfStudent_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfStudent"].ToString().Equals(Schoolinfo_NoOfStudent_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Schoolinfo_NoOfStudent_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfStudent"].ToString().Equals(Schoolinfo_NoOfStudent_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Schoolinfo_NoOfStudent_3.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Schoolinfo_NoOfStudent"].ToString().Equals(Schoolinfo_NoOfStudent_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Schoolinfo_NoOfStudent_4.Checked = true;
                    //}


                    chkFloor.Checked = false; chksingle_bench.Checked = false; chkbench2.Checked = false; chkround_table.Checked = false;
                    if (ds.Tables[1].Rows[0]["Floor"].ToString().Equals(chkFloor.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkFloor.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["single_bench"].ToString().Equals(chksingle_bench.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chksingle_bench.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["bench2"].ToString().Equals(chkbench2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkbench2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["round_table"].ToString().Equals(chkround_table.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkround_table.Checked = true;
                    }



                    Schoolinfo_Mealtime.SelectedValue = ds.Tables[1].Rows[0]["Schoolinfo_Mealtime"].ToString();
                    //foreach (ListItem item in Schoolinfo_Mealtime.Items)
                    //    if (item.Selected) selected.Add(item);

                    Schoolinfo_MealType_1.Checked = false; Schoolinfo_MealType_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_MealType"].ToString().Equals(Schoolinfo_MealType_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_MealType_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_MealType"].ToString().Equals(Schoolinfo_MealType_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_MealType_2.Checked = true;
                    }

                    Schoolinfo_Shareing_1.Checked = false; Schoolinfo_Shareing_2.Checked = false; Schoolinfo_Shareing_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Shareing"].ToString().Equals(Schoolinfo_Shareing_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Shareing_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Shareing"].ToString().Equals(Schoolinfo_Shareing_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Shareing_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Shareing"].ToString().Equals(Schoolinfo_Shareing_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Shareing_3.Checked = true;
                    }

                    Schoolinfo_HelpEating_1.Checked = false; Schoolinfo_HelpEating_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_HelpEating"].ToString().Equals(Schoolinfo_HelpEating_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_HelpEating_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_HelpEating"].ToString().Equals(Schoolinfo_HelpEating_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_HelpEating_2.Checked = true;
                    }

                    Schoolinfo_Friendship_1.Checked = false; Schoolinfo_Friendship_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Friendship"].ToString().Equals(Schoolinfo_Friendship_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Friendship_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Friendship"].ToString().Equals(Schoolinfo_Friendship_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Friendship_2.Checked = true;
                    }

                    Schoolinfo_InteractionPeer_1.Checked = false; Schoolinfo_InteractionPeer_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionPeer"].ToString().Equals(Schoolinfo_InteractionPeer_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_InteractionPeer_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionPeer"].ToString().Equals(Schoolinfo_InteractionPeer_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_InteractionPeer_2.Checked = true;
                    }

                    Schoolinfo_InteractionTeacher_1.Checked = false; Schoolinfo_InteractionTeacher_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionTeacher"].ToString().Equals(Schoolinfo_InteractionTeacher_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_InteractionTeacher_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_InteractionTeacher"].ToString().Equals(Schoolinfo_InteractionTeacher_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_InteractionTeacher_2.Checked = true;
                    }

                    Schoolinfo_AnnualFunction_1.Checked = false; Schoolinfo_InteractionTeacher_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_AnnualFunction"].ToString().Equals(Schoolinfo_AnnualFunction_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_AnnualFunction_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_AnnualFunction"].ToString().Equals(Schoolinfo_AnnualFunction_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_AnnualFunction_2.Checked = true;
                    }

                    Schoolinfo_Sports_1.Checked = false; Schoolinfo_Sports_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Sports"].ToString().Equals(Schoolinfo_Sports_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Sports_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Sports"].ToString().Equals(Schoolinfo_Sports_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Sports_2.Checked = true;
                    }

                    Schoolinfo_Picnic_1.Checked = false; Schoolinfo_Picnic_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Picnic"].ToString().Equals(Schoolinfo_Picnic_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Picnic_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Picnic"].ToString().Equals(Schoolinfo_Picnic_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Picnic_2.Checked = true;
                    }

                    Schoolinfo_ExtraCaricular_1.Checked = false; Schoolinfo_ExtraCaricular_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ExtraCaricular"].ToString().Equals(Schoolinfo_ExtraCaricular_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ExtraCaricular_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ExtraCaricular"].ToString().Equals(Schoolinfo_ExtraCaricular_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ExtraCaricular_2.Checked = true;
                    }

                    Schoolinfo_CopyBoard_1.Checked = false; Schoolinfo_CopyBoard_2.Checked = false; Schoolinfo_CopyBoard_3.Checked = false; Schoolinfo_CopyBoard_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString().Equals(Schoolinfo_CopyBoard_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CopyBoard_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString().Equals(Schoolinfo_CopyBoard_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CopyBoard_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString().Equals(Schoolinfo_CopyBoard_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CopyBoard_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CopyBoard"].ToString().Equals(Schoolinfo_CopyBoard_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CopyBoard_4.Checked = true;
                    }

                    Schoolinfo_Instructions_1.Checked = false; Schoolinfo_Instructions_2.Checked = false; Schoolinfo_Instructions_3.Checked = false; Schoolinfo_Instructions_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString().Equals(Schoolinfo_Instructions_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Instructions_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString().Equals(Schoolinfo_Instructions_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Instructions_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString().Equals(Schoolinfo_Instructions_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Instructions_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_Instructions"].ToString().Equals(Schoolinfo_Instructions_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_Instructions_4.Checked = true;
                    }

                    Schoolinfo_ShadowTeacher_1.Checked = false; Schoolinfo_ShadowTeacher_2.Checked = false; Schoolinfo_ShadowTeacher_3.Checked = false; Schoolinfo_ShadowTeacher_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString().Equals(Schoolinfo_ShadowTeacher_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ShadowTeacher_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString().Equals(Schoolinfo_ShadowTeacher_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ShadowTeacher_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString().Equals(Schoolinfo_ShadowTeacher_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ShadowTeacher_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_ShadowTeacher"].ToString().Equals(Schoolinfo_ShadowTeacher_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_ShadowTeacher_4.Checked = true;
                    }

                    Schoolinfo_CW_HW_1.Checked = false; Schoolinfo_CW_HW_2.Checked = false; Schoolinfo_CW_HW_3.Checked = false; Schoolinfo_CW_HW_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString().Equals(Schoolinfo_CW_HW_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CW_HW_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString().Equals(Schoolinfo_CW_HW_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CW_HW_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString().Equals(Schoolinfo_CW_HW_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CW_HW_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_CW_HW"].ToString().Equals(Schoolinfo_CW_HW_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_CW_HW_4.Checked = true;
                    }

                    Schoolinfo_SpecialEducator_1.Checked = false; Schoolinfo_SpecialEducator_2.Checked = false; Schoolinfo_SpecialEducator_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_SpecialEducator"].ToString().Equals(Schoolinfo_SpecialEducator_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_SpecialEducator_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_SpecialEducator"].ToString().Equals(Schoolinfo_SpecialEducator_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_SpecialEducator_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_SpecialEducator"].ToString().Equals(Schoolinfo_SpecialEducator_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_SpecialEducator_3.Checked = true;
                    }

                    Schoolinfo_DeliveryInformation_1.Checked = false; Schoolinfo_DeliveryInformation_2.Checked = false; Schoolinfo_DeliveryInformation_3.Checked = false; Schoolinfo_DeliveryInformation_4.Checked = false;
                    if (ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString().Equals(Schoolinfo_DeliveryInformation_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_DeliveryInformation_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString().Equals(Schoolinfo_DeliveryInformation_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_DeliveryInformation_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString().Equals(Schoolinfo_DeliveryInformation_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_DeliveryInformation_3.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Schoolinfo_DeliveryInformation"].ToString().Equals(Schoolinfo_DeliveryInformation_4.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Schoolinfo_DeliveryInformation_4.Checked = true;
                    }

                    Schoolinfo_RemarkTeacher.Text = ds.Tables[1].Rows[0]["Schoolinfo_RemarkTeacher"].ToString();

                    //foreach (ListItem item in Schoolinfo_PlatformInteraction.Items)
                    //    if (item.Selected) selected.Add(item);

                    //Schoolinfo_HourOnlineSchool.Text = ds.Tables[1].Rows[0]["Schoolinfo_HourOnlineSchool"].ToString();

                    //string Schoolinfo_SitOnlineSchool = string.Empty;
                    //if (Schoolinfo_SitOnlineSchool_1.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_1.Text.Trim(); }
                    //if (Schoolinfo_SitOnlineSchool_2.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_2.Text.Trim(); }
                    //if (Schoolinfo_SitOnlineSchool_3.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_3.Text.Trim(); }

                    //string Schoolinfo_TeacherInstruction = string.Empty;
                    //if (Schoolinfo_TeacherInstruction_1.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_1.Text.Trim(); }
                    //if (Schoolinfo_TeacherInstruction_2.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_2.Text.Trim(); }
                    //if (Schoolinfo_TeacherInstruction_3.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_3.Text.Trim(); }

                    //Schoolinfo_SetUp.Text = ds.Tables[1].Rows[0]["Schoolinfo_SetUp"].ToString();
                    //Schoolinfo_BehaviourOnlineSchool.Text = ds.Tables[1].Rows[0]["Schoolinfo_BehaviourOnlineSchool"].ToString();

                    //foreach (ListItem item in Arousal_Evaluation.Items)
                    //    if (item.Selected) selected.Add(item);
                    //string rangevalue = hdnrange.Value.ToString();

                    hdnrange.Value = ds.Tables[1].Rows[0]["rangevalue"].ToString() + "0";
                    Hdnrange2.Value = ds.Tables[1].Rows[0]["rangevalue2"].ToString() + "0";

                    //foreach (ListItem item in Arousal_GeneralState.Items)
                    //    if (item.Selected) selected.Add(item);

                    Arousal_Stimuli_1.Checked = false; Arousal_Stimuli_2.Checked = false; Arousal_Stimuli_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Arousal_Stimuli"].ToString().Equals(Arousal_Stimuli_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Arousal_Stimuli_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Arousal_Stimuli"].ToString().Equals(Arousal_Stimuli_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Arousal_Stimuli_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Arousal_Stimuli"].ToString().Equals(Arousal_Stimuli_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Arousal_Stimuli_3.Checked = true;
                    }

                    Arousal_Transition_1.Checked = false; Arousal_Transition_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Arousal_Transition"].ToString().Equals(Arousal_Transition_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Arousal_Transition_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Arousal_Transition"].ToString().Equals(Arousal_Transition_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Arousal_Transition_2.Checked = true;
                    }
                    //Arousal_Optimal.Text = ds.Tables[1].Rows[0]["Arousal_Optimal"].ToString();

                    //foreach (ListItem item in Arousal_FactorOCD.Items)
                    //    if (item.Selected) selected.Add(item);
                    Arousal_FactorOCD.Text = ds.Tables[1].Rows[0]["Arousal_FactorOCD"].ToString();
                    Arousal_ClaimingFactor.Text = ds.Tables[1].Rows[0]["Arousal_ClaimingFactor"].ToString();

                    //foreach (ListItem item in Arousal_ClaimingFactor.Items)
                    //    if (item.Selected) selected.Add(item);

                    Arousal_DipsDown.Text = ds.Tables[1].Rows[0]["Arousal_DipsDown"].ToString();

                    //foreach (ListItem item in Attention_Span.Items)
                    //    if (item.Selected) selected.Add(item);
                    Attention_FocusHandhome_1.Checked = false; Attention_FocusHandhome_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_FocusHandhome"].ToString().Equals(Attention_FocusHandhome_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_FocusHandhome_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_FocusHandhome"].ToString().Equals(Attention_FocusHandhome_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_FocusHandhome_2.Checked = true;
                    }

                    Attention_FocusHandSchool_1.Checked = false; Attention_FocusHandSchool_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_FocusHandSchool"].ToString().Equals(Attention_FocusHandSchool_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_FocusHandSchool_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_FocusHandSchool"].ToString().Equals(Attention_FocusHandSchool_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_FocusHandSchool_2.Checked = true;
                    }

                    Attention_Dividing_1.Checked = false; Attention_Dividing_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_Dividing"].ToString().Equals(Attention_Dividing_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_Dividing_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_Dividing"].ToString().Equals(Attention_Dividing_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_Dividing_2.Checked = true;
                    }

                    Attention_ChangeActivities.Text = ds.Tables[1].Rows[0]["Attention_ChangeActivities"].ToString();

                    Attention_AgeAppropriate_1.Checked = false; Attention_AgeAppropriate_2.Checked = false; Attention_AgeAppropriate_3.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Equals(Attention_AgeAppropriate_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_AgeAppropriate_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Equals(Attention_AgeAppropriate_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_AgeAppropriate_2.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_AgeAppropriate"].ToString().Equals(Attention_AgeAppropriate_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_AgeAppropriate_3.Checked = true;
                    }

                    Attention_AttentionSpan.Text = ds.Tables[1].Rows[0]["Attention_AttentionSpan"].ToString();
                    Attention_Distractibility.Text = ds.Tables[1].Rows[0]["Attention_Distractibility"].ToString();
                    Focal_Attention.Text = ds.Tables[1].Rows[0]["Focal_Attention"].ToString();
                    Joint_Attention.Text = ds.Tables[1].Rows[0]["Joint_Attention"].ToString();
                    Divided_Attention.Text = ds.Tables[1].Rows[0]["Divided_Attention"].ToString();
                    Alternating_Attention.Text = ds.Tables[1].Rows[0]["Alternating_Attention"].ToString();
                    Sustained_Attention.Text = ds.Tables[1].Rows[0]["Sustained_Attention"].ToString();

                    Attention_move_1.Checked = false; Attention_move_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Attention_move"].ToString().Equals(Attention_move_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_move_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Attention_move"].ToString().Equals(Attention_move_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Attention_move_2.Checked = true;
                    }

                    //Interaction_KnowPeople.Text = ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString();
                    //Interaction_KnowPeople_1.Checked = false; Interaction_KnowPeople_2.Checked = false; Interaction_KnowPeople_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString().Equals(Interaction_KnowPeople_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_KnowPeople_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString().Equals(Interaction_KnowPeople_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_KnowPeople_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Interaction_KnowPeople"].ToString().Equals(Interaction_KnowPeople_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_KnowPeople_3.Checked = true;
                    //}

                    //Interaction_Strangers.Text = ds.Tables[1].Rows[0]["Interaction_Strangers"].ToString();
                    cmtgathering.Text = ds.Tables[1].Rows[0]["cmtgathering"].ToString();
                    chkInteracts.Checked = false; chkDoes_not_initiate.Checked = false; chkSustain.Checked = false; chkFight.Checked = false; chkFreeze.Checked = false; chkFright.Checked = false;
                    chkAnxious.Checked = false; chkComfortable.Checked = false; chkNervous.Checked = false; chkANS_response.Checked = false; chkOTHERS.Checked = false;
                    if (ds.Tables[1].Rows[0]["Interacts"].ToString().Equals(chkInteracts.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkInteracts.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Does_not_initiate"].ToString().Equals(chkDoes_not_initiate.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkDoes_not_initiate.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Sustain"].ToString().Equals(chkSustain.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkSustain.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Fight"].ToString().Equals(chkFight.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkFight.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Freeze"].ToString().Equals(chkFreeze.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkFreeze.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Fright"].ToString().Equals(chkFright.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkFright.Checked = true;
                    }

                    if (ds.Tables[1].Rows[0]["Anxious"].ToString().Equals(chkAnxious.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkAnxious.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Comfortable"].ToString().Equals(chkComfortable.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkComfortable.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Nervous"].ToString().Equals(chkNervous.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkNervous.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ANS_response"].ToString().Equals(chkANS_response.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkANS_response.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["OTHERS"].ToString().Equals(chkOTHERS.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        chkOTHERS.Checked = true;
                    }

                    Interaction_SocialQues_1.Checked = false; Interaction_SocialQues_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Interaction_SocialQues"].ToString().Equals(Interaction_SocialQues_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Interaction_SocialQues_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Interaction_SocialQues"].ToString().Equals(Interaction_SocialQues_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Interaction_SocialQues_2.Checked = true;
                    }

                    Interaction_Happiness.Text = ds.Tables[1].Rows[0]["Interaction_Happiness"].ToString();
                    Interaction_Sadness.Text = ds.Tables[1].Rows[0]["Interaction_Sadness"].ToString();
                    Interaction_Surprise.Text = ds.Tables[1].Rows[0]["Interaction_Surprise"].ToString();
                    Interaction_Shock.Text = ds.Tables[1].Rows[0]["Interaction_Shock"].ToString();

                    Interaction_Friends_1.Checked = false; Interaction_Friends_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Interaction_Friends"].ToString().Equals(Interaction_Friends_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Interaction_Friends_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Interaction_Friends"].ToString().Equals(Interaction_Friends_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Interaction_Friends_2.Checked = true;
                    }

                    //string Interaction_RelatesPeople = string.Empty;
                    //if (Interaction_RelatesPeople_1.Checked)  { Interaction_RelatesPeople = Interaction_RelatesPeople_1.Text.Trim(); }
                    //if (Interaction_RelatesPeople_2.Checked) { Interaction_RelatesPeople = Interaction_RelatesPeople_2.Text.Trim(); }
                    //if (Interaction_RelatesPeople_3.Checked) { Interaction_RelatesPeople = Interaction_RelatesPeople_3.Text.Trim(); }
                    //Interaction_RelatesPeople_1.Checked = false; Interaction_RelatesPeople_2.Checked = false; Interaction_RelatesPeople_3.Checked = false;
                    //if (ds.Tables[1].Rows[0]["Interaction_RelatesPeople"].ToString().Equals(Interaction_RelatesPeople_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_RelatesPeople_1.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Interaction_RelatesPeople"].ToString().Equals(Interaction_RelatesPeople_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_RelatesPeople_2.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["Interaction_RelatesPeople"].ToString().Equals(Interaction_RelatesPeople_3.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    Interaction_RelatesPeople_3.Checked = true;
                    //}
                    //Interaction_RelatesPeople.Text = ds.Tables[1].Rows[0]["Interaction_RelatesPeople"].ToString();
                    Interaction_Enjoy.Text = ds.Tables[1].Rows[0]["Interaction_Enjoy"].ToString();

                    Affect_RangeEmotion_1.Checked = false; Affect_RangeEmotion_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Affect_RangeEmotion"].ToString().Equals(Affect_RangeEmotion_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_RangeEmotion_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_RangeEmotion"].ToString().Equals(Affect_RangeEmotion_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_RangeEmotion_2.Checked = true;
                    }


                    Affect_ExpressEmotion_1.Checked = false; Affect_ExpressEmotion_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Affect_ExpressEmotion"].ToString().Equals(Affect_ExpressEmotion_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_ExpressEmotion_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Affect_ExpressEmotion"].ToString().Equals(Affect_ExpressEmotion_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Affect_ExpressEmotion_2.Checked = true;
                    }

                    Affect_Environment.Text = ds.Tables[1].Rows[0]["Affect_Environment"].ToString();
                    Affect_Task.Text = ds.Tables[1].Rows[0]["Affect_Task"].ToString();
                    Affect_Individual.Text = ds.Tables[1].Rows[0]["Affect_Individual"].ToString();
                    Affect_ThroughOut.Text = ds.Tables[1].Rows[0]["Affect_ThroughOut"].ToString();
                    Affect_Charaterising.Text = ds.Tables[1].Rows[0]["Affect_Charaterising"].ToString();

                    Action_MotorPlanning.Text = ds.Tables[1].Rows[0]["Action_MotorPlanning"].ToString();

                    Action_Purposeful_1.Checked = false; Action_Purposeful_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Equals(Action_Purposeful_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Purposeful_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_Purposeful"].ToString().Equals(Action_Purposeful_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Purposeful_2.Checked = true;
                    }

                    Action_GoalOriented_1.Checked = false; Action_GoalOriented_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Equals(Action_GoalOriented_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_GoalOriented_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_GoalOriented"].ToString().Equals(Action_GoalOriented_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_GoalOriented_2.Checked = true;
                    }

                    Action_FeedBackDependent_1.Checked = false; Action_FeedBackDependent_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_FeedBackDependent"].ToString().Equals(Action_FeedBackDependent_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_FeedBackDependent_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_FeedBackDependent"].ToString().Equals(Action_FeedBackDependent_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_FeedBackDependent_2.Checked = true;
                    }

                    Action_Constructive_1.Checked = false; Action_Constructive_2.Checked = false;
                    if (ds.Tables[1].Rows[0]["Action_Constructive"].ToString().Equals(Action_Constructive_1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Constructive_1.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Action_Constructive"].ToString().Equals(Action_Constructive_2.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Action_Constructive_2.Checked = true;
                    }

                    //TestMeassures_IQ.Text = ds.Tables[1].Rows[0]["TestMeassures_IQ"].ToString();
                    //TestMeassures_DQ.Text = ds.Tables[1].Rows[0]["TestMeassures_DQ"].ToString();
                    TestMeassures_GrossMotor.Text = ds.Tables[1].Rows[0]["TestMeassures_GrossMotor"].ToString();
                    TestMeassures_FineMotor.Text = ds.Tables[1].Rows[0]["TestMeassures_FineMotor"].ToString();
                    TestMeassures_DenverLanguage.Text = ds.Tables[1].Rows[0]["TestMeassures_DenverLanguage"].ToString();
                    TestMeassures_DenverPersonal.Text = ds.Tables[1].Rows[0]["TestMeassures_DenverPersonal"].ToString();
                    //TestMeassures_ASQ.Text = ds.Tables[1].Rows[0]["TestMeassures_ASQ"].ToString();
                    //TestMeassures_HandWriting.Text = ds.Tables[1].Rows[0]["TestMeassures_HandWriting"].ToString();
                    //TestMeassures_SIPT.Text = ds.Tables[1].Rows[0]["TestMeassures_SIPT"].ToString();
                    //TestMeassures_SensoryProfile.Text = ds.Tables[1].Rows[0]["TestMeassures_SensoryProfile"].ToString();

                    Treatment_Home.Text = ds.Tables[1].Rows[0]["Treatment_Home"].ToString();
                    Treatment_School.Text = ds.Tables[1].Rows[0]["Treatment_School"].ToString();
                    Treatment_Threapy.Text = ds.Tables[1].Rows[0]["Treatment_Threapy"].ToString();

                    //Daily_cmt.Text = ds.Tables[1].Rows[0]["Daily_cmt"].ToString();
                    //Self_cmt.Text = ds.Tables[1].Rows[0]["Self_cmt"].ToString();
                    PERSONAL_cmt.Text = ds.Tables[1].Rows[0]["PERSONAL_cmt"].ToString();
                    Speech_cmt.Text = ds.Tables[1].Rows[0]["Speech_cmt"].ToString();
                    BEHAVIOUR_cmt.Text = ds.Tables[1].Rows[0]["BEHAVIOUR_cmt"].ToString();

                    SCHOOL_cmt.Text = ds.Tables[1].Rows[0]["SCHOOL_cmt"].ToString();
                    AROUSAL_cmt.Text = ds.Tables[1].Rows[0]["AROUSAL_cmt"].ToString();
                    ATTENTION_cmt.Text = ds.Tables[1].Rows[0]["ATTENTION_cmt"].ToString();
                    INTERACTION_cmt.Text = ds.Tables[1].Rows[0]["INTERACTION_cmt"].ToString();
                    Affect_cmt.Text = ds.Tables[1].Rows[0]["Affect_cmt"].ToString();
                    Action_cmt.Text = ds.Tables[1].Rows[0]["Action_cmt"].ToString();
                    Tests_cmt.Text = ds.Tables[1].Rows[0]["Tests_cmt"].ToString();
                    Treatment_cmt.Text = ds.Tables[1].Rows[0]["Treatment_cmt"].ToString();


                    General_Processing.Text = ds.Tables[1].Rows[0]["General_Processing"].ToString();
                    AUDITORY_Processing.Text = ds.Tables[1].Rows[0]["AUDITORY_Processing"].ToString();
                    VISUAL_Processing.Text = ds.Tables[1].Rows[0]["VISUAL_Processing"].ToString();
                    TOUCH_Processing.Text = ds.Tables[1].Rows[0]["TOUCH_Processing"].ToString();
                    MOVEMENT_Processing.Text = ds.Tables[1].Rows[0]["MOVEMENT_Processing"].ToString();
                    ORAL_Processing.Text = ds.Tables[1].Rows[0]["ORAL_Processing"].ToString();
                    Raw_score.Text = ds.Tables[1].Rows[0]["Raw_score"].ToString();


                    //Percentile_Range.Text = ds.Tables[1].Rows[0]["Percentile_Range"].ToString();
                    Total_rawscore.Text = ds.Tables[1].Rows[0]["Total_rawscore"].ToString();
                    Interpretation.Text = ds.Tables[1].Rows[0]["Interpretation"].ToString();
                    Comments_1.Text = ds.Tables[1].Rows[0]["Comments_1"].ToString();

                    //Score_seeking.Text = ds.Tables[1].Rows[0]["Score_seeking"].ToString();
                    //Score_Avoiding.Text = ds.Tables[1].Rows[0]["Score_Avoiding"].ToString();
                    //Score_sensitivity.Text = ds.Tables[1].Rows[0]["Score_sensitivity"].ToString();
                    //Score_Registration.Text = ds.Tables[1].Rows[0]["Score_Registration"].ToString();


                    SEEKING.SelectedValue = ds.Tables[1].Rows[0]["SEEKING"].ToString();
                    AVOIDING.SelectedValue = ds.Tables[1].Rows[0]["AVOIDING"].ToString();
                    SENSITIVITY_2.SelectedValue = ds.Tables[1].Rows[0]["SENSITIVITY_2"].ToString();
                    REGISTRATION.SelectedValue = ds.Tables[1].Rows[0]["REGISTRATION"].ToString();
                    GENERAL.SelectedValue = ds.Tables[1].Rows[0]["GENERAL"].ToString();
                    AUDITORY.SelectedValue = ds.Tables[1].Rows[0]["AUDITORY"].ToString();
                    VISUAL.SelectedValue = ds.Tables[1].Rows[0]["VISUAL"].ToString();
                    TOUCH.SelectedValue = ds.Tables[1].Rows[0]["TOUCH"].ToString();
                    MOVEMENT.SelectedValue = ds.Tables[1].Rows[0]["MOVEMENT"].ToString();
                    ORAL.SelectedValue = ds.Tables[1].Rows[0]["ORAL"].ToString();
                    BEHAVIORAL.SelectedValue = ds.Tables[1].Rows[0]["BEHAVIORAL"].ToString();
                    Comments_2.Text = ds.Tables[1].Rows[0]["Comments_2"].ToString();

                    ability_TOTAL.Text = ds.Tables[1].Rows[0]["ability_TOTAL"].ToString();
                    ability_COMMENTS.Text = ds.Tables[1].Rows[0]["ability_COMMENTS"].ToString();

                    Score_seeking.Text = ds.Tables[1].Rows[0]["Score_seeking"].ToString();
                    Score_Avoiding.Text = ds.Tables[1].Rows[0]["Score_Avoiding"].ToString();
                    Score_sensitivity.Text = ds.Tables[1].Rows[0]["Score_sensitivity"].ToString();
                    Score_Registration.Text = ds.Tables[1].Rows[0]["Score_Registration"].ToString();
                    Score_general.Text = ds.Tables[1].Rows[0]["Score_general"].ToString();
                    Score_Auditory.Text = ds.Tables[1].Rows[0]["Score_Auditory"].ToString();
                    Score_visual.Text = ds.Tables[1].Rows[0]["Score_visual"].ToString();
                    Score_touch.Text = ds.Tables[1].Rows[0]["Score_touch"].ToString();
                    Score_movement.Text = ds.Tables[1].Rows[0]["Score_movement"].ToString();
                    Score_oral.Text = ds.Tables[1].Rows[0]["Score_oral"].ToString();
                    Score_behavioural.Text = ds.Tables[1].Rows[0]["Score_behavioural"].ToString();

                    Seeking_Seeker.SelectedValue = ds.Tables[1].Rows[0]["Seeking_Seeker"].ToString();
                    SPchild_Seeker.Text = ds.Tables[1].Rows[0]["SPchild_Seeker"].ToString();
                    Avoiding_Avoider.SelectedValue = ds.Tables[1].Rows[0]["Avoiding_Avoider"].ToString();
                    SPchild_Avoider.Text = ds.Tables[1].Rows[0]["SPchild_Avoider"].ToString();
                    Sensitivity_Sensor.SelectedValue = ds.Tables[1].Rows[0]["Sensitivity_Sensor"].ToString();
                    SPchild_Sensor.Text = ds.Tables[1].Rows[0]["SPchild_Sensor"].ToString();
                    Registration_Bystander.SelectedValue = ds.Tables[1].Rows[0]["Registration_Bystander"].ToString();
                    SPchild_Bystander.Text = ds.Tables[1].Rows[0]["SPchild_Bystander"].ToString();
                    Auditory_3.SelectedValue = ds.Tables[1].Rows[0]["Auditory_3"].ToString();
                    SPchild_Auditory_3.Text = ds.Tables[1].Rows[0]["SPchild_Auditory_3"].ToString();
                    Visual_3.SelectedValue = ds.Tables[1].Rows[0]["Visual_3"].ToString();
                    SPchild_Visual_3.Text = ds.Tables[1].Rows[0]["SPchild_Visual_3"].ToString();
                    Touch_3.SelectedValue = ds.Tables[1].Rows[0]["Touch_3"].ToString();
                    SPchild_Touch_3.Text = ds.Tables[1].Rows[0]["SPchild_Touch_3"].ToString();
                    Movement_3.SelectedValue = ds.Tables[1].Rows[0]["Movement_3"].ToString();
                    SPchild_Movement_3.Text = ds.Tables[1].Rows[0]["SPchild_Movement_3"].ToString();
                    Body_position.SelectedValue = ds.Tables[1].Rows[0]["Body_position"].ToString();
                    SPchild_Body_position.Text = ds.Tables[1].Rows[0]["SPchild_Body_position"].ToString();
                    Oral_3.SelectedValue = ds.Tables[1].Rows[0]["Oral_3"].ToString();
                    SPchild_Oral_3.Text = ds.Tables[1].Rows[0]["SPchild_Oral_3"].ToString();
                    Conduct_3.SelectedValue = ds.Tables[1].Rows[0]["Conduct_3"].ToString();
                    SPchild_Conduct_3.Text = ds.Tables[1].Rows[0]["SPchild_Conduct_3"].ToString();
                    Social_emotional.SelectedValue = ds.Tables[1].Rows[0]["Social_emotional"].ToString();
                    SPchild_Social_emotional.Text = ds.Tables[1].Rows[0]["SPchild_Social_emotional"].ToString();
                    Attentional_3.SelectedValue = ds.Tables[1].Rows[0]["Attentional_3"].ToString();
                    SPchild_Attentional_3.Text = ds.Tables[1].Rows[0]["SPchild_Attentional_3"].ToString();


                    Comments_3.Text = ds.Tables[1].Rows[0]["Comments_3"].ToString();

                    SPAdult_Low_Registration.Text = ds.Tables[1].Rows[0]["SPAdult_Low_Registration"].ToString();
                    SPAdult_Sensory_seeking.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_seeking"].ToString();
                    SPAdult_Sensory_Sensitivity.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_Sensitivity"].ToString();
                    SPAdult_Sensory_Avoiding.Text = ds.Tables[1].Rows[0]["SPAdult_Sensory_Avoiding"].ToString();



                    Low_Registration.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration"].ToString();
                    SP_Low_Registration64.Text = ds.Tables[1].Rows[0]["SP_Low_Registration64"].ToString();
                    Sensory_seeking.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking"].ToString();
                    SP_Sensory_seeking_64.Text = ds.Tables[1].Rows[0]["SP_Sensory_seeking_64"].ToString();
                    Sensory_Sensitivity.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity"].ToString();
                    SP_Sensory_Sensitivity64.Text = ds.Tables[1].Rows[0]["SP_Sensory_Sensitivity64"].ToString();
                    Sensory_Avoiding.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding"].ToString();
                    SP_Sensory_Avoiding64.Text = ds.Tables[1].Rows[0]["SP_Sensory_Avoiding64"].ToString();
                    Comments_4.Text = ds.Tables[1].Rows[0]["Comments_4"].ToString();



                    Low_Registration_5.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_5"].ToString();
                    Sensory_seeking_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_5"].ToString();
                    Sensory_Sensitivity_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_5"].ToString();
                    Sensory_Avoiding_5.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_5"].ToString();
                    Comments_5.Text = ds.Tables[1].Rows[0]["Comments_5"].ToString();

                    Older_Low_Registration.Text = ds.Tables[1].Rows[0]["Older_Low_Registration"].ToString();
                    Older_Sensory_seeking.Text = ds.Tables[1].Rows[0]["Older_Sensory_seeking"].ToString();
                    Older_Sensory_Sensitivity.Text = ds.Tables[1].Rows[0]["Older_Sensory_Sensitivity"].ToString();
                    Older_Sensory_Avoiding.Text = ds.Tables[1].Rows[0]["Older_Sensory_Avoiding"].ToString();


                    Low_Registration_6.SelectedValue = ds.Tables[1].Rows[0]["Low_Registration_6"].ToString();
                    Sensory_seeking_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_seeking_6"].ToString();
                    Sensory_Sensitivity_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Sensitivity_6"].ToString();
                    Sensory_Avoiding_6.SelectedValue = ds.Tables[1].Rows[0]["Sensory_Avoiding_6"].ToString();
                    Comments_6.Text = ds.Tables[1].Rows[0]["Comments_6"].ToString();

                    score_Communication_2.Text = ds.Tables[1].Rows[0]["score_Communication_2"].ToString();
                    Inter_Communication_2.Text = ds.Tables[1].Rows[0]["Inter_Communication_2"].ToString();
                    GROSS_2.Text = ds.Tables[1].Rows[0]["GROSS_2"].ToString();
                    inter_Gross_2.Text = ds.Tables[1].Rows[0]["inter_Gross_2"].ToString();
                    FINE_2.Text = ds.Tables[1].Rows[0]["FINE_2"].ToString();
                    inter_FINE_2.Text = ds.Tables[1].Rows[0]["inter_FINE_2"].ToString();
                    PROBLEM_2.Text = ds.Tables[1].Rows[0]["PROBLEM_2"].ToString();
                    inter_PROBLEM_2.Text = ds.Tables[1].Rows[0]["inter_PROBLEM_2"].ToString();
                    PERSONAL_2.Text = ds.Tables[1].Rows[0]["PERSONAL_2"].ToString();
                    inter_PERSONAL_2.Text = ds.Tables[1].Rows[0]["inter_PERSONAL_2"].ToString();


                    TS_Registration.Text = ds.Tables[1].Rows[0]["TS_Registration"].ToString();
                    TS_Orientation.Text = ds.Tables[1].Rows[0]["TS_Orientation"].ToString();
                    TS_Discrimination.Text = ds.Tables[1].Rows[0]["TS_Discrimination"].ToString();
                    TS_Responsiveness.Text = ds.Tables[1].Rows[0]["TS_Responsiveness"].ToString();
                    SS_Bodyawareness.Text = ds.Tables[1].Rows[0]["SS_Bodyawareness"].ToString();
                    SS_Bodyschema.Text = ds.Tables[1].Rows[0]["SS_Bodyschema"].ToString();
                    SS_Orientation.Text = ds.Tables[1].Rows[0]["SS_Orientation"].ToString();
                    SS_Posterior.Text = ds.Tables[1].Rows[0]["SS_Posterior"].ToString();
                    SS_Bilateral.Text = ds.Tables[1].Rows[0]["SS_Bilateral"].ToString();
                    SS_Balance.Text = ds.Tables[1].Rows[0]["SS_Balance"].ToString();
                    SS_Dominance.Text = ds.Tables[1].Rows[0]["SS_Dominance"].ToString();
                    SS_Right.Text = ds.Tables[1].Rows[0]["SS_Right"].ToString();
                    SS_identifies.Text = ds.Tables[1].Rows[0]["SS_identifies"].ToString();
                    SS_point.Text = ds.Tables[1].Rows[0]["SS_point"].ToString();
                    SS_Constantly.Text = ds.Tables[1].Rows[0]["SS_Constantly"].ToString();
                    SS_clumsy.Text = ds.Tables[1].Rows[0]["SS_clumsy"].ToString();
                    SS_maneuver.Text = ds.Tables[1].Rows[0]["SS_maneuver"].ToString();
                    SS_overly.Text = ds.Tables[1].Rows[0]["SS_overly"].ToString();
                    SS_stand.Text = ds.Tables[1].Rows[0]["SS_stand"].ToString();
                    SS_indulge.Text = ds.Tables[1].Rows[0]["SS_indulge"].ToString();
                    SS_textures.Text = ds.Tables[1].Rows[0]["SS_textures"].ToString();
                    SS_monkey.Text = ds.Tables[1].Rows[0]["SS_monkey"].ToString();
                    SS_swings.Text = ds.Tables[1].Rows[0]["SS_swings"].ToString();
                    VM_Registration.Text = ds.Tables[1].Rows[0]["VM_Registration"].ToString();
                    VM_Orientation.Text = ds.Tables[1].Rows[0]["VM_Orientation"].ToString();
                    VM_Discrimination.Text = ds.Tables[1].Rows[0]["VM_Discrimination"].ToString();
                    VM_Responsiveness.Text = ds.Tables[1].Rows[0]["VM_Responsiveness"].ToString();
                    PS_Registration.Text = ds.Tables[1].Rows[0]["PS_Registration"].ToString();
                    PS_Gradation.Text = ds.Tables[1].Rows[0]["PS_Gradation"].ToString();
                    PS_Discrimination.Text = ds.Tables[1].Rows[0]["PS_Discrimination"].ToString();
                    PS_Responsiveness.Text = ds.Tables[1].Rows[0]["PS_Responsiveness"].ToString();
                    OM_Registration.Text = ds.Tables[1].Rows[0]["OM_Registration"].ToString();
                    OM_Orientation.Text = ds.Tables[1].Rows[0]["OM_Orientation"].ToString();
                    OM_Discrimination.Text = ds.Tables[1].Rows[0]["OM_Discrimination"].ToString();
                    OM_Responsiveness.Text = ds.Tables[1].Rows[0]["OM_Responsiveness"].ToString();
                    AS_Auditory.Text = ds.Tables[1].Rows[0]["AS_Auditory"].ToString();
                    AS_Orientation.Text = ds.Tables[1].Rows[0]["AS_Orientation"].ToString();
                    AS_Responsiveness.Text = ds.Tables[1].Rows[0]["AS_Responsiveness"].ToString();
                    AS_discrimination.Text = ds.Tables[1].Rows[0]["AS_discrimination"].ToString();
                    AS_Background.Text = ds.Tables[1].Rows[0]["AS_Background"].ToString();
                    AS_localization.Text = ds.Tables[1].Rows[0]["AS_localization"].ToString();
                    AS_Analysis.Text = ds.Tables[1].Rows[0]["AS_Analysis"].ToString();
                    AS_sequencing.Text = ds.Tables[1].Rows[0]["AS_sequencing"].ToString();
                    AS_blending.Text = ds.Tables[1].Rows[0]["AS_blending"].ToString();
                    VS_Visual.Text = ds.Tables[1].Rows[0]["VS_Visual"].ToString();
                    VS_Responsiveness.Text = ds.Tables[1].Rows[0]["VS_Responsiveness"].ToString();
                    VS_scanning.Text = ds.Tables[1].Rows[0]["VS_scanning"].ToString();
                    VS_constancy.Text = ds.Tables[1].Rows[0]["VS_constancy"].ToString();
                    VS_memory.Text = ds.Tables[1].Rows[0]["VS_memory"].ToString();
                    VS_Perception.Text = ds.Tables[1].Rows[0]["VS_Perception"].ToString();
                    VS_hand.Text = ds.Tables[1].Rows[0]["VS_hand"].ToString();
                    VS_foot.Text = ds.Tables[1].Rows[0]["VS_foot"].ToString();
                    VS_discrimination.Text = ds.Tables[1].Rows[0]["VS_discrimination"].ToString();
                    VS_closure.Text = ds.Tables[1].Rows[0]["VS_closure"].ToString();
                    VS_Figureground.Text = ds.Tables[1].Rows[0]["VS_Figureground"].ToString();
                    VS_Visualmemory.Text = ds.Tables[1].Rows[0]["VS_Visualmemory"].ToString();
                    VS_sequential.Text = ds.Tables[1].Rows[0]["VS_sequential"].ToString();
                    VS_spatial.Text = ds.Tables[1].Rows[0]["VS_spatial"].ToString();
                    OS_Registration.Text = ds.Tables[1].Rows[0]["OS_Registration"].ToString();
                    OS_Orientation.Text = ds.Tables[1].Rows[0]["OS_Orientation"].ToString();
                    OS_Discrimination.Text = ds.Tables[1].Rows[0]["OS_Discrimination"].ToString();
                    OS_Responsiveness.Text = ds.Tables[1].Rows[0]["OS_Responsiveness"].ToString();

                    SIPTInfo_History.Text = ds.Tables[1].Rows[0]["SIPTInfo_History"].ToString();
                    SIPTInfo_HandFunction1_GraspRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspRight"].ToString();
                    SIPTInfo_HandFunction1_GraspLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GraspLeft"].ToString();
                    SIPTInfo_HandFunction1_SphericalRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalRight"].ToString();
                    SIPTInfo_HandFunction1_SphericalLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_SphericalLeft"].ToString();
                    SIPTInfo_HandFunction1_HookRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookRight"].ToString();
                    SIPTInfo_HandFunction1_HookLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_HookLeft"].ToString();
                    SIPTInfo_HandFunction1_JawChuckRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckRight"].ToString();
                    SIPTInfo_HandFunction1_JawChuckLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_JawChuckLeft"].ToString();
                    SIPTInfo_HandFunction1_GripRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripRight"].ToString();
                    SIPTInfo_HandFunction1_GripLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_GripLeft"].ToString();
                    SIPTInfo_HandFunction1_ReleaseRight.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseRight"].ToString();
                    SIPTInfo_HandFunction1_ReleaseLeft.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction1_ReleaseLeft"].ToString();
                    SIPTInfo_HandFunction2_OppositionLfR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfR"].ToString();
                    SIPTInfo_HandFunction2_OppositionLfL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionLfL"].ToString();
                    SIPTInfo_HandFunction2_OppositionMFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFR"].ToString();
                    SIPTInfo_HandFunction2_OppositionMFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionMFL"].ToString();
                    SIPTInfo_HandFunction2_OppositionRFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFR"].ToString();
                    SIPTInfo_HandFunction2_OppositionRFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_OppositionRFL"].ToString();
                    SIPTInfo_HandFunction2_PinchLfR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfR"].ToString();
                    SIPTInfo_HandFunction2_PinchLfL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchLfL"].ToString();
                    SIPTInfo_HandFunction2_PinchMFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFR"].ToString();
                    SIPTInfo_HandFunction2_PinchMFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchMFL"].ToString();
                    SIPTInfo_HandFunction2_PinchRFR.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFR"].ToString();
                    SIPTInfo_HandFunction2_PinchRFL.Text = ds.Tables[1].Rows[0]["SIPTInfo_HandFunction2_PinchRFL"].ToString();
                    SIPTInfo_SIPT3_Spontaneous.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Spontaneous"].ToString();
                    SIPTInfo_SIPT3_Command.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT3_Command"].ToString();
                    SIPTInfo_SIPT4_Kinesthesia.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Kinesthesia"].ToString();
                    SIPTInfo_SIPT4_Finger.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Finger"].ToString();
                    SIPTInfo_SIPT4_Localisation.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Localisation"].ToString();
                    SIPTInfo_SIPT4_DoubleTactile.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_DoubleTactile"].ToString();
                    SIPTInfo_SIPT4_Tactile.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Tactile"].ToString();
                    SIPTInfo_SIPT4_Graphesthesia.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Graphesthesia"].ToString();
                    SIPTInfo_SIPT4_PostRotary.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_PostRotary"].ToString();
                    SIPTInfo_SIPT4_Standing.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT4_Standing"].ToString();
                    SIPTInfo_SIPT5_Color.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Color"].ToString();
                    SIPTInfo_SIPT5_Form.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Form"].ToString();
                    SIPTInfo_SIPT5_Size.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Size"].ToString();
                    SIPTInfo_SIPT5_Depth.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Depth"].ToString();
                    SIPTInfo_SIPT5_Figure.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Figure"].ToString();
                    SIPTInfo_SIPT5_Motor.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT5_Motor"].ToString();
                    SIPTInfo_SIPT6_Design.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Design"].ToString();
                    SIPTInfo_SIPT6_Constructional.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT6_Constructional"].ToString();
                    SIPTInfo_SIPT7_Scanning.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Scanning"].ToString();
                    SIPTInfo_SIPT7_Memory.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT7_Memory"].ToString();
                    SIPTInfo_SIPT8_Postural.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Postural"].ToString();
                    SIPTInfo_SIPT8_Oral.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Oral"].ToString();
                    SIPTInfo_SIPT8_Sequencing.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Sequencing"].ToString();
                    SIPTInfo_SIPT8_Commands.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT8_Commands"].ToString();
                    SIPTInfo_SIPT9_Bilateral.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Bilateral"].ToString();
                    SIPTInfo_SIPT9_Contralat.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_Contralat"].ToString();
                    SIPTInfo_SIPT9_PreferredHand.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_PreferredHand"].ToString();
                    SIPTInfo_SIPT9_CrossingMidline.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT9_CrossingMidline"].ToString();
                    SIPTInfo_SIPT10_Draw.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Draw"].ToString();
                    SIPTInfo_SIPT10_ClockFace.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_ClockFace"].ToString();
                    SIPTInfo_SIPT10_Filtering.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Filtering"].ToString();
                    SIPTInfo_SIPT10_MotorPlanning.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_MotorPlanning"].ToString();
                    SIPTInfo_SIPT10_BodyImage.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodyImage"].ToString();
                    SIPTInfo_SIPT10_BodySchema.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_BodySchema"].ToString();
                    SIPTInfo_SIPT10_Laterality.Text = ds.Tables[1].Rows[0]["SIPTInfo_SIPT10_Laterality"].ToString();
                    SIPTInfo_ActivityGiven_Remark.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Remark"].ToString();
                    SIPTInfo_ActivityGiven_InterestActivity.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestActivity"].ToString();
                    SIPTInfo_ActivityGiven_InterestCompletion.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_InterestCompletion"].ToString();
                    SIPTInfo_ActivityGiven_Learning.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Learning"].ToString();
                    SIPTInfo_ActivityGiven_Complexity.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Complexity"].ToString();
                    SIPTInfo_ActivityGiven_ProblemSolving.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_ProblemSolving"].ToString();
                    SIPTInfo_ActivityGiven_Concentration.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Concentration"].ToString();
                    SIPTInfo_ActivityGiven_Retension.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Retension"].ToString();
                    SIPTInfo_ActivityGiven_SpeedPerfom.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SpeedPerfom"].ToString();
                    SIPTInfo_ActivityGiven_Neatness.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Neatness"].ToString();
                    SIPTInfo_ActivityGiven_Frustation.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Frustation"].ToString();
                    SIPTInfo_ActivityGiven_Work.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Work"].ToString();
                    SIPTInfo_ActivityGiven_Reaction.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_Reaction"].ToString();
                    SIPTInfo_ActivityGiven_SociabilityTherapist.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityTherapist"].ToString();
                    SIPTInfo_ActivityGiven_SociabilityStudents.Text = ds.Tables[1].Rows[0]["SIPTInfo_ActivityGiven_SociabilityStudents"].ToString();

                    Evaluation_Strengths.Text = ds.Tables[1].Rows[0]["Evaluation_Strengths"].ToString();
                    Evaluation_Concern_Barriers.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Barriers"].ToString();
                    Evaluation_Concern_Limitations.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Limitations"].ToString();
                    Evaluation_Concern_Posture.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Posture"].ToString();
                    Evaluation_Concern_Impairment.Text = ds.Tables[1].Rows[0]["Evaluation_Concern_Impairment"].ToString();
                    Evaluation_Goal_Summary.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Summary"].ToString();
                    Evaluation_Goal_Previous.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Previous"].ToString();
                    Evaluation_Goal_LongTerm.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_LongTerm"].ToString();
                    Evaluation_Goal_ShortTerm.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_ShortTerm"].ToString();
                    Evaluation_Goal_Impairment.Text = ds.Tables[1].Rows[0]["Evaluation_Goal_Impairment"].ToString();
                    Evaluation_Plan_Frequency.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Frequency"].ToString();
                    Evaluation_Plan_Service.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Service"].ToString();
                    Evaluation_Plan_Strategies.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Strategies"].ToString();
                    Evaluation_Plan_Equipment.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Equipment"].ToString();
                    Evaluation_Plan_Education.Text = ds.Tables[1].Rows[0]["Evaluation_Plan_Education"].ToString();


                    DCDQ_Throws1.Text = ds.Tables[1].Rows[0]["DCDQ_Throws1"].ToString();
                    DCDQ_Throws2.Text = ds.Tables[1].Rows[0]["DCDQ_Throws3"].ToString();
                    DCDQ_Throws3.Text = ds.Tables[1].Rows[0]["DCDQ_Throws2"].ToString();
                    DCDQ_Catches1.Text = ds.Tables[1].Rows[0]["DCDQ_Catches1"].ToString();
                    DCDQ_Catches2.Text = ds.Tables[1].Rows[0]["DCDQ_Catches2"].ToString();
                    DCDQ_Catches3.Text = ds.Tables[1].Rows[0]["DCDQ_Catches3"].ToString();
                    DCDQ_Hits1.Text = ds.Tables[1].Rows[0]["DCDQ_Hits1"].ToString();
                    DCDQ_Hits2.Text = ds.Tables[1].Rows[0]["DCDQ_Hits2"].ToString();
                    DCDQ_Hits3.Text = ds.Tables[1].Rows[0]["DCDQ_Hits3"].ToString();
                    DCDQ_Jumps1.Text = ds.Tables[1].Rows[0]["DCDQ_Jumps1"].ToString();
                    DCDQ_Jumps2.Text = ds.Tables[1].Rows[0]["DCDQ_Jumps2"].ToString();
                    DCDQ_Jumps2.Text = ds.Tables[1].Rows[0]["DCDQ_Jumps3"].ToString();
                    DCDQ_Runs1.Text = ds.Tables[1].Rows[0]["DCDQ_Runs1"].ToString();
                    DCDQ_Runs2.Text = ds.Tables[1].Rows[0]["DCDQ_Runs2"].ToString();
                    DCDQ_Runs3.Text = ds.Tables[1].Rows[0]["DCDQ_Runs3"].ToString();
                    DCDQ_Plans1.Text = ds.Tables[1].Rows[0]["DCDQ_Plans1"].ToString();
                    DCDQ_Plans2.Text = ds.Tables[1].Rows[0]["DCDQ_Plans2"].ToString();
                    DCDQ_Plans3.Text = ds.Tables[1].Rows[0]["DCDQ_Plans3"].ToString();
                    DCDQ_Writing1.Text = ds.Tables[1].Rows[0]["DCDQ_Writing1"].ToString();
                    DCDQ_Writing2.Text = ds.Tables[1].Rows[0]["DCDQ_Writing2"].ToString();
                    DCDQ_Writing3.Text = ds.Tables[1].Rows[0]["DCDQ_Writing3"].ToString();
                    DCDQ_legibly1.Text = ds.Tables[1].Rows[0]["DCDQ_legibly1"].ToString();
                    DCDQ_legibly2.Text = ds.Tables[1].Rows[0]["DCDQ_legibly2"].ToString();
                    DCDQ_legibly3.Text = ds.Tables[1].Rows[0]["DCDQ_legibly3"].ToString();
                    DCDQ_Effort1.Text = ds.Tables[1].Rows[0]["DCDQ_Effort1"].ToString();
                    DCDQ_Effort2.Text = ds.Tables[1].Rows[0]["DCDQ_Effort2"].ToString();
                    DCDQ_Effort3.Text = ds.Tables[1].Rows[0]["DCDQ_Effort3"].ToString();
                    DCDQ_Cuts1.Text = ds.Tables[1].Rows[0]["DCDQ_Cuts1"].ToString();
                    DCDQ_Cuts2.Text = ds.Tables[1].Rows[0]["DCDQ_Cuts2"].ToString();
                    DCDQ_Cuts3.Text = ds.Tables[1].Rows[0]["DCDQ_Cuts3"].ToString();
                    DCDQ_Likes1.Text = ds.Tables[1].Rows[0]["DCDQ_Likes1"].ToString();
                    DCDQ_Likes2.Text = ds.Tables[1].Rows[0]["DCDQ_Likes2"].ToString();
                    DCDQ_Likes3.Text = ds.Tables[1].Rows[0]["DCDQ_Likes3"].ToString();
                    DCDQ_Learning1.Text = ds.Tables[1].Rows[0]["DCDQ_Learning1"].ToString();
                    DCDQ_Learning2.Text = ds.Tables[1].Rows[0]["DCDQ_Learning2"].ToString();
                    DCDQ_Learning3.Text = ds.Tables[1].Rows[0]["DCDQ_Learning3"].ToString();
                    DCDQ_Quick1.Text = ds.Tables[1].Rows[0]["DCDQ_Quick1"].ToString();
                    DCDQ_Quick2.Text = ds.Tables[1].Rows[0]["DCDQ_Quick2"].ToString();
                    DCDQ_Quick3.Text = ds.Tables[1].Rows[0]["DCDQ_Quick3"].ToString();
                    DCDQ_Bull1.Text = ds.Tables[1].Rows[0]["DCDQ_Bull1"].ToString();
                    DCDQ_Bull2.Text = ds.Tables[1].Rows[0]["DCDQ_Bull2"].ToString();
                    DCDQ_Bull3.Text = ds.Tables[1].Rows[0]["DCDQ_Bull3"].ToString();
                    DCDQ_Does1.Text = ds.Tables[1].Rows[0]["DCDQ_Does1"].ToString();
                    DCDQ_Does2.Text = ds.Tables[1].Rows[0]["DCDQ_Does2"].ToString();
                    DCDQ_Does3.Text = ds.Tables[1].Rows[0]["DCDQ_Does3"].ToString();
                    DCDQ_Control.Text = ds.Tables[1].Rows[0]["DCDQ_Control"].ToString();
                    DCDQ_Fine.Text = ds.Tables[1].Rows[0]["DCDQ_Fine"].ToString();
                    DCDQ_General.Text = ds.Tables[1].Rows[0]["DCDQ_General"].ToString();
                    DCDQ_Total.Text = ds.Tables[1].Rows[0]["DCDQ_Total"].ToString();
                    DCDQ_INTERPRETATION.Text = ds.Tables[1].Rows[0]["DCDQ_INTERPRETATION"].ToString();
                    DCDQ_COMMENT.Text = ds.Tables[1].Rows[0]["DCDQ_COMMENT"].ToString();



                    //score_Communication_2months.Text = ds.Tables[1].Rows[0]["score_Communication_2months"].ToString();
                    //Inter_Communication_2months.Text = ds.Tables[1].Rows[0]["Inter_Communication_2months"].ToString();
                    //GROSS_2months.Text = ds.Tables[1].Rows[0]["GROSS_2months"].ToString();
                    //inter_Gross_2months.Text = ds.Tables[1].Rows[0]["inter_Gross_2months"].ToString();
                    //FINE_2months.Text = ds.Tables[1].Rows[0]["FINE_2months"].ToString();
                    //inter_FINE_2months.Text = ds.Tables[1].Rows[0]["inter_FINE_2months"].ToString();
                    //PROBLEM_2months.Text = ds.Tables[1].Rows[0]["PROBLEM_2months"].ToString();
                    //inter_PROBLEM_2moths.Text = ds.Tables[1].Rows[0]["inter_PROBLEM_2moths"].ToString();
                    //PERSONAL_2months.Text = ds.Tables[1].Rows[0]["PERSONAL_2months"].ToString();
                    //inter_PERSONAL_2months.Text = ds.Tables[1].Rows[0]["inter_PERSONAL_2months"].ToString();
                    Comm_3.Text = ds.Tables[1].Rows[0]["Comm_3"].ToString();
                    inter_3.Text = ds.Tables[1].Rows[0]["inter_3"].ToString();
                    GROSS_3.Text = ds.Tables[1].Rows[0]["GROSS_3"].ToString();
                    GROSS_inter_3.Text = ds.Tables[1].Rows[0]["GROSS_inter_3"].ToString();
                    FINE_3.Text = ds.Tables[1].Rows[0]["FINE_3"].ToString();
                    PROBLEM_3.Text = ds.Tables[1].Rows[0]["FINE_inter_3"].ToString();
                    FINE_inter_3.Text = ds.Tables[1].Rows[0]["PROBLEM_3"].ToString();
                    PROBLEM_inter_3.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_3"].ToString();
                    PERSONAL_3.Text = ds.Tables[1].Rows[0]["PERSONAL_3"].ToString();
                    PERSONAL_inter_3.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_3"].ToString();
                    Communication_6.Text = ds.Tables[1].Rows[0]["Communication_6"].ToString();
                    comm_inter_6.Text = ds.Tables[1].Rows[0]["comm_inter_6"].ToString();
                    GROSS_6.Text = ds.Tables[1].Rows[0]["GROSS_6"].ToString();
                    GROSS_inter_6.Text = ds.Tables[1].Rows[0]["GROSS_inter_6"].ToString();
                    FINE_6.Text = ds.Tables[1].Rows[0]["FINE_6"].ToString();
                    FINE_inter_6.Text = ds.Tables[1].Rows[0]["FINE_inter_6"].ToString();
                    PROBLEM_6.Text = ds.Tables[1].Rows[0]["PROBLEM_6"].ToString();
                    PROBLEM_inter_6.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_6"].ToString();
                    PERSONAL_6.Text = ds.Tables[1].Rows[0]["PERSONAL_6"].ToString();
                    PERSONAL_inter_6.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_6"].ToString();
                    comm_7.Text = ds.Tables[1].Rows[0]["comm_7"].ToString();
                    inter_7.Text = ds.Tables[1].Rows[0]["inter_7"].ToString();
                    GROSS_7.Text = ds.Tables[1].Rows[0]["GROSS_7"].ToString();
                    GROSS_inter_7.Text = ds.Tables[1].Rows[0]["GROSS_inter_7"].ToString();
                    FINE_7.Text = ds.Tables[1].Rows[0]["FINE_7"].ToString();
                    FINE_inter_7.Text = ds.Tables[1].Rows[0]["FINE_inter_7"].ToString();
                    PROBLEM_7.Text = ds.Tables[1].Rows[0]["PROBLEM_7"].ToString();
                    PROBLEM_inter_7.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_7"].ToString();
                    PERSONAL_7.Text = ds.Tables[1].Rows[0]["PERSONAL_7"].ToString();
                    PERSONAL_inter_7.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_7"].ToString();
                    comm_9.Text = ds.Tables[1].Rows[0]["comm_9"].ToString();
                    inter_9.Text = ds.Tables[1].Rows[0]["inter_9"].ToString();
                    GROSS_9.Text = ds.Tables[1].Rows[0]["GROSS_9"].ToString();
                    GROSS_inter_9.Text = ds.Tables[1].Rows[0]["GROSS_inter_9"].ToString();
                    FINE_9.Text = ds.Tables[1].Rows[0]["FINE_9"].ToString();
                    FINE_inter_9.Text = ds.Tables[1].Rows[0]["FINE_inter_9"].ToString();
                    PROBLEM_9.Text = ds.Tables[1].Rows[0]["PROBLEM_9"].ToString();
                    PROBLEM_inter_9.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_9"].ToString();
                    PERSONAL_9.Text = ds.Tables[1].Rows[0]["PERSONAL_9"].ToString();
                    PERSONAL_inter_9.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_9"].ToString();
                    comm_10.Text = ds.Tables[1].Rows[0]["comm_10"].ToString();
                    inter_10.Text = ds.Tables[1].Rows[0]["inter_10"].ToString();
                    GROSS_10.Text = ds.Tables[1].Rows[0]["GROSS_10"].ToString();
                    GROSS_inter_10.Text = ds.Tables[1].Rows[0]["GROSS_inter_10"].ToString();
                    FINE_10.Text = ds.Tables[1].Rows[0]["FINE_10"].ToString();
                    FINE_inter_10.Text = ds.Tables[1].Rows[0]["FINE_inter_10"].ToString();
                    PROBLEM_10.Text = ds.Tables[1].Rows[0]["PROBLEM_10"].ToString();
                    PROBLEM_inter_10.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_10"].ToString();
                    PERSONAL_10.Text = ds.Tables[1].Rows[0]["PERSONAL_10"].ToString();
                    PERSONAL_inter_10.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_10"].ToString();
                    comm_11.Text = ds.Tables[1].Rows[0]["comm_11"].ToString();
                    inter_11.Text = ds.Tables[1].Rows[0]["inter_11"].ToString();
                    GROSS_11.Text = ds.Tables[1].Rows[0]["GROSS_11"].ToString();
                    GROSS_inter_11.Text = ds.Tables[1].Rows[0]["GROSS_inter_11"].ToString();
                    FINE_11.Text = ds.Tables[1].Rows[0]["FINE_11"].ToString();
                    FINE_inter_11.Text = ds.Tables[1].Rows[0]["FINE_inter_11"].ToString();
                    PROBLEM_11.Text = ds.Tables[1].Rows[0]["PROBLEM_11"].ToString();
                    PROBLEM_inter_11.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_11"].ToString();
                    PERSONAL_11.Text = ds.Tables[1].Rows[0]["PERSONAL_11"].ToString();
                    PERSONAL_inter_11.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_11"].ToString();
                    comm_13.Text = ds.Tables[1].Rows[0]["comm_13"].ToString();
                    inter_13.Text = ds.Tables[1].Rows[0]["inter_13"].ToString();
                    GROSS_13.Text = ds.Tables[1].Rows[0]["GROSS_13"].ToString();
                    GROSS_inter_13.Text = ds.Tables[1].Rows[0]["GROSS_inter_13"].ToString();
                    FINE_13.Text = ds.Tables[1].Rows[0]["FINE_13"].ToString();
                    FINE_inter_13.Text = ds.Tables[1].Rows[0]["FINE_inter_13"].ToString();
                    PROBLEM_13.Text = ds.Tables[1].Rows[0]["PROBLEM_13"].ToString();
                    PROBLEM_inter_13.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_13"].ToString();
                    PERSONAL_13.Text = ds.Tables[1].Rows[0]["PERSONAL_13"].ToString();
                    PERSONAL_inter_13.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_13"].ToString();
                    comm_15.Text = ds.Tables[1].Rows[0]["comm_15"].ToString();
                    inter_15.Text = ds.Tables[1].Rows[0]["inter_15"].ToString();
                    GROSS_15.Text = ds.Tables[1].Rows[0]["GROSS_15"].ToString();
                    GROSS_inter_15.Text = ds.Tables[1].Rows[0]["GROSS_inter_15"].ToString();
                    FINE_15.Text = ds.Tables[1].Rows[0]["FINE_15"].ToString();
                    FINE_inter_15.Text = ds.Tables[1].Rows[0]["FINE_inter_15"].ToString();
                    PROBLEM_15.Text = ds.Tables[1].Rows[0]["PROBLEM_15"].ToString();
                    PROBLEM_inter_15.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_15"].ToString();
                    PERSONAL_15.Text = ds.Tables[1].Rows[0]["PERSONAL_15"].ToString();
                    PERSONAL_inter_15.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_15"].ToString();
                    comm_17.Text = ds.Tables[1].Rows[0]["comm_17"].ToString();
                    inter_17.Text = ds.Tables[1].Rows[0]["inter_17"].ToString();
                    GROSS_17.Text = ds.Tables[1].Rows[0]["GROSS_17"].ToString();
                    GROSS_inter_17.Text = ds.Tables[1].Rows[0]["GROSS_inter_17"].ToString();
                    FINE_17.Text = ds.Tables[1].Rows[0]["FINE_17"].ToString();
                    FINE_inter_17.Text = ds.Tables[1].Rows[0]["FINE_inter_17"].ToString();
                    PROBLEM_17.Text = ds.Tables[1].Rows[0]["PROBLEM_17"].ToString();
                    PROBLEM_inter_17.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_17"].ToString();
                    PERSONAL_17.Text = ds.Tables[1].Rows[0]["PERSONAL_17"].ToString();
                    PERSONAL_inter_17.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_17"].ToString();
                    comm_19.Text = ds.Tables[1].Rows[0]["comm_19"].ToString();
                    inter_19.Text = ds.Tables[1].Rows[0]["inter_19"].ToString();
                    GROSS_19.Text = ds.Tables[1].Rows[0]["GROSS_19"].ToString();
                    GROSS_inter_19.Text = ds.Tables[1].Rows[0]["GROSS_inter_19"].ToString();
                    FINE_19.Text = ds.Tables[1].Rows[0]["FINE_19"].ToString();
                    FINE_inter_19.Text = ds.Tables[1].Rows[0]["FINE_inter_19"].ToString();
                    PROBLEM_19.Text = ds.Tables[1].Rows[0]["PROBLEM_19"].ToString();
                    PROBLEM_inter_19.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_19"].ToString();
                    PERSONAL_19.Text = ds.Tables[1].Rows[0]["PERSONAL_19"].ToString();
                    PERSONAL_inter_19.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_19"].ToString();
                    comm_21.Text = ds.Tables[1].Rows[0]["comm_21"].ToString();
                    inter_21.Text = ds.Tables[1].Rows[0]["inter_21"].ToString();
                    GROSS_21.Text = ds.Tables[1].Rows[0]["GROSS_21"].ToString();
                    GROSS_inter_21.Text = ds.Tables[1].Rows[0]["GROSS_inter_21"].ToString();
                    FINE_21.Text = ds.Tables[1].Rows[0]["FINE_21"].ToString();
                    FINE_inter_21.Text = ds.Tables[1].Rows[0]["FINE_inter_21"].ToString();
                    PROBLEM_21.Text = ds.Tables[1].Rows[0]["PROBLEM_21"].ToString();
                    PROBLEM_inter_21.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_21"].ToString();
                    PERSONAL_21.Text = ds.Tables[1].Rows[0]["PERSONAL_21"].ToString();
                    PERSONAL_inter_21.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_21"].ToString();
                    comm_23.Text = ds.Tables[1].Rows[0]["comm_23"].ToString();
                    inter_23.Text = ds.Tables[1].Rows[0]["inter_23"].ToString();
                    GROSS_23.Text = ds.Tables[1].Rows[0]["GROSS_23"].ToString();
                    GROSS_inter_23.Text = ds.Tables[1].Rows[0]["GROSS_inter_23"].ToString();
                    FINE_23.Text = ds.Tables[1].Rows[0]["FINE_23"].ToString();
                    FINE_inter_23.Text = ds.Tables[1].Rows[0]["FINE_inter_23"].ToString();
                    PROBLEM_23.Text = ds.Tables[1].Rows[0]["PROBLEM_23"].ToString();
                    PROBLEM_inter_23.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_23"].ToString();
                    PERSONAL_23.Text = ds.Tables[1].Rows[0]["PERSONAL_23"].ToString();
                    PERSONAL_inter_23.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_23"].ToString();
                    comm_25.Text = ds.Tables[1].Rows[0]["comm_25"].ToString();
                    inter_25.Text = ds.Tables[1].Rows[0]["inter_25"].ToString();
                    GROSS_25.Text = ds.Tables[1].Rows[0]["GROSS_25"].ToString();
                    GROSS_inter_25.Text = ds.Tables[1].Rows[0]["GROSS_inter_25"].ToString();
                    FINE_25.Text = ds.Tables[1].Rows[0]["FINE_25"].ToString();
                    FINE_inter_25.Text = ds.Tables[1].Rows[0]["FINE_inter_25"].ToString();
                    PROBLEM_25.Text = ds.Tables[1].Rows[0]["PROBLEM_25"].ToString();
                    PROBLEM_inter_25.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_25"].ToString();
                    PERSONAL_25.Text = ds.Tables[1].Rows[0]["PERSONAL_25"].ToString();
                    PERSONAL_inter_25.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_25"].ToString();
                    comm_28.Text = ds.Tables[1].Rows[0]["comm_28"].ToString();
                    inter_28.Text = ds.Tables[1].Rows[0]["inter_28"].ToString();
                    GROSS_28.Text = ds.Tables[1].Rows[0]["GROSS_28"].ToString();
                    GROSS_inter_28.Text = ds.Tables[1].Rows[0]["GROSS_inter_28"].ToString();
                    FINE_28.Text = ds.Tables[1].Rows[0]["FINE_28"].ToString();
                    FINE_inter_28.Text = ds.Tables[1].Rows[0]["FINE_inter_28"].ToString();
                    PROBLEM_28.Text = ds.Tables[1].Rows[0]["PROBLEM_28"].ToString();
                    PROBLEM_inter_28.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_28"].ToString();
                    PERSONAL_28.Text = ds.Tables[1].Rows[0]["PERSONAL_28"].ToString();
                    PERSONAL_inter_28.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_28"].ToString();
                    comm_31.Text = ds.Tables[1].Rows[0]["comm_31"].ToString();
                    inter_31.Text = ds.Tables[1].Rows[0]["inter_31"].ToString();
                    GROSS_31.Text = ds.Tables[1].Rows[0]["GROSS_31"].ToString();
                    GROSS_inter_31.Text = ds.Tables[1].Rows[0]["GROSS_inter_31"].ToString();
                    FINE_31.Text = ds.Tables[1].Rows[0]["FINE_31"].ToString();
                    FINE_inter_31.Text = ds.Tables[1].Rows[0]["FINE_inter_31"].ToString();
                    PROBLEM_31.Text = ds.Tables[1].Rows[0]["PROBLEM_31"].ToString();
                    PROBLEM_inter_31.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_31"].ToString();
                    PERSONAL_31.Text = ds.Tables[1].Rows[0]["PERSONAL_31"].ToString();
                    PERSONAL_inter_31.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_31"].ToString();
                    comm_34.Text = ds.Tables[1].Rows[0]["comm_34"].ToString();
                    inter_34.Text = ds.Tables[1].Rows[0]["inter_34"].ToString();
                    GROSS_34.Text = ds.Tables[1].Rows[0]["GROSS_34"].ToString();
                    GROSS_inter_34.Text = ds.Tables[1].Rows[0]["GROSS_inter_34"].ToString();
                    FINE_34.Text = ds.Tables[1].Rows[0]["FINE_34"].ToString();
                    FINE_inter_34.Text = ds.Tables[1].Rows[0]["FINE_inter_34"].ToString();
                    PROBLEM_34.Text = ds.Tables[1].Rows[0]["PROBLEM_34"].ToString();
                    PROBLEM_inter_34.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_34"].ToString();
                    PERSONAL_34.Text = ds.Tables[1].Rows[0]["PERSONAL_34"].ToString();
                    PERSONAL_inter_34.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_34"].ToString();
                    comm_42.Text = ds.Tables[1].Rows[0]["comm_42"].ToString();
                    inter_42.Text = ds.Tables[1].Rows[0]["inter_42"].ToString();
                    GROSS_42.Text = ds.Tables[1].Rows[0]["GROSS_42"].ToString();
                    GROSS_inter_42.Text = ds.Tables[1].Rows[0]["GROSS_inter_42"].ToString();
                    FINE_42.Text = ds.Tables[1].Rows[0]["FINE_42"].ToString();
                    FINE_inter_42.Text = ds.Tables[1].Rows[0]["FINE_inter_42"].ToString();
                    PROBLEM_42.Text = ds.Tables[1].Rows[0]["PROBLEM_42"].ToString();
                    PROBLEM_inter_42.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_42"].ToString();
                    PERSONAL_42.Text = ds.Tables[1].Rows[0]["PERSONAL_42"].ToString();
                    PERSONAL_inter_42.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_42"].ToString();
                    comm_45.Text = ds.Tables[1].Rows[0]["comm_45"].ToString();
                    inter_45.Text = ds.Tables[1].Rows[0]["inter_45"].ToString();
                    GROSS_45.Text = ds.Tables[1].Rows[0]["GROSS_45"].ToString();
                    GROSS_inter_45.Text = ds.Tables[1].Rows[0]["GROSS_inter_45"].ToString();
                    FINE_45.Text = ds.Tables[1].Rows[0]["FINE_45"].ToString();
                    FINE_inter_45.Text = ds.Tables[1].Rows[0]["FINE_inter_45"].ToString();
                    PROBLEM_45.Text = ds.Tables[1].Rows[0]["PROBLEM_45"].ToString();
                    PROBLEM_inter_45.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_45"].ToString();
                    PERSONAL_45.Text = ds.Tables[1].Rows[0]["PERSONAL_45"].ToString();
                    PERSONAL_inter_45.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_45"].ToString();
                    comm_51.Text = ds.Tables[1].Rows[0]["comm_51"].ToString();
                    inter_51.Text = ds.Tables[1].Rows[0]["inter_51"].ToString();
                    GROSS_51.Text = ds.Tables[1].Rows[0]["GROSS_51"].ToString();
                    GROSS_51.Text = ds.Tables[1].Rows[0]["GROSS_inter_51"].ToString();
                    FINE_51.Text = ds.Tables[1].Rows[0]["FINE_51"].ToString();
                    FINE_inter_51.Text = ds.Tables[1].Rows[0]["FINE_inter_51"].ToString();
                    PROBLEM_51.Text = ds.Tables[1].Rows[0]["PROBLEM_51"].ToString();
                    PROBLEM_inter_51.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_51"].ToString();
                    PERSONAL_51.Text = ds.Tables[1].Rows[0]["PERSONAL_51"].ToString();
                    PERSONAL_inter_51.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_51"].ToString();
                    comm_60.Text = ds.Tables[1].Rows[0]["comm_60"].ToString();
                    inter_60.Text = ds.Tables[1].Rows[0]["inter_60"].ToString();
                    GROSS_60.Text = ds.Tables[1].Rows[0]["GROSS_60"].ToString();
                    GROSS_inter_60.Text = ds.Tables[1].Rows[0]["GROSS_inter_60"].ToString();
                    FINE_60.Text = ds.Tables[1].Rows[0]["FINE_60"].ToString();
                    FINE_inter_60.Text = ds.Tables[1].Rows[0]["FINE_inter_60"].ToString();
                    PROBLEM_60.Text = ds.Tables[1].Rows[0]["PROBLEM_60"].ToString();
                    PROBLEM_inter_60.Text = ds.Tables[1].Rows[0]["PROBLEM_inter_60"].ToString();
                    PERSONAL_60.Text = ds.Tables[1].Rows[0]["PERSONAL_60"].ToString();
                    PERSONAL_inter_60.Text = ds.Tables[1].Rows[0]["PERSONAL_inter_60"].ToString();



                    int Physioptherapist = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Physioptherapist"].ToString(), out Physioptherapist);
                    if (Doctor_Physioptherapist.Items.FindByValue(Physioptherapist.ToString()) != null)
                    {
                        Doctor_Physioptherapist.SelectedValue = Physioptherapist.ToString();
                    }
                    int Occupational = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Occupational"].ToString(), out Occupational);
                    if (Doctor_Occupational.Items.FindByValue(Occupational.ToString()) != null)
                    {
                        Doctor_Occupational.SelectedValue = Occupational.ToString();
                    }

                    Session["AgeState"] = ds.Tables[1];
                    Session["Ability"] = ds.Tables[1];



                    List<optionMdel> qls = new List<optionMdel>();
                    //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //{

                    //    string tIME = string.Empty; string aCTIVITIES = string.Empty; string cOMMENTS = string.Empty;


                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TIME"].ToString()))
                    //    {
                    //        tIME = ds.Tables[1].Rows[i]["TIME"].ToString();
                    //    }
                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["ACTIVITIES"].ToString()))
                    //    {
                    //        aCTIVITIES = ds.Tables[1].Rows[i]["ACTIVITIES"].ToString();
                    //    }
                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["COMMENTS"].ToString()))
                    //    {
                    //        cOMMENTS = ds.Tables[1].Rows[i]["COMMENTS"].ToString();
                    //    }


                    //    qls.Add(new optionMdel
                    //    {
                    //        Option = ds.Tables[1].Rows[i]["SI_ID"].ToString(),
                    //        Option1 = tIME,
                    //        Option2 = aCTIVITIES,
                    //        Option3 = cOMMENTS,

                    //    });
                    //}


                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {

                        string tIME = string.Empty; string aCTIVITIES = string.Empty; string cOMMENTS = string.Empty;


                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["TIME"].ToString()))
                        {
                            tIME = ds.Tables[7].Rows[i]["TIME"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["ACTIVITIES"].ToString()))
                        {
                            aCTIVITIES = ds.Tables[7].Rows[i]["ACTIVITIES"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["COMMENTS"].ToString()))
                        {
                            cOMMENTS = ds.Tables[7].Rows[i]["COMMENTS"].ToString();
                        }


                        qls.Add(new optionMdel
                        {
                            Option = ds.Tables[7].Rows[i]["SITIME_ID"].ToString(),
                            Option1 = tIME,
                            Option2 = aCTIVITIES,
                            Option3 = cOMMENTS,

                        });
                    }
                    int temp = qls.Count; textVisibleOption.Value = qls.Count.ToString();
                    for (int jl = 0; jl < (OptionCount - temp); jl++)
                    {
                        qls.Add(new optionMdel() { Option = string.Empty });
                    }
                    txtSignleChoice.DataSource = qls;
                    txtSignleChoice.DataBind();


                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        SelectMonth.SelectedValue = ds.Tables[1].Rows[i]["MONTHS"].ToString();
                        SelectMonth_SelectedIndexChanged(null, null);

                        MonthSelect.SelectedValue = ds.Tables[1].Rows[i]["ABILITY_months"].ToString();
                        MonthSelect_SelectedIndexChanged(null, null);
                    }

                    updAgeStage.Update();
                    updAbility.Update();
                }
            }
        }




        //ClientScript.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('ok')", true);



        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string rangevalue = hdnrange.Value.ToString();
            int range1 = 0;
            if (!string.IsNullOrEmpty(rangevalue) && rangevalue != "")
            {
                int.TryParse(rangevalue, out range1);
                range1 = range1 / 10;
            }

            string rangevalue2 = Hdnrange2.Value.ToString();
            int range2 = 0;
            if (!string.IsNullOrEmpty(rangevalue2) && rangevalue2 != "")
            {
                int.TryParse(rangevalue2, out range2);
                range2 = range2 / 10;
            }

            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            int HidSI_ID = 0;
            string Option1 = string.Empty;
            string Option2 = string.Empty;
            string Option3 = string.Empty;


            for (int j = 1; j <= OptionCount; j++)
            {
                RepeaterItem item = txtSignleChoice.Items.Count >= j ? txtSignleChoice.Items[j - 1] : null;
                if (item != null)
                {
                    HiddenField SI_ID = item.FindControl("txtSI_ID") as HiddenField;
                    TextBox TIME = item.FindControl("txtTIME") as TextBox;
                    TextBox ACTIVITIES = item.FindControl("txtACTIVITIES") as TextBox;
                    TextBox COMMENTS = item.FindControl("txtCOMMENTS") as TextBox;
                    if (TIME.Text != "" || ACTIVITIES.Text != "" || COMMENTS.Text != "")
                    {
                        int.TryParse(SI_ID.Value.ToString(), out HidSI_ID);

                        Option1 = TIME.Text.Trim();

                        Option2 = ACTIVITIES.Text.Trim();

                        Option3 = COMMENTS.Text.Trim();



                        int k = SID.SetTimeLine(_appointmentID, HidSI_ID, Option1, Option2, Option3, DateTime.UtcNow.AddMinutes(330), _loginID);

                    }
                    else if (TIME.Text == "" && ACTIVITIES.Text == "" && COMMENTS.Text == "")
                    {
                        int.TryParse(SI_ID.Value.ToString(), out HidSI_ID);
                        int P = SID.DeleteRow(HidSI_ID);
                    }
                }
            }



            bool IsFinal = txtFinal.Checked;
            bool IsGiven = txtGiven.Checked;
            DateTime GivenDate = new DateTime();
            if (IsGiven)
            {
                if (txtGivenDate.Text.Trim().Length > 0)
                {
                    DateTime.TryParseExact(txtGivenDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                }
            }
            string DiagnosisIDs = "";
            for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            {
                if (txtDiagnosis.Items[k].Selected)
                {
                    DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
                }
            }
            DataSet ds = SID.Getsi2022(_appointmentID);
            //int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            //if (g < 0)
            //{
            //    DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            //}
            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }
            //int EnterReport = 0; if (Doctor_EnterReport.SelectedItem != null) { int.TryParse(Doctor_EnterReport.SelectedItem.Value, out EnterReport); }



            //string DailySchedule_Dailyaroutine = string.Empty;
            //if (DailySchedule_Dailyaroutine_1.Checked) { DailySchedule_Dailyaroutine = DailySchedule_Dailyaroutine_1.Text.Trim(); }
            //if (DailySchedule_Dailyaroutine_2.Checked) { DailySchedule_Dailyaroutine = DailySchedule_Dailyaroutine_2.Text.Trim(); }
            //if (DailySchedule_Dailyaroutine_3.Checked) { DailySchedule_Dailyaroutine = DailySchedule_Dailyaroutine_3.Text.Trim(); }
            //hfdTime.Value = ds.Tables[1].Rows[0]["DailySchedule_WakeUpTime"].ToString();
            List<ListItem> selected = new List<ListItem>();

            //foreach (ListItem item in DailySchedule_BreakFastContent.Items)
            //    if (item.Selected) selected.Add(item);

            //foreach (ListItem item in DailySchedule_LunchContent.Items)
            //    if (item.Selected) selected.Add(item);
            //foreach (ListItem item in DailySchedule_Snacks.Items)
            //    if (item.Selected) selected.Add(item);
            //foreach (ListItem item in DailySchedule_Dinner_content.Items)
            //    if (item.Selected) selected.Add(item);
            //foreach (ListItem item in SelfCare_CurrentlyEats.Items)
            //    if (item.Selected) selected.Add(item);


            //#region
            //string SelfCare_Brushing = string.Empty;
            //if (SelfCare_Brushing_1.Checked) { SelfCare_Brushing = SelfCare_Brushing_1.Text.Trim(); }
            //if (SelfCare_Brushing_2.Checked) { SelfCare_Brushing = SelfCare_Brushing_2.Text.Trim(); }
            //if (SelfCare_Brushing_3.Checked) { SelfCare_Brushing = SelfCare_Brushing_3.Text.Trim(); }

            //string SelfCare_Bathing = string.Empty;
            //if (SelfCare_Bathing_1.Checked) { SelfCare_Bathing = SelfCare_Bathing_1.Text.Trim(); }
            //if (SelfCare_Bathing_2.Checked) { SelfCare_Bathing = SelfCare_Bathing_2.Text.Trim(); }
            //if (SelfCare_Bathing_3.Checked) { SelfCare_Bathing = SelfCare_Bathing_3.Text.Trim(); }
            //string SelfCare_Toileting = string.Empty;
            //if (SelfCare_Toileting_1.Checked) { SelfCare_Toileting = SelfCare_Toileting_1.Text.Trim(); }
            //if (SelfCare_Toileting_2.Checked) { SelfCare_Toileting = SelfCare_Toileting_2.Text.Trim(); }
            //if (SelfCare_Toileting_3.Checked) { SelfCare_Toileting = SelfCare_Toileting_3.Text.Trim(); }

            //string SelfCare_Dressing = string.Empty;
            //if (SelfCare_Dressing_1.Checked) { SelfCare_Dressing = SelfCare_Dressing_1.Text.Trim(); }
            //if (SelfCare_Dressing_2.Checked) { SelfCare_Dressing = SelfCare_Dressing_2.Text.Trim(); }
            //if (SelfCare_Dressing_3.Checked) { SelfCare_Dressing = SelfCare_Dressing_3.Text.Trim(); }



            //string SelfCare_Breakfast = string.Empty;
            //if (SelfCare_Breakfast_1.Checked) { SelfCare_Breakfast = SelfCare_Breakfast_1.Text.Trim(); }
            //if (SelfCare_Breakfast_2.Checked) { SelfCare_Breakfast = SelfCare_Breakfast_2.Text.Trim(); }
            //if (SelfCare_Breakfast_3.Checked) { SelfCare_Breakfast = SelfCare_Breakfast_3.Text.Trim(); }

            //string SelfCare_Lunch = string.Empty;
            //if (SelfCare_Lunch_1.Checked) { SelfCare_Lunch = SelfCare_Lunch_1.Text.Trim(); }
            //if (SelfCare_Lunch_2.Checked) { SelfCare_Lunch = SelfCare_Lunch_2.Text.Trim(); }
            //if (SelfCare_Lunch_3.Checked) { SelfCare_Lunch = SelfCare_Lunch_3.Text.Trim(); }

            //string SelfCare_Snacks = string.Empty;
            //if (SelfCare_Snacks_1.Checked) { SelfCare_Snacks = SelfCare_Snacks_1.Text.Trim(); }
            //if (SelfCare_Snacks_2.Checked) { SelfCare_Snacks = SelfCare_Snacks_2.Text.Trim(); }
            //if (SelfCare_Snacks_3.Checked) { SelfCare_Snacks = SelfCare_Snacks_3.Text.Trim(); }

            //string SelfCare_Dinner = string.Empty;
            //if (SelfCare_Dinner_1.Checked) { SelfCare_Dinner = SelfCare_Dinner_1.Text.Trim(); }
            //if (SelfCare_Dinner_2.Checked) { SelfCare_Dinner = SelfCare_Dinner_2.Text.Trim(); }
            //if (SelfCare_Dinner_3.Checked) { SelfCare_Dinner = SelfCare_Dinner_3.Text.Trim(); }

            //string SelfCare_GettingInBus = string.Empty;
            //if (SelfCare_GettingInBus_1.Checked) { SelfCare_GettingInBus = SelfCare_GettingInBus_1.Text.Trim(); }
            //if (SelfCare_GettingInBus_2.Checked) { SelfCare_GettingInBus = SelfCare_GettingInBus_2.Text.Trim(); }
            //if (SelfCare_GettingInBus_3.Checked) { SelfCare_GettingInBus = SelfCare_GettingInBus_3.Text.Trim(); }

            //string SelfCare_GoingToSchool = string.Empty;
            //if (SelfCare_GoingToSchool_1.Checked) { SelfCare_GoingToSchool = SelfCare_GoingToSchool_1.Text.Trim(); }
            //if (SelfCare_GoingToSchool_2.Checked) { SelfCare_GoingToSchool = SelfCare_GoingToSchool_2.Text.Trim(); }
            //if (SelfCare_GoingToSchool_3.Checked) { SelfCare_GoingToSchool = SelfCare_GoingToSchool_3.Text.Trim(); }

            //string SelfCare_comeBackSchool = string.Empty;
            //if (SelfCare_comeBackSchool_1.Checked) { SelfCare_comeBackSchool = SelfCare_comeBackSchool_1.Text.Trim(); }
            //if (SelfCare_comeBackSchool_2.Checked) { SelfCare_comeBackSchool = SelfCare_comeBackSchool_2.Text.Trim(); }
            //if (SelfCare_comeBackSchool_3.Checked) { SelfCare_comeBackSchool = SelfCare_comeBackSchool_3.Text.Trim(); }

            //string SelfCare_Ambulation = string.Empty;
            //if (SelfCare_Ambulation_1.Checked) { SelfCare_Ambulation = SelfCare_Ambulation_1.Text.Trim(); }
            //if (SelfCare_Ambulation_2.Checked) { SelfCare_Ambulation = SelfCare_Ambulation_2.Text.Trim(); }
            //if (SelfCare_Ambulation_3.Checked) { SelfCare_Ambulation = SelfCare_Ambulation_3.Text.Trim(); }
            //#endregion


            string PersonalSocial_CurrentPlace = string.Empty;
            if (PersonalSocial_CurrentPlace_1.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_1.Text.Trim(); }
            if (PersonalSocial_CurrentPlace_2.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_2.Text.Trim(); }
            if (PersonalSocial_CurrentPlace_3.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_3.Text.Trim(); }

            string PersonalSocial_WhatHeDoes = string.Empty;
            if (PersonalSocial_WhatHeDoes_1.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_1.Text.Trim(); }
            if (PersonalSocial_WhatHeDoes_2.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_2.Text.Trim(); }
            if (PersonalSocial_WhatHeDoes_3.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_3.Text.Trim(); }

            string PersonalSocial_BodyAwareness = string.Empty;
            if (PersonalSocial_BodyAwareness_1.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_1.Text.Trim(); }
            if (PersonalSocial_BodyAwareness_2.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_2.Text.Trim(); }
            if (PersonalSocial_BodyAwareness_3.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_3.Text.Trim(); }

            string PersonalSocial_BodySchema = string.Empty;
            if (PersonalSocial_BodySchema_1.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_1.Text.Trim(); }
            if (PersonalSocial_BodySchema_2.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_2.Text.Trim(); }
            if (PersonalSocial_BodySchema_3.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_3.Text.Trim(); }

            string PersonalSocial_ExploreEnvironment = string.Empty;
            if (PersonalSocial_ExploreEnvironment_1.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_1.Text.Trim(); }
            if (PersonalSocial_ExploreEnvironment_2.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_2.Text.Trim(); }
            if (PersonalSocial_ExploreEnvironment_3.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_3.Text.Trim(); }

            string PersonalSocial_Motivated = string.Empty;
            if (PersonalSocial_Motivated_1.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_1.Text.Trim(); }
            if (PersonalSocial_Motivated_2.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_2.Text.Trim(); }
            if (PersonalSocial_Motivated_3.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_3.Text.Trim(); }

            string PersonalSocial_EyeContact = string.Empty;
            if (PersonalSocial_EyeContact_1.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_1.Text.Trim(); }
            if (PersonalSocial_EyeContact_2.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_2.Text.Trim(); }
            if (PersonalSocial_EyeContact_3.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_3.Text.Trim(); }
            if (PersonalSocial_EyeContact_4.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_4.Text.Trim(); }

            string PersonalSocial_SocialSmile = string.Empty;
            if (PersonalSocial_SocialSmile_1.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_1.Text.Trim(); }
            if (PersonalSocial_SocialSmile_2.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_2.Text.Trim(); }
            if (PersonalSocial_SocialSmile_3.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_3.Text.Trim(); }
            if (PersonalSocial_SocialSmile_4.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_3.Text.Trim(); }

            string PersonalSocial_FamilyRegards = string.Empty;
            if (PersonalSocial_FamilyRegards_1.Checked) { PersonalSocial_FamilyRegards = PersonalSocial_FamilyRegards_1.Text.Trim(); }
            if (PersonalSocial_FamilyRegards_2.Checked) { PersonalSocial_FamilyRegards = PersonalSocial_FamilyRegards_2.Text.Trim(); }

            //foreach (ListItem item in PersonalSocial_ChildSocially.Items)
            //    if (item.Selected) selected.Add(item);
            string PersonalSocial_ChildSocially = string.Empty;
            if (PersonalSocial_ChildSocially_1.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_1.Text.Trim(); }
            if (PersonalSocial_ChildSocially_2.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_2.Text.Trim(); }
            if (PersonalSocial_ChildSocially_3.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_3.Text.Trim(); }
            if (PersonalSocial_ChildSocially_4.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_4.Text.Trim(); }
            if (PersonalSocial_ChildSocially_5.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_5.Text.Trim(); }

            string SpeechLanguage_UnusualSoundsJargonSpeech = string.Empty;
            if (SpeechLanguage_UnusualSoundsJargonSpeech_1.Checked) { SpeechLanguage_UnusualSoundsJargonSpeech = SpeechLanguage_UnusualSoundsJargonSpeech_1.Text.Trim(); }
            if (SpeechLanguage_UnusualSoundsJargonSpeech_2.Checked) { SpeechLanguage_UnusualSoundsJargonSpeech = SpeechLanguage_UnusualSoundsJargonSpeech_2.Text.Trim(); }

            string SpeechLanguage_speechgestures = string.Empty;
            if (SpeechLanguage_speechgestures_1.Checked) { SpeechLanguage_speechgestures = SpeechLanguage_speechgestures_1.Text.Trim(); }
            if (SpeechLanguage_speechgestures_2.Checked) { SpeechLanguage_speechgestures = SpeechLanguage_speechgestures_2.Text.Trim(); }

            string SpeechLanguage_TwowayInteraction = string.Empty;
            if (SpeechLanguage_TwowayInteraction_1.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_1.Text.Trim(); }
            if (SpeechLanguage_TwowayInteraction_2.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_2.Text.Trim(); }
            if (SpeechLanguage_TwowayInteraction_3.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_3.Text.Trim(); }

            string FamilyStructure_QualityTimeMother = string.Empty;
            if (FamilyStructure_QualityTimeMother_1.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_1.Text.Trim(); }
            if (FamilyStructure_QualityTimeMother_2.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_2.Text.Trim(); }
            if (FamilyStructure_QualityTimeMother_3.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_3.Text.Trim(); }

            string FamilyStructure_QualityTimeFather = string.Empty;
            if (FamilyStructure_QualityTimeFather_1.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_1.Text.Trim(); }
            if (FamilyStructure_QualityTimeFather_2.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_2.Text.Trim(); }
            if (FamilyStructure_QualityTimeFather_3.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_3.Text.Trim(); }

            string FamilyStructure_QualityTimeWeekend = string.Empty;
            if (Mother_Weekends_1.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_1.Text.Trim(); }
            if (Mother_Weekends_2.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_2.Text.Trim(); }
            if (Mother_Weekends_3.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_3.Text.Trim(); }

            string Father_Weekends = string.Empty;
            if (Father_Weekends_1.Checked) { Father_Weekends = Father_Weekends_1.Text.Trim(); }
            if (Father_Weekends_2.Checked) { Father_Weekends = Father_Weekends_2.Text.Trim(); }
            if (Father_Weekends_3.Checked) { Father_Weekends = Father_Weekends_3.Text.Trim(); }

            string FamilyStructure_TimeForThreapy = string.Empty;
            if (FamilyStructure_TimeForThreapy_1.Checked) { FamilyStructure_TimeForThreapy = FamilyStructure_TimeForThreapy_1.Text.Trim(); }
            if (FamilyStructure_TimeForThreapy_2.Checked) { FamilyStructure_TimeForThreapy = FamilyStructure_TimeForThreapy_2.Text.Trim(); }

            string FamilyStructure_AcceptanceCondition = string.Empty;
            if (FamilyStructure_AcceptanceCondition_1.Checked) { FamilyStructure_AcceptanceCondition = FamilyStructure_AcceptanceCondition_1.Text.Trim(); }
            if (FamilyStructure_AcceptanceCondition_2.Checked) { FamilyStructure_AcceptanceCondition = FamilyStructure_AcceptanceCondition_2.Text.Trim(); }

            string FamilyStructure_ExtraCaricular = string.Empty;
            if (FamilyStructure_ExtraCaricular_1.Checked) { FamilyStructure_ExtraCaricular = FamilyStructure_ExtraCaricular_1.Text.Trim(); }
            if (FamilyStructure_ExtraCaricular_2.Checked) { FamilyStructure_ExtraCaricular = FamilyStructure_ExtraCaricular_2.Text.Trim(); }

            //string Mother_Working = string.Empty;
            //if (Mother_Working_1.Checked) { Mother_Working = Mother_Working_1.Text.Trim(); }
            //if (Mother_Working_2.Checked) { Mother_Working = Mother_Working_2.Text.Trim(); }
            //string Father_Working = string.Empty;
            //if (Father_Working_1.Checked) { Father_Working = Father_Working_1.Text.Trim(); }
            //if (Father_Working_2.Checked) { Father_Working = Father_Working_2.Text.Trim(); }
            //string Househelp = string.Empty;
            //if (Househelp_1.Checked) { Househelp = Househelp_1.Text.Trim(); }
            //if (Househelp_2.Checked) { Househelp = Househelp_2.Text.Trim(); }

            //string FamilyStructure_SiblingBrother = string.Empty;
            //if (FamilyStructure_SiblingBrother_1.Checked) { FamilyStructure_SiblingBrother = FamilyStructure_SiblingBrother_1.Text.Trim(); }
            //if (FamilyStructure_SiblingBrother_2.Checked) { FamilyStructure_SiblingBrother = FamilyStructure_SiblingBrother_2.Text.Trim(); }

            //string FamilyStructure_SiblingSister = string.Empty;
            //if (FamilyStructure_SiblingSister_1.Checked) { FamilyStructure_SiblingSister = FamilyStructure_SiblingSister_1.Text.Trim(); }
            //if (FamilyStructure_SiblingSister_2.Checked) { FamilyStructure_SiblingSister = FamilyStructure_SiblingSister_2.Text.Trim(); }

            //string FamilyStructure_SiblingNA = string.Empty;
            //if (FamilyStructure_SiblingNA_1.Checked) { FamilyStructure_SiblingNA = FamilyStructure_SiblingNA_1.Text.Trim(); }
            //if (FamilyStructure_SiblingNA_2.Checked) { FamilyStructure_SiblingNA = FamilyStructure_SiblingNA_2.Text.Trim(); }

            string unassociated = string.Empty;
            if (chkunassociated.Checked) { unassociated = chkunassociated.Text.Trim(); }
            string solitary = string.Empty;
            if (chksolitary.Checked) { solitary = chksolitary.Text.Trim(); }
            string onlooker = string.Empty;
            if (chkonlooker.Checked) { onlooker = chkonlooker.Text.Trim(); }
            string parallel = string.Empty;
            if (chkparallel.Checked) { parallel = chkparallel.Text.Trim(); }
            string associative = string.Empty;
            if (chkassociative.Checked) { associative = chkassociative.Text.Trim(); }
            string cooperative = string.Empty;
            if (chkcooperative.Checked) { cooperative = chkcooperative.Text.Trim(); }

            string Behaviour_situationalmeltdowns = string.Empty;
            if (Behaviour_situationalmeltdowns_1.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_1.Text.Trim(); }
            if (Behaviour_situationalmeltdowns_2.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_2.Text.Trim(); }
            if (Behaviour_situationalmeltdowns_3.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_3.Text.Trim(); }
            //foreach (ListItem item in Behaviour_situationalmeltdowns.Items)
            //    if (item.Selected) selected.Add(item);

            string Schoolinfo_Attend = string.Empty;
            if (Schoolinfo_Attend_1.Checked) { Schoolinfo_Attend = Schoolinfo_Attend_1.Text.Trim(); }
            if (Schoolinfo_Attend_2.Checked) { Schoolinfo_Attend = Schoolinfo_Attend_2.Text.Trim(); }

            string Schoolinfo_Type = string.Empty;
            if (Schoolinfo_Type_1.Checked) { Schoolinfo_Type = Schoolinfo_Type_1.Text.Trim(); }
            if (Schoolinfo_Type_2.Checked) { Schoolinfo_Type = Schoolinfo_Type_2.Text.Trim(); }
            if (Schoolinfo_Type_3.Checked) { Schoolinfo_Type = Schoolinfo_Type_3.Text.Trim(); }


            string School_Bus = string.Empty;
            if (chkSchool_Bus.Checked) { School_Bus = chkSchool_Bus.Text.Trim(); }
            string Car = string.Empty;
            if (chkCar.Checked) { Car = chkCar.Text.Trim(); }
            string Two_Wheelers = string.Empty;
            if (chkTwo_Wheelers.Checked) { Two_Wheelers = chkTwo_Wheelers.Text.Trim(); }
            string walking = string.Empty;
            if (chkwalking.Checked) { walking = chkwalking.Text.Trim(); }
            string Public_Transport = string.Empty;
            if (chkPublic_Transport.Checked) { Public_Transport = chkPublic_Transport.Text.Trim(); }


            string Schoolinfo_NoOfTeacher = string.Empty;
            if (Schoolinfo_NoOfTeacher_1.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_1.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_2.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_2.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_3.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_3.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_4.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_4.Text.Trim(); }

            //string Schoolinfo_NoOfStudent = string.Empty;
            //if (Schoolinfo_NoOfStudent_1.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfStudent_1.Text.Trim(); }
            //if (Schoolinfo_NoOfStudent_2.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfStudent_2.Text.Trim(); }
            //if (Schoolinfo_NoOfStudent_3.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfStudent_3.Text.Trim(); }
            //if (Schoolinfo_NoOfStudent_4.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfStudent_4.Text.Trim(); }

            //string Seating_arrangement = string.Empty;

            //if (Seating_arrangement_1.Checked) { Seating_arrangement = Seating_arrangement_1.Text.Trim(); }

            //if (Seating_arrangement_2.Checked) { Seating_arrangement = Seating_arrangement_2.Text.Trim(); }

            //if (Seating_arrangement_3.Checked) { Seating_arrangement = Seating_arrangement_3.Text.Trim(); }

            //if (Seating_arrangement_4.Checked) { Seating_arrangement = Seating_arrangement_4.Text.Trim(); }

            string Floor = string.Empty;
            if (chkFloor.Checked) { Floor = chkFloor.Text.Trim(); }
            string single_bench = string.Empty;
            if (chksingle_bench.Checked) { single_bench = chksingle_bench.Text.Trim(); }
            string bench2 = string.Empty;
            if (chkbench2.Checked) { bench2 = chkbench2.Text.Trim(); }
            string round_table = string.Empty;
            if (chkround_table.Checked) { round_table = chkround_table.Text.Trim(); }


            //foreach (ListItem item in Schoolinfo_Mealtime.Items)
            //    if (item.Selected) selected.Add(item);
            string Schoolinfo_MealType = string.Empty;
            if (Schoolinfo_MealType_1.Checked) { Schoolinfo_MealType = Schoolinfo_MealType_1.Text.Trim(); }
            if (Schoolinfo_MealType_2.Checked) { Schoolinfo_MealType = Schoolinfo_MealType_2.Text.Trim(); }

            string Schoolinfo_Shareing = string.Empty;
            if (Schoolinfo_Shareing_1.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_1.Text.Trim(); }
            if (Schoolinfo_Shareing_2.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_2.Text.Trim(); }
            if (Schoolinfo_Shareing_3.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_3.Text.Trim(); }
            string Schoolinfo_HelpEating = string.Empty;
            if (Schoolinfo_HelpEating_1.Checked) { Schoolinfo_HelpEating = Schoolinfo_HelpEating_1.Text.Trim(); }
            if (Schoolinfo_HelpEating_2.Checked) { Schoolinfo_HelpEating = Schoolinfo_HelpEating_2.Text.Trim(); }
            string Schoolinfo_Friendship = string.Empty;
            if (Schoolinfo_Friendship_1.Checked) { Schoolinfo_Friendship = Schoolinfo_Friendship_1.Text.Trim(); }
            if (Schoolinfo_Friendship_2.Checked) { Schoolinfo_Friendship = Schoolinfo_Friendship_2.Text.Trim(); }
            string Schoolinfo_InteractionPeer = string.Empty;
            if (Schoolinfo_InteractionPeer_1.Checked) { Schoolinfo_InteractionPeer = Schoolinfo_InteractionPeer_1.Text.Trim(); }
            if (Schoolinfo_InteractionPeer_2.Checked) { Schoolinfo_InteractionPeer = Schoolinfo_InteractionPeer_2.Text.Trim(); }
            string Schoolinfo_InteractionTeacher = string.Empty;
            if (Schoolinfo_InteractionTeacher_1.Checked) { Schoolinfo_InteractionTeacher = Schoolinfo_InteractionTeacher_1.Text.Trim(); }
            if (Schoolinfo_InteractionTeacher_2.Checked) { Schoolinfo_InteractionTeacher = Schoolinfo_InteractionTeacher_2.Text.Trim(); }
            string Schoolinfo_AnnualFunction = string.Empty;
            if (Schoolinfo_AnnualFunction_1.Checked) { Schoolinfo_AnnualFunction = Schoolinfo_AnnualFunction_1.Text.Trim(); }
            if (Schoolinfo_AnnualFunction_2.Checked) { Schoolinfo_AnnualFunction = Schoolinfo_AnnualFunction_2.Text.Trim(); }

            string Schoolinfo_Sports = string.Empty;
            if (Schoolinfo_Sports_1.Checked) { Schoolinfo_Sports = Schoolinfo_Sports_1.Text.Trim(); }
            if (Schoolinfo_Sports_2.Checked) { Schoolinfo_Sports = Schoolinfo_Sports_2.Text.Trim(); }

            string Schoolinfo_Picnic = string.Empty;
            if (Schoolinfo_Picnic_1.Checked) { Schoolinfo_Picnic = Schoolinfo_Picnic_1.Text.Trim(); }
            if (Schoolinfo_Picnic_2.Checked) { Schoolinfo_Picnic = Schoolinfo_Picnic_2.Text.Trim(); }

            string Schoolinfo_ExtraCaricular = string.Empty;
            if (Schoolinfo_ExtraCaricular_1.Checked) { Schoolinfo_ExtraCaricular = Schoolinfo_ExtraCaricular_1.Text.Trim(); }
            if (Schoolinfo_ExtraCaricular_2.Checked) { Schoolinfo_ExtraCaricular = Schoolinfo_ExtraCaricular_2.Text.Trim(); }

            string Schoolinfo_CopyBoard = string.Empty;
            if (Schoolinfo_CopyBoard_1.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_1.Text.Trim(); }
            if (Schoolinfo_CopyBoard_2.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_2.Text.Trim(); }
            if (Schoolinfo_CopyBoard_3.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_3.Text.Trim(); }
            if (Schoolinfo_CopyBoard_4.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_4.Text.Trim(); }

            string Schoolinfo_Instructions = string.Empty;
            if (Schoolinfo_Instructions_1.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_1.Text.Trim(); }
            if (Schoolinfo_Instructions_2.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_2.Text.Trim(); }
            if (Schoolinfo_Instructions_3.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_3.Text.Trim(); }
            if (Schoolinfo_Instructions_4.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_4.Text.Trim(); }
            string Schoolinfo_ShadowTeacher = string.Empty;
            if (Schoolinfo_ShadowTeacher_1.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_1.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_2.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_2.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_3.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_3.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_4.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_4.Text.Trim(); }
            string Schoolinfo_CW_HW = string.Empty;
            if (Schoolinfo_CW_HW_1.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_1.Text.Trim(); }
            if (Schoolinfo_CW_HW_2.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_2.Text.Trim(); }
            if (Schoolinfo_CW_HW_3.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_3.Text.Trim(); }
            if (Schoolinfo_CW_HW_4.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_4.Text.Trim(); }

            string Schoolinfo_SpecialEducator = string.Empty;
            if (Schoolinfo_SpecialEducator_1.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_1.Text.Trim(); }
            if (Schoolinfo_SpecialEducator_2.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_2.Text.Trim(); }
            if (Schoolinfo_SpecialEducator_3.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_3.Text.Trim(); }

            string Schoolinfo_DeliveryInformation = string.Empty;
            if (Schoolinfo_DeliveryInformation_1.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_1.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_2.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_2.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_3.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_3.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_4.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_4.Text.Trim(); }
            //string Schoolinfo_SitOnlineSchool = string.Empty;
            //if (Schoolinfo_SitOnlineSchool_1.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_1.Text.Trim(); }
            //if (Schoolinfo_SitOnlineSchool_2.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_2.Text.Trim(); }
            //if (Schoolinfo_SitOnlineSchool_3.Checked) { Schoolinfo_SitOnlineSchool = Schoolinfo_SitOnlineSchool_3.Text.Trim(); }
            //string Schoolinfo_TeacherInstruction = string.Empty;
            //if (Schoolinfo_TeacherInstruction_1.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_1.Text.Trim(); }
            //if (Schoolinfo_TeacherInstruction_2.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_2.Text.Trim(); }
            //if (Schoolinfo_TeacherInstruction_3.Checked) { Schoolinfo_TeacherInstruction = Schoolinfo_TeacherInstruction_3.Text.Trim(); }

            //foreach (ListItem item in Schoolinfo_PlatformInteraction.Items)
            //    if (item.Selected) selected.Add(item);

            //foreach (ListItem item in Arousal_Evaluation.Items)
            //    if (item.Selected) selected.Add(item);
            //foreach (ListItem item in Arousal_GeneralState.Items)
            //    if (item.Selected) selected.Add(item);
            string Arousal_Stimuli = string.Empty;
            if (Arousal_Stimuli_1.Checked) { Arousal_Stimuli = Arousal_Stimuli_1.Text.Trim(); }
            if (Arousal_Stimuli_2.Checked) { Arousal_Stimuli = Arousal_Stimuli_2.Text.Trim(); }
            if (Arousal_Stimuli_3.Checked) { Arousal_Stimuli = Arousal_Stimuli_3.Text.Trim(); }
            string Arousal_Transition = string.Empty;
            if (Arousal_Transition_1.Checked) { Arousal_Transition = Arousal_Transition_1.Text.Trim(); }
            if (Arousal_Transition_2.Checked) { Arousal_Transition = Arousal_Transition_2.Text.Trim(); }

            //foreach (ListItem item in Arousal_FactorOCD.Items)
            //    if (item.Selected) selected.Add(item);
            //foreach (ListItem item in Arousal_ClaimingFactor.Items)
            //    if (item.Selected) selected.Add(item);

            //foreach (ListItem item in Attention_Span.Items)
            //    if (item.Selected) selected.Add(item);
            string Attention_FocusHandhome = string.Empty;
            if (Attention_FocusHandhome_1.Checked) { Attention_FocusHandhome = Attention_FocusHandhome_1.Text.Trim(); }
            if (Attention_FocusHandhome_2.Checked) { Attention_FocusHandhome = Attention_FocusHandhome_2.Text.Trim(); }
            string Attention_FocusHandSchool = string.Empty;
            if (Attention_FocusHandSchool_1.Checked) { Attention_FocusHandSchool = Attention_FocusHandSchool_1.Text.Trim(); }
            if (Attention_FocusHandSchool_2.Checked) { Attention_FocusHandSchool = Attention_FocusHandSchool_2.Text.Trim(); }
            string Attention_Dividing = string.Empty;
            if (Attention_Dividing_1.Checked) { Attention_Dividing = Attention_Dividing_1.Text.Trim(); }
            if (Attention_Dividing_2.Checked) { Attention_Dividing = Attention_Dividing_2.Text.Trim(); }
            string Attention_AgeAppropriate = string.Empty;
            if (Attention_AgeAppropriate_1.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_1.Text.Trim(); }
            if (Attention_AgeAppropriate_2.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_2.Text.Trim(); }
            if (Attention_AgeAppropriate_3.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_3.Text.Trim(); }

            string Attention_move = string.Empty;
            if (Attention_move_1.Checked) { Attention_move = Attention_move_1.Text.Trim(); }
            if (Attention_move_2.Checked) { Attention_move = Attention_move_2.Text.Trim(); }

            string Interacts = string.Empty;
            if (chkInteracts.Checked) { Interacts = chkInteracts.Text.Trim(); }
            string Does_not_initiate = string.Empty;
            if (chkDoes_not_initiate.Checked) { Does_not_initiate = chkDoes_not_initiate.Text.Trim(); }
            string Sustain = string.Empty;
            if (chkSustain.Checked) { Sustain = chkSustain.Text.Trim(); }
            string Fight = string.Empty;
            if (chkFight.Checked) { Fight = chkFight.Text.Trim(); }
            string Freeze = string.Empty;
            if (chkFreeze.Checked) { Freeze = chkFreeze.Text.Trim(); }
            string Fright = string.Empty;
            if (chkFright.Checked) { Fright = chkFright.Text.Trim(); }

            string Anxious = string.Empty;
            if (chkAnxious.Checked) { Anxious = chkAnxious.Text.Trim(); }
            string Comfortable = string.Empty;
            if (chkComfortable.Checked) { Comfortable = chkComfortable.Text.Trim(); }
            string Nervous = string.Empty;
            if (chkNervous.Checked) { Nervous = chkNervous.Text.Trim(); }
            string ANS_response = string.Empty;
            if (chkANS_response.Checked) { ANS_response = chkANS_response.Text.Trim(); }
            string OTHERS = string.Empty;
            if (chkOTHERS.Checked) { OTHERS = chkOTHERS.Text.Trim(); }


            string Interaction_SocialQues = string.Empty;
            if (Interaction_SocialQues_1.Checked) { Interaction_SocialQues = Interaction_SocialQues_1.Text.Trim(); }
            if (Interaction_SocialQues_2.Checked) { Interaction_SocialQues = Interaction_SocialQues_2.Text.Trim(); }

            //string Interaction_RelatesPeople = string.Empty;
            //if (Interaction_RelatesPeople_1.Checked) { Interaction_RelatesPeople = Interaction_RelatesPeople_1.Text.Trim(); }
            //if (Interaction_RelatesPeople_2.Checked) { Interaction_RelatesPeople = Interaction_RelatesPeople_2.Text.Trim(); }
            //if (Interaction_RelatesPeople_3.Checked) { Interaction_RelatesPeople = Interaction_RelatesPeople_3.Text.Trim(); }

            string Interaction_Friends = string.Empty;
            if (Interaction_Friends_1.Checked) { Interaction_Friends = Interaction_SocialQues_1.Text.Trim(); }
            if (Interaction_Friends_2.Checked) { Interaction_Friends = Interaction_SocialQues_2.Text.Trim(); }
            //string Interaction_KnowPeople = string.Empty;
            //if (Interaction_KnowPeople_1.Checked) { Interaction_KnowPeople = Interaction_KnowPeople_1.Text.Trim(); }
            //if (Interaction_KnowPeople_2.Checked) { Interaction_KnowPeople = Interaction_KnowPeople_2.Text.Trim(); }
            //if (Interaction_KnowPeople_3.Checked) { Interaction_KnowPeople = Interaction_KnowPeople_3.Text.Trim(); }

            string Affect_RangeEmotion = string.Empty;
            if (Affect_RangeEmotion_1.Checked) { Affect_RangeEmotion = Affect_RangeEmotion_1.Text.Trim(); }
            if (Affect_RangeEmotion_2.Checked) { Affect_RangeEmotion = Affect_RangeEmotion_2.Text.Trim(); }
            string Affect_ExpressEmotion = string.Empty;
            if (Affect_ExpressEmotion_1.Checked) { Affect_ExpressEmotion = Affect_ExpressEmotion_1.Text.Trim(); }
            if (Affect_ExpressEmotion_2.Checked) { Affect_ExpressEmotion = Affect_ExpressEmotion_2.Text.Trim(); }

            string Action_Purposeful = string.Empty;
            if (Action_Purposeful_1.Checked) { Action_Purposeful = Action_Purposeful_1.Text.Trim(); }
            if (Action_Purposeful_2.Checked) { Action_Purposeful = Action_Purposeful_2.Text.Trim(); }
            string Action_GoalOriented = string.Empty;
            if (Action_GoalOriented_1.Checked) { Action_GoalOriented = Action_GoalOriented_1.Text.Trim(); }
            if (Action_GoalOriented_2.Checked) { Action_GoalOriented = Action_GoalOriented_2.Text.Trim(); }

            string Action_FeedBackDependent = string.Empty;
            if (Action_FeedBackDependent_1.Checked) { Action_FeedBackDependent = Action_FeedBackDependent_1.Text.Trim(); }
            if (Action_FeedBackDependent_2.Checked) { Action_FeedBackDependent = Action_FeedBackDependent_2.Text.Trim(); }

            string Action_Constructive = string.Empty;
            if (Action_Constructive_1.Checked) { Action_Constructive = Action_Constructive_1.Text.Trim(); }
            if (Action_Constructive_2.Checked) { Action_Constructive = Action_Constructive_2.Text.Trim(); }



            string questions = string.Empty;
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    string QuestionNo = ((Label)item.FindControl("lblQuestionNo")).Text;
                    string Yes = ((CheckBox)item.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                    string No = ((CheckBox)item.FindControl("chkMonthNo")).Checked == true ? "1" : "0";
                    string Comment = ((TextBox)item.FindControl("txtMonthComment")).Text;
                    questions += QuestionNo + "$" + Yes + "$" + No + "$" + Comment + "~";
                }
            }
            questions = questions.Remove(questions.Length - 0, 0);

            string ABILITY_questions = string.Empty;
            foreach (RepeaterItem item in abilityQuestionsParent.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rpt = ((Repeater)item.FindControl("abilityQuestionsChild"));

                    if (rpt != null)
                    {
                        foreach (RepeaterItem rptitem in rpt.Items)
                        {
                            string categoryId = ((Label)rptitem.FindControl("lblCategoryId")).Text;
                            string questionNO = ((Label)rptitem.FindControl("abilityquestionNO")).Text;
                            string Yes = ((CheckBox)rptitem.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                            string No = ((CheckBox)rptitem.FindControl("chkMonthNo")).Checked == true ? "1" : "0";

                            ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
                        }
                    }
                }
            }
            if (ABILITY_questions.Length > 0)
            {
                ABILITY_questions = ABILITY_questions.Remove(ABILITY_questions.Length - 1, 1);
                string abilityQuestionsChild = string.Empty;
            }






            ///////**************--------------------------App_Start set  procedure---------------------------------------------*****************-//



            int i = SID.Rpt_SI_SET_SUBMIT(_appointmentID, ClinicleObse_txt.Text.Trim(), DateTime.UtcNow.AddMinutes(330), _loginID,
                //DailySchedule_Dailyaroutine, hfdTime.Value, hfdTimerest.Value,
                //DailySchedule_Breakfast.Text.Trim(), hfdbreaktime.Value, hfdschooltime.Value,
                //DailySchedule_MidMorinig.Text.Trim(), DailySchedule_SchoolHours.Text.Trim(), hfdLunchTime.Value, 
                //DailySchedule_AfternoonRoutine.Text.Trim(), DailySchedule_Afternoo_nap.Text.Trim(),  hfddinnertime.Value,
                // DailySchedule_PistDinnerAct.Text.Trim(),
                // SelfCare_Brushing, SelfCare_Bathing,
                //SelfCare_Dressing, SelfCare_Toileting, SelfCare_Breakfast, SelfCare_Lunch, SelfCare_Snacks, SelfCare_Dinner, SelfCare_GettingInBus, SelfCare_GoingToSchool,
                //SelfCare_comeBackSchool, SelfCare_Ambulation, SelfCare_Homeostaticchanges.Text.Trim(), SelfCare_UrinationdetailsBedwetting.Text.Trim(),

                FamilyStructure_QualityTimeMother,
                FamilyStructure_QualityTimeFather, FamilyStructure_QualityTimeWeekend, Father_Weekends, FamilyStructure_TimeForThreapy, FamilyStructure_AcceptanceCondition, FamilyStructure_ExtraCaricular,
                 FamilyStructure_Diciplinary.Text.Trim(), FamilyStructure_SiblingBrother.Text.Trim(),
                 FamilyStructure_Expectations.Text.Trim(), FamilyStructure_CloselyInvolved.Text.Trim(),
                 FAMILY_cmt.Text.Trim(),

                  Schoolinfo_Attend, Schoolinfo_Type, Schoolinfo_SchoolHours.SelectedValue, School_Bus, Car, Two_Wheelers, walking, Public_Transport, Schoolinfo_NoOfTeacher, /*Schoolinfo_NoOfStudent,*/  Floor, single_bench, bench2, round_table, Schoolinfo_Mealtime.SelectedValue, Schoolinfo_MealType, Schoolinfo_Shareing, Schoolinfo_HelpEating,
                Schoolinfo_Friendship, Schoolinfo_InteractionPeer, Schoolinfo_InteractionTeacher, Schoolinfo_AnnualFunction, Schoolinfo_Sports, Schoolinfo_Picnic,
                Schoolinfo_ExtraCaricular, Schoolinfo_CopyBoard, Schoolinfo_Instructions, Schoolinfo_ShadowTeacher, Schoolinfo_CW_HW, Schoolinfo_SpecialEducator, Schoolinfo_DeliveryInformation,
                Schoolinfo_RemarkTeacher.Text.Trim(), SCHOOL_cmt.Text.Trim(),

                PersonalSocial_CurrentPlace, PersonalSocial_WhatHeDoes, PersonalSocial_BodyAwareness, PersonalSocial_BodySchema, PersonalSocial_ExploreEnvironment,
                PersonalSocial_Motivated, PersonalSocial_EyeContact, PersonalSocial_SocialSmile, PersonalSocial_FamilyRegards, /*PersonalSocial_RateChild.Text.Trim(),*/ PersonalSocial_ChildSocially,
                PERSONAL_cmt.Text.Trim(),

                SpeechLanguage_StartSpeek.Text.Trim(), SpeechLanguage_Monosyllables.Text.Trim(), SpeechLanguage_Bisyllables.Text.Trim(), SpeechLanguage_ShrotScentences.Text.Trim(),
                SpeechLanguage_LongScentences.Text.Trim(), SpeechLanguage_UnusualSoundsJargonSpeech, SpeechLanguage_speechgestures, SpeechLanguage_NonverbalfacialExpression.Text.Trim(),
                SpeechLanguage_NonverbalfacialEyeContact.Text.Trim(), SpeechLanguage_NonverbalfacialGestures.Text.Trim(), SpeechLanguage_SimpleComplex.Text.Trim(),
                SpeechLanguage_UnderstandImpliedMeaning.Text.Trim(), SpeechLanguage_UnderstandJokesarcasm.Text.Trim(), SpeechLanguage_Respondstoname.Text.Trim(), SpeechLanguage_TwowayInteraction,
                SpeechLanguage_NarrateIncidentsAtSchool.Text.Trim(),
                SpeechLanguage_NarrateIncidentsAtHome.Text.Trim(), SpeechLanguage_Needs.Text.Trim(), SpeechLanguage_Emotions.Text.Trim(),
                SpeechLanguage_AchievementsFailure.Text.Trim(), SpeechLanguage_Echolalia.Text.Trim(), Speech_cmt.Text.Trim(),

                Behaviour_FreeTime.Text.Trim(), unassociated, solitary, onlooker, parallel, associative, cooperative, Behaviour_situationalmeltdowns, BEHAVIOUR_cmt.Text.Trim(),

                //hdnrange.Value , Hdnrange2.Value,
                range1.ToString(), range2.ToString(),
                //Arousal_Evaluation.SelectedValue,Arousal_GeneralState.SelectedValue,
                Arousal_Stimuli, Arousal_Transition, Arousal_FactorOCD.Text.Trim(), Arousal_ClaimingFactor.Text.Trim(), Arousal_DipsDown.Text.Trim(), AROUSAL_cmt.Text.Trim(),

                 Affect_RangeEmotion, Affect_ExpressEmotion, Affect_Environment.Text.Trim(), Affect_Task.Text.Trim(), Affect_Individual.Text.Trim(), Affect_ThroughOut.Text.Trim(),
                Affect_Charaterising.Text.Trim(), Affect_cmt.Text.Trim(),

                Attention_AttentionSpan.Text.Trim(),
                Attention_FocusHandhome, Attention_FocusHandSchool, Attention_Dividing, Attention_ChangeActivities.Text.Trim(), Attention_AgeAppropriate,
                 Attention_Distractibility.Text.Trim(), Focal_Attention.Text.Trim(), Joint_Attention.Text.Trim(), Divided_Attention.Text.Trim(),
                Sustained_Attention.Text.Trim(), Alternating_Attention.Text.Trim(), Attention_move, ATTENTION_cmt.Text.Trim(),
                //Interaction_KnowPeople,


                Action_MotorPlanning.Text.Trim(), Action_Purposeful, Action_GoalOriented, Action_FeedBackDependent, Action_Constructive, Action_cmt.Text.Trim(),

                Interacts, cmtgathering.Text.Trim(), Does_not_initiate, Sustain, Fight, Freeze, Fright, Anxious, Comfortable, Nervous, ANS_response, OTHERS, Interaction_SocialQues, Interaction_Happiness.Text.Trim(), Interaction_Sadness.Text.Trim(),
                Interaction_Surprise.Text.Trim(), Interaction_Shock.Text.Trim(), Interaction_Friends, Interaction_Enjoy.Text.Trim(), INTERACTION_cmt.Text.Trim(),

                 TS_Registration.Text.Trim(), TS_Orientation.Text.Trim(), TS_Discrimination.Text.Trim(), TS_Responsiveness.Text.Trim(), SS_Bodyawareness.Text.Trim(), SS_Bodyschema.Text.Trim(), SS_Orientation.Text.Trim(),
             SS_Posterior.Text.Trim(), SS_Bilateral.Text.Trim(), SS_Balance.Text.Trim(), SS_Dominance.Text.Trim(), SS_Right.Text.Trim(), SS_identifies.Text.Trim(), SS_point.Text.Trim(), SS_Constantly.Text.Trim(),
             SS_clumsy.Text.Trim(), SS_maneuver.Text.Trim(), SS_overly.Text.Trim(), SS_stand.Text.Trim(), SS_indulge.Text.Trim(), SS_textures.Text.Trim(), SS_monkey.Text.Trim(), SS_swings.Text.Trim(), VM_Registration.Text.Trim(),
             VM_Orientation.Text.Trim(), VM_Discrimination.Text.Trim(), VM_Responsiveness.Text.Trim(), PS_Registration.Text.Trim(), PS_Gradation.Text.Trim(), PS_Discrimination.Text.Trim(), PS_Responsiveness.Text.Trim(),
             OM_Registration.Text.Trim(), OM_Orientation.Text.Trim(), OM_Discrimination.Text.Trim(), OM_Responsiveness.Text.Trim(), AS_Auditory.Text.Trim(), AS_Orientation.Text.Trim(), AS_Responsiveness.Text.Trim(),
             AS_discrimination.Text.Trim(), AS_Background.Text.Trim(), AS_localization.Text.Trim(), AS_Analysis.Text.Trim(), AS_sequencing.Text.Trim(), AS_blending.Text.Trim(), VS_Visual.Text.Trim(),
             VS_Responsiveness.Text.Trim(), VS_scanning.Text.Trim(), VS_constancy.Text.Trim(), VS_memory.Text.Trim(), VS_Perception.Text.Trim(), VS_hand.Text.Trim(), VS_foot.Text.Trim(), VS_discrimination.Text.Trim(),
             VS_closure.Text.Trim(), VS_Figureground.Text.Trim(), VS_Visualmemory.Text.Trim(), VS_sequential.Text.Trim(), VS_spatial.Text.Trim(), OS_Registration.Text.Trim(), OS_Orientation.Text.Trim(),
             OS_Discrimination.Text.Trim(), OS_Responsiveness.Text.Trim(),

             TestMeassures_GrossMotor.Text.Trim(), TestMeassures_FineMotor.Text.Trim(),
                TestMeassures_DenverLanguage.Text.Trim(), TestMeassures_DenverPersonal.Text.Trim(), Tests_cmt.Text.Trim(),

                score_Communication_2.Text.Trim(), Inter_Communication_2.Text.Trim(), GROSS_2.Text.Trim(), inter_Gross_2.Text.Trim(), FINE_2.Text.Trim(), inter_FINE_2.Text.Trim(), PROBLEM_2.Text.Trim(), inter_PROBLEM_2.Text.Trim(), PERSONAL_2.Text.Trim(), inter_PERSONAL_2.Text.Trim(),

                  Comm_3.Text.Trim(), inter_3.Text.Trim(), GROSS_3.Text.Trim(), GROSS_inter_3.Text.Trim(), FINE_3.Text.Trim(),
                  FINE_inter_3.Text.Trim(), PROBLEM_3.Text.Trim(), PROBLEM_inter_3.Text.Trim(), PERSONAL_3.Text.Trim(), PERSONAL_inter_3.Text.Trim(), Communication_6.Text.Trim(), comm_inter_6.Text.Trim(), GROSS_6.Text.Trim(), GROSS_inter_6.Text.Trim(), FINE_6.Text.Trim(), FINE_inter_6.Text.Trim(), PROBLEM_6.Text.Trim(), PROBLEM_inter_6.Text.Trim(), PERSONAL_6.Text.Trim(), PERSONAL_inter_6.Text.Trim(),
                  comm_7.Text.Trim(), inter_7.Text.Trim(), GROSS_7.Text.Trim(), GROSS_inter_7.Text.Trim(), FINE_7.Text.Trim(), FINE_inter_7.Text.Trim(), PROBLEM_7.Text.Trim(), PROBLEM_inter_7.Text.Trim(), PERSONAL_7.Text.Trim(), PERSONAL_inter_7.Text.Trim(), comm_9.Text.Trim(), inter_9.Text.Trim(), GROSS_9.Text.Trim(), GROSS_inter_9.Text.Trim(), FINE_9.Text.Trim(), FINE_inter_9.Text.Trim(), PROBLEM_9.Text.Trim(), PROBLEM_inter_9.Text.Trim(),
                  PERSONAL_9.Text.Trim(), PERSONAL_inter_9.Text.Trim(), comm_10.Text.Trim(), inter_10.Text.Trim(), GROSS_10.Text.Trim(), GROSS_inter_10.Text.Trim(), FINE_10.Text.Trim(), FINE_inter_10.Text.Trim(), PROBLEM_10.Text.Trim(), PROBLEM_inter_10.Text.Trim(), PERSONAL_10.Text.Trim(), PERSONAL_inter_10.Text.Trim(), comm_11.Text.Trim(), inter_11.Text.Trim(), GROSS_11.Text.Trim(), GROSS_inter_11.Text.Trim(),
                  FINE_11.Text.Trim(), FINE_inter_11.Text.Trim(), PROBLEM_11.Text.Trim(), PROBLEM_inter_11.Text.Trim(), PERSONAL_11.Text.Trim(), PERSONAL_inter_11.Text.Trim(), comm_13.Text.Trim(), inter_13.Text.Trim(), GROSS_13.Text.Trim(), GROSS_inter_13.Text.Trim(), FINE_13.Text.Trim(), FINE_inter_13.Text.Trim(), PROBLEM_13.Text.Trim(), PROBLEM_inter_13.Text.Trim(), PERSONAL_13.Text.Trim(),
                  PERSONAL_inter_13.Text.Trim(), comm_15.Text.Trim(), inter_15.Text.Trim(), GROSS_15.Text.Trim(), GROSS_inter_15.Text.Trim(), FINE_15.Text.Trim(), FINE_inter_15.Text.Trim(), PROBLEM_15.Text.Trim(), PROBLEM_inter_15.Text.Trim(), PERSONAL_15.Text.Trim(), PERSONAL_inter_15.Text.Trim(), comm_17.Text.Trim(), inter_17.Text.Trim(), GROSS_17.Text.Trim(), GROSS_inter_17.Text.Trim(), FINE_17.Text.Trim(),
                  FINE_inter_17.Text.Trim(), PROBLEM_17.Text.Trim(), PROBLEM_inter_17.Text.Trim(), PERSONAL_17.Text.Trim(), PERSONAL_inter_17.Text.Trim(), comm_19.Text.Trim(), inter_19.Text.Trim(), GROSS_19.Text.Trim(), GROSS_inter_19.Text.Trim(), FINE_19.Text.Trim(), FINE_inter_19.Text.Trim(), PROBLEM_19.Text.Trim(), PROBLEM_inter_19.Text.Trim(), PERSONAL_19.Text.Trim(), PERSONAL_inter_19.Text.Trim(),
                  comm_21.Text.Trim(), inter_21.Text.Trim(), GROSS_21.Text.Trim(), GROSS_inter_21.Text.Trim(), FINE_21.Text.Trim(), FINE_inter_21.Text.Trim(), PROBLEM_21.Text.Trim(), PROBLEM_inter_21.Text.Trim(), PERSONAL_21.Text.Trim(), PERSONAL_inter_21.Text.Trim(), comm_23.Text.Trim(), inter_23.Text.Trim(), GROSS_23.Text.Trim(), GROSS_inter_23.Text.Trim(), FINE_23.Text.Trim(), FINE_inter_23.Text.Trim(), PROBLEM_23.Text.Trim(),
                  PROBLEM_inter_23.Text.Trim(), PERSONAL_23.Text.Trim(), PERSONAL_inter_23.Text.Trim(), comm_25.Text.Trim(), inter_25.Text.Trim(), GROSS_25.Text.Trim(), GROSS_inter_25.Text.Trim(), FINE_25.Text.Trim(), FINE_inter_25.Text.Trim(), PROBLEM_25.Text.Trim(), PROBLEM_inter_25.Text.Trim(), PERSONAL_25.Text.Trim(), PERSONAL_inter_25.Text.Trim(), comm_28.Text.Trim(), inter_28.Text.Trim(), GROSS_28.Text.Trim(),
                  GROSS_inter_28.Text.Trim(), FINE_28.Text.Trim(), FINE_inter_28.Text.Trim(), PROBLEM_28.Text.Trim(), PROBLEM_inter_28.Text.Trim(), PERSONAL_28.Text.Trim(), PERSONAL_inter_28.Text.Trim(), comm_31.Text.Trim(), inter_31.Text.Trim(), GROSS_31.Text.Trim(), GROSS_inter_31.Text.Trim(), FINE_31.Text.Trim(), FINE_inter_31.Text.Trim(), PROBLEM_31.Text.Trim(), PROBLEM_inter_31.Text.Trim(),
                  PERSONAL_31.Text.Trim(), PERSONAL_inter_31.Text.Trim(), comm_34.Text.Trim(), inter_34.Text.Trim(), GROSS_34.Text.Trim(), GROSS_inter_34.Text.Trim(), FINE_34.Text.Trim(), FINE_inter_34.Text.Trim(), PROBLEM_34.Text.Trim(), PROBLEM_inter_34.Text.Trim(), PERSONAL_34.Text.Trim(), PERSONAL_inter_34.Text.Trim(), comm_42.Text.Trim(), inter_42.Text.Trim(), GROSS_42.Text.Trim(), GROSS_inter_42.Text.Trim(),
                  FINE_42.Text.Trim(), FINE_inter_42.Text.Trim(), PROBLEM_42.Text.Trim(), PROBLEM_inter_42.Text.Trim(), PERSONAL_42.Text.Trim(), PERSONAL_inter_42.Text.Trim(), comm_45.Text.Trim(), inter_45.Text.Trim(), GROSS_45.Text.Trim(), GROSS_inter_45.Text.Trim(), FINE_45.Text.Trim(), FINE_inter_45.Text.Trim(), PROBLEM_45.Text.Trim(), PROBLEM_inter_45.Text.Trim(), PERSONAL_45.Text.Trim(),
                  PERSONAL_inter_45.Text.Trim(), comm_51.Text.Trim(), inter_51.Text.Trim(), GROSS_51.Text.Trim(), GROSS_inter_51.Text.Trim(), FINE_51.Text.Trim(), FINE_inter_51.Text.Trim(), PROBLEM_51.Text.Trim(), PROBLEM_inter_51.Text.Trim(), PERSONAL_51.Text.Trim(), PERSONAL_inter_51.Text.Trim(), comm_60.Text.Trim(), inter_60.Text.Trim(), GROSS_60.Text.Trim(), GROSS_inter_60.Text.Trim(), FINE_60.Text.Trim(),
                  FINE_inter_60.Text.Trim(), PROBLEM_60.Text.Trim(), PROBLEM_inter_60.Text.Trim(), PERSONAL_60.Text.Trim(), PERSONAL_inter_60.Text.Trim(),

             SelectMonth.SelectedValue, questions,

             General_Processing.Text.Trim(), AUDITORY_Processing.Text.Trim(), VISUAL_Processing.Text.Trim(), TOUCH_Processing.Text.Trim(), MOVEMENT_Processing.Text.Trim(), ORAL_Processing.Text.Trim(), Raw_score.Text.Trim(),
             Total_rawscore.Text.Trim(), Interpretation.Text.Trim(), Comments_1.Text.Trim(),
              Score_seeking.Text.Trim(), Score_Avoiding.Text.Trim(), Score_sensitivity.Text.Trim(), Score_Registration.Text.Trim(), Score_general.Text.Trim(),
             Score_Auditory.Text.Trim(), Score_visual.Text.Trim(), Score_touch.Text.Trim(), Score_movement.Text.Trim(), Score_oral.Text.Trim(), Score_behavioural.Text.Trim(),
             SEEKING.SelectedValue, AVOIDING.SelectedValue, SENSITIVITY_2.SelectedValue, REGISTRATION.SelectedValue, GENERAL.SelectedValue, AUDITORY.SelectedValue,
             VISUAL.SelectedValue, TOUCH.SelectedValue, MOVEMENT.SelectedValue, ORAL.SelectedValue, BEHAVIORAL.SelectedValue, Comments_2.Text.Trim(),
              SPchild_Seeker.Text.Trim(), SPchild_Avoider.Text.Trim(), SPchild_Sensor.Text.Trim(), SPchild_Bystander.Text.Trim(), SPchild_Auditory_3.Text.Trim(),
             SPchild_Visual_3.Text.Trim(), SPchild_Touch_3.Text.Trim(), SPchild_Movement_3.Text.Trim(), SPchild_Body_position.Text.Trim(), SPchild_Oral_3.Text.Trim(), SPchild_Conduct_3.Text.Trim(), SPchild_Social_emotional.Text.Trim(), SPchild_Attentional_3.Text.Trim(),
             Seeking_Seeker.SelectedValue, Avoiding_Avoider.SelectedValue, Sensitivity_Sensor.SelectedValue,
             Registration_Bystander.SelectedValue, Auditory_3.SelectedValue, Visual_3.SelectedValue, Touch_3.SelectedValue, Movement_3.SelectedValue, Body_position.SelectedValue, Oral_3.SelectedValue, Conduct_3.SelectedValue, Social_emotional.SelectedValue,
             Attentional_3.SelectedValue, Comments_3.Text.Trim(),
              SPAdult_Low_Registration.Text.Trim(), SPAdult_Sensory_seeking.Text.Trim(), SPAdult_Sensory_Sensitivity.Text.Trim(), SPAdult_Sensory_Avoiding.Text.Trim(),
               Low_Registration.SelectedValue, Sensory_seeking.SelectedValue, Sensory_Sensitivity.SelectedValue, Sensory_Avoiding.SelectedValue, Comments_4.Text.Trim(),

                SP_Low_Registration64.Text.Trim(), SP_Sensory_seeking_64.Text.Trim(), SP_Sensory_Sensitivity64.Text.Trim(), SP_Sensory_Avoiding64.Text.Trim(),
             Low_Registration_5.SelectedValue, Sensory_seeking_5.SelectedValue,
             Sensory_Sensitivity_5.SelectedValue, Sensory_Avoiding_5.SelectedValue, Comments_5.Text.Trim(),
              Older_Low_Registration.Text.Trim(), Older_Sensory_seeking.Text.Trim(), Older_Sensory_Sensitivity.Text.Trim(), Older_Sensory_Avoiding.Text.Trim(),
             Low_Registration_6.SelectedValue, Sensory_seeking_6.SelectedValue, Sensory_Sensitivity_6.SelectedValue, Sensory_Avoiding_6.SelectedValue,
             Comments_6.Text.Trim(),

             MonthSelect.SelectedValue, ABILITY_questions, ability_TOTAL.Text.Trim(), ability_COMMENTS.Text.Trim(),


              DCDQ_Throws1.Text.Trim(), DCDQ_Throws2.Text.Trim(), DCDQ_Throws3.Text.Trim(),
                   DCDQ_Catches1.Text.Trim(), DCDQ_Catches2.Text.Trim(), DCDQ_Catches3.Text.Trim(),
                   DCDQ_Hits1.Text.Trim(), DCDQ_Hits2.Text.Trim(), DCDQ_Hits3.Text.Trim(),
                   DCDQ_Jumps1.Text.Trim(), DCDQ_Jumps2.Text.Trim(), DCDQ_Jumps3.Text.Trim(),
                   DCDQ_Runs1.Text.Trim(), DCDQ_Runs2.Text.Trim(), DCDQ_Runs3.Text.Trim(),
                   DCDQ_Plans1.Text.Trim(), DCDQ_Plans2.Text.Trim(), DCDQ_Plans3.Text.Trim(),
                   DCDQ_Writing1.Text.Trim(), DCDQ_Writing2.Text.Trim(), DCDQ_Writing3.Text.Trim(),
                   DCDQ_legibly1.Text.Trim(), DCDQ_legibly2.Text.Trim(), DCDQ_legibly3.Text.Trim(),
                    DCDQ_Effort1.Text.Trim(), DCDQ_Effort2.Text.Trim(), DCDQ_Effort3.Text.Trim(),
                    DCDQ_Cuts1.Text.Trim(), DCDQ_Cuts2.Text.Trim(), DCDQ_Cuts3.Text.Trim(),
                    DCDQ_Likes1.Text.Trim(), DCDQ_Likes2.Text.Trim(), DCDQ_Likes3.Text.Trim(),
                    DCDQ_Learning1.Text.Trim(), DCDQ_Learning2.Text.Trim(), DCDQ_Learning3.Text.Trim(),
                    DCDQ_Quick1.Text.Trim(), DCDQ_Quick2.Text.Trim(), DCDQ_Quick3.Text.Trim(),
                    DCDQ_Bull1.Text.Trim(), DCDQ_Bull2.Text.Trim(), DCDQ_Bull3.Text.Trim(),
                    DCDQ_Does1.Text.Trim(), DCDQ_Does2.Text.Trim(), DCDQ_Does3.Text.Trim(),
                    DCDQ_Control.Text.Trim(),
                    DCDQ_Fine.Text.Trim(),
                    DCDQ_General.Text.Trim(),
                    DCDQ_Total.Text.Trim(), DCDQ_INTERPRETATION.Text.Trim(), DCDQ_COMMENT.Text.Trim(),


                      SIPTInfo_History.Text.Trim(),
              SIPTInfo_HandFunction1_GraspRight.Text.Trim(), SIPTInfo_HandFunction1_GraspLeft.Text.Trim(), SIPTInfo_HandFunction1_SphericalRight.Text.Trim(), SIPTInfo_HandFunction1_SphericalLeft.Text.Trim(),
              SIPTInfo_HandFunction1_HookRight.Text.Trim(), SIPTInfo_HandFunction1_HookLeft.Text.Trim(), SIPTInfo_HandFunction1_JawChuckRight.Text.Trim(), SIPTInfo_HandFunction1_JawChuckLeft.Text.Trim(),
              SIPTInfo_HandFunction1_GripRight.Text.Trim(), SIPTInfo_HandFunction1_GripLeft.Text.Trim(), SIPTInfo_HandFunction1_ReleaseRight.Text.Trim(), SIPTInfo_HandFunction1_ReleaseLeft.Text.Trim(),
              SIPTInfo_HandFunction2_OppositionLfR.Text.Trim(), SIPTInfo_HandFunction2_OppositionLfL.Text.Trim(), SIPTInfo_HandFunction2_OppositionMFR.Text.Trim(),
              SIPTInfo_HandFunction2_OppositionMFL.Text.Trim(), SIPTInfo_HandFunction2_OppositionRFR.Text.Trim(), SIPTInfo_HandFunction2_OppositionRFL.Text.Trim(), SIPTInfo_HandFunction2_PinchLfR.Text.Trim(),
              SIPTInfo_HandFunction2_PinchLfL.Text.Trim(), SIPTInfo_HandFunction2_PinchMFR.Text.Trim(), SIPTInfo_HandFunction2_PinchMFL.Text.Trim(), SIPTInfo_HandFunction2_PinchRFR.Text.Trim(),
              SIPTInfo_HandFunction2_PinchRFL.Text.Trim(), SIPTInfo_SIPT3_Spontaneous.Text.Trim(), SIPTInfo_SIPT3_Command.Text.Trim(), SIPTInfo_SIPT4_Kinesthesia.Text.Trim(), SIPTInfo_SIPT4_Finger.Text.Trim(),
              SIPTInfo_SIPT4_Localisation.Text.Trim(), SIPTInfo_SIPT4_DoubleTactile.Text.Trim(), SIPTInfo_SIPT4_Tactile.Text.Trim(), SIPTInfo_SIPT4_Graphesthesia.Text.Trim(), SIPTInfo_SIPT4_PostRotary.Text.Trim(),
              SIPTInfo_SIPT4_Standing.Text.Trim(), SIPTInfo_SIPT5_Color.Text.Trim(), SIPTInfo_SIPT5_Form.Text.Trim(), SIPTInfo_SIPT5_Size.Text.Trim(), SIPTInfo_SIPT5_Depth.Text.Trim(), SIPTInfo_SIPT5_Figure.Text.Trim(),
              SIPTInfo_SIPT5_Motor.Text.Trim(), SIPTInfo_SIPT6_Design.Text.Trim(), SIPTInfo_SIPT6_Constructional.Text.Trim(), SIPTInfo_SIPT7_Scanning.Text.Trim(), SIPTInfo_SIPT7_Memory.Text.Trim(), SIPTInfo_SIPT8_Postural.Text.Trim(),
              SIPTInfo_SIPT8_Oral.Text.Trim(), SIPTInfo_SIPT8_Sequencing.Text.Trim(), SIPTInfo_SIPT8_Commands.Text.Trim(), SIPTInfo_SIPT9_Bilateral.Text.Trim(), SIPTInfo_SIPT9_Contralat.Text.Trim(),
              SIPTInfo_SIPT9_PreferredHand.Text.Trim(), SIPTInfo_SIPT9_CrossingMidline.Text.Trim(), SIPTInfo_SIPT10_Draw.Text.Trim(), SIPTInfo_SIPT10_ClockFace.Text.Trim(), SIPTInfo_SIPT10_Filtering.Text.Trim(),
              SIPTInfo_SIPT10_MotorPlanning.Text.Trim(), SIPTInfo_SIPT10_BodyImage.Text.Trim(), SIPTInfo_SIPT10_BodySchema.Text.Trim(), SIPTInfo_SIPT10_Laterality.Text.Trim(), SIPTInfo_ActivityGiven_Remark.Text.Trim(),
              SIPTInfo_ActivityGiven_InterestActivity.Text.Trim(), SIPTInfo_ActivityGiven_InterestCompletion.Text.Trim(), SIPTInfo_ActivityGiven_Learning.Text.Trim(), SIPTInfo_ActivityGiven_Complexity.Text.Trim(),
              SIPTInfo_ActivityGiven_ProblemSolving.Text.Trim(), SIPTInfo_ActivityGiven_Concentration.Text.Trim(), SIPTInfo_ActivityGiven_Retension.Text.Trim(), SIPTInfo_ActivityGiven_SpeedPerfom.Text.Trim(),
              SIPTInfo_ActivityGiven_Neatness.Text.Trim(), SIPTInfo_ActivityGiven_Frustation.Text.Trim(), SIPTInfo_ActivityGiven_Work.Text.Trim(), SIPTInfo_ActivityGiven_Reaction.Text.Trim(),
              SIPTInfo_ActivityGiven_SociabilityTherapist.Text.Trim(), SIPTInfo_ActivityGiven_SociabilityStudents.Text.Trim(),

               Evaluation_Strengths.Text.Trim(), Evaluation_Concern_Barriers.Text.Trim(), Evaluation_Concern_Limitations.Text.Trim(),
              Evaluation_Concern_Posture.Text.Trim(), Evaluation_Concern_Impairment.Text.Trim(), Evaluation_Goal_Summary.Text.Trim(), Evaluation_Goal_Previous.Text.Trim(), Evaluation_Goal_LongTerm.Text.Trim(),
              Evaluation_Goal_ShortTerm.Text.Trim(), Evaluation_Goal_Impairment.Text.Trim(), Evaluation_Plan_Frequency.Text.Trim(), Evaluation_Plan_Service.Text.Trim(), Evaluation_Plan_Strategies.Text.Trim(),
             Evaluation_Plan_Equipment.Text.Trim(), Evaluation_Plan_Education.Text.Trim(),


                  //Daily_cmt.Text.Trim(),
                  //Self_cmt.Text.Trim(), 


                  Treatment_Home.Text.Trim(), Treatment_School.Text.Trim(), Treatment_Threapy.Text.Trim(), Treatment_cmt.Text.Trim(),



             Physioptherapist, Occupational,
             IsFinal, IsGiven, GivenDate, DiagnosisIDs, DiagnosisOther);
            //(DailySchedule_BreakFastContent.SelectedValue, DailySchedule_Dinner_content.SelectedValue, DailySchedule_LunchContent.SelectedValue,  DailySchedule_Snacks.SelectedValue,Interaction_RelatesPeople,
            //SelfCare_CurrentlyEats.SelectedValue,SpeechLanguage_Emotionalmilestones.Text.Trim(),SpeechLanguage_LanguageSpoken.Text.Trim(), SpeechLanguage_Want.Text.Trim (),Mother_Working, Father_Working, Househelp,
            // Schoolinfo_PlatformInteraction.SelectedValue, Schoolinfo_HourOnlineSchool.Text.Trim(), Schoolinfo_SitOnlineSchool,Schoolinfo_TeacherInstruction,  Schoolinfo_SetUp.Text.Trim(), Schoolinfo_BehaviourOnlineSchool.Text.Trim(),
            // Arousal_Optimal.Text.Trim(), Attention_Span.SelectedValue,Interaction_Strangers.Text.Trim(),TestMeassures_IQ.Text.Trim(), TestMeassures_DQ.Text.Trim (),Percentile_Range.Text.Trim(),   TestMeassures_ASQ.Text.Trim(), TestMeassures_HandWriting.Text.Trim(),TestMeassures_SIPT.Text.Trim(), TestMeassures_SensoryProfile.Text.Trim(), )  
            //score_Communication_2months.Text.Trim(), Inter_Communication_2months.Text.Trim(),
            //      GROSS_2months.Text.Trim(), inter_Gross_2months.Text.Trim(), FINE_2months.Text.Trim(), inter_FINE_2months.Text.Trim(), PROBLEM_2months.Text.Trim(), inter_PROBLEM_2moths.Text.Trim(), PERSONAL_2months.Text.Trim(), inter_PERSONAL_2months.Text.Trim(), 
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "SI report saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                //Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
                Response.Redirect(ResolveClientUrl("~/SessionRpt/SIRpt2022.aspx?record=" + Request.QueryString["record"]), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }

        public string cloneButtonLeft_sm(int index)
        {
            if (index == 0)
            {
                return "<a href=\"javascript:;\" class=\"btn btn-xs btn-default btn-success\" style=\"float:right; margin-left:20px;\" onclick= \"show_next_option(this);\"><i class=\"fa fa-plus-circle\"></i></a>";
            }
            else
            {
                return "<div class=\"rbutton\"></div>";
            }
        }
        public string cloneClass(int index, string Option)
        {
            if (index <= 1)
            {
                return string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(Option))
                    return string.Empty;
                return "hide";
            }
        }
        public string cloneClass(int index, string Option, string Option1, string Option2, string Option3)
        {
            if (index <= 1)
            {
                return string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(Option1) || !string.IsNullOrEmpty(Option2) || !string.IsNullOrEmpty(Option3))
                    return string.Empty;
                return "hide";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            SnehBLL.ReportSI2022_Bll SID = new SnehBLL.ReportSI2022_Bll();
            DataSet ds = SID.Getsi2022(_appointmentID);


            bool IsFinal = txtFinal.Checked;
            bool IsGiven = txtGiven.Checked;

            DateTime GivenDate = new DateTime();
            if (IsGiven)
            {
                if (txtGivenDate.Text.Trim().Length > 0)
                {
                    DateTime.TryParseExact(txtGivenDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                }
            }

            string PersonalSocial_CurrentPlace = string.Empty;
            if (PersonalSocial_CurrentPlace_1.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_1.Text.Trim(); }
            if (PersonalSocial_CurrentPlace_2.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_2.Text.Trim(); }
            if (PersonalSocial_CurrentPlace_3.Checked) { PersonalSocial_CurrentPlace = PersonalSocial_CurrentPlace_3.Text.Trim(); }

            string PersonalSocial_WhatHeDoes = string.Empty;
            if (PersonalSocial_WhatHeDoes_1.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_1.Text.Trim(); }
            if (PersonalSocial_WhatHeDoes_2.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_2.Text.Trim(); }
            if (PersonalSocial_WhatHeDoes_3.Checked) { PersonalSocial_WhatHeDoes = PersonalSocial_WhatHeDoes_3.Text.Trim(); }

            string PersonalSocial_BodyAwareness = string.Empty;
            if (PersonalSocial_BodyAwareness_1.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_1.Text.Trim(); }
            if (PersonalSocial_BodyAwareness_2.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_2.Text.Trim(); }
            if (PersonalSocial_BodyAwareness_3.Checked) { PersonalSocial_BodyAwareness = PersonalSocial_BodyAwareness_3.Text.Trim(); }

            string PersonalSocial_BodySchema = string.Empty;
            if (PersonalSocial_BodySchema_1.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_1.Text.Trim(); }
            if (PersonalSocial_BodySchema_2.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_2.Text.Trim(); }
            if (PersonalSocial_BodySchema_3.Checked) { PersonalSocial_BodySchema = PersonalSocial_BodySchema_3.Text.Trim(); }

            string PersonalSocial_ExploreEnvironment = string.Empty;
            if (PersonalSocial_ExploreEnvironment_1.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_1.Text.Trim(); }
            if (PersonalSocial_ExploreEnvironment_2.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_2.Text.Trim(); }
            if (PersonalSocial_ExploreEnvironment_3.Checked) { PersonalSocial_ExploreEnvironment = PersonalSocial_ExploreEnvironment_3.Text.Trim(); }

            string PersonalSocial_Motivated = string.Empty;
            if (PersonalSocial_Motivated_1.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_1.Text.Trim(); }
            if (PersonalSocial_Motivated_2.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_2.Text.Trim(); }
            if (PersonalSocial_Motivated_3.Checked) { PersonalSocial_Motivated = PersonalSocial_Motivated_3.Text.Trim(); }

            string PersonalSocial_EyeContact = string.Empty;
            if (PersonalSocial_EyeContact_1.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_1.Text.Trim(); }
            if (PersonalSocial_EyeContact_2.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_2.Text.Trim(); }
            if (PersonalSocial_EyeContact_3.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_3.Text.Trim(); }
            if (PersonalSocial_EyeContact_4.Checked) { PersonalSocial_EyeContact = PersonalSocial_EyeContact_4.Text.Trim(); }

            string PersonalSocial_SocialSmile = string.Empty;
            if (PersonalSocial_SocialSmile_1.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_1.Text.Trim(); }
            if (PersonalSocial_SocialSmile_2.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_2.Text.Trim(); }
            if (PersonalSocial_SocialSmile_3.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_3.Text.Trim(); }
            if (PersonalSocial_SocialSmile_4.Checked) { PersonalSocial_SocialSmile = PersonalSocial_SocialSmile_3.Text.Trim(); }

            string PersonalSocial_FamilyRegards = string.Empty;
            if (PersonalSocial_FamilyRegards_1.Checked) { PersonalSocial_FamilyRegards = PersonalSocial_FamilyRegards_1.Text.Trim(); }
            if (PersonalSocial_FamilyRegards_2.Checked) { PersonalSocial_FamilyRegards = PersonalSocial_FamilyRegards_2.Text.Trim(); }


            string PersonalSocial_ChildSocially = string.Empty;
            if (PersonalSocial_ChildSocially_1.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_1.Text.Trim(); }
            if (PersonalSocial_ChildSocially_2.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_2.Text.Trim(); }
            if (PersonalSocial_ChildSocially_3.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_3.Text.Trim(); }
            if (PersonalSocial_ChildSocially_4.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_4.Text.Trim(); }
            if (PersonalSocial_ChildSocially_5.Checked) { PersonalSocial_ChildSocially = PersonalSocial_ChildSocially_5.Text.Trim(); }

            string SpeechLanguage_UnusualSoundsJargonSpeech = string.Empty;
            if (SpeechLanguage_UnusualSoundsJargonSpeech_1.Checked) { SpeechLanguage_UnusualSoundsJargonSpeech = SpeechLanguage_UnusualSoundsJargonSpeech_1.Text.Trim(); }
            if (SpeechLanguage_UnusualSoundsJargonSpeech_2.Checked) { SpeechLanguage_UnusualSoundsJargonSpeech = SpeechLanguage_UnusualSoundsJargonSpeech_2.Text.Trim(); }

            string SpeechLanguage_speechgestures = string.Empty;
            if (SpeechLanguage_speechgestures_1.Checked) { SpeechLanguage_speechgestures = SpeechLanguage_speechgestures_1.Text.Trim(); }
            if (SpeechLanguage_speechgestures_2.Checked) { SpeechLanguage_speechgestures = SpeechLanguage_speechgestures_2.Text.Trim(); }

            string SpeechLanguage_TwowayInteraction = string.Empty;
            if (SpeechLanguage_TwowayInteraction_1.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_1.Text.Trim(); }
            if (SpeechLanguage_TwowayInteraction_2.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_2.Text.Trim(); }
            if (SpeechLanguage_TwowayInteraction_3.Checked) { SpeechLanguage_TwowayInteraction = SpeechLanguage_TwowayInteraction_3.Text.Trim(); }

            string FamilyStructure_QualityTimeMother = string.Empty;
            if (FamilyStructure_QualityTimeMother_1.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_1.Text.Trim(); }
            if (FamilyStructure_QualityTimeMother_2.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_2.Text.Trim(); }
            if (FamilyStructure_QualityTimeMother_3.Checked) { FamilyStructure_QualityTimeMother = FamilyStructure_QualityTimeMother_3.Text.Trim(); }

            string FamilyStructure_QualityTimeFather = string.Empty;
            if (FamilyStructure_QualityTimeFather_1.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_1.Text.Trim(); }
            if (FamilyStructure_QualityTimeFather_2.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_2.Text.Trim(); }
            if (FamilyStructure_QualityTimeFather_3.Checked) { FamilyStructure_QualityTimeFather = FamilyStructure_QualityTimeFather_3.Text.Trim(); }

            string FamilyStructure_QualityTimeWeekend = string.Empty;
            if (Mother_Weekends_1.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_1.Text.Trim(); }
            if (Mother_Weekends_2.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_2.Text.Trim(); }
            if (Mother_Weekends_3.Checked) { FamilyStructure_QualityTimeWeekend = Mother_Weekends_3.Text.Trim(); }

            string Father_Weekends = string.Empty;
            if (Father_Weekends_1.Checked) { Father_Weekends = Father_Weekends_1.Text.Trim(); }
            if (Father_Weekends_2.Checked) { Father_Weekends = Father_Weekends_2.Text.Trim(); }
            if (Father_Weekends_3.Checked) { Father_Weekends = Father_Weekends_3.Text.Trim(); }

            string FamilyStructure_TimeForThreapy = string.Empty;
            if (FamilyStructure_TimeForThreapy_1.Checked) { FamilyStructure_TimeForThreapy = FamilyStructure_TimeForThreapy_1.Text.Trim(); }
            if (FamilyStructure_TimeForThreapy_2.Checked) { FamilyStructure_TimeForThreapy = FamilyStructure_TimeForThreapy_2.Text.Trim(); }

            string FamilyStructure_AcceptanceCondition = string.Empty;
            if (FamilyStructure_AcceptanceCondition_1.Checked) { FamilyStructure_AcceptanceCondition = FamilyStructure_AcceptanceCondition_1.Text.Trim(); }
            if (FamilyStructure_AcceptanceCondition_2.Checked) { FamilyStructure_AcceptanceCondition = FamilyStructure_AcceptanceCondition_2.Text.Trim(); }

            string FamilyStructure_ExtraCaricular = string.Empty;
            if (FamilyStructure_ExtraCaricular_1.Checked) { FamilyStructure_ExtraCaricular = FamilyStructure_ExtraCaricular_1.Text.Trim(); }
            if (FamilyStructure_ExtraCaricular_2.Checked) { FamilyStructure_ExtraCaricular = FamilyStructure_ExtraCaricular_2.Text.Trim(); }


            string unassociated = string.Empty;
            if (chkunassociated.Checked) { unassociated = chkunassociated.Text.Trim(); }
            string solitary = string.Empty;
            if (chksolitary.Checked) { solitary = chksolitary.Text.Trim(); }
            string onlooker = string.Empty;
            if (chkonlooker.Checked) { onlooker = chkonlooker.Text.Trim(); }
            string parallel = string.Empty;
            if (chkparallel.Checked) { parallel = chkparallel.Text.Trim(); }
            string associative = string.Empty;
            if (chkassociative.Checked) { associative = chkassociative.Text.Trim(); }
            string cooperative = string.Empty;
            if (chkcooperative.Checked) { cooperative = chkcooperative.Text.Trim(); }

            string Behaviour_situationalmeltdowns = string.Empty;
            if (Behaviour_situationalmeltdowns_1.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_1.Text.Trim(); }
            if (Behaviour_situationalmeltdowns_2.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_2.Text.Trim(); }
            if (Behaviour_situationalmeltdowns_3.Checked) { Behaviour_situationalmeltdowns = Behaviour_situationalmeltdowns_3.Text.Trim(); }


            string Schoolinfo_Attend = string.Empty;
            if (Schoolinfo_Attend_1.Checked) { Schoolinfo_Attend = Schoolinfo_Attend_1.Text.Trim(); }
            if (Schoolinfo_Attend_2.Checked) { Schoolinfo_Attend = Schoolinfo_Attend_2.Text.Trim(); }

            string Schoolinfo_Type = string.Empty;
            if (Schoolinfo_Type_1.Checked) { Schoolinfo_Type = Schoolinfo_Type_1.Text.Trim(); }
            if (Schoolinfo_Type_2.Checked) { Schoolinfo_Type = Schoolinfo_Type_2.Text.Trim(); }
            if (Schoolinfo_Type_3.Checked) { Schoolinfo_Type = Schoolinfo_Type_3.Text.Trim(); }


            string School_Bus = string.Empty;
            if (chkSchool_Bus.Checked) { School_Bus = chkSchool_Bus.Text.Trim(); }
            string Car = string.Empty;
            if (chkCar.Checked) { Car = chkCar.Text.Trim(); }
            string Two_Wheelers = string.Empty;
            if (chkTwo_Wheelers.Checked) { Two_Wheelers = chkTwo_Wheelers.Text.Trim(); }
            string walking = string.Empty;
            if (chkwalking.Checked) { walking = chkwalking.Text.Trim(); }
            string Public_Transport = string.Empty;
            if (chkPublic_Transport.Checked) { Public_Transport = chkPublic_Transport.Text.Trim(); }


            string Schoolinfo_NoOfTeacher = string.Empty;
            if (Schoolinfo_NoOfTeacher_1.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_1.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_2.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_2.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_3.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_3.Text.Trim(); }
            if (Schoolinfo_NoOfTeacher_4.Checked) { Schoolinfo_NoOfTeacher = Schoolinfo_NoOfTeacher_4.Text.Trim(); }



            string Floor = string.Empty;
            if (chkFloor.Checked) { Floor = chkFloor.Text.Trim(); }
            string single_bench = string.Empty;
            if (chksingle_bench.Checked) { single_bench = chksingle_bench.Text.Trim(); }
            string bench2 = string.Empty;
            if (chkbench2.Checked) { bench2 = chkbench2.Text.Trim(); }
            string round_table = string.Empty;
            if (chkround_table.Checked) { round_table = chkround_table.Text.Trim(); }



            string Schoolinfo_MealType = string.Empty;
            if (Schoolinfo_MealType_1.Checked) { Schoolinfo_MealType = Schoolinfo_MealType_1.Text.Trim(); }
            if (Schoolinfo_MealType_2.Checked) { Schoolinfo_MealType = Schoolinfo_MealType_2.Text.Trim(); }

            string Schoolinfo_Shareing = string.Empty;
            if (Schoolinfo_Shareing_1.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_1.Text.Trim(); }
            if (Schoolinfo_Shareing_2.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_2.Text.Trim(); }
            if (Schoolinfo_Shareing_3.Checked) { Schoolinfo_Shareing = Schoolinfo_Shareing_3.Text.Trim(); }
            string Schoolinfo_HelpEating = string.Empty;
            if (Schoolinfo_HelpEating_1.Checked) { Schoolinfo_HelpEating = Schoolinfo_HelpEating_1.Text.Trim(); }
            if (Schoolinfo_HelpEating_2.Checked) { Schoolinfo_HelpEating = Schoolinfo_HelpEating_2.Text.Trim(); }
            string Schoolinfo_Friendship = string.Empty;
            if (Schoolinfo_Friendship_1.Checked) { Schoolinfo_Friendship = Schoolinfo_Friendship_1.Text.Trim(); }
            if (Schoolinfo_Friendship_2.Checked) { Schoolinfo_Friendship = Schoolinfo_Friendship_2.Text.Trim(); }
            string Schoolinfo_InteractionPeer = string.Empty;
            if (Schoolinfo_InteractionPeer_1.Checked) { Schoolinfo_InteractionPeer = Schoolinfo_InteractionPeer_1.Text.Trim(); }
            if (Schoolinfo_InteractionPeer_2.Checked) { Schoolinfo_InteractionPeer = Schoolinfo_InteractionPeer_2.Text.Trim(); }
            string Schoolinfo_InteractionTeacher = string.Empty;
            if (Schoolinfo_InteractionTeacher_1.Checked) { Schoolinfo_InteractionTeacher = Schoolinfo_InteractionTeacher_1.Text.Trim(); }
            if (Schoolinfo_InteractionTeacher_2.Checked) { Schoolinfo_InteractionTeacher = Schoolinfo_InteractionTeacher_2.Text.Trim(); }
            string Schoolinfo_AnnualFunction = string.Empty;
            if (Schoolinfo_AnnualFunction_1.Checked) { Schoolinfo_AnnualFunction = Schoolinfo_AnnualFunction_1.Text.Trim(); }
            if (Schoolinfo_AnnualFunction_2.Checked) { Schoolinfo_AnnualFunction = Schoolinfo_AnnualFunction_2.Text.Trim(); }

            string Schoolinfo_Sports = string.Empty;
            if (Schoolinfo_Sports_1.Checked) { Schoolinfo_Sports = Schoolinfo_Sports_1.Text.Trim(); }
            if (Schoolinfo_Sports_2.Checked) { Schoolinfo_Sports = Schoolinfo_Sports_2.Text.Trim(); }

            string Schoolinfo_Picnic = string.Empty;
            if (Schoolinfo_Picnic_1.Checked) { Schoolinfo_Picnic = Schoolinfo_Picnic_1.Text.Trim(); }
            if (Schoolinfo_Picnic_2.Checked) { Schoolinfo_Picnic = Schoolinfo_Picnic_2.Text.Trim(); }

            string Schoolinfo_ExtraCaricular = string.Empty;
            if (Schoolinfo_ExtraCaricular_1.Checked) { Schoolinfo_ExtraCaricular = Schoolinfo_ExtraCaricular_1.Text.Trim(); }
            if (Schoolinfo_ExtraCaricular_2.Checked) { Schoolinfo_ExtraCaricular = Schoolinfo_ExtraCaricular_2.Text.Trim(); }

            string Schoolinfo_CopyBoard = string.Empty;
            if (Schoolinfo_CopyBoard_1.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_1.Text.Trim(); }
            if (Schoolinfo_CopyBoard_2.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_2.Text.Trim(); }
            if (Schoolinfo_CopyBoard_3.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_3.Text.Trim(); }
            if (Schoolinfo_CopyBoard_4.Checked) { Schoolinfo_CopyBoard = Schoolinfo_CopyBoard_4.Text.Trim(); }

            string Schoolinfo_Instructions = string.Empty;
            if (Schoolinfo_Instructions_1.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_1.Text.Trim(); }
            if (Schoolinfo_Instructions_2.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_2.Text.Trim(); }
            if (Schoolinfo_Instructions_3.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_3.Text.Trim(); }
            if (Schoolinfo_Instructions_4.Checked) { Schoolinfo_Instructions = Schoolinfo_Instructions_4.Text.Trim(); }
            string Schoolinfo_ShadowTeacher = string.Empty;
            if (Schoolinfo_ShadowTeacher_1.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_1.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_2.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_2.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_3.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_3.Text.Trim(); }
            if (Schoolinfo_ShadowTeacher_4.Checked) { Schoolinfo_ShadowTeacher = Schoolinfo_ShadowTeacher_4.Text.Trim(); }
            string Schoolinfo_CW_HW = string.Empty;
            if (Schoolinfo_CW_HW_1.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_1.Text.Trim(); }
            if (Schoolinfo_CW_HW_2.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_2.Text.Trim(); }
            if (Schoolinfo_CW_HW_3.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_3.Text.Trim(); }
            if (Schoolinfo_CW_HW_4.Checked) { Schoolinfo_CW_HW = Schoolinfo_CW_HW_4.Text.Trim(); }

            string Schoolinfo_SpecialEducator = string.Empty;
            if (Schoolinfo_SpecialEducator_1.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_1.Text.Trim(); }
            if (Schoolinfo_SpecialEducator_2.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_2.Text.Trim(); }
            if (Schoolinfo_SpecialEducator_3.Checked) { Schoolinfo_SpecialEducator = Schoolinfo_SpecialEducator_3.Text.Trim(); }

            string Schoolinfo_DeliveryInformation = string.Empty;
            if (Schoolinfo_DeliveryInformation_1.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_1.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_2.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_2.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_3.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_3.Text.Trim(); }
            if (Schoolinfo_DeliveryInformation_4.Checked) { Schoolinfo_DeliveryInformation = Schoolinfo_DeliveryInformation_4.Text.Trim(); }

            string Arousal_Stimuli = string.Empty;
            if (Arousal_Stimuli_1.Checked) { Arousal_Stimuli = Arousal_Stimuli_1.Text.Trim(); }
            if (Arousal_Stimuli_2.Checked) { Arousal_Stimuli = Arousal_Stimuli_2.Text.Trim(); }
            if (Arousal_Stimuli_3.Checked) { Arousal_Stimuli = Arousal_Stimuli_3.Text.Trim(); }
            string Arousal_Transition = string.Empty;
            if (Arousal_Transition_1.Checked) { Arousal_Transition = Arousal_Transition_1.Text.Trim(); }
            if (Arousal_Transition_2.Checked) { Arousal_Transition = Arousal_Transition_2.Text.Trim(); }


            string Attention_FocusHandhome = string.Empty;
            if (Attention_FocusHandhome_1.Checked) { Attention_FocusHandhome = Attention_FocusHandhome_1.Text.Trim(); }
            if (Attention_FocusHandhome_2.Checked) { Attention_FocusHandhome = Attention_FocusHandhome_2.Text.Trim(); }
            string Attention_FocusHandSchool = string.Empty;
            if (Attention_FocusHandSchool_1.Checked) { Attention_FocusHandSchool = Attention_FocusHandSchool_1.Text.Trim(); }
            if (Attention_FocusHandSchool_2.Checked) { Attention_FocusHandSchool = Attention_FocusHandSchool_2.Text.Trim(); }
            string Attention_Dividing = string.Empty;
            if (Attention_Dividing_1.Checked) { Attention_Dividing = Attention_Dividing_1.Text.Trim(); }
            if (Attention_Dividing_2.Checked) { Attention_Dividing = Attention_Dividing_2.Text.Trim(); }
            string Attention_AgeAppropriate = string.Empty;
            if (Attention_AgeAppropriate_1.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_1.Text.Trim(); }
            if (Attention_AgeAppropriate_2.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_2.Text.Trim(); }
            if (Attention_AgeAppropriate_3.Checked) { Attention_AgeAppropriate = Attention_AgeAppropriate_3.Text.Trim(); }

            string Attention_move = string.Empty;
            if (Attention_move_1.Checked) { Attention_move = Attention_move_1.Text.Trim(); }
            if (Attention_move_2.Checked) { Attention_move = Attention_move_2.Text.Trim(); }

            string Interacts = string.Empty;
            if (chkInteracts.Checked) { Interacts = chkInteracts.Text.Trim(); }
            string Does_not_initiate = string.Empty;
            if (chkDoes_not_initiate.Checked) { Does_not_initiate = chkDoes_not_initiate.Text.Trim(); }
            string Sustain = string.Empty;
            if (chkSustain.Checked) { Sustain = chkSustain.Text.Trim(); }
            string Fight = string.Empty;
            if (chkFight.Checked) { Fight = chkFight.Text.Trim(); }
            string Freeze = string.Empty;
            if (chkFreeze.Checked) { Freeze = chkFreeze.Text.Trim(); }
            string Fright = string.Empty;
            if (chkFright.Checked) { Fright = chkFright.Text.Trim(); }

            string Anxious = string.Empty;
            if (chkAnxious.Checked) { Anxious = chkAnxious.Text.Trim(); }
            string Comfortable = string.Empty;
            if (chkComfortable.Checked) { Comfortable = chkComfortable.Text.Trim(); }
            string Nervous = string.Empty;
            if (chkNervous.Checked) { Nervous = chkNervous.Text.Trim(); }
            string ANS_response = string.Empty;
            if (chkANS_response.Checked) { ANS_response = chkANS_response.Text.Trim(); }
            string OTHERS = string.Empty;
            if (chkOTHERS.Checked) { OTHERS = chkOTHERS.Text.Trim(); }


            string Interaction_SocialQues = string.Empty;
            if (Interaction_SocialQues_1.Checked) { Interaction_SocialQues = Interaction_SocialQues_1.Text.Trim(); }
            if (Interaction_SocialQues_2.Checked) { Interaction_SocialQues = Interaction_SocialQues_2.Text.Trim(); }


            string Interaction_Friends = string.Empty;
            if (Interaction_Friends_1.Checked) { Interaction_Friends = Interaction_SocialQues_1.Text.Trim(); }
            if (Interaction_Friends_2.Checked) { Interaction_Friends = Interaction_SocialQues_2.Text.Trim(); }

            string Affect_RangeEmotion = string.Empty;
            if (Affect_RangeEmotion_1.Checked) { Affect_RangeEmotion = Affect_RangeEmotion_1.Text.Trim(); }
            if (Affect_RangeEmotion_2.Checked) { Affect_RangeEmotion = Affect_RangeEmotion_2.Text.Trim(); }
            string Affect_ExpressEmotion = string.Empty;
            if (Affect_ExpressEmotion_1.Checked) { Affect_ExpressEmotion = Affect_ExpressEmotion_1.Text.Trim(); }
            if (Affect_ExpressEmotion_2.Checked) { Affect_ExpressEmotion = Affect_ExpressEmotion_2.Text.Trim(); }

            string Action_Purposeful = string.Empty;
            if (Action_Purposeful_1.Checked) { Action_Purposeful = Action_Purposeful_1.Text.Trim(); }
            if (Action_Purposeful_2.Checked) { Action_Purposeful = Action_Purposeful_2.Text.Trim(); }
            string Action_GoalOriented = string.Empty;
            if (Action_GoalOriented_1.Checked) { Action_GoalOriented = Action_GoalOriented_1.Text.Trim(); }
            if (Action_GoalOriented_2.Checked) { Action_GoalOriented = Action_GoalOriented_2.Text.Trim(); }

            string Action_FeedBackDependent = string.Empty;
            if (Action_FeedBackDependent_1.Checked) { Action_FeedBackDependent = Action_FeedBackDependent_1.Text.Trim(); }
            if (Action_FeedBackDependent_2.Checked) { Action_FeedBackDependent = Action_FeedBackDependent_2.Text.Trim(); }

            string Action_Constructive = string.Empty;
            if (Action_Constructive_1.Checked) { Action_Constructive = Action_Constructive_1.Text.Trim(); }
            if (Action_Constructive_2.Checked) { Action_Constructive = Action_Constructive_2.Text.Trim(); }

            string questions = string.Empty;
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    string QuestionNo = ((Label)item.FindControl("lblQuestionNo")).Text;
                    string Yes = ((CheckBox)item.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                    string No = ((CheckBox)item.FindControl("chkMonthNo")).Checked == true ? "1" : "0";
                    string Comment = ((TextBox)item.FindControl("txtMonthComment")).Text;
                    questions += QuestionNo + "$" + Yes + "$" + No + "$" + Comment + "~";
                }
            }
            questions = questions.Remove(questions.Length - 0, 0);

            string ABILITY_questions = string.Empty;
            foreach (RepeaterItem item in abilityQuestionsParent.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rpt = ((Repeater)item.FindControl("abilityQuestionsChild"));

                    if (rpt != null)
                    {
                        foreach (RepeaterItem rptitem in rpt.Items)
                        {
                            string categoryId = ((Label)rptitem.FindControl("lblCategoryId")).Text;
                            string questionNO = ((Label)rptitem.FindControl("abilityquestionNO")).Text;
                            string Yes = ((CheckBox)rptitem.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                            string No = ((CheckBox)rptitem.FindControl("chkMonthNo")).Checked == true ? "1" : "0";

                            ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
                        }
                    }
                }
            }
            if (ABILITY_questions.Length > 0)
            {
                ABILITY_questions = ABILITY_questions.Remove(ABILITY_questions.Length - 1, 1);
                string abilityQuestionsChild = string.Empty;
            }

            int Physioptherapist = 0; if (Doctor_Physioptherapist.SelectedItem != null) { int.TryParse(Doctor_Physioptherapist.SelectedItem.Value, out Physioptherapist); }
            int Occupational = 0; if (Doctor_Occupational.SelectedItem != null) { int.TryParse(Doctor_Occupational.SelectedItem.Value, out Occupational); }

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            int tabValue = 1;

            SqlCommand cmd = new SqlCommand("Rpt_SI_SET_TABWISE");
            cmd.CommandType = CommandType.StoredProcedure;

            switch (this.hfdTabs.Value)
            {

                case "":
                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab":

                    tabValue = 1;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@ClinicalObservation", ClinicleObse_txt.Text);
                    cmd.Parameters.AddWithValue("@ModifyDate", DateTime.UtcNow.AddMinutes(330));
                    cmd.Parameters.AddWithValue("@ModifyBy", _loginID);
                    int HidSI_ID = 0;
                    string Option1 = string.Empty;
                    string Option2 = string.Empty;
                    string Option3 = string.Empty;


                    for (int j = 1; j <= OptionCount; j++)
                    {
                        RepeaterItem item = txtSignleChoice.Items.Count >= j ? txtSignleChoice.Items[j - 1] : null;
                        if (item != null)
                        {
                            HiddenField SI_ID = item.FindControl("txtSI_ID") as HiddenField;
                            TextBox txtTIME = item.FindControl("txtTIME") as TextBox;
                            TextBox txtACTIVITIES = item.FindControl("txtACTIVITIES") as TextBox;
                            TextBox txtCOMMENTS = item.FindControl("txtCOMMENTS") as TextBox;

                            if (txtTIME.Text != "" || txtACTIVITIES.Text != "" || txtCOMMENTS.Text != "")
                            {
                                int.TryParse(SI_ID.Value.ToString(), out HidSI_ID);
                                Option1 = txtTIME.Text.Trim();
                                Option2 = txtACTIVITIES.Text.Trim();
                                Option3 = txtCOMMENTS.Text.Trim();

                                int k = SID.SetTimeLine(_appointmentID, HidSI_ID, Option1, Option2, Option3, DateTime.UtcNow.AddMinutes(330), _loginID);
                            }
                            else if (txtTIME.Text == "" && txtACTIVITIES.Text == "" && txtCOMMENTS.Text == "")
                            {
                                int.TryParse(SI_ID.Value.ToString(), out HidSI_ID);
                                int P = SID.DeleteRow(HidSI_ID);
                            }

                        }
                    }

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    ds = SID.GetsiSubmit(_appointmentID);
                    List<optionMdel> qls = new List<optionMdel>();
                    //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //{

                    //    string txtTIME = string.Empty; string txtACTIVITIES = string.Empty; string txtCOMMENTS = string.Empty;


                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["TIME"].ToString()))
                    //    {
                    //        txtTIME = ds.Tables[1].Rows[i]["TIME"].ToString();
                    //    }
                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["ACTIVITIES"].ToString()))
                    //    {
                    //        txtACTIVITIES = ds.Tables[1].Rows[i]["ACTIVITIES"].ToString();
                    //    }
                    //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["COMMENTS"].ToString()))
                    //    {
                    //        txtCOMMENTS = ds.Tables[1].Rows[i]["COMMENTS"].ToString();
                    //    }


                    //    qls.Add(new optionMdel
                    //    {
                    //        Option = ds.Tables[1].Rows[i]["SI_ID"].ToString(),
                    //        Option1 = txtTIME,
                    //        Option2 = txtACTIVITIES,
                    //        Option3 = txtCOMMENTS,

                    //    });
                    //}


                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {

                        string tIME = string.Empty; string aCTIVITIES = string.Empty; string cOMMENTS = string.Empty;


                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["TIME"].ToString()))
                        {
                            tIME = ds.Tables[7].Rows[i]["TIME"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["ACTIVITIES"].ToString()))
                        {
                            aCTIVITIES = ds.Tables[7].Rows[i]["ACTIVITIES"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[7].Rows[i]["COMMENTS"].ToString()))
                        {
                            cOMMENTS = ds.Tables[7].Rows[i]["COMMENTS"].ToString();
                        }


                        qls.Add(new optionMdel
                        {
                            Option = ds.Tables[7].Rows[i]["SITIME_ID"].ToString(),
                            Option1 = tIME,
                            Option2 = aCTIVITIES,
                            Option3 = cOMMENTS,

                        });
                    }
                    int temp = qls.Count; textVisibleOption.Value = qls.Count.ToString();
                    for (int jl = 0; jl < (OptionCount - temp); jl++)
                    {
                        qls.Add(new optionMdel() { Option = string.Empty });
                    }
                    txtSignleChoice.DataSource = qls;
                    txtSignleChoice.DataBind();

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab";
                    }
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab":
                    #region ===== Tab 2 =====
                    tabValue = 2;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@FamilyStructure_QualityTimeMother", FamilyStructure_QualityTimeMother);
                    cmd.Parameters.AddWithValue("@FamilyStructure_QualityTimeFather", FamilyStructure_QualityTimeFather);
                    cmd.Parameters.AddWithValue("@FamilyStructure_QualityTimeWeekend", FamilyStructure_QualityTimeWeekend);
                    cmd.Parameters.AddWithValue("@Father_Weekends", Father_Weekends);
                    cmd.Parameters.AddWithValue("@FamilyStructure_TimeForThreapy", FamilyStructure_TimeForThreapy);
                    cmd.Parameters.AddWithValue("@FamilyStructure_AcceptanceCondition", FamilyStructure_AcceptanceCondition);
                    cmd.Parameters.AddWithValue("@FamilyStructure_ExtraCaricular", FamilyStructure_ExtraCaricular);
                    cmd.Parameters.AddWithValue("@FamilyStructure_Diciplinary", FamilyStructure_Diciplinary.Text);
                    cmd.Parameters.AddWithValue("@FamilyStructure_SiblingBrother", FamilyStructure_SiblingBrother.Text);
                    cmd.Parameters.AddWithValue("@FamilyStructure_Expectations", FamilyStructure_Expectations.Text);
                    cmd.Parameters.AddWithValue("@FamilyStructure_CloselyInvolved", FamilyStructure_CloselyInvolved.Text);
                    cmd.Parameters.AddWithValue("@FAMILY_cmt", FAMILY_cmt.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab";
                    }

                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab":
                    #region ===== Tab 3 =====
                    tabValue = 3;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Attend", Schoolinfo_Attend);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Type", Schoolinfo_Type);
                    cmd.Parameters.AddWithValue("@Schoolinfo_SchoolHours", Schoolinfo_SchoolHours.SelectedValue);
                    // cmd.Parameters.AddWithValue("@Schoolinfo_Travel", Schoolinfo_Travel.SelectedValue);
                    cmd.Parameters.AddWithValue("@School_Bus", School_Bus);
                    cmd.Parameters.AddWithValue("@Car", Car);
                    cmd.Parameters.AddWithValue("@Two_Wheelers", Two_Wheelers);
                    cmd.Parameters.AddWithValue("@walking", walking);
                    cmd.Parameters.AddWithValue("@Public_Transport", Public_Transport);

                    //cmd.Parameters.AddWithValue("@Schoolinfo_NoOfStudent", Schoolinfo_NoOfStudent);
                    //cmd.Parameters.AddWithValue("@Schoolinfo_SeatingArrangement", Schoolinfo_SeatingArrangement.SelectedValue);
                    cmd.Parameters.AddWithValue("@Floor", Floor);
                    cmd.Parameters.AddWithValue("@single_bench", single_bench);
                    cmd.Parameters.AddWithValue("@bench2", bench2);
                    cmd.Parameters.AddWithValue("@round_table", round_table);
                    cmd.Parameters.AddWithValue("@Schoolinfo_NoOfTeacher", Schoolinfo_NoOfTeacher);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Mealtime", Schoolinfo_Mealtime.SelectedValue);
                    cmd.Parameters.AddWithValue("@Schoolinfo_MealType", Schoolinfo_MealType);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Shareing", Schoolinfo_Shareing);
                    cmd.Parameters.AddWithValue("@Schoolinfo_HelpEating", Schoolinfo_HelpEating);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Friendship", Schoolinfo_Friendship);
                    cmd.Parameters.AddWithValue("@Schoolinfo_InteractionPeer", Schoolinfo_InteractionPeer);
                    cmd.Parameters.AddWithValue("@Schoolinfo_InteractionTeacher", Schoolinfo_InteractionTeacher);
                    cmd.Parameters.AddWithValue("@Schoolinfo_AnnualFunction", Schoolinfo_AnnualFunction);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Sports", Schoolinfo_Sports);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Picnic", Schoolinfo_Picnic);
                    cmd.Parameters.AddWithValue("@Schoolinfo_ExtraCaricular", Schoolinfo_ExtraCaricular);
                    cmd.Parameters.AddWithValue("@Schoolinfo_CopyBoard", Schoolinfo_CopyBoard);
                    cmd.Parameters.AddWithValue("@Schoolinfo_Instructions", Schoolinfo_Instructions);
                    cmd.Parameters.AddWithValue("@Schoolinfo_ShadowTeacher", Schoolinfo_ShadowTeacher);
                    cmd.Parameters.AddWithValue("@Schoolinfo_CW_HW", Schoolinfo_CW_HW);
                    cmd.Parameters.AddWithValue("@Schoolinfo_SpecialEducator", Schoolinfo_SpecialEducator);
                    cmd.Parameters.AddWithValue("@Schoolinfo_DeliveryInformation", Schoolinfo_DeliveryInformation);
                    cmd.Parameters.AddWithValue("@Schoolinfo_RemarkTeacher", Schoolinfo_RemarkTeacher.Text);
                    cmd.Parameters.AddWithValue("@SCHOOL_cmt", SCHOOL_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab":
                    #region ===== Tab 4 =====
                    tabValue = 4;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@PersonalSocial_CurrentPlace", PersonalSocial_CurrentPlace);
                    cmd.Parameters.AddWithValue("@PersonalSocial_WhatHeDoes", PersonalSocial_WhatHeDoes);
                    cmd.Parameters.AddWithValue("@PersonalSocial_BodyAwareness", PersonalSocial_BodyAwareness);
                    cmd.Parameters.AddWithValue("@PersonalSocial_BodySchema", PersonalSocial_BodySchema);
                    cmd.Parameters.AddWithValue("@PersonalSocial_ExploreEnvironment", PersonalSocial_ExploreEnvironment);
                    cmd.Parameters.AddWithValue("@PersonalSocial_Motivated", PersonalSocial_Motivated);
                    cmd.Parameters.AddWithValue("@PersonalSocial_EyeContact", PersonalSocial_EyeContact);
                    cmd.Parameters.AddWithValue("@PersonalSocial_SocialSmile", PersonalSocial_SocialSmile);
                    cmd.Parameters.AddWithValue("@PersonalSocial_FamilyRegards", PersonalSocial_FamilyRegards);
                    cmd.Parameters.AddWithValue("@PersonalSocial_ChildSocially", PersonalSocial_ChildSocially);
                    cmd.Parameters.AddWithValue("@PERSONAL_cmt", PERSONAL_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab":
                    #region ===== Tab 5 =====
                    tabValue = 5;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_StartSpeek", SpeechLanguage_StartSpeek.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Monosyllables", SpeechLanguage_Monosyllables.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Bisyllables", SpeechLanguage_Bisyllables.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_ShrotScentences", SpeechLanguage_ShrotScentences.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_LongScentences", SpeechLanguage_LongScentences.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_UnusualSoundsJargonSpeech", SpeechLanguage_UnusualSoundsJargonSpeech);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_speechgestures", SpeechLanguage_speechgestures);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_NonverbalfacialExpression", SpeechLanguage_NonverbalfacialExpression.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_NonverbalfacialEyeContact", SpeechLanguage_NonverbalfacialEyeContact.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_NonverbalfacialGestures", SpeechLanguage_NonverbalfacialGestures.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_SimpleComplex", SpeechLanguage_SimpleComplex.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_UnderstandImpliedMeaning", SpeechLanguage_UnderstandImpliedMeaning.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_UnderstandJokesarcasm", SpeechLanguage_UnderstandJokesarcasm.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Respondstoname", SpeechLanguage_Respondstoname.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_TwowayInteraction", SpeechLanguage_TwowayInteraction);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_NarrateIncidentsAtSchool", SpeechLanguage_NarrateIncidentsAtSchool.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_NarrateIncidentsAtHome", SpeechLanguage_NarrateIncidentsAtHome.Text);
                    //cmd.Parameters.AddWithValue("@SpeechLanguage_Want", SpeechLanguage_Want.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Needs", SpeechLanguage_Needs.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Emotions", SpeechLanguage_Emotions.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_AchievementsFailure", SpeechLanguage_AchievementsFailure.Text);
                    //cmd.Parameters.AddWithValue("@SpeechLanguage_LanguageSpoken", SpeechLanguage_LanguageSpoken.Text);
                    cmd.Parameters.AddWithValue("@SpeechLanguage_Echolalia", SpeechLanguage_Echolalia.Text);
                    //cmd.Parameters.AddWithValue("@SpeechLanguage_Emotionalmilestones", SpeechLanguage_Emotionalmilestones.Text);
                    cmd.Parameters.AddWithValue("@Speech_cmt", Speech_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab":
                    #region ===== Tab 6 =====
                    tabValue = 6;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Behaviour_FreeTime", Behaviour_FreeTime.Text);
                    cmd.Parameters.AddWithValue("@unassociated", unassociated);
                    cmd.Parameters.AddWithValue("@solitary", solitary);
                    cmd.Parameters.AddWithValue("@onlooker", onlooker);
                    cmd.Parameters.AddWithValue("@parallel", parallel);
                    cmd.Parameters.AddWithValue("@associative", associative);
                    cmd.Parameters.AddWithValue("@cooperative", cooperative);
                    //cmd.Parameters.AddWithValue("@Behaviour_Playbehaviour", Behaviour_Playbehaviour.SelectedValue);
                    cmd.Parameters.AddWithValue("@Behaviour_situationalmeltdowns", Behaviour_situationalmeltdowns);
                    cmd.Parameters.AddWithValue("@BEHAVIOUR_cmt", BEHAVIOUR_cmt.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab":
                    #region ===== Tab 7 =====
                    tabValue = 7;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    string rangevalue = hdnrange.Value.ToString();
                    int range1 = 0;
                    if (!string.IsNullOrEmpty(rangevalue) && rangevalue != "")
                    {
                        int.TryParse(rangevalue, out range1);
                        range1 = range1 / 10;
                    }

                    string rangevalue2 = Hdnrange2.Value.ToString();
                    int range2 = 0;
                    if (!string.IsNullOrEmpty(rangevalue2) && rangevalue2 != "")
                    {
                        int.TryParse(rangevalue2, out range2);
                        range2 = range2 / 10;
                    }


                    //cmd.Parameters.AddWithValue("@Arousal_Evaluation", Arousal_Evaluation.SelectedValue);
                    //cmd.Parameters.AddWithValue("@Arousal_GeneralState", Arousal_GeneralState.SelectedValue);
                    cmd.Parameters.AddWithValue("@rangevalue", range1);
                    cmd.Parameters.AddWithValue("@rangevalue2", range2);
                    cmd.Parameters.AddWithValue("@Arousal_Stimuli", Arousal_Stimuli);
                    cmd.Parameters.AddWithValue("@Arousal_Transition", Arousal_Transition);
                    // cmd.Parameters.AddWithValue("@Arousal_Optimal", Arousal_Optimal.Text);
                    cmd.Parameters.AddWithValue("@Arousal_FactorOCD", Arousal_FactorOCD.Text);
                    cmd.Parameters.AddWithValue("@Arousal_ClaimingFactor", Arousal_ClaimingFactor.Text);
                    cmd.Parameters.AddWithValue("@Arousal_DipsDown", Arousal_DipsDown.Text);
                    cmd.Parameters.AddWithValue("@AROUSAL_cmt", AROUSAL_cmt.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab":
                    #region ===== Tab 8 =====
                    tabValue = 8;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Affect_RangeEmotion", Affect_RangeEmotion);
                    cmd.Parameters.AddWithValue("@Affect_ExpressEmotion", Affect_ExpressEmotion);
                    cmd.Parameters.AddWithValue("@Affect_Environment", Affect_Environment.Text);
                    cmd.Parameters.AddWithValue("@Affect_Task", Affect_Task.Text);
                    cmd.Parameters.AddWithValue("@Affect_Individual", Affect_Individual.Text);
                    cmd.Parameters.AddWithValue("@Affect_ThroughOut", Affect_ThroughOut.Text);
                    cmd.Parameters.AddWithValue("@Affect_Charaterising", Affect_Charaterising.Text);
                    cmd.Parameters.AddWithValue("@Affect_cmt", Affect_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab":
                    #region ===== Tab 9 =====
                    tabValue = 9;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    //cmd.Parameters.AddWithValue("@Attention_Span", Attention_Span.SelectedValue);
                    cmd.Parameters.AddWithValue("@Attention_FocusHandhome", Attention_FocusHandhome);
                    cmd.Parameters.AddWithValue("@Attention_FocusHandSchool", Attention_FocusHandSchool);
                    cmd.Parameters.AddWithValue("@Attention_Dividing", Attention_Dividing);
                    cmd.Parameters.AddWithValue("@Attention_ChangeActivities", Attention_ChangeActivities.Text);
                    cmd.Parameters.AddWithValue("@Attention_AgeAppropriate", Attention_AgeAppropriate);
                    cmd.Parameters.AddWithValue("@Attention_AttentionSpan", Attention_AttentionSpan.Text);
                    cmd.Parameters.AddWithValue("@Attention_Distractibility", Attention_Distractibility.Text);
                    cmd.Parameters.AddWithValue("@Focal_Attention", Focal_Attention.Text);
                    cmd.Parameters.AddWithValue("@Joint_Attention", Joint_Attention.Text);
                    cmd.Parameters.AddWithValue("@Divided_Attention", Divided_Attention.Text);
                    cmd.Parameters.AddWithValue("@Sustained_Attention", Sustained_Attention.Text);
                    cmd.Parameters.AddWithValue("@Alternating_Attention", Alternating_Attention.Text);
                    cmd.Parameters.AddWithValue("@Attention_move", Attention_move);
                    cmd.Parameters.AddWithValue("@ATTENTION_cmt", ATTENTION_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab":
                    #region ===== Tab 10 =====
                    tabValue = 10;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Action_MotorPlanning", Action_MotorPlanning.Text);
                    cmd.Parameters.AddWithValue("@Action_Purposeful", Action_Purposeful);
                    cmd.Parameters.AddWithValue("@Action_GoalOriented", Action_GoalOriented);
                    cmd.Parameters.AddWithValue("@Action_FeedBackDependent", Action_FeedBackDependent);
                    cmd.Parameters.AddWithValue("@Action_Constructive", Action_Constructive);
                    cmd.Parameters.AddWithValue("@Action_cmt", Action_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab":
                    #region ===== Tab 11 =====
                    tabValue = 11;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    ////cmd.Parameters.AddWithValue("@Interaction_KnowPeople", Interaction_KnowPeople.text);
                    //cmd.Parameters.AddWithValue("@Interaction_Strangers", Interaction_Strangers.Text)
                    ////cmd.Parameters.AddWithValue("@Interaction_EmotionalResponse", Interaction_EmotionalResponse.SelectedValue);
                    cmd.Parameters.AddWithValue("@Interacts", Interacts);
                    cmd.Parameters.AddWithValue("@cmtgathering", cmtgathering.Text);
                    cmd.Parameters.AddWithValue("@Does_not_initiate", Does_not_initiate);
                    cmd.Parameters.AddWithValue("@Sustain", Sustain);
                    cmd.Parameters.AddWithValue("@Fight", Fight);
                    cmd.Parameters.AddWithValue("@Freeze", Freeze);
                    cmd.Parameters.AddWithValue("@Fright", Fright);

                    cmd.Parameters.AddWithValue("@Anxious", Anxious);
                    cmd.Parameters.AddWithValue("@Comfortable", Comfortable);
                    cmd.Parameters.AddWithValue("@Nervous", Nervous);
                    cmd.Parameters.AddWithValue("@ANS_response", ANS_response);
                    cmd.Parameters.AddWithValue("@OTHERS", OTHERS);
                    cmd.Parameters.AddWithValue("@Interaction_SocialQues", Interaction_SocialQues);
                    cmd.Parameters.AddWithValue("@Interaction_Happiness", Interaction_Happiness.Text);
                    cmd.Parameters.AddWithValue("@Interaction_Sadness", Interaction_Sadness.Text);
                    cmd.Parameters.AddWithValue("@Interaction_Surprise", Interaction_Surprise.Text);
                    cmd.Parameters.AddWithValue("@Interaction_Shock", Interaction_Shock.Text);
                    cmd.Parameters.AddWithValue("@Interaction_Friends", Interaction_Friends);
                    //cmd.Parameters.AddWithValue("@Interaction_RelatesPeople", Interaction_RelatesPeople);
                    cmd.Parameters.AddWithValue("@Interaction_Enjoy", Interaction_Enjoy.Text);
                    cmd.Parameters.AddWithValue("@INTERACTION_cmt", INTERACTION_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab":
                    #region ===== Tab 12 =====
                    tabValue = 12;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@TS_Registration", TS_Registration.Text);
                    cmd.Parameters.AddWithValue("@TS_Orientation", TS_Orientation.Text);
                    cmd.Parameters.AddWithValue("@TS_Discrimination", TS_Discrimination.Text);
                    cmd.Parameters.AddWithValue("@TS_Responsiveness", TS_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@SS_Bodyawareness", SS_Bodyawareness.Text);
                    cmd.Parameters.AddWithValue("@SS_Bodyschema", SS_Bodyschema.Text);
                    cmd.Parameters.AddWithValue("@SS_Orientation", SS_Orientation.Text);
                    cmd.Parameters.AddWithValue("@SS_Posterior", SS_Posterior.Text);
                    cmd.Parameters.AddWithValue("@SS_Bilateral", SS_Bilateral.Text);
                    cmd.Parameters.AddWithValue("@SS_Balance", SS_Balance.Text);
                    cmd.Parameters.AddWithValue("@SS_Dominance", SS_Dominance.Text);
                    cmd.Parameters.AddWithValue("@SS_Right", SS_Right.Text);
                    cmd.Parameters.AddWithValue("@SS_identifies", SS_identifies.Text);
                    cmd.Parameters.AddWithValue("@SS_point", SS_point.Text);
                    cmd.Parameters.AddWithValue("@SS_Constantly", SS_Constantly.Text);
                    cmd.Parameters.AddWithValue("@SS_clumsy", SS_clumsy.Text);
                    cmd.Parameters.AddWithValue("@SS_maneuver", SS_maneuver.Text);
                    cmd.Parameters.AddWithValue("@SS_overly", SS_overly.Text);
                    cmd.Parameters.AddWithValue("@SS_stand", SS_stand.Text);
                    cmd.Parameters.AddWithValue("@SS_indulge", SS_indulge.Text);
                    cmd.Parameters.AddWithValue("@SS_textures", SS_textures.Text);
                    cmd.Parameters.AddWithValue("@SS_monkey", SS_monkey.Text);
                    cmd.Parameters.AddWithValue("@SS_swings", SS_swings.Text);
                    cmd.Parameters.AddWithValue("@VM_Registration", VM_Registration.Text);
                    cmd.Parameters.AddWithValue("@VM_Orientation", VM_Orientation.Text);
                    cmd.Parameters.AddWithValue("@VM_Discrimination", VM_Discrimination.Text);
                    cmd.Parameters.AddWithValue("@VM_Responsiveness", VM_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@PS_Registration", PS_Registration.Text);
                    cmd.Parameters.AddWithValue("@PS_Gradation", PS_Gradation.Text);
                    cmd.Parameters.AddWithValue("@PS_Discrimination", PS_Discrimination.Text);
                    cmd.Parameters.AddWithValue("@PS_Responsiveness", PS_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@OM_Registration", OM_Registration.Text);
                    cmd.Parameters.AddWithValue("@OM_Orientation", OM_Orientation.Text);
                    cmd.Parameters.AddWithValue("@OM_Discrimination", OM_Discrimination.Text);
                    cmd.Parameters.AddWithValue("@OM_Responsiveness", OM_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@AS_Auditory", AS_Auditory.Text);
                    cmd.Parameters.AddWithValue("@AS_Orientation", AS_Orientation.Text);
                    cmd.Parameters.AddWithValue("@AS_Responsiveness", AS_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@AS_discrimination", AS_discrimination.Text);
                    cmd.Parameters.AddWithValue("@AS_Background", AS_Background.Text);
                    cmd.Parameters.AddWithValue("@AS_localization", AS_localization.Text);
                    cmd.Parameters.AddWithValue("@AS_Analysis", AS_Analysis.Text);
                    cmd.Parameters.AddWithValue("@AS_sequencing", AS_sequencing.Text);
                    cmd.Parameters.AddWithValue("@AS_blending", AS_blending.Text);
                    cmd.Parameters.AddWithValue("@VS_Visual", VS_Visual.Text);
                    cmd.Parameters.AddWithValue("@VS_Responsiveness", VS_Responsiveness.Text);
                    cmd.Parameters.AddWithValue("@VS_scanning", VS_scanning.Text);
                    cmd.Parameters.AddWithValue("@VS_constancy", VS_constancy.Text);
                    cmd.Parameters.AddWithValue("@VS_memory", VS_memory.Text);
                    cmd.Parameters.AddWithValue("@VS_Perception", VS_Perception.Text);
                    cmd.Parameters.AddWithValue("@VS_hand", VS_hand.Text);
                    cmd.Parameters.AddWithValue("@VS_foot", VS_foot.Text);
                    cmd.Parameters.AddWithValue("@VS_discrimination", VS_discrimination.Text);
                    cmd.Parameters.AddWithValue("@VS_closure", VS_closure.Text);
                    cmd.Parameters.AddWithValue("@VS_Figureground", VS_Figureground.Text);
                    cmd.Parameters.AddWithValue("@VS_Visualmemory", VS_Visualmemory.Text);
                    cmd.Parameters.AddWithValue("@VS_sequential", VS_sequential.Text);
                    cmd.Parameters.AddWithValue("@VS_spatial", VS_spatial.Text);
                    cmd.Parameters.AddWithValue("@OS_Registration", OS_Registration.Text);
                    cmd.Parameters.AddWithValue("@OS_Orientation", OS_Orientation.Text);
                    cmd.Parameters.AddWithValue("@OS_Discrimination", OS_Discrimination.Text);
                    cmd.Parameters.AddWithValue("@OS_Responsiveness", OS_Responsiveness.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab":
                    #region ===== Tab 13 =====
                    tabValue = 13;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@TestMeassures_GrossMotor", TestMeassures_GrossMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_FineMotor", TestMeassures_FineMotor.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverLanguage", TestMeassures_DenverLanguage.Text);
                    cmd.Parameters.AddWithValue("@TestMeassures_DenverPersonal", TestMeassures_DenverPersonal.Text);
                    cmd.Parameters.AddWithValue("@Tests_cmt", Tests_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab":
                    #region ===== Tab 14 =====
                    tabValue = 14;


                    //string questions = string.Empty;
                    foreach (RepeaterItem item in rptQuestions.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            string QuestionNo = ((Label)item.FindControl("lblQuestionNo")).Text;
                            string Yes = ((CheckBox)item.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                            string No = ((CheckBox)item.FindControl("chkMonthNo")).Checked == true ? "1" : "0";
                            string Comment = ((TextBox)item.FindControl("txtMonthComment")).Text;
                            questions += QuestionNo + "$" + Yes + "$" + No + "$" + Comment + "~";
                        }
                    }
                    questions = questions.Remove(questions.Length - 0, 0);
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@score_Communication_2", score_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@Inter_Communication_2", Inter_Communication_2.Text);
                    cmd.Parameters.AddWithValue("@GROSS_2", GROSS_2.Text);
                    cmd.Parameters.AddWithValue("@inter_Gross_2 ", inter_Gross_2.Text);
                    cmd.Parameters.AddWithValue("@FINE_2", FINE_2.Text);
                    cmd.Parameters.AddWithValue("@inter_FINE_2", inter_FINE_2.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_2", PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PROBLEM_2", inter_PROBLEM_2.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_2", PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@inter_PERSONAL_2", inter_PERSONAL_2.Text);
                    cmd.Parameters.AddWithValue("@Comm_3", Comm_3.Text);
                    cmd.Parameters.AddWithValue("@inter_3", inter_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_3", GROSS_3.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_3", GROSS_inter_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_3", FINE_3.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_3", FINE_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_3", PROBLEM_3.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_3", PROBLEM_inter_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_3", PERSONAL_3.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_3", PERSONAL_inter_3.Text);
                    cmd.Parameters.AddWithValue("@Communication_6", Communication_6.Text);
                    cmd.Parameters.AddWithValue("@comm_inter_6", comm_inter_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_6", GROSS_6.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_6", GROSS_inter_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_6", FINE_6.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_6", FINE_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_6", PROBLEM_6.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_6", PROBLEM_inter_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_6", PERSONAL_6.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_6", PERSONAL_inter_6.Text);
                    cmd.Parameters.AddWithValue("@comm_7", comm_7.Text);
                    cmd.Parameters.AddWithValue("@inter_7", inter_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_7", GROSS_7.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_7", GROSS_inter_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_7", FINE_7.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_7", FINE_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_7", PROBLEM_7.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_7", PROBLEM_inter_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_7", PERSONAL_7.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_7", PERSONAL_inter_7.Text);
                    cmd.Parameters.AddWithValue("@comm_9", comm_9.Text);
                    cmd.Parameters.AddWithValue("@inter_9", inter_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_9", GROSS_9.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_9", GROSS_inter_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_9", FINE_9.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_9", FINE_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_9", PROBLEM_9.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_9", PROBLEM_inter_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_9", PERSONAL_9.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_9", PERSONAL_inter_9.Text);
                    cmd.Parameters.AddWithValue("@comm_10", comm_10.Text);
                    cmd.Parameters.AddWithValue("@inter_10", inter_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_10 ", GROSS_10.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_10", GROSS_inter_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_10", FINE_10.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_10", FINE_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_10", PROBLEM_10.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_10", PROBLEM_inter_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_10", PERSONAL_10.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_10", PERSONAL_inter_10.Text);
                    cmd.Parameters.AddWithValue("@comm_11", comm_11.Text);
                    cmd.Parameters.AddWithValue("@inter_11", inter_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_11", GROSS_11.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_11", GROSS_inter_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_11", FINE_11.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_11", FINE_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_11", PROBLEM_11.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_11", PROBLEM_inter_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_11", PERSONAL_11.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_11", PERSONAL_inter_11.Text);
                    cmd.Parameters.AddWithValue("@comm_13", comm_13.Text);
                    cmd.Parameters.AddWithValue("@inter_13", inter_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_13", GROSS_13.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_13", GROSS_inter_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_13", FINE_13.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_13", FINE_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_13", PROBLEM_13.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_13", PROBLEM_inter_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_13", PERSONAL_13.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_13", PERSONAL_inter_13.Text);
                    cmd.Parameters.AddWithValue("@comm_15", comm_15.Text);
                    cmd.Parameters.AddWithValue("@inter_15", inter_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_15", GROSS_15.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_15", GROSS_inter_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_15", FINE_15.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_15", FINE_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_15", PROBLEM_15.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_15", PROBLEM_inter_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_15", PERSONAL_15.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_15", PERSONAL_inter_15.Text);
                    cmd.Parameters.AddWithValue("@comm_17", comm_17.Text);
                    cmd.Parameters.AddWithValue("@inter_17", inter_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_17", GROSS_17.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_17", GROSS_inter_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_17", FINE_17.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_17", FINE_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_17", PROBLEM_17.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_17", PROBLEM_inter_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_17", PERSONAL_17.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_17", PERSONAL_inter_17.Text);
                    cmd.Parameters.AddWithValue("@comm_19", comm_19.Text);
                    cmd.Parameters.AddWithValue("@inter_19", inter_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_19", GROSS_19.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_19", GROSS_inter_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_19", FINE_19.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_19", FINE_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_19", PROBLEM_19.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_19", PROBLEM_inter_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_19", PERSONAL_19.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_19", PERSONAL_inter_19.Text);
                    cmd.Parameters.AddWithValue("@comm_21", comm_21.Text);
                    cmd.Parameters.AddWithValue("@inter_21", inter_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_21", GROSS_21.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_21", GROSS_inter_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_21", FINE_21.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_21", FINE_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_21", PROBLEM_21.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_21", PROBLEM_inter_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_21", PERSONAL_21.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_21", PERSONAL_inter_21.Text);
                    cmd.Parameters.AddWithValue("@comm_23", comm_23.Text);
                    cmd.Parameters.AddWithValue("@inter_23", inter_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_23", GROSS_23.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_23", GROSS_inter_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_23", FINE_23.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_23", FINE_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_23", PROBLEM_23.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_23", PROBLEM_inter_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_23", PERSONAL_23.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_23", PERSONAL_inter_23.Text);
                    cmd.Parameters.AddWithValue("@comm_25", comm_25.Text);
                    cmd.Parameters.AddWithValue("@inter_25", inter_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_25", GROSS_25.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_25", GROSS_inter_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_25", FINE_25.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_25", FINE_inter_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_25", PROBLEM_25.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_25", PROBLEM_inter_25.Text);
                    cmd.Parameters.AddWithValue("@comm_28", comm_28.Text);
                    cmd.Parameters.AddWithValue("@inter_28", inter_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_28", GROSS_28.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_28", GROSS_inter_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_28", FINE_28.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_28", FINE_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_28", PROBLEM_28.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_28", PROBLEM_inter_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_28", PERSONAL_28.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_28", PERSONAL_inter_28.Text);
                    cmd.Parameters.AddWithValue("@comm_31", comm_31.Text);
                    cmd.Parameters.AddWithValue("@inter_31", inter_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_31", GROSS_31.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_31", GROSS_inter_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_31", FINE_31.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_31", FINE_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_31", PROBLEM_31.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_31", PROBLEM_inter_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_31", PERSONAL_31.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_31", PERSONAL_inter_31.Text);
                    cmd.Parameters.AddWithValue("@comm_34", comm_34.Text);
                    cmd.Parameters.AddWithValue("@inter_34", inter_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_34", GROSS_34.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_34", GROSS_inter_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_34", FINE_34.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_34", FINE_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_34", PROBLEM_34.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_34", PROBLEM_inter_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_34", PERSONAL_34.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_34", PERSONAL_inter_34.Text);
                    cmd.Parameters.AddWithValue("@comm_42", comm_42.Text);
                    cmd.Parameters.AddWithValue("@inter_42", inter_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_42", GROSS_42.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_42", GROSS_inter_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_42", FINE_42.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_42", FINE_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_42", PROBLEM_42.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_42", PROBLEM_inter_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_42", PERSONAL_42.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_42", PERSONAL_inter_42.Text);
                    cmd.Parameters.AddWithValue("@comm_45", comm_45.Text);
                    cmd.Parameters.AddWithValue("@inter_45", inter_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_45", GROSS_45.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_45", GROSS_inter_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_45   ", FINE_45.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_45   ", FINE_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_45", PROBLEM_45.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_45", PROBLEM_inter_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_45", PERSONAL_45.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_45", PERSONAL_inter_45.Text);
                    cmd.Parameters.AddWithValue("@comm_51", comm_51.Text);
                    cmd.Parameters.AddWithValue("@inter_51", inter_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_51", GROSS_51.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_51", GROSS_inter_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_51", FINE_51.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_51", FINE_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_51", PROBLEM_51.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_51", PROBLEM_inter_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_51", PERSONAL_51.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_51", PERSONAL_inter_51.Text);
                    cmd.Parameters.AddWithValue("@comm_60", comm_60.Text);
                    cmd.Parameters.AddWithValue("@inter_60", inter_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_60", GROSS_60.Text);
                    cmd.Parameters.AddWithValue("@GROSS_inter_60", GROSS_inter_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_60", FINE_60.Text);
                    cmd.Parameters.AddWithValue("@FINE_inter_60", FINE_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_60", PROBLEM_60.Text);
                    cmd.Parameters.AddWithValue("@PROBLEM_inter_60", PROBLEM_inter_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_60", PERSONAL_60.Text);
                    cmd.Parameters.AddWithValue("@PERSONAL_inter_60", PERSONAL_inter_60.Text);

                    cmd.Parameters.AddWithValue("@MONTHS", SelectMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@questions", questions);



                    //cmd.Parameters.AddWithValue("lblQuestionNo", lblQuestionNo.Text);
                    //cmd.Parameters.AddWithValue("chkMonthYes", chkMonthYes);
                    //cmd.Parameters.AddWithValue("chkMonthNo", chkMonthNo);
                    //cmd.Parameters.AddWithValue("txtMonthComment", txtMonthComment.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab":
                    #region ===== Tab 15 =====
                    tabValue = 15;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@General_Processing", General_Processing.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY_Processing", AUDITORY_Processing.Text);
                    cmd.Parameters.AddWithValue("@VISUAL_Processing", VISUAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@TOUCH_Processing", TOUCH_Processing.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT_Processing", MOVEMENT_Processing.Text);
                    cmd.Parameters.AddWithValue("@ORAL_Processing", ORAL_Processing.Text);
                    cmd.Parameters.AddWithValue("@Raw_score", Raw_score.Text);
                    cmd.Parameters.AddWithValue("@Total_rawscore", Total_rawscore.Text);
                    cmd.Parameters.AddWithValue("@Interpretation", Interpretation.Text);
                    cmd.Parameters.AddWithValue("@Comments_1", Comments_1.Text);
                    cmd.Parameters.AddWithValue("@Score_seeking", Score_seeking.Text);
                    cmd.Parameters.AddWithValue("@SEEKING", SEEKING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Avoiding", Score_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@AVOIDING", AVOIDING.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_sensitivity", Score_sensitivity.Text);
                    cmd.Parameters.AddWithValue("@SENSITIVITY_2", SENSITIVITY_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Registration", Score_Registration.Text);
                    cmd.Parameters.AddWithValue("@REGISTRATION", REGISTRATION.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_general", Score_general.Text);
                    cmd.Parameters.AddWithValue("@GENERAL", GENERAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_Auditory", Score_Auditory.Text);
                    cmd.Parameters.AddWithValue("@AUDITORY", AUDITORY.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_visual", Score_visual.Text);
                    cmd.Parameters.AddWithValue("@VISUAL", VISUAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_touch", Score_touch.Text);
                    cmd.Parameters.AddWithValue("@TOUCH", TOUCH.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_movement", Score_movement.Text);
                    cmd.Parameters.AddWithValue("@MOVEMENT", MOVEMENT.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_oral", Score_oral.Text);
                    cmd.Parameters.AddWithValue("@ORAL", ORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Score_behavioural", Score_behavioural.Text);
                    cmd.Parameters.AddWithValue("@BEHAVIORAL", BEHAVIORAL.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_2", Comments_2.Text);
                    cmd.Parameters.AddWithValue("@SPchild_Seeker", SPchild_Seeker.Text);
                    cmd.Parameters.AddWithValue("@Seeking_Seeker", Seeking_Seeker.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Avoider", SPchild_Avoider.Text);
                    cmd.Parameters.AddWithValue("@Avoiding_Avoider", Avoiding_Avoider.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Sensor", SPchild_Sensor.Text);
                    cmd.Parameters.AddWithValue("@Sensitivity_Sensor", Sensitivity_Sensor.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Bystander", SPchild_Bystander.Text);
                    cmd.Parameters.AddWithValue("@Registration_Bystander", Registration_Bystander.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Auditory_3", SPchild_Auditory_3.Text);
                    cmd.Parameters.AddWithValue("@Auditory_3", Auditory_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Visual_3", SPchild_Visual_3.Text);
                    cmd.Parameters.AddWithValue("@Visual_3", Visual_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Touch_3", SPchild_Touch_3.Text);
                    cmd.Parameters.AddWithValue("@Touch_3", Touch_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Movement_3", SPchild_Movement_3.Text);
                    cmd.Parameters.AddWithValue("@Movement_3", Movement_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Body_position", SPchild_Body_position.Text);
                    cmd.Parameters.AddWithValue("@Body_position", Body_position.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Oral_3", SPchild_Oral_3.Text);
                    cmd.Parameters.AddWithValue("@Oral_3", Oral_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Conduct_3", SPchild_Conduct_3.Text);
                    cmd.Parameters.AddWithValue("@Conduct_3", Conduct_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Social_emotional", SPchild_Social_emotional.Text);
                    cmd.Parameters.AddWithValue("@Social_emotional", Social_emotional.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPchild_Attentional_3", SPchild_Attentional_3.Text);
                    cmd.Parameters.AddWithValue("@Attentional_3", Attentional_3.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_3", Comments_3.Text);
                    cmd.Parameters.AddWithValue("@SPAdult_Low_Registration", SPAdult_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration", Low_Registration.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_seeking", SPAdult_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking", Sensory_seeking.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Sensitivity", SPAdult_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity", Sensory_Sensitivity.SelectedValue);
                    cmd.Parameters.AddWithValue("@SPAdult_Sensory_Avoiding", SPAdult_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding", Sensory_Avoiding.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_4", Comments_4.Text);
                    cmd.Parameters.AddWithValue("@SP_Low_Registration64", SP_Low_Registration64.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_5", Low_Registration_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_seeking_64", SP_Sensory_seeking_64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_5", Sensory_seeking_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Sensitivity64", SP_Sensory_Sensitivity64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_5", Sensory_Sensitivity_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@SP_Sensory_Avoiding64", SP_Sensory_Avoiding64.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_5", Sensory_Avoiding_5.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_5", Comments_5.Text);
                    cmd.Parameters.AddWithValue("@Older_Low_Registration", Older_Low_Registration.Text);
                    cmd.Parameters.AddWithValue("@Low_Registration_6", Low_Registration_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_seeking", Older_Sensory_seeking.Text);
                    cmd.Parameters.AddWithValue("@Sensory_seeking_6", Sensory_seeking_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Sensitivity", Older_Sensory_Sensitivity.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Sensitivity_6", Sensory_Sensitivity_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Older_Sensory_Avoiding", Older_Sensory_Avoiding.Text);
                    cmd.Parameters.AddWithValue("@Sensory_Avoiding_6", Sensory_Avoiding_6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Comments_6", Comments_6.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab";
                    }
                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab":
                    #region ===== Tab 16 =====
                    tabValue = 16;

                    //string questions1 = string.Empty;
                    foreach (RepeaterItem item in abilityQuestionsParent.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Repeater rpt = ((Repeater)item.FindControl("abilityQuestionsChild"));

                            if (rpt != null)
                            {
                                foreach (RepeaterItem rptitem in rpt.Items)
                                {
                                    string categoryId = ((Label)rptitem.FindControl("lblCategoryId")).Text;
                                    string questionNO = ((Label)rptitem.FindControl("abilityquestionNO")).Text;
                                    string Yes = ((CheckBox)rptitem.FindControl("chkMonthYes")).Checked == true ? "1" : "0";
                                    string No = ((CheckBox)rptitem.FindControl("chkMonthNo")).Checked == true ? "1" : "0";

                                    ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
                                }
                            }
                        }
                    }
                    if (ABILITY_questions.Length > 0)
                    {
                        ABILITY_questions = ABILITY_questions.Remove(ABILITY_questions.Length - 1, 1);
                        string abilityQuestionsChild = string.Empty;
                    }


                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    //cmd.Parameters.AddWithValue("@updAbility", updAbility);
                    //cmd.Parameters.AddWithValue("@MonthSelect", MonthKSelect.Text);
                    //cmd.Parameters.AddWithValue("@abilityQuestionsParent", abilityQuestionsParent.Text);
                    //cmd.Parameters.AddWithValue("@rptlblCategory", rptlblCategory.Text);
                    //cmd.Parameters.AddWithValue("@abilityQuestionsChild", abilityQuestionsChild.Text);
                    //cmd.Parameters.AddWithValue("@abilityQuestionNo", abilityQuestionNo.Text);
                    //cmd.Parameters.AddWithValue("@lblCategoryId", lblCategoryId.Text);
                    //cmd.Parameters.AddWithValue("@chkMonthYes", chkMonthYes.Text);
                    //cmd.Parameters.AddWithValue("@chkMonthNo", chkMonthNo.Text);
                    cmd.Parameters.AddWithValue("@ABILITY_months", MonthSelect.Text);
                    cmd.Parameters.AddWithValue("@ability_TOTAL", ability_TOTAL.Text);
                    cmd.Parameters.AddWithValue("@ability_COMMENTS", ability_COMMENTS.Text);
                    cmd.Parameters.AddWithValue("@ABILITY_questions", ABILITY_questions);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab";
                    }
                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab":
                    #region ===== Tab 17 =====
                    tabValue = 17;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@DCDQ_Throws1", DCDQ_Throws1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Throws2", DCDQ_Throws2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Throws3", DCDQ_Throws3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Catches1", DCDQ_Catches1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Catches2", DCDQ_Catches2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Catches3", DCDQ_Catches3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Hits1", DCDQ_Hits1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Hits2", DCDQ_Hits2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Hits3", DCDQ_Hits3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Jumps1", DCDQ_Jumps1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Jumps2", DCDQ_Jumps2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Jumps3", DCDQ_Jumps3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Runs1", DCDQ_Runs1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Runs2", DCDQ_Runs2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Runs3", DCDQ_Runs3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Plans1", DCDQ_Plans1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Plans2", DCDQ_Plans2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Plans3", DCDQ_Plans3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Writing1", DCDQ_Writing1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Writing2", DCDQ_Writing2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Writing3", DCDQ_Writing3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_legibly1", DCDQ_legibly1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_legibly2", DCDQ_legibly2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_legibly3", DCDQ_legibly3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Effort1", DCDQ_Effort1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Effort2", DCDQ_Effort2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Effort3", DCDQ_Effort3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Cuts1", DCDQ_Cuts1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Cuts2", DCDQ_Cuts2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Cuts3", DCDQ_Cuts3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Likes1", DCDQ_Likes1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Likes2", DCDQ_Likes2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Likes3", DCDQ_Likes3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Learning1", DCDQ_Learning1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Learning2", DCDQ_Learning2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Learning3", DCDQ_Learning3.Text);

                    cmd.Parameters.AddWithValue("@DCDQ_Quick1", DCDQ_Quick1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Quick2", DCDQ_Quick2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Quick3", DCDQ_Quick3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Bull1", DCDQ_Likes3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Bull2", DCDQ_Bull2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Bull3", DCDQ_Bull3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Does1", DCDQ_Does1.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Does2", DCDQ_Does2.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Does3", DCDQ_Does3.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Control", DCDQ_Control.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Fine", DCDQ_Fine.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_General", DCDQ_General.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_Total", DCDQ_Total.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_INTERPRETATION", DCDQ_INTERPRETATION.Text);
                    cmd.Parameters.AddWithValue("@DCDQ_COMMENT", DCDQ_COMMENT.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab";
                    }
                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab":
                    #region ===== Tab 18 =====
                    tabValue = 18;

                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@SIPTInfo_History", @SIPTInfo_History.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GraspRight", @SIPTInfo_HandFunction1_GraspRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GraspLeft", @SIPTInfo_HandFunction1_GraspLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_SphericalRight", @SIPTInfo_HandFunction1_SphericalRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_SphericalLeft", @SIPTInfo_HandFunction1_SphericalLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_HookRight", @SIPTInfo_HandFunction1_HookRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_HookLeft", @SIPTInfo_HandFunction1_HookLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_JawChuckRight", @SIPTInfo_HandFunction1_JawChuckRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_JawChuckLeft", @SIPTInfo_HandFunction1_JawChuckLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GripRight", @SIPTInfo_HandFunction1_GripRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_GripLeft", @SIPTInfo_HandFunction1_GripLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_ReleaseRight", @SIPTInfo_HandFunction1_ReleaseRight.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction1_ReleaseLeft", @SIPTInfo_HandFunction1_ReleaseLeft.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionLfR", @SIPTInfo_HandFunction2_OppositionLfR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionLfL", @SIPTInfo_HandFunction2_OppositionLfL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionMFR", @SIPTInfo_HandFunction2_OppositionMFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionMFL", @SIPTInfo_HandFunction2_OppositionMFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionRFR", @SIPTInfo_HandFunction2_OppositionRFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_OppositionRFL", @SIPTInfo_HandFunction2_OppositionRFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchLfR", @SIPTInfo_HandFunction2_PinchLfR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchLfL", @SIPTInfo_HandFunction2_PinchLfL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchMFR", @SIPTInfo_HandFunction2_PinchMFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchMFL", @SIPTInfo_HandFunction2_PinchMFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchRFR", @SIPTInfo_HandFunction2_PinchRFR.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_HandFunction2_PinchRFL", @SIPTInfo_HandFunction2_PinchRFL.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT3_Spontaneous", @SIPTInfo_SIPT3_Spontaneous.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT3_Command", @SIPTInfo_SIPT3_Command.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Kinesthesia", @SIPTInfo_SIPT4_Kinesthesia.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Finger", @SIPTInfo_SIPT4_Finger.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Localisation", @SIPTInfo_SIPT4_Localisation.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_DoubleTactile", @SIPTInfo_SIPT4_DoubleTactile.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Tactile", @SIPTInfo_SIPT4_Tactile.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Graphesthesia", @SIPTInfo_SIPT4_Graphesthesia.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_PostRotary", @SIPTInfo_SIPT4_PostRotary.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT4_Standing", @SIPTInfo_SIPT4_Standing.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Color", @SIPTInfo_SIPT5_Color.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Form", @SIPTInfo_SIPT5_Form.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Size", @SIPTInfo_SIPT5_Size.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Depth", @SIPTInfo_SIPT5_Depth.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Figure", @SIPTInfo_SIPT5_Figure.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT5_Motor", @SIPTInfo_SIPT5_Motor.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT6_Design", @SIPTInfo_SIPT6_Design.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT6_Constructional", @SIPTInfo_SIPT6_Constructional.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT7_Scanning", @SIPTInfo_SIPT7_Scanning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT7_Memory", @SIPTInfo_SIPT7_Memory.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Postural", @SIPTInfo_SIPT8_Postural.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Oral", @SIPTInfo_SIPT8_Oral.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Sequencing", @SIPTInfo_SIPT8_Sequencing.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT8_Commands", @SIPTInfo_SIPT8_Commands.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_Bilateral", @SIPTInfo_SIPT9_Bilateral.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_Contralat", @SIPTInfo_SIPT9_Contralat.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_PreferredHand", @SIPTInfo_SIPT9_PreferredHand.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT9_CrossingMidline", @SIPTInfo_SIPT9_CrossingMidline.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Draw", @SIPTInfo_SIPT10_Draw.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_ClockFace", @SIPTInfo_SIPT10_ClockFace.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Filtering", @SIPTInfo_SIPT10_Filtering.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_MotorPlanning", @SIPTInfo_SIPT10_MotorPlanning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_BodyImage", @SIPTInfo_SIPT10_BodyImage.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_BodySchema", @SIPTInfo_SIPT10_BodySchema.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_SIPT10_Laterality", @SIPTInfo_SIPT10_Laterality.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Remark", @SIPTInfo_ActivityGiven_Remark.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_InterestActivity", @SIPTInfo_ActivityGiven_InterestActivity.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_InterestCompletion", @SIPTInfo_ActivityGiven_InterestCompletion.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Learning", @SIPTInfo_ActivityGiven_Learning.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Complexity", @SIPTInfo_ActivityGiven_Complexity.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_ProblemSolving", @SIPTInfo_ActivityGiven_ProblemSolving.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Concentration", @SIPTInfo_ActivityGiven_Concentration.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Retension", @SIPTInfo_ActivityGiven_Retension.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SpeedPerfom", @SIPTInfo_ActivityGiven_SpeedPerfom.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Neatness", @SIPTInfo_ActivityGiven_Neatness.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Frustation", @SIPTInfo_ActivityGiven_Frustation.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Work", @SIPTInfo_ActivityGiven_Work.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_Reaction", @SIPTInfo_ActivityGiven_Reaction.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SociabilityTherapist", @SIPTInfo_ActivityGiven_SociabilityTherapist.Text);
                    cmd.Parameters.AddWithValue("@SIPTInfo_ActivityGiven_SociabilityStudents", @SIPTInfo_ActivityGiven_SociabilityStudents.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    //if (this.hfdCallFrom.Value == "Tab")
                    //{
                    //    this.hfdTabs.Value = this.hfdCurTab.Value;
                    //    this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab";
                    //}
                    //else
                    //{
                    //    this.tb_Contents.ActiveTabIndex = tabValue;
                    //    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab";
                    //}
                    hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab";

                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab":
                    #region ===== Tab 19 =====
                    tabValue = 19;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    //////cmd.Parameters.AddWithValue("@TabContainer3", TabContainer3);
                    //////cmd.Parameters.AddWithValue("@TabPanel17", TabPanel17);
                    cmd.Parameters.AddWithValue("@Evaluation_Strengths", @Evaluation_Strengths.Text);
                    //cmd.Parameters.AddWithValue("@TabPanel18", TabPanel18);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Barriers", @Evaluation_Concern_Barriers.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Limitations", @Evaluation_Concern_Limitations.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Posture", @Evaluation_Concern_Posture.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Concern_Impairment", @Evaluation_Concern_Impairment.Text);
                    ////cmd.Parameters.AddWithValue("@TabPanel113", TabPanel113);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Summary", @Evaluation_Goal_Summary.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Previous", @Evaluation_Goal_Previous.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_LongTerm", @Evaluation_Goal_LongTerm.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_ShortTerm", @Evaluation_Goal_ShortTerm.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Goal_Impairment", @Evaluation_Goal_Impairment.Text);
                    ////cmd.Parameters.AddWithValue("@TabPanel114", TabPanel114);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Frequency", @Evaluation_Plan_Frequency.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Service", @Evaluation_Plan_Service.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Strategies", @Evaluation_Plan_Strategies.Text);
                    cmd.Parameters.AddWithValue("@Evaluation_Plan_Equipment", @Evaluation_Plan_Equipment.Text);



                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    //if (this.hfdCallFrom.Value == "Tab")
                    //{
                    //    this.hfdTabs.Value = this.hfdCurTab.Value;
                    //    this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab";
                    //}
                    //else
                    //{
                    //    this.tb_Contents.ActiveTabIndex = tabValue;
                    //    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab";
                    //}
                    #endregion
                    break;



                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab":
                    #region ===== Tab 20 =====
                    tabValue = 20;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Treatment_Home", Treatment_Home.Text);
                    cmd.Parameters.AddWithValue("@Treatment_School", Treatment_School.Text);
                    cmd.Parameters.AddWithValue("@Treatment_Threapy", Treatment_Threapy.Text);
                    cmd.Parameters.AddWithValue("@Treatment_cmt", Treatment_cmt.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);


                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report22_tab";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report22_tab";
                    }
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report22_tab":
                    #region ===== Tab 21 =====

                    tabValue = 21;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Doctor_Physioptherapist", @Doctor_Physioptherapist.Text);
                    cmd.Parameters.AddWithValue("@Doctor_Occupational", @Doctor_Occupational.Text);
                    //cmd.Parameters.AddWithValue("@Doctor_EnterReport", @Doctor_EnterReport.Text);
                    cmd.Parameters.AddWithValue("@Doctor_EnterReport", DBNull.Value);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    if (this.hfdCallFrom.Value == "Tab")
                    {
                        this.hfdTabs.Value = this.hfdCurTab.Value;
                        this.hfdCallFrom.Value = "";
                    }
                    else
                    {
                        this.tb_Contents.ActiveTabIndex = tabValue - 21;
                        this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab";
                    }
                    #endregion
                    break;
            }

        }

        protected void SelectMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //SqlConnection conn = new SqlConnection("Data Source=184.154.187.166;Initial Catalog=demo2;User ID=demo2;Password=Sneh#123db");
                string que = "SELECT QUESTIONS, QuestionNo, 0 as Yes, 0 as No, '' as Comments FROM QUESTIONNAIRE_SI WHERE MONTHS = " + SelectMonth.SelectedValue + "";
                SqlCommand cmd = new SqlCommand(que, conn);
                conn.Open();
                DataSet dss = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dss);
                conn.Close();

                DataTable dt = Session["AgeState"] as DataTable;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string _month = SelectMonth.SelectedValue;
                        string[] Ques = dt.Rows[i]["QUESTIONS"].ToString().Split('~');
                        if (dt.Rows[i]["MONTHS"].ToString() == _month)
                        {
                            for (int j = 0; j < Ques.Length; j++)
                            {
                                DataRow dr = (dss.Tables[0].AsEnumerable().Where(w => w.Field<int>("QuestionNo").ToString() == Ques[j].Split('$')[0].ToString())).FirstOrDefault();
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

                                }
                            }
                        }
                    }
                }

                rptQuestions.DataSource = dss.Tables[0];
                rptQuestions.DataBind();
                //Session["AgeState"] = null;
            }
            catch (Exception ex)
            {
            }

        }
        protected void MonthSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string categoryId = string.Empty;
                string questionNo = string.Empty;
                string yes = string.Empty;
                string No = string.Empty;
                //SqlConnection conn1 = new SqlConnection("Data Source=184.154.187.166;Initial Catalog=demo2;User ID=demo2;Password=Sneh#123db");
                string que1 = "SELECT DISTINCT CATEGORYID, CATEGORY_NAME FROM ABILITY_CHECKLIST_New WHERE MonthsQ = " + MonthSelect.SelectedValue + "; SELECT questionNO, Question, CategoryID, category_name,0 as Yes ,0 as No FROM ABILITY_CHECKLIST_New  WHERE MonthsQ = " + MonthSelect.SelectedValue + "";
                SqlCommand cmd1 = new SqlCommand(que1, conn);
                conn.Open();
                DataSet dss1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dss1);
                conn.Close();

                dss1.Relations.Add(new DataRelation("CategoriesRelation", dss1.Tables[0].Columns["CATEGORYID"], dss1.Tables[1].Columns["CATEGORYID"]));

                abilityQuestionsParent.DataSource = dss1.Tables[0];
                abilityQuestionsParent.DataBind();
                //Session["Ability"] = null;

                DataTable dt = Session["Ability"] as DataTable;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] Ques1 = dt.Rows[i]["ABILITY_questions"].ToString().Split('~');

                        for (int j = 0; j < Ques1.Length; j++)
                        {
                            categoryId = Ques1[j].Split('#')[0].ToString();
                            questionNo = Ques1[j].Split('#')[1].ToString().Split('$')[0].ToString();
                            yes = Ques1[j].Split('#')[1].ToString().Split('$')[1].ToString();
                            No = Ques1[j].Split('#')[1].ToString().Split('$')[2].ToString();

                            DataRow dr = (dss1.Tables[1].AsEnumerable().Where(w => w.Field<int>("CategoryID").ToString() == categoryId && w.Field<int>("questionNO").ToString() == questionNo)).FirstOrDefault();

                            if (yes == "1")
                            {
                                if (dr != null)
                                {
                                    dr["Yes"] = 1;
                                    dr["No"] = 0;
                                }
                            }
                            else if (No == "1")
                            {
                                if (dr != null)
                                {
                                    dr["No"] = 1;
                                    dr["Yes"] = 0;
                                }
                            }
                        }
                    }
                    abilityQuestionsParent.DataSource = dss1.Tables[0];
                    abilityQuestionsParent.DataBind();
                    //Session["Ability"] = null;
                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void abilityQuestionsParent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Repeater abilityQuestionsChild = e.Item.FindControl("abilityQuestionsChild") as Repeater;
                abilityQuestionsChild.DataSource = drv.CreateChildView("CategoriesRelation");
                abilityQuestionsChild.DataBind();
            }



            //DataTable dt = Session["Ability"] as DataTable;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ 

            //}
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    var chkMonthYes = (CheckBox)e.Item.FindControl("chkMonthYes");
            //    var chkMonthNo = (CheckBox)e.Item.FindControl("chkMonthNo");
            //    checkBox.Checked = true;
            //}
        }
        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //DataTable dt = Session["AgeState"] as DataTable;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ 

            //}
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    var chkMonthYes = (CheckBox)e.Item.FindControl("chkMonthYes");
            //    var chkMonthNo = (CheckBox)e.Item.FindControl("chkMonthNo");
            //    checkBox.Checked = true;
            //}
        }

        protected void chkMonthYes_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkMonthNo_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}