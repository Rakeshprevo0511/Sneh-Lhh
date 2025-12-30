<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Patient_SiView.aspx.cs" Inherits="snehrehab.Reports.Patient_SiView" %>
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
                Search SI Report :
            </div>
            <div class="pull-right">
                <a href="/Reports/" class="btn btn-primary">Back</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
             <div style="float:left;">
                    <%--<div class="span3" style="margin:0px;">
                        <asp:DropDownList ID="txtDoctor" runat="server" CssClass="chzn-select span3">
                        </asp:DropDownList>
                    </div>--%>
                </div>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name"></asp:TextBox>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker" placeholder="From Date" Width="100px"></asp:TextBox>
                <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker" placeholder="Upto Date" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
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
                <%--<asp:BoundField HeaderText="STATUS" DataField="IsFilled" />--%>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                    <%#GetAction(Eval("UniqueID").ToString())%>
                    <%# SessionRptLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="95px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
