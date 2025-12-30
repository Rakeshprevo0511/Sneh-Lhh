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
using System.Text;

public partial class Menus_LeftM : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DbHelper.Configuration.ClearPlaceHolder(txtMenus);
        StringBuilder html = new StringBuilder();
        int _catID = SnehBLL.UserAccount_Bll.getCategory();

        if (_catID == 1)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/Member/AppointmentChart.aspx\"><i class=\"icon-calendar\"></i><span><small>Appointments</small></span></a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\"><i class=\"icon-user\"></i><span>Patients</span></a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\"><i class=\"icon-user-md\"></i><span>Doctors</span></a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\"><i class=\"icon-money\"></i><span>Other Cash Entry</span></a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\"><i class=\"icon-money\"></i><span>Expense Entry</span></a></li>");
            html.Append("<li><a href=\"/Reports/\"><i class=\"icon-print\"></i><span>Reports</span></a></li>");
            html.Append("<li><a href=\"/Member/UserPwd.aspx\"><i class=\"icon-lock\"></i><span>Change Password</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        if (_catID == 2)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/Member/AppointmentChart.aspx\"><i class=\"icon-calendar\"></i><span><small>Appointments</small></span></a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\"><i class=\"icon-user\"></i><span>Patients</span></a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\"><i class=\"icon-user-md\"></i><span>Doctors</span></a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\"><i class=\"icon-money\"></i><span>Other Cash Entry</span></a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\"><i class=\"icon-money\"></i><span>Expense Entry</span></a></li>");
            html.Append("<li><a href=\"/Reports/\"><i class=\"icon-print\"></i><span>Reports</span></a></li>");
            html.Append("<li><a href=\"/Member/UserPwd.aspx\"><i class=\"icon-lock\"></i><span>Change Password</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        if (_catID == 3)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/Member/MyAppointments.aspx\"><i class=\"icon-calendar\"></i><span><small>Appointments</small></span></a></li>");
            html.Append("<li><a href=\"/Member/DoctorAppSheet.aspx\"><i class=\"icon-file\"></i><span><small>Appointment<br/>Sheet</small></span></a></li>");
            html.Append("<li><a href=\"/Member/DoctorDr.aspx\"><i class=\"icon-money\"></i><span>Transactions</span></a></li>");
            html.Append("<li><a href=\"/Member/UserPwd.aspx\"><i class=\"icon-lock\"></i><span>Change Password</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        if (_catID == 4)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/Member/AppointmentChart.aspx\"><i class=\"icon-calendar\"></i><span><small>Appointments</small></span></a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\"><i class=\"icon-user\"></i><span>Patients</span></a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\"><i class=\"icon-user-md\"></i><span>Doctors</span></a></li>");            
            html.Append("<li><a href=\"/Reports/\"><i class=\"icon-print\"></i><span>Reports</span></a></li>");
            html.Append("<li><a href=\"/Member/UserPwd.aspx\"><i class=\"icon-lock\"></i><span>Change Password</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        if (_catID == 5)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        if (_catID == 6)
        {
            html.Append("<li><a href=\"/Member/\"><i class=\"icon-home\"></i><span>Dashboard</span></a></li>");
            html.Append("<li><a href=\"/Member/AppointmentChart.aspx\"><i class=\"icon-calendar\"></i><span><small>Appointments</small></span></a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\"><i class=\"icon-user\"></i><span>Patients</span></a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\"><i class=\"icon-user-md\"></i><span>Doctors</span></a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\"><i class=\"icon-money\"></i><span>Other Cash Entry</span></a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\"><i class=\"icon-money\"></i><span>Expense Entry</span></a></li>");
            html.Append("<li><a href=\"/Reports/\"><i class=\"icon-print\"></i><span>Reports</span></a></li>");
            html.Append("<li><a href=\"/Member/UserPwd.aspx\"><i class=\"icon-lock\"></i><span>Change Password</span></a></li>");
            html.Append("<li><a href=\"/logout.ashx\"><i class=\"icon-signout\"></i><span>Log Out</span></a></li>");
        }
        txtMenus.Controls.Add(new LiteralControl(html.ToString()));
    }
}
