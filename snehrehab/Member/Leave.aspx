<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Leave" Title="" Codebehind="Leave.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                New Leave Application :</div>
            <div class="pull-right">
                <a href="/Member/Leaves.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Leave Types:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateLeaveTypes" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="txtLeaveTypes" runat="server" CssClass="chzn-select span3"
                                    AutoPostBack="True" OnSelectedIndexChanged="txtLeaveTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:UpdatePanel ID="UpdateLeaveDays" runat="server">
                <ContentTemplate>
                    <div id="tb_FullDay" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Balance Leaves:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtBalance" runat="server" CssClass="span2" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    From Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Upto Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtUptoDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </div>
                    <div id="tb_HalfDay" runat="server" visible="false">
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Leave Date:</label>
                                <div class="control-group">
                                    <asp:TextBox ID="txtLeaveDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    From Time:</label>
                                <div class="control-group">
                                    <asp:DropDownList ID="txtFromTime" runat="server" CssClass="span2 chzn-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="formRow">
                            <div class="span6">
                                <label class="control-label">
                                    Upto Time:</label>
                                <div class="control-group">
                                    <asp:DropDownList ID="txtUptoTime" runat="server" CssClass="span2 chzn-select"></asp:DropDownList>
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
                        Reason For Leave:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="2"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Contact Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="2"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Contact Number:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="span4" TextMode="MultiLine"
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
                        &nbsp; <a href="/Member/Leaves.aspx" class="btn btn-default">Cancel</a>
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
