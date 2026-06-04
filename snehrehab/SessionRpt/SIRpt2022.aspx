<%@ Page Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="SIRpt2022.aspx.cs" Inherits="snehrehab.SessionRpt.SIRpt2022" Title="" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Morphology-OuterTopTable {
        }

            .Morphology-OuterTopTable tr td {
                padding: 5px;
                border: 1px solid #ccc;
                text-align: center;
            }

        .Morphology-Upper-Limb {
        }

            .Morphology-Upper-Limb tr td {
                padding: 5px;
                border: 1px solid #CCC;
            }

        .Morphology-Lower-Limb {
        }

            .Morphology-Lower-Limb tr td {
                padding: 5px;
                border: 1px solid #CCC;
            }

        .ndt-default-table {
        }

            .ndt-default-table tr td {
                border: 1px solid #ccc;
                padding: 10px;
            }

        span.char-limit-msg {
            font-style: italic;
            color: red;
            font-size: 11px;
        }

        .checkboes {
            float: left;
            margin-right: 10px;
        }

        .lable_textarea {
            position: initial;
            margin: 21px auto;
        }

        .lable_text {
            position: absolute;
            margin: 50px auto;
        }

        .save-status {
            margin-right: 15px
        }

        /*RANGE SELECTOR*/
        datalist {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            writing-mode: horizontal-tb;
            width: 700px;
        }

        option {
            padding: 0;
        }

        input[type="range"] {
            width: 700px;
            margin: 0;
        }

        /*RANGE 2 SELECTOR*/
        datalist2 {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            writing-mode: horizontal-tb;
            width: 700px;
        }

        option {
            padding: 0;
        }

        input[type="range"] {
            width: 700px;
            margin: 0;
        }

        .buttonClass {
            background-color: springgreen;
        }

        /*        table, th, td {
  border: 1px solid black;
  border-collapse: collapse;*/
        /*}*/
    </style>

    <%--  <link rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="Stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker.min.css" />--%>

    <%-- <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootswatch/4.6.2/flatly/bootstrap.min.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/4.6.2/js/bootstrap.bundle.min.js" integrity="sha512-igl8WEUuas9k5dtnhKqyyld6TzzRjvMqLC79jkgT3z02FvJyHAuUtyemm/P/jYSne1xwFI06ezQxEwweaiV7VA==" crossorigin="anonymous" referrerpolicy="no-referrer" defer="defer"  type="text/javascript"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" integrity="sha512-894YE6QWD5I59HgZOGReFYm4dnWc1Qt5NtvYSaNcOP+u1T9qYdvdihz0PPSiiqn/+/3e7Jo4EaG7TubfWGUrMQ==" crossorigin="anonymous" referrerpolicy="no-referrer" defer="defer" type="text/javascript"></script>--%>

    <%-- <link href="https://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css">
    
    
    <script src="https://cdnjs.cloudflare.com/ajax/libs/dayjs/1.11.4/dayjs.min.js" integrity="sha512-Ot7ArUEhJDU0cwoBNNnWe487kjL5wAOsIYig8llY/l0P2TUFwgsAHVmrZMHsT8NGo+HwkjTJsNErS6QqIkBxDw==" crossorigin="anonymous" referrerpolicy="no-referrer" defer="defer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/js/all.min.js" integrity="sha512-Tn2m0TIpgVyTzzvmxLNuqbSJH3JP8jm+Cy3hvHrW7ndTDcJ1w5mBiksqDBb8GpE2ksktFvDB/ykZ0mDpsZj20w==" crossorigin="anonymous" referrerpolicy="no-referrer" defer="defer"></script>--%>

    <%--<script type="text/javascript" src="../js/timepicker-bs4.js" defer="defer"></script>--%>

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
        var total_to_view = parseInt($('#<%= textVisibleOption.ClientID %>').val()); if (isNaN(total_to_view)) { total_to_view = 0; }
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
            $('#<%= textVisibleOption.ClientID %>').val(ctls.length);
            ctls.find('.rbutton').html('');
            if (ctls.length > 2) {
                $(ctls[ctls.length - 1]).find('.rbutton').html('<a href=\"javascript:;\" class=\"btn btn-xs btn-default btn-danger\" style=\"float:right; margin- left:20px;\" onclick=\"remove_this_option(this)\"><i class=\"fa fa-minus\"></i></a>');
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

    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/0.9.0rc1/jspdf.min.js"></script>--%>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.3/jspdf.min.js"></script>--%>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js "></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>

</asp:Content>

<%--<script type="text/javascript" src="js/36d/jquery-1.4.2.min.js"></script>--%>
<%--</script>--%>
<%--    <script type="text/javascript">  
        $(document).ready(function () {
            $("form").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
        });
    </script>  --%>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="MsgPlaceHolder"></div>
    <div id="editor"></div>
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                SI Report :
            </div>
            <div class="pull-right">
                <a href="/Reports/Si2022.aspx" class="btn btn-primary">View List</a>
            </div>
        </div>
        <div class="grid-content" style="">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Patient Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPatient" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Session :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSession" runat="server" CssClass="span4" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Mark as Report Final :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtFinal" runat="server" />
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Mark as Report Given :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtGiven" runat="server" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <asp:Panel ID="PanelDiagnosis" runat="server" CssClass="span11 formRow">
                <div class="row">
                    <div class="span2">
                        Diagnosis :
                    </div>
                    <div class="span4">
                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                    </div>
                    <div class="span2">
                        Other Diagnosis :
                    </div>
                    <div class="span2">
                        <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span2"></asp:TextBox>
                    </div>
                </div>
            </asp:Panel>
            <div class="formRow">

                <div class="span6">
                    <label class="control-label">Given Date :</label>

                    <div class="control-group">
                        <asp:TextBox ID="txtGivenDate" runat="server"
                            CssClass="span2 my-datepicker"></asp:TextBox>
                    </div>
                </div>

                <div class="span6">
                    <label class="control-label">&nbsp;</label>

                    <div class="control-group">
                        <asp:LinkButton ID="lnkFinalSave" runat="server"
                            CssClass="btn btn-success"
                            OnClick="lnkFinalSave_Click"
                            OnClientClick="return confirm('Are you sure you want to Mark Date given?');">
                Report Given Save
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <%-- <asp:Panel ID="PanelDiagnosis" runat="server" CssClass="span11 formRow"> 
                 <div class="row">
                     <div class="span2">
                         Diagnosis :</div>
                     <div class="span4">
                         <asp:ListBoxID="txtDiagosis"runat="server"SelectionMode="Multiple"CssClass="chznselectmultispan4"dataplaceholder="SelecDiagnosis"><asp:ListBox>
                     </div>
                     <div class="span2">
                         Other Diagnosis :</div>
                     <div class="span2">
                         <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span2"></asp:TextBox>
                     </div>
                 </div> 
             </asp:Panel>--%>


            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <button type="button" id="btnFinalSubmit" class="btn btn-danger">
                            Submit
                        </button>
                        &nbsp; 
                        <%= _printUrl %>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                        <%--<input type='button' id='btn' value='Print' onclick='printDiv();'>--%>
                    </div>
                    <div style="margin-top:10px; color:#d9534f; font-weight:bold;">
            Note: 
            <span class="buttonClass" style="
                border: 1px solid #818080;
                padding: 1px 10px;
                display: inline-block;
                background-color: #00ff7f;
                color: #4b4949;
                cursor: not-allowed;">
                Save & Next
            </span> 
            is disabled. Please use the Submit button or navigation to save data.
        </div>

            </div>

                </div>
                <div class="clearfix">
                </div>
                

            <div class="formRow">
                <div class="span12">
                    <asp:HiddenField ID="hfdTabs" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCallFrom" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCurTab" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdPrevTab" runat="server" ClientIDMode="Static" />

                    <%--<asp:HiddenField ID="hfdSubTabs" runat="server" ClientIDMode="Static" />
                     <asp:HiddenField ID="hfdSubCallFrom" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdPrevsubTab" runat="server"  ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCursubTab" runat="server" ClientIDMode="Static" />--%>

                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" OnClientActiveTabChanged="clientActiveTabChanged">


                        <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="CLINICAL_OBSERVATION AND DAILY SCHEDULE ">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="col-md-12">
                                            <div class="control-label">
                                                Clinical observations :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ClinicleObse_txt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />

                                        <div class="clearfix"></div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12">
                                            <div class="control-label">
                                                Daily schedule :
                                            </div>
                                                <div style="color:red; font-weight:bold; margin-bottom:5px;">
                                                    Note: Please click <b>Submit</b> only for proper saving of the data after adding new shedule .
                                                </div>

                                            <div class="clearfix"></div>
                                            <ul style="display: flex; list-style-type: none; justify-content: space-evenly;">

                                                <label style="color: black; font: bold">TIME </label>
                                                <label style="color: black; font: bold">ACTIVITIES</label>
                                                <label style="color: black; font: bold">COMMENTS </label>
                                            </ul>
                                        </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                <asp:HiddenField ID="textVisibleOption" runat="server" Value="2" />
                                                <div id="option_box_single_choice">
                                                    <div class="form-group row col-sm-12">
                                                        <div class="col-md-10">
                                                            <div class="cloneContainer">
                                                                <asp:Repeater ID="txtSignleChoice" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class='row cloneThisRow <%# cloneClass(Container.ItemIndex, Eval("Option").ToString(), Eval("Option1").ToString(), Eval("Option2").ToString(), Eval("Option3").ToString()) %>'>
                                                                            <div class="col-sm-2">
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <ul class="d-flex" style="display: flex; list-style-type: none;">
                                                                                    <asp:HiddenField ID="txtSI_ID" runat="server" Value='<%#Eval("Option") %>' />
                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtTIME" runat="server" Width="350" Text='<%#Eval("Option1") %>' TextMode="MultiLine"></asp:TextBox></li>

                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtACTIVITIES" runat="server" Width="350" Text='<%#Eval("Option2") %>' TextMode="MultiLine"></asp:TextBox></li>

                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtCOMMENTS" runat="server" Width="350" Text='<%#Eval("Option3") %>' TextMode="MultiLine"></asp:TextBox></li>


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
                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <%--<ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Self Care">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <%--<div class="span12 formRow">
                 <div class="row">
                     <div class="span2">
                         <h5>1. CURRENTLY EATS :</h5>
                     </div>
                     <div class="span8">
                         <asp:CheckBoxList ID="SelfCare_CurrentlyEats" runat="server" CssClass="checkbox span8" SelectionMode="Multiple">
                             <asp:ListItem Value="BREAST MILK">BREAST MILK</asp:ListItem>
                             <asp:ListItem Value="FORMULA">FORMULA</asp:ListItem>
                             <asp:ListItem Value="BABY FOOD">BABY FOOD</asp:ListItem>
                             <asp:ListItem Value="JUNIOR FOOD">JUNIOR FOOD</asp:ListItem>
                             <asp:ListItem Value="MASHED TABLE FOODS">MASHED TABLE FOODS</asp:ListItem>
                             <asp:ListItem Value="TABLE FOOD">TABLE FOOD</asp:ListItem>

                         </asp:CheckBoxList>
                     </div>
                 </div>
             </div>


                                        <div class="formRow">
                                            <div class="span12">
                                                <h5>1. ACTIVITIES :</h5>
                                            </div>
                                            <div class="span12">
                                                <div class="control-label">
                                                    Brushing :
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="SelfCare_Brushing_1" runat="server" onclick="SelfCare_Brushing_1_Click();"
                                                        CssClass="checkboes" Text="Dependent" />
                                                    <asp:CheckBox ID="SelfCare_Brushing_2" runat="server" onclick="SelfCare_Brushing_2_Click();"
                                                        CssClass="checkboes" Text="Assisted" />
                                                    <asp:CheckBox ID="SelfCare_Brushing_3" runat="server" onclick="SelfCare_Brushing_3_Click();"
                                                        CssClass="checkboes" Text="Independent" />
                                                    <script type="text/javascript">
                                                        function SelfCare_Brushing_1_Click() {
                                                            var ctl = $('#<%=SelfCare_Brushing_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SelfCare_Brushing_2.ClientID %>').prop('checked', false);
                                                                 $('#<%=SelfCare_Brushing_3.ClientID %>').prop('checked', false);
                                                             }
                                                        }
                                                        function SelfCare_Brushing_2_Click() {
                                                            var ctl = $('#<%=SelfCare_Brushing_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SelfCare_Brushing_1.ClientID %>').prop('checked', false);
                                                                 $('#<%=SelfCare_Brushing_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function SelfCare_Brushing_3_Click() {
                                                            var ctl = $('#<%=SelfCare_Brushing_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SelfCare_Brushing_1.ClientID %>').prop('checked', false);
                                                                 $('#<%=SelfCare_Brushing_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Bathing :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Bathing_1" runat="server" onclick="SelfCare_Bathing_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Bathing_2" runat="server" onclick="SelfCare_Bathing_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Bathing_3" runat="server" onclick="SelfCare_Bathing_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Bathing_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Bathing_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Bathing_2.ClientID %>').prop('checked', false);
                                                             $('#<%=SelfCare_Bathing_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Bathing_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Brushing_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Bathing_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_Bathing_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Bathing_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Bathing_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Bathing_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Bathing_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Toileting :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Toileting_1" runat="server" onclick="SelfCare_Toileting_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Toileting_2" runat="server" onclick="SelfCare_Toileting_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Toileting_3" runat="server" onclick="SelfCare_Toileting_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Toileting_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Toileting_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Toileting_2.ClientID %>').prop('checked', false);
                                                             $('#<%=SelfCare_Toileting_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Toileting_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Toileting_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Toileting_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_Toileting_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Toileting_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Toileting_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Toileting_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Toileting_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                Dressing :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Dressing_1" runat="server" onclick="SelfCare_Dressing_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Dressing_2" runat="server" onclick="SelfCare_Dressing_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Dressing_3" runat="server" onclick="SelfCare_Dressing_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Dressing_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Dressing_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Dressing_2.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Dressing_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Dressing_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Dressing_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                             $('#<%=SelfCare_Dressing_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Dressing_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Dressing_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Dressing_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Dressing_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Dressing_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                Breakfast :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Breakfast_1" runat="server" onclick="SelfCare_Breakfast_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Breakfast_2" runat="server" onclick="SelfCare_Breakfast_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Breakfast_3" runat="server" onclick="SelfCare_Breakfast_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Breakfast_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Breakfast_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Breakfast_2.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Breakfast_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Breakfast_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Breakfast_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Breakfast_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Breakfast_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Breakfast_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Breakfast_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Breakfast_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Breakfast_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Lunch  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Lunch_1" runat="server" onclick="SelfCare_Lunch_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Lunch_2" runat="server" onclick="SelfCare_Lunch_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Lunch_3" runat="server" onclick="SelfCare_Lunch_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Lunch_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Lunch_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Lunch_2.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Lunch_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Lunch_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Breakfast_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Lunch_1.ClientID %>').prop('checked', false);
                                                             $('#<%=SelfCare_Lunch_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Lunch_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Lunch_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Lunch_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Lunch_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                Snacks  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Snacks_1" runat="server" onclick="SelfCare_Snacks_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Snacks_2" runat="server" onclick="SelfCare_Snacks_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Snacks_3" runat="server" onclick="SelfCare_Snacks_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Snacks_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Snacks_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Snacks_2.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Snacks_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Snacks_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Snacks_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Snacks_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Snacks_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Snacks_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Snacks_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Snacks_1.ClientID %>').prop('checked', false);
                                                                 $('#<%=SelfCare_Snacks_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Dinner  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Dinner_1" runat="server" onclick="SelfCare_Dinner_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Dinner_2" runat="server" onclick="SelfCare_Dinner_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Dinner_3" runat="server" onclick="SelfCare_Dinner_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Dinner_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Dinner_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Dinner_2.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Dinner_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Dinner_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Dinner_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Dinner_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Dinner_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Dinner_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Dinner_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Dinner_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_Dinner_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Getting In Bus  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_GettingInBus_1" runat="server" onclick="SelfCare_GettingInBus_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_GettingInBus_2" runat="server" onclick="SelfCare_GettingInBus_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_GettingInBus_3" runat="server" onclick="SelfCare_GettingInBus_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_GettingInBus_1_Click() {
                                                        var ctl = $('#<%=SelfCare_GettingInBus_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_GettingInBus_2.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_GettingInBus_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_GettingInBus_2_Click() {
                                                        var ctl = $('#<%=SelfCare_GettingInBus_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_GettingInBus_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_GettingInBus_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_GettingInBus_3_Click() {
                                                        var ctl = $('#<%=SelfCare_GettingInBus_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_GettingInBus_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_GettingInBus_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Going To School  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_GoingToSchool_1" runat="server" onclick="SelfCare_GoingToSchool_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_GoingToSchool_2" runat="server" onclick="SelfCare_GoingToSchool_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_GoingToSchool_3" runat="server" onclick="SelfCare_GoingToSchool_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_GoingToSchool_1_Click() {
                                                        var ctl = $('#<%=SelfCare_GoingToSchool_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                             $('#<%=SelfCare_GoingToSchool_2.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_GoingToSchool_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_GoingToSchool_2_Click() {
                                                        var ctl = $('#<%=SelfCare_GoingToSchool_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                             $('#<%=SelfCare_GoingToSchool_1.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_GoingToSchool_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_GoingToSchool_3_Click() {
                                                        var ctl = $('#<%=SelfCare_GoingToSchool_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_GoingToSchool_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_GoingToSchool_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Come Back From School  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_comeBackSchool_1" runat="server" onclick="SelfCare_comeBackSchool_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_comeBackSchool_2" runat="server" onclick="SelfCare_comeBackSchool_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_comeBackSchool_3" runat="server" onclick="SelfCare_comeBackSchool_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_comeBackSchool_1_Click() {
                                                        var ctl = $('#<%=SelfCare_comeBackSchool_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_comeBackSchool_2.ClientID %>').prop('checked', false);
                                                               $('#<%=SelfCare_comeBackSchool_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_comeBackSchool_2_Click() {
                                                        var ctl = $('#<%=SelfCare_comeBackSchool_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_comeBackSchool_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_comeBackSchool_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_comeBackSchool_3_Click() {
                                                        var ctl = $('#<%=SelfCare_GoingToSchool_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_comeBackSchool_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_comeBackSchool_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                Ambulation  :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SelfCare_Ambulation_1" runat="server" onclick="SelfCare_Ambulation_1_Click();"
                                                    CssClass="checkboes" Text="Dependent" />
                                                <asp:CheckBox ID="SelfCare_Ambulation_2" runat="server" onclick="SelfCare_Ambulation_2_Click();"
                                                    CssClass="checkboes" Text="Assisted" />
                                                <asp:CheckBox ID="SelfCare_Ambulation_3" runat="server" onclick="SelfCare_Ambulation_3_Click();"
                                                    CssClass="checkboes" Text="Independent" />
                                                <script type="text/javascript">
                                                    function SelfCare_Ambulation_1_Click() {
                                                        var ctl = $('#<%=SelfCare_Ambulation_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Ambulation_2.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Ambulation_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Ambulation_2_Click() {
                                                        var ctl = $('#<%=SelfCare_Ambulation_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Ambulation_1.ClientID %>').prop('checked', false);
                                                              $('#<%=SelfCare_Ambulation_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SelfCare_Ambulation_3_Click() {
                                                        var ctl = $('#<%=SelfCare_Ambulation_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SelfCare_Ambulation_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SelfCare_Ambulation_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Homeostatic changes :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SelfCare_Homeostaticchanges" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <div class="clearfix"></div>
                                            <span class="char-limit-msg"></span>
                                            <%--</div>
                                            <div class="clearfix">
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Urination details and Bed wetting etc:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SelfCare_UrinationdetailsBedwetting" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <div class="clearfix"></div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Self_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <%--</div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>--%>

                        <ajaxToolkit:TabPanel ID="tb_Report6" runat="server" HeaderText="FAMILY STRUCTURE">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <%--<div class="span12 formRow">--%>
                                    <div class=" formRow">


                                        <div class="span12">
                                            <div class="control-label">
                                                1.Mother's quality time spent with the child daily. 
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_1" runat="server" onclick="FamilyStructure_QualityTimeMother_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_2" runat="server" onclick="FamilyStructure_QualityTimeMother_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_3" runat="server" onclick="FamilyStructure_QualityTimeMother_3_Click();"
                                                    CssClass="checkboes" Text="24X7" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_QualityTimeMother_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeMother_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeMother_2.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeMother_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_QualityTimeMother_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeMother_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeMother_1.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeMother_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_QualityTimeMother_3_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeMother_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeMother_1.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeMother_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Father's quality time spent with the child daily. 
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_1" runat="server" onclick="FamilyStructure_QualityTimeFather_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_2" runat="server" onclick="FamilyStructure_QualityTimeFather_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_3" runat="server" onclick="FamilyStructure_QualityTimeFather_3_Click();"
                                                    CssClass="checkboes" Text="24X7" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_QualityTimeFather_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeFather_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeFather_2.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeFather_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_QualityTimeFather_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeFather_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeFather_1.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeFather_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_QualityTimeFather_3_Click() {
                                                        var ctl = $('#<%=FamilyStructure_QualityTimeFather_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_QualityTimeFather_1.ClientID %>').prop('checked', false);
                                                            $('#<%=FamilyStructure_QualityTimeFather_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Mother's quality time spent on Sunday/ Weekends with Child.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Mother_Weekends_1" runat="server" onclick="Mother_Weekends_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" />
                                                <asp:CheckBox ID="Mother_Weekends_2" runat="server" onclick="Mother_Weekends_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" />
                                                <asp:CheckBox ID="Mother_Weekends_3" runat="server" onclick="Mother_Weekends_3_Click();"
                                                    CssClass="checkboes" Text="24X7" />
                                                <script type="text/javascript">
                                                    function Mother_Weekends_1_Click() {
                                                        var ctl = $('#<%=Mother_Weekends_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Mother_Weekends_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Mother_Weekends_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Mother_Weekends_2_Click() {
                                                        var ctl = $('#<%=Mother_Weekends_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Mother_Weekends_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Mother_Weekends_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Mother_Weekends_3_Click() {
                                                        var ctl = $('#<%=Mother_Weekends_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Mother_Weekends_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Mother_Weekends_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Father's quality time spent on Sunday/ Weekends with Child.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Father_Weekends_1" runat="server" onclick="Father_Weekends_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" />
                                                <asp:CheckBox ID="Father_Weekends_2" runat="server" onclick="Father_Weekends_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" />
                                                <asp:CheckBox ID="Father_Weekends_3" runat="server" onclick="Father_Weekends_3_Click();"
                                                    CssClass="checkboes" Text="24X7" />
                                                <script type="text/javascript">
                                                    function Father_Weekends_1_Click() {
                                                        var ctl = $('#<%=Father_Weekends_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Father_Weekends_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Father_Weekends_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Father_Weekends_2_Click() {
                                                        var ctl = $('#<%=Father_Weekends_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Father_Weekends_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Father_Weekends_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Father_Weekends_3_Click() {
                                                        var ctl = $('#<%=Father_Weekends_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Father_Weekends_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Father_Weekends_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5. Willingness to devote time for therapy
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_TimeForThreapy_1" runat="server" onclick="FamilyStructure_TimeForThreapy_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStructure_TimeForThreapy_2" runat="server" onclick="FamilyStructure_TimeForThreapy_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_TimeForThreapy_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_TimeForThreapy_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_TimeForThreapy_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_TimeForThreapy_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_TimeForThreapy_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_TimeForThreapy_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6. Acceptance of the condition
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_AcceptanceCondition_1" runat="server" onclick="FamilyStructure_AcceptanceCondition_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStructure_AcceptanceCondition_2" runat="server" onclick="FamilyStructure_AcceptanceCondition_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_AcceptanceCondition_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_AcceptanceCondition_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_AcceptanceCondition_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_AcceptanceCondition_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_AcceptanceCondition_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_AcceptanceCondition_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7. Accessibility to play areas/Extracurricular activities
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_ExtraCaricular_1" runat="server" onclick="FamilyStructure_ExtraCaricular_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStructure_ExtraCaricular_2" runat="server" onclick="FamilyStructure_ExtraCaricular_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_ExtraCaricular_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_ExtraCaricular_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_ExtraCaricular_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_ExtraCaricular_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_ExtraCaricular_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_ExtraCaricular_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <%-- <div class="span12">
                         <div class="control-label">
                             8.Mother Working
                         </div>

                         <div class="control-group" style="padding-left: 20px">
                             <asp:CheckBox ID="Mother_Working_1" runat="server" onclick="Mother_Working_1_Click();"
                                 CssClass="checkboes" Text=" Yes" />
                             <asp:CheckBox ID="Mother_Working_2" runat="server" onclick="Mother_Working_2_Click();"
                                 CssClass="checkboes" Text="No" />
                             <script type="text/javascript">
                                 function Mother_Working_1_Click() {
                                     var ctl = $('#<%=Mother_Working_1.ClientID %>')[0];
                         if (ctl.checked) {
                             $('#<%=Mother_Working_2.ClientID %>').prop('checked', false);
                                     }
                                 }
                                 function Mother_Working_2_Click() {
                                     var ctl = $('#<%=Mother_Working_2.ClientID %>')[0];
                         if (ctl.checked) {
                             $('#<%=Mother_Working_1.ClientID %>').prop('checked', false);
                                     }
                                 }
                             </script>
                         </div>
                     </div>

                     <div class="span12">
                         <div class="control-label">
                             9.Father Working
                         </div>
                         <div class="control-group" style="padding-left: 20px">
                             <asp:CheckBox ID="Father_Working_1" runat="server" onclick="Father_Working_1_Click();"
                                 CssClass="checkboes" Text=" Yes" />
                             <asp:CheckBox ID="Father_Working_2" runat="server" onclick="Father_Working_2_Click();"
                                 CssClass="checkboes" Text="No" />
                             <script type="text/javascript">
                                 function Father_Working_1_Click() {
                                     var ctl = $('#<%=Father_Working_1.ClientID %>')[0];
                         if (ctl.checked) {
                             $('#<%=Father_Working_2.ClientID %>').prop('checked', false);
                                     }
                                 }
                                 function Father_Working_2_Click() {
                                     var ctl = $('#<%=Father_Working_2.ClientID %>')[0];
                         if (ctl.checked) {
                             $('#<%=Father_Working_1.ClientID %>').prop('checked', false);
                                     }
                                 }
                             </script>
                         </div>
                     </div>

                     <div class="span12">
                         <div class="control-label">
                             10.Househelp for child
                         </div>
                         <div class="control-group" style="padding-left: 20px">
                             <asp:CheckBox ID="Househelp_1" runat="server" onclick="Househelp_1_Click();"
                                 CssClass="checkboes" Text=" Yes" />
                             <asp:CheckBox ID="Househelp_2" runat="server" onclick="Househelp_2_Click();"
                                 CssClass="checkboes" Text="No" />
                             <script type="text/javascript">
                                 function Househelp_1_Click() {
                                     var ctl = $('#<%=Househelp_1.ClientID %>')[0];
                                 if (ctl.checked) {
                                     $('#<%=Househelp_2.ClientID %>').prop('checked', false);
                                     }
                                 }
                                 function Father_Working_2_Click() {
                                     var ctl = $('#<%=Househelp_2.ClientID %>')[0];
                                 if (ctl.checked) {
                                     $('#<%=Househelp_1.ClientID %>').prop('checked', false);
                                     }
                                 }
                             </script>
                         </div>
                     </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Disciplinary measures taken
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_Diciplinary" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%--<div class="span12">
                                            <div class="control-label">
                                                9.Relationship With Siblings.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_SiblingBrother_1" runat="server" onclick="FamilyStructure_SiblingBrother_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStructure_SiblingBrother_2" runat="server" onclick="FamilyStructure_SiblingBrother_1_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_SiblingBrother_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_SiblingBrother_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_SiblingBrother_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_SiblingBrother_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_SiblingBrother_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_SiblingBrother_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>--%>
                                        <%--<div class="span12">
                                            <div class="control-label">
                                                9.Relationship With Siblings.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_SiblingBrother" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>--%>
                                        <div class="span12">
                                            <div class="control-label">
                                                9.Relationship with siblings.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_SiblingBrother" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="10">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%--                                        <div class="span12">
                                            <div class="control-label">
                                                10.Sister sibling and cohabitation
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_SiblingSister_1" runat="server" onclick="FamilyStructure_SiblingSister_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStructure_SiblingSister_2" runat="server" onclick="FamilyStructure_SiblingSister_1_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function FamilyStructure_SiblingSister_1_Click() {
                                                        var ctl = $('#<%=FamilyStructure_SiblingSister_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_SiblingSister_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStructure_SiblingSister_2_Click() {
                                                        var ctl = $('#<%=FamilyStructure_SiblingSister_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStructure_SiblingSister_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>--%>

                                        <%--   <div class="span12">
                                            <div class="control-label">
                                            </div>
                                            11.NA sibling and cohabitation
                     <div class="control-group" style="padding-left: 20px">
                         <asp:CheckBox ID="FamilyStructure_SiblingNA_1" runat="server" onclick="FamilyStructure_SiblingNA_1_Click();"
                             CssClass="checkboes" Text=" Yes" />
                         <asp:CheckBox ID="FamilyStructure_SiblingNA_2" runat="server" onclick="FamilyStructure_SiblingNA_2_Click();"
                             CssClass="checkboes" Text="No" />
                         <script type="text/javascript">
                             function FamilyStructure_SiblingNA_1_Click() {
                                 var ctl = $('#<%=FamilyStructure_SiblingNA_1.ClientID %>')[0];
                                 if (ctl.checked) {
                                     $('#<%=FamilyStructure_SiblingNA_2.ClientID %>').prop('checked', false);
                                 }
                             }
                             function FamilyStructure_SiblingNA_2_Click() {
                                 var ctl = $('#<%=FamilyStructure_SiblingNA_2.ClientID %>')[0];
                                 if (ctl.checked) {
                                     $('#<%=FamilyStructure_SiblingNA_1.ClientID %>').prop('checked', false);
                                 }
                             }
                         </script>
                     </div>
                                        </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                10.Expectations from the child's performance

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_Expectations" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>


                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11. Others closely involved with:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_CloselyInvolved" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FAMILY_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>

                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report7" runat="server" HeaderText="SCHOOL INFORMATION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <%--<div class="span12">
                          <div class="control-label">
                              A) Physical School
                          </div>
                      1.Does the child attend school
                         
                          <div class="control-group">
                              <asp:TextBox ID="Schoolinfo_Attend" runat="server" CssClass="span10" TextMode="MultiLine"
                                  Rows="8">
                              </asp:TextBox>
                          </div>
                       
                      </div>--%>
                                        <div class="span12">
                                            <div class="control-label">
                                                <h6>A) Physical school</h6>
                                            </div>
                                            <div class="control-label">
                                                1.Does the child attend school
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Attend_1" runat="server" onclick="Schoolinfo_Attend_1_Click();"
                                                    CssClass="checkboes" Text="Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Attend_2" runat="server" onclick="Schoolinfo_Attend_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Attend_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Attend_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Attend_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Attend_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Attend_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Attend_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                <%--2.Type of school--%>
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Type_1" runat="server" onclick="Schoolinfo_Type_1_Click();"
                                                    CssClass="checkboes" Text="Open" />
                                                <asp:CheckBox ID="Schoolinfo_Type_2" runat="server" onclick="Schoolinfo_Type_2_Click();"
                                                    CssClass="checkboes" Text="Integrated" />
                                                <asp:CheckBox ID="Schoolinfo_Type_3" runat="server" onclick="Schoolinfo_Type_3_Click();"
                                                    CssClass="checkboes" Text="Special" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Type_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Type_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Type_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Type_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Type_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Type_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Type_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Type_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Type_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Type_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Type_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Type_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12 fromRow">
                                            <div class="row">
                                                <div class="span2">
                                                    2.Number of hours:
                                                </div>
                                                <asp:DropDownList ID="Schoolinfo_SchoolHours" runat="server" CssClass="input-medium chzn-select span2">
                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                    <asp:ListItem Value="1">1 to 5 Hour</asp:ListItem>
                                                    <asp:ListItem Value="2">More Than 5 Hours</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    3. How do they travel:
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                        <asp:CheckBox ID="chkSchool_Bus" runat="server" CssClass="checkboes" onclick="School_Bus_Click();" Text="School_bus" />
                                                        <asp:CheckBox ID="chkCar" runat="server" CssClass="checkboes" onclick="Car_Click();" Text="Car" />
                                                        <asp:CheckBox ID="chkTwo_Wheelers" runat="server" CssClass="checkboes" onclick="Two_Wheelers_Click();" Text="Two_wheelers" />
                                                        <asp:CheckBox ID="chkwalking" runat="server" CssClass="checkboes" onclick="walking_Click();" Text="Walking" />
                                                        <asp:CheckBox ID="chkPublic_Transport" runat="server" CssClass="checkboes" onclick="Public_Transport_Click();" Text="Public_transport" />>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                4.	Teacher to child ratio
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_1" runat="server" onclick="Schoolinfo_NoOfTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_2" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text=" 1 to 30" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_3" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text="1 to 60" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_4" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text="	more than 60" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_NoOfTeacher_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_NoOfTeacher_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_NoOfTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_NoOfTeacher_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_NoOfTeacher_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_NoOfTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_NoOfTeacher_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_NoOfTeacher_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_NoOfTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_NoOfTeacher_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_NoOfTeacher_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_NoOfTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_NoOfTeacher_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>




                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    5.Seating arrangement:
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                        <asp:CheckBox ID="chkFloor" runat="server" CssClass="checkboes" onclick="Floor_Click();" Text="Floor" />
                                                        <asp:CheckBox ID="chksingle_bench" runat="server" CssClass="checkboes" onclick="single_bench_Click();" Text="Single_bench" />
                                                        <asp:CheckBox ID="chkbench2" runat="server" CssClass="checkboes" onclick="bench2_Click();" Text="Bench2" />
                                                        <asp:CheckBox ID="chkround_table" runat="server" CssClass="checkboes" onclick="round_table_Click();" Text="Round_table" />


                                                        <script type="text/javascript">
                                                            function Floor_Click() {
                                                                var ctl = $('#<%=chkFloor.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=chksingle_bench.ClientID %>').prop('checked', false);
                                                                    $('#<%=chkbench2.ClientID %>').prop('checked', false);
                                                                    $('#<%=chkround_table.ClientID %>').prop('checked', false);

                                                                }
                                                            }
                                                            function single_bench_Click() {
                                                                var ctl = $('#<%=chksingle_bench.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=chkFloor.ClientID %>').prop('checked', false);
                                                                    $('#<%=chkbench2.ClientID %>').prop('checked', false);
                                                                    $('#<%=chkround_table.ClientID %>').prop('checked', false);

                                                                }
                                                            }
                                                            function bench2_Click() {
                                                                var ctl = $('#<%=chkbench2.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=chksingle_bench.ClientID %>').prop('checked', false);
                                                                     $('#<%=chkFloor.ClientID %>').prop('checked', false);
                                                                     $('#<%=chkround_table.ClientID %>').prop('checked', false);

                                                                }
                                                            }
                                                            function round_table_Click() {
                                                                var ctl = $('#<%=chkround_table.ClientID %>')[0];
                                                                if (ctl.checked) {
                                                                    $('#<%=chksingle_bench.ClientID %>').prop('checked', false);
                                                                     $('#<%=chkbench2.ClientID %>').prop('checked', false);
                                                                     $('#<%=chkFloor.ClientID %>').prop('checked', false);

                                                                }
                                                            }


                                                        </script>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span12 ">
                                            <div class="row">
                                                <div class="span2">
                                                    6.Meal time at the school:
                                                </div>
                                                <asp:DropDownList ID="Schoolinfo_Mealtime" runat="server" Height="30px" Width="132px">
                                                    <asp:ListItem Value="0">Select </asp:ListItem>
                                                    <asp:ListItem Value="01">1</asp:ListItem>
                                                    <asp:ListItem Value="02">2</asp:ListItem>
                                                    <asp:ListItem Value="03">3</asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7.Meal type
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_MealType_1" runat="server" onclick="Schoolinfo_MealType_1_Click();"
                                                    CssClass="checkboes" Text=" Provided by school" />
                                                <asp:CheckBox ID="Schoolinfo_MealType_2" runat="server" onclick="Schoolinfo_MealType_2_Click();"
                                                    CssClass="checkboes" Text="Tiffin carried from home" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_MealType_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_MealType_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_MealType_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_MealType_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_MealType_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_MealType_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Sharing done by child
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Shareing_1" runat="server" onclick="Schoolinfo_Shareing_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Shareing_2" runat="server" onclick="Schoolinfo_Shareing_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_Shareing_3" runat="server" onclick="Schoolinfo_Shareing_3_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Shareing_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Shareing_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Shareing_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Shareing_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Shareing_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Shareing_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Shareing_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Shareing_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Shareing_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                9.Help required in eating
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_HelpEating_1" runat="server" onclick="Schoolinfo_HelpEating_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_HelpEating_2" runat="server" onclick="Schoolinfo_HelpEating_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_HelpEating_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_HelpEating_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_HelpEating_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_HelpEating_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_HelpEating_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_HelpEating_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                10.Friendships initiated by child
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Friendship_1" runat="server" onclick="Schoolinfo_Friendship_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Friendship_2" runat="server" onclick="Schoolinfo_Friendship_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Friendship_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Friendship_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Friendship_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Friendship_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Friendship_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Friendship_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11.Interaction with peers
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_InteractionPeer_1" runat="server" onclick="Schoolinfo_InteractionPeer_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_InteractionPeer_2" runat="server" onclick="Schoolinfo_InteractionPeer_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_HelpEating_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_InteractionPeer_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_InteractionPeer_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_HelpEating_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_InteractionPeer_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_InteractionPeer_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                12.Interaction with the teacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_InteractionTeacher_1" runat="server" onclick="Schoolinfo_InteractionTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_InteractionTeacher_2" runat="server" onclick="Schoolinfo_InteractionTeacher_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_InteractionTeacher_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_InteractionTeacher_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_InteractionTeacher_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_InteractionTeacher_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_InteractionTeacher_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_InteractionTeacher_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                13.Annuals/culturals function
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_AnnualFunction_1" runat="server" onclick="Schoolinfo_AnnualFunction_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_AnnualFunction_2" runat="server" onclick="Schoolinfo_AnnualFunction_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_AnnualFunction_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_AnnualFunction_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_AnnualFunction_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_AnnualFunction_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_AnnualFunction_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_AnnualFunction_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                14.Sports.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Sports_1" runat="server" onclick="Schoolinfo_Sports_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Sports_2" runat="server" onclick="Schoolinfo_Sports_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Sports_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Sports_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Sports_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Sports_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Sports_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Sports_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                15.Picnics/Field trips
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Picnic_1" runat="server" onclick="Schoolinfo_Picnic_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Picnic_2" runat="server" onclick="Schoolinfo_Picnic_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_Picnic_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Picnic_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Picnic_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Picnic_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Picnic_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Picnic_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                16.Extra curricular
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_ExtraCaricular_1" runat="server" onclick="Schoolinfo_ExtraCaricular_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_ExtraCaricular_2" runat="server" onclick="Schoolinfo_ExtraCaricular_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_ExtraCaricular_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ExtraCaricular_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ExtraCaricular_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_ExtraCaricular_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ExtraCaricular_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ExtraCaricular_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                17.Copying from board
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_1" runat="server" onclick="Schoolinfo_CopyBoard_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_2" runat="server" onclick="Schoolinfo_CopyBoard_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_3" runat="server" onclick="Schoolinfo_CopyBoard_3_Click();"
                                                    CssClass="checkboes" Text="Inconsistent" />
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_4" runat="server" onclick="Schoolinfo_CopyBoard_4_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_CopyBoard_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CopyBoard_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CopyBoard_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CopyBoard_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CopyBoard_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CopyBoard_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CopyBoard_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CopyBoard_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CopyBoard_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CopyBoard_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CopyBoard_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CopyBoard_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CopyBoard_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                18.Follows instructions
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Instructions_1" runat="server" onclick="Schoolinfo_Instructions_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_Instructions_2" runat="server" onclick="Schoolinfo_Instructions_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_Instructions_3" runat="server" onclick="Schoolinfo_Instructions_3_Click();"
                                                    CssClass="checkboes" Text="Sometime" />
                                                <asp:CheckBox ID="Schoolinfo_Instructions_4" runat="server" onclick="Schoolinfo_Instructions_4_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_CopyBoard_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Instructions_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Instructions_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Instructions_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Instructions_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Instructions_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Instructions_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Instructions_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Instructions_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_Instructions_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_Instructions_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_Instructions_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_Instructions_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                19.Shadow teacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_1" runat="server" onclick="Schoolinfo_ShadowTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_2" runat="server" onclick="Schoolinfo_ShadowTeacher_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_3" runat="server" onclick="Schoolinfo_ShadowTeacher_3_Click();"
                                                    CssClass="checkboes" Text="Needs Help" />
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_4" runat="server" onclick="Schoolinfo_ShadowTeacher_4_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_ShadowTeacher_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ShadowTeacher_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ShadowTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_ShadowTeacher_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ShadowTeacher_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ShadowTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_ShadowTeacher_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ShadowTeacher_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ShadowTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_ShadowTeacher_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_ShadowTeacher_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_ShadowTeacher_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_ShadowTeacher_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                20.Completing CW/HW
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_1" runat="server" onclick="Schoolinfo_CW_HW_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_2" runat="server" onclick="Schoolinfo_CW_HW_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_3" runat="server" onclick="Schoolinfo_CW_HW_3_Click();"
                                                    CssClass="checkboes" Text="Needs Help" />
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_4" runat="server" onclick="Schoolinfo_CW_HW_4_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_CW_HW_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CW_HW_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CW_HW_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CW_HW_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CW_HW_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CW_HW_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CW_HW_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CW_HW_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CW_HW_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_CW_HW_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_CW_HW_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_CW_HW_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_CW_HW_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                21.Provision of special educator/Shadow/ Remedial teacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_1" runat="server" onclick="Schoolinfo_SpecialEducator_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_2" runat="server" onclick="Schoolinfo_SpecialEducator_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_3" runat="server" onclick="Schoolinfo_SpecialEducator_3_Click();"
                                                    CssClass="checkboes" Text="NA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_SpecialEducator_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_SpecialEducator_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_SpecialEducator_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_SpecialEducator_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_SpecialEducator_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_SpecialEducator_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_SpecialEducator_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_SpecialEducator_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_SpecialEducator_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_SpecialEducator_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_SpecialEducator_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_SpecialEducator_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                22.Mode of delivery of information
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_1" runat="server" onclick="Schoolinfo_DeliveryInformation_1_Click();"
                                                    CssClass="checkboes" Text=" PPT" />
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_2" runat="server" onclick="Schoolinfo_DeliveryInformation_2_Click();"
                                                    CssClass="checkboes" Text="Videos" />
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_3" runat="server" onclick="Schoolinfo_DeliveryInformation_3_Click();"
                                                    CssClass="checkboes" Text="Books" />
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_4" runat="server" onclick="Schoolinfo_DeliveryInformation_3_Click();"
                                                    CssClass="checkboes" Text="NOTA" />
                                                <script type="text/javascript">
                                                    function Schoolinfo_DeliveryInformation_1_Click() {
                                                        var ctl = $('#<%=Schoolinfo_DeliveryInformation_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_DeliveryInformation_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_DeliveryInformation_2_Click() {
                                                        var ctl = $('#<%=Schoolinfo_DeliveryInformation_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_DeliveryInformation_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_3.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_DeliveryInformation_3_Click() {
                                                        var ctl = $('#<%=Schoolinfo_DeliveryInformation_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_DeliveryInformation_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Schoolinfo_DeliveryInformation_4_Click() {
                                                        var ctl = $('#<%=Schoolinfo_DeliveryInformation_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Schoolinfo_DeliveryInformation_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Schoolinfo_DeliveryInformation_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                23.Remark of the teacher

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Schoolinfo_RemarkTeacher" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <%--<div class="clearfix"></div>--%>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SCHOOL_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>


                                        <%--  B) ONLINE SCHOOL
                                            
                                                          
                  <div class="span12 formRow">
        <div class="row">
            <div class="span2">
                <h5>1. What is the platform for interaction ?</h5>
            </div>
            <div class="span8">
                <asp:CheckBoxList ID="Schoolinfo_PlatformInteraction" runat="server" CssClass="checkbox span8" SelectionMode="Multiple">
                    <asp:ListItem Value="Zoom">Zoom</asp:ListItem>
                    <asp:ListItem Value="Google_Meet">Google_Meet</asp:ListItem>
                    <asp:ListItem Value="School_Application">School_Application</asp:ListItem>
                    <asp:ListItem Value="Youtube_videos">Youtube_videos</asp:ListItem>
                    <asp:ListItem Value="Whatsapp_groups">Whatsapp_groups</asp:ListItem>


                </asp:CheckBoxList>
            </div>
        </div>
    </div>

                  <div class="span12">
                       <div class="control-label">
                           2. How many hours of online school?

                       </div>
                       <div class="control-group">
                           <asp:TextBox ID="Schoolinfo_HourOnlineSchool" runat="server" CssClass="span10" TextMode="MultiLine"
                               Rows="8">
                           </asp:TextBox>
                       </div>
                       <%--<div class="clearfix"></div>
                       <span class="char-limit-msg"></span>
                   </div>

                  <div class="span12">
                   <div class="control-label">
                       3. Does your child sit during online school
                       </div>
       <div class="control-group" style="padding-left: 20px">
           <asp:CheckBox ID="Schoolinfo_SitOnlineSchool_1" runat="server" onclick="Schoolinfo_SitOnlineSchool_1_Click();"
               CssClass="checkboes" Text=" Yes" />
           <asp:CheckBox ID="Schoolinfo_SitOnlineSchool_2" runat="server" onclick="Schoolinfo_SitOnlineSchool_2_Click();"
               CssClass="checkboes" Text="No" />
           <asp:CheckBox ID="Schoolinfo_SitOnlineSchool_3" runat="server" onclick="Schoolinfo_SitOnlineSchool_3_Click();"
               CssClass="checkboes" Text="Sometimes" />
           <script type="text/javascript">
               function Schoolinfo_SitOnlineSchool_1_Click() {
                   var ctl = $('#<%=Schoolinfo_SitOnlineSchool_1.ClientID %>')[0];
                   if (ctl.checked) {
                       $('#<%=Schoolinfo_SitOnlineSchool_2.ClientID %>').prop('checked', false);
                   }
               }
               function Schoolinfo_SitOnlineSchool_2_Click() {
                   var ctl = $('#<%=Schoolinfo_SitOnlineSchool_2.ClientID %>')[0];
                   if (ctl.checked) {
                       $('#<%=Schoolinfo_SitOnlineSchool_1.ClientID %>').prop('checked', false);
                   }
               }
               function Schoolinfo_SitOnlineSchool_3_Click() {
                   var ctl = $('#<%=Schoolinfo_SitOnlineSchool_3.ClientID %>')[0];
                   if (ctl.checked) {
                       $('#<%=Schoolinfo_SitOnlineSchool_2.ClientID %>').prop('checked', false);
                   }
               }
           </script>
       </div>
                   </div>

                  <div class="span12">
                       <div class="control-label">
                           4. Does your child follows the teachers instructions?
                           </div>
              <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="Schoolinfo_TeacherInstruction_1" runat="server" onclick="Schoolinfo_TeacherInstruction_1_Click();"
                      CssClass="checkboes" Text=" Yes" />
                  <asp:CheckBox ID="Schoolinfo_TeacherInstruction_2" runat="server" onclick="Schoolinfo_TeacherInstruction_2_Click();"
                      CssClass="checkboes" Text="No" />
                  <asp:CheckBox ID="Schoolinfo_TeacherInstruction_3" runat="server" onclick="Schoolinfo_TeacherInstruction_3_Click();"
                      CssClass="checkboes" Text="Sometimes" />
                  <script type="text/javascript">
                      function Schoolinfo_TeacherInstruction_1_Click() {
                          var ctl = $('#<%=Schoolinfo_TeacherInstruction_1.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=Schoolinfo_TeacherInstruction_2.ClientID %>').prop('checked', false);
                          }
                      }
                      function Schoolinfo_TeacherInstruction_2_Click() {
                          var ctl = $('#<%=Schoolinfo_TeacherInstruction_2.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=Schoolinfo_TeacherInstruction_1.ClientID %>').prop('checked', false);
                          }
                      }
                      function Schoolinfo_TeacherInstruction_3_Click() {
                          var ctl = $('#<%=Schoolinfo_TeacherInstruction_3.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=Schoolinfo_TeacherInstruction_2.ClientID %>').prop('checked', false);
                          }
                      }
                  </script>
              </div>
                       </div>

                  <div class="span12">
                           <div class="control-label">
                               5. How is the set up at home during your childs online school?
                           </div>
                           <div class="control-group">
                               <asp:TextBox ID="Schoolinfo_SetUp" runat="server" CssClass="span10" TextMode="MultiLine"
                                   Rows="8">
                               </asp:TextBox>
                           </div>
                           <%--<div class="clearfix"></div>
                           <span class="char-limit-msg"></span>
                       </div>
 
                  <div class="span12">
                       <div class="control-label">
                           6. Does your child show any behavior during online school?
   
   
                       </div>
                       <div class="control-group">
                           <asp:TextBox ID="Schoolinfo_BehaviourOnlineSchool" runat="server" CssClass="span10" TextMode="MultiLine"
                               Rows="8">
                           </asp:TextBox>
                       </div>
                      
                       <span class="char-limit-msg"></span>
                   </div>  --%>


                                        <div class="clearfix"></div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="PERSONAL SOCIAL">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <%--<div class="span12 formRow">--%>
                                    <div class="formRow">

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    <h5>A) Relationship with self</h5>
                                                </div>
                                            </div>

                                            1. Does he/she know the current place? :
              
              <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_1" runat="server" onclick="PersonalSocial_CurrentPlace_1_Click();"
                      CssClass="checkboes" Text=" Yes" />
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_2" runat="server" onclick="PersonalSocial_CurrentPlace_2_Click();"
                      CssClass="checkboes" Text="No" />
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_3" runat="server" onclick="PersonalSocial_CurrentPlace_3_Click();"
                      CssClass="checkboes" Text="Sometimes" />
                  <script type="text/javascript">
                      function PersonalSocial_CurrentPlace_1_Click() {
                          var ctl = $('#<%=PersonalSocial_CurrentPlace_1.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_CurrentPlace_2.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_CurrentPlace_3.ClientID %>').prop('checked', false);
                          }
                      }
                      function PersonalSocial_CurrentPlace_2_Click() {
                          var ctl = $('#<%=PersonalSocial_CurrentPlace_2.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_CurrentPlace_1.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_CurrentPlace_3.ClientID %>').prop('checked', false);
                          }
                      }
                      function PersonalSocial_CurrentPlace_3_Click() {
                          var ctl = $('#<%=PersonalSocial_CurrentPlace_3.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_CurrentPlace_1.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_CurrentPlace_2.ClientID %>').prop('checked', false);
                          }
                      }
                  </script>
              </div>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row"></div>
                                            2. Is your child aware of what he/she does? :
                                        
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_1" runat="server" onclick="PersonalSocial_WhatHeDoes_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_2" runat="server" onclick="PersonalSocial_WhatHeDoes_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_3" runat="server" onclick="PersonalSocial_WhatHeDoes_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function PersonalSocial_WhatHeDoes_1_Click() {
                                                            var ctl = $('#<%=PersonalSocial_WhatHeDoes_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_WhatHeDoes_2.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_WhatHeDoes_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_WhatHeDoes_2_Click() {
                                                            var ctl = $('#<%=PersonalSocial_WhatHeDoes_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_WhatHeDoes_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_WhatHeDoes_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_WhatHeDoes_3_Click() {
                                                            var ctl = $('#<%=PersonalSocial_WhatHeDoes_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_WhatHeDoes_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_WhatHeDoes_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>

                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row"></div>

                                            3. Does the child have own body awareness?
                                                  
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_1" runat="server" onclick="PersonalSocial_CurrentPlace_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_2" runat="server" onclick="PersonalSocial_CurrentPlace_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_3" runat="server" onclick="PersonalSocial_CurrentPlace_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function PersonalSocial_BodyAwareness_1_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodyAwareness_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodyAwareness_2.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodyAwareness_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_BodyAwareness_2_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodyAwareness_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodyAwareness_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodyAwareness_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_BodyAwareness_3_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodyAwareness_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodyAwareness_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodyAwareness_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>

                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row"></div>

                                            4.Is your child aware of body schema?
                                            
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_1" runat="server" onclick="PersonalSocial_BodySchema_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_2" runat="server" onclick="PersonalSocial_BodySchema_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_3" runat="server" onclick="PersonalSocial_BodySchema_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function PersonalSocial_BodySchema_1_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodySchema_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodySchema_2.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodySchema_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_BodySchema_2_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodySchema_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodySchema_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodySchema_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_BodySchema_3_Click() {
                                                            var ctl = $('#<%=PersonalSocial_BodySchema_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_BodySchema_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_BodySchema_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>

                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row"></div>

                                            5.  Does your child self explores the environment?
                                            
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_1" runat="server" onclick="PersonalSocial_ExploreEnvironment_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_2" runat="server" onclick="PersonalSocial_ExploreEnvironment_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_3" runat="server" onclick="PersonalSocial_ExploreEnvironment_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function PersonalSocial_ExploreEnvironment_1_Click() {
                                                            var ctl = $('#<%=PersonalSocial_ExploreEnvironment_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_ExploreEnvironment_2.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_ExploreEnvironment_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_ExploreEnvironment_2_Click() {
                                                            var ctl = $('#<%=PersonalSocial_ExploreEnvironment_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_ExploreEnvironment_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_ExploreEnvironment_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_ExploreEnvironment_3_Click() {
                                                            var ctl = $('#<%=PersonalSocial_ExploreEnvironment_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_ExploreEnvironment_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_ExploreEnvironment_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>

                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row"></div>

                                            6. Is your child motivated?
                                                
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_1" runat="server" onclick="PersonalSocial_Motivated_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_2" runat="server" onclick="PersonalSocial_Motivated_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_3" runat="server" onclick="PersonalSocial_Motivated_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function PersonalSocial_Motivated_1_Click() {
                                                            var ctl = $('#<%=PersonalSocial_Motivated_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_Motivated_2.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_Motivated_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_Motivated_2_Click() {
                                                            var ctl = $('#<%=PersonalSocial_Motivated_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_Motivated_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_Motivated_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function PersonalSocial_Motivated_3_Click() {
                                                            var ctl = $('#<%=PersonalSocial_Motivated_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=PersonalSocial_Motivated_1.ClientID %>').prop('checked', false);
                                                                $('#<%=PersonalSocial_Motivated_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>

                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    <h5>B)  Relationship with others</h5>
                                                </div>
                                            </div>
                                            1. Eye contact
                          
              <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="PersonalSocial_EyeContact_1" runat="server" onclick="PersonalSocial_EyeContact_1_Click();"
                      CssClass="checkboes" Text="Fleeting" />
                  <asp:CheckBox ID="PersonalSocial_EyeContact_2" runat="server" onclick="PersonalSocial_EyeContact_2_Click();"
                      CssClass="checkboes" Text="Poor" />
                  <asp:CheckBox ID="PersonalSocial_EyeContact_3" runat="server" onclick="PersonalSocial_EyeContact_3_Click();"
                      CssClass="checkboes" Text="Fair" />
                  <asp:CheckBox ID="PersonalSocial_EyeContact_4" runat="server" onclick="PersonalSocial_EyeContact_4_Click();"
                      CssClass="checkboes" Text="Good" />
                  <script type="text/javascript">
                      function PersonalSocial_Motivated_1_Click() {
                          var ctl = $('#<%=PersonalSocial_EyeContact_1.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_EyeContact_2.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_3.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_4.ClientID %>').prop('checked', false);
                          }
                      }
                      function PersonalSocial_EyeContact_2_Click() {
                          var ctl = $('#<%=PersonalSocial_EyeContact_2.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_EyeContact_1.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_3.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_4.ClientID %>').prop('checked', false);
                          }
                      }
                      function PersonalSocial_EyeContact_3_Click() {
                          var ctl = $('#<%=PersonalSocial_EyeContact_3.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_EyeContact_1.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_2.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_4.ClientID %>').prop('checked', false);
                          }
                      }
                      function PersonalSocial_EyeContact_4_Click() {
                          var ctl = $('#<%=PersonalSocial_EyeContact_4.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=PersonalSocial_EyeContact_1.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_2.ClientID %>').prop('checked', false);
                              $('#<%=PersonalSocial_EyeContact_3.ClientID %>').prop('checked', false);
                          }
                      }
                  </script>
              </div>
                                        </div>
                                        <%--</div>--%>

                                        <div class="span12 ">
                                            <div class="control-label">
                                                2. Social smile
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="PersonalSocial_SocialSmile_1" runat="server" onclick="PersonalSocial_SocialSmile_1_Click();"
                                                    CssClass="checkboes" Text="Fleeting" />
                                                <asp:CheckBox ID="PersonalSocial_SocialSmile_2" runat="server" onclick="PersonalSocial_SocialSmile_2_Click();"
                                                    CssClass="checkboes" Text="Poor" />
                                                <asp:CheckBox ID="PersonalSocial_SocialSmile_3" runat="server" onclick="PersonalSocial_SocialSmile_3_Click();"
                                                    CssClass="checkboes" Text="Fair" />
                                                <asp:CheckBox ID="PersonalSocial_SocialSmile_4" runat="server" onclick="PersonalSocial_SocialSmile_4_Click();"
                                                    CssClass="checkboes" Text="Good" />
                                                <script type="text/javascript">
                                                    function PersonalSocial_SocialSmile_1_Click() {
                                                        var ctl = $('#<%=PersonalSocial_SocialSmile_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_SocialSmile_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_SocialSmile_2_Click() {
                                                        var ctl = $('#<%=PersonalSocial_SocialSmile_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_SocialSmile_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_SocialSmile_3_Click() {
                                                        var ctl = $('#<%=PersonalSocial_SocialSmile_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_SocialSmile_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_SocialSmile_4_Click() {
                                                        var ctl = $('#<%=PersonalSocial_SocialSmile_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_SocialSmile_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_SocialSmile_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                3. Family regards
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="PersonalSocial_FamilyRegards_1" runat="server" onclick="PersonalSocial_FamilyRegards_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="PersonalSocial_FamilyRegards_2" runat="server" onclick="PersonalSocial_FamilyRegards_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function PersonalSocial_FamilyRegards_1_Click() {
                                                        var ctl = $('#<%=PersonalSocial_FamilyRegards_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_FamilyRegards_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_FamilyRegards_2_Click() {
                                                        var ctl = $('#<%=PersonalSocial_FamilyRegards_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_FamilyRegards_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <%--<div class="span12">
                                            <div class="control-label">
                                                1.HOW WILL YOU RATE YOUR CHILDS FROM THE ABOVE SCALE?
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="PersonalSocial_RateChild" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>--%>

                                        <%--<div class="span12 formRow">
       <div class="row">
           <div class="span2">
               <h6>HOW IS THE CHILD SOCIALLY?:</h6>
           </div>
           <div class="span8">
               <asp:CheckBoxList ID="PersonalSocial_ChildSocially" runat="server" CssClass="checkbox span8" SelectionMode="Multiple">
                   <asp:ListItem Value="AWFUL">AWFUL</asp:ListItem>
                   <asp:ListItem Value="NOT VERY GOOD">NOT VERY GOOD</asp:ListItem>
                   <asp:ListItem Value="OKAY">OKAY</asp:ListItem>
                   <asp:ListItem Value="REALLY GOOD">REALLY GOOD</asp:ListItem>
                   <asp:ListItem Value="FANTASTIC">FANTASTIC</asp:ListItem>
               </asp:CheckBoxList>
           </div>
       </div>
   </div>--%>
                                        <div class="span12">
                                            <div class="control-label">
                                                <h6>How is the child sociallly?:</h6>
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_1" runat="server" onclick="PersonalSocial_ChildSocially_1_Click();"
                                                    CssClass="checkboes" Text=" Difficult to handle" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_2" runat="server" onclick="PersonalSocial_ChildSocially_2_Click();"
                                                    CssClass="checkboes" Text="Good" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_3" runat="server" onclick="PersonalSocial_ChildSocially_3_Click();"
                                                    CssClass="checkboes" Text="Okay" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_4" runat="server" onclick="PersonalSocial_ChildSocially_4_Click();"
                                                    CssClass="checkboes" Text="Really good" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_5" runat="server" onclick="PersonalSocial_ChildSocially_5_Click();"
                                                    CssClass="checkboes" Text="Fantastic" />
                                                <script type="text/javascript">
                                                    function PersonalSocial_ChildSocially_1_Click() {
                                                        var ctl = $('#<%=PersonalSocial_ChildSocially_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_ChildSocially_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_4.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_5.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_ChildSocially_2_Click() {
                                                        var ctl = $('#<%=PersonalSocial_ChildSocially_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_FamilyRegards_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_4.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_5.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_ChildSocially_3_Click() {
                                                        var ctl = $('#<%=PersonalSocial_ChildSocially_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_ChildSocially_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_4.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_5.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_ChildSocially_4_Click() {
                                                        var ctl = $('#<%=PersonalSocial_ChildSocially_4.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_ChildSocially_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_5.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function PersonalSocial_ChildSocially_5_Click() {
                                                        var ctl = $('#<%=PersonalSocial_ChildSocially_5.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=PersonalSocial_ChildSocially_1.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_2.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_3.ClientID %>').prop('checked', false);
                                                            $('#<%=PersonalSocial_ChildSocially_4.ClientID %>').prop('checked', false);
                                                        }
                                                    }

                                                </script>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="PERSONAL_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="SPEECH AND LANGUAGE">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="span12">
                                            <div class="control-label">
                                                1.  When did your child start to speak
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_StartSpeek" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.  When did your child start to Monosyllables
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Monosyllables" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.  When did your child start to Bisyllables
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Bisyllables" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.  When did your child start to short sentences
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_ShrotScentences" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.  When did your child start to long sentences

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_LongScentences" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    6. Unusual sounds /Jargon speech
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="SpeechLanguage_UnusualSoundsJargonSpeech_1" runat="server" onclick="SpeechLanguage_UnusualSoundsJargonSpeech_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="SpeechLanguage_UnusualSoundsJargonSpeech_2" runat="server" onclick="SpeechLanguage_UnusualSoundsJargonSpeech_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <script type="text/javascript">
                                                        function SpeechLanguage_UnusualSoundsJargonSpeech_1_Click() {
                                                            var ctl = $('#<%=SpeechLanguage_UnusualSoundsJargonSpeech_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SpeechLanguage_UnusualSoundsJargonSpeech_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function SpeechLanguage_UnusualSoundsJargonSpeech_2_Click() {
                                                            var ctl = $('#<%=SpeechLanguage_UnusualSoundsJargonSpeech_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SpeechLanguage_UnusualSoundsJargonSpeech_1.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span12">
                                            <div class="control-label">
                                                7. Imitation of speech / Gestures
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SpeechLanguage_speechgestures_1" runat="server" onclick="SpeechLanguage_speechgestures_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="SpeechLanguage_speechgestures_2" runat="server" onclick="SpeechLanguage_speechgestures_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function SpeechLanguage_speechgestures_1_Click() {
                                                        var ctl = $('#<%=SpeechLanguage_speechgestures_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SpeechLanguage_speechgestures_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SpeechLanguage_speechgestures_2_Click() {
                                                        var ctl = $('#<%=SpeechLanguage_speechgestures_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SpeechLanguage_speechgestures_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Non verbal facial: Expression

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NonverbalfacialExpression" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                9.  Non verbal facial: Eye contact

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NonverbalfacialEyeContact" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                10.Non verbal facial: Gestures

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NonverbalfacialGestures" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11.Interpretation of language: Simple / Complex

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_SimpleComplex" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                12.Interpretation of language:Understand implied meaning

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_UnderstandImpliedMeaning" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                13. Interpretation of language:Understand Joke / sarcasm

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_UnderstandJokesarcasm" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                14. Interpretation of language:Responds to name

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Respondstoname" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    <h6>15. Two - way interaction</h6>
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_1" runat="server" onclick="SpeechLanguage_TwowayInteraction_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_2" runat="server" onclick="SpeechLanguage_TwowayInteraction_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_3" runat="server" onclick="SpeechLanguage_TwowayInteraction_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function SpeechLanguage_TwowayInteraction_1_Click() {
                                                            var ctl = $('#<%=SpeechLanguage_TwowayInteraction_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SpeechLanguage_TwowayInteraction_2.ClientID %>').prop('checked', false);
                                                                $('#<%=SpeechLanguage_TwowayInteraction_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function SpeechLanguage_TwowayInteraction_2_Click() {
                                                            var ctl = $('#<%=SpeechLanguage_TwowayInteraction_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SpeechLanguage_TwowayInteraction_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SpeechLanguage_TwowayInteraction_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function SpeechLanguage_TwowayInteraction_3_Click() {
                                                            var ctl = $('#<%=SpeechLanguage_TwowayInteraction_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=SpeechLanguage_TwowayInteraction_1.ClientID %>').prop('checked', false);
                                                                $('#<%=SpeechLanguage_TwowayInteraction_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                16. Narrate incidents:At school

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NarrateIncidentsAtSchool" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                17.Narrate incidents:At Home/Expression of :Want

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NarrateIncidentsAtHome" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%-- <div class="span12">
        <div class="control-label">
            18.Expression of :Want

        </div>
        <div class="control-group">
            <asp:TextBox ID="SpeechLanguage_Want" runat="server" CssClass="span10" TextMode="MultiLine"
                Rows="8">
            </asp:TextBox>
        </div>

        <span class="char-limit-msg"></span>
    </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                19.Expression of :Needs

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Needs" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                20.Expression of :Emotions

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Emotions" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                21.Expression of :Achievements / Failure

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_AchievementsFailure" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%--<div class="span12">
        <div class="control-label">
            22.Language Spoken to the Child

        </div>
        <div class="control-group">
            <asp:TextBox ID="SpeechLanguage_LanguageSpoken" runat="server" CssClass="span10" TextMode="MultiLine"
                Rows="8">
            </asp:TextBox>
        </div>

        <span class="char-limit-msg"></span>
    </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                23.Echolalia

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Echolalia" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%--<div class="span12">
        <div class="control-label">
            24.Emotional milestones

        </div>
        <div class="control-group">
            <asp:TextBox ID="SpeechLanguage_Emotionalmilestones" runat="server" CssClass="span10" TextMode="MultiLine"
                Rows="8">
            </asp:TextBox>
        </div>

        <span class="char-limit-msg"></span>
    </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Speech_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report5" runat="server" HeaderText="BEHAVIOUR">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <%-- <div class="span12 formRow">--%>
                                    <div class="formRow">

                                        <div class="span12">
                                            <div class="control-label">
                                                1.Behaviour of the child :- What does the child do in his free time
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_FreeTime" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <%--<div class="clearfix"></div>--%>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    2. Type of play behaviour :
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                        <asp:CheckBox ID="chkunassociated" runat="server" CssClass="checkboes" onclick="unassociated_Click();" Text="Unassociated" />
                                                        <asp:CheckBox ID="chksolitary" runat="server" CssClass="checkboes" onclick="solitary_Click();" Text="Solitary" />
                                                        <asp:CheckBox ID="chkonlooker" runat="server" CssClass="checkboes" onclick="onlooker_Click();" Text="Onlooker" />
                                                        <asp:CheckBox ID="chkparallel" runat="server" CssClass="checkboes" onclick="parallel_Click();" Text="Parallel" />
                                                        <asp:CheckBox ID="chkassociative" runat="server" CssClass="checkboes" onclick="associative_Click();" Text="Associative" />
                                                        <asp:CheckBox ID="chkcooperative" runat="server" CssClass="checkboes" onclick="cooperative_Click();" Text="Cooperative" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    3. Does the child have situational meltdowns :
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="Behaviour_situationalmeltdowns_1" runat="server" onclick="Behaviour_situationalmeltdowns_1_Click();"
                                                        CssClass="checkboes" Text=" Yes" />
                                                    <asp:CheckBox ID="Behaviour_situationalmeltdowns_2" runat="server" onclick="Behaviour_situationalmeltdowns_2_Click();"
                                                        CssClass="checkboes" Text="No" />
                                                    <asp:CheckBox ID="Behaviour_situationalmeltdowns_3" runat="server" onclick="Behaviour_situationalmeltdowns_3_Click();"
                                                        CssClass="checkboes" Text="Sometimes" />
                                                    <script type="text/javascript">
                                                        function Behaviour_situationalmeltdowns_1_Click() {
                                                            var ctl = $('#<%=Behaviour_situationalmeltdowns_1.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Behaviour_situationalmeltdowns_2.ClientID %>').prop('checked', false);
                                                                $('#<%=Behaviour_situationalmeltdowns_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Behaviour_situationalmeltdowns_2_Click() {
                                                            var ctl = $('#<%=Behaviour_situationalmeltdowns_2.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Behaviour_situationalmeltdowns_1.ClientID %>').prop('checked', false);
                                                                $('#<%=Behaviour_situationalmeltdowns_3.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                        function Behaviour_situationalmeltdowns_3_Click() {
                                                            var ctl = $('#<%=Behaviour_situationalmeltdowns_3.ClientID %>')[0];
                                                            if (ctl.checked) {
                                                                $('#<%=Behaviour_situationalmeltdowns_1.ClientID %>').prop('checked', false);
                                                                $('#<%=Behaviour_situationalmeltdowns_2.ClientID %>').prop('checked', false);
                                                            }
                                                        }
                                                    </script>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BEHAVIOUR_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>

                                        <%--   <div class="span12 formRow">
     <div class="row">
         <div class="span2">
             <h5>3. Does the child have situational meltdowns :</h5>
         </div>
         <div class="span8">
             <asp:CheckBoxList ID="Behaviour_situationalmeltdowns" runat="server" CssClass="checkbox span8" SelectionMode="Multiple">
                 <asp:ListItem Value="Crying">Crying</asp:ListItem>
                 <asp:ListItem Value="pinching">pinching</asp:ListItem>
                 <asp:ListItem Value="hitting">hitting</asp:ListItem>
                 <asp:ListItem Value="pushing">pushing</asp:ListItem>
                 <asp:ListItem Value="pulling">pulling</asp:ListItem>
                 <asp:ListItem Value="screaming">screaming</asp:ListItem>
                 <asp:ListItem Value="poking">poking</asp:ListItem>
                 <asp:ListItem Value="clapping">clapping</asp:ListItem>
                 <asp:ListItem Value="tearing">tearing</asp:ListItem>
                 <asp:ListItem Value="eating">eating</asp:ListItem>
                 <asp:ListItem Value="inedible">inedible</asp:ListItem>
                 <asp:ListItem Value="shutdown">shutdown</asp:ListItem>
                 <asp:ListItem Value="headbanging">headbanging</asp:ListItem>
                 <asp:ListItem Value="biting">biting</asp:ListItem>

             </asp:CheckBoxList>
         </div>
     </div>
 </div>--%>
                                    </div>
                                    <%-- </div>--%>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="AROUSAL">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="span12">
                                            <p>
                                                <label for="range1">1.State of alertness during evaluation.</label>
                                                <input type="range" id="range1" step="10" name="range1" list="values" />
                                                <asp:HiddenField ID="hdnrange" runat="server" />
                                            </p>
                                            <datalist2 id="values">
                                                <option value="0" label="0"></option>
                                                <option value="1" label="1"></option>
                                                <option value="2" label="2"></option>
                                                <option value="3" label="3"></option>
                                                <option value="4" label="4"></option>
                                                <option value="5" label="5"></option>
                                                <option value="6" label="6"></option>
                                                <option value="7" label="7"></option>
                                                <option value="8" label="8"></option>
                                                <option value="9" label="9"></option>
                                                <option value="10" label="10"></option>
                                            </datalistv>
                                        </div>

                                        <div class="span12">
                                            <p>
                                                <label for="range2">2.General state of alertness.</label>
                                                <input type="range" id="range2" step="10" name="range2" list="values2" />
                                                <asp:HiddenField ID="Hdnrange2" runat="server" />
                                            </p>
                                            <datalist2 id="values2">
                                                <option value="0" label="0 to 4 Low Arousal"></option>
                                                <option value="50" label="5 to 6 Optimal Arousal"></option>
                                                <option value="100" label="7 to 10 High Arousal"></option>
                                            </datalist2>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.	Responds to stimuli?
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Arousal_Stimuli_1" runat="server" onclick="Arousal_Stimuli_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Arousal_Stimuli_2" runat="server" onclick="Arousal_Stimuli_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <asp:CheckBox ID="Arousal_Stimuli_3" runat="server" onclick="Arousal_Stimuli_3_Click();"
                                                    CssClass="checkboes" Text="Sometimes" />
                                                <script type="text/javascript">
                                                    function Arousal_Stimuli_1_Click() {
                                                        var ctl = $('#<%=Arousal_Stimuli_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Arousal_Stimuli_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Arousal_Stimuli_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Arousal_Stimuli_2_Click() {
                                                        var ctl = $('#<%=Arousal_Stimuli_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Arousal_Stimuli_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Arousal_Stimuli_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Arousal_Stimuli_3_Click() {
                                                        var ctl = $('#<%=Arousal_Stimuli_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Arousal_Stimuli_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Arousal_Stimuli_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>

                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Maintainance Of arousal during transition.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Arousal_Transition_1" runat="server" onclick="Arousal_Transition_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Arousal_Transition_2" runat="server" onclick="Arousal_Transition_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Arousal_Transition_1_Click() {
                                                        var ctl = $('#<%=Arousal_Transition_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Arousal_Transition_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Arousal_Transition_2_Click() {
                                                        var ctl = $('#<%=Arousal_Transition_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Arousal_Transition_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>



                                        <%--  <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    5.Alerting factor.
                                                </div>
                                            </div>
                                            <div class="span8">
                                                <asp:CheckBoxList ID="Arousal_FactorOCD" runat="server" RepeatDirection="Horizontal" CssClass="checkbox span8" SelectionMode="Multiple">
                                                    <asp:ListItem Value="Light">Light</asp:ListItem>
                                                    <asp:ListItem Value="Sound">Sound</asp:ListItem>
                                                    <asp:ListItem Value="Smell">Smell</asp:ListItem>
                                                    <asp:ListItem Value="unusual_characteristics">unusual_characteristics</asp:ListItem>
                                                    <asp:ListItem Value="Things">Things</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>

                                        </div>--%>
                                        <div class="span12">
                                            <div class="control-label">
                                                5.Alerting factor.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_FactorOCD" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.Calming factor.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_ClaimingFactor" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <%-- <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    6.Calming factor.
                                                </div>
                                            </div>
                                            <div class="span8">
                                                <asp:CheckBoxList ID="Arousal_ClaimingFactor" runat="server" RepeatDirection="Horizontal" CssClass="checkbox span8" SelectionMode="Multiple">
                                                    <asp:ListItem Value="Light">Light</asp:ListItem>
                                                    <asp:ListItem Value="Sound">Sound</asp:ListItem>
                                                    <asp:ListItem Value="Smell">Smell</asp:ListItem>
                                                    <asp:ListItem Value="unusual_characteristics">unusual_characteristics</asp:ListItem>
                                                    <asp:ListItem Value="Things">Things</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>--%>


                                        <div class="span12">
                                            <div class="control-label">
                                                7.When does your childs arousal dip down?
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_DipsDown" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="AROUSAL_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report11" runat="server" HeaderText="AFFECT">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="span12">
                                            <div class="control-label">
                                                1.Wide range of emotion
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Affect_RangeEmotion_1" runat="server" onclick="Affect_RangeEmotion_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Affect_RangeEmotion_2" runat="server" onclick="Affect_RangeEmotion_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Affect_RangeEmotion_1_Click() {
                                                        var ctl = $('#<%=Affect_RangeEmotion_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_RangeEmotion_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_RangeEmotion_2_Click() {
                                                        var ctl = $('#<%=Affect_RangeEmotion_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_RangeEmotion_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2 .Is the child able to express emotion
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Affect_ExpressEmotion_1" runat="server" onclick="Affect_ExpressEmotion_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Affect_ExpressEmotion_2" runat="server" onclick="Affect_ExpressEmotion_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Affect_ExpressEmotion_1_Click() {
                                                        var ctl = $('#<%=Affect_ExpressEmotion_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_ExpressEmotion_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_ExpressEmotion_2_Click() {
                                                        var ctl = $('#<%=Affect_ExpressEmotion_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_ExpressEmotion_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Affect appropriate to: Environment
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Environment" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Affect appropriate to: Task
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Task" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.Affect appropriate to: Individual
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Individual" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.Consistent emotion throughout
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_ThroughOut" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7.Factors characterising affect
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Charaterising" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>


                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report9" runat="server" HeaderText="ATTENTION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <%--<div class="span12 formRow">
                <div class="row">
                    <div class="span2">
                        <h5>1. Attention Span</h5>
                    </div>
                    <div class="span8">

                        <asp:CheckBoxList ID="Attention_Span" runat="server" CssClass="checkbox span8" SelectionMode="Multiple">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="6">6</asp:ListItem>
                            <asp:ListItem Value="7">7</asp:ListItem>
                            <asp:ListItem Value="8">8</asp:ListItem>
                            <asp:ListItem Value="9">9</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>

                        </asp:CheckBoxList>

                    </div>
                </div>
            </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                1.	Attention span

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_AttentionSpan" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <div class="clearfix"></div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Focus task at hand-Home
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_FocusHandhome_1" runat="server" onclick="Attention_FocusHandhome_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Attention_FocusHandhome_2" runat="server" onclick="Attention_FocusHandhome_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Attention_FocusHandhome_1_Click() {
                                                        var ctl = $('#<%=Attention_FocusHandhome_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_FocusHandhome_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Attention_FocusHandhome_2_Click() {
                                                        var ctl = $('#<%=Attention_FocusHandhome_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_FocusHandhome_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Focus task at hand-School
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_FocusHandSchool_1" runat="server" onclick="Attention_FocusHandSchool_1_Click();"
                                                    CssClass="checkboes" Text="Yes" />
                                                <asp:CheckBox ID="Attention_FocusHandSchool_2" runat="server" onclick="Attention_FocusHandSchool_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Attention_FocusHandSchool_1_Click() {
                                                        var ctl = $('#<%=Attention_FocusHandSchool_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_FocusHandSchool_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Attention_FocusHandSchool_2_Click() {
                                                        var ctl = $('#<%=Attention_FocusHandSchool_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_FocusHandSchool_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Dividing attention
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_Dividing_1" runat="server" onclick="Attention_Dividing_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Attention_Dividing_2" runat="server" onclick="Attention_Dividing_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Attention_Dividing_1_Click() {
                                                        var ctl = $('#<%=Attention_Dividing_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_Dividing_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Attention_Dividing_2_Click() {
                                                        var ctl = $('#<%=Attention_Dividing_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_Dividing_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5. Change of activities every
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_ChangeActivities" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.	Age appropriate attention   
                        <div class="control-group" style="padding-left: 20px">
                            <asp:CheckBox ID="Attention_AgeAppropriate_1" runat="server" onclick="Attention_AgeAppropriate_1_Click();"
                                CssClass="checkboes" Text=" Yes" />
                            <asp:CheckBox ID="Attention_AgeAppropriate_2" runat="server" onclick="Attention_AgeAppropriate_2_Click();"
                                CssClass="checkboes" Text="No" />
                            <asp:CheckBox ID="Attention_AgeAppropriate_3" runat="server" onclick="Attention_AgeAppropriate_3_Click();"
                                CssClass="checkboes" Text="Sometimes" />
                            <script type="text/javascript">
                                function Attention_AgeAppropriate_1_Click() {
                                    var ctl = $('#<%=Attention_AgeAppropriate_1.ClientID %>')[0];
                                    if (ctl.checked) {
                                        $('#<%=Attention_AgeAppropriate_2.ClientID %>').prop('checked', false);
                                        $('#<%=Attention_AgeAppropriate_3.ClientID %>').prop('checked', false);
                                    }
                                }
                                function Attention_AgeAppropriate_2_Click() {
                                    var ctl = $('#<%=Attention_AgeAppropriate_2.ClientID %>')[0];
                                    if (ctl.checked) {
                                        $('#<%=Attention_AgeAppropriate_1.ClientID %>').prop('checked', false);
                                        $('#<%=Attention_AgeAppropriate_3.ClientID %>').prop('checked', false);
                                    }
                                }
                                function Attention_AgeAppropriate_3_Click() {
                                    var ctl = $('#<%=Attention_AgeAppropriate_3.ClientID %>')[0];
                                    if (ctl.checked) {
                                        $('#<%=Attention_AgeAppropriate_1.ClientID %>').prop('checked', false);
                                        $('#<%=Attention_AgeAppropriate_2.ClientID %>').prop('checked', false);
                                    }
                                }
                            </script>
                        </div>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7.	Factors of distractibility
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_Distractibility" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Focal attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Focal_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                9.Joint attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Joint_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                10.Divided attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Divided_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11.Alternating attention
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Alternating_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                12.Sustained attention
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Sustained_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                13.Does the child move from one activity to another continuously?
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_move_1" runat="server" onclick="Attention_move_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Attention_move_2" runat="server" onclick="Attention_move_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Attention_move_1_Click() {
                                                        var ctl = $('#<%=Attention_move_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_move_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Attention_Dividing_2_Click() {
                                                        var ctl = $('#<%=Attention_move_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_move_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ATTENTION_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>


                                        <div class="clearfix"></div>

                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report12" runat="server" HeaderText="ACTION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="span12">
                                            <div class="control-label">
                                                1.Age appropriate motor planning
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Action_MotorPlanning" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2 .Purposeful
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_Purposeful_1" runat="server" onclick="Action_Purposeful_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_Purposeful_2" runat="server" onclick="Action_Purposeful_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Action_Purposeful_1_Click() {
                                                        var ctl = $('#<%=Action_Purposeful_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_Purposeful_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Action_Purposeful_2_Click() {
                                                        var ctl = $('#<%=Action_Purposeful_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_Purposeful_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Goal oriented
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_GoalOriented_1" runat="server" onclick="Action_GoalOriented_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_GoalOriented_2" runat="server" onclick="Action_GoalOriented_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Action_GoalOriented_1_Click() {
                                                        var ctl = $('#<%=Action_GoalOriented_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_GoalOriented_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Action_GoalOriented_2_Click() {
                                                        var ctl = $('#<%=Action_GoalOriented_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_GoalOriented_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Feedback dependent
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_FeedBackDependent_1" runat="server" onclick="Action_FeedBackDependent_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_FeedBackDependent_2" runat="server" onclick="Action_FeedBackDependent_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Action_FeedBackDependent_1_Click() {
                                                        var ctl = $('#<%=Action_FeedBackDependent_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_FeedBackDependent_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Action_FeedBackDependent_2_Click() {
                                                        var ctl = $('#<%=Action_FeedBackDependent_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_FeedBackDependent_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.Constructive?
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_Constructive_1" runat="server" onclick="Action_Constructive_1_click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_Constructive_2" runat="server" onclick="Action_Constructive_2_click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Action_Constructive_1_Click() {
                                                        var ctl = $('#<%=Action_Constructive_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_Constructive_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Action_Constructive_2_Click() {
                                                        var ctl = $('#<%=Action_Constructive_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_Constructive_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Action_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>

                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report10" runat="server" HeaderText="INTERACTION">
                            <ContentTemplate>

                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <%--<div class="span12">
                       <div class="control-label">
                           1.Interaction with Known People
                       </div>
                       <div class="control-group">
                           <asp:TextBox ID="Interaction_KnowPeople" runat="server" CssClass="span10" TextMode="MultiLine" Rows="4">
                           </asp:TextBox>
                       </div>
                       <span class="char-limit-msg"></span>
                   </div>--%>
                                        <div class="span12">
                                            <div class="control-label">
                                                1.Interaction during social gathering.
                                            </div>
                                            <div class="span5">
                                                <div class="control-label">
                                                    <asp:CheckBox ID="chkInteracts" runat="server" CssClass="checkboes" onclick="Interacts_Click();" Text="Interacts" />
                                                    <asp:CheckBox ID="chkDoes_not_initiate" runat="server" CssClass="checkboes" onclick="Does_not_initiate_Click();" Text="Does not initiate" />
                                                    <asp:CheckBox ID="chkSustain" runat="server" CssClass="checkboes" onclick="Sustain_Click();" Text="Does not sustain" />
                                                    <asp:CheckBox ID="chkFight" runat="server" CssClass="checkboes" onclick="Fight_Click();" Text="Fight" />
                                                    <asp:CheckBox ID="chkFreeze" runat="server" CssClass="checkboes" onclick="Freeze_Click();" Text="Freeze" />
                                                    <asp:CheckBox ID="chkFright" runat="server" CssClass="checkboes" onclick="Fright_Click();" Text="Fright" />
                                                    <asp:CheckBox ID="chkAnxious" runat="server" CssClass="checkboes" onclick="Anxious_Click();" Text="Anxious" />
                                                    <asp:CheckBox ID="chkComfortable" runat="server" CssClass="checkboes" onclick="Comfortable_Click();" Text="Comfortable" />
                                                    <asp:CheckBox ID="chkNervous" runat="server" CssClass="checkboes" onclick="Nervous_Click();" Text="Nervous" />
                                                    <asp:CheckBox ID="chkANS_response" runat="server" CssClass="checkboes" onclick="ANS_response_Click();" Text="ANS_response" />
                                                    <asp:CheckBox ID="chkOTHERS" runat="server" CssClass="checkboes" onclick="OTHERS_Click();" Text="OTHERS" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="cmtgathering" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%-- <div class="span12">
              <div class="control-label">
                  2.Interaction with / At :- Strangers
              </div>
              <div class="control-group">
                  <asp:TextBox ID="Interaction_Strangers" runat="server" CssClass="span10" TextMode="MultiLine"
                      Rows="8">
                  </asp:TextBox>
              </div>
              <span class="char-limit-msg"></span>
          </div>--%>

                                        <%-- <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    2.Interaction  Social  Gathering :
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>

                                        <%--<div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    3. Emotional Response :
                                                </div>
                                            </div>
                                            <div class="span8">
                                                <asp:CheckBoxList ID="Interaction_EmotionalResponse" runat="server" RepeatDirection="Horizontal" CssClass="checkbox span8" SelectionMode="Multiple">
                                                    <asp:ListItem Value="Anxious">Anxious</asp:ListItem>
                                                    <asp:ListItem Value="Comfortable">Comfortable</asp:ListItem>
                                                    <asp:ListItem Value="Nervous">Nervous</asp:ListItem>
                                                    <asp:ListItem Value="ANS_response">ANS_response</asp:ListItem>
                                                    <asp:ListItem Value="OTHERS">OTHERS</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>--%>
                                        <%--<div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    3. Emotional Response :
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Understands/Appreciates social cues.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Interaction_SocialQues_1" runat="server" onclick="Interaction_SocialQues_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Interaction_SocialQues_2" runat="server" onclick="Interaction_SocialQues_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Interaction_SocialQues_1_Click() {
                                                        var ctl = $('#<%=Interaction_SocialQues_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Interaction_SocialQues_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Interaction_SocialQues_2_Click() {
                                                        var ctl = $('#<%=Interaction_SocialQues_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Interaction_SocialQues_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Reaction to emotion of other happiness
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Happiness" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Reaction to emotion sadness
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Sadness" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.Reaction to emotion surprise
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Surprise" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.Reaction to emotion  shock
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Shock" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7.Friendship : can make friends
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Interaction_Friends_1" runat="server" onclick="Interaction_Friends_1_Click();"
                                                    CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Interaction_Friends_2" runat="server" onclick="Interaction_Friends_2_Click();"
                                                    CssClass="checkboes" Text="No" />
                                                <script type="text/javascript">
                                                    function Interaction_Friends_1_Click() {
                                                        var ctl = $('#<%=Interaction_Friends_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Interaction_Friends_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Interaction_SocialQues_2_Click() {
                                                        var ctl = $('#<%=Interaction_Friends_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Interaction_Friends_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>

                                        <%--<div class="span12">
              <div class="control-label">
                  10.Relates To known People.
              </div>
               <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="Interaction_RelatesPeople_1" runat="server" onclick="Interaction_RelatesPeople_1_Click();"
                      CssClass="checkboes" Text=" Yes" />
                  <asp:CheckBox ID="Interaction_RelatesPeople_2" runat="server" onclick="Interaction_RelatesPeople_2_Click();"
                      CssClass="checkboes" Text="No" />
                    <asp:CheckBox ID="Interaction_RelatesPeople_3" runat="server" onclick="Interaction_RelatesPeople_3_Click();"
                      CssClass="checkboes" Text="SOMETIMES" />
                  <script type="text/javascript">
                            function Interaction_RelatesPeople_1_Click() {
                                var ctl = $('#<%=Interaction_RelatesPeople_1.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=Interaction_RelatesPeople_2.ClientID %>').prop('checked', false);
                              $('#<%=Interaction_RelatesPeople_3.ClientID %>').prop('checked', false);
                          }
                      }
                            function Interaction_RelatesPeople_2_Click() {
                          var ctl = $('#<%=Interaction_RelatesPeople_2.ClientID %>')[0];
                          if (ctl.checked) {
                              $('#<%=Interaction_RelatesPeople_1.ClientID %>').prop('checked', false);
                              $('#<%=Interaction_RelatesPeople_3.ClientID %>').prop('checked', false);
                                }
                            }
                            function Interaction_RelatesPeople_3_Click() {
                                var ctl = $('#<%=Interaction_RelatesPeople_3.ClientID %>')[0];
                          if (ctl.checked) {
                                    $('#<%=Interaction_RelatesPeople_1.ClientID %>').prop('checked', false);
                                    $('#<%=Interaction_RelatesPeople_2.ClientID %>').prop('checked', false);
                                }
                            }
                  </script>
              </div>
          </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.What activities does he/she enjoys.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Enjoy" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="INTERACTION_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>


                                        <div class="clearfix">
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report18" runat="server" HeaderText="SYSTEM EVALUATION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="fromRow">
                                        <div class="span12">

                                            <div class="control-label">
                                                <h6>Tactile Systems</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Registration" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Responsiveness ( Hyper responsive/Hyporesponsive ) 
                                                      Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>Somatosensory system- ( Tactile-Vestibular - Prop Trio)</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Body awareness
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bodyawareness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Body schema
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bodyschema" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Orientation of body in space
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Posterior space awareness
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Posterior" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Bilateral Coordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bilateral" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Balance on static and dynamic surfaces
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Balance" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                               Dominance
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Dominance" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                               Right and Left Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Right" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                              How well he/she identifies body parts
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_identifies" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Can name and point objects/ people
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_point" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Constantly bumps into objects in his/her path
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Constantly" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Is he/she clumsy with his things
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_clumsy" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Can he/she maneuver himself out of a 
                                                variety of equipment or situations?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_maneuver" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Is he/she overly fidgety?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_overly" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Is he/she able to stand in line 
                                                      duringor waits for his/her turn
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_stand" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Does he/she indulge into rough/sportplay?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_indulge" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Does he/she dislike any type of textures?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_textures" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Use of monkey ladders, obstacle course 
                                                      (climbing up and crossing) Commando crawl
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_monkey" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Use of swings Slide Can he/she perform heavy activities Cycle/tricycle 
                                                      Riding Can he/she maintain good posture while sitting?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_swings" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>Vestibular system</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Registration" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Responsiveness ( Hyporesponsive /Hyperresponsive )
                                                     Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>Proprioceptive system</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Registration" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Gradation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Gradation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness mention the behavioral 
                                                      responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>ORO-Motor system:</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Registration" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness(Hyporesponsive /Hyperresponsive ) 
                                                   mention the behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>Auditory System</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Auditory registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Auditory" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Auditory orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness(Hyporesponsive/ Hyperresponsive) 
                                                   mention the behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Auditory discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Background-foreground discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Background" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Auditory localization
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_localization" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Analysis and synthesis
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Analysis" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Auditory memory and sequencing
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_sequencing" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Auditory blending (breaking of sounds)
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_blending" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>


                                            </table>
                                            <div class="control-label">
                                                <h6>Visual system:</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Visual Localization and Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Visual" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Responsiveness(Hyporesponsive/Hyperresponsive ) 
                                                  mention the behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual scanning
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_scanning" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual constancy
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_constancy" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_memory" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual Perception
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Perception" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Eye hand Co- ordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_hand" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Eye foot Co- ordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_foot" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual closure
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_closure" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Figure-ground discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Figureground" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Visualmemory" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual sequential memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_sequential" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual spatial relationships
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_spatial" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="control-label">
                                                <h6>Olfactory system: </h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Registration" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Orientation" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Discrimination" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Responsiveness(Hyporesponsive/Hyperresponsive ) 
                                                 mention the behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Responsiveness" runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="clerarfix"></div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report13" runat="server" HeaderText="DENVERS">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <%--                                <div class="span12">
                                    <div class="control-label">
                                        1.IQ
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeassures_IQ" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="8">
                                        </asp:TextBox>
                                    </div>
                                    <span class="char-limit-msg"></span>
                                </div>

                                <div class="span12">
                                    <div class="control-label">
                                        2.DQ
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeassures_DQ" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="8">
                                        </asp:TextBox>
                                    </div>
                                    <span class="char-limit-msg"></span>
                                </div>--%>
                                        <%-- <div class="span12">
                                            <div class="control-label">
                                                1.ASQ :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_ASQ" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                1.Denver’s  checklist  gross motor
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_GrossMotor" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Denver’s checklist fine motor
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_FineMotor" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Denver’s checklist language
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_DenverLanguage" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Denver’s checklist personal & social
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_DenverPersonal" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <%-- <div class="span12">
                                            <div class="control-label">
                                                7.HANDWRITING QUESTIONNAIRE

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_HandWriting" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>--%>

                                        <%--<div class="span12">
                                            <div class="control-label">
                                                8.SIPT

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_SIPT" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>--%>

                                        <%-- <div class="span12">
                                            <div class="control-label">
                                                9.Sensory Profile 2
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_SensoryProfile" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>--%>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Tests_cmt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report16" runat="server" HeaderText="AGES AND STAGES">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <asp:UpdatePanel ID="updAgeStage" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="SelectMonth" runat="server" CssClass="input-medium chzn-select span2" OnSelectedIndexChanged="SelectMonth_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
                                                    </asp:DropDownList>

                                                    <table style="border: 1px solid gray">
                                                        <tr>
                                                            <td>Sr No</td>
                                                            <td>OVERALL RESPONSES</td>
                                                            <td>YES  </td>
                                                            <td>NO   </td>
                                                            <td>COMMENTS</td>
                                                        </tr>
                                                        <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                                                            <ItemTemplate>

                                                                <tr>
                                                                    <%--<td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%></td>--%>
                                                                    <td>
                                                                        <asp:Label ID="lblQuestionNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.QuestionNo")%>'></asp:Label></td>
                                                                    <td><%#DataBinder.Eval(Container,"DataItem.QUESTIONS")%></td>
                                                                    <td>
                                                                        <center>
                                                                            <asp:CheckBox runat="server" ID="chkMonthYes" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.Yes"))%>' />
                                                                        </center>
                                                                    </td>
                                                                    <td>
                                                                        <center>
                                                                            <asp:CheckBox runat="server" ID="chkMonthNo" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.No"))%>' />
                                                                        </center>
                                                                    </td>
                                                                    <td>
                                                                        <center>
                                                                            <asp:TextBox ID="txtMonthComment" runat="server" CssClass="span3" Text='<%#DataBinder.Eval(Container,"DataItem.Comments")%>'></asp:TextBox></b>
                                                                        </center>
                                                                    </td>
                                                                </tr>

                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="SelectMonth" />
                                                </Triggers>

                                            </asp:UpdatePanel>
                                            <div class="control-label">
                                                <h6>AGES AND STAGES QUESTIONNAIRE - 2 months</h6>
                                                <h6>1 month 0 days through 2 months 30 days</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>22.7</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="score_Communication_2" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="Inter_Communication_2" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross motor</b>
                                                    </td>
                                                    <td>
                                                        <b>41.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_2" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_Gross_2" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>30.16</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_2" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_FINE_2" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>24.62</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_2" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_PROBLEM_2" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>33.71</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_2" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_PERSONAL_2" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--<table class="ndt-default-table">
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>34.60</b>                                                        
                                                    </td>
                                                    <td>
                                                          <b><asp:TextBox ID="score_Communication_2months" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b><asp:TextBox ID="Inter_Communication_2months" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>38.41</b>      
                                                    </td>
                                                    <td>
                                                       <b><asp:TextBox ID="GROSS_2months" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_Gross_2months" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                         <b>29.62</b>    
                                                    </td>
                                                    <td>
                                                        <b><asp:TextBox ID="FINE_2months" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_FINE_2months" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>34.98</b>
                                                    </td>
                                                    <td>
                                                        <b><asp:TextBox ID="PROBLEM_2months" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_PROBLEM_2moths" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>33.16</b>
                                                    </td>
                                                    <td>
                                                        <b><asp:TextBox ID="PERSONAL_2months" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b><asp:TextBox ID="inter_PERSONAL_2months" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE-  4 months</h6>
                                                    <h6>3 months 0 days through 4 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>34.60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="Comm_3" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_3" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>38.41</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_3" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_3" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>29.62</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_3" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_3" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>34.98</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_3" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_3" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>33.96</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_3" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_3" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE (6 months):</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>34.60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="Communication_6" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_inter_6" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>38.41</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_6" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_6" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>29.62</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_6" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_6" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>34.98</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_6" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_6" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>33.16</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_6" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_6" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE- 8 months</h6>
                                                    <h6>7 months 0 days through 8 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>33.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_7" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_7" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>30.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_7" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_7" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>40.15</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_7" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_7" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>36.17</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_7" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_7" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>35.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_7" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_7" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE - 9 MONTHS </h6>
                                                    <h6>9 months 0 days through 9 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>33.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_9" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_9" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>30.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_9" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_9" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>40.15</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_9" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_9" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>36.17</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_9" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_9" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>35.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_9" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_9" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  10 MONTHS</h6>
                                                    <h6>9 months 0 days through 10 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>22.87</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_10" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_10" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>30.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_10" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_10" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>37.97</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_10" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_10" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>32.51</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_10" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_10" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>27.25</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_10" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_10" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  12 MONTHS</h6>
                                                    <h6>11 months 0 days through 12 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>15.64</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_11" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_11" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>21.49</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_11" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_11" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>34.50</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_11" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_11" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>27.32</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_11" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_11" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>21.73</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_11" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_11" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  14 MONTHS </h6>
                                                    <h6>13 months 0 days through 14 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>17.40</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_13" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_13" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_13" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_13" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_13" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_13" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_13" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_13" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_13" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_13" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  16 MONTHS</h6>
                                                    <h6>15 months 0 days through 16 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>17.40</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_15" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_15" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_15" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_15" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_15" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_15" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_15" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_15" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_15" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_15" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  18 MONTHS</h6>
                                                    <h6>17 months 0 days through 18 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>17.40</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_17" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_17" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_17" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_17" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_17" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_17" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_17" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_17" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_17" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_17" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  20 MONTHS</h6>
                                                    <h6>19 months 0 days through 20 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>20.50</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_19" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_19" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>39.89</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_19" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_19" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>36.05</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_19" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_19" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>28.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_19" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_19" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>33.36</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_19" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_19" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  22 MONTHS</h6>
                                                    <h6>21 months 0 days through 22months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>13.04</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_21" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>27.75</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_21" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>29.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_21" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>29.30</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_21" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>30.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_21" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  24 MONTHS </h6>
                                                    <h6>23 months 0 days through 25 months 15 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>25.17</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_23" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_23" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>38.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_23" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_23" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>35.16</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_23" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_23" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>29.78</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_23" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_23" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>31.54</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_23" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_23" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  27 MONTHS </h6>
                                                    <h6>25 months 16 days through 28 months 15 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>24.02</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_25" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_25" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>28.01</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_25" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_25" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>18.42</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_25" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_25" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>27.62</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_25" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_25" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>25.31</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_25" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_25" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  30 MONTHS</h6>
                                                    <h6>28 months 16 days through 31 months 15 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>33.30</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_28" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_28" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>36.14</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_28" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_28" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>19.25</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_28" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_28" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>27.08</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_28" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_28" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>33.01</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_28" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_28" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  33 MONTHS</h6>
                                                    <h6>31 months 16 days through 34 months 15 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>25.36</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_31" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_31" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>34.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_31" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_31" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>12.28</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_31" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_31" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>26.92</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_31" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_31" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>28.96</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_31" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_31" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  36 MONTHS</h6>
                                                    <h6>34 months 16 days to 38 months 30 days </h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>30.99</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_34" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_34" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>36.99</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_34" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_34" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>18.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_34" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_34" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>30.29</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_34" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_34" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>35.33</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_34" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_34" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  42 MONTHS</h6>
                                                    <h6>39 months 0 days to 44 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>27.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_42" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_42" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>36.27</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_42" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_42" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>19.82</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_42" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_42" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>28.11</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_42" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_42" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>31.12</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_42" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_42" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  48 MONTHS</h6>
                                                    <h6>45 months 0 days through 50 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>30.72</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_45" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_45" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>32.78</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_45" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_45" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>15.81</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_45" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_45" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>31.30</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_45" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_45" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>26.60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_45" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_45" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  54 MONTHS</h6>
                                                    <h6>51 months 0 days through 56 months 30 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>31.85</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_51" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_51" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>35.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_51" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_51" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>17.32</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_51" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_51" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>28.12</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_51" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_51" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>32.33</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_51" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_51" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="ndt-default-table">
                                                <div class="control-label">
                                                    <h6>AGES AND STAGES QUESTIONNAIRE -  60 MONTHS</h6>
                                                    <h6>57 months 0 days through 66 months 0 days</h6>
                                                </div>
                                                <tr>
                                                    <td>
                                                        <b>AREA</b>
                                                    </td>
                                                    <td>
                                                        <b>CUT-OFF</b>
                                                    </td>
                                                    <td>
                                                        <b>SCORE</b>
                                                    </td>
                                                    <td>
                                                        <b>INTERPRETATION</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Communication</b>
                                                    </td>
                                                    <td><b>33.19</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_60" runat="server" CssClass="span1"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_60" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Gross Motor</b>
                                                    </td>
                                                    <td>
                                                        <b>31.28</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_60" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_60" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Fine motor</b>
                                                    </td>
                                                    <td>
                                                        <b>26.54</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_60" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_60" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Problem solving</b>
                                                    </td>
                                                    <td>
                                                        <b>29.99</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_60" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_60" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Personal social</b>
                                                    </td>
                                                    <td>
                                                        <b>39.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_60" runat="server" CssClass="span1"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_60" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                            </table>








                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report15" runat="server" HeaderText="SENSORY PROFILE- 2">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="formRow">
                                            <div class="control-label">
                                                <h6>1.Sensory Profile-2  0-6 Months</h6>
                                            </div>
                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span3">&nbsp;
                                                            SECTION
                                                        </td>
                                                        <td>
                                                            <b>RAW SCORE</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>GENERAL processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="General_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>AUDITORY processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="AUDITORY_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>VISUAL processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="VISUAL_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>TOUCH processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TOUCH_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>MOVEMENT processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="MOVEMENT_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ORAL processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ORAL_Processing" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Infant Sensory Profile 2 Raw Score Total
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Raw_score" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <div class="formRow">
                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                        </td>
                                                        <td>
                                                            <b>Raw score Total</b>
                                                        </td>
                                                        <td>
                                                            <b>Interpretation </b>
                                                        </td>
                                                        <%-- <td>
                                                <b>Percentile Range</b>
                                            </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Infant Total Score
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Total_rawscore" runat="server" CssClass="span1"></asp:TextBox>
                                                                /125</b>
                                                        </td>
                                                        <%--<td>
                                                <asp:TextBox ID="Percentile_Range" runat="server" CssClass="span3"></asp:TextBox>
                                            </td>--%>
                                                        <td>
                                                            <asp:TextBox ID="Interpretation" runat="server" CssClass="span3"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>
                                        </div>

                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    Comments :
                                                </div>
                                                <asp:TextBox ID="Comments_1" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="formRow">
                                            <div class="control-label">
                                                <h6>2.SENSORY PROFILE-2  TODDLER</h6>
                                            </div>
                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                <b>QUADRANTS/ SENSORY AND BEHAVIORAL SECTIONS</b>
                                                        </td>
                                                        <td>
                                                            <b>SCORES</b>
                                                        </td>
                                                        <td>
                                                            <b>INTERPRETATION</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                     SEEKING
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_seeking" runat="server" CssClass="span1"></asp:TextBox>
                                                                /35</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="SEEKING" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                AVOIDING
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_Avoiding" runat="server" CssClass="span1"></asp:TextBox>
                                                                /55</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="AVOIDING" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                SENSITIVITY
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_sensitivity" runat="server" CssClass="span1"></asp:TextBox>
                                                                /65</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="SENSITIVITY_2" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                REGISTRATION
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_Registration" runat="server" CssClass="span1"></asp:TextBox>
                                                                /55</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="REGISTRATION" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                GENERAL
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_general" runat="server" CssClass="span1"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="GENERAL" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                AUDITORY
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_Auditory" runat="server" CssClass="span1"></asp:TextBox>/35</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="AUDITORY" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                VISUAL
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_visual" runat="server" CssClass="span1"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="VISUAL" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                TOUCH
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_touch" runat="server" CssClass="span1"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="TOUCH" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                MOVEMENT
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_movement" runat="server" CssClass="span1"></asp:TextBox>/25</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="MOVEMENT" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                ORAL
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_oral" runat="server" CssClass="span1"></asp:TextBox>/35</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ORAL" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                BEHAVIORAL
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Score_behavioural" runat="server" CssClass="span1"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="BEHAVIORAL" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </div>
                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    Comments :
                                                </div>
                                                <asp:TextBox ID="Comments_2" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="formRow">
                                            <div class="control-label">
                                                <h6>3.SENSORY PROFILE-2 : CHILD</h6>
                                            </div>
                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                <b>QUADRANTS/ SENSORY AND BEHAVIORAL SECTIONS</b>
                                                        </td>
                                                        <td>
                                                            <b>SCORES</b>
                                                        </td>
                                                        <td>
                                                            <b>INTERPRETATION</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Seeking/Seeker
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Seeker" runat="server" CssClass="span1"></asp:TextBox>
                                                                /95</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Seeking_Seeker" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Avoiding/Avoider
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Avoider" runat="server" CssClass="span1"></asp:TextBox>
                                                                /100</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Avoiding_Avoider" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensitivity/Sensor
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Sensor" runat="server" CssClass="span1"></asp:TextBox>/95</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensitivity_Sensor" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Registration/Bystander
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Bystander" runat="server" CssClass="span1"></asp:TextBox>
                                                                /110</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Registration_Bystander" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Auditory
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Auditory_3" runat="server" CssClass="span1"></asp:TextBox>/40</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Auditory_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Visual
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Visual_3" runat="server" CssClass="span1"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Visual_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Touch
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Touch_3" runat="server" CssClass="span1"></asp:TextBox>/55</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Touch_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LOT">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Movement
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Movement_3" runat="server" CssClass="span1"></asp:TextBox>/40</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Movement_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Body position
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Body_position" runat="server" CssClass="span1"></asp:TextBox>/40</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Body_position" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Oral
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Oral_3" runat="server" CssClass="span1"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Oral_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Conduct
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Conduct_3" runat="server" CssClass="span1"></asp:TextBox>/45</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Conduct_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Social emotional
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Social_emotional" runat="server" CssClass="span1"></asp:TextBox>/70</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Social_emotional" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LTO">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Attentional
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPchild_Attentional_3" runat="server" CssClass="span1"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Attentional_3" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="JLMO">JLMO</asp:ListItem>
                                                                    <asp:ListItem Value="MTO">MTO</asp:ListItem>
                                                                    <asp:ListItem Value="MMTO">MMTO</asp:ListItem>
                                                                    <asp:ListItem Value="LOT">LTO</asp:ListItem>
                                                                    <asp:ListItem Value="MLTO">MLTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="formRow">
                                                <div class="span12">
                                                    <div class="control-label">
                                                        Comments :
                                                    </div>
                                                    <asp:TextBox ID="Comments_3" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>


                                        <div class="formRow">
                                            <div class="control-label">

                                                <h6>4.Sensory Profile 2 - Adolescent and Adult</h6>
                                            </div>
                                            <h6>Quadrant Summary chart for the ages 11- 17</h6>

                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                <b>Quadrant </b>
                                                        </td>
                                                        <td>
                                                            <b>Raw Score</b>
                                                        </td>
                                                        <td>
                                                            <b>Interpretation </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                               Low Registration 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPAdult_Low_Registration" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Low_Registration" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory seeking 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPAdult_Sensory_seeking" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_seeking" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Sensitivity 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPAdult_Sensory_Sensitivity" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Sensitivity" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Avoiding 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SPAdult_Sensory_Avoiding" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Avoiding" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="formRow">
                                                <div class="span12">
                                                    <div class="control-label">
                                                        Comments :
                                                    </div>
                                                    <asp:TextBox ID="Comments_4" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="formRow">
                                            <div class="control-label">
                                                <h6>Quadrant Summary chart for the ages 16-64</h6>
                                            </div>

                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                <b>Quadrant </b>
                                                        </td>
                                                        <td>
                                                            <b>Raw Score</b>
                                                        </td>
                                                        <td>
                                                            <b>Interpretation </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                               Low Registration 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SP_Low_Registration64" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Low_Registration_5" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory seeking 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SP_Sensory_seeking_64" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_seeking_5" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Sensitivity 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SP_Sensory_Sensitivity64" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Sensitivity_5" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Avoiding 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="SP_Sensory_Avoiding64" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Avoiding_5" runat="server" CssClass="input-medium chzn-select span2">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="formRow">
                                                <div class="span12">
                                                    <div class="control-label">
                                                        Comments :
                                                    </div>
                                                    <asp:TextBox ID="Comments_5" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="formRow">
                                            <div class="control-label">
                                                <h6>Quadrant Summary chart  for the ages 65 and older</h6>
                                            </div>

                                            <div class="span12">
                                                <table class="ndt-default-table">
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                <b>Quadrant </b>
                                                        </td>
                                                        <td>
                                                            <b>Raw Score</b>
                                                        </td>
                                                        <td>
                                                            <b>Interpretation </b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                               Low Registration 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Older_Low_Registration" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Low_Registration_6" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                    <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory seeking 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Older_Sensory_seeking" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_seeking_6" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                    <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Sensitivity 
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="Older_Sensory_Sensitivity" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_Sensitivity_6" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                    <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="span1">&nbsp;
                                                Sensory Avoiding 
                                                        </td>
                                                        <td>

                                                            <b>
                                                                <asp:TextBox ID="Older_Sensory_Avoiding" runat="server" CssClass="span1"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_Avoiding_6" runat="server" CssClass="input-medium chzn-select span2">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Much Less than Most People">Much Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Less than Most People">Less than Most People</asp:ListItem>
                                                                    <asp:ListItem Value="Similar To Most People">Similar To Most People</asp:ListItem>
                                                                    <asp:ListItem Value="More than Most People">More than Most People</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </div>

                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    Comments :
                                                </div>
                                                <asp:TextBox ID="Comments_6" runat="server" CssClass="span4 savedata" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report17" runat="server" HeaderText="ABILITY CHECKLIST">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <asp:UpdatePanel ID="updAbility" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="MonthSelect" runat="server" CssClass="input-medium chzn-select span2" OnSelectedIndexChanged="MonthSelect_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
                                                    </asp:DropDownList>

                                                    <table style="border: 1px solid gray">
                                                        <%-- <tr>
                                                            <td>SrNo</td>
                                                            <td>Question</td>
                                                            <td>YES</td>
                                                            <td>NO</td>
                                                        </tr>--%>

                                                        <asp:Repeater ID="abilityQuestionsParent" runat="server" OnItemDataBound="abilityQuestionsParent_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr style="background-color: #294487">
                                                                    <td style="color: white">Sr.</td>
                                                                    <td>
                                                                        <asp:Label ID="rptlblCategory" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.category_name")%>' Style="color: white"></asp:Label>
                                                                    </td>
                                                                    <td style="color: white">YES</td>
                                                                    <td style="color: white">NO</td>
                                                                </tr>
                                                                <asp:Repeater ID="abilityQuestionsChild" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <center>
                                                                                    <asp:Label ID="abilityQuestionNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.questionNO")%>'></asp:Label>
                                                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.CategoryID")%>' Style="display: none;"></asp:Label>
                                                                                </center>
                                                                            </td>
                                                                            <td><%#DataBinder.Eval(Container,"DataItem.Question")%></td>
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkMonthYes" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.Yes"))%>' /></td>
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkMonthNo" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.No"))%>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                </ul>
                                                            </ItemTemplate>
                                                        </asp:Repeater>





                                                        <%--  <asp:Repeater ID="abilityQuestions" runat="server" OnItemDataBound="abilityQuestions_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="abilityQuestionNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.questionNO")%>'></asp:Label></td>
                                                                    <td><%#DataBinder.Eval(Container,"DataItem.Question")%></td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkMonthYes" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.Yes"))%>' /></td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkMonthNo" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.No"))%>' /></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>--%>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="MonthSelect" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="clearfix"></div>

                                    </div>


                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="span12">
                                                <div class="control-label">
                                                    TOTAL
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="ability_TOTAL" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                                </div>
                                                <span class="char-limit-msg"></span>
                                            </div>

                                            <div class="span12">
                                                <div class="control-label">
                                                    COMMENTS
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="ability_COMMENTS" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                                </div>
                                                <span class="char-limit-msg"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report20" runat="server" HeaderText="DCDQ">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="fromRow">
                                        <div class="span12">

                                            <table class="ndt-default-table" width="800px">
                                                <thead>
                                                    <tr>
                                                        <td>
                                                            <h5>Not at all like your child 1</h5>
                                                        </td>
                                                        <td>
                                                            <h5>A bit like your child 2</h5>
                                                        </td>
                                                        <td>
                                                            <h5>Moderately like your child 3</h5>
                                                        </td>
                                                        <td>
                                                            <h5>Quite a bit like your child 4</h5>
                                                        </td>
                                                        <td>
                                                            <h5>Extremely like your child 5</h5>
                                                        </td>

                                                    </tr>

                                                </thead>
                                            </table>
                                            <table class="ndt-default-table">
                                                <thead>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <h6>Control during movement</h6>
                                                        </td>
                                                        <td>
                                                            <h6>Fine motor/Handwriting</h6>
                                                        </td>
                                                        <td>
                                                            <h6>General coordination</h6>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>1. Throws ball
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>2. Catches ball
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>3. Hits ball/birdie
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>4. Jumps over
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>5. Runs
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>6. Plans activity
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>7. Writing fast
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>8. Writing legibly
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>9. Effort and pressure
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>10. Cuts
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>11. Likes sports
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>12. Learning new skills
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>13. Quick and competent
                                                        </td>

                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>14. “Bull in shop”
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>15. Does not fatigue
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does1" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does2" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does3" runat="server" Width="200px" CssClass="span1"></asp:TextBox></b>
                                                        </td>
                                                    </tr>

                                                </thead>
                                            </table>

                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Control" runat="server" CssClass="span1"></asp:TextBox>/30 +</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Fine" runat="server" CssClass="span1"></asp:TextBox>/20 +</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_General" runat="server" CssClass="span1"></asp:TextBox>/25 =</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Total" runat="server" CssClass="span1"></asp:TextBox>/75</b></td>

                                                </tr>
                                                <tr>
                                                    <td>Control during movement  </td>
                                                    <td>Fine motor and handwriting </td>
                                                    <td>General coordination </td>
                                                    <td>Total</td>
                                                </tr>


                                            </table>

                                            <div class="formRow">
                                                <div class="span12">
                                                    <table class="ndt-default-table">
                                                        <tr>
                                                            <td>
                                                                <h5>Age group</h5>
                                                            </td>
                                                            <td>
                                                                <h5>Indication of,or suspect for,DCD</h5>
                                                            </td>
                                                            <td>
                                                                <h5>Probably not DCD</h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>5 years to 7 years 11 months
                                                            </td>
                                                            <td>15-46
                                                            </td>
                                                            <td>47-75
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>8 years 0 months to 9 years 11 months
                                                            </td>
                                                            <td>15-55
                                                            </td>
                                                            <td>56-75
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>10 years 0 months to 15 years
                                                            </td>
                                                            <td>15-57
                                                            </td>
                                                            <td>58-75
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </div>
                                            </div>

                                            <div class="span12">
                                                <div class="control-label">
                                                    INTERPRETATION :
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="DCDQ_INTERPRETATION" runat="server" CssClass="span10" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span12">
                                                <div class="control-label">
                                                    COMMENTS :
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="DCDQ_COMMENT" runat="server" CssClass="span10" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>

                                </div>

                            </ContentTemplate>

                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report19" runat="server" HeaderText="SIPT INFORMATION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span11">
                                            <h5>SIPT Information :</h5>
                                            <ajaxToolkit:TabContainer ID="TabContainer2" runat="server">
                                                <ajaxToolkit:TabPanel ID="tb_SubReport1" runat="server" HeaderText="History">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    History :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="SIPTInfo_History" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport2" runat="server" HeaderText="Hand Function-I">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Hand Functions</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Right</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Left</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Grasp : Cylindrical
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GraspRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GraspLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:Spherical
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:Hook
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_HookRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_HookLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:3 Jaw chuck
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Grip
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GripRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GripLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Release
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport3" runat="server" HeaderText="Hand Function-II">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Hand Functions</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Lf->R</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Lf->L</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>MF->R</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>MF->L</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>RF->R</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>RF->L</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Opposition
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Pinch
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFR" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFL" runat="server" CssClass="span1"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport4" runat="server" HeaderText="SIPT-III">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Reaching > Spontaneous
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT3_Spontaneous" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Reaching > On Command
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT3_Command" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport5" runat="server" HeaderText="SIPT-IV">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Kinesthesia
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Kinesthesia" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Finger Identification Test
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Finger" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Localisation Of Tactile Stimuli
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Localisation" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Double Tactile Localisation
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_DoubleTactile" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tactile Discrimination
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Tactile" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Graphesthesia
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Graphesthesia" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Post Rotary Nystagmus
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_PostRotary" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Standing And Walking Balance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Standing" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport6" runat="server" HeaderText="SIPT-V">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Color Recognition
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Color" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Form Constancy
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Form" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Size Differentiation
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Size" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Depth Perception
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Depth" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Figure Ground Perception
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Figure" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Motor Accuracy
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Motor" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport7" runat="server" HeaderText="SIPT-VI">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Design Copying
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT6_Design" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Constructional Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT6_Constructional" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport8" runat="server" HeaderText="SIPT-VII">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Visual Scanning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT7_Scanning" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Visual Memory
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT7_Memory" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport9" runat="server" HeaderText="SIPT-VIII">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span12">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Postural Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Postural" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Oral Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Oral" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sequencing Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Sequencing" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Praxis On Verbal Commands
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Commands" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport10" runat="server" HeaderText="SIPT-IX">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Bilateral Motor Co-ordination
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_Bilateral" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Space Visualisation Contralat Use
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_Contralat" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Space Visualisation Preferred Hand
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_PreferredHand" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Crossing Midline
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_CrossingMidline" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport11" runat="server" HeaderText="SIPT-X">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Draw A Person Test
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Draw" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Clock Face
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_ClockFace" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Filtering Information
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Filtering" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Motor Planning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_MotorPlanning" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Body Image
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_BodyImage" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Body Schema
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Laterality
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Laterality" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReport12" runat="server" HeaderText="Activity Given">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    Activity Given :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Remark" runat="server" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td>
                                                                            <b>Parameter</b>
                                                                        </td>
                                                                        <td>
                                                                            <b>Value</b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Interest In Activity
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Interest In Completion
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Initial Learning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Complexity And Organisation Of Task
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Problem Solving
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Concentration
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Retension And Recall
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Speed Of Perfomance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Activity Neatness
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Frustation Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Work Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Reaction To Authority
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sociability With Therapist
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sociability With Others Students
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityStudents" runat="server" CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                            </ajaxToolkit:TabContainer>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tb_Report21" runat="server" HeaderText="EVALUATION">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span11">
                                            <h5>Evaluation :</h5>
                                            <ajaxToolkit:TabContainer ID="TabContainer3" runat="server">
                                                <ajaxToolkit:TabPanel ID="tb_SubReporteval13" runat="server" HeaderText="Strength">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Strengths :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Strengths" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReporteval14" runat="server" HeaderText="Area of Concerns">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Barriers :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Functional limitations :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Posture and movement limitation(Prioritized) :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    4. Impairment(Prioritized) :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Impairment" runat="server" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReporteval15" runat="server" HeaderText="Goals">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Summary :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Previous long term goals :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Previous" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Long term goals(Functional outcome measured)1 - Year :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_LongTerm" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    4. Short term goals(Functional outcome measures) 3 - Month :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_ShortTerm" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    5. Impairment related objective goal-3 Months :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Impairment" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="tb_SubReporteval16" runat="server" HeaderText="Plan Of Care">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Frequency and duration :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Frequency" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Service delivery models :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Service" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Strategies to address impairments and posture movement issues motor learning
                                                                    :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Strategies" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    4. Equipment/Adjuncts :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Equipment" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    5. Client/Family education :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Education" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                            </ajaxToolkit:TabContainer>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>



                        <ajaxToolkit:TabPanel ID="tb_Report14" runat="server" HeaderText="TREATMENT ADVICE">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                        <div class="span12">
                                            <div class="control-label">
                                                1. Advice for home
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_Home" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2. Advice for school
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_School" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" Style="background-color: white; color: black;"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3. Advice for therapy
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_Threapy" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" Style="background-color: white; color: black;"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_cmt" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3" Style="background-color: white; color: black;"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>

                                    </div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>



                        <ajaxToolkit:TabPanel ID="tb_Report22" runat="server" HeaderText="DOCTOR">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow" style="display: none;">
                                        <%--<div class="span12">
                                            <div class="control-label">
                                                Goals and Expectations from therapy :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="GoalsAndExpectations" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>--%>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Physiotherapist :
                                            </div>
                                            <div class="control-group">
                                                <asp:DropDownList ID="Doctor_Physioptherapist" runat="server" CssClass="chzn-select span6">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Physiotherapist :
                                            </div>
                                            <div class="control-group">
                                                <asp:DropDownList ID="Doctor_Occupational" runat="server" CssClass="chzn-select span6">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <%--  <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Name of Director :
                                            </div>
                                            <div class="control-group">
                                                <asp:DropDownList ID="Doctor_EnterReport" runat="server" CssClass="chzn-select span6">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>--%>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                    </ajaxToolkit:TabContainer>

                    <div class="clearfix">
                        <button type="button" id="btnSaveNext" class="buttonClass" style="width: 200px; display:none">
                            Save&Next
                        </button>
                    </div>

                </div>
                <div class="clearfix"></div>
            </div>
            <div class="clearfix"></div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog" style="max-width: 400px; max-height: 400px">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Modal Header</h4>
                </div>
                <div class="modal-body">
                    <h5 class="modal-title">YOU DID NOT CLICK ON SAVE&NEXT BUTTON DATA IS SAVING PLEASE WAIT.. </h5>
                    <%--<img src="../images/NewLoader.gif" />--%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
                </div>
            </div>

        </div>
    </div>
      <div class="modal fade" id="confirmSaveModal" role="dialog">
  <div class="modal-dialog modal-lg">

    <div class="modal-content">

      <!-- HEADER -->
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Confirm Save</h4>
      </div>

      <!-- BODY -->
      <div class="modal-body">

        <div id="modalInternetStatus" style="margin-bottom:10px;"></div>

        <div style="max-height:300px; overflow:auto;">
          <table class="table table-bordered table-condensed">
            <tbody id="modalDataPreview"></tbody>
          </table>
        </div>

      </div>

      <!-- FOOTER -->
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary" id="confirmSaveBtn">Confirm & Save</button>
      </div>

    </div>

  </div>
</div>
    <script type="text/javascript">
        function clientActiveTabChanged(sender, args) {
            debugger;
            var tabName = sender.get_tabs()[sender.get_activeTabIndex()]._tab;
            //alert(tabName.id);
            document.getElementById("hfdTabs").value = tabName.id;
        }

    </script>





    <%--<script type="text/javascript">

        var preTabId;
        var CurTabId;
       
        function clientActiveTabChanged(sender, args) {
            debugger;

            $('#myModal').modal('show');

            var tabName = sender.get_tabs()[sender.get_activeTabIndex()]._tab;
            //alert(tabName.id);


            CurTabId = tabName.id

            preTabId = document.getElementById("hfdPrevTab").value

            if (preTabId == 'undefined' || preTabId == null || preTabId == "") {
                preTabId = 'tb_Report1';
            }

            if (preTabId != CurTabId) {
                document.getElementById("hfdTabs").value = preTabId;
                document.getElementById("hfdCallFrom").value = "Tab";
                document.getElementById("hfdCurTab").value = CurTabId;
                document.getElementById("hfdPrevTab").value = CurTabId;
                preTabId = CurTabId;
                $("#Button1").click();
            }
            else
                document.getElementById("hfdTabs").value = CurTabId;
        }


        
    </script>--%>

    <script type="text/javascript">
        function Changetab(ctl, tabp) {
            console.log("save", ctl, tabp);
        }
    </script>

    <script type="text/javascript">
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());
        gtag('config', 'G-1VDDWMRSTH');
    </script>

    <script type="text/javascript">
        try {
            fetch(new Request("https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js", { method: 'HEAD', mode: 'no-cors' })).then(function (response) {
                return true;
            }).catch(function (e) {
                var carbonScript = document.createElement("script");
                carbonScript.src = "//cdn.carbonads.com/carbon.js?serve=CK7DKKQU&placement=wwwjqueryscriptnet";
                carbonScript.id = "_carbonads_js";
                document.getElementById("carbon-block").appendChild(carbonScript);
            });
        } catch (error) {
            console.log(error);
        }
    </script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.19.1/moment.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>

                <script type="text/javascript">
                    var today = new Date();
                    var minDate = today.setDate(today.getDate() + 1);

                    $('#datePicker').datetimepicker({
                        useCurrent: false,
                        format: "MM/DD/YYYY",
                        minDate: minDate
                    });

                    var firstOpen = true;
                    var time;

                    $('#timePicker').datetimepicker({
                        useCurrent: false,
                        format: "hh:mm A"
                    }).on('dp.show', function () {
                        if (firstOpen) {
                            time = moment().startOf('day');
                            firstOpen = false;
                        } else {
                            time = "01:00 PM"
                        }

                        $(this).data('DateTimePicker').date(time);
                    });
                </script>--%>
    <%--</div>--%>

    <script type="text/javascript">
        var rangeInput1 = document.getElementById("range1");
        var hiddenField1 = document.getElementById("<%= hdnrange.ClientID %>");

        rangeInput1.addEventListener("change", function () {
            console.log(rangeInput1.value);
            hiddenField1.value = rangeInput1.value;
        });
    </script>
    <script type="text/javascript">
        var rangeInput2 = document.getElementById("range2");
        var hiddenField2 = document.getElementById("<%= Hdnrange2.ClientID %>");

        rangeInput2.addEventListener("change", function () {

            hiddenField2.value = rangeInput2.value;
        });
    </script>

    <script type="text/javascript">

        function setval() {
            var rangeInput1 = document.getElementById("range1");
            var hiddenField1 = document.getElementById("<%= hdnrange.ClientID %>");
            rangeInput1.value = hiddenField1.value;

            var rangeInput2 = document.getElementById("range2");
            var hiddenField2 = document.getElementById("<%= Hdnrange2.ClientID %>");
            rangeInput2.value = hiddenField2.value;
        }

        $(function () {
            setval();

            $('.clsMonthYes').click(function () {
                if ($(this).find('input').prop('checked')) {
                    $(this).closest('td').parent().find('td .clsMonthNo').find('input').prop('checked', false)
                }
            });

            $('.clsMonthNo').click(function () {
                if ($(this).find('input').prop('checked')) {
                    $(this).closest('td').parent().find('td .clsMonthYes').find('input').prop('checked', false)
                }
            });
        });

        function printDiv() {
            console.log("hh")
            debugger

            window.jsPDF = window.jspdf.jsPDF;
            var docPDF = new jsPDF();

            $('#ctl00_ContentPlaceHolder1_tb_Contents_header').find('[id^="ctl00_ContentPlaceHolder1_tb_Contents_tb_Report"]').addClass('ajax__tab_active');
            $('#ctl00_ContentPlaceHolder1_tb_Contents_body').find('[id^="ctl00_ContentPlaceHolder1_tb_Contents_tb_Report"][class="ajax__tab_panel"]').attr('style', 'visibility: visible;')
            //$('#ctl00_ContentPlaceHolder1_tb_Contents_header').find('[class="ajax__tab_active"]').attr('id')
            // Apply styles from classes to textareas and inputs
            //applyStylesFromClasses();
            const textBoxes = content.querySelectorAll('input[type="text"]');
            textBoxes.forEach(function (textBox) {
                textBox.style.backgroundColor = 'white'; // Set the background color to white
                textBox.style.color = 'black'; // Set the text color to black
            });

            var elementHTML = document.querySelector("#ctl00_ContentPlaceHolder1_tb_Contents_body");
            docPDF.html(elementHTML, {
                callback: function (docPDF) {
                    docPDF.save('HTML Linuxhint web page.pdf');
                },
                x: 15,
                y: 15,
                width: 170,
                windowWidth: 1050
            });

            //var printDiv = document.getElementById("ctl00_ContentPlaceHolder1_tb_Contents_tb_Report");
            //newWin = window.open("");
            //newWin.document.write(printDiv.outerHTML);
            //newWin.print();
            //newWin.close();

        }
        function applyStylesFromClasses() {
            // Get all textareas and inputs within the form
            var textareas = document.querySelectorAll('form textarea');
            var inputs = document.querySelectorAll('form input[type="text"]');

            // Iterate through textareas and inputs
            textareas.forEach(function (textarea) {
                // Get the computed styles for the element
                var computedStyles = window.getComputedStyle(textarea);

                // Apply the computed styles to the element
                for (var i = 0; i < computedStyles.length; i++) {
                    var propertyName = computedStyles[i];
                    textarea.style[propertyName] = computedStyles.getPropertyValue(propertyName);
                }
            });

            inputs.forEach(function (input) {
                // Get the computed styles for the element
                var computedStyles = window.getComputedStyle(input);

                // Apply the computed styles to the element
                for (var i = 0; i < computedStyles.length; i++) {
                    var propertyName = computedStyles[i];
                    input.style[propertyName] = computedStyles.getPropertyValue(propertyName);
                }
            });
        }


            //exportPDF('ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16')


            //var doc = new jsPDF();
            //var specialElementHandlers = {
            //    '#editor': function (element, renderer) {
            //        return true;
            //    }
            //};

            ///*$('#cmd').click(function () {*/
            //doc.fromHTML($('#ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16').html(), 150, 150, {
            //        'width': 900,
            //        'elementHandlers': specialElementHandlers
            //    });
            //    doc.save('sample-file.pdf');
            ////});








            //var divToPrint = document.getElementById("ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16");
            //newWin = window.open("");
            //newWin.document.write(divToPrint.outerHTML);
            //newWin.print();
            //newWin.close();




        //var specialElementHandlers = {
        //    // element with id of "bypass" - jQuery style selector
        //    '.no-export': function (element, renderer) {
        //        // true = "handled elsewhere, bypass text extraction"
        //        return true;
        //    }
        //};

        //function exportPDF(id) {
        //    var doc = new jsPDF('p', 'pt', 'a4');
        //    //A4 - 595x842 pts
        //    //https://www.gnu.org/software/gv/manual/html_node/Paper-Keywords-and-paper-size-in-points.html


        //    //Html source 
        //    var source = document.getElementById(id);
        //    console.log(source);
        //    var margins = {
        //        top: 10,
        //        bottom: 10,
        //        left: 10,
        //        width: 400//595
        //    };

        //    doc.fromHTML(
        //        source, // HTML string or DOM elem ref.
        //        margins.left,
        //        margins.top, {
        //        'width': margins.width,
        //        'elementHandlers': specialElementHandlers
        //    },

        //        function (dispose) {
        //            // dispose: object with X, Y of the last line add to the PDF 
        //            //          this allow the insertion of new lines after html
        //            doc.save('Test.pdf');
        //        }, margins);
        //}




    </script>


    <script>

        // Check if the page is already open in another tab
        var screenWidth = window.screen.width;
        var screenHeight = window.screen.height;
        var mobileThreshold = 768;
        if (screenWidth < mobileThreshold) {
            // Execute code for mobile devices
            // For example, you can use a different approach or show different content
            console.log('Mobile resolution detected.');
            // Add your mobile-specific logic here
        }
        else {
            // Execute code for non-mobile devices
            // Use your existing code or another approach suitable for non-mobile devices
            console.log('Non-mobile resolution detected.');
            if (sessionStorage.getItem('pageOpened')) {
                // Display a warning message
                alert('This page is already open in another tab.');
                // Redirect or take appropriate action
                window.location.href = '/SessionRpt/SiView.aspx'; // Redirect to another page
            }
            else {
                // Set a flag in sessionStorage indicating that the page is open
                sessionStorage.setItem('pageOpened', 'true');
                // Add an event listener to handle tab close events
                window.addEventListener('beforeunload', function () {
                    // Clear the flag when the tab is closed
                    sessionStorage.removeItem('pageOpened');
                });
            }
        }
    </script>

    <%--<script>
        // Check if the page is already open in another tab
        var screenWidth = window.screen.width;
        var screenHeight = window.screen.height;
        var mobileThreshold = 768;
        if (screenWidth < mobileThreshold) {
            // Execute code for mobile devices
            // For example, you can use a different approach or show different content
            console.log('Mobile resolution detected.');
            // Add your mobile-specific logic here
        } else {
            // Execute code for non-mobile devices
            // Use your existing code or another approach suitable for non-mobile devices
            console.log('Non-mobile resolution detected.');

            if (document.cookie.includes('pageOpened=true')) {
                // Display a warning message
                alert('This page is already open in another tab.');
                // Redirect or take appropriate action
                window.location.href = '/SessionRpt/SiView.aspx'; // Redirect to another page
            } else {
                // Set a cookie indicating that the page is open
                document.cookie = 'pageOpened=true; path=/'; // Set the cookie path appropriately
                // Add an event listener to handle tab close events
                window.addEventListener('beforeunload', function () {
                    // Clear the cookie when the tab is closed
                    document.cookie = 'pageOpened=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/'; // Clear the cookie
                });
            }
            // Use your existing logic or alternative approaches here
        }
        

    </script>--%>

    <%-- <script>
        // Check if sessionStorage is supported
        if (sessionStorage) {
            // Check if the page is already open in another tab
            if (sessionStorage.getItem('pageOpened')) {
                // Display a warning message
                alert('This page is already open in another tab.');
                // Redirect or take appropriate action
                window.location.href = '/SessionRpt/SiView.aspx'; // Redirect to another page
            } else {
                // Set a flag in sessionStorage indicating that the page is open
                sessionStorage.setItem('pageOpened', 'true');
                // Add an event listener to handle tab close events
                window.addEventListener('beforeunload', function () {
                    // Clear the flag when the tab is closed
                    sessionStorage.removeItem('pageOpened');
                });
            }
        } else {
            // If sessionStorage is not supported, perform a server-side check
            // Send an AJAX request to the server to check if the page is already open
            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/CheckPageOpened', true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    if (xhr.status === 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response.pageOpened) {
                            alert('This page is already open in another tab.');
                            window.location.href = '/SessionRpt/SiView.aspx'; // Redirect to another page
                        }
                    }
                }
            };Physical School
            xhr.send();
        }

    </script>--%>

     <script type="text/javascript">

         var preTabId = ""; var CurTabId = "";

         var nextAfterSave = false;
         let saveInProgress = false;
         $(document).ready(function () {
             if (!$("#hfdPrevTab").val())
                 $("#hfdPrevTab").val("tb_Report1");

             if (!$("#hfdCurTab").val())
                 $("#hfdCurTab").val("tb_Report1");

             // ✅ show message after reload (Final Submit)
             var msg = sessionStorage.getItem("afterReloadMsg");
             var type = sessionStorage.getItem("afterReloadType");

             if (msg) {
                 showAlert(msg, parseInt(type || "1"));
                 sessionStorage.removeItem("afterReloadMsg");
                 sessionStorage.removeItem("afterReloadType");
             }

             // =========================
             // Range 1
             // =========================
             $("#range1").on("input change", function () {
                 $("#<%= hdnrange.ClientID %>").val($(this).val());
            });

            $("#range2").on("input change", function () {
                $("#<%= Hdnrange2.ClientID %>").val($(this).val());
            });

            $("#btnSaveNext").on("click", function (e) {
                e.preventDefault();

                var curTab = getCurrentTabId();

                nextAfterSave = true;
                $("#hfdTabs").val(curTab);
                $("#hfdCurTab").val(curTab);
                $("#hfdCallFrom").val("SaveNext");

                SaveTabById(curTab, false); // ✅ modal handled inside SaveTab
            });

            $("#btnFinalSubmit").off("click").on("click", function (e) {
                e.preventDefault();



                nextAfterSave = false;

                var curTab = getCurrentTabId();

                $("#hfdTabs").val(curTab);
                $("#hfdCurTab").val(curTab);
                $("#hfdCallFrom").val("Submit");

                SaveTabById(curTab, true);
            });

        });

         function clientActiveTabChanged(sender, args) {

             try {

                 var tab = sender.get_tabs()[sender.get_activeTabIndex()];
                 CurTabId = tab.get_id();

                 $("#hfdCurTab").val(CurTabId);

                 var prevTab = $("#hfdPrevTab").val();

                 if (!prevTab || prevTab === "undefined")
                     prevTab = "tb_Report1";

                 if (prevTab !== CurTabId) {

                     $("#hfdTabs").val(prevTab);
                     $("#hfdCallFrom").val("Tab");
                     $("#hfdPrevTab").val(CurTabId);

                     var formData = { Tab: prevTab };

                     if (saveInProgress) return;

                     SaveTabById(prevTab, false);
                 }

             } catch (ex) {
                 console.log("clientActiveTabChanged error:", ex);
             }
         }


         // =========================
         // TAB HELPERS
         // =========================
         function getCurrentTabId() {
             var cur = $("#hfdCurTab").val();
             if (!cur || cur === "undefined") cur = "tb_Report1";
             return cur;
         }

         function getPreviousTabId() {
             var prev = $("#hfdPrevTab").val();
             if (!prev || prev === "undefined") prev = "tb_Report1";
             return prev;
         }

         // =========================
         // MAIN TAB SAVE SWITCH
         // =========================
         function SaveTabById(tabId, reloadAfterSave) {
             if (saveInProgress) {
                 console.warn("Save blocked: already in progress");
                 return;
             }

             switch (tabId) {

                 case "tb_Report1":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab":
                     SaveTab1(reloadAfterSave);
                     break;

                 case "tb_Report6":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report6_tab":
                     SaveTab2(reloadAfterSave);
                     break;

                 case "tb_Report7":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report7_tab":
                     SaveTab3(reloadAfterSave);
                     break;

                 case "tb_Report3":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report3_tab":
                     SaveTab4(reloadAfterSave);
                     break;

                 case "tb_Report4":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report4_tab":
                     SaveTab5(reloadAfterSave);
                     break;

                 case "tb_Report5":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report5_tab":
                     SaveTab6(reloadAfterSave);
                     break;

                 case "tb_Report8":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report8_tab":
                     SaveTab7(reloadAfterSave);
                     break;

                 case "tb_Report11":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report11_tab":
                     SaveTab8(reloadAfterSave);
                     break;

                 case "tb_Report9":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report9_tab":
                     SaveTab9(reloadAfterSave);
                     break;

                 case "tb_Report12":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report12_tab":
                     SaveTab10(reloadAfterSave);
                     break;

                 case "tb_Report10":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report10_tab":
                     SaveTab11(reloadAfterSave);
                     break;

                 case "tb_Report18":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report18_tab":
                     SaveTab12(reloadAfterSave);
                     break;

                 case "tb_Report13":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report13_tab":
                     SaveTab13(reloadAfterSave);
                     break;

                 case "tb_Report16":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report16_tab":
                     SaveTab14(reloadAfterSave);
                     break;

                 case "tb_Report15":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report15_tab":
                     SaveTab15(reloadAfterSave);
                     break;

                 case "tb_Report17":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report17_tab":
                     SaveTab16(reloadAfterSave);
                     break;

                 case "tb_Report20":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report20_tab":
                     SaveTab17(reloadAfterSave);
                     break;

                 case "tb_Report19":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report19_tab":
                     SaveTab18(reloadAfterSave);
                     break;

                 case "tb_Report21":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report21_tab":
                     SaveTab19(reloadAfterSave);
                     break;

                 case "tb_Report14":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report14_tab":
                     SaveTab20(reloadAfterSave);
                     break;

                 case "tb_Report22":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report22":
                 case "ctl00_ContentPlaceHolder1_tb_Contents_tb_Report22_tab":
                     SaveTab21(reloadAfterSave);
                     break;

                 default:
                     console.log("No save function for tab:", tabId);
                     break;
             }

         }

         // =========================
         // ONE COMMON AJAX FUNCTION
         // =========================
         function PostToHandler(formData, reloadAfterSave, tabName) {
             if (saveInProgress) return;

             saveInProgress = true;
             $("#btnSaveNext").prop("disabled", true);
             $("#btnFinalSubmit").prop("disabled", true);

             $.ajax({
                 type: "POST",
                 url: "<%= ResolveUrl("~/Handler/SaveReportSi_2023.ashx") %>",
                data: formData,

                success: function (res) {

                    var arr = (res || "").split("|");

                    if (arr[0] === "OK") {

                        // ✅ If Final Submit -> reload first, then show message
                        if (reloadAfterSave === true) {
                            sessionStorage.setItem("afterReloadMsg", (tabName || "Tab") + " Saved Successfully");
                            sessionStorage.setItem("afterReloadType", "1");
                            location.reload();
                            return;
                        }

                        // ✅ Normal save -> show message now
                        showAlert((tabName || "Tab") + " Saved Successfully", 1);

                        // ✅ Save&Next -> go next tab
                        if (nextAfterSave === true) {
                            nextAfterSave = false;

                            try {
                                var tabStrip = $find("<%= tb_Contents.ClientID %>"); // ajaxToolkit TabContainer
                                if (tabStrip) {
                                    var idx = tabStrip.get_activeTabIndex();
                                    var tabs = tabStrip.get_tabs();
                                    var total = tabs.length;

                                    if (idx < total - 1) {
                                        tabStrip.set_activeTabIndex(idx + 1);
                                    }
                                }
                            } catch (ex) {
                                console.log("Next tab error:", ex);
                            }
                        }

                    } else {
                        showAlert(arr[1] || res, 2);
                    }

                    $("#btnSaveNext").prop("disabled", false);
                    $("#btnFinalSubmit").prop("disabled", false);
                    isSaving = false;
                },

                error: function (xhr) {

                    $("#btnSaveNext").prop("disabled", false);
                    $("#btnFinalSubmit").prop("disabled", false);

                    console.log("AJAX ERROR:", xhr.status, xhr.responseText);
                    showAlert((tabName || "Tab") + " Save Failed!", 2);
                    isSaving = false;
                },
                complete: function () {
                    saveInProgress = false; // ✅ keep only this
                }
            });
         }

         // =========================
         // TAB 1 SAVE FUNCTION
         // =========================
         function SaveTab1(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 1;

            formData.ClinicalObsevation = $("#<%= ClinicleObse_txt.ClientID %>").val();

             var list = [];

             // ✅ use option_box_single_choice as container (it exists in HTML)
             $("#option_box_single_choice .cloneThisRow").each(function () {

                 var siId = $(this).find("input[id$='txtSI_ID']").val() || "0";

                 // because MultiLine => textarea
                 var time = $(this).find("textarea[id$='txtTIME']").val() || "";
                 var act = $(this).find("textarea[id$='txtACTIVITIES']").val() || "";
                 var com = $(this).find("textarea[id$='txtCOMMENTS']").val() || "";

                 if ($.trim(time) !== "" || $.trim(act) !== "" || $.trim(com) !== "" || siId !== "0") {
                     list.push({
                         SI_ID: siId,
                         TIME: time,
                         ACTIVITIES: act,
                         COMMENTS: com
                     });
                 }
             });

             formData.TimelineJson = JSON.stringify(list);

             saveWithModal(formData, reloadAfterSave, "CLINICAL_OBSERVATION AND DAILY SCHEDULE ");
         }
         function SaveTab2(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 2;

            // 1) Mother quality time daily (only one checked)
            formData.FamilyStructure_QualityTimeMother =
                getCheckedText(
            "#<%= FamilyStructure_QualityTimeMother_1.ClientID %>",
            "#<%= FamilyStructure_QualityTimeMother_2.ClientID %>",
            "#<%= FamilyStructure_QualityTimeMother_3.ClientID %>"
                );

            // 2) Father quality time daily
            formData.FamilyStructure_QualityTimeFather =
                getCheckedText(
            "#<%= FamilyStructure_QualityTimeFather_1.ClientID %>",
            "#<%= FamilyStructure_QualityTimeFather_2.ClientID %>",
            "#<%= FamilyStructure_QualityTimeFather_3.ClientID %>"
                );

            // 3) Mother weekends
            formData.Mother_Weekends =
                getCheckedText(
            "#<%= Mother_Weekends_1.ClientID %>",
            "#<%= Mother_Weekends_2.ClientID %>",
            "#<%= Mother_Weekends_3.ClientID %>"
                );

            // 4) Father weekends
            formData.Father_Weekends =
                getCheckedText(
            "#<%= Father_Weekends_1.ClientID %>",
            "#<%= Father_Weekends_2.ClientID %>",
            "#<%= Father_Weekends_3.ClientID %>"
                );

            // 5) Willingness to devote time for therapy (Yes/No)
            formData.FamilyStructure_TimeForThreapy =
                getCheckedText(
                  "#<%= FamilyStructure_TimeForThreapy_1.ClientID %>",
                  "#<%= FamilyStructure_TimeForThreapy_2.ClientID %>"
                );

            // 6) Acceptance condition (Yes/No)
            formData.FamilyStructure_AcceptanceCondition =
                getCheckedText(
                  "#<%= FamilyStructure_AcceptanceCondition_1.ClientID %>",
                  "#<%= FamilyStructure_AcceptanceCondition_2.ClientID %>"
                );

            // 7) Extra curricular (Yes/No)
            formData.FamilyStructure_ExtraCaricular =
                getCheckedText(
            "#<%= FamilyStructure_ExtraCaricular_1.ClientID %>",
            "#<%= FamilyStructure_ExtraCaricular_2.ClientID %>"
                );

            // TextAreas
            formData.FamilyStructure_Diciplinary = $("#<%= FamilyStructure_Diciplinary.ClientID %>").val();
            formData.FamilyStructure_SiblingBrother = $("#<%= FamilyStructure_SiblingBrother.ClientID %>").val();
            formData.FamilyStructure_Expectations = $("#<%= FamilyStructure_Expectations.ClientID %>").val();
            formData.FamilyStructure_CloselyInvolved = $("#<%= FamilyStructure_CloselyInvolved.ClientID %>").val();
            formData.FAMILY_cmt = $("#<%= FAMILY_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "FAMILY STRUCTURE");
         }
         function SaveTab3(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 3;

            // 1) Does child attend school (Yes/No)
            formData.Schoolinfo_Attend = "";
            if ($("#<%= Schoolinfo_Attend_1.ClientID %>").is(":checked")) formData.Schoolinfo_Attend = "Yes";
            if ($("#<%= Schoolinfo_Attend_2.ClientID %>").is(":checked")) formData.Schoolinfo_Attend = "No";

            // 2) Type of school (Open/Integrated/Special)
            formData.Schoolinfo_Type = "";
            if ($("#<%= Schoolinfo_Type_1.ClientID %>").is(":checked")) formData.Schoolinfo_Type = "Open";
            if ($("#<%= Schoolinfo_Type_2.ClientID %>").is(":checked")) formData.Schoolinfo_Type = "Integrated";
            if ($("#<%= Schoolinfo_Type_3.ClientID %>").is(":checked")) formData.Schoolinfo_Type = "Special";

            // 3) School Hours dropdown
            formData.Schoolinfo_SchoolHours = $("#<%= Schoolinfo_SchoolHours.ClientID %>").val();
            var travelModes = [];
            if ($("#<%= chkSchool_Bus.ClientID %>").is(":checked"))
                travelModes.push("School_bus");

            if ($("#<%= chkCar.ClientID %>").is(":checked"))
                travelModes.push("Car");

            if ($("#<%= chkTwo_Wheelers.ClientID %>").is(":checked"))
                travelModes.push("Two_wheelers");

            if ($("#<%= chkwalking.ClientID %>").is(":checked"))
                travelModes.push("Walking");

            if ($("#<%= chkPublic_Transport.ClientID %>").is(":checked"))
                travelModes.push("Public_transport");
            formData.School_Travel_Mode = travelModes.join(",");

            // 5) Teacher ratio (1 to 5 / 1 to 30 / 1 to 60 / >60)
            formData.Schoolinfo_NoOfTeacher = "";
            if ($("#<%= Schoolinfo_NoOfTeacher_1.ClientID %>").is(":checked")) formData.Schoolinfo_NoOfTeacher = "1 to 5";
            if ($("#<%= Schoolinfo_NoOfTeacher_2.ClientID %>").is(":checked")) formData.Schoolinfo_NoOfTeacher = "1 to 30";
            if ($("#<%= Schoolinfo_NoOfTeacher_3.ClientID %>").is(":checked")) formData.Schoolinfo_NoOfTeacher = "1 to 60";
            if ($("#<%= Schoolinfo_NoOfTeacher_4.ClientID %>").is(":checked")) formData.Schoolinfo_NoOfTeacher = "more than 60";

            // 6) Seating arrangement (Floor/Single_bench/Bench2/Round_table)
            var seating = [];
            if ($("#<%= chkFloor.ClientID %>").is(":checked")) seating.push("Floor");
            if ($("#<%= chksingle_bench.ClientID %>").is(":checked")) seating.push("Single_bench");
            if ($("#<%= chkbench2.ClientID %>").is(":checked")) seating.push("Bench2");
            if ($("#<%= chkround_table.ClientID %>").is(":checked")) seating.push("Round_table");

            formData.Seating_Type = seating.join(",");


            // 7) Meal time dropdown
            formData.Schoolinfo_Mealtime = $("#<%= Schoolinfo_Mealtime.ClientID %>").val();

            // 8) Meal type (Provided/Tiffin)
            formData.Schoolinfo_MealType = "";
            if ($("#<%= Schoolinfo_MealType_1.ClientID %>").is(":checked")) formData.Schoolinfo_MealType = "Provided by school";
            if ($("#<%= Schoolinfo_MealType_2.ClientID %>").is(":checked")) formData.Schoolinfo_MealType = "Tiffin carried from home";

            // 9) Sharing done (Yes/No/NA)
            formData.Schoolinfo_Shareing = "";
            if ($("#<%= Schoolinfo_Shareing_1.ClientID %>").is(":checked")) formData.Schoolinfo_Shareing = "Yes";
            if ($("#<%= Schoolinfo_Shareing_2.ClientID %>").is(":checked")) formData.Schoolinfo_Shareing = "No";
            if ($("#<%= Schoolinfo_Shareing_3.ClientID %>").is(":checked")) formData.Schoolinfo_Shareing = "NA";

            // 10) Help required eating (Yes/No)
            formData.Schoolinfo_HelpEating = "";
            if ($("#<%= Schoolinfo_HelpEating_1.ClientID %>").is(":checked")) formData.Schoolinfo_HelpEating = "Yes";
            if ($("#<%= Schoolinfo_HelpEating_2.ClientID %>").is(":checked")) formData.Schoolinfo_HelpEating = "No";

            // 11) Friendship initiated (Yes/No)
            formData.Schoolinfo_Friendship = "";
            if ($("#<%= Schoolinfo_Friendship_1.ClientID %>").is(":checked")) formData.Schoolinfo_Friendship = "Yes";
            if ($("#<%= Schoolinfo_Friendship_2.ClientID %>").is(":checked")) formData.Schoolinfo_Friendship = "No";

            // 12) Interaction with peers (Yes/No)
            formData.Schoolinfo_InteractionPeer = "";
            if ($("#<%= Schoolinfo_InteractionPeer_1.ClientID %>").is(":checked")) formData.Schoolinfo_InteractionPeer = "Yes";
            if ($("#<%= Schoolinfo_InteractionPeer_2.ClientID %>").is(":checked")) formData.Schoolinfo_InteractionPeer = "No";

            // 13) Interaction with teacher (Yes/No)
            formData.Schoolinfo_InteractionTeacher = "";
            if ($("#<%= Schoolinfo_InteractionTeacher_1.ClientID %>").is(":checked")) formData.Schoolinfo_InteractionTeacher = "Yes";
            if ($("#<%= Schoolinfo_InteractionTeacher_2.ClientID %>").is(":checked")) formData.Schoolinfo_InteractionTeacher = "No";

            // 14) Annual function (Yes/No)
            formData.Schoolinfo_AnnualFunction = "";
            if ($("#<%= Schoolinfo_AnnualFunction_1.ClientID %>").is(":checked")) formData.Schoolinfo_AnnualFunction = "Yes";
            if ($("#<%= Schoolinfo_AnnualFunction_2.ClientID %>").is(":checked")) formData.Schoolinfo_AnnualFunction = "No";

            // 15) Sports (Yes/No)
            formData.Schoolinfo_Sports = "";
            if ($("#<%= Schoolinfo_Sports_1.ClientID %>").is(":checked")) formData.Schoolinfo_Sports = "Yes";
            if ($("#<%= Schoolinfo_Sports_2.ClientID %>").is(":checked")) formData.Schoolinfo_Sports = "No";

            // 16) Picnic (Yes/No)
            formData.Schoolinfo_Picnic = "";
            if ($("#<%= Schoolinfo_Picnic_1.ClientID %>").is(":checked")) formData.Schoolinfo_Picnic = "Yes";
            if ($("#<%= Schoolinfo_Picnic_2.ClientID %>").is(":checked")) formData.Schoolinfo_Picnic = "No";

            // 17) Extra curricular (Yes/No)
            formData.Schoolinfo_ExtraCaricular = "";
            if ($("#<%= Schoolinfo_ExtraCaricular_1.ClientID %>").is(":checked")) formData.Schoolinfo_ExtraCaricular = "Yes";
            if ($("#<%= Schoolinfo_ExtraCaricular_2.ClientID %>").is(":checked")) formData.Schoolinfo_ExtraCaricular = "No";

            // 18) Copying from board (Yes/No/Inconsistent/NA)
            formData.Schoolinfo_CopyBoard = "";
            if ($("#<%= Schoolinfo_CopyBoard_1.ClientID %>").is(":checked")) formData.Schoolinfo_CopyBoard = "Yes";
            if ($("#<%= Schoolinfo_CopyBoard_2.ClientID %>").is(":checked")) formData.Schoolinfo_CopyBoard = "No";
            if ($("#<%= Schoolinfo_CopyBoard_3.ClientID %>").is(":checked")) formData.Schoolinfo_CopyBoard = "Inconsistent";
            if ($("#<%= Schoolinfo_CopyBoard_4.ClientID %>").is(":checked")) formData.Schoolinfo_CopyBoard = "NA";

            // 19) Follows instructions (Yes/No/Sometime/NA)
            formData.Schoolinfo_Instructions = "";
            if ($("#<%= Schoolinfo_Instructions_1.ClientID %>").is(":checked")) formData.Schoolinfo_Instructions = "Yes";
            if ($("#<%= Schoolinfo_Instructions_2.ClientID %>").is(":checked")) formData.Schoolinfo_Instructions = "No";
            if ($("#<%= Schoolinfo_Instructions_3.ClientID %>").is(":checked")) formData.Schoolinfo_Instructions = "Sometime";
            if ($("#<%= Schoolinfo_Instructions_4.ClientID %>").is(":checked")) formData.Schoolinfo_Instructions = "NA";

            // 20) Shadow teacher (Yes/No/Needs Help/NA)
            formData.Schoolinfo_ShadowTeacher = "";
            if ($("#<%= Schoolinfo_ShadowTeacher_1.ClientID %>").is(":checked")) formData.Schoolinfo_ShadowTeacher = "Yes";
            if ($("#<%= Schoolinfo_ShadowTeacher_2.ClientID %>").is(":checked")) formData.Schoolinfo_ShadowTeacher = "No";
            if ($("#<%= Schoolinfo_ShadowTeacher_3.ClientID %>").is(":checked")) formData.Schoolinfo_ShadowTeacher = "Needs Help";
            if ($("#<%= Schoolinfo_ShadowTeacher_4.ClientID %>").is(":checked")) formData.Schoolinfo_ShadowTeacher = "NA";

            // 21) Completing CW/HW (Yes/No/Needs Help/NA)
            formData.Schoolinfo_CW_HW = "";
            if ($("#<%= Schoolinfo_CW_HW_1.ClientID %>").is(":checked")) formData.Schoolinfo_CW_HW = "Yes";
            if ($("#<%= Schoolinfo_CW_HW_2.ClientID %>").is(":checked")) formData.Schoolinfo_CW_HW = "No";
            if ($("#<%= Schoolinfo_CW_HW_3.ClientID %>").is(":checked")) formData.Schoolinfo_CW_HW = "Needs Help";
            if ($("#<%= Schoolinfo_CW_HW_4.ClientID %>").is(":checked")) formData.Schoolinfo_CW_HW = "NA";

            // 22) Special educator (Yes/No/NA)
            formData.Schoolinfo_SpecialEducator = "";
            if ($("#<%= Schoolinfo_SpecialEducator_1.ClientID %>").is(":checked")) formData.Schoolinfo_SpecialEducator = "Yes";
            if ($("#<%= Schoolinfo_SpecialEducator_2.ClientID %>").is(":checked")) formData.Schoolinfo_SpecialEducator = "No";
            if ($("#<%= Schoolinfo_SpecialEducator_3.ClientID %>").is(":checked")) formData.Schoolinfo_SpecialEducator = "NA";

            // 23) Mode of delivery (PPT/Videos/Books/NOTA)
            formData.Schoolinfo_DeliveryInformation = "";
            if ($("#<%= Schoolinfo_DeliveryInformation_1.ClientID %>").is(":checked")) formData.Schoolinfo_DeliveryInformation = "PPT";
            if ($("#<%= Schoolinfo_DeliveryInformation_2.ClientID %>").is(":checked")) formData.Schoolinfo_DeliveryInformation = "Videos";
            if ($("#<%= Schoolinfo_DeliveryInformation_3.ClientID %>").is(":checked")) formData.Schoolinfo_DeliveryInformation = "Books";
            if ($("#<%= Schoolinfo_DeliveryInformation_4.ClientID %>").is(":checked")) formData.Schoolinfo_DeliveryInformation = "NOTA";

            // 24) Remark + Comment textboxes
            formData.Schoolinfo_RemarkTeacher = $("#<%= Schoolinfo_RemarkTeacher.ClientID %>").val();
            formData.SCHOOL_cmt = $("#<%= SCHOOL_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "SCHOOL INFORMATION");
         }
         function SaveTab4(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 4;

            // A) Relationship with self

            // 1) Current place (Yes/No/Sometimes)
            formData.PersonalSocial_CurrentPlace = "";
            if ($("#<%= PersonalSocial_CurrentPlace_1.ClientID %>").is(":checked")) formData.PersonalSocial_CurrentPlace = "Yes";
            if ($("#<%= PersonalSocial_CurrentPlace_2.ClientID %>").is(":checked")) formData.PersonalSocial_CurrentPlace = "No";
            if ($("#<%= PersonalSocial_CurrentPlace_3.ClientID %>").is(":checked")) formData.PersonalSocial_CurrentPlace = "Sometimes";

            // 2) What he/she does (Yes/No/Sometimes)
            formData.PersonalSocial_WhatHeDoes = "";
            if ($("#<%= PersonalSocial_WhatHeDoes_1.ClientID %>").is(":checked")) formData.PersonalSocial_WhatHeDoes = "Yes";
            if ($("#<%= PersonalSocial_WhatHeDoes_2.ClientID %>").is(":checked")) formData.PersonalSocial_WhatHeDoes = "No";
            if ($("#<%= PersonalSocial_WhatHeDoes_3.ClientID %>").is(":checked")) formData.PersonalSocial_WhatHeDoes = "Sometimes";

            // 3) Body awareness (Yes/No/Sometimes)
            formData.PersonalSocial_BodyAwareness = "";
            if ($("#<%= PersonalSocial_BodyAwareness_1.ClientID %>").is(":checked")) formData.PersonalSocial_BodyAwareness = "Yes";
            if ($("#<%= PersonalSocial_BodyAwareness_2.ClientID %>").is(":checked")) formData.PersonalSocial_BodyAwareness = "No";
            if ($("#<%= PersonalSocial_BodyAwareness_3.ClientID %>").is(":checked")) formData.PersonalSocial_BodyAwareness = "Sometimes";

            // 4) Body schema (Yes/No/Sometimes)
            formData.PersonalSocial_BodySchema = "";
            if ($("#<%= PersonalSocial_BodySchema_1.ClientID %>").is(":checked")) formData.PersonalSocial_BodySchema = "Yes";
            if ($("#<%= PersonalSocial_BodySchema_2.ClientID %>").is(":checked")) formData.PersonalSocial_BodySchema = "No";
            if ($("#<%= PersonalSocial_BodySchema_3.ClientID %>").is(":checked")) formData.PersonalSocial_BodySchema = "Sometimes";

            // 5) Explore environment (Yes/No/Sometimes)
            formData.PersonalSocial_ExploreEnvironment = "";
            if ($("#<%= PersonalSocial_ExploreEnvironment_1.ClientID %>").is(":checked")) formData.PersonalSocial_ExploreEnvironment = "Yes";
            if ($("#<%= PersonalSocial_ExploreEnvironment_2.ClientID %>").is(":checked")) formData.PersonalSocial_ExploreEnvironment = "No";
            if ($("#<%= PersonalSocial_ExploreEnvironment_3.ClientID %>").is(":checked")) formData.PersonalSocial_ExploreEnvironment = "Sometimes";

            // 6) Motivated (Yes/No/Sometimes)
            formData.PersonalSocial_Motivated = "";
            if ($("#<%= PersonalSocial_Motivated_1.ClientID %>").is(":checked")) formData.PersonalSocial_Motivated = "Yes";
            if ($("#<%= PersonalSocial_Motivated_2.ClientID %>").is(":checked")) formData.PersonalSocial_Motivated = "No";
            if ($("#<%= PersonalSocial_Motivated_3.ClientID %>").is(":checked")) formData.PersonalSocial_Motivated = "Sometimes";


            // B) Relationship with others

            // 1) Eye contact (Fleeting/Poor/Fair/Good)
            formData.PersonalSocial_EyeContact = "";
            if ($("#<%= PersonalSocial_EyeContact_1.ClientID %>").is(":checked")) formData.PersonalSocial_EyeContact = "Fleeting";
            if ($("#<%= PersonalSocial_EyeContact_2.ClientID %>").is(":checked")) formData.PersonalSocial_EyeContact = "Poor";
            if ($("#<%= PersonalSocial_EyeContact_3.ClientID %>").is(":checked")) formData.PersonalSocial_EyeContact = "Fair";
            if ($("#<%= PersonalSocial_EyeContact_4.ClientID %>").is(":checked")) formData.PersonalSocial_EyeContact = "Good";

            // 2) Social smile (Fleeting/Poor/Fair/Good)
            formData.PersonalSocial_SocialSmile = "";
            if ($("#<%= PersonalSocial_SocialSmile_1.ClientID %>").is(":checked")) formData.PersonalSocial_SocialSmile = "Fleeting";
            if ($("#<%= PersonalSocial_SocialSmile_2.ClientID %>").is(":checked")) formData.PersonalSocial_SocialSmile = "Poor";
            if ($("#<%= PersonalSocial_SocialSmile_3.ClientID %>").is(":checked")) formData.PersonalSocial_SocialSmile = "Fair";
            if ($("#<%= PersonalSocial_SocialSmile_4.ClientID %>").is(":checked")) formData.PersonalSocial_SocialSmile = "Good";

            // 3) Family regards (Yes/No)
            formData.PersonalSocial_FamilyRegards = "";
            if ($("#<%= PersonalSocial_FamilyRegards_1.ClientID %>").is(":checked")) formData.PersonalSocial_FamilyRegards = "Yes";
            if ($("#<%= PersonalSocial_FamilyRegards_2.ClientID %>").is(":checked")) formData.PersonalSocial_FamilyRegards = "No";

            // Child socially (Difficult/Good/Okay/Really good/Fantastic)
            formData.PersonalSocial_ChildSocially = "";
            if ($("#<%= PersonalSocial_ChildSocially_1.ClientID %>").is(":checked")) formData.PersonalSocial_ChildSocially = "Difficult to handle";
            if ($("#<%= PersonalSocial_ChildSocially_2.ClientID %>").is(":checked")) formData.PersonalSocial_ChildSocially = "Good";
            if ($("#<%= PersonalSocial_ChildSocially_3.ClientID %>").is(":checked")) formData.PersonalSocial_ChildSocially = "Okay";
            if ($("#<%= PersonalSocial_ChildSocially_4.ClientID %>").is(":checked")) formData.PersonalSocial_ChildSocially = "Really good";
            if ($("#<%= PersonalSocial_ChildSocially_5.ClientID %>").is(":checked")) formData.PersonalSocial_ChildSocially = "Fantastic";

            // Comment textbox
            formData.PERSONAL_cmt = $("#<%= PERSONAL_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "PERSONAL SOCIAL");
         }
         function SaveTab5(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 5;

            // Textboxes
            formData.SpeechLanguage_StartSpeek = $("#<%= SpeechLanguage_StartSpeek.ClientID %>").val();
            formData.SpeechLanguage_Monosyllables = $("#<%= SpeechLanguage_Monosyllables.ClientID %>").val();
            formData.SpeechLanguage_Bisyllables = $("#<%= SpeechLanguage_Bisyllables.ClientID %>").val();
            formData.SpeechLanguage_ShrotScentences = $("#<%= SpeechLanguage_ShrotScentences.ClientID %>").val();
            formData.SpeechLanguage_LongScentences = $("#<%= SpeechLanguage_LongScentences.ClientID %>").val();

            // 6) Unusual sounds / Jargon speech (Yes/No)
            formData.SpeechLanguage_UnusualSoundsJargonSpeech = "";
            if ($("#<%= SpeechLanguage_UnusualSoundsJargonSpeech_1.ClientID %>").is(":checked")) formData.SpeechLanguage_UnusualSoundsJargonSpeech = "Yes";
            if ($("#<%= SpeechLanguage_UnusualSoundsJargonSpeech_2.ClientID %>").is(":checked")) formData.SpeechLanguage_UnusualSoundsJargonSpeech = "No";

            // 7) Imitation of speech / Gestures (Yes/No)
            formData.SpeechLanguage_speechgestures = "";
            if ($("#<%= SpeechLanguage_speechgestures_1.ClientID %>").is(":checked")) formData.SpeechLanguage_speechgestures = "Yes";
            if ($("#<%= SpeechLanguage_speechgestures_2.ClientID %>").is(":checked")) formData.SpeechLanguage_speechgestures = "No";

            // Textboxes
            formData.SpeechLanguage_NonverbalfacialExpression = $("#<%= SpeechLanguage_NonverbalfacialExpression.ClientID %>").val();
            formData.SpeechLanguage_NonverbalfacialEyeContact = $("#<%= SpeechLanguage_NonverbalfacialEyeContact.ClientID %>").val();
            formData.SpeechLanguage_NonverbalfacialGestures = $("#<%= SpeechLanguage_NonverbalfacialGestures.ClientID %>").val();

            formData.SpeechLanguage_SimpleComplex = $("#<%= SpeechLanguage_SimpleComplex.ClientID %>").val();
            formData.SpeechLanguage_UnderstandImpliedMeaning = $("#<%= SpeechLanguage_UnderstandImpliedMeaning.ClientID %>").val();
            formData.SpeechLanguage_UnderstandJokesarcasm = $("#<%= SpeechLanguage_UnderstandJokesarcasm.ClientID %>").val();
            formData.SpeechLanguage_Respondstoname = $("#<%= SpeechLanguage_Respondstoname.ClientID %>").val();

            // 15) Two way interaction (Yes/No/Sometimes)
            formData.SpeechLanguage_TwowayInteraction = "";
            if ($("#<%= SpeechLanguage_TwowayInteraction_1.ClientID %>").is(":checked")) formData.SpeechLanguage_TwowayInteraction = "Yes";
            if ($("#<%= SpeechLanguage_TwowayInteraction_2.ClientID %>").is(":checked")) formData.SpeechLanguage_TwowayInteraction = "No";
            if ($("#<%= SpeechLanguage_TwowayInteraction_3.ClientID %>").is(":checked")) formData.SpeechLanguage_TwowayInteraction = "Sometimes";

            // Textboxes
            formData.SpeechLanguage_NarrateIncidentsAtSchool = $("#<%= SpeechLanguage_NarrateIncidentsAtSchool.ClientID %>").val();
            formData.SpeechLanguage_NarrateIncidentsAtHome = $("#<%= SpeechLanguage_NarrateIncidentsAtHome.ClientID %>").val();

            formData.SpeechLanguage_Needs = $("#<%= SpeechLanguage_Needs.ClientID %>").val();
            formData.SpeechLanguage_Emotions = $("#<%= SpeechLanguage_Emotions.ClientID %>").val();
            formData.SpeechLanguage_AchievementsFailure = $("#<%= SpeechLanguage_AchievementsFailure.ClientID %>").val();

            formData.SpeechLanguage_Echolalia = $("#<%= SpeechLanguage_Echolalia.ClientID %>").val();

            // Comment
            formData.Speech_cmt = $("#<%= Speech_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "SPEECH AND LANGUAGE");
         }
         function SaveTab6(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 6;

            formData.Behaviour_FreeTime = $("#<%= Behaviour_FreeTime.ClientID %>").val();

            formData.unassociated = $("#<%= chkunassociated.ClientID %>").is(":checked") ? "unassociated" : "";
            formData.solitary = $("#<%= chksolitary.ClientID %>").is(":checked") ? "solitary" : "";
            formData.onlooker = $("#<%= chkonlooker.ClientID %>").is(":checked") ? "onlooker" : "";
            formData.parallel = $("#<%= chkparallel.ClientID %>").is(":checked") ? "parallel" : "";
            formData.associative = $("#<%= chkassociative.ClientID %>").is(":checked") ? "associative" : "";
            formData.cooperative = $("#<%= chkcooperative.ClientID %>").is(":checked") ? "cooperative" : "";


            formData.Behaviour_situationalmeltdowns = "";
            if ($("#<%= Behaviour_situationalmeltdowns_1.ClientID %>").is(":checked")) formData.Behaviour_situationalmeltdowns = "Yes";
            if ($("#<%= Behaviour_situationalmeltdowns_2.ClientID %>").is(":checked")) formData.Behaviour_situationalmeltdowns = "No";
            if ($("#<%= Behaviour_situationalmeltdowns_3.ClientID %>").is(":checked")) formData.Behaviour_situationalmeltdowns = "Sometimes";

            formData.BEHAVIOUR_cmt = $("#<%= BEHAVIOUR_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "BEHAVIOUR");
         }
         function SaveTab7(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 7;

            // sliders hidden values
            formData.rangevalue = $("#<%= hdnrange.ClientID %>").val() || "0";
            formData.rangevalue2 = $("#<%= Hdnrange2.ClientID %>").val() || "0";

            // 3) Responds to stimuli? (Yes/No/Sometimes)
            formData.Arousal_Stimuli = "";
            if ($("#<%= Arousal_Stimuli_1.ClientID %>").is(":checked")) formData.Arousal_Stimuli = "Yes";
            if ($("#<%= Arousal_Stimuli_2.ClientID %>").is(":checked")) formData.Arousal_Stimuli = "No";
            if ($("#<%= Arousal_Stimuli_3.ClientID %>").is(":checked")) formData.Arousal_Stimuli = "Sometimes";

            // 4) Maintainance Of arousal during transition (Yes/No)
            formData.Arousal_Transition = "";
            if ($("#<%= Arousal_Transition_1.ClientID %>").is(":checked")) formData.Arousal_Transition = "Yes";
            if ($("#<%= Arousal_Transition_2.ClientID %>").is(":checked")) formData.Arousal_Transition = "No";

            // 5-7 Textboxes
            formData.Arousal_FactorOCD = $("#<%= Arousal_FactorOCD.ClientID %>").val();
            formData.Arousal_ClaimingFactor = $("#<%= Arousal_ClaimingFactor.ClientID %>").val();
            formData.Arousal_DipsDown = $("#<%= Arousal_DipsDown.ClientID %>").val();

            // Comments
            formData.AROUSAL_cmt = $("#<%= AROUSAL_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "AROUSAL");
         }
         function SaveTab8(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 8;

            // 1) Wide range of emotion (Yes/No)
            formData.Affect_RangeEmotion = "";
            if ($("#<%= Affect_RangeEmotion_1.ClientID %>").is(":checked")) formData.Affect_RangeEmotion = "Yes";
            if ($("#<%= Affect_RangeEmotion_2.ClientID %>").is(":checked")) formData.Affect_RangeEmotion = "No";

            // 2) Able to express emotion (Yes/No)
            formData.Affect_ExpressEmotion = "";
            if ($("#<%= Affect_ExpressEmotion_1.ClientID %>").is(":checked")) formData.Affect_ExpressEmotion = "Yes";
            if ($("#<%= Affect_ExpressEmotion_2.ClientID %>").is(":checked")) formData.Affect_ExpressEmotion = "No";

            // 3-7 Textboxes
            formData.Affect_Environment = $("#<%= Affect_Environment.ClientID %>").val();
            formData.Affect_Task = $("#<%= Affect_Task.ClientID %>").val();
            formData.Affect_Individual = $("#<%= Affect_Individual.ClientID %>").val();
            formData.Affect_ThroughOut = $("#<%= Affect_ThroughOut.ClientID %>").val();
            formData.Affect_Charaterising = $("#<%= Affect_Charaterising.ClientID %>").val();

            // Comments
            formData.Affect_cmt = $("#<%= Affect_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "AFFECT");
         }
         function SaveTab9(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 9;

            // 1) Attention span (textbox)
            formData.Attention_AttentionSpan = $("#<%= Attention_AttentionSpan.ClientID %>").val();

            // 2) Focus task at hand - Home (Yes/No)
            formData.Attention_FocusHandhome = "";
            if ($("#<%= Attention_FocusHandhome_1.ClientID %>").is(":checked")) formData.Attention_FocusHandhome = "Yes";
            if ($("#<%= Attention_FocusHandhome_2.ClientID %>").is(":checked")) formData.Attention_FocusHandhome = "No";

            // 3) Focus task at hand - School (Yes/No)
            formData.Attention_FocusHandSchool = "";
            if ($("#<%= Attention_FocusHandSchool_1.ClientID %>").is(":checked")) formData.Attention_FocusHandSchool = "Yes";
            if ($("#<%= Attention_FocusHandSchool_2.ClientID %>").is(":checked")) formData.Attention_FocusHandSchool = "No";

            // 4) Dividing attention (Yes/No)
            formData.Attention_Dividing = "";
            if ($("#<%= Attention_Dividing_1.ClientID %>").is(":checked")) formData.Attention_Dividing = "Yes";
            if ($("#<%= Attention_Dividing_2.ClientID %>").is(":checked")) formData.Attention_Dividing = "No";

            // 5) Change of activities every (textbox)
            formData.Attention_ChangeActivities = $("#<%= Attention_ChangeActivities.ClientID %>").val();

            // 6) Age appropriate attention (Yes/No/Sometimes)
            formData.Attention_AgeAppropriate = "";
            if ($("#<%= Attention_AgeAppropriate_1.ClientID %>").is(":checked")) formData.Attention_AgeAppropriate = "Yes";
            if ($("#<%= Attention_AgeAppropriate_2.ClientID %>").is(":checked")) formData.Attention_AgeAppropriate = "No";
            if ($("#<%= Attention_AgeAppropriate_3.ClientID %>").is(":checked")) formData.Attention_AgeAppropriate = "Sometimes";

            // 7) Factors of distractibility (textbox)
            formData.Attention_Distractibility = $("#<%= Attention_Distractibility.ClientID %>").val();

            // 8-12 Textboxes
            formData.Focal_Attention = $("#<%= Focal_Attention.ClientID %>").val();
            formData.Joint_Attention = $("#<%= Joint_Attention.ClientID %>").val();
            formData.Divided_Attention = $("#<%= Divided_Attention.ClientID %>").val();
            formData.Alternating_Attention = $("#<%= Alternating_Attention.ClientID %>").val();
            formData.Sustained_Attention = $("#<%= Sustained_Attention.ClientID %>").val();

            // 13) Move continuously (Yes/No)
            formData.Attention_move = "";
            if ($("#<%= Attention_move_1.ClientID %>").is(":checked")) formData.Attention_move = "Yes";
            if ($("#<%= Attention_move_2.ClientID %>").is(":checked")) formData.Attention_move = "No";

            // Comments
            formData.ATTENTION_cmt = $("#<%= ATTENTION_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "ATTENTION");
         }
         function SaveTab10(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 10;

            // 1) Motor planning (textbox)
            formData.Action_MotorPlanning = $("#<%= Action_MotorPlanning.ClientID %>").val();

            // 2) Purposeful (Yes/No)
            formData.Action_Purposeful = "";
            if ($("#<%= Action_Purposeful_1.ClientID %>").is(":checked")) formData.Action_Purposeful = "Yes";
            if ($("#<%= Action_Purposeful_2.ClientID %>").is(":checked")) formData.Action_Purposeful = "No";

            // 3) Goal oriented (Yes/No)
            formData.Action_GoalOriented = "";
            if ($("#<%= Action_GoalOriented_1.ClientID %>").is(":checked")) formData.Action_GoalOriented = "Yes";
            if ($("#<%= Action_GoalOriented_2.ClientID %>").is(":checked")) formData.Action_GoalOriented = "No";

            // 4) Feedback dependent (Yes/No)
            formData.Action_FeedBackDependent = "";
            if ($("#<%= Action_FeedBackDependent_1.ClientID %>").is(":checked")) formData.Action_FeedBackDependent = "Yes";
            if ($("#<%= Action_FeedBackDependent_2.ClientID %>").is(":checked")) formData.Action_FeedBackDependent = "No";

            // 5) Constructive (Yes/No)
            formData.Action_Constructive = "";
            if ($("#<%= Action_Constructive_1.ClientID %>").is(":checked")) formData.Action_Constructive = "Yes";
            if ($("#<%= Action_Constructive_2.ClientID %>").is(":checked")) formData.Action_Constructive = "No";

            // Comments
            formData.Action_cmt = $("#<%= Action_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "ACTION");
         }
         function SaveTab11(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 11;

            // 1) Social gathering multiple checkboxes (store Yes/No like your server variables)
            formData.Interacts = $("#<%= chkInteracts.ClientID %>").is(":checked") ? "Interacts" : "";
            formData.Does_not_initiate = $("#<%= chkDoes_not_initiate.ClientID %>").is(":checked") ? "Does not initiate" : "";
            formData.Sustain = $("#<%= chkSustain.ClientID %>").is(":checked") ? "Does not sustain" : "";

            formData.Fight = $("#<%= chkFight.ClientID %>").is(":checked") ? "Fight" : "";
            formData.Freeze = $("#<%= chkFreeze.ClientID %>").is(":checked") ? "Freeze" : "";
            formData.Fright = $("#<%= chkFright.ClientID %>").is(":checked") ? "Fright" : "";

            formData.Anxious = $("#<%= chkAnxious.ClientID %>").is(":checked") ? "Anxious" : "";
            formData.Comfortable = $("#<%= chkComfortable.ClientID %>").is(":checked") ? "Comfortable" : "";
            formData.Nervous = $("#<%= chkNervous.ClientID %>").is(":checked") ? "Nervous" : "";

            formData.ANS_response = $("#<%= chkANS_response.ClientID %>").is(":checked") ? "ANS_response" : "";
            formData.OTHERS = $("#<%= chkOTHERS.ClientID %>").is(":checked") ? "OTHERS" : "";

            // Gathering comment
            formData.cmtgathering = $("#<%= cmtgathering.ClientID %>").val();

            // 2) Understands social cues (Yes/No)
            formData.Interaction_SocialQues = "";
            if ($("#<%= Interaction_SocialQues_1.ClientID %>").is(":checked")) formData.Interaction_SocialQues = "Yes";
            if ($("#<%= Interaction_SocialQues_2.ClientID %>").is(":checked")) formData.Interaction_SocialQues = "No";

            // 3) Reactions textboxes
            formData.Interaction_Happiness = $("#<%= Interaction_Happiness.ClientID %>").val();
            formData.Interaction_Sadness = $("#<%= Interaction_Sadness.ClientID %>").val();
            formData.Interaction_Surprise = $("#<%= Interaction_Surprise.ClientID %>").val();
            formData.Interaction_Shock = $("#<%= Interaction_Shock.ClientID %>").val();

            // 4) Friendship (Yes/No)
            formData.Interaction_Friends = "";
            if ($("#<%= Interaction_Friends_1.ClientID %>").is(":checked")) formData.Interaction_Friends = "Yes";
            if ($("#<%= Interaction_Friends_2.ClientID %>").is(":checked")) formData.Interaction_Friends = "No";

            // 5) Enjoy activities + Comments
            formData.Interaction_Enjoy = $("#<%= Interaction_Enjoy.ClientID %>").val();
            formData.INTERACTION_cmt = $("#<%= INTERACTION_cmt.ClientID %>").val();

             saveWithModal(formData, reloadAfterSave, "INTERACTION");
         }
         function SaveTab12(reloadAfterSave) {

             var formData = {};
             formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 12;

            // --- Tactile Systems (TS)
            formData.TS_Registration = $("#<%= TS_Registration.ClientID %>").val();
            formData.TS_Orientation = $("#<%= TS_Orientation.ClientID %>").val();
            formData.TS_Discrimination = $("#<%= TS_Discrimination.ClientID %>").val();
            formData.TS_Responsiveness = $("#<%= TS_Responsiveness.ClientID %>").val();

            // --- Somatosensory system (SS)
            formData.SS_Bodyawareness = $("#<%= SS_Bodyawareness.ClientID %>").val();
            formData.SS_Bodyschema = $("#<%= SS_Bodyschema.ClientID %>").val();
            formData.SS_Orientation = $("#<%= SS_Orientation.ClientID %>").val();
            formData.SS_Posterior = $("#<%= SS_Posterior.ClientID %>").val();
            formData.SS_Bilateral = $("#<%= SS_Bilateral.ClientID %>").val();
            formData.SS_Balance = $("#<%= SS_Balance.ClientID %>").val();
            formData.SS_Dominance = $("#<%= SS_Dominance.ClientID %>").val();
            formData.SS_Right = $("#<%= SS_Right.ClientID %>").val();
            formData.SS_identifies = $("#<%= SS_identifies.ClientID %>").val();
            formData.SS_point = $("#<%= SS_point.ClientID %>").val();
            formData.SS_Constantly = $("#<%= SS_Constantly.ClientID %>").val();
            formData.SS_clumsy = $("#<%= SS_clumsy.ClientID %>").val();
            formData.SS_maneuver = $("#<%= SS_maneuver.ClientID %>").val();
            formData.SS_overly = $("#<%= SS_overly.ClientID %>").val();
            formData.SS_stand = $("#<%= SS_stand.ClientID %>").val();
            formData.SS_indulge = $("#<%= SS_indulge.ClientID %>").val();
            formData.SS_textures = $("#<%= SS_textures.ClientID %>").val();
            formData.SS_monkey = $("#<%= SS_monkey.ClientID %>").val();
            formData.SS_swings = $("#<%= SS_swings.ClientID %>").val();

            // --- Vestibular system (VM)
            formData.VM_Registration = $("#<%= VM_Registration.ClientID %>").val();
            formData.VM_Orientation = $("#<%= VM_Orientation.ClientID %>").val();
            formData.VM_Discrimination = $("#<%= VM_Discrimination.ClientID %>").val();
            formData.VM_Responsiveness = $("#<%= VM_Responsiveness.ClientID %>").val();

            // --- Proprioceptive system (PS)
            formData.PS_Registration = $("#<%= PS_Registration.ClientID %>").val();
            formData.PS_Gradation = $("#<%= PS_Gradation.ClientID %>").val();
            formData.PS_Discrimination = $("#<%= PS_Discrimination.ClientID %>").val();
            formData.PS_Responsiveness = $("#<%= PS_Responsiveness.ClientID %>").val();

            // --- ORO-Motor system (OM)
            formData.OM_Registration = $("#<%= OM_Registration.ClientID %>").val();
            formData.OM_Orientation = $("#<%= OM_Orientation.ClientID %>").val();
            formData.OM_Discrimination = $("#<%= OM_Discrimination.ClientID %>").val();
            formData.OM_Responsiveness = $("#<%= OM_Responsiveness.ClientID %>").val();

            // --- Auditory System (AS)
            formData.AS_Auditory = $("#<%= AS_Auditory.ClientID %>").val();
            formData.AS_Orientation = $("#<%= AS_Orientation.ClientID %>").val();
            formData.AS_Responsiveness = $("#<%= AS_Responsiveness.ClientID %>").val();
            formData.AS_discrimination = $("#<%= AS_discrimination.ClientID %>").val();
            formData.AS_Background = $("#<%= AS_Background.ClientID %>").val();
            formData.AS_localization = $("#<%= AS_localization.ClientID %>").val();
            formData.AS_Analysis = $("#<%= AS_Analysis.ClientID %>").val();
            formData.AS_sequencing = $("#<%= AS_sequencing.ClientID %>").val();
            formData.AS_blending = $("#<%= AS_blending.ClientID %>").val();

            // --- Visual system (VS)
            formData.VS_Visual = $("#<%= VS_Visual.ClientID %>").val();
            formData.VS_Responsiveness = $("#<%= VS_Responsiveness.ClientID %>").val();
            formData.VS_scanning = $("#<%= VS_scanning.ClientID %>").val();
            formData.VS_constancy = $("#<%= VS_constancy.ClientID %>").val();
            formData.VS_memory = $("#<%= VS_memory.ClientID %>").val();
            formData.VS_Perception = $("#<%= VS_Perception.ClientID %>").val();
            formData.VS_hand = $("#<%= VS_hand.ClientID %>").val();
            formData.VS_foot = $("#<%= VS_foot.ClientID %>").val();
            formData.VS_discrimination = $("#<%= VS_discrimination.ClientID %>").val();
            formData.VS_closure = $("#<%= VS_closure.ClientID %>").val();
            formData.VS_Figureground = $("#<%= VS_Figureground.ClientID %>").val();
            formData.VS_Visualmemory = $("#<%= VS_Visualmemory.ClientID %>").val();
            formData.VS_sequential = $("#<%= VS_sequential.ClientID %>").val();
            formData.VS_spatial = $("#<%= VS_spatial.ClientID %>").val();

            // --- Olfactory system (OS)
            formData.OS_Registration = $("#<%= OS_Registration.ClientID %>").val();
            formData.OS_Orientation = $("#<%= OS_Orientation.ClientID %>").val();
            formData.OS_Discrimination = $("#<%= OS_Discrimination.ClientID %>").val();
           formData.OS_Responsiveness = $("#<%= OS_Responsiveness.ClientID %>").val();

            saveWithModal(formData, reloadAfterSave, "SYSTEM EVALUATION");
        }
        function SaveTab13(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 13;

           formData.TestMeassures_GrossMotor = $("#<%= TestMeassures_GrossMotor.ClientID %>").val();
           formData.TestMeassures_FineMotor = $("#<%= TestMeassures_FineMotor.ClientID %>").val();
           formData.TestMeassures_DenverLanguage = $("#<%= TestMeassures_DenverLanguage.ClientID %>").val();
           formData.TestMeassures_DenverPersonal = $("#<%= TestMeassures_DenverPersonal.ClientID %>").val();

           formData.Tests_cmt = $("#<%= Tests_cmt.ClientID %>").val();

            saveWithModal(formData, reloadAfterSave, "DENVERS");
        }
        function SaveTab14(reloadAfterSave) {

            var formData = {};

            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 14;
           // Month dropdown
           formData.MONTHS = $("#<%= SelectMonth.ClientID %>").val();
           formData.questions = BuildQuestionsString();
           // Textboxes (same as your C#)
           formData.score_Communication_2 = $("#<%= score_Communication_2.ClientID %>").val();
           formData.Inter_Communication_2 = $("#<%= Inter_Communication_2.ClientID %>").val();
           formData.GROSS_2 = $("#<%= GROSS_2.ClientID %>").val();
           formData.inter_Gross_2 = $("#<%= inter_Gross_2.ClientID %>").val();
           formData.FINE_2 = $("#<%= FINE_2.ClientID %>").val();
           formData.inter_FINE_2 = $("#<%= inter_FINE_2.ClientID %>").val();
           formData.PROBLEM_2 = $("#<%= PROBLEM_2.ClientID %>").val();
           formData.inter_PROBLEM_2 = $("#<%= inter_PROBLEM_2.ClientID %>").val();
           formData.PERSONAL_2 = $("#<%= PERSONAL_2.ClientID %>").val();
           formData.inter_PERSONAL_2 = $("#<%= inter_PERSONAL_2.ClientID %>").val();

           formData.Comm_3 = $("#<%= Comm_3.ClientID %>").val();
           formData.inter_3 = $("#<%= inter_3.ClientID %>").val();
           formData.GROSS_3 = $("#<%= GROSS_3.ClientID %>").val();
           formData.GROSS_inter_3 = $("#<%= GROSS_inter_3.ClientID %>").val();
           formData.FINE_3 = $("#<%= FINE_3.ClientID %>").val();
           formData.FINE_inter_3 = $("#<%= FINE_inter_3.ClientID %>").val();
           formData.PROBLEM_3 = $("#<%= PROBLEM_3.ClientID %>").val();
           formData.PROBLEM_inter_3 = $("#<%= PROBLEM_inter_3.ClientID %>").val();
           formData.PERSONAL_3 = $("#<%= PERSONAL_3.ClientID %>").val();
           formData.PERSONAL_inter_3 = $("#<%= PERSONAL_inter_3.ClientID %>").val();

           formData.Communication_6 = $("#<%= Communication_6.ClientID %>").val();
           formData.comm_inter_6 = $("#<%= comm_inter_6.ClientID %>").val();
           formData.GROSS_6 = $("#<%= GROSS_6.ClientID %>").val();
           formData.GROSS_inter_6 = $("#<%= GROSS_inter_6.ClientID %>").val();
           formData.FINE_6 = $("#<%= FINE_6.ClientID %>").val();
           formData.FINE_inter_6 = $("#<%= FINE_inter_6.ClientID %>").val();
           formData.PROBLEM_6 = $("#<%= PROBLEM_6.ClientID %>").val();
           formData.PROBLEM_inter_6 = $("#<%= PROBLEM_inter_6.ClientID %>").val();
           formData.PERSONAL_6 = $("#<%= PERSONAL_6.ClientID %>").val();
           formData.PERSONAL_inter_6 = $("#<%= PERSONAL_inter_6.ClientID %>").val();

           formData.comm_7 = $("#<%= comm_7.ClientID %>").val();
           formData.inter_7 = $("#<%= inter_7.ClientID %>").val();
           formData.GROSS_7 = $("#<%= GROSS_7.ClientID %>").val();
           formData.GROSS_inter_7 = $("#<%= GROSS_inter_7.ClientID %>").val();
           formData.FINE_7 = $("#<%= FINE_7.ClientID %>").val();
           formData.FINE_inter_7 = $("#<%= FINE_inter_7.ClientID %>").val();
           formData.PROBLEM_7 = $("#<%= PROBLEM_7.ClientID %>").val();
           formData.PROBLEM_inter_7 = $("#<%= PROBLEM_inter_7.ClientID %>").val();
           formData.PERSONAL_7 = $("#<%= PERSONAL_7.ClientID %>").val();
           formData.PERSONAL_inter_7 = $("#<%= PERSONAL_inter_7.ClientID %>").val();

           formData.comm_9 = $("#<%= comm_9.ClientID %>").val();
           formData.inter_9 = $("#<%= inter_9.ClientID %>").val();
           formData.GROSS_9 = $("#<%= GROSS_9.ClientID %>").val();
           formData.GROSS_inter_9 = $("#<%= GROSS_inter_9.ClientID %>").val();
           formData.FINE_9 = $("#<%= FINE_9.ClientID %>").val();
           formData.FINE_inter_9 = $("#<%= FINE_inter_9.ClientID %>").val();
           formData.PROBLEM_9 = $("#<%= PROBLEM_9.ClientID %>").val();
           formData.PROBLEM_inter_9 = $("#<%= PROBLEM_inter_9.ClientID %>").val();
           formData.PERSONAL_9 = $("#<%= PERSONAL_9.ClientID %>").val();
           formData.PERSONAL_inter_9 = $("#<%= PERSONAL_inter_9.ClientID %>").val();

           formData.comm_10 = $("#<%= comm_10.ClientID %>").val();
           formData.inter_10 = $("#<%= inter_10.ClientID %>").val();
           formData.GROSS_10 = $("#<%= GROSS_10.ClientID %>").val();
           formData.GROSS_inter_10 = $("#<%= GROSS_inter_10.ClientID %>").val();
           formData.FINE_10 = $("#<%= FINE_10.ClientID %>").val();
           formData.FINE_inter_10 = $("#<%= FINE_inter_10.ClientID %>").val();
           formData.PROBLEM_10 = $("#<%= PROBLEM_10.ClientID %>").val();
           formData.PROBLEM_inter_10 = $("#<%= PROBLEM_inter_10.ClientID %>").val();
           formData.PERSONAL_10 = $("#<%= PERSONAL_10.ClientID %>").val();
           formData.PERSONAL_inter_10 = $("#<%= PERSONAL_inter_10.ClientID %>").val();

           formData.comm_11 = $("#<%= comm_11.ClientID %>").val();
           formData.inter_11 = $("#<%= inter_11.ClientID %>").val();
           formData.GROSS_11 = $("#<%= GROSS_11.ClientID %>").val();
           formData.GROSS_inter_11 = $("#<%= GROSS_inter_11.ClientID %>").val();
           formData.FINE_11 = $("#<%= FINE_11.ClientID %>").val();
           formData.FINE_inter_11 = $("#<%= FINE_inter_11.ClientID %>").val();
           formData.PROBLEM_11 = $("#<%= PROBLEM_11.ClientID %>").val();
           formData.PROBLEM_inter_11 = $("#<%= PROBLEM_inter_11.ClientID %>").val();
           formData.PERSONAL_11 = $("#<%= PERSONAL_11.ClientID %>").val();
           formData.PERSONAL_inter_11 = $("#<%= PERSONAL_inter_11.ClientID %>").val();

           formData.comm_13 = $("#<%= comm_13.ClientID %>").val();
           formData.inter_13 = $("#<%= inter_13.ClientID %>").val();
           formData.GROSS_13 = $("#<%= GROSS_13.ClientID %>").val();
           formData.GROSS_inter_13 = $("#<%= GROSS_inter_13.ClientID %>").val();
           formData.FINE_13 = $("#<%= FINE_13.ClientID %>").val();
           formData.FINE_inter_13 = $("#<%= FINE_inter_13.ClientID %>").val();
           formData.PROBLEM_13 = $("#<%= PROBLEM_13.ClientID %>").val();
           formData.PROBLEM_inter_13 = $("#<%= PROBLEM_inter_13.ClientID %>").val();
           formData.PERSONAL_13 = $("#<%= PERSONAL_13.ClientID %>").val();
           formData.PERSONAL_inter_13 = $("#<%= PERSONAL_inter_13.ClientID %>").val();

           formData.comm_15 = $("#<%= comm_15.ClientID %>").val();
           formData.inter_15 = $("#<%= inter_15.ClientID %>").val();
           formData.GROSS_15 = $("#<%= GROSS_15.ClientID %>").val();
           formData.GROSS_inter_15 = $("#<%= GROSS_inter_15.ClientID %>").val();
           formData.FINE_15 = $("#<%= FINE_15.ClientID %>").val();
           formData.FINE_inter_15 = $("#<%= FINE_inter_15.ClientID %>").val();
           formData.PROBLEM_15 = $("#<%= PROBLEM_15.ClientID %>").val();
           formData.PROBLEM_inter_15 = $("#<%= PROBLEM_inter_15.ClientID %>").val();
           formData.PERSONAL_15 = $("#<%= PERSONAL_15.ClientID %>").val();
           formData.PERSONAL_inter_15 = $("#<%= PERSONAL_inter_15.ClientID %>").val();

           formData.comm_17 = $("#<%= comm_17.ClientID %>").val();
           formData.inter_17 = $("#<%= inter_17.ClientID %>").val();
           formData.GROSS_17 = $("#<%= GROSS_17.ClientID %>").val();
           formData.GROSS_inter_17 = $("#<%= GROSS_inter_17.ClientID %>").val();
           formData.FINE_17 = $("#<%= FINE_17.ClientID %>").val();
           formData.FINE_inter_17 = $("#<%= FINE_inter_17.ClientID %>").val();
           formData.PROBLEM_17 = $("#<%= PROBLEM_17.ClientID %>").val();
           formData.PROBLEM_inter_17 = $("#<%= PROBLEM_inter_17.ClientID %>").val();
           formData.PERSONAL_17 = $("#<%= PERSONAL_17.ClientID %>").val();
           formData.PERSONAL_inter_17 = $("#<%= PERSONAL_inter_17.ClientID %>").val();

           formData.comm_19 = $("#<%= comm_19.ClientID %>").val();
           formData.inter_19 = $("#<%= inter_19.ClientID %>").val();
           formData.GROSS_19 = $("#<%= GROSS_19.ClientID %>").val();
           formData.GROSS_inter_19 = $("#<%= GROSS_inter_19.ClientID %>").val();
           formData.FINE_19 = $("#<%= FINE_19.ClientID %>").val();
           formData.FINE_inter_19 = $("#<%= FINE_inter_19.ClientID %>").val();
           formData.PROBLEM_19 = $("#<%= PROBLEM_19.ClientID %>").val();
           formData.PROBLEM_inter_19 = $("#<%= PROBLEM_inter_19.ClientID %>").val();
           formData.PERSONAL_19 = $("#<%= PERSONAL_19.ClientID %>").val();
           formData.PERSONAL_inter_19 = $("#<%= PERSONAL_inter_19.ClientID %>").val();

           formData.comm_21 = $("#<%= comm_21.ClientID %>").val();
           formData.inter_21 = $("#<%= inter_21.ClientID %>").val();
           formData.GROSS_21 = $("#<%= GROSS_21.ClientID %>").val();
           formData.GROSS_inter_21 = $("#<%= GROSS_inter_21.ClientID %>").val();
           formData.FINE_21 = $("#<%= FINE_21.ClientID %>").val();
           formData.FINE_inter_21 = $("#<%= FINE_inter_21.ClientID %>").val();
           formData.PROBLEM_21 = $("#<%= PROBLEM_21.ClientID %>").val();
           formData.PROBLEM_inter_21 = $("#<%= PROBLEM_inter_21.ClientID %>").val();
           formData.PERSONAL_21 = $("#<%= PERSONAL_21.ClientID %>").val();
           formData.PERSONAL_inter_21 = $("#<%= PERSONAL_inter_21.ClientID %>").val();

           formData.comm_23 = $("#<%= comm_23.ClientID %>").val();
           formData.inter_23 = $("#<%= inter_23.ClientID %>").val();
           formData.GROSS_23 = $("#<%= GROSS_23.ClientID %>").val();
           formData.GROSS_inter_23 = $("#<%= GROSS_inter_23.ClientID %>").val();
           formData.FINE_23 = $("#<%= FINE_23.ClientID %>").val();
           formData.FINE_inter_23 = $("#<%= FINE_inter_23.ClientID %>").val();
           formData.PROBLEM_23 = $("#<%= PROBLEM_23.ClientID %>").val();
           formData.PROBLEM_inter_23 = $("#<%= PROBLEM_inter_23.ClientID %>").val();
           formData.PERSONAL_23 = $("#<%= PERSONAL_23.ClientID %>").val();
           formData.PERSONAL_inter_23 = $("#<%= PERSONAL_inter_23.ClientID %>").val();

           formData.comm_25 = $("#<%= comm_25.ClientID %>").val();
           formData.inter_25 = $("#<%= inter_25.ClientID %>").val();
           formData.GROSS_25 = $("#<%= GROSS_25.ClientID %>").val();
           formData.GROSS_inter_25 = $("#<%= GROSS_inter_25.ClientID %>").val();
           formData.FINE_25 = $("#<%= FINE_25.ClientID %>").val();
           formData.FINE_inter_25 = $("#<%= FINE_inter_25.ClientID %>").val();
           formData.PROBLEM_25 = $("#<%= PROBLEM_25.ClientID %>").val();
           formData.PROBLEM_inter_25 = $("#<%= PROBLEM_inter_25.ClientID %>").val();
            formData.PERSONAL_25 = $("#<%= PERSONAL_25.ClientID %>").val();
            formData.PERSONAL_inter_25 = $("#<%= inter_25.ClientID %>").val();

           formData.comm_28 = $("#<%= comm_28.ClientID %>").val();
           formData.inter_28 = $("#<%= inter_28.ClientID %>").val();
           formData.GROSS_28 = $("#<%= GROSS_28.ClientID %>").val();
           formData.GROSS_inter_28 = $("#<%= GROSS_inter_28.ClientID %>").val();
           formData.FINE_28 = $("#<%= FINE_28.ClientID %>").val();
           formData.FINE_inter_28 = $("#<%= FINE_inter_28.ClientID %>").val();
           formData.PROBLEM_28 = $("#<%= PROBLEM_28.ClientID %>").val();
           formData.PROBLEM_inter_28 = $("#<%= PROBLEM_inter_28.ClientID %>").val();
           formData.PERSONAL_28 = $("#<%= PERSONAL_28.ClientID %>").val();
           formData.PERSONAL_inter_28 = $("#<%= PERSONAL_inter_28.ClientID %>").val();

           formData.comm_31 = $("#<%= comm_31.ClientID %>").val();
           formData.inter_31 = $("#<%= inter_31.ClientID %>").val();
           formData.GROSS_31 = $("#<%= GROSS_31.ClientID %>").val();
           formData.GROSS_inter_31 = $("#<%= GROSS_inter_31.ClientID %>").val();
           formData.FINE_31 = $("#<%= FINE_31.ClientID %>").val();
           formData.FINE_inter_31 = $("#<%= FINE_inter_31.ClientID %>").val();
           formData.PROBLEM_31 = $("#<%= PROBLEM_31.ClientID %>").val();
           formData.PROBLEM_inter_31 = $("#<%= PROBLEM_inter_31.ClientID %>").val();
           formData.PERSONAL_31 = $("#<%= PERSONAL_31.ClientID %>").val();
           formData.PERSONAL_inter_31 = $("#<%= PERSONAL_inter_31.ClientID %>").val();

           formData.comm_34 = $("#<%= comm_34.ClientID %>").val();
           formData.inter_34 = $("#<%= inter_34.ClientID %>").val();
           formData.GROSS_34 = $("#<%= GROSS_34.ClientID %>").val();
           formData.GROSS_inter_34 = $("#<%= GROSS_inter_34.ClientID %>").val();
           formData.FINE_34 = $("#<%= FINE_34.ClientID %>").val();
           formData.FINE_inter_34 = $("#<%= FINE_inter_34.ClientID %>").val();
           formData.PROBLEM_34 = $("#<%= PROBLEM_34.ClientID %>").val();
           formData.PROBLEM_inter_34 = $("#<%= PROBLEM_inter_34.ClientID %>").val();
           formData.PERSONAL_34 = $("#<%= PERSONAL_34.ClientID %>").val();
           formData.PERSONAL_inter_34 = $("#<%= PERSONAL_inter_34.ClientID %>").val();

           formData.comm_42 = $("#<%= comm_42.ClientID %>").val();
           formData.inter_42 = $("#<%= inter_42.ClientID %>").val();
           formData.GROSS_42 = $("#<%= GROSS_42.ClientID %>").val();
           formData.GROSS_inter_42 = $("#<%= GROSS_inter_42.ClientID %>").val();
           formData.FINE_42 = $("#<%= FINE_42.ClientID %>").val();
           formData.FINE_inter_42 = $("#<%= FINE_inter_42.ClientID %>").val();
           formData.PROBLEM_42 = $("#<%= PROBLEM_42.ClientID %>").val();
           formData.PROBLEM_inter_42 = $("#<%= PROBLEM_inter_42.ClientID %>").val();
           formData.PERSONAL_42 = $("#<%= PERSONAL_42.ClientID %>").val();
           formData.PERSONAL_inter_42 = $("#<%= PERSONAL_inter_42.ClientID %>").val();

           formData.comm_45 = $("#<%= comm_45.ClientID %>").val();
           formData.inter_45 = $("#<%= inter_45.ClientID %>").val();
           formData.GROSS_45 = $("#<%= GROSS_45.ClientID %>").val();
           formData.GROSS_inter_45 = $("#<%= GROSS_inter_45.ClientID %>").val();
           formData.FINE_45 = $("#<%= FINE_45.ClientID %>").val();
           formData.FINE_inter_45 = $("#<%= FINE_inter_45.ClientID %>").val();
           formData.PROBLEM_45 = $("#<%= PROBLEM_45.ClientID %>").val();
           formData.PROBLEM_inter_45 = $("#<%= PROBLEM_inter_45.ClientID %>").val();
           formData.PERSONAL_45 = $("#<%= PERSONAL_45.ClientID %>").val();
           formData.PERSONAL_inter_45 = $("#<%= PERSONAL_inter_45.ClientID %>").val();

           formData.comm_51 = $("#<%= comm_51.ClientID %>").val();
           formData.inter_51 = $("#<%= inter_51.ClientID %>").val();
           formData.GROSS_51 = $("#<%= GROSS_51.ClientID %>").val();
           formData.GROSS_inter_51 = $("#<%= GROSS_inter_51.ClientID %>").val();
           formData.FINE_51 = $("#<%= FINE_51.ClientID %>").val();
           formData.FINE_inter_51 = $("#<%= FINE_inter_51.ClientID %>").val();
           formData.PROBLEM_51 = $("#<%= PROBLEM_51.ClientID %>").val();
           formData.PROBLEM_inter_51 = $("#<%= PROBLEM_inter_51.ClientID %>").val();
           formData.PERSONAL_51 = $("#<%= PERSONAL_51.ClientID %>").val();
           formData.PERSONAL_inter_51 = $("#<%= PERSONAL_inter_51.ClientID %>").val();

           formData.comm_60 = $("#<%= comm_60.ClientID %>").val();
           formData.inter_60 = $("#<%= inter_60.ClientID %>").val();
           formData.GROSS_60 = $("#<%= GROSS_60.ClientID %>").val();
           formData.GROSS_inter_60 = $("#<%= GROSS_inter_60.ClientID %>").val();
           formData.FINE_60 = $("#<%= FINE_60.ClientID %>").val();
           formData.FINE_inter_60 = $("#<%= FINE_inter_60.ClientID %>").val();
           formData.PROBLEM_60 = $("#<%= PROBLEM_60.ClientID %>").val();
           formData.PROBLEM_inter_60 = $("#<%= PROBLEM_inter_60.ClientID %>").val();
           formData.PERSONAL_60 = $("#<%= PERSONAL_60.ClientID %>").val();
           formData.PERSONAL_inter_60 = $("#<%= PERSONAL_inter_60.ClientID %>").val();
            saveWithModal(formData, reloadAfterSave, "AGES AND STAGES");

        }
        function SaveTab15(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 15;

           // 1) Sensory Profile-2 0-6 Months
           formData.General_Processing = $("[id$='General_Processing']").val();
           formData.AUDITORY_Processing = $("[id$='AUDITORY_Processing']").val();
           formData.VISUAL_Processing = $("[id$='VISUAL_Processing']").val();
           formData.TOUCH_Processing = $("[id$='TOUCH_Processing']").val();
           formData.MOVEMENT_Processing = $("[id$='MOVEMENT_Processing']").val();
           formData.ORAL_Processing = $("[id$='ORAL_Processing']").val();
           formData.Raw_score = $("[id$='Raw_score']").val();

           formData.Total_rawscore = $("[id$='Total_rawscore']").val();
           formData.Interpretation = $("[id$='Interpretation']").val();
           formData.Comments_1 = $("[id$='Comments_1']").val();

           // 2) TODDLER
           // 2) TODDLER
           formData.Score_seeking = $("#<%= Score_seeking.ClientID %>").val();
           formData.SEEKING = $("#<%= SEEKING.ClientID %>").val();

           formData.Score_Avoiding = $("#<%= Score_Avoiding.ClientID %>").val();
           formData.AVOIDING = $("#<%= AVOIDING.ClientID %>").val();

           formData.Score_sensitivity = $("#<%= Score_sensitivity.ClientID %>").val();
           formData.SENSITIVITY_2 = $("#<%= SENSITIVITY_2.ClientID %>").val();

           formData.Score_Registration = $("#<%= Score_Registration.ClientID %>").val();
           formData.REGISTRATION = $("#<%= REGISTRATION.ClientID %>").val();

           formData.Score_general = $("#<%= Score_general.ClientID %>").val();
           formData.GENERAL = $("#<%= GENERAL.ClientID %>").val();

           formData.Score_Auditory = $("#<%= Score_Auditory.ClientID %>").val();
           formData.AUDITORY = $("#<%= AUDITORY.ClientID %>").val();

           formData.Score_visual = $("#<%= Score_visual.ClientID %>").val();
           formData.VISUAL = $("#<%= VISUAL.ClientID %>").val();

           formData.Score_touch = $("#<%= Score_touch.ClientID %>").val();
           formData.TOUCH = $("#<%= TOUCH.ClientID %>").val();

           formData.Score_movement = $("#<%= Score_movement.ClientID %>").val();
           formData.MOVEMENT = $("#<%= MOVEMENT.ClientID %>").val();

           formData.Score_oral = $("#<%= Score_oral.ClientID %>").val();
           formData.ORAL = $("#<%= ORAL.ClientID %>").val();

           formData.Score_behavioural = $("#<%= Score_behavioural.ClientID %>").val();
           formData.BEHAVIORAL = $("#<%= BEHAVIORAL.ClientID %>").val();

           formData.Comments_2 = $("#<%= Comments_2.ClientID %>").val();

           // 3) CHILD
           formData.SPchild_Seeker = $("#<%= SPchild_Seeker.ClientID %>").val();
           formData.Seeking_Seeker = $("#<%= Seeking_Seeker.ClientID %>").val();

           formData.SPchild_Avoider = $("#<%= SPchild_Avoider.ClientID %>").val();
           formData.Avoiding_Avoider = $("#<%= Avoiding_Avoider.ClientID %>").val();

           formData.SPchild_Sensor = $("#<%= SPchild_Sensor.ClientID %>").val();
           formData.Sensitivity_Sensor = $("#<%= Sensitivity_Sensor.ClientID %>").val();

           formData.SPchild_Bystander = $("#<%= SPchild_Bystander.ClientID %>").val();
           formData.Registration_Bystander = $("#<%= Registration_Bystander.ClientID %>").val();

           formData.SPchild_Auditory_3 = $("#<%= SPchild_Auditory_3.ClientID %>").val();
           formData.Auditory_3 = $("#<%= Auditory_3.ClientID %>").val();

           formData.SPchild_Visual_3 = $("#<%= SPchild_Visual_3.ClientID %>").val();
           formData.Visual_3 = $("#<%= Visual_3.ClientID %>").val();

           formData.SPchild_Touch_3 = $("#<%= SPchild_Touch_3.ClientID %>").val();
           formData.Touch_3 = $("#<%= Touch_3.ClientID %>").val();

           formData.SPchild_Movement_3 = $("#<%= SPchild_Movement_3.ClientID %>").val();
           formData.Movement_3 = $("#<%= Movement_3.ClientID %>").val();

           formData.SPchild_Body_position = $("#<%= SPchild_Body_position.ClientID %>").val();
           formData.Body_position = $("#<%= Body_position.ClientID %>").val();

           formData.SPchild_Oral_3 = $("#<%= SPchild_Oral_3.ClientID %>").val();
           formData.Oral_3 = $("#<%= Oral_3.ClientID %>").val();

           formData.SPchild_Conduct_3 = $("#<%= SPchild_Conduct_3.ClientID %>").val();
           formData.Conduct_3 = $("#<%= Conduct_3.ClientID %>").val();

           formData.SPchild_Social_emotional = $("#<%= SPchild_Social_emotional.ClientID %>").val();
           formData.Social_emotional = $("#<%= Social_emotional.ClientID %>").val();

           formData.SPchild_Attentional_3 = $("#<%= SPchild_Attentional_3.ClientID %>").val();
           formData.Attentional_3 = $("#<%= Attentional_3.ClientID %>").val();

           formData.Comments_3 = $("#<%= Comments_3.ClientID %>").val();

           // 4) Adolescent and Adult (11-17)
           formData.SPAdult_Low_Registration = $("#<%= SPAdult_Low_Registration.ClientID %>").val();
           formData.Low_Registration = $("#<%= Low_Registration.ClientID %>").val();

           formData.SPAdult_Sensory_seeking = $("#<%= SPAdult_Sensory_seeking.ClientID %>").val();
           formData.Sensory_seeking = $("#<%= Sensory_seeking.ClientID %>").val();

           formData.SPAdult_Sensory_Sensitivity = $("#<%= SPAdult_Sensory_Sensitivity.ClientID %>").val();
           formData.Sensory_Sensitivity = $("#<%= Sensory_Sensitivity.ClientID %>").val();

           formData.SPAdult_Sensory_Avoiding = $("#<%= SPAdult_Sensory_Avoiding.ClientID %>").val();
           formData.Sensory_Avoiding = $("#<%= Sensory_Avoiding.ClientID %>").val();

           formData.Comments_4 = $("#<%= Comments_4.ClientID %>").val();

           // 5) 16-64
           formData.SP_Low_Registration64 = $("#<%= SP_Low_Registration64.ClientID %>").val();
           formData.Low_Registration_5 = $("#<%= Low_Registration_5.ClientID %>").val();

           formData.SP_Sensory_seeking_64 = $("#<%= SP_Sensory_seeking_64.ClientID %>").val();
           formData.Sensory_seeking_5 = $("#<%= Sensory_seeking_5.ClientID %>").val();

           formData.SP_Sensory_Sensitivity64 = $("#<%= SP_Sensory_Sensitivity64.ClientID %>").val();
           formData.Sensory_Sensitivity_5 = $("#<%= Sensory_Sensitivity_5.ClientID %>").val();

           formData.SP_Sensory_Avoiding64 = $("#<%= SP_Sensory_Avoiding64.ClientID %>").val();
           formData.Sensory_Avoiding_5 = $("#<%= Sensory_Avoiding_5.ClientID %>").val();

           formData.Comments_5 = $("#<%= Comments_5.ClientID %>").val();

           // 6) 65+
           formData.Older_Low_Registration = $("#<%= Older_Low_Registration.ClientID %>").val();
           formData.Low_Registration_6 = $("#<%= Low_Registration_6.ClientID %>").val();

           formData.Older_Sensory_seeking = $("#<%= Older_Sensory_seeking.ClientID %>").val();
           formData.Sensory_seeking_6 = $("#<%= Sensory_seeking_6.ClientID %>").val();

           formData.Older_Sensory_Sensitivity = $("#<%= Older_Sensory_Sensitivity.ClientID %>").val();
           formData.Sensory_Sensitivity_6 = $("#<%= Sensory_Sensitivity_6.ClientID %>").val();

           formData.Older_Sensory_Avoiding = $("#<%= Older_Sensory_Avoiding.ClientID %>").val();
           formData.Sensory_Avoiding_6 = $("#<%= Sensory_Avoiding_6.ClientID %>").val();

           formData.Comments_6 = $("#<%= Comments_6.ClientID %>").val();

            saveWithModal(formData, reloadAfterSave, "SENSORY PROFILE- 2");
        }
        function SaveTab16(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 16;

           // WebForms safe selectors
           formData.ABILITY_months = $("#<%= MonthSelect.ClientID %>").val();
           formData.ability_TOTAL = $("#<%= ability_TOTAL.ClientID %>").val();
           formData.ability_COMMENTS = $("#<%= ability_COMMENTS.ClientID %>").val();

           var ABILITY_questions = "";

           // loop ALL rows inside the ability table
           $("#<%= updAbility.ClientID %> table tr").each(function () {

                var questionNO = $(this).find("span[id*='abilityQuestionNo']").text().trim();
                if (questionNO === "") return; // skip header/category rows

                var categoryId = $(this).find("span[id*='lblCategoryId']").text().trim();

                var Yes = $(this).find("input[id*='chkMonthYes']").is(":checked") ? "1" : "0";
                var No = $(this).find("input[id*='chkMonthNo']").is(":checked") ? "1" : "0";

                ABILITY_questions += categoryId + "#" + questionNO + "$" + Yes + "$" + No + "~";
            });

            // remove last "~"
            if (ABILITY_questions.length > 0 && ABILITY_questions.endsWith("~")) {
                ABILITY_questions = ABILITY_questions.slice(0, -1);
            }


            formData.ABILITY_questions = ABILITY_questions;

            // Debug (optional)
            // console.log(formData);
            // console.log("ABILITY_questions:", ABILITY_questions);

            saveWithModal(formData, reloadAfterSave, "ABILITY CHECKLIST");
        }

        function SaveTab17(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 17;

            // Throws
            formData.DCDQ_Throws1 = $("[id$='DCDQ_Throws1']").val();
            formData.DCDQ_Throws2 = $("[id$='DCDQ_Throws2']").val();
            formData.DCDQ_Throws3 = $("[id$='DCDQ_Throws3']").val();

            // Catches
            formData.DCDQ_Catches1 = $("[id$='DCDQ_Catches1']").val();
            formData.DCDQ_Catches2 = $("[id$='DCDQ_Catches2']").val();
            formData.DCDQ_Catches3 = $("[id$='DCDQ_Catches3']").val();

            // Hits
            formData.DCDQ_Hits1 = $("[id$='DCDQ_Hits1']").val();
            formData.DCDQ_Hits2 = $("[id$='DCDQ_Hits2']").val();
            formData.DCDQ_Hits3 = $("[id$='DCDQ_Hits3']").val();

            // Jumps
            formData.DCDQ_Jumps1 = $("[id$='DCDQ_Jumps1']").val();
            formData.DCDQ_Jumps2 = $("[id$='DCDQ_Jumps2']").val();
            formData.DCDQ_Jumps3 = $("[id$='DCDQ_Jumps3']").val();

            // Runs
            formData.DCDQ_Runs1 = $("[id$='DCDQ_Runs1']").val();
            formData.DCDQ_Runs2 = $("[id$='DCDQ_Runs2']").val();
            formData.DCDQ_Runs3 = $("[id$='DCDQ_Runs3']").val();

            // Plans
            formData.DCDQ_Plans1 = $("[id$='DCDQ_Plans1']").val();
            formData.DCDQ_Plans2 = $("[id$='DCDQ_Plans2']").val();
            formData.DCDQ_Plans3 = $("[id$='DCDQ_Plans3']").val();

            // Writing
            formData.DCDQ_Writing1 = $("[id$='DCDQ_Writing1']").val();
            formData.DCDQ_Writing2 = $("[id$='DCDQ_Writing2']").val();
            formData.DCDQ_Writing3 = $("[id$='DCDQ_Writing3']").val();

            // Legibly
            formData.DCDQ_legibly1 = $("[id$='DCDQ_legibly1']").val();
            formData.DCDQ_legibly2 = $("[id$='DCDQ_legibly2']").val();
            formData.DCDQ_legibly3 = $("[id$='DCDQ_legibly3']").val();

            // Effort
            formData.DCDQ_Effort1 = $("[id$='DCDQ_Effort1']").val();
            formData.DCDQ_Effort2 = $("[id$='DCDQ_Effort2']").val();
            formData.DCDQ_Effort3 = $("[id$='DCDQ_Effort3']").val();

            // Cuts
            formData.DCDQ_Cuts1 = $("[id$='DCDQ_Cuts1']").val();
            formData.DCDQ_Cuts2 = $("[id$='DCDQ_Cuts2']").val();
            formData.DCDQ_Cuts3 = $("[id$='DCDQ_Cuts3']").val();

            // Likes
            formData.DCDQ_Likes1 = $("[id$='DCDQ_Likes1']").val();
            formData.DCDQ_Likes2 = $("[id$='DCDQ_Likes2']").val();
            formData.DCDQ_Likes3 = $("[id$='DCDQ_Likes3']").val();

            // Learning
            formData.DCDQ_Learning1 = $("[id$='DCDQ_Learning1']").val();
            formData.DCDQ_Learning2 = $("[id$='DCDQ_Learning2']").val();
            formData.DCDQ_Learning3 = $("[id$='DCDQ_Learning3']").val();

            // Quick
            formData.DCDQ_Quick1 = $("[id$='DCDQ_Quick1']").val();
            formData.DCDQ_Quick2 = $("[id$='DCDQ_Quick2']").val();
            formData.DCDQ_Quick3 = $("[id$='DCDQ_Quick3']").val();

            // Bull
            formData.DCDQ_Bull1 = $("[id$='DCDQ_Bull1']").val();
            formData.DCDQ_Bull2 = $("[id$='DCDQ_Bull2']").val();
            formData.DCDQ_Bull3 = $("[id$='DCDQ_Bull3']").val();

            // Does
            formData.DCDQ_Does1 = $("[id$='DCDQ_Does1']").val();
            formData.DCDQ_Does2 = $("[id$='DCDQ_Does2']").val();
            formData.DCDQ_Does3 = $("[id$='DCDQ_Does3']").val();

            // Totals
            formData.DCDQ_Control = $("[id$='DCDQ_Control']").val();
            formData.DCDQ_Fine = $("[id$='DCDQ_Fine']").val();
            formData.DCDQ_General = $("[id$='DCDQ_General']").val();
            formData.DCDQ_Total = $("[id$='DCDQ_Total']").val();

            formData.DCDQ_INTERPRETATION = $("[id$='DCDQ_INTERPRETATION']").val();
            formData.DCDQ_COMMENT = $("[id$='DCDQ_COMMENT']").val();

            saveWithModal(formData, reloadAfterSave, "DCDQ");
        }
        function SaveTab18(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 18;

            formData.SIPTInfo_History = $("[id$='SIPTInfo_History']").val();

            // Hand Function 1
            formData.SIPTInfo_HandFunction1_GraspRight = $("[id$='SIPTInfo_HandFunction1_GraspRight']").val();
            formData.SIPTInfo_HandFunction1_GraspLeft = $("[id$='SIPTInfo_HandFunction1_GraspLeft']").val();
            formData.SIPTInfo_HandFunction1_SphericalRight = $("[id$='SIPTInfo_HandFunction1_SphericalRight']").val();
            formData.SIPTInfo_HandFunction1_SphericalLeft = $("[id$='SIPTInfo_HandFunction1_SphericalLeft']").val();
            formData.SIPTInfo_HandFunction1_HookRight = $("[id$='SIPTInfo_HandFunction1_HookRight']").val();
            formData.SIPTInfo_HandFunction1_HookLeft = $("[id$='SIPTInfo_HandFunction1_HookLeft']").val();
            formData.SIPTInfo_HandFunction1_JawChuckRight = $("[id$='SIPTInfo_HandFunction1_JawChuckRight']").val();
            formData.SIPTInfo_HandFunction1_JawChuckLeft = $("[id$='SIPTInfo_HandFunction1_JawChuckLeft']").val();
            formData.SIPTInfo_HandFunction1_GripRight = $("[id$='SIPTInfo_HandFunction1_GripRight']").val();
            formData.SIPTInfo_HandFunction1_GripLeft = $("[id$='SIPTInfo_HandFunction1_GripLeft']").val();
            formData.SIPTInfo_HandFunction1_ReleaseRight = $("[id$='SIPTInfo_HandFunction1_ReleaseRight']").val();
            formData.SIPTInfo_HandFunction1_ReleaseLeft = $("[id$='SIPTInfo_HandFunction1_ReleaseLeft']").val();

            // Hand Function 2
            formData.SIPTInfo_HandFunction2_OppositionLfR = $("[id$='SIPTInfo_HandFunction2_OppositionLfR']").val();
            formData.SIPTInfo_HandFunction2_OppositionLfL = $("[id$='SIPTInfo_HandFunction2_OppositionLfL']").val();
            formData.SIPTInfo_HandFunction2_OppositionMFR = $("[id$='SIPTInfo_HandFunction2_OppositionMFR']").val();
            formData.SIPTInfo_HandFunction2_OppositionMFL = $("[id$='SIPTInfo_HandFunction2_OppositionMFL']").val();
            formData.SIPTInfo_HandFunction2_OppositionRFR = $("[id$='SIPTInfo_HandFunction2_OppositionRFR']").val();
            formData.SIPTInfo_HandFunction2_OppositionRFL = $("[id$='SIPTInfo_HandFunction2_OppositionRFL']").val();
            formData.SIPTInfo_HandFunction2_PinchLfR = $("[id$='SIPTInfo_HandFunction2_PinchLfR']").val();
            formData.SIPTInfo_HandFunction2_PinchLfL = $("[id$='SIPTInfo_HandFunction2_PinchLfL']").val();
            formData.SIPTInfo_HandFunction2_PinchMFR = $("[id$='SIPTInfo_HandFunction2_PinchMFR']").val();
            formData.SIPTInfo_HandFunction2_PinchMFL = $("[id$='SIPTInfo_HandFunction2_PinchMFL']").val();
            formData.SIPTInfo_HandFunction2_PinchRFR = $("[id$='SIPTInfo_HandFunction2_PinchRFR']").val();
            formData.SIPTInfo_HandFunction2_PinchRFL = $("[id$='SIPTInfo_HandFunction2_PinchRFL']").val();

            // SIPT 3
            formData.SIPTInfo_SIPT3_Spontaneous = $("[id$='SIPTInfo_SIPT3_Spontaneous']").val();
            formData.SIPTInfo_SIPT3_Command = $("[id$='SIPTInfo_SIPT3_Command']").val();

            // SIPT 4
            formData.SIPTInfo_SIPT4_Kinesthesia = $("[id$='SIPTInfo_SIPT4_Kinesthesia']").val();
            formData.SIPTInfo_SIPT4_Finger = $("[id$='SIPTInfo_SIPT4_Finger']").val();
            formData.SIPTInfo_SIPT4_Localisation = $("[id$='SIPTInfo_SIPT4_Localisation']").val();
            formData.SIPTInfo_SIPT4_DoubleTactile = $("[id$='SIPTInfo_SIPT4_DoubleTactile']").val();
            formData.SIPTInfo_SIPT4_Tactile = $("[id$='SIPTInfo_SIPT4_Tactile']").val();
            formData.SIPTInfo_SIPT4_Graphesthesia = $("[id$='SIPTInfo_SIPT4_Graphesthesia']").val();
            formData.SIPTInfo_SIPT4_PostRotary = $("[id$='SIPTInfo_SIPT4_PostRotary']").val();
            formData.SIPTInfo_SIPT4_Standing = $("[id$='SIPTInfo_SIPT4_Standing']").val();

            // SIPT 5
            formData.SIPTInfo_SIPT5_Color = $("[id$='SIPTInfo_SIPT5_Color']").val();
            formData.SIPTInfo_SIPT5_Form = $("[id$='SIPTInfo_SIPT5_Form']").val();
            formData.SIPTInfo_SIPT5_Size = $("[id$='SIPTInfo_SIPT5_Size']").val();
            formData.SIPTInfo_SIPT5_Depth = $("[id$='SIPTInfo_SIPT5_Depth']").val();
            formData.SIPTInfo_SIPT5_Figure = $("[id$='SIPTInfo_SIPT5_Figure']").val();
            formData.SIPTInfo_SIPT5_Motor = $("[id$='SIPTInfo_SIPT5_Motor']").val();

            // SIPT 6
            formData.SIPTInfo_SIPT6_Design = $("[id$='SIPTInfo_SIPT6_Design']").val();
            formData.SIPTInfo_SIPT6_Constructional = $("[id$='SIPTInfo_SIPT6_Constructional']").val();

            // SIPT 7
            formData.SIPTInfo_SIPT7_Scanning = $("[id$='SIPTInfo_SIPT7_Scanning']").val();
            formData.SIPTInfo_SIPT7_Memory = $("[id$='SIPTInfo_SIPT7_Memory']").val();

            // SIPT 8
            formData.SIPTInfo_SIPT8_Postural = $("[id$='SIPTInfo_SIPT8_Postural']").val();
            formData.SIPTInfo_SIPT8_Oral = $("[id$='SIPTInfo_SIPT8_Oral']").val();
            formData.SIPTInfo_SIPT8_Sequencing = $("[id$='SIPTInfo_SIPT8_Sequencing']").val();
            formData.SIPTInfo_SIPT8_Commands = $("[id$='SIPTInfo_SIPT8_Commands']").val();

            // SIPT 9
            formData.SIPTInfo_SIPT9_Bilateral = $("[id$='SIPTInfo_SIPT9_Bilateral']").val();
            formData.SIPTInfo_SIPT9_Contralat = $("[id$='SIPTInfo_SIPT9_Contralat']").val();
            formData.SIPTInfo_SIPT9_PreferredHand = $("[id$='SIPTInfo_SIPT9_PreferredHand']").val();
            formData.SIPTInfo_SIPT9_CrossingMidline = $("[id$='SIPTInfo_SIPT9_CrossingMidline']").val();

            // SIPT 10
            formData.SIPTInfo_SIPT10_Draw = $("[id$='SIPTInfo_SIPT10_Draw']").val();
            formData.SIPTInfo_SIPT10_ClockFace = $("[id$='SIPTInfo_SIPT10_ClockFace']").val();
            formData.SIPTInfo_SIPT10_Filtering = $("[id$='SIPTInfo_SIPT10_Filtering']").val();
            formData.SIPTInfo_SIPT10_MotorPlanning = $("[id$='SIPTInfo_SIPT10_MotorPlanning']").val();
            formData.SIPTInfo_SIPT10_BodyImage = $("[id$='SIPTInfo_SIPT10_BodyImage']").val();
            formData.SIPTInfo_SIPT10_BodySchema = $("[id$='SIPTInfo_SIPT10_BodySchema']").val();
            formData.SIPTInfo_SIPT10_Laterality = $("[id$='SIPTInfo_SIPT10_Laterality']").val();

            // Activity Given
            formData.SIPTInfo_ActivityGiven_Remark = $("[id$='SIPTInfo_ActivityGiven_Remark']").val();
            formData.SIPTInfo_ActivityGiven_InterestActivity = $("[id$='SIPTInfo_ActivityGiven_InterestActivity']").val();
            formData.SIPTInfo_ActivityGiven_InterestCompletion = $("[id$='SIPTInfo_ActivityGiven_InterestCompletion']").val();
            formData.SIPTInfo_ActivityGiven_Learning = $("[id$='SIPTInfo_ActivityGiven_Learning']").val();
            formData.SIPTInfo_ActivityGiven_Complexity = $("[id$='SIPTInfo_ActivityGiven_Complexity']").val();
            formData.SIPTInfo_ActivityGiven_ProblemSolving = $("[id$='SIPTInfo_ActivityGiven_ProblemSolving']").val();
            formData.SIPTInfo_ActivityGiven_Concentration = $("[id$='SIPTInfo_ActivityGiven_Concentration']").val();
            formData.SIPTInfo_ActivityGiven_Retension = $("[id$='SIPTInfo_ActivityGiven_Retension']").val();
            formData.SIPTInfo_ActivityGiven_SpeedPerfom = $("[id$='SIPTInfo_ActivityGiven_SpeedPerfom']").val();
            formData.SIPTInfo_ActivityGiven_Neatness = $("[id$='SIPTInfo_ActivityGiven_Neatness']").val();
            formData.SIPTInfo_ActivityGiven_Frustation = $("[id$='SIPTInfo_ActivityGiven_Frustation']").val();
            formData.SIPTInfo_ActivityGiven_Work = $("[id$='SIPTInfo_ActivityGiven_Work']").val();
            formData.SIPTInfo_ActivityGiven_Reaction = $("[id$='SIPTInfo_ActivityGiven_Reaction']").val();
            formData.SIPTInfo_ActivityGiven_SociabilityTherapist = $("[id$='SIPTInfo_ActivityGiven_SociabilityTherapist']").val();
            formData.SIPTInfo_ActivityGiven_SociabilityStudents = $("[id$='SIPTInfo_ActivityGiven_SociabilityStudents']").val();

            saveWithModal(formData, reloadAfterSave, "SIPT INFORMATION");
        }
        function SaveTab19(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 19;

            formData.Evaluation_Strengths = $("[id$='Evaluation_Strengths']").val();

            formData.Evaluation_Concern_Barriers = $("[id$='Evaluation_Concern_Barriers']").val();
            formData.Evaluation_Concern_Limitations = $("[id$='Evaluation_Concern_Limitations']").val();
            formData.Evaluation_Concern_Posture = $("[id$='Evaluation_Concern_Posture']").val();
            formData.Evaluation_Concern_Impairment = $("[id$='Evaluation_Concern_Impairment']").val();

            formData.Evaluation_Goal_Summary = $("[id$='Evaluation_Goal_Summary']").val();
            formData.Evaluation_Goal_Previous = $("[id$='Evaluation_Goal_Previous']").val();
            formData.Evaluation_Goal_LongTerm = $("[id$='Evaluation_Goal_LongTerm']").val();
            formData.Evaluation_Goal_ShortTerm = $("[id$='Evaluation_Goal_ShortTerm']").val();
            formData.Evaluation_Goal_Impairment = $("[id$='Evaluation_Goal_Impairment']").val();

            formData.Evaluation_Plan_Frequency = $("[id$='Evaluation_Plan_Frequency']").val();
            formData.Evaluation_Plan_Service = $("[id$='Evaluation_Plan_Service']").val();
            formData.Evaluation_Plan_Strategies = $("[id$='Evaluation_Plan_Strategies']").val();
            formData.Evaluation_Plan_Equipment = $("[id$='Evaluation_Plan_Equipment']").val();

            saveWithModal(formData, reloadAfterSave, "EVALUATION");
        }
        function SaveTab20(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 20;

            formData.Treatment_Home = $("[id$='Treatment_Home']").val();
            formData.Treatment_School = $("[id$='Treatment_School']").val();
            formData.Treatment_Threapy = $("[id$='Treatment_Threapy']").val();
            formData.Treatment_cmt = $("[id$='Treatment_cmt']").val();

            saveWithModal(formData, reloadAfterSave, "TREATMENT ADVICE");
        }
        function SaveTab21(reloadAfterSave) {

            var formData = {};
            formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
            formData.TabNo = 21;

            formData.Doctor_Physioptherapist = $("[id$='Doctor_Physioptherapist']").val();
            formData.Doctor_Occupational = $("[id$='Doctor_Occupational']").val();

            // if you want always NULL in DB then do NOT send Doctor_EnterReport
            // formData.Doctor_EnterReport = $("[id$='Doctor_EnterReport']").val();

            saveWithModal(formData, reloadAfterSave, "DOCTOR");
        }
        $(document).ready(function () {

            $("#confirmSaveBtn").off("click").on("click", function () {

                if (saveInProgress) return;

                if (!navigator.onLine) {
                    alert("No internet connection!");
                    return;
                }

                // ✅ LOG CONFIRM
                logModalAction("CONFIRM", modalFormData);

                if (pendingSaveCallback) {
                    pendingSaveCallback();
                }

                // mark as confirmed
                pendingSaveCallback = null;

                $("#confirmSaveModal").modal("hide");
            });

        });
        $('#confirmSaveModal').on('hidden.bs.modal', function () {

            // if still has callback → means user cancelled
            if (pendingSaveCallback !== null) {
                logModalAction("CANCEL", modalFormData);
            }

            // reset everything
            pendingSaveCallback = null;
        });
        var pendingSaveCallback = null;

        function showConfirmPopup(formData, onConfirm) {

            pendingSaveCallback = onConfirm;
            modalFormData = formData;

            // ✅ LOG OPEN
            logModalAction("OPEN", formData);

            var isOnline = navigator.onLine;
            var statusText = isOnline
                ? "<span style='color:green'>🟢 Online</span>"
                : "<span style='color:red'>🔴 Offline</span>";

            var connection = navigator.connection || navigator.mozConnection || navigator.webkitConnection;

            var speedText = "";

            if (connection) {
                var type = connection.effectiveType; // 'slow-2g', '2g', '3g', '4g'

                if (type === "4g") {
                    speedText = "<span style='color:green'>⚡ High Speed</span>";
                } else if (type === "3g") {
                    speedText = "<span style='color:orange'>🚀 Medium Speed</span>";
                } else {
                    speedText = "<span style='color:red'>🐢 Slow Network</span>";
                }
            } else {
                speedText = "<span style='color:gray'>Speed: Unknown</span>";
            }

            updateInternetStatus(statusText, speedText);

            var rows = "";
            for (var key in formData) {
                rows += "<tr><td><b>" + key + "</b></td><td>" + (formData[key] || "") + "</td></tr>";
            }

            $("#modalDataPreview").html(rows);

            $("#confirmSaveModal").modal("show");
        }
        function logModalAction(action, data) {

            data = data || {};

            $.ajax({
                type: "POST",
                url: "<%= ResolveUrl("~/Handler/SaveReportSi_2023.ashx") %>",
        data: {
            ActionType: "MODAL_LOG",
            LogAction: action,
            ModalLog: JSON.stringify(data),
                     Record: data.Record || "<%= Request.QueryString["record"] ?? "" %>",
                     TabNo: data.TabNo || 0
                 },

                 success: function () {
                     $("#logStatusMsg").html(
                         "<span style='color:green'>✔ Log saved successfully</span>"
                     );
                 },

                 error: function (xhr) {
                     $("#logStatusMsg").html(
                         "<span style='color:red'>❌ Log failed (" + xhr.status + ")</span>"
                     );
                 }
             });
        }

        function saveWithModal(formData, reloadAfterSave, tabName) {
            showConfirmPopup(formData, function () {
                PostToHandler(formData, reloadAfterSave, tabName);
            });
        }
        function updateInternetStatus(statusText, speedText) {
            $("#modalInternetStatus").html(
                "<div>" +
                "<span><b>Internet:</b> " + statusText + "</span>" +
                "<span style='margin-left:20px;'><b>Speed:</b> " + speedText + "</span>" +
                "</div>" +
                "<div id='logStatusMsg' style='margin-top:6px; font-size:12px;'></div>"
            );
        }
        // =========================
        // ALERT OVERLAY FUNCTION
        // =========================
        function showAlert(msg, type) {

            // create placeholder if not exists
            if ($("#MsgPlaceHolder").length === 0) {
                $("body").append('<div id="MsgPlaceHolder"></div>');
            }

            // overlay container style
            $("#MsgPlaceHolder").css({
                position: "fixed",
                top: "15px",
                left: "50%",
                transform: "translateX(-50%)",
                zIndex: 99999,
                width: "auto",
                maxWidth: "90%",
                minWidth: "1000px"
            });

            var html = "";

            if (type == 1) {
                html = '<div class="alert alert-success alert-dismissible" style="box-shadow:0 8px 20px rgba(0,0,0,0.2);">' +
                    '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' +
                    '<strong>Success !</strong> ' + msg +
                    '</div>';
            }
            else if (type == 2) {
                html = '<div class="alert alert-danger alert-dismissible" style="box-shadow:0 8px 20px rgba(0,0,0,0.2);">' +
                    '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' +
                    '<strong>Error !</strong> ' + msg +
                    '</div>';
            }
            else {
                html = '<div class="alert alert-info alert-dismissible" style="box-shadow:0 8px 20px rgba(0,0,0,0.2);">' +
                    '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' +
                    '<strong>Info !</strong> ' + msg +
                    '</div>';
            }

            $("#MsgPlaceHolder").stop(true, true).hide().html(html).fadeIn(200);

            // auto disappear after 10 sec
            setTimeout(function () {
                $("#MsgPlaceHolder").fadeOut(400, function () {
                    $(this).html("").show();
                });
            }, 10000);
        }
        function getCheckedText() {
            // arguments = checkbox selectors
            for (var i = 0; i < arguments.length; i++) {

                var cb = $(arguments[i]);
                if (cb.length > 0 && cb.is(":checked")) {

                    // ASP.NET checkbox text is usually in next sibling <label>
                    var txt = cb.next("label").text();

                    // fallback: if no label found, try parent text
                    if (!txt) txt = cb.parent().text();

                    return $.trim(txt);
                }
            }
            return "";
        }
        function GetSingleCheckText(chk1Id, chk2Id, chk3Id, chk4Id) {
            // returns checked checkbox Text (label) OR "" if none checked
            if (chk1Id && $("#" + chk1Id).is(":checked")) return $("#" + chk1Id).next("label").text().trim() || $("#" + chk1Id).attr("value") || "1";
            if (chk2Id && $("#" + chk2Id).is(":checked")) return $("#" + chk2Id).next("label").text().trim() || $("#" + chk2Id).attr("value") || "2";
            if (chk3Id && $("#" + chk3Id).is(":checked")) return $("#" + chk3Id).next("label").text().trim() || $("#" + chk3Id).attr("value") || "3";
            if (chk4Id && $("#" + chk4Id).is(":checked")) return $("#" + chk4Id).next("label").text().trim() || $("#" + chk4Id).attr("value") || "4";
            return "";
        }

        function GetMultiCheckValues(arrIds) {
            var list = [];
            for (var i = 0; i < arrIds.length; i++) {
                if ($("#" + arrIds[i]).is(":checked")) {
                    // take checkbox text
                    var txt = $("#" + arrIds[i]).next("label").text().trim();
                    if (txt === "") txt = $("#" + arrIds[i]).attr("value") || arrIds[i];
                    list.push(txt);
                }
            }
            return list.join("|"); // same pattern like DiagnosisIDs
        }
        function BuildQuestionsString() {

            var questions = "";

            $("#<%= updAgeStage.ClientID %> table tr").each(function () {

                 var row = $(this);

                 var qNo = row.find("[id$='lblQuestionNo']").text().trim();
                 if (!qNo) return; // skip header row

                 var yes = row.find("[id$='chkMonthYes']").prop("checked") ? "1" : "0";
                 var no = row.find("[id$='chkMonthNo']").prop("checked") ? "1" : "0";

                 var comment = row.find("[id$='txtMonthComment']").val() || "";
                 comment = comment.replace(/[~$]/g, "");

                 questions += qNo + "$" + yes + "$" + no + "$" + comment + "~";
             });

             if (questions.endsWith("~")) {
                 questions = questions.substring(0, questions.length - 1);
             }

             console.log("Questions String:", questions);

             return questions;
         }
     </script>

</asp:Content>

