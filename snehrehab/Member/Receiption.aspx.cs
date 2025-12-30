using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class Receiption : System.Web.UI.Page
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
            if (receiptionid > 0) { _headerText = "Edit Receiption Detail"; } else { _headerText = "Add Receiption Detail"; }
            if (!IsPostBack)
            {
                if (receiptionid > 0)
                {
                    LoadData();
                }
                else
                {
                    upload.Visible = true;
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
            if (RD.Clinic_Shift_TimeFromChar.Length > 0)
            {
                txtclinicshifttimefrom.Text = new DateTime(RD.Clinic_Shift_TimeFrom.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            if (RD.Clinic_Shift_TimeUptoChar.Length > 0)
            {
                txtclinicshifttimeUpto.Text = new DateTime(RD.Clinic_Shift_TimeUpto.Ticks).ToString(DbHelper.Configuration.showTimeFormat);
            }
            txtbloodgroup.Text = RD.BloodGroup;
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
            SnehBLL.Degree_Bll B = new SnehBLL.Degree_Bll();
            DataTable dt = B.Get_CerData(1, receiptionid, 0, 0);
            if (dt.Rows.Count > 0)
            {
                upload.Visible = false;
            }
            else
            {
                upload.Visible = true;
            }
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            if (txtfullname.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter full name of account holder...", 2); return;
            }
            if (txtemailid.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter mail address...", 2); return;
            }
            if (!DbHelper.Configuration.isValidEmail(txtemailid.Text.Trim()))
            {
                DbHelper.Configuration.setAlert(Page, "Please enter correct mail address...", 2); return;
            }
            if (txtcontactno.Text.Trim().Length <= 0 || txtcontactno.Text.Trim().Length > 10)
            {
                DbHelper.Configuration.setAlert(Page, "Mobile no should not b greater than 10 digits or less than 10 digits.", 2); return;
            }
            if (txtemergencycontact.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter emergency contact no address...", 2); return;
            }
            if (txtdateofbirth.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select date of birth...", 2); return;
            }
            if (txtdesignation.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter designation...", 2); return;
            }
            if (txtqualifications.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter your qualifications...", 2); return;
            }
            if (txtdateofjoining.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select date of birth...", 2); return;
            }
            if (txtpancardno.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter your pancardno...", 2); return;
            }
            if (txtaddress.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter your pancardno...", 2); return;
            }
            if (txtadharcardno.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please enter your pancardno...", 2); return;
            }
            //if (Image1.ImageUrl == null)
            //{
            //    if (txtprofilephoto.HasFile == false)
            //    {
            //        DbHelper.Configuration.setAlert(Page, "Please select image to upload...", 2); return;
            //    }
            //}
            //if (upload.Visible == true)
            //{
            //    if (txtdegcer.HasFile == false)
            //    {
            //        DbHelper.Configuration.setAlert(Page, "Please select certificate/degree to upload...", 2); return ;
            //    }
            //}
            SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
            SnehDLL.Receiption_Dll RD = new SnehDLL.Receiption_Dll();
            RD.ReceiptionID = receiptionid;
            RD.FullName = DbHelper.StringHelper.ToTitleCase(txtfullname.Text.Trim(), DbHelper.TitleCase.All);
            RD.Designation = txtdesignation.Text.Trim();
            RD.Qualifications = txtqualifications.Text.Trim();
            RD.MailID = txtemailid.Text.Trim();
            RD.ContactNo = txtcontactno.Text.Trim();
            RD.Emergency_ContactNO = txtemergencycontact.Text.Trim();
            DateTime _birthdate = new DateTime(); DateTime.TryParseExact(txtdateofbirth.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _birthdate);
            RD.BirthDate = _birthdate;
            DateTime _anniversarydate = new DateTime(); DateTime.TryParseExact(txtanniversarydate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _anniversarydate);
            RD.Anniversary_Date = _anniversarydate;
            RD.Reference_Documents = txtrefdocument.Text.Trim();
            DateTime _joindate = new DateTime(); DateTime.TryParseExact(txtdateofjoining.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _joindate);
            RD.JoinDate = _joindate;
            RD.PancardNo = txtpancardno.Text.Trim();
            RD.AadharcardNo = txtadharcardno.Text.Trim();
            RD.Address = txtaddress.Text.Trim();
            TimeSpan ClinicShiftTimeFrom = new TimeSpan();
            DateTime ClinicShiftTimeFromD = new DateTime(); DateTime.TryParseExact(txtclinicshifttimefrom.Text.Trim(), DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeFromD);
            if (ClinicShiftTimeFromD > DateTime.MinValue && ClinicShiftTimeFromD < DateTime.MaxValue)
            {
                ClinicShiftTimeFrom = ClinicShiftTimeFromD.TimeOfDay;
            }
            TimeSpan ClinicShiftTimeUpto = new TimeSpan();
            DateTime ClinicShiftTimeUptoD = new DateTime(); DateTime.TryParseExact(txtclinicshifttimeUpto.Text.Trim(), DbHelper.Configuration.showTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeUptoD);
            if (ClinicShiftTimeUptoD > DateTime.MinValue && ClinicShiftTimeUptoD < DateTime.MaxValue)
            {
                ClinicShiftTimeUpto = ClinicShiftTimeUptoD.TimeOfDay;
            }
            RD.Clinic_Shift_TimeFrom = ClinicShiftTimeFrom;
            RD.Clinic_Shift_TimeUpto = ClinicShiftTimeUpto;
            RD.Clinic_Shift_TimeFromChar = txtclinicshifttimefrom.Text.Trim();
            RD.Clinic_Shift_TimeUptoChar = txtclinicshifttimeUpto.Text.Trim();
            RD.BloodGroup = txtbloodgroup.Text.Trim();
            RD.Profile_Image_Path = "";
            if (txtprofilephoto.FileName.Length > 0)
            {
                decimal size = Math.Round(((decimal)txtprofilephoto.PostedFile.ContentLength / (decimal)1024), 2);
                if (size > 150)
                {
                    DbHelper.Configuration.setAlert(Page, "File size must not exceed 150 KB.", 2); return;
                }
                string _fileName = "p_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + txtprofilephoto.FileName.Substring(txtprofilephoto.FileName.LastIndexOf('.'));

                if (RB.SaveFile(ref txtprofilephoto, _fileName))
                {
                    RD.Profile_Image_Path = _fileName;
                }
            }
            int i = RB.set(RD);

            if (i > 0)
            {
                if (upload.Visible == true)
                {
                    if (txtdegcer.HasFile == true)
                    {
                        SnehBLL.Degree_Bll DB = new SnehBLL.Degree_Bll();
                        SnehDLL.Degree_Dll DD = new SnehDLL.Degree_Dll();
                        int z = 0; int j = 0;
                        foreach (string fileName in Request.Files)
                        {
                            string FilePathQuestion = string.Empty; string fileNameQue = string.Empty;
                            HttpPostedFile file = Request.Files[z];
                            if (fileName == "ctl00$ContentPlaceHolder1$txtdegcer")
                            {
                                string FilePathCer = string.Empty; string FileNameCer = string.Empty;
                                FileNameCer = System.IO.Path.GetFileName(file.FileName);
                                FilePathCer = "d_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + FileNameCer.Substring(FileNameCer.LastIndexOf('.'));
                                if (RB.SaveFileNew(ref file, FilePathCer))
                                {
                                    DD.ReceiptionID = i;
                                    DD.ManagerID = 0;
                                    DD.DoctorID = 0;
                                    DD.Image_Path = FilePathCer;
                                    DD.Filename = FileNameCer;
                                    j = DB.set(DD);
                                }
                            }
                            z++;
                        }
                    }
                }

                if (receiptionid > 0)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Receiption data updated successfully...";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("/Member/ViewList.aspx"), true);
                }
                else
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Receiption data added successfully...";
                    Session[DbHelper.Configuration.messageTypeSession] = "1";
                    Response.Redirect(ResolveClientUrl("/Member/Receiption.aspx"), true);
                }
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process...", 2); return;
            }
        }
    }
}