<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="MettingScheduleEdit.aspx.cs" Inherits="snehrehab.Member.MettingScheduleEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Edit Dr Meeting Schedule :
            </div>
             <div class="pull-right">
                <a href='<%=returnUrl %>' class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
         <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Therapist :</label>
                    <div class="control-group">
                        <ContentTemplate>
                            <asp:ListBox ID="txtTherapist"  CssClass="chzn-select span4" runat="server" SelectionMode="Multiple"  OnSelectedIndexChanged="txtTherapist_SelectedIndexChanged"></asp:ListBox>  
                        </ContentTemplate>                           
                    </div>
                </div>
             <div class="span6">
                    <label class="control-label">Appointment Date:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAppointmentDate" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAppointmentDate" runat="server" CssClass="span2 my-datepicker-post" AutoPostBack="True" ontextchanged="txtAppointmentDate_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>         
                <div class="clearfix">
                </div>
            </div>
             <div class="formRow" style="visibility:hidden">
                   <div class="span6">
                    <label class="control-label">
                        Time From :</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateTimeFrom" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtAvailability1From" runat="server" CssClass="chzn-select span2">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                 <div class="span6">
                    <label class="control-label">
                        Time Upto :</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanelUpto" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtAvailability1Upto" runat="server" CssClass="chzn-select span2">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
               <%-- <div class="span6">
                    <label class="control-label">
                      Schedule Time From :</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1From" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>  
                        <span class="add-on"><i class="icon-time"></i></span>                      
                    </div>
                </div>                 
                <div class="span6">
                    <label class="control-label">
                      Schedule Time Upto :</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1Upto" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>  --%>
                <div class="clearfix">
                </div>
            </div> 
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Time From :</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtTimeFrom" runat="server" CssClass="chzn-select span2"></asp:DropDownList>                           
                    </div>
                </div>
                 <div class="span6">
                     <label class="control-label">Time Upto :</label>
                     <div class="control-group">
                         <asp:DropDownList ID="txtTimeUpto" runat="server" CssClass="chzn-select span2"></asp:DropDownList>
                     </div>
                 </div>
                
            </div>
            <div class="formRow">   
                 <div class="span6">
                    <label class="control-label">
                        Schedule Type :
                    </label>
                    <div class="control-group">
                        <asp:TextBox ID="txtScheduleType" runat="server" CssClass="span4"></asp:TextBox>
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
                        <div id="black_loader_save"></div>
                        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-danger" OnClick="btnSave_Click" OnClientClick="ShowBlakLoader('black_loader_save');DisableOnSubmit(this);"></asp:LinkButton>
                        &nbsp;
                        <a href='<%=returnUrl %>' class="btn btn-default">Cancel</a>                       
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
