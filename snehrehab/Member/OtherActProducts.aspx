<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="OtherActProducts.aspx.cs" Inherits="snehrehab.Member.OtherActProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Other Activity Product List :
            </div>
            <div class="pull-right" id="btn_add_product" runat="server">
                <a href="/Member/OtherActProduct.aspx" class="btn btn-primary">Add Product</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtOtherActCategory" runat="server" CssClass="input-medium">             
                </asp:DropDownList>
                <asp:TextBox ID="txtOtherActSearch" runat="server" placeholder="Product Name"></asp:TextBox>
                <asp:LinkButton ID="btnOtherActSearch" runat="server" CssClass="btn btn-default" onclick="btnOtherActSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ProductOtherActGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ProductOtherActGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="PRODUCT NAME"><ItemTemplate><%#Eval("ProductName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CATEGORY"><ItemTemplate><%#Eval("CategoryName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <a href='/Member/OtherActProduct.aspx?record=<%#Eval("UniqueID") %>'>Edit</a>
                    &nbsp;
                    <a href='/Member/OtherActProductd.aspx?record=<%#Eval("UniqueID") %>'>Delete</a>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
