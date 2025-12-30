using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class MettingScheduleCncl : System.Web.UI.Page
    {
        int _loginID = 0; int _appointmentID = 0; public string courses = string.Empty;
        int toReturn = 0; public string returnUrl = string.Empty;
        public SnehDLL.Appointments_Dll AD; public SnehDLL.DoctorMast_Dll GK;
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
                returnUrl = "/Member/MettingSchedules.aspx";
            if (_appointmentID > 0)
            {
                SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
                AD = AB.Get(_appointmentID);
                if (AD != null)
                {
                    SnehBLL.DoctorMast_Bll PB = new SnehBLL.DoctorMast_Bll();
                   // PD = PB.get_new(AD.AppointmentID);
                    //DOCTORLIST((_appointmentID).ToString());
                   
                    foreach (SnehDLL.DoctorMast_Dll PD in PB.get_new(AD.AppointmentID))
                    {
                        courses += PD.FullName + " ,";
                    }
                    courses = courses.TrimEnd(',');
                    //(GK.FullName) = courses;
                    if (!AD.IsDeleted)
                    {
                        if (AD.AppointmentStatus != 0)
                        {
                            if (toReturn == 101)
                                Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                            else
                                Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);
                        }
                    }
                    else
                    {
                        if (toReturn == 101)
                            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                        else
                            Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);
                    }
                }
                else
                {
                    if (toReturn == 101)
                        Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                    else
                        Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);
                }
                //SnehBLL.DoctorMast_Bll PB = new SnehBLL.DoctorMast_Bll();
                //PD = PB.Get(_doctorID);
                //if(PD != null)
                //{
                    
                //        if (toReturn == 101)
                //            Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                //        else
                //            Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);                    
                //}
            }
            else
            {
                if (toReturn == 101)
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            int i = DB.CancelDrMeetSched(_appointmentID);
            if (i > 0)
            {
                //if (DbHelper.Configuration.saveToOldDb)
                //{
                //    DbHelper.OldDbExport Odb = new DbHelper.OldDbExport();
                //    int j = Odb.Export_DoctorDelete(_appointmentID);
                //}

                Session[DbHelper.Configuration.messageTextSession] = "Dr Meeting Schedule entry cancelled successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                if (toReturn == 101)
                    Response.Redirect(ResolveClientUrl("~/Member/AppointmentChart.aspx"), true);
                else
                    Response.Redirect(ResolveClientUrl("~/Member/MettingSchedules.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
        //public string DOCTORLIST(string _appointmentID)
        //{
        //    int id = Convert.ToInt32(_appointmentID);
        //    SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
        //    SnehDLL.Appointments_Dll AD = AB.Get(id);

        //    SnehBLL.DoctorMast_Bll PB = new SnehBLL.DoctorMast_Bll();
        //    string courses = string.Empty;
        //    foreach (SnehDLL.DoctorMast_Dll PD in PB.get_new(AD.AppointmentID))
        //    {
        //        courses += PD.FullName + " ,";
        //    }
        //    courses = courses.TrimEnd(',');
        //    return courses;
        //}
    }
}