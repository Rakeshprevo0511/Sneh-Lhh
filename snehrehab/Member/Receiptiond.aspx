<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Receiptiond.aspx.cs" Inherits="snehrehab.Member.Receiptiond" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Delete Receiptionist :
            </div>
        </div>
        <div class="grid-content">
            <div class="alert alert-warning">
                Deleting receiptionist entry will be permanant.<br />
                Are you sure to Delete..??
                <div class="clearfix"></div>
                <br />
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger" onclick="btnDelete_Click">Delete</asp:LinkButton>
                &nbsp;
                <a href="/Member/ViewList.aspx" class="btn btn-default">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
