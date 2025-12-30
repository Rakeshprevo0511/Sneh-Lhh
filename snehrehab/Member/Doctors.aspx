<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Doctors" Title="" Codebehind="Doctors.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Doctor List :
            </div>
             <div class="pull-right">
                <asp:Literal ID="lblAddNew" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Doctor Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="Join From" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Join Upto" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="DoctorGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="DoctorGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate> <%#Eval("PreFix").ToString() + " "+ Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="QUALIFICATION" DataField="Qualification"/>
                <asp:TemplateField HeaderText="SPECIALITY"><ItemTemplate> <%# SPECIALITY(Eval("SpecialityID").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="MOBILE" DataField="MobileNo"/>
                <asp:TemplateField HeaderText="WORKPLACE"><ItemTemplate> <%# WORKPLACE(Eval("WorkplaceID").ToString(), Eval("WorkplaceOther").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CLINIC ADDRESS"><ItemTemplate> <%#Eval("ClinicAddress")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate> 
                     <%#GetAction(Eval("UniqueID").ToString())%> 
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="70px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

