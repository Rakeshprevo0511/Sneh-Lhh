<%@ Page Language="C#"  MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Ndt_view_2025_other.aspx.cs" Inherits="snehrehab.Reports.Ndt_view_2025_other" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.btn-print{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-print:hover{text-decoration: none;}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Search NDT 2025 Report :
            </div>
            <div class="pull-right">
                <a href="/Reports/ReportStatus.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
               
                <div class="span2" style="width: 100px;margin: 0px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="margin-left: 15px;">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="span2" placeholder="Patient Name"></asp:TextBox>
                </div>
                <div style="float:left;margin-top: 4px;margin-left: 15px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                </div>
                  <div style="float:left;margin-top: 4px; margin-left: 15px;">
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
                
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ReportGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:TemplateField HeaderText="THERAPIST"><ItemTemplate><%#Eval("Therapist").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="MAIL SEND"><ItemTemplate><%#Eval("MailSend").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%#GetAction(Eval("UniqueID").ToString())%>
                    <%# SessionDownloadViewLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>
                    <%# SessionViewLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>
                    <%# GetMailink(Eval("UniqueID").ToString())%>
                    <%--<%# SnehBLL.Appointments_Bll.SessionRptLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>--%>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="120px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
