<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="LeaveEdit.aspx.cs" Inherits="snehrehab.Member.LeaveEdit" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Approve/Reject Leave :</div>
            <div class="pull-right">
                <a href="/Member/LeaveAll.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Full Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Leave Types :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLeaveTypes" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Leave From :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLeaveFrom" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Leave Upto :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtLeaveUpto" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        Reason :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="span10" TextMode="MultiLine"
                            Rows="2" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Leave Action :</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtAction" runat="server" CssClass="chzn-select span3">
                            <asp:ListItem Value="0">Select Status</asp:ListItem>
                            <asp:ListItem Value="1">Approve</asp:ListItem>
                            <asp:ListItem Value="2">Reject</asp:ListItem>
                        </asp:DropDownList>
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
                        &nbsp; <a href="/Member/LeaveAll.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
