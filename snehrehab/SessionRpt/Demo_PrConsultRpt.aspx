<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_PrConsultRpt.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_PrConsultRpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.Morphology-OuterTopTable{}
.Morphology-OuterTopTable tr td{padding: 5px;border: 1px solid #ccc;text-align: center;}
.Morphology-Upper-Limb{}
.Morphology-Upper-Limb tr td{padding: 5px;border: 1px solid #CCC;}
.Morphology-Lower-Limb{}
.Morphology-Lower-Limb tr td{padding: 5px;border: 1px solid #CCC;}
.ndt-default-table{}
.ndt-default-table tr td{border: 1px solid #ccc;padding: 10px;}
span.char-limit-msg{font-style:italic;color:red;font-size: 11px;}
.checkboes{float:left;margin-right:10px;}
.lable_textarea{position: initial; margin: 21px auto;}
.lable_text{position: absolute; margin: 50px auto;}
</style>
<script type="text/javascript">
    $(function () {
        var maxLines = 8; var maxChar = 800;
        $('div.char-line-limiter textarea').keyup(function (e) {
            var lines = $(this).val().replace(/\r/g, '').split('\n');
            var chars = $(this).val().length;
            var s = $(this).parents('div.char-line-limiter').find('span.char-limit-msg');
            var msg = '';
            if (chars > 0) {
                if (maxChar - chars >= 0) {
                    msg = '<b>' + (maxChar - chars).toString() + '</b> Character\'s remaning';
                } else {
                    msg = '<b>' + (chars - maxChar).toString() + '</b> Character\'s exceeds';
                }
            } else {
                msg = 'You can enter maximum <b>' + (maxChar - chars).toString() + '</b> character\s';
            }
            if (lines.length > maxLines) {
                msg = 'Please use maximum <b>8</b> lines only';
                var le = false;
                for (var i = 0; i < lines.length; i++) {
                    if (lines[i].length > (maxChar / maxLines)) {
                        le = true; break;
                    }
                }
                if (le) {
                    msg = 'Use maximum <b>' + (maxChar / maxLines).toString() + '</b> character\'s in one line';
                }
            }
            $(s).html(msg);
        });
        $('div.char-line-limiter textarea').trigger('keyup');
    });
</script>
<script type="text/javascript">
    $('#option_box_single_choice').show();
    var total_to_view = parseInt($('#<%= txtVisibleOption.ClientID %>').val()); if (isNaN(total_to_view)) { total_to_view = 0; }
    if (total_to_view <= 2) { total_to_view = 2; }
    var ctls = $('#option_box_single_choice').find('.cloneThisRow');
    for (var i = 1; i <= ctls.length; i++) {
        if (i <= total_to_view) {
            $(ctls[i - 1]).removeClass('hide');
        } else {
            if (!$(ctls[i - 1]).hasClass('hide')) {
                $(ctls[i - 1]).addClass('hide');
            }
        }
    }
    AddRemoveButton($(ctls[0]));
    function show_next_option(ctl) {
        cloneThisRowAdded = $(ctl).parents('.cloneContainer').children('div.hide:first');
        $(cloneThisRowAdded).removeClass('hide');
        $(cloneThisRowAdded).find('input[type="text"], textarea').val('');
        AddRemoveButton(ctl);
    }
    function AddRemoveButton(ctl) {
        var ctls = $(ctl).parents('.cloneContainer').find('.cloneThisRow:not(.hide)');
        $('#<%= txtVisibleOption.ClientID %>').val(ctls.length);
        ctls.find('.rbutton').html('');
        if (ctls.length > 2) {
            $(ctls[ctls.length - 1]).find('.rbutton').html('<a href=\"javascript:;\" class=\"btn btn-xs btn-default btn-danger\" style=\"float:right; margin-left:20px;\" onclick=\"remove_this_option(this)\"><i class=\"fa fa-minus\"></i></a>');
        }
    }
    function remove_this_option(ctl) {
        cloneThisRowRemoved = $(ctl).closest('.cloneThisRow');
        $(cloneThisRowRemoved).closest('.cloneThisRow').addClass('hide');
        $(cloneThisRowRemoved).find('input[type="text"], textarea').val('');
        var my_fn = $(ctl).attr('my_fn');
        if (my_fn) {
            var fn = window[my_fn];
            if (typeof fn === "function") fn();
        }
        AddRemoveButton(ctl);
    }
</script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                Pre-Consultation Report :
            </div>
            <div class="pull-right">
                <a class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">Patient Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPatient" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">Session :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSession" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">Mark as Report Final :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtFinal" runat="server" />
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">Mark as Report Given :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtGiven" runat="server" />
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">Given Date :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtGivenDate" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <hr />
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">&nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text=" Submit "
                            OnClientClick="DisableOnSubmit(this);"  OnClick="btnSubmit_Click"></asp:LinkButton>
                        &nbsp;
                        <%= _printUrl %>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="clearfix"></div>
            <div class="formRow">
                <div class="span12">
                    <hr />
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="formRow">
                <div class="span12">
                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server">
                        <ajaxToolkit:TabPanel ID="TabPanel13" runat="server" HeaderText="Patient Information">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Date of pre-Consultation :
                                            </div>
                                            <asp:TextBox ID="txtDatepreConsult" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true" Enabled="False" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Language you're comfortable in :
                                            </div>
                                            <asp:TextBox ID="txtComfortableLanguage" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Date of birth :
                                            </div>
                                            <asp:TextBox ID="txtDateBirth" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Expected date of delivery :
                                            </div>
                                            <asp:TextBox ID="txtDateofDelivery" runat="server" CssClass="span2 my-datepicker" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Corrected Age - if relevant :
                                            </div>
                                            <asp:TextBox ID="txtCorrectAge" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Age :
                                            </div>
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gender :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckFemale" runat="server" CssClass="checkboes" onclick="Check_Female_Click();"  Text="Female" Enabled="false"/>
                                                    <asp:CheckBox ID="CheckMale" runat="server" CssClass="checkboes" onclick="Check_Male_Click();" Text="Male" Enabled="false"/>
                                                    <asp:CheckBox ID="CheckOther" runat="server" CssClass="checkboes" onclick="Check_Other_Click();"  Text="Other" Enabled="false"/>
                                                    <script type="text/javascript">
                                                        function Check_Female_Click() {
                                                            var ctl = $('#<%=CheckFemale.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=CheckMale.ClientID %>').prop('checked', false);
                                                            $('#<%=CheckOther.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Male_Click() {
                                                            var ctl = $('#<%=CheckMale.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=CheckFemale.ClientID %>').prop('checked', false);
                                                            $('#<%=CheckOther.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Other_Click() {
                                                            var ctl = $('#<%=CheckOther.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFemale.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMale.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Does Your child attend School ?
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="YesAttend" runat="server" CssClass="checkboes" onclick="YesAttends();"  Text="Yes" Enabled="false"/>
                                                    <asp:CheckBox ID="Noattend" runat="server" CssClass="checkboes" onclick="NoAttends();" Text="No" Enabled="false"/>
                                                    <script type="text/javascript">
                                                        function YesAttends() {
                                                            var ctl = $('#<%=YesAttend.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Noattend.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function NoAttends() {
                                                            var ctl = $('#<%=Noattend.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=YesAttend.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Which school does your child study in ? Mention online/offline :
                                            </div>
                                            <asp:TextBox ID="txtOnlineOffline" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Which grade ?
                                            </div>
                                            <asp:TextBox ID="txtWhichGrade" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Name :
                                            </div>
                                            <asp:TextBox ID="txtMotherName" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's current age  
                                            </div>
                                            <asp:TextBox ID="txtMotherAge" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's age during conception
                                            </div>
                                            <asp:TextBox ID="txtMotherAgeDC" runat="server" CssClass="span2"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Qualification :
                                            </div>
                                            <asp:TextBox ID="txtMotherQualification" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Occupation :
                                            </div>
                                            <asp:TextBox ID="txtMotherOccupation" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's Working Hours :
                                            </div>
                                            <asp:TextBox ID="txtMotherWorkingHour" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Name :
                                            </div>
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's current age :
                                            </div>
                                            <asp:TextBox ID="txtFatherAge" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's age during conception
                                            </div>
                                            <asp:TextBox ID="txtFatherAgeDC" runat="server" CssClass="span2"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Occupation :
                                            </div>
                                            <asp:TextBox ID="txtFatherOccupation" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Qualification :
                                            </div>
                                            <asp:TextBox ID="txtFatherQualification" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Father's Working Hours :
                                            </div>
                                            <asp:TextBox ID="txtFatherWorkingHour" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Address :
                                            </div>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Contact details - Mother and Father :
                                            </div>
                                            <asp:TextBox ID="txtContactDetails" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Parent’s Email id’s :
                                            </div>
                                            <asp:TextBox ID="txtEmailID" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Referred by :
                                            </div>
                                            <asp:TextBox ID="txtReferredBy" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Therapist during pre consultation :
                                            </div>
                                            <asp:TextBox ID="txtTherapistDuringPC" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Diagnosis if any :
                                            </div>
                                            <asp:TextBox ID="txtDiagnosis" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPI" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Chief Concerns">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at Home :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsHome" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at School :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsSchool" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Chief concerns at social gatherings :
                                            </div>
                                            <asp:TextBox ID="txtChiefConcernsSocialGath" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsCC" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel0" runat="server" HeaderText ="Timeline">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="col-md-12">
                                            <ul style="display:flex; list-style-type:none; justify-content: space-evenly;">
                                                <li style="margin-left: -125px;"><lable>Date/Month</lable></li>
                                                <li><lable>RelevantHistory</lable></li>
                                                <li><lable>Hospital<br/>DoctorsVisited</lable></li>
                                                <li><lable>Doctors<br/>Recommendation</lable></li>
                                                <li><lable>Investigations<br />RecordsResults</lable></li>
                                            </ul>
                                           </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                <asp:HiddenField ID="txtVisibleOption" runat="server" Value="2" />
                                               <div id="option_box_single_choice">
                                                    <div class="form-group row col-sm-12">
                                                        <div class="col-md-10">
                                                            <div class="cloneContainer">
                                                                <asp:Repeater ID="txtSignleChoice" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class='row cloneThisRow <%# cloneClass(Container.ItemIndex, Eval("Option").ToString(), Eval("Option1").ToString(), Eval("Option2").ToString(), Eval("Option3").ToString(), Eval("Option4").ToString(), Eval("Option5").ToString()) %>'>
                                                                            <div class="col-sm-2">
                                                                                <%--<label class="control-label"></label>--%>
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <ul class="d-flex" style="display:flex; list-style-type:none;">
                                                                                    <asp:HiddenField ID="txtPreConsultID" runat="server" Value='<%#Eval("Option") %>' />
                                                                                    <li class="mr_5"><asp:TextBox ID="txtDateMonth" runat="server"  Text='<%#Eval("Option1") %>' Enabled="false"></asp:TextBox></li>
                                                                                    
                                                                                    <li class="mr_5"><asp:TextBox ID="txtRelevantHistory" runat="server"  Text='<%#Eval("Option2") %>' Enabled="false"></asp:TextBox></li>
                                                                                    
                                                                                    <li class="mr_5"><asp:TextBox ID="txtHospitalDoctorsVisited" runat="server"  Text='<%#Eval("Option3") %>' Enabled="false"></asp:TextBox></li>
                                                                                    
                                                                                    <li class="mr_5"><asp:TextBox ID="txtDoctorsRecommendations" runat="server"  Text='<%#Eval("Option4") %>' Enabled="false"></asp:TextBox></li>
                                                                                    
                                                                                    <li class="mr_5"><asp:TextBox ID="txtInvestigationsRecordsResults" runat="server"  Text='<%#Eval("Option5") %>' Enabled="false"></asp:TextBox></li>
                                                                                     <div class="col-md-3 padding-5">
                                                                                      <%# cloneButtonLeft_sm(Container.ItemIndex)%>
                                                                                    </div>
                                                                                </ul>
                                                                            </div>
                                                                            <div class="col-sm-12">
                                                                                <div class="row">
                                                                                    <div class="col-sm-9">
                                                                                        <%--<asp:TextBox ID="DateMonth" runat="server"  Text='<%#Eval("Option") %>'></asp:TextBox>--%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                           
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Family History and Relations">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Consanguinity :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckConsan" runat="server" CssClass="checkboes" onclick="Consanguineous_Click();" Text="Consanguineous Marriage" Enabled="false"/>
                                                    <asp:CheckBox ID="CheckNonConsan" runat="server" CssClass="checkboes" onclick="NonConsanguineous_Click();" Text="Non- Consanguineous" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Consanguineous_Click() {
                                                            var ctl = $('#<%=CheckConsan.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNonConsan.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function NonConsanguineous_Click() {
                                                            var ctl = $('#<%=CheckNonConsan.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckConsan.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 If Consanguinous - Degree of Consanguinity :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check1Deg" runat="server" CssClass="checkboes" onclick="Check1Deg_Click();" Text="1st deg" Enabled="false" />
                                                    <asp:CheckBox ID="Check2Deg" runat="server" CssClass="checkboes" onclick="Check2Deg_Click();" Text="2nd  deg" Enabled="false" />
                                                    <asp:CheckBox ID="Check3Deg" runat="server" CssClass="checkboes" onclick="Check3Deg_Click();" Text="3rd  deg" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Check1Deg_Click() {
                                                            var ctl = $('#<%=Check1Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check2Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check3Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check2Deg_Click() {
                                                            var ctl = $('#<%=Check2Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check1Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check3Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check3Deg_Click() {
                                                            var ctl = $('#<%=Check3Deg.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check1Deg.ClientID %>').prop('checked', false);
                                                                $('#<%=Check2Deg.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span5">
                                            <div class="control-label">
                                                Years of marriage :
                                            </div>
                                            <asp:TextBox ID="txtYearsMarriage" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Family structure :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckNuclear" runat="server" CssClass="checkboes" onclick="Nuclear_Click();" Text="Nuclear" Enabled="false" />
                                                    <asp:CheckBox ID="CheckJoint" runat="server" CssClass="checkboes" onclick="Joint_Click();" Text="Joint" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Nuclear_Click() {
                                                            var ctl = $('#<%=CheckNuclear.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckJoint.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Joint_Click() {
                                                            var ctl = $('#<%=CheckJoint.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNuclear.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Conception :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckNatural" runat="server" CssClass="checkboes" onclick="Natural_Click();" Text="Natural" Enabled="false" />
                                                    <asp:CheckBox ID="CheckIUI" runat="server" CssClass="checkboes" onclick="IUI_Click();" Text="IUI"  Enabled="false" />
                                                    <asp:CheckBox ID="CheckIVF" runat="server" CssClass="checkboes" onclick="IVF_Click();" Text="IVF"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckISCI" runat="server" CssClass="checkboes" onclick="ISCI_Click();" Text="ISCI" Enabled="false" />
                                                    <asp:CheckBox ID="CheckOI" runat="server" CssClass="checkboes" onclick="OI_Click();" Text="OI" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Natural_Click() {
                                                            var ctl = $('#<%=CheckNatural.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function IUI_Click() {
                                                            var ctl = $('#<%=CheckIUI.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function IVF_Click() {
                                                            var ctl = $('#<%= CheckIVF.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function ISCI_Click() {
                                                            var ctl = $('#<%= CheckISCI.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckOI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function OI_Click() {
                                                            var ctl = $('#<%= CheckOI.ClientID%>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNatural.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIUI.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckIVF.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckISCI.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Planning of Conception :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPlanned" runat="server" CssClass="checkboes" onclick="Planned_Click();" Text="Planned" Enabled="false" />
                                                    <asp:CheckBox ID="CheckUnplanned" runat="server" CssClass="checkboes" onclick="Unplanned_Click();" Text="Unplanned"  Enabled="false"/>
                                                    <%--<script type="text/javascript">
                                                        function Planned_Click() {
                                                            var ctl = $('#<%=CheckPlanned.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckUnplanned.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Unplanned_Click() {
                                                            var ctl = $('#<%=CheckUnplanned.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPlanned.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Siblings History :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    Any Siblings ?
                                                </div>
                                                <asp:CheckBox ID="AnySiblingsYes" runat="server" CssClass="checkboes" Text="Yes" Enabled="false" />
                                                <asp:CheckBox ID="AnySiblingsNo" runat="server" CssClass="checkboes" Text="No" Enabled="false" />
                                            </div>                                      
                                        </div>
                                        <div class="span12">
                                            <div class="span5">
                                                <div class="control-label">
                                                    No of Siblings
                                                </div>
                                                <asp:TextBox ID="txtNoOfSiblings" runat="server" CssClass="span2" Enabled="false"></asp:TextBox>
                                            </div>
                                            
                                        </div>
                                        <div class="span12">
                                            <div class="span5">
                                                <div class="control-label">
                                                    Relevant History about siblings
                                                </div>
                                                <asp:TextBox ID="txtRHASiblings" runat="server" CssClass="span2" Enabled="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFH" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Inter parental relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPoor" runat="server" CssClass="checkboes" onclick="CheckPoor_Click();" Text="Poor" Enabled="false" />
                                                    <asp:CheckBox ID="CheckFair" runat="server" CssClass="checkboes" onclick="CheckFair_Click();" Text="Fair" Enabled="false" />
                                                    <asp:CheckBox ID="CheckGood" runat="server" CssClass="checkboes" onclick="CheckGood_Click();" Text="Good" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function CheckPoor_Click() {
                                                            var ctl = $('#<%=CheckPoor.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFair.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGood.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFair_Click() {
                                                            var ctl = $('#<%=CheckFair.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoor.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGood.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckGood_Click() {
                                                            var ctl = $('#<%=CheckGood.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoor.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFair.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Parent child relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPoorr" runat="server" CssClass="checkboes" onclick="CheckPoorr_Click();" Text="Poor"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckFairr" runat="server" CssClass="checkboes" onclick="CheckFairr_Click();" Text="Fair"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckGoodd" runat="server" CssClass="checkboes" onclick="CheckGoodd_Click();" Text="Good"  Enabled="false"/>
                                                    <%--<script type="text/javascript">
                                                        function CheckPoorr_Click() {
                                                            var ctl = $('#<%=CheckPoorr.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFairr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGoodd.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFairr_Click() {
                                                            var ctl = $('#<%=CheckFairr.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoorr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGoodd.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckGoodd_Click() {
                                                            var ctl = $('#<%=CheckGoodd.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPoorr.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFairr.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Inter sibling relationship :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check_Poor" runat="server" CssClass="checkboes" onclick="Check_Poorr_Click();" Text="Poor" Enabled="false" />
                                                    <asp:CheckBox ID="Check_Fair" runat="server" CssClass="checkboes" onclick="Check_Fairr_Click();" Text="Fair" Enabled="false" />
                                                    <asp:CheckBox ID="Check_Good" runat="server" CssClass="checkboes" onclick="Check_Goodd_Click();" Text="Good" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Poorr_Click() {
                                                            var ctl = $('#<%=Check_Poor.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Fair.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Good.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckFairr_Click() {
                                                            var ctl = $('#<%=Check_Fair.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Poor.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Good.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Goodd_Click() {
                                                            var ctl = $('#<%=Check_Good.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Poor.ClientID %>').prop('checked', false);
                                                                $('#<%=Check_Fair.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Domestic violence/ Physical /mental Abuse :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckYes" runat="server" CssClass="checkboes" onclick="Check_Yes_Click();" Text="Yes"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckNo" runat="server" CssClass="checkboes" onclick="Check_No_Click();" Text="No" Enabled="false" />
                                                    <asp:CheckBox ID="CheckMaybe" runat="server" CssClass="checkboes" onclick="Check_Maybe_Click();" Text="Maybe"  Enabled="false"/>
                                                    <%--<script type="text/javascript">
                                                        function Check_Yes_Click() {
                                                            var ctl = $('#<%=CheckYes.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNo.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMaybe.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_No_Click() {
                                                            var ctl = $('#<%=CheckNo.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYes.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckMaybe.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Maybe_Click() {
                                                            var ctl = $('#<%=CheckMaybe.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYes.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckNo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Family relocation :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="Check_Yes" runat="server" CssClass="checkboes" onclick="CheckYes_Click();" Text="Yes"  Enabled="false"/>
                                                    <asp:CheckBox ID="Check_No" runat="server" CssClass="checkboes" onclick="CheckNo_Click();" Text="No" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function CheckYes_Click() {
                                                            var ctl = $('#<%=Check_Yes.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_No.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function CheckNo_Click() {
                                                            var ctl = $('#<%=Check_No.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Check_Yes.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                If yes, state the frequency and write the history of relocation in short :
                                            </div>
                                            <asp:TextBox ID="txtfrequency" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                 Primary Care giver :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckMother" runat="server" CssClass="checkboes" onclick="Check_Mother_Click();" Text="Mother" Enabled="false" />
                                                    <asp:CheckBox ID="CheckFather" runat="server" CssClass="checkboes" onclick="Check_Father_Click();" Text="Father" Enabled="false" />
                                                    <asp:CheckBox ID="CheckGrandparents" runat="server" CssClass="checkboes" onclick="Check_Grandparents_Click();" Text="Grandparents"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckCaretaker" runat="server" CssClass="checkboes" onclick="Check_Caretaker_Click();" Text="Caretaker" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Mother_Click() {
                                                            var ctl = $('#<%=CheckMother.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Father_Click() {
                                                            var ctl = $('#<%=CheckFather.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Grandparents_Click() {
                                                            var ctl = $('#<%=CheckGrandparents.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckCaretaker.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Caretaker_Click() {
                                                            var ctl = $('#<%=CheckCaretaker.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMother.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckFather.ClientID %>').prop('checked', false);
                                                                $('#<%=CheckGrandparents.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mother's screen time :
                                            </div>
                                            <asp:TextBox ID="txtMotherScreenTime" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Screen time of the child :
                                            </div>
                                            <asp:TextBox ID="txtScreenTimeChild" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFR" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Maternal History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Prenatal conditions :
                                            </div>
                                            <asp:TextBox ID="txtPrenatalCondition" runat="server" CssClass="span4" TextMode="MultiLine"  Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Maternal Stress :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckPhysical" runat="server" CssClass="checkboes" onclick="Check_Physical_Click();" Text="Physical" Enabled="false" />
                                                    <asp:CheckBox ID="CheckMental" runat="server" CssClass="checkboes" onclick="Check_Mental_Click();" Text="Mental" Enabled="false" />
                                                    <%--<script type="text/javascript">
                                                        function Check_Physical_Click() {
                                                            var ctl = $('#<%=CheckPhysical.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckMental.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_Mental_Click() {
                                                            var ctl = $('#<%=CheckMental.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckPhysical.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Describe stressors in short :
                                            </div>
                                            <asp:TextBox ID="txtDescribeStressors" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Weight gain during pregnancy :
                                            </div>
                                            <asp:TextBox ID="txtWGDP" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Foetal movements :
                                            </div>
                                            <asp:TextBox ID="txtFoetalMovement" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Prenatal wellness program attended? :
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="CheckYess" runat="server" CssClass="checkboes" onclick="Check_Yes_Click();" Text="Yes"  Enabled="false"/>
                                                    <asp:CheckBox ID="CheckNoo" runat="server" CssClass="checkboes" onclick="Check_No_Click();" Text="No" Enabled="false" />
                                                    <script type="text/javascript">
                                                        function Check_Yes_Click() {
                                                            var ctl = $('#<%=CheckYess.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckNoo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Check_No_Click() {
                                                            var ctl = $('#<%=CheckNoo.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=CheckYess.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsMH" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Peri and Postnatal History">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Duration of labour :
                                            </div>
                                            <asp:TextBox ID="txtDurationLabour" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Type of delivery :
                                            </div>
                                            <asp:CheckBox runat="server" ID="rdoFTND" CssClass="checkboes" Text="FTND"  Enabled="false" />
                                            <asp:CheckBox runat="server" ID="rdoFTNDva" CssClass="checkboes"  Text="FTND vacuum assisted"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="rdoELSCS" CssClass="checkboes"  Text="E- LSCS" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="rdoElectiveLSCS" CssClass="checkboes"  Text="Elective LSCS" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                CIAB? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="rdoYess" CssClass="checkboes" Text="Yes" onclick="rdoYess_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="rdoNoo" CssClass="checkboes" Text="No" onclick="rdoNoo_check()" Enabled="false" />
                                            <script type="text/javascript">
                                                function rdoYess_check() {
                                                    var ctl = $('#<%=rdoYess.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=rdoNoo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function rdoNoo_check() {
                                                    var ctl = $('#<%=rdoNoo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=rdoYess.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Conditions post birth :
                                            </div>
                                            <asp:TextBox ID="txtConditionPostBirth" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Birth Weight :
                                            </div>
                                            <asp:TextBox ID="txtBirthWeight" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gestational Birth Age :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoAGA" CssClass="checkboes" Text="AGA" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoSGA" CssClass="checkboes" Text="SGA" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoLGA" CssClass="checkboes" Text="LGA" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                NICU stay :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoPresent" CssClass="checkboes" Text="Present" onclick="RdoPresent_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoAbsent" CssClass="checkboes" Text="Absent" onclick="RdoAbsent_check()" Enabled="false" />
                                             <script type="text/javascript">
                                                 function RdoPresent_check() {
                                                     var ctl = $('#<%=RdoPresent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoAbsent.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function RdoAbsent_check() {
                                                    var ctl = $('#<%=RdoAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoPresent.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Duration of the NICU stay :
                                            </div>
                                            <asp:TextBox ID="txtDurationNICUstay" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                NICU History :
                                            </div>
                                            <asp:TextBox ID="txtNICUHistory" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Reason For NICU stay :
                                            </div>
                                            <asp:TextBox ID="txtReasonNICUstay" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                APGAR score  :
                                            </div>
                                            <asp:TextBox ID="txtAPGARscore" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Breast fed :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoYes" CssClass="checkboes" Text="Yes" onclick="RdoYes_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoNo" CssClass="checkboes" Text="No" onclick="RdoNo_check()" Enabled="false" />
                                             <script type="text/javascript">
                                                 function RdoYes_check() {
                                                     var ctl = $('#<%=RdoYes.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=RdoNo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function RdoNo_check() {
                                                    var ctl = $('#<%=RdoNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RdoYes.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                If not, how was the baby fed  :
                                            </div>
                                            <asp:TextBox ID="txtBabyFed" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Problems during breast feeding  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioPresent" CssClass="checkboes" Text="Present" onclick="RadioPresent_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioAbsent" CssClass="checkboes" Text="Absent" onclick="RadioAbsent_check()" Enabled="false" />
                                             <script type="text/javascript">
                                                 function RadioPresent_check() {
                                                     var ctl = $('#<%=RadioPresent.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=RadioAbsent.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function RadioAbsent_check() {
                                                    var ctl = $('#<%=RadioAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresent.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Mention problems :
                                            </div>
                                            <asp:TextBox ID="txtMentionProblem" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Till what age was the child breast fed? (If child more than 1.5 years old ) :
                                            </div>
                                            <asp:TextBox ID="txtwaswtcbf" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Any colic issues as a baby? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioYes" CssClass="checkboes" Text="Yes" onclick="RadioYes_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioNo" CssClass="checkboes" Text="No" onclick="RadioNo_check()"  Enabled="false"/>
                                            <script type="text/javascript">
                                                function RadioYes_check() {
                                                    var ctl = $('#<%=RadioYes.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=RadioNo.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioNo_check() {
                                                    var ctl = $('#<%=RadioNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYes.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Other medical issues :
                                            </div>
                                            <asp:TextBox ID="txtOthrtMedicalIssues" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPPH" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Developmental Milestones">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Gross Motor :
                                            </div>
                                            <asp:TextBox ID="txtGrossMotor" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Fine Motor  :
                                            </div>
                                            <asp:TextBox ID="txtFineMotor" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Personal and Social :
                                            </div>
                                            <asp:TextBox ID="txtPersonalandSocial" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Communication  :
                                            </div>
                                            <asp:TextBox ID="txtCommunication" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments  :
                                            </div>
                                            <asp:TextBox ID="txtCommentsDM" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="Sleep">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sleep issues during 0-6 months (put NA if not relevant) :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadiooNo" CssClass="checkboes" Text="No" onclick="RadiooNo_check()"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadiooYes" CssClass="checkboes" Text="Yes" onclick="RadiooYes_check()" Enabled="false" />
                                             <script type="text/javascript">
                                                 function RadiooNo_check() {
                                                     var ctl = $('#<%=RadiooNo.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadiooYes.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function RadiooYes_check() {
                                                    var ctl = $('#<%=RadiooYes.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadiooNo.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Present sleep concerns :
                                            </div>
                                            <asp:CheckBox runat="server" ID="PresentRadio" CssClass="checkboes" Text="Present" onclick="PresentRadio_check()"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="AbsentRadio" CssClass="checkboes" Text="Absent" onclick="AbsentRadio_check()" Enabled="false" />
                                             <script type="text/javascript">
                                                 function PresentRadio_check() {
                                                     var ctl = $('#<%=PresentRadio.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=AbsentRadio.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function AbsentRadio_check() {
                                                    var ctl = $('#<%=AbsentRadio.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=PresentRadio.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sleep duration :
                                            </div>
                                            <asp:TextBox ID="txtSleepduration" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Sleep Type :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioLight" CssClass="checkboes" Text="Light" onclick="RadioLight_click()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioDeep" CssClass="checkboes" Text="Deep" onclick="RadioDeep_check()"  Enabled="false"/>
                                            <script type="text/javascript">
                                                function RadioLight_click() {
                                                    var ctl = $('#<%=RadioLight.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=RadioDeep.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioDeep_check() {
                                                    var ctl = $('#<%=RadioDeep.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioLight.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Co-sleeping :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioAbsentbtn" CssClass="checkboes" Text="Absent" onclick="RadioAbsentbtn_click()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioPresentbtn" CssClass="checkboes" Text="Present" onclick="RadioPresentbtn_click()" Enabled="false" />
                                            <script type="text/javascript">
                                                function RadioAbsentbtn_click() {
                                                    var ctl = $('#<%=RadioAbsentbtn.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresentbtn.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioPresentbtn_click() {
                                                    var ctl = $('#<%=RadioPresentbtn.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAbsentbtn.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Co- sleeping with ? :
                                            </div>
                                            <asp:TextBox ID="txtCosleepingwith" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Any Sleep Adjuncts used ? :
                                            </div>
                                            <asp:TextBox ID="txtAnySleepAdjunctsused" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Nap time :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonPresent" CssClass="checkboes" Text="Present" onclick="RadioButtonPresent_click()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioButtonAbsent" CssClass="checkboes" Text="Absent" onclick="RadioButtonAbsent_click()" Enabled="false" />
                                            <script type="text/javascript">
                                                function RadioButtonPresent_click() {
                                                    var ctl = $('#<%=RadioButtonPresent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioButtonAbsent.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioButtonAbsent_click() {
                                                    var ctl = $('#<%=RadioButtonAbsent.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioButtonPresent.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Nap duration  :
                                            </div>
                                            <asp:TextBox ID="txtNapduration" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsS" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="Feeding Habits">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Feeding habits  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioTypical" CssClass="checkboes" Text="Typical" onclick="RadioTypical_check()"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioAtypical" CssClass="checkboes" Text="Atypical" onclick="RadioAtypical_check()"  Enabled="false"/>
                                             <script type="text/javascript">
                                                 function RadioTypical_check() {
                                                     var ctl = $('#<%=RadioTypical.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioAtypical.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                 function RadioAtypical_check() {
                                                    var ctl = $('#<%=RadioAtypical.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioTypical.ClientID %>').prop('checked', false);
                                                     }
                                                 }
                                             </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Type of food had  :
                                            </div>
                                            <asp:TextBox ID="txtTypeoffoodhad" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food consistency :
                                            </div>
                                            <asp:TextBox ID="txtFoodconsistency" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food temperature :
                                            </div>
                                            <asp:TextBox ID="txtFoodtemperature" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Food taste :
                                            </div>
                                            <asp:TextBox ID="txtFoodtaste" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsFeHa" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel9" runat="server" HeaderText="Into the child’s heart">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                     <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are your child’s likes and dislikes ? :
                                            </div>
                                            <asp:TextBox ID="txtChildLikes" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="formRow"> 
                                        <div class="span12">
                                            <div class="control-label">
                                                What are your child’s dislikes? :
                                            </div>
                                            <asp:TextBox ID="txtChildDislikes" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are his/her moments of happiness? :
                                            </div>
                                            <asp:TextBox ID="txtMomentsOfHappiness" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                What are his/her moments of fear? :
                                            </div>
                                            <asp:TextBox ID="txtMomentsOfFear" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Can your child show/describe his/her feelings and emotions? :
                                            </div>
                                            <asp:TextBox ID="txtFeelingsNemotions" runat="server" CssClass="span4" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Do you think your child shows signs of stress/ anxiety ? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonYes" CssClass="checkboes" Text="Yes" />
                                            <asp:CheckBox runat="server" ID="RadioButtonNo" CssClass="checkboes" Text="No" />
                                            <asp:CheckBox runat="server" ID="RadioButtonMaybe" CssClass="checkboes" Text="Maybe" />
                                        </div>
                                    </div>--%> 
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsITCH" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel10" runat="server" HeaderText="Play Behaviour">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Play behaviour :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioOrganised" CssClass="checkboes" Text="Organised"  onclick="RadioOrganised_check()" Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioDisorganised" CssClass="checkboes" Text="Disorganised" onclick="RadioDisorganised_check()" Enabled="false" />
                                              <script type="text/javascript">
                                                  function RadioOrganised_check() {
                                                      var ctl = $('#<%=RadioOrganised.ClientID %>')[0];
                                                     if (ctl.checked) {
                                                         $('#<%=RadioDisorganised.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                  function RadioDisorganised_check() {
                                                    var ctl = $('#<%=RadioDisorganised.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioOrganised.ClientID %>').prop('checked', false);
                                                      }
                                                  }
                                              </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Interaction with peers  :
                                            </div>
                                            <asp:TextBox ID="txtInteractionwithpeers" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Stranger anxiety ? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioPresentButton" CssClass="checkboes" Text="Present" onclick="RadioPresentButton_check()" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioAbsentButton" CssClass="checkboes" Text="Absent" onclick="RadioAbsentButton_check()" Enabled="false" />
                                            <script type="text/javascript">
                                                function RadioPresentButton_check() {
                                                    var ctl = $('#<%=RadioPresentButton.ClientID %>')[0];
                                                      if (ctl.checked) {
                                                          $('#<%=RadioAbsentButton.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioAbsentButton_check() {
                                                    var ctl = $('#<%=RadioAbsentButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioPresentButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Does your child play with toys? :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioYesButton" CssClass="checkboes" Text="Yes" onclick="RadioYesButton_check()"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioNoButton" CssClass="checkboes" Text="No" onclick="RadioNoButton_check()"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioMaybeButton" CssClass="checkboes" Text="Maybe" onclick="RadioMaybeButton_check()"  Enabled="false"/>
                                            <script type="text/javascript">
                                                function RadioYesButton_check() {
                                                    var ctl = $('#<%=RadioYesButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioNoButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioMaybeButton.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                function RadioNoButton_check() {
                                                    var ctl = $('#<%=RadioNoButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYesButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioMaybeButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function RadioMaybeButton_check() {
                                                    var ctl = $('#<%=RadioMaybeButton.ClientID %>')[0];
                                                    if (ctl.checked) {
                                                        $('#<%=RadioYesButton.ClientID %>').prop('checked', false);
                                                        $('#<%=RadioNoButton.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Preference of toys :
                                            </div>
                                            <asp:TextBox ID="txtPreferenceoftoys" runat="server" CssClass="span4" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox ID="txtCommentsPB" runat="server" CssClass="span4" TextMode="MultiLine"  Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel11" runat="server" HeaderText="ADL's">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Brushing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioDependent" CssClass="checkboes"  Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioAssisted" CssClass="checkboes"  Text="Assisted"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioIndependent" CssClass="checkboes"  Text="Independent"  Enabled="false"/>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsBrushing" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Bathing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="DependentRadio" CssClass="checkboes"  Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="AssistedRadio" CssClass="checkboes" Text="Assisted" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="IndependentRadio" CssClass="checkboes"  Text="Independent" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsBathing" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Toileting :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioDependentButton" CssClass="checkboes" Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioAssistedButton" CssClass="checkboes"  Text="Assisted" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioIndependentButton" CssClass="checkboes"  Text="Independent"  Enabled="false"/>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsToileting" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Dressing :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioButtonDependent" CssClass="checkboes"  Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioButtonAssisted" CssClass="checkboes"  Text="Assisted"  Enabled="false"/>
                                            <asp:CheckBox runat="server" ID="RadioButtonIndependent" CssClass="checkboes"  Text="Independent"  Enabled="false"/>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsDressing" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Eating :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RadioBtnDependent" CssClass="checkboes"  Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioBtnAssisted" CssClass="checkboes"  Text="Assisted" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RadioBtnIndependent" CssClass="checkboes"  Text="Independent" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsEating" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Ambulation  :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdoDependent" CssClass="checkboes" Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoAssisted" CssClass="checkboes" Text="Assisted" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdoIndependent" CssClass="checkboes"  Text="Independent" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsAmbulation" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                               Transfers   :
                                            </div>
                                            <asp:CheckBox runat="server" ID="RdobtnDependent" CssClass="checkboes"  Text="Dependent" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdobtnAssisted" CssClass="checkboes"  Text="Assisted" Enabled="false" />
                                            <asp:CheckBox runat="server" ID="RdobtnIndependent" CssClass="checkboes"  Text="Independent" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Comments :
                                            </div>
                                            <asp:TextBox runat="server" ID="txtCommentsTransfers" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel14" runat="server" HeaderText="Observations">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Add comments :
                                            </div>
                                            <asp:TextBox ID="txtAddComments" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Evaluation Recommended">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                Add comments :
                                            </div>
                                            <asp:TextBox ID="txtAddEvalRec" runat="server" CssClass="span4" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>
                    <div class="clearfix"></div>
                </div>
               <div class="clearfix"></div>
            </div>  
          <div class="clearfix"></div>
        </div>
    </div>
</asp:Content>

