<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Doctorv.aspx.cs" Inherits="snehrehab.Member.Doctorv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function () {
        $('.grid-content input[type="text"]').attr('disabled', 'disabled');
        $('.grid-content textarea').attr('disabled', 'disabled');
        $('.grid-content select').attr('disabled', 'disabled');
    });
</script>
<style type="text/css">
    input[disabled], select[disabled], textarea[disabled], input[readonly], select[readonly], textarea[readonly] {cursor: pointer;background-color: #ffffff;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %> :
            </div>
            <div class="pull-right">
                <a href="/Member/Doctors.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Doctor Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSalute" runat="server" CssClass="span1" Text="DR"></asp:TextBox>
                        <div class="span3"><asp:TextBox ID="txtFullName" runat="server" CssClass="span3"></asp:TextBox></div>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Qualification:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtQualification" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Speciality:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtSpeciality" runat="server" CssClass="span4">
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Workplace:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtWorkplace" runat="server" CssClass="span4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Other Workplace:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtOtherWorkplace" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        E-Mail ID:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMailID" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Mobile No:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Telephone # 1:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtTelephone1" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Telephone # 2:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtTelephone2" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Pan Card:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPanCard" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Date Of Birth:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Join Date:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtJoinDate" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Country:</label>
                    <div class="control-group">
                    <asp:UpdatePanel ID="UpdateCountry" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtCountry" runat="server" CssClass="span4" >
                        </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        State:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateState" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtState" runat="server" CssClass="span4">
                        </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        City:</label>
                    <div class="control-group">
                    <asp:UpdatePanel ID="UpdateCity" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtCity" runat="server" CssClass="span4">
                        </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Zip Code:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Week Day(s) Off:</label>
                    <div class="control-group">
                        <asp:ListBox ID="txtWeekDayOff" runat="server" CssClass="span4">
                        <asp:ListItem Value="-1">Not Allowed</asp:ListItem>
                        <asp:ListItem Value="1">Sunday</asp:ListItem>
                        <asp:ListItem Value="2">Monday</asp:ListItem>
                        <asp:ListItem Value="3">Tuesday</asp:ListItem>
                        <asp:ListItem Value="4">Wednesday</asp:ListItem>
                        <asp:ListItem Value="5">Thursday</asp:ListItem>
                        <asp:ListItem Value="6">Friday</asp:ListItem>
                        <asp:ListItem Value="7">Saturday</asp:ListItem>
                        </asp:ListBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Clinic Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtClinicAddress" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Clinic Tel # 1:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtClinicTel1" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Clinic Tel # 2:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtClinicTel2" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Availability 1 From:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1From" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Availability 1 Upto:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1Upto" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Availability 2 From:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability2From" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Availability 2 Upto:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability2Upto" runat="server" CssClass="span2"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Facilitator:</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtFacilitator" runat="server" CssClass="checkboes" />
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Remarks:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div> 
        </div>
    </div> 
</asp:Content>
