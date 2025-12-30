<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="snehrehab.Member.Products" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Product List :
            </div>
            <div class="pull-right" id="btn_add_product" runat="server">
                <a href="/Member/Product.aspx" class="btn btn-primary">Add Product</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtCategory" runat="server" CssClass="input-medium">             
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Product Name"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ProductGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ProductGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="PRODUCT NAME"><ItemTemplate><%#Eval("ProductName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CATEGORY"><ItemTemplate><%#Eval("Category").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <a href='/Member/Product.aspx?record=<%#Eval("UniqueID") %>'>Edit</a>
                    &nbsp;
                    <a href='/Member/Productd.aspx?record=<%#Eval("UniqueID") %>'>Delete</a>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
