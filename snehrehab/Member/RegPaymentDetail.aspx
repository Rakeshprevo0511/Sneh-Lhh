<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="RegPaymentDetail.aspx.cs" Inherits="snehrehab.Member.RegPaymentDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Patient Registration List :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtpaymentmode" runat="server" CssClass="input-medium">
                    <asp:ListItem Value="1">Cash</asp:ListItem>                                                
                    <asp:ListItem Value="3">Cheque</asp:ListItem>
                    <asp:ListItem Value="4">Online</asp:ListItem>
                    <asp:ListItem Value="100">Bulk Package</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                
             </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="RegPayDetailGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="RegPayDetailGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate> <%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <%--<asp:TemplateField HeaderText="BULK PACKAGE AMOUNT"><ItemTemplate><%# Eval("PackageAmount") %></ItemTemplate></asp:TemplateField>--%>
                <asp:TemplateField HeaderText="REGISTRATION AMOUNT"><ItemTemplate><%# Eval("CreditAmt") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PAY DATE"><ItemTemplate> <%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate><HeaderStyle Width="85px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="REG DATE"><ItemTemplate> <%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PAYMODE"><ItemTemplate> <%#Eval("PayMode")%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
