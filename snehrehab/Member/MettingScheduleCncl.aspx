<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="MettingScheduleCncl.aspx.cs" Title="" Inherits="snehrehab.Member.MettingScheduleCncl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
    <div class="grid-title">
            <div class="pull-left">
                Cancel Dr Meeting Schedule :
            </div>
     </div>
    <div class="grid-content">
        <div class="formRow">
            <div class="span6">
                <label class="control-label">
                    Therapist:</label>
                <div class="control-group">
                    <strong><%=courses%></strong>
                    <%--<strong><%#DOCTORLIST(Eval("AppointmentID").ToString()) %></strong>--%>
                </div>
             </div>
            <div class="span6">
                <label class="control-label">
                    Schdeule Type:</label>
                <div class="control-group">
                    <strong><%=AD.ScheduleType%></strong>
                </div>
            </div>
            <div class="clearfix">
            </div>
        </div>
        <div class="formRow">
            <div class="span6">
                <label class="control-label">
                    Appointment Date:</label>
                <div class="control-group">
                    <strong><%=AD.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat)%></strong>
                </div>
            </div>
            <div class="span6">
                <label class="control-label">
                    Schedule Time:</label>
                <div class="control-group">
                    <strong><%=(AD.Available1FromChar + " TO " + AD.Available1UptoChar)%></strong>
                </div>
            </div>
        </div>
         <div class="alert alert-warning">
            Cancelling Dr Meeting Schedule entry will be permanant.<br />
            Are you sure to Cancel Dr Meeting Schedule..??
            <div class="clearfix">
            </div>
            <br />
            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" OnClick="btnSubmit_Click">Submit</asp:LinkButton>
            &nbsp; <a href='<%=returnUrl %>' class="btn btn-default">Cancel</a>
         </div>
    </div>
</div>
</asp:Content>