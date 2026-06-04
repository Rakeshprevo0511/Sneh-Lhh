<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="RevalRpt.aspx.cs" Inherits="snehrehab.SessionRpt.RevalRpt" %>
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

.buttonClass
        {
           background-color:springgreen;
        }
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
    <div id="MsgPlaceHolder"></div>
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
               Re-Eval Report :</div>
            <div class="pull-right">
            <a href="/Reports/RevalView1.aspx" class="btn btn-primary">View List</a>
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

    <div class="clearfix"></div>

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
                       <button type="button" id="btnFinalSubmit" class="btn btn-danger">
    Submit
</button>
                        &nbsp;
                        <%= _printUrl %>
                        <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
                        <asp:HiddenField ID="txtPrint" runat="server" />
                    </div>
                </div>
                <div class="clearfix">
                </div>
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
            <asp:HiddenField ID="hfdTabs" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfdCallFrom" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfdCurTab" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfdPrevTab" runat="server" ClientIDMode="Static" />

            <ajaxToolkit:TabContainer ID="tb_Contents" runat="server" OnClientActiveTabChanged="clientActiveTabChanged" ClientIDMode="Static">
                <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="Data Collection" value="0">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <asp:Panel ID="PanelDiagnosis" runat="server" CssClass="span11 formRow"> 
                                <div class="row">
                                    <div class="span2">
                                        Diagnosis :</div>
                                    <div class="span4">
                                        <asp:ListBox ID="txtDiagnosis" runat="server" SelectionMode="Multiple" CssClass="chzn-select-multi span4" data-placeholder="Select Diagnosis"></asp:ListBox>
                                    </div>
                                    <div class="span2">
                                        Other Diagnosis :</div>
                                    <div class="span2">
                                        <asp:TextBox ID="txtDiagnosisOther" runat="server" CssClass="span2"></asp:TextBox>
                                    </div>
                                </div> 
                            </asp:Panel>
                            <div class="formRow char-line-limiter">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Current Concern :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="DataCollection_CurrentConcern" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_ImprovementsSinceLastEval" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_MedicalHistory" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="DataCollection_DailyRoutine" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_Expectaion" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_TherapyHistory" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="DataCollection_Sources" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_NumberVisit" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="DataCollection_AdaptedEquipment" runat="server" CssClass="span10"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Morphology" value="1">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <table class="Morphology-OuterTopTable">
                                        <tr>
                                            <td>
                                                <b>Height</b><br />
                                                <asp:TextBox ID="Morphology_Height" runat="server" CssClass="span2"></asp:TextBox>
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
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLeft" TextMode=MultiLine CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbRight" TextMode="MultiLine" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    <b>True Limb Length</b>
                                                    </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_TrueLimbLengthLeft" TextMode="MultiLine" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                           <asp:TextBox runat="server" ID="Morphology_TrueLimbLengthRight" TextMode="MultiLine" CssClass="span2"></asp:TextBox>
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
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLeft" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_ArmRight" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="Morphology_ArmLength" CssClass="span4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <b>Head Circumference</b><br />
                                                <asp:TextBox ID="Morphology_Head" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Nipple</b><br />
                                                <asp:TextBox ID="Morphology_Nipple" runat="server" CssClass="span2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Weight </b>
                                                <br />
                                                <asp:TextBox ID="Morphology_Weight" runat="server" CssClass="span2"></asp:TextBox>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <b>Waist (Umbilical)</b><br />
                                                <asp:TextBox ID="Morphology_Waist" runat="server" CssClass="span2"></asp:TextBox>
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
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Above_ElbowRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLevel" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowLeft" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_At_ElbowRight" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Elbow
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthUpperLimb_Below_ElbowRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
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
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Above_KneeRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    At Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLevel" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeLeft" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_At_KneeRight" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Below Knee
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLevel3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeLeft3" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight1" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight2" runat="server" CssClass="span2 textbox"></asp:TextBox>
                                    <asp:TextBox ID="Morphology_GirthLowerLimb_Below_KneeRight3" runat="server" CssClass="span2 textbox"></asp:TextBox>
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
                                        <asp:TextBox ID="Morphology_OralMotorFactors" runat="server" CssClass="span10" TextMode="MultiLine"
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
                <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="Functional Activities" value="2">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
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
                                        4. ADL :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="FunctionalActivities_ADL" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="FunctionalActivities_OralMotor" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="FunctionalActivities_Communication" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="TestMeasures_GMFCS" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_GMFM" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_GMPM" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_AshworthScale" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_TradieusScale" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_OGS" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_Melbourne" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_COPM" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="TestMeasures_ClinicalObservation" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="TestMeasures_Others" runat="server" CssClass="span10" TextMode="MultiLine"
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
                            </div>
                            <div class="formRow">
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
                                        <asp:TextBox ID="Movement_Inertia" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Movement_Strategies" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Movement_Extremities" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Others_Integration" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Others_Assessments" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Regulatory_Regulation" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        3. Attention :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Attention" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Affect" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Action" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExtensionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipAbductionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipExternalRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_HipInternalRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Popliteal Angle
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PoplitealRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Knee Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_KneeExtensionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Dorsiflexion With Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionRight" runat="server"
                                                                        CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Plantar Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_PlantarFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom1_OthersRight" runat="server" CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Shoulder Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ShoulderExtensionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Horizontal Abduction
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_HorizontalAbductionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    External Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ExternalRotationRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Internal Rotation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_InternalRotationRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Elbow Extension
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_ElbowExtensionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_SupinationRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_PronationRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Flexion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristFlexionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Wrist Extesion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_WristExtesionRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Others
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Rom2_OthersRight" runat="server" CssClass="span3"></asp:TextBox>
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
                                                            <asp:TextBox ID="Musculoskeletal_Strengthlp" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthCC" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthMuscle" runat="server" CssClass="span10"
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
                                                            <asp:TextBox ID="Musculoskeletal_StrengthSkeletal" runat="server" CssClass="span10"
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
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HipflexorsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hams
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_HamsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Quads
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_QuadsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisAnteriorRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tibialis Posterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TibialisPosteriorRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorDigitorumRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorHallucisRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Peronei
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PeroneiRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Digitorum
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorDigitorumRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Hallucis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorHallucisRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Anterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AnteriorDeltoidRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Posterior Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PosteriorDeltoidRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Middle Deltoid
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_MiddleDeltoidRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supraspinatus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupraspinatusRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Serratus Anterior
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SerratusAnteriorRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Rhomboids
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_RhomboidsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Biceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_BicepsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Triceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_TricepsRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Supinator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_SupinatorRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Pronator
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_PronatorRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECULeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECURight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECRRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ECS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ECSRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCU
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCULeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCURight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCR
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCRRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    FCS
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FCSRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Opponens Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_OpponensPollicisRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Flexor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_FlexorPollicisRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Abductor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_AbductorPollicisRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Extensor Pollicis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExtensorPollicisRight" runat="server" CssClass="span3"></asp:TextBox>
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
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Ability to initate sustain,to initate sustain,and terminate muscle activity.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainGeneral" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainControl" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SustainTiming" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Recruitment of postural(SO)and phasic or movement(FF) motor unit.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalGeneral" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalControl" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_PosturalTiming" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Ability to perform concentric, isometric, and eccentric muscle contractions.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsGeneral" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsControl" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ContractionsTiming" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Recruitment of cocontraction and/or reciprocal inhibition of agonist and antagonist.</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistGeneral" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistControl" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_AntagonistTiming" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox5" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Synergy selectivity( mas vs.isolated,repertoire).</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyGeneral" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyControl" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_SynergyTiming" runat="server" CssClass="span3" TextMode="MultiLine"
                                                        Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox6" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Stiffiness(delta F/delta L) .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessGeneral" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessControl" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_StiffinessTiming" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox7" runat="server" CssClass="span3" TextMode="MultiLine" Rows="3"
                                                        ReadOnly="true">Extraneous movements (tremors,clonus,nystagmus, etc .</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousGeneral" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousControl" runat="server" CssClass="span3"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="RemarkVariable_ExtraneousTiming" runat="server" CssClass="span3"
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
                                        <asp:TextBox ID="SensorySystem_Vision" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensorySystem_Somatosensory" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensorySystem_Vestibular" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensorySystem_Auditory" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensorySystem_Gustatory" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="SensoryProfile_Profile" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                        <asp:TextBox ID="txtSensory_Profile_NameResults_Name" runat="server" CssClass="span4" Text='<%#Eval("NAME") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="txtSensory_Profile_NameResults_Result" runat="server" CssClass="span4" Text='<%#Eval("RESULTS") %>'></asp:TextBox></td>                                                    
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
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GraspLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Spherical
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_SphericalLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :Hook
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_HookLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    :3 Jaw Chuck
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_JawChuckLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Grip
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_HandFunction1_GripLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Release
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
                                                                <td>
                                                                    Pinch
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT3_Spontaneous" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaching > On Command
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Kinesthesia" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Finger Identification Test
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Finger" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Localisation Of Tactile Stimuli
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Localisation" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Double Tactile Localisation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_DoubleTactile" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Tactile Discrimination
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Tactile" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Graphesthesia
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_Graphesthesia" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Post Rotary Nystagmus
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT4_PostRotary" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Standing And Walking Balance
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Color" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Form Constancy
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Form" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Size Differentiation
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Size" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Depth Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Depth" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Figure Ground Perception
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT5_Figure" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Accuracy
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT6_Design" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Constructional Praxis
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT7_Scanning" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Visual Memory
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Postural" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Oral Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Oral" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sequencing Praxis
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT8_Sequencing" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Praxis On Verbal Commands
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Bilateral" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Contralat Use
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_Contralat" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Space Visualisation Preferred Hand
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT9_PreferredHand" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Crossing Midline
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
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Draw" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Clock Face
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_ClockFace" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Filtering Information
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_Filtering" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Motor Planning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_MotorPlanning" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Image
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodyImage" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Body Schema
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Laterality
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
                                        <ajaxToolkit:TabPanel ID="TabPanel12" runat="server" HeaderText="Activity Given">
                                            <ContentTemplate>
                                                <div class="formRow">
                                                    <div class="span12">
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
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Interest In Completion
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Initial Learning
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Complexity And Organisation Of Task
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Problem Solving
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Concentration
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Retension And Recall
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Speed Of Perfomance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Activity Neatness
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Frustation Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Work Tolerance
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Reaction To Authority
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Therapist
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Sociability With Others Students
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
                <ajaxToolkit:TabPanel ID="tb_Report15" runat="server" HeaderText="Cognition">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Intelligence :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="Cognition_Intelligence" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Attention" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Memory" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_Adaptibility" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_MotorPlanning" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_ExecutiveFunction" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cognition_CognitiveFunctions" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinIntegrity" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinColor" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Integumentary_SkinExtensibility" runat="server" CssClass="span10"
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
                                        <asp:TextBox ID="Respiratory_RateResting" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_Patterns" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Respiratory_BreathControl" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_HeartRate" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_BP" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_Edema" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_Circulation" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Cardiovascular_EEi" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Gastrointestinal_Bowel" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                        <asp:TextBox ID="Gastrointestinal_Intake" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Strengths" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evalutionadding_Strengths" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" CssClass="span10"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Concern_Impairment" runat="server" CssClass="span10"
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
                                                            <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Goal_ShortTearm_Previous" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Goal_Previous" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Goal_LongTerm" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Goal_ShortTerm" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Goal_Impairment" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Frequency" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Service" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Strategies" runat="server" CssClass="span10" TextMode="MultiLine"
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
                                                            <asp:TextBox ID="Evaluation_Plan_Equipment" runat="server" CssClass="span10" TextMode="MultiLine"
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
                <ajaxToolkit:TabPanel ID="tb_Report21" runat="server" HeaderText="Doctor">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
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
                      <%--      <div class="formRow">
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
            <div>
              <button type="button" id="btnSaveNext" class="buttonClass" style="width:200px;display:none">
    Save&Next
     </button>
            </div>
            </div>
            </div>
            <div class="clearfix">
            </div>
            </div>
        </div>
    </div>

      <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog"  style="max-width:400px; max-height:400px">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Modal Header</h4>
        </div>
        <div class="modal-body">
            <h5 class="modal-title"> DATA IS SAVING PLEASE WAIT.. </h5>
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
   </script>
      <script type="text/javascript">
          function Changetab(ctl, tabp) {
              console.log("save", ctl, tabp);
          }
      </script>--%>

  <script type="text/javascript">
      // =========================
      // GLOBAL VARIABLES
      // =========================
      var preTabId = "";
      var CurTabId = "";
      var nextAfterSave = false;
      let saveInProgress = false;
      // =========================
      // PAGE READY
      // =========================
      $(document).ready(function () {

          // default set first time
          if (!$("#hfdPrevTab").val()) $("#hfdPrevTab").val("tb_Report1");
          if (!$("#hfdCurTab").val()) $("#hfdCurTab").val("tb_Report1");

          // Save & Next button
          $("#btnSaveNext").on("click", function (e) {
              e.preventDefault();

              var curTab = getCurrentTabId();

              // 🔥 prepare data for preview (optional minimal)
              var formData = { Tab: curTab };

              showConfirmPopup(formData, function () {

                  // ✅ ORIGINAL LOGIC (unchanged)
                  nextAfterSave = true;
                  $("#hfdTabs").val(curTab);
                  $("#hfdCurTab").val(curTab);
                  $("#hfdCallFrom").val("SaveNext");

                  SaveTabById(curTab, false);
              });
          });

          // Final Submit button
          $("#btnFinalSubmit").on("click", function (e) {
              e.preventDefault();

              var curTab = getCurrentTabId();
              nextAfterSave = false;
              $("#hfdTabs").val(curTab);
              $("#hfdCurTab").val(curTab);
              $("#hfdCallFrom").val("Submit");
              SaveTabById(curTab, true);

          });
      });

      // =========================
      // IMPORTANT: MUST BE GLOBAL
      // Telerik Tab Change Event
      // =========================
      function clientActiveTabChanged(sender, args) {

          try {
              var tab = sender.get_tabs()[sender.get_activeTabIndex()];
              CurTabId = tab.get_id();

              var prevTab = $("#hfdCurTab").val(); // old tab before change

              $("#hfdPrevTab").val(prevTab);       // store previous tab
              $("#hfdCurTab").val(CurTabId);       // store current tab

              if (prevTab && prevTab !== CurTabId) {
                  SaveTabById(prevTab, false);     // save old tab
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
          switch (tabId) {
              case "tb_Report1": SaveTab1(reloadAfterSave); break;
              case "tb_Report2": SaveTab2(reloadAfterSave); break;
              case "tb_Report3": SaveTab3(reloadAfterSave); break;
              case "tb_Report4": SaveTab4(reloadAfterSave); break;
              case "tb_Report5": SaveTab5(reloadAfterSave); break;
              case "tb_Report6": SaveTab6(reloadAfterSave); break;
              case "tb_Report7": SaveTab7(reloadAfterSave); break;
              case "tb_Report8": SaveTab8(reloadAfterSave); break;
              case "tb_Report9": SaveTab9(reloadAfterSave); break;
              case "tb_Report10": SaveTab10(reloadAfterSave); break;
              case "tb_Report11": SaveTab11(reloadAfterSave); break;
              case "tb_Report12": SaveTab12(reloadAfterSave); break;
              case "tb_Report13": SaveTab13(reloadAfterSave); break;
              case "tb_Report14": SaveTab14(reloadAfterSave); break;
              case "tb_Report15": SaveTab15(reloadAfterSave); break;
              case "tb_Report16": SaveTab16(reloadAfterSave); break;
              case "tb_Report17": SaveTab17(reloadAfterSave); break;
              case "tb_Report18": SaveTab18(reloadAfterSave); break;
              case "tb_Report19": SaveTab19(reloadAfterSave); break;
              case "tb_Report20": SaveTab20(reloadAfterSave); break;
              case "tb_Report21": SaveTab21(reloadAfterSave); break;

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
              url: "<%= ResolveUrl("~/Handler/SaveRptNDTReval_New.ashx") %>",
               data: formData, // normal object
               success: function (res) {

                   var arr = (res || "").split("|");

                   if (arr[0] === "OK") {

                       showAlert((tabName || "Tab") + " Saved Successfully", 1);

                       if (nextAfterSave === true) {
                           nextAfterSave = false;

                           try {
                               var tabStrip = $find("<%= tb_Contents.ClientID %>");
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

                   if (reloadAfterSave === true) {
                       setTimeout(function () {
                           location.reload();
                       }, 1200);
                   }
               },

               error: function (xhr) {

                   $("#btnSaveNext").prop("disabled", false);
                   $("#btnFinalSubmit").prop("disabled", false);

                   showAlert((tabName || "Tab") + " Save Failed!");
                   console.log(xhr.responseText);
               },
               complete: function () {
                   $("#btnSaveNext").prop("disabled", false);
                   $("#btnFinalSubmit").prop("disabled", false);
                   saveInProgress = false; // 🔓 unlock here only
               }
           });
      }

      // =========================
      // TAB SAVE FUNCTIONS
      // =========================
      function SaveFinalOnly(reloadAfterSave) {

          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.Action = "FinalOnly";   // identify in handler

           formData.IsFinal = $("#<%= txtFinal.ClientID %>").is(":checked");
           formData.IsGiven = $("#<%= txtGiven.ClientID %>").is(":checked");
           formData.GivenDate = $("#<%= txtGivenDate.ClientID %>").val();

           // DiagnosisIDs pipe separated
           var ids = [];
           $("#<%= txtDiagnosis.ClientID %> option:selected").each(function () {
               ids.push($(this).val());
           });
           formData.DiagnosisIDs = ids.join("|");

           formData.DiagnosisOther = $("#<%= txtDiagnosisOther.ClientID %>").val();

          showConfirmPopup(formData, function () {
              PostToHandler(formData, reloadAfterSave, "Final Submit");
          });
      }
      function SaveTab1(reloadAfterSave) {

          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 1;

          formData.txtDiagnosis = GetMultiSelectValues("#txtDiagnosis");
          formData.txtDiagnosisOther = $("#txtDiagnosisOther").val();

          formData.DataCollection_CurrentConcern = $("#DataCollection_CurrentConcern").val();
          formData.DataCollection_ImprovementsSinceLastEval = $("#DataCollection_ImprovementsSinceLastEval").val();
          formData.DataCollection_MedicalHistory = $("#DataCollection_MedicalHistory").val();
          formData.DataCollection_DailyRoutine = $("#DataCollection_DailyRoutine").val();
          formData.DataCollection_Expectaion = $("#DataCollection_Expectaion").val();
          formData.DataCollection_TherapyHistory = $("#DataCollection_TherapyHistory").val();
          formData.DataCollection_Sources = $("#DataCollection_Sources").val();
          formData.DataCollection_NumberVisit = $("#DataCollection_NumberVisit").val();
          formData.DataCollection_AdaptedEquipment = $("#DataCollection_AdaptedEquipment").val();

          showConfirmPopup(formData, function () {
              PostToHandler(formData, reloadAfterSave, "Data Collection");
          });
      }
      function SaveTab2(reloadAfterSave) {

          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 2;

          // ---- Basic Morphology ----
          formData.Morphology_Height = $("#Morphology_Height").val();
          formData.Morphology_Weight = $("#Morphology_Weight").val();
          formData.Morphology_Head = $("#Morphology_Head").val();
          formData.Morphology_Nipple = $("#Morphology_Nipple").val();
          formData.Morphology_Waist = $("#Morphology_Waist").val();

          // ---- Limb Length ----
          formData.Morphology_LimbLeft = $("#Morphology_LimbLeft").val();
          formData.Morphology_LimbRight = $("#Morphology_LimbRight").val();

          formData.Morphology_TrueLimbLengthLeft = $("#Morphology_TrueLimbLengthLeft").val();
          formData.Morphology_TrueLimbLengthRight = $("#Morphology_TrueLimbLengthRight").val();

          // ---- Arm ----
          formData.Morphology_ArmLeft = $("#Morphology_ArmLeft").val();
          formData.Morphology_ArmRight = $("#Morphology_ArmRight").val();
          formData.Morphology_ArmLength = $("#Morphology_ArmLength").val();

          // ================= UPPER LIMB =================

          // Above Elbow – Level
          formData.Morphology_GirthUpperLimb_Above_ElbowLevel1 = $("#Morphology_GirthUpperLimb_Above_ElbowLevel1").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowLevel2 = $("#Morphology_GirthUpperLimb_Above_ElbowLevel2").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowLevel3 = $("#Morphology_GirthUpperLimb_Above_ElbowLevel3").val();

          // Above Elbow – Left
          formData.Morphology_GirthUpperLimb_Above_ElbowLeft1 = $("#Morphology_GirthUpperLimb_Above_ElbowLeft1").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowLeft2 = $("#Morphology_GirthUpperLimb_Above_ElbowLeft2").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowLeft3 = $("#Morphology_GirthUpperLimb_Above_ElbowLeft3").val();

          // Above Elbow – Right
          formData.Morphology_GirthUpperLimb_Above_ElbowRight1 = $("#Morphology_GirthUpperLimb_Above_ElbowRight1").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowRight2 = $("#Morphology_GirthUpperLimb_Above_ElbowRight2").val();
          formData.Morphology_GirthUpperLimb_Above_ElbowRight3 = $("#Morphology_GirthUpperLimb_Above_ElbowRight3").val();

          // At Elbow
          formData.Morphology_GirthUpperLimb_At_ElbowLevel = $("#Morphology_GirthUpperLimb_At_ElbowLevel").val();
          formData.Morphology_GirthUpperLimb_At_ElbowLeft = $("#Morphology_GirthUpperLimb_At_ElbowLeft").val();
          formData.Morphology_GirthUpperLimb_At_ElbowRight = $("#Morphology_GirthUpperLimb_At_ElbowRight").val();

          // Below Elbow – Level
          formData.Morphology_GirthUpperLimb_Below_ElbowLevel1 = $("#Morphology_GirthUpperLimb_Below_ElbowLevel1").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowLevel2 = $("#Morphology_GirthUpperLimb_Below_ElbowLevel2").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowLevel3 = $("#Morphology_GirthUpperLimb_Below_ElbowLevel3").val();

          // Below Elbow – Left
          formData.Morphology_GirthUpperLimb_Below_ElbowLeft1 = $("#Morphology_GirthUpperLimb_Below_ElbowLeft1").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowLeft2 = $("#Morphology_GirthUpperLimb_Below_ElbowLeft2").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowLeft3 = $("#Morphology_GirthUpperLimb_Below_ElbowLeft3").val();

          // Below Elbow – Right
          formData.Morphology_GirthUpperLimb_Below_ElbowRight1 = $("#Morphology_GirthUpperLimb_Below_ElbowRight1").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowRight2 = $("#Morphology_GirthUpperLimb_Below_ElbowRight2").val();
          formData.Morphology_GirthUpperLimb_Below_ElbowRight3 = $("#Morphology_GirthUpperLimb_Below_ElbowRight3").val();

          // ================= LOWER LIMB =================

          // Above Knee – Level
          formData.Morphology_GirthLowerLimb_Above_KneeLevel1 = $("#Morphology_GirthLowerLimb_Above_KneeLevel1").val();
          formData.Morphology_GirthLowerLimb_Above_KneeLevel2 = $("#Morphology_GirthLowerLimb_Above_KneeLevel2").val();
          formData.Morphology_GirthLowerLimb_Above_KneeLevel3 = $("#Morphology_GirthLowerLimb_Above_KneeLevel3").val();

          // Above Knee – Left
          formData.Morphology_GirthLowerLimb_Above_KneeLeft1 = $("#Morphology_GirthLowerLimb_Above_KneeLeft1").val();
          formData.Morphology_GirthLowerLimb_Above_KneeLeft2 = $("#Morphology_GirthLowerLimb_Above_KneeLeft2").val();
          formData.Morphology_GirthLowerLimb_Above_KneeLeft3 = $("#Morphology_GirthLowerLimb_Above_KneeLeft3").val();

          // Above Knee – Right
          formData.Morphology_GirthLowerLimb_Above_KneeRight1 = $("#Morphology_GirthLowerLimb_Above_KneeRight1").val();
          formData.Morphology_GirthLowerLimb_Above_KneeRight2 = $("#Morphology_GirthLowerLimb_Above_KneeRight2").val();
          formData.Morphology_GirthLowerLimb_Above_KneeRight3 = $("#Morphology_GirthLowerLimb_Above_KneeRight3").val();

          // At Knee
          formData.Morphology_GirthLowerLimb_At_KneeLevel = $("#Morphology_GirthLowerLimb_At_KneeLevel").val();
          formData.Morphology_GirthLowerLimb_At_KneeLeft = $("#Morphology_GirthLowerLimb_At_KneeLeft").val();
          formData.Morphology_GirthLowerLimb_At_KneeRight = $("#Morphology_GirthLowerLimb_At_KneeRight").val();

          // Below Knee – Level
          formData.Morphology_GirthLowerLimb_Below_KneeLevel1 = $("#Morphology_GirthLowerLimb_Below_KneeLevel1").val();
          formData.Morphology_GirthLowerLimb_Below_KneeLevel2 = $("#Morphology_GirthLowerLimb_Below_KneeLevel2").val();
          formData.Morphology_GirthLowerLimb_Below_KneeLevel3 = $("#Morphology_GirthLowerLimb_Below_KneeLevel3").val();

          // Below Knee – Left
          formData.Morphology_GirthLowerLimb_Below_KneeLeft1 = $("#Morphology_GirthLowerLimb_Below_KneeLeft1").val();
          formData.Morphology_GirthLowerLimb_Below_KneeLeft2 = $("#Morphology_GirthLowerLimb_Below_KneeLeft2").val();
          formData.Morphology_GirthLowerLimb_Below_KneeLeft3 = $("#Morphology_GirthLowerLimb_Below_KneeLeft3").val();

          // Below Knee – Right
          formData.Morphology_GirthLowerLimb_Below_KneeRight1 = $("#Morphology_GirthLowerLimb_Below_KneeRight1").val();
          formData.Morphology_GirthLowerLimb_Below_KneeRight2 = $("#Morphology_GirthLowerLimb_Below_KneeRight2").val();
          formData.Morphology_GirthLowerLimb_Below_KneeRight3 = $("#Morphology_GirthLowerLimb_Below_KneeRight3").val();

          // ---- Oral Motor ----
          formData.Morphology_OralMotorFactors = $("#Morphology_OralMotorFactors").val();

          // ---- POST ----
          showConfirmPopup(formData, function () {
              PostToHandler(formData, reloadAfterSave, "Morphology");
          });
      }
      function SaveTab3(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 3;

          formData.FunctionalActivities_GrossMotor = $("#FunctionalActivities_GrossMotor").val();
          formData.FunctionalActivities_HandFunction = $("#FunctionalActivities_HandFunction").val();
          formData.FunctionalActivities_FineMotor = $("#FunctionalActivities_FineMotor").val();
          formData.FunctionalActivities_ADL = $("#FunctionalActivities_ADL").val();
          formData.FunctionalActivities_OralMotor = $("#FunctionalActivities_OralMotor").val();
          formData.FunctionalActivities_Communication = $("#FunctionalActivities_Communication").val();

          saveWithModal(formData, reloadAfterSave, "Functional Activities");
      }

      function SaveTab4(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 4;

          formData.TestMeasures_GMFCS = $("#TestMeasures_GMFCS").val();
          formData.TestMeasures_GMFM = $("#TestMeasures_GMFM").val();
          formData.TestMeasures_GMPM = $("#TestMeasures_GMPM").val();
          formData.TestMeasures_AshworthScale = $("#TestMeasures_AshworthScale").val();
          formData.TestMeasures_TradieusScale = $("#TestMeasures_TradieusScale").val();
          formData.TestMeasures_OGS = $("#TestMeasures_OGS").val();
          formData.TestMeasures_Melbourne = $("#TestMeasures_Melbourne").val();
          formData.TestMeasures_COPM = $("#TestMeasures_COPM").val();
          formData.TestMeasures_ClinicalObservation = $("#TestMeasures_ClinicalObservation").val();
          formData.TestMeasures_Others = $("#TestMeasures_Others").val();

          saveWithModal(formData, reloadAfterSave, "Test And Measures");
      }

      function SaveTab5(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 5;

          formData.Posture_Alignment = $("#Posture_Alignment").val();
          formData.Posture_Biomechanics = $("#Posture_Biomechanics").val();
          formData.Posture_Stability = $("#Posture_Stability").val();
          formData.Posture_Anticipatory = $("#Posture_Anticipatory").val();
          formData.Posture_Postural = $("#Posture_Postural").val();
          formData.Posture_SignsPostural = $("#Posture_SignsPostural").val();

          saveWithModal(formData, reloadAfterSave, "Posture");
      }

      function SaveTab6(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 6;

          formData.Movement_Inertia = $("#Movement_Inertia").val();
          formData.Movement_Strategies = $("#Movement_Strategies").val();
          formData.Movement_Extremities = $("#Movement_Extremities").val();
          formData.Movement_Stability = $("#Movement_Stability").val();
          formData.Movement_Overuse = $("#Movement_Overuse").val();

          saveWithModal(formData, reloadAfterSave, "Movement");
      }

      function SaveTab7(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
          formData.TabNo = 7;

          formData.Others_Integration = $("#Others_Integration").val();
          formData.Others_Assessments = $("#Others_Assessments").val();

          saveWithModal(formData, reloadAfterSave, "Others");
      }

      function SaveTab8(reloadAfterSave) {
          var formData = {};
          formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 8;

           formData.Attention = $("#Attention").val();
           formData.Affect = $("#Affect").val();
           formData.Action = $("#Action").val();
           formData.Regulatory_Arousal = $("#Regulatory_Arousal").val();
           formData.Regulatory_Regulation = $("#Regulatory_Regulation").val();

           saveWithModal(formData, reloadAfterSave, "System Exmination");
       }

       function SaveTab9(reloadAfterSave) {
           var formData = {};
           formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 9;

           // ROM 1
           formData.Musculoskeletal_Rom1_HipFlexionLeft = $("#Musculoskeletal_Rom1_HipFlexionLeft").val();
           formData.Musculoskeletal_Rom1_HipFlexionRight = $("#Musculoskeletal_Rom1_HipFlexionRight").val();
           formData.Musculoskeletal_Rom1_HipExtensionLeft = $("#Musculoskeletal_Rom1_HipExtensionLeft").val();
           formData.Musculoskeletal_Rom1_HipAbductionLeft = $("#Musculoskeletal_Rom1_HipAbductionLeft").val();
           formData.Musculoskeletal_Rom1_HipAbductionRight = $("#Musculoskeletal_Rom1_HipAbductionRight").val();
           formData.Musculoskeletal_Rom1_HipExtensionRight = $("#Musculoskeletal_Rom1_HipExtensionRight").val();
           formData.Musculoskeletal_Rom1_HipExternalLeft = $("#Musculoskeletal_Rom1_HipExternalLeft").val();
           formData.Musculoskeletal_Rom1_HipExternalRight = $("#Musculoskeletal_Rom1_HipExternalRight").val();
           formData.Musculoskeletal_Rom1_HipInternalLeft = $("#Musculoskeletal_Rom1_HipInternalLeft").val();
           formData.Musculoskeletal_Rom1_HipInternalRight = $("#Musculoskeletal_Rom1_HipInternalRight").val();
           formData.Musculoskeletal_Rom1_PoplitealLeft = $("#Musculoskeletal_Rom1_PoplitealLeft").val();
           formData.Musculoskeletal_Rom1_PoplitealRight = $("#Musculoskeletal_Rom1_PoplitealRight").val();
           formData.Musculoskeletal_Rom1_KneeFlexionLeft = $("#Musculoskeletal_Rom1_KneeFlexionLeft").val();
           formData.Musculoskeletal_Rom1_KneeFlexionRight = $("#Musculoskeletal_Rom1_KneeFlexionRight").val();
           formData.Musculoskeletal_Rom1_KneeExtensionLeft = $("#Musculoskeletal_Rom1_KneeExtensionLeft").val();
           formData.Musculoskeletal_Rom1_KneeExtensionRight = $("#Musculoskeletal_Rom1_KneeExtensionRight").val();
           formData.Musculoskeletal_Rom1_DorsiflexionFlexionLeft = $("#Musculoskeletal_Rom1_DorsiflexionFlexionLeft").val();
           formData.Musculoskeletal_Rom1_DorsiflexionFlexionRight = $("#Musculoskeletal_Rom1_DorsiflexionFlexionRight").val();
           formData.Musculoskeletal_Rom1_DorsiflexionExtensionLeft = $("#Musculoskeletal_Rom1_DorsiflexionExtensionLeft").val();
           formData.Musculoskeletal_Rom1_DorsiflexionExtensionRight = $("#Musculoskeletal_Rom1_DorsiflexionExtensionRight").val();
           formData.Musculoskeletal_Rom1_PlantarFlexionLeft = $("#Musculoskeletal_Rom1_PlantarFlexionLeft").val();
           formData.Musculoskeletal_Rom1_PlantarFlexionRight = $("#Musculoskeletal_Rom1_PlantarFlexionRight").val();
           formData.Musculoskeletal_Rom1_OthersLeft = $("#Musculoskeletal_Rom1_OthersLeft").val();
           formData.Musculoskeletal_Rom1_OthersRight = $("#Musculoskeletal_Rom1_OthersRight").val();

           // ROM 2
           formData.Musculoskeletal_Rom2_ShoulderFlexionLeft = $("#Musculoskeletal_Rom2_ShoulderFlexionLeft").val();
           formData.Musculoskeletal_Rom2_ShoulderFlexionRight = $("#Musculoskeletal_Rom2_ShoulderFlexionRight").val();
           formData.Musculoskeletal_Rom2_ShoulderExtensionLeft = $("#Musculoskeletal_Rom2_ShoulderExtensionLeft").val();
           formData.Musculoskeletal_Rom2_ShoulderExtensionRight = $("#Musculoskeletal_Rom2_ShoulderExtensionRight").val();
           formData.Musculoskeletal_Rom2_HorizontalAbductionLeft = $("#Musculoskeletal_Rom2_HorizontalAbductionLeft").val();
           formData.Musculoskeletal_Rom2_HorizontalAbductionRight = $("#Musculoskeletal_Rom2_HorizontalAbductionRight").val();
           formData.Musculoskeletal_Rom2_ExternalRotationLeft = $("#Musculoskeletal_Rom2_ExternalRotationLeft").val();
           formData.Musculoskeletal_Rom2_ExternalRotationRight = $("#Musculoskeletal_Rom2_ExternalRotationRight").val();
           formData.Musculoskeletal_Rom2_InternalRotationLeft = $("#Musculoskeletal_Rom2_InternalRotationLeft").val();
           formData.Musculoskeletal_Rom2_InternalRotationRight = $("#Musculoskeletal_Rom2_InternalRotationRight").val();
           formData.Musculoskeletal_Rom2_ElbowFlexionLeft = $("#Musculoskeletal_Rom2_ElbowFlexionLeft").val();
           formData.Musculoskeletal_Rom2_ElbowFlexionRight = $("#Musculoskeletal_Rom2_ElbowFlexionRight").val();
           formData.Musculoskeletal_Rom2_ElbowExtensionLeft = $("#Musculoskeletal_Rom2_ElbowExtensionLeft").val();
           formData.Musculoskeletal_Rom2_ElbowExtensionRight = $("#Musculoskeletal_Rom2_ElbowExtensionRight").val();
           formData.Musculoskeletal_Rom2_SupinationLeft = $("#Musculoskeletal_Rom2_SupinationLeft").val();
           formData.Musculoskeletal_Rom2_SupinationRight = $("#Musculoskeletal_Rom2_SupinationRight").val();
           formData.Musculoskeletal_Rom2_PronationLeft = $("#Musculoskeletal_Rom2_PronationLeft").val();
           formData.Musculoskeletal_Rom2_PronationRight = $("#Musculoskeletal_Rom2_PronationRight").val();
           formData.Musculoskeletal_Rom2_WristFlexionLeft = $("#Musculoskeletal_Rom2_WristFlexionLeft").val();
           formData.Musculoskeletal_Rom2_WristFlexionRight = $("#Musculoskeletal_Rom2_WristFlexionRight").val();
           formData.Musculoskeletal_Rom2_WristExtesionLeft = $("#Musculoskeletal_Rom2_WristExtesionLeft").val();
           formData.Musculoskeletal_Rom2_WristExtesionRight = $("#Musculoskeletal_Rom2_WristExtesionRight").val();
           formData.Musculoskeletal_Rom2_OthersLeft = $("#Musculoskeletal_Rom2_OthersLeft").val();
           formData.Musculoskeletal_Rom2_OthersRight = $("#Musculoskeletal_Rom2_OthersRight").val();

           // Strength
           formData.Musculoskeletal_Strengthlp = $("#Musculoskeletal_Strengthlp").val();
           formData.Musculoskeletal_StrengthCC = $("#Musculoskeletal_StrengthCC").val();
           formData.Musculoskeletal_StrengthMuscle = $("#Musculoskeletal_StrengthMuscle").val();
           formData.Musculoskeletal_StrengthSkeletal = $("#Musculoskeletal_StrengthSkeletal").val();

           // MMT
           formData.Musculoskeletal_Mmt_HipflexorsLeft = $("#Musculoskeletal_Mmt_HipflexorsLeft").val();
           formData.Musculoskeletal_Mmt_HipflexorsRight = $("#Musculoskeletal_Mmt_HipflexorsRight").val();
           formData.Musculoskeletal_Mmt_AbductorsLeft = $("#Musculoskeletal_Mmt_AbductorsLeft").val();
           formData.Musculoskeletal_Mmt_AbductorsRight = $("#Musculoskeletal_Mmt_AbductorsRight").val();
           formData.Musculoskeletal_Mmt_ExtensorsLeft = $("#Musculoskeletal_Mmt_ExtensorsLeft").val();
           formData.Musculoskeletal_Mmt_ExtensorsRight = $("#Musculoskeletal_Mmt_ExtensorsRight").val();
           formData.Musculoskeletal_Mmt_HamsLeft = $("#Musculoskeletal_Mmt_HamsLeft").val();
           formData.Musculoskeletal_Mmt_HamsRight = $("#Musculoskeletal_Mmt_HamsRight").val();
           formData.Musculoskeletal_Mmt_QuadsLeft = $("#Musculoskeletal_Mmt_QuadsLeft").val();
           formData.Musculoskeletal_Mmt_QuadsRight = $("#Musculoskeletal_Mmt_QuadsRight").val();
           formData.Musculoskeletal_Mmt_TibialisAnteriorLeft = $("#Musculoskeletal_Mmt_TibialisAnteriorLeft").val();
           formData.Musculoskeletal_Mmt_TibialisAnteriorRight = $("#Musculoskeletal_Mmt_TibialisAnteriorRight").val();
           formData.Musculoskeletal_Mmt_TibialisPosteriorLeft = $("#Musculoskeletal_Mmt_TibialisPosteriorLeft").val();
           formData.Musculoskeletal_Mmt_TibialisPosteriorRight = $("#Musculoskeletal_Mmt_TibialisPosteriorRight").val();
           formData.Musculoskeletal_Mmt_ExtensorDigitorumLeft = $("#Musculoskeletal_Mmt_ExtensorDigitorumLeft").val();
           formData.Musculoskeletal_Mmt_ExtensorDigitorumRight = $("#Musculoskeletal_Mmt_ExtensorDigitorumRight").val();
           formData.Musculoskeletal_Mmt_ExtensorHallucisLeft = $("#Musculoskeletal_Mmt_ExtensorHallucisLeft").val();
           formData.Musculoskeletal_Mmt_ExtensorHallucisRight = $("#Musculoskeletal_Mmt_ExtensorHallucisRight").val();
           formData.Musculoskeletal_Mmt_PeroneiLeft = $("#Musculoskeletal_Mmt_PeroneiLeft").val();
           formData.Musculoskeletal_Mmt_PeroneiRight = $("#Musculoskeletal_Mmt_PeroneiRight").val();
           formData.Musculoskeletal_Mmt_FlexorDigitorumLeft = $("#Musculoskeletal_Mmt_FlexorDigitorumLeft").val();
           formData.Musculoskeletal_Mmt_FlexorDigitorumRight = $("#Musculoskeletal_Mmt_FlexorDigitorumRight").val();
           formData.Musculoskeletal_Mmt_FlexorHallucisLeft = $("#Musculoskeletal_Mmt_FlexorHallucisLeft").val();
           formData.Musculoskeletal_Mmt_FlexorHallucisRight = $("#Musculoskeletal_Mmt_FlexorHallucisRight").val();
           formData.Musculoskeletal_Mmt_AnteriorDeltoidLeft = $("#Musculoskeletal_Mmt_AnteriorDeltoidLeft").val();
           formData.Musculoskeletal_Mmt_AnteriorDeltoidRight = $("#Musculoskeletal_Mmt_AnteriorDeltoidRight").val();
           formData.Musculoskeletal_Mmt_PosteriorDeltoidLeft = $("#Musculoskeletal_Mmt_PosteriorDeltoidLeft").val();
           formData.Musculoskeletal_Mmt_PosteriorDeltoidRight = $("#Musculoskeletal_Mmt_PosteriorDeltoidRight").val();
           formData.Musculoskeletal_Mmt_MiddleDeltoidLeft = $("#Musculoskeletal_Mmt_MiddleDeltoidLeft").val();
           formData.Musculoskeletal_Mmt_MiddleDeltoidRight = $("#Musculoskeletal_Mmt_MiddleDeltoidRight").val();
           formData.Musculoskeletal_Mmt_SupraspinatusLeft = $("#Musculoskeletal_Mmt_SupraspinatusLeft").val();
           formData.Musculoskeletal_Mmt_SupraspinatusRight = $("#Musculoskeletal_Mmt_SupraspinatusRight").val();
           formData.Musculoskeletal_Mmt_SerratusAnteriorLeft = $("#Musculoskeletal_Mmt_SerratusAnteriorLeft").val();
           formData.Musculoskeletal_Mmt_SerratusAnteriorRight = $("#Musculoskeletal_Mmt_SerratusAnteriorRight").val();
           formData.Musculoskeletal_Mmt_RhomboidsLeft = $("#Musculoskeletal_Mmt_RhomboidsLeft").val();
           formData.Musculoskeletal_Mmt_RhomboidsRight = $("#Musculoskeletal_Mmt_RhomboidsRight").val();
           formData.Musculoskeletal_Mmt_BicepsLeft = $("#Musculoskeletal_Mmt_BicepsLeft").val();
           formData.Musculoskeletal_Mmt_BicepsRight = $("#Musculoskeletal_Mmt_BicepsRight").val();
           formData.Musculoskeletal_Mmt_TricepsLeft = $("#Musculoskeletal_Mmt_TricepsLeft").val();
           formData.Musculoskeletal_Mmt_TricepsRight = $("#Musculoskeletal_Mmt_TricepsRight").val();
           formData.Musculoskeletal_Mmt_SupinatorLeft = $("#Musculoskeletal_Mmt_SupinatorLeft").val();
           formData.Musculoskeletal_Mmt_SupinatorRight = $("#Musculoskeletal_Mmt_SupinatorRight").val();
           formData.Musculoskeletal_Mmt_PronatorLeft = $("#Musculoskeletal_Mmt_PronatorLeft").val();
           formData.Musculoskeletal_Mmt_PronatorRight = $("#Musculoskeletal_Mmt_PronatorRight").val();
           formData.Musculoskeletal_Mmt_ECULeft = $("#Musculoskeletal_Mmt_ECULeft").val();
           formData.Musculoskeletal_Mmt_ECURight = $("#Musculoskeletal_Mmt_ECURight").val();
           formData.Musculoskeletal_Mmt_ECRLeft = $("#Musculoskeletal_Mmt_ECRLeft").val();
           formData.Musculoskeletal_Mmt_ECRRight = $("#Musculoskeletal_Mmt_ECRRight").val();
           formData.Musculoskeletal_Mmt_ECSLeft = $("#Musculoskeletal_Mmt_ECSLeft").val();
           formData.Musculoskeletal_Mmt_ECSRight = $("#Musculoskeletal_Mmt_ECSRight").val();
           formData.Musculoskeletal_Mmt_FCULeft = $("#Musculoskeletal_Mmt_FCULeft").val();
           formData.Musculoskeletal_Mmt_FCURight = $("#Musculoskeletal_Mmt_FCURight").val();
           formData.Musculoskeletal_Mmt_FCRLeft = $("#Musculoskeletal_Mmt_FCRLeft").val();
           formData.Musculoskeletal_Mmt_FCRRight = $("#Musculoskeletal_Mmt_FCRRight").val();
           formData.Musculoskeletal_Mmt_FCSLeft = $("#Musculoskeletal_Mmt_FCSLeft").val();
           formData.Musculoskeletal_Mmt_FCSRight = $("#Musculoskeletal_Mmt_FCSRight").val();

           // Thumb
           formData.Musculoskeletal_Mmt_OpponensPollicisLeft = $("#Musculoskeletal_Mmt_OpponensPollicisLeft").val();
           formData.Musculoskeletal_Mmt_OpponensPollicisRight = $("#Musculoskeletal_Mmt_OpponensPollicisRight").val();
           formData.Musculoskeletal_Mmt_FlexorPollicisLeft = $("#Musculoskeletal_Mmt_FlexorPollicisLeft").val();
           formData.Musculoskeletal_Mmt_FlexorPollicisRight = $("#Musculoskeletal_Mmt_FlexorPollicisRight").val();
           formData.Musculoskeletal_Mmt_AbductorPollicisLeft = $("#Musculoskeletal_Mmt_AbductorPollicisLeft").val();
           formData.Musculoskeletal_Mmt_AbductorPollicisRight = $("#Musculoskeletal_Mmt_AbductorPollicisRight").val();
           formData.Musculoskeletal_Mmt_ExtensorPollicisLeft = $("#Musculoskeletal_Mmt_ExtensorPollicisLeft").val();
           formData.Musculoskeletal_Mmt_ExtensorPollicisRight = $("#Musculoskeletal_Mmt_ExtensorPollicisRight").val();

           saveWithModal(formData, reloadAfterSave, "Musculoskeletal");
       }

       function SaveTab10(reloadAfterSave) {
           var formData = {};
           formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 10;

        formData.SignOfCNS_NeuromotorControl = $("#SignOfCNS_NeuromotorControl").val();

           saveWithModal(formData, reloadAfterSave, "Sign of CNS");
    }

      function SaveTab11(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 11;

        // -------- GENERAL ----------
        formData.RemarkVariable_SustainGeneral = $("#RemarkVariable_SustainGeneral").val();
        formData.RemarkVariable_ContractionsGeneral = $("#RemarkVariable_ContractionsGeneral").val();
        formData.RemarkVariable_AntagonistGeneral = $("#RemarkVariable_AntagonistGeneral").val();
        formData.RemarkVariable_SynergyGeneral = $("#RemarkVariable_SynergyGeneral").val();
        formData.RemarkVariable_PosturalGeneral = $("#RemarkVariable_PosturalGeneral").val();
        formData.RemarkVariable_StiffinessGeneral = $("#RemarkVariable_StiffinessGeneral").val();
        formData.RemarkVariable_ExtraneousGeneral = $("#RemarkVariable_ExtraneousGeneral").val();

        // -------- CONTROL ----------
        formData.RemarkVariable_SustainControl = $("#RemarkVariable_SustainControl").val();
        formData.RemarkVariable_PosturalControl = $("#RemarkVariable_PosturalControl").val();
        formData.RemarkVariable_ContractionsControl = $("#RemarkVariable_ContractionsControl").val();
        formData.RemarkVariable_AntagonistControl = $("#RemarkVariable_AntagonistControl").val();
        formData.RemarkVariable_SynergyControl = $("#RemarkVariable_SynergyControl").val();
        formData.RemarkVariable_StiffinessControl = $("#RemarkVariable_StiffinessControl").val();
        formData.RemarkVariable_ExtraneousControl = $("#RemarkVariable_ExtraneousControl").val();

        // -------- TIMING ----------
        formData.RemarkVariable_SustainTiming = $("#RemarkVariable_SustainTiming").val();
        formData.RemarkVariable_PosturalTiming = $("#RemarkVariable_PosturalTiming").val();
        formData.RemarkVariable_ContractionsTiming = $("#RemarkVariable_ContractionsTiming").val();
        formData.RemarkVariable_AntagonistTiming = $("#RemarkVariable_AntagonistTiming").val();
        formData.RemarkVariable_SynergyTiming = $("#RemarkVariable_SynergyTiming").val();
        formData.RemarkVariable_StiffinessTiming = $("#RemarkVariable_StiffinessTiming").val();
        formData.RemarkVariable_ExtraneousTiming = $("#RemarkVariable_ExtraneousTiming").val();

          saveWithModal(formData, reloadAfterSave, "Remarks Variable");
    }

      function SaveTab12(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 12;

        formData.SensorySystem_Vision = $("#SensorySystem_Vision").val();
        formData.SensorySystem_Somatosensory = $("#SensorySystem_Somatosensory").val();
        formData.SensorySystem_Vestibular = $("#SensorySystem_Vestibular").val();
        formData.SensorySystem_Auditory = $("#SensorySystem_Auditory").val();
        formData.SensorySystem_Gustatory = $("#SensorySystem_Gustatory").val();

          saveWithModal(formData, reloadAfterSave, "Sensory System");
    }

      function SaveTab13(reloadAfterSave) {

           var formData = {};
           formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 13;

           formData.SensoryProfile_Profile = $("#SensoryProfile_Profile").val();

           var list = [];
           var sr = 1;

           // Loop all Name textboxes inside repeater
           $("input[id*='txtSensory_Profile_NameResults_Name']").each(function () {

               var name = $(this).val() || "";

               // Find Result textbox from same row
               var result = $(this).closest("tr").find("input[id*='txtSensory_Profile_NameResults_Result']").val() || "";

               if ($.trim(name) !== "" || $.trim(result) !== "") {
                   list.push({
                       SR_NO: sr,
                       NAME: $.trim(name),
                       RESULTS: $.trim(result)
                   });
                   sr++;
               }
           });

           formData.Sensory_Profile_NameResults = JSON.stringify(list);


          saveWithModal(formData, reloadAfterSave, "Sensory Profile");
       }


    function SaveTab14(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 14;

        formData.SIPTInfo_History = $("#SIPTInfo_History").val();

        formData.SIPTInfo_HandFunction1_GraspRight = $("#SIPTInfo_HandFunction1_GraspRight").val();
        formData.SIPTInfo_HandFunction1_GraspLeft = $("#SIPTInfo_HandFunction1_GraspLeft").val();
        formData.SIPTInfo_HandFunction1_SphericalRight = $("#SIPTInfo_HandFunction1_SphericalRight").val();
        formData.SIPTInfo_HandFunction1_SphericalLeft = $("#SIPTInfo_HandFunction1_SphericalLeft").val();
        formData.SIPTInfo_HandFunction1_HookRight = $("#SIPTInfo_HandFunction1_HookRight").val();
        formData.SIPTInfo_HandFunction1_HookLeft = $("#SIPTInfo_HandFunction1_HookLeft").val();
        formData.SIPTInfo_HandFunction1_JawChuckRight = $("#SIPTInfo_HandFunction1_JawChuckRight").val();
        formData.SIPTInfo_HandFunction1_JawChuckLeft = $("#SIPTInfo_HandFunction1_JawChuckLeft").val();
        formData.SIPTInfo_HandFunction1_GripRight = $("#SIPTInfo_HandFunction1_GripRight").val();
        formData.SIPTInfo_HandFunction1_GripLeft = $("#SIPTInfo_HandFunction1_GripLeft").val();
        formData.SIPTInfo_HandFunction1_ReleaseRight = $("#SIPTInfo_HandFunction1_ReleaseRight").val();
        formData.SIPTInfo_HandFunction1_ReleaseLeft = $("#SIPTInfo_HandFunction1_ReleaseLeft").val();

        formData.SIPTInfo_HandFunction2_OppositionLfR = $("#SIPTInfo_HandFunction2_OppositionLfR").val();
        formData.SIPTInfo_HandFunction2_OppositionLfL = $("#SIPTInfo_HandFunction2_OppositionLfL").val();
        formData.SIPTInfo_HandFunction2_OppositionMFR = $("#SIPTInfo_HandFunction2_OppositionMFR").val();
        formData.SIPTInfo_HandFunction2_OppositionMFL = $("#SIPTInfo_HandFunction2_OppositionMFL").val();
        formData.SIPTInfo_HandFunction2_OppositionRFR = $("#SIPTInfo_HandFunction2_OppositionRFR").val();
        formData.SIPTInfo_HandFunction2_OppositionRFL = $("#SIPTInfo_HandFunction2_OppositionRFL").val();

        formData.SIPTInfo_HandFunction2_PinchLfR = $("#SIPTInfo_HandFunction2_PinchLfR").val();
        formData.SIPTInfo_HandFunction2_PinchLfL = $("#SIPTInfo_HandFunction2_PinchLfL").val();
        formData.SIPTInfo_HandFunction2_PinchMFR = $("#SIPTInfo_HandFunction2_PinchMFR").val();
        formData.SIPTInfo_HandFunction2_PinchMFL = $("#SIPTInfo_HandFunction2_PinchMFL").val();
        formData.SIPTInfo_HandFunction2_PinchRFR = $("#SIPTInfo_HandFunction2_PinchRFR").val();
        formData.SIPTInfo_HandFunction2_PinchRFL = $("#SIPTInfo_HandFunction2_PinchRFL").val();

        formData.SIPTInfo_SIPT3_Spontaneous = $("#SIPTInfo_SIPT3_Spontaneous").val();
        formData.SIPTInfo_SIPT3_Command = $("#SIPTInfo_SIPT3_Command").val();

        formData.SIPTInfo_SIPT4_Kinesthesia = $("#SIPTInfo_SIPT4_Kinesthesia").val();
        formData.SIPTInfo_SIPT4_Finger = $("#SIPTInfo_SIPT4_Finger").val();
        formData.SIPTInfo_SIPT4_Localisation = $("#SIPTInfo_SIPT4_Localisation").val();
        formData.SIPTInfo_SIPT4_DoubleTactile = $("#SIPTInfo_SIPT4_DoubleTactile").val();
        formData.SIPTInfo_SIPT4_Tactile = $("#SIPTInfo_SIPT4_Tactile").val();
        formData.SIPTInfo_SIPT4_Graphesthesia = $("#SIPTInfo_SIPT4_Graphesthesia").val();
        formData.SIPTInfo_SIPT4_PostRotary = $("#SIPTInfo_SIPT4_PostRotary").val();
        formData.SIPTInfo_SIPT4_Standing = $("#SIPTInfo_SIPT4_Standing").val();

        formData.SIPTInfo_SIPT5_Color = $("#SIPTInfo_SIPT5_Color").val();
        formData.SIPTInfo_SIPT5_Form = $("#SIPTInfo_SIPT5_Form").val();
        formData.SIPTInfo_SIPT5_Size = $("#SIPTInfo_SIPT5_Size").val();
        formData.SIPTInfo_SIPT5_Depth = $("#SIPTInfo_SIPT5_Depth").val();
        formData.SIPTInfo_SIPT5_Figure = $("#SIPTInfo_SIPT5_Figure").val();
        formData.SIPTInfo_SIPT5_Motor = $("#SIPTInfo_SIPT5_Motor").val();

        formData.SIPTInfo_SIPT6_Design = $("#SIPTInfo_SIPT6_Design").val();
        formData.SIPTInfo_SIPT6_Constructional = $("#SIPTInfo_SIPT6_Constructional").val();

        formData.SIPTInfo_SIPT7_Scanning = $("#SIPTInfo_SIPT7_Scanning").val();
        formData.SIPTInfo_SIPT7_Memory = $("#SIPTInfo_SIPT7_Memory").val();

        formData.SIPTInfo_SIPT8_Postural = $("#SIPTInfo_SIPT8_Postural").val();
        formData.SIPTInfo_SIPT8_Oral = $("#SIPTInfo_SIPT8_Oral").val();
        formData.SIPTInfo_SIPT8_Sequencing = $("#SIPTInfo_SIPT8_Sequencing").val();
        formData.SIPTInfo_SIPT8_Commands = $("#SIPTInfo_SIPT8_Commands").val();

        formData.SIPTInfo_SIPT9_Bilateral = $("#SIPTInfo_SIPT9_Bilateral").val();
        formData.SIPTInfo_SIPT9_Contralat = $("#SIPTInfo_SIPT9_Contralat").val();
        formData.SIPTInfo_SIPT9_PreferredHand = $("#SIPTInfo_SIPT9_PreferredHand").val();
        formData.SIPTInfo_SIPT9_CrossingMidline = $("#SIPTInfo_SIPT9_CrossingMidline").val();

        formData.SIPTInfo_SIPT10_Draw = $("#SIPTInfo_SIPT10_Draw").val();
        formData.SIPTInfo_SIPT10_ClockFace = $("#SIPTInfo_SIPT10_ClockFace").val();
        formData.SIPTInfo_SIPT10_Filtering = $("#SIPTInfo_SIPT10_Filtering").val();
        formData.SIPTInfo_SIPT10_MotorPlanning = $("#SIPTInfo_SIPT10_MotorPlanning").val();
        formData.SIPTInfo_SIPT10_BodyImage = $("#SIPTInfo_SIPT10_BodyImage").val();
        formData.SIPTInfo_SIPT10_BodySchema = $("#SIPTInfo_SIPT10_BodySchema").val();
        formData.SIPTInfo_SIPT10_Laterality = $("#SIPTInfo_SIPT10_Laterality").val();

        formData.SIPTInfo_ActivityGiven_Remark = $("#SIPTInfo_ActivityGiven_Remark").val();
        formData.SIPTInfo_ActivityGiven_InterestActivity = $("#SIPTInfo_ActivityGiven_InterestActivity").val();
        formData.SIPTInfo_ActivityGiven_InterestCompletion = $("#SIPTInfo_ActivityGiven_InterestCompletion").val();
        formData.SIPTInfo_ActivityGiven_Learning = $("#SIPTInfo_ActivityGiven_Learning").val();
        formData.SIPTInfo_ActivityGiven_Complexity = $("#SIPTInfo_ActivityGiven_Complexity").val();
        formData.SIPTInfo_ActivityGiven_ProblemSolving = $("#SIPTInfo_ActivityGiven_ProblemSolving").val();
        formData.SIPTInfo_ActivityGiven_Concentration = $("#SIPTInfo_ActivityGiven_Concentration").val();
        formData.SIPTInfo_ActivityGiven_Retension = $("#SIPTInfo_ActivityGiven_Retension").val();
        formData.SIPTInfo_ActivityGiven_SpeedPerfom = $("#SIPTInfo_ActivityGiven_SpeedPerfom").val();
        formData.SIPTInfo_ActivityGiven_Neatness = $("#SIPTInfo_ActivityGiven_Neatness").val();
        formData.SIPTInfo_ActivityGiven_Frustation = $("#SIPTInfo_ActivityGiven_Frustation").val();
        formData.SIPTInfo_ActivityGiven_Work = $("#SIPTInfo_ActivityGiven_Work").val();
        formData.SIPTInfo_ActivityGiven_Reaction = $("#SIPTInfo_ActivityGiven_Reaction").val();
        formData.SIPTInfo_ActivityGiven_SociabilityTherapist = $("#SIPTInfo_ActivityGiven_SociabilityTherapist").val();
        formData.SIPTInfo_ActivityGiven_SociabilityStudents = $("#SIPTInfo_ActivityGiven_SociabilityStudents").val();
        saveWithModal(formData, reloadAfterSave, "SIPT Information");
    }

    function SaveTab15(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 15;

        formData.Cognition_Intelligence = $("#Cognition_Intelligence").val();
        formData.Cognition_Attention = $("#Cognition_Attention").val();
        formData.Cognition_Memory = $("#Cognition_Memory").val();
        formData.Cognition_Adaptibility = $("#Cognition_Adaptibility").val();
        formData.Cognition_MotorPlanning = $("#Cognition_MotorPlanning").val();
        formData.Cognition_ExecutiveFunction = $("#Cognition_ExecutiveFunction").val();
        formData.Cognition_CognitiveFunctions = $("#Cognition_CognitiveFunctions").val();

        saveWithModal(formData, reloadAfterSave, "Cognition");
    }

    function SaveTab16(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 16;

        formData.Integumentary_SkinIntegrity = $("#Integumentary_SkinIntegrity").val();
        formData.Integumentary_SkinColor = $("#Integumentary_SkinColor").val();
        formData.Integumentary_SkinExtensibility = $("#Integumentary_SkinExtensibility").val();

        saveWithModal(formData, reloadAfterSave, "Integumentary");
    }

    function SaveTab17(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 17;

        formData.Respiratory_RateResting = $("#Respiratory_RateResting").val();
        formData.Respiratory_PostExercise = $("#Respiratory_PostExercise").val();
        formData.Respiratory_Patterns = $("#Respiratory_Patterns").val();
        formData.Respiratory_BreathControl = $("#Respiratory_BreathControl").val();

        saveWithModal(formData, reloadAfterSave, "Respiratory");
    }

    function SaveTab18(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 18;

        formData.Cardiovascular_HeartRate = $("#Cardiovascular_HeartRate").val();
        formData.Cardiovascular_PostExercise = $("#Cardiovascular_PostExercise").val();
        formData.Cardiovascular_BP = $("#Cardiovascular_BP").val();
        formData.Cardiovascular_Edema = $("#Cardiovascular_Edema").val();
        formData.Cardiovascular_Circulation = $("#Cardiovascular_Circulation").val();
        formData.Cardiovascular_EEi = $("#Cardiovascular_EEi").val();

        saveWithModal(formData, reloadAfterSave, "Cardiovascular");
    }

    function SaveTab19(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 19;

        formData.Gastrointestinal_Bowel = $("#Gastrointestinal_Bowel").val();
        formData.Gastrointestinal_Intake = $("#Gastrointestinal_Intake").val();

        saveWithModal(formData, reloadAfterSave, "Gastrointestinal");
    }

    function SaveTab20(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
        formData.TabNo = 20;

        formData.Evaluation_Strengths = $("#Evaluation_Strengths").val();
        formData.Evalutionadding_Strengths = $("#Evalutionadding_Strengths").val();

        formData.Evaluation_Concern_Barriers = $("#Evaluation_Concern_Barriers").val();
        formData.Evaluation_Concern_Limitations = $("#Evaluation_Concern_Limitations").val();
        formData.Evaluation_Concern_Posture = $("#Evaluation_Concern_Posture").val();
        formData.Evaluation_Concern_Impairment = $("#Evaluation_Concern_Impairment").val();

        formData.Evaluation_Goal_Summary = $("#Evaluation_Goal_Summary").val();
        formData.Evaluation_Goal_ShortTearm_Previous = $("#Evaluation_Goal_ShortTearm_Previous").val();
        formData.Evaluation_Goal_Previous = $("#Evaluation_Goal_Previous").val();
        formData.Evaluation_Goal_LongTerm = $("#Evaluation_Goal_LongTerm").val();
        formData.Evaluation_Goal_ShortTerm = $("#Evaluation_Goal_ShortTerm").val();
        formData.Evaluation_Goal_Impairment = $("#Evaluation_Goal_Impairment").val();

        formData.Evaluation_Plan_Frequency = $("#Evaluation_Plan_Frequency").val();
        formData.Evaluation_Plan_Service = $("#Evaluation_Plan_Service").val();
        formData.Evaluation_Plan_Strategies = $("#Evaluation_Plan_Strategies").val();
        formData.Evaluation_Plan_Equipment = $("#Evaluation_Plan_Equipment").val();
        formData.Evaluation_Plan_Education = $("#Evaluation_Plan_Education").val();

        saveWithModal(formData, reloadAfterSave, "Evaluation");
    }

    function SaveTab21(reloadAfterSave) {
        var formData = {};
        formData.Record = "<%= Request.QueryString["record"] ?? "" %>";
           formData.TabNo = 21;

           formData.Doctor_Physioptherapist = $("#Doctor_Physioptherapist").val();
           formData.Doctor_Occupational = $("#Doctor_Occupational").val();
           formData.Doctor_EnterReport = "";

        saveWithModal(formData, reloadAfterSave, "Doctor");
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
               url: "<%= ResolveUrl("~/Handler/SaveRptNDTReval_New.ashx") %>",
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
      // MULTI SELECT HELPER
      // =========================
      function GetMultiSelectValues(selector) {
          var values = [];
          $(selector + " option:selected").each(function () {
              values.push($(this).val());
          });
          return values.join(",");
      }
  </script>



     <%--<script>
         // Check if the page is already open in another tab
         if (sessionStorage.getItem('pageOpened')) {
             // Display a warning message
             alert('This page is already open in another tab.');
             // Redirect or take appropriate action
             window.location.href = '/SessionRpt/RevalView.aspx'; // Redirect to another page
         } else {
             // Set a flag in sessionStorage indicating that the page is open
             sessionStorage.setItem('pageOpened', 'true');
             // Add an event listener to handle tab close events
             window.addEventListener('beforeunload', function () {
                 // Clear the flag when the tab is closed
                 sessionStorage.removeItem('pageOpened');
             });
         }
     </script>--%>

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
        } else {
            // Execute code for non-mobile devices
            // Use your existing code or another approach suitable for non-mobile devices
            console.log('Non-mobile resolution detected.');

            if (sessionStorage.getItem('pageOpened')) {
                // Display a warning message
                alert('This page is already open in another tab.');
                // Redirect or take appropriate action
                window.location.href = '/SessionRpt/RevalView.aspx'; // Redirect to another page
            } else {
                // Set a flag in sessionStorage indicating that the page is open
                sessionStorage.setItem('pageOpened', 'true');
                // Add an event listener to handle tab close events
                window.addEventListener('beforeunload', function () {
                    // Clear the flag when the tab is closed
                    sessionStorage.removeItem('pageOpened');
                });
            }
            // Use your existing logic or alternative approaches here
        }

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


    </script>

</asp:Content>
