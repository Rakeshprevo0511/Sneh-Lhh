<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Site.master" CodeBehind="NDT_SFT_REVAL_RPT.aspx.cs" Inherits="snehrehab.SessionRpt.NDT_SFT_REVAL_RPT" %>
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
    </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid">
        <div class="grid-title">
            <div class="pull-left">
                NDT Report 2025 :</div>
            <div class="pull-right">
            <a href="/Reports/Ndt_view_2025.aspx" class="btn btn-primary" id="sessview" runat="server" visible="false">View List</a>
            <a href="/Reports/Ndt_view_2025.aspx" class="btn btn-primary" id="rptview" runat="server" visible="false">View List</a>
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
                    <label class="control-label">
                        Given Date :</label>
                    <div class="control-group">
                        <asp:TextBox ID="txtGivenDate" runat="server" CssClass="span2 my-datepicker"></asp:TextBox>
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
                    <asp:HiddenField ID="hfdTabs" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCallFrom" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdCurTab" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfdPrevTab" runat="server" ClientIDMode="Static" />
                    <ajaxToolkit:TabContainer ID="tb_Contents" runat="server"   OnClientActiveTabChanged="clientActiveTabChanged">

                   <ajaxToolkit:TabPanel ID="tb_Report1" runat="server" HeaderText="Functional Abilities and Limitations">
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
                                                    
                                                    <asp:TextBox ID="FA_GrossMotor_Ability" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_GrossMotor_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Fine Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_FineMotor_Ability" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_FineMotor_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Communication</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_Communication_Ability" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_Communication_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Cognition</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="FA_Cognition_Ability" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="FA_Cognition_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
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
                   <ajaxToolkit:TabPanel ID="tb_Report2" runat="server" HeaderText="Participation Abilities and Limitations">
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
                                                    <asp:TextBox ID="ParticipationAbility_GrossMotor" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_GrossMotor_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Fine Motor</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_FineMotor" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_FineMotor_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Communication</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_Communication" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_Communication_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                            </tr>
                                            <tr>
                                                <td><label><b>Cognition</b></label></td>
                                                <td>
                                                    <asp:TextBox ID="ParticipationAbility_Cognition" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                 <asp:TextBox ID="ParticipationAbility_Cognition_Limit" runat="server" class="span5" TextMode="MultiLine"></asp:TextBox>
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
                                                    <asp:TextBox ID="Contextual_Personal_Positive" runat="server" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                 <td><label><b>Positive : </b></label></td>
                                                 <td>
                                                    <asp:TextBox ID="Contextual_Enviremental_Positive" runat="server" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                
                                            </tr> 
                                            <tr>
                                               <td><label><b>Negative : </b></label></td>
                                                <td>
                                                <asp:TextBox ID="Contextual_Personal_Negative" runat="server" class="span4" TextMode="MultiLine"></asp:TextBox>
                                                </td> 
                                                <td><label><b>Negative : </b></label></td>
                                                <td>
                                                 <asp:TextBox ID="Contextual_Enviremental_Negative" runat="server" class="span4" TextMode="MultiLine"></asp:TextBox>
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
                   <ajaxToolkit:TabPanel ID="tb_Report3" runat="server" HeaderText="Multisystem -Posture ">
                     <ContentTemplate>
                         <div style="margin-top: 20px; margin-bottom: 20px;">
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         Alignment :
                                     </div>
                                     <div class="span5">
                                         <table>
                                             <%--<tr>
                                                 <td>Right:</td>
                                                 <td>
                                                     <asp:TextBox ID="txtRight" runat="server" CssClass="Span6" Textmode="MultiLine"/>
                                                 </td>
                                                 <td>Left:</td>
                                                 <td>
                                                     <asp:TextBox ID="txtLeft" runat="server" CssClass="Span6" Textmode="MultiLine" />
                                                 </td>
                                             </tr>--%>
                                             <tr>
                                                 <td style="padding-right: 30px;">Symmetric:</td>
                                                 <td >
                                                     <asp:CheckBox ID="chkSymmetric" runat="server" CssClass="Span6"
                                                         onclick="selectOnlyOne(this, 'chkAsymmetric')" />
                                                          &nbsp;&nbsp;&nbsp;
                                                 &nbsp;&nbsp;&nbsp;
                                                 </td>
                                            
                                                 <td style="padding-right: 30px;" >Asymmetric:</td>
                                                 <td>
                                                     <asp:CheckBox ID="chkAsymmetric" runat="server" CssClass="Span6"
                                                         onclick="selectOnlyOne(this, 'chkSymmetric')" />
                                                 </td>
                                             </tr>
                                         </table>
                                     </div>
                                 </div>
                             </div>
                             <script type="text/javascript">
                                 function selectOnlyOne(clickedCheckbox, otherCheckboxId) {
                                     var otherCheckbox = document.getElementById(otherCheckboxId);
                                     if (clickedCheckbox.checked) {
                                         otherCheckbox.checked = false;
                                     }
                                 }
                             </script>                  
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span12">
                                         <h5>General Posture Comments :</h5>
                                     </div>
                                 </div>
                             </div>
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         1) Head :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkHead_Forward" runat="server" CssClass="checkboes" Text=" Forward Head" />
                                         <asp:CheckBox ID="chkHead_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                         <asp:CheckBox ID="chkHead_PlagiocephalyRight" runat="server" CssClass="checkboes" Text=" Plagiocephaly Right" />
                                         <asp:CheckBox ID="chkHead_PlagiocephalyLeft" runat="server" CssClass="checkboes" Text=" Plagiocephaly Left" />
                                         <asp:CheckBox ID="chkHead_FrontalBossing" runat="server" CssClass="checkboes" Text=" Frontal Bossing" />
                                     </div>
                                 </div>
                             </div>

                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         2) Shoulder :
                                     </div>
                                     <div class="span8">
                                             <asp:CheckBox ID="chkShoulder_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                             <asp:CheckBox ID="chkShoulder_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                             <asp:CheckBox ID="chkShoulder_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                             <asp:CheckBox ID="chkShoulder_InternallyRotated" runat="server" CssClass="checkboes" Text=" Internally Rotated" />
                                             <asp:CheckBox ID="chkShoulder_ExternallyRotated" runat="server" CssClass="checkboes" Text=" Externally Rotated" />
                                             <asp:CheckBox ID="chkShoulder_Elevated" runat="server" CssClass="checkboes" Text=" Elevated" />
                                             <asp:CheckBox ID="chkShoulder_Depressed" runat="server" CssClass="checkboes" Text=" Depressed" />
                                             <asp:CheckBox ID="chkShoulder_Protracted" runat="server" CssClass="checkboes" Text=" Protracted" />
                                             <asp:CheckBox ID="chkShoulder_Retracted" runat="server" CssClass="checkboes" Text=" Retracted" />
                                             <asp:CheckBox ID="chkShoulder_Abducted" runat="server" CssClass="checkboes" Text=" Abducted" />
                                             <asp:CheckBox ID="chkShoulder_Adducted" runat="server" CssClass="checkboes" Text=" Adducted" />
                                            
                                             <asp:CheckBox ID="chkShoulder_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                       
                                     </div>
                                 </div>
                             </div>
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         3) Neck :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkNeck_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkNeck_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkNeck_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkNeck_LateralTilt" runat="server" CssClass="checkboes" Text=" Lateral Tilt" />
                                         <asp:CheckBox ID="chkNeck_Hyperextended" runat="server" CssClass="checkboes" Text=" Hyperextended" />
                                         <asp:CheckBox ID="chkNeck_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkNeck_ChinTuck" runat="server" CssClass="checkboes" Text=" Chin Tuck" />
                                         <asp:CheckBox ID="chkNeck_Neutral" runat="server" CssClass="checkboes" Text="Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         4) Scapulae :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkScapulae_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkScapulae_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkScapulae_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkScapulae_Protracted" runat="server" CssClass="checkboes" Text=" Protracted" />
                                         <asp:CheckBox ID="chkScapulae_Retracted" runat="server" CssClass="checkboes" Text=" Retracted" />
                                         <asp:CheckBox ID="chkScapulae_Abducted" runat="server" CssClass="checkboes" Text=" Abducted" />
                                         <asp:CheckBox ID="chkScapulae_Adducted" runat="server" CssClass="checkboes" Text=" Adducted" />
                                         <asp:CheckBox ID="chkScapulae_Elevated" runat="server" CssClass="checkboes" Text=" Elevated" />
                                         <asp:CheckBox ID="chkScapulae_Depressed" runat="server" CssClass="checkboes" Text=" Depressed" />
                                         <asp:CheckBox ID="chkScapulae_Winging" runat="server" CssClass="checkboes" Text=" Winging of Scapula" />
                                         <asp:CheckBox ID="chkScapulae_Neutral" runat="server" CssClass="checkboes" Text="Neutral" />
                                     </div>
                                 </div>
                             </div>
                             <!-- Elbow Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         5) Elbow :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkElbow_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkElbow_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkElbow_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkElbow_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkElbow_Extended" runat="server" CssClass="checkboes" Text=" Extended" />
                                         <asp:CheckBox ID="chkElbow_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Forearm Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         6) Forearm :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkForearm_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkForearm_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkForearm_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkForearm_Supinated" runat="server" CssClass="checkboes" Text=" Supinated" />
                                         <asp:CheckBox ID="chkForearm_Pronated" runat="server" CssClass="checkboes" Text=" Pronated" />
                                         <asp:CheckBox ID="chkForearm_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Wrist Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         7) Wrist :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkWrist_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkWrist_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                         <asp:CheckBox ID="chkWrist_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkWrist_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkWrist_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkWrist_Extended" runat="server" CssClass="checkboes" Text=" Extended" />
                                         <asp:CheckBox ID="chkWrist_UD" runat="server" CssClass="checkboes" Text=" Ulnar Deviation (UD)" />
                                         <asp:CheckBox ID="chkWrist_RD" runat="server" CssClass="checkboes" Text=" Radial Deviation (RD)" />
                                     </div>
                                 </div>
                             </div>
                             <!-- Hand Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         8) Hand :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkHand_Fist" runat="server" CssClass="checkboes" Text=" Fist of hand" />
                                         <asp:CheckBox ID="chkHand_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkHand_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkHand_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkHand_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Fingers Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         9) Fingers :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkFingers_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkFingers_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkFingers_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkFingers_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkFingers_Extended" runat="server" CssClass="checkboes" Text=" Extended" />
                                         <asp:CheckBox ID="chkFingers_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Thumb Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         10) Thumb :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkThumb_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkThumb_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkThumb_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkThumb_Adducted" runat="server" CssClass="checkboes" Text=" Adducted" />
                                         <asp:CheckBox ID="chkThumb_Abducted" runat="server" CssClass="checkboes" Text=" Abducted" />
                                         <asp:CheckBox ID="chkThumb_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>


                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         11) Ribcage :
                                     </div>
                                     <div class="span8">
                                         <asp:TextBox ID="Posture_Gen_Ribcage" runat="server" CssClass="span8"></asp:TextBox>
                                     </div>
                                 </div>
                             </div>
                             <!-- Thoracic Spine Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         12) Thoracic Spine :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkThoracicSpine_Rounded" runat="server" CssClass="checkboes" Text=" Rounded" />
                                         <asp:CheckBox ID="chkThoracicSpine_Hyperextended" runat="server" CssClass="checkboes" Text=" Hyperextended" />
                                         <asp:CheckBox ID="chkThoracicSpine_LaterallyFlexed" runat="server" CssClass="checkboes" Text=" Laterally Flexed" />
                                         <asp:CheckBox ID="chkThoracicSpine_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>
                             <!-- Lumbar Spine Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         13) Lumbar Spine :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkLumbarSpine_Flattened" runat="server" CssClass="checkboes" Text=" Flattened" />
                                         <asp:CheckBox ID="chkLumbarSpine_Hyperextended" runat="server" CssClass="checkboes" Text=" Hyperextended" />
                                         <asp:CheckBox ID="chkLumbarSpine_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Pelvis Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         14) Pelvis :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkPelvis_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                         <asp:CheckBox ID="chkPelvis_AnteriorTilted" runat="server" CssClass="checkboes" Text=" Anteriorly Tilted" />
                                         <asp:CheckBox ID="chkPelvis_PosteriorTilted" runat="server" CssClass="checkboes" Text=" Posteriorly Tilted" />
                                     
                                     </div>
                                 </div>
                             </div>
                             <!-- Hips Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         15) Hips :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkHips_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkHips_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkHips_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkHips_LaterallyRotated" runat="server" CssClass="checkboes" Text=" Laterally Rotated" />
                                         <asp:CheckBox ID="chkHips_Abducted" runat="server" CssClass="checkboes" Text=" Abducted" />
                                         <asp:CheckBox ID="chkHips_InternallyRotated" runat="server" CssClass="checkboes" Text=" Internally Rotated" />
                                         <asp:CheckBox ID="chkHips_Adducted" runat="server" CssClass="checkboes" Text=" Adducted" />
                                         <asp:CheckBox ID="chkHips_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkHips_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Knees Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         16) Knees :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkKnees_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkKnees_Hyperextended" runat="server" CssClass="checkboes" Text=" Hyperextended" />
                                         <asp:CheckBox ID="chkKnees_Flexed" runat="server" CssClass="checkboes" Text=" Flexed" />
                                         <asp:CheckBox ID="chkKnees_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>
                             <!-- Ankle Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         17) Ankle :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkAnkle_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkAnkle_Plantarflexed" runat="server" CssClass="checkboes" Text=" Plantarflexed" />
                                         <asp:CheckBox ID="chkAnkle_Dorsiflexed" runat="server" CssClass="checkboes" Text=" Dorsiflexed" />
                                         <asp:CheckBox ID="chkAnkle_Inverted" runat="server" CssClass="checkboes" Text=" Inverted" />
                                         <asp:CheckBox ID="chkAnkle_Everted" runat="server" CssClass="checkboes" Text=" Everted" />
                                         <asp:CheckBox ID="chkAnkle_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- Feet Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         18) Feet :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkFeet_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkFeet_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkFeet_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkFeet_Pronated" runat="server" CssClass="checkboes" Text=" Pronated" />
                                         <asp:CheckBox ID="chkFeet_Supinated" runat="server" CssClass="checkboes" Text=" Supinated" />
                                         <asp:CheckBox ID="chkFeet_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>
                             <!-- Toes Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         19) Toes :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkToes_BL" runat="server" CssClass="checkboes" Text=" B/L" />
                                         <asp:CheckBox ID="chkToes_Right" runat="server" CssClass="checkboes" Text=" Right" />
                                         <asp:CheckBox ID="chkToes_Left" runat="server" CssClass="checkboes" Text=" Left" />
                                         <asp:CheckBox ID="chkToes_Curled" runat="server" CssClass="checkboes" Text=" Curled" />
                                         <asp:CheckBox ID="chkToes_Extended" runat="server" CssClass="checkboes" Text=" Extended" />
                                         <asp:CheckBox ID="chkToes_Neutral" runat="server" CssClass="checkboes" Text=" Neutral" />
                                     </div>
                                 </div>
                             </div>

                             <!-- BOS Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         20) BOS :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkBOS_Narrow" runat="server" CssClass="checkboes" Text=" Narrow" />
                                         <asp:CheckBox ID="chkBOS_Wide" runat="server" CssClass="checkboes" Text=" Wide" />
                                     </div>
                                 </div>
                             </div>

                             <!-- COM & COG Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         2) COM & COG :
                                     </div>
                                     <div class="span8">
                                         <asp:TextBox ID="txtCOM_COG" runat="server" CssClass="span8" TextMode="MultiLine" Rows="3" placeholder=" comments..."></asp:TextBox>
                                     </div>
                                 </div>
                             </div>


                             <div class="span11 formRow">
                                 
                                 <div class="span11 formRow">
                                <div class="row">
                                    <div class="span12">
                                        <h5> Oral Structure Alignment:</h5>
                                    </div>
                                </div>
                            </div> 
                                 <table class="ndt-default-table">
                                     <tr>
                                         <td><b>Mouth</b></td>
                                         <td>
                                             <asp:TextBox ID="txtMouth" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                     <tr>
                                         <td>Tongue</td>
                                         <td>
                                             <asp:TextBox ID="txtTongue" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                     <tr>
                                         <td>Teeth</td>
                                         <td>
                                             <asp:TextBox ID="txtTeeth" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                     <tr>
                                         <td>Chin</td>
                                         <td>
                                             <asp:TextBox ID="txtChin" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                     <tr>
                                         <td>Cheeks</td>
                                         <td>
                                             <asp:TextBox ID="txtCheeks" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                     <tr>
                                         <td>Lips</td>
                                         <td>
                                             <asp:TextBox ID="txtLips" runat="server" CssClass="span3"></asp:TextBox></td>
                                     </tr>
                                 </table>

                             </div>

                             <!-- Strategies for Stability Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         3) Strategies for Stability :
                                     </div>
                                     <div class="span8">
                                         <asp:CheckBox ID="chkStability_PosturalTone" runat="server" CssClass="checkboes" Text=" Increasing postural tone" />
                                         <asp:CheckBox ID="chkStability_LockingJoints" runat="server" CssClass="checkboes" Text=" Locking of joints" />
                                         <asp:CheckBox ID="chkStability_BroadeningBOS" runat="server" CssClass="checkboes" Text=" Broadening the BOS" />
                                         <br />
                                       
                                      
                                     </div>
                                 </div>
                                 <div class="row">
                                       <div class="span2">
                                         comments :
                                     </div>
                                        <div class="span8">
                                               <asp:TextBox ID="txtStability_Comments" runat="server" CssClass="span8" TextMode="MultiLine" Rows="3" placeholder="Enter comments..."></asp:TextBox>
                                        </div>
                                 </div>
                             </div>
                             <!-- Anticipatory Control Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         4) Anticipatory Control :
                                     </div>
                                     <div class="span8">
                                         <asp:TextBox ID="txtAnticipatoryControl" runat="server" CssClass="span8" TextMode="MultiLine" Rows="3" placeholder="Enter details..."></asp:TextBox>
                                     </div>
                                 </div>
                             </div>
                             
                             <!-- Postural Counter Balance Section -->
                             <div class="span11 formRow">
                                 <div class="row">
                                     <div class="span2">
                                         5) Postural Counter Balance :
                                     </div>
                                     <div class="span8">
                                         <asp:TextBox ID="txtPosturalCounterBalance" runat="server" CssClass="span8" TextMode="MultiLine" Rows="3" placeholder="Enter details..."></asp:TextBox>
                                     </div>
                                 </div>
                             </div>




                             <div class="clearfix">
                             </div>
                         </div>
                     </ContentTemplate>
                </ajaxToolkit:TabPanel>
                   <ajaxToolkit:TabPanel ID="tb_Report4" runat="server" HeaderText="Multisystem -Movement">
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
                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            2. Quality of movement?  :
                                        </div>
                                        <div class="span5">
                                            <asp:CheckBox ID="Multi_Movement_TypeOf_1" runat="server" onclick="Movement_TypeOf_1_Click()"
                                                CssClass="checkboes" Text=" Ramp" />
                                            <asp:CheckBox ID="Multi_Movement_TypeOf_2" runat="server" onclick="Movement_TypeOf_2_Click()"
                                                CssClass="checkboes" Text=" Ballistic" />
                                            <script type="text/javascript">
                                                function Movement_TypeOf_1_Click() {
                                                    var ctl = $('#<%=Multi_Movement_TypeOf_1.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Multi_Movement_TypeOf_2.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                                function Movement_TypeOf_2_Click() {
                                                    var ctl = $('#<%=Multi_Movement_TypeOf_2.ClientID %>')[0];
                                                if (ctl.checked) {
                                                    $('#<%=Multi_Movement_TypeOf_1.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>
                                        </div>
                                    </div>
                                </div>
                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            Plane of Movements :
                                        </div>
                                        <div class="span8">
                                            <div class="row">
                                                <div class="span5">
                                                    <asp:CheckBox ID="Multi_Movement_Sagittal" runat="server"
                                                        onclick="Movement_Sagittal_Click()" CssClass="checkboes" Text=" Sagittal" />
                                                    <asp:CheckBox ID="Multi_Movement_Coronal" runat="server"
                                                        onclick="Movement_Coronal_Click()" CssClass="checkboes" Text=" Coronal" />
                                                    <asp:CheckBox ID="Multi_Movement_Frontal" runat="server"
                                                        onclick="Movement_Frontal_Click()" CssClass="checkboes" Text=" Transverse" />
                                                </div>
                                            </div>

                                            <script type="text/javascript">
                                                function Movement_Sagittal_Click() {
                                                    var ctl = $('#<%= Multi_Movement_Sagittal.ClientID %>')[0];
        if (ctl.checked) {
            $('#<%= Multi_Movement_Coronal.ClientID %>').prop('checked', false);
            $('#<%= Multi_Movement_Frontal.ClientID %>').prop('checked', false);
                                                    }
                                                }

                                                function Movement_Coronal_Click() {
                                                    var ctl = $('#<%= Multi_Movement_Coronal.ClientID %>')[0];
        if (ctl.checked) {
            $('#<%= Multi_Movement_Sagittal.ClientID %>').prop('checked', false);
            $('#<%= Multi_Movement_Frontal.ClientID %>').prop('checked', false);
                                                    }
                                                }

                                                function Movement_Frontal_Click() {
                                                    var ctl = $('#<%= Multi_Movement_Frontal.ClientID %>')[0];
        if (ctl.checked) {
            $('#<%= Multi_Movement_Sagittal.ClientID %>').prop('checked', false);
            $('#<%= Multi_Movement_Coronal.ClientID %>').prop('checked', false);
                                                    }
                                                }
                                            </script>

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
                                            Weight Shifts :
                                        </div>
                                        <div class="span8">
                                            <asp:CheckBoxList ID="Movement_WeightShift" runat="server" CssClass="checkboes">
                                                <asp:ListItem Value="Sagittal">Sagittal</asp:ListItem>
                                                <asp:ListItem Value="Frontal">Transverse</asp:ListItem>
                                                <asp:ListItem Value="Coronal">Coronal</asp:ListItem>
                                                <asp:ListItem Value="Posterolateral">Posterolateral</asp:ListItem>
                                                <asp:ListItem Value="Anterolateral">Anterolateral</asp:ListItem>
                                                <asp:ListItem Value="Shortened">Shortened</asp:ListItem>
                                                <asp:ListItem Value=" Elongated"> Elongated</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>

                                <script type="text/javascript">
                                    function uncheckOthers(currentId) {
                                        <%--var checkboxes = [
            '<%= Movement_WeightShift_Sagittal.ClientID %>',
            '<%= Movement_WeightShift_Frontal.ClientID %>',
            '<%= Movement_WeightShift_Coronal.ClientID %>',
            '<%= Movement_WeightShift_Posterolateral.ClientID %>',Weight Shifts 
            '<%= Movement_WeightShift_Anterolateral.ClientID %>'
                                        ];

                                        checkboxes.forEach(function (id) {
                                            if (id !== currentId) {
                                                $('#' + id).prop('checked', false);
                                            }
                                        });
                                    }

                                    function WeightShift_Sagittal_Click() {
                                        uncheckOthers('<%= Movement_WeightShift_Sagittal.ClientID %>');
                                    }

                                    function WeightShift_Frontal_Click() {
                                        uncheckOthers('<%= Movement_WeightShift_Frontal.ClientID %>');
                                    }

                                    function WeightShift_Coronal_Click() {
                                        uncheckOthers('<%= Movement_WeightShift_Coronal.ClientID %>');
                                    }

                                    function WeightShift_Posterolateral_Click() {
                                        uncheckOthers('<%= Movement_WeightShift_Posterolateral.ClientID %>');
                                    }

                                    function WeightShift_Anterolateral_Click() {
                                        uncheckOthers('<%= Movement_WeightShift_Anterolateral.ClientID %>');
                                    }--%>
                                </script>

                                <div class="span11 formRow">
                                    <div class="row" >
                                        <div class="span2">
                                            Dissociation :
                                        </div>
                                        <div class="span8" style="display: none;" >
                                           
                                                <h6>Interlimb Dissociation</h6>
                                                <asp:CheckBox ID="Movement_Interlimb_SpineToShoulder" runat="server" CssClass="checkboes" Text=" Spine to Shoulder Girdle" />
                                                <asp:CheckBox ID="Movement_Interlimb_Scapulohumeral" runat="server" CssClass="checkboes" Text=" Scapulohumeral" />
                                                <asp:CheckBox ID="Movement_Interlimb_Pelvifemoral" runat="server" CssClass="checkboes" Text=" Pelvifemoral" />
                                                <asp:CheckBox ID="Movement_Interlimb_WithinUL" runat="server" CssClass="checkboes" Text=" Within UL" />
                                                <asp:CheckBox ID="Movement_Interlimb_WithinLL" runat="server" CssClass="checkboes" Text=" Within LL" />
                                          </div>
                                         <div class="span2">
                                        
                                        </div>
                                        <div class="span12">
                                           <b>Interlimb Dissociation</b>
                                        </div>
                                        <div class="span11 formRow">
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th></th>
                                                        <th>Poor</th>
                                                        <th>Fair</th>
                                                        <th>Good</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
    <tr>
        <td>Soine to shoulder</td>
        <td>
            <asp:CheckBox ID="chkSoinePoor" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkSoineFair" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkSoineGood" runat="server" CssClass="checkboes" Text=" " />
        </td>
    </tr>
    <tr>
        <td>scapulohumeral</td>
        <td>
            <asp:CheckBox ID="chkScapuloPoor" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkScapuloFair" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkScapuloGood" runat="server" CssClass="checkboes" Text=" " />
        </td>
    </tr>
    <tr>
        <td>pelvifomarol</td>
        <td>
            <asp:CheckBox ID="chkPelviPoor" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkPelviFair" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkPelviGood" runat="server" CssClass="checkboes" Text=" " />
        </td>
    </tr>
    <tr>
        <td>within ul</td>
        <td>
            <asp:CheckBox ID="chkWithinUlPoor" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkWithinUlFair" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkWithinUlGood" runat="server" CssClass="checkboes" Text=" " />
        </td>
    </tr>
    <tr>
        <td>within ll</td>
        <td>
            <asp:CheckBox ID="chkWithinLlPoor" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkWithinLlFair" runat="server" CssClass="checkboes" Text=" " />
        </td>
        <td>
            <asp:CheckBox ID="chkWithinLlGood" runat="server" CssClass="checkboes" Text=" " />
        </td>
    </tr>
</tbody>

                                            </table>
                                        </div>
                                            

                                       <br />
                                                    <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                                <strong>Intralimb Dissociation</strong>
                                            </div>
                                                  <div class="span2">
                                                <asp:CheckBox ID="Movement_Intralimb_LE" runat="server" CssClass="checkboes upper-limb" Text=" LE" />
                                                </div>
                                                  <div class="span2">
                                                      <asp:CheckBox ID="Movement_Intralimb_UE" runat="server" CssClass="checkboes upper-limb" Text=" UE" />
                                                </div>
                                                  <div class="span2">
                                                      <asp:CheckBox ID="Movement_Intralimb_Spine" runat="server" CssClass="checkboes upper-limb" Text=" Spine" />
                                       </div>
                                        </div>
                                                       </div>
                                        </div>
                                    
                                </div>

                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            <strong>Range  of Movements:</strong>
                                        </div>
                                       <div class="span8">
    <!-- Upper Limb -->
    <h6>Upper Limb</h6>
    <div class="row">
        <div class="span2">
            <asp:CheckBox ID="Movement_UpperLimb_Inner" runat="server" CssClass="checkboes upper-limb" Text=" Inner" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_UpperLimb_Mid" runat="server" CssClass="checkboes upper-limb" Text=" Mid" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_UpperLimb_Outer" runat="server" CssClass="checkboes upper-limb" Text=" Outer" />
        </div>
    </div>

    <!-- Lower Limb -->
    <h6>Lower Limb</h6>
    <div class="row">
        <div class="span2">
            <asp:CheckBox ID="Movement_LowerLimb_Inner" runat="server" CssClass="checkboes lower-limb" Text=" Inner" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_LowerLimb_Mid" runat="server" CssClass="checkboes lower-limb" Text=" Mid" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_LowerLimb_Outer" runat="server" CssClass="checkboes lower-limb" Text=" Outer" />
        </div>
    </div>

    <!-- Cervical Spine -->
    <h6>Cervical Spine</h6>
    <div class="row">
        <div class="span2">
            <asp:CheckBox ID="Movement_CervicalSpine_Inner" runat="server" CssClass="checkboes cervical-spine" Text=" Inner" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_CervicalSpine_Mid" runat="server" CssClass="checkboes cervical-spine" Text=" Mid" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_CervicalSpine_Outer" runat="server" CssClass="checkboes cervical-spine" Text=" Outer" />
        </div>
    </div>

    <!-- Thoracic Spine -->
    <h6>Thoracic Spine</h6>
    <div class="row">
        <div class="span2">
            <asp:CheckBox ID="Movement_ThoracicSpine_Inner" runat="server" CssClass="checkboes thoracic-spine" Text=" Inner" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_ThoracicSpine_Mid" runat="server" CssClass="checkboes thoracic-spine" Text=" Mid" />
        </div>
        <div class="span2">
            <asp:CheckBox ID="Movement_ThoracicSpine_Outer" runat="server" CssClass="checkboes thoracic-spine" Text=" Outer" />
        </div>
    </div>

    <script type="text/javascript">
                                        $(document).ready(function () {
                                            $(".upper-limb").click(function () {
                                                $(".upper-limb").not(this).prop("checked", false);
                                            });

                                            $(".lower-limb").click(function () {
                                                $(".lower-limb").not(this).prop("checked", false);
                                            });

                                            $(".cervical-spine").click(function () {
                                                $(".cervical-spine").not(this).prop("checked", false);
                                            });

                                            $(".thoracic-spine").click(function () {
                                                $(".thoracic-spine").not(this).prop("checked", false);
                                            });
                                        });
    </script>
</div>
                                    </div>
                                </div>


                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            Strategies For Stability & Mobility :
                                        </div>
                                        <div class="span8">
                                        <h6>Movement Stability</h6>
<asp:CheckBox ID="chkOveruseMomentum" runat="server" CssClass="checkboes" Text=" Overuse of momentum" />
<asp:CheckBox ID="chkIncreasedBOS" runat="server" CssClass="checkboes" Text=" Increased BOS" />
<asp:CheckBox ID="chkIncreasingPosturalTone" runat="server" CssClass="checkboes" Text=" Increasing postural tone" />

                                        </div>
                                    </div>
                                </div>
                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            Sign of Movement System Impairment or Overuse :
                                        </div>
                                        <div class="span8">
                                            <h6>Movement Overuse</h6>
<asp:CheckBox ID="chkLeanMuscle" runat="server" CssClass="checkboes" Text=" Lean Muscle" />
<asp:CheckBox ID="chkLockingJoints" runat="server" CssClass="checkboes" Text=" Locking of Joints" />
<asp:CheckBox ID="chkBroadBOS" runat="server" CssClass="checkboes" Text=" Broad BOS" />
<asp:CheckBox ID="chkGeneralPosture" runat="server" CssClass="checkboes" Text=" General Posture" />

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
                                            While maintaining a posture :
                                        </div>
                                        <div class="span8">
                                            <asp:TextBox ID="Movement_Balance_Maintain" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            During Transitions :
                                        </div>
                                        <div class="span8">
                                            <asp:TextBox ID="Movement_Balance_During" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                  <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span12">
                                            <h5>General observations  :</h5>
                                        </div>
                                    </div>
                                </div> <div class="span11 formRow">
                                    <div class="row">
                                        <div class="span2">
                                            Comments :
                                        </div>
                                        <div class="span8">
                                            <asp:TextBox ID="Gene_obsr_comments_txt" runat="server" CssClass="span8" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                        </div>
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
                   <ajaxToolkit:TabPanel ID="tb_Report5" runat="server" HeaderText="Neuromoter System">
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
                                            <th>Neck Trunk</th> <%--Initiation--%>
                                            <th>Upper Exthen</th><%--Sustainance--%>
                                            <th>Lower Exthen</th><%--Termination--%>
                                            <th>Control & Gradation</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Recruitment<br />Movement<br />Postrual</td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Recruitment_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                          <tr>
                                            <td>Initialigy<br />Sustainance<br />Termination</td>
                                            <td><asp:TextBox ID="Neurometer_Initialigy_initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neurometer_Initialigy_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neurometer_Initialigy_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neurometer_Initialigy_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Type of contraction<br />Concentric<br />Isometric<br />Eccentric</td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Contraction_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Co-activation / Reciprocal inhibition</td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Sustainance" runat="server" CssClass="span3"  Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Coactivation_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Type of Synergy</td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Synergy_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Stiffness(Static/Dynamic)</td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Stiffness_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                        </tr>
                                        <tr>                                     
                                            <td>Extraneous Movement</td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Initial" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Sustainance" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Termination" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
                                            <td><asp:TextBox ID="Neuromotor_Extraneous_Control" runat="server" CssClass="span3" Textmode="MultiLine" style="width: 217px; height: 65px;"></asp:TextBox></td>
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
                                                        <asp:TextBox ID="SelectionMotorControl_Muscle" runat="server" CssClass="span4" Text='<%#Eval("MUSCLE") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_Right" runat="server" CssClass="span4" Text='<%#Eval("RIGHT") %>'></asp:TextBox></td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_Left" runat="server" CssClass="span4" Text='<%#Eval("LEFT") %>'></asp:TextBox></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div style="display:none;">
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
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Gross" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Fine Motor :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Fine" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Communication :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Communication" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Cognition :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Denvers_Cognition" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>  
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        <b>GMFM score :</b></div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_GMFM" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
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
                                                        <asp:TextBox ID="SelectionMotorControl_Muscle" runat="server" CssClass="span6" Text='<%#Eval("MUSCLE") %>'></asp:TextBox>
                                                    </td>
                                                    <td><asp:TextBox ID="SelectionMotorControl_MAS" runat="server" CssClass="span6" Text='<%#Eval("MAS") %>'></asp:TextBox></td>                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="span11 formRow" style="display:none;">
                                <div class="row">
                                    <div class="span2">
                                        Observational Gait Analysis Scale :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="SelectionMotorControl_Observation" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div  runat="server" visible="false">
                            <div class="span11 formRow" >
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
                                        <asp:TextBox ID="TheFourA_Arousal" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Attention :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Attention" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Affect :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Affect" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        Action :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_Action" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                            <div class="span11 formRow" style="display:none;">
                                <div class="row" >
                                    <div class="span2">
                                        State Regulation :</div>
                                    <div class="span8">
                                        <asp:TextBox ID="TheFourA_StateRegulation" runat="server" CssClass="span8"></asp:TextBox>
                                    </div>
                                </div>
                            </div> 
                                </div>
                            <div class="clearfix">
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                   <ajaxToolkit:TabPanel ID="tb_Report6" runat="server" HeaderText="Morphology">
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
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLeft" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Morphology_LimbRight" CssClass="span2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox runat="server" ID="Morphology_LimbLength" CssClass="span4"></asp:TextBox>
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
                                        <asp:TextBox ID="Morphology_OralMotorFactors" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                   <ajaxToolkit:TabPanel ID="tb_Report7" runat="server" HeaderText="Musculoskeletal">
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
                                                            <tr>
                                                                <td>External Obliques
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExternalObliquesLeft" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_ExternalObliquesRight" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                      <div class="span12">
                                                <div class="span12">
                                                    <h5>Back Extensors </h5>
                                                </div>
                                                <div class="control-group">
                                                    <asp:TextBox ID="Musculoskeletal_Back_Extensors_cmt" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                                </div>
                                                <span class="char-limit-msg"></span>
                                            </div>
                                                     <div class="span12">
                                            <div class="span12">
                                               <h5>Rectus Abdominis</h5> 
                                            </div>
                                            <div class="control-group">
                                                <asp:TextBox ID="Musculoskeletal_Rectus_Abdominis_cmt" runat="server" CssClass="span10" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                            </div>
                                            <span class="char-limit-msg"></span>
                                        </div>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Tardieus">
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
                                                                    TA
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_Ta_Left" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_Ta_Right" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hamstrings
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_Hamstring_left" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_Hamstring_Right" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Adductors
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_adductors_left" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_adductors_right" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Hip Flexor Angle
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_hipFlexor_left" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_hipFlexor_Right" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Biceps
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_biceps_left" runat="server" CssClass="span3"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Musculoskeletal_Mmt_biceps_right" runat="server" CssClass="span3"></asp:TextBox>
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
                   <ajaxToolkit:TabPanel ID="tb_Report8" runat="server" HeaderText="Single systems">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="span12">
                                        <h5> 1)Regulatory System:</h5>
                                    </div>
                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">a) Self Regulation:</div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtSelfRegulation" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">b) Arousal:</div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtArousal" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">c) Attention:</div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtAttention" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">d) Affect:</div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtAffect" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">e) Action:</div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtAction" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>2) Cognition:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtCognition" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>3) GI:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtGI" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>4) Respiratory:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtRespiratory" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>5) Cardiovascular:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtCardiovascular" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>6) Skin and Integumentary:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtSkinIntegumentary" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2"><h5>7) Nutrition:</h5></div>
                                    <div class="span8">
                                        <asp:TextBox ID="txtNutrition" runat="server" CssClass="span8" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                            </div>


                        </div>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                   <ajaxToolkit:TabPanel ID="tb_Report9" runat="server" HeaderText="Sensory System">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">
                            <div class="formRow">
                                <div class="span12">
                                    <div class="control-label">
                                        1. Vision :
                                    </div>
                                    <div class="control-group">
                                        <asp:TextBox ID="SensorySystem_Vision" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Auditory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Propioceptive" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Oromotpor" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Vestibular" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Tactile" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                                        <asp:TextBox ID="SensorySystem_Olfactory" runat="server" CssClass="span10" TextMode="MultiLine"
                                            Rows="4"></asp:TextBox>
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
                   <ajaxToolkit:TabPanel ID="tb_Report10" runat="server" HeaderText="Ages and Stages ">
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
                                                    </table>                                                </ContentTemplate>
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
                                                        <b> Gross motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Problem solving</b>
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
                                                        <b> Personal social</b>
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
                                                        <b> Gross Motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Problem solving</b>
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
                                                        <b> Personal social</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                                                        <b> Fine motor</b>
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
                   <ajaxToolkit:TabPanel ID="tb_Report11" runat="server" HeaderText=" Ability Checklist">
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
                                                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.CategoryID")%>' Visible="false"></asp:Label>
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
                   <ajaxToolkit:TabPanel ID="tb_Report12" runat="server" HeaderText="Tests and Measures">
                    <ContentTemplate>
                        <div style="margin-top: 20px; margin-bottom: 20px;">

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                        GMFCS :
                                    </div>
                                    <div class="span8">
                                        <asp:CheckBox ID="GMFCSCheckBoxI" runat="server" CssClass="checkboes" Text=" I" />
                                        <asp:CheckBox ID="GMFCSCheckBoxII" runat="server" CssClass="checkboes" Text=" II" />
                                        <asp:CheckBox ID="GMFCSCheckBoxIII" runat="server" CssClass="checkboes" Text=" III" />
                                        <asp:CheckBox ID="GMFCSCheckBoxIV" runat="server" CssClass="checkboes" Text=" IV" />
                                        <asp:CheckBox ID="GMFCSCheckBoxV" runat="server" CssClass="checkboes" Text=" V" />
                                    </div>
                                </div>
                            </div>
                            <table class="ndt-default-table">
                                <tr>
                                    <td><b>Category</b></td>
                                    <td><b>GMFM</b></td>
                                    <%--<td><b>I</b></td>
                                    <td><b>II</b></td>
                                    <td><b>III </b></td>
                                    <td><b>IV</b></td>
                                    <td><b>V</b></td>--%>

                                </tr>
                                <tr>
                                    <td><b>Lying and Rolling</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_LyingRolling" runat="server" CssClass="span2" style="width: 932px;height: 83px;" TextMode="MultiLine" Rows="4" ></asp:TextBox>
                                    </td>
                                    <%--<td>
                                        <asp:CheckBox ID="chkI_LyingRolling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkII_LyingRolling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIII_LyingRolling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIV_LyingRolling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkV_LyingRolling" runat="server" CssClass="single-select" /></td>--%>
                                </tr>

                                <tr>
                                    <td><b>Sitting</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_Sitting" runat="server" CssClass="span2" style="width: 932px;height: 83px;"  TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                                  <%--  <td>
                                        <asp:CheckBox ID="chkI_Sitting" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkII_Sitting" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIII_Sitting" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIV_Sitting" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkV_Sitting" runat="server" CssClass="single-select" /></td>--%>
                                </tr>

                                <tr>
                                    <td><b>Kneeling & Crawling</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_KneelingCrawling" runat="server" CssClass="span2" style="width: 932px;height: 83px;"  TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                                    <%--<td>
                                        <asp:CheckBox ID="chkI_KneelingCrawling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkII_KneelingCrawling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIII_KneelingCrawling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIV_KneelingCrawling" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkV_KneelingCrawling" runat="server" CssClass="single-select" /></td>--%>
                                </tr>

                                <tr>
                                    <td><b>Standing</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_Standing" runat="server" CssClass="span2" style="width: 932px;height: 83px;" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                                  <%--  <td>
                                        <asp:CheckBox ID="chkI_Standing" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkII_Standing" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIII_Standing" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIV_Standing" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkV_Standing" runat="server" CssClass="single-select" /></td>--%>
                                </tr>

                                <tr>
                                    <td><b>Running & Jumping</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_RunningJumping" runat="server" CssClass="span2" style="width: 932px;height: 83px;"  TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                                    <%--<td>
                                        <asp:CheckBox ID="chkI_RunningJumping" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkII_RunningJumping" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIII_RunningJumping" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkIV_RunningJumping" runat="server" CssClass="single-select" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkV_RunningJumping" runat="server" CssClass="single-select" /></td>--%>
                                </tr>
                                  <tr>
                                    <td><b>Total Score</b></td>
                                    <td>
                                        <asp:TextBox ID="txtGmfm_TotalScore" runat="server" CssClass="span2" style="width: 932px;height: 83px;"  TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </td>
                              
                                </tr>
                                </table>

                            <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                       MACs:
                                    </div>
                                    <div class="span8">
                                        <asp:CheckBox ID="chkMACs_I" runat="server" CssClass="checkboes" Text=" I" />
                                        <asp:CheckBox ID="chkMACs_II" runat="server" CssClass="checkboes" Text=" II" />
                                        <asp:CheckBox ID="chkMACs_III" runat="server" CssClass="checkboes" Text=" III" />
                                        <asp:CheckBox ID="chkMACs_IV" runat="server" CssClass="checkboes" Text=" IV" />
                                        <asp:CheckBox ID="chkMACs_V" runat="server" CssClass="checkboes" Text=" V" />
                                    </div>
                                </div>
                            </div>
                         <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                       FMS:
                                    </div>
                                    <div class="span8">
                                        <asp:CheckBox ID="chkFMS_I" runat="server" CssClass="checkboes" Text=" I" />
                                        <asp:CheckBox ID="chkFMS_II" runat="server" CssClass="checkboes" Text=" II" />
                                        <asp:CheckBox ID="chkFMS_III" runat="server" CssClass="checkboes" Text=" III" />
                                        <asp:CheckBox ID="chkFMS_IV" runat="server" CssClass="checkboes" Text=" IV" />
                                        <asp:CheckBox ID="chkFMS_V" runat="server" CssClass="checkboes" Text=" V" />
                                    </div>
                                </div>
                            </div>
                          <div class="span11 formRow">
                                <div class="row">
                                    <div class="span2">
                                       Barry Albright Dystonia Scale:
                                    </div>
                                    <div class="span8" style="display:none;">
                                        <asp:CheckBox ID="chkBarry_I" runat="server" CssClass="checkboes" Text=" I" />
                                        <asp:CheckBox ID="chkBarry_II" runat="server" CssClass="checkboes" Text=" II" />
                                        <asp:CheckBox ID="chkBarry_III" runat="server" CssClass="checkboes" Text=" III" />
                                        <asp:CheckBox ID="chkBarry_IV" runat="server" CssClass="checkboes" Text=" IV" />
                                        <asp:CheckBox ID="chkBarry_V" runat="server" CssClass="checkboes" Text=" V" />
                                        <asp:CheckBox ID="chkBarry_VI" runat="server" CssClass="checkboes" Text=" VI" />
                                    </div>
                                     <asp:TextBox ID="Barry_albright_txt" runat="server" CssClass="span2" style="width: 932px;height: 83px;"  TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                            
                           
                        <script type="text/javascript">
                                        document.addEventListener('DOMContentLoaded', function () {
                                            // Loop through each group class
                                            ['TotalScore', 'MACs', 'FMS', 'Barry'].forEach(function (groupName) {
                                                let checkboxes = document.querySelectorAll('.group-' + groupName);
                                                checkboxes.forEach(function (chk) {
                                                    chk.addEventListener('change', function () {
                                                        if (chk.checked) {
                                                            checkboxes.forEach(function (otherChk) {
                                                                if (otherChk !== chk) otherChk.checked = false;
                                                            });
                                                        }
                                                    });
                                                });
                                            });
                                        });
                        </script>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                   <ajaxToolkit:TabPanel ID="tb_Report13" runat="server" HeaderText="Denvers">
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
                   <ajaxToolkit:TabPanel ID="tb_Report14" runat="server" HeaderText="Sensory Profile-2">
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
                                     <div class="clearfix">
                                                    </div>
                                </div>
                             
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>            
                   <ajaxToolkit:TabPanel ID="tb_Report15" runat="server" HeaderText="Evaluation ">
                      <ContentTemplate>
                          <div style="margin-top: 20px; margin-bottom: 20px;">
                              <div class="formRow">
                                  <div class="span12">
                                      <div class="control-label">
                                          1. Summary :
                                      </div>
                                      <div class="control-group">
                                          <asp:TextBox ID="Evaluation_Goal_Summary" runat="server" CssClass="span10" TextMode="MultiLine"
                                              Rows="4"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="clearfix">
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      2.Systems of impairments  :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evaluation_System_Impairment" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      3.LTG  :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evaluation_LTG" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      4.STG :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evaluation_STG" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <h5>Plan of care </h5>
                              <div class="span12">
                                  <div class="control-label">
                                      ●	Advice  :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evalution_Plan_advice" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      ●	Frequency and duration  :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evalution_Plan__Frequency" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      ●	Therapy adjuncts :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evalution_Plan_Adjuncts" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="span12">
                                  <div class="control-label">
                                      ●	Family education :
                                  </div>
                                  <div class="control-group">
                                      <asp:TextBox ID="Evalution_Plan__Education" runat="server" CssClass="span10" TextMode="MultiLine"
                                          Rows="4"></asp:TextBox>
                                  </div>
                              </div>
                              <div class="clearfix">
                              </div>
                          </div>

                        </div>
                            </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                   <ajaxToolkit:TabPanel ID="tb_Report16" runat="server" HeaderText="Doctor">
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
                                    <div class="clearfix">
                                                    </div>  
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
                                    <div class="clearfix">
                                                    </div>   
                                </div>
                                            
                            </div>
                             
                        </div>
                          
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                 </ajaxToolkit:TabContainer>
            </div>
                   <div class="clearfix"></div>
           
                <asp:Button ID="Button1" CssClass="buttonClass" runat="server" align="center" OnClick="Button1_Click" Font-Bold="True"  width="200px" style="margin-left: 30px;" Text="SAVE&NEXT" ClientIDMode="Static" />
            </div>
           
        </div>
    </div>

     <div class="modal fade" id="myModal" role="dialog"  style="max-width:400px; max-height:400px">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Modal Header</h4>
        </div>
        <div class="modal-body">
            <h5 class="modal-title">DATA IS SAVING PLEASE WAIT.. </h5>
            <%--<img src="../images/NewLoader.gif" />--%>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
        </div>
      </div>
      
    </div>
  </div>
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
                  preTabId = 'ctl00_ContentPlaceHolder1_tb_Contents_tb_Report1_tab';
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
                    window.location.href = '/Reports/Ndt_view_2025.aspx'; // Redirect to another page
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
    </script>
    </asp:Content>