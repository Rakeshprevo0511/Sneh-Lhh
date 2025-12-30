<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AppChngeRequestC.aspx.cs" Inherits="snehrehab.Member.AppChngeRequestC" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Reject Request :
            </div>
        </div>
        <div class="grid-content">
            <div class="alert alert-info">
                Rejecting doctor change request will be permanant.<br />
                Are you sure to Reject..??
                <div class="clearfix"></div>
                <br />
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger" onclick="btnDelete_Click">Reject</asp:LinkButton>
                &nbsp;
                <a href="/Member/AppChngeRequest.aspx" class="btn btn-default">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
