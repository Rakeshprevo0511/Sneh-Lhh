<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_SupportMy" Title="" Codebehind="SupportMy.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                My Support Tickets :
            </div>
            <div class="pull-right">
                <a href="/Member/Support.aspx" class="btn btn-primary">Create New</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:GridView ID="TicketGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="TicketGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="TICKET"><ItemTemplate><%#Eval("TicketCode")%></ItemTemplate><HeaderStyle Width="80px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="MESSAGE"><ItemTemplate><%#Eval("tMessage")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate><HeaderStyle Width="80px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="STATUS"><ItemTemplate><%#Eval("cStatusName")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="SOLVED REMARK"><ItemTemplate><%#Eval("Remark")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="ATTACHMENT"><ItemTemplate><%#FILELINK(Eval("UniqueID").ToString(), Eval("uFile").ToString())%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

