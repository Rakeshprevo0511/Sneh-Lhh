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
using System.Globalization;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class Member_BirthDay : System.Web.UI.Page
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
            if (Request.QueryString["tab"] != null)
            {
                if (Request.QueryString["tab"].ToString().Length > 0)
                {
                    if (Request.QueryString["tab"].ToString() == "patient")
                    {
                        tb_Contents.ActiveTabIndex = 0;
                    }
                    if (Request.QueryString["tab"].ToString() == "doctor")
                    {
                        tb_Contents.ActiveTabIndex = 1;
                    }
                    if (Request.QueryString["tab"].ToString() == "manager")
                    {
                        tb_Contents.ActiveTabIndex = 2;
                    }
                    if (Request.QueryString["tab"].ToString() == "reception")
                    {
                        tb_Contents.ActiveTabIndex = 3;
                    }
                }
            }
            LoadForm();
        }
    }

    private void LoadForm()
    {
        SqlCommand cmd = new SqlCommand("TodaysBirthDay"); cmd.CommandType = CommandType.StoredProcedure;
        DbHelper.SqlDb db = new DbHelper.SqlDb();
        DataSet ds = db.DbFetch(cmd);
        if (ds.Tables.Count > 0)
            PatientGV.DataSource = ds.Tables[0];
        PatientGV.DataBind();
        if (PatientGV.HeaderRow != null) { PatientGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        if (ds.Tables.Count > 1)
            DoctorGV.DataSource = ds.Tables[1];
        DoctorGV.DataBind();
        if (DoctorGV.HeaderRow != null) { DoctorGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        if (ds.Tables.Count > 2)
            ManagerGV.DataSource = ds.Tables[2];
        ManagerGV.DataBind();
        if (ManagerGV.HeaderRow != null) { ManagerGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        if (ds.Tables.Count > 3)
            ReceptionGV.DataSource = ds.Tables[3];
        ReceptionGV.DataBind();
        if (ReceptionGV.HeaderRow != null) { ReceptionGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
    }

    protected void btnPatient_Click(object sender, EventArgs e)
    {
        if (txtPatient.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }
        List<string> _mobiles = new List<string>();
        foreach (GridViewRow gr in PatientGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField h = gr.FindControl("txtMobile") as HiddenField;

            if (h != null && c != null)
            {
                if (h.Value.Length >= 10 && c.Checked)
                {
                    _mobiles.Add(h.Value);
                }
            }
        }
        if (_mobiles.Count > 0)
        {
            SnehBLL.ApiSms_Bll SB = new SnehBLL.ApiSms_Bll();
            bool _sent = SB.Send(_mobiles.ToArray(), txtPatient.Text.Trim());
            if (_sent)
            {
                #region

                foreach (GridViewRow gr in PatientGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;

                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;

                    if (h != null && c != null)
                    {
                        if (h.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MOB";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MESSAGE_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "P";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtPatient.Text.Trim();
                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion


                Session[DbHelper.Configuration.messageTextSession] = "Event messages sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=patient"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select patient name having mobile number...", 2);
        }
    }

    protected void btnDoctor_Click(object sender, EventArgs e)
    {
        if (txtDoctor.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }
        List<string> _mobiles = new List<string>();
        foreach (GridViewRow gr in DoctorGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField h = gr.FindControl("txtMobile") as HiddenField;

            if (h != null && c != null)
            {
                if (h.Value.Length >= 10 && c.Checked)
                {
                    _mobiles.Add(h.Value);
                }
            }
        }
        if (_mobiles.Count > 0)
        {
            SnehBLL.ApiSms_Bll SB = new SnehBLL.ApiSms_Bll();
            bool _sent = SB.Send(_mobiles.ToArray(), txtDoctor.Text.Trim());
            if (_sent)
            {
                #region

                foreach (GridViewRow gr in DoctorGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;

                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;

                    if (h != null && c != null)
                    {
                        if (h.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MOB";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MESSAGE_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "D";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtDoctor.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion

                Session[DbHelper.Configuration.messageTextSession] = "Event messages sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=doctor"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select doctor name having mobile number...", 2);
        }
    }

    protected void btn_Reception_Click(object sender, EventArgs e)
    {
        if (txtReception.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }
        List<string> _mobiles = new List<string>();
        foreach (GridViewRow gr in ReceptionGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField h = gr.FindControl("txtMobile") as HiddenField;

            if (h != null && c != null)
            {
                if (h.Value.Length >= 10 && c.Checked)
                {
                    _mobiles.Add(h.Value);
                }
            }
        }
        if (_mobiles.Count > 0)
        {
            SnehBLL.ApiSms_Bll SB = new SnehBLL.ApiSms_Bll();
            bool _sent = SB.Send(_mobiles.ToArray(), txtReception.Text.Trim());
            if (_sent)
            {
                #region

                foreach (GridViewRow gr in ReceptionGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;

                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;

                    if (h != null && c != null)
                    {
                        if (h.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MOB";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MESSAGE_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "R";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtReception.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion

                Session[DbHelper.Configuration.messageTextSession] = "Event messages sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=reception"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Reception name having mobile number...", 2);
        }


    }

    protected void btnManager_Click(object sender, EventArgs e)
    {
        if (txtManager.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }
        List<string> _mobiles = new List<string>();
        foreach (GridViewRow gr in ManagerGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField h = gr.FindControl("txtMobile") as HiddenField;

            if (h != null && c != null)
            {
                if (h.Value.Length >= 10 && c.Checked)
                {
                    _mobiles.Add(h.Value);
                }
            }
        }
        if (_mobiles.Count > 0)
        {
            SnehBLL.ApiSms_Bll SB = new SnehBLL.ApiSms_Bll();
            bool _sent = SB.Send(_mobiles.ToArray(), txtManager.Text.Trim());
            if (_sent)
            {
                #region

                foreach (GridViewRow gr in ManagerGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;

                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;

                    if (h != null && c != null)
                    {
                        if (h.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MOB";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MESSAGE_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "R";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtManager.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion


                Session[DbHelper.Configuration.messageTextSession] = "Event messages sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=manager"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Manager name having mobile number...", 2);
        }
    }


    protected void btnPatientEmail_Click(object sender, EventArgs e)
    {
        if (txtPatient.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }

        List<string> _emails = new List<string>();
        foreach (GridViewRow gr in PatientGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField em = gr.FindControl("txtEmail") as HiddenField;
            if (em != null && c != null)
            {
                if (em.Value.Length >= 10 && c.Checked)
                {
                    _emails.Add(em.Value);
                }
            }
        }

        if (_emails.Count > 0)
        {
            bool mail = false;
            foreach (var a in _emails)
            {

                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                mail = AM.send(a, txtPatient.Text.Trim(), "Sneh Rehab");

            }
            if (mail == true)
            {

                #region

                foreach (GridViewRow gr in PatientGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;
                    HiddenField em = gr.FindControl("txtEmail") as HiddenField;
                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;
                    if (em != null && c != null)
                    {
                        if (em.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MAIL";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            //cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MAILID", SqlDbType.VarChar, 50).Value = em.Value;
                            cmd.Parameters.Add("@EMAIL_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "P";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtPatient.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion

                Session[DbHelper.Configuration.messageTextSession] = "Event Email sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=patient"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Reception name having Email ID...", 2);
        }
    }

    protected void btn_ReceEmail_Click(object sender, EventArgs e)
    {
        if (txtReception.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }

        List<string> _emails = new List<string>();
        foreach (GridViewRow gr in ReceptionGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField em = gr.FindControl("txtEmail") as HiddenField;


            if (em != null && c != null)
            {
                if (em.Value.Length >= 10 && c.Checked)
                {
                    _emails.Add(em.Value);
                }
            }
        }

        if (_emails.Count > 0)
        {
            bool mail = false;
            foreach (var a in _emails)
            {


                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                mail = AM.send(a, txtReception.Text.Trim(), "Sneh Rehab");

            }
            if (mail == true)
            {
                #region

                foreach (GridViewRow gr in ReceptionGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;
                    HiddenField em = gr.FindControl("txtEmail") as HiddenField;
                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;
                    if (em != null && c != null)
                    {
                        if (em.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MAIL";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            //cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MAILID", SqlDbType.VarChar, 50).Value = em.Value;
                            cmd.Parameters.Add("@EMAIL_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "R";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtReception.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion

                Session[DbHelper.Configuration.messageTextSession] = "Event Email sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=reception"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Reception name having Email ID...", 2);
        }
    }

    protected void btnDoctorEmail_Click(object sender, EventArgs e)
    {
        if (txtDoctor.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }

        List<string> _emails = new List<string>();
        foreach (GridViewRow gr in DoctorGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField em = gr.FindControl("txtEmail") as HiddenField;

            if (em != null && c != null)
            {
                if (em.Value.Length >= 10 && c.Checked)
                {
                    _emails.Add(em.Value);
                }
            }
        }

        if (_emails.Count > 0)
        {
            bool mail = false;
            foreach (var a in _emails)
            {

                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                mail = AM.send(a, txtDoctor.Text.Trim(), "Sneh Rehab");

            }
            if (mail == true)
            {
                #region
                foreach (GridViewRow gr in DoctorGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;
                    HiddenField em = gr.FindControl("txtEmail") as HiddenField;
                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;
                    if (em != null && c != null)
                    {
                        if (em.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MAIL";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            //cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MAILID", SqlDbType.VarChar, 50).Value = em.Value;
                            cmd.Parameters.Add("@EMAIL_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "D";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtDoctor.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion


                Session[DbHelper.Configuration.messageTextSession] = "Event Email sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=doctor"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Reception name having Email ID...", 2);
        }
    }

    protected void btnManagerEmail_Click(object sender, EventArgs e)
    {
        if (txtManager.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter your message...", 2); return;
        }

        List<string> _emails = new List<string>();
        foreach (GridViewRow gr in ManagerGV.Rows)
        {
            CheckBox c = gr.FindControl("txtSelect") as CheckBox;
            HiddenField em = gr.FindControl("txtEmail") as HiddenField;

            if (em != null && c != null)
            {
                if (em.Value.Length >= 10 && c.Checked)
                {
                    _emails.Add(em.Value);
                }
            }
        }

        if (_emails.Count > 0)
        {
            bool mail = false;
            foreach (var a in _emails)
            {

                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                mail = AM.send(a, txtManager.Text.Trim(), "Sneh Rehab");

            }
            if (mail == true)
            {

                #region
                foreach (GridViewRow gr in ManagerGV.Rows)
                {
                    CheckBox c = gr.FindControl("txtSelect") as CheckBox;
                    HiddenField h = gr.FindControl("txtMobile") as HiddenField;
                    HiddenField em = gr.FindControl("txtEmail") as HiddenField;
                    HiddenField fullname = gr.FindControl("txtFullName") as HiddenField;
                    HiddenField birthdate = gr.FindControl("txtBirthDate") as HiddenField;
                    HiddenField address = gr.FindControl("txtrAddress") as HiddenField;
                    HiddenField trgistration = gr.FindControl("txtRegistrationDate") as HiddenField;
                    HiddenField ID = gr.FindControl("txtID") as HiddenField;
                    if (em != null && c != null)
                    {
                        if (em.Value.Length >= 10 && c.Checked)
                        {
                            DbHelper.SqlDb dbs = new DbHelper.SqlDb();
                            SqlCommand cmd = new SqlCommand("Birthday_Report"); cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@MODE", SqlDbType.VarChar, 50).Value = "MAIL";
                            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 100).Value = fullname.Value;
                            cmd.Parameters.Add("@PATIENTID", SqlDbType.VarChar, 50).Value = ID.Value;
                            cmd.Parameters.Add("@BIRTHDATE", SqlDbType.VarChar, 50).Value = birthdate.Value;
                            //cmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar, 50).Value = h.Value;
                            cmd.Parameters.Add("@MAILID", SqlDbType.VarChar, 50).Value = em.Value;
                            cmd.Parameters.Add("@EMAIL_STATUS", SqlDbType.VarChar, 50).Value = "YES";
                            cmd.Parameters.Add("@utype", SqlDbType.VarChar, 50).Value = "M";
                            cmd.Parameters.Add("@UMessage", SqlDbType.VarChar, 300).Value = txtManager.Text.Trim();

                            int i = dbs.DbUpdate(cmd);
                        }
                    }
                }
                #endregion

                Session[DbHelper.Configuration.messageTextSession] = "Event Email sent successfully...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/BirthDay.aspx?tab=manager"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, Please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Please select Reception name having Email ID...", 2);
        }
    }
}
