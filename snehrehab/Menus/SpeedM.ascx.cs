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

public partial class Menus_SpeedM : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        DbHelper.Configuration.ClearPlaceHolder(txtMenus);
        StringBuilder html = new StringBuilder();
        int _catID = SnehBLL.UserAccount_Bll.getCategory();

        if (_catID == 1)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\">View Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Adult.aspx\">Adult Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Pediatric.aspx\">Pediatric Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Appointment.aspx\">New Appointment</a></li>");
            html.Append("<li><a href=\"/Member/Appointments.aspx\">View Appointments</a></li>");
            html.Append("<li><a href=\"/Reports/AppointmentDaily.aspx\">Appointment Sheet</a></li>");
            html.Append("<li><a href=\"/Member/PackageBooking.aspx\">New Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/PackageBookings.aspx\">View Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\">Doctor Registration</a></li>");
            html.Append("<li><a href=\"/Member/RegistrationPayment.aspx\">Registration Balance</a></li>");
            html.Append("<li><a href=\"/Member/BookingPayment.aspx\">Session / Pkg. Balance</a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\">Other Cash Entry</a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\">Expense Entry</a></li>");
            html.Append("<li><a href=\"/Member/Leaves.aspx\">My Leave Detail</a></li>");
            html.Append("<li><a href=\"/Member/LeaveAll.aspx\">On Hold Leaves</a></li>");
            html.Append("<li><a href=\"/Member/FeedBack.aspx\">Feedback</a></li>");
            html.Append("<li><a href=\"/Member/Supports.aspx\">All Support Ticket</a></li>");
            html.Append("<li><a href=\"/Member/SupportMy.aspx\">My Support Ticket</a></li>");
            html.Append("<li><a href=\"/Member/BirthDay.aspx\">Birth Day Events</a></li>");
            html.Append("<li><a href=\"/Reports/\">Management Reports</a></li>");
            html.Append("<li><a href=\"/Member/UserAccs.aspx\">User Accounts</a></li>");
            html.Append("<li><a href=\"/Member/Banks.aspx\">Bank Master</a></li>");
            html.Append("<li><a href=\"/Member/Products.aspx\">Product Master</a></li>");
            html.Append("<li><a href=\"/Member/ProductCat.aspx\">Product Categories</a></li>");
            html.Append("<li><a href=\"/Member/PatientChrges.aspx\">Registration Charges</a></li>");
            html.Append("<li><a href=\"/Member/SessionChrges.aspx\">Doctor Charges</a></li>");
            html.Append("<li><a href=\"/Member/Packages.aspx\">Package Master</a></li>");
            //html.Append("<li><a href=\"/Member/SettingContact.aspx\">Closing Contacts</a></li>");
            html.Append("<li><a href=\"/Member/UserAct.aspx\">My Account Activity</a></li>");
        }
        if (_catID == 2)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\">View Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Adult.aspx\">Adult Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Pediatric.aspx\">Pediatric Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Appointment.aspx\">New Appointment</a></li>");
            html.Append("<li><a href=\"/Member/Appointments.aspx\">View Appointments</a></li>");
            html.Append("<li><a href=\"/Reports/AppointmentDaily.aspx\">Appointment Sheet</a></li>");
            html.Append("<li><a href=\"/Member/PackageBooking.aspx\">New Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/PackageBookings.aspx\">View Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\">Doctor Registration</a></li>");
            html.Append("<li><a href=\"/Member/RegistrationPayment.aspx\">Registration Balance</a></li>");
            html.Append("<li><a href=\"/Member/BookingPayment.aspx\">Session / Pkg. Balance</a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\">Other Cash Entry</a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\">Expense Entry</a></li>");
            html.Append("<li><a href=\"/Member/Products.aspx\">Product Master</a></li>");
            html.Append("<li><a href=\"/Member/Leaves.aspx\">Leave Applications</a></li>");
            html.Append("<li><a href=\"/Member/FeedBack.aspx\">Feedback</a></li>");
            html.Append("<li><a href=\"/Member/SupportMy.aspx\">My Support Ticket</a></li>");
            html.Append("<li><a href=\"/Member/BirthDay.aspx\">Birth Day Events</a></li>");
            html.Append("<li><a href=\"/Member/UserAct.aspx\">My Account Activity</a></li>");
        }
        if (_catID == 3)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
            html.Append("<li><a href=\"/Member/TodayAppointments.aspx\">Todays Appointment</a></li>");
            html.Append("<li><a href=\"/Member/MyAppointments.aspx\">Appointment List</a></li>");
            html.Append("<li><a href=\"/Member/DoctorAppSheet.aspx\">Appointment Sheet</a></li>");
            //html.Append("<li><a href=\"/Member/DoctorCrDr.aspx\">All Transactions</a></li>");
            html.Append("<li><a href=\"/Member/DoctorDr.aspx\">Transaction List</a></li>");
            //html.Append("<li><a href=\"/Member/DoctorCr.aspx\">Credit Transaction</a></li>");
            html.Append("<li><a href=\"/Member/Leaves.aspx\">Leave Applications</a></li>");
            html.Append("<li><a href=\"/Member/FeedBack.aspx\">Feedback</a></li>");
            html.Append("<li><a href=\"/Member/SupportMy.aspx\">My Support Ticket</a></li>");
            html.Append("<li><a href=\"/Member/UserAct.aspx\">My Account Activity</a></li>");
        }
        if (_catID == 4)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\">Registratons</a></li>");  
            html.Append("<li><a href=\"/Member/Appointments.aspx\">View Appointments</a></li>"); 
            html.Append("<li><a href=\"/Member/PackageBookings.aspx\">View Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\">Doctor Registration</a></li>");
            html.Append("<li><a href=\"/Member/RegistrationPayment.aspx\">Registration Balance</a></li>");
            html.Append("<li><a href=\"/Member/BookingPayment.aspx\">Session / Pkg. Balance</a></li>"); 
            html.Append("<li><a href=\"/Member/Leaves.aspx\">My Leave Detail</a></li>");
            html.Append("<li><a href=\"/Member/LeaveAll.aspx\">On Hold Leaves</a></li>");
            html.Append("<li><a href=\"/Member/FeedBack.aspx\">Feedback</a></li>");
            html.Append("<li><a href=\"/Member/Support.aspx\">Support Ticket</a></li>"); 
            html.Append("<li><a href=\"/Member/BirthDay.aspx\">Birth Day Events</a></li>");
            html.Append("<li><a href=\"/Reports/\">Management Reports</a></li>"); 
        }
        if (_catID == 5)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
        }
        if (_catID == 6)
        {
            html.Append("<li><a href=\"/Member/\">Dashboard</a></li>");
            html.Append("<li><a href=\"/Member/Patients.aspx\">View Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Adult.aspx\">Adult Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Pediatric.aspx\">Pediatric Registraton</a></li>");
            html.Append("<li><a href=\"/Member/Appointment.aspx\">New Appointment</a></li>");
            html.Append("<li><a href=\"/Member/Appointments.aspx\">View Appointments</a></li>");
            html.Append("<li><a href=\"/Reports/AppointmentDaily.aspx\">Appointment Sheet</a></li>");
            html.Append("<li><a href=\"/Member/PackageBooking.aspx\">New Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/PackageBookings.aspx\">View Package Booking</a></li>");
            html.Append("<li><a href=\"/Member/Doctors.aspx\">Doctor Registration</a></li>");
            html.Append("<li><a href=\"/Member/RegistrationPayment.aspx\">Registration Balance</a></li>");
            html.Append("<li><a href=\"/Member/BookingPayment.aspx\">Session / Pkg. Balance</a></li>");
            html.Append("<li><a href=\"/Member/CashEntry.aspx\">Other Cash Entry</a></li>");
            html.Append("<li><a href=\"/Member/ExpenseEntry.aspx\">Expense Entry</a></li>");
            html.Append("<li><a href=\"/Member/Products.aspx\">Product Master</a></li>");
            html.Append("<li><a href=\"/Member/Leaves.aspx\">Leave Applications</a></li>");
            html.Append("<li><a href=\"/Member/FeedBack.aspx\">Feedback</a></li>");
            html.Append("<li><a href=\"/Member/SupportMy.aspx\">My Support Ticket</a></li>");
            html.Append("<li><a href=\"/Member/BirthDay.aspx\">Birth Day Events</a></li>");
            html.Append("<li><a href=\"/Member/UserAct.aspx\">My Account Activity</a></li>");
        }

        txtMenus.Controls.Add(new LiteralControl(html.ToString()));
    }
}
