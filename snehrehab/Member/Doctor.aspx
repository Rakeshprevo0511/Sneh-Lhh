<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" Inherits="Member_Doctor" Title="" Codebehind="Doctor.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            margin: 13px 19px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                        <span class="req_red">*</span> Doctor Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSalute" runat="server" CssClass="span1" Text="DR" Enabled="False"></asp:TextBox>
                        <div class="span3"><asp:TextBox ID="txtFullName" runat="server" CssClass="span3 capitalize"></asp:TextBox></div>
                    </div>
                </div>
                 <div class="span6">
                    <label class="control-label">
                         Father Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtfather" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                         Mother Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtmother" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                         Husband/Wife Name:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txthusbandwife" runat="server" CssClass="span4"></asp:TextBox></div>
                    <div class="clearfix">
                    </div>
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Qualification:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtQualification" runat="server" CssClass="chzn-select span4">
                            <asp:ListItem Value="-1">Select Qualification</asp:ListItem>
                            <asp:ListItem Value="1">BPTH</asp:ListItem>
                            <asp:ListItem Value="2">MPT</asp:ListItem>
                            <asp:ListItem Value="3">Neuro</asp:ListItem>
                            <asp:ListItem Value="4">MPT Pediatrics</asp:ListItem>
                            <asp:ListItem Value="5">MPT(Others)</asp:ListItem>
                            <asp:ListItem Value="6">MS</asp:ListItem>
                            <asp:ListItem Value="7">DPT</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Speciality:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtSpeciality" runat="server" CssClass="chzn-select span4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
            <asp:UpdatePanel ID="workplace" runat="server"><ContentTemplate>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Workplace:</label>
                    <div class="control-group">
                        <asp:DropDownList ID="txtWorkplace" runat="server" AutoPostBack="true" CssClass="chzn-select span4"  OnSelectedIndexChanged="txtWorkplace_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                </ContentTemplate></asp:UpdatePanel>
                <div class="span6">
                    <label class="control-label">
                        Other Workplace:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtOtherWorkplace" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
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
                        <span class="req_red">*</span> Mobile No:</label>
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
                       <span class="req_red">*</span>Emergency Contact No:</label>
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
                        <span class="req_red">*</span> Pan Card:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPanCard" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Aadhar Card:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtaadharcard" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Blood Group:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtbloodgroup" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Date Of Birth:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="span4"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Join Date:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtJoinDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Anniversary Date:</label>
                    <div class="input-append date control-group">
                        <asp:TextBox ID="txtanniversary" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-calendar"></i></span>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Country:</label>
                    <div class="control-group">
                    <asp:UpdatePanel ID="UpdateCountry" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtCountry" runat="server" CssClass="chzn-select span4" 
                            AutoPostBack="True" onselectedindexchanged="txtCountry_SelectedIndexChanged">
                        </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> State:</label>
                    <div class="control-group">
                        <asp:UpdatePanel ID="UpdateState" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtState" runat="server" CssClass="chzn-select span4" 
                            AutoPostBack="True" onselectedindexchanged="txtState_SelectedIndexChanged">
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
                        <span class="req_red">*</span> City:</label>
                    <div class="control-group">
                    <asp:UpdatePanel ID="UpdateCity" runat="server"><ContentTemplate>
                        <asp:DropDownList ID="txtCity" runat="server" CssClass="chzn-select span4">
                        </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        <span class="req_red">*</span> Zip Code:</label>
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
                <asp:UpdatePanel ID="clinic" runat="server"><ContentTemplate>
                <div class="span6">
                    <label class="control-label">
                        Clinic Address:</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtClinicAddress" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                </ContentTemplate></asp:UpdatePanel>
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
                        Clinic ShiftTime From:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1From" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Clinic ShiftTime Upto:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability1Upto" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
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
                        <asp:TextBox ID="txtAvailability2From" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
                        <span class="add-on"><i class="icon-time"></i></span>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Clinic ShiftTime Upto:</label>
                    <div class="input-append control-group">
                        <asp:TextBox ID="txtAvailability2Upto" runat="server" CssClass="span2 my-timepicker"></asp:TextBox>
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
                        <asp:Image ID="Image1" runat="server" Style="width: 16%; border: 1px solid #000;" />
                        <asp:FileUpload ID="txtprofilephoto" runat="server" CssClass="input-sm btn-file"
                            onchange="ShowImagePreview(this);" Style="margin-left: 20px; width: 165px;" />
                        <span class="flot" style="color: red;width: 64%;"><b>Note :-</b> For best appearance image size should be 500 x 500.</span>
                    </div>
                </div>
                <div class="span6" id="upload" runat="server">
                    <label class="control-label">
                        Upload Degree/Certificate:</label>
                    <div class="control-group">
                        <asp:FileUpload ID="txtdegcer" runat="server" CssClass="input-sm btn-file" Style="margin-left: 20px;
                            width: 165px;" AllowMultiple="true" accept=".png,.jpg,.jpeg,.gif,.pdf" />
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
            <div class="formRow">
                <div class="span6">
                    <label class="control-label"></label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger" onclick="btnSave_Click" OnClientClick="DisableOnSubmit(this);">Save</asp:LinkButton>
                        <asp:LinkButton ID="btnSaveNew" runat="server" CssClass="btn btn-danger" onclick="btnSaveNew_Click" OnClientClick="DisableOnSubmit(this);">Save & New</asp:LinkButton>
                        <a href="/Member/Doctors.aspx" class="btn btn-default">Cancel</a>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function OtherWorkplace() {
            var a = document.getElementById("<%= txtWorkplace.ClientID %>").value;
            if (a == 0) {
                document.getElementById("<%= txtOtherWorkplace.ClientID %>").disabled = false;
                document.getElementById("<%= txtOtherWorkplace.ClientID %>").focus();
            }
            else {
                document.getElementById("<%= txtOtherWorkplace.ClientID %>").value = "";
                document.getElementById("<%= txtOtherWorkplace.ClientID %>").disabled = true;
            }
        }
    </script>
    <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading_message_mask">
            </div>
            <div class="loading_message">
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

