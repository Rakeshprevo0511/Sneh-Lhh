<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="NoAuth.aspx.cs" Inherits="snehrehab.Member.NoAuth" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<meta http-equiv="refresh" content="5;URL=/Member/">
<style type="text/css">
.grid-content img{width:8em;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                No Authentication</div>
            <div class="pull-right">
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="grid-content">
            <br />
            <br />
            <div class="formRow">
                <div class="span1">
                    <img src="/images/user-no-auth.png" alt="" />
                </div>
                <div class="span8">
                    <p>
                        <b>Dear User,</b><br />
                        We could not complete your request.<br />
                        You do not have access permission for reuested page.
                    </p>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
