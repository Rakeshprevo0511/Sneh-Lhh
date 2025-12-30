<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="SettingContact.aspx.cs" Inherits="snehrehab.Member.SettingContact" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Closing Contacts :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Mobile No :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="3"></asp:TextBox>
                    </div>
                    <div class="clearfix">
                    </div>
                    <hr />
                    <label class="control-label hidden-phone">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnMobileNo" runat="server" CssClass="btn btn-danger" OnClientClick="DisableOnSubmit(this);"
                            OnClick="btnMobileNo_Click">Add / Update</asp:LinkButton>
                    </div>
                    <div class="clearfix">
                    </div>
                    <br />
                </div>
                <div class="span6">
                    <label class="control-label">
                        Mail Address :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMailID" runat="server" CssClass="span4" TextMode="MultiLine"
                            Rows="3"></asp:TextBox>
                    </div>
                    <div class="clearfix">
                    </div>
                    <hr />
                    <label class="control-label hidden-phone">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnMailID" runat="server" CssClass="btn btn-danger" OnClientClick="DisableOnSubmit(this);"
                            OnClick="btnMailID_Click">Add / Update</asp:LinkButton>
                    </div>
                    <div class="clearfix">
                    </div>
                    <br />
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <b style="color: Red;">Note : - </b>&nbsp;&nbsp; You can enter multiple contacts
                    seperated by comma.
                </div>
                <div class="clearfix">
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
