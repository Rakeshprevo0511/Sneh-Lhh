<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ViewList.aspx.cs" Inherits="snehrehab.Member.ViewList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                All List :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtType" runat="server" CssClass="input-medium">
                <asp:ListItem Value="1">Management</asp:ListItem>
                <asp:ListItem Value="3">Receiption</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                
             </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="StaffGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="StaffGV_PageIndexChanging" PageSize="10" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate> <%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="TELEPHONE" DataField="ContactNo"/>
                <asp:BoundField HeaderText="EMAIL ID" DataField="MailID"/>
                <asp:BoundField HeaderText="DESIGNATION" DataField="Designation"/>
                <asp:BoundField HeaderText="QUALIFICATIONS" DataField="Qualifications"/>
                <asp:TemplateField HeaderText="BIRTH DATE"><ItemTemplate> <%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate><HeaderStyle Width="85px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="JOIN DATE"><ItemTemplate> <%# FORMATDATE(Eval("JoinDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <%#GetAction(Eval("UniqueID").ToString(), Eval("TYPEID").ToString())%>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
