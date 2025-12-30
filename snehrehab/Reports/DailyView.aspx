<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="DailyView.aspx.cs" Inherits="snehrehab.Reports.DailyView" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.btn-print{padding: 2px 8px;border-radius: 3px;margin: 0 3px;}
.btn-print:hover{text-decoration: none;}
.table-bordered th{white-space:nowrap;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Search Daily Report :
            </div>
            <div class="pull-right">
                <a href="/Reports/ReportStatus.aspx" class="btn btn-primary">Report List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div style="float:left;">
                    <div class="span3" style="margin:0px;">
                        <asp:DropDownList ID="txtDoctor" runat="server" CssClass="chzn-select span3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="float:left;">
                    <div class="span3" style="margin:0px;">
                        <asp:DropDownList ID="txtstatus" runat="server" CssClass="chzn-select span3" >
                        <asp:ListItem Value="-1">Select Status</asp:ListItem>
                        <asp:ListItem Value="1">Complete</asp:ListItem>
                        <asp:ListItem Value="2">Pending</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="span2" style="width: 100px;margin: 0px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="width: 100px;margin-left: 15px;margin-right:15px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="span2" style="margin-left: 15px;">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="span2" placeholder="Patient Name"></asp:TextBox>
                </div>
                <div style="float:left;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                </div>
                 <div style="float:left;margin-top: 4px;">
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
            <div class="table-responsive" style="/*overflow-x:auto;*/">
            <asp:GridView ID="ReportGV" runat="server" CssClass="table table-bordered" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="ReportGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" >
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="PATIENT NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <%--<asp:TemplateField HeaderText="THERAPIST"><ItemTemplate><%#Eval("Therapist").ToString()%></ItemTemplate></asp:TemplateField>--%>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# DbHelper.Configuration.FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <%--<asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>--%>
                <asp:TemplateField HeaderText="SESSION GOAL"><ItemTemplate><%#Eval("SessionGoal").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="IMPAIREMENTS"><ItemTemplate><%#IMPAIREMENTS(Eval("AppointmentID").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTIVITY"><ItemTemplate><%#Eval("Activity").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="EQUIPMENTS"><ItemTemplate><%#Eval("Equipments").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="PERFORMANCE"><ItemTemplate><%#PERFORMANCE(Eval("PerformanceID").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="GOAL ASS. SCALE"><ItemTemplate><%#GOALASSSCALE(Eval("GoalAssScaleID").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="LONG TERM GOAL"><ItemTemplate><%#Eval("LongTermGoals").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SHORT TERM GOAL"><ItemTemplate><%#Eval("ShortTermGoals").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SUGGESTIONS"><ItemTemplate><%#Eval("Suggestions").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SNEHAL DESHPANDE SUGGESTION"><ItemTemplate><%#Eval("Suggestion").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="SNEHAL DESHPANDE OBSERVATION"><ItemTemplate><%#Eval("Observation").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="MAIL SEND"><ItemTemplate><%#Eval("MailSend").ToString()%></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTION"><ItemTemplate> 
                    <%#GetAction(Eval("UniqueID").ToString())%>
                    &nbsp;
                    <%# SnehBLL.Appointments_Bll.SessionDownloadViewLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>
                    <%# SnehBLL.Appointments_Bll.SessionViewLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>
                    <%#GetMailink(Eval("UniqueID").ToString())%>
                    <%--<%# SnehBLL.Appointments_Bll.SessionRptLink(Eval("UniqueID").ToString(), Eval("IsUpdated").ToString())%>--%>
                </ItemTemplate>
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="95px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">
                        Daily Report</h5>
                </div>
                <div class="modal-body">
                     
                </div>
                <div class="modal-footer">
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
