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

public partial class Member_Site : System.Web.UI.MasterPage
{
    int _loginID = 0; public string _loginName = "";
    public bool isenableDB = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            if (Session[DbHelper.Configuration.messageTypeSession] != null && Session[DbHelper.Configuration.messageTextSession] != null)
            {
                int _MsgType = 0; int.TryParse(Session[DbHelper.Configuration.messageTypeSession].ToString(), out _MsgType);
                DbHelper.Configuration.setAlert(MsgPlaceHolder, Session[DbHelper.Configuration.messageTextSession].ToString(), _MsgType);
                Session[DbHelper.Configuration.messageTypeSession] = null; Session[DbHelper.Configuration.messageTextSession] = null;
            }
        }
        if (Session[DbHelper.Configuration.loginFullName] != null) { _loginName = Session[DbHelper.Configuration.loginFullName].ToString(); }



        int _catID = SnehBLL.UserAccount_Bll.getCategory(); if (_catID == 3) { mnuSpeedM.Visible = false; pnlSearch.Visible = false; }
    }

    public void dashB(bool data)
    {
        mnu_LeftM.Visible = data;
    }
}
