<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master"  CodeBehind="AptWaitings.aspx.cs" Inherits="snehrehab.Member.AptWaitings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.appointment-complete{color: #3C8600;}
.appointment-absent{color: #ff0024;}
.appointment-cancel{color: #ff8400;}
.btn-pay{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-pay:hover{text-decoration: none;}
.btn-cancel{padding: 2px 5px;border-radius: 3px;margin: 0 3px;background-color: #FCB83B;}
.btn-cancel:hover{text-decoration: none;}
.btn-absent{padding: 2px 5px;border-radius: 3px;margin: 0 3px;}
.btn-absent:hover{text-decoration: none;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Waiting List :
            </div>
            <div class="pull-right">
                <a href="/Member/AptWaiting.aspx" class="btn btn-primary">Add Waiting</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtStatus" runat="server" CssClass="input-medium chzn-select span2">
                    <asp:ListItem Value="0">Pending</asp:ListItem>
                    <asp:ListItem Value="1">Confirmed</asp:ListItem>                    
                    <asp:ListItem Value="10">Cancelled</asp:ListItem>
                    <asp:ListItem Value="-1">All Record</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="ddl_Session" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name" CssClass="span2"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="AppointmentGV" runat="server" CssClass="table table-bordered table-responsive" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="AppointmentGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" OnRowDataBound="AppointmentGV_RowDataBound">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:BoundField HeaderText="THERAPIST" DataField="Therapist"/>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <%#GETACTION(Eval("AppointmentStatus").ToString())%>
                    <asp:LinkButton ID="btnConfirm" runat="server" class="btn-pay btn-success" CommandArgument='<%#Eval("AppointmentID") %>' OnClick="btnConfirm_Click">Confirm</asp:LinkButton> 
                    <asp:LinkButton ID="btnCancel" runat="server" class="btn-absent btn-danger" CommandArgument='<%#Eval("AppointmentID") %>' OnClick="btnCancel_Click">Cancel</asp:LinkButton>
                <asp:HiddenField ID="txtAppointmentStatusID" runat="server" Value='<%# Eval("AppointmentStatus") %>'/> 
                </ItemTemplate>                
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="125px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div> 
</asp:Content>
