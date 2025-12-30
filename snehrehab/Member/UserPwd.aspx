<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_UserPwd" Title="" Codebehind="UserPwd.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                My Password :</div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span style="color:Red;font-weight:bold">*</span> Old Password :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtOld" runat="server" TextMode="Password" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span style="color:Red;font-weight:bold">*</span> New Password :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtNew" runat="server" TextMode="Password" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                       <span style="color:Red;font-weight:bold">*</span> Confirm Password :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <hr />
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-danger" OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>

