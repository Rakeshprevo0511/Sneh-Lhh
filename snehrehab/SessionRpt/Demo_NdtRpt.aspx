<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_NdtRpt.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_NdtRpt" %>
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
.textbox {    margin-bottom: 5px!important;
    width: 256px!important;}
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                NDT Report :</div>
            <%--<div class="pull-right">
            <a href="/SessionRpt/Demo_NdtView.aspx" class="btn btn-primary">View List</a>
            </div>--%>
        </div>
        <div class="grid-content" style="">
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        Patient Name :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtPatient" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Session :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtSession" runat="server" CssClass="span4" ReadOnly="true"></asp:TextBox>
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
                        <asp:CheckBox ID="txtFinal" runat="server"  Enabled="false"/>
                    </div>
                </div>
                <div class="span6">
                    <label class="control-label">
                        Mark as Report Given :</label>
                    <div class="control-group">
                        <asp:CheckBox ID="txtGiven" runat="server" Enabled="false" />
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
                        <asp:TextBox ID="txtGivenDate" runat="server" CssClass="span2 my-datepicker" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="formRow">
                <div class="span12">
                    <hr />
                </div>
            </div>
            <div class="formRow">
                <div class="span6">
                    <label class="control-label">
                        &nbsp;</label>
                    <div class="control-group">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text=" Submit " OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
                        &nbsp;
                        <%= _printUrl %>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="formRow">
            <div class="span12">
            <hr />
            </div></div>
            <div class="clearfix">
            </div>
            <div class="formRow">
                <div class="span12">
            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server">
                <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="Data Collection">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <asp:Panel ID="PanelDiagnosis" runat="server" CssClass="span11 formRow"> 
                                <div class="row">
                                    <div class="span2">
                                        Diagnosis :</div>
                                    <div class="span4">
                                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple"  Enabled="false" CssClass="chzn-select-multi span4"  data-placeholder="Select Diagnosis"></asp:ListBox>
                                    </div>
                                    <div class="span2">
                                        Other Diagnosis :</div>
                                    <div class="span2">
                                        <asp:TextBox ID="txtDiagnosisOther" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div> 
                            </asp:Panel>
                            <div class="formRow char-line-limiter">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Reason for Referral :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_Referral" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="8"></asp:TextBox>
                                    </div>
                                    <div class="clearfix"></div>
                                    <span class="char-limit-msg"></span>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow char-line-limiter">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Medical History :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_MedicalHistory" runat="server" ReadOnly="true" CssClass="span10"
                                            TextMode="MultiLine" Rows="8"></asp:TextBox>
                                    </div>
                                    <div class="clearfix"></div>
                                    <span class="char-limit-msg"></span>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow char-line-limiter">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Investigation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_Investigation" runat="server" ReadOnly="true" CssClass="span10"
                                            TextMode="MultiLine" Rows="8"></asp:TextBox>
                                    </div>
                                    <div class="clearfix"></div>
                                    <span class="char-limit-msg"></span>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Daily Routine :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_DailyRoutine" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Family Goals/Expectaion :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_Expectaion" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Therapy History :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_TherapyHistory" runat="server" ReadOnly="true" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. Sources at this facility or other :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_Sources" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. Number of visit since last evaluation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_NumberVisit" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        8. Adapted Equipment/Assistive Technology :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_AdaptedEquipment"  ReadOnly="true" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Morphology">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <table class="Morphology-OuterTopTable">
                                        <tr>
                                            <td>
                                                <b>Height</b><br />
                                                <asp:TextBox ID="Morphology_Height" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td rowspan="3">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Limb Length</b>
                                                        </td>
                                                        <td>
                                                            <b>Left</b>
                                                        </td>
                                                        <td>
                                                            <b>Right</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLeft" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbRight" ReadOnly="true"  CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLength" ReadOnly="true" CssClass="span4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <b>Arm Length </b>
                                                        </td>
                                                        <td>
                                                            <b>Left</b>
                                                        </td>
                                                        <td>
                                                            <b>Right</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLeft" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_ArmRight" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLength" ReadOnly="true" CssClass="span4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <b>Head Circumference</b><br />
                                                <asp:TextBox ID="Morphology_Head" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Nipple</b><br />
                                                <asp:TextBox ID="Morphology_Nipple" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Weight </b>
                                                <br />
                                                <asp:TextBox ID="Morphology_Weight" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Waist (Umbilical)</b><br />
                                                <asp:TextBox ID="Morphology_Waist" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                          <%--  <div class="formRow">
                                <div class="span12">
                                    <h5>
                                        Upper Limb :
                                    </h5>
                                    <table class="Morphology-Upper-Limb span10">
                                        <tr>
                                            <td class="span3">
                                                <b>Discription</b>
                                            </td>
                                            <td>
                                                <b>Level>Right</b>
                                            </td>
                                            <td>
                                                <b>Level>Left</b>
                                            </td>
                                            <td>
                                                <b>Girth>Right</b>
                                            </td>
                                            <td>
                                                <b>Girth>Left</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Above Elbow
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelRight_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelLeft_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthRight_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthLeft_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                At Elbow
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelRight_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelLeft_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthRight_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthLeft_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Below Elbow
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelRight_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbLevelLeft_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthRight_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_UpperLimbGirthLeft_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </div>--%>
                            <div class="form-row">
                            <div class="span12">
                            <h5>
                            Girth Upper Limb :
                            </h5>
                            <table class="Morphology-Upper-Limb span10">
                            <tr>
                                <td class="span2">
                                    <b>Discription</b>
                                </td>
                                <td class="span2">
                                    <b>Level</b>
                                </td>
                                <td class="span2">
                                     <b>Left</b>
                                </td>
                                <td class="span2">
                                    <b>Right</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Above Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLevel" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLeft" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowRight" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            </div>
                            </div>
                            <div class="form-row">
                            <div class="span12">
                            <h5>
                            Girth Lower Limb :
                            </h5>
                            <table class="Morphology-Upper-Limb span10">
                            <tr>
                                <td class="span2">
                                    <b>Discription</b>
                                </td>
                                <td class="span2">
                                    <b>Level</b>
                                </td>
                                <td class="span2">
                                     <b>Left</b>
                                </td>
                                <td class="span2">
                                    <b>Right</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Above Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLevel" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLeft" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeRight" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight1" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight2" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight3" runat="server" ReadOnly="true" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            </div>
                            </div>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <h5>
                                        Lower Limb :
                                    </h5>
                                    <table class="Morphology-Lower-Limb span10">
                                        <tr>
                                            <td class="span3">
                                                <b>Discription</b>
                                            </td>
                                            <td>
                                                <b>Level>Right</b>
                                            </td>
                                            <td>
                                                <b>Level>Left</b>
                                            </td>
                                            <td>
                                                <b>Girth>Right</b>
                                            </td>
                                            <td>
                                                <b>Girth>Left</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Above Knee
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelRight_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelLeft_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthRight_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthLeft_ABV" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                At Knee
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelRight_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelLeft_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthRight_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthLeft_AT" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Below Knee
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelRight_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbLevelLeft_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthRight_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Morphology_LowerLimbGirthLeft_BLW" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </div>--%>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        Key Oral Motor Factors :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Morphology_OralMotorFactors" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="Functional Activities">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span11">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <td></td>
                                                <td><b>Functional  Abilities</b><br /><small>(what do you observe the child doing? How much assistance do they need to do these tasks/skills/movements? Keep it pertinent to the current episode of care and discipline)</small></td>
                                                <td><b>Functional Limitations</b></td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><label><b>Gross Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_GrossMotor_Ability" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_GrossMotor_Limit" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Fine Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_FineMotor_Ability" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_FineMotor_Limit" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Communication</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_Communication_Ability" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_Communication_Limit" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Cognition</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_Cognition_Ability" runat="server" class="span5" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_Cognition_Limit" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                           <%-- <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Gross Motor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_GrossMotor" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Hand Function :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_HandFunction" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Fine Motor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_FineMotor" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Communication :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_Communication" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Cognition :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="txtCognition" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. ADL :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_ADL" runat="server" CssClass="span10" ReadOnly="true" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Oral Motor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_OralMotor" runat="server" ReadOnly="true" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel19" runat="server" HeaderText="Participation Activities">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span11">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <td></td>
                                                <td><b>Participation  Abilities</b></td>
                                                <td><b>Participation Limitations</b></td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><label><b>Gross Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_GrossMotor" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_GrossMotor_Limit" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Fine Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_FineMotor" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_FineMotor_Limit" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Communication</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_Communication" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_Communication_Limit" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Cognition</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_Cognition" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_Cognition_Limit" runat="server" ReadOnly="true" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="span11">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <td colspan="4"><h5>Contextual factors :</h5> </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="2"><label><b>Personal : </b></label></td> 
                                                <td colspan="2"><label><b>Environmental : </b></label></td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Positive : </b></label></td>
                                                <td>
                                                    <asp:TextBox ID="Contextual_Personal_Positive" runat="server" ReadOnly="true" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                 <td><label><b>Positive : </b></label></td>
                                                 <td>
                                                    <asp:TextBox ID="Contextual_Enviremental_Positive" runat="server" ReadOnly="true" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                
                                            </tr> 
                                            <tr>
                                               <td><label><b>Negative : </b></label></td>
                                                <td>
                                                <asp:TextBox ID="Contextual_Personal_Negative" runat="server" ReadOnly="true" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                                <td><label><b>Negative : </b></label></td>
                                                <td>
                                                 <asp:TextBox ID="Contextual_Enviremental_Negative" runat="server" ReadOnly="true" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>  
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="Test And Measures">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. GMFCS :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_GMFCS" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. GMFM :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_GMFM" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. GMPM :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_GMPM" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow" style="display:none;">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Ashworth's Scale :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_AshworthScale" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow" style="display:none;">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Tradieus Scale :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_TradieusScale" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow" style="display:none;">
                                <div class="span12">
                                    <div class="control-label">
                                        6. OGS :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_OGS" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. MELBOURNE :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_Melbourne" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. COPM :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_COPM" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Clinical Observation Of Patient During Free Play :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_ClinicalObservation" ReadOnly="true" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. Others :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_Others" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report5" runat="server" HeaderText="Posture">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Alignment :</div>
                                    <div class="span5">
                                        <asp:CheckBox ID="Posture_Alignment_Type_1" runat="server" onclick="Posture_Alignment_Type_1_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Symmetrical" />
                                        <asp:CheckBox ID="Posture_Alignment_Type_2" runat="server" onclick="Posture_Alignment_Type_2_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Asymmetrical" />
                                        <script type="text/javascript">
                                            function Posture_Alignment_Type_1_Click() {
                                                var ctl = $('#<%=Posture_Alignment_Type_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Alignment_Type_2.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Posture_Alignment_Type_2_Click() {
                                                var ctl = $('#<%=Posture_Alignment_Type_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Alignment_Type_1.ClientID %>').prop('checked', false);
                                                }
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>
                                            General Posture Comments :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        1) Head :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Head" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        2) Shoulder :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Shoulder" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        3) Ribcage :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Ribcage" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        4) Trunk :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Trunk" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        5) Pelvis :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Pelvis" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        6) Hips :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Hips" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        7) Knees :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Knees" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        8) Ankle/Feet :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Gen_Ankle_Feet" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>
                                            Oral Structures Alignment :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        1) Neck :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Neck" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        2) Jaw :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Jaw" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        3) Lips :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Lips" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        4) Teeth :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Teeth" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        5) Tounue :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Tounge" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Palate :</div>
                                    <div class="span8">
                                        <asp:CheckBox ID="Posture_Stru_Palate_1" runat="server" onclick="Posture_Stru_Palate_1_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" High" />
                                        <asp:CheckBox ID="Posture_Stru_Palate_2" runat="server" onclick="Posture_Stru_Palate_2_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Shallow" />
                                        <asp:CheckBox ID="Posture_Stru_Palate_3" runat="server" onclick="Posture_Stru_Palate_3_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Normal" />
                                        <script type="text/javascript">
                                            function Posture_Stru_Palate_1_Click() {
                                                var ctl = $('#<%=Posture_Stru_Palate_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Stru_Palate_2.ClientID %>').prop('checked', false);
                                                    $('#<%=Posture_Stru_Palate_3.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Posture_Stru_Palate_2_Click() {
                                                var ctl = $('#<%=Posture_Stru_Palate_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Stru_Palate_1.ClientID %>').prop('checked', false);
                                                    $('#<%=Posture_Stru_Palate_3.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Posture_Stru_Palate_3_Click() {
                                                var ctl = $('#<%=Posture_Stru_Palate_3.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Stru_Palate_1.ClientID %>').prop('checked', false);
                                                    $('#<%=Posture_Stru_Palate_2.ClientID %>').prop('checked', false);
                                                }
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Mouth Posture :</div>
                                    <div class="span5">
                                        <asp:CheckBox ID="Posture_Stru_MouthPosture_1" runat="server" onclick="Posture_Stru_MouthPosture_1_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Open" />
                                        <asp:CheckBox ID="Posture_Stru_MouthPosture_2" runat="server" onclick="Posture_Stru_MouthPosture_2_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Closed" />
                                        <script type="text/javascript">
                                            function Posture_Stru_MouthPosture_1_Click() {
                                                var ctl = $('#<%=Posture_Stru_MouthPosture_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Stru_MouthPosture_2.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Posture_Stru_MouthPosture_2_Click() {
                                                var ctl = $('#<%=Posture_Stru_MouthPosture_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Posture_Stru_MouthPosture_1.ClientID %>').prop('checked', false);
                                                }
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Tongue Movements :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_ToungueMove" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Bite :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Bite" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Swallow :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Swallow" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Chew :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Chew" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Suck :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Suck" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>


                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <h5>2) Base Support :</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_BaseSupport" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>3) Center Of Mass </h5>
                                        Transition of COM over BOS :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_CenterOfMass" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <h5>4) Strategies for Stability :</h5></div>
                                    <div class="span8">
                                        <asp:CheckBoxList ID="Posture_Stru_StrategyForStability" runat="server" CssClass="checkbox span8" SelectionMode="Multiple" Enabled="false">
                                            <asp:ListItem Value="Increased Postural Tone">Increased Postural Tone</asp:ListItem>
                                            <asp:ListItem Value="Increased BOS">Increased BOS</asp:ListItem>
                                            <asp:ListItem Value="Locking Of Joint">Locking Of Joint</asp:ListItem>
                                            <asp:ListItem Value="Gaze Fixation">Gaze Fixation</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <h5>5) Anticipatory Control :</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_Anticipatory" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <h5>6) Postural Counter Balance :</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Stru_CounterBalance" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>
                                            7) Sign Of Postural Impairments :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Muscle Architecture :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Impairment_Muscle" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Atrophy :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Impairment_Atrophy" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Hypertrophy :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Impairment_Hypertrophy" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Callosities :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_Impairment_Callosities" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <h5>8) General Posture :</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="Posture_GeneralPosture" runat="server" CssClass="span8"  ReadOnly="true" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                             


                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Alignment(Head/Neck,Spine,Shoulder,Girdle,UE's,LE's) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Alignment" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. BOS/COM(Biomechanics) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Biomechanics" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Strategies for Stability(Posture tone,muscule synergies or biomechanical strategies
                                        utilized) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Stability" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Anticipatory Control Set For Movement :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Anticipatory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Postural Counter Balance During Movement :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Postural" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Signs of Postural System Impairments(Muscular Architecture,General Posture) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_SignsPostural" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report6" runat="server" HeaderText="Movement">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. How does child overcome inertia? :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Inertia" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Type of Movements :</div>
                                    <div class="span5">
                                        <asp:CheckBox ID="Movement_TypeOf_1" runat="server" onclick="Movement_TypeOf_1_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Ramp" />
                                        <asp:CheckBox ID="Movement_TypeOf_2" runat="server" onclick="Movement_TypeOf_2_Click()" Enabled="false"
                                            CssClass="checkboes" Text=" Ballistic" />
                                        <script type="text/javascript">
                                            function Movement_TypeOf_1_Click() {
                                                var ctl = $('#<%=Movement_TypeOf_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Movement_TypeOf_2.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Movement_TypeOf_2_Click() {
                                                var ctl = $('#<%=Movement_TypeOf_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Movement_TypeOf_1.ClientID %>').prop('checked', false);
                                                }
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Plane of Movements :</div>
                                        <div class="span8">
                                            <div class="row">
                                                <div class="span1">
                                                    <asp:Label ID=lblSagittal runat="server" Text="Sagittal"></asp:Label></div>
                                                    <div class="span2" style = "margin-left: 0;">
                                                        <asp:TextBox ID="Movement_Sagittal" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                </div>
                                                <div class="span1" style = "margin-left: 0;">
                                                    <asp:Label ID=Label1 runat="server" Text="Coronal"></asp:Label></div>
                                                    <div class="span2" style = "margin-left: 0;">
                                                        <asp:TextBox ID="Movement_Coronal" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                </div>
                                                <div class="span1" style = "margin-left: 0;">
                                                    <asp:Label ID=Label2 runat="server" Text="Transverse"></asp:Label></div>
                                                    <div class="span2">
                                                        <asp:TextBox ID="Movement_Transverse" runat="server" ReadOnly="true" CssClass="span2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                               <%--     <div class="span8">
                                        <asp:CheckBox ID="Movement_Plane_1" runat="server" onclick="Movement_Plane_1_Click()"
                                            CssClass="checkboes" Text=" Sagittal" />
                                        <asp:CheckBox ID="Movement_Plane_2" runat="server" onclick="Movement_Plane_2_Click()"
                                            CssClass="checkboes" Text=" Coronal" />
                                        <asp:CheckBox ID="Movement_Plane_3" runat="server" onclick="Movement_Plane_3_Click()"
                                            CssClass="checkboes" Text=" Transverse" />
                                        <script type="text/javascript">
                                            function Movement_Plane_1_Click() {
                                                var ctl = $('#<%=Movement_Plane_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Movement_Plane_2.ClientID %>').prop('checked', false);
                                                    $('#<%=Movement_Plane_3.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Movement_Plane_2_Click() {
                                                var ctl = $('#<%=Movement_Plane_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Movement_Plane_1.ClientID %>').prop('checked', false);
                                                    $('#<%=Movement_Plane_3.ClientID %>').prop('checked', false);
                                                }
                                            }
                                            function Movement_Plane_3_Click() {
                                                var ctl = $('#<%=Movement_Plane_3.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Movement_Plane_2.ClientID %>').prop('checked', false);
                                                    $('#<%=Movement_Plane_1.ClientID %>').prop('checked', false);
                                                }
                                            }
                                        </script>
                                    </div>--%>

                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Weight Shifts :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_WeightShift" runat="server" CssClass="span8" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Intra & Inter Limb Dessociation :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_LimbDissociation" runat="server" CssClass="span8" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Range & Speed of Movements :</div>
                                    <%--<div class="span8">
                                        <asp:CheckBoxList ID="Movement_RangeSpeed" runat="server" CssClass="checkboes">
                                            <asp:ListItem Value="Truncal">Truncal</asp:ListItem>
                                            <asp:ListItem Value="U. E.">U. E.</asp:ListItem>
                                            <asp:ListItem Value="L. E.">L. E.</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>--%>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_RangeSpeedOfMovements" runat="server" CssClass="span8" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Strategies For Stability & Mobility :</div>
                                    <div class="span8">
                                        <asp:CheckBoxList ID="Movement_Stability" runat="server" CssClass="checkboes" Enabled="false">
                                            <asp:ListItem Value="Overuse of momentum">Overuse of momentum</asp:ListItem>
                                            <asp:ListItem Value="Increased BOS">Increased BOS</asp:ListItem>
                                            <asp:ListItem Value="Increasing postural tone">Increasing postural tone</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Sign of Movement System Impairment or Overuse :</div>
                                    <div class="span8">
                                        <asp:CheckBoxList ID="Movement_Overuse" runat="server" CssClass="checkboes" Enabled="false">
                                            <asp:ListItem Value="Lean Muscle">Lean Muscle</asp:ListItem>
                                            <asp:ListItem Value="Locking of Joints">Locking of Joints</asp:ListItem>
                                            <asp:ListItem Value="Board BOS">Board BOS</asp:ListItem>
                                            <asp:ListItem Value="General Posture">General Posture</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Balance :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        While maintaining a posture :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_Balance_Maintain" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        During Transitions :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_Balance_During" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Accuracy of Movement :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        For Upper extremity functions :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_Accuracy_Upper" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        For Lower extremity functions :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="Movement_Accuracy_Lower" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Movement Strategies :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Strategies" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Discuss range,speed of movement,Consider both trunk and extremities :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Extremities" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Strategies for stability, Mobility :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Stability" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Sign of movement system impairment or overuse :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Overuse" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel20" runat="server" HeaderText="Neuromoter System">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Neuromoter System :</h5>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Components</th>
                                            <th>Initiation</th>
                                            <th>Sustainance</th>
                                            <th>Termination</th>
                                            <th>Control & Gradation</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Recruitment<br />Movement<br />Postrual</td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Type of contraction<br />Concentric<br />Isometric<br />Eccentric</td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Co-activation / Reciprocal inhibition</td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Type of Synergy</td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Stiffness(Static/Dynamic)</td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                        <tr>                                     
                                            <td>Extraneous Movement</td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Initial" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Sustainance" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Termination" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Control" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox></td>
                                        </tr>
                                    </tbody>
                                </table> 
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Other Tests :</h5>
                                        <b>Tardieus</b>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th> </th>
                                            <th>Right</th>
                                            <th>Left</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>TA</td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_TA_Right" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_TA_Left" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td> 
                                        </tr>
                                        <tr>
                                            <td>Hamstrings</td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Hamstring_Right" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Hamstring_Left" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td> 
                                        </tr>
                                        <tr>
                                            <td>Adductors</td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Adductor_Right" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Adductor_Left" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td> 
                                        </tr>
                                        <tr>
                                            <td>Hip Flexor Angle</td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Hip_Right" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Hip_Left" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td> 
                                        </tr>
                                        <tr>
                                            <td>Biceps</td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Biceps_Right" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td>
                                            <td><asp:TextBox ID="OtherTest_Tardieus_Biceps_Left" runat="server" ReadOnly="true" CssClass="span5"></asp:TextBox></td> 
                                        </tr>
                                    </tbody>
                                </table>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Selective Motor Control :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Muscle  
                                            </th>
                                            <th>
                                                Right
                                            </th>
                                            <th>
                                                Left
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="txtSelectionMotorControl_Muscle" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="txtSelectionMotorControl_ID" runat="server" Value='<%#Eval("SR_NO") %>' />
                                                        <asp:TextBox ID="SelectionMotorControl_Muscle" runat="server" ReadOnly="true" CssClass="span4" Text='<%#Eval("MUSCLE") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_Right" runat="server" CssClass="span4" ReadOnly="true" Text='<%#Eval("RIGHT") %>'></asp:TextBox></td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_Left" runat="server" ReadOnly="true" CssClass="span4" Text='<%#Eval("LEFT") %>'></asp:TextBox></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>Denvers :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Gross Motor :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Gross" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Fine Motor :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Fine" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Communication :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Communication" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Cognition :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Cognition" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>  
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <b>GMFM score :</b></div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_GMFM" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>MAS :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Muscle  
                                            </th>
                                            <th>
                                                MAS
                                            </th> 
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="txtSelectionMotorControl_MAS" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="txtSelectionMotorControl_MAS_ID" runat="server" Value='<%#Eval("SR_NO") %>' />
                                                        <asp:TextBox ID="SelectionMotorControl_Muscle" runat="server" ReadOnly="true" CssClass="span6" Text='<%#Eval("MUSCLE") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_MAS" runat="server" ReadOnly="true" CssClass="span6" Text='<%#Eval("MAS") %>'></asp:TextBox></td>                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Observational Gait Analysis Scale :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Observation" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5>The four A’s :</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Arousal :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Arousal" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Attention :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Attention" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Affect :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Affect" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Action :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Action" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        State Regulation :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_StateRegulation" runat="server" ReadOnly="true" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report7" runat="server" HeaderText="Others">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Integration of Posture and Movement(Balance,Transition,Accuracy and Efficency)
                                        :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Others_Integration" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Special Assessments and Gait :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Others_Assessments" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
               <%-- <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="Regulatory">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Arousal :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Regulatory_Arousal" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. State Regulation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Regulatory_Regulation" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>--%>
                <ajaxToolkit:TabPanel ID="tb_Report9" runat="server" HeaderText="Musculoskeletal">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <h5>
                                        Musculoskeletal :</h5>
                                    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
                                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="ROM-1">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <table class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">&nbsp;
                                                                </td>
                                                                <td>
                                                                    <b>Left</b>
                                                                </td>
                                                                <td>
                                                                    <b>Right</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Popliteal Angle
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionRight" runat="server" ReadOnly="true"
                                                                        CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Plantar Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="ROM-2">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                               <td class="span3">&nbsp;
                                                                </td>
                                                                <td>
                                                                    <b>Left</b>
                                                                </td>
                                                                <td>
                                                                    <b>Right</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Shoulder Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Shoulder Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Horizontal Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionLeft" runat="server"  ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationLeft" runat="server"  ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationLeft" runat="server"  ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Extesion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Strength">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            1. lp(In pattern) :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Musculoskeletal_Strengthlp" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            2. CC(Consious Control) :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Musculoskeletal_StrengthCC" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            3. Muscle Endurance :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Musculoskeletal_StrengthMuscle" runat="server" CssClass="span10" ReadOnly="true"
                                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            4. Skeletal Comments :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Musculoskeletal_StrengthSkeletal" runat="server" CssClass="span10" ReadOnly="true"
                                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="MMT">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Muscle</b>
                                                                </td>
                                                                <td>
                                                                    <b>Left</b>
                                                                </td>
                                                                <td>
                                                                    <b>Right</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip flexors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hams
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Quads
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Posterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Peronei
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Anterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Posterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Middle Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supraspinatus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Serratus Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Rhomboids
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Biceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Triceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supinator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECULeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECURight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSLeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSRight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCULeft" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCURight" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Opponens Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisLeft" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisRight" ReadOnly="true" runat="server" CssClass="span3"></asp:TextBox>
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
                <%--<ajaxToolkit:TabPanel ID="tb_Report10" runat="server" HeaderText="Sign of CNS">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Neuromotor Control and Coordination Sign of CNS integrity/impairment(DTR's,Spasticity,Ashworth
                                        Measure) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SignOfCNS_NeuromotorControl" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>--%>
                <ajaxToolkit:TabPanel ID="tb_Report11" runat="server" HeaderText="Remarks Variable">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <table  class="ndt-default-table">
                                        <thead>
                                            <tr>
                                                <th>
                                                    Variables
                                                </th>
                                                <th>
                                                    General Comments
                                                </th>
                                                <th>
                                                    Control and Gradation
                                                </th>
                                                <th>
                                                    Co-ordination and Timing
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Ability to initate sustain,to initate sustain,and terminate muscle activity.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainGeneral" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainControl" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainTiming" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Recruitment of postural(SO)and phasic or movement(FF) motor unit.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalGeneral" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalControl" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalTiming" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Ability to perform concentric, isometric, and eccentric muscle contractions.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsGeneral" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsControl" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsTiming" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Recruitment of cocontraction and/or reciprocal inhibition of agonist and antagonist.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistGeneral" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistControl" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistTiming" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox5" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Synergy selectivity( mas vs.isolated,repertoire).</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyGeneral" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyControl" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyTiming" runat="server" CssClass="span3" TextMode="MultiLine" ReadOnly="true"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox6" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Stiffiness(delta F/delta L) .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessGeneral" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessControl" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessTiming" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox7" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3" 
                                                        ReadOnly="true">Extraneous movements (tremors,clonus,nystagmus, etc .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousGeneral" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousControl" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousTiming" runat="server" CssClass="span3" ReadOnly="true"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report12" runat="server" HeaderText="Sensory System">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Vision :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Vision" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Somatosensory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Somatosensory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                           <%-- <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Vestibular :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Vestibular" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Auditory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Auditory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <%--<div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Gustatory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Gustatory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>--%>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Auditory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Auditory" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Propioceptive :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Propioceptive" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Oromotor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Oromotpor" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Vestibular :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Vestibular" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Tactile :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Tactile" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. Olfactory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Olfactory" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report13" runat="server" HeaderText="Sensory Profile">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        Sensory Profile :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensoryProfile_Profile" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                <table class="ndt-default-table"> 
                                    <tbody>
                                        <asp:Repeater ID="txtSensory_Profile_NameResults" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="txtSensory_Profile_NameResults_ID" runat="server" Value='<%#Eval("SR_NO") %>' />
                                                        <asp:TextBox ID="txtSensory_Profile_NameResults_Name" runat="server" ReadOnly="true" CssClass="span4" Text='<%#Eval("NAME") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="txtSensory_Profile_NameResults_Result" runat="server" ReadOnly="true" CssClass="span4" Text='<%#Eval("RESULTS") %>'></asp:TextBox></td>                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report14" runat="server" HeaderText="SIPT Information">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <h5>
                                        SIPT Information :</h5>
                                    <ajaxToolkit:TabContainer ID="TabContainer2" runat="server">
                                        <ajaxToolkit:TabPanel ID="TabPanel15" runat="server" HeaderText="History">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            History :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="SIPTInfo_History" runat="server" ReadOnly="true" CssClass="span10" TextMode="MultiLine"
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
                                        <ajaxToolkit:TabPanel ID="TabPanel16" runat="server" HeaderText="Hand Function-I">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
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
                                                                <td>
                                                                    Grasp : Cylindrical
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Spherical
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Hook
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :3 Jaw Chuck
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Grip
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Release
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseRight" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseLeft" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
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
                                                                <td>
                                                                    Opposition
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pinch
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFR" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFL" runat="server" ReadOnly="true" CssClass="span1"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaching > Spontaneous
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT3_Spontaneous" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaching > On Command
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT3_Command" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Kinesthesia
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Kinesthesia" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Finger Identification Test
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Finger" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Localisation Of Tactile Stimuli
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Localisation" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Double Tactile Localisation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_DoubleTactile" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tactile Discrimination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Tactile" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Graphesthesia
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Graphesthesia" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Post Rotary Nystagmus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_PostRotary" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Standing And Walking Balance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Standing" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Color Recognition
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Color" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Form Constancy
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Form" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Size Differentiation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Size" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Depth Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Depth" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Figure Ground Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Figure" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Accuracy
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Motor" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Design Copying
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT6_Design" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Constructional Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT6_Constructional" runat="server" CssClass="span3" ReadOnly="true"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Visual Scanning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT7_Scanning" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Visual Memory
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT7_Memory" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Postural Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Postural" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Oral Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Oral" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sequencing Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Sequencing" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Praxis On Verbal Commands
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Commands" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Bilateral Motor Co-ordination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Bilateral" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Contralat Use
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Contralat" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Preferred Hand
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_PreferredHand" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Crossing Midline
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_CrossingMidline" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td class="span3">
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Draw A Person Test
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Draw" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Clock Face
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_ClockFace" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Filtering Information
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Filtering" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Planning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_MotorPlanning" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Image
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodyImage" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Schema
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server"  ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Laterality
                                                                </td>
                                                                <td> 
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Laterality" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            Activity Given :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Remark" runat="server" CssClass="span10" ReadOnly="true"
                                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <table  class="ndt-default-table">
                                                            <tr>
                                                                <td>
                                                                    <b>Parameter</b>
                                                                </td>
                                                                <td>
                                                                    <b>Value</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Interest In Activity
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Interest In Completion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Initial Learning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Complexity And Organisation Of Task
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Problem Solving
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Concentration
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Retension And Recall
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Speed Of Perfomance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Activity Neatness
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Frustation Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Work Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaction To Authority
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Therapist
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Others Students
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityStudents" runat="server" ReadOnly="true" CssClass="span3"></asp:TextBox>
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
                <ajaxToolkit:TabPanel ID="tb_Report15" runat="server" HeaderText="Cognition">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Intelligence :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_Intelligence" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Attention :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_Attention" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Memory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_Memory" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Adaptibility :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_Adaptibility" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Motor Planning :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_MotorPlanning" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Executive Function :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_ExecutiveFunction" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. Age Appropriate Cognitive Functions :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_CognitiveFunctions" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report16" runat="server" HeaderText="Integumentary">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Skin Integrity :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Integumentary_SkinIntegrity" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Skin Color :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Integumentary_SkinColor" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Skin Extensibility :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Integumentary_SkinExtensibility" runat="server" CssClass="span10" ReadOnly="true"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report17" runat="server" HeaderText="Respiratory">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Rate-resting :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Respiratory_RateResting" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Post Exercise :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Respiratory_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Patterns :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Respiratory_Patterns" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Breath Control Capacity :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Respiratory_BreathControl" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Chest Excursion :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Respiratory_ChestExcursion" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report18" runat="server" HeaderText="Cardiovascular">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Heart Rate-Resting :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_HeartRate" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Post Exercise :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. BP :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_BP" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Edema :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_Edema" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Peripheral Circulation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_Circulation" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. EEi :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cardiovascular_EEi" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report19" runat="server" HeaderText="Gastrointestinal">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Bowel/Blader :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Gastrointestinal_Bowel" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Intake and Tolerance :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Gastrointestinal_Intake" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report20" runat="server" HeaderText="Evaluation">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <h5>
                                        Evaluation :</h5>
                                    <ajaxToolkit:TabContainer ID="TabContainer3" runat="server">
                                        <ajaxToolkit:TabPanel ID="TabPanel17" runat="server" HeaderText="Strength">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            1. Strengths :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Strengths" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            1. Barriers :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            2. Functional Limitations :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" CssClass="span10" ReadOnly="true"
                                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            3. Posture and Movement Limitation(Prioritized) :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            4. Impairment(Prioritized) :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Concern_Impairment" runat="server" CssClass="span10" ReadOnly="true"
                                                                TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            1. Summary :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            2. Previous Long Term Goals :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_Previous" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            3. Long Term Goals(Functional Outcome Measured)1 - Year :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_LongTerm" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            4. Short Term Goals(Functional Outcome Measures) 3 - Month :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_ShortTerm" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            5. impairment related Objective goal-3 Months :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_Impairment" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
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
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            1. Frequency and Duration :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Plan_Frequency" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            2. Service Delivery Models :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Plan_Service" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            3. Strategies to Address Impairments and Posture Movement Issues Motor Learning
                                                            :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Plan_Strategies" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            4. Equipment/Adjuncts :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Plan_Equipment" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            5. Client/Family Education :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Plan_Education" runat="server" CssClass="span10" TextMode="MultiLine" ReadOnly="true"
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
                <ajaxToolkit:TabPanel ID="tb_Report21" runat="server" HeaderText="Doctor">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Physioptherapist :
                                    </div>
                                    <div class="control-group">
                                        <asp:DropDownList ID="Doctor_Physioptherapist" runat="server" CssClass="chzn-select span6" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Occupational Therapist :
                                    </div>
                                    <div class="control-group">
                                        <asp:DropDownList ID="Doctor_Occupational" runat="server" CssClass="chzn-select span6" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Name of Director :
                                    </div>
                                    <div class="control-group">
                                        <asp:DropDownList ID="Doctor_EnterReport" runat="server" CssClass="chzn-select span6"  Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
            </div>
            <div class="clearfix">
            </div>
            </div>
        </div>
    </div>
</asp:Content>
