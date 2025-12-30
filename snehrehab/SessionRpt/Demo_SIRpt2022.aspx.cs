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
    public partial class Demo_SIRpt2022 : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string _cancelUrl = ""; public string _printUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
        }

        





        

        public string cloneButtonLeft_sm(int index)
        {
            //if (index == 0)
            //{
            //    return "<a href=\"javascript:;\" class=\"btn btn-xs btn-default btn-success\" style=\"float:right; margin-left:20px;\" onclick= \"show_next_option(this);\"><i class=\"fa fa-plus-circle\"></i></a>";
            //}
            //else
            //{
            //    return "<div class=\"rbutton\"></div>";
            //}

            return string.Empty;
        }
        public string cloneClass(int index, string Option)
        {
            //if (index <= 1)
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(Option))
            //        return string.Empty;
            //    return "hide";
            //}

            return string.Empty;
        }
        public string cloneClass(int index, string Option, string Option1, string Option2, string Option3)
        {
            //if (index <= 1)
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(Option1) || !string.IsNullOrEmpty(Option2) || !string.IsNullOrEmpty(Option3))
            //        return string.Empty;
            //    return "hide";
            //}

            return string.Empty;
        }


    }
}