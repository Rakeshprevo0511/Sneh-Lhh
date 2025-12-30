<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Site.master" AutoEventWireup="true" CodeBehind="Demo_SiRpt.aspx.cs" Inherits="snehrehab.SessionRpt.Demo_SiRpt" %>
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

<script type="text/javascript">
    $(function() {
        var maxLines = 8; var maxChar = 800;
        $('div.char-line-limiter textarea').keyup(function(e) {
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
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                SI Report :</div>
            <%--<div class="pull-right">
                <a href="/SessionRpt/Demo_SiView.aspx" class="btn btn-primary">View List</a>
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
                        <asp:TextBox ID="txtSession" runat="server"  CssClass="span4" Enabled="False"></asp:TextBox>
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
                        <asp:CheckBox ID="txtFinal" runat="server" Enabled="false" />
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
                        <asp:TextBox ID="txtGivenDate" runat="server" Enabled="false" CssClass="span2 my-datepicker"></asp:TextBox>
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
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-danger" Text=" Submit "
                            OnClientClick="DisableOnSubmit(this);" OnClick="btnSubmit_Click"></asp:LinkButton>
                        &nbsp; <a href='<%= _cancelUrl %>' class="btn btn-default">Cancel</a>
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
                </div>
            </div>
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
                                                1. Birth History :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="DataCollection_Referral" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                    Rows="8"></asp:TextBox>
                                            </div>
                                            <div class="clearfix">
                                            </div>
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
                                                <asp:TextBox ID="DataCollection_MedicalHistory" runat="server" Enabled="false" CssClass="span10"
                                                    TextMode="MultiLine" Rows="8"></asp:TextBox>
                                            </div>
                                            <div class="clearfix">
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Daily Routine :
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
                                                4. Milestones Achieved till now :
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
                                                5. Investigations like MRI, BERA, TSH, Karyotype test, Opthalmology test were done
                                                :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="DataCollection_TherapyHistory" runat="server" Enabled="false" CssClass="span10"
                                                    TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow hidden">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Sources at this facility or other :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="DataCollection_Sources" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow hidden">
                                        <div class="span12">
                                            <div class="control-label">
                                                7. Number of visit since last evaluation :
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
                                                6. Adapted Equipment/Assistive Technology :
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
                        <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Morphology" Visible="false">
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
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="Morphology_LimbLeft" Enabled="false" CssClass="span2"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="Morphology_LimbRight" Enabled="false" CssClass="span2"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox runat="server" ID="Morphology_LimbLength" Enabled="false" CssClass="span4"></asp:TextBox>
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
                                    <div class="formRow">
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
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelRight_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelLeft_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthRight_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthLeft_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        At Elbow
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelRight_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelLeft_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthRight_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthLeft_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Below Elbow
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelRight_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbLevelLeft_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthRight_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_UpperLimbGirthLeft_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="clearfix">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
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
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelRight_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelLeft_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthRight_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthLeft_ABV" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        At Knee
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelRight_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelLeft_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthRight_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthLeft_AT" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Below Knee
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelRight_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbLevelLeft_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthRight_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Morphology_LowerLimbGirthLeft_BLW" runat="server" Enabled="false" CssClass="span2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="clearfix">
                                            </div>
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
                                        <div class="span11">
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <b>Functional Abilities</b><br />
                                                            <small>(what do you observe the child doing? How much assistance do they need to do
                                                                these tasks/skills/movements? Keep it pertinent to the current episode of care and
                                                                discipline specific) GM, FM, Oral motor, speech and language.</small>
                                                        </td>
                                                        <td>
                                                            <b>Functional Limitations</b>
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Gross Motor</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalAbilities_GrossMotor" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalLimitations_GrossMotor" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Fine Motor</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalAbilities_FineMotor" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalLimitations_FineMotor" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Communication</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalAbilities_Communication" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalLimitations_Communication" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Cognition</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalAbilities_Cognitive" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalLimitations_Cognitive" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Behaviour</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalAbilities_Behaviour" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="FunctionalLimitations_Behaviour" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                                1. ADL :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FunctionalActivities_ADL" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Communication :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FunctionalActivities_Communication" runat="server" Enabled="false" CssClass="span10"
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
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <b>Participation Abilities</b>
                                                        </td>
                                                        <td>
                                                            <b>Participation Limitations</b>
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Gross Motor</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationAbilities_GrossMotor" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationLimitations_GrossMotor" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Fine Motor</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationAbilities_FineMotor" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationLimitations_FineMotor" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Communication</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationAbilities_Communication" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationLimitations_Communication" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Cognition</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationAbilities_Cognitive" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationLimitations_Cognitive" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <b>Behaviour</b></label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationAbilities_Behaviour" runat="server" Enabled="false" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ParticipationLimitations_Behaviour" runat="server" Enabled="false" class="span5"
                                                                TextMode="MultiLine"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="TabPanel20" runat="server" HeaderText="Family Structure">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. No of caregivers :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_NoOfCaregivers" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Time spent with the child: Daily :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_TimeWithChild" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Sunday/Holidays :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Holiday" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Willingness to devote time for therapy :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_DivoteTime" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Contextual factor : Economic :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_ContextualFactor" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Social :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Social" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                7. Environment :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Environment" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                8. Acceptance of condition :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Acceptance" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                9. Accessibility to play areas/ extra circular activities :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Accessibility" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                10. Comparison with sibling/cousin :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_CompareSibling" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                11. Working parents/ household help :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_Working" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                12. Family pressure for performance :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_FamilyPressure" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                13. With whom/who does the child spend most of his day? :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_SpentMostTime" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                14. Name of others closely involved with child :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_CloselyInvolved" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                15. How does your child choose to use his/her free time :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_ChooseFreeTime" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                16. Does your child play appropriately with toys? :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStru_PlayWithToys_1" runat="server" onclick="FamilyStru_PlayWithToys_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStru_PlayWithToys_2" runat="server" onclick="FamilyStru_PlayWithToys_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function FamilyStru_PlayWithToys_1_Click() {
                                                        var ctl = $('#<%=FamilyStru_PlayWithToys_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStru_PlayWithToys_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStru_PlayWithToys_2_Click() {
                                                        var ctl = $('#<%=FamilyStru_PlayWithToys_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStru_PlayWithToys_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                • If no explain :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="FamilyStru_ToysExplain" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                17. Does the child Throw tantrum? :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="FamilyStru_ThrowTantrum_1" runat="server" onclick="FamilyStru_ThrowTantrum_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="FamilyStru_ThrowTantrum_2" runat="server" onclick="FamilyStru_ThrowTantrum_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function FamilyStru_ThrowTantrum_1_Click() {
                                                        var ctl = $('#<%=FamilyStru_ThrowTantrum_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStru_ThrowTantrum_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function FamilyStru_ThrowTantrum_2_Click() {
                                                        var ctl = $('#<%=FamilyStru_ThrowTantrum_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=FamilyStru_ThrowTantrum_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
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
                        <ajaxToolkit:TabPanel ID="TabPanel21" runat="server" HeaderText="School Information">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Type of school :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="SchoolInfo_SchoolType_1" runat="server" onclick="SchoolInfo_SchoolType_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Open" />
                                                <asp:CheckBox ID="SchoolInfo_SchoolType_2" runat="server" onclick="SchoolInfo_SchoolType_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Integrated" />
                                                <asp:CheckBox ID="SchoolInfo_SchoolType_3" runat="server" onclick="SchoolInfo_SchoolType_3_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Special" />
                                                <script type="text/javascript">
                                                    function SchoolInfo_SchoolType_1_Click() {
                                                        var ctl = $('#<%=SchoolInfo_SchoolType_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SchoolInfo_SchoolType_2.ClientID %>').prop('checked', false);
                                                            $('#<%=SchoolInfo_SchoolType_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SchoolInfo_SchoolType_2_Click() {
                                                        var ctl = $('#<%=SchoolInfo_SchoolType_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SchoolInfo_SchoolType_1.ClientID %>').prop('checked', false);
                                                            $('#<%=SchoolInfo_SchoolType_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function SchoolInfo_SchoolType_3_Click() {
                                                        var ctl = $('#<%=SchoolInfo_SchoolType_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=SchoolInfo_SchoolType_1.ClientID %>').prop('checked', false);
                                                            $('#<%=SchoolInfo_SchoolType_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. How many hours :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Hours" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. How do they travel :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Traveling" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Child teacher ratio :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Teachers" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Seating arrangement :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_SeatingArr" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Sitting tolerance of child :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_SeatingTol" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                7. Meal time/ help required/ sharing food :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_MeanTime" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                8. Friend/ social interaction :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_FriendInteraction" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <h5>
                                                    9. Participate in :</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                a. Sports :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Sports" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                b. Extra-curricular :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Curricular" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                c. Cultural :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_Cultural" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                10. Shadow teacher required :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_ShadowTeacher" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                11. Remark from teacher :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_RemarkTeacher" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                12. Copying from board :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_CopyBoard" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                13. Completing CW/HW :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_CW_HW" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                14. Following instruction :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_FollowInstru" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                15. Provision of special educator/ remedial/ shadow teacher :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_SpecialEducator" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                16. Mode of delivery of info ppt / CD / board / video :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_DeliveryMode" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                17. Academic scope and syllabus :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="SchoolInfo_AcademicScope" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="TabPanel22" runat="server" HeaderText="Behavior">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <h5>
                                                    1. Behavior of child :</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                a. At home :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_AtHome" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                b. At school :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_AtSchool" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                c. With elders :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_WithElder" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                d. With peers :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_WithPeers" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                e. With teachers :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_WithTeacher" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                f. At the Mall :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_AtTheMall" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                g. At the playground :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Behaviour_AtPlayground" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <p>
                                                    (Stubborn / coperative / introvert / extrovert / bully / timid / aggressive/ Violent)</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <h5>
                                                    2. Play Behavor :</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                a. Constructive / Destructive :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPl_Constructive" runat="server" onclick="BehaviourPl_Constructive_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Constructive" />
                                                <asp:CheckBox ID="BehaviourPl_Destructive" runat="server" onclick="BehaviourPl_Destructive_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Destructive" />
                                                <script type="text/javascript">
                                                    function BehaviourPl_Constructive_Click() {
                                                        var ctl = $('#<%=BehaviourPl_Constructive.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPl_Destructive.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPl_Destructive_Click() {
                                                        var ctl = $('#<%=BehaviourPl_Destructive.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPl_Constructive.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="control-label span1">
                                                        Remark</div>
                                                    <asp:TextBox ID="BehaviourPl_CD_Remark" runat="server" Enabled="false" CssClass="span9" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                b. Independent / supervised :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_Independant" runat="server" onclick="BehaviourPL_Independant_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Independent" />
                                                <asp:CheckBox ID="BehaviourPL_Supervised" runat="server" onclick="BehaviourPL_Supervised_Click();"
                                                   Enabled="false"  CssClass="checkboes" Text=" Supervised" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_Independant_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Independant.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Supervised.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_Supervised_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Supervised.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Independant.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="control-label span1">
                                                        Remark</div>
                                                    <asp:TextBox ID="BehaviourPL_IS_Remark" runat="server" Enabled="false" CssClass="span9" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                c. Sedentary :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BehaviourPL_Sedentary" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                • on the go :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BehaviourPL_OnTheGo" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                d. Age appropriate :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_AgeAppropriate_1" runat="server" onclick="BehaviourPL_AgeAppropriate_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="BehaviourPL_AgeAppropriate_2" runat="server" onclick="BehaviourPL_AgeAppropriate_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_AgeAppropriate_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_AgeAppropriate_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_AgeAppropriate_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_AgeAppropriate_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_AgeAppropriate_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_AgeAppropriate_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                e. Follows rules :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_FollowRule_1" runat="server" onclick="BehaviourPL_FollowRule_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="BehaviourPL_FollowRule_2" runat="server" onclick="BehaviourPL_FollowRule_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_FollowRule_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_FollowRule_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_FollowRule_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_FollowRule_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_FollowRule_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_FollowRule_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                f. Bullies / Bullied :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_Bullied_1" runat="server" onclick="BehaviourPL_Bullied_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="BehaviourPL_Bullied_2" runat="server" onclick="BehaviourPL_Bullied_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_Bullied_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Bullied_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Bullied_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_Bullied_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Bullied_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Bullied_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                g. Types of play achieved :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BehaviourPL_PlayAchieved" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                h. Toys at home / choice :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BehaviourPL_ToyChoice" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                i. Repetitive / Versatile :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_Repetitive_1" runat="server" onclick="BehaviourPL_Repetitive_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Repetitive" />
                                                <asp:CheckBox ID="BehaviourPL_Repetitive_2" runat="server" onclick="BehaviourPL_Repetitive_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Versatile" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_Repetitive_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Repetitive_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Repetitive_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_Repetitive_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_Repetitive_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_Repetitive_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                j. Participate in group :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_PartInGroup_1" runat="server" onclick="BehaviourPL_PartInGroup_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="BehaviourPL_PartInGroup_2" runat="server" onclick="BehaviourPL_PartInGroup_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_PartInGroup_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_PartInGroup_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_PartInGroup_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_PartInGroup_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_PartInGroup_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_PartInGroup_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                k. Leaders / Follower :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="BehaviourPL_IsLeader_1" runat="server" onclick="BehaviourPL_IsLeader_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Leaders" />
                                                <asp:CheckBox ID="BehaviourPL_IsLeader_2" runat="server" onclick="BehaviourPL_IsLeader_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Follower" />
                                                <script type="text/javascript">
                                                    function BehaviourPL_IsLeader_1_Click() {
                                                        var ctl = $('#<%=BehaviourPL_IsLeader_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_IsLeader_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function BehaviourPL_IsLeader_2_Click() {
                                                        var ctl = $('#<%=BehaviourPL_IsLeader_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=BehaviourPL_IsLeader_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                l. Pretend play :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="BehaviourPL_PretendPlay" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                m. Regulation :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span10">
                                                        <div class="control-label">
                                                            Child regulates self :
                                                        </div>
                                                        <div class="control-group" style="padding-left: 20px">
                                                            <asp:CheckBox ID="Behaviour_RegulatesSelf_1" runat="server" onclick="Behaviour_RegulatesSelf_1_Click();"
                                                                Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                            <asp:CheckBox ID="Behaviour_RegulatesSelf_2" runat="server" onclick="Behaviour_RegulatesSelf_2_Click();"
                                                                Enabled="false" CssClass="checkboes" Text=" No" />
                                                            <script type="text/javascript">
                                                                function Behaviour_RegulatesSelf_1_Click() {
                                                                    var ctl = $('#<%=Behaviour_RegulatesSelf_1.ClientID %>')[0];
                                                                    if (ctl.checked) {
                                                                        $('#<%=Behaviour_RegulatesSelf_2.ClientID %>').prop('checked', false);
                                                                    }
                                                                }
                                                                function Behaviour_RegulatesSelf_2_Click() {
                                                                    var ctl = $('#<%=Behaviour_RegulatesSelf_2.ClientID %>')[0];
                                                                    if (ctl.checked) {
                                                                        $('#<%=Behaviour_RegulatesSelf_1.ClientID %>').prop('checked', false);
                                                                    }
                                                                }
                                                            </script>
                                                        </div>
                                                    </div>
                                                    <div class="span10">
                                                        <div class="control-label">
                                                            Any seeking behavior when not regulated :
                                                        </div>
                                                        <asp:TextBox ID="Behaviour_BehaveNotReg" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span10">
                                                        <div class="control-label">
                                                            What calm him/her down :
                                                        </div>
                                                        <asp:TextBox ID="Behaviour_WhatCalmDown" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                n. Happiness :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            likes :
                                                        </div>
                                                        <asp:TextBox ID="Behaviour_HappyLike" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Dislikes :
                                                        </div>
                                                        <asp:TextBox ID="Behaviour_HappyDislike" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
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
                        <ajaxToolkit:TabPanel ID="TabPanel23" runat="server" HeaderText="Arousal">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. State of alertness during eval :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_EvalAlert" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. General state of alertness :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_GeneralAlert" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Responses to stimuli :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_StimuliResponse" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Transition :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_Transition" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Optimum arousal :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_Optimum" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Alerting factor :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_AlertingFactor" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                7. Calming factor :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Arousal_CalmingFactor" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <p>
                                                    ( Light / sound / smell / unusual characteristics / thing ) OCD</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="TabPanel24" runat="server" HeaderText="Attention">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Focus attention to task at hand :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            In school :
                                                        </div>
                                                        <asp:TextBox ID="Attention_InSchool" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            At home :
                                                        </div>
                                                        <asp:TextBox ID="Attention_InHome" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Dividing Attention :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_Dividing" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Change of activities :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_ChangeActivities" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Age appropriate attention :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Attention_AgeAppropriate_1" runat="server" onclick="Attention_AgeAppropriate_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Attention_AgeAppropriate_2" runat="server" onclick="Attention_AgeAppropriate_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function Attention_AgeAppropriate_1_Click() {
                                                        var ctl = $('#<%=Attention_AgeAppropriate_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_AgeAppropriate_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Attention_AgeAppropriate_2_Click() {
                                                        var ctl = $('#<%=Attention_AgeAppropriate_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Attention_AgeAppropriate_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Attention span :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Attention_AttentionSpan" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Factors of distractibility :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            At home :
                                                        </div>
                                                        <asp:TextBox ID="Attention_Distractibility_Home" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            At school :
                                                        </div>
                                                        <asp:TextBox ID="Attention_Distractibility_School" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
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
                        <ajaxToolkit:TabPanel ID="TabPanel25" runat="server" HeaderText="Affect / Action">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <h5>
                                                    Affect :</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Wide range of emotions :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Affect_EmotionRange_1" runat="server" onclick="Affect_EmotionRange_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Affect_EmotionRange_2" runat="server" onclick="Affect_EmotionRange_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                    <asp:CheckBox ID="Affect_EmotionRangeSometime" runat="server" onclick="Affect_EmotionRangeSometime_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Sometime" />
                                                <script type="text/javascript">
                                                    function Affect_EmotionRange_1_Click() {
                                                        var ctl = $('#<%=Affect_EmotionRange_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_EmotionRange_2.ClientID %>').prop('checked', false);
                                                            $('#<%= Affect_EmotionRangeSometime.ClientID%>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_EmotionRange_2_Click() {
                                                        var ctl = $('#<%=Affect_EmotionRange_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_EmotionRange_1.ClientID %>').prop('checked', false);
                                                            $('#<%= Affect_EmotionRangeSometime.ClientID%>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_EmotionRangeSometime_Click() {
                                                        var ctl = $('#<%= Affect_EmotionRangeSometime.ClientID%>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_EmotionRange_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Affect_EmotionRange_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Is the child able to express emotion :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Affect_EmotionExpress_1" runat="server" onclick="Affect_EmotionExpress_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Affect_EmotionExpress_2" runat="server" onclick="Affect_EmotionExpress_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                    <asp:CheckBox ID="Affect_EmotionExpressSometime" runat="server" onclick="Affect_EmotionExpressSometime_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Sometime" />
                                                <script type="text/javascript">
                                                    function Affect_EmotionExpress_1_Click() {
                                                        var ctl = $('#<%=Affect_EmotionExpress_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_EmotionExpress_2.ClientID %>').prop('checked', false);
                                                            $('#<%= Affect_EmotionExpressSometime.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_EmotionExpress_2_Click() {
                                                        var ctl = $('#<%=Affect_EmotionExpress_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Affect_EmotionExpress_1.ClientID %>').prop('checked', false);
                                                            $('#<%= Affect_EmotionExpressSometime.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Affect_EmotionExpressSometime_Click() {
                                                        var ctl = $('#<%=Affect_EmotionExpressSometime.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%= Affect_EmotionExpress_2.ClientID%>').prop('checked', false);
                                                            $('#<%= Affect_EmotionExpress_1.ClientID%>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Affect appropriate to :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Environment :
                                                        </div>
                                                        <asp:TextBox ID="Affect_Environment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Task :
                                                        </div>
                                                        <asp:TextBox ID="Affect_Task" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Individual :
                                                        </div>
                                                        <asp:TextBox ID="Affect_Individual" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Consistent emotion throughout :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Consistent" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Factors characterising affects :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Affect_Characterising" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="span11 formRow">
                                        <div class="row">
                                            <div class="span12">
                                                <h5>
                                                    Action :</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Age appropriate action/ motor planning :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Action_Planning" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Purposeful :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_Purposeful_1" runat="server" onclick="Action_Purposeful_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_Purposeful_2" runat="server" onclick="Action_Purposeful_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
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
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Goal Oriented :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_GoalOriented_1" runat="server" onclick="Action_GoalOriented_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_GoalOriented_2" runat="server" onclick="Action_GoalOriented_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
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
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Feedback Dependent :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Action_FeedbackDependent_1" runat="server" onclick="Action_FeedbackDependent_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Action_FeedbackDependent_2" runat="server" onclick="Action_FeedbackDependent_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function Action_FeedbackDependent_1_Click() {
                                                        var ctl = $('#<%=Action_FeedbackDependent_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_FeedbackDependent_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Action_FeedbackDependent_2_Click() {
                                                        var ctl = $('#<%=Action_FeedbackDependent_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Action_FeedbackDependent_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
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
                        <ajaxToolkit:TabPanel ID="TabPanel26" runat="server" HeaderText="Social Interaction">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Interaction with / At :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Known People :
                                                        </div>
                                                        <asp:TextBox ID="Social_KnownPeople" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Strangers :
                                                        </div>
                                                        <asp:TextBox ID="Social_Strangers" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Social gathering :
                                                        </div>
                                                        <asp:TextBox ID="Social_Gathering" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <p>
                                                (Interacts/ Does not initiate/ Sustains / Fight/ Flight/ Freeze/ Fright)
                                            </p>
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Emotional Response: (Anxious/ Comfortable/ Nervous/ ANS response) :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Social_Emotional" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Understands / appreciates social cues :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Social_Appreciates_1" runat="server" onclick="Social_Appreciates_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Social_Appreciates_2" runat="server" onclick="Social_Appreciates_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function Social_Appreciates_1_Click() {
                                                        var ctl = $('#<%=Social_Appreciates_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Social_Appreciates_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Social_Appreciates_2_Click() {
                                                        var ctl = $('#<%=Social_Appreciates_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Social_Appreciates_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Reaction to emotion / of other :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Happiness :
                                                        </div>
                                                        <asp:TextBox ID="Social_Reaction" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Sadness :
                                                        </div>
                                                        <asp:TextBox ID="Social_Sadness" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Surprise :
                                                        </div>
                                                        <asp:TextBox ID="Social_Surprise" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Shock :
                                                        </div>
                                                        <asp:TextBox ID="Social_Shock" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Friendships : can make friends
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span9">
                                                        <div class="control-group" style="padding-left: 20px">
                                                            <asp:CheckBox ID="Social_Friendships_1" runat="server" onclick="Social_Friendships_1_Click();"
                                                                Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                            <asp:CheckBox ID="Social_Friendships_2" runat="server" onclick="Social_Friendships_2_Click();"
                                                                Enabled="false" CssClass="checkboes" Text=" No" />
                                                            <script type="text/javascript">
                                                                function Social_Friendships_1_Click() {
                                                                    var ctl = $('#<%=Social_Friendships_1.ClientID %>')[0];
                                                                    if (ctl.checked) {
                                                                        $('#<%=Social_Friendships_2.ClientID %>').prop('checked', false);
                                                                    }
                                                                }
                                                                function Social_Friendships_2_Click() {
                                                                    var ctl = $('#<%=Social_Friendships_2.ClientID %>')[0];
                                                                    if (ctl.checked) {
                                                                        $('#<%=Social_Friendships_1.ClientID %>').prop('checked', false);
                                                                    }
                                                                }
                                                            </script>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Relates to known people :
                                                        </div>
                                                        <asp:TextBox ID="Social_Relates" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            What activities they enjoy :
                                                        </div>
                                                        <asp:TextBox ID="Social_ActivitiestheyEnjoy" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
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
                        <ajaxToolkit:TabPanel ID="TabPanel27" runat="server" HeaderText="Communication">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. When did your child start to :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Speak :
                                                        </div>
                                                        <asp:TextBox ID="Communication_StartToSpeak" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Monosyllables :
                                                        </div>
                                                        <asp:TextBox ID="Communication_Monosyllables" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Bisyllables :
                                                        </div>
                                                        <asp:TextBox ID="Communication_Bisyllables" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Short Sentences :
                                                        </div>
                                                        <asp:TextBox ID="Communication_ShortSentences" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Long sentence :
                                                        </div>
                                                        <asp:TextBox ID="Communication_LongSentence" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Unusual sounds/ jargon speech :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Communication_UnusualSounds_1" runat="server" onclick="Communication_UnusualSounds_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Communication_UnusualSounds_2" runat="server" onclick="Communication_UnusualSounds_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function Communication_UnusualSounds_1_Click() {
                                                        var ctl = $('#<%=Communication_UnusualSounds_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_UnusualSounds_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Communication_UnusualSounds_2_Click() {
                                                        var ctl = $('#<%=Communication_UnusualSounds_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_UnusualSounds_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Imitation of speech / gestures :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Communication_ImitationOfSpeech_1" runat="server" onclick="Communication_ImitationOfSpeech_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Yes" />
                                                <asp:CheckBox ID="Communication_ImitationOfSpeech_2" runat="server" onclick="Communication_ImitationOfSpeech_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" No" />
                                                <script type="text/javascript">
                                                    function Communication_ImitationOfSpeech_1_Click() {
                                                        var ctl = $('#<%=Communication_ImitationOfSpeech_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_ImitationOfSpeech_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Communication_ImitationOfSpeech_2_Click() {
                                                        var ctl = $('#<%=Communication_ImitationOfSpeech_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_ImitationOfSpeech_1.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                4. Non verbal facial :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Expression :
                                                        </div>
                                                        <asp:TextBox ID="Communication_FacialExpression" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Eye contact :
                                                        </div>
                                                        <asp:TextBox ID="Communication_EyeContact" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Gestures :
                                                        </div>
                                                        <asp:TextBox ID="Communication_Gestures" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Interpretation of language :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Simple/ Complex :
                                                        </div>
                                                        <asp:TextBox ID="Communication_InterpretationOfLanguage" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Understand implied meaning :
                                                        </div>
                                                        <asp:TextBox ID="Communication_UnderstandImplied" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Understand joke/ sarcasm :
                                                        </div>
                                                        <asp:TextBox ID="Communication_UnderstandJoke" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Responds to name :
                                                        </div>
                                                        <asp:TextBox ID="Communication_RespondsToName" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Two – way Interaction :
                                            </div>
                                            <div class="control-group" style="padding-left: 20px">
                                                <asp:CheckBox ID="Communication_TwoWayInteraction_1" runat="server" onclick="Communication_TwoWayInteraction_1_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Intiates " />
                                                <asp:CheckBox ID="Communication_TwoWayInteraction_2" runat="server" onclick="Communication_TwoWayInteraction_2_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Sustains" />
                                                <asp:CheckBox ID="Communication_TwoWayInteraction_3" runat="server" onclick="Communication_TwoWayInteraction_3_Click();"
                                                    Enabled="false" CssClass="checkboes" Text=" Follows" />
                                                <script type="text/javascript">
                                                    function Communication_TwoWayInteraction_1_Click() {
                                                        var ctl = $('#<%=Communication_TwoWayInteraction_1.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_TwoWayInteraction_2.ClientID %>').prop('checked', false);
                                                            $('#<%=Communication_TwoWayInteraction_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Communication_TwoWayInteraction_2_Click() {
                                                        var ctl = $('#<%=Communication_TwoWayInteraction_2.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_TwoWayInteraction_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Communication_TwoWayInteraction_3.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                    function Communication_TwoWayInteraction_3_Click() {
                                                        var ctl = $('#<%=Communication_TwoWayInteraction_3.ClientID %>')[0];
                                                        if (ctl.checked) {
                                                            $('#<%=Communication_TwoWayInteraction_1.ClientID %>').prop('checked', false);
                                                            $('#<%=Communication_TwoWayInteraction_2.ClientID %>').prop('checked', false);
                                                        }
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                7. Narrate Incidents :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            At school :
                                                        </div>
                                                        <asp:TextBox ID="Communication_NarrateIncidentsSchool" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            At home :
                                                        </div>
                                                        <asp:TextBox ID="Communication_NarrateIncidentsHome" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                8. Expressions of :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Wants :
                                                        </div>
                                                        <asp:TextBox ID="Communication_ExpressionsWants" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Needs :
                                                        </div>
                                                        <asp:TextBox ID="Communication_ExpressionsNeeds" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Emotions :
                                                        </div>
                                                        <asp:TextBox ID="Communication_ExpressionsEmotion" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Achievement/Failure :
                                                        </div>
                                                        <asp:TextBox ID="Communication_ExpressionsAchive" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                9. Language spoken to the child :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Communication_LanguagSpoken" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                10. Echolalia :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Communication_Echolalia" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="TabPanel28" runat="server" HeaderText="Repetitive Interests">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Any interests that dominates his/her life :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="RepetitiveInterests_Dominates" runat="server" Enabled="false" CssClass="span10"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Repetitive/ Unusual Behavior (stereotypes) :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="RepetitiveInterests_Behavior" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Copes up with unexpected changes :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="RepetitiveInterests_Changes" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"></asp:TextBox>
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
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                10 .SENSORY INTEGRATION &PRAXIS TEST :
                                            </div>
                                            <div class="control-label">
                                                A) Praxis test :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Praxistest" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                B) Designcopying :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Designcopying" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                C) Constructional Praxis :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="ConstructionalPraxis" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                D) Oral praxis :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Oralpraxis" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                E) Postural praxis :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Posturalpraxis" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                F) Praxis on verbal commands :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Praxisonverbalcommands" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                G) Sequencing praxis :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Sequencingpraxis" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                H) Sensory integration tests :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Sensoryintegrationtests" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                I) Bilateral motor co-ordination :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Bilateralmotorcoordination" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                J) Motor accuracy :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Motoraccuracy" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                K) Post rotatory nystagmus :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Postrotatorynystagmus" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                L) Standing & walking balance :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Standingwalkingbalance" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                M) Touch tests :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Touchtests" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                N) Graphesthesia :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Graphesthesia" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                O) Kinesthesia :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Kinesthesia" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                P) Localization of tactile stimuli :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Localizationoftactilestimuli" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                Q) Manual form perception :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Manualformperception" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                R)Visual perception tests :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Visualperceptiontests" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                S) Figure ground perception :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Figuregroundperception" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                T) Space visualization :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Spacevisualization" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                U) Others :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Others" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                V) Clock face :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Clockface" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                W) Motor planning :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Motorplanning" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                X) Body image :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Bodyimage" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                Y) Body schema :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Bodyschema" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                Z) Laterality :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Laterality" runat="server" Enabled="false" CssClass="span10" Rows="3"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="tb_Report7" runat="server" HeaderText="Others" Visible="false">
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
                        <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="Regulatory">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Arousal :
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
                                    <div class="clearfix">
                                    </div>
                                </div>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tb_Report9" runat="server" HeaderText="Musculoskeletal">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span11">
                                            <h5>
                                                Musculoskeletal :</h5>
                                            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
                                                <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="ROM-1">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            &nbsp;
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
                                                                            <asp:TextBox ID="Musculoskeletal_Rom1_DorsiflexionExtensionRight" runat="server"
                                                                                Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
                                                                    <tr>
                                                                        <td class="span3">
                                                                            &nbsp;
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
                                                                <table class="ndt-default-table">
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
                                                                            <asp:TextBox ID="Musculoskeletal_Mmt_PronatorRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                                                                            <asp:TextBox ID="Musculoskeletal_Mmt_ECSRight" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="tb_Report11" runat="server" HeaderText="Remarks Variable"
                            Visible="false">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="ndt-default-table">
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
                                                1. Visual System :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Focal Vision :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Focal" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Ambient vision :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Ambient" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Focus & Tracking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Focus" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Depth Perception :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Depth" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Refractive Error :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Refractive" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Physical Impairment :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Physical" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVisual_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                2. Vestibular :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Seeking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVestibula_Seeking" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Avoiding :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVestibula_Avoiding" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Gravitational Insecurities :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVestibula_Insecurities" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsVestibula_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                3. Oromotor / Gustatory :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Defensive / Seeking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Defensive" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Drooling :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Drooling" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Posture of mouth :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Mouth" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Mouthing :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Mouthing" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Bite/ Swallow/ Chew :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Chew" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOromotor_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
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
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Response and registration :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsAuditory_Response" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Seeking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsAuditory_Seeking" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Avoiding :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsAuditory_Avoiding" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsAuditory_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                5. Olfactory :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Seeking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOlfactory_seeking" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Avoiding :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOlfactory_Avoiding" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsOlfactory_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                6. Somatosensory :
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span10">
                                                        <b>Tactile Response</b>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Seeking :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensory_Seeking" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Avoiding :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensory_Avoiding" runat="server" Enabled="false" CssClass="span5"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">
                                                    <div class="span10">
                                                        <b>Proprioceptive</b>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Body Image :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensoryProprioceptive_BodyImage" runat="server" Enabled="false" 
                                                            CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Body Parts :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensoryProprioceptive_BodyParts" runat="server" Enabled="false" 
                                                            CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Clumsiness :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensoryProprioceptive_Clumsiness" runat="server" Enabled="false" 
                                                            CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Coordination :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensoryProprioceptive_Coordination" runat="server" Enabled="false" 
                                                            CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <div class="formRow">                                                    
                                                    <div class="span5">
                                                        <div class="control-label">
                                                            Additional Comments :
                                                        </div>
                                                        <asp:TextBox ID="SensorySystemsSomatosensory_Comment" runat="server" Enabled="false" CssClass="span5" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                    <div class="formRow" style="display:none;">
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
                                                <asp:TextBox ID="SensoryProfile_Profile" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
                                                    Rows="3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <table class="ndt-default-table">
                                                <%--<tr>
                                                               
                                                                <td>
                                                                    <b>Left</b>
                                                                </td>
                                                                <td>
                                                                    <b>Right</b>
                                                                </td>
                                                            </tr>--%>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName1" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result1" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName2" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result2" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName3" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result3" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName4" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result4" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName5" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result5" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName6" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result6" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName7" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result7" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName8" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result8" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName9" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result9" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName10" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result10" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName11" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result11" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName12" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result12" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName13" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result13" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName14" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result14" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName15" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result15" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName16" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result16" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName17" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result17" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName18" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result18" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName19" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result19" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName20" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result20" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName21" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result21" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName22" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result22" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName23" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result23" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName24" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result24" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName25" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result25" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName26" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result26" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName27" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result27" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName28" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result28" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName29" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result29" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName30" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result30" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName31" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result31" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName32" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result32" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName33" Enabled="false"  runat="server" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result33" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName34" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result34" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName35" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result35" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName36" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result36" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName37" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result37" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName38" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result38" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName39" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result39" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="SensoryName40" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Result40" runat="server" Enabled="false" CssClass="span3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="clearfix">
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
                                        <div class="span11">
                                            <h5>
                                                SIPT Information :</h5>
                                            <ajaxToolkit:TabContainer ID="TabContainer2" runat="server">
                                                <ajaxToolkit:TabPanel ID="TabPanel15" runat="server" HeaderText="History">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
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
                                                                            <asp:TextBox ID="SIPTInfo_HandFunction2_OppositionMFL" runat="server" Enabled="false" CEnabled="false" ssClass="span1"></asp:TextBox>
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
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_BodySchema" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Laterality
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_SIPT10_Laterality" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="SIPTInfo_ActivityGiven_Remark" runat="server" Enabled="false"  CssClass="span10"
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
                                                                        <td>
                                                                            Interest In Activity
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestActivity" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Interest In Completion
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_InterestCompletion" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Initial Learning
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Learning" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Complexity And Organisation Of Task
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Complexity" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Problem Solving
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_ProblemSolving" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Concentration
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Concentration" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Retension And Recall
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Retension" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Speed Of Perfomance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SpeedPerfom" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Activity Neatness
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Neatness" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Frustation Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Frustation" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Work Tolerance
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Work" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Reaction To Authority
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_Reaction" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Sociability With Therapist
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityTherapist" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Sociability With Others Students
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="SIPTInfo_ActivityGiven_SociabilityStudents" runat="server" Enabled="false"  CssClass="span3"></asp:TextBox>
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
                        <ajaxToolkit:TabPanel ID="TabPanel29" runat="server" HeaderText="Other">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                Sensory Profile :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Other_SensoryProfile" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                SIPT :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Other_SIPT" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                DCD questionnaire :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Other_DCD" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                DSM 5 :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Other_DSM" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
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
                                                <asp:TextBox ID="Cognition_Intelligence" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_Attention" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_Memory" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_Adaptibility" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_MotorPlanning" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_ExecutiveFunction" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cognition_CognitiveFunctions" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                        <ajaxToolkit:TabPanel ID="tb_Report16" runat="server" HeaderText="Integumentary"
                            Visible="false">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Skin Integrity :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Integumentary_SkinIntegrity" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Integumentary_SkinColor" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Integumentary_SkinExtensibility" runat="server" CssClass="span10" Enabled="false"   
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
                        <ajaxToolkit:TabPanel ID="tb_Report17" runat="server" HeaderText="Respiratory" Visible="false">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Rate-resting :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Respiratory_RateResting" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Respiratory_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                                                <asp:TextBox ID="Respiratory_Patterns" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                                                <asp:TextBox ID="Respiratory_BreathControl" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                        <ajaxToolkit:TabPanel ID="tb_Report18" runat="server" HeaderText="Cardiovascular"
                            Visible="false">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Heart Rate-Resting :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Cardiovascular_HeartRate" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                                                <asp:TextBox ID="Cardiovascular_PostExercise" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                                                <asp:TextBox ID="Cardiovascular_BP" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cardiovascular_Edema" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cardiovascular_Circulation" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Cardiovascular_EEi" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                        <ajaxToolkit:TabPanel ID="tb_Report19" runat="server" HeaderText="Gastrointestinal"
                            Visible="false">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;">
                                    <div class="formRow">
                                        <div class="span12">
                                            <div class="control-label">
                                                1. Bowel/Blader :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Gastrointestinal_Bowel" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false" 
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
                                                <asp:TextBox ID="Gastrointestinal_Intake" runat="server" CssClass="span10" TextMode="MultiLine" Enabled="false"
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
                                        <div class="span11">
                                            <h5>
                                                Evaluation :</h5>
                                            <ajaxToolkit:TabContainer ID="TabContainer3" runat="server">
                                                <ajaxToolkit:TabPanel ID="TabPanel17" runat="server" HeaderText="Strength">
                                                    <ContentTemplate>
                                                        <div class="formRow">
                                                            <div class="span10">
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
                                                                    <asp:TextBox ID="Evaluation_Concern_Barriers" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                                        Rows="3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="Evaluation_Concern_Limitations" runat="server" Enabled="false" CssClass="span10"
                                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                                                                    <asp:TextBox ID="Evaluation_Concern_Posture" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
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
                                                            <div class="span10">
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
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    2. Previous Long Term Goals :
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
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    3. Long Term Goals(Functional Outcome Measured)1 - Year :
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
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    4. Short Term Goals(Functional Outcome Measures) 3 - Month :
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
                                                            <div class="span10">
                                                                <div class="control-label">
                                                                    5. impairment related Objective goal-3 Months :
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                                            <div class="span10">
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
                                    <div class="formRow" style="display:none;">
                                        <div class="span12">
                                            <div class="control-label">
                                                Goals and Expectations from therapy :
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="GoalsAndExpectations" runat="server" Enabled="false" CssClass="span10" TextMode="MultiLine"
                                                    Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                    </div>
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
                                                <asp:DropDownList ID="Doctor_EnterReport" runat="server" Enabled="false" CssClass="chzn-select span6">
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
