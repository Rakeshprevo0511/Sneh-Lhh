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
using System.Globalization;

namespace snehrehab.Member
{
    public partial class Feedbacks : System.Web.UI.Page
    {
        int _loginID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FeedbackGV.PageIndex = 0; LoadData();
        }

        protected void FeedbackGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FeedbackGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            SnehBLL.FeedBackMst_Bll DB = new SnehBLL.FeedBackMst_Bll();
            FeedbackGV.DataSource = DB.GetMyList(_loginID, txtSearch.Text.Trim());
            FeedbackGV.DataBind();
            if (FeedbackGV.HeaderRow != null) { FeedbackGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        public string FEEDBACKTO(string _id, string _val, string _typeVal)
        {
            int id = 0; int.TryParse(_id, out id);
            if (id <= 0) { return _val; }
            return _typeVal;
        }

        public string ISSUETYPE(string _id, string _val, string _typeVal)
        {
            int id = 0; int.TryParse(_id, out id);
            if (id <= 0) { return _val; }
            return _typeVal;
        }
    }
}
