<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_CashEntry" Title="" Codebehind="CashEntry.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Other Cash Entry :</div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Account Type:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateAccountType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtAccountType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtAccountType_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">Select Account Type</asp:ListItem>
                                    <%--<asp:ListItem Value="1">Account Head</asp:ListItem>--%>
                                    <asp:ListItem Value="2">Doctor Head</asp:ListItem>
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
                        <asp:UpdatePanel ID="UpdateAccountName" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtAccountName" runat="server" CssClass="chzn-select span4">
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
                        Product Category:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtProductCategory" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtProductCategory_SelectedIndexChanged">
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
                        Product Name:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtProductName" runat="server" CssClass="chzn-select span4">
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
                        Amount:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="span2"></asp:TextBox>
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
                        <asp:TextBox ID="txtPayDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        <asp:UpdatePanel ID="UpdatePaymentType" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtPaymentType" runat="server" CssClass="chzn-select span4"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="txtPaymentType_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Cash</asp:ListItem>
                                    <%--<asp:ListItem Value="2">Credit</asp:ListItem>--%>
                                    <asp:ListItem Value="3">Cheque</asp:ListItem>
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
                                    <asp:DropDownList ID="txtBankName" runat="server" CssClass="chzn-select span4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="span6">
                                <label class="control-label">
                                    Cheque Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        <asp:TextBox ID="txtNarration" runat="server" CssClass="span4" TextMode="MultiLine" Rows="2"></asp:TextBox>
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
