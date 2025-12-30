using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;

public partial class Menus_TopM : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DbHelper.Configuration.ClearPlaceHolder(txtMenus);
        StringBuilder html = new StringBuilder();
        int _catID = SnehBLL.UserAccount_Bll.getCategory();

        if (_catID == 1)
        {
            
        }
        if (_catID == 2)
        {
           
        }
        if (_catID == 3)
        {
             
        }
        
        txtMenus.Controls.Add(new LiteralControl(html.ToString()));
    }
}
