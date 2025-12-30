using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.SessionRpt
{
    public partial class Demo_EIPRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = ""; string Demo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (!IsPostBack)
            {
                LoadForm();
            }
        }

        private void LoadForm()
        {
            SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
            Doctor_Physioptherapist.Items.Clear(); Doctor_Physioptherapist.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_Occupational.Items.Clear(); Doctor_Occupational.Items.Add(new ListItem("Select Doctor", "-1"));
            Doctor_EnterReport.Items.Clear(); Doctor_EnterReport.Items.Add(new ListItem("Select Doctor", "-1"));
            foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
            {
                Doctor_Physioptherapist.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_Occupational.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                Doctor_EnterReport.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
            }

            SnehBLL.ReportEIPMst_Bll RDB = new SnehBLL.ReportEIPMst_Bll();

            //if (!RDB.IsValid(_appointmentID))
            //{
            //    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            //}

            //DataSet ds = RDB.Get(_appointmentID);

            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        txtPatient.Text = ds.Tables[0].Rows[0]["FullName"].ToString();
            //        txtSession.Text = ds.Tables[0].Rows[0]["SessionName"].ToString();
            //    }
            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        DataCollection_EDD.Text = ds.Tables[1].Rows[0]["DataCollection_EDD"].ToString();
            //        DataCollection_CGA.Text = ds.Tables[1].Rows[0]["DataCollection_CGA"].ToString();
            //        txtbirthhistory_nc.Text = ds.Tables[1].Rows[0]["BirthHistory_NC_SecDelivery"].ToString();
            //        txtbirthhistory_prenatal.Text = ds.Tables[1].Rows[0]["BirthHistory_PreNatal_MaterialHistory"].ToString();
            //        txtbirthhistorynatal.Text = ds.Tables[1].Rows[0]["BirthHistory_Natal"].ToString();
            //        txtbirthhistory_postnatal.Text = ds.Tables[1].Rows[0]["BirthHistory_PostNatal"].ToString();
            //        txtobservationhr.Text = ds.Tables[1].Rows[0]["Observation_Autonomic_HR"].ToString();
            //        txtobservationrespiration.Text = ds.Tables[1].Rows[0]["Observation_Autonomic_TypesOfRespiration"].ToString();
            //        txtobservationskincolor.Text = ds.Tables[1].Rows[0]["Observation_Autonomic_SkinColour"].ToString();
            //        txtobservationtemperature.Text = ds.Tables[1].Rows[0]["Observation_Autonomic_TemperatureCentral_Peripheral"].ToString();
            //        txtobservationMotor.Text = ds.Tables[1].Rows[0]["Observation_Motor"].ToString();
            //        txtobservationUpperLimbLevel1.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLevel1"].ToString();
            //        txtobservationUpperLimbLeft1.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLeft1"].ToString();
            //        txtobservationUpperLimbRight1.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbRight1"].ToString();
            //        txtobservationUpperLimbLevel2.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLevel2"].ToString();
            //        txtobservationUpperLimbLeft2.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLeft2"].ToString();
            //        txtobservationUpperLimbRight2.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbRight2"].ToString();
            //        txtobservationUpperLimbLevel3.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLevel3"].ToString();
            //        txtobservationUpperLimbLeft3.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbLeft3"].ToString();
            //        txtobservationUpperLimbRight3.Text = ds.Tables[1].Rows[0]["Observation_UpperLimbRight3"].ToString();
            //        txtobservationLowerLimbLevel1.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLevel1"].ToString();
            //        txtobservationLowerLimbLeft1.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLeft1"].ToString();
            //        txtobservationLowerLimbRight1.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbRight1"].ToString();
            //        txtobservationLowerLimbLevel2.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLevel2"].ToString();
            //        txtobservationLowerLimbLeft2.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLeft2"].ToString();
            //        txtobservationLowerLimbRight2.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbRight2"].ToString();
            //        txtobservationLowerLimbLevel3.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLevel3"].ToString();
            //        txtobservationLowerLimbLeft3.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbLeft3"].ToString();
            //        txtobservationLowerLimbRight3.Text = ds.Tables[1].Rows[0]["Observation_LowerLimbRight3"].ToString();
            //        txtobservationtrunk.Text = ds.Tables[1].Rows[0]["Observation_Trunk"].ToString();
            //        txtobservationgeneralposture.Text = ds.Tables[1].Rows[0]["Observation_GeneralPosture"].ToString();
            //        txtobservationsocialinteraction.Text = ds.Tables[1].Rows[0]["Observation_SocialInteraction_Responsivity"].ToString();
            //        txtobservationFeeding.Text = ds.Tables[1].Rows[0]["Observation_Feeding"].ToString();
            //        txtobservationParticipation.Text = ds.Tables[1].Rows[0]["Observation_Participation"].ToString();
            //        txtobservationParticipationRestriction.Text = ds.Tables[1].Rows[0]["Observation_Participation_Restriction"].ToString();
            //        txtExaminationBallards1.Text = ds.Tables[1].Rows[0]["Examination_Ballards1"].ToString();
            //        txtExaminationBallards2.Text = ds.Tables[1].Rows[0]["Examination_Ballards2"].ToString();
            //        txtExaminationBallards3.Text = ds.Tables[1].Rows[0]["Examination_Ballards3"].ToString();
            //        txtExaminationBallards4.Text = ds.Tables[1].Rows[0]["Examination_Ballards4"].ToString();
            //        txtExaminationBallards5.Text = ds.Tables[1].Rows[0]["Examination_Ballards5"].ToString();
            //        txtExaminationBallards6.Text = ds.Tables[1].Rows[0]["Examination_Ballards6"].ToString();
            //        txtExaminationBallards7.Text = ds.Tables[1].Rows[0]["Examination_Ballards7"].ToString();
            //        txtExaminationBallards8.Text = ds.Tables[1].Rows[0]["Examination_Ballards8"].ToString();
            //        txtExaminationBallards9.Text = ds.Tables[1].Rows[0]["Examination_Ballards9"].ToString();
            //        txtExaminationBallards10.Text = ds.Tables[1].Rows[0]["Examination_Ballards10"].ToString();
            //        txtExaminationBallards11.Text = ds.Tables[1].Rows[0]["Examination_Ballards11"].ToString();
            //        txtExaminationBallards12.Text = ds.Tables[1].Rows[0]["Examination_Ballards12"].ToString();
            //        txtExaminationTimp1.Text = ds.Tables[1].Rows[0]["Examination_Timp1"].ToString();
            //        txtExaminationTimp2.Text = ds.Tables[1].Rows[0]["Examination_Timp2"].ToString();
            //        txtExaminationTimp3.Text = ds.Tables[1].Rows[0]["Examination_Timp3"].ToString();
            //        txtExaminationTimp4.Text = ds.Tables[1].Rows[0]["Examination_Timp4"].ToString();
            //        txtExaminationTimp5.Text = ds.Tables[1].Rows[0]["Examination_Timp5"].ToString();
            //        txtExaminationTimp6.Text = ds.Tables[1].Rows[0]["Examination_Timp6"].ToString();
            //        txtExaminationTimp7.Text = ds.Tables[1].Rows[0]["Examination_Timp7"].ToString();
            //        txtExaminationTimp8.Text = ds.Tables[1].Rows[0]["Examination_Timp8"].ToString();
            //        txtExaminationVoitas1.Text = ds.Tables[1].Rows[0]["Examination_Voitas1"].ToString();
            //        txtExaminationVoitas2.Text = ds.Tables[1].Rows[0]["Examination_Voitas2"].ToString();
            //        txtExaminationVoitas3.Text = ds.Tables[1].Rows[0]["Examination_Voitas3"].ToString();
            //        txtExaminationVoitas4.Text = ds.Tables[1].Rows[0]["Examination_Voitas4"].ToString();
            //        txtExaminationVoitas5.Text = ds.Tables[1].Rows[0]["Examination_Voitas5"].ToString();
            //        txtExaminationVoitas6.Text = ds.Tables[1].Rows[0]["Examination_Voitas6"].ToString();
            //        txtExaminationVoitas7.Text = ds.Tables[1].Rows[0]["Examination_Voitas7"].ToString();
            //        txtExaminationVoitas8.Text = ds.Tables[1].Rows[0]["Examination_Voitas8"].ToString();
            //        txtExaminationgoalstreatment.Text = ds.Tables[1].Rows[0]["Examination_GoalsOf_Treatment"].ToString();
            //        txtExaminationtreatmentgiven.Text = ds.Tables[1].Rows[0]["Examination_TreatmentGiven"].ToString();
            //        bool IsFinal = false; bool.TryParse(ds.Tables[1].Rows[0]["IsFinal"].ToString(), out IsFinal);
            //        txtFinal.Checked = IsFinal;
            //        bool IsGiven = false; bool.TryParse(ds.Tables[1].Rows[0]["IsGiven"].ToString(), out IsGiven);
            //        txtGiven.Checked = IsGiven;
            //        DateTime _givenDate = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["GivenDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _givenDate);
            //        if (_givenDate > DateTime.MinValue)
            //        {
            //            txtGivenDate.Text = _givenDate.ToString(DbHelper.Configuration.showDateFormat);
            //        }
            //        int Physioptherapist = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Physioptherapist"].ToString(), out Physioptherapist);
            //        if (Doctor_Physioptherapist.Items.FindByValue(Physioptherapist.ToString()) != null)
            //        {
            //            Doctor_Physioptherapist.SelectedValue = Physioptherapist.ToString();
            //        }
            //        int Occupational = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_Occupational"].ToString(), out Occupational);
            //        if (Doctor_Occupational.Items.FindByValue(Occupational.ToString()) != null)
            //        {
            //            Doctor_Occupational.SelectedValue = Occupational.ToString();
            //        }
            //        int EnterReport = 0; int.TryParse(ds.Tables[1].Rows[0]["Doctor_EnterReport"].ToString(), out EnterReport);
            //        if (Doctor_EnterReport.Items.FindByValue(EnterReport.ToString()) != null)
            //        {
            //            Doctor_EnterReport.SelectedValue = EnterReport.ToString();
            //        }
            //        txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
            //    }
            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
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
        }
    }
}