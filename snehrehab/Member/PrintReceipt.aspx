<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="PrintReceipt.aspx.cs" Inherits="snehrehab.Member.PrintReceipt" %>
<%@ MasterType VirtualPath="~/Member/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Print Receipt :
            </div> 
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>                
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" onclick="btnExport_Click">Export</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <div class="alert alert-info">
            <strong>Total Receipt Amount : 
                <asp:Label ID="lblTotal" runat="server" ></asp:Label>/- INR</strong>
            </div>
            <asp:GridView ID="BookingGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="BookingGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="RECEIPT NO."><ItemTemplate><%# GetReceipt(Eval("ReceiptPrefix").ToString(),Eval("ReceiptNo").ToString()) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="AMOUNT"><ItemTemplate><%#Eval("NetAmt")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="RECEIPT DATE"><ItemTemplate><%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CHQ./ONLINE NO."><ItemTemplate><%# GetTxnNo(Eval("ReceiptType").ToString(),Eval("TableID").ToString(),Eval("FiscalDate").ToString()) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TRANSACTION DATE"><ItemTemplate><%#  FORMATDATE(Eval("TransactionDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="BOOKING DATE"><ItemTemplate><%# FORMATLDATE(Eval("AddedDate").ToString())%></ItemTemplate></asp:TemplateField>                   
                <asp:BoundField HeaderText="REMARK" DataField="Remarks"/> 
                <asp:BoundField HeaderText="SEND MAIL" DataField="email_flag"/> 
                
                    <asp:TemplateField HeaderText="ACTION"><ItemTemplate>                    
                    <a href='/Member/PrintReceipt.ashx?id=<%#GetUrlQuery(Eval("UniqueID").ToString(), Eval("ReceiptType").ToString(), Eval("FiscalDate").ToString(), Eval("ReceiptNo").ToString()) %>' target="_blank">Print /</a>
                    &nbsp;
                    <a href='/Member/SendMail.aspx?pid=<%#Eval("PatientID").ToString() %>&id=<%# GetUrlQuery(Eval("UniqueID").ToString(), Eval("ReceiptType").ToString(), Eval("FiscalDate").ToString(), Eval("ReceiptNo").ToString()) %>' target="_blank">Send Mail</a>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="50px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
