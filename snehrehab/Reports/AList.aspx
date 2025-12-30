<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AList.aspx.cs" Inherits="snehrehab.Reports.AList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                A-List Reports :</div>
            <div class="pull-right">
                <a href="AList.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div id="tb_manager" runat="server">
                <table class="report-table">
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/MonthlyPatientList.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>MONTHLY PATIENT LIST</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/LeaveReport.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>LEAVE REPORT</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/DoctorSession.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Daily State Report</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/PediatricFrequency.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>PEDIATRIC FREQUENCY</span> </a>
                        </td>
                    </tr>
                      <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/PatienSession.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span> PATIENT SESSION LIST</span> </a>
                        </td>
                             </tr>
                </table>
                <div class="clear">
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
