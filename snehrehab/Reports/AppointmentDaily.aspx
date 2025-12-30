<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="AppointmentDaily.aspx.cs" Inherits="snehrehab.Reports.AppointmentDaily" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.report-gv-top-align th{vertical-align:top !important;}
.report-gv-top-align td{vertical-align:top !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Appointment Sheet :</div>
            <div class="pull-right">
                <a href="/Reports/Account.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                <asp:TextBox ID="txtDate" runat="server" CssClass="my-datepicker span2" placeholder="Select Date" Width="100px"></asp:TextBox>
                </div>
                </div>
                <div style="float:left;margin-top: 4px;">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click">Search</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <br />
            <div style="white-space: nowrap;overflow-x:auto;">
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered report-gv-top-align" PagerStyle-CssClass="custome-pagination"
                AutoGenerateColumns="false" OnPageIndexChanging="ReportGV_PageIndexChanging"
                PageSize="51" AllowPaging="false" OnDataBound="ReportGV_DataBound">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#FORMATTIME(Eval("TimeHourNew").ToString())%></ItemTemplate><HeaderStyle Width="60px" /></asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
