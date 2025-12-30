<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Default" Title="" Codebehind="Default.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <script type="text/javascript">
         var idleTime = 0;
         $(document).ready(function () {
             //Increment the idle time counter every minute.
             var idleInterval = setInterval(timerIncrement, 60000); // 1 minute
             //console.log("idleinterval set " + idleInterval + " " + idleTime);
             //Zero the idle timer on mouse movement.
             $(this).mousemove(function (e) {
                 idleTime = 0;
                 //console.log("idleinterval mv " + idleInterval + " " + idleTime);
             });
             $(this).keypress(function (e) {
                 idleTime = 0;
             });
         });

         function timerIncrement() {
             idleTime = idleTime + 1;
             if (idleTime > 2) { // 15 minutes
                 //console.log("idle time " + idleTime);
                 window.location.reload();
                 //console.log("idle time reload " + idleTime + " idle interval " + idleInterval);
                 window.location.href = '/Login.aspx';
             }
         }
     </script>  


<style type="text/css">
.dashboard-icon{width: 100%;}
.dashboard-icon tr td img{max-width: 8em;}
.dashboard-icon tr td{text-align: center;
    padding-left: 0;
    padding-right: 0;}
.dashboard-icon tr td .btn-group{float:none;}
.dashboard-icon .dropdown-menu{text-align:justify;}
.btn .caret {
    margin-top: 0;
    margin-left: 0;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Dashboard</div>
            <div class="pull-right">
            </div>
            <div class="clear">
            </div>
        </div>  
        <div class="grid-content">
            <div class="buttons_set">
                <table id="tb_admin" runat="server" cellpadding="20" cellspacing="20" class="dashboard-icon">
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-appointment.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-booking.png" alt="" />
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Patients</option>
                                    <option value="/Member/Patients.aspx">View Registration</option>
                                    <option value="/Member/PatientPhotoAlert.aspx">Pending Photo</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Staff Details</option>
                                    <option value="/Member/Doctor.aspx">Add New Doctor</option>
                                    <option value="/Member/Manager.aspx">Add New Manager</option>
                                    <option value="/Member/Receiption.aspx">Add New Receiption</option>
                                    <option value="/Member/Doctors.aspx">View Doctor List</option>
                                    <option value="/Member/ViewList.aspx">View Manager/Receiption List</option>
                                    <option value="/Member/View_CerDeg_List.aspx">View Degree/Certificate List</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reference Doctors</option>
                                    <option value="/Reports/ReferrenceList.aspx">Reference Doctors List</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Appointments</option>
                                    <option value="/Member/Appointments.aspx">View Appointments</option>
                                    <option value="/Member/AppointmentChart.aspx">Appointment Chart</option>
                                    <option value="/Member/MettingSchedule.aspx">Dr. Metting Schedule</option>
                                    <option value="/Member/MettingSchedules.aspx">View Dr. Metting Schedule</option>
                                    <option value="/Member/AppChngeRequest.aspx">Doctor Requests</option>
                                    <option value="/Member/AptWaitings.aspx">View Waiting</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Package Booking</option>
                                    <option value="/Member/PackageBookings.aspx">View Booking</option>
                                    <option value="/Member/PackageBulks.aspx">View Bulk Booking</option>
                                    <option value="/Member/PrintReceipt.aspx">Print Receipt</option>
                                    <option value="/Member/PrintReceiptCas_Cred.aspx">Cash & Credit Print Receipt</option>
                                </select>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-leaves.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-support-ticket.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-reportings.png" alt="" />
                        </td>
                        <td style="text-align: center;">                             
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td style="text-align: center;">
                            <img src="/images/dh-management.png" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Leave Application</option>
                                    <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                    <option value="/Member/LeaveAll.aspx">On Hold Leaves</option>
                                    <option value="/Member/LeaveRpt.aspx">Leave Reports</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Support & Feedback</option>
                                    <option value="/Member/Support.aspx">Create New Ticket</option>
                                    <option value="/Member/Supports.aspx">All Support Ticket</option>
                                    <option value="/Member/Feedback.aspx">New Feedback</option>
                                    <option value="/Member/FeedbackAll.aspx">All Feedback</option>
                                    <option value="/Member/DrYesterdayAppoint.aspx">Dr. Yesterday's Appointment</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reportings</option>
                                    <option value="/Member/BirthDay.aspx">Birth Day Events</option>
                                    <option value="/Member/BirthDayUpComming.aspx">UpComming BirthDay Events</option>
                                    <option value="/Reports/Account.aspx">Account</option>
                                    <option value="/Reports/ReportStatus.aspx">Report Status</option>
                                    <option value="/Reports/AList.aspx">A-List</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Registration Payment Detail</option>
                                    <option value="/Member/RegPaymentDetail.aspx">Payment Detail</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Transactions</option>
                                    <option value="/Member/RegistrationPayment.aspx">Registration Balance</option>
                                    <option value="/Member/BookingPayment.aspx">Session / Pkg. Balance</option>
                                </select>
                            </div>
                        </td>
                    </tr>
                </table>

                <table id="tb_manager" runat="server" cellpadding="20" cellspacing="20" class="dashboard-icon">
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-appointment.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-booking.png" alt="" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Patients</option>
                                    <option value="/Member/Patients.aspx"> View Registration</option>
                                    <option value="/Member/Adult.aspx"> Adult Registraton </option>
                                    <option value="/Member/Pediatric.aspx"> Pediatric Registraton</option>
                                    <option value="/Member/PatientPhotoAlert.aspx"> Pending Photo</option>
                                    <option value="/Member/EnquiryReg.aspx">Enquiry Registration</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                      <option value="/Member/TransferAdult.aspx">Transfer Adult</option>
                                    <option value="/Member/TransferPediatric.aspx">Transfer Pediatric</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Staff Details</option>
                                    <option value="/Member/Doctor.aspx"> Add New Doctor</option>
                                    <option value="/Member/Manager.aspx"> Add New Manager </option>
                                    <option value="/Member/Receiption.aspx"> Add New Receiption</option>
                                    <option value="/Member/Doctors.aspx"> View Doctor List</option>
                                    <option value="/Member/ViewList.aspx">View Manager/Receiption List</option>
                                    <option value="/Member/View_CerDeg_List.aspx">View Degree/Certificate List</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reference Doctors</option>
                                    <option value="/Reports/ReferrenceList.aspx"> Reference Doctors List</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Appointments</option>
                                    <option value="/Member/Appointments.aspx"> View Appointments</option>
                                    <option value="/Member/Appointment.aspx"> New Appointment </option>
                                    <option value="/Member/AppointmentChart.aspx"> Appointment Chart</option>
                                    <option value="/Member/AptWaitings.aspx">View Waiting</option>
                                    <option value="/Member/MettingSchedule.aspx">Dr. Metting Schedule</option>
                                    <option value="/Member/MettingSchedules.aspx">View Dr. Metting Schedule</option>
                                    <option value="/Member/AppointmentDaily.aspx"> Appointment Sheet</option>
                                    <option value="/Member/AppChngeRequest.aspx">Doctor Requests</option>
                                    <option value="/Member/Appointmentc.aspx">Edit Appointments</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Package Booking</option>
                                    <option value="/Member/PackageBookings.aspx"> View Booking</option>
                                    <option value="/Member/PackageBooking.aspx"> New Booking </option>
                                    <option value="/Member/PackageBulks.aspx"> View Bulk Booking</option>
                                    <option value="/Member/PackageBulk.aspx"> New Bulk Booking</option>
                                    <option value="/Member/PrintReceipt.aspx">Print Receipt</option>
                                       <option value="/Member/PrintReceiptCas_Cred.aspx">Cash & Credit Print Receipt</option>
                                </select>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-management.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-leaves.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-support-ticket.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-reportings.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-users.png" alt="" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Transactions</option>
                                    <option value="/Member/RegistrationPayment.aspx">Registration Balance</option>
                                    <option value="/Member/BookingPayment.aspx">Session / Pkg. Balance</option>
                                    <option value="/Member/CashEntry.aspx">Other Cash Entry</option>
                                    <option value="/Member/OtherActivity.aspx">Other Activity Entry</option>
                                    <option value="/Member/ExpenseEntry.aspx">Expense Entry</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Leave Application</option>
                                    <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                    <option value="/Member/LeaveAll.aspx">On Hold Leaves</option>
                                    <option value="/Member/LeaveRpt.aspx">Leave Reports</option>
                                    <option id="optn_altert_sttng" value="/Member/LeaveSetting.aspx" runat="server">Alert Setting</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Support & Feedback</option>
                                <%--<option value="/Member/SupportMy.aspx">My Support Ticket</option>--%>
                                    <option value="/Member/Support.aspx">Create New Ticket</option>
                                    <option value="/Member/Supports.aspx">All Support Ticket</option>
                                    <option value="/Member/Feedback.aspx">New Feedback</option>
                                    <option value="/Member/FeedbackAll.aspx">All Feedback</option>
                                    <option value="/Member/DrYesterdayAppoint.aspx">Dr. Yesterday's Appointment</option>
                                    <option value="/Member/DrYesterdayAppointRecord.aspx">Dr. Yesterday's Appointment Record</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reportings</option>
                                    <option value="/Member/BirthDay.aspx">Birth Day Events</option>
                                    <%-- <option value="/Reports/">Management Reports</option>--%>
                                     <option value="/Member/BirthDayUpComming.aspx">UpComming BirthDay Events</option>
                                    <option value="/Reports/Account.aspx">Account</option>
                                    <option value="/Reports/ReportStatus.aspx">Report Status</option>
                                    <option value="/Reports/AList.aspx">A-List</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>User Accounts</option>
                                    <option value="/Member/UserAccs.aspx">View Accounts</option>
                                    <option value="/Member/UserAcc.aspx" id="create" runat="server">Create Account</option>
                                    <option value="/Member/UserAct.aspx">My Account Activity</option>
                                    <option value="/Member/UserActs.aspx">Users Activity</option>
                                </select>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-settings.png" alt="" />
                        </td>
                    </tr>
                    <tr >
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Registration Payment Detail</option>
                                    <option value="/Member/RegPaymentDetail.aspx">Payment Detail</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Settings</option>
                                    <option value="/Member/Banks.aspx">Bank Master</option>
                                    <option value="/Member/Products.aspx">Product Master</option>
                                    <option value="/Member/PatientChrges.aspx">Registration Charge</option>
                                    <option value="/Member/SessionChrges.aspx">Doctor Charge</option>
                                    <option value="/Member/Packages.aspx">Package Master</option>
                                    <option value="/Member/OtherActProducts.aspx">Other Act.Product</option>
                                </select>
                            </div>
                        </td>
                    </tr>
                </table>

                <table id="tb_reception" runat="server" cellpadding="20" cellspacing="20" class="dashboard-icon">
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-appointment.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-booking.png" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Patients</option>
                                    <option value="/Member/Patients.aspx">View Registraton</option>
                                    <option value="/Member/Adult.aspx">Adult Registraton</option>
                                    <option value="/Member/Pediatric.aspx">Pediatric Registraton</option>
                                    <option value="/Member/PatientPhotoAlert.aspx">Pending Photo</option>
                                    <option value="/Member/EnquiryReg.aspx">Enquiry Registration</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Staff Details</option>
                                    <option value="/Member/Doctor.aspx">Add New Doctor</option>
                                    <option value="/Member/Doctors.aspx">View Doctor List</option>
                                    <option value="/Member/ViewList.aspx">View Manager/Receiption List</option>
                                    <option value="/Member/View_CerDeg_List.aspx">View Degree/Certificate List</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reference Doctors</option>
                                    <option value="/Reports/ReferrenceList.aspx">Reference Doctors List</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Appointments</option>
                                    <option value="/Member/Appointments.aspx">View Appointments</option>
                                    <option value="/Member/Appointment.aspx">New Appointment</option>
                                    <option value="/Member/AptWaitings.aspx">View Waiting</option>
                                    <option value="/Member/AppointmentChart.aspx">Appointment Chart</option>
                                    <option value="/Reports/AppointmentDaily.aspx">Appointment Sheet</option>
                                    <option value="/Member/MettingSchedule.aspx">Dr. Metting Schedule</option>
                                    <option value="/Member/MettingSchedules.aspx">View Dr. Metting Schedule</option>
                                    <option value="/Member/AppChngeRequest.aspx">Doctor Requests</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Package Booking</option>
                                    <option value="/Member/PackageBookings.aspx">View Booking</option>
                                    <option value="/Member/PackageBooking.aspx">New Booking</option>
                                    <option value="/Member/PackageBulks.aspx">View Bulk Booking</option>
                                    <option value="/Member/PackageBulk.aspx">New Bulk Booking</option>
                                    <option value="/Member/PrintReceipt.aspx">Print Receipt</option>
                                       <option value="/Member/PrintReceiptCas_Cred.aspx">Cash & Credit Print Receipt</option>
                                </select>
                            </div>
                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: center;">
                            <img src="/images/dh-management.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-leaves.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-support-ticket.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-birthday.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-products.png" alt="" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Transactions</option>
                                    <option value="/Member/RegistrationPayment.aspx">Registration Balance</option>
                                    <option value="/Member/BookingPayment.aspx">Session / Pkg. Balance</option>
                                    <option value="/Member/CashEntry.aspx">Other Cash Entry</option>
                                    <option value="/Member/OtherActivity.aspx">Other Activity Entry</option>
                                    <option value="/Member/ExpenseEntry.aspx">Expense Entry</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Leave Application</option>
                                    <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Support & Feedback</option>
                                    <option value="/Member/SupportMy.aspx">My Support Ticket</option>
                                    <option value="/Member/Support.aspx">Create New Ticket</option>
                                    <option value="/Member/Feedback.aspx">New Feedback</option>
                                    <option value="/Member/Feedbacks.aspx">My Feedbacks</option>
                                    <option value="/Member/DrYesterdayAppoint.aspx">Dr. Yesterday's Appointment</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Birth Days</option>
                                    <option value="/Member/BirthDay.aspx">Birth Day Events</option>
                                    <option value="/Member/BirthDayUpComming.aspx">UpComming BirthDay Events</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Product Master</option>
                                    <option value="/Member/Products.aspx">Product Master</option>
                                    <option value="/Member/ProductCat.aspx">Product Categories</option>
                                </select>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-reportings.png" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Registration Payment Detail</option>
                                    <option value="/Member/RegPaymentDetail.aspx">Payment Detail</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reportings</option>
                                    <option value="/Reports/">Management Reports</option>
                                    <option value="/Reports/ReportStatus.aspx">Report Status</option>
                                </select>
                            </div>
                        </td>
                    </tr>
                </table>

                <table id="tb_therapist" runat="server" cellpadding="0" cellspacing="20" class="dashboard-icon">
                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-appointment.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-session-rpt.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-management.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-leaves.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-support-ticket.png" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Appointments</option>
                                    <option value="/Member/TodayAppointments.aspx">Todays Appointment</option>
                                    <option value="/Member/MyAppointments.aspx">View Appointments</option>
                                    <option value="/Member/DoctorAppSheet.aspx">Appointment Sheet</option>
                                     <option value="/Member/AppointmentChart.aspx">Appointment Chart</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reports</option>
                                    <option value="/SessionRpt/DailyView.aspx">Daily Report</option>
                                    <option value="/SessionRpt/NdtView.aspx">NDT Report</option>
                                    <option value="/SessionRpt/EIPView.aspx">EIA Report</option>
                                    <option value="/SessionRpt/BotoxView.aspx">Botox Report</option>
                                    <option value="/SessionRpt/SiView.aspx">SI Report</option>
                                    <option value="/SessionRpt/RevalView.aspx">Re-Eval Report</option>
                                    <option value="/SessionRpt/PreConsultationView.aspx">Pre Consultation Report</option>
                                    <option value="/Reports/PreConsultation.aspx">Pre Consultation Report 2021</option>
                                    <option value="/SessionRpt/Construction.aspx?rpt=leave-report">Leave Report</option>
                                    <option value="/SessionRpt/Construction.aspx?rpt=pediatric-registration">Pediatric Registration</option>
                                    <option value="/SessionRpt/Construction.aspx?rpt=patient-report-status">Patient Report Status</option>
                                    <option value="/SessionRpt/Construction.aspx?rpt=patient-statistics">Patient Statistics</option>
                                    <option value="/SessionRpt/Construction.aspx?rpt=total-patient-list">Total Patient List</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                     <option value="/SessionRpt/SIview2022.aspx">SI2023 Report</option>
                                    <option value="/Reports/Ndt_view_2025_other.aspx">NDT REPORT 2025</option>
                                   <%-- <asp:ListItem Value="10">SI2023 Report</asp:ListItem>--%>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Transactions</option>
                                <%--<option value="/Member/DoctorCrDr.aspx">All Transactions</option>--%>
                                    <option value="/Member/DoctorDr.aspx">Transaction List</option>
                                    <option value="/Member/DrAccountSheet.aspx">AccountSheet</option>
                                <%--<option value="/Member/DoctorCr.aspx">Credit Transaction</option>--%>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Leave Application</option>
                                    <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Support & Feedback</option>
                                    <option value="/Member/SupportMy.aspx">My Support Ticket</option>
                                    <option value="/Member/Support.aspx">Create New Ticket</option>
                                    <option value="/Member/Feedback.aspx">New Feedback</option>
                                    <option value="/Member/Feedbacks.aspx">My Feedbacks</option>
                                    <option value="/Member/DrYesterdayAppointMy.aspx">Dr. Yesterday's Appointment</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        
                    </tr>

                    <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-session-rpt.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-session-rpt.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-doctor.png" alt="" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm-mb-5" name="link" onchange="window.location.href=this.value;">
                                    <option>Demo Reports</option>
                                    <option value="/SessionRpt/Demo_DailyRpt.aspx">Demo Daily Report</option>
                                    <option value="/SessionRpt/Demo_NdtRpt.aspx">Demo NDT Report</option>
                                    <option value="/SessionRpt/Demo_BotoxRpt.aspx">Demo Botox Report</option>
                                    <option value="/SessionRpt/Demo_SiRpt.aspx">Demo SI Report</option>
                                    <option value="/SessionRpt/Demo_RevalRpt.aspx">Demo Re-Eval Report</option>
                                    <option value="/SessionRpt/Demo_PreScreenRpt.aspx?">Demo Pre-Screening Report</option>
                                    <option value="/Reports/Demo_Diagnosis.aspx">Demo Diagnosis</option>
                                    <option value="/SessionRpt/Demo_PrConsultRpt.aspx">Demo Pre Consultant 2021</option>
                                    <option value="/SessionRpt/Demo_EIPRpt.aspx">Demo EIP Report</option>
                                    <option value="/SessionRpt/Demo_SIRpt2022.aspx">Demo SI2023 Report</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm-mb-5" name="link" onchange="window.location.href=this.value;">
                                    <option>Patient Reports</option>
                                    <option value="/Reports/Patient_DailyView.aspx">Daily Report</option>
                                    <option value="/Reports/Patient_NdtView.aspx">NDT Report</option>
                                    <option value="/Reports/Patient_BotoxView.aspx">Botox Report</option>
                                    <option value="/Reports/Patient_SiView.aspx">SI Report</option>
                                    <option value="/Reports/Patient_RevalView.aspx">Re-Eval Report</option>
                                    <option value="/Reports/Patient_PreScreenView.aspx">Pre-Screening Report</option>
                                    <option value="/Reports/Patient_PreConsultationView.aspx">Pre-Consultant Report 2021</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                    <option value="/Reports/Patient_Si2022.aspx">SI2023 Report</option>
                                       <option value="/Reports/AList.aspx">A-List</option>
                                       <option value="/Reports/Patient_NDT2025.aspx">NDT-2025</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm-mb-5" name="link" onchange="window.location.href=this.value;">
                                    <option>Reference Doctors & Others</option>
                                    <option value="/Reports/ReferrenceList.aspx">Reference Doctor List</option>
                                </select>
                            </div>
                        </td>
                    </tr>
                </table>

                <table id="tb_researcher" runat="server" cellpadding="0" cellspacing="20" class="dashboard-icon">
                    <tr>
                        <td style="text-align:center">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align:center">
                            <img src="/images/dh-appointment.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align:center">
                            <img src="/images/dh-reportings.png" alt="" />
                        </td>
                          <td style="text-align: center;">
                            <img src="/images/dh-leaves.png" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Patients</option>
                                    <option value="/Member/Patients.aspx"> View Registration</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Appointments</option>
                                    <option value="/Member/Appointments.aspx"> View Appointments</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                           <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Reportings</option>
                                    <option value="/Reports/ReportStatus.aspx">Report Status</option>
                                    <option value="/Reports/AList.aspx">A-List</option>
                                </select>
                           </div> 
                        </td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Leave Application</option>
                                    <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                     <option value="/Member/LeaveAll.aspx">On Hold Leaves</option>
                                    <option value="/Member/LeaveRpt.aspx">Leave Reports</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>

                 <table id="tb_assManager" runat="server" cellpadding="20" cellspacing="20" class="dashboard-icon">
    <tr>
        <td style="text-align: center;">
            <img src="/images/dh-patient.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-doctor.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-doctor.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-appointment.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-booking.png" alt="" />
        </td>
    </tr>
    <tr>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                   <option>Patients</option>
                                      <option value="/Member/Patients.aspx"> View Registration</option>
                                    <option value="/Member/Adult.aspx"> Adult Registraton </option>
                                    <option value="/Member/Pediatric.aspx"> Pediatric Registraton</option>
                                    <option value="/Member/PatientPhotoAlert.aspx"> Pending Photo</option>
                                    <option value="/Member/EnquiryReg.aspx">Enquiry Registration</option>
                                    <option value="/Reports/Diagnosis.aspx">Diagnosis</option>
                                      <option value="/Member/TransferAdult.aspx">Transfer Adult</option>
                                    <option value="/Member/TransferPediatric.aspx">Transfer Pediatric</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Staff Details</option>
                      <option value="/Member/Doctor.aspx"> Add New Doctor</option>
                                    <option value="/Member/Manager.aspx"> Add New Manager </option>
                                    <option value="/Member/Receiption.aspx"> Add New Receiption</option>
                                    <option value="/Member/Doctors.aspx"> View Doctor List</option>
                                    <option value="/Member/ViewList.aspx">View Manager/Receiption List</option>
                                    <option value="/Member/View_CerDeg_List.aspx">View Degree/Certificate List</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Reference Doctors & Others</option>
                    <option value="/Reports/ReferrenceList.aspx"> Reference Doctors List</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Appointments</option>
                    <option value="/Member/Appointments.aspx"> View Appointments</option>
                                    <option value="/Member/Appointment.aspx"> New Appointment </option>
                                    <option value="/Member/AppointmentChart.aspx"> Appointment Chart</option>
                                    <option value="/Member/AptWaitings.aspx">View Waiting</option>
                                    <option value="/Member/MettingSchedule.aspx">Dr. Metting Schedule</option>
                                    <option value="/Member/MettingSchedules.aspx">View Dr. Metting Schedule</option>
                                    <option value="/Member/AppointmentDaily.aspx"> Appointment Sheet</option>
                                    <option value="/Member/AppChngeRequest.aspx">Doctor Requests</option>
                                    
                </select>
            </div>
        </td>
        <td></td>
        <td>
            
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Package Booking</option>
                                    <option value="/Member/PackageBookings.aspx"> View Booking</option>
                                    <option value="/Member/PackageBooking.aspx"> New Booking </option>
                                    <option value="/Member/PackageBulks.aspx"> View Bulk Booking</option>
                                    <option value="/Member/PackageBulk.aspx"> New Bulk Booking</option>
                                    <option value="/Member/PrintReceipt.aspx">Print Receipt</option>
                                    <option value="/Member/PrintReceiptCas_Cred.aspx">Cash & Credit Print Receipt</option>
                </select>
            </div>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
            <img src="/images/dh-management.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-leaves.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-support-ticket.png" alt="" />
        </td>
        <td></td>
        <td style="text-align: center;">
            <img src="/images/dh-reportings.png" alt="" />
        </td>
    </tr>
    <tr>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Transactions</option>
              <option value="/Member/RegistrationPayment.aspx">Registration Balance</option>
                                    <option value="/Member/BookingPayment.aspx">Session / Pkg. Balance</option>
                                    <option value="/Member/CashEntry.aspx">Other Cash Entry</option>
                                    <option value="/Member/OtherActivity.aspx">Other Activity Entry</option>
                                    <option value="/Member/ExpenseEntry.aspx">Expense Entry</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Leave Application</option>
                        <option value="/Member/Leaves.aspx">My Applications</option>
                                    <option value="/Member/Leave.aspx">New Application</option>
                                    <option value="/Member/LeaveAll.aspx">On Hold Leaves</option>
                                    <option value="/Member/LeaveRpt.aspx">Leave Reports</option>
                                    <option id="Option1" value="/Member/LeaveSetting.aspx" runat="server">Alert Setting</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Support & Feedback</option>
                     <%--<option value="/Member/SupportMy.aspx">My Support Ticket</option>--%>
                                    <option value="/Member/Support.aspx">Create New Ticket</option>
                                    <option value="/Member/Supports.aspx">All Support Ticket</option>
                                    <option value="/Member/Feedback.aspx">New Feedback</option>
                                    <option value="/Member/FeedbackAll.aspx">All Feedback</option>
                                    <option value="/Member/DrYesterdayAppoint.aspx">Dr. Yesterday's Appointment</option>
                                    <option value="/Member/DrYesterdayAppointRecord.aspx">Dr. Yesterday's Appointment Record</option>
                </select>
            </div>
        </td>
        <td></td>
        <td>
            <div class="form-group">
                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                    <option>Reportings</option>
                    <option value="/Member/BirthDay.aspx">Birth Day Events</option>
                                    <%-- <option value="/Reports/">Management Reports</option>--%>
                                     <option value="/Member/BirthDayUpComming.aspx">UpComming BirthDay Events</option>
                                    <option value="/Reports/Account.aspx">Account</option>
                                    <option value="/Reports/ReportStatus.aspx">Report Status</option>
                                    <option value="/Reports/AList.aspx">A-List</option>
                </select>
            </div>
        </td>
        <td></td>
    </tr>
     <tr>
                        <td style="text-align: center;">
                            <img src="/images/dh-patient.png" alt="" />
                        </td>
                        <td></td>
                        <td style="text-align: center;">
                            <img src="/images/dh-settings.png" alt="" />
                        </td>
                    </tr>
    <tr>
         <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Registration Payment Detail</option>
                                    <option value="/Member/RegPaymentDetail.aspx">Payment Detail</option>
                                </select>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div class="form-group">
                                <select class="form-control input-sm mb-5" name="links" onchange="window.location.href=this.value;">
                                    <option>Settings</option>
                                    <option value="/Member/Banks.aspx">Bank Master</option>
                                    <option value="/Member/Products.aspx">Product Master</option>
                                    <option value="/Member/PatientChrges.aspx">Registration Charge</option>
                                    <option value="/Member/SessionChrges.aspx">Doctor Charge</option>
                                    <option value="/Member/Packages.aspx">Package Master</option>
                                    <option value="/Member/OtherActProducts.aspx">Other Act.Product</option>
                                </select>
                            </div>
                        </td>
    </tr>
</table>

            </div>
            <div class="clear">
            </div>
            <div class="seperator_dashed">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            /*var pending_photo_html = ''; var pending_photo_shown = false; var pending_photo_header = 'Pending Photo &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <a href="/Member/PatientPhotoAlert.aspx" class="btn btn-mini btn-info" style="font-weight: normal;">View All</a>';
            $.ajax({
                url: '/Member/Alert_PatientPhoto.ashx', contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var status = false; if (result && result.status) { status = result.status; }
                    if (status) {
                        if (result.data && result.data.length) {
                            var html = '';
                            for (var j = 0; j < result.data.length; j++) {
                                if (j == 0) { pending_photo_html += '<div style="padding: 3px;margin: 0px;display: block;clear: both;"></div>'; }
                                pending_photo_html += ('<div class="pending_photo_item"><a class="pull-right btn btn-mini btn-success" href="/Member/Patiente.aspx?record=' + result.data[j].id + '&tab=upload" target="_blank">Upload<a/>' + result.data[j].name + '</div>');
                            }
                        }
                    }
                },
                complete: function () {
                    if (!pending_photo_shown && pending_photo_html.length > 0) {
                        pending_photo_shown = true; tost_notification(pending_photo_header, pending_photo_html, 4);
                    }
                }
            });*/
            var _pa = parseInt($('#<%=txtPasswordAlert.ClientID %>').val()); if (isNaN(_pa)) { _pa = 0; }
            if (_pa > 0) {
                $('#password_alert').modal().on('shown.bs.modal', function () {
                }).show().on('hidden.bs.modal', function () {
                });
                $('#<%=txtPasswordAlert.ClientID %>').val("1");
            }
        });
    </script> 
    <asp:HiddenField ID="txtPasswordAlert" runat="server" Value="0"/>
    <div class="modal fade" id="password_alert" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header" style="background:#ff0606;color:#FFF;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF !important;text-shadow: none;opacity: 1;"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin:0px;">Warning</h5>
                </div>
                <div class="modal-body">
                     <p>
                        Dear User,<br />
                        We have detected your password is too old.<br />
                        For security reason please change your account password.<br /><br />
                        <a href="/Member/UserPwd.aspx">Click here to change password.</a>
                     </p>
                </div>
                <div class="modal-footer"> 
                    <a class="btn btn-danger" data-dismiss="modal">Close</a>
                </div>
            </div>
        </div>
    </div>



    <script type="text/javascript">
        $(function () {

            var _pa = parseInt($('#<%=txtBirthdates.ClientID %>').val()); if (isNaN(_pa)) { _pa = 0; }
            if (_pa > 0) {
                $('#Birth_Alert').modal().on('shown.bs.modal', function () {
                }).show().on('hidden.bs.modal', function () {
                });
                $('#<%=txtBirthdates.ClientID %>').val("1");
            }
        });

        $(function () {

            var _pa = parseInt($('#<%=txtReval.ClientID %>').val()); if (isNaN(_pa)) { _pa = 0; }
            if (_pa > 0) {
                $('#Revalid').modal().on('shown.bs.modal', function () {
                }).show().on('hidden.bs.modal', function () {
                });
                $('#<%=txtReval.ClientID %>').val("1");
             }
        });

        $(function () {

            var _pa = parseInt($('#<%=txtappointment.ClientID %>').val()); if (isNaN(_pa)) { _pa = 0; }
              if (_pa > 0) {
                  $('#Appointid').modal().on('shown.bs.modal', function () {
                  }).show().on('hidden.bs.modal', function () {
                  });
                $('#<%=txtappointment.ClientID %>').val("1");
            }
        });
    </script>

 <asp:HiddenField ID="txtBirthdates" runat="server" Value="0" />
    <asp:HiddenField ID="txtReval" runat="server" Value="0" />
    <asp:HiddenField ID="txtappointment" runat="server" Value="0" />
    <div class="modal fade" id="Birth_Alert" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header" style="background: #ff0606; color: #FFF;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF !important; text-shadow: none; opacity: 1;"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">Todays BirthDay</h5>
                </div>
                <div class="modal-body">
                    <p>
                        Dear User,<br />
                        Check Todays BirthDay.<br />
                        Send Mail Or Text Message.<br />
                        <br />
                        <a href="/Member/BirthDay.aspx">Click here to wish.</a>
                    </p>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-danger" data-dismiss="modal">Close</a>
                </div>
            </div>
        </div>
    </div>


     <div class="modal fade" id="Revalid" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header" style="background: #ff0606; color: #FFF;">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF !important; text-shadow: none; opacity: 1;"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">Todays Re-Eval /Upcomming 15 Days</h5>
                </div>
                <div class="modal-body">
                    <p>
                        Dear User,<br />
                      Todays Re-Eval /Upcomming 15 Days.<br />
                        
                        <br />
                        <a href="/Reports/RevalPopUp.aspx">Click here to wish.</a>
                    </p>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-danger" data-dismiss="modal">Close</a>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Appointid" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header" style="background: #ff0606; color: #FFF;">
                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF !important; text-shadow: none; opacity: 1;"><span aria-hidden="true">&times;</span></button>--%>
                    <h5 class="modal-title" style="margin: 0px;">Yesterday's Appointment</h5>
                </div>
                <div class="modal-body">
                    <p>
                        Dear User,<br />
                        Check Yesterday's Appointment .<br />

                        <br />
                        <a href="/member/YesterdayAppointments.aspx">Click here to Check.</a>
                    </p>
                </div>
                <%--<div class="modal-footer">
                    <a class="btn btn-danger" data-dismiss="modal">Close</a>
                </div>--%>
            </div>
        </div>
    </div>



</asp:Content>