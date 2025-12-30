<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_DoctorCrDr" Title="" Codebehind="DoctorCrDr.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                All Transaction :</div>
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
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="30" AllowPaging="true" OnDataBound="ReportGV_DataBound">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="ACCOUNT"><ItemTemplate><%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:BoundField HeaderText="LEDGER HEAD" DataField="LedgerHead" />
                    <asp:BoundField HeaderText="DESCRIPTION" DataField="cDescription" />
                    <asp:TemplateField HeaderText="CREDIT"><ItemTemplate><%#Eval("CreditAmt")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="DEBIT"><ItemTemplate><%#Eval("DebitAmt")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>