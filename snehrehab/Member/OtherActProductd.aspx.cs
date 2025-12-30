using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class OtherActProductd : System.Web.UI.Page
    {
        int _loginID = 0; int _productID = 0;

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
                    _productID = SnehBLL.OtherActProduct_BLL.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_productID > 0)
            {
                if (!IsPostBack)
                {

                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/OtherActProducts.aspx"), true);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.OtherActProduct_BLL OPB = new SnehBLL.OtherActProduct_BLL();
            int i = OPB.Delete(_productID);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Product entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/OtherActProducts.aspx"), true);
            }
            else if (i == -10)
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to delete, Product entry is in use.", 3);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}