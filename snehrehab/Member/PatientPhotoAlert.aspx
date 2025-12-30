<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="PatientPhotoAlert.aspx.cs" Inherits="snehrehab.Member.PatientPhotoAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Pending Photo List :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:DropDownList ID="txtPatientType" runat="server" CssClass="input-medium">
                </asp:DropDownList>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="PatientGV" runat="server" CssClass="table table-bordered" PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="PatientGV_PageIndexChanging" PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate> <%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
               <asp:TemplateField HeaderText="PATIENT CODE"><ItemTemplate> <%# Eval("PatientCode").ToString()%></ItemTemplate><HeaderStyle Width="100px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate> <%# Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="TELEPHONE" DataField="TelephoneNo"/>
                <asp:TemplateField HeaderText="BIRTH DATE"><ItemTemplate> <%# FORMATDATE(Eval("BirthDate").ToString())%></ItemTemplate><HeaderStyle Width="85px"/></asp:TemplateField>
                <asp:BoundField HeaderText="PURPOSE" DataField="VisitPurpose"/>
                <asp:TemplateField HeaderText="REG DATE"><ItemTemplate> <%# FORMATDATE(Eval("RegistrationDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="REFERRED BY"><ItemTemplate> <%# Eval("ReferredBy").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="CONSULTED BY"><ItemTemplate> <%#Eval("ConsultedBy")%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%#GetAction(Eval("UniqueID").ToString())%>                    
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="50px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
