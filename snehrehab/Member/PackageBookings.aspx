<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_PackageBookings" Title="" Codebehind="PackageBookings.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Package Booking List :
            </div>
            <div class="pull-right">
                <asp:Literal ID="lblAddNew" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <div class="alert alert-info">
            <strong>Total Booking Amount : 
                <asp:Label ID="lblTotal" runat="server" ></asp:Label>/- INR</strong>
            </div>
            <asp:GridView ID="BookingGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="BookingGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo"/>
                <asp:BoundField HeaderText="CITY" DataField="CityName"/>
                <asp:BoundField HeaderText="PATIENT TYPE" DataField="PatientType"/>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:BoundField HeaderText="PACKAGE" DataField="PackageCode"/>
                <asp:TemplateField HeaderText="AMOUNT"><ItemTemplate><%#Eval("PackageAmount")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="BOOKING DATE"><ItemTemplate><%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ADDED DATE"><ItemTemplate><%# FORMATDATETIME(Eval("AddedDateTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CARRY FWD"><ItemTemplate><%# TOTALBALANCE(Eval("TotalBalance").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%# DELETELINK(Eval("UniqueID").ToString(), Eval("CanDelete").ToString())%>

                     <br />
                   <%# CHECKUSAGELINK(Eval("BookingID").ToString(), Eval("CanDelete").ToString()) %>

                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="110px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
