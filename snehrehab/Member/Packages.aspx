<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Packages.aspx.cs" Inherits="snehrehab.Member.Packages" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Package List :
            </div>
            <div class="pull-right" id="btn_add_package" runat="server">
                <a href="/Member/Package.aspx" class="btn btn-primary">Add New</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtSession" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Package Name"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="PackageGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="PackageGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="CODE NO"><ItemTemplate> <%#Eval("PackageCode")%></ItemTemplate><HeaderStyle CssClass="nowrap" /><ItemStyle CssClass="nowrap" /></asp:TemplateField>
                <asp:BoundField HeaderText="DESCRIPTION" DataField="cDescription"/>
                <asp:BoundField HeaderText="PATIENT TYPE" DataField="PatientType" HeaderStyle-CssClass="nowrap" ItemStyle-CssClass="nowrap"/>
                <asp:BoundField HeaderText="CATEGORY" DataField="CategoryName"/>
                <asp:BoundField HeaderText="PACKAGE AMT" DataField="PackageAmt" HeaderStyle-CssClass="nowrap" ItemStyle-CssClass="nowrap"/>
                <asp:BoundField HeaderText="SESSION AMT" DataField="OneTimeAmt" HeaderStyle-CssClass="nowrap" ItemStyle-CssClass="nowrap"/>
                <asp:BoundField HeaderText="APPOINTMENTS" DataField="Appointments"/>
                <asp:BoundField HeaderText="DURATION" DataField="MaximumTime"/>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <a href='/Member/Package.aspx?record=<%#Eval("UniqueID") %>'>Edit</a>
                    &nbsp;
                    <%--<a href='/Member/Packaged.aspx?record=<%#Eval("UniqueID") %>'>Delete</a>--%>
                    <asp:LinkButton ID="btnToggle" runat="server" CommandArgument='<%#Eval("PackageID") %>' OnClick="btnToggle_Click"><%#GetToggleText(Eval("IsEnabled").ToString())%></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="80px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
