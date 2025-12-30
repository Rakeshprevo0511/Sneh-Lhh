<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Banks.aspx.cs" Inherits="snehrehab.Member.Banks" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Bank Master :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow" id="add_bank" runat="server">
                <label id="lblBankName" runat="server" style="text-align: center;">Add New Bank</label>
                <asp:TextBox ID="txtBankName" runat="server"></asp:TextBox>
                <asp:LinkButton ID="btnAddNew" runat="server" CssClass="btn btn-danger" OnClick="btnAddNew_Click">Submit</asp:LinkButton>
                <a href="/Member/Banks.aspx" class="btn btn-default" id="btnCancel" runat="server" visible="false">Cancel</a>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:GridView ID="BanksGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="BanksGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="BANK NAME"><ItemTemplate><%# Eval("BankName").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTION">
                        <ItemTemplate>
                            <a href='/Member/Banks.aspx?record=<%# Eval("UniqueID") %>'>Edit</a>
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center" />
                        <HeaderStyle CssClass="text-center" Width="55px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
