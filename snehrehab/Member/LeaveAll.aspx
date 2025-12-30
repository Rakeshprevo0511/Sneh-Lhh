<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_LeaveAll" Title="" Codebehind="LeaveAll.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                On Hold Leave :
            </div>
            
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtEmployee" runat="server" placeholder="Search User"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="LeavesGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="LeavesGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="USER DETAIL"><ItemTemplate> <%# Eval("FullName") + "<br/>(" + Eval("LoginName")+")"%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="LEAVE TYPE" DataField="TypeName"/>
                <asp:TemplateField HeaderText="REASON"><ItemTemplate> <%# Eval("Reason").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="FROM"><ItemTemplate> <%# LEAVEFROM(Eval("TypeID").ToString(), Eval("FromDate").ToString(), Eval("FromTime").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                <asp:TemplateField HeaderText="UPTO"><ItemTemplate> <%# LEAVEUPTO(Eval("TypeID").ToString(), Eval("UptoDate").ToString(), Eval("UptoTime").ToString())%></ItemTemplate><HeaderStyle Width="85px" /></asp:TemplateField>
                <asp:TemplateField HeaderText="REQUEST DATE"><ItemTemplate> <%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate><HeaderStyle Width="100px"/></asp:TemplateField>
                <asp:BoundField HeaderText="CONTACT NO" DataField="cNumber"/>
                <%--<asp:TemplateField HeaderText="STATUS"><ItemTemplate> <b><%# Eval("LeaveStatusName").ToString()%></b></ItemTemplate></asp:TemplateField>--%>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <a href='/Member/LeaveEdit.aspx?record=<%#Eval("UniqueID") %>'>Edit</a>
                &nbsp;
                <a href='/Member/LeaveDel.aspx?record=<%#Eval("UniqueID") %>'>Delete</a>
                </ItemTemplate><HeaderStyle Width="70px" CssClass="text-center"/><ItemStyle CssClass="text-center"/></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

