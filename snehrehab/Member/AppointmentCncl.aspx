<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_AppointmentCncl" Title="" Codebehind="AppointmentCncl.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Cancel Appointment :
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Full Name:</label>
                    <div class="control-group">
                        <strong><%=PD.FullName%></strong>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Patient Type:</label>
                    <div class="control-group">
                        <strong><%=PD.PatientTypeID == 1 ? "Adult Registration" : "Pediatric Registration"%></strong>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Session:</label>
                    <div class="control-group">
                        <strong><%=new SnehBLL.SessionMast_Bll().Get(AD.SessionID).SessionName%></strong>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Session Date:</label>
                    <div class="control-group">
                        <strong><%=AD.AppointmentDate.ToString(DbHelper.Configuration.showDateFormat)%></strong>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Duration:</label>
                    <div class="control-group">
                        <strong><%=TIMEDURATION(AD.Duration, AD.AppointmentFrom)%></strong>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Time In Minute:</label>
                    <div class="control-group">
                        <strong><%=AD.Duration%> Minute</strong>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Remark:</label>
                    <div class="control-group">
                         <asp:TextBox ID="txtRemark" runat="server" CssClass="span4" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                     
                </div>
                <div class="clearfix">
                </div>
            </div>



            <div class="alert alert-warning">
                Cancelling appointment entry will be permanant.<br />
                Are you sure to Cancel Appointment..??
                <div class="clearfix">
                </div>
                <br />
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" OnClick="btnSubmit_Click">Submit</asp:LinkButton>
                &nbsp; <a href='<%=returnUrl %>' class="btn btn-default">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>

