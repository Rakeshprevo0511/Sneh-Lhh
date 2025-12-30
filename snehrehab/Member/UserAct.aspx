<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="UserAct.aspx.cs" Inherits="snehrehab.Member.UserAct" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                My Activities :</div>
            <div class="pull-right">
                
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <div class="span2" style="width: 100px;margin: 0px;">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br /><br />
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px" /></asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate><HeaderStyle Width="20%"/></asp:TemplateField>
                    <asp:TemplateField HeaderText="LOGIN ID"><ItemTemplate><%#Eval("LoginName")%></ItemTemplate><HeaderStyle Width="13%"/></asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# DbHelper.Configuration.FORMATLONGDATE(Eval("aDate").ToString())%></ItemTemplate><HeaderStyle Width="125px"/></asp:TemplateField>
                    <asp:TemplateField HeaderText="DESCRIPTION"><ItemTemplate><%#Eval("Remark").ToString()%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
