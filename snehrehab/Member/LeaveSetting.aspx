<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="LeaveSetting.aspx.cs" Inherits="snehrehab.Member.LeaveSetting" %>
<%@ MasterType VirtualPath="~/Member/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Leave Alert Setting :</div>
        </div>
        <div class="grid-content"> 
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Mobile Nos.:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMobileNos" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        <p class="info right" style="color: #f10b0b;">Multiple mobile number should be seperated by comma (,)</p>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>  
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Email Ids:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtEmailIds" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        <p class="info right" style="color: #f10b0b;">Multiple email address should be seperated by comma (,)</p>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Approved Mail To:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLeaveAMailTo" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        <p class="info right" style="color: #f10b0b;">Multiple email address should be seperated by comma (,)</p>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Approved Mail Bcc:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLeaveAMailBcc" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        <p class="info right" style="color: #f10b0b;">Multiple email address should be seperated by comma (,)</p>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Email Footer:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtEmailFooter" runat="server" CssClass="span4" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        <p class="info right" style="color: #f10b0b;">Email footer will be appear in leave email this can contains<br /> address, contact no. etc..</p>
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
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-danger" OnClientClick="DisableOnSubmit(this);" onclick="btnSubmit_Click"></asp:LinkButton>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div> 
</asp:Content>
