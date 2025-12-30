<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="OtherActivity.aspx.cs" Inherits="snehrehab.Member.OtherActivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Other Activity :</div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Account Type:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateOtherActAccountType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherActAccountType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtOtherActAccountType_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">Select Account Type</asp:ListItem>
                                    <%--<asp:ListItem Value="1">Account Head</asp:ListItem>--%>
                                    <%--<asp:ListItem Value="2">Doctor Head</asp:ListItem>--%>
                                    <asp:ListItem Value="3">Patient Head</asp:ListItem>
                                    
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Account Name:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateOtherActAccountName" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherActAccountName" runat="server" CssClass="chzn-select span4">
                                </asp:DropDownList>
                                <asp:TextBox ID="textOtherAct_AccountName" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Product Category:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherActProductCategory" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtOtherActProductCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="textOtherAct_ProductCategory" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Product Name:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherActProductName" runat="server" CssClass="chzn-select span4"
                                Autopostback="True" OnSelectedIndexChanged="txtOtherActProductName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="textOtherAct_ProductName" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Doctor:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherAct_Doctor" runat="server" CssClass="chzn-select span4">
                                </asp:DropDownList>
                                <asp:TextBox ID="textOtherAct_Doctor" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Assistant Doctor:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherAct_Ass_Doctor" runat="server" CssClass="chzn-select span4">
                                </asp:DropDownList>
                                <asp:TextBox ID="textOtherAct_Ass_Doctor" runat="server" CssClass="span4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Amount:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtOtherActAmount" runat="server" CssClass="span2"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Pay Date:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtOtherActPayDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Payment Type:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateOtherActPaymentType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtOtherActPaymentType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtOtherActPaymentType_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Cash</asp:ListItem>
                                    <%--<asp:ListItem Value="2">Credit</asp:ListItem>--%>
                                    <asp:ListItem Value="3">Cheque</asp:ListItem>
                                    <asp:ListItem Value="4">Online</asp:ListItem>
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
                    <div id="tb_SessionBank" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Bank Name:</label>
                                <div class="control-group">
                                    <asp:DropDownList ID="txtOtherActBankName" runat="server" CssClass="chzn-select span4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Branch Name:</label>
                                <div class="control-group">
                                     <asp:TextBox ID="txtOtherActBranchName" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="span6">
                                <label class="control-label">
                                    Cheque No.:</label>
                                <div class="control-group">
                                     <asp:TextBox ID="txtOtherActChequeNumber" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Cheque Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtOtherActChequeDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="tab_online" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Transaction ID:
                                    </label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtOtherActTransactionID" runat="server" CssClass="span4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Transaction Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="TextOthActTran_Date" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        Narration:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtOtherActNarration" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="2"></asp:TextBox>
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
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-danger"
                            OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
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
