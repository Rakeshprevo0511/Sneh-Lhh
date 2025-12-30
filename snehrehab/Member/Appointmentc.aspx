<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Appointmentc.aspx.cs" Inherits="snehrehab.Member.Appointmentc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Edit Appointment :
            </div>
            <div class="pull-right">
                <a href="/Member/Appointment.aspx" class="btn btn-primary">Add New</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">                
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="ddl_Session" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:DropDownList ID="txtTherapist" runat="server" CssClass="chzn-select input-medium span3"> 
                    </asp:DropDownList>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Patient Name" CssClass="span2"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="my-datepicker span2" placeholder="From Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;">
                    <asp:TextBox ID="txtUpto" runat="server" CssClass="my-datepicker span2" placeholder="Upto Date" Width="100px"></asp:TextBox>
                </div>
                <div class="pull-left" style="display:inline-block;margin-right:5px;margin-top: 4px;">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" onclick="btnSearch_Click">Search</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-danger" OnClick="btnExport_Click">Export</asp:LinkButton>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
            <asp:GridView ID="AppointmentGV" runat="server" CssClass="table table-bordered table-responsive" 
                PagerStyle-CssClass="custome-pagination" AutoGenerateColumns="false"
                OnPageIndexChanging="AppointmentGV_PageIndexChanging" PageSize="30" 
                AllowPaging="true" onrowdatabound="AppointmentGV_RowDataBound">
                <EmptyDataTemplate>No records found...</EmptyDataTemplate>
                <Columns>
                <asp:TemplateField HeaderText="SR NO"><ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate><HeaderStyle Width="40px"/></asp:TemplateField>
                <asp:TemplateField HeaderText="FULL NAME"><ItemTemplate><%#Eval("FullName").ToString()%></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="SESSION" DataField="SessionName"/>
                <asp:BoundField HeaderText="THERAPIST" DataField="Therapist"/>
                <asp:TemplateField HeaderText="DATE"><ItemTemplate><%# FORMATDATE(Eval("AppointmentDate").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="DURATION"><ItemTemplate><%#Eval("Duration").ToString() +" Min"%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="TIME"><ItemTemplate><%#TIMEDURATION(Eval("Duration").ToString(), Eval("AppointmentTime").ToString())%></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="ACTION"><ItemTemplate>
                <%#GETACTION(Eval("UniqueID").ToString(), Eval("AppointmentStatus").ToString())%>
                <asp:HiddenField ID="txtAppointmentID" runat="server" Value='<%# Eval("UniqueID") %>'/>
                <asp:HiddenField ID="txtAppointmentStatusID" runat="server" Value='<%# Eval("AppointmentStatus") %>'/>
                <span style="display:none;" class="apt-rmk"><%#Eval("Remark")%></span>
                </ItemTemplate>                
                <ItemStyle CssClass="text-center" />
                <HeaderStyle CssClass="text-center" Width="50px"/>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
<script type="text/javascript">
    $(function () {
        $('span.apt-rmk').each(function () {
            if ($(this).html().length > 0) {
                $('<tr><td></td><td colspan="7"><i>' + $(this).html() + '</i></td></tr>').insertAfter($(this).closest('tr'));
            }
        });
    });
</script>
</asp:Content>
