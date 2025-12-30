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
    public partial class Banks : System.Web.UI.Page
    {
        int _loginID = 0; int _bankID = 0;

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
                    _bankID = SnehBLL.Banks_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (!IsPostBack)
            {
                if (_bankID > 0)
                {
                    lblBankName.InnerText = "Edit Bank Name";
                    btnAddNew.Text = "Update"; btnCancel.Visible = true;
                    SnehBLL.Banks_Bll SB = new SnehBLL.Banks_Bll();
                    SnehDLL.Banks_Dll SD = SB.Get(_bankID);
                    if (SD != null)
                    {
                        txtBankName.Text = SD.BankName;
                    }
                    else
                    {
                        Session[DbHelper.Configuration.messageTextSession] = "Bank details not found, please try again.";
                        Session[DbHelper.Configuration.messageTypeSession] = "3";

                        Response.Redirect(ResolveClientUrl("~/Member/Banks.aspx"), true);
                    }
                }
                LoadData();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (txtBankName.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter bank name...", 2); return;
            }
            SnehDLL.Banks_Dll SD = new SnehDLL.Banks_Dll();
            SD.BankID = _bankID; SD.UniqueID = ""; SD.BankName = txtBankName.Text.Trim();
            SD.AddedDate = DateTime.UtcNow.AddMinutes(330); SD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            SD.AddedBy = _loginID; SD.ModifyBy = _loginID;
            SnehBLL.Banks_Bll SB = new SnehBLL.Banks_Bll();
            int i = SB.Set(SD);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Bank name saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/Banks.aspx"), true);
            }
            else if (i == -1)
            {
                DbHelper.Configuration.setAlert(Page, "Bank name is already available, please try another...", 3);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }

        protected void BanksGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BanksGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            SnehBLL.Banks_Bll DB = new SnehBLL.Banks_Bll();
            BanksGV.DataSource = DB.GetList();
            BanksGV.DataBind();
            if (BanksGV.HeaderRow != null) { BanksGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
            add_bank.Visible = false;
            BanksGV.Columns[2].Visible = false;
            if (_loginID == DbHelper.Configuration.managerLoginId)
            {
                add_bank.Visible = true;
                BanksGV.Columns[2].Visible = true;
            }            
        }
    }
}
