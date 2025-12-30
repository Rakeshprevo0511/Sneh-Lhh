using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static snehrehab.rModel;
using System.Text;
using System.Data.SqlClient;

namespace snehrehab.SessionRpt
{
    public partial class PreConsultRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        public int OptionCount = 30;

        protected void Page_Load(object sender, EventArgs e)
        {
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
            _cancelUrl = "/Reports/PreConsultation.aspx";
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
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            List<optionMdel> ql = new List<optionMdel>();
            for (int i = 1; i <= OptionCount; i++) { ql.Add(new optionMdel() { }); }
            txtSignleChoice.DataSource = ql;
            txtSignleChoice.DataBind();
            if (!RDB.IsValid(_appointmentID))
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            }
            DataSet ds = RDB.Get_New(_appointmentID);
            if (ds.Tables.Count > 0)
            {
                bool HasDiagnosisID = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPatient.Text = ds.Tables[0].Rows[0]["FullName"].ToString();
                    txtSession.Text = ds.Tables[0].Rows[0]["SessionName"].ToString();
                    bool.TryParse(ds.Tables[0].Rows[0]["HasDiagnosisID"].ToString(), out HasDiagnosisID);
                    DateTime DatePreConsult = new DateTime(); DateTime.TryParseExact(ds.Tables[0].Rows[0]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatePreConsult);
                    if (DatePreConsult > DateTime.MinValue)
                    {
                        txtDatepreConsult.Text = DatePreConsult.ToString(DbHelper.Configuration.showDateFormat);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    bool IsFinal = false; bool.TryParse(ds.Tables[1].Rows[0]["IsFinal"].ToString(), out IsFinal);
                    txtFinal.Checked = IsFinal;
                    bool IsGiven = false; bool.TryParse(ds.Tables[1].Rows[0]["IsGiven"].ToString(), out IsGiven);
                    txtGiven.Checked = IsGiven;
                    DateTime _givenDate = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["GivenDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _givenDate);
                    if (_givenDate > DateTime.MinValue)
                    {
                        txtGivenDate.Text = _givenDate.ToString(DbHelper.Configuration.showDateFormat);
                    }
                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?type=8&record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
                    //DateTime DatePreConsult = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["DatepreConsult"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatePreConsult);
                    //if (DatePreConsult > DateTime.MinValue)
                    //{
                    //    txtDatepreConsult.Text = DatePreConsult.ToString(DbHelper.Configuration.showDateFormat);
                    //}
                    txtComfortableLanguage.Text = ds.Tables[1].Rows[0]["ComfortableLanguage"].ToString();
                    DateTime BirthDate = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["DateBirth"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                    if (BirthDate > DateTime.MinValue)
                    {
                        txtDateBirth.Text = BirthDate.ToString(DbHelper.Configuration.showDateFormat);
                    }
                    DateTime DeliveryDate = new DateTime(); DateTime.TryParseExact(ds.Tables[1].Rows[0]["DateDelivery"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DeliveryDate);
                    if (DeliveryDate > DateTime.MinValue)
                    {
                        txtDateofDelivery.Text = DeliveryDate.ToString(DbHelper.Configuration.showDateFormat);
                    }
                    txtCorrectAge.Text = ds.Tables[1].Rows[0]["CorrectAge"].ToString();
                    txtAge.Text = ds.Tables[1].Rows[0]["Age"].ToString();
                    CheckFemale.Checked = false; CheckMale.Checked = false; CheckOther.Checked = false;
                    if (ds.Tables[1].Rows[0]["Gender"].ToString().Equals(CheckFemale.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckFemale.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Gender"].ToString().Equals(CheckMale.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckMale.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Gender"].ToString().Equals(CheckOther.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckOther.Checked = true;
                    }
                    txtMotherName.Text = ds.Tables[1].Rows[0]["MotherName"].ToString();
                    txtMotherAge.Text = ds.Tables[1].Rows[0]["MotherAge"].ToString();
                    txtMotherQualification.Text = ds.Tables[1].Rows[0]["MotherQualification"].ToString();
                    txtMotherOccupation.Text = ds.Tables[1].Rows[0]["MotherOccupation"].ToString();
                    txtMotherWorkingHour.Text = ds.Tables[1].Rows[0]["MotherWorkingHour"].ToString();
                    txtFatherName.Text = ds.Tables[1].Rows[0]["FatherName"].ToString();
                    txtFatherAge.Text = ds.Tables[1].Rows[0]["FatherAge"].ToString();
                    txtFatherOccupation.Text = ds.Tables[1].Rows[0]["FatherOccupation"].ToString();
                    txtFatherQualification.Text = ds.Tables[1].Rows[0]["FatherQualification"].ToString();
                    txtFatherWorkingHour.Text = ds.Tables[1].Rows[0]["FatherWorkingHour"].ToString();
                    txtAddress.Text = ds.Tables[1].Rows[0]["Address"].ToString();
                    txtContactDetails.Text = ds.Tables[1].Rows[0]["ContactDetails"].ToString();
                    txtEmailID.Text = ds.Tables[1].Rows[0]["EmailID"].ToString();
                    txtReferredBy.Text = ds.Tables[1].Rows[0]["ReferredBy"].ToString();
                    txtTherapistDuringPC.Text = ds.Tables[1].Rows[0]["TherapistDuringPC"].ToString();
                    txtDiagnosis.Text = ds.Tables[1].Rows[0]["Diagnosis"].ToString();
                    txtCommentsPI.Text = ds.Tables[1].Rows[0]["CommentsPI"].ToString();
                    txtChiefConcernsHome.Text = ds.Tables[1].Rows[0]["ChiefConcernsHome"].ToString();
                    txtChiefConcernsSchool.Text = ds.Tables[1].Rows[0]["ChiefConcernsSchool"].ToString();
                    txtChiefConcernsSocialGath.Text = ds.Tables[1].Rows[0]["ChiefConcernsSocialGath"].ToString();
                    txtCommentsCC.Text = ds.Tables[1].Rows[0]["CommentsCC"].ToString();
                    CheckConsan.Checked = false; CheckNonConsan.Checked = false;
                    if (ds.Tables[1].Rows[0]["Consanguinity"].ToString().Equals(CheckConsan.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckConsan.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Consanguinity_1"].ToString().Equals(CheckNonConsan.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckNonConsan.Checked = true;
                    }
                    Check1Deg.Checked = false; Check2Deg.Checked = false; Check3Deg.Checked = false;
                    if (ds.Tables[1].Rows[0]["ConsanguinityDegree"].ToString().Equals(Check1Deg.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check1Deg.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ConsanguinityDegree_1"].ToString().Equals(Check2Deg.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check2Deg.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ConsanguinityDegree_2"].ToString().Equals(Check3Deg.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check3Deg.Checked = true;
                    }
                    txtYearsMarriage.Text = ds.Tables[1].Rows[0]["YearsMarriage"].ToString();
                    CheckNuclear.Checked = false; CheckJoint.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyStructure"].ToString().Equals(CheckNuclear.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckNuclear.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyStructure_1"].ToString().Equals(CheckJoint.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckJoint.Checked = true;
                    }
                    CheckNatural.Checked = false; CheckIUI.Checked = false; CheckIVF.Checked = false; CheckISCI.Checked = false; ; CheckOI.Checked = false;
                    if (ds.Tables[1].Rows[0]["Conception"].ToString().Equals(CheckNatural.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckNatural.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Conception_1"].ToString().Equals(CheckIUI.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckIUI.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Conception_2"].ToString().Equals(CheckIVF.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckIVF.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Conception_3"].ToString().Equals(CheckISCI.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckISCI.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Conception_4"].ToString().Equals(CheckOI.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckOI.Checked = true;
                    }
                    CheckPlanned.Checked = false; CheckUnplanned.Checked = false;
                    if (ds.Tables[1].Rows[0]["PlanningConception"].ToString().Equals(CheckPlanned.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckPlanned.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PlanningConception_1"].ToString().Equals(CheckUnplanned.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckUnplanned.Checked = true;
                    }
                    txtCommentsFH.Text = ds.Tables[1].Rows[0]["CommentsFH"].ToString();
                    CheckPoor.Checked = false; CheckFair.Checked = false; CheckGood.Checked = false;
                    if (ds.Tables[1].Rows[0]["InterParentalRelation"].ToString().Equals(CheckPoor.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckPoor.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["InterParentalRelation_1"].ToString().Equals(CheckFair.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckFair.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["InterParentalRelation_2"].ToString().Equals(CheckGood.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckGood.Checked = true;
                    }
                    CheckPoorr.Checked = false; CheckFairr.Checked = false; CheckGoodd.Checked = false;
                    if (ds.Tables[1].Rows[0]["ParentChildRelation"].ToString().Equals(CheckPoorr.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckPoorr.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ParentChildRelation_1"].ToString().Equals(CheckFairr.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckFairr.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ParentChildRelation_2"].ToString().Equals(CheckGoodd.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckGoodd.Checked = true;
                    }
                    Check_Poor.Checked = false; Check_Fair.Checked = false; Check_Good.Checked = false;
                    if (ds.Tables[1].Rows[0]["InterSiblingRelation"].ToString().Equals(Check_Poor.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check_Poor.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["InterSiblingRelation_1"].ToString().Equals(Check_Fair.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check_Fair.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["InterSiblingRelation_2"].ToString().Equals(Check_Good.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check_Good.Checked = true;
                    }
                    CheckYes.Checked = false; CheckNo.Checked = false; CheckMaybe.Checked = false;
                    if (ds.Tables[1].Rows[0]["DomesticViolence"].ToString().Equals(CheckYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckYes.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["DomesticViolence_1"].ToString().Equals(CheckNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckNo.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["DomesticViolence_2"].ToString().Equals(CheckMaybe.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckMaybe.Checked = true;
                    }
                    Check_Yes.Checked = false; Check_No.Checked = false;
                    if (ds.Tables[1].Rows[0]["FamilyRelocation"].ToString().Equals(Check_Yes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check_Yes.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["FamilyRelocation_1"].ToString().Equals(Check_No.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Check_No.Checked = true;
                    }
                    txtfrequency.Text = ds.Tables[1].Rows[0]["frequency"].ToString();
                    CheckMother.Checked = false; CheckFather.Checked = false; CheckGrandparents.Checked = false; CheckCaretaker.Checked = false;
                    if (ds.Tables[1].Rows[0]["PrimaryCare"].ToString().Equals(CheckMother.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckMother.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PrimaryCare_1"].ToString().Equals(CheckFather.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckFather.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PrimaryCare_2"].ToString().Equals(CheckGrandparents.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckGrandparents.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PrimaryCare_3"].ToString().Equals(CheckCaretaker.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckCaretaker.Checked = true;
                    }
                    txtMotherScreenTime.Text = ds.Tables[1].Rows[0]["MotherScreenTime"].ToString();
                    txtScreenTimeChild.Text = ds.Tables[1].Rows[0]["ScreenTimeChild"].ToString();
                    txtCommentsFR.Text = ds.Tables[1].Rows[0]["CommentsFR"].ToString();
                    txtPrenatalCondition.Text = ds.Tables[1].Rows[0]["PrenatalCondition"].ToString();
                    CheckPhysical.Checked = false; CheckMental.Checked = false;
                    if (ds.Tables[1].Rows[0]["CheckMental"].ToString().Equals(CheckPhysical.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckPhysical.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["MaternalStress_1"].ToString().Equals(CheckMental.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckMental.Checked = true;
                    }
                    txtDescribeStressors.Text = ds.Tables[1].Rows[0]["DescribeStressors"].ToString();
                    txtWGDP.Text = ds.Tables[1].Rows[0]["WGDP"].ToString();
                    txtFoetalMovement.Text = ds.Tables[1].Rows[0]["FoetalMovement"].ToString();
                    CheckYess.Checked = false; CheckNoo.Checked = false;
                    if (ds.Tables[1].Rows[0]["Prenatalwellness"].ToString().Equals(CheckYess.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckYess.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Prenatalwellness"].ToString().Equals(CheckNoo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckNoo.Checked = true;
                    }
                    txtCommentsMH.Text = ds.Tables[1].Rows[0]["CommentsMH"].ToString();
                    txtDurationLabour.Text = ds.Tables[1].Rows[0]["DurationLabour"].ToString();
                    rdoFTND.Checked = false; rdoFTNDva.Checked = false; rdoELSCS.Checked = false; rdoElectiveLSCS.Checked = false;
                    if (ds.Tables[1].Rows[0]["delivery"].ToString().Equals(rdoFTND.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoFTND.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["delivery_1"].ToString().Equals(rdoFTNDva.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoFTNDva.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["delivery_2"].ToString().Equals(rdoELSCS.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoELSCS.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["delivery_3"].ToString().Equals(rdoElectiveLSCS.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoElectiveLSCS.Checked = true;
                    }
                    rdoYess.Checked = false; rdoNoo.Checked = false;
                    if (ds.Tables[1].Rows[0]["ciab"].ToString().Equals(rdoYess.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoYess.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ciab"].ToString().Equals(rdoNoo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        rdoNoo.Checked = true;
                    }
                    txtConditionPostBirth.Text = ds.Tables[1].Rows[0]["ConditionPostBirth"].ToString();
                    txtBirthWeight.Text = ds.Tables[1].Rows[0]["BirthWeight"].ToString();
                    RdoAGA.Checked = false; RdoSGA.Checked = false; RdoLGA.Checked = false;
                    if (ds.Tables[1].Rows[0]["GestationalBirthAge"].ToString().Equals(RdoAGA.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoAGA.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["GestationalBirthAge_1"].ToString().Equals(RdoSGA.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoSGA.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["GestationalBirthAge_2"].ToString().Equals(RdoLGA.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoLGA.Checked = true;
                    }
                    RdoPresent.Checked = false; RdoAbsent.Checked = false;
                    if (ds.Tables[1].Rows[0]["NICUstay"].ToString().Equals(RdoPresent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoPresent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["NICUstay"].ToString().Equals(RdoAbsent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoAbsent.Checked = true;
                    }
                    txtDurationNICUstay.Text = ds.Tables[1].Rows[0]["DurationNICUstay"].ToString();
                    txtNICUHistory.Text = ds.Tables[1].Rows[0]["NICUHistory"].ToString();
                    txtReasonNICUstay.Text = ds.Tables[1].Rows[0]["ReasonNICUstay"].ToString();
                    txtAPGARscore.Text = ds.Tables[1].Rows[0]["APGARscore"].ToString();
                    RdoYes.Checked = false; RdoNo.Checked = false;
                    if (ds.Tables[1].Rows[0]["Breastfed"].ToString().Equals(RdoYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoYes.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Breastfed"].ToString().Equals(RdoNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoNo.Checked = true;
                    }
                    txtBabyFed.Text = ds.Tables[1].Rows[0]["BabyFed"].ToString();
                    RadioPresent.Checked = false; RadioAbsent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Problemsduringbreastfeeding"].ToString().Equals(RadioPresent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioPresent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Problemsduringbreastfeeding"].ToString().Equals(RadioAbsent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAbsent.Checked = true;
                    }
                    txtMentionProblem.Text = ds.Tables[1].Rows[0]["MentionProblem"].ToString();
                    txtwaswtcbf.Text = ds.Tables[1].Rows[0]["waswtcbf"].ToString();
                    RadioYes.Checked = false; RadioNo.Checked = false;
                    if (ds.Tables[1].Rows[0]["colicissue"].ToString().Equals(RadioYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioYes.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["colicissue"].ToString().Equals(RadioNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioNo.Checked = true;
                    }
                    txtOthrtMedicalIssues.Text = ds.Tables[1].Rows[0]["OthrtMedicalIssues"].ToString();
                    txtCommentsPPH.Text = ds.Tables[1].Rows[0]["CommentsPPH"].ToString();
                    txtGrossMotor.Text = ds.Tables[1].Rows[0]["GrossMotor"].ToString();
                    txtFineMotor.Text = ds.Tables[1].Rows[0]["FineMotor"].ToString();
                    txtPersonalandSocial.Text = ds.Tables[1].Rows[0]["PersonalandSocial"].ToString();
                    txtCommunication.Text = ds.Tables[1].Rows[0]["Communication"].ToString();
                    txtCommentsDM.Text = ds.Tables[1].Rows[0]["CommentsDM"].ToString();
                    RadiooNo.Checked = false; RadiooYes.Checked = false;
                    if (ds.Tables[1].Rows[0]["Sleepissues"].ToString().Equals(RadiooNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadiooNo.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Sleepissues"].ToString().Equals(RadiooYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadiooYes.Checked = true;
                    }
                    PresentRadio.Checked = false; AbsentRadio.Checked = false;
                    if (ds.Tables[1].Rows[0]["Presentsleep"].ToString().Equals(PresentRadio.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        PresentRadio.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Presentsleep"].ToString().Equals(AbsentRadio.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        AbsentRadio.Checked = true;
                    }
                    txtSleepduration.Text = ds.Tables[1].Rows[0]["Sleepduration"].ToString();
                    RadioLight.Checked = false; RadioDeep.Checked = false;
                    if (ds.Tables[1].Rows[0]["SleepType"].ToString().Equals(RadioLight.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioLight.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["SleepType"].ToString().Equals(RadioDeep.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioDeep.Checked = true;
                    }
                    RadioAbsentbtn.Checked = false; RadioPresentbtn.Checked = false;
                    if (ds.Tables[1].Rows[0]["Cosleeping"].ToString().Equals(RadioAbsentbtn.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAbsentbtn.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Cosleeping"].ToString().Equals(RadioPresentbtn.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioPresentbtn.Checked = true;
                    }
                    txtCosleepingwith.Text = ds.Tables[1].Rows[0]["Cosleepingwith"].ToString();
                    txtAnySleepAdjunctsused.Text = ds.Tables[1].Rows[0]["AnySleepAdjunctsused"].ToString();
                    RadioButtonPresent.Checked = false; RadioButtonAbsent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Naptime"].ToString().Equals(RadioButtonPresent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonPresent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Naptime"].ToString().Equals(RadioButtonAbsent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonAbsent.Checked = true;
                    }
                    txtNapduration.Text = ds.Tables[1].Rows[0]["Napduration"].ToString();
                    txtCommentsS.Text = ds.Tables[1].Rows[0]["CommentsS"].ToString();
                    RadioTypical.Checked = false; RadioAtypical.Checked = false;
                    if (ds.Tables[1].Rows[0]["Feedinghabits"].ToString().Equals(RadioTypical.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioTypical.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Feedinghabits"].ToString().Equals(RadioAtypical.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAtypical.Checked = true;
                    }
                    txtTypeoffoodhad.Text = ds.Tables[1].Rows[0]["Typeoffoodhad"].ToString();
                    txtFoodconsistency.Text = ds.Tables[1].Rows[0]["Foodconsistency"].ToString();
                    txtFoodtemperature.Text = ds.Tables[1].Rows[0]["Foodtemperature"].ToString();
                    txtFoodtaste.Text = ds.Tables[1].Rows[0]["Foodtaste"].ToString();
                    txtCommentsFeHa.Text = ds.Tables[1].Rows[0]["CommentsFeHa"].ToString();
                    txtChildLikes.Text = ds.Tables[1].Rows[0]["ChildLikes"].ToString();
                    //txtChildDislikes.Text = ds.Tables[1].Rows[0]["ChildDislikes"].ToString();
                    //txtMomentsOfHappiness.Text = ds.Tables[1].Rows[0]["MomentsOfHappiness"].ToString();
                    //txtMomentsOfFear.Text = ds.Tables[1].Rows[0]["MomentsOfFear"].ToString();
                    //txtFeelingsNemotions.Text = ds.Tables[1].Rows[0]["FeelingsNemotions"].ToString();
                    //RadioButtonYes.Checked = false; RadioButtonNo.Checked = false; RadioButtonMaybe.Checked = false;
                    //if (ds.Tables[1].Rows[0]["signsofstress"].ToString().Equals(RadioButtonYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    RadioButtonYes.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["signsofstress"].ToString().Equals(RadioButtonNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    RadioButtonNo.Checked = true;
                    //}
                    //if (ds.Tables[1].Rows[0]["signsofstress"].ToString().Equals(RadioButtonMaybe.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    RadioButtonMaybe.Checked = true;
                    //}
                    txtCommentsITCH.Text = ds.Tables[1].Rows[0]["CommentsITCH"].ToString();
                    RadioOrganised.Checked = false; RadioDisorganised.Checked = false;
                    if (ds.Tables[1].Rows[0]["Playbehaviour"].ToString().Equals(RadioOrganised.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioOrganised.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Playbehaviour"].ToString().Equals(RadioDisorganised.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioDisorganised.Checked = true;
                    }
                    txtInteractionwithpeers.Text = ds.Tables[1].Rows[0]["Interactionwithpeers"].ToString();
                    RadioPresentButton.Checked = false; RadioAbsentButton.Checked = false;
                    if (ds.Tables[1].Rows[0]["Strangeranxiety"].ToString().Equals(RadioPresentButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioPresentButton.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Strangeranxiety"].ToString().Equals(RadioAbsentButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAbsentButton.Checked = true;
                    }
                    RadioYesButton.Checked = false; RadioNoButton.Checked = false; RadioMaybeButton.Checked = false;
                    if (ds.Tables[1].Rows[0]["PlayToys"].ToString().Equals(RadioYesButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioYesButton.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PlayToys"].ToString().Equals(RadioNoButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioNoButton.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["PlayToys"].ToString().Equals(RadioMaybeButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioMaybeButton.Checked = true;
                    }
                    txtPreferenceoftoys.Text = ds.Tables[1].Rows[0]["Preferenceoftoys"].ToString();
                    txtCommentsPB.Text = ds.Tables[1].Rows[0]["CommentsPB"].ToString();
                    RadioDependent.Checked = false; RadioAssisted.Checked = false; RadioIndependent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Brushing"].ToString().Equals(RadioDependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioDependent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Brushing_1"].ToString().Equals(RadioAssisted.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAssisted.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Brushing_2"].ToString().Equals(RadioIndependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioIndependent.Checked = true;
                    }
                    txtCommentsBrushing.Text = ds.Tables[1].Rows[0]["CommentsBrushing"].ToString();
                    DependentRadio.Checked = false; AssistedRadio.Checked = false; IndependentRadio.Checked = false;
                    if (ds.Tables[1].Rows[0]["Bathing"].ToString().Equals(DependentRadio.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        DependentRadio.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Bathing_1"].ToString().Equals(AssistedRadio.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        AssistedRadio.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Bathing_2"].ToString().Equals(IndependentRadio.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        IndependentRadio.Checked = true;
                    }
                    txtCommentsBathing.Text = ds.Tables[1].Rows[0]["CommentsBathing"].ToString();
                    RadioDependentButton.Checked = false; RadioAssistedButton.Checked = false; RadioIndependentButton.Checked = false;
                    if (ds.Tables[1].Rows[0]["Toileting"].ToString().Equals(RadioDependentButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioDependentButton.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Toileting_1"].ToString().Equals(RadioAssistedButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioAssistedButton.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Toileting_2"].ToString().Equals(RadioIndependentButton.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioIndependentButton.Checked = true;
                    }
                    txtCommentsToileting.Text = ds.Tables[1].Rows[0]["CommentsToileting"].ToString();
                    RadioButtonDependent.Checked = false; RadioButtonAssisted.Checked = false; RadioButtonIndependent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Dressing"].ToString().Equals(RadioButtonDependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonDependent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Dressing_1"].ToString().Equals(RadioButtonAssisted.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonAssisted.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Dressing_2"].ToString().Equals(RadioButtonIndependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonIndependent.Checked = true;
                    }
                    txtCommentsDressing.Text = ds.Tables[1].Rows[0]["CommentsDressing"].ToString();
                    RadioBtnDependent.Checked = false; RadioBtnAssisted.Checked = false; RadioBtnIndependent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Eating"].ToString().Equals(RadioBtnDependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioBtnDependent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Eating_1"].ToString().Equals(RadioBtnAssisted.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioBtnAssisted.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Eating_2"].ToString().Equals(RadioBtnIndependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioBtnIndependent.Checked = true;
                    }
                    txtCommentsEating.Text = ds.Tables[1].Rows[0]["CommentsEating"].ToString();
                    RdoDependent.Checked = false; RdoAssisted.Checked = false; RdoIndependent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Ambulation"].ToString().Equals(RdoDependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoDependent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Ambulation_1"].ToString().Equals(RdoAssisted.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoAssisted.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Ambulation_2"].ToString().Equals(RdoIndependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdoIndependent.Checked = true;
                    }
                    txtCommentsAmbulation.Text = ds.Tables[1].Rows[0]["CommentsAmbulation"].ToString();
                    RdobtnDependent.Checked = false; RdobtnAssisted.Checked = false; RdobtnIndependent.Checked = false;
                    if (ds.Tables[1].Rows[0]["Transfers"].ToString().Equals(RdobtnDependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdobtnDependent.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Transfers_1"].ToString().Equals(RdobtnAssisted.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdobtnAssisted.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Transfers_2"].ToString().Equals(RdobtnIndependent.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        RdobtnIndependent.Checked = true;
                    }
                    txtCommentsTransfers.Text = ds.Tables[1].Rows[0]["CommentsTransfers"].ToString();
                    txtAddComments.Text = ds.Tables[1].Rows[0]["AddComments"].ToString();
                    txtAddEvalRec.Text = ds.Tables[1].Rows[0]["AddEvalRec"].ToString();

                    // txtMotherAgeDC.Text = ds.Tables[1].Rows[0]["MotherAgeDC"].ToString();
                    // txtFatherAgeDC.Text = ds.Tables[1].Rows[0]["FatherAgeDC"].ToString();
                    AnySiblingsYes.Checked = false; AnySiblingsNo.Checked = false;
                    if (ds.Tables[1].Rows[0]["Siblings"].ToString().Equals(AnySiblingsYes.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        AnySiblingsYes.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["Siblings"].ToString().Equals(AnySiblingsNo.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        AnySiblingsNo.Checked = true;
                    }
                    txtNoOfSiblings.Text = ds.Tables[1].Rows[0]["NoOfSiblings"].ToString();
                    txtRHASiblings.Text = ds.Tables[1].Rows[0]["RHASiblings"].ToString();
                    // AddCommentaAttend.Text = ds.Tables[1].Rows[0]["AddCommentaAttend"].ToString();
                    txtOnlineOffline.Text = ds.Tables[1].Rows[0]["OnlineOffline"].ToString();
                    txtWhichGrade.Text = ds.Tables[1].Rows[0]["WhichGrade"].ToString();
                    YesAttend.Checked = false; Noattend.Checked = false;
                    if (ds.Tables[1].Rows[0]["ChildAttend"].ToString().Equals(YesAttend.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        YesAttend.Checked = true;
                    }
                    if (ds.Tables[1].Rows[0]["ChildAttend"].ToString().Equals(Noattend.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        Noattend.Checked = true;
                    }


                    List<optionMdel> qls = new List<optionMdel>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        string dateMonth = string.Empty; string relevantHistory = string.Empty; string hospitalDoctorsVisited = string.Empty;
                        string doctorsRecommendations = string.Empty; string investigationsRecordsResults = string.Empty;

                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["DateMonth"].ToString()))
                        {
                            dateMonth = ds.Tables[1].Rows[i]["DateMonth"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["RelevantHistory"].ToString()))
                        {
                            relevantHistory = ds.Tables[1].Rows[i]["RelevantHistory"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["HospitalDoctorsVisited"].ToString()))
                        {
                            hospitalDoctorsVisited = ds.Tables[1].Rows[i]["HospitalDoctorsVisited"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["DoctorsRecommendations"].ToString()))
                        {
                            doctorsRecommendations = ds.Tables[1].Rows[i]["DoctorsRecommendations"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["InvestigationsRecordsResults"].ToString()))
                        {
                            investigationsRecordsResults = ds.Tables[1].Rows[i]["InvestigationsRecordsResults"].ToString();
                        }

                        qls.Add(new optionMdel
                        {
                            Option = ds.Tables[1].Rows[i]["PreConsultID"].ToString(),
                            Option1 = dateMonth,
                            Option2 = relevantHistory,
                            Option3 = hospitalDoctorsVisited,
                            Option4 = doctorsRecommendations,
                            Option5 = investigationsRecordsResults
                        });
                    }




                    int temp = qls.Count; txtVisibleOption.Value = qls.Count.ToString();
                    for (int jl = 0; jl < (OptionCount - temp); jl++)
                    {
                        qls.Add(new optionMdel() { Option = string.Empty });
                    }
                    txtSignleChoice.DataSource = qls;
                    txtSignleChoice.DataBind();

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();

            int HidPreConsultID = 0;
            string Option1 = string.Empty;
            string Option2 = string.Empty;
            string Option3 = string.Empty;
            string Option4 = string.Empty;
            string Option5 = string.Empty;

            for (int j = 1; j <= OptionCount; j++)
            {
                RepeaterItem item = txtSignleChoice.Items.Count >= j ? txtSignleChoice.Items[j - 1] : null;
                if (item != null)
                {
                    HiddenField PreConsultID = item.FindControl("txtPreConsultID") as HiddenField;
                    TextBox DateMonth = item.FindControl("txtDateMonth") as TextBox;
                    TextBox RelevantHistory = item.FindControl("txtRelevantHistory") as TextBox;
                    TextBox HospitalDoctorsVisited = item.FindControl("txtHospitalDoctorsVisited") as TextBox;
                    TextBox DoctorsRecommendations = item.FindControl("txtDoctorsRecommendations") as TextBox;
                    TextBox InvestigationsRecordsResults = item.FindControl("txtInvestigationsRecordsResults") as TextBox;
                    if (DateMonth.Text != "" || RelevantHistory.Text != "" || HospitalDoctorsVisited.Text != "" || DoctorsRecommendations.Text != "" || InvestigationsRecordsResults.Text != "")
                    {
                        int.TryParse(PreConsultID.Value.ToString(), out HidPreConsultID);

                        Option1 = DateMonth.Text.Trim();

                        Option2 = RelevantHistory.Text.Trim();

                        Option3 = HospitalDoctorsVisited.Text.Trim();

                        Option4 = DoctorsRecommendations.Text.Trim();

                        Option5 = InvestigationsRecordsResults.Text.Trim();

                        int k = RDB.SetTimeLine(_appointmentID, HidPreConsultID, Option1, Option2, Option3, Option4, Option5, DateTime.UtcNow.AddMinutes(330), _loginID);

                    }
                    else if (DateMonth.Text == "" && RelevantHistory.Text == "" && HospitalDoctorsVisited.Text == "" && DoctorsRecommendations.Text == "" && InvestigationsRecordsResults.Text == "")
                    {
                        int.TryParse(PreConsultID.Value.ToString(), out HidPreConsultID);
                        int P = RDB.DeleteRow(HidPreConsultID);
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

            // Patient Information Start //
            DateTime DatepreConsult = new DateTime();
            if (txtDatepreConsult.Text.Trim().Length > 0)
            {
                DateTime.TryParseExact(txtDatepreConsult.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatepreConsult);
            }
            DateTime DateBirth = new DateTime();
            DateTime.TryParseExact(txtDateBirth.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth);
            DateTime DateDelivery = new DateTime();
            DateTime.TryParseExact(txtDateofDelivery.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateDelivery);
            string Gender = string.Empty;
            if (CheckFemale.Checked) { Gender = CheckFemale.Text.Trim(); }
            if (CheckMale.Checked) { Gender = CheckMale.Text.Trim(); }
            if (CheckOther.Checked) { Gender = CheckOther.Text.Trim(); }
            string ChildAttend = string.Empty;
            if (YesAttend.Checked) { ChildAttend = YesAttend.Text.Trim(); }
            if (Noattend.Checked) { ChildAttend = Noattend.Text.Trim(); }
            // Patient Information End //

            // Family History Start //
            string Consanguinity = string.Empty;
            if (CheckConsan.Checked) { Consanguinity = CheckConsan.Text.Trim(); }
            string Consanguinity_1 = string.Empty;
            if (CheckNonConsan.Checked) { Consanguinity_1 = CheckNonConsan.Text.Trim(); }
            string ConsanguinityDegree = string.Empty;
            if (Check1Deg.Checked) { ConsanguinityDegree = Check1Deg.Text.Trim(); }
            string ConsanguinityDegree_1 = string.Empty;
            if (Check2Deg.Checked) { ConsanguinityDegree_1 = Check2Deg.Text.Trim(); }
            string ConsanguinityDegree_2 = string.Empty;
            if (Check3Deg.Checked) { ConsanguinityDegree_2 = Check3Deg.Text.Trim(); }
            string FamilyStructure = string.Empty;
            if (CheckNuclear.Checked) { FamilyStructure = CheckNuclear.Text.Trim(); }
            string FamilyStructure_1 = string.Empty;
            if (CheckJoint.Checked) { FamilyStructure_1 = CheckJoint.Text.Trim(); }
            string Conception = string.Empty;
            if (CheckNatural.Checked) { Conception = CheckNatural.Text.Trim(); }
            string Conception_1 = string.Empty;
            if (CheckIUI.Checked) { Conception_1 = CheckIUI.Text.Trim(); }
            string Conception_2 = string.Empty;
            if (CheckIVF.Checked) { Conception_2 = CheckIVF.Text.Trim(); }
            string Conception_3 = string.Empty;
            if (CheckISCI.Checked) { Conception_3 = CheckISCI.Text.Trim(); }
            string Conception_4 = string.Empty;
            if (CheckOI.Checked) { Conception_4 = CheckOI.Text.Trim(); }
            string PlanningConception = string.Empty;
            if (CheckPlanned.Checked) { PlanningConception = CheckPlanned.Text.Trim(); }
            string PlanningConception_1 = string.Empty;
            if (CheckUnplanned.Checked) { PlanningConception_1 = CheckUnplanned.Text.Trim(); }
            string Siblings = string.Empty;
            if (AnySiblingsYes.Checked) { Siblings = AnySiblingsYes.Text.Trim(); }
            if (AnySiblingsNo.Checked) { Siblings = AnySiblingsNo.Text.Trim(); }
            // Family History End //

            // Family Relation Start //
            string InterParentalRelation = string.Empty;
            if (CheckPoor.Checked) { InterParentalRelation = CheckPoor.Text.Trim(); }
            string InterParentalRelation_1 = string.Empty;
            if (CheckFair.Checked) { InterParentalRelation_1 = CheckFair.Text.Trim(); }
            string InterParentalRelation_2 = string.Empty;
            if (CheckGood.Checked) { InterParentalRelation_2 = CheckGood.Text.Trim(); }
            string ParentChildRelation = string.Empty;
            if (CheckPoorr.Checked) { ParentChildRelation = CheckPoorr.Text.Trim(); }
            string ParentChildRelation_1 = string.Empty;
            if (CheckFairr.Checked) { ParentChildRelation_1 = CheckFairr.Text.Trim(); }
            string ParentChildRelation_2 = string.Empty;
            if (CheckGoodd.Checked) { ParentChildRelation_2 = CheckGoodd.Text.Trim(); }
            string InterSiblingRelation = string.Empty;
            if (Check_Poor.Checked) { InterSiblingRelation = Check_Poor.Text.Trim(); }
            string InterSiblingRelation_1 = string.Empty;
            if (Check_Fair.Checked) { InterSiblingRelation_1 = Check_Fair.Text.Trim(); }
            string InterSiblingRelation_2 = string.Empty;
            if (Check_Good.Checked) { InterSiblingRelation_2 = Check_Good.Text.Trim(); }
            string DomesticViolence = string.Empty;
            if (CheckYes.Checked) { DomesticViolence = CheckYes.Text.Trim(); }
            string DomesticViolence_1 = string.Empty;
            if (CheckNo.Checked) { DomesticViolence_1 = CheckNo.Text.Trim(); }
            string DomesticViolence_2 = string.Empty;
            if (CheckMaybe.Checked) { DomesticViolence_2 = CheckMaybe.Text.Trim(); }
            string FamilyRelocation = string.Empty;
            if (Check_Yes.Checked) { FamilyRelocation = Check_Yes.Text.Trim(); }
            string FamilyRelocation_1 = string.Empty;
            if (Check_No.Checked) { FamilyRelocation_1 = Check_No.Text.Trim(); }
            string PrimaryCare = string.Empty;
            if (CheckMother.Checked) { PrimaryCare = CheckMother.Text.Trim(); }
            string PrimaryCare_1 = string.Empty;
            if (CheckFather.Checked) { PrimaryCare_1 = CheckFather.Text.Trim(); }
            string PrimaryCare_2 = string.Empty;
            if (CheckGrandparents.Checked) { PrimaryCare_2 = CheckGrandparents.Text.Trim(); }
            string PrimaryCare_3 = string.Empty;
            if (CheckCaretaker.Checked) { PrimaryCare_3 = CheckCaretaker.Text.Trim(); }
            // Family Relation End //

            // Maternal History start //
            string MaternalStress = string.Empty;
            if (CheckPhysical.Checked) { MaternalStress = CheckPhysical.Text.Trim(); }
            string MaternalStress_1 = string.Empty;
            if (CheckMental.Checked) { MaternalStress_1 = CheckMental.Text.Trim(); }
            string Prenatalwellness = string.Empty;
            if (CheckYess.Checked) { Prenatalwellness = CheckYess.Text.Trim(); }
            if (CheckNoo.Checked) { Prenatalwellness = CheckNoo.Text.Trim(); }
            // Maternal History End //

            // Peri and Postnatal History Start //
            string delivery = string.Empty;
            if (rdoFTND.Checked) { delivery = rdoFTND.Text.Trim(); }
            string delivery_1 = string.Empty;
            if (rdoFTNDva.Checked) { delivery_1 = rdoFTNDva.Text.Trim(); }
            string delivery_2 = string.Empty;
            if (rdoELSCS.Checked) { delivery_2 = rdoELSCS.Text.Trim(); }
            string delivery_3 = string.Empty;
            if (rdoElectiveLSCS.Checked) { delivery_3 = rdoElectiveLSCS.Text.Trim(); }
            string ciab = string.Empty;
            if (rdoYess.Checked) { ciab = rdoYess.Text.Trim(); }
            if (rdoNoo.Checked) { ciab = rdoNoo.Text.Trim(); }
            string GestationalBirthAge = string.Empty;
            if (RdoAGA.Checked) { GestationalBirthAge = RdoAGA.Text.Trim(); }
            string GestationalBirthAge_1 = string.Empty;
            if (RdoSGA.Checked) { GestationalBirthAge_1 = RdoSGA.Text.Trim(); }
            string GestationalBirthAge_2 = string.Empty;
            if (RdoLGA.Checked) { GestationalBirthAge_2 = RdoLGA.Text.Trim(); }
            string NICUstay = string.Empty;
            if (RdoPresent.Checked) { NICUstay = RdoPresent.Text.Trim(); }
            if (RdoAbsent.Checked) { NICUstay = RdoAbsent.Text.Trim(); }
            string Breastfed = string.Empty;
            if (RdoYes.Checked) { Breastfed = RdoYes.Text.Trim(); }
            if (RdoNo.Checked) { Breastfed = RdoNo.Text.Trim(); }
            string Problemsduringbreastfeeding = string.Empty;
            if (RadioPresent.Checked) { Problemsduringbreastfeeding = RadioPresent.Text.Trim(); }
            if (RadioAbsent.Checked) { Problemsduringbreastfeeding = RadioAbsent.Text.Trim(); }
            string colicissue = string.Empty;
            if (RadioYes.Checked) { colicissue = RadioYes.Text.Trim(); }
            if (RadioNo.Checked) { colicissue = RadioNo.Text.Trim(); }
            // Peri and Postnatal History End //

            // Sleep Start //
            string Sleepissues = string.Empty;
            if (RadiooNo.Checked) { Sleepissues = RadiooNo.Text.Trim(); }
            if (RadiooYes.Checked) { Sleepissues = RadiooYes.Text.Trim(); }
            string Presentsleep = string.Empty;
            if (PresentRadio.Checked) { Presentsleep = PresentRadio.Text.Trim(); }
            if (AbsentRadio.Checked) { Presentsleep = AbsentRadio.Text.Trim(); }
            string SleepType = string.Empty;
            if (RadioLight.Checked) { SleepType = RadioLight.Text.Trim(); }
            if (RadioDeep.Checked) { SleepType = RadioDeep.Text.Trim(); }
            string Cosleeping = string.Empty;
            if (RadioAbsentbtn.Checked) { Cosleeping = RadioAbsentbtn.Text.Trim(); }
            if (RadioPresentbtn.Checked) { Cosleeping = RadioPresentbtn.Text.Trim(); }
            string Naptime = string.Empty;
            if (RadioButtonPresent.Checked) { Naptime = RadioButtonPresent.Text.Trim(); }
            if (RadioButtonAbsent.Checked) { Naptime = RadioButtonAbsent.Text.Trim(); }
            // Sleep end //

            // Feeding habits Start //
            string Feedinghabits = string.Empty;
            if (RadioTypical.Checked) { Feedinghabits = RadioTypical.Text.Trim(); }
            if (RadioAtypical.Checked) { Feedinghabits = RadioAtypical.Text.Trim(); }
            // Feeding habits end //

            // Into the Child's Heart start //
            //string signsofstress = string.Empty;
            //if (RadioButtonYes.Checked) { signsofstress = RadioButtonYes.Text.Trim(); }
            //if (RadioButtonNo.Checked) { signsofstress = RadioButtonNo.Text.Trim(); }
            //if (RadioButtonMaybe.Checked) { signsofstress = RadioButtonMaybe.Text.Trim(); }
            // Into the Child's Heart end //

            // Play Behaviour start //
            string Playbehaviour = string.Empty;
            if (RadioOrganised.Checked) { Playbehaviour = RadioOrganised.Text.Trim(); }
            if (RadioDisorganised.Checked) { Playbehaviour = RadioDisorganised.Text.Trim(); }
            string Strangeranxiety = string.Empty;
            if (RadioPresentButton.Checked) { Strangeranxiety = RadioPresentButton.Text.Trim(); }
            if (RadioAbsentButton.Checked) { Strangeranxiety = RadioAbsentButton.Text.Trim(); }
            string PlayToys = string.Empty;
            if (RadioYesButton.Checked) { PlayToys = RadioYesButton.Text.Trim(); }
            if (RadioNoButton.Checked) { PlayToys = RadioNoButton.Text.Trim(); }
            if (RadioMaybeButton.Checked) { PlayToys = RadioMaybeButton.Text.Trim(); }
            // Play Behaviour end //

            // ADLs start //
            string Brushing = string.Empty;
            if (RadioDependent.Checked) { Brushing = RadioDependent.Text.Trim(); }
            string Brushing_1 = string.Empty;
            if (RadioAssisted.Checked) { Brushing_1 = RadioAssisted.Text.Trim(); }
            string Brushing_2 = string.Empty;
            if (RadioIndependent.Checked) { Brushing_2 = RadioIndependent.Text.Trim(); }
            string Bathing = string.Empty;
            if (DependentRadio.Checked) { Bathing = DependentRadio.Text.Trim(); }
            string Bathing_1 = string.Empty;
            if (AssistedRadio.Checked) { Bathing_1 = AssistedRadio.Text.Trim(); }
            string Bathing_2 = string.Empty;
            if (IndependentRadio.Checked) { Bathing_2 = IndependentRadio.Text.Trim(); }
            string Toileting = string.Empty;
            if (RadioDependentButton.Checked) { Toileting = RadioDependentButton.Text.Trim(); }
            string Toileting_1 = string.Empty;
            if (RadioAssistedButton.Checked) { Toileting_1 = RadioAssistedButton.Text.Trim(); }
            string Toileting_2 = string.Empty;
            if (RadioIndependentButton.Checked) { Toileting_2 = RadioIndependentButton.Text.Trim(); }
            string Dressing = string.Empty;
            if (RadioButtonDependent.Checked) { Dressing = RadioButtonDependent.Text.Trim(); }
            string Dressing_1 = string.Empty;
            if (RadioButtonAssisted.Checked) { Dressing_1 = RadioButtonAssisted.Text.Trim(); }
            string Dressing_2 = string.Empty;
            if (RadioButtonIndependent.Checked) { Dressing_2 = RadioButtonIndependent.Text.Trim(); }
            string Eating = string.Empty;
            if (RadioBtnDependent.Checked) { Eating = RadioBtnDependent.Text.Trim(); }
            string Eating_1 = string.Empty;
            if (RadioBtnAssisted.Checked) { Eating_1 = RadioBtnAssisted.Text.Trim(); }
            string Eating_2 = string.Empty;
            if (RadioBtnIndependent.Checked) { Eating_2 = RadioBtnIndependent.Text.Trim(); }
            string Ambulation = string.Empty;
            if (RdoDependent.Checked) { Ambulation = RdoDependent.Text.Trim(); }
            string Ambulation_1 = string.Empty;
            if (RdoAssisted.Checked) { Ambulation_1 = RdoAssisted.Text.Trim(); }
            string Ambulation_2 = string.Empty;
            if (RdoIndependent.Checked) { Ambulation_2 = RdoIndependent.Text.Trim(); }
            string Transfers = string.Empty;
            if (RdobtnDependent.Checked) { Transfers = RdobtnDependent.Text.Trim(); }
            string Transfers_1 = string.Empty;
            if (RdobtnAssisted.Checked) { Transfers_1 = RdobtnAssisted.Text.Trim(); }
            string Transfers_2 = string.Empty;
            if (RdobtnIndependent.Checked) { Transfers_2 = RdobtnIndependent.Text.Trim(); }
            // ADLs end //




            int i = RDB.Set_New(_appointmentID, IsFinal, IsGiven, GivenDate, DateTime.UtcNow.AddMinutes(330), _loginID,
            DatepreConsult, txtComfortableLanguage.Text.Trim(), DateBirth, DateDelivery, txtCorrectAge.Text.Trim(),
            txtAge.Text.Trim(), Gender, txtMotherName.Text.Trim(), txtMotherAge.Text.Trim(), txtMotherQualification.Text.Trim(),
            txtMotherOccupation.Text.Trim(), txtMotherWorkingHour.Text.Trim(), txtFatherName.Text.Trim(), txtFatherAge.Text.Trim(),
            txtFatherOccupation.Text.Trim(), txtFatherQualification.Text.Trim(), txtFatherWorkingHour.Text.Trim(), txtAddress.Text.Trim(),
            txtContactDetails.Text.Trim(), txtEmailID.Text.Trim(), txtReferredBy.Text.Trim(), txtTherapistDuringPC.Text.Trim(), txtDiagnosis.Text.Trim(),
            txtCommentsPI.Text.Trim(), txtChiefConcernsHome.Text.Trim(), txtChiefConcernsSchool.Text.Trim(), txtChiefConcernsSocialGath.Text.Trim(),
            txtCommentsCC.Text.Trim(), Consanguinity, ConsanguinityDegree, txtYearsMarriage.Text.Trim(), FamilyStructure, Conception,
            PlanningConception, txtCommentsFH.Text.Trim(), ParentChildRelation, InterParentalRelation, InterSiblingRelation, DomesticViolence, FamilyRelocation,
            txtfrequency.Text.Trim(), PrimaryCare, txtMotherScreenTime.Text.Trim(), txtScreenTimeChild.Text.Trim(), txtCommentsFR.Text.Trim(),
            txtPrenatalCondition.Text.Trim(), MaternalStress, txtDescribeStressors.Text.Trim(), txtWGDP.Text.Trim(), txtFoetalMovement.Text.Trim(),
            txtCommentsMH.Text.Trim(), txtDurationLabour.Text.Trim(), delivery, ciab, txtConditionPostBirth.Text.Trim(), txtBirthWeight.Text.Trim(),
            GestationalBirthAge, NICUstay, txtDurationNICUstay.Text.Trim(), txtNICUHistory.Text.Trim(), txtReasonNICUstay.Text.Trim(), txtAPGARscore.Text.Trim(),
            Breastfed, txtBabyFed.Text.Trim(), Problemsduringbreastfeeding, txtMentionProblem.Text.Trim(), txtwaswtcbf.Text.Trim(), colicissue,
            txtOthrtMedicalIssues.Text.Trim(), txtCommentsPPH.Text.Trim(), txtGrossMotor.Text.Trim(), txtFineMotor.Text.Trim(), txtPersonalandSocial.Text.Trim(),
            txtCommunication.Text.Trim(), txtCommentsDM.Text.Trim(), Sleepissues, Presentsleep, txtSleepduration.Text.Trim(), SleepType, Cosleeping,
            txtCosleepingwith.Text.Trim(), txtAnySleepAdjunctsused.Text.Trim(), Naptime, txtNapduration.Text.Trim(), txtCommentsS.Text.Trim(), Feedinghabits,
            txtTypeoffoodhad.Text.Trim(), txtFoodconsistency.Text.Trim(), txtFoodtemperature.Text.Trim(), txtFoodtaste.Text.Trim(), txtCommentsFeHa.Text.Trim(),
            txtChildLikes.Text.Trim(), txtCommentsITCH.Text.Trim(), Playbehaviour, txtInteractionwithpeers.Text.Trim(), Strangeranxiety, PlayToys,
            txtPreferenceoftoys.Text.Trim(), txtCommentsPB.Text.Trim(), Brushing, txtCommentsBrushing.Text.Trim(), Bathing, txtCommentsBathing.Text.Trim(), Toileting,
            txtCommentsToileting.Text.Trim(), Dressing, txtCommentsDressing.Text.Trim(), Eating, txtCommentsEating.Text.Trim(), Ambulation, txtCommentsAmbulation.Text.Trim(),
            Transfers, txtCommentsTransfers.Text.Trim(), txtAddComments.Text.Trim(), Prenatalwellness, Siblings,
            txtNoOfSiblings.Text.Trim(), txtRHASiblings.Text.Trim(), Consanguinity_1, ConsanguinityDegree_1, ConsanguinityDegree_2, FamilyStructure_1,
            Conception_1, Conception_2, Conception_3, Conception_4, PlanningConception_1, InterParentalRelation_1, InterParentalRelation_2, ParentChildRelation_1,
            ParentChildRelation_2, InterSiblingRelation_1, InterSiblingRelation_2, DomesticViolence_1, DomesticViolence_2, FamilyRelocation_1, PrimaryCare_1, PrimaryCare_2, PrimaryCare_3,
            MaternalStress_1, delivery_1, delivery_2, delivery_3, GestationalBirthAge_1, GestationalBirthAge_2, txtAddEvalRec.Text.Trim(), ChildAttend, txtOnlineOffline.Text.Trim(), txtWhichGrade.Text.Trim(),
            Brushing_1, Brushing_2, Bathing_1, Bathing_2, Toileting_1, Toileting_2, Dressing_1, Dressing_2, Eating_1, Eating_2, Ambulation_1, Ambulation_2,
            Transfers_1, Transfers_2
            // ,  Option1, Option2, Option3, Option4, Option5
            );
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Pre Consultation report saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                LoadForm();
                Response.Redirect(ResolveClientUrl("~/SessionRpt/PreConsultRpt.aspx?record=" + Request.QueryString["record"]), true);
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
                return "<a href=\"javascript:;\" class=\"btn btn-xs btn-default btn-success\" style=\"float:right; margin-left:20px;\" onclick=\"show_next_option(this);\"><i class=\"fa fa-plus-circle\"></i></a>";
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
        public string cloneClass(int index, string Option, string Option1, string Option2, string Option3, string Option4, string Option5)
        {
            if (index <= 1)
            {
                return string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(Option1) || !string.IsNullOrEmpty(Option2) || !string.IsNullOrEmpty(Option3) || !string.IsNullOrEmpty(Option4) || !string.IsNullOrEmpty(Option5))
                    return string.Empty;
                return "hide";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            SnehBLL.ReportPreConsultMst_Bll RDB = new SnehBLL.ReportPreConsultMst_Bll();
            DataSet ds = RDB.Get_New(_appointmentID);
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            int tabValue = 1;
            SqlCommand cmd = new SqlCommand("Report_PreConsultantMst_Set_TABWISE");
            cmd.CommandType = CommandType.StoredProcedure;

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

            // Patient Information Start //
            //DateTime DatepreConsult = new DateTime();
            //if (txtDatepreConsult.Text.Trim().Length > 0)
            //{
            //    DateTime.TryParseExact(txtDatepreConsult.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatepreConsult);
            //}
            //DateTime DateBirth = new DateTime();
            //DateTime.TryParseExact(txtDateBirth.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth);
            //DateTime DateDelivery = new DateTime();
            //DateTime.TryParseExact(txtDateofDelivery.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateDelivery);
            string Gender = string.Empty;
            if (CheckFemale.Checked) { Gender = CheckFemale.Text.Trim(); }
            if (CheckMale.Checked) { Gender = CheckMale.Text.Trim(); }
            if (CheckOther.Checked) { Gender = CheckOther.Text.Trim(); }
            string ChildAttend = string.Empty;
            if (YesAttend.Checked) { ChildAttend = YesAttend.Text.Trim(); }
            if (Noattend.Checked) { ChildAttend = Noattend.Text.Trim(); }
            // Patient Information End //

            // Family History Start //
            string Consanguinity = string.Empty;
            if (CheckConsan.Checked) { Consanguinity = CheckConsan.Text.Trim(); }
            string Consanguinity_1 = string.Empty;
            if (CheckNonConsan.Checked) { Consanguinity_1 = CheckNonConsan.Text.Trim(); }
            string ConsanguinityDegree = string.Empty;
            if (Check1Deg.Checked) { ConsanguinityDegree = Check1Deg.Text.Trim(); }
            string ConsanguinityDegree_1 = string.Empty;
            if (Check2Deg.Checked) { ConsanguinityDegree_1 = Check2Deg.Text.Trim(); }
            string ConsanguinityDegree_2 = string.Empty;
            if (Check3Deg.Checked) { ConsanguinityDegree_2 = Check3Deg.Text.Trim(); }
            string FamilyStructure = string.Empty;
            if (CheckNuclear.Checked) { FamilyStructure = CheckNuclear.Text.Trim(); }
            string FamilyStructure_1 = string.Empty;
            if (CheckJoint.Checked) { FamilyStructure_1 = CheckJoint.Text.Trim(); }
            string Conception = string.Empty;
            if (CheckNatural.Checked) { Conception = CheckNatural.Text.Trim(); }
            string Conception_1 = string.Empty;
            if (CheckIUI.Checked) { Conception_1 = CheckIUI.Text.Trim(); }
            string Conception_2 = string.Empty;
            if (CheckIVF.Checked) { Conception_2 = CheckIVF.Text.Trim(); }
            string Conception_3 = string.Empty;
            if (CheckISCI.Checked) { Conception_3 = CheckISCI.Text.Trim(); }
            string Conception_4 = string.Empty;
            if (CheckOI.Checked) { Conception_4 = CheckOI.Text.Trim(); }
            string PlanningConception = string.Empty;
            if (CheckPlanned.Checked) { PlanningConception = CheckPlanned.Text.Trim(); }
            string PlanningConception_1 = string.Empty;
            if (CheckUnplanned.Checked) { PlanningConception_1 = CheckUnplanned.Text.Trim(); }
            string Siblings = string.Empty;
            if (AnySiblingsYes.Checked) { Siblings = AnySiblingsYes.Text.Trim(); }
            if (AnySiblingsNo.Checked) { Siblings = AnySiblingsNo.Text.Trim(); }
            // Family History End //

            // Family Relation Start //
            string InterParentalRelation = string.Empty;
            if (CheckPoor.Checked) { InterParentalRelation = CheckPoor.Text.Trim(); }
            string InterParentalRelation_1 = string.Empty;
            if (CheckFair.Checked) { InterParentalRelation_1 = CheckFair.Text.Trim(); }
            string InterParentalRelation_2 = string.Empty;
            if (CheckGood.Checked) { InterParentalRelation_2 = CheckGood.Text.Trim(); }
            string ParentChildRelation = string.Empty;
            if (CheckPoorr.Checked) { ParentChildRelation = CheckPoorr.Text.Trim(); }
            string ParentChildRelation_1 = string.Empty;
            if (CheckFairr.Checked) { ParentChildRelation_1 = CheckFairr.Text.Trim(); }
            string ParentChildRelation_2 = string.Empty;
            if (CheckGoodd.Checked) { ParentChildRelation_2 = CheckGoodd.Text.Trim(); }
            string InterSiblingRelation = string.Empty;
            if (Check_Poor.Checked) { InterSiblingRelation = Check_Poor.Text.Trim(); }
            string InterSiblingRelation_1 = string.Empty;
            if (Check_Fair.Checked) { InterSiblingRelation_1 = Check_Fair.Text.Trim(); }
            string InterSiblingRelation_2 = string.Empty;
            if (Check_Good.Checked) { InterSiblingRelation_2 = Check_Good.Text.Trim(); }
            string DomesticViolence = string.Empty;
            if (CheckYes.Checked) { DomesticViolence = CheckYes.Text.Trim(); }
            string DomesticViolence_1 = string.Empty;
            if (CheckNo.Checked) { DomesticViolence_1 = CheckNo.Text.Trim(); }
            string DomesticViolence_2 = string.Empty;
            if (CheckMaybe.Checked) { DomesticViolence_2 = CheckMaybe.Text.Trim(); }
            string FamilyRelocation = string.Empty;
            if (Check_Yes.Checked) { FamilyRelocation = Check_Yes.Text.Trim(); }
            string FamilyRelocation_1 = string.Empty;
            if (Check_No.Checked) { FamilyRelocation_1 = Check_No.Text.Trim(); }
            string PrimaryCare = string.Empty;
            if (CheckMother.Checked) { PrimaryCare = CheckMother.Text.Trim(); }
            string PrimaryCare_1 = string.Empty;
            if (CheckFather.Checked) { PrimaryCare_1 = CheckFather.Text.Trim(); }
            string PrimaryCare_2 = string.Empty;
            if (CheckGrandparents.Checked) { PrimaryCare_2 = CheckGrandparents.Text.Trim(); }
            string PrimaryCare_3 = string.Empty;
            if (CheckCaretaker.Checked) { PrimaryCare_3 = CheckCaretaker.Text.Trim(); }
            // Family Relation End //

            // Maternal History start //
            string MaternalStress = string.Empty;
            if (CheckPhysical.Checked) { MaternalStress = CheckPhysical.Text.Trim(); }
            string MaternalStress_1 = string.Empty;
            if (CheckMental.Checked) { MaternalStress_1 = CheckMental.Text.Trim(); }
            string Prenatalwellness = string.Empty;
            if (CheckYess.Checked) { Prenatalwellness = CheckYess.Text.Trim(); }
            if (CheckNoo.Checked) { Prenatalwellness = CheckNoo.Text.Trim(); }
            // Maternal History End //

            // Peri and Postnatal History Start //
            string delivery = string.Empty;
            if (rdoFTND.Checked) { delivery = rdoFTND.Text.Trim(); }
            string delivery_1 = string.Empty;
            if (rdoFTNDva.Checked) { delivery_1 = rdoFTNDva.Text.Trim(); }
            string delivery_2 = string.Empty;
            if (rdoELSCS.Checked) { delivery_2 = rdoELSCS.Text.Trim(); }
            string delivery_3 = string.Empty;
            if (rdoElectiveLSCS.Checked) { delivery_3 = rdoElectiveLSCS.Text.Trim(); }
            string ciab = string.Empty;
            if (rdoYess.Checked) { ciab = rdoYess.Text.Trim(); }
            if (rdoNoo.Checked) { ciab = rdoNoo.Text.Trim(); }
            string GestationalBirthAge = string.Empty;
            if (RdoAGA.Checked) { GestationalBirthAge = RdoAGA.Text.Trim(); }
            string GestationalBirthAge_1 = string.Empty;
            if (RdoSGA.Checked) { GestationalBirthAge_1 = RdoSGA.Text.Trim(); }
            string GestationalBirthAge_2 = string.Empty;
            if (RdoLGA.Checked) { GestationalBirthAge_2 = RdoLGA.Text.Trim(); }
            string NICUstay = string.Empty;
            if (RdoPresent.Checked) { NICUstay = RdoPresent.Text.Trim(); }
            if (RdoAbsent.Checked) { NICUstay = RdoAbsent.Text.Trim(); }
            string Breastfed = string.Empty;
            if (RdoYes.Checked) { Breastfed = RdoYes.Text.Trim(); }
            if (RdoNo.Checked) { Breastfed = RdoNo.Text.Trim(); }
            string Problemsduringbreastfeeding = string.Empty;
            if (RadioPresent.Checked) { Problemsduringbreastfeeding = RadioPresent.Text.Trim(); }
            if (RadioAbsent.Checked) { Problemsduringbreastfeeding = RadioAbsent.Text.Trim(); }
            string colicissue = string.Empty;
            if (RadioYes.Checked) { colicissue = RadioYes.Text.Trim(); }
            if (RadioNo.Checked) { colicissue = RadioNo.Text.Trim(); }
            // Peri and Postnatal History End //

            // Sleep Start //
            string Sleepissues = string.Empty;
            if (RadiooNo.Checked) { Sleepissues = RadiooNo.Text.Trim(); }
            if (RadiooYes.Checked) { Sleepissues = RadiooYes.Text.Trim(); }
            string Presentsleep = string.Empty;
            if (PresentRadio.Checked) { Presentsleep = PresentRadio.Text.Trim(); }
            if (AbsentRadio.Checked) { Presentsleep = AbsentRadio.Text.Trim(); }
            string SleepType = string.Empty;
            if (RadioLight.Checked) { SleepType = RadioLight.Text.Trim(); }
            if (RadioDeep.Checked) { SleepType = RadioDeep.Text.Trim(); }
            string Cosleeping = string.Empty;
            if (RadioAbsentbtn.Checked) { Cosleeping = RadioAbsentbtn.Text.Trim(); }
            if (RadioPresentbtn.Checked) { Cosleeping = RadioPresentbtn.Text.Trim(); }
            string Naptime = string.Empty;
            if (RadioButtonPresent.Checked) { Naptime = RadioButtonPresent.Text.Trim(); }
            if (RadioButtonAbsent.Checked) { Naptime = RadioButtonAbsent.Text.Trim(); }
            // Sleep end //

            // Feeding habits Start //
            string Feedinghabits = string.Empty;
            if (RadioTypical.Checked) { Feedinghabits = RadioTypical.Text.Trim(); }
            if (RadioAtypical.Checked) { Feedinghabits = RadioAtypical.Text.Trim(); }
            // Feeding habits end //

            // Into the Child's Heart start //
            //string signsofstress = string.Empty;
            //if (RadioButtonYes.Checked) { signsofstress = RadioButtonYes.Text.Trim(); }
            //if (RadioButtonNo.Checked) { signsofstress = RadioButtonNo.Text.Trim(); }
            //if (RadioButtonMaybe.Checked) { signsofstress = RadioButtonMaybe.Text.Trim(); }
            // Into the Child's Heart end //

            // Play Behaviour start //
            string Playbehaviour = string.Empty;
            if (RadioOrganised.Checked) { Playbehaviour = RadioOrganised.Text.Trim(); }
            if (RadioDisorganised.Checked) { Playbehaviour = RadioDisorganised.Text.Trim(); }
            string Strangeranxiety = string.Empty;
            if (RadioPresentButton.Checked) { Strangeranxiety = RadioPresentButton.Text.Trim(); }
            if (RadioAbsentButton.Checked) { Strangeranxiety = RadioAbsentButton.Text.Trim(); }
            string PlayToys = string.Empty;
            if (RadioYesButton.Checked) { PlayToys = RadioYesButton.Text.Trim(); }
            if (RadioNoButton.Checked) { PlayToys = RadioNoButton.Text.Trim(); }
            if (RadioMaybeButton.Checked) { PlayToys = RadioMaybeButton.Text.Trim(); }
            // Play Behaviour end //

            // ADLs start //
            string Brushing = string.Empty;
            if (RadioDependent.Checked) { Brushing = RadioDependent.Text.Trim(); }
            string Brushing_1 = string.Empty;
            if (RadioAssisted.Checked) { Brushing_1 = RadioAssisted.Text.Trim(); }
            string Brushing_2 = string.Empty;
            if (RadioIndependent.Checked) { Brushing_2 = RadioIndependent.Text.Trim(); }
            string Bathing = string.Empty;
            if (DependentRadio.Checked) { Bathing = DependentRadio.Text.Trim(); }
            string Bathing_1 = string.Empty;
            if (AssistedRadio.Checked) { Bathing_1 = AssistedRadio.Text.Trim(); }
            string Bathing_2 = string.Empty;
            if (IndependentRadio.Checked) { Bathing_2 = IndependentRadio.Text.Trim(); }
            string Toileting = string.Empty;
            if (RadioDependentButton.Checked) { Toileting = RadioDependentButton.Text.Trim(); }
            string Toileting_1 = string.Empty;
            if (RadioAssistedButton.Checked) { Toileting_1 = RadioAssistedButton.Text.Trim(); }
            string Toileting_2 = string.Empty;
            if (RadioIndependentButton.Checked) { Toileting_2 = RadioIndependentButton.Text.Trim(); }
            string Dressing = string.Empty;
            if (RadioButtonDependent.Checked) { Dressing = RadioButtonDependent.Text.Trim(); }
            string Dressing_1 = string.Empty;
            if (RadioButtonAssisted.Checked) { Dressing_1 = RadioButtonAssisted.Text.Trim(); }
            string Dressing_2 = string.Empty;
            if (RadioButtonIndependent.Checked) { Dressing_2 = RadioButtonIndependent.Text.Trim(); }
            string Eating = string.Empty;
            if (RadioBtnDependent.Checked) { Eating = RadioBtnDependent.Text.Trim(); }
            string Eating_1 = string.Empty;
            if (RadioBtnAssisted.Checked) { Eating_1 = RadioBtnAssisted.Text.Trim(); }
            string Eating_2 = string.Empty;
            if (RadioBtnIndependent.Checked) { Eating_2 = RadioBtnIndependent.Text.Trim(); }
            string Ambulation = string.Empty;
            if (RdoDependent.Checked) { Ambulation = RdoDependent.Text.Trim(); }
            string Ambulation_1 = string.Empty;
            if (RdoAssisted.Checked) { Ambulation_1 = RdoAssisted.Text.Trim(); }
            string Ambulation_2 = string.Empty;
            if (RdoIndependent.Checked) { Ambulation_2 = RdoIndependent.Text.Trim(); }
            string Transfers = string.Empty;
            if (RdobtnDependent.Checked) { Transfers = RdobtnDependent.Text.Trim(); }
            string Transfers_1 = string.Empty;
            if (RdobtnAssisted.Checked) { Transfers_1 = RdobtnAssisted.Text.Trim(); }
            string Transfers_2 = string.Empty;
            if (RdobtnIndependent.Checked) { Transfers_2 = RdobtnIndependent.Text.Trim(); }
            // ADLs end //


            switch (this.hfdTabs.Value)
            {
                case "":
                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel13_tab":
                    #region ===== Tab 1 =====

                    tabValue = 1;

                    //DateTime DatepreConsult = new DateTime();
                    //if (txtDatepreConsult.Text.Trim().Length > 0)
                    //{
                    //    DateTime.TryParseExact(txtDatepreConsult.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatepreConsult);
                    //}
                    //DateTime DateBirth = new DateTime();
                    //DateTime.TryParseExact(txtDateBirth.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth);
                    //DateTime DateDelivery = new DateTime();
                    //DateTime.TryParseExact(txtDateofDelivery.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateDelivery);

                    //cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    //cmd.Parameters.AddWithValue("@DatepreConsult", txtDatepreConsult.Text);
                    //cmd.Parameters.AddWithValue("@ComfortableLanguage", txtComfortableLanguage.Text);
                    ////cmd.Parameters.AddWithValue("@DateBirth", txtDateBirth.Text);
                    ////if (DateBirth > DateTime.MinValue)
                    ////{
                    ////    cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DateBirth;
                    ////}
                    ////else
                    ////{
                    ////    cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DBNull.Value;
                    ////}
                    //////cmd.Parameters.AddWithValue("@DateDelivery", txtDateofDelivery.Text);
                    ////if (DateDelivery > DateTime.MinValue)
                    ////{
                    ////    cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DateDelivery;
                    ////}
                    ////else
                    ////{
                    ////    cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DBNull.Value;
                    ////}

                    //if (DatepreConsult > DateTime.MinValue)
                    //    cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DatepreConsult;
                    //else
                    //    cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DBNull.Value;

                    //if (DateBirth > DateTime.MinValue)
                    //    cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DateBirth;
                    //else
                    //    cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DBNull.Value;

                    //if (DateDelivery > DateTime.MinValue)
                    //    cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DateDelivery;
                    //else
                    //    cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DBNull.Value;



                    //cmd.Parameters.AddWithValue("@CorrectAge", txtCorrectAge.Text);
                    //cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    //cmd.Parameters.AddWithValue("@Gender", Gender);
                    //cmd.Parameters.AddWithValue("@MotherName", txtMotherName.Text);
                    //cmd.Parameters.AddWithValue("@MotherAge", txtMotherAge.Text);
                    //cmd.Parameters.AddWithValue("@MotherQualification", txtMotherQualification.Text);
                    //cmd.Parameters.AddWithValue("@MotherOccupation", txtMotherOccupation.Text);
                    //cmd.Parameters.AddWithValue("@MotherWorkingHour", txtMotherWorkingHour.Text);
                    //cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                    //cmd.Parameters.AddWithValue("@FatherAge", txtFatherAge.Text);
                    //cmd.Parameters.AddWithValue("@FatherOccupation", txtFatherOccupation.Text);
                    //cmd.Parameters.AddWithValue("@FatherQualification", txtFatherQualification.Text);
                    //cmd.Parameters.AddWithValue("@FatherWorkingHour", txtFatherWorkingHour.Text);
                    //cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    //cmd.Parameters.AddWithValue("@ContactDetails", txtContactDetails.Text);
                    //cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
                    //cmd.Parameters.AddWithValue("@ReferredBy", txtReferredBy.Text);
                    //cmd.Parameters.AddWithValue("@TherapistDuringPC", txtTherapistDuringPC.Text);
                    //cmd.Parameters.AddWithValue("@Diagnosis", txtDiagnosis.Text);
                    //cmd.Parameters.AddWithValue("@CommentsPI", txtCommentsPI.Text);
                    //cmd.Parameters.AddWithValue("@RetVal", 1);
                    //cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    //db.DbUpdate(cmd);

                    //this.tb_Contents.ActiveTabIndex = tabValue;
                    //this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel1_tab";
                    DateTime DatepreConsult = new DateTime();
                    if (txtDatepreConsult.Text.Trim().Length > 0)
                    {
                        DateTime.TryParseExact(txtDatepreConsult.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DatepreConsult);
                    }
                    DateTime DateBirth = new DateTime();
                    DateTime.TryParseExact(txtDateBirth.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth);
                    DateTime DateDelivery = new DateTime();
                    DateTime.TryParseExact(txtDateofDelivery.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateDelivery);
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@ModifyDate", DateTime.UtcNow.AddMinutes(330));
                    cmd.Parameters.AddWithValue("@ModifyBy", _loginID);

                    cmd.Parameters.AddWithValue("@ComfortableLanguage", txtComfortableLanguage.Text);

                    if (DatepreConsult > DateTime.MinValue)
                        cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DatepreConsult;
                    else
                        cmd.Parameters.Add("@DatepreConsult", SqlDbType.DateTime).Value = DBNull.Value;

                    if (DateBirth > DateTime.MinValue)
                        cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DateBirth;
                    else
                        cmd.Parameters.Add("@DateBirth", SqlDbType.DateTime).Value = DBNull.Value;

                    if (DateDelivery > DateTime.MinValue)
                        cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DateDelivery;
                    else
                        cmd.Parameters.Add("@DateDelivery", SqlDbType.DateTime).Value = DBNull.Value;


                    cmd.Parameters.AddWithValue("@CorrectAge", txtCorrectAge.Text);
                    cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@ChildAttend", ChildAttend);
                    cmd.Parameters.AddWithValue("@MotherName", txtMotherName.Text);
                    cmd.Parameters.AddWithValue("@OnlineOffline", txtOnlineOffline.Text);
                    cmd.Parameters.AddWithValue("@WhichGrade", txtWhichGrade.Text);
                    cmd.Parameters.AddWithValue("@MotherAge", txtMotherAge.Text);
                    cmd.Parameters.AddWithValue("@MotherQualification", txtMotherQualification.Text);
                    cmd.Parameters.AddWithValue("@MotherOccupation", txtMotherOccupation.Text);
                    cmd.Parameters.AddWithValue("@MotherWorkingHour", txtMotherWorkingHour.Text);
                    cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                    cmd.Parameters.AddWithValue("@FatherAge", txtFatherAge.Text);
                    cmd.Parameters.AddWithValue("@FatherOccupation", txtFatherOccupation.Text);
                    cmd.Parameters.AddWithValue("@FatherQualification", txtFatherQualification.Text);
                    cmd.Parameters.AddWithValue("@FatherWorkingHour", txtFatherWorkingHour.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@ContactDetails", txtContactDetails.Text);
                    cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
                    cmd.Parameters.AddWithValue("@ReferredBy", txtReferredBy.Text);
                    cmd.Parameters.AddWithValue("@TherapistDuringPC", txtTherapistDuringPC.Text);
                    cmd.Parameters.AddWithValue("@Diagnosis", txtDiagnosis.Text);
                    cmd.Parameters.AddWithValue("@CommentsPI", txtCommentsPI.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel1_tab";
                    #endregion

                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel1_tab":
                    #region ===== Tab 2 =====
                    tabValue = 2;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@ChiefConcernsHome", txtChiefConcernsHome.Text);
                    cmd.Parameters.AddWithValue("@ChiefConcernsSchool", txtChiefConcernsSchool.Text);
                    cmd.Parameters.AddWithValue("@ChiefConcernsSocialGath", txtChiefConcernsSocialGath.Text);
                    cmd.Parameters.AddWithValue("@CommentsCC", txtCommentsCC.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel0_tab";
                    #endregion

                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel0_tab":
                    #region ===== Tab 3 =====
                    tabValue = 3;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);

                    
                    int HidPreConsultID = 0;
                    string Option1 = string.Empty;
                    string Option2 = string.Empty;
                    string Option3 = string.Empty;
                    string Option4 = string.Empty;
                    string Option5 = string.Empty;

                    for (int j = 1; j <= OptionCount; j++)
                    {
                        RepeaterItem item = txtSignleChoice.Items.Count >= j ? txtSignleChoice.Items[j - 1] : null;
                        if (item != null)
                        {
                            HiddenField PreConsultID = item.FindControl("txtPreConsultID") as HiddenField;
                            TextBox DateMonth = item.FindControl("txtDateMonth") as TextBox;
                            TextBox RelevantHistory = item.FindControl("txtRelevantHistory") as TextBox;
                            TextBox HospitalDoctorsVisited = item.FindControl("txtHospitalDoctorsVisited") as TextBox;
                            TextBox DoctorsRecommendations = item.FindControl("txtDoctorsRecommendations") as TextBox;
                            TextBox InvestigationsRecordsResults = item.FindControl("txtInvestigationsRecordsResults") as TextBox;
                            if (DateMonth.Text != "" || RelevantHistory.Text != "" || HospitalDoctorsVisited.Text != "" || DoctorsRecommendations.Text != "" || InvestigationsRecordsResults.Text != "")
                            {
                                int.TryParse(PreConsultID.Value.ToString(), out HidPreConsultID);

                                Option1 = DateMonth.Text.Trim();

                                Option2 = RelevantHistory.Text.Trim();

                                Option3 = HospitalDoctorsVisited.Text.Trim();

                                Option4 = DoctorsRecommendations.Text.Trim();

                                Option5 = InvestigationsRecordsResults.Text.Trim();

                                int k = RDB.SetTimeLine(_appointmentID, HidPreConsultID, Option1, Option2, Option3, Option4, Option5, DateTime.UtcNow.AddMinutes(330), _loginID);

                            }
                            else if (DateMonth.Text == "" && RelevantHistory.Text == "" && HospitalDoctorsVisited.Text == "" && DoctorsRecommendations.Text == "" && InvestigationsRecordsResults.Text == "")
                            {
                                int.TryParse(PreConsultID.Value.ToString(), out HidPreConsultID);
                                int P = RDB.DeleteRow(HidPreConsultID);
                            }
                        }
                    }
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    ds = RDB.Get_New(_appointmentID);
                    List<optionMdel> qls = new List<optionMdel>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        string dateMonth = string.Empty; string relevantHistory = string.Empty; string hospitalDoctorsVisited = string.Empty;
                        string doctorsRecommendations = string.Empty; string investigationsRecordsResults = string.Empty;

                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["DateMonth"].ToString()))
                        {
                            dateMonth = ds.Tables[1].Rows[i]["DateMonth"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["RelevantHistory"].ToString()))
                        {
                            relevantHistory = ds.Tables[1].Rows[i]["RelevantHistory"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["HospitalDoctorsVisited"].ToString()))
                        {
                            hospitalDoctorsVisited = ds.Tables[1].Rows[i]["HospitalDoctorsVisited"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["DoctorsRecommendations"].ToString()))
                        {
                            doctorsRecommendations = ds.Tables[1].Rows[i]["DoctorsRecommendations"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["InvestigationsRecordsResults"].ToString()))
                        {
                            investigationsRecordsResults = ds.Tables[1].Rows[i]["InvestigationsRecordsResults"].ToString();
                        }

                        qls.Add(new optionMdel
                        {
                            Option = ds.Tables[1].Rows[i]["PreConsultID"].ToString(),
                            Option1 = dateMonth,
                            Option2 = relevantHistory,
                            Option3 = hospitalDoctorsVisited,
                            Option4 = doctorsRecommendations,
                            Option5 = investigationsRecordsResults
                        });
                    }

                    int temp = qls.Count; txtVisibleOption.Value = qls.Count.ToString();
                    for (int jl = 0; jl < (OptionCount - temp); jl++)
                    {
                        qls.Add(new optionMdel() { Option = string.Empty });
                    }
                    txtSignleChoice.DataSource = qls;
                    txtSignleChoice.DataBind();

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel2_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel2_tab":

                    #region ===== Tab 4 =====
                    tabValue = 4;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Consanguinity", Consanguinity);
                    cmd.Parameters.AddWithValue("@Consanguinity_1", Consanguinity_1);
                    cmd.Parameters.AddWithValue("@ConsanguinityDegree", ConsanguinityDegree);
                    cmd.Parameters.AddWithValue("@ConsanguinityDegree_1", ConsanguinityDegree_1);
                    cmd.Parameters.AddWithValue("@ConsanguinityDegree_2", ConsanguinityDegree_2);
                    cmd.Parameters.AddWithValue("@YearsMarriage", txtYearsMarriage.Text);
                    cmd.Parameters.AddWithValue("@FamilyStructure", FamilyStructure);
                    cmd.Parameters.AddWithValue("@FamilyStructure_1", FamilyStructure_1);
                    cmd.Parameters.AddWithValue("@Conception", Conception);
                    cmd.Parameters.AddWithValue("@Conception_1", Conception_1);
                    cmd.Parameters.AddWithValue("@Conception_2", Conception_2);
                    cmd.Parameters.AddWithValue("@Conception_3", Conception_3);
                    cmd.Parameters.AddWithValue("@Conception_4", Conception_4);
                    cmd.Parameters.AddWithValue("@PlanningConception", PlanningConception);
                    cmd.Parameters.AddWithValue("@PlanningConception_1", PlanningConception_1);
                    cmd.Parameters.AddWithValue("@Siblings", Siblings);
                    cmd.Parameters.AddWithValue("@CommentsFH", txtCommentsFH.Text);
                    cmd.Parameters.AddWithValue("@NoOfSiblings", txtNoOfSiblings.Text);
                    cmd.Parameters.AddWithValue("@RHASiblings", txtRHASiblings.Text);
                    cmd.Parameters.AddWithValue("@InterParentalRelation", InterParentalRelation);
                    cmd.Parameters.AddWithValue("@InterParentalRelation_1", InterParentalRelation_1);
                    cmd.Parameters.AddWithValue("@InterParentalRelation_2", InterParentalRelation_2);
                    cmd.Parameters.AddWithValue("@ParentChildRelation", ParentChildRelation);
                    cmd.Parameters.AddWithValue("@ParentChildRelation_1", ParentChildRelation_1);
                    cmd.Parameters.AddWithValue("@ParentChildRelation_2", ParentChildRelation_2);
                    cmd.Parameters.AddWithValue("@InterSiblingRelation", InterSiblingRelation);
                    cmd.Parameters.AddWithValue("@InterSiblingRelation_1", InterSiblingRelation_1);
                    cmd.Parameters.AddWithValue("@InterSiblingRelation_2", InterSiblingRelation_2);
                    cmd.Parameters.AddWithValue("@DomesticViolence", DomesticViolence);
                    cmd.Parameters.AddWithValue("@DomesticViolence_1", DomesticViolence_1);
                    cmd.Parameters.AddWithValue("@DomesticViolence_2", DomesticViolence_2);
                    cmd.Parameters.AddWithValue("@FamilyRelocation", FamilyRelocation);
                    cmd.Parameters.AddWithValue("@FamilyRelocation_1", FamilyRelocation_1);
                    cmd.Parameters.AddWithValue("@frequency", txtfrequency.Text);
                    cmd.Parameters.AddWithValue("@PrimaryCare", PrimaryCare);
                    cmd.Parameters.AddWithValue("@PrimaryCare_1", PrimaryCare_1);
                    cmd.Parameters.AddWithValue("@PrimaryCare_2", PrimaryCare_2);
                    cmd.Parameters.AddWithValue("@PrimaryCare_3", PrimaryCare_3);
                    cmd.Parameters.AddWithValue("@MotherScreenTime", txtMotherScreenTime.Text);
                    cmd.Parameters.AddWithValue("@ScreenTimeChild", txtScreenTimeChild.Text);
                    cmd.Parameters.AddWithValue("@CommentsFR", txtCommentsFR.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel4_tab";
                    #endregion
                    break;


                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel4_tab":
                    #region ===== Tab 5  =====
                    tabValue = 5;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@PrenatalCondition", txtPrenatalCondition.Text);
                    cmd.Parameters.AddWithValue("@CheckMental", MaternalStress_1);
                    cmd.Parameters.AddWithValue("@MaternalStress_1", MaternalStress_1);
                    cmd.Parameters.AddWithValue("@DescribeStressors", txtDescribeStressors.Text);
                    cmd.Parameters.AddWithValue("@WGDP", txtWGDP.Text);
                    cmd.Parameters.AddWithValue("@FoetalMovement", txtFoetalMovement.Text);
                    cmd.Parameters.AddWithValue("@Prenatalwellness", Prenatalwellness);
                    cmd.Parameters.AddWithValue("@CommentsMH", txtCommentsMH.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel5_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel5_tab":
                    #region ===== Tab 6  =====
                    tabValue = 6;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@DurationLabour", txtDurationLabour.Text);
                    cmd.Parameters.AddWithValue("@delivery", delivery);
                    cmd.Parameters.AddWithValue("@delivery_1", delivery_1);
                    cmd.Parameters.AddWithValue("@delivery_2", delivery_2);
                    cmd.Parameters.AddWithValue("@delivery_3", delivery_3);
                    cmd.Parameters.AddWithValue("@ciab", ciab);
                    cmd.Parameters.AddWithValue("@ConditionPostBirth", txtConditionPostBirth.Text);
                    cmd.Parameters.AddWithValue("@BirthWeight", txtBirthWeight.Text);
                    cmd.Parameters.AddWithValue("@GestationalBirthAge", GestationalBirthAge);
                    cmd.Parameters.AddWithValue("@GestationalBirthAge_1", GestationalBirthAge_1);
                    cmd.Parameters.AddWithValue("@GestationalBirthAge_2", GestationalBirthAge_1);
                    cmd.Parameters.AddWithValue("@NICUstay", NICUstay);
                    cmd.Parameters.AddWithValue("@DurationNICUstay", txtDurationNICUstay.Text);
                    cmd.Parameters.AddWithValue("@NICUHistory", txtNICUHistory.Text);
                    cmd.Parameters.AddWithValue("@ReasonNICUstay", txtReasonNICUstay.Text);
                    cmd.Parameters.AddWithValue("@APGARscore", txtAPGARscore.Text);
                    cmd.Parameters.AddWithValue("@Breastfed", Breastfed);
                    cmd.Parameters.AddWithValue("@BabyFed", txtBabyFed.Text);
                    cmd.Parameters.AddWithValue("@Problemsduringbreastfeeding", Problemsduringbreastfeeding);
                    cmd.Parameters.AddWithValue("@MentionProblem", txtMentionProblem.Text);
                    cmd.Parameters.AddWithValue("@waswtcbf", txtwaswtcbf.Text);
                    cmd.Parameters.AddWithValue("@colicissue", colicissue);
                    cmd.Parameters.AddWithValue("@OthrtMedicalIssues", txtOthrtMedicalIssues.Text);
                    cmd.Parameters.AddWithValue("@CommentsPPH", txtCommentsPPH.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel6_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel6_tab":
                    #region ===== Tab 7  =====
                    tabValue = 7;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@GrossMotor", txtGrossMotor.Text);
                    cmd.Parameters.AddWithValue("@FineMotor", txtFineMotor.Text);
                    cmd.Parameters.AddWithValue("@PersonalandSocial", txtPersonalandSocial.Text);
                    cmd.Parameters.AddWithValue("@Communication", txtCommunication.Text);
                    cmd.Parameters.AddWithValue("@CommentsDM", txtCommentsDM.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel7_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel7_tab":
                    #region ===== Tab 8  =====
                    tabValue = 8;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Sleepissues", Sleepissues);
                    cmd.Parameters.AddWithValue("@Presentsleep", Presentsleep);
                    cmd.Parameters.AddWithValue("@Sleepduration", txtSleepduration.Text);
                    cmd.Parameters.AddWithValue("@SleepType", SleepType);
                    cmd.Parameters.AddWithValue("@Cosleeping", Cosleeping);
                    cmd.Parameters.AddWithValue("@Cosleepingwith", txtCosleepingwith.Text);
                    cmd.Parameters.AddWithValue("@AnySleepAdjunctsused", txtAnySleepAdjunctsused.Text);
                    cmd.Parameters.AddWithValue("@Naptime", Naptime);
                    cmd.Parameters.AddWithValue("@Napduration", txtNapduration.Text);
                    cmd.Parameters.AddWithValue("@CommentsS", txtCommentsS.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel8_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel8_tab":
                    #region ===== Tab 9  =====
                    tabValue = 9;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Feedinghabits", Feedinghabits);
                    cmd.Parameters.AddWithValue("@Typeoffoodhad", txtTypeoffoodhad.Text);
                    cmd.Parameters.AddWithValue("@Foodconsistency", txtFoodconsistency.Text);
                    cmd.Parameters.AddWithValue("@Foodtemperature", txtFoodtemperature.Text);
                    cmd.Parameters.AddWithValue("@Foodtaste", txtFoodtaste.Text);
                    cmd.Parameters.AddWithValue("@CommentsFeHa", txtCommentsFeHa.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel9_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel9_tab":
                    #region ===== Tab 10  =====
                    tabValue = 10;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@ChildLikes", txtChildLikes.Text);
                    cmd.Parameters.AddWithValue("@CommentsITCH", txtCommentsITCH.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel10_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel10_tab":
                    #region ===== Tab 11  =====
                    tabValue = 11;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Playbehaviour", Playbehaviour);
                    cmd.Parameters.AddWithValue("@Interactionwithpeers", txtInteractionwithpeers.Text);
                    cmd.Parameters.AddWithValue("@Strangeranxiety", Strangeranxiety);
                    cmd.Parameters.AddWithValue("@PlayToys", PlayToys);
                    cmd.Parameters.AddWithValue("@Preferenceoftoys", txtPreferenceoftoys.Text);
                    cmd.Parameters.AddWithValue("@CommentsPB", txtCommentsPB.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel11_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel11_tab":
                    #region ===== Tab 12  =====
                    tabValue = 12;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@Brushing", Brushing);
                    cmd.Parameters.AddWithValue("@Brushing_1", Brushing_1);
                    cmd.Parameters.AddWithValue("@Brushing_2", Brushing_2);
                    cmd.Parameters.AddWithValue("@CommentsBrushing", txtCommentsBrushing.Text);
                    cmd.Parameters.AddWithValue("@Bathing", Bathing);
                    cmd.Parameters.AddWithValue("@Bathing_1", Bathing_1);
                    cmd.Parameters.AddWithValue("@Bathing_2", Bathing_2);
                    cmd.Parameters.AddWithValue("@CommentsBathing", txtCommentsBathing.Text);
                    cmd.Parameters.AddWithValue("@Toileting", Toileting);
                    cmd.Parameters.AddWithValue("@Toileting_1", Toileting_1);
                    cmd.Parameters.AddWithValue("@Toileting_2", Toileting_2);
                    cmd.Parameters.AddWithValue("@CommentsToileting", txtCommentsToileting.Text);
                    cmd.Parameters.AddWithValue("@Dressing", Dressing);
                    cmd.Parameters.AddWithValue("@Dressing_1", Dressing_1);
                    cmd.Parameters.AddWithValue("@Dressing_2", Dressing_2);
                    cmd.Parameters.AddWithValue("@CommentsDressing", txtCommentsDressing.Text);
                    cmd.Parameters.AddWithValue("@Eating", Eating);
                    cmd.Parameters.AddWithValue("@Eating_1", Eating_1);
                    cmd.Parameters.AddWithValue("@Eating_2", Eating_2);
                    cmd.Parameters.AddWithValue("@CommentsEating", txtCommentsEating.Text);
                    cmd.Parameters.AddWithValue("@Ambulation", Ambulation);
                    cmd.Parameters.AddWithValue("@Ambulation_1", Ambulation_1);
                    cmd.Parameters.AddWithValue("@Ambulation_2", Ambulation_2);
                    cmd.Parameters.AddWithValue("@CommentsAmbulation", txtCommentsAmbulation.Text);
                    cmd.Parameters.AddWithValue("@Transfers", Transfers);
                    cmd.Parameters.AddWithValue("@Transfers_1", Transfers_1);
                    cmd.Parameters.AddWithValue("@Transfers_2", Transfers_2);
                    cmd.Parameters.AddWithValue("@CommentsTransfers", txtCommentsTransfers.Text);


                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel14_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel14_tab":
                    #region ===== Tab 13  =====
                    tabValue = 13;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@AddComments", txtAddComments.Text);

                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel3_tab";
                    #endregion
                    break;

                case "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel3_tab":
                    #region ===== Tab 14  =====
                    tabValue = 14;
                    cmd.Parameters.AddWithValue("@AppointmentID", _appointmentID);
                    cmd.Parameters.AddWithValue("@AddEvalRec", txtAddEvalRec.Text);
                    cmd.Parameters.AddWithValue("@RetVal", 1);
                    cmd.Parameters.AddWithValue("@TabNo", tabValue);
                    db.DbUpdate(cmd);

                    this.tb_Contents.ActiveTabIndex = tabValue - 14;
                    this.hfdTabs.Value = "ctl00_ContentPlaceHolder1_tb_Contents_TabPanel3_tab";
                    #endregion
                    break;

            }
        }
    }
}