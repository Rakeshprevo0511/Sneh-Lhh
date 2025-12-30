<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="EnquiryReg.aspx.cs" Inherits="snehrehab.Member.EnquiryReg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                <%= _headerText %> :
            </div>
            <div class="pull-right">
                <a href="/Member/Patients.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content" style="padding: 0px;">
             <div style="margin-top: 20px; margin-bottom: 20px;">
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Patient Code:</label>
                        <div class="control-group">
                            <asp:TextBox ID="txtPatientCode" runat="server" CssClass="span3" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            <span class="req_red">*</span> Full Name:</label>
                        <div class="control-group">
                            <asp:DropDownList ID="txtSaluteName" runat="server" CssClass="span1" onchange="SelectGender();">
                                <asp:ListItem Value="">- - -</asp:ListItem>
                                <asp:ListItem Value="MR">Mr</asp:ListItem>
                                <asp:ListItem Value="MS">Ms</asp:ListItem>
                                <asp:ListItem Value="MAST">Mast</asp:ListItem>
                                <asp:ListItem Value="MISS">Miss</asp:ListItem>
                            </asp:DropDownList>
                            <div class="span3">
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="span3 capitalize"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div> 
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Sex:</label>
                        <div class="control-group">
                            <asp:TextBox ID="txtSex" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div> 
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Telephone No:</label>
                        <div class="control-group">
                            <asp:TextBox ID="txtTelephoneNo" runat="server" CssClass="span4" onkeyup="CheckPatientExists(this.value);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Father Mobile No:</label>
                        <div class="control-group">
                            <asp:TextBox ID="txtFatherMobileNo" runat="server" CssClass="span4" onkeyup="CheckPatientExists(this.value);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Mother Mobile No:</label>
                        <div class="control-group">
                            <asp:TextBox ID="txtMotherMobileNo" runat="server" CssClass="span4" onkeyup="CheckPatientExists(this.value);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                 <div class="formRow">
                    <div class="span6">
                        <label classs="control-label">
                            Diagnosis:
                        </label>
                        <div class="control-group">
                            <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                            Other Diagnosis :
                        </label>
                        <div class="control-group">
                            <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span4"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <hr />
                <div class="formRow">
                    <div class="span6">
                        <label class="control-label">
                        </label>
                        <div class="control-group">
                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" OnClick="btnSubmit_Click" OnClientClick="DisableOnSubmit(this);"> Submit </asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="btnComplete" runat="server" CssClass="btn btn-success" Visible="false"> Complete Registration </asp:LinkButton>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
            </div>
            <div class="clearfix">
            </div>
        </div>
    </div> 
    <script type="text/javascript">
        window.onload = function() {
            SelectGender();  
        }
        function SelectGender() {
            var a = document.getElementById("<%= txtSaluteName.ClientID %>").selectedIndex;
            document.getElementById("<%= txtSex.ClientID %>").value = "";
            if (a == 1 || a == 3) {
                document.getElementById("<%= txtSex.ClientID %>").value = "Male";
            }
            else
                if (a == 2 || a == 4) {
                document.getElementById("<%= txtSex.ClientID %>").value = "Female";
            }
        }
        function CheckPatientExists(val) {
            if ($('#<%=txtPatientUnique.ClientID %>').val().length <= 0) {
                if (val.length >= 10) {
                    $.ajax({
                        type: "POST", url: "/Snehrehab.asmx/CheckPatientExists", data: "{'tel':'" + $('#<%= txtTelephoneNo.ClientID %>').val() + "', 'fa':'" + $('#<%= txtFatherMobileNo.ClientID %>').val() + "', 'mo':'" + $('#<%= txtMotherMobileNo.ClientID %>').val() + "'}",
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (response) {
                            if (response.d && response.d.length > 0) {
                                $('#myModal div.modal-body').html(response.d);
                                $('#myModal').modal().show();
                            }
                        }
                    });
                }
            }
        }
        var new_apt_id = ''; var new_pat_id = '';
        function fwdToRegistration(p, a) {
            new_apt_id = a; new_pat_id = p;
            $('#patient_type_modal').modal().show();
        }
        function NextToRegistration() {
            var t = $('#txt_new_patient_type').val();
            if (t == 1) {
                window.location = '/Member/Adult.aspx?record=' + new_pat_id + '&apt=' + new_apt_id + '&return=101';
            } else if (t == 2) {
                window.location = '/Member/Pediatric.aspx?record=' + new_pat_id + '&apt=' + new_apt_id + '&return=101';
            }
        }
    </script>
    <asp:HiddenField ID="txtPatientUnique" runat="server" Value="" />
    <style type="text/css">
        .modal{width: auto;margin: 0px;border: none;border-radius: 6px;box-shadow: none;background-color: transparent;
               background-clip: unset;display: none;overflow: auto;overflow-y: scroll;position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1050;-webkit-overflow-scrolling: touch;outline: 0;}
        .modal-dialog {background: #FFF;position: relative;width: auto;max-width: 600px;margin: 10px;}
        .modal-sm {max-width: 300px;}
        .modal-lg {max-width: 900px;}
        @media (min-width: 768px) {.modal-dialog {margin: 30px auto;}}
        @media (min-width: 320px) {.modal-sm {margin-right: auto;margin-left: auto;}}
        @media (min-width: 620px) {.modal-dialog {margin-right: auto;margin-left: auto;}
                                   .modal-lg {margin-right: 10px;margin-left: 10px;}}
        @media (min-width: 920px) {.modal-lg {margin-right: auto;margin-left: auto;}}
        .modal.fade.in{top:0%;}
    </style>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" style="margin: 0px;">
                        Registered Patient</h5>
                </div>
                <div class="modal-body">
                     
                </div>
                <div class="modal-footer">
                    
                </div>
            </div>
        </div>
    </div>
    <div id="patient_type_modal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" style="">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="opacity: 0.8;"><span aria-hidden="true">×</span></button>
                    <h5 class="modal-title" style="margin:0px;">Patient Type</h5>
                </div>
                <div class="modal-body">
                     <div class="form-horizontal">
                        <div class="formRow">
                            <label class="span1" style="width: 100px;padding-top: 5px;margin-left: 0px;">Select Type</label>
                            <select id="txt_new_patient_type" class="span2">
                                <option value="1">Adult Registration</option>
                                <option value="2">Pediatric Registration</option>
                            </select>  
                        </div> 
                     </div>
                     <div class="clearfix"></div>
                        <br />
                </div>
                <div class="modal-footer">
                    <a href="javascript:;" class="btn btn-sm btn-primary" onclick="NextToRegistration();"> Next </a>
                    &nbsp;
                    <button class="btn btn-sm btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>