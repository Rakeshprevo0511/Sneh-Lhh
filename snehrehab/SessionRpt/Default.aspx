<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="snehrehab.SessionRpt.Default" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
@-webkit-keyframes pulse {25% {-webkit-transform: scale(1.03);transform: scale(1.03);}75% {-webkit-transform: scale(0.97);transform: scale(0.97);}}
@keyframes pulse {25% {-webkit-transform: scale(1.03);transform: scale(1.03);}75% {-webkit-transform: scale(0.97);transform: scale(0.97);}}
.report-table{width:100%;}
.report-table td {max-width:30%;}
.report-table td a{text-align:center;display: block;text-decoration:none;font-family: Trebuchet MS;color: #575A61;padding: 10px 20px;margin: 10px;border: 1px solid #029E51;box-shadow: 2px 3px #008945;}
.report-table td a img{width:8em;}
.report-table td a span{display: block;margin: 10px auto;font-size: 1.4em;text-transform:uppercase;}
.report-table td a:hover{color:#008945;text-decoration: underline;-webkit-animation-name: pulse;animation-name: pulse;-webkit-animation-duration: 1s;animation-duration: 1s;-webkit-animation-timing-function: linear;animation-timing-function: linear;-webkit-animation-iteration-count: infinite;animation-iteration-count: infinite;}
</style> 
<script type="text/javascript">
    $('<img src="/images/r-session-rpt.png" />');
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Session Reports :</div>
        </div>
        <div class="grid-content">
            <table class="report-table">
                <tr>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/DailyView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>Daily Report</span> </a>
                    </td>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/NdtView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>NDT Report</span> </a>
                    </td>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/BotoxView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>Botox Report</span> </a>
                    </td>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/SiView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>SI Report</span> </a>
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
                        <a href="/SessionRpt/EIPView.aspx">
                            <img src="/images/r-eip-rpt.png" alt="" />
                            <span>EIP Report</span> </a>
                    </td>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/RevalView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>Re-Eval Report</span> </a>
                    </td>
                    <td style="width: 25%;">
                        <a href="/SessionRpt/PreConsultationView.aspx">
                            <img src="/images/r-session-rpt.png" alt="" />
                            <span>Pre-Consultation Report</span> </a>
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
</asp:Content>
