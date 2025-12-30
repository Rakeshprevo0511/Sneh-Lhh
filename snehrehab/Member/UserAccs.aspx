<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_UserAccs" Title="" Codebehind="UserAccs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                User Accounts :
            </div>
            <div id="btn_create" class="pull-right" runat="server">
                <a href="/Member/UserAcc.aspx" class="btn btn-primary">Create New</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="AccountGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="AccountGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="MOBILE NO" DataField="MobileNo"/>
                <asp:BoundField HeaderText="E-MAIL ID" DataField="MailID"/>
                <asp:BoundField HeaderText="LOGIN NAME" DataField="LoginName"/>
                <asp:BoundField HeaderText="CATEGORY" DataField="CategoryName"/>
                <asp:TemplateField HeaderText="MODIFY DATE"><ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("ModifyDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <a href='/Member/UserAcce.aspx?record=<%#Eval("UniqueID") %>'>Edit</a>
                &nbsp;
                <a href='/Member/UserAccd.aspx?record=<%#Eval("UniqueID") %>'>Delete</a>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="80px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>