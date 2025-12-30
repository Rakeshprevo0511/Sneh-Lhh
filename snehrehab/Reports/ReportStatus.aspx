<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ReportStatus.aspx.cs" Inherits="snehrehab.Reports.ReportStatus" %>
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
                Report Status:</div>
            <div class="pull-right">
                <a href="ReportStatus.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div id="tb_manager" runat="server"> 
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
                            <a href="/Reports/SiView1.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Si Report</span> </a>
                        </td>
                    </tr>
                </table>
                
                <table class="report-table" >
                    <tr>
                        <td style="width: 25%;">
                            <a href="/Reports/EIPView.aspx">
                                <img src="/images/r-eip-rpt.png" alt="" />
                                <span>EIA Report</span> </a>
                        </td>
                        <td style="width: 25%;">
                            <a href="/Reports/RevalView1.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Re-eval Report</span> </a>
                        </td>
                         <td style="width: 25%;">
                             <a href="/Reports/RevalLast.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>Last Re-eval Report</span> </a>
                        </td>
                        <td style="width:25%;">
                        <a href="/Reports/PreConsultationView.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Pre-Consultation Report</span> </a>
                        </td>
                        <td style="width:25%;">
                        </td>
                    </tr>
                </table>
                <table class="report-table">
                    <tr>
                        <td style="width:25%;">
                            <a href="RevalLastReport.aspx">
                                <img src="/images/r-session-rpt.png" alt=""  />
                                <span>Pop-Up Reval Report</span> 
                            </a>
                        </td>
                        <td style="width:25%;">
                            <a href="PreConsultation.aspx">
                                <img src="/images/r-session-rpt.png" alt=""  />
                                <span>Pre-Consultant Report 2021</span> 
                            </a>
                        </td>

                         <td style="width: 25%;">
                            <a href="../Reports/Si2022.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>si2023 report</span> </a>
                        </td>
                        <td style="width:25%;">
                             <a href="../Reports/Combine_ni_ndt.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>Complete Report</span> </a>
                        
                         <td style="width:25%;"></td>
                    </tr>

                </table>
                  <table class="report-table">
                    <tr>
                       
                              <td style="width:25%">
                                  <a href="../Reports/Ndt_view_2025.aspx">
                                <img src="/images/r-session-rpt.png" alt="" />
                                <span>NDT Report 2025</span> </a>
                        </td>
                         <td style="width:25%;"></td>
                         <td style="width:25%;"></td>
                         <td style="width:25%;"></td>
                        
                    </tr>

                </table>
                <div class="clear">
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
