<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="FeedBack.aspx.cs" Inherits="snehrehab.Member.FeedBack" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Feedback :</div>
            <div class="pull-right">
                <a href="/Member/Feedbacks.aspx" class="btn btn-primary">My Feedback</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Feedback To:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAccountType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtFeedbackTo" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtFeedbackTo_SelectedIndexChanged"> 
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="formRow" id="tab_FeedbackTo" runat="server">
                        <div class="span6">
                            <label class="control-label">
                                &nbsp;</label>
                            <div class="control-group">
                                <asp:TextBox ID="txtFeedbackToOther" runat="server" CssClass="span4" placeholder="Feedback To"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Issue Type:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAccountName" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtIssueType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtIssueType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="formRow" id="tab_IssueType" runat="server">
                        <div class="span6">
                            <label class="control-label">
                                &nbsp;</label>
                            <div class="control-group">
                                <asp:TextBox ID="txtIssueTypeOther" runat="server" CssClass="span4" placeholder="Issue Type"></asp:TextBox>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Message:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="span4" TextMode="MultiLine" Rows="5"></asp:TextBox>
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
