<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="CashEntries.aspx.cs" Inherits="snehrehab.Reports.CashEntries" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Other Cash Entry List :
            </div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <asp:UpdatePanel ID="UpdateCashEntries" runat="server"><ContentTemplate>
                <div class="span3" style="margin:0px;">
                 <asp:DropDownList ID="txtAccountType" runat="server" CssClass="chzn-select span3" AutoPostBack="true" onselectedindexchanged="txtAccountType_SelectedIndexChanged">
                    <asp:ListItem Value="-1">All Account Type</asp:ListItem>
                    <%--<asp:ListItem Value="1">Account Head</asp:ListItem>--%>
                    <asp:ListItem Value="2">Doctor Head</asp:ListItem>
                    <asp:ListItem Value="3">Patient Head</asp:ListItem>
                </asp:DropDownList>
                </div>
                <div class="span3" style="margin: 0px;">
                <asp:DropDownList ID="txtAccountName" runat="server" CssClass="chzn-select span3">             
                </asp:DropDownList>
                </div>
                <div class="span2" style="width: 100px;margin: 0px;">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="span2" placeholder="Product Name"></asp:TextBox>
                </div>
                </ContentTemplate></asp:UpdatePanel>
                </div>
                <div style="float:left;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
             <div class="clearfix">
            </div>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <asp:GridView ID="CashEntryGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="CashEntryGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="ACCOUNT TYPE"><ItemTemplate><%#Eval("AccountType")%></ItemTemplate><HeaderStyle Width="100px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="ACCOUNT NAME"><ItemTemplate><%#Eval("AccountName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PRODUCT"><ItemTemplate><%#Eval("ProductName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PAY MODE"><ItemTemplate><%#Eval("PayModeValue").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="NARRATION"><ItemTemplate><%#Eval("Narration").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="AMOUNT"><ItemTemplate><%#Eval("DebitAmt")%></ItemTemplate><HeaderStyle Width="75px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("PayDate").ToString())%></ItemTemplate><HeaderStyle Width="85px"/></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
