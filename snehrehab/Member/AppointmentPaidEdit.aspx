<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" 
    CodeBehind="AppointmentPaidEdit.aspx.cs" Inherits="snehrehab.Member.AppointmentPaidEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .my-checkbox
        {
        }
        .my-checkbox input[type='checkbox']
        {
            padding: 0;
            margin: 0;
            margin-top: -3px;
        }
        .my-checkbox label
        {
            display: inline-block;
            float: none;
            width: auto;
            margin: 0px;
            padding: 0px;
            margin-left: 10px;
            margin-top: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Edit Appointment Entry :
            </div>
            <div class="pull-right">
                <a href="/Member/Appointmentc.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content" style="padding: 0px;">
            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" CssClass="fancy fancy-green">
                <ajaxToolkit:TabPanel ID="tb_Session" runat="server" HeaderText="Session">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="REG DATE">
                                                <ItemTemplate>
                                                    <%# DbHelper.Configuration.FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate>
                                                <HeaderStyle Width="85px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FULL NAME">
                                                <ItemTemplate>
                                                    <%# Eval("FullName").ToString()%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="YEAR" DataField="AgeYear">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="MONTH" DataField="AgeMonth">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="GENDER" DataField="Gender">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemStyle CssClass="text-center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="CATEGORY" DataField="CategoryName" />
                                            <asp:BoundField HeaderText="ADULT CASE" DataField="AdultCase" />
                                            <asp:TemplateField HeaderText="TYPE">
                                                <ItemTemplate>
                                                    <%#Eval("PatientType")%></ItemTemplate>
                                                <HeaderStyle Width="145px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="custome-pagination" />
                                    </asp:GridView>
                                    <hr style="border: none;" />
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdateAll" runat="server">
                                <ContentTemplate>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Select Session:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtSession" runat="server" CssClass="chzn-select span4" AutoPostBack="True" OnSelectedIndexChanged="txtSession_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Appointment Date:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtAppointmentDate" runat="server" CssClass="span2 my-datepicker-post"
                                                    AutoPostBack="True" OnTextChanged="txtAppointmentDate_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Therapist :</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                                    OnSelectedIndexChanged="txtTherapist_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Time From :</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtTimeFrom" runat="server" CssClass="chzn-select span2">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" id="Tab_Assistant1" runat="server" visible="false">
                                        <div class="span6">
                                            <label class="control-label">
                                                Assistant Therapist :</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtAssistant1" runat="server" CssClass="chzn-select span4">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6" style="display: none;">
                                            <label class="control-label">
                                                Assistant Share :</label>
                                            <div class="control-group">
                                                <div class="span2" style="margin-left: 0;">
                                                    <asp:TextBox ID="txtAssistantShare" runat="server" CssClass="span2"></asp:TextBox>
                                                </div>
                                                <asp:DropDownList ID="txtAssistantShareType" runat="server" CssClass="span1">
                                                    <asp:ListItem Value="2">%</asp:ListItem>
                                                    <asp:ListItem Value="1">Rs</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Duration :</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtDuration" runat="server" CssClass="chzn-select span2" OnSelectedIndexChanged="txtDuration_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="-1">Select Duration</asp:ListItem>
                                                    <asp:ListItem Value="15">15 Minute</asp:ListItem>
                                                    <asp:ListItem Value="30">30 Minute</asp:ListItem>
                                                    <asp:ListItem Value="45">45 Minute</asp:ListItem>
                                                    <asp:ListItem Value="60">60 Minute</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <hr style="border: none;" />
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Entry Date/Time:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtEntryDateTime" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                                <asp:Label ID="lblEntryDateTime" runat="server" CssClass="span1"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Pay From Bulk Package:</label>
                                            <div class="control-group">
                                                <asp:CheckBox ID="chkBulkPackage" runat="server" CssClass="checkbox" Text=" Yes"
                                                    AutoPostBack="True" OnCheckedChanged="chkBulkPackage_CheckedChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Type_Package" runat="server" visible="False">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Patient Packages:</label>
                                                <div class="control-group">
                                                    <div class="span3" style="margin-left: 0px;">
                                                        <asp:DropDownList ID="txtPatientPackages" runat="server" CssClass="chzn-select span3"
                                                            OnSelectedIndexChanged="txtPatientPackages_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="span1" style="margin-left: 20px; margin-top: 6px;">
                                                        <asp:LinkButton ID="btnNewPackage" runat="server" CssClass="btn btn-info btn-mini"
                                                            Text="Book Now" OnClick="btnNewPackage_Click"></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Previous Balance:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPackageBalance" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Session Charge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionCharge" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Carry Forword:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtBalanceAmount" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Type_Evaluation" runat="server" visible="False">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Select Evaluation:</label>
                                                <div class="control-group">
                                                    <div class="span3" style="margin-left: 0px;">
                                                        <asp:DropDownList ID="txtEvaluation" runat="server" CssClass="chzn-select span4"
                                                            OnSelectedIndexChanged="txtEvaluation_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Session Charge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtEvaluationAmount" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Type_Single" runat="server" visible="False">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Session Charge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSingleSessionCharge" runat="server" CssClass="span4" onkeyup="SingleSessionChargePay();"
                                                        onchange="SingleSessionChargePay();"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="update20" runat="server"><ContentTemplate>
                                    <div id="Type_BulkPay" runat="server" visible="False">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Session Charge:</label>
                                                
                                                        <div class="control-group">
                                                            <asp:TextBox ID="txtBulkSessionCharge" runat="server" CssClass="span4" onkeyup="BulkSessionChargePay();"
                                                                onchange="BulkSessionChargePay();"></asp:TextBox>
                                                        </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    </ContentTemplate></asp:UpdatePanel>
                                    <div class="formRow" style="padding: 0;">
                                        <div class="span12">
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="formRow" id="tabPaymentModes" runat="server">
                                        <div class="span6">
                                            <label class="control-label">
                                                Payment Type:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtPaymentType" runat="server" CssClass="chzn-select" AutoPostBack="True"
                                                    OnSelectedIndexChanged="txtPaymentType_SelectedIndexChanged">
                                                    <%--<asp:ListItem Value="-1">Select Payment Type</asp:ListItem>
                                                    <asp:ListItem Value="1">Cash Payment</asp:ListItem>
                                                    <asp:ListItem Value="2">Credit Payment</asp:ListItem>
                                                    <asp:ListItem Value="3">Cheque Payment</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Amount To Pay:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtAmountToPay" runat="server" CssClass="span4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div id="tabPaymentBulkMode" runat="server">
                                    </div>
                                    <div id="bulk" runat="server" visible="true">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Selected Bulk Booking :</label>
                                                <div class="control-group">
                                                    <%--<asp:DropDownList ID="txtBulkPackages" runat="server" CssClass="chzn-select span4"
                                                        AutoPostBack="True" OnSelectedIndexChanged="txtBulkPackages_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtBulkPackages" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6" id="bulkpack" runat="server" visible="false">
                                                <label class="control-label">
                                                    Select Package :</label>
                                                <div class="control-group">
                                                    <asp:DropDownList ID="txtbulkpackagesnew" runat="server" CssClass="chzn-select span4"
                                                        AutoPostBack="True" OnSelectedIndexChanged="txtbulkpackagesnew_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="bulkpackage" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Appointment Charge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtbulkappointmentcharge" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="bulkusedamntchrg" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Used AppointmentCharge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtusedappointmentcharge" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Used AppointmentCount:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtusedappointmentcount" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="bulkcarry" runat="server" visible="true">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Balance Amount:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtBulkBalance" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Carry Forword:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtBulkForword" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tb_SessionBank" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Bank Name:</label>
                                                <div class="control-group">
                                                    <asp:DropDownList ID="txtSessionBankName" runat="server" CssClass="chzn-select span4">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Branch Name:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionBankBranch" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Cheque No.:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionChequeNo" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Cheque Date:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionChequeDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tb_SessionOnline" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Transaction ID:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionTransactionID" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Transaction Date:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtSessionTransactionDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tb_SessionOtherPackages" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Select Package:</label>
                                                <div class="control-group">
                                                    <asp:DropDownList ID="txtPaymentOtherPackage" runat="server" CssClass="chzn-select span4"
                                                        AutoPostBack="True" OnSelectedIndexChanged="txtPaymentOtherPackage_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Previous Balance:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPaymentOtherPackageBalance" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Carry Forword:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPaymentOtherBalanceAmount" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Narration:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtSessionNarration" runat="server" CssClass="span4" TextMode="MultiLine"
                                                    Rows="2"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="padding: 0;">
                                        <div class="span12">
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                &nbsp;</label>
                                            <div class="control-group">
                                                <div id="black_loader_pay">
                                                </div>
                                                <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit Changes" CssClass="btn btn-danger"
                                                    OnClick="btnSubmit_Click" OnClientClick="if(confirm('Are you sure to submit appointment changes..??')){ShowBlakLoader('black_loader_pay');DisableOnSubmit(this);return true;}else{return false;}"></asp:LinkButton>
                                                &nbsp; <a href="/Member/Appointmentc.aspx" class="btn btn-default">Cancel</a>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnNewPackage" />
                                    <asp:PostBackTrigger ControlID="btnSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Packages" runat="server" HeaderText="Package">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <asp:UpdatePanel ID="UpdatePaneNew" runat="server">
                                <ContentTemplate>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Select Package:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtPackage" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                                    OnSelectedIndexChanged="txtPackage_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Appointment Charge:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtAppointmentCharge" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" id="tbExtraSessionCharge" runat="server" visible="false">
                                        <div class="span6">
                                            <label class="control-label">
                                                Extra Session Charge:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtExtraSessionCharge" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Appointment Count:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtAppointmentCount" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Package Amount:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtPackageAmount" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Apply Discount:</label>
                                            <div class="control-group">
                                                <div class="span1" style="margin-left: 0px;">
                                                    <asp:CheckBox ID="txtPackageIsDiscounted" runat="server" Text=" Yes" CssClass="my-checkbox"
                                                        AutoPostBack="true" OnCheckedChanged="txtPackageIsDiscounted_CheckedChanged" />
                                                </div>
                                                <div class="span3">
                                                    <asp:DropDownList ID="txtPackageDiscountedOn" runat="server" CssClass="chzn-select"
                                                        Width="100%" AutoPostBack="True" OnSelectedIndexChanged="txtPackageDiscountedOn_SelectedIndexChanged"
                                                        Enabled="false">
                                                        <asp:ListItem Value="-1">Select Discount</asp:ListItem>
                                                        <asp:ListItem Value="1">Discount In Session</asp:ListItem>
                                                        <asp:ListItem Value="2">Discount In Package</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Discount Type:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtPackageDiscountType" runat="server" CssClass="chzn-select span2"
                                                    AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="txtPackageDiscountType_SelectedIndexChanged">
                                                    <asp:ListItem Value="-1">Discount Type</asp:ListItem>
                                                    <asp:ListItem Value="1">Percent (%)</asp:ListItem>
                                                    <asp:ListItem Value="2">Rupees (INR)</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div id="tbPackageDiscount" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Discount Value:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPackageDiscountValue" runat="server" CssClass="span4" AutoPostBack="True"
                                                        OnTextChanged="txtPackageDiscountValue_TextChanged"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Net Payable:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPackageNetAmt" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tbPackageDiscountSessionCh" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    New Session Charge:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtPackageSessionChargeNew" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Mode of Payment:</label>
                                            <div class="control-group">
                                                <asp:DropDownList ID="txtPaymentMode" runat="server" CssClass="chzn-select span4"
                                                    OnSelectedIndexChanged="txtPaymentMode_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="1">Cash</asp:ListItem>
                                                    <asp:ListItem Value="2">Credit</asp:ListItem>
                                                    <asp:ListItem Value="3">Cheque</asp:ListItem>
                                                    <asp:ListItem Value="4">Online</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <label class="control-label">
                                                Booking Date:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtBookingDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div id="tab_Cheque" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Bank Name:</label>
                                                <div class="control-group">
                                                    <asp:DropDownList ID="txtBankName" runat="server" CssClass="chzn-select span4">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Branch Name:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtBranchName" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Cheque No.:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtChequeNo" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Cheque Date:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab_online" runat="server" visible="false">
                                        <div class="formRow">
                                            <div class="span6">
                                                <label class="control-label">
                                                    Transaction ID:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtTransactionID" runat="server" CssClass="span4"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <label class="control-label">
                                                    Transaction Date:</label>
                                                <div class="control-group">
                                                    <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                Narration:</label>
                                            <div class="control-group">
                                                <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span6">
                                            <label class="control-label">
                                                &nbsp;</label>
                                            <div class="control-group">
                                                <div id="black_loader_package">
                                                </div>
                                                <asp:LinkButton ID="btnSavePackage" runat="server" Text="Book Now" CssClass="btn btn-danger"
                                                    OnClick="btnSavePackage_Click" OnClientClick="if(confirm('Are you sure to book new package..??')){ShowBlakLoader('black_loader_package');DisableOnSubmit(this);return true;}else{return false;}"></asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="btnCancelPackage" runat="server" Text="Cancel" CssClass="btn btn-default"
                                                    OnClick="btnCancelPackage_Click"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                   
                                   <asp:PostBackTrigger ControlID="btnCancelPackage" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                    
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    <asp:HiddenField ID="hidStatus" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="txthSingleSession" runat="server" />
            <asp:HiddenField ID="txthBulkPackage" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function SingleSessionChargePay() {
            var c = 0; try { c = document.getElementById('<%= txthSingleSession.ClientID %>').value; } catch (e) { } if (isNaN(c)) { c = 0; }
            if (c > 0) {
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").disabled = true; } catch (e) { }
                var a = 0; try { a = document.getElementById('<%= txtSingleSessionCharge.ClientID %>').value; } catch (e) { }
                if (isNaN(a)) { a = 0; }
                var val = ""; if (a > 0) { val = a.toString(); }
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").disabled = false; } catch (e) { }
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").value = val; } catch (e) { }
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").setAttribute("value", val) } catch (e) { }
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").innerText = val; } catch (e) { }
                try { document.getElementById("<%= txtAmountToPay.ClientID %>").disabled = true; } catch (e) { }
            }
        }
        function BulkSessionChargePay() {
            var c = 0; try { c = document.getElementById('<%= txthBulkPackage.ClientID %>').value; } catch (e) { } if (isNaN(c)) { c = 0; }
            if (c > 0) {
                if ($("#<%=Type_BulkPay.ClientID %>").length > 0) 
                {
                    var cc = 0; try { cc = document.getElementById('<%= txtBulkSessionCharge.ClientID %>').value; } catch (e) { } if (isNaN(cc)) { cc = 0; }
                    var bb = 0; try { bb = document.getElementById('<%= txtBulkBalance.ClientID %>').value; } catch (e) { } if (isNaN(bb)) { bb = 0; }
                    var val = (bb - cc).toString();
                    try { document.getElementById("<%= txtBulkForword.ClientID %>").disabled = false; } catch (e) { }
                    try { document.getElementById("<%= txtBulkForword.ClientID %>").value = val; } catch (e) { }
                    try { document.getElementById("<%= txtBulkForword.ClientID %>").setAttribute("value", val) } catch (e) { }
                    try { document.getElementById("<%= txtBulkForword.ClientID %>").innerText = val; } catch (e) { }
                    try { document.getElementById("<%= txtBulkForword.ClientID %>").disabled = true; } catch (e) { }

                }
                else 
                {                 
                }
            }
        }
        $(function () {
            var status = $("#<%= hidStatus.ClientID %>").val(); if (isNaN(status)) { status = 0; }
            if (status <= 0) {
                SingleSessionChargePay();
                BulkSessionChargePay();
            } else {
                $("#<%= hidStatus.ClientID %>").val('0');
            }
        });
    </script>
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
