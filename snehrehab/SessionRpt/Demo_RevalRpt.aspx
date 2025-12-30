<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_RevalRpt.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_RevalRpt" %>
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
               Re-Eval Report :</div>
            <%--<div class="pull-right">
            <a href="/SessionRpt/Demo_RevalView.aspx" class="btn btn-primary">View List</a>
            </div>--%>
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
                        <asp:CheckBox ID="txtFinal" runat="server" Enabled="false"  />
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
                        <asp:TextBox ID="txtGivenDate" runat="server" CssClass="span2 my-datepicker" Enabled="false" ></asp:TextBox>
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
                                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" Enabled="false" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                                    </div>
                                    <div class="span2">
                                        Other Diagnosis :</div>
                                    <div class="span2">
                                        <asp:TextBox ID="txtDiagnosisOther" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div> 
                            </asp:Panel>
                            <div class="formRow char-line-limiter">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Current Concern :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_CurrentConcern" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        2. Improvements Since Last Eval :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_ImprovementsSinceLastEval" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        3. Medical History :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_MedicalHistory" runat="server" Enabled="false" CssClass="span10"
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
                                        <asp:TextBox ID="DataCollection_DailyRoutine" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_Expectaion" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_TherapyHistory" runat="server" Enabled="false" CssClass="span10"
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
                                        <asp:TextBox ID="DataCollection_Sources" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        8. Number of visit since last evaluation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_NumberVisit" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        9. Adapted Equipment/Assistive Technology :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_AdaptedEquipment" runat="server" Enabled="false" CssClass="span10"
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
                                                <asp:TextBox ID="Morphology_Height" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
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
                                                        <b>APP Limb Length</b>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLeft" TextMode=MultiLine Enabled="false" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbRight" TextMode="MultiLine" Enabled="false" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <b>True Limb Length</b>
                                                    </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_TrueLimbLengthLeft" TextMode="MultiLine" Enabled="false" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                           <asp:TextBox runat="server" ID="Morphology_TrueLimbLengthRight" TextMode="MultiLine" Enabled="false" CssClass="span2"></asp:TextBox>
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
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLeft" Enabled="false" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_ArmRight" Enabled="false" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLength" Enabled="false" CssClass="span4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <b>Head Circumference</b><br />
                                                <asp:TextBox ID="Morphology_Head" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Nipple</b><br />
                                                <asp:TextBox ID="Morphology_Nipple" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Weight </b>
                                                <br />
                                                <asp:TextBox ID="Morphology_Weight" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Waist (Umbilical)</b><br />
                                                <asp:TextBox ID="Morphology_Waist" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                           <%-- <div class="formRow">
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
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLevel" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLeft" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowRight" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            </div>
                            </div>
                    <%--        <div class="formRow">
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
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLevel" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLeft" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeRight" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight1" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight2" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight3" runat="server" Enabled="false" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        Key Oral Motor Factors :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Morphology_OralMotorFactors" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                <div class="span12">
                                    <div class="control-label">
                                        1. Gross Motor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_GrossMotor" runat="server" Enabled="false" CssClass="span10"
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
                                        <asp:TextBox ID="FunctionalActivities_HandFunction" runat="server" Enabled="false" CssClass="span10"
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
                                        <asp:TextBox ID="FunctionalActivities_FineMotor" runat="server" Enabled="false" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. ADL :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_ADL" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Oral Motor :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_OralMotor" runat="server" Enabled="false" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Communication :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_Communication" Enabled="false" runat="server" CssClass="span10"
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
                <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="Test And Measures">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. GMFCS :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_GMFCS" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_GMFM" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_GMPM" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Ashworth's Scale :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_AshworthScale" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Tradieus Scale :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_TradieusScale" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. OGS :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_OGS" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        7. MELBOURNE :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_Melbourne" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        8. COPM :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_COPM" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        9. Clinical Observation Of Patient During Free Play :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_ClinicalObservation" runat="server" Enabled="false" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        10. Others :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="TestMeasures_Others" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Alignment(Head/Neck,Spine,Shoulder,Girdle,UE's,LE's) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_Alignment" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Posture_Biomechanics" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Posture_Stability" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Posture_Anticipatory" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Posture_Postural" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        6. Signs of Postural System Impairments(Muscular Architecture,General Posture) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Posture_SignsPostural" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                <ajaxToolkit:TabPanel ID="tb_Report6" runat="server" HeaderText="Movement">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. How does child overcome inertia? :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Inertia" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Movement Strategies :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Strategies" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Movement_Extremities" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Strategies for stability, Mobility :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Movement_Stability" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Movement_Overuse" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Others_Integration" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Others_Assessments" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="System Exmination">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                           
                             <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. State Regulation :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Regulatory_Regulation" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                             <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Arousal :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Regulatory_Arousal" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                             <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Attention :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Attention" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                             <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                       4. Affect :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Affect" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                             <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Action :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Action" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Popliteal Angle
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionRight" runat="server" Enabled="false" 
                                                                        CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Plantar Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Shoulder Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Horizontal Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Extesion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                            <asp:TextBox ID="Musculoskeletal_Strengthlp" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthCC" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthMuscle" runat="server" Enabled="false" CssClass="span10"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthSkeletal" runat="server" Enabled="false" CssClass="span10"
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
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hams
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Quads
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Posterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Peronei
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Anterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Posterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Middle Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supraspinatus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Serratus Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Rhomboids
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Biceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Triceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supinator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorRight" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECULeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECURight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSRight" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCULeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCURight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Opponens Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                <ajaxToolkit:TabPanel ID="tb_Report10" runat="server" HeaderText="Sign of CNS">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Neuromotor Control and Coordination Sign of CNS integrity/impairment(DTR's,Spasticity,Ashworth
                                        Measure) :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SignOfCNS_NeuromotorControl" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                    <asp:TextBox ID="TextBox1" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Ability to initate sustain,to initate sustain,and terminate muscle activity.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainGeneral" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainControl" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainTiming" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox2" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Recruitment of postural(SO)and phasic or movement(FF) motor unit.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalGeneral" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalControl" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalTiming" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox3" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Ability to perform concentric, isometric, and eccentric muscle contractions.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsGeneral" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsControl" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsTiming" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox4" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Recruitment of cocontraction and/or reciprocal inhibition of agonist and antagonist.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistGeneral" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistControl" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistTiming" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox5" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Synergy selectivity( mas vs.isolated,repertoire).</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyGeneral" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyControl" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyTiming" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox6" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Stiffiness(delta F/delta L) .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessGeneral" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessControl" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessTiming" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox7" runat="server" Enabled="false" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Extraneous movements (tremors,clonus,nystagmus, etc .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousGeneral" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousControl" runat="server" Enabled="false" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousTiming" runat="server" Enabled="false" CssClass="span3"
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
                                        <asp:TextBox ID="SensorySystem_Vision" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Somatosensory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Somatosensory" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        3. Vestibular :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Vestibular" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        4. Auditory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Auditory" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        5. Gustatory :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Gustatory" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensoryProfile_Profile" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                            Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                <table class="reval-default-table"> 
                                    <tbody>
                                        <asp:Repeater ID="txtSensory_Profile_NameResults" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="txtSensory_Profile_NameResults_ID" runat="server" Value='<%#Eval("SR_NO") %>' />
                                                        <asp:TextBox ID="txtSensory_Profile_NameResults_Name" runat="server" Enabled="false" CssClass="span4" Text='<%#Eval("NAME") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="txtSensory_Profile_NameResults_Result" runat="server" Enabled="false" CssClass="span4" Text='<%#Eval("RESULTS") %>'></asp:TextBox></td>                                                    
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
                                                            <asp:TextBox ID="SIPTInfo_History" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Spherical
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Hook
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :3 Jaw Chuck
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Grip
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Release
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_ReleaseLeft" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionLfL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionRFL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pinch
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchLfL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchMFL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFR" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction2_PinchRFL" runat="server" Enabled="false" CssClass="span1"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT3_Spontaneous" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaching > On Command
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT3_Command" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Kinesthesia" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Finger Identification Test
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Finger" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Localisation Of Tactile Stimuli
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Localisation" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Double Tactile Localisation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_DoubleTactile" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tactile Discrimination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Tactile" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Graphesthesia
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Graphesthesia" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Post Rotary Nystagmus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_PostRotary" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Standing And Walking Balance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Standing" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Color" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Form Constancy
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Form" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Size Differentiation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Size" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Depth Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Depth" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Figure Ground Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Figure" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Accuracy
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Motor" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT6_Design" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Constructional Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT6_Constructional" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT7_Scanning" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Visual Memory
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT7_Memory" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Postural" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Oral Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Oral" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sequencing Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Sequencing" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Praxis On Verbal Commands
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Commands" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Bilateral" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Contralat Use
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Contralat" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Preferred Hand
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_PreferredHand" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Crossing Midline
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_CrossingMidline" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Draw" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Clock Face
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_ClockFace" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Filtering Information
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Filtering" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Planning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_MotorPlanning" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Image
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodyImage" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Schema
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Laterality
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Laterality" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Remark" runat="server" Enabled="false" CssClass="span10"
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
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Interest In Completion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Initial Learning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Complexity And Organisation Of Task
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Problem Solving
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Concentration
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Retension And Recall
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Speed Of Perfomance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Activity Neatness
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Frustation Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Work Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaction To Authority
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Therapist
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Others Students
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityStudents" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                        <asp:TextBox ID="Cognition_Intelligence" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Attention" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Memory" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Adaptibility" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_MotorPlanning" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_ExecutiveFunction" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_CognitiveFunctions" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinIntegrity" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinColor" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinExtensibility" runat="server" Enabled="false" CssClass="span10"
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
                                        <asp:TextBox ID="Respiratory_RateResting" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_PostExercise" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_Patterns" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_BreathControl" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_HeartRate" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_PostExercise" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_BP" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_Edema" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_Circulation" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_EEi" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Gastrointestinal_Bowel" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Gastrointestinal_Intake" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Strengths" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                
                                                <div class="formRow">-
                                                    <div class="span12">
                                                        <div class="control-label">
                                                           2. Area of Improvement  :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evalutionadding_Strengths" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" Enabled="false" CssClass="span10"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Impairment" runat="server" Enabled="false" CssClass="span10"
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
                                                            <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                 <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            2. Previous Short Term Goals  :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_ShortTearm_Previous" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                
                                                
                                                
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            3. Previous Long Term Goals :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_Previous" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                           4. Long Term Goals(Functional Outcome Measured)1 - Year :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_LongTerm" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            5. Short Term Goals(Functional Outcome Measures) 3 - 4 Months :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_ShortTerm" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                Rows="3"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix">
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span12">
                                                        <div class="control-label">
                                                            6. impairment related Objective goal-3 Months :
                                                        </div>
                                                        <div class="control-group">
                                                            <asp:TextBox ID="Evaluation_Goal_Impairment" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Frequency" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Service" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Strategies" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Equipment" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Education" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:DropDownList ID="Doctor_Physioptherapist" runat="server" Enabled="false" CssClass="chzn-select span6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        2. Physioptherapist :
                                    </div>
                                    <div class="control-group">
                                        <asp:DropDownList ID="Doctor_Occupational" runat="server" Enabled="false" CssClass="chzn-select span6">
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
                                        <asp:DropDownList ID="Doctor_EnterReport" Enabled="false"  runat="server" CssClass="chzn-select span6">
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
