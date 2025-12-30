<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Productd.aspx.cs" Inherits="snehrehab.Member.Productd" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Delete Product :
            </div>
        </div>
        <div class="grid-content">
            <div class="alert alert-warning">
                Deleting product entry will be permanant.<br />
                Are you sure to Delete..??
                <div class="clearfix"></div>
                <br />
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger" onclick="btnDelete_Click">Delete</asp:LinkButton>
                &nbsp;
                <a href="/Member/Products.aspx" class="btn btn-default">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
