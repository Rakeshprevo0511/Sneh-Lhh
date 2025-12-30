using System;
using System.Collections.Generic;
using static snehrehab.rModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.SessionRpt
{
    public partial class Demo_PrConsultRpt : System.Web.UI.Page
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
            //if (Request.QueryString["record"] != null)
            //{
            //    if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
            //    {
            //        _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
            //    }
            //}
            //_cancelUrl = "/Reports/PreConsultation.aspx";
            //if (_appointmentID > 0)
            //{
            //    if (!IsPostBack)
            //    {
            //        LoadForm();
            //    }
            //}
            //else
            //{
            //    Response.Redirect(ResolveClientUrl("~" + _cancelUrl), true);
            //}
            //_printUrl = txtPrint.Value;

            if (!IsPostBack)
            {
                LoadForm();
            }
        }

        private void LoadForm()
        {
            List<optionMdel> ql = new List<optionMdel>();
            for (int i = 1; i <= OptionCount; i++) { ql.Add(new optionMdel() { }); }
            txtSignleChoice.DataSource = ql;
            txtSignleChoice.DataBind();


            List<optionMdel> qls = new List<optionMdel>();
            int temp = qls.Count; txtVisibleOption.Value = qls.Count.ToString();
            for (int jl = 0; jl < (OptionCount - temp); jl++)
            {
                qls.Add(new optionMdel() { Option = string.Empty });
            }
            txtSignleChoice.DataSource = qls;
            txtSignleChoice.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

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
    }
}