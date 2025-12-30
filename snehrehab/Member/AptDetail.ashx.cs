using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Globalization;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for AptDetail
    /// </summary>
    public class AptDetail : IHttpHandler, IRequiresSessionState
    {
        int _loginID = 0; bool isSuperAdmin = false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            rModel r = new rModel();
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                r.msg = "You need to login again into system.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            } 
            if (SnehBLL.UserAccount_Bll.getCategory() == 4)
            {
                isSuperAdmin = true;
            }
            if (SnehBLL.UserAccount_Bll.getCategory() == 3)
            {
                isSuperAdmin = true;
            }
            if (!SnehBLL.UserAccount_Bll.IsAdminOrReception())
            {
                if (!isSuperAdmin)
                {
                    r.msg = "Invalid request.";
                    context.Response.Write(JsonConvert.SerializeObject(r));
                    return;
                }
            }
            int resourceid = 0;
            if (context.Request.QueryString["resourceid"] != null)
            {
                int.TryParse(context.Request.QueryString["resourceid"].ToString(), out resourceid);
            }
            int appointmentID = 0; int table = 0;
            if (context.Request.QueryString["table"] != null)
            {
                int.TryParse(context.Request.QueryString["table"].ToString(), out table);
            }
            if (context.Request.QueryString["id"] != null)
            {
                if (DbHelper.Configuration.IsGuid(context.Request.QueryString["id"].ToString()))
                {
                    if (table <= 0)
                    {
                        appointmentID = SnehBLL.Appointments_Bll.Check(context.Request.QueryString["id"].ToString());
                    }
                    else
                    {
                        appointmentID = SnehBLL.AptWaiting_BAL.Check(context.Request.QueryString["id"].ToString());
                    }
                }
            }
            
            if (appointmentID <= 0)
            {
                r.msg = "Invalid request.";
                context.Response.Write(JsonConvert.SerializeObject(r));
                return;
            }
            if (table <= 0)
            {
                #region
                SnehBLL.Appointments_Bll AB = new SnehBLL.Appointments_Bll();
                DataTable dt = AB.AptDetail(appointmentID); int _Patient = 0;
                DataTable dtt = AB.ScheDetail(appointmentID, resourceid);
                int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out _Patient);
                if (_Patient != 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        int PatientTypeID = 0; int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out PatientTypeID);
                        DateTime AppointmentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                        int _appointmentStatusID = 0; int.TryParse(dt.Rows[0]["AppointmentStatus"].ToString(), out _appointmentStatusID);
                        string appointment_status = "<span class=\"stat_pending\">Pending</span>";
                        if (_appointmentStatusID == 1)
                        {
                            appointment_status = "<span class=\"stat_complete\">Completed</span>";
                        }
                        else if (_appointmentStatusID == 2)
                        {
                            appointment_status = "<span class=\"stat_absent\">Absent</span>";
                        }
                        else if (_appointmentStatusID == 10)
                        {
                            appointment_status = "<span class=\"stat_cancel\">Cancelled</span>";
                        }
                        StringBuilder html = new StringBuilder();
                        html.Append("<div class=\"modal-header\">" +
                                        "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\" style=\"opacity: 0.8;\"><span aria-hidden=\"true\">&times;</span></button>" +
                                        "<h5 class=\"modal-title\" style=\"margin:0px;\">Appointment Detail</h5>" +
                                    "</div>");
                        html.Append("<div class=\"modal-body\">");
                        html.Append("<table class=\"table table-nobordered\" cellspacing=\"0\">" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<b>FULL NAME :</b>" +
                                            "</td>" +
                                            "<td>" +
                                            dt.Rows[0]["FullName"].ToString() +
                                            "</td>" +
                                            "<td>" +
                                                "<b>SESSION :</b>" +
                                            "</td>" +
                                            "<td>" +
                                                  dt.Rows[0]["SessionName"].ToString() +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<b>THERAPIST :</b>" +
                                            "</td>" +
                                            "<td>" +
                                                dt.Rows[0]["Therapist"].ToString() +
                                            "</td>" +
                                            "<td>" +
                                                "<b>DATE :</b>" +
                                            "</td>" +
                                            "<td>" +
                                                (AppointmentDate > DateTime.MinValue ? AppointmentDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty) +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<b>DURATION :</b>" +
                                            "</td>" +
                                            "<td>" +
                                                dt.Rows[0]["Duration"].ToString() + " Min" +
                                            "</td>" +
                                            "<td>" +
                                                "<b>TIME :</b>" +
                                            "</td>" +
                                            "<td>" +
                                                TIMEDURATION(dt.Rows[0]["Duration"].ToString(), dt.Rows[0]["AppointmentTime"].ToString()) +
                                            "</td>" +
                                        "</tr> " +
                                        "<tr>" +
                                            "<td>" +
                                                "<b>STATUS :</b>" +
                                            "</td>" +
                                            "<td colspan=\"3\">" +
                                                appointment_status +
                                            "</td>" +
                                        "</tr> " +
                                    "</tbody>" +
                                "</table>");
                        html.Append("</div>");
                        if (_appointmentStatusID == 0)
                        {
                            html.Append("<div class=\"modal-footer\">");
                            if (!isSuperAdmin)
                            {
                                if (PatientTypeID == 3)
                                {
                                    html.Append("<a href=\"javascript:;\" onclick=\"fwdToRegistration('" + dt.Rows[0]["UniqueID"].ToString() + "', '" + dt.Rows[0]["PatUniqueID"].ToString() + "')\"  class=\"btn btn-sm btn-success\">Register</a>");
                                }
                                else
                                {
                                    html.Append("<a href=\"/Member/AppointmentPay.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-success\">Pay</a>");
                                }
                                html.Append("<a href=\"/Member/AppointmentCncl.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-warning\">Cancel</a>");
                                if (PatientTypeID != 3)
                                {
                                    html.Append("<a href=\"/Member/AppointmentAbst.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-danger\">Absent</a>");
                                }
                                html.Append("<a href=\"/Member/AppointmentEdit.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-primary\">Edit</a>");
                            }
                            html.Append("</div>");
                        }
                        r.status = true; r.msg = "Appointment Details"; r.data = html.ToString();
                        context.Response.Write(JsonConvert.SerializeObject(r));
                    }
                    else
                    {
                        r.msg = "Unable to process, Please try again.";
                        context.Response.Write(JsonConvert.SerializeObject(r));
                        return;
                    }
                    #endregion
                }
                else
                {
                    DateTime AppointmentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                    int _appointmentStatusID = 0; int.TryParse(dt.Rows[0]["AppointmentStatus"].ToString(), out _appointmentStatusID);
                    string appointment_status = "<span class=\"stat_pending\">Pending</span>";
                    if (_appointmentStatusID == 1)
                    {
                        appointment_status = "<span class=\"stat_complete\">Completed</span>";
                    }
                    else if (_appointmentStatusID == 2)
                    {
                        appointment_status = "<span class=\"stat_absent\">Absent</span>";
                    }
                    else if (_appointmentStatusID == 10)
                    {
                        appointment_status = "<span class=\"stat_cancel\">Cancelled</span>";
                    }
                    StringBuilder html = new StringBuilder();
                    html.Append("<div class=\"modal-header\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\" style=\"opacity: 0.8;\"><span aria-hidden=\"true\">&times;</span></button>" +
                                    "<h5 class=\"modal-title\" style=\"margin:0px;\">Schedule Detail</h5>" +
                                "</div>");
                    html.Append("<div class=\"modal-body\">");
                    html.Append("<table class=\"table table-nobordered\" cellspacing=\"0\">" +
                              "<tbody>" +
                                  "<tr>" +
                                      "<td>" +
                                          "<b>THERAPIST :</b>" +
                                      "</td>" +
                                      "<td>" +
                                            dtt.Rows[0]["Doctor"].ToString() +
                                      "</td>" +
                                      "<td>" +
                                          "<b>SCHEDULE TYPE :</b>" +
                                      "</td>" +
                                      "<td>" +
                                            dt.Rows[0]["ScheduleType"].ToString() +
                                      "</td>" +
                                  "</tr>" +
                                  "<tr>" +
                                      "<td>" +
                                          "<b>DATE :</b>" +
                                      "</td>" +
                                      "<td>" +
                                          (AppointmentDate > DateTime.MinValue ? AppointmentDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty) +
                                      "</td>" +
                                      "<td>" +
                                          "<b>TIME :</b>" +
                                      "</td>" +
                                      "<td>" +
                                          (dt.Rows[0]["Available1FromChar"].ToString() + " TO " + dt.Rows[0]["Available1UptoChar"].ToString()) +
                                      "</td>" +
                                  "</tr>" +
                              "</tbody>" +
                          "</table>");
                    html.Append("</div>");
                    if (_appointmentStatusID == 0)
                    {
                        html.Append("<div class=\"modal-footer\">");
                        if (!isSuperAdmin)
                        {
                            html.Append("<a href=\"/Member/MettingScheduleCncl.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-warning\">Cancel</a>");
                            html.Append("<a href=\"/Member/MettingScheduleEdit.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-primary\">Edit</a>");
                        }
                        html.Append("</div>");
                    }
                    r.status = true; r.msg = "Schedule Details"; r.data = html.ToString();
                    context.Response.Write(JsonConvert.SerializeObject(r));
                }

            }
            else
            {
                #region
                SnehBLL.AptWaiting_BAL AB = new SnehBLL.AptWaiting_BAL();
                DataTable dt = AB.AptDetail(appointmentID);
                if (dt.Rows.Count > 0)
                {
                    int PatientTypeID = 0; int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out PatientTypeID);
                    DateTime AppointmentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                    int _appointmentStatusID = 0; int.TryParse(dt.Rows[0]["AppointmentStatus"].ToString(), out _appointmentStatusID);
                    string appointment_status = "<span class=\"stat_pending\">Waiting</span>";
                    StringBuilder html = new StringBuilder();
                    html.Append("<div class=\"modal-header\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\" style=\"opacity: 0.8;\"><span aria-hidden=\"true\">&times;</span></button>" +
                                    "<h5 class=\"modal-title\" style=\"margin:0px;\">Appointment Detail</h5>" +
                                "</div>");
                    html.Append("<div class=\"modal-body\">");
                    html.Append("<table class=\"table table-nobordered\" cellspacing=\"0\">" +
                                "<tbody>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<b>FULL NAME :</b>" +
                                        "</td>" +
                                        "<td>" +
                                        dt.Rows[0]["FullName"].ToString() +
                                        "</td>" +
                                        "<td>" +
                                            "<b>SESSION :</b>" +
                                        "</td>" +
                                        "<td>" +
                                              dt.Rows[0]["SessionName"].ToString() +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<b>THERAPIST :</b>" +
                                        "</td>" +
                                        "<td>" +
                                            dt.Rows[0]["Therapist"].ToString() +
                                        "</td>" +
                                        "<td>" +
                                            "<b>DATE :</b>" +
                                        "</td>" +
                                        "<td>" +
                                            (AppointmentDate > DateTime.MinValue ? AppointmentDate.ToString(DbHelper.Configuration.showDateFormat) : string.Empty) +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td>" +
                                            "<b>DURATION :</b>" +
                                        "</td>" +
                                        "<td>" +
                                            dt.Rows[0]["Duration"].ToString() + " Min" +
                                        "</td>" +
                                        "<td>" +
                                            "<b>TIME :</b>" +
                                        "</td>" +
                                        "<td>" +
                                            TIMEDURATION(dt.Rows[0]["Duration"].ToString(), dt.Rows[0]["AppointmentTime"].ToString()) +
                                        "</td>" +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td>" +
                                            "<b>STATUS :</b>" +
                                        "</td>" +
                                        "<td colspan=\"3\">" +
                                            appointment_status +
                                        "</td>" +
                                    "</tr> " +
                                "</tbody>" +
                            "</table>");
                    html.Append("</div>");
                    if (_appointmentStatusID == 0)
                    {
                        html.Append("<div class=\"modal-footer\">");
                        if (!isSuperAdmin)
                        {
                            html.Append("<a href=\"javascript:;\" onclick=\"WaitingConfirm('" + dt.Rows[0]["UniqueID"].ToString() + "')\"  class=\"btn btn-sm btn-success\">Confirm</a>");
                            html.Append("<a href=\"javascript:;\" onclick=\"WaitingCancel('" + dt.Rows[0]["UniqueID"].ToString() + "')\"  class=\"btn btn-sm btn-danger\">Cancel</a>");
                            html.Append("<a href=\"/Member/AptWaiting.aspx?return=101&record=" + dt.Rows[0]["UniqueID"].ToString() + "\" class=\"btn btn-sm btn-primary\">Edit</a>");
                        }
                        html.Append("</div>");
                    }
                    r.status = true; r.msg = "Appointment Details"; r.data = html.ToString();
                    context.Response.Write(JsonConvert.SerializeObject(r));
                }
                else
                {
                    r.msg = "Unable to process, Please try again.";
                    context.Response.Write(JsonConvert.SerializeObject(r));
                    return;
                }
                #endregion
            }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}