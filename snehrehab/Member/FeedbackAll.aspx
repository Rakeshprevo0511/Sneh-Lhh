<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="FeedbackAll.aspx.cs" Inherits="snehrehab.Member.FeedbackAll" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Feedback List :
            </div>
            <div class="pull-right">
                <a href="/Member/Feedback.aspx" class="btn btn-primary">Add New</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Type to search"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="Join From" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Join Upto" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="FeedbackGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="FeedbackGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="USER ID"><ItemTemplate> <%#Eval("LoginName")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate> <%#Eval("FullName")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="FEEDBACK TO"><ItemTemplate> <%#FEEDBACKTO(Eval("ToID").ToString(), Eval("OtherToID").ToString(), Eval("ToName").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ISSUE TYPE"><ItemTemplate> <%#ISSUETYPE(Eval("TypeID").ToString(), Eval("OtherTypeID").ToString(), Eval("TypeName").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="MESSAGE"><ItemTemplate> <%# Eval("cMessage")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate> <%# DbHelper.Configuration.FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
