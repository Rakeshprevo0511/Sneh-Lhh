using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Reports
{
    public partial class ReferrenceList : System.Web.UI.Page
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
                txtFrom.Text = new DateTime(DateTime.UtcNow.AddMinutes(330).Year, DateTime.UtcNow.AddMinutes(330).Month,
               1// DateTime.UtcNow.AddMinutes(330).Day
                ).ToString(DbHelper.Configuration.showDateFormat);
                txtUpto.Text = DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat);
                LoadData();
            }
        }

        protected void ReferenceGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReferenceGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            int ReferID = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out ReferID); }
            int Ref_Selected = 0; if (txtReferSelected.SelectedItem != null) { int.TryParse(txtReferSelected.SelectedItem.Value, out Ref_Selected); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll();
            ReferenceGV.DataSource = RB.Search(ReferID, Ref_Selected, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            ReferenceGV.DataBind();
            if (ReferenceGV.HeaderRow != null) { ReferenceGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        private void LoadReferedBY()
        {
            txtReferSelected.Items.Clear(); txtReferSelected.Items.Add(new ListItem("Select Name", "-1"));
            int ReferID = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out ReferID); }
            SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll();
            if (ReferID > 0)
            {
                if (ReferID == 1)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_DrList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Prefix + " " + RD.Name, RD.DoctorID.ToString()));
                    }

                }
                else if (ReferID == 2)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_SchoolList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Name, RD.SchoolID.ToString()));
                    }
                }
                else if (ReferID == 3)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_HospitalList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Name, RD.HospitalID.ToString()));
                    }
                }
                else if (ReferID == 4)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_TeacherList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Name, RD.TeacherID.ToString()));
                    }
                }
                else if (ReferID == 5)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_OtherList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Name, RD.OtherID.ToString()));
                    }
                }
                else if (ReferID == 6)
                {
                    foreach (SnehDLL.Reference_Dll RD in RB.Get_Reference_OnlineList())
                    {
                        txtReferSelected.Items.Add(new ListItem(RD.Name, RD.OnlineID.ToString()));
                    }
                }
            }

        }

        protected void txtreferred_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReferedBY();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReferenceGV.PageIndex = 0; LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int ReferID = 0; if (txtReferred.SelectedItem != null) { int.TryParse(txtReferred.SelectedItem.Value, out ReferID); }
            int Ref_Selected = 0; if (txtReferSelected.SelectedItem != null) { int.TryParse(txtReferSelected.SelectedItem.Value, out Ref_Selected); }
            DateTime _fromDate = new DateTime(); DateTime.TryParseExact(txtFrom.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(txtUpto.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll();

            DataTable dt = RB.Search(ReferID, Ref_Selected, txtSearch.Text.Trim(), _fromDate, _uptoDate);
            if (dt.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><td><b>Report Name:</b></td><td>Appointment List Report</td></tr>");
                html.Append("<tr><td><b>Report Date:</b></td><td>" + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + " &nbsp;&nbsp;&nbsp; to &nbsp;&nbsp;&nbsp; " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat) + "</td></tr>");
                html.Append("</table>");
                html.Append("<br/>");
                html.Append("<table border=\"1\" style=\"font-family: Verdana;font-size: 11px;\">");
                html.Append("<tr><th>SR NO</th><th>FULL NAME</th><th>CONTACT NO</th><th>ADDRESS</th><th>EMAIL-ID</th><th>WEBSITE</th><th>REFERENCE NAME</th></tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td style=\"vertical-align:top;\">" + (i + 1).ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["FullName"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["MobileNo"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Address"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["EmailID"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Website"].ToString() + "</td>");
                    html.Append("<td style=\"vertical-align:top;\">" + dt.Rows[i]["Name"].ToString() + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=reference list report - " + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + ".xls");
                Response.ContentType = "application/vnd.xls";
                Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
                Response.Charset = "";
                Response.Output.Write(html.ToString());
                Response.End();
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "No records found to export...", 3);
            }
        }

        public string GetAction(string referredid, string refselected, string name, string mobile, string email, string website, string address, string date, string UniqueID)
        {
            int ReferredID = 0; int.TryParse(referredid, out ReferredID);
            int ReferSelected = 0; int.TryParse(refselected, out ReferSelected); string dateadded = string.Empty;
            dateadded = FORMATDATE(date);
            //return "<a href=\"javascript:;\" onclick=\"LoadDetail(this, '" + ReferredID + "','" + ReferSelected + "');\">Edit</a>";

            //return "<a href=\"Patientv.aspx?record=" + UniqueID + "\">Edit</a>";
            //return "<a href=\"Patiente.aspx?record=" + UniqueID + "\">Edit</a> &nbsp;" +
            //"<a href=\"Patientd.aspx?record=" + UniqueID + "\">Delete</a>";

            return "<a href=\"javascript:;\" onclick=\"LoadDetail(this, '" + ReferredID + "','" + ReferSelected + "','" + name + "','" + mobile + "','" + email + "','" + website + "','" + address + "','" + dateadded + "');\">Edit Reference /</a>&nbsp;" +
                "<a href=\"/Member/Patiente.aspx?record=" + UniqueID + "&referid=" + ReferredID + "\">Edit Patient /</a>&nbsp;" +
                "<a href=\"/Member/Patientd.aspx?record=" + UniqueID + "&referid=" + ReferredID + "\">Delete</a>";
        }

        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        protected void btnSaveRefer_Click(object sender, EventArgs e)
        {
            string name = string.Empty; string contact = string.Empty; string email = string.Empty; string msg = string.Empty;
            string website = string.Empty; string address = string.Empty; string referedbymsg = string.Empty; int referid = 0; int referselid = 0;
            name = txtmodname.Text;
            contact = txtmodcontact.Text;
            email = txtmodemailid.Text;
            website = txtmodwebsite.Text;
            address = txtmodaddress.Text;
            DateTime addeddate = new DateTime(); DateTime.TryParseExact(txtmodAddedDate.Text, DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out addeddate);
            if (txtmodname.Text.Length <= 0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter name.", 2); return;
            }
            if (txtmodcontact.Text.Length <= 0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter contact no.", 2); return;
            }
            if (txtmodemailid.Text.Length <= 0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter emailid.", 2); return;
            }
            if (txtmodwebsite.Text.Length <= 0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter website name.", 2); return;
            }
            if (txtmodaddress.Text.Length <= 0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter address.", 2); return;
            }
            if (addeddate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please select date.", 2); return;
            }
            int.TryParse(txtReferedByID.Value, out referid);
            int.TryParse(txtReferSelectedID.Value, out referselid);
            SnehDLL.Reference_Dll RD = new SnehDLL.Reference_Dll();
            SnehBLL.Reference_Bll RB = new SnehBLL.Reference_Bll(); int i = 0; string refername = string.Empty;
            if (referid == 1)
            {
                refername = "Doctor Reference";
                RD.DoctorID = referselid;
                RD.Prefix = "DR";
                RD.Name = name.Substring(3);
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceDr(RD);
            }
            else if (referid == 2)
            {
                refername = "School Reference";
                RD.SchoolID = referselid;
                RD.Name = name;
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceSchool(RD);
            }
            else if (referid == 3)
            {
                refername = "Hospital Reference";
                RD.HospitalID = referselid;
                RD.Name = name;
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceHospital(RD);
            }
            else if (referid == 4)
            {
                refername = "Teacher Reference";
                RD.TeacherID = referselid;
                RD.Name = name;
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceTeacher(RD);
            }
            else if (referid == 5)
            {
                refername = "Other Reference";
                RD.OtherID = referselid;
                RD.Name = name;
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceOther(RD);
            }
            else if (referid == 6)
            {
                refername = "Online Reference";
                RD.OnlineID = referselid;
                RD.Name = name;
                RD.MobileNo = contact;
                RD.EmailID = email;
                RD.Website = website;
                RD.Address = address;
                RD.AddedDate = addeddate;
                i = RB.Set_RefernceOnlie(RD);
            }
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = refername + "added successfully..";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("/Reports/ReferrenceList.aspx"), true);
            }
        }
    }
}