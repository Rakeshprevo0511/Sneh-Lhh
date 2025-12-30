<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true"
    Inherits="Reports_Default" Title="" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <style type="text/css">
@-webkit-keyframes pulse {25% {-webkit-transform: scale(1.02);transform: scale(1.02);}75% {-webkit-transform: scale(0.97);transform: scale(0.97);}}
@keyframes pulse {25% {-webkit-transform: scale(1.02);transform: scale(1.02);}75% {-webkit-transform: scale(0.97);transform: scale(0.97);}}
.report-table{width:100%;}
.report-table td a{text-align:center;display: block;text-decoration:none;font-family: Trebuchet MS;color: #575A61;padding: 10px 20px;margin: 10px;border: 1px solid #029E51;box-shadow: 2px 3px #008945;}
.report-table td a img{width:8em;}
.report-table td a span{display: block;margin: 10px auto;font-size: 1.4em;text-transform:uppercase;}
.report-table td a:hover{color:#008945;text-decoration: underline;-webkit-animation-name: pulse;animation-name: pulse;-webkit-animation-duration: 1s;animation-duration: 1s;-webkit-animation-timing-function: linear;animation-timing-function: linear;-webkit-animation-iteration-count: infinite;animation-iteration-count: infinite;
border-color:#0874b4;box-shadow: 2px 3px #0874b4;}
</style> 
<script type="text/javascript">
    $('<img src="/images/r-hospital-account.png" />');
    $('<img src="/images/r-patient-account.png" />');
    $('<img src="/images/r-doctor-account.png" />');
    $('<img src="/images/r-head-account.png" />');
    $('<img src="/images/r-monthly-account.png" />');
    $('<img src="/images/r-account-sheet.png" />');
    $('<img src="/images/r-cash-account.png" />');
    $('<img src="/images/r-session-rpt.png" />');
    $('<img src="/images/r-adult-account.png" />');
    $('<img src="/images/r-appointment-daily.png" />');
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Management Reports :</div>
        </div>
        <div class="grid-content">
            <div id="tb_admin" runat="server">
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/HospitalAccount.aspx">
                                <img src="/images/r-hospital-account.png" alt="" />
                                <span>Hospital Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/MonthlyAccount.aspx">
                                <img src="/images/r-monthly-account.png" alt="" />
                                <span>Monthly Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountHead.aspx">
                                <img src="/images/r-head-account.png" alt="" />
                                <span>Account Head</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountSheet.aspx">
                                <img src="/images/r-account-sheet.png" alt="" />
                                <span>Account Sheet</span> </a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div> 
            </div>
            <div id="tb_manager" runat="server">
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/HospitalAccount.aspx">
                                <img src="/images/r-hospital-account.png" alt="" />
                                <span>Hospital Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/PatientAccount.aspx">
                                <img src="/images/r-patient-account.png" alt="" />
                                <span>Patient Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/DoctorAccount.aspx">
                                <img src="/images/r-doctor-account.png" alt="" />
                                <span>Doctor Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountHead.aspx">
                                <img src="/images/r-head-account.png" alt="" />
                                <span>Account Head</span> </a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/MonthlyAccount.aspx">
                                <img src="/images/r-monthly-account.png" alt="" />
                                <span>Monthly Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountSheet.aspx">
                                <img src="/images/r-account-sheet.png" alt="" />
                                <span>Account Sheet</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/CashEntries.aspx">
                                <img src="/images/r-cash-account.png" alt="" />
                                <span>Other Cash Entries</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AppointmentDaily.aspx">
                                <img src="/images/r-appointment-daily.png" alt="" />
                                <span>Appointment Sheet</span></a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/DailyView.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Daily Report</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/NdtView.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>NDT Report</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/BotoxView.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Botox Report</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/PatientRptStatus.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>PATIENT STATUS</span> </a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/MonthlyPatientList.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>MONTHLY PATIENT LIST</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/PediatricFrequency.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>PEDIATRIC FREQUENCY</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/LeaveReport.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>LEAVE REPORT</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/DoctorSession.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Doctor Session</span> </a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/WebForm1.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Graph</span> </a>
                        </td>
                        <td style="width: 25%;">
                         <a href="/Reports/SiView1.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>SI Report</span> </a>
                       </td>
                        <td style="width: 25%;">
                        <a href="/Reports/EIPView.aspx">
                        <img src="/images/r-eip-rpt.png" alt="" />
                        <span>EIA Report</span>
                        </a>
                        </td>
                        <td style="width: 25%;">
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
            </div>
            <div id="tb_reception" runat="server">
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/MonthlyAccount.aspx">
                                <img src="/images/r-monthly-account.png" alt="" />
                                <span>Monthly Account</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountSheet.aspx">
                                <img src="/images/r-account-sheet.png" alt="" />
                                <span>Account Sheet</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/CashEntries.aspx">
                                <img src="/images/r-cash-account.png" alt="" />
                                <span>Other Cash Entries</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/AccountHead.aspx">
                                <img src="/images/r-head-account.png" alt="" />
                                <span>Account Head</span> </a>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/AppointmentDaily.aspx">
                                <img src="/images/r-appointment-daily.png" alt="" />
                                <span>Appointment Sheet</span></a>
                        </td>
                        <td style="width: 25%;">
                        </td>
                        <td style="width: 25%;">
                        </td>
                        <td style="width: 25%;">
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="seperator_dashed">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
