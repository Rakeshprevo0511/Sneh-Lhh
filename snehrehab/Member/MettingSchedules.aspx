<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="MettingSchedules.aspx.cs" Inherits="snehrehab.Member.MettingSchedules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
    <div class="grid-title">
        <div class="pull-left">
            Dr Meeting Schedule List :
        </div>
        <div class="pull-right">
            <asp:Literal ID="lblAddNew" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="grid-content">
        <div class="formRow">
            <div class="pull-left" style="display:inline-block;margin-right:5px;">
                <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select input-medium span3"> 
                </asp:DropDownList>
            </div>
            <div class="pull-left" style="display:inline-block;margin-right:5px;">
               <asp:TextBox ID="txtScheduleType" runat="server" CssClass="span2" placeholder="Schdeule Type" Width="100px"></asp:TextBox>
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
                AllowPaging="true" onrowdatabound="AppointmentGV_RowDataBound">
        <EmptyDataTemplate>No records found...</EmptyDataTemplate>
        <Columns>
          <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
            <%--<asp:BoundField HeaderText="THERAPIST" DataField="Therapist"/>--%>
            <asp:TemplateField HeaderText="THERAPIST"><ItemTemplate>
                <%#DOCTORLIST(Eval("AppointmentID").ToString())%>
                <asp:HiddenField ID="txtAppointmentID" runat="server" Value='<%# Eval("AppointmentID") %>'/>
                </ItemTemplate></asp:TemplateField>
            <asp:BoundField HeaderText="SCHEDULETYPE" DataField="ScheduleType"/>
            <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
            <%--<asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>--%>
            <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#(Eval("Available1FromChar").ToString() +  " TO " + Eval("Available1UptoChar").ToString())%></ItemTemplate></asp:TemplateField>
            <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <%#GETACTION(Eval("UniqueID").ToString(), Eval("AppointmentStatus").ToString())%>
                <asp:HiddenField ID="txtAppointmentIDs" runat="server" Value='<%# Eval("UniqueID") %>'/>
                <asp:HiddenField ID="txtAppointmentStatusID" runat="server" Value='<%# Eval("AppointmentStatus") %>'/>
                </ItemTemplate>                
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="110px"/>
                </asp:TemplateField>
        </Columns>
        </asp:GridView>
    </div>
</div>
</asp:Content>





























