using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;

public partial class Member_AppointmentCncl : System.Web.UI.Page
{
    int _loginID = 0; int _appointmentID = 0;
    int toReturn = 0; public string returnUrl = string.Empty;

    public SnehDLL.Appointments_Dll AD; public SnehDLL.PatientMast_Dll PD;

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
                _appointmentID = SnehBLL.Appointments_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (Request.QueryString["return"] != null)
        {
            int.TryParse(Request.QueryString["return"].ToString(), out toReturn);
        }
        if (toReturn == 101)
            returnUrl = "/Member/AppointmentChart.aspx";
        else
            returnUrl = "/Member/Appointments.aspx"; 
        if (_appointmentID > 0)
        {
            SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
            AD = AB.Get(_appointmentID);
            if (AD != null)
            {
                SnehBLL.PatientMast_Bll PB = new SnehBLL.PatientMast_Bll();
                PD = PB.Get(AD.PatientID);
                if (!AD.IsDeleted)
                {
                    if (AD.AppointmentStatus != 0)
                    {
                        if (toReturn == 101)
                            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                        else
                            Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                    }
                }
                else
                {
                    if (toReturn == 101)
                        Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                    else
                        Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
                }
            }
            else
            {
                if (toReturn == 101)
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
            }
        }
        else
        {
            if (toReturn == 101)
                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
            else
                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtRemark.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(this.Page, "Please enter remark.", 2); return;
        }
        SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
        int i = DB.CancelAppointment(_appointmentID, txtRemark.Text.Trim());
        if (i > 0)
        {
            //if (DbHelper.Configuration.saveToOldDb)
            //{
            //    DbHelper.OldDbExport Odb = new DbHelper.OldDbExport();
            //    int j = Odb.Export_DoctorDelete(_appointmentID);
            //}

            Session[DbHelper.Configuration.messageTextSession] = "Appointment entry cancelled successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            if (toReturn == 101)
                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
            else
                Response.Redirect(ResolveClientUrl("~/Member/Appointments.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
        }
    }

    public string TIMEDURATION(int _duration, DateTime TimeHourD)
    {
        //DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
        if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
        {
            return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
        }
        return "- - -";
    }
}