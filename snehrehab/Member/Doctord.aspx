<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Doctord" Title="" Codebehind="Doctord.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Delete Doctor :
            </div>
        </div>
        <div class="grid-content">
            <div class="alert alert-warning">
                Deleting doctor entry will be permanant.<br />
                Are you sure to Delete..??
                <div class="clearfix"></div>
                <br />
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger" onclick="btnDelete_Click">Delete</asp:LinkButton>
                &nbsp;
                <a href="/Member/Doctors.aspx" class="btn btn-default">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>

