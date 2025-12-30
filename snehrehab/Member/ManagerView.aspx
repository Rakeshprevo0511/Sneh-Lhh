<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="ManagerView.aspx.cs" Inherits="snehrehab.Member.ManagerView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %> :
            </div>
            
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Full Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtfullname" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>E-mail ID:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtemailid" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Mobile Number:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtcontactno" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Emergency Contact Number:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtemergencycontact" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Date Of Birth:</label>
                    <div class=" input-append date control-group">
                        <asp:TextBox ID="txtdateofbirth" runat="server" CssClass="span2 my-datepicker" ReadOnly="true"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Anniversary Date:</label>
                    <div class=" input-append date control-group">
                    <asp:TextBox ID="txtanniversarydate" runat="server" CssClass="span2 my-datepicker" ReadOnly="true"></asp:TextBox>
                    <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>Designation:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtdesignation" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Qualifications:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtqualifications" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Reference Documents:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtrefdocument" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>Date of Joining:</label>
                    <div class=" input-append date control-group">
                        <asp:TextBox ID="txtdateofjoining" runat="server" CssClass="span2 my-datepicker" ReadOnly="true"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Pancard Number:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtpancardno" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtaddress" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Aadhar card Number:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtadharcardno" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
            
                <div class="span6">
                    <label class="control-label">
                        Photo:</label>
                    <div class="control-group">
                        <asp:Image ID="Image1" runat="server" style="width: 16%; border: 1px solid #000;"/>
                    </div>
                </div>
                
                
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label"></label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-primary" OnClick="btnBack_Click" >Back</asp:LinkButton>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
