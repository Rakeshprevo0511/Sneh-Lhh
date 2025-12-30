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

namespace snehrehab.SessionRpt
{
    public partial class DailyRpt1 : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = ""; string Demo = string.Empty;

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
            if (!RDB.IsDailyRpt_Type1(_appointmentID))
            {
                if (RDB.IsDailyRpt_Type0(_appointmentID))
                {
                    Response.Redirect(ResolveClientUrl("~/SessionRpt/DailyRpt.aspx?record=" + Request.QueryString["record"].ToString()), true); return;
                }
                else
                {
                    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true); return;
                }
            }
            tb_Contents.ActiveTabIndex = 0; tb_Report1.Enabled = true;
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
                    bool.TryParse(ds.Tables[0].Rows[0]["HasDiagnosisID"].ToString(), out HasDiagnosisID);
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
                    txtObservation.Text = ds.Tables[1].Rows[0]["Observation"].ToString();
                    txtSuggestion.Text = ds.Tables[1].Rows[0]["Suggestion"].ToString();

                    txtPrint.Value = "<a target='_blank' href='/SessionRpt/CreateRpt.ashx?record=" + Request.QueryString["record"].ToString() + "' class='btn btn-primary'>Print</a>&nbsp;";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.ReportDailyMst_Bll RDB = new SnehBLL.ReportDailyMst_Bll();
            string DiagnosisIDs = "";
            for (int k = 0; k < txtDiagnosis.Items.Count; k++)
            {
                if (txtDiagnosis.Items[k].Selected)
                {
                    DiagnosisIDs += txtDiagnosis.Items[k].Value + "|";
                }
            }
            string DiagnosisOther = txtDiagnosisOther.Text.Trim();
            SnehBLL.Diagnosis_Bll DIB = new SnehBLL.Diagnosis_Bll();
            DIB.setFromOther(DiagnosisOther);

            int i = RDB.Set(_appointmentID, txtObservation.Text.Trim(), txtSuggestion.Text.Trim(),
                DateTime.UtcNow.AddMinutes(330), _loginID, DiagnosisIDs, DiagnosisOther);
            if (i > 0)
            {
                RDB.DeleteAttr(_appointmentID, RDB._impairementTypeID);

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