<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AppChngeRequestP.aspx.cs" Inherits="snehrehab.Member.AppChngeRequestP" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Change Appointment :
            </div>
             <div class="pull-right">
                <a href="/Member/Appointments.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
            <div class="span12">
            <asp:UpdatePanel ID="UpdatDetail" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                        AutoGenerateColumns="false">
                        <EmptyDataTemplate>
                            Patient registration detail not found...</EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="REG DATE"><ItemTemplate><%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                            <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                            <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo" />
                            <asp:BoundField HeaderText="ADDRESS" DataField="rAddress" />
                            <asp:BoundField HeaderText="CITY" DataField="CityName" />
                            <asp:TemplateField HeaderText="BIRTH DATE"><ItemTemplate><%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                            <asp:TemplateField HeaderText="TYPE"><ItemTemplate><%#Eval("PatientType")%></ItemTemplate><HeaderStyle Width="125px" /></asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
             <div class="clearfix">
             </div>
            </div>
            <hr style="border:none;"/>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Select Session:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateSession" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtSession" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                    OnSelectedIndexChanged="txtSession_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Appointment Date:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAppointmentDate" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAppointmentDate" runat="server" CssClass="span2" ReadOnly="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                        <asp:UpdatePanel ID="UpdateTherapist" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select span4" AutoPostBack="True"
                                    OnSelectedIndexChanged="txtTherapist_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Time From :</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateTimeFrom" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtTimeFrom" runat="server" CssClass="chzn-select span2">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdateAssistantTherapist" runat="server">
                <ContentTemplate>
                    <div class="formRow" id="Tab_Assistant1" runat="server" visible="false">
                        <div class="span6">
                            <label class="control-label">
                                Assistant Therapist :</label>
                            <div class="control-group">
                                <asp:DropDownList ID="txtAssistant1" runat="server" CssClass="chzn-select span4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="span6" style="display:none;">
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Duration :</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateDuration" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtDuration" runat="server" CssClass="chzn-select span2">
                                    <asp:ListItem Value="-1">Select Duration</asp:ListItem>
                                    <asp:ListItem Value="15">15 Minute</asp:ListItem>
                                    <asp:ListItem Value="30">30 Minute</asp:ListItem>
                                    <asp:ListItem Value="45">45 Minute</asp:ListItem>
                                    <asp:ListItem Value="60">60 Minute</asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Change Remark :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow" style="padding: 0px;">
                <div class="span12">
                    <hr />
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <div id="black_loader_save"></div>
                        <asp:LinkButton ID="btnSave" runat="server" Text="Update" CssClass="btn btn-danger" OnClick="btnSave_Click" OnClientClick="ShowBlakLoader('black_loader_save');DisableOnSubmit(this);"></asp:LinkButton>
                        &nbsp;
                        <a id="btnCancel" runat="server" href="/Member/Appointments.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
    
    
    
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
