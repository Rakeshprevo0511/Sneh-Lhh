<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_SIRpt2022.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_SIRpt2022" %>

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
    <div id="editor"></div>
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                SI Report :
            </div>
            <div class="pull-right">
                <a href="#" class="btn btn-primary">View List</a>
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
                        <asp:CheckBox ID="txtFinal" runat="server" Enabled="False" />
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Mark as Report Given :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtGiven" runat="server" Enabled="False" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Given Date :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtGivenDate" runat="server" CssClass="span2 my-datepicker" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
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
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text="Submit"
                            ClientIDMode="Static"></asp:LinkButton>
                        &nbsp; 
                        <%= _printUrl %>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                        <%--<input type='button' id='btn' value='Print' onclick='printDiv();'>--%>
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
                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis" Enabled="False"></asp:ListBox>
                    </div>
                    <div class="span2">
                        Other Diagnosis :
                    </div>
                    <div class="span2">
                        <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span2" Enabled="False"></asp:TextBox>
                    </div>
                </div>
            </asp:Panel>


            <div class="formRow">
                <div class="span12">
                       <asp:HiddenField ID="hfdTabs" runat="server" ClientIDMode="Static" />
                       <asp:HiddenField ID="hfdCallFrom" runat="server" ClientIDMode="Static" />
                       <asp:HiddenField ID="hfdCurTab" runat="server" ClientIDMode="Static" />
                       <asp:HiddenField ID="hfdPrevTab" runat="server" ClientIDMode="Static" />
                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" OnClientActiveTabChanged="clientActiveTabChanged">


                        <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="CLINICAL_OBSERVATION AND DAILY SCHEDULE ">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">

                                         <div class="col-md-12">
                                            <div class="control-label">
                                                Clinical Observations :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ClinicleObse_txt" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />

                                        <div class="clearfix"></div>
                                              <div class="clearfix"></div>
                                        <div class="col-md-12">
                                            <div class="control-label">
                                                Daily Schedule :
                                            </div>
                                             <div class="clearfix"></div>
                                            <ul style="display: flex; list-style-type: none; justify-content: space-evenly;">
                                               
                                                <label style="color:black;font:bold">TIME </label>
                                                <label style="color:black;font:bold">ACTIVITIES</label>
                                                <label style="color:black;font:bold">COMMENTS </label>
                                            </ul>
                                        </div>
                                        <div class="span5">
                                            <div class="control-label">
                                                <asp:HiddenField ID="textVisibleOption" runat="server" Value="2" />
                                                <div id="option_box_single_choice">
                                                    <div class="form-group row col-sm-12">
                                                        <div class="col-md-10">
                                                            <div class="cloneContainer">
                                                                <asp:Repeater ID="txtSignleChoice" runat="server" >
                                                                    <ItemTemplate>
                                                                        <div class='row cloneThisRow <%#cloneClass(Container.ItemIndex, Eval("Option").ToString(), Eval("Option1").ToString(), Eval("Option2").ToString(), Eval("Option3").ToString()) %>' >
                                                                            <div class="col-sm-2">
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <ul class="d-flex" style="display: flex; list-style-type: none;">
                                                                                    <asp:HiddenField ID="txtSI_ID" runat="server" Value='<%#Eval("Option") %>' />
                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtTIME" runat="server" Width="350" Text='<%#Eval("Option1") %>' Enabled="False"></asp:TextBox></li>

                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtACTIVITIES" runat="server" Width="350" Text='<%#Eval("Option2") %>' Enabled="False"></asp:TextBox></li>

                                                                                    <li class="mr_5">
                                                                                        <asp:TextBox ID="txtCOMMENTS" runat="server" Width="350" Text='<%#Eval("Option3") %>' Enabled="False"></asp:TextBox></li>


                                                                                   <%-- <div class="col-md-3 padding-5">
                                                                                        <%# cloneButtonLeft_sm(Container.ItemIndex)%>
                                                                                    </div>--%>
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
                                                1.Mother's Quality Time spent with the child daily. 
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_1" runat="server" onclick="FamilyStructure_QualityTimeMother_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" Enabled="False" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_2" runat="server" onclick="FamilyStructure_QualityTimeMother_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" Enabled="False"/>
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeMother_3" runat="server" onclick="FamilyStructure_QualityTimeMother_3_Click();"
                                                    CssClass="checkboes" Text="24X7" Enabled="False" />
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
                                                2.Father's Quality Time spent with the child daily. 
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_1" runat="server" onclick="FamilyStructure_QualityTimeFather_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours"  Enabled="False"/>
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_2" runat="server" onclick="FamilyStructure_QualityTimeFather_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" Enabled="False" />
                                                <asp:CheckBox ID="FamilyStructure_QualityTimeFather_3" runat="server" onclick="FamilyStructure_QualityTimeFather_3_Click();"
                                                    CssClass="checkboes" Text="24X7" Enabled="False" />
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
                                                3.Mother's Quality time spent on Sunday/ weekends with Child.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Mother_Weekends_1" runat="server" onclick="Mother_Weekends_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" Enabled="False" />
                                                <asp:CheckBox ID="Mother_Weekends_2" runat="server" onclick="Mother_Weekends_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" Enabled="False" />
                                                <asp:CheckBox ID="Mother_Weekends_3" runat="server" onclick="Mother_Weekends_3_Click();"
                                                    CssClass="checkboes" Text="24X7" Enabled="False" />
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
                                                4.Father's Quality time spent on Sunday/ weekends with Child.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Father_Weekends_1" runat="server" onclick="Father_Weekends_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5 hours" Enabled="False"/>
                                                <asp:CheckBox ID="Father_Weekends_2" runat="server" onclick="Father_Weekends_2_Click();"
                                                    CssClass="checkboes" Text="more than 5 hours" Enabled="False" />
                                                <asp:CheckBox ID="Father_Weekends_3" runat="server" onclick="Father_Weekends_3_Click();"
                                                    CssClass="checkboes" Text="24X7" Enabled="False"/>
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="FamilyStructure_TimeForThreapy_2" runat="server" onclick="FamilyStructure_TimeForThreapy_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="FamilyStructure_AcceptanceCondition_2" runat="server" onclick="FamilyStructure_AcceptanceCondition_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                7. Accessibility to play areas/extracurricular activities
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStructure_ExtraCaricular_1" runat="server" onclick="FamilyStructure_ExtraCaricular_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="FamilyStructure_ExtraCaricular_2" runat="server" onclick="FamilyStructure_ExtraCaricular_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                 CssClass="checkboes" Text=" YES" />
                             <asp:CheckBox ID="Mother_Working_2" runat="server" onclick="Mother_Working_2_Click();"
                                 CssClass="checkboes" Text="NO" />
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
                                 CssClass="checkboes" Text=" YES" />
                             <asp:CheckBox ID="Father_Working_2" runat="server" onclick="Father_Working_2_Click();"
                                 CssClass="checkboes" Text="NO" />
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
                                 CssClass="checkboes" Text=" YES" />
                             <asp:CheckBox ID="Househelp_2" runat="server" onclick="Househelp_2_Click();"
                                 CssClass="checkboes" Text="NO" />
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
                                                    Rows="8" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" />
                                                <asp:CheckBox ID="FamilyStructure_SiblingBrother_2" runat="server" onclick="FamilyStructure_SiblingBrother_1_Click();"
                                                    CssClass="checkboes" Text="NO" />
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
                                                9.Relationship With Siblings.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_SiblingBrother" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="10" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" />
                                                <asp:CheckBox ID="FamilyStructure_SiblingSister_2" runat="server" onclick="FamilyStructure_SiblingSister_1_Click();"
                                                    CssClass="checkboes" Text="NO" />
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
                             CssClass="checkboes" Text=" YES" />
                         <asp:CheckBox ID="FamilyStructure_SiblingNA_2" runat="server" onclick="FamilyStructure_SiblingNA_2_Click();"
                             CssClass="checkboes" Text="NO" />
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>


                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11. Others Closely Involved With:
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStructure_CloselyInvolved" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                <h6>A) Physical School</h6>
                                            </div>
                                            <div class="control-label">
                                                1.Does the child attend school
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Attend_1" runat="server" onclick="Schoolinfo_Attend_1_Click();"
                                                    CssClass="checkboes" Text="YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_Attend_2" runat="server" onclick="Schoolinfo_Attend_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    CssClass="checkboes" Text="Open" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_Type_2" runat="server" onclick="Schoolinfo_Type_2_Click();"
                                                    CssClass="checkboes" Text="Integrated" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_Type_3" runat="server" onclick="Schoolinfo_Type_3_Click();"
                                                    CssClass="checkboes" Text="Special" Enabled="False" />
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
                                                    3.Number of hours:
                                                </div>
                                                <asp:DropDownList ID="Schoolinfo_SchoolHours" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                    <asp:ListItem Value="1">1 to 5 Hour</asp:ListItem>
                                                    <asp:ListItem Value="2">More Than 5 Hours</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                        <div class="formRow">
                                            <div class="span12">
                                                <div class="control-label">
                                                    4. How do they travel:
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                        <asp:CheckBox ID="chkSchool_Bus" runat="server" CssClass="checkboes" onclick="School_Bus_Click();" Text="School_Bus" Enabled="False"/>
                                                        <asp:CheckBox ID="chkCar" runat="server" CssClass="checkboes" onclick="Car_Click();" Text="Car" Enabled="False" />
                                                        <asp:CheckBox ID="chkTwo_Wheelers" runat="server" CssClass="checkboes" onclick="Two_Wheelers_Click();" Text="Two_Wheelers" Enabled="False"/>
                                                        <asp:CheckBox ID="chkwalking" runat="server" CssClass="checkboes" onclick="walking_Click();" Text="walking" Enabled="False" />
                                                        <asp:CheckBox ID="chkPublic_Transport" runat="server" CssClass="checkboes" onclick="Public_Transport_Click();" Text="Public_Transport" Enabled="False"/>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                5.	Teacher to child ratio
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_1" runat="server" onclick="Schoolinfo_NoOfTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" 1 to 5" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_2" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text=" 1 to 30" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_3" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text="1 to 60" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_NoOfTeacher_4" runat="server" onclick="Schoolinfo_NoOfTeacher_2_Click();"
                                                    CssClass="checkboes" Text="	more than 60" Enabled="False" />
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
                                                    6.Seating arrangement:
                                                </div>
                                                <div class="span5">
                                                    <div class="control-label">
                                                        <asp:CheckBox ID="chkFloor" runat="server" CssClass="checkboes" onclick="Floor_Click();" Text="Floor" Enabled="False" />
                                                        <asp:CheckBox ID="chksingle_bench" runat="server" CssClass="checkboes" onclick="single_bench_Click();" Text="single_bench" Enabled="False" />
                                                        <asp:CheckBox ID="chkbench2" runat="server" CssClass="checkboes" onclick="bench2_Click();" Text="bench2" Enabled="False"/>
                                                        <asp:CheckBox ID="chkround_table" runat="server" CssClass="checkboes" onclick="round_table_Click();" Text="round_table" Enabled="False"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span12 ">
                                            <div class="row">
                                                <div class="span2">
                                                    7.Meal time at the school:
                                                </div>
                                                <asp:DropDownList ID="Schoolinfo_Mealtime" runat="server" Height="30px" Width="132px" Enabled="False">
                                                    <asp:ListItem Value="0">Select </asp:ListItem>
                                                    <asp:ListItem Value="01">1</asp:ListItem>
                                                    <asp:ListItem Value="02">2</asp:ListItem>
                                                    <asp:ListItem Value="03">3</asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Meal type
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
                                                9.Sharing done by child
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Shareing_1" runat="server" onclick="Schoolinfo_Shareing_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Shareing_2" runat="server" onclick="Schoolinfo_Shareing_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Shareing_3" runat="server" onclick="Schoolinfo_Shareing_3_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False" />
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
                                                10.Help required in eating
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_HelpEating_1" runat="server" onclick="Schoolinfo_HelpEating_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_HelpEating_2" runat="server" onclick="Schoolinfo_HelpEating_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                11.Friendships initiated by child
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Friendship_1" runat="server" onclick="Schoolinfo_Friendship_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_Friendship_2" runat="server" onclick="Schoolinfo_Friendship_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                12.Interaction with peers
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_InteractionPeer_1" runat="server" onclick="Schoolinfo_InteractionPeer_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_InteractionPeer_2" runat="server" onclick="Schoolinfo_InteractionPeer_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                13.Interaction with the teacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_InteractionTeacher_1" runat="server" onclick="Schoolinfo_InteractionTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_InteractionTeacher_2" runat="server" onclick="Schoolinfo_InteractionTeacher_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                14.Annuals/culturals function
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_AnnualFunction_1" runat="server" onclick="Schoolinfo_AnnualFunction_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_AnnualFunction_2" runat="server" onclick="Schoolinfo_AnnualFunction_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                15.SPORTS.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Sports_1" runat="server" onclick="Schoolinfo_Sports_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Sports_2" runat="server" onclick="Schoolinfo_Sports_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                16.Picnics/field trips
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Picnic_1" runat="server" onclick="Schoolinfo_Picnic_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Picnic_2" runat="server" onclick="Schoolinfo_Picnic_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                17.Extra curricular
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_ExtraCaricular_1" runat="server" onclick="Schoolinfo_ExtraCaricular_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_ExtraCaricular_2" runat="server" onclick="Schoolinfo_ExtraCaricular_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                18.Copying from board
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_1" runat="server" onclick="Schoolinfo_CopyBoard_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_2" runat="server" onclick="Schoolinfo_CopyBoard_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_3" runat="server" onclick="Schoolinfo_CopyBoard_3_Click();"
                                                    CssClass="checkboes" Text="Inconsistent" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_CopyBoard_4" runat="server" onclick="Schoolinfo_CopyBoard_4_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False" />
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
                                                19.Follows instructions
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_Instructions_1" runat="server" onclick="Schoolinfo_Instructions_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Instructions_2" runat="server" onclick="Schoolinfo_Instructions_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Instructions_3" runat="server" onclick="Schoolinfo_Instructions_3_Click();"
                                                    CssClass="checkboes" Text="Sometime" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_Instructions_4" runat="server" onclick="Schoolinfo_Instructions_4_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False" />
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
                                                20.ShadowTeacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_1" runat="server" onclick="Schoolinfo_ShadowTeacher_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_2" runat="server" onclick="Schoolinfo_ShadowTeacher_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_3" runat="server" onclick="Schoolinfo_ShadowTeacher_3_Click();"
                                                    CssClass="checkboes" Text="Needs Help"  Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_ShadowTeacher_4" runat="server" onclick="Schoolinfo_ShadowTeacher_4_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False"/>
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
                                                21.Completing CW/HW
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_1" runat="server" onclick="Schoolinfo_CW_HW_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_2" runat="server" onclick="Schoolinfo_CW_HW_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_3" runat="server" onclick="Schoolinfo_CW_HW_3_Click();"
                                                    CssClass="checkboes" Text="Needs Help" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_CW_HW_4" runat="server" onclick="Schoolinfo_CW_HW_4_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False"/>
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
                                                22.Provision of special educator/shadow/ remedial teacher
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_1" runat="server" onclick="Schoolinfo_SpecialEducator_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_2" runat="server" onclick="Schoolinfo_SpecialEducator_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_SpecialEducator_3" runat="server" onclick="Schoolinfo_SpecialEducator_3_Click();"
                                                    CssClass="checkboes" Text="NA" Enabled="False" />
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
                                                23.Mode of delivery of information
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_1" runat="server" onclick="Schoolinfo_DeliveryInformation_1_Click();"
                                                    CssClass="checkboes" Text=" PPT" Enabled="False" />
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_2" runat="server" onclick="Schoolinfo_DeliveryInformation_2_Click();"
                                                    CssClass="checkboes" Text="Videos" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_3" runat="server" onclick="Schoolinfo_DeliveryInformation_3_Click();"
                                                    CssClass="checkboes" Text="Books" Enabled="False"/>
                                                <asp:CheckBox ID="Schoolinfo_DeliveryInformation_4" runat="server" onclick="Schoolinfo_DeliveryInformation_3_Click();"
                                                    CssClass="checkboes" Text="NOTA" Enabled="False"/>
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
                                                24.Remark of the teacher

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Schoolinfo_RemarkTeacher" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
               CssClass="checkboes" Text=" YES" />
           <asp:CheckBox ID="Schoolinfo_SitOnlineSchool_2" runat="server" onclick="Schoolinfo_SitOnlineSchool_2_Click();"
               CssClass="checkboes" Text="NO" />
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
                      CssClass="checkboes" Text=" YES" />
                  <asp:CheckBox ID="Schoolinfo_TeacherInstruction_2" runat="server" onclick="Schoolinfo_TeacherInstruction_2_Click();"
                      CssClass="checkboes" Text="NO" />
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
                                                    <h5>A)RELATIONSHIP WITH SELF</h5>
                                                </div>
                                            </div>
                                            1. DOES HE KNOW THE CURRENT PLACE? :
              
              <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_1" runat="server" onclick="PersonalSocial_CurrentPlace_1_Click();"
                      CssClass="checkboes" Text=" YES" Enabled="False" />
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_2" runat="server" onclick="PersonalSocial_CurrentPlace_2_Click();"
                      CssClass="checkboes" Text="N0" Enabled="False"/>
                  <asp:CheckBox ID="PersonalSocial_CurrentPlace_3" runat="server" onclick="PersonalSocial_CurrentPlace_3_Click();"
                      CssClass="checkboes" Text="SOMETIMES" Enabled="False" />
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
                                            <div class="row">
                                                <div class="span2">
                                                    2. IS YOUR CHILD AWARE OF WHAT HE DOES? :
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_1" runat="server" onclick="PersonalSocial_WhatHeDoes_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_2" runat="server" onclick="PersonalSocial_WhatHeDoes_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_WhatHeDoes_3" runat="server" onclick="PersonalSocial_WhatHeDoes_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False" />
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
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    3. DOES THE CHILD HAVE OWN BODY AWARENESS?
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_1" runat="server" onclick="PersonalSocial_CurrentPlace_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_2" runat="server" onclick="PersonalSocial_CurrentPlace_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_BodyAwareness_3" runat="server" onclick="PersonalSocial_CurrentPlace_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False"/>
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
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    4. IS YOUR CHILD AWARE OF BODY SCHEMA?
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_1" runat="server" onclick="PersonalSocial_BodySchema_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_2" runat="server" onclick="PersonalSocial_BodySchema_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_BodySchema_3" runat="server" onclick="PersonalSocial_BodySchema_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False"/>
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
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    5. DOES YOUR CHILD SELF EXPLORES THE ENVIRONMENT?
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_1" runat="server" onclick="PersonalSocial_ExploreEnvironment_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_2" runat="server" onclick="PersonalSocial_ExploreEnvironment_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="PersonalSocial_ExploreEnvironment_3" runat="server" onclick="PersonalSocial_ExploreEnvironment_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False" />
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
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    6. IS YOUR CHILD MOTIVATED?
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_1" runat="server" onclick="PersonalSocial_Motivated_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_2" runat="server" onclick="PersonalSocial_Motivated_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False"/>
                                                    <asp:CheckBox ID="PersonalSocial_Motivated_3" runat="server" onclick="PersonalSocial_Motivated_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False" />
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
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    <h5>B) RELATIONSHIP WITH OTHERS</h5>
                                                </div>
                                            </div>
                                            1. Eye contact
                          
              <div class="control-group" style="padding-left: 20px">
                  <asp:CheckBox ID="PersonalSocial_EyeContact_1" runat="server" onclick="PersonalSocial_EyeContact_1_Click();"
                      CssClass="checkboes" Text="fleeting" Enabled="False"/>
                  <asp:CheckBox ID="PersonalSocial_EyeContact_2" runat="server" onclick="PersonalSocial_EyeContact_2_Click();"
                      CssClass="checkboes" Text="Poor" Enabled="False" />
                  <asp:CheckBox ID="PersonalSocial_EyeContact_3" runat="server" onclick="PersonalSocial_EyeContact_3_Click();"
                      CssClass="checkboes" Text="Fair" Enabled="False" />
                  <asp:CheckBox ID="PersonalSocial_EyeContact_4" runat="server" onclick="PersonalSocial_EyeContact_4_Click();"
                      CssClass="checkboes" Text="Good" Enabled="False" />
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

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="control-group" style="padding-left: 20px">
                                                    2. Social Smile
                                                </div>
                                            </div>
                                            <asp:CheckBox ID="PersonalSocial_SocialSmile_1" runat="server" onclick="PersonalSocial_SocialSmile_1_Click();"
                                                CssClass="checkboes" Text="fleeting" Enabled="False" />
                                            <asp:CheckBox ID="PersonalSocial_SocialSmile_2" runat="server" onclick="PersonalSocial_SocialSmile_2_Click();"
                                                CssClass="checkboes" Text="Poor" Enabled="False" />
                                            <asp:CheckBox ID="PersonalSocial_SocialSmile_3" runat="server" onclick="PersonalSocial_SocialSmile_3_Click();"
                                                CssClass="checkboes" Text="Fair" Enabled="False" />
                                            <asp:CheckBox ID="PersonalSocial_SocialSmile_4" runat="server" onclick="PersonalSocial_SocialSmile_4_Click();"
                                                CssClass="checkboes" Text="Good" Enabled="False" />
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


                                        <div class="span12">
                                            <div class="control-label">
                                                3. Family Regards
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="PersonalSocial_FamilyRegards_1" runat="server" onclick="PersonalSocial_FamilyRegards_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="PersonalSocial_FamilyRegards_2" runat="server" onclick="PersonalSocial_FamilyRegards_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                <h6>HOW IS THE CHILD SOCIALLY?:</h6>
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_1" runat="server" onclick="PersonalSocial_ChildSocially_1_Click();"
                                                    CssClass="checkboes" Text="AWFUL" Enabled="False" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_2" runat="server" onclick="PersonalSocial_ChildSocially_2_Click();"
                                                    CssClass="checkboes" Text="GOOD"  Enabled="False"/>
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_3" runat="server" onclick="PersonalSocial_ChildSocially_3_Click();"
                                                    CssClass="checkboes" Text="OKAY" Enabled="False" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_4" runat="server" onclick="PersonalSocial_ChildSocially_4_Click();"
                                                    CssClass="checkboes" Text="REALLY GOOD" Enabled="False" />
                                                <asp:CheckBox ID="PersonalSocial_ChildSocially_5" runat="server" onclick="PersonalSocial_ChildSocially_5_Click();"
                                                    CssClass="checkboes" Text="FANTASTIC" Enabled="False" />
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                1.  When did your Child Start to Speak
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_StartSpeek" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.  When did your Child Start to Monosyllables
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Monosyllables" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.  When did your Child Start to Bisyllables
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_Bisyllables" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.  When did your Child Start to short sentences
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_ShrotScentences" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.  When did your Child Start to long Sentences

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_LongScentences" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    6. Unusual Sounds /Jargon Speech
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="SpeechLanguage_UnusualSoundsJargonSpeech_1" runat="server" onclick="SpeechLanguage_UnusualSoundsJargonSpeech_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="SpeechLanguage_UnusualSoundsJargonSpeech_2" runat="server" onclick="SpeechLanguage_UnusualSoundsJargonSpeech_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
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
                                                7. Imitation of speech / gestures
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SpeechLanguage_speechgestures_1" runat="server" onclick="SpeechLanguage_speechgestures_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="SpeechLanguage_speechgestures_2" runat="server" onclick="SpeechLanguage_speechgestures_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                9.  Non verbal facial: Eye Contact

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NonverbalfacialEyeContact" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                12.Interpretation of language:Understand Implied Meaning

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_UnderstandImpliedMeaning" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12 formRow">
                                            <div class="row">
                                                <div class="span2">
                                                    <h6>15. Two - way Interaction</h6>
                                                </div>
                                                <div class="control-group" style="padding-left: 20px">
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_1" runat="server" onclick="SpeechLanguage_TwowayInteraction_1_Click();"
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_2" runat="server" onclick="SpeechLanguage_TwowayInteraction_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="SpeechLanguage_TwowayInteraction_3" runat="server" onclick="SpeechLanguage_TwowayInteraction_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False"/>
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
                                                16. Narrate Incidents:At School

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NarrateIncidentsAtSchool" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>

                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                17.Narrate Incidents:At Home/Expression of :Want

                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SpeechLanguage_NarrateIncidentsAtHome" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                1.BEHAVIOUR OF THE CHILD :- What does the child do in his free time
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_FreeTime" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                        <asp:CheckBox ID="chkunassociated" runat="server" CssClass="checkboes" onclick="unassociated_Click();" Text="unassociated" Enabled="False" />
                                                        <asp:CheckBox ID="chksolitary" runat="server" CssClass="checkboes" onclick="solitary_Click();" Text="solitary" Enabled="False"/>
                                                        <asp:CheckBox ID="chkonlooker" runat="server" CssClass="checkboes" onclick="onlooker_Click();" Text="onlooker" Enabled="False"/>
                                                        <asp:CheckBox ID="chkparallel" runat="server" CssClass="checkboes" onclick="parallel_Click();" Text="parallel" Enabled="False"/>
                                                        <asp:CheckBox ID="chkassociative" runat="server" CssClass="checkboes" onclick="associative_Click();" Text="associative" Enabled="False" />
                                                        <asp:CheckBox ID="chkcooperative" runat="server" CssClass="checkboes" onclick="cooperative_Click();" Text="cooperative" Enabled="False"/>
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
                                                        CssClass="checkboes" Text=" YES" Enabled="False" />
                                                    <asp:CheckBox ID="Behaviour_situationalmeltdowns_2" runat="server" onclick="Behaviour_situationalmeltdowns_2_Click();"
                                                        CssClass="checkboes" Text="N0" Enabled="False" />
                                                    <asp:CheckBox ID="Behaviour_situationalmeltdowns_3" runat="server" onclick="Behaviour_situationalmeltdowns_3_Click();"
                                                        CssClass="checkboes" Text="SOMETIMES" Enabled="False"/>
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                            <datalist2 id="values" Enabled="False">
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
                                            <datalist2 id="values2" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Arousal_Stimuli_2" runat="server" onclick="Arousal_Stimuli_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
                                                <asp:CheckBox ID="Arousal_Stimuli_3" runat="server" onclick="Arousal_Stimuli_3_Click();"
                                                    CssClass="checkboes" Text="Sometimes" Enabled="False" />
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
                                                4.Maintainance Of Arousal During Transition.
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Arousal_Transition_1" runat="server" onclick="Arousal_Transition_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Arousal_Transition_2" runat="server" onclick="Arousal_Transition_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                <asp:TextBox ID="Arousal_FactorOCD" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3" Enabled="False"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.Calming factor.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_ClaimingFactor" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3" Enabled="False"></asp:TextBox>
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Affect_RangeEmotion_2" runat="server" onclick="Affect_RangeEmotion_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                2 .Is the Child able to express emotion
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Affect_ExpressEmotion_1" runat="server" onclick="Affect_ExpressEmotion_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Affect_ExpressEmotion_2" runat="server" onclick="Affect_ExpressEmotion_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                7.Factors Characterising affect
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Charaterising" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                    Rows="8" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Attention_FocusHandhome_2" runat="server" onclick="Attention_FocusHandhome_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    CssClass="checkboes" Text="YES" Enabled="False" />
                                                <asp:CheckBox ID="Attention_FocusHandSchool_2" runat="server" onclick="Attention_FocusHandSchool_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                4.	Dividing attention
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_Dividing_1" runat="server" onclick="Attention_Dividing_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Attention_Dividing_2" runat="server" onclick="Attention_Dividing_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                5.	Change of activities every
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_ChangeActivities" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.	Age appropriate attention   
                        <div class="control-group" style="padding-left: 20px">
                            <asp:CheckBox ID="Attention_AgeAppropriate_1" runat="server" onclick="Attention_AgeAppropriate_1_Click();"
                                CssClass="checkboes" Text=" YES" Enabled="False" />
                            <asp:CheckBox ID="Attention_AgeAppropriate_2" runat="server" onclick="Attention_AgeAppropriate_2_Click();"
                                CssClass="checkboes" Text="NO" Enabled="False"/>
                            <asp:CheckBox ID="Attention_AgeAppropriate_3" runat="server" onclick="Attention_AgeAppropriate_3_Click();"
                                CssClass="checkboes" Text="Sometimes" Enabled="False"/>
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
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                8.Focal Attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Focal_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                9.Joint Attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Joint_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                10.Divided Attention 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Divided_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                11.Alternating Attention
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Alternating_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                12.Sustained Attention
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Sustained_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Attention_move_2" runat="server" onclick="Attention_move_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                1.Age appropriate Motor planning
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Action_MotorPlanning" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Action_Purposeful_2" runat="server" onclick="Action_Purposeful_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                3.Goal Oriented
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_GoalOriented_1" runat="server" onclick="Action_GoalOriented_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Action_GoalOriented_2" runat="server" onclick="Action_GoalOriented_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                4.Feedback Dependent
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_FeedBackDependent_1" runat="server" onclick="Action_FeedBackDependent_1_Click();"
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Action_FeedBackDependent_2" runat="server" onclick="Action_FeedBackDependent_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False"/>
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False"/>
                                                <asp:CheckBox ID="Action_Constructive_2" runat="server" onclick="Action_Constructive_2_click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                    <asp:CheckBox ID="chkInteracts" runat="server" CssClass="checkboes" onclick="Interacts_Click();" Text="Interacts" Enabled="False" />
                                                    <asp:CheckBox ID="chkDoes_not_initiate" runat="server" CssClass="checkboes" onclick="Does_not_initiate_Click();" Text="Does not initiate" Enabled="False" />
                                                    <asp:CheckBox ID="chkSustain" runat="server" CssClass="checkboes" onclick="Sustain_Click();" Text="Does not Sustain" Enabled="False" />
                                                    <asp:CheckBox ID="chkFight" runat="server" CssClass="checkboes" onclick="Fight_Click();" Text="Fight" Enabled="False" />
                                                    <asp:CheckBox ID="chkFreeze" runat="server" CssClass="checkboes" onclick="Freeze_Click();" Text="Freeze" Enabled="False" />
                                                    <asp:CheckBox ID="chkFright" runat="server" CssClass="checkboes" onclick="Fright_Click();" Text="Fright" Enabled="False" />
                                                    <asp:CheckBox ID="chkAnxious" runat="server" CssClass="checkboes" onclick="Anxious_Click();" Text="Anxious" Enabled="False" />
                                                    <asp:CheckBox ID="chkComfortable" runat="server" CssClass="checkboes" onclick="Comfortable_Click();" Text="Comfortable" Enabled="False" />
                                                    <asp:CheckBox ID="chkNervous" runat="server" CssClass="checkboes" onclick="Nervous_Click();" Text="Nervous" Enabled="False" />
                                                    <asp:CheckBox ID="chkANS_response" runat="server" CssClass="checkboes" onclick="ANS_response_Click();" Text="ANS_response" Enabled="False" />
                                                    <asp:CheckBox ID="chkOTHERS" runat="server" CssClass="checkboes" onclick="OTHERS_Click();" Text="OTHERS" Enabled="False" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="cmtgathering" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Interaction_SocialQues_2" runat="server" onclick="Interaction_SocialQues_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                                                3.Reaction to emotion of other Happiness
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Happiness" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Reaction to emotion Sadness
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Sadness" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                5.Reaction to emotion Surprise
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Surprise" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                6.Reaction to emotion  Shock
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Shock" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    CssClass="checkboes" Text=" YES" Enabled="False" />
                                                <asp:CheckBox ID="Interaction_Friends_2" runat="server" onclick="Interaction_Friends_2_Click();"
                                                    CssClass="checkboes" Text="NO" Enabled="False" />
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
                      CssClass="checkboes" Text=" YES" />
                  <asp:CheckBox ID="Interaction_RelatesPeople_2" runat="server" onclick="Interaction_RelatesPeople_2_Click();"
                      CssClass="checkboes" Text="NO" />
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
                                                8.What Activities Does He/She Enjoys.
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Interaction_Enjoy" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                        <asp:TextBox ID="TS_Registration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Responsiveness ( Hyper responsive/Hyporesponsive ) 
                                                      Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TS_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>SOMATOSENSORY SYSTEM- ( Tactile-Vestibular - Prop Trio)</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Body awareness
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bodyawareness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Body schema
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bodyschema" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Orientation of body in space
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Posterior space awareness
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Posterior" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Bilateral Coordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Bilateral" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Balance on static and dynamic surfaces
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Balance" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                               Dominance
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Dominance" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                               Right and Left Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Right" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                              How well he identifies body parts
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_identifies" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Can name and point objects/ people
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_point" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Constantly bumps into objects in his path
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_Constantly" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Is he clumsy with his things
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_clumsy" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Can he maneuver himself out ofa 
                                                variety of equipment orsituations?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_maneuver" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Is he overly fidgety?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_overly" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Is he able to stand in line 
                                                      duringor waits for his turn
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_stand" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Does he indulge into rough/sportplay?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_indulge" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Does he dislike any type of textures?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_textures" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                      Use of monkey ladders, obstacle course 
                                                      (climbing up and crossing) Commando crawl
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_monkey" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Use of swings Slide Can he perform heavy activities Cycle/tricycle 
                                                      Riding Can he maintain good posture while sitting?
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SS_swings" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>VESTIBULAR SYSTEM</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Registration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                     Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Responsiveness ( Hyporesponsive /Hyperresponsive )
                                                     Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VM_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>PROPRIOCEPTIVE SYSTEM</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Registration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Gradation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Gradation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                    Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness Mention the Behavioral 
                                                      responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PS_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>ORO- MOTOR SYSTEM:</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Registration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness(Hyporesponsive /Hyperresponsive ) 
                                                   Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OM_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div class="control-label">
                                                <h6>AUDITORY SYSTEM:</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Auditory Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Auditory" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Auditory Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                   Responsiveness(Hyporesponsive/ Hyperresponsive) 
                                                   Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Auditory discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Background-foreground discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Background" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Auditory localization
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_localization" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Analysis and synthesis
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_Analysis" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Auditory memory and sequencing
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_sequencing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Auditory blending (breaking of sounds)
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AS_blending" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>


                                            </table>
                                            <div class="control-label">
                                                <h6>VISUAL SYSTEM:</h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Visual Localization and Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Visual" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                  Responsiveness(Hyporesponsive/Hyperresponsive ) 
                                                  Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual scanning
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_scanning" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual constancy
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_constancy" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_memory" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Visual Perception
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Perception" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Eye hand Co- ordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_hand" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Eye foot Co- ordination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_foot" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual closure
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_closure" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Figure-ground discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Figureground" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_Visualmemory" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual sequential memory
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_sequential" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                Visual spatial relationships
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="VS_spatial" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="control-label">
                                                <h6>OLFACTORY SYSTEM : </h6>
                                            </div>
                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Registration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Registration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Orientation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Orientation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Discrimination
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Discrimination" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="span3">&nbsp;
                                                 Responsiveness(Hyporesponsive/Hyperresponsive ) 
                                                 Mention the Behavioral responses shown by the child
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="OS_Responsiveness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
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
                                                1.Denver’s  checklist  Gross motor
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_GrossMotor" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2.Denver’s checklist Fine motor
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_FineMotor" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3.Denver’s checklist Language
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_DenverLanguage" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
                                                </asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                4.Denver’s checklist Personal & social
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="TestMeassures_DenverPersonal" runat="server" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8" Enabled="False">
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
                                                    Rows="3" Enabled="False"></asp:TextBox>
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
                                                            <asp:TextBox ID="score_Communication_2" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="Inter_Communication_2" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>41.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_2" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_Gross_2" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>30.16</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_2" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_FINE_2" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>24.62</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_2" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_PROBLEM_2" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>33.71</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_2" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_PERSONAL_2" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="Comm_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_3" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="GROSS_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_3" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="FINE_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_3" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_3" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>33.96</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_3" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="Communication_6" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="comm_inter_6" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="GROSS_6" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_6" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="FINE_6" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_6" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_6" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_6" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_6" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_6" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_7" runat="server" CssClass="span1" Enabled="false"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_7" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>30.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_7" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_7" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>40.15</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_7" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_7" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>36.17</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_7" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_7" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>35.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_7" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_7" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_9" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_9" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>30.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_9" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_9" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>40.15</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_9" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_9" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>36.17</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_9" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_9" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>35.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_9" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_9" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_10" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_10" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>30.07</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_10" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_10" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>37.97</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_10" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_10" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>32.51</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_10" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_10" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>27.25</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_10" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_10" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_11" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_11" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>21.49</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_11" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_11" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>34.50</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_11" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_11" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>27.32</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_11" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_11" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>21.73</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_11" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_11" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_13" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_13" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_13" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_13" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_13" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_13" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_13" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_13" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_13" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_13" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_15" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_15" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_15" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_15" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_15" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_15" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_15" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_15" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_15" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_15" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_17" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_17" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>25.80</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_17" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_17" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>23.06</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_17" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_17" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>22.56</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_17" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_17" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>23.18</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_17" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_17" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_19" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_19" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>39.89</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_19" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_19" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>36.05</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_19" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_19" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>28.84</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_19" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_19" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
                                                    </td>
                                                    <td>
                                                        <b>33.36</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_19" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PERSONAL_inter_19" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
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
                                                            <asp:TextBox ID="comm_21" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="inter_21" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>GROSS MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>27.75</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_21" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="GROSS_inter_21" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>FINE MOTOR</b>
                                                    </td>
                                                    <td>
                                                        <b>29.61</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_21" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="FINE_inter_21" runat="server" CssClass="span3" Enabled="False"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PROBLEM SOLVING</b>
                                                    </td>
                                                    <td>
                                                        <b>29.30</b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_21" runat="server" CssClass="span1" Enabled="False"></asp:TextBox></b><b>/60 </b>
                                                    </td>
                                                    <td>
                                                        <b>
                                                            <asp:TextBox ID="PROBLEM_inter_21" runat="server" CssClass="span3"></asp:TextBox></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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
                                                        <b>GROSS MOTOR</b>
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
                                                        <b>FINE MOTOR</b>
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
                                                        <b>PROBLEM SOLVING</b>
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
                                                        <b>PERSONAL SOCIAL</b>
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





                                            <asp:UpdatePanel ID="updAgeStage" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="SelectMonth" runat="server" CssClass="input-medium chzn-select span2"  EnableViewState="true">
                                                    </asp:DropDownList>

                                                    <table style="border: 1px solid gray">
                                                        <tr>
                                                            <td>Sr No</td>
                                                            <td>OVERALL RESPONSES</td>
                                                            <td>YES  </td>
                                                            <td>NO   </td>
                                                            <td>COMMENTS</td>
                                                        </tr>
                                                        <asp:Repeater ID="rptQuestions" runat="server" >
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
                                                        <td>GENERAL Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="General_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>AUDITORY Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="AUDITORY_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>VISUAL Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="VISUAL_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>TOUCH Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TOUCH_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>MOVEMENT Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="MOVEMENT_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ORAL Processing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ORAL_Processing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Infant Sensory Profile 2 Raw Score Total
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Raw_score" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="Total_rawscore" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /125</b>
                                                        </td>
                                                        <%--<td>
                                                <asp:TextBox ID="Percentile_Range" runat="server" CssClass="span3"></asp:TextBox>
                                            </td>--%>
                                                        <td>
                                                            <asp:TextBox ID="Interpretation" runat="server" CssClass="span3" Enabled="false"></asp:TextBox>
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
                                                <asp:TextBox ID="Comments_1" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="Score_seeking" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /35</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="SEEKING" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_Avoiding" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /55</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="AVOIDING" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_sensitivity" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /65</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="SENSITIVITY_2" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_Registration" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /55</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="REGISTRATION" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_general" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="GENERAL" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_Auditory" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/35</b>
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
                                                                <asp:TextBox ID="Score_visual" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="VISUAL" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_touch" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="TOUCH" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_movement" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/25</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="MOVEMENT" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_oral" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/35</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ORAL" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Score_behavioural" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="BEHAVIORAL" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                <asp:TextBox ID="Comments_2" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="SPchild_Seeker" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /95</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Seeking_Seeker" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Avoider" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /100</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Avoiding_Avoider" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Sensor" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/95</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensitivity_Sensor" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Bystander" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                /110</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Registration_Bystander" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Auditory_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Auditory_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Visual_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/30</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Visual_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Touch_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/55</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Touch_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Movement_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/40</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Movement_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Body_position" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/40</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Body_position" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Oral_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/35</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Oral_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Conduct_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/45</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Conduct_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Social_emotional" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/70</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Social_emotional" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPchild_Attentional_3" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/50</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Attentional_3" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                    <asp:TextBox ID="Comments_3" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="SPAdult_Low_Registration" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Low_Registration" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPAdult_Sensory_seeking" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_seeking" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPAdult_Sensory_Sensitivity" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Sensitivity" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SPAdult_Sensory_Avoiding" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Avoiding" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                    <asp:TextBox ID="Comments_4" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="SP_Low_Registration64" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Low_Registration_5" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SP_Sensory_seeking_64" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_seeking_5" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SP_Sensory_Sensitivity64" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Sensitivity_5" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="SP_Sensory_Avoiding64" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Sensory_Avoiding_5" runat="server" CssClass="input-medium chzn-select span2"  Enabled="False">
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
                                                    <asp:TextBox ID="Comments_5" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                                <asp:TextBox ID="Older_Low_Registration" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Low_Registration_6" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Older_Sensory_seeking" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_seeking_6" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Older_Sensory_Sensitivity" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_Sensitivity_6" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                                <asp:TextBox ID="Older_Sensory_Avoiding" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b>
                                                        </td>
                                                        <td>
                                                            <div class="" style="display: inline-block; margin-right: 5px;">
                                                                <asp:DropDownList ID="Sensory_Avoiding_6" runat="server" CssClass="input-medium chzn-select span2" Enabled="False">
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
                                                <asp:TextBox ID="Comments_6" runat="server" CssClass="span4 savedata" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                                                    <asp:DropDownList ID="MonthSelect" runat="server" CssClass="input-medium chzn-select span2"  EnableViewState="true" Enabled="False">
                                                    </asp:DropDownList>

                                                    <table style="border: 1px solid gray">
                                                        <%-- <tr>
                                                            <td>SrNo</td>
                                                            <td>Question</td>
                                                            <td>YES</td>
                                                            <td>NO</td>
                                                        </tr>--%>


                                                        <asp:Repeater ID="abilityQuestionsParent" runat="server" >
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
                                                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.CategoryID")%>' Visible="false"></asp:Label>
                                                                                </center>
                                                                            </td>
                                                                            <td><%#DataBinder.Eval(Container,"DataItem.Question")%></td>
                                                                            <td>
                                                                                <center>
                                                                                    <asp:CheckBox runat="server" ID="CheckBox1" CssClass="clsMonthYes" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.Yes"))%>' />
                                                                                </center>
                                                                            </td>
                                                                            <td>
                                                                                <center>
                                                                                    <asp:CheckBox runat="server" ID="CheckBox2" CssClass="clsMonthNo" Checked='<%#Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.No"))%>'  Enabled="False"/>
                                                                                </center>
                                                                            </td>
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
                                                    <asp:TextBox ID="ability_TOTAL" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" Enabled="False" ></asp:TextBox>
                                                </div>
                                                <span class="char-limit-msg"></span>
                                            </div>

                                            <div class="span12">
                                                <div class="control-label">
                                                    COMMENTS
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="ability_COMMENTS" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" Enabled="False"></asp:TextBox>
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
                                                            <h5>A bit like your Child 2</h5>
                                                        </td>
                                                        <td>
                                                            <h5>Moderately like your child 3</h5>
                                                        </td>
                                                        <td>
                                                            <h5>Quite a bit like your Child 4</h5>
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
                                                            <h6>Control During Movement</h6>
                                                        </td>
                                                        <td>
                                                            <h6>Fine Motor/Handwriting</h6>
                                                        </td>
                                                        <td>
                                                            <h6>General Coordination</h6>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>1. Throws ball
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Throws3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>2. Catches ball
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Catches3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>3. Hits ball/birdie
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Hits3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>4. Jumps over
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Jumps3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>5. Runs
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Runs3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>6. Plans activity
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Plans3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>7. Writing fast
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Writing3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>8. Writing legibly
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly1" runat="server" Width="200px" CssClass="span1" Enabled="False" ></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_legibly3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>9. Effort and pressure
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Effort3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>10. Cuts
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Cuts3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>11. Likes sports
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes2" runat="server" Width="200px" CssClass="span1"  Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Likes3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>12. Learning new skills
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning1" runat="server" Width="200px" CssClass="span1"  Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Learning3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>13. Quick and competent
                                                        </td>

                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Quick3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>14. “Bull in shop”
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Bull3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>15. Does not fatigue
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does1" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does2" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <asp:TextBox ID="DCDQ_Does3" runat="server" Width="200px" CssClass="span1" Enabled="False"></asp:TextBox></b>
                                                        </td>
                                                    </tr>

                                                </thead>
                                            </table>

                                            <table class="ndt-default-table">
                                                <tr>
                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Control" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/30 +</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Fine" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/20 +</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_General" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/25 =</b></td>

                                                    <td><b>
                                                        <asp:TextBox ID="DCDQ_Total" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>/75</b></td>

                                                </tr>
                                                <tr>
                                                    <td>Control during movement  </td>
                                                    <td>Fine motor and Handwriting </td>
                                                    <td>General Coordination </td>
                                                    <td>Total</td>
                                                </tr>


                                            </table>

                                            <div class="formRow">
                                                <div class="span12">
                                                    <table class="ndt-default-table" >
                                                        <tr>
                                                            <td>
                                                                <h5>Age Group</h5>
                                                            </td>
                                                            <td>
                                                                <h5>Indication of,or Suspect for,DCD</h5>
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
                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="span12">
                                                <div class="control-label">
                                                    COMMENTS :
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="DCDQ_COMMENT" runat="server" CssClass="span10" TextMode="MultiLine"
                                                        Rows="3" Enabled="False"></asp:TextBox>
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
                                                <ajaxToolkit:TabPanel ID="TabPanel15" runat="server" HeaderText="History">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    History :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="SIPTInfo_History" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel16" runat="server" HeaderText="Hand Function-I">
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
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GraspRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GraspLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:Spherical
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:Hook
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_HookRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_HookLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>:3 Jaw Chuck
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Grip
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GripRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_GripLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Release
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseRight" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseLeft" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel13" runat="server" HeaderText="Hand Function-II">
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
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFR" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFR" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Pinch
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfR" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFR" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFR" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFL" runat="server" CssClass="span1" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel14" runat="server" HeaderText="SIPT-III">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT3_Spontaneous" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Reaching > On Command
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT3_Command" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="SIPT-IV">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Kinesthesia" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Finger Identification Test
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Finger" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Localisation Of Tactile Stimuli
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Localisation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Double Tactile Localisation
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_DoubleTactile" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tactile Discrimination
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Tactile" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Graphesthesia
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Graphesthesia" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Post Rotary Nystagmus
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_PostRotary" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Standing And Walking Balance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT4_Standing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="SIPT-V">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Color" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Form Constancy
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Form" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Size Differentiation
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Size" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Depth Perception
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Depth" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Figure Ground Perception
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Figure" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Motor Accuracy
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT5_Motor" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="SIPT-VI">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT6_Design" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Constructional Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT6_Constructional" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="SIPT-VII">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT7_Scanning" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Visual Memory
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT7_Memory" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel9" runat="server" HeaderText="SIPT-VIII">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Postural" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Oral Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Oral" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sequencing Praxis
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Sequencing" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Praxis On Verbal Commands
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT8_Commands" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel10" runat="server" HeaderText="SIPT-IX">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_Bilateral" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Space Visualisation Contralat Use
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_Contralat" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Space Visualisation Preferred Hand
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_PreferredHand" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Crossing Midline
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT9_CrossingMidline" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel11" runat="server" HeaderText="SIPT-X">
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Draw" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Clock Face
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_ClockFace" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Filtering Information
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Filtering" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Motor Planning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_MotorPlanning" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Body Image
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_BodyImage" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Body Schema
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Laterality
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Laterality" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix">
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel12" runat="server" HeaderText="Activity Given">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    Activity Given :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Remark" runat="server" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3" Enabled="False"></asp:TextBox>
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
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Interest In Completion
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Initial Learning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Complexity And Organisation Of Task
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Problem Solving
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Concentration
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Retension And Recall
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Speed Of Perfomance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Activity Neatness
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Frustation Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Work Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Reaction To Authority
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sociability With Therapist
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Sociability With Others Students
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityStudents" runat="server" CssClass="span3" Enabled="False"></asp:TextBox>
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
                                                <ajaxToolkit:TabPanel ID="TabPanel17" runat="server" HeaderText="Strength">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Strengths :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Strengths" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel18" runat="server" HeaderText="Area of Concerns">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Barriers :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Functional Limitations :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Posture and Movement Limitation(Prioritized) :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
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
                                                                        TextMode="MultiLine" Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel113" runat="server" HeaderText="Goals">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Summary :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Previous Long Term Goals :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Previous" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Long Term Goals(Functional Outcome Measured)1 - Year :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_LongTerm" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    4. Short Term Goals(Functional Outcome Measures) 3 - Month :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_ShortTerm" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    5. impairment related Objective goal-3 Months :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Goal_Impairment" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabPanel114" runat="server" HeaderText="Plan Of Care">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    1. Frequency and Duration :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Frequency" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Service Delivery Models :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Service" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Strategies to Address Impairments and Posture Movement Issues Motor Learning
                                                                    :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Strategies" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
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
                                                                        Rows="3" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix">
                                                            </div>
                                                        </div>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    5. Client/Family Education :
                                                                </div>
                                                                <div class="control-group">
                                                                    <asp:TextBox ID="Evaluation_Plan_Education" runat="server" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3" Enabled="False"></asp:TextBox>
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
                                                <asp:TextBox ID="Treatment_Home" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" Enabled="False"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                2. Advice for school
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_School" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" style="background-color: white; color: black;" Enabled="False"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                3. Advice for therapy
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_Threapy" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8" style="background-color: white; color: black;" Enabled="False"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>

                                        <div class="span12">
                                            <div class="control-label">
                                                COMMENTS :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Treatment_cmt" runat="server" CssClass="span10" TextMode="MultiLine" Rows="3" style="background-color: white; color: black;" Enabled="False"></asp:TextBox>
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
                                                <asp:DropDownList ID="Doctor_Physioptherapist" runat="server" CssClass="chzn-select span6" Enabled="False">
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
                                                <asp:DropDownList ID="Doctor_Occupational" runat="server" CssClass="chzn-select span6" Enabled="False">
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

                                    <%--<div class="clearfix">
                                  <asp:Button ID="Button1" CssClass="buttonClass" runat="server" Text="SAVE&NEXT" align="center" Font-Bold="True" OnClick="Button1_Click"  ClientIDMode="Static" width="200px"/>
 
                                          </div>--%>

                </div>
            </div>
            <%--<div class="clearfix"></div>--%>
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
 

    <script type="text/javascript">

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
    </script>

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

///////////////////////////////////////pdf

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
</asp:Content>

