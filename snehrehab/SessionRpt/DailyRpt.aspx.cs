using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace snehrehab.SessionRpt
{
    public partial class DailyRpt : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        int PatientID = 0;
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
            _cancelUrl = "/SessionRpt/DailyView.aspx";
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
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();

            if (!RDB.IsValid(_appointmentID))
            {
                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
            }
            if (!RDB.IsDailyRpt_Type0(_appointmentID))
            {
                if (RDB.IsDailyRpt_Type1(_appointmentID))
                {
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/DailyRpt1.aspx?record=" + Request.QueryString["record"].ToString()), true); return;
                }
                else
                {
                    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
                }
            }
           // tb_Contents.ActiveTabIndex = 0; tb_Report1.Enabled = true; tb_Report2.Enabled = false;

            txtImpairements.DataSource = RDB.impairement_GetList();
            txtImpairements.DataTextField = "Impairements";
            txtImpairements.DataValueField = "ImpairementID";
            txtImpairements.DataBind();

            //txtPerformance.DataSource = RDB.Performance_GetList();
            //txtPerformance.DataTextField = "Performance";
            //txtPerformance.DataValueField = "PerformanceID";
            //txtPerformance.DataBind();

            txtGoalAssScale.DataSource = RDB.GoalScale_GetList();
            txtGoalAssScale.DataTextField = "Scale";
            txtGoalAssScale.DataValueField = "ScaleID";
            txtGoalAssScale.DataBind();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            foreach (SnehDLL.Diagnosis_Dll DMD in DIB.Dropdown())
            {
                txtDiagnosis.Items.Add(new ListItem(DMD.dName, DMD.DiagnosisID.ToString()));
            }


            DataSet ds = RDB.Get(_appointmentID);
            if (ds.Tables.Count > 0)
            {
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
                if (HasDiagnosisID) { PanelDiagnosis.Visible = true; } else { PanelDiagnosis.Visible = true; }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (HasDiagnosisID)
                    {
                        PanelDiagnosis.Visible = true;
                        //string[] DiagnosisIDs = ds.Tables[1].Rows[0]["DiagnosisID"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        //for (int i = 0; DiagnosisIDs != null && i < DiagnosisIDs.Length; i++)
                        //{
                        //    for (int j = 0; j < txtDiagnosis.Items.Count; j++)
                        //    {
                        //        if (txtDiagnosis.Items[j].Value == DiagnosisIDs[i])
                        //        {
                        //            txtDiagnosis.Items[j].Selected = true; break;
                        //        }
                        //    }
                        //}
                        //txtDiagnosisOther.Text = ds.Tables[1].Rows[0]["DiagnosisOther"].ToString();
                    }
                    else
                    {
                        PanelDiagnosis.Visible = true;
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtSessionGoal.Text = ds.Tables[1].Rows[0]["SessionGoal"].ToString();
                    txtActivity.Text = ds.Tables[1].Rows[0]["Activity"].ToString();
                   // txtEquipments.Text = ds.Tables[1].Rows[0]["Equipments"].ToString();
                    //int _performanceID = 0; int.TryParse(ds.Tables[1].Rows[0]["PerformanceID"].ToString(), out _performanceID);
                    //if (txtPerformance.Items.FindByValue(_performanceID.ToString()) != null)
                   // {
                    //    txtPerformance.SelectedValue = _performanceID.ToString();
                   // }
                    int _goalAssScaleID = 0; int.TryParse(ds.Tables[1].Rows[0]["GoalAssScaleID"].ToString(), out _goalAssScaleID);
                    if (txtGoalAssScale.Items.FindByValue(_goalAssScaleID.ToString()) != null)
                    {
                        txtGoalAssScale.SelectedValue = _goalAssScaleID.ToString();
                    }
                    //txtLongTerm.Text = ds.Tables[1].Rows[0]["LongTermGoals"].ToString();
                    //txtShortTerm.Text = ds.Tables[1].Rows[0]["ShortTermGoals"].ToString();
                    txtSuggestions.Text = ds.Tables[1].Rows[0]["Suggestions"].ToString();


                    DataTable dt = RDB.GetAttr(_appointmentID, RDB._impairementTypeID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int _attrID = 0; int.TryParse(dr["AttributeID"].ToString(), out _attrID);
                        foreach (ListItem li in txtImpairements.Items)
                        {
                            if (li.Value == _attrID.ToString())
                            {
                                li.Selected = true; break;
                            }
                        }
                    }

                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";

                }
            }
        }

        //protected void btnNext_Click(object sender, EventArgs e)
        //{
        //    tb_Contents.ActiveTabIndex = 1; tb_Report1.Enabled = false; tb_Report2.Enabled = true;
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
           // int _performanceID = 0; if (txtPerformance.SelectedItem != null) { int.TryParse(txtPerformance.SelectedItem.Value, out _performanceID); }
            int _goalAssScaleID = 0; if (txtGoalAssScale.SelectedItem != null) { int.TryParse(txtGoalAssScale.SelectedItem.Value, out _goalAssScaleID); }
            string DiagnosisIDs = "";
            for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            {
                if (txtDiagnosis.Items[k].Selected)
                {
                    DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
                }
            }
            DataSet ds = RDB.Get(_appointmentID);
            int.TryParse(ds.Tables[0].Rows[0]["PatientID"].ToString(), out PatientID);
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            int g = 0;
            g = DIB.setFromOther(DiagnosisIDs, DiagnosisOther, PatientID);
            if (g < 0)
            {
                DbHelper.Configuration.setAlert(Page, "Diagnosis already exist...", 2); return;
            }

            int i = RDB.SET1(_appointmentID, txtSessionGoal.Text.Trim(), txtActivity.Text.Trim(), /*txtEquipments.Text.Trim(),*//* _performanceID,*/ _goalAssScaleID,
               /* txtLongTerm.Text.Trim(),*/ /*txtShortTerm.Text.Trim(),*/
               txtSuggestions.Text.Trim(), DateTime.UtcNow.AddMinutes(330), _loginID, DiagnosisIDs, DiagnosisOther);

            if (i > 0)
            {
                RDB.DeleteAttr(_appointmentID, RDB._impairementTypeID);
                foreach (ListItem li in txtImpairements.Items)
                {
                    if (li.Selected)
                    {
                        int _attrID = 0; int.TryParse(li.Value, out _attrID);
                        if (_attrID > 0)
                        {
                            RDB.SetAttr(_appointmentID, RDB._impairementTypeID, _attrID); 
                        }
                    }
                }

                Session[DbHelper.Configuration.messageTextSession] = "Daily report saved successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
    }
}
