<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_UserAcc" Title="" Codebehind="UserAcc.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Create Account :</div>
            <div class="pull-right">
                <a href="/Member/UserAccs.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Account Type :</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAccountType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtAccountType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtAccountType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <div id="tb_Therapist" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Select Therapist :</label>
                                <div class="control-group">
                                    <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select span4" 
                                        AutoPostBack="True" onselectedindexchanged="txtTherapist_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Full Name :</label>
                    <div class="control-group"><asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="span4"></asp:TextBox> </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Mobile No :</label>
                    <div class="control-group"><asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="span4"></asp:TextBox> </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
             <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        E-Mail ID :</label>
                    <div class="control-group"><asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtMail" runat="server" CssClass="span4"></asp:TextBox> </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
              <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Login Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLoginName" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div><div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Password :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="span4"></asp:TextBox>
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
                        &nbsp;
                        <a href="/Member/UserAccs.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
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
