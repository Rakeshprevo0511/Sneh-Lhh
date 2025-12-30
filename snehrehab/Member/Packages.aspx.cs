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

namespace snehrehab.Member
{
    public partial class Packages : System.Web.UI.Page
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
                txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("All Session", "-1"));
                SnehBLL.SessionMast_Bll SB = new SnehBLL.SessionMast_Bll();
                foreach (SnehDLL.SessionMast_Dll SD in SB.GetList().Where(s => s.IsEvaluation == true || s.IsPackage == true).ToList())
                {
                    txtSession.Items.Add(new ListItem(SD.SessionName, SD.SessionID.ToString()));
                }
                LoadData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PackageGV.PageIndex = 0; LoadData();
        }

        protected void PackageGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PackageGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
            SnehBLL.Packages_Bll DB = new SnehBLL.Packages_Bll();
            PackageGV.DataSource = DB.Search(txtSearch.Text.Trim(), _sessionID);
            PackageGV.DataBind();
            if (PackageGV.HeaderRow != null) { PackageGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            btn_add_package.Visible = false;
            PackageGV.Columns[9].Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId) 
            {
                btn_add_package.Visible = true;
                PackageGV.Columns[9].Visible = true; 
            }
        }

        public string GetToggleText(string a)
        {
            bool IsEnabled = true; bool.TryParse(a, out IsEnabled);
            if (IsEnabled)
                return "Disable";
            return "Enable";
        }

        protected void btnToggle_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton)sender;
            int PackageID = 0; if (lk != null) { int.TryParse(lk.CommandArgument, out PackageID); }
            if (PackageID > 0)
            {
                SnehBLL.Packages_Bll DB = new SnehBLL.Packages_Bll();
                int i = DB.Toggle(PackageID);
                if (i > 0)
                {
                    LoadData();
                    DbHelper.Configuration.setAlert(Page, "Package status changed successfully.", 1);
                }
                else
                {
                    DbHelper.Configuration.setAlert(Page, "Unable to process, Please try again.", 2);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process, Please try again.", 2);
            }
        }
    }
}
