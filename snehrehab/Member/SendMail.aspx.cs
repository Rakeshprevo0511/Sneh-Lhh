using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class SendMail : System.Web.UI.Page
    {
        int _loginID = 0; int PatientID = 0; string id = string.Empty; string type = string.Empty; int _appointmentID = 0;
        string UniqueID = string.Empty; int ReceiptType = 0; string FiscalDate = string.Empty; long ReceiptNo = 0; string rn = string.Empty;
        int receivertype = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            
            if (Request.QueryString["dtype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["dtype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidDailType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["ndttype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["ndttype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidNdtType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["ndttype_new"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["ndttype_new"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidNdt_newType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["bottype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["bottype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidBotType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["type"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["type"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidSiType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["revtype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["revtype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidRevType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["eiptype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["eiptype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidEipType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["pretype"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["pretype"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                HidPreType.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }
            if (Request.QueryString["id"] != null && Request.QueryString["pid"] != null)
            {
                int.TryParse(Request.QueryString["pid"].ToString(), out PatientID);
                hidValue.Value = PatientID.ToString();

                rModel r = new rModel();
                id = Request.QueryString["id"].ToString();
                dynamic data = JsonConvert.DeserializeObject((r.Decode(HttpUtility.UrlDecode(Request.QueryString["id"].ToString()))));
                if (DbHelper.Configuration.IsGuid((string)data.un))
                {
                    UniqueID = (string)data.un;
                }
                int.TryParse((string)data.rt, out ReceiptType);
                FiscalDate = (string)data.fy;
                rn = (string)data.rn;
                long ReceiptNo = 0; long.TryParse(rn, out ReceiptNo);
                Hiduniqueid.Value = UniqueID.ToString();
                hidrectype.Value = ReceiptType.ToString();
                hidreceiptno.Value = ReceiptNo.ToString();
                Hidfiscaldate.Value = FiscalDate.ToString();
            }

            if (Request.QueryString["typesi2022"] != null && Request.QueryString["record"] != null)
            {
                type = Request.QueryString["typesi2022"].ToString();
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
                    PatientID = SnehBLL.Appointments_Bll.GetPatientID(_appointmentID);
                }
                Hidsirpt2022.Value = type.ToString();
                HidAppointID.Value = _appointmentID.ToString();
                hidValue.Value = PatientID.ToString();
            }

            if (!IsPostBack)
            {
                if (PatientID > 0)
                {
                    SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
                    DataTable dt = PB.GetMailDropdown(PatientID, 1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        txtpatientmail.Text = dt.Rows[i]["MailID"].ToString();
                    }

                    DataTable dt1 = PB.GetMailDropdown(PatientID, 2);
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        txtfathermail.Text = dt1.Rows[i]["MailID"].ToString();
                    }

                    DataTable dt2 = PB.GetMailDropdown(PatientID, 3);
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        txtmothermail.Text = dt2.Rows[i]["MailID"].ToString();
                    }

                    DataTable dt3 = PB.GetMailDropdown(PatientID, 4);
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        txtrefmail.Text = dt3.Rows[i]["MailID"].ToString();
                    }
                }

            }
        }



        protected void rdpat_CheckedChanged(object sender, EventArgs e)
        {
            if (rdpat.Checked == true)
            {
                mailsend.Visible = true;
                int val = 1;
                HidReciver.Value = val.ToString();
                txtmail.Text = txtpatientmail.Text;

            }
            else
            {
                mailsend.Visible = false;
                txtmail.Text = "";
            }
        }

        protected void rdfat_CheckedChanged(object sender, EventArgs e)
        {
            if (rdfat.Checked == true)
            {
                mailsend.Visible = true;
                int val = 2;
                HidReciver.Value = val.ToString();
                txtmail.Text = txtfathermail.Text;
            }
            else
            {
                mailsend.Visible = false;
                txtmail.Text = "";
            }
        }

        protected void rdmot_CheckedChanged(object sender, EventArgs e)
        {
            if (rdmot.Checked == true)
            {
                mailsend.Visible = true;
                int val = 3;
                HidReciver.Value = val.ToString();
                txtmail.Text = txtmothermail.Text;
            }
            else
            {
                mailsend.Visible = false;
                txtmail.Text = "";
            }
        }

        //protected void rdref_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdref.Checked == true)
        //    {
        //        mailsend.Visible = true;
        //        int val = 4;
        //        HidReciver.Value = val.ToString() ;
        //        txtmail.Text = txtrefmail.Text;
        //    }
        //    else
        //    {
        //        mailsend.Visible = false;
        //        txtmail.Text = "";
        //    }
        //}

        protected void rdref_CheckedChanged(object sender, EventArgs e)
        {
            if (rdref.Checked == true)
            {
                receivertype = 4;
                Hidreceivertype.Value = receivertype.ToString();
                mailsend.Visible = true;
                txtmail.Text = txtrefmail.Text;
            }
            else
            {
                mailsend.Visible = false;
                txtmail.Text = "";
            }
        }

        protected void send_Click(object sender, EventArgs e)
        {
            string mailid = string.Empty;
            mailid = txtmail.Text.Trim();
            if (mailid.Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select mail id...", 2); return;
            }
            if (!string.IsNullOrEmpty(mailid))
            {
                Response.Redirect(ResolveClientUrl("/Member/MailReceipt_PDF.ashx?mailid=" + mailid + "&id=" + id + "&pid=" + PatientID), true);
            }
        }

    }
}