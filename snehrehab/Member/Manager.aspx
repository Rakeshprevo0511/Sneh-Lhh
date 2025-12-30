<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Manager.aspx.cs" Inherits="snehrehab.Member.Manager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function ShowImagePreview(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#<%=Image1.ClientID%>').prop('src', e.target.result)
                      .width(100)
                      .height(100);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
    </script>
     <style type="text/css">
        .flot
        {
            float: right;
            margin: 14px 7px;
        }
       @media (max-width:425px)
       {
           .flot
            {
               width: 84%!important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %> :
            </div>
            <div class="pull-right">
                <a href="/Member/ViewList.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Full Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtfullname" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>E-mail ID:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtemailid" runat="server" CssClass="span4"></asp:TextBox>
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
                        <asp:TextBox ID="txtcontactno" runat="server" CssClass="span4" onkeypress="CheckNumeric(event);"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Emergency Contact Number:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtemergencycontact" runat="server" CssClass="span4" onkeypress="CheckNumeric(event);"></asp:TextBox>
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
                        <asp:TextBox ID="txtdateofbirth" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Anniversary Date:</label>
                    <div class=" input-append date control-group">
                    <asp:TextBox ID="txtanniversarydate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        <asp:TextBox ID="txtdesignation" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Qualifications:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtqualifications" runat="server" CssClass="span4"></asp:TextBox>
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
                        <asp:TextBox ID="txtrefdocument" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>Date of Joining:</label>
                    <div class=" input-append date control-group">
                        <asp:TextBox ID="txtdateofjoining" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        <asp:TextBox ID="txtpancardno" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                 <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Aadhar card Number:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtadharcardno" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span>Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtaddress" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Blood Group:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtbloodgroup" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
             <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Clinic ShiftTime From:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtclinicshifttimefrom" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Clinic ShiftTime Upto:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtclinicshifttimeUpto" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
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
                        <%--<img id="Image1" runat="server" style="width: 16%; border: 1px solid #000;"/>--%>
                        <asp:Image ID="Image1" runat="server" style="width: 16%; border: 1px solid #000;"/>
                         <asp:FileUpload ID="txtprofilephoto" runat="server" CssClass="input-sm btn-file" onchange="ShowImagePreview(this);" style="margin-left:20px; width: 165px;" />
                         <span class="flot" style="color: red;width: 67%;"><b>Note :-</b> For best appearance image size should be 500 x 500.</span>
                    </div>
                </div>
                <div class="span6" id="upload" runat="server">
                    <label class="control-label">
                        Upload Degree/Certificate:</label>
                    <div class="control-group">
                        <%--<asp:Image ID="imgdegcert" runat="server"  style="width: 16%; border: 1px solid #000;"/>--%>
                        <asp:FileUpload ID="txtdegcer" runat="server" CssClass="input-sm btn-file" Style="margin-left: 20px;
                            width: 165px;" AllowMultiple="true" accept=".png,.jpg,.jpeg,.gif,.pdf" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label"></label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" >Save</asp:LinkButton>
                        <%--<asp:LinkButton ID="btnSaveNew" runat="server" CssClass="btn btn-primary">Save & New</asp:LinkButton>--%>
                        <a href="/Member/ViewList.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
   
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
        function CheckNumeric(e) {
            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8 & e.keyCode != 46) {
                    event.returnValue = false;
                    return false;
                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8 & e.keyCode != 46) {
                    e.preventDefault();
                    return false;
                }
            }
        }
    </script>
</asp:Content>
