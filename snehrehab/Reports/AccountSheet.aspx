<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AccountSheet.aspx.cs" Inherits="snehrehab.Reports.AccountSheet" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Other Account Sheet :</div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
           <div class="formRow">
                <div style="float:left;">
                <div class="span4" style="margin:0px;">
                <asp:DropDownList ID="txtDoctors" runat="server" CssClass="chzn-select span4"></asp:DropDownList>
                </div>
                <div class="span2" style="width: 100px;margin: 0px;">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:Literal ID="txtContent" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
