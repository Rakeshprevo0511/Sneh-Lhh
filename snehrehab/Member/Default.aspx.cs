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
using SnehBLL;

public partial class Member_Default : System.Web.UI.Page
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
            tb_admin.Visible = false; tb_manager.Visible = false; tb_reception.Visible = false; tb_therapist.Visible = false;tb_assManager.Visible = false;
            tb_researcher.Visible = false; create.Visible = false; optn_altert_sttng.Visible = false;            

            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            SnehBLL.UserAccount_Bll ua = new UserAccount_Bll();
            if (_catID == 1)
            {
                tb_manager.Visible = true;                
                if (_loginID == DbHelper.Configuration.managerLoginId)
                {                   
                    create.Visible = true;                   
                    optn_altert_sttng.Visible = true;
                    
                }
            }
            if (_catID == 2)
            {
                tb_reception.Visible = true;
            }
            if (_catID == 3)
            {
                tb_therapist.Visible = true;
            }
            if (_catID == 4)
            {
                tb_admin.Visible = true;
            }
            if(_catID == 5)
            {
                tb_researcher.Visible = true;
            }
            if (_catID == 6)
            {
                tb_assManager.Visible = true;
            }
            if (_catID == 2 || _catID == 3)
            {
                int reval_alert_show = 0; if (Session["reval_alert_show"] != null) { int.TryParse(Session["reval_alert_show"].ToString(), out reval_alert_show); }
                if (reval_alert_show <= 0)
                {
                    // SqlCommand cmd = new SqlCommand("REVAL_ALERT");
                    SqlCommand cmd = new SqlCommand("REVAL_ALERT_NEW");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DoctorID", SqlDbType.BigInt).Value = _loginID;
                    DbHelper.SqlDb db = new DbHelper.SqlDb();
                    DataSet ds = db.DbFetch(cmd);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtReval.Value = "1"; Session["reval_alert_show"] = 1;
                    }
                }
            }
            int pwd_alert_show = 0; if (Session["pwd_alert_show"] != null) { int.TryParse(Session["pwd_alert_show"].ToString(), out pwd_alert_show); }
            if (pwd_alert_show <= 0)
            {
                SnehBLL.Reports_Bll DB = new SnehBLL.Reports_Bll();
                DataTable dtpopup = DB.PopUpDateCalcutation(_loginID);
                if (dtpopup.Rows.Count > 0)
                {
                    DateTime ModifyDateNew = new DateTime(); DateTime.TryParseExact(dtpopup.Rows[0]["ModifyDateNew"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDateNew);
                    if (ModifyDateNew > DateTime.MinValue)
                    {
                        double days = (DateTime.UtcNow.AddMinutes(330) - ModifyDateNew).TotalDays;
                        if (days > 60)
                        {
                            txtPasswordAlert.Value = "1"; Session["pwd_alert_show"] = 1;
                        }
                    }
                }
            }
            int bday_alert_show = 0; if (Session["bday_alert_show"] != null) { int.TryParse(Session["bday_alert_show"].ToString(), out bday_alert_show); }
            if (bday_alert_show <= 0)
            {
                SqlCommand cmd = new SqlCommand("TodaysBirthDay"); cmd.CommandType = CommandType.StoredProcedure;
                DbHelper.SqlDb db = new DbHelper.SqlDb();
                DataSet ds = db.DbFetch(cmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBirthdates.Value = "1"; Session["bday_alert_show"] = 1;
                }
            }
            //int reval_alert_show = 0; if (Session["reval_alert_show"] != null) { int.TryParse(Session["reval_alert_show"].ToString(), out reval_alert_show); }
            //if (reval_alert_show <= 0)
            //{
            //    SqlCommand cmd = new SqlCommand("REVAL_ALERT"); cmd.CommandType = CommandType.StoredProcedure;
            //    DbHelper.SqlDb db = new DbHelper.SqlDb();
            //    DataSet ds = db.DbFetch(cmd);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        txtReval.Value = "1"; Session["reval_alert_show"] = 1;
            //    }
            //}
            int appointment_alert_show = 0; if (Session["appointment_alert_show"] != null) { int.TryParse(Session["appointment_alert_show"].ToString(), out appointment_alert_show); }
            if (appointment_alert_show <= 0)
            {
                SqlCommand cmd = new SqlCommand("Appointments_Alert_new"); cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DoctorID", SqlDbType.BigInt).Value = _loginID;
                DbHelper.SqlDb db = new DbHelper.SqlDb();
                DataSet ds = db.DbFetch(cmd);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtappointment.Value = "1"; Session["appointment_alert_show"] = 1;
                    }
                }
            }
        }
    }
}
