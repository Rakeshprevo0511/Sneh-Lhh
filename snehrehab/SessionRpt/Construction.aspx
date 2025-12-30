<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Construction.aspx.cs" Inherits="snehrehab.SessionRpt.Construction" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Search <%= _rptName %> Report :
            </div>
            <div class="pull-right">
                <a href="/SessionRpt/" class="btn btn-primary">View Reports</a>
            </div>
        </div>
        <div class="grid-content">
             <div style="text-align:center;">
                <img src="/images/underconstruction.png" style="max-width:100%;"/>
             </div>
        </div>
    </div>
</asp:Content>
