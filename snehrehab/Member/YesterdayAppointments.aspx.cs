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
using System.Text;

namespace snehrehab.Member
{
    public partial class YesterdayAppointments : System.Web.UI.Page
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
                LoadData();
                Master.dashB(false);
            }
           
        }

        protected void AppointmentGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AppointmentGV.PageIndex = e.NewPageIndex; LoadData();
        }

        private void LoadData()
        {
            DateTime _fromDate = DateTime.UtcNow.AddMinutes(330).Date;
            DateTime _uptoDate = DateTime.UtcNow.AddMinutes(330).Date;

            SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
            AppointmentGV.DataSource = DB.YesterDayAppointments(_loginID);
            AppointmentGV.DataBind();
            if (AppointmentGV.HeaderRow != null) { AppointmentGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        }

        public string record(string _appointmentStatus, int _appointmentid)
        {
            StringBuilder html = new StringBuilder();
            SnehBLL.AppointmentSession_Bll ASB = new SnehBLL.AppointmentSession_Bll();
            SnehDLL.AppointmentSession_Dll ASD = ASB.Get(_appointmentid);
            int _appointmentStatusID = 0; int.TryParse(_appointmentStatus, out _appointmentStatusID);
            if (_appointmentStatus == "")
            {
                // return ASD.AppointmentCharge.ToString();
                SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
                SnehDLL.Appointments_Dll AD = DB.Get(_appointmentid);
                if (AD != null)
                {
                    if (AD.AppointmentStatus == 0)
                    {
                        html.Append("<span class=\"label label-primary label-mini\">Pending</span>");
                    }
                    //else if (AD.AppointmentStatus == 1)
                    //{
                    //    html.Append("<span class=\"label label-success label-mini\">Completed</span>");
                    //}
                    else if (AD.AppointmentStatus == 2)
                    {
                        html.Append("<span class=\"label label-important label-mini\">Absent</span>" + ' ' + ASD.AppointmentCharge.ToString());
                    }
                    else if (AD.AppointmentStatus == 10)
                    {
                        html.Append("<span class=\"label label-warning label-mini\">Cancelled</span>");
                    }
                }
                return html.ToString();
            }
            else if (ASD != null)
            {
                return ASD.AppointmentCharge.ToString();
                //SnehBLL.Appointments_Bll DB = new SnehBLL.Appointments_Bll();
                //SnehDLL.Appointments_Dll AD = DB.Get(_appointmentid);
                //if (AD != null)
                //{
                //    if (AD.AppointmentStatus == 0)
                //    {
                //        html.Append("<span class=\"label label-primary label-mini\">Pending</span>");
                //    }
                //    else if (AD.AppointmentStatus == 1)
                //    {
                //        html.Append("<span class=\"label label-success label-mini\">Completed</span>");
                //    }
                //    else if (AD.AppointmentStatus == 2)
                //    {
                //        html.Append("<span class=\"label label-important label-mini\">Absent</span>");
                //    }
                //    else if (AD.AppointmentStatus == 10)
                //    {
                //        html.Append("<span class=\"label label-warning label-mini\">Cancelled</span>");
                //    }
                //}
            }
            return html.ToString();
        }
        public string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public string TIMEDURATION(string _durationStr, string _timeStr)
        {
            int _duration = 0; int.TryParse(_durationStr, out _duration);
            DateTime TimeHourD = new DateTime();
            DateTime.TryParseExact(_timeStr, DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
            if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
            {
                return TimeHourD.ToString(DbHelper.Configuration.showTimeFormat) + " TO " + TimeHourD.AddMinutes(_duration).ToString(DbHelper.Configuration.showTimeFormat);
            }
            return "- - -";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {        
            int i=0;
            if (txtMessage.Text.Length <=0)
            {
                DbHelper.Configuration.setAlert(msgmodal, "Please enter narration.", 2); return;
            }
            SnehDLL.SupportTicket_Dll DB = new SnehDLL.SupportTicket_Dll();
            DB.yNarration = txtMessage.Text;
            DB.UserID = _loginID;
            DB.ModifyBy = _loginID;
            DB.AddedBy = _loginID; DB.UniqueID = "";
            DB.AddedDate = DateTime.UtcNow.AddMinutes(330);
            DB.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            DB.yes_no = "1";        // On click on No button
            DB.yes_no_value = "No";
            SnehBLL.Appointments_Bll st = new SnehBLL.Appointments_Bll();
            DataTable dt = st.SaveNarrat(_loginID);

            i = st.SaveNarrat_New(DB);
            if (i > 0)
            {
                Master.dashB(true);
                Session[DbHelper.Configuration.messageTextSession] = "Narration added successfully..";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("/Member/"), true);
            }
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            int i = 0;
            SnehDLL.SupportTicket_Dll DB = new SnehDLL.SupportTicket_Dll();
            DB.UserID = _loginID;
            DB.ModifyBy = _loginID;
            DB.AddedBy = _loginID; DB.UniqueID = "";
            DB.AddedDate = DateTime.UtcNow.AddMinutes(330);
            DB.ModifyDate = DateTime.UtcNow.AddMinutes(330);
            DB.yes_no = "0";        // On click on yes button
            DB.yes_no_value = "Yes";
            SnehBLL.Appointments_Bll st = new SnehBLL.Appointments_Bll();
            DataTable dt = st.SaveNarrat(_loginID);

            i = st.SaveWithoutNarrat_New(DB);
            if (i > 0)
            {
                Master.dashB(true);
                Session[DbHelper.Configuration.messageTextSession] = "Everything is fine...";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("/Member/"), true);
            }
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            Master.dashB(true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
    }
}