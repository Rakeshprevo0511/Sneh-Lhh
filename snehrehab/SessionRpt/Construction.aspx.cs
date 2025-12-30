using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace snehrehab.SessionRpt
{
    public partial class Construction : System.Web.UI.Page
    {
        public string _rptName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["rpt"] != null)
            {

                if (Request.QueryString["rpt"].ToString() == "si")
                {
                    _rptName = "SI";
                }
                if (Request.QueryString["rpt"].ToString() == "re-eval")
                {
                    _rptName = "Re-Eval";
                }
                if (Request.QueryString["rpt"].ToString() == "leave-report")
                {
                    _rptName = "Leave Report";
                }
                if (Request.QueryString["rpt"].ToString() == "pediatric-registration")
                {
                    _rptName = "Pediatric Registration";
                }
                if (Request.QueryString["rpt"].ToString() == "patient-report-status")
                {
                    _rptName = "Patient Report Status";
                }
                if (Request.QueryString["rpt"].ToString() == "patient-statistics")
                {
                    _rptName = "Patient Statistics";
                }
                if (Request.QueryString["rpt"].ToString() == "total-patient-list")
                {
                    _rptName = "Total Patient List";
                }
            }
        }
    }
}
