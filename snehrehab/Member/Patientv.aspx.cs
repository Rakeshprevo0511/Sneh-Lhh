using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class Patientv : System.Web.UI.Page
    {
        int _loginID = 0; int _patientID = 0;

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
                    _patientID = SnehBLL.PatientMast_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_patientID > 0)
            {
                string tab = string.Empty; if (Request.QueryString["tab"] != null) { tab = Request.QueryString["tab"].ToString(); }
                int _patientTypeID = SnehBLL.PatientMast_Bll.PatientTypeID(_patientID);
                if (_patientTypeID == 1)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/AdultView.aspx?record=" + Request.QueryString["record"].ToString() + (!string.IsNullOrEmpty(tab) ? "&tab=" + tab : string.Empty)), true);
                }
                else if (_patientTypeID == 2)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/PediatricView.aspx?record=" + Request.QueryString["record"].ToString() + (!string.IsNullOrEmpty(tab) ? "&tab=" + tab : string.Empty)), true);
                }
                else if (_patientTypeID == 3)
                {
                    Response.Redirect(ResolveClientUrl("~/Member/EnquiryRegView.aspx?record=" + Request.QueryString["record"].ToString() + (!string.IsNullOrEmpty(tab) ? "&tab=" + tab : string.Empty)), true);
                }
                else
                {
                    Response.Redirect(ResolveClientUrl("~/Member/Patients.aspx"), true);
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/Patients.aspx"), true);
            }
        }
    }
}