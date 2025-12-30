<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Package.aspx.cs" Inherits="snehrehab.Member.Package" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.checkboes{margin: 0px;max-height: 86px;overflow-x: auto;border: 1px solid #cccccc;padding: 5px;}
.checkboes input[type="checkbox"]{float: none;margin: 0px;}
.checkboes label{float: none;padding: 0px;width: auto;height: auto;display: inline-block;margin: 0px;line-height: normal;margin-bottom: 5px;margin-left: 10px;padding-top: 4px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
    <div class="grid-title">
        <div class="pull-left">
            <%= _headerText %> :
        </div>
    </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Package Code:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPackageCode" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Description:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtcDescription" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Patient Type:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtPatientType" runat="server" CssClass="chzn-select span4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Category:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtCategory" runat="server" CssClass="chzn-select span4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Package Amt:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPackageAmt" runat="server" CssClass="span2"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Session Amt:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSessionAmt" runat="server" CssClass="span2"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Appointments:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAppointments" runat="server" CssClass="span2"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Validity:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtValidity" runat="server" CssClass="span2"></asp:TextBox> <label style="  margin-top: 5px;margin-left: 10px;">(In Days)</label>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Time Duration:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtTimeDuration" runat="server" CssClass="chzn-select span2">
                        <asp:ListItem Value="0">No Limit</asp:ListItem>
                        <asp:ListItem Value="15">15 Min</asp:ListItem>
                        <asp:ListItem Value="30">30 Min</asp:ListItem>
                        <asp:ListItem Value="45">45 Min</asp:ListItem>
                        <asp:ListItem Value="60">60 Min</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        For Session:</label>
                    <div class="control-group">
                        <div class="span4 checkboes" style="margin: 0px;">
                            <asp:CheckBoxList ID="txtToSession" runat="server" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="formRow">
                <div class="span12">
                    <hr />
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="formRow">
                <div class="span12">
                    <label class="control-label">
                    </label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger" OnClick="btnSave_Click" OnClientClick="DisableOnSubmit(this);">Save</asp:LinkButton>
                        &nbsp; 
                        <a href="/Member/Packages.aspx" class="btn btn-default">Cancel</a>
                        &nbsp;
                        <asp:LinkButton ID="btnRevise" runat="server" CssClass="btn btn-danger pull-right" Visible="false" OnClick="btnRevise_Click" OnClientClick="if(confirm('Old package will be disable and new package will be added.\nAre you sure to revise package.?')) { DisableOnSubmit(this); return true; }else { return false; }">Revise Package</asp:LinkButton>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
