<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Leaves" Title="" Codebehind="Leaves.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                My Leave Applications :
            </div>
            <div class="pull-right">
                <a href="/Member/Leave.aspx" class="btn btn-primary">New Leave</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="LeavesGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="LeavesGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="LEAVE TYPE"><ItemTemplate> <%# Eval("TypeName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="REASON"><ItemTemplate> <%# Eval("Reason").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="FROM"><ItemTemplate> <%# LEAVEFROM(Eval("TypeID").ToString(), Eval("FromDate").ToString(), Eval("FromTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="UPTO"><ItemTemplate> <%# LEAVEUPTO(Eval("TypeID").ToString(), Eval("UptoDate").ToString(), Eval("UptoTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="REQUEST DATE"><ItemTemplate> <%# FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate><HeaderStyle Width="100px"/></asp:TemplateField>
                <asp:BoundField HeaderText="CONTACT NO" DataField="cNumber"/>
                <asp:TemplateField HeaderText="STATUS"><ItemTemplate> <b><%# Eval("LeaveStatusName").ToString()%></b></ItemTemplate></asp:TemplateField>
            
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

