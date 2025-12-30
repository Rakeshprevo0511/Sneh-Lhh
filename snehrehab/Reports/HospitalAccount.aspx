<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Reports_HospitalAccount" Title="" Codebehind="HospitalAccount.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Hospital Account Report :</div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date"
                    Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date"
                    Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
            </div>
            <div class="clearfix">
            </div>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="30" AllowPaging="true" OnDataBound="ReportGV_DataBound">
                <EmptyDataTemplate>
                    No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %></ItemTemplate>
                        <HeaderStyle Width="40px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACCOUNT">
                        <ItemTemplate>
                            <%#Eval("AccountName").ToString()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="LEDGER HEAD" DataField="LedgerHead" />
                    <asp:BoundField HeaderText="DESCRIPTION" DataField="cDescription" />
                    <asp:TemplateField HeaderText="CREDIT">
                        <ItemTemplate>
                            <%#Eval("CreditAmt")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DEDIT">
                        <ItemTemplate>
                            <%#Eval("DebitAmt")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DATE">
                        <ItemTemplate>
                            <%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

