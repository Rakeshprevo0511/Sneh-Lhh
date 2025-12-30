<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_TodayAppointments" Title="" Codebehind="TodayAppointments.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Today's Appointment :
            </div>
        </div>
        <div class="grid-content">
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="AppointmentGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="AppointmentGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:TemplateField HeaderText="THERAPIST"><ItemTemplate><%#Eval("Therapist").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>
                <%--<asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="80px"/>
                </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

