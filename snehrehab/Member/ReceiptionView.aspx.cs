using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class ReceiptionView : System.Web.UI.Page
    {
        int _loginID = 0; int receiptionid = 0; public string _headerText = "";

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
                    receiptionid = SnehBLL.Receiption_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (receiptionid > 0) { _headerText = "Receiption Detail"; }
            if (!IsPostBack)
            {
                if (receiptionid > 0)
                {
                    LoadData();
                }
                else
                {
                    Image1.ImageUrl = "../images/dh-users.png";
                }

            }
        }

        private void LoadData()
        {
            SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
            SnehDLL.Receiption_Dll RD = RB.Get(receiptionid);
            txtfullname.Text = RD.FullName;
            txtemailid.Text = RD.MailID;
            txtcontactno.Text = RD.ContactNo;
            txtemergencycontact.Text = RD.Emergency_ContactNO;
            if (RD.BirthDate > DateTime.MinValue)
            {
                txtdateofbirth.Text = RD.BirthDate.ToString(DbHelper.Configuration.showDateFormat);
            }
            if (RD.Anniversary_Date > DateTime.MinValue)
            {
                txtanniversarydate.Text = RD.Anniversary_Date.ToString(DbHelper.Configuration.showDateFormat);
            }
            txtdesignation.Text = RD.Designation;
            txtqualifications.Text = RD.Qualifications;
            txtrefdocument.Text = RD.Reference_Documents;
            if (RD.JoinDate > DateTime.MinValue)
            {
                txtdateofjoining.Text = RD.JoinDate.ToString(DbHelper.Configuration.showDateFormat);
            }
            txtpancardno.Text = RD.PancardNo;
            txtaddress.Text = RD.Address;
            txtadharcardno.Text = RD.AadharcardNo;
            if (RD.Profile_Image_Path.Length > 0)
            {
                Image1.ImageUrl = "/Files/" + RD.Profile_Image_Path;
            }
            else
            {
                Image1.ImageUrl = "../images/dh-users.png";
            }
        }

        protected void btnBack_Click(Object sender, EventArgs e)
        {
            Response.Redirect(ResolveClientUrl("/Member/ViewList.aspx"), true);
        }
    }
}