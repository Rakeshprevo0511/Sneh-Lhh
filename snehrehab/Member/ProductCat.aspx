<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ProductCat.aspx.cs" Inherits="snehrehab.Member.ProductCat" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdateCats" runat="server"><ContentTemplate>
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Product Categories :
            </div>
            <div class="pull-right">
                <a href="/Member/Product.aspx" class="btn btn-primary">Add Product</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtCategory" runat="server" placeholder="Category Name"></asp:TextBox>
                &nbsp;
                <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger" onclick="btnSave_Click">Add New</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-default" 
                    Visible="false" onclick="btnCancel_Click">Cancel</asp:LinkButton>
                <asp:HiddenField ID="txtHidden" runat="server" />
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="CategoryGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="CategoryGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <PagerStyle CssClass="custome-pagination"></PagerStyle>
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="50px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="CATEGORY NAME"><ItemTemplate><%#Eval("Category").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("CategoryID") %>' onclick="btnEdit_Click">Edit</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("CategoryID") %>' onclick="btnDelete_Click" OnClientClick="return confirm('Are you sure to delete category..??');">Delete</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </ContentTemplate></asp:UpdatePanel>
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
