<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="DrYesterdayAppointMy.aspx.cs" Inherits="snehrehab.Member.DrYesterdayAppointMy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Dr Yesterday's Appointment :
            </div>            
        </div>
        <div class="grid-content">
            <div class="formRow">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <asp:GridView ID="TicketGV" runat="server" CssClass="table table-bordered table-responsive" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="TicketGV_PageIndexChanging"
                PageSize="30" AllowPaging="true">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px" /></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="TICKET"><ItemTemplate><%#Eval("TicketCode")%></ItemTemplate><HeaderStyle Width="80px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="NARRATION"><ItemTemplate><%#Eval("yNarration")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="DATE"><ItemTemplate><%#FORMATDATE(Eval("AddedDate").ToString())%></ItemTemplate><HeaderStyle Width="80px" /></asp:TemplateField>
                    <asp:TemplateField HeaderText="STATUS"><ItemTemplate><%#Eval("yStatusName")%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="REMARK"><ItemTemplate><%#Eval("yRemark")%></ItemTemplate></asp:TemplateField>                    
                    <%--<asp:TemplateField HeaderText="ACTION"  Visible="false"><ItemTemplate><a href='/Member/DrYesterdayAppointEdit.aspx?record=<%#Eval("UniqueID")%>'>Edit</a></ItemTemplate><HeaderStyle Width="50px" /><ItemStyle CssClass="text-center" /></asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="ACTION" Visible="false"><ItemTemplate><%#GETACTION(Eval("UniqueID").ToString())%></ItemTemplate><HeaderStyle Width="50px" /><ItemStyle CssClass="text-center" /></asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>


