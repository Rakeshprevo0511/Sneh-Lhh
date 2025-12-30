<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Supporte" Title="" Codebehind="Supporte.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Support Ticket :</div>
            <div class="pull-right">
                <a href="/Member/Supports.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        Message:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="span7" TextMode="MultiLine" Rows="5" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        Attachment :</label>
                    <div class="control-group">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        Remark:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="span7" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        Status:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtStatus" runat="server" CssClass="span4">
                        <asp:ListItem Value="-1">Select Status</asp:ListItem>
                        <asp:ListItem Value="0">Pending</asp:ListItem>
                        <asp:ListItem Value="1">Solved</asp:ListItem>
                        <asp:ListItem Value="2">Not Solved</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <hr />
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-danger" OnClientClick="DisableOnSubmit(this);" onclick="btnSubmit_Click"></asp:LinkButton>
                        &nbsp;
                        <a href="/Member/Supports.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>

